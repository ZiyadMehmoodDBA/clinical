
Admin_HL7SIU = {
    bIsFirstLoad: true,
    params: [],
    Total: null,
    Rejected: null,
    Accepted: null,

    Load: function (params) {
        Admin_HL7SIU.params = params;

        if (Admin_HL7SIU.params["FromAdmin"] == "0" && Admin_HL7SIU.params["PanelID"] == 'pnlAdminHL7SIU')
            Admin_HL7SIU.params["FromAdmin"] = "1";

        if (Admin_HL7SIU.bIsFirstLoad) {
            Admin_HL7SIU.bIsFirstLoad = false;

            var self = "";
            if (Admin_HL7SIU.params["PanelID"] != 'pnlAdminHL7SIU')
                self = $('#' + Admin_HL7SIU.params["PanelID"] + ' #pnlAdminHL7SIU');
            else
                self = $('#pnlAdminHL7SIU');

            self.loadDropDowns(true).done(function () {

                utility.CreateDatePicker('pnlAdminHL7SIU #CreatedOn', function () {
                    //on-change callback method 
                }, true);

                Admin_HL7SIU.HL7SIUSearch();
            });
        }
    },

    HL7SIUAdd: function () {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("HL7SIU", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var params = [];
            params["MirthLogID"] = null;
            params["mode"] = "Add";
            params["FromAdmin"] = Admin_HL7SIU.params["FromAdmin"];
            if (Admin_HL7SIU.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'Admin_HL7SIU';
            }
            LoadActionPan('HL7SIUDetail', params);
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});
    },

    HL7SIUEdit: function (MirthLogID) {
        var strMessage = "";
        utility.SelectGridRow($('#gvHL7SIU_row' + MirthLogID));
        //AppPrivileges.GetFormPrivileges("HL7SIU", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var selectedValue = MirthLogID;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var params = [];
                params["MirthLogID"] = selectedValue;
                params["mode"] = "Edit";
                params["FromAdmin"] = Admin_HL7SIU.params["FromAdmin"];
                if (Admin_HL7SIU.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_HL7SIU';
                }
                LoadActionPan('HL7SIUDetail', params);
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});
    },

    HL7SIUDelete: function (MirthLogID) {
        var strMessage = "";
        utility.SelectGridRow($('#gvHL7SIU_row' + MirthLogID));
        utility.SelectGridRow($("#" + Admin_HL7SIU.params["PanelID"] + ' #gvHL7SIU_row' + MirthLogID));
        //AppPrivileges.GetFormPrivileges("HL7SIU", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = MirthLogID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_HL7SIU.DeleteHL7SIU(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#' + Admin_HL7SIU.params["PanelID"] + ' #dgvHL7SIU').DataTable();
                            table1.row('.active').remove().draw(false);
                            utility.DisplayMessages(response.Message, 1);
                            //CacheManager.BindCodes('GetHL7SIU', true);
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
        //});
    },

    HL7SIUActiveInactive: function (MirthLogID, IsActive, event) {
        var strMessage = "";

        //AppPrivileges.GetFormPrivileges("HL7SIU", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = MirthLogID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    HL7SIUDetail.UpdateHL7SIUActiveInactive(selectedValue, IsActive).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_HL7SIU.HL7SIUSearch('0');
                            //CacheManager.BindCodes('GetHL7SIU', true);
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
        //});
    },

    HL7SIUSearch: function (MirthLogID, PageNo, rpp) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("HL7SIU", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($("#" + Admin_HL7SIU.params["PanelID"] + " #pnlHL7SIU_Result").css("display") == "none") {
                $("#" + Admin_HL7SIU.params["PanelID"] + " #pnlHL7SIU_Result").show();
            }

            var self = $("#" + Admin_HL7SIU.params["PanelID"] + " #pnlAdminHL7SIU_Search");
            var myJSON = self.getMyJSON();

            Admin_HL7SIU.SearchHL7SIU(myJSON, MirthLogID, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_HL7SIU.HL7SIUGridLoad(response);

                    $("#pnlAdminHL7SIU #total").text(Total);
                    $("#pnlAdminHL7SIU #accepted").text(Accepted);
                    $("#pnlAdminHL7SIU #rejected").text(Rejected);

                    var TableControl = Admin_HL7SIU.params["PanelID"] + " #dgvHL7SIU";
                    var PagingPanelControlID = Admin_HL7SIU.params["PanelID"] + " #divHL7SIUPaging";
                    var ClassControlName = "Admin_HL7SIU";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.HL7SIUCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_HL7SIU.HL7SIUSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});
    },

    HL7SIUGridLoad: function (response) {
        $("#" + Admin_HL7SIU.params["PanelID"] + " #dgvHL7SIU").dataTable().fnDestroy();
        $("#" + Admin_HL7SIU.params["PanelID"] + " #pnlHL7SIU_Result #dgvHL7SIU tbody").find("tr").remove();
        if (response.HL7SIUCount > 0) {
            var HL7SIULoad_JSON = JSON.parse(response.HL7SIULoad_JSON);
            $.each(HL7SIULoad_JSON, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvHL7SIU_row" + item.MirthLogID);
                $row.attr("MirthLogID", item.MirthLogID);
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

                var selectHL7SIU = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                //if (Admin_HL7SIU.params["FromAdmin"] == "0") {
                //    var selectMethod = "Admin_HL7SIU.FillHL7SIUName('" + item.MirthLogID + "','" + item.ShortName + "','" + item.PracticeId + "','" + item.PracticeName + "','" + item.EntityName + "',event);"
                //    selectHL7SIU = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                //    $row.attr("onclick", selectMethod);
                //}
                //else {
                //    $row.attr("onclick", "Admin_HL7SIU.HL7SIUEdit('" + item.MirthLogID + "',event);");
                //}
                if (Admin_HL7SIU.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    //$row.append('<td style="display:none;">' + item.MirthLogID + '</td><td>' + selectHL7SIU + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.MirthLogID + '</td><td>' + item.FileName + '</td><td>' + item.MessageStatus + '</td><td>' + item.MRNumber + '</td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.Duration + '</td><td>' + item.StartDateTime + '</td><td>' + item.EndDateTime + '</td><td>' + item.CreatedOn + '</td><td>' + item.ErrorMessage + '</td>');

                } else {
                    $('#btn-add').show();
                    //$row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_HL7SIU.HL7SIUDelete(' + item.MirthLogID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_HL7SIU.HL7SIUEdit(' + item.MirthLogID + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_HL7SIU.HL7SIUActiveInactive(' + item.MirthLogID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectHL7SIU + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    var Start, End;
                    if (item.StartDateTime != "") {
                        Start = item.StartDateTime.substring(0, 8) + '-' + item.StartDateTime.substring(8, 10) + ':' + item.StartDateTime.substring(10, 12) + ':' + item.StartDateTime.substring(12, 14);
                    }
                    else { Start = "";}
                    if (item.EndDateTime != "") {
                        End = item.EndDateTime.substring(0, 8) + '-' + item.EndDateTime.substring(8, 10) + ':' + item.EndDateTime.substring(10, 12) + ':' + item.EndDateTime.substring(12, 14);
                    } else { End = ""; }
                    
                    
                    $row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a href="#" onclick="Admin_HL7SIU.ShowHL7File(' + item.MirthLogID + ', \'' + item.FileText + '\');" title="View File"><i>' + item.FileName + '</i></a></td><td>' + item.MessageStatus + '</td><td>' + item.MRNumber + '</td><td>' + item.PatientName + '</td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.Duration + '</td><td>' + Start + '</td><td>' + End + '</td><td>' + item.AppStatus + '</td><td>' + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ErrorMessage + '</td>');
                }
                $("#" + Admin_HL7SIU.params["PanelID"] + " #pnlHL7SIU_Result #dgvHL7SIU tbody").last().append($row);
                
                Total = item.RecordCount;
                Accepted = item.Accepted;
                Rejected = item.Rejected;
            });
        }
        else {
            $('#' + Admin_HL7SIU.params["PanelID"] + ' #dgvHL7SIU').DataTable({
                "language": {
                    "emptyTable": "No HL7SIU Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            Total = 0;
            Accepted = 0;
            Rejected = 0;

        }
        if ($.fn.dataTable.isDataTable('#' + Admin_HL7SIU.params["PanelID"] + ' #dgvHL7SIU'))
            ;
        else
            $("#" + Admin_HL7SIU.params["PanelID"] + " #pnlHL7SIU_Result #dgvHL7SIU").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_HL7SIU.params["PanelID"] + " #pnlHL7SIU_Result #dgvHL7SIU").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ShowHL7File: function (MirthLogID, FileText) {

        var strMessage = "";
        utility.SelectGridRow($('#gvHL7SIU_row' + MirthLogID));
        //AppPrivileges.GetFormPrivileges("EDIServiceHandle", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var selectedValue = MirthLogID;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var params = [];
                params["MirthLogID"] = MirthLogID;
                params["FileText"] = FileText;
                LoadActionPan('Admin_HL7ViewFile', params);
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    FillHL7SIUName: function (MirthLogID, HL7SIUName, PracticeId, PracticeName, EntityName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtHL7SIU";
        var RefHiddenIdCtrl = " #hfHL7SIU";
        if (Admin_HL7SIU.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_HL7SIU.params["RefCtrl"];
        }
        if (Admin_HL7SIU.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_HL7SIU.params["RefHiddenIdCtrl"];
        }

        if (Admin_Provider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_HL7SIU.params["PanelID"] + RefCtrl).val(HL7SIUName + ' - ' + EntityName).focus();
            else
                $('#' + Admin_HL7SIU.params["PanelID"] + RefCtrl).val(HL7SIUName).focus();
        }
        else {
            $('#' + Admin_HL7SIU.params["PanelID"] + RefCtrl).val(HL7SIUName).focus();
        }
        //$('#' + Admin_HL7SIU.params["PanelID"] + RefCtrl).val(HL7SIUName).focus();
        $('#' + Admin_HL7SIU.params["PanelID"] + RefHiddenIdCtrl).val(MirthLogID);


        if (Admin_HL7SIU.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_HL7SIU.params["MirthLogID"] = MirthLogID;
            Admin_HL7SIU.params["PracticeId"] = PracticeId;
            $('#' + Admin_HL7SIU.params["PanelID"] + ' #lblHL7SIU').css("display", "inline");
        } else {
            if (Admin_HL7SIU.params["IsOptional"] != null && Admin_HL7SIU.params["RefForm"] != null && Admin_HL7SIU.params["IsOptional"] == false) {
                if ($('#' + Admin_HL7SIU.params["PanelID"] + ' #' + Admin_HL7SIU.params["RefForm"]).data('bootstrapValidator') != null && typeof $('#' + Admin_HL7SIU.params["PanelID"] + ' #' + Admin_HL7SIU.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
                    $('#' + Admin_HL7SIU.params["PanelID"] + ' #' + Admin_HL7SIU.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_HL7SIU.params.PanelID + RefCtrl).attr("name"));
                }
            }
            $('#' + Admin_HL7SIU.params.PanelID + ' #txtPractice').val(PracticeName);
            $('#' + Admin_HL7SIU.params.PanelID + ' #hfPractice').val(PracticeId);
            $('#' + Admin_HL7SIU.params["PanelID"] + ' #lblHL7SIU').css("display", "none");
            $('#' + Admin_HL7SIU.params["PanelID"] + ' #lnkHL7SIUEdit').css("display", "inline");
        }

        if (Admin_HL7SIU.params != null && Admin_HL7SIU.params.ParentCtrl != null && Admin_HL7SIU.params.PanelID != 'pnlAdminHL7SIU') {
            UnloadActionPan(Admin_HL7SIU.params.ParentCtrl, 'Admin_HL7SIU', null, Admin_HL7SIU.params.PanelID);
        }
        else
            UnloadActionPan(Admin_HL7SIU.params["ParentCtrl"], "Admin_HL7SIU");

        $('#' + Admin_HL7SIU.params["PanelID"] + RefCtrl).focus();
    },

    SearchHL7SIU: function (HL7SIUData, MirthLogID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "HL7SIUData=" + HL7SIUData + "&MirthLogID=" + MirthLogID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HL7", "SEARCH_HL7_SIU");
    },

    DeleteHL7SIU: function (MirthLogID) {
        var data = "MirthLogID=" + MirthLogID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HL7", "DELETE_EDI_SERVICE_HANDLE");
    },

    UnLoadTab: function (Tab) {
        if (Admin_HL7SIU.params["FromAdmin"] == "0") {


            if (Admin_HL7SIU.params != null && Admin_HL7SIU.params.ParentCtrl != null && Admin_HL7SIU.params.PanelID != 'pnlAdminHL7SIU') {
                UnloadActionPan(Admin_HL7SIU.params.ParentCtrl, 'Admin_HL7SIU', null, Admin_HL7SIU.params.PanelID);
            }

            else if (Admin_HL7SIU.params != null && Admin_HL7SIU.params.ParentCtrl != null) {
                UnloadActionPan(Admin_HL7SIU.params.ParentCtrl, 'Admin_HL7SIU');
            }

            else
                UnloadActionPan(null, 'Admin_HL7SIU');
        }
        else {
            RemoveAdminTab();
        }
    },
}