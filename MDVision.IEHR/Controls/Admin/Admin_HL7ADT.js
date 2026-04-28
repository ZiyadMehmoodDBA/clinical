
Admin_HL7ADT = {
    bIsFirstLoad: true,
    params: [],
    Total: null,
    Rejected: null,
    Accepted: null,
    Errored: null,

    Load: function (params) {
        Admin_HL7ADT.params = params;

        if (Admin_HL7ADT.params["FromAdmin"] == "0" && Admin_HL7ADT.params["PanelID"] == 'pnlAdminHL7ADT')
            Admin_HL7ADT.params["FromAdmin"] = "1";

        if (Admin_HL7ADT.bIsFirstLoad) {
            Admin_HL7ADT.bIsFirstLoad = false;

            var self = "";
            if (Admin_HL7ADT.params["PanelID"] != 'pnlAdminHL7ADT')
                self = $('#' + Admin_HL7ADT.params["PanelID"] + ' #pnlAdminHL7ADT');
            else
                self = $('#pnlAdminHL7ADT');

            self.loadDropDowns(true).done(function () {

                utility.CreateDatePicker('pnlAdminHL7ADT #CreatedOn', function () {
                    //on-change callback method 
                }, true);

                Admin_HL7ADT.HL7ADTSearch();
            });
        }
    },
    DataFixesPDF: function () {
        Admin_HL7ADT.DataFixes_DbCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status == true)
                utility.DisplayMessages(response.Message, 1);
            else
                utility.DisplayMessages(response.Message, 2);
        });
    },
    DataFixes_DbCall: function () {
        var objNotesModel = new Object();
        return MDVisionService.APIServiceComplex(null, "ClinicalNotes", "CreateNotesPDFFixesFilesV2");
    },
    HL7ADTAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ADT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var params = [];
            params["MirthLogID"] = null;
            params["mode"] = "Add";
            params["FromAdmin"] = Admin_HL7ADT.params["FromAdmin"];
            if (Admin_HL7ADT.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'Admin_HL7ADT';
            }
            LoadActionPan('HL7ADTDetail', params);
        }
        else
            utility.DisplayMessages(strMessage, 2);
        });
    },

    HL7ADTEdit: function (MirthLogID) {
        var strMessage = "";
        utility.SelectGridRow($('#gvHL7ADT_row' + MirthLogID));
        AppPrivileges.GetFormPrivileges("ADT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var selectedValue = MirthLogID;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var params = [];
                params["MirthLogID"] = selectedValue;
                params["mode"] = "Edit";
                params["FromAdmin"] = Admin_HL7ADT.params["FromAdmin"];
                if (Admin_HL7ADT.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_HL7ADT';
                }
                LoadActionPan('HL7ADTDetail', params);
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
        });
    },

    HL7ADTDelete: function (MirthLogID) {
        var strMessage = "";
        utility.SelectGridRow($('#gvHL7ADT_row' + MirthLogID));
        utility.SelectGridRow($("#" + Admin_HL7ADT.params["PanelID"] + ' #gvHL7ADT_row' + MirthLogID));
        AppPrivileges.GetFormPrivileges("ADT", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = MirthLogID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_HL7ADT.DeleteHL7ADT(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#' + Admin_HL7ADT.params["PanelID"] + ' #dgvHL7ADT').DataTable();
                            table1.row('.active').remove().draw(false);
                            utility.DisplayMessages(response.Message, 1);
                            //CacheManager.BindCodes('GetHL7ADT', true);
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

    HL7ADTActiveInactive: function (MirthLogID, IsActive, event) {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("ADT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = MirthLogID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    HL7ADTDetail.UpdateHL7ADTActiveInactive(selectedValue, IsActive).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_HL7ADT.HL7ADTSearch('0');
                            //CacheManager.BindCodes('GetHL7ADT', true);
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

    HL7ADTSearch: function (MirthLogID, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ADT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($("#" + Admin_HL7ADT.params["PanelID"] + " #pnlHL7ADT_Result").css("display") == "none") {
                $("#" + Admin_HL7ADT.params["PanelID"] + " #pnlHL7ADT_Result").show();
            }

            var self = $("#" + Admin_HL7ADT.params["PanelID"] + " #pnlAdminHL7ADT_Search");
            var myJSON = self.getMyJSON();

            Admin_HL7ADT.SearchHL7ADT(myJSON, MirthLogID, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_HL7ADT.HL7ADTGridLoad(response);
                    $("#pnlAdminHL7ADT #total").text(Total);
                    $("#pnlAdminHL7ADT #accepted").text(Accepted);
                    $("#pnlAdminHL7ADT #rejected").text(Rejected);
                    $("#pnlAdminHL7ADT #errored").text(Errored);

                    var TableControl = Admin_HL7ADT.params["PanelID"] + " #dgvHL7ADT";
                    var PagingPanelControlID = Admin_HL7ADT.params["PanelID"] + " #divHL7ADTPaging";
                    var ClassControlName = "Admin_HL7ADT";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.HL7ADTCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_HL7ADT.HL7ADTSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
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

    HL7ADTGridLoad: function (response) {
        $("#" + Admin_HL7ADT.params["PanelID"] + " #dgvHL7ADT").dataTable().fnDestroy();
        $("#" + Admin_HL7ADT.params["PanelID"] + " #pnlHL7ADT_Result #dgvHL7ADT tbody").find("tr").remove();
        if (response.HL7ADTCount > 0) {
            var HL7ADTLoad_JSON = JSON.parse(response.HL7ADTLoad_JSON);
            $.each(HL7ADTLoad_JSON, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvHL7ADT_row" + item.MirthLogID);
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

                var selectHL7ADT = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                //if (Admin_HL7ADT.params["FromAdmin"] == "0") {
                //    var selectMethod = "Admin_HL7ADT.FillHL7ADTName('" + item.MirthLogID + "','" + item.ShortName + "','" + item.PracticeId + "','" + item.PracticeName + "','" + item.EntityName + "',event);"
                //    selectHL7ADT = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                //    $row.attr("onclick", selectMethod);
                //}
                //else {
                //    $row.attr("onclick", "Admin_HL7ADT.HL7ADTEdit('" + item.MirthLogID + "',event);");
                //}
                if (Admin_HL7ADT.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    //$row.append('<td style="display:none;">' + item.MirthLogID + '</td><td>' + selectHL7ADT + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a href="#" onclick="Admin_HL7ADT.ShowHL7File(' + item.MirthLogID + ', \'' + item.FileText + '\');" title="View File"><i>' + item.FileName + '</i></a></td><td>' + item.MessageStatus + '</td><td>' + item.MRNumber + '</td><td>' + item.Facility + '</td><td>' + item.PatientName + '</td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.RefProvider + '</td><td>' + item.PCProvider + '</td><td>' + item.Guarantor + '</td><td>' + item.InsCompanyName + '</td><td>' + item.InsSubscriberNumber + '</td><td>' + item.Insured + '</td><td>' + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ErrorMessage + '</td>');

                } else {
                    $('#btn-add').show();
                    //$row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_HL7ADT.HL7ADTDelete(' + item.MirthLogID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_HL7ADT.HL7ADTEdit(' + item.MirthLogID + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_HL7ADT.HL7ADTActiveInactive(' + item.MirthLogID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectHL7ADT + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a href="#" onclick="Admin_HL7ADT.ShowHL7File(' + item.MirthLogID + ', \'' + item.FileText + '\');" title="View File"><i>' + item.FileName + '</i></a></td><td>' + item.MessageStatus + '</td><td>' + item.MRNumber + '</td><td>' + item.PatientName + '</td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.RefProvider + '</td><td>' + item.PCProvider + '</td><td>' + item.Guarantor + '</td><td>' + item.InsCompanyName + '</td><td>' + item.InsSubscriberNumber + '</td><td>' + item.Insured + '</td><td>' + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')+ '</td><td>' + item.ErrorMessage + '</td>');
                }
                $("#" + Admin_HL7ADT.params["PanelID"] + " #pnlHL7ADT_Result #dgvHL7ADT tbody").last().append($row);

                Total = item.RecordCount;
                Accepted = item.Accepted;
                Rejected = item.Rejected;
                Errored = item.Errored;
            });
        }
        else {
            $('#' + Admin_HL7ADT.params["PanelID"] + ' #dgvHL7ADT').DataTable({
                "language": {
                    "emptyTable": "No HL7ADT Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

            Total = 0;
            Accepted = 0;
            Rejected = 0;
            Errored = 0;
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_HL7ADT.params["PanelID"] + ' #dgvHL7ADT'))
            ;
        else
            $("#" + Admin_HL7ADT.params["PanelID"] + " #pnlHL7ADT_Result #dgvHL7ADT").DataTable({ "bInfo": false, "bPaginate": false, "bSort": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_HL7ADT.params["PanelID"] + " #pnlHL7ADT_Result #dgvHL7ADT").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ShowHL7File: function (MirthLogID, FileText) {

        var strMessage = "";
        utility.SelectGridRow($('#gvHL7ADT_row' + MirthLogID));
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

    FillHL7ADTName: function (MirthLogID, HL7ADTName, PracticeId, PracticeName, EntityName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtHL7ADT";
        var RefHiddenIdCtrl = " #hfHL7ADT";
        if (Admin_HL7ADT.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_HL7ADT.params["RefCtrl"];
        }
        if (Admin_HL7ADT.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_HL7ADT.params["RefHiddenIdCtrl"];
        }

        if (Admin_Provider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_HL7ADT.params["PanelID"] + RefCtrl).val(HL7ADTName + ' - ' + EntityName).focus();
            else
                $('#' + Admin_HL7ADT.params["PanelID"] + RefCtrl).val(HL7ADTName).focus();
        }
        else {
            $('#' + Admin_HL7ADT.params["PanelID"] + RefCtrl).val(HL7ADTName).focus();
        }
        //$('#' + Admin_HL7ADT.params["PanelID"] + RefCtrl).val(HL7ADTName).focus();
        $('#' + Admin_HL7ADT.params["PanelID"] + RefHiddenIdCtrl).val(MirthLogID);


        if (Admin_HL7ADT.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_HL7ADT.params["MirthLogID"] = MirthLogID;
            Admin_HL7ADT.params["PracticeId"] = PracticeId;
            $('#' + Admin_HL7ADT.params["PanelID"] + ' #lblHL7ADT').css("display", "inline");
        } else {
            if (Admin_HL7ADT.params["IsOptional"] != null && Admin_HL7ADT.params["RefForm"] != null && Admin_HL7ADT.params["IsOptional"] == false) {
                if ($('#' + Admin_HL7ADT.params["PanelID"] + ' #' + Admin_HL7ADT.params["RefForm"]).data('bootstrapValidator') != null && typeof $('#' + Admin_HL7ADT.params["PanelID"] + ' #' + Admin_HL7ADT.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
                    $('#' + Admin_HL7ADT.params["PanelID"] + ' #' + Admin_HL7ADT.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_HL7ADT.params.PanelID + RefCtrl).attr("name"));
                }
            }
            $('#' + Admin_HL7ADT.params.PanelID + ' #txtPractice').val(PracticeName);
            $('#' + Admin_HL7ADT.params.PanelID + ' #hfPractice').val(PracticeId);
            $('#' + Admin_HL7ADT.params["PanelID"] + ' #lblHL7ADT').css("display", "none");
            $('#' + Admin_HL7ADT.params["PanelID"] + ' #lnkHL7ADTEdit').css("display", "inline");
        }

        if (Admin_HL7ADT.params != null && Admin_HL7ADT.params.ParentCtrl != null && Admin_HL7ADT.params.PanelID != 'pnlAdminHL7ADT') {
            UnloadActionPan(Admin_HL7ADT.params.ParentCtrl, 'Admin_HL7ADT', null, Admin_HL7ADT.params.PanelID);
        }
        else
            UnloadActionPan(Admin_HL7ADT.params["ParentCtrl"], "Admin_HL7ADT");

        $('#' + Admin_HL7ADT.params["PanelID"] + RefCtrl).focus();
    },

    SearchHL7ADT: function (HL7ADTData, MirthLogID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "HL7ADTData=" + HL7ADTData + "&MirthLogID=" + MirthLogID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HL7", "SEARCH_HL7_ADT");
    },

    DeleteHL7ADT: function (MirthLogID) {
        var data = "MirthLogID=" + MirthLogID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HL7", "DELETE_EDI_SERVICE_HANDLE");
    },

    UnLoadTab: function (Tab) {
        if (Admin_HL7ADT.params["FromAdmin"] == "0") {


            if (Admin_HL7ADT.params != null && Admin_HL7ADT.params.ParentCtrl != null && Admin_HL7ADT.params.PanelID != 'pnlAdminHL7ADT') {
                UnloadActionPan(Admin_HL7ADT.params.ParentCtrl, 'Admin_HL7ADT', null, Admin_HL7ADT.params.PanelID);
            }

            else if (Admin_HL7ADT.params != null && Admin_HL7ADT.params.ParentCtrl != null) {
                UnloadActionPan(Admin_HL7ADT.params.ParentCtrl, 'Admin_HL7ADT');
            }

            else
                UnloadActionPan(null, 'Admin_HL7ADT');
        }
        else {
            RemoveAdminTab();
        }
    },
}