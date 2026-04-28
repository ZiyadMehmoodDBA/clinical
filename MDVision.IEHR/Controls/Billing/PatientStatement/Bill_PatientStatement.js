Bill_PatientStatement = {
    bIsFirstLoad: true,
    params: [],
    statments: [],
    isDeletePrivilige: false,
    SelectedStatementCount: 0,
    SelectedStatementJson: "",

    Load: function (params) {

        Bill_PatientStatement.params = params;

        if (Bill_PatientStatement.params.TabID == "billTabPatientStatement") {
            Bill_PatientStatement.removeDialogClasses();
            Bill_PatientStatement.params.PanelID = "pnlBillPatientStatement";
        }
        else {
            Bill_PatientStatement.params.PanelID = Bill_PatientStatement.params.PanelID + " #pnlBillPatientStatement";
        }
        if (Bill_PatientStatement.bIsFirstLoad) {

            Bill_PatientStatement.bIsFirstLoad = false;

            Bill_PatientStatement.LoadAllAutoComplete();
            Bill_PatientStatement.ValidateAccountNumber();
            utility.ValidateFromToDate('frmBillPatientStatement', 'dtpDOSFrom', 'dtpDOSTo', true);
            utility.ValidateFromToDate('frmBillPatientStatement', 'dtpLastStatmentDateFrom', 'dtpLastStatmentDateTo', true);
            //submit date shouldn't be later than today !
            utility.ValidateDOB(Bill_PatientStatement.params.PanelID + ' #frmBillPatientStatement', Bill_PatientStatement.params.PanelID + ' #frmBillPatientStatement #dtpSubmittedDate', new Date('1800-01-01'), new Date(), false);


            AppPrivileges.GetFormPrivileges("Patient Statement", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "")
                    Bill_PatientStatement.isDeletePrivilige = true;
                else
                    Bill_PatientStatement.isDeletePrivilige = false;
            });

            var self = $("#" + Bill_PatientStatement.params.PanelID);
            var self_statement = $("#" + Bill_PatientStatement.params.PanelID + " #Statements");
            var self_BatchStatement = $("#" + Bill_PatientStatement.params.PanelID + " #SubmittedStatementsBatch");
            var self_submittedStatement = $("#" + Bill_PatientStatement.params.PanelID + " #SubmittedStatements");

            self_statement.loadDropDowns(true).done(function () {
                if (Bill_PatientStatement.params["PatientID"]) {
                    $("#" + Bill_PatientStatement.params.PanelID + " #hfPatientId").val(Bill_PatientStatement.params["PatientID"]);
                }

                Bill_PatientStatement.LoadPatientStatements(null, 300, null);

                $('#' + Bill_PatientStatement.params["PanelID"] + ' #ddlClearinghouse option:eq(1)').attr("selected", "selected");
            })

            self_BatchStatement.loadDropDowns(true).done(function () {
                $('#' + Bill_PatientStatement.params["PanelID"] + ' #SubmittedStatementsBatch #ddlClearinghouse option:eq(1)').attr("selected", "selected");

            });
            self_submittedStatement.loadDropDowns(true).done(function () {
                $('#' + Bill_PatientStatement.params["PanelID"] + ' #SubmittedStatements #ddlClearinghouse option:eq(1)').attr("selected", "selected");

            });
        }



    },
    ValidateAccountNumber: function () {
        $('#frmBillPatientStatement')
             .bootstrapValidator({
                 live: 'disabled',
                 message: 'This value is not valid',
                 feedbackIcons: {
                     valid: 'glyphicon glyphicon-ok',
                     invalid: 'glyphicon glyphicon-remove',
                     validating: 'glyphicon glyphicon-refresh'
                 },
                 fields: {
                     AccountNumber: {
                         group: '.col-sm-3',
                         enabled: false,
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     }
                 }
             }).on('success.form.bv', function (e) {
                 e.preventDefault();
                 Bill_PatientStatement.LoadPatientStatements(null, null, null, null);
             });
    },
    checkUncheckAll: function (obj) {
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #pnlStatement_Result #dgvStatement input[id*="chkPatStmt"]').prop("checked", $(obj).prop("checked"));
    },
    PrintStatement: function (isSave) {

        if (isSave) {
            if (!$('#' + Bill_PatientStatement.params["PanelID"] + ' #ddlClearinghouse option:selected').val()) {
                utility.DisplayMessages("Please select clearing house.", 4);
                return false;
            }
        }

        var selectedObj = $('#' + Bill_PatientStatement.params["PanelID"] + ' input[id*="chkPatStmt"]:checked').map(function () {
            return this;
        }).get();

        if (selectedObj.length == 0) {
            utility.DisplayMessages("Please select statement(s) to print.", 4);
        }
        else {
            if (isSave) {
                Bill_PatientStatement.saveStatements("true", null, false);
            }
            else {
                var infoArr = new Array();
                $.each(selectedObj, function (i, item) {
                    var dummy = new Object();
                    dummy['DOSTo'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSTo').val();
                    dummy['DOSFrom'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSFrom').val();
                    dummy['PatientId'] = $(this).attr('patientid');
                    dummy['FacilityId'] = $(this).attr('facilityid');
                    dummy['Age'] = $('#' + Bill_PatientStatement.params.PanelID + ' #ddlAge').val();
                    infoArr.push(dummy);
                });
                Bill_PatientStatement.ViewAndPrintPatientStatement(JSON.stringify(infoArr), true);
            }
        }

    },

    ViewAndPrintPatientStatement: function (infoArr, isStatementTab) {
        AppPrivileges.GetFormPrivileges("Patient Statement", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var headerMarkUp = $('#' + Bill_PatientStatement.params["PanelID"] + ' printStatement .templatePrintStatement .tableHeader')[0].outerHTML;
                var footerMarkUp = $('#' + Bill_PatientStatement.params["PanelID"] + ' printStatement .templatePrintStatement .tableFooter')[0].outerHTML;
                var bodyMarkUp = $('#' + Bill_PatientStatement.params["PanelID"] + ' printStatement .templatePrintStatement .tableBody')[0].outerHTML;
                Bill_PatientStatement.PrintPatientStatements(infoArr).done(function (response) {
                    if (response.status) {
                        $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').empty();
                        //var allStatements = JSON.parse(response);
                        if (response.Statement_JSON_Count > 0) {
                            var allStatements = JSON.parse(response.Statement_JSON);
                            Bill_PatientStatement.statments = [];
                            $.each(allStatements, function (i, statement) {

                                Bill_PatientStatement.statments.push(statement);
                                //var header = JSON.parse(statement.StatementHeaders)[0];
                                var ledgerAmount = 0;
                                var resHeader = statement.StatementHeader;
                                //start syed zia 18-02-2016, resolve parsor exception in case of table html
                                var header = "";
                                header = JSON.parse(statement.StatementHeader)[0];

                                //end syed zia 18-02-2016, resolve parsor exception in case of table html

                                var footer = JSON.parse(statement.StatementFooter)[0];
                                var body = JSON.parse(statement.StatementDetail);
                                //  var LetterHTML = header.LetterHtmlDocument;
                                if (i == 0)
                                    $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').append("<button class='btn btn-success bt-sm m-xs pull-right' id='btnPrint' onclick='RemoveBottomSpace();'><i class='glyphicon glyphicon-print'></i> Print</button>");


                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').append('<div id="' + header.AccountNumber + i + '" class="statements pageBreak statmentPrint" PatientId="' + header.PatientId + '" style="margin-bottom:40px"></div>');



                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i).append(headerMarkUp);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i).append(bodyMarkUp);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i).append(footerMarkUp);

                                // Header

                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .FromName').text(header.FromName);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .FromAddress').text(header.FromAddress);
                                var FromZipExt = "";
                                if (header.FromZipExt) {
                                    FromZipExt = " - " + header.FromZipExt;
                                }
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .FromCity').text(header.FromCity + " " + header.FromState + ", " + header.FromZip + FromZipExt);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .Ofctime').text(header.OfcHoursFrom + " - " + header.OfcHoursTo);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .OfcPhone').text(header.PhoneNo);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .RemitToName').text(header.RemitToName);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .RemitToAddress').text(header.RemitToAddress);
                                var RemitToZipExtension = "";
                                if (header.RemitToZipExt) {
                                    RemitToZipExtension = " - " + header.RemitToZipExt;
                                }
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .RemitToCity').text(header.RemitToCity + " " + header.RemitToState + ", " + header.RemitToZip + RemitToZipExtension);
                                //footer
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .StatementGrpMessage').text(header.Message);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .Telephone').text(header.PhoneNo);
                                ////////////////
                                
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .actualpatientName').text(header.PatFullName.replace('#', ','));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientName').text(header.LastName + ", " + header.FirstName);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientAddress').text(header.Address1 + ", " + header.Address2);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientCity').text(header.City + " " + header.State + "," + header.ZipCode);

                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientAccount').text(header.AccountNumber);
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .patientCurrentDate').text($.datepicker.formatDate(date_format.replace('yy', ''), new Date()));

                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .tableHeader').after($('#' + Bill_PatientStatement.params.PanelID + ' printStatement .cuttingSection').prop('outerHTML'));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .cuttingSection').after($('#' + Bill_PatientStatement.params.PanelID + ' printStatement .cutting').prop('outerHTML'));


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
                                        //  Row = $(temp).append('<td>' + utility.RemoveTimeFromDate(null, obj.Date) + '</td><td>' + fullName + '</td><td>' + obj.Procedure + '</td><td>' + Description + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Paid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + ledgerAmount + '</td>');
                                        //remove procedure and paid column
                                        Row = $(temp).append('<td>' + utility.RemoveTimeFromDate(null, obj.Date) + '</td><td>' + fullName + '</td><td>' + Description + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Paid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>');
                                    }
                                    else {
                                        //Row = $(temp).append('<td></td><td></td><td>' + obj.Procedure + '</td><td>' + Description + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + paid + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + ledgerAmount + '</td>');
                                        Row = $(temp).append('<td></td><td></td><td>' + Description + '</td><td style="text-align: right;">' + globalAppdata.DefaultCurrency + Number(obj.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="text-align: right;">' + paid + '</td>');

                                    }

                                    $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + " .tableBody .printBody").last().append(Row);

                                });

                                //Footer

                                //$('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .aging').text('');
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .0_30Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age0-30']).toFixed(Number(globalAppdata.DecimalPlaces)));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .31_60Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age31-60']).toFixed(Number(globalAppdata.DecimalPlaces)));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .61_90Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age61-90']).toFixed(Number(globalAppdata.DecimalPlaces)));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .91_120Days').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age91-120']).toFixed(Number(globalAppdata.DecimalPlaces)));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .120_above').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer['Age121-Onward']).toFixed(Number(globalAppdata.DecimalPlaces)));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .PatientBalanceDue').css('text-align', 'right').text(globalAppdata.DefaultCurrency + Number(footer.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)));
                                $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer ' + '#' + header.AccountNumber + i + ' .statementMessage').text(footer.StatementMessage);


                            });
                            //if (isSave) {
                            //    Bill_PatientStatement.saveStatements("true");
                            //}
                            //if (!isSave) {
                            //$('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').printMe({ 'path': 'Content/Default/bootstrap.css,Content/Blue/theme.css,Content/Blue/theme-custom.css,Content/Blue/default.css,Content/Default/print-media.css' })
                            var docType = '<!doctype html>';
                            $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').show();
                            var docCnt = $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer')[0].outerHTML;
                            $('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').hide();


                            //if from statement tab then no need to send object in view method.but in other case we send the selected statement data in ViewMethod
                            if (isStatementTab) {
                                var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                                   + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script><script src="Scripts/MDVisionDefaultCommon.js"></script>'
                                   + '<script>var isPrinted = false;function ResizePrintStatmentGrid (){var ua = window.navigator.userAgent;var msie = ua.indexOf("MSIE ");if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {$(".tableBody").addClass("resizeGrid");}} function RemoveBottomSpace(){ ResizePrintStatmentGrid ();$(".statmentPrint").removeAttr("style");isPrinted = true;utility.myConfirm("Do you want to submit statement ?", function () {window.opener.Bill_PatientStatement.saveStatements("false",null,false);}, function () {},"<b>Confirm submission</b>");window.print(); } '
                                   + '</script>'
                                   + '</head>';
                            }
                            else {

                                var SelectedStatementObj = JSON.stringify(infoArr)
                                var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                                  + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script><script src="Scripts/MDVisionDefaultCommon.js"></script>'
                                  + '<script>var isPrinted = false;function ResizePrintStatmentGrid (){var ua = window.navigator.userAgent;var msie = ua.indexOf("MSIE ");if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {$(".tableBody").addClass("resizeGrid");}} function RemoveBottomSpace(){ ResizePrintStatmentGrid ();$(".statmentPrint").removeAttr("style");isPrinted = true;utility.myConfirm("Do you want to submit statement ?", function () {window.opener.Bill_PatientStatement.saveStatements("false",' + SelectedStatementObj + ',false);}, function () {},"<b>Confirm submission</b>");window.print(); } '
                                  + '</script>'
                                  + '</head>';
                            }
                            var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=865, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                            var newWin = window.open("", "_blank", winAttr);
                            writeDoc = newWin.document;
                            writeDoc.open();
                            writeDoc.write(docType + '<html>' + docHead + '<body>' + docCnt + '</body></html>');
                            writeDoc.close();
                            newWin.focus();

                            //  }
                            // uncomment to autoclose the preview window when printing is confirmed or canceled.
                            //
                            //setTimeout(function () { newWin.close(); }, 100);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 4);
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 4)
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },


    MarkAsPrinted: function (obj) {
        var PatStmtIds = $('#' + Bill_PatientStatement.params["PanelID"] + ' input[id*="chkPatStmt"]:checked').map(function () {
            return this.id.replace("chkPatStmt", "");
        }).get().join(',');
        if (PatStmtIds == "") {
            utility.DisplayMessages("Please select statement(s) to mark as printed.", 4);
            return false;
        }
        else {

        }
    },
    IgnoreCycleDays: function (obj) {
        var formValidation = $("#" + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement').data("bootstrapValidator");
        if ($(obj).prop("checked") == true) {
            formValidation.enableFieldValidators('AccountNumber', true);
            $("#lblAccountNumber").css("display", "inline");
        }
        else {
            formValidation.enableFieldValidators('AccountNumber', false);
            $("#lblAccountNumber").css("display", "none");
        }
    },
    BindPatientAccount: function () {
        var Ctrl = $("#" + Bill_PatientStatement.params.PanelID + " #txtAccountNumber");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Bill_PatientStatement.params.PanelID + " #pnlStatementsSearch #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },

    BindSubmittedPatientAccount: function () {
        var Ctrl = $("#" + Bill_PatientStatement.params.PanelID + " #txtSubmitAccountNumber");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Bill_PatientStatement.params.PanelID + " #pnlSubmittedStatementsSearch #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },
    //select: function (event, ui) {

    //    //$("#appointmentDetail #txtAccountNo").val(ui.item.id); // add the selected id
    //    //$("#appointmentDetail #txtFullName").val(ui.item.patientName);
    //    setTimeout(function () {
    //        $("#" + Bill_PatientStatement.params.PanelID + " #hfPatientId").val(ui.item.id);
    //    }, 100);

    //    //$("#hfpatientid").val(ui.item.id);
    //}
    //});
    //},


    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
            params["ParentCtrl"] = "Bill_PatientStatement";
        }
        else {
            params["ParentCtrl"] = "billTabPatientStatement";
        }
        LoadActionPan('Patient_Search', params, Bill_PatientStatement.params.PanelID);
    },
    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var stype = $('#' + Bill_PatientStatement.params.PanelID + ' .tabs-custom').find('li.active a').attr('href');
        var Ctrl, hfCtrl;
        if (stype == ".tabStatements" || typeof stype == "undefined") {
            hfCtrl=$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatementsSearch #hfPatientId");
            Ctrl=$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatementsSearch #txtAccountNumber");
        }
        else if (stype == ".tabSubmittedStatements") {
            hfCtrl=$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlSubmittedStatementsSearch #hfPatientId");
            Ctrl=$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlSubmittedStatementsSearch #txtSubmitAccountNumber");
        }

        if (Ctrl.data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate(Ctrl, patFullName.split(" - ")[0], hfCtrl, PatientId);

        if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
            UnloadActionPan("Bill_PatientStatement", "Patient_Search", null, Bill_PatientStatement.params.PanelID);
        }
        else {
            UnloadActionPan("billTabPatientStatement", "Patient_Search", null, Bill_PatientStatement.params.PanelID);
        }
        utility.InsertRecentPatient(PatientId);
    },
    LoadAllAutoComplete: function () {

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPatientStatement #txtFacility");
            var hfCtrl = $("#" + Bill_PatientStatement.params.PanelID + " #hfFacility");
            var onSelect = function (e) {
                $("#" + Bill_PatientStatement.params.PanelID + " #txtPractice").val(e.Practice);
                $("#" + Bill_PatientStatement.params.PanelID + " #hfPractice").val(e.PracticeId);
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPatientStatement #txtSubmitFacility");
            var hfCtrl = $("#" + Bill_PatientStatement.params.PanelID + " #hfSubmitFacility");
            var onSelect = function (e) {
                $("#" + Bill_PatientStatement.params.PanelID + " #txtPractice").val(e.Practice);
                $("#" + Bill_PatientStatement.params.PanelID + " #hfPractice").val(e.PracticeId);
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });


        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPatientStatement #txtProvider");
            var hfCtrl = $("#" + Bill_PatientStatement.params.PanelID + " #hfProvider");
            var onSelect = function (e) { $("#" + Bill_PatientStatement.params.PanelID + " #txtProvider").attr("ProviderId", e.id); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            var Ctrl = $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPatientStatement #txtPractice");
            var hfCtrl = $("#" + Bill_PatientStatement.params.PanelID + " #hfPractice");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Practices, null, hfCtrl);
        });
        Bill_PatientStatement.BindPatientAccount();
        Bill_PatientStatement.BindSubmittedPatientAccount();
    },
    //-----------Pagination Functions--------------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        var stype = $('#' + Bill_PatientStatement.params.PanelID + ' .tabs-custom').find('li.active a').attr('href');
        if (stype == ".tabStatements" || typeof stype == "undefined") {
            $("#Statements li").each(function () {
                if ($(this).text() == PageNo) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#SubmittedStatements li").each(function () {
                if ($(this).text() == PageNo) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        if (stype == ".tabSubmittedStatementsBatch") {
            Bill_PatientStatement.LoadSubmittedStatementsBatch(PageNo, 100, null, null);
        }
        else { Bill_PatientStatement.LoadPatientStatements(PageNo, 300, null); }
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        var stype = $('#' + Bill_PatientStatement.params.PanelID + ' .tabs-custom').find('li.active a').attr('href');
        if (stype == ".tabStatements" || typeof stype == "undefined") {
            $("#Statements li").each(function () {
                if ($(this).attr("class") == "active") {
                    $(this).removeAttr("class");
                    currentPageNo = parseInt($(this).text());
                }
            });
        }
        else {
            $("#SubmittedStatements li").each(function () {
                if ($(this).attr("class") == "active") {
                    $(this).removeAttr("class");
                    currentPageNo = parseInt($(this).text());
                }
            });
        }
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_PatientStatement.LoadPatientStatements(PageNo, 300, null);
        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        var stype = $('#' + Bill_PatientStatement.params.PanelID + ' .tabs-custom').find('li.active a').attr('href');
        if (stype == ".tabStatements" || typeof stype == "undefined") {
            $("#Statements li").each(function () {
                if ($(this).attr("class") == "active") {
                    $(this).removeAttr("class");
                    currentPageNo = parseInt($(this).text());
                }
            });
        }
        else {
            $("#SubmittedStatements li").each(function () {
                if ($(this).attr("class") == "active") {
                    $(this).removeAttr("class");
                    currentPageNo = parseInt($(this).text());
                }
            });
        }
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_PatientStatement.LoadPatientStatements(currentPageNo, 300, null);
        }
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmBillPatientStatement";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabPatientStatement";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Bill_PatientStatement.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'billTabPatientStatement';
        LoadActionPan('providerDetail', params);
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmBillPatientStatement";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-PMS-3336
        params["ParentCtrl"] = "Bill_PatientStatement";
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-PMS-PMS-3336
        LoadActionPan('Admin_Facility', params, Bill_PatientStatement.params.PanelID);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_PatientStatement.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-PMS-3336
        params["ParentCtrl"] = 'Bill_PatientStatement';
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-PMS-3336
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params, Bill_PatientStatement.params.PanelID);
    },

    OpenSubmitFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_PatientStatement.params.PanelID + ' #hfSubmitFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-PMS-3336
        params["ParentCtrl"] = 'Bill_PatientStatement';
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-PMS-3336
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params, Bill_PatientStatement.params.PanelID);
    },

    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        LoadActionPan('Admin_Practice', params, Bill_PatientStatement.params.PanelID);
    },
    OpenPracticeDetail: function () {
        var params = [];
        params["PracticeId"] = $('#' + Bill_PatientStatement.params.PanelID + ' #hfPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPractice";
        LoadActionPan('practiceDetail', params, Bill_PatientStatement.params.PanelID);
    },
    LoadPatientStatements: function (PageNo, rpp, RowId, PatientStatementID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Payment Posting", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Bill_PatientStatement.params.PanelID + " #pnlStatement_Result").css("display") == "none") {
                    $('#' + Bill_PatientStatement.params.PanelID + " #pnlStatement_Result").show();
                }


                var stype = $('#' + Bill_PatientStatement.params.PanelID + ' .tabs-custom').find('li.active a').attr('href');


                //start syed zia 22-02-2016, PMS-4070
                //When patientstatement open from Follow up screen due to ids conflicts stype is undefiend so else condition run and statement filter working become stop
                if (stype == ".tabStatements" || typeof stype == "undefined") {
                    var self = $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatementsSearch");
                    var myJSON = self.getMyJSONByName();

                    //end syed zia 22-02-2016, PMS-4070
                    Bill_PatientStatement.SearchPatientStatements(myJSON, PageNo, rpp).done(function (response) {
                        if (response.status != false) {
                            if (response.PatientStatementCount > 0) {
                                //    $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPaymentPosting #txtInsuranceTransfer").trigger("keyup");
                                //    $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPaymentPosting #txtPatientTransfer").trigger("keyup");

                                //------------Pagination-----------
                                $("#" + Bill_PatientStatement.params["PanelID"] + " #divPostingStatementPaging").css("display", "inline");
                                //Showing 1 to 15 of 15 entries
                                var RecordsPerPage = rpp != null ? rpp : 300;
                                var CurrentPage = PageNo != null ? PageNo : 1;

                                //params["myJSON"] = myJSON;

                                var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                                var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                                if (PageNo == null) {
                                    utility.GetCustomPaging("divPostingStatementPaging", response.iTotalDisplayRecords, 5, "Bill_PatientStatement", CurrentPage, RecordsPerPage);
                                }
                                var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                                var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                                $("#" + Bill_PatientStatement.params["PanelID"] + " #divPostingStatementPaging #divShowingEntries").text(showingText);
                                // Change Background Color to Black for selected page
                                $("#" + Bill_PatientStatement.params["PanelID"] + " li").each(function () {
                                    if ($(this).text() == CurrentPage) {
                                        $(this).attr("class", "active");
                                    }
                                    else

                                        $(this).removeAttr("class");
                                });
                                //------------End Pagination-------
                            }
                            else {
                                $("#" + Bill_PatientStatement.params["PanelID"] + " #divPostingStatementPaging").css("display", "none");
                            }
                            Bill_PatientStatement.PatientStatementGridLoad(response, RowId);
                            if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
                                $('#' + Bill_PatientStatement.params.PanelID + ' #pnlStatement_Result .tabs-custom li a[href=#SubmittedStatements]').click();
                                //   $($('#' + Bill_PatientStatement.params.PanelID + ' #pnlStatement_Result .tabs-custom li')[0]).removeClass('active');
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                        $("#" + Bill_PatientStatement.params["PanelID"] + " #statementli").addClass("active");
                    });
                }
                else if (stype == ".tabSubmittedStatements") {
                    var self = $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlSubmittedStatementsSearch");
                    var myJSON = self.getMyJSONByName();
                    Bill_PatientStatement.SearchSubmittedPatientStatements(myJSON, PageNo, rpp, PatientStatementID).done(function (response) {
                        if (response.status != false) {
                            if (response.PatientStatementCount > 0) {
                                //   $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPaymentPosting #txtInsuranceTransfer").trigger("keyup");
                                // $('#' + Bill_PatientStatement.params.PanelID + " #frmBillPaymentPosting #txtPatientTransfer").trigger("keyup");
                                //------------Pagination-----------
                                $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementPaging").css("display", "inline");
                                //Showing 1 to 15 of 15 entries
                                var RecordsPerPage = rpp != null ? rpp : 300;
                                var CurrentPage = PageNo != null ? PageNo : 1;
                                //params["myJSON"] = myJSON;
                                var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                                var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                                if (PageNo == null) {
                                    utility.GetCustomPaging("divSubmittedStatementPaging", response.iTotalDisplayRecords, 5, "Bill_PatientStatement", CurrentPage, RecordsPerPage);
                                }
                                var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                                var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                                $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementPaging #divShowingEntries").text(showingText);
                                // Change Background Color to Black for selected page
                                $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementPaging li").each(function () {
                                    if ($(this).text() == CurrentPage) {
                                        $(this).attr("class", "active");
                                    }
                                    else
                                        $(this).removeAttr("class");
                                });
                                //------------End Pagination-------
                            }
                            else {
                                $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementPaging").css("display", "none");
                            }
                            Bill_PatientStatement.PrintedPatientStatementGridLoad(response, RowId);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    LoadSubmittedStatementsBatch: function (PageNo, rpp, RowId, PatientStatementID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Payment Posting", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Bill_PatientStatement.params.PanelID + " #pnlStatement_Result").css("display") == "none") {
                    $('#' + Bill_PatientStatement.params.PanelID + " #pnlStatement_Result").show();
                }
                var self = $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatementsBatchSearch");
                var myJSON = self.getMyJSONByName();
                Bill_PatientStatement.SearchPatientStatementsBatch(myJSON, PageNo, rpp, PatientStatementID).done(function (response) {
                    if (response.status != false) {
                        if (response.PatientStatementBatchCount > 0) {

                            //------------Pagination-----------
                            $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementBatchPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 100;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            //params["myJSON"] = myJSON;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divSubmittedStatementBatchPaging", response.iTotalDisplayRecords, 5, "Bill_PatientStatement", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementBatchPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementBatchPaging li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else

                                    $(this).removeAttr("class");
                            });
                            //------------End Pagination-------
                        }
                        else {
                            $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementBatchPaging").css("display", "none");
                        }

                        Bill_PatientStatement.SubmittedStatementBatchGridLoad(response, RowId);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    SearchPatientStatements: function (PatientStatementData, PageNumber, RowsPerPage) {


        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 300;
        }
        Bill_PatientStatement.params.CurrentPageNo = PageNumber;
        //var data = "PatientStatementData=" + PatientStatementData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        //// serach parameter , class name, command name of class
        //return MDVisionService.defaultService(data, "BILLING_STATEMENT", "SEARCH_PATIENT_STATEMENT");


        var objData = new JSON.constructor();
        if (PatientStatementData)
            objData = JSON.parse(PatientStatementData);

        objData["Age"] = $("#ddlAge").val();
        objData["PatientId"] = $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlStatementsSearch #hfPatientId").val();
        objData["FacilityId"] = $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlStatementsSearch #hfFacility").val();
        objData["RowsPerPage"] = RowsPerPage;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "search";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },
    SearchPatientStatementsBatch: function (PatientStatementData, PageNumber, RowsPerPage, PatientStatementID) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 300;
        }
        Bill_PatientStatement.params.CurrentPageNo = PageNumber;
        var objData = new JSON.constructor();
        if (PatientStatementData)
            objData = JSON.parse(PatientStatementData);

        var submittedBy = "";
        var submittedById = "";
        if ($("#ddlSubmittedBy").val()) {
            submittedBy = $("#ddlSubmittedBy option:selected").text();
            submittedById = $("#ddlSubmittedBy option:selected").val();
        }

        objData["PatientStatementID"] = PatientStatementID;
        objData["SubmittedBy"] = submittedBy;
        objData["SubmittedById"] = submittedById;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Search_patient_statements_batch";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },
    SearchSubmittedPatientStatements: function (PatientStatementData, PageNumber, RowsPerPage, PatientStatementID) {


        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 300;
        }
        Bill_PatientStatement.params.CurrentPageNo = PageNumber;
        //var data = "PatientStatementData=" + PatientStatementData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&PatientStatementID=" + PatientStatementID;
        //// serach parameter , class name, command name of class
        //return MDVisionService.defaultService(data, "BILLING_STATEMENT", "SEARCH_SUBMITTED_PATIENT_STATEMENT");
        var objData = new JSON.constructor();
        if (PatientStatementData)
            objData = JSON.parse(PatientStatementData);

        objData["PatientId"] = $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlSubmittedStatementsSearch #hfPatientId").val();
        objData["FacilityId"] = $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlSubmittedStatementsSearch #hfSubmitFacility").val();
        objData["PatientStatementID"] = PatientStatementID;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "search_submitted_statements";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },

    PrintPatientStatements: function (jsnData) {

        var objData = new JSON.constructor();
        objData["CommandType"] = "print_patient_statements";
        objData["ItemList"] = jsnData;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },
    PatientStatementGridLoad: function (response, RowId) {
        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement"))
            $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement").dataTable().fnDestroy();
        $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement tbody").find("tr").remove();
        if (response.PatientStatementCount > 0) {//response.ChargeCount
            var ChargeLoadJSONData = JSON.parse(response.PatientStatementLoad_JSON);
            $.each(ChargeLoadJSONData, function (i, item) {
                //var rowEvent = "";
                //if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
                //    rowEvent = "Bill_PatientStatement.openPatientResponsibilityPatient('" + item.PatientId + "','" + item.FacilityId + "','" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "','" + utility.RemoveTimeFromDate(null, item.DOSTo) + "');";
                //}
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'Bill_PatientStatement', event);";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvStatement_row" + i + "'));");
                $row.attr("id", "gvStatement_row" + i);
                $row.attr("StatementId", item.PatStatementId);
                $row.append('<span style="display:none">' + item.PatientId + "|" + item.LastName + "|" + item.FirstName + "|" + item.FacilityId + "|" + Number(item.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + "|" + item.SubmittedChargeIds + "|" + item.VisitId + "|" + item.GuarantorId + "|" + item.Age + '</span>')
                $row.append('<td style="display:none;">' + item.PatStatementId + '</td><td><input type="checkbox"  facilityId=' + item.FacilityId + ' patientId=' + item.PatientId + ' id="chkPatStmt' + item.PatStatementId + '" /></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.FacilityName + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.AdvancedPayment).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + utility.RemoveTimeFromDate(null, item.LastStatementDate) + '</td><td>' + item.StatementCount + '</td>');
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement tbody").last().append($row);
            });


        }
        else {
            if (!$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Bill_PatientStatement.params["PanelID"] + " #divPostingStatementPaging").css("display", "none");
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement").DataTable({
                    "language": {
                        "emptyTable": "No Patient Statement Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Patient Statement Found</td>');
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement tbody").last().append($row);
            }
        }
        //if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement"))
        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement") || $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvStatement").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 5, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        $('#' + Bill_PatientStatement.params["PanelID"] + ' #pnlStatement_Result #dgvStatement input[id*="chkPatStmt"]').each(function () {
            var value = $(this).prop('checked');
        });
    },

    SubmittedStatementBatchGridLoad: function (response, RowId) {
        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch"))
            $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch").dataTable().fnDestroy();
        $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch tbody").find("tr").remove();
        if (response.PatientStatementBatchCount > 0) {//response.ChargeCount
            var PatientStatementBatchJSONData = JSON.parse(response.PatientStatementBatchLoad_JSON);
            $.each(PatientStatementBatchJSONData, function (i, item) {
                var $row = $('<tr/>');
                var SubmitType = "";
                var viewBatchXMLMethod = '';

                if (item.SubmitType == "True") {
                    SubmitType = "Electronic";
                    viewBatchXMLMethod = "Bill_PatientStatement.OpenSubmittedStatementBatchXML('" + item.BatchId + "')";
                }
                else {
                    SubmitType = "Paper";
                    viewBatchXMLMethod = "utility.DisplayMessages('Statements were submitted on Paper', 1);";
                }
                viewBatchXMLMethod = '<a class="btn  btn-xs" href="#" onclick="' + viewBatchXMLMethod + '" title="View Batch XML"><i class="fa fa-file blue"></i></a>';
                var viewBatchPatientsMethod = '<a class="btn  btn-xs" href="#" onclick="Bill_PatientStatement.ViewBatchDetail(' + item.BatchId + "," + item.ClearingHouseId + ",'" + SubmitType + '\');" title="View Batch Detail"><i class="fa fa-calculator black"></i></a>';

                $row.attr("onclick", "utility.SelectGridRow($('#dgvSubmittedStatementBatch_row" + i + "'));");
                $row.attr("id", "gvStatement_row" + i);
                $row.attr("BatchId", item.BatchId);

                if (SubmitType == "Paper") {
                    item.ClearingHouse = "";
                }

                $row.append('<td style="display:none;">' + item.BatchId + '</td><td>' + viewBatchPatientsMethod + viewBatchXMLMethod + '</td><td>' + item.BatchNumber + '</td><td>' + item.SubmittedDate + '</td><td>' + item.SubmittedBy + '</td><td>' + SubmitType + '</td><td>' + item.ClearingHouse + '</td><td>' + item.TotalPatients + '</td><td>' + item.BatchStatus + '</td>');
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch tbody").last().append($row);
            });


        }
        else {
            if (!$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementBatchPaging").css("display", "none");
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch").DataTable({
                    "language": {
                        "emptyTable": "No Statement Batch Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Statement Batch Found</td>');
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch tbody").last().append($row);
            }
        }

        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch") || $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #dgvSubmittedStatementBatch").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    PrintedPatientStatementGridLoad: function (response, RowId) {
        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement"))
            $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement").dataTable().fnDestroy();
        $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement tbody").find("tr").remove();
        if (response.PatientStatementCount > 0) {

            var ChargeLoadJSONData = JSON.parse(response.PatientStatementLoad_JSON);
            $.each(ChargeLoadJSONData, function (i, item) {
                var rowEvent = "";
                var actionEvent = "";
                //start syed zia 16-02-2016, bug No #PMS-3418,remove check after discussion with salman
                if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
                    rowEvent = "Bill_PatientStatement.openPatientResponsibilityPatient('" + item.PatientId + "','" + item.FacilityId + "','" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "','" + utility.RemoveTimeFromDate(null, item.DOSTo) + "');";
                    actionEvent = "<a class='btn  btn-xs' href='#' onclick=" + rowEvent + " title='Payment Posting'><i class='fa fa-dollar green'></i></a>";
                }

                //end syed zia 16-02-2016, bug No #PMS-3418,remove check after discussion with salman
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'Bill_PatientStatement', event);";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#PrintStatementgvStatement_row" + i + "'));");
                $row.attr("id", "PrintStatementgvStatement_row" + i);
                $row.attr("StatementId", item.PatStatementId);
                var deleteAction = null;
                if (Bill_PatientStatement.isDeletePrivilige)
                    deleteAction = '<a class="btn  btn-xs" href="#" onclick="Bill_PatientStatement.StatementDelete(' + item.PatStatementId + ');" title="Delete Statement"><i class="fa fa-close red"></i></a>&nbsp';
                else
                    deleteAction = "";

                var action = '<td>' + deleteAction + '<a class="btn btn-xs"  onclick="Bill_PatientStatement.StatementShow(' + item.PatStatementId + ',' + item.PatientId + ',' + item.FacilityId + ',\'' + item.LastName + '\',\'' + item.FirstName + '\',' + Number(item.PatientBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + ',\'' + item.SubmittedChargeIds + '\',\'' + item.VisitIDs + '\',\'' + item.GuarantorId + '\',\'' + item.Age + '\');"   title="View Statement"><i class="fa fa-eye blue"></i></a>' + actionEvent + '</td>';
                $row.append('<td style="display:none;">' + item.PatStatementId + '</td> ' + action + '  <td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.FacilityName + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.AdvancedPayment).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PatientBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + utility.RemoveTimeFromDate(null, item.LastStatementDate) + '</td>');
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlPatientStatement_Search #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement tbody").last().append($row);
            });


        }
        else {
            if (!$("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Bill_PatientStatement.params["PanelID"] + " #divSubmittedStatementPaging").css("display", "none");
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlPatientStatement_Search #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement").DataTable({
                    "language": {
                        "emptyTable": "No Patient Statement Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Patient Statement Found</td>');
                $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement tbody").last().append($row);
            }


        }
        //if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement"))
        if ($.fn.dataTable.isDataTable("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement") || $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Bill_PatientStatement.params["PanelID"] + " #pnlPatientStatement_Search #pnlStatement_Result #SubmittedStatements #dgvSubmittedStatement").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[8, "desc"]], "bFilter": true, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },
    saveStatements: function (isFromSubmitted, submittedStatementdata) {
        AppPrivileges.GetFormPrivileges("Patient Statement", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var isStatementExist = "false";
                var displayMsg = "";
                var Status = true;
                var count = 0;
                var strData = "";
                var selectedCount = 0;

                if (submittedStatementdata) {
                    //in case of "submitted" and "submitted statement batch" bind the selected object.
                    strData = submittedStatementdata;
                }
                else {
                    $('#' + Bill_PatientStatement.params["PanelID"] + ' #pnlStatement_Result #dgvStatement input[id*="chkPatStmt"]').each(function () {
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
                            rowData['Age'] = result[8];

                            rowData['DOSTo'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSTo').val();
                            rowData['DOSFrom'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSFrom').val();
                            strData += JSON.stringify(rowData) + ",";
                        }
                    });
                    strData = "[" + strData.slice(0, -1) + "]";
                }
                Bill_PatientStatement.SavePatientStatement(strData, isFromSubmitted).done(function (response) {
                    if (response.Message != "Already Submitted") {

                        if (response.status) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 4)
                        }
                        Bill_PatientStatement.LoadPatientStatements();
                    }
                    else {
                        utility.DisplayMessages("Statement already submitted.", 4);
                    }

                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    CreateSelectStatementJson: function (data, markUp, statementcount) {
        Bill_PatientStatement.SelectedStatementCount++;
        if (Bill_PatientStatement.SelectedStatementCount == statementcount) {
            Bill_PatientStatement.SelectedStatementJson += data;

        }
        else {
            Bill_PatientStatement.SelectedStatementJson += data + ",";
        }
    },

    SavePatientStatement: function (jsnData, isFromSubmitted) {
        //start syed zia 11-02-2016,bug #PMS-3863 ,replace special character
        var clearinghouseId = $('#' + Bill_PatientStatement.params["PanelID"] + ' #ddlClearinghouse option:selected').val();

        var objData = new JSON.constructor();
        objData["CommandType"] = "save_patient_statement";
        objData["ItemList"] = utility.replaceSpecialCharacters(jsnData);
        objData["MarkUp"] = "";
        objData["isFromSubmitted"] = isFromSubmitted;
        objData["ClearingHouseId"] = clearinghouseId;
        objData["IgnoreCycleDays"] = $('#' + Bill_PatientStatement.params["PanelID"] + ' #chkIgnoreCycleDays').prop("checked");
        var data = objData;
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },
    StatementShow: function (SubmittedStatementId, patientId, facilityId, lastName, FirstName, patientBal, chargeIds, VisitIDs, gurantorID, Age) {

        var infoArr = new Array();
        var dummy = new Object();
        dummy["PatientId"] = patientId;
        dummy["LastName"] = lastName;
        dummy["FirstName"] = FirstName;
        dummy["FacilityId"] = facilityId;
        dummy["PatBalance"] = patientBal;
        dummy["SubmittedChargeIds"] = chargeIds;
        dummy["VisitIDs"] = VisitIDs;
        dummy["GuarantorId"] = gurantorID;
        dummy["Age"] = Age;


        dummy["SubmittedStatementId"] = SubmittedStatementId;
        dummy['DOSTo'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSTo').val();
        dummy['DOSFrom'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSFrom').val();

        infoArr.push(dummy);
        Bill_PatientStatement.GetSubmittedStatementHtmL(SubmittedStatementId).done(function (response) {
            if (response.status == true) {
                var docType = '<!doctype html>';
                var docCnt = response.SubmittedStatementLoad_JSON;
                docCnt = docCnt.replace(/&lt;/g, '<');
                docCnt = docCnt.replace(/&gt;/g, '>');
                docCnt = docCnt.replace(/&quot;/g, '\"');
                $.each($(docCnt).find('img'), function (index, image) {
                    var base64 = $(image).attr('src');
                    base64 = base64.replace(/ /g, "+")
                    base64 = base64.replace(/'.'/g, "+")
                    docCnt = docCnt.replace($($(docCnt).find('img')[index]).attr('src'), base64);
                });
                var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
              + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script><script src="Scripts/MDVisionDefaultCommon.js"></script>'
              + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print();setTimeout(function () { utility.myConfirm("Do you want to submit statement ?", function () {window.opener.Bill_PatientStatement.saveStatements("false");}, function () {},"<b>Confirm submission</b>"); }, 100); } '
              + '</script>'
              + '</head>';
                //var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"><script src="Scripts/js/bootstrap.js"></script></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" /><link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script><script> function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");window.print(); }</script></head>';
                var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no, dependent=no, width=865, height=600, resizable=true, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                var newWin = window.open("", "_blank", winAttr);
                writeDoc = newWin.document;
                writeDoc.open();
                writeDoc.write(docType + '<html>' + docHead + '<body> ' + "<button class='btn btn-success bt-sm m-xs pull-right' id='btnPrint' onclick='RemoveBottomSpace();'><i class='glyphicon glyphicon-print'></i> Print</button>" + docCnt + '</body></html>');
            }
            else {
                Bill_PatientStatement.ViewAndPrintPatientStatement(JSON.stringify(infoArr), false);
            }
        });
    },
    GetSubmittedStatementHtmL: function (SubmittedStatementId) {
        var objData = new JSON.constructor();
        objData["CommandType"] = "Get_Submitted_Statement_HtmL";
        objData["SubmittedStatementId"] = SubmittedStatementId;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },
    StatementDelete: function (PatStmtID) {
        utility.myConfirm('1', function () {
            var selectedValue = PatStmtID;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Bill_PatientStatement.DeleteStatement(PatStmtID).done(function (response) {
                    if (response.status == true) {
                        Bill_PatientStatement.LoadPatientStatements(null, null, null, null);
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
        }, function () { });
    },
    DeleteStatement: function (PatStmtID) {
        //var data = "PatStmtID=" + PatStmtID;
        //return MDVisionService.defaultService(data, "BILLING_STATEMENT", "DELETE_PATIENT_STATEMENT");
        var objData = new JSON.constructor();
        objData["CommandType"] = "delete_statement";
        objData["PatientStatementID"] = PatStmtID;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },
    ResetFilters: function () {
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlStatementsSearch').resetAllControls();
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlStatementsSearch #txtc').val("");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlStatementsSearch #txtPatBalLess').val("");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlStatementsSearch #ddlClearinghouse option:eq(1)').attr("selected", "selected");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlStatementsSearch [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        var formValidation = $("#" + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement').data("bootstrapValidator");
        formValidation.enableFieldValidators('AccountNumber', false);
        $("#lblAccountNumber").css("display", "none");
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #dtpDOSTo').attr('disabled', true);
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #LastStatmentDateTo').attr('disabled', true);
        /// PRD-91
        // reset facility to default state
        if ($("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #lnkFacilityEdit").is(":visible"))
        { $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #lnkFacilityEdit").hide(); }
        if (!$("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #lblFacility").is(":visible"))
        { $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #lblFacility").show(); }
    },
    ResetBatchFilters: function () {
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #SubmittedStatementsBatch').resetAllControls();
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #SubmittedStatementsBatch #txtc').val("");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #SubmittedStatementsBatch #txtPatBalLess').val("");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlStatementsSearch #ddlClearinghouse option:eq(1)').attr("selected", "selected");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #SubmittedStatementsBatch [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #dtpDOSTo').attr('disabled', true);
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #LastStatmentDateTo').attr('disabled', true);
    },

    SubmitResetFilters: function () {
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlSubmittedStatementsSearch').resetAllControls();
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlSubmittedStatementsSearch #txtc').val("");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlSubmittedStatementsSearch #txtPatBalLess').val("");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlStatementsSearch #ddlClearinghouse option:eq(1)').attr("selected", "selected");
        $('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #pnlSubmittedStatementsSearch [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        /// PRD-91
        // reset facility to default state
        if ($("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlSubmittedStatementsSearch #lnkFacilityEdit").is(":visible"))
        { $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlSubmittedStatementsSearch #lnkFacilityEdit").hide(); }
        if (!$("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlSubmittedStatementsSearch #lblFacility").is(":visible"))
        { $("#" + Bill_PatientStatement.params["PanelID"] + " #frmBillPatientStatement #pnlSubmittedStatementsSearch #lblFacility").show(); }
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #dtpDOSTo').attr('disabled', true);
        //$('#' + Bill_PatientStatement.params["PanelID"] + ' #frmBillPatientStatement #LastStatmentDateTo').attr('disabled', true);
    },
    ViewStatementXML: function (jsnData) {

        //var data = "data=" + jsnData;
        //// serach parameter , class name, command name of class
        //return MDVisionService.defaultService(data, "BILLING_STATEMENT", "PATIENT_STATEMENT_XML");

        var objData = new JSON.constructor();
        objData["CommandType"] = "view_statement_xml";
        objData["ItemList"] = jsnData;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },


    OpenSubmittedStatementBatchXML: function (BatchId) {

        Bill_PatientStatement.ViewBatchXML(BatchId).done(function (response) {
            if (response.status) {
                $("#" + Bill_PatientStatement.params.PanelID + " #xmlData").text(response.Statement_XML);
                $("#" + Bill_PatientStatement.params.PanelID + " #btnDownloadXML").show();


                $("#" + Bill_PatientStatement.params.PanelID + " #statementXMLConatiner").modal({
                    show: 'true',
                    backdrop: 'static',
                    keyboard: false
                });
            }
            else {
                utility.DisplayMessages(response.Message, 4)
            }
        });

    },

    ViewBatchXML: function (BatchId) {
        var objData = new JSON.constructor();
        objData["CommandType"] = "view_batch_xml";
        objData["BatchId"] = BatchId;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "PatientStatements", "PatientStatement");
    },

    ViewBatchDetail: function (BatchId, ClearingHouseId, SubmitType) {
        var params = [];
        params["FromAdmin"] = "0";
        params["BatchId"] = BatchId;
        params["SubmitType"] = SubmitType;
        params["ClearingHouseId"] = ClearingHouseId;
        if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
            params["ParentCtrl"] = "Bill_PatientStatement";
        }
        else {
            params["ParentCtrl"] = "billTabPatientStatement";
        }
        LoadActionPan('Bill_PatientStatementBatch', params, Bill_PatientStatement.params.PanelID);

    },
    StatementXMLView: function () {
        var selectedObj = $('#' + Bill_PatientStatement.params["PanelID"] + ' input[id*="chkPatStmt"]:checked').map(function () {
            return this;
        }).get();

        if (selectedObj.length == 0) {
            utility.DisplayMessages("Please select statement(s) to print.", 4);
        }
        else {
            var infoArr = new Array();
            $.each(selectedObj, function (i, item) {
                var dummy = new Object();
                dummy['DOSTo'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSTo').val();
                dummy['DOSFrom'] = $('#' + Bill_PatientStatement.params.PanelID + ' #dtpDOSFrom').val();
                dummy['PatientId'] = $(this).attr('patientid');
                dummy['FacilityId'] = $(this).attr('facilityid');
                infoArr.push(dummy);
            });
            Bill_PatientStatement.ViewStatementXML(JSON.stringify(infoArr)).done(function (response) {
                if (response.status) {
                    $("#" + Bill_PatientStatement.params.PanelID + " #xmlData").text(response.Statement_XML);
                    $("#" + Bill_PatientStatement.params.PanelID + " #btnDownloadXML").hide();
                    $("#" + Bill_PatientStatement.params.PanelID + " #statementXMLConatiner").modal({
                        show: 'true',
                        backdrop: 'static',
                        keyboard: false
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 4)
                }
            });
        }
    },
    downloadXML: function () {
        var xmlArr = new Array();
        xmlArr.push($("#" + Bill_PatientStatement.params.PanelID + " #xmlData").text());
        var blob = new Blob(xmlArr, { type: 'text/xml' });
        saveAs(blob, "PatientStatement.xml");
    },
    removeDialogClasses: function () {
        //$('#' + Bill_PatientStatement.params.PanelID + ' .modal-header').hide();
        // $('#' + Bill_PatientStatement.params.PanelID + ' #modalbody').removeClass('panel-body');
        $('#' + Bill_PatientStatement.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Bill_PatientStatement.params.PanelID + ' .modal-dialog').removeAttr('class');
        $('#' + Bill_PatientStatement.params.PanelID + ' #containerModalDialog').removeClass('modal-dialog');
        // $('#' + Bill_PatientStatement.params.PanelID + ' #DivEncounterTabs').addClass('ml-sm');

    },
    openPatientResponsibilityPatient: function (patientID, facilityId, DOSFrom, DOSTo) {
        var parammeters = {};
        parammeters["patientID"] = patientID;
        parammeters["facilityId"] = facilityId;

        if (Bill_PatientStatement.params.BatchId) {
            parammeters["BatchId"] = Bill_PatientStatement.params.BatchId;
        }
        else {
            parammeters["BatchId"] = "";
        }
        parammeters["DOSFrom"] = DOSFrom;
        parammeters["DOSTo"] = DOSTo;
        if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
            parammeters["ParentCtrl"] = "Bill_PatientStatement";
        }
        LoadActionPan("Bill_PatientResponsibilityPayment", parammeters, Bill_PatientStatement.params.PanelID);


    },
    UnLoadTab: function (Tab) {

        if (Bill_PatientStatement.params != null && Bill_PatientStatement.params.ParentCtrl != null) {
            if (Bill_PatientStatement.params.ParentCtrl == "billTabPaymentPosting" || Bill_PatientStatement.params.ParentCtrl == "Bill_PaymentPosting") {
                UnloadActionPan(Bill_PatientStatement.params.ParentCtrl, "Bill_PatientStatement", null, "pnlBillPaymentPosting");
            }
            else {
                UnloadActionPan(Bill_PatientStatement.params.ParentCtrl, "Bill_PatientStatement");
            }

        }
        else
            RemoveAdminTab(Tab);
        Bill_PatientStatement.params = null;
    },
    UnLoadXMLView: function () {
        $("#" + Bill_PatientStatement.params.PanelID + " #xmlData").text('');
        $("#" + Bill_PatientStatement.params.PanelID + " #statementXMLConatiner").modal('hide');
    },
    //SelectedPageClick: function (PageNo, objPage) {
    //    // Change Background Color to Black for selected page
    //    $("#" + Bill_PatientStatement.params["PanelID"] + " li").each(function () {
    //        if ($(this).text() == PageNo) {
    //            $(this).attr("class", "active");
    //        }
    //        else
    //            $(this).removeAttr("class");
    //    });
    //    Bill_PatientStatement.LoadSubmittedStatementsBatch(PageNo, 300,null,null);
    //},
}