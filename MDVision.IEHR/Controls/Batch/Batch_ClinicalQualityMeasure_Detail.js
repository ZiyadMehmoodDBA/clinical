Batch_ClinicalQualityMeasureDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Batch_ClinicalQualityMeasureDetail.params = params;



        if (Batch_ClinicalQualityMeasureDetail.bIsFirstLoad) {
            Batch_ClinicalQualityMeasureDetail.bIsFirstLoad = false;
            var self;
            if (Batch_ClinicalQualityMeasureDetail.params["PanelID"] != "pnlBatchClinicalQualityMeasureDetail")
                self = $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail");
            else
                self = $("#pnlBatchClinicalQualityMeasureDetail");

            if (Batch_ClinicalQualityMeasureDetail.params.CQMSearchData != null) {
                var CqmSearchData = JSON.parse(Batch_ClinicalQualityMeasureDetail.params.CQMSearchData);
                if (CqmSearchData.PatientId != "") {
                    $("#pnlBatchClinicalQualityMeasureDetail #txtPatientName").val(CqmSearchData.PatientAccountNumber);
                    $("#pnlBatchClinicalQualityMeasureDetail #hfPatientId").val(CqmSearchData.PatientId);
                }
            }

            self.loadDropDowns(true).done(function () {
                //utility.CreateDatePicker(Batch_ClinicalQualityMeasureDetail.params.PanelID + " #dpFrom, #dpTo", function () {
                //}, false);

                Batch_ClinicalQualityMeasureDetail.ClinicalQualityMeasuresSearch(Batch_ClinicalQualityMeasureDetail.params.CQMSearchData, Batch_ClinicalQualityMeasureDetail.params.CQMID, Batch_ClinicalQualityMeasureDetail.params.providerId, Batch_ClinicalQualityMeasureDetail.params.dateFrom, Batch_ClinicalQualityMeasureDetail.params.dateTo);

                $("#measureName").text(Batch_ClinicalQualityMeasureDetail.params.Measure);
                $("#dpFrom").prop('disabled', true);
                $("#dpFrom").datepicker("setDate", Batch_ClinicalQualityMeasureDetail.params.dateFrom);
                $("#dpTo").prop('disabled', true);
                $("#dpTo").datepicker("setDate", Batch_ClinicalQualityMeasureDetail.params.dateTo);

            });
        }
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Batch_ClinicalQualityMeasureDetail";
        LoadActionPan("Patient_Search", params);
    },
    BatchClinicalQualityMeasureExport: function () {
        Batch_ClinicalQualityMeasureDetail.BatchClinicalQualityMeasureExport_().done(function (response) {
            if (response.status != false) {
                download("data:application/octet-stream;base64," + response.DownloadFile, response.FileName + ".xml", "application/octet-stream");

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    BatchClinicalQualityMeasureExport_: function () {

        var cqmid = Batch_ClinicalQualityMeasureDetail.params["CQMID"];

        var providerId = Batch_ClinicalQualityMeasureDetail.params["providerId"];
        var appointmentDateFrom = Batch_ClinicalQualityMeasureDetail.params["dateFrom"];
        var appointmentDateTo = Batch_ClinicalQualityMeasureDetail.params["dateTo"];
        var batchClinicalQualityMeasureData = Batch_ClinicalQualityMeasureDetail.params["CQMSearchData"]
        var tmp = JSON.parse(batchClinicalQualityMeasureData);
        tmp["Sex_text"] = tmp["Sex_text"] == "- Select -" ? "" : tmp["Sex_text"];
        tmp["AgeCondition_text"] = tmp["AgeCondition_text"] == "- Select -" ? "" : tmp["AgeCondition_text"];
        tmp["ProviderTypeId_text"] = tmp["ProviderTypeId_text"] == "- Select -" ? "" : tmp["ProviderTypeId_text"];
        batchClinicalQualityMeasureData = JSON.stringify(tmp)

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo + "&FROM=" + "Detail";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURES");
    },

    ExportPatientData: function (mode, cqmid, patientId, providerId,isC1) {

        if ($("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result").css("display") === "none") {
            $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result").show();
        }

        var self = $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Search");
        var myJson = self.getMyJSON();

        if (cqmid == "" || cqmid == null)
            cqmid = Batch_ClinicalQualityMeasureDetail.params.CQMID;

        if (providerId == "" || providerId == null)
            providerId = Batch_ClinicalQualityMeasureDetail.params.providerId;

        Batch_ClinicalQualityMeasureDetail.ExportClinicalQualityMeasureDetail(myJson, cqmid, patientId, providerId, isC1);
    },

    BindPatientAccount: function (fromFill) {
        var accountNo = null;
        if (!fromFill)
            accountNo = $("#pnlBatchClinicalQualityMeasureDetail #txtPatientName").val();
        else
            accountNo = $("#pnlBatchClinicalQualityMeasureDetail #txtPatientName").val().split("-")[0].trim();
        utility.Keyupdelay(function () {
            utility.GetPatientArray(accountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.AccountNumber;
                });

                $("#pnlBatchClinicalQualityMeasureDetail #txtPatientName").autocomplete({
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) {
                        disable = true
                    },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#pnlBatchClinicalQualityMeasureDetail #txtPatientName").val(ui.item.AccountNumber);
                            $("#pnlBatchClinicalQualityMeasureDetail #hfPatientId").val(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                    }, 200);
                });;

                $("#pnlBatchClinicalQualityMeasureDetail #txtPatientName").autocomplete("search");
                $("#pnlBatchClinicalQualityMeasureDetail #txtPatientName").focus();
            });
        });
    },

    ClinicalQualityMeasuresSearch: function (CQMSearchData, cqmid, providerId, dateFrom, dateTo, pageNo, rpp) {
        if ($("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result").css("display") === "none") {
            $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result").show();
        }

        var self = $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Search");
        var myJson = self.getMyJSON();
        var objData = JSON.parse(myJson);
        objData["CQMSearchData"] = CQMSearchData;
        var myJson = JSON.stringify(objData);
        var objData = JSON.parse(myJson);
        objData["PatientId"] = $("#pnlBatchClinicalQualityMeasureDetail #hfPatientId").val() != -1 ? $("#pnlBatchClinicalQualityMeasureDetail #hfPatientId").val() : null;
        myJson = JSON.stringify(objData);

        Batch_ClinicalQualityMeasureDetail.SearchClinicalQualityMeasureDetail(myJson, cqmid, providerId, dateFrom, dateTo, pageNo, 100).done(function (response) {

            if (response.status != false) {

                Batch_ClinicalQualityMeasureDetail.ClinicalQualityMeasureDetailGridLoad(response);

                var tableControl = Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #dgvBatchClinicalQualityMeasureDetail";
                var pagingPanelControlId = Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #divClinicalQualityMeasurePaging";

                var classControlName = "Batch_ClinicalQualityMeasureDetail";

                var pagesToDisplay = 5;
                var iTotalDisplayRecords = Batch_ClinicalQualityMeasureDetail.params["iP"];// response.iTotalDisplayRecords;
                if (rpp === "undefined" || rpp == null || rpp == undefined) {
                    rpp = 100; //response.ClinicalQualityMeasureDetailCount;
                }
                setTimeout(CreatePagination(response.ClinicalQualityMeasureDetailCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords, function (primaryId, pageNumber, resultPerPage) {
                    Batch_ClinicalQualityMeasureDetail.SearchClinicalQualityMeasureDetail("", "", "", "", "", pageNumber, 100);
                }), 10);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ExportClinicalQualityMeasureDetail: function (myJson, cqmid, patientId, providerId, isC1) {
        if ($("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result").css("display") === "none") {
            $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result").show();
        }

        var self = $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Search");
        var myJson = self.getMyJSON();

        if ($("#dgvBatchClinicalQualityMeasureDetail tr").length > 1) {


            Batch_ClinicalQualityMeasureDetail.ExportClinicalQualityMeasureDetail_(myJson,'','','','','', isC1).done(function (response) {

                if (response.status != false) {
                    var zip = new JSZip();
                    for (var i = 0; i < response.DownloadFile.length; i++) {
                        zip.file(response.DownloadFile[i].Key + ".xml", response.DownloadFile[i].Value, { base64: true });
                    }

                    zip.generateAsync({ type: "blob" }).then(function (content) {
                        saveAs(content, response.FileName);
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            utility.DisplayMessages("No Record Found to Export.", 3);
        }
    },

    ClinicalQualityMeasureDetailGridLoad: function (response) {

        $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #dgvBatchClinicalQualityMeasureDetail").dataTable().fnDestroy();
        $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result #dgvBatchClinicalQualityMeasureDetail tbody").find("tr").remove();

        if (response.ClinicalQualityMeasureDetailCount > 0) {

            var clinicalQualityMeasureDetailLoadJsonData = JSON.parse(response.ClinicalQualityMeasureDetailLoad_JSON);
            $.each(clinicalQualityMeasureDetailLoadJsonData, function (i, item) {
                
                if (item.CQMID == "0421a") {
                    item.CQMID = "0421";
                }
                if (item.CQMID == response.CQMID) {
                    var $row = $("<tr/>");
                    var providerId = Batch_ClinicalQualityMeasureDetail.params["providerId"];

                    $row.attr("id", "gvClinicalQualityMeasureDetail_row" + response.CQMID);
                    //$row.attr("onclick", "Batch_ClinicalQualityMeasureDetail.ExportPatientData('Export','" + response.CQMID + "','" + item.PatientID + "','" + providerId + "',event);utility.SelectGridRow($(this));");

                    $row.attr("CQMID", response.CQMID);

                    //if (item.IsActive === "True") {
                    //    isactive = 0;
                    //    activeTitle = "Active Record";
                    //    tglclass = "fa fa-toggle-on green";
                    //}
                    //else {
                    //    isactive = 1;
                    //    activeTitle = "Inactive Record";
                    //    tglclass = "fa fa-toggle-on red";
                    //}

                    var initialPopulation = item.InitialPopulation;
                    var denominator = item.Denominator;
                    var numerator = item.Numerator;
                    var denominatorExclusion = item.DenominatorExclusion;
                    var denominatorException = item.DenominatorException;

                    if (initialPopulation <= 0)
                        initialPopulation = "<i class=\"fa fa-close red\"></i>";
                    else
                        initialPopulation = "<i class=\"fa fa-check black\"></i>";

                    if (denominator <= 0)
                        denominator = "<i class=\"fa fa-close red\"></i>";
                    else
                        denominator = "<i class=\"fa fa-check black\"></i>";

                    if (numerator <= 0)
                        numerator = "<i class=\"fa fa-close red\"></i>";
                    else
                        numerator = "<i class=\"fa fa-check black\"></i>";

                    if (denominatorExclusion <= 0)
                        denominatorExclusion = "<i class=\"fa fa-close red\"></i>";
                    else
                        denominatorExclusion = "<i class=\"fa fa-check black\"></i>";

                    if (denominatorException <= 0)
                        denominatorException = "<i class=\"fa fa-close red\"></i>";
                    else
                        denominatorException = "<i class=\"fa fa-check black\"></i>";

                    $row.append(
                        "<td style=\"display:none;\">" + response.CQMID + "</td>" +
                        "<td><a href=\"#\" onclick=\"Batch_ClinicalQualityMeasureDetail.PatientDemographics(" + item.PatientID + ",event);\"  title=\"View Patient\">" + item.AccountNumber + "</a></td>" +
                        "<td>" + item.LastName + "</td>" +
                        "<td>" + item.FirstName + "</td>" +
                        "<td>" + initialPopulation + "</td>" +
                        "<td>" + denominator + "</td>" +
                        "<td>" + numerator + "</td>" +
                        "<td>" + denominatorExclusion + "</td>" +
                        "<td>" + denominatorException + "</td>");

                    $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result #dgvBatchClinicalQualityMeasureDetail tbody").last().append($row);

                }
            });

        }
        else {
            $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #dgvBatchClinicalQualityMeasureDetail").DataTable({
                "language": {
                    "emptyTable": "No patient against the Measure"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #dgvBatchClinicalQualityMeasureDetail"))
            ;
        else
            $("#" + Batch_ClinicalQualityMeasureDetail.params["PanelID"] + " #pnlBatchClinicalQualityMeasureDetail_Result #dgvBatchClinicalQualityMeasureDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    PatientDemographics: function (patientid, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["mode"] = "Edit";
        params["PatBanner"] = true;
        params["patientID"] = patientid;
        params["IsFill"] = false;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Batch_ClinicalQualityMeasureDetail";
        LoadActionPan("demographicDetail", params);
    },

    SearchClinicalQualityMeasureDetail: function (clinicalQualityMeasureDetailData, cqmid, providerId, dateFrom, dateTo, pageNumber, rowsPerPage) {

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = Batch_ClinicalQualityMeasureDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = Batch_ClinicalQualityMeasureDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = Batch_ClinicalQualityMeasureDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = Batch_ClinicalQualityMeasureDetail.params.dateTo;

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var data = "ClinicalQualityMeasureDetailData=" + clinicalQualityMeasureDetailData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_CQM_MEASURE_DETAILS");
    },

    ExportClinicalQualityMeasureDetail_: function (clinicalQualityMeasureDetailData, cqmid, patientId, providerId, dateFrom, dateTo,isC1) {

        if (patientId == null || patientId === "" || patientId === "undefined")
            patientId = Batch_ClinicalQualityMeasureDetail.params.PatientId;

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = Batch_ClinicalQualityMeasureDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = Batch_ClinicalQualityMeasureDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = Batch_ClinicalQualityMeasureDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = Batch_ClinicalQualityMeasureDetail.params.dateTo;

        var data = "ClinicalQualityMeasureDetailData=" + clinicalQualityMeasureDetailData + "&PatientId=" + patientId + "&providerId=" + providerId + "&CQMID=" + cqmid + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&isC1=" + isC1;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURE_DETAILS");
    },

    UnLoadTab: function () {
        UnloadActionPan(null, "Batch_ClinicalQualityMeasureDetail");
    },

    FillPatientInfoFromSearch: function (patientId, accountNumber, fullname, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = null;
        self = $("#pnlBatchClinicalQualityMeasureDetail");
        self.find("#hfPatientId").val(patientId);
        self.find("#txtPatientName").val(accountNumber);

        $Ctrl = $("#" + Batch_ClinicalQualityMeasureDetail.params.PanelID + " #txtPatientName");
        $hfCtrl = $("#" + Batch_ClinicalQualityMeasureDetail.params.PanelID + " #hfPatientId");
        //Patient
        utility.SetAutoCompleteSource($Ctrl, $hfCtrl);

        //$("#txtPatientName").val(accountNumber);
        UnloadActionPan("Batch_ClinicalQualityMeasureDetail");

    }
}