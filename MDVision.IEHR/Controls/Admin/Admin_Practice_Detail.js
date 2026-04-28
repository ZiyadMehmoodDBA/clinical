practiceDetail = {
    params: [],
    AddressEnalbed: false,
    Load: function (params) {
        practiceDetail.params = params;

        //initialization of date-pickers.
        utility.CreateDatePicker('tblpracticeDetail #dtpStartDate', function () {
            //on-change callback method 
        });

        var self = null;
        if (practiceDetail.params.PanelID == 'practiceDetail')
            self = $('#practiceDetail');
        else
            self = $('#' + practiceDetail.params.PanelID + ' #practiceDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find('#ddlEntity').attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find('#ddlEntity').val(globalAppdata["SeletedEntityId"]);
                self.find('#DivScan').addClass('hidden');
                self.find('#DivOCR').addClass('hidden');
                
            }
            $('#practiceDetail #chkOCR').prop("disabled", true);
            practiceDetail.LoadPractice();
            $("#tblpracticeDetail #txtWebSite").change(function () {
                if ((this.value).match(/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/))
                {
                    if (this.value != '' && !/^(http|https):\/\//.test(this.value)) {
                        this.value = "http://" + this.value;
                    }
                }
            });
        });
    },
    enabledisableOCR: function(obj){
        if ($(obj).prop("checked") == true) {
            $('#practiceDetail #chkOCR').prop("disabled", false);
        } else {
            $('#practiceDetail #chkOCR').prop("disabled", true);
            $('#practiceDetail #chkOCR').prop("checked", false);
        }
    },

    enabledisableScan: function(obj){
        if ($(obj).prop("checked") == true) {
            $('#practiceDetail #chkScan').prop("disabled", true);
        } else {
            $('#practiceDetail #chkScan').prop("disabled", false);
        }
    },
    LoadPractice: function () {
        if (practiceDetail.params.mode == "Add") {
            $('#practiceDetail #txtShortName').attr("enabled", "enabled")

            practiceDetail.ValidatePractice();
            practiceDetail.PayAddress($("#practiceDetail #chkIsPayToAddress"));
            //Serialize data
            $('#frmPracticeDetail').data('serialize', $('#frmPracticeDetail').serialize());
        }
        else if (practiceDetail.params.mode == "Edit") {
            $('#practiceDetail #txtShortName').attr("disabled", "disabled")
            practiceDetail.FillPractice(practiceDetail.params.PracticeId).done(function (response) {
                if (response.status != false) {
                    var medication_detail = JSON.parse(response.PracticeFill_JSON);
                    var self = $("#practiceDetail");
                    utility.bindMyJSON(true, medication_detail, false, self).done(function () {

                        //practiceDetail.LoadEntityBasedData(medication_detail.ddlEntity, medication_detail.ddlFeeGroup, medication_detail.ddlBasicFeeGroup);
                        practiceDetail.ValidatePractice();
                        if ($('#practiceDetail #chkScan').prop("checked") == true) {
                            $('#practiceDetail #chkOCR').prop("disabled", false);
                        }
                        //set is to pay validation
                        practiceDetail.PayAddress($("#practiceDetail #chkIsPayToAddress"));

                        //Serialize data
                        $('#frmPracticeDetail').data('serialize', $('#frmPracticeDetail').serialize());

                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    LoadEntityBasedData: function (entityID, ddlFeeGroup, ddlBasicFeeGroup) {
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#tblpracticeDetail #ddlFeeGroup', 'GetFeeGroup', false, entityID);
            CacheManager.BindDropDownsByEntityID('#tblpracticeDetail #ddlBasicFeeGroup', 'GetBasicFeeGroup', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#tblpracticeDetail #ddlFeeGroup', 'GetFeeGroup', true, null);
            CacheManager.BindDropDownsByEntityID('#tblpracticeDetail #ddlBasicFeeGroup', 'GetBasicFeeGroup', true, null);
        }

        $('#practiceDetail #ddlFeeGroup').val(ddlFeeGroup);
        $('#practiceDetail #ddlBasicFeeGroup').val(ddlBasicFeeGroup);

        //CacheManager.BindCodes('GetBasicFeeGroup', true);

        //practiceDetail.LoadBasicFeeGroup(entityID);
        //practiceDetail.LoadFeeGroup(entityID);
    },

    LoadBasicFeeGroup: function (entityID) {
        // Loads Entity Based Basic Fee Group
        practiceDetail.FillBasicFeeGroup(entityID).done(function (response) {
            if (response.status != false) {
                var basicfeegroup_detail = JSON.parse(response.BasicFeeGroupLoad_JSON);
                $("#tblpracticeDetail #ddlBasicFeeGroup").empty();
                $("#tblpracticeDetail #ddlBasicFeeGroup").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(basicfeegroup_detail, function (i, item) {
                    $("#tblpracticeDetail #ddlBasicFeeGroup").append(
                        $('<option/>', {
                            value: item.BasicFeeGroupId,
                            html: item.EntityName + " - " + item.ShortName
                        })
                    );
                });
            }

        });
    },

    LoadFeeGroup: function (entityID) {
        // Loads Entity Based Basic Fee Group
        practiceDetail.FillFeeGroup(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.FeeGroupLoad_JSON);
                $("#tblpracticeDetail #ddlFeeGroup").empty();
                $("#tblpracticeDetail #ddlFeeGroup").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#tblpracticeDetail #ddlFeeGroup").append(
                        $('<option/>', {
                            value: item.FeeGroupId,
                            html: item.EntityName + " - " + item.ShortName
                        })
                    );
                });
            }

        });
    },

    //CallCityState: function (control, field1, field2) {
    //    var zipcode = $('#practiceDetail ' + control).val();
    //    var cityname = null;
    //    var statename = null;
    //    practiceDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
    //        if (response.status != false) {
    //            var citystate = JSON.parse(response.CITYSTATE_JSON);
    //            $('#practiceDetail ' + field1).val(citystate.txtCity);
    //            $('#practiceDetail ' + field2).val(citystate.txtState);
    //            //practiceDetail.ValidatePractice();
    //        }
    //        else {
    //            $('#practiceDetail ' + field1).val('');
    //            $('#practiceDetail ' + field2).val('');
    //        }
    //    });
    //},

    PayAddress: function (obj) {

        var bootstrapValidator = $('#frmPracticeDetail').data('bootstrapValidator');

        if ($(obj).is(":checked")) {
            //make fields required.
            practiceDetail.AddressEnalbed = true;
            $("#frmPracticeDetail #pay_to_address").removeClass('disableAll');
            $("#frmPracticeDetail #pay_to_address span").css('display', 'inline-block');
        }
        else {
            //not required.
            practiceDetail.AddressEnalbed = false;
            $("#frmPracticeDetail #pay_to_address").addClass('disableAll');
            $("#frmPracticeDetail #pay_to_address span").css('display', 'none');
        }

        bootstrapValidator.enableFieldValidators('PayZip', practiceDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayState', practiceDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayCity', practiceDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayAddress2', practiceDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayAddress1', practiceDetail.AddressEnalbed);

    },

    ValidatePractice: function () {
        $('#frmPracticeDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    shortName: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    descripition: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },

                    entity: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    EIN: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    NPI: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    address: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    city: {
                        group: '.size60per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    state: {
                        group: '.size40per',
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                            regexp: {
                                regexp: /^[a-zA-Z]+$/,
                                message: ' '
                            }
                        }
                    },
                    zip: {
                        group: '.size60per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    'email': {
                        group: '.col-sm-3',
                        enabled: false,
                        validators: {
                            regexp: {
                                regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                                message: 'Email not Valid'
                            }
                        }
                    },
                    'Website': {
                        group: '.col-sm-3',
                        enabled: false,
                        validators: {
                            uri: {
                                message: 'Format not Valid'
                            }
                        }
                    },
                    PayAddress1: {
                        group: '.col-sm-3',
                        enabled: practiceDetail.AddressEnalbed,
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    PayAddress2: {
                        group: '.col-sm-3',
                        enabled: practiceDetail.AddressEnalbed,
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    PayCity: {
                        group: '.col-sm-3',
                        enabled: practiceDetail.AddressEnalbed,
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    PayState: {
                        group: '.col-sm-3',
                        enabled: practiceDetail.AddressEnalbed,
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    PayZip: {
                        group: '.col-sm-3',
                        enabled: practiceDetail.AddressEnalbed,
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
            practiceDetail.PracticeSave();
        });
    },

    EnableValidation: function (obj) {

        var objprovider = $('#practiceDetail #frmPracticeDetail');
        var formValidation = objprovider.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        } else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }

    },

    PracticeSave: function () {
        var strMessage = "";
        var self = $("#practiceDetail");
        var myJSON = self.getMyJSON();
        if (practiceDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Practice", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    practiceDetail.SavePractice(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_Practice.PracticeSearch(response.PracticeId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(practiceDetail.params["ParentCtrl"]);
                            CacheManager.BindCodes('GetPractice', true);
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
        else if (practiceDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Practice", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    practiceDetail.UpdatePractice(myJSON, practiceDetail.params.PracticeId, 1).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            if (practiceDetail.params.RefCtrl != null) {
                                UnloadActionPan(practiceDetail.params["ParentCtrl"], "practiceDetail");
                            }
                            else {
                                Admin_Practice.PracticeSearch(practiceDetail.params.PracticeId);
                                UnloadActionPan(practiceDetail.params["ParentCtrl"], "practiceDetail");
                            }
                            CacheManager.BindCodes('GetPractice', true);
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

    SavePractice: function (PracticeData) {
        var data = "PracticeData=" + PracticeData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "SAVE_PRACTICE");
    },

    UpdatePractice: function (PracticeData, practiceID, IsActive) {
        var data = "PracticeData=" + PracticeData + "&PracticeID=" + practiceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "UPDATE_PRACTICE");
    },

    FillPractice: function (PracticeID) {
        var data = "PracticeID=" + PracticeID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "FILL_PRACTICE");
    },
    DemographicPractice: function (PracticeID) {
        var data = "PracticeID=" + PracticeID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "DEMOGRAPHIC_PRACTICE");
    },

    FillBasicFeeGroup: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "FILL_BASIC_FEE_GROUP");
    },

    FillFeeGroup: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "FILL_FEE_GROUP");
    },

    UpdatePracticeActiveInactive: function (PracticeID, IsActive) {
        var data = "PracticeID=" + PracticeID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "UPDATE_PRACTICE_ACTIVE_INACTIVE");
    },

    //FillCityState: function (zipcode, cityname, statename) {
    //    var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    //},

    //UnLoad: function () {
    //    if ($('#frmPracticeDetail').serialize() != $('#frmPracticeDetail').data('serialize')) {
    //        utility.myConfirm('2', function () {
    //            UnloadActionPan();
    //        }, function () { },
    //                '2'
    //            );
    //    }
    //    else {
    //        UnloadActionPan();
    //    }
    //},
    UnLoad: function () {

        if (practiceDetail.params != null && practiceDetail.params.ParentCtrl != null && practiceDetail.params.PanelID != 'tblpracticeDetail') {
            UnloadActionPan(practiceDetail.params.ParentCtrl, 'practiceDetail', null, practiceDetail.params.PanelID);
        } else {
            utility.UnLoadDialog("frmPracticeDetail", function () {
                UnloadActionPan(practiceDetail.params["ParentCtrl"], "practiceDetail");
            }, function () {
                UnloadActionPan(practiceDetail.params["ParentCtrl"], "practiceDetail");
            });
        }
    },

    ShowHistory: function () {
        var PanelID = 'practiceDetail';
        var ParentCtrl = 'practiceDetail';
        var ProfileName = 'Practice';
        var DBTableName = 'Practice';
        var ColumnKeyId = practiceDetail.params.PracticeId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}