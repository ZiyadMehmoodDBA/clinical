Clinical_OrderSets = {
    params: [],
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    Load: function (params) {
        Clinical_OrderSets.params = params;
        if (Clinical_OrderSets.params.PanelID != 'pnlClinicalOrderSets') {
            Clinical_OrderSets.params.PanelID = Clinical_OrderSets.params.PanelID + ' #pnlClinicalOrderSets';
        } else {
            Clinical_OrderSets.params.PanelID = 'pnlClinicalOrderSets';
        }
        var selectedEntity = globalAppdata["SeletedEntityId"];
        var self = $('#' + Clinical_OrderSets.params.PanelID);
        self.loadDropDowns(true).done(function () {
            if (Clinical_OrderSets.params.ParentCtrl == "clinicalTabProgressNote") {
                 $.when(Clinical_OrderSets.loadEntityProvider(selectedEntity)).then(function () {
                
                Clinical_OrderSets.OrderSetsSearch();
                   });
            } else {
                $.when(Clinical_OrderSets.loadEntitySpecialty(selectedEntity)).then(function () {
                    Clinical_OrderSets.loadEntityProvider(selectedEntity);
                });
                Clinical_OrderSets.OrderSetsSearch();
            }
            
        });

        if (Clinical_OrderSets.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_OrderSets.params.PanelID, 'Miscellaneous', 'OrderSet', 'Clinical_OrderSets.UnLoadTab();', 'frmOrderSetPreview');
        }
    },
    OrderSetActiveInactive: function (orderSetId, isActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            if (orderSetId == "" || orderSetId == "undefined") {
            }
            else {
                Clinical_OrderSets.OrderSetActiveInactive_Dbcall(orderSetId, isActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_OrderSets.OrderSetsSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
                      '3', null, null, null, isActive
                    );
    },
    OrderSetActiveInactive_Dbcall: function (orderSetId, isActive) {
        var objData = {};
        objData["OrderSetId"] = orderSetId;
        objData["IsActive"] = isActive;

        objData["commandType"] = "UPDATE_ORDER_SET_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    OrderSetEdit: function (orderSetId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Template_Review of System", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (orderSetId == "" || orderSetId == "undefined") {
                }
                else {
                    var params = [];
                    params["ParentCtrl"] = "Clinical_OrderSets";
                    params["OrderSetId"] = orderSetId;
                    params["mode"] = "Edit";
                    LoadActionPan('Clinical_OrderSetDetails', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OrderSetDelete: function (orderSetId, event, NotesId) {
        if (event != null) {
            event.stopPropagation();
        }
        if (NotesId != null && NotesId != "") {
            utility.DisplayMessages('This form is currently associated with Provider Notes and cannot be deleted.', 3);
        } else {
            utility.myConfirm('1', function () {
                if (orderSetId > 0) {
                    Clinical_OrderSets.OrderSetDelete_DbCall(orderSetId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_OrderSets.OrderSetsSearch();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            });
        }
    },
    OrderSetDelete_DbCall: function (orderSetId) {
        var objData = {};
        objData["OrderSetId"] = orderSetId;
        objData["commandType"] = "DELETE_ORDER_SET";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    OrderSetsSearch: function (OrderSetId, PageNo, rpp) {
        Clinical_OrderSets.searchOrderSets_DBCall(PageNo, rpp, OrderSetId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_OrderSets.OrderSetGridLoad(response);
                var TableControl = Clinical_OrderSets.params.PanelID + " #dgvOrderSets";
                var PagingPanelControlID = Clinical_OrderSets.params.PanelID + " #divOrderSetsPaging";
                var ClassControlName = "Clinical_OrderSets";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.OrderSetCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_OrderSets.OrderSetsSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    activeOrderSetsearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        Clinical_OrderSets.OrderSetsSearch();
    },
    searchOrderSets_DBCall: function (PageNumber, RowsPerPage, OrderSetId) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var IsActive = null;
        IsActive = $('#' + Clinical_OrderSets.params.PanelID + ' #pnlOrderSets_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var objData = {};
        objData["OrderSetName"] = $('#' + Clinical_OrderSets.params.PanelID + " #txtName").val();
        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = IsActive;
        objData["RowsPerPage"] = RowsPerPage;
        if (Clinical_OrderSets.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["SpecialtyIds"] = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfSpecialtyId').val()
            objData["ProviderIds"] = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfProviderId').val();
        } else {
            objData["SpecialtyIds"] = $('#' + Clinical_OrderSets.params.PanelID + " #ddlSpecialty").val() ? $('#' + Clinical_OrderSets.params.PanelID + " #ddlSpecialty").val().join() : '';
            objData["ProviderIds"] = $('#' + Clinical_OrderSets.params.PanelID + " #ddlProvider").val() ? $('#' + Clinical_OrderSets.params.PanelID + " #ddlProvider").val().join() : '';
        }
        objData["commandType"] = "LOAD_ORDER_SET";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    OrderSetGridLoad: function (response) {
        if (Clinical_OrderSets.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_OrderSets.params.PanelID + " #OrderSetAdd").hide();
            if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets")) {
                $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets").dataTable().fnDestroy();
                $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets tbody").find("tr").remove();
            } else {
                $.each($.fn.DataTable.fnTables(), function () {
                    if (this.id == 'dgvOrderSets') {
                        $(this).dataTable().fnClearTable();
                        $(this).dataTable().fnDestroy();
                        $(this).find("tbody tr").remove();
                        $("#" + Clinical_OrderSets.params.PanelID + " #pnlOrderSets_Result #dgvOrderSets tbody").find("tr").remove();
                        $("#" + Clinical_OrderSets.params.PanelID + " #pnlOrderSets_Result #dgvOrderSets").parent().parent().find('div.row').remove();
                    }
                })
            }

            if (response.OrderSetCount > 0) {
                var listOrderSet_JSON = response.listOrderSet;
                $.each(listOrderSet_JSON, function (i, item) {
                    var Parameter = "null," + item.OrderSetId + ", event,'" + item.OrderSetName.trim() + "'";
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvROSDataTemplate_row" + item.OrderSetId + "'));");
                    $row.attr("id", "gvROSDataTemplate_row" + item.OrderSetId);
                    $row.attr("OrderSetId", item.OrderSetId);
                    $row.attr("Active", item.IsActive);
                    $row.append('<td>' +
                        //'<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSets.OrderSetSelect(' + item.OrderSetId + ',' + "'" + item.OrderSetName + "'" + ', event,true);" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_ProgressNote.OrderSetDetail(' + Parameter + ');" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSets.OrderSetPreview(' + item.OrderSetId + ', event,false);" title="Preview Record"><i class="fa fa-credit-card blue"></i></a>&nbsp;</td>' +
                        '<td>' + item.OrderSetName + '</td><td>' + item.SpecialtyNames + '</td><td>' + item.ProviderNames + '</td><td>' + item.ModifiedOn.replace(/(.*)\D\d+/, '$1') + ' by ' + item.ModifiedByName + '</td>');
                    $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets tbody").last().append($row);
                });

                if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets"))
                    ;
                else {
                    $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

                }
            }
            else {
                $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets").DataTable({
                    "language": {
                        "emptyTable": "No Order Set Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }

        } else {
            $('#' + Clinical_OrderSets.params.PanelID + " #OrderSetAdd").show();
            var isGridactive = $('#' + Clinical_OrderSets.params.PanelID + ' #pnlOrderSets_Result #divSwitch #switchActive').attr('isactive');
            if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets")) {
                $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets").dataTable().fnDestroy();
                $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets tbody").find("tr").remove();
            }
            else {
                $.each($.fn.DataTable.fnTables(), function () {
                    if (this.id == 'dgvOrderSets') {
                        $(this).dataTable().fnClearTable();
                        $(this).dataTable().fnDestroy();
                        $(this).find("tbody tr").remove();
                        $("#" + Clinical_OrderSets.params.PanelID + " #pnlOrderSets_Result #dgvOrderSets tbody").find("tr").remove();
                        $("#" + Clinical_OrderSets.params.PanelID + " #pnlOrderSets_Result #dgvOrderSets").parent().parent().find('div.row').remove();
                    }
                })
            }
            var checked = '';
            if (isGridactive == "0" || isGridactive == 0) {
            } else if (isGridactive == null) {
                isGridactive = "1";
                checked = 'checked="checked"';
            } else {
                isGridactive = "1";
                checked = 'checked="checked"';
            }
            if (response.OrderSetCount > 0) {
                var listOrderSet_JSON = response.listOrderSet;
                $.each(listOrderSet_JSON, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvROSDataTemplate_row" + item.OrderSetId + "'));");
                    $row.attr("id", "gvROSDataTemplate_row" + item.OrderSetId);
                    $row.attr("OrderSetId", item.OrderSetId);
                    $row.attr("Active", item.IsActive);

                    if (item.IsActive == "True") {
                        isactive = 1;
                        isEventactive = 0;
                        activeTitle = "Active Record";
                        tglclass = "fa fa-toggle-on green";
                    }
                    else {
                        isactive = 0;
                        isEventactive = 1;
                        activeTitle = "Inactive Record";
                        tglclass = "fa fa-toggle-on red";
                    }
                    $row.attr("onclick", "Clinical_OrderSets.OrderSetEdit(" + item.OrderSetId + ", event);");
                    $row.append('<td>' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSets.OrderSetDelete(' + item.OrderSetId + ', event' + ((item.NoteId != 0) ? ',' + item.NoteId : '') + ');"title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSets.OrderSetEdit(' + item.OrderSetId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSets.OrderSetActiveInactive(' + item.OrderSetId + ', ' + isEventactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSets.OrderSetPreview(' + item.OrderSetId + ',event);" title="Preview Record"><i class="fa fa-credit-card blue"></i></a>&nbsp;</td>' +
                        '<td>' + item.OrderSetName + '</td><td>' + item.SpecialtyNames + '</td><td>' + item.ProviderNames + '</td><td>' + item.ModifiedOn.replace(/(.*)\D\d+/, '$1') + ' by ' + item.ModifiedByName + '</td>');
                    $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets tbody").last().append($row);
                });

                if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets"))
                    ;
                else {
                    $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

                }
                var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                            '<input id="switchActive" isactive="' + isGridactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_OrderSets.activeOrderSetsearch(this);">' +
                             '</div><span class="pl-xs">Active</span>' +
             '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

                $("#" + Clinical_OrderSets.params.PanelID + " #dgvOrderSets_wrapper").find('.datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
                EMRUtility.SwicthWidgetInializatoin();
            }
            else {
                $('#' + Clinical_OrderSets.params.PanelID + " #dgvOrderSets").DataTable({
                    "language": {
                        "emptyTable": "No Order Set Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
                var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                          '<input id="switchActive" isactive="' + isGridactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_OrderSets.activeOrderSetsearch(this);">' +
                           '</div><span class="pl-xs">Active</span>' +
           '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

                $("#" + Clinical_OrderSets.params.PanelID + " #dgvOrderSets_wrapper").find('.datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
                EMRUtility.SwicthWidgetInializatoin();
            }
        }
    },
    IntializeMultiSelectDropDown: function () {
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            allSelectedText: 'Selected All',
        });
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            allSelectedText: 'Selected All',
        });
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty, #' + Clinical_OrderSets.params.PanelID + ' #ddlProvider').multiselect('selectAll', false);
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty, #' + Clinical_OrderSets.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
    },
    checkProvidersBySpecialityIds: function (option, checked, select) {
        var specialtyContext = '#' + Clinical_OrderSets.params.PanelID + ' #divSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_OrderSets.specialityCheckedIds = [];
            Clinical_OrderSets.providerCheckedIds = [];
            Clinical_OrderSets.ProviderIds = '';
            Clinical_OrderSets.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_OrderSets.specialityCheckedIds = Clinical_OrderSets.removeFromArray(Clinical_OrderSets.specialityCheckedIds, spacialityId);
                    Clinical_OrderSets.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_OrderSets.specialityCheckedIds = Clinical_OrderSets.removeFromArray(Clinical_OrderSets.specialityCheckedIds, spacialityId);
                }
            }
            else {
                Clinical_OrderSets.specialityCheckedIds = [];
                $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Clinical_OrderSets.specialityCheckedIds.push(spacialityId);
                });
            }
        }
    },
    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },
    loadEntitySpecialty: function (entityID) {
        var objDeffered = $.Deferred();
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    if (Clinical_OrderSets.SpecialtyIds != '') {

                        var Specialties = Clinical_OrderSets.SpecialtyIds.split(",");
                        Clinical_OrderSets.specialityCheckedIds = Specialties;
                        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                Clinical_OrderSets.IntializeMultiSelectDropDownSpecialties();
                objDeffered.resolve();
            });
        }
        else {

            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').multiselect('destroy');
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_OrderSets.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_OrderSets.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_OrderSets.ProviderIds != '') {
                        var Providers = Clinical_OrderSets.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_OrderSets.providerCheckedIds = Clinical_OrderSets.removeFromArray(Clinical_OrderSets.providerCheckedIds, item);
                                Clinical_OrderSets.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider').val(Clinical_OrderSets.providerCheckedIds);
                    Clinical_OrderSets.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_OrderSets.SpecialtyIds != '') {
                    var spacialties = Clinical_OrderSets.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_OrderSets.specialityCheckedIds = Clinical_OrderSets.removeFromArray(Clinical_OrderSets.specialityCheckedIds, item);
                            Clinical_OrderSets.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_OrderSets.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').multiselect('select', Clinical_OrderSets.specialityCheckedIds);
            },
        });
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Clinical_OrderSets.params.PanelID + ' #ddlHiddenProvider');

                $providerDdl.empty();
                $providerHiddenDdl.empty();

                //Loop through all providers loaded from the server
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {
                        if (Clinical_OrderSets.params.ParentCtrl == "clinicalTabProgressNote") {


                            // User will see these providers in multiSelect dropdownlist
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfProviderId').val() == item.Value) {

                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfSpecialtyId').val(item.RefName)
                                $providerDdl.append(
                                    $('<option/>', {
                                        value: item.Value,
                                        html: item.Name,
                                        refname: item.RefName,
                                        refvalue: item.RefValue
                                    })
                                );
                                $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').append(
                               $('<option/>', {
                                   value: item.RefName,
                                   html: item.ExValue
                               })
                           );
                            }
                            // Populate hidden ddl provider
                            //A Hack to load all the providers in hidden dropdownlist
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfProviderId').val() == item.Value) {
                                $providerHiddenDdl.append(
                                     $('<option/>', {
                                         value: item.Value,
                                         html: item.Name,
                                         refname: item.RefName,
                                         refvalue: item.RefValue
                                     })
                                );
                            }
                        } else {
                            // User will see these providers in multiSelect dropdownlist

                            $providerDdl.append(
                                $('<option/>', {
                                    value: item.Value,
                                    html: item.Name,
                                    refname: item.RefName,
                                    refvalue: item.RefValue
                                })
                            );
                            $('#' + Clinical_OrderSets.params.PanelID + ' #ddlSpecialty').append(
                           $('<option/>', {
                               value: item.RefName,
                               html: item.ExValue
                           })
                       );

                            // Populate hidden ddl provider
                            //A Hack to load all the providers in hidden dropdownlist

                            $providerHiddenDdl.append(
                                 $('<option/>', {
                                     value: item.Value,
                                     html: item.Name,
                                     refname: item.RefName,
                                     refvalue: item.RefValue
                                 })
                            );

                        }
                    }
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (Clinical_OrderSets.ProviderIds != '') {
                    var Providers = Clinical_OrderSets.ProviderIds.split(",");
                    Clinical_OrderSets.providerCheckedIds = Providers;
                    $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect
                $('#' + Clinical_OrderSets.params.PanelID + ' #divSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.
                Clinical_OrderSets.IntializeMultiSelectDropDownProviders();
                Clinical_OrderSets.IntializeMultiSelectDropDownSpecialties();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_OrderSets.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
            },
        });
    },
    setSpacialtiesByselectedProviderIds: function () {

        $.each(Clinical_OrderSets.providerCheckedIds, function (index, item) {

            $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {
                        Clinical_OrderSets.specialityCheckedIds = Clinical_OrderSets.removeFromArray(Clinical_OrderSets.specialityCheckedIds, $(this).attr('refname'));
                        Clinical_OrderSets.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Clinical_OrderSets.params.PanelID + ' #ddlHiddenProvider';

        var providerContext = '#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider';
        $(providerContext).empty();

        if (Clinical_OrderSets.specialityCheckedIds.length > 0) {

            $.each(Clinical_OrderSets.specialityCheckedIds, function (index, specialtyId) {

                $(providerHiddenContext).find('option').each(function (index, option) {
                    if ($(option).attr('refname') == specialtyId) {
                        $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
                    }
                });
            });
        }
        else {
            $(providerHiddenContext).find('option').each(function (index, option) {
                $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
            });
        }
    },
    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + Clinical_OrderSets.params.PanelID + ' #divProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            Clinical_OrderSets.providerCheckedIds = [];
            Clinical_OrderSets.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            Clinical_OrderSets.providerCheckedIds = [];
            $('#' + Clinical_OrderSets.params.PanelID + ' #ddlProvider option').each(function () {
                var providerValue = $(this).val();
                Clinical_OrderSets.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Clinical_OrderSets.providerCheckedIds = Clinical_OrderSets.removeFromArray(Clinical_OrderSets.providerCheckedIds, providerValue);
                Clinical_OrderSets.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Clinical_OrderSets.providerCheckedIds = Clinical_OrderSets.removeFromArray(Clinical_OrderSets.providerCheckedIds, $(option).val());
            }

        }
    },
    AddOrderSet: function () {
        var params = [];
        params["ParentCtrl"] = "Clinical_OrderSets";
        params["mode"] = "Add";
        params["FromAdmin"] = Clinical_OrderSets.params["FromAdmin"];
        if (Clinical_OrderSets.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_OrderSets';
        }
        LoadActionPan("Clinical_OrderSetDetails", params);
    },
    UnLoadTab: function () {
        if (Clinical_OrderSets.params["FromAdmin"] == "0") {
            if (Clinical_OrderSets.params != null && Clinical_OrderSets.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_OrderSets.params.ParentCtrl, 'Clinical_OrderSets');
            }
            else
                UnloadActionPan(null, 'Clinical_OrderSets');
        }
        else {
            RemoveAdminTab();
        }
    },

    //-----------------Components------------//
    addComponent: function (type, event) {
        if (event != null) {
            if ($(event.target).hasClass('toggleEditableHeader')) {
                return;
            }
            else {
                event.stopPropagation();
            }
        }
        var params = [];
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        params["mode"] = "Add";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        params["FromAdmin"] = "0";
        LoadActionPan("OrderSet_" + type, params, Clinical_OrderSetDetails.params.PanelID);

    },

    // Progress Note work
    OrderSetSelect: function (OrderSetId, OrderSetName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        Clinical_OrderSets.OrderSetSelect_DbCall(OrderSetId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ProgressNote.LoadOrderSetToNote(response);
                Clinical_OrderSets.UnLoadTab();
            }
            else
                utility.DisplayMessages(response.Message, 2);
        });
    },
    OrderSetSelect_DbCall: function (orderSetId) {
        var objData = {};
        objData["OrderSetId"] = orderSetId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["commandType"] = "ATTACH_ORDER_SET_TO_NOTE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    OrderSetPreview: function (orderSetId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Template_Review of System", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (orderSetId == "" || orderSetId == "undefined") {
                }
                else {
                    var params = [];
                    var PanelID = "";
                    if (Clinical_OrderSets.params && Clinical_OrderSets.params.ParentCtrl == "clinicalTabProgressNote") {
                        PanelID = 'pnlClinicalProgressNote #pnlClinicalOrderSets';
                        params["IsFormNotes"] = "1";
                    }
                    else
                        PanelID = 'pnlClinicalOrderSets';
                    params["ParentCtrl"] = "Clinical_OrderSets";
                    params["OrderSetId"] = orderSetId;
                    params["mode"] = "View";
                    params["CustomMode"] = "View";
                    LoadActionPan('Clinical_OrderSetDetails', params, PanelID);

                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
}