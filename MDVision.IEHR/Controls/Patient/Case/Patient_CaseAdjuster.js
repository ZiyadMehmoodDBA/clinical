Patient_CaseAdjuster = {
    
    bIsFirstLoad: true,
    params: [],
    Load: function (paramerters) {
       
        Patient_CaseAdjuster.params = paramerters;
        if (Patient_CaseAdjuster.params.PanelID != null && Patient_CaseAdjuster.params.PanelID != 'pnlCaseAdjuster') {
            Patient_CaseAdjuster.params["PanelID"] = Patient_Case.params["PanelID"] + ' #pnlCaseAdjuster';
        }
        else {
            Patient_CaseAdjuster.params["PanelID"] = 'pnlCaseAdjuster';
        }
        if (Patient_CaseAdjuster.bIsFirstLoad) {
            Patient_CaseAdjuster.bIsFirstLoad = false;
            var self = $('#' + Patient_CaseAdjuster.params["PanelID"]);
            self.loadDropDowns(true);
        }
        Patient_CaseAdjuster.CaseAdjusterSearch();
        //utility.CreateDatePicker('pnlPatientCase #dtpCalledDate,#pnlPatientCase #dtpEntryDate', function () {
        //    //  on-change callback method
        //}, true);
    },
    CaseAdjusterAdd:function(){
        var strMessage = "";
        var params = [];
        params["mode"] = "Add";
        params["ParentCtrl"] = "Patient_CaseAdjuster";
        if (Patient_CaseAdjuster.params["ParentCtrl"] && Patient_CaseAdjuster.params["ParentCtrl"].indexOf('Referrals') >= 0)
            params["IsFromReferrals"] = true;
        else
            params["IsFromReferrals"] = false;

        LoadActionPan('CaseAdjusterDetail', params);
       
    },

    CaseAdjusterSearch: function (CaseAdjusterId, PageNo, rpp) {
        var strMessage = "";
        if (!$("#pnlCaseAdjuster #pnlCaseAdjuster_Result").is(":visible")) {
            $("#pnlCaseAdjuster #pnlCaseAdjuster_Result").show();
        }
        var self = $("#pnlCaseAdjuster_Search");
        var myJSON = self.getMyJSON();
        Patient_CaseAdjuster.SearchCaseAdjuster(myJSON, CaseAdjusterId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                Patient_CaseAdjuster.CaseAdjusterGridLoad(response);    //this will append table data in table body and create datatables instance
                var TableControl = "pnlCaseAdjuster #dgvCaseAdjuster"; //Table ID
                var PagingPanelControlID = "pnlCaseAdjuster #divCaseAdjusterPaging"; //Table Pagination ID
                var ClassControlName = "Patient_CaseAdjuster";  //Javascipt Class Name for this form
                var PagesToDisplay = 5; //Number of pages you need to display
                var iTotalDisplayRecords = response.iTotalDisplayRecords; //Total number of records to display (Count)
                //Setting Time out so that datatables instance is fully created.
                setTimeout(
                    CreatePagination(response.CaseAdjusterCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                    //Anonymous  function is for Pagination Call Backs
                    function (PrimaryID, PageNumber, ResultPerPage) {
                        Patient_CaseAdjuster.CaseAdjusterSearch(PrimaryID, PageNumber, ResultPerPage);
                    }),
                10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    CaseAdjusterGridLoad: function (response) {
        $("#dgvCaseAdjuster").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlCaseAdjuster_Result #dgvCaseAdjuster tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.CaseAdjusterCount > 0) {
            var CaseAdjusterLoad_JSON = JSON.parse(response.CaseAdjusterLoad_JSON); //Parsing array to JSON
            $.each(CaseAdjusterLoad_JSON, function (i, item) { 
                var $row = $('<tr/>');
                $row.attr("onclick", "Patient_CaseAdjuster.CaseAdjusterEdit(" + item.CaseAdjusterId + ",event);");
                $row.attr("id", "gv_CaseAdjuster_row" + item.CaseAdjusterId);
                $row.attr("CaseAdjusterId", item.CaseAdjusterId);
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
                var selectCaseAdjuster = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                var selectMethod = "Patient_CaseAdjuster.FillCaseAdjusterInfo('" + item.CaseAdjusterId +"',event);"
                selectCaseAdjuster = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                $row.attr("onclick", "Patient_CaseAdjuster.CaseAdjusterEdit('" + item.CaseAdjusterId + "',event);");
                $row.append('<td style="display:none;">' + item.CaseAdjusterId + '</td><td><a class="btn btn-xs" href="#" onclick="Patient_CaseAdjuster.CaseAdjusterDelete(\'' + item.CaseAdjusterId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_CaseAdjuster.CaseAdjusterEdit(\'' + item.CaseAdjusterId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_CaseAdjuster.CaseAdjusterActiveInactive(\'' + item.CaseAdjusterId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectCaseAdjuster + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.Address1 + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.Zip + '</td><td>' + item.Extention + '</td><td>' + item.Phone + '</td>');
                $("#pnlCaseAdjuster_Result #dgvCaseAdjuster tbody").last().append($row);
            });
        }
        else {
            $('#pnlCaseAdjuster_Result #dgvCaseAdjuster').DataTable({
                "language": {
                    "emptyTable": "No Case Adjuster Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlCaseAdjuster_Result #dgvCaseAdjuster'))
            ;
        else {
            $("#pnlCaseAdjuster_Result #dgvCaseAdjuster").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
    },

    SearchCaseAdjuster: function (CaseAdjusterData, CaseAdjusterId, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "CaseAdjusterData=" + CaseAdjusterData + "&CaseAdjusterId=" + CaseAdjusterId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // search parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE_ADJUSTER", "SEARCH_CASE");
    },
    FillCaseAdjusterInfo: function (CaseAdjusterId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var PageNo = null, rpp = null;
        var self = $("#pnlCaseAdjuster_Search");
        var myJSON = self.getMyJSON();
        Patient_CaseAdjuster.SearchCaseAdjuster(myJSON, CaseAdjusterId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                if (response.CaseAdjusterCount > 0) {
                    var CaseAdjusterLoad_JSON = JSON.parse(response.CaseAdjusterLoad_JSON);
                    
                    $("#" + Patient_CaseAdjuster.params["ParentPanelId"] + " #" + Patient_CaseAdjuster.params["ParentFormId"] + " #WCDetails" + " #txtCaseAdjuster").val(CaseAdjusterLoad_JSON[0].LastName + ', ' + CaseAdjusterLoad_JSON[0].FirstName);
                    $("#" + Patient_CaseAdjuster.params["ParentPanelId"] + " #" + Patient_CaseAdjuster.params["ParentFormId"] + " #hfCaseAdjusterId").val(CaseAdjusterLoad_JSON[0].CaseAdjusterId);
                    $("#" + Patient_CaseAdjuster.params["ParentPanelId"] + " #" + Patient_CaseAdjuster.params["ParentFormId"] + " #WCDetails" + " #txtTelephone").val(CaseAdjusterLoad_JSON[0].Phone);
                    $("#" + Patient_CaseAdjuster.params["ParentPanelId"] + " #" + Patient_CaseAdjuster.params["ParentFormId"] + " #WCDetails" + " #txtFax").val(CaseAdjusterLoad_JSON[0].Fax);
                    $("#" + Patient_CaseAdjuster.params["ParentPanelId"] + " #" + Patient_CaseAdjuster.params["ParentFormId"] + " #WCDetails" + " #txtEmail").val(CaseAdjusterLoad_JSON[0].Email);
                    if (Patient_CaseAdjuster.params["IsFromParentCtrl"] == "0")
                    { Patient_CaseAdjuster.UnLoadTab(); }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    CaseAdjusterEdit: function (CaseAdjusterId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gv_CaseAdjuster_row' + CaseAdjusterId));
      
        var selectedValue = CaseAdjusterId;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            var params = [];
            params["CaseAdjusterId"] = selectedValue;
            params["mode"] = "Edit";
            params["ParentCtrl"] = "Patient_CaseAdjuster";

            if (Patient_CaseAdjuster.params["ParentCtrl"] && Patient_CaseAdjuster.params["ParentCtrl"].indexOf('Referrals') >= 0)
                params["IsFromReferrals"] = true;
            else
                params["IsFromReferrals"] = false;
            LoadActionPan('CaseAdjusterDetail', params);
        }

    },
   
    CaseAdjusterDelete: function (CaseAdjusterId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gv_CaseAdjuster_row' + CaseAdjusterId));
      
        utility.myConfirm('1', function () {
            var selectedValue = CaseAdjusterId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_CaseAdjuster.DeleteCaseAdjuster(selectedValue).done(function (response) {
                    if (response.status != false) {
                        var table1 = $('#dgvCaseAdjuster').DataTable();
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
    },
    DeleteCaseAdjuster: function (CaseAdjusterId) {
        var data = "CaseAdjusterId=" + CaseAdjusterId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE_ADJUSTER", "DELETE_CASE");
    },
    CaseAdjusterActiveInactive: function (CaseAdjusterId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            var selectedValue = CaseAdjusterId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_CaseAdjuster.UpdateCaseAdjusterActiveInactive(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Patient_CaseAdjuster.CaseAdjusterSearch(selectedValue);
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
    UpdateCaseAdjusterActiveInactive: function (CaseAdjusterId, IsActive) {
        var data = "CaseAdjusterId=" + CaseAdjusterId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE_ADJUSTER", "UPDATE_CASE_ACTIVE_INACTIVE");
    },

  
    UnLoadTab: function () {
        UnloadActionPan(Patient_CaseAdjuster.params.ParentCtrl, 'Patient_CaseAdjuster');
    },
  
}