advancePaymentDetail = {

    params: [],
    bIsFirstLoad: true,

    Load: function (params) {


        advancePaymentDetail.params = params;

        //initialization of date-pickers.
        advancePaymentDetail.InitializeControl();

        var self = $('#pnlPatientAdvancePayment');
        self.loadDropDowns(true).done(function () {

            advancePaymentDetail.ValidateAdvancePayment();

            $.when(utility.ValidateDOB('frmPatientAdvancePayment', 'dtpDatePaid', new Date('1800-01-01'), new Date(), false)).done(function () {

                $("#frmPatientAdvancePayment #dtpDatePaid").datepicker("setDate", $.datepicker.formatDate(globalAppdata['DateFormat'].replace('yy', ''), new Date()));


            });

            $("#frmPatientAdvancePayment  #ddlPaymentType option").each(function () {
                if ($(this).text().toLowerCase() == "advance payment") {
                    $(this).remove();
                    return;
                }

            });

            $("#frmPatientAdvancePayment #ddlPaymentType").val(1).attr("selected", "selected").trigger('change');

            advancePaymentDetail.fillLedgerAccount();

            $('#pnlPatientAdvancePayment #txtEnteredBy').val(globalAppdata.AppUserNameFullName);
            //$('#pnlPatientAdvancePayment #dtpEntryDate').val($.datepicker.formatDate('mm/dd/yy', new Date()));

            utility.CreateDatePicker('pnlPatientAdvancePayment #dtpEntryDate', function () {
                //on-change callback method


            }, true);

            advancePaymentDetail.params["PanelID"] = "pnlPatientAdvancePayment";
            advancePaymentDetail.setSelectedPatientInfo();
            advancePaymentDetail.LoadPatientAdvancePayment();
            advancePaymentDetail.BindFacility();



        });

    },

    InitializeControl: function () {

        //initialization of date-pickers.
        //utility.CreateDatePicker('pnlPatientAdvancePayment #dtpDatePaid', function () {
        //    //on-change callback method
        //    $('#frmPatientAdvancePayment').bootstrapValidator('revalidateField', 'DatePaid');
        //});
        //initialization of date-pickers.
        utility.CreateDatePicker('pnlPatientAdvancePayment #dtpEntryDate', function () {
            //on-change callback method
            //  $('#frmPatientAdvancePayment').bootstrapValidator('revalidateField', 'EntryDate');
        });
        //initialization of date-pickers.
        utility.CreateDatePicker('pnlPatientAdvancePayment #dtpChequeDate', function () {
            //on-change callback method

           // $('#frmPatientAdvancePayment').bootstrapValidator('revalidateField', 'checkDate');
        });

        utility.CreateDatePicker('pnlPatientAdvancePayment #dtpExpiryDate', function () {
            //on-change callback method

            //$('#frmPatientAdvancePayment').bootstrapValidator('revalidateField', 'expiryDate');
        });
        //utility.CreateDatePicker('pnlPatientAdvancePayment #dtpExpiryDate', function () {
        //    //on-change callback method


        //}, true);
        //initialization of date-pickers.

        //utility.CreateDatePicker('pnlPatientAdvancePayment #dtpExpiryDate', function () {
        //    //on-change callback method
        //    $('#frmPatientAdvancePayment').bootstrapValidator('revalidateField', 'ExpiryDate');
        //});

        utility.creditCardExpiryDate('frmPatientAdvancePayment #dtpExpiryDate', function () {

            // on-change callback method
            //$('#frmPatientAdvancePayment').bootstrapValidator('revalidateField', 'expiryDate');
        });
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'advancePaymentDetail';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#pnlPatientAdvancePayment #hfPatientId").val(PatientId);
        $("#pnlPatientAdvancePayment #txtPatientName").val(patFullName.split(" - ")[0]);
        $("#pnlPatientAdvancePayment #txtFullName").val(patFullName.split(" - ")[1]);

        UnloadActionPan("advancePaymentDetail");
        utility.InsertRecentPatient(PatientId);
    },

    BindPatientAccount: function () {
        var AccountNo = $('#' + advancePaymentDetail.params.PanelID + ' #txtPatientName').val();
        var AllPatients = utility.GetPatientArray(AccountNo, 1).done(function (response) {

            $("#pnlPatientAdvancePayment #txtPatientName").autocomplete({
                autoFocus: true,
                //source: AllPatients, // pass an array (without a comma)
                source: response,

                select: function (event, ui) {

                    //$("#appointmentDetail #txtAccountNo").val(ui.item.id); // add the selected id
                    //$("#appointmentDetail #txtFullName").val(ui.item.patientName);
                    setTimeout(function () {
                        $("#pnlPatientAdvancePayment #hfPatientId").val(ui.item.id);
                        $("#pnlPatientAdvancePayment #txtPatientName").val(ui.item.value.split(" ")[0]);
                        $("#pnlPatientAdvancePayment #txtFullName").val(ui.item.value.split(" ")[2]);
                        utility.InsertRecentPatient(ui.item.id);
                    }, 100);

                    //$("#hfpatientid").val(ui.item.id);
                }
            });

            $("#pnlPatientAdvancePayment #txtPatientName").autocomplete("search");
            $("#pnlPatientAdvancePayment #txtPatientName").focus();
        });

    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmPatientAdvancePayment";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "advancePaymentDetail";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {

        var params = [];
        params["FacilityId"] = $('#' + advancePaymentDetail.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'advancePaymentDetail';
        LoadActionPan('facilityDetail', params);
    },
    UnLoad: function (Tab) {


        utility.UnLoadDialog('frmPatientAdvancePayment', function () {

            if (advancePaymentDetail.params.FromAdmin != "1") {
                if (advancePaymentDetail.params != null && advancePaymentDetail.params.ParentCtrl != null) {
                    UnloadActionPan(advancePaymentDetail.params.ParentCtrl, 'advancePaymentDetail');
                }
                else
                    UnloadActionPan(null, 'advancePaymentDetail');
            }
            else
                RemoveAdminTab(Tab);

        }, function () {
            UnloadActionPan(advancePaymentDetail.params.ParentCtrl, 'advancePaymentDetail');
        });
    },

    ValidateAdvancePayment: function () {
        $('#frmPatientAdvancePayment')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  PatientName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Facility: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },

                  PaymentType: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DatePaid: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Paid: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },

                  LedgerAccount: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },

                  // Begin: PMS 770- New jira - Billing requirement, Abdur Rehman
                  //checkNumber: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},

                  //checkDate: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},

                  //creditCardType: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},
                  //cardNumber: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},
                  //expiryDate: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},

                  // End: PMS 770- New jira - Billing requirement, Abdur Rehman

              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           advancePaymentDetail.saveAdvancePayment();
       });
    },

    saveAdvancePayment: function () {



        $(advancePaymentDetail.panelID + " #hfPatientId").val("");
        var strMessage = "";
        var self = $('#pnlPatientAdvancePayment');
        var myJSON = self.getMyJSON();
        if (advancePaymentDetail.params.mode.toLowerCase() == "add") {
            AppPrivileges.GetFormPrivileges("Advance Payment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    advancePaymentDetail.AdvancePaymentSave(myJSON).done(function (response) {
                        if (response.status != false) {
                            advancePaymentDetail.params.patientID = response.patientID;
                            utility.DisplayMessages(response.message, 1);
                            Patient_AdvancePayment.SearchAdvancePayment();

                            //update the patient balances in patient banner
                            Patient_Demographic.UpdateBalancesInBanner();

                            if (advancePaymentDetail.params != null && advancePaymentDetail.params.ParentCtrl != null) {
                                UnloadActionPan(advancePaymentDetail.params.ParentCtrl, 'advancePaymentDetail');
                            }
                            else
                                UnloadActionPan(null, 'advancePaymentDetail');
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
        else if (advancePaymentDetail.params.mode.toLowerCase() == "edit") {
            AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    advancePaymentDetail.AdvancePaymentUpdate(myJSON, advancePaymentDetail.params.AdvancePaymentID).done(function (response) {
                        if (response.status != false) {

                            utility.DisplayMessages(response.message, 1);
                            Patient_AdvancePayment.SearchAdvancePayment();
                            UnloadActionPan(advancePaymentDetail.params["ParentCtrl"]);
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
    },

    BindFacility: function () {
        var Ctrl = $("#" + advancePaymentDetail.params.PanelID + " #frmPatientAdvancePayment #txtFacility");
        var hfCtrl = $("#" + advancePaymentDetail.params.PanelID + " #frmPatientAdvancePayment #hfFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    AdvancePaymentSave: function (AdvancePaymentData) {
        var data = "AdvancePaymentData=" + AdvancePaymentData + "&PatientID=" + advancePaymentDetail.params.PatientId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ADVANCE_PAYMENT", "SAVE_PATIENT_ADVANCE_PAYMENT");
    },

    AdvancePaymentUpdate: function (AdvancePaymentData) {
        var data = "AdvancePaymentData=" + AdvancePaymentData + "&AdvancePaymentID=" + advancePaymentDetail.params.AdvancePaymentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ADVANCE_PAYMENT", "UPDATE_PATIENT_ADVANCE_PAYMENT");
    },

    fillLedgerAccount: function () {

        CacheManager.BindDropDownsByMultipleIDs('#pnlPatientAdvancePayment #ddlLedgerAccount', 'GetAdvancePaymentLedgerAccount', true, 1, 1, 10).done(function () {

            $("#pnlPatientAdvancePayment #ddlLedgerAccount").val('7').trigger('change');

        });

    },

    PaymentTypeChanged: function () {


        var v = $('#pnlPatientAdvancePayment #ddlPaymentType option:selected').text();

        if (v == "Check") {

            $('#pnlPatientAdvancePayment #cardDiv').hide();
            $('#pnlPatientAdvancePayment #chequeDiv').show();


        }
        else if (v == "Credit Card") {
            $('#pnlPatientAdvancePayment #cardDiv').show();
            $('#pnlPatientAdvancePayment #chequeDiv').hide();
        }
        else if (v == "") { }
        else {
            $('#pnlPatientAdvancePayment #cardDiv').hide();
            $('#pnlPatientAdvancePayment #chequeDiv').hide();
        }
    },

    LoadPatientAdvancePayment: function () {

        AppPrivileges.GetFormPrivileges("Advance Payment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (advancePaymentDetail.params.mode == "Add") {

                    //advancePaymentDetail.setSelectedPatientInfo();
                    $('#pnlPatientAdvancePayment #txtFacility').val(globalAppdata['DefaultFacilityName']);
                    $('#pnlPatientAdvancePayment #hfFacility').val(globalAppdata['DefaultFacilityId']);

                    //start syed zia 02-02-2016,bug #PMS-3800
                    // $('#pnlPatientAdvancePayment #txtPatientName').val(advancePaymentDetail.params['PatientName']);

                    //end syed zia 02-02-2016,bug #PMS-3800
                    $('#pnlPatientAdvancePayment #hfPatientId').val(advancePaymentDetail.params["patientID"]);

                    if ($("#pnlPatientAdvancePayment #lnkFacilityEdit").css("display") == "none" && $('#pnlPatientAdvancePayment #txtFacility').val() != "") {
                        $("#pnlPatientAdvancePayment #lnkFacilityEdit").css("display", "inline");
                        $("#pnlPatientAdvancePayment #lblFacility").css("display", "none");
                    }

                    $Ctrl = $("#" + advancePaymentDetail.params.PanelID + " #frmPatientAdvancePayment #txtFacility");
                    $hfCtrl = $("#" + advancePaymentDetail.params.PanelID + " #frmPatientAdvancePayment #hfFacility");
                    //Facility
                    utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());

                    //serialize data
                    $('#frmPatientAdvancePayment').data('serialize', $('#frmPatientAdvancePayment').serialize());
                }
                else if (advancePaymentDetail.params.mode == "Edit") {
                    advancePaymentDetail.DocumentSearch();
                    advancePaymentDetail.FillAdvancePayment(advancePaymentDetail.params.AdvancePaymentID).done(function (response) {
                        if (response.status != false) {
                            var self = $('#pnlPatientAdvancePayment');

                            var parsedJson = JSON.parse(response.AdvancePaymentFill_JSON);

                            params["PatientId"] = parsedJson.hfPatientId;

                            /*****************/
                            $('#pnlPatientAdvancePayment #frmPatientAdvancePayment #btnSave').hide();
                            //start syed zia, bug PMS-4383
                            $('#pnlPatientAdvancePayment #frmPatientAdvancePayment :input:text').attr('disabled', true)
                            $('#pnlPatientAdvancePayment #frmPatientAdvancePayment textarea').attr('disabled', true)
                            //end syed zia, bug PMS-4383
                            $('#pnlPatientAdvancePayment #frmPatientAdvancePayment select').attr('disabled', true)
                            $('#pnlPatientAdvancePayment #frmPatientAdvancePayment :button').hide();
                            /****************/

                            if (parsedJson.isRefund.toLowerCase() != "true") {
                                $('#pnlPatientAdvancePayment #btnBrowse').show();
                                $('#pnlPatientAdvancePayment #btnScan').show();
                            }

                            utility.bindMyJSON(true, parsedJson, false, self).done(function () {

                                advancePaymentDetail.PaymentTypeChanged();

                                //serialize data
                                $('#frmPatientAdvancePayment').data('serialize', $('#frmPatientAdvancePayment').serialize());
                            });

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    },
    FillAdvancePayment: function (paymentId) {
        var data = "AdvancePaymentId=" + paymentId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ADVANCE_PAYMENT", "FILL_PATIENT_ADVANCE_PAYMENT");

    },

    DocumentImport: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Advance Payment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];


                params["patientId"] = advancePaymentDetail.params["PatientId"];
                params["RefCtrl"] = "advancePayment";
                params['AdvancePaymentId'] = advancePaymentDetail.params.AdvancePaymentID;
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["ParentCtrl"] = 'advancePaymentDetail';
                params["PatientName"] = advancePaymentDetail.params.PatientName;
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    DocumentScan: function () {
        AppPrivileges.GetFormPrivileges("Advance Payment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["mode"] = "Scan";
                param["RefCtrl"] = "advancePayment";
                param['AdvancePaymentId'] = advancePaymentDetail.params.AdvancePaymentID;
                param['patientID'] = advancePaymentDetail.params['PatientId'];
                param["ParentCtrl"] = 'advancePaymentDetail';
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    DocumentSearch: function (DocumentId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Advance Payment", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlPatientAdvancePayment #pnlPatientDocument_Result").css("display") == "none") {
                    $("#pnlPatientAdvancePayment #pnlPatientDocument_Result").show();
                }

                var self = $("#pnlPatientAdvancePayment");
                var myJSON = "";//self.getMyJSON();

                advancePaymentDetail.SearchDocument(myJSON, advancePaymentDetail.params.PatientId, advancePaymentDetail.params.AdvancePaymentID).done(function (response) {
                    if (response.status != false) {
                        advancePaymentDetail.DocumentGridLoad(response);
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

    SearchDocument: function (DocumentData, PatientID, AdvancePaymentID) {
        if (PatientID == null) {
            PatientID = 0;
        }

        if (AdvancePaymentID == null) {
            AdvancePaymentID = 0;
        }

        var data = "PatientDocumentData=" + DocumentData + "&PatientID=" + PatientID + "&AdvancePaymentID=" + AdvancePaymentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SEARCH_PATIENT_DOCUMENT");
    },


    DocumentGridLoad: function (response) {
        $("#" + advancePaymentDetail.params["PanelID"] + " #dgvAdvancePaymentDocument").dataTable().fnDestroy();

        $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocument tbody").find("tr").remove();

        // var PendingDivHTML = $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result div#Pending").html();


        $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocument thead").find("#threviewd").css("display", "none");
        $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocument thead").find("#threviewddate").css("display", "none");
        $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocument thead").find("#thsigned").css("display", "none");




        if (response.DocumentCount > 0) {
            var DocumentLoad_JSONData = JSON.parse(response.DocumentLoad_JSON);
            $.each(DocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvAdvancePaymentDocument_row" + item.PatDocId + "'))");
                $row.attr("id", "dgvAdvancePaymentDocument_row" + item.PatDocId);
                $row.attr("DocumentId", item.PatDocId);

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

                var selectDocument = "";
                var dos = utility.RemoveTimeFromDate(null, item.DOS) != null ? utility.RemoveTimeFromDate(null, item.DOS) : '';
                //if (item.ReviewDate == "") {

                //Pending
                $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><a class="btn  btn-xs" href="#" onclick="advancePaymentDetail.DocumentDelete(' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="advancePaymentDetail.DocumentEdit(' + item.PatientId + '  ,  ' + item.PatDocId + '   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="advancePaymentDetail.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectDocument + '</td><td>' + item.DocumentName + '</td><td>' + dos + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td style="display:none" >' + item.ReviewBy + '</td><td style="display:none" >' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td style="display:none">' + item.SignBy + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');

                $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocument tbody").last().append($row);
                //   }
                //else {

                //    //Reviewed
                //    $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" /></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="advancePaymentDetail.DocumentEdit(' + item.PatientId + '  ,  ' + item.PatDocId + '   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectDocument + '</td><td>' + item.DocumentName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td>' + item.Comments + '</td>');

                //    $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocumentReviewed tbody").last().append($row);
                //}


            });
            var pendingRows = $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocument tbody").find("tr");
            //  var ReviewedRows = $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocumentReviewed tbody").find("tr");
            if (pendingRows.length < 1) {
                $('#' + advancePaymentDetail.params["PanelID"] + ' #dgvAdvancePaymentDocument').DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }

        }
        else {
            $('#' + advancePaymentDetail.params["PanelID"] + ' #dgvAdvancePaymentDocument').DataTable({
                "language": {
                    "emptyTable": "No Documents Found for this Advance Payment"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#' + advancePaymentDetail.params["PanelID"] + ' #dgvAdvancePaymentDocument'));
        else
            $("#" + advancePaymentDetail.params["PanelID"] + " #pnlPatientDocument_Result #dgvAdvancePaymentDocument").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },


    DocumentEdit: function (PatientID, PatDocID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var pparams = [];
                pparams["PatientID"] = PatientID;
                pparams["PatDocID"] = PatDocID;
                pparams["mode"] = "Edit";
                pparams["FromAdmin"] = "0";
                pparams["ParentCtrl"] = 'advancePaymentDetail';
                pparams["PanelID"] = advancePaymentDetail.params["PanelID"]
                LoadActionPan('Document_Viewer', pparams);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    DocumentDelete: function (DocumentId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Advance Payment", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = DocumentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        advancePaymentDetail.DeleteDocument(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + advancePaymentDetail.params["PanelID"] + ' #dgvFacility').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                advancePaymentDetail.DocumentSearch();
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

    DeleteDocument: function (DocumentID) {
        var data = "DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DELETE_PATIENT_DOCUMENT");
    },


    ActiveInactivePatientDocument: function (PatientDocumentId, IsActive) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PatientDocumentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        advancePaymentDetail.UpdateDocumentActiveInactive(advancePaymentDetail.params.patientID, selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                advancePaymentDetail.DocumentSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                     '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },



    UpdateDocument: function (PatientID, DocumentID) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT");
    },

    UpdateDocumentActiveInactive: function (PatientID, DocumentID, IsActive) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "DocumentID=" + DocumentID + "&IsActive=" + IsActive + "&PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT_ACTIVE_INACTIVE");
    },


    setSelectedPatientInfo: function () {

        var AccountNo = $("#PatientProfile #hfAccountNo").val();
        var PatientFullName = $("#PatientProfile #hfPatientFullName").val();
        var PatientId = $("#PatientProfile #hfPatientId").val();

        if (advancePaymentDetail.params.IsPatientDetail == "1") {
            $('#pnlPatientAdvancePayment #txtPatientName').val(advancePaymentDetail.params.PatAccountNumber);
            $('#pnlPatientAdvancePayment #txtFullName').val(advancePaymentDetail.params.PatLastName + " " + advancePaymentDetail.params.PatFirstName + " " + advancePaymentDetail.params.PatMidInitial);
            if (advancePaymentDetail.params.patientID) {
                $('#pnlPatientAdvancePayment #hfPatientId').val(advancePaymentDetail.params.patientID);
            }
        } else {
            if (AccountNo.length > 0) {
                $('#pnlPatientAdvancePayment #txtPatientName').val(AccountNo);
            }

            if (PatientFullName.length > 0) {
                Firstname = PatientFullName.split(" ")[1];
                Lastname = PatientFullName.split(" ")[0];
                MiddleInitial = PatientFullName.split(" ")[2];

                $('#pnlPatientAdvancePayment #txtFullName').val(Lastname + " " + Firstname + " " + MiddleInitial);

            }

            if (PatientId.length > 0) {
                $('#pnlPatientAdvancePayment #hfPatientId').val(PatientId);
            }
        }


    },
}