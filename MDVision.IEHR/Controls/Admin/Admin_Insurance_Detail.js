insurDetail = {
    params: [],
    Load: function (params) {
        insurDetail.params = params;
        var self = null;
        if (insurDetail.params.PanelID == "tblinsurDetail")
            self = $('#tblinsurDetail');
        else
            self = $('#'+ insurDetail.params.PanelID +' #tblinsurDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#tblinsurDetail #divInsurance_Entity").css("display", "none");
            //    $("#tblinsurDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            $("#tblinsurDetail #txtWebsite").change(function () {
                if (this.value != '' && !/^(http|https):\/\//.test(this.value)) {
                    this.value = "http://" + this.value;
                }
            });

            insurDetail.LoadInsurance();
        });

    },

    LoadInsurance: function () {
        if (insurDetail.params.mode == "Add") {
            $('#insurDetail #txtShortName').attr("enabled", "enabled");
            $('#frmInsurDetail').data('serialize', $('#frmInsurDetail').serialize());
            insurDetail.ValidateInsurance();
        }
        else if (insurDetail.params.mode == "Edit") {
            $('#insurDetail #txtShortName').attr("disabled", "disabled");
            insurDetail.FillInsurance(insurDetail.params.InsuranceId).done(function (response) {
                if (response.status != false) {
                    var insurance_detail = JSON.parse(response.InsuranceFill_JSON);
                    var self = $("#insurDetail");
                    utility.bindMyJSON(true, insurance_detail, false, self);
                    if (insurance_detail.chkActive == 'True')
                        $("#insurDetail #chkActive").attr("checked", true);
                    else
                        $("#insurDetail #chkActive").attr("checked", false);
                    insurDetail.ValidateInsurance();
                    $('#frmInsurDetail').data('serialize', $('#frmInsurDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateInsurance: function () {
        $('#frmInsurDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ShortName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Description: {
                       group: '.col-sm-5',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Entity: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   'URL': {
                       group: '.col-sm-5',
                       validators: {
                           uri: {
                               message: 'URL not valid.'
                           }
                       }
                   },
                   'Email': {
                       group: '.col-sm-4',
                       validators: {
                           emailAddress: {
                               message: 'Email not Valid.'
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            insurDetail.InsuranceSave();
        });
    },

    InsuranceSave: function () {
        var strMessage = "";
        var self = $("#insurDetail");
        var myJSON = self.getMyJSON();
        if (insurDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Insurance", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    insurDetail.SaveInsurance(myJSON).done(function (response) {
                        if (response.status != false) {
                            if (insurDetail.params.ParentCtrl == "Admin_Insur") {
                                Admin_Insur.InsuranceSearch(response.InsuranceId);
                            }
                            utility.DisplayMessages(response.message, 1);
                            insurDetail.UnLoad();
                            CacheManager.BindCodes('GetInsurancePlan', true);
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
        else if (insurDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    insurDetail.UpdateInsurance(myJSON, insurDetail.params.InsuranceId).done(function (response) {
                        if (response.status != false) {
                            if (insurDetail.params.ParentCtrl == "Admin_Insur") {
                                Admin_Insur.InsuranceSearch(insurDetail.params.InsuranceId);
                            }
                            utility.DisplayMessages(response.message, 1);
                            insurDetail.UnLoad();
                            //UnloadActionPan();
                            CacheManager.BindCodes('GetInsurancePlan', true);
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

    SaveInsurance: function (InsuranceData) {
        var data = "InsuranceData=" + InsuranceData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_DETAIL", "SAVE_INSURANCE");
    },

    UpdateInsurance: function (InsuranceData, InsuranceID) {
        var data = "InsuranceData=" + InsuranceData + "&InsuranceID=" + InsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_DETAIL", "UPDATE_INSURANCE");
    },

    FillInsurance: function (InsuranceID) {
        var data = "InsuranceID=" + InsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_DETAIL", "FILL_INSURANCE");
    },

    UpdateInsuranceActiveInactive: function (InsuranceID, IsActive) {
        var data = "InsuranceID=" + InsuranceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_DETAIL", "UPDATE_INSURANCE_ACTIVE_INACTIVE");
    },

    UnLoad: function () {
        if (insurDetail.params != null && insurDetail.params.ParentCtrl != null) {
            UnloadActionPan(insurDetail.params.ParentCtrl, 'insurDetail', null, insurDetail.params.PanelID);
        }
        else
            UnloadActionPan(null, 'insurDetail');
    },

    ShowHistory: function () {
        var PanelID = 'insurDetail';
        var ParentCtrl = 'insurDetail';
        var ProfileName = 'Insurance';
        var DBTableName = 'Insurance';
        var ColumnKeyId = insurDetail.params.InsuranceId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}