Immunization_LotNumber = {
    AddOrUpdateLot: false,
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Immunization_LotNumber.AddOrUpdateLot = false;
        Immunization_LotNumber.params = params;
        Immunization_LotNumber.params.mode = "Add";
        Immunization_LotNumber.Type = "Incoming";
        if (Immunization_LotNumber.params.PanelID != 'pnlImmunization_LotNumber') {
            Immunization_LotNumber.params.PanelID = Immunization_LotNumber.params.PanelID + ' #pnlImmunization_LotNumber';
        } else {
            Immunization_LotNumber.params.PanelID = 'pnlImmunization_LotNumber';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Immunization_LotNumber.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        var self = $('#' + Immunization_LotNumber.params.PanelID);
        if (Immunization_LotNumber.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {

                if (typeof Immunization_LotNumber.params.ParentCtrl != typeof undefined && Immunization_LotNumber.params.ParentCtrl != null && (Immunization_LotNumber.params.ParentCtrl == "Clinical_ImmunizationDetail" || Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection")) {
                    AppPrivileges.GetFormPrivileges("Immunization_Lot Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            $("#" + Immunization_LotNumber.params.PanelID + " #AddLot").show();
                        }
                    });
                }
                else {
                    $("#" + Immunization_LotNumber.params.PanelID + " #AddLot").show();
                }

            });
            Immunization_LotNumber.LoadLotNumberList();
        }

    },
    AddLotNumber: function () {
        var params = [];
        params["ParentCtrl"] = "Immunization_LotNumber";
        params["FromAdmin"] = 0;
        params["patientID"] = Immunization_LotNumber.params["patientID"];
        LoadActionPan("Immunization_LotTypeSelection", params);
    },
    //AddLotNumber: function () {
    //    var params = [];
    //    params["ParentCtrl"] = "Immunization_LotNumber";
    //    params["FromAdmin"] = 0;
    //    params["mode"] = "Add";
    //    params["PatientId"] = Immunization_LotNumber.params["patientID"];
    //    params["FromAdmin"] = Immunization_LotNumber.params["FromAdmin"];
    //    if (Immunization_LotNumber.params["ParentCtrl"] == "Clinical_ImmunizationDetail") {
    //        params["VaccineId"] = Immunization_LotNumber.params["VaccineId"];
    //        params["VaccineText"] = Immunization_LotNumber.params["VaccineText"];
    //        params["Type"] = 'vaccine';
    //    }
    //    else if (Immunization_LotNumber.params["ParentCtrl"] == "Immunization_TherapeuticInjection") {
    //        params["TherapeuticInjectionId"] = Immunization_LotNumber.params.TherapeuticInjectionId;
    //        params["TherapeuticInjectionText"] = Immunization_LotNumber.params.TherapeuticInjectionText;
    //        params["Type"] = 'TherapeuticInjection';
    //    }
    //    // params["ParentCtrl"] = Admin_BillingProvider.params["ParentCtrl"];
    //    if (Immunization_LotNumber.params["FromAdmin"] == "0") {
    //        params["ParentCtrl"] = 'Immunization_LotNumber';
    //    }
    //    LoadActionPan("Immunization_LotNumberDetail", params);
    //},
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_LotNumber.params.ParentCtrl == "Clinical_ImmunizationDetail" && Immunization_LotNumber.AddOrUpdateLot == true) {
            Immunization_LotNumber.AddOrUpdateLot = false;
            $.when(Clinical_ImmunizationDetail.RefreshLotDetail()).then(function () {
                if (Immunization_LotNumber.params["FromAdmin"] == "0") {
                    if (Immunization_LotNumber.params != null && Immunization_LotNumber.params.ParentCtrl != null) {
                        UnloadActionPan(Immunization_LotNumber.params.ParentCtrl, 'Immunization_LotNumber');
                    }
                    else
                        UnloadActionPan(null, 'Immunization_LotNumber');
                }
                else {

                    RemoveAdminTab();
                }
            });
        }
        else if (Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection" && Immunization_LotNumber.AddOrUpdateLot == true) {
            Immunization_LotNumber.AddOrUpdateLot = false;
            $.when(Immunization_TherapeuticInjection.RefreshLotDetail()).then(function () {
                if (Immunization_LotNumber.params["FromAdmin"] == "0") {
                    if (Immunization_LotNumber.params != null && Immunization_LotNumber.params.ParentCtrl != null) {
                        UnloadActionPan(Immunization_LotNumber.params.ParentCtrl, 'Immunization_LotNumber');
                    }
                    else
                        UnloadActionPan(null, 'Immunization_LotNumber');
                }
                else {

                    RemoveAdminTab();
                }
            });
        }
        else {
            if (Immunization_LotNumber.params["FromAdmin"] == "0") {
                if (Immunization_LotNumber.params != null && Immunization_LotNumber.params.ParentCtrl != null) {
                    UnloadActionPan(Immunization_LotNumber.params.ParentCtrl, 'Immunization_LotNumber');
                }
                else
                    UnloadActionPan(null, 'Immunization_LotNumber');
            }
            else {

                RemoveAdminTab();
            }
        }

        return objDeffered;
    },
    LoadLotNumberList: function (VaccineLotNoId, PageNo, rpp) {
        Immunization_LotNumber.SearchLotNumber(VaccineLotNoId, PageNo, rpp, 1).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Immunization_LotNumber.LotNumberGridLoad(response);
                var TableControl = Immunization_LotNumber.params.PanelID + " #pnlLotNumber_Result #dgvLotNumer";
                var PagingPanelControlID = Immunization_LotNumber.params.PanelID + " #divLotNumber_Paging";
                var ClassControlName = "Immunization_LotNumber";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;

                setTimeout(CreatePagination(response.LotNumberCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNo, rpp) {
                    Immunization_LotNumber.LoadLotNumberList(PrimaryID, PageNo, rpp);
                }), 10);
                setTimeout(function () {
                    if (Immunization_LotNumber.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Immunization_LotNumber.params.PanelID + "  #divLotNumber_Paging #btnAddImmunizationToNotes").length == 0) {
                        $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Immunization.addImmunizationToNotes();" disabled id="btnAddImmunizationToNotes">Add on Note</button>').insertAfter("#" + Immunization_LotNumber.params.PanelID + "  #divLotNumber_Paging .pagination")
                    }
                }, 11);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SearchLotNumber: function (VaccineLotNoId, PageNumber, RowsPerPage, IsActive) {
        var objData = new Object();
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        objData["VaccineLotNoId"] = VaccineLotNoId;
        objData["Active"] = $('#' + Immunization_LotNumber.params.PanelID + ' #frmImmunization_LotNumber #divSwitch #switchActive').is(':checked');
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["Type"] = "";

        objData["commandType"] = "get_all_lotnumber";
        if (Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection") {
            objData["ProviderId"] = Immunization_LotNumber.params.ProviderId;
            objData["Type"] = "TherapueticInjection";
            objData["TherapeuticInjectionId"] = Immunization_LotNumber.params.TherapeuticInjectionId;
        }
        else if (Immunization_LotNumber.params.ParentCtrl == "Clinical_ImmunizationDetail") {
            objData["ProviderId"] = Immunization_LotNumber.params.ProviderId;
            objData["Type"] = "Vaccine";
            objData["VaccineId"] = Immunization_LotNumber.params.VaccineId;
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },
    LotNumberGridLoad: function (response) {
        var dfds = [];
        var editDefer = $.Deferred();
        var delDefer = $.Deferred();
        dfds = [editDefer, delDefer]
        var editPermission = false;
        var delPermission = false;
        AppPrivileges.GetFormPrivileges("Immunization_Lot Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                editPermission = true;
            }
            editDefer.resolve();
        });
        AppPrivileges.GetFormPrivileges("Immunization_Lot Management", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                delPermission = true;
            }
            delDefer.resolve();
        });
        $.when.apply($, dfds).then(function () {
            if (Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection") {
                $('#' + Immunization_LotNumber.params.PanelID + ' #pnlLotNumber_Result #dgvLotNumer th[name="vaccineName"]').html("Therapeutic Injection");
            }

            //var gridLotNumbeControls = $('#' + Immunization_LotNumber.params.PanelID + ' #gridLotNumbeControls').html();
            //var gridLotNumbeControls = '<div class="col-sm-2 pt-md" id="AddLot">' +
            //                                '<button type="button" class="btn btn-sm btn-success" onclick="Immunization_LotNumber.AddLotNumber()"><i class="fa fa-plus-circle"></i> Add</button>' +
            //                           '</div>' +
            //                           '<div class="col-sm-4 pt-md" id="divSwitch">' +
            //                           '<span class="pr-xs">Inactive</span>' +
            //                           '<div class="btnWidgetSwitch switch switch-xs switch-success">' +
            //                           '    <input id="switchActive" isactive="1" type="checkbox" checked="checked" data-plugin-ios-switch="" style="display: none;" onchange="  Immunization_LotNumber.LoadLotNumberList();">' +
            //                           ' </div>' +
            //                           ' <span class="pl-xs">Active</span>' +
            //                           '</div>';

            var gridLotNumbeControlsChecked = $('#' + Immunization_LotNumber.params.PanelID + ' #gridLotNumbeControls input[type=checkbox]').is(':checked')
            //$('#' + Immunization_LotNumber.params.PanelID + ' #gridLotNumbeControls').remove();


            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == "dgvLotNumer")
                    $(this).dataTable().fnDestroy();
            });

            $('#' + Immunization_LotNumber.params.PanelID + " #pnlLotNumber_Result #dgvLotNumer tbody").find("tr").remove();
            if (response.LotNumberCount > 0) {
                var LotNumberDataJSONData = JSON.parse(response.LotNumberLoad_JSON);
                $.each(LotNumberDataJSONData, function (i, item) {



                    var $row = $('<tr/>');
                    $row.attr("id", item.VaccineLotNoId);
                    $row.attr("VaccineLotNoId", item.VaccineLotNoId);
                    var EditMethod = "Immunization_LotNumber.EditLotNumber(" + item.VaccineLotNoId + ",event,'" + item.Type + "');"

                    var selectLotNumber = "";
                    var selectMethod = ""
                    var ClassDisabled = item.Active == "True" ? "" : "disabled";
                    if (Immunization_LotNumber.params["FromAdmin"] == "0") {

                        var exp_date = item.ExpiryDate ? utility.RemoveTimeFromDate(null, item.ExpiryDate) : "-1";
                        var RouteId = item.RouteId ? item.RouteId : "-1";

                        if (item.Type == "TherapueticInjection") {
                            selectMethod = "Immunization_LotNumber.SelectLotNumber(" + item.VaccineLotNoId + "," + item.TherapeuticInjectionId + ",'" + exp_date + "'," + RouteId + ",event," + item.QuantityLeft + ",'" + item.Type + "','" + item.IsExpired + "');"
                            selectLotNumber = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                            if (typeof Immunization_LotNumber.params.ParentCtrl != typeof undefined && Immunization_LotNumber.params.ParentCtrl != null && (Immunization_LotNumber.params.ParentCtrl == "Clinical_ImmunizationDetail" || Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection")) {

                            }
                            else {
                                $row.attr("onclick", selectMethod);
                            }
                        }
                        else {
                            selectMethod = "Immunization_LotNumber.SelectLotNumber(" + item.VaccineLotNoId + "," + Immunization_LotNumber.params.VaccineId + ",'" + exp_date + "'," + RouteId + ",event," + item.QuantityLeft + ",'" + item.Type + "','" + item.IsExpired + "');"
                            selectLotNumber = "&nbsp;<a class='btn  btn-xs " + ClassDisabled + "' onclick=" + selectMethod + " title='Select Record'><i class='fa fa-check black'></i></a>";
                            if (typeof Immunization_LotNumber.params.ParentCtrl != typeof undefined && Immunization_LotNumber.params.ParentCtrl != null && (Immunization_LotNumber.params.ParentCtrl == "Clinical_ImmunizationDetail" || Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection")) {

                            }
                            else {
                                $row.attr("onclick", selectMethod);
                            }
                        }

                    }
                    else {
                        if (typeof Immunization_LotNumber.params.ParentCtrl != typeof undefined && Immunization_LotNumber.params.ParentCtrl != null && (Immunization_LotNumber.params.ParentCtrl == "Clinical_ImmunizationDetail" || Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection")) {

                        }
                        else {
                            $row.attr("onclick", selectMethod);
                        }
                    }
                    var methods = "";
                    var EditParamter = "";
                    EditParamter = item.VaccineLotNoId + ",event,'" + item.Type + "'";
                    if (typeof Immunization_LotNumber.params.ParentCtrl != typeof undefined && Immunization_LotNumber.params.ParentCtrl != null && (Immunization_LotNumber.params.ParentCtrl == "Clinical_ImmunizationDetail" || Immunization_LotNumber.params.ParentCtrl == "Immunization_TherapeuticInjection")) {
                        if (delPermission) {
                            methods += '<a class="btn  btn-xs" href="#" onclick="Immunization_LotNumber.LotNumberDelete( ' + item.VaccineLotNoId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>';
                        }

                        if (editPermission) {
                            methods += '&nbsp;<a class="btn btn-xs" href="#" onclick="Immunization_LotNumber.EditLotNumber(' + EditParamter + ');"  title="Edit Record"><i class="fa fa-edit black"></i></a>';
                        }
                        methods = selectLotNumber + methods;
                    }
                    else {
                        methods = '<a class="btn  btn-xs" href="#" onclick="Immunization_LotNumber.LotNumberDelete( ' + item.VaccineLotNoId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Immunization_LotNumber.EditLotNumber(' + EditParamter + ');"  title="Edit Record"><i class="fa fa-edit black"></i></a>';
                    }




                    /*var ClassForExpiry = "";
                    var ClassForQty = "";
                    if (parseInt(item.QuantityLeft) <= 0 && item.IsExpired.toLowerCase() == "true") {
                        ClassForExpiry = 'Class="red bold"';
                        ClassForQty = 'Class="red bold"';
                    }
                    else if (item.IsExpired.toLowerCase() == "true") {
                        ClassForExpiry = 'Class="red bold"';
                    }
                    else if (parseInt(item.QuantityLeft) <= 0) {
                        ClassForQty = 'Class="red bold"';
                    }
                    $row.append('<td style="display:none;">' + item.VaccineLotNoId + '</td><td>' + methods + '</td><td>' + item.LotNo + '</td><td>' + item.VaccineName + '</td><td>' + item.ManufacturerName + '</td><td ' + ClassForExpiry + '>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td>' + item.Quantity + '</td><td ' + ClassForExpiry + '>' + item.QuantityLeft + '</td><td>' + item.Type + '</td>');*/

                    if (parseInt(item.QuantityLeft) <= 0 || item.IsExpired.toLowerCase() == "true") {
                        $row.append('<td style="display:none;">' + item.VaccineLotNoId + '</td><td>' + methods + '</td><td>' + item.LotNo + '</td><td>' + item.VaccineName.split('|').join('<br>') + '</td><td>' + item.ManufacturerName + '</td><td class="red bold">' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td>' + item.Quantity + '</td><td class="red bold">' + item.QuantityLeft + '</td><td>' + (item.Type == "TherapueticInjection" ? "Therapuetic Injection" : item.Type) + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.VaccineLotNoId + '</td><td>' + methods + '</td><td>' + item.LotNo + '</td><td>' + item.VaccineName.split('|').join('<br>') + '</td><td>' + item.ManufacturerName + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td>' + item.Quantity + '</td><td>' + item.QuantityLeft + '</td><td>' + (item.Type == "TherapueticInjection" ? "Therapuetic Injection" : item.Type) + '</td>');
                    }

                    $('#' + Immunization_LotNumber.params.PanelID + " #pnlLotNumber_Result #dgvLotNumer tbody").last().append($row);
                });
                if ($.fn.dataTable.isDataTable('#' + Immunization_LotNumber.params.PanelID + " #dgvLotNumer") || $('#' + Immunization_LotNumber.params.PanelID + " #dgvLotNumer").parent().parent().hasClass("dataTables_wrapper"))
                    ;
                else
                    $('#' + Immunization_LotNumber.params.PanelID + " #dgvLotNumer").DataTable({ "paging": false, "info": false, bDestroy: true, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            }
            else {

                $('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer').DataTable({
                    "language": {
                        "emptyTable": "No Lot is Found"
                    }, "autoWidth": false, "bLengthChange": false,
                    "paging": false,
                    "info": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }


            if ($.fn.dataTable.isDataTable('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer'))
                ;
            else {
                $('#' + Immunization_LotNumber.params.PanelID + " #pnlLotNumber_Result #dgvLotNumer").DataTable({ "bInfo": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            }
            //$('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div:first').html('<div id="gridLotNumbeControls">' + gridLotNumbeControls + '</div>');

            //if (Immunization_LotNumber.params.PanelID == "pnlClinicalImmunizationDetail #pnlImmunization_LotNumber") {
            //$("#AddLot").remove();
            //}

            //$('#' + Immunization_LotNumber.params.PanelID + ' #gridLotNumbeControls input[type=checkbox]').prop('checked', gridLotNumbeControlsChecked);
            //if ($('#' + Immunization_LotNumber.params.PanelID + '  .ios-switch').length > 0) {
            //    $('#' + Immunization_LotNumber.params.PanelID + '  .ios-switch').remove();
            //}
            //(function ($) {
            //    'use strict';
            //    $(function () {
            //        $('#' + Immunization_LotNumber.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
            //            var $this = $(this);
            //            $this.themePluginIOS7Switch();
            //        });
            //    });
            //}).apply(this, [jQuery]);
            //$('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
            //$('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');


            //if (!$('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-5')) {
            //    $('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-5');
            //}
            //if (!$('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div.dataTables_filter').parent().hasClass('mt-md')) {
            //    $('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div.dataTables_filter').addClass('mt-md');
            //}
            //if (!$('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-9')) {
            //    $('#' + Immunization_LotNumber.params.PanelID + ' #dgvLotNumer_wrapper .datatables-header div:first').addClass('col-sm-6 col-md-7');
            //}
        });
    },
    EditLotNumber: function (VaccineLotNoId, event, Type) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = "Immunization_LotNumber";
        params["VaccineLotNoId"] = VaccineLotNoId;
        params["mode"] = "Edit";
        params["PatientId"] = Immunization_LotNumber.params["patientID"];
        params["FromAdmin"] = Immunization_LotNumber.params["FromAdmin"];
        if (Immunization_LotNumber.params["ParentCtrl"] == "Clinical_ImmunizationDetail") {
            params["VaccineId"] = Immunization_LotNumber.params["VaccineId"];
            params["VaccineText"] = Immunization_LotNumber.params["VaccineText"];
        }
        if (Immunization_LotNumber.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Immunization_LotNumber';
        }

        if (Type == "TherapueticInjection") {
            params["Type"] = 'TherapeuticInjection';
        }
        else {
            params["Type"] = 'vaccine';
        }
        LoadActionPan("Immunization_LotNumberDetail", params);
    },
    LotNumberDelete: function (VaccineLotNoId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        var deletionMsg = "Do you want to delete this record ?";
        AppPrivileges.GetFormPrivileges("Immunization_Lot Management", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = VaccineLotNoId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_LotNumber.DeleteLotNumber(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_LotNumber.LoadLotNumberList();
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
    DeleteLotNumber: function (VaccineLotNoId) {
        var objData = new Object();
        objData["VaccineLotNoId"] = VaccineLotNoId;
        objData["commandType"] = "delete_lotnumber";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },
    SelectLotNumber: function (VaccineLotNoId, Vaccineid, ExpiryDate, RouteId, event, QuantityLeft, Type, IsExpiryed) {

        if (QuantityLeft > 0 && IsExpiryed.toLowerCase() != "true") {
            var strMessage = "";
            if (event != null) {
                event.stopPropagation();
            }
            var selectedValue = VaccineLotNoId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                if (Immunization_LotNumber.params["ParentCtrl"] && Immunization_LotNumber.params["ParentCtrl"] == "Clinical_ImmunizationDetail") {
                    Clinical_ImmunizationDetail.PopulateLotNumber(Vaccineid);
                    Clinical_ImmunizationDetail.PopulateLotNumberFromLotNumberList(Vaccineid, VaccineLotNoId);
                    Clinical_ImmunizationDetail.SetExpiryDateAndRoute(ExpiryDate, RouteId);
                    Immunization_LotNumber.UnLoadTab();
                }
                else if (Immunization_LotNumber.params["ParentCtrl"] && Immunization_LotNumber.params["ParentCtrl"] == "Immunization_TherapeuticInjection") {
                    $.when(Immunization_TherapeuticInjection.PopulateLotNumber(Vaccineid)).then(function () {
                        $.when(Immunization_TherapeuticInjection.PopulateLotNumberFromLotNumberList(Vaccineid, VaccineLotNoId)).then(function () {
                            $.when(Immunization_TherapeuticInjection.SetExpiryDateAndRoute(ExpiryDate, RouteId)).then(function () {
                                Immunization_LotNumber.UnLoadTab();
                            });
                        });
                    });
                }
            }
        }
        else {
            if (QuantityLeft <= 0 && IsExpiryed.toLowerCase() == "true") {
                utility.DisplayMessages("Either Vaccine lot is expired or out-of-stock!", 2);
            }
            else if (QuantityLeft <= 0) {
                utility.DisplayMessages("Lot is out-of-stock!", 2);
            }
            else if (IsExpiryed.toLowerCase() == "true") {
                utility.DisplayMessages("Lot has been expired!", 2);
            }
        }

    },
}