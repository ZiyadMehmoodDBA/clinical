Clinical_PatientEducation = {
    bIsFirstLoad: true,
    params: [],
    patientId: null,
    AttachedNoteComponentIds: [],
    DetachedNoteComponentIds: [],

    Load: function (params) {
        Clinical_PatientEducation.params = params;
        if (Clinical_PatientEducation.params["PanelID"] != 'pnlClinicalPatientEducation')
            Clinical_PatientEducation.params["PanelID"] = Clinical_PatientEducation.params["PanelID"] + ' #pnlClinicalPatientEducation';

        Clinical_PatientEducation.params.GridInfoDocument = "dgvInfoDocument";
        Clinical_PatientEducation.params.GridNonInfoDocument = "dgvNonInfoDocument";

        if (Clinical_PatientEducation.params.ParentCtrl == 'clinicalTabProgressNote') {

            $("#" + Clinical_PatientEducation.params.PanelID + " #btnAddDocsToNote").show();
            $("#" + Clinical_PatientEducation.params.PanelID + " #btnAddInfoDocsToNote").show();
        }
        else {

            $("#" + Clinical_PatientEducation.params.PanelID + " #btnAddDocsToNote").hide();
            $("#" + Clinical_PatientEducation.params.PanelID + " #btnAddInfoDocsToNote").hide();
        }

        Clinical_PatientEducation.patientId = $("div#PatientProfile #hfPatientId").val();
        if (Clinical_PatientEducation.bIsFirstLoad == true) {
            Clinical_PatientEducation.ValidateDocument();
            Clinical_PatientEducation.bIsFirstLoad = false;
            Clinical_PatientEducation.BindAutocomplete();
        }
        Clinical_PatientEducation.DocumentSearch();
    },

    ValidateDocument: function () {
        $('#pnlClinicalPatientEducation  #frmClinicalPatientEducation')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   DocumentName: {
                       group: '.col-sm-6',
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
            Clinical_PatientEducation.DocumentSave();
        });
    },

    DocumentSave: function () {
        if ($("#" + Clinical_PatientEducation.params.PanelID + " #frmClinicalPatientEducation #txtDocument").val() !== "") {
            var self = $("#" + Clinical_PatientEducation.params.PanelID + " #frmClinicalPatientEducation");
            var myJson = self.getMyJSONByName();

            Clinical_PatientEducation.DocumentSaveDb(myJson).done(function (response) {
                response = JSON.parse(response);
                if (response.status !== false) {

                    utility.DisplayMessages(response.Message, 1);

                    $.when(Clinical_PatientEducation.DocumentSearch(null, null, null, response.PatEducationId)).then(function () {
                        if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
                            $("#pnlClinicalProgressNote #pnlClinicalPatientEducation #dgvNonInfoDocument input#" + response.PatEducationId).prop("checked", true);
                        }
                    });

                    $('#' + Clinical_PatientEducation.params.PanelID + " #frmClinicalPatientEducation").resetAllControls(null);
                    Clinical_PatientEducation.patientId = $("div#PatientProfile #hfPatientId").val();
                    $("#" + Clinical_PatientEducation.params.PanelID + " #txtComments").prop("disabled", true);
                    $('#' + Clinical_PatientEducation.params.PanelID + " #frmClinicalPatientEducation").bootstrapValidator('enableFieldValidators', 'DocumentName', false);
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
            });
        }
    },

    DocumentSaveDb: function (documentData) {
        var param = JSON.parse(documentData);
        param["PatientId"] = Clinical_PatientEducation.patientId;
        if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
            param["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            param["NoteId"] = "";
        }
        param["commandType"] = "insert_patienteducation";
        param["IsNonInfo"] = "Yes";
        param["DocType"] = "0";
        var data = JSON.stringify(param);

        return MDVisionService.APIService(data, "Medical", "PatientEducation");
    },

    DocumentSearch: function (type, pageNo, rpp, PatEducationId) {
        var dfd = $.Deferred();
        if ($("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result").css("display") == "none") {
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result").css("display", "inline");
        }

        var patientId = Clinical_PatientEducation.patientId;
        Clinical_PatientEducation.DocumentSearchDb(patientId, pageNo, rpp, type).done(function (response) {
            response = JSON.parse(response);
            if (response.status !== false) {
                if (response.DocumentCount > 0) {

                }
                else {
                    $("#" + Clinical_PatientEducation.params["PanelID"] + " #divInfoPaging").css("display", "none");
                    $("#" + Clinical_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "none");
                }
                if (response.InfoDocumentLoad_JSON !== "") {
                    Clinical_PatientEducation.InfoDocumentGridLoad(response, pageNo, rpp);
                }

                if (response.NonInfoDocumentLoad_JSON !== "") {
                    $.when(Clinical_PatientEducation.NonInfoDocumentGridLoad(response, pageNo, rpp, PatEducationId)).then(function () {
                        dfd.resolve();
                    });
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 2);
                dfd.resolve();
            }
        });
        return dfd;
    },
    addDocsToNotes: function (selectedAttachedOrders, selectedDetachedOrders, hideAlertMessage, fromUnload) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + Clinical_PatientEducation.params["PanelID"] + " #" + Clinical_PatientEducation.params.GridNonInfoDocument).dataTable().fnSettings().aoData.length == 0) {
            //   Clinical_ConsultationOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + Clinical_PatientEducation.params.PanelID + " #SelectRecordNonInfo").prop('checked') == true || $("#" + Clinical_PatientEducation.params.PanelID + " #SelectRecordNonInfo").prop('checked') == false) {

                $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridNonInfoDocument + " tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_PatientEducation.enableNonInfoDocument(this, null);
                });
            }


            var AttachedSelectedNonInfoDoc = [];
            var DettachedSelectedNonInfoDoc = [];
            if ((selectedAttachedOrders != '' && selectedAttachedOrders != null) || (selectedDetachedOrders != '' && selectedDetachedOrders != null)) {
                AttachedSelectedNonInfoDoc = selectedNonInfoDoc;
                DettachedSelectedNonInfoDoc = selectedNonInfoDoc;
            } else {
                var AttachSelectedOrdersAndResults = Clinical_PatientEducation.AttachedNoteComponentIds.slice();
                var DettachSelectedOrdersAndResults = Clinical_PatientEducation.DetachedNoteComponentIds.slice();
                AttachedSelectedNonInfoDoc = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "NonInfoDoc", 0, -10);
                DettachedSelectedNonInfoDoc = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "NonInfoDoc", 0, -10);
            }
            if (AttachedSelectedNonInfoDoc != null && AttachedSelectedNonInfoDoc != '') {
                for (var i = 0; i < Clinical_PatientEducation.AttachedNoteComponentIds.length; i++) {
                    var ALid = Clinical_PatientEducation.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_PatientEducation.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_PatientEducation_Main' + ALid).length != 0) {
                        var index = AttachedSelectedNonInfoDoc.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedNonInfoDoc.splice(index, 1);
                        }
                    }
                }
            }
            var detachedvalues = DettachedSelectedNonInfoDoc;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                $.when(Clinical_PatientEducation.detachPatientEducation(detachedvalues)).then(function () {
                    if (AttachedSelectedNonInfoDoc.join() != null && AttachedSelectedNonInfoDoc.join() != '') {
                        Clinical_PatientEducation.attachPatientEducatonWithNotes(AttachedSelectedNonInfoDoc, hideAlertMessage);
                        if (!fromUnload) {
                            utility.DisplayMessages('Successfully Updated', 1);
                        }
                    } else {
                        Clinical_ProgressNote.saveComponentSOAPText('Patient Education', hideAlertMessage);
                    }
                });
            } else if (AttachedSelectedNonInfoDoc.join() != null && AttachedSelectedNonInfoDoc.join() != '') {
                Clinical_PatientEducation.attachPatientEducatonWithNotes(AttachedSelectedNonInfoDoc, hideAlertMessage);
                if (!fromUnload) {
                    utility.DisplayMessages('Successfully Updated', 1);
                }
            }
            if (Clinical_PatientEducation.params && Clinical_PatientEducation.params.ParentCtrl && Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
                EMRUtility.scrollToPNcomponent('clinical_patienteducation');
                UnloadActionPan(Clinical_PatientEducation.params.ParentCtrl, 'Clinical_PatientEducation');
            }
        }
    },
    addInfoDocsToNotes: function (selectedAttachedOrders, selectedDetachedOrders, hideAlertMessage, fromUnload) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + Clinical_PatientEducation.params["PanelID"] + " #" + Clinical_PatientEducation.params.GridInfoDocument).dataTable().fnSettings().aoData.length == 0) {
            //   Clinical_ConsultationOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + Clinical_PatientEducation.params.PanelID + " #SelectRecordInfo").prop('checked') == true || $("#" + Clinical_PatientEducation.params.PanelID + " #SelectRecordInfo").prop('checked') == false) {

                $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_PatientEducation.enableInfoDocument(this, null);
                });
            }


            var AttachedSelectedInfoDoc = [];
            var DettachedSelectedInfoDoc = [];
            if ((selectedAttachedOrders != '' && selectedAttachedOrders != null) || (selectedDetachedOrders != '' && selectedDetachedOrders != null)) {
                AttachedSelectedInfoDoc = selectedNonInfoDoc;
                DettachedSelectedInfoDoc = selectedNonInfoDoc;
            } else {
                var AttachSelectedOrdersAndResults = Clinical_PatientEducation.AttachedNoteComponentIds.slice();
                var DettachSelectedOrdersAndResults = Clinical_PatientEducation.DetachedNoteComponentIds.slice();
                AttachedSelectedInfoDoc = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "NonInfoDoc", 0, -10);
                DettachedSelectedInfoDoc = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "NonInfoDoc", 0, -10);
            }
            if (AttachedSelectedInfoDoc != null && AttachedSelectedInfoDoc != '') {
                for (var i = 0; i < Clinical_PatientEducation.AttachedNoteComponentIds.length; i++) {
                    var ALid = Clinical_PatientEducation.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_PatientEducation.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_PatientEducation_Main' + ALid).length != 0) {
                        var index = AttachedSelectedInfoDoc.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedInfoDoc.splice(index, 1);
                        }
                    }
                }
            }
            var detachedvalues = DettachedSelectedInfoDoc;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                $.when(Clinical_PatientEducation.detachPatientEducation(detachedvalues)).then(function () {
                    if (AttachedSelectedInfoDoc.join() != null && AttachedSelectedInfoDoc.join() != '') {
                        Clinical_PatientEducation.attachPatientEducatonWithNotes(AttachedSelectedInfoDoc, hideAlertMessage);
                        utility.DisplayMessages('Successfully Updated', 1);
                        Clinical_ProgressNote.saveComponentSOAPText('Patient Education', hideAlertMessage);
                    }
                });
            } else if (AttachedSelectedInfoDoc.join() != null && AttachedSelectedInfoDoc.join() != '') {

                Clinical_PatientEducation.attachPatientEducatonWithNotes(AttachedSelectedInfoDoc, hideAlertMessage);
                utility.DisplayMessages('Successfully Updated', 1);
                Clinical_ProgressNote.saveComponentSOAPText('Patient Education', hideAlertMessage);
            }
        }
        if (Clinical_PatientEducation.params && Clinical_PatientEducation.params.ParentCtrl && Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.scrollToPNcomponent('clinical_patienteducation');
            UnloadActionPan(Clinical_PatientEducation.params.ParentCtrl, 'Clinical_PatientEducation');
        }
    },
    DocumentSearchDb: function (patientId, pageNumber, rowsPerPage, type) {
        if (patientId == null) {
            patientId = 0;
        }
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var param = {};
        param["PatientId"] = patientId;
        param["NoteId"] = Clinical_ProgressNote.params.NotesId;
        param["PageNumber"] = pageNumber;
        param["RowsPerPage"] = rowsPerPage;
        param["DocType"] = type;
        param["commandType"] = "load_patienteducation";

        var data = JSON.stringify(param);
        return MDVisionService.APIService(data, "Medical", "PatientEducation");
    },

    SetGrid: function (tab) {
        if (tab == "Info") {
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument + " thead").find("#threviewd").css("display", "");
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #AddDocument").css("display", "none");
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #Info").addClass("active");
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #NonInfo").removeClass("active");
        }
        else {
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #NonInfo").addClass("active");
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #Info").removeClass("active");

            $("#" + Clinical_PatientEducation.params["PanelID"] + " #AddDocument").css("display", "");
        }

    },
    checkUncheckInfoDocuments: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + Clinical_PatientEducation.params.PanelID + " input[name='SelectCheckBoxInfoDoc']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_PatientEducation.params.PanelID + " input[name='SelectCheckBoxInfoDoc']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " tbody").find('input[type="checkbox"]').each(function () {
            Clinical_PatientEducation.enableInfoDocument(this, null);
        });

    },



    enableInfoDocument: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " #SelectRecordNonInfo").prop("checked", true);
        }
        else {
            $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " #SelectRecordNonInfo").prop("checked", false);
        }

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'NonInfoDoc', Clinical_PatientEducation.AttachedNoteComponentIds) == -1) {
                Clinical_PatientEducation.AttachedNoteComponentIds.push(obj.id + 'NonInfoDoc');
            } if ($.inArray(obj.id + 'NonInfoDoc', Clinical_PatientEducation.DetachedNoteComponentIds) != -1) {
                var index = Clinical_PatientEducation.DetachedNoteComponentIds.indexOf(obj.id + 'NonInfoDoc');
                if (index > -1) {
                    Clinical_PatientEducation.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {

            var index = Clinical_PatientEducation.AttachedNoteComponentIds.indexOf(obj.id + 'NonInfoDoc');
            if (index > -1) {
                Clinical_PatientEducation.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'NonInfoDoc', Clinical_PatientEducation.DetachedNoteComponentIds) == -1) {
                Clinical_PatientEducation.DetachedNoteComponentIds.push(obj.id + 'NonInfoDoc');
            }
        }
    },
    InfoDocumentGridLoad: function (response, pageNo, rpp) {

        if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_PatientEducation.params.PanelID + " #dgvInfoDocument thead tr #SelectRecordInfo").length == 0) {
                $("#" + Clinical_PatientEducation.params.PanelID + " #dgvInfoDocument thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_PatientEducation.checkUncheckInfoDocuments(this);" controlname="SelectRecordInfo" id="SelectRecordInfo" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }

        } else {
            $("#" + Clinical_PatientEducation.params.PanelID + " #dgvInfoDocument th#SelectRecordInfo").remove();
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument")) {
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").dataTable().fnClearTable();
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").dataTable().fnDestroy();

        }
        $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument tbody").find("tr").remove();
        if (response.InfoCount > 0) {
            var infoDocumentLoadJsonData = JSON.parse(response.InfoDocumentLoad_JSON);
            $.each(infoDocumentLoadJsonData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Clinical_PatientEducation.DocumentPreview('" + item.PatientId + "', '" + item.PatDocId + "', '" + item.PatEducationId + "', event);utility.SelectGridRow($(this))");
                $row.attr("id", "dgvDocument_row" + item.PatEducationId);
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.PatEducationId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.PatEducationId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }


                if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_PatientEducation.enableInfoDocument(this,event);" id="' + item.PatEducationId + '" name="SelectCheckBoxInfoDoc" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //Info Panel
                $row.append(SelectionCheckBoxColumn + '<td><a class="btn  btn-xs" href="#" onclick="Clinical_PatientEducation.DocumentDelete(\'' + item.PatEducationId + '\', \'' + item.PatDocId + '\', event   );" title="Delete Record"><i class="fa fa-close red"></i></a></td><td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedByName + '</td>');
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridInfoDocument + " tbody").last().append($row);
            });

            var infoRows = $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridInfoDocument + " tbody").find("tr");
            if (infoRows.length < 1) {
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #divInfoPaging").css("display", "none");
                $('#' + Clinical_PatientEducation.params["PanelID"] + ' #' + Clinical_PatientEducation.params.GridInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }


            //------------Pagination Info Documents-----------
            var tableControl = Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridInfoDocument;
            var pagingPanelControlId = Clinical_PatientEducation.params["PanelID"] + " #divInfoPaging";
            var classControlName = "Clinical_PatientEducation";
            var pagesToDisplay = 5;
            var iTotalDisplayRecords = response.InfoCount;
            setTimeout(CreatePagination(response.InfoCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords, function (type, pageNumber, resultPerPage) {
                Clinical_PatientEducation.DocumentSearch(1, pageNumber, resultPerPage);
            }), 10);
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #divInfoPaging").css("display", "inline");
        }
        else {
            if (!$("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #divInfoPaging").css("display", "none");
                $('#' + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            } else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Document Found</td>');
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument tbody").last().append($row);
            }
        }

        if ($.fn.dataTable.isDataTable('#' + Clinical_PatientEducation.params["PanelID"] + ' #' + Clinical_PatientEducation.params.GridInfoDocument) || $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").parent().parent().hasClass("dataTables_wrapper"));
        else
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #" + Clinical_PatientEducation.params.GridInfoDocument + "").DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown


        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        // EMRUtility.fixDataTableDuplication("#" + Clinical_PatientEducation.params["PanelID"] + ' #pnlPatientEducation_Result');

    },
    checkUncheckNonInfoDocuments: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + Clinical_PatientEducation.params.PanelID + " input[name='SelectCheckBoxNonInfoDoc']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_PatientEducation.params.PanelID + " input[name='SelectCheckBoxNonInfoDoc']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridNonInfoDocument + " tbody").find('input[type="checkbox"]').each(function () {
            Clinical_PatientEducation.enableNonInfoDocument(this, null);
        });

    },

    enableNonInfoDocument: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " #SelectRecordInfo").prop("checked", true);
        }
        else {
            $("#" + Clinical_PatientEducation.params.PanelID + " #" + Clinical_PatientEducation.params.GridInfoDocument + " #SelectRecordInfo").prop("checked", false);
        }

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'NonInfoDoc', Clinical_PatientEducation.AttachedNoteComponentIds) == -1) {
                Clinical_PatientEducation.AttachedNoteComponentIds.push(obj.id + 'NonInfoDoc');
            } if ($.inArray(obj.id + 'NonInfoDoc', Clinical_PatientEducation.DetachedNoteComponentIds) != -1) {
                var index = Clinical_PatientEducation.DetachedNoteComponentIds.indexOf(obj.id + 'NonInfoDoc');
                if (index > -1) {
                    Clinical_PatientEducation.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {

            var index = Clinical_PatientEducation.AttachedNoteComponentIds.indexOf(obj.id + 'NonInfoDoc');
            if (index > -1) {
                Clinical_PatientEducation.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'NonInfoDoc', Clinical_PatientEducation.DetachedNoteComponentIds) == -1) {
                Clinical_PatientEducation.DetachedNoteComponentIds.push(obj.id + 'NonInfoDoc');
            }
        }
    },
    NonInfoDocumentGridLoad: function (response, pageNo, rpp, PatEducationId) {
        var dfd = $.Deferred();
        //$("#" + Clinical_PatientEducation.params["PanelID"] + " #" + Clinical_PatientEducation.params.GridNonInfoDocument + "").dataTable().fnDestroy();
        //$("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument + " tbody").find("tr").remove();
        if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_PatientEducation.params.PanelID + " #dgvNonInfoDocument thead tr #SelectRecordNonInfo").length == 0) {
                $("#" + Clinical_PatientEducation.params.PanelID + " #dgvNonInfoDocument thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_PatientEducation.checkUncheckNonInfoDocuments(this);" controlname="SelectRecordNonInfo" id="SelectRecordNonInfo" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }

        } else {
            $("#" + Clinical_PatientEducation.params.PanelID + " #dgvNonInfoDocument th#SelectRecord").remove();
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument")) {
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").dataTable().fnClearTable();
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").dataTable().fnDestroy();

        }
        $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument tbody").find("tr").remove();
        if (response.NonInfoCount > 0) {
            var nonInfoDocumentLoadJsonData = JSON.parse(response.NonInfoDocumentLoad_JSON);
            $.each(nonInfoDocumentLoadJsonData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Clinical_PatientEducation.DocumentPreview('" + item.PatientId + "', '" + item.PatDocId + "', '" + item.PatEducationId + "', event);utility.SelectGridRow($(this))");
                $row.attr("id", "dgvDocument_row" + item.PatEducationId);
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.PatEducationId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote" && item.PatEducationId == PatEducationId) {
                    Checked = " checked";
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.PatEducationId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }


                if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_PatientEducation.enableNonInfoDocument(this,event);" id="' + item.PatEducationId + '" name="SelectCheckBoxNonInfoDoc" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //Non-Info
                $row.append(SelectionCheckBoxColumn + '<td><a class="btn  btn-xs" href="#" onclick="Clinical_PatientEducation.DocumentDelete(\'' + item.PatEducationId + '\', \'' + item.PatDocId + '\', event   );" title="Delete Record"><i class="fa fa-close red"></i></a></td><td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedByName + '</td>');
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument + " tbody").last().append($row);

            });

            var nonInfoRows = $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument + " tbody").find("tr");

            if (nonInfoRows.length < 1) {
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "none");
                $('#' + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }

            //------------Pagination Non-Info Documents-----------
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "inline");

            var tableControl = Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument;
            var pagingPanelControlId = Clinical_PatientEducation.params["PanelID"] + " #divNonInfoPaging";
            var classControlName = "Clinical_PatientEducation";
            var pagesToDisplay = 5;
            var iTotalDisplayRecords = response.NonInfoCount;
            setTimeout(CreatePagination(response.NonInfoCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords, function (type, pageNumber, resultPerPage) {
                Clinical_PatientEducation.DocumentSearch(0, pageNumber, resultPerPage);
            }), 10);
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "inline");
            //------------End Pagination-------
        }
        else {
            if (!$("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "none");
                $('#' + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            } else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Document Found</td>');
                $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument tbody").last().append($row);
            }
        }
        if ($.fn.dataTable.isDataTable('#' + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument) || $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").parent().parent().hasClass("dataTables_wrapper"));
        else
            $("#" + Clinical_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + Clinical_PatientEducation.params.GridNonInfoDocument).DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        // EMRUtility.fixDataTableDuplication("#" + Clinical_PatientEducation.params["PanelID"] + ' #pnlPatientEducation_Result');
        dfd.resolve();
        return dfd;
    },

    DocumentPreview: function (patientId, patDocId, patEducationId, event) {
        if (event != null) {
            event.preventDefault();
        }
        utility.SelectGridRow($("dgvDocument_row" + patEducationId));
        var params = [];

        params["PatientID"] = patientId;
        params["PatDocID"] = patDocId;
        params["PanelID"] = "pnlClinicalPatientEducation";

        if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'Clinical_PatientEducation';
            PanelID = 'pnlClinicalProgressNote #pnlClinicalPatientEducation';
        } else {
            params["ParentCtrl"] = 'clinicalTabPatientEducation';
            PanelID = 'pnlClinicalPatientEducation';
        }


        LoadActionPan('Document_Viewer', params, PanelID);


    },

    DocumentDelete: function (patEducationId, patDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("dgvDocument_row" + patEducationId));
        utility.myConfirm("1", function () {
            Clinical_PatientEducation.DocumentDeleteDb(patEducationId, patDocId).done(function (response) {
                response = JSON.parse(response);
                if (response.status !== false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_PatientEducation.DocumentSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
            });

        });
    },

    DocumentDeleteDb: function (patEducationId, patDocId) {
        var param = {};
        param["PatientEducationId"] = patEducationId;
        param["DocumentId"] = patDocId;
        param["commandType"] = "delete_patienteducation";

        data = JSON.stringify(param);
        return MDVisionService.APIService(data, "Medical", "PatientEducation");
    },

    BindAutocomplete: function () {
        var Ctrl = $("#" + Clinical_PatientEducation.params["PanelID"] + " #txtDocument");
        var hfCtrl = $('#' + Clinical_PatientEducation.params["PanelID"] + ' #hfDocumentId');
        var func = function () { return Clinical_PatientEducation.GetDocumentArray(Ctrl.val()) };
        var onSelect = function (dataItem) {
            $("#pnlClinicalPatientEducation #txtComments").prop("disabled", false);
            if (dataItem)
            { $('#' + Clinical_PatientEducation.params["PanelID"] + ' #hfDocumentId').val(dataItem.id) }
        };
        var onChange = function (valid) { Clinical_PatientEducation.ValidateAutoComplete(); }
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },

    GetDocumentArray: function (documentName) {
        var isValid = false;
        var documentList = [];
        if (documentName != null && documentName.length > 2)
            isValid = true;
        else
            isValid = false;

        var dfd = new $.Deferred();
        if (isValid) {
            Clinical_PatientEducation.LookupDocument(documentName).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status !== false) {
                    if (responseData.DocumentCount > 0) {
                        var nonInfoDocumentLoadJson = JSON.parse(responseData.NonInfoDocumentLoad_JSON);
                        $.each(nonInfoDocumentLoadJson, function (i, item) {
                            documentList.push({ id: item.Item1, value: item.Item2 });
                        });
                    }
                }

                dfd.resolve(documentList);

            });
        }
        else {
            dfd.resolve(documentList);
        }

        return dfd.promise();
    },

    ValidateAutoComplete: function () {

        if ($("#pnlClinicalPatientEducation #hfDocumentId").val() !== "")
            $("#pnlClinicalPatientEducation #txtComments").prop("disabled", false);
        else
            $("#pnlClinicalPatientEducation #txtComments").prop("disabled", true);
    },

    LookupDocument: function (documentName) {
        var param = new Object();
        param["DocumentName"] = documentName;
        param["commandType"] = "lookup_patienteducation";

        data = JSON.stringify(param);
        return MDVisionService.APIService(data, "Medical", "PatientEducation");
    },

    UnLoadTab: function () {
        Clinical_PatientEducation.DetachedNoteComponentIds = [];
        Clinical_PatientEducation.AttachedNoteComponentIds = [];
        if (Clinical_PatientEducation.params != null && Clinical_PatientEducation.params.ParentCtrl != null) {
            if (Clinical_PatientEducation.params.ParentCtrl == "clinicalTabProgressNote") {
                var exist = false;
                $("#" + Clinical_PatientEducation.params.PanelID + " #dgvInfoDocument tbody").find('input[type="checkbox"]').each(function () {
                    if (this.checked) {
                        exist = true;
                    }
                    if (exist) {
                        return false;
                    }
                });
                $("#" + Clinical_PatientEducation.params.PanelID + " #dgvNonInfoDocument tbody").find('input[type="checkbox"]').each(function () {
                    if (this.checked) {
                        exist = true;
                    }
                    if (exist) {
                        return false;
                    }
                });
                if (exist) {
                    utility.myConfirmNote('1', function () {
                        EMRUtility.scrollToPNcomponent('clinical_patienteducation');
                        Clinical_PatientEducation.addInfoDocsToNotes(null, null, true);
                        Clinical_PatientEducation.addDocsToNotes(null, null, true, true);
                        UnloadActionPan(Clinical_PatientEducation.params.ParentCtrl, 'Clinical_PatientEducation');
                    }, "", function () {
                        EMRUtility.scrollToPNcomponent('clinical_patienteducation');
                        UnloadActionPan(Clinical_PatientEducation.params.ParentCtrl, 'Clinical_PatientEducation');
                    });
                }
                else {
                    EMRUtility.scrollToPNcomponent('clinical_patienteducation');
                    UnloadActionPan(Clinical_PatientEducation.params.ParentCtrl, 'Clinical_PatientEducation');
                }
            }
        }
        else
            UnloadActionPan(null, 'Clinical_PatientEducation');
    },


    detachPatientEducation: function (patientEducationId, fromNotes) {


        var selectedValue = patientEducationId.toString();
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {

            Clinical_PatientEducation.detachPatientEducationFromNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (fromNotes == "1") {
                        $('#Cli_PatientEducation_Main' + selectedValue).remove();
                    } else {
                        $.each(patientEducationId, function (index, value) {
                            $('#Cli_PatientEducation_Main' + value).remove();
                        });
                    }
                    if ($('#AllNonInfoDoc').parent().find('section').length == 0) {
                        $('#AllNonInfoDoc').remove();
                    }
                    if ($('#AllInfoDoc').parent().find('section').length == 0) {
                        $('#AllInfoDoc').remove();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });

        }

    },

    detachPatientEducationFromNotes: function (patientEducationId, fromNotes) {

        utility.myConfirm('Do you want to de-attach this record?', function () {
            EMRUtility.scrollToPNcomponent('clinical_patienteducation');
            var selectedValue = patientEducationId.toString();
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                Clinical_PatientEducation.detachPatientEducationFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (fromNotes == "1") {
                            $('#Cli_PatientEducation_Main' + selectedValue).remove();
                        } else {
                            $.each(patientEducationId, function (index, value) {
                                $('#Cli_PatientEducation_Main' + value).remove();
                            });
                        }
                        if ($('#AllNonInfoDoc').parent().find('section').length == 0) {
                            $('#AllNonInfoDoc').remove();
                        }
                        if ($('#AllInfoDoc').parent().find('section').length == 0) {
                            $('#AllInfoDoc').remove();
                        }
                        if ($('#AllNonInfoDoc').parent().find('section').length == 0 && $('#AllInfoDoc').parent().find('section').length == 0) {
                            $('#pnlClinicalProgressNote #ProgressnoteHTML clinical_patienteducation').parent().parent().find('#hardText').remove();
                        }

                        utility.DisplayMessages(response.Message, 1);
                        Clinical_ProgressNote.saveComponentSOAPText('Patient Education', true);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });

            }
        }, function () { },
            'Confirm Delete'
        );
    },


    detachPatientEducationFromNotes_DBCall: function (patientEducatioId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientEducationId"] = patientEducatioId;
        objData["commandType"] = "detach_patienteducation_from_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "PatientEducation");
    },

    getLatestPatientEducationByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        if (strMessage == "") {
            Clinical_PatientEducation.getLatestPatientEducationByPatientId_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_PatientEducation.createPatientEducationBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },

    getLatestPatientEducationByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_patienteducationby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "PatientEducation");

    },

    //This Function is used to create SOAP html and append it to  Progress note
    createPatientEducationBodyHTML: function (response, NoteHTMLCtrl, PatientEducationIds, hideAlertMessage, FromPatientEducation) {
        Clinical_PatientEducation.checkPatientEducationExists();
        //   $(NoteHTMLCtrl + ' li.patientEducation > div').remove();
        var PatientEducationSoap_JSON = JSON.parse(response.PatientEducationSoap_JSON);
        var $mainDiv = $(document.createElement('div'));
        var $mainDivinfo = $(document.createElement('div'));
        var $mainDivnoninfo = $(document.createElement('div'));
        if (PatientEducationSoap_JSON == null || PatientEducationSoap_JSON.length == 0) {
            Clinical_ProgressNote.saveComponentSOAPText('Patient Education', hideAlertMessage);
            return "";
        }
        var PListId = [];
        if (response.PatientEducationSoapCount > 0) {

            var infoArray = $.grep(PatientEducationSoap_JSON, function (element, index) {
                return element.DocType == 'True';
            });

            var nonInfoArray = $.grep(PatientEducationSoap_JSON, function (element, index) {
                return element.DocType == 'False';
            });
            if (infoArray.length > 0) {
                if ($('#AllInfoDoc').length == 0 || $('#AllInfoDoc').length == "")
                    $mainDivinfo.append('<div id="AllInfoDoc"><strong>Info Education Documents  </strong></div>');
            }
            if (nonInfoArray.length > 0) {
                if ($('#AllNonInfoDoc').length == 0 || $('#AllNonInfoDoc').length == "")
                    $mainDivnoninfo.append('<div id="AllNonInfoDoc"><strong>Non-Info Education Documents  </strong></div>');
            }

            $.each(infoArray, function (index, element) {
                var PEid = element.PatEducationId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_PatientEducation_Main" + PEid);
                $SectionBodyVital.attr('PatDocId', element.PatDocId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_PatientEducation_" + PEid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_PatientEducation_" + PEid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_PatientEducation_" + PEid + '"  ><i class="fa fa-times"></i></a></div> ');

                //if (index == 0) {
                //    $ListVital.append("<li><strong>Info Education Documents</strong></li>");
                //}

                $ListVital.append("<li>" + element.FilePath +
                    (element.CreatedOn == '' ? "" : " added on " + utility.RemoveTimeFromDate(null, element.CreatedOn) + "<li>")
                    );

                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);

                if ($(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid).length == 0) {
                    PListId.push(PEid);
                    $mainDivinfo.append($SectionBodyVital);
                    $mainDiv.append($mainDivinfo);
                } else {
                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' Cli_PatientEducation_Main').parent().parent().find('#Cli_PatientEducation_Main' + PEid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid + ' ul').append(CommentHTML);
                }

            });

            //if (PListId.join(",") != "") {
            //    PatientEducationIds = PListId.join(",");
            //}

            //if ($mainDivVital.html() != '') {
            //    Clinical_PatientEducation.updatePatientEducationHtml($mainDivVital.html(), PatientEducationIds, NoteHTMLCtrl);
            //} else {
            //   Clinical_PatientEducation.updatePatientEducationHtml('', PatientEducationIds, NoteHTMLCtrl);
            //    Clinical_ProgressNote.saveComponentSOAPText();
            //}

            //////////////////////////////////////


            $.each(nonInfoArray, function (index, element) {

                var PEid = element.PatEducationId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_PatientEducation_Main" + PEid);
                $SectionBodyVital.attr('PatDocId', element.PatDocId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_PatientEducation_" + PEid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_PatientEducation_" + PEid + '"><i class="fa fa-edit"></i></a>' +
                   '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_PatientEducation_" + PEid + '"  ><i class="fa fa-times"></i></a></div> ');

                //if (index == 0) {
                //    $ListVital.append("<li><strong>Non-Info Education Documents</strong></li>");
                //}
                $ListVital.append("<li>" + element.FilePath +
                    (element.CreatedOn == '' ? "" : " added on " + utility.RemoveTimeFromDate(null, element.CreatedOn))
                    );

                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);

                if ($(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid).length == 0) {
                    PListId.push(PEid);
                    $mainDivnoninfo.append($SectionBodyVital);
                    $mainDiv.append($mainDivnoninfo);
                } else {
                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' Cli_PatientEducation_Main').parent().parent().find('#Cli_PatientEducation_Main' + PEid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#Cli_PatientEducation_Main' + PEid + ' ul').append(CommentHTML);
                }

            });

            if (PListId.join(",") != "") {
                PatientEducationIds = PListId.join(",");
            }

            if ($mainDiv.html() != '') {
                Clinical_PatientEducation.updatePatientEducationHtml($mainDiv.html(), PatientEducationIds, NoteHTMLCtrl, hideAlertMessage, FromPatientEducation);
                Clinical_ProgressNote.saveComponentSOAPText('Patient Education', hideAlertMessage);
            } else {
                Clinical_PatientEducation.updatePatientEducationHtml('', PatientEducationIds, NoteHTMLCtrl, hideAlertMessage);
                Clinical_ProgressNote.saveComponentSOAPText('Patient Education', hideAlertMessage);
            }



        } else {
            Clinical_ProgressNote.saveComponentSOAPText('Patient Education', hideAlertMessage);
        }
    },
    updatePatientEducationHtml: function (PatientEducationHtml, PatientEducationIds, NoteHTMLCtrl, hideAlertMessage, fromPatienteducation) {
        $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().addClass('initialVisitBody').addClass("patientEducation");
        if (PatientEducationHtml != '') {
            $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().find('#hardText').remove();
            $(NoteHTMLCtrl + ' clinical_patienteducation').parent().parent().append(PatientEducationHtml);
            var hardHTML = '<ul id="hardText" class="list-unstyled"><li>Education was printed and given to patient.</li></ul>';
            $('.patientEducation').append(hardHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (PatientEducationHtml != '' && !fromPatienteducation) {
            Clinical_PatientEducation.attachPatientEducatonWithNotes(PatientEducationIds, hideAlertMessage);
        }

    },

    checkPatientEducationExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_patienteducation').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="PatientEducationComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_patienteducation title="Patient Education"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'PatientEducation\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Patient Education">Patient Education</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'PatientEducation\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'PatientEducation\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_patienteducation> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_patienteducation').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },

    detach_ComponentsPatientEducation: function (ComponentName, IsUpdate, ProblemListComponentRemove) {
        var Clinical_PatientEducationIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_patienteducation').parent().parent().find('section[id*="Cli_PatientEducation_Main"]').map(function () {
            return this.id.replace("Cli_PatientEducation_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .PatientEducationComponent').attr('NoteComponentId');

        if (Clinical_PatientEducationIds == "" || Clinical_PatientEducationIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_patienteducation').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Patient Education', true))
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
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_patienteducation').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                utility.DisplayMessages('Successfully Deleted', 1);
            });
        }
        else {

            Clinical_PatientEducation.detachPatientEducationFromNotes_DBCall(Clinical_PatientEducationIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Patient Education', true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (ProblemListComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_patienteducation').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Patient Education', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_patienteducation').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Patient Education']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_patienteducation').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Patient Education', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_patienteducation').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    utility.DisplayMessages('Successfully Deleted', 1);
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_patienteducation').parent().parent().find('section[id*="Cli_PatientEducation_Main"]').remove();
        }

    },
    getPatEducationForSOAP_DBCall: function (PatEducationId) {
        var objData = new Object();
        objData["PatientEducationId"] = PatEducationId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_PatientEducation.patientId;
        objData["commandType"] = "get_pateducation_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "PatientEducation");
    },
    attachPatientEducatonWithNotes: function (PatientEducationId, hideAlertMessage) {
        var strMessage = "";
        if (strMessage == "") {

            var selectedValue = PatientEducationId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                Clinical_PatientEducation.attachPatientEducationWithNotes_DBCall(selectedValue.toString()).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var dfd = new $.Deferred();
                        var data_ = JSON.parse(response.PatientEducationLoad_JSON);
                        if (data_.length > 0)
                        {
                            if (data_[0].MUAlertsCount && parseInt(data_[0].MUAlertsCount) > 0)
                                utility.toggelMU3Alerts(true, parseInt(data_[0].MUAlertsCount));
                        }

                        Clinical_PatientEducation.getPatEducationForSOAP_DBCall(selectedValue.toString()).done(function (response) {
                            if (response != "") {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    if (response.PatientEducationSoapCount > 0) {
                                        $.when(Clinical_PatientEducation.createPatientEducationBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', selectedValue.toString(), true, true)).then(function () {
                                            Clinical_ProgressNote.saveComponentSOAPText('Patient Education', true);
                                        });
                                    }
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            }
                            dfd.resolve('ok');
                        });
                        return dfd.promise();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    attachPatientEducationWithNotes_DBCall: function (PatientEducationId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientEducationId"] = PatientEducationId;
        objData["commandType"] = "attach_patienteducation_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "PatientEducation");
    },

}

