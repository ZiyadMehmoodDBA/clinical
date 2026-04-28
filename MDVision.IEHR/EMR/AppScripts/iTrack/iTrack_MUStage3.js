
iTrack_MUStage3 = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        iTrack_MUStage3.params = params;
        $('#pnlMUStage3 #headingTitle').html('<strong>MU Stage 3 Report</strong> | Measure Name: ' + iTrack_MUStage3.params['Measure']);

        iTrack_MUStage3.resultSet = params["resultSet"];
        iTrack_MUStage3.heading = params["rptName"];
        iTrack_MUStage3.ID = params["ID"];
        if (iTrack_MUStage3.params.PanelID != 'pnliTrack_MUStage3') {
            iTrack_MUStage3.params.PanelID = iTrack_MUStage3.params.PanelID + ' #pnliTrack_MUStage3';
        } else {
            iTrack_MUStage3.params.PanelID = 'pnliTrack_MUStage3';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + iTrack_MUStage3.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $('#' + iTrack_MUStage3.params["PanelID"] + ' #headingTitle').text(iTrack_MUStage3.heading);
        if (iTrack_MUStage3.heading == "MU Stage 1 Report" || iTrack_MUStage3.heading == "MU Stage 2 Report") {
            iTrack_MUStage3.fillPatientData();
        } else if (iTrack_MUStage3.heading == "MU Stage 2 Report Latest" || iTrack_MUStage3.heading == "MU Stage 3 Report") {
            iTrack_MUStage3.fillPatientDataNumerator();
        }
        $("#pnlMUStage3 #btnMeasureDocument").append(" How to report " + iTrack_MUStage3.params['Measure']);
    },
    printPreview: function () {
        var providerData = "";
        if (iTrack_MUStage3.params.ProviderText != 'undefined' && iTrack_MUStage3.params.ProviderText != null)
            providerData = iTrack_MUStage3.params.ProviderText.split('<br/>');
        var newProviderText = '';
        for (var i = 0; i < providerData.length; i++) {
            if ($.trim(providerData[i]) != '') {
                newProviderText += '<li class="text-left">' + providerData[i] + '</li>';
            }
        }
        $("#pnlMUStage3 #printcall #ProviderList").html(newProviderText);
        $("#pnlMUStage3 #printcall #ulContent #MeasureName").append(iTrack_MUStage3.params.Measure);
        var practiceData = "";
        if (iTrack_MUStage3.params.PracticeText != 'undefined' && iTrack_MUStage3.params.PracticeText != null)
            practiceData = iTrack_MUStage3.params.PracticeText.split('<br/>');
        var newPracticeText = '';
        for (var i = 0; i < practiceData.length; i++) {
            if ($.trim(practiceData[i]) != '') {
                newPracticeText += '<li class="text-right">' + practiceData[i] + '</li>';
            }
        }
        $("#pnlMUStage3 #printcall #PracticeList").html(newPracticeText);


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
        $("#pnlMUStage3 #printcall #liCurrentDate").text(datetime);
        params["UlContent"] = $("#pnlMUStage3 #tblMUData")[0].outerHTML
        $("#pnlMUStage3 #printcall #ulContent").append(params["UlContent"]);
        iTrack_MUStage3.PrintReports();
    },
    PrintReports: function () {
        $("#pnlMUStage3 #printcall").removeClass('hidden');
        kendo.drawing.drawDOM("#pnlMUStage3 #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($("#pnlMUStage3 #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, null, null, true);
                $("#pnlMUStage3 #printcall").addClass('hidden');
                $("#pnlMUStage3 #printcall #ulContent").html("");
            });

        });
    },
    ExportExcelData: function (e) {

        var JSONData = [];
        $("#pnlMUStage3 #tblMUData tbody tr").each(function () {

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
        iTrack_MUStage3.ExportDataToExcel(JSONData);
    },
    ExportDataToExcel: function (JSONData) {

        var ReportTitle = iTrack_MUStage3.params.Measure;
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
    GetMeasurePDF: function () {
        iTrack_MUStage3.LoadMeasurePDF().done(function (response) {
            if (response.status != false) {
                var base64String = response.pdfHelperBase64;
                download("data:application/octet-stream;base64," + base64String, response.FileName, "application/octet-stream");
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadMeasurePDF: function () {
        var measureId = iTrack_MUStage3.params.Measure;
        if (measureId == "Request/Accept Summary of Care") {
            measureId = "Request Accept Summary of Care";
        }
        var data = "fileName=" + measureId + ".pdf";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "GET_MEASURE_DOCUMENT");
    },
    fillPatientData: function () {
        if (iTrack_MUStage3.resultSet.length > 0 && iTrack_MUStage3.ID > 0) {
            $('#' + iTrack_MUStage3.params["PanelID"] + ' #tblMUData > tbody').html("");
            $.each(iTrack_MUStage3.resultSet, function (i, item) {
                if (item.ID == iTrack_MUStage3.ID && item.Numerator == "0") {
                    var MethodMode = "iTrack_MUStage3.SelectPatient(" + item.PatientID + ", event);";
                    var $row = $('<tr/>');
                    $row.attr("onclick", MethodMode);
                    $row.append('<td id="AccountNumber">' + item.accountnumber + '</td><td id="FirstName">' + item.FirstName + '</td>' + '<td id="LastName">' + item.LastName + '</td>'
                        + '<td id="DOB">' + utility.RemoveTimeFromDate(null, item.DOB) + '</td>'
                        + '<td id="Gender">' + item.Gender + '</td><td id="Denominator">' + item.Denominator + '</td><td id="Numerator">' + item.Numerator + '</td>');
                    $('#' + iTrack_MUStage3.params["PanelID"] + ' #tblMUData > tbody').last().append($row);
                }
            });
        } 

    },

    fillPatientDataNumerator: function () {
        if (iTrack_MUStage3.resultSet.length > 0 && iTrack_MUStage3.ID > 0) {
            $('#' + iTrack_MUStage3.params["PanelID"] + ' #tblMUData > tbody').html("");
            $.each(iTrack_MUStage3.resultSet, function (i, item) {
                if (item.ID == iTrack_MUStage3.ID/* && item.Numerator == "1"*/) {
                    var MethodMode = "iTrack_MUStage3.SelectPatient(" + item.PatientID + ", event);";
                    var $row = $('<tr/>');
                    $row.attr("onclick", MethodMode);
                    $row.append('<td id="AccountNumber">' + item.accountnumber + '</td><td id="FirstName">' + item.FirstName + '</td>' + '<td id="LastName">' + item.LastName + '</td>'
                        + '<td id="DOB">' + utility.RemoveTimeFromDate(null, item.DOB) + '</td>'
                        + '<td id="Gender">' + item.Gender + '</td><td id="Denominator">' + item.Denominator + '</td><td id="Numerator">' + item.Numerator + '</td>');
                    if (iTrack_MUStage3.params.ParentCtrl == "iTrack_MUReport")
                    {
                        $('#pnlMUStage3 #tblMUData > tbody').last().append($row);
                    }
                    else {
                        $('#' + iTrack_MUStage3.params["PanelID"] + ' #tblMUData > tbody').last().append($row);
                    }
                }
            });
        } else {
            $("#pnlMUStage3 #tblMUData").DataTable({
                "language": {
                    "emptyTable": "No patient against the Measure"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#pnlMUStage3 #tblMUData"))
            ;
        else
            $("#pnlMUStage3 #tblMUData").DataTable({ "bInfo": true, "bPaginate": true, "pageLength": 30, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },

    SelectPatient: function (PatientId, event) {
        iTrack_MUStage3.UnLoadTab();
        if (event != null) {
            event.stopPropagation();
        }
        $.when(SelectPatient(PatientId, "")).done(function () {
            $('#patTabDemographic').click();
        });
    },
    UnLoadTab: function () {
        if (iTrack_MUStage3.params["FromAdmin"] == "0") {
            if (iTrack_MUStage3.params != null && iTrack_MUStage3.params.ParentCtrl != null) {
                UnloadActionPan(iTrack_MUStage3.params.ParentCtrl, 'iTrack_MUStage3');

            }
            else
                UnloadActionPan(null, 'iTrack_MUStage3');
        }
        else {

            RemoveAdminTab();
        }
    },
}