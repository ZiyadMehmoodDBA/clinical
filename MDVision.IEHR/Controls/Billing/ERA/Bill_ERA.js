Bill_ERA = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {

        Bill_ERA.params = params;
        if (Bill_ERA.bIsFirstLoad) {
            Bill_ERA.bIsFirstLoad = false;



            if (Bill_ERA.params != null && Bill_ERA.params.PanelID != "pnlBillERA") {
                Bill_ERA.params["PanelID"] = Bill_ERA.params["PanelID"] + ' #pnlBillERA';
            }
            else {
                //Bill_ERA.params = [];
                Bill_ERA.params["PanelID"] = "pnlBillERA"
            }

            var self = $('#' + Bill_ERA.params["PanelID"]);
            self.loadDropDowns(true).done(function () {


                var partiall_posted = self.find('#ddlStatus option').filter(function () {
                    return this.text == "Partially Posted"
                });

                var unposted = self.find('#ddlStatus option').filter(function () {
                    return this.text == "UnPosted"
                });

                var vals = "";
                if (partiall_posted)
                    vals = partiall_posted.val()
                if (unposted)
                    vals += "," + unposted.val();

                //add Partially Posted and Unposted option in ERA Status
                self.find("#ddlStatus").append($("<option />").val(vals).text("Partially / UnPosted").attr("selected", "selected"));
                Bill_ERA.LoadDefaultData();
                Bill_ERA.ERASearch();
            });
        }
    },

    LoadDefaultData: function () {


        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#" + Bill_ERA.params["PanelID"] + " input#txtFacility");
            var hfCtrl = $("#" + Bill_ERA.params["PanelID"] + " #hfFacility");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });

        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            $("#" + Bill_ERA.params["PanelID"] + " input#txtPractice").autocomplete({
                autoFocus: true,
                source: Practices, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Bill_ERA.params["PanelID"] + " #hfPractice").val(ui.item.id); // add the selected id
                        if ($("#" + Bill_ERA.params["PanelID"] + " #lnkPracticeEdit").css("display") == "none") {
                            $("#" + Bill_ERA.params["PanelID"] + " #lnkPracticeEdit").css("display", "inline");
                            $("#" + Bill_ERA.params["PanelID"] + " #lblPractice").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        //utility.CreateDatePicker('pnlBillERA #frmBillERA #dpFrmEntryDate,#dpToEntryDate', function (ev) { }, false);
        utility.ValidateFromToDate('frmBillERA', 'dpFrmEntryDate', 'dpToEntryDate', true);
    },

    Bill_ERAReset: function () {
       // $('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA').resetAllControls();
        $('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        $('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA #dpToEntryDate').attr('disabled', true);
        if ($('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA #lnkFacilityEdit').is(":visible")) {
            $('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA #lnkFacilityEdit').hide();
            $('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA #lblFacility').show();
        }
         $('#' + Bill_ERA.params["PanelID"] + ' #frmBillERA input[type=number]').val("");
    },

    ERAAdd: function (mode, ERAId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Emergency Contact", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {
                var params = [];
                params["ERAId"] = null;
                params["PatientID"] = Bill_ERA.params.patientID;
                params["mode"] = mode;
                params["ParentCtrl"] = 'Bill_ERA';
                LoadActionPan('emergencyContactDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ERASearch: function (ERAId, PageNo, rpp) {
        AppPrivileges.GetFormPrivileges("ERA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Bill_ERA.params["PanelID"] + " #pnlBillERA_Result").css("display") == "none") {
                    $('#' + Bill_ERA.params["PanelID"] + " #pnlBillERA_Result").show();
                }
                var self = $('#' + Bill_ERA.params["PanelID"]);
                var myJSON = self.getMyJSONByName();
                Bill_ERA.SearchERA(myJSON, ERAId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        //-----------------Pagination------------

                        if (response.ERACount > 0) {
                            $('#' + Bill_ERA.params["PanelID"] + " #divERAPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divERAPaging", response.iTotalDisplayRecords, 5, "Bill_ERA", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $('#' + Bill_ERA.params["PanelID"] + " #divERAPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $('#' + Bill_ERA.params["PanelID"] + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $('#' + Bill_ERA.params["PanelID"] + " #divERAPaging").css("display", "none");
                        }

                        //--------------------End Pagination-------------------
                        Bill_ERA.ERAGridLoad(response);
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

    ERAGridLoad: function (response) {
        $('#' + Bill_ERA.params["PanelID"] + " #pnlBillERA_Result #dgvBillERA").dataTable().fnDestroy();
        $('#' + Bill_ERA.params["PanelID"] + " #pnlBillERA_Result #dgvBillERA tbody").find("tr").remove();
        if (response.ERACount > 0) {// For Temporary Purpose we need to remove this check when data will be available
            $('#' + Bill_ERA.params["PanelID"] + " #divERATotalAmount").css("display", "inline");
            var ERALoadJSONData = JSON.parse(response.ERALoad_JSON);
           // var TotalAmount = 0;
           // var totalPostedAmount = 0;
            $.each(ERALoadJSONData, function (i, item) {
                var Edit = "Edit";
                var $row = $('<tr/>');
                $row.attr("onclick", "Bill_ERA.OpenERADetail('" + Edit + "','" + item.ERAId + "','" + item.EDIReportId + "','" + item.CheckNo + "',event);utility.SelectGridRow($(this));");
                $row.attr("id", "dgvBillERA_row" + item.ERAId);
                $row.attr("ERAId", item.ERAId);

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

                //TotalAmount = TotalAmount + Number(item.CheckAmount);
               //totalPostedAmount = totalPostedAmount + Number(item.PostedAmount);
                var PaymentPostingMethod = "Bill_ERA.PostPayment('" + item.Status + "','" + item.LinkedCharges + "','" + item.ERAId + "','" + item.CheckNo + "','" + item.LinkedRecoupmentCharges + "','" + item.PLBAdjustmentsCount + "','" + item.EDIReportId + "',event)";

                //&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_ERA.ActiveInactiveERA(\'' + item.ERAId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;
                //syed zia, bug#PMS-4780, change tool tip
                $row.append('<td style="display:none;">' + item.ERAId + '</td><td><a class="btn  btn-xs" href="#" onclick="Bill_ERA.ERADelete(\'' + item.ERAId + '\',\'' + item.CheckNo + '\',event);" title="Delete Record" data-toggle="tooltip" data-placement="right"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_ERA.OpenERADetail(\'' + Edit + '\', \'' + item.ERAId + '\', \'' + item.EDIReportId + '\', \'' + item.CheckNo + '\',event);"  title="Edit Record" data-toggle="tooltip" data-placement="right"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs " href="#" onclick="' + PaymentPostingMethod + '" title="Post Payment" data-toggle="tooltip" data-placement="right"><i class="fa fa-dollar green"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_ERA.OpenViewERA(\'' + item.EDIReportId + '\',\'' + item.CheckNo + '\',event);" title="View ERA" data-toggle="tooltip" data-placement="right"><i class="fa fa-eye black"></i></a></td><td>' + item.PayerName + '</td><td>' + item.CheckNo + '</td><td class="text-right">' + utility.convertToFigure(item.CheckAmount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.AppliedAmount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PostedAmount, true) + '</td><td>' + item.Status + '</td><td>' + item.PayeeName + '</td><td>' + item.ClearingHouse + '</td><td>' + Number(item.UnLinkedCharges) + '</td><td>' + Number(item.LinkedRecoupmentCharges) + '</td><td>' + item.CreatedOn + '</td>');
                //syed zia, bug#PMS-4780, change tool tip

                $('#' + Bill_ERA.params["PanelID"] + " #pnlBillERA_Result #dgvBillERA tbody").last().append($row);
                $('#' + Bill_ERA.params["PanelID"] + ' #totalAmt').text(utility.convertToFigure(item.TotalCheckAmount, true));
                $('#' + Bill_ERA.params["PanelID"] + ' #totalPostedAmt').text(utility.convertToFigure(item.TotalPostedAmount, true));
            });
           
            
        }
        else {
            $('#' + Bill_ERA.params["PanelID"] + ' #totalAmt').text(utility.convertToFigure(0, true));
            $('#' + Bill_ERA.params["PanelID"] + ' #totalPostedAmt').text(utility.convertToFigure(0, true));

            $('#' + Bill_ERA.params["PanelID"] + " #divERAPaging").css("display", "none");
            $('#' + Bill_ERA.params["PanelID"] + " #divERATotalAmount").css("display", "none");
            $('#pnlBillERA_Result #dgvBillERA').DataTable({
                "language": {
                    "emptyTable": "No ERA Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlBillERA_Result #dgvBillERA'))
            ;
        else
            $("#pnlBillERA_Result #dgvBillERA").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": []  /*"aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] */ }); // to remove records per page dropdown


        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    PostPayment: function (status, linkedcharges, eraid, checkNo, LinkedRecoupmentCharges, PLBAdjustmentsCount, EDIReportId, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var allocation_count = Number(PLBAdjustmentsCount) + Number(LinkedRecoupmentCharges);
        var OpenDefaultTab = "";
        if (LinkedRecoupmentCharges > 0)
            OpenDefaultTab = "a_LinkedRecoupmentCharges";
        else if (PLBAdjustmentsCount > 0)
            OpenDefaultTab = "a_PLBSegment";

      

        var _message = "";
        if (linkedcharges <= 0)
            _message = "Couldn't found any linked charge to post payment.";
        else if (status.toLowerCase() == "posted")
            _message = "This payment is already posted.";

        if (_message == "") {

            utility.myConfirm("13", function () {

                AppPrivileges.GetFormPrivileges("ERA", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

                    if (strMessage == "") {

                        var objDeffered = $.Deferred();
                        Bill_PaymentPosting.IsCheckPosted(checkNo).done(function (response) {

                            if (response.Message != "") {
                                utility.myConfirm(response.Message, function () {
                                    objDeffered.resolve("ok");
                                }, function () { }, "Confirm Duplicate Payment");
                            }
                            else {
                                objDeffered.resolve("ok");
                            }

                        });

                        objDeffered.done(function (message) {
                            ERADetail.IsVoidedClaimExist(eraid).done(function (result) {
                                if (result.status != false && result.VoidedClaims) {
                                    var VoidedClaimNumber = result.VoidedClaims;
                                    utility.myConfirm("The claim/s " + VoidedClaimNumber.substring(0, VoidedClaimNumber.length - 1) + " are voided please link ERA with new claims. Do you want to proceed?", function () {
                                        Bill_ERA.PostERAPayment(allocation_count, OpenDefaultTab, status, linkedcharges, eraid, checkNo, LinkedRecoupmentCharges, PLBAdjustmentsCount, EDIReportId, event);
                                    }, function () {

                                    }, "Voided Claims Alert");
                                }
                                else {
                                    Bill_ERA.PostERAPayment(allocation_count, OpenDefaultTab, status, linkedcharges, eraid, checkNo, LinkedRecoupmentCharges, PLBAdjustmentsCount, EDIReportId, event);
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
            utility.DisplayMessages(_message, 2);
        }
    },

               
    PostERAPayment: function (allocation_count, OpenDefaultTab, status, linkedcharges, eraid, checkNo, LinkedRecoupmentCharges, PLBAdjustmentsCount, EDIReportId, event)
    {
        var IsPaginataion = true;
        //No pagination required when Open  ERA Detail for PLB or Recoupment Alert.
        ERADetail.Post_Payment(eraid).done(function (response) {
            if (response.status != false) {
                if (response.IsMessage == true)
                    utility.DisplayMessages(response.Message, 1);

                Bill_ERA.ERASearch();

                if (response.UnPostedChargesCount > 0) {

                    if (allocation_count > 0) {
                        utility.myConfirm("There are recoupments in the ERA would like to allocate them?", function () {
                            //No pagination required when Open  ERA Detail for PLB or Recoupment Alert.
                            IsPaginataion = false;
                            //Open ERA Detail Screen.
                            Bill_ERA.OpenERADetail('Edit', eraid, EDIReportId, checkNo, event, OpenDefaultTab, IsPaginataion);
                            ERADetail.OpenERASummary(eraid, response.UnPostedChargesCount, response.UnPostedCharges_JSON);

                        }, function () {
                            Bill_ERA.OpenERASummary(eraid, response.UnPostedChargesCount, response.UnPostedCharges_JSON);
                        }, "Recoupments Allocation Alert");
                    }
                    else
                        Bill_ERA.OpenERASummary(eraid, response.UnPostedChargesCount, response.UnPostedCharges_JSON);
                }
                else if (allocation_count > 0) {

                    utility.myConfirm("There are recoupments in the ERA would like to allocate them?", function () {

                        //No pagination required when Open  ERA Detail for PLB or Recoupment Alert.
                        IsPaginataion = false;
                        //Open ERA Detail Screen.
                        Bill_ERA.OpenERADetail('Edit', eraid, EDIReportId, checkNo, event, OpenDefaultTab, IsPaginataion)
                    }, function () { }, "Recoupments Allocation Alert");

                }


            } else {

                utility.DisplayMessages(response.Message, 3);
            }
        });
    
    },
    ERADelete: function (ERAId, CheckNo, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvBillERA_row' + ERAId));
        AppPrivileges.GetFormPrivileges("ERA", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Do you want to delete <b>' + CheckNo + '</b> ERA?', function () {
                    var selectedValue = ERAId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Bill_ERA.DeleteERA(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#pnlBillERA_Result #dgvBillERA').DataTable();
                                table1.row('.active').remove().draw(false);
                                Bill_ERA.ERASearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    'Confirm Delete'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ActiveInactiveERA: function (ERAId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("ERA", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ERAId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Bill_ERA.ERAUpdateActiveInactive(ERAId, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Bill_ERA.ERASearch();
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



    ERAUpdateActiveInactive: function (ERAId, IsActive) {

        var objData = new JSON.constructor();

        objData["ERAID"] = ERAId;
        objData["IsActive"] = IsActive;
        objData["CommandType"] = "era_update_active_inactive";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERA");
    },
    /*Added "EDIReportId" by Azeem Raza Tayyab to Implement CR:"PMS-942" on 01-Nov-2016*/
    Download_ERA: function (EDIReportId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ERA", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Bill_ERA.DownloadERA(EDIReportId).done(function (response) {
                    if (response.status != false) {

                        Bill_ERA.ERASearch();
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

    },

    DownloadERA: function (EDIReportId) {

        if (!EDIReportId)
            EDIReportId = 0;

        var objData = new JSON.constructor();

        objData["EDIReportID"] = EDIReportId;
        objData["CommandType"] = "download_era";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERA");
    },

    DeleteERA: function (ERAId) {

        var objData = new JSON.constructor();

        objData["ERAID"] = ERAId;
        objData["CommandType"] = "delete_era";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERA");
    },

    SearchERA: function (ERAData, ERAId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var objData = new JSON.constructor();
        if (ERAData)
            objData = JSON.parse(ERAData);

        objData["ERAID"] = ERAId;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "search_era";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERA");
    },

    UnLoadTab: function (Tab) {

        if (Bill_ERA.params["ParentCtrl"] == "Bill_ERA") {

            UnloadActionPan("Bill_ERA");

        }

        else {
            RemoveAdminTab(Tab);
        }

    },

    OpenERADetail: function (mode, ERAId, EDIReportId, CheckNo, event, OpenDefaultTab, IsPaginataion) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["mode"] = mode;
        params["ERAId"] = ERAId;
        params["EDIReportId"] = EDIReportId;
        params["CheckNo"] = CheckNo;
        params["OpenDefaultTab"] = (OpenDefaultTab == "" || OpenDefaultTab == "undefined" || OpenDefaultTab == null) ? null : OpenDefaultTab;
        params["IsPaginataion"] = (IsPaginataion == "" || IsPaginataion == "undefined" || IsPaginataion == null) ? true : IsPaginataion;
        params["ParentCtrl"] = "billTabERA";
        LoadActionPan('ERADetail', params);

    },

    OpenERASummary: function (ERAId, UnPostedChargesCount, UnPostedCharges_JSON) {
        var params = [];
        params["mode"] = "Edit";
        params["ERAId"] = ERAId;
        params["UnPostedCharges_JSON"] = UnPostedCharges_JSON;
        params["UnPostedChargesCount"] = UnPostedChargesCount;
        params["ParentCtrl"] = "billTabERA";
        LoadActionPan('Bill_ERA_Summary', params);
    },

    OpenViewERA: function (EDIReportId, CheckNo, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["EDIReportId"] = EDIReportId;
        params["CheckNo"] = CheckNo;
        params["ParentCtrl"] = 'billTabERA';
        LoadActionPan('EDIReviewReport', params);

    },

    //--------Facility-----
    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabERA";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_ERA.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'billTabERA';
        LoadActionPan('facilityDetail', params);
    },
    //-----------End Facility-------------

    //-----------------Practice---------------

    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabERA";
        LoadActionPan('Admin_Practice', params);
    },

    OpenPracticeDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["PracticeId"] = $('#' + Bill_ERA.params.PanelID + ' #hfPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPractice";
        params["ParentCtrl"] = 'billTabERA';
        LoadActionPan('practiceDetail', params);
    },

    //-------------End Practice-------------
    //--------Pagination Functions--------

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlBillERA_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_ERA.ERASearch(0, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillERA_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_ERA.ERASearch(0, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlBillERA_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_ERA.ERASearch(0, currentPageNo, 15);
        }
    },
}