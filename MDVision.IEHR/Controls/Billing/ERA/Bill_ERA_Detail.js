ERADetail = {
    params: [],
    bIsFirstLoad: true,
    IsAnyLinkedCharge: false,
    RowsState: [],
    ResponseforRad: null,
    Load: function (params) {
        ERADetail.params = params;
        ERADetail.IsAnyLinkedCharge = false;

        if (ERADetail.bIsFirstLoad) {
            ERADetail.bIsFirstLoad = false;
            var self = $("#" + ERADetail.params["PanelID"] + " #ERADetail");
            if (ERADetail.params["PanelID"] != "ERADetail") {
                self = $("#" + ERADetail.params["PanelID"] + " #ERADetail");
            }
            self.loadDropDowns(true).done(function () {
                ERADetail.LoadERADetail();
                //  utility.ValidateFromToDate("frmSSRSReports", ReportsControlDiv.find('.datepickerStart')[i].id, ReportsControlDiv.find('.datepickerEnd')[i].id, true);
                ERADetail.LoadAllAutocomplete(); //ValidateDecimal isValidDate datepicker
                utility.CreateDatePicker(ERADetail.params["PanelID"] + ' #ERADetail #CheckDate');
                utility.CreateDatePicker(ERADetail.params["PanelID"] + ' #ERADetail #CheckDepositDate');
                utility.CreateDatePicker(ERADetail.params["PanelID"] + ' #ERADetail #DateofEntry');


            });
        }
    },

    LoadERADetail: function () {
        AppPrivileges.GetFormPrivileges("Emergency Contact", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (ERADetail.params.mode == "Add") {
                    //serialize Data after all controls loaded.
                    $("#" + ERADetail.params["PanelID"] + " #ERADetail #frmERADetail").data('serialize', $("#" + ERADetail.params["PanelID"] + " #ERADetail #frmERADetail").serialize());
                    ERADetail.UnLoad();

                    //ERADetail.ValidateERADetail();
                }
                else if (ERADetail.params.mode == "Edit") {
                    ERADetail.ERAFill(ERADetail.params.PatientID, ERADetail.params.ERAId).done(function (response) {
                        if (response.status != false) {
                            var ERA_detail = JSON.parse(response.ERAFill_JSON);
                            var self = $("#" + ERADetail.params["PanelID"] + " #ERADetail");
                            utility.bindMyJSONByName(true, ERA_detail, false, self).done(function () {

                                utility.RemoveTimeFromDate("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDate', $("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDate').val());
                                utility.RemoveTimeFromDate("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDepositDate', $("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDepositDate').val());
                                utility.RemoveTimeFromDate("#" + ERADetail.params["PanelID"] + ' #ERADetail #DateofEntry', $("#" + ERADetail.params["PanelID"] + ' #ERADetail #DateofEntry').val());

                                if ($(self).find("#ERAStatus option:selected").text().toLowerCase() == "posted")
                                    $(self).find("#btnPaymentPosting").addClass("disabled");

                                //ERADetail.ValidateERADetail();
                                //serialize Data after all controls loaded.
                                $("#" + ERADetail.params["PanelID"] + " #ERADetail #frmERADetail").data('serialize', $("#" + ERADetail.params["PanelID"] + " #ERADetail #frmERADetail").serialize());
                            });

                            ERADetail.ERADetailFill(ERADetail.params.ERAId).done(function (response) {
                                ERADetail.ResponseforRad = response;
                                ERADetail.NotLinkedChargesGridLoad(response);
                                ERADetail.LinkedPaidChargesGridLoad(response, "");
                                ERADetail.LinkedUnpaidChargesGridLoad(response);
                                ERADetail.LinkedRecoupmentChargesGridLoad(response, "");
                                ERADetail.PLBSegmentGridLoad(response);
                                if (response.LinkedPaidChargessCount <= 0 && response.LinkedUnpaidCharges <= 0) {
                                    $("#" + ERADetail.params["PanelID"] + " #ERADetail #btnPaymentPosting").addClass('disableAll');
                                } else {
                                    $("#" + ERADetail.params["PanelID"] + " #ERADetail #btnPaymentPosting").removeClass('disableAll');
                                }

                                $("#" + ERADetail.params["PanelID"] + " #ERADetail #LinkedPaidCharges #radAll").trigger("click");
                                $("#" + ERADetail.params["PanelID"] + " #ERADetail #LinkedRecoupmentCharges #radLRCAll").trigger("click");
                                //OpenDefaultTab
                                if ($("#" + ERADetail.params["PanelID"] + ' #' + ERADetail.params["OpenDefaultTab"]).length > 0)
                                    $("#" + ERADetail.params["PanelID"] + ' #' + ERADetail.params["OpenDefaultTab"]).click();

                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    OpenERAChargeSearch: function (ERADtId, CPT, ClaimNumber, AccountNo, FirstName, LastName, DOSFrom, DOSTo, TabName) {
        var params = [];
        var data = new Object();
        data['ClaimNumber'] = '';
        data['AccountNo'] = AccountNo;
        data['FirstName'] = FirstName.replace("&@", "'");
        data['LastName'] = LastName.replace("&@", "'");
        data["DOSFrom"] = utility.RemoveTimeFromDate(null, DOSFrom);
        data["DOSTo"] = utility.RemoveTimeFromDate(null, DOSTo);
        data['CPT'] = CPT;
        params["ParentCtrl"] = "ERADetail";
        params["data"] = data;
        ERADetail.params.ERADtId = ERADtId;
        LoadActionPan('ERA_ChargeSearch', params);

    },

    ERADetailUpdate: function (ChargeID, ClaimNumber, event) {

        if (event != null) {
            event.stopPropagation();
        }


        AppPrivileges.GetFormPrivileges("ERA", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Bill_ERACharge_Detail.UpdateERADetail(ERADetail.params.ERADtId, ChargeID, ClaimNumber).done(function (response) {
                    if (response.status != false) {
                        ERA_ChargeSearch.UnLoad();
                        utility.DisplayMessages(response.Message, 1);
                        //Load Grid again.
                        ERADetail.LoadERADetail();
                        Bill_ERA.ERASearch();
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
    //----------------------------------Link and Unlink ERA------------------------------------------------
    LinkERA: function (ERADtlID, ChargeID, IsLink, event, PatientID) {

        if (event != null) {
            event.stopPropagation();
        }

        if (!ERADtlID)
            ERADtlID = ERADetail.params.ERADtId;

        if (IsLink == "true") {
            var params = [];
            params["ParentCtrl"] = "ERA_ChargeSearch";
            params["ScreenName"] = "ERADetail";
            params["ERADtlID"] = ERADtlID;
            params["ChargeID"] = ChargeID;
            params["PatientID"] = PatientID;
            params["IsLink"] = IsLink;
            LoadActionPan('Bill_ERA_Charge_Link_Wizard', params);
        }
        else if (IsLink == "false") {

            AppPrivileges.GetFormPrivileges("ERA", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    utility.myConfirm('20', function () {
                        var selectedValue = ERADtlID;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ERADetail.ERALink(selectedValue, ChargeID, IsLink).done(function (response) {
                                if (response.status != false) {
                                    ERADetail.LoadERADetail();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { }, '1');
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    ERALink: function (ERADtID, ChargeID, IsLink, PaymentInsuranceID) {

        var objData = new JSON.constructor();
        objData["ChargeID"] = ChargeID;
        objData["ERADetailID"] = ERADtID;
        objData["PaymentInsuranceID"] = PaymentInsuranceID;
        objData["IsLink"] = IsLink;
        objData["CommandType"] = "link_era_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },
    //-------------------------------------------------------------------------------------------

    CalculateInsuranceBlance: function ($obj, id, PostStatus, ChargeBalance) {

        /*$row = $($obj).closest("tr");
        var writeoff = Number($row.find($("#WriteOff" + id)).val()); // ERA WriteOff
        var paid = Number($row.find($("#PaidAmount" + id)).val()); // ERA Paid Amount
        var ded = Number($row.find($("#Deductable" + id)).val()); //ERA Deductibles
        var patresp = Number($row.find($("#PatientResponsibility" + id)).val()); //ERA Patient Responsibility
        var coins = Number($row.find($("#CoInsurance" + id)).val()); //ERA Coins
        var copay = Number($row.find($("#Copayment" + id)).val()); // EOB Copay
        var InsCharge = Number($row.find($("#lblInsCharge" + id)).attr("inscharge")); //insurance charge

        //In Case of UnPosted Charge 
        //(ChargeBalance – (ERA Paid + ERA W/O+ ERA Deductibles+ ERA Co Ins + ERA Patient Responsibility)
        //ChargeBalance = InsCharges - ( PaidAmount + WriteOffAmount ) //system charge

        var balance = ChargeBalance - (paid + writeoff + ded + coins + patresp);

        $row.find($("#FeactureInsBalance" + id)).html(utility.convertToFigure(balance));*/

    },


    NotLinkedChargesGridLoad: function (response) {
        $("#" + ERADetail.params["PanelID"] + " #dgvNotLinkedChargesDetail").dataTable().fnDestroy();
        $("#" + ERADetail.params["PanelID"] + " #NotLinkedCharges #dgvNotLinkedChargesDetail tbody").find("tr").remove();
        if (response.NotLinkedChargesCount > 0) {
            var NotLinkedChargesLoadJSONData = JSON.parse(response.NotLinkedCharges);
            $.each(NotLinkedChargesLoadJSONData, function (i, item) {

                var PatientName = '<td title="Patient not found for this claim." >' + item.PatLastName + ", " + item.PatFirstName + " " + item.MI + '</td>';
                var ClaimNumber = '<td title="Claim Number not found.">' + item.ERAClaimNumber + '</td>';

                if (item.UnlinkPatientId && item.UnlinkVisitId) {
                    var DemographicsMethod = "utility.PatientDemographics('" + item.UnlinkPatientId + "', 'ERADetail', event);";
                    var VisitDetail = "utility.LoadVisitDetail('" + item.UnlinkVisitId + "', '" + item.UnlinkPatientId + "', 'ERADetail', event);";

                    PatientName = '<td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatLastName + ", " + item.PatFirstName + " " + item.MI + '</a></td>';
                    ClaimNumber = '<td><a href="#" onclick="' + VisitDetail + '"  title="View Patient">' + item.ERAClaimNumber + '</a></td>';
                }

                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvNotLinkedCharges_row" + item.ERADtlId + "'))");
                $row.attr("id", "gvNotLinkedCharges_row" + item.ERADtlId);
                $row.attr("ERADtlId", item.ERADtlId);

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

                var DisplayClass = "";
                if (item.PostStatus == "Posted")
                    DisplayClass = "disabled";

                var deleteMethod = "ERADetail.DeleteERADetail('" + item.ERADtlId + "')";

                $row.append('<td style="display:none;">' + item.ERADtlId + '</td>'
                            + '<td><a class="btn  btn-xs" href="#" onclick="' + deleteMethod + '" title="Detete Record"><i class="fa fa-close red"></i></a>'
                            + '<a class="btn  btn-xs" href="#" onclick="ERADetail.OpenChargeDetail(' + item.ERADtlId + ',' + item.ERAId + ',' + "'NotLinked'" + ');" title="View Record"><i class="fa fa-eye black"></i></a>'
                            + '<a class="btn  btn-xs ' + DisplayClass + '" href="#" onclick="ERADetail.OpenERAChargeSearch(' + "'" + item.ERADtlId + "'" + ',' + "'" + item.CPTCode + "'" + ',' + "'" + item.ClaimNumber + "'" + ',' + "''" + ',' + "'" + item.PatFirstName.replace("&#39;", "&@") + "'" + ',' + "'" + item.PatLastName.replace("&#39;", "&@") + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSTo) + "'" + ',' + "'NotLinked'" + ');" title="Link Charge"><i class="fa fa-link green"></i></a>' + '</td>'
                            + ClaimNumber + PatientName
                            + '<td>' + item.SubscriberId + '</td>'
                            + '<td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td>'
                            + '<td>' + item.CPTCode + '</td>'
                            //+ '<td>' + utility.convertToFigure(item.ExpectedFee, true) + '</td>'
                            + '<td>' + item.ModifierCode + '</td><td>' + item.UnitsBilled + '</td>'
                            + '<td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PaidAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>'
                            + '<td class="ellipses"  data-placement="left" data-toggle="tooltip" title="' + item.Comments + '" >' + item.Comments + '</td>');

                $("#" + ERADetail.params["PanelID"] + " #NotLinkedCharges #dgvNotLinkedChargesDetail tbody").last().append($row);
            });
        }
        else {
            $("#" + ERADetail.params["PanelID"] + " #NotLinkedCharges #dgvNotLinkedChargesDetail").DataTable({
                "language": {
                    "emptyTable": "No Not Linked Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]

            });
        }
        if ($.fn.dataTable.isDataTable('#dgvNotLinkedChargesDetail'))
            ;
        else
            $("#" + ERADetail.params["PanelID"] + " #NotLinkedCharges #dgvNotLinkedChargesDetail").DataTable({ "bLengthChange": false, "bSort": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    LinkedPaidChargesGridLoad: function (response, Posted) {
        $("#" + ERADetail.params["PanelID"] + " #dgvLinkedPaidChargesDetail").dataTable().fnDestroy();
        $("#" + ERADetail.params["PanelID"] + " #LinkedPaidCharges #dgvLinkedPaidChargesDetail tbody").find("tr").remove();
        if (response.LinkedPaidChargessCount > 0) {
            //set IsAnyLinkedCharge to true, we have some linked charges so make post button to post payments.
            ERADetail.IsAnyLinkedCharge = true;

            var LinkedPaidChargesJSONData = JSON.parse(response.LinkedPaidCharges);
            $.each(LinkedPaidChargesJSONData, function (i, item) {
                var $row = $('<tr mode="write" saved="false" />');
                $row.attr("onclick", "utility.SelectGridRow($('#gvLinkedPaidCharges_row" + item.ERADtlId + "'))");
                $row.attr("id", "gvLinkedPaidCharges_row" + item.ERADtlId);
                $row.attr("ERADtlId", item.ERADtlId);

                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'ERADetail', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'ERADetail', event);";

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

                var DisplayClass = "";
                if (item.PostStatus == "Posted")
                    DisplayClass = "disabled";

                var CrossedOver = "";
                if (item.IsCrossedOver.toLowerCase() == "true")
                    CrossedOver = "Yes";
                else
                    CrossedOver = "No";

                var PostStatus = "";
                if (item.PostStatus.toLowerCase() == "posted")
                    PostStatus = "Yes";
                else
                    PostStatus = "No";

                var postchargeMethod = 'ERADetail.PostCharge(this,' + item.ERADtlId + ',' + item.ERAId + ',' + item.InsBalance + ',' + item.ChargeId + ',' + item.VisitId + ');';
                var updateMethod = "ERADetail.UpdateERACharge('" + item.ERADtlId + "',\'LinkedPaidCharges'\)";
                var unlinkMethod = "ERADetail.LinkERA('" + item.ERADtlId + "','','false', event)";
                var edittoggle = "ERADetail.EditToggleRow(this,'" + item.ERADtlId + "','" + item.PatientId + "','" + item.InsuranceId + "','" + item.PatientInsurancePlanName + "','" + PostStatus + "','" + item.ChargeBalance + "',\'LinkedPaidCharges'\)";

                //var balance = Number(item.ChargedAmount) - (Number(item.PaidAmount) + Number(item.WriteOff) + Number(item.CoInsuranceAmount) + Number(item.DeductableAmount) + Number(item.PatientResponsibility) /*+ Number(item.Copayment)*/);
                if (PostStatus.toLowerCase() == Posted.toLowerCase() || Posted.toLowerCase() == "") {
                    $row.append('<td style="display:none;">' + item.ERADtlId + '</td>'
                        + '<td>'
                        + '<a id="togle_btn' + item.ERADtlId + '" ' + DisplayClass + ' class="btn  btn-xs" href="#" onclick="' + edittoggle + '" title="Edit Record"><i class="fa fa-edit black"></i></a>'
                        + '<a id="save_row' + item.ERADtlId + '" class="btn  btn-xs" href="#" style="display:none" onclick="' + updateMethod + '" title="Save Record"><i class="fa fa-save green"></i></a>'
                        + '<a class="btn  btn-xs" href="#" onclick="ERADetail.OpenChargeDetail(' + item.ERADtlId + ',' + item.ERAId + ',' + "'Paid'" + ');" title="View Record"><i class="fa fa-eye black"></i></a>'
                        + '<a class="btn  btn-xs" ' + DisplayClass + ' href="#" onclick="' + unlinkMethod + '" title="Unlink charge"><i class="fa fa-unlink red"></i></a>'
                        + '<a class="btn  btn-xs btn-element ' + DisplayClass + '" href="#" onclick="ERADetail.OpenERAChargeSearch(' + "'" + item.ERADtlId + "'" + ',' + "'" + item.CPTCode + "'" + ',' + "'" + item.ClaimNumber + "'" + ',' + "'" + item.AccountNumber + "'" + ',' + "'" + item.PatFirstName.replace("&#39;", "&@") + "'" + ',' + "'" + item.PatLastName.replace("&#39;", "&@") + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSTo) + "'" + ',' + "'Paid'" + ');" title="Link Charge"><i class="fa fa-link green"></i></a>'
                        + '<a id="btnPostChargePayment' + item.ERADtlId + '" class="btn  btn-xs btn-element ' + DisplayClass + '" href="#" onclick="' + postchargeMethod + '" title="Post Payment"><i class="fa fa-dollar green"></i></a>'
                        + '<a class="btn  btn-xs btn-element" href="#" onclick="ERADetail.OpenPaymentPosting(' + "'" + item.ClaimNumber + "'" + ',' + "'" + item.ChargeId + "'" + ');" title="Payment Posting"><i class="fa fa-credit-card green"></i></a>'
                        + '</td>'
                   + '<td>' + item.PatientInsurancePlanName + '</td>'
                   + '<td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td>'
                   + '<td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatLastName + ", " + item.PatFirstName + " " + item.MI + '</a></td>'
                   + '<td><a href="#" onclick="' + VisitDetail + '"  title="View Claim">' + item.ClaimNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td>'
                   + '<td>' + item.CPTCode + '</td><td class="text-right">' + utility.convertToFigure(item.ChargedAmount, true) + '</td>'
                   + '<td class="text-right">' + utility.convertToFigure(item.ExpectedFee, true) + '</td>'
                   + '<td id="lblInsCharge' + item.ERADtlId + '" inscharge=' + item.InsCharges + '  class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td>'
                   /*+ '<td id="FeactureInsBalance' + item.ERADtlId + '" >' + utility.convertToFigure(item.InsBalance, true) + '</td>'*/
                   + '<td class="text-right">' + utility.convertToFigure(item.AllowedAmount, true) + '</td>'
                   + '<td type="text" name="PaidAmount" class="text-right">' + utility.convertToFigure(item.PaidAmount, true) + '</td>'
                   + '<td type="text" name="WriteOff" class="text-right" >' + utility.convertToFigure(item.WriteOff, true) + '</td>'
                   + '<td type="text" name="Deductable" class="text-right" >' + utility.convertToFigure(item.DeductableAmount, true) + '</td>'
                   + '<td type="text" name="CoInsurance" class="text-right" >' + utility.convertToFigure(item.CoInsuranceAmount, true) + '</td>'
                   + '<td type="text" name="Copayment" class="text-right" >' + utility.convertToFigure(item.Copayment, true) + '</td>'
                   + '<td class="text-right">' + utility.convertToFigure(item.ChargeCopay, true) + '</td>'
                   + '<td type="text" name="PatientResponsibility" class="text-right" >' + utility.convertToFigure(item.PatientResponsibility, true) + '</td>'
                   + '<td type="ddl"  method="NextResponsibilityDropDown" >' + item.NextResponsibility + '</td>'
                   + '<td type="ddl"  method="NextInsurancePlan"  >' + item.NextInsuranName + '</td>'
                   + '<td>' + item.SecondaryInsurance + '</td><td>' + PostStatus + '</td><td>' + CrossedOver + '</td>');

                    $("#" + ERADetail.params["PanelID"] + " #LinkedPaidCharges #dgvLinkedPaidChargesDetail tbody").last().append($row);
                }
            });
        }
        else {
            $("#" + ERADetail.params["PanelID"] + " #LinkedPaidCharges #dgvLinkedPaidChargesDetail").DataTable({
                "language": {
                    "emptyTable": "No Linked Paid Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvLinkedPaidChargesDetail'))
            ;
        else
            $("#" + ERADetail.params["PanelID"] + " #LinkedPaidCharges #dgvLinkedPaidChargesDetail").DataTable({ "bLengthChange": false, "bSort": false, "autoWidth": false });//, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    LinkedRecoupmentChargesGridLoad: function (response, Posted) {
        $("#" + ERADetail.params["PanelID"] + " #dgvLinkedRecoupmentChargesDetail").dataTable().fnDestroy();
        $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail tbody").find("tr").remove();

        if (response.LinkedRecoupmentChargesCount > 0) {
            var LinkedRecoupmentChargesJSONData = JSON.parse(response.LinkedRecoupmentCharges);

            $.each(LinkedRecoupmentChargesJSONData, function (i, item) {
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'ERADetail', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'ERADetail', event);";
                var $row = $('<tr mode="write" saved="false" />');
                $row.attr("onclick", "utility.SelectGridRow($('#gvLinkedRecoupmentCharges_row" + item.ERADtlId + "'))");
                $row.attr("id", "gvLinkedRecoupmentCharges_row" + item.ERADtlId);
                $row.attr("ERADtlId", item.ERADtlId);

                var DisplayClass = "";
                if (item.PostStatus == "Posted")
                    DisplayClass = "disabled";

                var CrossedOver = "";
                if (item.IsCrossedOver.toLowerCase() == "true")
                    CrossedOver = "Yes";
                else
                    CrossedOver = "No";

                var PostStatus = "";
                if (item.PostStatus.toLowerCase() == "posted")
                    PostStatus = "Yes";
                else
                    PostStatus = "No";

                var updateMethod = "ERADetail.UpdateERACharge('" + item.ERADtlId + "',\'LinkedRecoupmentCharges'\)";
                var edittoggle = "ERADetail.EditToggleRow(this,'" + item.ERADtlId + "','" + item.PatientId + "','" + item.InsuranceId + "','" + item.PatientInsurancePlanName + "','" + PostStatus + "','" + item.ChargeBalance + "',\'LinkedRecoupmentCharges'\)";
                var unlinkMethod = "ERADetail.LinkERA('" + item.ERADtlId + "','','false', event)";
                var balance = Number(item.ChargedAmount) - (Number(item.PaidAmount) + Number(item.WriteOff) + Number(item.CoInsuranceAmount) + Number(item.DeductableAmount) + Number(item.PatientResponsibility) /*+ Number(item.Copayment)*/);

                if (PostStatus.toLowerCase() == Posted.toLowerCase() || Posted.toLowerCase() == "") {
                    $row.append('<td style="display:none;">' + item.ERADtlId + '</td>'
                        + '<td>'
                            + '<a id="togle_btn' + item.ERADtlId + '" ' + DisplayClass + ' class="btn  btn-xs" href="#" onclick="' + edittoggle + '" title="Edit Record"><i class="fa fa-edit black"></i></a>'
                            + '<a id="save_row' + item.ERADtlId + '" class="btn  btn-xs" href="#" style="display:none" onclick="' + updateMethod + '" title="Save Record"><i class="fa fa-save green"></i></a>'
                            + '<a class="btn  btn-xs" href="#" onclick="ERADetail.OpenChargeDetail(' + item.ERADtlId + ',' + item.ERAId + ',' + "'Recoupment'" + ');" title="View Record"><i class="fa fa-eye black"></i></a>'
                            + '<a class="btn  btn-xs" ' + DisplayClass + ' href="#" onclick="' + unlinkMethod + '" title="Unlink charge"><i class="fa fa-unlink red"></i></a>'
                            + '<a class="btn  btn-xs btn-element ' + DisplayClass + '" href="#" onclick="ERADetail.OpenERAChargeSearch(' + "'" + item.ERADtlId + "'" + ',' + "'" + item.CPTCode + "'" + ',' + "'" + item.ClaimNumber + "'" + ',' + "'" + item.AccountNumber + "'" + ',' + "'" + item.PatFirstName.replace("&#39;", "&@") + "'" + ',' + "'" + item.PatLastName.replace("&#39;", "&@") + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSTo) + "'" + ',' + "'Recoupment'" + ');" title="Link Charge"><i class="fa fa-link green"></i></a>'
                            + '<a id="btnPostChargePayment' + item.ERADtlId + '" class="btn  btn-xs btn-element ' + DisplayClass + '" href="#" onclick="ERADetail.PostCharge(this,' + item.ERADtlId + ',' + item.ERAId + ',' + item.InsBalance + ',' + item.ChargeId + ');" title="Post Payment"><i class="fa fa-dollar green"></i></a>'
                            + '<a class="btn  btn-xs btn-element" href="#" onclick="ERADetail.OpenPaymentPosting(' + "'" + item.ClaimNumber + "'" + ',' + "'" + item.ChargeId + "'" + ',true,' + "'" + item.CheckNo + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.CheckDate) + "'" + ',' + "'" + item.PaidDate + "'" + ',' + "'" + item.PaidAmount + "'" + ');" title="Payment Posting"><i class="fa fa-credit-card green"></i></a>'
                        + '</td>'
                        + '<td>' + item.PatientInsurancePlanName + '</td>'
                        + '<td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td>'
                        + '<td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatLastName + ", " + item.PatFirstName + " " + item.MI + '</a></td>'
                        + '<td><a href="#" onclick="' + VisitDetail + '"  title="View Claim">' + item.ClaimNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</a></td>'
                        + '<td>' + item.CPTCode + '</td><td class="text-right">' + utility.convertToFigure(item.ChargedAmount, true) + '</td>'
                        + '<td class="text-right">' + utility.convertToFigure(item.ExpectedFee, true) + '</td>'
                        + '<td class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td>'
                        + '<td class="text-right">' + utility.convertToFigure(item.AllowedAmount, true) + '</td>'
                        + '<td type="text" name="PaidAmount" class="text-right">' + utility.convertToFigure(item.PaidAmount, true) + '</td>'
                        + '<td type="text" name="WriteOff" class="text-right">' + utility.convertToFigure(item.WriteOff, true) + '</td>'
                        + '<td type="text" name="Deductable" class="text-right">' + utility.convertToFigure(item.DeductableAmount, true) + '</td>'
                        + '<td type="text" name="CoInsurance" class="text-right">' + utility.convertToFigure(item.CoInsuranceAmount, true) + '</td>'
                        + '<td type="text" name="Copayment" class="text-right">' + utility.convertToFigure(item.Copayment, true) + '</td>'
                        + '<td class="text-right">' + utility.convertToFigure(item.ChargeCopay, true) + '</td>'
                        + '<td type="text" name="PatientResponsibility" class="text-right">' + utility.convertToFigure(item.PatientResponsibility, true) + '</td>'
                        + '<td type="ddl"  method="NextResponsibilityDropDown">' + item.NextResponsibility + '</td>'
                        + '<td type="ddl"  method="NextInsurancePlan">' + item.NextInsuranName + '</td>'
                        + '<td>' + item.SecondaryInsurance + '</td><td>' + PostStatus + '</td><td>' + CrossedOver + '</td>');

                    $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail tbody").last().append($row);
                }
            });
        }
        else {
            $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail").DataTable({
                "language": {
                    "emptyTable": "No Linked Recoupment Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvLinkedRecoupmentChargesDetail'))
            ;
        else if (ERADetail.params["IsPaginataion"] == false)
            $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail").DataTable({ "bLengthChange": false, "bSort": false, "autoWidth": false, "bPaginate": false, "bInfo": false });//, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        else
            $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail").DataTable({ "bLengthChange": false, "bSort": false, "autoWidth": false });//, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    PLBSegmentGridLoad: function (response) {

        $("#" + ERADetail.params["PanelID"] + " #dgvPLBSegmentDetail").dataTable().fnDestroy();
        $("#" + ERADetail.params["PanelID"] + " #PLBSegment #dgvPLBSegmentDetail tbody").find("tr").remove();
        if (response.PLBSegmentCount > 0) {

            var PLBSegmentJSONData = JSON.parse(response.PLBSegment_JSON);
            $.each(PLBSegmentJSONData, function (i, item) {

                var $row = $('<tr mode="write" saved="false" />');
                $row.attr("onclick", "utility.SelectGridRow($('#gvPLBSegment_row" + item.ERAProviderAdjId + "'))");
                $row.attr("id", "gvPLBSegment_row" + item.ERAProviderAdjId);
                $row.attr("ERADtlId", item.ERAProviderAdjId);


                $row.append('<td style="display:none;">' + item.ERAProviderAdjId + '</td>'
                    + '<td>'
                     + '<a class="btn  btn-xs btn-element" href="#" onclick="ERADetail.OpenPaymentPosting(' + "null" + ',' + "null" + ',true);" title="Post Payment"><i class="fa fa-dollar green"></i></a>'
                    + '</td>'
               + '<td>' + item.ReasonCode + '</td>'
               + '<td class="ellipses"  data-placement="left" data-toggle="tooltip" title="' + item.Description + '"  >' + item.Description + '</td>'
               + '<td>' + item.Identifier + '</td>'
               + '<td class="text-right">' + item.Amount + '</td>');

                $("#" + ERADetail.params["PanelID"] + " #PLBSegment #dgvPLBSegmentDetail tbody").last().append($row);
            });
        }
        else {
            $("#" + ERADetail.params["PanelID"] + " #PLBSegment #dgvPLBSegmentDetail").DataTable({
                "language": {
                    "emptyTable": "No PLB Segment Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPLBSegmentDetail'))
            ;
        else if (ERADetail.params["IsPaginataion"] == false)
            $("#" + ERADetail.params["PanelID"] + " #PLBSegment #dgvPLBSegmentDetail").DataTable({ "bLengthChange": false, "bSort": false, "autoWidth": false, "bPaginate": false, "bInfo": false });//, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        else
            $("#" + ERADetail.params["PanelID"] + " #PLBSegment #dgvPLBSegmentDetail").DataTable({ "bLengthChange": false, "bSort": false, "autoWidth": false });//, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    OpenPaymentPosting: function (ClaimNumber, ChargeId, IsSearchCharges, CheckNumber, CheckDate, PaidDate, PaidAmount) {

        if (IsSearchCharges == "" || IsSearchCharges == "undefined" || IsSearchCharges == null)
            IsSearchCharges = false;

        if (IsSearchCharges == true)
            ChargeId = 0;

        if (CheckNumber == "" || CheckNumber == "undefined" || CheckNumber == null)
            CheckNumber = "";

        if (CheckDate == "" || CheckDate == "undefined" || CheckDate == null)
            CheckDate = "";

        if (PaidDate == "" || PaidDate == "undefined" || PaidDate == null)
            PaidDate = "";

        if (PaidAmount == "" || PaidAmount == "undefined" || PaidAmount == null)
            PaidAmount = "";

        var Comments = "";
        if (ClaimNumber != "" && CheckNumber != "")
            Comments = "Paid amount allocated from the recouped amount of claim# " + ClaimNumber + " under check# " + CheckNumber;


        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["mode"] = 'edit';
                params["ParentCtrl"] = 'ERADetail';
                params["ClaimNumber"] = ClaimNumber;
                params["ChargeId"] = ChargeId;
                params["IsSearchCharges"] = IsSearchCharges;
                params["CheckNumber"] = CheckNumber;
                params["CheckDate"] = CheckDate;
                params["PaidDate"] = PaidDate;
                params["PaidAmount"] = PaidAmount;
                params["Comments"] = Comments;
                params["PaymentRef"] = "ERADetail";

                LoadActionPan('Bill_PaymentPosting', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    SetInsurancePlanddl: function (obj, Id, PatientId) {
        if ($(obj).val().toLowerCase() == "patient" || $(obj).val() == "") {
            $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + Id).attr("disabled", "disabled");
            $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + Id).val("");
        }
        else {
            $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + Id).removeAttr("disabled");
            ERADetail.NextInsurancePlan($("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + Id).parent(), Id, PatientId, '');
        }
    },

    NextResponsibilityDropDown: function (Id, PatientId, val_) {
        var str = '<select onchange="ERADetail.SetInsurancePlanddl(this,' + Id + ',' + PatientId + ')" id="ddlNextResponsibility' + Id + '" class="form-control p-tiny size80">'
                  + '<option value="" >- Select -</option>'
                  + '<option value="Patient" >Patient</option>'
                  + '<option value="Insurance">Insurance</option>'
                  + '</select>';

        return $(str).val(val_);
    },

    NextInsurancePlan: function (obj, Id, PatientId, val_) {
        var str = '<select id="ddlNextInsurancePlan' + Id + '" class="form-control p-tiny size80">'
                 + '<option value="" >- Select -</option>'
                 + '</select>';
        var $str = $(str);
        var ObjDeferred = $.Deferred();

        CacheManager.BindPatientData('GetPatientInsurance', true, PatientId, 1).done(function (result) {
            result = JSON.parse(result.GetPatientInsurance);

            $.each(result, function (i, item) {
                if (item.Value != "")
                    $($str).append(
                    $('<option />', {
                        value: item.Value,
                        html: item.Name,
                        priority: item.RefValue,
                        coPayment: item.RefName
                    }));
            });

            ObjDeferred.resolve();
            var string = "";
        });

        $.when(ObjDeferred).then(function () {
            $(obj).html($str);
            if ($("#ddlNextResponsibility" + Id).val() == "" || $("#ddlNextResponsibility" + Id).val().toLowerCase() == "patient") {
                $(obj).find($str).attr("disabled", "disabled");
                $(obj).find($str).val("");
            }
            else {
                if (val_ != "")
                    $(obj).find($str).val($(obj).find($str).find('option:contains("' + val_ + '")').val());
            }
        });
    },

    EditToggleRow: function (obj, ERADtlId, PatientId, InsuranceId, PatientInsurancePlanName, PostStatus, ChargeBalance, TabId) {
        var $currentRow;

        if (TabId == 'LinkedPaidCharges')
            $currentRow = $("#" + ERADetail.params["PanelID"] + " #LinkedPaidCharges #dgvLinkedPaidChargesDetail tbody").find("#gvLinkedPaidCharges_row" + ERADtlId);
        else if (TabId == 'LinkedRecoupmentCharges')
            $currentRow = $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail tbody").find("#gvLinkedRecoupmentCharges_row" + ERADtlId);

        var mode = $($currentRow).attr("mode");

        if (mode == "write") {
            var row_state = ERADetail.RowsState.filter(function (obj) {
                return obj.Id == ERADtlId;
            });

            if (row_state.length > 0)
                ERADetail.RowsState.pop(row_state[0]);

            //save row state
            ERADetail.RowsState.push({ Id: ERADtlId, State: $currentRow.html() });

            //set icons and title
            $(obj).children("i").removeClass("fa fa-edit");
            $(obj).children("i").addClass("fa fa-close");
            $(obj).attr("title", "Cancle Row");

            $($currentRow).find("td").each(function (e, item) {
                var type = $(this).attr("type");

                if (type) {
                    if (type == "text") {
                        var name = $(this).attr("name");
                        var val_ = $(this).html();
                        $(this).html('<input onkeyup="utility.ValidateNegativeDecimal(event, 2);" class="form-control" id="' + name + '' + ERADtlId + '" value="' + ERADetail.RemoveSign(val_) + '" type="text">');
                    }
                    else if (type == "ddl") {
                        var method = $(this).attr("method");
                        var val_ = $(this).html();
                        if (method.toLowerCase() == "nextinsuranceplan") {
                            ERADetail.NextInsurancePlan($(this), ERADtlId, PatientId, val_);
                        }
                        else if (method.toLowerCase() == "nextresponsibilitydropdown") {
                            $(this).html(ERADetail.NextResponsibilityDropDown(ERADtlId, PatientId, val_));
                        }
                    }
                }
            });

            //view save button
            $("#" + ERADetail.params["PanelID"] + " #save_row" + ERADtlId).css("display", "inline-block");
            //set row mode
            $($currentRow).attr("mode", "read");
            $($currentRow).find("#btnPostChargePayment" + ERADtlId).css("display", "none");
        }
        else {
            if ($currentRow.attr("saved").toLowerCase() != "true") {
                var row_state = ERADetail.RowsState.filter(function (obj) {
                    return obj.Id == ERADtlId;
                });

                var $old_row = $(row_state[0].State);
                if (TabId == 'LinkedPaidCharges')
                    $("#" + ERADetail.params["PanelID"] + " #LinkedPaidCharges #dgvLinkedPaidChargesDetail tbody").find($currentRow).html($old_row);
                else if (TabId == 'LinkedRecoupmentCharges')
                    $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail tbody").find($currentRow).html($old_row);
                
                ERADetail.RowsState.pop(row_state[0]);
                $($currentRow).attr("mode", "write");
                $currentRow.find("#btnPostChargePayment" + ERADtlId).css("display", "inline-block");
            }
            else {
                //set icons and title
                $(obj).children("i").removeClass("fa fa-close");
                $(obj).children("i").addClass("fa fa-edit");
                $(obj).attr("title", "Edit Record");

                $($currentRow).find("td").each(function (e, item) {
                    var type = $(this).attr("type");
                    if (type) {
                        if (type == "text") {
                            var name = $(this).attr("name");
                            $(this).html(utility.convertToFigure($("#" + ERADetail.params["PanelID"] + " #" + name + ERADtlId).val(), true));
                        }
                        else if (type == "ddl") {
                            var method = $(this).attr("method");
                            if (method.toLowerCase() == "nextinsuranceplan") {
                                var text_ = $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + ERADtlId + " :selected").text();
                                if (text_ == "- Select -")
                                    text_ = "";

                                $(this).html(text_);
                            }
                            else if (method.toLowerCase() == "nextresponsibilitydropdown") {
                                var text_ = $("#" + ERADetail.params["PanelID"] + " #ddlNextResponsibility" + ERADtlId + " :selected").text();
                                if (text_ == "- Select -")
                                    text_ = "";

                                $(this).html(text_);
                            }
                        }
                    }
                });

                //hide save button
                $("#" + ERADetail.params["PanelID"] + " #save_row" + ERADtlId).css("display", "none");
                //set row mode
                $($currentRow).attr("mode", "write");
                $currentRow.attr("saved", "false");
                $currentRow.find("#btnPostChargePayment" + ERADtlId).css("display", "inline-block");
            }
        }
    },

    UpdateERACharge: function (ERADtlId, TabId) {
        var $currentRow;

        if (TabId == 'LinkedPaidCharges')
            $currentRow = $("#" + ERADetail.params["PanelID"] + " #LinkedPaidCharges #dgvLinkedPaidChargesDetail tbody").find("#gvLinkedPaidCharges_row" + ERADtlId);
        else if (TabId == 'LinkedRecoupmentCharges')
            $currentRow = $("#" + ERADetail.params["PanelID"] + " #LinkedRecoupmentCharges #dgvLinkedRecoupmentChargesDetail tbody").find("#gvLinkedRecoupmentCharges_row" + ERADtlId);

        if (ERADetail.ValidateRow($currentRow, ERADtlId)) {
            var myJSON = $currentRow.getMyJSON();

            ERADetail.Update_ERADetail(myJSON, ERADtlId).done(function (response) {
                if (response.status != false) {
                    $currentRow.attr("saved", "true");
                    ERADetail.LoadERADetail();
                    utility.DisplayMessages(response.Message, 1);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }
        else {
            utility.DisplayMessages("Fill all required fields", 3);
        }
    },

    RemoveSign: function (val) {
        var str = val.split("$");
        if (str.length > 1)
            return str[1];
        else
            return 0;
    },

    ValidateRow: function ($currentRow, ERADtlId) {

        var isvalid = true;

        $($currentRow).find("td").each(function (e, item) {

            var type = $(this).attr("type");

            if (type) {
                if (type == "text") {
                    var name = $(this).attr("name");
                    var val_ = $("#" + ERADetail.params["PanelID"] + " #" + name + ERADtlId).val();
                    if (val_ == "") {
                        $("#" + ERADetail.params["PanelID"] + " #" + name + ERADtlId).css("border-color", "red");
                        isvalid = false;
                    }
                    else
                        $("#" + ERADetail.params["PanelID"] + " #" + name + ERADtlId).css("border-color", "#ccc");
                }
                else if (type == "ddl") {
                    var method = $(this).attr("method");
                    if (method.toLowerCase() == "nextinsuranceplan") {

                        var val_ = $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + ERADtlId).val();
                        var isdisable = $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + ERADtlId).attr("disabled");

                        if (isdisable != "disabled") {
                            if (val_ == "") {
                                $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + ERADtlId).css("border-color", "red");
                                isvalid = false;
                            }
                            else
                                $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + ERADtlId).css("border-color", "#ccc");
                        }
                        else
                            $("#" + ERADetail.params["PanelID"] + " #ddlNextInsurancePlan" + ERADtlId).css("border-color", "#ccc");


                    }
                    //    else if (method.toLowerCase() == "nextresponsibilitydropdown") {

                    //        var val_ = $("#" + ERADetail.params["PanelID"] + " #ddlNextResponsibility" + ERADtlId).val();
                    //        if (val_ == "") {
                    //            $("#" + ERADetail.params["PanelID"] + " #ddlNextResponsibility" + ERADtlId).css("border-color", "red");
                    //            isvalid = false;
                    //        }
                    //        else
                    //            $("#" + ERADetail.params["PanelID"] + " #ddlNextResponsibility" + ERADtlId).css("border-color", "#ccc");
                    //    }
                }

            }
        });

        return isvalid;

    },

    DeleteERADetail: function (ERADetailId) {

        AppPrivileges.GetFormPrivileges("ERA", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ERADetailId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        ERADetail.Delete_ERADetail(ERADetailId).done(function (response) {
                            if (response.status != false) {
                                ERADetail.LoadERADetail();
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

        });

    },

    //adnan maqbool, pms-4259, 02-03-2016 
    ChangeColor: function (ob, control) {
        if (ob.className == "toggle active") {
            $("#" + ERADetail.params["PanelID"] + " #" + control).css("cssText", "color:white !important;");
        } else {

            $("#" + ERADetail.params["PanelID"] + " #" + control).css("cssText", "color:#0088cc !important;");
        }

    },
    //end
    LinkedUnpaidChargesGridLoad: function (response) {
        $("#" + ERADetail.params["PanelID"] + " #dgvLinkedUnpaidChargessDetail").dataTable().fnDestroy();
        $("#" + ERADetail.params["PanelID"] + " #LinkedUnpaidCharges #dgvLinkedUnpaidChargessDetail tbody").find("tr").remove();
        if (response.LinkedUnpaidChargesCount > 0) {

            //set IsAnyLinkedCharge to true, we have some linked charges so make post button to post payments.
            ERADetail.IsAnyLinkedCharge = true;

            var LinkedUnpaidChargesJSONData = JSON.parse(response.LinkedUnpaidCharges);
            $.each(LinkedUnpaidChargesJSONData, function (i, item) {
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'ERADetail', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'ERADetail', event);";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvLinkedUnpaidCharges_row" + item.ERADtlId + "'))");
                $row.attr("id", "gvLinkedUnpaidCharges_row" + item.ERADtlId);
                $row.attr("ERADtlId", item.ERADtlId);

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

                var DisplayClass = "";
                if (item.PostStatus == "Posted")
                    DisplayClass = "disabled";

                var PostStatus = "";
                if (item.PostStatus.toLowerCase() == "posted")
                    PostStatus = "Yes";
                else
                    PostStatus = "No";

                var unlinkMethod = "ERADetail.LinkERA('" + item.ERADtlId + "','','false', event)";

                //var balance = Number(item.ChargedAmount) - (Number(item.PaidAmount) + Number(item.WriteOff) + Number(item.CoInsuranceAmount) + Number(item.DeductableAmount) + Number(item.PatientResponsibility) /*+ Number(item.Copayment)*/);


                $row.append('<td style="display:none;">' + item.ERADtlId + '</td>'
                           + '<td><a class="btn  btn-xs" href="#" onclick="ERADetail.OpenChargeDetail(' + item.ERADtlId + ',' + item.ERAId + ',' + "'UnPaid'" + ');" title="View Record"><i class="fa fa-eye black"></i></a>'
                           + '<a class="btn  btn-xs" ' + DisplayClass + ' href="#" onclick="' + unlinkMethod + '" title="Unlink Charge"><i class="fa fa-unlink red"></i></a>'
                           + '<a class="btn  btn-xs btn-element ' + DisplayClass + '" href="#" onclick="ERADetail.OpenERAChargeSearch(' + "'" + item.ERADtlId + "'" + ',' + "'" + item.CPTCode + "'" + ',' + "'" + item.ClaimNumber + "'" + ',' + "'" + item.AccountNumber + "'" + ',' + "'" + item.PatFirstName.replace("&#39;", "&@") + "'" + ',' + "'" + item.PatLastName.replace("&#39;", "&@") + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "'" + ',' + "'" + utility.RemoveTimeFromDate(null, item.DOSTo) + "'" + ',' + "'UnPaid'" + ');" title="Link Charge"><i class="fa fa-link green"></i></a>'
                           + '<a  class="btn  btn-xs btn-element ' + DisplayClass + '" href="#" onclick="ERADetail.PostCharge(this,' + item.ERADtlId + ',' + item.ERAId + ',' + item.InsBalance + ',' + item.ChargeId + ');" title="Post Payment"><i class="fa fa-dollar green"></i></a>'
                           + '<a class="btn  btn-xs btn-element" href="#" onclick="ERADetail.OpenPaymentPosting(' + "'" + item.ClaimNumber + "'" + ',' + "'" + item.ChargeId + "'" + ');" title="Payment Posting"><i class="fa fa-credit-card green"></i></a></td>'
                           + '<td>' + item.PatientInsurancePlanName + '</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatLastName + ", " + item.PatFirstName + " " + item.MI + '</a></td>'
                           + '<td><a href="#" onclick="' + VisitDetail + '"  title="View Claim">' + item.ClaimNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td>'
                           + '<td>' + item.CPTCode + '</td><td class="text-right">' + utility.convertToFigure(item.ChargedAmount, true) + '</td>'
                           + '<td class="text-right">' + utility.convertToFigure(item.ExpectedFee, true) + '</td>'
                           //+ '<td>' + utility.convertToFigure(item.InsBalance, true) + '</td>'
                           + '<td class="text-right">' + utility.convertToFigure(item.AllowedAmount, true) + '</td>'
                           + '<td class="text-right">' + utility.convertToFigure(item.PaidAmount, true) + '</td>'
                           + '<td>' + item.RemitCode + '</td><td>' + PostStatus + '</td><td>' + item.SecondaryInsurance + '</td>');

                $("#" + ERADetail.params["PanelID"] + " #LinkedUnpaidCharges #dgvLinkedUnpaidChargessDetail tbody").last().append($row);
            });
        }
        else {
            $("#" + ERADetail.params["PanelID"] + " #LinkedUnpaidCharges #dgvLinkedUnpaidChargessDetail").DataTable({
                "language": {
                    "emptyTable": "No Linked UnPaid Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvLinkedUnpaidChargessDetail'))
            ;
        else
            $("#" + ERADetail.params["PanelID"] + " #LinkedUnpaidCharges #dgvLinkedUnpaidChargessDetail").DataTable({ "bLengthChange": false, "bSort": false, "autoWidth": false });//, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    //-------------------------------------------------------------------------------------------
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'ERADetail';
        LoadActionPan('Patient_Search', params);
    },

    ValidateERADetail: function () {
        $("#" + ERADetail.params["PanelID"] + " #ERADetail #frmERADetail")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   txtFacility: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtProvider: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            ERADetail.SaveERA();
        });
    },

    FillERA: function (PatientID, ERAId) {
        ERADetail.params["ERAId"] = ERAId;
        AppPrivileges.GetFormPrivileges("Emergency Contact", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                ERADetail.ERAFill(PatientID, ERAId).done(function (response) {
                    if (response.status != false) {
                        LoadActionPan('ERADetail', null);
                        //UnloadActionPan(ERADetail.params["ParentCtrl"]);
                        var ERA_detail = JSON.parse(response.ERAFill_JSON);
                        var self = $("#" + ERADetail.params["PanelID"] + " #ERADetail");
                        utility.bindMyJSONByName(true, ERA_detail, false, self);

                        utility.RemoveTimeFromDate("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDate', $("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDate').val());
                        utility.RemoveTimeFromDate("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDepositDate', $("#" + ERADetail.params["PanelID"] + ' #ERADetail #CheckDepositDate').val());
                        utility.RemoveTimeFromDate("#" + ERADetail.params["PanelID"] + ' #ERADetail #DateofEntry', $("#" + ERADetail.params["PanelID"] + ' #ERADetail #DateofEntry').val());
                        ERADetail.params["mode"] = "Edit";

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

    PostCharge: function (obj, DetailId, ERAId, InsBalance, ChargeId, VisitId) {
        ERADetail.IsVoidedClaimExist(ERAId, VisitId).done(function (result) {
            if (result.status != false && result.VoidedClaims) {
                var VoidedClaimNumber = result.VoidedClaims;
                utility.myConfirm("The claim/s " + VoidedClaimNumber.substring(0, VoidedClaimNumber.length - 1) + " are voided please link ERA with new claims. Do you want to proceed?", function () {
                    ERADetail.ERAChargePost(obj, DetailId, ERAId, InsBalance, ChargeId);
                }, function () {

                }, "Voided Claims Alert");
            }
            else {
                ERADetail.ERAChargePost(obj, DetailId, ERAId, InsBalance, ChargeId);
            }
        });
        
    },
    ERAChargePost: function (obj, DetailId, ERAId, InsBalance, ChargeId)
    {
        var dfd = new $.Deferred();
        if (Number(InsBalance).toFixed(globalAppdata.DecimalPlaces) < 0) {

            utility.myConfirm('31', function () { dfd.resolve("ok"); }, function () { }, '31', "Continue posting", "Cancel");
        }
        else {
            dfd.resolve("ok");
        }

        dfd.done(function () {

            var objDeffered = $.Deferred();
            Bill_PaymentPosting.IsCheckPosted(ERADetail.params.CheckNo, ChargeId).done(function (response) {

                if (response.Message != "") {
                    utility.myConfirm(response.Message, function () {
                        objDeffered.resolve("ok");
                    }, function () { }, "Confirm Duplicate Payment");
                }
                else
                    objDeffered.resolve("ok");

            });

            objDeffered.done(function (message) {

                AppPrivileges.GetFormPrivileges("ERA", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        ERADetail.Post_Charges_Payment(ERAId, DetailId).done(function (response) {
                            if (response.status != false) {

                                $(obj).addClass("disabled");
                                $($(obj).closest('a').siblings()[1]).addClass("disabled");

                                utility.DisplayMessages(response.Message, 1);

                                //load grid again.
                                ERADetail.LoadERADetail();
                                Bill_ERA.ERASearch();

                            } else {

                                utility.DisplayMessages(response.Message, 3);
                            }
                        });

                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });

            });

        });
    },


    PostPayment: function (ERAId, event) {


        if (event != null) {
            event.stopPropagation();
        }

        if (!ERAId)
            ERAId = ERADetail.params.ERAId;

        if (ERADetail.IsAnyLinkedCharge == true) {

            utility.myConfirm("13", function () {

                AppPrivileges.GetFormPrivileges("ERA", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

                    if (strMessage == "") {

                        var objDeffered = $.Deferred();
                        Bill_PaymentPosting.IsCheckPosted(ERADetail.params.CheckNo).done(function (response) {

                            if (response.Message != "") {
                                utility.myConfirm(response.Message, function () {
                                    objDeffered.resolve("ok");
                                }, function () { }, "Confirm Duplicate Payment");
                            }
                            else {
                                objDeffered.resolve("ok");
                            }

                        });

                        objDeffered.done(function () {

                            ERADetail.Post_Payment(ERAId).done(function (response) {
                                if (response.status != false) {

                                    $("#" + ERADetail.params["PanelID"] + " #dgvLinkedPaidChargesDetail").find("a").each(function () {

                                        if ($(this).hasClass("btn-element"))
                                            $(this).addClass("disabled");
                                    });

                                    $("#" + ERADetail.params["PanelID"] + " #dgvLinkedUnpaidChargessDetail").find("a").each(function () {

                                        if ($(this).hasClass("btn-element"))
                                            $(this).addClass("disabled");
                                    });

                                    if (response.IsMessage == true)
                                        utility.DisplayMessages(response.Message, 1);

                                    //load grid again.
                                    ERADetail.LoadERADetail();
                                    Bill_ERA.ERASearch();

                                    if (response.UnPostedChargesCount > 0) {
                                        ERADetail.OpenERASummary(ERAId, response.UnPostedChargesCount, response.UnPostedCharges_JSON);
                                    }



                                } else {

                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        });

                    }
                    else
                        utility.DisplayMessages(strMessage, 2);

                });

            }, function () { }, "13");

        }
        else {
            utility.DisplayMessages("Couldn't found any linked charge to post payment.", 2);
        }

    },


    OpenERASummary: function (ERAId, UnPostedChargesCount, UnPostedCharges_JSON) {
        var params = [];
        params["mode"] = "Edit";
        params["ERAId"] = ERAId;
        params["UnPostedCharges_JSON"] = UnPostedCharges_JSON;
        params["UnPostedChargesCount"] = UnPostedChargesCount;
        params["ParentCtrl"] = "ERADetail";
        LoadActionPan('Bill_ERA_Summary', params);
    },

    Post_Payment: function (ERAId, DetailId) {

        var objData = new JSON.constructor();
        objData["ERAID"] = ERAId;
        objData["ERADetailID"] = DetailId;
        objData["CommandType"] = "post_payment";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");

    },
    IsVoidedClaimExist: function (ERAId, VisitId) {

        var objData = new JSON.constructor();
        objData["ERAID"] = ERAId;
        objData["VisitID"] = VisitId;
        objData["CommandType"] = "Voided_Claim_Exist";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");

    },

    Post_Charges_Payment: function (ERAId, ERADetailIds) {


        var objData = new JSON.constructor();
        objData["ERAID"] = ERAId;
        objData["ERADetailIDs"] = ERADetailIds;
        objData["CommandType"] = "post_charges_payment";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");

    },



    OpenViewERA: function () {

        var params = [];
        params["EDIReportId"] = ERADetail.params.EDIReportId;
        params["CheckNo"] = ERADetail.params.CheckNo;
        params["ParentCtrl"] = 'ERADetail';
        LoadActionPan('EDIReviewReport', params);

    },


    SaveERA: function () {
        var strMessage = "";
        if (ERADetail.params.ERACount == "0") {
            $("#hfIsPrimary").val("True");
        }
        var self = $("#" + ERADetail.params["PanelID"] + " #ERADetail");
        var myJSON = self.getMyJSONByName();

        if (ERADetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Emergency Contact", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ERADetail.ERASave(myJSON, ERADetail.params.PatientID).done(function (response) {
                        if (response.status != false) {
                            ERADetail.params["ERAId"] = response.ERAID;
                            ERADetail.params["mode"] = "Edit";
                            Bill_ERA.Load();
                            UnloadActionPan(ERADetail.params["ParentCtrl"]);
                            utility.DisplayMessages(response.message, 1);
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
        else if (ERADetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Emergency Contact", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ERADetail.ERAUpdate(myJSON, ERADetail.params.ERAId).done(function (response) {
                        if (response.status != false) {

                            utility.DisplayMessages(response.message, 1);
                            // ERADetail.UnLoad();
                            UnloadActionPan(ERADetail.params["ParentCtrl"]);
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



    Delete_ERADetail: function (ERADetailId) {

        var objData = new JSON.constructor();

        objData["ERADetailID"] = ERADetailId;
        objData["CommandType"] = "delete_era_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },

    Update_ERADetail: function (ERAData, ERADetailId) {
        var data = "ERAData=" + ERAData + "&ERADetailId=" + ERADetailId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILL_ERA_DETAIL", "UPDATE_ERA_DETAIL_RECORD");
    },


    ERAUpdate: function (ERAData, ERAID) {

        var objData = new JSON.constructor();
        if (ERAData)
            objData = JSON.parse(ERAData);

        objData["ERAID"] = ERAID;
        objData["CommandType"] = "update_era";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },

    ERASave: function (ERAData, patientID) {

        var objData = new JSON.constructor();
        if (ERAData)
            objData = JSON.parse(ERAData);

        objData["PatientID"] = patientID;
        objData["CommandType"] = "save_era_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },

    ERAFill: function (patientID, ERAId) {

        var objData = new JSON.constructor();
        objData["patientID"] = patientID;
        objData["ERAID"] = ERAId;
        objData["CommandType"] = "fill_era";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },
    ERADetailFill: function (ERAId) {

        var objData = new JSON.constructor();
        objData["ERAID"] = ERAId;
        objData["CommandType"] = "fill_era_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },
    OpenChargeDetail: function (ERADtlId, ERAId, TabName) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'ERADetail';
        params["ERAId"] = ERAId;
        params["ERADtId"] = ERADtlId;
        LoadActionPan('Bill_ERACharge_Detail', params);
    },

    UnLoad: function (Tab) {
        ERADetail.ResponseforRad = null;
        utility.UnLoadDialog(ERADetail.params["PanelID"] + " #ERADetail #frmERADetail", function () {
            UnloadActionPan(ERADetail.params["ParentCtrl"], "ERADetail");
            if ($('body').find('.modal-backdrop').length > 0) {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
        }, function () {
            UnloadActionPan(ERADetail.params["ParentCtrl"], "ERADetail");
            if ($('body').find('.modal-backdrop').length > 0) {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
        });
        //var ua = window.navigator.userAgent;
        //var msie = ua.indexOf("MSIE ");

        // bug# pms-992
        //if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        //if ($('body').find('.modal-backdrop').length > 0) {
        //    $('body').removeClass('modal-open');
        //    $('.modal-backdrop').remove();
        //}
        ///}
    },

    // To make sure of Sub tabs styles
    ChangeSubTabStyle: function (ev) {
        $(ev).parent().find("a").removeClass("active");
        $(ev).addClass("active");
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + ERADetail.params["PanelID"] + " input#txtProvider");
            var hfCtrl = $("#" + ERADetail.params["PanelID"] + " #hfProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });


        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $('#' + ERADetail.params["PanelID"] + " input#txtFacility");
            var hfCtrl = $("#" + ERADetail.params["PanelID"] + " #hfFacility")
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });

    },


    // -------------- Provider ---------------------

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmERADetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ERADetail";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + ERADetail.params.PanelID + ' #hfProvider').val(), "ERADetail");
        var params = [];
        params["ProviderId"] = $('#' + ERADetail.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ERADetail';
        LoadActionPan('providerDetail', params);
    },

    HideProviderLink: function () {
        $('#' + ERADetail.params.PanelID + ' #txtProvider').attr("ProviderId", "-1");
        $('#' + ERADetail.params.PanelID + ' #hfProvider').val("-1");
        $('#' + ERADetail.params.PanelID + ' #lnkProviderEdit').css("display", "none");
        $('#' + ERADetail.params.PanelID + ' #lblProvider').css("display", "inline");
    },
    // -------------- End Provider -----------------

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmERADetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ERADetail";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + ERADetail.params.PanelID + ' #hfFacility').val(), "ERADetail");
        var params = [];
        params["FacilityId"] = $('#' + ERADetail.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'ERADetail';
        LoadActionPan('facilityDetail', params);
    },

    HideFacilityLink: function () {
        $('#' + ERADetail.params.PanelID + ' #txtFacility').attr("FacilityId", "-1");
        $('#' + ERADetail.params.PanelID + ' #hfFacility').val("-1");
        $('#' + ERADetail.params.PanelID + ' #lnkFacilityEdit').css("display", "none");
        $('#' + ERADetail.params.PanelID + ' #lblFacility').css("display", "inline");
    },
    // -------------- End Facility -----------------

    LinkedPaidChargesRadio: function (obj) {
        if ($(obj).prop('id') == "radYes" || $(obj).prop('id') == "radLRCYes") {
            if ($(obj).parent().parent().prop('id') == 'divRadPosted')
                ERADetail.LinkedPaidChargesGridLoad(ERADetail.ResponseforRad, "Yes");
            else
                ERADetail.LinkedRecoupmentChargesGridLoad(ERADetail.ResponseforRad, "Yes");
        }
        else if ($(obj).prop('id') == "radNo" || $(obj).prop('id') == "radLRCNo") {
            if ($(obj).parent().parent().prop('id') == 'divRadPosted')
                ERADetail.LinkedPaidChargesGridLoad(ERADetail.ResponseforRad, "No");
            else
                ERADetail.LinkedRecoupmentChargesGridLoad(ERADetail.ResponseforRad, "No");
        }
        else {
            if ($(obj).parent().parent().prop('id') == 'divRadPosted')
                ERADetail.LinkedPaidChargesGridLoad(ERADetail.ResponseforRad, "");
            else
                ERADetail.LinkedRecoupmentChargesGridLoad(ERADetail.ResponseforRad, "");
        }
    },
}