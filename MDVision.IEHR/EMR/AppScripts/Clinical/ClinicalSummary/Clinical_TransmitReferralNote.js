Clinical_TransmitReferralNote = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_TransmitReferralNote.params = params;
        $('#frmTransmitReferralNote #chkHTML').attr("checked", true);
        $('#frmTransmitReferralNote #chkXML').attr("checked", true);
        $('#frmTransmitReferralNote #chkXML').change(function () {
            if ($(this).is(':checked')) {
                $('#frmTransmitReferralNote #xmlFile').show();
            }
            else {
                $('#frmTransmitReferralNote #xmlFile').hide();
            }
        });
        $('#frmTransmitReferralNote #chkHTML').change(function () {
            if ($(this).is(':checked')) {
                $('#frmTransmitReferralNote #htmlFile').show();
            }
            else {
                $('#frmTransmitReferralNote #htmlFile').hide();
            }
        });
    },

    Transmit: function () {
        var TransmitParams = [];
        if ($('#frmTransmitReferralNote #txtTo').val() == '') {
            utility.DisplayMessages("please enter to email.", 3);
            return;
        }
        if ($("#frmTransmitReferralNote [type='checkbox']:checked").length > 0) {

            Clinical_ReferralNote.TransmitCCDA($('#frmTransmitReferralNote #txtTo').val(), $('#frmTransmitReferralNote #chkXML').prop("checked"), $('#frmTransmitReferralNote #chkHTML').prop("checked"), Clinical_TransmitReferralNote.params["DocType"]);

        }
        else {
            utility.DisplayMessages("please select atleast one file.", 3);
        }

    },

    UnLoad: function () {
        var objDeffered = $.Deferred();
        UnloadActionPan(Clinical_TransmitReferralNote.params.ParentCtrl, 'Clinical_TransmitReferralNote');
        objDeffered.resolve();
        return objDeffered;
    },
}