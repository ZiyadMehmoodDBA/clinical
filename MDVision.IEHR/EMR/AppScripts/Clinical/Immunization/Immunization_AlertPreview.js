Immunization_AlertPreview = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Immunization_AlertPreview.params = params;
        //if (Immunization_AlertPreview.params.PanelID != 'pnlImmunization_AlertPreview') {
        //    Immunization_AlertPreview.params.PanelID = Immunization_AlertPreview.params.PanelID + ' #pnlImmunization_AlertPreview';
        //} else {
        Immunization_AlertPreview.params.PanelID = 'pnlImmunization_AlertPreview';
        //}

        if ($('#PatientProfile #hfPatientId').val() != "") {

            Immunization_AlertPreview.params.patientID = $('#PatientProfile #hfPatientId').val();
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            Immunization_AlertPreview.SearchAlerts();
            if ($("#" + Immunization_AlertPreview.params.PanelID + " #listSchedulerAlert").hasClass("hidden")) {
                $("#" + Immunization_AlertPreview.params.PanelID + " #listSchedulerAlert").removeClass("hidden");
            }

            $('#' + Immunization_AlertPreview.params.PanelID).find('[id*="list"]').removeClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID).find('[id*="tab"]').removeClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID + ' #listSchedulerAlert').addClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID + ' #tabSchedulerAlert').addClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID + ' #listSchedulerAlert').click();

        }
        else {
            if (!$("#" + Immunization_AlertPreview.params.PanelID + " #listSchedulerAlert").hasClass("hidden")) {
                $("#" + Immunization_AlertPreview.params.PanelID + " #listSchedulerAlert").addClass("hidden");
            }

            $('#' + Immunization_AlertPreview.params.PanelID).find('[id*="list"]').removeClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID).find('[id*="tab"]').removeClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID + ' #listExpiryLot').addClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID + ' #tabExpiryLot').addClass('active');
            $('#' + Immunization_AlertPreview.params.PanelID + ' #listExpiryLot').click();
        }

        Immunization_AlertPreview.SearchExpiredLot(null, 1, 15, true, false);
        Immunization_AlertPreview.SearchExpiredLot(null, 1, 15, false, true);
        //$("#" + Immunization_AlertPreview.params.PanelID + " #listSchedulerAlert").addClass("hidden");
        //if(){

        //}
    },
    unLoadTab: function () {
        if (Immunization_AlertPreview.params["FromAdmin"] == "0") {
            if (Immunization_AlertPreview.params != null && Immunization_AlertPreview.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_AlertPreview.params.ParentCtrl, 'Immunization_AlertPreview');
            } else
                UnloadActionPan(null, 'Immunization_AlertPreview');
        } else {
            RemoveAdminTab();
        }
    },
    SearchExpiredLot: function (PrimaryID, PageNo, rpp, onlyExpired, OnlyLowQuantity) {

        if (onlyExpired == null && OnlyLowQuantity == null) {

            if ($("#" + Immunization_AlertPreview.params.PanelID + " #listExpiryLot").hasClass('active'))
            {
                onlyExpired = true; OnlyLowQuantity = false;
            }
            else if ($("#" + Immunization_AlertPreview.params.PanelID + " #listLowQtyLot").hasClass('active'))
            {
                onlyExpired = false; OnlyLowQuantity = true;
            }

        }


        var GridPnlId = "";
        var GriddgvId = "";
        var GridPagingId = "";
        if (onlyExpired) {
            GridPnlId = "pnlExpiryLot_Result";
            GriddgvId = "dgvExpiryLot";
            GridPagingId = "divExpiryLotPaging";
        }
        else if (OnlyLowQuantity) {
            GridPnlId = "pnlLowQtyLot_Result";
            GriddgvId = "dgvLowQtyLot";
            GridPagingId = "divLowQtyLotPaging";
        }


        var dfd = $.Deferred();
        if ($("#" + Immunization_AlertPreview.params.PanelID + " #" + GriddgvId).css("display") == "none") {
            $("#" + Immunization_AlertPreview.params.PanelID + " #" + GriddgvId).show();
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Immunization_AlertPreview.SearchLot_Call(PageNo, rpp, onlyExpired, OnlyLowQuantity).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Immunization_AlertPreview.ImmunizationLotGridLoad(response, onlyExpired, OnlyLowQuantity);
                        var TableControl = Immunization_AlertPreview.params.PanelID + " #" + GriddgvId;
                        var PagingPanelControlID = Immunization_AlertPreview.params.PanelID + " #" + GridPagingId;
                        var ClassControlName = "Immunization_AlertPreview";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.LotNumberCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            var pid = PrimaryID; var pn = PageNumber; var rpp = ResultPerPage;
                            Immunization_AlertPreview.SearchExpiredLot(pid, pn, rpp,null,null);
                        }), 10);
                        dfd.resolve();
                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        return dfd;
    },
    SearchAlerts: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();
        if ($("#" + Immunization_AlertPreview.params.PanelID + " #dgvImmunizationAlerts").css("display") == "none") {
            $("#" + Immunization_AlertPreview.params.PanelID + " #dgvImmunizationAlerts").show();
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Immunization_AlertPreview.SearchAlertsDB_Call(PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Immunization_AlertPreview.ImmunizationAlertGridLoad(response);
                        var TableControl = Immunization_AlertPreview.params.PanelID + " #dgvImmunizationAlerts";
                        var PagingPanelControlID = Immunization_AlertPreview.params.PanelID + " #divImmunizationAlertsPaging";
                        var ClassControlName = "Immunization_AlertPreview";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ImmunizationAlertCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Immunization_AlertPreview.SearchAlerts(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                        dfd.resolve();
                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        return dfd;
    },
    ImmunizationAlertGridLoad: function (response) {
        $("#" + Immunization_AlertPreview.params.PanelID + " #dgvImmunizationAlerts").dataTable().fnDestroy();
        $("#" + Immunization_AlertPreview.params.PanelID + " #pnlImmunizationAlerts_Result #dgvImmunizationAlerts tbody").find("tr").remove();
        $("#" + Immunization_AlertPreview.params.PanelID + " #spnListSchedulerAlertCount").html("");
        if (response.ImmunizationAlertCount > 0) {
            $("#" + Immunization_AlertPreview.params.PanelID + " #spnListSchedulerAlertCount").html(response.ImmunizationAlertCount);
            var ImmunizationAlertLoadJSONData = JSON.parse(response.ImmunizationAlert_Json);
            $.each(ImmunizationAlertLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                var AddParameters = "'Add','" + item.Category + "'," + item.VaccineScheduleId + ",'Alert'";

                $row.attr("onclick", "Immunization_AlertPreview.ClickOnFromAlertGrid(" + AddParameters + ")");
                $row.attr("id", item.AlertId);
                // $row.attr("", item.NotesId);
                var AlertColor = "text-danger";
                if (item.Alert == "Due") {
                    AlertColor = "text-warning";
                }
                $row.append('<td style="display:none;">' + item.AlertId + '</td><td>' + item.VaccineName + '</td><td class="' + AlertColor + '"><span style="font-weight:bold">' + item.Alert + '</span></td><td>' + item.NoOfDays + '</td><td>' + item.DueDate + '</td>');
                //<a class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesActiveInactive(' + item.NotesId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>
                $("#" + Immunization_AlertPreview.params.PanelID + " #dgvImmunizationAlerts tbody").last().append($row);
            });
        }
        else {

            $("#" + Immunization_AlertPreview.params.PanelID + " #dgvImmunizationAlerts").DataTable({
                "language": {
                    "emptyTable": "No Immunization Alert Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Immunization_AlertPreview.params.PanelID + " #dgvImmunizationAlerts"))
            ;
        else {
            $("#" + Immunization_AlertPreview.params.PanelID + " #pnlImmunizationAlerts_Result #dgvImmunizationAlerts").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
    },
    ImmunizationLotGridLoad: function (response, onlyExpired, OnlyLowQuantity) {
        var GridPnlId = "";
        var GriddgvId = "";
        var GridPagingId = "";
        var countSpan = "";
        var NoRecordMessage = "";
        if (onlyExpired) {
            GridPnlId = "pnlExpiryLot_Result";
            GriddgvId = "dgvExpiryLot";
            GridPagingId = "divExpiryLotPaging";
            countSpan = "spnListExpiryLotCount";
            NoRecordMessage = "No Exired Lot Found";
        }
        else if (OnlyLowQuantity) {
            GridPnlId = "pnlLowQtyLot_Result";
            GriddgvId = "dgvLowQtyLot";
            GridPagingId = "divLowQtyLotPaging";
            countSpan = "spnListLowQtyCount";
            NoRecordMessage = "No Low Quantity Lot Found";
        }

        $("#" + Immunization_AlertPreview.params.PanelID + " #" + GriddgvId).dataTable().fnDestroy();
        $("#" + Immunization_AlertPreview.params.PanelID + " #" + GridPnlId + " #" + GriddgvId + " tbody").find("tr").remove();
        $("#" + Immunization_AlertPreview.params.PanelID + " #" + countSpan).html("");
        if (response.LotNumberCount > 0) {
            $("#" + Immunization_AlertPreview.params.PanelID + " #" + countSpan).html(response.LotNumberCount);
            var LotNumberLoad_JSON = JSON.parse(response.LotNumberLoad_JSON);
            $.each(LotNumberLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.VaccineLotNoId);
                $row.attr("VaccineLotNoId", item.VaccineLotNoId);

                //var AddParameters = "'Add','" + item.Category + "'," + item.VaccineScheduleId + ",'Alert'";

                //$row.attr("onclick", "Immunization_AlertPreview.ClickOnFromAlertGrid(" + AddParameters + ")");

                $row.append('<td style="display:none;">' + item.VaccineLotNoId + '</td><td>' + item.LotNo + '</td><td>' + item.VaccineName.split('|').join('<br>') + '</td><td>' + item.ManufacturerName + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td>' + item.Quantity + '</td><td>' + item.QuantityLeft + '</td><td>' + (item.Type == "TherapueticInjection" ? "Therapuetic Injection" : item.Type) + '</td>');
                //<a class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesActiveInactive(' + item.NotesId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>
                $("#" + Immunization_AlertPreview.params.PanelID + " #" + GriddgvId + " tbody").last().append($row);
            });
        }
        else {

            $("#" + Immunization_AlertPreview.params.PanelID + " #" + GriddgvId).DataTable({
                "language": {
                    "emptyTable": NoRecordMessage
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Immunization_AlertPreview.params.PanelID + " #" + GriddgvId))
            ;
        else {
            $("#" + Immunization_AlertPreview.params.PanelID + " #" + GridPnlId + " #" + GriddgvId).DataTable({
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            }); // to remove records per page dropdown
        }
    },


    ClickOnFromAlertGrid: function (Mode, VaccineCategoryId, VaccineScheduleId, TabId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var PanelID = "";


                params["ParentCtrl"] = 'Immunization_AlertPreview';
                PanelID = Immunization_AlertPreview.params.PanelID;
                params["from"] = 'Immunization_AlertPreview';
                params["PanelID"] = Immunization_AlertPreview.params.PanelID;
                params["ParentPanelID"] = Immunization_AlertPreview.params.PanelID;



                // params["VaccineHxId"] = vaccineHxId;

                params["FromAdmin"] = 0;
                params["VaccineScheduleId"] = VaccineScheduleId;
                params["CategoryId"] = VaccineCategoryId;

                //if (Mode == "Edit") {
                //    params["VaccineHxId"] = VaccineHxId;
                //    params["Type"] = Type;
                //}

                params["TabId"] = TabId;
                params["mode"] = Mode;
                params["patientID"] = Immunization_AlertPreview.params.patientID;
                LoadActionPan('Clinical_ImmunizationDetail', params, PanelID);
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenImnunizationAlertPrintPage: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = 'Immunization_AlertPreview';
                params["PanelID"] = Immunization_AlertPreview.params.PanelID;
                params["PatientId"] = Immunization_AlertPreview.params.patientID;
                params["FromAdmin"] = "0";
                LoadActionPan('Immunization_AlertPrint', params);
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    SearchAlertsDB_Call: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_IMMUNIZATION_ALERTS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Immunization");
    },
    SearchLot_Call: function (PageNumber, RowsPerPage, onlyExpired, OnlyLowQuantity) {
        var objData = new Object();
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        objData["OnlyExpired"] = onlyExpired;
        objData["OnlyLowQuantity"] = OnlyLowQuantity;
        objData["Active"] = true;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["Type"] = "";
        objData["commandType"] = "get_all_lotnumber";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },
}