Patient_MessageKey = {
    bIsFirstLoad: true,
    params: [],
    UserMessageId: "",

    Load: function (params) {
        Patient_MessageKey.params = params;
        Patient_MessageKey.ValidateSecretKeyForm();
    },

    ValidateSecretKeyForm: function () {
        $('#frmPatientMessageKey')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   MessageSecretKey: {
                       group: '.MessageSecretKey',
                       validators: {
                           notEmpty: {
                               message: 'Please enter secret key'
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Patient_MessageKey.ValidateSecretKey();
        });
    },

    ValidateSecretKey: function () {
        var secretKey = $("#txtSecretKey").val();
        var UserMessageId = Patient_MessageKey.params.UserMessageId

        var objData = new Object();
        objData["UserMesgId"] = UserMessageId;
        objData["SecretKey"] = secretKey;
        objData["CommandType"] = "validate_secret_key";

        Patient_MessageKey.ValidateSecretKey_DBCall(objData).done(function (response) {
            if (response.status) {
                console.log(response.message);
                Patient_MessageKey.UnLoad();

                var params = [];
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = Patient_UserMessages.params["TabID"];
                params["mode"] = 'Edit';
                params["Isopentask"] = '1';
                params["FromPatModule"] = '1';
                params["MessageType"] = 'Patient';
                params["UserMessageId"] = UserMessageId;

                setTimeout(function () {
                    LoadActionPan('Patient_MessageCreate', params);
                }, 510);
            } else if (response.status == false) {
                {
                    if (response.message == "Mismatched") {
                        $("#LblSecretKey").addClass("hidden");
                        $("#LblInvalidKey").removeClass("hidden");
                        $("#txtSecretKey").val("");
                    }
                    else if (response.message == "Expired") {
                        utility.DisplayMessages("Your secret key has expired. Please reopen the message to receive a new key.", 3);
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                }
            }
        });
    },


    ValidateSecretKey_DBCall: function (objData) {

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },

    UnLoad: function () {

        if (Patient_MessageKey.params != null && Patient_MessageKey.params.ParentCtrl != null) {
            UnloadActionPan(Patient_MessageKey.params.ParentCtrl, 'pnlPatientMessageKey');
        }
        else
            UnloadActionPan(null, 'pnlPatientMessageKey');
    },

}