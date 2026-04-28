Clinical_ReportHeader = {
    bIsFirstLoad: true,
    params: [],
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    //Start || 24 August, 2016 || Talha Tanweer ||
    Load: function (params) {
        Clinical_ReportHeader.params = params;


        if (Clinical_ReportHeader.params.PanelID != 'pnlClinicalReportHeader') {
            Clinical_ReportHeader.params.PanelID = Clinical_ReportHeader.params.PanelID + ' #pnlClinicalReportHeader';
        } else {
            Clinical_ReportHeader.params.PanelID = 'pnlClinicalReportHeader';
        }
        if (Clinical_ReportHeader.bIsFirstLoad) {
            $('#' + Clinical_ReportHeader.params["PanelID"] + ' select:visible').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            })
            Clinical_ReportHeader.bIsFirstLoad = false;
            var self = $('#' + Clinical_ReportHeader.params.PanelID);
            var data = "EntityId=";
            self.loadDropDowns(true, data).done(function () {
                $('#' + Clinical_ReportHeader.params["PanelID"] + ' #ddlFacility').multiselect('destroy');
                $('#' + Clinical_ReportHeader.params["PanelID"] + ' #ddlFacility').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true
                });
                Clinical_ReportHeader.isEntitySelected();

            });
            Clinical_ReportHeader.ReportHeaderSearch();
        } else {
            Clinical_ReportHeader.ReportHeaderSearch();
        }


    },


    //*******************************************
    ReportHeaderSearch: function (ReportHeaderId, PageNo, rpp) {

        Clinical_ReportHeader.searchReportHeader_DBCall(ReportHeaderId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ReportHeader.ReportHeaderGridLoad(response);
                //Adding Pagination on 04 Dec 2015 by Azhar
                var TableControl = Clinical_ReportHeader.params.PanelID + " #dgvReportHeader";
                var PagingPanelControlID = Clinical_ReportHeader.params.PanelID + " #dgvReportHeader_Paging";
                var ClassControlName = "Clinical_ReportHeader";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.reportHeaderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_ReportHeader.ReportHeaderSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },


    /*
  Author: Muhammad Azhar Shahzad
  Purpose: for Grid Load of Notes template rows html
  Creation Date: March 02,2016 */
    ReportHeaderGridLoad: function (response) {
        var isactive = $('#' + Clinical_ReportHeader.params.PanelID + ' #pnlReportHeader_Result #divSwitch #switchActive').attr('isactive');
        if ($.fn.dataTable.isDataTable('#' + Clinical_ReportHeader.params.PanelID + " #dgvReportHeader")) {
            $('#' + Clinical_ReportHeader.params.PanelID + " #dgvReportHeader").dataTable().fnDestroy();
            $('#' + Clinical_ReportHeader.params.PanelID + " #dgvReportHeader tbody").find("tr").remove();
        }

        if (response.reportHeaderCount > 0) {
            var ReportHeaderLoadJSONData = JSON.parse(response.reportHeaderList_JSON);
            $.each(ReportHeaderLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvReportHeader_row" + item.ReportHeaderId + "'))");
                $row.attr("id", "gvReportHeader_row" + item.ReportHeaderId);
                $row.attr("ReportHeaderId", item.ReportHeaderId);
                $row.attr("Active", item.IsActive);

                if (item.IsActive) {
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
                $row.append('<td>' +
                    '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_ReportHeader.ReportHeaderDelete(' + item.ReportHeaderId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                    '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_ReportHeader.ReportHeaderEdit(' + item.ReportHeaderId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                    '<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_ReportHeader.ReportHeaderActiveInactive(' + item.ReportHeaderId + ', ' + isEventactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass +
                    '"></i></a>&nbsp;</td>' +
                    '<td onclick="Clinical_ReportHeader.ReportHeaderEdit(' + item.ReportHeaderId + ', event);">' + item.ReportHeaderName + '</td><td onclick="Clinical_ReportHeader.ReportHeaderEdit(' + item.ReportHeaderId + ', event);" class="ellip200"  data-placement="left" title="' + item.SpecialtyNames + '">' + item.SpecialtyNames + '</td><td  onclick="Clinical_ReportHeader.ReportHeaderEdit(' + item.ReportHeaderId + ', event);" class="ellip200"  data-placement="left" title="' + item.ProviderNames + '">' + item.ProviderNames + '</td><td  onclick="Clinical_ReportHeader.ReportHeaderEdit(' + item.ReportHeaderId + ', event);" class="ellip200"  data-placement="left" title="' + item.FacilityNames + '">' + item.FacilityNames + '</td><td onclick="Clinical_ReportHeader.ReportHeaderEdit(' + item.ReportHeaderId + ', event);">' + item.LastUpdated + '</td>');
                $('#' + Clinical_ReportHeader.params.PanelID + " #dgvReportHeader tbody").last().append($row);
            });
            var checked = '';
            if (isactive == "0" || isactive == 0) {
                isactive = "0";
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }
            if ($.fn.dataTable.isDataTable('#' + Clinical_ReportHeader.params.PanelID + " #dgvReportHeader"))
                ;
            else {
                $('#' + Clinical_ReportHeader.params.PanelID + " #dgvReportHeader").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_ReportHeader.activeReportHeaderSearch(this);">' +
                         '</div><span class="pl-xs">Active</span>';

            $("#" + Clinical_ReportHeader.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else if (isactive == "1" || isactive == 1) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "0";
                checked = '';
            }
            $('#' + Clinical_ReportHeader.params.PanelID + " #dgvReportHeader").DataTable({
                "language": {
                    "emptyTable": "No Report Header Template is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                      '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_ReportHeader.activeReportHeaderSearch(this);">' +
                       '</div><span class="pl-xs">Active</span>' +
       '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + Clinical_ReportHeader.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }

        EMRUtility.SwicthWidgetInializatoin();

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template for active/ inactive records 
   Creation Date: March 02,2016 */
    activeReportHeaderSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        Clinical_ReportHeader.ReportHeaderSearch();
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template to delete records
   Creation Date: March 02,2016 */
    ReportHeaderDelete: function (ReportHeaderId) {

        utility.myConfirm('30', function () {
            if (ReportHeaderId > 0) {
                Clinical_ReportHeader.ReportHeaderDelete_DbCall(ReportHeaderId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_ReportHeader.ReportHeaderSearch();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });

    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template for edit records
   Creation Date: March 02,2016 */
    ReportHeaderEdit: function (ReportHeaderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Clinical_Report_Report Header", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (ReportHeaderId == "" || ReportHeaderId == "undefined") {
                }
                else {
                    var params = [];
                    params["ReportHeaderId"] = ReportHeaderId;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = 0;
                    LoadActionPan('Clinical_AddReportHeaderTemplate', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReportHeaderAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Report_Report Header", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["mode"] = "Add";
                params["FromAdmin"] = 0;
                LoadActionPan('Clinical_AddReportHeaderTemplate', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: to change active / in active records of Grid of Notes template 
   Creation Date: March 02,2016 */
    ReportHeaderActiveInactive: function (ReportHeaderId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (ReportHeaderId == "" || ReportHeaderId == "undefined") {
            }
            else {
                Clinical_ReportHeader.updateReportHeaderActiveInactive_Dbcall(ReportHeaderId, IsActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        utility.DisplayMessages(response.Message, 1);
                        Clinical_ReportHeader.ReportHeaderSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
                       '3', null, null, null, IsActive
                    );
    },

    //--------------------------------- DbCall Functions of Notes Template  start----------------------
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template
   Creation Date: March 02,2016 */
    searchReportHeader_DBCall: function (ReportHeaderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var IsActive = null;
        IsActive = $('#' + Clinical_ReportHeader.params.PanelID + ' #pnlReportHeader_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var self = $('#' + Clinical_ReportHeader.params.PanelID + ' #frmClinicalReportHeader')
        var objData = self != null ? self.getMyJSONByName() : "{}";
        objData = JSON.parse(objData);
        objData["ReportHeaderId"] = ReportHeaderId == null ? -1 : ReportHeaderId;
        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = IsActive;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "SEARCH_REPORT_HEADER";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template to delete records
   Creation Date: March 02,2016 */
    ReportHeaderDelete_DbCall: function (ReportHeaderId) {
        var objData = {};
        objData["ReportHeaderId"] = ReportHeaderId;
        objData["commandType"] = "DELETE_CLINICAL_REPORT_HEADER";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: to change active / in active records of Grid of Notes template 
   Creation Date: March 02,2016 */
    updateReportHeaderActiveInactive_Dbcall: function (ReportHeaderId, IsActive) {
        var objData = {};
        objData["ReportHeaderId"] = ReportHeaderId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_CLINICAL_REPORT_HEADER_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },

    //--------------------------------- DbCall Functions of Notes Template  END----------------------
    //Call to vet Report Header print html to add on print
    ReportHeaderPrint_DbCall: function (ProviderId, PatientId, formName) {
        var objData = {};
        objData["ProviderId"] = ProviderId == null || ProviderId == '' ? -1 : ProviderId;
        objData["PatientId"] = PatientId == null || PatientId == '' ? -1 : PatientId;
        objData["commandType"] = "GET_REPORT_HEADER_TAGS_HTML";
        if (formName==null) {
            formName = "Clinical Reports";
        }
        objData["FormName"] = formName;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },
    // ---------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------- Start Provider and Speciality Dropdown ----------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------
    //Load Specilties and Providers for all entities
    isEntitySelected: function () {
        var objDeffered = $.Deferred();
        $.when(Clinical_ReportHeader.loadEntitySpecialty('')).then(function () {
            $.when(Clinical_ReportHeader.loadEntityProvider('')).then(function () {
                objDeffered.resolve();
            });
        });
        return objDeffered;
    },


    //"loadEntitySpecialty" This function will load entity based specialty

    loadEntitySpecialty: function (entityID) {
        // Loads Spacialties Based on entityId
        var data = "EntityId=";
        MDVisionService.lookups('GetSpecialty', true, data).done(function (result) {
            result = JSON.parse(result["GetSpecialty"]);
            var options = result;
            $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').empty();
            $.each(options, function (i, item) {
                if (item.Value != "" && typeof item.Value != 'undefined') {
                    $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').append(
                    $('<option/>', {
                        value: item.Value,
                        html: item.Name + "(" + item.RefName + ")",
                        refname: item.RefName,
                        refvalue: item.RefValue

                    })
                );
                }
            });

            //Assign server side spacialties to the specialityCheckedIds array
            if (Clinical_ReportHeader.SpecialtyIds != '') {
                var Specialties = Clinical_ReportHeader.SpecialtyIds.split(",");
                Clinical_ReportHeader.specialityCheckedIds = Specialties;
                $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').val(Specialties);
            }
        }).then(function () {
            Clinical_ReportHeader.IntializeMultiSelectDropDownSpecialties();
        });
    },

    loadEntityProvider: function (entityId) {

        var data = "entityID=" + (entityId == "" ? -1 : entityId);
        if (entityId != null && entityId > 0 || entityId == '') {
            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlHiddenProvider');

                //Empty both the providers ddls.
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
                if (Clinical_ReportHeader.ProviderIds != '') {
                    var Providers = Clinical_ReportHeader.ProviderIds.split(",");
                    Clinical_ReportHeader.providerCheckedIds = Providers;
                    $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + Clinical_ReportHeader.params.PanelID + ' #divSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                Clinical_ReportHeader.IntializeMultiSelectDropDownProviders();

            });

        }
    },

    //This function will save spacialty ids and will show privders on spacialty selection
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + Clinical_ReportHeader.params.PanelID + ' #divSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_ReportHeader.specialityCheckedIds = [];
            Clinical_ReportHeader.providerCheckedIds = [];
            Clinical_ReportHeader.ProviderIds = '';
            Clinical_ReportHeader.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    if ($.inArray(Clinical_ReportHeader.specialityCheckedIds, spacialityId) == -1) {
                        Clinical_ReportHeader.specialityCheckedIds.push(spacialityId);
                    }
                }
                else {
                    Clinical_ReportHeader.specialityCheckedIds = Clinical_ReportHeader.removeFromArray(Clinical_ReportHeader.specialityCheckedIds, spacialityId);
                }
            }
            else {
                Clinical_ReportHeader.specialityCheckedIds = [];
                var values = $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').val();
                if ($.isArray(values)) {
                    Clinical_ReportHeader.specialityCheckedIds = values
                } else if (values != null && values != '') {
                    Clinical_ReportHeader.specialityCheckedIds.push(values);
                }
            }
        }
        $.when(Clinical_ReportHeader.filterProvidersBySpecialtyIds()).then(function () {
            if (Clinical_ReportHeader.ProviderIds != '') {
                var Providers = Clinical_ReportHeader.ProviderIds.split(",");
                if (Providers != '' && typeof Providers != 'undefined') {
                    $.each(Providers, function (index, item) {
                        Clinical_ReportHeader.providerCheckedIds = Clinical_ReportHeader.removeFromArray(Clinical_ReportHeader.providerCheckedIds, item);
                        if ($.inArray(Clinical_ReportHeader.providerCheckedIds, item) == -1) {
                            Clinical_ReportHeader.providerCheckedIds.push(item);
                        }
                    });
                }
            }
            $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider').val(Clinical_ReportHeader.providerCheckedIds);
            Clinical_ReportHeader.IntializeMultiSelectDropDownProviders();
        });
    },


    //This function will remove item from the "array and item" provided as input args
    removeFromArray: function (array, removeItem) {
        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },

    //This function will save spacialty ids and will show privders on spacialty selection
    filterProvidersBySpecialtyIds: function () {
        var providerHiddenContext = '#' + Clinical_ReportHeader.params.PanelID + ' #ddlHiddenProvider';
        var providerContext = '#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider';
        $(providerContext).empty();
        if (Clinical_ReportHeader.specialityCheckedIds.length > 0) {
            $.each(Clinical_ReportHeader.specialityCheckedIds, function (index, specialtyId) {
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


    //This function will save privder ids and will check speciality on provider selection
    checkSpecialtiesByProviderId: function (option, checked, select) {
        //provider context
        var providerContext = '#' + Clinical_ReportHeader.params.PanelID + ' #divProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            Clinical_ReportHeader.providerCheckedIds = [];
            Clinical_ReportHeader.ProviderIds = '';
        }
            //push all provider checked items
        else if (!isAllProviderSelected) {
            // provider value
            var providerValue = $(option).val();
            // add to provider array if checked
            if (checked) {
                if ($.inArray(Clinical_ReportHeader.providerCheckedIds, providerValue) == -1) {
                    Clinical_ReportHeader.providerCheckedIds.push(providerValue);
                }
            }
                //delete from provider array if not checked
            else {
                Clinical_ReportHeader.providerCheckedIds = Clinical_ReportHeader.removeFromArray(Clinical_ReportHeader.providerCheckedIds, providerValue);
            }
        }
        else {
            Clinical_ReportHeader.providerCheckedIds = [];
            var values = $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider').val();
            if ($.isArray(values)) {
                Clinical_ReportHeader.providerCheckedIds = values
            } else if (values != null && values != '') {
                Clinical_ReportHeader.providerCheckedIds.push(values);
            }
        }
        Clinical_ReportHeader.setSpacialtiesByselectedProviderIds();
    },
    //This function will set specialty Ids in specailChekedIds array
    setSpacialtiesByselectedProviderIds: function () {
        //$('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').multiselect('select', []);
        $.when($.each(Clinical_ReportHeader.providerCheckedIds, function (index, item) {

            $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider option:checked').each(function () {
                if ($(this).val() != '' && $(this).val() == item) {
                    if (jQuery.inArray($(this).attr('refname'), Clinical_ReportHeader.specialityCheckedIds) == -1) {
                        Clinical_ReportHeader.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        })).then(function () {
            if (Clinical_ReportHeader.specialityCheckedIds != null && Clinical_ReportHeader.specialityCheckedIds.length > 0) {
                $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').val(Clinical_ReportHeader.specialityCheckedIds);
                $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').multiselect("refresh");
            }

        });
    },

    // This function will initialize provider multiselect ddl
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_ReportHeader.checkSpecialtiesByProviderId(option, checked, select);
            },

        });
    },
    // which intialize all multi select dropdowns
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').multiselect('destroy');
        $('#' + Clinical_ReportHeader.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_ReportHeader.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_ReportHeader.SpecialtyIds != '') {
                    var spacialties = Clinical_ReportHeader.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_ReportHeader.specialityCheckedIds = Clinical_ReportHeader.removeFromArray(Clinical_ReportHeader.specialityCheckedIds, item);
                            Clinical_ReportHeader.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_ReportHeader.setSpacialtiesByselectedProviderIds();
            },
        });
    },
    // ---------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------- end Provider and Speciality Dropdown ----------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------
    //Author: Talha Tanweer
    //Date :  4th August 2016
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {

            utility.UnLoadDialog(Clinical_LabOrder.params.PanelID + ' #frmClinicalCDS', function () {


                //$("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.orderSearchGridId).attr('id', 'dgvLabOrder')
                //$("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId).attr('id', 'dgvLabResult')

                if (Clinical_LabOrder.params["FromAdmin"] == "0") {
                    if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_LabOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_LabOrder.params["FromAdmin"] == "0") {
                    if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_LabOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Clinical_LabOrder.params["FromAdmin"] == "0") {
                if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
                }
                else
                    UnloadActionPan(null, 'Clinical_LabOrder');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },
}
