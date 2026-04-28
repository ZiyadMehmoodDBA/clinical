Patient_AdvancePayment = {

    params: [],
    bIsFirstLoad: true,

    Load: function (params) {


        Patient_AdvancePayment.params = params;

        //if (Patient_AdvancePayment.params["PanelID"] != 'pnlAdvancePayment')
        //    Patient_AdvancePayment.params["PanelID"] = Patient_AdvancePayment.params["PanelID"] + ' #pnlAdvancePayment';

        var self = null;

        if (Patient_AdvancePayment.params.PanelID != "pnlAdvancePayment")
            self = $('#' + Patient_AdvancePayment.params.PanelID + ' #pnlAdvancePayment');
        else
            self = $('#' + Patient_AdvancePayment.params.PanelID);


        if (Patient_AdvancePayment.bIsFirstLoad) {
            Patient_AdvancePayment.bIsFirstLoad = false;
            // self = $('#' + Patient_AdvancePayment.params["PanelID"]);


            self.loadDropDowns(true).done(function () {

                //Patient_AdvancePayment.ValidateAdvancePayment();
                //Insert all option in ddlSearchSex
                try {
                    var ddlPaymentType = self.find("select[id*='ddlPaymentType']");
                    ddlPaymentType.prepend('<option value="" selected="selected">All</option>');

                    ddlPaymentType.find("option").each(function () {
                        if ($(this).text().toLowerCase() == "advance payment") {
                            $(this).remove();
                            return;
                        }

                    });
                } catch (ex) {
                    console.log(ex);
                }


                Patient_AdvancePayment.SearchAdvancePayment();
            });
            Patient_AdvancePayment.BindFacility();
        }



        utility.ValidateFromToDate('frmAdvancePayment', 'dtpPaidFrom', 'dtpPaidTo', true);

        Patient_AdvancePayment.setSelectedPatientInfo();



    },


    setSelectedPatientInfo: function () {

        var AccountNo = $("#PatientProfile #hfAccountNo").val();
        var PatientFullName = $("#PatientProfile #hfPatientFullName").val();
        var PatientId = $("#PatientProfile #hfPatientId").val();

        if (AccountNo.length > 0) {
            $('#pnlAdvancePayment #txtPatientName').val(AccountNo);
        }

        if (PatientFullName.length > 0) {
            Firstname = PatientFullName.split(" ")[1];
            Lastname = PatientFullName.split(" ")[0];
            MiddleInitial = PatientFullName.split(" ")[2];

            $('#pnlAdvancePayment #txtFullName').val(Lastname + " " + Firstname + " " + MiddleInitial);
        }

        if (PatientId.length > 0) {
            $('#pnlAdvancePayment #hfPatientId').val(PatientId);
        }
    },

    UnLoad: function () {

        if (Patient_AdvancePayment.params != null && Patient_AdvancePayment.params.ParentCtrl != null) {
            UnloadActionPan(Patient_AdvancePayment.params.ParentCtrl, 'Patient_AdvancePayment');
        }
        else
            UnloadActionPan(null, 'Patient_AdvancePayment');
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_AdvancePayment';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#pnlAdvancePayment #hfPatientId").val(PatientId);
        $("#pnlAdvancePayment #txtPatientName").val(patFullName.split(" - ")[0]);
        $("#pnlAdvancePayment #txtFullName").val(patFullName.split(" - ")[1]);
        //appointmentDetail.FillPatientAccount(PatientId);
        //item.AccountNumber + " - " + item.FullName
        UnloadActionPan("Patient_AdvancePayment");
        utility.InsertRecentPatient(PatientId);
    },

    BindPatientAccount: function () {
        $("#pnlAdvancePayment #txtPatientName").autocomplete({
            autoFocus: true,
            //source: AllPatients, // pass an array (without a comma)
            source: function (request, response) {

                var AccountNo = $('#pnlAdvancePayment #txtPatientName').val();
                if (AccountNo.length > 2) {
                    // serach parameter , class name, command name of class
                    appointmentDetail.LoadActivePatients(AccountNo).done(function (responseData) {
                        if (responseData.status != false) {
                            if (responseData.PatientCount > 0) {
                                var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                                var AllPatients = [];
                                $.each(PatientLoadJSONData, function (i, item) {
                                    AllPatients.push({ id: item.PatientId, value: item.AccountNumber + " - " + item.FullName });
                                });
                                response(AllPatients);
                            }
                        }
                    });
                }
            },

            select: function (event, ui) {

                //$("#appointmentDetail #txtAccountNo").val(ui.item.id); // add the selected id
                //$("#appointmentDetail #txtFullName").val(ui.item.patientName);

                setTimeout(function () {
                    $("#pnlAdvancePayment #hfPatientId").val(ui.item.id);
                    $("#pnlAdvancePayment #txtPatientName").val(ui.item.value.split(" ")[0]);
                    $("#pnlAdvancePayment #txtFullName").val(ui.item.value.split(" ")[2]);

                }, 100);

                //$("#hfpatientid").val(ui.item.id);
            }
        });
    },


    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_AdvancePayment";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + Patient_AdvancePayment.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Patient_AdvancePayment';
        LoadActionPan('facilityDetail', params);
    },

    SearchAdvancePayment: function (PrimaryID, PageNumber, ResultPerPage) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Advance Payment", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if ($("#" + Patient_AdvancePayment.params["PanelID"] + " #pnlAdvancePayment_Result").css("display") == "none") {
                    $("#" + Patient_AdvancePayment.params["PanelID"] + " #pnlAdvancePayment_Result").show();
                }

                var PaymentId = null;




                if ($('#pnlAdvancePayment #txtPatientName').val() == "") {
                    //start syed zia 02-02-2016,bug #PMS-3800
                    //  $('#pnlAdvancePayment #hfPatientId').val(-1);
                    //end syed zia 02-02-2016,bug #PMS-3800
                    $('#pnlAdvancePayment #divAdvanceBalance').hide();

                }
                else {
                    $('#pnlAdvancePayment #divAdvanceBalance').show();
                    $('#pnlAdvancePayment #lblAdvanceBalance').text('00.00');
                }

                if ((Patient_AdvancePayment.params["ParentCtrl"] == "Bill_PaymentPosting" || Patient_AdvancePayment.params["ParentCtrl"] == "billTabPaymentPosting" || Patient_AdvancePayment.params["ParentCtrl"] == "schcopayment" || Patient_AdvancePayment.params["ParentCtrl"] == "Bill_PatientResponsibilityPayment" || Patient_AdvancePayment.params["ParentCtrl"] == "demographicDetail" || Patient_AdvancePayment.params["ParentCtrl"] == "Scheduling_UnallocatedCopayment") && Patient_AdvancePayment.params["patientID"] != null) {

                    $('#pnlAdvancePayment #divPatientId').hide();
                    $('#pnlAdvancePayment #divPatientName').hide();
                    $('#pnlAdvancePayment #hfPatientId').val(Patient_AdvancePayment.params["patientID"]);
                    if (Patient_AdvancePayment.params["ParentCtrl"] != "demographicDetail")
                        $('#pnlAdvancePayment #btnAdvancePaymentAdd').hide();

                }



                //if facility field is empty its corresponding hidden field is being reset
                if ($('#pnlAdvancePayment #txtFacility').val() == "") {
                    $('#pnlAdvancePayment #hfFacility').val(-1);
                }

                // PatientID = Patient_AdvancePayment.params.patientID;

                var self = $("#pnlAdvancePayment");
                var myJSON = self.getMyJSON();


                Patient_AdvancePayment.AdvancePaymentSearch(myJSON, PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                    if (response.status != false) {
                        Patient_AdvancePayment.AdvancePaymentGridLoad(response);

                        var TableControl = $("#pnlAdvancePayment_Result #dgvAdvancePayment");
                        var PagingPanelControlID = Patient_AdvancePayment.params.PanelID + " #dgvAdvancePayment_Paging";
                        var ClassControlName = "Patient_AdvancePayment";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.MessageCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Patient_AdvancePayment.SearchAdvancePayment(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
                //Patient_Case.CaseGridLoad();

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    AdvancePaymentSearch: function (JSONstr, PrimaryID, PageNumber, ResultPerPage) {
        if (JSONstr == null) {
            JSONstr = "";
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (ResultPerPage == null) {
            ResultPerPage = 15;
        }

        //if (AdvancePaymentID == null) {
        //    AdvancePaymentID = 0;
        //}
        //FIXME
        var data = "AdvancePaymentData=" + JSONstr + "&pageNumber=" + PageNumber + "&rowsPerPage=" + ResultPerPage;

        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ADVANCE_PAYMENT", "SEARCH_PATIENT_ADVANCE_PAYMENT");
    },

    AdvancePaymentAddEdit: function (AdvancePaymentID, mode, PatientName, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        if (mode == "Add") {
            AppPrivileges.GetFormPrivileges("Advance Payment", "Add", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    var params = [];
                    params["AdvancePaymentID"] = AdvancePaymentID;
                    params["PatientName"] = PatientName;
                    params["patientID"] = Patient_AdvancePayment.params.patientID;
                    params["mode"] = mode;
                    params["ParentCtrl"] = "Patient_AdvancePayment";

                    //------------------------------------
                    params["PatAccountNumber"] = Patient_AdvancePayment.params.PatAccountNumber;
                    params["PatFirstName"] = Patient_AdvancePayment.params.PatFirstName;
                    params["PatLastName"] = Patient_AdvancePayment.params.PatLastName;
                    params["PatMidInitial"] = Patient_AdvancePayment.params.PatMidInitial;
                    params["IsPatientDetail"] = Patient_AdvancePayment.params.IsPatientDetail;
                    //------------------------------------

                    LoadActionPan('advancePaymentDetail', params);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else {
            AppPrivileges.GetFormPrivileges("Advance Payment", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    var params = [];
                    params["AdvancePaymentID"] = AdvancePaymentID;
                    params["PatientName"] = PatientName;
                    params["PatientId"] = PatientId;
                    params["mode"] = mode;
                    params["ParentCtrl"] = "Patient_AdvancePayment";
                    LoadActionPan('advancePaymentDetail', params);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    AdvancePaymentGridLoad: function (response) {
        $("#pnlAdvancePayment #pnlAdvancePayment_Result #dgvAdvancePayment").dataTable().fnDestroy();
        $("#pnlAdvancePayment #pnlAdvancePayment_Result #dgvAdvancePayment tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var AdvancePaymentLoadJSONData = JSON.parse(response.AdvancePaymentLoad_JSON);

            var TotalBalance = 0;
            $.each(AdvancePaymentLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "dgvAdvancePayment_row" + item.AdvPaymentId);
                $row.attr("AdvancePaymentId", item.AdvPaymentId);

                var selectMethod = "";
                var selMethod = "";
                var rowStyle = "";
                var copayColor = "";

                if (item.IsRefund.toLowerCase() != "true" && parseFloat(item.Balance) > 0) {

                    tglclass = "fa fa-money green";
                    var RefundAdvancePaymentMethod = "Patient_AdvancePayment.RefundAdvancePayment(" + item.AdvPaymentId + ",event);";


                    RefundAdvancePaymentMethod = '<a class="btn  btn-xs" href="#" onclick="' + RefundAdvancePaymentMethod + '" title="Refund Advance Payment"><i class="' + tglclass + '"></i></a>';
                    if (Patient_AdvancePayment.params["ParentCtrl"] == "Bill_PaymentPosting" || Patient_AdvancePayment.params["ParentCtrl"] == "billTabPaymentPosting" || Patient_AdvancePayment.params.ParentCtrl == "Bill_PatientResponsibilityPayment") {
                        selMethod = "Patient_AdvancePayment.FillAdvancePayment('" + item.AdvPaymentId + "','" + utility.RemoveTimeFromDate(null, item.PaymentDate) + "','" + item.Balance + "',event);";
                        selectMethod = '<a class="btn  btn-xs " href="#" onclick=' + selMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    }
                    else if (Patient_AdvancePayment.params["ParentCtrl"] == "schcopayment") {
                        selMethod = "schcopayment.FillAdvancePayment('" + item.AdvPaymentId + "','" + utility.RemoveTimeFromDate(null, item.PaymentDate) + "','" + item.Balance + "',event);"
                        selectMethod = '<a class="btn  btn-xs " href="#" onclick=' + selMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    }
                    else if (Patient_AdvancePayment.params["ParentCtrl"] == "Scheduling_UnallocatedCopayment") {
                        selMethod = "Scheduling_UnallocatedCopayment.FillAdvancePayment('" + item.AdvPaymentId + "','" + utility.RemoveTimeFromDate(null, item.PaymentDate) + "','" + item.Balance + "',event);"
                        selectMethod = '<a class="btn  btn-xs " href="#" onclick=' + selMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    }
                }
                else {

                    // tglclass = "fa fa-money red";
                    //RefundAdvancePaymentMethod = "Patient_AdvancePayment.RefundAdvancePayment(" + item.AdvPaymentId + ");";

                    RefundAdvancePaymentMethod = '';// '&nbsp;&nbsp;<span title="Advance Payment Refunded"><i class="' + tglclass + '"></i></span>';
                    selectMethod = "";
                }

                // if payment is credited then its sourrounded by paranthesis

                var myAmount;
                if (item.AmtPaidCr != "") {

                    //credit amount
                    myAmount = utility.convertToFigure(item.Amount, true, true);

                    var EditMethod = '';

                    rowStyle = "border:solid 1px red!important;"
                    copayColor = "color:red;font-weight:bold;"
                }
                else {

                    rowStyle = "border:solid 1px green!important;"

                    //myAmount = Number(item.Amount).toFixed(Number(globalAppdata.DecimalPlaces));
                    //debit amount
                    myAmount = utility.convertToFigure(item.Amount, true);

                    EditMethod = "Patient_AdvancePayment.AdvancePaymentAddEdit('" + item.AdvPaymentId + "','Edit','" + item.PatientName + "','" + item.PatientId + "',event);";
                    EditMethod = '<a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a>';
                }
                if (selMethod != '') {
                    $row.attr("onclick", selMethod);
                }
                else if (EditMethod != "") {
                    $row.attr("onclick", "Patient_AdvancePayment.AdvancePaymentAddEdit('" + item.AdvPaymentId + "','Edit','" + item.PatientName + "','" + item.PatientId + "',event);");
                }
                $row.append('<td style="display:none; ' + rowStyle + '">' + item.AdvPaymentId + '</td><td style = "' + rowStyle + '">' + EditMethod + RefundAdvancePaymentMethod + selectMethod + '&nbsp;</td><td style = "' + rowStyle + '">' + item.PatientName + '</td><td style = "' + rowStyle + '">' + item.FacilityName + '</td><td style = "' + rowStyle + '' + copayColor + '">' + myAmount + '</td><td style = "' + rowStyle + '">' + utility.convertToFigure(item.Balance, true) + '</td><td style = "' + rowStyle + '">' + utility.RemoveTimeFromDate(null, item.PaymentDate) + '</td>');

                $("#pnlAdvancePayment #pnlAdvancePayment_Result #dgvAdvancePayment tbody").last().append($row);


                TotalBalance += parseFloat(item.Balance);

            });

            // Start 22/02/2016 Muhammad Irfan for bug # PMS-4060
            $("#pnlAdvancePayment #lblAdvanceBalance").text(TotalBalance.toFixed(Number(globalAppdata.DecimalPlaces)));
            // End 22/02/2016 Muhammad Irfan for bug # PMS-4060
        }
        else {

            //FIXME
            if ($("#pnlAdvancePayment #pnlAdvancePayment_Result").css("display") == "none") {
                $("#pnlAdvancePayment #pnlAdvancePayment_Result").show();
            }

            $("#pnlAdvancePayment #pnlAdvancePayment_Result #dgvAdvancePayment").DataTable({
                "language": {
                    "emptyTable": "No Advance Payments Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bPaginate": false, "bSortable": false, "aTargets": [1] }]
            });


        }
        if ($.fn.dataTable.isDataTable("#pnlAdvancePayment #pnlAdvancePayment_Result #dgvAdvancePayment"));
        else
            $("#pnlAdvancePayment #pnlAdvancePayment_Result #dgvAdvancePayment").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },


    RefundAdvancePayment: function (AdvancePaymentId, event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure want to refund?', function () {
                    var selectedValue = AdvancePaymentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {

                        Patient_AdvancePayment.AdvancePaymentRefund(selectedValue).done(function (response) {
                            if (response.status != false) {


                                // var patcopaymentDetail = JSON.parse(response.PatientPayments_JSON);
                                Patient_AdvancePayment.SearchAdvancePayment();

                                //update the patient balances in patient banner
                                Patient_Demographic.UpdateBalancesInBanner();

                                utility.DisplayMessages(response.Message, 1);
                                // UnloadActionPan(Patient_AdvancePayment.params["ParentCtrl"], "actionPanCopayment");

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    'Refund confirm'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    AdvancePaymentRefund: function (AdvancePaymentId) {
        var data = "AdvancePaymentId=" + AdvancePaymentId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ADVANCE_PAYMENT", "REFUND_PATIENT_ADVANCE_PAYMENT");
    },

    BindFacility: function () {
        var Ctrl = $("#" + Patient_AdvancePayment.params.PanelID + " #frmAdvancePayment #txtFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_AdvancePayment.params.PanelID + " #frmAdvancePayment #hfFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    FillAdvancePayment: function (AdvancePaymentId, datePaid, Balance) {

        var RefCtrl = " #txtAdvancePayment";
        // var PaidCtrl = " #txtPatientPaid";
        var balanceCtrl = " #hfAdvanceBalance";
        var RefHiddenIdCtrl = " #hfAdvancePaymentId";
        if (Patient_AdvancePayment.params["RefCtrl"] != null) {
            RefCtrl = " #" + Patient_AdvancePayment.params["RefCtrl"];
        }
        if (Patient_AdvancePayment.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Patient_AdvancePayment.params["RefHiddenIdCtrl"];
        }
        $('#' + Patient_AdvancePayment.params["PanelID"] + RefCtrl).val(datePaid + " - " + Balance);
        //start syed zia 02-02-2016, bug #PMS-3792
        //  $('#' + Patient_AdvancePayment.params["PanelID"] + PaidCtrl).val(Balance);
        //start syed zia 02-02-2016, bug #PMS-3792
        $('#' + Patient_AdvancePayment.params["PanelID"] + RefHiddenIdCtrl).val(AdvancePaymentId);
        $('#' + Patient_AdvancePayment.params["PanelID"] + balanceCtrl).val(Balance);

        // $('#' + Patient_AdvancePayment.params["PanelID"] + ' #lblAdvancePayment').css("display", "none");
        // $('#' + Patient_AdvancePayment.params["PanelID"] + ' #lnkAdvancePaymentEdit').css("display", "inline");


        if (Patient_AdvancePayment.params["ParentCtrl"] != null) {
            if (Patient_AdvancePayment.params["ParentCtrl"] == "Bill_PaymentPosting" && Patient_AdvancePayment.params.PaymentRef != null)
                UnloadActionPan(Patient_AdvancePayment.params.ParentCtrl, "Patient_AdvancePayment", null, Patient_AdvancePayment.params.PaymentRef);



            else {

                UnloadActionPan(Patient_AdvancePayment.params["ParentCtrl"]);
            }

        }

        // UnloadActionPan(Patient_AdvancePayment.params["ParentCtrl"], Patient_AdvancePayment.params["ParentCtrl"]);

        $('#' + Patient_AdvancePayment.params["PanelID"] + RefCtrl).focus();
    },

}