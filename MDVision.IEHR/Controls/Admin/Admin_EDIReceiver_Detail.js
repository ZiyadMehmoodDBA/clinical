EDIReceiverDetail = {
    params: [],
    Load: function (params) {
        EDIReceiverDetail.params = params;
        
        var self = "";
        if (EDIReceiverDetail.params["PanelID"] != 'tblEDIReceiverDetail')
            self = $('#' + EDIReceiverDetail.params["PanelID"] + ' #tblEDIReceiverDetail');
        else
            self = $('#tblEDIReceiverDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            EDIReceiverDetail.LoadEDIReceiver();
        });
    },

    LoadEDIReceiver: function () {
        if (EDIReceiverDetail.params.mode == "Add") {
            $('#EDIReceiverDetail #txtShortName').attr("enabled", "enabled");
            
            EDIReceiverDetail.ValidateEDIReceiver();
            EDIReceiverDetail.ValidateEDIReceiverX12();
            //$('#EDIReceiverDetail #lnkEdiReceiverX12').addClass("disableAll");

            
            //serialize data
            $('#frmReceiverSetup').data('serialize', $('#frmReceiverSetup').serialize());
            $('#frmReceiverSetupX12').data('serialize', $('#frmReceiverSetupX12').serialize());
        }
        else if (EDIReceiverDetail.params.mode == "Edit") {
            $('#EDIReceiverDetail #txtShortName').attr("disabled", "disabled");
            EDIReceiverDetail.FillEDIReceiver(EDIReceiverDetail.params.EDIReceiverId).done(function (response) {
                if (response.status != false) {
                    $("#EDIReceiverDetail #lnkEdiReceiverX12").removeClass('disableAll');
                    var receiverSetup_detail = JSON.parse(response.EDIReceiverSetupFill_JSON);
                    var self = $("#tblEDIReceiverDetail");
                    utility.bindMyJSON(true, receiverSetup_detail, false, self).done(function () {

                        if ($("#EDIReceiverDetail #txtInterSenderID").val() == "")
                            $("#EDIReceiverDetail #txtInterSenderID").val(receiverSetup_detail.txtSubmitterID);

                        if ($("#EDIReceiverDetail #txtInterReceiverID").val() == "")
                            $("#EDIReceiverDetail #txtInterReceiverID").val(receiverSetup_detail.txtReceiverID);

                        if (receiverSetup_detail.chkBatchBillingNPI == 'True')
                            $("#EDIReceiverDetail #chkBatchBillingNPI").attr("checked", true);
                        else
                            $("#EDIReceiverDetail #chkBatchBillingNPI").attr("checked", false);
                        if (receiverSetup_detail.chkANSI5010 == 'True')
                            $("#EDIReceiverDetail #chkANSI5010").attr("checked", true);
                        else
                            $("#EDIReceiverDetail #chkANSI5010").attr("checked", false);
                        if (receiverSetup_detail.chkSendSiteID == 'True')
                            $("#EDIReceiverDetail #chkSendSiteID").attr("checked", true);
                        else
                            $("#EDIReceiverDetail #chkSendSiteID").attr("checked", false);
                        if (receiverSetup_detail.chkIsPrimary == 'True')
                            $("#EDIReceiverDetail #chkIsPrimary").attr("checked", true);
                        else
                            $("#EDIReceiverDetail #chkIsPrimary").attr("checked", false);


                        if (response.EDIReceiverX12SetupFill_JSON != "") {
                            var receiverSetupX12_detail = JSON.parse(response.EDIReceiverX12SetupFill_JSON);
                            var self_x12 = $("#frmReceiverSetupX12");

                            utility.bindMyJSON(true, receiverSetupX12_detail, false, self_x12).done(function () {

                                if (receiverSetupX12_detail.chkISAOneFile == 'True')
                                    $("#EDIReceiverDetail #chkISAOneFile").attr("checked", true);
                                else
                                    $("#EDIReceiverDetail #chkISAOneFile").attr("checked", false);

                                EDIReceiverDetail.params.X12mode = "Edit";
                                EDIReceiverDetail.params.EDIReceiverX12Id = response.EDIReceiverX12SetupId;


                                //serialize data
                                 $('#frmReceiverSetup').data('serialize', $('#frmReceiverSetup').serialize());
                                 $('#frmReceiverSetupX12').data('serialize', $('#frmReceiverSetupX12').serialize());
                            });
                            
                        }
                        else
                            EDIReceiverDetail.params.X12mode = "Add";

                        EDIReceiverDetail.ValidateEDIReceiver();
                        EDIReceiverDetail.ValidateEDIReceiverX12();

                        //serialize data
                        $('#frmReceiverSetup').data('serialize', $('#frmReceiverSetup').serialize());
                        $('#frmReceiverSetupX12').data('serialize', $('#frmReceiverSetupX12').serialize());

                    });
                    

                    
                }

                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateEDIReceiver: function () {
        $('#frmReceiverSetup')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    ShortName: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    SubmitterID: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //TranSetPurposeCode: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    Entity: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ClearingHouse: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ReceiverID: {
                        group: '.col-md-3',
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
            EDIReceiverDetail.EDIReceiverSave();
        });
    },

    ValidateEDIReceiverX12: function () {
        $('#frmReceiverSetupX12')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    AuthInfoQual: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    AuthInfo: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    SecurityInfoQual: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    SecurityInfo: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    InterSenderIDQual: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    InterSenderID: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    InterReceiverIDQual: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    InterReceiverID: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    RepetitionSeparator: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    InterCtrlVerNum: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //InterchangeControlNumber: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    AcknowledgementRequested: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    UsageIndicator: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //CompElemSeparator: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //AppSenderCode: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //AppReceiverCode: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //GroupControlNum: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    VersionReleaseID: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //TranSetupPurposeCode: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //ReferenceID: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //TranTypeCode: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //TranSetupCtrlNum: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //ImplementConventionRef: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //TNSHDLRClass: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    ReceiverName: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //FileCounter: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    RecPrimaryIDNum: {
                        group: '.col-md-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //ToDay: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //SegmentTerminator: {
                    //    group: '.col-md-3',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            EDIReceiverDetail.EDIReceiverX12Save();
        });
    },

    EDIReceiverSave: function () {
        var strMessage = "";
        var self = $("#EdiReceiver");
        var myJSON = self.getMyJSON();
        if (EDIReceiverDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("EDI Receiver", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (EDIReceiverDetail.params.EDIReceiverId == "-1") {
                        EDIReceiverDetail.SaveEDIReceiver(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#EDIReceiverDetail #lnkEdiReceiverX12").removeClass('disableAll');
                                Admin_EDIReceiver.ReceiverSetupSearch(response.EDIReceiverSetupId);
                                EDIReceiverDetail.params.EDIReceiverId = response.EDIReceiverSetupId;
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetEDIReceiverSetup', true);
                                //UnloadActionPan();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (EDIReceiverDetail.params.EDIReceiverId != "-1" && EDIReceiverDetail.params.EDIReceiverId != "" && EDIReceiverDetail.params.EDIReceiverId != "0") {
                        EDIReceiverDetail.UpdateEDIReceiver(myJSON, EDIReceiverDetail.params.EDIReceiverId).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_EDIReceiver.ReceiverSetupSearch(EDIReceiverDetail.params.EDIReceiverId);
                                CacheManager.BindCodes('GetEDIReceiverSetup', true);
                                //UnloadActionPan();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (EDIReceiverDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("EDI Receiver", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIReceiverDetail.UpdateEDIReceiver(myJSON, EDIReceiverDetail.params.EDIReceiverId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_EDIReceiver.ReceiverSetupSearch(EDIReceiverDetail.params.EDIReceiverId);
                            CacheManager.BindCodes('GetEDIReceiverSetup', true);
                            //UnloadActionPan();
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

    EDIReceiverX12Save: function () {
        $('#frmReceiverSetup').data('serialize', $('#frmReceiverSetup').serialize());
        var strMessage = "";
        var self = $("#EdiReceiverSecond");
        var myJSON = self.getMyJSON();
        if (EDIReceiverDetail.params.X12mode == "Add") {
            AppPrivileges.GetFormPrivileges("EDI Receiver", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    //if (EDIReceiverDetail.params.EDIReceiverX12Id == "-1") {
                    EDIReceiverDetail.SaveEDIReceiverX12(myJSON, EDIReceiverDetail.params.EDIReceiverId).done(function (response) {
                        if (response.status != false) {
                            Admin_EDIReceiver.ReceiverSetupSearch(EDIReceiverDetail.params.EDIReceiverId);
                            EDIReceiverDetail.params.EDIReceiverX12Id = response.EDIReceiverX12SetupId;
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                    //}
                    //else if (EDIReceiverDetail.params.EDIReceiverX12Id != "-1" && EDIReceiverDetail.params.EDIReceiverX12Id != "" && EDIReceiverDetail.params.EDIReceiverX12Id != "0") {
                    //    EDIReceiverDetail.UpdateEDIReceiverX12(myJSON, EDIReceiverDetail.params.EDIReceiverId, EDIReceiverDetail.params.EDIReceiverX12Id).done(function (response) {
                    //        if (response.status != false) {
                    //            utility.DisplayMessages(response.message, 1);
                    //            Admin_EDIReceiver.ReceiverSetupSearch('0');
                    //            UnloadActionPan();
                    //        }
                    //        else {
                    //            utility.DisplayMessages(response.Message, 3);
                    //        }
                    //    });
                    //}
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (EDIReceiverDetail.params.X12mode == "Edit") {
            AppPrivileges.GetFormPrivileges("EDI Receiver", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIReceiverDetail.UpdateEDIReceiverX12(myJSON, EDIReceiverDetail.params.EDIReceiverId, EDIReceiverDetail.params.EDIReceiverX12Id).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_EDIReceiver.ReceiverSetupSearch(EDIReceiverDetail.params.EDIReceiverId);
                            UnloadActionPan();
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

    SaveEDIReceiver: function (EDIReceiverData) {
        var data = "EDIReceiverData=" + EDIReceiverData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP_DETAIL", "SAVE_RECEIVER_SETUP");
    },

    UpdateEDIReceiver: function (EDIReceiverData, EDIReceiverID) {
        var data = "EDIReceiverData=" + EDIReceiverData + "&EDIReceiverID=" + EDIReceiverID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP_DETAIL", "UPDATE_RECEIVER_SETUP");
    },

    SaveEDIReceiverX12: function (EDIReceiverData, EDIReceiverID) {
        var data = "EDIReceiverData=" + EDIReceiverData + "&EDIReceiverID=" + EDIReceiverID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP_DETAIL", "SAVE_RECEIVER_SETUP_X12");
    },

    UpdateEDIReceiverX12: function (EDIReceiverData, EDIReceiverID, EDIReceiverX12ID) {
        var data = "EDIReceiverData=" + EDIReceiverData + "&EDIReceiverID=" + EDIReceiverID + "&EDIReceiverX12ID=" + EDIReceiverX12ID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP_DETAIL", "UPDATE_RECEIVER_SETUP_X12");
    },

    FillEDIReceiver: function (EDIReceiverID) {
        var data = "EDIReceiverID=" + EDIReceiverID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP_DETAIL", "FILL_RECEIVER_SETUP");
    },

    UpdateEDIReceiverActiveInactive: function (EDIReceiverID, IsActive) {
        var data = "EDIReceiverID=" + EDIReceiverID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP_DETAIL", "UPDATE_RECEIVER_SETUP_ACTIVE_INACTIVE");
    },

    SetInterchangeSenderID:function(obj)
    {
        $("#EDIReceiverDetail #txtInterSenderID").val($(obj).val());
    },

    ValidateCtrl: function (obj) {

        if ($(obj).val().length < 10)
            $(obj).val('');
    },

    SetInterchangeReceiverID: function (obj) {

        $("#EDIReceiverDetail #txtInterReceiverID").val($(obj).val());
    },
    UnLoad: function () {


        utility.UnLoadDialog("frmReceiverSetup", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'EDIReceiverDetail';
        var ParentCtrl = 'EDIReceiverDetail';
        var ProfileName = 'EDI Receiver';
        var DBTableName = 'EDIReceiverSetup';
        var ColumnKeyId = EDIReceiverDetail.params.EDIReceiverId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

    ShowHistoryReceiverX12: function () {
        var PanelID = 'EDIReceiverDetail';
        var ParentCtrl = 'EDIReceiverDetail';
        var ProfileName = 'EDI Receiver';
        var DBTableName = 'EDIReceiverX12Setup';
        var ColumnKeyId = EDIReceiverDetail.params.EDIReceiverX12Id;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

}