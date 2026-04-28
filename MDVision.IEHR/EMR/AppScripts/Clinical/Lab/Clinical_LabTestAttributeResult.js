Clinical_LabTestAttributeResult = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_LabTestAttributeResult.params = params;
        if (Clinical_LabTestAttributeResult.params.PanelID != 'pnlClinicalLabTestAttributesResult')
            Clinical_LabTestAttributeResult.params.PanelID = Clinical_LabTestAttributeResult.params.PanelID + ' #pnlClinicalLabTestAttributesResult';
        else
            Clinical_LabTestAttributeResult.params.PanelID = 'pnlClinicalLabTestAttributesResult';

        Clinical_LabTestAttributeResult.LoadResultList(Clinical_LabTestAttributeResult.params.LabTestAttributeId);
        Clinical_LabTestAttributeResult.ValidateLabTestAttributeResult();
    },
    ValidateLabTestAttributeResult: function () {
        var self = $('#' + Clinical_LabTestAttributeResult.params.PanelID + ' #frmClinicalLabTestAttributesResult');
        self.bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                ClinicalLabTestAttributesResultName: {
                    enable: false,
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
            }
        })
     .on('success.form.bv', function (e) {
         e.preventDefault();
         Clinical_LabTestAttributeResult.Save();
     });
    },
    Save: function () {
        if ($('#' + Clinical_LabTestAttributeResult.params.PanelID + ' #frmClinicalLabTestAttributesResult').data("bootstrapValidator") != null)
            $('#' + Clinical_LabTestAttributeResult.params.PanelID + ' #frmClinicalLabTestAttributesResult').bootstrapValidator('revalidateField', 'ClinicalLabTestAttributesResultName');
        if ($("#" + Clinical_LabTestAttributeResult.params.PanelID + " #txtClinicalLabTestAttributesResult").val() != "") {
            var objData = {};
            objData["LabTestAttributeId"] = Clinical_LabTestAttributeResult.params.LabTestAttributeId;
            objData["ResultName"] = $("#" + Clinical_LabTestAttributeResult.params.PanelID + " #txtClinicalLabTestAttributesResult").val();
            objData["commandType"] = "insert_labtestattribute_result";
            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest").done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_LabTestAttributeResult.LoadResultList(Clinical_LabTestAttributeResult.params.LabTestAttributeId);
                    $("#" + Clinical_LabTestAttributeResult.params.PanelID + " #txtClinicalLabTestAttributesResult").val("");
                    if ($('#' + Clinical_LabTestAttributeResult.params.PanelID + ' #frmClinicalLabTestAttributesResult').data("bootstrapValidator") != null)
                        $('#' + Clinical_LabTestAttributeResult.params.PanelID + ' #frmClinicalLabTestAttributesResult').bootstrapValidator('revalidateField', 'ClinicalLabTestAttributesResultName');
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }
    },
    LoadResultList: function (LabTestAttributeId) {
        Clinical_LabTestAttributeResult.searchLabTestAttributeResult_DBCall(Clinical_LabTestAttributeResult.params.LabTestAttributeId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false)
                Clinical_LabTestAttributeResult.LabTestAttributeResultListLoad(response);
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    searchLabTestAttributeResult_DBCall: function (LabTestAttributeId) {
        var objData = {};
        objData["commandType"] = "load_labtestattribute_result";
        objData["LabTestAttributeId"] = LabTestAttributeId
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },
    LabTestAttributeResultListLoad: function (response) {
        $("#" + Clinical_LabTestAttributeResult.params.PanelID + ' #ulClinicalLabTestAttributesResultList').empty();
        if (response.LabTestAttributeResultCount > 0) {
            var LabTestAttributeResultJSON = JSON.parse(response.LabTestAttributeResultJSON);
            $.each(LabTestAttributeResultJSON, function (i, item) {
                var onclick = "Clinical_LabTestAttributeResult.deleteResult(this,'" + item.LabTestAttributeResultId + "');";
                var $list = $("#" + Clinical_LabTestAttributeResult.params.PanelID + " #ulClinicalLabTestAttributesResultList");
                var li = '<li id=' + item.LabTestAttributeResultId + '><span class="pull-left pr-xlg">' + item.ResultName + ' </span><span class="removeIconListHover">' +
                              '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                              '</span><div class="clearfix"></div></li>';
                $list.append(li);
            });
        }
    },
    deleteResult: function (obj, LabTestAttributeResultId) {
        var objData = {};
        objData["LabTestAttributeResultId"] = LabTestAttributeResultId;
        objData["commandType"] = "delete_labtestattribute_result";
        var data = JSON.stringify(objData);
        MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest").done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_LabTestAttributeResult.LoadResultList(Clinical_LabTestAttributeResult.params.LabTestAttributeId);
                utility.DisplayMessages(response.Message, 1);
            }

            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    ValidateSpecialCharacters: function (obj) {
        var pat_ = /"/;
        if (pat_.test($(obj).val())) {
            $(obj).val($(obj).val().replace(pat_, ""));
            if (pat_.test($(obj).val()))
                Clinical_LabTestAttributeResult.ValidateSpecialCharacters(obj);
        }
    },
    UnLoadTab: function () {
        if (Clinical_LabTestAttributeResult.params["FromAdmin"] == "0") {
            if (Clinical_LabTestAttributeResult.params != null && Clinical_LabTestAttributeResult.params.ParentCtrl != null)
                UnloadActionPan(Clinical_LabTestAttributeResult.params.ParentCtrl, 'Clinical_LabTestAttributeResult');
            else
                UnloadActionPan(null, 'Clinical_LabTestAttributeResult');
        }
        else
            RemoveAdminTab();
    },
}