insurancePlanDetail = {
    params: [],
    Enable: true,
    IsSplitInsurancePlan: false,
    Load: function (params) {
        insurancePlanDetail.params = params;

        //Bind Modifier Field
        utility.BindModifierField($('#insurancePlanDetail #txtModifier'));

        var self = $('#insurancePlanDetail');
        self.loadDropDowns(true).done(function () {

            insurancePlanDetail.LoadInsurancePlan();
        });
        insurancePlanDetail.LoadAllAutocomplete();
        $("#addressDetailGrid #tblAddressDetail #txtAddInfoWebPortal").change(function () {
            if ((this.value).match(/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/)) {
                if (this.value != '' && !/^(http|https):\/\//.test(this.value)) {
                    this.value = "http://" + this.value;
                }
            }
        });
    },
    FillEDISubmitInsurance: function (submitID, clearinghouse, submitName, payorid) {

        $("#insurancePlanDetail #ddlEDISubmitInsurance").val(clearinghouse + ' - ' + submitName);
        $("#insurancePlanDetail #hfEDIInsurance").val(submitID);
        $("#insurancePlanDetail #txtClaimPayorId").val(payorid);
        $("#insurancePlanDetail #ddlEDISubmitInsurance").focus();
        $('#insurancePlanDetail #frmInsurancePlanDetail').bootstrapValidator('revalidateField', "EDISubmitInsurance");
        Admin_EDISubmitInsurance.UnLoadTab();
    },
    FILLEDIEligibilityInsurance: function (submitID, submitName) {

        $("#insurancePlanDetail #ddlEDIEligibilityInsurance").val(submitName);
        $("#insurancePlanDetail #hfEDIEligibilityInsurance").val(submitID);
        $("#insurancePlanDetail #ddlEDIEligibilityInsurance").focus();
        Admin_EDIEligibilityInsurance.UnLoadTab();
    },
    //adnan maqbool, PMS-3918 ,23-02-2016
    
    RevalidateField: function (obj, Field) {
        if ($(obj).is(':checked') &&  insurancePlanDetail.IsSplitInsurancePlan == true) {
            $('#' + insurancePlanDetail.params.PanelID + ' #tblinsurancePlanDetail #frmInsurancePlanDetail').bootstrapValidator('revalidateField', Field);
        }
    },
    //end
    LoadInsurancePlan: function () {
        $("#insurancePlanDetail #pnlAddressInfo").removeClass('disableAll');
        if (insurancePlanDetail.params.mode == "Add") {
            $('#insurancePlanDetail #txtShortName').attr("enabled", "enabled");

            $('#insurancePlanDetail #pnlAddressInfo').addClass("disableAll");
            insurancePlanDetail.ValidateInsurancePlan();
            insurancePlanDetail.ValidateInsurancePlanAddress();
            $('#insurancePlanDetail #ddlClaimType option').filter(function () { return $(this).val() == '1'; }).prop("selected", true);
            //Serialize Data
            $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').data('serialize', $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').serialize());
        }
        else if (insurancePlanDetail.params.mode == "Edit") {
            $('#insurancePlanDetail #txtShortName').attr("disabled", "disabled");
            insurancePlanDetail.FillInsurancePlan(insurancePlanDetail.params.InsurancePlanId).done(function (response) {
                if (response.status != false) {
                    var insurancePlan_detail = JSON.parse(response.InsurancePlanFill_JSON);
                    var self = $("#insurancePlanDetail");
                    utility.bindMyJSON(true, insurancePlan_detail, false, self).done(function () {

                        if (insurancePlan_detail.chkActive == 'True')
                            $("#insurancePlanDetail #chkActive").attr("checked", true);
                        else
                            $("#insurancePlanDetail #chkActive").attr("checked", false);
                        if ($("#insurancePlanDetail #ddlEDISubmitInsurance").val() != "") {
                            $("#insurancePlanDetail #ddlEDISubmitInsurance").focus();
                        }
                        if ($("#insurancePlanDetail #ddlEDIEligibilityInsurance").val() != "") {
                            $("#insurancePlanDetail #ddlEDIEligibilityInsurance").focus();
                        }

                        $("#insurancePlanDetail #chkCapitatedPlan").focus();


                        if (insurancePlan_detail.chkCapitatedPlan == 'True')
                            $("#insurancePlanDetail #chkCapitatedPlan").attr("checked", true);
                        else
                            $("#insurancePlanDetail #chkCapitatedPlan").attr("checked", false);
                        if (insurancePlan_detail.chkElectronicSubmit == 'True')
                            $("#insurancePlanDetail #chkElectronicSubmit").attr("checked", true);
                        else
                            $("#insurancePlanDetail #chkElectronicSubmit").attr("checked", false);
                        if (insurancePlan_detail.chkCapitatedAutoWriteoff == 'True')
                            $("#insurancePlanDetail #chkCapitatedAutoWriteoff").attr("checked", true);
                        else
                            $("#insurancePlanDetail #chkCapitatedAutoWriteoff").attr("checked", false);
                        if (insurancePlan_detail.chkManagedCare == 'True')
                            $("#insurancePlanDetail #chkManagedCare").attr("checked", true);
                        else
                            $("#insurancePlanDetail #chkManagedCare").attr("checked", false);

                        $("#insurancePlanDetail #pnlAddressInfo").removeClass('disableAll');
                        insurancePlanDetail.ValidateInsurancePlan();
                        insurancePlanDetail.ValidateInsurancePlanAddress();
                        if ($("#insurancePlanDetail #ddlEDISubmitInsurance").val()) {
                            if ($("#insurancePlanDetail #lnkInsuranceEDI").css("display") == "none") {
                                $("#insurancePlanDetail #lnkInsuranceEDI").css("display", "inline");
                                $("#insurancePlanDetail #lblInsuranceEDI").css("display", "none");
                            }
                        }
                        insurancePlanDetail.LoadInsurancePlanAddress().done(function (response) {
                            if (response.status != false) {
                                insurancePlanDetail.InsurancePlanAddressGridLoad(response);

                                //Serialize Data
                                $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').data('serialize', $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').serialize());
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });

                        insurancePlanDetail.ElectronicSubmit($("#frmInsurancePlanDetail #chkElectronicSubmit"));
                        insurancePlanDetail.SplitInsurancePlan($("#frmInsurancePlanDetail #chkSplit"));

                        //Serialize Data
                        $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').data('serialize', $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').serialize());
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateInsurancePlan: function () {
        $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail')
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
                       group: '.col-sm-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Insurance: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PlanCategory: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PlanType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ClaimFlag: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ClaimType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   EDISubmitInsurance: {
                       group: '.col-md-3',
                       enabled: insurancePlanDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ICDTypeInsurancePlan: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
     

                   SplitInsurancePlan: {
                       group: '.col-sm-4',
                       enabled: insurancePlanDetail.IsSplitInsurancePlan,
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
            insurancePlanDetail.InsurancePlanSave();
        });
    },

    enableemailvalidation: function (obj) {
        var objdirect = $('#insurancePlanDetail #frmAddressInfo');
        var formValidation = objdirect.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        }
        else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }
    },

    SplitInsurancePlan: function (obj) {
        if ($(obj).is(':checked')) {
            insurancePlanDetail.IsSplitInsurancePlan = true;
            $("#insurancePlanDetail #span_required").css('display', 'inline-block');
        }
        else {
            insurancePlanDetail.IsSplitInsurancePlan = false;
            $("#insurancePlanDetail #span_required").css('display', 'none');
        }

        var bootstrapValidator = $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').data('bootstrapValidator');
        bootstrapValidator.enableFieldValidators('SplitInsurancePlan', insurancePlanDetail.IsSplitInsurancePlan);

    },

    GetPayorId: function (obj) {
        var val_ = $('#insurancePlanDetail #ddlEDISubmitInsurance option:selected').attr("refvalue");
        $("#insurancePlanDetail #txtClaimPayorId").val(val_);
    },

    ElectronicSubmit: function (obj) {
        if ($(obj).is(':checked')) {
            $("#frmInsurancePlanDetail #SubmitInsurance_span").css('display', 'inline-block');
            insurancePlanDetail.Enable = true;
        }
        else {
            $("#frmInsurancePlanDetail #SubmitInsurance_span").css('display', 'none');
            insurancePlanDetail.Enable = false;
        }


        var bootstrapValidator = $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').data('bootstrapValidator');
        bootstrapValidator.enableFieldValidators('EDISubmitInsurance', insurancePlanDetail.Enable);
    },

    OpenInsurancePlan: function () {
        var params = [];
        var PanelID = null;
        params["ParentCtrl"] = 'insurancePlanDetail';
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        LoadActionPan('Admin_InsurancePlan', params);
    },

    //Adnan Maqbool Dated Jan 14,2015 PMS-3345
    OpenEDISubmitInsurance: function () {
        var params = [];
        params["ParentCtrl"] = 'insurancePlanDetail';
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "ddlEDISubmitInsurance";
        LoadActionPan('Admin_EDISubmitInsurance', params);
    },

    OpenEDIEligibilityInsurance: function () {
        var params = [];
        params["ParentCtrl"] = 'insurancePlanDetail';
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "ddlEDIEligibilityInsurance";
        LoadActionPan('Admin_EDIEligibilityInsurance', params);
    },

    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetEDISubmitInsurance', false).done(function (result) {
            var Ctrl = $("#" + insurancePlanDetail.params["PanelID"] + " input#ddlEDISubmitInsurance");
            var hfCtrl = $("#insurancePlanDetail #hfEDIInsurance");
            var onSelect = function (e) { $("#insurancePlanDetail #txtClaimPayorId").val(e.payorId); };
            var onChange = function (valid) { if (!valid) $("#insurancePlanDetail #txtClaimPayorId").val('') };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", EDISubmitInsurance, null, hfCtrl, onSelect, onChange);
        });
        CacheManager.BindCodes('GetEDIEligibilityInsurance', false).done(function (result) {
            var Ctrl = $("#" + insurancePlanDetail.params["PanelID"] + " input#ddlEDIEligibilityInsurance");
            var hfCtrl = $("#insurancePlanDetail #hfEDIEligibilityInsurance");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", EligibilityInsurance, null, hfCtrl);
        });

        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            var Ctrl = $("#" + insurancePlanDetail.params["PanelID"] + " input#txtSplitInsurancePlan");
            var hfCtrl = $("#insurancePlanDetail #hfSplitInsurancePlanId");
            var onChange = function () { insurancePlanDetail.RevalidateField('#chkSplit', 'SplitInsurancePlan') };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl, null, onChange);
        });
    },
    OpenEDISubmitInsuranceDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#pnlPatientInsurance #hfInsurancePlan").val());
        var params = [];
        params["ParentCtrl"] = 'insurancePlanDetail';
        params["EDISubmitInsuranceId"] = $("#insurancePlanDetail #hfEDIInsurance").val();
        params["mode"] = "Edit";

        params["FromAdmin"] == "0";
        params["RefCtrl"] = "lnkInsuranceEDI";
        LoadActionPan('EDISubmitInsuranceDetail', params);
    },

    OpenEDIEligibilityInsuranceDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#pnlPatientInsurance #hfInsurancePlan").val());
        var params = [];
        params["ParentCtrl"] = 'insurancePlanDetail';
        params["EDIEligibilityInsuranceId"] = $("#insurancePlanDetail #hfEDIEligibilityInsurance").val();
        params["mode"] = "Edit";

        params["FromAdmin"] == "0";
        params["RefCtrl"] = "lnkEligibilityInsuranceEDI";
        LoadActionPan('EDIEligibilityInsuranceDetail', params);
    },
    //end Dated Jan 14,2015 PMS-3345
    InsurancePlanSave: function () {
        $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').data('serialize', $('#insurancePlanDetail #tblinsurancePlanDetail #frmInsurancePlanDetail').serialize());
        var strMessage = "";
        var self = $("#insurancePlanDetail");
        var myJSON = self.getMyJSON();
        if (insurancePlanDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Insurance Plan", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (insurancePlanDetail.params.InsurancePlanId == "-1") {
                        insurancePlanDetail.SaveInsurancePlan(myJSON).done(function (response) {
                            if (response.status != false) {
                                var chkRequired= $("#chkRefRequired").prop("checked");
                                Patient_Insurance.SetReferralRequired(chkRequired);
                                $("#insurancePlanDetail #pnlAddressInfo").removeClass('disableAll');
                                Admin_InsurancePlan.InsurancePlanSearch(response.InsurancePlanId);
                                insurancePlanDetail.params.InsurancePlanId = response.InsurancePlanId;
                                utility.DisplayMessages(response.message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (insurancePlanDetail.params.InsurancePlanId != "-1" && insurancePlanDetail.params.InsurancePlanId != "" && insurancePlanDetail.params.InsurancePlanId != "0") {
                        insurancePlanDetail.UpdateInsurancePlan(myJSON, insurancePlanDetail.params.InsurancePlanId).done(function (response) {
                            if (response.status != false) {
                                Admin_InsurancePlan.InsurancePlanSearch(insurancePlanDetail.params.InsurancePlanId);
                                utility.DisplayMessages(response.message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    CacheManager.BindCodes('GetInsurancePlan', true);
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (insurancePlanDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Insurance Plan", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    insurancePlanDetail.UpdateInsurancePlan(myJSON, insurancePlanDetail.params.InsurancePlanId).done(function (response) {
                        if (response.status != false) {
                            var chkRequired = $("#chkRefRequired").prop("checked");
                            Patient_Insurance.SetReferralRequired(chkRequired);
                            if (insurancePlanDetail.params.ParentCtrl == 'Admin_InsurancePlan')
                                Admin_InsurancePlan.InsurancePlanSearch(insurancePlanDetail.params.InsurancePlanId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetInsurancePlan', true);

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

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {

        $("#" + insurancePlanDetail.params.PanelID + " #txtSplitInsurancePlan").val(InsurancePlanName);
        $("#" + insurancePlanDetail.params.PanelID + " #hfSplitInsurancePlanId").val(InsurancePlanId);
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
    },

    // ------------ Insurance Plan Address Region (Detail Grid)
    InsurancePlanAddressGridLoad: function (response) {
        $("#dgvAddressInfo").dataTable().fnDestroy();
        $("#insurancePlanDetail #dgvAddressInfo tbody").find("tr").remove();
        if (response.InsurancePlanAddressCount > 0) {
            var InsurancePlanAddressJSON = JSON.parse(response.InsurancePlanAddress_JSON);
            $.each(InsurancePlanAddressJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvInsurancePlanAddress_row" + item.InsurancePlanAddressId + "'))");
                $row.attr("id", "gvInsurancePlanAddress_row" + item.InsurancePlanAddressId);
                $row.attr("InsurancePlanAddressId", item.InsurancePlanAddressId);

                $row.append('<td style="display:none;">' + item.InsurancePlanAddressId + '</td><td><a class="btn  btn-xs" href="#" onclick="insurancePlanDetail.InsurancePlanAddressDelete(' + item.InsurancePlanAddressId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="insurancePlanDetail.InsurancePlanAddressEdit(' + item.InsurancePlanAddressId + ');"  title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + item.Address + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.ZipCode + '</td><td>' + item.PhoneNo + '</td>');

                $("#insurancePlanDetail #dgvAddressInfo tbody").last().append($row);
            });
        }
        else {
            $('#dgvAddressInfo').DataTable({
                "language": {
                    "emptyTable": "No Plan Address Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvAddressInfo'))
            ;
        else
            $("#insurancePlanDetail #dgvAddressInfo").DataTable({ "bLengthChange": false, "autoWidth": false }); // to remove records per page dropdown
        if (insurancePlanDetail.params.ParentCtrl == "patTabInsurance" || insurancePlanDetail.params.ParentCtrl == "Patient_Insurance") {
            var self = $("#" + Patient_Insurance.params["PanelID"] + ' #frmPatientInsurance #containerPlanAddress');
            CacheManager.BindDropDownsByID('#pnlPatientInsurance #frmPatientInsurance #ddlPlanAddress', 'GetInsurancePlanAddress', true, insurancePlanDetail.params.InsurancePlanId).done(function () {
                // Start 27/01/2016 Muhammad Irfan for bug #  PMS-3441
                $('#pnlPatientInsurance #frmPatientInsurance #ddlPlanAddress').val(insurancePlanDetail.params.PlanAddressId);
                // End 27/01/2016 Muhammad Irfan for bug #  PMS-3441
            });
        }

    },

    InsurancePlanAddressAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Insurance Plan", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                insurancePlanDetail.params.InsurancePlanAddressId = null;
                $('#addressDetailGrid').resetAllControls();
                setTimeout(function () { $('#addressDetailGrid #frmAddressInfo').data('bootstrapValidator').resetForm(); }, 300);

                insurancePlanDetail.params.PlanMode = "Add";
                $('#addressDetailGrid').modal({
                    show: 'true',
                    backdrop: 'static',
                    keyboard: false
                });
                //$("#frmAddressInfo").trigger('reset'); //$('#addressDetailGrid').find('#insurancePlanDetail #tblAddressDetail #frmAddressInfo')[0].reset(); //$("#frmAddressInfo")[0].reset();

                //Serialize Data
                $('#insurancePlanDetail #tblAddressDetail #frmAddressInfo').data('serialize', $('#insurancePlanDetail #tblAddressDetail #frmAddressInfo').serialize());
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    InsurancePlanAddressEdit: function (AddressId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Insurance Plan", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var selectedValue = AddressId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    setTimeout(function () { $('#addressDetailGrid #frmAddressInfo').data('bootstrapValidator').resetForm(); }, 300);

                    insurancePlanDetail.params.InsurancePlanAddressId = selectedValue;
                    insurancePlanDetail.params.PlanMode = "Edit";
                    $('#addressDetailGrid').modal({
                        show: 'true',
                        backdrop: 'static',
                        keyboard: false
                    });
                    insurancePlanDetail.FillInsurancePlanAddress(insurancePlanDetail.params.InsurancePlanAddressId).done(function (response) {
                        if (response.status != false) {
                            var posPlan_detail = JSON.parse(response.InsurancePlanAddressFill_JSON);
                            var self = $("#addressDetailGrid");

                            utility.bindMyJSON(true, posPlan_detail, false, self).done(function () {

                                //Serialize Data
                                $('#insurancePlanDetail #tblAddressDetail #frmAddressInfo').data('serialize', $('#insurancePlanDetail #tblAddressDetail #frmAddressInfo').serialize());

                            });
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
    },

    InsurancePlanAddressDelete: function (AddressId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Insurance Plan", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = AddressId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        insurancePlanDetail.DeleteInsurancePlanAddress(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvAddressInfo').DataTable();
                                table1.row('.active').remove().draw(false);

                                //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                utility.removePaginationFromGrid($('#' + insurancePlanDetail.params.PanelID + ' #pnlAddressInfo'));
                                //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ValidateInsurancePlanAddress: function () {
        $('#insurancePlanDetail #tblAddressDetail #frmAddressInfo')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Address: {
                       group: '.col-sm-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   City: {
                       group: '.col-xs-7',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   State: {
                       group: '.col-xs-5',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           regexp: {
                               regexp: /^[a-zA-Z]+$/,
                               message: ' '
                           }
                       }
                   },

                   Zip: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   //Telephone: {
                   //    group: '.col-sm-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   //Fax: {
                   //    group: '.col-sm-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   //UserID: {
                   //    group: '.col-sm-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   //Password: {
                   //    group: '.col-sm-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   WebPortal: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           uri: {
                               message: ' '
                           }
                       }
                   },
                   Email: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           regexp: {
                               regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                               message: 'Email not Valid'
                           }

                       }

                   }
               }
               //,
               //'WebPortal': {
               //    group: '.col-sm-3',
               //    validators: {
               //        notEmpty: {
               //            message: ''
               //        },
               //        uri: {
               //            message: 'Format not Valid'
               //        }
               //    }
               //}

           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            insurancePlanDetail.InsurancePlanAddressSave();
        });
    },

    InsurancePlanAddressSave: function () {
        var strMessage = "";
        var self = $("#addressDetailGrid");
        var myJSON = self.getMyJSON();
        if (insurancePlanDetail.params.PlanMode == "Add") {
            AppPrivileges.GetFormPrivileges("Insurance Plan", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    insurancePlanDetail.SaveInsurancePlanAddress(myJSON).done(function (response) {
                        if (response.status != false) {
                            insurancePlanDetail.LoadInsurancePlanAddress().done(function (response) {
                                if (response.status != false) {
                                    insurancePlanDetail.InsurancePlanAddressGridLoad(response);
                                    $('#addressDetailGrid').modal('hide');
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                            utility.DisplayMessages(response.message, 1);
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
        else if (insurancePlanDetail.params.PlanMode == "Edit") {
            AppPrivileges.GetFormPrivileges("Insurance Plan", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    insurancePlanDetail.UpdateInsurancePlanAddress(myJSON, insurancePlanDetail.params.InsurancePlanAddressId).done(function (response) {
                        if (response.status != false) {
                            insurancePlanDetail.LoadInsurancePlanAddress().done(function (response) {
                                if (response.status != false) {
                                    insurancePlanDetail.InsurancePlanAddressGridLoad(response);
                                    $('#addressDetailGrid').modal('hide');
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                            utility.DisplayMessages(response.message, 1);
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

    SaveInsurancePlanAddress: function (InsurancePlanAddressData) {
        var data = "InsurancePlanAddressData=" + InsurancePlanAddressData + "&InsurancePlanId=" + insurancePlanDetail.params.InsurancePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "SAVE_ADDRESS_INFO");
    },

    UpdateInsurancePlanAddress: function (InsurancePlanAddressData, InsurancePlanAddressID) {
        var data = "InsurancePlanAddressData=" + InsurancePlanAddressData + "&InsurancePlanAddressID=" + InsurancePlanAddressID + "&InsurancePlanId=" + insurancePlanDetail.params.InsurancePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "UPDATE_ADDRESS_INFO");
    },

    FillInsurancePlanAddress: function (InsurancePlanAddressID) {
        var data = "InsurancePlanAddressID=" + InsurancePlanAddressID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "FILL_ADDRESS_INFO");
    },

    LoadInsurancePlanAddress: function () {
        var data = "InsurancePlanId=" + insurancePlanDetail.params.InsurancePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "LOAD_ADDRESS_INFO");
    },

    DeleteInsurancePlanAddress: function (InsurancePlanAddressId) {
        var data = "InsurancePlanAddressId=" + InsurancePlanAddressId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "DELETE_ADDRESS_INFO");
    },

    UnLoadPlan: function () {

        if ($('#insurancePlanDetail #tblAddressDetail #frmAddressInfo').serialize() != $('#insurancePlanDetail #tblAddressDetail #frmAddressInfo').data('serialize')) {
            utility.myConfirm('2', function () {
                $('#addressDetailGrid').modal('hide');
            }, function () { },
                    '2'
                );
        }
        else {
            $('#addressDetailGrid').modal('hide');
        }


    },
    //----------------------------------------------------------------

    SaveInsurancePlan: function (InsurancePlanData) {
        var data = "InsurancePlanData=" + InsurancePlanData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "SAVE_INSURANCE_PLAN");
    },

    UpdateInsurancePlan: function (InsurancePlanData, InsurancePlanID) {
        var data = "InsurancePlanData=" + InsurancePlanData + "&InsurancePlanID=" + InsurancePlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "UPDATE_INSURANCE_PLAN");
    },

    FillInsurancePlan: function (InsurancePlanID) {
        var data = "InsurancePlanID=" + InsurancePlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "FILL_INSURANCE_PLAN");
    },

    UpdateInsurancePlanActiveInactive: function (InsurancePlanID, IsActive) {
        var data = "InsurancePlanID=" + InsurancePlanID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "UPDATE_INSURANCE_PLAN_ACTIVE_INACTIVE");
    },

    //SaveInsurancePlanAddressInfo: function (Address, City, State, Zip, Phone, ZipExt, PhoneExt, Fax, User, Password, Email, WebPortal) {
    //    var data = "Address=" + Address + "&City=" + City + "&State=" + State + "&Zip=" + Zip + "&ZipExt=" + ZipExt + "&Phone=" + Phone + "&PhoneExt=" + PhoneExt + "&Fax=" + Fax + "&User=" + User + "&Password=" + Password + "&Email=" + Email + "&WebPortal=" + WebPortal + "&InsurancePlanID=" + insurancePlanDetail.params.InsurancePlanId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "SAVE_ADDRESS_INFO");
    //},

    //UpdateInsurancePlanAddressInfo: function (Address, City, State, Zip, Phone, ZipExt, PhoneExt, Fax, User, Password, Email, WebPortal, InsurPlanAddressId) {
    //    var data = "Address=" + Address + "&City=" + City + "&State=" + State + "&Zip=" + Zip + "&ZipExt=" + ZipExt + "&Phone=" + Phone + "&PhoneExt=" + PhoneExt + "&Fax=" + Fax + "&User=" + User + "&Password=" + Password + "&Email=" + Email + "&WebPortal=" + WebPortal + "&InsurPlanAddressID=" + InsurPlanAddressId + "&InsurancePlanID=" + insurancePlanDetail.params.InsurancePlanId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "UPDATE_ADDRESS_INFO");
    //},

    //LoadInsurancePlanAddressInfo: function () {
    //    var data = "InsurancePlanID=" + insurancePlanDetail.params.InsurancePlanId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "LOAD_ADDRESS_INFO");
    //},

    //DeleteInsurancePlanAddressInfo: function (InsurPlanAddressId) {
    //    var data = "InsurPlanAddressID=" + InsurPlanAddressId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "DELETE_ADDRESS_INFO");
    //},

    UnLoad: function () {
        utility.UnLoadDialog(insurancePlanDetail.params.PanelID + ' #frmInsurancePlanDetail', function () {
            UnloadActionPan(insurancePlanDetail.params["ParentCtrl"], 'insurancePlanDetail', null, insurancePlanDetail.params.PanelID);
        }, function () {
            UnloadActionPan(insurancePlanDetail.params["ParentCtrl"], 'insurancePlanDetail', null, insurancePlanDetail.params.PanelID);
        });
    },

    LoadInsurancePlanAddresses: function () {
        insurancePlanDetail.LoadInsurancePlanAddressInfo().done(function (response) {
            if (response.status != false) {
                if (response.InsurancePlanAddressCount > 0) {
                    var InsurancePlanAddressJSON = JSON.parse(response.InsurancePlanAddress_JSON);
                    $.each(InsurancePlanAddressJSON, function (i, item) {
                        var $row = $('<tr/>');
                        $row.attr("InsurancePlanAddressId", item.InsurancePlanAddressId)
                        $row.append('<td style="display:none;">' + item.InsurancePlanAddressId + '</td><td>' + item.Address + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.ZipCode + '</td><td>' + item.PhoneNo + '</td><td style="display:none;">' + item.ZipCodeExt + '</td><td style="display:none;">' + item.PhoneExt + '</td><td style="display:none;">' + item.Fax + '</td><td style="display:none;">' + item.UserName + '</td><td>' + item.Password + '</td><td style="display:none;">' + item.EmailAddress + '</td><td style="display:none;">' + item.WebPortal + '</td>');
                        $("#dgvAddressInfo tbody").last().append($row);
                    });
                }
            }
        });
    },

    InsurancePlanInfo: function () {
        $(function () {
            var tbl = $("#dgvAddressInfo");
            var obj = $.paramquery.tableToArray(tbl);

            var newObj = {
                width: 800,
                height: 560,
                sortIndx: 0,

                numberCell: false,
                flexWidth: true,
                bottomVisible: false,
                editable: false,
                flexHeight: true,
                title: "PLAN ADDRESS",
                freezeCols: 1,
                resizable: false,
                scrollModel: { horizontal: false },
                selectionModel: {
                    type: 'row'
                },
                editModel: {
                    clicksToEdit: 2
                },
                selectionModel: {
                    mode: 'single'
                }
            };

            newObj.dataModel = {
                data: obj.data,
                paging: "local",
                rPP: 15,
                rPPOptions: [10, 15, 20, 50, 100]
            };
            newObj.colModel = obj.colModel;

            newObj.colModel[0].width = 100;
            newObj.colModel[1].width = 200;
            newObj.colModel[2].width = 100;
            newObj.colModel[3].width = 80;
            newObj.colModel[4].width = 100;
            newObj.colModel[5].width = 100;
            newObj.colModel[0].hidden = true;
            newObj.colModel[6].hidden = true;
            newObj.colModel[7].hidden = true;
            newObj.colModel[8].hidden = true;
            newObj.colModel[9].hidden = true;
            newObj.colModel[10].hidden = true;
            newObj.colModel[11].hidden = true;
            newObj.colModel[12].hidden = true;
            //append or prepend the CRUD toolbar to .pq-grid-top or .pq-grid-bottom
            $("#grid_crud").on("pqgridrender", function (evt, obj) {
                var $toolbar = $("<div class='pq-grid-toolbar pq-grid-toolbar-crud'></div>").appendTo($(".pq-grid-top", this));

                $("<a class='btn  btn-xs'  title='Add Record'><i class='fa fa-plus-circle '></i></a>").appendTo($toolbar).button({
                    icons: {
                        primary: "ui-icon-circle-plus"
                    }
                }).click(function (evt) {
                    addRow();
                });
                $("<a class='btn  btn-xs'  title='Edit Record'><i class='fa fa-edit '></i></a>").appendTo($toolbar).button({
                    icons: {
                        primary: "ui-icon-pencil"
                    }
                }).click(function (evt) {
                    editRow();
                });
                $("<a class='btn  btn-xs'  title='Delete Record'><i class='fa fa-trash-o '></i></a>").appendTo($toolbar).button({
                    icons: {
                        primary: "ui-icon-circle-minus"
                    }
                }).click(function () {
                    deleteRow();
                });
                $toolbar.disableSelection();
            });

            var $grid = $("#grid_crud").pqGrid(newObj);
            //create popup dialog.
            $("#popup-dialog-addressplan").dialog({
                width: 700,
                modal: true,
                open: function () {
                    $(".ui-dialog").position({
                        of: "#grid_crud"
                    });
                },
                autoOpen: false
            });

            //edit Row
            function editRow() {
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Insurance Plan", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var rowIndx = getRowIndx();
                        if (rowIndx != null) {
                            var DM = $grid.pqGrid("option", "dataModel");
                            var data = DM.data;
                            var row = data[rowIndx];
                            var $frm = $("form#frmAddressInfo");
                            $frm.find("input[name='Address']").val(row[1]);
                            $frm.find("input[name='City']").val(row[2]);
                            $frm.find("input[name='State']").val(row[3]);
                            $frm.find("input[name='Zip']").val(row[4]);
                            $frm.find("input[name='Telephone']").val(row[5]);
                            $frm.find("input[name='ZipExt']").val(row[6]);
                            $frm.find("input[name='PhoneExt']").val(row[7]);
                            $frm.find("input[name='Fax']").val(row[8]);
                            $frm.find("input[name='UserID']").val(row[9]);
                            $frm.find("input[name='Password']").val(row[10]);
                            $frm.find("input[name='Email']").val(row[11]);
                            $frm.find("input[name='WebPortal']").val(row[12]);

                            $("#popup-dialog-addressplan").dialog({
                                title: "Edit Record (" + (rowIndx + 1) + ")",
                                buttons: {
                                    Update: function () {
                                        //save the record in DM.data.
                                        row[1] = $frm.find("input[name='Address']").val();
                                        row[2] = $frm.find("input[name='City']").val();
                                        row[3] = $frm.find("input[name='State']").val();
                                        row[4] = $frm.find("input[name='Zip']").val();
                                        row[5] = $frm.find("input[name='Telephone']").val();
                                        row[6] = $frm.find("input[name='ZipExt']").val();
                                        row[7] = $frm.find("input[name='PhoneExt']").val();
                                        row[8] = $frm.find("input[name='Fax']").val();
                                        row[9] = $frm.find("input[name='UserID']").val();
                                        row[10] = $frm.find("input[name='Password']").val();
                                        row[11] = $frm.find("input[name='Email']").val();
                                        row[12] = $frm.find("input[name='WebPortal']").val();

                                        insurancePlanDetail.UpdateInsurancePlanAddressInfo(row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[0]);

                                        //$grid.pqGrid("refreshDataAndView").pqGrid('setSelection',{ rowIndx:rowIndx});
                                        $grid.pqGrid("refreshRow", {
                                            rowIndx: rowIndx
                                        }).pqGrid('setSelection', {
                                            rowIndx: rowIndx
                                        });
                                        $(this).dialog("close");
                                    },
                                    Cancel: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            }).dialog("open");
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }

            //append Row
            function addRow() {
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Insurance Plan", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        //debugger;
                        var DM = $grid.pqGrid("option", "dataModel");
                        var data = DM.data;

                        var $frm = $("form#frmAddressInfo");
                        $frm.find("input").val("");

                        $("#popup-dialog-addressplan").dialog({
                            title: "Add Record",
                            buttons: {
                                Add: function () {
                                    var row = [];
                                    //save the record in DM.data.
                                    row[1] = $frm.find("input[name='Address']").val();
                                    row[2] = $frm.find("input[name='City']").val();
                                    row[3] = $frm.find("input[name='State']").val();
                                    row[4] = $frm.find("input[name='Zip']").val();
                                    row[5] = $frm.find("input[name='Telephone']").val();
                                    row[6] = $frm.find("input[name='ZipExt']").val();
                                    row[7] = $frm.find("input[name='PhoneExt']").val();
                                    row[8] = $frm.find("input[name='Fax']").val();
                                    row[9] = $frm.find("input[name='UserID']").val();
                                    row[10] = $frm.find("input[name='Password']").val();
                                    row[11] = $frm.find("input[name='Email']").val();
                                    row[12] = $frm.find("input[name='WebPortal']").val();

                                    insurancePlanDetail.SaveInsurancePlanAddressInfo(row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12]).done(function (response) {
                                        if (response.status != false) {
                                            row[0] = response.InsurancePlanAddressId;
                                        }
                                    });

                                    data.push(row);
                                    $grid.pqGrid("refreshDataAndView");
                                    $(this).dialog("close");
                                },
                                Cancel: function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                        $("#popup-dialog-addressplan").dialog("open");
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }

            //delete Row.
            function deleteRow() {
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Insurance Plan", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var rowIndx = getRowIndx();
                        if (rowIndx != null) {
                            var DM = $grid.pqGrid("option", "dataModel");
                            insurancePlanDetail.DeleteInsurancePlanAddressInfo(DM.data[rowIndx][0]);
                            DM.data.splice(rowIndx, 1);
                            $grid.pqGrid("refreshDataAndView");
                            $grid.pqGrid("setSelection", {
                                rowIndx: rowIndx
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }

            function getRowIndx() {
                //var $grid = $("#grid_render_cells");

                //var obj = $grid.pqGrid("getSelection");
                //debugger;
                var arr = $grid.pqGrid("selection", {
                    type: 'row',
                    method: 'getSelection'
                });
                if (arr && arr.length > 0) {
                    var rowIndx = arr[0].rowIndx;

                    //if (rowIndx != null && colIndx == null) {
                    return rowIndx;
                } else {
                    //alert("Select a row.");
                    utility.DisplayMessages("Select a row.", 3);
                    return null;
                }
            }
        });
    },

    ShowHistory: function () {
        var PanelID = 'insurancePlanDetail';
        var ParentCtrl = 'insurancePlanDetail';
        var ProfileName = 'Insurance Plan';
        var DBTableName = 'InsurancePlan';
        var ColumnKeyId = insurancePlanDetail.params.InsurancePlanId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

    ShowPlanAddressHistory: function () {
        var PanelID = 'insurancePlanDetail';
        var ParentCtrl = 'insurancePlanDetail';
        var ProfileName = 'Insurance Plan Address';
        var DBTableName = 'InsurancePlanAddress';
        var ColumnKeyId = insurancePlanDetail.params.InsurancePlanAddressId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
};