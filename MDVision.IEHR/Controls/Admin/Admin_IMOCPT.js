
Admin_IMOCPT = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        Admin_IMOCPT.params = params;

        if (Admin_IMOCPT.params["FromAdmin"] == "0" && Admin_IMOCPT.params["PanelID"] == 'pnlAdminIMOCPT')
            Admin_IMOCPT.params["FromAdmin"] = "1";

        if (Admin_IMOCPT.bIsFirstLoad) {

            Admin_IMOCPT.bIsFirstLoad = false;

            var self = "";
            if (Admin_IMOCPT.params["PanelID"] != "pnlAdminIMOCPT") {
                self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlAdminIMOCPT")
            }
            else
                self = $('#' + Admin_IMOCPT.params["PanelID"]);

            if (Admin_IMOCPT.params["ParentCtrl"] == "mstrTabReports") {
                self.find('#btn_CPTAdd').hide();

            } else {
                self.find('#btn_CPTAdd').show();
            }
            self.loadDropDowns(true).done(function () {
            });
        }

        $(function () {
            $('#txtCPTAndDescription').keypress(function (e) {
                var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keycode == 13) {
                    Admin_IMOCPT.CPTSearch();
                }
            });
        });
    },

    ValidateCPT: function () {
        $('#frmCPTIMO')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   CPTAndDescription: {
                       group: '.col-sm-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Admin_IMOCPT.CPTSearch();
        });
    },

    CPTSearch: function (CPTId, PageNo, rpp) {
        var CPT = $("#txtCPTAndDescription").val();
        var strMessage = "";
        var IsSCTSearch = false;
        if (CPT.substring(0, 3).toLowerCase() == "sct") {
            IsSCTSearch = true;
            var lowerStr = CPT.toLowerCase();
            var splitArr = lowerStr.split("sct");
            CPT = splitArr[1];
        } else {
            IsSCTSearch = false;
        }
        AppPrivileges.GetFormPrivileges("CPT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Admin_IMOCPT.params["PanelID"] + ' #pnlIMOCPT_Result').css("display") == "none") {
                    $('#' + Admin_IMOCPT.params["PanelID"] + ' #pnlIMOCPT_Result').show();
                }
                var self = "";
                if (Admin_IMOCPT.params["PanelID"] != "pnlAdminIMOCPT") {
                    self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlAdminIMOCPT #pnlIMOCPT_Search")
                }
                else
                    self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlIMOCPT_Search");

                //  var self = $('#' + Admin_IMOCPT.params["PanelID"] + ' #pnlIMOCPT_Search');
                var myJSON = self.getMyJSON();

                if (IsSCTSearch) {
                    Admin_IMOCPT.Search_SCT_CPT(CPT).done(function (response) {
                        if (response.status != false) {
                            Admin_IMOCPT.CPTGridLoad(response);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else {
                    Admin_IMOCPT.SearchCPT(myJSON, CPT, PageNo, rpp).done(function (response) {
                        if (response.status != false) {
                            if (response.CPTCount > 0) {
                                $("#" + Admin_IMOCPT.params["PanelID"] + " #divIMOCPTPaging").css("display", "inline");
                                ////Showing 1 to 15 of 15 entries
                                //var RecordsPerPage = rpp != null ? rpp : 15;
                                //var CurrentPage = PageNo != null ? PageNo : 1;
                                //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                                //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                                //if (PageNo == null) {
                                //    utility.GetCustomPaging("divIMOCPTPaging", response.iTotalDisplayRecords, 5, "Admin_IMOCPT", CurrentPage, RecordsPerPage);
                                //}
                                //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                                //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                                //$("#" + Admin_IMOCPT.params["PanelID"] + " #divIMOCPTPaging #divShowingEntries").text(showingText);
                                //// Change Background Color to Black for selected page
                                //self.find("li").each(function () {
                                //    if ($(this).text() == CurrentPage) {
                                //        $(this).attr("class", "active");
                                //    }
                                //    else
                                //        $(this).removeAttr("class");
                                //});

                                //-------------------------------------------


                                var TableControl = Admin_IMOCPT.params["PanelID"] + " #divIMOCPTPaging";
                                var PagingPanelControlID = Admin_IMOCPT.params["PanelID"] + " #divIMOCPTPaging";
                                var ClassControlName = "Admin_IMOCPT";
                                var PagesToDisplay = 5;
                                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                                setTimeout(CreatePagination(response.CPTCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                    Admin_IMOCPT.CPTSearch(PrimaryID, PageNumber, ResultPerPage);
                                }), 10);

                                //-------------------------------------------

                            }
                            else {
                                $("#" + Admin_IMOCPT.params["PanelID"] + " #divIMOCPTPaging").css("display", "none");
                            }
                             //Start//17-08-2016//Ahmad Raza//PQRS CPT Search Changes
                            if (Admin_IMOCPT.params.ParentCtrl == 'PQRS_ICDCPTCodes') {
                                var CPTMatched = false;
                                $.each(PQRS_ICDCPTCodes.arrCPTs, function (i, item) {

                                    var CPTJSON = JSON.parse(response.CPTLoad_JSON)
                                    $.each(CPTJSON, function (j, val) {
                                        if (val.CPTCode == item.Code) {
                                            CPTMatched = true;
                                        }
                                    });

                                });
                                if (CPTMatched == false) {
                                    $("#" + Admin_IMOCPT.params["PanelID"] + " #divIMOCPTPaging").css("display", "none");
                                    return true;
                                }
                            }
			    //End//17-08-2016//Ahmad Raza//PQRS CPT Search Changes
                            Admin_IMOCPT.CPTGridLoad(response);

                        }
                        else {
                            // AST-330 did not show message but append no cpt found in grid.
                            $("#" + Admin_IMOCPT.params["PanelID"] + " #divIMOCPTPaging").css("display", "none");
                            Admin_IMOCPT.CPTGridLoad(response);
                            //utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }


                //else {
                //    utility.DisplayMessages(response.Message, 3);
                //}

                //});
            } else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    CPTGridLoad: function (response) {
        $('#' + Admin_IMOCPT.params["PanelID"] + ' #dgvIMOCPT').dataTable().fnDestroy();
        $('#' + Admin_IMOCPT.params["PanelID"] + ' #pnlIMOCPT_Result #dgvIMOCPT tbody').find("tr").remove();
        if (response.CPTCount > 0) {
            var CPTLoadJSONData = JSON.parse(response.CPTLoad_JSON);
            $.each(CPTLoadJSONData, function (i, item) {
                var selectCPT = "";
                //var _concatinatedString = item.CPTCode + "-" + item.Description + " (SCT: " + item.SNOMEDId + ")";
                var _concatinatedString = item.CPTCode + "-" + item.Description;
                var $row = $('<tr/>');
                var imo = "imo";
                var prtCtrl = "Admin_IMOCPT";
                var containerCtrl_ = Admin_IMOCPT.params.RefCtrl;
                var hiddenCtrl = Admin_IMOCPT.params.RefHiddenCtrl;
                var containerCtrlDescription = Admin_IMOCPT.params.RefCtrlDescription;
                var containerCtrl = "";
                if (containerCtrlDescription != null && containerCtrlDescription != "") {
                    containerCtrl = containerCtrl_ + '@' + containerCtrlDescription;
                }
                else {
                    containerCtrl = containerCtrl_;
                }
                var lexiCode = "~" + item.SNOMEDId + "+" + item.SNOMEDDescription + "^";
                var onclickmethod = '';
                if (Admin_IMOCPT.params.ParentCtrl == 'Clinical_CustomFormsPreview' || Admin_IMOCPT.params.ParentCtrl == 'Batch_FaxSend') {
                    onclickmethod = "cptcodeDetail.SetControlValues('" + _concatinatedString + "', '" + lexiCode + "', '" + prtCtrl + "', '" + containerCtrl + "', '" + hiddenCtrl + "', '" + Admin_IMOCPT.params.ParentCtrl + "', '" + Admin_IMOCPT.params.ContainerProcedureDivId + "');";
                }
                else {
                    onclickmethod = "cptcodeDetail.SetControlValues('" + _concatinatedString + "', '" + lexiCode + "', '" + prtCtrl + "', '" + containerCtrl + "', '" + hiddenCtrl + "', '" + Admin_IMOCPT.params.ParentCtrl + "');";
                }
                $row.attr("onclick", onclickmethod);

                $row.attr("CPTId", item.CPTId);

                $row.append('<td style="display:none;">' + item.CPTId + '</td><td>' + item.CPTCode + '</td><td>' + item.Description.replace("\''", "&apos") + '</td>');
                $('#' + Admin_IMOCPT.params["PanelID"] + ' #pnlIMOCPT_Result #dgvIMOCPT tbody').last().append($row);
            });
        }
        else {
            $('#dgvIMOCPT').DataTable({
                "language": {
                    "emptyTable": "No CPT Found"
                }
                , "bLengthChange": false, "autoWidth": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#dgvIMOCPT'))
            ;
        else
            $('#' + Admin_IMOCPT.params["PanelID"] + ' #pnlIMOCPT_Result #dgvIMOCPT').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchCPT: function (CPTData, CPT, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "CPTData=" + CPTData + "&CPT=" + CPT + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Admin_IMOCPT", "SEARCH_CPT");
    },

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        var self = "";
        if (Admin_IMOCPT.params["PanelID"] != "pnlAdminIMOCPT") {
            self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlAdminIMOCPT #pnlIMOCPT_Search")
        }
        else
            self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlIMOCPT_Search");

        $(self).find("li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Admin_IMOCPT.CPTSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        var self = "";
        if (Admin_IMOCPT.params["PanelID"] != "pnlAdminIMOCPT") {
            self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlAdminIMOCPT #pnlIMOCPT_Search")
        }
        else
            self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlIMOCPT_Search");

        $(self).find("li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_IMOCPT.CPTSearch(null, currentPageNo, 15);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        var self = "";
        if (Admin_IMOCPT.params["PanelID"] != "pnlAdminIMOCPT") {
            self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlAdminIMOCPT #pnlIMOCPT_Search")
        }
        else
            self = $('#' + Admin_IMOCPT.params["PanelID"] + " #pnlIMOCPT_Search");

        $(self).find("li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_IMOCPT.CPTSearch(null, currentPageNo, 15);
        }
    },

    UnLoadTab: function (Tab) {

        if (Admin_IMOCPT.params != null && Admin_IMOCPT.params.ParentCtrl != null) {

            UnloadActionPan(Admin_IMOCPT.params.ParentCtrl, 'Admin_IMOCPT');

            if (Admin_IMOCPT.params.ParentCtrlPanelID != null && Admin_IMOCPT.params.ParentCtrlPanelID.indexOf("pnlClinical") != -1) {
                UnloadActionPan(Admin_IMOCPT.params.ParentCtrl, 'Admin_IMOCPT', null, Admin_IMOCPT.params.ParentCtrlPanelID);
            }
            //Start//29/01/2016//Ahmad Raza//Revalidating Procedure field when Procedure selected from popup, for MedicalHx,HospitalizationHx and SurgicalHx
            if (Admin_IMOCPT.params.ParentCtrl == "Clinical_MedicalHx") {
                $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx").bootstrapValidator('revalidateField', 'CPTCode');
            }
            else if (Admin_IMOCPT.params.ParentCtrl == "Clinical_HospitalizationHx") {
                $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('revalidateField', 'CPT');
            }
            else if (Admin_IMOCPT.params.ParentCtrl == "Clinical_SurgicalHx") {
                $("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx").bootstrapValidator('revalidateField', 'CPTCode');
            }

            //End//29/01/2016//Ahmad Raza//Revalidating Procedure field when Procedure selected from popup, for MedicalHx,HospitalizationHx and SurgicalHx
        }
        else
            UnloadActionPan(null, 'Admin_IMOCPT');
    },

    //********************* SCT Implementation *******************\\

    Search_SCT_CPT: function (CPT) {
        var data = "SCTSnomed=" + CPT;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Admin_IMOCPT", "SEARCH_SCT_CPT");
    },


}
