
Clinical_InfoButtonView = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Clinical_InfoButtonView.params = params;
        if (Clinical_InfoButtonView.params != null && Clinical_InfoButtonView.params.PanelID != "pnlClinical_InfoButtonView") {
            Clinical_InfoButtonView.params["PanelID"] = Clinical_InfoButtonView.params["PanelID"] + ' #pnlClinical_InfoButtonView';
        }
        else {
            Clinical_InfoButtonView.params = [];
            Clinical_InfoButtonView.params["PanelID"] = "pnlClinical_InfoButtonView";
        }

        if (Clinical_InfoButtonView.bIsFirstLoad) {
            Clinical_InfoButtonView.bIsFirstLoad = false;

            Clinical_InfoButtonView.InfoButtonPreview(Clinical_InfoButtonView.params.PatientId, Clinical_InfoButtonView.params.searchStr, Clinical_InfoButtonView.params.codeSystem, Clinical_InfoButtonView.params.UserName, Clinical_InfoButtonView.params.ParentCtrl);

        }

    },

    InfoButtonPreview: function (patientID, searchStr, codeSystem, userName, caller) {
        if (Clinical_InfoButtonView.params["caller"] != undefined) {
            caller = Clinical_InfoButtonView.params["caller"];
        }
        Clinical_InfoButtonView.previewInfoButton(patientID, searchStr, codeSystem, userName, caller).done(function (response) {
            response = JSON.parse(response);
            if (response.status !== false) {
                //Start 22-07-2016 Humaira Yousaf
                $("#pnlClinical_InfoButtonView #OpenHTMLDocument").html(atob(response.data));

                $($("#pnlClinical_InfoButtonView .results")[1]).css("min-height", '400px');
                $($("#pnlClinical_InfoButtonView .results")[1]).css("height", '400px');
                // EMR 697    $($("#pnlClinical_InfoButtonView .results")[1]).css("overflow-y", "scroll");
                $($("#pnlClinical_InfoButtonView .results")[1]).css("overflow-y", "auto");

                Clinical_InfoButtonView.medlineLinks();
                $("#pnlClinical_InfoButtonView #drugs").removeClass("results");
                $("#pnlClinical_InfoButtonView #drugs ul").replaceWith($("#pnlClinical_InfoButtonView #drugs ul").contents());
                $("#pnlClinical_InfoButtonView #drugs li").replaceWith($("#pnlClinical_InfoButtonView #drugs li").contents());
                $('#' + Clinical_InfoButtonView.params["PanelID"] + " #ptEduFooter").hide();
                $('#' + Clinical_InfoButtonView.params["PanelID"] + " #footerbar").hide();
                $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument #btnDownload').hide();
                $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument #btnPrinter').hide();
                $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument #btnEmail').hide();
                kendo.drawing.drawDOM('#pnlClinical_InfoButtonView #OpenHTMLDocument', {
                    landscape: false,
                    scale: 0.6,
                    paperSize: "A4",
                    // margin: "2cm 3cm ",
                    margin: {
                        left: "10mm",
                        top: "7mm",
                        right: "10mm",
                        bottom: "15mm"
                    },
                    template: kendo.template($('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-templateLegacy").html())
                }).then(function (group) {
                    kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                        var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');

                        var objData = {};

                        objData["stream"] = PrintPDFDataURL;
                        objData["PatientId"] = patientID;
                        objData["FileName"] = response.Message;
                        objData["fileType"] = "application/pdf";
                        var data = JSON.stringify(objData);

                        var resp = Clinical_InfoButtonView.SaveDocument(data);
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #ptEduFooter").show();
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument #btnDownload').show();
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument #btnPrinter').show();
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument #btnEmail').show();
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #footerbar").show();

                        if (resp.status != true) {
                            utility.DisplayMessages(resp.Message, 3);
                        }

                    });

                });
            }
            else
                utility.DisplayMessages(response.Message, 3);

        });
    },

    DownloadInfo: function () {
        var header = "";
        header = '<div style="top:20px;"></div';
        var form = $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument').clone();
        $(form).find('title').remove();
        $(form).find('link').remove();

        if (Clinical_InfoButtonView.params["ParentCtrl"] != 'Clinical_Medications')
            $(form).find('article').remove();

        $(form).find('#btnDownload').remove();
        $(form).find('#btnPrinter').remove();
        $(form).find('#btnEmail').remove();
        $(form).find(".results").removeAttr('style');
        $(form).find("#drugs").removeAttr('style');
        var str = $(form).find(".noBoxshadow").first().find(".pt-sm").first();
        header = $(header).append($(str).html());
        $(form).find(".noBoxshadow").first().remove();

        $("#hDownloadContent").append($(header).html() + $(form).html());
        kendo.drawing.drawDOM($("#hDownloadContent")).then(function (group) {
            group.options.set("pdf", {
                margin: {
                    left: "5mm",
                    top: "10mm",
                    right: "5mm",
                    bottom: "10mm"
                }
            });
            kendo.drawing.pdf.saveAs(group, "PatientEducation.pdf");
            $("#hDownloadContent").empty();
        });
    },

    previewInfoButton: function (patientID, searchstr, codeSystem, UserName, caller) {
        var objData = {};

        objData["searchStr"] = searchstr;
        objData["codeSystem"] = codeSystem;
        objData["UserName"] = UserName;
        var ProviderId = Clinical_ProgressNote.params.CurrentNotesProviderId;
        if (ProviderId && Clinical_InfoButtonView.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId;
            objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        }
        else {
            objData["ProviderId"] = -1;
        }
        if (Clinical_InfoButtonView.params.ParentCtrl == "OrderSet_Problems" || Clinical_InfoButtonView.params.ParentCtrl == "Clinical_OrderSetDetails") {
            objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
            objData["Caller"] = caller;
            objData["PatientId"] = -1;
            var data = JSON.stringify(objData);

            return MDVisionService.APIService(data, "OrderSet", "GetInfobuttonDetails");
        }
        else {
            objData["PatientId"] = patientID;
            objData["Caller"] = caller;

            var data = JSON.stringify(objData);

            return MDVisionService.APIService(data, "InfoButton", "GetInfobuttonDetails");
        }

    },

    UnLoad: function () {

        if (Clinical_InfoButtonView.params != null && Clinical_InfoButtonView.params.ParentCtrl) {
            if (Clinical_InfoButtonView.params["ParentCtrl"] == "Clinical_LabOrder" || Clinical_InfoButtonView.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                UnloadActionPan(Clinical_InfoButtonView.params["ParentCtrl"], "Clinical_InfoButtonView", null, Clinical_InfoButtonView.params["ParentCtrlPanelID"]);
            } else if (Clinical_InfoButtonView.params.ParentCtrl == "Clinical_OrderSetDetails") {
                UnloadActionPan(Clinical_InfoButtonView.params.ParentCtrl, "Clinical_InfoButtonView");
                Clinical_OrderSetDetails.LoadOrderSetPatientEducation();
            }
            else {
                UnloadActionPan(Clinical_InfoButtonView.params.ParentCtrl, "Clinical_InfoButtonView");
            }
        }
        else {
            UnloadActionPan();
        }
    },

    GenerateInfoLink: function (searchstr, PanelID, bcase, parentControlPanelID, caller, PatientId) {
        caller = caller == null ? "" : caller;
        parentControlPanelID = parentControlPanelID == null ? "" : parentControlPanelID;
        return " <a onclick=\"Clinical_InfoButtonView.getInfofromMediPlus('" + searchstr + "', '" + PanelID + "','" + bcase + "','" + parentControlPanelID + "','" + caller + "','" + PatientId + "')\" style=\"cursor:pointer\"><b>(Info)</b></a>";
    },

    getInfofromMediPlus: function (searchstr, ParentCtrl, bcase, parentControlPanelID, caller, PatientId) {

        var code = "";
        if (bcase == 1) // Medication
            code = "2.16.840.1.113883.6.69";
        else if (bcase == 2) // Problems - Diagnostics
            code = "2.16.840.1.113883.6.90";
        else if (bcase == 3) // Lab Results
            code = "2.16.840.1.113883.6.1";

        var params = [];
        params["codeSystem"] = code;
        params["searchStr"] = searchstr;
        params["PatientId"] = $('#PatientProfile #hfPatientId').val() != "" ? $('#PatientProfile #hfPatientId').val() : PatientId;
        params["FromAdmin"] = 0;

        params["ParentCtrl"] = ParentCtrl;
        params["UserName"] = globalAppdata.AppUserName;

        if (caller != "") {
            params["caller"] = caller;
        }

        if (parentControlPanelID != "") {
            params["ParentCtrlPanelID"] = parentControlPanelID;
            LoadActionPan('Clinical_InfoButtonView', params, parentControlPanelID);
        }
        else
            LoadActionPan('Clinical_InfoButtonView', params);
    },

    //Function Name: medlineLinks
    //Author Name: Humaira Yousaf
    //Created Date: 22-07-2016
    //Description: medlineLinks
    medlineLinks: function () {

        $("#pnlClinical_InfoButtonView #OpenHTMLDocument").find('a').each(function (index, item) {
            $(item).removeAttr('onclick');
            var url;
            if ($(item).attr('href') != null) {
                url = $(item).attr('href');
            }
            else {
                url = $(item).attr('refURL');
            }

            var name = $(item).text().trim();

            if (url != null) {
                if (url.indexOf('medlineplus') != -1) {
                    $(item).removeAttr('href');
                    $(item).removeAttr('rel');
                    $(item).removeAttr('target');
                }
                else {
                    if (url.match('^./')) {
                        url = 'https://medlineplus.gov/ency/article' + url.substring(1, url.length);
                        $(item).removeAttr('href');
                    }
                }

                $(item).attr("onclick", "Clinical_InfoButtonView.appendLinksResponse('" + url + "', '" + name + "');return false;");
                $(item).attr("refURL", url);
                $(item).attr("style", "cursor:pointer");
                $(item).attr("class", "btn btn-link btn-xs");

            }

        });
    },
    //Function Name: appendLinksResponse
    //Author Name: Humaira Yousaf
    //Created Date: 22-07-2016
    //Description: append response links
    appendLinksResponse: function (url, name) {

        if ($("#pnlClinical_InfoButtonView").find('article[name="' + name + '"]').length <= 0) {

            Clinical_InfoButtonView.getLinkResponse(url).done(function (response) {
                response = JSON.parse(response);
                if (response.error !== '') {
                    var linkResponseDiv = document.createElement('div');
                    $(linkResponseDiv).html(response.data);
                    $(linkResponseDiv).find("article").attr('name', name);

                    if (Clinical_InfoButtonView.params.ParentCtrl == 'Clinical_Medications') {
                        $("#pnlClinical_InfoButtonView #drugs").append($(linkResponseDiv).find("article"));
                        $("#pnlClinical_InfoButtonView #drugs").find(".page-actions").remove();
                    }
                    else {
                        $("#pnlClinical_InfoButtonView #AllContent").append($(linkResponseDiv).find("article"));
                        $("#pnlClinical_InfoButtonView #AllContent").find(".page-actions").remove();
                    }

                    Clinical_InfoButtonView.medlineLinks();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Function Name: getLinkResponse
    //Author Name: Humaira Yousaf
    //Created Date: 22-07-2016
    //Description: gets response
    getLinkResponse: function (url) {
        var objData = {};
        objData["LinkURL"] = url;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "InfoButton", "GetLinksResponse");
    },

    //Function Name: printInfo
    //Author Name: Humaira Yousaf
    //Created Date: 29-07-2016
    //Description: Prints medline response with single header and footer
    printInfo: function () {
        var docType = '<!doctype html>';
        var infohtml = $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument').clone();

        $(infohtml).find('title').remove();
        $(infohtml).find('link').remove();
        $(infohtml).find('#btnDownload').remove();
        $(infohtml).find('#btnPrinter').remove();
        $(infohtml).find('#btnEmail').remove();


        var headerHtml = $($(infohtml).find('#mplus-wrap')[0]).html();
        var infoText = $($(infohtml).find('#AllContent')[0]);

        $(infoText).find(".results").removeAttr('style');
        $(infoText).find("#drugs").removeAttr('style');

        var generatedBy = $(infohtml).find("#ptEduFooter").text();
        var footerHtml = '<div class="divFooter"><div class="blueBgPrint whiteColorPrint" style="float:left;width:100%;background:#005da9 !important; color:#fff !important;padding:0px 25px;"> ' +
                                       generatedBy + ' <span class="whiteColorPrint" style="float:right;"></span></div></div><div class="clearfix"></div>';
        Clinical_ReportHeader.ReportHeaderPrint_DbCall(-1, Clinical_InfoButtonView.params.PatientId, 'Patient Education').done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var contents = '';
                if (response.Header == "" && response.Footer == "") {
                    var a = $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument').clone();
                    var b = a.find('table')[1];
                    $(b).find('#btnDownload').remove();
                    $(b).find('#btnPrinter').remove();
                    $(b).find('#btnEmail').remove();
                    //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    $(b).attr("style", "width:850px;")
                    //END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
                    if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList").length <= 0) {
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall ").append('<div id="PatientInfo" class="panel-body panel-featured"><div class="col-sm-4 col-lg-2 pull-left"><img src="content/images/SHS-nav-logo-small-100.png" class="img-responsive"></div><div class="clearfix"></div><div class="splitter m-none mt-xs"><div class="spacer3"></div></div> <ul class="list-unstyled pull-left line-height-fix" id="PatientList"><li id="PatientName"></li><li id="PatientAge"></li><li id="PatientAccount"></li><li id="PatientPhone"></li><li id="PatientAddress"></li></ul></div>');
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList").html($(b));
                    } else {
                        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList").html($(b));
                    }
                    contents = $(infoText).html();
                } else {
                    contents = $(infoText).html();
                }

                while (contents.indexOf("<div>&nbsp;</div>") > -1) {
                    contents = contents.replace("<div>&nbsp;</div>", "");
                }
                var docHead = '<head><title></title> <link rel="stylesheet" href="Content/medlineplus/connect.css" /><link rel="stylesheet" href="Content/Blue/theme.css" />'
               + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" />'
               + '</head>';

                contents = docType + '<html>' + docHead + '<body style="background:floralWhite !important;">' + response.Header + contents + response.Footer + '</body></html>';

                //contents = response.Header + contents + response.Footer;
                Clinical_InfoButtonView.getPrintnotePDF(response, contents, headerHtml);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    getPrintnotePDF: function (response, contents, headerHTML) {

        var Logo = '';
        var logoHTML = $(headerHTML);
        Logo = logoHTML.find('img').prop('src');
        if ($('#' + Clinical_InfoButtonView.params["PanelID"] + ' #printcall:visible').length == 0) {
            $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #printcall').css('display', '');
        }
        $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #printcall').append("<div id='ulContenttemp'></div>");
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ulContenttemp").html(contents);
        //$('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ulContent").html(contents);
        var imgLength = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall img").length;
        var images = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall img");
        var HeaderLogo = '';
        $.each(images, function (i, item) {
            if (item.src == Logo) {
                HeaderLogo = item.src;
            }
        });

        var FooterText = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall footer").text().split('Generated by: ').join('');

        if (HeaderLogo !== null && HeaderLogo !== "") {
            var CustomLogo = '<img id="ClinicalReportsHeaderLogo" src="' + HeaderLogo + '   " class="img-responsive" style="width: 100px;">';
            var insideHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2354
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', HeaderLogo).css({ "max-width": "350px", "max-height": "140px" });
            //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2354
            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html(PageTemp);
        }
        else {
            var defaultLogo = 'content/images/SHS-nav-logo-small-100.png';
            var insideHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', defaultLogo);
            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html(PageTemp);
        }

        // ----- footer
        if (FooterText !== null && FooterText !== "") {

            var insideHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooter').text(FooterText);
            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html(PageTemp);

        }
        else {
            var footerText = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall  footer").text().split('Generated by: ').join('');
            var insideHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooter').text(footerText);
            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html(PageTemp);

        }
        //PracticeDiv start
        var practiceHTML = '';
        if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PracticeList").length > 0) {

            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PracticeList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PracticeList > li").length > 7) {
                $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PracticeList > li:nth-child(7)").nextAll("li").remove();
            }
            practiceHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PracticeList").length == 1 ? $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PracticeList")[0].outerHTML : $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PracticeList")[1].outerHTML;
        }
        var insideHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html();
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#PracticeDiv').html(practiceHTML.split('#').join('\\#'));
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html(PageTemp);

        //PracticeDiv end
        //PatientList start
        var patientHTML = '';
        if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList").length > 0) {
            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList > li").length > 7) {
                $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList > li:nth-child(7)").nextAll("li").remove();
            }
            patientHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList").length == 1 ? $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList")[0].outerHTML : $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientList")[1].outerHTML;
            patientHTML = patientHTML.replace("12:00AM", "");
        }

        var insideHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html();
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#PatientDiv').html(patientHTML.split('#').join('\\#'));
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html(PageTemp);

        //PatientList end
        //ProviderDiv start
        var providerHTML = '';
        if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ProviderList").length > 0) {

            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ProviderList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ProviderList > li").length > 7) {
                $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ProviderList > li:nth-child(7)").nextAll("li").remove();
            }

            providerHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ProviderList").length == 1 ? $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ProviderList")[0].outerHTML : $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ProviderList")[1].outerHTML;
        }

        var insideHTML = $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html();;
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#ProviderDiv').html(providerHTML.split('#').join('\\#'));
        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
        $(PageTemp).find('#PracticeList li').addClass('text-right');
        $(PageTemp).find('#ProviderList li').addClass('text-right');
        //Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html(PageTemp);
        var topMargin = 0;

        var height = 0;

        if ($.trim(response.Footer) != "" && response.Header != "") {

            var img = new Image();
            img.src = $(PageTemp).find('#ClinicalReportsHeaderLogo').attr('src');
            var srcWidth = img.width;
            var srcHeight = img.height;
            var maxWidth = 350;
            var maxHeight = 140;

            var headerImgHeight = 0;

            var topMargin = 0;
            var height = 0;

            var pracHeight = $(PageTemp).find('#PracticeDiv ul li:not(:empty)').length * 4.68;
            var logoHeight = 0;
            if (srcHeight > maxHeight) {
                headerImgHeight = Clinical_InfoButtonView.calculateHeaderImageSize(srcWidth, srcHeight, maxWidth, maxHeight).height;
                logoHeight = headerImgHeight * 0.26;
            }
            else {
                logoHeight = 140 * 0.26;
            }

            if (pracHeight < logoHeight) {
                pracHeight = logoHeight;
            }

            var ProvHeight = $(PageTemp).find('#ProviderDiv ul li:not(:empty)').length * 4.68;
            var patHeight = $(PageTemp).find('#PatientDiv ul li:not(:empty)').length * 4.68;
            if (patHeight > ProvHeight) {
                height += pracHeight + patHeight;
            } else {
                height += pracHeight + ProvHeight;
            }

            if ($(PageTemp).find('#ProviderDiv ul li').length == 7 || $(PageTemp).find('#PatientDiv ul li').length == 7) {
                height = height - 2;
            }
        }
        else {
            var a = $('.results')[0];
            // height = $(a).find('table:first').height() + $(a).find('table:last').height() - 18;
            var ht = $(a).height();
            height = ht / 4;
            height = height - 8;

        }
        topMargin = height;


        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientInfo").remove();

        if ($('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template .blueBorderPrint").length >= 1) {
            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall .form-group").hide();
            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall footer").hide();
        }

        //if (utility.UserBrowser() == "IE" || utility.UserBrowser() == "Chrome" || utility.UserBrowser() == "Chrome") {
        Clinical_InfoButtonView.printPatientEducationInfo('<div class="blueBorderPrint">' + $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template #headerTemplate").html() + "</div>" + $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall")[0].outerHTML);
        $("#printcall").css("display", 'none');
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientInfo").hide();
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall footer").hide();
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall").hide();
        $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ulContenttemp").remove();
        //} else {
        //    kendo.drawing.drawDOM('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall", {
        //        landscape: false,
        //        scale: 0.6,
        //        paperSize: "A4",
        //        margin: {
        //            left: "10mm",
        //            top: topMargin + "mm",
        //            right: "10mm",
        //            bottom: "35mm"
        //        },
        //        template: $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html()
        //    }).then(function (group) {


        //        kendo.drawing.pdf.toDataURL(group, function (dataURL) {
        //            var params = [];
        //            params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
        //            params["PreviewPdf"] = true;
        //            //utility.PDFViewer(params["PrintPDFDataURL"], true, '#pnlClinical_InfoButtonView #PreviewReportPrint', false, true);
        //            utility.documentPrint(params["PrintPDFDataURL"]);
        //            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #PatientInfo").hide();
        //            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall footer").hide();
        //            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall").hide();
        //            $('#' + Clinical_InfoButtonView.params["PanelID"] + " #printcall #ulContenttemp").remove();
        //        });
        //    });
        //}
        // ------------------------------------- End Download functionality--------------------------------------
    },

    printPatientEducationInfo: function (contents) {
        var frame1 = $('<iframe />');
        frame1[0].name = "Clinical_InfoButtonPrint";
        frame1.attr("scrolling", "no");
        frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden", "margin": "0", "padding": "0" });
        $("body").append(frame1);
        var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
        frameDoc.document.open();
        frameDoc.document.write('<html><head><title></title>');
        frameDoc.document.write('</head><body>');
        //Append the external CSS file.
        frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Default/print-media.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
        //Append the DIV contents.
        frameDoc.document.write("<div style='bottom:20px !important;'>" + contents + "</div>");
        frameDoc.document.write('<div style="position:fixed;bottom:1px !important;font-size:8px !important; width:98%;background: #005da9 !important;height: 10px;"> <div style="margin-top: -6px;padding-left: 3px !important; color: #fff !important;">Generated By: ' + $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template #ClinicalReportsFooter").text() + '</div></div>');
        frameDoc.document.write('</body></html>');
        frameDoc.document.close();
        if (utility.UserBrowser() == "IE") {
            setTimeout(function () {
                window.frames[frame1[0].name].document.execCommand('print', false, null);
                frame1.remove();
            }, 200);
        } else {
            setTimeout(function () {
                window.frames[frame1[0].name].focus();
                window.frames[frame1[0].name].print();
                frame1.remove();
            }, 500);
        }
    },

    calculateHeaderImageSize: function (srcWidth, srcHeight, maxWidth, maxHeight) {

        var ratio = Math.min(maxWidth / srcWidth, maxHeight / srcHeight);
        return { width: srcWidth * ratio, height: srcHeight * ratio };
    },

    SendEmail: function (patientID) {

        setTimeout(function () {

            var docType = '<!doctype html>';

            var infohtml = $('#' + Clinical_InfoButtonView.params["PanelID"] + ' #OpenHTMLDocument').clone();

            $(infohtml).find('a').each(function (index, item) {
                $(item).removeAttr('onclick');
                $(item).attr('href', $(item).attr("refURL"));
            });

            $(infohtml).find('title').remove();
            $(infohtml).find('link').remove();
            $(infohtml).find('#btnDownload').remove();
            $(infohtml).find('#btnPrinter').remove();
            $(infohtml).find('#btnEmail').remove();


            var headerHtml = $($(infohtml).find('#mplus-wrap')[0]).html();
            var infoText = $($(infohtml).find('#AllContent')[0]);

            $(infoText).find(".results").removeAttr('style');
            $(infoText).find("#drugs").removeAttr('style');

            var generatedBy = $(infohtml).find("#ptEduFooter").text();
            var footerHtml = '<div class="divFooter"><div class="blueBgPrint whiteColorPrint" style="float:left;width:100%;background:#005da9 !important; color:#fff !important;padding:0px 25px;"> ' +
                                           generatedBy + '</span></div></div><div class="clearfix"></div>';

            var docHead = '<head><title></title> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="https://apps.nlm.nih.gov/medlineplus/services/css/connect.css" /><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                + '<style> div.divFooter { position: fixed; bottom: 0; width: 100%;}'
                + '</style>'
                + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                + '</script>'
                + '</head>';

            var emailBody = docType + '<html>' + docHead + '<body style="background:floralWhite !important;">' + headerHtml + $(infoText).html() + footerHtml + '</body></html>';

            Clinical_InfoButtonView.EmailSend(patientID, emailBody).done(function (response) {
                response = JSON.parse(response);
                if (response.error !== '') {
                    utility.DisplayMessages(response.error, 4);
                }
                else {
                    utility.DisplayMessages("Email has been sent successfully.", 1);
                }
            });

        }, 100);

    },
    EmailSend: function (patientID, emailBody) {
        var objData = {};
        objData["PatientId"] = patientID;
        objData["Body"] = emailBody;
        objData["EmailAddress"] = params.PatientEmail;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "InfoButton", "SendEmail");
    },


    getPatientDocument: function (PatientID, PatDocIDs) {
        Clinical_InfoButtonView.getPatientDocumentDBCall(PatientID, PatDocIDs).done(function (response) {

            if (response.status !== false) {
                var DocumentLoad = JSON.parse(response.DocumentLoad_JSON);

                $("#pnlClinical_InfoButtonView #OpenHTMLDocument").html(atob(DocumentLoad.Base64FileStream));

                $($("#pnlClinical_InfoButtonView .results")[1]).css("min-height", '400px');
                $($("#pnlClinical_InfoButtonView .results")[1]).css("height", '400px');

                $($("#pnlClinical_InfoButtonView .results")[1]).css("overflow-y", "auto");

                Clinical_InfoButtonView.medlineLinks();

                $("#pnlClinical_InfoButtonView #drugs").removeClass("results");
                $("#pnlClinical_InfoButtonView #drugs ul").replaceWith($("#pnlClinical_InfoButtonView #drugs ul").contents());
                $("#pnlClinical_InfoButtonView #drugs li").replaceWith($("#pnlClinical_InfoButtonView #drugs li").contents());


            }
            else
                utility.DisplayMessages(response.Message, 3);

        });
    },
    getPatientDocumentDBCall: function (PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&bFileStream=1";
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_DOCUMENT");
    },
    SaveDocument: function (data) {

        return MDVisionService.APIServiceSyncCall(data, "InfoButton", "GetInfobuttonDetail");
    }
}
