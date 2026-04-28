iTrack_PromotingInteroperabilityDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        iTrack_PromotingInteroperabilityDetail.params = params;
        iTrack_PromotingInteroperabilityDetail.resultSet = params["resultSet"];
        iTrack_PromotingInteroperabilityDetail.params.Measure = params["Measure"];
        iTrack_PromotingInteroperabilityDetail.params.ProviderText = params["ProviderText"];
        iTrack_PromotingInteroperabilityDetail.params.PracticeText = params["PracticeText"];
        iTrack_PromotingInteroperabilityDetail.ID = params["ID"];
        if (iTrack_PromotingInteroperabilityDetail.bIsFirstLoad) {
            iTrack_PromotingInteroperabilityDetail.bIsFirstLoad = false;
            var self;
            if (iTrack_PromotingInteroperabilityDetail.params["PanelID"] != "pnliTrackPromotingInteroperabilityDetail")
                self = $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail");
            else
                self = $("#pnliTrackPromotingInteroperabilityDetail");

            if (iTrack_PromotingInteroperabilityDetail.params.CQMSearchData != null) {
                var CqmSearchData = JSON.parse(iTrack_PromotingInteroperabilityDetail.params.CQMSearchData);
                if (CqmSearchData.PatientId != "") {
                    $("#pnliTrackPromotingInteroperabilityDetail #txtPatientName").val(CqmSearchData.PatientAccountNumber);
                    $("#pnliTrackPromotingInteroperabilityDetail #hfPatientId").val(CqmSearchData.PatientId);
                }
            }

            self.loadDropDowns(true).done(function () {
                iTrack_PromotingInteroperabilityDetail.BindPatientName();
                iTrack_PromotingInteroperabilityDetail.fillPatientData();
                //iTrack_PromotingInteroperabilityDetail.ClinicalQualityMeasuresSearch(iTrack_PromotingInteroperabilityDetail.params.CQMSearchData, iTrack_PromotingInteroperabilityDetail.params.CQMID, iTrack_PromotingInteroperabilityDetail.params.providerId, iTrack_PromotingInteroperabilityDetail.params.dateFrom, iTrack_PromotingInteroperabilityDetail.params.dateTo);
                $("#measureName").text(iTrack_PromotingInteroperabilityDetail.params.Measure);
                $("#btnMeasureDocument").append(" How to document and report " + iTrack_PromotingInteroperabilityDetail.params.Measure);
                $("#dpFrom").prop('disabled', true);
                $("#dpFrom").datepicker("setDate", iTrack_PromotingInteroperabilityDetail.params.dateFrom);
                $("#dpTo").prop('disabled', true);
                $("#dpTo").datepicker("setDate", iTrack_PromotingInteroperabilityDetail.params.dateTo);

            });
        }
    },
    BindPatientName: function () {
        var Ctrl = $("#pnliTrackPromotingInteroperabilityDetail #txtFullName");
        var func = function () { return utility.GetPatientArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#pnliTrackPromotingInteroperabilityDetail #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },
    fillPatientData: function () {
        if (iTrack_PromotingInteroperabilityDetail.resultSet.length > 0) {
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #dgviTrackPromotingInteroperabilityDetail").dataTable().fnDestroy();
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result #dgviTrackPromotingInteroperabilityDetail tbody").find("tr").remove();

            $.each(iTrack_PromotingInteroperabilityDetail.resultSet, function (i, item) {
                var numval = "0";
                if (iTrack_PromotingInteroperabilityDetail.params.IsCompliance == "Compliance") {
                    numval = "1";
                } else if (iTrack_PromotingInteroperabilityDetail.params.IsCompliance == "NonCompliance") {
                    numval = "0";
                }
                if (item.ID == iTrack_PromotingInteroperabilityDetail.ID && item.Numerator == numval) {
                    var $row = $('<tr/>');
                    $row.attr("id", "gvClinicalQualityMeasureDetail_row" + item.ID);
                    $row.append('<td style="display:none" >' + item.ID + '</td><td class="size80" id="AccountNumber">' + item.accountnumber + '</td><td class="size80" id="FirstName">' + item.FirstName + '</td>' + '<td id="LastName" class="size80">' + item.LastName + '</td>'
                        + '<td class="size80" id="DOB">' + utility.RemoveTimeFromDate(null, item.DOB) + '</td>'
                        + '<td class="size80" ID="Gender">' + item.Gender + '</td><td class="size80" id="Denominator">' + item.Denominator + '</td><td class="size80" id="Numerator">' + item.Numerator + '</td>');
                    $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result #dgviTrackPromotingInteroperabilityDetail tbody").last().append($row);
                }
            });
        } else {
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #dgviTrackPromotingInteroperabilityDetail").DataTable({
                "language": {
                    "emptyTable": "No patient against the Measure"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #dgviTrackPromotingInteroperabilityDetail"))
            ;
        else
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result #dgviTrackPromotingInteroperabilityDetail").DataTable({ "bInfo": true, "bPaginate": true, "pageLength": 30, "bLengthChange": false, "autoWidth": false }); // to remove records per page dropdown

    },
    OpenPatientSearch: function (RefCtrl) {
        var params = [];
        params["RefCtrl"] = RefCtrl;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "iTrack_PromotingInteroperabilityDetail";
        LoadActionPan("Patient_Search", params);
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNo, FirstName, LastName, RefCtrl, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if (RefCtrl == "txtFullName") {
            $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #txtFullName').val(LastName + ", " + FirstName + " ");
            $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #txtPatientName').val(AccountNo);
        }
        else {
            $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #frmiTrackPromotingInteroperabilityDetail #txtPatientName').val(AccountNo);
            // iTrack_PromotingInteroperabilityDetail.BindAutocomplete();
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #txtFullName").val(LastName + ", " + FirstName + " ");
            utility.SetKendoAutoCompleteSourceforValidate($('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #frmiTrackPromotingInteroperabilityDetail #txtPatientName'), AccountNo, $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #hfPatientId'), PatientId);
            $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #frmiTrackPromotingInteroperabilityDetail #txtPatientName').focus();
        }

        $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #hfPatientId').val(PatientId);
        $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #hfAccountNo').val(AccountNo);
        utility.SetKendoAutoCompleteSourceforValidate($('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #txtFullName'), LastName + ", " + FirstName + " ", $('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + ' #hfPatientId'), PatientId, "FullName");
        if (Patient_Search.params != null && Patient_Search.params.ParentCtrl) {
            if (Patient_Search.params.ParentCtrl == "iTrack_PromotingInteroperabilityDetail") {

                UnloadActionPan(Patient_Search.params.ParentCtrl, 'Patient_Search', null, Patient_Search.params.ParentPanelID);
            } else {
                UnloadActionPan(Patient_Search.params.ParentCtrl);
            }

            Patient_Search.params = null;
        } else {
            UnloadActionPan("iTrack_PromotingInteroperabilityDetail");
        }
        utility.InsertRecentPatient(PatientId);


    },
    BatchClinicalQualityMeasureExport: function () {
        iTrack_PromotingInteroperabilityDetail.BatchClinicalQualityMeasureExport_().done(function (response) {
            if (response.status != false) {
                download("data:application/octet-stream;base64," + response.DownloadFile, response.FileName + ".xml", "application/octet-stream");

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    BatchClinicalQualityMeasureExport_: function () {

        var cqmid = iTrack_PromotingInteroperabilityDetail.params["CQMID"];

        var providerId = iTrack_PromotingInteroperabilityDetail.params["providerId"];
        var appointmentDateFrom = iTrack_PromotingInteroperabilityDetail.params["dateFrom"];
        var appointmentDateTo = iTrack_PromotingInteroperabilityDetail.params["dateTo"];
        var batchClinicalQualityMeasureData = iTrack_PromotingInteroperabilityDetail.params["CQMSearchData"]
        var tmp = JSON.parse(batchClinicalQualityMeasureData);
        tmp["Sex_text"] = null;
        tmp["AgeCondition_text"] = null;
        tmp["ProviderTypeId_text"] = null;
        batchClinicalQualityMeasureData = JSON.stringify(tmp)

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo + "&FROM=" + "Detail";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURES");
    },

    ExportPatientData: function (mode, cqmid, patientId, providerId, isC1) {

        if ($("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result").css("display") === "none") {
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result").show();
        }

        var self = $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Search");
        var myJson = self.getMyJSON();

        if (cqmid == "" || cqmid == null)
            cqmid = iTrack_PromotingInteroperabilityDetail.params.CQMID;

        if (providerId == "" || providerId == null)
            providerId = iTrack_PromotingInteroperabilityDetail.params.providerId;

        iTrack_PromotingInteroperabilityDetail.ExportClinicalQualityMeasureDetail(myJson, cqmid, patientId, providerId, isC1);
    },

    BindPatientAccount: function (fromFill) {
        var accountNo = null;
        if (!fromFill)
            accountNo = $("#pnliTrackPromotingInteroperabilityDetail #txtPatientName").val();
        else
            accountNo = $("#pnliTrackPromotingInteroperabilityDetail #txtPatientName").val().split("-")[0].trim();
        utility.Keyupdelay(function () {
            utility.GetPatientArray(accountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.AccountNumber;
                });

                $("#pnliTrackPromotingInteroperabilityDetail #txtPatientName").autocomplete({
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
                            $("#pnliTrackPromotingInteroperabilityDetail #txtPatientName").val(ui.item.AccountNumber);
                            $("#pnliTrackPromotingInteroperabilityDetail #hfPatientId").val(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                    }, 200);
                });;

                $("#pnliTrackPromotingInteroperabilityDetail #txtPatientName").autocomplete("search");
                $("#pnliTrackPromotingInteroperabilityDetail #txtPatientName").focus();
            });
        });
    },

    ClinicalQualityMeasuresSearch: function () {
        if ($("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result").css("display") === "none") {
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result").show();
        }

        var self = $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Search");
        var myJson = self.getMyJSON();
        var objData = JSON.parse(myJson);
        objData["CQMSearchData"] = iTrack_PromotingInteroperabilityDetail.params.CQMSearchData;
        var myJson = JSON.stringify(objData);
        var objData = JSON.parse(myJson);
        objData["PatientId"] = $("#pnliTrackPromotingInteroperabilityDetail #hfPatientId").val() != -1 ? $("#pnliTrackPromotingInteroperabilityDetail #hfPatientId").val() : null;
        myJson = JSON.stringify(objData);

        iTrack_PromotingInteroperabilityDetail.SearchClinicalQualityMeasureDetail(myJson, cqmid, providerId, dateFrom, dateTo, pageNo, 100).done(function (response) {

            if (response.status != false) {

                iTrack_PromotingInteroperabilityDetail.ClinicalQualityMeasureDetailGridLoad(response);

                var tableControl = iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #dgviTrackPromotingInteroperabilityDetail";
                var pagingPanelControlId = iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #diviTrackPromotingInteroperabilityDetailPaging";

                var classControlName = "iTrack_PromotingInteroperabilityDetail";

                var pagesToDisplay = 5;
                var iTotalDisplayRecords = iTrack_PromotingInteroperabilityDetail.params["iP"];// response.iTotalDisplayRecords;
                if (rpp === "undefined" || rpp == null || rpp == undefined) {
                    rpp = 100; //response.ClinicalQualityMeasureDetailCount;
                }
                //setTimeout(CreatePagination(response.ClinicalQualityMeasureDetailCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords, function (primaryId, pageNumber, resultPerPage) {
                //    iTrack_PromotingInteroperabilityDetail.SearchClinicalQualityMeasureDetail("", "", "", "", "", pageNumber, 100);
                //}), 10);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ExportClinicalQualityMeasureDetail: function (myJson, cqmid, patientId, providerId, isC1) {
        if ($("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result").css("display") === "none") {
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result").show();
        }

        var self = $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Search");
        var myJson = self.getMyJSON();

        if ($("#dgviTrackPromotingInteroperabilityDetail tr").length > 1) {


            iTrack_PromotingInteroperabilityDetail.ExportClinicalQualityMeasureDetail_(myJson, '', '', '', '', '', isC1).done(function (response) {

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


        $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #dgviTrackPromotingInteroperabilityDetail").dataTable().fnDestroy();
        $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result #dgviTrackPromotingInteroperabilityDetail tbody").find("tr").remove();

        if (response.ClinicalQualityMeasureDetailCount > 0) {

            var clinicalQualityMeasureDetailLoadJsonData = JSON.parse(response.ClinicalQualityMeasureDetailLoad_JSON);
            $.each(clinicalQualityMeasureDetailLoadJsonData, function (i, item) {

                if (item.CQMID == "0421a") {
                    item.CQMID = "0421";
                }
                if (item.CQMID == response.CQMID) {
                    var $row = $("<tr/>");
                    var providerId = iTrack_PromotingInteroperabilityDetail.params["providerId"];

                    $row.attr("id", "gvPromotingInteroperabilityDetail_row" + response.CQMID);
                    //$row.attr("onclick", "iTrack_PromotingInteroperabilityDetail.ExportPatientData('Export','" + response.CQMID + "','" + item.PatientID + "','" + providerId + "',event);utility.SelectGridRow($(this));");

                    $row.attr("CQMID", response.CQMID);


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
                        "<td id='AccountNumber'><a href=\"#\" onclick=\"iTrack_PromotingInteroperabilityDetail.PatientDemographics(" + item.PatientID + ",event);\"  title=\"View Patient\">" + item.AccountNumber + "</a></td>" +
                        "<td id='FirstName'>" + item.LastName + "</td>" +
                        "<td id='LastName'>" + item.FirstName + "</td>" +
                        "<td id='DOB'>" + denominator + "</td>" +
                        "<td id='Gender'>" + numerator + "</td>" +
                        "<td id='Denominator'>" + denominatorExclusion + "</td>" +
                        "<td id='Numerator'>" + denominatorException + "</td>");

                    $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result #dgviTrackPromotingInteroperabilityDetail tbody").last().append($row);

                }
            });

        }
        else {
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #dgviTrackPromotingInteroperabilityDetail").DataTable({
                "language": {
                    "emptyTable": "No patient against the Measure"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #dgviTrackPromotingInteroperabilityDetail"))
            ;
        else
            $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result #dgviTrackPromotingInteroperabilityDetail").DataTable({ "bInfo": true, "bPaginate": true, "pageLength": 30, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    printPreview: function () {
        var providerData = "";
        if (iTrack_PromotingInteroperabilityDetail.params.ProviderText != 'undefined' && iTrack_PromotingInteroperabilityDetail.params.ProviderText != null)
            providerData = iTrack_PromotingInteroperabilityDetail.params.ProviderText.split('<br/>');
        var newProviderText = '';
        for (var i = 0; i < providerData.length; i++) {
            if ($.trim(providerData[i]) != '') {
                newProviderText += '<li class="text-left">' + providerData[i] + '</li>';
            }
        }
        $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall #ProviderList").html(newProviderText);
        $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall #ulContent #MeasureName").append(iTrack_PromotingInteroperabilityDetail.params.Measure);
        var practiceData = "";
        if (iTrack_PromotingInteroperabilityDetail.params.PracticeText != 'undefined' && iTrack_PromotingInteroperabilityDetail.params.PracticeText != null)
            practiceData = iTrack_PromotingInteroperabilityDetail.params.PracticeText.split('<br/>');
        var newPracticeText = '';
        for (var i = 0; i < practiceData.length; i++) {
            if ($.trim(practiceData[i]) != '') {
                newPracticeText += '<li class="text-right">' + practiceData[i] + '</li>';
            }
        }
        $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall #PracticeList").html(newPracticeText);


        var date = new Date();
        var day = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();

        var mnth = day.split('/')[0];
        var dy = day.split('/')[1];
        var yr = day.split('/')[2];
        mnth = mnth.length == 1 ? "0" + mnth : mnth;
        dy = dy.length == 1 ? "0" + dy : dy;
        var curdate = mnth + "/" + dy + "/" + yr;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        var time = strTime;
        var datetime = curdate + " " + time;
        $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall #liCurrentDate").text(datetime);
        params["UlContent"] = $("#" + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #pnliTrackPromotingInteroperabilityDetail_Result #dgviTrackPromotingInteroperabilityDetail")[0].outerHTML;
        $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall #ulContent").append(params["UlContent"]);
        iTrack_PromotingInteroperabilityDetail.PrintReports();
    },
    PrintReports: function () {
        $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall").removeClass('hidden');
        kendo.drawing.drawDOM('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($('#' + iTrack_PromotingInteroperabilityDetail.params["PanelID"] + " #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, null, null, true);
                $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall").addClass('hidden');
                $('#' + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #printcall #ulContent").html("");
            });

        });
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
        params["ParentCtrl"] = "iTrack_PromotingInteroperabilityDetail";
        LoadActionPan("demographicDetail", params);
    },

    SearchClinicalQualityMeasureDetail: function (clinicalQualityMeasureDetailData, cqmid, providerId, dateFrom, dateTo, pageNumber, rowsPerPage) {

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = iTrack_PromotingInteroperabilityDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = iTrack_PromotingInteroperabilityDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = iTrack_PromotingInteroperabilityDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = iTrack_PromotingInteroperabilityDetail.params.dateTo;

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
            patientId = iTrack_PromotingInteroperabilityDetail.params.PatientId;

        if (cqmid == null || cqmid === "" || cqmid === "undefined")
            cqmid = iTrack_PromotingInteroperabilityDetail.params.CQMID;

        if (providerId == null || providerId === "" || providerId === "undefined")
            providerId = iTrack_PromotingInteroperabilityDetail.params.providerId;

        if (dateFrom == null || dateFrom === "" || dateFrom === "undefined")
            dateFrom = iTrack_PromotingInteroperabilityDetail.params.dateFrom;

        if (dateTo == null || dateTo === "" || dateTo === "undefined")
            dateTo = iTrack_PromotingInteroperabilityDetail.params.dateTo;

        var data = "ClinicalQualityMeasureDetailData=" + clinicalQualityMeasureDetailData + "&PatientId=" + patientId + "&providerId=" + providerId + "&CQMID=" + cqmid + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&isC1=" + isC1;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURE_DETAILS");
    },

    UnLoadTab: function () {
        UnloadActionPan(null, "iTrack_PromotingInteroperabilityDetail");
    },

    //FillPatientInfoFromSearch: function (patientId, accountNumber, fullname, event) {
    //    if (event != null) {
    //        event.stopPropagation();
    //    }
    //    var self = null;
    //    self = $("#pnliTrackPromotingInteroperabilityDetail");
    //    self.find("#hfPatientId").val(patientId);
    //    self.find("#txtPatientName").val(accountNumber);

    //    $Ctrl = $("#" + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #txtPatientName");
    //    $hfCtrl = $("#" + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #hfPatientId");
    //    //Patient
    //    utility.SetAutoCompleteSource($Ctrl, $hfCtrl);

    //    //$("#txtPatientName").val(accountNumber);
    //    UnloadActionPan("iTrack_PromotingInteroperabilityDetail");

    //},
    GetMeasurePDF: function () {
        iTrack_PromotingInteroperabilityDetail.LoadMeasurePDF().done(function (response) {
            if (response.status != false) {
                var base64String = response.pdfHelperBase64;
                download("data:application/octet-stream;base64," + base64String, response.FileName, "application/octet-stream");
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadMeasurePDF: function () {
        var measureId = iTrack_PromotingInteroperabilityDetail.params.Measure;
        if (measureId == "Request/Accept Summary of Care") {
            measureId = "Request Accept Summary of Care";
        } 
        var data = "fileName=" + measureId + ".pdf";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "GET_MEASURE_DOCUMENT");
    },
    ExportExcelData: function (e) {

        var JSONData = [];
        $("#" + iTrack_PromotingInteroperabilityDetail.params.PanelID + " #dgviTrackPromotingInteroperabilityDetail tbody tr").each(function () {

            var obj = {
                AccountNumber: $(this).find("#AccountNumber").text().trim(),
                FirstName: $(this).find("#FirstName").text().trim(),
                LastName: $(this).find("#LastName").text().trim(),
                DOB: $(this).find("#DOB").text().trim(),
                Gender: $(this).find("#Gender").text().trim(),
                Denominator: $(this).find("#Denominator").text().trim(),
                Numerator: $(this).find("#Numerator").text().trim(),

            }
            JSONData.push(obj);

        });
        iTrack_PromotingInteroperabilityDetail.ExportDataToExcel(JSONData);
    },
    ExportDataToExcel: function (JSONData) {

        var ReportTitle = iTrack_PromotingInteroperabilityDetail.params.Measure;
        var ShowLabel = true;
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        CSV += ReportTitle + '\r\n\n';
        if (ShowLabel) {
            var row = "";
            for (var index in arrData[0]) {
                if (index == "AccountNumber") {
                    index = "Account Number";
                } else if (index == "First Name") {
                    index = "First Name";
                } else if (index == "LastName") {
                    index = "Last Name";
                }
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