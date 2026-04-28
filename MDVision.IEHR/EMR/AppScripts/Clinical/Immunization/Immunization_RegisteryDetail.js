Immunization_RegisteryDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Immunization_RegisteryDetail.params = params;

        if (Immunization_RegisteryDetail.params.PanelID)
            Immunization_RegisteryDetail.params.PanelID = 'pnlImmunization_Registery';
        else
            Immunization_RegisteryDetail.params.PanelID = 'pnlImmunization_Registery';
        if (Immunization_RegisteryDetail.bIsFirstLoad) {
            Immunization_RegisteryDetail.bIsFirstLoad = false;
            var self = $('#pnlImmunization_RegisteryrDetail');
            self.loadDropDowns(true).done(function () {
                Immunization_RegisteryDetail.ValidateRegisterDetails();
                Immunization_RegisteryDetail.LoadRegistery();
            });
        }
    },
    LoadRegistery: function () {
        $('#' + Immunization_RegisteryDetail.params.PanelID + ' #ddlSubmission').val($("#ddlSubmission option:eq(1)").val());
        Immunization_RegisteryDetail.DdlSubmissionType_Change($('#' + Immunization_RegisteryDetail.params.PanelID + ' #ddlSubmission'));
        $('#' + Immunization_RegisteryDetail.params.PanelID + ' #txtSendingApplication').val("MDvision");
        $('#' + Immunization_RegisteryDetail.params.PanelID + ' #txtSendingApplication').attr('disabled', 'disabled');

        if (Immunization_RegisteryDetail.params.mode == "Add") {
            $('#frmRegisteryDetail').data('serialize', $('#frmRegisteryDetail').serialize());
            Immunization_RegisteryDetail.ValidateRegisterDetails();
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #ddlSubmission').val($("#ddlSubmission option:eq(1)").val());
            Immunization_RegisteryDetail.DdlSubmissionType_Change($('#' + Immunization_RegisteryDetail.params.PanelID + ' #ddlSubmission'));
        }
        else if (Immunization_RegisteryDetail.params.mode == "Edit") {
            Immunization_RegisteryDetail.FillRegisteryDetail(Immunization_RegisteryDetail.params.RegisteryDetailId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var _detail = JSON.parse(response.RegisteryLoad_JSON);
                    _detail = $.grep(_detail, function (a) { return a.RegistryConfigurationId == Immunization_RegisteryDetail.params.RegisteryDetailId })[0];
                    if (_detail) {
                        var self = $("#frmRegisteryDetail");
                        utility.bindMyJSONByName(true, _detail, false, self).done(function () {
                            if (_detail.Status) {
                                Immunization_RegisteryDetail.DdlSubmissionType_Change($('#' + Immunization_RegisteryDetail.params.PanelID + ' #ddlSubmission'));
                                if (_detail.Status == "T")
                                    self.find("#rdobtnTesting").prop("checked", true);
                                else if (_detail.Status == "P")
                                    self.find("#rdobtnProduction").prop("checked", true);
                                else
                                    self.find("#rdobtnTesting").prop("checked", true);
                            }
                            else
                                self.find("#rdobtnTesting").prop("checked", true);
                            if (_detail.SubmissionName == "Queue" || _detail.RegistrySubmissionId == "1")
                                self.find("#txtTimeslot").val(_detail.Timeslot);
                            else if (_detail.SubmissionName == "Batch" || _detail.RegistrySubmissionId == "2")
                                self.find("#txtFilesPerBatch").val(_detail.FilesPerBatch);
                            $('#frmRegisteryDetail').data('serialize', $('#frmRegisteryDetail').serialize());
                            Immunization_RegisteryDetail.ValidateRegisterDetails();
                        });
                    }
                    else
                        utility.DisplayMessages("Record Not Found", 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }
    },


    ValidateRegisterDetails: function () {
        $('#frmRegisteryDetail').bootstrapValidator('destroy');
        $('#frmRegisteryDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    ProviderId: {
                        group: '.col-xs-12',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ReceivingApplicationId: {
                        group: '.col-xs-12',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    PoviderFacilityId: {
                        group: '.col-xs-12',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    
                    RegistrySubmissionId: {
                        group: '.col-xs-12',
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
            Immunization_RegisteryDetail.RegisteryDetailSave();
        });
    },

    validateSubmissionBatchValues: function (operationid) {
        $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divFilesPerBatch").find("i").remove();
        if (operationid == 1) {
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #txtFilesPerBatch").css("border-color", "#cc2724");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divFilesPerBatch").find(".control-label").css("color", "#cc2724");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divFilesPerBatch").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #txtFilesPerBatch").css("border-color", "#3c763d");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divFilesPerBatch").find(".control-label").css("color", "#3c763d");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divFilesPerBatch").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #txtFilesPerBatch").css("border-color", "#ccc");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divFilesPerBatch").find(".control-label").css("color", "#000000");
        }
    },
    validateSubmissionTimeValues: function (operationid) {
        $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divTimeSlot").find("i").remove();
        if (operationid == 1) {
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #txtTimeslot").css("border-color", "#cc2724");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divTimeSlot").find(".control-label").css("color", "#cc2724");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divTimeSlot").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #txtTimeslot").css("border-color", "#3c763d");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divTimeSlot").find(".control-label").css("color", "#3c763d");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divTimeSlot").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #txtTimeslot").css("border-color", "#ccc");
            $("#pnlImmunization_RegisteryrDetail #frmRegisteryDetail #divTimeSlot").find(".control-label").css("color", "#000000");
        }
    },
    RegisteryDetailSave: function () {
        var strMessage = "";
        var self = $("#frmRegisteryDetail");
        var myJSON = self.getMyJSONByName();

        var SubmissionId = $('#' + Immunization_RegisteryDetail.params.PanelID + ' #ddlSubmission').val();
        if (SubmissionId == "1"){
            if (self.find("#txtTimeslot").val() == "") {
                Immunization_RegisteryDetail.validateSubmissionTimeValues(1);
                return false;
            }
            else
                Immunization_RegisteryDetail.validateSubmissionTimeValues(2);
        }
        else if (SubmissionId == "2") {
            if (self.find("#txtFilesPerBatch").val() == "") {
                Immunization_RegisteryDetail.validateSubmissionBatchValues(1);
                return false;
            }
            else
                Immunization_RegisteryDetail.validateSubmissionBatchValues(2);
        }
        else {
            return false;
        }
        if (Immunization_RegisteryDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Registery", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_RegisteryDetail.SaveRegisteryDetail(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Immunization_RegisteryDetail.params.RegisteryDetailId = response.RegisteryId;
                            utility.DisplayMessages(response.message, 1);
                            Immunization_RegisteryDetail.UnLoadTab();
                            Immunization_Registery.SearchRegisteryConfiguration();
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
        else if (Immunization_RegisteryDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Registery", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_RegisteryDetail.UpdateRegisteryDetail(myJSON, Immunization_RegisteryDetail.params.RegisteryDetailId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Immunization_RegisteryDetail.UnLoadTab();
                            Immunization_Registery.SearchRegisteryConfiguration();
                            utility.DisplayMessages(response.message, 1);
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
    maxLengthCheck: function (event) {
        var t = event.target;
        if (t.hasAttribute('maxlength'))
            t.value = t.value.slice(0, t.getAttribute('maxlength'));
    },

    DdlSubmissionType_Change: function (obj) {
        var currval = $(obj).find("option:selected").text();
        if (currval && currval.toLowerCase() == "batch") {
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #divFilesPerBatch').show();
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #txtFilesPerBatch').val("");
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #divTimeSlot').hide();
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #txtTimeslot').val("");
        }
        else if (currval && currval.toLowerCase() == "queue") {
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #divFilesPerBatch').hide();
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #divTimeSlot').show();
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #txtFilesPerBatch').val("");
            $('#' + Immunization_RegisteryDetail.params.PanelID + ' #txtTimeslot').val("");
        }
    },
    FillRegisteryDetail: function (RegisteryDetailId) {
        var objData = new Object();
        objData["commandType"] = "SEARCH_IMMUNIZATION_REGISTERY";
        objData["RegistryConfigurationId"] = RegisteryDetailId;
        objData["IsActive"] = Immunization_RegisteryDetail.params.Active;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "ImmunizationRegistery");
    },
    SaveRegisteryDetail: function (datatosave, RegisteryDetailId) {
        var objData = JSON.parse(datatosave);
        RegisteryDetailId ? objData["RegistryConfigurationId"] = RegisteryDetailId : objData["RegistryConfigurationId"] = 0;
        objData["commandType"] = "SAVE_IMMUNIZATION_REGISTERY";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "ImmunizationRegistery");
    },
    UpdateRegisteryDetail: function (datatosave, RegisteryDetailId) {
        var objData = JSON.parse(datatosave);
        RegisteryDetailId ? objData["RegistryConfigurationId"] = RegisteryDetailId : objData["RegistryConfigurationId"] = 0;
        objData["commandType"] = "UPDATE_IMMUNIZATION_REGISTERY";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "ImmunizationRegistery");
    },

    UnLoadTab: function () {

        if (Immunization_RegisteryDetail.params["FromAdmin"] == "0") {
            if (Immunization_RegisteryDetail.params != null && Immunization_RegisteryDetail.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_RegisteryDetail.params.ParentCtrl, 'Immunization_RegisteryDetail');
            }
            else
                UnloadActionPan(null, 'Immunization_RegisteryDetail');
        }
        else {
            RemoveAdminTab();
        }
    },

}