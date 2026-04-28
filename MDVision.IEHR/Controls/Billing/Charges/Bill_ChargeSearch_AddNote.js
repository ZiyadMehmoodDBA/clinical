Bill_ChargeSearch_AddNote = {
    params: [],
    Load: function (params) {
        Bill_ChargeSearch_AddNote.params = params;

        if (Bill_ChargeSearch_AddNote.params.PanelID != 'pnlBill_ChargeSearch_AddNote') {
            Bill_ChargeSearch_AddNote.params.PanelID = Bill_ChargeSearch_AddNote.params.PanelID + ' #pnlBill_ChargeSearch_AddNote';
        }
        else {
            Bill_ChargeSearch_AddNote.params.PanelID = 'pnlBill_ChargeSearch_AddNote';
        }

        Bill_ChargeSearch_AddNote.ValidateAddNoteComments();
    },

    ValidateAddNoteComments: function () {
        $('#pnlBill_ChargeSearch_AddNote #frmBill_ChargeSearch_AddNote').bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                NoteComments: {
                    group: '.col-sm-12',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Bill_ChargeSearch_AddNote.SaveAddNoteComments();
        });
    },

    SaveAddNoteComments_DBCall: function () {
        var objData = new Object();

        objData["VisitIds"] = Bill_ChargeSearch_AddNote.params.VisitIds;
        objData["NoteComments"] = utility.encodeURIData($('#' + Bill_ChargeSearch_AddNote.params.PanelID + ' #txtNoteComments').val());

        var data = "VisitData=" + JSON.stringify(objData);
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SAVE_MULTIPLE_NOTE_COMMENTS");
    },

    SaveAddNoteComments: function () {
        Bill_ChargeSearch_AddNote.SaveAddNoteComments_DBCall().done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                Bill_ChargeSearch_AddNote.UnLoad();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    UnLoad: function (caller) {
        if (Bill_ChargeSearch_AddNote.params.ParentCtrl != "") {
            UnloadActionPan(Bill_ChargeSearch_AddNote.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },
}