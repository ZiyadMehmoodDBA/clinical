ClinicalLabDetail = {
    params: [],
    bIsFirstLoad: true,
    arrLabs:[],
    EditableGrid: null,

    Load: function (params) {
        BackgroundLoaderShow(true);
        params["TabID"] = 'ClinicalLabDetail';
        ClinicalLabDetail.params = params;

        var labId = ClinicalLabDetail.params.LabId;
        $('#' + ClinicalLabDetail.params.PanelID + " #hfLabId").val(labId);
        var self = $('#pnlClinicalLabDetail');

        var PanelLabGrid = "#" + ClinicalLabDetail.params.PanelID + " #pnlLab_Result";
        var LabGridId = "#" + ClinicalLabDetail.params.PanelID + " #dgvLab";
        //$(LabGridId + " tbody tr").remove();

        if (ClinicalLabDetail.bIsFirstLoad == true) {
            ClinicalLabDetail.bIsFirstLoad = false;

            //if (ClinicalLabDetail.params.mode == "Edit") {
            //    ClinicalLabDetail.loadLab(labId);
            //}

            ClinicalLabDetail.validateLabDetail();

            $('#ddlCategory').on('change', function () {
                if ($('#ddlCategory option:selected').val() == '1') {
                    $('#clientNo').hide();
                    var bootstrapValidator = $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").data('bootstrapValidator');
                    bootstrapValidator.enableFieldValidators('ClientNo', false);
                }
                else {
                    $('#clientNo').show();
                    var bootstrapValidator = $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").data('bootstrapValidator');
                    bootstrapValidator.enableFieldValidators('ClientNo', true);
                }

            });



            self.loadDropDowns(true).done(function () {

                //Start//24-05-2016//Ahmad Raza//Entity Selection Logic
                if (globalAppdata["AppUserName"].toLowerCase() == "mdvision") {

                    $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail #entityList").removeClass("hidden");
                }
                else {
                    $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail #entityList").addClass("hidden");
                    $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                //End//24-05-2016//Ahmad Raza//Entity Selection Logic

                if (ClinicalLabDetail.params.mode == "Edit") {
                    $('#' + ClinicalLabDetail.params.PanelID + " #divResultAttributes").css("display", "");

                    ClinicalLabDetail.loadLab(labId);

                } else {
                    $('#' + ClinicalLabDetail.params.PanelID + " #divResultAttributes").css("display", "");
                    $('#' + ClinicalLabDetail.params.PanelID + " #divResultAttributes").addClass('disableAll');
                }
                if (ClinicalLabDetail.params.mode == "Add") {
                    $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").data('serialize', $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").serialize());
                }
            });
        }

        ClinicalLabDetail.domReadyFunction();
    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Edit Lab
    LabEdit: function (LabId, event) {
        //if icon is clicked then  popup the window

        Clinical_Lab.LabAddEdit(LabId);
        event.stopPropagation();

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to apply bootstrap validations 
    validateLabDetail: function () {


        $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   LabId: {
                       group: '.col-md-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LabName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ClientNo: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LabType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Entity: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Category: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CodeSystem: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   RequisitionTemplate: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Type: {
                       group: '.col-sm-3',
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
            ClinicalLabDetail.LabSave('save');
            e.type = "";
        });

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    domReadyFunction: function () {
        $(document).ready(function () {
            (function ($) {
                'use strict';
                $(function () {
                    $('#pnlClinicalLabDetail [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });


    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to show delete icon on hover
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to hide delete icon on mouse leave
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    UnLoad: function (caller) {
        var saveButtonisHidden = $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail #btnSaveOrder").hasClass("hidden");

        if (caller == 'saveExit' || saveButtonisHidden == true) {
            UnloadActionPan(ClinicalLabDetail.params["ParentCtrl"], "ClinicalLabDetail");
        }
        else {
            utility.UnLoadDialog("frmClinicalLabDetail", function () {
                UnloadActionPan(ClinicalLabDetail.params["ParentCtrl"], "ClinicalLabDetail", null, ClinicalLabDetail.params["PanelID"]);

            }, function () {
                UnloadActionPan(ClinicalLabDetail.params["ParentCtrl"], "ClinicalLabDetail");
            });
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves Lab    
    LabSave: function (sender) {

        var LabId = $('#' + ClinicalLabDetail.params.PanelID + " #hfLabId").val() != "" ? $('#' + ClinicalLabDetail.params.PanelID + " #hfLabId").val() : "-1";
        if (parseInt(LabId) > 0) {
            ClinicalLabDetail.params.mode = "Edit";
        }
        else {
            ClinicalLabDetail.params.mode = "Add";
        }

        var self = $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail");

        if (true) {

            var mainErrorMessage = "";

            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            //Ahsan Nasir

            objData["CategoryId"] = objData["Category"];
            objData["CodeSystemId"] = objData["CodeSystem"];
            objData["RequisitionTemplateId"] = objData["RequisitionTemplate"];
            objData["LabTypeId"] = objData["Type"];

            myJSON = JSON.stringify(objData);
            JSONToSave = myJSON;
            //--------------------------------------------------------------
            if (ClinicalLabDetail.params.mode == "Add") {

                AppPrivileges.GetFormPrivileges("Laboratory", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalLabDetail.saveLab(JSONToSave).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $('#' + ClinicalLabDetail.params.PanelID + " #hfLabId").val(response.radiologicalOrderId);

                                utility.DisplayMessages(response.message, 1);

                                Clinical_Lab.LabSearch();

                                if (sender == 'signprintorder') {
                                    ClinicalLabDetail.printLab();
                                }
                                else {
                                    ClinicalLabDetail.UnLoad('saveExit');
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
            else if (ClinicalLabDetail.params.mode == "Edit") {

                AppPrivileges.GetFormPrivileges("Laboratory", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalLabDetail.updateLab(JSONToSave, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $('#' + ClinicalLabDetail.params.PanelID + " #hfLabId").val(response.radiologicalOrderId);

                                utility.DisplayMessages(response.message, 1);
                                Clinical_Lab.LabSearch();

                                if (sender == 'signprintorder') {
                                    ClinicalLabDetail.printLab();
                                }
                                else {

                                    ClinicalLabDetail.UnLoad('saveExit');

                                }


                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });

            }

        }
        else {
            //      utility.DisplayMessages("Title is required.", 3);
        }
    },



    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves Lab 
    //Params: LabData
    saveLab: function (LabData) {
        var objData = JSON.parse(LabData);

        objData["commandType"] = "save_ClinicalLab";
        // objData["IsActive"] = objData.Active;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLab");
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To show Lab Search in Popup 
    openLabAlert: function () {

        if ($(" #mainForm  li#LabAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
            BackgroundLoaderShow(true);
            var params = [];


            params["FromAdmin"] = 0;
            //   params["StartupScreen"] = "message";
            LoadActionPan("Clinical_Lab", params);
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads Lab data 
    loadLab: function (LabId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Laboratory", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $('#' + ClinicalLabDetail.params.PanelID + " #pnlClinicalLabDetail #hfLabId").val(LabId);
                var self = $('#' + ClinicalLabDetail.params.PanelID + " #pnlClinicalLabDetail");
                // var data = self.getMyJSONByName();
                ClinicalLabDetail.fillLab(LabId).done(function (response) {
                    if (response != "") {
                        var data = JSON.parse(response);
                        if (data.status != false) {
                            var LabDetail = JSON.parse(data.ClinicalLab_JSON);
                            var self = $('#' + ClinicalLabDetail.params.PanelID + " #pnlClinicalLabDetail");
                            LabDetail = LabDetail[0];
                            utility.bindMyJSONByName(true, LabDetail, false, self).done(function () {

                                self.find('#txtLabName').val(LabDetail.Name);
                                self.find('#txtLabType').val(LabDetail.Type);
                                self.find('#ddlEntity').val(LabDetail.EntityId);//kr
                                //self.find('#txtClientNo').val(LabDetail.ClientNo);
                                self.find('#ddlCategory').val(LabDetail.CategoryId);
                                self.find('#ddlCategory').attr('disabled', 'true'); // Category cannot be changed once added against a lab
                                self.find('#ddlType').val(LabDetail.LabTypeId);
                                self.find('#ddlRequisitionTemplate').val(LabDetail.RequisitionTemplateId);
                                self.find('#ddlCodeSystem').val(LabDetail.CodeSystemId);


                                //if ($('#ddlCategory option:selected').val() == '1') {
                                //    $('#clientNo').hide();
                                //    var bootstrapValidator = $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").data('bootstrapValidator');
                                //    bootstrapValidator.enableFieldValidators('ClientNo', false);
                                //}
                                //else {
                                //    $('#clientNo').show();
                                //    var bootstrapValidator = $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").data('bootstrapValidator');
                                //    bootstrapValidator.enableFieldValidators('ClientNo', true);
                                //}
                            });
                            $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").data('serialize', $('#' + ClinicalLabDetail.params.PanelID + " #frmClinicalLabDetail").serialize());
                            //if (data.ClinicalLabTestCount > 0) {
                            var LabTestDetail = JSON.parse(data.ClinicalLabTest_JSON);
                            ClinicalLabDetail.loadLabTests(LabTestDetail);
                            //}

                        }
                    }
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Fills Lab 
    //Params: LabId
    fillLab: function (LabId) {

        var objData = {};
        objData["commandType"] = "load_ClinicalLab";
        objData["LabId"] = LabId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLab");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Updates Lab 
    //Params: LabData, LabId
    updateLab: function (LabData, LabId) {

        var objData = JSON.parse(LabData);
        objData["LabId"] = LabId;
        objData["commandType"] = "save_ClinicalLab";

        //  objData["IsActive"] = objData.Active;

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLab");

    },


    // Start Result Attributes Functions

    AddResultAttributes: function () {
        ClinicalLabDetail.arrLabs = [];
        $("#ulLabTest li").each(function (i, li) {
            var tstName = $(li).text();
            ClinicalLabDetail.arrLabs.push(tstName);
        });
        var params = [];
        params["LabId"] = ClinicalLabDetail.params.LabId;
        params["mode"] = "Add";
        params["ParentCtrl"] = "ClinicalLabDetail";
        params["LabNames"] = ClinicalLabDetail.arrLabs;
        LoadActionPan('ClinicalLabTestAttributes', params);

    },

    loadLabTests: function (result) {

        Crtl = '#pnlClinicalLabDetail div#divResultAttributes #ulLabTest';
        l = $(Crtl);

        l.empty();


        var isFirstLi = true;
        if (result.length > 0) {
            $.each(result, function (j, item) {
                var li = "";
                li = "<li  id=\"" + item.LabTestId + "\" onclick='ClinicalLabDetail.selectLabTestAttributes(this, event, " + item.LabTestId + ");' onmouseover='ClinicalLabDetail.showIcon(this);' onmouseout='ClinicalLabDetail.hideIcon(this);' ><a href='#'>" + item.LOINC + " " + item.LOINCDescription + "<span  class='removeIconListHover' onclick='ClinicalLabDetail.editLabTest($($(this).parent()).parent(), event, " + item.LabTestId + ");'><i class='fa fa-edit'></i></span><span  class='removeIconListHover' onclick='ClinicalLabDetail.deleteLabTest($($(this).parent()).parent(), event, " + item.LabTestId + ");'><i class='fa fa-times'></i></span></a></li>"
                l.append(li);
            });
            $('#pnlClinicalLabDetail div#divResultAttributes #ulLabTest li:first').trigger('click');
        } else {
            $("#pnlClinicalLabDetail #dgvLabTestAttributes").dataTable().fnDestroy();
            $("#pnlClinicalLabDetail #dgvLabTestAttributes tbody").find("tr").remove();

            $('#pnlClinicalLabDetail #dgvLabTestAttributes').DataTable({
                "language": {
                    "emptyTable": "No Attribute Found."
                }, "autoWidth": false, "bLengthChange": false, "searching": false, "bPaginate": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            if ($.fn.dataTable.isDataTable('#pnlClinicalLabDetail #dgvLabTestAttributes'))
                ;
            else
                $("#pnlClinicalLabDetail #dgvLabTestAttributes").DataTable({ "bLengthChange": false, "bInfo": false, "bFilter": false, "bPaginate": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        $("#pnlClinicalLabDetail #pnlLabTestAttributes_Result").find('.row').remove();
    },

    selectLabTestAttributes: function (obj, ev, LabTestId) {
        ev.stopPropagation();
        $("#pnlClinicalLabDetail div#divResultAttributes #ulLabTest li").each(function (i, item) {
            $(this).removeClass("active");
            $(this).find('div').css('display', 'none');
        });
        $(obj).addClass("active");
        $(obj).find('div').css('display', '');

        ClinicalLabDetail.labTestAttributesLoad_DBCall(LabTestId).done(function (response) {
            if (response != "") {
                var data = JSON.parse(response);
                if (data.status != false) {

                    ClinicalLabDetail.LabTestAttributesGridLoad(data);

                } else {

                }
            }
        });
    },
    LabTestAttributesGridLoad: function (response) {
        $("#pnlClinicalLabDetail #dgvLabTestAttributes").dataTable().fnDestroy();
        $("#pnlClinicalLabDetail #dgvLabTestAttributes tbody").find("tr").remove();
        if (response.ClinicalLabTestAttributeCount > 0) {
            var ClinicalLabTestAttributeLoadJSONData = JSON.parse(response.ClinicalLabTestAttribute_JSON);
            var ClinicalLabTestAttributeResultJSONData = JSON.parse(response.ClinicalLabTestAttributeResult_JSON);
            if (ClinicalLabTestAttributeLoadJSONData[0].AttributeName != "") {
                $.each(ClinicalLabTestAttributeLoadJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("AttributeId", item.LabTestAttributeId);
                    if (response.ClinicalLabTestAttributeResultCount > 0) {
                        var resultColVal = '';
                        var resultArr = [];
                        $.each(ClinicalLabTestAttributeResultJSONData, function (i, res) {
                            if (res.LabTestAttributeId == item.LabTestAttributeId) {
                                resultArr.push(res.ResultName);
                            }
                        });
                        resultColVal = resultArr.join(', ');
                    }

                    $row.append('<td style="display:none;"> ' + item.LabTestAttributeId + '</td><td>' + item.AttributeName + '</td><td>' + item.UoM + '</td><td>' + item.Range + '</td><td>' + ((resultColVal) ? resultColVal : "") + '</td>');
                    $("#pnlClinicalLabDetail #dgvLabTestAttributes tbody").last().append($row);
                });
            }
            else {
                $('#pnlClinicalLabDetail #dgvLabTestAttributes').DataTable({
                    "language": {
                        "emptyTable": "No Attribute Found."
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
        }
        else {
            $('#pnlClinicalLabDetail #dgvLabTestAttributes').DataTable({
                "language": {
                    "emptyTable": "No Attribute Found."
                }, "autoWidth": false, "bLengthChange": false, "searching": false, "bPaginate": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#pnlClinicalLabDetail #dgvLabTestAttributes'))
            ;
        else
            $("#pnlClinicalLabDetail #dgvLabTestAttributes").DataTable({ "bLengthChange": false, "bInfo": false, "bFilter": false, "bPaginate": false, "order": [[0, "asc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [2] }] }); // to remove records per page dropdown


        $("#pnlClinicalLabDetail #pnlLabTestAttributes_Result").find('.row').remove();
    },
    labTestAttributesLoad_DBCall: function (LabTestId) {
        var objData = new Object();
        objData["LabTestId"] = LabTestId;
        objData["commandType"] = "load_clinical_lab_test_attributes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    editLabTest: function (obj, ev, LabTestId) {

        ev.stopPropagation();

        var params = [];
        params["LabId"] = ClinicalLabDetail.params.LabId;
        params["LabTestId"] = LabTestId;
        params["mode"] = "Edit";
        params["ParentCtrl"] = "ClinicalLabDetail";
        LoadActionPan('ClinicalLabTestAttributes', params);


    },


    deleteLabTest: function (obj, ev, LabTestId) {
        ev.stopPropagation();
        utility.myConfirm('1', function () {
            ClinicalLabDetail.deleteLabTest_DBCall(LabTestId).done(function (response) {
                var result = JSON.parse(response);
                if (result.status != false) {
                    utility.DisplayMessages(result.Message, 1);
                    ClinicalLabDetail.loadClinicalLabTest();
                }
                else {
                    utility.DisplayMessages(result.Message, 3);
                }

            });
        }, function () { },
                    '1'
                );



    },

    deleteLabTest_DBCall: function (LabTestId) {
        var objData = new Object();
        objData["LabTestId"] = LabTestId;
        objData["commandType"] = "delete_clinical_lab_test";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    showIcon: function (obj) {
        $(obj).find('div').css('display', '');
    },

    hideIcon: function (obj) {
        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }
    },

    reloadTestsOnSwitch: function (objThis) {
        var Status = $(objThis).attr('isActive');
        if (Status == '1') {
            $(objThis).attr('isActive', '0');
        }
        else if (Status == '0') {
            $(objThis).attr('isActive', '1');
        }
        ClinicalLabDetail.loadClinicalLabTest();
        //DashBoard.DashBoardAppointmentSearch(null, null, null, null);
    },

    loadClinicalLabTest: function () {

        ClinicalLabDetail.loadLabTests_DBCall().done(function (response) {

            var data = JSON.parse(response);
            var LabTestDetail = JSON.parse(data.ClinicalLabTest_JSON);
            ClinicalLabDetail.loadLabTests(LabTestDetail);

        });

    },

    loadLabTests_DBCall: function () {
        var IsCheckedIn = null;
        IsCheckedIn = $('#pnlClinicalLabDetail #switchActive').attr('isActive');
        var objData = new Object();
        objData["commandType"] = "load_clinical_lab_test";
        objData["LabId"] = ClinicalLabDetail.params.LabId;
        objData["IsActive"] = IsCheckedIn;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");

    },
    // End Result Attributes /functions
}