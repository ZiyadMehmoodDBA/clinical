Clinical_CustomForms = {

    params: [],
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',


    Load: function (params) {
        Clinical_CustomForms.params = params;
        if (Clinical_CustomForms.params.PanelID != 'pnlClinicalCustomForms') {
            Clinical_CustomForms.params.PanelID = Clinical_CustomForms.params.PanelID + ' #pnlClinicalCustomForms';
        } else {
            Clinical_CustomForms.params.PanelID = 'pnlClinicalCustomForms';
        }
        selectedEntity = globalAppdata["SeletedEntityId"];
        var self = $('#' + Clinical_CustomForms.params.PanelID);
        self.loadDropDowns(true).done(function () {
            $.when(Clinical_CustomForms.loadEntitySpecialty(selectedEntity)).then(function () {
                Clinical_CustomForms.loadEntityProvider(selectedEntity).done(function () { 
                    Clinical_CustomForms.customFormsSearch();
                });
            });
            Clinical_CustomForms.IntializeMultiSelectDropDown();
        });

        if (Clinical_CustomForms.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.setFavoriteSectionStyle(Clinical_CustomForms.params.PanelID);
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_CustomForms.params.PanelID, 'Miscellaneous', 'CustomForm', 'Clinical_CustomForms.UnLoadTab();', 'frmCustomFormPreview');
            Clinical_CustomForms.searchFavCustomFormList();
            $("#" + Clinical_CustomForms.params.PanelID + " #showAllCFLnk").show();
            $("#" + Clinical_CustomForms.params.PanelID + " #favSectionDiv").parent().css('height', '420px').addClass('borderT');
            $("#" + Clinical_CustomForms.params.PanelID + " #favSectionDiv").show();
        }
        else {
            $("#" + Clinical_CustomForms.params.PanelID + " #showAllCFLnk").hide();            
            $("#" + Clinical_CustomForms.params.PanelID + " #favSectionDiv").remove();
        }
    },



    // Start for custom form preview - ZeeshanAK on 27 September, 2016
    customFormPreview: function (formId, customFormName, event, isAddToNote, fromAdmin) {
        if (event != null) {
            event.stopPropagation();
        }

        var params = [];
        var PanelID = "";
        if (Clinical_CustomForms.params && Clinical_CustomForms.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'clinicalTabProgressNote';
            params["IsFromAdminOrNot"] = "0";
            params["IsAddToNote"] = isAddToNote;

            if (!isAddToNote) {
                params["IsPreview"] = true;
            }
            else {
                params["IsPreview"] = false;
            }
        }
        //else if (Select_CustomForm.params && Select_CustomForm.params.PanelID) {
        //    params["ParentCtrl"] = 'Select_CustomForm';
        //    PanelID = 'pnlPatientCustomForm #pnlSelectCustomForm';
        //    params["IsFromAdminOrNot"] = "0";
        //    params["IsAddToNote"] = isAddToNote;
        //}
        if (fromAdmin) {
            params["ParentCtrl"] = 'Clinical_CustomForms';
            PanelID = 'pnlClinicalCustomForms';
            params["IsFromAdminOrNot"] = "1";
        }

        params["FromAdmin"] = Clinical_CustomForms.params["FromAdmin"];
        params["CustomFormId"] = formId;
        params["CustomFormName"] = customFormName;
        params["PanelID"] = PanelID;       
        
        if (!fromAdmin) {
            Clinical_CustomForms.UnLoadTab();
        }
        setTimeout(function () {
            LoadActionPan("Clinical_CustomFormsPreview", params);
        }, 510);

    },
    initilizeGridster: function (canvasCols) {
        var objDeffered = $.Deferred();
        var canvousCol = canvasCols;
        var wdgWidth = 300;
        var wdgHeight = 80;
        switch (canvousCol) {
            case "1":
                wdgWidth = 900;
            case "2":
                wdgWidth = 600;
            case "3":
                wdgWidth = 300;
                break;
            default:
                break;

        }
        gridster = $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails").gridster({
            widget_base_dimensions: [900, 80],
            widget_margins: [5, 5],
            max_cols: parseInt(canvousCol),
            //  autogenerate_stylesheet_opt: false,
            autogenerate_stylesheet: false,
            serialize_params: function ($w, wgd) {
                return {
                    col: wgd.col,
                    row: wgd.row,
                    size_y: wgd.size_y,
                    size_x: wgd.size_x,
                }
            }
        }).data('gridster');//.disable();
        $("#customFormDetails").gridster().width("100%");
        $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails").gridster().width("100%");
        objDeffered.resolve();
        return objDeffered;
    },
    unLoadPreview: function () {
        var actionPan = $('#pnlCustomFormPreview');
        actionPan.modal('hide');
    },
    // End for custom form form preview - ZeeshanAK on 27 September, 2016

    // Start for custom form active inactive  - ZeeshanAK on 26 September, 2016
    customFormActiveInactive: function (formId, isActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            if (formId == "" || formId == "undefined") {
            }
            else {
                Clinical_CustomForms.customFormActiveInactive_Dbcall(formId, isActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_CustomForms.customFormsSearch();
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
    customFormActiveInactive_Dbcall: function (formId, isActive) {
        var objData = {};
        objData["CustomFormId"] = formId;
        objData["IsActive"] = isActive;

        objData["commandType"] = "UPDATE_CUSTOM_FORM_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    // End for custom form active inactive  - ZeeshanAK on 26 September, 2016


    // Start for custom form edit  - ZeeshanAK on 23 September, 2016
    customFormEdit: function (formId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Template_Review of System", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (formId == "" || formId == "undefined") {
                }
                else {
                    var params = [];
                    params["ParentCtrl"] = "Clinical_CustomForms";
                    params["CustomFormId"] = formId;
                    params["mode"] = "Edit";
                    LoadActionPan('Clinical_CustomFormsDetails', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    // End for custom form edit - ZeeshanAK on 23 September, 2016

    // Start for custom form delete  - ZeeshanAK on 22 September, 2016
    customFormDelete: function (formId, event, NotesId) {
        if (event != null) {
            event.stopPropagation();
        }
        if (NotesId != null && NotesId != "") {
            utility.DisplayMessages('This form is currently associated with Provider Notes and cannot be deleted.', 3);
        } else {
            utility.myConfirm('1', function () {
                if (formId > 0) {
                    Clinical_CustomForms.customFormDelete_DbCall(formId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_CustomForms.customFormsSearch();
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
    customFormDelete_DbCall: function (formId) {
        var objData = {};
        objData["CustomFormId"] = formId;
        objData["commandType"] = "delete_custom_forms";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    // End for custom form delete - ZeeshanAK on 22 September, 2016

    // Start for Grid load and data binding  - ZeeshanAK on 21 September, 2016
    customFormsSearch: function (CustomFormId, PageNo, rpp) {
        if (Clinical_CustomForms.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_CustomForms.params.PanelID + ' #ddlFavoriteListCF').val('-1');
        }
        Clinical_CustomForms.searchCustomForms_DBCall(PageNo, rpp, CustomFormId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_CustomForms.customFromsGridLoad(response);
                var TableControl = Clinical_CustomForms.params.PanelID + " #dgvCustomForms";
                var PagingPanelControlID = Clinical_CustomForms.params.PanelID + " #divCustomFormsPaging";
                var ClassControlName = "Clinical_CustomForms";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.customFormCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_CustomForms.customFormsSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    activeCustomFormSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        Clinical_CustomForms.customFormsSearch();
    },
    searchCustomForms_DBCall: function (PageNumber, RowsPerPage, CustomFormId) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var IsActive = null;
        IsActive = $('#' + Clinical_CustomForms.params.PanelID + ' #pnlCustomForms_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var objData = {};
        objData["FormName"] = $('#' + Clinical_CustomForms.params.PanelID + " #txtCustomFormName").val();
        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = IsActive;
        objData["RowsPerPage"] = RowsPerPage;
        //objData["SpecialtyIds"] = $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialtyCustomForm").parent().find('button').text().trim().indexOf('Select') == 0 || $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialtyCustomForm").parent().find('button').text().trim().indexOf('All') == 0 ? '' : $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialtyCustomForm").val().join();
        //objData["ProviderIds"] = $('#' + Clinical_CustomForms.params.PanelID + " #ddlProviderCustomForm").parent().find('button').text().trim().indexOf('Select') == 0 || $('#' + Clinical_CustomForms.params.PanelID + " #ddlProviderCustomForm").parent().find('button').text().trim().indexOf('All') == 0 ? '' : $('#' + Clinical_CustomForms.params.PanelID + " #ddlProviderCustomForm").val().join();

        //------------------------------------------------------------------------------------------------------------------------------

        var SpecialtyIds = $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialtyIds"] = SpecialtyIds;

        var ProviderIds = $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;

        //-------------------------------------------------------------------------------------------------------------------------------
        if (Clinical_CustomForms.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["IsFromNotes"] = "1";
        }
        else {
            objData["IsFromNotes"] = "0";
        }

        objData["commandType"] = "load_custom_forms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    customFromsGridLoad: function (response) {
        if (Clinical_CustomForms.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_CustomForms.params.PanelID + " #customFormAdd").hide();
            if ($.fn.dataTable.isDataTable('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms")) {
                $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms").dataTable().fnDestroy();
                $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms tbody").find("tr").remove();
            } else {
                //for stop make dublicate Datatables
                $.each($.fn.DataTable.fnTables(), function () {
                    if (this.id == 'dgvCustomForms') {
                        $(this).dataTable().fnClearTable();
                        $(this).dataTable().fnDestroy();
                        $(this).find("tbody tr").remove();
                        $("#" + Clinical_CustomForms.params.PanelID + " #pnlCustomForms_Result #dgvCustomForms tbody").find("tr").remove();
                        $("#" + Clinical_CustomForms.params.PanelID + " #pnlCustomForms_Result #dgvCustomForms").parent().parent().find('div.row').remove();
                    }
                })
            }

            if (response.customFormCount > 0) {
                var listCustomForm_JSON = response.listCustomForm;
                $.each(listCustomForm_JSON, function (i, item) {
                    if (item.ProviderNames != "") {


                        var $row = $('<tr/>');
                        $row.attr("onclick", "utility.SelectGridRow($('#gvROSDataTemplate_row" + item.CustomFormId + "'));");
                        $row.attr("id", "gvROSDataTemplate_row" + item.CustomFormId);
                        $row.attr("CustomFormId", item.CustomFormId);
                        $row.attr("Active", item.IsActive);

                        //  $row.attr("onclick", "Clinical_CustomForms.customFormEdit(" + item.CustomFormId + ", event);");

                        $row.append('<td>' +
                            '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_CustomForms.customFormPreview(' + item.CustomFormId + ',' + "'" + item.FormName + "'" + ', event,true,false);" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;' +
                            '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_CustomForms.customFormPreview(' + item.CustomFormId + ',' + "'" + item.FormName + "'" + ', event,false,false);" title="Preview Record"><i class="fa fa-credit-card blue"></i></a>&nbsp;</td>' +
                            '<td>' + item.FormName + '</td><td>' + item.SpecialtyNames + '</td><td>' + item.ProviderNames + '</td><td>' + item.ModifiedOn.replace(/(.*)\D\d+/, '$1') + ' by ' + item.ModifiedByName + '</td>');
                        $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms tbody").last().append($row);
                    }
                });

                if ($.fn.dataTable.isDataTable('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms"))
                    ;
                else {
                    $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

                }
            }
            else {
                $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms").DataTable({
                    "language": {
                        "emptyTable": "No Custom Form Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }

        } else {
            $("#customFormAdd").show();
            var isGridactive = $('#' + Clinical_CustomForms.params.PanelID + ' #pnlCustomForms_Result #divSwitch #switchActive').attr('isactive');
            if ($.fn.dataTable.isDataTable('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms")) {
                $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms").dataTable().fnDestroy();
                $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms tbody").find("tr").remove();
            }
            else {
                //for stop make dublicate Datatables
                $.each($.fn.DataTable.fnTables(), function () {
                    if (this.id == 'dgvCustomForms') {
                        $(this).dataTable().fnClearTable();
                        $(this).dataTable().fnDestroy();
                        $(this).find("tbody tr").remove();
                        $("#" + Clinical_CustomForms.params.PanelID + " #pnlCustomForms_Result #dgvCustomForms tbody").find("tr").remove();
                        $("#" + Clinical_CustomForms.params.PanelID + " #pnlCustomForms_Result #dgvCustomForms").parent().parent().find('div.row').remove();
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
            if (response.customFormCount > 0) {
                var listCustomForm_JSON = response.listCustomForm;
                $.each(listCustomForm_JSON, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvROSDataTemplate_row" + item.CustomFormId + "'));");
                    $row.attr("id", "gvROSDataTemplate_row" + item.CustomFormId);
                    $row.attr("CustomFormId", item.CustomFormId);
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

                    $row.attr("onclick", "Clinical_CustomForms.customFormEdit(" + item.CustomFormId + ", event);");

                    $row.append('<td>' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_CustomForms.customFormDelete(' + item.CustomFormId + ', event' + ((item.NoteId != 0) ? ',' + item.NoteId : '') + ');"title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_CustomForms.customFormEdit(' + item.CustomFormId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_CustomForms.customFormActiveInactive(' + item.CustomFormId + ', ' + isEventactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;' +
                        '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_CustomForms.customFormPreview(' + item.CustomFormId + ',' + "'" + item.FormName + "'" + ', event,false,true);" title="Preview Record"><i class="fa fa-credit-card blue"></i></a>&nbsp;</td>' +
                        '<td>' + item.FormName + '</td><td>' + item.SpecialtyNames + '</td><td>' + item.ProviderNames + '</td><td>' + item.ModifiedOn.replace(/(.*)\D\d+/, '$1') + ' by ' + item.ModifiedByName + '</td>');
                    $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms tbody").last().append($row);
                });

                if ($.fn.dataTable.isDataTable('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms"))
                    ;
                else {
                    $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

                }
                var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                            '<input id="switchActive" isactive="' + isGridactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_CustomForms.activeCustomFormSearch(this);">' +
                             '</div><span class="pl-xs">Active</span>' +
             '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

                $("#" + Clinical_CustomForms.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            }
            else {
                $('#' + Clinical_CustomForms.params.PanelID + " #dgvCustomForms").DataTable({
                    "language": {
                        "emptyTable": "No Custom Form Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
                var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                          '<input id="switchActive" isactive="' + isGridactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_CustomForms.activeCustomFormSearch(this);">' +
                           '</div><span class="pl-xs">Active</span>' +
           '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

                $("#" + Clinical_CustomForms.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            }

            EMRUtility.SwicthWidgetInializatoin();
        }
    },
    // End for Grid load and data binding - ZeeshanAK on 21 September, 2016


    // Start for Specialty and Provider multiselect cascading dropdowns - ZeeshanAK on 19 September, 2016
    IntializeMultiSelectDropDown: function () {


        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            allSelectedText: 'Selected All',
        });
        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            allSelectedText: 'Selected All',
        });
        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm, #' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm').multiselect('selectAll', false);
        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm, #' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm').multiselect('updateButtonText');
    },
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + Clinical_CustomForms.params.PanelID + ' #divCustomFormsSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_CustomForms.specialityCheckedIds = [];
            Clinical_CustomForms.providerCheckedIds = [];
            Clinical_CustomForms.ProviderIds = '';
            Clinical_CustomForms.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_CustomForms.specialityCheckedIds = Clinical_CustomForms.removeFromArray(Clinical_CustomForms.specialityCheckedIds, spacialityId);
                    Clinical_CustomForms.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_CustomForms.specialityCheckedIds = Clinical_CustomForms.removeFromArray(Clinical_CustomForms.specialityCheckedIds, spacialityId);
                }
            }
            else {
                Clinical_CustomForms.specialityCheckedIds = [];
                $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Clinical_CustomForms.specialityCheckedIds.push(spacialityId);
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
                    $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (Clinical_CustomForms.SpecialtyIds != '') {

                        var Specialties = Clinical_CustomForms.SpecialtyIds.split(",");
                        Clinical_CustomForms.specialityCheckedIds = Specialties;
                        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm').val(Specialties);
                    }
                }

            }).then(function () {
                Clinical_CustomForms.IntializeMultiSelectDropDownSpecialties();
                objDeffered.resolve();
            });
        }
        else {

            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm').multiselect('destroy');
        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_CustomForms.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_CustomForms.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_CustomForms.ProviderIds != '') {
                        var Providers = Clinical_CustomForms.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_CustomForms.providerCheckedIds = Clinical_CustomForms.removeFromArray(Clinical_CustomForms.providerCheckedIds, item);
                                Clinical_CustomForms.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm').val(Clinical_CustomForms.providerCheckedIds);
                    Clinical_CustomForms.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_CustomForms.SpecialtyIds != '') {
                    var spacialties = Clinical_CustomForms.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_CustomForms.specialityCheckedIds = Clinical_CustomForms.removeFromArray(Clinical_CustomForms.specialityCheckedIds, item);
                            Clinical_CustomForms.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_CustomForms.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_CustomForms.params.PanelID + ' #ddlSpecialtyCustomForm').multiselect('select', Clinical_CustomForms.specialityCheckedIds);
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
                var $providerDdl = $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm');
                var $providerHiddenDdl = $('#' + Clinical_CustomForms.params.PanelID + ' #ddlHiddenCustomFormsProvider');

                $providerDdl.empty();
                $providerHiddenDdl.empty();

                //Loop through all providers loaded from the server
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {

                        // User will see these providers in multiSelect dropdownlist
                        $providerDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

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
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (Clinical_CustomForms.ProviderIds != '') {
                    var Providers = Clinical_CustomForms.ProviderIds.split(",");
                    Clinical_CustomForms.providerCheckedIds = Providers;
                    $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect
                $('#' + Clinical_CustomForms.params.PanelID + ' #divCustomFormsSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.
                Clinical_CustomForms.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm').multiselect('destroy');
        $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_CustomForms.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
            },
        });
    },
    setSpacialtiesByselectedProviderIds: function () {

        $.each(Clinical_CustomForms.providerCheckedIds, function (index, item) {

            $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {
                        Clinical_CustomForms.specialityCheckedIds = Clinical_CustomForms.removeFromArray(Clinical_CustomForms.specialityCheckedIds, $(this).attr('refname'));
                        Clinical_CustomForms.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Clinical_CustomForms.params.PanelID + ' #ddlHiddenCustomFormsProvider';

        var providerContext = '#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm';
        $(providerContext).empty();

        if (Clinical_CustomForms.specialityCheckedIds.length > 0) {

            $.each(Clinical_CustomForms.specialityCheckedIds, function (index, specialtyId) {

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
        var providerContext = '#' + Clinical_CustomForms.params.PanelID + ' #divCustomFormsProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            Clinical_CustomForms.providerCheckedIds = [];
            Clinical_CustomForms.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            Clinical_CustomForms.providerCheckedIds = [];
            $('#' + Clinical_CustomForms.params.PanelID + ' #ddlProviderCustomForm option').each(function () {
                var providerValue = $(this).val();
                Clinical_CustomForms.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Clinical_CustomForms.providerCheckedIds = Clinical_CustomForms.removeFromArray(Clinical_CustomForms.providerCheckedIds, providerValue);
                Clinical_CustomForms.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Clinical_CustomForms.providerCheckedIds = Clinical_CustomForms.removeFromArray(Clinical_CustomForms.providerCheckedIds, $(option).val());
            }

        }
    },
    // End for Specialty and Provider multiselect cascading dropdowns - ZeeshanAK on 19 September, 2016


    AddCustomForm: function () {
        var params = [];
        params["ParentCtrl"] = "Clinical_CustomForms";
        params["mode"] = "Add";
        params["FromAdmin"] = Clinical_CustomForms.params["FromAdmin"];
        if (Clinical_CustomForms.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_CustomForms';
        }
        LoadActionPan("Clinical_CustomFormsDetails", params);
    },

    initilizeChildGridster: function (canvasCols) {
        var canvousCol = canvasCols;
        var wdgWidth = 300;
        var wdgHeight = 80;
        switch (canvousCol) {
            case "1":
                wdgWidth = 900;
            case "2":
                wdgWidth = 600;
            case "3":
                wdgWidth = 300;
                break;
            default:
                break;

        }
        var gridster = $("#toolQuestionGroupHTML").data('gridster');
        if (gridster) {
            gridster.remove_all_widgets();
            gridster.destroy();
            $("#toolQuestionGroupHTML").empty();
        }
        gridster = $("#toolQuestionGroupHTML").gridster({
            widget_base_dimensions: [900, 80],
            widget_margins: [5, 5],
            max_cols: parseInt(canvousCol),
            //autogenerate_stylesheet_opt: false,
            autogenerate_stylesheet: true,
            serialize_params: function ($w, wgd) {
                return {
                    col: wgd.col,
                    row: wgd.row,
                    size_y: wgd.size_y,
                    size_x: wgd.size_x,
                }
            }
        }).data('gridster');//.disable();
        $("#toolQuestionGroupHTML").gridster().width("100%");

    },
    rebuildParentGridster: function () {
        var gridster = $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails").data('gridster');
        var arr_widget = [];
        $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails li:not(.groupli)").each(function () {
            //if($(this).hasClass())
            var widget_object = {
                col: parseInt($(this).attr("data-col")),
                row: parseInt($(this).attr("data-row")),
                size_x: parseInt($(this).attr("data-sizex")),
                size_y: parseInt($(this).attr("data-sizey")),
                html: $(this)[0].outerHTML,
            }
            arr_widget.push(widget_object);
        });
        gridster.remove_all_widgets();
        $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails").empty();
        $.each(arr_widget, function () {
            gridster.add_widget(this.html, this.size_x, this.size_y, this.col, this.row);
        });
        var childGridsterItems = $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails li.groupli")
        if (childGridsterItems && childGridsterItems.length > 0) {
            Clinical_CustomForms.rebuildChildGridster();
        }
        setTimeout(function () {
            var canvasCols = $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails").attr('canvasCol');
            var widthpercent = 100;
            switch (canvasCols) {
                case "1":
                    widthpercent = 100;
                    break;
                case "2":
                    widthpercent = 50;
                    break;
                case "3":
                    widthpercent = 33;
                    break;
                default:
                    widthpercent = 100;
                    break;

            }
            $("#" + Clinical_CustomForms.params.PanelID + " #customFormDetails li:not(.groupli)").each(function () {
                var data_sizex = parseInt($(this).attr("data-sizex"));
                if (data_sizex)
                    $(this).css('width', (widthpercent * data_sizex) + '% !important');
            });
        }, 300);
    },
    rebuildChildGridster: function () {
        var gridster = $("#" + Clinical_CustomForms.params.PanelID + " #toolQuestionGroupHTML").data('gridster');
        var arr_widget = [];
        $("#" + Clinical_CustomForms.params.PanelID + " #toolQuestionGroupHTML li:not(.groupli)").each(function () {
            //if($(this).hasClass())
            var widget_object = {
                col: parseInt($(this).attr("data-col")),
                row: parseInt($(this).attr("data-row")),
                size_x: parseInt($(this).attr("data-sizex")),
                size_y: parseInt($(this).attr("data-sizey")),
                html: $(this)[0].outerHTML,
            }
            arr_widget.push(widget_object);
        });
        gridster.remove_all_widgets();
        $("#" + Clinical_CustomForms.params.PanelID + " #toolQuestionGroupHTML").empty();
        $.each(arr_widget, function () {
            gridster.add_widget(this.html, this.size_x, this.size_y, this.col, this.row);
        });
    },

    getCustomFormForNotes: function (customFormName, customFormId, CustomFormUniqueId, customFormNameForDoc, bSaveComponent) {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_customform[uniqueid=' + CustomFormUniqueId + ']').length == 0) {
            var uniqueId = "" + customFormId + '_' + utility.makeRendomKey() + "";

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #MiscellaneousNoteComponentList';


            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            if ($(CompnentSelector).find("clinical_customforms").length > 0) {
                CompnentSelector = $(CompnentSelector).find("clinical_customforms").closest('.initialVisitBody');

                CompnentSelector.after(' <li class="CustomFormsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_customform title="' + customFormName.replace('{{', '').replace('}}', '').replace('Custom Form', '').trim() + '"  id="' + customFormId + '" uniqueId ="' + uniqueId + '" docid ="' + customFormNameForDoc + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'CustomFormPreview\',' + customFormId + ',' + Clinical_ProgressNote.params.NotesId + ',' + "'" + customFormName + "'" + ',' + "'" + uniqueId + "'" + ',' + "'" + customFormNameForDoc + "'" + ');" title="CustomForms">' + customFormName.replace('{{', '').replace('}}', '').replace('Custom Form', '').trim() + '</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'CustomForm\',' + customFormId + ',' + Clinical_ProgressNote.params.NotesId + ',' + "'" + uniqueId + "'" + ',' + "'" + customFormNameForDoc + "'" + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_customform> </header></li>');
            }

            else {
                $(CompnentSelector).append(' <li class="CustomFormsComponent" NoteComponentId="NCDummyId"> <header>' +
            '<clinical_customform title="' + customFormName.replace('{{', '').replace('}}', '').replace('Custom Form', '').trim() + '"  id="' + customFormId + '" uniqueId ="' + uniqueId + '" docid ="' + customFormNameForDoc + '" class="NotesComponent">' +
            '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'CustomFormPreview\',' + customFormId + ',' + Clinical_ProgressNote.params.NotesId + ',' + "'" + customFormName + "'" + ',' + "'" + uniqueId + "'" + ',' + "'" + customFormNameForDoc + "'" + ');" title="CustomForms">' + customFormName.replace('{{', '').replace('}}', '').replace('Custom Form', '').trim() + '</a> ' +
                            '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'CustomForm\',' + customFormId + ',' + Clinical_ProgressNote.params.NotesId + ',' + "'" + uniqueId + "'" + ',' + "'" + customFormNameForDoc + "'" + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                       '</clinical_customform> </header></li>');
            }

            Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
            return uniqueId;
        }

    },

    UnLoadTab: function () {
        // if from other flow.
        if (Clinical_CustomForms.params["ParentCtrl"]) {
            UnloadActionPan(Clinical_CustomForms.params.ParentCtrl, 'Clinical_CustomForms');
        }
            //AST - 429  if from Admin fow. 
        else {
            RemoveAdminTab();
        }
       
        Clinical_CustomForms.params = [];

    },
    showAllCustomForms: function () {
        $('#' + Clinical_CustomForms.params.PanelID + " #ddlFavoriteListCF").val('-1');
        Clinical_CustomForms.resetSearchFilter();
        Clinical_CustomForms.customFormsSearch();
    },

    resetSearchFilter: function(){
      
        $('#' + Clinical_CustomForms.params.PanelID + " #txtCustomFormName").val('');
        $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialtyCustomForm").removeAttr("selected");
        $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialtyCustomForm").multiselect("clearSelection");
        $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialtyCustomForm").multiselect("refresh");

        $('#' + Clinical_CustomForms.params.PanelID + " #ddlProviderCustomForm").removeAttr("selected");
        $('#' + Clinical_CustomForms.params.PanelID + " #ddlProviderCustomForm").multiselect("clearSelection");
        $('#' + Clinical_CustomForms.params.PanelID + " #ddlProviderCustomForm").multiselect("refresh");

        Clinical_CustomForms.specialityCheckedIds = [];
        Clinical_CustomForms.providerCheckedIds = [];
        Clinical_CustomForms.ProviderIds = '';
        Clinical_CustomForms.SpecialtyIds = '';
    },
    loadfavoriteListCF: function (id, PageNo, rpp) {

        Clinical_CustomForms.resetSearchFilter();

        var FavoriteListId = $('#' + Clinical_CustomForms.params.PanelID + ' #ddlFavoriteListCF').val();
        if (FavoriteListId != "-1") {
            Clinical_CustomForms.searchFavoriteList_CF_DBCall(FavoriteListId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_CustomForms.customFromsGridLoad(response);
                    var TableControl = Clinical_CustomForms.params.PanelID + " #dgvCustomForms";
                    var PagingPanelControlID = Clinical_CustomForms.params.PanelID + " #divCustomFormsPaging";
                    var ClassControlName = "Clinical_CustomForms";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = $('#' + Clinical_CustomForms.params.PanelID + ' #dgvCustomForms> tbody > tr').length;
                    setTimeout(CreatePagination(response.customFormCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_CustomForms.loadfavoriteListCF(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                    //if (response.customFormCount > 0) {
                    //    Clinical_CustomForms.customFromsGridLoad(response);
                    //}
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    searchFavoriteList_CF_DBCall: function (FavoriteListId, PageNumber, PageNumber) {
    
        var objData = {};
        
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["IsActive"] = "1";
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;

        objData["commandType"] = "load_favoritelist_customforms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
   
    searchFavCustomFormList: function () {

        Clinical_CustomForms.searchFavoriteList_DBCall("CustomForms", null, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + Clinical_CustomForms.params.PanelID + ' #ddlFavoriteListCF');
                var favouriteLists = JSON.parse(response.FavoriteListJSON)
                $ddl.empty();
                $ddl.append($('<option/>', {
                    id: -1,
                    value: -1,
                    html: "- Select -"
                }));
                $.each(favouriteLists, function (i, item) {
                    if (item.Name != "") {
                        $ddl.append(
                          $('<option/>', {
                              id: item.FavoriteListId,
                              value: item.FavoriteListId,
                              html: item.Name,
                          })
                        );
                    }
                });
            }         
        });

    },

    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["IsActive"] = true;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["IsSelectForLookUp"] = Clinical_CustomForms.params.IsSelectForLookUp;
        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
}