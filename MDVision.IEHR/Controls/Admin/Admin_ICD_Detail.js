ICDDetail = {
    params: [],
    lexiCode_: "",
    Load: function (params) {

        ICDDetail.params = params;

        if (ICDDetail.params["PanelID"] != "ICDDetail") {
            ICDDetail.params["PanelID"] = ICDDetail.params["PanelID"] + " #ICDDetail";
        }

        var self = $('#' + ICDDetail.params["PanelID"]);
        self.loadDropDowns(true).done(function () {

            // KKK

            //if (globalAppdata['IsAdmin'] != "True") {
            //    $('#' + ICDDetail.params["PanelID"] + ' #divICDCode_Entity').css("display", "none");
            //    $('#' + ICDDetail.params["PanelID"] + ' #ddlEntity').val(globalAppdata["SeletedEntityId"]);
            //}

            /// -KKK

            //Set Search Parameters if ICD Open from other.

            // KKK

            //if (ICDDetail.params["EntityId"] != null && ICDDetail.params["EntityId"] != undefined) {
            //    $('#' + ICDDetail.params["PanelID"] + ' #ddlEntity').val(ICDDetail.params["EntityId"]);
            //    $('#' + ICDDetail.params["PanelID"] + ' #ddlEntity').attr("disabled", "disabled");
            //    ICDDetail.params["EntityId"] = null;
            //}

            /// -KKK

            ICDDetail.LoadICD();
        });
        //$("#ICDDetail #txtICD10Code").attr('disabled', true);
        //jQuery('#txtICD9Code').on('input propertychange paste', function () {
        //    jQuery('#txtICD9Description, #txtICD10Code, #txtICD10Description, #txtSnomedCode,#txtSnomedDescription').val('');
        //});
        //jQuery('#txtICD10Code').on('input propertychange paste', function () {
        //    jQuery('#txtICD9Description, #txtICD9Code, #txtICD10Description, #txtSnomedCode,#txtSnomedDescription').val('');
        //});
        //$("#txtICD9Description, #txtICD10Description, #txtSnomedCode,#txtSnomedDescription").attr('disabled', true);
        $(function () {
            $('#ICDDetail #txtICDAndDescription').keypress(function (e) {
                var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keycode == 13) {
                    e.preventDefault();
                }
            });
        });
    },

    LoadICD: function () {
        PageNo = null;
        rpp = null;
        if (ICDDetail.params.mode == "Add") {
            $("#divICDFields").hide();
            $('#' + ICDDetail.params["PanelID"] + ' #txtShortName').attr("enabled", "enabled");

            //serialize data
            $('#frmICDDetail').data('serialize', $('#frmICDDetail').serialize());
            ICDDetail.ValidateICD();
        }
        else if (ICDDetail.params.mode == "Edit") {
            $("#divICDFields").show();
            $('#' + ICDDetail.params["PanelID"] + ' #txtShortName').attr("disabled", "disabled");
            ICDDetail.FillICD(ICDDetail.params.ICDId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    var ICD_detail = JSON.parse(response.ICDFill_JSON);
                    var self = $("#" + ICDDetail.params["PanelID"]);

                    utility.bindMyJSON(true, ICD_detail, false, self).done(function () {

                        // KKK
                        //if (ICD_detail.ddlEntity == null || ICD_detail.ddlEntity == "") {
                        //    $('#' + ICDDetail.params["PanelID"] + ' #txtICD9').attr("disabled", "disabled");
                        //}

                        // -KKK

                        if (ICD_detail.chkValid == 'True')
                            $("#" + ICDDetail.params["PanelID"] + " #chkValid").attr("checked", true);
                        else
                            $("#" + ICDDetail.params["PanelID"] + " #chkValid").attr("checked", false);
                        if (ICD_detail.chkActivelyUsed == 'True')
                            $("#" + ICDDetail.params["PanelID"] + " #chkActivelyUsed").attr("checked", true);
                        else
                            $("#" + ICDDetail.params["PanelID"] + " #chkActivelyUsed").attr("checked", false);
                        ICDDetail.ValidateICD();

                        //serialize data
                        $('#frmICDDetail').data('serialize', $('#frmICDDetail').serialize());

                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateICD: function () {
        $('#frmICDDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ICD9: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ICD9Description: {
                       group: '.col-sm-8',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            ICDDetail.ICDSave();
        });
    },

    ICDSave: function () {
        var strMessage = "";
        var self = $("#" + ICDDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        if (ICDDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("ICD", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ICDDetail.SaveICD(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_ICD.ICDSearch(response.ICDId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(ICDDetail.params["ParentCtrl"], "ICDDetail");
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
        else if (ICDDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("ICD", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ICDDetail.UpdateICD(myJSON, ICDDetail.params.ICDId).done(function (response) {
                        if (response.status != false) {
                            Admin_ICD.ICDSearch(ICDDetail.params.ICDId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(ICDDetail.params["ParentCtrl"], "ICDDetail");
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

    SaveICD: function (ICDData) {
        var data = "ICDData=" + ICDData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ICD_DETAIL", "SAVE_ICD");
    },

    UpdateICD: function (ICDData, ICDID) {
        var data = "ICDData=" + ICDData + "&ICDID=" + ICDID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ICD_DETAIL", "UPDATE_ICD");
    },

    FillICD: function (ICDID, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "ICDID=" + ICDID + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ICD_DETAIL", "FILL_ICD");
    },

    UpdateICDActiveInactive: function (ICDID, IsActive, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "ICDID=" + ICDID + "&IsActive=" + IsActive + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ICD_DETAIL", "UPDATE_ICD_ACTIVE_INACTIVE");
    },

    UnLoad: function () {
        if ($('#frmICDDetail').serialize() != $('#frmICDDetail').data('serialize')) {
            utility.myConfirm('2', function () {
                UnloadActionPan(ICDDetail.params["ParentCtrl"], "ICDDetail");
            }, function () { },
                    '2'
                );
        }
        else {
            UnloadActionPan(ICDDetail.params["ParentCtrl"], "ICDDetail");
        }
    },

    SearchCode: function () {

        //if (SupperBillDetail.params["action"] == "ICD") {
        ICDDetail.OpenICDDetail();
        //}
        //else if (SupperBillDetail.params["action"] == "CPT") {
        //    SupperBillDetail.OpenCPTDetail();
        //}
        //else if (SupperBillDetail.params["action"] == "Modifier") {
        //    SupperBillDetail.OpenModifierDetail();
        //}
    },

    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, 100, true, -1, "ICD", true, "ICDDetail", null, false);
    },

    BindICD10AutoComplete: function (element) {

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, 100, true, -1, "ICD", false, "ICDDetail", null, false);
    },

    OpenICDDetail: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ICDDetail";
        params["EntityId"] = 100;
        LoadActionPan('Admin_ICD', params);
    },

    SetControlValues: function (Lexicode_ICD_9_10_SnoMed) {

        var LexiCode = Lexicode_ICD_9_10_SnoMed.split("*")[0];
        var ICD9CodePlusICD9Title = Lexicode_ICD_9_10_SnoMed.split("*")[1].split("$")[0].trim().replace("&#10;", "").trim();
        ICD9CodePlusICD9Title = ICD9CodePlusICD9Title.replace("ICD9-CM - ".trim(), "").trim().replace("&#10;", "");
        var ICD9Code = ICD9CodePlusICD9Title.split('-')[0].trim();
        var ICD9Title = ICD9CodePlusICD9Title.split('-')[1].trim();
        var ICD10CodePlusICD10Title = Lexicode_ICD_9_10_SnoMed.split("*")[1].split("$-")[1].trim().replace("&#10;", "").trim();
        var ICD10Code = ICD10CodePlusICD10Title.split('-')[0].trim();
        var ICD10Title = ICD10CodePlusICD10Title.split('-')[1].trim().replace("~", "");
        var SnoMedCodePlusSnoMedTitle = Lexicode_ICD_9_10_SnoMed.split("*")[1].split("~-")[1].trim().replace("&#10;", "").trim();
        var SnoMedCode = SnoMedCodePlusSnoMedTitle.split('-')[0].trim();
        var SnoMedTitle = SnoMedCodePlusSnoMedTitle.split('-')[1].trim();


        //$("#txtICD9Code").attr('value', ICD9Code);
        //$("#ICDDetail #txtICD9Code").val(ICD9Code);
        $("#ICDDetail #txtICD9Description").val(ICD9Title);
        $("#ICDDetail #txtICD10Code").val(ICD10Code);
        $("#ICDDetail #txtICD10Description").val(ICD10Title);
        $("#ICDDetail #txtSnomedCode").val(SnoMedCode);
        $("#ICDDetail #txtSnomedDescription").val(SnoMedTitle);

        //$("#lblICD10").remove();
        //$("#anchorICD10").show();
        //$("#anchorICD10").attr("type", Lexicode_ICD_9_10_SnoMed);

        $("#ICDDetail #lblICD9").remove();
        $("#ICDDetail #anchorICD9").show();
        $("#ICDDetail #anchorICD9").attr("type", Lexicode_ICD_9_10_SnoMed);

        $("#ICDDetail #hfLexiCode").val(LexiCode);
        setTimeout(function () { $("#ICDDetail #txtICD9Code").val(ICD9Code); $("#ICDDetail #txtICD10Code").val(ICD10Code); $("#ICDDetail #hfLexiCode").val(LexiCode); }, 130);
    },

    ResetHiddenFields: function (obj, codeType, hfcontrolid, ParentCtrl) {

        var RefHiddenCtrlArray = [];
        RefHiddenCtrlArray = hfcontrolid.split(",");
        if (RefHiddenCtrlArray.length > 1) {
            $("#" + obj.id).val('');
            $('#' + RefHiddenCtrlArray[0]).val('');
            $('#' + RefHiddenCtrlArray[1]).val('');
            $('#' + RefHiddenCtrlArray[2]).val('');
            $('#' + RefHiddenCtrlArray[3]).val('');
            $('#' + RefHiddenCtrlArray[4]).val('');
            $('#' + RefHiddenCtrlArray[5]).val('');
        }
    },

    ShowHistory: function () {
        var PanelID = 'ICDDetail';
        var ParentCtrl = 'ICDDetail';
        var ProfileName = 'ICD';
        var DBTableName = 'ICD';
        var ColumnKeyId = ICDDetail.params.ICDId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}