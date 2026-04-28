Clinical_Section = {
    bIsFirstLoad: true,
    params: [],
    Load: function (param) {

        Clinical_Section.params = params;

        if (Clinical_Section.bIsFirstLoad) {
            Clinical_Section.bIsFirstLoad = false;

            //var self = "";
            //if (Clinical_Section.params["PanelID"] != "pnlClinicalSection") {
            //    self = $('#' + Clinical_Section.params["PanelID"] + " #pnlClinicalSection")
            //}
            //else
            //    self = $('#' + Clinical_Section.params["PanelID"]);

            var self = $('#pnlClinicalSection');
            //self.loadDropDowns(true);

            self.loadDropDowns(true).done(function () {
                Clinical_Section.SectionSearch();
            });

            //AppPrivileges.GetFormPrivileges("Question", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            //    }
            //});

        }
        //$('#dgvSection_info').remove(); $('#dgvSection_paginate').remove();

    },

    SectionAdd: function () {
        var params = [];
        params["SectionId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('sectionDetail', params);
    },

    UnLoadTab: function () {
        if (Clinical_Section.params["FromAdmin"] == "0") {
            if (Clinical_Section.params != null && Clinical_Section.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_Section.params.ParentCtrl, 'Clinical_Section');
            }
            else
                UnloadActionPan(null, 'Clinical_Section');
        }
        else {
            RemoveAdminTab();
        }
    },

    SectionEdit: function (sectionId, mode, sectionTypeId) {
        // var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Question", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {
        //var selectedValue = UserId;
        // var selectedAdmin = IsUAdmin;
        // if (selectedValue == "" || selectedValue == "undefined") {
        // }
        // else {

        var params = [];
        params["SectionId"] = sectionId;
        params["mode"] = mode;
        params["SectionTypeId"] = sectionTypeId;
        LoadActionPan('sectionDetail', params);
        //LoadActionPan('userDetail', params);
        //   }
        //}
        // else
        //     utility.DisplayMessages(strMessage, 2);
        //});
    },

    SectionDelete: function (sectionId) { //AppPrivileges.GetFormPrivileges("Question", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

        var strMessage = "";
        if (strMessage == "") {

            utility.myConfirm('1', function () {
                var selectedValue = sectionId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_Section.DeleteSection(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#dgvSection').DataTable();
                            table1.row('.active').remove().draw(false);
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_Section.SectionSearch();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () {
            },
                '1'
            );
        }
        //else
        //    utility.DisplayMessages(strMessage, 2);
        //});
    },

    DeleteSection: function (sectionId) {
        var data = "sectionID=" + sectionId;
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "DELETE_SECTION");
    },

    SectionActiveInactive: function (sectionId, isActive) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Question", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        // if (strMessage == "") {
        //if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
        //{ utility.DisplayMessages("Admin User can't be Active / In Active !", 3); }
        //else
        //{
        utility.myConfirm('3', function () {
            var selectedValue = sectionId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                Clinical_Section.UpdateSectionActiveInactive(selectedValue, isActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Clinical_Section.SectionSearch('0');
                        UnloadActionPan();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '3', null, null, null, isActive
        );
        //}
        // }
        // else
        //   utility.DisplayMessages(strMessage, 2);
        // });
    },

    UpdateSectionActiveInactive: function (sectionId, isActive) {
        var data = "SectionID=" + sectionId + "&IsActive=" + isActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "UPDATE_SECTION_ACTIVE_INACTIVE");
    },

    SectionSearch: function (sectionId, PageNo, rpp) {
        var strMessage = "";
        if (strMessage == "") {
            if ($('#' + Clinical_Section.params["PanelID"] + ' #pnlSection_Result').css("display") == "none") {
                $('#' + Clinical_Section.params["PanelID"] + ' #pnlSection_Result').show();
            }
            var self = $("#pnlClinicalSection");
            var myJson = self.getMyJSON();

            Clinical_Section.SearchSection(myJson, sectionId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    if (response.SectionCount > 0) {

                        $("#" + Clinical_Section.params["PanelID"] + " #divSectionPaging").css("display", "inline");


                        //Showing 1 to 15 of 15 entries
                        var RecordsPerPage = rpp != null ? rpp : 15;
                        var CurrentPage = PageNo != null ? PageNo : 1;
                        var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                        var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                        if (PageNo == null) {
                            utility.GetCustomPaging("divSectionPaging", response.iTotalDisplayRecords, 5, "Clinical_Section", CurrentPage, RecordsPerPage);
                        }
                        var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                        var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                        $("#" + Clinical_Section.params["PanelID"] + " #divSectionPaging #divShowingEntries").text(showingText);
                        // Change Background Color to Black for selected page
                        $("#" + Clinical_Section.params["PanelID"] + " li").each(function () {
                            if ($(this).text() == CurrentPage) {
                                $(this).attr("class", "active");
                            }
                            else
                                $(this).removeAttr("class");
                        });
                    }
                    else {
                        $("#" + Clinical_Section.params["PanelID"] + " #divSectionPaging").css("display", "none");
                    }
                    Clinical_Section.SectionGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                
            });
        }
        else
            utility.DisplayMessages(response.Message, 2);
       $('#dgvSection_info').remove(); $('#dgvSection_paginate').remove();
    },

    SearchSection: function (sectionData, sectionId, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "SectionData=" + sectionData + "&sectionID=" + sectionId + "&PageNo=" + PageNo + "&rpp=" + rpp;
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "SEARCH_SECTION");
    },

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#" + Clinical_Section.params["PanelID"] + " li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Clinical_Section.SectionSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Clinical_Section.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Clinical_Section.SectionSearch(null, currentPageNo, 15);

        }
        PageNo = currentPageNo;
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Clinical_Section.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Clinical_Section.SectionSearch(null, currentPageNo, 15);
        }
        PageNo = currentPageNo;
    },

    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    SectionGridLoad: function (response) {
        $("#pnlClinicalSection #dgvSection").dataTable().fnDestroy();
        $("#pnlClinicalSection #pnlSection_Result #dgvSection tbody").find("tr").remove();
        if (response.SectionCount > 0) {
            var sectionLoadJsonData = JSON.parse(response.SectionLoad_JSON);
            $.each(sectionLoadJsonData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvSection_row" + item.SectionId + "'))");
                $row.attr("id", "gvSection_row" + item.SectionId);
                $row.attr("SectionId", item.SectionId);
                //$row.attr("ShortName", item.ShortName);
                //$row.attr("Description", item.Description);
                //$row.attr("Speciality", item.SpecialityName);

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

                $row.append('<td style="display:none;">' + item.SectionId + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Section.SectionDelete(' + item.SectionId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Section.SectionEdit(' + item.SectionId + ",'Edit'," + item.SectionTypeId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Section.SectionActiveInactive(' + item.SectionId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.SectionTitle + '</td><td>' + item.Description + '</td><td>' + item.SectionTypeShortName + '</td><td>' + item.SpecialityName + '</td><td>' + item.IsActive + '</td>');

                $("#pnlSection_Result #dgvSection tbody").last().append($row);
            });
        }
        else {
            $('#pnlClinicalSection #dgvSection').DataTable({
                "language": {
                    "emptyTable": "No Section Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlClinicalSection #dgvSection'))
            ;
        else
            $('#pnlClinicalSection #pnlSection_Result #dgvSection').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //   $("#pnlClinicalSection #pnlSection_Result #dgvSection").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('#dgvSection_info').remove(); $('#dgvSection_paginate').remove();
    }
}
