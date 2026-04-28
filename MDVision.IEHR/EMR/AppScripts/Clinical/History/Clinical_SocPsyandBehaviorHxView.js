Clinical_SocPsyandBehaviorHxView = {
    bIsFirstLoad: true,
    params: [],
    overallComments: '',
    unremarkable: false,
    Load: function (params) {
        Clinical_SocPsyandBehaviorHxView.params = params;
        if (Clinical_SocPsyandBehaviorHxView.params.PanelID != 'pnlClinicalSocPsyandBehaviorHxView') {
            Clinical_SocPsyandBehaviorHxView.params.PanelID = Clinical_SocPsyandBehaviorHxView.params.PanelID + ' #pnlClinicalSocPsyandBehaviorHxView';
        } else {
            Clinical_SocPsyandBehaviorHxView.params.PanelID = 'pnlClinicalSocPsyandBehaviorHxView';
        }
        $.when(Clinical_SocPsyandBehaviorHxView.GetSocPsyandBehaviorQuestions()).then(function () {
            $.when(Clinical_SocPsyandBehaviorHxView.LoadAnswers()).then(function () {
                Clinical_SocPsyandBehaviorHxView.FillSocPsyandBehaviorHx();
            });
            $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #btnSocPsyandBehaviorHxSave").prop('disabled', true);
        });
    },


    LoadAnswers: function () {
        var dfd = $.Deferred();
        var count = 1;
        var def = [];
        $.each($("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " .scoQuestions"), function (i, item) {
            var QuestionId = $(this).find("p").attr("id");
            var self = $(this);
            self.find('.Answer').attr('ddlist', 'GetAnswers');
            self.find('.Answer').attr('id', 'Q' + QuestionId + '_A');
            self.find('.Answer').attr('name', 'Q' + QuestionId + '_A');
            self.find('.Answer').attr('QuestionId', QuestionId);
            var data = "IsActive=&ID=" + QuestionId;
            def.push(self.loadDropDownsWithTitle(true, data).done(function () {
            }));
            count++;
        });
        $.when.apply($, def).done(function ($n) {
            dfd.resolve();
        });
        return dfd;
    },
    GetSocPsyandBehaviorQuestions: function () {
        var dfd = $.Deferred();
        Clinical_SocPsyandBehaviorHxView.GetSocPsyandBehaviorQuestions_DBCALL().done(function (responseData) {
            responseData = JSON.parse(responseData);
            if (responseData.status != false) {
                if (responseData.QuestionCount > 0) {
                    $.each(responseData.Question_JSON, function (i, item) {
                        $("p[sequence='" + item.Sequence + "']").html(item.Question);
                        $("p[sequence='" + item.Sequence + "']").attr("id", item.QuestionnaireID);
                    });
                }
                dfd.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    GetSocPsyandBehaviorQuestions_DBCALL: function () {
        var objData = new Object();
        objData["commandType"] = "GetQuestions";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "History", "SocPsyandBehaviorHx");
    },
    unLoadTab: function () {
        if (Clinical_SocPsyandBehaviorHxView.params["FromAdmin"] == "0") {
            if (Clinical_SocPsyandBehaviorHxView.params != null && Clinical_SocPsyandBehaviorHxView.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_SocPsyandBehaviorHxView.params.ParentCtrl, 'Clinical_SocPsyandBehaviorHxView', null, Clinical_SocPsyandBehaviorHxView.params.UnPanelID);
            }
            else
                UnloadActionPan(null, 'Clinical_SocPsyandBehaviorHxView');
        }
        else {
            RemoveAdminTab();
        }
    },




    LoadQuestionAnswer: function (SocPsyQuestionAnswer_JSON) {
        var dfd = $.Deferred();
        $.each(SocPsyQuestionAnswer_JSON, function (i, item) {
            $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " select[id^='Q" + item.QuestionnaireID + "_']").attr("SocialandBehaviorQAId", item.SocialandBehaviorQAId);
            if (item.AnswerId != null) {
                $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " select[id^='Q" + item.QuestionnaireID + "_']").val(item.AnswerId);
                $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " select[id^='Q" + item.QuestionnaireID + "_']").attr("id", "Q" + item.QuestionnaireID + "_A" + item.AnswerId);
            }
        });
        dfd.resolve();
        return dfd;
    },

    FillSocPsyandBehaviorHx: function () {
        var dfd = $.Deferred();
        Clinical_SocPsyandBehaviorHxView.FillSocPsyandBehaviorHx_DBCALL().done(function (responseData) {
            responseData = JSON.parse(responseData);
            if (responseData.status != false) {
                if (responseData.SocPsyandBehaviorHxCount > 0) {
                    var detail = responseData.SocPsyandBehaviorHx_JSON;
                    $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #TxtPHQScore").val(detail.PHQScore);
                    $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #TxtTotalScore").val(detail.TotalScore);
                    $.when(Clinical_SocPsyandBehaviorHxView.LoadQuestionAnswer(responseData.SocPsyQuestionAnswer_JSON)).then(function () {
                        Clinical_SocPsyandBehaviorHxView.GetScore();
                    });
                }
                else {

                }
                dfd.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    FillSocPsyandBehaviorHx_DBCALL: function () {
        var objData = new Object();
        objData["commandType"] = "FillSocPsyandBehaviorHx";
        objData["SocialandBehaviorHxId"] = Clinical_SocPsyandBehaviorHxView.params.ID;
        objData["Current"] = Clinical_SocPsyandBehaviorHxView.params.Current;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "History", "SocPsyandBehaviorHx");
    },
    GetScore: function () {
        var AllAnswers = "";
        var PHQAnswers = "";
        var AlcoholAnswer = "";
        var SocConnAndIsolAnswer = "";
        var ExposToViol = "";
        $.each($("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " .Answer"), function (i, item) {
            if ($(item).val() != "") {
                if (AllAnswers != "") {
                    AllAnswers = AllAnswers + "," + $(item).val();
                }
                else {
                    AllAnswers = $(item).val();
                }
                // Set AnswerId
                var AnsId = $(item).attr("id").split('_');
                $(item).attr("id", AnsId[0] + '_A' + $(item).val());
            }
            else {
                // Set AnswerId
                var AnsId = $(item).attr("id").split('_');
                $(item).attr("id", AnsId[0] + '_A');
            }
        });
        $.each($("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " .PHQAnswer"), function (i, item) {
            if ($(item).val() != "") {
                if (PHQAnswers != "") {
                    PHQAnswers = PHQAnswers + "," + $(item).val();
                }
                else {
                    PHQAnswers = $(item).val();
                }
            }
        });
        $.each($("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " .AlcoholAnswer"), function (i, item) {
            if ($(item).val() != "") {
                if (AlcoholAnswer != "") {
                    AlcoholAnswer = AlcoholAnswer + "," + $(item).val();
                }
                else {
                    AlcoholAnswer = $(item).val();
                }
            }
        });
        $.each($("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " .SocConnAndIsolAnswer"), function (i, item) {
            if ($(item).val() != "") {
                if (SocConnAndIsolAnswer != "") {
                    SocConnAndIsolAnswer = SocConnAndIsolAnswer + "," + $(item).val();
                }
                else {
                    SocConnAndIsolAnswer = $(item).val();
                }
            }
        });
        $.each($("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " .ExposToViol"), function (i, item) {
            if ($(item).val() != "") {
                if (ExposToViol != "") {
                    ExposToViol = ExposToViol + "," + $(item).val();
                }
                else {
                    ExposToViol = $(item).val();
                }
            }
        });
        Clinical_SocPsyandBehaviorHx.GetTotalAndPHQScore(AllAnswers, PHQAnswers, AlcoholAnswer, SocConnAndIsolAnswer, ExposToViol).done(function (responseData) {
            responseData = JSON.parse(responseData);
            if (responseData.status != false) {
                if (responseData.ScoreCount > 0) {
                    $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #TxtPHQScore").val(responseData.Score_JSON[0].PHQScore != "0" ? responseData.Score_JSON[0].PHQScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #TxtTotalScore").val(responseData.Score_JSON[0].TotalScore != "0" ? responseData.Score_JSON[0].TotalScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #TxtAlcohalScore").val(responseData.Score_JSON[0].AlcoholScore != "0" ? responseData.Score_JSON[0].AlcoholScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #TxtSocialScore").val(responseData.Score_JSON[0].SocConnAndIsolScore != "0" ? responseData.Score_JSON[0].SocConnAndIsolScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHxView.params.PanelID + " #TxtExposureScore").val(responseData.Score_JSON[0].ExposToViolScore != "0" ? responseData.Score_JSON[0].ExposToViolScore : "");
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

}