Admin_HL7ViewFile = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_HL7ViewFile.params = params;
        //serialize Data.
        $('#frmAdminHL7ViewFile').data('serialize', $('#frmAdminHL7ViewFile').serialize());
        $('#txtFileContect').val(Admin_HL7ViewFile.params.FileText);
    },

    UnLoad: function () {

            UnloadActionPan(Admin_HL7ViewFile.params["ParentCtrl"], "actionPanAdminHL7ViewFile");
       
    },
}