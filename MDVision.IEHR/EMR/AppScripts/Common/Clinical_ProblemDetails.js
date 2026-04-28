Clinical_ProblemDetails = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    icdsValues: {},
    LastSctBaseSearch: '',
    FavListName: "ClinicalProblemList",

    Load: function (params) {

        Clinical_ProblemDetails.params = params;

        Clinical_ProblemDetails.params.mode = "Add";

        if (Clinical_ProblemDetails.params.PanelID != 'pnlClinicalProblemDetails') {
            Clinical_ProblemDetails.params.PanelID = Clinical_ProblemDetails.params.PanelID + ' #pnlClinicalProblemDetails';
        }
        else {
            Clinical_ProblemDetails.params.PanelID = 'pnlClinicalProblemDetails';
        }

        $('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails #txtDiagnosis').val(Clinical_ProblemDetails.params.Diagnosis);

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_ProblemDetails.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        if (Clinical_ProblemDetails.bIsFirstLoad) {
            Clinical_ProblemDetails.domReadyFunc();
            Clinical_ProblemDetails.ValidateProblemLists();
            
        }
        var self = $('#' + Clinical_ProblemDetails.params.PanelID);
        if (Clinical_ProblemDetails.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {

              
                Clinical_ProblemDetails.loadAllLookups().done(function () {
                    if (Clinical_ProblemDetails.params.IsUpdate == true) {
                        Clinical_ProblemDetails.loadDetailsProblem(Clinical_ProblemDetails.params.ProblemListId);
                    }

                    Clinical_ProblemDetails.bIsFirstLoad = false;
                    $('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').data('serialize', $('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').serialize());
                });
               
            });
        }
        else {
            if (EMRUtility.getFavListStatus(Clinical_ProblemDetails.FavListName)) {

                $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #favSectionDiv").addClass("toggledHor");
                $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #FormDiv").addClass("toggleHorContainer");
            }
            else {
                $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #favSectionDiv").removeClass("toggledHor");
                $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #FormDiv").removeClass("toggleHorContainer");
            }

        }

        if (Clinical_ProblemDetails.params.ParentCtrl == "clinicalTabProgressNote") {

            $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #chkChiefComplaints").attr('checked', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #chkCC").removeClass('hidden');

            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_ProblemDetails.params.PanelID, 'Medical', 'Problems', 'Clinical_ProblemDetails.UnLoadTab();', 'frmClinicalProblemDetails');
        }

        if (Clinical_ProblemDetails.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ProblemDetails.params.PanelID + " div#FaceSheetPager", Clinical_ProblemDetails.params.FaceSheetComponents, 'problem list');
        }
    },
    loadDetailsProblem: function (problemListId) {

        Clinical_ProblemDetails.loadDetails(problemListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var self = $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails");
                var objData = JSON.parse(response.Fill_JSON);
               
                utility.bindMyJSONByName(true, objData, false, self).done(function () {

                    if (objData.NKOClinical == "True") {
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpClinicalDiagnosisDate").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageDescriptor").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryClinicalTumor").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNC").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatases").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerClinicalCancer").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageGroup").attr('disabled', true);
                    } else {
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpClinicalDiagnosisDate").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageDescriptor").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryClinicalTumor").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNC").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatases").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerClinicalCancer").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageGroup").attr('disabled', false);
                    }
                    if (objData.NKOPathologic == "True") {
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpEffectiveDate").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageGroup").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageDescriptor").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryTumorPathologic").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNP").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatasesPathologic").attr('disabled', true);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerPathologicCancer").attr('disabled', true);
                    } else {
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpEffectiveDate").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageGroup").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageDescriptor").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryTumorPathologic").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNP").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatasesPathologic").attr('disabled', false);
                        $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerPathologicCancer").attr('disabled', false);
                    }
                });


                

               
            }
        });


    },

    loadDetails: function (problemListId) {
        var objData = new Object();
        objData["ProblemListId"] = problemListId;
        objData["commandType"] = "load_details";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
   
    enableDisableClinicalSection: function (obj) {
        if ($(obj).prop('checked') == true) {
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpClinicalDiagnosisDate").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpClinicalDiagnosisDate").val('');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageDescriptor").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageDescriptor option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryClinicalTumor").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryClinicalTumor option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNC").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNC option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatases").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatases option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerClinicalCancer").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerClinicalCancer option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageGroup").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageGroup option:first").attr('selected', 'selected');

        } else {
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpClinicalDiagnosisDate").attr('disabled', false);
            var date = new Date();
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpClinicalDiagnosisDate").val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageDescriptor").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryClinicalTumor").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNC").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatases").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerClinicalCancer").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlStageGroup").attr('disabled', false);
        }

    },

    enableDisablePathologicSection: function (obj) {
        if ($(obj).prop('checked') == true) {

            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpEffectiveDate").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpEffectiveDate").val('');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageGroup").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageGroup option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageDescriptor").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageDescriptor option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryTumorPathologic").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryTumorPathologic option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNP").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNP option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatasesPathologic").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatasesPathologic option:first").attr('selected', 'selected');
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerPathologicCancer").attr('disabled', true);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerPathologicCancer option:first").attr('selected', 'selected');
        } else {
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpEffectiveDate").attr('disabled', false);
           
            var date = new Date();
          
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #dpEffectiveDate").val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
           // $("#mydate").datepicker().datepicker("setDate", new Date());
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageGroup").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPathologicStageDescriptor").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlPrimaryTumorPathologic").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlRLNP").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddlDistanceMestastatasesPathologic").attr('disabled', false);
            $('#' + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #ddStagerPathologicCancer").attr('disabled', false);
        }

    },
    loadlookups: function (ProblemListId, PageNumber, RowsPerPage, getInActiveProblems) {
        var objData = new Object();
        objData["commandType"] = "loadlookups";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    loadAllLookups: function () {
        var objDeffered = $.Deferred();
        Clinical_ProblemDetails.loadlookups().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var DiagnosisConfirmationObj = response.LookupsJSON.DiagnosisConfirmation;
                var LateralityObj = response.LookupsJSON.Laterality;
                var BehaviorObj = response.LookupsJSON.BehaviorCode;
                var GradeObj = response.LookupsJSON.Grade;
                var ClinicalStageGroupObj = response.LookupsJSON.ClinicalStageGroup;
                var ClinicalStageDescriptorObj = response.LookupsJSON.ClinicalStageDescriptor;
                var ClinicalTumorObj = response.LookupsJSON.ClinicalTumor;
                var ClinicalNodeObj = response.LookupsJSON.ClinicalNode;
                var ClinicalMetastasisObj = response.LookupsJSON.ClinicalMetastasis;
                var PathologicMetastasisObj = response.LookupsJSON.PathologicMetastasis;
                var PathologicNodeObj = response.LookupsJSON.PathologicNode;
                var PathologicStageDescriptorObj = response.LookupsJSON.PathologicStageDescriptor;
                var PathologicTumorObj = response.LookupsJSON.PathologicTumor;
                var PathologicStageGroupObj = response.LookupsJSON.PathologicStageGroup;
                var PathologicStagerCancerObj = response.LookupsJSON.pathologicStagerCancer;
                var StagerCancerObj = response.LookupsJSON.StagerCancer;
                var primarySiteObj = response.LookupsJSON.PrimarySite;
                var histologicTypeObj = response.LookupsJSON.HistologicType;


                Clinical_ProblemDetails.params.PrimarySite = primarySiteObj;
                Clinical_ProblemDetails.params.HistologicType = histologicTypeObj;
                var $ddlDiagnosisConfirmation = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlDiagnosisConfirmation');
                var $ddlLaterality = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlLaterality');
                var $ddlBehaviorCode = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlBehavior');
                var $ddlGrade = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlGrade');
                var $ddlClinicalStageGroup = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlStageGroup');
                var $ddlClinicalStageDescriptor = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlStageDescriptor');
                var $ddlClinicalTumor = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlPrimaryClinicalTumor');
                var $ddlClinicalNode = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlRLNC');
                var $ddlClinicalMetastasis = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlDistanceMestastatases');
                var $ddlPathologicMetastasis = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlDistanceMestastatasesPathologic');
                var $ddlPathologicNode = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlRLNP');
                var $ddlPathologicStageDescriptor = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlPathologicStageDescriptor');
                var $ddlPathologicTumor = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlPrimaryTumorPathologic');
                var $ddlPathologicStageGroup = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddlPathologicStageGroup');
                var $ddlStagerClinicalCancer = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddStagerClinicalCancer');
                var $ddlStagerPathologicCancer = $('#' + Clinical_ProblemDetails.params.PanelID + ' #ddStagerPathologicCancer');

                var ddls = [];
                ddls.push($ddlDiagnosisConfirmation);
                ddls.push($ddlLaterality);
                ddls.push($ddlBehaviorCode);
                ddls.push($ddlGrade);
                ddls.push($ddlClinicalStageGroup);
                ddls.push($ddlClinicalStageDescriptor);
                ddls.push($ddlClinicalTumor);
                ddls.push($ddlClinicalNode);
                ddls.push($ddlClinicalMetastasis);
                ddls.push($ddlPathologicMetastasis);
                ddls.push($ddlPathologicNode);
                ddls.push($ddlPathologicStageDescriptor);
                ddls.push($ddlPathologicTumor);
                ddls.push($ddlPathologicStageGroup);
                ddls.push($ddlStagerClinicalCancer);
                ddls.push($ddlStagerPathologicCancer);

                $.each(ddls, function (ind, it) {

                    it.append($('<option/>', {
                        value: "",
                        html: "- Select -"
                    }));
                });

                $.each(DiagnosisConfirmationObj, function (i, item) {

                    $ddlDiagnosisConfirmation.append(
                            $('<option/>', {
                                value: item.TNMSystemCodeId,
                                html: item.Code + ' - ' + item.Description,

                            })
                        );
                });

                $.each(LateralityObj, function (i, item) {

                    $ddlLaterality.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(BehaviorObj, function (i, item) {

                    $ddlBehaviorCode.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(GradeObj, function (i, item) {

                    $ddlGrade.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(ClinicalStageGroupObj, function (i, item) {

                    $ddlClinicalStageGroup.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(PathologicStageGroupObj, function (i, item) {

                    $ddlPathologicStageGroup.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(ClinicalStageDescriptorObj, function (i, item) {

                    $ddlClinicalStageDescriptor.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(ClinicalTumorObj, function (i, item) {

                    $ddlClinicalTumor.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(ClinicalNodeObj, function (i, item) {

                    $ddlClinicalNode.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(ClinicalMetastasisObj, function (i, item) {

                    $ddlClinicalMetastasis.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(PathologicMetastasisObj, function (i, item) {

                    $ddlPathologicMetastasis.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(PathologicNodeObj, function (i, item) {

                    $ddlPathologicNode.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(PathologicStageDescriptorObj, function (i, item) {

                    $ddlPathologicStageDescriptor.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(PathologicTumorObj, function (i, item) {

                    $ddlPathologicTumor.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });


                $.each(StagerCancerObj, function (i, item) {

                    $ddlStagerClinicalCancer.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });

                $.each(PathologicStagerCancerObj, function (i, item) {

                    $ddlStagerPathologicCancer.append(
                        $('<option/>', {
                            value: item.TNMSystemCodeId,
                            html: item.Code + ' - ' + item.Description,

                        })
                    );
                });





            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            objDeffered.resolve();
        });

        return objDeffered;
    },

    bindPrimarySite: function () {


        utility.Keyupdelay(function () {

            var AllData = [];
            $.each(Clinical_ProblemDetails.params.PrimarySite, function (j, item) {
                AllData.push({ id: item.Id, value: item.Description });
            });


            $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtPrimarySite").autocomplete({
                autoFocus: true,
                source: AllData,

                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtPrimarySite").val(ui.item.value);
                        $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #hfPrimarySiteId").val(ui.item.id);

                    }, 100);
                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtPrimarySite"), 'pnlClinicalProblemDetails #hfPrimarySiteId', false);

                }, 200);
            });


            $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtPrimarySite").autocomplete("search");

        });

    },
    bindHistologicType: function () {


        utility.Keyupdelay(function () {

            var AllData = [];
            $.each(Clinical_ProblemDetails.params.HistologicType, function (j, item) {
                AllData.push({ id: item.Id, value: item.Description });
            });

            $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtHistologicType").autocomplete({
                autoFocus: true,
                source: AllData,

                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtHistologicType").val(ui.item.value);
                        $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #hfHistologicTypeId").val(ui.item.id);

                    }, 100);
                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtHistologicType"), 'pnlClinicalProblemDetails #hfHistologicTypeId', false);

                }, 200);
            });


            $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails #txtHistologicType").autocomplete("search");

        });

    },

    domReadyFunc: function () {

        $(document).ready(function () {
            
            utility.CreateDatePicker(Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails #dpClinicalDiagnosisDate', function () {
                if ($('#'+Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').data("bootstrapValidator") != null) {
                    $('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').bootstrapValidator('revalidateField', 'ClinicalDiagnosisDate');
                }
            }, true);
            utility.CreateDatePicker(Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails #dpEffectiveDate', function () {
                if ($('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').data("bootstrapValidator") != null) {
                    $('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').bootstrapValidator('revalidateField', 'EffectiveDate');
                }
            }, true);
            utility.CreateDatePicker(Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails #dpStartDate', function () {
                if ($('#'+Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').data("bootstrapValidator") != null) {
                    $('#'+Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').bootstrapValidator('revalidateField', 'DiagnosisDate');
                }
            }, true);

            $('#' + Clinical_ProblemDetails.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });

            //for autocomplete zIndex fix
            $("#txtProblems").on("autocompleteopen", function (event, ui) {
                if ($(this).closest(".modal-dialog").length == 0)
                    $(this).autocomplete('widget').zIndex("1018");
            });
        });

    },

    ValidateProblemLists: function () {
        $('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   Diagnosis: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   DiagnosisDate: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   

                   DiagnosisConfirmation: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   PrimarySite: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   Laterality: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   HistologicType: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   Behavior: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   Grade: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   ClinicalDiagnosisDate: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   ClinicalStageGroup: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   ClinicalStageDescriptor: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   PrimaryClinicalTumor: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   RLNC: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DistanceMestastatases: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   StagerClinicalCancer: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   EffectiveDate: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PathologicStageGroup: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PathologicStageDescriptor: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PrimaryTumorPathologic: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   RLNP: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DistanceMestastatasesPathologic: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   StagerPathologicCancer: {
                       group: '.col-md-4',
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
            $('#pnlClinicalProblemDetails #frmClinicalProblemDetails').bootstrapValidator('revalidateField', "DiagnosisDate");
            $('#pnlClinicalProblemDetails #frmClinicalProblemDetails').bootstrapValidator('revalidateField', "ClinicalDiagnosisDate");
            $('#pnlClinicalProblemDetails #frmClinicalProblemDetails').bootstrapValidator('revalidateField', "EffectiveDate");
            Clinical_ProblemDetails.ProblemDetailsSave();
        });
    },
    ProblemDetailsSave: function () {

        var self = $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails");
        var myJSON = self.getMyJSONByName();

        Clinical_ProblemDetails.saveProblemDetails(myJSON).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                if (Clinical_ProblemDetails.params.IsUpdate == true) {

                    utility.DisplayMessages("Successfully Updated", 1);                   

                } else {
                    utility.DisplayMessages(response.message, 1);
                   
                }
                var ProblemListId = Clinical_ProblemDetails.params.ProblemListsId == null ? Clinical_ProblemDetails.params.ProblemListId : Clinical_ProblemDetails.params.ProblemListsId;
                var ProblemInNote = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + ProblemListId)
                if (ProblemInNote.length > 0)
                    Clinical_ProblemDetails.UpdateProblemSoaptext();

                $('#' + Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails').resetAllControls(null);
                UnloadActionPan(Clinical_ProblemDetails.params.ParentCtrl, 'Clinical_ProblemDetails');

                Clinical_ProblemLists.ProblemListsSearch();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    UpdateProblemSoaptext: function () {
        var self = $("#" + Clinical_ProblemDetails.params.PanelID + " #frmClinicalProblemDetails");
        var myJSON = self.getMyJSONByName();
        var element = JSON.parse(myJSON);
        // Description only
        var Behavior = element.Behavior_text.split('-').pop().trim();
        var Grade = element.Behavior_text.split('-').pop().trim();
        var Diagnosisconfirmation = element.DiagnosisConfirmation_text.split('-').pop().trim();
        var Laterality = element.Laterality_text.split('-').pop().trim();
        var TNMClinicalStageDescriptor = element.ClinicalStageDescriptor_text.split('-').pop().trim();
        var TNMPathologicStageDescriptor = element.PathologicStageDescriptor_text.split('-').pop().trim();
        var StagerPathologicCancer = element.StagerPathologicCancer_text.split('-').pop().trim();
        var StagerClinicalCancer = element.StagerClinicalCancer_text.split('-').pop().trim();
        var HistologicType = element.HistologicType.substr(element.HistologicType.lastIndexOf('-') + 1, element.HistologicType.length - 1).trim();
        var PrimarySite = element.PrimarySite.split('-').pop().trim();
        // Code 

        var TNMClinicalStageGroup = element.ClinicalStageGroup_text.substr(0, element.ClinicalStageGroup_text.indexOf('-')).trim();
        var PrimaryTumorClinical = element.PrimaryClinicalTumor_text.substr(0, element.PrimaryClinicalTumor_text.indexOf('-')).trim();
        var RLNC = element.RLNC_text.substr(0, element.RLNC_text.indexOf('-')).trim();
        var DistantMetastasesClinical = element.DistanceMestastatases_text.substr(0, element.DistanceMestastatases_text.indexOf('-')).trim();
     
        var TNMPathologicStageGroup = element.PathologicStageGroup_text.substr(0, element.PathologicStageGroup_text.indexOf('-')).trim();
        var PrimaryTumorPathologic = element.PrimaryTumorPathologic_text.substr(0, element.PrimaryTumorPathologic_text.indexOf('-')).trim();
        var RLNP = element.RLNP_text.substr(0, element.RLNP_text.indexOf('-')).trim();
        var DistantMetastasesPathologic = element.DistanceMestastatasesPathologic_text.substr(0, element.DistanceMestastatasesPathologic_text.indexOf('-')).trim();
       // end of code only

       
        var NKOClinical = element.NKOClinical
        var NKOPathologic = element.NKOPathologic

        var ProblemListId = Clinical_ProblemDetails.params.ProblemListsId == null ? Clinical_ProblemDetails.params.ProblemListId : Clinical_ProblemDetails.params.ProblemListsId;
       
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + ProblemListId).removeAttr('IsCancerDisease');
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + ProblemListId).attr('IsCancerDisease', 'True');

        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + ProblemListId).find('#CancerDetailList').remove();

        var ProblemDetailcolor = 'style = "color:#444444;font-weight:bolder" ';

        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + ProblemListId).append("<li id='CancerDetailList'><span " + ProblemDetailcolor + ">Diagnosis Confirmation- </span>   Status:" + (element.IsActive  ? "Active" : "Inactive") + ",Diagnosis:" + element.Diagnosis + ",Diagnosis Date:" +  element.DiagnosisDate + ", Diagnostic Confirmation:" + Diagnosisconfirmation + ","
            + "Primary Site:" + PrimarySite + ", Laterality:" + Laterality + ", Histologic Type:" + HistologicType + ", Behavior:" + Behavior + ", Grade:" + Grade +
"<br/><span " + ProblemDetailcolor + ">TNM Clinical Stage Information- </span>" + (NKOClinical == true ? "No Clinical Stage Information" : "Diagnosis Date:" + element.ClinicalDiagnosisDate + ", TNM Stage Group:" + TNMClinicalStageGroup + ", TNM Stage Descriptor:" + TNMClinicalStageDescriptor + 
", Primary Clinical Tumor:" + PrimaryTumorClinical + ", Regional Lymph Nodes Clinical:" + RLNC + ",Distance Mestastatases" + DistantMetastasesClinical + ", Stager Clinical Cancer :" + StagerClinicalCancer) +
"<br/><span " + ProblemDetailcolor + ">TNM Pathologic Stage Information- </span>" + (NKOPathologic == true ? "No Pathologic Stage Information" : "Effective  Date:" + element.EffectiveDate + ", TNM Pathologic Stage Group:" + TNMPathologicStageGroup + ", TNM Pathologic Stage Descriptor:" + TNMPathologicStageDescriptor +
", Primary Tumor Pathologic:" + PrimaryTumorPathologic + ", Regional Lymph Nodes Pathologic:" + RLNP + ",Distance Mestastatases Pathologic:" + DistantMetastasesPathologic + ", Stager Pathologic Cancer:" + StagerPathologicCancer) + "<br/>")

         Clinical_ProgressNote.saveComponentSOAPText('Problems', true);

    },
    saveProblemDetails: function (myjson) {

        if (myjson == "null") {

            var objData = new Object();
        }
        else {
            var objData = JSON.parse(myjson);
        }
        objData["ProblemListId"] = Clinical_ProblemDetails.params.ProblemListsId == null ? Clinical_ProblemDetails.params.ProblemListId : Clinical_ProblemDetails.params.ProblemListsId;
        objData["commandType"] = "SAVE_PROBLEM_DETAILS";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_ProblemDetails.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(Clinical_ProblemDetails.params.PanelID + ' #frmClinicalProblemDetails', function () {
                if (Clinical_ProblemDetails.params["FromAdmin"] == "0") {
                    if (Clinical_ProblemDetails.params != null && Clinical_ProblemDetails.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_ProblemDetails.params.ParentCtrl, 'Clinical_ProblemDetails');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_ProblemDetails');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('clinical_problems');
            }, function () {
                if (Clinical_ProblemDetails.params["FromAdmin"] == "0") {
                    if (Clinical_ProblemDetails.params != null && Clinical_ProblemDetails.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_ProblemDetails.params.ParentCtrl, 'Clinical_ProblemDetails');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_ProblemDetails');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('clinical_problems');
            });

            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else if (Clinical_ProblemDetails.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_ProblemDetails.addProblemListsToNotes();
            UnloadActionPan(Clinical_ProblemDetails.params.ParentCtrl, 'Clinical_ProblemDetails');
        }
        else {
            if (Clinical_ProblemDetails.params["FromAdmin"] == "0") {
                if (Clinical_ProblemDetails.params != null && Clinical_ProblemDetails.params.ParentCtrl != null) {
                   
                    // Set IsCancerDisease as true...
                    if (!Clinical_ProblemDetails.params.IsUpdate) {
                        var MyJson = "null";
                        Clinical_ProblemDetails.saveProblemDetails(MyJson).done(function (response) {
                            UnloadActionPan(Clinical_ProblemDetails.params.ParentCtrl, 'Clinical_ProblemDetails');
                            response = JSON.parse(response);
                            if (response.status != false) {

                                Clinical_ProblemLists.ProblemListsSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        UnloadActionPan(Clinical_ProblemDetails.params.ParentCtrl, 'Clinical_ProblemDetails');
                    }
                }
                else
                    UnloadActionPan(null, 'Clinical_ProblemDetails');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('clinical_problems');
        }
        return objDeffered;
    },


}