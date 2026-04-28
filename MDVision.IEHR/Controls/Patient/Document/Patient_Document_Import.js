Document_Import = {
    params: [],
    SingleFileLimit: 6,
    //bVisitFirst:true,
    PasswordJSON: "",
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    Load: function (params) {
        Document_Import.params = params;
        var self = $('#pnlDocumentImport');

        var Tab = GetTab(Document_Import.params["TabID"]);
        if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "" && Document_Import.params.PatientDetail != "1") {

            Document_Import.params["PanelID"] = Tab["Container"] + ' #' + Tab["PanelID"];

            if (Tab["MasterTabID"] == "mstrTabPatient")
                Document_Import.SetDefaultDocument();
            else
                Document_Import.SetDocument(Tab);

        }
        if (Document_Import.params["PanelID"].trim().indexOf('#') == 0) {
            Document_Import.params["PanelID"] = Document_Import.params["PanelID"].trim().replace(/^#/, "");
        }
        if (Document_Import.params.RefCtrl && Document_Import.params.RefCtrl == "ImportDoc") {
            if ($('#' + Document_Import.params["PanelID"] + " #frmDocumentImport #uploadFilePH").hasClass("disableAll")) {
                $('#' + Document_Import.params["PanelID"] + " #frmDocumentImport #uploadFilePH").removeClass("disableAll");
            }
            //AST-285 set patient id from Batch->Docment flow 
            if (Document_Import.params['PanelID'] == "ctrlPanBatch #pnlBatchDocuments") {
                if (Document_Import.params['patientId']) {
                    $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val(Document_Import.params['patientId'])
                }
            }

        }

        if (Document_Import.params.RefCtrl != null && Document_Import.params.RefCtrl == "chargeBatchDetail" || Document_Import.params.RefCtrl == "paymentBatchDetail") {
            if (Document_Import.params.mode == "ImportDoc") {
                //  $('#pnlDocumentImport #Upload_Import_file').removeAttr("multiple");
                $("#pnlDocumentImport #frmDocumentImport #DivPrivate").show();
            }
            else {
                $('#pnlDocumentImport #Upload_Import_file').attr("multiple", "multiple");
                $('#pnlDocumentImport #titleDocument').html("Import Folder");
                $("#pnlDocumentImport #frmDocumentImport #DivPrivate").hide();
            }
            $('#pnlDocumentImport #btnSaveBatchDocument').show();
            $('#pnlDocumentImport #btnDocumentImport').hide();
            Document_Import.EnableAndDisableControls(false);
        }


        $.when(Document_Import.LoadDocumentProviderDropDown(), self.loadDropDowns(true)).then(function () {

            
            var patientId = $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val();
            $('#pnlDocumentImport #ddlFolder option').filter(function () { return $(this).val() == Document_Import.params.FolderId; }).prop("selected", true);
            if (Document_Import.params.RefCtrl != null && Document_Import.params.RefCtrl == "ImportDoc") {
                // $('#pnlDocumentImport #Upload_Import_file').removeAttr("multiple");
                $("#pnlDocumentImport #frmDocumentImport #DivPrivate").show();
                Document_Import.ValidateImport();
                Document_Import.LoadPatientCase(patientId);
                Document_Import.BindTagAutocomplete();
            } else if (Document_Import.params.RefCtrl != null && Document_Import.params.RefCtrl == "ImportFolder") {
                $('#pnlDocumentImport #Upload_Import_file').attr("multiple", "multiple");
                $('#pnlDocumentImport #titleDocument').html("Import Folder");
                $("#pnlDocumentImport #frmDocumentImport #DivPrivate").hide();
                Document_Import.ValidateImport();
                Document_Import.LoadPatientCase(patientId);
            } else if (Document_Import.params.RefCtrl != null && Document_Import.params.RefCtrl == "chargeBatchDetail" || Document_Import.params.RefCtrl == "paymentBatchDetail") {
                if (Document_Import.params.mode == "ImportDoc") {
                    //  $('#pnlDocumentImport #Upload_Import_file').removeAttr("multiple");
                    $("#pnlDocumentImport #frmDocumentImport #DivPrivate").show();
                }
                else {
                    $('#pnlDocumentImport #Upload_Import_file').attr("multiple", "multiple");
                    $('#pnlDocumentImport #titleDocument').html("Import Folder");
                    $("#pnlDocumentImport #frmDocumentImport #DivPrivate").hide();
                }
                $('#pnlDocumentImport #btnSaveBatchDocument').show();
                $('#pnlDocumentImport #btnDocumentImport').hide();
                Document_Import.EnableAndDisableControls(false);
                Document_Import.ValidateExternalImports();
            } else if (Document_Import.params.RefCtrl != null && Document_Import.params.RefCtrl == "Patient_MessageCompose") {
                $('#pnlDocumentImport #Upload_Import_file').removeAttr("multiple");
                $('#pnlDocumentImport #btnSaveBatchDocument').hide();
                $('#pnlDocumentImport #btnDocumentImport').hide();
                $('#pnlDocumentImport #btnSaveDirectMSGDocument').show();
                Document_Import.EnableAndDisableControls(true);
                Document_Import.ValidateExternalImportsDirectMSG();
                $('#pnlDocumentImport #fileinputImport').hide();
            }

            if (Document_Import.params.FromNote == "1") {
                $('#pnlDocumentImport #divDateOfService').hide();
                // $('#pnlDocumentImport #divPatientAccount').hide();
                $('#pnlDocumentImport #divClaimNumber').hide();
                $('#pnlDocumentImport #divCaseNumber').hide();
                $('#pnlDocumentImport #divFolder').hide();
                $('#pnlDocumentImport #divAssignUserto').hide();
                $('#pnlDocumentImport #btnDocumentImport').show();
                $('#pnlDocumentImport #titleDocument').text('Import audio');
                $('#pnlDocumentImport #Upload_Import_file').attr('accept', 'audio/mp3');
                $('#pnlDocumentImport #hfPatientId').val(Document_Import.params.patientId);
                $('#pnlDocumentImport #modaldocument').attr('class', 'modal-dialog modal-dialog-lg');

            }

            $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #ddlDocumentSource").on("change", function (e) {
                if ($("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #ddlDocumentSource option:selected").text().toLowerCase() == "other") {
                    $('#' + Document_Import.params["PanelID"] + " #frmDocumentImport #divOtherDocumentSource").removeClass('hidden');
                    $('#' + Document_Import.params["PanelID"] + " #frmDocumentImport #txtOtherDocumentSource").focus();
                }
                else {
                    $('#' + Document_Import.params["PanelID"] + " #frmDocumentImport #divOtherDocumentSource").addClass('hidden');
                    $('#' + Document_Import.params["PanelID"] + " #frmDocumentImport #txtOtherDocumentSource").val('');
                }

            });

            if (Document_Import.params.ParentCtrl == "advancePaymentDetail") {
                $("#pnlDocumentImport #frmDocumentImport #hfPatientId").val(Document_Import.params.patientId);
                $("#pnlDocumentImport #frmDocumentImport #txtAccountNumber").val(Document_Import.params.PatientName);
                $("#pnlDocumentImport #frmDocumentImport #divDateOfService").hide();
                $("#pnlDocumentImport #frmDocumentImport #divClaimNumber").hide();
                $("#pnlDocumentImport #frmDocumentImport #divCaseNumber").hide();
                $('#pnlDocumentImport #frmDocumentImport #txtAccountNumber').attr("disabled", "disabled");
                $('#pnlDocumentImport #frmDocumentImport #lnkPatientAccount').attr("disabled", "disabled");
                Document_Import.ValidateImport();
            }

            if (Document_Import.params.ParentCtrl == "Patient_Document" && Document_Import.params.PatientDetail == "1") {

                $("#pnlDocumentImport #frmDocumentImport #hfPatientId").val(Document_Import.params.patientId);


            }
            if (Document_Import.params["ParentCtrl"] == "OrderSet_Patient_Referrals_Outgoing_Detail") {
                Document_Import.params.ValidateAccountNumber = false;
                // $('#' + Document_Import.params["PanelID"] + " #divPatientAccount").css("display", "none");
            }


            //Start 27-04-2016 Humaira Yousaf to upload lab result
            if (Document_Import.params.RefCtrl != null && (Document_Import.params.RefCtrl == "LabResult" || Document_Import.params.RefCtrl == "RadiologyResult")) {
                //Start 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1024, EMR-1035
                var patientId = $('#PatientProfile #hfPatientId').val();
                var accountNo = $("#PatientProfile #hfAccountNo").val();

                if (parseInt(patientId == "" ? "0" : patientId) <= 0 && Document_Import.params.patientId != null) {
                    patientId = Document_Import.params.patientId;
                    if (Document_Import.params.AccountNumber != null)
                        accountNo = Document_Import.params.AccountNumber;
                }



                $("#" + Document_Import.params.PanelID + " #hfPatientId").val(patientId);
                $("#" + Document_Import.params.PanelID + " #txtAccountNumber").val(accountNo);
                $("#" + Document_Import.params.PanelID + " #txtAccountNumber").addClass("disableAll");
                $("#" + Document_Import.params.PanelID + " #lnkPatientAccount").addClass("disableAll");
                //End 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1024, EMR-1035
                Document_Import.ValidateImport();
            }
            //End 27-04-2016 Humaira Yousaf to upload lab result


            if (Document_Import.params.RefCtrl != null && (Document_Import.params.RefCtrl == "IncomingReferral" || Document_Import.params.RefCtrl == "OutgoingReferral" || Document_Import.params['RefCtrl'] == "OrderSetOutgoingReferral")) {

                if (Document_Import.params['RefCtrl'] != "OrderSetOutgoingReferral") {

                    var patientId = $('#PatientProfile #hfPatientId').val();
                    var accountNo = $("#PatientProfile #hfAccountNo").val();
                    //Start 01-11-2016 Humaira Yousaf for patient demograohic detail when patient is not selected
                    if (Document_Import.params.ParentCtrl == "demographicDetail" && $('#PatientProfile #hfPatientId').val() == "") {
                        patientId = Document_Import.params.patientId;
                        accountNo = Document_Import.params.AccountNo;
                    }
                    //End 01-11-2016 Humaira Yousaf for patient demograohic detail when patient is not selected
                    $("#" + Document_Import.params.PanelID + " #hfPatientId").val(patientId);
                    $("#" + Document_Import.params.PanelID + " #txtAccountNumber").val(accountNo);
                    if (Document_Import.params.RefCtrl == "OutgoingReferral") {
                        if (Document_Import.params.AccountNo) {
                            $("#" + Document_Import.params.PanelID + " #hfPatientId").val(Document_Import.params.PatientId);
                            $("#" + Document_Import.params.PanelID + " #txtAccountNumber").val(Document_Import.params.AccountNo);
                        }

                    }
                    if (patientId) {
                        $("#" + Document_Import.params.PanelID + " #txtAccountNumber").addClass("disableAll");
                        $("#" + Document_Import.params.PanelID + " #lnkPatientAccount").addClass("disableAll");
                    } else {
                        $("#" + Document_Import.params.PanelID + " #lnkPatientAccount").removeAttr("disable");
                        $("#" + Document_Import.params.PanelID + " #txtAccountNumber").removeAttr("disable");
                    }
                }
                //Start 08-09-2016 Humaira Yousaf for bug EMR-1169
                $('#pnlDocumentImport #ddlFolder').attr('disabled', true);
                if (Document_Import.params.RefCtrl == "OutgoingReferral" || Document_Import.params.RefCtrl == "OrderSetOutgoingReferral") {
                    var optionRef = $('#pnlDocumentImport #ddlFolder option').filter(function () {
                        return $(this).text() == 'Outgoing Referr';
                    }).prop("selected", true);

                    if (optionRef.length <= 0) {
                        $('#pnlDocumentImport #ddlFolder').removeAttr('disabled');
                    }
                }
                else {
                    var optionRef = $('#pnlDocumentImport #ddlFolder option').filter(function () {
                        return $(this).text() == 'Incoming Referr';
                    }).prop("selected", true);

                    if (optionRef.length <= 0) {
                        $('#pnlDocumentImport #ddlFolder').removeAttr('disabled');
                    }
                }

                //End 08-09-2016 Humaira Yousaf for bug EMR-1169
                Document_Import.ValidateImport();
            }
            //-------------------------------------------------------Start IMP-752 Irfan
            if (Document_Import.params.ParentCtrl == "ClinicalLabResultDetail") {
                var labFolder = $('#pnlDocumentImport #ddlFolder option').filter(function () {
                    return $(this).text() == 'Lab Result';
                }).prop("selected", true);
            }
            if (Document_Import.params.ParentCtrl == "ClinicalRadiologyResultDetail") {
                var labFolder = $('#pnlDocumentImport #ddlFolder option').filter(function () {
                    return $(this).text() == 'Radiology Resul';
                }).prop("selected", true);
            }
            //-------------------------------------------------------End IMP-752 Irfan
            //initialization of date-picker.
            utility.CreateDatePicker('pnlDocumentImport #dtpDOS', function () {
                //on-change callback method
            });
            utility.CreateDatePicker('pnlDocumentImport #dtpReceivedOn',
                 function (ev) {
                     if ($('#pnlDocumentImport #frmDocumentImport').data("bootstrapValidator") != null && typeof $('#frmDocumentImport').data('bootstrapValidator') != 'undefined') {
                         $('#pnlDocumentImport #frmDocumentImport').bootstrapValidator('revalidateField', 'ReceivedOn');
                     }
                 }, false);
            //initialization of date-picker.
            utility.CreateDatePicker('pnlDocumentImport #dtpExpirtyDate', function () {
                //on-change callback method
            });
            // default Doc Priority value selected 
            var defaultselected = "";
            // set low code commented by Faizan Ameen Fix  AST-451
            //$("#pnlDocumentImport #frmDocumentImport #ddlDocumentPriority >option").each(function () {
            //    if ($(this).text().toLowerCase() == "low") {
            //        {
            //            defaultselected = $(this).val();

            //        }
            //    }
            //});
            if (globalAppdata.DefaultDocumentPriorityId != "")
                defaultselected = globalAppdata.DefaultDocumentPriorityId;
            $("#pnlDocumentImport #frmDocumentImport #ddlDocumentPriority").val(defaultselected);
            Document_Import.IntializeMultiSelectDropDownFolder();
            // load dos of selected patient
            var patientId = $('#PatientProfile #hfPatientId').val();
            if (patientId == "") {
                patientId = $("#frmPatientDocument #hfPatientId").val();
            }
            Document_Import.LoadPatientVisitDOS(patientId);
            $('#pnlDocumentImport #ddlFolder option').filter(function () {
                if ($(this).text() == $('#' + Patient_Document.params["PanelID"] + ' #hfFolder').val()) {
                    $('#pnlDocumentImport #ddlFolder').val($(this).val());
                    $('#pnlDocumentImport #ddlFolder').multiselect("refresh");
                }
            });
            Document_Import.PasswordJSON = "";
            //serialize data
            $('#frmDocumentImport').data('serialize', $('#frmDocumentImport').serialize());
        });

        if (Document_Import.params["PopulateDOS"] == true) {
            $('#' + Document_Import.params["PanelID"] + ' #frmDocumentImport #dtpDOS').datepicker('setDate', new Date());
            $('#' + Document_Import.params["PanelID"] + ' #frmDocumentImport #dtpDOS').addClass('disableAll');
        }
        else {
            $('#' + Document_Import.params["PanelID"] + ' #frmDocumentImport #dtpDOS').removeClass('disableAll');
        }

        //select default patient PMS-1611
        if ($('#PatientProfile #hfPatientId').val() == "") {
            Document_Import.checkSelection();
        }
        else if (Document_Import.params.patientId && Document_Import.params.GrandParent == "demographicDetail") {
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val(Document_Import.params.patientId);
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #txtAccountNumber").val(Document_Import.params.AccountNo);
        }
        else {
            // if patient is selected in Patient-> document or batch-> document or patient is from schedular->demographics then  set Patientid
            if ($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val() || Patient_Document.params['ParentCtrl'] == 'demographicDetail') {
                // PMS-4555 First  select patient from Patient Document form not from Patient Banner.
                if (Document_Import.params.patientId && Document_Import.params.AccountNo) {
                    $("#" + Document_Import.params["PanelID"] + " #hfPatientId").val(Document_Import.params.patientId);
                    $("#" + Document_Import.params["PanelID"] + " #txtAccountNumber").val(Document_Import.params.AccountNo);
                    
                }
                else {
                    $("#" + Document_Import.params["PanelID"] + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                    $("#" + Document_Import.params["PanelID"] + " #txtAccountNumber").val($('#PatientProfile #hfAccountNo').val());
                }
             
                //$("#" + Document_Import.params["PanelID"] + " #txtAccountNumber").val($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfAccountNo').val());
            }
            else {
                // EMR-3706 Fixation by Bilawal
                $("#" + Document_Import.params["PanelID"] + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                $("#" + Document_Import.params["PanelID"] + " #txtAccountNumber").val($('#PatientProfile #hfAccountNo').val());
            }
        }




        Document_Import.BindPatientAccount();
        Document_Import.BindClaimNumber();
        //serialize data
        $('#frmDocumentImport').data('serialize', $('#frmDocumentImport').serialize());


        if (globalAppdata["isPatientHealthInformationCapture"] && globalAppdata["isPatientHealthInformationCapture"].toLowerCase() == "false") {
            $('#' + Document_Import.params.PanelID + " #frmDocumentImport #divDocumentProvider").addClass("hidden");
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #divReceivedOn").addClass("hidden");
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #divDocumentSource").addClass("hidden");
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #divDocumentRefrenceDetailHeading").addClass("hidden");
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #divDocumentRefrenceDetail").addClass("hidden");

        }
        if (Document_Import.params.ParentCtrlPanelID && Document_Import.params.ParentCtrlPanelID == "pnlEncounterChargeCapture #pnlPatientDocument") {

            $("#" + Document_Import.params.PanelID + " #txtClaimNumber").val(Document_Import.params.ClaimNumber);
            $("#" + Document_Import.params.PanelID + " #txtClaimNumber,#lnkClaimNumber").attr("disabled", "disabled");
            $("#" + Document_Import.params.PanelID + " #hfPatientId").val(Document_Import.params.patientId);
            $("#" + Document_Import.params.PanelID + " #txtAccountNumber").val(Document_Import.params.AccountNo);
            $("#" + Document_Import.params.PanelID + " #txtAccountNumber,#lnkPatientAccount").attr("disabled", "disabled");
            $("#" + Document_Import.params.PanelID + " #hfVisitId").val(Document_Import.params.VisitId);
            $('#' + Document_Import.params["PanelID"] + ' #frmDocumentImport #dtpDOS').datepicker('setDate', Document_Import.params.DOS);
        }
        else {
            $("#" + Document_Import.params.PanelID + " #txtClaimNumber").removeAttr("disabled");
            $("#" + Document_Import.params.PanelID + " #txtAccountNumber").removeAttr("disabled");

        }       
        
    },

    LoadDocumentProviderDropDown: function () {
        var patientId = Document_Import.params.patientId;
        if (!patientId)
            patientId = '-1';
        var data = "IsActive=&ID=" + patientId;
        return MDVisionService.lookups('GetDocumentProvider', true, data).done(function (result) {
            if (result['GetDocumentProvider']) {
                var options = JSON.parse(result['GetDocumentProvider']);
                var $documentProviderDDL = $('#' + Document_Import.params.PanelID + ' #ddlDocumentProvider');
                $documentProviderDDL.empty();
                $.each(options, function (i, item) {
                    if (item.Value != null) {
                        $documentProviderDDL.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                    }
                });
                $documentProviderDDL.append(
                    $('<option/>', {
                        value: 0,
                        html: 'Self',
                        refname: '',
                        refvalue: ''

                    })
                );
            }
        })
    },

    IntializeMultiSelectDropDownFolder: function () {
        $('#' + Document_Import.params.PanelID + ' #ddlFolder').multiselect('destroy');
        $('#' + Document_Import.params.PanelID + ' #ddlFolder').multiselect({
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (option, checked) {
                var options = $(option).parent().find('option');
                var Selectedoptions = $(option).parent().find('option:selected');
                if (option.length > 0) {
                    var optionText = $(option)[0].outerText;
                    var optionVal = $($(option)[0]).val();
                    if (checked) {
                        $('#' + Document_Import.params.PanelID + " #ddlFolder").multiselect('refresh');
                    }
                    else {
                        options.each(function () {
                            var input = $('#pnlDocumentImport #ddlFolder input[type=checkbox][value="' + $(this).val() + '"]');
                            input.prop('disabled', false);
                        });
                    }
                }
            },
        });
        $('#' + Patient_Demographic.params.PanelID + " #ddlFolder").val("");
        Document_Import.validateFolder(3);
        if (Document_Import.params.ParentCtrl && Document_Import.params.ParentCtrl == "Bill_Insurance_Payment_Detail") {
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfEOBManualPostingId").val(Document_Import.params.EOBPostingId);

            var optionRef = $("#" + Document_Import.params.PanelID + " #frmDocumentImport #ddlFolder option").filter(function () {
                console.log($(this).text());
                return $(this).text() == 'EOB Documents';
            }).prop("selected", true);
            $("#" + Document_Import.params.PanelID + " #frmDocumentImport #ddlFolder").multiselect('refresh');
            //$("#" + Document_Import.params.PanelID + " #frmDocumentImport #ddlFolder").attr("disabled", "disabled");
            $('#pnlDocumentImport #ddlFolder').multiselect("disable");
        }

    },

    checkSelection: function () {
        var bResult = false;
        var self = $("#pnlDocumentImport");
        self.find('#divFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            if (this.nextSibling.data.trim() == "Pat Education") {
                bResult = true;
            }
        });
        if (bResult) {
            //  $('#' + Document_Import.params["PanelID"] + " #divPatientAccount").css("display", "none");
            Document_Import.params.ValidateAccountNumber = false;
        } else {
            if ($("#" + Document_Import.params.PanelID + " #hfPatientId").val() == 0 || $("#" + Document_Import.params.PanelID + " #hfPatientId").val() == -1) {
                $("#" + Document_Import.params.PanelID + " #hfPatientId").val(-1); //Set to defualt value
                $('#' + Document_Import.params["PanelID"] + " #divPatientAccount").css("display", "inline");
                Document_Import.params.ValidateAccountNumber = true;
            }
        }
        var FolderIds = self.find('#divFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        if (FolderIds != "" && FolderIds != ",") {
            Document_Import.validateFolder(2);
        } else {
            Document_Import.validateFolder(1);
        }
    },

    SetDefaultDocument: function () {

        //$('#' + Document_Import.params["PanelID"] + " #divPatientAccount").css("display", "none");
        $("#" + Document_Import.params.PanelID + " #hfPatientId").val(Document_Import.params.patientId);
        Document_Import.params.ValidateAccountNumber = false;
    },

    SetDocument: function (Tab) {

        if (Document_Import.params["PanelID"].trim().indexOf('#') == 0)
            $(Document_Import.params["PanelID"] + " #divPatientAccount").css("display", "inline");
        else
            $('#' + Document_Import.params["PanelID"] + " #divPatientAccount").css("display", "inline");

        Document_Import.params.ValidateAccountNumber = true;

    },

    BufferFile: function (obj) {
        var toReturn = true;
        if (obj.files && obj.files.length != 0) {
            var filteredfiles = Document_Import.FilterFiles();
            if (Document_Import.params.FromNote == "1") {
                Document_Import.ValidateUploadedAudioFiles();
            } else
                Document_Import.ValidateUploadedFiles(filteredfiles);
            Document_Import.FilesContainer.Files = filteredfiles;
            $('#frmDocumentImport').bootstrapValidator('revalidateField', 'fileupload');
            //Document_Import.FilesContainer.Name = obj.files[0].name;
            //var FileData = JSON.stringify(obj.files[0].rawFile);
            //var FileData = obj.files[0].name;
        }
        else {
            delete Document_Import.FilesContainer.Files;
            Document_Import.TruncateFileControl();
            $('#frmDocumentImport').bootstrapValidator('revalidateField', 'fileupload');
            toReturn = false;
        }
        return toReturn;

    },

    BrowseFile: function () {
        setTimeout(function () { $("#Upload_Import_file").click() }, 10);
    },

    ValidateFile: function (obj) {
        var extensionsAttrib = "ext";
        var toReturn = false;
        if (!$(obj).prop("disabled") && $(obj).css('display') != "none") {
            if (obj.files[0]) {
                var fileName = obj.files[0].name;
                //-New snippet start
                if ($(obj).attr(extensionsAttrib)) {
                    $.each(($(obj).attr(extensionsAttrib)).split(','), function (index, extension) {
                        if ($.trim(extension).length > 0) {
                            if ($.trim(fileName).toLowerCase().lastIndexOf(
                                $.trim(extension).toLowerCase()) +
                                extension.length ==
                               $.trim(fileName).length)
                                toReturn = true;
                        }
                    });
                }
                else {
                    var reg = /^([0-9a-zA-Z_\-~ :\\])+(.pdf|.PDF|||.bmp|.BMP|.gif|.GIF|)$/;

                    if (reg.test(fileName)) {
                        toReturn = true;
                    }
                }
            }
        }
        return toReturn;
    },

    ValidateImport: function () {
        $('#frmDocumentImport')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    fileupload: {
                        group: '.col-sm-9',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Folder: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    /* DocumentProvider: {
                         group: '.col-sm-3',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     DocumentSource: {
                         group: '.col-sm-3',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     ReceivedOn: {
                         group: '.col-sm-3',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },*/
                    //AccountNumber: {
                    //    group: '.col-sm-3',
                    //    enabled: Document_Import.params.ValidateAccountNumber,
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
            Document_Import.ImportSave();
        });
    },
    ValidateExternalImports: function () {
        $('#frmDocumentImport')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    fileupload: {
                        group: '.col-sm-9',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            })
        .on('success.form.bv', function (e) {

        });
    },

    ValidateExternalImportsDirectMSG: function () {
        $('#frmDocumentImport')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    AccountNumber: {
                        group: '.col-sm-3',
                        enabled: Document_Import.params.ValidateAccountNumber,
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Folder: {
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

        });
    },


    ImportSave: function () {
        if (Document_Import.FilesContainer.Files) {
            var objDefIportSave = $.Deferred();
            var fileCount = Document_Import.FilesContainer.Files.length;
            if ($('#' + Document_Import.panelID + " #hfPatientId").val() == "-1") {
                utility.DisplayMessages("Patient not Valid", 2);
                return false;
            }
            if (Document_Import.params.FromNote == "1") {
                $('#pnlDocumentImport #hfPhoneEncounterFolder').val('PhoneEncounterFolder');
            } else {
                $('#pnlDocumentImport #hfPhoneEncounterFolder').val('');
            }
            var data = new FormData();
            var counter = 0;
            var filenameFull = "";
            var FileType = "";
            if (isFileCompressed) {
                $.each(Document_Import.FilesContainer.Files, function (key, value) {
                    var objDef = $.Deferred();
                    var fileObject = "";
                    filenameFull = "";
                    filenameFull = value.name;
                    FileType = value.type;
                    data.append("fileType", value.type);
                    data.append("FileName", value.name);
                    var zipFileToLoad = value;
                    var fileReader = new FileReader();
                    fileReader.addEventListener("load", function () {
                        fileObject = fileReader.result;
                        objDef.resolve("ok")
                    }, false);
                    if (zipFileToLoad) {
                        fileReader.readAsDataURL(zipFileToLoad);
                    }
                    objDef.then(function () {
                        var zip = new JSZip();
                        zip.file(filenameFull, fileObject.split(',')[1], { base64: true });
                        zip.generateAsync({ type: "blob", compression: "DEFLATE", compressionOptions: { level: 9 } }).then(function (blob) {
                            data.append(key, blob, value.name);
                            counter = counter + 1;
                            if (fileCount == counter) {
                                objDefIportSave.resolve("ok")
                            }
                        });
                    });
                });
            } else {
                var extensions;
                var fileNameWithoutExtensions;
                if (Document_Import.params.RefCtrl && Document_Import.params.RefCtrl == "ImportDoc") {
                    extensions = $("#" + Document_Import.params.PanelID + " #uploadFilePH").attr("extension").split(',');
                    fileNameWithoutExtensions = $("#" + Document_Import.params.PanelID + " #uploadFilePH").val().split(',');
                }
                $.each(Document_Import.FilesContainer.Files, function (key, value) {
                    var valueName = "";
                    if (Document_Import.params.RefCtrl && Document_Import.params.RefCtrl == "ImportDoc") {
                        valueName = fileNameWithoutExtensions[key] + extensions[key];
                    }
                    else {
                        valueName = value.name;
                    }
                    var objDef = $.Deferred();
                    filenameFull = valueName;
                    FileType = value.type;
                    data.append("fileType", value.type);
                    data.append("FileName", valueName);
                    data.append(key, value);
                    counter = counter + 1;
                    if (fileCount == counter) {
                        objDefIportSave.resolve("ok")
                    }
                });
            }
            data.append("PatientID", $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val());
            data.append("NotesId", Document_Import.params.NotesId);
            data.append("VisitId", Document_Import.params.VisitId);

            var self = $("#pnlDocumentImport");
            var myJSON = self.getMyJSON();
            var formJson = JSON.parse(myJSON);
            var docAssignedUserId = formJson.ddlAssignUserto;
            // For multiple folder selection
            var FolderList = [];
            self.find('#divFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                var objectFolder = new Object();
                objectFolder.Value = this.value;
                objectFolder.Name = this.nextSibling.data.trim();
                FolderList.push(objectFolder);
            });
            if (FolderList.length > 0) {
                data.append("PatientDocumentData", myJSON);
                data.append("FolderList", JSON.stringify(FolderList));
                if (Document_Import.params['RefCtrl'] == "advancePayment")
                    data.append('advancePaymentId', Document_Import.params['AdvancePaymentId']);
                if (Document_Import.PasswordJSON != "") {
                    data.append("PasswordJSON", Document_Import.PasswordJSON)
                } else {
                    data.append("PasswordJSON", "{'SetPassword':'','ConfirmPassword':'','UserId':null}")
                }
                //Start 03-05-2016 Humaira Yousaf
                if (Document_Import.params['RefCtrl'] == "LabResult" || Document_Import.params['RefCtrl'] == "RadiologyResult" || Document_Import.params['RefCtrl'] == "IncomingReferral" || Document_Import.params['RefCtrl'] == "OutgoingReferral" || Document_Import.params['RefCtrl'] == "OrderSetOutgoingReferral") {
                    data.append('RefModuleName', Document_Import.params['RefModuleName']);
                    data.append('TransitionId', Document_Import.params['TransitionId']);
                    data.append('OrderSetReferralId', Document_Import.params['OrderSetReferralId']);
                }
                objDefIportSave.then(function () {
                    //End 03-05-2016 Humaira Yousaf
                    if (Document_Import.params.mode == "Add") {
                        Document_Import.SaveImport(data).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);

                                if (docAssignedUserId == globalAppdata["AppUserId"]) {
                                    // Quick link Document count set                                       
                                    Document_AssignedTo.PendingCountSearchDoc();
                                }

                                if (Document_Import.params.FromNote == "1") {
                                    UnloadActionPan(Document_Import.params["ParentCtrl"], "Document_Import");
                                    $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkPlayfile').show().text($('#pnlDocumentImport #uploadFilePH').val());
                                    $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkAudioupload').hide();
                                    $('#' + Clinical_ProgressNote.params.PanelID + '  #filetag').hide();
                                } else {
                                    if (Document_Import.params['RefCtrl'] == "advancePayment") {
                                        UnloadActionPan(Document_Import.params["ParentCtrl"], 'Document_Import');

                                        advancePaymentDetail.DocumentSearch();
                                    }
                                        //Start 27-04-2016 Humaira Yousaf to upload lab result
                                    else if (Document_Import.params['RefCtrl'] == "LabResult" || Document_Import.params['RefCtrl'] == "RadiologyResult" || Document_Import.params['RefCtrl'] == "IncomingReferral" || Document_Import.params['RefCtrl'] == "OutgoingReferral" || Document_Import.params['RefCtrl'] == "OrderSetOutgoingReferral" || Document_Import.params['ParentCtrl'] == "Bill_Insurance_Payment_Detail") {
                                        UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import');
                                    }
                                        //End 27-04-2016 Humaira Yousaf to upload lab result
                                    else {
                                        //Patient_Document.DocumentSearch();

                                        //var ListItemId = Document_Import.params.FolderId;
                                        //Patient_Document.LoadFoldersAfterDeleteOrUpdateRecords().done(function () {

                                        //    ListItemHighLight(ListItemId);
                                        //});
                                        $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #hfPatientId").val($("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val());
                                        if (Document_Import.params.GrandParent != "demographicDetail") {
                                            if (Document_Import.params.PatientId)
                                                $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(Document_Import.params.PatientId);
                                            if (Document_Import.params.FullName)
                                                $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(Document_Import.params.FullName);
                                            if (Document_Import.params.AccountNumber)
                                                $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtAccountNumber').val(Document_Import.params.AccountNumber);
                                        }
                                        //AST-285 BY:MAHMAD
                                        Patient_Document.BindAutocomplete()
                                        //AST-285 BY:MAHMAD
                                        Patient_Document.LoadFolders(true, true);
                                        if (Document_Import.params.PatientDetail == "1") {
                                            if (Document_Import.params.ParentCtrl == "Patient_Document" && Document_Import.params.ParentCtrlPanelID) {
                                                UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import', null, Document_Import.params.ParentCtrlPanelID);
                                            }
                                            else {
                                                UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import', null, Document_Import.params.PanelID);
                                            }
                                        }
                                        else if (Document_Import.params.ParentCtrl == "Patient_Document" && Document_Import.params.ParentCtrlPanelID) {
                                            UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import', null, Document_Import.params.ParentCtrlPanelID);
                                            if (Document_Import.params.DocPrivate == "1") {
                                                if ($('.modal-backdrop').length > 0) {
                                                    $('body').find('.modal-backdrop').removeClass('modal-backdrop');
                                                }
                                            }
                                        } else if (Document_Import.params.ParentCtrl == "Clinical_FaceSheet") {
                                            UnloadActionPan(Document_Import.params.ParentCtrl, "Document_Import");
                                        }
                                        else {
                                            UnloadActionPan(folderDetail.params["ParentCtrl"], "Document_Import");
                                            if (Document_Import.params.DocPrivate == "1") {
                                                if ($('.modal-backdrop').length > 0) {
                                                    $('body').find('.modal-backdrop').removeClass('modal-backdrop');
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                });
            }
            else {
                Document_Import.validateFolder(1);
            }
        }
        else {
            utility.DisplayMessages("Please select a valid file to upload.", 3);
        }
    },

    validateFolder: function (operationid) {
        $("#pnlDocumentImport #frmDocumentImport #divFolder label").find("i").remove();
        if (operationid == 1) {
            $("#pnlDocumentImport #frmDocumentImport #divFolder .multiselect").css("border-color", "#cc2724");
            $("#pnlDocumentImport #frmDocumentImport #divFolder").find(".control-label").css("color", "#cc2724");
            $("#pnlDocumentImport #frmDocumentImport #divFolder").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlDocumentImport #frmDocumentImport #divFolder .multiselect").css("border-color", "#3c763d");
            $("#pnlDocumentImport #frmDocumentImport #divFolder").find(".control-label").css("color", "#3c763d");
            $("#pnlDocumentImport #frmDocumentImport #divFolder").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlDocumentImport #frmDocumentImport #divFolder .multiselect").css("border-color", "#ccc");
            $("#pnlDocumentImport #frmDocumentImport #divFolder").find(".control-label").css("color", "#000000");
        }
    },

    SaveImport: function (data, PatientDocumentData) {
        //var data = "data=" + data + "&PatientDocumentData=" + PatientDocumentData;// + "&PatientID=" + PatientID + "&DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "SAVE_PATIENT_DOCUMENT");
    },

    SaveImport1: function (PatientDocumentData, PatientID, DocumentID) {
        if (PatientID == null) {
            PatientID = 0;
        }
        if (DocumentID == null) {
            DocumentID = 0;
        }
        //var data = "PatientDocumentData=" + PatientDocumentData + "&PatientID=" + PatientID + "&DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "PATIENT_DOCUMENT");
        //return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT");
    },

    UnLoad: function () {
        //AST-285 BY:MAHMAD
        if (Document_Import.params["from"] == "Patient_Document") {
            Patient_Document.BindAutocomplete()
        }
        //AST-285 BY:MAHMAD
        utility.UnLoadDialog('frmDocumentImport', function () {
            if (Document_Import.params["FromAdmin"] == "0") {
                var PanelID = Patient_Document.params["PanelID"];
                if (Document_Import.params != null && Document_Import.params.ParentCtrl != null) {
                    if (typeof Clinical_LabOrder != "undefined" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                        UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import', null, "pnlClinicalProgressNote");
                    }
                    else if (typeof Patient_Document != "undefined" && Patient_Document.params["ParentCtrl"] && Patient_Document.params["PanelID"].indexOf(PanelID) > -1) {
                        var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
                        UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import', null, parentPanelId);
                    }

                    else {
                        UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import');
                    }
                }
                else {
                    UnloadActionPan(null, 'Document_Import');
                }
                if (Document_Import.params.DocPrivate == "1") {
                    if ($('.modal-backdrop').length > 0) {
                        $('body').find('.modal-backdrop').removeClass('modal-backdrop');
                    }
                }
            }
            else {
                RemoveAdminTab();
            }

        }, function () {
            if (Document_Import.params["FromAdmin"] == "0") {
                if (Document_Import.params != null && Document_Import.params.ParentCtrl != null) {
                    if (Document_Import.params.ParentCtrl == "Patient_Document") {
                        UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import', null, Document_Import.params.PanelID);
                    }
                    else {
                        UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import');
                    }
                }
                else
                    UnloadActionPan(null, 'Document_Import');
            }
            else {
                RemoveAdminTab();
            }
        });

    },
    ValidateUploadedAudioFiles: function () {
        var fileName = "";
        var files = $('#Upload_Import_file').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "audio/mp3") {
                utility.DisplayMessages("Only (.mp3) files are supported", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            //if (Document_Import.ValidateFileSize(files) > Number(globalAppdata['FileSize'])) {
            //    utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
            //    Document_Import.TruncateFileControl();
            //    return false;
            //}
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
        $('#totalFiles').text(files.length + " file(s) selected");
        return true;
    },
    FilterFiles: function () {
        var filteredfiles = [];
        Document_Import.FilesContainer.Files = undefined;
        var files = $('#Upload_Import_file').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType == "application/pdf" || fileType == "image/jpeg" || fileType == "image/png" || fileType == "image/jpg" || fileType == "image/gif" || fileType == "image/bmp") {
                filteredfiles.push(files[i]);
            }

        }
        if (filteredfiles.length == 0) {
            if (!Document_Import.params.ParentCtrlPanelID)
                utility.DisplayMessages("File Type is Invalid", 4);
        }

        return filteredfiles;
    },
    ValidateUploadedFiles: function (filteredfiles) {
        var fileName = "";
        var fileNameWithoutExtension = "";
        var fileExtension = "";
        //var files = $('#Upload_Import_file').get(0).files;
        var files = filteredfiles;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/pdf" && fileType != "image/jpeg" && fileType != "image/png" && fileType != "image/jpg" && fileType != "image/gif" && fileType != "image/bmp" && fileType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && fileType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && fileType != "application/msword" && fileType != "application/vnd.ms-excel") {
                utility.DisplayMessages("File Type is Invalid", 4);
                Document_Import.TruncateFileControl();
                //return false;
            }
            if (Document_Import.ValidateFileSizeCustom(files[i]) > Document_Import.SingleFileLimit) {
                utility.DisplayMessages("File " + files[i].name + " exceeds size limit.Maximum " + Document_Import.SingleFileLimit + "MB is allowed.", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                Document_Import.TruncateFileControl();
                filteredfiles = [];
                return false;
            }

            fileName = fileName + files[i].name + ',';
            if (Document_Import.params.RefCtrl && Document_Import.params.RefCtrl == "ImportDoc") {
                fileNameWithoutExtension += files[i].name.substring(0, files[i].name.lastIndexOf(".")) + ',';
                fileExtension += files[i].name.substring(files[i].name.lastIndexOf("."), files[i].name.length) + ',';
            }
        }

        fileName = fileName.slice(0, fileName.length - 1);
        fileNameWithoutExtension = fileNameWithoutExtension.slice(0, fileNameWithoutExtension.length - 1);
        fileExtension = fileExtension.slice(0, fileExtension.length - 1);

        if (Document_Import.params.RefCtrl && Document_Import.params.RefCtrl == "ImportDoc") {
            document.getElementById("uploadFilePH").value = fileNameWithoutExtension;
            $('#' + Document_Import.params.PanelID + ' #uploadFilePH').attr("extension", fileExtension);
        }
        else {
            document.getElementById("uploadFilePH").value = fileName;
        }

        $('#totalFiles').text(files.length + " file(s) selected");
        return true;
    },
    ValidateFileSize: function (files) {
        var size = 0;
        $.each(files, function (index, file) {
            size = size + Number((file.size / (1024 * 1024)).toFixed(2));
        });
        return size;

    },

    ValidateFileSizeCustom: function (file) {
        var size = 0;
        size = size + Number((file.size / (1024 * 1024)).toFixed(2));
        return size;
    },

    saveBatchDocument: function () {
        if (Document_Import.params.RefCtrl == "chargeBatchDetail") {
            chargeBatchDetail.GetImportDocuments(Document_Import.FilesContainer.Files);
            if (Document_Import.FilesContainer.Files == null)
                return false;
            delete Document_Import.FilesContainer.Files;
        }
        else if (Document_Import.params.RefCtrl == "paymentBatchDetail") {
            paymentBatchDetail.GetImportDocuments(Document_Import.FilesContainer.Files);
            if (Document_Import.FilesContainer.Files == null)
                return false;
            delete Document_Import.FilesContainer.Files;
        }
        $('#frmDocumentImport').data('serialize', $('#frmDocumentImport').serialize());
        Document_Import.UnLoad();
    },
    saveDirectMSGDocument: function () {
        if (Document_Import.params.RefCtrl == "Patient_MessageCompose" && Patient_MessageCompose.ImportFileData != "" && Patient_MessageCompose.ImportFileType != "") {
            Document_Import.ImportDirectMSGSave();
        }
        //Document_Import.UnLoad();
    },

    ImportDirectMSGSave: function () {
        if (Patient_MessageCompose.ImportFileData != "" && Patient_MessageCompose.ImportFileType != "") {
            //var objDefIportSave = $.Deferred();
            var fileCount = 1;//Document_Import.FilesContainer.Files.length;
            if ($('#' + Document_Import.panelID + " #hfPatientId").val() == "-1") {
                utility.DisplayMessages("Patient not Valid", 2);
                return false;
            }
            var data = new FormData();
            data.append("fileType", Patient_MessageCompose.ImportFileType);
            data.append("FileName", Patient_MessageCompose.ImportFileName);
            data.append("DirectMSG", Patient_MessageCompose.ImportFileData)
            data.append("PatientID", $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val());
            data.append("NotesId", Document_Import.params.NotesId);
            data.append("VisitId", Document_Import.params.VisitId);

            var self = $("#pnlDocumentImport");
            var myJSON = self.getMyJSON();
            // For multiple folder selection
            var FolderList = [];
            self.find('#divFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                var objectFolder = new Object();
                objectFolder.Value = this.value;
                objectFolder.Name = this.nextSibling.data.trim();
                FolderList.push(objectFolder);
            });
            if (FolderList.length > 0) {
                data.append("PatientDocumentData", myJSON);
                data.append("FolderList", JSON.stringify(FolderList));
                //objDefIportSave.then(function () {
                //End 03-05-2016 Humaira Yousaf
                Document_Import.SaveImport(data).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        UnloadActionPan(Document_Import.params.ParentCtrl, 'Document_Import');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                Document_Import.validateFolder(1);
            }
        }
        else {
            utility.DisplayMessages("Please select a valid file to upload.", 3);
        }
    },



    EnableAndDisableControls: function (active) {
        if (!active) {
            //$('#pnlDocumentImport #frmDocumentImport').find('input[type=text], textarea, select').each(function () {
            //    $(this).prop('disabled', true);
            //});
            $.each(document.getElementsByName('conditionalTag'), function (i, _this) {
                $(_this).hide();
            });
            $('#pnlDocumentImport #frmDocumentImport .conditional').css('float', 'right');
        }
        else {
            //$('#pnlDocumentImport #frmDocumentImport').find('input[type=text], textarea, select').find('a').each(function () {
            //    $(this).prop('disabled', false);
            //});
            $.each(document.getElementsByName('conditionalTag'), function (i, _this) {
                $(_this).show();
            });
        }
    },
    TruncateFileControl: function () {
        $("#" + Document_Import.params.PanelID + " #uploadFilePH").val('');
        $('#' + Document_Import.params.PanelID + ' #totalFiles').text("0 file(s) selected");
        $('#' + Document_Import.params.PanelID + ' #Upload_Import_file').val('');
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Document_Import';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val(PatientId);
        $("#" + Document_Import.params.PanelID + " #frmDocumentImport #txtAccountNumber").val(patFullName.split(' - ')[0].trim());
        Document_Import.params.PatientId = PatientId;
        Document_Import.params.FullName = patFullName.split(' - ')[1];
        Document_Import.params.AccountNumber = patFullName.split(' - ')[0].trim();
        Document_Import.LoadPatientCase(Number(PatientId));
        UnloadActionPan("Document_Import");
        utility.InsertRecentPatient(PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Document_Import.params.PanelID + " #frmDocumentImport #txtAccountNumber"), $("#" + Document_Import.params.PanelID + " #frmDocumentImport #txtAccountNumber").val(), $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId"), PatientId);
        //$('#frmDocumentImport').bootstrapValidator('revalidateField', 'AccountNumber');
        Document_Import.LoadPatientVisitDOS(PatientId);
    },
    BindPatientAccount: function () {
        var Ctrl = $('#' + Document_Import.params.PanelID + ' #txtAccountNumber');
        var hfCtrl = $("#" + Document_Import.params.PanelID + " #hfPatientId");
        var func = function () { return Document_Import.GetActivePatientsArray(Ctrl.val()); };
        var onSelect = function (e) {

            Document_Import.params.PatientId = e.id;
            Document_Import.params.FullName = e.FullName;
            Document_Import.params.AccountNumber = e.AccountNo;

            Document_Import.LoadPatientCase(e.id);
            utility.InsertRecentPatient(e.id);
            Document_Import.LoadPatientVisitDOS(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect);
    },

    GetActivePatientsArray: function (name) {
        var AllPatients = [];
        var IsAccountWithFullName = "0";
        var dfd = new $.Deferred();
        appointmentDetail.LoadActivePatients(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.PatientCount > 0) {
                    var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                    $.each(PatientLoadJSONData, function (i, item) {
                        if (IsAccountWithFullName != null && IsAccountWithFullName == "1") {
                            AllPatients.push({ id: item.PatientId, value: item.AccountNumber + " - " + item.FullName, AccountNumber: item.AccountNumber, FullName: item.FullName });
                        }
                        else {
                            AllPatients.push({ id: item.PatientId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName });
                        }
                    });
                }
            }
            dfd.resolve(AllPatients);
        });
        return dfd.promise();
    },

    LoadPatientCase: function (PatientId) {
        $('#' + Document_Import.params.PanelID + " input#txtCaseNumber").val('');
        $("#" + Document_Import.params.PanelID + " #hfCaseId").val('');
        CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
            var Ctrl = $('#' + Document_Import.params.PanelID + " input#txtCaseNumber");
            var hfCtrl = $("#" + Document_Import.params.PanelID + " #hfCaseId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", PatientCase, null, hfCtrl);

            if (Document_Import.params.ParentCtrl == "Patient_Document" && Document_Import.params.PatientDetail == "1") {
                if (Document_Import.params["CaseNo"] != null && Document_Import.params["CaseNo"] != "") {
                    $("#pnlDocumentImport #frmDocumentImport #txtCaseNumber").val(Document_Import.params.CaseNo);
                    $("#pnlDocumentImport #frmDocumentImport #hfCaseId").val(Document_Import.params.CaseId);
                }
            }
            //serialize data
            $('#frmDocumentImport').data('serialize', $('#frmDocumentImport').serialize());
        });

    },
    OpenCaseDetail: function (HiddenCtrl) {
        var params = [];
        params["CaseId"] = parseInt($('#' + Document_Import.params["PanelID"] + ' #' + HiddenCtrl).val());
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["PatientId"] = $("#" + Document_Import.params["PanelID"] + " #hfPatientId").val();
        params["RefCtrl"] = "txtCaseNumber";
        params["ParentCtrl"] = "Document_Import";
        LoadActionPan('Patient_Case_Detail', params);
    },
    OpenCase: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = Document_Import.params["TabID"];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["CaseId"] = "-1";
        params["patientID"] = $("#" + Document_Import.params.PanelID + " #hfPatientId").val();
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Document_Import";
        LoadActionPan('Patient_Case', params);
    },

    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'Document_Import';
        params["patientID"] = $("#" + Document_Import.params.PanelID + " #hfPatientId").val();
        LoadActionPan('Encounter_Visits', params);
        //if (Document_Import.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Document_Import.bVisitFirst = false;
        //}

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Document_Import.params.PanelID + " #txtClaimNumber").val(ClaimNumber);
        $("#" + Document_Import.params.PanelID + " #hfPatientId").val(PatientId);
        $("#" + Document_Import.params.PanelID + " #txtAccountNumber").val(AccountNumber);
        $("#" + Document_Import.params.PanelID + " #hfVisitId").val(VisitId);
        Document_Import.LoadPatientCase(PatientId);
        Encounter_Visits.UnLoad();
    },
    BindClaimNumber: function () {
        var Ctrl = $("#" + Document_Import.params.PanelID + " #txtClaimNumber");
        var hfId = $("#" + Document_Import.params.PanelID + " #hfVisitId");
        var func = function () { return Document_Import.GetClaimNumberArray(Ctrl.val()); };
        var onSelect = function (e) {
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Document_Import.params.PanelID + " #frmDocumentImport #txtAccountNumber"), e.PatientName, $("#" + Document_Import.params.PanelID + " #frmDocumentImport #hfPatientId"), e.PatientId);
            Document_Import.LoadPatientCase(e.PatientId);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfId, onSelect);
    },

    GetClaimNumberArray: function (ClaimNumber) {
        var AllClaimsVisits = [];
        var patientId = $("#" + Document_Import.params.PanelID + " #hfPatientId").val();
        var dfd = new $.Deferred();
        Document_Import.LoadClaimNumers(ClaimNumber, patientId).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                    });
                }
            }
            dfd.resolve(AllClaimsVisits);
        });
        return dfd.promise();
    },

    LoadClaimNumers: function (claimNumber, patientID) {
        var data = "ClaimNumber=" + claimNumber + "&PatientID=" + patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },
    LoadPatientVisitDOS: function (patientId) {
        if (patientId) {
            Document_Import.LoadVisitDOS(patientId).done(function (responce) {
                $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentImport #divddlDOS #ddlDOS").empty();
                $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentImport #divddlDOS #ddlDOS").append($("<option/>", {
                    value: "",
                    text: "-Select-"
                }));
                if (responce.TotalDOS > 0) {
                    $.each(responce.PatientDOS, function (k, item) {
                        $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #divddlDOS #ddlDOS").append($("<option/>", {
                            value: utility.RemoveTimeFromDate(null, item.DOSFrom),
                            text: utility.RemoveTimeFromDate(null, item.DOSFrom)
                        }));
                    });

                }

            });
        }

    },
    LoadVisitDOS: function (PatientID) {

        var data = "PatientID=" + PatientID;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "GET_VISIT_DOS");
    },
    SelectedDOSControl: function (obj) {
        if ($(obj).val() == "Calendar") {
            $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #dtpDOS").removeAttr("disabled");
            $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #ddlDOS").attr("disabled", "disabled");
            //$("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #ddlDOS").val("");
        }
        else {
            $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #dtpDOS").attr("disabled", "disabled");
            $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #ddlDOS").removeAttr("disabled");
            // $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #dtpDOS").val("");
        }
    },
    fileNameChange: function (e, obj) {
        var key = event.keyCode || event.charCode;
        var thisValue = $(obj).val();
        if (key == 8 || key == 46) {
            console.log("back space");
            if (thisValue[(($(obj).caret().start) - 1)] == ',') {
                e.preventDefault();
            }
        }
        else if (key == 18 || key == 188) {
            e.preventDefault();
        }
    },
    SaveDocPassword: function () {
        var DocPassword = {};
        if ($("#popDiv #TxtDocPassword").val() != "") {
            var PasswordMatch = $("#popDiv #TxtDocPassword").val() == $("#popDiv #TxtDocConfirmPassword").val() ? true : false;
            if (PasswordMatch) {
                var Selectedvalues = $("#popDiv").find("#DivShareAccess #ddlUsers").val();
                Selectedvalues = $.isArray(Selectedvalues) ? Selectedvalues.join() : Selectedvalues;
                DocPassword["SetPassword"] = $("#popDiv #TxtDocPassword").val();
                DocPassword["ConfirmPassword"] = $("#popDiv #TxtDocConfirmPassword").val();
                DocPassword["UserId"] = Selectedvalues;

                Document_Import.PasswordJSON = JSON.stringify(DocPassword);
                Document_Import.cancelConfirmDialog();
            } else {
                utility.DisplayMessages("Passwords entered do not match", 3)
            }
        } else {
            utility.DisplayMessages("Please enter Password.", 3);
        }

    },

    cancelConfirmDialog: function () {
        $("#modal-from-dom-DocumentPassword").modal('hide');
    },

    PasswordMatch: function () {
        if ($("#popDiv #TxtDocPassword").val() == $("#popDiv #TxtDocConfirmPassword").val()) {
            return true;
        } else {
            return false;
        }
    },
    AddTags: function () {
        var params = [];
        params["ParentCtrl"] = "Document_Import";
        params["TabID"] = Document_Import.params["TabID"];
        LoadActionPan('Patient_DocumentTag', params);
    },
    BindTagAutocomplete: function () {
        var Ctrl = $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #txtTagName");
        var hfCtrl = $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #hfDocumentTagId");
        var func = function () { return utility.GetDocumentTagArray(Ctrl.val(), 0) };
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfCtrl);
    },
    FillTagName: function (hfTagId, txtTagName) {
        $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #txtTagName").val(txtTagName);
        $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #hfDocumentTagId").val(hfTagId);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #txtTagName"), txtTagName, $("#" + Document_Import.params["PanelID"] + " #frmDocumentImport #hfDocumentTagId"), hfTagId);
        Patient_DocumentTag.UnLoad();
    },
}
