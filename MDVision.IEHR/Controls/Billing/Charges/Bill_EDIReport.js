Bill_EDIReport = {

    bIsFirstLoad: true,
    Load: function (params) {

        Bill_EDIReport.params = params;

        if (Bill_EDIReport.bIsFirstLoad) {
            Bill_EDIReport.bIsFirstLoad = false;
            var self = "";
            if (Bill_EDIReport.params["PanelID"] != "pnlBillEDIReport") {
                self = $("#pnlBillEDIReport");
                Bill_EDIReport.params["PanelID"] = "pnlBillEDIReport";
            }
            else
                self = $('#' + Bill_EDIReport.params["PanelID"]);
            self.loadDropDowns(true);
            Bill_EDIReport.EDIReportSearch();
        }

        utility.CreateDatePicker('frmBillEDIReport #DownloadDate', function () { });

    },

    Bill_EDIReportReset: function () {
        $('#' + Bill_EDIReport.params["PanelID"] + ' #frmBillEDIReport').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_EDIReport.params["PanelID"] + ' #frmBillEDIReport').resetAllControls();
        $('#' + Bill_EDIReport.params["PanelID"] + ' #frmBillEDIReport [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
    },

    ValidateClearinghouse: function () {

        if ($("#pnlBillEDIReport #Clearinghouse").val() == "") {
            $('#Import_edi_file').val('');
            $("#pnlBillEDIReport #Clearinghouse").closest("div").addClass("has-feedback has-error");
            utility.DisplayMessages("Clearinghouse is not selected.", 3);
            return false;
        }
        else {
            $("#pnlBillEDIReport #Clearinghouse").closest("div").removeClass("has-feedback has-error");
            return true;
        }
    },

    Attach_EDIReport: function (obj, isBatch) {

        if (Bill_EDIReport.ValidateClearinghouse()) {
            EDIBatchDetail.Attach_EDIReport(obj, isBatch, $("#pnlBillEDIReport #Clearinghouse").val());
        }
    },

    EDIReportSearch: function (PageNo, rpp) {
        var strMessage = "";
        $("#pnlBillEDIReport #Clearinghouse").closest("div").removeClass("has-feedback has-error");
        //AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result').css("display") == "none") {
                $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result').show();
            }

            var self = $('#frmBillEDIReport');
            var myJSON = self.getMyJSONByName();

            Bill_EDIReport.SearchEDIReport(myJSON, "", "", PageNo, rpp).done(function (response) {
                if (response.status != false) {

                    //-----------------Pagination------

                    if (response.EDIReportCount > 0) {
                        $('#' + Bill_EDIReport.params["PanelID"] + ' #divEDIRepPaging').css("display", "inline");
                        //Showing 1 to 15 of 15 entries
                        var RecordsPerPage = rpp != null ? rpp : 15;
                        var CurrentPage = PageNo != null ? PageNo : 1;

                        var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                        var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                        if (PageNo == null) {
                            utility.GetCustomPaging("divEDIRepPaging", response.iTotalDisplayRecords, 5, "Bill_EDIReport", CurrentPage, RecordsPerPage);
                        }
                        var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                        var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                        $('#' + Bill_EDIReport.params["PanelID"] + ' #divEDIRepPaging #divShowingEntries').text(showingText);
                        // Change Background Color to Black for selected page
                        $('#' + Bill_EDIReport.params["PanelID"] + ' li').each(function () {
                            if ($(this).text() == CurrentPage) {
                                $(this).attr("class", "active");
                            }
                            else
                                $(this).removeAttr("class");
                        });
                    }
                    else {
                        $('#' + Bill_EDIReport.params["PanelID"] + ' #divEDIRepPaging').css("display", "none");
                    }
                    //--------------------End Pagination---------
                    Bill_EDIReport.EDIReportGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});
    },

    SearchEDIReport: function (EDIReportData, _837BatchNumber, _837ClauimNumber, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        //var data = "EDIReportData=" + EDIReportData + "&_837BatchNumber=" + _837BatchNumber + "&_837ClauimNumber=" + _837ClauimNumber + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        //return MDVisionService.defaultService(data, "BILL_EDIREPORT", "SEARCH_EDIREPORT");

        var objData = new JSON.constructor();
        if (EDIReportData)
            objData = JSON.parse(EDIReportData);
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["CommandType"] = "search";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Charges", "EDIReportSearch");
    },

    EDIReportGridLoad: function (response) {
        $('#' + Bill_EDIReport.params["PanelID"] + ' #dgvEDIReport').dataTable().fnDestroy();
        $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport tbody').find("tr").remove();
        if ($('#' + Bill_EDIReport.params["PanelID"] + " #frmBillEDIReport #ReportTitle").val() == '277') {
            if ($('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport #claimStatus ').length == 1) {
                $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport #claimStatus').remove();
                $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport #claimStatusSubCol ').remove();
            }
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(0)').attr("rowspan", "2");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(1)').attr("rowspan", "2");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(2)').attr("rowspan", "2");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(3)').attr("rowspan", "2");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(4)').attr("rowspan", "2");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(5)').attr("rowspan", "2");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(6)').attr("rowspan", "2");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(7)').attr("rowspan", "2");
            $("<th colspan='3' id='claimStatus' style='text-align:center;'>Claims</th>").insertAfter($('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq(6)'));
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead ').append("<tr id='claimStatusSubCol'><th> Accepted</th><th> Rejected</th><th>Total</th></tr>")
        }
        else {
            if ($('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport #claimStatus ').length == 1) {
                $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport #claimStatus').remove();
                $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport #claimStatusSubCol ').remove();
            }
            for (var i = 0; i < 8; i++) {
                $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport thead tr:first th:eq('+i+')').attr("rowspan", "1");
            }
        }
        if (response.EDIReportCount > 0) {
            var EDIReportLoad_JSON = response.EDIReportLoad_JSON;
            EDIReportLoad_JSON = JSON.stringify(EDIReportLoad_JSON);
            EDIReportLoad_JSON = EDIReportLoad_JSON.replace(/\\f/g, "");
            EDIReportLoad_JSON = JSON.parse(EDIReportLoad_JSON);

            $.each(JSON.parse(EDIReportLoad_JSON), function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Bill_EDIReport.EDIReportView('" + item.EDIReportId + "','" + item.ReportType + "',event);utility.SelectGridRow($(this));");
                $row.attr("id", "gvPractice_row" + item.EDIReportId);
                $row.attr("EDIReportId", item.EDIReportId);
                $row.attr("FileName", item.FileName);
                $row.attr("DownloadDate", item.CreatedOn);
                $row.attr("ReportType", item.ReportTitle);
                $row.attr("ReviewStatus", item.ReviewStatus);
                $row.attr("Clearinghouse", item.Clearinghouse);
                $row.attr("Comments", item.Comments);

                var ReviewedStatus = "";
                if (item.IsReviewed == "False")
                    ReviewedStatus = "No";
                else if (item.IsReviewed == "True")
                    ReviewedStatus = "Yes";

                var ReProcessAction = "";
                if (item.ReportType == '835')
                {
                    var reprocess = "";
                    if (item.IsERADeleted.toLowerCase() != "true")
                        reprocess = "disabled";

                    ReProcessAction = '&nbsp;<a class="btn btn-xs ' + reprocess + '" href="#" onclick="Bill_EDIReport.DownloadERA(\'' + item.EDIReportId + "','" + item.IsERADeleted + "'" + ',event);"   title="Reprocess"><i class="glyphicon glyphicon-retweet green"></i></a>';

                }
                var claimStatusCount, totalClaims;
                // $('#' + Bill_EDIReport.params["PanelID"] + " #frmBillEDIReport #ReportTitle").val() 
                if ($('#' + Bill_EDIReport.params["PanelID"] + " #frmBillEDIReport #ReportTitle").val() == '277') {

                    if (!item.TotalAccepted) {
                        item.TotalAccepted = 0;
                    }
                    if (!item.TotalRejected) {
                        item.TotalRejected = 0;
                    }
                    totalClaims = parseInt(item.TotalAccepted) + parseInt(item.TotalRejected);
                    claimStatusCount = "<td>" + item.TotalAccepted + "</td><td>" + item.TotalRejected + "</td><td>" + totalClaims + "</td>";
                }
                if ($('#' + Bill_EDIReport.params["PanelID"] + " #frmBillEDIReport #ReportTitle").val() == '277') {
                    $row.append('<td style="display:none;">' + item.EDIReportId + '</td><td><a class="btn  btn-xs" href="#" onclick="Bill_EDIReport.EDIReportDelete(\'' + item.EDIReportId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_EDIReport.EDIReportView(\'' + item.EDIReportId + "','" + item.ReportType + "'" + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>' + ReProcessAction + '</td><td>' + item.FileName + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.ReportTitle + '</td><td>' + ReviewedStatus + '</td><td>' + item.Clearinghouse + '</td>' + claimStatusCount + '<td>' + item.Comments + '</td>');
                }
                else {
                    $row.append('<td style="display:none;">' + item.EDIReportId + '</td><td><a class="btn  btn-xs" href="#" onclick="Bill_EDIReport.EDIReportDelete(\'' + item.EDIReportId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_EDIReport.EDIReportView(\'' + item.EDIReportId + "','" + item.ReportType + "'" + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>' + ReProcessAction + '</td><td>' + item.FileName + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.ReportTitle + '</td><td>' + ReviewedStatus + '</td><td>' + item.Clearinghouse + '</td><td>' + item.Comments + '</td>');
                }
                $("#pnlEDIReport_Result #dgvEDIReport tbody").last().append($row);
            });

            // Begin 12-Jan-2016  Added By Azeem Raza Tayyab Bug # PMS-3348
            var ClearinghouseCount = $('#pnlBillEDIReport #Clearinghouse option').length;
            //if there is only one Clearinghouse, then it should be selected by default
            if (ClearinghouseCount == 2) {
                $('#pnlBillEDIReport #Clearinghouse option:nth-child(2)').prop("selected", true);
            }
            // End 12-Jan-2016  Added By Azeem Raza Tayyab Bug # PMS-3348
        }
        else {
            $('#' + Bill_EDIReport.params["PanelID"] + ' #divEDIRepPaging').css("display", "none");
            $('#' + Bill_EDIReport.params["PanelID"] + ' #dgvEDIReport').DataTable({
                "language": {
                    "emptyTable": "No EDI Report Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Bill_EDIReport.params["PanelID"] + ' #dgvEDIReport'))
            ;
        else
            $('#' + Bill_EDIReport.params["PanelID"] + ' #pnlEDIReport_Result #dgvEDIReport').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": true, "aTargets": [1] }], "aaSorting": [[0, 'desc']] }); // to remove records per page dropdown
        //$('#pnlEDIReport_Result #dgvEDIReport_info').html("Total Records: " + response.EDIReportCount);
    },

    EDIReportDelete: function (EDIReportId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = EDIReportId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Bill_EDIReport.DeleteEDIReport(selectedValue).done(function (response) {
                        if (response.status != false) {
                            Bill_EDIReport.EDIReportSearch();
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
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },
    DeleteEDIReport: function (EDIReportId) {
        //var data = "EDIReportId=" + EDIReportId;
        //// serach parameter , class name, command name of class 
        //return MDVisionService.defaultService(data, "BILLING_EDI_BATCH_DETAIL", "DELETE_EDI_REPORT");

        var objData = new JSON.constructor();
        objData["EDIReportID"] = EDIReportId;
        objData["CommandType"] = "delete_edi_reports";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Charges", "EDIReport");
    },
    EDIReportView: function (EDIReportId, ReportType, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["EDIReportId"] = EDIReportId;
        params["ReportType"] = ReportType;
        params["ParentCtrl"] = 'billTabEDIReport';
        LoadActionPan('EDIReviewReport', params);
    },

    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    GetFilesList: function () {
        var self = $('#frmBillEDIReport');
        var myJSON = self.getMyJSONByName();

        if (Bill_EDIReport.ValidateClearinghouse()) {
            Bill_EDIReport.GetFilesLists(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Bill_EDIReport.EDIReportSearch();
                    //Bill_ERA.Download_ERA();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

    },

    GetFilesLists: function (json) {
       // var Jsondata = "json=" + json;
       // return MDVisionService.defaultService(data, "BILL_EDIREPORT", "GET_FTP_FILES_LIST");

        var objData = new JSON.constructor();
        if (json)
            objData = JSON.parse(json);

        objData["CommandType"] = "get_latest_reports";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Charges", "EDIReport");
    },

    DownloadERA: function (EDIReportId, IsERADeleted, event) {

        if (event != null) {
            event.stopPropagation();
        }

        if (IsERADeleted.toLowerCase() == "true") {
            
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("ERA", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    Bill_ERA.DownloadERA(EDIReportId).done(function (response) {
                        if (response.status != false) {

                            Bill_EDIReport.EDIReportSearch();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });

        }
        else {
            utility.DisplayMessages("This Report has been already processed.", 2);
        }



    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

    //----------Pagination Functions-------------

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlEDIReport_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_EDIReport.EDIReportSearch(PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlEDIReport_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_EDIReport.EDIReportSearch(currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlEDIReport_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_EDIReport.EDIReportSearch(currentPageNo, 15);
        }
    },
}