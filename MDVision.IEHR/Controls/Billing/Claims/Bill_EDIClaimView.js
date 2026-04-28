EDIClaimViewDetail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        EDIClaimViewDetail.params = params;
        if (EDIClaimViewDetail.bIsFirstLoad) {
            EDIClaimViewDetail.bIsFirstLoad = false;
        }
        EDIClaimViewDetail.LoadEDIClaimViewDetail(EDIClaimViewDetail.params._837BatchId);
    },



    LoadEDIClaimViewDetail: function (_837BatchId) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        if ($("#EDIClaimViewDetail #pnlEDIClaimViewDetail_Result").css("display") == "none") {
            $("#EDIClaimViewDetail #pnlEDIClaimViewDetail_Result").show();
        }


        EDIClaimViewDetail.ClaimViewDetailLoad(_837BatchId).done(function (response) {
            if (response.status != false) {
                EDIClaimViewDetail.ClaimViewDetailGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    ClaimViewDetailGridLoad: function (response) {
        $("#dgvEDIClaimView").dataTable().fnDestroy();
        $("#EDIClaimViewDetail #pnlEDIClaimViewDetail_Result #dgvEDIClaimView tbody").find("tr").remove();
        if (response.EDIClaimDetailCount > 0) {
            var EDIClaimDetailJSONData = JSON.parse(response.EDIClaimDetail_JSON);
            var resubmitetd_count = 0;
            var regular_count = 0;
            $.each(EDIClaimDetailJSONData, function (i, item) {
                var $row = $('<tr/>');

                var actionattr = "";
                var actiontitle = "Select Claim";

                if (item.ClaimStatus == "Regular") {
                    regular_count++;
                    actionattr = "disabled";
                    actiontitle = "Claim is Regular.";
                }

                if (item.ClaimStatus == "ReSubmit") {
                    resubmitetd_count++;
                    actionattr = "disabled";
                    actiontitle = "Claim is already Resubmitted.";
                }

                $row.append('<td style="display:none;">' + item.VisitId + '</td><td><input type="checkbox" title="' + actiontitle + '"  ' + actionattr + '  onclick="EDIClaimViewDetail.CheckedClaims(this)" name="checkbox" id="' + item.VisitId + '"></td><td>' + item.DOS.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td><a href="#" onclick="EDIClaimViewDetail.LoadVisitDetail(' + item.VisitId.trim() + ',' + item.PatientId.trim() + ',event);"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td><a href="#" onclick="EDIClaimViewDetail.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="EDIClaimViewDetail.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.InsurancePlanName + '</td><td>' + item.SubscriberId + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td class="text-right">' + utility.convertToFigure(item.Fee, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td><td>' + item.ClaimStatus + '</td>');
                $("#pnlEDIClaimViewDetail_Result #dgvEDIClaimView tbody").last().append($row);

            });

            // disable controls if all claims are already submitted.
            if (resubmitetd_count == response.EDIClaimDetailCount || regular_count == response.EDIClaimDetailCount) {
                $("#pnlEDIClaimViewDetail_Result #chkAllClaims").attr("disabled", "disabled");
                $("#EDIClaimViewDetail #btnResubmit").addClass("disabled");
            }
        }
        else {

            $("#EDIClaimViewDetail #btnResubmit").addClass("disabled");

            $('#dgvEDIClaimView').DataTable({
                "language": {
                    "emptyTable": "No Claim Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvEDIClaimView'))
            ;
        else
            $("#pnlEDIClaimViewDetail_Result #dgvEDIClaimView").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        if ($("#EDIClaimViewDetail #hfActivePageNumber").val() != "") {
            $('#dgvEDIClaimView_paginate li').each(function () {
                if (this.firstChild.text == $("#EDIClaimViewDetail #hfActivePageNumber").val()) {
                    this.click();
                    //this.addClass("active");
                }
            });
        }

    },

    Claim_ReSubmit: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var claims = EDIClaimViewDetail.GetSelectedClaims();
                //if (claims.length > 0) {
                if (claims != "") {
                    EDIClaimViewDetail.UpdateClaim(claims).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            //fix against QAC1-193
                            $("#EDIClaimViewDetail #chkAllClaims").prop('checked', false);
                            $("#EDIClaimViewDetail #chkAllClaims").attr('title', 'Select all');
                            //if (EDIClaimViewDetail.params.ParentCtrl == "EDIBatchDetail") {
                            $("#EDIClaimViewDetail #hfActivePageNumber").val($("#dgvEDIClaimView_paginate li.active a").text());
                            //}
                            EDIClaimViewDetail.LoadEDIClaimViewDetail(EDIClaimViewDetail.params._837BatchId);
                            //Update claims and submitted batch grid
                            Bill_ClaimSubmission.Claim_Search();
                            Bill_ClaimSubmission.SubmitedBatch_Search();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    utility.DisplayMessages("Please select claim.", 2);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'EDIClaimViewDetail';
                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;
                LoadActionPan('EncounterChargeCapture', params, EDIClaimViewDetail.params.PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    PatientDemographics: function (patientid, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["IsFill"] = false;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "EDIClaimViewDetail";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    CheckedClaims: function (obj) {
        if (!$(obj).prop('checked')) {
            $("#EDIClaimViewDetail #chkAllClaims").prop('checked', false);
            $("#EDIClaimViewDetail #chkAllClaims").attr('title', 'Select all');
        }

    },
    GetSelectedClaims: function () {
        // var Claims = [];
        var Claims = "";
        $("#pnlEDIClaimViewDetail_Result #dgvEDIClaimView").find("input[type='checkbox']").each(function (index) {
            if (index > 0 && $(this).prop('checked')) {
                // Claims.push($(this).attr("id"));
                Claims = Claims != "" ? Claims + "," + $(this).attr("id") : "" + $(this).attr("id");
            }

        });
        return Claims;
    },

    ClaimViewDetailLoad: function (_837BatchId) {
        var data = "_837BatchId=" + _837BatchId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_EDI_CLAIMVIEW_DETAIL", "SEARCH_EDI_CLAIMVIEW_DETAIL");
    },

    UpdateClaim: function (visitIDs) {

        var data = "_837BatchId=" + EDIClaimViewDetail.params._837BatchId + "&VisitIDs=" + visitIDs;
        return MDVisionService.defaultService(data, "BILLING_EDI_CLAIMVIEW_DETAIL", "UPDATE_CLAIM");
    },

    UnLoad: function () {
        if (EDIClaimViewDetail.params != null && EDIClaimViewDetail.params.ParentCtrl != null) {
            UnloadActionPan(EDIClaimViewDetail.params.ParentCtrl, "EDIClaimViewDetail");
        }
        else
            UnloadActionPan(null, 'EDIClaimViewDetail');
    },
}
