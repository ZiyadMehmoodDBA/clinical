EDIServiceHandleDetail = {
    params: [],
    Load: function (params) {
        EDIServiceHandleDetail.params = params;
        $('#divInterval').hide();
        var self = null;
        if (EDIServiceHandleDetail.params.PanelID == "EDIServiceHandleDetail")
            self = $('#EDIServiceHandleDetail');
        else
            self = $('#' + EDIServiceHandleDetail.params.PanelID + ' #EDIServiceHandleDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {

            EDIServiceHandleDetail.LoadEDIServiceHandle();

        });

    },

    UnLoad: function () {

        utility.UnLoadDialog("frmEDIServiceHandleDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ModeChanged: function () {
        var txtMode = $('#ddlInterval :selected').text();
        var txtModeTrimed = $('#ddlInterval :selected').text().replace(/ /g, '');

        if (txtModeTrimed === 'Interval') {
            $('#divInterval').show();
        }
        else {
            $('#divInterval').hide();
        }

    },

    LoadEDIServiceHandle: function () {
        if (EDIServiceHandleDetail.params.mode == "Add") {
            $('#EDIServiceHandleDetail #txtShortName').attr("enabled", "enabled");

            $('input#txtClearingHoueUserPassword').removeAttr("type");
            $('input#txtClearingHoueUserPassword').prop('type', 'password');

              if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    $("#EDIServiceHandleDetail #frmEDIServiceHandleDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                    }

            EDIServiceHandleDetail.ValidateEDIServiceHandle();

            //Serialize data
            setTimeout(function () {
                        $('#frmEDIServiceHandleDetail').data('serialize', $('#frmEDIServiceHandleDetail').serialize());
                    }, "100");

        }
        else if (EDIServiceHandleDetail.params.mode == "Edit") {
            EDIServiceHandleDetail.FillEDIServiceHandle(EDIServiceHandleDetail.params.EDIServiceHandleID).done(function (response) {
                if (response.status != false) {


                    var EDIServiceHandle_detail = JSON.parse(response.EDIServiceHandleFill_JSON);
                    var self = $("#EDIServiceHandleDetail");

                    if (EDIServiceHandle_detail.ddlInterval == 'Interval')
                        $('#divInterval').show();
                    else
                        $('#divInterval').hide();

                    utility.bindMyJSON(true, EDIServiceHandle_detail, false, self).done(function () {

                        EDIServiceHandleDetail.ValidateEDIServiceHandle();
                        $('#frmEDIServiceHandleDetail').data('serialize', $('#frmEDIServiceHandleDetail').serialize());

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateEDIServiceHandle: function () {
        $('#frmEDIServiceHandleDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    ClearingHouse: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Entity: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Case: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Mode: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Time: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },

                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            EDIServiceHandleDetail.EDIServiceHandleSave();
        });
    },

    EDIServiceHandleSave: function () {
        var strMessage = "";
        var self = $("#EDIServiceHandleDetail");
        var myJSON = self.getMyJSON();
        if (EDIServiceHandleDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("EDI Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIServiceHandleDetail.SaveEDIServiceHandle(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_EDIServiceHandle.EDIServiceHandleSearch(response.EDIServiceHandleID);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetEDIServiceHandle', true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (EDIServiceHandleDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("EDI Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIServiceHandleDetail.UpdateEDIServiceHandle(myJSON, EDIServiceHandleDetail.params.EDIServiceHandleID).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_EDIServiceHandle.EDIServiceHandleSearch(EDIServiceHandleDetail.params.EDIServiceHandleID);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetEDIServiceHandle', true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    SaveEDIServiceHandle: function (EDIServiceHandleData) {
        var data = "EDIServiceHandleData=" + EDIServiceHandleData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SERVICE_HANDLE", "SAVE_EDI_SERVICE_HANDLE");
    },

    UpdateEDIServiceHandle: function (EDIServiceHandleData, EDIServiceHandleID) {
        var data = "EDIServiceHandleData=" + EDIServiceHandleData + "&EDIServiceHandleID=" + EDIServiceHandleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SERVICE_HANDLE", "UPDATE_EDI_SERVICE_HANDLE");
    },

    FillEDIServiceHandle: function (EDIServiceHandleID) {
        var data = "EDIServiceHandleID=" + EDIServiceHandleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SERVICE_HANDLE", "FILL_EDI_SERVICE_HANDLE");
    },

    UpdateEDIServiceHandleActiveInactive: function (EDIServiceHandleID, IsActive) {
        var data = "EDIServiceHandleID=" + EDIServiceHandleID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SERVICE_HANDLE", "UPDATE_EDI_SERVICE_HANDLE_ACTIVE_INACTIVE");
    },

    ResetTimeDropDown: function () {
        var from = $('#EDIServiceHandleDetail #txtTime').val();

        var fromRes = from.substring(6, 8);

        if (fromRes == 'AM') {
            $("#EDIServiceHandleDetail #time1").val("AM");
        }
        else if (fromRes == 'PM') {
            $("#EDIServiceHandleDetail #time1").val("PM");
        }
        else {
            $("#EDIServiceHandleDetail #time1").val("ALL");
        }
    },

    ShowHistory: function () {
        var PanelID = 'EDIServiceHandleDetail';
        var ParentCtrl = 'EDIServiceHandleDetail';
        var ProfileName = 'EDI Service';
        var DBTableName = 'EDIServiceHandle';
        var ColumnKeyId = EDIServiceHandleDetail.params.EDIServiceHandleID;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}