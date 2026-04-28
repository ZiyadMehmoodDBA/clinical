Clinical_OrderSetDetails = {
    bIsFirstLoad: true,
    params: [],
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    isDragged: false,
    isBelongToQgroup: false,
    EditableGridOrder: null,
    labOrderRows: [],
    labOrderRowsSent: [],
    labResultRows: [],
    OrderSetFollowUpCount: 0,
    ImmunizationTable: null,
    defaultFollowupId: null,
    ReferralsVisitTypes: ["Select", "Follow Up", "New Patient", "Physical", "Surgical Clearance", "Consult", "Office Visit",
        "Immunization", "Injection", "Blood Work", "Ultrasound", "Procedure", "Surgery", "Infusion", "Test"],

    Load: function (params) {
        Clinical_OrderSetDetails.params = params;
        Clinical_OrderSetDetails.ImmunizationTable = null;
        Clinical_OrderSetDetails.isFirstLoadProblems = true,
        Clinical_OrderSetDetails.isFirstLoadProcedure = true;
        Clinical_OrderSetDetails.isFirstLoadProcedureOrder = true;
        Clinical_OrderSetDetails.isFirstLoadLabOrders = true;
        Clinical_OrderSetDetails.isFirstLoadRadiologyOrders = true;
        Clinical_OrderSetDetails.isFirstLoadImmunizationOrders = true;
        Clinical_OrderSetDetails.isFirstLoadFollowUp = true;
        Clinical_OrderSetDetails.isFirstLoadPatientEducation = true;
        Clinical_OrderSetDetails.isFirstLoadReferrals = true;
        Clinical_OrderSetDetails.isFirstLoadTherapeuticOrders = true;
        Clinical_OrderSetDetails.isFirstLoadMedication = true;
        if (Clinical_OrderSetDetails.params["PanelID"] != 'pnlClinicalOrderSetDetails')
            Clinical_OrderSetDetails.params["PanelID"] = Clinical_OrderSetDetails.params["PanelID"] + ' #pnlClinicalOrderSetDetails';

        Clinical_OrderSetDetails.OrderSetFollowUpCount = 0;

        if (Clinical_OrderSetDetails.bIsFirstLoad) {
            Clinical_OrderSetDetails.bIsFirstLoad = false;
            selectedEntity = globalAppdata["SeletedEntityId"];
            var self = $('#' + Clinical_OrderSetDetails.params.PanelID);
            self.loadDropDowns(true).done(function () {
                Clinical_OrderSetDetails.ValidateOrderSetDetails();
                utility.callbackAfterAllDOMLoaded(function () {
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp input:checkbox[name="SelectCheckBoxFollowUp"]').on('change', function () {
                        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp input:checkbox[name="SelectCheckBoxFollowUp"]').not(this).prop('checked', false);
                    });
                });
                $.when(Clinical_OrderSetDetails.loadEntitySpecialty(selectedEntity)).then(function () {
                    Clinical_OrderSetDetails.loadEntityProvider(selectedEntity);
                });
                if (Clinical_OrderSetDetails.params.OrderSetId > 0) {

                    //AST-15 BY:MAhmad
                    //AST - 74 BY:MAhmad
                    Clinical_OrderSetDetails.loadProblemLookUp();
                    //AST - 74 BY:MAhmad
                    //AST-15 BY:MAhmad
                    if (typeof Clinical_OrderSetDetails.params.ShowSelectedProblems != typeof undefined && Clinical_OrderSetDetails.params.ShowSelectedProblems != null && Clinical_OrderSetDetails.params.ShowSelectedProblems) {
                        $('#' + Clinical_OrderSetDetails.params.PanelID + " #dvProblemList").addClass('disableAll');
                        Clinical_OrderSetDetails.LoadAssociatedProblems(Clinical_OrderSetDetails.params.SelectedProblems);
                        $('#' + Clinical_OrderSetDetails.params.PanelID + " #AssociatedProblems").removeClass('hidden');
                    }

                    Clinical_OrderSetDetails.OrderSetFill(Clinical_OrderSetDetails.params.OrderSetId);
                    Clinical_OrderSetDetails.LoadOrderSetReferrals();
                    Clinical_OrderSetDetails.LabOrderSearch(null, null, null, null, "Pending");
                    Clinical_OrderSetDetails.LoadOrderSetPatientEducation();
                    Clinical_OrderSetDetails.LoadOrderSetFollowUp();
                    OrderSet_ProblemListGrid.ProblemListsSearch(null, null, null, Clinical_OrderSetDetails.params["mode"], Clinical_OrderSetDetails.params["IsNotes"]);
                    OrderSet_ProceduresGrid.ProceduresSearch(null, null, null, Clinical_OrderSetDetails.params["mode"], Clinical_OrderSetDetails.params["IsNotes"]);
                    Clinical_OrderSetDetails.radiologyOrderSearch();
                    Clinical_OrderSetDetails.ImmunizationSearch();
                    Clinical_OrderSetDetails.TherapeuticSearch();
                    Clinical_OrderSetDetails.MedicationSearch();
                    Clinical_OrderSetDetails.ProcedureOrderSearch();
                }
                if (Clinical_OrderSetDetails.params["OSName"] != null) {
                    $("#" + Clinical_OrderSetDetails.params.PanelID + " h4").text(Clinical_OrderSetDetails.params["OSName"]);
                }
                if (Clinical_OrderSetDetails.params["mode"] == "View") {
                    $("#" + Clinical_OrderSetDetails.params.PanelID).find('button').not('.close').addClass('disableAll');
                    $("#" + Clinical_OrderSetDetails.params.PanelID + " #btnLabOrderDelete").removeClass('disableAll');
                    $("#" + Clinical_OrderSetDetails.params.PanelID).find('.disbleCustom').remove();
                    Clinical_OrderSetDetails.SetOrderSetForNotes();
                    if ((typeof Clinical_OrderSetDetails.params.DirectFromNotes != typeof undefined && Clinical_OrderSetDetails.params.DirectFromNotes != null && Clinical_OrderSetDetails.params.DirectFromNotes) || Clinical_OrderSetDetails.params.ParentCtrl == "Clinical_OrderSets" || Clinical_OrderSetDetails.params.CDSId != null) {
                        $("#" + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails").addClass('hidden');
                    } else {
                        $("#" + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails").removeClass('hidden');
                    }
                }
                else
                    $('#frmOrderSetDetails').data('serialize', $('#frmOrderSetDetails').serialize());
            });
            utility.callbackAfterAllDOMLoaded(function () {
                Clinical_OrderSetDetails.ExpandTheGrid();
            });

        }

    },
    ExpandTheGrid: function () {
        if (globalAppdata.IsExpand.toLowerCase() == "true") {
            $($("#" + Clinical_OrderSetDetails.params.PanelID).find($('[data-plugin-toggle]'))).each(function () {
                var $this = $(this);
                if (!$this.hasClass("PatientEducation")) {
                    if ($this.find("section div.toggle-content tbody tr").length == 1) {
                        if ($this.find("section div.toggle-content tbody tr td").length > 1) {
                            $this.find("section").addClass("active");
                            $this.find("section div.toggle-content").css("display", "block");
                            if (typeof $this.attr("onclick") != typeof undefined && $this.attr("onclick") != "") {
                                $this.trigger('onclick');
                            }
                            else {
                                $this.find("section").trigger('onclick');
                            }
                        }
                    }
                    else {
                        if ($this.find("section div.toggle-content tbody tr").length > 1) {
                            $this.find("section").addClass("active");
                            $this.find("section div.toggle-content").css("display", "block");
                            if (typeof $this.attr("onclick") != typeof undefined && $this.attr("onclick") != "") {
                                $this.trigger('onclick');
                            }
                            else {
                                $this.find("section").trigger('onclick');
                            }
                        }
                        else {

                        }
                    }
                }
                else {
                    var expand = false;
                    if ($this.find("section div.toggle-content #OSInfo tbody tr").length == 1) {
                        if ($this.find("section div.toggle-content #OSInfo tbody tr td").length > 1) {
                            $this.find("section").addClass("active");
                            $this.find("section div.toggle-content").css("display", "block");
                            expand = true;
                            $this.find("section").trigger('onclick');
                        }
                    }
                    else {
                        if ($this.find("section div.toggle-content #OSInfo tbody tr").length > 1) {
                            $this.find("section").addClass("active");
                            $this.find("section div.toggle-content").css("display", "block");
                            expand = true;
                            $this.find("section").trigger('onclick');
                        }
                        else {

                        }
                    }


                    if (!expand) {
                        if ($this.find("section div.toggle-content #OSNonInfo tbody tr").length == 1) {
                            if ($this.find("section div.toggle-content #OSNonInfo tbody tr td").length > 1) {
                                $this.find("section").addClass("active");
                                $this.find("section div.toggle-content").css("display", "block");
                                expand = true;
                                $this.find("section").trigger('onclick');
                            }
                        }
                        else {
                            if ($this.find("section div.toggle-content #OSNonInfo tbody tr").length > 1) {
                                $this.find("section").addClass("active");
                                $this.find("section div.toggle-content").css("display", "block");
                                expand = true;
                                $this.find("section").trigger('onclick');
                            }
                            else {

                            }
                        }
                    }


                }
            });
        }
    },
    clearOrderSet: function () {
        Clinical_ProgressNote.OSProblems_ComponentIds = [];
        Clinical_ProgressNote.OSProcedures_ComponentIds = [];
        Clinical_ProgressNote.OSLabOrder_ComponentIds = [];
        Clinical_ProgressNote.OSRadiologyOrder_ComponentIds = [];
        Clinical_ProgressNote.OSFollowUp_ComponentIds = [];
        Clinical_ProgressNote.OSPatientEducation_ComponentIds = [];
        Clinical_ProgressNote.OSReferrals_ComponentIds = [];
        Clinical_ProgressNote.OSImmunization_ComponentIds = [];
        Clinical_ProgressNote.OSTherapeutic_ComponentIds = [];
        Clinical_ProgressNote.OSMedication_ComponentIds = [];
        Clinical_ProgressNote.OSProcedureOrder_ComponentIds = [];
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').val('')
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection');
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect('refresh');
        Clinical_OrderSetDetails.specialityCheckedIds = ''

        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').val('')
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect('clearSelection');
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect('refresh');
        Clinical_OrderSetDetails.providerCheckedIds = ''

    },
    OrderSetFill: function (formId) {
        Clinical_OrderSetDetails.OrderSetFill_DBCall(formId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_OrderSetDetails.OrderSetFillData(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    OrderSetFill_DBCall: function (formId) {
        var objData = {};
        objData["OrderSetId"] = formId;
        objData["NotesId"] = Clinical_OrderSetDetails.params["NoteId"] ? Clinical_OrderSetDetails.params["NoteId"] : "";
        objData["CDSId"] = Clinical_OrderSetDetails.params["CDSId"] ? Clinical_OrderSetDetails.params["CDSId"] : "";
        objData["commandType"] = "FILL_ORDER_SET";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    loadOrderSetProblems: function (OrderSetProblems) {

        var dfd = $.Deferred();
        $('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList").empty();
        $(OrderSetProblems).each(function (index, item) {
            var li = '';

            li = "<li class='fromDB' icd9Code='" + item.ICD9 + "' icd9Desc='" + item.Icd9Description + "' icd10Code='" + item.ICD10 + "' icd10Desc='" + item.Icd10Description + "' snomedCode='" + item.SnomedId + "' snomedDesc='" + item.SnomedDescription + "' data='" + item.SnomedCode + "' id=" + item.OrderSetProblemId + " name='" + item.Problem + "' onclick='' \"><div class='col-sm-12 col-lg-12'><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='Clinical_OrderSetDetails.deleteProblemList($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList").append(li);

            if (index != 0) {
                if (item.ProblemOperator == 'AND') {
                    $($('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li#" + item.OrderSetProblemId).find("#ddlProblemList" + item.OrderSetProblemId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li#" + item.OrderSetProblemId).find("#ddlProblemList" + item.OrderSetProblemId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #txtOrderSetProblemList").val('');
        });
        dfd.resolve();
        return dfd;
    },
    LoadAssociatedProblems: function (OrderSetProblems) {
        $('#' + Clinical_OrderSetDetails.params.PanelID + " #AssociatedProblems #ulOrderSetAssociatedProblems").empty();
        $(OrderSetProblems).each(function (index, item) {
            var li = '';
            li = "<li class='fromDB' icd9Code='" + item.ICD9 + "' icd9Desc='" + item.Icd9Description + "' icd10Code='" + item.ICD10 + "' icd10Desc='" + item.Icd10Description + "' snomedCode='" + item.SnomedId + "' snomedDesc='" + item.SnomedDescription + "' data='" + item.SnomedCode + "' id=" + item.OrderSetProblemId + " name='" + item.Problem + "' onclick='' \"><div class='col-sm-12 col-lg-12'><a href='#'>" + item.Problem + "</a></div><div class='clearfix'></div></li>";
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #AssociatedProblems #ulOrderSetAssociatedProblems").append(li);
        });
    },
    OrderSetFillData: function (response) {
        var OrderSetData = response.listOrderSet;
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
        if ($('#' + Clinical_OrderSetDetails.params.PanelID + ' input#chkActive').hasClass('disableAll'))
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' input#chkActive').removeClass('disableAll');
        var self = $("#" + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails");
        if (OrderSetData != null && OrderSetData.length > 0) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #hfEntityId').val(OrderSetData[0].EntityId);
            Clinical_OrderSetDetails.params.OrderSetId = OrderSetData[0].OrderSetId;
            $("#" + Clinical_OrderSetDetails.params.PanelID).find('.rowlink').removeClass('disableAll');
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' input#txtOrderSetName').val(OrderSetData[0].OrderSetName);
            if (Clinical_OrderSetDetails.params["IsNotes"] == true && $("#pnlClinicalProgressNote #Cli_OrderSets_Main" + OrderSetData[0].OrderSetId + " ul").length > 0) {
                if ($("#pnlClinicalProgressNote #Cli_OrderSets_Main" + OrderSetData[0].OrderSetId + " ul").html().split('<br>').length > 1) {
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' textarea#txtComments').val($("#pnlClinicalProgressNote #Cli_OrderSets_Main" + OrderSetData[0].OrderSetId + " ul").html().split('<br>')[1].trim());
                }
            }
            else {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' textarea#txtComments').val(OrderSetData[0].Comments);
            }
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' input#chkActive').prop('checked', OrderSetData[0].IsActive == 'True' ? true : false);



            //set order set compunents check boxes
            // 1- unselect all.
            $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #orderSetCompunents").find("input:checked").each(function (i, item) {
                $(item).prop('checked', false);
            });

            //2-  set order set compunents check boxes for Note View
            if (OrderSetData[0].OrderSetComponents && OrderSetData[0].OrderSetComponents != "" && OrderSetData[0].OrderSetComponents != undefined) {
                $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #orderSetCompunents").find("input").each(function (i, item) {
                    if (OrderSetData[0].OrderSetComponents.indexOf($(item).attr('name')) >= 0)
                        $(item).prop('checked', true);
                });
            }
            else //3- select all
                $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #orderSetCompunents").find("input").each(function (i, item) {
                    $(item).prop('checked', true);
                });

            setTimeout(function () {


                if (OrderSetData[0].SpecialtyIds != "") {
                    EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty'), OrderSetData[0].SpecialtyIds);
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect("refresh");
                    var Specialties = OrderSetData[0].SpecialtyIds.split(",");
                    Clinical_OrderSetDetails.specialityCheckedIds = Specialties;
                    Clinical_OrderSetDetails.filterProvidersBySpecialtyIds();
                    Clinical_OrderSetDetails.IntializeMultiSelectDropDownProviders()

                }


                setTimeout(function () {
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
                    if (OrderSetData[0].ProviderIds != "") {
                        EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider'), OrderSetData[0].ProviderIds);
                        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect("refresh");
                        var Providers = OrderSetData[0].ProviderIds.split(",");
                        Clinical_OrderSetDetails.providerCheckedIds = Providers;
                    }
                }, 500);

            }, 1000);
            Clinical_OrderSetDetails.loadOrderSetProblems(response.listProblem);
        }


    },
    loadEntitySpecialty: function (entityID) {
        var objDeffered = $.Deferred();
        if (entityID != null && entityID > 0) {
            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {
                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').empty();
                    $.each(spacialties, function (i, item) {
                        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });
                    //Assign server side spacialties to the specialityCheckedIds array
                    if (Clinical_OrderSetDetails.SpecialtyIds != '') {
                        var Specialties = Clinical_OrderSetDetails.SpecialtyIds.split(",");
                        Clinical_OrderSetDetails.specialityCheckedIds = Specialties;
                        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                Clinical_OrderSetDetails.IntializeMultiSelectDropDownSpecialties();
                objDeffered.resolve();
            });
        }
        else {

            objDeffered.resolve();
        }
        return objDeffered;
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlHiddenProvider');

                $providerDdl.empty();
                $providerHiddenDdl.empty();

                //Loop through all providers loaded from the server
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {

                        // User will see these providers in multiSelect dropdownlist
                        $providerDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                        // Populate hidden ddl provider
                        //A Hack to load all the providers in hidden dropdownlist
                        $providerHiddenDdl.append(
                             $('<option/>', {
                                 value: item.Value,
                                 html: item.Name,
                                 refname: item.RefName,
                                 refvalue: item.RefValue

                             })
                        );
                    }
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (Clinical_OrderSetDetails.ProviderIds != '') {
                    var Providers = Clinical_OrderSetDetails.ProviderIds.split(",");
                    Clinical_OrderSetDetails.providerCheckedIds = Providers;
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #divSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.
                Clinical_OrderSetDetails.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect('destroy');
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_OrderSetDetails.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_OrderSetDetails.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_OrderSetDetails.ProviderIds != '') {
                        var Providers = Clinical_OrderSetDetails.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_OrderSetDetails.providerCheckedIds = Clinical_OrderSetDetails.removeFromArray(Clinical_OrderSetDetails.providerCheckedIds, item);
                                Clinical_OrderSetDetails.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').val(Clinical_OrderSetDetails.providerCheckedIds);
                    Clinical_OrderSetDetails.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_OrderSetDetails.SpecialtyIds != '') {
                    var spacialties = Clinical_OrderSetDetails.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_OrderSetDetails.specialityCheckedIds = Clinical_OrderSetDetails.removeFromArray(Clinical_OrderSetDetails.specialityCheckedIds, item);
                            Clinical_OrderSetDetails.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_OrderSetDetails.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').val(Clinical_OrderSetDetails.specialityCheckedIds);
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty').multiselect('refresh')
            },
        });
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_OrderSetDetails.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
            },


        });
        //$('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect('selectAll', false);
        //$('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');        
    },
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlHiddenProvider';

        var providerContext = '#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider';
        $(providerContext).empty();

        if (Clinical_OrderSetDetails.specialityCheckedIds.length > 0) {

            $.each(Clinical_OrderSetDetails.specialityCheckedIds, function (index, specialtyId) {

                $(providerHiddenContext).find('option').each(function (index, option) {
                    if ($(option).attr('refname') == specialtyId) {
                        $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
                    }
                });
            });
        }
        else {
            $(providerHiddenContext).find('option').each(function (index, option) {
                $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
            });
        }
    },
    setSpacialtiesByselectedProviderIds: function () {
        if (Clinical_OrderSetDetails.providerCheckedIds.length > 0) {
            $.each(Clinical_OrderSetDetails.providerCheckedIds, function (index, item) {

                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider option').each(function () {
                    if ($(this).val() != '') {
                        if ($(this).val() == item) {
                            Clinical_OrderSetDetails.specialityCheckedIds = Clinical_OrderSetDetails.removeFromArray(Clinical_OrderSetDetails.specialityCheckedIds, $(this).attr('refname'));
                            Clinical_OrderSetDetails.specialityCheckedIds.push($(this).attr('refname'));
                        }
                    }
                });
            });
        }
        else {
            Clinical_OrderSetDetails.specialityCheckedIds = [];
        }
    },
    checkSpecialtiesByProviderId: function (option, checked, select) {
        //provider context
        var providerContext = '#' + Clinical_OrderSetDetails.params.PanelID + ' #divProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            Clinical_OrderSetDetails.providerCheckedIds = [];
            Clinical_OrderSetDetails.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            Clinical_OrderSetDetails.providerCheckedIds = [];
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlProvider option').each(function () {
                var providerValue = $(this).val();
                Clinical_OrderSetDetails.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Clinical_OrderSetDetails.providerCheckedIds = Clinical_OrderSetDetails.removeFromArray(Clinical_OrderSetDetails.providerCheckedIds, providerValue);
                Clinical_OrderSetDetails.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Clinical_OrderSetDetails.providerCheckedIds = Clinical_OrderSetDetails.removeFromArray(Clinical_OrderSetDetails.providerCheckedIds, $(option).val());
            }

        }
    },
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + Clinical_OrderSetDetails.params.PanelID + ' #divSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_OrderSetDetails.specialityCheckedIds = [];
            Clinical_OrderSetDetails.providerCheckedIds = [];
            Clinical_OrderSetDetails.ProviderIds = '';
            Clinical_OrderSetDetails.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_OrderSetDetails.specialityCheckedIds = Clinical_OrderSetDetails.removeFromArray(Clinical_OrderSetDetails.specialityCheckedIds, spacialityId);
                    Clinical_OrderSetDetails.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_OrderSetDetails.specialityCheckedIds = Clinical_OrderSetDetails.removeFromArray(Clinical_OrderSetDetails.specialityCheckedIds, spacialityId);
                }
            }
            else {
                Clinical_OrderSetDetails.specialityCheckedIds = [];
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #ddlSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Clinical_OrderSetDetails.specialityCheckedIds.push(spacialityId);
                });
            }
        }
    },
    uncheckProblems: function () {
        if (Clinical_OrderSetDetails.isFirstLoadProblems == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProblemListsOS input[name=SelectCheckBoxProbList]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadProblems = false;
        }


    },
    uncheckProcedures: function () {
        if (Clinical_OrderSetDetails.isFirstLoadProcedure == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProceduresOS input[name=SelectCheckBoxProcedure]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadProcedure = false;
        }
    },
    uncheckProcedureOrders: function () {
        if (Clinical_OrderSetDetails.isFirstLoadProcedureOrder == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProcedureOrder input[name=SelectCheckBoxProcedureOrder]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadProcedureOrder = false;
        }
    },
    uncheckLabOrders: function () {
        if (Clinical_OrderSetDetails.isFirstLoadLabOrders == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvLabOrder input[name=SelectCheckBoxLabOrder]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadLabOrders = false;
        }
    },
    uncheckRadiologyOrders: function () {
        if (Clinical_OrderSetDetails.isFirstLoadRadiologyOrders == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvRadiologyOrder input[name=SelectCheckBoxRadiologyOrder]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadRadiologyOrders = false;
        }

    },
    uncheckImmunizationOrders: function () {
        if (Clinical_OrderSetDetails.isFirstLoadImmunizationOrders == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS input[name=SelectCheckBoxImmunization]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadImmunizationOrders = false;
        }

    },
    uncheckTherapeuticOrders: function () {
        if (Clinical_OrderSetDetails.isFirstLoadTherapeuticOrders == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvTherapeuticOS input[name=SelectCheckBoxTherapeutic]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadTherapeuticOrders = false;
        }
    },
    uncheckFollowUp: function () {
        if (Clinical_OrderSetDetails.isFirstLoadFollowUp == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp input[name=SelectCheckBoxFollowUp]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadFollowUp = false;
        }
    },
    uncheckReferrals: function () {
        if (Clinical_OrderSetDetails.isFirstLoadReferrals == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetReferrals input[name=SelectCheckBoxReferrals]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadReferrals = false;
        }
    },
    uncheckPatientEducation: function () {
        if (Clinical_OrderSetDetails.isFirstLoadPatientEducation == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetNonInfoDocument input[name=SelectCheckBoxPatientEducation]').prop("checked", false);
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetInfoDocument input[name=SelectCheckBoxInfo]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadPatientEducation = false;
        }
    },
    uncheckMedications: function () {
        if (Clinical_OrderSetDetails.isFirstLoadMedication == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvMedicationsOS input[name=SelectCheckBoxMedication]').prop("checked", false);
            Clinical_OrderSetDetails.isFirstLoadMedication = false;
        }
    },
    
    UnLoadTab: function () {
        if (Clinical_OrderSetDetails.params["ParentCtrl"] != "clinicalTabProgressNote" && Clinical_OrderSetDetails.params["ParentCtrlPanelID"] != "pnlClinicalProgressNote") {
            var isDefaultFollowUpSelected = true;
            var defaultFollowupId = null;
            if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").length > 0 && $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr")[0].innerText != "No Order Set FollowUp Found") {
                isDefaultFollowUpSelected = false;
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").each(function (index, item) {
                    var input = $(item).find('td')[6];
                    if ($(input).find('input').prop('checked')) {
                        isDefaultFollowUpSelected = true;
                        defaultFollowupId = $(item).attr('followupid');
                    }
                });
                if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").length > 1 && isDefaultFollowUpSelected == false) {
                    utility.DisplayMessages("Kindly select a default follow up in order to proceed", 3);
                    return;
                } else {
                    Clinical_OrderSetDetails.OrderSetDetailsSave();
                }
            }
        }
        Clinical_OrderSetDetails.clearOrderSet();
        if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabProgressNote") {
            utility.myConfirmNote('1', function () {
                Clinical_OrderSetDetails.AddOrderSetToNote(true);
                if (typeof Clinical_OrderSetDetails.params.DirectFromNotes != typeof undefined && Clinical_OrderSetDetails.params.DirectFromNotes != null && Clinical_OrderSetDetails.params.DirectFromNotes) {
                    UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails', null, "pnlClinicalProgressNote");
                    EMRUtility.scrollToPNcomponent('clinical_ordersets');
                }
                else {
                    UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails', null, "pnlClinicalProgressNote #pnlClinicalOrderSets");
                    EMRUtility.scrollToPNcomponent('clinical_ordersets');
                }

            }, "", function () {
                if (typeof Clinical_OrderSetDetails.params.DirectFromNotes != typeof undefined && Clinical_OrderSetDetails.params.DirectFromNotes != null && Clinical_OrderSetDetails.params.DirectFromNotes) {
                    UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails', null, "pnlClinicalProgressNote");
                    Clinical_ProgressNote.ResetDefaultOrderSet();
                    EMRUtility.scrollToPNcomponent('clinical_ordersets');
                }
                else {
                    UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails', null, "pnlClinicalProgressNote #pnlClinicalOrderSets");
                    EMRUtility.scrollToPNcomponent('clinical_ordersets');
                }
            });
        }
        else {
            if (Clinical_OrderSetDetails.params["IsFormNotes"])
                UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails', null, "pnlClinicalProgressNote #pnlClinicalOrderSets");
            else {
                UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails');
                Clinical_OrderSets.OrderSetsSearch();
            }
        }
        //Clinical_OrderSetDetails.params = null;
        Clinical_OrderSetDetails.defaultFollowupId = null;
    },
    removeFromArray: function (array, removeItem) {
        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },
    OrderSetDetailsSave: function () {
        if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").length == 1 && $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr")[0].innerText != "No Order Set FollowUp Found") {
            var onlyRow = $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr")[0];
            var onlyRowTd = $(onlyRow).find('td')[6];
            $(onlyRowTd).find('input').trigger('click');
        }
        var isDefaultFollowUpSelected = true;
        var defaultFollowupId = null;
        if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").length > 0 && $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr")[0].innerText != "No Order Set FollowUp Found") {
            isDefaultFollowUpSelected = false;
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").each(function (index, item) {
                var input = $(item).find('td')[6];
                if ($(input).find('input').prop('checked')) {
                    isDefaultFollowUpSelected = true;
                    defaultFollowupId = $(item).attr('followupid');
                }

            });
            if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").length > 1 && isDefaultFollowUpSelected == false) {
                utility.DisplayMessages("Kindly select a default follow up in order to proceed", 3);
                return;
            }
        }
        var strMessage = "";
        var self = $("#" + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails");
        var isValid = Clinical_OrderSetDetails.ValidateSpecialty(self);
        if (isValid) {
            var myJSON = self.getMyJSONByName();
            if (Clinical_OrderSetDetails.params.mode == "Add") {
                Clinical_OrderSetDetails.SaveOrderSetDetails(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var canvasCol = $("#" + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ddlCanvasCol option:selected").val();
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_OrderSetDetails.params.OrderSetId = response.OrderSetId;
                        Clinical_OrderSetDetails.params.mode = "Edit";
                        $("#" + Clinical_OrderSetDetails.params.PanelID).find('.rowlink').removeClass('disableAll');
                        $.when(Clinical_OrderSetDetails.LoadOrderProblems()).then(function () {
                            $('#frmOrderSetDetails').data('serialize', $('#frmOrderSetDetails').serialize());
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else if (Clinical_OrderSetDetails.params.mode == "Edit") {
                var selectedSchTypeId = $("#" + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ddlScheduleType option:selected").val();
                Clinical_OrderSetDetails.UpdateOrderSetDetails(myJSON, Clinical_OrderSetDetails.params.VaccineGroupId, defaultFollowupId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_OrderSetDetails.params.mode = "Edit";
                        $.when(Clinical_OrderSetDetails.LoadOrderProblems()).then(function () {
                            $('#frmOrderSetDetails').data('serialize', $('#frmOrderSetDetails').serialize());
                        });

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else {
            utility.DisplayMessages("Please select Specialty(s).", 3);
        }
    },
    LoadOrderProblems: function () {
        var dfd = $.Deferred();
        Clinical_OrderSetDetails.OrderSetFill_DBCall(Clinical_OrderSetDetails.params.OrderSetId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Clinical_OrderSetDetails.loadOrderSetProblems(response.listProblem)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    ValidateSpecialty: function (self) {
        var isValid = true;
        $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlSpecialty").val() ? $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlSpecialty").val().join() : ''
        var icds = self.find('#ddlSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        if (icds == "")
            isValid = false;

        return isValid;
    },
    ValidateOrderSetDetails: function () {
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #frmOrderSetDetails')
        .bootstrapValidator('destroy');
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #frmOrderSetDetails')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   OrderSetName: {
                       group: '.col-sm-4',
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
            Clinical_OrderSetDetails.OrderSetDetailsSave();
        });
    },
    SaveOrderSetDetails: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["SpecialtyIds"] = $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlSpecialty").val() ? $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlSpecialty").val().join() : '';
        objData["ProviderIds"] = $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlProvider").val() ? $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlProvider").val().join() : '';
        objData["Comments"] = $('#' + Clinical_OrderSetDetails.params.PanelID + " #txtComments").val();
        var problemData = [];
        $('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li").each(function (index, item) {
            var problem = null;
            if (index == 0) {
                problem = {
                    Problem: $(this).find('a').text(),
                    SnomedCode: $(this).attr('data'),
                    ProblemOperator: '',
                    OrderSetProblemId: $(this).attr('id'),
                    ICD9: $(this).attr('icd9code'),
                    Icd9Description: $(this).attr('icd9desc'),
                    ICD10: $(this).attr('icd10code'),
                    Icd10Description: $(this).attr('icd10desc'),
                    SnomedId: $(this).attr('snomedcode'),
                    SnomedDescription: $(this).attr('snomeddesc')
                };
            }
            else {
                problem = {
                    Problem: $(this).find('a').text(),
                    SnomedCode: $(this).attr('data'),
                    ProblemOperator: $(this).find('#ddlProblemList' + $(this).attr('id')).val(),
                    OrderSetProblemId: $(this).attr('id'),
                    ICD9: $(this).attr('icd9code'),
                    Icd9Description: $(this).attr('icd9desc'),
                    ICD10: $(this).attr('icd10code'),
                    Icd10Description: $(this).attr('icd10desc'),
                    SnomedId: $(this).attr('snomedcode'),
                    SnomedDescription: $(this).attr('snomeddesc')
                };
            }
            problemData.push(problem);
        });
        objData["AssociatedProblemData"] = problemData;
        objData["commandType"] = "SAVE_ORDER_SET";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    UpdateOrderSetDetails: function (jsonData, vaccineId, defaultFollowupId) {
        var objData = JSON.parse(jsonData);
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["SpecialtyIds"] = $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlSpecialty").val() ? $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlSpecialty").val().join() : '';
        objData["ProviderIds"] = $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlProvider").val() ? $('#' + Clinical_OrderSetDetails.params.PanelID + " #ddlProvider").val().join() : '';
        objData["Comments"] = $('#' + Clinical_OrderSetDetails.params.PanelID + " #txtComments").val();
        objData["DefaultFollowUpId"] = defaultFollowupId;
        var problemData = [];
        $('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li").each(function (index, item) {
            var problem = null;
            if (index == 0) {
                problem = {
                    Problem: $(this).find('a').text(),
                    SnomedCode: $(this).attr('data'),
                    ProblemOperator: '',
                    OrderSetProblemId: $(this).attr('id'),
                    ICD9: $(this).attr('icd9code'),
                    Icd9Description: $(this).attr('icd9desc'),
                    ICD10: $(this).attr('icd10code'),
                    Icd10Description: $(this).attr('icd10desc'),
                    SnomedId: $(this).attr('snomedcode'),
                    SnomedDescription: $(this).attr('snomeddesc')
                };
            }
            else {
                problem = {
                    Problem: $(this).find('a').text(),
                    SnomedCode: $(this).attr('data'),
                    ProblemOperator: $(this).find('#ddlProblemList' + $(this).attr('id')).val(),
                    OrderSetProblemId: $(this).attr('id'),
                    ICD9: $(this).attr('icd9code'),
                    Icd9Description: $(this).attr('icd9desc'),
                    ICD10: $(this).attr('icd10code'),
                    Icd10Description: $(this).attr('icd10desc'),
                    SnomedId: $(this).attr('snomedcode'),
                    SnomedDescription: $(this).attr('snomeddesc')
                };
            }
            problemData.push(problem);
        });

        objData["AssociatedProblemData"] = problemData;
        objData["commandType"] = "UPDATE_ORDER_SET";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },

    OpenOrderSetPatientEducation: function (event, mode, OrderSetPatEducationId) {

        if (event != null) {
            event.stopPropagation();

        }

        var params = [];
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        params["FromAdmin"] = Clinical_OrderSetDetails.params["FromAdmin"];
        if (Clinical_OrderSetDetails.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        }

        if (mode == 'Edit') {
            params["mode"] = 'Edit';
            params["OrderSetPatEducationId"] = OrderSetPatEducationId;
            params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        }
        else {
            params["mode"] = "Add";
            params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        }

        LoadActionPan("OrderSet_PatientEducation", params);

    },

    OpenOrderSetReferrals: function (event, mode, OrderSetReferralId) {

        if (event != null) {
            event.stopPropagation();
            if ($(event.target).is('input[type=checkbox]')) {
                return;
            }
        }

        var params = [];
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        params["FromAdmin"] = Clinical_OrderSetDetails.params["FromAdmin"];
        if (Clinical_OrderSetDetails.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        }

        if (mode == 'Edit') {
            params["mode"] = 'Edit';
            params["OrderSetReferralId"] = OrderSetReferralId;
            params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        }
        else {
            params["mode"] = "Add";
            params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        }

        LoadActionPan("OrderSet_Patient_Referrals_Outgoing_Detail", params);

    },

    DeleteOrderSetReferrals: function (OrderSetReferralId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            var selectedValue = OrderSetReferralId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_OrderSetDetails.DeleteOrderSetReferrals_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_OrderSetDetails.LoadOrderSetReferrals();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );

    },

    DeleteOrderSetReferrals_DBCall: function (OrderSetReferralId) {

        var objData = {};
        objData["OrderSetReferralId"] = OrderSetReferralId;
        objData["commandType"] = "delete_ordersetreferral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientReferralsOutgoingDetail");

    },


    LoadOrderSetReferrals: function () {

        Clinical_OrderSetDetails.LoadOrderSetReferrals_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_OrderSetDetails.OrderSetReferralsFillData(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    OrderSetReferralsFillData: function (response) {

        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxReferrals" id="chkHeaderReferrals" onchange="Clinical_OrderSetDetails.checkUncheckAllReferrals(this);"  class="input-block" coltype="checkbox"/> </th>');
        }
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetReferrals').dataTable().fnDestroy();
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals tbody").find("tr").remove();
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals").parent().parent().find('div.row').remove();

        if (response.OrderSetReferralsCount > 0) {
            var listOrderSetReferrals_JSON = response.OrderSetReferralsJSON;
            $.each(listOrderSetReferrals_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this));Clinical_OrderSetDetails.OpenOrderSetReferrals(event,'Edit','" + item.OrderSetReferralId + "');");
                $row.attr("id", "gvOrderSetReferrals_row" + item.OrderSetReferralId);
                $row.attr("OrderSetReferralId", item.OrderSetReferralId);
                $row.attr("OrderSetId", item.OrderSetId);
                var date_ = utility.DateTemplate(item.Date) + " " + item.Time;
                var visitType = item.Visits && item.Visits != 0 ? Clinical_OrderSetDetails.ReferralsVisitTypes[item.Visits] : "";
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddReferrals(this,event);" id="' + item.OrderSetReferralId + '" name="SelectCheckBoxReferrals"  class="input-block text-center"/></td>';
                }

                $row.append(SelectionCheckBoxColumn + '<td style="display:none" >' + item.OrderSetReferralId + '</td>'
                    + '<td class="Actions_"><a class="btn btn-xs" href="#" onclick="Clinical_OrderSetDetails.DeleteOrderSetReferrals(\'' + item.OrderSetReferralId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_OrderSetDetails.OpenOrderSetReferrals(event,\'Edit\',\'' + item.OrderSetReferralId + '\');" title="Edit Record"><i class="fa fa-edit black"></i></a></td>'
                    + '<td>' + date_ + '</td>'
                    + '<td>' + item.Procedures + '</td>'
                    + '<td>' + item.RefProvider + '</td>'
                    + '<td>' + item.SpecialtyFromName + '</td>'
                    + '<td>' + item.ProviderName + '</td>'
                    //+ '<td>' + item.FacilityFromName + '</td>'
                    + '<td>' + visitType + '</td>'
                    + '<td>' + item.Assignee + '</td>');
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals tbody").last().append($row);
            });
            if (Clinical_OrderSetDetails.params["mode"] == "View") {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetReferrals').find('a').addClass('disableAll');
            }
            if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals"))
                ;
            else {
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals").DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            }
        }
        else {
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals").DataTable({
                "language": {
                    "emptyTable": "No Order Set Referral Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }
        if (Clinical_OrderSetDetails.params["IsNotes"])
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetReferrals').find('td.Actions_').addClass('hidden');

        setTimeout(function () {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetReferrals input[name=SelectCheckBoxReferrals]').prop("checked", false);
        }, 200);

    },

    checkUncheckAllReferrals: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxReferrals']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxReferrals']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetReferrals tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddReferrals(this);
        });
    },

    enableAddReferrals: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSReferrals_ComponentIds) == -1) {
                Clinical_ProgressNote.OSReferrals_ComponentIds.push(obj.id);
            }
        } else {
            var index = Clinical_ProgressNote.OSReferrals_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSReferrals_ComponentIds.splice(index, 1);
            }
        }


    },
    enableAddProcedureOrOrder: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSProcedureOrder_ComponentIds) == -1) {
                Clinical_ProgressNote.OSProcedureOrder_ComponentIds.push(obj.id);
            }
        } else {
            var index = Clinical_ProgressNote.OSProcedureOrder_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSProcedureOrder_ComponentIds.splice(index, 1);
            }
        }


    },
    LoadOrderSetReferrals_DBCall: function () {
        var objData = {};
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["commandType"] = "fill_OrderSetReferral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientReferralsOutgoingDetail");
    },
    OpenOrderSetLabOrder: function () {
        var params = [];
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        params["mode"] = "Add";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        params["FromAdmin"] = Clinical_OrderSetDetails.params["FromAdmin"];
        if (Clinical_OrderSetDetails.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        }
        LoadActionPan("OrderSet_LabOrder", params);

    },
    LabOrderSearch: function (LabId, PageNo, rpp, caller, OrderStatus) {
        var strMessage = "";
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvLabOrdertSent" + " th#selectRecordOrders").remove();

        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result").css("display") == "none") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + Clinical_OrderSetDetails.params.PanelID + " form");

                Clinical_OrderSetDetails.searchLabOrder(null, LabId, PageNo, rpp, OrderStatus).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (OrderStatus != "Signed") {
                            Clinical_OrderSetDetails.LabGridLoad(response);
                            var TableControl = Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder";
                            var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + " #dgvLabOrder_Paging";
                            var ClassControlName = "Clinical_OrderSetDetails";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            Clinical_OrderSetDetails.pending = "Pending";
                            setTimeout(
                                CreatePagination(response.LabOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                    Clinical_OrderSetDetails.LabOrderSearch(null, PageNumber, ResultPerPage, null, Clinical_OrderSetDetails.pending);
                                }), 10);
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
    },
    searchLabOrder: function (LabOrderData, LabOrderId, PageNumber, RowsPerPage, OrderStatus) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (LabOrderData != null) {
            objData = JSON.parse(LabOrderData);
        }
        objData["LabOrderId"] = LabOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;

        if (OrderStatus != null) {
            if (OrderStatus == "Pending") {
                objData["Status"] = "Pending";
            }
            else {
                objData["Status"] = "Signed";
            }
        }
        else {
            objData["Status"] = "Pending";
        }
        objData["commandType"] = "search_laborders_dashboard";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },
    LabGridLoad: function (response) {



        Clinical_OrderSetDetails.labOrderRows = [];

        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder")) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder").dataTable().fnClearTable();
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder").dataTable().fnDestroy();
        }
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder tbody").find("tr").remove();
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder thead tr #SelectRecord").length == 0 ) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxLabOrder" id="chkHeaderLabOrder" onchange="Clinical_OrderSetDetails.checkUncheckAllLabOrder(this);"  class="input-block" coltype="checkbox"/> </th>');
        } else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder thead tr #SelectRecord").prop('checked', false);
        }
        if (response.LabOrderCount > 0) {
            var LabLoadJSONData = JSON.parse(response.LabOrderFill_JSON);
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvLab_row" + item.LabOrderId);
                $row.attr("LabId", item.LabOrderId);
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var onclick = "Clinical_OrderSetDetails.labOrderRowExpand(this,event);";
                var onCellClick = "Clinical_OrderSetDetails.labOrderRowExpand(this,event,null, 'cell');";
                var Checked = "";
                var editmode = 'onclick="Clinical_OrderSetDetails.LabEdit(\'' + item.LabOrderId + '\',event);"';
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                var SelectionCheckBoxColumn = "";
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="pull-left mt-default" onchange="Clinical_OrderSetDetails.enableAddLabOrder(this);" id="' + item.LabOrderId + '" name="SelectCheckBoxLabOrder"  class="input-block text-center"/></td>';
                    if (item.Status == "Signed") {
                        $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.LabOrderId + '</td><td ' + editmode + '>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td ' + editmode + '>' + item.LabName + '</td><td ' + editmode + '>' + item.OrderNo + '</td><td ' + editmode + '>' + item.Status + '</td><td ' + editmode + '>' + item.AssigneeName + '</td>');

                    }
                    else {
                        $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.LabOrderId + '</td><td ' + editmode + '>'
                            + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td ' + editmode + '>' + item.LabName + '</td><td ' + editmode + '>' + item.OrderNo + '</td><td ' + editmode + '>' + item.Status + '</td><td ' + editmode + '>' + item.AssigneeName + '</td>');
                    }
                } else {
                    SelectionCheckBoxColumn = '<input type="checkbox" class="pull-left mt-default" id="' + item.LabOrderId + '" name="SelectCheckBoxLabOrder"  class="input-block text-center"/>';
                    if (item.Status == "Signed") {
                        $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td class="Actions_">' + SelectionCheckBoxColumn + ' title="Edit Record"><i class="fa fa-edit black"></i></a></td><td ' + editmode + '>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td ' + editmode + '>' + item.LabName + '</td><td ' + editmode + '>' + item.OrderNo + '</td><td ' + editmode + '>' + item.Status + '</td><td ' + editmode + '>' + item.AssigneeName + '</td>');

                    }
                    else {
                        $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td class="Actions_">' + SelectionCheckBoxColumn + '</td><td ' + editmode + '>'
                            + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td ' + editmode + '>' + item.LabName + '</td><td ' + editmode + '>' + item.OrderNo + '</td><td ' + editmode + '>' + item.Status + '</td><td ' + editmode + '>' + item.AssigneeName + '</td>');
                    }
                }


                $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder" + " tbody").last().append($row);
                var childRows = Clinical_OrderSetDetails.buildLabOrderRowChild(item.LabOrderTests, item.LabOrderId);
                Clinical_OrderSetDetails.labOrderRows.push({ row: $row, childs: childRows });
                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);
                });
            });
            if (Clinical_OrderSetDetails.params["mode"] == "View") {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvLabOrder').find('a').addClass('disableAll');
            }

            if (Clinical_OrderSetDetails.params["IsNotes"])
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvLabOrder').find('td.Actions_').addClass('hidden');

        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + ' #pnlLabOrder_Result #dgvLabOrder').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + ' #pnlLabOrder_Result #dgvLabOrder'))
            ;
        else {
            Clinical_OrderSetDetails.EditableGridOrder = $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder").DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1], "orderable": false, "aTargets": [0] }], "aaSorting": [[2, "desc"]]
            }); // to remove records per page dropdown
        }

        setTimeout(function () {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvLabOrder input[name=SelectCheckBoxLabOrder]').prop("checked", false);
        }, 200);
        EMRUtility.fixDataTableDuplication("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result");

        $.each(Clinical_OrderSetDetails.labOrderRows, function (i, item) {

            if (Clinical_OrderSetDetails.EditableGridOrder != null) {

                var row = Clinical_OrderSetDetails.EditableGridOrder.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                }
            }
        });
    },

    checkUncheckAllLabOrder: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxLabOrder']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxLabOrder']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvLabOrder tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddLabOrder(this);
        });
    },

    enableAddLabOrder: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSLabOrder_ComponentIds) == -1) {
                Clinical_ProgressNote.OSLabOrder_ComponentIds.push(obj.id);
            }

        } else {

            var index = Clinical_ProgressNote.OSLabOrder_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSLabOrder_ComponentIds.splice(index, 1);
            }

        }


    },
    LabEdit: function (LabOrderId, event) {
        //if icon is clicked then  popup the window
        if (event != null) {
            event.stopPropagation();
        }
        Clinical_OrderSetDetails.LabOrderAddEdit(LabOrderId);

    },
    RadiologyOrderAddEdit: function (radiologyOrderId, event) {
        if (event != null) {
            if ($(event.target).hasClass('toggleEditableHeader')) {
                return;
            }
            else {
                event.stopPropagation();
            }
        }
        var strMessage = "";
        var permissionState = radiologyOrderId != null && parseInt(radiologyOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
                if (radiologyOrderId != null && parseInt(radiologyOrderId) > 0) {
                    params["RadiologyOrderId"] = radiologyOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyOrderId"] = -1;
                    params["mode"] = "Add";
                }

                params["FromAdmin"] = Clinical_OrderSetDetails.params["FromAdmin"];

                if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabProgressNote") {

                    params["ParentCtrl"] = 'Clinical_OrderSetDetails';
                    params["ParentCtrlPanelID"] = Clinical_OrderSetDetails.params.PanelID;
                    LoadActionPan('OrderSet_RadiologyOrderDetails', params, Clinical_OrderSetDetails.params.PanelID);

                }
                else if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabFaceSheet") {
                    params["ParentCtrl"] = 'Clinical_OrderSetDetails';
                    params["ParentCtrlPanelID"] = Clinical_OrderSetDetails.params.PanelID;
                    LoadActionPan('OrderSet_RadiologyOrderDetails', params, Clinical_OrderSetDetails.params.PanelID);
                }
                else {
                    params["ParentCtrl"] = 'Clinical_OrderSetDetails';
                    LoadActionPan('OrderSet_RadiologyOrderDetails', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    ProcedureOrderAddEdit: function (ProcedureOrderId, event) {
        if (event != null) {
            if ($(event.target).hasClass('toggleEditableHeader')) {
                return;
            }
            else {
                event.stopPropagation();
            }
        }
        var strMessage = "";
        var permissionState = ProcedureOrderId != null && parseInt(ProcedureOrderId) > 0 ? "EDIT" : "ADD";
        //AppPrivileges.GetFormPrivileges("Orders and Results_Radiology Order", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var params = [];
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        if (ProcedureOrderId != null && parseInt(ProcedureOrderId) > 0) {
            params["ProcedureOrderId"] = ProcedureOrderId;
            params["mode"] = "Edit";
        }
        else {
            params["ProcedureOrderId"] = -1;
            params["mode"] = "Add";
        }

        params["FromAdmin"] = Clinical_OrderSetDetails.params["FromAdmin"];

        if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabProgressNote") {

            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
            params["ParentCtrlPanelID"] = Clinical_OrderSetDetails.params.PanelID;
            LoadActionPan('OrderSet_ProcedureOrderDetails', params, Clinical_OrderSetDetails.params.PanelID);

        }
        else if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
            params["ParentCtrlPanelID"] = Clinical_OrderSetDetails.params.PanelID;
            LoadActionPan('OrderSet_ProcedureOrderDetails', params, Clinical_OrderSetDetails.params.PanelID);
        }
        else {
            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
            LoadActionPan('OrderSet_ProcedureOrderDetails', params);
        }
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    radiologyOrderSearch: function (radiologyId, PageNo, rpp, caller) {
        var strMessage = "";
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvRadiologyOrder th#selectRecordOrders").remove();
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result").css("display") == "none") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + Clinical_OrderSetDetails.params.PanelID + " form");
                var myJSON = self.getMyJSONByName();
                // search specific on caller Id
                if (caller != null) {
                    if (caller.indexOf("RadiologyOrderDetail") >= 0) {
                        myJSON = null;
                        // Reload Providers
                        Clinical_OrderSetDetails.fillProvider('frmOrderSetDetails');
                    }
                }
                Clinical_OrderSetDetails.searchRadiologyOrder(myJSON, radiologyId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_OrderSetDetails.radiologyGridLoad(response);
                        var totalRows = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvRadiologyOrder tr").length;
                        totalRows -= 1;
                        var selectedRows = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvRadiologyOrder tbody tr input:checked").length;
                        if (totalRows == selectedRows) {
                            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvRadiologyOrder #selectRecordOrders").prop("checked", true);
                        }
                        else {
                            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvRadiologyOrder #selectRecordOrders").prop("checked", false);
                        }
                        var TableControl = Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder";
                        var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + " #dgvRadiologyOrder_Paging";
                        var ClassControlName = "Clinical_OrderSetDetails";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.radiologyOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_OrderSetDetails.radiologyOrderSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //Function Name: radiologyGridLoad
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Loads Radiology Orders data in grid
    //Params: response
    radiologyGridLoad: function (response) {

        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxRadiologyOrder" id="chkHeaderRadiologyOrder" onchange="Clinical_OrderSetDetails.checkUncheckAllRadiologyOrder(this);"  class="input-block" coltype="checkbox"/> </th>');
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder")) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder").dataTable().fnClearTable();
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder").dataTable().fnDestroy();
        }
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder tbody").find("tr").remove();

        if (response.radiologyOrderCount > 0) {
            var radiologyLoadJSONData = JSON.parse(response.RadiologyLoad_JSON); //Parsing array to JSON
            $.each(radiologyLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvRadiology_row" + item.RadiologyOrderId);
                $row.attr("RadiologyId", item.RadiologyOrderId);
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddRadiologyOrder(this);" id="' + item.RadiologyOrderId + '" name="SelectCheckBoxRadiologyOrder"  class="input-block text-center"/></td>';
                }

                var Checked = "";
                if (item.Status != "Signed") {
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td class="Actions_"><a class="btn btn-xs" href="#" onclick="Clinical_OrderSetDetails.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_OrderSetDetails.radiologyEdit(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.AssigneeName + '</td>');
                }
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder tbody").last().append($row);
            });
            if (Clinical_OrderSetDetails.params["mode"] == "View") {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvRadiologyOrder').find('a').addClass('disableAll');
            }

            if (Clinical_OrderSetDetails.params["IsNotes"])
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvRadiologyOrder').find('td.Actions_').addClass('hidden');
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + ' #pnlRadiologyOrder_Result #dgvRadiologyOrder').DataTable({
                "language": {
                    "emptyTable": "No Diagnostic Imaging Order found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + ' #pnlRadiologyOrder_Result #dgvRadiologyOrder'))
            ;
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result #dgvRadiologyOrder").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
        setTimeout(function () {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvRadiologyOrder input[name=SelectCheckBoxRadiologyOrder]').prop("checked", false);
        }, 200);
        EMRUtility.fixDataTableDuplication("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlRadiologyOrder_Result");
    },

    checkUncheckAllRadiologyOrder: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxRadiologyOrder']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxRadiologyOrder']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvRadiologyOrder tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddRadiologyOrder(this);
        });
    },

    enableAddRadiologyOrder: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSRadiologyOrder_ComponentIds) == -1) {
                Clinical_ProgressNote.OSRadiologyOrder_ComponentIds.push(obj.id);
            }

        } else {

            var index = Clinical_ProgressNote.OSRadiologyOrder_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSRadiologyOrder_ComponentIds.splice(index, 1);
            }
        }


    },
    //Function Name: searchRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Radiology Orders
    //Params: radiologyOrderData, radiologyOrderId, PageNumber, RowsPerPage
    searchRadiologyOrder: function (radiologyOrderData, radiologyOrderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (radiologyOrderData != null)
            objData = JSON.parse(radiologyOrderData);

        objData["RadiologyOrderId"] = radiologyOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["commandType"] = "search_radiologyorders";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },
    labOrderRowExpand: function ($row, event, gridType, from) {

        event.stopPropagation();
        event.preventDefault();
        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');
        var $actions,
         values = [];
        var row = null;
        if (gridType != null) {
            row = Clinical_OrderSetDetails.EditableGridOrderSent.row($row);
        }
        else {
            row = Clinical_OrderSetDetails.EditableGridOrder.row($row);
        }
        if (row.child.isShown()) {
            $row.find("td:eq(3) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
        }
        else {
            $row.find("td:eq(3) .fa-plus-square").attr("class", "fa fa-minus-square");
            row.child.show();
        }
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder" + " tbody tr").each(function (i, item) {
            if (currentRowId != $(item).attr('id')) {
                var allotherrows = Clinical_OrderSetDetails.EditableGridOrder.row(item);
                if (allotherrows.child.isShown()) {
                    $(item).find(".fa-minus-square").attr("class", "fa fa-plus-square");
                    allotherrows.child.hide();

                }
            }
        });
    },
    buildLabOrderRowChild: function (tests, labOrderId) {

        var CurrentRowchilds = $();
        var templateHtml = $("#" + Clinical_OrderSetDetails.params.PanelID + " #LabOrderTemplate").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabOrderTemplate').find('tbody');
            $.each(tests, function (i, item) {
                var i = 1;
                do {
                    var $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    if (i == 1) {
                        $ChilRowDetail.append('<td class="bold" colspan="2" >' + item.CPTCode + " " + item.CPTDescription + '</td> <td></td>');
                    }
                    else {
                    }
                    $tbody.append($ChilRowDetail);
                    i++;
                } while (i <= 2)
            });
        }

        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="9">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);

    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Delete the selected Lab Order
    LabOrderDelete: function (event) {
        var strMessage = "";
        event.stopPropagation();
        var LabId = "";

        event.preventDefault();
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any order to delete', 4);
            return;
        } else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlLabOrder_Result  #dgvLabOrder tbody tr input:checked").each(function (i, item) {

                LabId += "," + $(item).attr('id');
            });
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_OrderSetDetails.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();
                        OrderSet_LabOrder.deleteLabOrder(myJSON, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_OrderSetDetails.LabOrderSearch(null, null, null, null, "Pending");
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
    //Author: Abid Ali
    //Date :  31-03-2016
    //This function will handle Add/Edit of LabOrder
    LabOrderAddEdit: function (LabOrderId, ParentCtrl, event, ParentCtrlPanelID) {
        if (event != null) {
            if ($(event.target).hasClass('toggleEditableHeader')) {
                return;
            }
            else {
                event.stopPropagation();
            }
        }
        var strMessage = "";
        var permissionState = LabOrderId != null && parseInt(LabOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
                if (LabOrderId != null && parseInt(LabOrderId) > 0) {
                    params["LabOrderId"] = LabOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabOrderId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_OrderSetDetails.params["FromAdmin"] != null ? Clinical_OrderSetDetails.params["FromAdmin"] : "0";
                if (ParentCtrl != null && ParentCtrl != "") {
                    params["ParentCtrl"] = ParentCtrl;
                    params["ParentCtrlPanelID"] = ParentCtrlPanelID;//DashBoard.params.PanelID;
                    params["PanelID"] = ParentCtrlPanelID;
                    LoadActionPan('OrderSet_LabOrderDetails', params, ParentCtrlPanelID);
                }
                else if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_OrderSetDetails';
                    params["ParentCtrlPanelID"] = Clinical_OrderSetDetails.params.PanelID;
                    LoadActionPan('OrderSet_LabOrderDetails', params, Clinical_OrderSetDetails.params.PanelID);
                }
                else if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabFaceSheet") {
                    params["ParentCtrl"] = 'Clinical_OrderSetDetails';
                    params["ParentCtrlPanelID"] = Clinical_OrderSetDetails.params.PanelID;
                    LoadActionPan('OrderSet_LabOrderDetails', params, Clinical_OrderSetDetails.params.PanelID);
                }

                else {
                    params["ParentCtrl"] = 'Clinical_OrderSetDetails';
                    LoadActionPan('OrderSet_LabOrderDetails', params);
                }
                //params["TabID"] = 'Clinical_OrderSetDetails';


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    LoadOrderSetPatientEducation: function () {

        Clinical_OrderSetDetails.LoadOrderSetPatientEducation_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_OrderSetDetails.OrderSetInfoPEDocFillData(response);
                Clinical_OrderSetDetails.OrderSetNonInfoPEDocFillData(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },


    LoadOrderSetPatientEducation_DBCall: function (pageNumber, rowsPerPage) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 1000;
        }
        var param = {};
        param["PageNumber"] = pageNumber;
        param["RowsPerPage"] = rowsPerPage;
        param["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        param["commandType"] = "load_ordersetpatienteducation";
        var data = JSON.stringify(param);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientEducation");

    },

    OrderSetNonInfoPEDocFillData: function (response) {

        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetNonInfoDocument")) {
            $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetNonInfoDocument").dataTable().fnClearTable();
            $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetNonInfoDocument").dataTable().fnDestroy();
        }
        $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetNonInfoDocument tbody").find("tr").remove();

        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetNonInfoDocument thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetNonInfoDocument thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxPatientEducation" id="chkHeaderPatientEducation" onchange="Clinical_OrderSetDetails.checkUncheckAllPatientEducation(this);"  class="input-block" coltype="checkbox"/> </th>');
        }

        if (response.NonInfoCount > 0) {
            var listOrderSetPatientEducation_JSON = response.NonInfoDocumentLoad_JSON;
            $.each(listOrderSetPatientEducation_JSON, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "dgvOrderSetNonInfo_row" + item.OrderSetPatEducationId);
                $row.attr("OrderSetPatEducationId", item.OrderSetPatEducationId);
                $row.attr("OrderSetId", item.OrderSetId);
                var mode_ = "Edit";
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddPatientEducation(this,event);" id="' + item.OrderSetPatEducationId + '" name="SelectCheckBoxPatientEducation"  class="input-block text-center"/></td>';
                }

                $row.append(SelectionCheckBoxColumn + '<td style="display:none" >' + item.OrderSetPatEducationId + '</td>'
                    + '<td class="Actions_"><a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.DocumentDelete(\'' + item.OrderSetPatEducationId + '\', \'' + item.DocId + '\', event   );" title="Delete Record"><i class="fa fa-close red"></i></a>'//<a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.OpenOrderSetPatientEducation(event,\'' + mode_ + '\',\'' + item.OrderSetPatEducationId + '\');" title="Edit Record"><i class="fa fa-edit black"></i></a></td>'
                    + '<td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td>'
                    + '<td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td>'
                    + '<td>' + item.CreatedByName + '</td>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvOrderSetNonInfo_row" + item.OrderSetPatEducationId + "'));Clinical_OrderSetDetails.DocumentPreview('" + item.DocId + "', '" + item.OrderSetPatEducationId + "', event);");
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetNonInfoDocument tbody").last().append($row);
            });
            if (Clinical_OrderSetDetails.params["mode"] == "View") {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetNonInfoDocument').find('a').addClass('disableAll');
            }
            if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetNonInfoDocument"))
                ;
            else {
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetNonInfoDocument").DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            }

            if (Clinical_OrderSetDetails.params["IsNotes"])
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetNonInfoDocument').find('td.Actions_').addClass('hidden');
        }
        else {
            if (!$("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetNonInfoDocument").parent().parent().hasClass("dataTables_wrapper")) {
                $('#' + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetNonInfoDocument").DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            } else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Document Found</td>');
                $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetNonInfoDocument tbody").last().append($row);
            }
        }

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        setTimeout(function () {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetNonInfoDocument input[name=SelectCheckBoxPatientEducation]').prop("checked", false);
        }, 200);
    },

    OrderSetInfoPEDocFillData: function (response) {

        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument")) {
            $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument").dataTable().fnClearTable();
            $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument").dataTable().fnDestroy();
        }
        $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument tbody").find("tr").remove();

        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetInfoDocument thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetInfoDocument thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxInfo" id="chkInfoDocAll" onchange="Clinical_OrderSetDetails.checkUncheckAllInfoDocs(this);"  class="input-block" coltype="checkbox"/> </th>');
        }
        if (response.InfoCount > 0) {
            var infoDocumentLoadJsonData = response.InfoDocumentLoad_JSON;
            $.each(infoDocumentLoadJsonData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "dgvOrderSetInfo_row" + item.OrderSetPatEducationId);
                $row.attr("OrderSetPatEducationId", item.OrderSetPatEducationId);
                $row.attr("OrderSetId", item.OrderSetId);
                var mode_ = "Edit";
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddPatientEducation(this,event);" id="' + item.OrderSetPatEducationId + '" name="SelectCheckBoxInfo"  class="input-block text-center"/></td>';
                }

                $row.append(SelectionCheckBoxColumn + '<td style="display:none" >' + item.OrderSetPatEducationId + '</td>'
                   + '<td class="Actions_"><a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.DocumentDelete(\'' + item.OrderSetPatEducationId + '\', \'' + item.DocId + '\', event   );" title="Delete Record"><i class="fa fa-close red"></i></a>'
                   + '<td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FilePath + '">' + item.FilePath + '</td>'
                   + '<td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td>'
                   + '<td>' + item.CreatedByName + '</td>');

                $row.attr("onclick", "utility.SelectGridRow($('#dgvOrderSetInfo_row" + item.OrderSetPatEducationId + "'));Clinical_OrderSetDetails.DocumentPreview('" + item.DocId + "', '" + item.OrderSetPatEducationId + "', event);");
                $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument tbody").last().append($row);
            });

            if (Clinical_OrderSetDetails.params["mode"] == "View") {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetInfoDocument').find('a').addClass('disableAll');
            }
            if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetInfoDocument"))
                ;
            else {
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetInfoDocument").DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            }
            if (Clinical_OrderSetDetails.params["IsNotes"])
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetInfoDocument').find('td.Actions_').addClass('hidden');
        }
        else {
            if (!$("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument").parent().parent().hasClass("dataTables_wrapper")) {
                $('#' + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument").DataTable({
                    "language": {
                        "emptyTable": "No Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            } else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" >No Document Found</td>');
                $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #dgvOrderSetInfoDocument tbody").last().append($row);
            }
        }

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        setTimeout(function () {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetInfoDocument input[name=SelectCheckBoxInfo]').prop("checked", false);
        }, 200);

    },

    checkUncheckAllPatientEducation: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxPatientEducation']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxPatientEducation']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetNonInfoDocument tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddPatientEducation(this);
        });
    },

    enableAddPatientEducation: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSPatientEducation_ComponentIds) == -1) {
                Clinical_ProgressNote.OSPatientEducation_ComponentIds.push(obj.id);
            }
        } else {
            var index = Clinical_ProgressNote.OSPatientEducation_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSPatientEducation_ComponentIds.splice(index, 1);
            }
        }


    },
    checkUncheckAllInfoDocs: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxInfo']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxInfo']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetInfoDocument tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddPatientEducation(this);
        });
    },

    DocumentDelete: function (OrderSetPatEducationId, DocId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm("1", function () {
            OrderSet_PatientEducation.DocumentDeleteDb(OrderSetPatEducationId, DocId).done(function (response) {
                response = JSON.parse(response);
                if (response.status !== false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_OrderSetDetails.LoadOrderSetPatientEducation();
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
            });

        });

    },
    //Function Name: fillProvider
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will fill Provider dropdown
    fillProvider: function (formId) {

        Clinical_OrderSetDetails.fillproviderDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var $ddlProvider = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #' + formId + ' #txtProvider');
                $ddlProvider.empty();

                $.each(response, function (i, item) {
                    $ddlProvider.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });
            }
        });
    },
    //Function Name: fillProvider
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will call DB to fill Provider dropdown
    fillproviderDBCall: function () {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "getorderingprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");

    },
    radiologyEdit: function (radiologyOrderId, event) {
        //if icon is clicked then  popup the window
        Clinical_OrderSetDetails.RadiologyOrderAddEdit(radiologyOrderId);
        event.stopPropagation();
    },
    //Function Name: radiologyOrderDelete
    //Author Name: Ahmad Raza
    //Created Date: 22-03-2016
    //Description: Delete the selected Radiology Order
    radiologyOrderDelete: function (radiologyId, event, PageNo, rpp) {
        var strMessage = "";
        event.stopPropagation();
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = radiologyId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_OrderSetDetails.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        Clinical_OrderSetDetails.deleteRadiologyOrder(myJSON, radiologyId, PageNo, rpp).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_OrderSetDetails.radiologyOrderSearch("", 1, 15);
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

    //Function Name: deleteRadiologyOrder
    //Author Name: Ahmad Raza
    //Created Date: 22-03-2016
    //Description: DB Call to Delete the selected Radiology Order
    deleteRadiologyOrder: function (radiologyData, radiologyId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(radiologyData);
        objData["RadiologyOrderId"] = radiologyId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["commandType"] = "delete_radiologyorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");

    },

    OpenOrderSetFollowUp: function (event, mode, FollowUpId) {

        if (event != null) {
            event.stopPropagation();
        }


        var params = [];
        var IsToOpen = true;
        var Message_ = "";
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        params["FromAdmin"] = Clinical_OrderSetDetails.params["FromAdmin"];
        if (Clinical_OrderSetDetails.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        }

        if (mode == 'Edit') {
            params["mode"] = 'Edit';
            params["FollowUpId"] = FollowUpId;
            params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        }
        else {



            params["mode"] = "Add";
            params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        }

        if (IsToOpen)
            LoadActionPan("OrderSet_FollowUp", params);
        else
            utility.DisplayMessages(Message_, 3);

    },

    LoadOrderSetFollowUp: function (PrimaryID, PageNo, rpp) {

        OrderSet_FollowUp.FillOrderSetFollowUp_DBCall(0, Clinical_OrderSetDetails.params.OrderSetId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_OrderSetDetails.OrderSetFollowUpFillData(response);
                var TableControl = Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp';
                var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + ' #dgvFollowUpPagination';
                var ClassControlName = "Clinical_OrderSetDetails";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.OrderSetFollowUpCount, 1, 1000, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_OrderSetDetails.LoadOrderSetFollowUp(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    OrderSetFollowUpFillData: function (response) {

        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox">  </th>');
        }

        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp').dataTable().fnDestroy();
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody").find("tr").remove();
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp").parent().parent().find('div.row').remove();
        Clinical_OrderSetDetails.OrderSetFollowUpCount = Number(response.OrderSetFollowUpCount);
        var defaultFollowUpId = null;
        if (response.OrderSetFollowUpCount > 0) {
            var listOrderSetFollowUpJSON = response.OrderSetFollowUpJSON;
            $.each(listOrderSetFollowUpJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this));");
                $row.attr("id", "gvOrderSetFollowUp_row" + item.FollowUpId);
                $row.attr("FollowUpId", item.FollowUpId);
                $row.attr("OrderSetId", item.OrderSetId);
                var mode_ = "Edit";
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    if (OrderSet_FollowUp.params.FollowUpId && OrderSet_FollowUp.params.FollowUpId == item.FollowUpId)
                        SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddFollowUp(this,event);" id="' + item.FollowUpId + '" name="SelectCheckBoxFollowUp"  class="input-block text-center" checked/></td>';
                    else
                        SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddFollowUp(this,event);" id="' + item.FollowUpId + '" name="SelectCheckBoxFollowUp"  class="input-block text-center"/></td>';
                }
                var isCreateAppointment = "No";
                if (item.CreateAppointment == "True") {
                    isCreateAppointment = "Yes";
                }
                var isDefault = "";
                var noteFlowIsDefault = "No";
                if (item.IsDefault == "True") {
                    isDefault = "checked='checked'";
                    noteFlowIsDefault = "Yes";
                    defaultFollowUpId = item.FollowUpId;
                    Clinical_OrderSetDetails.defaultFollowupId = item.FollowUpId;

                }

                var defaultColumnHtml = '<td><div class="radio-custom"><input onchange="Clinical_OrderSetDetails.checkUncheckRadios(\'' + item.FollowUpId + '\')" type="radio" id="rdoOther' + item.FollowUpId + '" ' + isDefault + ' name="Default" ><label class="control-label"></label></div></td>';
                if (Clinical_OrderSetDetails.params.ParentCtrl == "clinicalTabProgressNote" || Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {

                    defaultColumnHtml = '<td>' + noteFlowIsDefault + '</td>'
                }
                $row.append(SelectionCheckBoxColumn + '<td style="display:none" >' + item.FollowUpId + '</td>'
                    + '<td class="Actions_"><a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.OrderSetFollowUpDelete(\'' + item.FollowUpId + '\', event   );" title="Delete Record"><i class="fa fa-close red"></i></a><a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.OpenOrderSetFollowUp(event,\'' + mode_ + '\',\'' + item.FollowUpId + '\');" title="Edit Record"><i class="fa fa-edit black"></i></a></td>'

                    + '<td>' + item.FollowUpText + '</td>'
                    + '<td>' + isCreateAppointment + '</td>'
                    + '<td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '     ' + item.ModifiedByName + '</td>'
                    + '<td  class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>'
                + defaultColumnHtml);

                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody").last().append($row);
            });

            if (Clinical_OrderSetDetails.params["CustomMode"] == "View") {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp').find('a').addClass('disableAll');
                var checkBoxRow = $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr#gvOrderSetFollowUp_row" + defaultFollowUpId + " td")[0];
                $(checkBoxRow).find('input').addClass('disableAll');
            }

            if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp"))
                ;
            else {
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp").DataTable({
                    "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1, 6] }]
                }); // to remove records per page dropdown
            }


            if (Clinical_OrderSetDetails.params["IsNotes"])
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp').find('td.Actions_').addClass('hidden');
        }
        else {
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp").DataTable({
                "language": {
                    "emptyTable": "No Order Set FollowUp Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1, 6] }]
            });
        }
        utility.callbackAfterAllDOMLoaded(function () {

            if (Clinical_OrderSetDetails.defaultFollowupId == null) {
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").each(function (index, item) {
                    var input = $(item).find('td')[6];

                    $(input).find('input').prop('checked', false);


                });
            }
            if (Clinical_OrderSetDetails.params.ParentCtrl == "clinicalTabProgressNote" || Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                if (defaultFollowUpId) {
                    var checkBoxRow = $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr#gvOrderSetFollowUp_row" + defaultFollowUpId + " td")[0];
                    $(checkBoxRow).find('input').trigger('click');
                    $(checkBoxRow).find('input').prop('checked', 'checked');
                    $(checkBoxRow).find('input').attr('checked', true);
                    var checkedId = $(checkBoxRow).find('input').attr('id');
                    if ($.inArray(checkedId, Clinical_ProgressNote.OSFollowUp_ComponentIds) == -1) {
                        Clinical_ProgressNote.OSFollowUp_ComponentIds.push(checkedId);
                    }

                }
            } else {
                if (defaultFollowUpId) {
                    var checkRow = $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr#gvOrderSetFollowUp_row" + defaultFollowUpId + " td")[6];
                    $(checkRow).find('input').trigger('click');
                    $(checkRow).find('input').prop('checked', 'checked');
                    $(checkRow).find('input').attr('checked', true);
                }
            }
        });
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        //setTimeout(function () {
        //    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvOrderSetFollowUp input[name=SelectCheckBoxFollowUp]').prop("checked", false);
        //}, 200);
    },
    checkUncheckRadios: function (followUpId) {
        $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").each(function (index, item) {
            var input = $(item).find('td')[6];
            $(input).find('input').attr('checked', false);

        });
        var checkRow = $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr#gvOrderSetFollowUp_row" + followUpId + " td")[6];
        $(checkRow).find('input').prop('checked', 'checked');
        $(checkRow).find('input').attr('checked', true);

    },
    checkUncheckAllFollowUp: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxFollowUp']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxFollowUp']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddFollowUp(this);
        });
    },

    enableAddFollowUp: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if (event != null) {
            event.preventDefault();
        }
        Clinical_ProgressNote.OSFollowUp_ComponentIds = [];
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSFollowUp_ComponentIds) == -1) {
                Clinical_ProgressNote.OSFollowUp_ComponentIds.push(obj.id);
            }

        } else {
            var index = Clinical_ProgressNote.OSFollowUp_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSFollowUp_ComponentIds.splice(index, 1);
            }
        }


    },

    OrderSetFollowUpDelete: function (FollowUpId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm("1", function () {
            OrderSet_FollowUp.FollowUpAppointmentDelete_DBCall(FollowUpId).done(function (response) {
                response = JSON.parse(response);
                if (response.status !== false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_OrderSetDetails.LoadOrderSetFollowUp();
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
            });

        });

    },
    OpenSaveAsWindow: function () {
        var ConfirmDiv = $("#" + Clinical_OrderSetDetails.params.PanelID + " #divSaveAs");
        if (Clinical_OrderSetDetails.params.OrderSetId > 0) {
            Clinical_OrderSetDetails.myConfirm(function () {
                var orderSetName = $("#popDiv").find("input");
                if (orderSetName.val() == "") {
                    orderSetName.focus();
                    orderSetName.css("border", "1px solid red");
                    utility.DisplayMessages('Please enter order set name.', 2);
                }
                else {
                    orderSetName.css("border", "");
                    Clinical_OrderSetDetails.SaveAsOrderSetDetails(orderSetName.val()).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status == true) {
                            $("#modal-from-dom-SaveAs").modal('hide');
                            Clinical_OrderSets.OrderSetsSearch();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () {
                if ($("#modal-from-dom-SaveAs")) {
                    $("#modal-from-dom-SaveAs").remove();
                }
                return false;
            });
        }
        else {
            utility.DisplayMessages("Please save order set first.", 2);
        }
    },
    myConfirm: function (okFunc, cancelFunc) {
        if ($("#modal-from-dom-SaveAs")) {
            $("#modal-from-dom-SaveAs").remove();
        }
        var ConfirmDiv = $("#" + Clinical_OrderSetDetails.params.PanelID + " #divSaveAs");
        var dialogText = "Save As";
        var dialogTitle = "Save As";

        okBtnTitle = "Create";
        cancelBtnTitle = "Cancel";

        Clinical_OrderSetDetails.okFunc = okFunc;
        Clinical_OrderSetDetails.cancelFunc = cancelFunc;

        var markUp = '';
        markUp = '<div id="modal-from-dom-SaveAs" class="modal fade "> <div class="modal-dialog modal-dialog-smd modal-top-adjust"><div class="modal-content">' +
                 '<div class="modal-header">' +
                 '<button type="button" onclick="Clinical_OrderSetDetails.cancelConfirmDialog();"  class="close" ">' +
                 '<span class=" red">&nbsp;</span><span class="sr-only">Close</span></button>' +
                         '<h4 class="modal-title">' + dialogTitle + ' </h4></div><div class="modal-body" id="popDiv" style="min-height:80px;">' + ConfirmDiv.html() + '<div classs=clearfix></div></div>' +
                  '</div> <div></div>';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {

        }).on('hidden.bs.modal', function () {
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });
    },
    cancelConfirmDialog: function () {
        $("#modal-from-dom-SaveAs").modal('hide');
    },
    saveConfirmDialog: function () {
        if (true) {
            if (typeof (Clinical_OrderSetDetails.okFunc) == 'function') {
                setTimeout(Clinical_OrderSetDetails.okFunc, 50);
            }
            $('body').addClass('modal-open');
        }
    },
    SaveAsOrderSetDetails: function (OrderSetName) {
        var objData = {};
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["OrderSetName"] = OrderSetName;
        var defaultFollowupId = null;
        if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").length > 0) {
            isDefaultFollowUpSelected = false;
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvOrderSetFollowUp tbody tr").each(function (index, item) {
                var input = $(item).find('td')[6];
                if ($(input).find('input').prop('checked')) {
                    isDefaultFollowUpSelected = true;
                    defaultFollowupId = $(item).attr('followupid');
                }

            });
            if (isDefaultFollowUpSelected == false) {
                utility.DisplayMessages("Kindly select a default follow up in order to proceed", 3);
                return;
            }
        }
        objData["DefaultFollowUpId"] = defaultFollowupId;
        objData["commandType"] = "SAVEAS_ORDER_SET";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },


    AddOrderSetToNote: function (isUnload) {
        var Comments = $('#' + Clinical_OrderSetDetails.params.PanelID + " #txtComments").val();
        if (Clinical_ProgressNote.OSImmunization_ComponentIds.length > 0) {
            OrderSet_ImmunizationDetail.IsVaccineHxInValidAge(Clinical_ProgressNote.OSImmunization_ComponentIds.join(','), $('#PatientProfile #hfPatientId').val()).done(function (result) {
                if (result == "0") {
                    Clinical_OrderSetDetails.AddOrderSetToNoteNew(isUnload, false, Comments);
                }
                else if (result == "1") {
                    utility.myConfirm('49', function () {
                        Clinical_OrderSetDetails.AddOrderSetToNoteNew(isUnload, true, Comments);
                    }, function () {
                        Clinical_OrderSetDetails.AddOrderSetToNoteNew(isUnload, false, Comments);
                    },
                        '1'
                    );
                }
                else {
                    Clinical_OrderSetDetails.AddOrderSetToNoteNew(isUnload, false, Comments);
                }
            });
        }
        else {
            Clinical_OrderSetDetails.AddOrderSetToNoteNew(isUnload, false, Comments);
        }

        if (Clinical_OrderSetDetails.params.OrderSetType == "defaultOrderSet") {
            Clinical_OrderSetDetails.detachDefaultOrderSetFromNotes();
        }

    },
    AddOrderSetToNoteNew: function (isUnload, AddInValidAgeRecordsInHxTab, Comments) {
        //$('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfOSComponents').val(componentsToRemove);

        var checked_OrderSetComponents = [];
        $("#" + Clinical_OrderSetDetails.params["PanelID"] + " #orderSetCompunents").find("input:checked").each(function (i, item) {
            checked_OrderSetComponents.push($(item).attr('name'));
        });
        var SelectedProblems = false;
        if (typeof Clinical_OrderSetDetails.params.ShowSelectedProblems != typeof undefined && Clinical_OrderSetDetails.params.ShowSelectedProblems != null && Clinical_OrderSetDetails.params.ShowSelectedProblems) {
            if (Clinical_OrderSetDetails.params.SelectedProblems) {
                SelectedProblems = true;
            }
        }
        if (checked_OrderSetComponents.length > 0 || SelectedProblems) {
            var components_ = checked_OrderSetComponents.join(",");
            Clinical_OrderSetDetails.AddOrderSetToNoteDBCAll(components_).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    // call add order set to Note function.
                    Clinical_OrderSetDetails.AddToNoteDBCall(components_, "", "", "", AddInValidAgeRecordsInHxTab).done(function (inner_response) {

                        inner_response = JSON.parse(inner_response);
                        if (inner_response.status != false) {
                            if (inner_response.ExistsMedicationsDrugName != "") {
                                utility.DisplayMessages(inner_response.ExistsMedicationsDrugName + " already Exists", 3);
                            }
                            $("#mainForm  li#CDSAlert").show();
                            $.when(ClinicalCDSDetail.showCDSAlert("", Clinical_OrderSetDetails.params.PatientId)).then(function () {
                                if (Clinical_OrderSetDetails.params.ParentCtrl == "clinicalTabProgressNote" || Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote")
                                    Clinical_ProgressNote.LoadCDSAlerts();
                            });
                            var OrderSetName = $("#" + Clinical_OrderSetDetails.params.PanelID + " h4").text();
                            Clinical_ProgressNote.LoadOrderSetToNote(inner_response, OrderSetName, Clinical_OrderSetDetails.params.OrderSetId, true, Comments);
                            if (!isUnload) {
                                if (Clinical_OrderSetDetails.params["ParentCtrl"] == "clinicalTabProgressNote") {
                                    Clinical_OrderSetDetails.clearOrderSet();
                                    if (typeof Clinical_OrderSetDetails.params.DirectFromNotes != typeof undefined && Clinical_OrderSetDetails.params.DirectFromNotes != null && Clinical_OrderSetDetails.params.DirectFromNotes) {
                                        UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails', null, "pnlClinicalProgressNote");
                                        EMRUtility.scrollToPNcomponent('clinical_ordersets');
                                    }
                                    else {
                                        UnloadActionPan(Clinical_OrderSetDetails.params.ParentCtrl, 'Clinical_OrderSetDetails', null, "pnlClinicalProgressNote #pnlClinicalOrderSets");
                                        EMRUtility.scrollToPNcomponent('clinical_ordersets');
                                    }
                                }
                                else {
                                    Clinical_OrderSetDetails.UnLoadTab();
                                }
                            }
                            if (Clinical_ProgressNote.IsDefaultOrderSet == "1") {
                                Clinical_ProgressNote.DefaultOrderSetName = OrderSetName;
                                Clinical_ProgressNote.DefaultOrderSetID = Clinical_OrderSetDetails.params.OrderSetId;
                            }
                        }
                        else
                            utility.DisplayMessages(inner_response.Message, 3);
                    });
                }
                else
                    utility.DisplayMessages(response.Message, 3);

            });

        }
        else
            utility.DisplayMessages("please select to add in Note.", 3);
    },
    detachOrderSetsFromNotes_DBCall: function (OrderSetId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["OrderSetId"] = OrderSetId;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "detach_orderset_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    detachOrderSetFromNotes: function (OrderSetId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    EMRUtility.scrollToPNcomponent('clinical_ordersets');
                    var selectedValue = OrderSetId.replace('Cli_OrderSets_Main', '');
                    if (selectedValue == "" || selectedValue == "undefined") {

                    }
                    else {
                        Clinical_OrderSetDetails.detachOrderSetsFromNotes_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                var CmponentandIDs = JSON.parse(response.DeletedIDsFill_JSON);
                                $.each(CmponentandIDs, function (i, item) {
                                    if (item.Component.indexOf('ProblemList') > -1) {
                                        var deletedProblems = item.DeletedIds.split(',');
                                        if (deletedProblems != "") {

                                            $.each(deletedProblems, function (index, val) {

                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main" + val + "']").remove();

                                            });
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("Problems", true);
                                    }
                                    else if (item.Component == "NotesProcedureOrder") {
                                        var deletedRadiologyOrders = item.DeletedIds.split(',');
                                        if (deletedRadiologyOrders != "") {
                                            $.each(deletedRadiologyOrders, function (index, val) {

                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_ProcedureOrderDetail_Main" + val + "']").remove();

                                            });
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", true);
                                    }
                                    else if (item.Component == "NoteProcedure") {
                                        var deletedProcedures = item.DeletedIds.split(',');
                                        if (deletedProcedures != "") {
                                            $.each(deletedProcedures, function (index, val) {

                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Procedures_Main" + val + "']").remove();

                                            });
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("Procedures", true);
                                    } else if (item.Component.indexOf('NotesPatientEducation') > -1) {
                                        var deletedPatientEducation = item.DeletedIds.split(',');
                                        if (deletedPatientEducation != "") {
                                            $.each(deletedPatientEducation, function (index, val) {
                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_PatientEducation_Main" + val + "']").remove();
                                            });
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllNonInfoDoc section[id*='Cli_PatientEducation_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#hardText").remove();
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllNonInfoDoc").closest('.PatientEducationComponent').find('div').remove();
                                            }
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllInfoDoc section[id*='Cli_PatientEducation_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#hardText").remove();
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllInfoDoc").closest('.PatientEducationComponent').find('div').remove();
                                            }
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("Patient Education", true);
                                    } else if (item.Component.indexOf('Referrals') > -1) {
                                        var deletedReferrals = item.DeletedIds.split(',');
                                        if (deletedReferrals != "") {
                                            $.each(deletedReferrals, function (index, val) {

                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Patient_Referrals_Main" + val + "']").remove();

                                            });
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("Referrals", true);
                                    } else if (item.Component.indexOf('LabOrder') > -1) {
                                        var deletedLabOrders = item.DeletedIds.split(',');
                                        if (deletedLabOrders != "") {
                                            $.each(deletedLabOrders, function (index, val) {

                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_LabOrderDetail_Main" + val + "']").remove();

                                            });
                                        }
                                        if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("section[id*='Cli_LabOrderDetail_Main']").length == 0) {
                                            $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#pendingordertext").remove();
                                        }

                                        Clinical_ProgressNote.saveComponentSOAPText("Lab Orders", true);
                                    } else if (item.Component.indexOf('RadiologyOrder') > -1) {
                                        var deletedRadiologyOrders = item.DeletedIds.split(',');
                                        if (deletedRadiologyOrders != "") {
                                            $.each(deletedRadiologyOrders, function (index, val) {

                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_RadiologyOrderDetail_Main" + val + "']").remove();

                                            });
                                        }

                                        Clinical_ProgressNote.saveComponentSOAPText("Radiology Order", true);
                                    } else if (item.Component.indexOf('Vaccine') > -1) {
                                        var deletedVaccines = item.DeletedIds.split(',');
                                        if (deletedVaccines != "") {
                                            $.each(deletedVaccines, function (index, val) {

                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Immunization_Main" + val + "']").remove();

                                            });
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineAdminister section[id*='Cli_Immunization_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineAdminister").remove();
                                            }
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineDocumentHx section[id*='Cli_Immunization_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineDocumentHx").remove();
                                            }
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineRefusal section[id*='Cli_Immunization_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineRefusal").remove();
                                            }
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("Immunization", true);
                                    }
                                    else if (item.Component.indexOf('TherapeuticInjection') > -1) {
                                        var deletedTherapeutic = item.DeletedIds.split(',');
                                        if (deletedTherapeutic != "") {
                                            $.each(deletedTherapeutic, function (index, val) {
                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Immunization_Main" + val + "thera']").remove();
                                            });
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjection section[id*='Cli_Immunization_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjection").remove();
                                            }
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjectionHistory section[id*='Cli_Immunization_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjectionHistory").remove();
                                            }
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("Immunization", true);
                                    }
                                    else if (item.Component.indexOf('Medication') > -1) {
                                        var deletedTherapeutic = item.DeletedIds.split(',');
                                        if (deletedTherapeutic != "") {
                                            $.each(deletedTherapeutic, function (index, val) {
                                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Medications_Main" + val + "']").remove();
                                            });
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_CurrentMedications section[id*='Cli_Medications_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_CurrentMedications").remove();
                                            }
                                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_PastMedications section[id*='Cli_Medications_Main']").length == 0) {
                                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_PastMedications").remove();
                                            }
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText("Medications", true);
                                    }

                                    else if (item.Component.indexOf('PatientAppointments') > -1) {
                                        var deletedVaccines = item.DeletedIds.split(',');
                                        $.each(deletedVaccines, function (index, val) {
                                            $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_FollowUp_Main" + val + "']").remove();
                                        });
                                        Clinical_ProgressNote.saveComponentSOAPText('Follow Up');
                                    }


                                });
                                $('#' + OrderSetId).remove();
                                $.when(ClinicalCDSDetail.showCDSAlert("", Clinical_ProgressNote.params.patientID)).then(function () {
                                    Clinical_ProgressNote.LoadCDSAlerts();
                                });
                                $.when(Clinical_ProgressNote.saveComponentSOAPText('OrderSets', true)).then(function () {
                                    Clinical_ProgressNote.HideShowBillingInfo();
                                });
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);


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

    detach_ComponentsOrderSet: function (ComponentName, IsUpdate, ProblemListComponentRemove) {
        var outerDeffered = $.Deferred();
        var promise = [];
        var Clinical_OrderSetIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML').parent().parent().find('section[id*="Cli_OrderSets_Main"]').map(function () {
            return this.id.replace("Cli_OrderSets_Main", "");
        }).get().join(',');
        if (Clinical_OrderSetIds == "" || Clinical_OrderSetIds == "undefined") {
            outerDeffered.resolve();
        }
        else {
            Clinical_OrderSetDetails.detachOrderSetsFromNotes_DBCall(Clinical_OrderSetIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var CmponentandIDs = JSON.parse(response.DeletedIDsFill_JSON);
                    $.each(CmponentandIDs, function (i, item) {
                        if (item.Component.indexOf('ProblemList') > -1) {
                            var deletedProblems = item.DeletedIds.split(',');
                            if (deletedProblems != "") {
                                $.each(deletedProblems, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main" + val + "']").remove();
                                });
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Problems", true));
                        } else if (item.Component == "NotesProcedureOrder") {
                            var deletedRadiologyOrders = item.DeletedIds.split(',');
                            if (deletedRadiologyOrders != "") {
                                $.each(deletedRadiologyOrders, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_ProcedureOrderDetail_Main" + val + "']").remove();
                                });
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", true));
                        } else if (item.Component == "NoteProcedure") {
                            var deletedProcedures = item.DeletedIds.split(',');
                            if (deletedProcedures != "") {
                                $.each(deletedProcedures, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Procedures_Main" + val + "']").remove();
                                });
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Procedures", true));
                        } else if (item.Component.indexOf('NotesPatientEducation') > -1) {
                            var deletedPatientEducation = item.DeletedIds.split(',');
                            if (deletedPatientEducation != "") {
                                $.each(deletedPatientEducation, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_PatientEducation_Main" + val + "']").remove();
                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllNonInfoDoc section[id*='Cli_PatientEducation_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#hardText").remove();
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllNonInfoDoc").closest('.PatientEducationComponent').find('div').remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllInfoDoc section[id*='Cli_PatientEducation_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#hardText").remove();
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllInfoDoc").closest('.PatientEducationComponent').find('div').remove();
                                }
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Patient Education", true));
                        } else if (item.Component.indexOf('Referrals') > -1) {
                            var deletedReferrals = item.DeletedIds.split(',');
                            if (deletedReferrals != "") {
                                $.each(deletedReferrals, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Patient_Referrals_Main" + val + "']").remove();

                                });
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Referrals", true));
                        } else if (item.Component.indexOf('LabOrder') > -1) {
                            var deletedLabOrders = item.DeletedIds.split(',');
                            if (deletedLabOrders != "") {
                                $.each(deletedLabOrders, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_LabOrderDetail_Main" + val + "']").remove();

                                });
                            }
                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("section[id*='Cli_LabOrderDetail_Main']").length == 0) {
                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#pendingordertext").remove();
                            }

                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Lab Orders", true));
                        } else if (item.Component.indexOf('RadiologyOrder') > -1) {
                            var deletedRadiologyOrders = item.DeletedIds.split(',');
                            if (deletedRadiologyOrders != "") {
                                $.each(deletedRadiologyOrders, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_RadiologyOrderDetail_Main" + val + "']").remove();

                                });
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Radiology Order", true));
                        } else if (item.Component.indexOf('Vaccine') > -1) {
                            var deletedVaccines = item.DeletedIds.split(',');
                            if (deletedVaccines != "") {
                                $.each(deletedVaccines, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Immunization_Main" + val + "']").remove();

                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineAdminister section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineAdminister").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineDocumentHx section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineDocumentHx").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineRefusal section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineRefusal").remove();
                                }
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Immunization", true));
                        }
                        else if (item.Component.indexOf('TherapeuticInjection') > -1) {
                            var deletedTherapeutic = item.DeletedIds.split(',');
                            if (deletedTherapeutic != "") {
                                $.each(deletedTherapeutic, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Immunization_Main" + val + "thera']").remove();
                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjection section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjection").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjectionHistory section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjectionHistory").remove();
                                }
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Immunization", true));
                        }
                        else if (item.Component.indexOf('Medication') > -1) {
                            var deletedTherapeutic = item.DeletedIds.split(',');
                            if (deletedTherapeutic != "") {
                                $.each(deletedTherapeutic, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Medications_Main" + val + "']").remove();
                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_CurrentMedications section[id*='Cli_Medications_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_CurrentMedications").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_PastMedications section[id*='Cli_Medications_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_PastMedications").remove();
                                }
                            }
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText("Medications", true));
                        }
                        else if (item.Component.indexOf('PatientAppointments') > -1) {
                            var deletedVaccines = item.DeletedIds.split(',');
                            $.each(deletedVaccines, function (index, val) {
                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_FollowUp_Main" + val + "']").remove();
                            });
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText('Follow Up'));
                        }
                    });
                    $.each(Clinical_OrderSetIds.split(','), function (i, item) {
                        $('#Cli_OrderSets_Main' + item).remove();
                    });
                    $.when(ClinicalCDSDetail.showCDSAlert("", Clinical_ProgressNote.params.patientID)).then(function () {
                        Clinical_ProgressNote.LoadCDSAlerts();
                    });
                    var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .OrderSetsComponent').attr('NoteComponentId');
                    if (NoteComponentId && NoteComponentId != "NCDummyId") {
                        if (Clinical_ProgressNote.params["TemplateName"]) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_ordersets').parent().parent().addClass('hidden');
                            promise.push(Clinical_ProgressNote.saveComponentSOAPText('OrderSets', true))
                        }
                        else
                            promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                    }
                    $.when.apply($, promise).done(function () {
                        if (Clinical_ProgressNote.params["TemplateName"] == "")
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_ordersets').parent().parent().remove();
                        Clinical_ProgressNote.ShowHideComponetsHeaders();
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                        Clinical_ProgressNote.HideShowBillingInfo();
                        outerDeffered.resolve();
                    });
                }
                else {
                    outerDeffered.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        return outerDeffered;
    },
    AddToNoteDBCall: function (OrderSetComponents, OrderSetId, NoteId, PatientId, AddInValidAgeRecordsInHxTab) {

        if (!OrderSetId)
            OrderSetId = Clinical_OrderSetDetails.params.OrderSetId;

        if (!NoteId)
            NoteId = Clinical_OrderSetDetails.params.NoteId;

        if (!PatientId)
            PatientId = Clinical_OrderSetDetails.params.PatientId;
        if (!AddInValidAgeRecordsInHxTab) {
            AddInValidAgeRecordsInHxTab = false;
        }

        var objData = {};
        objData["AddInValidAgeRecordsInHxTab"] = AddInValidAgeRecordsInHxTab;
        objData["OrderSetId"] = OrderSetId;
        objData["NotesId"] = NoteId;
        objData["PatientId"] = PatientId;
        objData["ProblemListIDs"] = Clinical_ProgressNote.OSProblems_ComponentIds.join(',');
        objData["ProceduresIDs"] = Clinical_ProgressNote.OSProcedures_ComponentIds.join(',');
        objData["LabOrderIDs"] = Clinical_ProgressNote.OSLabOrder_ComponentIds.join(',');
        objData["RadiologyOrderIDs"] = Clinical_ProgressNote.OSRadiologyOrder_ComponentIds.join(',');
        objData["FollowUpIDs"] = Clinical_ProgressNote.OSFollowUp_ComponentIds.join(',');
        objData["PatientEducationIDs"] = Clinical_ProgressNote.OSPatientEducation_ComponentIds.join(',');
        objData["ReferralsIDs"] = Clinical_ProgressNote.OSReferrals_ComponentIds.join(',');
        objData["ImmunizationIDs"] = Clinical_ProgressNote.OSImmunization_ComponentIds.join(',');
        objData["TherapeuticIDs"] = Clinical_ProgressNote.OSTherapeutic_ComponentIds.join(',');
        objData["MedicationIDs"] = Clinical_ProgressNote.OSMedication_ComponentIds.join(',');
        objData["ProcedureOrderIDs"] = Clinical_ProgressNote.OSProcedureOrder_ComponentIds.join(',');
        objData["OrderSetComponents"] = OrderSetComponents;
        var PatientProblemIds = "";
        var OrderSetAssociatedProblemIds = "";
        if (typeof Clinical_OrderSetDetails.params.ShowSelectedProblems != typeof undefined && Clinical_OrderSetDetails.params.ShowSelectedProblems != null && Clinical_OrderSetDetails.params.ShowSelectedProblems) {
            $.each(Clinical_OrderSetDetails.params.SelectedProblems, function (i, item) {
                if (item.id.indexOf("PatientProb_") > -1) {
                    PatientProblemIds = PatientProblemIds + item.id.replace("PatientProb_", "") + ",";
                }
                else {
                    OrderSetAssociatedProblemIds = OrderSetAssociatedProblemIds + item.id.replace("OrderSetProb_", "") + ",";
                }
            });
        }
        objData["PatientProblemIds"] = PatientProblemIds.substring(0, (PatientProblemIds.length - 1));
        objData["OrderSetAssociatedProblemIds"] = OrderSetAssociatedProblemIds.substring(0, (OrderSetAssociatedProblemIds.length - 1));
        objData["commandType"] = "attach_order_set_to_note";
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");

    },

    AddOrderSetToNoteDBCAll: function (OrderSetComponents, OrderSetId, NoteId, CDSId) {

        if (!OrderSetId)
            OrderSetId = Clinical_OrderSetDetails.params.OrderSetId;

        if (!NoteId)
            NoteId = Clinical_OrderSetDetails.params.NoteId;

        if (!CDSId)
            CDSId = Clinical_OrderSetDetails.params.CDSId;

        var objData = {};
        objData["OrderSetId"] = OrderSetId;
        objData["NoteId"] = NoteId;
        objData["CDSId"] = CDSId;
        objData["OrderSetComponents"] = OrderSetComponents;
        objData["IsDeleted"] = 0;
        objData["IsDefaultOrderSet"] = Clinical_ProgressNote.IsDefaultOrderSet;
        objData["commandType"] = "save_note_order_set";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "NoteOrderSet");

    },

    SetOrderSetForNotes: function () {

        if (Clinical_OrderSetDetails.params["IsNotes"]) {
            $("#" + Clinical_OrderSetDetails.params["PanelID"]).find('.notesOrderSets').removeClass("hidden").removeClass("disableAll");
            $("#" + Clinical_OrderSetDetails.params["PanelID"]).find('.notesActions').addClass("hidden");
        }

    },

    ImmunizationSearch: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_OrderSetDetails.searchImmunization(PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $.when(Clinical_OrderSetDetails.VaccineHxGridLoad(response)).then(function () {
                            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxImmunization']").prop("checked", false);
                            $("#chkHeaderImmunizationOrder").prop("checked", false);
                            dfd.resolve();
                        });
                        var TableControl = Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS';
                        var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + ' #dgvImunization_PagingOS';
                        var ClassControlName = "Clinical_OrderSetDetails";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.ParentImmunizationCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_OrderSetDetails.ImmunizationSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        return dfd;
        //==============================});
    },
    ProcedureOrderSearch: function (procedureId, PageNo, rpp) {
        var strMessage = "";

        Clinical_OrderSetDetails.searchProcedureOrder(procedureId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Clinical_OrderSetDetails.procedureOrderGridLoad(response)).then(function () {
                    $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxProcedureOrder']").prop("checked", false);
                    $("#chkHeaderProcedureOrder").prop("checked", false);
                });
                var TableControl = Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder";
                var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + " #dgvProcedureOrder_Paging";
                var ClassControlName = "Clinical_OrderSetDetails";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.procedureOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_OrderSetDetails.procedureOrderSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    procedureOrderGridLoad: function (response) {
        var dfd = $.Deferred();
        try {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        } catch (ex) {
            console.log(ex);
        }
        try {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder tbody").find("tr").remove(); //Removing all the table data from table body
        } catch (ex) {
            console.log(ex);
        }
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxProcedureOrder" id="chkHeaderProcedureOrder" onchange="Clinical_OrderSetDetails.checkUncheckAllProcedureOrder(this);"  class="input-block" coltype="checkbox"/> </th>');
        }
        if (response.procedureOrderCount > 0) {
            var procedureLoadJSONData = JSON.parse(response.ProcedureLoad_JSON); //Parsing array to JSON
            $.each(procedureLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Clinical_OrderSetDetails.ProcedureOrderAddEdit('" + item.ProcedureOrderId + "',event);");
                $row.attr("id", "gvProcedure_row" + item.ProcedureOrderId);
                $row.attr("ProcedureId", item.ProcedureOrderId);
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddProcedureOrOrder(this);" id="' + item.ProcedureOrderId + '" name="SelectCheckBoxProcedureOrder"  class="input-block text-center"/></td>';
                }
                $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.ProcedureOrderId + '</td><td class="actions Actions_"><a class="btn btn-xs" href="#" onclick="Clinical_OrderSetDetails.procedureOrderDelete(\'' + item.ProcedureOrderId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_OrderSetDetails.ProcedureOrderAddEdit(\'' + item.ProcedureOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>'
                    + item.Procedures.replace(/\|/g, '<br/>') + '</td><td>' + item.Status + '</td><td>' + item.AssigneeName + '</td>');

                $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + ' #pnlProcedureOrder_Result #dgvProcedureOrder').DataTable({
                "language": {
                    "emptyTable": "No Procedure Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + ' #pnlProcedureOrder_Result #dgvProcedureOrder'))
            ;
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
        if (Clinical_OrderSetDetails.params["IsNotes"] == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProcedureOrder').find('td.Actions_').addClass('hidden');
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProcedureOrder #ColumnAction').addClass('hidden');
        }

        if (Clinical_OrderSetDetails.params.mode && Clinical_OrderSetDetails.params.mode == "View") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProcedureOrder").find('.actions a').addClass('disableAll');
        }
        dfd.resolve();
        return dfd;
        //EMRUtility.fixDataTableDuplication("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedureOrder_Result");
    },
    searchProcedureOrder: function (procedureId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};

        if (typeof procedureId != typeof undefined && procedureId != null) {
            objData["ProcedureOrderId"] = procedureId;
        }
        else {
            objData["ProcedureOrderId"] = 0;
        }
        objData["OrdersetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "search_procedureorders";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProcedureOrder");

    },
    procedureOrderDelete: function (procedureId, PageNo, rpp) {
        utility.myConfirm('1', function () {
            var selectedValue = procedureId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var self = $("#" + Clinical_OrderSetDetails.params.PanelID + " form");


                Clinical_OrderSetDetails.deleteProcedureOrder(procedureId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_OrderSetDetails.ProcedureOrderSearch("", 1, 15);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );
    },
    deleteProcedureOrder: function (procedureId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        objData["ProcedureOrderId"] = procedureId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        //AST-279  By:MAHMAD
        //objData["PatientId"] = Clinical_ProcedureOrder.patientId;
        //AST-279  By:MAHMAD
        objData["commandType"] = "delete_procedureorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProcedureOrder");
    },
    MedicationSearch: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        Clinical_OrderSetDetails.searchMedication(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $.when(Clinical_OrderSetDetails.MedicationGridLoad(response)).then(function () {
                    $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxMedication']").prop("checked", false);
                    $("#chkHeaderMedication").prop("checked", false);
                    dfd.resolve();
                });
                var TableControl = Clinical_OrderSetDetails.params.PanelID + ' #dgvMedicationsOS';
                var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + ' #dgvMedications_PagingOS';
                var ClassControlName = "Clinical_OrderSetDetails";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.MedicationCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_OrderSetDetails.MedicationSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);

            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }

        });
        //    } else {
        //        utility.DisplayMessages(strMessage, 2);
        //        dfd.resolve();
        //    }
        //});
        return dfd;
        //==============================});
    },

    MedicationGridLoad: function (response) {
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox"  name="SelectCheckBoxMedication" id="chkHeaderMedicationOrder" checked="false" onchange="Clinical_OrderSetDetails.checkUncheckAllMedicationOrder(this);"  class="input-block" coltype="checkbox"/> </th>');
        }

        if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS")) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS").dataTable().fnClearTable();
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS").dataTable().fnDestroy();
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS tbody").find("tr").remove();
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS tbody").find("tr").remove();


        if (response.MedicationCount > 0) {
            var Medication_JSON = response.Medication_JSON;

            $.each(Medication_JSON, function (i, item) {
                var OS_MedicationId = item.OS_MedicationId;
                var $row = $('<tr/>');
                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.NDCID, 'Clinical_OrderSetDetails', 1, '', 'Medication');
                $row.attr("id", OS_MedicationId);
                var AddParameters = "'Edit'," + item.OS_MedicationId + ",event";
                var MethodMode = 'Clinical_OrderSetDetails.ClickOnMedicationGrid(' + AddParameters + ');';
                //$row.attr("onclick", MethodMode);
                var editParameters = OS_MedicationId + ",event";
                var actions = '<a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.Clinical_OrderSetMedicationDelete(' + OS_MedicationId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSetDetails.OrderSetMedicationEdit(' + editParameters + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>'
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddMedicationOrder(this);" id="' + OS_MedicationId + '" name="SelectCheckBoxMedication"  class="input-block text-center"/></td>';
                }
                var MedicationName = item.BrandName + ' (' + item.GenericName + ') ' + (item.Strength != null ? item.Strength : '') + ' ' + (item.Form != null ? item.Form : '' + ' ');
                var strDrugDosage = "";

                if (item.ActionName != "" || item.Dose != "" || item.DoseUnitName != "" || item.RouteName != "" || item.DoseTimingName != "" || item.DoseOtherName != "") {
                    strDrugDosage = item.ActionName + " " + item.Dose + " " + item.DoseUnitName + " " + item.RouteName + " " + item.DoseTimingName + " " + item.DoseOtherName;
                    strDrugDosage += " " + item.AddDirectionToPatient;
                }
                else {
                    strDrugDosage = item.AddDirectionToPatient;
                }
                $row.append(SelectionCheckBoxColumn + '<td class="actions Actions_" id="' + OS_MedicationId + '" >' + actions + '</td><td>' + MedicationName + $infoButtonrow + '</td><td>' + strDrugDosage + '</td>' + '<td>' + item.Refill + '</td>');
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS tbody").append($row);
            });
            var MedicationRows = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS tbody").find("tr");

            if (MedicationRows.length < 1) {
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS").DataTable({
                    "language": {
                        "emptyTable": "No Medication Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }, { "targets": [3], "visible": false }]
                });
                $("#pnlClinicalNotes #dgvMedications_PagingOS").css("display", "none");
            }
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedications_PagingOS").css("display", "none");
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS").DataTable({
                "language": {
                    "emptyTable": "No Medication Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if (Clinical_OrderSetDetails.params["IsNotes"] == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvMedicationsOS').find('td.Actions_').addClass('hidden');
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvMedicationsOS #ColumnAction').addClass('hidden');
        }

        if (Clinical_OrderSetDetails.params.mode && Clinical_OrderSetDetails.params.mode == "View") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS").find('a').addClass('disableAll');
        }
    },
    TherapeuticSearch: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_OrderSetDetails.searchTherapeutic(PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        $.when(Clinical_OrderSetDetails.TherapeuticGridLoad(response)).then(function () {
                            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxTherapeutic']").prop("checked", false);
                            $("#chkHeaderTherapeuticOrder").prop("checked", false);
                            dfd.resolve();
                        });
                        var TableControl = Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS';
                        var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + ' #dgvTherapeutic_PagingOS';
                        var ClassControlName = "Clinical_OrderSetDetails";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.TherapeuticInjectionCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_OrderSetDetails.TherapeuticSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);

                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        return dfd;
        //==============================});
    },
    OrderSetMedicationEdit: function (OS_MedicationId, event) {
        var params = [];
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        params["OS_MedicationId"] = OS_MedicationId;
        params["mode"] = "Edit";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        params["FromAdmin"] = "0";
        LoadActionPan("OrderSet_Medications", params, Clinical_OrderSetDetails.params.PanelID);
    },
    VaccineHxGridLoad: function (response) {
        var dfd = $.Deferred();
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;">  </th>');
        }

        if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS")) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS").dataTable().fnClearTable();
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS").dataTable().fnDestroy();
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS tbody").find("tr").remove();
        }


        var arraTemp = [];
        if (response.ParentImmunizationCount > 0) {
            var immunizationParentChildLoadJSONData = JSON.parse(response.ImmunizationParentChildLoad_JSON);
            $.each(immunizationParentChildLoadJSONData, function (i, item) {
                var CurrentRowchilds = $();
                var parentRow = item.ParentImmunization;
                var childRows = item.ChildImmunizationList;
                var vaccineId = parentRow.VaccineId;
                var vaccineHxId = parentRow.VaccineHxId;
                var $row = $('<tr/>');
                $row.attr("id", vaccineHxId);
                $row.attr("vaccineHxId", vaccineHxId);
                $row.attr("VaccineScheduleId", parentRow.VaccineScheduleId);
                $row.attr("VaccineCategoryId", parentRow.Category);
                $row.attr("Type", parentRow.Type);
                var ParentType = "";
                if (parentRow.Type != null && parentRow.Type.indexOf("ADMINISTER") >= 0) {
                    ParentType = "Administered";
                }
                else if (parentRow.Type == "DOCUMENTHX") {
                    ParentType = "DocumentHx";
                }
                else if (parentRow.Type == "REFUSAL") {
                    ParentType = "Refusal";
                }
                var actions = '<a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.Clinical_OrderSetImmunizationDelete(' + parentRow.VaccineHxId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSetDetails.OrderSetImmunizationEdit(' + vaccineHxId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>'

                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddImmunizationOrder(this);" id="' + parentRow.VaccineHxId + '" name="SelectCheckBoxImmunization"  class="input-block text-center"/></td>';
                }
                if (childRows.length > 0) {
                    var onclick = "Clinical_OrderSetDetails.ImmunizationRowExpand(this,event);"
                    var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                    $row.append(SelectionCheckBoxColumn + '<td>' + expandCollapseIcon + '</td>' + '<td class="actions Actions_" id="' + item.vaccineHxId + '" >' + actions + '</td><td>' + parentRow.VaccineName + '</td><td>' + parentRow.Dose + '</td><td>' + parentRow.Location + '</td><td>' + parentRow.LotNumber + '</td><td> ' + ParentType + ' </td>');
                } else {
                    $row.append(SelectionCheckBoxColumn + '<td></td><td class="actions Actions_" id="' + item.vaccineHxId + '" >' + actions + '</td><td>' + parentRow.VaccineName + '</td><td>' + parentRow.Dose + '</td><td>' + parentRow.Location + '</td><td>' + parentRow.LotNumber + '</td><td> ' + ParentType + ' </td>');
                }
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlImunization_ResultOS #dgvImunizationOS tbody').last().append($row);
                if (childRows.length > 0) {
                    var childRow = '';
                    $.each(childRows, function (i, item) {
                        var AddParameters = "'Edit','" + item.Category + "'," + item.VaccineScheduleId + ",'Hx'," + item.VaccineHxId + ",'" + item.Type + "'" + ",event";
                        var ChildType = "";
                        if (item.Type != null && item.Type.indexOf("ADMINISTER") >= 0) {
                            ChildType = "Administered";
                        }
                        else if (item.Type == "DOCUMENTHX") {
                            ChildType = "DocumentHx";
                        }
                        else if (item.Type == "REFUSAL") {
                            ChildType = "Refusal";
                        }

                        var ChildActions = '<a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.Clinical_OrderSetImmunizationDelete(' + item.VaccineHxId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSetDetails.OrderSetImmunizationEdit(' + item.VaccineHxId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>'


                        var SelectionCheckBoxColumnChild = '';
                        if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                            SelectionCheckBoxColumnChild = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddImmunizationOrder(this);" id="' + item.VaccineHxId + '" name="SelectCheckBoxImmunization"  class="input-block text-center"/></td>';
                        }

                        childRow += '<tr class="childRow-bg" id=' + item.VaccineHxId + ' vaccineHxId=' + item.VaccineHxId + ' VaccineScheduleId=' + item.VaccineScheduleId + ' Category=' + item.Category + ' Type=' + item.Type + '>' + SelectionCheckBoxColumnChild + '<td></td><td class="actions Actions_" id="' + item.VaccineHxId + '" >' + ChildActions + '</td><td>' + item.VaccineName + '</td><td>' + item.Dose + '</td><td>' + item.Location + '</td><td>' + item.LotNumber + '</td><td>' + ChildType + '</td></tr>';
                    });
                    CurrentRowchilds = CurrentRowchilds.add(childRow);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });
                }
            });
        }
        else {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlImunization_ResultOS #dgvImunizationOS').DataTable({
                "language": {
                    "emptyTable": "No Vaccine Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlImunization_ResultOS #dgvImunizationOS'))
            ;
        else {
            Clinical_OrderSetDetails.ImmunizationTable = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlImunization_ResultOS #dgvImunizationOS').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            //rander childs
            $.each(arraTemp, function (i, item) {
                if (Clinical_OrderSetDetails.ImmunizationTable != null) {
                    var row = Clinical_OrderSetDetails.ImmunizationTable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlImunization_ResultOS #dgvImunizationOS').off()
            .on('click', 'a.expand-row', function (e) {
                e.preventDefault();

                Clinical_OrderSetDetails.rowExpand($(this).closest('tr'));
            })
        }

        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS thead tr th:first-child').removeClass('sorting_asc');
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS thead tr th:nth-child(2)').removeClass('sorting');
        if (Clinical_OrderSetDetails.params["IsNotes"] == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS').find('td.Actions_').addClass('hidden');
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS #ColumnAction').addClass('hidden');
        }

        if (Clinical_OrderSetDetails.params.mode && Clinical_OrderSetDetails.params.mode == "View") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS").find('.actions a').addClass('disableAll');
        }
        dfd.resolve();
        return dfd;
    },
    TherapeuticGridLoad: function (response) {
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox"  name="SelectCheckBoxTherapeutic" id="chkHeaderTherapeuticOrder" checked="false" onchange="Clinical_OrderSetDetails.checkUncheckAllTherapeuticOrder(this);"  class="input-block" coltype="checkbox"/> </th>');
        }

        if ($.fn.dataTable.isDataTable('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS")) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS").dataTable().fnClearTable();
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS").dataTable().fnDestroy();
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS tbody").find("tr").remove();
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS tbody").find("tr").remove();
        var TherapeuticInjectionLoad_JSON = JSON.parse(response.TherapeuticInjectionLoad_JSON);
        if (TherapeuticInjectionLoad_JSON.length > 0) {
            $.each(TherapeuticInjectionLoad_JSON, function (i, item) {
                var OSImmTherInjectionId = item.OSImmTherInjectionId;
                var $row = $('<tr/>');
                $row.attr("id", OSImmTherInjectionId);
                var AddParameters = "'Edit'," + item.OSImmTherInjectionId + ",event,'" + item.Type + "'";
                var MethodMode = 'Clinical_OrderSetDetails.ClickOnTherapeuticInjectionGrid(' + AddParameters + ');';
                //$row.attr("onclick", MethodMode);
                var editParameters = OSImmTherInjectionId + ",'" + item.Type + "'";
                var actions = '<a class="btn  btn-xs" href="#" onclick="Clinical_OrderSetDetails.Clinical_OrderSetTherapeuticDelete(' + OSImmTherInjectionId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_OrderSetDetails.OrderSetTherapeuticEdit(' + editParameters + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>'
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_OrderSetDetails.enableAddTherapeuticOrder(this);" id="' + OSImmTherInjectionId + '" name="SelectCheckBoxTherapeutic"  class="input-block text-center"/></td>';
                }
                $row.append('<td style="display:none">' + item.ModifiedOn + '</td>' + SelectionCheckBoxColumn + '<td class="actions Actions_" id="' + OSImmTherInjectionId + '" >' + actions + '</td><td>' + item.TherapeuticInjection + '</td><td>' + (item.Dose != "" ? item.Dose + " " + item.Amount : "") + '</td><td >' + item.LotText + '</td><td>' + item.Type + '</td>');
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS tbody").append($row);
            });


            var TherapeuticRows = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS tbody").find("tr");

            if (TherapeuticRows.length < 1) {
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS").DataTable({
                    "language": {
                        "emptyTable": "No Therapeutic Injection Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }, { "targets": [7], "visible": false }]
                });
                $("#pnlClinicalNotes #dgvTherapeutic_PagingOS").css("display", "none");
            }
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeutic_PagingOS").css("display", "none");
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS").DataTable({
                "language": {
                    "emptyTable": "No Therapeutic Injection Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if (Clinical_OrderSetDetails.params["IsNotes"] == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvTherapeuticOS').find('td.Actions_').addClass('hidden');
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvTherapeuticOS #ColumnAction').addClass('hidden');
        }

        if (Clinical_OrderSetDetails.params.mode && Clinical_OrderSetDetails.params.mode == "View") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS").find('.actions a').addClass('disableAll');
        }
    },
    OrderSetTherapeuticEdit: function (OSImmTherInjectionId, Type) {
        var params = [];
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        params["OSImmTherInjectionId"] = OSImmTherInjectionId;
        params["mode"] = "Edit";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        params["Type"] = Type;
        params["FromAdmin"] = "0";
        LoadActionPan("OrderSet_TherapeuticDetail", params, Clinical_OrderSetDetails.params.PanelID);
    },
    ImmunizationRowExpand: function ($row) {
        $row = $($row).parent().parent();
        var row = Clinical_OrderSetDetails.ImmunizationTable.row($row);
        if (row.child.isShown()) {
            $row.find("td .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
        }
        else {
            $row.find("td .fa-plus-square").attr("class", "fa fa-minus-square");
            row.child.show();
            if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
            }
            if (Clinical_OrderSetDetails.params["IsNotes"] == true) {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS').find('td.Actions_').addClass('hidden');
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS #ColumnAction').addClass('hidden');
            }
            if (Clinical_OrderSetDetails.params.mode && Clinical_OrderSetDetails.params.mode == "View") {
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS").find('.actions a').addClass('disableAll');
            }
        }
    },
    checkUncheckAllImmunizationOrder: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxImmunization']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxImmunization']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvImunizationOS tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddImmunizationOrder(this);
        });
    },
    checkUncheckAllTherapeuticOrder: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxTherapeutic']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxTherapeutic']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvTherapeuticOS tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddTherapeuticOrder(this);
        });
    },
    checkUncheckAllMedicationOrder: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxMedication']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxMedication']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvMedicationsOS tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddMedicationOrder(this);
        });
    },
    checkUncheckAllProcedureOrder: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxProcedureOrder']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxProcedureOrder']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProcedureOrder tbody").find('input[type="checkbox"]').each(function () {
            Clinical_OrderSetDetails.enableAddProcedureOrOrder(this);
        });
    },
    enableAddMedicationOrder: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSMedication_ComponentIds) == -1) {
                Clinical_ProgressNote.OSMedication_ComponentIds.push(obj.id);
            }
        } else {
            var index = Clinical_ProgressNote.OSMedication_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSMedication_ComponentIds.splice(index, 1);
            }
        }
    },
    enableAddTherapeuticOrder: function (obj) {
        if ($(obj).is(':checked')) {
            OrderSet_ImmunizationDetail.IsVaccineHxLotIssue(obj.id, 'Therapeutic').done(function (result) {
                if (result == "0") {
                    if ($.inArray(obj.id, Clinical_ProgressNote.OSTherapeutic_ComponentIds) == -1) {
                        Clinical_ProgressNote.OSTherapeutic_ComponentIds.push(obj.id);
                    }
                }
                else if (result == "1") {
                    $(obj).prop("checked", false);
                    utility.DisplayMessages("Either Therapeutic lot is expired or out-of-stock!", 2);
                }
                else {
                    $(obj).prop("checked", false);
                    utility.DisplayMessages("Either Therapeutic lot is expired or out-of-stock!", 2);
                }
            });
        } else {
            var index = Clinical_ProgressNote.OSTherapeutic_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSTherapeutic_ComponentIds.splice(index, 1);
            }
        }
    },
    enableAddImmunizationOrder: function (obj) {
        if ($(obj).is(':checked')) {
            OrderSet_ImmunizationDetail.IsVaccineHxLotIssue(obj.id, null, Clinical_ProgressNote.OSImmunization_ComponentIds).done(function (result) {
                if (result == "0") {
                    if ($.inArray(obj.id, Clinical_ProgressNote.OSImmunization_ComponentIds) == -1) {
                        Clinical_ProgressNote.OSImmunization_ComponentIds.push(obj.id);
                    }
                }
                else if (result == "1") {
                    $(obj).prop("checked", false);
                    utility.DisplayMessages("Either vaccine lot is expired or out-of-stock!", 2);
                }
                else if (result == "2") {
                    $(obj).prop("checked", false);
                    utility.DisplayMessages("Vaccine lot is out-of-stock!", 2);
                }
                else {
                    $(obj).prop("checked", false);
                    utility.DisplayMessages("Either vaccine lot is expired or out-of-stock!", 2);
                }
            });
        } else {

            var index = Clinical_ProgressNote.OSImmunization_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSImmunization_ComponentIds.splice(index, 1);
            }
        }
    },
    ClickOnAddButtonFromSchedulerGrid: function (Mode, VaccineCategoryId, VaccineScheduleId, VaccineHxId, Type, event) {


        var MODE = "ADD";
        if (Mode == "Edit") {
            MODE = "EDIT";
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", MODE, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = "Clinical_OrderSetDetails";
                params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
                params["FromAdmin"] = "0";
                params["VaccineScheduleId"] = VaccineScheduleId;
                params["CategoryId"] = VaccineCategoryId;
                if (Mode == "EDIT") {
                    params["VaccineHxId"] = VaccineHxId;
                    params["Type"] = Type;
                }
                params["mode"] = Mode;
                LoadActionPan("OrderSet_ImmunizationDetail", params, Clinical_OrderSetDetails.params.PanelID);
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowExpand: function ($row) {
        var row = Clinical_OrderSetDetails.ImmunizationTable.row($row);
        if (row.child.isShown()) {
            $row.find("td .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
        }
        else {
            $row.find("td .fa-plus-square").attr("class", "fa fa-minus-square");
            row.child.show();
            if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
            }
        }
    },
    OrderSetImmunizationEdit: function (vaccineHxId) {
        var $row = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS tbody tr#' + vaccineHxId);
        vaccineHxId = $($row).attr("vaccineHxId");
        VaccineScheduleId = $($row).attr("VaccineScheduleId");
        VaccineCategoryId = $($row).attr("VaccineCategoryId");
        Type = $($row).attr("Type");
        Clinical_OrderSetDetails.ClickOnAddButtonFromSchedulerGrid("EDIT", VaccineCategoryId, VaccineScheduleId, vaccineHxId, Type);
    },
    Clinical_OrderSetImmunizationDelete: function (vaccineHxId) {
        var row = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvImunizationOS tbody tr#' + vaccineHxId);
        var strMessage = "";
        var id = $(row).attr("id");
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Clinical_OrderSetDetails.OrderSetImmunizationDelete_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_OrderSetDetails.ImmunizationSearch();
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

    Clinical_OrderSetTherapeuticDelete: function (TherapeuticId) {
        var row = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvTherapeuticOS tbody tr#' + TherapeuticId);
        var strMessage = "";
        var id = $(row).attr("id");
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Clinical_OrderSetDetails.OrderSetTherapeuticDelete_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_OrderSetDetails.TherapeuticSearch();
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

    Clinical_OrderSetMedicationDelete: function (OS_MedicationId) {
        var row = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvMedicationsOS tbody tr#' + OS_MedicationId);
        var id = $(row).attr("id");
        utility.myConfirm('1', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_OrderSetDetails.OrderSetMedicationDelete_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_OrderSetDetails.MedicationSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
        }, function () { },
            '1'
        );

    },

    OrderSetImmunizationDelete_DBCall: function (VaccineHxId) {
        var objData = new Object();
        objData["VaccineHxId"] = VaccineHxId;
        objData["commandType"] = "DELETE_OS_VACCINEHX";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");
    },
    OrderSetTherapeuticDelete_DBCall: function (TherapeuticId) {
        var objData = new Object();
        objData["OSImmTherInjectionId"] = TherapeuticId;
        objData["commandType"] = "DELETE_OS_THERAPEUTIC_HX";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },
    OrderSetMedicationDelete_DBCall: function (OS_MedicationId) {
        var objData = new Object();
        objData["OS_MedicationId"] = OS_MedicationId;
        objData["commandType"] = "DELETE_OS_Medication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "MEDICATION");
    },
    searchImmunization: function (pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        if (Clinical_OrderSetDetails.params["IsNotes"] == true) {
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        }
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");
    },
    searchTherapeutic: function (pageNumber, rowsPerPage, OsImmTherInjectionId, Type) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        if (typeof OsImmTherInjectionId != typeof undefined && OsImmTherInjectionId != null && OsImmTherInjectionId > 0) {
            objData["OSImmTherInjectionId"] = OsImmTherInjectionId;
            objData["Type"] = Type;
        }
        else {
            objData[" OSImmTherInjectionId"] = 0;
        }
        if (Clinical_OrderSetDetails.params["IsNotes"] == true) {
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        }
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Therapeutic";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },
    searchMedication: function (pageNumber, rowsPerPage, OS_MedicationId) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        if (typeof OS_MedicationId != typeof undefined && OS_MedicationId != null && OS_MedicationId > 0) {
            objData["OS_MedicationId"] = OS_MedicationId;
        }
        else {
            objData[" OS_MedicationId"] = 0;
        }
        objData["OrdersetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Medication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "MEDICATION");
    },
    DocumentPreview: function (patDocId, orderSetPatEducationId, event) {
        if (event != null) {
            event.stopPropagation();
            if ($(event.target).is('input[type=checkbox]')) {
                return;
            }
        }
        var params = [];

        params["OrderSetPatEducationId"] = orderSetPatEducationId;
        params["PatDocID"] = patDocId;
        params["PanelID"] = "pnlClinicalOrderSetDetails";
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        PanelID = 'pnlClinicalOrderSetDetails';
        LoadActionPan('Document_Viewer', params, PanelID);


    },
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_OrderSetDetails", null, false);
    },
    showIcon: function (obj) {
        $(obj).find('div').css('display', '');
    },
    hideIcon: function (obj) {
        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }
    },
    deleteProblemList: function (obj, ev) {
        ev.stopPropagation();
        var problemListId = $(obj).attr('id');
        if ($(obj).hasClass('fromDB') == false) {
            $(obj).parent().remove();
            if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li").length > 0) {
                $('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li:first select").remove();
            }
        } else {
            utility.myConfirm('1', function () {
                var selectedValue = problemListId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_OrderSetDetails.OrderSetProblemListDelete(selectedValue).done(function (response) {

                        response = JSON.parse(response);
                        if (response.status != false) {
                            $(obj).remove();
                            if ($('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li").length > 0) {
                                $('#' + Clinical_OrderSetDetails.params.PanelID + " #frmOrderSetDetails #ulOrderSetProblemList li:first select").remove();
                            }
                            // dfd.resolve();
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

    },
    OrderSetProblemListDelete: function (problemListId) {
        var objData = new Object();
        objData["OrderSetProblemId"] = problemListId;
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["commandType"] = "DELETE_OrderSet_ProblemList";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {

            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }

        $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').attr("data-popupunload", "true");
        var params = [];
        params["FromAdmin"] = "0";
        if (Clinical_OrderSetDetails.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        }

        else {
            params["ParentCtrl"] = "Clinical_OrderSetDetails";

        }
        params["PanelID"] = Clinical_OrderSetDetails.params["PanelID"];

        params["ActionPanContainer"] = Clinical_OrderSetDetails.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_OrderSetDetails.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, Clinical_OrderSetDetails.params.PanelID);
        }

    },
    detachDefaultOrderSetFromNotes: function () {

        if (!Clinical_ProgressNote.DefaultOrderSetID) {
        }
        else {
            var OrderSetId = Clinical_ProgressNote.DefaultOrderSetID;
            Clinical_OrderSetDetails.detachOrderSetsFromNotes_DBCall(OrderSetId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var CmponentandIDs = JSON.parse(response.DeletedIDsFill_JSON);
                    $.each(CmponentandIDs, function (i, item) {
                        if (item.Component.indexOf('ProblemList') > -1) {
                            var deletedProblems = item.DeletedIds.split(',');
                            if (deletedProblems != "") {

                                $.each(deletedProblems, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main" + val + "']").remove();

                                });
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("Problems", true);
                        }
                        else if (item.Component == "NotesProcedureOrder") {
                            var deletedRadiologyOrders = item.DeletedIds.split(',');
                            if (deletedRadiologyOrders != "") {
                                $.each(deletedRadiologyOrders, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_ProcedureOrderDetail_Main" + val + "']").remove();

                                });
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", true);
                        }
                        else if (item.Component == "NoteProcedure") {
                            var deletedProcedures = item.DeletedIds.split(',');
                            if (deletedProcedures != "") {
                                $.each(deletedProcedures, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Procedures_Main" + val + "']").remove();

                                });
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("Procedures", true);
                        } else if (item.Component.indexOf('NotesPatientEducation') > -1) {
                            var deletedPatientEducation = item.DeletedIds.split(',');
                            if (deletedPatientEducation != "") {
                                $.each(deletedPatientEducation, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_PatientEducation_Main" + val + "']").remove();
                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllNonInfoDoc section[id*='Cli_PatientEducation_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#hardText").remove();
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllNonInfoDoc").closest('.PatientEducationComponent').find('div').remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllInfoDoc section[id*='Cli_PatientEducation_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#hardText").remove();
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#AllInfoDoc").closest('.PatientEducationComponent').find('div').remove();
                                }
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("Patient Education", true);
                        } else if (item.Component.indexOf('Referrals') > -1) {
                            var deletedReferrals = item.DeletedIds.split(',');
                            if (deletedReferrals != "") {
                                $.each(deletedReferrals, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Patient_Referrals_Main" + val + "']").remove();

                                });
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("Referrals", true);
                        } else if (item.Component.indexOf('LabOrder') > -1) {
                            var deletedLabOrders = item.DeletedIds.split(',');
                            if (deletedLabOrders != "") {
                                $.each(deletedLabOrders, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_LabOrderDetail_Main" + val + "']").remove();

                                });
                            }
                            if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("section[id*='Cli_LabOrderDetail_Main']").length == 0) {
                                $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#pendingordertext").remove();
                            }

                            Clinical_ProgressNote.saveComponentSOAPText("Lab Orders", true);
                        } else if (item.Component.indexOf('RadiologyOrder') > -1) {
                            var deletedRadiologyOrders = item.DeletedIds.split(',');
                            if (deletedRadiologyOrders != "") {
                                $.each(deletedRadiologyOrders, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_RadiologyOrderDetail_Main" + val + "']").remove();

                                });
                            }

                            Clinical_ProgressNote.saveComponentSOAPText("Radiology Order", true);
                        } else if (item.Component.indexOf('Vaccine') > -1) {
                            var deletedVaccines = item.DeletedIds.split(',');
                            if (deletedVaccines != "") {
                                $.each(deletedVaccines, function (index, val) {

                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Immunization_Main" + val + "']").remove();

                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineAdminister section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineAdminister").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineDocumentHx section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineDocumentHx").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineRefusal section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_VaccineRefusal").remove();
                                }
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("Immunization", true);
                        }
                        else if (item.Component.indexOf('TherapeuticInjection') > -1) {
                            var deletedTherapeutic = item.DeletedIds.split(',');
                            if (deletedTherapeutic != "") {
                                $.each(deletedTherapeutic, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Immunization_Main" + val + "thera']").remove();
                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjection section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjection").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjectionHistory section[id*='Cli_Immunization_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_TherapeuticInjectionHistory").remove();
                                }
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("Immunization", true);
                        }
                        else if (item.Component.indexOf('Medication') > -1) {
                            var deletedTherapeutic = item.DeletedIds.split(',');
                            if (deletedTherapeutic != "") {
                                $.each(deletedTherapeutic, function (index, val) {
                                    $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Medications_Main" + val + "']").remove();
                                });
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_CurrentMedications section[id*='Cli_Medications_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_CurrentMedications").remove();
                                }
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_PastMedications section[id*='Cli_Medications_Main']").length == 0) {
                                    $('#pnlClinicalProgressNote #ProgressnoteHTML').find("#Section_PastMedications").remove();
                                }
                            }
                            Clinical_ProgressNote.saveComponentSOAPText("Medications", true);
                        }

                        else if (item.Component.indexOf('PatientAppointments') > -1) {
                            var deletedVaccines = item.DeletedIds.split(',');
                            $.each(deletedVaccines, function (index, val) {
                                $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_FollowUp_Main" + val + "']").remove();
                            });
                            Clinical_ProgressNote.saveComponentSOAPText('Follow Up');
                        }
                    });
                    $('#Cli_OrderSets_Main' + OrderSetId).remove();
                    $.when(Clinical_ProgressNote.saveComponentSOAPText('OrderSets', true)).then(function () {
                        Clinical_ProgressNote.HideShowBillingInfo();
                    });
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }


    },
    //Comments Update

    AddComments: function (ProcedureId) {
        var params = [];
        params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        params["ProcedureID"] = ProcedureId;
        params["Comments"] = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();
        params["FromAdmin"] = "0";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        LoadActionPan('Clinical_ProceduresComments', params, Clinical_OrderSetDetails.params.PanelID);
    },
    //AST - 74 BY:MAhmad
    loadProblemLookUp: function () {
        var self = $('#' + Clinical_OrderSetDetails.params.PanelID);
        self.find('.Diagnosis > select').attr('ddlist', 'LookupProblemListsForOrderSet');
        var data = "IsActive=&ID=" + Clinical_OrderSetDetails.params.OrderSetId;
        self.find('.Diagnosis').loadDropDowns(true, data).done(function () {
        });
    }
    //AST - 74 BY:MAhmad
}