
Admin_HL7DFT = {
    bIsFirstLoad: true,
    params: [],
    Total: null,
    Rejected: null,
    Accepted: null,

    Load: function (params) {
        Admin_HL7DFT.params = params;

        if (Admin_HL7DFT.params["FromAdmin"] == "0" && Admin_HL7DFT.params["PanelID"] == 'pnlAdminHL7DFT')
            Admin_HL7DFT.params["FromAdmin"] = "1";

        if (Admin_HL7DFT.bIsFirstLoad) {
            Admin_HL7DFT.bIsFirstLoad = false;

            var self = "";
            if (Admin_HL7DFT.params["PanelID"] != 'pnlAdminHL7DFT')
                self = $('#' + Admin_HL7DFT.params["PanelID"] + ' #pnlAdminHL7DFT');
            else
                self = $('#pnlAdminHL7DFT');

            self.loadDropDowns(true).done(function () {

                utility.CreateDatePicker('pnlAdminHL7DFT #CreatedOn', function () {
                    //on-change callback method 
                }, true);

                Admin_HL7DFT.HL7DFTSearch();
            });
        }
    },

    HL7DFTSearch: function (MirthLogID, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("DFT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($("#" + Admin_HL7DFT.params["PanelID"] + " #pnlHL7DFT_Result").css("display") == "none") {
                $("#" + Admin_HL7DFT.params["PanelID"] + " #pnlHL7DFT_Result").show();
            }

            var self = $("#" + Admin_HL7DFT.params["PanelID"] + " #pnlAdminHL7DFT_Search");
            var myJSON = self.getMyJSON();

            Admin_HL7DFT.SearchHL7DFT(myJSON, MirthLogID, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_HL7DFT.HL7DFTGridLoad(response);

                    $("#pnlAdminHL7DFT #total").text(Total);
                    $("#pnlAdminHL7DFT #accepted").text(Accepted);
                    $("#pnlAdminHL7DFT #rejected").text(Rejected);

                    var TableControl = Admin_HL7DFT.params["PanelID"] + " #dgvHL7DFT";
                    var PagingPanelControlID = Admin_HL7DFT.params["PanelID"] + " #divHL7DFTPaging";
                    var ClassControlName = "Admin_HL7DFT";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.HL7DFTCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_HL7DFT.HL7DFTSearch(PrimaryID, PageNumber, ResultPerPage);
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

    HL7DFTGridLoad: function (response) {
        $("#" + Admin_HL7DFT.params["PanelID"] + " #dgvHL7DFT").dataTable().fnDestroy();
        $("#" + Admin_HL7DFT.params["PanelID"] + " #pnlHL7DFT_Result #dgvHL7DFT tbody").find("tr").remove();
        if (response.HL7DFTCount > 0) {
            var HL7DFTLoad_JSON = JSON.parse(response.HL7DFTLoad_JSON);
            $.each(HL7DFTLoad_JSON, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvHL7DFT_row" + item.MirthLogID);
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

                var selectHL7DFT = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                //if (Admin_HL7DFT.params["FromAdmin"] == "0") {
                //    var selectMethod = "Admin_HL7DFT.FillHL7DFTName('" + item.MirthLogID + "','" + item.ShortName + "','" + item.PracticeId + "','" + item.PracticeName + "','" + item.EntityName + "',event);"
                //    selectHL7DFT = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                //    $row.attr("onclick", selectMethod);
                //}
                //else {
                //    $row.attr("onclick", "Admin_HL7DFT.HL7DFTEdit('" + item.MirthLogID + "',event);");
                //}
                if (Admin_HL7DFT.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    //$row.append('<td style="display:none;">' + item.MirthLogID + '</td><td>' + selectHL7DFT + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a href="#" onclick="Admin_HL7DFT.ShowHL7File(' + item.MirthLogID + ', \'' + item.FileText + '\');" title="View File"><i>' + item.FileName + '</i></a></td><td>' + item.MessageStatus + '</td><td>' + item.MRNumber + '</td><td>' + item.PatientName + '</td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.FinTranQuantity + '</td><td>' + item.ICDCode1 + '</td><td>' + item.ICDCodeDesc1 + '</td><td>' + item.ICDCode2 + '</td><td>' + item.ICDCodeDesc2 + '</td><td>' + item.ICDCode3 + '</td><td>' + item.ICDCodeDesc3 + '</td><td>' + item.ICDCode4 + '</td><td>' + item.ICDCodeDesc4 + '</td><td>' + item.FinTranProcedureCode + '</td><td>' + item.FinTranStartDate + '</td><td>' + item.FinTranEndDate + '</td><td>' + item.FinTranCoPay + '</td><td>' + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ErrorMessage + '</td>');
                } else {
                    $('#btn-add').show();
                    //$row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_HL7DFT.HL7DFTDelete(' + item.MirthLogID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_HL7DFT.HL7DFTEdit(' + item.MirthLogID + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_HL7DFT.HL7DFTActiveInactive(' + item.MirthLogID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectHL7DFT + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    var Start, End;
                    if (item.FinTranStartDate != "") {
                        Start = item.FinTranStartDate.substring(0, 8) + '-' + item.FinTranStartDate.substring(8, 10) + ':' + item.FinTranStartDate.substring(10, 12) + ':' + item.FinTranStartDate.substring(12, 14);
                    }
                    else { Start = ""; }
                    if (item.FinTranEndDate != "") {
                        End = item.FinTranEndDate.substring(0, 8) + '-' + item.FinTranEndDate.substring(8, 10) + ':' + item.FinTranEndDate.substring(10, 12) + ':' + item.FinTranEndDate.substring(12, 14);
                    } else { End = ""; }


                    $row.append('<td style="display:none;">' + item.MirthLogID + '</td><td><a href="#" onclick="Admin_HL7DFT.ShowHL7File(' + item.MirthLogID + ', \'' + item.FileText + '\');" title="View File"><i>' + item.FileName + '</i></a></td><td>' + item.MessageStatus + '</td><td>' + item.MRNumber + '</td><td>' + item.PatientName + '</td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.FinTranQuantity + '</td><td>' + item.ICDCode1 + '</td><td>' + item.ICDCodeDesc1 + '</td><td>' + item.ICDCode2 + '</td><td>' + item.ICDCodeDesc2 + '</td><td>' + item.ICDCode3 + '</td><td>' + item.ICDCodeDesc3 + '</td><td>' + item.ICDCode4 + '</td><td>' + item.ICDCodeDesc4 + '</td><td>' + item.FinTranProcedureCode + '</td><td>' + Start + '</td><td>' + End + '</td><td>' + item.FinTranCoPay + '</td><td>' + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ErrorMessage + '</td>');
                }
                $("#" + Admin_HL7DFT.params["PanelID"] + " #pnlHL7DFT_Result #dgvHL7DFT tbody").last().append($row);

                Total = item.RecordCount;
                Accepted = item.Accepted;
                Rejected = item.Rejected;
            });
        }
        else {
            $('#' + Admin_HL7DFT.params["PanelID"] + ' #dgvHL7DFT').DataTable({
                "language": {
                    "emptyTable": "No HL7DFT Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            Total = 0;
            Accepted = 0;
            Rejected = 0;

        }
        if ($.fn.dataTable.isDataTable('#' + Admin_HL7DFT.params["PanelID"] + ' #dgvHL7DFT'))
            ;
        else
            $("#" + Admin_HL7DFT.params["PanelID"] + " #pnlHL7DFT_Result #dgvHL7DFT").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_HL7DFT.params["PanelID"] + " #pnlHL7DFT_Result #dgvHL7DFT").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ShowHL7File: function (MirthLogID, FileText) {

        var strMessage = "";
        utility.SelectGridRow($('#gvHL7DFT_row' + MirthLogID));
        AppPrivileges.GetFormPrivileges("EDI Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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
    });
    },

    FillHL7DFTName: function (MirthLogID, HL7DFTName, PracticeId, PracticeName, EntityName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtHL7DFT";
        var RefHiddenIdCtrl = " #hfHL7DFT";
        if (Admin_HL7DFT.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_HL7DFT.params["RefCtrl"];
        }
        if (Admin_HL7DFT.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_HL7DFT.params["RefHiddenIdCtrl"];
        }

        if (Admin_Provider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_HL7DFT.params["PanelID"] + RefCtrl).val(HL7DFTName + ' - ' + EntityName).focus();
            else
                $('#' + Admin_HL7DFT.params["PanelID"] + RefCtrl).val(HL7DFTName).focus();
        }
        else {
            $('#' + Admin_HL7DFT.params["PanelID"] + RefCtrl).val(HL7DFTName).focus();
        }
        //$('#' + Admin_HL7DFT.params["PanelID"] + RefCtrl).val(HL7DFTName).focus();
        $('#' + Admin_HL7DFT.params["PanelID"] + RefHiddenIdCtrl).val(MirthLogID);


        if (Admin_HL7DFT.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_HL7DFT.params["MirthLogID"] = MirthLogID;
            Admin_HL7DFT.params["PracticeId"] = PracticeId;
            $('#' + Admin_HL7DFT.params["PanelID"] + ' #lblHL7DFT').css("display", "inline");
        } else {
            if (Admin_HL7DFT.params["IsOptional"] != null && Admin_HL7DFT.params["RefForm"] != null && Admin_HL7DFT.params["IsOptional"] == false) {
                if ($('#' + Admin_HL7DFT.params["PanelID"] + ' #' + Admin_HL7DFT.params["RefForm"]).data('bootstrapValidator') != null && typeof $('#' + Admin_HL7DFT.params["PanelID"] + ' #' + Admin_HL7DFT.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
                    $('#' + Admin_HL7DFT.params["PanelID"] + ' #' + Admin_HL7DFT.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_HL7DFT.params.PanelID + RefCtrl).attr("name"));
                }
            }
            $('#' + Admin_HL7DFT.params.PanelID + ' #txtPractice').val(PracticeName);
            $('#' + Admin_HL7DFT.params.PanelID + ' #hfPractice').val(PracticeId);
            $('#' + Admin_HL7DFT.params["PanelID"] + ' #lblHL7DFT').css("display", "none");
            $('#' + Admin_HL7DFT.params["PanelID"] + ' #lnkHL7DFTEdit').css("display", "inline");
        }

        if (Admin_HL7DFT.params != null && Admin_HL7DFT.params.ParentCtrl != null && Admin_HL7DFT.params.PanelID != 'pnlAdminHL7DFT') {
            UnloadActionPan(Admin_HL7DFT.params.ParentCtrl, 'Admin_HL7DFT', null, Admin_HL7DFT.params.PanelID);
        }
        else
            UnloadActionPan(Admin_HL7DFT.params["ParentCtrl"], "Admin_HL7DFT");

        $('#' + Admin_HL7DFT.params["PanelID"] + RefCtrl).focus();
    },

    SearchHL7DFT: function (HL7DFTData, MirthLogID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "HL7DFTData=" + HL7DFTData + "&MirthLogID=" + MirthLogID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HL7", "SEARCH_HL7_DFT");
    },



    UnLoadTab: function (Tab) {
        if (Admin_HL7DFT.params["FromAdmin"] == "0") {


            if (Admin_HL7DFT.params != null && Admin_HL7DFT.params.ParentCtrl != null && Admin_HL7DFT.params.PanelID != 'pnlAdminHL7DFT') {
                UnloadActionPan(Admin_HL7DFT.params.ParentCtrl, 'Admin_HL7DFT', null, Admin_HL7DFT.params.PanelID);
            }

            else if (Admin_HL7DFT.params != null && Admin_HL7DFT.params.ParentCtrl != null) {
                UnloadActionPan(Admin_HL7DFT.params.ParentCtrl, 'Admin_HL7DFT');
            }

            else
                UnloadActionPan(null, 'Admin_HL7DFT');
        }
        else {
            RemoveAdminTab();
        }
    },
}