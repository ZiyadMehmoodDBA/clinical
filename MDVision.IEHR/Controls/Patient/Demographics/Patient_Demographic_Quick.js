Patient_DemographicQuick = {
    params: [],
    bIsFirstLoad: true,
    imagedata: '',
    filetype: '',
    filename: '',
    frontimage: '',
    ScanPrivilige: false,
    OCRPrivilige: false,
    multipleEthnicityIds: '',
    checkedEthnicityNodes: [],

    // scannerjson: '',
    Load: function (params) {
        Patient_DemographicQuick.params = params;
        if (Patient_DemographicQuick.bIsFirstLoad) {
            Patient_DemographicQuick.bIsFirstLoad = false;
            Patient_DemographicQuick.LoadAllAutocomplete();
            Patient_DemographicQuick.BindRefProviderReferral();
            Patient_Demographic.BindCity("#pnlDemographicQuick #frmDemographicQuick");
            Patient_DemographicQuick.OpenAssignee();
        }

        utility.MakeSerializableHTML('#pnlDemographicQuick');

        $('#pnlDemographicQuick #chkActive').attr("checked", true);
        var self = $('#pnlDemographicQuick');
        $.when(Patient_Demographic.LoadingDropDowns('pnlDemographicQuick'), self.loadDropDowns(true)).then(function () {

            utility.InitKendoRaceAutoComplete('pnlDemographicQuick #ddlPatientRace', 'pnlDemographicQuick #hfRaceIds');
            Patient_DemographicQuick.IntializeMultiSelectDropDownEthnicity();

            Patient_DemographicQuick.multipleEthnicityIds = '';
            Patient_DemographicQuick.checkedEthnicityNodes = [];

            Patient_DemographicQuick.ValidateDemographicQuick();
            Patient_DemographicQuick.LoadPatientDemogrphic();
        });
    },
    fillQuickDemographicWithCardInfo: function (parsedDLCardData, faceimage) {

        var PatientSex = 0;
        if (parsedDLCardData.Sex == "M") {
            PatientSex = 0;
            //  $("#" + demographicDetail.params.PanelID + " #divdemographicDetailPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
        }
        else if (parsedDLCardData.Sex == "F") {
            PatientSex = 1;
            //  $("#" + demographicDetail.params.PanelID + " #divdemographicDetailPicture #imgPatient").attr("src", "Content/images/default_female_profile.gif");
        }
        else {
            PatientSex = 2;
        }
        var strzipCode = "";
        var strZipExt = "";
        if (parsedDLCardData.Zip.indexOf("-") > -1) {
            var strzip = parsedDLCardData.Zip.split("-");
            strzipCode = strzip[0];
            if (strzip.length > 1) {
                strZipExt = strzip[1];
            }
        }
        else {
            strzipCode = parsedDLCardData.Zip.substring(0, 5);
            strZipExt = parsedDLCardData.Zip.substring(5, parsedDLCardData.Zip.length);
        }

        Patient_DemographicQuick.frontimage = faceimage;

        if (Document_Scan.params["IsFromIfram"] == true) {

            $(window.parent.document).find("#imageFromScanner").attr("src", faceimage);

            $(window.parent.document).find("#pnlDemographicQuick #txtLastName").val(parsedDLCardData.NameLast);
            $(window.parent.document).find("#pnlDemographicQuick #txtFirstName").val(parsedDLCardData.NameFirst);
            $(window.parent.document).find("#pnlDemographicQuick #dtpDOB").datepicker("setDate", utility.formatDate(parsedDLCardData.DateOfBirth4));

            $(window.parent.document).find("#pnlDemographicQuick #ddlSex").val(PatientSex);
            $(window.parent.document).find("#pnlDemographicQuick #txtAddress1").val(parsedDLCardData.Address);
            //$('#' + Patient_DemographicQuick.params.PanelID + " #txtAddress2").val(parsedDLCardData.Address2);
            $(window.parent.document).find("#pnlDemographicQuick #txtCity").val(parsedDLCardData.City);
            $(window.parent.document).find("#pnlDemographicQuick #txtState").val(parsedDLCardData.State);



            $(window.parent.document).find("#pnlDemographicQuick #txtZip").val(strzipCode);
            $(window.parent.document).find("#pnlDemographicQuick #txtZipExt").val(strZipExt);

            $(window.parent.document).find('#pnlDocument_Scanner #btnIframeScannerClose').click();

        } else {
            $("#pnlDemographicQuick #txtLastName").val(parsedDLCardData.NameLast);
            //$('#' + demographicDetail.params.PanelID + " #ddlSuffix").val(parsedDLCardData.NameSuffix);
            $("#pnlDemographicQuick #txtFirstName").val(parsedDLCardData.NameFirst);


            //$('#' + demographicDetail.params.PanelID + " #dtpDOB").val(utility.formatDate(parsedDLCardData.DateOfBirth4));

            //$('#' + demographicDetail.params.PanelID + " #dtpDOB").val(utility.formatDate(parsedDLCardData.DateOfBirth4));
            $("#pnlDemographicQuick #dtpDOB").datepicker("setDate", utility.formatDate(parsedDLCardData.DateOfBirth4));
            //$('#' + demographicDetail.params.PanelID + " #dtpDOB").bootstrapzzValidator('revalidateField', 'DOB');


            $("#pnlDemographicQuick #ddlSex").val(PatientSex);
            $("#pnlDemographicQuick #txtAddress1").val(parsedDLCardData.Address);
            //$('#' + Patient_DemographicQuick.params.PanelID + " #txtAddress2").val(parsedDLCardData.Address2);
            $("#pnlDemographicQuick #txtCity").val(parsedDLCardData.City);
            $("#pnlDemographicQuick #txtState").val(parsedDLCardData.State);



            $("#pnlDemographicQuick #txtZip").val(strzipCode);
            $("#pnlDemographicQuick #txtZipExt").val(strZipExt);
            // $('#' + Patient_DemographicQuick.params.PanelID + " #chkBadAddress").prop("checked", !parsedDLCardData.IsAddressVerified);
            Document_Scan.UnLoadTab();
            BackgroundLoaderShow(false);

        }

    },
    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#pnlDemographicQuick #txtProvider");
            var hfCtrl = $("#pnlDemographicQuick #hfProvider");
            var onSelect = function (dataItem) { $("#pnlDemographicQuick #txtProvider").attr("ProviderId", dataItem.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });


        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#pnlDemographicQuick #txtProviderReferral");
            var hfCtrl = $("#pnlDemographicQuick #hfProviderReferral");
            var onSelect = function (dataItem) { $("#pnlDemographicQuick #txtProviderReferral").attr("ProviderId", dataItem.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#pnlDemographicQuick input#txtFacility");
            var hfCtrl = $("#pnlDemographicQuick #hfFacility");
            var onSelect = function (dataItem) {
                $("#pnlDemographicQuick #txtPractice").val(dataItem.Practice);
                $("#pnlDemographicQuick #hfPractice").val(dataItem.PracticeId);
                demographicDetail.ScanOCRPriviliges($('#hfPractice').val());
            }
            var onChange = function (valid) {
                if (Ctrl.val() != "" && $("#pnlDemographicQuick #txtPractice").val() == "") {
                    if (!valid) {
                        $("#pnlDemographicQuick #txtPractice").val("");
                        $("#pnlDemographicQuick #hfPractice").val("");
                    }
                }
                if ($(Ctrl).val() == "") {
                    $("#pnlDemographicQuick #txtPractice").val("");
                    $("#pnlDemographicQuick #hfPractice").val("");
                    ScanPrivilige = false;
                    OCRPrivilige = false;
                }
            }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect, onChange);
        });

    },


    IntializeMultiSelectDropDownEthnicity: function () {
        $('#pnlDemographicQuick #ddlEthnicity').multiselect('destroy');
        $('#pnlDemographicQuick #ddlEthnicity').multiselect({
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (element, checked) {
                var self = $('#pnlDemographicQuick');
                var options = $(element).parent().find('option');
                var Selectedoptions = $(element).parent().find('option:selected');
                if (element.length > 0) {
                    var optionText = $(element)[0].outerText;
                    var optionVal = $($(element)[0]).val();
                    if (checked) {
                        if (optionText.trim().toLowerCase() == "declined to specify" || optionText.trim().toLowerCase() == "refused to report") {
                            Selectedoptions.each(function (i, itm) {
                                if ($(itm).val() != optionVal)
                                    $(this).prop('selected', false);
                            });
                            $('#pnlDemographicQuick #ddlEthnicity').multiselect('refresh');
                            options.each(function (e, item) {
                                if ($(item).val() != optionVal) {
                                    var input = $('#pnlDemographicQuick #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                    input.prop('disabled', true);
                                    input.parent('li').addClass('disabled');
                                }
                            });
                        }
                    }
                    else {
                        if (optionText.trim().toLowerCase() == "declined to specify" || optionText.trim().toLowerCase() == "refused to report") {
                            options.each(function (e, item) {
                                if ($(item).val() != optionVal) {
                                    var input = $('#pnlDemographicQuick #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                    input.prop('disabled', false);
                                    input.parent('li').removeClass('disabled');
                                }
                            });
                        }
                        else {
                            options.each(function () {
                                var input = $('#pnlDemographicQuick #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                input.prop('disabled', false);
                                input.parent('li').addClass('disabled');
                            });
                        }
                    }
                }
                var EthnicityIds = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                    return this.value;
                }).get().join(',');
                Patient_DemographicQuick.multipleEthnicityIds = EthnicityIds;
            }
        });
        $("#pnlDemographicQuick #ddlEthnicity").val("");

        $('#pnlDemographicQuick').find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]').each(function () {
            if ($(this).attr('refval') == "hidden") {
                $(this).parent().addClass('text-bold');
            }
        });
    },


    LoadPatientDemogrphic: function () {

        if (globalAppdata['DefaultProviderName'] != "") {
            if (globalAppdata['DefaultProviderName'] != "- Select -")
                $('#pnlDemographicQuick #txtProvider').val(globalAppdata['DefaultProviderName']);
        }
        if (globalAppdata['DefaultProviderId'] != "")
            $('#pnlDemographicQuick #hfProvider').val(globalAppdata['DefaultProviderId']);
        if (globalAppdata['DefaultProviderId'] != "") {
            $('#pnlDemographicQuick #lnkProviderEdit').css("display", "inline");
            $('#pnlDemographicQuick #lblProvider').css("display", "none");
        }
        if (globalAppdata['DefaultFacilityName'] != "") {
            if (globalAppdata['DefaultFacilityName'] != "- Select -")
                $('#pnlDemographicQuick #txtFacility').val(globalAppdata['DefaultFacilityName']);
        }
        if (globalAppdata['DefaultFacilityId'] != "")
            $('#pnlDemographicQuick #hfFacility').val(globalAppdata['DefaultFacilityId']);
        if (globalAppdata['DefaultFacilityId'] != "") {
            $('#pnlDemographicQuick #lnkFacilityEdit').css("display", "inline");
            $('#pnlDemographicQuick #lblFacility').css("display", "none");
        }
        if (globalAppdata['DefaultPracticeName'] != "")
            $('#pnlDemographicQuick #txtPractice').val(globalAppdata['DefaultPracticeName']);
        if (globalAppdata['DefaultPracticeId'] != "") {
            $('#pnlDemographicQuick #hfPractice').val(globalAppdata['DefaultPracticeId']);
            demographicDetail.ScanOCRPriviliges(globalAppdata['DefaultPracticeId']);

        }


        $("#pnlDemographicQuick #ddlPrefLanguage").find("option").filter(function (index) {
            return "english" === $(this).text().toLowerCase();
        }).prop("selected", "selected");

        //serialize data
        $('#frmDemographicQuick').data('serialize', $('#frmDemographicQuick').serialize());
    },

    ValidateDemographicQuick: function () {
        $('#pnlDemographicQuick #frmDemographicQuick')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  LastName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  FirstName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  //Gender: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  DOB: {
                      group: '.col-sm-3',
                      validators: {
                          date: {
                              format: 'MM/DD/YYYY',
                              message: ''
                          },
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  //MaritalStatus: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  Ethnicity: {
                      enabled: false,
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  PatientRaceIds: {
                      enabled: false,
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  PrefLanguage: {
                      enabled: false,
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Address1: {
                      enabled: false,
                      group: '.col-sm-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  city: {
                      enabled: false,
                      group: '.size60per',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  State: {
                      enabled: false,
                      group: '.size40per',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Zip: {
                      enabled: false,
                      group: '.col-xs-8',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Provider: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Facility: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Practice: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  RefProviderReferral: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },

                  PatientRaceIds: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Status: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  //RefProvider: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  //PCP: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  //Guarantor: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  'Email': {
                      group: '.col-sm-4',
                      validators: {
                          emailAddress: {
                              message: 'Email not Valid'
                          }
                      }
                  },
              }
          })

       .on('success.form.bv', function (e) {

           e.preventDefault();

           Patient_DemographicQuick.DemographicQuickSave();
       });
    },

    ValidateValues: function () {
        if ($("#pnlDemographicQuick #hfPractice").val() == "-1") {
            utility.DisplayMessages("Practice not Valid", 2);
            return false;
        }
        else
            return true;
    },

    DemographicQuickSave: function () {
        var strMessage = "";
        var self = $("#pnlDemographicQuick");
        var myJSON = self.getMyJSONByName();
        var EthnicityIds = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        Patient_DemographicQuick.multipleEthnicityIds = EthnicityIds;
        AppPrivileges.GetFormPrivileges("Demographic", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var result;
                result = Patient_DemographicQuick.ValidateValues();
                if (result != false) {
                    var objData = JSON.parse(myJSON);
                    objData["strEthnicityIds"] = EthnicityIds;
                    myJSON = JSON.stringify(objData);
                    Patient_DemographicQuick.SaveDemographic(myJSON, true).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Patient_DemographicQuick.imagedata = "";
                            Patient_DemographicQuick.filetype = "";
                            Patient_DemographicQuick.filename = "";
                            Patient_DemographicQuick.frontimage = "";
                            ScanPrivilige = false;
                            OCRPrivilige = false;
                            $('#frmDemographicQuick').data('serialize', $('#frmDemographicQuick').serialize());
                            params["QuickAddPatient"] = true;
                            //PRD-114
                            if (Patient_DemographicQuick.params != null && Patient_DemographicQuick.params["ParentCtrl"] != null && Patient_DemographicQuick.params["ParentCtrl"] == "appointmentDetail" || (Patient_DemographicQuick.params && Patient_DemographicQuick.params["ParentCtrl"] == "Patient_Search")) {
                                UnloadActionPan(Patient_DemographicQuick.params["ParentCtrl"], "Patient_DemographicQuick");
                                if (Patient_DemographicQuick.params["ParentCtrl"] == "Patient_Search") {
                                    //AST 20
                                    if (Patient_DemographicQuick.params["GrandParentCtrl"] == "Patient_Document") {
                                        Patient_Search.PatientSearch(response.PatientId, null, null, 0);
                                        UnloadActionPan(Patient_DemographicQuick.params["ParentCtrl"]);

                                    }
                                        //prd 4370 load patient in banner.
                                    else {
                                        if (Patient_DemographicQuick.params.GrandParentCtrl == "appointmentDetail") {
                                            appointmentDetail.FillPatientInfoFromSearch(response.PatientId);
                                        }
                                        else {
                                            Patient_Search.UnLoad();
                                            setTimeout(function () {
                                                Patient_Search.SelectPatient(response.PatientId, null);
                                            }, 500);

                                        }
                                        
                              
                                    }


                                }
                                else {
                                    appointmentDetail.FillPatientAccount(response.PatientId, 0);
                                }

                            }
                            else {
                                Patient_DemographicQuick.UnLoad();
                                if (DefaultMenuSelected && DefaultMenuSelected == "MDVisionBilling") {
                                    SelectPatient(response.PatientId, "");
                                } else {
                                    SelectTab('mstrTabPatient', 'false');
                                    setTimeout(function () {
                                        SelectPatient(response.PatientId, "");
                                        localStorage.setItem("SelectedPatientId", response.PatientId);
                                        localStorage.setItem("SelectedAccountNumber", response.AccountNumber);
                                        localStorage.setItem("SelectPatientEntityId", globalAppdata.SeletedEntityId);
                                        IsBackgroundLoaderShow = false;
                                        $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount(response.PatientId, false)).then(function () {
                                            IsBackgroundLoaderShow = true;
                                            if (globalAppdata.IsImmunizationAlert != "False") {
                                            }
                                        });
                                    }, 500);
                                }
                            }
                            $(" #mainForm #hfTriggerLocation").val('FaceSheet');
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

    multiselect_Validator: function (cntrl) {
        if ($(cntrl).val() == null || $(cntrl).val() == "") {
            $(cntrl).closest('div').find('label.control-label').addClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '#cc2724');
            $(cntrl).closest('div').find('button').css('border-color', '#cc2724');
            $('#multiselectRace').css('display', '');
        } else {
            $(cntrl).closest('div').find('label.control-label').removeClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '');
            $(cntrl).closest('div').find('button').css('border-color', '');
            $('#multiselectRace').css('display', 'none');
        }
    },

    SaveDemographic: function (myJSON, bit) {

        var objData = JSON.parse(myJSON);
        objData["CommandType"] = "SAVE_DEMOGRAPHIC_QUICK";
        objData["imagedata"] = $('#frmDemographicQuick #myQuickCanvasUploadImg').attr('src');
        objData["filetype"] = Patient_DemographicQuick.filetype;
        objData["filename"] = Patient_DemographicQuick.filename;
        //objData["foldername"] = Patient_DemographicQuick.scannerjson.ddlFolder;
        objData["foldername"] = "130";
        objData["frontimage"] = Patient_DemographicQuick.frontimage;
        //objData["ScannerDOS"] = Patient_DemographicQuick.scannerjson.dtpDOS;
        //objData["AssignUserto"] = Patient_DemographicQuick.scannerjson.ddlAssignUserto;
        //objData["ScannerComments"] = Patient_DemographicQuick.scannerjson.txtComments;
        //objData["AssignedToName"] = Patient_DemographicQuick.scannerjson.ddlAssignUserto_text;
        objData["strRaceIds"] = $('#pnlDemographicQuick #hfRaceIds').val();
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographicQuick");

    },
    // -------------- Provider ---------------------

    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["IsOptional"] = false;
        params["RefForm"] = "frmDemographicQuick";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_DemographicQuick';
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($("#pnlDemographicQuick #hfProvider").val(),'Patient_DemographicQuick');
        var params = [];
        params["ProviderId"] = $("#pnlDemographicQuick #hfProvider").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Patient_DemographicQuick';
        LoadActionPan('providerDetail', params);
    },

    HideProviderLink: function () {
        $("#pnlDemographicQuick #txtProvider").attr("ProviderId", "-1");
        $("#pnlDemographicQuick #hfProvider").val("-1");
        $('#pnlDemographicQuick #lnkProviderEdit').css("display", "none");
        $('#pnlDemographicQuick #lblProvider').css("display", "inline");
    },
    // -------------- End Provider -----------------
    OpenDocumentScan: function () {
        var pid = $("#frmDemographicQuick #hfPractice").val();
        practiceDetail.DemographicPractice(pid).done(function (response) {
            if (response.status != false) {
                var medication_detail = JSON.parse(response.PracticeFill_JSON);
                ScanPrivilige = medication_detail.chkScan;
                OCRPrivilige = medication_detail.chkOCR;
                if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                    Patient_Demographic.ScanPrivilige = medication_detail.chkScan;
                    Patient_Demographic.OCRPrivilige = medication_detail.chkOCR;
                }
            }
            else {
                ScanPrivilige = false;
                OCRPrivilige = false;
                if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                    Patient_Demographic.ScanPrivilige = false;
                    Patient_Demographic.OCRPrivilige = false;
                }
            }
            if (ScanPrivilige == "True") {
                AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        var param = [];
                        var PanelID = null;

                        // PanelID = 'pnlDemographicQuick';

                        param["mode"] = "Scan";
                        param["RefFill"] = "QuickpatientDemographic";
                        param["RefCtrl"] = "patientDemographic";
                        param["ParentCtrl"] = 'Patient_DemographicQuick';
                        param["PracticeId"] = pid;
                        setDefaultValuesForScanCanvas(500, 260);
                        LoadActionPan('Document_Scanner', param, PanelID);
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
            else {
                utility.DisplayMessages("Either practice is not selected or practice does not have privileges to Scan. Please contact your administrator.", 2);
            }
        });


    },
    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmDemographicQuick";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_DemographicQuick';
        LoadActionPan('Admin_Facility', params);
    },


    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($("#pnlDemographicQuick #hfFacility").val(), 'Patient_DemographicQuick');
        var params = [];
        params["FacilityId"] = $("#pnlDemographicQuick #hfFacility").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Patient_DemographicQuick';
        LoadActionPan('facilityDetail', params);
    },

    HideFacilityLink: function () {
        $("#pnlDemographicQuick #txtFacility").attr("FacilityId", "-1");
        $("#pnlDemographicQuick #hfFacility").val("-1");
        $('#pnlDemographicQuick #lnkFacilityEdit').css("display", "none");
        $('#pnlDemographicQuick #lblFacility').css("display", "inline");
    },
    // -------------- End Facility -----------------

    CalculateAge: function () {
        Patient_DemographicQuick.FillAge($("#pnlDemographicQuick #dtpDOB").val()).done(function (response) {
            if (response.status != false) {
                $("#pnlDemographicQuick #txtAge").val(response.ActualAge);
            }
        });
    },

    UnLoad: function () {
        Patient_DemographicQuick.imagedata = "";
        Patient_DemographicQuick.filetype = "";
        Patient_DemographicQuick.filename = "";
        ScanPrivilige = false;
        OCRPrivilige = false;

        // Patient_DemographicQuick.scannerjson = "";
        utility.UnLoadDialog('frmDemographicQuick', function () {


            if (Patient_DemographicQuick.params != null && Patient_DemographicQuick.params.ParentCtrl != undefined && Patient_DemographicQuick.params != null && Patient_DemographicQuick.params.ParentCtrl != "") {
                UnloadActionPan(Patient_DemographicQuick.params.ParentCtrl);
            }
            else
                UnloadActionPan();

        }, function () {
            if (Patient_DemographicQuick.params != null && Patient_DemographicQuick.params.ParentCtrl != undefined && Patient_DemographicQuick.params != null && Patient_DemographicQuick.params.ParentCtrl != "") {
                UnloadActionPan(Patient_DemographicQuick.params.ParentCtrl);
            }
            else
                UnloadActionPan();
        });


    },

    hideShowTextbox: function () {
        if ($("#pnlDemographicQuick #ddlHearFrom").val() == 1) {
            var bootstrapValidator = $('#frmDemographicQuick').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('RefProviderReferral', true);

            var bootstrapValidator = $('#frmDemographicQuick').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('Status', true);

            Patient_DemographicQuick.getPatientReferral();
            $("#pnlDemographicQuick #incomingReferral").removeClass('hidden');
        }
        else {
            $("#pnlDemographicQuick #incomingReferral").addClass('hidden');

            var bootstrapValidator = $('#frmDemographicQuick').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('RefProviderReferral', false);

            var bootstrapValidator = $('#frmDemographicQuick').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('Status', false);

        }

        if ($("#pnlDemographicQuick #ddlHearFrom").val() == 10) {
            $("#pnlDemographicQuick #divOtherText").removeClass('hidden');
        }
        else {
            $("#pnlDemographicQuick #txtOtherText").val('');
            $("#pnlDemographicQuick #divOtherText").addClass('hidden');
        }
    },

    ///Referrals -- Added by Humaira Yousaf ///

    OpenRefProviderReferral: function () {
        var params = [];
        params["ReferringProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmDemographicQuick";
        params["IsOptional"] = false;
        params["RefCtrl"] = "txtRefProviderReferral";
        params["ParentCtrl"] = "Patient_DemographicQuick";
        params["RefCtrlHidden"] = "hfRefProviderReferral";
        LoadActionPan('Admin_ReferringProvider', params);
    },
    OpenProviderReferral: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmDemographicQuick";
        params["ProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_DemographicQuick";
        params["RefCtrl"] = "txtProviderReferral";
        params["RefCtrlHidden"] = "hfProviderReferral";
        LoadActionPan('Admin_Provider', params);
    },

    BindRefProviderReferral: function () {
        var Ctrl = $('#pnlDemographicQuick #txtRefProviderReferral');
        var hfCtrl = $("#pnlDemographicQuick #hfRefProviderReferral");
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    OpenAssignee: function () {
        CacheManager.BindCodes('GetUsers', true).done(function (result) {
            var Ctrl = $("#pnlDemographicQuick #frmDemographicQuick #txtAssignee");
            var hfCtrl = $("#pnlDemographicQuick #frmDemographicQuick #hfAssignee");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Users, null, hfCtrl);
        });
    },

    getPatientReferral: function () {
        utility.CreateDatePicker("pnlDemographicQuick #dtDate", function () {
        }, true);

        $('#pnlDemographicQuick #tmTime').timepicker({
            defaultTime: new Date()
        });


        $("#pnlDemographicQuick #menuAttach").remove();
        $("#pnlDemographicQuick #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="">View Attachment</a>');
        $("#pnlDemographicQuick #btnScanResult,#btnViewAttachment").addClass("disableAll");

    },



    //end referrals ///
}