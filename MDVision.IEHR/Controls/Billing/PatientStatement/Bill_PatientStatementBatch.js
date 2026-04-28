Bill_PatientStatementBatch = {
    params: [],
    statments: [],
    SelectedStatementCount: 0,
    SelectedStatementJson: "",
    Load: function (params) {

        Bill_PatientStatementBatch.params = params;

        if (Bill_PatientStatementBatch.params.TabID == "billTabPatientStatement") {
            // Bill_PatientStatementBatch.removeDialogClasses();
            Bill_PatientStatementBatch.params.PanelID = "pnlBillPatientStatementBatch";
        }
        else {
            Bill_PatientStatementBatch.params.PanelID = Bill_PatientStatementBatch.params.PanelID + " #pnlBillPatientStatementBatch";
        }


        //if submission was made on paper, batch doesn't need to be resubmitted !
        //if (Bill_PatientStatementBatch.params["SubmitType"] == "Paper") {

        //    $('#' + Bill_PatientStatementBatch.params.PanelID + ' #btnResubmit').addClass("disabled")
        //}

        Bill_PatientStatementBatch.LoadPatientStatements(null, 3000, null);

    },
    checkUncheckAll: function (obj) {
        $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch input[id*="chkPatStmt"]').prop("checked", $(obj).prop("checked"));
    },

    CheckedStatement: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }


        if (!$(obj).prop('checked')) {
            $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').prop('checked', false);
            $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').attr('title', 'Select all');
        }
        else {

            var selected = [];
            $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch tbody').find("input[type='checkbox']").each(function () {

                if (!$(this).is(":checked")) {
                    selected.push(this);
                }
            });

            if (selected.length > 0) {
                $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').prop('checked', false);
                $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').attr('title', 'Select all');

            }
            else {
                $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').prop('checked', true);
                $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').attr('title', 'Unselect all');
            }
        }
    },

    Statement_ReSubmit: function (isSave) {
        var strData = "";
        var selectedObj = $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' input[id*="chkPatStmt"]:checked').map(function () {
            return this;
        }).get();

        if (selectedObj.length == 0) {
            utility.DisplayMessages("Please select statement(s) to print.", 4);
        }
        else {

            $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #dgvStatementBatch input[id*="chkPatStmt"]').each(function () {
                var rowStatus = $(this).prop('checked');
                if (rowStatus == true) {
                    var rowData = new JSON.constructor();
                    var rowObj = $(this).parent().parent().find("span").text();
                    var result = rowObj.split("|");
                    rowData["PatientId"] = result[0];
                    rowData["LastName"] = result[1];
                    rowData["FirstName"] = result[2];
                    rowData["FacilityId"] = result[3];
                    rowData["PatBalance"] = result[4];
                    rowData['SubmittedChargeIds'] = result[5];
                    rowData['VisitIDs'] = result[6];
                    rowData['GuarantorId'] = result[7];
                    var strAge = result[8];
                    if(Number(strAge) >= 0 && Number(strAge) <= 30 )
                    {
                        rowData['Age'] = 1;
                    }
                    else if(Number(strAge) > 30 && Number(strAge) <= 60 )
                    {
                        rowData['Age'] = 2;
                    }
                    else if(Number(strAge) > 60 && Number(strAge) <= 90 )
                    {
                        rowData['Age'] = 3;
                    }
                    else if(Number(strAge) > 90 && Number(strAge) <= 120 )
                    {
                        rowData['Age'] = 4;
                    }
                    else
                    {
                        rowData['Age'] = 5;
                    }
                   // rowData['Age'] = result[8];

                    rowData["BatchId"] = Bill_PatientStatementBatch.params["BatchId"];
                    rowData['DOSTo'] = "";
                    rowData['DOSFrom'] = "";
                    rowData["BatchResubmit"] = $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').prop('checked');
                    strData += JSON.stringify(rowData) + ",";
                }
            });

            strData = "[" + strData.slice(0, -1) + "]";

            if (Bill_PatientStatementBatch.params["SubmitType"] == "Paper") {
                Bill_PatientStatementBatch.ViewAndPrintPatientStatement(strData);
            }
            else {
                Bill_PatientStatement.saveStatements(true, strData);
            }

        }

    },
    ViewAndPrintPatientStatement: function (infoArr) {
        var headerMarkUp = $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' printStatement .templatePrintStatement .tableHeader')[0].outerHTML;
        var footerMarkUp = $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' printStatement .templatePrintStatement .tableFooter')[0].outerHTML;
        var bodyMarkUp = $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' printStatement .templatePrintStatement .tableBody')[0].outerHTML;
        Bill_PatientStatementBatch.PrintPatientStatements(infoArr).done(function (response) {
            if (response.status) {
                $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer').empty();
                //var allStatements = JSON.parse(response);
                if (response.Statement_JSON_Count > 0) {
                    var allStatements = JSON.parse(response.Statement_JSON);
                    Bill_PatientStatementBatch.statments = [];
                    $.each(allStatements, function (i, statement) {

                        Bill_PatientStatementBatch.statments.push(statement);
                        //var header = JSON.parse(statement.StatementHeaders)[0];
                        var ledgerAmount = 0;
                        var resHeader = statement.StatementHeader;
                        //start syed zia 18-02-2016, resolve parsor exception in case of table html
                        var header = "";
                        header = JSON.parse(statement.StatementHeader)[0];

                        var footer = JSON.parse(statement.StatementFooter)[0];
                        var body = JSON.parse(statement.StatementDetail);
                        if (i == 0)
                            $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer').append("<button class='btn btn-success bt-sm m-xs pull-right' id='btnPrint' onclick='RemoveBottomSpace();'><i class='glyphicon glyphicon-print'></i> Print</button>");


                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer').append('<div id="' + header.AccountNumber + i + '" class="statements pageBreak statmentPrint" PatientId="' + header.PatientId + '" style="margin-bottom:40px"></div>');



                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i).append(headerMarkUp);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i).append(bodyMarkUp);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i).append(footerMarkUp);

                        // Header

                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .FromName').text(header.FromName);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .FromAddress').text(header.FromAddress);
                        var FromZipExt = "";
                        if (header.FromZipExt) {
                            FromZipExt = " - " + header.FromZipExt;
                        }
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .FromCity').text(header.FromCity + " " + header.FromState + ", " + header.FromZip + FromZipExt);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .Ofctime').text(header.OfcHoursFrom + " - " + header.OfcHoursTo);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .OfcPhone').text(header.PhoneNo);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .RemitToName').text(header.RemitToName);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .RemitToAddress').text(header.RemitToAddress);
                        var RemitToZipExtension = "";
                        if (header.RemitToZipExt) {
                            RemitToZipExtension = " - " + header.RemitToZipExt;
                        }
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .RemitToCity').text(header.RemitToCity + " " + header.RemitToState + ", " + header.RemitToZip + RemitToZipExtension);
                        //footer
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .StatementGrpMessage').text(header.Message);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .Telephone').text(header.PhoneNo);
                        ////////////////

                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientName').text(header.FirstName + " " + header.LastName);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientAddress').text(header.Address1);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientCity').text(header.City + " " + header.State + "," + header.ZipCode);

                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientAccount').text(header.AccountNumber);
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientCurrentDate').text($.datepicker.formatDate(date_format.replace('yy', ''), new Date()));

                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .tableHeader').after($('#' + Bill_PatientStatementBatch.params.PanelID + ' printStatement .cuttingSection').prop('outerHTML'));
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .cuttingSection').after($('#' + Bill_PatientStatementBatch.params.PanelID + ' printStatement .cutting').prop('outerHTML'));


                        //Body
                        $.each(body, function (index, obj) {
                            var temp = '<tr></tr>';


                            if (obj.Procedure != null && obj.Procedure != "") {
                                ledgerAmount = Number(ledgerAmount) + Number(obj.Charges);
                                ledgerAmount = Number(ledgerAmount).toFixed(Number(globalAppdata.DecimalPlaces));

                            }
                            else {
                                if (obj.LedgerType != "Copay to Ins" && obj.LedgerType != "Pat to Ins" && obj.LedgerType != "Ins to Pat" && obj.LedgerType != "Ins to Ins") {
                                    ledgerAmount = Number(ledgerAmount) - Number(obj.Paid);
                                    ledgerAmount = Number(ledgerAmount).toFixed(Number(globalAppdata.DecimalPlaces));
                                }


                            }

                            var Description = obj.Description.split("#")[0];
                            if (typeof obj.Description.split("#")[1] != "undefined") {
                                Description += '<br/>' + obj.Description.split("#")[1];
                            }
                            if (obj.Procedure == "99201" || obj.Procedure == "99202" || obj.Procedure == "99203" || obj.Procedure == "99204" || obj.Procedure == "99205") {
                                Description = "Office Outpatient New";
                            }
                            else if (obj.Procedure == "99211" || obj.Procedure == "99212" || obj.Procedure == "99213" || obj.Procedure == "99214" || obj.Procedure == "99215") {
                                Description = "Office Outpatient Visit";
                            }

                            var fullName = obj.FullName;
                            if (fullName.indexOf("-") >= 0) {
                                fullName = obj.FullName.replace('-', ',')
                            }

                            var Row = "";
                            var paid = "";
                            if (Description.indexOf("Patient Responsibility") >= 0) {
                                Description = "<span style='color:red'>" + Description + "</span>";
                                paid = "<span style='color:red'>" + globalAppdata.DefaultCurrency + Number(obj.Paid).toFixed(Number(globalAppdata.DecimalPlaces)) + "</span>";
                            }
                            else {
                                paid = globalAppdata.DefaultCurrency + Number(obj.Paid).toFixed(Number(globalAppdata.DecimalPlaces));
                            }

                            if (obj.Procedure != null && obj.Procedure != "") {
                                //remove procedure and paid column
                                Row = $(temp).append('<td>' + utility.RemoveTimeFromDate(null, obj.Date) + '</td><td>' + fullName + '</td><td>' + Description + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Paid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>');
                            }
                            else {
                                //Row = $(temp).append('<td></td><td></td><td>' + obj.Procedure + '</td><td>' + Description + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + paid + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + ledgerAmount + '</td>');
                                Row = $(temp).append('<td></td><td></td><td>' + Description + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + paid + '</td>');

                            }

                            $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + " .tableBody .printBody").last().append(Row);

                        });

                        //Footer

                        //$('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .aging').text('');
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .0_30Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age0-30']).toFixed(Number(globalAppdata.DecimalPlaces)));
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .31_60Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age31-60']).toFixed(Number(globalAppdata.DecimalPlaces)));
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .61_90Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age61-90']).toFixed(Number(globalAppdata.DecimalPlaces)));
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .91_120Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age91-120']).toFixed(Number(globalAppdata.DecimalPlaces)));
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .120_above').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age121-Onward']).toFixed(Number(globalAppdata.DecimalPlaces)));
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .PatientBalanceDue').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)));
                        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .statementMessage').text(footer.StatementMessage);


                    });
                    var docType = '<!doctype html>';
                    $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer').show();
                    var docCnt = $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer')[0].outerHTML;
                    $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer').hide();
                     var SelectedStatementObj = JSON.stringify(infoArr)
                    var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                      + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                      + '<script>var isPrinted = false;function ResizePrintStatmentGrid (){var ua = window.navigator.userAgent;var msie = ua.indexOf("MSIE ");if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {$(".tableBody").addClass("resizeGrid");}} function RemoveBottomSpace(){ ResizePrintStatmentGrid ();$(".statmentPrint").removeAttr("style");isPrinted = true;utility.myConfirm("Do you want to submit statement ?", function () {window.opener.Bill_PatientStatement.saveStatements("false",' + SelectedStatementObj + ',false);}, function () {},"<b>Confirm submission</b>");window.print(); } '
                      + '</script>'
                      + '</head>';

                    var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=865, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                    var newWin = window.open("", "_blank", winAttr);
                    writeDoc = newWin.document;
                    writeDoc.open();
                    writeDoc.write(docType + '<html>' + docHead + '<body>' + docCnt + '</body></html>');
                    writeDoc.close();
                    newWin.focus();

                    
                }
                else {
                    utility.DisplayMessages(response.Message, 4);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 4)
            }
        });
    },
    saveStatements: function (isFromSubmitted) {
        var isStatementExist = "false";
        var displayMsg = "";
        var Status = true;
        var count = 0;
        var selectedCount = 0;
        $.each($('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer .statements'), function (i, item) {
            var header = [];
            var headerJson = Bill_PatientStatementBatch.statments[i].StatementHeader;
            var header = "";

            header = JSON.parse(Bill_PatientStatementBatch.statments[i].StatementHeader)[0];

            delete header.LetterHtmlDocument;
            Bill_PatientStatementBatch.statments[i].StatementHeader = JSON.stringify(header);
            // Bill_PatientStatement.PatientStatementSave(JSON.stringify(Bill_PatientStatement.statments[i]), $(item).prop('outerHTML'), isFromSubmitted);
            selectedCount++
            var statementCount = $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer .statements').length;
            //start syed zia , bug #PMS-4575

            Bill_PatientStatementBatch.CreateSelectStatementJson(JSON.stringify(Bill_PatientStatementBatch.statments[i]), $(item).prop('outerHTML'), $('#' + Bill_PatientStatementBatch.params.PanelID + ' #accountStatementContainer .statements').length);

            if (selectedCount == statementCount) {
                //Bill_PatientStatement.SavePatientStatement(JSON.stringify(Bill_PatientStatement.statments[i]), $(item).prop('outerHTML'), isFromSubmitted, statementCount);
                Bill_PatientStatementBatch.SavePatientStatement(Bill_PatientStatementBatch.SelectedStatementJson, $(item).prop('outerHTML'), isFromSubmitted).done(function (response) {
                    if (response.Message != "Already Submitted") {

                        if (response.status) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 4)
                        }
                    }
                    else {
                        utility.DisplayMessages("Statement already submitted.", 4);
                    }

                    //Reset variables
                    Bill_PatientStatementBatch.SelectedStatementCount = 0;
                    Bill_PatientStatementBatch.SelectedStatementJson = "";
                });
            }
        });
    },

    PrintPatientStatements: function (jsnData) {

        var objData = new JSON.constructor();
        objData["CommandType"] = "print_patient_statements";
        objData["ItemList"] = jsnData;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },

    LoadPatientStatements: function (PageNo, rpp, RowId, PatientStatementID) {

        if ($('#' + Bill_PatientStatementBatch.params.PanelID + " #pnlStatementBatch_Result").css("display") == "none") {
            $('#' + Bill_PatientStatementBatch.params.PanelID + " #pnlStatementBatch_Result").show();
        }

        Bill_PatientStatementBatch.SearchPatientStatementsBatch(Bill_PatientStatementBatch.params.BatchId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                if (response.PatientStatementCount > 0) {
                    $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #divPostingStatementPaging").css("display", "inline");
                    //Showing 1 to 15 of 15 entries
                    var RecordsPerPage = rpp != null ? rpp : 3000;
                    var CurrentPage = PageNo != null ? PageNo : 1;

                    var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    if (PageNo == null) {
                        utility.GetCustomPaging("divPostingStatementPaging", response.iTotalDisplayRecords, 3000, "Bill_PatientStatementBatch", CurrentPage, RecordsPerPage);
                    }
                    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #divPostingStatementPaging #divShowingEntries").text(showingText);
                    // Change Background Color to Black for selected page
                    $("#" + Bill_PatientStatementBatch.params["PanelID"] + " li").each(function () {
                        if ($(this).text() == CurrentPage) {
                            $(this).attr("class", "active");
                        }
                        else

                            $(this).removeAttr("class");
                    });
                    //------------End Pagination-------
                }
                else {
                    $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #divPostingStatementPaging").css("display", "none");
                }
                Bill_PatientStatementBatch.PatientStatementGridLoad(response, RowId);
                if (Bill_PatientStatementBatch.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatementBatch.params.ParentCtrl == "Bill_PaymentPosting") {
                    $('#' + Bill_PatientStatementBatch.params.PanelID + ' #pnlStatementBatch_Result .tabs-custom li a[href=#SubmittedStatements]').click();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #statementli").addClass("active");
        });

    },
    SearchPatientStatementsBatch: function (BatchId, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 3000;
        }
        Bill_PatientStatementBatch.params.CurrentPageNo = PageNumber;

        var objData = new JSON.constructor();

        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["BatchId"] = BatchId;
        objData["CommandType"] = "search_batch_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },
    PatientStatementGridLoad: function (response, RowId) {
        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch"))
            $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch").dataTable().fnDestroy();
        $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch tbody").find("tr").remove();
        if (response.PatientStatementCount > 0) {
            var ChargeLoadJSONData = JSON.parse(response.PatientStatementLoad_JSON);
            $.each(ChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvStatementBatch_row" + i + "'));");
                $row.attr("id", "gvStatement_row" + i);
                $row.attr("StatementId", item.PatStatementId);
                $row.append('<span style="display:none">' + item.PatientId + "|" + item.LastName + "|" + item.FirstName + "|" + item.FacilityId + "|" + Number(item.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + "|" + item.SubmittedChargeIds + "|" + item.VisitIDs + "|" + item.GuarantorId + "|" + item.Age + '</span>')
                $row.append('<td style="display:none;">' + item.PatStatementId + '</td><td><input type="checkbox" facilityId=' + item.FacilityId + ' patientId=' + item.PatientId + ' id="chkPatStmt' + item.PatStatementId + '"onclick="Bill_PatientStatementBatch.CheckedStatement(this,event)"  " /></td><td>' + item.AccountNumber + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.FacilityName + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.AdvancedPayment).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + utility.RemoveTimeFromDate(null, item.LastStatementDate) + '</td>');
                $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch tbody").last().append($row);
            });

        }
        else {
            if (!$("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #divStatementPaging").css("display", "none");
                $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch").DataTable({
                    "language": {
                        "emptyTable": "No Patient Statement Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Patient Statement Found</td>');
                $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #SubmittedStatements #dgvSubmittedStatement tbody").last().append($row);
            }
        }
        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch") || $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Bill_PatientStatementBatch.params["PanelID"] + " #pnlStatementBatch_Result #dgvStatementBatch").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    SavePatientStatement: function (jsnData, markUp, isFromSubmitted) {
        //  var clearinghouseId = $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #ddlClearinghouse option:selected').val();
        var objData = new JSON.constructor();
        objData["CommandType"] = "save_patient_statement";
        objData["ItemList"] = utility.replaceSpecialCharacters(jsnData);
        objData["MarkUp"] = markUp;
        objData["isFromSubmitted"] = isFromSubmitted;
        objData["ClearingHouseId"] = Bill_PatientStatementBatch.params.ClearingHouseId;
        objData["BatchId"] = Bill_PatientStatementBatch.params.BatchId;
        objData["IsResubmit"] = "true";
        objData["BatchResubmit"] = $('#' + Bill_PatientStatementBatch.params["PanelID"] + ' #pnlStatementBatch_Result #dgvStatementBatch  #chkAllStatements').prop('checked');
        var data = objData;

        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");

    },
    CreateSelectStatementJson: function (data, markUp, statementcount) {
        Bill_PatientStatementBatch.SelectedStatementCount++;
        if (Bill_PatientStatementBatch.SelectedStatementCount == statementcount) {
            Bill_PatientStatementBatch.SelectedStatementJson += data;
        }
        else {
            Bill_PatientStatementBatch.SelectedStatementJson += data + ",";
        }
    },

    removeDialogClasses: function () {
        $('#' + Bill_PatientStatementBatch.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Bill_PatientStatementBatch.params.PanelID + ' .modal-dialog').removeAttr('class');
        $('#' + Bill_PatientStatementBatch.params.PanelID + ' #containerModalDialog').removeClass('modal-dialog');

    },
    UnLoadTab: function (Tab) {
        $('#pnlBillPatientStatementBatch #btnClose').addClass('disableAll');
        if (Bill_PatientStatementBatch.params != null && Bill_PatientStatementBatch.params.ParentCtrl != null) {
            if (Bill_PatientStatementBatch.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatementBatch.params.ParentCtrl == "Bill_PaymentPosting") {
                UnloadActionPan(Bill_PatientStatementBatch.params.ParentCtrl, "Bill_PatientStatementBatch", null, "pnlBillPaymentPosting");
            }
            else {
                UnloadActionPan(Bill_PatientStatementBatch.params.ParentCtrl, "Bill_PatientStatementBatch");
            }

        }
        else
            RemoveAdminTab(Tab);

        Bill_PatientStatementBatch.params = null;
    },
}