Batch_PatientList = {
    params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,

    Load: function (params) {
        Batch_PatientList.params = params;
        if (Batch_PatientList.params.PanelID != 'pnlBatchPatientList') {
            Batch_PatientList.params.PanelID = Batch_PatientList.params.PanelID + ' #pnlBatchPatientList';
        } else {
            Batch_PatientList.params.PanelID = 'pnlBatchPatientList';
        }
        if (Batch_PatientList.bIsFirstLoad) {
            Batch_PatientList.bIsFirstLoad = false;
            var self = $('#' + Batch_PatientList.params.PanelID);
            self.loadDropDowns(true).done(function () {
                // $('#ddlSex').prepend('<option>All</option>').val($("#ddlSex option:first").val());

                Batch_PatientList.listSearchLength = $('#' + Batch_PatientList.params.PanelID + ' #PMAL li').length;
                Batch_PatientList.IntializeMultiSelectDropDown();
                $('#' + Batch_PatientList.params.PanelID + ' #frmBatchPatientList').data('serialize', $('#' + Batch_PatientList.params.PanelID + ' #frmBatchPatientList').serialize());

            });
            $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList").DataTable({
                "language": {
                    "emptyTable": "No Patient Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
            utility.ValidateFromToDate('pnlBatchPatientList', 'dtFromPtCreate', 'dtToPtCreate', true);
            utility.ValidateFromToDate('pnlBatchPatientList', 'dtFromProblems', 'dtToProblems', true);
            utility.ValidateFromToDate('pnlBatchPatientList', 'dtFromMedication', 'dtToMedication', true);
            utility.ValidateFromToDate('pnlBatchPatientList', 'dtFromAllergies', 'dtToAllergies', true);
            utility.ValidateFromToDate('pnlBatchPatientList', 'dtFromLabResult', 'dtToLabResult', true);

        }





    },
    IntializeMultiSelectDropDown: function () {
        $('#' + Batch_PatientList.params.PanelID + ' #ddlSex').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 290

        });
        $('#' + Batch_PatientList.params.PanelID + ' #ddlSmokingStatus').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 290

        });
        $('#' + Batch_PatientList.params.PanelID + ' #ddlRace').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 290

        });
        $('#' + Batch_PatientList.params.PanelID + ' #ddlEthnicity').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 290

        });
        $('#' + Batch_PatientList.params.PanelID + ' #ddlPrefLanguage').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 290

        });
        $('#' + Batch_PatientList.params.PanelID + ' #ddlPreferredComm').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 290

        });
    },
    // This Function will replace the text with ToFrom Calendar
    // Author: ZeeshanAK | Date: April 05, 2016
    showFromToCalendar: function (obj) {
        $(obj).parents('#divLabel').hide();
        $(obj).parents('#divLabel').next().removeClass('hidden');
        if($(obj).attr('id') == 'ptCreateDate')
            $(obj).parents('#divLabel').parent().attr('class', 'col-sm-4 col-md-3');
        $(obj).parents('#divLabel').next().find('input').each(function (i, elem) {
            utility.CreateDatePicker(Batch_PatientList.params.PanelID + '  #' + elem.id + "'", function () {
            }, true);
        });
    },

    /*
    Author: Muhammad Azhar Shahzad
    Purpose: To Show Grid Data
    Created on April 06, 2016*/
    patientListSearch: function (patientListId, PageNo, rpp, searchbtnClick) {
        var ProcessRequest = true;
        if (searchbtnClick) {
            ProcessRequest = EMRUtility.compareFormDataWithSerialized(Batch_PatientList.params.PanelID + ' #frmBatchPatientList') || (Batch_PatientList.listSearchLength != $('#' + Batch_PatientList.params.PanelID + ' #PMAL li').length);
        }
        if (ProcessRequest) {

            if (PageNo == null) {
                PageNo = 1;
            }
            if (rpp == null) {
                rpp = 30;
            }
            Batch_PatientList.patientList_DbCall(patientListId, PageNo, rpp).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Batch_PatientList.PatientListGridLoad(response);
                    //Adding Pagination on 04 Dec 2015 by Azhar
                    var TableControl = Batch_PatientList.params.PanelID + " #dgvBatchPatientList";
                    var PagingPanelControlID = Batch_PatientList.params.PanelID + " #divBatchPatientListPaging";
                    var ClassControlName = "Batch_PatientList";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.BatchPatientListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Batch_PatientList.patientListSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                    if (!$('#' + Batch_PatientList.params.PanelID + " #divBatchPatientListPaging").hasClass('mt-md')) {
                        $('#' + Batch_PatientList.params.PanelID + " #divBatchPatientListPaging").addClass('mt-md')
                    }
                    $('#' + Batch_PatientList.params.PanelID + ' #frmBatchPatientList').data('serialize', $('#' + Batch_PatientList.params.PanelID + ' #frmBatchPatientList').serialize());
                    Batch_PatientList.listSearchLength = $('#' + Batch_PatientList.params.PanelID + ' #PMAL li').length;
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            utility.DisplayMessages("Please enter any criteria for search.", 3);
        }
    },

    /*
  Author: Muhammad Azhar Shahzad
  Purpose:  To export Excel of Grid Data
  Created on April 06, 2016*/
    patientListPrint: function () {
        Batch_PatientList.printPatientList_DbCall(-1, -1, -1).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                //  window.open('data:application/vnd.ms-excel,' + response.HTMLResponse);
                var uri = '';//'data:application/vnd.ms-excel;base64,';
                download(uri + response.HTMLResponse, "PatientList.xls", "application/octet-stream");
                // Batch_PatientList.tableToExcel(response.HTMLResponse, 'W3C Example Table');
            } else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    jqgridData: function (BatchPatientListLoad_JSON, BatchPatientListCount) {
        //   grid = ;
        var problemIds = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li").map(function () {
            return $.trim($(this).attr('id'));
        });

        var problemDateFrom = $("#" + Batch_PatientList.params.PanelID + " #dtFromProblems").val();
        var problemDateTo = $("#" + Batch_PatientList.params.PanelID + " #dtToProblems").val();

        var medicationIds = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li").map(function () {
            return $.trim($(this).attr('id'));
        });

        var medDateFrom = $("#" + Batch_PatientList.params.PanelID + " #dtFromMedication").val();
        var medDateTo = $("#" + Batch_PatientList.params.PanelID + " #dtToMedication").val();

        var allergyIds = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li").map(function () {
            return $.trim($(this).attr('id'));
        });

        var allergyDateFrom = $("#" + Batch_PatientList.params.PanelID + " #dtFromAllergies").val();
        var allergyDateTo = $("#" + Batch_PatientList.params.PanelID + " #dtToAllergies").val();

        var LabResultsIds = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li").map(function () {
            return $.trim($(this).attr('id'));
        });

        var resultDateFrom = $("#" + Batch_PatientList.params.PanelID + " #dtFromLabResult").val();
        var resultDateTo = $("#" + Batch_PatientList.params.PanelID + " #dtToLabResult").val();

        var hideProblem = problemIds.length > 0 || problemDateFrom || problemDateTo ? false : true;
        var hideMedication = medicationIds.length > 0 || medDateFrom || medDateTo ? false : true;
        var hideAllergy = allergyIds.length > 0 || allergyDateFrom || allergyDateTo ? false : true;
        var hideLabResults = LabResultsIds.length > 0 || resultDateFrom || resultDateTo ? false : true;
        var sourceFormate = "Y-m-d H:i:s";// "m/d/Y H:i:s A";
        var newFormate = "m/d/Y h:i A";
        var heightPx = 30 * BatchPatientListCount;
        heightPx = heightPx < 60 ? 60 : heightPx;
        $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList").jqGrid({
            datatype: 'local',
            data: BatchPatientListLoad_JSON,
            // to increase number of records for jqGrid
            rowNum: BatchPatientListCount,
            colNames: ['Action','PatientId', 'Account', 'Patient', 'Gender', 'DOB', 'Smoking', 'Race', 'Ethnicity',
                'Language', 'Communication', 'Pt. Creation', 'Problem', 'Problem Date', 'Medication', 'Medication Date'
            , 'Allergy', 'Allergy Date', 'Lab Results', 'Lab Results Date'],
            colModel: [{
                name: 'Action', index: 'PatientId', align: "center", formatter: Batch_PatientList.formateActions,
                cellattr: function (rowId, tv, rawObject, cm, rdata) {
                    return 'id=\'PatientId' + rowId + "\'";

                }
            }
                , {
                    name: 'PatientId', index: 'PatientId', editable: false, hidden: true
                }, {
                    name: 'AccountNumber',
                    index: 'AccountNumber',
                    align: 'center',
                    sorttype: 'text',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'AccountNumber' + rowId + "\'";

                    }
                }, {
                    name: 'PatientFullName',
                    Width: 150,
                    index: 'PatientFullName',
                    align: 'center',
                    sorttype: 'text',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'PatientFullName' + rowId + "\'";

                    }
                }, {
                    name: 'Gender',
                    index: 'Gender',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'Gender' + rowId + "\'";

                    }
                }, {
                    name: 'DOB',
                    index: 'DOB',
                    align: 'center',
                    sorttype: 'date',
                    formatter: 'date',
                    formatoptions: {
                        newformat: 'm/d/Y',
                        srcformat: 'm/d/Y H:i:s A'
                        //srcformat: 'm/d/Y h:i:s A'
                    },
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'DOB' + rowId + "\'";

                    }
                }, {
                    name: 'SmokingStatus',
                    index: 'SmokingStatus',
                    align: 'center',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'SmokingStatus' + rowId + "\'";

                    }
                }, {
                    name: 'Race',
                    index: 'Race',
                    align: 'center',

                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'Race' + rowId + "\'";

                    }
                }, {
                    name: 'Ethnicity',
                    index: 'Ethnicity',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'Ethnicity' + rowId + "\'";
                    }
                }, {
                    name: 'Language',
                    index: 'Language',
                    align: 'center',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'Language' + rowId + "\'";

                    }
                }, {
                    name: 'Communication',
                    index: 'Communication',
                    align: 'center',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'Communication' + rowId + "\'";

                    }
                }, {

                    name: 'CreatedOn',
                    index: 'CreatedOn',
                    align: 'center',
                    sorttype: 'date',
                    formatter: 'date',
                    formatoptions: {
                        // newformat: 'm/d/Y',
                        newformat: newFormate,
                        srcformat: 'm/d/Y H:i:s A',//sourceFormate
                        //srcformat: 'm/d/Y h:i:s A'
                    },
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'CreatedOn' + rowId + "\'";
                    }
                }, {
                    name: 'ProblemName',
                    index: 'ProblemName',
                    align: 'center',
                    sorttype: 'text',
                    hidden: hideProblem,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'ProblemName' + rowId + "\'";
                    }
                }, {
                    name: 'ProblemDate',
                    index: 'ProblemDate',
                    hidden: hideProblem,
                    align: 'center',
                    sorttype: 'date',
                    formatter: 'date',
                    formatoptions: {
                        //  newformat: 'm/d/Y',
                        newformat: newFormate,
                        srcformat: sourceFormate
                        //srcformat: 'm/d/Y h:i:s A'
                    },
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'ProblemDate' + rowId + "\'";
                    }
                }, {
                    name: 'Medication',
                    index: 'Medication',
                    hidden: hideMedication,
                    align: 'center',
                    sorttype: 'text',

                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'Medication' + rowId + "\'";
                    }
                }, {
                    name: 'MedicationDate',
                    index: 'MedicationDate',
                    hidden: hideMedication,
                    align: 'center',
                    sorttype: 'date',
                    formatter: 'date',
                    formatoptions: {
                        // newformat: 'm/d/Y',
                        newformat: newFormate,
                        srcformat: sourceFormate
                        //srcformat: 'm/d/Y h:i:s A'
                    },
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'MedicationDate' + rowId + "\'";
                    }
                }, {
                    name: 'Allergy',
                    index: 'Allergy',
                    hidden: hideAllergy,
                    align: 'center',
                    sorttype: 'text',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'Allergy' + rowId + "\'";
                    }
                }, {
                    name: 'AllergyDate',
                    index: 'AllergyDate',
                    hidden: hideAllergy,
                    align: 'center',
                    sorttype: 'date',
                    formatter: 'date',
                    formatoptions: {
                        // newformat: 'm/d/Y',
                        newformat: newFormate,
                        srcformat: sourceFormate
                        //srcformat: 'm/d/Y h:i:s A'
                    },
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'AllergyDate' + rowId + "\'";
                    }
                }, {
                    name: 'LabResults',
                    index: 'LabResults',
                    hidden: hideLabResults,
                    align: 'center',
                    sorttype: 'text',
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'LabResults' + rowId + "\'";
                    }
                }, {
                    name: 'LabResultsDate',
                    index: 'LabResultsDate',
                    align: 'center',
                    hidden: hideLabResults,
                    sorttype: 'date',
                    formatter: 'date',
                    formatoptions: {
                        // newformat: 'm/d/Y',
                        newformat: newFormate,
                        srcformat: sourceFormate
                        //srcformat: 'm/d/Y h:i:s A'
                    },
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'LabResultsDate' + rowId + "\'";
                    },
                }],
            gridview: false,
            multiSort: true,
            sortname: 'PatientId',
            sortorder: 'desc',
            shrinkToFit: false,
            forceFit: false,
            autowidth: true,
            autoheight: true,
            scrollTimeout: 50,
            headertitles: true,
            height: heightPx,
            gridComplete: function () {
                //Batch_PatientList.Merger(gridName, 'PatientId');
                //Batch_PatientList.Merger(gridName, 'AccountNumber');
                //Batch_PatientList.Merger(gridName, 'PatientFullName');
                //  Batch_PatientList.Merger(gridName, 'Gender');
                //Batch_PatientList.Merger(gridName, 'DOB');
                //Batch_PatientList.Merger(gridName, 'Medication');
                //Batch_PatientList.Merger(gridName, 'Allergy');
                //Batch_PatientList.Merger(gridName, 'LabResults');
                //Batch_PatientList.Merger(gridName, 'ProblemName');

                var gridName = "dgvBatchPatientList";
                jQuery("#lui_" + gridName).hide();
                jQuery("#load_" + gridName).hide();
                jQuery("#gbox_" + gridName).addClass("noBg");

            },
            //Row Click event
            onSelectRow: function (rowId, tv, event) {
                var rowData = $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList").getRowData(rowId);
                if (rowId && rowData) {
                    if (rowData.PatientId) {
                        Batch_PatientList.PatientAddEdit(rowData.PatientId, 'Edit', event);
                    }
                }
            },
            // all current options
            loadComplete: function () {
                var gridName = "dgvBatchPatientList";
                jQuery("#lui_" + gridName).hide();
                jQuery("#load_" + gridName).hide();
                $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList tr.jqgrow td').css('white-space', 'normal');
                Batch_PatientList.getFilteredGrid();
            },
            loadError: function () {
                var gridName = "dgvBatchPatientList";
                jQuery("#lui_" + gridName).hide();
                jQuery("#load_" + gridName).hide();
            }

        });
    },

    /*
  Author: Muhammad Azhar Shahzad
  Purpose: To Show Grid Data Details binding to Tables
  Created on April 06, 2016*/
    PatientListGridLoad: function (response) {
        var mydataGrid = {};
        $('#' + Batch_PatientList.params.PanelID + " #pnlBatchPatientList_Result").show();
        $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList").jqGrid('GridUnload');
        if (response.BatchPatientListCount > 0) {
            var filterGridBatchPatientList = ' <div class="col-md-12 col-sm-12" id="filterGridBatchPatientList" style="display:block">' +
                '<div class="pull-right pt-md pr-default col-md-3 pb-md">' +
                   " <input type='text' id='txtSearchCharacteristics' onkeyup='Batch_PatientList.getFilteredGrid(this);' class='form-control' placeholder='Search ...' />" +
                '</div>            </div>';
            $('#' + Batch_PatientList.params.PanelID + " #filterGridBatchPatientList").show();
            var BatchPatientListLoad_JSON = JSON.parse(response.BatchPatientListLoad_JSON);
            if ($.fn.dataTable.isDataTable('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList")) {
                $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList").dataTable().fnDestroy();
                $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList tbody").find("tr").remove();
            }
            Batch_PatientList.jqgridData(BatchPatientListLoad_JSON, response.BatchPatientListCount);
            if ($('#' + Batch_PatientList.params.PanelID + " #filterGridBatchPatientList").length <= 0) {
                $('#' + Batch_PatientList.params.PanelID + " #gview_dgvBatchPatientList").prepend(filterGridBatchPatientList);
            } else {
                $('#' + Batch_PatientList.params.PanelID + " #gview_dgvBatchPatientList").prepend($('#' + Batch_PatientList.params.PanelID + " #filterGridBatchPatientList"));
            }

        }
        else {
            $('#' + Batch_PatientList.params.PanelID + " #filterGridBatchPatientList").hide();
            if ($.fn.dataTable.isDataTable('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList")) {
                $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList").dataTable().fnDestroy();
                $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList tbody").find("tr").remove();
            }
            $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList").DataTable({
                "language": {
                    "emptyTable": "No Patient Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
            $('#' + Batch_PatientList.params.PanelID + " #dgvBatchPatientList th").text("Patient List");
        }


    },
    // This Function will replace the text with ToFrom Calendar
    // Author: ZeeshanAK | Date: April 05, 2016
    //showFromToCalendar: function (obj) {
    //    $(obj).parents('#divLabel').hide();
    //    $(obj).parents('#divLabel').next().removeClass('hidden');
    //    $(obj).parents('#divLabel').parent().attr('class', 'col-sm-4 col-md-3');
    //    $(obj).parents('#divLabel').next().find('input').each(function (index, elem) {
    //        utility.CreateDatePicker(Batch_PatientList.params.PanelID + '  #' + elem.id, function () {
    //        }, true);
    //    });
    //},

    // AutoComplete for Problems
    // Author: ZeeshanAK | Date: April 05, 2016
    BindproblemText: function () {
        var problemText = $('#' + Batch_PatientList.params.PanelID + ' #txtProblem');

        utility.Keyupdelay(function () {
            problemText.autocomplete({
                autoFocus: true,
                source: function (request, response) {
                    // utility.Keyupdelay(function () {
                    Batch_PatientList.loadProblems_DBCall(problemText.val()).done(function (responseData) {
                        var responseData = JSON.parse(responseData);
                        if (responseData.status != false) {
                            if (responseData.ProblemCount > 0) {
                                var Problems = JSON.parse(responseData.ProblemLoad_JSON);
                                var allProblems = [];
                                $.each(Problems, function (i, item) {
                                    allProblems.push({ id: item.ProblemListId, value: item.Description });
                                });
                                response(allProblems);
                            }
                        }
                    });

                    //  });
                },

                select: function (event, ui) {

                    setTimeout(function () {
                        var problemLi = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li");
                        if (problemLi.length > 0) {

                            var problemLi = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li").map(function () {
                                return $.trim($(this).text());
                            });

                            var isMatch = jQuery.grep(problemLi, function (element, index) {
                                return element == ui.item.value;
                            });

                            isMatch.length > 0 ? utility.DisplayMessages("Problem alreay exist.", 2) : $("#" + Batch_PatientList.params.PanelID + " #ulProblems").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>');
                            problemText.val('');
                        } else {
                            $("#" + Batch_PatientList.params.PanelID + " #ulProblems").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>');
                            problemText.val('');
                        };
                    }, 100);
                }
            });
        });
    },

    // AutoComplete for Allergies
    // Author: ZeeshanAK | Date: April 06, 2016
    BindAllergiesText: function () {
        var allergiesText = $('#' + Batch_PatientList.params.PanelID + ' #txtAllergies');
        
        utility.Keyupdelay(function () {
            allergiesText.autocomplete({
                autoFocus: true,
                source: function (request, response) {
                    //utility.Keyupdelay(function () {
                    Batch_PatientList.loadAllergies_DBCall(allergiesText.val()).done(function (responseData) {
                        var responseData = JSON.parse(responseData);
                        if (responseData.status != false) {
                            if (responseData.AllergiesCount > 0) {
                                var Allergies = JSON.parse(responseData.AllergiesLoad_JSON);
                                var allAllergies = [];
                                $.each(Allergies, function (i, item) {
                                    allAllergies.push({ id: item.AllergyId, value: item.Allergen });
                                });
                                response(allAllergies);
                            }
                        }
                    });

                    // });
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        var allergyLi = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li");
                        if (allergyLi.length > 0) {

                            var allergyLi = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li").map(function () {
                                return $.trim($(this).text());
                            });

                            var isMatch = jQuery.grep(allergyLi, function (element, index) {
                                return element == ui.item.value;
                            });

                            isMatch.length > 0 ? utility.DisplayMessages("Allergy alreay exist.", 2) : $("#" + Batch_PatientList.params.PanelID + " #ulAllergies").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>');
                            allergiesText.val('');
                        } else {
                            $("#" + Batch_PatientList.params.PanelID + " #ulAllergies").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>');
                            allergiesText.val('');
                        };
                    }, 100);
                }
            });
        });
    },


    // AutoComplete for Medications
    // Author: ZeeshanAK | Date: April 06, 2016
    BindMedicationText: function () {

        var medicationsText = $('#' + Batch_PatientList.params.PanelID + ' #txtMedication');
        utility.Keyupdelay(function () {
            medicationsText.autocomplete({
                autoFocus: true,
                source: function (request, response) {
                    //utility.Keyupdelay(function () {

                        Batch_PatientList.loadMedications_DBCall(medicationsText.val()).done(function (responseData) {
                            var responseData = JSON.parse(responseData);
                            if (responseData.status != false) {
                                if (responseData.MedicationsCount > 0) {
                                    var Medications = JSON.parse(responseData.MedicationsLoad_JSON);
                                    var allMedications = [];

                                    $.each(Medications, function (i, item) {
                                        allMedications.push({ id: item.MedicationID, value: item.MedicationName });

                                    });
                                    response(allMedications);
                                }
                            }
                        });

                    //});
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        var medicationLi = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li");
                        if (medicationLi.length > 0) {

                            var medicationLi = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li").map(function () {
                                return $.trim($(this).text());
                            });

                            var isMatch = jQuery.grep(medicationLi, function (element, index) {
                                return element == ui.item.value;
                            });

                            isMatch.length > 0 ? utility.DisplayMessages("Medication alreay exist.", 2) : $("#" + Batch_PatientList.params.PanelID + " #ulMedications").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>');
                            medicationsText.val('');
                        } else {
                            $("#" + Batch_PatientList.params.PanelID + " #ulMedications").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>')
                            medicationsText.val('');
                        };
                    }, 100);
                }
            });
        });
    },

    // AutoComplete for Lab Result
    // Author: ZeeshanAK | Date: April 06, 2016
    BindLabResultText: function () {
        var LabResultControl = $('#' + Batch_PatientList.params.PanelID + ' #txtLabResult');
        utility.Keyupdelay(function () {
            LabResultControl.autocomplete({
                autoFocus: true,
                source: function (request, response) {
                    //utility.Keyupdelay(function () {
                    var AccountNo = LabResultControl.val();
                    if (AccountNo.length > 0) {
                        Clinical_LOINC.loadLabResultsLOINC(null, AccountNo).done(function (responseData) {
                            responseData = JSON.parse(responseData)
                            if (responseData.status != false) {
                                if (responseData.LabResultLOINCCount > 0) {
                                    var LabResultLOINCLoadJSONData = JSON.parse(responseData.LabResultLOINCLoad_JSON);
                                    var AllLOINC = [];
                                    $.each(LabResultLOINCLoadJSONData, function (i, item) {

                                        AllLOINC.push({ id: item.LOINCCode, value: item.LOINCCode + " " + item.LOINCDescription, LOINCDescription: item.LOINCDescription, LabId: item.LabId, UoM: item.UoM, Range: item.Range, IsActive: item.IsActive });


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
                        var medicationLi = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li");
                        if (medicationLi.length > 0) {

                            var medicationLi = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li").map(function () {
                                return $.trim($(this).text());
                            });

                            var isMatch = jQuery.grep(medicationLi, function (element, index) {
                                return element == ui.item.value;
                            });

                            isMatch.length > 0 ? utility.DisplayMessages("Lab Result alreay exist.", 2) :
                            $("#" + Batch_PatientList.params.PanelID + " #ulLabResults").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>');
                            LabResultControl.val('');
                        } else {
                            $("#" + Batch_PatientList.params.PanelID + " #ulLabResults").append('<li id="' + ui.item.id + '" ><a class="">' + ui.item.value + '<span class="removeIconList" onclick="Batch_PatientList.deleteLiFromBox(this);"><i class="fa fa-times"></i></span> </a></li>')
                            LabResultControl.val('');
                        };
                    }, 100);

                }
            });
        });
    },

    // This Function will delete the li from respective Box
    // Author: ZeeshanAK | Date: April 06, 2016
    deleteLiFromBox: function (obj) {
        $(obj).parents('li').remove();
    },

    // This function won't allow FromAge to be greater from ToAge
    // Author: ZeeshanAK | Date: April 13, 2016
    compareAge: function (obj) {
        var ageFrom = $('#' + Batch_PatientList.params.PanelID + ' #frmBatchPatientList #ageFrom').val();
        var ageTo = $('#' + Batch_PatientList.params.PanelID + ' #frmBatchPatientList #ageTo').val();
        if (ageFrom != '' && ageTo != '') {
            if (Number(ageTo) < Number(ageFrom)) {
                utility.DisplayMessages("Age range is not correct.", 3);
                $(obj).focus();
                $(obj).val("");
            }
        }
    },

    // DB call to get Problems
    // Author: ZeeshanAK | Date: April 05, 2016
    loadProblems_DBCall: function (problemText) {
        var objData = new Object();
        objData["ProblemName"] = problemText;
        objData["commandType"] = "get_all_problemlists";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    // DB call to get Problems
    // Author: ZeeshanAK | Date: April 06, 2016
    loadAllergies_DBCall: function (allergen) {
        var objData = new Object();
        objData["Allergen"] = allergen;
        objData["commandType"] = "lookup_allergies";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");
    },

    // DB call to get Medications
    // Author: ZeeshanAK | Date: April 06, 2016
    loadMedications_DBCall: function (medicationName) {
        var objData = new Object();
        objData["MedicationName"] = medicationName;
        objData["commandType"] = "lookup_Medications";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },

    /*
  Author: Muhammad Azhar Shahzad
  Purpose: To Show Grid Data DB CAll
  Created on April 06, 2016*/
    patientList_DbCall: function (patientListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 30;
        }
        var self = $("#" + Batch_PatientList.params.PanelID);
        var myJSON = self.getMyJSONByName();
        myJSON = JSON.parse(myJSON);
        var objData = myJSON;
        var problemIds = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li").map(function () {
            return $.trim($(this).attr('id'));
        });

        var problemTexts = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li").map(function () {
            return $(this).text();
        });

        //var medicationIds = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});

        //var allergyIds = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});
        //var labResultIds = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});

        var medicationText = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li").map(function () {
            return $(this).text();
        });

        var allergyIds = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li").map(function () {
            return $.trim($(this).attr('id'));
        });
        var labResultText = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li").map(function () {
            return $(this).text();
        });


        //
        objData["gender"] = self.find('#ddlSex option:Selected').map(function () {
            return this.text;
        }).get().join(',');

        objData["SmokingStatusId"] = self.find('#ddlSmokingStatus option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["RaceId"] = self.find('#ddlRace option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["EthnicityId"] = self.find('#ddlEthnicity option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["PrefLanguageId"] = self.find('#ddlPrefLanguage option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["PrefCommunicationId"] = self.find('#ddlPreferredComm option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        //

        objData["PatientListId"] = patientListId == null ? -1 : patientListId;
        //objData["SmokingStatusId"] = objData["SmokingStatusId"] == "" ? -1 : objData["SmokingStatusId"];
        //objData["RaceId"] = objData["RaceId"] == "" ? -1 : objData["RaceId"];
        //objData["EthnicityId"] = objData["EthnicityId"] == "" ? -1 : objData["EthnicityId"];
        //objData["PrefLanguageId"] = objData["PrefLanguageId"] == "" ? -1 : objData["PrefLanguageId"];
        //objData["PrefCommunicationId"] = objData["PrefCommunicationId"] == "" ? -1 : objData["PrefCommunicationId"];
        objData["PatientId"] = objData["PatientId"] == "" ? -1 : objData["PatientId"];
        //  objData["gender"] = objData["gender_text"];

        //
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = null;
        } else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["commandType"] = "SEARCH_PATIENT_LIST";

        objData["Problems"] = problemIds.get().join('|');
        objData["Medications"] = medicationText.get().join('|');
        objData["Allergies"] = allergyIds.get().join('|');
        objData["LabResults"] = labResultText.get().join('|');

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "BatchPatientList");
    },
    /*
  Author: Muhammad Azhar Shahzad
  Purpose: To export Excel of Grid Data DB Call
  Created on April 06, 2016*/
    printPatientList_DbCall: function (patientListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        //var self = $("#" + Batch_PatientList.params.PanelID);
        //var myJSON = self.getMyJSONByName();
        //myJSON = JSON.parse(myJSON);
        //var objData = myJSON;
        //var problemIds = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});

        //var medicationIds = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});

        //var allergyIds = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});
        //var labResultIds = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});

        //objData["PatientListId"] = patientListId == null ? -1 : patientListId;
        //objData["SmokingStatusId"] = objData["SmokingStatusId"] == "" ? -1 : objData["SmokingStatusId"];
        //objData["RaceId"] = objData["RaceId"] == "" ? -1 : objData["RaceId"];
        //objData["EthnicityId"] = objData["EthnicityId"] == "" ? -1 : objData["EthnicityId"];
        //objData["PrefLanguageId"] = objData["PrefLanguageId"] == "" ? -1 : objData["PrefLanguageId"];
        //objData["PrefCommunicationId"] = objData["PrefCommunicationId"] == "" ? -1 : objData["PrefCommunicationId"];
        //objData["PatientId"] = objData["PatientId"] == "" ? -1 : objData["PatientId"];
        //objData["gender"] = objData["gender_text"];


        //objData["PageNumber"] = PageNumber;
        //objData["RowsPerPage"] = RowsPerPage;
        //if (globalAppdata['AppUserName'] == DefaultUser) {
        //    objData["EntityId"] = null;
        //} else {
        //    objData["EntityId"] = globalAppdata["SeletedEntityId"];
        //}

        var self = $("#" + Batch_PatientList.params.PanelID);
        var myJSON = self.getMyJSONByName();
        myJSON = JSON.parse(myJSON);
        var objData = myJSON;
        var problemIds = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li").map(function () {
            return $.trim($(this).attr('id'));
        });

        var problemTexts = $("#" + Batch_PatientList.params.PanelID + " #ulProblems li").map(function () {
            return $(this).text();
        });

        //var medicationIds = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});

        //var allergyIds = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});
        //var labResultIds = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li").map(function () {
        //    return $.trim($(this).attr('id'));
        //});

        var medicationText = $("#" + Batch_PatientList.params.PanelID + " #ulMedications li").map(function () {
            return $(this).text();
        });

        var allergyText = $("#" + Batch_PatientList.params.PanelID + " #ulAllergies li").map(function () {
            return $(this).text();
        });
        var labResultText = $("#" + Batch_PatientList.params.PanelID + " #ulLabResults li").map(function () {
            return $(this).text();
        });


        //
        objData["gender"] = self.find('#ddlSex option:Selected').map(function () {
            return this.text;
        }).get().join(',');

        objData["SmokingStatusId"] = self.find('#' + Batch_PatientList.params.PanelID + ' #ddlSmokingStatus option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["RaceId"] = self.find('#ddlRace option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["EthnicityId"] = self.find('#ddlEthnicity option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["PrefLanguageId"] = self.find('#ddlPrefLanguage option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["PrefCommunicationId"] = self.find('#ddlPreferredComm option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        //

        objData["PatientListId"] = patientListId == null ? -1 : patientListId;
        //objData["SmokingStatusId"] = objData["SmokingStatusId"] == "" ? -1 : objData["SmokingStatusId"];
        //objData["RaceId"] = objData["RaceId"] == "" ? -1 : objData["RaceId"];
        //objData["EthnicityId"] = objData["EthnicityId"] == "" ? -1 : objData["EthnicityId"];
        //objData["PrefLanguageId"] = objData["PrefLanguageId"] == "" ? -1 : objData["PrefLanguageId"];
        //objData["PrefCommunicationId"] = objData["PrefCommunicationId"] == "" ? -1 : objData["PrefCommunicationId"];
        objData["PatientId"] = objData["PatientId"] == "" ? -1 : objData["PatientId"];
        //  objData["gender"] = objData["gender_text"];

        //
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = null;
        } else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["commandType"] = "print_patient_list";

        objData["Problems"] = problemTexts.get().join('|');
        objData["Medications"] = medicationText.get().join('|');
        objData["Allergies"] = allergyText.get().join('|');
        objData["LabResults"] = labResultText.get().join('|');


        objData["commandType"] = "print_patient_list";


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "BatchPatientList");
    },
    getFilteredGrid: function (cntrl) {
        var height = 0;
        if (cntrl) {
            var rex = new RegExp($(cntrl).val(), 'i');
            $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList tbody tr').hide();
            $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList tbody tr').filter(function () {
                return rex.test($(this).text());
            }).show();
        }
        if ($('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList tbody tr:visible').length == 0) {
            $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList tbody').append('<tr id="noResultFound" class="text-center"><td colspan="18"> No matching records found </td></tr>');
        } else {
            $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList tbody #noResultFound').remove();
        }
        $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList tbody tr:visible').each(function () {
            height += $(this).height()
        });

        //   if (height < 60) {
        $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList .ui-jqgrid-bdiv').css('height', (height + 30) + "px");
        //} else {
        //   $('#' + Batch_PatientList.params.PanelID + ' #gbox_dgvBatchPatientList .ui-jqgrid-bdiv').css('height', height + "px");
        //}
    },
    formateActions: function (cellvalue, options, rowObject) {

        if (rowObject.IsActive == "True") {
            isEventactive = 0;
            activeTitle = "Active Record";
            tglclass = "fa fa-toggle-on green";
        }
        else {
            isEventactive = 1;
            activeTitle = "Inactive Record";
            tglclass = "fa fa-toggle-on red";
        }
        return '<a class="btn btn-xs" href="javascript:void(0);" onclick="Batch_PatientList.PatientAddEdit(' + rowObject.PatientId + ',\'Edit\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>' +
                    '<a class="btn btn-xs" href="javascript:void(0);" onclick="Batch_PatientList.ActiveInactivePatient(' + rowObject.PatientId + ',' + isEventactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;' +
                    '<a class="btn btn-xs" href="javascript:void(0);" onclick="Batch_PatientList.SelectPatient(' + rowObject.PatientId + ',event);" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;';

    },
    Merger: function (gridpatient, Cellpatient) {
        var gridpatient = Batch_PatientList.params.PanelID + " #dgvBatchPatientList"
        var mya = $("#" + gridpatient + "").getDataIDs();

        var length = mya.length;
        for (var i = 0; i < length; i++) {

            var before = $("#" + gridpatient + "").jqGrid('getRowData', mya[i]);

            var rowSpanTaxCount = 1;
            for (j = i + 1; j <= length; j++) {

                var end = $("#" + gridpatient + "").jqGrid('getRowData', mya[j]);
                if (before[Cellpatient] == end[Cellpatient]) {
                    rowSpanTaxCount++;
                    $("#" + gridpatient + "").setCell(mya[j], Cellpatient, '', {
                        display: 'none'
                    });
                } else {
                    rowSpanTaxCount = 1;
                    break;
                }
                $("#" + Cellpatient + "" + mya[i] + "").attr("rowspan", rowSpanTaxCount);
            }
        }
    },

    UnLoad: function () {
        if (Batch_PatientList.params != null && Batch_PatientList.params.ParentCtrl != null) {
            UnloadActionPan(Batch_PatientList.params.ParentCtrl, 'Batch_PatientList');
        }
        else {
            RemoveAdminTab('batchTabPatientList');
        }
    },

    PatientAddEdit: function (patientId, mode, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        AppPrivileges.GetFormPrivileges("Demographic", mode.toUpperCase(), "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["mode"] = mode;
                params["patientID"] = patientId;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "batchTabPatientList";
                LoadActionPan('demographicDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    SelectPatient: function (PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $.when(SelectPatient(PatientId, "")).done(function () {
            $('#patTabDemographic').click();
            //Start//15-03-2016//Ahmad Raza//showing CDS ALert icon on patient selection
            $(" #mainForm  li#CDSAlert").show();
            if (globalAppdata.IsImmunizationAlert != "False") {
                //$(" #mainForm  li#ImmunizationAlert").show();
            }
            //End//15-03-2016//Ahmad Raza//showing CDS ALert icon on patient selection
        });

    },
    ActiveInactivePatient: function (patientId, IsActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvPatient_row" + patientId));
        utility.myConfirm('3', function () {
            var selectedValue = patientId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_Search.PatientUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Batch_PatientList.patientListSearch(patientId, 1, 30);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '3', null, null, null, IsActive);
    },
    PatientUpdateActiveInactive: function (patientID, IsActive) {
        var data = "PatientID=" + patientID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_SEARCH", "UPDATE_PATIENT_ACTIVE_INACTIVE");
    },
}
