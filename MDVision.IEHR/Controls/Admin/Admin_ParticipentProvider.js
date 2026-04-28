Admin_ParticipentProvider = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Admin_ParticipentProvider.params = params;
        if (Admin_ParticipentProvider.params["FromAdmin"] == "0" && Admin_ParticipentProvider.params["PanelID"] == 'pnlAdminParticipentProvider')
            Admin_ParticipentProvider.params["FromAdmin"] = "1";

        if (Admin_ParticipentProvider.bIsFirstLoad) {
            Admin_ParticipentProvider.bIsFirstLoad = false;
        }
        var self;
        if (Admin_ParticipentProvider.params["PanelID"] != 'pnlAdminParticipentProvider') {
            self = $('#' + Admin_ParticipentProvider.params["PanelID"] + ' #pnlAdminParticipentProvider');
        } else {
            self = $('#pnlAdminParticipentProvider');
        }
        self.loadDropDowns(true).done(function () {
        });
        Admin_ParticipentProvider.ParticipantProviderSearch();
    },
   
    ParticipantProviderSearch: function (ProviderParticipentId ,PageNo, rpp) {

        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("ParticipentProvider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
          //  if (strMessage == "") {
                if (!$("#pnlAdminParticipentProvider #pnlParticipent_Result").is(":visible")) {
                    $("#pnlAdminParticipentProvider #pnlParticipent_Result").show();
                }
                var self = $("#pnlParticipantProvider_Search");
                var ProviderId = "";
                var myJSON = self.getMyJSON();
                if (Admin_ParticipentProvider.params["ProviderId"] != null && Admin_ParticipentProvider.params["ProviderId"] > 0) {
                    ProviderId = Admin_ParticipentProvider.params["ProviderId"];
                    $("#txtProviderName").val(Admin_ParticipentProvider.params["ProviderName"]);
                }
                Admin_ParticipentProvider.SearchParticipantProvider(myJSON, ProviderParticipentId, ProviderId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        ;
                        Admin_ParticipentProvider.ParticipantProviderGridLoad(response);    //this will append table data in table body and create datatables instance
                        var TableControl = "pnlAdminParticipentProvider #dgvParticipent"; //Table ID
                        var PagingPanelControlID = "pnlAdminParticipentProvider #divParticipentPaging"; //Table Pagination ID
                        var ClassControlName = "Admin_ParticipentProvider";  //Javascipt Class Name for this form
                        var PagesToDisplay = 5; //Number of pages you need to display
                        var iTotalDisplayRecords = response.iTotalDisplayRecords; //Total number of records to display (Count)
                        //Setting Time out so that datatables instance is fully created.
                        setTimeout(
                            CreatePagination(response.ParticipantCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                            //Anonymous  function is for Pagination Call Backs
                            function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_ParticipentProvider.ParticipantProviderSearch(PrimaryID, PageNumber, ResultPerPage);
                            }),
                        10);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
           // }
            //else
            //    utility.DisplayMessages(strMessage, 2);
      //  });
    },
    SearchParticipantProvider: function (ParticipantProviderData, ParticipantProviderId, ProviderId, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ParticipantProviderData=" + ParticipantProviderData + "&ParticipantProviderId=" + ParticipantProviderId +"&ProviderId="+ProviderId+ "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // search parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PARTICIPANT_PROVIDER", "SEARCH_PARTICIPANT_PROVIDER");
    },
    ParticipentAdd: function () {

        var strMessage = "";
      //  AppPrivileges.GetFormPrivileges("ParticipentProvider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
           // if (strMessage == "") {
                var params = [];
                params["mode"] = "Add";
                params["ParentCtrl"] = "Admin_ParticipentProvider";
                if (Admin_Specialty.params["ParentCtrl"] && Admin_Specialty.params["ParentCtrl"].indexOf('Referrals') >= 0)
                    params["IsFromReferrals"] = true;
                else
                    params["IsFromReferrals"] = false;

                LoadActionPan('ParticipentProviderDetail', params);
            //}
            //else
            //    utility.DisplayMessages(strMessage, 2);
       // });
    },
    ParticipantProviderGridLoad: function (response) {

        $("#dgvParticipent").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlParticipent_Result #dgvParticipent tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.ParticipantCount > 0) {
            var ParticipantProviderLoadJSONData = JSON.parse(response.ParticipantProviderLoad_JSON); //Parsing array to JSON
           // $("#txtProviderFirstName").val(ParticipantProviderLoadJSONData[0].FirstName);
          //  $("#txtProviderLastName").val(ParticipantProviderLoadJSONData[0].LastName);
            $.each(ParticipantProviderLoadJSONData, function (i, item) {
                var ProviderName = item.ProviderName.split('-');
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_ParticipentProvider.ParticipantProviderEdit("+item.ParticipantProviderId+",event);");
                $row.attr("id", "gv_Participent_row" + item.ProviderParticipantId);
                $row.attr("ProviderParticipantId", item.ProviderParticipantId);

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
                var selectProviderParticipant = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                var selectMethod = "Admin_ParticipentProvider.FillParticipantProviderName('" + item.ProviderParticipantStatusId + "','" + "',event);"
                selectProviderParticipant = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    //if (ClassDisabled != "disabled")
                    //    $row.attr("onclick", selectMethod);
                $row.attr("onclick", "Admin_ParticipentProvider.ParticipantProviderEdit('" + item.ProviderParticipantId + "',event);");
                $row.append('<td style="display:none;">' + item.ProviderParticipantId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_ParticipentProvider.ParticipantProviderDelete(\'' + item.ProviderParticipantId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ParticipentProvider.ParticipantProviderEdit(\'' + item.ProviderParticipantId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ParticipentProvider.ParticipantProviderActiveInactive(\'' + item.ProviderParticipantId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectProviderParticipant + '</td><td>' + ProviderName[0] + '</td><td>' + item.Assignment + '</td><td>' + item.AssingnmentId + '</td><td>' + item.ProviderParticipantStatus + '</td><td>' + item.AssignmentTypeRTK + '</td><td>' + item.AssingnmentAdditionalId + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td>');
                $("#pnlParticipent_Result #dgvParticipent tbody").last().append($row);
            });
        }
        else {
            $('#pnlParticipent_Result #dgvParticipent').DataTable({
                "language": {
                    "emptyTable": "No Participant Provider Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlParticipent_Result #dgvParticipent'))
            ;
        else {
            $("#pnlParticipent_Result #dgvParticipent").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
    },
    FillParticipantProviderName: function (ProviderParticipantStatusId,ProviderStatus ,event) {
       
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = "#ddlParticipentStatus";
        
        var RefHiddenIdCtrl = " #hfParticipantStatus";
        if (Admin_ParticipentProvider.params["RefCtrl"] != null) {
            Admin_ParticipentProvider.params["RefCtrl"] = RefCtrl;
        }
        if (Admin_ParticipentProvider.params["RefCtrlHidden"] != null) {
            Admin_ParticipentProvider.params["RefCtrlHidden"] = RefHiddenIdCtrl
        }
        $('#' + Admin_ParticipentProvider.params["PanelID"] + RefHiddenIdCtrl).val(ProviderParticipantStatusId);
        $('#' + Admin_ParticipentProvider.params["PanelID"]).find("#" + Admin_ParticipentProvider.params["RefForm"]).find(RefCtrl).val(ProviderParticipantStatusId);
        UnloadActionPan(Admin_ParticipentProvider.params["ParentCtrl"], "Admin_ParticipentProvider");
        $('#' + Admin_ParticipentProvider.params["PanelID"]).find("#" + Admin_ParticipentProvider.params["RefForm"]).find(RefCtrl).focus();
    },
    ParticipantProviderEdit: function (ParticipantProviderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gv_Participent_row' + ParticipantProviderId));
     //   AppPrivileges.GetFormPrivileges("ParticipentProvider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
          //  if (strMessage == "") {

                var selectedValue = ParticipantProviderId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ParticipantProviderId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = "Admin_ParticipentProvider";

                    if (Admin_ParticipentProvider.params["ParentCtrl"] && Admin_ParticipentProvider.params["ParentCtrl"].indexOf('Referrals') >= 0)
                        params["IsFromReferrals"] = true;
                    else
                        params["IsFromReferrals"] = false;

                    LoadActionPan('ParticipentProviderDetail', params);
                }
          //  }
            //else
            //    utility.DisplayMessages(strMessage, 2);
       // });
    },
    ParticipantProviderDelete: function (ParticipantProviderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gv_Participent_row' + ParticipantProviderId));
   //     AppPrivileges.GetFormPrivileges("ParticipentProvider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
           // if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ParticipantProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ParticipentProvider.DeleteParticipantProvider(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvParticipent').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                               // CacheManager.BindCodes('GetSpecialty', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
           // }
            //else
            //    utility.DisplayMessages(strMessage, 2);
      //  });
    },
    DeleteParticipantProvider: function (ParticipantProviderId) {
        var data = "ParticipantProviderId=" + ParticipantProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PARTICIPANT_PROVIDER_DETAIL", "DELETE_PARTICIPANT_PROVIDER");
        
    },
    ParticipantProviderActiveInactive: function (ParticipantProviderId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
       // AppPrivileges.GetFormPrivileges("ParticipentProvider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
          //  if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ParticipantProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        ParticipentProviderDetail.UpdateParticipantProviderActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ParticipentProvider.ParticipantProviderSearch('0');
                                CacheManager.BindCodes('GetProviderParticipentStatus', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '3', null, null, null, IsActive
                );
            //}
            //else
            //    utility.DisplayMessages(strMessage, 2);
       // });
    },
    UnLoadTab: function (Tab) {
        if (Admin_ParticipentProvider.params["FromAdmin"] == "0") {


            if (Admin_ParticipentProvider.params != null && Admin_ParticipentProvider.params.ParentCtrl != null && Admin_ParticipentProvider.params.PanelID != 'pnlAdminParticipentProvider') {
                UnloadActionPan(Admin_ParticipentProvider.params.ParentCtrl, 'Admin_ParticipentProvider', null, Admin_ParticipentProvider.params.PanelID);
            }

            else if (Admin_ParticipentProvider.params != null && Admin_ParticipentProvider.params.ParentCtrl != null) {
                UnloadActionPan(Admin_ParticipentProvider.params.ParentCtrl, 'Admin_ParticipentProvider');
            }

            else
                UnloadActionPan(null, 'Admin_ParticipentProvider');
        }
        else {
            RemoveAdminTab();
        }
    },
    //UnLoad: function () {
    //    utility.UnLoadDialog("pnlAdminParticipentProvider", function () {
    //        Admin_ParticipentProvider.UnloadPan();
    //    }, function () {
    //        Admin_ParticipentProvider.UnloadPan();
    //    });
    //},

    UnloadPan: function () {

        if (Admin_ParticipentProvider.params != null && Admin_ParticipentProvider.params.ParentCtrl != null) {
            UnloadActionPan(Admin_ParticipentProvider.params.ParentCtrl, 'pnlAdminParticipentProvider');
        }
        else
            UnloadActionPan();
    },
  

}