
Clinical_LOINC = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        Clinical_LOINC.params = params;

        if (Clinical_LOINC.params["FromAdmin"] == "0" && Clinical_LOINC.params["PanelID"] == 'pnlClinicalLOINC')
            Clinical_LOINC.params["FromAdmin"] = "1";

        if (Clinical_LOINC.bIsFirstLoad) {

            Clinical_LOINC.bIsFirstLoad = false;

            var self = "";
            if (Clinical_LOINC.params["PanelID"] != "pnlClinicalLOINC") {
                self = $('#' + Clinical_LOINC.params["PanelID"] + " #pnlClinicalLOINC")
            }
            else
                self = $('#' + Clinical_LOINC.params["PanelID"]);

            if (Clinical_LOINC.params["ParentCtrl"] == "mstrTabReports") {
                self.find('#btn_LOINCAdd').hide();

            } else {
                self.find('#btn_LOINCAdd').show();
            }
            //self.loadDropDowns(true).done(function () {
            //});
        }
        //Start//Abid Ali to add test as result 
        if (Clinical_LOINC.params["displayTestControl"] != null) {

            //To display Test control.
            $('#' + Clinical_LOINC.params["PanelID"] + ' #cpt-control').removeClass('hidden');
            $('#' + Clinical_LOINC.params["PanelID"] + ' #loinic-control').addClass('hidden');
            $('#' + Clinical_LOINC.params["PanelID"] + ' .heading-title').text('Search Test');
        }
            //to add loinc as result 
        else {
            //To display LOINC control.
            $('#' + Clinical_LOINC.params["PanelID"] + ' #loinic-control').removeClass('hidden');
            $('#' + Clinical_LOINC.params["PanelID"] + ' #cpt-control').addClass('hidden');
            $('#' + Clinical_LOINC.params["PanelID"] + ' .heading-title').text('Search LOINC');
        }
        //End//Abid Ali to add test as result on radiology result page
        $(function () {
            $('#txtLOINCAndDescription').keypress(function (e) {
                var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keycode == 13) {
                    //Clinical_LOINC.LOINCSearch();
                }
            });
        });
    },

    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //This function will handle autocomplete of LOINC
    BindLOINCCodes: function () {
        $("#" + Clinical_LOINC.params.PanelID + " #txtLOINCAndDescription").autocomplete({
            autoFocus: true,
            source: function (request, response) {
                // utility.Keyupdelay(function () {
                var AccountNo = $('#' + Clinical_LOINC.params.PanelID + ' #txtLOINCAndDescription').val();
                if (AccountNo.length > 0) {
                    Clinical_LOINC.loadLabResultsLOINC(null, AccountNo).done(function (responseData) {
                        responseData = JSON.parse(responseData)
                        if (responseData.status != false) {
                            if (responseData.LabResultLOINCCount > 0) {
                                var LabResultLOINCLoadJSONData = JSON.parse(responseData.LabResultLOINCLoad_JSON);
                                var AllLOINC = [];
                                $.each(LabResultLOINCLoadJSONData, function (i, item) {

                                    AllLOINC.push({ id: item.LOINCCode, value: item.LOINCCode + " " + item.LOINCDescription, LOINCDescription: item.LOINCDescription, LabId: item.LabId, UoM: item.UoM, Range: item.Range, IsActive: item.IsActive, SampleStorage: item.SampleStorage, OrderTestLOINCId: item.OrderTestLOINCId });


                                });
                                response(AllLOINC);
                            }
                        }
                    });
                }

                //});
            },

            select: function (event, ui) {

                setTimeout(function () {

                    var parentControl = eval(Clinical_LOINC.params["ParentCtrl"]);
                    var controlName = Clinical_LOINC.params["ParentCtrl"];
                    if (parentControl != null) {

                        var obj = {};
                        obj["Observation"] = ui.item.value;
                        obj['LOINICCODE'] = ui.item.id;
                        obj['LOINICDescription'] = ui.item.LOINCDescription;
                        obj['OrderTestLOINCId'] = ui.item.LOINCDescription;

                        if (controlName.indexOf('ResultDetail') > -1) {

                            var row = Clinical_LOINC.params["GridRow"];
                            var $row = Clinical_LOINC.params["Grid$Row"];

                            //Pass Values to the child row added
                            var childRow = parentControl.addNewResultChildRow(row, obj, null, null, null, null, null, null, null, null, $row);
                            var parentId = $(childRow).attr("child-id");
                            var gRow = null;
                            if (parentControl.params.ParentCtrl == "Clinical_LabOrder" || parentControl.params.ParentCtrl == "ClinicalLabOrderDetail" || parentControl.params.ParentCtrl == "clinicalTabLabOrder")
                                gRow = parentControl.addNewResultGrandChildRow(parentId, row, obj, null, $row);
                            if (childRow != null) {

                                if (gRow != null)
                                    childRow = childRow.add(gRow);

                                childRow = childRow.add(row.child());
                                row.child(childRow);



                                // Show minus icon on children expand
                                $($row).find("td:first").find('a').show();
                                $($row).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                                // Open this row
                                row.child.show();

                                //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
                                ClinicalLabResultDetail.SetDateTimeControl(childRow, true);
                                //Hide show form buttons
                                parentControl.enableDisableOrderResultButtons();
                            }
                            else {
                                utility.DisplayMessages("Result is already selected", 2);
                            }
                        }
                        else if (controlName.indexOf('Favorite_LabOrderDetail') > -1) {

                            parentControl.pushLOINCAsCpt(obj);

                        }
                        else if (controlName.indexOf('clinicalTabLabOrder') > -1) {

                            var refCtrl = $(Clinical_LOINC.params["RefCtrl"]);
                            Clinical_LabOrder.pushLOINCAsCpt(obj, refCtrl);

                        }
                        //Clear Loinic txt field
                        $("#" + Clinical_LOINC.params.PanelID + " #txtLOINCAndDescription").val("");

                    }

                }, 100);
            }
        });
    },


    ValidateLOINC: function () {
        $('#frmLOINCIMO')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   LOINCAndDescription: {
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
            Clinical_LOINC.LOINCSearch();
        });
    },

    LOINCSearch: function (LOINCId, PageNo, rpp) {
        var LOINC = $("#txtLOINCAndDescription").val();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("LOINC", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Clinical_LOINC.params["PanelID"] + ' #pnlLOINC_Result').css("display") == "none") {
                    $('#' + Clinical_LOINC.params["PanelID"] + ' #pnlLOINC_Result').show();
                }
                var self = "";
                if (Clinical_LOINC.params["PanelID"] != "pnlClinicalLOINC") {
                    self = $('#' + Clinical_LOINC.params["PanelID"] + " #pnlClinicalLOINC #pnlLOINC_Search")
                }
                else
                    self = $('#' + Clinical_LOINC.params["PanelID"] + " #pnlLOINC_Search");

                //  var self = $('#' + Clinical_LOINC.params["PanelID"] + ' #pnlLOINC_Search');
                var myJSON = self.getMyJSON();


                Clinical_LOINC.SearchLOINC(myJSON, LOINC, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.LOINCCount > 0) {
                            $("#" + Clinical_LOINC.params["PanelID"] + " #divLOINCPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;
                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divLOINCPaging", response.iTotalDisplayRecords, 5, "Clinical_LOINC", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Clinical_LOINC.params["PanelID"] + " #divLOINCPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            self.find("li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $("#" + Clinical_LOINC.params["PanelID"] + " #divLOINCPaging").css("display", "none");
                        }
                        Clinical_LOINC.LOINCGridLoad(response);

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

    LOINCGridLoad: function (response) {
        $('#' + Clinical_LOINC.params["PanelID"] + ' #dgvLOINC').dataTable().fnDestroy();
        $('#' + Clinical_LOINC.params["PanelID"] + ' #pnlLOINC_Result #dgvLOINC tbody').find("tr").remove();
        if (response.LOINCCount > 0) {
            var LOINCLoadJSONData = JSON.parse(response.LOINCLoad_JSON);
            $.each(LOINCLoadJSONData, function (i, item) {
                var selectLOINC = "";
                var _concatinatedString = item.LOINCCode + "-" + item.Description;
                var $row = $('<tr/>');
                var imo = "imo";
                var prtCtrl = "Clinical_LOINC";
                var containerCtrl_ = Clinical_LOINC.params.RefCtrl;
                var hiddenCtrl = Clinical_LOINC.params.RefHiddenCtrl;
                var containerCtrlDescription = Clinical_LOINC.params.RefCtrlDescription;
                var containerCtrl = containerCtrl_ + '@' + containerCtrlDescription;
                var onclickmethod = "LOINCcodeDetail.SetControlValues('" + _concatinatedString + "', '" + item.LexiCode + "', '" + prtCtrl + "', '" + containerCtrl + "', '" + hiddenCtrl + "', '" + Clinical_LOINC.params.ParentCtrl + "');";
                $row.attr("onclick", onclickmethod);

                $row.attr("LOINCId", item.LOINCId);

                $row.append('<td style="display:none;">' + item.LOINCId + '</td><td>' + item.LOINCCode + '</td><td>' + item.Description.replace("\''", "&apos") + '</td><td>' + item.SNOMEDId + '</td><td>' + item.SNOMEDDescription.replace("\''", "&apos") + '</td>');
                $('#' + Clinical_LOINC.params["PanelID"] + ' #pnlLOINC_Result #dgvLOINC tbody').last().append($row);
            });
        }
        else {
            $('#dgvLOINC').DataTable({
                "language": {
                    "emptyTable": "No LOINC Found"
                }
                , "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#dgvLOINC'))
            ;
        else
            $('#' + Clinical_LOINC.params["PanelID"] + ' #pnlLOINC_Result #dgvLOINC').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchLOINC: function (LOINCData, LOINC, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "LOINCData=" + LOINCData + "&LOINC=" + LOINC + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Clinical_LOINC", "SEARCH_LOINC");
    },

    //Author: Muhammad Arshad
    //Date :  18-04-2016
    //Description: Db Call for loading LabResult LOINC
    loadLabResultsLOINC: function (LOINCCode, LOINCCOdeDescription, LabId) {
        var objData = {};
        if (LOINCCode == null) {
            LOINCCode = "";
        }

        if (LOINCCOdeDescription == null) {
            LOINCCOdeDescription = "";
        }

        objData["LOINCCode"] = LOINCCode;
        objData["LOINCDescription"] = LOINCCOdeDescription;
        objData["LabId"] = LabId;
        objData["commandType"] = "load_LabResultLOINC";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    loadLabResultsOrganisms: function (LOINCCOdeDescription) {
        var objData = {};


        if (LOINCCOdeDescription == null) {
            LOINCCOdeDescription = "";
        }

        objData["LOINCDescription"] = LOINCCOdeDescription;

        objData["commandType"] = "load_LabResultOrganisms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_LOINC", null, true);

    },

    BindResultGridItem: function (cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription) {

        var parentControl = eval(Clinical_LOINC.params["ParentCtrl"]);
        var controlName = Clinical_LOINC.params["ParentCtrl"];
        if (parentControl != null) {

            var obj = {};

            obj["Observation"] = procedureDescription;
            obj['LOINICCODE'] = cptCode;
            obj['LOINICDescription'] = procedureDescription;
            obj['CPTSNOMEDCodeId'] = SNOMEDId;
            obj['CPTSNOMEDDescription'] = SNOMEDDescription;

            if (controlName.indexOf('ResultDetail') > -1) {

                var row = Clinical_LOINC.params["GridRow"];
                var $row = Clinical_LOINC.params["Grid$Row"];

                //Pass Values to the child row added
                var childRow = parentControl.addNewResultChildRow(row, obj);
                var parentId = $(childRow).attr("child-id");

                var gRow = null;
                if (parentControl.params.ParentCtrl == "Clinical_LabOrder" || parentControl.params.ParentCtrl == "ClinicalLabOrderDetail" || parentControl.params.ParentCtrl == "clinicalTabLabOrder")
                    gRow = parentControl.addNewResultGrandChildRow(parentId, row, obj);

                if (childRow != null) {

                    if (gRow != null)
                        childRow = childRow.add(gRow);

                    childRow = childRow.add(row.child());
                    row.child(childRow);

                    // Show minus icon on children expand
                    $($row).find("td:first").find('a').show();
                    $($row).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                    // Open this row
                    row.child.show();


                    //Hide show form buttons
                    parentControl.enableDisableOrderResultButtons();
                    //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
                    ClinicalLabResultDetail.SetDateTimeControl(childRow, true);
                }
                else {
                    utility.DisplayMessages("Result is already selected", 2);
                }
                $('#' + Clinical_LOINC.params["PanelID"] + ' #txtCPTCode').val("");

            }
        }
    },
    UnLoadTab: function (Tab) {

        if (Clinical_LOINC.params != null && Clinical_LOINC.params.ParentCtrl != null) {

            UnloadActionPan(Clinical_LOINC.params.ParentCtrl, 'Clinical_LOINC');

            if (Clinical_LOINC.params.PanelID.indexOf("pnlClinical") != -1) {
                UnloadActionPan(Clinical_LOINC.params.ParentCtrl, 'Clinical_LOINC', null, Clinical_LOINC.params.PanelID);
            }
        }
        else
            UnloadActionPan(null, 'Clinical_LOINC');
    },


}
