SupperBillDetail = {
    bIsFirstLoad: true,
    params: [],
    ContainerCtrl: "",
    BillId: -1,


    Load: function (params) {

        SupperBillDetail.params = params;

        if (SupperBillDetail.params["PanelID"] != "SupperBillDetail") {
            SupperBillDetail.params["PanelID"] = SupperBillDetail.params["PanelID"] + " #SupperBillDetail";
        }

        var self = $('#tblSupperBillDetail');
        self.loadDropDowns(true).done(function () {
            SupperBillDetail.LoadSupperBill();
        });

    },

    LoadSupperBill: function () {

        if (SupperBillDetail.params.mode == "Add") {

            SupperBillDetail.ValidateSupperBill();
            //serialize Data.
            $('#frmSupperBillDetail').data('serialize', $('#frmSupperBillDetail').serialize());

            $("#SupperBillDetail #tblSupper_Bill").addClass("disableAll");

        }
        else if (SupperBillDetail.params.mode == "Edit") {

            $("#SupperBillDetail #tblSupper_Bill").addClass("disableAll");

            SupperBillDetail.FillSupperBill(SupperBillDetail.params.BillId).done(function (response) {

                if (response.status != false) {

                    var self = $("#tblSupperBillDetail");
                    var SupperBill_JSON = JSON.parse(response.BillFill_JSON);
                    utility.bindMyJSON(true, SupperBill_JSON, false, self).done(function () {

                        $("#SupperBillDetail #ddlPractice").attr("disabled", "disabled");
                        SupperBillDetail.params["EntityId"] = $('#SupperBillDetail #ddlPractice option:selected').attr('refvalue');
                        SupperBillDetail.SupperBillLoad(response);

                        SupperBillDetail.ValidateSupperBill();
                        //serialize Data.
                        $('#frmSupperBillDetail').data('serialize', $('#frmSupperBillDetail').serialize());
                        $("#pnlCPT_Result #dgvCPT thead tr th:first-child").removeClass('sorting_asc');
                        $("#SupperBillDetail #tblSupper_Bill").removeClass("disableAll");
                    });

                }
                else {
                    UnloadActionPan();
                    utility.DisplayMessages(response.Message, 3);
                }

            });

        }
    },

    ValidateSupperBill: function () {
        $('#frmSupperBillDetail')
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
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         Description: {
                             group: '.col-sm-5',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         Practice: {
                             group: '.col-sm-2',
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
                 SupperBillDetail.SaveSupperBill();
             });
    },

    SaveSupperBill: function () {

        var self = $("#tblSupperBillDetail");
        var myJSON = self.getMyJSON();

        if (SupperBillDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Super Bill", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    SupperBillDetail.SaveBill(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);

                            $("#SupperBillDetail #ddlPractice").attr("disabled", "disabled");
                            SupperBillDetail.params["EntityId"] = $('#SupperBillDetail #ddlPractice option:selected').attr('refvalue');

                            $("#SupperBillDetail #hfbillId").val(response.BillId);

                            //Modifier
                            $("#pnlSB_Modifier_Result").css("display", "block");
                            EditableTable.initialize('#btnAddModifier', '#dgvModifier_SB', true);

                            //ICD
                            $("#pnlSB_ICT_Result").css("display", "block");
                            EditableTable.initialize('#btnAddICD', '#dgvICD_SB', true);

                            //CPT
                            SupperBillDetail.params["action"] = "CPT";
                            $("#pnlCPT_Result").css("display", "block");
                            EditableTable.initialize('#btnAddCPT', '#dgvCPT', true);

                            $("#SupperBillDetail #tblSupper_Bill").removeClass("disableAll");
                            SupperBillDetail.params.mode = "Edit";
                            //serialize Data.
                            $('#frmSupperBillDetail').data('serialize', $('#frmSupperBillDetail').serialize());

                            $("#pnlCPT_Result #dgvCPT thead tr th:first-child").removeClass('sorting_asc');

                            if (Admin_SupperBill != null) {
                                Admin_SupperBill.SupperBillSearch();
                            }
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
        else if (SupperBillDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Super Bill", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    SupperBillDetail.UpdateBill(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            //serialize Data.
                            $('#frmSupperBillDetail').data('serialize', $('#frmSupperBillDetail').serialize());
                            $("#pnlCPT_Result #dgvCPT thead tr th:first-child").removeClass('sorting_asc');

                            if (Admin_SupperBill != null) {
                                Admin_SupperBill.SupperBillSearch();
                            }
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

    AddTitle: function (item, tableId) {
        var $row = $('<tr  Id="' + item.SBTitleId + '" class="r_title" />');
        $row.attr("onclick", "utility.SelectGridRow($(this))");
        $row.append('<td class="actions sorting_1" >' + SupperBillDetail.GetActions(item.SBTitleId, 'Title') + '</td><td class="code" style="display: none;" >' + "" + '</td><td colspan="3" >' + item.Description + '</td>');

        //if (tableId.indexOf("dgvICD_SB") >= 0)
        //    $row.append("<td style='display: none;'></td>");

        $(tableId + " tbody").last().append($row);

    },

    AddICDCode: function (TitleId, Codeitem) {

        $.each(Codeitem, function (i, item) {

            if (item.SBTitleId == TitleId) {

                var $row = $('<tr sortId="' + item.SortId + '" titleId="' + item.SBTitleId + '" Id="' + item.SBICDId + '" />');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.append('<td class="actions sorting_1" >' + SupperBillDetail.GetActions(item.SBICDId, 'ICD') + '</td><td class="code">' + item.ICD10 + '</td><td class="description">' + item.Description + '</td>');
                //<td class="code" >' + item.ICDCode + '</td>
                $("#pnlSB_ICT_Result #dgvICD_SB tbody").last().append($row);
            }

        });

    },

    AddCPTCode: function (TitleId, Codeitem) {
        $.each(Codeitem, function (i, item) {

            if (item.SBTitleId == TitleId) {

                var $row = $('<tr sortId="' + item.SortId + '" titleId="' + item.SBTitleId + '" Id="' + item.SBCPTId + '" />');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.append('<td class="actions sorting_1" >' + SupperBillDetail.GetActions(item.SBCPTId, 'CPT') + '</td><td class="code" >' + item.CPTCode + '</td><td class="description">' + item.Description + '</td>');

                $("#pnlCPT_Result #dgvCPT tbody").last().append($row);
            }

        });

    },

    AddModifierCode: function (TitleId, Codeitem) {
        $.each(Codeitem, function (i, item) {

            if (item.SBTitleId == TitleId) {

                var $row = $('<tr sortId="' + item.SortId + '" titleId="' + item.SBTitleId + '" Id="' + item.SBModifierId + '" />');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.append('<td class="actions sorting_1" >' + SupperBillDetail.GetActions(item.SBModifierId, 'MODIFIER') + '</td><td class="code" >' + item.ModifierCode + '</td><td>' + item.Description + '</td>');

                $("#pnlSB_Modifier_Result #dgvModifier_SB tbody").last().append($row);
            }

        });
    },

    GetActions: function (id, type) {

        var titleclass = "";
        if (type.toLowerCase().indexOf("title") >= 0)
            titleclass = "hidden";

        return '<a href="#" class="btn-xs hidden on-editing cancel-row" title="Cancel Record"><i class="fa fa-close black"></i></a>' +
        '<a href="#" class="btn-xs hidden on-editing save-row" title="Save Record" ><i class="fa fa-save green"></i></a>' +
        '<a href="#" class="btn-xs hidden on-editing title-row" title="Add Title" ><i class="fa fa-paste black"></i></a>' +
        '<a href="#" class="btn-xs on-default remove-row" title="Delete Record" ><i class="fa fa-close red"></i></a>' +
        '<a href="#" class="btn-xs on-default edit-row" title="Edit Record" ><i class="fa fa-edit black"></i></a>' +
        '<a href="#" class="' + titleclass + ' btn-xs on-default up-row" title="Up Record" ><i class="fa fa-arrow-up black"></i></a>' +
        '<a href="#" class="' + titleclass + ' btn-xs on-default down-row" title="Down Record" ><i class="fa fa-arrow-down black"></i></a>';
    },

    SupperBillLoad: function (response) {


        var TitleLoadJSONData = JSON.parse(response.Title_JSON);

        var modifier_list = JSON.parse(response.Modifier_JSON);
        var Icd_list = JSON.parse(response.ICD_JSON);
        var cpt_list = JSON.parse(response.CPT_JSON);

        //Title
        $.each(TitleLoadJSONData, function (i, item) {

            //Modifier
            if (item.TitleType == "Modifier") {
                //Add Title
                SupperBillDetail.AddTitle(item, "#pnlSB_Modifier_Result #dgvModifier_SB");
                //Add Modifier of that title.
                SupperBillDetail.AddModifierCode(item.SBTitleId, modifier_list);

            } //ICD
            else if (item.TitleType == "ICD") {
                //Add Title
                SupperBillDetail.AddTitle(item, "#pnlSB_ICT_Result #dgvICD_SB");
                //Add ICD of that title.
                SupperBillDetail.AddICDCode(item.SBTitleId, Icd_list);

            } //CPT
            else if (item.TitleType == "CPT") {
                //Add Title
                SupperBillDetail.AddTitle(item, "#pnlCPT_Result #dgvCPT");
                //Add CPT of that title.
                SupperBillDetail.AddCPTCode(item.SBTitleId, cpt_list);
            }

        });

        //Modifier
        $("#pnlSB_Modifier_Result").css("display", "block");
        EditableTable.initialize('#btnAddModifier', '#dgvModifier_SB', true);

        //ICD
        SupperBillDetail.params["action"] = "ICD";
        $("#pnlSB_ICT_Result").css("display", "block");
        EditableTable.initialize('#btnAddICD', '#dgvICD_SB', true);

        //CPT
        SupperBillDetail.params["action"] = "CPT";
        $("#pnlCPT_Result").css("display", "block");
        EditableTable.initialize('#btnAddCPT', '#dgvCPT', true);

    },

    InitializeICD: function () {
        SupperBillDetail.params["action"] = "ICD";
        var oTable = $("#dgvICD_SB").dataTable();
        oTable.fnDestroy();

        EditableTable.initialize('#btnAddICD', '#dgvICD_SB', false);
        $("#pnlSB_ICT_Result #dgvICD_SB thead tr th:first-child").removeClass('sorting_asc');

    },

    InitializeCPT: function () {

        SupperBillDetail.params["action"] = "CPT";
        var oTable = $("#dgvCPT").dataTable();
        oTable.fnDestroy();
        EditableTable.initialize('#btnAddCPT', '#dgvCPT', false);
        $("#pnlCPT_Result #dgvCPT thead tr th:first-child").removeClass('sorting_asc');

    },

    InitializeModifier: function () {

        SupperBillDetail.params["action"] = "Modifier";
        var oTable = $("#dgvModifier_SB").dataTable();
        oTable.fnDestroy();
        EditableTable.initialize('#btnAddModifier', '#dgvModifier_SB', false);
        $("#pnlSB_Modifier_Result #dgvModifier_SB thead tr th:first-child").removeClass('sorting_asc');
    },

    SaveBill: function (SupperBillData) {

        var data = "SupperBillData=" + SupperBillData;
        // save parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "SAVE_SUPPERBILL");
    },

    SupperBillValidateCode: function (obj) {

        if (SupperBillDetail.params["action"] == "CPT") {
            //utility.ValidateCode(obj, 'CPT', null, SupperBillDetail.params.EntityId);
        }
        else if (SupperBillDetail.params["action"] == "ICD") {
            //utility.ValidateCode(obj, 'ICD', null, SupperBillDetail.params.EntityId);
        }
        else if (SupperBillDetail.params["action"] == "Modifier") {
            utility.ValidateCode(obj, 'MODIFIER', null, null);
        }
    },

    BindSupperBillAutoComplete: function (element) {

        var descriptionCrtl = $(element).closest('td').siblings().find('input');

        if (SupperBillDetail.params["action"] == "ICD") {

            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, SupperBillDetail.params.EntityId, true, -1, "ICD", false, "SupperBillDetail", null, true);
        }
        else if (SupperBillDetail.params["action"] == "CPT") {

            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', descriptionCrtl, SupperBillDetail.params.EntityId, true, -1, "CPT", true, "SupperBillDetail", null, true);
        }
        else if (SupperBillDetail.params["action"] == "Modifier") {

            utility.BindAutoCompleteText(element, 'COMMON_CODE', 'GET_MODIFIER_CODE', descriptionCrtl, "", true);
        }
    },

    UpdateBill: function (SupperBillData) {

        var data = "SupperBillData=" + SupperBillData;
        // update parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "UPDATE_SUPPERBILL");
    },

    FillSupperBill: function (BillID) {

        var data = "BillID=" + BillID;
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "FILL_SUPPERBILL");

    },

    SaveSupperBillTitle: function (SupperBillTitleData) {

        var data = SupperBillTitleData + "&action=" + SupperBillDetail.params["action"];
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "SAVE_SUPPERBILL_TITLE");

    },

    UpdateSupperBillTitle: function (SupperBillTitleData) {

        var data = SupperBillTitleData;
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "UPDATE_SUPPERBILL_TITLE");

    },

    SaveSupperBillCode: function (CodeData) {

        var data = CodeData + "&action=" + SupperBillDetail.params["action"];
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "SAVE_SUPPERBILL_CODE");

    },

    UpdateSupperBillCode: function (CodeData) {

        var data = CodeData + "&action=" + SupperBillDetail.params["action"];
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "UPDATE_SUPPERBILL_CODE");
    },

    DeleteTitle: function (SupperBillTitleData) {

        var data = SupperBillTitleData;
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "DELETE_SUPPERBILL_TITLE");

    },

    DeleteCode: function (CodeData) {

        var data = CodeData + "&action=" + SupperBillDetail.params["action"];
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "DELETE_SUPPERBILL_CODE");
    },

    SortCode: function (SupperBillData) {
        var data = SupperBillData + "&action=" + SupperBillDetail.params["action"];
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "SORT_SUPPERBILL_CODE");
    },

    UnLoad: function () {
        if ($('#frmSupperBillDetail').serialize() != $('#frmSupperBillDetail').data('serialize')) {
            utility.myConfirm('2', function () {
                UnloadActionPan();
            }, function () { },
                    '2'
                );
        }
        else {
            UnloadActionPan(null, 'SupperBillDetail');
        }
    },
    ShowHistory: function () {
        var PanelID = 'SupperBillDetail';
        var ParentCtrl = 'SupperBillDetail';
        var ProfileName = 'Super Bill';
        var DBTableName = 'SuperBills';
        var ColumnKeyId = SupperBillDetail.params.BillId;
        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

    ValidateCode: function (Code) {

        if (SupperBillDetail.params["action"] == "ICD") {
            return Admin_ICD.ValidateICDCode(Code, SupperBillDetail.params.EntityId);
        }
        else if (SupperBillDetail.params["action"] == "CPT") {
            return Admin_CPTCode.ValidateCPTCode(Code, SupperBillDetail.params.EntityId);
        }
        else if (SupperBillDetail.params["action"] == "Modifier") {
            return Admin_Modifier.ValidateModifierCode(Code);
        }
        else
            return [];

    },

    OpenCPTDetail: function (Id) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "SupperBillDetail";
        params["RefCtrl"] = "txtCPT_" + Id;
        params["RefCtrlDescription"] = "txtCPTDescription_" + Id;
        params["EntityId"] = SupperBillDetail.params.EntityId;
        LoadActionPan('Admin_IMOCPT', params);
    },

    OpenICDDetail: function (Id) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "SupperBillDetail";
        params["RefId"] = Id;
        params["EntityId"] = SupperBillDetail.params.EntityId;
        LoadActionPan('Admin_IMOICD', params);
    },

    OpenIMODetail: function (lexiCode, icdCodeAndDescription, ParentControl, ContainerCtrl, LocalOrIMO, text, AccessPanel, txtBoxCtrlParentId) {

        SupperBillDetail.ContainerCtrl = ContainerCtrl;

        Clinical_ProblemLists.icdsValues = {};
        if (CCM_Patient_Hub != null && CCM_Patient_Hub.icdsValues) {
            CCM_Patient_Hub.icdsValues = {};
        }
        if (OrderSet_Problems != null && OrderSet_Problems.icdsValues) {
            OrderSet_Problems.icdsValues = {};
        }
        IMODetail.SearchIMO(lexiCode.split("*")[0]).done(function (response) {

            if (response.IMOCount <= 0) {

                if (text.indexOf("MedicalHx") > -1) {
                    ParentControl = "Clinical_MedicalHx";
                }
                else if (text.indexOf("HospitalizationHx") > -1) {
                    ParentControl = "Clinical_HospitalizationHx";
                }
                else if (text.indexOf("SurgicalHx") > -1) {
                    ParentControl = "Clinical_SurgicalHx";
                }
                else if (text.indexOf("FamilyHx") > -1) {
                    ParentControl = "Clinical_FamilyHx";
                }
                else if (text.indexOf("Favorite_ProblemsDetail") > -1) {
                    ParentControl = "Favorite_ProblemsDetail";
                }
                else if (text.indexOf("Favorite_Complaints_Detail") > -1) {
                    ParentControl = "Favorite_Complaints_Detail";
                }
                else if (text.indexOf("HPIComplaints") > -1) {
                    ParentControl = "Clinical_HPIComplaints";
                }
                else if (text.indexOf("Complaints") > -1) {
                    ParentControl = "Clinical_Complaints";
                }
                else if (text.indexOf("Favorite_FamilyHistoryDetail") > -1) {
                    ParentControl = "Favorite_FamilyHistoryDetail";
                }
                else if (text.indexOf("Favorite_MedicalHistoryDetail") > -1) {
                    ParentControl = "Favorite_MedicalHistoryDetail";
                }
                else if (text.indexOf("Favorite_SurgicalHistoryDetail") > -1) {
                    ParentControl = "Favorite_SurgicalHistoryDetail";
                }
                else if (text.indexOf("Favorite_HospitalizationHistoryDetail") > -1) {
                    ParentControl = "Favorite_HospitalizationHistoryDetail";
                }
                

                else if (text.indexOf("CDSDetail") > -1) {
                    ParentControl = "ClinicalCDSDetail";
                }
                else if (text.indexOf("OrderSetDetails") > -1) {
                    ParentControl = "Clinical_OrderSetDetails";
                }
                else if (text.indexOf("PlanOfCare") > -1) {
                    ParentControl = "Clinical_PlanOfCare";
                }
                else if (text.indexOf("Cognitive") > -1) {
                    ParentControl = "Clinical_Cognitive";
                }
                else if (text.indexOf("Clinical_ProblemLists") > -1) {
                    ParentControl = "Clinical_ProblemLists";
                }
                else if (text.indexOf("BillingInformation") > -1) {
                    ParentControl = "BillingInformation";
                }
                else if (text.indexOf("OutOfOfficeVisits") > -1) {
                    ParentControl = "OutOfOfficeVisits";
                } else if (text.indexOf("OrderSet_Problems") > -1) {
                    ParentControl = "OrderSet_Problems";
                }
                else if (text.indexOf("Clinical_CustomFormsPreview") > -1) {
                    ParentControl = "Clinical_CustomFormsPreview";
                }
                else if (text.indexOf("Batch_FaxSend") > -1) {
                    ParentControl = "Batch_FaxSend";
                }
                else if (text.indexOf("CarePlan") > -1) {
                    ParentControl = "Clinical_CarePlan";
                }
                else if (text.indexOf("appointmentDetail") > -1) {
                    ParentControl = "appointmentDetail";
                }
                else if (text.indexOf("Scheduling_Force_Booking") > -1) {
                    ParentControl = "Scheduling_Force_Booking";
                }
                else if (text.indexOf("Batch_ClinicalQualityMeasure") > -1) {
                    ParentControl = "Batch_ClinicalQualityMeasure";
                }
                else if (text.indexOf("iTrackeCQMs") > -1) {
                    ParentControl = "iTrackeCQMs";
                }
                else if (text.indexOf("IA_DiabetesScreening") > -1) {
                    ParentControl = "IA_DiabetesScreening";
                }
                var icd9Code = "", icd9Description = "", icd10Code = "", icd10Description = "", snomedCode = "", snomedDescription = "";
                //if (IMODetail.params.ICDCodeAndDescription != undefined) {
                var ICD10 = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                icd10Code = ICD10.split("+")[0].trim();
                icd10Description = ICD10.split("+")[1];

                var SNOMED = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);
                snomedCode = SNOMED.split("+")[0].trim();
                snomedDescription = SNOMED.split("+")[1];
                if (snomedDescription.indexOf('^') >= 0) {
                    snomedDescription = snomedDescription.split('^')[0];
                }
                //}
                var ICD9 = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                icd9Code = ICD9.split("+")[0].trim();
                icd9Description = ICD9.split("+")[1];

                if (ParentControl == "ICDDetail") {

                    $("#ICDDetail #txtICDAndDescription").val(text);
                    $("#ICDDetail #txtICD9Code").val(icd9Code);
                    $("#ICDDetail #txtICD9Description").val(icd9Description);
                    $("#ICDDetail #txtICD10Code").val(icd10Code); // Selected List Item ID i.e. [ICD10 Code]
                    $("#ICDDetail #txtICD10Description").val(icd10Description); // Selected List Item text i.e. [ICD10 description]
                    $("#ICDDetail #txtSnomedCode").val(snomedCode);
                    $("#ICDDetail #txtSnomedDescription").val(snomedDescription);
                    $("#ICDDetail #hfLexiCode").val(IMODetail.params.lexiCodeId);
                    $("#ICDDetail #divICDFields").show();

                }

                    // MKMK

                else if (ParentControl == "Admin_CCMICDGroups_Detail") {
                    var ControlsArray = [];
                    var thatisSomeShit = (icd9Code + "$" + icd9Description + '*' + icd10Code + '$' + icd10Description + '#' + snomedCode + '$' + snomedDescription);
                    ControlsArray.push(thatisSomeShit);

                    Admin_CCMICDGroups_Detail.BindICDNValues($("#CCMICDGroupsDetail"), ControlsArray);

                }

                else if (ParentControl.indexOf("BillingInformation") >= 0) {// Start Farooq Ahmad Adding ICD search on Billing Information
                    var ControlsArray = [];
                    if (Admin_IMOICD != null != undefined && Admin_IMOICD.params != null && Admin_IMOICD.params.RefHiddenCtrl != null) {
                        ControlsArray = Admin_IMOICD.params.RefHiddenCtrl.split(',');
                    }
                    else if (BillingInformation != null && BillingInformation.params != null && BillingInformation.params.RefHiddenCtrl != null) {
                        ControlsArray = BillingInformation.params.RefHiddenCtrl.split(',');
                    }

                    if (ControlsArray.length > 0) {

                        for (index = 0; index < ControlsArray.length; index++) {

                            if (ControlsArray[index].indexOf("hfICDCode9") >= 0)
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Code)

                            if (ControlsArray[index].indexOf("hfICDDescription9") >= 0)
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Description);

                            if (ControlsArray[index].indexOf("hfICDCode10") >= 0)
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code);

                            if (ControlsArray[index].indexOf("hfICDDescription10") >= 0)
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Description);

                            if (ControlsArray[index].indexOf("hfSNOMEDCode") >= 0)
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedCode);

                            if (ControlsArray[index].indexOf("hfSNOMEDDescription") >= 0)
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedDescription);

                            if (ControlsArray[index].indexOf("txtDisease_") >= 0) {
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code + ' - ' + icd10Description);
                            }

                            if (icd10Code == '' && icd9Code == '') {
                                $('#pnlBillingInformation  #' + ControlsArray[index]).val('')
                            }
                        }
                    }
                    if (Admin_IMOICD.params['fromIcon'] != null && Admin_IMOICD.params['fromIcon'] == "true") {
                        Admin_IMOICD.params['fromIcon'] = "false";
                        Admin_IMOICD.UnLoadTab();
                        BillingInformation.EnableDisableICDs();
                    }
                    //Admin_IMOICD.UnLoadTab();
                }// End Farooq Ahmad Adding ICD search on Billing Information
                else if (ParentControl.indexOf("appointmentDetail") >= 0) {
                    var icdCode = (icd10Code != null && icd10Code != "") ? icd10Code : icd9Code;
                    $("#appointmentDetail #txtComplaintsICD").val(icd10Code + ' - ' + icd10Description);
                    $("#appointmentDetail #hfICDCode").val(icdCode);
                    $("#appointmentDetail #hfICDCode9").val(icd9Code);
                    $("#appointmentDetail #hfICDDescription9").val(icd9Description);
                    $("#appointmentDetail #hfICDCode10").val(icd10Code); 
                    $("#appointmentDetail #hfICDDescription10").val(icd10Description); 
                    $("#appointmentDetail #hfSNOMEDCode").val(snomedCode);
                    $("#appointmentDetail #hfSNOMEDDescription").val(snomedDescription);
                    if (Admin_IMOICD.params['fromIcon']) {
                        Admin_IMOICD.params['fromIcon'] = "false";
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else if (ParentControl.indexOf("Scheduling_Force_Booking") >= 0) {
                    var icdCode = (icd10Code != null && icd10Code != "") ? icd10Code : icd9Code;
                    $("#PnlSchedulingForceBooking #txtComplaintsICD").val(icd10Code + ' - ' + icd10Description);
                    $("#PnlSchedulingForceBooking #hfICDCode").val(icdCode);
                    $("#PnlSchedulingForceBooking #hfICDCode9").val(icd9Code);
                    $("#PnlSchedulingForceBooking #hfICDDescription9").val(icd9Description);
                    $("#PnlSchedulingForceBooking #hfICDCode10").val(icd10Code);
                    $("#PnlSchedulingForceBooking #hfICDDescription10").val(icd10Description);
                    $("#PnlSchedulingForceBooking #hfSNOMEDCode").val(snomedCode);
                    $("#PnlSchedulingForceBooking #hfSNOMEDDescription").val(snomedDescription);
                    if (Admin_IMOICD.params['fromIcon']) {
                        Admin_IMOICD.params['fromIcon'] = "false";
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else if (ParentControl.indexOf("EncounterChargeCapture") >= 0) {

                    var ContainerCtrl = SupperBillDetail.ContainerCtrl;
                    $("#txtICD" + ContainerCtrl).val(icd10Code);
                    $("#txtICD10Description" + ContainerCtrl).val(icd10Description);
                    $("#hfICD" + ContainerCtrl).val(icd9Code);
                    $("#hfICDDescription" + ContainerCtrl).val(icd9Description);
                    $("#hfICD10" + ContainerCtrl).val(icd10Code);
                    $("#hfICD10Description" + ContainerCtrl).val(icd10Description);
                    $("#hfSNOMED" + ContainerCtrl).val(snomedCode);
                    $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
                    $("#txtICD" + ContainerCtrl).parent("div").attr("title", icd10Description).attr("data-original-title", icd10Description).attr("data-toggle", "tooltip").attr("data-placement", "right");
                    //Set ToolTip for Comments.
                    $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                    EncounterChargeCapture.ValidateICDCodeAndSetDesc($("#txtICD" + ContainerCtrl));
                    EncounterChargeCapture.BindICDNValues($("#pnlEncounterChargeCapture"));
                    //Admin_IMOICD.UnLoadTab();

                }
                else if (ParentControl == "SupperBillDetail") {

                    var ContainerCtrl = SupperBillDetail.ContainerCtrl;
                    $("#SupperBillDetail #txtICD" + SupperBillDetail.ContainerCtrl).val(icd10Code);
                    $("#SupperBillDetail #hfICD" + ContainerCtrl).val(icd9Code);
                    $("#SupperBillDetail #txtICDDescription" + ContainerCtrl).val(icd10Description);
                    $("#SupperBillDetail #hfICDDescription" + ContainerCtrl).val(icd9Description);
                    $("#SupperBillDetail #txtICD10" + ContainerCtrl).val(icd10Code);
                    $("#SupperBillDetail #hfICD10" + ContainerCtrl).val(icd10Code);
                    $("#SupperBillDetail #hfICD10Description" + ContainerCtrl).val(icd10Description);
                    $("#SupperBillDetail #hfSNOMED" + ContainerCtrl).val(snomedCode);
                    $("#SupperBillDetail #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
                    $("#SupperBillDetail #hfLexiCode" + ContainerCtrl).val(lexiCode.substring(0, lexiCode.lastIndexOf("*")));
                }
                    //start//Abid Ali//For Out of Office Visit
                else if (ParentControl == "OutOfOfficeVisits") {
                    //Faizan
                    $("#pnlBillOutOfOfficeVisits #txtICDCode").val(icd10Code + ' - ' + icd10Description);
                    // $("#pnlBillOutOfOfficeVisits #txtICDCode").val(icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + icd10Description);
                    var icdCode = (icd10Code != null && icd10Code != "") ? icd10Code : icd9Code;
                    $("#pnlBillOutOfOfficeVisits #hfICDCode").val(icdCode);
                    //Begin Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
                    Admin_IMOICD.UnLoadTab();
                    $("#pnlBillOutOfOfficeVisits #txtICDCode").focus();
                    //End Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
                }
                    //End//Abid Ali//For Out of Office Visit
                else if (ParentControl == "Clinical_ProblemLists") {
                    Clinical_ProblemLists.ResetDiagnosis();
                    //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
                    $("#pnlClinicalProblemLists #ddlChronicityLevel").prop("disabled", false);
                    $("#pnlClinicalProblemLists #ddlSeverity").prop("disabled", false);
                    $("#pnlClinicalProblemLists #dpStartDate").prop("disabled", false);
                    $("#pnlClinicalProblemLists #dpEndDate").prop("disabled", false);
                    $("#pnlClinicalProblemLists #txtComments").prop("disabled", false);

                    if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                        $('#pnlClinicalProblemLists #ddlSeverity').val('Unspecified Severity');
                    } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                        $('#pnlClinicalProblemLists #ddlSeverity').val('Severe Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                        $('#pnlClinicalProblemLists #ddlSeverity').val('Moderate Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                        $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                        $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Intermittent');
                    } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                        $('#pnlClinicalProblemLists #ddlSeverity').val(6);
                    }

                    //Start || 11 April, 2016 || ZeeshanAK || Fix for showing SNOMED ID with ICD9 and 10 under Problem List screen
                    //$("#pnlClinicalProblemLists #txtDiagnosis").val(icd9Code + ' - ' + icd10Code + ' - ' + icd10Description);
                    $("#pnlClinicalProblemLists #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                    //End   || 11 April, 2016 || ZeeshanAK || Fix for showing SNOMED ID with ICD9 and 10 under Problem List screen

                    $("#pnlClinicalProblemLists #txtProblems").val(icd10Description);
                    // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
                    $("#pnlClinicalProblemLists #hfIMOProblem").val(icd10Description);
                    // End 19/01/2016 Muhammad Irfan for bug # EMR-219
                    var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
                    $('#pnlClinicalProblemLists #ulProblemDisease').html(li);
                    $("#pnlClinicalProblemLists #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');//kr

                    // Check if unload from Problem List main screen
                    var isUnload = "false";
                    var txtProblemInPnlClinicalProblemLists = $('#pnlClinicalProblemLists #txtProblems');
                    if (txtProblemInPnlClinicalProblemLists.is('[data-popupunload]')) {
                        isUnload = txtProblemInPnlClinicalProblemLists.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txtProblemInPnlClinicalProblemLists.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                    //if (icd10Description.indexOf('Cancer') > -1) {
                    //    var params = [];
                    //    params["FromAdmin"] = "0";
                    //    params["ParentCtrl"] = "Clinical_ProblemLists";

                    //    LoadActionPan("Clinical_ProblemDetails", params);
                    //}
                }
                else if (ParentControl == "Batch_ClinicalQualityMeasure") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {
                    }
                    else {
                        var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-12 col-lg-12'><a href='#'>" + icd10Code + ' - ' + icd10Description + "<span class='removeIconListHover' onclick='Batch_ClinicalQualityMeasure.deleteProblem($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                        var IsAlreadyExist = false;
                        $('#pnlBatchClinicalQualityMeasure #ulCQMProblemList li').each(function () {
                            if ($(this).attr('snomedCode') == snomedCode) {
                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlBatchClinicalQualityMeasure #ulCQMProblemList').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlBatchClinicalQualityMeasure #ulCQMProblemList').val('');
                            $('#pnlBatchClinicalQualityMeasure #txtProblems').val('');

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab){
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Problem List already added', 2);
                            $('#pnlBatchClinicalQualityMeasure #txtProblems').val('');
                        }
                    }
                }
                else if (ParentControl == "CCM_Patient_Hub") {
                    $("#" + CCM_Patient_Hub.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #ddlSeverity").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #dpStartDate").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #dpEndDate").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtComments").prop("disabled", false);

                    if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                        $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Unspecified Severity');
                    } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                        $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Severe Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                        $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Moderate Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                        $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                        $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Intermittent');
                    } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                        $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val(6);
                    }

                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val(icd10Description);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #hfIMOProblem").val(icd10Description);
                    var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
                    $("#" + CCM_Patient_Hub.params.PanelID + ' #ulProblemDisease').html(li);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');//kr
                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && Admin_IMOICD.params.length > 0)
                        Admin_IMOICD.UnLoadTab();
                }
                else if (ParentControl == "OrderSet_Problems") {
                    $("#" + OrderSet_Problems.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #ddlSeverity").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #dpStartDate").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #dpEndDate").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #txtComments").prop("disabled", false);

                    if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                        $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Unspecified Severity');
                    } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                        $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Severe Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                        $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Moderate Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                        $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Mild Persistent');
                    } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                        $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Mild Intermittent');
                    } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                        $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val(6);
                    }

                    $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                    $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val(icd10Description);
                    $("#" + OrderSet_Problems.params.PanelID + " #hfIMOProblem").val(icd10Description);
                    var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
                    $("#" + OrderSet_Problems.params.PanelID + ' #ulProblemDisease').html(li);
                    $("#" + OrderSet_Problems.params.PanelID + " #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');//kr
                   var isUnload = "false";
                   var txtProblem = $("#" + OrderSet_Problems.params.PanelID + " #txtProblems")
                   if (txtProblem.is('[data-popupunload]'))
                       isUnload = txtProblem.attr('data-popupunload');
                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txtProblem.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else if (ParentControl == "chargeSearchDetail") {
                    var ContainerCtrl = SupperBillDetail.ContainerCtrl;
                    $("#pnlBillChargeSearch #txtICD" + ContainerCtrl).val(icd10Code);

                    $("#pnlBillChargeSearch #hfICD" + ContainerCtrl).val(icd9Code);
                    $("#pnlBillChargeSearch #hfICDDescription" + ContainerCtrl).val(icd9Description);
                    $("#pnlBillChargeSearch #hfICD10" + ContainerCtrl).val(icd10Code);
                    $("#pnlBillChargeSearch #hfICD10Description" + ContainerCtrl).val(icd10Description);
                    $("#pnlBillChargeSearch #hfSNOMED" + ContainerCtrl).val(snomedCode);
                    $("#pnlBillChargeSearch #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);

                    setTimeout(function () { $("#pnlBillChargeSearch #txtICD" + ContainerCtrl).val(icd10Code); $("#hfICD" + ContainerCtrl).val(icd9Code); $("#hfICDDescription" + ContainerCtrl).val(icd9Description); $("#hfICD10" + ContainerCtrl).val(icd10Code); $("#hfICD10Description" + ContainerCtrl).val(icd10Description); $("#hfSNOMED" + ContainerCtrl).val(snomedCode); $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription); }, 110);
                }

                    //Start 8/02/2016 Muhammad Ahmad Imran Chief Compliant IMO search'
                else if (ParentControl == "Clinical_Complaints") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {
                        var currId = -1;
                        $("#pnlClinicalComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);
                        var li = "<li  id=" + currId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\">" +
                            "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "</span></a>" +
                                "<span class='removeIconListHover'>" +
                                    "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                        "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                        "</span>" +
                            "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                            "<div class='clearfix'></div>" +
                        "</li>";
                        var IsAlreadyExist = false;
                        $('#pnlClinicalComplaints #ulCompliantDisease li').each(function () {
                            if ($(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description) {
                                IsAlreadyExist = true;
                            }
                            else {
                                if ($(this).text().trim() == (icd10Code != '' ? icd10Code + " - " : "") + icd10Description.trim()) {
                                    IsAlreadyExist = true;
                                }
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlClinicalComplaints #txtComplaints').val('');
                            Clinical_Complaints.AddInArray(currId, icd10Description, true);//for record of complaints

                            var isUnload = "false";
                            var txt = $('#pnlClinicalComplaints #txtComplaints');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                            Clinical_Complaints.IsUpdate = true;
                        }
                        else {
                            utility.DisplayMessages('Disease already added', 2);

                            $('#pnlClinicalComplaints #txtComplaints').val('');
                        }
                    }
                }
                    //End 8/02/2016 Muhammad Ahmad Imran Chief Compliant IMO search'
                else if (ParentControl == "Clinical_HPIComplaints") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {
                        var currId = -1;
                        $("#pnlClinicalHPIComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);
                        var li = "<li  id=" + currId + " onclick='Clinical_HPIComplaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\">" +
                            "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "</span></a>" +
                                "<span class='removeIconListHover'>" +
                                    "<a href='#' onclick='Clinical_HPIComplaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                        "<a href='#' onclick='Clinical_HPIComplaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                        "</span>" +
                            "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_HPIComplaints.updateComplaintsText(this, event);'/>" +
                            "<div class='clearfix'></div>" +
                        "</li>";
                        var IsAlreadyExist = false;
                        $('#pnlClinicalHPIComplaints #ulCompliantDisease li').each(function () {
                            if ($(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description) {
                                IsAlreadyExist = true;
                            }
                            else {
                                if ($(this).text().trim() == (icd10Code != '' ? icd10Code + " - " : "") + icd10Description.trim()) {
                                    IsAlreadyExist = true;
                                }
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlClinicalHPIComplaints #ulCompliantDisease').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlClinicalHPIComplaints #txtComplaints').val('');
                            Clinical_HPIComplaints.AddInArray(currId, icd10Description, true);//for record of complaints

                            var isUnload = "false";
                            var txt = $('#pnlClinicalHPIComplaints #txtComplaints');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                            Clinical_HPIComplaints.IsUpdate = true;
                        }
                        else {
                            utility.DisplayMessages('Disease already added', 2);

                            $('#pnlClinicalHPIComplaints #txtComplaints').val('');
                        }
                    }
                }

                    //Start 22/03/2016 Muhammad Ahmad Imran favorite Chief Compliant IMO search'
                else if (ParentControl == "Favorite_Complaints_Detail") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {


                        var currId = -1;
                        $("#pnlFavoriteComplaintsDetail #frmFavoriteComplaintsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);
                        var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                        var IsAlreadyExist = false;
                        $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease li').each(function () {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlFavoriteComplaintsDetail #frmFavoriteComplaintsDetail #txtDiagnosis').val(icd9Code)
                            var bootstrapValidator = $('#pnlFavoriteComplaintsDetail #frmFavoriteComplaintsDetail').data('bootstrapValidator');
                             bootstrapValidator.revalidateField("Complaints");
                            $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                            Favorite_Complaints_Detail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints


                            var isUnload = "false";
                            var txt = $('#pnlFavoriteComplaintsDetail #txtDiagnosis');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Diagnosis already added', 2);

                            $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                        }
                    }

                }
                    //Start 22/03/2016 Muhammad Ahmad Imran Chief favorite Chief Compliant IMO search'
                    //-----------------------------------------------------------------kr
                    //Start 25/04/2016 Khaleel Ur Rehman favorite Problem Lists IMO search'
                else if (ParentControl == "Favorite_ProblemsDetail") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {


                        var currId = -1;
                        $("#pnlFavoriteProblemsDetail #frmFavoriteProblemsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);
                        var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_ProblemsDetail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                        var IsAlreadyExist = false;
                        $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease li').each(function () {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                            Favorite_ProblemsDetail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                            var isUnload = "false";
                            var txt = $('#pnlFavoriteProblemsDetail #txtDiagnosis');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Diagnosis already added', 2);

                            $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                        }
                    }

                }
                    //krFavorite_ProblemsDetail
                    //End 25/04/2016 Khaleel Ur Rehman favorite Problem Lists IMO search'
                    //------------------------------------------------------------------


                    //Start//02-03-2016//Ahmad Raza//CDS Detail IMO Search for ProblemList
                else if (ParentControl == "ClinicalCDSDetail") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {
                        var currId = -1;
                        $("#ClinicalCDSDetail #frmClinicalCDSDetail ul#ulCDSProblemList li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);


                        //Start//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                        if ($('#ClinicalCDSDetail #ulCDSProblemList li:first').length > 0) {
                            var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlProblemList" + currId + "' name = 'CDSProblemList" + snomedDescription + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + snomedDescription + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                        }
                        else {
                            var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + snomedDescription + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                        }
                        //End//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                        var IsAlreadyExist = false;
                        $('#ClinicalCDSDetail #ulCDSProblemList li').each(function () {
                            if ($(this).attr('name') == snomedDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#ClinicalCDSDetail #ulCDSProblemList').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#ClinicalCDSDetail #txtCDSProblemList').val('');

                            var isUnload = "false";
                            var txtProblemInCDS = $('#ClinicalCDSDetail #txtCDSProblemList');
                            if (txtProblemInCDS.is('[data-popupunload]')) {
                                isUnload = txtProblemInCDS.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txtProblemInCDS.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                        }
                        else {
                            utility.DisplayMessages('Problem List already added', 2);

                            $('#ClinicalCDSDetail #txtCDSProblemList').val('');
                        }
                    }
                }

                    //End//02-03-2016//Ahmad Raza//CDS Detail IMO Search for ProblemList


                    //Start//10-2-2017//Ahmad Imran//OrderSet Detail IMO Search for ProblemList
                else if (ParentControl == "Clinical_OrderSetDetails") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {
                        var currId = -1;
                        $("#pnlClinicalOrderSetDetails #frmOrderSetDetails ul#ulOrderSetProblemList li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);


                        //Start//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                        //if ($('#pnlClinicalOrderSetDetails #ulOrderSetProblemList li:first').length > 0) {
                        //var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + snomedDescription + "<span class='removeIconListHover' onclick='Clinical_OrderSetDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                        //}
                        //else {
                        var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-12 col-lg-12'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + icd10Description + "<span class='removeIconListHover' onclick='Clinical_OrderSetDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                        //}
                        //End//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                        var IsAlreadyExist = false;
                        $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList li').each(function () {
                            if ($(this).attr('icd10Code') == icd10Code) {
                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');

                            var isUnload = "false";
                            var txtProblemInCDS = $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList');
                            if (txtProblemInCDS.is('[data-popupunload]')) {
                                isUnload = txtProblemInCDS.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txtProblemInCDS.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                        }
                        else {
                            utility.DisplayMessages('Problem List already added', 2);

                            $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');
                        }
                    }
                }

                    //End//10-2-2017//Ahmad Imran//OrderSet Detail IMO Search for ProblemList


                    //Start//31-03-2016//Ahmad Raza//PlanOfCare IMO Search for Goal
                else if (ParentControl == "Clinical_PlanOfCare") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {
                        var currId = -1;
                        $("#pnlPlanOfCare #frmPlanOfCare #ulPlanOfCareGoal li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);

                        var li = "<li  id=" + currId + " onclick = 'Clinical_PlanOfCare.fillPlanOfCareGoal(this, event);' onmouseover='Clinical_PlanOfCare.showIcon(this);' onmouseout='Clinical_PlanOfCare.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_PlanOfCare.activeInActive($(this), event);'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_PlanOfCare.deletePlanOfCareGoal($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"


                        var IsAlreadyExist = false;
                        $('#pnlPlanOfCare #ulPlanOfCareGoal li').each(function () {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlPlanOfCare #ulPlanOfCareGoal').append(li);
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlPlanOfCare #txtGoal').val('');
                            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);
                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab)
                                Admin_IMOICD.UnLoadTab();
                        }
                        else {
                            utility.DisplayMessages('Goal already added', 2);

                            $('#pnlPlanOfCare #txtGoal').val('');
                        }
                    }
                }



                else if (ParentControl == "Clinical_Cognitive") {
                    if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                    }
                    else {
                        if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtCognitiveStatus') {
                            var currId = -1;
                            $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulCognitiveStatus li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });
                            currId = parseInt(currId) + (-1);

                            var status = "Status";
                            var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + status + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + status + "\");'><i class='fa fa-close'></i></span></a></li>"


                            var IsAlreadyExist = false;
                            $('#' + Clinical_Cognitive.params.PanelID + ' #ulCognitiveStatus li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#' + Clinical_Cognitive.params.PanelID + ' #ulCognitiveStatus').append(li);
                                $(li).trigger('click');
                                $('.modal-backdrop').removeClass('in');
                                $('.modal-backdrop').addClass('out');
                                $('.modal-backdrop').hide();
                                $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').val('');
                                //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                                var isUnload = "false";
                                var txt = $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Cognitive Status already added', 2);

                                $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').val('');
                            }
                        }

                        else if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtFunctionalStatus') {
                            var currId = -1;
                            $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulFunctionalStatus li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });
                            currId = parseInt(currId) + (-1);
                            var functionalStatus = "FunctionalStatus";
                            var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + functionalStatus + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_Cognitive.activeInActiveFunctionalStatus($(this), event);'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + functionalStatus + "\");'><i class='fa fa-close'></i></span></a></li>"


                            var IsAlreadyExist = false;
                            $('#' + Clinical_Cognitive.params.PanelID + ' #ulFunctionalStatus li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#' + Clinical_Cognitive.params.PanelID + ' #ulFunctionalStatus').append(li);
                                $(li).trigger('click');
                                $('.modal-backdrop').removeClass('in');
                                $('.modal-backdrop').addClass('out');
                                $('.modal-backdrop').hide();
                                $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').val('');
                                //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                                var isUnload = "false";
                                var txt = $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Functional Status already added', 2);

                                $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').val('');
                            }
                        }
                        else if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtMentalStatus') {
                            var currId = -1;
                            $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulMentalStatus li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });
                            currId = parseInt(currId) + (-1);
                            var MentalStatus = "MentalStatus";
                            var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + MentalStatus + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_Cognitive.activeInActiveMentalStatus($(this), event);'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + MentalStatus + "\");'><i class='fa fa-close'></i></span></a></li>"


                            var IsAlreadyExist = false;
                            $('#' + Clinical_Cognitive.params.PanelID + ' #ulMentalStatus li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#' + Clinical_Cognitive.params.PanelID + ' #ulMentalStatus').append(li);
                                $(li).trigger('click');
                                $('.modal-backdrop').removeClass('in');
                                $('.modal-backdrop').addClass('out');
                                $('.modal-backdrop').hide();
                                $('#' + Clinical_Cognitive.params.PanelID + ' #txtMentalStatus').val('');
                                //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                                var isUnload = "false";
                                var txt = $('#' + Clinical_Cognitive.params.PanelID + ' #txtMentalStatus');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Functional Status already added', 2);

                                $('#' + Clinical_Cognitive.params.PanelID + ' #txtMentalStatus').val('');
                            }
                        }


                    }


                }
                    //End//31-03-2016//Ahmad Raza//PlanOfCare IMO Search for Goal









                    //Start 11/01/2016 Muhammad Irfan MedicalHx IMO search
                else if (ParentControl == "Clinical_MedicalHx" || ParentControl.toLowerCase() == "clinicaltabmedicalhx") {


                    var currId = -1;
                    $("#pnlClinicalMedicalHx #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li  id=" + currId + " onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd9Description + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                    var IsAlreadyExist = false;


                    $('#pnlClinicalMedicalHx #ulMedicalDisease li').each(function () {
                        if ($(this).attr('icd9Code') == null) {
                            if ($(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                IsAlreadyExist = true;
                            }
                        }
                        else {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                IsAlreadyExist = true;
                            }
                        }
                    });


                    if (!IsAlreadyExist) {
                        $('#pnlClinicalMedicalHx #ulMedicalDisease').append(li);
                        $(li).trigger('click');
                        $('#pnlClinicalMedicalHx #txtDisease').val('');

                        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                            var disease = $(li).get(0).outerHTML;
                            var diseaseDetails = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").clone();
                            $(diseaseDetails).resetAllControls(null);
                            var diseaseData = $(diseaseDetails).getMyJSONByName();
                            Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData, disease);
                        }

                        var isUnload = "false";
                        var txt = $('#pnlClinicalMedicalHx #txtDisease');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {

                        utility.DisplayMessages('Disease already added', 2);
                        $('#pnlClinicalMedicalHx #txtDisease').val('');
                    }

                }
                    //End 11/01/2016 Muhammad Irfan MedicalHx IMO search
                    //Start 20/01/2016 Farooq Ahmad HospitalizationHx IMO search
                else if (ParentControl == "Clinical_HospitalizationHx") {


                    var currId = -1;
                    //Start 11/02/2016 Abid Ali, for bug# 311
                    $("#pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx #HospitalizationHxDisease ul#ulHospitalizationDisease li[id*='-']").each(function (i, item) {
                        //End 11/02/2016 Abid Ali, for bug# 311
                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li  id=" + currId + " onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
                    var IsAlreadyExist = false;
                    $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease li').each(function () {
                        if ($(this).attr('icd9Code') == null) {
                            if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                IsAlreadyExist = true;
                            }
                        }
                        else {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                IsAlreadyExist = true;
                            }
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease').append(li);

                        $(li).trigger('click');
                        $('#pnlClinicalHospitalizationHx #txtDisease').val('');

                        if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                            var disease = $(li).get(0).outerHTML;
                            var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                            Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData, disease);
                        }

                        var isUnload = "false";
                        var txt = $('#pnlClinicalHospitalizationHx #txtDisease');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Disease already added', 2);

                        $('#pnlClinicalHospitalizationHx #txtDisease').val('');
                    }

                }
                    //End 20/01/2016 Farooq Ahmad HospitalizationHx IMO search
                    /*Start 20/01/2016 Muhammad Irfan Clinical_FamilyHx IMO search*/
                else if (ParentControl == "Clinical_FamilyHx") {

                    var currId = -1;
                    var AccessPanelId = "pnlClinicalFamilyHx";
                    if (AccessPanel != null && typeof AccessPanel != "undefined") {
                        AccessPanelId = AccessPanel;
                    }
                    $("#" + AccessPanelId + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    //Start 04-11-2016 Humaira Yousaf to change onclick event
                    //Start 19-10-2017 Humaira Yousaf MU3-5
                    var info = icd10Code + " - (" + snomedCode + ") - \n" + icd10Description;
                    globalAppdata["isMU3FamilyHistory"] && globalAppdata["isMU3FamilyHistory"].toLowerCase() == "false" ? info = "" : "";
                    var li = "<li title = \"" + info + "\"  id=" + currId + " onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\" MemberDetailId=\"" + currId + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";
                    //End 19-10-2017 Humaira Yousaf MU3-5
                    //End 04-11-2016 Humaira Yousaf to change onclick event

                    var IsAlreadyExist = false;
                    $('#' + AccessPanelId + ' #ulFamilyHxDisease li').each(function () {

                        if ($(this).attr('icd9Desc') == null) {
                            if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                IsAlreadyExist = true;
                            }
                        } else {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                IsAlreadyExist = true;
                            }
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#' + AccessPanelId + ' #ulFamilyHxDisease').append(li);

                        //Start 04-11-2016 Humaira Yousaf
                        $(li).trigger('click');
                        //Clinical_FamilyHx.fillFamilyHxDisease($(li));
                        //End 04-11-2016 Humaira Yousaf
                        $('#' + AccessPanelId + ' #txtDisease').val('');

                        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            var diseaseId = currId;
                            var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                            var disease = $(li).get(0).outerHTML;
                            var detailsdata = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').clone();
                            $(detailsdata).resetAllControls(null);
                            var diseaseData = $(detailsdata).getMyJSONByName();
                            Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, diseaseData, disease);
                            $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyMember li.active").find("a").css("color", "green");

                        }


                        var isUnload = "false";
                        var txt = $('#' + AccessPanelId + ' #txtDisease');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Diagnose already added', 2);

                        $('#' + AccessPanelId + ' #txtDisease').val('');
                    }
                }
                /*End 20/01/2016 Muhammad Irfan Clinical_FamilyHx IMO search*/

                if (ParentControl == "Clinical_CustomFormsPreview") {
                    if (txtBoxCtrlParentId) {
                        var selectedProblemTool = $("#pnlClinicalCustomFormsPreview #frmCustomFormPreview").find('#' + txtBoxCtrlParentId);
                        if (selectedProblemTool) {

                            var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');
                            var currId = -1;
                            $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                                currId = $(this).attr("id");
                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li isnew='true'  id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                            var IsAlreadyExist = false;

                            $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                                if ($(this).attr('icd9Code') == null) {
                                    if (!$(this).hasClass('hidden') && $(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                            if (!IsAlreadyExist) {

                                var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                                if (problemsExist == 0) {
                                    var problemsDiv = $('#pnlClinicalCustomFormsPreview #frmCustomFormPreview .problemsListDiv').clone().removeClass('problemsListDiv');
                                    $(problemsDiv).find("ul#customFormProblemsList").append(li);
                                    $(problemsDiv).css('display', 'block');
                                    $(selectedProblemTool).append($(problemsDiv));
                                    $(selectedProblemTool).css('height', 'auto');
                                }
                                else {
                                    $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                                    $(selectedProblemTool).children().last().css('display', 'block');
                                }
                                Clinical_CustomFormsPreview.saveProblem(li, txtBoxCtrlParentId);

                                $(txtBoxCtrl).val('');
                                if ($(txtBoxCtrl).attr('style')) {
                                    $(txtBoxCtrl).removeAttr('style');
                                }

                                var isUnload = "false";
                                if ($(txtBoxCtrl).is('[data-popupunload]')) {
                                    isUnload = $(txtBoxCtrl).attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    $(txtBoxCtrl).attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Problem already added', 2);
                                $(txtBoxCtrl).val('');
                            }

                        }
                    }
                }

                if (ParentControl == "Batch_FaxSend") {
                    if (txtBoxCtrlParentId) {
                        var selectedProblemTool = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend').find('#' + txtBoxCtrlParentId);
                        if (selectedProblemTool) {

                            var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');
                            var currId = -1;
                            $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                                currId = $(this).attr("id");
                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li isnew='true'  id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                            var IsAlreadyExist = false;

                            $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                                if ($(this).attr('icd9Code') == null) {
                                    if (!$(this).hasClass('hidden') && $(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                            if (!IsAlreadyExist) {

                                var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                                if (problemsExist == 0) {
                                    var problemsDiv = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend .problemsListDiv').clone().removeClass('problemsListDiv');
                                    $(problemsDiv).find("ul#customFormProblemsList").append(li);
                                    $(problemsDiv).css('display', 'block');
                                    $(selectedProblemTool).append($(problemsDiv));
                                    $(selectedProblemTool).css('height', 'auto');
                                }
                                else {
                                    $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                                    $(selectedProblemTool).children().last().css('display', 'block');
                                }

                                $(txtBoxCtrl).val('');
                                if ($(txtBoxCtrl).attr('style')) {
                                    $(txtBoxCtrl).removeAttr('style');
                                }

                                var isUnload = "false";
                                if ($(txtBoxCtrl).is('[data-popupunload]')) {
                                    isUnload = $(txtBoxCtrl).attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    $(txtBoxCtrl).attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Problem already added', 2);
                                $(txtBoxCtrl).val('');
                            }

                        }
                    }
                }

                    //Start// 11/06/2016 //Ahmad Raza// FamilyHx Favorite list IMO search
                else if (ParentControl == "Favorite_FamilyHistoryDetail") {

                    var currId = -1;
                    $("#pnlFavoriteFamilyHistoryDetail #frmFavoriteFamilyHistoryDetail ul#ulFavFamilyHistoryDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_FamilyHistoryDetail.showIcon(this);' onmouseout='Favorite_FamilyHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_FamilyHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                    var IsAlreadyExist = false;
                    $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease').append(li);
                        $(li).trigger('click');
                        var item = {};
                        item["ICD9"] = icd9Code;
                        item["ICD10"] = icd10Code;
                        item["ICD10Description"] = icd10Description;
                        item["ICD9Description"] = icd9Description;
                        item["SNOMEDDescription"] = snomedDescription;
                        item["SNOMED"] = snomedCode;
                        Favorite_FamilyHistoryDetail.CPTData.push(item);
                        $('#pnlFavoriteFamilyHistoryDetail #frmFavoriteFamilyHistoryDetail #txtDiagnosis').val(icd9Code)
                        var bootstrapValidator = $('#pnlFavoriteFamilyHistoryDetail #frmFavoriteFamilyHistoryDetail').data('bootstrapValidator');
                        var isvalid=bootstrapValidator.revalidateField("Diagnosis");
                        $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');

                        var isUnload = "false";
                        var txt = $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Diagnose already added', 2);

                        $('#pnlFavoriteFamilyHistoryDetail #txtDiagnose').val('');
                    }
                    //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                }
                    //End// 11/06/2016 //Ahmad Raza// FamilyHx Favorite list IMO search



                else if (ParentControl == "Favorite_HospitalizationHistoryDetail") {

                    var currId = -1;
                    $("#pnlFavoriteHospitalizationHistoryDetail #frmFavoriteHospitalizationHistoryDetail ul#ulFavHospitalizationHistoryDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_HospitalizationHistoryDetail.showIcon(this);' onmouseout='Favorite_HospitalizationHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_HospitalizationHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                    var IsAlreadyExist = false;
                    $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease').append(li);
                        $(li).trigger('click');
                        var item = {};
                        item["ICD9"] = icd9Code;
                        item["ICD10"] = icd10Code;
                        item["ICD10Description"] = icd10Description;
                        item["ICD9Description"] = icd9Description;
                        item["SNOMEDDescription"] = snomedDescription;
                        item["SNOMED"] = snomedCode;
                        Favorite_HospitalizationHistoryDetail.CPTData.push(item);
                        $('#pnlFavoriteHospitalizationHistoryDetail #frmFavoriteHospitalizationHistoryDetail #txtDiagnosis').val(icd9Description)
                        var bootstrapValidator = $('#pnlFavoriteHospitalizationHistoryDetail #frmFavoriteHospitalizationHistoryDetail').data('bootstrapValidator');
                         bootstrapValidator.revalidateField("Diagnosis");
                        $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis').val('');

                        var isUnload = "false";
                        var txt = $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Diagnose already added', 2);

                        $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnose').val('');
                    }
                }

                    //Start// 11/06/2016 //Ahmad Raza// MedicalHx Favorite list IMO search
                else if (ParentControl == "Favorite_MedicalHistoryDetail") {

                    var currId = -1;
                    $("#pnlFavoriteMedicalHistoryDetail #frmFavoriteMedicalHistoryDetail ul#ulFavMedicalHistoryDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_MedicalHistoryDetail.showIcon(this);' onmouseout='Favorite_MedicalHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_MedicalHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                    var IsAlreadyExist = false;
                    $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease').append(li);
                        $(li).trigger('click');
                        var item = {};
                        item["ICD9"] = icd9Code;
                        item["ICD10"] = icd10Code;
                        item["ICD10Description"] = icd10Description;
                        item["ICD9Description"] = icd9Description;
                        item["SNOMEDDescription"] = snomedDescription;
                        item["SNOMED"] = snomedCode;
                        Favorite_MedicalHistoryDetail.CPTData.push(item);
                        $('#pnlFavoriteMedicalHistoryDetail #frmFavoriteMedicalHistoryDetail #txtDiagnosis').val(icd9Description)
                        var bootstrapValidator = $('#pnlFavoriteMedicalHistoryDetail #frmFavoriteMedicalHistoryDetail').data('bootstrapValidator');
                         bootstrapValidator.revalidateField("Diagnosis");
                        $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis').val('');


                        var isUnload = "false";
                        var txt = $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Diagnose already added', 2);

                        $('#pnlFavoriteMedicalHistoryDetail #txtDiagnose').val('');
                    }
                    //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                }
                    //End// 11/06/2016 //Ahmad Raza// MedicalHx Favorite list IMO search

                    //Start// 11/06/2016 //Ahmad Raza// SurgicalHx Favorite list IMO search
                else if (ParentControl == "Favorite_SurgicalHistoryDetail") {

                    var currId = -1;
                    $("#pnlFavoriteSurgicalHistoryDetail #frmFavoriteSurgicalHistoryDetail ul#ulFavSurgicalHistoryDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_SurgicalHistoryDetail.showIcon(this);' onmouseout='Favorite_SurgicalHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Code + " - " + icd9Description + "<span class='removeIconListHover' onclick='Favorite_SurgicalHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                    var IsAlreadyExist = false;
                    $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease').append(li);
                        $(li).trigger('click');
                        var item = {};
                        item["ICD9"] = icd9Code;
                        item["ICD10"] = icd10Code;
                        item["ICD10Description"] = icd10Description;
                        item["ICD9Description"] = icd9Description;
                        item["SNOMEDDescription"] = snomedDescription;
                        item["SNOMED"] = snomedCode;
                        Favorite_SurgicalHistoryDetail.CPTData.push(item);

                        $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnosis').val('');

                        var isUnload = "false";
                        var txt = $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnosis');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Diagnose already added', 2);

                        $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnose').val('');
                    }
                    //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                }
                    //End// 11/06/2016 //Ahmad Raza// SurgicalHx Favorite list IMO search

                    //Start 21/01/2016 Syed zia Surgical history IMO search
                else if (ParentControl == "Clinical_SurgicalHx") {


                    var currId = -1;
                    $("#pnlClinicalSurgicalHx #frmClinicalSurgicalHx #Surgical ul#ulSurgicalDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);
                    /*Start 28/01/2016 Abid Ali, bug reslove : changed function call on delete*/
                    var li = "<li  id=" + currId + " onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' onmouseover='Clinical_SurgicalHx.showIcon(this);' onmouseout='Clinical_SurgicalHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
                    /*End  28/01/2016 Abid Ali, bug reslove : changed function call on delete*/
                    var IsAlreadyExist = false;
                    $('#pnlClinicalSurgicalHx #ulSurgicalDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlClinicalSurgicalHx #ulSurgicalDisease').append(li);
                        $(li).trigger('click');
                        $('#pnlClinicalSurgicalHx #txtDisease').val('');

                        //if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        //    var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                        //    var disease = $(li).get(0).outerHTML;
                        //    var diseaseData = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName();
                        //    Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData, disease);
                        //}

                        var isUnload = "false";
                        var txt = $('#pnlClinicalSurgicalHx #txtDisease');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Diagnose already added', 2);
                        $('#pnlClinicalSurgicalHx #txtDisease').val('');
                    }

                }
                    //end 21/01/2016 Syed zia Surgical history IMO search

                else if (ParentControl.indexOf("Admin_IMOICD") >= 0) {

                    if (SupperBillDetail.ContainerCtrl != "undefined") {
                        var ContainerCtrl = SupperBillDetail.ContainerCtrl;
                        var RefHiddenCtrlArray = [];
                        RefHiddenCtrlArray = ContainerCtrl.split(",");
                        if (RefHiddenCtrlArray.length > 1) {

                            var ctrl = RefHiddenCtrlArray[0].substring(5);
                            $("#txtICD" + ctrl).val(icd10Code);
                            $(" #" + RefHiddenCtrlArray[0]).val(icd9Code);
                            $(" #" + RefHiddenCtrlArray[1]).val(icd9Description);
                            $(" #" + RefHiddenCtrlArray[2]).val(icd10Code);
                            $(" #" + RefHiddenCtrlArray[3]).val(icd10Description);
                            $(" #" + RefHiddenCtrlArray[4]).val(snomedCode);
                            $(" #" + RefHiddenCtrlArray[5]).val(snomedDescription);
                            $("#txtICD" + ctrl).parent("div").attr("data-original-title", icd10Description);
                        }
                    }
                    else {
                        $("#SupperBillDetail #txtICD_0").val(icd9Code);
                        $("#SupperBillDetail #txtICDDescription_0").val(icd9Description);
                        $("#SupperBillDetail #txtICD10_0").val(icd10Code);
                        $("#SupperBillDetail #hfICD10Description_0").val(icd10Description);
                        $("#SupperBillDetail #hfSNOMED_0").val(snomedCode);
                        $("#SupperBillDetail #hfSNOMEDDescription_0").val(snomedDescription);
                        $("#SupperBillDetail #hfLexiCode_0").val(snomedCode);
                    }

                    if (text.indexOf("EncounterChargeCapture") >= 0)
                        EncounterChargeCapture.BindICDNValues($("#pnlEncounterChargeCapture"));

                    Admin_IMOICD.UnLoadTab();
                }

                else if (ParentControl == "Clinical_CarePlan") {
                    if (txtBoxCtrlParentId == "Goals") {
                        var currId = -1;
                        var AccessPanelId = "pnlClinicalCarePlan";
                        if (AccessPanel != null && typeof AccessPanel != "undefined") {
                            AccessPanelId = AccessPanel;
                        }
                        $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanGoals ul#ulCarePlanGoals li[id*='-']").each(function (i, item) {
                            currId = $(this).attr("id");
                        });

                        currId = parseInt(currId) + (-1);

                        var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillGoalDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\" MemberDetailId=\"" + currId + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.GoalDelete(\"" + currId + "\",true,event);'><i class='fa fa-close'></i></span></a></li>";

                        var IsAlreadyExist = false;
                        $('#' + AccessPanelId + ' #ulCarePlanGoals li').each(function () {

                            if ($(this).attr('icd9Desc') == null) {
                                if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                    IsAlreadyExist = true;
                                }
                            } else {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                    $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                    $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                    IsAlreadyExist = true;
                                }
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#' + AccessPanelId + ' #ulCarePlanGoals').append(li);
                            if ($('#' + AccessPanelId + ' #sectionGoals').hasClass('disableAll') == true) {
                                $('#' + AccessPanelId + ' #sectionGoals').removeClass('disableAll');
                            }
                            $(li).trigger('click');
                            $('#' + AccessPanelId + ' #txtDisease').val('');                            
                            $('#' + AccessPanelId + ' #Goals #DivICDAutoComplete').addClass('disableAll');
                            $('#' + AccessPanelId + ' #txtDisease').attr('readonly', true);
                            var isUnload = "false";
                            var txt = $('#' + AccessPanelId + ' #txtDisease');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Diagnose already added', 2);

                            $('#' + AccessPanelId + ' #txtDisease').val('');
                        }
                    }
                    else if (txtBoxCtrlParentId == "Concern" || txtBoxCtrlParentId == "Observation" || txtBoxCtrlParentId == "Risk") {

                        var currId = -1;
                        var AccessPanelId = "pnlClinicalCarePlan";
                        if (AccessPanel != null && typeof AccessPanel != "undefined") {
                            AccessPanelId = AccessPanel;
                        }
                        var containerId = "";
                        if (txtBoxCtrlParentId == "Concern") {
                            containerId = AccessPanelId + ' #txtConcern';
                            $('#' + containerId).attr('concernId', currId);
                            $('#' + AccessPanelId + ' #concernInfoDiv').removeClass('disableAll');
                        }
                        else if (txtBoxCtrlParentId == "Observation") {
                            containerId = AccessPanelId + ' #txtObservation';
                            $('#' + containerId).attr('obsevationId', currId);
                            $('#' + AccessPanelId + ' #observationInfoDiv').removeClass('disableAll');
                        }
                        else if (txtBoxCtrlParentId == "Risk") {
                            containerId = AccessPanelId + ' #txtRisk';
                            $('#' + containerId).attr('riskId', currId);
                            $('#' + AccessPanelId + ' #riskInfoDiv').removeClass('disableAll');
                        }
                        $('#' + containerId).val(icd10Description);

                        $('#' + containerId).attr('icd9Code', icd9Code);
                        $('#' + containerId).attr('icd9Description', icd9Description);
                        $('#' + containerId).attr('icd10Code', icd10Code);
                        $('#' + containerId).attr('icd10Description', icd10Description);
                        $('#' + containerId).attr('snomedCode', snomedCode);
                        $('#' + containerId).attr('snomedDescription', snomedDescription);
                       
                            var isUnload = "false";
                            var txt = $('#' + containerId);
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }                        
                    }

                    else if (txtBoxCtrlParentId == "Intervention") {
                        var currId = -1;
                        var AccessPanelId = "pnlClinicalCarePlan";
                        if (AccessPanel != null && typeof AccessPanel != "undefined") {
                            AccessPanelId = AccessPanel;
                        }
                        $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanInterventions ul#ulCarePlanInterventions li[id*='-']").each(function (i, item) {
                            currId = $(this).attr("id");
                        });

                        currId = parseInt(currId) + (-1);

                        var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillInterventionDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.InterventionDelete(\"" + currId + "\",true,event);'><i class='fa fa-close'></i></span></a></li>";

                        var IsAlreadyExist = false;
                        $('#' + AccessPanelId + ' #ulCarePlanInterventions li').each(function () {

                            if ($(this).attr('icd9Desc') == null) {
                                if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                    IsAlreadyExist = true;
                                }
                            } else {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                    $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                    $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                    IsAlreadyExist = true;
                                }
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#' + AccessPanelId + ' #ulCarePlanInterventions').append(li);
                            if ($('#' + AccessPanelId + ' #sectionInterventions').hasClass('disableAll') == true) {
                                $('#' + AccessPanelId + ' #sectionInterventions').removeClass('disableAll');
                            }
                            $(li).trigger('click');
                            $('#' + AccessPanelId + ' #txtInterventions').val('');

                            var isUnload = "false";
                            var txt = $('#' + AccessPanelId + ' #txtInterventions');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Intervention already added', 2);
                            $('#' + AccessPanelId + ' #txtInterventions').val('');
                        }
                    }
                    else if (txtBoxCtrlParentId == "Outcome") {
                        var currId = -1;
                        var AccessPanelId = "pnlClinicalCarePlan";
                        if (AccessPanel != null && typeof AccessPanel != "undefined") {
                            AccessPanelId = AccessPanel;
                        }
                        $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanOutcomes ul#ulCarePlanOutcomes li[id*='-']").each(function (i, item) {
                            currId = $(this).attr("id");
                        });

                        currId = parseInt(currId) + (-1);

                        var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillOutcomeDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.OutcomeDelete(\"" + currId + "\",true,event);'><i class='fa fa-close'></i></span></a></li>";

                        var IsAlreadyExist = false;
                        $('#' + AccessPanelId + ' #ulCarePlanOutcomes li').each(function () {

                            if ($(this).attr('icd9Desc') == null) {
                                if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                    IsAlreadyExist = true;
                                }
                            } else {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                    $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                    $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                    IsAlreadyExist = true;
                                }
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#' + AccessPanelId + ' #ulCarePlanOutcomes').append(li);
                            if ($('#' + AccessPanelId + ' #sectionOutcomes').hasClass('disableAll') == true) {
                                $('#' + AccessPanelId + ' #sectionOutcomes').removeClass('disableAll');
                            }
                            $(li).trigger('click');
                            $('#' + AccessPanelId + ' #txtOutcomes').val('');

                            var isUnload = "false";
                            var txt = $('#' + AccessPanelId + ' #txtOutcomes');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Outcome already added', 2);
                            $('#' + AccessPanelId + ' #txtOutcomes').val('');
                        }
                    }
                }
                else if (ParentControl == "IA_DiabetesScreening") {
                    var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
                    var IsAlreadyExist = false;
                    $('#pnlDiabtesScreening #ulProblemDisease li').each(function () {
                        if ($(this).attr('snomedCode') == snomedCode) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlDiabtesScreening #ulProblemDisease').append(li);
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtProblems").val(icd10Code);
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICDAndDescription").val(text);
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD9Code").val(icd9Code);
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD9Description").val(icd9Description);
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD10Code").val(icd10Code); // Selected List Item ID i.e. [ICD10 Code]
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD10Description").val(icd10Description); // Selected List Item text i.e. [ICD10 description]
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtSnomedCode").val(snomedCode);
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtSnomedDescription").val(snomedDescription);
                        $("#pnlDiabtesScreening #frmClinicalDiabetesLists #hfLexiCode").val(IMODetail.params.lexiCodeId);
                        
                        
                    }
                    else {
                        utility.DisplayMessages('Problem List already added', 2);
                    }
                    // Check if unload from Problem List main screen
                    var isUnload = "false";
                    var txtProblemInPnlClinicalProblemLists = $('#pnlDiabtesScreening #txtProblems');
                    if (txtProblemInPnlClinicalProblemLists.is('[data-popupunload]')) {
                        isUnload = txtProblemInPnlClinicalProblemLists.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txtProblemInPnlClinicalProblemLists.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                if (IMODetail.params.Ctrltext == "mstrTabReports" || IMODetail.params.ParentCtrl == "mstrTabReports" || ParentControl == "mstrTabReports" || text == "mstrTabReports") {
                    $("#pnlReportsSSRSDashboard #txtICD").val(icd10Code);
                    $("#pnlReportsSSRSDashboard #hfICDCode").val(icd10Code);

                    $("#pnlReportsSSRSDashboard #txtProblems").val(icd10Description);
                    $("#pnlReportsSSRSDashboard #hfICD10Code").val(icd10Code);
                    
                    $("#pnlReportsSSRSDashboard #hfICD10Description").val(icd10Description);
                    $("#pnlReportsSSRSDashboard #hdnProblemName").val("");
                }
            }
                //--------------------kr
                // else if (LocalOrIMO == "imo" && ParentControl == "Clinical_ProblemLists") {
            else if ((response.IMOCount == undefined || response.IMOCount == 'undefined') && LocalOrIMO == "imo" && ParentControl == "Clinical_ProblemLists") {
                if (typeof lexiCode != 'undefined' && lexiCode != null && lexiCode.length > 0) {
                    var ICD9CodePlusICD9Title = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                    var ICD10CodePlusICD10Title = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                    var SNOMEDCodePlusSNOMEDTitle = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);

                    var icd9Code = ICD9CodePlusICD9Title.split("+")[0];
                    var icd9Description = ICD9CodePlusICD9Title.split("+")[1];
                    var icd10Code = ICD10CodePlusICD10Title.split("+")[0];
                    var icd10Description = ICD10CodePlusICD10Title.split("+")[1];
                    var snomedCode = SNOMEDCodePlusSNOMEDTitle.split("+")[0];
                    var snomedDescription = SNOMEDCodePlusSNOMEDTitle.split("+")[1];
                    if (snomedDescription.indexOf('^') >= 0) {
                        snomedDescription = snomedDescription.split('^')[0];
                    }

                    //       if (ParentControl == "Clinical_ProblemLists") {
                    $("#pnlClinicalProblemLists #txtDiagnosis").val('');
                    $("#pnlClinicalProblemLists #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                    $("#pnlClinicalProblemLists #hfIMOProblem").val($("#pnlClinicalProblemLists #txtProblems").val());

                    $("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", true);
                    $("#pnlClinicalProblemLists #ddlChronicityLevel").prop("disabled", false);
                    $("#pnlClinicalProblemLists #ddlSeverity").prop("disabled", false);
                    $("#pnlClinicalProblemLists #dpStartDate").prop("disabled", false);
                    $("#pnlClinicalProblemLists #dpEndDate").prop("disabled", false);
                    $("#pnlClinicalProblemLists #txtComments").prop("disabled", false);

                    Clinical_ProblemLists.icdsValues["ICD9"] = icd9Code;
                    Clinical_ProblemLists.icdsValues["ICD10"] = icd10Code;
                    Clinical_ProblemLists.icdsValues["ICD9_Description"] = icd9Description;
                    Clinical_ProblemLists.icdsValues["ICD10_Description"] = icd10Description;
                    Clinical_ProblemLists.icdsValues["SNOMEDID"] = snomedCode;
                    Clinical_ProblemLists.icdsValues["SNOMED_DESCRIPTION"] = snomedDescription;
                    $("#pnlClinicalProblemLists #txtProblems").val(icd10Description);
                    $("#pnlClinicalProblemLists #hfIMOProblem").val($("#pnlClinicalProblemLists #txtProblems").val());
                    //      }

                }
            }

            else if ((response.IMOCount == undefined || response.IMOCount == 'undefined') && LocalOrIMO == "imo" && ParentControl == "Clinical_CustomFormsPreview") {
                if (lexiCode && lexiCode.length > 0) {
                    if (txtBoxCtrlParentId) {

                        var ICD9CodePlusICD9Title = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                        var ICD10CodePlusICD10Title = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                        var SNOMEDCodePlusSNOMEDTitle = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);

                        var icd9Code = ICD9CodePlusICD9Title.split("+")[0];
                        var icd9Description = ICD9CodePlusICD9Title.split("+")[1];
                        var icd10Code = ICD10CodePlusICD10Title.split("+")[0];
                        var icd10Description = ICD10CodePlusICD10Title.split("+")[1];
                        var snomedCode = SNOMEDCodePlusSNOMEDTitle.split("+")[0];
                        var snomedDescription = SNOMEDCodePlusSNOMEDTitle.split("+")[1];
                        if (snomedDescription.indexOf('^') >= 0) {
                            snomedDescription = snomedDescription.split('^')[0];
                        }

                        var selectedProblemTool = $("#pnlClinicalCustomFormsPreview #frmCustomFormPreview").find('#' + txtBoxCtrlParentId);
                        if (selectedProblemTool) {
                            var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');
                            var currId = -1;
                            $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                                currId = $(this).attr("id");
                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                            var IsAlreadyExist = false;

                            $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                                if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == null) {
                                    if ($(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                        }

                        if (!IsAlreadyExist) {

                            var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                            if (problemsExist == 0) {
                                var problemsDiv = $('#pnlClinicalCustomFormsPreview #frmCustomFormPreview .problemsListDiv').clone().removeClass('problemsListDiv');
                                $(problemsDiv).find("ul#customFormProblemsList").append(li);
                                $(problemsDiv).css('display', 'block');
                                $(selectedProblemTool).append($(problemsDiv));
                                $(selectedProblemTool).css('height', 'auto');
                            }
                            else {
                                $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                                $(selectedProblemTool).children().last().css('display', 'block');
                            }

                            Clinical_CustomFormsPreview.saveProblem(li, txtBoxCtrlParentId);

                            $(txtBoxCtrl).val('');
                            if ($(txtBoxCtrl).attr('style')) {
                                $(txtBoxCtrl).removeAttr('style');
                            }
                        }
                        else {
                            utility.DisplayMessages('Problem already added', 2);
                            $(txtBoxCtrl).val('');
                        }
                    }
                }
            }


            else if ((response.IMOCount == undefined || response.IMOCount == 'undefined') && LocalOrIMO == "imo" && ParentControl == "Batch_FaxSend") {
                if (lexiCode && lexiCode.length > 0) {
                    if (txtBoxCtrlParentId) {

                        var ICD9CodePlusICD9Title = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                        var ICD10CodePlusICD10Title = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                        var SNOMEDCodePlusSNOMEDTitle = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);

                        var icd9Code = ICD9CodePlusICD9Title.split("+")[0];
                        var icd9Description = ICD9CodePlusICD9Title.split("+")[1];
                        var icd10Code = ICD10CodePlusICD10Title.split("+")[0];
                        var icd10Description = ICD10CodePlusICD10Title.split("+")[1];
                        var snomedCode = SNOMEDCodePlusSNOMEDTitle.split("+")[0];
                        var snomedDescription = SNOMEDCodePlusSNOMEDTitle.split("+")[1];
                        if (snomedDescription.indexOf('^') >= 0) {
                            snomedDescription = snomedDescription.split('^')[0];
                        }

                        var selectedProblemTool = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend').find('#' + txtBoxCtrlParentId);
                        if (selectedProblemTool) {
                            var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');
                            var currId = -1;
                            $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                                currId = $(this).attr("id");
                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                            var IsAlreadyExist = false;

                            $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                                if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == null) {
                                    if ($(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                        }

                        if (!IsAlreadyExist) {

                            var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                            if (problemsExist == 0) {
                                var problemsDiv = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend .problemsListDiv').clone().removeClass('problemsListDiv');
                                $(problemsDiv).find("ul#customFormProblemsList").append(li);
                                $(problemsDiv).css('display', 'block');
                                $(selectedProblemTool).append($(problemsDiv));
                                $(selectedProblemTool).css('height', 'auto');
                            }
                            else {
                                $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                                $(selectedProblemTool).children().last().css('display', 'block');
                            }

                            $(txtBoxCtrl).val('');
                            if ($(txtBoxCtrl).attr('style')) {
                                $(txtBoxCtrl).removeAttr('style');
                            }
                        }
                        else {
                            utility.DisplayMessages('Problem already added', 2);
                            $(txtBoxCtrl).val('');
                        }
                    }
                }
            }

                //--------------------kr ends.
            else if ((response.IMOCount == undefined || response.IMOCount == 'undefined') && LocalOrIMO == "imo" && ParentControl == "CCM_Patient_Hub") {
                if (typeof lexiCode != 'undefined' && lexiCode != null && lexiCode.length > 0) {
                    var ICD9CodePlusICD9Title = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                    var ICD10CodePlusICD10Title = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                    var SNOMEDCodePlusSNOMEDTitle = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);

                    var icd9Code = ICD9CodePlusICD9Title.split("+")[0];
                    var icd9Description = ICD9CodePlusICD9Title.split("+")[1];
                    var icd10Code = ICD10CodePlusICD10Title.split("+")[0];
                    var icd10Description = ICD10CodePlusICD10Title.split("+")[1];
                    var snomedCode = SNOMEDCodePlusSNOMEDTitle.split("+")[0];
                    var snomedDescription = SNOMEDCodePlusSNOMEDTitle.split("+")[1];
                    if (snomedDescription.indexOf('^') >= 0) {
                        snomedDescription = snomedDescription.split('^')[0];
                    }

                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val('');
                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #hfIMOProblem").val($("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val());

                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").prop("disabled", true);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #ddlSeverity").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #dpStartDate").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #dpEndDate").prop("disabled", false);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtComments").prop("disabled", false);

                    CCM_Patient_Hub.icdsValues["ICD9"] = icd9Code;
                    CCM_Patient_Hub.icdsValues["ICD10"] = icd10Code;
                    CCM_Patient_Hub.icdsValues["ICD9_Description"] = icd9Description;
                    CCM_Patient_Hub.icdsValues["ICD10_Description"] = icd10Description;
                    CCM_Patient_Hub.icdsValues["SNOMEDID"] = snomedCode;
                    CCM_Patient_Hub.icdsValues["SNOMED_DESCRIPTION"] = snomedDescription;
                    $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val(icd10Description);
                    $("#" + CCM_Patient_Hub.params.PanelID + " #hfIMOProblem").val(icd10Description);

                }
            }
            else if ((response.IMOCount == undefined || response.IMOCount == 'undefined') && LocalOrIMO == "imo" && ParentControl == "OrderSet_Problems") {
                if (typeof lexiCode != 'undefined' && lexiCode != null && lexiCode.length > 0) {
                    var ICD9CodePlusICD9Title = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                    var ICD10CodePlusICD10Title = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                    var SNOMEDCodePlusSNOMEDTitle = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);

                    var icd9Code = ICD9CodePlusICD9Title.split("+")[0];
                    var icd9Description = ICD9CodePlusICD9Title.split("+")[1];
                    var icd10Code = ICD10CodePlusICD10Title.split("+")[0];
                    var icd10Description = ICD10CodePlusICD10Title.split("+")[1];
                    var snomedCode = SNOMEDCodePlusSNOMEDTitle.split("+")[0];
                    var snomedDescription = SNOMEDCodePlusSNOMEDTitle.split("+")[1];
                    if (snomedDescription.indexOf('^') >= 0) {
                        snomedDescription = snomedDescription.split('^')[0];
                    }

                    $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val('');
                    $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                    $("#" + OrderSet_Problems.params.PanelID + " #hfIMOProblem").val($("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val());

                    $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").prop("disabled", true);
                    $("#" + OrderSet_Problems.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #ddlSeverity").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #dpStartDate").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #dpEndDate").prop("disabled", false);
                    $("#" + OrderSet_Problems.params.PanelID + " #txtComments").prop("disabled", false);

                    OrderSet_Problems.icdsValues["ICD9"] = icd9Code;
                    OrderSet_Problems.icdsValues["ICD10"] = icd10Code;
                    OrderSet_Problems.icdsValues["ICD9_Description"] = icd9Description;
                    OrderSet_Problems.icdsValues["ICD10_Description"] = icd10Description;
                    OrderSet_Problems.icdsValues["SNOMEDID"] = snomedCode;
                    OrderSet_Problems.icdsValues["SNOMED_DESCRIPTION"] = snomedDescription;
                    $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val(icd10Description);
                    $("#" + OrderSet_Problems.params.PanelID + " #hfIMOProblem").val(icd10Description);

                }
            }


                //Start//03-03-2016//Ahmad Raza//CDS Detail Allergy Search
            else if (response.IMOCount != "undefined" && ParentControl == "ClinicalCDSDetail") {



                if (text.indexOf('txtCDSAllergies') < 0 && text.indexOf('txtCDSMedications') < 0 && text.indexOf('txtCDSLabResults') < 0 && ParentControl == "ClinicalCDSDetail") {

                    var snomedCode = "", snomedDescription = "", icd10Code = "", icd10Description = "";
                    var ICD10 = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                    icd10Code = ICD10.split("+")[0];
                    icd10Description = ICD10.split("+")[1];

                    var SNOMED = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);
                    snomedCode = SNOMED.split("+")[0];
                    snomedDescription = SNOMED.split("+")[1];
                    if (snomedDescription.indexOf('^') >= 0) {
                        snomedDescription = snomedDescription.split('^')[0];
                    }

                    var icd9Code = "", icd9Description = "";
                    var ICD9 = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                    icd9Code = ICD9.split("+")[0];
                    icd9Description = ICD9.split("+")[1];
                    var currId = -1;
                    $("#ClinicalCDSDetail #frmClinicalCDSDetail ul#ulCDSProblemList li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });
                    currId = parseInt(currId) + (-1);


                    // Start//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                    //if ($('#ClinicalCDSDetail #ulCDSProblemList li:first').length > 0) {
                    //    var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlProblemList" + currId + "' name = 'CDSProblemList" + snomedDescription + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + icd9Description + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                    //}
                    //else {
                    var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + icd9Description + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                    //}
                    // End//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                    var IsAlreadyExist = false;
                    $('#ClinicalCDSDetail #ulCDSProblemList li').each(function () {
                        if ($(this).attr('name') == snomedDescription) {

                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#ClinicalCDSDetail #ulCDSProblemList').append(li);
                        $('.modal-backdrop').removeClass('in');
                        $('.modal-backdrop').addClass('out');
                        $('.modal-backdrop').hide();
                        $('#ClinicalCDSDetail #txtCDSProblemList').val('');
                        Clinical_Complaints.AddInArray(currId, icd10Description, true);

                    }
                    else {
                        utility.DisplayMessages('Problem List already added', 2);

                        $('#ClinicalCDSDetail #txtCDSProblemList').val('');
                    }
                }




            }
                //End//03-03-2016//Ahmad Raza//CDS Detail Allergy Search



                //Start//10-02-2017//Ahmad Imran//OrderSet Detail Problem Search
            else if (response.IMOCount != "undefined" && ParentControl == "Clinical_OrderSetDetails") {





                var snomedCode = "", snomedDescription = "", icd10Code = "", icd10Description = "";
                var ICD10 = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                icd10Code = ICD10.split("+")[0];
                icd10Description = ICD10.split("+")[1];

                var SNOMED = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);
                snomedCode = SNOMED.split("+")[0];
                snomedDescription = SNOMED.split("+")[1];
                if (snomedDescription.indexOf('^') >= 0) {
                    snomedDescription = snomedDescription.split('^')[0];
                }

                var icd9Code = "", icd9Description = "";
                var ICD9 = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                icd9Code = ICD9.split("+")[0];
                icd9Description = ICD9.split("+")[1];
                var currId = -1;
                $("#pnlClinicalOrderSetDetails #frmOrderSetDetails ul#ulOrderSetProblemList li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });
                currId = parseInt(currId) + (-1);



                var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-12 col-lg-12'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + icd10Description + "<span class='removeIconListHover' onclick='Clinical_OrderSetDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";

                // End//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                var IsAlreadyExist = false;
                $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList li').each(function () {
                    if ($(this).attr('data') == snomedDescription) {
                        if ($(this).attr('icd10Code') == icd10Code) {
                            IsAlreadyExist = true;
                        }
                    }
                    else {
                        if ($(this).attr('icd10Code') == icd10Code) {
                            IsAlreadyExist = true;
                        }
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');
                    Clinical_Complaints.AddInArray(currId, icd10Description, true);

                }
                else {
                    utility.DisplayMessages('Problem List already added', 2);

                    $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');
                }
            }
           
            
            
                //End//10-02-2017//Ahmad Imran//OrderSet Detail Problem Search
            else {

                if (LocalOrIMO == "imo") {
                    //  if (response.IMOCount == undefined && ParentControl == "Clinical_Complaints") {
                    if (response.IMOCount == undefined && (ParentControl == "Clinical_MedicalHx" || ParentControl == "Clinical_FamilyHx" || ParentControl == "Clinical_HospitalizationHx" || ParentControl == "Favorite_FamilyHistoryDetail" || ParentControl == "Favorite_HospitalizationHistoryDetail" || ParentControl == "Favorite_MedicalHistoryDetail" || ParentControl == "Clinical_Complaints" || ParentControl == "Favorite_Complaints_Detail" || ParentControl == "Favorite_ProblemsDetail" || ParentControl == "Admin_IMOICD" || ParentControl == "Clinical_Cognitive" || ParentControl == "BillingInformation" || ParentControl == "Clinical_CarePlan")) {

                        var icd9Code = "", icd9Description = "", icd10Code = "", icd10Description = "", snomedCode = "", snomedDescription = "";
                        //if (IMODetail.params.ICDCodeAndDescription != undefined) {
                        var ICD10 = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                        icd10Code = ICD10.split("+")[0].trim();
                        icd10Description = ICD10.split("+")[1];

                        var SNOMED = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);
                        snomedCode = SNOMED.split("+")[0].trim();
                        snomedDescription = SNOMED.split("+")[1];
                        if (snomedDescription.indexOf('^') >= 0) {
                            snomedDescription = snomedDescription.split('^')[0];
                        }
                        //}
                        var ICD9 = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                        icd9Code = ICD9.split("+")[0].trim();
                        icd9Description = ICD9.split("+")[1];

                        if (text.indexOf("Favorite_MedicalHistoryDetail") != -1) {

                            var currId = -1;
                            $("#pnlFavoriteMedicalHistoryDetail #frmFavoriteMedicalHistoryDetail ul#ulFavMedicalHistoryDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_MedicalHistoryDetail.showIcon(this);' onmouseout='Favorite_MedicalHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_MedicalHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                            var IsAlreadyExist = false;
                            $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease').append(li);
                                $(li).trigger('click');
                                var item = {};
                                item["ICD9"] = icd9Code;
                                item["ICD10"] = icd10Code;
                                item["ICD10Description"] = icd10Description;
                                item["ICD9Description"] = icd9Description;
                                item["SNOMEDDescription"] = snomedDescription;
                                item["SNOMED"] = snomedCode;
                                Favorite_MedicalHistoryDetail.CPTData.push(item);

                                $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis').val('');


                                var isUnload = "false";
                                var txt = $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#pnlFavoriteMedicalHistoryDetail #txtDiagnose').val('');
                            }
                            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                        }



                        if (text.indexOf("Favorite_HospitalizationHistoryDetail") != -1) {

                            var currId = -1;
                            $("#pnlFavoriteHospitalizationHistoryDetail #frmFavoriteHospitalizationHistoryDetail ul#ulFavHospitalizationHistoryDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_HospitalizationHistoryDetail.showIcon(this);' onmouseout='Favorite_HospitalizationHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_HospitalizationHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                            var IsAlreadyExist = false;
                            $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease').append(li);
                                $(li).trigger('click');
                                var item = {};
                                item["ICD9"] = icd9Code;
                                item["ICD10"] = icd10Code;
                                item["ICD10Description"] = icd10Description;
                                item["ICD9Description"] = icd9Description;
                                item["SNOMEDDescription"] = snomedDescription;
                                item["SNOMED"] = snomedCode;
                                Favorite_HospitalizationHistoryDetail.CPTData.push(item);

                                $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis').val('');


                                var isUnload = "false";
                                var txt = $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnose').val('');
                            }
                            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                        }

                        if (text.indexOf("EncounterChargeCapture") > -1) {
                            if (SupperBillDetail.ContainerCtrl != "undefined") {
                                var ContainerCtrl = SupperBillDetail.ContainerCtrl;
                                var RefHiddenCtrlArray = [];
                                RefHiddenCtrlArray = ContainerCtrl.split(",");
                                if (RefHiddenCtrlArray.length > 1) {

                                    var ctrl = RefHiddenCtrlArray[0].substring(5);
                                    $("#txtICD" + ctrl).val(icd10Code);
                                    $(" #" + RefHiddenCtrlArray[0]).val(icd9Code);
                                    $(" #" + RefHiddenCtrlArray[1]).val(icd9Description);
                                    $(" #" + RefHiddenCtrlArray[2]).val(icd10Code);
                                    $(" #" + RefHiddenCtrlArray[3]).val(icd10Description);
                                    $(" #" + RefHiddenCtrlArray[4]).val(snomedCode);
                                    $(" #" + RefHiddenCtrlArray[5]).val(snomedDescription);
                                    $("#txtICD" + ctrl).parent("div").attr("data-original-title", icd10Description);
                                }
                            }
                            else {
                                $("#SupperBillDetail #txtICD_0").val(icd9Code);
                                $("#SupperBillDetail #txtICDDescription_0").val(icd9Description);
                                $("#SupperBillDetail #txtICD10_0").val(icd10Code);
                                $("#SupperBillDetail #hfICD10Description_0").val(icd10Description);
                                $("#SupperBillDetail #hfSNOMED_0").val(snomedCode);
                                $("#SupperBillDetail #hfSNOMEDDescription_0").val(snomedDescription);
                                $("#SupperBillDetail #hfLexiCode_0").val(snomedCode);
                            }

                            if (text.indexOf("EncounterChargeCapture") >= 0)
                                EncounterChargeCapture.BindICDNValues($("#pnlEncounterChargeCapture"));

                            Admin_IMOICD.UnLoadTab();
                        }

                        if (text.indexOf("Favorite_FamilyHistoryDetail") > -1) {

                            var currId = -1;
                            $("#pnlFavoriteFamilyHistoryDetail #frmFavoriteFamilyHistoryDetail ul#ulFavFamilyHistoryDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_FamilyHistoryDetail.showIcon(this);' onmouseout='Favorite_FamilyHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_FamilyHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                            var IsAlreadyExist = false;
                            $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease').append(li);
                                $(li).trigger('click');
                                var item = {};
                                item["ICD9"] = icd9Code;
                                item["ICD10"] = icd10Code;
                                item["ICD10Description"] = icd10Description;
                                item["ICD9Description"] = icd9Description;
                                item["SNOMEDDescription"] = snomedDescription;
                                item["SNOMED"] = snomedCode;
                                Favorite_FamilyHistoryDetail.CPTData.push(item);

                                $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');

                                var isUnload = "false";
                                var txt = $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#pnlFavoriteFamilyHistoryDetail #txtDiagnose').val('');
                            }
                            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                        }

                        if (text.indexOf("Favorite_ProblemsDetail") > -1) {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {


                                var currId = -1;
                                $("#pnlFavoriteProblemsDetail #frmFavoriteProblemsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                                    currId = $(this).attr("id");

                                });
                                currId = parseInt(currId) + (-1);
                                var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_ProblemsDetail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                                var IsAlreadyExist = false;
                                $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease li').each(function () {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                        IsAlreadyExist = true;
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease').append(li);
                                    $('.modal-backdrop').removeClass('in');
                                    $('.modal-backdrop').addClass('out');
                                    $('.modal-backdrop').hide();
                                    $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                                    Favorite_ProblemsDetail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                                    var isUnload = "false";
                                    var txt = $('#pnlFavoriteProblemsDetail #txtDiagnosis');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Diagnosis already added', 2);

                                    $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                                }
                            }

                        }

                        if (text.indexOf("Clinical_FamilyHx") > -1) {

                            var currId = -1;
                            var AccessPanelId = "pnlClinicalFamilyHx";
                            if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                AccessPanelId = AccessPanel;
                            }
                            $("#" + AccessPanelId + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            //Start 04-11-2016 Humaira Yousaf to change onclick event
                            //Start 19-10-2017 Humaira Yousaf MU3-5
                            var info = icd10Code + " - (" + snomedCode + ") - \n" + icd10Description;
                            var li = "<li title = \"" + info + "\"  id=" + currId + " onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\" MemberDetailId=\"" + currId + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";
                            //Start 19-10-2017 Humaira Yousaf MU3-5
                            //End 04-11-2016 Humaira Yousaf to change onclick event

                            var IsAlreadyExist = false;
                            $('#' + AccessPanelId + ' #ulFamilyHxDisease li').each(function () {
                                if ($(this).attr('icd9Desc') == null) {
                                    if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                } else {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#' + AccessPanelId + ' #ulFamilyHxDisease').append(li);
                                //Start 04-11-2016 Humaira Yousaf
                                $(li).trigger('click');
                                //Clinical_FamilyHx.fillFamilyHxDisease($(li));
                                //End 04-11-2016 Humaira Yousaf
                                $('#' + AccessPanelId + ' #txtDisease').val('');


                                var isUnload = "false";
                                var txt = $('#' + AccessPanelId + ' #txtDisease');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#' + AccessPanelId + ' #txtDisease').val('');
                            }
                        }

                        if (text.indexOf("Clinical_MedicalHx") > -1) {
                            var currId = -1;
                            $("#pnlClinicalMedicalHx #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li  id=" + currId + " onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                            var IsAlreadyExist = false;


                            $('#pnlClinicalMedicalHx #ulMedicalDisease li').each(function () {
                                if ($(this).attr('icd9Code') == null) {
                                    if ($(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });


                            if (!IsAlreadyExist) {
                                $('#pnlClinicalMedicalHx #ulMedicalDisease').append(li);
                                $(li).trigger('click');
                                $('#pnlClinicalMedicalHx #txtDisease').val('');

                                var isUnload = "false";
                                var txt = $('#pnlClinicalMedicalHx #txtDisease');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {

                                utility.DisplayMessages('Disease already added', 2);
                                $('#pnlClinicalMedicalHx #txtDisease').val('');
                            }
                        }


                        if (text.indexOf("Favorite_Complaints_Detail") > -1) {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {


                                var currId = -1;
                                $("#pnlFavoriteComplaintsDetail #frmFavoriteComplaintsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                                    currId = $(this).attr("id");

                                });
                                currId = parseInt(currId) + (-1);
                                var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                                var IsAlreadyExist = false;
                                $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease li').each(function () {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                        IsAlreadyExist = true;
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease').append(li);
                                    $('.modal-backdrop').removeClass('in');
                                    $('.modal-backdrop').addClass('out');
                                    $('.modal-backdrop').hide();
                                    $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                                    Favorite_Complaints_Detail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                                    var isUnload = "false";
                                    var txt = $('#pnlFavoriteComplaintsDetail #txtDiagnosis');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }

                                }
                                else {
                                    utility.DisplayMessages('Diagnosis already added', 2);

                                    $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                                }
                            }

                        }

                        if (text.indexOf("Clinical_HospitalizationHx") > -1) {


                            var currId = -1;
                            //Start 11/02/2016 Abid Ali, for bug# 311
                            $("#pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx #HospitalizationHxDisease ul#ulHospitalizationDisease li[id*='-']").each(function (i, item) {
                                //End 11/02/2016 Abid Ali, for bug# 311
                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li  id=" + currId + " onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
                            var IsAlreadyExist = false;
                            $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease li').each(function () {
                                if ($(this).attr('icd9Code') == null) {
                                    if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease').append(li);
                                $(li).trigger('click');
                                $('#pnlClinicalHospitalizationHx #txtDisease').val('');

                                var isUnload = "false";
                                var txt = $('#pnlClinicalHospitalizationHx #txtDisease');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Disease already added', 2);

                                $('#pnlClinicalHospitalizationHx #txtDisease').val('');
                            }

                        }


                        if (text.indexOf("Clinical_Complaints") > -1) {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {
                                var currId = -1;
                                $("#pnlClinicalComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                                    currId = $(this).attr("id");

                                });
                                currId = parseInt(currId) + (-1);
                                var li = "<li  id=" + currId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Complaints.deleteChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>";
                                var IsAlreadyExist = false;
                                $('#pnlClinicalComplaints #ulCompliantDisease li').each(function () {
                                    if (
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description
                                       ) {

                                        IsAlreadyExist = true;
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                                    $('.modal-backdrop').removeClass('in');
                                    $('.modal-backdrop').addClass('out');
                                    $('.modal-backdrop').hide();
                                    $('#pnlClinicalComplaints #txtComplaints').val('');
                                    Clinical_Complaints.AddInArray(currId, icd10Description, true);//for record of complaints

                                    var isUnload = "false";
                                    var txt = $('#pnlClinicalComplaints #txtComplaints');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                    Clinical_Complaints.IsUpdate = true;
                                }
                                else {
                                    utility.DisplayMessages('Disease already added', 2);

                                    $('#pnlClinicalComplaints #txtComplaints').val('');
                                }
                            }
                        }

                        if (text.indexOf("Clinical_Cognitive") > -1) {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {
                                if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtCognitiveStatus') {
                                    var currId = -1;
                                    $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulCognitiveStatus li[id*='-']").each(function (i, item) {

                                        currId = $(this).attr("id");

                                    });
                                    currId = parseInt(currId) + (-1);

                                    var status = "Status";
                                    var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + status + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + status + "\");'><i class='fa fa-close'></i></span></a></li>"


                                    var IsAlreadyExist = false;
                                    $('#' + Clinical_Cognitive.params.PanelID + ' #ulCognitiveStatus li').each(function () {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                            IsAlreadyExist = true;
                                        }
                                    });

                                    if (!IsAlreadyExist) {
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #ulCognitiveStatus').append(li);
                                        $(li).trigger('click');
                                        $('.modal-backdrop').removeClass('in');
                                        $('.modal-backdrop').addClass('out');
                                        $('.modal-backdrop').hide();
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').val('');
                                        //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                                        var isUnload = "false";
                                        var txt = $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus');
                                        if (txt.is('[data-popupunload]')) {
                                            isUnload = txt.attr('data-popupunload');
                                        }

                                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                            txt.attr("data-popupunload", "false");
                                            Admin_IMOICD.UnLoadTab();
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages('Cognitive Status already added', 2);

                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').val('');
                                    }
                                }

                                else if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtFunctionalStatus') {
                                    var currId = -1;
                                    $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulFunctionalStatus li[id*='-']").each(function (i, item) {

                                        currId = $(this).attr("id");

                                    });
                                    currId = parseInt(currId) + (-1);
                                    var functionalStatus = "FunctionalStatus";
                                    var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + functionalStatus + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_Cognitive.activeInActiveFunctionalStatus($(this), event);'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + functionalStatus + "\");'><i class='fa fa-close'></i></span></a></li>"


                                    var IsAlreadyExist = false;
                                    $('#' + Clinical_Cognitive.params.PanelID + ' #ulFunctionalStatus li').each(function () {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                            IsAlreadyExist = true;
                                        }
                                    });

                                    if (!IsAlreadyExist) {
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #ulFunctionalStatus').append(li);
                                        $(li).trigger('click');
                                        $('.modal-backdrop').removeClass('in');
                                        $('.modal-backdrop').addClass('out');
                                        $('.modal-backdrop').hide();
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').val('');
                                        //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                                        var isUnload = "false";
                                        var txt = $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus');
                                        if (txt.is('[data-popupunload]')) {
                                            isUnload = txt.attr('data-popupunload');
                                        }

                                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                            txt.attr("data-popupunload", "false");
                                            Admin_IMOICD.UnLoadTab();
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages('Functional Status already added', 2);

                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').val('');
                                    }
                                }
                            }


                        }


                        if (text.indexOf("Clinical_CarePlan") > -1) {
                            if (txtBoxCtrlParentId == 'Goals') {
                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanGoals ul#ulCarePlanGoals li[id*='-']").each(function (i, item) {
                                    currId = $(this).attr("id");
                                });

                                currId = parseInt(currId) + (-1);

                                var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillGoalDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\" MemberDetailId=\"" + currId + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.GoalDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>";

                                var IsAlreadyExist = false;
                                $('#' + AccessPanelId + ' #ulCarePlanGoals li').each(function () {
                                    if ($(this).attr('icd9Desc') == null) {
                                        if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                            IsAlreadyExist = true;
                                        }
                                    } else {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#' + AccessPanelId + ' #ulCarePlanGoals').append(li);
                                    if ($('#' + AccessPanelId + ' #sectionGoals').hasClass('disableAll') == true) {
                                        $('#' + AccessPanelId + ' #sectionGoals').removeClass('disableAll');
                                    }
                                    $(li).trigger('click');                               
                                    $('#' + AccessPanelId + ' #txtDisease').val('');
                                    $('#' + AccessPanelId + ' #Goals #DivICDAutoComplete').addClass('disableAll');
                                    $('#' + AccessPanelId + ' #txtDisease').attr('readonly', true);

                                    var isUnload = "false";
                                    var txt = $('#' + AccessPanelId + ' #txtDisease');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Diagnose already added', 2);

                                    $('#' + AccessPanelId + ' #txtDisease').val('');
                                }
                            }
                            else if (txtBoxCtrlParentId == "Concern" || txtBoxCtrlParentId == "Observation" || txtBoxCtrlParentId == "Risk") {

                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                var containerId = "";
                                if (txtBoxCtrlParentId == "Concern") {
                                    containerId = AccessPanelId + ' #txtConcern';
                                    $('#' + containerId).attr('concernId', currId);
                                    $('#' + AccessPanelId + ' #concernInfoDiv').removeClass('disableAll');
                                }
                                else if (txtBoxCtrlParentId == "Observation") {
                                    containerId = AccessPanelId + ' #txtObservation';
                                    $('#' + containerId).attr('obsevationId', currId);
                                    $('#' + AccessPanelId + ' #observationInfoDiv').removeClass('disableAll');
                                }
                                else if (txtBoxCtrlParentId == "Risk") {
                                    containerId = AccessPanelId + ' #txtRisk';
                                    $('#' + containerId).attr('riskId', currId);
                                    $('#' + AccessPanelId + ' #riskInfoDiv').removeClass('disableAll');
                                }
                                $('#' + containerId).val(icd10Description);

                                $('#' + containerId).attr('icd9Code', icd9Code);
                                $('#' + containerId).attr('icd9Description', icd9Description);
                                $('#' + containerId).attr('icd10Code', icd10Code);
                                $('#' + containerId).attr('icd10Description', icd10Description);
                                $('#' + containerId).attr('snomedCode', snomedCode);
                                $('#' + containerId).attr('snomedDescription', snomedDescription);

                                var isUnload = "false";
                                var txt = $('#' + containerId);
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else if (txtBoxCtrlParentId == 'Intervention') {
                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanInterventions ul#ulCarePlanInterventions li[id*='-']").each(function (i, item) {
                                    currId = $(this).attr("id");
                                });

                                currId = parseInt(currId) + (-1);

                                var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillInterventionDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.InterventionDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>";

                                var IsAlreadyExist = false;
                                $('#' + AccessPanelId + ' #ulCarePlanInterventions li').each(function () {
                                    if ($(this).attr('icd9Desc') == null) {
                                        if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                            IsAlreadyExist = true;
                                        }
                                    } else {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#' + AccessPanelId + ' #ulCarePlanInterventions').append(li);
                                    if ($('#' + AccessPanelId + ' #sectionInterventions').hasClass('disableAll') == true) {
                                        $('#' + AccessPanelId + ' #sectionInterventions').removeClass('disableAll');
                                    }
                                    $(li).trigger('click');                               
                                    $('#' + AccessPanelId + ' #txtInterventions').val('');


                                    var isUnload = "false";
                                    var txt = $('#' + AccessPanelId + ' #txtInterventions');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Intervention already added', 2);
                                    $('#' + AccessPanelId + ' #txtInterventions').val('');
                                }
                            }
                            else if (txtBoxCtrlParentId == 'Outcome') {
                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanOutcomes ul#ulCarePlanOutcomes li[id*='-']").each(function (i, item) {
                                    currId = $(this).attr("id");
                                });

                                currId = parseInt(currId) + (-1);

                                var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillOutcomeDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.OutcomeDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>";

                                var IsAlreadyExist = false;
                                $('#' + AccessPanelId + ' #ulCarePlanOutcomes li').each(function () {
                                    if ($(this).attr('icd9Desc') == null) {
                                        if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                            IsAlreadyExist = true;
                                        }
                                    } else {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#' + AccessPanelId + ' #ulCarePlanOutcomes').append(li);
                                    if ($('#' + AccessPanelId + ' #sectionOutcomes').hasClass('disableAll') == true) {
                                        $('#' + AccessPanelId + ' #sectionOutcomes').removeClass('disableAll');
                                    }
                                    $(li).trigger('click');
                                    $('#' + AccessPanelId + ' #txtOutcomes').val('');


                                    var isUnload = "false";
                                    var txt = $('#' + AccessPanelId + ' #txtOutcomes');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Outcome already added', 2);
                                    $('#' + AccessPanelId + ' #txtOutcomes').val('');
                                }
                            }
                        }








                        if (text.indexOf("Clinical_ProblemLists") > -1) {
                            Clinical_ProblemLists.ResetDiagnosis();
                            //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
                            $("#pnlClinicalProblemLists #ddlChronicityLevel").prop("disabled", false);
                            $("#pnlClinicalProblemLists #ddlSeverity").prop("disabled", false);
                            $("#pnlClinicalProblemLists #dpStartDate").prop("disabled", false);
                            $("#pnlClinicalProblemLists #dpEndDate").prop("disabled", false);
                            $("#pnlClinicalProblemLists #txtComments").prop("disabled", false);

                            if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                                $('#pnlClinicalProblemLists #ddlSeverity').val('Unspecified Severity');
                            } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                                $('#pnlClinicalProblemLists #ddlSeverity').val('Severe Persistent');
                            } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                                $('#pnlClinicalProblemLists #ddlSeverity').val('Moderate Persistent');
                            } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                                $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Persistent');
                            } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                                $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Intermittent');
                            } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                                $('#pnlClinicalProblemLists #ddlSeverity').val(6);
                            }

                            //Start || 11 April, 2016 || ZeeshanAK || Fix for showing SNOMED ID with ICD9 and 10 under Problem List screen
                            //$("#pnlClinicalProblemLists #txtDiagnosis").val(icd9Code + ' - ' + icd10Code + ' - ' + icd10Description);
                            $("#pnlClinicalProblemLists #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                            //End   || 11 April, 2016 || ZeeshanAK || Fix for showing SNOMED ID with ICD9 and 10 under Problem List screen

                            $("#pnlClinicalProblemLists #txtProblems").val(icd10Description);
                            // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
                            $("#pnlClinicalProblemLists #hfIMOProblem").val(icd10Description);
                            // End 19/01/2016 Muhammad Irfan for bug # EMR-219
                            var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
                            $('#pnlClinicalProblemLists #ulProblemDisease').html(li);
                            $("#pnlClinicalProblemLists #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');//kr

                            // Check if unload from Problem List main screen
                            var isUnload = "false";
                            var txtProblemInPnlClinicalProblemLists = $('#pnlClinicalProblemLists #txtProblems');
                            if (txtProblemInPnlClinicalProblemLists.is('[data-popupunload]')) {
                                isUnload = txtProblemInPnlClinicalProblemLists.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txtProblemInPnlClinicalProblemLists.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }


                        else if (text.indexOf("BillingInformation") > -1) {// Start Farooq Ahmad Adding ICD search on Billing Information
                            var ControlsArray = [];
                            if (Admin_IMOICD != null && Admin_IMOICD.params != null && Admin_IMOICD.params.RefHiddenCtrl != null) {
                                ControlsArray = Admin_IMOICD.params.RefHiddenCtrl.split(',');
                            }
                            else if (BillingInformation != null && BillingInformation.params != null && BillingInformation.params.RefHiddenCtrl != null) {
                                ControlsArray = BillingInformation.params.RefHiddenCtrl.split(',');
                            }

                            if (ControlsArray.length > 0) {

                                for (index = 0; index < ControlsArray.length; index++) {

                                    if (ControlsArray[index].indexOf("hfICDCode9") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Code)

                                    if (ControlsArray[index].indexOf("hfICDDescription9") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Description);

                                    if (ControlsArray[index].indexOf("hfICDCode10") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code);

                                    if (ControlsArray[index].indexOf("hfICDDescription10") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Description);

                                    if (ControlsArray[index].indexOf("hfSNOMEDCode") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedCode);

                                    if (ControlsArray[index].indexOf("hfSNOMEDDescription") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedDescription);

                                    if (ControlsArray[index].indexOf("txtDisease_") >= 0) {
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code + ' - ' + icd10Description);
                                    }

                                    if (icd10Code == '' && icd9Code == '') {
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val('')
                                    }
                                }
                            }
                            if (Admin_IMOICD.params['fromIcon'] != null && Admin_IMOICD.params['fromIcon'] == "true") {
                                Admin_IMOICD.params['fromIcon'] = "false";
                                Admin_IMOICD.UnLoadTab();
                                BillingInformation.EnableDisableICDs();
                            }
                            //Admin_IMOICD.UnLoadTab();
                        }// End Farooq Ahmad Adding ICD search on Billing Information

                        else if (text.indexOf("Clinical_CustomFormsPreview") != -1) {
                            if (txtBoxCtrlParentId) {
                                var selectedProblemTool = $("#pnlClinicalCustomFormsPreview #frmCustomFormPreview").find('#' + txtBoxCtrlParentId);
                                if (selectedProblemTool) {

                                    var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');
                                    var currId = -1;
                                    $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                                        currId = $(this).attr("id");
                                    });

                                    currId = parseInt(currId) + (-1);

                                    var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                                    var IsAlreadyExist = false;

                                    $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                                        if ($(this).attr('icd9Code') == null) {
                                            if (!$(this).hasClass('hidden') && $(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                                IsAlreadyExist = true;
                                            }
                                        }
                                        else {
                                            if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                                IsAlreadyExist = true;
                                            }
                                        }
                                    });

                                    if (!IsAlreadyExist) {

                                        var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                                        if (problemsExist == 0) {
                                            var problemsDiv = $('#pnlClinicalCustomFormsPreview #frmCustomFormPreview .problemsListDiv').clone().removeClass('problemsListDiv');
                                            $(problemsDiv).find("ul#customFormProblemsList").append(li);
                                            $(problemsDiv).css('display', 'block');
                                            $(selectedProblemTool).append($(problemsDiv));
                                            $(selectedProblemTool).css('height', 'auto');
                                        }
                                        else {
                                            $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                                            $(selectedProblemTool).children().last().css('display', 'block');
                                        }

                                        Clinical_CustomFormsPreview.saveProblem(li, txtBoxCtrlParentId);

                                        $(txtBoxCtrl).val('');
                                        if ($(txtBoxCtrl).attr('style')) {
                                            $(txtBoxCtrl).removeAttr('style');
                                        }

                                        var isUnload = "false";
                                        if ($(txtBoxCtrl).is('[data-popupunload]')) {
                                            isUnload = $(txtBoxCtrl).attr('data-popupunload');
                                        }

                                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                            $(txtBoxCtrl).attr("data-popupunload", "false");
                                            Admin_IMOICD.UnLoadTab();
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages('Problem already added', 2);
                                        $(txtBoxCtrl).val('');
                                    }
                                }
                            }
                        }

                        else if (text.indexOf("Batch_FaxSend") != -1) {
                            if (txtBoxCtrlParentId) {
                                var selectedProblemTool = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend').find('#' + txtBoxCtrlParentId);
                                if (selectedProblemTool) {

                                    var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');
                                    var currId = -1;
                                    $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                                        currId = $(this).attr("id");
                                    });

                                    currId = parseInt(currId) + (-1);

                                    var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                                    var IsAlreadyExist = false;

                                    $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                                        if ($(this).attr('icd9Code') == null) {
                                            if (!$(this).hasClass('hidden') && $(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                                IsAlreadyExist = true;
                                            }
                                        }
                                        else {
                                            if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                                IsAlreadyExist = true;
                                            }
                                        }
                                    });

                                    if (!IsAlreadyExist) {

                                        var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                                        if (problemsExist == 0) {
                                            var problemsDiv = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend .problemsListDiv').clone().removeClass('problemsListDiv');
                                            $(problemsDiv).find("ul#customFormProblemsList").append(li);
                                            $(problemsDiv).css('display', 'block');
                                            $(selectedProblemTool).append($(problemsDiv));
                                            $(selectedProblemTool).css('height', 'auto');
                                        }
                                        else {
                                            $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                                            $(selectedProblemTool).children().last().css('display', 'block');
                                        }

                                        $(txtBoxCtrl).val('');
                                        if ($(txtBoxCtrl).attr('style')) {
                                            $(txtBoxCtrl).removeAttr('style');
                                        }

                                        var isUnload = "false";
                                        if ($(txtBoxCtrl).is('[data-popupunload]')) {
                                            isUnload = $(txtBoxCtrl).attr('data-popupunload');
                                        }

                                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                            $(txtBoxCtrl).attr("data-popupunload", "false");
                                            Admin_IMOICD.UnLoadTab();
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages('Problem already added', 2);
                                        $(txtBoxCtrl).val('');
                                    }
                                }
                            }
                        }

                        if (ParentControl == "Clinical_MedicalHx") {
                            var currId = -1;
                            $("#pnlClinicalMedicalHx #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li  id=" + currId + " onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd9Description + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                            var IsAlreadyExist = false;


                            $('#pnlClinicalMedicalHx #ulMedicalDisease li').each(function () {
                                if ($(this).attr('icd9Code') == null) {
                                    if ($(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });


                            if (!IsAlreadyExist) {
                                $('#pnlClinicalMedicalHx #ulMedicalDisease').append(li);
                                $(li).trigger('click');
                                $('#pnlClinicalMedicalHx #txtDisease').val('');

                                if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                                    var disease = $(li).get(0).outerHTML;
                                    var diseaseDetails = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").clone();
                                    $(diseaseDetails).resetAllControls(null);
                                    var diseaseData = $(diseaseDetails).getMyJSONByName();
                                    Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData, disease);
                                }

                                var isUnload = "false";
                                var txt = $('#pnlClinicalMedicalHx #txtDisease');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {

                                utility.DisplayMessages('Disease already added', 2);
                                $('#pnlClinicalMedicalHx #txtDisease').val('');
                            }
                        }


                        else if (ParentControl == "BillingInformation") {// Start Farooq Ahmad Adding ICD search on Billing Information
                            var ControlsArray = [];
                            if (Admin_IMOICD != null && Admin_IMOICD.params != null && Admin_IMOICD.params.RefHiddenCtrl != null) {
                                ControlsArray = Admin_IMOICD.params.RefHiddenCtrl.split(',');
                            }
                            else if (BillingInformation != null && BillingInformation.params != null && BillingInformation.params.RefHiddenCtrl != null) {
                                ControlsArray = BillingInformation.params.RefHiddenCtrl.split(',');
                            }

                            if (ControlsArray.length > 0) {

                                for (index = 0; index < ControlsArray.length; index++) {

                                    if (ControlsArray[index].indexOf("hfICDCode9") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Code)

                                    if (ControlsArray[index].indexOf("hfICDDescription9") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Description);

                                    if (ControlsArray[index].indexOf("hfICDCode10") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code);

                                    if (ControlsArray[index].indexOf("hfICDDescription10") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Description);

                                    if (ControlsArray[index].indexOf("hfSNOMEDCode") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedCode);

                                    if (ControlsArray[index].indexOf("hfSNOMEDDescription") >= 0)
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedDescription);

                                    if (ControlsArray[index].indexOf("txtDisease_") >= 0) {
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code + ' - ' + icd10Description);
                                    }

                                    if (icd10Code == '' && icd9Code == '') {
                                        $('#pnlBillingInformation  #' + ControlsArray[index]).val('')
                                    }
                                }
                            }
                            if (Admin_IMOICD.params['fromIcon'] != null && Admin_IMOICD.params['fromIcon'] == "true") {
                                Admin_IMOICD.params['fromIcon'] = "false";
                                Admin_IMOICD.UnLoadTab();
                                BillingInformation.EnableDisableICDs();
                            }
                            //Admin_IMOICD.UnLoadTab();
                        }// End Farooq Ahmad Adding ICD search on Billing Information

                        else if (ParentControl == "Clinical_Cognitive") {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {
                                if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtCognitiveStatus') {
                                    var currId = -1;
                                    $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulCognitiveStatus li[id*='-']").each(function (i, item) {

                                        currId = $(this).attr("id");

                                    });
                                    currId = parseInt(currId) + (-1);

                                    var status = "Status";
                                    var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + status + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + status + "\");'><i class='fa fa-close'></i></span></a></li>"


                                    var IsAlreadyExist = false;
                                    $('#' + Clinical_Cognitive.params.PanelID + ' #ulCognitiveStatus li').each(function () {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                            IsAlreadyExist = true;
                                        }
                                    });

                                    if (!IsAlreadyExist) {
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #ulCognitiveStatus').append(li);
                                        $(li).trigger('click');
                                        $('.modal-backdrop').removeClass('in');
                                        $('.modal-backdrop').addClass('out');
                                        $('.modal-backdrop').hide();
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').val('');
                                        //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                                        var isUnload = "false";
                                        var txt = $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus');
                                        if (txt.is('[data-popupunload]')) {
                                            isUnload = txt.attr('data-popupunload');
                                        }

                                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                            txt.attr("data-popupunload", "false");
                                            Admin_IMOICD.UnLoadTab();
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages('Cognitive Status already added', 2);

                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').val('');
                                    }
                                }

                                else if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtFunctionalStatus') {
                                    var currId = -1;
                                    $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulFunctionalStatus li[id*='-']").each(function (i, item) {

                                        currId = $(this).attr("id");

                                    });
                                    currId = parseInt(currId) + (-1);
                                    var functionalStatus = "FunctionalStatus";
                                    var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + functionalStatus + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_Cognitive.activeInActiveFunctionalStatus($(this), event);'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + functionalStatus + "\");'><i class='fa fa-close'></i></span></a></li>"


                                    var IsAlreadyExist = false;
                                    $('#' + Clinical_Cognitive.params.PanelID + ' #ulFunctionalStatus li').each(function () {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                            IsAlreadyExist = true;
                                        }
                                    });

                                    if (!IsAlreadyExist) {
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #ulFunctionalStatus').append(li);
                                        $(li).trigger('click');
                                        $('.modal-backdrop').removeClass('in');
                                        $('.modal-backdrop').addClass('out');
                                        $('.modal-backdrop').hide();
                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').val('');
                                        //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                                        var isUnload = "false";
                                        var txt = $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus');
                                        if (txt.is('[data-popupunload]')) {
                                            isUnload = txt.attr('data-popupunload');
                                        }

                                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                            txt.attr("data-popupunload", "false");
                                            Admin_IMOICD.UnLoadTab();
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages('Functional Status already added', 2);

                                        $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').val('');
                                    }
                                }
                            }


                        }









                        else if (ParentControl == "Clinical_FamilyHx") {

                            var currId = -1;
                            var AccessPanelId = "pnlClinicalFamilyHx";
                            if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                AccessPanelId = AccessPanel;
                            }
                            $("#" + AccessPanelId + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            //Start 04-11-2016 Humaira Yousaf to change onclick event
                            //Start 19-10-2017 Humaira Yousaf MU3-5
                            var info = icd10Code + " - (" + snomedCode + ") - \n" + icd10Description;
                            var li = "<li title = \"" + info + "\"  id=" + currId + " onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\" MemberDetailId=\"" + currId + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";
                            //End 19-10-2017 Humaira Yousaf MU3-5
                            //End 04-11-2016 Humaira Yousaf to change onclick event

                            var IsAlreadyExist = false;
                            $('#' + AccessPanelId + ' #ulFamilyHxDisease li').each(function () {
                                if ($(this).attr('icd9Desc') == null) {
                                    if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                } else {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#' + AccessPanelId + ' #ulFamilyHxDisease').append(li);
                                //Start 04-11-2016 Humaira Yousaf
                                $(li).trigger('click');
                                //Clinical_FamilyHx.fillFamilyHxDisease($(li));
                                //End 04-11-2016 Humaira Yousaf
                                $('#' + AccessPanelId + ' #txtDisease').val('');

                                if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var diseaseId = currId;
                                    var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                                    var disease = $(li).get(0).outerHTML;
                                    var detailsdata = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').clone();
                                    $(detailsdata).resetAllControls(null);
                                    var diseaseData = $(detailsdata).getMyJSONByName();
                                    Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, diseaseData, disease);
                                    $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyMember li.active").find("a").css("color", "green");

                                }

                                var isUnload = "false";
                                var txt = $('#' + AccessPanelId + ' #txtDisease');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#' + AccessPanelId + ' #txtDisease').val('');
                            }
                        }
                        else if (ParentControl == "Clinical_HospitalizationHx") {


                            var currId = -1;
                            //Start 11/02/2016 Abid Ali, for bug# 311
                            $("#pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx #HospitalizationHxDisease ul#ulHospitalizationDisease li[id*='-']").each(function (i, item) {
                                //End 11/02/2016 Abid Ali, for bug# 311
                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li  id=" + currId + " onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
                            var IsAlreadyExist = false;
                            $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease li').each(function () {
                                if ($(this).attr('icd9Code') == null) {
                                    if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                        IsAlreadyExist = true;
                                    }
                                }
                                else {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                        IsAlreadyExist = true;
                                    }
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease').append(li);
                                $(li).trigger('click');
                                $('#pnlClinicalHospitalizationHx #txtDisease').val('');

                                if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                                    var disease = $(li).get(0).outerHTML;
                                    var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                                    Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData, disease);
                                }

                                var isUnload = "false";
                                var txt = $('#pnlClinicalHospitalizationHx #txtDisease');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Disease already added', 2);

                                $('#pnlClinicalHospitalizationHx #txtDisease').val('');
                            }

                        }
                        else if (ParentControl == "Favorite_FamilyHistoryDetail") {

                            var currId = -1;
                            $("#pnlFavoriteFamilyHistoryDetail #frmFavoriteFamilyHistoryDetail ul#ulFavFamilyHistoryDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_FamilyHistoryDetail.showIcon(this);' onmouseout='Favorite_FamilyHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_FamilyHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                            var IsAlreadyExist = false;
                            $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease').append(li);
                                $(li).trigger('click');
                                var item = {};
                                item["ICD9"] = icd9Code;
                                item["ICD10"] = icd10Code;
                                item["ICD10Description"] = icd10Description;
                                item["ICD9Description"] = icd9Description;
                                item["SNOMEDDescription"] = snomedDescription;
                                item["SNOMED"] = snomedCode;
                                Favorite_FamilyHistoryDetail.CPTData.push(item);

                                $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');

                                var isUnload = "false";
                                var txt = $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#pnlFavoriteFamilyHistoryDetail #txtDiagnose').val('');
                            }
                            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                        }

                        else if (ParentControl == "Favorite_MedicalHistoryDetail") {

                            var currId = -1;
                            $("#pnlFavoriteMedicalHistoryDetail #frmFavoriteMedicalHistoryDetail ul#ulFavMedicalHistoryDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_MedicalHistoryDetail.showIcon(this);' onmouseout='Favorite_MedicalHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_MedicalHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                            var IsAlreadyExist = false;
                            $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease').append(li);
                                $(li).trigger('click');
                                var item = {};
                                item["ICD9"] = icd9Code;
                                item["ICD10"] = icd10Code;
                                item["ICD10Description"] = icd10Description;
                                item["ICD9Description"] = icd9Description;
                                item["SNOMEDDescription"] = snomedDescription;
                                item["SNOMED"] = snomedCode;
                                Favorite_MedicalHistoryDetail.CPTData.push(item);

                                $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis').val('');


                                var isUnload = "false";
                                var txt = $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#pnlFavoriteMedicalHistoryDetail #txtDiagnose').val('');
                            }
                            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                        }













                        else if (ParentControl == "Favorite_HospitalizationHistoryDetail") {

                            var currId = -1;
                            $("#pnlFavoriteHospitalizationHistoryDetail #frmFavoriteHospitalizationHistoryDetail ul#ulFavHospitalizationHistoryDisease li[id*='-']").each(function (i, item) {

                                currId = $(this).attr("id");

                            });

                            currId = parseInt(currId) + (-1);

                            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_HospitalizationHistoryDetail.showIcon(this);' onmouseout='Favorite_HospitalizationHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_HospitalizationHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                            var IsAlreadyExist = false;
                            $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease li').each(function () {
                                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                                    IsAlreadyExist = true;
                                }
                            });

                            if (!IsAlreadyExist) {
                                $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease').append(li);
                                $(li).trigger('click');
                                var item = {};
                                item["ICD9"] = icd9Code;
                                item["ICD10"] = icd10Code;
                                item["ICD10Description"] = icd10Description;
                                item["ICD9Description"] = icd9Description;
                                item["SNOMEDDescription"] = snomedDescription;
                                item["SNOMED"] = snomedCode;
                                Favorite_HospitalizationHistoryDetail.CPTData.push(item);

                                $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis').val('');


                                var isUnload = "false";
                                var txt = $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis');
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else {
                                utility.DisplayMessages('Diagnose already added', 2);

                                $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnose').val('');
                            }
                            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
                        }
































                        else if (ParentControl == "Clinical_Complaints") {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {
                                var currId = -1;
                                $("#pnlClinicalComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                                    currId = $(this).attr("id");

                                });
                                currId = parseInt(currId) + (-1);
                                var li = "<li  id=" + currId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\">" +
                                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "</span></a>" +
                                        "<span class='removeIconListHover'>" +
                                            "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                                "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                                "</span>" +
                                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                                    "<div class='clearfix'></div>" +
                                "</li>";
                                var IsAlreadyExist = false;
                                $('#pnlClinicalComplaints #ulCompliantDisease li').each(function () {
                                    if ($(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description) {

                                        IsAlreadyExist = true;
                                    } else {
                                        if ($(this).text().trim() == (icd10Code != '' ? icd10Code + " - " : "") + icd10Description.trim()) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                                    $('.modal-backdrop').removeClass('in');
                                    $('.modal-backdrop').addClass('out');
                                    $('.modal-backdrop').hide();
                                    $('#pnlClinicalComplaints #txtComplaints').val('');
                                    Clinical_Complaints.AddInArray(currId, icd10Description, true);//for record of complaints

                                    var isUnload = "false";
                                    var txt = $('#pnlClinicalComplaints #txtComplaints');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                    Clinical_Complaints.IsUpdate = true;
                                }
                                else {
                                    utility.DisplayMessages('Disease already added', 2);

                                    $('#pnlClinicalComplaints #txtComplaints').val('');
                                }
                            }
                        }

                        else if (ParentControl == "Clinical_HPIComplaints") {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {
                                var currId = -1;
                                $("#pnlClinicalHPIComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                                    currId = $(this).attr("id");

                                });
                                currId = parseInt(currId) + (-1);
                                var li = "<li  id=" + currId + " onclick='Clinical_HPIComplaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\">" +
                                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "</span></a>" +
                                        "<span class='removeIconListHover'>" +
                                            "<a href='#' onclick='Clinical_HPIComplaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                                "<a href='#' onclick='Clinical_HPIComplaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                                "</span>" +
                                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_HPIComplaints.updateComplaintsText(this, event);'/>" +
                                    "<div class='clearfix'></div>" +
                                "</li>";
                                var IsAlreadyExist = false;
                                $('#pnlClinicalHPIComplaints #ulCompliantDisease li').each(function () {
                                    if ($(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description) {

                                        IsAlreadyExist = true;
                                    } else {
                                        if ($(this).text().trim() == (icd10Code != '' ? icd10Code + " - " : "") + icd10Description.trim()) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#pnlClinicalHPIComplaints #ulCompliantDisease').append(li);
                                    $('.modal-backdrop').removeClass('in');
                                    $('.modal-backdrop').addClass('out');
                                    $('.modal-backdrop').hide();
                                    $('#pnlClinicalHPIComplaints #txtComplaints').val('');
                                    Clinical_HPIComplaints.AddInArray(currId, icd10Description, true);//for record of complaints

                                    var isUnload = "false";
                                    var txt = $('#pnlClinicalHPIComplaints #txtComplaints');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                    Clinical_HPIComplaints.IsUpdate = true;
                                }
                                else {
                                    utility.DisplayMessages('Disease already added', 2);

                                    $('#pnlClinicalHPIComplaints #txtComplaints').val('');
                                }
                            }
                        }

                        else if (ParentControl == "Favorite_Complaints_Detail") {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {


                                var currId = -1;
                                $("#pnlFavoriteComplaintsDetail #frmFavoriteComplaintsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                                    currId = $(this).attr("id");

                                });
                                currId = parseInt(currId) + (-1);
                                var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                                var IsAlreadyExist = false;
                                $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease li').each(function () {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                        IsAlreadyExist = true;
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease').append(li);
                                    $('.modal-backdrop').removeClass('in');
                                    $('.modal-backdrop').addClass('out');
                                    $('.modal-backdrop').hide();
                                    $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                                    Favorite_Complaints_Detail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                                    var isUnload = "false";
                                    var txt = $('#pnlFavoriteComplaintsDetail #txtDiagnosis');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }

                                }
                                else {
                                    utility.DisplayMessages('Diagnosis already added', 2);

                                    $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                                }
                            }

                        }

                        else if (ParentControl == "Favorite_ProblemsDetail") {
                            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                            }
                            else {


                                var currId = -1;
                                $("#pnlFavoriteProblemsDetail #frmFavoriteProblemsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                                    currId = $(this).attr("id");

                                });
                                currId = parseInt(currId) + (-1);
                                var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_ProblemsDetail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                                var IsAlreadyExist = false;
                                $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease li').each(function () {
                                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                        IsAlreadyExist = true;
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease').append(li);
                                    $('.modal-backdrop').removeClass('in');
                                    $('.modal-backdrop').addClass('out');
                                    $('.modal-backdrop').hide();
                                    $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                                    Favorite_ProblemsDetail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                                    var isUnload = "false";
                                    var txt = $('#pnlFavoriteProblemsDetail #txtDiagnosis');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Diagnosis already added', 2);

                                    $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                                }
                            }

                        }
                        else if (ParentControl == "Clinical_CarePlan") {
                            if (txtBoxCtrlParentId == 'Goals') {
                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanGoals ul#ulCarePlanGoals li[id*='-']").each(function (i, item) {
                                    currId = $(this).attr("id");
                                });

                                currId = parseInt(currId) + (-1);

                                var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillGoalDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\" MemberDetailId=\"" + currId + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.GoalDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>";

                                var IsAlreadyExist = false;
                                $('#' + AccessPanelId + ' #ulCarePlanGoals li').each(function () {
                                    if ($(this).attr('icd9Desc') == null) {
                                        if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                            IsAlreadyExist = true;
                                        }
                                    } else {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#' + AccessPanelId + ' #ulCarePlanGoals').append(li);
                                    if ($('#' + AccessPanelId + ' #sectionGoals').hasClass('disableAll') == true) {
                                        $('#' + AccessPanelId + ' #sectionGoals').removeClass('disableAll');
                                    }
                                    $(li).trigger('click');

                                    $('#' + AccessPanelId + ' #txtDisease').val('');                                    
                                    $('#' + AccessPanelId + ' #Goals #DivICDAutoComplete').addClass('disableAll');
                                    $('#' + AccessPanelId + ' #txtDisease').attr('readonly', true);

                                    var isUnload = "false";
                                    var txt = $('#' + AccessPanelId + ' #txtDisease');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Diagnose already added', 2);

                                    $('#' + AccessPanelId + ' #txtDisease').val('');
                                }
                            }
                            else if (txtBoxCtrlParentId == "Concern" || txtBoxCtrlParentId == "Observation" || txtBoxCtrlParentId == "Risk") {

                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                var containerId = "";
                                if (txtBoxCtrlParentId == "Concern") {
                                    containerId = AccessPanelId + ' #txtConcern';
                                    $('#' + containerId).attr('concernId', currId);
                                    $('#' + AccessPanelId + ' #concernInfoDiv').removeClass('disableAll');
                                }
                                else if (txtBoxCtrlParentId == "Observation") {
                                    containerId = AccessPanelId + ' #txtObservation';
                                    $('#' + containerId).attr('obsevationId', currId);
                                    $('#' + AccessPanelId + ' #observationInfoDiv').removeClass('disableAll');
                                }
                                else if (txtBoxCtrlParentId == "Risk") {
                                    containerId = AccessPanelId + ' #txtRisk';
                                    $('#' + containerId).attr('riskId', currId);
                                    $('#' + AccessPanelId + ' #riskInfoDiv').removeClass('disableAll');
                                }
                                $('#' + containerId).val(icd10Description);

                                $('#' + containerId).attr('icd9Code', icd9Code);
                                $('#' + containerId).attr('icd9Description', icd9Description);
                                $('#' + containerId).attr('icd10Code', icd10Code);
                                $('#' + containerId).attr('icd10Description', icd10Description);
                                $('#' + containerId).attr('snomedCode', snomedCode);
                                $('#' + containerId).attr('snomedDescription', snomedDescription);

                                var isUnload = "false";
                                var txt = $('#' + containerId);
                                if (txt.is('[data-popupunload]')) {
                                    isUnload = txt.attr('data-popupunload');
                                }

                                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                    txt.attr("data-popupunload", "false");
                                    Admin_IMOICD.UnLoadTab();
                                }
                            }
                            else if (txtBoxCtrlParentId == 'Intervention') {
                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanInterventions ul#ulCarePlanInterventions li[id*='-']").each(function (i, item) {
                                    currId = $(this).attr("id");
                                });

                                currId = parseInt(currId) + (-1);

                                var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillInterventionDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.InterventionDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>";

                                var IsAlreadyExist = false;
                                $('#' + AccessPanelId + ' #ulCarePlanInterventions li').each(function () {
                                    if ($(this).attr('icd9Desc') == null) {
                                        if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                            IsAlreadyExist = true;
                                        }
                                    } else {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#' + AccessPanelId + ' #ulCarePlanInterventions').append(li);
                                    if ($('#' + AccessPanelId + ' #sectionInterventions').hasClass('disableAll') == true) {
                                        $('#' + AccessPanelId + ' #sectionInterventions').removeClass('disableAll');
                                    }
                                    $(li).trigger('click');

                                    $('#' + AccessPanelId + ' #txtInterventions').val('');

                                    var isUnload = "false";
                                    var txt = $('#' + AccessPanelId + ' #txtInterventions');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Intervention already added', 2);
                                    $('#' + AccessPanelId + ' #txtInterventions').val('');
                                }
                            }
                            else if (txtBoxCtrlParentId == 'Outcome') {
                                var currId = -1;
                                var AccessPanelId = "pnlClinicalCarePlan";
                                if (AccessPanel != null && typeof AccessPanel != "undefined") {
                                    AccessPanelId = AccessPanel;
                                }
                                $("#" + AccessPanelId + " #frmClinicalCarePlan #CarePlanOutcomes ul#ulCarePlanOutcomes li[id*='-']").each(function (i, item) {
                                    currId = $(this).attr("id");
                                });

                                currId = parseInt(currId) + (-1);

                                var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillOutcomeDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CarePlan.OutcomeDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>";

                                var IsAlreadyExist = false;
                                $('#' + AccessPanelId + ' #ulCarePlanOutcomes li').each(function () {
                                    if ($(this).attr('icd9Desc') == null) {
                                        if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                                            IsAlreadyExist = true;
                                        }
                                    } else {
                                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                                            IsAlreadyExist = true;
                                        }
                                    }
                                });

                                if (!IsAlreadyExist) {
                                    $('#' + AccessPanelId + ' #ulCarePlanOutcomes').append(li);
                                    if ($('#' + AccessPanelId + ' #sectionOutcomes').hasClass('disableAll') == true) {
                                        $('#' + AccessPanelId + ' #sectionOutcomes').removeClass('disableAll');
                                    }
                                    $(li).trigger('click');

                                    $('#' + AccessPanelId + ' #txtOutcomes').val('');

                                    var isUnload = "false";
                                    var txt = $('#' + AccessPanelId + ' #txtOutcomes');
                                    if (txt.is('[data-popupunload]')) {
                                        isUnload = txt.attr('data-popupunload');
                                    }

                                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                        txt.attr("data-popupunload", "false");
                                        Admin_IMOICD.UnLoadTab();
                                    }
                                }
                                else {
                                    utility.DisplayMessages('Outcome already added', 2);
                                    $('#' + AccessPanelId + ' #txtOutcomes').val('');
                                }
                            }
                        }
                    }
                        // }
                    else {
                        var a = LocalOrIMO;
                        var parentCtrl = "";
                        if (SupperBillDetail.params && SupperBillDetail.params.PanelID && SupperBillDetail.params.PanelID.indexOf("#") >= 0)
                            parentCtrl = SupperBillDetail.params.PanelID.split('#')[1].trim();



                        if (SupperBillDetail.params.TabID && SupperBillDetail.params.TabID.indexOf("EncounterChargeCapture") < 0) {
                            ParentControl = ParentControl.substring(0, ParentControl.indexOf("EncounterChargeCapture") + 22)
                        }

                        var params = [];
                        if (typeof (lexiCode) == "string") {

                            params["lexiCodeId"] = lexiCode.split("*")[0];
                            params["ICD9CodePlusICD9Title"] = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                            params["ICD10CodePlusICD10Title"] = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                            params["SNOMEDCodePlusSNOMEDTitle"] = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);

                        } else if (typeof (lexiCode) == "object") {

                            lexiCode = lexiCode.type;
                            params["lexiCodeId"] = lexiCode.split("*")[0];
                            params["ICD9CodePlusICD9Title"] = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                            params["ICD10CodePlusICD10Title"] = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                            params["SNOMEDCodePlusSNOMEDTitle"] = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);

                        }
                        params["ICDCodeAndDescription"] = lexiCode;

                        if (ParentControl == undefined) {
                            ParentControl = parentCtrl;
                        }
                        //else if (ParentControl == "EncounterChargeCaptureundefined") {
                        //    ParentControl = "EncounterChargeCapture";
                        //}

                        params["mode"] = "Add";
                        params["ParentCtrl"] = ParentControl;//parentCtrl;
                        params["ContainerCtrl"] = SupperBillDetail.ContainerCtrl;
                        params["Ctrltext"] = text;
                        if (ParentControl == "Clinical_CustomFormsPreview" || ParentControl == "Batch_FaxSend") {
                            params["ContainerTextBoxParentId"] = txtBoxCtrlParentId;
                        }
                        LoadActionPan('IMODetail', params);
                    }


                } else {
                    var snomedCode = "", snomedDescription = "", icd10Code = "", icd10Description = "";
                    //if (IMODetail.params.ICDCodeAndDescription != undefined) {
                    var ICD10 = lexiCode.substring(lexiCode.lastIndexOf("$") + 1, lexiCode.lastIndexOf("~"));
                    icd10Code = ICD10.split("+")[0];
                    icd10Description = ICD10.split("+")[1];

                    var SNOMED = lexiCode.substring(lexiCode.lastIndexOf("~") + 1);
                    snomedCode = SNOMED.split("+")[0];
                    snomedDescription = SNOMED.split("+")[1];
                    if (snomedDescription.indexOf('^') >= 0) {
                        snomedDescription = snomedDescription.split('^')[0];
                    }
                    //}

                    var icd9Code = "", icd9Description = "";
                    var ICD9 = lexiCode.substring(lexiCode.lastIndexOf("*") + 1, lexiCode.lastIndexOf("$"));
                    icd9Code = ICD9.split("+")[0];
                    icd9Description = ICD9.split("+")[1];

                    if (ParentControl == "ICDDetail") {

                        $("#ICDDetail #txtICD9Code").val(icd9Code);
                        $("#ICDDetail #txtICD9Description").val(icd9Description);
                        $("#ICDDetail #txtICD10Code").val(icd10Code); // Selected List Item ID i.e. [ICD10 Code]
                        $("#ICDDetail #txtICD10Description").val(icd10Description); // Selected List Item text i.e. [ICD10 description]
                        $("#ICDDetail #txtSnomedCode").val(snomedCode);
                        $("#ICDDetail #txtSnomedDescription").val(snomedDescription);
                        $("#ICDDetail #hfLexiCode").val(IMODetail.params.lexiCodeId);
                        $("#ICDDetail #divICDFields").show();


                    } else if (ParentControl.indexOf("EncounterChargeCapture") >= 0) {

                        var RefHiddenCtrlArray = [];
                        RefHiddenCtrlArray = ContainerCtrl.split(",");
                        if (RefHiddenCtrlArray.length > 1) {

                            var ctr = RefHiddenCtrlArray[0].substring(5);
                            $("#txtICD" + ctr).val(icd10Code);

                            $(" #" + RefHiddenCtrlArray[0]).val(icd9Code);
                            $(" #" + RefHiddenCtrlArray[1]).val(icd9Description);
                            $(" #" + RefHiddenCtrlArray[2]).val(icd10Code);
                            $(" #" + RefHiddenCtrlArray[3]).val(icd10Description);
                            $(" #" + RefHiddenCtrlArray[4]).val(snomedCode);
                            $(" #" + RefHiddenCtrlArray[5]).val(snomedDescription);
                            $("#txtICD" + ctrl).parent("div").attr("title", icd10Description).attr("data-original-title", icd10Description).attr("data-toggle", "tooltip").attr("data-placement", "right");
                            //Set ToolTip for Comments.
                            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                            EncounterChargeCapture.ValidateICDCodeAndSetDesc($("#txtICD" + ctr));
                            EncounterChargeCapture.BindICDNValues($("#pnlEncounterChargeCapture"));
                            UnloadActionPan(ParentControl, 'Admin_IMOICD');
                        }
                        else {

                            $("#txtICD" + ContainerCtrl).val(icd10Code);
                            $("#txtICD10Description" + ContainerCtrl).val(icd10Description);
                            $("#hfICD" + ContainerCtrl).val(icd9Code);
                            $("#hfICDDescription" + ContainerCtrl).val(icd9Code);
                            $("#hfICD10" + ContainerCtrl).val(icd10Code);
                            $("#hfICD10Description" + ContainerCtrl).val(icd10Description);
                            $("#hfSNOMED" + ContainerCtrl).val(snomedCode);
                            $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
                            EncounterChargeCapture.ValidateICDCodeAndSetDesc($("#txtICD" + ContainerCtrl));
                            EncounterChargeCapture.BindICDNValues($("#pnlEncounterChargeCapture"));
                        }
                    }
                    else if (ParentControl == "SupperBillDetail") {

                        $("#SupperBillDetail #txtICD" + ContainerCtrl).val(icd9Code);
                        $("#SupperBillDetail #hfICD" + ContainerCtrl).val(icd9Code);
                        $("#SupperBillDetail #txtICDDescription" + ContainerCtrl).val(icd9Description);
                        $("#SupperBillDetail #hfICDDescription" + ContainerCtrl).val(icd9Description);
                        $("#SupperBillDetail #txtICD10" + ContainerCtrl).val(icd10Code);
                        $("#SupperBillDetail #hfICD10" + ContainerCtrl).val(icd10Code);
                        $("#SupperBillDetail #hfICD10Description" + ContainerCtrl).val(icd10Description);
                        $("#SupperBillDetail #hfSNOMED" + ContainerCtrl).val(snomedCode);
                        $("#SupperBillDetail #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
                        $("#SupperBillDetail #hfLexiCode" + ContainerCtrl).val(lexiCode.substring(0, lexiCode.lastIndexOf("*")));
                    }
                    else if (ParentControl == "chargeSearchDetail") {

                        $("#pnlBillChargeSearch #txtICD" + ContainerCtrl).val(icd10Code);

                        $("#pnlBillChargeSearch #hfICD" + ContainerCtrl).val(icd9Code);
                        $("#pnlBillChargeSearch #hfICDDescription" + ContainerCtrl).val(icd9Description);
                        $("#pnlBillChargeSearch #hfICD10" + ContainerCtrl).val(icd10Code);
                        $("#pnlBillChargeSearch #hfICD10Description" + ContainerCtrl).val(icd10Description);
                        $("#pnlBillChargeSearch #hfSNOMED" + ContainerCtrl).val(snomedCode);
                        $("#pnlBillChargeSearch #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);

                        setTimeout(function () { $("#pnlBillChargeSearch #txtICD" + ContainerCtrl).val(icd9Code); $("#hfICD" + ContainerCtrl).val(icd9Code); $("#hfICDDescription" + ContainerCtrl).val(icd9Description); $("#hfICD10" + ContainerCtrl).val(icd10Code); $("#hfICD10Description" + ContainerCtrl).val(icd10Description); $("#hfSNOMED" + ContainerCtrl).val(snomedCode); $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription); }, 110);
                    } else if (ParentControl == "Clinical_ProblemLists") {
                        $("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
                        $("#pnlClinicalProblemLists #ddlChronicityLevel").prop("disabled", false);
                        $("#pnlClinicalProblemLists #ddlSeverity").prop("disabled", false);
                        $("#pnlClinicalProblemLists #dpStartDate").prop("disabled", false);
                        $("#pnlClinicalProblemLists #dpEndDate").prop("disabled", false);
                        $("#pnlClinicalProblemLists #txtComments").prop("disabled", false);

                        if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                            $('#pnlClinicalProblemLists #ddlSeverity').val('Unspecified Severity');
                        } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                            $('#pnlClinicalProblemLists #ddlSeverity').val('Severe Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                            $('#pnlClinicalProblemLists #ddlSeverity').val('Moderate Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                            $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                            $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Intermittent');
                        } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                            $('#pnlClinicalProblemLists #ddlSeverity').val(6);
                        }

                        $("#pnlClinicalProblemLists #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                        $("#pnlClinicalProblemLists #txtProblems").val(icd10Description);
                    } else if (ParentControl == "CCM_Patient_Hub") {
                        $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").prop("disabled", false);
                        $("#" + CCM_Patient_Hub.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
                        $("#" + CCM_Patient_Hub.params.PanelID + " #ddlSeverity").prop("disabled", false);
                        $("#" + CCM_Patient_Hub.params.PanelID + " #dpStartDate").prop("disabled", false);
                        $("#" + CCM_Patient_Hub.params.PanelID + " #dpEndDate").prop("disabled", false);
                        $("#" + CCM_Patient_Hub.params.PanelID + " #txtComments").prop("disabled", false);

                        if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                            $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Unspecified Severity');
                        } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                            $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Severe Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                            $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Moderate Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                            $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                            $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Intermittent');
                        } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                            $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val(6);
                        }

                        $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                        $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val(icd10Description);
                    }
                    else if (ParentControl == "OrderSet_Problems") {
                        $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").prop("disabled", false);
                        $("#" + OrderSet_Problems.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
                        $("#" + OrderSet_Problems.params.PanelID + " #ddlSeverity").prop("disabled", false);
                        $("#" + OrderSet_Problems.params.PanelID + " #dpStartDate").prop("disabled", false);
                        $("#" + OrderSet_Problems.params.PanelID + " #dpEndDate").prop("disabled", false);
                        $("#" + OrderSet_Problems.params.PanelID + " #txtComments").prop("disabled", false);

                        if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                            $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Unspecified Severity');
                        } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                            $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Severe Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                            $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Moderate Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                            $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Mild Persistent');
                        } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                            $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Mild Intermittent');
                        } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                            $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val(6);
                        }

                        $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                        $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val(icd10Description);
                    }
                }
            }
        });


    },

    OpenModifierDetail: function (Id) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "SupperBillDetail";
        params["RefCtrl"] = "txtModifier_" + Id;
        params["RefCtrlDescription"] = "txtModifierDescription_" + Id;
        LoadActionPan('Admin_Modifier', params);
    },

    SearchCode: function (Id) {

        if (SupperBillDetail.params["action"] == "ICD") {
            SupperBillDetail.OpenICDDetail(Id);
        }
        else if (SupperBillDetail.params["action"] == "CPT") {
            SupperBillDetail.OpenCPTDetail(Id);
        }
        else if (SupperBillDetail.params["action"] == "Modifier") {
            SupperBillDetail.OpenModifierDetail(Id);
        }


    },

}

//////-------------Data Grid Scripts Starts-----------------------

var EditableTable = {

    options: {
        table: '#table',
        addButton: '#addToTable',
        dialog: {
            wrapper: '#dialog',
            cancelButton: '#dialogCancel',
            confirmButton: '#dialogConfirm',
        }
    },

    initialize: function (addButtonId, tableId, IsFirst) {

        EditableTable.options.table = tableId;
        EditableTable.options.addButton = addButtonId;

        EditableTable.setVars();
        EditableTable.build();

        if (IsFirst)
            EditableTable.events();
    },

    setVars: function () {
        EditableTable.$table = $(EditableTable.options.table);
        EditableTable.$addButton = $(EditableTable.options.addButton);

        // dialog
        EditableTable.dialog = {};
        EditableTable.dialog.$wrapper = $(EditableTable.options.dialog.wrapper);
        EditableTable.dialog.$cancel = $(EditableTable.options.dialog.cancelButton);
        EditableTable.dialog.$confirm = $(EditableTable.options.dialog.confirmButton);

        return EditableTable;
    },

    build: function () {

        if (SupperBillDetail.params["action"] == "ICD") {
            EditableTable.datatable = EditableTable.$table.DataTable({
                "columns": [
                    { "bSortable": false },
                    null,
                    null,
                ],
                "bScrollCollapse": true, "bPaginate": false, "bInfo": false,
            });
        }
        else {
            EditableTable.datatable = EditableTable.$table.DataTable({
                "columns": [
                    { "bSortable": false },
                    null,
                    null,
                ],
                "bScrollCollapse": true, "bPaginate": false, "bInfo": false,
            });
        }
        window.dt = EditableTable.datatable;

        return EditableTable;
    },

    events: function () {
        var _self = EditableTable;

        EditableTable.$table
            .on('click', 'a.save-row', function (e) {
                e.preventDefault();

                _self.rowSave($(this).closest('tr'));
            })
            .on('click', 'a.cancel-row', function (e) {
                e.preventDefault();
                _self.rowCancel($(this).closest('tr'));
            })
            .on('click', 'a.up-row', function (e) {
                e.preventDefault();
                _self.rowUp($(this).closest('tr'));
            })
            .on('click', 'a.down-row', function (e) {
                e.preventDefault();
                _self.rowDown($(this).closest('tr'));
            })
            .on('click', 'a.title-row', function (e) {
                e.preventDefault();
                _self.rowTitle($(this).closest('tr'));
            })
            .on('click', 'a.edit-row', function (e) {
                e.preventDefault();
                _self.rowEdit($(this).closest('tr'), 'edit');
            })
            .on('click', 'a.remove-row', function (e) {
                e.preventDefault();

                var $row = $(this).closest('tr');

                _self.rowRemove($row);

                //$.magnificPopup.open({
                //	items: {
                //		src: '#dialog',
                //		type: 'inline'
                //	},
                //	preloader: false,
                //	modal: true,
                //	callbacks: {
                //		change: function() {
                //			_self.dialog.$confirm.on( 'click', function( e ) {
                //				e.preventDefault();

                //				_self.rowRemove( $row );
                //				$.magnificPopup.close();
                //			});
                //		},
                //		close: function() {
                //			_self.dialog.$confirm.off( 'click' );
                //		}
                //	}
                //});
            });

        EditableTable.dialog.$cancel.on('click', function (e) {
            e.preventDefault();
            $.magnificPopup.close();
        });

        return this;
    },



    // ==========================================================================================
    // ROW FUNCTIONS
    // ==========================================================================================
    rowAdd: function () {

        EditableTable.$addButton.css({ 'display': 'none' });

        var actions,
            data,
            $row;

        actions = [
            '<a href="#" class="btn-xs hidden on-editing cancel-row" title="Cancel Record"><i class="fa fa-close black"></i></a>',
            '<a href="#" class="btn-xs hidden on-editing save-row" title="Save Record" ><i class="fa fa-save green"></i></a>',
            '<a href="#" class="btn-xs hidden on-editing title-row" title="Add Title" ><i class="fa fa-paste black"></i></a>',
            '<a href="#" class="btn-xs on-default remove-row" title="Delete Record" ><i class="fa fa-close red"></i></a>',
            '<a href="#" class="btn-xs on-default edit-row" title="Edit Record" ><i class="fa fa-edit black"></i></a>',
            '<a href="#" class="btn-xs on-default up-row" title="Up Record" ><i class="fa fa-arrow-up black"></i></a>',
            '<a href="#" class="btn-xs on-default down-row" title="Down Record" ><i class="fa fa-arrow-down black"></i></a>',
        ].join(' ');


        if (SupperBillDetail.params["action"] == "ICD") {
            data = EditableTable.datatable.row.add([actions, '', '']);
        }
        else
            data = EditableTable.datatable.row.add([actions, '', '']);
        $row = EditableTable.datatable.row(data[0]).nodes().to$();
        $row
            .addClass('adding')
            .find('td:first')
            .addClass('actions');

        $row
            .find('td:nth-child(2)')
            .addClass('code');



        //var index = $(EditableTable.options.table).find('tbody tr.active').index();
        //if (index == -1)
        //    index = 0;

        //$(EditableTable.options.table).find('tbody tr').eq(index).after('<tr class="adding even" role="row">'+$row.html()+'</tr>');

        $row.attr("Id", "0");
        EditableTable.rowEdit($row);
        EditableTable.datatable.order([0, 'asc']).draw(); // always show fields

        var index = $(EditableTable.options.table).find('tbody tr.active').index();

        if (index == -1) {
            var obj = $row.prev();
            EditableTable.setParms($row, obj);
        }
        else {
            var obj = $(EditableTable.options.table).find('tbody tr').eq(index);
            EditableTable.setParms($row, obj);

            //move new row to its selected position.
            EditableTable.moveEditableRow($row);

        }

        //add Title Row as first row.
        if ($(EditableTable.options.table).find('tbody tr').length == 1) {
            $(EditableTable.options.table).find('tbody tr :first .title-row').click();
            $(EditableTable.options.table).find('tbody tr :first .title-row').addClass("hidden");
        }
    },

    moveEditableRow: function ($row) {

        var index = $(EditableTable.options.table).find('tbody tr.active').index();

        //hide title action if index >=0 and  to add only code under title.
        if ($row.index() - index != 1)
            $row.find(':first .title-row').addClass("hidden");

        if (index != -1) {
            var diffrence = $row.index() - index;
            for (var i = 0; i < diffrence - 1; i++) {

                var pre_row = $row.prev('tr').get(0);
                //move new row to its selected position.
                EditableTable.rowSwap($row.get(0), pre_row);
            }
        }
    },

    setParms: function ($row, obj) {

        var titleId = "";
        if (obj.hasClass("r_title")) {
            titleId = obj.attr("Id");
        }
        else {
            titleId = obj.attr("titleid");
            $row.attr("sortId", obj.attr("sortId"));
        }

        $row.attr('titleid', titleId);
    },

    rowCancel: function ($row) {
        var _self = this,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            EditableTable.$addButton.css('display', 'block');
            EditableTable.datatable.row($row.get(0)).remove().draw();
        } else {

            data = EditableTable.datatable.row($row.get(0)).data();
            EditableTable.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                EditableTable.rowSetActionsDefault($row);
            }

            //EditableTable.datatable.draw();
        }
    },

    rowEdit: function ($row, key) {

        var _self = this,
            data;
        data = this.datatable.row($row.get(0)).data();

        //var RowId = $($row).attr("id");
        //if (!RowId) {
        //    RowId = "0";//0 means it's new row
        //    $($row).attr("id", "0");
        //}


        $row.children('td').each(function (i) {

            //var $this_ = $(_self.options.table + ' tr th:nth-child(' + (i + 1) + ')');
            //var controlId = $this_.attr("controlid");
            //var CtrlId = controlId + k + "" + RowId;

            var $this = $(this);
            var id = $this.parent().attr("Id");

            if ($this.hasClass('actions')) {
                _self.rowSetActionsEditing($row);
            }
            else if ($this.hasClass('code')) {

                $this.html(_self.addAutoCompleteField(data[i], id));

            }
            else {

                var id_ = "";
                var length_ = 100;
                if ($row.hasClass("r_title"))
                    length_ = 250;

                if (SupperBillDetail.params["action"] == "ICD") {

                    if (i == 2) {
                        id_ = "txtICDDescription_" + id;
                        // id_ = "txtICD10_" + id;
                    }
                    else if (i == 3) {
                        //id_ = "txtICDDescription_" + id;
                    }
                }
                else if (SupperBillDetail.params["action"] == "CPT") {
                    if (i == 2) {
                        id_ = "txtCPTDescription_" + id;
                    }
                }
                else if (SupperBillDetail.params["action"] == "Modifier") {
                    if (i == 2) {
                        id_ = "txtModifierDescription_" + id;
                    }
                }

                $this.html('<input id="' + id_ + '" type="text" required class="form-control input-block" maxlength="' + length_ + '" value="' + data[i] + '"/>');
            }
        });

        if (key === "edit")
            $row.find(':first .title-row').addClass("hidden");
    },

    addDropDownField: function (selected_value) {
        var array = ["--Select--", "1", "2", "3"];
        var ddl = document.createElement("select");
        ddl.setAttribute("class", "form-control");

        //Create and append the options
        for (var i = 0; i < array.length; i++) {
            var option = document.createElement("option");

            if (array[i] == "--Select--")
                option.value = "";
            else
                option.value = array[i];

            option.text = array[i];
            if (option.value == selected_value)
                option.selected = true;

            ddl.appendChild(option);
        }

        return ddl

    },

    addAutoCompleteField: function (value, ID) {

        var id_ = "";
        var hiddenFields = "";

        if (SupperBillDetail.params["action"] == "ICD") {
            id_ = "txtICD_" + ID;
            hiddenFields = "<input  id='hfICD_" + ID + "' type='hidden' class='form-control input-block' />";
            hiddenFields += "<input id='hfICDDescription_" + ID + "' type='hidden' class='form-control input-block' />";
            hiddenFields += "<input id='hfICD10_" + ID + "' type='hidden' class='form-control input-block' />";
            hiddenFields += "<input id='hfICD10Description_" + ID + "' type='hidden' class='form-control input-block' />";
            hiddenFields += "<input id='hfSNOMED_" + ID + "' type='hidden' class='form-control input-block' />";
            hiddenFields += "<input id='hfSNOMEDDescription_" + ID + "' type='hidden' class='form-control input-block' />";
            hiddenFields += "<input id='hfLexiCode_" + ID + "' type='hidden' class='form-control input-block' />";

        }
        else if (SupperBillDetail.params["action"] == "CPT") {
            id_ = "txtCPT_" + ID;
        }
        else if (SupperBillDetail.params["action"] == "Modifier") {
            id_ = "txtModifier_" + ID;
        }

        return autoField = '<div class="mb-tiny" ><div class="input-group">' +
            '<input class="form-control" id="' + id_ + '" placeholder="Search Code" value="' + value + '" type="text" onblur="SupperBillDetail.SupperBillValidateCode(this);" oninput="SupperBillDetail.BindSupperBillAutoComplete(this);">' +
                           '<div class="input-group-btn">' +
                              '<button class="btn btn-primary btn-xs " type="button" onclick="SupperBillDetail.SearchCode(' + ID + ');"><i class="glyphicon glyphicon-search"></i></button>' +
                           '</div></div></div>' + hiddenFields;
    },

    rowSave: function ($row) {

        if (EditableTable.rowValidate($row)) {

            var _self = EditableTable,
            $actions,
            values = [];

            values = $row.find('td').map(function () {

                var $this = $(this);

                if ($this.hasClass('actions')) {
                    return $this.html();
                }
                else if ($this.hasClass('ddl')) {
                    return $.trim($this.find('select').val());

                } else {

                    return $.trim($this.find('input').val());
                }
            });

            if ($row.hasClass('r_title')) {

                //set actions to default
                _self.rowSetActionsDefault($row);

                var title_Id = $row.attr("Id");
                var BillId = $("#SupperBillDetail #hfbillId").val();
                var Title = values[2];

                var SupperBillData = "BillID=" + BillId + "&Title=" + Title;
                if (title_Id != "" && title_Id != undefined && title_Id != "0") {

                    //Update Title.
                    AppPrivileges.GetFormPrivileges("Super Bill", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            SupperBillData += "&TitleId=" + title_Id;
                            SupperBillDetail.UpdateSupperBillTitle(SupperBillData).done(function (response) {

                                if (response.status != false) {

                                    utility.DisplayMessages(response.Message, 1);

                                    // Draw row on grid
                                    EditableTable.rowDraw($row, values);
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
                else {

                    // Save Title.
                    AppPrivileges.GetFormPrivileges("Super Bill", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            SupperBillDetail.SaveSupperBillTitle(SupperBillData).done(function (response) {


                                if (response.status != false) {

                                    utility.DisplayMessages(response.Message, 1);

                                    //seting Id.
                                    $row.attr("Id", response.TitleId);
                                    $row.attr("onclick", "utility.SelectGridRow($(this))");

                                    // Draw row on grid
                                    EditableTable.rowDraw($row, values);
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

            }
            else {

                //Save Code
                var titleId = $row.attr('titleid');

                if (titleId == "" || titleId == undefined || titleId == 'undefined') {
                    var obj = $row.prev();
                    if (obj.hasClass("r_title")) {
                        titleId = obj.attr("Id");
                    }
                    else {
                        titleId = obj.attr("titleid");
                    }
                }

                var Code = values[1];
                //SupperBillDetail.ValidateCode(Code).done(function (res) {

                //if (res.length != 0)
                //{
                //set actions to default
                _self.rowSetActionsDefault($row);

                var Description = values[2];
                var IsAlreadyExists = false;
                if (SupperBillDetail.params["action"] == "CPT") {
                    $('#pnlAdminSupperBill #tblSupper_Bill #dgvCPT').find('tr').each(function (i, itemRow) {
                        if ($(itemRow).find('td.description').text() == Description) {
                            IsAlreadyExists = true;
                        }
                    });
                } else if (SupperBillDetail.params["action"] == "ICD") {
                    $('#pnlAdminSupperBill #tblSupper_Bill #dgvICD_SB').find('tr').each(function (i, itemRow) {
                        if ($(itemRow).find('td.code').text() == values[1]) {
                            IsAlreadyExists = true;
                        }
                    });
                }
                var CodeData = "TitleId=" + titleId + "&Code=" + Code + "&Description=" + Description;
                var record_Id = $row.attr("Id");

                if (IsAlreadyExists == false) {
                    if (record_Id != "" && record_Id != undefined && record_Id != "0") {

                        if (SupperBillDetail.params["action"] == "ICD") {

                            var Icd10 = $row.children().children("#hfICD10_" + record_Id).val();
                            var Icd10Description = $row.children().children("#hfICD10Description_" + record_Id).val();
                            var SNOMEDId = $row.children().children("#hfSNOMED_" + record_Id).val();
                            var SNOMEDdescription = $row.children().children("#hfSNOMEDDescription_" + record_Id).val();
                            var LexiCode = $row.children().children("#hfLexiCode_" + record_Id).val();
                            var icd9description = $row.children().children("#txtICDDescription_" + record_Id).val();
                            var icd9 = Code;

                            CodeData = "TitleId=" + titleId + "&CodeID=" + record_Id + "&Description="
                             + icd9description + "&Code=" + icd9 + "&ICD10=" + Icd10 + "&ICD10Description=" + Icd10Description
                             + "&SnomedId=" + SNOMEDId + "&SnomedDescription=" + SNOMEDdescription + "&LexiCode=" + LexiCode;
                        }
                        else {

                            CodeData += "&CodeID=" + record_Id;
                        }
                        //Update Code
                        AppPrivileges.GetFormPrivileges("Super Bill", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                SupperBillDetail.UpdateSupperBillCode(CodeData).done(function (response) {

                                    if (response.status != false) {

                                        utility.DisplayMessages(response.Message, 1);

                                        // Draw row on grid
                                        EditableTable.rowDraw($row, values);
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
                    else {

                        if (SupperBillDetail.params["action"] == "ICD") {

                            var Icd10 = $row.children().children("#hfICD10_" + record_Id).val();
                            var Icd10Description = $row.children().children("#hfICD10Description_" + record_Id).val();
                            var SNOMEDId = $row.children().children("#hfSNOMED_" + record_Id).val();
                            var SNOMEDdescription = $row.children().children("#hfSNOMEDDescription_" + record_Id).val();
                            var LexiCode = $row.children().children("#hfLexiCode_" + record_Id).val();
                            var icd9description = $row.children().children("#txtICDDescription_" + record_Id).val();
                            var icd9 = Code;

                            CodeData = "TitleId=" + titleId + "&CodeID=" + record_Id + "&Description="
                             + icd9description + "&Code=" + icd9 + "&ICD10=" + Icd10 + "&ICD10Description=" + Icd10Description
                             + "&SnomedId=" + SNOMEDId + "&SnomedDescription=" + SNOMEDdescription + "&LexiCode=" + LexiCode;

                            var sort_Id = $row.attr("sortId");
                            if (sort_Id == "" || sort_Id == undefined || sort_Id == 'undefined')
                                sort_Id = 0;

                            CodeData += "&SortId=" + sort_Id;

                        }
                        else {

                            var sort_Id = $row.attr("sortId");
                            if (sort_Id == "" || sort_Id == undefined || sort_Id == 'undefined')
                                sort_Id = 0;

                            CodeData += "&SortId=" + sort_Id;
                        }
                        //Save Code
                        AppPrivileges.GetFormPrivileges("Super Bill", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                SupperBillDetail.SaveSupperBillCode(CodeData).done(function (response) {

                                    if (response.status != false) {

                                        utility.DisplayMessages(response.Message, 1);

                                        //seting CodeId, SortId, TitleId.
                                        $row.attr("Id", response.CodeId);
                                        $row.attr("sortId", response.SortId);
                                        $row.attr("titleid", response.TitleId);
                                        $row.attr("onclick", "utility.SelectGridRow($(this))");

                                        // Draw row on grid
                                        EditableTable.rowDraw($row, values);
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
                } else {
                    utility.DisplayMessages(SupperBillDetail.params["action"] + " already exists.", 2);
                    if ($row.hasClass('adding')) {
                        EditableTable.$addButton.css('display', 'block');
                    }
                    EditableTable.datatable.row($row.get(0)).remove().draw();
                }
                //}
                //});
            }

        }
    },

    rowDraw: function ($row, values) {
        if ($row.hasClass('adding')) {
            EditableTable.$addButton.css('display', 'block');
            $row.removeClass('adding');
        }

        EditableTable.datatable.row($row.get(0)).data(values);

        $actions = $row.find('td.actions');

        if ($actions.get(0)) {
            EditableTable.rowSetActionsDefault($row);
            EditableTable.rowHidArrows($row);
        }
        if (SupperBillDetail.params["action"] == "CPT") {
            $row.find('td:nth-child(3)').addClass('description');
        }
        EditableTable.datatable.draw();

        //move new row to its selected position.
        EditableTable.moveEditableRow($row);

        //find first tr is it have editable fields.
        //var add_ = $(EditableTable.options.table).find('tbody tr:last').hasClass('adding');
        //if (!add_)
        //    EditableTable.rowAdd();
    },

    rowRemove: function ($row) {

        utility.myConfirm('1', function () {

            AppPrivileges.GetFormPrivileges("Super Bill", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if ($row.hasClass('r_title')) {

                        var SupperBillData = "&TitleId=" + $row.attr("Id");

                        var hasChild = $row.next().attr("titleid");

                        if (hasChild != $row.attr("Id")) {
                            SupperBillDetail.DeleteTitle(SupperBillData).done(function (response) {

                                if (response.status != false) {

                                    if ($row.hasClass('adding')) {
                                        EditableTable.$addButton.css('display', 'block');
                                    }
                                    EditableTable.datatable.row($row.get(0)).remove().draw();

                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }

                            });
                        }
                        else {
                            utility.DisplayMessages("This title has some " + SupperBillDetail.params["action"] + " codes.", 3);
                        }

                    }
                    else {

                        var SupperBillData = "&CodeId=" + $row.attr("Id");

                        SupperBillDetail.DeleteCode(SupperBillData).done(function (response) {

                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                    EditableTable.$addButton.css('display', 'block');
                                }
                                EditableTable.datatable.row($row.get(0)).remove().draw();

                                utility.DisplayMessages(response.Message, 1);
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

        }, function () { }, '1');



    },

    rowSetActionsEditing: function ($row) {
        $row.find('.on-editing').removeClass('hidden');
        $row.find('.on-default').addClass('hidden');

        //$row.find('.up-row').removeClass('hidden');
        //$row.find('.down-row').removeClass('hidden');
    },

    rowSetActionsDefault: function ($row) {
        $row.find('.on-editing').addClass('hidden');
        $row.find('.on-default').removeClass('hidden');

    },

    rowHidArrows: function ($row) {
        $row.children('td').each(function (i) {

            var $this = $(this);
            if ($this.hasClass('code') && $this.css('display') == 'none') {

                $row.find('.up-row').addClass('hidden');
                $row.find('.down-row').addClass('hidden');
            }
        });
    },

    rowValidate: function ($row) {


        var isValidRow = true;

        $row.find('td').map(function () {

            var $this = $(this);

            $this.each(function () {

                var select = $this.find('select');
                var input = $this.find('input');

                if (select.length > 0 && select.val() == "") {
                    $(select).css("border", "1px solid red");
                    isValidRow = false;
                }
                else
                    $(select).css("border", "1px solid #ccc");

                if ($this.hasClass('code') && $this.css('display') == 'none') {

                }
                else if (input.length > 0) {
                    if (input.attr("Id").indexOf("Description") < 0) {
                        if (input.val() == "") {
                            $(input).css("border", "1px solid red");
                            isValidRow = false;
                        }
                        else
                            $(input).css("border", "1px solid #ccc");
                    }

                }

            });
        });

        var Istitle_row = $row.hasClass('r_title');
        if (!Istitle_row) {
            var title_id = $row.closest('tr').prevAll('tr.r_title').first().attr('Id');
            if (title_id == "" || title_id == null || title_id == undefined) {
                if (isValidRow)
                    utility.DisplayMessages("Please Enter Title.", 3);

                isValidRow = false;

            }

        }


        if (isValidRow == false)
            return false;
        else
            return true;
    },

    rowDown: function ($row) {

        AppPrivileges.GetFormPrivileges("Super Bill", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var index = EditableTable.datatable.row($row).index();
                if ((index + 1) >= 0) {

                    var row_data = EditableTable.datatable.row(index).data();
                    var row_next = EditableTable.datatable.row(index + 1).data();

                    var length_ = EditableTable.datatable.data().length;

                    var Istitle_row = $row.next('tr').hasClass('r_title');
                    var Isedit_row = $row.next('tr').hasClass('adding');

                    //condition to ignore Edit and Title row.
                    if (Istitle_row != true && Isedit_row != true) {

                        var from_Id = $row.attr("Id");
                        var to_Id = $row.next('tr').attr("Id");

                        if (from_Id != undefined && to_Id != undefined) {
                            var data = "FromElementId=" + from_Id + "&ToElementId=" + to_Id;
                            SupperBillDetail.SortCode(data).done(function (response) {

                                if (response.status != false) {

                                    utility.DisplayMessages(response.Message, 1);

                                    // Draw row on grid
                                    EditableTable.rowSwap($row.get(0), $row.next('tr').get(0));
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }

                            });
                        }


                    }

                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    rowUp: function ($row) {

        AppPrivileges.GetFormPrivileges("Super Bill", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var index = EditableTable.datatable.row($row).index();
                if ((index - 1) >= 0) {

                    var row_data = EditableTable.datatable.row(index).data();
                    var row_per = EditableTable.datatable.row(index - 1).data();

                    var Istitle_row = $row.prev('tr').hasClass('r_title');



                    //condition to ignore Title row.
                    if (Istitle_row != true) {

                        var from_Id = $row.attr("Id");
                        var to_Id = $row.prev('tr').attr("Id");

                        if (from_Id != undefined && to_Id != undefined) {
                            var data = "FromElementId=" + from_Id + "&ToElementId=" + to_Id;
                            SupperBillDetail.SortCode(data).done(function (response) {

                                if (response.status != false) {

                                    utility.DisplayMessages(response.Message, 1);

                                    // Draw row on grid
                                    EditableTable.rowSwap($row.get(0), $row.prev('tr').get(0));
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }

                            });

                        }


                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    rowSwap: function (elm1, elm2) {

        var parent1, next1,
            parent2, next2;

        parent1 = elm1.parentNode;
        next1 = elm1.nextSibling;
        parent2 = elm2.parentNode;
        next2 = elm2.nextSibling;

        parent1.insertBefore(elm2, next1);
        parent2.insertBefore(elm1, next2);

    },

    rowTitle: function ($row) {

        $row.children('td').each(function (i) {

            var $this = $(this);

            if ($this.hasClass('actions')) {

            }
            else if ($this.hasClass('code')) {

                if ($this.css("display") != "none") {
                    $row.addClass('r_title');
                    $this.css("display", "none");
                }
                else {
                    $row.removeClass('r_title');
                    $this.css("display", "block");
                }

            }
            else {

                if (i == 3) {
                    //$row.hasClass('r_title')
                    if ($this.css("display") != "none") {
                        $this.css("display", "none");
                    }
                    else {
                        $this.css("display", "block");
                    }
                }

                if ($row.hasClass('r_title'))
                    $this.attr("colspan", "4");
                else
                    $this.attr("colspan", "0");
            }
        });

    },

   
};

//////-------------Data Grid Scripts Ends-----------------------

