Bill_FollowUpInsuranceAR_Detail = {
    bIsFirstLoad: true,
    params: [],
    PaymentPostingParams:[],
    Load: function (params) {
        Bill_FollowUpInsuranceAR_Detail.PaymentPostingParams = null;
        Bill_FollowUpInsuranceAR_Detail.params = params;
        if (Bill_FollowUpInsuranceAR_Detail.params.PanelID != "pnlBillFollowUpInsuranceARDetail")
            Bill_FollowUpInsuranceAR_Detail.params.PanelID = Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #pnlBillFollowUpInsuranceARDetail';

        if (Bill_FollowUpInsuranceAR_Detail.bIsFirstLoad) {
            Bill_FollowUpInsuranceAR_Detail.bIsFirstLoad = false;
            if (Bill_FollowUpInsuranceAR_Detail.params["FromDenial"] == "true" && Bill_PaymentPosting.params != null) {
                Bill_FollowUpInsuranceAR_Detail.PaymentPostingParams = Bill_PaymentPosting.params;
            }

            utility.CreateDatePicker(Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #frmFollowUpInsuranceARDetail #dtpSuspendedDate');

            $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID).loadDropDowns(true).done(function () {

                Bill_FollowUpInsuranceAR_Detail.LoadFollowUpInsuranceARDetail();
                Bill_FollowUpInsuranceAR_Detail.LoadARCharges(Bill_FollowUpInsuranceAR_Detail.params.VisitId);
                Bill_FollowUpInsuranceAR_Detail.ValidateFollowUpInsuranceAR();
                Bill_FollowUpInsuranceAR_Detail.FollowUpCommentLoad(Bill_FollowUpInsuranceAR_Detail.params.VisitId);

                $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #frmFollowUpInsuranceARDetail').data('serialize', $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #frmFollowUpInsuranceARDetail').serialize());
            });
        }
    },



    ValidateFollowUpInsuranceAR: function () {
        $('#frmFollowUpInsuranceARDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  //GroupID: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},


                  ActionID: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },


                  //Reason: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},

                  //RemitanceCode: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},


              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Bill_FollowUpInsuranceAR_Detail.saveFollowUpInsuranceAR();
       });
    },



    saveFollowUpInsuranceAR: function (showMessage) {

        var strMessage = "";
        var self = $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID);

       
        // set  flag for Update Comments
        if ($("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #txtComments").text() != $("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #hftxtComments").val()) {
            $("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #hfIsCommnetsChanged").val("true");
        }
        else {
            $("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #hfIsCommnetsChanged").val("false");
        }
       // $("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #txtComments").val($("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #txtComments").text())
        var myJSON = self.getMyJSONByName();
        

        if (Bill_FollowUpInsuranceAR_Detail.params.mode.toLowerCase() == "add") {
            AppPrivileges.GetFormPrivileges("Advance Payment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    //
                    Bill_FollowUpInsuranceAR_Detail.FollowUpCommentLoad(Bill_FollowUpInsuranceAR_Detail.params.VisitId);
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Bill_FollowUpInsuranceAR_Detail.params.mode.toLowerCase() == "edit") {
            AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Bill_FollowUpInsuranceAR_Detail.followUpInsuranceARUpdate(myJSON, Bill_FollowUpInsuranceAR_Detail.params.followUpInsuranceARID).done(function (response) {
                        if (response.status != false) {

                            if (showMessage != null && showMessage == false) {
                                ;
                            } else {

                                utility.DisplayMessages(response.message, 1);
                            }
                            //Bill_FollowUpInsuranceAR_Detail.SearchAdvancePayment();
                            //UnloadActionPan(Bill_FollowUpInsuranceAR_Detail.params["ParentCtrl"]);

                            Bill_FollowUpInsuranceAR.ARSearch("Insurance");
                            $('#frmFollowUpInsuranceARDetail').data('serialize', $('#frmFollowUpInsuranceARDetail').serialize());
                            $('#frmFollowUpInsuranceARDetail #txtComments').text("");
                            Bill_FollowUpInsuranceAR_Detail.FollowUpCommentLoad(Bill_FollowUpInsuranceAR_Detail.params.VisitId);
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

    followUpInsuranceARUpdate: function (followUpInsuranceARData) {

        var objData = JSON.parse(followUpInsuranceARData);
        objData["FollowUpInsuranceARID"] = Bill_FollowUpInsuranceAR_Detail.params.FollowUpInsuranceARID;
        objData["GroupName"] =  $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #ddlGroup option:selected").text();
        objData["ActionName"] = $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #ddlAction option:selected").text();
        objData["ReasonName"] = $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #ddlReason option:selected").text();
        objData["Comments"] =   $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #txtComments").text()
        objData["RemitanceCodeText"] = $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #ddlRemitanceCode option:selected").text();
        objData["hfIsCommnetsChanged"] = $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #hfIsCommnetsChanged").val();
        objData["CommandType"] = "update_followup_insurance_ar";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "FollowUps", "FollowUpInsuranceARDetail");

    },



    PrintPatientLetter: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ARDetailId"] = "-1";
        params["VisitId"] = "-1";
        params["ParentCtrl"] = 'Bill_FollowUpInsuranceAR_Detail';
        LoadActionPan('designLetterPrinting', params);
    },

    LoadPaymentPosting: function (ChargeCapId, VisitId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_FollowUpInsuranceAR_Detail';
                params["ARDetailId"] = "-1";
                params["ChargeId"] = ChargeCapId;
                params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
                params["PaymentRef"] = "pnlBillFollowUpInsuranceARDetail";

                LoadActionPan('Bill_PaymentPosting', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenInsuranceARCall: function () {
        var params = [];
        params["ParentCtrl"] = "Bill_FollowUpInsuranceAR_Detail";
        params["FollowUpARID"] = Bill_FollowUpInsuranceAR_Detail.params.FollowUpInsuranceARID;
        params["FollowUpARType"] = "insurance";
        params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
        LoadActionPan('Bill_FollowUpARCall', params);

    },

    OpenInsuranceARHistory: function () {
        var params = [];
        params["ParentCtrl"] = "Bill_FollowUpInsuranceAR_Detail";
        params["ARDetailId"] = "-1";
        params["FollowUpARType"] = "insurance";
        params["FollowUpARID"] = Bill_FollowUpInsuranceAR_Detail.params.FollowUpInsuranceARID;
        params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
        LoadActionPan('Bill_FollowUpARHistory', params);
    },

    OpenInsuranceARSplitClaim: function () {
        var params = [];
        params["ParentCtrl"] = "Bill_FollowUpInsuranceAR_Detail";
        params["ARDetailId"] = "-1";
        params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
        LoadActionPan('Bill_FollowUpClaimSplit', params);

    },

    OpenPatientEligibility: function () {
        var params = [];
        params["FromAdmin"] = "0";

        params["patientID"] = Bill_FollowUpInsuranceAR_Detail.params.patientID;
        params["patientAccount"] = Bill_FollowUpInsuranceAR_Detail.params.patientAccount;
        params["patientLastName"] = Bill_FollowUpInsuranceAR_Detail.params.patientLastName;
        params["patientFirstName"] = Bill_FollowUpInsuranceAR_Detail.params.patientFirstName;
        params["patientInsurancePlanId"] = Bill_FollowUpInsuranceAR_Detail.params.patientInsurancePlanId;
        params["Provider"] = Bill_FollowUpInsuranceAR_Detail.params.Provider;
        params["ProviderId"] = Bill_FollowUpInsuranceAR_Detail.params.ProviderId;

        params["ParentCtrl"] = "Bill_FollowUpInsuranceAR_Detail";
        params["ARDetailId"] = "-1";
        params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;



        LoadActionPan('Patient_Eligibility', params);

    },


    OpenPatientDemographics: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["patientID"] = $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #hfPatientId').val();
                params["FromAdmin"] = "0";
                params["PatBanner"] = true;
                params["IsFill"] = false;
                params["ParentCtrl"] = "Bill_FollowUpInsuranceAR_Detail";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },


    UnLoad: function () {
        ////in case of multiple opening of same screen,after closing of first screen params becomes empty so thats why set param again in case of null json
        //var self = $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID);
        //var myJSON = self.getMyJSON();
        //if (myJSON == "{}") {
        //    Bill_FollowUpInsuranceAR_Detail.params.PanelID = "pnlBillFollowUpInsuranceAR #pnlBillFollowUpInsuranceARDetail";
        //    Bill_FollowUpInsuranceAR_Detail.params["ParentCtrl"] = "billTabFollowUpInsuranceAR";
        //}

        utility.UnLoadDialog(Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #frmFollowUpInsuranceARDetail', function () {
            UnloadActionPan(Bill_FollowUpInsuranceAR_Detail.params["ParentCtrl"], "Bill_FollowUpInsuranceAR_Detail");
        }, function () {
            UnloadActionPan(Bill_FollowUpInsuranceAR_Detail.params["ParentCtrl"], "Bill_FollowUpInsuranceAR_Detail");
        });
        //start syed zia PMS-4147, add check to handle payment posting screen panel id null issue in case of unload tab.
        if (Bill_FollowUpInsuranceAR_Detail.params["FromDenial"] == "true" && Bill_PaymentPosting.params == null)
        {
            Bill_PaymentPosting.params = Bill_FollowUpInsuranceAR_Detail.PaymentPostingParams;
         
        }
        else if (Bill_FollowUpInsuranceAR_Detail.params["FromDenial"] == "true")
        {
            //in case of paymentposting denial refresh ledger entries grid 
            Bill_PaymentPosting.LoadChargesPaidPayment(Bill_FollowUpInsuranceAR_Detail.params["ChargeId"]);
        }
        
        //end syed zia PMS-4147, add check to handle payment posting screen panel id null issue in case of unload tab.
    },


    /*****AR DETAILS******/

    LoadFollowUpInsuranceARDetail: function () {


        if (Bill_FollowUpInsuranceAR_Detail.params.mode.toLowerCase() == "add") {

            //serialize Data.
            $('#frmFollowUpInsuranceARDetail').data('serialize', $('#frmFollowUpInsuranceARDetail').serialize());

        }
        else if (Bill_FollowUpInsuranceAR_Detail.params.mode.toLowerCase() == "edit") {



            Bill_FollowUpInsuranceAR_Detail.FillFollowUpInsuranceARDetail(Bill_FollowUpInsuranceAR_Detail.params.FollowUpInsuranceARID, Bill_FollowUpInsuranceAR_Detail.params.patientID, Bill_FollowUpInsuranceAR_Detail.params.VisitId).done(function (response) {

                if (response.status != false) {

                    var self = $("#" + Bill_FollowUpInsuranceAR_Detail.params.PanelID);
                    var FollowUpInsuranceARDetailFill_JSON = JSON.parse(response.FollowUpInsuranceARDetailFill_JSON);
                    utility.bindMyJSONByName(true, FollowUpInsuranceARDetailFill_JSON, false, self).done(function () {

                        //serialize Data.
                        $('#frmFollowUpInsuranceARDetail').data('serialize', $('#frmFollowUpInsuranceARDetail').serialize());
                        // set html 
                        //$("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #txtComments").html(FollowUpInsuranceARDetailFill_JSON.Comments);
                       // $("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #hftxtComments").val($("#pnlBillFollowUpInsuranceARDetail #frmFollowUpInsuranceARDetail #txtComments").text());


                    });

                }
                else {
                    UnloadActionPan();
                    utility.DisplayMessages(response.Message, 3);
                }

                Bill_FollowUpInsuranceAR_Detail.enableEditLinks();

            });

        }
    },

    FillFollowUpInsuranceARDetail: function (FollowUpInsuranceARID, PatientID, VisitId) {

        var objData = new Object();
        objData["FollowUpInsuranceARID"] = FollowUpInsuranceARID;
        objData["PatientID"] = PatientID;
        objData["VisitID"] = VisitId;
        objData["CommandType"] = "fill_followup_insurance_ar";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "FollowUps", "FollowUpInsuranceARDetail");


    },

    /*****AR DETAILS END******/


    /*****CHARGES******/



    LoadARCharges: function (VisitId, PageNo, rpp, RowId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Encounter", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + " #pnlBillFollowUpInsuranceARDetail_Result").css("display") == "none") {
                    $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + " #pnlBillFollowUpInsuranceARDetail_Result").show();
                }

                //var self = $("#" + Bill_PaymentPosting.params["PanelID"]);
                //var myJSON = self.getMyJSON();

                Bill_FollowUpInsuranceAR_Detail.SearchCharges("", VisitId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.ChargeCount > 0) {


                            //claims can only be split when charges are greate than 1
                            if (response.ChargeCount > 1) {
                                // claim can be split

                                $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + " #aClaimSplit").removeClass("disableAll");
                            }
                            else {
                                // claim can't be split

                                $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + " #aClaimSplit").addClass("disableAll");
                            }

                            //------------Pagination-----------
                            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #divBillFollowUpInsuranceARDetailPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 5;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            //params["myJSON"] = myJSON;


                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            utility.GetCustomPaging("divBillFollowUpInsuranceARDetailPaging", response.iTotalDisplayRecords, 5, "Bill_FollowUpInsuranceAR_Detail", CurrentPage, RecordsPerPage);
                            //}
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #divBillFollowUpInsuranceARDetailPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else

                                    $(this).removeAttr("class");
                            });
                            //------------End Pagination-------
                        }
                        else {
                            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #divBillFollowUpInsuranceARDetailPaging").css("display", "none");
                        }
                        Bill_FollowUpInsuranceAR_Detail.ChargesGridLoad(response, RowId);
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

    ChargesGridLoad: function (response, RowId) {
        $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail").dataTable().fnDestroy();
        $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail tbody").find("tr").remove();
        if (response.ChargeCount > 0) {//response.ChargeCount
            var ChargeLoadJSONData = JSON.parse(response.ChargeLoad_JSON);
            $.each(ChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvBillFollowUpInsuranceARDetail_row" + item.ChargeCapId + "'));");
                $row.attr("id", "gvBillFollowUpInsuranceARDetail_row" + item.ChargeCapId);
                $row.attr("ChargeId", item.ChargeCapId);

                //if (item.IsActive == "True") {
                //    isactive = 0;
                //    activeRecord = "Active Record";
                //    tglclass = "fa fa-toggle-on green";
                //}
                //else {
                //    isactive = 1;
                //    activeRecord = "Inactive Record";
                //    tglclass = "fa fa-toggle-on red";
                //}

                //var MethodMode = "";
                //var ActionBit = false;

                //var EditMethod = "Bill_PaymentPosting.ChargePaymentAdd(" + item.ChargeCapId.trim() + ",'Add','" + item.PatientId + "','" + item.PatientInsuranceId + "','" + item.InsurancePlanName + "','" + item.VisitId + "','" + item.FacilityId + "','" + item.FacilityName + "','" + item.ProviderId + "','" + item.ProviderName + "','" + item.TotalBal + "','" + item.InsCharges + "','" + item.PatCharges + "','" + item.Copay + "','" + item.InsBalance + "','" + item.PatBalance + "','" + item.CopayBalance + "','" + item.Fee + "');";

                //var ActiveInacvtiveMethod = "";//"Patient_Search.ActiveInactivePatient(" + item.PatientId.trim() + "," + isactive + ");";
                //var strAction = "";
                //strAction = '<a class="btn btn-xs" href="#sectionChargeDetail" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs mr-xs" href="#sectionChargeDetail" onclick="' + ActiveInacvtiveMethod + '" title="' + activeRecord + '"><i class="' + tglclass + '"></i></a>';

                // $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + strAction + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + item.ClaimNumber + '</td><td>' + item.CPTCode + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.Fee + '</td><td>' + item.TotalBal + '</td><td>0</td><td>0</td><td>0</td><td>' + item.PatChargeAmt + '</td><td>0</td><td>0</td><td>0</td><td>' + item.Copay + '</td><td>0</td><td>0</td><td>0</td><td></td>');
                //start syed zia 09-02-2016, bug # PMS-3840,correct copay paid parameter
                $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td>' + '<td><a href="#" onclick="Bill_FollowUpInsuranceAR_Detail.LoadVisitDetail();"  title="View Claim Detail">' + item.ClaimNumber + '</a></td>' + '<td title="' + item.CPTDescription + '" data-toggle="tooltip" data-placement="right">' + item.CPTCode + '</td><td>' + item.InsurancePlanName + '</td><td class="text-right">' + utility.convertToFigure(item.Fee, true) + '</td><td>' + utility.convertToFigure(item.Units) + '</td><td class="text-right">' + utility.convertToFigure(parseFloat(item.Units) * parseFloat(item.Fee), true) + '</td><td class="text-right">' + utility.convertToFigure(item.TotalBal, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsWriteOff, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatDiscount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatBalance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayDiscount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayBalance, true) + '</td>');
                //start syed zia 09-02-2016, bug # PMS-3840
                $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail tbody").last().append($row);
            });
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        }
        else {
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #divBillFollowUpInsuranceARDetailPaging").css("display", "none");
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail").DataTable({
                "language": {
                    "emptyTable": "No Charges Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail"))
            ;
        else
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 5, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        if (Bill_FollowUpInsuranceAR_Detail.params.ChargeId != null) {
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail #gvBillFollowUpInsuranceARDetail_row" + Bill_FollowUpInsuranceAR_Detail.params.ChargeId).trigger("click");
        }
        else if (RowId != null) {
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail tbody tr#" + RowId).trigger("click");
        }
        else
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #pnlBillFollowUpInsuranceARDetail_Result #dgvBillFollowUpInsuranceARDetail tbody tr:eq(0) ").click();




    },



    SearchCharges: function (ChargeData, VisitId, PageNumber, RowsPerPage) {

        var objData = new JSON.constructor();
        if (ChargeData) {
            objData = JSON.parse(ChargeData);
        }

        if (VisitId == null) {
            VisitId = 0;
        }

        var ChargeId = 0;
        //if (ChargeId == null) {
        //    ChargeId = 0;
        //}
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 5;
        }


        Bill_FollowUpInsuranceAR_Detail.params.CurrentPageNo = PageNumber;

        objData["ChargeId"] = ChargeId;
        objData["VisitId"] = VisitId;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "search";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Payments", "PaymentPosting");
    },



    //---------------Pagination functions-----
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlBillFollowUpInsuranceARDetail_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        if (Bill_FollowUpInsuranceAR_Detail.params.VisitId != null)
            Bill_FollowUpInsuranceAR_Detail.LoadARCharges(Bill_FollowUpInsuranceAR_Detail.params.VisitId, PageNo, 5);
        else
            Bill_FollowUpInsuranceAR_Detail.LoadARCharges(0, PageNo, 5);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpInsuranceARDetail_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {

            if (Bill_FollowUpInsuranceAR_Detail.params.VisitId != null)
                Bill_FollowUpInsuranceAR_Detail.LoadARCharges(Bill_FollowUpInsuranceAR_Detail.params.VisitId, currentPageNo, 5);
            else
                Bill_FollowUpInsuranceAR_Detail.LoadARCharges(0, currentPageNo, 5);


        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpInsuranceARDetail_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {


            if (Bill_FollowUpInsuranceAR_Detail.params.VisitId != null)
                Bill_FollowUpInsuranceAR_Detail.LoadARCharges(Bill_FollowUpInsuranceAR_Detail.params.VisitId, currentPageNo, 5);
            else
                Bill_FollowUpInsuranceAR_Detail.LoadARCharges(0, currentPageNo, 5);

        }
    },


    /*****CHARGES END******/


    appendCallComments: function (callComments) {

        var objComments = $("#" + Bill_FollowUpInsuranceAR_Detail.params.PanelID + " #frmFollowUpInsuranceARDetail #txtComments");

        var seperator = "";

        if (objComments.val().trim() != "") {

            seperator = " | "
        }

        var callData = seperator + "Call: " + globalAppdata.AppUserName + " " + $.datepicker.formatDate(globalAppdata['DateFormat'].replace('yy', ''), new Date()) + "  " + callComments;

        objComments.val(objComments.val() + callData);

        Bill_FollowUpInsuranceAR_Detail.saveFollowUpInsuranceAR(false);
    },



    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_FollowUpInsuranceAR_Detail';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },

    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Bill_FollowUpInsuranceAR_Detail';
        LoadActionPan('providerDetail', params);
    },


    OpenPracticeDetail: function () {
        var params = [];
        params["PracticeId"] = $('#' + Bill_FollowUpInsuranceAR_Detail.params.PanelID + ' #hfPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPractice";
        params["ParentCtrl"] = 'Bill_FollowUpInsuranceAR_Detail';
        LoadActionPan('practiceDetail', params);
    },


    LoadVisitDetail: function () {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_FollowUpInsuranceAR_Detail';

                params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
                params["patientID"] = Bill_FollowUpInsuranceAR_Detail.params.patientID;

                LoadActionPan('EncounterChargeCapture', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    enableEditLinks: function () {


        var self = $("#" + Bill_FollowUpInsuranceAR_Detail.params.PanelID + " #frmFollowUpInsuranceARDetail");


        if (self.find("#hfProvider").val() != "") {
            self.find("#lnkProviderEdit").css("display", "inline");
            self.find("#lblProvider").css("display", "none");
        }

        if (self.find("#hfFacility").val() != "") {
            self.find("#lnkFacilityEdit").css("display", "inline");
            self.find("#lblFacility").css("display", "none");
        }


        if (self.find("#hfPractice").val() != "") {
            self.find("#lnkPracticeEdit").css("display", "inline");
            self.find("#lblPractice").css("display", "none");
        }

        if (self.find("#hfPatientId").val() != "") {
            self.find("#lnkPatientEdit").css("display", "inline");
            self.find("#lblPatient").css("display", "none");
        }

        if (self.find("#hfVisitId").val() != "") {
            self.find("#lnkVisitEdit").css("display", "inline");
            self.find("#lblVisit").css("display", "none");
        }
        if (self.find("#hfInsurancePlan").val() != "") {
            self.find("#lnkInsurancePlanDetail").css("display", "inline");
            self.find("#lblInsurancePlan").css("display", "none");
        }

    },
    OpenInsurancePlanDetail: function () {

        var params = [];
        params["InsurancePlanId"] = $("#" + Bill_FollowUpInsuranceAR_Detail.params.PanelID + " #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        params["ParentCtrl"] = 'Bill_FollowUpInsuranceAR_Detail';
        LoadActionPan('insurancePlanDetail', params);
    },
    FollowUpCommentLoad: function (VisitId) {
        Bill_FollowUpARComments.FollowUpCommentsSearch(0, VisitId).done(function (response) {
            Bill_FollowUpInsuranceAR_Detail.FollowUpCommentGridLoad(response);
        });
    },
    FollowUpCommentGridLoad: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment")) {
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment").dataTable().fnClearTable();
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment").dataTable().fnDestroy();
        }
        $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment tbody").empty();
        if (response.FollowUpCommentInfoCount > 0) {
            $.each(response.FollowUpCommentInfo, function (i, item) {
                var MethodMode = "Bill_FollowUpInsuranceAR_Detail.FillFollowUpComment(" + item.Id + ")";
                var $row = $('<tr/>');
                $row.attr("Id", item.Id);
                //$row.attr("onclick", MethodMode);
                $row.append('<td><a class="btn  btn-xs" href="#" onclick="Bill_FollowUpInsuranceAR_Detail.DeleteFollowUpComment(\'' + item.Id + '\',event);" title="Delete Record"> <i class="fa fa-close red"></i></a><a class="btn  btn-xs" href="#"  onclick="Bill_FollowUpInsuranceAR_Detail.FillFollowUpComment(' + item.Id + ');"></a><a class="btn btn-xs" onclick="' + MethodMode + '" href="#" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + item.followUpComments + '</td><td>' + item.ModifiedOn + '</td><td>' + item.ModifiedBy + '</td>');
                $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment tbody").last().append($row);
            });
        }
        else {

            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment").DataTable({
                "language": {
                    "emptyTable": "No Comment Found"
                }, "autoWidth": false, "bLengthChange": false, "bInfo": false, "searching": false, "bFilter": false, "bPaginate": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable($("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment")))
            ;
        else
            $("#" + Bill_FollowUpInsuranceAR_Detail.params["PanelID"] + " #dgvClaimFollowUpComment").DataTable({ "bInfo": false, "searching": false, "bPaginate": false, "bFilter": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": true, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    FillFollowUpComment: function (FollowUpId) {
        var params = [];
        params["mode"] = "Edit";
        params["IsFromClaim"] = false;
        params["FollowUpCommentId"] = FollowUpId;
        params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
        params["ParentCtrl"] = "Bill_FollowUpInsuranceAR_Detail";
        LoadActionPan("Bill_FollowUpARComments", params);
    },
    OpenFollowUpComments: function (isEdit) {
        var params = [];
        params["VisitId"] = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
        params["mode"] = "Add";
        params["IsFromClaim"] = false;
        params["ParentCtrl"] = "Bill_FollowUpInsuranceAR_Detail";
        params["FromAdmin"] = "0";
        LoadActionPan('Bill_FollowUpARComments', params);
    },
    DeleteFollowUpComment: function (Id, event) {
        event.stopPropagation();
        Bill_FollowUpARComments.params.VisitId = Bill_FollowUpInsuranceAR_Detail.params.VisitId;
        Bill_FollowUpARComments.params.ParentCtrl = "Bill_FollowUpInsuranceAR_Detail";
        Bill_FollowUpARComments.DeleteFollowUpComments(Id);

    },
}