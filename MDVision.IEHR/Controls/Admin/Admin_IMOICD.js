
Admin_IMOICD = {
    bIsFirstLoad: true,
    params: [],
    IsShowICD10:false,

    Load: function (params) {

        Admin_IMOICD.params = params;

        if (Admin_IMOICD.params["FromAdmin"] == "0" && Admin_IMOICD.params["PanelID"] == 'pnlAdminIMOICD')
            Admin_IMOICD.params["FromAdmin"] = "1";

        if (Admin_IMOICD.bIsFirstLoad) {

            Admin_IMOICD.bIsFirstLoad = false;

            var self = "";
            if (Admin_IMOICD.params["PanelID"] != "pnlAdminIMOICD") {
                self = $('#' + Admin_IMOICD.params["PanelID"] + " #pnlAdminIMOICD")
            }
            else
                self = $('#' + Admin_IMOICD.params["PanelID"]);

            if (Admin_IMOICD.params["ParentCtrl"] == "mstrTabReports") {
                self.find('#btn_ICDAdd').hide();

            } else {
                self.find('#btn_ICDAdd').show();
            }
            self.loadDropDowns(true).done(function () {
            });
        }

        $(function () {
            $('#txtICDAndDescription').keypress(function (e) {
                var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keycode == 13) {
                    Admin_IMOICD.ICDSearch();
                }
            });
        });
    },

    ValidateICD: function () {
        $('#frmICDIMO')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ICDAndDescription: {
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
            Admin_IMOICD.ICDSearch();
        });
    },

    ICDSearch: function (ICDId, PageNo, rpp) {
        var ICD = $("#txtICDAndDescription").val();
        // Faizan Ameen.
        Admin_IMOICD.GetISShowICD10();
           

       
        if (Admin_IMOICD.params["ParentCtrl"] == "Clinical_ProblemLists") {
            if (ICD.length > 3) {
                if (ICD.substring(0, 3).toLowerCase() == "sct") {
                    Clinical_ProblemLists.LastSctBaseSearch = ICD.substring(3, ICD.length);
                }
                else {
                    Clinical_ProblemLists.LastSctBaseSearch = "";
                }
            }
            else {
                Clinical_ProblemLists.LastSctBaseSearch = "";
            }
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ICD", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Result').css("display") == "none") {
                    $('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Result').show();
                }
                var self = "";
                if (Admin_IMOICD.params["PanelID"] != "pnlAdminIMOICD") {
                    self = $('#' + Admin_IMOICD.params["PanelID"] + " #pnlAdminIMOICD #pnlIMOICD_Search")
                }
                else
                    self = $('#' + Admin_IMOICD.params["PanelID"] + " #pnlIMOICD_Search");

                //  var self = $('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Search');
                var myJSON = self.getMyJSON();
               

                Admin_IMOICD.SearchICD(myJSON, ICD, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.ICDCount > 0) {
                            $("#" + Admin_IMOICD.params["PanelID"] + " #divIMOICDPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            //var RecordsPerPage = rpp != null ? rpp : 15;
                            //var CurrentPage = PageNo != null ? PageNo : 1;
                            //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            //    utility.GetCustomPaging("divIMOICDPaging", response.iTotalDisplayRecords, 5, "Admin_IMOICD", CurrentPage, RecordsPerPage);
                            //}
                            //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            //$("#" + Admin_IMOICD.params["PanelID"] + " #divIMOICDPaging #divShowingEntries").text(showingText);
                            //// Change Background Color to Black for selected page
                            //self.find("li").each(function () {
                            //    if ($(this).text() == CurrentPage) {
                            //        $(this).attr("class", "active");
                            //    }
                            //    else
                            //        $(this).removeAttr("class");
                            //});

                            //-------------------------------------------


                            var TableControl = Admin_IMOICD.params["PanelID"] + " #divIMOICDPaging";
                            var PagingPanelControlID = Admin_IMOICD.params["PanelID"] + " #divIMOICDPaging";
                            var ClassControlName = "Admin_IMOICD";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.ICDCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_IMOICD.ICDSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);

                            //-------------------------------------------


                        }
                        else {
                            $("#" + Admin_IMOICD.params["PanelID"] + " #divIMOICDPaging").css("display", "none");
                        }
                        //Start//17-08-2016//Ahmad Raza//PQRS ICD Search Changes
                        if (Admin_IMOICD.params.ParentCtrl == 'PQRS_ICDCPTCodes') {
                            var ICDMatched = false;
                            $.each(PQRS_ICDCPTCodes.arrICDs, function (i, item) {

                                var ICDJSON = JSON.parse(response.ICDLoad_JSON)
                                $.each(ICDJSON, function (j, val) {
                                    if (val.ICD10 == item.Code) {
                                        ICDMatched = true;
                                    }
                                });

                            });
                            if (ICDMatched == false) {
                                $("#" + Admin_IMOICD.params["PanelID"] + " #divIMOICDPaging").css("display", "none");
                                return true;
                            }
                        }
                        //End//17-08-2016//Ahmad Raza//PQRS ICD Search Changes
                        Admin_IMOICD.ICDGridLoad(response);

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

    ICDGridLoad: function (response) {
        
        $('#' + Admin_IMOICD.params["PanelID"] + ' #dgvIMOICD').dataTable().fnDestroy();
        $('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Result #dgvIMOICD tbody').find("tr").remove();
        
        if (response.ICDCount > 0) {
            var ICDLoadJSONData = JSON.parse(response.ICDLoad_JSON);
            $.each(ICDLoadJSONData, function (i, item) {
                if (item.ICD9 != "IMO0001" && item.ICD10 != "IMO0001") {
                    var selectICD = "";
                    var _concatinatedString = item.LexiCode + "*" + item.ICD9 + " +" + item.Description + "$" + item.ICD10 + " +" + item.ICD10Description + "~" + item.SNOMEDId + " +" + item.SNOMEDDescription + "^imo";
                    var $row = $('<tr/>');
                    //$row.attr("onclick", "utility.SelectGridRow($('#gvIMOICD_row" + item.ICDId + "'))");
                    var imo = "imo";
                    var prtCtrl = "";
                    //if (Admin_IMOICD.params.ParentCtrl != null &&
                    //    (Admin_IMOICD.params.ParentCtrl.indexOf("MedicalHx") > -1
                    //    //|| Admin_IMOICD.params.ParentCtrl.indexOf("HospitalizationHx") > -1 
                    //    )) {
                    //    prtCtrl = Admin_IMOICD.params.ParentCtrl;//+ "#Admin_IMOICD";
                    //}
                    //else {
                    prtCtrl = "Admin_IMOICD";
                    ///  }

                    var containerCtrl = Admin_IMOICD.params.RefHiddenCtrl;
                    var oncliclmethod = '';
                    
                    if (Admin_IMOICD.params.ParentCtrl == 'Clinical_CustomFormsPreview' || Admin_IMOICD.params.ParentCtrl == 'Batch_FaxSend' || Admin_IMOICD.params.ParentCtrl == 'clinicalTabCarePlan' || Admin_IMOICD.params.ParentCtrl == 'Clinical_CarePlan') {
                        oncliclmethod = "SupperBillDetail.OpenIMODetail('" + _concatinatedString + "', '" + imo + "', '" + prtCtrl + "', '" + containerCtrl + "', '" + imo + "', '" + Admin_IMOICD.params.ParentCtrl + "', null,'" + Admin_IMOICD.params.ContainerProblemDivId + "');";
                    }
                    else {
                        oncliclmethod = "SupperBillDetail.OpenIMODetail('" + _concatinatedString + "', '" + imo + "', '" + prtCtrl + "', '" + containerCtrl + "', '" + imo + "', '" + Admin_IMOICD.params.ParentCtrl + "');";
                    }
                    $row.attr("onclick", oncliclmethod);

                    //$row.attr("onclick", 'Admin_ICD.FillICDCode(' + item.ICDId + ', \'' + item.ICD9 + '\', \'' + item.Description + '\' , \'' + item.ICD10 + '\' , \'' + item.ICD10Description + '\' , \'' + item.SNOMEDId + '\'' + '\ , \'' + item.SNOMEDDescription + '\' , \'' + item.LexiCode + '\');');
                    //$row.attr("onclick", 'SupperBillDetail.OpenIMODetail(\'' + _concatinatedString + '\', \'' + _concatinatedString + '\', \'' + prt + '\', \'' + containerCtrl + '\', \'' + imo + '\');');


                    //$row.attr("id", "gvIMOICD_row" + item.ICDId);
                    $row.attr("ICDId", item.ICDId);


                    //if (Admin_IMOICD.params["FromAdmin"] == "0") {
                    //    selectICD = '&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_IMOICD.FillICDCode(' + item.ICDId + ', \'' + item.ICD9 + '\', \'' + item.Description + '\' , \'' + item.ICD10 + '\' , \'' + item.ICD10Description + '\' , \'' + item.SNOMEDId + '\'' + '\ , \'' + item.SNOMEDDescription + '\');" title="Select Record"><i class="fa fa-check black"></i></a>';
                    //    //Admin_IMOICD.FillICDCode(4, '00.09', 'OTHER THERAPEUTIC ULTRASOUND' , 'S12.01XA , 'Stable burst fracture of first cervical vertebra, initial encounter for closed fracture' , '269063003''+ , 'Closed fracture of first cervical vertebra without mention of spinal cord injury');
                    //}
                    // Faizan Ameen
                    // Dated : 28-03-2017
                    if (!Admin_IMOICD.IsShowICD10) {
                        
                       
                        $row.append('<td style="display:none;">' + item.ICDId + '</td><td>' + item.ICD9 + '</td><td>' + item.Description.replace("\''", "&apos") + '</td><td>' + item.ICD10 + '</td><td>' +
                            item.ICD10Description.replace("\''", "&apos") + '</td>');
                    }
                    else {
                        $('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Result #dgvIMOICD th#thICD9').remove();
                        $('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Result #dgvIMOICD th#thICD9Description').remove();
                        $row.append('<td style="display:none;">' + item.ICDId + '</td><td>' + item.ICD10 + '</td><td>' +
                                item.ICD10Description.replace("\''", "&apos") + '</td>');
                    }
                        //$row.append('<td style="display:none;">' + item.ICDId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_IMOICD.ICDDelete(' + item.ICDId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_IMOICD.ICDEdit(' + item.ICDId + ');"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_IMOICD.ICDActiveInactive(' + item.ICDId + ', ' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectICD + '</td><td>' + item.ICD9 + '</td><td>' + item.Description + '</td><td>' + item.ICD10 + '</td><td>' + item.ICD10Description + '</td><td>' + item.SNOMEDId + '</td><td>' + item.SNOMEDDescription + '</td><td>' + item.ActivelyUsed + '</td>');
                   
                    $('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Result #dgvIMOICD tbody').last().append($row);
                }
            });
        }
        else {
            $('#dgvIMOICD').DataTable({
                "language": {
                    "emptyTable": "No ICD Found"
                }
                , "bLengthChange": false, "autoWidth": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#dgvIMOICD'))
            ;
        else
            $('#' + Admin_IMOICD.params["PanelID"] + ' #pnlIMOICD_Result #dgvIMOICD').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchICD: function (ICDData, ICD, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        //ICDData = ICDData.replace(" (Info)", "");
        var data = "ICDData=" + ICDData + "&ICD=" + ICD + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Admin_IMOICD", "SEARCH_ICD");
    },

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#" + Admin_IMOICD.params["PanelID"] + " li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Admin_IMOICD.ICDSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Admin_IMOICD.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_IMOICD.ICDSearch(null, currentPageNo, 15);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Admin_IMOICD.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_IMOICD.ICDSearch(null, currentPageNo, 15);
        }
    },
    GetISShowICD10: function () {

        Admin_IMOICD.FillIsShowICD10(globalAppdata.AppUserName).done(function (response) {
            if (response.status == true) {
                var entityJSON = JSON.parse(response.ISShowICD10_JSON);
                if (entityJSON.hfIsShowICD10 == "True") {
                    Admin_IMOICD.IsShowICD10 = true;
                }
                else {
                    Admin_IMOICD.IsShowICD10 = false;
                }
            }


        });

    },
    FillIsShowICD10: function (userName) {
        var data = "userName=" + userName;
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "FILL_SHOW_ICD10");
    },
    UnLoadTab: function (Tab) {

        if (Admin_IMOICD.params != null && Admin_IMOICD.params.ParentCtrl != null) {
            if (Admin_IMOICD.params.FromProgressNote != null)
                UnloadActionPan(Admin_IMOICD.params.ParentCtrl, 'Admin_IMOICD', null, Admin_IMOICD.params.FromProgressNote);
            else
                UnloadActionPan(Admin_IMOICD.params.ParentCtrl, 'Admin_IMOICD');
        }
        else {
            //Begin Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
            UnloadActionPan(null, 'Admin_IMOICD');
            //End Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
        }
        // UnloadActionPan(null, 'Admin_IMOICD'); 

        Admin_IMOICD.params.RefHiddenCtrl = null;
    }
}
