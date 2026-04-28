Scheduling_ProviderAppointmentPrint = {
    bIsFirstLoad: true,
    params: [],
    pdf: '',
    printData: [],
    Load: function (params) {
        Scheduling_ProviderAppointmentPrint.params = params;
        Scheduling_ProviderAppointmentPrint.pdf = "";

        if (Scheduling_ProviderAppointmentPrint.params == null) {
            Scheduling_ProviderAppointmentPrint.params = [];
        }
        if (Scheduling_ProviderAppointmentPrint.params.PanelID != "pnlScheduling_ProviderAppointmentPrint") {
            Scheduling_ProviderAppointmentPrint.params["PanelID"] = "pnlScheduling_ProviderAppointmentPrint";
        }
        if (Scheduling_ProviderAppointmentPrint.params.IsResourceSelected == 1)
            $("#pnlScheduling_ProviderAppointmentPrint #titleId").html('Print Preview Resource Appointments');

        if (Scheduling_ProviderAppointmentPrint.bIsFirstLoad) {
            Scheduling_CopaymentView.bIsFirstLoad = false;
            var ProviderId = Scheduling_ProviderAppointmentPrint.params.ProviderId;
            var FacilityId = Scheduling_ProviderAppointmentPrint.params.FacilityId;
            var AppointmentDate = Scheduling_ProviderAppointmentPrint.params.AppointmentDate;
            var ResourceId = Scheduling_ProviderAppointmentPrint.params.ResourceId;
            Scheduling_ProviderAppointmentPrint.AppointmentPreview(ProviderId, FacilityId, AppointmentDate, true, ResourceId);
        }
        $("#pnlScheduling_ProviderAppointmentPrint #statusMultiselectPrint").kendoMultiSelect({
            dataSource: PMSScheduler.scheduleStatusDataSource,
            dataTextField: "name",
            dataValueField: "id",
            //placeholder: "Status",
            height: 400,
            itemTemplate: '<span class="k-state-default"><p>#: data.name #</p></span>'
            + '<span class="k-state-default" style="background-color:#:data.color#"></span>',
            footerTemplate: 'Total #: instance.dataSource.total() # items found',
            tagTemplate: kendo.template($("#tagStatusTemplate").html()),
            tagMode: "single",
            autoClose: false,
            change: function (e) {
                var values_ = $("#pnlScheduling_ProviderAppointmentPrint #statusMultiselectPrint").data("kendoMultiSelect").value();
                var sitem = $.grep(values_, function (itemm) {
                    return itemm == "0"
                });
                if ((values_.indexOf("0") == values_.length - 1) && sitem.length > 0) {
                    setTimeout(function () {
                        $("#statusMultiselectPrint_taglist span:first").html("All Status(es) Selected");
                    }, 100);
                    $("#pnlPMSScheduler #statusMultiselectPrint").data("kendoMultiSelect").value(sitem);
                } else if (sitem.length > 0) {
                    values_.splice(values_.indexOf("0"), 1);
                    $("#pnlPMSScheduler #statusMultiselectPrint").data("kendoMultiSelect").value(values_);
                }
            },
            dataBound: function (e) {
                var schStatusVals = $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").value();
                $("#pnlPMSScheduler #statusMultiselectPrint").data("kendoMultiSelect").value($("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").value());
                if (schStatusVals[0] == "0") {
                    $("#statusMultiselectPrint_taglist span:first").html("All Status(es) Selected");
                }
            }
        });
    },

    AppointmentPreview: function (ProviderId, FacilityId, AppointmentDate, IsPreview, ResourceId) {
        Scheduling_ProviderAppointmentPrint.PreviewReceipt(ProviderId, FacilityId, AppointmentDate, ResourceId).done(function (response) {
            if (response.status == true) {
                var receiptInfo = response.ProviderAppointmentPrintListInfo_JSON;
                Scheduling_ProviderAppointmentPrint.printData = receiptInfo;
                Scheduling_ProviderAppointmentPrint.getPreview(receiptInfo, IsPreview);
            }
        });
    },

    getPreview: function (receiptInfo, IsPreview) {
        var dfd_html = new $.Deferred();

        if (IsPreview == true) {
            var $html = $("#" + Scheduling_ProviderAppointmentPrint.params["PanelID"]);

            dfd_html.resolve($html);
        }
        else {
            var ajax_get = $.get("./Controls/Scheduling/Scheduling_ProviderAppointmentPrint.html", {
                cache: false
            }, function (content) {
                var $html = $(content);
                $("body").append($html);
                dfd_html.resolve($html);

            }, "html");
        }

        dfd_html.then(function ($html) {
            if (receiptInfo && receiptInfo.length) {
                $("#pnlScheduling_ProviderAppointmentPrint #AppointmentDetailOutter").html('');

                var getProvResNames, sortedProvResNames;
                if (Scheduling_ProviderAppointmentPrint.params.IsResourceSelected == 1) {
                    getProvResNames = receiptInfo.map(function (a) { return a.ResourceName; });
                }
                else {
                    getProvResNames = receiptInfo.map(function (a) { return a.ProviderName; });
                }

                sortedProvResNames = [...new Set(getProvResNames)].sort(function (a, b) { return a > b ? 1 : -1; });

                $.each(sortedProvResNames, function (provResIndex, provResElement) {
                    $("#pnlScheduling_ProviderAppointmentPrint #AppointmentDetailOutter").append("<div id='AppointmentDetail" + provResIndex + "'> " +
                                                       "<header class='appointment-header'>" +
                                                           "<div class='logo'>" +
                                                               "<img src='content/images/SHS-final-logo.png' class='img-responsive'>" +
                                                           "</div>" +
                                                           "<div class='appointment-detail'>" +
                                               "   <h2 id='ProviderName'>" + provResElement + "</h2>" +
                                               "  <h3>Today's Appointment</h3>" +
                                               " <p id='AppointmentDate'>" + receiptInfo[0].AppointmentDate + "</p>" +
                                               "</div>" +
                                               "</header>" +
                                                       "<br />" +
                                                   "</div>");

                    var getProvResAppts, getProvResFacilities, sortedfacilities;

                    if (Scheduling_ProviderAppointmentPrint.params.IsResourceSelected == 1) {
                        getProvResAppts = $.grep(receiptInfo, function (e) { return e.ResourceName == provResElement });
                    }
                    else {
                        getProvResAppts = $.grep(receiptInfo, function (e) { return e.ProviderName == provResElement });
                    }

                    getProvResFacilities = getProvResAppts.map(function (a) { return a.FacilityName; });
                    sortedfacilities = [...new Set(getProvResFacilities)].sort(function (a, b) { return a > b ? 1 : -1; });

                    $.each(sortedfacilities, function (facilityIndex, facilityDetail) {
                        var getFacilityAppts = $.grep(getProvResAppts, function (e) { return e.FacilityName == facilityDetail });
                        $.each(getFacilityAppts, function (k, FacilityDetail) {
                            if (k == 0) {
                                $($html).find("#AppointmentDetail" + provResIndex + "").append(
                                    "<table style='width:100%;'>" +
                                    "<tbody>" +
                                    "<tr class='appointment-location'>" +
                                    "<td><h3><strong>Location:</strong> " + FacilityDetail.FacilityName + "</h3>" +
                                    "</tr>" +
                                    "</tbody>"
                              );
                            }
                            Scheduling_ProviderAppointmentPrint.getAppointmentDetails($html, provResIndex, FacilityDetail);
                        });
                    });
                });

                BackgroundLoaderShow(true);
            }
            else {
                $("#pnlScheduling_ProviderAppointmentPrint #AppointmentDetailOutter").html('<h3 class="center"> No Record Found.</h3>');
            }
            Scheduling_ProviderAppointmentPrint.getPrintPDF($html, IsPreview, receiptInfo).done(function () {
                if (IsPreview == false)
                    $("body").find($html).remove();
                BackgroundLoaderShow(false);
            });
        });
    },
    getAppointmentDetails: function ($html, provResIndex, FacilityDetail) {
        if (Scheduling_ProviderAppointmentPrint.params.IsResourceSelected == 1) {
            $($html).find("#AppointmentDetail" + provResIndex + "").append(
                "<table class='appointment-table'>" +
                    "<tbody>" +
                        "<tr>" +
                            "<td>" +
                                '<div class="appointment-title"><strong>Patient Account:</strong><span>' + FacilityDetail.AccountNumber + '</span></div>' +
                                '<div class="appointment-title"><strong>Patient Name:</strong><span>' + FacilityDetail.PatientName + '</span></div>' +
                                '<div class="appointment-title"><strong>Patient Type:</strong><span>' + FacilityDetail.PatientType + '</span></div>' +
                                '<div class="appointment-title"><strong>Visit Type:</strong><span>' + FacilityDetail.VisitType + '</span></div>' +
                                '<div class="appointment-title"><strong>Phone:</strong><span>' + FacilityDetail.Phone + '</span></div>' +
                                '<div class="appointment-title"><strong>DOB:</strong><span>' + utility.RemoveTimeFromDate(null, FacilityDetail.DOB) + '</span></div>' +
                                '<div class="appointment-title"><strong>Reason:</strong><span>' + FacilityDetail.Reason + '</span></div>' +
                            "</td>" +
                            "<td>" +
                                '<div class="appointment-title"><strong>Provider Name:</strong><span>' + FacilityDetail.ProviderName + '</span></div>' +
                                '<div class="appointment-title"><strong>Appt. Date:</strong><span>' + FacilityDetail.AppointmentDate + '</span></div>' +
                                '<div class="appointment-title"><strong>Subscriber ID:</strong><span>' + FacilityDetail.SubscriberId + '</span></div>' +
                                '<div class="appointment-title"><strong>Subscriber Group ID:</strong><span>' + FacilityDetail.GroupId + '</span></div>' +
                                '<div class="appointment-title"><strong>Insurance Plan:</strong><span>' + FacilityDetail.InsurancePlan + '</span></div>' +
                                '<div class="appointment-title"><strong>Comments:</strong><span>' + FacilityDetail.Comments + '</span></div>' +
                                '<div class="appointment-title"><strong>&nbsp;</strong><span></span></div>' +
                            "</td>" +
                            "<td>" +
                                '<div class="appointment-title"><strong>Start Time:</strong><span>' + FacilityDetail.TimeFrom + '</span></div>' +
                                '<div class="appointment-title"><strong>End Time:</strong><span>' + FacilityDetail.TimeTo + '</span></div>' +
                                '<div class="appointment-title"><strong>Copayment:</strong><span>' + FacilityDetail.Copay + '</span></div>' +
                                '<div class="appointment-title"><strong>Ins. Balance:</strong><span>' + FacilityDetail.InsBal + '</span></div>' +
                                '<div class="appointment-title"><strong>Patient Balance:</strong><span>' + FacilityDetail.PatBal + '</span></div>' +
                                '<div class="appointment-title"><strong>Appointment Status:</strong><span>' + FacilityDetail.SchStatus + '</span></div>' +
                                '<div class="appointment-title"><strong>&nbsp;</strong><span></span></div>' +
                            "</td>" +
                        "</tr>" +
                    "</tbody>" +
                "</table>");
        }
        else {
            $($html).find("#AppointmentDetail" + provResIndex + "").append(
                "<table class='appointment-table'>" +
                    "<tbody>" +
                        "<tr>" +
                            "<td>" +
                                '<div class="appointment-title"><strong>Patient Account:</strong><span>' + FacilityDetail.AccountNumber + '</span></div>' +
                                '<div class="appointment-title"><strong>Patient Name:</strong><span>' + FacilityDetail.PatientName + '</span></div>' +
                                '<div class="appointment-title"><strong>Patient Type:</strong><span>' + FacilityDetail.PatientType + '</span></div>' +
                                '<div class="appointment-title"><strong>Visit Type:</strong><span>' + FacilityDetail.VisitType + '</span></div>' +
                                '<div class="appointment-title"><strong>Phone:</strong><span>' + FacilityDetail.Phone + '</span></div>' +
                                '<div class="appointment-title"><strong>DOB:</strong><span>' + utility.RemoveTimeFromDate(null, FacilityDetail.DOB) + '</span></div>' +
                            "</td>" +
                            "<td>" +
                                '<div class="appointment-title"><strong>Appt. Date:</strong><span>' + FacilityDetail.AppointmentDate + '</span></div>' +
                                '<div class="appointment-title"><strong>Subscriber ID:</strong><span>' + FacilityDetail.SubscriberId + '</span></div>' +
                                '<div class="appointment-title"><strong>Subscriber Group ID:</strong><span>' + FacilityDetail.GroupId + '</span></div>' +
                                '<div class="appointment-title"><strong>Insurance Plan:</strong><span>' + FacilityDetail.InsurancePlan + '</span></div>' +
                                '<div class="appointment-title"><strong>Comments:</strong><span>' + FacilityDetail.Comments + '</span></div>' +
                                '<div class="appointment-title"><strong>Reason:</strong><span>' + FacilityDetail.Reason + '</span></div>' +
                            "</td>" +
                            "<td>" +
                                '<div class="appointment-title"><strong>Start Time:</strong><span>' + FacilityDetail.TimeFrom + '</span></div>' +
                                '<div class="appointment-title"><strong>End Time:</strong><span>' + FacilityDetail.TimeTo + '</span></div>' +
                                '<div class="appointment-title"><strong>Copayment:</strong><span>' + FacilityDetail.Copay + '</span></div>' +
                                '<div class="appointment-title"><strong>Ins. Balance:</strong><span>' + FacilityDetail.InsBal + '</span></div>' +
                                '<div class="appointment-title"><strong>Patient Balance:</strong><span>' + FacilityDetail.PatBal + '</span></div>' +
                                '<div class="appointment-title"><strong>Appointment Status:</strong><span>' + FacilityDetail.SchStatus + '</span></div>' +
                            "</td>" +
                        "</tr>" +
                    "</tbody>" +
                "</table>");
        }
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
                    top: "5mm",
                    right: "3mm",
                    bottom: "8mm"
                },
            }).then(function (group) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    Scheduling_ProviderAppointmentPrint.pdf = params["PrintPDFDataURL"];

                    if (isPreview)
                        utility.PDFViewer(params["PrintPDFDataURL"], false, 'pnlScheduling_ProviderAppointmentPrint #PreviewPrintReceipt', true);

                    if (Scheduling_ProviderAppointmentPrint.params["isSaveReceiptDoc"] == true) {}
                    else {
                        def.resolve();
                    }
                    $obj.find("#printcall").hide();
                });
            });
        }, 500);

        return def.promise();
    },

    PreviewReceipt: function (ProviderId, FacilityId, AppointmentDate, ResourceId) {
        var schStatusIds = $("#pnlPMSScheduler #hfAppointmentStatusIds").val();
        var data = "ProviderId=" + ProviderId + "&FacilityId=" + FacilityId + "&AppointmentDate=" + AppointmentDate + "&SchStatusIds=" + schStatusIds + "&ResourceId=" + ResourceId;
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "PROVIDER_APPOINTMENT_PRINT");
    },

    UnLoad: function () {
        Scheduling_CopaymentView.pdf = "";
        UnloadActionPan(Scheduling_ProviderAppointmentPrint.params["ParentCtrl"], "actionPanSchedulingPrint");
    },

    PrintReceipt: function () {
        var params = [];
        params["PrintPDFDataURL"] = Scheduling_ProviderAppointmentPrint.pdf;
        params["PreviewPdf"] = true;
        utility.documentPrint(params["PrintPDFDataURL"]);
    },

    updatePreview: function () {
        var values_ = $("#pnlScheduling_ProviderAppointmentPrint #statusMultiselectPrint").data("kendoMultiSelect").value();
        var newPrintData = [];
        if (Scheduling_ProviderAppointmentPrint.printData && values_.length && !(values_[0] == "0")) {
            $.each(Scheduling_ProviderAppointmentPrint.printData, function (ii, e) {
                $.each(values_, function (i) {
                    if (e.SchStatusId == values_[i]) {
                        newPrintData.push(e);
                    }
                })
            });
        } else {
            newPrintData = Scheduling_ProviderAppointmentPrint.printData;
        }
        Scheduling_ProviderAppointmentPrint.getPreview(newPrintData, true);
    },
}