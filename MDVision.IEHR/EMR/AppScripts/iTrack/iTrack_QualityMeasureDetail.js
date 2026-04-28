iTrack_QualityMeasureDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        iTrack_QualityMeasureDetail.params = params;

        if (iTrack_QualityMeasureDetail.bIsFirstLoad) {
            iTrack_QualityMeasureDetail.bIsFirstLoad = false;
            var self;
            if (iTrack_QualityMeasureDetail.params["PanelID"] != "pnliTrackQualityMeasureDetail")
                self = $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail");
            else
                self = $("#pnliTrackQualityMeasureDetail");

            if (iTrack_QualityMeasureDetail.params.CQMSearchData != null) {
                var CqmSearchData = JSON.parse(iTrack_QualityMeasureDetail.params.CQMSearchData);
                if (CqmSearchData.PatientId != "") {
                    $("#pnliTrackQualityMeasureDetail #txtPatientName").val(CqmSearchData.PatientAccountNumber);
                    $("#pnliTrackQualityMeasureDetail #hfPatientId").val(CqmSearchData.PatientId);
                }
            }

            self.loadDropDowns(true).done(function () {
                //utility.CreateDatePicker(iTrack_QualityMeasureDetail.params.PanelID + " #dpFrom, #dpTo", function () {
                //}, false);
                iTrack_QualityMeasureDetail.BindPatientName();
                iTrack_QualityMeasureDetail.ClinicalQualityMeasuresSearch(iTrack_QualityMeasureDetail.params.CQMSearchData, iTrack_QualityMeasureDetail.params.CQMID, iTrack_QualityMeasureDetail.params.providerId, iTrack_QualityMeasureDetail.params.dateFrom, iTrack_QualityMeasureDetail.params.dateTo);

                $("#measureName").text(iTrack_QualityMeasureDetail.params.Measure);
                $("#btnMeasureDocument").append(" How to "+iTrack_QualityMeasureDetail.params.Measure);
                $("#dpFrom").prop('disabled', true);
                $("#dpFrom").datepicker("setDate", iTrack_QualityMeasureDetail.params.dateFrom);
                $("#dpTo").prop('disabled', true);
                $("#dpTo").datepicker("setDate", iTrack_QualityMeasureDetail.params.dateTo);

            });
        }
    },

    OpenPatientSearch: function (RefCtrl) {
        var params = [];
        params["RefCtrl"] = RefCtrl;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "iTrack_QualityMeasureDetail";
        LoadActionPan("Patient_Search", params);
    },
    BatchClinicalQualityMeasureExport: function () {
        iTrack_QualityMeasureDetail.BatchClinicalQualityMeasureExport_().done(function (response) {
            if (response.status != false) {
                download("data:application/octet-stream;base64," + response.DownloadFile, response.FileName + ".xml", "application/octet-stream");

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    BatchClinicalQualityMeasureExport_: function () {

        var cqmid = iTrack_QualityMeasureDetail.params["CQMID"];

        var providerId = iTrack_QualityMeasureDetail.params["providerId"];
        var appointmentDateFrom = iTrack_QualityMeasureDetail.params["dateFrom"];
        var appointmentDateTo = iTrack_QualityMeasureDetail.params["dateTo"];
        var batchClinicalQualityMeasureData = iTrack_QualityMeasureDetail.params["CQMSearchData"]
        var tmp = JSON.parse(batchClinicalQualityMeasureData);
        tmp["Sex_text"] = null;
        tmp["AgeCondition_text"] = null;
        tmp["ProviderTypeId_text"] = null;
        batchClinicalQualityMeasureData = JSON.stringify(tmp)

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo + "&FROM=" + "Detail";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURES");
    },

    ExportPatientData: function (mode, cqmid, patientId, providerId, isC1) {

        if ($("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result").css("display") === "none") {
            $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result").show();
        }

        var self = $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Search");
        var myJson = self.getMyJSON();

        if (cqmid == "" || cqmid == null)
            cqmid = iTrack_QualityMeasureDetail.params.CQMID;

        if (providerId == "" || providerId == null)
            providerId = iTrack_QualityMeasureDetail.params.providerId;

        iTrack_QualityMeasureDetail.ExportClinicalQualityMeasureDetail(myJson, cqmid, patientId, providerId, isC1);
    },

    BindPatientAccount: function (fromFill) {
        var accountNo = null;
        if (!fromFill)
            accountNo = $("#pnliTrackQualityMeasureDetail #txtPatientName").val();
        else
            accountNo = $("#pnliTrackQualityMeasureDetail #txtPatientName").val().split("-")[0].trim();
        utility.Keyupdelay(function () {
            utility.GetPatientArray(accountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.AccountNumber;
                });

                $("#pnliTrackQualityMeasureDetail #txtPatientName").autocomplete({
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
                            $("#pnliTrackQualityMeasureDetail #txtPatientName").val(ui.item.AccountNumber);
                            $("#pnliTrackQualityMeasureDetail #hfPatientId").val(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                    }, 200);
                });;

                $("#pnliTrackQualityMeasureDetail #txtPatientName").autocomplete("search");
                $("#pnliTrackQualityMeasureDetail #txtPatientName").focus();
            });
        });
    },
    BindPatientName: function () {
        var Ctrl = $("#pnliTrackQualityMeasureDetail #txtFullName");
        var func = function () { return utility.GetPatientArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#pnliTrackQualityMeasureDetail #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },
    ClinicalQualityMeasuresSearch: function (CQMSearchData, cqmid, providerId, dateFrom, dateTo, pageNo, rpp) {
        if ($("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result").css("display") === "none") {
            $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result").show();
        }

        var self = $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Search");
        var myJson = self.getMyJSON();
        var objData = JSON.parse(myJson);
        objData["CQMSearchData"] = CQMSearchData;
        var myJson = JSON.stringify(objData);
        var objData = JSON.parse(myJson);
        objData["PatientId"] = $("#pnliTrackQualityMeasureDetail #hfPatientId").val() != -1 ? $("#pnliTrackQualityMeasureDetail #hfPatientId").val() : null;
        myJson = JSON.stringify(objData);

        iTrack_QualityMeasureDetail.SearchClinicalQualityMeasureDetail(myJson, cqmid, providerId, dateFrom, dateTo, pageNo, rpp).done(function (response) {

            if (response.status != false) {

                iTrack_QualityMeasureDetail.ClinicalQualityMeasureDetailGridLoad(response);

                var tableControl = iTrack_QualityMeasureDetail.params["PanelID"] + " #dgviTrackQualityMeasureDetail";
                var pagingPanelControlId = iTrack_QualityMeasureDetail.params["PanelID"] + " #divClinicalQualityMeasurePaging";

                var classControlName = "iTrack_QualityMeasureDetail";

                var pagesToDisplay = 5;
                var iTotalDisplayRecords = iTrack_QualityMeasureDetail.params["iP"];// response.iTotalDisplayRecords;
                if (rpp === "undefined" || rpp == null || rpp == undefined) {
                    rpp = response.ClinicalQualityMeasureDetailCount;
                }
                //setTimeout(CreatePagination(response.ClinicalQualityMeasureDetailCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords, function (primaryId, pageNumber, resultPerPage) {
                //    iTrack_QualityMeasureDetail.SearchClinicalQualityMeasureDetail("", "", "", "", "", pageNumber, rpp);
                //}), 10);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    
    ExportClinicalQualityMeasureDetail: function (myJson, cqmid, patientId, providerId, isC1) {
        if ($("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result").css("display") === "none") {
            $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result").show();
        }

        var self = $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Search");
        var myJson = self.getMyJSON();

        if ($("#dgviTrackQualityMeasureDetail tr").length > 1) {


            iTrack_QualityMeasureDetail.ExportClinicalQualityMeasureDetail_(myJson, '', '', '', '', '', isC1).done(function (response) {

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

        $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #dgviTrackQualityMeasureDetail").dataTable().fnDestroy();
        $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result #dgviTrackQualityMeasureDetail tbody").find("tr").remove();

        if (response.ClinicalQualityMeasureDetailCount > 0) {

            var clinicalQualityMeasureDetailLoadJsonData = JSON.parse(response.ClinicalQualityMeasureDetailLoad_JSON);
            $.each(clinicalQualityMeasureDetailLoadJsonData, function (i, item) {

                if (iTrack_QualityMeasureDetail.params.IsNonCompliance == "True") {
                    if (!(item.DenominatorExclusion == 0 && item.Numerator == 0)) {
                        return;
                    }       
                }
                if (item.CQMID == "0421a") {
                    item.CQMID = "0421";
                }
                if (item.CQMID == response.CQMID) {
                    var $row = $("<tr/>");
                    var providerId = iTrack_QualityMeasureDetail.params["providerId"];

                    $row.attr("id", "gvClinicalQualityMeasureDetail_row" + response.CQMID);
                    $row.attr("onclick", "iTrack_QualityMeasureDetail.SelectPatient('" + item.PatientID + "',event);");

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
                    $row.attr("onclick", "iTrack_QualityMeasureDetail.PatientSelect('" + item.PatientID + "','" + item.AccountNumber + "',event,);");
                    $row.append(
                        "<td style=\"display:none;\">" + response.CQMID + "</td>" +
                        
                        "<td id='FirstName'>" + item.AccountNumber + "</td>" +
                       "<td id='AccountNumber'>" + item.LastName + "</td>" +
                        "<td id='LastName'>" + item.FirstName + "</td>" +
                        "<td id='Denominator'>" + denominator + "</td>" +
                        "<td id='Numerator'>" + numerator + "</td>" +
                        "<td id='Exclusion'>" + denominatorExclusion + "</td>" +
                        "<td id='Exception'>" + denominatorException + "</td>");

                    $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result #dgviTrackQualityMeasureDetail tbody").last().append($row);

                }
            });

        }
        else {
            $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #dgviTrackQualityMeasureDetail").DataTable({
                "language": {
                    "emptyTable": "No patient against the Measure"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #dgviTrackQualityMeasureDetail"))
            ;
        else
            $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #pnliTrackQualityMeasureDetail_Result #dgviTrackQualityMeasureDetail").DataTable({ "bInfo": true, "bPaginate": true, "pageLength":30, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    PatientSelect: function (patientId,accountNbr,event) {
        event.preventDefault();
        event.stopPropagation();
        $.when(Patient_Search.SelectPatient(patientId, accountNbr, 'N', event)).then(function () {

            window.location = "/MDVisionDefault.aspx#Patient";
        });
        
       
      
      
    },
    PatientDemographics: function (patientId, event) {

        if (event != null) {
            event.preventDefault();
            event.stopPropagation();
 
        }
        $.when(iTrack_QualityMeasureDetail.openPatientDemographics(patientId, event)).then(function () {

            setTimeout(function () {
                demographicDetail.OpenClinicalSummary();
            }, 510);
        });

    },
    openPatientDemographics: function (patientid, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["mode"] = "Edit";
        params["PatBanner"] = true;
        params["patientID"] = patientid;
        params["IsFill"] = false;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "iTrack_QualityMeasureDetail";
        LoadActionPan("demographicDetail", params);
    },

    SearchClinicalQualityMeasureDetail: function (clinicalQualityMeasureDetailData, cqmid, providerId, dateFrom, dateTo, pageNumber, rowsPerPage) {

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = iTrack_QualityMeasureDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = iTrack_QualityMeasureDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = iTrack_QualityMeasureDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = iTrack_QualityMeasureDetail.params.dateTo;

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
            patientId = iTrack_QualityMeasureDetail.params.PatientId;

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = iTrack_QualityMeasureDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = iTrack_QualityMeasureDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = iTrack_QualityMeasureDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = iTrack_QualityMeasureDetail.params.dateTo;

        var data = "ClinicalQualityMeasureDetailData=" + clinicalQualityMeasureDetailData + "&PatientId=" + patientId + "&providerId=" + providerId + "&CQMID=" + cqmid + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&isC1=" + isC1;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURE_DETAILS");
    },

    UnLoadTab: function () {
        UnloadActionPan(null, "iTrack_QualityMeasureDetail");
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo, FirstName, LastName, RefCtrl, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if (RefCtrl == "txtFullName") {
            $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #txtFullName').val(LastName + ", " + FirstName + " ");
            $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #txtPatientName').val(AccountNo);
        }
        else {
            $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #frmiTrackPromotingInteroperabilityDetail #txtPatientName').val(AccountNo);
            // iTrack_QualityMeasureDetail.BindAutocomplete();
            $("#" + iTrack_QualityMeasureDetail.params["PanelID"] + " #txtFullName").val(LastName + ", " + FirstName + " ");
            utility.SetKendoAutoCompleteSourceforValidate($('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #frmiTrackPromotingInteroperabilityDetail #txtPatientName'), AccountNo, $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #hfPatientId'), PatientId);
            $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #frmiTrackPromotingInteroperabilityDetail #txtPatientName').focus();
        }

        $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #hfPatientId').val(PatientId);
        $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #hfAccountNo').val(AccountNo);
        utility.SetKendoAutoCompleteSourceforValidate($('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #txtFullName'), LastName + ", " + FirstName + " ", $('#' + iTrack_QualityMeasureDetail.params["PanelID"] + ' #hfPatientId'), PatientId, "FullName");
        if (Patient_Search.params != null && Patient_Search.params.ParentCtrl) {
            if (Patient_Search.params.ParentCtrl == "iTrack_QualityMeasureDetail") {

                UnloadActionPan(Patient_Search.params.ParentCtrl, 'Patient_Search', null, Patient_Search.params.ParentPanelID);
            } else {
                UnloadActionPan(Patient_Search.params.ParentCtrl);
            }

            Patient_Search.params = null;
        } else {
            UnloadActionPan("iTrack_QualityMeasureDetail");
        }
        utility.InsertRecentPatient(PatientId);


    },
    GetMeasurePDF: function () {
        iTrack_QualityMeasureDetail.LoadMeasurePDF().done(function (response) {
            if (response.status != false) {
                var base64String = response.pdfHelperBase64;
                download("data:application/octet-stream;base64," + base64String, response.FileName, "application/octet-stream");
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadMeasurePDF: function () {
        var measureId = iTrack_QualityMeasureDetail.params.MeasureNumber;
        if (measureId == "CMS160v5(A)" || measureId == "CMS160v5(B)" || measureId == "CMS160v5(C)") {
            measureId = "CMS160v5";
        } else if (measureId == "CMS156v5(A)" || measureId == "CMS156v5(B)" ) {
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
    ExportExcelData: function (e) {

        var JSONData = [];
        $("#" + iTrack_QualityMeasureDetail.params.PanelID + " #dgviTrackQualityMeasureDetail tbody tr").each(function () {

            var AccountNumber = $(this).find("#AccountNumber").get(0);
            AccountNumber = $(AccountNumber).text();
            var Denominator = $(this).find("#Denominator").get(0);
            Denominator = $(Denominator).find('i').hasClass('black') ? 1 : 0;
            var Numerator = $(this).find("#Numerator").get(0);
            Numerator = $(Numerator).find('i').hasClass('black') ? 1 : 0;
            var Exclusion = $(this).find("#Exclusion").get(0);
            Exclusion = $(Exclusion).find('i').hasClass('black') ? 1 : 0;
            var Exception = $(this).find("#Exception").get(0);
            Exception = $(Exception).find('i').hasClass('black') ? 1 : 0;
            var obj = {
                AccountNumber: $(this).find("#FirstName").text().trim(),
                LastName: AccountNumber,
                FirstName: $(this).find("#LastName").text().trim(),
                Denominator: Denominator,
                Numerator: Numerator,
                Exclusion: Exclusion,
                Exception: Exception,
               
            }
            JSONData.push(obj);

        });
        iTrack_QualityMeasureDetail.ExportDataToExcel(JSONData);
    },
    ExportDataToExcel: function (JSONData) {

        var ReportTitle = iTrack_QualityMeasureDetail.params.Measure;
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
}