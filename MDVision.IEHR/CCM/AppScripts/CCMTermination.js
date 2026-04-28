
CCMTermination = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCMTermination.params = params;

        if (CCMTermination.params.PanelID != 'pnlCCMTermination') {
            CCMTermination.params.PanelID = CCMTermination.params.PanelID + ' #pnlCCMTermination';
        } else {
            CCMTermination.params.PanelID = 'pnlCCMTermination';
        }

        var IsTerminate = "";
        if (CCMTermination.params.mode == "Add" || CCMTermination.params.IsTerminate == null || CCMTermination.params.IsTerminate == "" || CCMTermination.params.IsTerminate == "-1") {
            IsTerminate = "-1";
        }
        else if (CCMTermination.params.mode == "Edit") {
            IsTerminate = CCMTermination.params.IsTerminate;
            //CCMTermination.FamilyHxEdit(FamilyHxId);
        }

        var self = $('#' + CCMTermination.params.PanelID);

        self.loadDropDowns(true).done(function () {

            $("#txtTerminationReason").bind('paste', function (e) {
                var elem = $(this);
                setTimeout(function () {
                    var text = elem.val();
                    CCMTermination.CountTerminationReasonChar(text);
                }, 100);
            });

        });
    },

    CountTerminationReasonChar: function () {

        var terminationReason = $("#txtTerminationReason").val();
        if (terminationReason)
        {
            var len = terminationReason.length;
            if (len > 100) {
                $("#txtTerminationReason").val(terminationReason.substring(0, 100));
            } else {
                $('#charNum').text(100 - len);
            }
        }
    },

    SaveTerminationReason: function ()
    {
        var self = $("#" + CCMTermination.params["PanelID"]);
        var strMessage = "";
        var myJSON = self.getMyJSONByName();
            //fixme add priviliges
            //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CCMTermination.CCMTerminationSave(myJSON).done(function (response) {
                //CCMEnrollmentInfo.FillCCMEnrollmentInfo(CCMTermination.params.EnrollmentInfoId, 0, 15).done(function (response) {
                    if (response.status != false) {
                        

                        if ($("#lblCCMterminateProgram").text() == "Resume Program")
                        {
                            $("#lblCCMterminateProgram").text("Terminate Program");
                            $("#chkCCMterminateProgram").prop('checked', false);
                            response.Message = "Successfully Resumed";
                            $("#ccmTerminated").addClass('hidden');
                            setPatientBanner(CCMTermination.params.PatientId, "1", "");

                            $("#tabCCM_ button").removeClass('disableAll');
                            $(".ccmControls").removeClass('disableAll');
                            $("#PatientHubPrint").removeClass('disableAll');


                            $("#divHRA").removeClass('disableAll');
                            $("#divProblems").removeClass('disableAll');
                            $("#divCarePlan").removeClass('disableAll');
                            $("#divProgramUpdates").removeClass('disableAll');

                        }
                        else {
                            $("#lblCCMterminateProgram").text("Resume Program");
                            $("#chkCCMterminateProgram").prop('checked', false);
                            response.Message = "Successfully Terminated";
                            $("#ccmTerminated").removeClass('hidden');
                            setPatientBanner(CCMTermination.params.PatientId, "1", $("#txtTerminationReason").val());
                            $("#ccmTerminated").attr("data-original-title", $("#txtTerminationReason").val());

                            $("#tabCCM_ button").addClass('disableAll');
                            $(".ccmControls").addClass('disableAll');
                            $("#PatientHubPrint").addClass('disableAll');

                            $("#divHRA").addClass('disableAll');
                            $("#divProblems").addClass('disableAll');
                            $("#divCarePlan").addClass('disableAll');
                            $("#divProgramUpdates").addClass('disableAll');

                        }
                        utility.DisplayMessages(response.Message, 1);

                        if (CCMTermination.params != null && CCMTermination.params.ParentCtrl != null) {
                            UnloadActionPan(CCMTermination.params.ParentCtrl, 'CCMTermination');
                        }
                        else
                            UnloadActionPan(null, 'CCMTermination');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            // });
    },

    CCMTerminationSave: function (CCMTerminationData) {

        var objData = JSON.parse(CCMTerminationData);
        if ($("#lblCCMterminateProgram").text() == "Resume Program")
        {
            CCMTermination.params.Terminated = "Accepted";
        }
        objData["Status"] = CCMTermination.params.Terminated;
        objData["Reason"] = $("#txtTerminationReason").val();
        objData["EnrollmentInfoId"] = CCMTermination.params.EnrollmentInfoId;
        objData["PatientId"] = CCMTermination.params.PatientId;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMEnrollmentInfo", "TerminationCCMEnrollmentInfo");
    },

    CCMTerminationSave_: function (EnrollmentInfoId, PatientId, Status) {

        var objData = new Object();
        if ($("#lblCCMterminateProgram").text() == "Resume Program") {
            CCMTermination.params.Terminated = "Accepted";
        }
        objData["Status"] = Status;
        objData["Reason"] = ""; //$("#txtTerminationReason").val();
        objData["EnrollmentInfoId"] = EnrollmentInfoId;
        objData["PatientId"] = PatientId;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMEnrollmentInfo", "TerminationCCMEnrollmentInfo");
    },



    UnLoad: function () {

        $("#chkCCMterminateProgram").prop('checked', false);
        var objDeffered = $.Deferred();
        UnloadActionPan(CCMTermination.params.ParentCtrl, 'CCMTermination');
        objDeffered.resolve();
        return objDeffered;
    },
}