clearingHouseDetail = {
    params: [],
    Load: function (params) {
        clearingHouseDetail.params = params;
        var self = null;
        if (clearingHouseDetail.params.PanelID == "clearingHouseDetail")
            self = $('#clearingHouseDetail');
        else
            self = $('#'+clearingHouseDetail.params.PanelID+' #clearingHouseDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {            
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#tblclearingHouseDetail #divClearingHouse_Entity").css("display", "none");
            //    $("#tblclearingHouseDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }

            
            clearingHouseDetail.LoadClearingHouse();
        });
        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            $("#clearingHouseDetail #FTPHostKeydiv").addClass('hidden');
        }
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmClearingHouseDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    LoadClearingHouse: function () {
        if (clearingHouseDetail.params.mode == "Add") {
            $('#clearingHouseDetail #txtShortName').attr("enabled", "enabled");
            clearingHouseDetail.ValidateClearingHouse();

            //Serialize data
            setTimeout(function () {
                $('#frmClearingHouseDetail').data('serialize', $('#frmClearingHouseDetail').serialize());
            },"100");
            
        }
        else if (clearingHouseDetail.params.mode == "Edit") {
            $('#clearingHouseDetail #txtShortName').attr("disabled", "disabled");
            clearingHouseDetail.FillClearingHouse(clearingHouseDetail.params.ClearingHouseId).done(function (response) {
                if (response.status != false) {
                    var clearingHouse_detail = JSON.parse(response.ClearingHouseFill_JSON);
                    var self = $("#clearingHouseDetail");
                    utility.bindMyJSON(true, clearingHouse_detail, false, self).done(function () {

                        if (clearingHouse_detail.chkClaimSubmitAllowed == 'True')
                            $("#clearingHouseDetail #chkClaimSubmitAllowed").attr("checked", true);
                        else
                            $("#clearingHouseDetail #chkClaimSubmitAllowed").attr("checked", false);
                        if (clearingHouse_detail.chkEligibilityAllowed == 'True')
                            $("#clearingHouseDetail #chkEligibilityAllowed").attr("checked", true);
                        else
                            $("#clearingHouseDetail #chkEligibilityAllowed").attr("checked", false);
                        if (clearingHouse_detail.chkClaimStatusAllowed == 'True')
                            $("#clearingHouseDetail #chkClaimStatusAllowed").attr("checked", true);
                        else
                            $("#clearingHouseDetail #chkClaimStatusAllowed").attr("checked", false);
                        if (clearingHouse_detail.chkElectronicEOBAllowed == 'True')
                            $("#clearingHouseDetail #chkElectronicEOBAllowed").attr("checked", true);
                        else
                            $("#clearingHouseDetail #chkElectronicEOBAllowed").attr("checked", false);
                        if (clearingHouse_detail.chkSecondaryAllowed == 'True')
                            $("#clearingHouseDetail #chkSecondaryAllowed").attr("checked", true);
                        else
                            $("#clearingHouseDetail #chkSecondaryAllowed").attr("checked", false);

                        clearingHouseDetail.ValidateClearingHouse();
                        //serialize data
                        $('#frmClearingHouseDetail').data('serialize', $('#frmClearingHouseDetail').serialize());


                    });

                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateClearingHouse: function () {
        $('#frmClearingHouseDetail')
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
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Type: {
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
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            clearingHouseDetail.ClearingHouseSave();
        });
    },

    ClearingHouseSave: function () {
        var strMessage = "";
        var self = $("#clearingHouseDetail");
        var myJSON = self.getMyJSON();
        if (clearingHouseDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Clearinghouse", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    clearingHouseDetail.SaveClearingHouse(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_ClearingHouse.ClearingHouseSearch(response.ClearingHouseId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetClearingHouse', true);
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
        else if (clearingHouseDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Clearinghouse", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    clearingHouseDetail.UpdateClearingHouse(myJSON, clearingHouseDetail.params.ClearingHouseId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_ClearingHouse.ClearingHouseSearch(clearingHouseDetail.params.ClearingHouseId);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetClearingHouse', true);
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

    SaveClearingHouse: function (ClearingHouseData) {
        var data = "ClearingHouseData=" + ClearingHouseData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CLEARING_HOUSE_DETAIL", "SAVE_CLEARING_HOUSE");
    },

    UpdateClearingHouse: function (ClearingHouseData, ClearingHouseID) {
        var data = "ClearingHouseData=" + ClearingHouseData + "&ClearingHouseID=" + ClearingHouseID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CLEARING_HOUSE_DETAIL", "UPDATE_CLEARING_HOUSE");
    },

    FillClearingHouse: function (ClearingHouseID) {
        var data = "ClearingHouseID=" + ClearingHouseID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CLEARING_HOUSE_DETAIL", "FILL_CLEARING_HOUSE");
    },

    UpdateClearingHouseActiveInactive: function (ClearingHouseID, IsActive) {
        var data = "ClearingHouseID=" + ClearingHouseID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CLEARING_HOUSE_DETAIL", "UPDATE_CLEARING_HOUSE_ACTIVE_INACTIVE");
    },
    ShowHistory: function () {
        var PanelID = 'clearingHouseDetail';
        var ParentCtrl = 'clearingHouseDetail';
        var ProfileName = 'Clearinghouse';
        var DBTableName = 'ClearingHouse';
        var ColumnKeyId = clearingHouseDetail.params.ClearingHouseId;
        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}