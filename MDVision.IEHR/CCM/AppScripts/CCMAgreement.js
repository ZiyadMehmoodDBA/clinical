CCMAgreement = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCMAgreement.params = params;
        if (CCMAgreement.params != null && CCMAgreement.params.PanelID != "pnlCCMAgreement") {
            CCMAgreement.params["PanelID"] = CCMAgreement.params["PanelID"] + ' #pnlCCMAgreement';
        }
        else {
            CCMAgreement.params["PanelID"] = "pnlCCMAgreement";
        }

        if (CCMAgreement.bIsFirstLoad) {
            CCMAgreement.bIsFirstLoad = false;

            CCMAgreement.FillInfo();
        }
    },



    FillInfo: function () {

        $("#" + CCMAgreement.params["PanelID"] + " #spnPatientName").text(CCMAgreement.params.PatientName);
        $("#" + CCMAgreement.params["PanelID"] + " #spnProvideName").text(CCMAgreement.params.ProviderName);

        var currentDate = $.datepicker.formatDate(globalAppdata.DateFormat.replace("yy", ""), new Date())
        $("#" + CCMAgreement.params["PanelID"] + " #spnDate").text(currentDate);
    },

    IAgree: function () {
        if ($("#" + CCMAgreement.params["PanelID"] + " #imgPatientSignature").attr("src") == "") {

            utility.DisplayMessages("Patient Signature are required", 3);
        }
        else {

            CCMAgreement.makePDF();

        }
    },

    makePDF: function () {
        $('#' + CCMAgreement.params["PanelID"] + " #btnCancel").hide();
        $('#' + CCMAgreement.params["PanelID"] + " #btnIAgree").hide();
        $('#' + CCMAgreement.params["PanelID"] + " #btnSign").hide();
        
        kendo.drawing.drawDOM('#' + CCMAgreement.params["PanelID"] + " #divToPrint", {
            landscape: false,
            scale: 0.7,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "10mm",
                right: "10mm",
                bottom: "20mm"
            },
            // template: $('#' + Clinical_InfoButtonView.params["PanelID"] + " #page-template").html()
        }).then(function (group) {


            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var base64 = dataURL.split('data:application/pdf;base64,').join('');

                CCMEnrollmentInfo.setConsentFile(base64)

                CCMAgreement.UnLoad();
            });
        });

    },



    OpenSignDevice: function () {

        var params = [];
        params["ParentCtrl"] = 'CCMAgreement';
        var controlId = "Admin_Provider_eSignature";
        LoadActionPan(controlId, params);

    },


    setImageSource: function (base64) {

        $("#" + CCMAgreement.params["PanelID"] + " #imgPatientSignature").attr("src", base64);
    },

    UnLoad: function () {

        UnloadActionPan(CCMAgreement.params.ParentCtrl, 'CCMAgreement');

    },
}