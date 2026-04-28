Patient_Document_Search = {

    bIsFirstLoad: true,
    params: [],
    SelectedDocs: [],
    FolderSearchType: "0",
    FavListName: "Patient_Document_Status",
    PriorityDDLLoad: false,
    interval: null,
    PrivateDoc: "",
    PasswordTries: "1",
    Load: function (parameters) {

        Patient_Document_Search.params = parameters;
        Patient_Document_Search.params["GridPatientDocument"] = "dgvPatientDocument_Search";
        Patient_Document_Search.params["GridRevDocument"] = "dgvPatRevDocument_Search";
        if (Patient_Document.params.ParentCtrl == "demographicDetail") {

            Patient_Document_Search.params.PatientID = Patient_Document.params["PatientId"];
            $("#" + Patient_Document_Search.params.PanelID + " #hfPatientId").val(Patient_Document.params["PatientId"]);
        }        
        Patient_Document_Search.params["PanelID"] = "pnlPatientDocument_Search";

        if (Patient_Document_Search.params['ParentCtrl'] == "Document_Scan") {          
            $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search #hfPatientId").val(Patient_Document_Search.params["PatientId"]);
        }
        //PRD-94 Start
        if (!$("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search #hfPatientId").val()) {
            utility.AddDaysFromToDate('frmPatientDocument_Search', 'dtpFromEntry', 'dtpToEntry', -15, 0);
        }
        //PRD-94 END
        var self = $('#' + Patient_Document_Search.params["PanelID"]);
        self.loadDropDowns(false);
        if (EMRUtility.getFavListStatus(Patient_Document.FavListName)) {
            Patient_Document_Search.FolderSearchType = "1";
        }
        else {
            Patient_Document_Search.FolderSearchType = "0";
        }
        Patient_Document_Search.params.PatientDocumentTree = "treeBasicDocumentScan";
        Patient_Document_Search.LoadFolders();
        Patient_Document_Search.DocumentSearch();
    },

    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {

        if (pagingDivId.indexOf("divReviewedPaging_Search") >= 0) {
            Patient_Document_Search.DocumentSearch(0, PageNo, 15, '1');
        }
        else if (pagingDivId.indexOf("divPendingPaging_Search") >= 0) {
            Patient_Document_Search.DocumentSearch(0, PageNo, 15, '0');
        }

    },

    SearchFolder: function () {
        if (Patient_Document_Search.FolderSearchType == "1") {
            Patient_Document_Search.FolderSearchType = "0";
            $('#' + Patient_Document_Search.params["PanelID"] + ' #FavType').html("Show All");
            EMRUtility.insertUpdateFavListStatus(Patient_Document_Search.FavListName, false);
        }
        else {
            Patient_Document_Search.FolderSearchType = "1";
            $('#' + Patient_Document_Search.params["PanelID"] + ' #FavType').html("Show Less");
            EMRUtility.insertUpdateFavListStatus(Patient_Document_Search.FavListName, true);
        }
        Patient_Document_Search.LoadFolders();
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo, FirstName, LastName, event) {
        if (event != null) {
            event.stopPropagation();
        }

        LastName = AccountNo.split(',')[0].split('-')[1];
        FirstName = AccountNo.split(',')[1];
        AccountNo = AccountNo.split('-')[0];
        var $ctr = $('#' + Patient_Document_Search.params["PanelID"] + ' #frmPatientDocument_Search #txtAccountNumber');
        var $hfctr = $('#' + Patient_Document_Search.params["PanelID"] + ' #frmPatientDocument_Search #hfPatientId');
        $ctr.val(AccountNo);
        $hfctr.val(PatientId);
        $('#' + Patient_Document_Search.params["PanelID"] + ' #frmPatientDocument_Search #txtPatientLastName').val(LastName);
        $('#' + Patient_Document_Search.params["PanelID"] + ' #frmPatientDocument_Search #txtPatientFirstName').val(FirstName);
        utility.SetAutoCompleteSource($ctr, $hfctr);
        // $("#" + Document_Scan.params["PanelID"] + " #txtAccountNumber").val(AccountNo);
        Patient_Search.UnLoad();
        utility.InsertRecentPatient(PatientId);
        $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search #hfPatientId").val(PatientId);
        Patient_Document_Search.LoadFolders(true);
        Patient_Document_Search.DocumentSearch();

    },
    DocumentSearch: function (DocumentId, PageNo, rpp, IsReviewed) {
        if ($("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search").css("display") == "none") {
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search").css("display", "inline");
        }

        var self = $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search");
        if (Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
            var patientId = Patient_Document.params["PatientId"];
        } else {
            var patientId = $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search #hfPatientId").val();
            if (!patientId) {
                patientId = Patient_Document_Search.params["PatientId"];
            }
        }
        if (patientId == "") {
            patientId = $("#PatientProfile #hfPatientId").val();
        }
        var myJSON = self.getMyJSON();
        myJSON = JSON.parse(myJSON);
        myJSON.ddlEnteredBy_text = $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search" + " #ddlEnteredBy option:selected").attr('refvalue');
        myJSON = JSON.stringify(myJSON);
        Patient_Document.SearchDocument(myJSON, patientId, PageNo, rpp, IsReviewed).done(function (response) {
            if (response.status != false) {


                var DocumentLoad_JSONData = [];
                var ReviewedDocumentLoad_JSONData = [];

                if (response.DocumentCount > 0) {
                    if (response.DocumentLoad_JSON != "")
                    { DocumentLoad_JSONData = JSON.parse(response.DocumentLoad_JSON); }
                    if (response.ReviewedDocumentLoad_JSON) {
                        ReviewedDocumentLoad_JSONData = JSON.parse(response.ReviewedDocumentLoad_JSON);
                    }
                }
                else {
                    $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging_Search").css("display", "none");
                    $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging_Search").css("display", "none");
                }

                if (response.DocumentCount > 0) {
                    if (response.ReviewedDocumentLoad_JSON) {
                        response.ReviewedDocumentLoad_JSON = JSON.parse(response.ReviewedDocumentLoad_JSON);
                    }
                    if (response.DocumentLoad_JSON != "")
                    { response.DocumentLoad_JSON = JSON.parse(response.DocumentLoad_JSON); }
                }
                if (response.PendingCount) {
                    Patient_Document_Search.DocumentGridLoad(response, PageNo, rpp);
                    $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search" + " #pendingDocument_Search").text(response.PendingCount);
                }
                else {
                    if (IsReviewed == (undefined)) {
                        Patient_Document_Search.DocumentGridLoad(response, PageNo, rpp);
                        $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search" + " #pendingDocument_Search").text(0);
                    }
                }

                //if (response.DocumentLoad_JSON != "") {

                //if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
                //    var totalRows = $("#" + Patient_Document.params.PanelID + " #dgvPatientDocument tr").length;
                //    totalRows -= 1;
                //    var selectedRows = $("#" + Patient_Document.params.PanelID + " #dgvPatientDocument tbody tr input:checked").length;
                //    if (totalRows == selectedRows) {
                //        $("#" + Patient_Document.params.PanelID + " #dgvPatientDocument tr #chkMasterPatDoc").prop("checked", true);
                //    }
                //    else {
                //        $("#" + Patient_Document.params.PanelID + " #dgvPatientDocument tr #chkMasterPatDoc").prop("checked", false);
                //    }
                //}
                //}
                if (response.ReviewedCount) {
                    Patient_Document_Search.ReviewedDocumentGridLoad(response, PageNo, rpp);
                    $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search" + " #ReviewedDocument_Search").text(response.ReviewedCount);
                }

                else {
                    if (IsReviewed == (undefined)) {
                        Patient_Document_Search.ReviewedDocumentGridLoad(response, PageNo, rpp);
                        $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search" + " #ReviewedDocument_Search").text(0);
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ReviewedDocumentGridLoad: function (response, PageNo, rpp) {
        //$("#" + Patient_Document_Search.params["PanelID"] + " #" + Patient_Document_Search.params.GridRevDocument + " #chkMasterPatDoc").prop("checked", false);
        if ($.fn.dataTable.isDataTable("#" + Patient_Document_Search.params["PanelID"] + " #" + Patient_Document_Search.params.GridRevDocument)) {
            $("#" + Patient_Document_Search.params["PanelID"] + " #" + Patient_Document_Search.params.GridRevDocument).dataTable().fnClearTable();
            $("#" + Patient_Document_Search.params["PanelID"] + " #" + Patient_Document_Search.params.GridRevDocument).dataTable().fnDestroy();
        }
        $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridRevDocument + " tbody").find("tr").remove();

        if (response.ReviewedCount > 0) {
            var ReviewedDocumentLoad_JSONData = response.ReviewedDocumentLoad_JSON;
            $.each(ReviewedDocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Patient_Document_Search.SelectDocument('" + item.FilePath + "'  ,'" + item.DocumentName + "'  ,'" + item.CreatedOn + "'  ,'" + item.PatientId + "','" + item.PatDocId + "',event);utility.SelectGridRow($(this))");

                $row.attr("id", "dgvPatRevDocument_Search_row" + item.PatDocId);
                $row.attr("DocumentId", item.PatDocId);
                $row.attr("FileType", item.FileType);

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
                var DocPriority = "";
                if (item.DocPrioirty.toLowerCase().trim() == "low") {
                    DocPriority = '<span class=green bold>' + item.DocPrioirty + '</span>';
                }
                else if (item.DocPrioirty.toLowerCase().trim() == "medium") {
                    DocPriority = '<span class=dark-yellow bold>' + item.DocPrioirty + '</span>';
                } else if (item.DocPrioirty.toLowerCase().trim() == "high") {
                    DocPriority = '<span class=red bold>' + item.DocPrioirty + '</span>';
                }
                var selectDocument = "";
                if (item.ReviewDate != "") {
                    $row.append('<td style="display:none;">' + item.PatDocId + '</td><td>' + item.DocumentName + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td data-toggle="tooltip" data-placement="left" class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                    $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridRevDocument + " tbody").last().append($row);
                }
            });

            var ReviewedRows = $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridRevDocument + " tbody").find("tr");

            if (ReviewedRows.length < 1) {
                $("#" + Patient_Document_Search.params["PanelID"] + " #divReviewedPaging_Search").css("display", "none");
                $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.GridRevDocument).DataTable({
                    "language": {
                        "emptyTable": "No Reviewed Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            //------------Pagination Reviewed Documents-----------
            $("#" + Patient_Document_Search.params["PanelID"] + " #divReviewedPaging_Search").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.ReviewedCount / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("" + Patient_Document_Search.params["PanelID"] + " #divReviewedPaging_Search", response.ReviewedCount, 5, "Patient_Document_Search", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.ReviewedCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.ReviewedCount;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.ReviewedCount + " Record(s)";
            $("#" + Patient_Document_Search.params["PanelID"] + " #divReviewedPaging_Search #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $('#' + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #divReviewedPaging_Search li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //------------End Pagination-------
        }
        else {
            $("#" + Patient_Document_Search.params["PanelID"] + " #divReviewedPaging_Search").css("display", "none");
            $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.GridRevDocument).DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Reviewed Document Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.GridRevDocument))
            ;
        else
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridRevDocument + "").DataTable({
                "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [1]
                }]
            }); // to remove records per page dropdown

        EMRUtility.fixDataTableDuplication("#" + Patient_Document_Search.params.PanelID + " #Reviewed_Search");
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    DocumentGridLoad: function (response, PageNo, rpp) {
        //$("#" + Patient_Document_Search.params["PanelID"] + " #" + Patient_Document_Search.params.GridPatientDocument + " #chkMasterPatDoc").prop("checked", false);
        $("#" + Patient_Document_Search.params["PanelID"] + " #" + Patient_Document_Search.params.GridPatientDocument).dataTable().fnDestroy();
        $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridPatientDocument + " tbody").find("tr").remove();

        if (response.PendingCount > 0) {
            var DocumentLoad_JSONData = response.DocumentLoad_JSON;
            $.each(DocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Patient_Document_Search.SelectDocument('" + item.FilePath + "'  ,'" + item.DocumentName + "'  ,'" + item.CreatedOn + "'  ,'" + item.PatientId + "'  ,'" + item.PatDocId + "',event);utility.SelectGridRow($(this))");

                $row.attr("id", "dgvPatientDocument_Search_row" + item.PatDocId);
                $row.attr("DocumentId", item.PatDocId);
                $row.attr("FileType", item.FileType);

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
                var DocPriority = "";
                if (item.DocPrioirty) {
                    if (item.DocPrioirty.toLowerCase().trim() == "low") {
                        DocPriority = '<span class=green bold>' + item.DocPrioirty + '</span>';
                    }
                    else if (item.DocPrioirty.toLowerCase().trim() == "medium") {
                        DocPriority = '<span class=dark-yellow bold>' + item.DocPrioirty + '</span>';
                    } else if (item.DocPrioirty.toLowerCase().trim() == "high") {
                        DocPriority = '<span class=red bold>' + item.DocPrioirty + '</span>';
                    }
                }
                var selectDocument = "";
                if (item.ReviewBy == "") {
                    $row.append('<td style="display:none;">' + item.PatDocId + '</td><td>' + item.DocumentName + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedByName + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td data-toggle="tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                    $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridPatientDocument + " tbody").last().append($row);
                }
            });
            var pendingRows = $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridPatientDocument + " tbody").find("tr");
            if (pendingRows.length < 1) {
                $("#" + Patient_Document_Search.params["PanelID"] + " #divPendingPaging_Search").css("display", "none");
                $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.GridPatientDocument).DataTable({
                    "language": {
                        "emptyTable": "No Pending Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }

            //------------Pagination Pending Documents-----------
            $("#" + Patient_Document_Search.params["PanelID"] + " #divPendingPaging_Search").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.PendingCount / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("" + Patient_Document_Search.params["PanelID"] + " #divPendingPaging_Search", response.PendingCount, 5, "Patient_Document_Search", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.PendingCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.PendingCount;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.PendingCount + " Record(s)";
            $("#" + Patient_Document_Search.params["PanelID"] + " #divPendingPaging_Search #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $('#' + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #divPendingPaging_Search li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //------------End Pagination-------
        }
        else {
            $("#" + Patient_Document_Search.params["PanelID"] + " #divPendingPaging").css("display", "none");
            $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.GridPatientDocument).DataTable({
                "language": {
                    "emptyTable": "No Pending Document Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.GridPatientDocument))
            ;
        else
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document_Search.params.GridPatientDocument + "").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        EMRUtility.fixDataTableDuplication("#" + Patient_Document_Search.params.PanelID + " #Pending");
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    SelectDocument: function (FilePath, DocumentName, CreatedOn, PatientId, PatDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        Patient_Document.CheckforPrivacy(PatientId, PatDocId).done(function (response) {
            if (response.status == true) {
                if (response.DocPasswordInfoCount > 0) {
                    Patient_Document_Search.PrivateDoc = response.DocPasswordInfo[0].Password;
                    ShowPasswordAlert = response.DocPasswordInfo[0].ShowPasswordAlert;
                }
            }
            if ((Patient_Document_Search.PrivateDoc == "" && ShowPasswordAlert != "True") || (Patient_Document_Search.PrivateDoc != "" && ShowPasswordAlert == "True")) {
                utility.myConfirm('58', function () {
                    Patient_Document_Search.AppendSelectedDocument(FilePath, DocumentName, CreatedOn, PatientId, PatDocId);
                });
            } else {
                if ($('body').find('#modal-from-dom-DocumentConfirmPasswordLink').length < 1) {
                    Patient_Document_Search.VerifyPassword(PatientId, PatDocId, event, FilePath, DocumentName, CreatedOn);
                } else {
                    $('body').find('#modal-from-dom-DocumentConfirmPasswordLink').modal("show");
                    $("#DivConfirmPasswordLink #TxtDocPassword").val("");
                    $("#modal-from-dom-DocumentConfirmPasswordLink #DivConfirmPasswordLink #btnDocumentScan").attr("onclick", "Patient_Document_Search.DocPasswordMatch('" + PatientId + "', '" + PatDocId + "', '" + FilePath + "', '" + DocumentName + "', '" + CreatedOn + "');");
                }
            }
        });
    },

    AppendSelectedDocument: function (FilePath, DocumentName, CreatedOn, PatientId, PatDocId) {
        var matches = $.grep(Document_Scan.AttachedDocsArray, function (e) { return e.PatDocId == PatDocId });
        if (matches && matches.length > 0) {
            utility.DisplayMessages("Document is already added.", 3);
        } else {
            $("#ScanImageAttachtbl tbody").children().removeClass('active');
            var $row = $('<tr/>');
            $row.attr("onclick", "Patient_Document_Search.GetSelectedDocument('" + PatientId + "'  ,'" + PatDocId + "',event);utility.SelectGridRow($(this))");
            $row.attr("id", "dgvPatientDocument_Attach_row" + PatDocId);
            $row.attr("class", "active");
            var AttachedDocs = new Object();
            AttachedDocs.FilePath = FilePath
            AttachedDocs.DocumentName = DocumentName;
            AttachedDocs.CreatedOn = CreatedOn;
            AttachedDocs.PatientId = PatientId;
            AttachedDocs.PatDocId = PatDocId;
            $row.attr("DocumentId", PatDocId);
            $row.attr("PatientId", PatientId);
            $row.append('<td> ' + FilePath + '</td><td>' + DocumentName + '</td><td>' + utility.RemoveTimeFromDate(null, CreatedOn) + '</td><td class="text-center"><a class="btn  btn-xs" href="#" onclick="Patient_Document_Search.DocumentDelete(\'' + PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a></td>');
            Document_Viewer.FillDocument(null, PatientId, PatDocId).done(function (response) {
                if (response.status != false) {
                    var document_details = JSON.parse(response.DocumentLoad_JSON);
                    $("#" + Document_Scan.params["PanelID"] + " #div_ScanImageAttach #ScanImageAttachtbl tbody").prepend($row);
                    AttachedDocs.byteArray = document_details.Base64FileStream;
                    AttachedDocs.FileType = document_details.FileType;
                    Document_Scan.AttachedDocsArray.push(AttachedDocs);
                    $("#frmDocumentScan #dwtcontrolContainer").parent().addClass("hide");
                    $("#frmDocumentScan #dwtcontrolContainerAttach").removeClass("hide");
                    if (document_details.FileType.indexOf("pdf") > -1) {
                        $("#frmDocumentScan #canvasContainerAttach").addClass("hide");
                        $("#frmDocumentScan #OpenDocumentIFAttach").removeClass("hide");
                        Patient_Document_Search.PDFViewer(document_details.Base64FileStream, false, 'frmDocumentScan #OpenDocumentIFAttach');
                    } else {
                        $("#frmDocumentScan #canvasContainerAttach").removeClass("hide");
                        $("#frmDocumentScan #OpenDocumentIFAttach").addClass("hide");
                        try {
                            var imageObj = new Image();
                            //for IE
                            //  imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;
                            canvas = document.getElementById("canvasAttach");
                            context = canvas.getContext("2d");
                            imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;
                            setTimeout(function () {
                                canvas.width = imageObj.width;
                                canvas.height = imageObj.height;
                            }, 1000);

                            //Patient_Document_Search.context.clearRect(0, 0, Patient_Document_Search.canvas.width, Patient_Document_Search.canvas.height);
                            function draw() {
                                var scale = 1;
                                var originx = 0;
                                var originy = 0;
                                context.save();
                                context.setTransform(1, 0, 0, 1, 0, 0);
                                context.clearRect(0, 0, canvas.width, canvas.height);
                                context.restore();
                                context.drawImage(imageObj, 0, 0);
                            }

                            setTimeout(function () { draw(); }, 1000);
                            //Patient_Document_Search.interval = setInterval(function () {
                            //    draw();
                            //}, 1000);
                        }
                        catch (ex) {
                            utility.DisplayMessages(ex, 2);
                            console.log(ex);
                        }
                    }
                    Patient_Document_Search.UnLoadTab();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    VerifyPassword: function (PatientID, PatDocID, event, FilePath, DocumentName, CreatedOn) {
        var DivFormGroup = '<div class="form-group">';
        var DivEnd = '</div>'
        var Password = '<input type="password" name="DocPassword" id="TxtDocPassword" class="form-control" autofocus>';
        var dialogTitle = "Confirm Password";
        var Required = '<span class="required">*</span>';
        var Clearfix = '<div class="clearfix"></div>';
        var DocMatchFunction = "Patient_Document_Search.DocPasswordMatch('" + PatientID + "', '" + PatDocID + "', '" + FilePath + "', '" + DocumentName + "', '" + CreatedOn + "');";

        var markUp = '';
        markUp = '<div id="modal-from-dom-DocumentConfirmPasswordLink" class="modal fade">' +
                        '<div class="modal-dialog modal-dialog-sm modal-top-adjust">' +
                            '<div class="modal-content">'
                            + '<div class="modal-header">' + '<button type="button" onclick="Patient_Document_Search.cancelConfirmDialog();"  class="close" "></button>'
                                + '<h4 class="modal-title">' + dialogTitle + ' </h4>'
                            + DivEnd
                                + '<div class="modal-body bg-white" id="DivConfirmPasswordLink">'
                                    + '<div class="col-xs-6"><label class="control-label">Confirm Password' + Required + '</label>' + Password + DivEnd + Clearfix
                                    + DivFormGroup
                                        + '<div class="col-xs-12 pad-a-labelsize-btn">'
                                            + '<button id="btnDocumentScan" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="' + DocMatchFunction + '">Ok</button>'
                                        + DivEnd
                                    + DivEnd
                                 + DivEnd
                        + DivEnd
                    + DivEnd
                + '</div><div></div>';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {

        }).on('hidden.bs.modal', function () {
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }
            }
        });

        $("#DivConfirmPasswordLink #TxtDocPassword").val("");
    },

    cancelConfirmDialog: function () {
        $("#modal-from-dom-DocumentConfirmPasswordLink").modal('hide');
        //$('body').find('.modal-backdrop').removeClass('modal-backdrop');
    },

    DocPasswordMatch: function (PatientID, PatDocID, FilePath, DocumentName, CreatedOn) {
        var TypedPassword = "";
        if (parseInt(Patient_Document_Search.PasswordTries) >= 0 && parseInt(Patient_Document_Search.PasswordTries) <= 3) {
            TypedPassword = $("#modal-from-dom-DocumentConfirmPasswordLink #DivConfirmPasswordLink #TxtDocPassword").val();

            Patient_Document.MatchDocPassword(PatDocID, TypedPassword).done(function (response) {
                if (response.status != false) {
                    Patient_Document_Search.cancelConfirmDialog();
                    Patient_Document_Search.AppendSelectedDocument(FilePath, DocumentName, CreatedOn, PatientID, PatDocID);
                    Patient_Document_Search.PasswordTries = 0;
                } else {
                    utility.DisplayMessages("Password is incorrect or You do not have Access to the document.", 2);
                    if (parseInt(Patient_Document_Search.PasswordTries) == 3) {
                        Patient_Document_Search.PasswordTries = 0;
                    }
                }
                Patient_Document_Search.PasswordTries = parseInt(Patient_Document_Search.PasswordTries) + 1;
            });
        } else {
            $("#modal-from-dom-DocumentConfirmPasswordLink").modal('hide');
            $('body').find('.modal-backdrop').removeClass('modal-backdrop');
        }
    },

    PDFViewer: function (base64, IsnewTab, ObjectControlID, IsIframe, IsPrint) {
        //if (utility.UserBrowser() == "Firefox") {
        var byteCharacters = atob(base64);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: "application/pdf" });
        var blobUrl = URL.createObjectURL(blob);
        if (IsIframe) {
            $('#' + ObjectControlID).attr('src', blobUrl);
        } else {
            $('#' + ObjectControlID).attr('data', blobUrl);
        }
        //}
        //else {
        //    $("#helperPDFAttach").val(base64);
        //    if (IsIframe) {
        //        $('#' + ObjectControlID).attr('src', 'Scripts/js/pdf/web/viewer.html');
        //    } else {
        //        $('#' + ObjectControlID).attr('data', 'Scripts/js/pdf/web/viewer.html');
        //    }
        //}
    },


    DocumentDelete: function (PatDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        Document_Scan.AttachedDocsArray = $.grep(Document_Scan.AttachedDocsArray, function (e) { return e.PatDocId != PatDocId });
        if ($("#dgvPatientDocument_Attach_row" + PatDocId).hasClass("active")) {
            $("#frmDocumentScan #dwtcontrolContainer").parent().removeClass("hide");
            $("#frmDocumentScan #dwtcontrolContainerAttach").addClass("hide");
        }
        $("#dgvPatientDocument_Attach_row" + PatDocId).remove();
    },

    GetSelectedDocument: function (PatientId, PatDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var matches = $.grep(Document_Scan.AttachedDocsArray, function (e) { return e.PatDocId == PatDocId });
        $("#frmDocumentScan #dwtcontrolContainer").parent().addClass("hide");
        $("#frmDocumentScan #dwtcontrolContainerAttach").removeClass("hide");
        if (matches && matches.length > 0 && matches[0].FileType.indexOf("image") > -1) {
            try {
                var imageObj = new Image();
                canvas = document.getElementById("canvasAttach");
                imageObj.src = "data:" + matches[0].FileType + ";base64," + matches[0].byteArray;
                setTimeout(function () {
                    canvas.width = imageObj.width;
                    canvas.height = imageObj.height;
                }, 1000);
                context = canvas.getContext("2d");
                function draw() {
                    var scale = 1;
                    var originx = 0;
                    var originy = 0;
                    context.save();
                    context.setTransform(1, 0, 0, 1, 0, 0);
                    context.clearRect(0, 0, canvas.width, canvas.height);
                    context.restore();
                    context.drawImage(imageObj, 0, 0);
                }
                setTimeout(function () {
                    draw();
                    $("#frmDocumentScan #canvasContainerAttach").removeClass("hide");
                    $("#frmDocumentScan #OpenDocumentIFAttach").addClass("hide");
                }, 1000);
            }
            catch (ex) {
                utility.DisplayMessages(ex, 2);
                console.log(ex);
            }
        } else if (matches && matches.length > 0 && matches[0].FileType.indexOf("pdf") > -1) {
            $("#frmDocumentScan #canvasContainerAttach").addClass("hide");
            $("#frmDocumentScan #OpenDocumentIFAttach").removeClass("hide");
            Patient_Document_Search.PDFViewer(matches[0].byteArray, false, 'frmDocumentScan #OpenDocumentIFAttach');
        }
    },

    DocumentFill: function (PatientID, PatDocID) {
        var documentCall = Document_Viewer.FillDocument(null, PatientID, PatDocID);
        $.when(documentCall).done(function (response) {

        });
    },

    UnLoadTab: function () {
        UnloadActionPan(Patient_Document_Search.params.ParentCtrl, 'Patient_Document_Search');
        if ($('.modal-backdrop').length > 0) {
            $('.modal-backdrop').remove();
        }
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        //if (Patient_Document.params.ParentCtrl == "demographicDetail") {
        params["ParentCtrl"] = "Patient_Document_Search";
        //} else {
        //    params["ParentCtrl"] = Patient_Document.params["TabID"];
        //}
        LoadActionPan('Patient_Search', params);
    },

    ValidateAutoComplete: function (obj) {

        utility.ValidateAutoComplete(obj, Patient_Document_Search.params["PanelID"] + ' #hfPatientId', false);
    },

    LoadFolders: function (FromLoad) {
        var def = $.Deferred();
        var patientId = $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search #hfPatientId").val();
        if (!patientId)
            patientId = $("#PatientProfile #hfPatientId").val();


        if ($("#PatientProfile #hfPatientId").val() == "") {
            patientId = "";
            $("#" + Patient_Document_Search.params["PanelID"] + " #txtAccountNumber").val("");

        }
        var self = $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search #divGridView");
        var myJSON = self.getMyJSON();
        myJSON = JSON.parse(myJSON);
        myJSON.ddlEnteredBy_text = $("#" + Patient_Document_Search.params["PanelID"] + " #frmPatientDocument_Search" + " #ddlEnteredBy option:selected").attr('refvalue');
        myJSON = JSON.stringify(myJSON);
        Patient_Document.GetPatientDocument(myJSON, patientId).done(function (response) {
            if (response.status == true) {

                $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.PatientDocumentTree).jstree("destroy");
                $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.PatientDocumentTree).html("");
                $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.PatientDocumentTree).bind("select_node.jstree", function (e, data) {
                    Patient_Document_Search.TogglingView(data);
                });
                var Folders_JSON = JSON.parse(response.Folders_JSON);
                var FolderChild_JSON = JSON.parse(response.FolderChild_JSON);
                if (Patient_Document_Search.FolderSearchType == "0") {
                    var Folders_JSON_New = [];
                    $.each(Folders_JSON, function (i, item) {
                        if (item.FolderCount > 0) {
                            Folders_JSON_New.push(item);
                        }
                    });
                    Folders_JSON = Folders_JSON_New;
                }

                var pair = [];

                $.each(Folders_JSON, function (i, item) {

                    pair.push({ "id": item.DocId, "parent": "#", "text": item.ShortName, "FolderCount": item.FolderCount, 'icon': 'fa  fa-folder blue', 'li_attr': { 'class': 'rootFolder' } });
                });

                $.each(FolderChild_JSON, function (i, item) {
                    if (item.IsReviewed == 1) {
                        pair.push({ "id": item.DocumentId, "parent": item.FolderId, "text": item.DocumentName, "FolderCount": item.DocCount, "IsDocumentReviewed": item.IsReviewed, "DocumentFolderName": item.DocumentFolderName, "PatientId": item.PatientId, 'icon': 'fa fa-file blue', 'li_attr': { 'class': 'searchData' } });
                    }
                    else {
                        pair.push({ "id": item.DocumentId, "parent": item.FolderId, "text": item.DocumentName, "FolderCount": item.DocCount, "IsDocumentReviewed": item.IsReviewed, "DocumentFolderName": item.DocumentFolderName, "PatientId": item.PatientId, 'icon': 'fa fa-file blue', 'li_attr': { 'class': 'red  searchData' } });

                    }
                });
                $('#' + Patient_Document_Search.params["PanelID"] + ' #' + Patient_Document_Search.params.PatientDocumentTree).jstree({
                    'core': {
                        'check_callback': true,
                        'data': pair
                    },
                    "search": {
                        "case_insensitive": true,
                        "show_only_matches": true,
                        "fuzzy": false,
                    },
                    "root": {
                        "icon": "/Content/Default/images/tree_icon.png"//,
                    },
                    'sort': function (a, b) {
                        a1 = this.get_node(a);
                        b1 = this.get_node(b);
                        if (a1.icon == b1.icon) {
                            return (a1.text > b1.text) ? 1 : -1;
                        } else {
                            return (a1.icon > b1.icon) ? 1 : -1;
                        }
                    },

                    "plugins": ["search", 'types', "sort"]
                });




            } // end if
            //call to get document
            if (FromLoad) {
                if (Patient_Document.FolderSearchType == "1") {
                    $('#' + Patient_Document_Search.params["PanelID"] + ' #FavType').html("Show Less");
                }
                else {
                    $('#' + Patient_Document_Search.params["PanelID"] + ' #FavType').html("Show All");
                }
            }
            def.resolve();
        });
        return def;


        //var $treeview = $("#treeBasicDocument");
        //$treeview
        //.jstree(options)
        //.on('loaded.jstree', function () {
        //    $treeview.jstree('open_all');
        //});
    },

    SetGrid: function (tab) {

        if (tab == "Pending") {

            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document.params.GridRevDocument + " thead").find("#threviewd").css("display", "");
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document.params.GridRevDocument + " thead").find("#threviewddate").css("display", "");
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #" + Patient_Document.params.GridRevDocument + " thead").find("#thsigned").css("display", "");
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #Pending_Search").show();
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #Reviewed_Search").hide();
        }
        else {
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #Pending_Search").hide();
            $("#" + Patient_Document_Search.params["PanelID"] + " #pnlPatientDocument_Result_Search #Reviewed_Search").show();
        }
    },
    TogglingView: function (data) {
        var id = data.node.id;
        var ParentId = data.node.parent;
        var FolderCount = data.node.original.FolderCount;
        var IsReviewed = data.node.original.IsDocumentReviewed;
        var PatientId = data.node.original.PatientId;
        var FolderName = data.node.original.DocumentFolderName;
        var folderText = data.node.original.text;
        if (folderText.indexOf('(') > -1) {
            folderText = folderText.split('(')[0].trim();
        }

        if (ParentId == "#") {
            var FolderId = id;
            $('#' + Patient_Document_Search.params["PanelID"] + ' #hfDocumentId').val(id);
            Patient_Document_Search.DocumentSearch();
        }
    },

}

function ListItemClick_Search(obj, Id) {
    $("#" + Patient_Document_Search.params["PanelID"] + " #lstDocument_Search li").each(function () {
        if ($(this).val() == Id) {
            $(this).attr("class", "active");
        }
        else
            $(this).removeAttr("class");
    });
    $('#' + Patient_Document_Search.params["PanelID"] + ' #hfDocumentId').val(Id);
    Patient_Document_Search.DocumentSearch();
}

function ListItemHighLight_Search(Id) {

    var IsHighlighted = false;

    var listItems = $("#" + Patient_Document_Search.params["PanelID"] + " #lstDocument_Search li");
    listItems.each(function (indx, li) {
        var item = $(li);
        if (item.val() == Id) {
            $(item).attr("class", "active");
            IsHighlighted = true;
        }

        // and the rest of your code
    });
    if (!IsHighlighted) {
        Patient_Document_Search.LoadFolders();
        // $('#' + Patient_Document.params["PanelID"] + ' #lstDocument li:first').click();
    }
}