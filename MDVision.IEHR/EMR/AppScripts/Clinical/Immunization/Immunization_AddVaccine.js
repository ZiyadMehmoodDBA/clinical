Immunization_AddVaccine = {
    bIsFirstLoad: true,
    params: [],
    LastSctBaseSearch: '',
    Load: function (params) {
        Immunization_AddVaccine.params = params;
        if (Immunization_AddVaccine.params.PanelID != 'pnlImmunization_AddVaccine') {
            Immunization_AddVaccine.params.PanelID = Immunization_AddVaccine.params.PanelID + ' #pnlImmunization_AddVaccine';
        } else {
            Immunization_AddVaccine.params.PanelID = 'pnlImmunization_AddVaccine';
        }

        var self = $('#' + Immunization_AddVaccine.params.PanelID);

        if (Immunization_AddVaccine.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                Immunization_AddVaccine.bIsFirstLoad = false;
                Immunization_AddVaccine.IntializeMultiSelectDropDown();
                utility.CreateDatePicker(Immunization_AddVaccine.params.PanelID + ' #frmAddImmunization #dpVISDate', function (ev) { }, false, "frmAddImmunization", true);
                Immunization_AddVaccine.ValidateImmunization();
                if (Immunization_AddVaccine.params.mode == "Edit") {
                    $('#' + Immunization_AddVaccine.params.PanelID + " #frmAddImmunization #IsActive").attr("disabled", false);
                    Immunization_AddVaccine.LoadVaccineDetail();
                }
                else {
                    $('#' + Immunization_AddVaccine.params.PanelID + " #frmAddImmunization #IsActive").attr("disabled", true);
                }
            });
        }
    },
    LoadVaccineDetail: function () {
        Immunization_AddVaccine.SearchVaccineDetail().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Immunization_AddVaccine.params.PanelID + " #frmAddImmunization");
                var ImmunizationJSONData = response.Vaccine_JSON;
                utility.bindMyJSONByName(true, ImmunizationJSONData, false, self).done(function () {

                    $('#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode").attr('code', ImmunizationJSONData.CPTCode);
                    $('#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode").attr('description', ImmunizationJSONData.CPTDescription);
                    $('#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode").attr('code', ImmunizationJSONData.AdminCode);
                    $('#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode").attr('description', ImmunizationJSONData.AdminCodeDescription);
                    $('#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode").val((ImmunizationJSONData.CPTCode !== "" && ImmunizationJSONData.CPTCode !== null) ? ImmunizationJSONData.CPTCode + " - " + ImmunizationJSONData.CPTDescription : "");
                    $('#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode").val((ImmunizationJSONData.AdminCode !== "" && ImmunizationJSONData.AdminCode !== null) ? ImmunizationJSONData.AdminCode + " - " + ImmunizationJSONData.AdminCodeDescription : "");

                    $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody').empty();
                    $.each(ImmunizationJSONData.VaccineVisInformation, function (i, item) {

                        var $row;

                        var id = ($("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr').length) + 1;
                        $row = $('<tr id="' + item.VaccineVISId + '" VisId="' + item.VaccineVISId + '" VisUrlId="' + item.VaccineVIS_URLId + '" VisDate="' + item.VISDate.split(' ')[0] + '" DocumentName="' + item.VISDocumentName + '" DocumentLink="' + item.VISDocumentLink + '" EncodedText="' + item.VISFullyEncodedText + '"/>');
                        $row.append('<td>' + '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_AddVaccine.deleteVisInformation(this);"><i class="fa fa-times red"></i></a>' + '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_AddVaccine.EditVisInformation(this);"><i class="fa fa-edit black"></i></a>' + '</td>');
                        $row.append('<td id="VisDate">' + item.VISDate.split(' ')[0] + '</td>');
                        $row.append('<td id="DocumentName">' + item.VISDocumentName + '</td>');
                        $row.append('<td id="DocumentLink">' + item.VISDocumentLink + '</td>');
                        $row.append('<td id="EncodedText">' + item.VISFullyEncodedText + '</td>');
                        $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody').last().append($row);

                    });



                    if (ImmunizationJSONData.ManufactureIds != null && ImmunizationJSONData.ManufactureIds != '') {
                        $('#' + Immunization_AddVaccine.params.PanelID + " #ddlManufacture").val(ImmunizationJSONData.ManufactureIds.split(','));
                        $('#' + Immunization_AddVaccine.params.PanelID + " #ddlManufacture").multiselect("refresh");
                        $('#' + Immunization_AddVaccine.params.PanelID + " #ddlManufacture").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 300
                        });
                    }

                    if (ImmunizationJSONData.Status == "Active") {
                        $('#' + Immunization_AddVaccine.params.PanelID + " #frmAddImmunization #IsActive").prop('checked', true);
                    }
                    else {
                        $('#' + Immunization_AddVaccine.params.PanelID + " #frmAddImmunization #IsActive").prop('checked', false);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SearchVaccineDetail: function () {
        var objData = new Object();
        objData["ImmunizationId"] = Immunization_AddVaccine.params.ImmunizationId;
        objData["commandType"] = "Load_Vaccine_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },

    IntializeMultiSelectDropDown: function () {
        $('#' + Immunization_AddVaccine.params.PanelID + ' #ddlManufacture').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 300
        });
    },
    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {
            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                Immunization_AddVaccine.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                Immunization_AddVaccine.LastSctBaseSearch = "";
            }
        }
        else {
            Immunization_AddVaccine.LastSctBaseSearch = "";
        }
        $('#' + Immunization_AddVaccine.params.PanelID + ' #txtProblems').attr("data-popupunload", "false");

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Immunization_AddVaccine", null, false);
    },
    bindAutoComplete: function (element, type) {

        if (type == "CPT") {
            if ($(element).val() == "") {
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode").attr('code', '');
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode").attr('description', '');
            }
            var hiddenCrtl = $('#' + Immunization_AddVaccine.params.PanelID + ' #txtCPTCode');
            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Immunization_AddVaccine", null, true);
        }
        else {
            if ($(element).val() == "") {
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode").attr('code', '');
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode").attr('description', '');
            }
            var hiddenCrtl = $('#' + Immunization_AddVaccine.params.PanelID + ' #txtAdminCode');
            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Immunization_AddVaccine", null, true);
        }
    },
    openCPTCode: function (type) {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Immunization_AddVaccine";
        if (type == "CPT") {
            params["RefHiddenCtrl"] = '#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode";
            params["RefCtrl"] = "txtCPTCode";
        }
        else {
            params["RefHiddenCtrl"] = '#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode";
            params["RefCtrl"] = "txtAdminCode";
        }
        params["ParentCtrlPanelID"] = Immunization_AddVaccine.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Immunization_AddVaccine.params.PanelID);
    },
    OpenSearchPopup: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Immunization_AddVaccine";
        params["RefCtrl"] = "txtProblems";

        $('#' + Immunization_AddVaccine.params.PanelID + ' #txtProblems').attr('data-popupunload', 'true');

        params["Parent"] = 'pnlAdminIMOICD';
        HiddenCtrl = 'hfICD1-1,hfICDDescription1-1,hfICD101-1,hfICD10Description1-1,hfSNOMED1-1,hfSNOMEDDescription1-1';
        params["RefHiddenCtrl"] = HiddenCtrl;
        LoadActionPan(controlToLoad, params);
    },
    AddProblemRow: function (Code, Description) {
        //$('#' + Immunization_AddVaccine.params.PanelID + ' #txtProblems').
        //alert(Code + "---" + Description);
        var $row;
        $row = $('<tr code="' + Code + '" Description="' + Description + '"/>');
        $row.append('<td>' + '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_AddVaccine.deleteVaccineProblem(this);"><i class="fa fa-times red"></i></a></td>');
        $row.append('<td>' + Code + '</td>');
        $row.append('<td>' + Description + '</td>');
        $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVaccineProblems tbody').last().append($row);
    },
    deleteVaccineProblem: function (obj) {
        utility.myConfirm('1', function () {
            $(obj).closest("tr").remove();
        }, function () { },
                    '1'
                );
    },
    ValidateImmunization: function () {
        $('#' + Immunization_AddVaccine.params.PanelID + ' #frmAddImmunization')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   ImmunizationName: {
                       group: '.col-md-9',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CVX: {
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
            Immunization_AddVaccine.ImmunizationSave();
        });
    },
    ImmunizationSave: function () {
        var self = $("#" + Immunization_AddVaccine.params.PanelID + " #frmAddImmunization");
        var myJSON = self.getMyJSONByName();
        if (Immunization_AddVaccine.params.mode == "Add") {
            Immunization_AddVaccine.SaveImmunization(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic();
                    Immunization_AddVaccine.UnLoadTab();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else if (Immunization_AddVaccine.params.mode == "Edit") {
            Immunization_AddVaccine.SaveImmunization(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic();
                    Immunization_AddVaccine.UnLoadTab();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveImmunization: function (ImmunizationData) {
        var objData = JSON.parse(ImmunizationData);
        objData["CPTCode"] = $('#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode").attr('code');
        objData["CPTDescription"] = $('#' + Immunization_AddVaccine.params.PanelID + " #txtCPTCode").attr('description');
        objData["AdminCode"] = $('#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode").attr('code');
        objData["AdminCodeDescription"] = $('#' + Immunization_AddVaccine.params.PanelID + " #txtAdminCode").attr('description');
        //var VaccineProblems = [];
        //$.each($('#' + Immunization_AddVaccine.params.PanelID + " #dgvVaccineProblems tbody tr"), function (i,item) {
        //    var Problem = {};
        //    Problem.Code = $(item).attr("code");
        //    Problem.Description = $(item).attr("description");
        //    VaccineProblems.push(Problem);
        //});
        //objData["VaccineProblems"] = VaccineProblems;

        var VaccineVisInformation = [];
        $.each($('#' + Immunization_AddVaccine.params.PanelID + " #dgvVISInformation tbody tr"), function (i, item) {
            var VIS = {};
            if ($(item).attr("id") > 0) {
                VIS.Mode = "Edit";
                VIS.VaccineVISId = $(item).attr("VisId");
                VIS.VaccineVIS_URLId = $(item).attr("VisUrlId");
            }
            else {
                VIS.Mode = "Add";
            }
            VIS.VISDate = $(item).attr("VisDate");
            VIS.VISDocumentName = $(item).attr("DocumentName");
            VIS.VISDocumentLink = $(item).attr("DocumentLink");
            VIS.VISFullyEncodedText = $(item).attr("EncodedText");
            VaccineVisInformation.push(VIS);
        });
        objData["VaccineVisInformation"] = VaccineVisInformation;

        if (Immunization_AddVaccine.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_Vaccine";
            if (objData.IsActive) {
                objData["Status"] = "Active";
            }
            else {
                objData["Status"] = "InActive";
            }
            objData["ImmunizationId"] = Immunization_AddVaccine.params.ImmunizationId;
        }
        else {
            objData["commandType"] = "SAVE_Vaccine";
            objData["Status"] = "Active";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    VisButtonClick: function () {
        var WhatVisButtonDo = $('#' + Immunization_AddVaccine.params.PanelID + " #WhatVisButtonDo").val()//if value is 1 then add new vis information else value  is 2 then edit information in vis grid
        var VisDate = $('#' + Immunization_AddVaccine.params.PanelID + " #dpVISDate").val();
        var DocumentName = $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentName").val();
        var DocumentLink = $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentLink").val();
        var EncodedText = $('#' + Immunization_AddVaccine.params.PanelID + " #txtVISFullyEncodedText").val();
        if (VisDate != "" && DocumentName != "" && DocumentLink != "" && EncodedText != "") {
            if (WhatVisButtonDo == 1) {
                var $row;

                var id = ($("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr').length) + 1;
                $row = $('<tr id="-' + id + '" VisId="-1" VisUrlId="-1" VisDate="' + VisDate + '" DocumentName="' + DocumentName + '" DocumentLink="' + DocumentLink + '" EncodedText="' + EncodedText + '"/>');
                $row.append('<td>' + '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_AddVaccine.deleteVisInformation(this);"><i class="fa fa-times red"></i></a>' + '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_AddVaccine.EditVisInformation(this);"><i class="fa fa-edit black"></i></a>' + '</td>');
                $row.append('<td id="VisDate">' + VisDate + '</td>');
                $row.append('<td id="DocumentName">' + DocumentName + '</td>');
                $row.append('<td id="DocumentLink">' + DocumentLink + '</td>');
                $row.append('<td id="EncodedText">' + EncodedText + '</td>');
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody').last().append($row);
                $('#' + Immunization_AddVaccine.params.PanelID + " #dpVISDate").val("")
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentName").val("");
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentLink").val("");
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtVISFullyEncodedText").val("");
            }
            else {

                var WhichRowEdit = $('#' + Immunization_AddVaccine.params.PanelID + " #WhichRowEdit").val();
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit).attr("VisDate", VisDate);
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit).attr("DocumentName", DocumentName);
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit).attr("DocumentLink", DocumentLink);
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit).attr("EncodedText", EncodedText);

                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit + " #VisDate").html(VisDate);
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit + " #DocumentName").html(DocumentName);
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit + " #DocumentLink").html(DocumentLink);
                $("#" + Immunization_AddVaccine.params.PanelID + ' #dgvVISInformation tbody tr#' + WhichRowEdit + " #EncodedText").html(EncodedText);
                $('#' + Immunization_AddVaccine.params.PanelID + " #dpVISDate").val("")
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentName").val("");
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentLink").val("");
                $('#' + Immunization_AddVaccine.params.PanelID + " #txtVISFullyEncodedText").val("");
                $('#' + Immunization_AddVaccine.params.PanelID + " #VisButton").html("Add VIS Informtion");
                $('#' + Immunization_AddVaccine.params.PanelID + " #WhatVisButtonDo").val("1");
                $('#' + Immunization_AddVaccine.params.PanelID + " #WhichRowEdit").val("");
            }
        }
        else {
            utility.DisplayMessages("Information is missing", 2);
        }
        
    },

    VisCancleClick: function () {
        $('#' + Immunization_AddVaccine.params.PanelID + " #dpVISDate").val("")
        $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentName").val("");
        $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentLink").val("");
        $('#' + Immunization_AddVaccine.params.PanelID + " #txtVISFullyEncodedText").val("");
        $('#' + Immunization_AddVaccine.params.PanelID + " #VisButton").html("Add VIS Informtion");
        $('#' + Immunization_AddVaccine.params.PanelID + " #WhatVisButtonDo").val("1");
        $('#' + Immunization_AddVaccine.params.PanelID + " #WhichRowEdit").val("");
    },
    deleteVisInformation: function (obj) {
        utility.myConfirm('1', function () {
            if ($(obj).closest("tr").attr("id") > 0) {
                Immunization_AddVaccine.DeleteVISinformation($(obj).closest("tr").attr("id")).done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {
                        $(obj).closest("tr").remove();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                $(obj).closest("tr").remove();
            }
        }, function () { },
                    '1'
                );

    },
    DeleteVISinformation: function (VaccineVISId) {
        var objData = new Object();
        objData["commandType"] = "DELETE_VaccineVIS";
        objData["VaccineVISId"] = VaccineVISId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    EditVisInformation: function (obj) {
        $('#' + Immunization_AddVaccine.params.PanelID + " #VisButton").html("Edit VIS Informtion");
        $('#' + Immunization_AddVaccine.params.PanelID + " #WhatVisButtonDo").val("2");
        $('#' + Immunization_AddVaccine.params.PanelID + " #WhichRowEdit").val($(obj).closest("tr").attr("id"));
        $('#' + Immunization_AddVaccine.params.PanelID + ' #dpVISDate').datepicker('setDate', $(obj).closest("tr").attr("VisDate"));
        $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentName").val($(obj).closest("tr").attr("DocumentName"));
        $('#' + Immunization_AddVaccine.params.PanelID + " #txtDocumentLink").val($(obj).closest("tr").attr("DocumentLink"));
        $('#' + Immunization_AddVaccine.params.PanelID + " #txtVISFullyEncodedText").val($(obj).closest("tr").attr("EncodedText"));
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_AddVaccine.params["FromAdmin"] == "0") {
            if (Immunization_AddVaccine.params != null && Immunization_AddVaccine.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_AddVaccine.params.ParentCtrl, 'Immunization_AddVaccine');
            }
            else
                UnloadActionPan(null, 'Immunization_AddVaccine');
        }
        else {
            RemoveAdminTab();
        }
        return objDeffered;
    },
}

