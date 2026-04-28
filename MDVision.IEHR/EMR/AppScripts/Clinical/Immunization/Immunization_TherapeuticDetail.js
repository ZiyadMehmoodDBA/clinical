Immunization_TherapeuticDetail = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Immunization_TherapeuticDetail.params = params;
        if (Immunization_TherapeuticDetail.params.PanelID != 'pnlImmunization_TherapeuticDetail') {
            Immunization_TherapeuticDetail.params.PanelID = Immunization_TherapeuticDetail.params.PanelID + ' #pnlImmunization_TherapeuticDetail';
        } else {
            Immunization_TherapeuticDetail.params.PanelID = 'pnlImmunization_TherapeuticDetail';
        }
        var self = $('#' + Immunization_TherapeuticDetail.params.PanelID);
        if (Immunization_TherapeuticDetail.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                Immunization_TherapeuticDetail.bIsFirstLoad = false;
                Immunization_TherapeuticDetail.IntializeMultiSelectDropDown();
                Immunization_TherapeuticDetail.ValidateTherapeutic();
                if (Immunization_TherapeuticDetail.params.mode == "Edit") {
                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #frmAddTherapeutic #IsActive").attr("disabled", false);
                    Immunization_TherapeuticDetail.LoadTherapeuticDetail();
                }
                else {
                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #frmAddTherapeutic #IsActive").attr("disabled", true);
                }
            });
        }
    },
    LoadTherapeuticDetail: function () {
        Immunization_TherapeuticDetail.SearchTherapeuticDetail().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Immunization_TherapeuticDetail.params.PanelID + " #frmAddTherapeutic");
                
                var Therapeutic_JSON = response.Therapeutic_JSON[0];
                utility.bindMyJSONByName(true, Therapeutic_JSON, false, self).done(function () {

                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode").attr('code', Therapeutic_JSON.CPTCode);
                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode").attr('description', Therapeutic_JSON.CPTDescription);
                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode").attr('code', Therapeutic_JSON.AdminCode);
                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode").attr('description', Therapeutic_JSON.AdminCodeDescription);
                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode").val((Therapeutic_JSON.CPTCode !== "" && Therapeutic_JSON.CPTCode !== null) ? Therapeutic_JSON.CPTCode + " - " + Therapeutic_JSON.CPTDescription : "");
                    $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode").val((Therapeutic_JSON.AdminCode !== "" && Therapeutic_JSON.AdminCode !== null) ? Therapeutic_JSON.AdminCode + " - " + Therapeutic_JSON.AdminCodeDescription : "");

                    if (Therapeutic_JSON.ManufactureIds != null && Therapeutic_JSON.ManufactureIds != '') {
                        $('#' + Immunization_TherapeuticDetail.params.PanelID + " #ddlManufacture").val(Therapeutic_JSON.ManufactureIds.split(','));
                        $('#' + Immunization_TherapeuticDetail.params.PanelID + " #ddlManufacture").multiselect("refresh");
                        $('#' + Immunization_TherapeuticDetail.params.PanelID + " #ddlManufacture").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 300
                        });
                    }

                    if (Therapeutic_JSON.Status == "True") {
                        $('#' + Immunization_TherapeuticDetail.params.PanelID + " #frmAddTherapeutic #IsActive").prop('checked', true);
                    }
                    else {
                        $('#' + Immunization_TherapeuticDetail.params.PanelID + " #frmAddTherapeutic #IsActive").prop('checked', false);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SearchTherapeuticDetail: function () {
        var objData = new Object();
        objData["TherapeuticId"] = Immunization_TherapeuticDetail.params.TherapeuticId;
        objData["commandType"] = "Load_Therapeutic_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    IntializeMultiSelectDropDown: function () {
        $('#' + Immunization_TherapeuticDetail.params.PanelID + ' #ddlManufacture').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 300
        });
    },
    bindAutoComplete: function (element, type) {

        if (type == "CPT") {
            if ($(element).val() == "") {
                $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode").attr('code', '');
                $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode").attr('description', '');
            }
            var hiddenCrtl = $('#' + Immunization_TherapeuticDetail.params.PanelID + ' #txtCPTCode');
            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Immunization_TherapeuticDetail", null, true);
        }
        else {
            if ($(element).val() == "") {
                $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode").attr('code', '');
                $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode").attr('description', '');
            }
            var hiddenCrtl = $('#' + Immunization_TherapeuticDetail.params.PanelID + ' #txtAdminCode');
            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Immunization_TherapeuticDetail", null, true);
        }
    },
    openCPTCode: function (type) {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Immunization_TherapeuticDetail";
        if (type == "CPT") {
            params["RefHiddenCtrl"] = '#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode";
            params["RefCtrl"] = "txtCPTCode";
        }
        else {
            params["RefHiddenCtrl"] = '#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode";
            params["RefCtrl"] = "txtAdminCode";
        }
        params["ParentCtrlPanelID"] = Immunization_TherapeuticDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Immunization_TherapeuticDetail.params.PanelID);
    },
    ValidateTherapeutic: function () {
        $('#' + Immunization_TherapeuticDetail.params.PanelID + ' #frmAddTherapeutic')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   TherInjName: {
                       group: '.col-md-9',
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
            Immunization_TherapeuticDetail.TherapeuticSave();
        });
    },
    
    TherapeuticSave: function () {
        var self = $("#" + Immunization_TherapeuticDetail.params.PanelID + " #frmAddTherapeutic");
        var myJSON = self.getMyJSONByName();
        if (Immunization_TherapeuticDetail.params.mode == "Add") {
            Immunization_TherapeuticDetail.SaveTherapeutic(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic();
                    Immunization_TherapeuticDetail.UnLoadTab();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else if (Immunization_TherapeuticDetail.params.mode == "Edit") {
            Immunization_TherapeuticDetail.SaveTherapeutic(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic();
                    Immunization_TherapeuticDetail.UnLoadTab();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveTherapeutic: function (TherapeuticData) {
        var objData = JSON.parse(TherapeuticData);
        objData["CPTCode"] = $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode").attr('code');
        objData["CPTDescription"] = $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtCPTCode").attr('description');
        objData["AdminCode"] = $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode").attr('code');
        objData["AdminCodeDescription"] = $('#' + Immunization_TherapeuticDetail.params.PanelID + " #txtAdminCode").attr('description');
        //var VaccineProblems = [];
        //$.each($('#' + Immunization_AddVaccine.params.PanelID + " #dgvVaccineProblems tbody tr"), function (i,item) {
        //    var Problem = {};
        //    Problem.Code = $(item).attr("code");
        //    Problem.Description = $(item).attr("description");
        //    VaccineProblems.push(Problem);
        //});
        //objData["VaccineProblems"] = VaccineProblems;

        

        if (Immunization_TherapeuticDetail.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_Therapeutic";
            if (objData.IsActive) {
                objData["Status"] = "1";
            }
            else {
                objData["Status"] = "0";
            }
            objData["TherapeuticId"] = Immunization_TherapeuticDetail.params.TherapeuticId;
        }
        else {
            objData["commandType"] = "SAVE_Therapeutic";
            objData["Status"] = "1";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_TherapeuticDetail.params["FromAdmin"] == "0") {
            if (Immunization_TherapeuticDetail.params != null && Immunization_TherapeuticDetail.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_TherapeuticDetail.params.ParentCtrl, 'Immunization_TherapeuticDetail');
            }
            else
                UnloadActionPan(null, 'Immunization_TherapeuticDetail');
        }
        else {
            RemoveAdminTab();
        }
        return objDeffered;
    },
}