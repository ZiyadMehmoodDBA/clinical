Clinical_Treatment = {
    bIsFirstLoad: true,
    params: [],
    labOrderRows: [],
    Load: function (params) {
        Clinical_Treatment.params = params;
        Clinical_Treatment.params.TreatmentProblems = [];
        Clinical_Treatment.params.IsExpand = true;
        Clinical_Treatment.params.CurrentTreatmentProblem = -1;
        if (Clinical_Treatment.params.PanelID != 'pnlClinicalTreatment') {
            Clinical_Treatment.params.PanelID = Clinical_Treatment.params.PanelID + ' #pnlClinicalTreatment';
        } else {
            Clinical_Treatment.params.PanelID = 'pnlClinicalTreatment';
        }
        Clinical_Treatment.params.PatientId = $('#PatientProfile #hfPatientId').val();
        if (Clinical_Treatment.bIsFirstLoad) {
            var self = $('#' + Clinical_Treatment.params.PanelID);
            self.find('.Diagnosis > select').attr('ddlist', 'LookupProblemLists');
            var data = "IsActive=&ID=" + $('#PatientProfile #hfPatientId').val();
            self.find('.Diagnosis').loadDropDowns(true, data).done(function () {
            });
            Clinical_Treatment.bIsFirstLoad = false;
            Treatment_ProblemListGrid.ProblemListsSearch();
            Clinical_Treatment.prescriptionSearch();
            Treatment_ProcedureListGrid.ProceduresSearch();
            Clinical_Treatment.LabOrderSearch(null, null, null, null, "Pending");
            Clinical_Treatment.radiologyOrderSearch();
            Clinical_Treatment.LoadReferrals(null, 1, 15);
            Clinical_Treatment.TherapeuticSearch('', 1, 15);
            Clinical_Treatment.ImmunizationSearch('', 1, 15);
            utility.callbackAfterAllDOMLoaded(function () {
                Clinical_Treatment.LoadTreatment();
            });
            Clinical_Treatment.OnDomeReady();
        }

    },
    OnDomeReady: function () {
        $("#" + Clinical_Treatment.params.PanelID + " #txtComments").focusout(function () {
            Clinical_Treatment.SetCommentValue();
        });
    },
    SetCommentValue: function () {
        $.each(Clinical_Treatment.params.TreatmentProblems, function (i, item) {
            if (item.ProblemId == Clinical_Treatment.params.CurrentTreatmentProblem) {
                item.Comments = $("#" + Clinical_Treatment.params.PanelID + " #txtComments").val();
            }
        });
    },
    UnLoadTab: function (Check) {
        if (Check) {
            if (JSON.stringify(Clinical_Treatment.params.TreatmentProblems) != JSON.stringify(Clinical_Treatment.params.ActualTreatments)) {
                utility.myConfirm('60', function () {
                    Clinical_Treatment.UnLoad();
                }, function () {

                },
                '1'
                );
            }
            else {
                Clinical_Treatment.UnLoad();
            }

        }
        else {
            Clinical_Treatment.UnLoad();
        }
    },
    UnLoad: function () {
        if (Clinical_Treatment.params["FromAdmin"] == "0") {
            if (Clinical_Treatment.params != null && Clinical_Treatment.params.ParentCtrl != null) {
                if (Clinical_Treatment.params.ParentCtrl == "clinicalTabProgressNote") {
                    EMRUtility.scrollToPNcomponent('clinical_treatment');
                }
                UnloadActionPan(Clinical_Treatment.params.ParentCtrl, 'Clinical_Treatment');
                
            }
            else
                UnloadActionPan(null, 'Clinical_Treatment');
        }
        else {
            RemoveAdminTab();
        }
    },
    SelectProblem: function (obj, ProblemId, ProblemDescription, event) {

        if (event != null) {
            if ($(event.target).is('i[class*="fa fa-close"]') || $(event.target).is('i[class*="fa fa-edit"]') || $(event.target).is('i[class*="fa-plus-square"]')) {
                return;
            }
        }



        if (obj.hasClass("active")) {
        }
        else {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
        Clinical_Treatment.UncheckAllCustomCheckBoxs();
        Clinical_Treatment.EnableAllComponenets(true);
        Clinical_Treatment.params.CurrentTreatmentProblem = ProblemId;
        var CheckedProblem = $.grep(Clinical_Treatment.params.TreatmentProblems, function (n, i) {
            return n.ProblemId == ProblemId;
        });
        if (CheckedProblem.length > 0) {
            Clinical_Treatment.loadCasheData(CheckedProblem);
        }
        else {
            var item = {};
            item.ProblemId = ProblemId;
            item.TreatmentId = -1;
            item.ProblemDescription = ProblemDescription;
            item.PrescriptionIds = "";
            item.LabOrderIds = "";
            item.DiagnosticImagingIds = "";
            item.ProcedureIds = "";
            item.ImmunizationIds = "";
            item.TherapeuticIds = "";
            item.ReferralIds = "";
            item.Comments = "";
            Clinical_Treatment.params.TreatmentProblems.push(item);
        }
        Clinical_Treatment.DisableEnableComments();
        Clinical_Treatment.ExpandTheGrid();
    },
    UncheckAllCustomCheckBoxs: function (enable) {
        $("#" + Clinical_Treatment.params.PanelID + " .CustomCheckBox").prop("checked", false);
        $("#" + Clinical_Treatment.params.PanelID + " #txtComments").val("");
    },
    EnableAllComponenets: function (enable) {
        if (enable) {
            $("#" + Clinical_Treatment.params.PanelID + " .TreatComponent").removeClass("disableAll");
        }
    },
    CheckUncheckAll: function (chkBox) {
        var ElementName = $(chkBox).attr("Childname");
        var ArrayProperty = $(chkBox).attr("ArrayProperty");
        var CurrentProblem = $.grep(Clinical_Treatment.params.TreatmentProblems, function (n, i) {
            return n.ProblemId == Clinical_Treatment.params.CurrentTreatmentProblem;
        });
        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Treatment.params.PanelID + " [name='" + ElementName + "']").prop("checked", true);

            var list = $("input[name='" + ElementName + "']").map(function () {
                return this.id.split('_')[1];
            }).get();
            if (CurrentProblem.length > 0) {
                if (CurrentProblem[0][ArrayProperty] != "") {
                    CurrentProblem[0][ArrayProperty] = CurrentProblem[0][ArrayProperty] + "," + list.join(',');
                }
                else {
                    CurrentProblem[0][ArrayProperty] = list.join(',');
                }

            }
        } else {
            $("#" + Clinical_Treatment.params.PanelID + " [name='" + ElementName + "']").prop("checked", false);
            var actualData = [];
            $.each($("input[name='" + ElementName + "']"), function (i, item) {
                var Data = [];
                $.each(CurrentProblem[0][ArrayProperty].split(','), function (n, i) {
                    if (i != $(item).attr("id").split('_')[1]) {
                        Data.push(i);
                    }
                });
                CurrentProblem[0][ArrayProperty] = Data.join(',');
            });
        }
        Clinical_Treatment.DisableEnableComments();
    },
    CheckUncheck: function (chkBox) {
        var ArrayProperty = $(chkBox).attr("ArrayProperty");
        var id = $(chkBox).attr("id").split('_')[1];
        var name = $(chkBox).attr("name");

        var CurrentProblem = $.grep(Clinical_Treatment.params.TreatmentProblems, function (n, i) {
            return n.ProblemId == Clinical_Treatment.params.CurrentTreatmentProblem;
        });
        if (CurrentProblem.length > 0) {
            if ($(chkBox).is(':checked')) {
                CurrentProblem[0][ArrayProperty] = CurrentProblem[0][ArrayProperty] + ((CurrentProblem[0][ArrayProperty] == "" ? "" : ",") + id);
            }
            else {
                var array = [];
                $.each(CurrentProblem[0][ArrayProperty].split(','), function (i, item) {
                    if (item != id) {
                        array.push(item);
                    }
                });
                CurrentProblem[0][ArrayProperty] = array.join(',');
            }
        }
        //check parent checkBox
        if ($("#" + Clinical_Treatment.params.PanelID + " [name='" + name + "']").length == $("#" + Clinical_Treatment.params.PanelID + " [name='" + name + "']:checked").length) {
            $("#" + Clinical_Treatment.params.PanelID + " [name='" + name + "Hdr']").prop("checked", true);
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + " [name='" + name + "Hdr']").prop("checked", false);
        }
        Clinical_Treatment.DisableEnableComments();
    },
    DisableEnableComments: function () {
        var CurrentProblem = $.grep(Clinical_Treatment.params.TreatmentProblems, function (n, i) {
            return n.ProblemId == Clinical_Treatment.params.CurrentTreatmentProblem;
        });
        if (CurrentProblem.length > 0) {
            if ($("#" + Clinical_Treatment.params.PanelID + " #TreatmentComments").hasClass('disableAll')) {
                $("#" + Clinical_Treatment.params.PanelID + " #TreatmentComments").removeClass('disableAll');
            }
            if (CurrentProblem[0] && (CurrentProblem[0].DiagnosticImagingIds || CurrentProblem[0].ImmunizationIds || CurrentProblem[0].LabOrderIds || CurrentProblem[0].PrescriptionIds) || CurrentProblem[0].ProcedureIds || CurrentProblem[0].ReferralIds || CurrentProblem[0].TherapeuticIds) {
                //if ($("#" + Clinical_Treatment.params.PanelID + " #TreatmentComments").hasClass('disableAll'))
                //$("#" + Clinical_Treatment.params.PanelID + " #TreatmentComments").removeClass('disableAll');
            }
            else {
                //$("#" + Clinical_Treatment.params.PanelID + " #TreatmentComments").addClass('disableAll');
                $("#" + Clinical_Treatment.params.PanelID + " #txtComments").val("");
                Clinical_Treatment.SetCommentValue();
            }
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + " #TreatmentComments").addClass('disableAll');
            $("#" + Clinical_Treatment.params.PanelID + " #txtComments").val("");
            Clinical_Treatment.SetCommentValue();
        }
    },
    loadCasheData: function (CheckedProblem) {
        $.each($("#" + Clinical_Treatment.params.PanelID + " .HeaderCheckBox"), function (i, item) {
            var ChildName = $(item).attr("Childname");
            var ArrayProperty = $(item).attr("ArrayProperty");
            $.each(CheckedProblem[0][ArrayProperty].split(','), function (i, item) {
                $("#" + Clinical_Treatment.params.PanelID + " #" + ChildName + "_" + item).prop("checked", true);
            });

            if ($("#" + Clinical_Treatment.params.PanelID + " [name='" + ChildName + "']").length > 0 && ($("#" + Clinical_Treatment.params.PanelID + " [name='" + ChildName + "']").length == $("#" + Clinical_Treatment.params.PanelID + " [name='" + ChildName + "']:checked").length)) {
                $(item).prop("checked", true);
            }

        });
        $("#" + Clinical_Treatment.params.PanelID + " #txtComments").val(CheckedProblem[0].Comments);
    },
    AddTreatmentToNote: function () {
        var AcTreatmentProblems = [];
        var CopyOfTreatmentProblems = Clinical_Treatment.params.TreatmentProblems;
        $.each(Clinical_Treatment.params.TreatmentProblems, function (i, item) {
            if (item.PrescriptionIds == "" &&
               item.LabOrderIds == "" &&
               item.DiagnosticImagingIds == "" &&
               item.ProcedureIds == "" &&
               item.ImmunizationIds == "" &&
               item.TherapeuticIds == "" &&
               item.ReferralIds == "" && item.TreatmentId < 0) {
            }
            else {
                AcTreatmentProblems.push(item);
            }
        });

        Clinical_Treatment.params.TreatmentProblems = AcTreatmentProblems;
        if (Clinical_Treatment.params.TreatmentProblems.length > 0) {
            var params = [];
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = "Clinical_Treatment";
            params["NoteId"] = Clinical_Treatment.params.NotesId;
            params["Comment"] = $("#" + Clinical_Treatment.params.PanelID + " #txtComments").val();
            params["mode"] = Clinical_Treatment.params.mode;
            params["PatientId"] = Clinical_Treatment.params.PatientId;
            params["TreatmentProblems"] = Clinical_Treatment.params.TreatmentProblems;
            LoadActionPan('Treatment_ProblemSelection', params);
        }
        else {
            if (AcTreatmentProblems.length == 0 && CopyOfTreatmentProblems.length > 0) {
                $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");
            }
            utility.DisplayMessages("Select atleast one Treatment", 2);
        }
    },
    checkTreatmentExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_treatment').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="TreatmentComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_treatment title="Treatment"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Treatment\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Treatment">Treatment</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Treatment\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Treatment\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_treatment> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_treatment').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());

    },
    createTreatmentBodyHTML: function (response, NoteHTMLCtrl, hideAlertMessage, commentsSOAP) {
        Clinical_Treatment.checkTreatmentExists();
        var $mainDivTreatment = $(document.createElement('div'));
        $mainDivTreatment.attr('id', "Section_Treatment");
        if (response && response.TreatmentList && response.TreatmentList.length > 0) {


            var $SectionBodyTreatment = $(document.createElement('section'));
            $SectionBodyTreatment.attr('id', "Cli_Treatment_Main" + Clinical_Treatment.params.NotesId);
            $SectionBodyTreatment.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Treatment_" + Clinical_Treatment.params.NotesId + '"><i class="fa fa-edit"></i></a>' +
         '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Treatment_Main" + Clinical_Treatment.params.NotesId + '"  onclick="javascript:void(0);"><i class="fa fa-times"></i></a></div> ');
            var $ListTreatment = $(document.createElement('ul'));
            $ListTreatment.attr('id', "Cli_Treatment" + Clinical_Treatment.params.NotesId);
            $ListTreatment.attr('class', 'list-unstyled')
            $.each(response.TreatmentList, function (index, element) {

                var $li = $(document.createElement('li'));
                $li.attr('id', "Treatment_" + element.TreatmentId);
                //$li.attr('style', "padding-left:10px");
                $li.append('<b>' + element.ProblemDescription + '</b><br>');
                $li.append(Clinical_Treatment.GetTreatmentDataHtml(response, element.TreatmentId, element.Comments));
                $ListTreatment.append($li);
            });
            $SectionBodyTreatment.append($ListTreatment);
            $mainDivTreatment.html($SectionBodyTreatment);
            Clinical_Treatment.updateTreatmenttHtml($mainDivTreatment.append(), Clinical_Treatment.params.NotesId, NoteHTMLCtrl, hideAlertMessage);
            Clinical_ProgressNote.initilizeEditableContent();
        }
        else {
            Clinical_Treatment.updateTreatmenttHtml($mainDivTreatment.append(), Clinical_Treatment.params.NotesId, NoteHTMLCtrl, hideAlertMessage, commentsSOAP);
        }
    },
    updateTreatmenttHtml: function (TreatmentHtml, NoteId, noteHTMLCtrl, hideAlertMessage, commentsSOAP) {
        if (!$(noteHTMLCtrl + ' Clinical_treatment').parent().parent().hasClass('initialVisitBody')) {
            $(noteHTMLCtrl + ' Clinical_treatment').parent().parent().addClass('initialVisitBody');
        }
        if (TreatmentHtml != '') {
            var InnerComments = "";
            if ($(noteHTMLCtrl + " #Comments_Cli_Treatment" + Clinical_Treatment.params.NotesId).length > 0) {
                InnerComments = $(noteHTMLCtrl + " #Comments_Cli_Treatment" + Clinical_Treatment.params.NotesId)[0];
            }
            $(noteHTMLCtrl + " #Section_Treatment").remove();
            $(noteHTMLCtrl + ' Clinical_treatment').parent().parent().append(TreatmentHtml);
            if (InnerComments != "") {
                $(noteHTMLCtrl + ' #Section_Treatment ul#Cli_Treatment' + Clinical_Treatment.params.NotesId).append(InnerComments);
            }
            if (commentsSOAP) {
                $('Clinical_treatment').parent().parent().append($('<section id="Treatment"><div id="Treatment"><ul class="list-unstyled"><li id="Comments_Treatment" class="mt-md" style="overflow-wrap: break-word; display: list-item;">' + commentsSOAP + '</li> </ul></div></section>'));
            }
           
        }
        Clinical_ProgressNote.saveComponentSOAPText('Treatment', hideAlertMessage);
    },
    GetTreatmentDataHtml: function (response, TreatmentId, Comments) {
        var $DetailsDiv = $(document.createElement('div'));
        $DetailsDiv.attr("style", "padding-left:10px");
        $DetailsDiv.attr("id", "Treatment_Detail_" + TreatmentId);
        var TreatmentData = $.grep(response.TreatmentDataList, function (n, i) {
            return n.TreatmentId == TreatmentId && n.ComponentName == "Prescription";
        });
        if (TreatmentData.length > 0) {
            var prescription = [];
            $.each(TreatmentData, function (i, item) {
                $.each(response.TreatementPrescriptionList, function (ii, item1) {
                    if (item.DataId == item1.PrescriptionID) {
                        prescription.push(item1);
                    }
                });
            });
            if (prescription.length > 0) {
                $DetailsDiv.append(Clinical_Treatment.GetPrescriptionHTML(prescription));
            }
        }
        TreatmentData = $.grep(response.TreatmentDataList, function (n, i) {
            return n.TreatmentId == TreatmentId && n.ComponentName == "Lab Orders";
        });
        if (TreatmentData.length > 0) {
            var LabOrderList = [];
            var LabOrderTestList = [];
            var LabOrderProblemList = [];
            $.each(TreatmentData, function (i, item) {
                $.each(response.LabOrderList, function (ii, item1) {
                    if (item.DataId == item1.LabOrderId) {
                        LabOrderList.push(item1);
                    }
                });
                $.each(response.LabOrderTestList, function (ii, item1) {
                    if (item.DataId == item1.LabOrderId) {
                        LabOrderTestList.push(item1);
                    }
                });
                $.each(response.LabOrderProblemList, function (ii, item1) {
                    if (item.DataId == item1.LabOrderId) {
                        LabOrderProblemList.push(item1);
                    }
                });
            });
            if (LabOrderList.length > 0) {
                $DetailsDiv.append(Clinical_Treatment.GetLabOrderHTML(LabOrderList, LabOrderTestList, LabOrderProblemList));
            }
        }
        TreatmentData = $.grep(response.TreatmentDataList, function (n, i) {
            return n.TreatmentId == TreatmentId && n.ComponentName == "Diagnostic Imaging Order";
        });
        if (TreatmentData.length > 0) {
            var RadiologyOrderList = [];
            var RadiologyOrderTestList = [];
            var RadiologyOrderProblemList = [];
            $.each(TreatmentData, function (i, item) {
                $.each(response.RadiologyOrderList, function (ii, item1) {
                    if (item.DataId == item1.RadiologyOrderId) {
                        RadiologyOrderList.push(item1);
                    }
                });
                $.each(response.RadiologyOrderTestList, function (ii, item1) {
                    if (item.DataId == item1.RadiologyOrderId) {
                        RadiologyOrderTestList.push(item1);
                    }
                });
                $.each(response.RadiologyOrderProblemList, function (ii, item1) {
                    if (item.DataId == item1.RadiologyOrderId) {
                        RadiologyOrderProblemList.push(item1);
                    }
                });
            });
            if (RadiologyOrderList.length > 0) {
                $DetailsDiv.append(Clinical_Treatment.GetRadiologyOrderHTML(RadiologyOrderList, RadiologyOrderTestList, RadiologyOrderProblemList));
            }
        }
        TreatmentData = $.grep(response.TreatmentDataList, function (n, i) {
            return n.TreatmentId == TreatmentId && n.ComponentName == "Procedures";
        });
        if (TreatmentData.length > 0) {
            var procedure = [];
            $.each(TreatmentData, function (i, item) {
                $.each(response.TreatmentProcedureList, function (ii, item1) {
                    if (item.DataId == item1.ProcedureId) {
                        procedure.push(item1);
                    }
                });
            });
            if (procedure.length > 0) {
                $DetailsDiv.append(Clinical_Treatment.GetProcedureHTML(procedure));
            }
        }



        TreatmentData = $.grep(response.TreatmentDataList, function (n, i) {
            return n.TreatmentId == TreatmentId && n.ComponentName == "Immunization";
        });
        var VaccineHxList = [];
        if (TreatmentData.length > 0) {
            $.each(TreatmentData, function (i, item) {

                $.each(response.VaccineHxList, function (ii, item1) {
                    if (item.DataId == item1.VaccineHxId) {
                        VaccineHxList.push(item1);
                    }
                });
            });
        }
        TreatmentData = $.grep(response.TreatmentDataList, function (n, i) {
            return n.TreatmentId == TreatmentId && n.ComponentName == "Therapeutic";
        });
        var TerapeuticList = [];
        if (TreatmentData.length > 0) {
            $.each(TreatmentData, function (i, item) {
                $.each(response.TerapeuticList, function (ii, item1) {
                    if (item.DataId == item1.ImmTherInjectionId) {
                        TerapeuticList.push(item1);
                    }
                });
            });
        }
        if (VaccineHxList.length > 0 || TerapeuticList.length > 0) {
            $DetailsDiv.append(Clinical_Treatment.GetImmunizationHTML(VaccineHxList, TerapeuticList));
        }

        TreatmentData = $.grep(response.TreatmentDataList, function (n, i) {
            return n.TreatmentId == TreatmentId && n.ComponentName == "Referrals";
        });
        if (TreatmentData.length > 0) {
            var ReferralList = [];
            $.each(TreatmentData, function (i, item) {

                $.each(response.ReferralList, function (ii, item1) {
                    if (item.DataId == item1.ReferralId) {
                        //find referral Procedure
                        var ReferralProcedure = [];
                        $.each(response.ReferralProcedureList, function (a, item2) {
                            if (item1.ReferralId == item2.ReferralId) {
                                ReferralProcedure.push(item2);
                            }
                        });
                        item1.ReferralProcedure = ReferralProcedure;
                        //find referral Problem
                        var ReferralProblemList = [];
                        $.each(response.ReferralProblemList, function (a, item2) {
                            if (item1.ReferralId == item2.ReferralId) {
                                ReferralProblemList.push(item2);
                            }
                        });
                        item1.ReferralProblemList = ReferralProblemList;
                        ReferralList.push(item1);
                    }
                });
            });
            if (ReferralList.length > 0) {
                $DetailsDiv.append(Clinical_Treatment.GetReferralHTML(ReferralList));
            }
        }
        if (Comments != "") {
            $DetailsDiv.append("<br>" + "<div>" + Comments.replace(/\n/g, '<br />') + "</div>");
        }
        return $DetailsDiv;
    },
    GetPrescriptionHTML: function (PresData) {
        var $DetailsDiv = $(document.createElement('div'));
        var $ListPrescription = $(document.createElement('ul'));
        $ListPrescription.attr('class', 'list-unstyled')

        $.each(PresData, function (index, element) {
            var ALid = element.PrescriptionID;
            $ListPrescription.append("<li> <strong>" + (element.MedicationName == null || element.MedicationName == "" ? "" : element.MedicationName + " " + ", ") + " </strong>"
                + (element.Action == null || element.Action == "" ? "" : element.Action + " ")
                + (element.Dose == null || element.Dose == "" ? "" : element.Dose + " ")
                + (element.DoseUnit == null || element.DoseUnit == "" ? "" : element.DoseUnit + " ")
                + (element.Routeby == null || element.Routeby == "" ? "" : element.Routeby + " ")
                + (element.DoseTiming == null || element.DoseTiming == "" ? "" : element.DoseTiming + " ")
                + (element.Duration == null || element.Duration == "" || parseInt(element.Duration) == 0 ? "" : " for " + element.Duration + " Day(s) ")
                + (element.Quantity == null || element.Quantity == "" ? "" : ",Quantity " + element.Quantity)
                + (element.QuantityUnit == null || element.QuantityUnit == "" ? "" : " " + element.QuantityUnit) + "(s)"
                + (element.Refill == null || element.Refill == "" ? "" : " , Refill(s) " + element.Refill)
                + (element.Substitution == null || element.Substitution == "" ? "" : ((element.Substitution).toLowerCase() == 'n' ? ', Dispense as written ' : ', Substitution permitted '))
                + (element.CreatedDate == null || element.CreatedDate == "" ? "" : (utility.RemoveTimeFromDate(null, element.CreatedDate) == null ? "" : ", Prescribed on " + utility.RemoveTimeFromDate(null, element.CreatedDate)))
                + (element.ProviderName == null || element.ProviderName == "" ? "" : " by " + element.ProviderName)
            );
        });
        $DetailsDiv.append($ListPrescription);
        return $DetailsDiv;
    },
    GetProcedureHTML: function (ProcData) {
        var $mainDivVital = $(document.createElement('div'));
        $.each(ProcData, function (index, element) {
            var color = "";
            var PLid = element.ProcedureId;

            var $DetailsDiv = $(document.createElement('div'));
            var $ListVital = $(document.createElement('ul'));
            $ListVital.attr('class', 'list-unstyled')
            var StartDateEndDate = "";
            var inActiveText = "";
            if (element.IsActive != null && element.IsActive == "False") {
                inActiveText = "<span style = 'color:red;font-weight:bold'> (Inactive) </span>";
            }
            if (element.StartDate != '' && element.StartDate != null && element.EndDate != '' && element.EndDate != null && utility.RemoveTimeFromDate(null, element.StartDate) != utility.RemoveTimeFromDate(null, element.EndDate)) {
                StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " from " + utility.RemoveTimeFromDate(null, element.StartDate)) + (element.EndDate == '' ? "" : " to " + utility.RemoveTimeFromDate(null, element.EndDate));
            }
            else if (element.StartDate == '' || element.StartDate == null && (element.EndDate != '' && element.EndDate != null)) {
                StartDateEndDate = (element.EndDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.EndDate));
            }
            else if (element.EndDate == '' || element.EndDate == null && (element.StartDate != '' && element.StartDate != null)) {
                StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.StartDate));
            }
            else if (element.EndDate == '' || element.EndDate == null && (element.StartDate == '' || element.StartDate == null)) {
                StartDateEndDate = "";
            }
            else if (element.StartDate == element.EndDate) {
                StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.StartDate));
            }
            if (element.ProcedureCodeName == '') {
                if (element.ShowCptCode == 0) {
                    if (element.CPTCode != '' && element.CPT_DESCRIPTION != '') {
                        element.ProcedureCodeName = unescape(element.CPT_DESCRIPTION);
                    }
                }
                else {
                    if (element.CPTCode != '' && element.CPT_DESCRIPTION != '') {
                        element.ProcedureCodeName = element.CPTCode + ' - ' + unescape(element.CPT_DESCRIPTION);
                    }
                }
            }
            if (element.Diagnosis == '' || element.Diagnosis == undefined) {
                element.Diagnosis = element.ICD10 == '' || element.ICD10 == null ? '' : element.ICD10 + ' - ' + unescape(element.ICD10_DESCRIPTION);
            }

            //Start 27-09-2017 Edit By Humaira Yousaf IMP-1152
            $ListVital.append("<li>" +
                //(element.ProcedureCodeName == '' ? "" : "Patient underwent " + element.ProcedureCodeName) +
                (element.ProcedureCodeName == '' ? "" : element.ProcedureCodeName) +
                (element.Diagnosis == '' || element.Diagnosis == " select  " || element.Diagnosis == " Select  " || element.Diagnosis == null || element.Diagnosis == undefined ? "" : " based on the following assessment: " + element.Diagnosis)
                //+ StartDateEndDate
                );
            //End 27-09-2017 Edit By Humaira Yousaf IMP-1152
            $ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
            if (element.ProcedureTemplateSoapTextExists) {
                ProcedureSysObservationDetail.GetProcedureTemplateSoapText(element.ProcedureId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status) {
                        $ListVital.append(utility.decodeHtml(response.ProcedureTemplateSoapText));
                    }
                });
            }
            $DetailsDiv.append($ListVital);

            $mainDivVital.append($DetailsDiv);
        });
        return $mainDivVital;
    },
    GetLabOrderHTML: function (LabOrderList, LabOrderTestList, LabOrderProblemList) {

        var sendOrderList = [];

        var MedicationsSOAPJSON = LabOrderList;
        var LabOrderTestSOAPJSON = LabOrderTestList;
        var LabOrderAssociatedProblemSOAPJSON = LabOrderProblemList;


        var $mainDivMedications = $(document.createElement('div'));
        $mainDivMedications.attr("id", "Lab_SoapText")


        var AListId = [];

        $.each(MedicationsSOAPJSON, function (index, element) {
            var LabOrderTestSoapText = "", LabOrderAssociatedProblemSoapText = "", LabOrderDate = "";
            var ALid = element.LabOrderId;
            var $SectionBodyMedications = $(document.createElement('section'));
            $SectionBodyMedications.attr("data-status", element.Status);
            $SectionBodyMedications.attr("style", "padding:0px !important");
            var $DetailsDiv = $(document.createElement('div'));
            var $ListMedications = $(document.createElement('ul'));
            var duration = "";
            $ListMedications.attr('class', 'list-unstyled')


            var obj = $('<p>');
            if (element.SoapText != "") {
                $(obj).html(element.SoapText);
                $ListMedications.append("<li>" + $(obj).html() + "</li>");
            }
            else {
                $.each(LabOrderTestSOAPJSON, function (i, item) {
                    if (element.LabOrderId == item.LabOrderId) {
                        LabOrderTestSoapText += (item.ShowCPTCode == 1 ? "<b>" + item.CPTCode + " " + item.CPTCodeDescription + "</b>" : "<b>" + item.CPTCodeDescription + "</b>")
                            + (item.Specimen != "" ? "<b> Specimen:</b> " + item.Specimen : "")
                            + (item.Volume != "" ? "<b> Volume:</b> " + item.Volume : "")
                            + (item.UrgencyName != "" ? "<b> Urgency:</b> " + item.UrgencyName : "")
                            + "<br/>";
                    }
                });

                $.each(LabOrderAssociatedProblemSOAPJSON, function (ind, value) {
                    if (element.LabOrderId == value.LabOrderId) {
                        LabOrderAssociatedProblemSoapText += (value.SoapText != "" ? "<b>" + value.SoapText + "</b>" : "") + "</br>";
                    }
                });

                if (element.CreatedOn == element.ModifiedOn) {
                    LabOrderDate = " Added On " + utility.RemoveTimeFromDate(null, element.CreatedOn);
                }
                else {
                    LabOrderDate = " Last Updated On " + utility.RemoveTimeFromDate(null, element.ModifiedOn);
                }
                $ListMedications.append("<li>" + LabOrderTestSoapText + (LabOrderAssociatedProblemSoapText != "" ? " Associated Problem(s)  <br/>" + LabOrderAssociatedProblemSoapText : "") + LabOrderDate + "</li>");

            }
            $DetailsDiv.append($ListMedications);
            $SectionBodyMedications.append($DetailsDiv);
            if (element.Status == "Pending") {
                $mainDivMedications.append($SectionBodyMedications);
            } else {
                sendOrderList.push($SectionBodyMedications);
            }



        });


        if (sendOrderList.length > 0) {
            $mainDivMedications.append("<section id='sentordertext' class='text-info'>Sent Orders</section>");
        }
        $.each(sendOrderList, function (index, element) {
            $mainDivMedications.append(element);
        });
        return $mainDivMedications;
    },
    GetRadiologyOrderHTML: function (RadiologyOrderList, RadiologyOrderTestList, RadiologyOrderProblemList) {
        var radiologyOrderSOAPJSON = RadiologyOrderList;
        var radiologyOrderTestSOAPJSON = RadiologyOrderTestList;
        var radiologyOrderProblemSOAPJSON = RadiologyOrderProblemList;



        var $mainDivRadiology = $(document.createElement('div'));
        $mainDivRadiology.attr("id", "Radiology_SoapText");

        var AListId = [];
        $.each(radiologyOrderSOAPJSON, function (index, element) {
            var RadiologyOrderTestSoapText = "", RadiologyOrderAssociatedProblemSoapText = "", RadiologyOrderDate = "";
            var ALid = element.RadiologyOrderId;
            var $SectionBodyRadiology = $(document.createElement('section'));
            $SectionBodyRadiology.attr("style", "padding:0px !important");
            var $DetailsDiv = $(document.createElement('div'));
            var $ListRadiology = $(document.createElement('ul'));
            var duration = "";
            $ListRadiology.attr('class', 'list-unstyled')


            var obj = $('<p>');
            if (element.SoapText != "") {
                $(obj).html(element.SoapText);
                $ListRadiology.append("<li>" + $(obj).html() + "</li>");
            }
            else {
                $.each(radiologyOrderTestSOAPJSON, function (i, item) {
                    if (element.RadiologyOrderId == item.RadiologyOrderId) {
                        RadiologyOrderTestSoapText += (item.ShowCPTCode == 1 ? "<b>" + item.CPTCode + " " + item.CPTCodeDescription + "</b>" : "<b>" + item.CPTCodeDescription + "</b>")
                            + (item.Specimen != "" ? "<b> Specimen:</b> " + item.Specimen : "")
                            + (item.Volume != "" ? "<b> Volume:</b> " + item.Volume : "")
                            + (item.UrgencyName != "" ? "<b> Urgency:</b> " + item.UrgencyName : "")
                            + "<br/>";
                    }
                });
                $.each(radiologyOrderProblemSOAPJSON, function (ind, value) {
                    if (element.RadiologyOrderId == value.RadiologyOrderId) {
                        RadiologyOrderAssociatedProblemSoapText += (value.SoapText != "" ? "<b>" + value.SoapText + "</b>" : "") + "</br>";
                    }
                });

                if (element.CreatedOn == element.ModifiedOn) {
                    RadiologyOrderDate = " Added On " + utility.RemoveTimeFromDate(null, element.CreatedOn);
                }
                else {
                    RadiologyOrderDate = " Last Updated On " + utility.RemoveTimeFromDate(null, element.ModifiedOn);
                }
                $ListRadiology.append("<li>" + RadiologyOrderTestSoapText + (RadiologyOrderAssociatedProblemSoapText != "" ? " Associated Problem(s)  <br/>" + RadiologyOrderAssociatedProblemSoapText : "") + (element.Comments != "" ? element.Comments : "") + RadiologyOrderDate + "</li>");

            }




            $DetailsDiv.append($ListRadiology);
            $SectionBodyRadiology.append($DetailsDiv);
            AListId.push(ALid);
            $mainDivRadiology.append($SectionBodyRadiology);
        });

        return $mainDivRadiology;

    },
    GetReferralHTML: function (ReferralList) {
        var fromprogressnote = false;
        var ReferralSoap_JSON = ReferralList;
        var $mainDivVital = $(document.createElement('div'));
        $mainDivVital.attr("id", "Referral_SoapText");

        $.each(ReferralSoap_JSON, function (index, element) {
            element.Provider = element.ProviderName;
            element.RefProvider = element.RefProviderName;
            element.Assignee = element.AssigneeName;
            var PLid = element.ReferralId;
            var $SectionBodyVital = $(document.createElement('section'));
            $SectionBodyVital.attr("style", "padding:0px !important");
            //$SectionBodyVital.attr('id', "Patient_Referrals_Main" + PLid);

            var $DetailsDiv = $(document.createElement('div'));
            //$DetailsDiv.attr('id', "Patient_Referrals_" + PLid);

            var $ListVital = $(document.createElement('ul'));
            $ListVital.attr('class', 'list-unstyled')



            var $DetailsTable = $(document.createElement('table'));
            $DetailsTable.attr('id', "Patient_ReferralsTable" + PLid);
            $DetailsTable.attr('border', '0');


            if (!fromprogressnote) {
                $DetailsTable.append("<tr><td><strong> Date:</strong></td><td>&nbsp</td><td class='pr-sm'>" + ($.datepicker.formatDate('mm/dd/yy', new Date(element.Date))) + " " + element.Time + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                $DetailsTable.append("<tr><td><strong> Category:</strong></td><td>&nbsp</td><td>" + element.Type + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
            }



            if (element.Visits != "" && element.Visits != null) {

                if (!fromprogressnote) {
                    var visitType = Patient_Referrals.getVisitName(element.Visits);

                    $DetailsTable.append("<tr>" + (visitType && visitType != "" ? "<td><strong> Type:</strong></td><td>&nbsp</td><td>" + visitType + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td></td>" + (element.SpecialityToName && element.SpecialityToName != "" ? "<td><strong class='noWordBreak'>Referral To Specialty:</strong></td><td>&nbsp</td><td>" + element.SpecialityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");

                }
            } else {
                $DetailsTable.append("<tr>" + (element.SpecialityToName && element.SpecialityToName != "" ? "<td><strong class='noWordBreak'> Referral To Specialty:</strong></td><td>&nbsp</td><td>" + element.SpecialityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td></td>" + ("" != "" ? "<td><strong class='noWordBreak'>Referral To Specialty:</strong></td><td>&nbsp</td><td>" + "" + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
            }



            if (!fromprogressnote) {
                if (element.RefProvider != "" && element.RefProvider != null) {
                    $DetailsTable.append("<tr>" + (element.RefProvider != "" ? "<td><strong> Referral To:</strong></td><td>&nbsp</td><td>" + element.RefProvider + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td>&nbsp</td>" + (element.Provider != "" ? "<td><strong> Referred From:</strong></td><td>&nbsp</td><td>" + element.Provider + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                }
                else {
                    if (element.Provider != null && typeof element.Provider != typeof undefined) {
                        $DetailsTable.append("<tr>" + (element.Provider != "" ? "<td><strong> Referred From:</strong></td><td>&nbsp</td><td>" + element.Provider + "</td>" : "") + "</tr>");
                    }
                    else {
                        $DetailsTable.append("<tr>" + (element.ProviderName && element.ProviderName != "" ? "<td><strong> Referred From:</strong></td><td>&nbsp</td><td>" + element.ProviderName + "</td>" : "") + "</tr>");
                    }
                }
            }


            if (element.Type == 'Outgoing' && !fromprogressnote) {
                if (element.FacilityFromName != "" && element.FacilityToName != "") {
                    $DetailsTable.append("<tr>" + (element.FacilityToName != "" && element.FacilityToName != null ? "<td><strong> Facility To:</strong></td><td>&nbsp</td><td>" + element.FacilityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td>&nbsp</td>" + (element.FacilityFromName != "" && element.FacilityFromName != null ? "<td><strong> Facility From:</strong></td><td>&nbsp</td><td>" + element.FacilityFromName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                }
                else if (element.FacilityFromName == "" && element.FacilityToName != "") {
                    $DetailsTable.append("<tr>" + (element.FacilityToName != "" && element.FacilityToName != null ? "<td><strong> Facility To:</strong></td><td>&nbsp</td><td>" + element.FacilityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                }
                else if (element.FacilityFromName != "" && element.FacilityToName == "") {
                    $DetailsTable.append("<tr>" + (element.FacilityFromName != "" && element.FacilityFromName != null ? "<td><strong> Facility From:</strong></td><td>&nbsp</td><td>" + element.FacilityFromName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                }

                if (element.ReferralProcedure != null && typeof element.ReferralProcedure != typeof undefined) {
                    $.each(element.ReferralProcedure, function (index, element) {
                        if (element.ShowCPTCode == 1) {
                            $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Procedure(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element.Procedure + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                        }
                        else {
                            $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Procedure(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element.CPTCodeDescription + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                        }
                    });
                }
                else {
                    if (element.Procedures != null && typeof element.Procedures != typeof undefined) {
                        $.each(element.Procedures.split(','), function (index, element) {
                            $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Procedure(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                        });
                    }
                }

                if (element.ReferralProblemList != null && typeof element.ReferralProblemList != typeof undefined) {
                    $.each(element.ReferralProblemList, function (index, element) {
                        $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Problem(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element.ProblemName + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                    });
                }
            }


            if (element.Comments != "" && element.Comments != null) {
                if (!fromprogressnote)
                    $DetailsTable.append("<tr>" + (element.Comments != "" ? "<td><strong> Comments:</strong></td><td>&nbsp</td><td>" + element.Comments + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
            }




            $ListVital.append($DetailsTable);
            $DetailsDiv.append($ListVital);
            $SectionBodyVital.append($DetailsDiv);
            $mainDivVital.append($SectionBodyVital);
        });
        return $mainDivVital;
    },
    GetImmunizationHTML: function (VaccineHxList, TerapeuticList) {
        Vaccinesoap_JSON = VaccineHxList;
        TheraeuticInjectionLoad_JSON = TerapeuticList;

        var $mainDivVital = $(document.createElement('div'));
        $mainDivVital.attr("id", "Immunization_SoapText");
        var $mainDivAdminister = $(document.createElement('div'));
        $mainDivAdminister.attr('id', "Section_VaccineAdminister");
        $mainDivAdminister.append('<h6 class="text-bold">Administration Vaccine</h6>' + "<div id='AllAdministerVaccine'></div>");
        var $mainDivDocumentHx = $(document.createElement('div'));
        $mainDivDocumentHx.attr('id', "Section_VaccineDocumentHx");
        $mainDivDocumentHx.append('<h6 class="text-bold">Document Hx Vaccine</h6>' + "<div id='AllDocumentHxVaccine'></div>");

        var $mainDivRefusal = $(document.createElement('div'));
        $mainDivRefusal.attr('id', "Section_VaccineRefusal");
        $mainDivRefusal.append('<h6 class="text-bold">Refusal Vaccine</h6>' + "<div id='AllRefusalVaccine'></div>");

        var $mainDivVoidDose = $(document.createElement('div'));
        $mainDivVoidDose.attr('id', "Section_VaccineVoidDose");
        $mainDivVoidDose.append('<h6 class="text-bold">Void Dose</h6>' + "<div id='AllVoidDoseVaccine'></div>");


        var $mainDivTherapeuticInjection = $(document.createElement('div'));
        $mainDivTherapeuticInjection.attr('id', "Section_TherapeuticInjection");
        $mainDivTherapeuticInjection.append('<h6 class="text-bold">Therapeutic Injection</h6>' + "<div id='AllTherapeuticInjection'></div>");

        var $mainDivTherapeuticInjectionHistory = $(document.createElement('div'));
        $mainDivTherapeuticInjectionHistory.attr('id', "Section_TherapeuticInjectionHistory");
        $mainDivTherapeuticInjectionHistory.append('<h6 class="text-bold">Therapeutic Injection History</h6>' + "<div id='AllTherapeuticInjectionHistory'></div>");


        var $DivNewTherapeuticInjection = $(document.createElement('div'));
        var $DivNewTherapeuticInjectionHistory = $(document.createElement('div'));
        var $DivNewAdminister = $(document.createElement('div'));
        var $DivNewDocumentHx = $(document.createElement('div'));
        var $DivNewRefusal = $(document.createElement('div'));
        var $DivNewVoidDose = $(document.createElement('div'));



        if (Vaccinesoap_JSON.length > 0 || ((TheraeuticInjectionLoad_JSON.length > 0))) {
            var PListId = [];
            var def = [];

            $.each(Vaccinesoap_JSON, function (index, element) {
                var color = "";
                var PLid = element.VaccineHxId;
                var $SectionBodyVital = $(document.createElement('section'));
                var $DetailsDiv = $(document.createElement('div'));
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')


                if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        ((element.CPT == '' || element.CPT == null) ? "" : " <b>(" + element.CPT.substring(0, element.CPT.length - 1) + ") </b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                        (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))

                        );
                }
                else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                      (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                        );
                }
                else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.RefusalReason == '' ? "" : ", not given to the patient due to " + element.RefusalReason)
                        );
                }
                else if (element.VoidDose == "True" || element.VoidDose == true) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A"))) +
                        (element.Comments == '' ? "" : "</br> Reason: " + element.Comments)
                        );
                }


                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);

                var MainDivId = "";
                if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllAdministerVaccine";
                }
                else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllDocumentHxVaccine";
                }
                else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllRefusalVaccine";
                }
                else if (element.VoidDose == "True" || element.VoidDose == true) {
                    MainDivId = "AllVoidDoseVaccine";
                }
                if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $DivNewAdminister.append($SectionBodyVital);
                }
                else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {

                    $DivNewDocumentHx.append($SectionBodyVital);
                }
                else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $DivNewRefusal.append($SectionBodyVital);
                }
                else if (element.VoidDose == "True" || element.VoidDose == true) {
                    $DivNewVoidDose.append($SectionBodyVital);
                }
            });






            $.when.apply($, def).done(function ($n) {
                if (TheraeuticInjectionLoad_JSON.length > 0) {

                    $.each(TheraeuticInjectionLoad_JSON, function (index, element1) {
                        var color = "";


                        var TheraInjectionid = element1.ImmTherInjectionId + "thera";
                        var $SectionBodyVital = $(document.createElement('section'));
                        var $DetailsDiv = $(document.createElement('div'));
                        var $ListVital = $(document.createElement('ul'));

                        $ListVital.attr('class', 'list-unstyled')

                         var _InjecNameAndCode = (element1.CPTCode == '' ? (element1.TherapeuticInjection == '' ? "" : element1.TherapeuticInjection): element1.CPTCode + " - " +element1.TherapeuticInjection);
                        $ListVital.append("<li>" + "<b>" + _InjecNameAndCode + "</b>" +
                       (element1.Dose == '' ? "" : ", Dose: " +element1.Dose + " " + element1.Amount) +
                         (!(element1.RouteDescription) ? "": ", Route: " +element1.RouteDescription) +
                         (!(element1.SiteDescription) ? "": ", Site: " +element1.SiteDescription) +
                          (!(element1.LotNumber) ? "": ", Lot Number: " +element1.LotNumber) +
                          (!(element1.ManufacturerName) ? "": ", Manufacturer: " +element1.ManufacturerName) +
                        (element1.ProviderName == '' ? "": ", administrated by " +element1.ProviderName) +
                         (element1.AdministrationDate == '' ? "": " on " +(moment(element1.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                       );



                        $DetailsDiv.append($ListVital);
                        $SectionBodyVital.append($DetailsDiv);

                        if (element1.Type == "Administered") {

                            $DivNewTherapeuticInjection.append($SectionBodyVital);

                        }
                        else {
                            $DivNewTherapeuticInjectionHistory.append($SectionBodyVital);
                        }

                    });

                    if ($DivNewTherapeuticInjectionHistory.html() != '' || $DivNewTherapeuticInjection.html() != '' || $DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {

                        if ($DivNewAdminister.html() != '') {

                            $mainDivVital.append($mainDivAdminister);
                            $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);

                        }
                        if ($DivNewDocumentHx.html() != '') {
                            $mainDivVital.append($mainDivDocumentHx);
                            $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                        }

                        if ($DivNewRefusal.html() != '') {
                            $mainDivVital.append($mainDivRefusal);
                            $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);

                        }

                        if ($DivNewVoidDose.html() != '') {
                            $mainDivVital.append($mainDivVoidDose);
                            $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);

                        }
                        if ($DivNewTherapeuticInjection.html() != '') {
                            $mainDivVital.append($mainDivTherapeuticInjection);
                            $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                        }
                        if ($DivNewTherapeuticInjectionHistory.html() != '') {
                            $mainDivVital.append($mainDivTherapeuticInjectionHistory);
                            $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                        }
                    }
                }
                else {
                    if ($DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {
                        if ($DivNewAdminister.html() != '') {

                            $mainDivVital.append($mainDivAdminister);
                            $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);

                        }
                        if ($DivNewDocumentHx.html() != '') {
                            $mainDivVital.append($mainDivDocumentHx);
                            $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                        }

                        if ($DivNewRefusal.html() != '') {
                            $mainDivVital.append($mainDivRefusal);
                            $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);

                        }

                        if ($DivNewVoidDose.html() != '') {
                            $mainDivVital.append($mainDivVoidDose);
                            $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);

                        }
                    }
                }
            });
        }

        return $mainDivVital
    },
    detachTreatment: function (TreatmentId) {
        utility.myConfirm('28', function () {
            EMRUtility.scrollToPNcomponent('clinical_treatment');
            var selectedValue = TreatmentId.replace('Cli_Treatment_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Treatment.detachTreatmentFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .TreatmentComponent').attr('NoteComponentId');
                        $('#' + TreatmentId).remove();
                        if ($('#' + Clinical_ProgressNote.params["PanelID"]).find('#Section_Treatment').length > 0) {
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('#Section_Treatment').remove();
                        }
                        Clinical_ProgressNote.saveComponentSOAPText('Treatment');
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
    },
    detachTreatmentFromNotes_DBCall: function (NotesId) {
        var objData = {};
        objData["NoteId"] = NotesId;
        objData["commandType"] = "detach_Treatment_from_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "Treatment");
    },
    ExpandTheGrid: function () {
        if (Clinical_Treatment.params.IsExpand) {
            $($("#" + Clinical_Treatment.params.PanelID).find($('[data-plugin-toggle]'))).each(function () {
                var $this = $(this);
                if ($this.find("section div.toggle-content tbody tr").length > 0) {
                    if ($this.find("section div.toggle-content tbody tr td").length > 1) {
                        $this.find("section").addClass("active");
                        $this.find("section div.toggle-content").css("display", "block");
                    }
                }
                else {
                    $this.find("section").removeClass("active");
                    $this.find("section div.toggle-content").css("display", "none");
                }
            });
        }
        else {
            Clinical_Treatment.params.IsExpand = true;
        }

    },

    //This Function detach Problem list From progress note
    detach_ComponentsTreatment: function (ComponentName, IsUpdate, ImmunizationComponentRemove) {
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_treatment').parent().parent().attr('NoteComponentId');
        if (NoteComponentId && NoteComponentId != "NCDummyId") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_treatment').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Treatment', true))
            }
            else
                promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
            $.when.apply($, promise).done(function () {
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_treatment').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Treatment']").remove();
            });
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Treatment']").remove();
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_treatment').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Treatment', true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_treatment').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            });
        }
        var TreatmentId = 'Cli_Treatment_Main' + Clinical_Treatment.params.NotesId;
        var selectedValue = TreatmentId.replace('Cli_Treatment_Main', '');
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_Treatment.detachTreatmentFromNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .TreatmentComponent').attr('NoteComponentId');
                    $('#' + TreatmentId).remove();
                    if ($('#' + Clinical_ProgressNote.params["PanelID"]).find('#Section_Treatment').length > 0) {
                        $('#' + Clinical_ProgressNote.params["PanelID"]).find('#Section_Treatment').remove();
                    }
                    Clinical_ProgressNote.saveComponentSOAPText('Treatment');
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    LoadTreatment: function () {
        var dfd = $.Deferred();
        var strMessage = "";


        Clinical_Treatment.searchTreatment().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.each(response.TreatmentData.TreatmentList, function (i, data) {
                    var item = {};
                    item.ProblemId = data.ProblemListId;
                    item.TreatmentId = data.TreatmentId;
                    item.ProblemDescription = data.ProblemDescription;
                    item.Comments = data.Comments
                    var Prescription = [];
                    var LabOrders = [];
                    var DiagnosticImagingOrder = [];
                    var Procedures = [];
                    var Immunization = [];
                    var Therapeutic = [];
                    var Referrals = [];
                    $.each(response.TreatmentData.TreatmentDataList, function (ii, data1) {
                        if (data1.ComponentName == "Prescription" && data1.TreatmentId == data.TreatmentId) {
                            Prescription.push(data1.DataId);
                        }
                        if (data1.ComponentName == "Lab Orders" && data1.TreatmentId == data.TreatmentId) {
                            LabOrders.push(data1.DataId);
                        }
                        if (data1.ComponentName == "Diagnostic Imaging Order" && data1.TreatmentId == data.TreatmentId) {
                            DiagnosticImagingOrder.push(data1.DataId);
                        }
                        if (data1.ComponentName == "Procedures" && data1.TreatmentId == data.TreatmentId) {
                            Procedures.push(data1.DataId);
                        }
                        if (data1.ComponentName == "Immunization" && data1.TreatmentId == data.TreatmentId) {
                            Immunization.push(data1.DataId);
                        }
                        if (data1.ComponentName == "Therapeutic" && data1.TreatmentId == data.TreatmentId) {
                            Therapeutic.push(data1.DataId);
                        }
                        if (data1.ComponentName == "Referrals" && data1.TreatmentId == data.TreatmentId) {
                            Referrals.push(data1.DataId);
                        }
                    });
                    item.PrescriptionIds = Prescription.join(',');
                    item.LabOrderIds = LabOrders.join(',');
                    item.DiagnosticImagingIds = DiagnosticImagingOrder.join(',');
                    item.ProcedureIds = Procedures.join(',');
                    item.ImmunizationIds = Immunization.join(',');
                    item.TherapeuticIds = Therapeutic.join(',');
                    item.ReferralIds = Referrals.join(',');
                    Clinical_Treatment.params.TreatmentProblems.push(item);
                });
                //select first treatment default
                if (Clinical_Treatment.params.TreatmentProblems.length > 0) {
                    $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody").find("tr#" + Clinical_Treatment.params.TreatmentProblems[0].ProblemId).trigger("click");
                }
                else {
                    if ($("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr").length > 0 && $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr:first td").length>1) {
                        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr:first").trigger("click");
                    }
                }
                Clinical_Treatment.params.ActualTreatments = JSON.parse(JSON.stringify(Clinical_Treatment.params.TreatmentProblems));
                dfd.resolve();
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(strMessage, 3);
            }

        });
        return dfd;
    },
    searchTreatment: function (prescriptionId, pageNumber, rowsPerPage) {
        var objData = {};
        objData["NoteId"] = Clinical_Treatment.params.NotesId;
        objData["commandType"] = "SEARCH_Treatment";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "Treatment");
    },
    ////////////////////////////////////////////////OpenComponents///////////////////////////////////////////////////
    AddProblem: function () {
        var params = [];
        if (Clinical_Treatment.params.ParentCtrl && Clinical_Treatment.params.ParentCtrl == "clinicalTabProgressNote") {
            params["IsFromNote"] = true;
        }
        params["patientID"] = Clinical_Treatment.params.PatientId;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_Treatment";
        params["mode"] = "Add";
        params["PatientId"] = Clinical_Treatment.params.PatientId;
        params["ProblemListId"] = "-1";
        params["CurrentNotesProviderId"] = Clinical_Treatment.params["CurrentNotesProviderId"];
        if (Clinical_ProblemLists.bIsFirstLoad == false) {
            $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
            RemoveAdminTab('clinicalTabProblemLists');
        }
        LoadActionPan('Clinical_ProblemLists', params);
    },
    AddPrescription: function (e) {
        if (e) {
            e.stopPropagation();
        }
        var params = [];
        params["patientID"] = Clinical_Treatment.params.PatientId;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_Treatment";
        params["mode"] = "Add";
        params["PatientId"] = Clinical_Treatment.params.PatientId;
        params["MedicationsId"] = "-1";
        params["MedicationsTab"] = "Prescription";
        params["PrescriptionId"] = "-1";
        LoadActionPan('Clinical_Medications', params);
    },
    AddProcedure: function (e) {
        if (e) {
            e.stopPropagation();
        }
        var params = [];
        params["patientID"] = Clinical_Treatment.params.PatientId;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_Treatment";
        params["mode"] = "Add";
        params["PatientId"] = Clinical_ProgressNote.params.patientID;
        params["ProceduresId"] = "-1";
        params["PrPanelID"] = "pnlClinicalProgressNote #pnlClinicalTreatment";
        LoadActionPan('Clinical_Procedures', params);
    },
    AddLabOrder: function (LabOrderId, e) {
        if (e) {
            e.stopPropagation();
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["isProblemAdded"] = false;

                if (LabOrderId != null && parseInt(LabOrderId) > 0) {
                    params["LabOrderId"] = LabOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabOrderId"] = -1;
                    params["mode"] = "Add";

                }
                params["LastLabName"] = "";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = 'Clinical_Treatment';
                params["ParentCtrlPanelID"] = Clinical_Treatment.params.PanelID;
                LoadActionPan('ClinicalLabOrderDetail', params, Clinical_Treatment.params.PanelID);
            }
        });
    },
    AddRadiologyOrder: function (radiologyOrderId, e) {
        if (e) {
            e.stopPropagation();
        }
        var strMessage = "";
        var permissionState = radiologyOrderId != null && parseInt(radiologyOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["isProblemAdded"] = false;
                params["FromAdmin"] = 0;
                if (radiologyOrderId != null && parseInt(radiologyOrderId) > 0) {
                    params["RadiologyOrderId"] = radiologyOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyOrderId"] = -1;
                    params["mode"] = "Add";
                    params["LastRadiologyLabId"] = "";
                }
                params["ParentCtrl"] = 'Clinical_Treatment';
                params["ParentCtrlPanelID"] = Clinical_Treatment.params.PanelID;
                LoadActionPan('ClinicalRadiologyOrderDetail', params, Clinical_Treatment.params.PanelID);
            }
        });


    },
    AddReferral: function (ReferralId, e) {
        if (e) {
            e.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = "Clinical_Treatment";
        params["ParentCtrlPanelID"] = Clinical_Treatment.params.PanelID;
        params["FromAdmin"] = 0;
        if (ReferralId != null) {
            params["ReferralId"] = ReferralId;
            params["mode"] = "Edit";
        }
        else {
            params["mode"] = "Add";
        }
        params["PatientId"] = Clinical_Treatment.params.PatientId;
        LoadActionPan("Patient_Referrals_Outgoing_Detail", params, Clinical_Treatment.params.PanelID);
    },
    addTherapeutic: function (e, TherapeuticId, Mode, Type) {
        if (e) {
            e.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                PARAMS = {}
                PARAMS["ParentCtrl"] = 'Clinical_Treatment';
                PanelID = 'pnlClinicalProgressNote #pnlClinicalTreatment';
                PARAMS["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalTreatment';
                PARAMS["ParentPanelID"] = 'pnlClinicalProgressNote #pnlClinicalTreatment';
                PARAMS["PatientId"] = Clinical_Treatment.params.PatientId;
                PARAMS["FromAdmin"] = 0;
                PARAMS["NotesId"] = 0;
                PARAMS["mode"] = Mode;
                if (typeof Type != typeof undefined) {
                    PARAMS["Type"] = Type;
                }
                PARAMS["ImmTherInjectionId"] = TherapeuticId;
                LoadActionPan("Immunization_TherapeuticInjection", PARAMS, PanelID);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    AddImmunization: function (Mode, e, VaccineCategoryId, VaccineScheduleId, TabId, VaccineHxId, Type, g, VaccineCategory, OrderSetId) {
        if (e) {
            e.stopPropagation();
        }
        var MODE = "ADD";
        if (Mode == "Edit") {
            MODE = "EDIT";
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", MODE, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                var PanelID = "";
                params["ParentCtrl"] = 'Clinical_Treatment';
                PanelID = 'pnlClinicalProgressNote #pnlClinicalTreatment';
                params["from"] = 'Clinical_Treatment';
                params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalTreatment';
                params["ParentPanelID"] = 'pnlClinicalProgressNote #pnlClinicalTreatment';
                if (typeof OrderSetId != typeof undefined && OrderSetId != null) {
                    params["OrderSetId"] = OrderSetId;
                }
                else {
                    params["OrderSetId"] = "";
                }
                params["FromAdmin"] = 0;
                params["VaccineScheduleId"] = VaccineScheduleId;
                params["CategoryId"] = VaccineCategoryId;//category Name
                params["Category"] = VaccineCategory;
                if (Mode == "Edit") {
                    params["VaccineHxId"] = VaccineHxId;
                    params["Type"] = Type;
                }
                params["TabId"] = "Clinical_Treatment";
                params["mode"] = Mode;
                params["patientID"] = Clinical_Treatment.params.PatientId;
                LoadActionPan('Clinical_ImmunizationDetail', params, PanelID);
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    ///////////////////////////////////////////////////////////////////prescription-section///////////////////////////////////////////////////////////////////////
    prescriptionSearch: function (prescriptionId, pageNo, rpp, fromPagination) {
        var dfd = $.Deferred();
        var strMessage = "";


        Clinical_Treatment.searchPrescriptions(prescriptionId, pageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Treatment.isViewed = false;
                $.when(Clinical_Treatment.prescriptionsGridLoad(response, fromPagination)).then(function () {
                    dfd.resolve();
                });
                var TableControl = Clinical_Treatment.params.PanelID + " #dgvPrescriptionsT";
                var PagingPanelControlID = Clinical_Treatment.params.PanelID + " #divPrescriptions_PagingT";
                var ClassControlName = "Clinical_Treatment";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.PrescriptionCount, pageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (primaryID, pageNumber, resultPerPage) {
                    Clinical_Treatment.prescriptionSearch(primaryID, pageNumber, resultPerPage, true);
                }), 10);

            }
            else {
                dfd.resolve();
                utility.DisplayMessages(strMessage, 3);
            }

        });
        return dfd;
    },
    searchPrescriptions: function (prescriptionId, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = {};
        objData["PrescriptionId"] = prescriptionId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["isViewed"] = false;
        objData["commandType"] = "SEARCH_PRESCRIPTIONS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Prescriptions");
    },
    prescriptionsGridLoad: function (response, fromPagination) {
        var dfd = $.Deferred();
        if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #pnlPrescriptions_ResultT #dgvPrescriptionsT")) {
            $("#" + Clinical_Treatment.params.PanelID + "  #pnlPrescriptions_ResultT #dgvPrescriptionsT").dataTable().fnClearTable();
            $("#" + Clinical_Treatment.params.PanelID + "  #pnlPrescriptions_ResultT #dgvPrescriptionsT").dataTable().fnDestroy();
            $("#" + Clinical_Treatment.params.PanelID + "  #pnlPrescriptions_ResultT #dgvPrescriptionsT tbody").find("tr").remove();
        }
        else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvPrescriptionsT') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_Treatment.params.PanelID + " #pnlPrescriptions_ResultT #dgvPrescriptionsT tbody").find("tr").remove();
                    $("#" + Clinical_Treatment.params.PanelID + " #pnlPrescriptions_ResultT #dgvPrescriptionsT").parent().parent().find('div.row').remove();
                }
            });
        }


        if (response.PrescriptionCount > 0) {
            var PrescriptinosLoadJSONData = JSON.parse(response.PrescriptionLoad_JSON);
            $.each(PrescriptinosLoadJSONData, function (i, item) {

                var serachstr = item.NDCID;
                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(serachstr, "Clinical_Treatment", 1);
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvPrescription_row" + item.PrescriptionID + "'))");
                $row.attr("id", "gvPrescription_row" + item.PrescriptionID);
                $row.attr("PrescriptionId", item.PrescriptionID);
                $row.attr("status", item.Status);


                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var comma = ",";
                if (item.PharmacyName == "") {
                    comma = "";
                }
                var checkbox = '<td class="center"><input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordPrescriptions" ArrayProperty="PrescriptionIds" name="selectRecordPrescriptions" class="input-block text-center" coltype="checkbox" id="selectRecordPrescriptions_' + item.PrescriptionID + '"/></td>';
                var actions = '&nbsp;<a class="btn btn-xs " href="javascript:void(O)" title="Record History" onclick="Clinical_Treatment.rowHistory(' + item.PrescriptionID + ');"> <i class="fa fa-history blue"></i></a>'
                //$row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.PrescriptionID + '</td><td>' + actions + '</td> <td>' + item.MedicationName + " " + $infoButtonrow + '</td><td>' +
                $row.append('<td style="display:none;">' + item.PrescriptionID + '</td>' + checkbox + '<td>' + actions + '</td> <td>' + item.MedicationName + " " + '</td><td>' +
                item.PharmacyName + comma + item.PharmacyAddress + comma + item.PharmacyCity + comma + item.PharmacyState + item.PharmacyZip + '</td>' + '</td><td>' + item.Refill + '</td>' + '</td><td>' + item.Status + '</td>' + '<td>' + utility.RemoveTimeFromDate(null, item.CreatedDate) + '</td>');
                $("#" + Clinical_Treatment.params.PanelID + ' #pnlPrescriptions_ResultT #dgvPrescriptionsT tbody').last().append($row);
            });
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + ' #pnlPrescriptions_ResultT #dgvPrescriptionsT').DataTable({
                "language": {
                    "emptyTable": "No Prescriptions"
                }, "bDestroy": true, "autoWidth": false, "bLengthChange": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($('#' + Clinical_Treatment.params.PanelID + " #pnlPrescriptions_ResultT #dgvPrescriptionsT_wrapper").length > 0) {
        }
        else {
            $('#' + Clinical_Treatment.params.PanelID + " #pnlPrescriptions_ResultT #dgvPrescriptionsT").DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }] }); // to remove records per page dropdown
        }
        if (fromPagination) {
            Clinical_Treatment.params.IsExpand = false;
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");

        dfd.resolve();
        return dfd;
    },
    rowHistory: function (PrescriptionId) {
        var currentPrescriptionId = PrescriptionId != null ? PrescriptionId : -1;
        if (currentPrescriptionId > 0) {
            Clinical_Treatment.ShowHistory(currentPrescriptionId);
        }
    },
    ShowHistory: function (PrescriptionId) {
        EMRUtility.showCurrentItemHistory(Clinical_Treatment.params.PanelID, null, PrescriptionId, "Prescription", Clinical_Treatment.params.patientID, Clinical_Treatment.params.ParentCtrl != "clinicalTabProgressNote" ? Clinical_Treatment.params.TabID : "Clinical_Treatment", null);
    },
    ///////////////////////////////////////////////////////////////////Lab Order //////////////////////////////////////////////////////////////////////////////////
    LabOrderSearch: function (LabId, PageNo, rpp, caller, OrderStatus, fromPagination) {
        var strMessage = "";


        if ($("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT").css("display") == "none") {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT").show();
        }

        var self = $("#" + Clinical_Treatment.params.PanelID + " form");

        Clinical_Treatment.searchLabOrder(null, LabId, PageNo, rpp, OrderStatus).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (OrderStatus != "Signed") {
                    Clinical_Treatment.LabGridLoad(response, fromPagination);
                    var TableControl = Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT #dgvLabOrderT";
                    var PagingPanelControlID = Clinical_Treatment.params.PanelID + " #dgvLabOrderT_Paging";
                    var ClassControlName = "Clinical_Treatment";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    Clinical_Treatment.pending = "Pending";
                    setTimeout(
                        CreatePagination(response.LabOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_Treatment.LabOrderSearch(null, PageNumber, ResultPerPage, null, Clinical_Treatment.pending, true);
                        }), 10);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });


    },
    searchLabOrder: function (LabOrderData, LabOrderId, PageNumber, RowsPerPage, OrderStatus) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (LabOrderData != null) {
            objData = JSON.parse(LabOrderData);
        }
        objData["LabOrderId"] = LabOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_Treatment.params.PatientId;


        //    objData["Test"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #txtCPTCode').val();
        //    objData["LabId"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #ddlLaboratory').val();
        //    objData["ProviderId"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #ddlProvider').val();
        //    objData["OrderFromDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #dpStartDate').val();
        //    objData["OrderToDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"]  #frmClinicalLabOrder #dpToDate').val();
        //    objData["OrderNo"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"]  #frmClinicalLabOrder #txtOrderNumber').val();
        //}



        if (OrderStatus != null) {
            if (OrderStatus == "Pending") {
                objData["Status"] = "Pending";
            }
            else {
                objData["Status"] = "Transmitted";
            }
        }
        else {
            objData["Status"] = "Pending";
        }
        objData["commandType"] = "search_laborders_dashboard";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },
    checkUncheckAllLabOrder: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Treatment.params.PanelID + " [name='selectRecordLabOrder']").prop("checked", true);
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + " [name='selectRecordLabOrder']").prop("checked", false);
        }

        $("#" + Clinical_Treatment.params.PanelID + " #dgvLabOrderT tbody").find('input[type="checkbox"]').each(function () {
            Clinical_Treatment.CheckUncheck(this);
        });
    },
    enableAddLabOrder: function () {
        var LabId = "";
        $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT tbody tr input:checked").each(function (i, item) {

            LabId += "," + $(item).attr('id');
        });
        var item = {};
        item.LabOrderIds = LabId;
        Clinical_Treatment.params.TreatmentProblems.push(item);

    },
    LabGridLoad: function (response, fromPagination) {


        Clinical_Treatment.labOrderRows = [];

        if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT #dgvLabOrderT")) {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT").dataTable().fnClearTable();
            $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT").dataTable().fnDestroy();
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT tbody").find("tr").remove();
        if ($("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT thead tr #SelectRecord").length == 0) {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxLabOrder" id="chkHeaderLabOrder" onchange="Clinical_Treatment.checkUncheckAllLabOrder(this);"  class="input-block" coltype="checkbox"/> </th>');
        } else {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT thead tr #SelectRecord").prop('checked', false);
        }
        if (response.LabOrderCount > 0) {
            var LabLoadJSONData = JSON.parse(response.LabOrderFill_JSON);
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvLab_row" + item.LabOrderId);
                $row.attr("LabId", item.LabOrderId);
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var onclick = "Clinical_Treatment.labOrderRowExpand(this,event);";
                var onCellClick = "Clinical_Treatment.labOrderRowExpand(this,event,null,'cell');";
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                var checkbox = '<input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordLabOrder" ArrayProperty="LabOrderIds" name="selectRecordLabOrder" class="input-block text-center" coltype="checkbox" laborderid="' + item.LabOrderId + '" id="selectRecordLabOrder_' + item.LabOrderId + '"/>';
                var editMode = 'onclick="Clinical_Treatment.AddLabOrder(\'' + item.LabOrderId + '\',event);"';
                var SelectionCheckBoxColumn = '<input type="checkbox" class="pull-left mt-default" id="' + item.LabOrderId + '" name="SelectCheckBoxLabOrder" onchange="Clinical_Treatment.CheckUncheck(this);"  class="input-block text-center"/>';

                $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td class="Actions_">' + checkbox + '</td><td ' + editMode + '>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick = "' + onCellClick + '">' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.AssigneeName + '</td>');

                $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT" + " tbody").last().append($row);
                var childRows = Clinical_Treatment.buildLabOrderRowChild(item.LabOrderTests, item.LabOrderId);
                Clinical_Treatment.labOrderRows.push({ row: $row, childs: childRows });
                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);
                });
            });


        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + ' #pnlLabOrder_ResultT #dgvLabOrderT').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Order Found"
                }, "autoWidth": false, "bFilter": false, "bLengthChange": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + ' #pnlLabOrder_ResultT #dgvLabOrderT'))
            ;
        else {
            Clinical_Treatment.EditableGridOrder = $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT").DataTable({
                "destroy": true,
                "bInfo": false, "bFilter": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }]
            }); // to remove records per page dropdown
        }

        setTimeout(function () {
            $('#' + Clinical_Treatment.params.PanelID + ' #dgvLabOrderT input[name=SelectCheckBoxLabOrder]').prop("checked", false);
        }, 200);
        EMRUtility.fixDataTableDuplication("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT");

        $.each(Clinical_Treatment.labOrderRows, function (i, item) {

            if (Clinical_Treatment.EditableGridOrder != null) {

                var row = Clinical_Treatment.EditableGridOrder.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                }
            }
        });
        if (fromPagination) {
            Clinical_Treatment.params.IsExpand = false;
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");
    },
    buildLabOrderRowChild: function (tests, labOrderId) {
        var CurrentRowchilds = $();
        var templateHtml = $("#" + Clinical_Treatment.params.PanelID + " #LabOrderTemplateT").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabOrderTemplateT').find('tbody');
            $.each(tests, function (i, item) {
                var i = 1;
                do {
                    var $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    if (i == 1) {
                        $ChilRowDetail.append('<td class="bold" colspan="2" >' + item.CPTCode + " " + item.CPTDescription + '</td> <td></td>');
                    }
                    else {
                    }
                    $tbody.append($ChilRowDetail);
                    i++;
                } while (i <= 2)
            });
        }

        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="9">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);
    },
    labOrderRowExpand: function ($row, event, gridType, from) {

        event.stopPropagation();
        event.preventDefault();
        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');
        var $actions,
         values = [];
        var row = null;
        if (gridType != null) {
            row = Clinical_Treatment.EditableGridOrderSent.row($row);
        }
        else {
            row = Clinical_Treatment.EditableGridOrder.row($row);
        }
        if (row.child.isShown()) {
            $row.find("td:eq(3) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
        }
        else {
            $row.find("td:eq(3) .fa-plus-square").attr("class", "fa fa-minus-square");
            row.child.show();
        }
       
        $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT" + " tbody tr").each(function (i, item) {
            if (currentRowId != $(item).attr('id')) {
                var allotherrows = Clinical_Treatment.EditableGridOrder.row(item);
                if (allotherrows.child.isShown()) {
                    $(item).find(".fa-minus-square").attr("class", "fa fa-plus-square");
                    allotherrows.child.hide();

                }
            }
        });

    },
    LabOrderDelete: function (event) {
        var strMessage = "";
        event.stopPropagation();
        event.preventDefault();
        var LabId = "";
       
        if ($("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any order to delete', 4);
            return;
        } else {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlLabOrder_ResultT  #dgvLabOrderT tbody tr input:checked").each(function (i, item) {
                LabId += "," + $(item).attr('laborderid');
            });
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_Treatment.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();
                        Clinical_Treatment.deleteLabOrder(myJSON, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_Treatment.LabOrderSearch(null, null, null, null, "Pending");
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
        });
    },
    deleteLabOrder: function (LabData, LabId, PageNumber, RowsPerPage, PatientId) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(LabData);
        objData["LabOrderId"] = LabId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["commandType"] = "delete_Laborder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },
    /////////////////////////////////////////////////////////////////// Diagnostic Imaging //////////////////////////////////////////////////////////////////////////////////
    radiologyOrderSearch: function (radiologyId, PageNo, rpp, caller, fromPagination) {
        var strMessage = "";
        $("#" + Clinical_Treatment.params.PanelID + " #dgvRadiologyOrderT th#selectRecordOrders").remove();
        if ($("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT").css("display") == "none") {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT").show();
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + Clinical_Treatment.params.PanelID + " form");
                var myJSON = self.getMyJSONByName();
                Clinical_Treatment.searchRadiologyOrder(myJSON, radiologyId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_Treatment.radiologyGridLoad(response, fromPagination);
                        var TableControl = Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT";
                        var PagingPanelControlID = Clinical_Treatment.params.PanelID + " #dgvRadiologyOrderT_Paging";
                        var ClassControlName = "Clinical_Treatment";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.radiologyOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_Treatment.radiologyOrderSearch(PrimaryID, PageNumber, ResultPerPage, null, true);
                            }), 10);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    radiologyGridLoad: function (response, fromPagination) {

        if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT")) {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT").dataTable().fnClearTable();
            $("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT").dataTable().fnDestroy();
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT tbody").find("tr").remove();

        if (response.radiologyOrderCount > 0) {
            var radiologyLoadJSONData = JSON.parse(response.RadiologyLoad_JSON); //Parsing array to JSON
            $.each(radiologyLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvRadiology_row" + item.RadiologyOrderId);
                $row.attr("RadiologyId", item.RadiologyOrderId);
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var SelectionCheckBoxColumn = '<td class="center"><input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordDiagnosticImaging" ArrayProperty="DiagnosticImagingIds" name="selectRecordDiagnosticImaging" class="input-block text-center" coltype="checkbox" id="selectRecordDiagnosticImaging_' + item.RadiologyOrderId + '"/></td>';

                if (item.Status != "Signed") {
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td class="Actions_"><a class="btn btn-xs" href="#" onclick="Clinical_Treatment.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Treatment.AddRadiologyOrder(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.AssigneeName + '</td>');
                    //AST_331 by:MAHMAD
                    $("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT tbody").last().append($row);
                    //AST_331 by:MAHMAD
                }
            });
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + ' #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT').DataTable({
                "language": {
                    "emptyTable": "No Diagnostic Imaging Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[2, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + ' #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT'))
            ;
        else {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT #dgvRadiologyOrderT").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }] }); // to remove records per page dropdown
        }
        EMRUtility.fixDataTableDuplication("#" + Clinical_Treatment.params.PanelID + " #pnlRadiologyOrder_ResultT");
        if (fromPagination) {
            Clinical_Treatment.params.IsExpand = false;
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");
    },
    searchRadiologyOrder: function (radiologyOrderData, radiologyOrderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        objData["RadiologyOrderId"] = radiologyOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        // objData["Test"] = objData["CPTCode"];

        //var divId = " #pnlRadiologyOrder_Search"
        //if (type == "External") {
        //    divId = " #pnlExternalRadiologyOrder_Search"
        //}
        //objData["ProviderId"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #txtProvider').val();

        /* Start 27/05/2015 Abid Ali / labId for radiology order */
        //objData["LabId"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId +' #frmClinicalRadiologyOrder #ddlLaboratory').val();
        /* End 27/05/2015 Abid Ali / labId for radiology order */

        //objData["OrderFromDate"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #dpStartDate').val();
        //objData["Status"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #ddlStatus option:selected').val();
        //objData["OrderToDate"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #dpToDate').val();
        //objData["OrderNo"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #txtOrderNumber').val();
        //objData["RadiologyType"] = type;
        objData["RadiologyType"] = "both";
        objData["commandType"] = "search_radiologyorders";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },
    radiologyOrderDelete: function (radiologyId, event, PageNo, rpp) {
        var strMessage = "";
        event.stopPropagation();
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = radiologyId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_Treatment.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        Clinical_Treatment.deleteRadiologyOrder(myJSON, radiologyId, PageNo, rpp).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_Treatment.radiologyOrderSearch("", 1, 15);
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
        });
    },

    deleteRadiologyOrder: function (radiologyData, radiologyId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(radiologyData);
        objData["RadiologyOrderId"] = radiologyId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_RadiologyOrder.patientId;
        objData["commandType"] = "delete_radiologyorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },
    /////////////////////////////////////////////////////////////////// Referral //////////////////////////////////////////////////////////////////////////////////
    LoadReferrals: function (PrimaryID, PageNumber, ResultPerPage, fromPagination) {
        var PnlResult = "pnlOutgoingReferal_ResultT";
        var dgvDivId = "dgvOutgoingReferralT";
        var pagingDivId = "dgvOutgoingReferral_PagingT";
        Clinical_Treatment.SearchReferral(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Treatment.ReferralGridLoadNew(response, fromPagination);
                var TableControl = Clinical_Treatment.params.PanelID + " #" + pagingDivId;
                var PagingPanelControlID = Clinical_Treatment.params.PanelID + " #" + pagingDivId;
                var ClassControlName = "Clinical_Treatment";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ReferralListCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_Treatment.ReferralSearch(PrimaryID, PageNumber, ResultPerPage, true);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    ReferralGridLoadNew: function (response, fromPagination) {
        var PnlResult = "pnlOutgoingReferal_ResultT";
        var dgvDivId = "dgvOutgoingReferralT";
        var pagingDivId = "dgvOutgoingReferral_PagingT";

        if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId)) {
            $("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnClearTable();
            $("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
            $("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId + " tbody").find("tr").remove();
        }

        if (response.ReferralListCount > 0) {
            var ReferralLoadJSONData = JSON.parse(response.ReferralListLoad_JSON);

            if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId)) {
                $("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
            }
            var arraTemp = [];
            $.each(ReferralLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.ReferralId);
                $row.attr("ProblemListNotesId", item.NoteId);
                var SelectionCheckBoxColumn = '<td class="center"><input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordReferral" ArrayProperty="ReferralIds" name="selectRecordReferral" class="input-block text-center" coltype="checkbox" id="selectRecordReferral_' + item.ReferralId + '"/></td>';
                var proceduresHtml = "";
                var procedures = item.Procedures.split(',');
                for (var p = 0; p < procedures.length; p++) {
                    proceduresHtml = proceduresHtml + procedures[p] + "<br>";
                }
                NewActions = '&nbsp;<a class="btn  btn-xs"  href="#" onclick="Clinical_Treatment.DeleteReferral(\'' + item.ReferralId + '\',this,event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Treatment.AddReferral(' + item.ReferralId + ',event);utility.SelectGridRow($(this));"   title="Edit Record"><i class="fa fa-edit black"></i></a>';
                if (item.Type == 'Outgoing') {
                    if (item.RefProviderName != '') {
                        $row.append(SelectionCheckBoxColumn + '<td>' + NewActions + '</td><td>' + utility.RemoveTimeFromDate(null, item.Date) + ' ' + item.Time + '</td><td>' + item.Procedures + '</td><td>' + item.ProviderName + '</td><td>' + item.RefProviderName + ' (' + item.RefProviderEntityName + ')</td><td>' + $($("#" + Clinical_Treatment.params.PanelID + " #ddlVisitType > option")[item.Visits]).text() + '</td><td>' + item.AssigneeName + ' </td>');
                    }
                    else {
                        $row.append(SelectionCheckBoxColumn + '<td>' + NewActions + '</td><td>' + utility.RemoveTimeFromDate(null, item.Date) + ' ' + item.Time + '</td><td>' + item.Procedures + '</td><td>' + item.ProviderName + '</td><td>' + item.RefProviderName + '</td><td>' + $($("#" + Clinical_Treatment.params.PanelID + " #ddlVisitType > option")[item.Visits]).text() + '</td><td>' + item.AssigneeName + ' </td>');
                    }
                    $("#" + Clinical_Treatment.params.PanelID + " #" + dgvDivId + " tbody").last().append($row);
                }

            });
            //Inalize grid
            var PanelGrid = "#" + Clinical_Treatment.params.PanelID + " #" + PnlResult;
            var GridId = "#" + Clinical_Treatment.params.PanelID + " #" + dgvDivId;

            if (!($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId)))
                $("#" + Clinical_Treatment.params.PanelID + " #" + PnlResult + " #" + dgvDivId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1] }] });
        }
        else {
            $('#' + Clinical_Treatment.params.PanelID + ' #' + PnlResult + ' #' + dgvDivId).DataTable({
                "language": {
                    "emptyTable": "No Outgoing Referral Found."
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1] }], "bDestroy": true
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        if (fromPagination) {
            Clinical_Treatment.params.IsExpand = false;
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");
    },
    SearchReferral: function (PrimaryID, PageNumber, ResultPerPage) {
        var PnlResult = "";
        var dgvDivId = "";
        var pagingDivId = "";
        var SearchFormDivId = "";
        var IsCheckedIn = "1";
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (ResultPerPage == null) {
            ResultPerPage = 15;
        }
        var objData = {};
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["IsActive"] = IsCheckedIn;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = ResultPerPage;
        objData["LoadFor"] = "Grid";
        objData["Type"] = "Outgoing";
        objData["commandType"] = "SEARCH_REFERRAL";
        objData["NoteId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    DeleteReferral: function (ReferralId, $row, obj) {
        if (obj != null) {
            obj.stopPropagation();
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Patient_Referrals.DeleteReferral_DBCall(ReferralId).done(function (response) {

                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_Treatment.LoadReferrals(null, 1, 15);
                            utility.DisplayMessages(response.Message, 1);
                            var _self = $row;
                        } else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }, function () { },
                    '3'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    DeleteReferral_DBCall: function (ReferralId) {
        var objData = new Object();
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "DELETE_REFERRAL";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    ////////////////////////////////////////////////////////////////// Therapeutic //////////////////////////////////////////////////////////////////////////////////
    TherapeuticSearch: function (PrimaryID, PageNo, rpp, fromPagination) {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_Treatment.searchTherapeutic(PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        $.when(Clinical_Treatment.TherapeuticInjectionGridLoad(response, fromPagination)).then(function () {
                            $("#" + Clinical_Treatment.params.PanelID + " [name='SelectCheckBoxTherapeutic']").prop("checked", false);
                            $("#chkHeaderTherapeuticOrder").prop("checked", false);
                            dfd.resolve();
                        });
                        var TableControl = Clinical_Treatment.params.PanelID + ' #dgvImunizationOS';
                        var PagingPanelControlID = Clinical_Treatment.params.PanelID + ' #dgvTherapeuticT_PagingOS';
                        var ClassControlName = "Clinical_Treatment";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.TherapeuticInjectionCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_Treatment.TherapeuticSearch(PrimaryID, PageNumber, ResultPerPage, true);
                            }), 10);

                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        return dfd;
        //==============================});
    },
    searchTherapeutic: function (pageNumber, rowsPerPage, OsImmTherInjectionId, Type) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization_Therapeutic_Injection";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },
    TherapeuticInjectionGridLoad: function (response, fromPagination) {
        $("#" + Clinical_Treatment.params.PanelID + " #pnlTherapeutic_ResultT #dgvTherapeuticT").dataTable().fnDestroy();
        $("#" + Clinical_Treatment.params.PanelID + " #pnlTherapeutic_ResultT #dgvTherapeuticT tbody").find("tr").remove();
        var TherapeuticInjectionLoad_JSON = JSON.parse(response.TherapeuticInjectionLoad_JSON);
        if (TherapeuticInjectionLoad_JSON.length > 0) {
            $.each(TherapeuticInjectionLoad_JSON, function (i, item) {
                var immTherInjectionId = item.ImmTherInjectionId;
                var $row = $('<tr/>');
                $row.attr("id", immTherInjectionId);
                var editParameters = 'event,' + immTherInjectionId + ",'Edit','" + item.Type + "'";
                var SelectionCheckBoxColumn = "";
                var actions = '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Treatment.addTherapeutic(' + editParameters + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>'

                var SelectionCheckBoxColumn = '<td class="center"><input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordTherapeutic" ArrayProperty="TherapeuticIds" name="selectRecordTherapeutic" class="input-block text-center" coltype="checkbox" id="selectRecordTherapeutic_' + immTherInjectionId + '"/></td>';

                $row.append(SelectionCheckBoxColumn + '<td style="display:none">' + item.ModifiedOn + '</td><td class="actions Actions_" id="' + immTherInjectionId + '" >' + actions + '</td>' + '<td>' + item.TherapeuticInjection + '</td><td>' + (item.Dose != "" ? item.Dose + " " + item.Amount : "") + '</td><td>' + item.AdministrationDate + '</td><td >' + item.ProviderName + '</td><td >' + item.GivenByName + '</td><td>' + item.Type + '</td>');
                $("#" + Clinical_Treatment.params.PanelID + " #dgvTherapeuticT tbody").append($row);
            });


            var TherapeuticRows = $("#" + Clinical_Treatment.params.PanelID + " #pnlTherapeutic_ResultT #dgvTherapeuticT tbody").find("tr");

            if (TherapeuticRows.length < 1) {
                $("#" + Clinical_Treatment.params.PanelID + " #pnlTherapeutic_ResultT #dgvTherapeuticT").DataTable({
                    "language": {
                        "emptyTable": "No Therapeutic Injection Found"
                    }, "autoWidth": false, "bLengthChange": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }, { "targets": [7], "visible": false }]
                });
            }
            else if (!$.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + ' #pnlTherapeutic_ResultT #dgvTherapeuticT')) {
                $("#" + Clinical_Treatment.params.PanelID + " #pnlTherapeutic_ResultT #dgvTherapeuticT").DataTable({
                    "destroy": true,
                    "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }]
                });
            }
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + " #divTherapeutic_PagingT").css("display", "none");


            $("#" + Clinical_Treatment.params.PanelID + " #pnlTherapeutic_ResultT #dgvTherapeuticT").DataTable({
                "language": {
                    "emptyTable": "No Therapeutic Injection Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1, 2] }]
            });
        }
        if (fromPagination) {
            Clinical_Treatment.params.IsExpand = false;
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");
    },


    ////////////////////////////////////////////////////////////////// VaccineHx //////////////////////////////////////////////////////////////////////////////////
    ImmunizationSearch: function (PrimaryID, PageNo, rpp, fromPagination) {
        var dfd = $.Deferred();

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_Treatment.VaccineHxCount = 0;
                Clinical_Treatment.searchImmunization(PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        Clinical_Treatment.VaccineHxCount = response.iTotalDisplayRecords;
                        Clinical_Treatment.VaccineHxGridLoad(response, fromPagination);
                        var TableControl = Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT";
                        var PagingPanelControlID = Clinical_Treatment.params.PanelID + " #divImmunizationVaccineHxT_Paging";
                        var ClassControlName = "Clinical_Treatment";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;

                        setTimeout(CreatePagination(response.ParentImmunizationCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_Treatment.ImmunizationSearch(PrimaryID, PageNumber, ResultPerPage, true);
                        }), 10);
                        dfd.resolve();
                    } else {
                        Clinical_Treatment.VaccineHxCount = 0;
                        $('#' + Clinical_Treatment.params.PanelID + ' #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT').DataTable({
                            "language": {
                                "emptyTable": "No Vaccine found."
                            },
                            "autoWidth": false,
                            "bLengthChange": false,
                            "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                            "bDestroy": true
                        });
                        setTimeout(function () {
                            //$('#' + Clinical_Treatment.params.PanelID + " #VaccineBtn").click();
                            dfd.resolve();
                        }, 5);
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        //==============================});
    },
    searchImmunization: function (pageNumber, rowsPerPage) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        IsActive = "1";
        var objData = new Object();
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["IsActive"] = IsActive;
        objData["NotesId"] = 0;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    VaccineHxGridLoad: function (response, fromPagination) {
        Clinical_Treatment.params.VaccineHxGridArray = [];
        var isactive = $('#' + Clinical_Treatment.params.PanelID + ' #pnlVaccineHx_ResultT #divSwitch #switchActive').attr('isactive');

        // get Actions
        var actions = "";


        $("#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT").dataTable().fnClearTable();
        $("#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT").dataTable().fnDestroy();
        $("#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT tbody").find("tr").remove();





        //tem array to hold rows and childs
        var arraTemp = [];
        var immunizationParentChildLoadJSONData = JSON.parse(response.ImmunizationParentChildLoad_JSON);
        if (immunizationParentChildLoadJSONData.length > 0) {

            if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT")) {
                $("#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT").dataTable().fnDestroy();
            }

            $.each(immunizationParentChildLoadJSONData, function (i, item) {

                var parentRow = item.ParentImmunization;
                var childRows = item.ChildImmunizationList;

                var vaccineId = parentRow.VaccineId;
                var vaccineHxId = parentRow.VaccineHxId;
                var $row = $('<tr/>');

                $row.attr("id", vaccineId);
                $row.attr("vaccineHxId", vaccineHxId);
                var AddParameters = "'Edit',event,'" + parentRow.Category + "','" + parentRow.VaccineScheduleId + "'," + (parentRow.IsHistoryDose ? "'HistoryDose'" : "'Hx'") + ",'" + parentRow.VaccineHxId + "','" + parentRow.Type + "',event,'','" + parentRow.OrderSetId + "'";

                var actions = '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Treatment.AddImmunization(' + AddParameters + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>';





                var SelectionCheckBoxColumn = '<td class="center"><input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordImmunization" ArrayProperty="ImmunizationIds" name="selectRecordImmunization" class="input-block text-center" coltype="checkbox" id="selectRecordImmunization_' + parentRow.VaccineHxId + '"/></td>';


                var ParentType = "";
                if (parentRow.Type != null && parentRow.Type.indexOf("ADMINISTER") >= 0) {
                    ParentType = "Administered";
                }
                else if (parentRow.Type == "DOCUMENTHX") {
                    ParentType = "DocumentHx";
                }
                else if (parentRow.Type == "REFUSAL") {
                    ParentType = "Refusal";
                }
                var Acknowledgement = "";
                var AcknowledgmentMessage1 = "";
                var AcknowledgmentMessage2 = "";
                var Class = "";
                if (parentRow.AcknowledgementCode.toLowerCase() == "aa") {
                    Acknowledgement = "Successful";
                    AcknowledgmentMessage1 = "Application Accept";
                    AcknowledgmentMessage2 = "Accept";
                    Class = "fa fa-bell pull-right";
                }
                else if (parentRow.AcknowledgementCode.toLowerCase() == "ae") {
                    Acknowledgement = "Error";
                    AcknowledgmentMessage1 = "Application Error";
                    AcknowledgmentMessage2 = "Error";
                    Class = "fa fa-exclamation-triangle pull-right";
                }
                else if (parentRow.AcknowledgementCode.toLowerCase() == "ar") {
                    Acknowledgement = "Error";
                    AcknowledgmentMessage1 = "Application Rejected";
                    AcknowledgmentMessage2 = "Reject";
                    Class = "fa fa-exclamation-triangle pull-right";
                }
                else {
                    AcknowledgmentMessage1 = "";
                    AcknowledgmentMessage2 = "";

                }
                var obj = {};
                obj.VaccineHxId = parentRow.VaccineHxId;
                obj.Message1 = AcknowledgmentMessage1;
                obj.Message2 = AcknowledgmentMessage2;
                obj.AcknowledgementCode = parentRow.AcknowledgementCode;
                Clinical_Treatment.params.VaccineHxGridArray.push(obj);


                if (childRows.length > 0) {
                    $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td>' + '<td class="actions Actions_" id="' + parentRow.VaccineHxId + '" >' + actions + '</td><td>' + parentRow.CategoryName + '</td><td>' + parentRow.VaccineName + '</td><td>' + parentRow.Dose + '</td><td>' + parentRow.AdministrationDate + '</td><td>' + parentRow.ProviderName + '</td><td>' + parentRow.GivenByName + '</td><td>' + parentRow.Location + '</td><td>' + parentRow.LotNumber + '</td><td> ' + ParentType + ' </td>');
                } else {
                    $row.append(SelectionCheckBoxColumn + '<td></td>' + '<td class="actions Actions_" id="' + parentRow.VaccineHxId + '" >' + actions + '</td><td>' + parentRow.CategoryName + '</td><td>' + parentRow.VaccineName + '</td><td>' + parentRow.Dose + '</td><td>' + parentRow.AdministrationDate + '</td><td>' + parentRow.ProviderName + '</td><td >' + parentRow.GivenByName + '</td><td>' + parentRow.Location + '</td><td>' + parentRow.LotNumber + '</td><td> ' + ParentType + ' </td>');
                }
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit button

                // Append parent row to the table body
                $("#" + Clinical_Treatment.params.PanelID + " #dgvImmunizationVaccineHxT tbody").last().append($row);

                //Child Records
                var CurrentRowchilds = $();

                if (childRows.length > 0) {


                    $.each(childRows, function (i, item) {


                        var SelectionCheckBoxColumn = '<td class="center"><input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordImmunization" ArrayProperty="ImmunizationIds" name="selectRecordImmunization" class="input-block text-center" coltype="checkbox" id="selectRecordImmunization_' + item.VaccineHxId + '"/></td>';








                        var AddParameters = "'Edit',event,'" + item.Category + "'," + item.VaccineScheduleId + "," + (item.IsHistoryDose ? "'HistoryDose'" : "'Hx'") + "," + item.VaccineHxId + ",'" + item.Type + "'" + ",event(),'','" + item.OrderSetId + "'";

                        var ChildType = "";
                        if (item.Type != null && item.Type.indexOf("ADMINISTER") >= 0) {
                            ChildType = "Administered";
                        }
                        else if (item.Type == "DOCUMENTHX") {
                            ChildType = "DocumentHx";
                        }
                        else if (item.Type == "REFUSAL") {
                            ChildType = "Refusal";
                        }
                        var ChildClass = "";
                        var Acknowledgement1 = "";
                        if (item.AcknowledgementCode.toLowerCase() == "aa") {
                            Acknowledgement1 = "Successful";
                            AcknowledgmentMessage1 = "Application Accept";
                            AcknowledgmentMessage2 = "Accept";
                            ChildClass = "fa fa-bell pull-right";
                        }
                        else if (item.AcknowledgementCode.toLowerCase() == "ae") {
                            Acknowledgement1 = "Error";
                            AcknowledgmentMessage1 = "Application Error";
                            AcknowledgmentMessage2 = "Error";
                            ChildClass = "fa fa-exclamation-triangle pull-right";
                        }
                        else if (item.AcknowledgementCode.toLowerCase() == "ar") {
                            Acknowledgement1 = "Error";
                            AcknowledgmentMessage1 = "Application Rejected";
                            AcknowledgmentMessage2 = "Reject";
                            ChildClass = "fa fa-exclamation-triangle pull-right";
                        }
                        else {
                            AcknowledgmentMessage1 = "";
                            AcknowledgmentMessage2 = "";
                        }
                        obj = {};
                        obj.VaccineHxId = item.VaccineHxId;
                        obj.Message1 = AcknowledgmentMessage1;
                        obj.Message2 = AcknowledgmentMessage2;
                        obj.AcknowledgementCode = item.AcknowledgementCode;
                        Clinical_Treatment.params.VaccineHxGridArray.push(obj);
                        var actions = '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Treatment.AddImmunization(' + AddParameters + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>';

                        //Start || 22 April, 2016 || ZeeshanAK || Changes for audit button
                        var childRow = '<tr class="childRow-bg">' + SelectionCheckBoxColumn + '<td></td>' + '<td class="actions Actions_" id="' + item.VaccineHxId + '" >' + actions + '</td><td>' + parentRow.CategoryName + '</td><td>' + item.VaccineName + '</td><td>' + item.Dose + '</td><td>' + item.AdministrationDate + '</td><td>' + item.ProviderName + '</td><td>' + item.GivenByName + '</td><td>' + item.Location + '</td><td>' + item.LotNumber + '</td><td>' + ChildType + '</td></tr>';
                        //End   || 22 April, 2016 || ZeeshanAK || Changes for audit button

                        CurrentRowchilds = CurrentRowchilds.add(childRow);
                    });
                }
                arraTemp.push({ row: $row, childs: CurrentRowchilds });
            });

            //Clear Data Table and draw

            //Inalize grid


            var PanelGrid = "#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT";
            var GridId = "#" + Clinical_Treatment.params.PanelID + " #dgvImmunizationVaccineHxT";

            if (Clinical_Treatment.ImmunizationmyGrid != null) {

                if ($.fn.dataTable.isDataTable(Clinical_Treatment.ImmunizationmyGrid)) {
                    Clinical_Treatment.ImmunizationmyGrid.$table.dataTable().fnDestroy();
                } else {
                    Clinical_Treatment.ImmunizationmyGrid = null;
                }
                if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT")) {
                    $("#" + Clinical_Treatment.params.PanelID + " #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT").dataTable().fnDestroy();
                }
            }

            Clinical_Treatment.ImmunizationmyGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Clinical_Treatment, 0, false, true, false, true, false, null);


            //Clinical_Treatment.EditableGrid.datatable.clear().draw();


            //push parent/childs rows to the datatable
            Clinical_Treatment.params.arraTemp = arraTemp;
            $.each(arraTemp, function (i, item) {

                if (Clinical_Treatment.ImmunizationmyGrid != null) {

                    var row = Clinical_Treatment.ImmunizationmyGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {

                        row.child(item.childs);
                    } else {
                    }
                }

            });

            if ($('#' + Clinical_Treatment.params.PanelID + ' #pnlVaccineHx_ResultT').length > 0) {
                $('#' + Clinical_Treatment.params.PanelID + ' #dgvImmunizationVaccineHxT').dataTable().fnSettings().aoColumns[0].bSortable = false;
            }





        } else {

            var Message = "No Active Immunization";

            $('#' + Clinical_Treatment.params.PanelID + ' #pnlVaccineHx_ResultT #dgvImmunizationVaccineHxT').DataTable({
                "language": {
                    "emptyTable": Message
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                "bDestroy": true,
                "aaSorting": [],
            });
        }

        EMRUtility.SwicthWidgetInializatoin();

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Start//22//02//2016//Muhammad Ahmad Imran//Sorting removed from first column of grid
        $('#' + Clinical_Treatment.params.PanelID + ' #dgvImmunizationVaccineHxT thead tr th:first-child').removeClass('sorting_asc');
        if (Clinical_Treatment.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_Treatment.params.PanelID + ' #dgvImmunizationVaccineHxT thead tr th:nth-child(2)').removeClass('sorting');
        }
        //End//22//02//2016//Muhammad Ahmad Imran//Sorting removed from first column of grid
        if (fromPagination) {
            Clinical_Treatment.params.IsExpand = false;
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");
    },
    rowExpand: function ($row, obj) {
        $.each(Clinical_Treatment.params.arraTemp, function (i, item) {

            if (Clinical_Treatment.ImmunizationmyGrid != null) {

                var row = Clinical_Treatment.ImmunizationmyGrid.datatable.row(item.row);
                if (item.childs.length > 0) {

                    row.child(item.childs);
                } else {
                }
            }

        });

        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:nth-child(2) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        } else {
            $row.find("td:nth-child(2) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
            if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
            }
        }
    },

}