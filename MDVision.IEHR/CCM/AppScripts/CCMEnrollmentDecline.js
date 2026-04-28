CCMEnrollmentDecline = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCMEnrollmentDecline.params = params;
        if (CCMEnrollmentDecline.params != null && CCMEnrollmentDecline.params.PanelID != "pnlCCMEnrollmentDecline") {
            CCMEnrollmentDecline.params["PanelID"] = CCMEnrollmentDecline.params["PanelID"] + ' #pnlCCMEnrollmentDecline';
        }
        else {
            CCMEnrollmentDecline.params["PanelID"] = "pnlCCMEnrollmentDecline";
        }

        if (CCMEnrollmentDecline.bIsFirstLoad) {
            CCMEnrollmentDecline.bIsFirstLoad = false;
            CCMEnrollmentDecline.ValidateCCMEnrollementDecline()
        }

    },
    ValidateCCMEnrollementDecline: function () {
        $('#frmCCMEnrollmentDecline')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {

                    DeclineReason: {
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
            CCMEnrollmentDecline.saveCCMEnrollmentDecline();
        });
    },


    saveCCMEnrollmentDecline: function () {

        var self = $("#" + CCMEnrollmentDecline.params["PanelID"]);

        var strMessage = "";

        var myJSON = self.getMyJSONByName();
        if (CCMEnrollmentDecline.params.mode.toLowerCase() == "add") {
            //fixme add priviliges
            //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CCMEnrollmentDecline.CCMEnrollmentDeclineSave(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);

                        

                        if (CCMEnrollmentDecline.params != null && CCMEnrollmentDecline.params.ParentCtrl != null) {
                            UnloadActionPan(CCMEnrollmentDecline.params.ParentCtrl, 'CCMEnrollmentDecline');
                        }
                        else
                            UnloadActionPan(null, 'CCMEnrollmentDecline');

                        //if (CCMEnrollmentInfo.params["ParentCtrl"] == "mstrTabDashBoard") {
                        DashBoard.DashBoardCCMEnrollmentInfoSearch(1, 1000, null, null)
                        setPatientBanner(CCMEnrollmentDecline.params.PatientId, "1");
                        //}
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);

                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            // });
        }
        else if (CCMEnrollmentDecline.params.mode.toLowerCase() == "edit") {
            //   AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CCMEnrollmentDecline.CCMEnrollmentDeclineUpdate(myJSON).done(function (response) {
                    if (response.status != false) {

                        utility.DisplayMessages(response.Message, 1);
                        UnloadActionPan(CCMEnrollmentDecline.params["ParentCtrl"]);
                        //if (CCMEnrollmentInfo.params["ParentCtrl"] == "mstrTabDashBoard") {
                        DashBoard.DashBoardCCMEnrollmentInfoSearch(1, 1000, null, null)
                        setPatientBanner(CCMEnrollmentDecline.params.PatientId, "1");
                        //}
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            //  });
        }
    },


    CCMEnrollmentDeclineSave: function (CCMEnrollmentData) {
        var objData = JSON.parse(CCMEnrollmentData);

        objData["ConsentFileStream"] = "";// CCMEnrollmentInfo.ConsentFileStream;
        objData["PatientId"] = CCMEnrollmentDecline.params.PatientId;
        objData["StatusId"] = "3";

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMEnrollmentInfo", "SaveCCMEnrollmentDecline");
    },



    calculateRemainingCharacters: function () {


        var self = $("#" + CCMEnrollmentDecline.params["PanelID"]);

        self.find("#spnRemainingCharacters").text(" " + Number(self.find("#txtDeclineReason").attr("maxlength")) - self.find("#txtDeclineReason").val().length);

    },

    UnLoad: function () {

        if (CCMEnrollmentDecline.params != null && CCMEnrollmentDecline.params.ParentCtrl != null && CCMEnrollmentDecline.params.PanelID != 'pnlCCMEnrollmentDecline') {
            UnloadActionPan(CCMEnrollmentDecline.params.ParentCtrl, 'pnlCCMEnrollmentDecline', null, CCMEnrollmentDecline.params.PanelID);
        }

        else if (CCMEnrollmentDecline.params != null && CCMEnrollmentDecline.params.ParentCtrl != null) {
            UnloadActionPan(CCMEnrollmentDecline.params.ParentCtrl, 'CCMEnrollmentDecline');
        }

    },
}