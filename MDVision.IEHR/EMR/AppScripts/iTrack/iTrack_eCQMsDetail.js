iTrack_eCQMsDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        iTrack_eCQMsDetail.params = params;

        if (iTrack_eCQMsDetail.bIsFirstLoad) {
            iTrack_eCQMsDetail.bIsFirstLoad = false;
            var self;
            if (iTrack_eCQMsDetail.params["PanelID"] != "pnliTrackeCQMsDetail")
                self = $("#pnliTrackeCQMsDetail");
            else
                self = $("#pnliTrackeCQMsDetail");

            if (iTrack_eCQMsDetail.params.CQMSearchData != null) {
                var CqmSearchData = JSON.parse(iTrack_eCQMsDetail.params.CQMSearchData);
                if (CqmSearchData.PatientId != "") {
                    $("#pnliTrackeCQMsDetail #txtPatientName").val(CqmSearchData.PatientAccountNumber);
                    $("#pnliTrackeCQMsDetail #hfPatientId").val(CqmSearchData.PatientId);
                }
            }

            self.loadDropDowns(true).done(function () {

                iTrack_eCQMsDetail.ClinicalQualityMeasuresSearch(iTrack_eCQMsDetail.params.CQMSearchData, iTrack_eCQMsDetail.params.CQMID, iTrack_eCQMsDetail.params.providerId, iTrack_eCQMsDetail.params.dateFrom, iTrack_eCQMsDetail.params.dateTo);

                $("#measureName").text(iTrack_eCQMsDetail.params.Measure);
                $("#btnMeasureDocument").append(" How to " + iTrack_eCQMsDetail.params.Measure);
                $("#dpFrom").prop('disabled', true);
                $("#dpFrom").datepicker("setDate", iTrack_eCQMsDetail.params.dateFrom);
                $("#dpTo").prop('disabled', true);
                $("#dpTo").datepicker("setDate", iTrack_eCQMsDetail.params.dateTo);

            });
        }
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "iTrack_eCQMsDetail";
        LoadActionPan("Patient_Search", params);
    },
    GetMeasurePDF: function () {
        iTrack_eCQMsDetail.LoadMeasurePDF().done(function (response) {
            if (response.status != false) {
                var base64String = response.pdfHelperBase64;
                download("data:application/octet-stream;base64," + base64String, response.FileName, "application/octet-stream");
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    LoadMeasurePDF: function () {
        var measureId = iTrack_eCQMsDetail.params.MeasureNumber;
        if (measureId == "CMS160v5(A)" || measureId == "CMS160v5(B)" || measureId == "CMS160v5(C)") {
            measureId = "CMS160v5";
        } else if (measureId == "CMS156v5(A)" || measureId == "CMS156v5(B)") {
            measureId = "CMS156v5";
        } else if (measureId == "CMS182v6(A)" || measureId == "CMS182v6(B)") {
            measureId = "CMS182v6";
        }
        else if (measureId.indexOf('(') > -1) {
            measureId = measureId.substring(0, measureId.indexOf('('));
        }
        var data = "fileName=" + measureId + ".pdf";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "GET_MEASURE_DOCUMENT");
    },
    BatchClinicalQualityMeasureExport: function () {
        iTrack_eCQMsDetail.BatchClinicalQualityMeasureExport_().done(function (response) {
            if (response.status != false) {
                download("data:application/octet-stream;base64," + response.DownloadFile, response.FileName + ".xml", "application/octet-stream");

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    BatchClinicalQualityMeasureExport_: function () {

        var cqmid = iTrack_eCQMsDetail.params["CQMID"];

        var providerId = iTrack_eCQMsDetail.params["providerId"];
        var appointmentDateFrom = iTrack_eCQMsDetail.params["dateFrom"];
        var appointmentDateTo = iTrack_eCQMsDetail.params["dateTo"];
        var batchClinicalQualityMeasureData = iTrack_eCQMsDetail.params["CQMSearchData"]
        var tmp = JSON.parse(batchClinicalQualityMeasureData);
        tmp["Sex_text"] = tmp["Sex_text"] == "- Select -" ? "" : tmp["Sex_text"];
        tmp["AgeCondition_text"] = tmp["AgeCondition_text"] == "- Select -" ? "" : tmp["AgeCondition_text"];
        tmp["ProviderTypeId_text"] = tmp["ProviderTypeId_text"] == "- Select -" ? "" : tmp["ProviderTypeId_text"];
        batchClinicalQualityMeasureData = JSON.stringify(tmp)

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo + "&FROM=" + "Detail";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURES");
    },

    ExportPatientData: function (mode, cqmid, patientId, providerId, isC1) {

        if ($("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result").css("display") === "none") {
            $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result").show();
        }

        var self = $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Search");
        var myJson = self.getMyJSON();

        if (cqmid == "" || cqmid == null)
            cqmid = iTrack_eCQMsDetail.params.CQMID;

        if (providerId == "" || providerId == null)
            providerId = iTrack_eCQMsDetail.params.providerId;

        iTrack_eCQMsDetail.ExportClinicalQualityMeasureDetail(myJson, cqmid, patientId, providerId, isC1);
    },

    BindPatientAccount: function (fromFill) {
        var accountNo = null;
        if (!fromFill)
            accountNo = $("#pnliTrackeCQMsDetail #txtPatientName").val();
        else
            accountNo = $("#pnliTrackeCQMsDetail #txtPatientName").val().split("-")[0].trim();
        utility.Keyupdelay(function () {
            utility.GetPatientArray(accountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.AccountNumber;
                });

                $("#pnliTrackeCQMsDetail #txtPatientName").autocomplete({
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
                            $("#pnliTrackeCQMsDetail #txtPatientName").val(ui.item.AccountNumber);
                            $("#pnliTrackeCQMsDetail #hfPatientId").val(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                    }, 200);
                });;

                $("#pnliTrackeCQMsDetail #txtPatientName").autocomplete("search");
                $("#pnliTrackeCQMsDetail #txtPatientName").focus();
            });
        });
    },

    ClinicalQualityMeasuresSearch: function (CQMSearchData, cqmid, providerId, dateFrom, dateTo, pageNo, rpp) {
        if ($("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result").css("display") === "none") {
            $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result").show();
        }

        var self = $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Search");
        var myJson = self.getMyJSON();
        var objData = JSON.parse(myJson);
        objData["CQMSearchData"] = CQMSearchData;
        var myJson = JSON.stringify(objData);
        var objData = JSON.parse(myJson);
        objData["PatientId"] = $("#pnliTrackeCQMsDetail #hfPatientId").val() != -1 ? $("#pnliTrackeCQMsDetail #hfPatientId").val() : null;
        myJson = JSON.stringify(objData);

        iTrack_eCQMsDetail.SearchClinicalQualityMeasureDetail(myJson, cqmid, providerId, dateFrom, dateTo, pageNo, 100).done(function (response) {

            if (response.status != false) {

                iTrack_eCQMsDetail.ClinicalQualityMeasureDetailGridLoad(response);

                var tableControl = iTrack_eCQMsDetail.params["PanelID"] + " #dgviTrackeCQMsDetail";
                var pagingPanelControlId = iTrack_eCQMsDetail.params["PanelID"] + " #diviTrackeCQMsDetailPaging";

                var classControlName = "iTrack_eCQMsDetail";

                var pagesToDisplay = 5;
                var iTotalDisplayRecords = iTrack_eCQMsDetail.params["iP"];// response.iTotalDisplayRecords;
                if (rpp === "undefined" || rpp == null || rpp == undefined) {
                    rpp = 100; //response.ClinicalQualityMeasureDetailCount;
                }
                //setTimeout(CreatePagination(response.ClinicalQualityMeasureDetailCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords, function (primaryId, pageNumber, resultPerPage) {
                //    iTrack_eCQMsDetail.SearchClinicalQualityMeasureDetail("", "", "", "", "", pageNumber, 100);
                //}), 10);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ExportClinicalQualityMeasureDetail: function (myJson, cqmid, patientId, providerId, isC1) {
        if ($("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result").css("display") === "none") {
            $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result").show();
        }

        var self = $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Search");
        var myJson = self.getMyJSON();

        if ($("#dgviTrackeCQMsDetail tr").length > 1) {


            iTrack_eCQMsDetail.ExportClinicalQualityMeasureDetail_(myJson, '', '', '', '', '', isC1).done(function (response) {

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

        $("#pnliTrackeCQMsDetail #dgviTrackeCQMsDetail").dataTable().fnDestroy();
        $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result #dgviTrackeCQMsDetail tbody").find("tr").remove();

        if (response.ClinicalQualityMeasureDetailCount > 0) {

            var clinicalQualityMeasureDetailLoadJsonData = JSON.parse(response.ClinicalQualityMeasureDetailLoad_JSON);
            $.each(clinicalQualityMeasureDetailLoadJsonData, function (i, item) {

                if (item.CQMID == "0421a") {
                    item.CQMID = "0421";
                }
                if (item.CQMID == response.CQMID) {
                    var $row = $("<tr/>");
                    var providerId = iTrack_eCQMsDetail.params["providerId"];

                    $row.attr("id", "gviTrackeCQMsDetail_row" + response.CQMID);
                    $row.attr("onclick", "iTrack_eCQMsDetail.PatientDemographics(" + item.PatientID + ",event);");
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
                        "<td><a href=\"#\" onclick=\"iTrack_eCQMsDetail.PatientDemographics(" + item.PatientID + ",event);\"  title=\"View Patient\">" + item.AccountNumber + "</a></td>" +
                        "<td>" + item.LastName + "</td>" +
                        "<td>" + item.FirstName + "</td>" +
                        "<td>" + initialPopulation + "</td>" +
                        "<td>" + denominator + "</td>" +
                        "<td>" + numerator + "</td>" +
                        "<td>" + denominatorExclusion + "</td>" +
                        "<td>" + denominatorException + "</td>");

                    $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result #dgviTrackeCQMsDetail tbody").last().append($row);

                }
            });

        }
        else {
            $("#pnliTrackeCQMsDetail #dgviTrackeCQMsDetail").DataTable({
                "language": {
                    "emptyTable": "No patient against the Measure"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#pnliTrackeCQMsDetail #dgviTrackeCQMsDetail"))
            ;
        else
            $("#pnliTrackeCQMsDetail #pnliTrackeCQMsDetail_Result #dgviTrackeCQMsDetail").DataTable({ "bInfo": true, "bPaginate": true, "pageLength": 30, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
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
        params["ParentCtrl"] = "iTrack_eCQMsDetail";
        LoadActionPan("demographicDetail", params);
    },

    SearchClinicalQualityMeasureDetail: function (clinicalQualityMeasureDetailData, cqmid, providerId, dateFrom, dateTo, pageNumber, rowsPerPage) {

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = iTrack_eCQMsDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = iTrack_eCQMsDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = iTrack_eCQMsDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = iTrack_eCQMsDetail.params.dateTo;

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var data = "ClinicalQualityMeasureDetailData=" + clinicalQualityMeasureDetailData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_CQM_MEASURE_DETAILS");
    },

    ExportClinicalQualityMeasureDetail_: function (clinicalQualityMeasureDetailData, cqmid, patientId, providerId, dateFrom, dateTo, isC1) {

        if (patientId == null || patientId === "" || patientId === "undefined")
            patientId = iTrack_eCQMsDetail.params.PatientId;

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = iTrack_eCQMsDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = iTrack_eCQMsDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = iTrack_eCQMsDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = iTrack_eCQMsDetail.params.dateTo;

        var data = "ClinicalQualityMeasureDetailData=" + clinicalQualityMeasureDetailData + "&PatientId=" + patientId + "&providerId=" + providerId + "&CQMID=" + cqmid + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&isC1=" + isC1;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURE_DETAILS");
    },

    UnLoadTab: function () {
        UnloadActionPan(iTrack_eCQMsDetail.params["ParentCtrl"], "iTrack_eCQMsDetail");
    },
    ExportExcelData: function (e) {

        var JSONData = [];
        $("#pnliTrackeCQMsDetail #dgviTrackeCQMsDetail tbody tr").each(function () {

            var AccountNumber = $(this).find("#AccountNumber").get(0);
            AccountNumber = $(AccountNumber).text();
            var IP = $(this).find("#InitialPopulation").get(0);
            IP = $(IP).find('i').hasClass('black') ? 1 : 0;
            var Denominator = $(this).find("#Denominator").get(0);
            Denominator = $(Denominator).find('i').hasClass('black') ? 1 : 0;
            var Numerator = $(this).find("#Numerator").get(0);
            Numerator = $(Numerator).find('i').hasClass('black') ? 1 : 0;
            var Exclusion = $(this).find("#Exclusion").get(0);
            Exclusion = $(Exclusion).find('i').hasClass('black') ? 1 : 0;
            var Exception = $(this).find("#Exception").get(0);
            Exception = $(Exception).find('i').hasClass('black') ? 1 : 0;
            var obj = {
                AccountNumber: AccountNumber,
                FirstName: $(this).find("#FirstName").text().trim(),
                LastName: $(this).find("#LastName").text().trim(),
                InitialPopulation: IP,
                Denominator: Denominator,
                Numerator: Numerator,
                Exclusion: Exclusion,
                Exception: Exception,

            }
            JSONData.push(obj);

        });
        iTrack_eCQMsDetail.ExportDataToExcel(JSONData);
    },
    ExportDataToExcel: function (JSONData) {

        var ReportTitle = iTrack_eCQMsDetail.params.Measure;
        var ShowLabel = true;
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        CSV += ReportTitle + '\r\n\n';
        if (ShowLabel) {
            var row = "";
            for (var index in arrData[0]) {

                row += index + ',';
            }
            row = row.slice(0, -1);
            CSV += row + '\r\n';
        }

        for (var i = 0; i < arrData.length; i++) {
            var row = "";
            for (var index in arrData[i]) {
                row += '"' + arrData[i][index] + '",';
            }
            row.slice(0, row.length - 1);
            CSV += row + '\r\n';
        }

        if (CSV == '') {
            alert("Invalid data");
            return;
        }
        var fileName = "";
        fileName += ReportTitle.replace(/ /g, "_");
        var csvData = new Blob([CSV], { type: 'text/csv' }); //new way
        var csvUrl = URL.createObjectURL(csvData);
        var link = document.createElement("a");
        link.href = csvUrl;
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    },
    FillPatientInfoFromSearch: function (patientId, accountNumber, fullname, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = null;
        self = $("#pnliTrackeCQMsDetail");
        self.find("#hfPatientId").val(patientId);
        self.find("#txtPatientName").val(accountNumber);

        $Ctrl = $("#" + iTrack_eCQMsDetail.params.PanelID + " #txtPatientName");
        $hfCtrl = $("#" + iTrack_eCQMsDetail.params.PanelID + " #hfPatientId");
        //Patient
        utility.SetAutoCompleteSource($Ctrl, $hfCtrl);

        //$("#txtPatientName").val(accountNumber);
        UnloadActionPan("iTrack_eCQMsDetail");

    }
}