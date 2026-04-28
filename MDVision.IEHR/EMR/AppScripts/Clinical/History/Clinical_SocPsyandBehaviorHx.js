Clinical_SocPsyandBehaviorHx = {
    bIsFirstLoad: true,
    params: [],
    SocPsyandBehaviorJSON: '',
    overallComments: '',
    unremarkable: false,
    date: '',
    Load: function (params) {
        Clinical_SocPsyandBehaviorHx.params = params;
        //if (Clinical_SocPsyandBehaviorHx.params.mode == null) {
        Clinical_SocPsyandBehaviorHx.params.mode = "Add";
        //}
        if (Clinical_SocPsyandBehaviorHx.params.PanelID != 'pnlClinicalSocPsyandBehaviorHx') {
            Clinical_SocPsyandBehaviorHx.params.PanelID = Clinical_SocPsyandBehaviorHx.params.PanelID + ' #pnlClinicalSocPsyandBehaviorHx';
        } else {
            Clinical_SocPsyandBehaviorHx.params.PanelID = 'pnlClinicalSocPsyandBehaviorHx';
        }
        if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocPsyandBehaviorHx.params.PanelID.indexOf("pnlClinicalProgressNote") < 0) {
            Clinical_SocPsyandBehaviorHx.params.PanelID = "pnlClinicalProgressNote #" + Clinical_SocPsyandBehaviorHx.params.PanelID;
        }
        Clinical_SocPsyandBehaviorHx.ResetFormData();
        if ($('#PatientProfile #hfPatientId').val() != "") {
            Clinical_SocPsyandBehaviorHx.params.PatientId = $('#PatientProfile #hfPatientId').val();
        }
        utility.CreateDatePicker(Clinical_SocPsyandBehaviorHx.params.PanelID + '  #dpSocialBehaviorDate', function () {
        }, true);
        Clinical_SocPsyandBehaviorHx.toggleReadyFunction();
        if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorJSON = '';
            $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + ' #btnSocPsyandBehaviorHxSave').addClass("hidden");
        }
        $.when(Clinical_SocPsyandBehaviorHx.GetSocPsyandBehaviorQuestions()).then(function () {
            $.when(Clinical_SocPsyandBehaviorHx.LoadAnswers()).then(function () {
                Clinical_SocPsyandBehaviorHx.FillSocPsyandBehaviorHx();
            });

        });
        Clinical_SocPsyandBehaviorHx.SearchSocPsyandBehaviorHx();
    },

    ResetFormData: function () {
        var details = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx");
        $(details).resetAllControls(null);
        $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx #QuestionAnswerSection").removeClass('disableAll');        
    },
    toggleReadyFunction: function () {
        $(function () {
            (function ($) {
                'use strict';
                $(function () {
                    $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);

        });


    },
    LoadAnswers: function () {
        var dfd = $.Deferred();
        var count = 1;
        var def = [];
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .scoQuestions"), function (i, item) {
            var QuestionId = $(this).find("p").attr("id");
            var self = $(this);
            self.find('.Answer').attr('ddlist', 'GetAnswers');
            self.find('.Answer').attr('id', 'Q' + QuestionId + '_A');
            self.find('.Answer').attr('name', 'Q' + QuestionId + '_A');
            self.find('.Answer').attr('QuestionId', QuestionId);
            var data = "IsActive=&ID=" + QuestionId;
            var sequence = $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + QuestionId).attr("sequence");
            def.push(self.loadDropDownsWithTitle(true, data, Clinical_SocPsyandBehaviorHx.params.PanelID + ' #Question_Seq' + sequence).done(function () {
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
        Clinical_SocPsyandBehaviorHx.GetSocPsyandBehaviorQuestions_DBCALL().done(function (responseData) {
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
        if (Clinical_SocPsyandBehaviorHx.params["FromAdmin"] == "0") {
            if (Clinical_SocPsyandBehaviorHx.params != null && Clinical_SocPsyandBehaviorHx.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_SocPsyandBehaviorHx.params.ParentCtrl, 'unLoadTab');
            }
            else
                UnloadActionPan(null, 'Clinical_SocPsyandBehaviorHx');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_socPsyandBehaviorHx").remove();
            RemoveAdminTab();
        }
        //EMRUtility.scrollToPNcomponent('unLoadTab');
    },
    GetScore: function () {
        var AllAnswers = "";
        var PHQAnswers = "";
        var AlcoholAnswer = "";
        var SocConnAndIsolAnswer = "";
        var ExposToViol = "";
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .Answer"), function (i, item) {
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
        //if (AllAnswers == "") {
        //    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #btnSocPsyandBehaviorHxSave").prop('disabled', true);
        //}
        //else {
        //    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #btnSocPsyandBehaviorHxSave").prop('disabled', false);
        //}
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .PHQAnswer"), function (i, item) {
            if ($(item).val() != "") {
                if (PHQAnswers != "") {
                    PHQAnswers = PHQAnswers + "," + $(item).val();
                }
                else {
                    PHQAnswers = $(item).val();
                }
            }
        });
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .AlcoholAnswer"), function (i, item) {
            if ($(item).val() != "") {
                if (AlcoholAnswer != "") {
                    AlcoholAnswer = AlcoholAnswer + "," + $(item).val();
                }
                else {
                    AlcoholAnswer = $(item).val();
                }
            }
        });
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .SocConnAndIsolAnswer"), function (i, item) {
            if ($(item).val() != "") {
                if (SocConnAndIsolAnswer != "") {
                    SocConnAndIsolAnswer = SocConnAndIsolAnswer + "," + $(item).val();
                }
                else {
                    SocConnAndIsolAnswer = $(item).val();
                }
            }
        });
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .ExposToViol"), function (i, item) {
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
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #TxtPHQScore").val(responseData.Score_JSON[0].PHQScore != "0" ? responseData.Score_JSON[0].PHQScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #TxtTotalScore").val(responseData.Score_JSON[0].TotalScore != "0" ? responseData.Score_JSON[0].TotalScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #TxtAlcohalScore").val(responseData.Score_JSON[0].AlcoholScore != "0" ? responseData.Score_JSON[0].AlcoholScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #TxtSocialScore").val(responseData.Score_JSON[0].SocConnAndIsolScore != "0" ? responseData.Score_JSON[0].SocConnAndIsolScore : "");
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #TxtExposureScore").val(responseData.Score_JSON[0].ExposToViolScore != "0" ? responseData.Score_JSON[0].ExposToViolScore : "");
                }
            }
            else {
                utility.DisplayMessages(responseData.Message, 3);
            }
        });
    },
    GetTotalAndPHQScore: function (AllAnswerIds, PHQAnswerIds, AlcoholAnswerIds, SocConnAndIsolAnswerIds, ExposToViolIds) {
        var objData = new Object();
        objData["commandType"] = "GetTotalAndPHQScore";
        objData["AllAnswerIds"] = AllAnswerIds;
        objData["PHQAnswerIds"] = PHQAnswerIds;
        objData["AlcoholAnswerIds"] = AlcoholAnswerIds;
        objData["SocConnAndIsolAnswerIds"] = SocConnAndIsolAnswerIds;
        objData["ExposToViolIds"] = ExposToViolIds;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "History", "SocPsyandBehaviorHx");
    },

    GetArrayofQuestionAnswer: function () {

        var QuestionAnswerArray = [];
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .Answer"), function (i, item) {
            if ($(item).attr("id") != "") {
                var QuesAnsId = $(item).attr("id").split("_");
                if (QuesAnsId.length == 2) {
                    var obj = {};
                    obj.QuestionnaireID = QuesAnsId[0].replace("Q", "");
                    obj.AnswerId = (QuesAnsId[1].replace("A", "") != "" && QuesAnsId[1].replace("A", "") != "null") ? (QuesAnsId[1].replace("A", "")) : "-1";
                    if (Clinical_SocPsyandBehaviorHx.params.mode == "Edit") {
                        obj.SocialandBehaviorQAId = $(item).attr("socialandbehaviorqaid");
                        obj.SocialandBehaviorHxId = $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #SocialandBehaviorHxId").val();
                    }
                    else {
                        obj.SocialandBehaviorHxId = -1;
                    }
                    QuestionAnswerArray.push(obj);
                }
            }
        });
        return QuestionAnswerArray;
    },

    saveSocPsyandBehaviorHx: function () {
        var self = $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx");
        var myJSON = self.getMyJSONByName();
        var QuestionAnswerArray = Clinical_SocPsyandBehaviorHx.GetArrayofQuestionAnswer();

        if (Clinical_SocPsyandBehaviorHx.params.mode == "Add") {
            Clinical_SocPsyandBehaviorHx.SaveSocPsyandBehaviorHx_DBCall(myJSON, QuestionAnswerArray).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_SocPsyandBehaviorHx.params.mode = "Edit";
                    Clinical_SocPsyandBehaviorHx.LoadQuestionAnswer(response.SocPsyQuestionAnswer_JSON);
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #SocialandBehaviorHxId").val(response.SocPsyandBehaviorHxId);
                    Clinical_SocPsyandBehaviorHx.SearchSocPsyandBehaviorHx();
                    if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_SocPsyandBehaviorHx.cacheSocPsyandBehaviorHxJSON();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            Clinical_SocPsyandBehaviorHx.UpdateSocPsyandBehaviorHx_DBCall(myJSON, QuestionAnswerArray).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_SocPsyandBehaviorHx.SearchSocPsyandBehaviorHx();
                    if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_SocPsyandBehaviorHx.cacheSocPsyandBehaviorHxJSON();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    LoadQuestionAnswer: function (SocPsyQuestionAnswer_JSON) {
        var dfd = $.Deferred();
        $.each(SocPsyQuestionAnswer_JSON, function (i, item) {
            $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " select[id^='Q" + item.QuestionnaireID + "_']").attr("SocialandBehaviorQAId", item.SocialandBehaviorQAId);
            if (item.AnswerId != null) {
                $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " select[id^='Q" + item.QuestionnaireID + "_']").val(item.AnswerId == "" || item.AnswerId == "-1" || item.AnswerId == "null" ? "" : item.AnswerId);
                $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " select[id^='Q" + item.QuestionnaireID + "_']").attr("id", "Q" + item.QuestionnaireID + "_A" + (item.AnswerId == "" || item.AnswerId == "null" ? -1 : item.AnswerId));
            }
        });
        dfd.resolve();
        return dfd;
    },
    SaveSocPsyandBehaviorHx_DBCall: function (myJSON, QuestionAnswerArray) {
        var objData = JSON.parse(myJSON);
        objData["commandType"] = "SocPsyandBehaviorHxSave";
        objData["QuestionAnswerArray"] = QuestionAnswerArray;
        //if()
        objData["SocialandBehaviorHxId"] = -1;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "History", "SocPsyandBehaviorHx");
    },
    UpdateSocPsyandBehaviorHx_DBCall: function (myJSON, QuestionAnswerArray) {
        var objData = JSON.parse(myJSON);
        objData["commandType"] = "SocPsyandBehaviorHxUpdate";
        objData["QuestionAnswerArray"] = QuestionAnswerArray;
        objData["SocialandBehaviorHxId"] = $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #SocialandBehaviorHxId").val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "History", "SocPsyandBehaviorHx");
    },
    FillSocPsyandBehaviorHx: function () {
        var dfd = $.Deferred();
        Clinical_SocPsyandBehaviorHx.FillSocPsyandBehaviorHx_DBCALL().done(function (responseData) {
            responseData = JSON.parse(responseData);
            if (responseData.status != false) {
                if (responseData.SocPsyandBehaviorHxCount > 0) {
                    //$("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #btnSocPsyandBehaviorHxSave").prop('disabled', false);
                    var self = $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx");

                    var detail = '';
                    var SocPsyQuestionAnswer_JSON = [];
                    var SocPsyQuestionAnswer_JSON1 = [];
                    if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx != null) {
                        detail = Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx;
                        SocPsyQuestionAnswer_JSON = Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx.QuestionAnswerArray;
                    }
                    else {
                        detail = responseData.SocPsyandBehaviorHx_JSON;
                        SocPsyQuestionAnswer_JSON = responseData.SocPsyQuestionAnswer_JSON;
                    }

                    var Unremarkable = detail.Unremarkable;


                    Clinical_SocPsyandBehaviorHx.date = detail.SocialBehaviorDate || '';
                    Clinical_SocPsyandBehaviorHx.unremarkable = detail.Unremarkable;
                    Clinical_SocPsyandBehaviorHx.overallComments = detail.Comments || '';


                    utility.bindMyJSONByName(true, detail, false, self).done(function () {
                        var dateFormat = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + ' #dpSocialBehaviorDate').val();
                        var Day = new Date(dateFormat);
                        var dateFormat = $.datepicker.formatDate('mm/dd/yy', new Date(dateFormat));
                        $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + ' #dpSocialBehaviorDate').datepicker('setDate', dateFormat);
                        Clinical_SocPsyandBehaviorHx.params.mode = "Edit";
                        if (Unremarkable != null && Unremarkable.toString().toLowerCase() == "false") {
                            $.when(Clinical_SocPsyandBehaviorHx.LoadQuestionAnswer(SocPsyQuestionAnswer_JSON)).then(function () {
                                Clinical_SocPsyandBehaviorHx.SetJSON();
                                Clinical_SocPsyandBehaviorHx.GetScore();
                            });
                        }
                        else {
                            if (!$('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").hasClass('disableAll')) {
                                $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").addClass('disableAll')
                            }
                            Clinical_SocPsyandBehaviorHx.SetJSON();
                        }
                        
                    });
                }
                else {
                    //$("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #btnSocPsyandBehaviorHxSave").prop('disabled', true);
                    if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx != null) {
                        var self = $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx");

                        var detail = Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx;
                        var SocPsyQuestionAnswer_JSON = Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx.QuestionAnswerArray;

                        var Unremarkable = detail.Unremarkable;


                        Clinical_SocPsyandBehaviorHx.date = detail.SocialBehaviorDate || '';
                        Clinical_SocPsyandBehaviorHx.unremarkable = detail.Unremarkable;
                        Clinical_SocPsyandBehaviorHx.overallComments = detail.Comments || '';


                        utility.bindMyJSONByName(true, detail, false, self).done(function () {
                            var dateFormat = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + ' #dpSocialBehaviorDate').val();
                            var Day = new Date(dateFormat);
                            var dateFormat = $.datepicker.formatDate(globalAppdata.DateFormat, new Date(dateFormat));
                            $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + ' #dpSocialBehaviorDate').datepicker('setDate', dateFormat);
                            Clinical_SocPsyandBehaviorHx.params.mode = "Edit";
                            if (Unremarkable != null && Unremarkable.toString().toLowerCase() == "false") {
                                $.when(Clinical_SocPsyandBehaviorHx.LoadQuestionAnswer(SocPsyQuestionAnswer_JSON)).then(function () {
                                    Clinical_SocPsyandBehaviorHx.SetJSON();
                                    Clinical_SocPsyandBehaviorHx.GetScore();
                                });
                            }
                            else {
                                if (!$('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").hasClass('disableAll')) {
                                    $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").addClass('disableAll')
                                }
                                Clinical_SocPsyandBehaviorHx.SetJSON();
                            }

                        });
                    }
                    else {
                        Clinical_SocPsyandBehaviorHx.SetJSON();
                    }
                }
                dfd.resolve();
            }
            else {
                //$("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #btnSocPsyandBehaviorHxSave").prop('disabled', true);
                utility.DisplayMessages(responseData.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    SetJSON: function () {
        if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorJSON = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx").getMyJSONByName();
        }
        else {
            Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorJSON = '';
        }
    },
    FillSocPsyandBehaviorHx_DBCALL: function () {
        var objData = new Object();
        objData["commandType"] = "FillSocPsyandBehaviorHx";
        objData["PatientId"] = Clinical_SocPsyandBehaviorHx.params.PatientId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "History", "SocPsyandBehaviorHx");
    },
    SearchSocPsyandBehaviorHx: function (SocPsyandBehaviorHxId, PageNo, rpp) {
        var dfd = $.Deferred();
        Clinical_SocPsyandBehaviorHx.SearchSocPsyandBehaviorHx_DBCALL(SocPsyandBehaviorHxId, PageNo, rpp).done(function (responseData) {
            responseData = JSON.parse(responseData);
            if (responseData.status != false) {
                if (responseData.SocPsyandBehaviorHxCount > 0) {
                    Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorHxGridLoad(responseData);
                    var TableControl = Clinical_SocPsyandBehaviorHx.params.PanelID + " #dgvSocPsyandBehaviorHx";
                    var PagingPanelControlID = Clinical_SocPsyandBehaviorHx.params.PanelID + " #dgvSocPsyandBehaviorHx_Paging";
                    var ClassControlName = "Clinical_SocPsyandBehaviorHx";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = responseData.iTotalDisplayRecords;
                    setTimeout(CreatePagination(responseData.SocPsyandBehaviorHxCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (SocPsyandBehaviorHxId, PageNo, rpp) {
                        Clinical_SocPsyandBehaviorHx.SearchSocPsyandBehaviorHx(SocPsyandBehaviorHxId, PageNo, rpp);
                    }), 10);
                    if (responseData.SocPsyandBehaviorHx_JSON[0].Current == "1") {
                        $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #dgvSocPsyandBehaviorHx_Paging").addClass("hidden");
                    }
                }
                else {
                    var GridPnlId = "pnlSocPsyandBehaviorHx_Result";
                    var GriddgvId = "dgvSocPsyandBehaviorHx";

                    if ($.fn.dataTable.isDataTable("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId)) {
                        $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).dataTable().fnClearTable();
                        $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).dataTable().fnDestroy();
                        $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId + " tbody").find("tr").remove();
                    } else {
                        //for stop make dublicate Datatables
                        $.each($.fn.DataTable.fnTables(), function () {
                            if (this.id == 'dgvSocPsyandBehaviorHx') {
                                $(this).dataTable().fnClearTable();
                                $(this).dataTable().fnDestroy();
                                $(this).find("tbody tr").remove();
                                $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId + " tbody").find("tr").remove();
                                $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).parent().parent().find('div.row').remove();
                            }
                        })
                    }
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #dgvSocPsyandBehaviorHx").DataTable({
                        "language": {
                            "emptyTable": "No record found"
                        }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
                dfd.resolve();
            }
            else {
                utility.DisplayMessages(responseData.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    SocPsyandBehaviorHxGridLoad: function (response) {
        var GridPnlId = "pnlSocPsyandBehaviorHx_Result";
        var GriddgvId = "dgvSocPsyandBehaviorHx";
        var GridPagingId = "dgvSocPsyandBehaviorHx_Paging";

        if ($.fn.dataTable.isDataTable("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId)) {
            $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).dataTable().fnClearTable();
            $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).dataTable().fnDestroy();
            $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId + " tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvSocPsyandBehaviorHx') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId + " tbody").find("tr").remove();
                    $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).parent().parent().find('div.row').remove();
                }
            })
        }

        if (response.SocPsyandBehaviorHxCount > 0) {
            var SocPsyandBehaviorHx_JSON = response.SocPsyandBehaviorHx_JSON;
            $.each(SocPsyandBehaviorHx_JSON, function (i, item) {
                var Id;
                if (item.Current == '1') {
                    Id = item.SocialandBehaviorHxId;
                }
                else {
                    Id = item.SocialandBehaviorHxHistoryId;
                }
                var $row = $('<tr/>');
                $row.attr("id", Id);
                var Action = "Added Social, Psychological and Behavior History";
                var AddParameters = Id + ",'" + item.Current + "'";

                var SoapDate = $.datepicker.formatDate('mm/dd/yy', new Date((item.CreatedOn == item.ModifiedOn ? item.CreatedOn : item.ModifiedOn)));
                SoapDate = item.CreatedOn == item.ModifiedOn ? "Added On " + SoapDate : "Last Updated On " + SoapDate;
                var Soap = "";
                if (item.Unremarkable.toLowerCase() == "true") {
                    Soap = "Unremarkable" + (item.Comments != null ? "</br>" + item.Comments : "") + "</br>" + SoapDate;
                }
                else {
                    Soap = (item.TotalScore != null ? "<b>Total Score: </b>" + item.TotalScore + "</br>" : "") + ((item.PHQScore != null) ? "<b>PHQ Score:</b> " + item.PHQScore + "</br>" : "") + (item.Comments != null ? item.Comments + "</br>" : "") + SoapDate;
                }


                if (item.Current != '1' && item.ShowDetail == "1") {
                    $row.attr("onclick", "Clinical_SocPsyandBehaviorHx.ViewDetailOfSocPsyandBehaviorHxHistory(" + AddParameters + ")");
                }

                if (item.CreatedOn != item.ModifiedOn) {
                    Action = "Updated Social, Psychological and Behavior History";
                }

                $row.append('<td style="display:none;">' + Id + '</td><td>' + Action + '</td><td>' + Soap + '</td><td>' + item.ModifiedOn + " " + item.ModifiedBy + '</td>');
                $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GriddgvId + " tbody").last().append($row);
            });
        }
        else {

            $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GriddgvId).DataTable({
                "language": {
                    "emptyTable": "No record found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GriddgvId))
            ;
        else {
            $("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).DataTable({
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            }); // to remove records per page dropdown
        }
    },
    ChangeCurrentPast: function (obj) {
        if ($(obj).attr("status") == "1") {
            $(obj).attr("status", "0");
        }
        else {
            $(obj).attr("status", "1");
        }
        Clinical_SocPsyandBehaviorHx.SearchSocPsyandBehaviorHx();
    },
    ViewDetailOfSocPsyandBehaviorHxHistory: function (ID, Current) {
        var params = {};
        params["mode"] = "View";
        params["FromAdmin"] = "0";
        if (Clinical_SocPsyandBehaviorHx.params.TabID == 'clinicalTabProgressNote') {
            params['ParentCtrl'] = 'Clinical_SocPsyandBehaviorHx';
        }
        else {
            params["ParentCtrl"] = "clinicalTabSocPsyandBehaviorHx";//"Clinical_SocPsyandBehaviorHx";
        }
        params["ID"] = ID;
        params["Current"] = Current;
        var PanelID="";
        if (Clinical_SocPsyandBehaviorHx.params.ParentCtrl == "clinicalTabProgressNote") {
            PanelID = 'pnlClinicalProgressNote #pnlClinicalSocPsyandBehaviorHx';
            params["UnPanelID"] = 'pnlClinicalProgressNote #pnlClinicalSocPsyandBehaviorHx';
        }
        else {
            PanelID = 'pnlClinicalSocPsyandBehaviorHx';
            params["UnPanelID"] = 'pnlClinicalSocPsyandBehaviorHx';
        }
        LoadActionPan('Clinical_SocPsyandBehaviorHxView', params, PanelID);
    },
    SearchSocPsyandBehaviorHx_DBCALL: function (SocialandBehaviorHxId, PageNo, rpp) {
        var objData = new Object();
        objData["commandType"] = "SearchSocPsyandBehaviorHx";
        var IsCheckedIn = null;

        IsCheckedIn = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + ' #pnlSocPsyandBehaviorHx_Result #divSwitch #switchVisit').attr('status');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        objData["Current"] = IsCheckedIn;
        objData["SocialandBehaviorHxId"] = SocialandBehaviorHxId;
        objData["PageNumber"] = PageNo;
        objData["RowspPage"] = rpp;
        objData["PatientId"] = Clinical_SocPsyandBehaviorHx.params.PatientId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "History", "SocPsyandBehaviorHx");
    },
    cacheSocPsyandBehaviorHxJSON: function () {
        var dfd = $.Deferred();
        if (Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorJSON != '') {
            var updatedJSON = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx").getMyJSONByName();
            if (Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorJSON != updatedJSON) {
                Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorJSON = updatedJSON;
                $.when(Clinical_SocPsyandBehaviorHx.cacheSocPsyandBehaviorHxData(updatedJSON)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },
    cacheSocPsyandBehaviorHxData: function (updatedJSON, saveParentOnly) {
        var dfd = $.Deferred();
        jsonData = JSON.parse(updatedJSON);
        Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx = null;
        Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx = {};
        var QuestionAnswerArray = Clinical_SocPsyandBehaviorHx.GetArrayofQuestionAnswer();
        var SocPsyandBehaviorHxData = {
            SocialandBehaviorHxId: $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #SocialandBehaviorHxId").val(),
            PatientId: Clinical_SocPsyandBehaviorHx.params.patientID,
            SocialBehaviorDate: $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #dpSocialBehaviorDate").val(),
            Unremarkable: $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #chkUnremarkable").prop("checked"),
            Comments: $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #txtComments").val(),
            TotalScore: $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #TxtTotalScore").val(),
            PHQScore: $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #TxtPHQScore").val(),
            QuestionAnswerArray: QuestionAnswerArray,
        }
        Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx = SocPsyandBehaviorHxData;
        dfd.resolve();
        return dfd;
    },
    unRemarkableSocPsyandBehaviorHx: function (obj) {
        var isRemarkable = $(obj).prop("checked");
        if (isRemarkable == true) {
            if (!$('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").hasClass('disableAll')) {
                $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").addClass('disableAll')
            }
            var detailsSection = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection");
            $(detailsSection).resetAllControls(null);
            Clinical_SocPsyandBehaviorHx.ResetAnswerIds();
        }
        else {
            if ($('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").hasClass('disableAll')) {
                $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #QuestionAnswerSection").removeClass('disableAll')
            }
        }


        Clinical_SocPsyandBehaviorHx.cacheSocPsyandBehaviorHxJSON();
    },

    ResetAnswerIds: function () {
        $.each($("#" + Clinical_SocPsyandBehaviorHx.params.PanelID + " .Answer"), function (i, item) {
            var QuesAnsId = $(item).attr("id").split("_");
            if (QuesAnsId.length == 2) {
                $(item).attr("id", QuesAnsId[0] + "_A");
            }
        });
    },

    checkSocPsyandBehaviorHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_SocPsyandBehaviorHx').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
            var onClick = "Clinical_ProgressNote.SelectNotesComponentTab(\'SocPsyandBehaviorHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ')";
            if (globalAppdata["isMU3SocPsycBehaviourHx"] && globalAppdata["isMU3SocPsycBehaviourHx"].toLowerCase() == "false")
                onClick = "";
            $(CompnentSelector).append(' <li class="SocPsyandBehaviorHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_SocPsyandBehaviorHx title="Social, Psychological and Behavior Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="' + onClick + '" title="Social, Psychological and Behavior Hx">Social, Psychological and Behavior Hx</a> ' +
                      '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'SocPsyandBehaviorHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'SocPsyandBehaviorHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_SocPsyandBehaviorHx> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_SocPsyandBehaviorHx').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    createSocPsyandBehaviorHxBodyHTMLFromNote: function (response, NoteHTMLCtrl, UnloadSocPsyandBehaviorHx, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_SocPsyandBehaviorHx.checkSocPsyandBehaviorHxExists();
        if (response) {
            var SocPsyandBehaviorHxFill_Obj = response;
            var $mainDivSocPsyandBehaviorHx = $(document.createElement('div'));

            var SocPsyandBehaviorHxId = SocPsyandBehaviorHxFill_Obj.SocialandBehaviorHxId;

            if (SocPsyandBehaviorHxId > 0) {
                var $SectionBodySocPsyandBehaviorHx = $(document.createElement('section'));
                $SectionBodySocPsyandBehaviorHx.attr('id', "Cli_SocPsyandBehaviorHx_Main" + SocPsyandBehaviorHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_SocPsyandBehaviorHx_" + SocPsyandBehaviorHxId);
                var $ListSocPsyandBehaviorHx = $(document.createElement('ul'));

                $ListSocPsyandBehaviorHx.attr('class', 'list-unstyled')

                $SectionBodySocPsyandBehaviorHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SocPsyandBehaviorHx_" + SocPsyandBehaviorHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SocPsyandBehaviorHx_Main" + SocPsyandBehaviorHxId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListSocPsyandBehaviorHx.append("<li>" + SocPsyandBehaviorHxFill_Obj.SocPsyandBehaviorHxSoapText + "</li>");
                $DetailsDiv.append($ListSocPsyandBehaviorHx);
                $SectionBodySocPsyandBehaviorHx.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId).length == 0) {
                    $mainDivSocPsyandBehaviorHx.append($SectionBodySocPsyandBehaviorHx);
                    var HospitalizationHtml = $mainDivSocPsyandBehaviorHx.html();
                    if (HospitalizationHtml != '') {
                        $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().addClass('initialVisitBody');
                        $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().append(HospitalizationHtml);
                    }
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId).html($SectionBodySocPsyandBehaviorHx.html());
                    $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId + ' ul').append(CommentHTML);
                }
            }
        }
        dfd.resolve();
        return dfd;
    },

    createSocPsyandBehaviorHxBodyHTMLFromDefaultNotes: function (response, NoteHTMLCtrl, UnloadSocPsyandBehaviorHx, hideAlertMessage) {
        Clinical_SocPsyandBehaviorHx.checkSocPsyandBehaviorHxExists();
        if (response) {
            var SocPsyandBehaviorHxFill_Obj = response;
            var $mainDivSocPsyandBehaviorHx = $(document.createElement('div'));

            var SocPsyandBehaviorHxId = SocPsyandBehaviorHxFill_Obj.SocialandBehaviorHxId;

            if (SocPsyandBehaviorHxId > 0) {
                var $SectionBodySocPsyandBehaviorHx = $(document.createElement('section'));
                $SectionBodySocPsyandBehaviorHx.attr('id', "Cli_SocPsyandBehaviorHx_Main" + SocPsyandBehaviorHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_SocPsyandBehaviorHx_" + SocPsyandBehaviorHxId);
                var $ListSocPsyandBehaviorHx = $(document.createElement('ul'));

                $ListSocPsyandBehaviorHx.attr('class', 'list-unstyled')

                $SectionBodySocPsyandBehaviorHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SocPsyandBehaviorHx_" + SocPsyandBehaviorHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SocPsyandBehaviorHx_Main" + SocPsyandBehaviorHxId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListSocPsyandBehaviorHx.append("<li>" + SocPsyandBehaviorHxFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListSocPsyandBehaviorHx);
                $SectionBodySocPsyandBehaviorHx.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId).length == 0) {
                    $mainDivSocPsyandBehaviorHx.append($SectionBodySocPsyandBehaviorHx);
                    var HospitalizationHtml = $mainDivSocPsyandBehaviorHx.html();
                    if (HospitalizationHtml != '') {
                        $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().addClass('initialVisitBody');
                        $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().append(HospitalizationHtml);
                    }
                    return SocPsyandBehaviorHxId
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId).html($SectionBodySocPsyandBehaviorHx.html());
                    $(NoteHTMLCtrl + ' clinical_SocPsyandBehaviorHx').parent().parent().find('#Cli_SocPsyandBehaviorHx_Main' + SocPsyandBehaviorHxId + ' ul').append(CommentHTML);
                }
            }
        }
    },
    detachSocPsyandBehaviorHxFromNotes: function (SocPsyandBehaviorHxId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_socpsyandbehaviorhx');
                var selectedValue = SocPsyandBehaviorHxId.replace('Cli_SocPsyandBehaviorHx_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_SocPsyandBehaviorHx.DetachSocPsyandBehaviorHxFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + SocPsyandBehaviorHxId).remove();
                            Clinical_ProgressNote.Add_NoText();
                            Clinical_ProgressNote.saveComponentSOAPText("SocPsyandBehaviorHx", true);
                            //Clinical_ProgressNote.updateProgressNoteHTML();
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                '1'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },
    DetachSocPsyandBehaviorHxFromNotes_DBCall: function (SocPsyandBehaviorHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["SocialandBehaviorHxId"] = SocPsyandBehaviorHxId;
        objData["commandType"] = "detach_SocPsyandBehaviorHx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SocPsyandBehaviorHx");
    },

    detach_ComponentsSocPsyandBehaviorHx: function (componentName, isUpdate, SocPsyandBehaviorHxComponentRemove) {

        var Clinical_SocPsyandBehaviorHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_SocPsyandBehaviorHx').parent().parent().find('section[id*="Cli_SocPsyandBehaviorHx_Main"]').map(function () {
            return this.id.replace("Cli_SocPsyandBehaviorHx_Main", "");
        }).get().join(',');

        if (SocPsyandBehaviorHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_SocPsyandBehaviorHx').parent().parent().attr('NoteComponentId');
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='SocPsyandBehaviorHx']").remove();
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_SocPsyandBehaviorHx').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_SocPsyandBehaviorHx').parent().parent().remove();
            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_SocPsyandBehaviorHx').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('SocPsyandBehaviorHx', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_SocPsyandBehaviorHx').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }

        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_SocPsyandBehaviorHx').parent().parent().find('section[id*="Cli_SocPsyandBehaviorHx_Main"]').remove();
        }

        if (Clinical_SocPsyandBehaviorHxIds == "" || Clinical_SocPsyandBehaviorHxIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("SocPsyandBehaviorHx", true);
            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_SocPsyandBehaviorHx.DetachSocPsyandBehaviorHxFromNotes_DBCall(Clinical_SocPsyandBehaviorHxIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("SocPsyandBehaviorHx", true);
                        //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
}