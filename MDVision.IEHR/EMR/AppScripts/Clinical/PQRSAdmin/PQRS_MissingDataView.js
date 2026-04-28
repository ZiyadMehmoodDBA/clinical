//Author: Ahmad Raza
//File:   PQRS_MissingDataView
//Date:   17-08-2016
PQRS_MissingDataView = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {
        BackgroundLoaderShow(true);
        //PQRS_MissingDataView.params["TabID"] = 'PQRS_MissingDataView';
        PQRS_MissingDataView.params = params;

        if (PQRS_MissingDataView.params.PanelID != 'pnlMissingDataView') {
            PQRS_MissingDataView.params.PanelID = PQRS_MissingDataView.params.PanelID + ' #pnlMissingDataView';
        } else {
            PQRS_MissingDataView.params.PanelID = 'pnlMissingDataView';
        }

        if (PQRS_MissingDataView.params.ParentCtrl == "PQRS_Patient_List") {
            PQRS_MissingDataView.loadHeaderData(PQRS_MissingDataView.params.NoteId, PQRS_MissingDataView.params.PatientId, PQRS_MissingDataView.params.ProviderId, "Notes");
        }

    },

    loadHeaderData: function (NoteId, PatientId, ProviderId, FormName) {
        PQRS_MissingDataView.PreviewClinicalHeaderData(NoteId, PatientId, ProviderId, FormName).done(function (response) {
            //response = JSON.parse(response);
            if (response.status != false) {
                var objHeader = $(response.ReportHeaderInfo);
                $(objHeader).find('li').each(function () {
                    if ($(this).text().indexOf('DOB') > -1) {
                        $(this).text($(this).text().replace('12:00AM', ''))
                    }
                });
                response.ReportHeaderInfo = $(objHeader).html();
                var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                // Adding Header Footer to Report, If Selected provider of patient has any Report Header | Change Implmeneted by Muhammad Arshad on 26/01/2017
                if (response.ReportHeaderInfo == null || response.ReportHeaderInfo == '') {
                    var patientData = JSON.parse(response.NoteHeaderPatientData);
                    var providerData = JSON.parse(response.NoteHeaderProviderData);
                    var practiceData = JSON.parse(response.NoteHeaderPracticeData);
                    if (patientData.length > 0) {
                        var patientAccount = patientData[0].AccountNumber != "" ? "Acc. #: " + patientData[0].AccountNumber : "";
                        var patientCell = patientData[0].CellNo != "" ? "Ph: " + patientData[0].CellNo : "";
                        var patientName = (patientData[0].FirstName != "" ? patientData[0].FirstName : "") + (patientData[0].LastName != "" ? " " + patientData[0].LastName : "")
                        var age = (patientData[0].Age != "" ? patientData[0].Age + " Y," : "") + " " + patientData[0].Gender + ", DOB: " + utility.RemoveTimeFromDate(null, patientData[0].DOB);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PatientName").html(patientName);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PatientAge").html(age);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PatientAccount").html(patientAccount);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PatientPhone").html(patientCell);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PatientAddress").html(patientData[0].Address1);
                    }
                    if (providerData.length > 0) {
                        var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "")
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #ProviderName").html(providerName);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #Speciality").html(providerData[0].SpecialtyName);
                    }
                    if (practiceData.length > 0) {
                        var city = practiceData[0].City;
                        city += practiceData[0].State != "" ? ", " + practiceData[0].State : "";
                        city += practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : "";
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PracticeName").html(practiceData[0].ShortName);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PracticeAddress").html(practiceData[0].Address);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PracticeCity").html(city);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #PracticePhone").html(practiceData[0].PhoneNo);
                    }
                    if (NotesLoad_JSON.length > 0) {
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #NoteDateTime").html(utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate) + " " + NotesLoad_JSON[0].VisitTime);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #VisitReason").html(NotesLoad_JSON[0].VisitReason);
                    }
                } else {
                    if (NotesLoad_JSON.length > 0) {
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #NoteDateTime").html(utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate) + " " + NotesLoad_JSON[0].VisitTime);
                        $('#' + "pnlMissingDataView" + " #frmMissingDataView #VisitReason").html(NotesLoad_JSON[0].VisitReason);
                    }
                    $('#' + "pnlMissingDataView" + " #frmMissingDataView #printcall > div:not(#PatientInfo)").remove()
                    $('#' + "pnlMissingDataView" + " #frmMissingDataView #printcall header").remove();
                    $('#' + "pnlMissingDataView" + " #frmMissingDataView #printcall").prepend(response.ReportHeaderInfo);
                    $('#' + "pnlMissingDataView" + " #frmMissingDataView #PatientInfo").hide();
                }
                if (PQRS_MissingDataView.params["FromControlId"] != null && PQRS_MissingDataView.params["FromControlId"] != "") {
                    $('#' + "pnlMissingDataView" + " #frmMissingDataView #ulContent").html($(PQRS_MissingDataView.params["FromControlId"]).html());
                }
                //$('#' + "pnlMissingDataView" + " #frmMissingDataView #ulContent").html();
                BackgroundLoaderShow(true);
                PQRS_MissingDataView.getPrintnotePDF(false).done(function () {
                    BackgroundLoaderShow(false);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    PrintReports: function () {
        document.getElementById("PreviewReportPrint").contentWindow.print();
    },
    getPrintnotePDF: function (isSignNote) {
        var def = $.Deferred();

        //if (PQRS_MissingDataView.params.FromProgressnote == "1") {
        //    + "pnlMissingDataView" = "progressnotesign #PQRS_MissingDataView"
        //}
        // $('#' + "pnlMissingDataView" + " #contemporaryViewDiv").css('display', 'none');
        //   $('#' + "pnlMissingDataView" + " #legacyViewDiv").css('display', 'block');
        //$('#' + "pnlMissingDataView" + " #legacyViewTemplateDiv").css('display', 'block');

        // var HeaderLogo = $('#' + "pnlMissingDataView" + " #printcall :first img").prop('src');//response.HeaderLogo;
        var FooterText = $('#' + "pnlMissingDataView" + " #printcall footer").text().split('Generated by: ').join('');
        $('#progressnotesign').removeClass("hidden");
        $('#progressnotesign').show();
        $('#' + "pnlMissingDataView" + " #printcall").show();
        $('#' + "pnlMissingDataView" + " #printcall").css("display", "inline");
        // ----- footer 
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $('#' + "pnlMissingDataView" + " #page-templateLegacy").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(FooterText);
            $('#' + "pnlMissingDataView" + " #page-templateLegacy").html(PageTemp);
        }
        else {
            var footerText = $('#' + "pnlMissingDataView" + " #printcall  footer").text().split('Generated by: ').join('');
            var insideHTML = $('#' + "pnlMissingDataView" + " #page-templateLegacy").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(footerText);
            $('#' + "pnlMissingDataView" + " #page-templateLegacy").html(PageTemp);

        }
        var insideHTML = $('#' + "pnlMissingDataView" + " #page-templateLegacy").html();
        var PageTemp = $(insideHTML);
        $('#' + "pnlMissingDataView" + " #page-templateLegacy").html(PageTemp);
        var height = 0;

        var topMargin = height <= 18 ? 18 : height + 3;

        if ($('#' + "pnlMissingDataView" + " #page-templateLegacy .blueBorderPrint").length >= 1) {
            $('#' + "pnlMissingDataView" + " #printcall .form-group").remove();
            $('#' + "pnlMissingDataView" + " #printcall footer").remove();
        }
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM('#' + "pnlMissingDataView" + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            // margin: "2cm 3cm ",
            margin: {
                left: "10mm",
                //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                top: "3mm",
                //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                right: "10mm",
                bottom: "15mm"
            },
            template: $('#' + "pnlMissingDataView" + " #page-templateLegacy").html()
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                PQRS_MissingDataView.pdf = dataURL;
                var params = [];
                params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], false, 'PreviewReportPrint', true);
                def.resolve('ok');
                $('#' + "pnlMissingDataView" + " #printcall").hide();

            });

        });
        // ------------------------------------- End Download functionality--------------------------------------



        return def.promise();


    },
    UnLoad: function (caller) {
        if (PQRS_MissingDataView.params != null && PQRS_MissingDataView.params.ParentCtrl != null) {
            UnloadActionPan(PQRS_MissingDataView.params.ParentCtrl, 'PQRS_MissingDataView');
        }
        else
            UnloadActionPan(null, 'PQRS_MissingDataView');
    },

    printView: function () {
        setTimeout(function () {
            var docType = '<!doctype html>';
            var docCnt = $(' #printData').html();
            var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
         + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
         + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
         + '</script>'
         + '</head>';
            var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=865, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
            var newWin = window.open("", "_blank", winAttr);
            writeDoc = newWin.document;
            writeDoc.open();
            writeDoc.write(docType + '<html>' + docHead + '<body>' + docCnt + '</body></html>');
            writeDoc.close();

            newWin.focus();
            setTimeout(function () {
                newWin.focus();
                newWin.print();
            }, 200);

        }, 100);
    },
    //Author: Muhammad Arshad
    //Function Name: openPrintPreview Screen
    //Date: 26-01-2017
    //Description: DB Call to Load Header Data
    PreviewClinicalHeaderData: function (NotesID, PatientId, ProviderId, FormName) {
        var varFormName = "Notes";
        if (FormName != null && FormName != "") {
            varFormName = FormName;
        }
        var data = "NotesID=" + NotesID + "&PatientId=" + PatientId + "&ProviderId=" + ProviderId + "&FormName=" + varFormName;

        return MDVisionService.defaultService(data, "DASHBOARD", "PREVIEW_NOTES");
    },



}