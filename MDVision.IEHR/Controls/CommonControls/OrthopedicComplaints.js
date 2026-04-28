OrthopedicComplaints = {
    params: [],
    IsComplaintSaved: false,
    Load: function (params) {

        OrthopedicComplaints.params = params;
        OrthopedicComplaints.IsComplaintSaved = false;

        $("#pnlOrthopedicComplaints #complaint_title").html("Add Complaints for " + OrthopedicComplaints.params["BodyPart"]);

        OrthopedicComplaints.LoadComplaints();
    },

    LoadComplaints: function () {

        OrthopedicComplaints.LoadOrthopedicComplaints_DBCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                $.each(response.OrthopedicComplaints_JSON, function () {

                    var btn = "";
                    if (parseInt(this.UserId) > 1 && this.CreatedBy.toLocaleLowerCase() != "mdvision")
                        btn = '<a class="btn  btn-xs pull-right" href="#" onclick="OrthopedicComplaints.DeleteOrthopedicComplaints(' + this.OrthopedicComplainId + ')" title="Delete Record"><i class="fa fa-close red"></i></a>';

                    var li = '<li id="ortho_complaint' + this.OrthopedicComplainId + '" onclick="OrthopedicComplaints.addComplaint(\'' + this.Complaint + '\')"  > ' + this.Complaint + ' ' + btn + ' </li>';
                    $("#pnlOrthopedicComplaints #ul_complaints").append(li);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },

    DeleteOrthopedicComplaints: function (OrthopedicComplainId) {

        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {

            var selectedValue = OrthopedicComplainId;
            var IsValue = (selectedValue == "" || selectedValue == "undefined");

            if (!IsValue) {
                OrthopedicComplaints.DeleteOrthopedicComplaints_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);

                    if (response.status != false) {
                        $("#pnlOrthopedicComplaints #ul_complaints").find("#ortho_complaint" + selectedValue).remove();
                        utility.DisplayMessages(response.Message, 1);
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

    Clear: function () {
        // Clear Field
        $("#pnlOrthopedicComplaints #txtOrthoComplaints").val("");
    },

    OK: function () {
        // Add to Note
        var $Complaint = $("#pnlOrthopedicComplaints #txtOrthoComplaints");
        if ($Complaint.val()) {
            $Complaint.css("border-color", "#ccc");

            var Complaints = [];
            Complaints.push({ "ComplaintDetailId": "0", "ComplaintDescription": OrthopedicComplaints.params["BodyPart"] + ": " + $("#pnlOrthopedicComplaints #txtOrthoComplaints").val() });


            OrthopedicComplaints.SaveBodyPartAndComplaint_DBCall(Complaints).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {

                    OrthopedicComplaints.IsComplaintSaved = true;
                    utility.DisplayMessages(response.Message, 1);
                    $("#pnlOrthopedicComplaints #txtOrthoComplaints").val('');

                    if (OrthopedicComplaints.params["ParentControl"] == "Clinical_OrthopedicChart") {
                        Clinical_OrthopedicChart.params["ComplaintId"] = response.ComplaintId;
                    }
                    else if (OrthopedicComplaints.params["ParentControl"] == "Clinical_OrthopedicChartDetail") {
                        Clinical_OrthopedicChartDetail.params["ComplaintId"] = response.ComplaintId;
                    }

                    // Update SOAP Text
                    OrthopedicComplaints.addComplaintSOAPTextToNotes(response);

                    if (OrthopedicComplaints.params["ParentControl"] == "Clinical_OrthopedicChart") {
                        Clinical_OrthopedicChart.LoadNotesBodyPartsComplaints();
                    }
                    if (OrthopedicComplaints.params["ParentControl"] == "Clinical_OrthopedicChartDetail") {
                        Clinical_OrthopedicChartDetail.LoadNotesBodyPartsComplaints();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $Complaint.css("border-color", "red");
        }
    },

    AddNew: function () {
        // Create New Field
        if ($("#pnlOrthopedicComplaints #ul_complaints li.add-complaint").length <= 0) {
            var markup = '<li class="add-complaint">'
                      + '<input type="text" />'
                      + '<span class="pull-right">'
                           + '<a class="btn  btn-xs" href="#" onclick="OrthopedicComplaints.saveComplaint(this)"  title="Save Record"><i class="fa fa-save green"></i></a>'
                           + '<a class="btn  btn-xs" href="#" onclick="OrthopedicComplaints.removeRow(this)" title="Delete Record"><i class="fa fa-close red"></i></a>'
                      + '</span>'
                   + '</li>';

            $("#pnlOrthopedicComplaints #ul_complaints").append(markup);
            $("#pnlOrthopedicComplaints #ul_complaints li.add-complaint").find("input").focus();
        }
        else {
            $("#pnlOrthopedicComplaints #ul_complaints li.add-complaint").find("input").focus();
        }
    },

    addComplaint: function (Complaint) {
        var val_ = $("#pnlOrthopedicComplaints #txtOrthoComplaints").val();
        $("#pnlOrthopedicComplaints #txtOrthoComplaints").val(val_ + " " + Complaint);
    },

    removeRow: function (obj) {
        $(obj).parent().parent().remove();
    },

    saveComplaint: function (obj) {
        var $input = $(obj).parent().parent().find("input");
        var val_ = $input.val();
        if (val_) {
            $input.css("border-color", "#ccc");
            OrthopedicComplaints.SaveOrthopedicComplaints_DBCall(val_).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {

                    OrthopedicComplaints.removeRow(obj);

                    var btn = '<a class="btn  btn-xs pull-right" href="#" onclick="OrthopedicComplaints.DeleteOrthopedicComplaints(' + response.OrthopedicComplainId + ')" title="Delete Record"><i class="fa fa-close red"></i></a>';
                    var li = '<li id="ortho_complaint' + response.OrthopedicComplainId + '" onclick="OrthopedicComplaints.addComplaint(\'' + val_ + '\')"  > ' + val_ + ' ' + btn + ' </li>';
                    $("#pnlOrthopedicComplaints #ul_complaints").append(li);

                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $input.css("border-color", "red");
        }
    },

    DeleteBodyPartAndComplaint: function (NotesBodyPartId, ComplaintDetailId, NotesId, PatientId, IsDeleteBodyPartAssociation) {

        if (event != null) {
            event.stopPropagation();
        }

        var dfd = new $.Deferred();

        utility.myConfirm('64', function () {

            var selectedValue = ComplaintDetailId;
            var IsValue = (selectedValue == "" || selectedValue == "undefined" || selectedValue == undefined);

            if (!IsValue) {
                OrthopedicComplaints.DeleteBodyPartAndComplaint_DBCall(NotesBodyPartId, selectedValue, NotesId, PatientId, IsDeleteBodyPartAssociation).done(function (response) {
                    response = JSON.parse(response);

                    if (response.status != false) {

                        // Update SOAP Text
                        OrthopedicComplaints.addComplaintSOAPTextToNotes(response);

                        dfd.resolve(true);
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        dfd.resolve(false);
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                dfd.resolve(false);

        }, function () {
            dfd.resolve(false);
        },
            'Confirm Delete', 'Ok', 'Cancel'
        );

        return dfd.promise();
    },

    SaveBodyPartAndComplaint_DBCall: function (Complaints) {

        var ComplaintId = 0;
        if (OrthopedicComplaints.params["ParentControl"] == "Clinical_OrthopedicChart") {
            ComplaintId = Clinical_OrthopedicChart.params["ComplaintId"] ? Clinical_OrthopedicChart.params["ComplaintId"] : 0;
        }
        if (OrthopedicComplaints.params["ParentControl"] == "Clinical_OrthopedicChartDetail") {
            ComplaintId = Clinical_OrthopedicChartDetail.params["ComplaintId"] ? Clinical_OrthopedicChartDetail.params["ComplaintId"] : 0;
        }

        var objData = {};
        objData["ComplaintId"] = ComplaintId;
        objData["BodyPart"] = OrthopedicComplaints.params["BodyPart"];
        objData["Complaints"] = Complaints;
        objData["NotesId"] = OrthopedicComplaints.params["NotesId"];
        objData["PatientId"] = OrthopedicComplaints.params["PatientId"];
        objData["commandType"] = "save_bodypart_and_complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    DeleteBodyPartAndComplaint_DBCall: function (NotesBodyPartId, ComplaintDetailId, NotesId, PatientId, IsDeleteBodyPartAssociation) {

        var objData = {};
        objData["NotesBodyPartId"] = NotesBodyPartId;
        objData["ComplaintDetailId"] = ComplaintDetailId;
        objData["IsDeleteBodyPartAssociation"] = IsDeleteBodyPartAssociation;
        objData["NotesId"] = NotesId;
        objData["PatientId"] = PatientId;
        objData["commandType"] = "delete_bodypart_and_complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    LoadNotesBodyPartsComplaints_DBCall: function (NotesId) {

        var objData = {};
        objData["NotesId"] = NotesId;
        objData["commandType"] = "load_notes_bodyparts_complaints";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    LoadOrthopedicComplaints_DBCall: function () {

        var objData = {};
        objData["commandType"] = "load_orthopedic_complaints";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    SaveOrthopedicComplaints_DBCall: function (Complaint) {

        var objData = {};
        objData["Complaint"] = Complaint;
        objData["commandType"] = "save_orthopedic_complaints";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    DeleteOrthopedicComplaints_DBCall: function (OrthopedicComplainId) {

        var objData = {};
        objData["OrthopedicComplainId"] = OrthopedicComplainId;
        objData["commandType"] = "delete_orthopedic_complaints";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    UnLoad: function () {

        if (OrthopedicComplaints.IsComplaintSaved == false) {
            OrthopedicChartSkeleton.CanvanId = "ChartCanvas";
            OrthopedicChartSkeleton.ParentControl = OrthopedicComplaints.params["ParentControl"];
            OrthopedicChartSkeleton.unSelectBodyPart(OrthopedicComplaints.params["BodyPart"]);
        }
        $('#containerOrthoPedicComplaints').modal('hide');
        $('#containerOrthoPedicComplaints').removeClass('modal fade')
        $('#containerOrthoPedicComplaints').empty();
    },

    // Complaint SOAP Text
    addComplaintSOAPTextToNotes: function (response) {

        var Complaints = JSON.parse(response.ComplaintDetail_JSON);
        var ComplaintHxId = response.ComplaintId;
        var patientInfo = Clinical_HPIComplaints.GetPatientInfo();

        OrthopedicComplaints.checkComplaintExists(ComplaintHxId);

        var $mainDivComplaintHx = $(document.createElement('div'));
        $mainDivComplaintHx.attr('id', "Section_Complaint");
        var $CCdiv = $(document.createElement('div'));
        var $SectionBodyComplaint = $(document.createElement('section'));
        var $ListComplaint = $(document.createElement('ul'));
        var $DetailsDiv = $(document.createElement('div'));

        var ComplaintComments = $('#' + Clinical_ProgressNote.params["PanelID"]).find('#Comments_Cli_Complaint_' + ComplaintHxId);
        var $HistoryListComplaint = $(document.createElement('ul'));
        var $HistoryDetailsDiv = $(document.createElement('div'));
        $HistoryListComplaint.attr('class', 'list-unstyled')
        $ListComplaint.attr('class', 'list-unstyled')
        var divId = '"Cli_Complaint_' + ComplaintHxId + '"';
        var ServerDateTime = $("#mainForm #userCurrentTime").html().split(" ")
        var d = new Date(ServerDateTime[1] + " " + ServerDateTime[2] + " " + ServerDateTime[3]);
        var UpdateDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
        if (Complaints.length > 0) {
            $HistoryListComplaint.append("<li class='btn btn-link p-none' style='color:#428BCA; margin-top:5px; cursor:pointer;' onclick='Clinical_ProgressNote.addComment(" + divId + ")'>" + "History of Present Illness" + "</li>")
        }
        if (Complaints.length > 0) {
            $SectionBodyComplaint.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Complaint_" + ComplaintHxId + '"><i class="fa fa-edit"></i></a>' +
              '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Complaint_Main" + ComplaintHxId + '"  onclick="javascript:void(0);"><i class="fa fa-times"></i></a></div> ');
        }
        $HistoryListComplaint.append("<li id='liHpi'><p>");
        //reset list as now we fill with all new Complaints below.

        $(Complaints).each(function (i, item) {

            var complaintsCC = "";
            $ListComplaint.append("<li complaintdetailid = '" + item.ComplaintDetailId + "' id='" + i + "' class='sopTextEditable ui-sortable-handle editableContentli'>"
                + "<span class='editable' style='display: block;'>" + item.ComplaintDescription + "</span>"
                + "<textarea class='edit form-control' spellcheck='true' style='display: none;'></textarea>"
                + "</li>");
            complaintsCC = item.ComplaintDescription;
            var $DetailsDiv = "<span style='font-weight:bold'>" + complaintsCC + "</span>";

            var StartLine = "";
            if (i == 0) {
                var StartLine = "<span> A " + patientInfo.age + " old " + patientInfo.gender + " presents with ";
            }
            else {
                var StartLine = "<span> The patient also presents with ";
            }
            if (item.OverallComments != "" || item.OverallComments != null) {
                $HistoryListComplaint.find('p').append(StartLine + $DetailsDiv + "." + (item.PreviousHistory && item.PreviousHistory != "" ? " Previous History: " + item.PreviousHistory + "." : "")
                    + ((item.Complaint_CaseId_text && item.Complaint_CaseId_text != "- Select -" && item.Complaint_CaseId_text != "") ? " Case is " + item.Complaint_CaseId_text + "." : "")
                    + ((item.Complaint_LocationIds_text && item.Complaint_LocationIds_text != "" && typeof item.Complaint_LocationIds_text != "undefined") ? " Location: " + item.Complaint_LocationIds_text + "." : "")
                    + ((item.Complaint_RadiationId_text && item.Complaint_RadiationId_text != "- Select -" && item.Complaint_RadiationId_text != "") ? " Radiates to: " + item.Complaint_RadiationId_text + "." : "")
                    + ((item.Complaint_QualityId_text && item.Complaint_QualityId_text != "- Select -" && item.Complaint_QualityId_text != "") ? " Quality is " + item.Complaint_QualityId_text + "." : "")
                    + ((item.Complaint_SeverityId_text && item.Complaint_SeverityId_text != "- Select -" && item.Complaint_SeverityId_text != "") ? " Severity is " + item.Complaint_SeverityId_text + "." : "")
                    + (item.Onset && item.Onset != "" ? " Onset: " + item.Onset + "." : "")
                    + (item.Duration && item.Duration != "" ? " Duration is " + item.Duration : "")
                    + ((item.Complaint_DurationId_text && item.Complaint_DurationId_text != "- Select -" && item.Complaint_DurationId_text != "") ? " " + item.Complaint_DurationId_text + "." : "")
                    + ((item.Complaint_FrequencyId_text && item.Complaint_FrequencyId_text != "- Select -" && item.Complaint_FrequencyId_text != "") ? " Frequency is " + item.Complaint_FrequencyId_text + "." : "")
                    + ((item.Complaint_ContextId_text && item.Complaint_ContextId_text != "- Select -" && item.Complaint_ContextId_text != "") ? " Context: " + item.Complaint_ContextId_text + "." : "")
                    + ((item.Complaint_CharacterIds_text && item.Complaint_CharacterIds_text != "" && typeof item.Complaint_CharacterIds_text != "undefined") ? " Character is " + item.Complaint_CharacterIds_text + "." : "")
                    + (item.AssociatedWith && item.AssociatedWith != "" ? " Associated with " + item.AssociatedWith + "." : "")
                    + (item.PrecipitatedBy && item.PrecipitatedBy != "" ? " Precipitated by " + item.PrecipitatedBy + "." : "")
                    + ((item.Complaint_AggravatedById_text && item.Complaint_AggravatedById_text != "- Select -" && item.Complaint_AggravatedById_text != "") ? " Aggravated by " + item.Complaint_AggravatedById_text + "." : "")
                    + ((item.Complaint_RelievedById_text && item.Complaint_RelievedById_text != "- Select -" && item.Complaint_RelievedById_text != "") ? " Relieved by " + item.Complaint_RelievedById_text + "." : "")
                    + (item.Comments && item.Comments != "" ? " " + item.Comments : "")
                    + '</span>'

               );
            }
            else {
                $HistoryListComplaint.find('p').append(StartLine + $DetailsDiv);
            }
        });

        if (Complaints.length > 0) {

            if (isNaN(UpdateDate) == false) {
                $HistoryListComplaint.append("</br> Last Updated on " + UpdateDate);
            }
        }
        else {
            $HistoryListComplaint.html("");
        }
        $HistoryDetailsDiv.attr('id', "Cli_Complaint_History_" + ComplaintHxId);
        $DetailsDiv.attr('id', "Cli_Complaint_" + ComplaintHxId);
        $ListComplaint.append($HistoryListComplaint.html())
        if (ComplaintComments.html() != undefined) {
            $ListComplaint.append(ComplaintComments[0].outerHTML);
        }
        $DetailsDiv.html($ListComplaint);
        //History
        $SectionBodyComplaint.append($DetailsDiv);
        $SectionBodyComplaint.attr('id', "Cli_Complaint_Main" + ComplaintHxId);

        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';

        if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).length == 0) {

            if ($(noteHTMLCtrl).find('#Section_Complaint').length > 0) {
                $(noteHTMLCtrl).find('[id="Section_Complaint"]').remove();
            }
            $mainDivComplaintHx.html($SectionBodyComplaint);

            OrthopedicComplaints.updateComplaintHtml($mainDivComplaintHx.append(), noteHTMLCtrl);

        } else {

            var CommentHTML = "";
            var CommentsID = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').attr('id');
            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                CommentHTML = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').get(0).outerHTML;
            }
            if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId))
                $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).remove();
            if ($(noteHTMLCtrl).find('#Section_Complaint #Cli_Complaint_Main' + ComplaintHxId)) {
                $(noteHTMLCtrl).find('#Section_Complaint #Cli_Complaint_Main' + ComplaintHxId).remove();
            }
            if ($(noteHTMLCtrl).find('#allComments')) {
                $(noteHTMLCtrl).find('#allComments').remove();
            }

            $mainDivComplaintHx.html($SectionBodyComplaint);

            OrthopedicComplaints.updateComplaintHtml($mainDivComplaintHx.append(), noteHTMLCtrl);
        }
        Clinical_ProgressNote.initilizeEditableContent();

    },

    updateComplaintHtml: function (birthHxHtml, noteHTMLCtrl) {
        if (!$(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().hasClass('initialVisitBody')) {
            $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().addClass('initialVisitBody');
        }

        if (birthHxHtml != '') {
            $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().append(birthHxHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (birthHxHtml != '') {
            Clinical_ProgressNote.saveComponentSOAPText('Complaints');
        }
    },

    checkComplaintExists: function (ComplaintId) {

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ComplaintsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<Clinical_Complaints title="Complaints"  id="' + ComplaintId + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'HPIComplaints\',\'-1\',' + OrthopedicComplaints.params["NotesId"] + ');" title="Complaints">Complaints</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Complaints\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Complaints\',\'-1\',' + OrthopedicComplaints.params["NotesId"] + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</Clinical_Complaints> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons(OrthopedicComplaints.params["NotesId"]);
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
}