Clinical_NotesProblemLists = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Clinical_NotesProblemLists.params = params;
        Clinical_ProgressNote.AttachedNoteComponentIds = [];
        Clinical_ProgressNote.DetachedNoteComponentIds = [];
        var patientId = Clinical_NotesProblemLists.params.PatientId;
        var noteId = Clinical_NotesProblemLists.params.NoteId;
        var providerId = Clinical_NotesProblemLists.params.ProviderId;
        var self = $('#' + Clinical_NotesProblemLists.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Clinical_NotesProblemLists.loadNotesProviders();
        });

        Clinical_NotesProblemLists.loadPreviousProblems(patientId, noteId, providerId);

    },
    loadPreviousProblems: function (patientId, noteId, providerId) {
        Clinical_ProgressNote.DetachedNoteComponentIds = [];
        Clinical_ProgressNote.AttachedNoteComponentIds = [];
        var strMessage = "";
        if ($("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result").css("display") == "none") {
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result").show();
        }

        Clinical_NotesProblemLists.SearchPreviousProblemList_DBCall(patientId, noteId, providerId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_NotesProblemLists.ProblemListGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ProblemListGridLoad: function (response) {

        if ($.fn.dataTable.isDataTable("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists")) {
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists").dataTable().fnClearTable();
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists").dataTable().fnDestroy();
        }
        $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody").find("tr").remove();
        var jsResponse = JSON.parse(response.ResponseModel);

        if (jsResponse.ProblemListCount > 0) {
            var headCount = 0;
            var isFirstRow = true;
            $.each(jsResponse.ProblemListLoad_JSON, function (i, item) {
                var $rowShowAll = $('<tr/>');
                var $row1 = $('<tr/>');
                var $row2 = $('<tr/>');

                if (isFirstRow == true) {
                    $row1.attr("id", "topParent");
                } else {
                    $row1.attr("id", "Parent");
                }
                // $row1.attr("id", item.ProblemListId);
                //$row1.attr("ProblemListNotesId", item.NoteId);
                $row2.attr("Id", item.ProblemListId);
                //$row1.attr("ProblemListNotesId", item.NoteId);

                $rowShowAll.attr("Id", "btnShowAll");
                $rowShowAll.append('<td colspan="4" class="text-right"><button  class="btn btn-link btn-xs" onclick="Clinical_NotesProblemLists.showAllVisitProblems();">Show All</button></td>');


                var SelectionCheckBoxColumn1 = "";
                var SelectionCheckBoxColumn2 = "";
                var Checked = "";
                SelectionCheckBoxColumn1 = '<td class="size20"><input type="checkbox" id="parent" class="text-center" onchange="Clinical_NotesProblemLists.enableAddProbList(this);"  name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
                //SelectionCheckBoxColumn2 = '<td class="sorting_1 center"><input type="checkbox" id=' + item.ProblemListId + ' class="text-center" onchange="Clinical_NotesProblemLists.enableAddProbList(this);"  name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';

                SelectionCheckBoxColumn2 = '<td colspan="4"><div class="checkbox-custom checkboxTiny pull-left ml-lg mb-none"> <input type="checkbox" class="text-center" id=' + item.ProblemListId + ' onchange="Clinical_NotesProblemLists.enableAddProbList(this);" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center" /><label class="control-label" for="IsNonBilable"></label></div>' +
                '<div class="pull-left"> ' + item.ICD10 + ' - ' + item.ICD10_Description + '</div></td>';


                $row1.append(SelectionCheckBoxColumn1 + '<td class="text-danger bold">' + item.VisitDate + '</td><td class="text-danger bold">' + item.reasoncomments + '</td><td class="text-danger bold">' + item.ProviderName + '</td>');

                //$row2.append(SelectionCheckBoxColumn2 + '<td>' + item.ICD10 + '</td><td>' + item.ICD10_Description + '</td>');
                $row2.append(SelectionCheckBoxColumn2);
                if ($("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tr > td:contains(" + item.VisitDate + ")").length < 1) {
                    headCount++;
                    //if (headCount >= 3) {
                    //    $($row1).addClass('hidden');
                    //    $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody").last().append($row1);
                    //} else {
                        $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody").last().append($row1);
                    //}

                }

                //if (headCount >= 3) {
                //    $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnShowMore").removeClass('hidden');
                //    $($row2).addClass('hidden');
                //    $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody").last().append($row2);
                //} else {
                    $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody").last().append($row2);
                //}

                var isExists = $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody tr > td:contains(Show All)").length;
                if (headCount >= 3 && isExists == 0) {
                    //  $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody").last().append($rowShowAll);
                }
                isFirstRow = false;
            });

        }
        else {

            $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnShowMore").addClass('hidden');
            //$("#" + Clinical_NotesProblemLists.params.PanelID + ' #pnlNotesProblemLists_Result #dgvNotesProblemLists').DataTable({
            //    "destroy": true,
            //    "language": {
            //        "emptyTable": "No Record Found"
            //    }, "autoWidth": false, "bLengthChange": false
            //});
        }
        //if ($.fn.dataTable.isDataTable("#" + Clinical_NotesProblemLists.params.PanelID + ' #pnlNotesProblemLists_Result #dgvNotesProblemLists'))
        //    ;
        //else {
        //    $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //}



    },


    loadNotesProviders: function () {
        var patientId = $('#PatientProfile #hfPatientId').val();
        var data = "entityID=" + patientId;

        MDVisionService.lookups('GetNotesProviders', true, data).done(function (result) {
            result = JSON.parse(result["GetNotesProviders"]);
            var options = result;
            var $providerDdl = $('#' + Clinical_NotesProblemLists.params.PanelID + ' #ddlNotesProviders');

            $providerDdl.empty();
            $.each(options, function (i, item) {
                if (item.Value != "" && typeof item.Value != 'undefined') {
                    $providerDdl.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                            refname: item.RefName,
                            refvalue: item.RefValue

                        })
                    );

                }
            });
            if (Clinical_NotesProblemLists.ProviderIds != '') {
            }

        }).then(function () {
            Clinical_NotesProblemLists.IntializeMultiSelectDropDownProviders();
        });
    },

    IntializeMultiSelectDropDownProviders: function () {
        var notesProviderId = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        $('#' + Clinical_NotesProblemLists.params.PanelID + ' #ddlNotesProviders option[value="' + notesProviderId + '"]').attr("selected", "selected");
        $('#' + Clinical_NotesProblemLists.params.PanelID + ' #ddlNotesProviders').multiselect("refresh");
        var patientId = Clinical_NotesProblemLists.params.PatientId;
        var noteId = Clinical_NotesProblemLists.params.NoteId;
        $('#' + Clinical_NotesProblemLists.params.PanelID + ' #ddlNotesProviders').multiselect('destroy');
        $('#' + Clinical_NotesProblemLists.params.PanelID + ' #ddlNotesProviders').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: '- Select -',
            selectAll: false,
            onChange: function (option, checked, select) {
                var ProviderIDs = '';
                var selectedProviders = $('#' + Clinical_NotesProblemLists.params.PanelID + ' #ddlNotesProviders option:selected');
                $.each(selectedProviders, function () {
                    ProviderIDs += $(this).val() + ',';
                });
                Clinical_NotesProblemLists.loadPreviousProblems(patientId, noteId, ProviderIDs);
                $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnShowMore").text('Show More');
            },
            onDropdownHide: function (event) {

            },


        });
    },

    showAllVisitProblems: function () {

        if ($("#" + Clinical_NotesProblemLists.params.PanelID + " #btnShowMore").text() == "Show More") {
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #pnlNotesProblemLists_Result #dgvNotesProblemLists tbody tr").removeClass('hidden');
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnShowMore").text('Back To Top');
        }
        else if ($("#" + Clinical_NotesProblemLists.params.PanelID + " #btnShowMore").text() == "Back To Top") {
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnShowMore").text('Show More');
        }

    },
    checkProblemListExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AssessmentNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ProblemsComponent"> <header>' +
                        '<clinical_problems title="Problems"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Problems\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Problem">Problems</a> ' +
                        '<a onclick="Clinical_ProgressNote.openProblemLists();" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="OpenProblemLists" name=""><i class="fa fa-caret-down orange" aria-hidden="true"></i></a>' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Problems\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Problems\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_problems> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
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
    },
    createProblemListBodyHTML: function (response, NoteHTMLCtrl, ProblemListsId, hideAlertMessage) {
        Clinical_NotesProblemLists.checkProblemListExists();
        var ProblemListSoap_JSON = response.ProblemListSoap_JSON;
        var $mainDivVital = $(document.createElement('div'));
        if (ProblemListSoap_JSON == null || ProblemListSoap_JSON.length == 0) {
            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            Clinical_ProgressNote.saveComponentSOAPText('Problems', true)
            return "";
        }
        var PListId = [];
        if (response.ProblemListSoapCount > 0) {
            $.each(ProblemListSoap_JSON, function (index, element) {
                var color = "";
                var $infoButtonrow = "";
                if (element.Description != "") {
                    var searchstr = element.Description.split('-')[0].trim();
                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "clinicalTabProgressNote", 2, "", "Clinical_NotesProblemLists");
                }
                if (element.Severity == "Mild Intermittent" || element.Severity == "Mild Persistent") {
                    color = 'style = "color:green;font-weight:bold" ';
                }
                if (element.Severity == "Severe Persistent" || element.Severity == "Unspecified Severity") {
                    color = 'style = "color:red;font-weight:bold" ';
                }
                if (element.Severity == "Moderate Persistent") {
                    color = 'style = "color:orange;font-weight:bold" ';
                }

                var PLid = element.ProblemListId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('problemOrder', element.ProblemOrder);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_ProblemList_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ProblemList_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Problems_Main" + PLid + '"  ><i class="fa fa-times"></i></a><a href="javascript:void(0);" class="btn-xs on-default up-row" title="Up Record" onclick="Clinical_ProgressNote.problemUp(\'' + PLid + '\')"><i class="fa fa-arrow-up black"></i></a>'
                    + '<a href="javascript:void(0);" class="btn-xs on-default down-row" title="Down Record" onclick="Clinical_ProgressNote.problemDown(\'' + PLid + '\')"><i class="fa fa-arrow-down black"></i></a></div> ');

                var StartDateEndDate = "";
                if (element.StartDate != '' && element.StartDate != null && element.EndDate != '' && element.EndDate != null && utility.RemoveTimeFromDate(null, element.StartDate) != utility.RemoveTimeFromDate(null, element.EndDate)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate)) + (element.EndDate == '' ? "" : " and ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.StartDate == '' || element.StartDate == null) && (element.EndDate != '' && element.EndDate != null)) {
                    StartDateEndDate = (element.EndDate == '' ? "" : " ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.EndDate == '' || element.EndDate == null) && (element.StartDate != '' && element.StartDate != null)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                else if (element.EndDate == '' || element.EndDate == null && (element.StartDate == '' || element.StartDate == null)) {
                    StartDateEndDate = "";
                }
                else if (element.StartDate == element.EndDate) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                var inActiveText = "";
                if (element.IsActive != null && element.IsActive == "False") {
                    inActiveText = " : <span style = 'color:red;font-weight:bold'> (Inactive) </span>";
                }
                var CodeDescription = element.Description == '' ? "" : element.Description;
                $ListVital.append("<li><strong> " + CodeDescription + inActiveText + " " + $infoButtonrow + " </strong>" + (element.ChronicityLevel == '' ? "" : " Chronicity: " + element.ChronicityLevel) + ((element.Severity && element.ChronicityLevel) ? ", " : "") +
                    (element.Severity == '' ? "" : "<span " + color + '>Severity: ' + element.Severity + "</span> ") +
                    StartDateEndDate +
                    (element.ModifiedOn == '' ? "" : " Modified on " + utility.RemoveTimeFromDate(null, element.ModifiedOn) + ".")
                    );

                $ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).length == 0) {
                    PListId.push(PLid);
                    $mainDivVital.append($SectionBodyVital);
                } else {
                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul').append(CommentHTML);;
                }
            });
            if (PListId.join(",") != "") {
                ProblemListsId = PListId.join(",");
            }
            if ($mainDivVital.html() != '') {
                Clinical_NotesProblemLists.updateProblemListHtml($mainDivVital.html(), ProblemListsId, NoteHTMLCtrl, hideAlertMessage);
            } else {
                Clinical_NotesProblemLists.updateProblemListHtml('', ProblemListsId, NoteHTMLCtrl, hideAlertMessage);
                //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                Clinical_ProgressNote.saveComponentSOAPText('Problems', true)
            }
        } else {

            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            Clinical_ProgressNote.saveComponentSOAPText('Problems', true)
        }
    },
    attachProblemsListWithNotes_DBCall: function (ProblemsListId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProblemListId"] = ProblemsListId;
        objData["commandType"] = "attach_problemlists_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    updateProblemListHtml: function (ProblemListHtml, ProblemListId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_problems').parent().parent().addClass('initialVisitBody');
        $(NoteHTMLCtrl + ' clinical_problems').parent().parent().addClass('probList');
        if (ProblemListHtml != '') {
            $(NoteHTMLCtrl + ' clinical_problems').parent().parent().append(ProblemListHtml);
        }
        if (ProblemListId != null && typeof ProblemListId != "string") {
            ProblemListId = ProblemListId.toString();
        }
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ProblemListHtml != '') {
            Clinical_NotesProblemLists.attachProblemListignFromNotes(ProblemListId, hideAlertMessage);
        }

    },
    attachProblemListignFromNotes: function (ProblemsListId, hideAlertMessage) {
        if (ProblemsListId == "" || ProblemsListId == "undefined") {
        }
        else {
            Clinical_NotesProblemLists.attachProblemsListWithNotes_DBCall(ProblemsListId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.when(Clinical_ProgressNote.saveComponentSOAPText('Problems', true)).then(function () {
                        Clinical_ProgressNote.HideShowBillingInfo();
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    getProblemListsInfo: function (ProblemListsId, hideAlertMessage) {
        if (ProblemListsId == null || ProblemListsId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_NotesProblemLists.get_ProblemLists_ForSOAP(ProblemListsId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ProblemListSoapCount > 0) {
                        Clinical_NotesProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', ProblemListsId, hideAlertMessage);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    OverwrightProblemListsToNotes: function () {
        Clinical_NotesProblemLists.detach_ComponentsProblemList("Problems", true, true);
    },
    detach_ComponentsProblemList: function (ComponentName, IsUpdate, ProblemListComponentRemove) {

        var Clinical_ProblemListIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').parent().parent().find('section[id*="Cli_Problems_Main"]').map(function () {
            return this.id.replace("Cli_Problems_Main", "");
        }).get().join(',');

        if (ProblemListComponentRemove) {
            //$('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Problem Lists']").remove();
            //$('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').parent().parent().remove();
            $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").remove();
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').parent().parent().find('section[id*="Cli_Problems_Main"]').remove();
        }

        if (Clinical_ProblemListIds == "" || Clinical_ProblemListIds == "undefined") {
            $.when(Clinical_ProgressNote.saveComponentSOAPText('Problems', true)).then(function () {
                Clinical_ProgressNote.HideShowBillingInfo();
            });
           // utility.DisplayMessages('Successfully Updated', 1);
            Clinical_NotesProblemLists.addProblemListsToNotes(false);
        }
        else {
            Clinical_NotesProblemLists.detachProblemsListFromNotes_DBCall(Clinical_ProblemListIds).done(function (response) {
                response = JSON.parse(response);

                if (response.status != false) {
                    if (IsUpdate) {
                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Problems', true)).then(function () {
                            Clinical_ProgressNote.HideShowBillingInfo();
                            Clinical_NotesProblemLists.addProblemListsToNotes(false);
                        });
                    }
                    utility.DisplayMessages("Successfully Updated", 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    detachProblemsListFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        Clinical_NotesProblemLists.detachProblemsListFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var PLid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + PLid).remove();
                }
                Clinical_ProgressNote.HideShowBillingInfo();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    detachProblemsListFromNotes_DBCall: function (ProblemsListId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProblemListId"] = ProblemsListId;
        objData["commandType"] = "detach_problemlists_from_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    attachProblemsListFromNotes: function (SelectedProblemLists, hideAlertMessage) {
        Clinical_NotesProblemLists.getProblemListsInfo(SelectedProblemLists.join()).done(function () {
            setTimeout(function () {
                if (hideAlertMessage != false) {
                    utility.DisplayMessages('Successfully Updated', 1);
                }
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            }, 5);
        });
    },
    get_ProblemLists_ForSOAP: function (ProblemListsId) {

        var objData = new Object();
        objData["ProblemListId"] = ProblemListsId;
        objData["commandType"] = "get_previousproblemlists_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    addProblemListsToNotes: function (hideAlertMessage) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var uniqueAttachedNoteComponentIds = Clinical_ProgressNote.AttachedNoteComponentIds.filter(function (elem, index, self) {
                    return index == self.indexOf(elem);
                });
                var SelectedProblemLists = uniqueAttachedNoteComponentIds.slice();
                if (SelectedProblemLists != null && SelectedProblemLists != '') {
                    for (var i = 0; i < uniqueAttachedNoteComponentIds.length; i++) {

                        var ind = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(uniqueAttachedNoteComponentIds[i]);
                        if (ind > -1) {
                            Clinical_ProgressNote.DetachedNoteComponentIds.splice(ind, 1);
                        }
                        var PLid = uniqueAttachedNoteComponentIds[i];
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + PLid).length != 0) {
                            var index = SelectedProblemLists.indexOf(PLid);
                            if (index > -1) {
                                SelectedProblemLists.splice(index, 1);
                            }
                        }
                    }

                }
                //var uniqueDetachedNoteComponentIds = Clinical_ProgressNote.DetachedNoteComponentIds.filter(function (elem, index, self) {
                //    return index == self.indexOf(elem);
                //});
                //var detachedvalues = uniqueDetachedNoteComponentIds;
                //if (detachedvalues.join() != '' && detachedvalues.join() != "undefined") {
                //Clinical_NotesProblemLists.detachProblemsListFromNotes(detachedvalues).done(function () {


                if (SelectedProblemLists.join() != null && SelectedProblemLists.join() != '' && SelectedProblemLists.join() != 'undefined') {
                    Clinical_NotesProblemLists.attachProblemsListFromNotes(SelectedProblemLists,hideAlertMessage);
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Problems', true);
                }
                //   });
                // }

                $("#" + Clinical_NotesProblemLists.params.PanelID + "  #dgvProblemLists_Paging #btnAddProbListToNotes").prop('disabled', true);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        if (Clinical_NotesProblemLists.params && Clinical_NotesProblemLists.params.ParentCtrl && Clinical_NotesProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Clinical_NotesProblemLists.params.ParentCtrl, 'Clinical_NotesProblemLists');
        }
    },
    enableAddProbList: function (obj) {
        obj.id = $(obj).parent().parent().parent().attr('id');
        if ($(obj).is(':checked') && obj.id > 0) {
            Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id);
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id);
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
            var parent = $(obj).parent().parent().parent().prevAll('#Parent')[0];
            if (parent == undefined) {
                parent = $(obj).parent().parent().parent().prevAll('#topParent')[0];
            }
            var AllSelected = true;
            $.each($(parent).nextAll(), function () {
                if ($(this).attr('id').indexOf('Parent') > -1) {
                    return false;
                } else {
                    if ($(this).find('input').prop('checked') == false) {
                        AllSelected = false;
                        return false;
                    }
                }
            });
            if (AllSelected == true) {
                $(parent).find('input').prop('checked', true);
            } else {
                $(parent).find('input').prop('checked', false);
            }
        } else if ($(obj).is(':checked') && obj.id == "undefined") {
            var childs = $(obj).parent().parent().nextAll();

            $.each(childs, function (index, value) {
                if (childs[index].id != "" && childs[index].id != 'Parent') {
                    var chkbox = $(childs[index].firstChild).find('input')[0];
                    // $(chkbox).attr('checked', 'checked').trigger('change');
                    if (!$(chkbox).prop('checked')) {
                        $(chkbox).trigger('click');
                    }
                    //Clinical_ProgressNote.AttachedNoteComponentIds.push(childs[index].id);
                    //var ind = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(childs[index].id);
                    //if (ind > -1) {
                    //    Clinical_ProgressNote.DetachedNoteComponentIds.splice(ind, 1);
                    //}
                } else {
                    return false;
                }


            });
        } else if (!$(obj).is(':checked') && obj.id == "undefined") {
            var childs = $(obj).parent().parent().nextAll();

            $.each(childs, function (index, value) {
                if (childs[index].id != "" && childs[index].id != 'Parent') {
                    var chkbox = $(childs[index].firstChild).find('input')[0];
                    $(chkbox).prop('checked', false);
                    //$(chkbox).attr('checked', 'checked').trigger('change');

                    Clinical_ProgressNote.DetachedNoteComponentIds.push(childs[index].id);
                    var ind = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(childs[index].id);
                    if (ind > -1) {
                        Clinical_ProgressNote.AttachedNoteComponentIds.splice(ind, 1);
                    }
                } else {
                    return false;
                }


            });
        }

        else {
            $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlProblemLists_Result #chkHeaderProblemsList').prop('checked', false);
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }

            var parent = $(obj).parent().parent().parent().prevAll('#Parent')[0];
            if (parent == undefined) {
                parent = $(obj).parent().parent().parent().prevAll('#topParent')[0];
            }
            var AllSelected = true;
            $.each($(parent).nextAll(), function () {
                if ($(this).attr('id').indexOf('Parent') > -1) {
                    return false;
                } else {
                    if ($(this).find('input').prop('checked') == false) {
                        AllSelected = false;
                        return false;
                    }
                }
            });
            if (AllSelected == true) {
                $(parent).find('input').prop('checked', true);
            } else {
                $(parent).find('input').prop('checked', false);
            }
        }
        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0) {

            $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnAddandOwerriteProblemListsToNote").removeClass('disableAll');
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnAddNotesProblemListsToNote").removeClass('disableAll');
        } else {
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnAddandOwerriteProblemListsToNote").addClass('disableAll');
            $("#" + Clinical_NotesProblemLists.params.PanelID + " #btnAddNotesProblemListsToNote").addClass('disableAll');
        }
    },
    SearchPreviousProblemList_DBCall: function (patientId, noteId, providerId) {

        var objData = new Object();

        objData["PatientId"] = patientId;
        objData["NoteId"] = noteId;
        objData["ProviderIDs"] = providerId;
        objData["commandType"] = "SEARCH_PREVIOUS_PROBLEMLISTS";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    UnLoad: function () {
        Clinical_ProgressNote.AttachedNoteComponentIds = [];
        Clinical_ProgressNote.DetachedNoteComponentIds = [];
        UnloadActionPan(Clinical_NotesProblemLists.params["ParentCtrl"], "Clinical_NotesProblemLists");
        Clinical_NotesProblemLists.params.PanelID = null;


    },
}