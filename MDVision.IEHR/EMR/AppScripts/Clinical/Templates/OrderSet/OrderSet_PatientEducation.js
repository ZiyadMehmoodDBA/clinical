OrderSet_PatientEducation = {
    bIsFirstLoad: true,
    params: [],
    IsAnyChange: false,
    AttachedNoteComponentIds: [],
    DetachedNoteComponentIds: [],
    Load: function (params) {
        OrderSet_PatientEducation.params = params;
        OrderSet_PatientEducation.IsAnyChange = false;

        if (OrderSet_PatientEducation.params["PanelID"] != 'pnlClinicalOrderSetPatientEducation')
            OrderSet_PatientEducation.params["PanelID"] = OrderSet_PatientEducation.params["PanelID"] + ' #pnlClinicalOrderSetPatientEducation';

        OrderSet_PatientEducation.params.GridInfoDocument = "dgvInfoDocument";
        OrderSet_PatientEducation.params.GridNonInfoDocument = "dgvNonInfoDocument";

        if (OrderSet_PatientEducation.bIsFirstLoad == true) {
            OrderSet_PatientEducation.ValidateDocument();
            OrderSet_PatientEducation.bIsFirstLoad = false;
            OrderSet_PatientEducation.BindAutocomplete();
        }
        OrderSet_PatientEducation.DocumentSearch();
    },

    ValidateDocument: function () {
        $('#pnlClinicalOrderSetPatientEducation  #frmClinicalOrderSetPatientEducation')
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
                       group: '.col-sm-2',
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
            OrderSet_PatientEducation.DocumentSave();
        });
    },

    DocumentSave: function () {
        if ($("#" + OrderSet_PatientEducation.params.PanelID + " #frmClinicalOrderSetPatientEducation #txtDocument").val() !== "" && $("#" + OrderSet_PatientEducation.params.PanelID + " #frmClinicalOrderSetPatientEducation #hfDocumentId").val() !== "") {
            var self = $("#" + OrderSet_PatientEducation.params.PanelID + " #frmClinicalOrderSetPatientEducation");
            var myJson = self.getMyJSONByName();

            OrderSet_PatientEducation.DocumentSaveDb(myJson).done(function (response) {
                response = JSON.parse(response);
                if (response.status !== false) {

                    OrderSet_PatientEducation.IsAnyChange = true;
                    utility.DisplayMessages(response.Message, 1);
                    OrderSet_PatientEducation.DocumentSearch();

                    $('#' + OrderSet_PatientEducation.params.PanelID + " #frmClinicalOrderSetPatientEducation").resetAllControls(null);
                    $("#" + OrderSet_PatientEducation.params.PanelID + " #txtComments").prop("disabled", true);
                    $('#' + OrderSet_PatientEducation.params.PanelID + " #frmClinicalOrderSetPatientEducation").bootstrapValidator('enableFieldValidators', 'DocumentName', false);
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
            });
        }
    },

    DocumentSaveDb: function (documentData) {
        var param = JSON.parse(documentData);
        param["commandType"] = "insert_ordersetpatienteducation";
        param["DocType"] = "0";
        param["OrderSetId"] = OrderSet_PatientEducation.params["OrderSetId"];
        var data = JSON.stringify(param);

        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientEducation");
    },

    DocumentSearch: function (type, pageNo, rpp) {
        if ($("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result").css("display") == "none") {
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result").css("display", "inline");
        }

        OrderSet_PatientEducation.DocumentSearchDb(pageNo, rpp, type).done(function (response) {
            response = JSON.parse(response);
            if (response.status !== false) {
                if (response.InfoDocumentLoad_JSON !== "") {
                    OrderSet_PatientEducation.InfoDocumentGridLoad(response, pageNo, rpp);
                }
                if (response.NonInfoDocumentLoad_JSON != "") {
                    OrderSet_PatientEducation.NonInfoDocumentGridLoad(response, pageNo, rpp);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },


    DocumentSearchDb: function (pageNumber, rowsPerPage, type) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var param = {};
        param["PageNumber"] = pageNumber;
        param["RowsPerPage"] = rowsPerPage;
        param["OrderSetId"] = OrderSet_PatientEducation.params["OrderSetId"];
        param["DocType"] = type;
        param["commandType"] = "load_ordersetpatienteducation";

        var data = JSON.stringify(param);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientEducation");
    },


    NonInfoDocumentGridLoad: function (response, pageNo, rpp) {


        if ($.fn.dataTable.isDataTable("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument")) {
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").dataTable().fnClearTable();
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").dataTable().fnDestroy();

        }
        $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument tbody").find("tr").remove();
        if (response.NonInfoCount > 0) {
            var nonInfoDocumentLoadJsonData = response.NonInfoDocumentLoad_JSON;
            $.each(nonInfoDocumentLoadJsonData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "OrderSet_PatientEducation.DocumentPreview('" + item.DocId + "', '" + item.OrderSetPatEducationId + "', event);utility.SelectGridRow($(this))");
                $row.attr("id", "dgvDocument_row" + item.OrderSetPatEducationId);


                //Non-Info
                $row.append('<td><a class="btn  btn-xs" href="#" onclick="OrderSet_PatientEducation.DocumentDelete(\'' + item.OrderSetPatEducationId + '\', \'' + item.DocId + '\', event   );" title="Delete Record"><i class="fa fa-close red"></i></a></td><td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedByName + '</td>');
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridNonInfoDocument + " tbody").last().append($row);

            });

            var nonInfoRows = $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridNonInfoDocument + " tbody").find("tr");

            if (nonInfoRows.length < 1) {
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "none");
                $('#' + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridNonInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }

            //------------Pagination Non-Info Documents-----------
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "inline");

            var tableControl = OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridNonInfoDocument;
            var pagingPanelControlId = OrderSet_PatientEducation.params["PanelID"] + " #divNonInfoPaging";
            var classControlName = "OrderSet_PatientEducation";
            var pagesToDisplay = 5;
            setTimeout(CreatePagination(response.NonInfoCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, response.NonInfoCount, function (type, pageNumber, resultPerPage) {
                OrderSet_PatientEducation.DocumentSearch(0, pageNumber, resultPerPage);
            }), 10);
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "inline");
            //------------End Pagination-------
        }
        else {
            if (!$("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #divNonInfoPaging").css("display", "none");
                $('#' + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridNonInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            } else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Document Found</td>');
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument tbody").last().append($row);
            }
        }
        if ($.fn.dataTable.isDataTable('#' + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridNonInfoDocument) || $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvNonInfoDocument").parent().parent().hasClass("dataTables_wrapper"));
        else
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridNonInfoDocument).DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        // EMRUtility.fixDataTableDuplication("#" + OrderSet_PatientEducation.params["PanelID"] + ' #pnlPatientEducation_Result');

    },

    DocumentPreview: function (patDocId, orderSetPatEducationId, event) {
        if (event != null) {
            event.preventDefault();
        }
        utility.SelectGridRow($("dgvDocument_row" + orderSetPatEducationId));
        var params = [];

        //params["PatientID"] = patientId;
        params["OrderSetPatEducationId"] = orderSetPatEducationId;
        params["PatDocID"] = patDocId;
        params["PanelID"] = "pnlClinicalOrderSetPatientEducation";
        params["ParentCtrl"] = "OrderSet_PatientEducation";
        PanelID = 'pnlClinicalOrderSetPatientEducation';
        LoadActionPan('Document_Viewer', params, PanelID);


    },

    DocumentDelete: function (OrderSetPatEducationId, DocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("dgvDocument_row" + OrderSetPatEducationId));
        utility.myConfirm("1", function () {
            OrderSet_PatientEducation.DocumentDeleteDb(OrderSetPatEducationId, DocId).done(function (response) {
                response = JSON.parse(response);
                if (response.status !== false) {
                    utility.DisplayMessages(response.Message, 1);
                    OrderSet_PatientEducation.DocumentSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
            });

        });
    },

    DocumentDeleteDb: function (OrderSetPatEducationId, DocId) {
        var param = {};
        param["OrderSetPatEducationId"] = OrderSetPatEducationId;
        param["DocId"] = DocId;
        param["commandType"] = "delete_ordersetpatienteducation";

        data = JSON.stringify(param);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientEducation");
    },

    BindAutocomplete: function () {
        var Ctrl = $("#" + OrderSet_PatientEducation.params["PanelID"] + " #txtDocument");
        var hfCtrl = $('#' + OrderSet_PatientEducation.params["PanelID"] + ' #hfDocumentId');
        var func = function () { return OrderSet_PatientEducation.GetDocumentArray(Ctrl.val()) };
        var onSelect = function (dataItem) { $("#" + OrderSet_PatientEducation.params["PanelID"] + " #txtComments").prop("disabled", false); };
        var onChange = function (valid) { OrderSet_PatientEducation.ValidateAutoComplete(); }
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
            OrderSet_PatientEducation.LookupDocument(documentName).done(function (responseData) {
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

        if ($("#" + OrderSet_PatientEducation.params["PanelID"] + " #hfDocumentId").val() !== "")
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #txtComments").prop("disabled", false);
        else
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #txtComments").prop("disabled", true);
    },

    LookupDocument: function (documentName) {
        var param = new Object();
        param["DocumentName"] = documentName;
        param["commandType"] = "lookup_patienteducation";

        data = JSON.stringify(param);
        return MDVisionService.APIService(data, "Medical", "PatientEducation");
    },

    UnLoadTab: function () {

        OrderSet_PatientEducation.IsAnyChange = true;
        if (OrderSet_PatientEducation.IsAnyChange == true && OrderSet_PatientEducation.params["ParentCtrl"] == "Clinical_OrderSetDetails") {
            Clinical_OrderSetDetails.LoadOrderSetPatientEducation();
        }

        if (OrderSet_PatientEducation.params != null && OrderSet_PatientEducation.params.ParentCtrl != null) {
            UnloadActionPan(OrderSet_PatientEducation.params.ParentCtrl, 'OrderSet_PatientEducation');
        }
        else
            UnloadActionPan(null, 'OrderSet_PatientEducation');
    },

    InfoDocumentGridLoad: function (response, pageNo, rpp) {
        if ($.fn.dataTable.isDataTable("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument")) {
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").dataTable().fnClearTable();
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").dataTable().fnDestroy();
        }
        $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument tbody").find("tr").remove();
        if (response.InfoCount > 0) {
            var infoDocumentLoadJsonData = response.InfoDocumentLoad_JSON;
            $.each(infoDocumentLoadJsonData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "OrderSet_PatientEducation.DocumentPreview('" + item.DocId + "', '" + item.OrderSetPatEducationId + "', event)");
                $row.attr("id", "dgvDocument_row" + item.PatEducationId);
                //Info Panel
                $row.append('<td><a class="btn  btn-xs" href="#" onclick="OrderSet_PatientEducation.DocumentDelete(\'' + item.OrderSetPatEducationId + '\', \'' + item.DocId + '\', event   );" title="Delete Record"><i class="fa fa-close red"></i></a></td><td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedByName + '</td>');
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridInfoDocument + " tbody").last().append($row);
            });

            var infoRows = $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridInfoDocument + " tbody").find("tr");
            if (infoRows.length < 1) {
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #divInfoPaging").css("display", "none");
                $('#' + OrderSet_PatientEducation.params["PanelID"] + ' #' + OrderSet_PatientEducation.params.GridInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }


            //------------Pagination Info Documents-----------
            var tableControl = OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridInfoDocument;
            var pagingPanelControlId = OrderSet_PatientEducation.params["PanelID"] + " #divInfoPaging";
            var classControlName = "OrderSet_PatientEducation";
            var pagesToDisplay = 5;
            var iTotalDisplayRecords = response.InfoCount;
            setTimeout(CreatePagination(response.InfoCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords, function (type, pageNumber, resultPerPage) {
                OrderSet_PatientEducation.DocumentSearch(1, pageNumber, resultPerPage);
            }), 10);
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #divInfoPaging").css("display", "inline");
        }
        else {
            if (!$("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #divInfoPaging").css("display", "none");
                $('#' + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #" + OrderSet_PatientEducation.params.GridInfoDocument).DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            } else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Document Found</td>');
                $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument tbody").last().append($row);
            }
        }

        if ($.fn.dataTable.isDataTable('#' + OrderSet_PatientEducation.params["PanelID"] + ' #' + OrderSet_PatientEducation.params.GridInfoDocument) || $("#" + OrderSet_PatientEducation.params["PanelID"] + " #pnlPatientEducation_Result #dgvInfoDocument").parent().parent().hasClass("dataTables_wrapper"));
        else
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #" + OrderSet_PatientEducation.params.GridInfoDocument + "").DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown


        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    SetGrid: function (tab) {
        if (tab == "Info") {
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #AddDocument").css("display", "none");
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #Info").addClass("active");
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #NonInfo").removeClass("active");
        }
        else {
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #NonInfo").addClass("active");
            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #Info").removeClass("active");

            $("#" + OrderSet_PatientEducation.params["PanelID"] + " #AddDocument").css("display", "");
        }

    },
}

