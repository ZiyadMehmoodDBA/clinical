Immunization_QueryResponseDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Immunization_QueryResponseDetail.params = params;
        Immunization_QueryResponseDetail.params.AddHXTabIds = [];
        if (Immunization_QueryResponseDetail.params.PanelID != 'pnlImmunization_QueryResponseDetail') {
            Immunization_QueryResponseDetail.params.PanelID = Immunization_QueryResponseDetail.params.PanelID + ' #pnlImmunization_QueryResponseDetail';
        } else {
            Immunization_QueryResponseDetail.params.PanelID = 'pnlImmunization_QueryResponseDetail';
        }
        Immunization_QueryResponseDetail.FillQueryResponseDetail(Immunization_QueryResponseDetail.params.ImmunizationQueryResponseId,1,15);
    },
    FillQueryResponseDetail: function (ImmunizationQueryResponseId,PageNo,RowPerPage) {
        Immunization_QueryResponseDetail.LoadImmunizationQueryResponsePatientdetail(ImmunizationQueryResponseId);
        Immunization_QueryResponseDetail.LoadImmunizationQueryResponseHXdetail(ImmunizationQueryResponseId, PageNo, RowPerPage);
        Immunization_QueryResponseDetail.LoadImmunizationQueryResponseForecastdetail(ImmunizationQueryResponseId, PageNo, RowPerPage);
    },
    LoadImmunizationQueryResponsePatientdetail: function (ImmunizationQueryResponseId) {
        Immunization_QueryResponseDetail.LoadImmunizationQueryResponsePatientdetail_DBCALL(ImmunizationQueryResponseId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Immunization_QueryResponseDetail.ImmQueryResponsePatientGridLoad(response);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }





        });
    },
    LoadImmunizationQueryResponseHXdetail: function (ImmunizationQueryResponseId, PageNo, RowPerPage) {
        Immunization_QueryResponseDetail.params.AddHXTabIds = [];
        Immunization_QueryResponseDetail.LoadImmunizationQueryResponseHXdetail_DBCALL(ImmunizationQueryResponseId, PageNo, RowPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Immunization_QueryResponseDetail.ImmQueryResponseHXGridLoad(response)).then(function () {
                    if (response.ImmQueryResponseHXCount > 0) {
                        var TableControl = Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory";
                        var PagingPanelControlID = Immunization_QueryResponseDetail.params.PanelID + " #divQueryResonse_EvaluatedHistory_Paging";
                        var ClassControlName = "Immunization_QueryResponseDetail";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.ImmQueryResponseHX_JSON[0].RecordCount;
                        CreatePagination(response.ImmQueryResponseHXCount, PageNo, RowPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (ImmunizationQueryResponseId, PageNo, RowPerPage) {
                            Immunization_QueryResponseDetail.LoadImmunizationQueryResponseHXdetail(Immunization_QueryResponseDetail.params.ImmunizationQueryResponseId, PageNo, RowPerPage);
                        });
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    LoadImmunizationQueryResponseForecastdetail: function (ImmunizationQueryResponseId, PageNo, RowPerPage) {
        Immunization_QueryResponseDetail.LoadImmunizationQueryResponseForecastdetail_DBCALL(ImmunizationQueryResponseId, PageNo, RowPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $.when(Immunization_QueryResponseDetail.ImmQueryResponseForecastGridLoad(response)).then(function () {
                    if (response.ImmQueryResponseForecastCount > 0) {
                        var TableControl = Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedForecast #dgvQueryResonse_EvaluatedForecast";
                        var PagingPanelControlID = Immunization_QueryResponseDetail.params.PanelID + " #divQueryResonse_EvaluatedForecast_Paging";
                        var ClassControlName = "Immunization_QueryResponseDetail";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.ImmQueryResponseForecast_JSON[0].RecordCount;
                        CreatePagination(response.ImmQueryResponseForecastCount, PageNo, RowPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (ImmunizationQueryResponseId, PageNo, RowPerPage) {
                            Immunization_QueryResponseDetail.LoadImmunizationQueryResponseForecastdetail(Immunization_QueryResponseDetail.params.ImmunizationQueryResponseId, PageNo, RowPerPage);
                        });
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    //Immunization_QueryResponseHxLoad: function (ImmunizationQueryResponseId, PageNumber, ResultPerPage) {

    //    Immunization_QueryResponseDetail.Immunization_QueryResponseHxLoad_DBCALL(ImmunizationQueryResponseId).done(function (response) {
    //        response = JSON.parse(response);
    //        if (response.status != false) {
    //            Immunization_QueryResponseDetail.ImmQueryResponsePatientGridLoad(response);

    //            $.when(Immunization_QueryResponseDetail.ImmQueryResponseHXGridLoad(response)).then(function () {
    //                if (response.ImmQueryResponseHXCount > 0) {
    //                    var TableControl = Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory";
    //                    var PagingPanelControlID = Immunization_QueryResponseDetail.params.PanelID + " #divQueryResonse_EvaluatedHistory_Paging";
    //                    var ClassControlName = "Immunization_QueryResponseDetail";
    //                    var PagesToDisplay = 5;
    //                    var iTotalDisplayRecords = response.ImmQueryResponseHX_JSON[0].RecordCount;
    //                    setTimeout(CreatePagination(response.ImmQueryResponseHXCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (ImmunizationQueryResponseId, PageNumber, ResultPerPage) {
    //                        Immunization_QueryResponseDetail.ImmunizationQueryResponseSearch(ImmunizationQueryResponseId, PageNumber, ResultPerPage);
    //                    }), 10);
    //                }
    //            });
    //        }
    //        else {
    //            utility.DisplayMessages(response.Message, 3);
    //        }


    //    });
    //},
    
    LoadImmunizationQueryResponsePatientdetail_DBCALL: function (ImmunizationQueryResponseId) {

        var objData = new Object();
        objData["commandType"] = "Search_QueryResponsePatient";
        objData["ImmunizationQueryResponseId"] = ImmunizationQueryResponseId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    LoadImmunizationQueryResponseHXdetail_DBCALL: function (ImmunizationQueryResponseId, PageNo, RowPerPage) {

        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "Search_QueryResponsePatient";
        objData["ImmunizationQueryResponseId"] = ImmunizationQueryResponseId;
        if (PageNo != null) {
            objData["PageNumber"] = PageNo;
        }
        else {
            objData["PageNumber"] = 1;
        }
        if (RowPerPage != null) {
            objData["RowsPerPage"] = RowPerPage;
        }
        else {
            objData["RowsPerPage"] = 15;
        }
        objData["commandType"] = "search_queryresponsehx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    LoadImmunizationQueryResponseForecastdetail_DBCALL: function (ImmunizationQueryResponseId, PageNo, RowPerPage) {

        var objData = new Object();
        objData["commandType"] = "Search_QueryResponsePatient";
        objData["ImmunizationQueryResponseId"] = ImmunizationQueryResponseId;
        if (PageNo != null) {
            objData["PageNumber"] = PageNo;
        }
        else {
            objData["PageNumber"] = 1;
        }
        if (RowPerPage != null) {
            objData["RowsPerPage"] = RowPerPage;
        }
        else {
            objData["RowsPerPage"] = 15;
        }
        objData["commandType"] = "search_queryresponseforecast";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    Immunization_QueryResponseHxLoad_DBCALL: function (ImmunizationQueryResponseId) {

        var objData = new Object();

        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();

        objData["commandType"] = "Search_QueryResponseHX";
        objData["ImmunizationQueryResponseId"] = ImmunizationQueryResponseId;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    ImmQueryResponsePatientGridLoad: function (response) {
        $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_Patient #dgvQueryResonse_Patient").dataTable().fnDestroy();
        $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_Patient #dgvQueryResonse_Patient tbody").find("tr").remove();
        var ImmQueryResponsePatient_JSON = response.ImmQueryResponsePatient_JSON;
        if (response.ImmQueryResponsePatientCount > 0) {
            $.each(ImmQueryResponsePatient_JSON, function (i, item) {
                
                var $row = $('<tr/>');
                $row.attr("id", item.ImmunizationQueryResponseId);
                $row.append('<td style="display:none">' + item.ImmunizationQueryResponseId + '</td><td>' + item.PatientName + '</td><td>' + item.DOB + '</td><td >' + item.Gender + '</td><td>' + item.MothersMaidenName + '</td><td>'+item.Address+'</td><td>'+item.Comments+'</td>');
                $("#" + Immunization_QueryResponseDetail.params.PanelID + " #dgvQueryResonse_Patient tbody").append($row);
            });


            var ImmQueryPatientRows = $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_Patient #dgvQueryResonse_Patient tbody").find("tr");

            if (ImmQueryPatientRows.length < 1) {
                $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_Patient #dgvQueryResonse_Patient").DataTable({
                    "language": {
                        "emptyTable": "No Patient Information Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }, { "targets": [7], "visible": false }]
                });
            }
        }
        else {

            $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_Patient #dgvQueryResonse_Patient").DataTable({
                "language": {
                    "emptyTable": "No Patient Information Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },
    ImmQueryResponseHXGridLoad: function (response) {
        var dfd = $.Deferred();
        $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory").dataTable().fnDestroy();
        $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory tbody").find("tr").remove();
        var ImmQueryResponseHX_JSON = response.ImmQueryResponseHX_JSON;
        if (response.ImmQueryResponseHXCount > 0) {
            $.each(ImmQueryResponseHX_JSON, function (i, item) {

                var $row = $('<tr/>');
                $row.attr("id", item.EvaluatedImmunizationHistoryId);
                var warning = "";
                var classdisabled = "";
                if(item.AlreadyExists=="1"){
                    warning = ' <i class="fa fa-exclamation-triangle text-warning"></i>';
                    $row.attr("title", "Record already exists in the application!");
                    classdisabled = "disabled";
                }
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Immunization_QueryResponseDetail.AddInArray(this);" id="' + item.EvaluatedImmunizationHistoryId + '" name="SelectCheckBoxEvaluatedImmunizationHistory" class="input-block text-center" ' + classdisabled + '/></td>';
                $row.append('<td style="display:none">' + item.EvaluatedImmunizationHistoryId + '</td>'+SelectionCheckBoxColumn+'<td>' + item.VaccineGroupDescription + warning + '</td><td>' + item.AdministrationDate + '</td><td >' + item.VaccineDescription + '</td><td>' + item.ValidDose + '</td><td>' + item.ValidityReason + '</td><td>' + item.CompletionStatus + '</td><td>' + item.Reaction + '</td><td>' + item.Comments + '</td>');
                $("#" + Immunization_QueryResponseDetail.params.PanelID + " #dgvQueryResonse_EvaluatedHistory tbody").append($row);
                    });
            var ImmQueryHxRows = $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory tbody").find("tr");
            if (ImmQueryHxRows.length > 0) {
                var IsDataTable = $.fn.dataTable.isDataTable('#pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory')
                if (!IsDataTable) {
                    $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
                }
            }
        }
        else {
            $("#" + Immunization_QueryResponseDetail.params.PanelID + " #divQueryResonse_EvaluatedHistory_Paging").css("display", "none");
            $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedHistory #dgvQueryResonse_EvaluatedHistory").DataTable({
                "language": {
                    "emptyTable": "No Evaluated Immunization History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        dfd.resolve();
        return dfd;
    },
    AddInArray: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Immunization_QueryResponseDetail.params.AddHXTabIds) == -1) {
                Immunization_QueryResponseDetail.params.AddHXTabIds.push(obj.id);
            } 
        } else {
            var index = Immunization_QueryResponseDetail.params.AddHXTabIds.indexOf(obj.id);
            if (index > -1) {
                Immunization_QueryResponseDetail.params.AddHXTabIds.splice(index, 1);
            }
        }
    },
    AddToHxTab: function () {
        if (Immunization_QueryResponseDetail.params.AddHXTabIds.length > 0) {
            Immunization_QueryResponseDetail.AddToHxTab_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_QueryResponseDetail.LoadImmunizationQueryResponseHXdetail(Immunization_QueryResponseDetail.params.ImmunizationQueryResponseId, 1, 15);
                    //Clinical_OrderSetDetails.MedicationSearch();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        
        else {
            utility.DisplayMessages("Select at least one Evaluated History", 2);
        }
    },
    AddToHxTab_DBCall: function () {
        var objData = {};
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["EvaluatedImmunizationHistoryIds"] = Immunization_QueryResponseDetail.params.AddHXTabIds.join(',');
        objData["commandType"] = "AddToHxTab";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    ImmQueryResponseForecastGridLoad: function (response) {
        var dfd = $.Deferred();
        $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedForecast #dgvQueryResonse_EvaluatedForecast").dataTable().fnDestroy();
        $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedForecast #dgvQueryResonse_EvaluatedForecast tbody").find("tr").remove();
        var ImmQueryResponseForecast_JSON = response.ImmQueryResponseForecast_JSON;
        if (response.ImmQueryResponseForecastCount > 0) {
            $.each(ImmQueryResponseForecast_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.ImmunizationForecastId);
                $row.append('<td style="display:none">' + item.ImmunizationForecastId + '</td><td>' + item.VaccineGroupDescription + '</td><td>' + item.DueDate + '</td><td >' + item.EarliestDateToGive + '</td><td>' + item.LatestDateToGive + '</td><td>' + item.Comments + '</td>');
                $("#" + Immunization_QueryResponseDetail.params.PanelID + " #dgvQueryResonse_EvaluatedForecast tbody").append($row);
            });


            var ImmQueryHxRows = $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedForecast #dgvQueryResonse_EvaluatedForecast tbody").find("tr");
            if (ImmQueryHxRows.length > 0) {
                var IsDataTable = $.fn.dataTable.isDataTable('#pnlQueryResonse_EvaluatedForecast #dgvQueryResonse_EvaluatedForecast')
                if (!IsDataTable) {
                    $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedForecast #dgvQueryResonse_EvaluatedForecast").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
                }
            }
        }
        else {
            
            $("#" + Immunization_QueryResponseDetail.params.PanelID + " #divQueryResonse_EvaluatedForecast_Paging").css("display", "none");
            $("#" + Immunization_QueryResponseDetail.params.PanelID + " #pnlQueryResonse_EvaluatedForecast #dgvQueryResonse_EvaluatedForecast").DataTable({
                "language": {
                    "emptyTable": "No Immunization Forecast Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        dfd.resolve();
        return dfd;
    },
    UnLoad: function () {
        if (Immunization_QueryResponseDetail.params != null && Immunization_QueryResponseDetail.params.ParentCtrl) {
            if (Immunization_QueryResponseDetail.params.ParentCtrl == 'clinicalTabImmunization') {
                UnloadActionPan(Immunization_QueryResponseDetail.params["ParentCtrl"], "Immunization_QueryResponseDetail");
            } else {
                Immunization_QueryResponseDetail.params.PanelID = Immunization_QueryResponseDetail.params.PanelID.replace(" #pnlImmunization_QueryResponseDetail", "");
                UnloadActionPan(Immunization_QueryResponseDetail.params.ParentCtrl, 'Immunization_QueryResponseDetail', null, Immunization_QueryResponseDetail.params.PanelID);
            }
        }
        else {
            UnloadActionPan();
        }
    },
}