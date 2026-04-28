schoolDetail = {
    params: [],
    Load: function (params) {
        schoolDetail.params = params;
        schoolDetail.LoadSchool();
    },

    LoadSchool: function () {
        AppPrivileges.GetFormPrivileges("School", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (schoolDetail.params.mode == "Add") {
                    $('#frmSchoolDetail').data('serialize', $('#frmSchoolDetail').serialize());
                    schoolDetail.ValidateSchool();
                }
                else if (schoolDetail.params.mode == "Edit") {
                    schoolDetail.FillSchool(schoolDetail.params.SchoolId).done(function (response) {
                        if (response.status != false) {
                            var school_detail = JSON.parse(response.SchoolFill_JSON);
                            var self = $("#schoolDetail");
                            utility.bindMyJSON(true, school_detail, false, self);
                            if (school_detail.chkActive == 'True')
                                $("#schoolDetail #chkActive").attr("checked", true);
                            else
                                $("#schoolDetail #chkActive").attr("checked", false);
                            schoolDetail.ValidateSchool();
                            $('#frmSchoolDetail').data('serialize', $('#frmSchoolDetail').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    SchoolSave: function () {
        var strMessage = "";
        var self = $("#schoolDetail");
        var myJSON = self.getMyJSON();
        if (schoolDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("School", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    schoolDetail.SaveSchool(myJSON).done(function (response) {
                        if (response.status != false) {

                            if (schoolDetail.params.ParentCtrl != "Patient_Preferences") {
                            Patient_School.SchoolSearch(response.SchoolId);
                            }
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetSchool', true);
                            UnloadActionPan(schoolDetail.params["ParentCtrl"]);
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
        else if (schoolDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("School", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    schoolDetail.UpdateSchool(myJSON, schoolDetail.params.SchoolId).done(function (response) {
                        if (response.status != false) {
                            if (schoolDetail.params.ParentCtrl != "Patient_Preferences") {
                            Patient_School.SchoolSearch(schoolDetail.params.SchoolId);
                            }
                            Patient_Preferences.LoadPreferences();
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(schoolDetail.params["ParentCtrl"]);
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

    ValidateSchool: function () {
        $('#frmSchoolDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   SchoolName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Email: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           regexp: {
                               regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                               message: 'Email not Valid'
                           }

                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            schoolDetail.SchoolSave();
        }).on('keyup', 'input#txtEmail', function (e) {
            var formValidation = $('#' + schoolDetail.params.PanelID + ' #frmSchoolDetail').data("bootstrapValidator");
            switch ($(this).attr("name")) {
                case 'Email':
                    var email = $("input#txtEmail").val();
                    if (email != "") {
                        formValidation.enableFieldValidators('Email', true);
                    }
                    else
                        formValidation.enableFieldValidators('Email', false);
                    break;
            }
        });
    },

    SaveSchool: function (SchoolData) {
        var data = "SchoolData=" + SchoolData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_SCHOOL_DETAIL", "SAVE_SCHOOL");
    },

    UpdateSchool: function (SchoolData, SchoolID) {
        var data = "SchoolData=" + SchoolData + "&SchoolID=" + SchoolID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_SCHOOL_DETAIL", "UPDATE_SCHOOL");
    },

    FillSchool: function (SchoolID) {
        var data = "SchoolID=" + SchoolID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_SCHOOL_DETAIL", "FILL_SCHOOL");
    },

    UpdateSchoolActiveInactive: function (SchoolID, IsActive) {
        var data = "SchoolID=" + SchoolID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_SCHOOL_DETAIL", "UPDATE_SCHOOL_ACTIVE_INACTIVE");
    },

    //UnLoad: function () {
    //    if ($('#frmSchoolDetail').serialize() != $('#frmSchoolDetail').data('serialize')) {
    //        utility.myConfirm('2', function () {
    //            UnloadActionPan(schoolDetail.params["ParentCtrl"]);
    //        }, function () { },
    //                '2'
    //            );
    //    }
    //    else {
    //        UnloadActionPan(schoolDetail.params["ParentCtrl"]);
    //    }
    //},

    UnLoad: function () {

        utility.UnLoadDialog('frmSchoolDetail', function () {
            UnloadActionPan(schoolDetail.params["ParentCtrl"], "schoolDetail");
        }, function () {
            UnloadActionPan(schoolDetail.params["ParentCtrl"], "schoolDetail");
        });

        
    },
}