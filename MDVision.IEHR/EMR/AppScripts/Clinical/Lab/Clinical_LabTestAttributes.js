ClinicalLabTestAttributes = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    AttributesList: [],
    Load: function (params) {
        BackgroundLoaderShow(true);
        ClinicalLabTestAttributes.params = params;

        var labId = ClinicalLabTestAttributes.params.LabId;
        $('#pnlClinicalLabTestAttributes #hfLabId').val(labId);
        var self = $('#pnlClinicalLabTestAttributes');

        if (ClinicalLabTestAttributes.bIsFirstLoad == true) {

            ClinicalLabTestAttributes.bIsFirstLoad = false;

            ClinicalLabTestAttributes.validateClinicalLabTestAttributes();


            self.loadDropDowns(true).done(function () {

                if (ClinicalLabTestAttributes.params.mode == "Edit") {
                    ClinicalLabTestAttributes.loadLabTest(labId);
                    $("#pnlClinicalLabTestAttributes #btnSaveAttributes").prop("disabled", false);
                } else {
                    $("#pnlClinicalLabTestAttributes #btnSaveAttributes").prop("disabled", true);
                }

                $('#pnlClinicalLabTestAttributes #frmClinicalLabTestAttributes').data('serialize', $('#pnlClinicalLabTestAttributes #frmClinicalLabTestAttributes').serialize());
            });
        }
    },

    validateClinicalLabTestAttributes: function () {


        $('#pnlClinicalLabTestAttributes #frmClinicalLabTestAttributes')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   LabCPTCode: {
                       group: '.col-sm-10',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Attribute: {
                       group: '.col-sm-10',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //Description: {
                   //    group: '.col-sm-10',
                   //    enabled: false,
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
            ClinicalLabTestAttributes.ClinicalLabTestSave();
            e.type = "";
        });

    },

    loadLabTest: function () {

        ClinicalLabTestAttributes.loadLabTest_DBCall().done(function (response) {

            if (response != "") {
                var data = JSON.parse(response);
                if (data.status != false) {
                    var ClinicalLabTestJSONData = JSON.parse(data.ClinicalLabTest_JSON);
                    var ClinicalLabTestAttributeJSONData = JSON.parse(data.ClinicalLabTestAttribute_JSON);

                    $("#pnlClinicalLabTestAttributes #txtLabCPTCode").val(ClinicalLabTestJSONData[0].LOINC + " " + ClinicalLabTestJSONData[0].LOINCDescription);

                    $("#pnlClinicalLabTestAttributes #txtLabCPTCode").attr("disabled", true);
                    $("#pnlClinicalLabTestAttributes #chkTemplate").attr("disabled", true);
                    if (ClinicalLabTestJSONData[0].IsActive == "True") {
                        $('#pnlClinicalLabTestAttributes #chkActive').prop('checked', true);
                    } else {
                        $('#pnlClinicalLabTestAttributes #chkActive').prop('checked', false);
                    }
                    if (ClinicalLabTestJSONData[0].IsTemplate == "True") {
                        $('#pnlClinicalLabTestAttributes #chkTemplate').prop('checked', true);
                        $("#pnlClinicalLabTestAttributes #chkTemplate").trigger("change");
                        if (ClinicalLabTestAttributeJSONData.length > 0) {
                            $("#pnlClinicalLabTestAttributes #txtDescription").val(ClinicalLabTestAttributeJSONData[0].Description);
                        }
                    }
                    $("#pnlClinicalLabTestAttributes #hfLLOINICCODE").val(ClinicalLabTestJSONData[0].LOINC);
                    $("#pnlClinicalLabTestAttributes #hfLOINICDescription").val(ClinicalLabTestJSONData[0].LOINCDescription);
                    $("#pnlClinicalLabTestAttributes #hfOBSERVATION").val(ClinicalLabTestJSONData[0].LOINC + " " + ClinicalLabTestJSONData[0].LOINCDescription);
                    ClinicalLabTestAttributes.labTestAttributesGridLoad(data);
                }
            }
        });
    },
    validateUOM: function (event) {

        if (navigator.userAgent.search("Firefox") > -1) {
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39 || event.key == "Tab") return true;
        }

        var code = event.keyCode || event.charCode;
        var test = /^[a-zA-Z!”@#$%&’()*\+,\/;\[\\\]\^_`{|}~]+$/;
        var value = String.fromCharCode(code);
        if (value.match(test)) {
            return true;
        }
        else {
            event.preventDefault();
            return false;

        }
    },
    checkRange: function (event) {
        if (navigator.userAgent.search("Firefox") > -1 || event.charCode == 46) {
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39 || event.key == "Tab" || event.charCode == 46) return true;
        }
        var code = event.keyCode || event.charCode;
        var test = /^[0-9]|\-$/;
        var value = String.fromCharCode(code);
        if (value != "" && value.match(test)) {
            return true;
        }
        else {
            event.preventDefault();
            // $(obj).val(""); //emtpy range value if does not match the pattern
            return false;
        }
    },
    labTestAttributesGridLoad: function (response) {
        $("#pnlClinicalLabTestAttributes #dgvLabTestAttributesDtl").dataTable().fnDestroy();
        $("#pnlClinicalLabTestAttributes #dgvLabTestAttributesDtl tbody").find("tr").remove();
        ClinicalLabTestAttributes.AttributesList = [];
        if (response.ClinicalLabTestAttributeCount > 0) {
            var ClinicalLabTestAttributeLoadJSONData = JSON.parse(response.ClinicalLabTestAttribute_JSON);
            if (ClinicalLabTestAttributeLoadJSONData[0].AttributeName != "") {
                $.each(ClinicalLabTestAttributeLoadJSONData, function (i, item) {
                    ClinicalLabTestAttributes.AttributesList.push(item.AttributeName);
                    var $row = $('<tr/>');
                    $row.attr("AttributeId", item.LabTestAttributeId);

                    $row.append('<td style="display:none;">' + item.LabTestAttributeId + '</td><td onmouseover="ClinicalLabTestAttributes.showIcon(this);" onmouseout="ClinicalLabTestAttributes.hideIcon(this);">' + item.AttributeName + '<a href="#" style="display:none;"><span  class="removeIconListHover" onclick="ClinicalLabTestAttributes.deleteTestAttribute(' + item.LabTestAttributeId + ',event);"><i class="fa fa-close red"></i></span></a></td><td><input class="form-control" name="UOM" value="' + item.UoM + '" id="txtUOM" onkeypress="ClinicalLabTestAttributes.validateUOM(event)" onblur="ClinicalLabTestAttributes.saveUOMRange(this,' + item.LabTestAttributeId + ');" type="text" /></td><td><input class="form-control" name="Range" value="' + item.Range + '" id="txtRange" type="text" onkeypress="ClinicalLabTestAttributes.checkRange(event)"  onblur="ClinicalLabTestAttributes.saveUOMRange(this,' + item.LabTestAttributeId + ');" /></td><td><a href="#" onclick="ClinicalLabTestAttributes.AddResult(' + item.LabTestAttributeId + ')">Add Result</td>');

                    $("#pnlClinicalLabTestAttributes #dgvLabTestAttributesDtl tbody").last().append($row);
                });
            } else {
                $('#pnlClinicalLabTestAttributes #dgvLabTestAttributesDtl').DataTable({
                    "language": {
                        "emptyTable": "No Attribute Found."
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });

                //$('#pnlClinicalLabTestAttributes #txtLabCPTCode,#chkTemplate').prop("disabled", false);
            }
        }
        else {
            $('#pnlClinicalLabTestAttributes #dgvLabTestAttributesDtl').DataTable({
                "language": {
                    "emptyTable": "No Attribute Found."
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            $('#pnlClinicalLabTestAttributes #txtLabCPTCode,#chkTemplate').prop("disabled", false);
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#pnlClinicalLabTestAttributes #dgvLabTestAttributesDtl'))
            ;
        else
            $("#pnlClinicalLabTestAttributes #dgvLabTestAttributesDtl").DataTable({ "bLengthChange": false, "order": [[0, "asc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [2] }] }); // to remove records per page dropdown

    },
    AddResult: function (LabTestAttributeId) {
        var params = [];
        params["ParentCtrl"] = "ClinicalLabTestAttributes";
        params["FromAdmin"] = 0;
        params["LabTestAttributeId"] = LabTestAttributeId;
        params["LabId"] = ClinicalLabTestAttributes.params.LabId;
        params["LabTestId"] = ClinicalLabTestAttributes.params.LabTestId;
        LoadActionPan("Clinical_LabTestAttributeResult", params);
    },

    saveUOMRange: function (obj, rowId) {


        ClinicalLabTestAttributes.ClinicalLabTestAttributeEdit_DBCall(obj, rowId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
            } else {
                utility.DisplayMessages(response.message, 1);
            }

        });
    },
    loadLabTest_DBCall: function () {
        var objData = new Object();
        objData["LabId"] = ClinicalLabTestAttributes.params.LabId;
        objData["LabTestId"] = ClinicalLabTestAttributes.params.LabTestId;
        objData["commandType"] = "load_clinical_lab_test";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    ClinicalLabTestSave: function () {

        var self = $("#pnlClinicalLabTestAttributes");
        var myJSON = self.getMyJSONByName();


        var isAttributeExists = false;
        var isLabExists = false;

        if (ClinicalLabTestAttributes.AttributesList.length > 0) {
            $.each(ClinicalLabTestAttributes.AttributesList, function (i, l) {
                if (ClinicalLabTestAttributes.AttributesList[i].trim() == $("#pnlClinicalLabTestAttributes #txtAttribute").val().trim()) {
                    isAttributeExists = true;
                }
            });
        }

        if (ClinicalLabTestAttributes.params.mode == "Add") {
            if (ClinicalLabTestAttributes.params.LabNames && ClinicalLabTestAttributes.params.LabNames.length > 0) {
                $.each(ClinicalLabTestAttributes.params.LabNames, function (i, l) {
                    if (ClinicalLabTestAttributes.params.LabNames[i].trim() == $("#pnlClinicalLabTestAttributes #hfLLOINICCODE_AND_DESCRIPTION").val().trim()) {
                        isLabExists = true;
                    }
                });
            }
            if (!isLabExists) {
                ClinicalLabTestAttributes.ClinicalLabTestSave_DBCall(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#pnlClinicalLabTestAttributes #hfLabTest').val(response.labTestId);
                        ClinicalLabTestAttributes.params.LabTestId = response.labTestId;
                        utility.DisplayMessages(response.message, 1);
                        ClinicalLabTestAttributes.params.mode = "Edit";
                        $("#pnlClinicalLabTestAttributes #txtLabCPTCode,#chkTemplate").attr("disabled", true);
                        $("#pnlClinicalLabTestAttributes #btnSaveAttributes").prop("disabled", false);
                        ClinicalLabTestAttributes.loadLabTest(response.labTestId);
                        $("#pnlClinicalLabTestAttributes #txtAttribute").val('');
                        $('#frmClinicalLabTestAttributes').bootstrapValidator('revalidateField', 'Attribute');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                $('#frmClinicalLabTestAttributes #txtLabCPTCode').val('');
                $('#frmClinicalLabTestAttributes #txtAttribute').val('');
                $('#frmClinicalLabTestAttributes').bootstrapValidator('revalidateField', 'Attribute');
                $('#frmClinicalLabTestAttributes').bootstrapValidator('revalidateField', 'LabCPTCode');
                utility.DisplayMessages("Test already exists.", 2);
            }
        }

        else if (ClinicalLabTestAttributes.params.mode == "Edit") {
            if (!isAttributeExists) {
                ClinicalLabTestAttributes.ClinicalLabTestEdit_DBCall().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //$('#' + ClinicalLabTestAttributes.params.PanelID + " #hfLabId").val(response.radiologicalOrderId);
                        utility.DisplayMessages(response.message, 1);
                        ClinicalLabTestAttributes.loadLabTest(response.labTestId);
                        $("#pnlClinicalLabTestAttributes #txtAttribute").val('');
                        $('#frmClinicalLabTestAttributes').bootstrapValidator('revalidateField', 'Attribute');
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            } else {
                utility.DisplayMessages("This Attribute already exists.", 2);
            }
        }

    },

    ClinicalLabTestSave_DBCall: function (myJSON) {

        var objData = JSON.parse(myJSON);
        objData["LabId"] = ClinicalLabTestAttributes.params.LabId;
        objData["commandType"] = "save_clinical_lab_test";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    ClinicalLabTestEdit_DBCall: function (myJSON) {

        var objData = new Object();
        objData["LabId"] = ClinicalLabTestAttributes.params.LabId;
        objData["LabTestId"] = ClinicalLabTestAttributes.params.LabTestId;
        if ($("#pnlClinicalLabTestAttributes #chkActive").is(':checked')) {
            objData["Active"] = "True";
        } else {
            objData["Active"] = "False";
        }
        if ($("#pnlClinicalLabTestAttributes #chkTemplate").is(':checked')) {
            objData["Template"] = "True";
        } else {
            objData["Template"] = "False";
        }
        objData["Description"] = $("#pnlClinicalLabTestAttributes #txtDescription").val();
        objData["Attribute"] = $("#pnlClinicalLabTestAttributes #txtAttribute").val();
        objData["LOINICCODE"] = $("#pnlClinicalLabTestAttributes #hfLLOINICCODE").val();
        objData["LOINICDescription"] = $("#pnlClinicalLabTestAttributes #hfLOINICDescription").val();

        objData["commandType"] = "edit_clinical_lab_test";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    ClinicalLabTestAttributeEdit_DBCall: function (obj, rowId) {

        var objData = new Object();
        objData["LabTestAttributeId"] = rowId;
        objData["LabTestId"] = ClinicalLabTestAttributes.params.LabTestId;

        if ($(obj).attr('id') == "txtUOM") {
            objData["UOM"] = $(obj).val();
            objData["isUOM"] = "True";
        } else {
            objData["Range"] = $(obj).val();
            objData["isUOM"] = "False";
        }



        objData["commandType"] = "updateuomrange";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    saveLabTestAttributes: function () {
        var isAttributeExists = false;

        if (ClinicalLabTestAttributes.AttributesList.length > 0) {
            $.each(ClinicalLabTestAttributes.AttributesList, function (i, l) {
                if (ClinicalLabTestAttributes.AttributesList[i].trim() == $("#pnlClinicalLabTestAttributes #txtAttribute").val().trim()) {
                    isAttributeExists = true;
                }
            });
        }

        if ($("#pnlClinicalLabTestAttributes #txtAttribute").val() != "") {
            if (!isAttributeExists) {
                ClinicalLabTestAttributes.saveLabTestAttributes_DBCall().done(function (response) {
                    var response = JSON.parse(response);
                    if (response.status != false) {
                        ClinicalLabTestAttributes.loadLabTest(ClinicalLabTestAttributes.params.LabId);
                        utility.DisplayMessages("Saved Successfully!", 1);
                        $("#pnlClinicalLabTestAttributes #txtAttribute").val("");
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }


                });
            } else {
                utility.DisplayMessages("This Attribute already exists.", 2);
            }
        } else {
            utility.DisplayMessages("Please Enter Attribute", 4);
        }


    },

    saveLabTestAttributes_DBCall: function () {

        var objData = new Object();
        objData["LabId"] = ClinicalLabTestAttributes.params.LabId;
        objData["LabTestId"] = ClinicalLabTestAttributes.params.LabTestId;
        objData["Attribute"] = $("#pnlClinicalLabTestAttributes #txtAttribute").val();
        objData["commandType"] = "save_clinical_lab_test_attribute";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");

    },

    deleteTestAttribute: function (attributeTestId) {

        utility.myConfirm('1', function () {
            ClinicalLabTestAttributes.deleteTestAttribute_DBCall(attributeTestId).done(function (response) {
                var result = JSON.parse(response);
                if (result.status != false) {

                    utility.DisplayMessages(result.Message, 1);
                    ClinicalLabTestAttributes.loadLabTest(ClinicalLabTestAttributes.params.LabId);

                }
                else {
                    utility.DisplayMessages(result.Message, 3);
                }

            });
        }, function () { },
                    '1'
                );



    },

    deleteTestAttribute_DBCall: function (attributeTestId) {
        var objData = new Object();
        objData["LabTestAttributeId"] = attributeTestId;
        objData["commandType"] = "delete_clinical_lab_test_attribute";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    changeTemplateChkBox: function (obj) {
        if ($("#pnlClinicalLabTestAttributes #chkTemplate").is(':checked')) {
            $("#pnlClinicalLabTestAttributes #divAttrTextBox").addClass("disableAll");
            $("#pnlClinicalLabTestAttributes #divDescription").css("display", "");
            //$('#frmClinicalLabTestAttributes').data('bootstrapValidator').enableFieldValidators('Description', true);
            $('#frmClinicalLabTestAttributes').data('bootstrapValidator').enableFieldValidators('Attribute', false);
        } else {
            $("#pnlClinicalLabTestAttributes #divAttrTextBox").removeClass("disableAll");
            $("#pnlClinicalLabTestAttributes #divDescription").css("display", "none");
            //$('#frmClinicalLabTestAttributes').data('bootstrapValidator').enableFieldValidators('Description', false);
            $('#frmClinicalLabTestAttributes').data('bootstrapValidator').enableFieldValidators('Attribute', true);
        }
    },

    bindAutoComplete: function (element) {

        //EMRUtility.BindLOINCCodes(element, "ClinicalLabTestAttributes");
        EMRUtility.BindLOINCCodes(element, "ClinicalLabTestAttributes", ClinicalLabTestAttributes.params.LabId, '', $("#pnlClinicalLabDetail #ddlCodeSystem").val());
    },

    pushLOINCAsCpt: function (JsonObj, $element) {

        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];
        $("#pnlClinicalLabTestAttributes #hfLLOINICCODE_AND_DESCRIPTION").val(LOINCCOde + " " + LOINCDescription);
        $("#pnlClinicalLabTestAttributes #hfLLOINICCODE").val(LOINCCOde);
        $("#pnlClinicalLabTestAttributes #hfLOINICDescription").val(LOINCDescription);
        $("#pnlClinicalLabTestAttributes #hfOBSERVATION").val(observation);


    },

    UnLoad: function () {
        //utility.UnLoadDialog("frmClinicalLabTestAttributes", function () {
        //    UnloadActionPan(ClinicalLabTestAttributes.params["ParentCtrl"], "ClinicalLabTestAttributes");

        //}, function () {
        //    UnloadActionPan(ClinicalLabTestAttributes.params["ParentCtrl"], "ClinicalLabTestAttributes");
        //});
        UnloadActionPan(ClinicalLabTestAttributes.params["ParentCtrl"], "ClinicalLabTestAttributes");
        //ClinicalLabDetail.loadLab(ClinicalLabTestAttributes.params.LabId);
        ClinicalLabDetail.loadClinicalLabTest();
    },

    showIcon: function (obj) {
        $(obj).find('a').css('display', '');
        //console.log("show");
    },

    hideIcon: function (obj) {
        if ($(obj).hasClass("active") == false) {
            $(obj).find('a').css('display', 'none');
        }
        //console.log("hide");
    },

    validateProcedure: function () {
        var cptcode = $('#pnlClinicalLabTestAttributes #hfOBSERVATION').val();
        //var cptdescp = $('#pnlClinicalLabTestAttributes #hfLOINICDescription').val();
        cptcode = (cptcode != null && cptcode != "") ? cptcode : "";
        if ($('#pnlClinicalLabTestAttributes #txtLabCPTCode').val() == "") {
            $('#pnlClinicalLabTestAttributes #hfLLOINICCODE').val('');
            $('#pnlClinicalLabTestAttributes #hfLOINICDescription').val('');
            $('#pnlClinicalLabTestAttributes #hfOBSERVATION').val('');
            return true;
        }
        else if (cptcode != $('#pnlClinicalLabTestAttributes #txtLabCPTCode').val().trim()) {
            utility.DisplayMessages("Invalid Test Information", 2);
            $('#pnlClinicalLabTestAttributes #txtLabCPTCode').val('');
            return false;
        }
        else
            return true;

        //var cptDescription = $('#pnlClinicalLabTestAttributes #hfLOINICDescription').val();
        //var cptCode = $('#pnlClinicalLabTestAttributes #hfOBSERVATION').val();
        //var $txtLabCPTCode = $('#pnlClinicalLabTestAttributes #txtLabCPTCode');
        //if ($txtLabCPTCode.val() == "") {
        //    return true;
        //}
        //if (cptCode == "") {
        //    if (cptDescription != $txtLabCPTCode.val()) {
        //        utility.DisplayMessages("Test not Valid", 2);
        //        $txtLabCPTCode.val('');
        //        return false;
        //    }
        //}
        //else if (cptCode + " - " + cptDescription != $txtLabCPTCode.val()) {
        //    utility.DisplayMessages("Test not Valid", 2);
        //    $txtLabCPTCode.val('');
        //    return false;
        //}
        //else
        //    return true;

    },

    onChangeInActive: function () {
        if (ClinicalLabTestAttributes.params.mode == "Edit") {
            ClinicalLabTestAttributes.onChangeInActive_DBCall().done(function (response) {

                var data = JSON.parse(response);

                if (data.status != false) {
                    utility.DisplayMessages("Successfully Updated", 1);
                } else {

                }

            });
        }
    },

    onChangeInActive_DBCall: function () {

        var objData = new Object();
        objData["LabTestId"] = ClinicalLabTestAttributes.params.LabTestId;
        if ($("#pnlClinicalLabTestAttributes #chkActive").is(':checked')) {
            objData["Active"] = "True";
        } else {
            objData["Active"] = "False";
        }
        objData["commandType"] = "update_active_inactive";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },


    BindLOINCCodes: function () {
        $("#pnlClinicalLabTestAttributes #txtAttribute").autocomplete({
            autoFocus: true,
            source: function (request, response) {
                // utility.Keyupdelay(function () {
                var AccountNo = $('#pnlClinicalLabTestAttributes #txtAttribute').val();
                if (AccountNo.length > 0) {
                    Clinical_LOINC.loadLabResultsLOINC(null, AccountNo).done(function (responseData) {
                        responseData = JSON.parse(responseData)
                        if (responseData.status != false) {
                            if (responseData.LabResultLOINCCount > 0) {
                                var LabResultLOINCLoadJSONData = JSON.parse(responseData.LabResultLOINCLoad_JSON);
                                var AllLOINC = [];
                                $.each(LabResultLOINCLoadJSONData, function (i, item) {

                                    AllLOINC.push({ id: item.LOINCCode, value: item.LOINCCode + " " + item.LOINCDescription, LOINCDescription: item.LOINCDescription, LabId: item.LabId, UoM: item.UoM, Range: item.Range, IsActive: item.IsActive, SampleStorage: item.SampleStorage });


                                });
                                response(AllLOINC);
                            }
                        }
                    });
                }

                //});
            },

            select: function (event, ui) {
                var obj = {};
                obj["Observation"] = ui.item.value;
                obj['LOINICCODE'] = ui.item.id;
                obj['LOINICDescription'] = ui.item.LOINCDescription;

                //$('#pnlClinicalLabTestAttributes #txtAttribute').val(ui.item.value.substr(1, 19))

            }
        });
    },

}