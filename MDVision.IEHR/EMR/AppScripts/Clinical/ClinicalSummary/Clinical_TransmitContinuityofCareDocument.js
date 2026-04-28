Clinical_TransmitContinuityofCareDocument = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_TransmitContinuityofCareDocument.params = params;
        $('#frmTransmitContinuityofCareDocument #chkHTML').attr("checked", true);
        $('#frmTransmitContinuityofCareDocument #chkXML').attr("checked", true);
        $('#frmTransmitContinuityofCareDocument #chkXML').change(function () {
            if ($(this).is(':checked')) {
                $('#frmTransmitContinuityofCareDocument #xmlFile').show();
            }
            else {
                $('#frmTransmitContinuityofCareDocument #xmlFile').hide();
            }
        });
        $('#frmTransmitContinuityofCareDocument #chkHTML').change(function () {
            if ($(this).is(':checked')) {
                $('#frmTransmitContinuityofCareDocument #htmlFile').show();
            }
            else {
                $('#frmTransmitContinuityofCareDocument #htmlFile').hide();
            }
        });
    },

    Transmit: function () {
        var TransmitParams = [];
        if ($('#frmTransmitContinuityofCareDocument #txtTo').val() == '') {
            utility.DisplayMessages("please enter to email.", 3);
            return;
        }
        if ($("#frmTransmitContinuityofCareDocument [type='checkbox']:checked").length > 0) {

            Clinical_ContinuityofCareDocument.TransmitCCDA($('#frmTransmitContinuityofCareDocument #txtTo').val(), $('#frmTransmitContinuityofCareDocument #chkXML').prop("checked"), $('#frmTransmitContinuityofCareDocument #chkHTML').prop("checked"), Clinical_TransmitContinuityofCareDocument.params["DocType"]);

        }
        else {
            utility.DisplayMessages("please select atleast one file.", 3);
        }

    },

    UnLoad: function () {
        var objDeffered = $.Deferred();
        UnloadActionPan(Clinical_TransmitContinuityofCareDocument.params.ParentCtrl, 'Clinical_TransmitContinuityofCareDocument');
        objDeffered.resolve();
        return objDeffered;
    },
}