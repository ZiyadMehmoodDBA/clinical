specialtyDetail = {
    params: [],
    Load: function (params) {
        specialtyDetail.params = params;
        var self = null;
        if (specialtyDetail.params.PanelID != 'specialtyDetail')
            self = $('#' + specialtyDetail.params.PanelID + ' #specialtyDetail')
        else
            self = $('#specialtyDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find('#ddlEntity').attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {

            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#specialtyDetail #divSpecialty_Entity").css("display", "none");
            //    $("#specialtyDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find('#ddlEntity').val(globalAppdata["SeletedEntityId"]);
            }
            specialtyDetail.LoadSpecialty();
            $('#frmSpecialtyDetail').data('serialize', $('#frmSpecialtyDetail').serialize());

        });

    },

    LoadSpecialty: function () {
        if (specialtyDetail.params.mode == "Add") {
            $('#specialtyDetail #txtShortName').attr("enabled", "enabled");

            specialtyDetail.ValidateSpecialty();
        }
        else if (specialtyDetail.params.mode == "Edit") {
            $('#specialtyDetail #txtShortName').attr("disabled", "disabled");
            specialtyDetail.FillSpecialty(specialtyDetail.params.SpecialtyId).done(function (response) {
                if (response.status != false) {
                    var specialty_detail = JSON.parse(response.SpecialtyFill_JSON);
                    var self = $("#specialtyDetail");
                    utility.bindMyJSON(true, specialty_detail, false, self);
                    if (specialty_detail.chkActive == 'True')
                        $("#specialtyDetail #chkActive").attr("checked", true);
                    else
                        $("#specialtyDetail #chkActive").attr("checked", false);
                    specialtyDetail.ValidateSpecialty();
                    $('#frmSpecialtyDetail').data('serialize', $('#frmSpecialtyDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    SpecialtySave: function () {
        var strMessage = "";
        var self = $("#specialtyDetail");
        var myJSON = self.getMyJSON();
        if (specialtyDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Specialty", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    specialtyDetail.SaveSpecialty(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_Specialty.SpecialtySearch(response.SpecialtyId);
                            utility.DisplayMessages(response.message, 1);
                            specialtyDetail.UnloadPan();
                            CacheManager.BindCodes('GetSpecialty', true);
                            if (specialtyDetail.params["IsFromReferrals"] == true)
                                CacheManager.BindCodes('GetSpecialtiesAllEntitiesReferals', true);
                        }
                        else {
                            if (response.Message.indexOf('duplicate key') > -1) {
                                utility.DisplayMessages("Specialty already exists for this Entity", 3);
                            } else
                                utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (specialtyDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Specialty", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    specialtyDetail.UpdateSpecialty(myJSON, specialtyDetail.params.SpecialtyId).done(function (response) {
                        if (response.status != false) {
                            Admin_Specialty.SpecialtySearch(specialtyDetail.params.SpecialtyId);
                            utility.DisplayMessages(response.message, 1);
                            specialtyDetail.UnloadPan();
                            CacheManager.BindCodes('GetSpecialty', true);
                            if (specialtyDetail.params["IsFromReferrals"] == true)
                                CacheManager.BindCodes('GetSpecialtiesAllEntitiesReferals', true);
                        }
                        else {
                            if (response.Message.indexOf('duplicate key') > -1) {
                                utility.DisplayMessages("Specialty already exists for this Entity", 3);
                            } else
                                utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    ValidateSpecialty: function () {
        $('#frmSpecialtyDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   shortname: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   entity: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            specialtyDetail.SpecialtySave();
        });
    },

    SaveSpecialty: function (SpecialtyData) {
        var data = "SpecialtyData=" + SpecialtyData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SPECIALTY_DETAIL", "SAVE_SPECIALTY");
    },

    UpdateSpecialty: function (SpecialtyData, SpecialtyID) {
        var data = "SpecialtyData=" + SpecialtyData + "&SpecialtyID=" + SpecialtyID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SPECIALTY_DETAIL", "UPDATE_SPECIALTY");
    },

    FillSpecialty: function (SpecialtyID) {
        var data = "SpecialtyID=" + SpecialtyID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SPECIALTY_DETAIL", "FILL_SPECIALTY");
    },

    UpdateSpecialtyActiveInactive: function (SpecialtyID, IsActive) {
        var data = "SpecialtyID=" + SpecialtyID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SPECIALTY_DETAIL", "UPDATE_SPECIALTY_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmSpecialtyDetail", function () {
            specialtyDetail.UnloadPan();
        }, function () {
            specialtyDetail.UnloadPan();
        });
    },

    UnloadPan: function () {

        if (specialtyDetail.params != null && specialtyDetail.params.ParentCtrl != null) {
            UnloadActionPan(specialtyDetail.params.ParentCtrl, 'specialtyDetail');
        }
        else
            UnloadActionPan();
    },

    ShowHistory: function () {
        var PanelID = 'specialtyDetail';
        var ParentCtrl = 'specialtyDetail';
        var ProfileName = 'Specialty';
        var DBTableName = 'Specialty';
        var ColumnKeyId = specialtyDetail.params.SpecialtyId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}