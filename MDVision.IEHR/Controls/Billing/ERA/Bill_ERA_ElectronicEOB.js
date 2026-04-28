Bill_ERA_ElectronicEOB = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        Bill_ERA_ElectronicEOB.params = params;
        if (Bill_ERA_ElectronicEOB.bIsFirstLoad) {
            Bill_ERA_ElectronicEOB.bIsFirstLoad = false;
            Bill_ERA_ElectronicEOB.Load_ElectronicEOB();
            if (Bill_ERA_ElectronicEOB.params["PanelID"].indexOf("pnlElectronicEOB") < 0) {
                Bill_ERA_ElectronicEOB.params.PanelID = Bill_ERA_ElectronicEOB.params.PanelID + " #pnlElectronicEOB"; //$('#' + Bill_ERA_ElectronicEOB.params.PanelID + " #pnlElectronicEOB");
            }
            if (Bill_ERA_ElectronicEOB.params.ParentCtrl == "Bill_PaymentPosting")
            {
              $("#aPaymentPosting").hide();
            }
        }

    },

    Load_ElectronicEOB: function () {
        AppPrivileges.GetFormPrivileges("ERA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Bill_ERA_ElectronicEOB.LoadElectronicEOB().done(function (response) {
                    if (response.status != false) {
                        var self = $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #frmElectronicEOB");
                        self.find("#txtHTMLView").html(response.ElectronicEOB_JSON);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    PrintReport: function () {

        //$("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #reportTable").removeClass("Of-a");
        $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #printMe").hide();
        $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #CheckCount").hide();
        $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #CheckBreak").hide();

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            var strcontents = $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #ReportDetails").html();

            var ReportName = "Electronic EOB";
            var windowUrl = 'Electronic EOB';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open('', 'printwindow');
            printWindow.document.write('<html><head><title>' + ReportName + ' | PrintReport</title>');
            printWindow.document.write('</head><body>');
            //Append the external CSS file.
            printWindow.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
            //Append the DIV contents.
            printWindow.document.write(strcontents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
            $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #printMe").show();
            $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #CheckCount").show();
            $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #CheckBreak").show();
            
        }
        else {
            var ReportName = "Electronic EOB";
            setTimeout(function () {
                var contents = $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #ReportDetails").html();
                var frame1 = $('<iframe />');
                frame1[0].name = ReportName.toLowerCase().trim();
                frame1.attr("scrolling", "no");
                frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
                $("body").append(frame1);
                var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                frameDoc.document.open();
                //Create a new HTML document.
                frameDoc.document.write('<html><head><title>' + ReportName + ' | PrintReport</title>');
                frameDoc.document.write('</head><body>');
                //Append the external CSS file.
                frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
                //Append the DIV contents.
                frameDoc.document.write(contents);
                frameDoc.document.write('</body></html>');
                frameDoc.document.close();

                setTimeout(function () {
                    window.frames[ReportName.toLowerCase().trim()].focus();
                    window.frames[ReportName.toLowerCase().trim()].print();
                    frame1.remove();
                    //$("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #reportTable").addClass("Of-a");
                    $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #printMe").show();
                    $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #CheckCount").show();
                    $("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #CheckBreak").show();
                }, 200);

            }, 100);
        }

    },

    UnLoad: function () {

        if (Bill_ERA_ElectronicEOB.params != null && Bill_ERA_ElectronicEOB.params.ParentCtrl != null) {
            UnloadActionPan(Bill_ERA_ElectronicEOB.params.ParentCtrl, 'Bill_ERA_ElectronicEOB');
        }
        else
            UnloadActionPan();
    },


    //----------------------------Service Call Methods-----------------------------\\

    LoadElectronicEOB: function () {

        var objData = new JSON.constructor();
        objData["ChargeID"] = Bill_ERA_ElectronicEOB.params.ChargeId;
        objData["VisitID"] = Bill_ERA_ElectronicEOB.params.VisitId;
        objData["CommandType"] = "load_electronic_eob";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERA");

    },
    //Start PRD-21
    OpenPatientDemographic: function () {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    var params = [];
                    params["mode"] = 'Edit';
                    params["PatBanner"] = true;
                    params["patientID"] = Bill_ERA_ElectronicEOB.params.patientID;;
                    params["IsFill"] = false;
                    params["FromAdmin"] = "0";
                    params["ParentCtrl"] = "Bill_ERA_ElectronicEOB";
                    LoadActionPan('demographicDetail', params);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });

        
    },
    OpenPaymentPosting: function () {
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["VisitId"] = Bill_ERA_ElectronicEOB.params["VisitId"];
                params["PaymentRef"] = "Bill_ERA_ElectronicEOB";
                params["ParentCtrl"] = "Bill_ERA_ElectronicEOB";
                params["IsFromCollectCopay"] =false ;
                LoadActionPan('Bill_PaymentPosting', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    ShowHistory: function () {
        var params = [];
        params["PanelID"] = Bill_ERA_ElectronicEOB.params.PanelID;
        params["VisitId"] = Bill_ERA_ElectronicEOB.params["VisitId"];
        params["patientID"] = Bill_ERA_ElectronicEOB.params.patientID;
        params["ChargeCapId"] =Bill_ERA_ElectronicEOB.params.ChargeCapId;
        params["ParentCtrl"] = "Bill_ERA_ElectronicEOB";
        LoadActionPan('Activity_Log', params);
    },
    //END PRD-21
};