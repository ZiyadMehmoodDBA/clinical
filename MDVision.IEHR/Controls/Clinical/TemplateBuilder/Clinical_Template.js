Clinical_Template = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_Template.params = params;
        if (Clinical_Template.bIsFirstLoad) {
            Clinical_Template.bIsFirstLoad = false;

          //  var self = $('#pnlClinicalTemplate');
            var self = "";
            if (Clinical_Template.params["PanelID"] != "pnlClinicalTemplate") {
                self = $('#' + Clinical_Template.params["PanelID"] + " #pnlClinicalTemplate")
            }
            else
                self = $('#' + Clinical_Template.params["PanelID"]);

            self.loadDropDowns(true);
            AppPrivileges.GetFormPrivileges("Template", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                   Clinical_Template.TemplateSearch();
                    
                }
            });
           
        }
    },

    TemplateAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ClinicalTemplateId"] = "-1";
                params["mode"] = "Add";
                params["IsUAdmin"] = "False";
                LoadActionPan('Clinical_Template_Detail', params);
               // LoadActionPan('userDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    TemplateSearch: function (TemplateID, PageNo, rpp) {
        var strMessage = "";
       // AppPrivileges.GetFormPrivileges("Template", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlClinicalTemplate #pnlTemplate_Result").css("display") == "none") {
                    $("#pnlClinicalTemplate #pnlTemplate_Result").show();
                }

                var self = $("#pnlTemplate_Search");
                var myJSON = self.getMyJSON();

                Clinical_Template.SearchTemplate(myJSON, TemplateID, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        //
                        if (response.TemplateCount > 0) {
                            $("#" + Clinical_Template.params["PanelID"] + " #divQuestionGroupPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;
                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divTemplatePaging", response.iTotalDisplayRecords, 5, "Clinical_Template", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Clinical_Template.params["PanelID"] + " #divTemplatePaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Clinical_Template.params["PanelID"] + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {

                            $("#" + Clinical_Template.params["PanelID"] + " #divTemplatePaging").css("display", "none");
                        }
                        Clinical_Template.TemplateGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                    
                });
            }
            else
                utility.DisplayMessages(response.Message, 2);
        //});
    },

    SearchTemplate: function (TemplateFormData, TemplateID, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "TemplateFormData=" + TemplateFormData + "&TemplateID=" + TemplateID + "&PageNo=" + PageNo + "&rpp=" + rpp;

        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "SEARCH_TEMPLATE");
    },

    TemplateGridLoad: function (response) {
        $("#pnlClinicalTemplate #dgvTemplate").dataTable().fnDestroy();
        $("#pnlClinicalTemplate #pnlTemplate_Result #dgvTemplate tbody").find("tr").remove();
        if (response.TemplateCount > 0) {
            var TemplateLoad_JSON = JSON.parse(response.TemplateLoad_JSON);
            $.each(TemplateLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvTemplate_row" + item.TemplateId + "'))");
                $row.attr("id", "gvTemplate_row" + item.TemplateID);
                $row.attr("TemplateID", item.TemplateID);
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
                $row.append('<td style="display:none;">' + item.TemplateId + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Template.TemplateDelete(' + item.TemplateId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Template.TemplateEdit(' + item.TemplateId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Template.TemplateActiveInactive(' + item.TemplateId + ',' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.TemplateType + '</td><td>' + item.SpecialityName + '</td><td>' + item.ProviderName + '</td><td>' + item.LetterName + '</td><td>' + item.FolderName + '</td><td>' + item.IsActive + '</td>');

                $("#pnlTemplate_Result #dgvTemplate tbody").last().append($row);
            });
        }
        else {
            $('#' + Clinical_Template.params["PanelID"] + ' #dgvTemplate tbody').html('');

            $('#pnlClinicalTemplate #dgvTemplate').DataTable({
                "language": {
                    "emptyTable": "No Template Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            var showingText = "Showing 0 to 0 of 0 Record(s)";
            $("#" + Clinical_Template.params["PanelID"] + " #divTemplatePaging #divShowingEntries").text(showingText);

        }


        if ($.fn.dataTable.isDataTable('#pnlClinicalTemplate #dgvTemplate'))
            ;
        else
            $('#' + Clinical_Template.params["PanelID"] + ' #pnlTemplate_Result #dgvTemplate').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            
    },

 

    TemplateActiveInactive: function (TemplateID, IsActive) {
        PageNo = null;
        rpp = null;
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = TemplateID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_Template.UpdateTemplateActiveInactive(selectedValue, IsActive, PageNo, rpp).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Clinical_Template.TemplateSearch('0');
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
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    
    UpdateTemplateActiveInactive: function (TemplateID, IsActive, PageNo, rpp) {
            if (PageNo == null) {
                PageNo = 1;
            }
            if (rpp == null) {
                rpp = 15;
            }
            var data = "TemplateID=" + TemplateID + "&IsActive=" + IsActive + "&PageNo=" + PageNo + "&rpp=" + rpp;
            // serach parameter , class name, command name of class 
            return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "UPDATE_TEMPLATE_ACTIVE_INACTIVE");
        },


    TemplateEdit: function (TemplateID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = TemplateID;
               
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ClinicalTemplateId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('Clinical_Template_Detail', params);
                    //LoadActionPan('userDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    TemplateDelete: function (TemplateID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                    utility.myConfirm('1', function () {
                        var selectedValue = TemplateID;
                        var oTable = $('#dgvTemplate').DataTable();
                        var ind = $(this).index();
                        var idx = oTable.row(this).index();
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Clinical_Template.DeleteTemplate(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    var table1 = $('#dgvTemplate').DataTable();
                                    table1.row('.active').remove().draw(false);
                                    utility.DisplayMessages(response.Message, 1);
                                    //utility.DisplayMessages("Record Deleted Successfully.", 1);
                                    Clinical_Template.TemplateSearch();
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
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DeleteTemplate: function (TemplateID) {
        var data = "TemplateID=" + TemplateID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "DELETE_TEMPLATE");
    },

    
    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}