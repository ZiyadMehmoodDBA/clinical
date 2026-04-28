Unallocated_CopaymentView = {
    bIsFirstLoad: true,
    params: [],
    pdf: '',
    Load: function (params) {
        Unallocated_CopaymentView.params = params;
        Unallocated_CopaymentView.pdf = "";

        if (Unallocated_CopaymentView.params == null) {
            Unallocated_CopaymentView.params = [];
        }
        if (Unallocated_CopaymentView.params.PanelID != "Unallocated_CopaymentView") {
            Unallocated_CopaymentView.params["PanelID"] = "Unallocated_CopaymentView";
        }

        if (Unallocated_CopaymentView.bIsFirstLoad) {
            Unallocated_CopaymentView.bIsFirstLoad = false;

            Unallocated_CopaymentView.UnallocatedCopayPreview(Unallocated_CopaymentView.params.UnallocatedCopaymentId, true);
        }
    },

    UnallocatedCopayPreview: function (unallocatedCopayId, IsPreview) {


        var dfd = new $.Deferred();
        Unallocated_CopaymentView.PreviewReceipt(unallocatedCopayId).done(function (response) {

            if (response.status == true) {

                var receiptInfo = JSON.parse(response.ReceiptInfo);

                var dfd_html = new $.Deferred();

                if (IsPreview == true) {
                    var $html = $("#" + Unallocated_CopaymentView.params["PanelID"]);
                    dfd_html.resolve($html);
                }
                else {

                    var ajax_get = $.get("./Controls/Scheduling/Scheduling_Unallocated_CopaymentView.html", {
                        cache: false
                    }, function (content) {
                        var $html = $(content);
                        $("body").append($html);
                        dfd_html.resolve($html);

                    }, "html");
                }

                dfd_html.then(function ($html) {

                    var $copay = receiptInfo.CopayAmount ? "$" + parseFloat(receiptInfo.CopayAmount).toFixed(2) : "$0.00";
                    var res_date = receiptInfo.ReceiptDate == null ? "N/A" : utility.RemoveTimeFromDate(null, receiptInfo.ReceiptDate);
                    var app_date = receiptInfo.AppointmentDate == null ? "N/A" : receiptInfo.AppointmentDate;

                    $html.find("#frmUnallocatedCopaymentView #PracticeName").html('<b>' + receiptInfo.PracticeName + '</b>');
                    $html.find("#frmUnallocatedCopaymentView #PracticeAddress").html(receiptInfo.PracticeAddress);
                    $html.find("#frmUnallocatedCopaymentView #PracticeCity").html(receiptInfo.PracticeCity);

                    var practiceContact = (receiptInfo.PracticePhoneNo != "" ? "Phone: " + receiptInfo.PracticePhoneNo : "") + " " + (receiptInfo.PracticeFax != "" ? "Fax: " + receiptInfo.PracticeFax : "");
                    $html.find("#frmUnallocatedCopaymentView #PracticeContact").html(practiceContact);

                    $html.find("#frmUnallocatedCopaymentView #PatientName").html(receiptInfo.PatientName);
                    $html.find("#frmUnallocatedCopaymentView #PatientAccount").html(receiptInfo.AccountNumber);
                    $html.find("#frmUnallocatedCopaymentView #AmountPaid").html($copay);
                    $html.find("#frmUnallocatedCopaymentView #PatientType").html(receiptInfo.PaymentType);


                    $html.find("#frmUnallocatedCopaymentView #AppointmentDate").html(app_date);
                    $html.find("#frmUnallocatedCopaymentView #ReceiptDate").html(res_date);
                    $html.find("#frmUnallocatedCopaymentView #ReceiptNumber").html(receiptInfo.ReceiptNumber);

                    var tblCharges = $html.find("#frmUnallocatedCopaymentView #tblCharges > tbody");
                    var row = '<tr></tr>';
                    $(tblCharges).append('<tr><td>' + res_date + '</td><td>' + receiptInfo.PaidAccountType + '</td><td>' + $copay + '</td></tr>');
                    $(tblCharges).append('<tr><td style="padding:15px"></td><td></td><td></td></tr>');
                    $(tblCharges).append('<tr><td></td><td>Total</td><td>' + $copay + '</td></tr>');

                    $html.find("#frmUnallocatedCopaymentView #ProviderName").html(receiptInfo.ProviderName);

                    $html.find("#frmUnallocatedCopaymentView #FacilitytName").html(receiptInfo.FacilityName);
                    $html.find("#frmUnallocatedCopaymentView #FacilityAddress").html(receiptInfo.FacilityAddress);
                    $html.find("#frmUnallocatedCopaymentView #FacilityCity").html(receiptInfo.FacilityCity);

                    var facilityContact = (receiptInfo.FacilityPhoneNo ? "Phone# " + receiptInfo.FacilityPhoneNo : "") + " " + (receiptInfo.FacilityFax ? "Fax# " + receiptInfo.FacilityFax : "");
                    $html.find("#frmUnallocatedCopaymentView #FacilityContact").html(facilityContact);

                    $html.find("#frmUnallocatedCopaymentView #Comments").html(receiptInfo.Comments);
                    $html.find("#frmUnallocatedCopaymentView #ProcessedBy").html(receiptInfo.CreatedByName + ' ' + (receiptInfo.CreatedOn == null ? '' : receiptInfo.CreatedOn));


                    BackgroundLoaderShow(true);
                    Unallocated_CopaymentView.getPrintPDF($html, IsPreview, receiptInfo).done(function () {

                        if (IsPreview == false)
                            $("body").find($html).remove();

                        BackgroundLoaderShow(false);
                        dfd.resolve();

                    });

                });
            }
        });
        return dfd.promise();
    },

    getPrintPDF: function ($obj, isPreview, receiptInfo) {

        var def = $.Deferred();
        setTimeout(function () {

          
            $obj.find("#printcall").show();
            $obj.find("#printcall").css("display", "inline");

            kendo.drawing.drawDOM($obj.find("#printcall"), {
                landscape: false,
                scale: 0.6,
                paperSize: "A4",
                margin: {
                    left: "3mm",
                    top: "3mm",
                    right: "3mm",
                    bottom: "3mm"
                },

            }).then(function (group) {

                kendo.drawing.pdf.toDataURL(group, function (dataURL) {

                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    Unallocated_CopaymentView.pdf = params["PrintPDFDataURL"];

                    if (isPreview)
                        utility.PDFViewer(params["PrintPDFDataURL"], false, 'Unallocated_CopaymentView #PreviewPrintReceipt', true);

                    if (Unallocated_CopaymentView.params["isSaveReceiptDoc"] == true) {
                        var VisitId = Unallocated_CopaymentView.params["VisitId"] ? Unallocated_CopaymentView.params["VisitId"] : 0;
                        Scheduling_UnallocatedCopayment.SaveCopayReceiptInFolder(VisitId, receiptInfo.ReceiptDate, receiptInfo.ReceiptNumber, receiptInfo.PatientId, Unallocated_CopaymentView.pdf).done(function (res) {
                            if (res.status != false) {
                                def.resolve();
                                utility.DisplayMessages("Receipt saved successfully.", 1);
                            }
                            else {
                                def.resolve();
                                utility.DisplayMessages(res.Message, 3);
                            }
                        });
                    }
                    else {
                        def.resolve();
                    }
                    $obj.find("#printcall").hide();
                });


            });

        }, 500);

        return def.promise();

    },

    PreviewReceipt: function (unallocatedCopayId) {

        var data = "UnAllocatedCopayId=" + unallocatedCopayId;
        return MDVisionService.defaultService(data, "SCHEDULING_UNALLOCATEDCOPAYMENT", "LOAD_RECEIPT_INFO");
    },

    UnLoad: function () {

        Unallocated_CopaymentView.pdf = "";
        UnloadActionPan(Unallocated_CopaymentView.params["ParentCtrl"], "actionPanUnallocatedCopayView");

    },

    PrintReceipt: function () {
        var params = [];
        params["PrintPDFDataURL"] = Unallocated_CopaymentView.pdf;
        params["PreviewPdf"] = true;
        utility.documentPrint(params["PrintPDFDataURL"]);
    },

}