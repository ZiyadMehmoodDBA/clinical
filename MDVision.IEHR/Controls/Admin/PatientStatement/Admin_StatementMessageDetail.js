StatementMessageDetail = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        StatementMessageDetail.params = params;

        if (StatementMessageDetail.bIsFirstLoad) {
            StatementMessageDetail.bIsFirstLoad = false;

            StatementMessageDetail.LoadStatementMessage();


        }
    },


    ValidateStatementMessage: function () {
        $('#frmStatementMessage')
                 .bootstrapValidator({
                     live: 'disabled',
                     message: 'This value is not valid',
                     feedbackIcons: {
                         valid: 'glyphicon glyphicon-ok',
                         invalid: 'glyphicon glyphicon-remove',
                         validating: 'glyphicon glyphicon-refresh'
                     },
                     fields: {
                         shortName: {
                             group: '.col-sm-6',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         message: {
                             group: '.col-sm-12',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },

                     }
                 })
             .on('success.form.bv', function (e) {
                 e.preventDefault();
                 StatementMessageDetail.SaveStatementMessage();
             });
    },


    LoadStatementMessage: function () {


        if (StatementMessageDetail.params.mode == "Add") {

            StatementMessageDetail.ValidateStatementMessage();
            //serialize Data.
            $('#frmStatementMessage').data('serialize', $('#frmStatementMessage').serialize());

        }
        else if (StatementMessageDetail.params.mode == "Edit") {

         

            StatementMessageDetail.FillStatementMessage(StatementMessageDetail.params.StatementMessageId).done(function (response) {

                if (response.status != false) {

                    var self = $("#StatementMessageDetail");
                    var StatementMessage_JSON = JSON.parse(response.StatementMessageFill_JSON);
                    utility.bindMyJSON(true, StatementMessage_JSON, false, self).done(function () {

                        
                        //StatementMessageDetail.SupperBillLoad(response);

                        StatementMessageDetail.ValidateStatementMessage();
                        //serialize Data.
                        $('#frmStatementMessage').data('serialize', $('#frmStatementMessage').serialize());

                    });

                }
                else {
                    UnloadActionPan();
                    utility.DisplayMessages(response.Message, 3);
                }

            });

        }
    },



    FillStatementMessage: function (StatementMessageID) {

        var data = "StatementMessageID=" + StatementMessageID;
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_MESSAGE", "FILL_STATEMENT_MESSAGE");

    },

    SaveStatementMessage: function () {

        var self = $("#StatementMessageDetail");
        var myJSON = self.getMyJSON();

        if (StatementMessageDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Statement Message", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    StatementMessageDetail.SaveMessage(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            UnloadActionPan();
                            Admin_StatementMessage.SearchStatementMessage();


                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (StatementMessageDetail.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Statement Message", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    StatementMessageDetail.UpdateStatementMessage(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            UnloadActionPan();
                            Admin_StatementMessage.SearchStatementMessage();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });


        }





    },

    SaveMessage: function (StatementMessageData) {

        var data = "StatementMessageData=" + StatementMessageData;
        // save parameter , class name, command name of class 

        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_MESSAGE", "SAVE_STATEMENT_MESSAGE");
    },

    UpdateStatementMessage: function (StatementMessageData) {

        var data = "StatementMessageData=" + StatementMessageData;
        // update parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_MESSAGE", "UPDATE_STATEMENT_MESSAGE");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmStatementMessage', function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'StatementMessageDetail';
        var ParentCtrl = 'StatementMessageDetail';
        var ProfileName = 'Statement Message';
        var DBTableName = 'StatementMessage';
        var ColumnKeyId = StatementMessageDetail.params.StatementMessageId;
        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

};

