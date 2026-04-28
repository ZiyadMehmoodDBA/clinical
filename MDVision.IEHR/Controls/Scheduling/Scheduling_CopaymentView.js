Scheduling_CopaymentView = {
    bIsFirstLoad: true,
    params: [],
    pdf: '',
    Load: function (params) {
        Scheduling_CopaymentView.params = params;
        Scheduling_CopaymentView.pdf = "";

        if (Scheduling_CopaymentView.params == null) {
            Scheduling_CopaymentView.params = [];
        }
        if (Scheduling_CopaymentView.params.PanelID != "pnlScheduling_CopaymentView") {
            Scheduling_CopaymentView.params["PanelID"] = "pnlScheduling_CopaymentView";
        }

        if (Scheduling_CopaymentView.bIsFirstLoad) {
            Scheduling_CopaymentView.bIsFirstLoad = false;

            Scheduling_CopaymentView.CopayPreview(Scheduling_CopaymentView.params.PaymentId, true);
        }
    },

    CopayPreview: function (PaymentId, IsPreview) {


        var dfd = new $.Deferred();
        Scheduling_CopaymentView.PreviewReceipt(PaymentId).done(function (response) {

            if (response.status == true) {

                var receiptInfo = response.PaymentReceiptInfo_JSON;
                    //receiptInfo = receiptInfo[0];

                var dfd_html = new $.Deferred();

                if (IsPreview == true) {
                    var $html = $("#" + Scheduling_CopaymentView.params["PanelID"]);
                    dfd_html.resolve($html);
                }
                else {

                    var ajax_get = $.get("./Controls/Scheduling/Scheduling_CopaymentView.html", {
                        cache: false
                    }, function (content) {
                        var $html = $(content);
                        $("body").append($html);
                        dfd_html.resolve($html);

                    }, "html");
                }

                dfd_html.then(function ($html) {
                    var MultiCheckTypeRecord = false;   //set flag for multiple records, payment type 'check' 
                    // check payment types
                    var getPaymentTypes = receiptInfo.map(function (a) { return a.PaymentType; });
                    // paymentType='check',to decide payment-type ui will be table column or label
                    if (getPaymentTypes.length > 1) {
                        MultiCheckTypeRecord = getPaymentTypes.every(function (value, index, array) {
                            return value === 'Check';
                        });
                    }
                    var sortedItems = $.unique(getPaymentTypes).sort(function (a, b) {         // unique payment types
                        return a - b;
                    });
                   var $copay = 0, app_date = null,practiceName ="", practiceAddress="",practiceCity = "", practiceContact = "",patientName = "",patientAccount = "",
                       reciptNumber = "", tblCharges = $html.find("#frmSchedulingCopaymentView #tblCharges > tbody"),ProviderName = "",FacilityName = "",
                       FacilityAddress = "", FacilityCity = "", res_date = utility.RemoveTimeFromDate(null, Date()), facilityContact = "", Comments = "", CreatedBy = "", CreatedOn="";
                    var row = '<tr></tr>';
                    $.each(receiptInfo, function (i, item) {
                        if (parseFloat(item.CopayAmount)>0)
                            $copay = parseFloat(item.CopayAmount) + parseFloat($copay);
                        app_date = item.AppointmentDate;
                        practiceName = item.PracticeName;
                        practiceAddress = item.PracticeAddress;
                        practiceCity = item.practiceCity;
                        practiceContact = (item.PracticePhoneNo != "" ? "Phone: " + item.PracticePhoneNo : "") + " " + (item.PracticeFax != "" ? "Fax: " + item.PracticeFax : "");
                        patientName = item.PatientName;
                        patientAccount = item.AccountNumber;
                        if (i == 0)
                        {
                            reciptNumber = item.ReceiptNumber;
                            Comments = item.Comments;
                        }
                        else
                        {
                            reciptNumber = reciptNumber + ", " + item.ReceiptNumber;
                            Comments = Comments + ", " + item.Comments;
                        }
                        if (item.PaymentType == "Check") {
                            item.PaymentType = item.PaymentType + " -" + item.CheckNo;
                        }
                        if (sortedItems.length > 1 || MultiCheckTypeRecord==true )
                        {
                            
                            $html.find("#printcall #PatientList li").last().hide();
                            $html.find("#frmSchedulingCopaymentView #tblCharges >thead tr th").eq(1).show();
                          
                            $(tblCharges).append('<tr><td>' + res_date + '</td><td>' + item.PaymentType + '</td><td>' + item.PaidAccountType + '</td><td>' + "$" + parseFloat(item.CopayAmount).toFixed(2) + '</td></tr>');
                        }
                        else {
                            $html.find("#printcall #PatientList li").last().show();
                            $html.find("#frmSchedulingCopaymentView #tblCharges >thead tr th").eq(1).hide();
                            $html.find("#frmSchedulingCopaymentView #PatientType").html(item.PaymentType);
                            $(tblCharges).append('<tr><td>' + res_date + '</td><td>' + item.PaidAccountType + '</td><td>' + "$" + parseFloat(item.CopayAmount).toFixed(2) + '</td></tr>');
                             }
                        ProviderName = item.ProviderName;
                        FacilityName = item.FacilityName;
                        FacilityAddress = item.FacilityAddress;
                        FacilityCity = item.FacilityCity;
                        facilityContact = (receiptInfo.FacilityPhoneNo ? "Phone# " + receiptInfo.FacilityPhoneNo : "") + " " + (receiptInfo.FacilityFax ? "Fax# " + receiptInfo.FacilityFax : "");
                        CreatedBy = item.CreatedBy;
                        CreatedOn = item.CreatedOn;
                    });
                    $copay = $copay ? "$" + parseFloat($copay).toFixed(2) : "$0.00";
                    if (sortedItems.length > 1 || MultiCheckTypeRecord == true)
                        $(tblCharges).append('<tr><td></td><td></td><td>Total</td><td>' + $copay + '</td></tr>');
                    else
                        $(tblCharges).append('<tr><td></td><td>Total</td><td>' + $copay + '</td></tr>');
                    //$copay = receiptInfo.CopayAmount ? "$" + parseFloat(receiptInfo.CopayAmount).toFixed(2) : "$0.00";

                    //var res_date = receiptInfo.PaymentDate == null ? "N/A" : utility.RemoveTimeFromDate(null, receiptInfo.PaymentDate);
                    
                    
                    //var app_date = receiptInfo.AppointmentDate == null ? "N/A" : receiptInfo.AppointmentDate;

                    //$html.find("#frmSchedulingCopaymentView #PracticeName").html('<b>' + receiptInfo.PracticeName + '</b>');
                    //$html.find("#frmSchedulingCopaymentView #PracticeAddress").html(receiptInfo.PracticeAddress);
                    //$html.find("#frmSchedulingCopaymentView #PracticeCity").html(receiptInfo.PracticeCity);
                    $html.find("#frmSchedulingCopaymentView #PracticeName").html('<b>' + practiceName + '</b>');
                    $html.find("#frmSchedulingCopaymentView #PracticeAddress").html(practiceAddress);
                    $html.find("#frmSchedulingCopaymentView #PracticeCity").html(practiceCity);
                   // var practiceContact = (receiptInfo.PracticePhoneNo != "" ? "Phone: " + receiptInfo.PracticePhoneNo : "") + " " + (receiptInfo.PracticeFax != "" ? "Fax: " + receiptInfo.PracticeFax : "");
                    $html.find("#frmSchedulingCopaymentView #PracticeContact").html(practiceContact);

                    //$html.find("#frmSchedulingCopaymentView #PatientName").html(receiptInfo.PatientName);
                    //$html.find("#frmSchedulingCopaymentView #PatientAccount").html(receiptInfo.AccountNumber);
                    $html.find("#frmSchedulingCopaymentView #PatientName").html(patientName);
                    $html.find("#frmSchedulingCopaymentView #PatientAccount").html(patientAccount);
                    $html.find("#frmSchedulingCopaymentView #AmountPaid").html($copay);
                    //$html.find("#frmSchedulingCopaymentView #PatientType").html(receiptInfo.PaymentType);


                    $html.find("#frmSchedulingCopaymentView #AppointmentDate").html(app_date);
                    $html.find("#frmSchedulingCopaymentView #ReceiptDate").html(res_date);
                    //$html.find("#frmSchedulingCopaymentView #ReceiptNumber").html(receiptInfo.ReceiptNumber);
                    $html.find("#frmSchedulingCopaymentView #ReceiptNumber").html(reciptNumber);
                    
                    //var tblCharges = $html.find("#frmSchedulingCopaymentView #tblCharges > tbody");
                    //var row = '<tr></tr>';
                    //$(tblCharges).append('<tr><td>' + res_date + '</td><td>' + receiptInfo.PaidAccountType + '</td><td>' + $copay + '</td></tr>');
                    //$(tblCharges).append('<tr><td style="padding:15px"></td><td></td><td></td></tr>');
                    //$(tblCharges).append('<tr><td></td><td>Total</td><td>' + $copay + '</td></tr>');

                    //$html.find("#frmSchedulingCopaymentView #ProviderName").html(receiptInfo.ProviderName);

                    //$html.find("#frmSchedulingCopaymentView #FacilitytName").html(receiptInfo.FacilityName);
                    //$html.find("#frmSchedulingCopaymentView #FacilityAddress").html(receiptInfo.FacilityAddress);
                    //$html.find("#frmSchedulingCopaymentView #FacilityCity").html(receiptInfo.FacilityCity);

                    //var facilityContact = (receiptInfo.FacilityPhoneNo ? "Phone# " + receiptInfo.FacilityPhoneNo : "") + " " + (receiptInfo.FacilityFax ? "Fax# " + receiptInfo.FacilityFax : "");
                    //$html.find("#frmSchedulingCopaymentView #FacilityContact").html(facilityContact);

                    //$html.find("#frmSchedulingCopaymentView #Comments").html(receiptInfo.Comments);
                    //$html.find("#frmSchedulingCopaymentView #ProcessedBy").html(receiptInfo.CreatedBy + ' ' + (receiptInfo.CreatedOn == null ? '' : receiptInfo.CreatedOn));
                    $html.find("#frmSchedulingCopaymentView #ProviderName").html(ProviderName);
                    $html.find("#frmSchedulingCopaymentView #FacilitytName").html(FacilityName);
                    $html.find("#frmSchedulingCopaymentView #FacilityAddress").html(FacilityAddress);
                    $html.find("#frmSchedulingCopaymentView #FacilityCity").html(FacilityCity);
                    $html.find("#frmSchedulingCopaymentView #FacilityContact").html(facilityContact);
                    $html.find("#frmSchedulingCopaymentView #Comments").html(Comments);
                    $html.find("#frmSchedulingCopaymentView #ProcessedBy").html(CreatedBy + ' ' + (CreatedOn == null ? '' : CreatedOn));
                    
                    BackgroundLoaderShow(true);
                    Scheduling_CopaymentView.getPrintPDF($html, IsPreview, receiptInfo).done(function () {

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
                    Scheduling_CopaymentView.pdf = params["PrintPDFDataURL"];

                    if (isPreview)
                        utility.PDFViewer(params["PrintPDFDataURL"], false, 'pnlScheduling_CopaymentView #PreviewPrintReceipt', true);

                    if (Scheduling_CopaymentView.params["isSaveReceiptDoc"] == true) {
                        var VisitId = Scheduling_CopaymentView.params["VisitId"] ? Scheduling_CopaymentView.params["VisitId"] : 0;
                        Scheduling_UnallocatedCopayment.SaveCopayReceiptInFolder(VisitId, receiptInfo.PaymentDate, receiptInfo.ReceiptNumber, receiptInfo.PatientId, Scheduling_CopaymentView.pdf).done(function (res) {
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

    PreviewReceipt: function (PaymentId) {

        var data = "PaymentId=" + PaymentId;
        return MDVisionService.defaultService(data, "SCHEDULING_COPAYMENT", "LOAD_RECEIPT_INFO");
    },

    UnLoad: function () {

        Scheduling_CopaymentView.pdf = "";
        UnloadActionPan(Scheduling_CopaymentView.params["ParentCtrl"], "actionPanSchedulingCopaymentView");

    },

    PrintReceipt: function () {
        var params = [];
        params["PrintPDFDataURL"] = Scheduling_CopaymentView.pdf;
        params["PreviewPdf"] = true;
        utility.documentPrint(params["PrintPDFDataURL"]);
    },
    
}