Patient_Guarantor = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_Guarantor.params = params;
        if (Patient_Guarantor.bIsFirstLoad) {
            Patient_Guarantor.bIsFirstLoad = false;
            var self = $('#pnlGuarantor');
            self.loadDropDowns(true);
            if (Patient_Guarantor.params.GuarantorId != "-1")
            { Patient_Guarantor.ValidateSearchCriteria(); }
        }
    },

    GuarantorAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Guarantor", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["GuarantorId"] = null;
                params["mode"] = "Add";
                if (Patient_Guarantor.params.ParentCtrl == "demographicDetail" || Patient_Guarantor.params.ParentCtrl == "patTabDemographic") {
                    params["Address1"] = Patient_Guarantor.params.Address1;
                    params["Zip"] = Patient_Guarantor.params.Zip;
                    params["City"] = Patient_Guarantor.params.City;
                    params["State"] = Patient_Guarantor.params.State;
                }
                params["ParentCtrl"] = 'Patient_Guarantor';
                LoadActionPan('guarantorDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    GuarantorEdit: function (GuarantorId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvGuarantor_row' + GuarantorId));
        AppPrivileges.GetFormPrivileges("Guarantor", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = GuarantorId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["GuarantorId"] = selectedValue;
                    params["mode"] = "Edit";
                    if (Patient_Guarantor.params.ParentCtrl == "demographicDetail" || Patient_Guarantor.params.ParentCtrl == "patTabDemographic") {
                        params["Address1"] = Patient_Guarantor.params.Address1;
                        params["Zip"] = Patient_Guarantor.params.Zip;
                        params["City"] = Patient_Guarantor.params.City;
                        params["State"] = Patient_Guarantor.params.State;
                    }
                    params["ParentCtrl"] = 'Patient_Guarantor';
                    LoadActionPan('guarantorDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    GuarantorDelete: function (GuarantorId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvGuarantor_row' + GuarantorId));
        AppPrivileges.GetFormPrivileges("Guarantor", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = GuarantorId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Guarantor.DeleteGuarantor(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvGuarantor').DataTable();
                                //table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                if (Patient_Guarantor.params.ParentCtrl == "patTabDemographic" || Patient_Guarantor.params.ParentCtrl == "demographicDetail") {
                                    var pnlParent = null;
                                    if (Patient_Guarantor.params.ParentCtrl == "patTabDemographic")
                                        pnlParent = 'pnlDemographic';
                                    else if (Patient_Guarantor.params.ParentCtrl == "demographicDetail")
                                        pnlParent = 'pnldemographicDetail';

                                    utility.GetGuarontorArray($('#' + pnlParent + ' #txtGuarantor').val()).done(function (response) {

                                        $('#' + pnlParent + ' #txtGuarantor').autocomplete({
                                            //source: AllPatients, // pass an array (without a comma)
                                            autoFocus: true,
                                            source: response,
                                            select: function (event, ui) {

                                                setTimeout(function () {

                                                    $('#' + pnlParent + ' #hfGuarantor').val(ui.item.id); // add the selected id
                                                    if ($('#' + pnlParent + ' #lnkGuarantorEdit').css("display") == "none") {
                                                        $('#' + pnlParent + ' #lnkGuarantorEdit').css("display", "inline");
                                                        $('#' + pnlParent + ' #lblGuarantor').css("display", "none");
                                                    }
                                                }, 100);
                                            }
                                        });
                                        $('#' + pnlParent + ' #txtGuarantor').trigger('onblur');
                                    });
                                }
                                Patient_Guarantor.GuarantorSearch();
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

    GuarantorActiveInactive: function (GuarantorId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Guarantor", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = GuarantorId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        guarantorDetail.UpdateGuarantorActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                //CacheManager.BindCodes('GetGuarantor', true);

                                var pnlParent = null;
                                if (Patient_Guarantor.params.ParentCtrl == "patTabDemographic")
                                    pnlParent = 'pnlDemographic';
                                else if (Patient_Guarantor.params.ParentCtrl == "demographicDetail")
                                    pnlParent = 'pnldemographicDetail';

                                utility.GetGuarontorArray($('#' + pnlParent + ' #txtGuarantor').val()).done(function (response) {

                                    $('#' + pnlParent + ' #txtGuarantor').autocomplete({
                                        //source: AllPatients, // pass an array (without a comma)
                                        autoFocus: true,
                                        source: response,
                                        select: function (event, ui) {

                                            setTimeout(function () {

                                                $('#' + pnlParent + ' #hfGuarantor').val(ui.item.id); // add the selected id
                                                if ($('#' + pnlParent + ' #lnkGuarantorEdit').css("display") == "none") {
                                                    $('#' + pnlParent + ' #lnkGuarantorEdit').css("display", "inline");
                                                    $('#' + pnlParent + ' #lblGuarantor').css("display", "none");
                                                }
                                            }, 100);
                                        }
                                    });
                                    $('#' + pnlParent + ' #txtGuarantor').trigger('onblur');
                                });

                                Patient_Guarantor.GuarantorSearch('0');
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

    GuarantorSearch: function (GuarantorId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Guarantor", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlGuarantor #pnlGuarantor_Result").css("display") == "none") {
                    $("#pnlGuarantor #pnlGuarantor_Result").show();
                }

                var self = $("#pnlGuarantor_Search");
                var myJSON = self.getMyJSON();

                Patient_Guarantor.SearchGuarantor(myJSON, GuarantorId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_Guarantor.GuarantorGridLoad(response, PageNo, rpp);
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

    GuarantorGridLoad: function (response, PageNo, rpp) {

        //Set Custom Paging
        if (response.GuarantorCount > 0) {
            $("#pnlGuarantor_Result #divGuarantorPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divGuarantorPaging", response.iTotalDisplayRecords, 5, "Patient_Guarantor", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlGuarantor_Result #divGuarantorPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlGuarantor #pnlGuarantor_Result li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                } else
                    $(this).removeAttr("class");
            });
        } else {
            $("#pnlGuarantor #divGuarantorPaging").css("display", "none");
        }


        $("#dgvGuarantor").dataTable().fnDestroy();
        $("#pnlGuarantor_Result #dgvGuarantor tbody").find("tr").remove();
        if (response.GuarantorCount > 0) {
            var GuarantorLoadJSONData = JSON.parse(response.GuarantorLoad_JSON);
            $.each(GuarantorLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvGuarantor_row" + item.GuarantorId);
                $row.attr("GuarantorId", item.GuarantorId);

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
                var selectGuarantor = "";
                var selectMethod = "";

                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";

                if (Patient_Guarantor.params["FromAdmin"] == "0") {
                    if (Patient_Guarantor.params["RefCtrl"] == "txtGuarantor") {
                        selectMethod = "Patient_Guarantor.FillGuarantorName('" + item.GuarantorId + "','" + item.FullName + "',event,'" + item.FirstName + "','" + item.LastName + "');"
                        selectGuarantor = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '"  onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>';
                    }

                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Patient_Guarantor.GuarantorEdit('" + item.GuarantorId + "',event);");
                }

                $row.append('<td style="display:none;">' + item.GuarantorId + '</td><td><a class="btn btn-xs" href="#" onclick="Patient_Guarantor.GuarantorDelete(\'' + item.GuarantorId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Guarantor.GuarantorEdit(\'' + item.GuarantorId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Guarantor.GuarantorActiveInactive(\'' + item.GuarantorId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectGuarantor + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.RelationName + '</td><td>' + item.PhoneNo + '</td><td>' + item.Address1 + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.ZipCode + '</td>');

                $("#pnlGuarantor_Result #dgvGuarantor tbody").last().append($row);
            });
        }
        else {
            $('#pnlGuarantor_Result #dgvGuarantor').DataTable({
                "language": {
                    "emptyTable": "No Guarantor Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlGuarantor_Result #dgvGuarantor'))
            ;
        else {
            $("#pnlGuarantor_Result #dgvGuarantor").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
        }
    },



    //Custom Paging Overloaded Methods

    SelectedPageClick: function (PageNo, objPage, frm, to, pagingDivId) {

        var selecter = "#pnlGuarantor_Result li";

        // Change Background Color to Black for selected page
        $(selecter).each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        Patient_Guarantor.GuarantorSearch("0", PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var selecter = "#pnlGuarantor_Result li";

        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {

            Patient_Guarantor.GuarantorSearch("0", currentPageNo, 15);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var selecter = "#pnlGuarantor_Result li";


        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Patient_Guarantor.GuarantorSearch("0", currentPageNo, 15);
        }
    },



    FillGuarantorName: function (GuarantorId, GuarantorName, event, firstName, lastName) {
        if (event != null) {
            event.stopPropagation();
        }

        $('#' + Patient_Guarantor.params["PanelID"] + ' #txtGuarantor').val(GuarantorName);
        $('#' + Patient_Guarantor.params["PanelID"] + ' #hfGuarantor').val(GuarantorId);
        $('#' + Patient_Guarantor.params["PanelID"] + ' #lblGuarantor').css("display", "none");
        $('#' + Patient_Guarantor.params["PanelID"] + ' #lnkGuarantorEdit').css("display", "inline");
        utility.SetKendoAutoCompleteSourceforValidate($('#' + Patient_Guarantor.params["PanelID"] + ' #txtGuarantor'), GuarantorName, $('#' + Patient_Guarantor.params["PanelID"] + ' #hfGuarantor'), GuarantorId);
        UnloadActionPan(Patient_Guarantor.params["ParentCtrl"], "Patient_Guarantor");
    },

    SearchGuarantor: function (GuarantorData, GuarantorId, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var data = "GuarantorData=" + GuarantorData + "&GuarantorID=" + GuarantorId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_GUARANTOR", "SEARCH_GUARANTOR");
    },

    DeleteGuarantor: function (GuarantorId) {
        var data = "GuarantorID=" + GuarantorId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_GUARANTOR_DETAIL", "DELETE_GUARANTOR");
    },

    UnLoad: function () {
        if (Patient_Guarantor.params != null && Patient_Guarantor.params.ParentCtrl != null) {
            UnloadActionPan(Patient_Guarantor.params.ParentCtrl, 'Patient_Guarantor');
        }
        else
            UnloadActionPan(null, 'Patient_Guarantor');
    },
    ValidateSearchCriteria: function () {
        utility.ValidateSearchCriteria("pnlGuarantor #pnlGuarantor_Search", function () {
            Patient_Guarantor.GuarantorSearch();
        });
    }

}