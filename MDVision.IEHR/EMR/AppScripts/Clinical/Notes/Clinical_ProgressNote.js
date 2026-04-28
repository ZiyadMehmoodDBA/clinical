/*
    Author: Muhammad Azhar Shahzad
    Creation Date: November 18,2015
    OverView:This File Is created for Clinical Progress Notes
*/
Clinical_ProgressNote = {
    bIsFirstLoad: true,
    params: [],
    // variables to get attached/ unattached component, during selecting components data
    AttachedNoteComponentIds: [],
    DetachedNoteComponentIds: [],
    OSProblems_ComponentIds: [],
    OSProcedures_ComponentIds: [],
    OSLabOrder_ComponentIds: [],
    OSRadiologyOrder_ComponentIds: [],
    OSFollowUp_ComponentIds: [],
    OSPatientEducation_ComponentIds: [],
    OSReferrals_ComponentIds: [],
    OSImmunization_ComponentIds: [],
    OSTherapeutic_ComponentIds: [],
    OSMedication_ComponentIds: [],
    OSProcedureOrder_ComponentIds: [],
    ComeFromCopyNote: false,
    intervalProcess: null,
    arrCQMReasoning: [],
    arrVBPReasoning: [],
    Toggle_Element: null,
    SignalRHub: null,
    NoteAccessTime: null,
    IsNewNote: false,
    IsAnyDocumentAttached: false,
    DefaultOrderSetName: "",
    DefaultOrderSetId: 0,
    IsDefaultOrderSet: 0,
    NewDefaultOrderSetId: 0,
    IsPreviousNotePE: false,
    IsPreviousNoteROS: false,
    IsPreviousNoteComplaints: false,
    ddlOrderSetPreviousValRetain: '',
    Load: function (params) {

        findInDiv.Load();

        //findInDiv.hide(true);
        Clinical_ProgressNote.params = params;
        Clinical_ProgressNote.AppointmentTime = Clinical_ProgressNote.params.NotesVisitTime;
        Clinical_ProgressNote.AppointmentTimeFrom = Clinical_ProgressNote.params.AppointmentTimeFrom;
        Clinical_ProgressNote.AppointmentTimeTo = Clinical_ProgressNote.params.AppointmentTimeTo;
        Clinical_ProgressNote.params.newlyAddedProblemLists = [];
        Clinical_ProgressNote.params.isProgressNoteSelected = true;
        Clinical_ProgressNote.IsNewNote = Clinical_ProgressNote.params.IsFromCreateNote;

        if (!Clinical_ProgressNote.params.NotesId || Clinical_ProgressNote.params.NotesId <= 0) {
            Clinical_ProgressNote.UnLoad();
            return false;
        }
        else if (!($('#mstrDivNotes').is(':visible'))) {
            $('#mstrDivNotes').show();
        }

        IsBackgroundLoaderShow = false;
        ShowHideLoaderOnScreen(false);
        xhrPool = [];
        Clinical_ProgressNote.ComeFromCopyNote = false;

        if (Clinical_ProgressNote.bIsFirstLoad) {
            if (globalAppdata["isTransitonCancerRegistries"] && globalAppdata["isTransitonCancerRegistries"].toLowerCase() == "false")
                Clinical_ProgressNote.LoadVisitTypeddlWO_CancerRegistries();
            else
                $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTypeDiv #ddlVisitType').attr('ddlist', "GetPatientVisitType");
            var self = $('#' + Clinical_ProgressNote.params.PanelID);

            self.loadDropDowns(true);

            Clinical_ProgressNote.bIsFirstLoad = false;
            if ($('#pnlTab3 #mstrDivClinical ').parent().find('#mstrDivNotes').length > 0) {
                $('#pnlTab3 #mstrDivClinical #mstrDivNotes').remove();
            }

            // it is an extra call, as it's select control is commented earlier. MA-28
            //if (Clinical_ProgressNote.params.patientID) {
            //    CacheManager.BindDropDownsByEntityID('#' + Clinical_ProgressNote.params["PanelID"] + ' #ddlDuration', 'GetPhoneEncounterDuration', false, Clinical_ProgressNote.params.patientID);
            //}

            if (Clinical_ProgressNote.params.PanelID != 'pnlClinicalProgressNote') {
                Clinical_ProgressNote.params.PanelID = Clinical_ProgressNote.params.PanelID + ' #pnlClinicalProgressNote';
            }

            if (Clinical_ProgressNote.params.IsDisableDateTimeCtrl) {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTime').prop('disabled', false);
                $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').prop('disabled', false);
            }

            // Binding Validatoin Function
            Clinical_ProgressNote.ValidateProgressNotes(Clinical_ProgressNote.params.PanelID + ' #frmClinicalProgressNote');

            Clinical_ProgressNote.bindDateAndTimepicker();
            Clinical_ProgressNote.BindReferralProvider();

            Clinical_ProgressNote.domReadyFunctions();
        }
        else {

            if (Clinical_ProgressNote.params.IsDisableDateTimeCtrl) {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTime').prop('disabled', false);
                $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').prop('disabled', false);
            }
            if (globalAppdata["isTransitonCancerRegistries"] && globalAppdata["isTransitonCancerRegistries"].toLowerCase() == "false")
                Clinical_ProgressNote.LoadVisitTypeddlWO_CancerRegistries();
            else
                $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTypeDiv #ddlVisitType').attr('ddlist', "GetPatientVisitType");
            var self = $('#' + Clinical_ProgressNote.params.PanelID);

            self.find('#VisitTypeDiv').loadDropDowns(true);

        }

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + " #btnNoteAttachment").hasClass('disableAll')) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #btnNoteAttachment").removeClass('disableAll');
        }

        //check if edit call back is come from other forms, other than Notes
        if (Clinical_ProgressNote.params.NotesId == null) {
            Clinical_ProgressNote.Clinical_NotesFill(Clinical_Notes.params.NotesId, Clinical_ProgressNote.params.PanelID + ' #frmClinicalProgressNote');
        }
        else {
            $('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteId').val(Clinical_ProgressNote.params.NotesId);
            Clinical_ProgressNote.Clinical_NotesFill(Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.PanelID + ' #frmClinicalProgressNote');
        }

        // Reseting Paramaters set for Notes in global params
        Clinical_ProgressNote.ResetParams();

        // For Phone Enounter show/ hide controls
        Clinical_ProgressNote.SetValuesBasedOnNoteType();

        //Load CDS Alerts and Pervileges
        Clinical_ProgressNote.LoadCDSAlerts(true);

        //Clinical_ProgressNote.custom_events();

        //Clinical_ProgressNote.CheckPervileges();

        // it's an extra call
        //Clinical_ProgressNote.LoadCDSAlertsForNotes();

        // connect to SignalR when Note loaded completely.
        utility.callbackAfterAllDOMLoaded(function () {
            Clinical_ProgressNote.connectUserToSignalR();
            if ($('#' + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML').length > 0 &&
                $('#' + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML').html() == "") {
                if ($("#mstrDivNotes #clinicalTabProgressNote").length > 0
                    && !$("#mstrDivNotes #clinicalTabProgressNote").attr('isclicked')) {
                    $("#mstrDivNotes #clinicalTabProgressNote").attr('isclicked', "isclicked");
                    $("#mstrDivNotes #clinicalTabProgressNote").click();
                }
                else
                    $("#mstrDivNotes #clinicalTabProgressNote").removeAttr('isclicked');


            }
            $("#ActionsInitialOfficeVisit").removeClass('tab_selected');
        });

        try {
            var d = new Date();
            var hours = d.getHours();
            var minutes = d.getMinutes();
            var seconds = d.getSeconds();

            if (hours && hours.toString().length <= 1) {
                hours = "0" + hours
            }
            if (minutes && minutes.toString().length <= 1) {
                minutes = "0" + minutes
            }
            if (seconds && seconds.toString().length <= 1) {
                seconds = "0" + seconds
            }

            var dd = $.datepicker.formatDate('yymmdd', d);
            Clinical_ProgressNote.NoteAccessTime = dd + hours + minutes + seconds;
        } catch (e) {
            console.log('Clinical_ProgressNote.NoteAccessTime:' + Clinical_ProgressNote.NoteAccessTime + " Execption:" + e.message);
        }

        Clinical_ProgressNote.SetCollapseExpandPanel();
        Clinical_ProgressNote.RegisterNoteAgainstUser();
        $("#ClinicalUL").find('li:not(#clinicalMenuNotes)').removeClass("active nav-active nav-expanded");
        $('#btnSearchInDiv').trigger('click');
        if (localStorage.getItem('IsNoteLeftCollasped') && localStorage.getItem('IsNoteLeftCollasped') == 'true') {
            $('#pnlClinicalProgressNote #wrapper').removeClass('toggled');
        }
        if (localStorage.getItem('IsNoteRightCollasped') && localStorage.getItem('IsNoteRightCollasped') == 'true') {
            $('html').addClass('sidebar-left-collapsed')
        }
    },
    GetOrderSetId: function (obj) {
        Clinical_ProgressNote.ddlOrderSetPreviousValRetain = obj.value;
    },
    ChangeOrderSet: function () {
        var orderSetID = $("#" + Clinical_ProgressNote.params.PanelID + " #ddlOrderSet").val();
        var orderSetText = $('#pnlClinicalProgressNote #ddlOrderSet option:selected').text();
        if (orderSetID && orderSetText) {
            Clinical_ProgressNote.CheckIfAnyOrderSetIsAssociatedWithNote(Clinical_ProgressNote.params.NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false && response.Result != false) {
                    utility.myConfirm("All the order sets will be replaced with this order set. Do you wish to continue? ", function () {
                        Clinical_ProgressNote.AddOrderSet(orderSetID);
                    }, function () {
                        $("#" + Clinical_ProgressNote.params.PanelID + " #ddlOrderSet").val(Clinical_ProgressNote.ddlOrderSetPreviousValRetain);
                    }, '<b>Confirm Replacement</b>');
                }
                else
                    Clinical_ProgressNote.AddOrderSet(orderSetID);
            });
        }
    },
    FindTextOnNote: function (obj) {
        $(obj).focus();

    },
    GetOrderSetTemplate: function () {

        var self = $('#' + Clinical_ProgressNote.params.PanelID);
        var NoteTemplateVal = $("#" + Clinical_ProgressNote.params.PanelID + " #NoteTemplate").val();
        var data = "IsActive=1&StrID=" + globalAppdata.DefaultProviderId;
        self.find('.OrderSet > select').attr('ddlist', 'GetOrderSetTemplate');
        self.find('.OrderSet').loadDropDowns(true, data).done(function () {

            var data1 = "IsActive=1&StrID=" + globalAppdata.DefaultProviderId + "&StrID2=" + NoteTemplateVal;
            return MDVisionService.lookups('GetOrderSetTemplateByID', true, data1).done(function (result) {
                if (result["GetOrderSetTemplateByID"]) {
                    result = JSON.parse(result["GetOrderSetTemplateByID"]);
                    var orderSetDefaultValue = "";
                    $.each(result, function (j, result) {
                        if (result.Value) {
                            orderSetDefaultValue = result.Name;
                        }
                    });

                    if (orderSetDefaultValue == "") {
                        self.find('.OrderSet > select').val(self.find('.OrderSet > select option[value!=""]:first').val());
                        return false;
                    } else {

                        self.find(".OrderSet option:contains(" + orderSetDefaultValue + ")").attr('selected', true);
                    }

                }
            })

        });

    },
    LoadVisitTypeddlWO_CancerRegistries: function () {
        var dfd = $.Deferred();
        MDVisionService.lookups('GetPatientVisitTypeWithoutCancerRegistries', true, "").done(function (results) {
            var htmlddl = "";
            if (results["GetPatientVisitTypeWithoutCancerRegistries"])
                results = JSON.parse(results["GetPatientVisitTypeWithoutCancerRegistries"]);
            if (results) {
                $.each(results, function (j, result) {
                    htmlddl += '<option refval="' + result.RefValue + '" value="' + result.Value + '">' + result.Name + '</option>';
                });
            }
            $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTypeDiv #ddlVisitType').html(htmlddl);
            dfd.resolve();
        });
        return dfd;
    },
    SetCollapseExpandPanel: function () {
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Clinical_ProgressNote.params.PanelID + ' #splitterbody').attr('class', 'splitterbody active');
            $('#' + Clinical_ProgressNote.params.PanelID + ' #splitterbody').show();
        }
        else {
            $('#' + Clinical_ProgressNote.params.PanelID + ' #splitterbody').removeClass('splitterbody active');
            $('#' + Clinical_ProgressNote.params.PanelID + ' #splitterbody').hide();
        }

    },
    bindOrdersetList: function () {

        Clinical_ProgressNote.searchOrderSets_DBCall(1, 2000, null).done(function (response) {
            response = JSON.parse(response);
            $("#pnlClinicalProgressNote #OrderSet_toggle #OrderSetList #OrderSetUl").empty();
            if (response.status != false) {
                if (response.OrderSetCount > 0) {
                    var listOrderSet_JSON = response.listOrderSet;

                    $.each(listOrderSet_JSON, function (i, item) {
                        var Parameter = "null," + item.OrderSetId + ", event,'" + item.OrderSetName.trim() + "','clinicalTabProgressNote','null'";
                        var li = '<li id="Orderset_' + item.OrderSetId + '" class="ui-draggable ui-draggable-handle"><a href="javascript:Clinical_ProgressNote.OrderSetDetail(' + Parameter + ');" title="' + item.OrderSetName.trim() + '">' + item.OrderSetName.trim() + '</a></li>';
                        $("#pnlClinicalProgressNote #OrderSet_toggle #OrderSetList #OrderSetUl").append(li);
                    });

                }
                else {
                    $("#pnlClinicalProgressNote #OrderSet_toggle").addClass("hidden");
                }
            }
        });

    },
    searchOrderSets_DBCall: function (PageNumber, RowsPerPage, OrderSetId) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var IsActive = 1
        var objData = {};

        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = IsActive;
        objData["RowsPerPage"] = RowsPerPage;

        objData["SpecialtyIds"] = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfSpecialtyId').val()
        objData["ProviderIds"] = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfProviderId').val();

        objData["commandType"] = "LOAD_ORDER_SET";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },

    CheckPervileges: function (response) {
        if (response.status != false) {
            var Privilege_Data = response.Privilege_JSON;
            $.each(Privilege_Data, function (i, item) {

                if (item.FormName == "Miscellaneous_eSuperbill") {
                    Clinical_ProgressNote.eSuperbillPermissionsFromNote(item.PermissionsResponseModel[0].IsAccessible);
                }
                else if (item.FormName == "Notes_Sign") {
                    if (item.PermissionsResponseModel[0].IsAccessible == true) {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnSign').removeClass('hidden');
                    }
                    else {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnSign').addClass('hidden');
                    }
                }
                else if (item.FormName == "Notes_Co-Sign") {
                    if (item.PermissionsResponseModel[0].IsAccessible == true) {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteCoSign').removeClass('hidden');
                    }
                    else {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteCoSign').addClass('hidden');
                    }
                }
                else if (item.FormName == "Medical_Orthopedic Chart") {
                    if (item.PermissionsResponseModel[0].IsAccessible == true) {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #OrthopedicChart_toggle').removeClass('hidden');
                    }
                    else {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #OrthopedicChart_toggle').addClass('hidden');
                    }
                }
            });
        }
        else {
            utility.DisplayMessages(response.Message, 2);
        }
    },

    SetValuesBasedOnNoteType: function () {

        var IsPhoneEncounter = Clinical_ProgressNote.params.IsPhoneEncounter || false;
        var NoteTypeString = IsPhoneEncounter ? "Encounter " : "Visit ";

        $('#' + Clinical_ProgressNote.params.PanelID + '  #lblVisitDate').html(NoteTypeString + "Date<span class='required'>*</span>");
        $('#' + Clinical_ProgressNote.params.PanelID + '  #lblVisitTime').html(NoteTypeString + "Time<span class='required'>*</span>");
        $('#' + Clinical_ProgressNote.params.PanelID + '  #lblVisitReason').html((!IsPhoneEncounter ? NoteTypeString : "") + "Reason<span class='required'>*</span>");
        $('#' + Clinical_ProgressNote.params.PanelID + '  #DivDuration').toggle(IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #divfacility').toggle(!IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #GetRoomsDiv').toggle(!IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #divLinkAppointment').toggle(!IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #divUser').toggle(IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #divCaller').toggle(IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #divReceiver').toggle(IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #EncounterTypeDiv').toggle(IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #Audioupload').show();
        $('#' + Clinical_ProgressNote.params.PanelID + '  #AudioPlayer').hide();
        $('#' + Clinical_ProgressNote.params.PanelID + '  #NoteTemplate').prop('disabled', IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #VisitTime').prop('disabled', IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + '  #dtpVisitDate').prop('disabled', IsPhoneEncounter);
        $('#' + Clinical_ProgressNote.params.PanelID + ' #StickyPNSection .panel-heading > h2').html(IsPhoneEncounter ? 'Phone Encounter' : 'Progress Note');

        if (IsPhoneEncounter) {
            $('#' + Clinical_ProgressNote.params.PanelID + '  #splitterbody').attr('class', 'splitterBody active');
            $('#' + Clinical_ProgressNote.params.PanelID + '  #splitterbody').show();
        }

        if ($('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val() == 'Signed' && !$('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val() > 0) {
            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
            $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
        }
        else {
            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
        }
    },

    LoadVisitDetail: function (VisitId, PatientId, event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {

                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                params["Parent"] = Clinical_ProgressNote.params.ParentCtrl;
                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["IsPhoneEncounter"] = Clinical_ProgressNote.params.IsPhoneEncounter;

                LoadActionPan('EncounterChargeCapture', params, Clinical_ProgressNote.params.PanelID);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    LoadCDSAlerts: function (IsLoadPriviliges) {

        $("#pnlClinicalProgressNote #CDSAlert_toggle").css('display', 'none');
        $('#pnlClinicalProgressNote #CDsAlertList').html('');
        var objDeffered = $.Deferred();

        var IsLoadCDS = false;
        if ($("#mainForm  li#CDSAlert #hfCDSIDs").val() && $("#mainForm  li#CDSAlert #hfCDSIDs").val() != "") {
            IsLoadCDS = true;
        }

        if (IsLoadPriviliges == true) {

            var Permissions = [];
            Permissions.push('VIEW');
            var obj_eBill = {
                FormName: "Miscellaneous_eSuperbill",
                Permissions: Permissions
            };

            var obj_Sign = {
                FormName: "Notes_Sign",
                Permissions: Permissions
            };
            var obj_coSign = {
                FormName: "Notes_Co-Sign",
                Permissions: Permissions
            };
            var obj_OrthopedicChart = {
                FormName: "Medical_Orthopedic Chart",
                Permissions: Permissions
            };

            var data_ = [];
            data_.push(obj_eBill, obj_Sign, obj_coSign, obj_OrthopedicChart);

            Clinical_ProgressNote.GetCDSOrderSetAndPriviliges(JSON.stringify(data_), IsLoadCDS).done(function (response) {
                var res = JSON.parse(response);
                Clinical_ProgressNote.CheckPervileges(JSON.parse(res.PrivilegeData_JSON));
                if (IsLoadCDS) {
                    objDeffered.resolve(JSON.parse(res.CDSOrderSet_JSON));
                }
                else
                    objDeffered.resolve('nocds');

            });
        }
        else if (IsLoadCDS) {
            Clinical_ProgressNote.showCDSAlertDBCall().done(function (response) {
                //AST-435 BY:MAHMAD
                response = JSON.parse(response);
                //AST-435 BY:MAHMAD
                objDeffered.resolve(response);
            });
        }
        else
            objDeffered.resolve('nocds');

        objDeffered.done(function (response) {
            if (response != 'nocds') {
                if (response.status != false) {

                    if (response.OrderSetCount > 0) {

                        var cds_alerts = [];
                        var res_json = JSON.parse(response.CDSOrderSet_JSON);
                        var res_note_json = JSON.parse(response.CDSNoteOrderSetJSON);

                        $list_ul = $('<ul class="nav nav-main nav-small" id="ulCDsAlertList"></ul>');

                        $.each(res_json, function (i, item) {

                            if (cds_alerts.indexOf(item.CDSId) < 0) {
                                cds_alerts.push(item.CDSId);

                                $cds_li = $('<li class="nav-parent " onclick="Clinical_ProgressNote.SetToggle(event,this);" id="cdsalerts_' + item.CDSId + '" ><a data-toggle="tab" title="' + item.CDSTitle + '" href="#"><span onclick="Clinical_ProgressNote.OpenCDSAlert(\'' + item.CDSId + '\',\'' + item.CDSPatientStatusId + '\',event);" class="bold">' + item.CDSTitle + '</span></a></li>');

                                var order_sets = $.grep(res_json, function (item_) { return (item_.CDSId == item.CDSId) });
                                if (order_sets.length > 0) {

                                    $os_ul = $('<ul id="ul_os_' + item.CDSId + '" class="nav nav-children Of-a own-scroll" style="height: 142px;"></ul>');
                                    $.each(order_sets, function (i, item_) {

                                        var orderset_detail = '<i title="View Orderset Detail" style="float: right;margin-top: 3px;font-size: 15px;" onclick="Clinical_ProgressNote.OrderSetDetail(\'' + item_.CDSId + '\',\'' + item_.OrderSetId + '\',event,\'' + item_.OrderSetName + '\');" class="fa fa-ellipsis-h fa-11 black"></i>';
                                        var link_orderset = '<i title="Attach Orderst to Note" style="font-size: 12px; margin-left: -10px; float: left; margin-top: 6px;" onclick="Clinical_ProgressNote.LinkOrderSetWithNote(event,\'' + item_.OrderSetId + '\',\'' + item_.CDSId + '\');" class="fa fa-circle-o-notch green"></i>';

                                        var obj_deleted = $.grep(res_note_json, function (item_1) { return (item_1.OrderSetId == item_.OrderSetId && item_1.CDSId == item_.CDSId && item_1.IsDeleted.toLowerCase() == 'true' && item_1.NoteId == Clinical_ProgressNote.params.NotesId) });
                                        var obj_linked = $.grep(res_note_json, function (item_1) { return (item_1.OrderSetId == item_.OrderSetId && item_1.CDSId == item_.CDSId && item_1.IsDeleted.toLowerCase() == 'false' && item_1.NoteId == Clinical_ProgressNote.params.NotesId) });

                                        if (obj_linked.length > 0) {

                                            var delete_row = ' <i  style="font-size: 12px; margin-left: -10px; float: left; margin-top: 6px;" onclick="Clinical_ProgressNote.DeleteOrderSetFromNote(event,\'' + obj_linked[0].NoteOSId + '\',\'' + obj_linked[0].OrderSetId + '\');" class="fa fa-close fa-1 red"></i>';
                                            $os_ul.append('<li  id="os_' + obj_linked[0].OrderSetId + '" ><a href="#">' + delete_row + '<span style="cursor: default;"><b>' + item_.OrderSetName + '</b></span>' + orderset_detail + '</a></li>');

                                        } else if (obj_deleted.length > 0) {
                                            // ignore that order set
                                        }
                                        else {
                                            if (item_.OrderSetId != "")
                                                $os_ul.append('<li  id="os_' + item_.OrderSetId + '" ><a href="#">' + link_orderset + '<span style="cursor: default;"><b>' + item_.OrderSetName + '</b></span>' + orderset_detail + '</a></li>');
                                        }
                                    });

                                    $cds_li.append($os_ul);
                                    $list_ul.append($cds_li);
                                }
                            }
                        });

                        $('#pnlClinicalProgressNote #CDsAlertList').append($list_ul);
                        $("#pnlClinicalProgressNote #CDSAlert_toggle").css('display', 'block');
                    }
                    else {
                        $("#pnlClinicalProgressNote #CDSAlert_toggle").css('display', 'none');
                    }
                }
            }

        });

    },

    SetToggle: function (event, obj) {

        if (event != null) {
            event.stopPropagation();
        }

        $(obj).siblings().each(function (i, item) {

            if ($(item).hasClass("nav-expanded")) {
                $(item).removeClass("nav-expanded");
                $(item).removeClass("active");
            }
        });

        if ($(obj).siblings().length <= 0) {

            $(obj).toggleClass("active nav-expanded", !$(obj).hasClass("nav-expanded"));
            Clinical_ProgressNote.Toggle_Element = obj;
        }
        else if (Clinical_ProgressNote.Toggle_Element == obj) {
            $(obj).removeClass("active nav-expanded");
            Clinical_ProgressNote.Toggle_Element = null;
        }
        else {
            $(obj).addClass("active nav-expanded");
            Clinical_ProgressNote.Toggle_Element = obj;
        }


    },

    OpenCDSAlert: function (CDSId, CDSPatientStatusId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        if (!CDSId) {
            utility.DisplayMessages("No record found.", 3);
        }
        else {
            var params = [];

            params["CDSId"] = CDSId;
            params["CDSPatientStatusId"] = CDSPatientStatusId;
            params["mode"] = "Edit";
            params["PatientId"] = Clinical_ProgressNote.params.patientID;
            params["NotesId"] = Clinical_ProgressNote.params.NotesId;
            params["FromAdmin"] = "0";
            params["ParentCtrlPanelID"] = Clinical_ProgressNote.params.PanelID;
            params["ParentCtrl"] = "clinicalTabProgressNote";

            LoadActionPan('Clinical_CDSAlertDetail', params);
        }
    },

    LinkOrderSetWithNote: function (event, OrderSetId, CDSId) {

        if (event != null) {
            event.stopPropagation();
        }

        // all compunents
        var components_ = "";
        Clinical_OrderSetDetails.AddOrderSetToNoteDBCAll(components_, OrderSetId, Clinical_ProgressNote.params.NotesId, CDSId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                // call add order set to Note function.
                Clinical_OrderSetDetails.AddToNoteDBCall(components_, OrderSetId, Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.patientID).done(function (inner_response) {

                    inner_response = JSON.parse(inner_response);
                    if (inner_response.status != false) {
                        if (inner_response.listOrderSet.length > 0) {
                            var ordersetList = inner_response.listOrderSet[0];
                            Clinical_ProgressNote.LoadOrderSetToNote(inner_response, ordersetList.OrderSetName, OrderSetId, true, ordersetList.Comments);
                        }
                    }
                    else {
                        utility.DisplayMessages(inner_response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadOrderSetToNote: function (inner_response, OrderSetName, orderSetId, fromCDS, Comments, hideAlert) {

        if (inner_response.ProcedureSoapCount > 0) {
            Clinical_Procedures.createProceduresBodyHTML(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
        }

        if (inner_response.ProblemListSoapCount > 0) {
            Clinical_ProblemLists.createProblemListBodyHTML(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
        }

        if (inner_response.MedicationSoapCount > 0) {
            Clinical_LabOrder.createMedicationBodyHTML(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, null, true);
        }

        if (inner_response.radiologySoapCount > 0) {
            Clinical_RadiologyOrder.createRadiologyBodyHTML(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
        }

        if (inner_response.PatientEducationSoapCount > 0) {
            Clinical_PatientEducation.createPatientEducationBodyHTML(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true, true);
        }
        if (inner_response.VaccineCount > 0 || inner_response.TheraeuticInjectionCount > 0) {
            Clinical_ImmunizationDetail.createImmunizationBodyHTMLForSoap(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', "", true, true, true);
        }
        if (inner_response.MedicationsSoapCount > 0) {
            Clinical_Medications.createMedicationBodyHTMLWithOutReconcile(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', "", null, true, true);
        }
        if (inner_response.ProcedureOrderSoapCount > 0) {
            Clinical_ProcedureOrder.createProcedureOrderBodyHTML(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', "");
        }
        var AppointmentId = null;
        if (inner_response.Appointment != null && inner_response.Appointment != '') {

            var Appointment = JSON.parse(inner_response.Appointment);
            if (Appointment.length > 0) {

                if (Appointment[0].ErrorMessage != null && Appointment[0].ErrorMessage != '') {
                    utility.DisplayMessages(Appointment[0].ErrorMessage, 2);

                } else if (Appointment[0].AppointmentId != null && Appointment[0].AppointmentId != '') {
                    AppointmentId = Appointment[0].AppointmentId;
                }
            }
        }

        if (inner_response.FollowUp) {

            var FollowUp = JSON.parse(inner_response.FollowUp);
            if (FollowUp.length > 0) {

                if (FollowUp[0].ErrorMessage != null && FollowUp[0].ErrorMessage != '') {
                    utility.DisplayMessages(FollowUp[0].ErrorMessage, 2);
                }
                else if (FollowUp[0].FollowUpText || FollowUp[0].Comments) {
                    Clinical_FollowUpAppointment.params["OrderSetId"] = FollowUp[0].OrderSetId;

                    var cval = "";
                    var ctype = "";
                    var Soap = FollowUp[0].FollowUpText.replace("Patient needs to be seen again in", "").trim().split(" ");
                    if (Soap.length == 2) {
                        cval = Soap[0];
                        ctype = Soap[1];
                    }
                    Clinical_FollowUpAppointment.CreateFollowUp_SOAP_TextProgressNote(FollowUp[0].FollowUpText, FollowUp[0].Comments, true, cval, ctype);
                }
            }
        }
        else {
            var FollowUp_ = JSON.parse(inner_response.FollowUp);
            if (FollowUp_.length > 0) {
                if (FollowUp_[0].ErrorMessage)
                    utility.DisplayMessages(FollowUp_[0].ErrorMessage, 2);
            }
        }

        if (inner_response.ReferralListCount > 0) {
            Patient_Referrals.createReferralBodyHTML(inner_response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', "", true);
        }

        var ComponentsToRemove = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfOSComponents').val();

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ordersets').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if ($(CompnentSelector).length > 0) {
                $(CompnentSelector).append(' <li class="OrderSetsComponent" NoteComponentId="NCDummyId"> <header>' +
                            '<clinical_ordersets title="Order Sets"  id="' + this.id + '" class="NotesComponent">' +
                            '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'OrderSets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Order Sets">Order Sets</a> ' +
                                            '<a id="OSCoRemove"  onclick="Clinical_ProgressNote.RemoveComponentTab(\'Order Sets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                       '</clinical_ordersets> </header></li>');
            } else {
                var insertedinPlan = false;
                var headings = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .CustomComponent').children();
                $.each(headings, function (i, item) {

                    if ($(item).text() == 'Plan') {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ImmunizationComponent').after('<li class="OrderSetsComponent" NoteComponentId="NCDummyId"> <header>' +
                            '<clinical_ordersets title="Order Sets"  id="' + this.id + '" class="NotesComponent">' +
                            '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'OrderSets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Order Sets">Order Sets</a> ' +
                                            '<a id="OSCoRemove"  onclick="Clinical_ProgressNote.RemoveComponentTab(\'Order Sets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                       '</clinical_ordersets> </header></li>');
                        //$(this).parent().append(' <li class="OrderSetsComponent" NoteComponentId="NCDummyId"> <header>' +
                        //    '<clinical_ordersets title="Order Sets"  id="' + this.id + '" class="NotesComponent">' +
                        //    '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'OrderSets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Order Sets">Order Sets</a> ' +
                        //                    '<a id="OSCoRemove"  onclick="Clinical_ProgressNote.RemoveComponentTab(\'Order Sets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                        //               '</clinical_ordersets> </header></li>');
                        insertedinPlan = true;
                    }

                });
                if (insertedinPlan == false) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML').append('<li class="OrderSetsComponent" NoteComponentId="NCDummyId"> <header>' +
                            '<clinical_ordersets title="Order Sets"  id="' + this.id + '" class="NotesComponent">' +
                            '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'OrderSets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Order Sets">Order Sets</a> ' +
                                            '<a id="OSCoRemove"  onclick="Clinical_ProgressNote.RemoveComponentTab(\'Order Sets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                       '</clinical_ordersets> </header></li>');
                }
            }
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ordersets').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        var attachedOS = $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().find('div');


        var OrderSet_ID = "";
        if (typeof fromCDS != typeof undefined && fromCDS != null && fromCDS) {
            OrderSet_ID = orderSetId;
        }
        else {
            OrderSet_ID = Clinical_OrderSetDetails.params.OrderSetId;
        }

        var attachedOSName = $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().find('#Cli_OrderSets_Main' + OrderSet_ID + '  ul').html();

        var checkAttachedOSName = attachedOSName;
        if (typeof attachedOSName != typeof undefined && attachedOSName != null && attachedOSName != "") {
            checkAttachedOSName = attachedOSName.split('<br>')[0].trim();
        }
        var CommentsVar = "";
        if (typeof Comments != typeof undefined && Comments != null && Comments != "") {
            CommentsVar = Comments;
        }
        if (checkAttachedOSName != OrderSetName) {

            var $mainDivFollowUp = $(document.createElement('div'));
            var $sectionBodyFollowUp = $(document.createElement('section'));
            var $detailsDiv = $(document.createElement('div'));

            $sectionBodyFollowUp.attr('id', "Cli_OrderSets_Main" + OrderSet_ID);
            if (Clinical_ProgressNote.NewDefaultOrderSetId && Clinical_ProgressNote.NewDefaultOrderSetId == OrderSet_ID) {
                $sectionBodyFollowUp.attr('IsDefaultOrderSet', "1");
            }
            $sectionBodyFollowUp.append(' <div class="pull-right hidden">' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_OrderSets_Main" + OrderSet_ID + '"  ><i class="fa fa-times"></i></a>'
                + '</div> ');

            var $listFollowUp = $(document.createElement('ul'));
            //  var txt = $.parseHTML(SoapText);

            $listFollowUp.append(OrderSetName + (CommentsVar != '' ? '<br>' + CommentsVar : ''));
            $detailsDiv.append($listFollowUp);
            $sectionBodyFollowUp.append($detailsDiv);
            $mainDivFollowUp.append($sectionBodyFollowUp);
            $mainDivFollowUp.html()

            $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().addClass('initialVisitBody');

            $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().append($mainDivFollowUp.html());
        } else {
            $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().find('#Cli_OrderSets_Main' + OrderSet_ID + '  ul').html(OrderSetName + (CommentsVar != '' ? '<br>' + CommentsVar : ''));
        }
        $("#" + Clinical_ProgressNote.params.PanelID + " #ddlOrderSet").val(orderSetId);

        Clinical_ProgressNote.saveComponentSOAPText('Order Sets', true);

        if (!hideAlert) {
            utility.DisplayMessages(inner_response.Message, 1);
        }


        //Load CDS Alerts
        if (typeof fromCDS != typeof undefined && fromCDS != null && fromCDS) {
        }
        else {
            Clinical_ProgressNote.LoadCDSAlerts();
        }
    },

    LinkCDSOrderSetWithNote: function (OrderSetIds, CDSId, hideAlert) {

        // all compunents
        var components_ = "";
        Clinical_OrderSetDetails.AddOrderSetToNoteDBCAll(components_, OrderSetIds, Clinical_ProgressNote.params.NotesId, CDSId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                // call add order set to Note function.
                Clinical_ProgressNote.LinkCDSOrderSetWithNote_DBCall(components_, OrderSetIds, Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.patientID).done(function (inner_response) {

                    inner_response = JSON.parse(inner_response);
                    if (inner_response.status != false) {
                        if (inner_response.ExistsMedicationsDrugName != "") {
                            utility.DisplayMessages(inner_response.ExistsMedicationsDrugName + " already Exists", 3);
                        }
                        var orderset_JSON = inner_response.OrderSet_JSON;
                        var ordersetList = inner_response.listOrderSet;
                        $.each(orderset_JSON, function (i, item) {
                            item = JSON.parse(item);
                            var orderSetId = "";
                            var orderSetName = "";
                            var comments = "";
                            $.each(ordersetList, function (ii, item1) {
                                if (item.OrdersetId == item1.OrderSetId) {
                                    orderSetId = item1.OrderSetId;
                                    orderSetName = item1.OrderSetName;
                                    comments = item1.Comments;
                                }
                            });
                            if (orderSetId != "" && orderSetName != "") {
                                Clinical_ProgressNote.LoadOrderSetToNote(item, orderSetName, orderSetId, true, comments, true);
                            }

                        });
                        // Clinical_ProgressNote.LoadCDSAlerts();
                        if (!hideAlert) {
                            utility.DisplayMessages(inner_response.Message, 1);
                        }

                    }
                    else {
                        utility.DisplayMessages(inner_response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LinkCDSOrderSetWithNote_DBCall: function (OrderSetComponents, OrderSetIds, NoteId, PatientId) {

        var objData = {};

        objData["OrderSetIds"] = OrderSetIds;
        objData["NotesId"] = NoteId;
        objData["PatientId"] = PatientId;
        objData["OrderSetComponents"] = OrderSetComponents;
        objData["commandType"] = "attach_note_order_sets_to_note";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },

    DeleteOrderSetFromNote: function (event, NoteOSId, OrderSetId) {

        if (event != null) {
            event.stopPropagation();
        }
        //utility.myConfirm('Do you want to delete this Order Set for this Note?', function () {
        Clinical_OrderSetDetails.detachOrderSetFromNotes(OrderSetId);
        //}, function () {
        //},
        //    'Confirm Delete'
        //    );
    },

    DeleteOrderSetCompunentsFromSopeText: function (response) {

        var orderset_compunents_Ids = JSON.parse(response.OrderSetIDS_JSON);

        // Delete Problems from Sope Text
        var problems_ = $.grep(orderset_compunents_Ids, function (a) {
            return a.FromTable == "Problems";
        });

        $.each(problems_, function (i, item) {
            $("#pnlClinicalProgressNote #frmClinicalProgressNote section[id*='Cli_Problems_Main" + item.PKID + "']").remove();
        });

        // Delete Procedures from Sope Text
        var procedures_ = $.grep(orderset_compunents_Ids, function (a) {
            return a.FromTable == "Procedures";
        });

        $.each(procedures_, function (i, item) {
            $("#pnlClinicalProgressNote #frmClinicalProgressNote section[id*='Cli_Procedures_Main" + item.PKID + "']").remove();
        });

        // Delete PatientEducation from Sope Text
        var patientEducation_ = $.grep(orderset_compunents_Ids, function (a) {
            return a.FromTable == "PatientEducation";
        });

        $.each(patientEducation_, function (i, item) {
            $("#pnlClinicalProgressNote #frmClinicalProgressNote section[id*='Cli_PatientEducation_Main" + item.PKID + "']").remove();
        });

        // Delete Referrals from Sope Text
        var referrals_ = $.grep(orderset_compunents_Ids, function (a) {
            return a.FromTable == "Referrals";
        });

        $.each(referrals_, function (i, item) {
            $("#pnlClinicalProgressNote #frmClinicalProgressNote section[id*='Patient_Referrals_Main" + item.PKID + "']").remove();
        });

        // Delete LabOrder from Sope Text
        var labOrder_ = $.grep(orderset_compunents_Ids, function (a) {
            return a.FromTable == "LabOrder";
        });

        $.each(labOrder_, function (i, item) {
            $("#pnlClinicalProgressNote #frmClinicalProgressNote section[id*='Cli_LabOrderDetail_Main" + item.PKID + "']").remove();
        });

        // Delete RadiologyOrder from Sope Text
        var radiologyOrder_ = $.grep(orderset_compunents_Ids, function (a) {
            return a.FromTable == "RadiologyOrder";
        });

        $.each(radiologyOrder_, function (i, item) {
            $("#pnlClinicalProgressNote #frmClinicalProgressNote section[id*='Cli_RadiologyOrderDetail_Main" + item.PKID + "']").remove();
        });

        $("#pnlClinicalProgressNote #frmClinicalProgressNote #btnSave").click();
    },

    deleteOrderSetFromNoteDBCall: function (NoteOSId, OrderSetId) {

        var objData = {};

        objData["NoteOSId"] = NoteOSId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["OrderSetId"] = OrderSetId;
        objData["commandType"] = "delete_note_order_set";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "NoteOrderSet");
    },

    OrderSetDetail: function (CDSId, orderSetId, event, OSName, ParentCtrl, OrderSetType) {
        if (event != null)
            event.stopPropagation();
        if (orderSetId == "" || orderSetId == "undefined") {
            utility.DisplayMessages("No record found.", 3);
        }
        else {
            if (OrderSetType == "defaultOrderSet") {
                Clinical_ProgressNote.IsDefaultOrderSet = "1";
            } else { Clinical_ProgressNote.IsDefaultOrderSet = "0"; }

            Clinical_ProgressNote.CheckIfAnyOrderSetIsAssociatedWithNote(Clinical_ProgressNote.params.NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false && response.Result != false)
                    Clinical_ProgressNote.OpenOrderSetDetail(CDSId, orderSetId, event, OSName, ParentCtrl, OrderSetType, response.Result);
                else
                    Clinical_ProgressNote.OpenOrderSetDetail(CDSId, orderSetId, event, OSName, ParentCtrl, OrderSetType);
            });
        }
    },
    OpenOrderSetDetail: function (CDSId, orderSetId, event, OSName, ParentCtrl, OrderSetType, OrderSetExistOnNote) {
        var params = [];
        if (CDSId == null)
            params["ParentCtrl"] = "Clinical_OrderSets";
        else
            params["ParentCtrl"] = "clinicalTabProgressNote";
        if (typeof ParentCtrl != typeof undefined && ParentCtrl != null)
            params["ParentCtrl"] = ParentCtrl;
        params["OrderSetId"] = orderSetId;
        params["PatientId"] = Clinical_ProgressNote.params.patientID;
        params["CDSId"] = CDSId;
        params["NoteId"] = Clinical_ProgressNote.params.NotesId;
        params["IsNotes"] = true;
        params["mode"] = "View";
        params["CustomMode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrlPanelID"] = Clinical_ProgressNote.params.PanelID;
        params["OSName"] = OSName;
        params["OrderSetExistOnNote"] = OrderSetExistOnNote ? OrderSetExistOnNote : false;
        if (CDSId == null) {
            Clinical_ProgressNote.LoadPatientAndOrdProblems_DBCALL(orderSetId, Clinical_ProgressNote.params.patientID).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (OrderSetExistOnNote) {
                        params["listProblem"] = response.listProblem;
                        params["OrderSetType"] = OrderSetType;
                        LoadActionPan('OrderSet_OrdAndPatientProbSelection', params);
                    }
                    else {
                        if (typeof ParentCtrl != typeof undefined && ParentCtrl != null) {
                            params["DirectFromNotes"] = true;
                        }
                        LoadActionPan('Clinical_OrderSetDetails', params);
                    }
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }
        else
            LoadActionPan('Clinical_OrderSetDetails', params);
    },
    LoadPatientAndOrdProblems_DBCALL: function (OrderSetId, PatientId) {
        var objData = {
        };
        objData["OrderSetId"] = OrderSetId;
        objData["PatientId"] = PatientId;
        objData["commandType"] = "Load_Patient_And_Ord_Problems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    showCDSAlertDBCall: function () {

        var objData = {};

        objData["CDSId"] = $("#mainForm  li#CDSAlert #hfCDSIDs").val();
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "load_cds_ordersets";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "CDS", "CDS");
    },

    GetCDSOrderSetAndPriviliges: function (PrivilegeData, IsLoadCDS) {

        var objData = {};

        objData["PrivilegeData"] = PrivilegeData;
        objData["IsLoadCDS"] = IsLoadCDS;
        objData["CDSId"] = $("#mainForm  li#CDSAlert #hfCDSIDs").val();
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "Get_CDS_OrderSet_And_Priviliges";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "CDS", "CDS");
    },


    BindUserName: function () {

        var UserName = $('#pnlClinicalProgressNote #txtUser').val();

        if (UserName) {

            utility.Keyupdelay(function () {

                var AllPatients = utility.GetUserArray(UserName, 1, 1).done(function (response) {

                    $('#pnlClinicalProgressNote #txtUser').autocomplete({
                        autoFocus: true,
                        source: response,
                        minLength: 0,
                        open: function (event, ui) {
                            disable = true
                        },
                        close: function (event, ui) {
                            disable = false; $(this).focus();
                        },
                        select: function (event, ui) {
                            setTimeout(function () {
                                $('#pnlClinicalProgressNote #txtUser').val(ui.item.value);
                                $('#pnlClinicalProgressNote #hfUserId').val(ui.item.id);
                                if ($('#frmClinicalProgressNote').data('bootstrapValidator') != null && typeof $('#frmClinicalProgressNote').data('bootstrapValidator') != 'undefined') {
                                    $('#frmClinicalProgressNote').bootstrapValidator('revalidateField', 'User');
                                }
                            }, 100);
                        }
                    }).blur(function () {
                        setTimeout(function () {
                            utility.ValidateAutoComplete($('#frmClinicalProgressNote #txtUser'), "frmClinicalProgressNote #hfUserId", false, null, null, null);
                        }, 200);
                    });

                    $('#frmClinicalProgressNote #txtUser').autocomplete("search", "");
                });
            });
        }
    },

    domReadyFunctions: function () {

        if (ClinicalMenuSettings != null && ClinicalMenuSettings.selectClinicalMenu != null)
            ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');

        $('#' + Clinical_ProgressNote.params.PanelID + ' #menu-toggle1').affix();

        //idle Users, auto save after 5 mins, Dr Hijjar Requirement, Implemeneted By Azhar Shahzad on July 14, 2016
        if (Number(globalAppdata["SessionTimout"]) > 5) {

            var myVar = setInterval(function () {

                if (params.NotesId && Number(params.NotesId) > 0 && $('#pnlClinicalProgressNote').is(':visible') && Number(globalAppdata["SessionTimout"]) > 5) {

                    var IdleTime = globalAppdata["SessionTimout"] - ($.idleTimer('getRemainingTime') / 1000 / 60);

                    if (IdleTime > 1 && parseInt(IdleTime, 10) % 5 == 0) {

                    }

                } else {
                    clearInterval(myVar);
                }
            }, 60000);
        }

        $("#pnlClinicalProgressNote .ui-draggable").mouseup(function () {
            $("#pnlClinicalProgressNote .border-dash").remove();
        });

        //Affix for notes Panel's
        $('#StickyPNSection').on('affix.bs.affix', function () {

            if ($('#StickyPNSection').css('position') != 'undefined')
                $('#StickyPNSection').css('position', '');
            if ($('#StickyPNSection').css('top') != 'undefined')
                $('#StickyPNSection').css('top', '');
            var margintop = $('#StickyPNSection').outerHeight();
            if (Clinical_ProgressNote.params.IsPhoneEncounter == true && $(window).scrollTop() > 40) {
                margintop += 40;
            }
            else {
                var tempMarginPatch = (margintop - 22);
                if (tempMarginPatch > 10) tempMarginPatch = 10;
                $("#InitialOfficeVisit").css("margin-top", tempMarginPatch);
            }
        });
        $('#ActionsInitialOfficeVisit').on('affix.bs.affix', function () {
            if ($('#ActionsInitialOfficeVisit').css('position') != 'undefined')
                $('#ActionsInitialOfficeVisit').css('position', '');
            var margintop = 149 + $('#StickyPNSection').outerHeight();
            $('#ActionsInitialOfficeVisit').css('top', margintop);
        });


        $("#" + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote").find("input,select").change(function () {
            Clinical_ProgressNote.params.triggerCount = 0;
        });

        var $ProgressNoteDiv = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

        $ProgressNoteDiv.on("dblclick", "li section", Clinical_ProgressNote.OnDoubleClickComponent);
        $ProgressNoteDiv.on('mouseenter', '.hoverToggle', Clinical_ProgressNote.OnMouseEventOnROSToggle);
        $ProgressNoteDiv.on('mouseleave', '.hoverToggle', Clinical_ProgressNote.OnMouseleaveEventOnROSToggle);
        $ProgressNoteDiv.on("click", "section .btnPNC_Remove", Clinical_ProgressNote.OnRemoveHandler);
        $ProgressNoteDiv.on("click", "section .btnPNC_Edit", Clinical_ProgressNote.OnEditHandler);
        $ProgressNoteDiv.on('click', 'li', function (e) {
            e.stopPropagation();
        });

        //Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit').on('mouseenter', 'li section', function (e) {
            $(this).children('div.pull-right').removeClass('hidden');
        });
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit').on("mouseleave", 'li section', function (e) {
            $(this).children('div.pull-right').addClass('hidden');
        });

        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit').on("mouseleave", 'li section', function (e) {
            $(this).children('div.pull-right').addClass('hidden');
        });

        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit').on("mouseenter", 'li', function (e) {
            $(this).children('.editBtn').removeClass('hidden');
        });
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit').on("mouseleave", 'li', function (e) {
            $(this).children('.editBtn').addClass('hidden');
        });

        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit').on("mouseenter", 'li header', function (e) {
            $(this).find('.closeBtn').removeClass('hidden');
            $(this).css('background', '#EAF1F8');
        });

        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit').on("mouseleave", 'li header', function (e) {
            $(this).find('.closeBtn').addClass('hidden');
            //$(this).css('background', '#fff');
        });
        var $customformli = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit clinical_customform').closest('li');
        $customformli.find('header').off("mouseenter")
        $customformli.find('header').off("mouseleave");
        $customformli.off("mouseenter");
        $customformli.off("mouseleave");


        $ProgressNoteDiv.on('mouseenter', 'li header', Clinical_ProgressNote.OnMouseEnterToggleEditIcon);
        $ProgressNoteDiv.on('mouseleave', 'li header', Clinical_ProgressNote.OnMouseLeaveToggleEditIcon);

        $ProgressNoteDiv.on('mouseenter', 'li', function (e) {
            $(this).children('.editBtn').removeClass('hidden');
        });
        $ProgressNoteDiv.on('mouseleave', 'li', function (e) {
            $(this).children('.editBtn').addClass('hidden');
        });
        $ProgressNoteDiv.on("mouseenter", "li section", function (e) {
            $(this).children('div.pull-right').removeClass('hidden');
        });
        $ProgressNoteDiv.on("mouseleave", "li section", function (e) {
            $(this).children('div.pull-right').addClass('hidden');
        });
        $('#' + Clinical_ProgressNote.params.PanelID).on("mouseenter", "#reasonHeading", function (e) {
            $(this).find('.btnPNC_Edit').removeClass('hidden');
        });
        $('#' + Clinical_ProgressNote.params.PanelID).on("mouseleave", "#reasonHeading", function (e) {
            $(this).find('.btnPNC_Edit').addClass('hidden');
        });

        $ProgressNoteDiv.on("mouseenter", '#reasonLi', function (e) {
            $('#btnEditReason').removeClass('hidden');
        });

        $ProgressNoteDiv.on("mouseleave", '#reasonLi', function (e) {
            $('#btnEditReason').addClass('hidden');
        });

        Clinical_ProgressNote.initilizeEditableContent();
    },

    OnMouseEnterToggleEditIcon: function (e) {
        var $this = $(this);
        $this.find('.closeBtn').removeClass('hidden');
        $this.find('.btnPNC_Edit').removeClass('hidden');
        $this.css('background', '#EAF1F8');
    },

    OnMouseLeaveToggleEditIcon: function (e) {
        var $this = $(this);
        $this.find('.closeBtn').addClass('hidden');
        $this.find('.btnPNC_Edit').addClass('hidden');
        $this.removeAttr('style');
        //$this.css('background', '#fff');
    },

    OnMouseEventOnROSToggle: function (e) {

        var $this = $(this);

        $this.addClass("font-xs");
        if ($this.hasClass('red')) {
            $this.removeClass('red');
            $this.addClass('green');
            $this.html(' ( - ) ');
        } else {
            $this.addClass('red');
            $this.removeClass('green');
            $this.html(' ( + ) ');
        }
    },

    OnMouseleaveEventOnROSToggle: function (e) {

        var $this = $(this);

        $this.removeClass("font-xs");
        if ($this.hasClass('red')) {
            $this.removeClass('red');
            $this.addClass('green');
            $this.html(' ( - ) ');
        } else {
            $this.addClass('red');
            $this.removeClass('green');
            $this.html(' ( + ) ');
        }
    },

    //Checks Permissions to show/hide eSuperbill info (Notes Flow)
    eSuperbillPermissionsFromNote: function (IsAccessible) {

        //Start//Abid Ali//26-7-2016//Check eSuperbill permissions and show hide billing info button

        if (IsAccessible == false) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "none");
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "none");
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").closest('li').css("display", "none");
            $('#' + Clinical_ProgressNote.params.PanelID + '  #frmClinicalProgressNote #btnBillingInfo').hide();
            $('#' + Clinical_ProgressNote.params.PanelID + '  #frmClinicalProgressNote #clinicalMenu_BillingInfo').hide();
        }
        else {
            $('#' + Clinical_ProgressNote.params.PanelID + '  #frmClinicalProgressNote #btnBillingInfo').show();
            $('#' + Clinical_ProgressNote.params.PanelID + '  #frmClinicalProgressNote #clinicalMenu_BillingInfo').show();
            $('#' + Clinical_ProgressNote.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "");
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "");
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").closest('li').css("display", "");
        }

        //End//Abid Ali//26-7-2016//Check eSuperbill permissions and show hide billing info button
    },

    // Reseting Paramaters set for Notes in global params
    ResetParams: function () {

        if (params['NotesId'] != null) {

            params['ScheduleReason'] = null;
            params["NotesVisitTime"] = null;
            params["NotesVisitId"] = -1;
            params["NotesVisitDate"] = null;
            params["NotesFacilityId"] = -1;
            params["NotesProviderId"] = -1;
            params["CurrentNotesProviderId"] = -1;
            params['NotesRoom'] = null;
            params["NotesFacilityName"] = null;
            params["NotesProviderName"] = null;
            params['ForProgressNote'] = null;
        }
    },

    //Binding  Validation to form
    ValidateProgressNotes: function (FrmCtrl) {

        $('#' + FrmCtrl).bootstrapValidator('destroy');
        $('#' + FrmCtrl)
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  VisitDate: {
                      group: '.col-sm-4',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VisitTime: {
                      group: '.col-sm-4',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  'Facility': {
                      group: '.col-sm-4',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  NoteType: {
                      group: '.col-sm-4',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  User: {
                      group: '.col-sm-4',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },

                  'Provider[]': {
                      group: '.col-sm-4',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VisitReason: {
                      group: '.col-sm-4',
                      enabled: false,
                      validators: {
                          callback: {
                              message: '',
                              callback: function (value, validator, $field) {
                                  $('#pnlClinicalProgressNote #divVisitReason').find('small').remove();
                                  if ($($field).val() != '') {

                                      setTimeout(function ($field) {
                                          $('#pnlClinicalProgressNote #divVisitReason').addClass('has-success');
                                          $('#pnlClinicalProgressNote #divVisitReason').append('<i class="form-control-feedback bv-icon-input-group glyphicon glyphicon-ok" style="display: block;" data-bv-icon-for="VisitReason"></i>');
                                          $('#pnlClinicalProgressNote #divVisitReason').find('small').remove();
                                      }, 300, $field);
                                      return true;
                                  }
                                  else {
                                      setTimeout(function ($field) {
                                          $('#pnlClinicalProgressNote #divVisitReason').addClass('has-error');
                                          $('#pnlClinicalProgressNote #divVisitReason').append('<i class="form-control-feedback glyphicon glyphicon-remove" style="display: block;" data-bv-icon-for="VisitReason"></i>');
                                          $('#pnlClinicalProgressNote #divVisitReason').find('small').remove();
                                      }, 300, $field);
                                      return false;
                                  }

                              }
                          }
                      }
                  },
              }
          })
.on('success.form.bv', function (e) {
    e.preventDefault();
    if (Clinical_ProgressNote.params.triggerCount != 1) {
        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
            Clinical_PhoneEncounter.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"), true);
        }
        else {
            Clinical_Notes.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"));
        }
        //Begin Added By Azeem Raza Tayyab on 31-Mar-2017 to Fix Issue EMR-3453
        if (Clinical_Notes.params.mode == "Edit") {
            Clinical_ProgressNote.FillNotes(null, Clinical_ProgressNote.params.NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    Clinical_ProgressNote.params["VisitId"] = Clinical_Notes_detail.VisitId;
                }
            });
        }
        //End Added By Azeem Raza Tayyab on 31-Mar-2017 to Fix Issue EMR-3453
        Clinical_ProgressNote.params.triggerCount = 1;
    }
});
    },


    BindReferralProvider: function () {
        var Ctrl = $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #txtRefProvider");
        var hfCtrl = $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #hfRefProvider");
        var func = function () {
            return utility.GetRefProviderArray(Ctrl.val())
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindFacility: function () {

        var shortName = $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #txtFacility").val();
        utility.GetFacilityArray(shortName).done(function (response) {

            $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #txtFacility").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #txtFacility").val(ui.item.value);
                        $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #hfFacility").val(ui.item.id);
                        if ($("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #lnkFacilityEdit").css("display") == "none") {
                            $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #lnkFacilityEdit").css("display", "inline");
                            $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #lblFacility").css("display", "none");
                        }

                    }, 100);

                }
            });
            //$("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalNotes #txtRefProvider").autocomplete("search");
        });
    },

    BindBlockReason: function () {

        var shortName = $("#frmClinicalProgressNote input#txtVisitReason").val();
        Clinical_ProgressNote.GetBlockHours(shortName).done(function (response) {

            $("#frmClinicalProgressNote input#txtVisitReason").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#pnlClinicalProgressNote #hfVisitReason").val(ui.item.id); // add the selected id
                    }, 100);
                }
            });

            $("#frmClinicalProgressNote input#txtVisitReason").autocomplete("search");
        });
    },

    GetBlockHours: function (name, IsGetAll) {

        var AllBlockReasons = [];
        var dfd = new $.Deferred();

        Clinical_ProgressNote.LoadBlockhoursDBCall(name).done(function (responseData) {

            responseData = JSON.parse(responseData);
            if (responseData.status != false) {
                if (responseData.ResonsCount > 0) {
                    var Resons = JSON.parse(responseData.ResonsLoad_JSON);
                    $.each(Resons, function (i, item) {

                        AllBlockReasons.push({
                            id: item.ScheduleReasonId, value: item.ShortName
                        });
                    });
                }
            }

            dfd.resolve(AllBlockReasons);
        });

        return dfd.promise();
    },

    LoadBlockhoursDBCall: function (name) {

        var objData = {};
        objData["ShortName"] = name;
        objData["commandType"] = "REASONS_LOOKUP_AUTOCOMPLETE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    /*
           Purpose: this function get the rooms ddl based on facility selected
           Author: Muhammad Azhar Shahzad
           Created Date: March 24,2016
       */

    GetRooms: function (FacilityId) {

        var objDeffered = $.Deferred();
        var self = $('#' + Clinical_ProgressNote.params.PanelID);

        if (FacilityId == "") {
            self.find('#GetRoomsDiv > select').val("");
            self.find('#GetRoomsDiv > select').val("");
            self.find('#GetRoomsDiv > select option[value!=""]').remove();
        }
        else {

            self.find('#GetRoomsDiv > select').attr('ddlist', 'GetRooms');
            var data = "IsActive=&ID=" + FacilityId;


            self.find('#GetRoomsDiv').loadDropDowns(true, data).done(function () {
                objDeffered.resolve();
            });

        }

        return objDeffered;
    },

    Remove_Components: function (json, NotesId) {

        var objData = {};
        objData["NotesId"] = NotesId;
        objData["ComponentsIdsString"] = json;
        objData["commandType"] = "remove_components";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    loadTemplate_HTML: function (NotesId, ComponentsIdsString) {

        var objData = {};
        objData["NotesId"] = NotesId;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["TemplateId"] = $('#pnlClinicalProgressNote #NoteTemplate').val();
        objData["ComponentsIdsString"] = ComponentsIdsString;
        objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId;
        objData["commandType"] = "load_template_html";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    Remove_ComponentsDBCall: function (existingNoteComponentsString) {
        Clinical_ProgressNote.Remove_Components(existingNoteComponentsString, Clinical_ProgressNote.params.NotesId).done(function (response) { });
    },

    LoadTemplateData: function (templateName) {

        var FormId = Clinical_ProgressNote.params.PanelID + ' #frmClinicalProgressNote';
        var existingNoteComponents = [];
        var existingNoteComponentsString = "";
        $('#ProgressnoteHTML .initialVisitBody').find('header').each(function () {
            var $component = $(this);
            if ($component.find('.NotesComponent').attr('title') != 'Billing Information') {
                if (($component.find('.NotesComponent').attr('title'))) {
                    var title = $component.find('.NotesComponent').attr('title');
                    if (title == "Social, Psychological and Behavior Hx")
                        title = "Social Psychological and Behavior Hx"
                    if (!$component.parent().hasClass('hidden')) {
                        existingNoteComponents.push($component.find('.NotesComponent').attr('title'));
                    existingNoteComponentsString += (title).replace(/\s/g, '') + ',';
                    }
                }
            }
            if ($component.parent().hasClass('hidden'))
                $component.parent().removeClass('hidden');
        });
        Clinical_ProgressNote.saveComponentSOAPTextBulk(true, true).done(function () {
            Clinical_ProgressNote.loadTemplate_HTML(Clinical_ProgressNote.params.NotesId, existingNoteComponentsString).done(function (response) {

                response = JSON.parse(response);

                if (response.status != false) {

                    var ClinicalNotesModel = JSON.parse(response.NotesFill_JSON);
                    var Clinical_Template_HTML = ClinicalNotesModel.NoteText;

                    if (Clinical_Template_HTML == null || Clinical_Template_HTML == '') {

                        $("#" + FormId + " #ProgressnoteHTML").html('<h4 class="green hidden">Subjective</h4><ul class="initialVisit ui-sortable" id="SubjectiveNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle placeholder-free-text" style="min-height: 3px !important"></li></ul>'
                            + '<h4 class="green hidden">Objective</h4><ul class="initialVisit ui-sortable" id="ObjectiveNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle placeholder-free-text" style="min-height: 3px !important"></li></ul>'
                            + '<h4 class="green hidden">Assessment</h4><ul class="initialVisit ui-sortable" id="AssessmentNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle placeholder-free-text" style="min-height: 3px !important"></li></ul>'
                            + '<h4 class="green hidden">Plan</h4><ul class="initialVisit ui-sortable" id="PlanNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle placeholder-free-text" style="min-height: 3px !important"></li></ul>'
                            + '<ul class="initialVisit ui-sortable" id="MiscellaneousNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle placeholder-free-text" style="min-height: 3px !important"></li></ul>'
                            + '<ul class="initialVisit ui-sortable" id="ProgressNoteComponentList" style="min-height: 20px;"><li class="BillingInfoComponent initialVisitBody ui-sortable-handle hidden" NoteComponentId="NCDummyId" style="width: auto; right: auto; height: auto; bottom: auto;"><header><clinical_billinginfo title="eSuperbill" id="clinicalMenu_BillingInfo" class="NotesComponent"><a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.BillingInfo();" title="eSuperbill"> eSuperbill</a></clinical_billinginfo></header></li><li class="sopTextEditable defaultli ui-sortable-handle placeholder-free-text"></li></ul>');

                        $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").html('');

                    }
                    else {

                        var NoteHTML = $("<div id='NoteHTML' class='hidden' ></div>").append(Clinical_Template_HTML);
                        var ComponentNamesHTML = [];

                        NoteHTML.find("[value^='{{ Clinical']").each(function () {
                            var $inputComponent = $(this);
                            ComponentNamesHTML.push($inputComponent.attr('value'));
                        });

                        var existingNoteComponentsStringFiltered = "";
                        var similarNoteComponents = [];
                        if (ComponentNamesHTML.length == 0) {

                            $("#" + FormId + " #ProgressnoteHTML").empty();
                            $("#" + FormId + " #ProgressnoteHTML").html(Clinical_Template_HTML);
                        }
                        else if (existingNoteComponents.length > 0) {

                            similarNoteComponents = $.map(ComponentNamesHTML, function (val, i) {

                                for (i = 0; i < existingNoteComponents.length; i++) {

                                    if (~val.toLowerCase().indexOf(existingNoteComponents[i].toLowerCase().replace(/s$/, ''))) {
                                        return existingNoteComponents[i];
                                    }
                                }
                            });
                        }

                        if (similarNoteComponents.length == 0) {

                            Clinical_ProgressNote.Remove_ComponentsDBCall(existingNoteComponentsString);

                            $("#" + FormId + " #ProgressnoteHTML").empty();
                            $("#" + FormId + " #ProgressnoteHTML").html(Clinical_Template_HTML);
                        }
                        else {

                            $('#ProgressnoteHTML .initialVisitBody').find('header').each(function () {

                                var $component = $(this);
                                var isExist = false;

                                for (i = 0; i < similarNoteComponents.length; i++) {
                                    if (($component.find('.NotesComponent').attr('title')) != undefined) {

                                        if (~($component.find('.NotesComponent').attr('title').toLowerCase()).indexOf(similarNoteComponents[i].toLowerCase().replace(/s$/, ''))) {

                                            isExist = true;
                                        }
                                    }
                                }

                                if (!isExist) {
                                    if ($component.find('.NotesComponent').attr('title') != 'Billing Information') {
                                        if (($component.find('.NotesComponent').attr('title')) != undefined) {
                                            existingNoteComponentsStringFiltered += $component.find('.NotesComponent').attr('title').replace(/\s/g, '') + ',';
                                        }
                                    }
                                }
                                else {
                                    NoteHTML.find("[value^='{{ Clinical']").each(function () {

                                        if (($component.find('.NotesComponent').attr('title')) != undefined) {

                                            if (~($(this).attr('value').toLowerCase().indexOf($component.find('.NotesComponent').attr('title').toLowerCase().replace(/s$/, '')))) {
                                                $(this).replaceWith($component.parent());
                                            }
                                        }
                                    });
                                }
                            });

                            Clinical_ProgressNote.Remove_ComponentsDBCall(existingNoteComponentsStringFiltered);

                            $("#" + FormId + " #ProgressnoteHTML").empty();
                            $("#" + FormId + " #ProgressnoteHTML").html(NoteHTML.html());
                            $("#" + FormId + " #ProgressnoteHTML #ProgressNoteComponentList").children().each(function () {
                                if ($(this).is('div') == true && $(this).children().is('li')) {
                                    $(this).children().unwrap();
                                }
                            });


                        }

                        $("#" + FormId + " #ProgressnoteHTML").find(".editable").css("display", "block")
                        $("#" + FormId + " #ProgressnoteHTML").css('list-style-type', 'none');

                        if (($("#" + FormId + " #ProgressnoteHTML .initialVisit").length <= 0 || $("#" + FormId + " #ProgressnoteHTML .initialVisit").html().trim() == '') && Clinical_Template_detail.NoteText.indexOf('initialVisit') <= -1) {
                            $("#" + FormId + " #ProgressnoteHTML").html('<ul class="initialVisit" id="ProgressNoteComponentList">' + Clinical_Template_detail.NoteText + '</ul>');
                        }

                        // Generating buttons for Components which are dropped on Progress note
                        Clinical_ProgressNote.CreateNotesComponent_Buttons(Clinical_ProgressNote.params.NotesId);

                        if (Clinical_ProgressNote.params.IsPhoneEncounter) {
                            if (Clinical_PhoneEncounter.CopyNotes == true)//for Change Ids In Case CopyNot
                            {
                                Clinical_ProgressNote.ComeFromCopyNote = true;
                                $("#pnlClinicalPhoneEncounter #CopyNoteText").html("");
                                Clinical_ProgressNote.DisableFields(false);
                                Clinical_ProgressNote.bindDateAndTimepicker();
                                Clinical_ProgressNote.ChangeIds(Clinical_PhoneEncounter.NewInsertTables, Clinical_PhoneEncounter.PrevNoteId, true);
                            }
                            else {
                                Clinical_ProgressNote.ComeFromCopyNote = false;
                                Clinical_ProgressNote.DisableFields(true);
                            }
                        }
                        else {
                            if (Clinical_Notes.CopyNotes == true)//for Change Ids In Case CopyNot
                            {
                                Clinical_ProgressNote.ComeFromCopyNote = true;
                                $("#pnlClinicalNotes #CopyNoteText").html("");
                                Clinical_ProgressNote.DisableFields(false);
                                Clinical_ProgressNote.bindDateAndTimepicker();
                                Clinical_ProgressNote.ChangeIds(Clinical_Notes.NewInsertTables, Clinical_Notes.PrevNoteId, true);
                            }
                            else {
                                Clinical_ProgressNote.ComeFromCopyNote = false;
                                Clinical_ProgressNote.DisableFields(true);
                            }
                        }
                    }

                    Clinical_ProgressNote.InitializeSorting();

                    ClinicalNotesModel.NotesText = NoteHTML;
                    if (templateName && templateName != "- Blank -") {
                        Clinical_ProgressNote.params.TemplateName = templateName;
                        Clinical_ProgressNote.InitializeProgressNoteComponent();
                    }
                    else {
                        Clinical_ProgressNote.params.TemplateName = "";
                        $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML").html('');
                        $.each(NoteSections, function (i, itm) {
                            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML").append(itm.SectionMarkup);
                        });
                        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                            Clinical_PhoneEncounter.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"), true);

                        }
                        else if (Clinical_Notes.CopyNotes != true) {
                            Clinical_Notes.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"));
                        }
                        Clinical_ProgressNote.InitializeSubjectiveDragable();
                        Clinical_ProgressNote.InitializeObjectiveDragable();
                        Clinical_ProgressNote.InitializeAssesmentDragable();
                        Clinical_ProgressNote.InitializePlanDragable();
                        Clinical_ProgressNote.InitializeMiscellaneousDragable();
                    }
                    Clinical_ProgressNote.Fill_NotesTemplateComponent(response, ClinicalNotesModel.PEDataTemptId, ClinicalNotesModel.PETemplateId, ClinicalNotesModel.ROSDataTemptId, ClinicalNotesModel.ROSDataTemptId).done(function () {
                        Clinical_ProgressNote.fixDivWithLiInNoteTemplate();
                        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                            Clinical_ProgressNote.saveComponentSOAPTextBulk(true, true).done(function () {
                                // Clinical_PhoneEncounter.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"), true);
                            });

                        }
                        else {
                            if (Clinical_Notes.CopyNotes != true) {
                                Clinical_ProgressNote.saveComponentSOAPTextBulk(true, true).done(function () {
                                    //  Clinical_Notes.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"));
                                });
                            }
                        }
                    });
                    //Clinical_ProgressNote.Load_NotesTemplateComponent(NoteHTML.html(), 'LoadTemplate');
                    $('#' + FormId).data('serialize', $('#' + FormId).serialize());
                    Clinical_ProgressNote.HideShowBillingInfo();
                }

            });
        });
    },

    // Getting Notes Information which is going to be edited in progress note
    Clinical_NotesFill: function (NotesId, FormId) {

        var compDefer = $.Deferred();
        if (NoteComponents.length > 0 && NoteSections.length > 0) {
            compDefer.resolve('ok');
        }
        else if (NoteComponents.length <= 0 && NoteSections.length <= 0) {
            var def = [];
            def.push(CacheManager.BindCodes('GetNoteSections', false));
            def.push(CacheManager.BindCodes('GetNoteComponents', false));
            $.when.apply($, def).done(function () {
                compDefer.resolve('ok');
            });
        }
        else if (NoteComponents.length < 0 && NoteSections.length > 0) {
            CacheManager.BindCodes('GetNoteComponents', false).done(function () {
                compDefer.resolve('ok');
            });
        }
        else if (NoteSections.length < 0 && NoteComponents.length > 0) {
            CacheManager.BindCodes('GetNoteSections', false).done(function () {
                compDefer.resolve('ok');
            });
        }
        else
            compDefer.resolve('ok');

        $.when(compDefer).done(function () {
            try {
                IsBackgroundLoaderShow = true;
                ShowHideLoaderOnScreen(true);
                Clinical_ProgressNote.LoadProgressNote(null, NotesId).done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {

                        var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

                        if (Clinical_Notes_detail.IsPhoneEncounter && Clinical_Notes_detail.TemplateName.toLowerCase() == "phone encounter tcm" && Clinical_Notes_detail.VisitReason.toLowerCase() == "transitional care management" && $("#PatientProfile #hfDischargeDate").val() != "") {
                            $('#' + Clinical_ProgressNote.params.PanelID + '  #frmClinicalProgressNote #btnBillingInfo').addClass('disableAll');
                        } else {
                            $('#' + Clinical_ProgressNote.params.PanelID + '  #frmClinicalProgressNote #btnBillingInfo').removeClass('disableAll');
                        }

                        if (response.HasDeleteRights == "No") {
                            $('#pnlClinicalProgressNote #btnNotesDelete').hide();
                        }
                        Clinical_ProgressNote.params["CurrentNotesFacilityDescription"] = Clinical_Notes_detail.FacilityDescription;

                        Clinical_ProgressNote.params["CurrentNotesProviderId"] = Clinical_Notes_detail.ProviderId;
                        Clinical_ProgressNote.params["CurrentNotesProviderText"] = Clinical_Notes_detail.ProviderFullName;
                        Clinical_ProgressNote.params["HxtabOrder"] = Clinical_Notes_detail.HxtabOrder;
                        Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"] = Clinical_Notes_detail.FacilityId;
                        Clinical_ProgressNote.params["NotesFacilityName"] = Clinical_Notes_detail.FacilityName;
                        Clinical_ProgressNote.params["VisitDateForFollowUp"] = Clinical_Notes_detail.VisitDate;
                        Clinical_ProgressNote.params["PatientTypeId"] = Clinical_Notes_detail.PatientTypeId;
                        Clinical_ProgressNote.params["TemplateName"] = Clinical_Notes_detail.TemplateName;
                        Clinical_ProgressNote.params["FilePath"] = Clinical_Notes_detail.FilePath;
                        Clinical_ProgressNote.params["IsBodyPart"] = Clinical_Notes_detail.IsBodyPart;

                        if (Clinical_Notes_detail.IsAnyDocumentAttached && Clinical_Notes_detail.IsAnyDocumentAttached.toLowerCase() == "true")
                            $("#" + Clinical_Notes.params.PanelID + " #noteatch_i").addClass("fa-paperclip");
                        else
                            $("#" + Clinical_Notes.params.PanelID + " #noteatch_i").removeClass("fa-paperclip");

                        if (Clinical_Notes_detail.IsAnyDocumentAttached != null) {
                            Clinical_ProgressNote.IsAnyDocumentAttached = Clinical_Notes_detail.IsAnyDocumentAttached.toLowerCase() == "true" ? true : false;
                        }


                        if (Clinical_ProgressNote.params.IsPhoneEncounter) {
                            if (Clinical_Notes_detail.FilePath != '') {
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkPlayfile').show();
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkPlayfile').text(Clinical_ProgressNote.params.FilePath);
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #filetag').hide();
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkAudioupload').hide();
                            }
                            else {
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkAudioupload').show();
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #filetag').show();
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkPlayfile').hide();
                                $('#' + Clinical_ProgressNote.params.PanelID + '  #AudioPlayer').hide();
                            }
                        }
                        else {
                            $('#' + Clinical_ProgressNote.params.PanelID + '  #filetag').hide();
                            $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkAudioupload').hide();
                            $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkPlayfile').hide();
                            $('#' + Clinical_ProgressNote.params.PanelID + '  #AudioPlayer').hide();

                        }

                        var $panelid = $('#' + Clinical_Notes.params.PanelID);
                        $panelid.find('.NoteTemplate > select').attr('ddlist', 'GetNoteTemplate');

                        var NoteType = Clinical_Notes_detail.NoteType;
                        NoteType = NoteType ? NoteType : 1;

                        var data = "IsActive=&ID=" + NoteType + "&ID2=" + Clinical_ProgressNote.params.CurrentNotesProviderId;

                        $panelid.find('.NoteTemplate').loadDropDowns(true, data).done(function () {
                            setTimeout(function () {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + " #NoteTemplate").val(Clinical_Notes_detail.NoteTemplate);
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + " #NoteTemplate").val() == "")
                                    $panelid.find('.OrderSet').addClass('disableAll');
                                else
                                    $panelid.find('.OrderSet').removeClass('disableAll');
                            }, 5)

                        });


                        Clinical_ProgressNote.params["IsNonBilable"] = Clinical_Notes_detail.IsNonBilable;
                        Clinical_ProgressNote.params["VisitId"] = Clinical_Notes_detail.VisitId;

                        $('#' + Clinical_ProgressNote.params["PanelID"] + " #hfbMedReconciled").val(Clinical_Notes_detail.bMedReconciled);
                        $('#' + Clinical_ProgressNote.params["PanelID"] + " #hfMedReconciledId").val(Clinical_Notes_detail.MedReconciledId);

                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #lblNotesReason")) {

                            $('#' + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #lblNotesReason").empty();
                            $('#' + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #lblNotesReason").append(Clinical_Notes_detail.VisitReason);
                        }
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val(Clinical_Notes_detail.NoteStatus);
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(Clinical_Notes_detail.BillingInfoId);
                        $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSave ,#btnSign , #btnReview,#btnNotesDelete,#btnBillingInfo,#btnNoteViewCharges,#btnCreateLetter').addClass('disabled');
                        if (Clinical_Notes_detail.NoteStatus == "Signed") {
                            if ($('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val() == 'Signed' && $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val() > 0 && Clinical_Notes_detail.IsNonBilable.toString().toLowerCase() != "true") {
                                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                                $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                                $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnBillingInfo').removeClass('disabled');
                            }
                            else {
                                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                            }

                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ddlVisitType').prop('disabled', true);
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('#ChkBox_IsNonBilable').parent().addClass('disableAll');
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('div#InitialOfficeVisit,div#sidebar-wrapper').addClass('disableAll');
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnPrint ,#btnAssign , #btnSend,#btnSyndromicSurveillance,#btnNoteCoSign,#btnNoteAmendment').removeClass('disabled');
                            $('#' + Clinical_ProgressNote.params["PanelID"] + " #btnNoteAttachment").addClass('disableAll');
                            $('#' + Clinical_ProgressNote.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "none");
                            $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "none");
                        }
                        else {
                            if (Clinical_Notes_detail.NoteStatus == 'Signed' && Clinical_Notes_detail.BillingInfoId > 0 && Clinical_Notes_detail.IsNonBilable.toString().toLowerCase() != "true") {
                                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                                $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                            }
                            else {
                                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                            }
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSend,#btnReview,#btnCreate_eSupperbill,#btnNoteCoSign,#btnNoteAmendment').addClass('disabled');
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('div#InitialOfficeVisit,div#sidebar-wrapper').removeClass('disableAll');
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('#ChkBox_IsNonBilable').parent().removeClass('disableAll');
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSave,#btnSign,#btnPrint,#btnAssign,#btnBillingInfo,#btnCreateLetter,#btnNotesDelete,#btnSyndromicSurveillance').removeClass('disabled');
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ddlVisitType').prop('disabled', false);
                            $('#' + Clinical_ProgressNote.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "");
                            $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "");
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' .splitterBody').hasClass('disableAll')) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' .splitterBody').removeClass('disableAll');
                            }
                        }

                        // MU3-32

                        $.when(Clinical_ProgressNote.GetRooms(Clinical_Notes_detail.FacilityId)).done(function () {

                            var self = $('#' + FormId);
                            //binding values to form controls
                            utility.bindMyJSONByName(true, Clinical_Notes_detail, false, self).done(function () {
                                Clinical_ProgressNote.bindOrdersetList();

                            });

                            $Ctrl = $("#" + Clinical_ProgressNote.params.PanelID + " #txtFacility");
                            $hfCtrl = $("#" + Clinical_ProgressNote.params.PanelID + " #hfFacility");
                            //Facility
                            utility.SetAutoCompleteSource($Ctrl, $hfCtrl);

                            if (!(Clinical_ProgressNote.params["ParentCntrlLoadid"] == 'Schedular' && Clinical_ProgressNote.params["mode"] == 'Add')) {

                                $('#' + Clinical_ProgressNote.params["PanelID"] + " #NoteType").append(
                                                   $('<option></option>').val(Clinical_Notes_detail.NoteType).html(Clinical_Notes_detail.TemplateTypeName));

                                $('#' + Clinical_ProgressNote.params["PanelID"] + " #NoteType").val(Clinical_Notes_detail.NoteType);

                                $('#' + Clinical_ProgressNote.params["PanelID"] + " #NoteTemplate").val(Clinical_Notes_detail.NoteTemplate);

                                $('#' + Clinical_ProgressNote.params["PanelID"] + " #VisitReason").append(
             $('<option></option>').val(Clinical_Notes_detail.VisitReasonId).html(Clinical_Notes_detail.VisitReason));

                                $('#' + Clinical_ProgressNote.params["PanelID"] + " #VisitReason").val(Clinical_Notes_detail.VisitReasonId);
                            }

                            if (Clinical_Notes_detail.NoteStatus == "Draft" || Clinical_Notes_detail.NoteStatus == null || Clinical_Notes_detail.NoteStatus == "") {

                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote div:not(".panel-actions")').removeClass('disableAllUltra');
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #sidebar-wrapper').removeClass('disableAllUltra');
                            }
                            else if (Clinical_Notes_detail.NoteStatus == "Signed") {

                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote div:not(".panel-actions")').removeClass('disableAllUltra');
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #sidebar-wrapper').addClass('disableAllUltra');
                            }
                            else {

                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote div:not(".panel-actions")').addClass('disableAllUltra');
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #sidebar-wrapper').addClass('disableAllUltra');
                            }

                            // Show/Hide of Labels (Copy Previous Note, Ref Provider, Link Appointment)
                            //if (Clinical_Notes_detail.IsCopayPreviousNote == true)
                            //    $("#" + FormId + " #chkCopayPreviousNote").attr("checked", true);
                            //else
                            //    $("#" + FormId + " #chkCopayPreviousNote").attr("checked", false);

                            $("#" + FormId + " #chkCopayPreviousNote").attr("checked", Clinical_Notes_detail.IsCopayPreviousNote);

                            Clinical_Notes.UnLinkPreviousNotePatient($("#" + Clinical_ProgressNote.params.PanelID + " #chkCopayPreviousNote"), Clinical_ProgressNote.params.PanelID);

                            $("#" + PanelID + " #txtCopayPreviousNote").attr("disabled", "disabled");
                            $("#" + PanelID + " #chkCopayPreviousNote").attr("disabled", "disabled");
                            $("#" + PanelID + " #lblPreviousNote").show();
                            $("#" + PanelID + " #lnkPreviousNote").hide();

                            if (Clinical_ProgressNote.params.IsPhoneEncounter) {

                                var Duration = "";
                                if (Clinical_Notes_detail.Duration && Clinical_Notes_detail.Duration != "")
                                    Duration = Clinical_Notes_detail.Duration.split(':');

                                var self = $("#" + Clinical_ProgressNote.params["PanelID"] + " #DivDuration");

                                self.find("#txtTaskHours").val(Duration[0]);
                                self.find("#txtTaskMinutes").val(Duration[1]);
                                self.find("#txtTaskSeconds").val(Duration[2]);

                                /*// Commented for the changeSet:20920(remove validation from CCM_Hub duration)
                                if (Clinical_ProgressNote.params.CCMDuration)
                                    $("#pnlClinicalProgressNote #EitherFromCCM").removeClass('hidden');
                                else
                                    $("#pnlClinicalProgressNote #EitherFromCCM").addClass('hidden');*/

                                $("#pnlClinicalProgressNote #txtTaskHours").attr('disabled', false);
                                $("#pnlClinicalProgressNote #txtTaskMinutes").attr('disabled', false);
                                $("#pnlClinicalProgressNote #txtTaskSeconds").attr('disabled', false);
                            }

                            if (Clinical_Notes_detail.IsLinkedAppointment == true) {
                                $("#" + FormId + " #ChkBox_LinkedAppointment").attr("checked", true);
                                Clinical_ProgressNote.AppointmentID = Clinical_Notes_detail.AppointmentID;
                                $('#' + Clinical_Notes.params.PanelID + ' #hfAppointmentId').val(Clinical_Notes_detail.AppointmentID);
                            }
                            else {
                                Clinical_ProgressNote.AppointmentID = 0;
                                $("#" + FormId + " #ChkBox_LinkedAppointment").attr("checked", false);
                                $('#' + Clinical_Notes.params.PanelID + ' #txtLinkedAppointment').val('No Appointment Selected');
                            }


                            //------- show/hide link of link appointment

                            if (!(Clinical_ProgressNote.params["ParentCntrlLoadid"] == 'Schedular' && Clinical_ProgressNote.params["mode"] == 'Add')) {
                                Clinical_Notes.UnlinkAppointment($("#" + Clinical_ProgressNote.params.PanelID + " #ChkBox_LinkedAppointment"), Clinical_ProgressNote.params.PanelID, false, Clinical_Notes_detail.IsNonBilable);
                            }

                            var self = $('#' + Clinical_ProgressNote.params.PanelID);

                            var DefVisiType = $.Deferred();
                            var DefVisiTypeInner = $.Deferred();
                            Clinical_ProgressNote.params.IsVisiTypeLoaded = DefVisiType;
                            if (globalAppdata["isTransitonCancerRegistries"] && globalAppdata["isTransitonCancerRegistries"].toLowerCase() == "false")
                                Clinical_ProgressNote.LoadVisitTypeddlWO_CancerRegistries().done(function () {
                                    DefVisiTypeInner.resolve()
                                });
                            else if ($('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTypeDiv #ddlVisitType option').length > 0
                                && $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTypeDiv #ddlVisitType').attr("ddlist") == "GetPatientVisitType") {
                                DefVisiTypeInner.resolve()
                            }
                            else {
                                $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTypeDiv #ddlVisitType').attr('ddlist', "GetPatientVisitType");
                                var self = $('#' + Clinical_ProgressNote.params.PanelID);

                                self.find('#VisitTypeDiv').loadDropDowns(true).done(function () { DefVisiTypeInner.resolve() });

                            }

                            $.when(DefVisiTypeInner).done(function () {
                                if (Clinical_ProgressNote.params.PatientTypeId != "0") {
                                    Clinical_Notes.showDropdownOptions(Clinical_ProgressNote.params.PatientTypeId).done(function () {
                                        if (Clinical_Notes_detail.VisitTypeId != 'undefined' && Clinical_Notes_detail.VisitTypeId != undefined && Clinical_Notes_detail.VisitTypeId != null) {
                                            $("#" + FormId + " #ddlVisitType").val(Clinical_Notes_detail.VisitTypeId);
                                        }
                                    });

                                }
                                else {
                                    if (Clinical_Notes_detail.VisitTypeId != 'undefined' && Clinical_Notes_detail.VisitTypeId != undefined && Clinical_Notes_detail.VisitTypeId != null) {
                                        $("#" + FormId + " #ddlVisitType").val(Clinical_Notes_detail.VisitTypeId);
                                    }
                                }
                                DefVisiType.resolve('ok');
                            });


                            var NoteTemplateVal = $("#" + Clinical_ProgressNote.params.PanelID + " #NoteTemplate").val();
                            var data = "IsActive=1&StrID=" + Clinical_ProgressNote.params.CurrentNotesProviderId;
                            $panelid.find('.OrderSet > select').attr('ddlist', 'GetOrderSetTemplate');

                            $panelid.find('.OrderSet').loadDropDowns(true, data).done(function () {

                                if (Clinical_Notes_detail.OrderSetName) {
                                    $panelid.find(".OrderSet option").prop('selected', false).filter(function () {
                                        return $(this).text() == Clinical_Notes_detail.OrderSetName;
                                    }).prop('selected', true);

                                }
                            });


                            if (Clinical_Notes_detail.OrderSetName && Clinical_Notes_detail.OrderSetId) {

                                Clinical_ProgressNote.DefaultOrderSetName = Clinical_Notes_detail.OrderSetName;
                                Clinical_ProgressNote.DefaultOrderSetId = Clinical_Notes_detail.OrderSetId;
                                Clinical_ProgressNote.IsDefaultOrderSet = "1";
                            }
                            if (Clinical_Notes_detail.IsNonBilable.toString().toLowerCase() != "true") {

                                if (Clinical_Notes_detail.AppointmentID > 0) {
                                    Clinical_ProgressNote.FillAppointment(Clinical_Notes_detail.AppointmentID).done(function (response) {

                                        if (response.status != false) {

                                            var appointment_detail = JSON.parse(response.AppointmentFill_JSON);
                                            if (appointment_detail.chkNonBilable.toLowerCase() == "true") {
                                                $("#" + PanelID + " #ChkBox_IsNonBilable").prop('checked', Clinical_Notes_detail.IsNonBilable);
                                            }
                                            else {
                                                $("#" + PanelID + " #ChkBox_IsNonBilable").prop('checked', false);
                                            }
                                        } else {
                                            utility.DisplayMessages(response.Message, 3);
                                            BackgroundLoaderShow(false);
                                        }
                                    });
                                }
                            }

                            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ChkBox_IsNonBilable").prop('checked', Clinical_Notes_detail.IsNonBilable);

                            if (Clinical_Notes_detail.IsLinkedAppointment == true) {
                                $("#" + FormId + " #ChkBox_LinkedAppointment").prop("disabled", true);
                            }

                            if (Clinical_Notes_detail.RefProviderId != "" && Clinical_Notes_detail.RefProviderId != null) {

                                $("#pnlClinicalProgressNote #hfRefProvider").val(Clinical_Notes_detail.RefProviderId);

                                if ($("#pnlClinicalProgressNote #lnkRefProviderEdit").css("display") == "none") {
                                    $("#pnlClinicalProgressNote #lnkRefProviderEdit").css("display", "inline");
                                    $("#pnlClinicalProgressNote #lblRefProvider").css("display", "none");
                                }

                                $Ctrl = $("#" + Clinical_ProgressNote.params.PanelID + " #txtRefProvider");
                                $hfCtrl = $("#" + Clinical_ProgressNote.params.PanelID + " #hfRefProvider");
                                //RefProvider
                                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());

                            } else {

                                $("#pnlClinicalProgressNote #hfRefProvider").val("");

                                if ($("#pnlClinicalProgressNote #lnkRefProviderEdit").css("display") == "inline") {
                                    $("#pnlClinicalProgressNote #lnkRefProviderEdit").css("display", "none");
                                    $("#pnlClinicalProgressNote #lblRefProvider").css("display", "inline");
                                }
                            }
                            $("#" + FormId + " #ProgressnoteHTML").html('');
                            var defaultSOAPText = Clinical_Notes_detail.NoteText;
                            //$.each(NoteSections, function (i, itm) {
                            //    $("#" + FormId + " #ProgressnoteHTML").append(itm.SectionMarkup);
                            //});
                            // End Show/Hide of Labels (Copy Previous Note, Ref Provider, Link Appointment)
                            if (Clinical_Notes_detail.NoteText == null || Clinical_Notes_detail.NoteText == '') {
                                $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").html('');
                                if (Clinical_ProgressNote.params.TemplateName) {
                                    $.each(NoteSections, function (i, itm) {
                                        if (itm.value == "Progress")
                                            $("#" + FormId + " #ProgressnoteHTML").append(itm.SectionMarkup);
                                    });
                                    $("#" + FormId + " #ProgressnoteHTML #ProgressNoteComponentList").html('');
                                }
                                else {
                                    $.each(NoteSections, function (i, itm) {
                                        if (Clinical_ProgressNote.params.IsPhoneEncounter == true && itm.value != "PhoneEncounterData") {
                                            $("#" + FormId + " #ProgressnoteHTML").append(itm.SectionMarkup);
                                        }
                                        else if (itm.value != "PhoneEncounterData") {
                                            $("#" + FormId + " #ProgressnoteHTML").append(itm.SectionMarkup);
                                        }
                                    });
                                }

                                if (response.NoteComponentListCount > 0) {
                                    $.when(Clinical_ProgressNote.params.IsVisiTypeLoaded).then(function () {
                                        var temp = response.NoteComponentListFill_JSON;

                                        //EMR-6630 To remove order set component from print preview note soaptext, when user signs the progress note
                                        if (Clinical_Notes_detail.NoteStatus == 'Signed')
                                            temp = $.grep(response.NoteComponentListFill_JSON, function (item_) { return (item_.ComponentName != 'Order Sets') });

                                        Clinical_ProgressNote.BindNoteComponents(temp, Clinical_Notes_detail, FormId).done(function () {
                                            Clinical_ProgressNote.EnableDisableCancerReportButton();
                                            Clinical_ProgressNote.CreateNotesComponent_Buttons(NotesId);
                                        });

                                    });


                                    if ($("#" + FormId + " #ProgressnoteHTML #PhoneEncounterData").length > 0) {
                                        var encounterMarkup = $("#" + FormId + " #ProgressnoteHTML #PhoneEncounterData")[0].outerHTML;
                                        $("#" + FormId + " #ProgressnoteHTML #PhoneEncounterData").remove();
                                        $("#" + FormId + " #ProgressnoteHTML").prepend(encounterMarkup);
                                    }
                                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                                    $("#ProgressNoteComponentList input.TagInserted").each(function (index) {
                                        if ($(this).hasClass('TagInserted')) {
                                            var tText = $(this).val();
                                            if (tText.indexOf("Custom Form") > -1)
                                                $(this).remove();
                                        }
                                    });
                                }
                                else {
                                    Clinical_ProgressNote.EnableDisableCancerReportButton();

                                    if (($("#" + FormId + " #ProgressnoteHTML .initialVisit").length <= 0 || $("#" + FormId + " #ProgressnoteHTML .initialVisit").html().trim() == '') && Clinical_Notes_detail.NoteText.indexOf('initialVisit') <= -1) {
                                        $("#" + FormId + " #ProgressnoteHTML").html('<ul class="initialVisit" id="ProgressNoteComponentList">' + Clinical_Notes_detail.NoteText + '</ul>');
                                    }
                                    if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                                        var dateOfDischarge = '';
                                        if (Clinical_ProgressNote.params["IsDateOfDischarge"] == true) {
                                            dateOfDischarge = '<li><span  style="color:#0088cc;">Date of Discharge:</span><span id="DateOfDischarge">' + ' '
                                       + $('#PatientProfile #hfDischargeDate').val() + '</span></li>';
                                        }
                                        if (Clinical_ProgressNote.params.TaskComments == 'undefined' || Clinical_ProgressNote.params.TaskComments == undefined || Clinical_ProgressNote.params.TaskComments == null)
                                            Clinical_ProgressNote.params.TaskComments = "";

                                        if (Clinical_ProgressNote.params.TaskVisitReason == 'undefined' || Clinical_ProgressNote.params.TaskVisitReason == undefined || Clinical_ProgressNote.params.TaskVisitReason == null)
                                            Clinical_ProgressNote.params.TaskVisitReason = "";
                                        $("#" + FormId + " #ProgressnoteHTML").prepend('<ul class="initialVisit ui-sortable PhoneEncounterDataComponent"  id="PhoneEncounterData" style="min-height: 3px; list-unstyled" NoteComponentId="NCDummyId"> <section> <li><span style="color:#0088cc;">Call Time:</span><span id="CallTime">' + ' '
                                        + Clinical_Notes_detail.VisitTime + '</span></li><li><span style="color:#0088cc;">Note date:</span><span id="CallTime">' + ' '
                                        + Clinical_Notes_detail.VisitDate + '</span></li><li><span  style="color:#0088cc;">Caller:</span><span id="Caller">' + ' '
                                        + Clinical_Notes_detail.Caller + '</span></li><li><span  style="color:#0088cc;">Receiver:</span><span id="Receiver">' + ' '
                                        + Clinical_Notes_detail.Receiver + '</span></li><li><span  style="color:#0088cc;">Reason:</span><span id="Reason">' + ' '
                                        + Clinical_Notes_detail.VisitReason + '</span></li><section ><li><li><span  style="color:#0088cc;">Total duration:</span><span id="totalDuration">' + ' '
                                        + Clinical_ProgressNote.params.CalculatedDuration + '</span></li>' + dateOfDischarge + '<br><br><span style="color:#0088cc;">Call Summary:</span><br><span id="CallSummary">' + ' '
                                        + Clinical_ProgressNote.params.TaskComments + '</span></li><li class="sopTextEditable defaultli ui-sortable-handle CallSummaryTextArea"  style="min-height: 15px !important" NoteComponentId="NCDummyId"></li></section></section></ul>');

                                        $('#pnlClinicalProgressNote #Caller').text(' ' + $('#pnlClinicalProgressNote #txtCaller').val());
                                        $('#pnlClinicalProgressNote #Receiver').text(' ' + $('#pnlClinicalProgressNote #txtReceiver').val());
                                        Clinical_ProgressNote.StopTaskTime();
                                    }
                                }
                                ShowHideLoaderOnScreen(false);
                            }
                            else {
                                $("#" + FormId + " #ProgressnoteHTML").html(Clinical_Notes_detail.NoteText);
                                $("#" + FormId + " #ProgressnoteHTML").find(".editable").css("display", "block")
                                $("#" + FormId + " #ProgressnoteHTML").css('list-style-type', 'none');

                                if (($("#" + FormId + " #ProgressnoteHTML .initialVisit").length <= 0 || $("#" + FormId + " #ProgressnoteHTML .initialVisit").html().trim() == '') && Clinical_Notes_detail.NoteText.indexOf('initialVisit') <= -1) {
                                    $("#" + FormId + " #ProgressnoteHTML").html('<ul class="initialVisit" id="ProgressNoteComponentList">' + Clinical_Notes_detail.NoteText + '</ul>');
                                }
                                if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                                    var dateOfDischarge = '';
                                    if (Clinical_ProgressNote.params["IsDateOfDischarge"] == true) {
                                        dateOfDischarge = '<li><span  style="color:#0088cc;">Date of Discharge:</span><span id="DateOfDischarge">' + ' '
                                        + $('#PatientProfile #hfDischargeDate').val() + '</span></li>';
                                    }

                                    if (Clinical_ProgressNote.params.TaskComments == 'undefined' || Clinical_ProgressNote.params.TaskComments == undefined || Clinical_ProgressNote.params.TaskComments == null)
                                        Clinical_ProgressNote.params.TaskComments = "";

                                    if (Clinical_ProgressNote.params.TaskVisitReason == 'undefined' || Clinical_ProgressNote.params.TaskVisitReason == undefined || Clinical_ProgressNote.params.TaskVisitReason == null)
                                        Clinical_ProgressNote.params.TaskVisitReason = "";
                                    $("#" + FormId + " #ProgressnoteHTML").prepend('<ul class="initialVisit ui-sortable PhoneEncounterDataComponent"  id="PhoneEncounterData" style="min-height: 3px; list-unstyled" NoteComponentId="NCDummyId"> <section> <li><span style="color:#0088cc;">Call Time:</span><span id="CallTime">' + ' '
                                    + Clinical_Notes_detail.VisitTime + '</span></li><li><span style="color:#0088cc;">Note date:</span><span id="CallTime">' + ' '
                                    + Clinical_Notes_detail.VisitDate + '</span></li><li><span  style="color:#0088cc;">Caller:</span><span id="Caller">' + ' '
                                    + Clinical_Notes_detail.Caller + '</span></li><li><span  style="color:#0088cc;">Receiver:</span><span id="Receiver">' + ' '
                                    + Clinical_Notes_detail.Receiver + '</span></li><li><span  style="color:#0088cc;">Reason:</span><span id="Reason">' + ' '
                                    + Clinical_Notes_detail.VisitReason + '</span></li><section ><li><li><span  style="color:#0088cc;">Total duration:</span><span id="totalDuration">' + ' '
                                    + Clinical_ProgressNote.params.CalculatedDuration + '</span></li>' + dateOfDischarge + '<br><br><span style="color:#0088cc;">Call Summary:</span><br><span id="CallSummary">' + ' '
                                    + Clinical_ProgressNote.params.TaskComments + '</span></li><li class="sopTextEditable defaultli ui-sortable-handle CallSummaryTextArea"  style="min-height: 15px !important" NoteComponentId="NCDummyId"></li></section></section></ul>');

                                    $('#pnlClinicalProgressNote #Caller').text(' ' + $('#pnlClinicalProgressNote #txtCaller').val());
                                    $('#pnlClinicalProgressNote #Receiver').text(' ' + $('#pnlClinicalProgressNote #txtReceiver').val());
                                    Clinical_ProgressNote.StopTaskTime();
                                }
                                /***************************************************
                                azhar change for Template Componenets
                                ****************************************************/
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Clinical"]').length > 0 || $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Custom"]').length > 0 || $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[data-title^="{FT}"]').length > 0) {
                                    Clinical_ProgressNote.Fill_NotesTemplateComponent(response, Clinical_Notes_detail.PEDataTemptId, Clinical_Notes_detail.PETemplateId, Clinical_Notes_detail.ROSDataTemptId, Clinical_Notes_detail.ROSTemplateId, Clinical_Notes_detail.HPITemplateId).done(function () {
                                        ShowHideLoaderOnScreen(false);
                                        //Clinical_ProgressNote.CheckNoteComponentsData(NotesId, false).done(function (res) {
                                        //res = JSON.parse(res);
                                        // if (res.status == true && res.IsNoteComponentsAttached == true) {

                                        if ($('#pnlClinicalProgressNote #ProgressnoteHTML .CustomFormsComponent').length > 0) {
                                            $('#pnlClinicalProgressNote #ProgressnoteHTML .FreeTextComponent').prev().after($('#pnlClinicalProgressNote #ProgressnoteHTML .CustomFormsComponent'));
                                        }
                                        Clinical_ProgressNote.fixDivWithLiInNoteTemplate();
                                        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                                            Clinical_ProgressNote.saveComponentSOAPTextBulk(true, true).done(function (result) {
                                                if (result == true) {
                                                    // Clinical_PhoneEncounter.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"), true);
                                                }
                                                else {
                                                    Clinical_ProgressNote.CheckNoteComponentsData(NotesId, true).done(function (res) {
                                                        res = JSON.parse(res);
                                                        if (res.status) {
                                                            Clinical_ProgressNote.UnLoad();
                                                        }
                                                    });
                                                }
                                            });

                                        }
                                        else {
                                            if (Clinical_Notes.CopyNotes != true) {
                                                Clinical_ProgressNote.saveComponentSOAPTextBulk(true, true).done(function (result) {
                                                    if (result == true) {
                                                        //Clinical_Notes.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"));
                                                        Clinical_ProgressNote.EnableDisableCancerReportButton();
                                                    }
                                                    else {
                                                        Clinical_ProgressNote.CheckNoteComponentsData(NotesId, true).done(function (res) {
                                                            res = JSON.parse(res);
                                                            if (res.status) {
                                                                Clinical_ProgressNote.UnLoad();
                                                            }
                                                        });
                                                    }
                                                });
                                            }
                                        }
                                        //}
                                        //else {
                                        //utility.DisplayMessages(res.Message, 3);
                                        //Clinical_ProgressNote.UnLoad();
                                        //}
                                        //});

                                    });
                                }
                                else {
                                    ShowHideLoaderOnScreen(false);
                                    if (response.NoteComponentListCount > 0) {
                                    }
                                    else {
                                        //if (defaultSOAPText && defaultSOAPText.toLowerCase() != $("#" + FormId + " #ProgressnoteHTML").html().toLowerCase())
                                        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                                            Clinical_ProgressNote.FixOldNotesForSOAPComponents(true).done(function () {
                                                Clinical_PhoneEncounter.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"), true);
                                            });

                                        }
                                        else {
                                            if (Clinical_Notes.CopyNotes != true) {
                                                Clinical_ProgressNote.FixOldNotesForSOAPComponents(true).done(function () {
                                                    Clinical_Notes.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"));
                                                });
                                            }
                                        }
                                    }
                                }
                                //Fix ProblemOrder in OLD SOAP Text
                                Clinical_ProgressNote.fixProblemOrderForOldSOAP();
                                // Generating buttons for Components which are dropped on Progress note
                                Clinical_ProgressNote.CreateNotesComponent_Buttons(NotesId);
                                $("#ProgressNoteComponentList input.TagInserted").each(function (index) {
                                    if ($(this).hasClass('TagInserted')) {
                                        var tText = $(this).val();
                                        if (tText.indexOf("Custom Form") > -1)
                                            $(this).remove();
                                    }
                                });

                                if (Clinical_ProgressNote.params.IsPhoneEncounter) {
                                    if (Clinical_PhoneEncounter.CopyNotes == true)//for Change Ids In Case CopyNot
                                    {
                                        Clinical_ProgressNote.ComeFromCopyNote = true;
                                        $("#pnlClinicalPhoneEncounter #CopyNoteText").html("");
                                        Clinical_ProgressNote.DisableFields(false);
                                        Clinical_ProgressNote.bindDateAndTimepicker();
                                        //Clinical_ProgressNote.ChangeIds(Clinical_PhoneEncounter.NewInsertTables, Clinical_PhoneEncounter.PrevNoteId);
                                    }
                                    else {
                                        Clinical_ProgressNote.ComeFromCopyNote = false;
                                        Clinical_ProgressNote.DisableFields(true);
                                    }
                                }
                                else {
                                    if (Clinical_Notes.CopyNotes == true)//for Change Ids In Case CopyNot
                                    {
                                        Clinical_ProgressNote.ComeFromCopyNote = true;
                                        $("#pnlClinicalNotes #CopyNoteText").html("");
                                        Clinical_ProgressNote.DisableFields(false);
                                        Clinical_ProgressNote.bindDateAndTimepicker();
                                        //Clinical_ProgressNote.ChangeIds(Clinical_Notes.NewInsertTables, Clinical_Notes.PrevNoteId);
                                    }
                                    else {
                                        Clinical_ProgressNote.ComeFromCopyNote = false;
                                        Clinical_ProgressNote.DisableFields(true);
                                    }
                                }
                            }

                            //Start//27/03/2017//Muhammad Zain ul abdin//IMP-538
                            Clinical_ProgressNote.ChangeNoteFontSize();
                            //End//27/03/2017//Muhammad Zain ul abdin//IMP-538

                            // Getting Progress Note Right Side Bar List
                            Clinical_ProgressNote.Fill_NotesComponent();
                            //edhr
                            Clinical_ProgressNote.bindCCMProcedureProblems();
                            if (Clinical_ProgressNote.params.IsPhoneEncounter == true && Clinical_ProgressNote.params.IsFromCreateNote) {
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Clinical"]').length == 0 && $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Custom"]').length == 0) {
                                    Clinical_ProgressNote.saveComponentSOAPTextBulk(true, false);
                                }
                            }
                            //Binding Page Ready Function
                            Clinical_ProgressNote.InitializeSorting();

                            if (Clinical_ProgressNote.params.IsFromCreateNote != null && Clinical_ProgressNote.params.IsFromCreateNote == true) {
                                //If Vitals Component which is dropeed in Progress note has no Vital attached, than it will call for Latest Vital for this patient
                                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_vitals').length > 0 && $('#pnlClinicalProgressNote #ProgressnoteHTML clinical_vitals').parent().parent().find('section') == 0) {
                                    Clinical_Vitals.GetLatestVitalByPatientId();
                                }
                                params['IsFromCreateNote'] = null;
                            }
                            var total_ = $('#' + Clinical_ProgressNote.params["PanelID"]).find("#coSignedByProvider li[id='coSignedBy']").length;

                            if (total_ > 0) {

                                $('#' + Clinical_ProgressNote.params["PanelID"]).find("#coSignedByProvider li[id='coSignedBy']").each(function () {
                                    var CoSignedProviderId = $(this).attr("cosignedproviderid");
                                    var $coSignedBy = $(this);
                                    if (CoSignedProviderId) {
                                        providerDetail.FillProvider(CoSignedProviderId, 0).done(function (response) {
                                            if (response.status != false) {
                                                if ($('div.CoSignComponent').length > 0) {
                                                    $('div.CoSignComponent')[0].remove();
                                                }

                                                var provider_detailCoS = JSON.parse(response.ProviderFill_JSON);
                                                var eSignature_image_Src = provider_detailCoS.imgeSignature;
                                                var Is_eSignatured = provider_detailCoS.chkIs_eSignatured && provider_detailCoS.chkIs_eSignatured != "" ? JSON.parse(provider_detailCoS.chkIs_eSignatured.toLowerCase()) : false;

                                                if (eSignature_image_Src != "" && Is_eSignatured) {
                                                    var isBrowserIE = providerDetail.GetIEVersion() > 0;
                                                    if (isBrowserIE) {
                                                        eSignature_image_Src = eSignature_image_Src.replace("System.Byte[]", "image/gif");
                                                    }

                                                    var imgeCoSignatureHtml = '<li><div style="max-height:350px; overflow-y:auto;margin-top:15px;" >' +
                                                                                '<img id="img_eSignatureCoS_ProgressNotes" src="' + eSignature_image_Src + '" ' +
                                                                                     'alt="" style="height: 125px; width: 315px;border:none;" ' +
                                                                                     'class="img-responsive img-center mt-lg img-thumbnail"/>'
                                                    '</div></li>';
                                                    $(imgeCoSignatureHtml).insertBefore($($coSignedBy));

                                                }
                                            }
                                        });
                                    }
                                });
                            }

                            // fixation of EMR-2500
                            var date_ = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #dtpVisitDate').val();
                            if (date_) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #dtpVisitDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date(date_)));
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #VisitTime').blur();
                            }

                            $('#' + FormId).data('serialize', $('#' + FormId).serialize());
                            Clinical_ProgressNote.HideShowBillingInfo();
                            Clinical_ProgressNote.hoverFunction();
                        });

                        //findInDiv.hide(true);


                    }
                    else {
                        ShowHideLoaderOnScreen(false);
                        if (response.UnDo) {

                            utility.DisplayMessages(response.Message, 3);

                            Clinical_ProgressNote.CheckNoteComponentsData(NotesId, true).done(function (res) {
                                res = JSON.parse(res);
                                if (res.status) {
                                    Clinical_ProgressNote.UnLoad();
                                }
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                    // Renaming the Radiology Order Component with Stupidiest but THE SAFEST Solution
                    setTimeout(function () {
                        if ($('#InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').find('a').length > 0) {
                            $($('#InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').find('a')[0]).attr("title", "Diagnostic Imaging Order");
                            $($('#InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').find('a')[0]).text("Diagnostic Imaging Order");
                        }

                        if ($('#InitialOfficeVisit #ProgressnoteHTML clinical_radiologyresults').find('a').length > 0) {
                            $($('#InitialOfficeVisit #ProgressnoteHTML clinical_radiologyresults').find('a')[0]).attr("title", "Diagnostic Imaging Results");
                            $($('#InitialOfficeVisit #ProgressnoteHTML clinical_radiologyresults').find('a')[0]).text("Diagnostic Imaging Results");
                        }

                        //buttons
                        if ($('#InitialOfficeVisit button[title="Radiology Order"]').length > 0) {
                            $('#InitialOfficeVisit button[title="Radiology Order"]').text("Diagnostic Imaging Order");
                            $('#InitialOfficeVisit button[title="Radiology Order"]').attr("title", "Diagnostic Imaging Order");
                        }
                        if ($('#InitialOfficeVisit button[title="Radiology Results"]').length > 0) {

                            $('#InitialOfficeVisit button[title="Radiology Results"]').text("Diagnostic Imaging Results");
                            $('#InitialOfficeVisit button[title="Radiology Results"]').attr("title", "Diagnostic Imaging Results");
                        }
                    }, 3000);
                });
                IsBackgroundLoaderShow = false;
            }
            catch (e) {
                console.log(e.message);
                IsBackgroundLoaderShow = false;
                ShowHideLoaderOnScreen(false);
            }
        });

    },

    BindNoteComponents: function (NoteComponentJSON, Clinical_Notes_detail, FormId) {
        var deffered = $.Deferred();
        $.each(NoteComponentJSON, function (i, item) {

            try {
                if (Clinical_Notes_detail && Clinical_Notes_detail.IsPhoneEncounter && Clinical_Notes_detail.TemplateName.toLowerCase() == "phone encounter tcm" && Clinical_Notes_detail.VisitReason.toLowerCase() == "transitional care management" && $("#PatientProfile #hfDischargeDate").val() != "") {
                    var tempFreetextSoapText = '';
                    if (item.ComponentName == "Free Text") {
                        tempFreetextSoapText = item.SOAPText;
                    } else {
                        if (item.ComponentName == "Billing Info")
                            $("#" + FormId + " #ProgressnoteHTML #ProgressNoteComponentList clinical_billinginfo").closest('li').remove();
                        if (item.SectionName && item.SectionName != "PhoneEncounterData")
                            $("#" + FormId + " #ProgressnoteHTML #" + item.SectionName + "NoteComponentList").append(item.SOAPText);
                        else
                            $("#" + FormId + " #ProgressnoteHTML").append(item.SOAPText);
                    }

                    if (item.ComponentName == "Billing Info") {
                        $("#" + FormId + " #ProgressnoteHTML #ProgressNoteComponentList").append(tempFreetextSoapText);
                    }

                } else {

                    if (item.ComponentName == "Billing Info")
                        $("#" + FormId + " #ProgressnoteHTML #ProgressNoteComponentList clinical_billinginfo").closest('li').remove();
                    if (item.SectionName && item.SectionName != "PhoneEncounterData")
                        $("#" + FormId + " #ProgressnoteHTML #" + item.SectionName + "NoteComponentList").append(item.SOAPText);
                    else
                        $("#" + FormId + " #ProgressnoteHTML").append(item.SOAPText);
                }

            } catch (e) {
                console.log(e.message);
            }

        });
        deffered.resolve('OK');
        return deffered.promise();
    },
    // To check in Note Components have any error then Rollback Note.
    CheckNoteComponentsData: function (NotesId, IsDirectRollBack) {

        var objData = {};
        objData["NotesId"] = NotesId;
        objData["IsDirectRollBack"] = IsDirectRollBack;
        objData["commandType"] = "check_clinical_notes_conponents_data";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },

    ChangeNoteFontSize: function () {
        var id = $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote #ProgressnoteHTML");
        if (globalAppdata["NoteFontSize"] == "10") {
            id.removeClass("font12 font14");
        } else if (globalAppdata["NoteFontSize"] == "12") {
            id.removeClass("font14");
            id.addClass("font12");
        } else if (globalAppdata["NoteFontSize"] == "14") {
            id.removeClass("font12");
            id.addClass("font14");
        }
    },

    bindCCMProcedureProblems: function () {
        if (Clinical_ProgressNote.params.EnrollmentInfoId && Clinical_ProgressNote.params.IsFromCreateNote) {
            $("#ProgressnoteHTML section[id*='Cli_Goals_Main']").remove();

            //fixme
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append(Clinical_ProgressNote.params.ProgramUpdateHTML);

            Clinical_ProblemLists.getLatestProblemListByPatientId(true);
            $("#ProgressnoteHTML section[id*='Cli_Procedures_Main']").remove();
            Clinical_Procedures.ProceduresSaveCCM(Clinical_ProgressNote.params.Program);
            //db call to save mapping
            delete Clinical_ProgressNote.params.EnrollmentInfoId;
            delete Clinical_ProgressNote.params.ProgramUpdateHTML;
        } else {
            delete Clinical_ProgressNote.params.EnrollmentInfoId;
            delete Clinical_ProgressNote.params.ProgramUpdateHTML;
        }
    },

    hoverFunction: function () {
        $('#pnlClinicalProgressNote #frmClinicalProgressNote .hoverToggle').css('text-decoration', 'none');

    },
    loadAudio: function (obj) {

        var params = [];
        params["patientId"] = $("#" + Clinical_ProgressNote.params["PanelID"] + " #hfPatientId").val();
        params["FolderId"] = '156';
        params["RefCtrl"] = "ImportDoc";
        params["FromNote"] = "1";
        params["mode"] = "Add";
        params["FromAdmin"] = "0";
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["VisitId"] = Clinical_ProgressNote.params.VisitId;
        if (Clinical_ProgressNote.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = Clinical_ProgressNote.params["TabID"];
        }
        LoadActionPan('Document_Import', params);
    },


    AudioFileSearch: function () {
        Clinical_ProgressNote.SearchAudioFile(Clinical_ProgressNote.params.VisitId).done(function (response) {
            if (response.status != false) {

                // $("#AudioPlayer").html("<source id=\"track\" src=\"data:" + response.FileStream.FileType + ";base64," + response.FileStream.Base64FileStream + "\"/>");
                $("#AudioPlayer").html("<source id=\"track\" src=\"data: audio/mp3;base64," + response.FileStream.Base64FileStream + "\"/>");

                $('#' + Clinical_ProgressNote.params.PanelID + '  #AudioPlayer').show();
                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkAudioupload').hide();
                $('#' + Clinical_ProgressNote.params.PanelID + '  #filetag').hide();
                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkPlayfile').hide();
            }
            else {
                $('#' + Clinical_ProgressNote.params.PanelID + '  #AudioPlayer').hide();
                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkAudioupload').show();
                $('#' + Clinical_ProgressNote.params.PanelID + '  #filetag').show();
                $('#' + Clinical_ProgressNote.params.PanelID + '  #lnkPlayfile').hide();
            }

        });

    },

    SearchAudioFile: function (VisitId) {
        var data = "VisitId=" + VisitId;
        // serach parameter , class name, command name of class zia
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "GET_AUDIOFILE");

    },
    ResetTaskTime: function () {
        var self = $("#" + Clinical_ProgressNote.params["PanelID"] + " #DivDuration");

        self.find("#txtTaskHours").val(0);
        self.find("#txtTaskMinutes").val(0);
        self.find("#txtTaskSeconds").val(0);

    },
    StopTaskTime: function () {
        var self = $("#" + Clinical_ProgressNote.params["PanelID"] + " #DivDuration");
        clearInterval(Clinical_ProgressNote.ticker);

        self.find("#btnRecord").removeClass("disableAll");
        self.find("#btnLogTime").removeClass("disableAll");
        self.find("#btnReset").removeClass("disableAll");

        self.find("#txtTaskHours").removeClass("disableAll");
        self.find("#txtTaskMinutes").removeClass("disableAll");
        self.find("#txtTaskSeconds").removeClass("disableAll");

        $("#pnlClinicalProgressNote #txtTaskHours").attr('disabled', false);
        $("#pnlClinicalProgressNote #txtTaskMinutes").attr('disabled', false);
        $("#pnlClinicalProgressNote #txtTaskSeconds").attr('disabled', false);


    },
    RecordTaskTime: function () {
        var self = $("#" + Clinical_ProgressNote.params["PanelID"] + " #DivDuration");
        self.find("#btnRecord").addClass("disableAll")
        self.find("#btnLogTime").addClass("disableAll")
        self.find("#btnReset").addClass("disableAll")

        var hoursControl = self.find("#txtTaskHours");
        var minutesControl = self.find("#txtTaskMinutes");
        var secondsControl = self.find("#txtTaskSeconds");
        var miliSecondsControl = self.find("#txtTaskMiliSeconds");



        hoursControl.addClass("disableAll");
        minutesControl.addClass("disableAll");
        secondsControl.addClass("disableAll");

        var hours = hoursControl.val();
        var minutes = minutesControl.val();
        var seconds = secondsControl.val();

        Clinical_ProgressNote.ticker = setInterval(function () {
            if (seconds == 60) {
                seconds = 0;
                minutes++;
            }
            if (minutes == 60) {
                minutes = 0;
                hours++;
            }

            seconds++;
            hoursControl.val(hours);
            minutesControl.val(minutes);
            secondsControl.val(seconds);

        }, 1000);
    },

    GetNoteTemplateType: function (obj) {

        var data = "IsActive=1&ID=" + $(obj).val();

        var self = $('#' + Clinical_ProgressNote.params.PanelID);
        self.find('.NoteType > select').attr('ddlist', 'GetNoteTemplateType');

        self.find('.NoteType').loadDropDowns(true, data).done(function () {

            if ($(obj).val() == "") {
                self.find('.NoteType > select').val("");
            }
            else {
                self.find('.NoteType > select').val(self.find('.NoteType > select option[value!=""]:first').val());
            }

            $("#" + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #NoteType option").each(function () {
                if ($(this).text().replace(' ', '').toLowerCase() == "phoneencounter") {
                    $(this).remove();
                }
            });

            if ($('#frmClinicalProgressNote').data('bootstrapValidator') != null && typeof $('#frmClinicalProgressNote').data('bootstrapValidator') != 'undefined') {
                $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
            }
        });
    },

    OnChangeNoteTemplate: function (obj) {

        var data = "IsActive=1&ID=" + $(obj).val();
        var templateName = $(obj).find('option:selected').text();
        var self = $('#' + Clinical_ProgressNote.params.PanelID);
        self.find('.NoteType > select').attr('ddlist', 'GetNoteTemplateType');

        self.find('.NoteType').loadDropDowns(true, data).done(function () {

            if ($(obj).val() == "") {
                self.find('.NoteType > select').val(self.find('.NoteType > select option[value!=""]:first').val());
                self.find('.OrderSet').addClass('disableAll');
            }
            else {
                self.find('.NoteType > select').val(self.find('.NoteType > select option[value!=""]:first').val());
                self.find('.OrderSet').removeClass('disableAll');
            }

            $("#" + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #NoteType option").each(function () {
                if ($(this).text().replace(' ', '').toLowerCase() == "phoneencounter") {
                    $(this).remove();
                }
            });

            if ($('#frmClinicalProgressNote').data('bootstrapValidator') != null && typeof $('#frmClinicalProgressNote').data('bootstrapValidator') != 'undefined') {
                $("#" + Clinical_ProgressNote.params.PanelID + " #frmClinicalProgressNote").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
            }

            Clinical_ProgressNote.LoadTemplateData(templateName);
            Clinical_ProgressNote.GetOrderSetTemplate();
        });
    },

    AddOrderSet: function (orderSetID, noDetach) {
        if (noDetach) {
            Clinical_ProgressNote.IsDefaultOrderSet = true;
            Clinical_ProgressNote.DefaultOrderSetID = orderSetID;
            Clinical_ProgressNote.LinkCDSOrderSetWithNote(orderSetID, null, true);
            Clinical_ProgressNote.IsDefaultOrderSet = false;
        }
        else {


            Clinical_OrderSetDetails.detach_ComponentsOrderSet().done(function () {
                Clinical_ProgressNote.IsDefaultOrderSet = true;
                Clinical_ProgressNote.DefaultOrderSetID = orderSetID;
                Clinical_ProgressNote.LinkCDSOrderSetWithNote(orderSetID, null, true);
                Clinical_ProgressNote.IsDefaultOrderSet = false;
            });
        }
    },
    CheckIfAnyOrderSetIsAssociatedWithNote: function (noteid) {
        var objData = new Object();
        objData["NotesId"] = noteid;
        objData["commandType"] = "CheckIfAnyOrderSetIsAssociatedWithNote";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    FillAppointment: function (AppointmentID) {
        var data = "AppointmentID=" + AppointmentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_APPOINTMENT");
    },

    isRightClicked: function (e) {

        e = e || window.event;
        var rightclick;
        if (e.which)
            rightclick = (e.which == 3);
        else if (e.button)
            rightclick = (e.button == 2);

        return rightclick;

    },


    //-------------  START  --------------
    //-- NOTE COMPONENT PARALLER ACCESS --

    IsNoteComponentAvaliable: function (isToCheck, ComponentName) {

        var objDeffered = $.Deferred();

        Clinical_ProgressNote.IsNoteSigned(isToCheck).done(function (res) {

            // if note is not signed then check component access
            if (res == false) {

                if (ComponentName) {
                    Clinical_ProgressNote.getNoteComponentAccess(ComponentName).done(function (response) {
                        objDeffered.resolve(response);
                    });
                }
                else {
                    console.log("Component Name is empty." + ComponentName);
                    objDeffered.resolve(true);
                }

            }
            else // if note is signed then stop component access show not signed alert.
                objDeffered.resolve(false);

        });

        return objDeffered;
    },

    IsNoteSigned: function (isToCheck) {

        var objDeffered = $.Deferred();

        if (isToCheck) {
            objDeffered.resolve(false);
            return objDeffered;
        }

        Clinical_ProgressNote.loadNoteInfo_DBCALL(Clinical_ProgressNote.params.NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status && response.NoteInfo_JSON.length > 0) {
                var status_ = response.NoteInfo_JSON[0].NoteStatus;
                var visitid_ = response.NoteInfo_JSON[0].VisitId;
                if (status_.toLowerCase() == "signed" && visitid_) {
                    utility.DisplayMessages("Note is signed, you can't change in signed note.", 4);

                    $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                    $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + visitid_ + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSave ,#btnSign , #btnReview,#btnNotesDelete,#btnCreate_eSupperbill').addClass('disabled');
                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#ChkBox_IsNonBilable').parent().addClass('disableAll');
                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('div#InitialOfficeVisit,div#sidebar-wrapper').addClass('disableAll');
                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnPrint ,#btnAssign , #btnSend,#btnCreateLetter,#btnSyndromicSurveillance,#btnNoteCoSign,#btnNoteAmendment').removeClass('disabled');
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "none");
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "none");


                    objDeffered.resolve(true);
                }
                else {
                    //console.warn(status_);
                    objDeffered.resolve(false);
                }
            }
            else {
                //console.warn(response.Message);
                objDeffered.resolve(false);
            }

        });

        return objDeffered;
    },

    loadNoteInfo_DBCALL: function (NotesId) {
        var objData = {
        };
        objData["NotesId"] = NotesId;
        objData["commandType"] = "load_clinical_note_info";

        var data = JSON.stringify(objData);
        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    // Method Overview
    //1- Is Note Component avaliable.
    //2- show alert if not avaliable else provide access.
    //3- show revoke alert if that user have permission of Revoke access.
    getNoteComponentAccess: function (ComponentName) {

        var objDeffered = $.Deferred();

        Clinical_ProgressNote.getNoteComponentAccess_DBCALL(ComponentName).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {

                // Note Component is not avaliable User have permission to revoke access.
                if (response.IsNoteComponentAvaliable == true && response.IsUserHaveRevokeAccess == true) {


                    utility.myConfirm(response.Message, function () {
                        // revoke access of this note component.
                        Clinical_ProgressNote.revokeNoteComponentAccess(ComponentName, response.PriorUserId, response.PriorUserName);

                        objDeffered.resolve(true); // after response

                    }, function () {
                        // stop to access note component.
                        objDeffered.resolve(false);
                    },
                     '<b>Concurrent Access</b>');

                } // Note Component is not avaliable and User have't permission to revoke access.
                else if (response.IsNoteComponentAvaliable == false && response.IsUserHaveRevokeAccess == false) {

                    utility.myConfirm(response.Message, function () {
                        // stop to access note component.
                        objDeffered.resolve(false);

                    }, function () {
                        // N/A
                    },
                     '<b>Concurrent Access</b>', "OK", null, true);

                } // Provide access Note Component is avaliable.
                else if (response.IsNoteComponentAvaliable == true) {
                    // allow access to note component
                    Clinical_ProgressNote.registerNoteComponentAccess(ComponentName, response.IsComponentUpdated);
                    objDeffered.resolve(true);
                }
            }
            else {
                //console.warn(response.Message);
                objDeffered.resolve(false);
            }

        });

        return objDeffered;
    },

    getNoteComponentAccess_DBCALL: function (ComponentName) {

        var objData = {
        };
        objData["ComponentName"] = ComponentName;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["NoteAccessTime"] = Clinical_ProgressNote.NoteAccessTime;
        objData["commandType"] = "get_clinical_note_component_access";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");

    },

    revokeNoteComponentAccess: function (ComponentName, PriorUserId, PriorUserName) {

        //1- revoke access from other user
        //2- update access in cashe

        Clinical_ProgressNote.NoteComponentAccessAction_DBCALL("RevokeAccess", ComponentName, PriorUserId, PriorUserName).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {

                if (!Clinical_ProgressNote.SignalRHub)
                    Clinical_ProgressNote.connectUserToSignalR();

                if (Clinical_ProgressNote.SignalRHub) {
                    Clinical_ProgressNote.SignalRHub.server.revokeComponentAccess(response.UserName, response.PriorUserName, response.PriorUserId, response.ComponentName)
                    .done(function (res) {
                        if (res) {
                            UserRecentAccessedNoteComponent = response.ComponentName;
                            Clinical_ProgressNote.getNoteComponentHTML(Clinical_ProgressNote.params.NotesId, response.ComponentName).done(function (res) {
                                Clinical_ProgressNote.updateNoteComponentHTML(res, response.ComponentName);
                            });

                        }

                    });
                }
                else
                    console.log('Not connected to server.');
            }
        });
    },

    closeUserNoteComponent: function (ComponentName) {

        var $Component = $("#pnlClinicalProgressNote");//.find('div[id*="' + ComponentName + '"]');
        var $Component_ActionPan = $Component.find('div[id*="actionPan"]');
        IsRemoveNoteComponent = false;

        if ($Component && $Component_ActionPan.length > 0) {
            if ($Component_ActionPan.length > 1) {

                var unload_backdrop = false;
                for (var i = $Component_ActionPan.length; i >= 0; i--) {

                    if ($($Component_ActionPan[i - 1]).css('display') == 'block') {
                        $($Component_ActionPan[i - 1]).modal('hide');
                        $($Component_ActionPan[i - 1]).find('div').first().hide('blind', 500, function () {
                            $(this).remove();
                        });
                        unload_backdrop = true;
                    }
                    //if ($($Component_ActionPan[i]).siblings('#modaldialog').find('button[class^="close"]'))
                    //    $($Component_ActionPan[i]).siblings('#modaldialog').find('button[class^="close"]').click();
                }

                //unload backdrop
                if (unload_backdrop)
                    $("body").find('div[class*="modal-backdrop fade in"]').each(function () {
                        $(this).remove();
                    });
            }
            else {
                if ($($Component_ActionPan).css('display') == 'block') {

                    $($Component_ActionPan).modal('hide');
                    $($Component_ActionPan).find('div').first().hide('blind', 500, function () {
                        $(this).remove();
                    });

                    //unload backdrop
                    $("body").find('div[class*="modal-backdrop fade in"]').each(function () {
                        $(this).remove();
                    });
                }
                //if ($($Component_ActionPan).siblings('#modaldialog').find('button[class^="close"]'))
                //    $($Component_ActionPan).siblings('#modaldialog').find('button[class^="close"]').click();
            }
        }

        setTimeout(function () {
            IsRemoveNoteComponent = true;
        }, 1000);

    },

    updateNoteComponentHTML: function (response, ComponentName) {
        response = JSON.parse(response);
        if (response.NoteComponentListCount > 0) {

            var parentli = null;
            var arr = $.grep($('a'), function (i, item) {
                if ($(i).attr('onclick')
                   && $(i).attr('onclick').indexOf('SelectNotesComponentTab') >= 0
                   && $(i).attr('onclick').indexOf(ComponentName) >= 0
                   && $(i).parent().parent().parent().attr('notecomponentid')
                   ) {
                    parentli = $(i).parent().parent().parent("li");
                    return;
                }
            });

            $.each(response.NoteComponentListFill_JSON, function (i, item) {

                if (item.SOAPText
                    && item.ComponentName
                    && parentli
                    && parentli.length > 0
                    && parentli.attr('notecomponentid') == item.NoteComponentId) {
                    $(parentli).replaceWith(item.SOAPText);
                }
            });
            Clinical_ProgressNote.CreateNotesComponent_Buttons(Clinical_ProgressNote.params.NotesId);
            if (Clinical_Notes.CopyNotes == true)//for Change Ids In Case CopyNot
            {
                Clinical_ProgressNote.ChangeIds(Clinical_Notes.NewInsertTables, Clinical_Notes.PrevNoteId);
            }
            Clinical_ProgressNote.ShowHideComponetsHeaders();
        }

    },

    registerNoteComponentAccess: function (ComponentName, IsComponentUpdated) {

        //1- register access into cashe

        Clinical_ProgressNote.NoteComponentAccessAction_DBCALL("RegisterAccess", ComponentName).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                UserRecentAccessedNoteComponent = ComponentName;
                if (IsComponentUpdated == true) {
                    Clinical_ProgressNote.getNoteComponentHTML(Clinical_ProgressNote.params.NotesId, ComponentName).done(function (res) {
                        Clinical_ProgressNote.updateNoteComponentHTML(res, ComponentName);
                    });
                }
            }
        });

    },

    NoteComponentAccessAction_DBCALL: function (action, ComponentName, PriorUserId, PriorUserName) {

        var objData = {
        };
        objData["ComponentName"] = ComponentName;
        objData["Action"] = action;
        objData["PriorUserId"] = PriorUserId;
        objData["PriorUserName"] = PriorUserName;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "clinical_note_component_access_action";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");

    },

    connectUserToSignalR: function () {

        try {

            $.connection.hub.stop();
            Clinical_ProgressNote.SignalRHub = null;
            Clinical_ProgressNote.NoteAccessTime = null;
            $.connection.hub.qs = {
                "Username": globalAppdata.AppUserFirstName + " " + globalAppdata.AppUserLastName,
                "UserId": globalAppdata.AppUserId,
                "NotesId": Clinical_ProgressNote.params.NotesId,
            };
            $.connection.hub.disconnected(function () {
                setTimeout(function () {
                    $.connection.hub.start();
                }, 3000); // Restart connection after 5 seconds. 
            });

            Clinical_ProgressNote.SignalRHub = $.connection.providerNoteAccessHub;
            Clinical_ProgressNote.SignalRHub.client.releaseNoteComponentAccess = function (response) {
                response = JSON.parse(response);
                if (response.status) {

                    //1- show alert that User B accessed this component.
                    //2- close component

                    utility.myConfirm(response.Message, function () {
                        // close component
                        Clinical_ProgressNote.closeUserNoteComponent(response.ComponentName);
                    }, function () {
                        // N/A
                    }, '<b>Concurrent Access</b>', "OK", null, true);
                }
            };
            Clinical_ProgressNote.SignalRHub.client.RefreshOtherUserAfterSignNote = function (response) {
                response = JSON.parse(response);
                if (response.status && $("#pnlClinicalProgressNote :visible").length != 0 && Clinical_ProgressNote &&
                        Clinical_ProgressNote.params && Clinical_ProgressNote.params.NotesId && response.NotesId == Clinical_ProgressNote.params.NotesId) {
                    utility.myConfirm((response.UserName ? utility.CapitalizeFirstCharCommaSepString(response.UserName) : "Other User") + " has signed this note", function () {
                        Clinical_ProgressNote.RefreshNote();
                    }, function () {
                    }, '<b>Concurrent Access</b>', "OK", null, true);
                }
            };
            Clinical_ProgressNote.SignalRHub.client.DisableOtherUserSignBtn = function (response) {
                response = JSON.parse(response);
                if (response.status && $("#pnlClinicalProgressNote :visible").length != 0 && Clinical_ProgressNote &&
                        Clinical_ProgressNote.params && Clinical_ProgressNote.params.NotesId && response.NotesId == Clinical_ProgressNote.params.NotesId) {
                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSave,#btnSign,#btnReview,#btnNotesDelete,#btnBillingInfo,#btnSignBottom,#btnSaveBottom').addClass('disabled');
                }
            };
            Clinical_ProgressNote.SignalRHub.client.EnableOtherUserSignBtn = function (response) {
                response = JSON.parse(response);
                if (response.status && $("#pnlClinicalProgressNote :visible").length != 0 && Clinical_ProgressNote &&
                        Clinical_ProgressNote.params && Clinical_ProgressNote.params.NotesId && response.NotesId == Clinical_ProgressNote.params.NotesId) {
                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSave,#btnSign,#btnReview,#btnNotesDelete,#btnBillingInfo,#btnSignBottom,#btnSaveBottom').removeClass('disabled');
                }
            };

            $.connection.hub.start()
           .done(function () {
               console.log('You have connected to the server.');
           })
           .fail(function () {
               console.log('You could not have connected to the server.');
           });

        } catch (e) {
            console.log(e.message);
        }

    },

    disconnectUserFromSignalR: function () {

        try {

            $.connection.hub.stop();
            Clinical_ProgressNote.remove_UserNoteAccess(false, null).done(function (res) {

            });

            Clinical_ProgressNote.SignalRHub = null;
            Clinical_ProgressNote.NoteAccessTime = null;

        } catch (e) {
            console.log(e.message);
        }

    },

    remove_UserNoteAccess: function (IsComponentOnly, ComponentName) {

        var objData = {
        };
        objData["IsComponentOnly"] = IsComponentOnly;
        objData["ComponentName"] = ComponentName;
        objData["commandType"] = "remove_user_note_access";
        var data = JSON.stringify(objData);
        var objDeffered = $.Deferred();


        MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes").done(function (result) {
            objDeffered.resolve(result);
        });


        return objDeffered.promise();
    },


    RegisterNoteAgainstUser: function () {
        Clinical_ProgressNote.registerNoteComponentAccess("SignNote");
    },

    //--------------  END  ---------------
    //-- NOTE COMPONENT PARALLER ACCESS --


    DisableFields: function (IsDisable) {
        if (!IsDisable) {
            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit').hasClass('disableAll')) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit').removeClass('disableAll');
            }

            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' .splitterBody').hasClass('disableAll')) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' .splitterBody').removeClass('disableAll');
            }
            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #sidebar-wrapper').hasClass('disableAll')) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #sidebar-wrapper').removeClass('disableAll');
            }

        }

        $("#frmClinicalProgressNote #txtVisitReason").prop('disabled', IsDisable);
        if (Clinical_ProgressNote.params.IsPhoneEncounter == false) {
            $("#frmClinicalProgressNote #NoteType").prop('disabled', IsDisable);
        }
        else {
            $("#frmClinicalProgressNote #NoteType").prop('disabled', true);
            $("#frmClinicalProgressNote #ddlDuration").prop('disabled', IsDisable);
        }
        $("#frmClinicalProgressNote #txtFacility").prop('disabled', IsDisable);
        $("#frmClinicalProgressNote #lnkFacility").prop('disabled', IsDisable);
        if (IsDisable) {
            if ($('#frmClinicalProgressNote').data('bootstrapValidator') != null && typeof $('#frmClinicalProgressNote').data('bootstrapValidator') != 'undefined') {
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('VisitDate', false);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('VisitTime', false);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('VisitReason', false);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('Facility', false);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('Provider[]', false);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('NoteType', false);
            }
        }
        else {
            if ($('#frmClinicalProgressNote').data('bootstrapValidator') != null && typeof $('#frmClinicalProgressNote').data('bootstrapValidator') != 'undefined') {
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('VisitDate', true);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('VisitTime', true);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('VisitReason', true);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('Facility', true);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('Provider[]', true);
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').data('bootstrapValidator').enableFieldValidators('NoteType', true);
            }
        }
    },

    RefreshNote: function () {
        $("#mstrDivNotes #clinicalTabProgressNote").click();
    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalProgressNote";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabProgressNote";
        LoadActionPan('Admin_Facility', params);
    },
    HideShowBillingInfo: function () {
        //Start 01-12-2016 Humaira Yousaf for lab and radiology order procedures
        if ($("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").length > 0 || $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Procedures_Main']").length > 0
         || $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_LabOrderDetail_Main']").length > 0 || $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_RadiologyOrderDetail_Main']").length > 0) {
            //End 01-12-2016 Humaira Yousaf for lab and radiology order procedures
            if ($("#pnlClinicalProgressNote #ProgressnoteHTML #clinicalMenu_BillingInfo").parent().parent().hasClass("hidden")) {
                //$("#pnlClinicalProgressNote #ProgressnoteHTML #clinicalMenu_BillingInfo").parent().parent().removeClass("hidden");
            }
            if ($("#pnlClinicalProgressNote #ActionsInitialOfficeVisit #clinicalMenu_BillingInfo").hasClass("hidden")) {
                $("#pnlClinicalProgressNote #ActionsInitialOfficeVisit #clinicalMenu_BillingInfo").removeClass("hidden");
            }
        }
        else {
            if (!$("#pnlClinicalProgressNote #ProgressnoteHTML #clinicalMenu_BillingInfo").parent().parent().hasClass("hidden")) {
                //$("#pnlClinicalProgressNote #ProgressnoteHTML #clinicalMenu_BillingInfo").parent().parent().addClass("hidden");
            }
            //if (!$("#pnlClinicalProgressNote #ActionsInitialOfficeVisit #clinicalMenu_BillingInfo").hasClass("hidden")) {
            //    $("#pnlClinicalProgressNote #ActionsInitialOfficeVisit #clinicalMenu_BillingInfo").addClass("hidden");
            //}
        }
    },
    ChangeIds: function (NewInsertTablesData, PrNoteId, notSave) {
        if (NewInsertTablesData != "") {
            var NewInsertTables = JSON.parse(NewInsertTablesData);
            $.each(NewInsertTables, function (i, item) {
                if (item.TableName == "VitalSigns") {
                    Clinical_Vitals.GetVitalInfo(item.PkId, true);
                }
                else if (item.TableName == "Complaints") {
                    var NewComplaintId = item.PkId;
                    var PreviouseComplaintId = $("#" + Clinical_ProgressNote.params.PanelID + ' clinical_complaints').closest('li').find('section').attr('id').replace('Cli_Complaint_Main', '');
                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_complaints').closest('li').find('[id^=Cli_Complaint]'), function (i, item) {
                        $(item).attr('id', $(item).attr('id').replace(PreviouseComplaintId, NewComplaintId));
                    });

                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_complaints').closest('li').find('[name^=Cli_Complaint]'), function (i, item) {
                        $(item).attr('name', $(item).attr('name').replace(PreviouseComplaintId, NewComplaintId));
                    });
                    var clickEvent = $("#" + Clinical_ProgressNote.params.PanelID + ' clinical_complaints').closest('li').find('header .NotesComponent a:nth-child(2)').attr('onclick').replace(',' + PreviouseComplaintId, ',' + NewComplaintId);
                    $("#" + Clinical_ProgressNote.params.PanelID + ' clinical_complaints').closest('li').find('header .NotesComponent a:nth-child(2)').attr('onclick', clickEvent);
                }
                else if (item.TableName == "Functionalandcognitive") {
                    var NewFuntionalandcognitiveId = item.PkId;
                    var PreviouseFuntionalandcognitiveId = $("#" + Clinical_ProgressNote.params.PanelID + ' clinical_functionalandcognitive').closest('li').find('section').attr('id').replace('Cli_Cognitive_Main', '');
                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_functionalandcognitive').closest('li').find('[id^=Cli_Cognitive]'), function (i, item) {
                        $(item).attr('id', $(item).attr('id').replace(PreviouseFuntionalandcognitiveId, NewFuntionalandcognitiveId));
                    });

                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_functionalandcognitive').closest('li').find('[name^=Cli_Cognitive]'), function (i, item) {
                        $(item).attr('name', $(item).attr('name').replace(PreviouseFuntionalandcognitiveId, NewFuntionalandcognitiveId));
                    });
                }

                else if (item.TableName == "Planofcare") {
                    var NewPlanofcareId = item.PkId;
                    var PreviousePlanofcareId = $("#" + Clinical_ProgressNote.params.PanelID + ' clinical_planofcare').closest('li').find('section').attr('id').replace('Cli_PlanOfCare_Main', '');
                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_planofcare').closest('li').find('[id^=Cli_PlanOfCare]'), function (i, item) {
                        $(item).attr('id', $(item).attr('id').replace(PreviousePlanofcareId, NewPlanofcareId));
                    });

                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_planofcare').closest('li').find('[name^=Cli_PlanOfCare]'), function (i, item) {
                        $(item).attr('name', $(item).attr('name').replace(PreviousePlanofcareId, NewPlanofcareId));
                    });
                }

                else if (item.TableName == "PhysicalExam") {
                    var NewPhysicalExamId = item.PkId;
                    var PreviousePhysicalExamId = $("#" + Clinical_ProgressNote.params.PanelID + ' clinical_physicalexam').closest('li').find('section').attr('id').replace('Cli_PhysicalExam_Main', '');
                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_physicalexam').closest('li').find('[id^=Cli_PhysicalExam]'), function (i, item) {
                        $(item).attr('id', $(item).attr('id').replace(PreviousePhysicalExamId, NewPhysicalExamId));
                    });

                    $.each($("#" + Clinical_ProgressNote.params.PanelID + ' clinical_physicalexam').closest('li').find('[name^=Cli_PhysicalExam]'), function (i, item) {
                        $(item).attr('name', $(item).attr('name').replace(PreviousePhysicalExamId, NewPhysicalExamId));
                    });
                }
            });
        }

        //Start Change previouseNoteIdWithNew
        $.each($("#" + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML .initialVisitBody header a'), function (i, item) {
            if ($(this).parent().prop("tagName") == "CLINICAL_COMPLAINTS") {
                var clickEvent = $(this).attr('onclick');
                if (clickEvent.indexOf('SelectNotesComponentTab') > -1) {
                    if (clickEvent.indexOf(',' + PrNoteId) > -1) {
                        clickEvent = clickEvent.replace(',' + PrNoteId, ',' + Clinical_ProgressNote.params.NotesId);
                        $(this).attr('onclick', clickEvent);
                    }
                }
            }
            else if ($(this).parent().prop("tagName") == "CLINICAL_CUSTOMFORM") {
                var clickEvent = $(this).attr('onclick');
                var FunctionParameters = clickEvent.substring(clickEvent.indexOf('(') + 2, clickEvent.indexOf(')') - 1);
                var FunctionParametersSplit = FunctionParameters.split(',');
                FunctionParametersSplit[2] = Clinical_ProgressNote.params.NotesId;
                if (clickEvent.indexOf("SelectNotesComponentTab") > -1) {
                    clickEvent = clickEvent.substring(0, clickEvent.indexOf('(') + 2) + FunctionParametersSplit.join(',') + "')";
                }
                else if (clickEvent.indexOf("RemoveComponentTab") > -1) {
                    clickEvent = clickEvent.substring(0, clickEvent.indexOf('(') + 2) + FunctionParametersSplit.join(',') + "')";
                }

                $(this).attr('onclick', clickEvent);
            }
            else {
                var clickEvent = $(this).attr('onclick');
                if (clickEvent.indexOf(',' + PrNoteId) > -1) {
                    clickEvent = clickEvent.replace(',' + PrNoteId, ',' + Clinical_ProgressNote.params.NotesId);
                    $(this).attr('onclick', clickEvent);
                }
            }

        });

        if ($("#" + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML #signedByProvider').length >= 1) {
            $("#" + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML #signedByProvider').remove();
        }
        else {
            $("#" + Clinical_ProgressNote.params.PanelID + " #ProgressnoteHTML .list-unstyled li:contains('e-Signed by:')").parent().remove();
        }

        if ($("#" + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML #AmendmentSection').length >= 1) {
            $("#" + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML #AmendmentSection').remove();
        }
        if ($("#" + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML #coSignedByProvider').length >= 1) {
            $("#" + Clinical_ProgressNote.params.PanelID + ' #ProgressnoteHTML #coSignedByProvider').remove();
        }
        //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
        if (typeof notSave == typeof undefined || !notSave) {
            Clinical_ProgressNote.saveComponentSOAPTextBulk(true, false);
        }
        Clinical_Notes.CopyNotes = false;
        Clinical_Notes.PrevNoteId = 0;
        Clinical_Notes.NewInsertTables = "";
        Clinical_PhoneEncounter.CopyNotes = false;
        Clinical_PhoneEncounter.PrevNoteId = 0;
        Clinical_PhoneEncounter.NewInsertTables = "";

    },

    //Binding Page Ready Function
    InitializeSorting: function () {
        $("#ActionsInitialOfficeVisit").attr('style', '');
        $("#StickyPNSection").affix();
        $("#ActionsInitialOfficeVisit").affix();
        $("#InitialOfficeVisit").resize(function (e) {
            $('#StickyPNSection').css('width', $("#InitialOfficeVisit").css('width'));
            $('#Customeformprintparentdiv').css('width', $("#InitialOfficeVisit").css('width'));
            $('#ActionsInitialOfficeVisit').css('width', ($("#InitialOfficeVisit").width() - 15) + 'px');
        });
        $('#StickyPNSection').css('width', $("#InitialOfficeVisit").css('width'));
        $('#ActionsInitialOfficeVisit').css('width', ($("#InitialOfficeVisit").width() - 15) + 'px');
        $('#Customeformprintparentdiv').css('width', $("#InitialOfficeVisit").css('width'));
        var htmlBillingInfo = $("#pnlClinicalProgressNote #ProgressNoteComponentList #clinicalMenu_BillingInfo").closest('li');
        $("#pnlClinicalProgressNote #ProgressNoteComponentList #clinicalMenu_BillingInfo").closest('li').remove();

        if (Clinical_ProgressNote.params["TemplateName"] != '') {
            //$("#pnlClinicalProgressNote #ProgressNoteComponentList").sortable({
            //    revert: true,
            //    out: function (event, ui) {
            //        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
            //    },
            //});

            $("#pnlClinicalProgressNote #ProgressNoteComponentList").disableSelection();

        }
        else {

            $("#pnlClinicalProgressNote #SubjectiveNoteComponentList").sortable({
                revert: true,
                out: function (event, ui) {
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                },
                stop: function (event, ui) {
                    Clinical_ProgressNote.SetNoteComponentsOrder('Subjective');
                },
            });
            $("#pnlClinicalProgressNote #ObjectiveNoteComponentList").sortable({
                revert: true,
                out: function (event, ui) {
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                },
                stop: function (event, ui) {
                    Clinical_ProgressNote.SetNoteComponentsOrder('Objective');
                },
            });
            $("#pnlClinicalProgressNote #AssessmentNoteComponentList").sortable({
                revert: true,
                out: function (event, ui) {
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                },
                stop: function (event, ui) {
                    Clinical_ProgressNote.SetNoteComponentsOrder('Assessment');
                },
            });
            $("#pnlClinicalProgressNote #PlanNoteComponentList").sortable({
                revert: true,
                out: function (event, ui) {
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                },
                stop: function (event, ui) {
                    Clinical_ProgressNote.SetNoteComponentsOrder('Plan');
                },
            });
            $("#pnlClinicalProgressNote #MiscellaneousNoteComponentList").sortable({
                revert: true,
                out: function (event, ui) {
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                },
                stop: function (event, ui) {
                    Clinical_ProgressNote.SetNoteComponentsOrder('Miscellaneous');
                },
            });
            $("#pnlClinicalProgressNote #SubjectiveNoteComponentList").disableSelection();
            $("#pnlClinicalProgressNote #ObjectiveNoteComponentList").disableSelection();
            $("#pnlClinicalProgressNote #AssessmentNoteComponentList").disableSelection();
            $("#pnlClinicalProgressNote #PlanNoteComponentList").disableSelection();
            $("#pnlClinicalProgressNote #MiscellaneousNoteComponentList").disableSelection();

        }

        if ($('#pnlClinicalProgressNote #hfNoteStatus').val() != 'Signed')
            $("#pnlClinicalProgressNote #ProgressNoteComponentList").append(htmlBillingInfo);

        $('.splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('.splitterBtn a'));

        $('.splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);

            // for affix panel
            if ($($(this).parents("#StickyPNSection")).hasClass("affix")) {
                if ($('#StickyPNSection').css('position') != 'undefined')
                    $('#StickyPNSection').css('position', '');
                setTimeout(function () {
                    var margintop = $('#StickyPNSection').outerHeight();
                    if (Clinical_ProgressNote.params.IsPhoneEncounter == true && $(window).scrollTop() > 40) {
                        margintop += 40;
                    }
                    else {
                        var tempMarginPatch = (margintop - 22);
                        if (tempMarginPatch > 10) tempMarginPatch = 10;
                        $("#InitialOfficeVisit").css("margin-top", tempMarginPatch);
                    }

                }, 300);
            }

            if ($($(this).closest('#frmClinicalProgressNote').find("#ActionsInitialOfficeVisit")).hasClass("affix")) {
                setTimeout(function () {
                    if ($("#StickyPNSection").hasClass("affix-bottom")) {
                        $("#StickyPNSection").removeClass("affix-bottom").addClass('affix');
                        if ($('#StickyPNSection').css('position') != 'undefined')
                            $('#StickyPNSection').css('position', '');
                        if ($('#StickyPNSection').css('top') != 'undefined')
                            $('#StickyPNSection').css('top', '');
                    }
                    if ($('#ActionsInitialOfficeVisit').css('position') != 'undefined')
                        $('#ActionsInitialOfficeVisit').css('position', '');
                    $('#ActionsInitialOfficeVisit').css('top', $('#StickyPNSection').outerHeight() + 149);
                }, 400);
            }
        });
        $('#pnlClinicalProgressNote section').on('click', '.btnPNC_Edit', function (e) {
            Clinical_ProgressNote.EditNotesComments_ComponentAttached($(this).parent().next(), e);
        });


    },

    OnRemoveHandler: function (e) {

        var $this = $(this);
        Clinical_ProgressNote.IsNoteComponentAvaliable(false, $this.attr("name")).done(function (res) {
            if (res == true) {
                Clinical_ProgressNote.RemoveComponentAttached($this.attr("name"));
            }
        });

    },

    OnEditHandler: function (e) {
        var $this = $(this);
        var event_ = e;
        Clinical_ProgressNote.IsNoteComponentAvaliable(false, $this.attr("name")).done(function (res) {
            if (res == true) {
                Clinical_ProgressNote.EditNotesComments_ComponentAttached($this.parent().next(), event_);
            }
        });

    },

    OnDoubleClickComponent: function (ev) {

        var componentId = this.id;
        Clinical_ProgressNote.IsNoteComponentAvaliable(false, componentId).done(function (res) {
            if (res == true) {

                var selectedValue = componentId.match(/[\d\.]+/g).toString();

                if (selectedValue == "" || selectedValue == "undefined") {
                    return false;
                }
                else {
                    var componentType = "";

                    if (componentId.indexOf("Vitals") >= 0) {
                        componentType = 'Vitals';
                    } else if (componentId.indexOf("Problems") >= 0) {
                        componentType = 'ProblemList';
                    } else if (componentId.indexOf("Allergies") >= 0) {
                        componentType = 'Allergies';
                    } else if (componentId.indexOf("SocialHx") >= 0) {
                        componentType = 'SocialHx';
                    } else if (componentId.indexOf("BirthHx") >= 0) {
                        componentType = 'BirthHx';
                    }
                    else if (componentId.indexOf("Immunization") >= 0) {
                        componentType = 'Immunization';
                    }
                    else if (componentId.indexOf("Referrals") >= 0) {
                        componentType = 'Referrals';
                    } else if (componentId.indexOf("MedicalHx") >= 0) {
                        componentType = 'MedicalHx';
                    } else if (componentId.indexOf("Medications") >= 0) {
                        componentType = 'Medications';
                    } else if (componentId.indexOf("Prescription") >= 0) {
                        componentType = 'Prescription';
                    }
                    else if (componentId.indexOf("FamilyHx") >= 0) {
                        componentType = 'FamilyHx';
                    }
                    else if (componentId.indexOf("SurgicalHx") >= 0) {
                        componentType = 'SurgicalHx';
                    }
                    else if (componentId.indexOf("HospitalizationHx") >= 0) {
                        componentType = 'HospitalizationHx';
                    }
                    else if (componentId.indexOf("PhysicalExam") >= 0) {
                        componentType = 'PhysicalExam';
                    }
                    else if (componentId.indexOf("PlanOfCare") >= 0) {
                        componentType = 'PlanOfCare';
                    }
                    else if (componentId.indexOf("Cognitive") >= 0) {
                        componentType = 'Cognitive';
                    }
                    else if (componentId.indexOf("ConsultationOrder") >= 0) {
                        componentType = 'ConsultationOrder';
                    }
                    else if (componentId.indexOf("RadiologyOrder") >= 0) {
                        componentType = 'RadiologyOrder';
                    }
                    else if (componentId.indexOf("LabOrder") >= 0) {
                        componentType = 'LabOrders';
                    }
                    else if (componentId.indexOf("LabResult") >= 0) {
                        componentType = 'LabResults';
                    }
                    else if (componentId.indexOf("RadiologyResult") >= 0) {
                        componentType = 'RadiologyResult';
                    }
                    else if (componentId.indexOf("Cli_Procedures_Main") >= 0) {
                        componentType = 'Procedures';
                    }
                    else if (componentId.indexOf("ProcedureOrder") >= 0) {
                        componentType = 'ProcedureOrder';
                    }
                    else if (componentId.indexOf("FollowUp") >= 0) {
                        componentType = 'FollowUp';
                    }
                    else if (componentId.indexOf("ReviewofSystems") >= 0) {
                        componentType = 'ReviewofSystems';
                    } else if (componentId.indexOf("Complaints") >= 0) {
                        componentType = 'Complaints';
                    }
                    else if (componentId.indexOf("SocPsyandBehaviorHx") >= 0) {
                        componentType = 'SocPsyandBehaviorHx';
                    }

                    else if (componentId.indexOf("PatientEducation") >= 0) {
                        var params = [];
                        params["ParentCtrl"] = "clinicalTabProgressNote";
                        params["FromAdmin"] = 0;
                        params["mode"] = "Add";
                        params["PatientId"] = Clinical_ProgressNote.params.patientID;
                        params["PatDocID"] = $('#' + componentId).attr("PatDocId");
                        LoadActionPan('Document_Viewer', params, Clinical_ProgressNote.params.PanelID);
                    }
                    if (componentType != '') {
                        Clinical_ProgressNote.LoadNotesComponentInEditMode(componentType, selectedValue);
                    }
                }

                //findInDiv.hide(true);

            }
        });


    },

    //This function will load CQM with Reasoning List
    CQMWithReasoningLoad: function (BillingInformation, Obj, customSigMsg, isComponentSelect, NotesId, IsFromProgressNote, isFromEsuperBill) {
        var objDeffered = $.Deferred();

        if (!IsFromProgressNote && BillingInformation && BillingInformation.params && BillingInformation.params.length > 0 && BillingInformation.params["PatientId"])
            Clinical_ProgressNote.params.patientID = BillingInformation.params["PatientId"];

        //this function get CQM notes based on Patient Id, ProviderId, VisitDate
        var VisitDate = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
        Clinical_ProgressNote.arrCQMReasoning = [];
        Clinical_ProgressNote.loadCQMWithReasoning(VisitDate, VisitDate, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params["CurrentNotesProviderId"], Clinical_Notes.params.NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ProgressNote.params["CQMResponse"] = response;
                var IsShowAlert = false;
                if (response.CQMMeasuresCount > 0) {

                    var arrNonCompliantPatients = JSON.parse(response.CQMMeasures_JSON);
                    Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID] = JSON.stringify(arrNonCompliantPatients);
                    IsShowAlert = Clinical_ProgressNote.ValidateMeasure();
                }

                if (IsShowAlert) {
                    //var arrNonCompliantPatients = JSON.parse(response.CQMMeasures_JSON);
                    //Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID] = JSON.stringify(arrNonCompliantPatients);

                    var CQMFoundMsg = "Our System found some <span class=\\'red\\'>missing data</span> related to this patient."
                                    + " In order to meet the performance criteria required for the  <b>Quality category of 2018 MIPS Program,</b> you must enter those missing data values"
                                    + " against <b> Quality measures/eCQMs</b> that you have planned to report this year.<br /> Do you want to enter the data here before signing off this note?";

                    utility.myConfirm(CQMFoundMsg, function () {
                        $.when(Clinical_ProgressNote.ShoweCQMAlerts(NotesId, true)).then(function () {
                            Clinical_ProgressNote.params.isCQMExists = 1;
                            objDeffered.resolve();
                        });
                        //$.when(Clinical_ProgressNote.openPatientList(BillingInformation, Obj, customSigMsg, isComponentSelect, NotesId, IsFromProgressNote)).then(function () {
                        //    Clinical_ProgressNote.params.isCQMExists = 1;
                        //    objDeffered.resolve();
                        //});
                    }, function () {

                        utility.myConfirm("63", function () {

                            if (NotesId && parseInt(NotesId) > 0) {

                                $.when(Clinical_ProgressNote.signNote(NotesId, Clinical_ProgressNote.params.patientID, IsFromProgressNote, Clinical_ProgressNote.params.IsPhoneEncounter, customSigMsg, true, isFromEsuperBill)).then(function () {
                                    objDeffered.resolve();
                                });
                            }
                            else {
                                $.when(Clinical_ProgressNote.SignESuperBill(BillingInformation, Obj, customSigMsg, isComponentSelect, response)).then(function () {
                                    objDeffered.resolve();
                                });
                            }

                        }, function () {
                            objDeffered.resolve();
                        }, "", "Save", "Cancel");



                    }, '<b>2018 eCQMs Missing Data Alert</b>', "Yes, I do", "No, not this time");

                }
                else {

                    $.when(Clinical_ProgressNote.ShoweCQMAlerts(NotesId, false)).then(function () {
                        if (NotesId && parseInt(NotesId) > 0) {
                            $.when(Clinical_ProgressNote.signNote(NotesId, Clinical_ProgressNote.params.patientID, IsFromProgressNote, Clinical_ProgressNote.params.IsPhoneEncounter, customSigMsg, i, isFromEsuperBill)).then(function () {
                                objDeffered.resolve();
                            });
                        }
                        else {
                            $.when(Clinical_ProgressNote.SignESuperBill(BillingInformation, Obj, customSigMsg, isComponentSelect, response)).then(function () {
                                objDeffered.resolve();
                            });
                        }
                    });


                }
            }
            else {
                objDeffered.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return objDeffered;
    },

    VBPWithReasoningLoad: function (BillingInformation, Obj, customSigMsg, isComponentSelect, prntcntrl, NotesId, IsFromProgressNote, isFromEsuperBill) {
        var objDeffered = $.Deferred();
        var VisitDate = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
        Clinical_ProgressNote.loadVBPWithReasoning(VisitDate, VisitDate, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params["CurrentNotesProviderId"], NotesId, IsFromProgressNote).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ProgressNote.params["VBPResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == Clinical_ProgressNote.params.patientID && a.NoteId == Clinical_ProgressNote.params.NotesId;
                    });
                    if (arrNonCompliantPatients.length > 0) {
                        Clinical_ProgressNote.arrVBPReasoning[Clinical_ProgressNote.params.patientID] = JSON.stringify(arrNonCompliantPatients);
                        var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                    + " In order to qualify for the <b>2017 Value Based program incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                    + " against the Value Based program that you have planned to report this year. Do you want to enter the data here before signing off this note?";

                        var PHQ2Missing = false;
                        var PHQ9Missing = false;
                        utility.myConfirm(CQMFoundMsg, function () {
                            $.each(arrNonCompliantPatients, function (key) {
                                if (arrNonCompliantPatients[key].MeasureId == "PHQ2") {
                                    PHQ2Missing = true;
                                }
                                else if (arrNonCompliantPatients[key].MeasureId == "PHQ9") {
                                    PHQ9Missing = true;
                                }
                            });
                            $.when(Clinical_ProgressNote.openMissingAlert_VBP(BillingInformation, Obj, customSigMsg, isComponentSelect, prntcntrl, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val(), PHQ2Missing, PHQ9Missing)).then(function () {
                                Clinical_ProgressNote.params.isVBPExists = 1;
                                objDeffered.resolve();
                            });
                        }, function () {
                            $.when(Clinical_ProgressNote.CQMWithReasoningLoad(BillingInformation, Obj, customSigMsg, isComponentSelect, NotesId, IsFromProgressNote, isFromEsuperBill)).then(function () {
                                objDeffered.resolve();
                            });
                        },
                                  '<b>2017 Value Based Program Missing Data Alert</b>', "Yes, I do", "No, not this time"
                              );
                    }
                    else {
                        $.when(Clinical_ProgressNote.CQMWithReasoningLoad(BillingInformation, Obj, customSigMsg, isComponentSelect, NotesId, IsFromProgressNote, isFromEsuperBill)).then(function () {
                            objDeffered.resolve();
                        });
                    }
                }
                else {
                    $.when(Clinical_ProgressNote.CQMWithReasoningLoad(BillingInformation, Obj, customSigMsg, isComponentSelect, NotesId, IsFromProgressNote, isFromEsuperBill)).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                $.when(Clinical_ProgressNote.CQMWithReasoningLoad(BillingInformation, Obj, customSigMsg, isComponentSelect, NotesId, IsFromProgressNote, isFromEsuperBill)).then(function () {
                    objDeffered.resolve();
                });
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return objDeffered;
    },

    ShoweCQMAlerts: function (NotesId, IsShowPopup) {

        var objDeffered = $.Deferred();

        if (Clinical_ProgressNote.arrCQMReasoning.length > 0 && Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID]) {
            var measures = JSON.parse(Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID]);
            var meas = [];
            var meas_array = [];
            for (var i = 0; i < measures.length; i++) {
                if (meas.indexOf(measures[i].MeasureSectionID) < 0) {
                    meas.push(measures[i].MeasureSectionID);
                    var obj = {
                        ProfileName: measures[i].MeasureNumber + ":- " + measures[i].eMeasureTitle,
                        Fields: measures[i].MeasureDescription,
                        NotesId: Clinical_ProgressNote.params.NotesId,
                        PatientId: Clinical_ProgressNote.params.patientID,
                        MeasureType: measures[i].MeasureType,
                        IsHighPriority: measures[i].HighPriorityMeasure && measures[i].HighPriorityMeasure.toLowerCase() == "yes" ? true : false,
                        Process: measures[i].Process,
                        IsShowAlert: true,
                        Type: "eCQM"
                    };
                    meas_array.push(obj);
                }
            }

            if (meas_array.length > 0) {
                Patient_Demographic.UpdateMUAlert(meas_array, true).done(function (result) {
                    if (result.status != false) {
                        var data = JSON.parse(result.MUAlerts_JSON);
                        var IsAnyOtherAlert = data.filter(item=>item.PatientId + "" == Clinical_ProgressNote.params.patientID + "");
                        if (IsAnyOtherAlert.length > 0 && result.MissingDataAlertCount > 0) {
                            utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
                        }
                        else {
                            utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
                        }
                        //Launch CQM Aler Dialog-box
                        if (IsShowPopup)
                            OpenMU3Alerts('CQM');

                        objDeffered.resolve();
                    } else {
                        console.log(result.Message);
                        objDeffered.resolve();
                    }
                });

            } else {
                objDeffered.resolve();
            }

        } else {
            objDeffered.resolve();
        }

        return objDeffered;
    },

    ValidateMeasure: function () {

        var measures = [];
        if (Clinical_ProgressNote.arrCQMReasoning.length > 0 && Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID]) {
            measures = JSON.parse(Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID]);
        }
        if (measures.length > 0) {
            var meas = [];
            var meas_array = [];
            for (var i = 0; i < measures.length; i++) {
                if (meas.indexOf(measures[i].MeasureSectionID) < 0) {
                    meas.push(measures[i].MeasureSectionID);


                    if (Clinical_ProgressNote.ValidateMeasure_Item(measures[i].MeasureNumber))
                        meas_array.push(measures[i]);
                }
            }
        }

        if (meas_array.length > 0)
            return true;
        else
            return false;

    },

    ValidateMeasure_Item: function (MeasureNumber) {
        var IstoAdd = false;
        switch (MeasureNumber) {
            case "CMS65v7":
                {
                    if (globalAppdata["IsCMS65v7"].toLowerCase() == "true")
                        IstoAdd = true;
                    break;
                };
            case "CMS69v6":
                {
                    if (globalAppdata["IsCMS69v6"].toLowerCase() == "true")
                        IstoAdd = true;
                    break;
                };
            case "CMS68v7":
                {
                    if (globalAppdata["IsCMS68v7"].toLowerCase() == "true")
                        IstoAdd = true;
                    break;
                };
            case "CMS138v6":
                {
                    if (globalAppdata["IsCMS138v6"].toLowerCase() == "true")
                        IstoAdd = true;
                    break;
                };
            case "CMS165v6":
                {
                    if (globalAppdata["IsCMS165v6"].toLowerCase() == "true")
                        IstoAdd = true;
                    break;
                };
            case "CMS22v6":
                {
                    if (globalAppdata["IsCMS22v6"].toLowerCase() == "true")
                        IstoAdd = true;
                    break;
                };
            default:

        }

        return IstoAdd;
    },

    //Opening Patient List from PQRS Report Dashboard
    openPatientList: function (BillingInformation, Obj, customSigMsg, isComponentSelect, NotesId, IsFromProgressNote) {
        var objDeffered = $.Deferred();
        var params = [];
        params["mode"] = "Add";
        params["PatientIds"] = Clinical_ProgressNote.params.patientID;
        params["BillingInformation"] = BillingInformation;
        params["Obj"] = Obj;
        params["NotesId"] = NotesId;
        params["IsFromProgressNote"] = IsFromProgressNote;
        params["customSigMsg"] = customSigMsg;
        params["isComponentSelect"] = isComponentSelect;
        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        var arrcurrentMeasureReasoning = [];
        if (Clinical_ProgressNote.arrCQMReasoning.length > 0 && Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID]) {
            arrcurrentMeasureReasoning = JSON.parse(Clinical_ProgressNote.arrCQMReasoning[Clinical_ProgressNote.params.patientID]);
        }
        if (arrcurrentMeasureReasoning != null && arrcurrentMeasureReasoning.length > 0) {
            params["arrcurrentMeasureReasoning"] = arrcurrentMeasureReasoning;
        }
        else {
            params["arrcurrentMeasureReasoning"] = "";
        }
        params["FromParentCtrl"] = "ProgressNote";
        params["PatientId"] = Clinical_ProgressNote.params.patientID;
        params["FromAdmin"] = 0;
        params["ReportFromDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
        params["ReportToDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();

        LoadActionPan('PQRS_Patient_List', params);
        objDeffered.resolve();
        return objDeffered;
    },

    openMissingAlert_VBP: function (BillingInformation, Obj, customSigMsg, isComponentSelect, prntctrl, patientID, NotesId, ProviderId, NoteDate, PHQ2Missing, PHQ9Missing) {
        var params = [];
        params["FromAdmin"] = "0";
        params["PatientIds"] = patientID;
        params["NoteId"] = NotesId;
        params["BillingInformation"] = BillingInformation;
        params["Obj"] = Obj;
        params["customSigMsg"] = customSigMsg;
        params["isComponentSelect"] = isComponentSelect;
        params["ProviderId"] = ProviderId;
        params["NoteDate"] = NoteDate; //$('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
        params["ParentCtrl"] = prntctrl;// 'clinicalTabProgressNote';
        params["PHQ2Missing"] = PHQ2Missing;
        params["PHQ9Missing"] = PHQ9Missing;
        LoadActionPan('VBP_MissingDataAlert', params);
    },
    //------------------DB Call Functions------------------

    //This Function will load CQM with Reasoning for current Note's Patient, ProviderId, VisitDate
    loadCQMWithReasoning: function (from, to, PatientId, ProviderId, NotesId) {
        var objData = {
        };

        objData["from"] = from;
        objData["to"] = from;
        objData["reportType"] = "0";
        objData["cqmId"] = "";
        objData["eitherDetail"] = "0";
        objData["PatientId"] = PatientId;
        objData["ProviderId"] = ProviderId;
        //objData["VisitId"] = (Clinical_ProgressNote.params.AppointmentVisitId < 1) ? Clinical_ProgressNote.params.VisitId : Clinical_ProgressNote.params.AppointmentVisitId;
        objData["NotesId"] = NotesId;
        objData["commandType"] = "load_cqm_with_reasoning";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    loadVBPWithReasoning: function (from, to, PatientId, ProviderId, NotesId, IsFromProgressNote) {
        var objData = {};
        objData["from"] = from;
        objData["to"] = to;
        objData["reportType"] = "0";
        objData["eitherDetail"] = "0";
        objData["PatientId"] = PatientId;
        objData["ProviderId"] = ProviderId;
        objData["NotesId"] = NotesId;
        objData["IsFromProgressNote"] = IsFromProgressNote;
        objData["commandType"] = "load_VBP_with_reasoning";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    //This Function will get values against the NotesID, edited in Progress Note
    FillNotes: function (NotesData, NotesId, PatientId) {
        var objData = {
        };
        if (NotesData != null) {
            objData = JSON.parse(NotesData);
        }

        if (!PatientId)
            PatientId = $('#PatientProfile #hfPatientId').val();

        objData["NotesId"] = NotesId;
        objData["PatientId"] = PatientId;
        objData["commandType"] = "FILL_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    LoadProgressNote: function (NotesData, NotesId, PatientId) {
        var objData = {
        };
        if (NotesData != null) {
            objData = JSON.parse(NotesData);
        }

        if (!PatientId)
            PatientId = $('#PatientProfile #hfPatientId').val();

        objData["NotesId"] = NotesId;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["ProviderId"] = Clinical_ProgressNote.params.ProviderId;
        objData["commandType"] = "LOAD_CLINICAL_PROGRESS_NOTE";
        objData["OrderSetId"] = Clinical_ProgressNote.DefaultOrderSetID;
        objData["IsPreviousNoteROS"] = Clinical_ProgressNote.IsPreviousNoteROS;
        objData["IsPreviousNotePE"] = Clinical_ProgressNote.IsPreviousNotePE;
        objData["IsPreviousNoteComplaints"] = Clinical_ProgressNote.IsPreviousNoteComplaints;
        objData["IsPreviousNoteProblems"] = Clinical_ProgressNote.IsPreviousNoteProblems;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    getNoteComponentHTML: function (NotesId, ComponentName) {
        var objData = {
        };
        objData["NotesId"] = NotesId;
        objData["ComponentName"] = ComponentName;
        objData["commandType"] = "get_clinical_note_component_html";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },


    //this function get patient notes based on Patient Id From  Server
    SearchPatientNotes: function (NotesData, NotesId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = null;
        }
        if (RowsPerPage == null) {
            RowsPerPage = null;
        }
        var objData = {
        };
        objData["NotesId"] = null;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        if (Clinical_Notes.params.patientID != null) {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
            objData["IsPhoneEncounter"] = true;
        }
        objData["commandType"] = "SEARCH_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },



    //this function delete attached to Patient Progress note based on Patient Id From  Server
    RemoveVitals_DBcall: function (VitalsId, NotesId) {
        var objData = {
        };
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_vitalby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Vitals");
    },

    //This Function Updates Progreses Note HTML, if there is any change in HTML from server
    updateProgressNoteHTML_DBCALL: function (NotesData, NotesID, NoteText, bMedReconciled, MedReconciledId) {
        var objData = (NotesData != null) ? JSON.parse(NotesData) : {
        };
        if (NoteText != null && NotesData == null) {
            objData["NoteText"] = NoteText;
        }
        objData["bMedReconciled"] = (bMedReconciled != null && bMedReconciled != "" && bMedReconciled == true) ? "1" : "0";
        if (objData["bMedReconciled"] == "1") {
            objData["MedReconciledId"] = MedReconciledId;
        }
        else {
            objData["MedReconciledId"] = "";
        }

        objData["NotesId"] = NotesID;
        objData["PatientID"] = Clinical_Notes.params.patientID;
        objData["AppointmentID"] = Clinical_Notes.params.AppointmentID;
        objData["commandType"] = "update_clinical_notes_progressnotehtml";

        var data = JSON.stringify(objData);
        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    //------------------END DB Call Functions------------------

    //This function get information of previous Note saved for the current Patient Selected
    GetPreviousNotePatient: function () {

        Clinical_Notes.GetPreviousNotePatient_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_PreviousNote_detail = JSON.parse(response.PreviousNote_JSON);// LinkedAppointmentCount
                if (response.PreviousNoteCount > 0) {
                    //Detaching Previously AttachedVitals
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML li header a[title=Remove]').each(function (i, ele) {
                        RemoveEvent = $(this).attr('onclick');
                        var ComponentName = $(this).parent().attr("Title");
                        var IsUpdate = false;
                        if (ComponentName == "Vitals") {
                            //Detaching Previously AttachedVitals
                            Clinical_Vitals.detach_ComponentsVitals(ComponentName, IsUpdate, true);
                        }
                    });
                    var CC = Clinical_Notes_PreviousNote_detail.CheifComplaint;
                    $("#pnlClinicalProgressNote #txtCopayPreviousNote").val(utility.RemoveTimeFromDate(null, Clinical_Notes_PreviousNote_detail.NoteDate) + ((CC != null && CC != '') ? " - " + Clinical_Notes_PreviousNote_detail.CheifComplaint : ""));
                    $("#pnlClinicalProgressNote #hfNoteText").val(Clinical_Notes_PreviousNote_detail.NoteText);
                    $("#pnlClinicalProgressNote #hfPrevNotesId").val(Clinical_Notes_PreviousNote_detail.PrevNotesId);
                    $("#pnlClinicalProgressNote #chkCopayPreviousNote").prop('checked', true);

                    //Show Hide of Previous Note Label
                    Clinical_Notes.UnLinkPreviousNotePatient($("#pnlClinicalProgressNote #chkCopayPreviousNote"), 'pnlClinicalProgressNote');

                    if (Clinical_Notes_PreviousNote_detail.NoteText == null || Clinical_Notes_PreviousNote_detail.NoteText == '') {
                        $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML").html('<ul class="initialVisit" id="ProgressNoteComponentList"></ul>'
                             + '<label class="m-sm p-xs bg-gray ">Subjective</label>'
                                + '<ul class="initialVisit ui-sortable" id="SubjectiveNoteComponentList"></ul>'
                                + '<label class="m-sm p-xs bg-gray ">Objective</label>'
                                + '<ul class="initialVisit ui-sortable" id="ObjectiveNoteComponentList"></ul>'
                                + '<label class="m-sm p-xs bg-gray ">Assessment</label>'
                                + '<ul class="initialVisit ui-sortable" id="AssessmentNoteComponentList"></ul>'
                                + '<label class="m-sm p-xs bg-gray ">Plan</label>'
                                + '<ul class="initialVisit ui-sortable" id="PlanNoteComponentList"></ul>'
                                + '<label class="m-sm p-xs bg-gray ">Miscellaneous</label>'
                                + '<ul class="initialVisit ui-sortable" id="MiscellaneousNoteComponentList"></ul>');
                    } else {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').html('');
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append(Clinical_Notes_PreviousNote_detail.NoteText);
                        //Attaching New Vitals to Progress Note, as New HTML is attached to Progress Note
                        Clinical_ProgressNote.AttachVitalsOfPrevComponent();
                        //Call For HTML Update for Progress note
                        Clinical_ProgressNote.updateProgressNoteHTML();
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Attaching New Vitals to Progress Note, as New HTML is attached to Progress Note
    AttachVitalsOfPrevComponent: function (ComponentName) {
        var VitalId = '';

        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML  ').find('li').find('section').each(function () {
            var elementVital = this.id.replace('Cli_Vitals_Main', '')
            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + elementVital).length > 0) {

                VitalId = VitalId + elementVital;
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + elementVital).html($(this).html());

            }
        });

        if (VitalId != '') {
            Clinical_Vitals.AttachVitalSignFromNotes(VitalId);
        }

        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
    },

    // Generating buttons for Components which are dropped on Progress note
    CreateNotesComponent_Buttons: function (NotesId) {
        $('#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit').html('');
        $('#pnlClinicalProgressNote #ProgressnoteHTML .NotesComponent').each(function (index, element) {
            var componentTitle = $(element).attr("title");
            if (componentTitle == "eSuperbill" || componentTitle == "Billing Information") {
                if (!$(element).parent().parent().hasClass("hidden")) {
                    if (Clinical_ProgressNote.params.FromCCM) {
                        $("#pnlClinicalProgressNote #txtTaskHours").attr('disabled', false);
                        $("#pnlClinicalProgressNote #txtTaskMinutes").attr('disabled', false);
                        $("#pnlClinicalProgressNote #txtTaskSeconds").attr('disabled', false);
                    }
                }


            } else if ($(element).find('a').attr('title') == "CustomForms") {
                //Begin Edited by Azeem Raza Tayyab on 23-Nov-2016 to Fix Bug#:EMR-2048
                $(element).parent().next().addClass("disableAll");
                if (componentTitle == "Custom Forms") {
                    var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("CustomFormPreview","' + element.id + '","' + NotesId + '","' + encodeURIComponent(componentTitle) + '","' + $(element).attr("uniqueid") + '","' + encodeURIComponent($(element).attr("docid")) + '");';
                    if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                        onClick = "EMRUtility.scrollToPNcomponent('clinical_CustomForm')";
                    var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" uniqueid="' + $(element).attr("uniqueid") + '"  title="' + componentTitle
                           + '" id="' + element.id + '"  onclick=' + onClick + '>' + componentTitle + '</button>');
                }
                else {
                    var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("CustomFormPreview","' + element.id + '","' + NotesId + '","' + encodeURIComponent(componentTitle) + '","' + $(element).attr("uniqueid") + '","' + encodeURIComponent($(element).attr("docid")) + '");';
                    if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                        onClick = "EMRUtility.scrollToPNcomponent('clinical_CustomForm')";
                    var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" uniqueid="' + $(element).attr("uniqueid") + '"  title="' + componentTitle
                           + '" id="' + element.id + '"  onclick=' + onClick + '>' + componentTitle + '</button>');

                    if (!$(element).parent().parent().hasClass("hidden"))
                        $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
                }
                // End Edited by Azeem Raza Tayyab on 23-Nov-2016 to Fix Bug#:EMR-2048
            } else if (componentTitle.toLowerCase() == 'complaints') {
                var title = componentTitle.split(" ").join("");
                var $control = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML Clinical_Complaints').find('a[title=Complaints]');
                if ((globalAppdata["IsDefaultHPI"] == "True" && Clinical_ProgressNote.IsNewNote) || ($control && $control.attr("onclick") && $control.attr("onclick").indexOf('HPIComplaints') > -1)) {
                    "HPIComplaints";
                }
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("' + title + '","-1",' + NotesId + ');';
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_" + Clinical_ProgressNote.GetComponentName(componentTitle) + "')";
                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle
                                              + '" id="' + element.id + '" onclick=' + onClick + '>' + componentTitle + '</button>');

                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);

            } else if (componentTitle == 'Review of Systems') {
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("ReviewofSystemsRevmap","-1",' + NotesId + ');';
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_reviewofsystems')";
                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle
                               + '" id="' + element.id + '" onclick=' + onClick + '>' + componentTitle + '</button>');
                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
            }
            else if (componentTitle == 'Social, Psychological and Behavior Hx') {
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("SocPsyandBehaviorHx","-1",' + NotesId + ');';
                if (globalAppdata["isMU3SocPsycBehaviourHx"] && globalAppdata["isMU3SocPsycBehaviourHx"].toLowerCase() == "false")
                    onClick = "";
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_SocPsyandBehaviorHx')";
                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle
                               + '" id="' + element.id + '" onclick=' + onClick + '>' + componentTitle + '</button>');

                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
            }
            else if (componentTitle == 'Radiology Order' || componentTitle == 'Diagnostic Imaging Order') {
                var componentName = 'Diagnostic Imaging Order';
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("' + componentTitle.split(" ").join("") + '","' + element.id + '",' + NotesId + ')';
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_radiologyorder')";
                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle
                                + '" id="' + element.id + '" onclick=' + onClick + '>' + componentName + '</button>');

                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
            }
            else if (componentTitle == 'Radiology Results' || componentTitle == 'Diagnostic Imaging Results') {
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("' + componentTitle.split(" ").join("") + '","' + element.id + '",' + NotesId + ')';
                var componentName = 'Diagnostic Imaging Result';
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_diagnosticimagingresults'); EMRUtility.scrollToPNcomponent('clinical_radiologyresults');";
                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle
                                + '" id="' + element.id + '" onclick=' + onClick + '>' + componentName + '</button>');

                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
            }
            else if (componentTitle == 'Implantable Devices' || componentTitle == 'ImplantableDevices') {
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("ImplantableDevices","-1",' + NotesId + ');';
                if (globalAppdata["isImplantableDevices"] && globalAppdata["isImplantableDevices"].toLowerCase() == "false")
                    onClick = "";
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_" + Clinical_ProgressNote.GetComponentName(componentTitle) + "')";
                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle + '" id="' + element.id + '" onclick=' + onClick + '>' + componentTitle + '</button>');
                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
            }
            else if (componentTitle == 'FunctionalAndCognitive' || componentTitle == 'FuntionalAndCognitive' || componentTitle == 'FunctionalandCognitive') {
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("FunctionalAndCognitive","-1",' + NotesId + ');';
                if (globalAppdata["isConsolidatedCDACreationPreformance"] && globalAppdata["isConsolidatedCDACreationPreformance"].toLowerCase() == "false")
                    onClick = "";
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_FuntionalAndCognitive)";
                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle + '" id="' + element.id + '" onclick=' + onClick + '>' + componentTitle + '</button>');
                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
            }
            else {
                //if (componentTitle == 'Review of Systems')
                //    componentTitle = 'Review of Systems Revamp';
                var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab("' + componentTitle.split(" ").join("") + '","' + element.id + '",' + NotesId + ');';
                if (globalAppdata["isLandOnComponent"] && globalAppdata["isLandOnComponent"].toLowerCase() == "true")
                    onClick = "EMRUtility.scrollToPNcomponent('clinical_" + Clinical_ProgressNote.GetComponentName(componentTitle) + "')";

                var Element = $('<button type="button" class="btn btn-default btn-xs tab_space mb-xs" title="' + componentTitle
                                + '" id="' + element.id + '" onclick=' + onClick + '>' + componentTitle + '</button>');

                if (!$(element).parent().parent().hasClass("hidden"))
                    $("#pnlClinicalProgressNote #frmClinicalProgressNote #ActionsInitialOfficeVisit").append(Element);
            }
        });
    },
    GetComponentName: function (componentTitle) {
        var name = ""
        switch (componentTitle.replace(/ /g, '')) {
            case 'Medication':
            case 'Medications':
                name = "medications";
                break;
            case 'Prescription':
            case 'Prescriptions':
                name = "prescription";
                break;
            case 'PhysicalExam':
                name = "physicalexam";
                break;
            case 'FunctionalAndCognitive':
            case 'FuntionalAndCognitive':
            case 'FunctionalandCognitive':
                name = "functionalandcognitive";
                break;
            case 'ReviewofSystems':
            case 'ReviewofSystemsRevmap':
                name = "reviewofsystems";
                break;
            case 'RadiologyOrder':
            case 'DiagnosticImagingOrder':
                name = "radiologyorder";
                break;
            case 'LabOrders':
            case 'LabOrder':
                name = "LabOrders";
                break;
            case 'SocPsyandBehaviorHx':
                name = "SocPsyandBehaviorHx";
                break;
            default:
                name = componentTitle.split(" ").join("");
                break;
        }
        return name;
    },

    showPHQScorePopUp: function (ParentCntrl) {
        var params = [];
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["PatientId"] = Clinical_ProgressNote.params.patientID;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = ParentCntrl;//"Clinical_Procedures";//"clinicalTabProgressNote";
        params["mode"] = "Add";
        params["VisitId"] = Clinical_ProgressNote.params.VisitId;
        params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
        params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        LoadActionPan("VBP_PHQ2Questionnaire", params);
    },
    refreshPatientBanner: function (SelectedProcedureIds) {
        if ($("#PatientProfile #hfPatientId").val() == Clinical_ProgressNote.params.patientID) {
            Clinical_Procedures.isPHQProcedure(SelectedProcedureIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false && response.isPHQProcedure.toLowerCase() == "1") {
                    setPatientBanner(Clinical_ProgressNote.params.patientID);
                }
                else {
                    //utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SelectNotesComponentTab: function (NoteComponent, ComponentId, NotesId, customFormName, customFormUniqueId, customFormNameForDoc) {

        Clinical_ProgressNote.params["IsFromPanel"] = false;
        Clinical_ProgressNote.IsNoteComponentAvaliable(false, NoteComponent).done(function (res) {
            if (res == true) {
                Clinical_ProgressNote.DetachedNoteComponentIds = [];
                Clinical_ProgressNote.AttachedNoteComponentIds = [];

                var params = [];
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "clinicalTabProgressNote";
                params["mode"] = "Add";
                // diffrent practice to use PatientId, in captalization
                params["PatientId"] = Clinical_ProgressNote.params.patientID;

                switch (NoteComponent.replace(/ /g, '')) {
                    case 'Vitals':
                        var Clinical_VitalIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('section[id*="Cli_Vitals_Main"]').map(function () {
                            return this.id.replace("Cli_Vitals_Main", "");
                        }).get().join(',');
                        var vitalDateStr = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('section[id*="Cli_Vitals_Main"]').find('ul > li:first').text();
                        var vitalSignDate = vitalDateStr.substring(vitalDateStr.indexOf(" on ") + 4, vitalDateStr.indexOf(" at "));

                        if (Clinical_VitalIds != '') {
                            params["VitalSignId"] = Clinical_VitalIds;
                            params["VitalSignDate"] = vitalSignDate;
                            params["mode"] = "Edit";
                        } else {
                            params["VitalSignId"] = "-1";
                        }
                        params["NoteVitalSignId"] = params["VitalSignId"];
                        params["RefCtrl"] = "hfVitalsId";
                        LoadActionPan('Clinical_Vitals', params);
                        break;
                    case 'Problems':
                        params["ProblemListId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["IsFromNote"] = true;
                        if (Clinical_ProblemLists.bIsFirstLoad == false) {
                            $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                            // Commented agaisnt Ticket No AST-179
                            //RemoveAdminTab('clinicalTabProblemLists');  
                        }
                        LoadActionPan('Clinical_ProblemLists', params);


                        break;

                    case 'Allergies':
                        params["AllergyId"] = "-1";
                        LoadActionPan('Clinical_Allergies', params);

                        break;

                    case 'Procedures':
                        params["ProceduresId"] = "-1";
                        LoadActionPan('Clinical_Procedures', params);

                        break;

                    case 'SocialHx':
                        var Clinical_SocialHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_socialhx').parent().parent().find('section[id*="Cli_SocialHx_Main"]').map(function () {
                            return this.id.replace("Cli_SocialHx_Main", "");
                        }).get().join(',');
                        if (Clinical_SocialHxIds != null && Clinical_SocialHxIds != '') {
                            params["SocialHxId"] = Clinical_SocialHxIds;
                            params["mode"] = "Edit";
                        } else {
                            params["SocialHxId"] = "-1";
                        }

                        params["TabToSelected"] = "SocialHx";
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;

                    case 'BirthHx':
                        var Clinical_BirthHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Birthhx').parent().parent().find('section[id*="Cli_BirthHx_Main"]').map(function () {
                            return this.id.replace("Cli_BirthHx_Main", "");
                        }).get().join(',');
                        if (Clinical_BirthHxIds != null && Clinical_BirthHxIds != '') {
                            params["BirthHxId"] = Clinical_BirthHxIds;
                            params["mode"] = "Edit";
                        } else {
                            params["BirthHxId"] = "-1";
                        }
                        params["TabToSelected"] = "BirthHx";
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;

                    case 'MedicalHx':
                        var Clinical_MedicalHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Medicalhx').parent().parent().find('section[id*="Cli_MedicalHx_Main"]').map(function () {
                            return this.id.replace("Cli_MedicalHx_Main", "");
                        }).get().join(',');
                        if (Clinical_MedicalHxIds != null && Clinical_MedicalHxIds != '') {
                            params["MedicalHxId"] = Clinical_MedicalHxIds;
                            params["mode"] = "Edit";
                        } else {
                            params["MedicalHxId"] = "-1";
                        }
                        params["TabToSelected"] = "MedicalHx";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;

                    case 'FamilyHx':
                        var Clinical_FamilyHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Familyhx').parent().parent().find('section[id*="Cli_FamilyHx_Main"]').map(function () {
                            return this.id.replace("Cli_FamilyHx_Main", "");
                        }).get().join(',');
                        if (Clinical_FamilyHxIds != null && Clinical_FamilyHxIds != '') {
                            params["FamilyHxId"] = Clinical_FamilyHxIds;
                            params["mode"] = "Edit";
                        } else {
                            params["FamilyHxId"] = "-1";
                        }
                        params["TabToSelected"] = "FamilyHx";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;

                    case 'SurgicalHx':
                        var Clinical_SurgicalHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Surgicalhx').parent().parent().find('section[id*="Cli_SurgicalHx_Main"]').map(function () {
                            return this.id.replace("Cli_SurgicalHx_Main", "");
                        }).get().join(',');
                        if (Clinical_SurgicalHxIds != null && Clinical_SurgicalHxIds != '') {
                            params["SurgicalHxId"] = Clinical_SurgicalHxIds;
                            params["mode"] = "Edit";
                        } else {
                            params["SurgicalHxId"] = "-1";
                        }
                        params["TabToSelected"] = "SurgicalHx";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;

                    case 'HospitalizationHx':
                        var Clinical_HospitalizationHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Hospitalizationhx').parent().parent().find('section[id*="Cli_HospitalizationHx_Main"]').map(function () {
                            return this.id.replace("Cli_HospitalizationHx_Main", "");
                        }).get().join(',');
                        if (Clinical_HospitalizationHxIds != null && Clinical_HospitalizationHxIds != '') {
                            params["HospitalizationHxId"] = Clinical_HospitalizationHxIds;
                            params["mode"] = "Edit";
                        } else {
                            params["HospitalizationHxId"] = "-1";
                        }
                        params["TabToSelected"] = "HospitalizationHx";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;

                    case 'Immunization':

                        var Clinical_ImmunizationIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Immunization').parent().parent().find('section[id*="Cli_Immunization_Main"]').map(function () {
                            return this.id.replace("Cli_Immunization_Main", "");
                        }).get().join(',');
                        if (Clinical_ImmunizationIds != null && Clinical_ImmunizationIds != '') {
                            params["ImmunizationId"] = Clinical_ImmunizationIds;
                            params["mode"] = "Edit";
                        } else {
                            params["ImmunizationId"] = "-1";
                        }

                        params["ParentPanelID"] = Clinical_ProgressNote.params.PanelID;
                        LoadActionPan('Clinical_Immunization', params);

                        break;

                    case 'Referrals':
                        var Clinical_ReferralsIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_referrals').parent().parent().find('section[id*="Patient_Referrals_Main"]').map(function () {
                            return this.id.replace("Patient_Referrals_Main", "");
                        }).get().join(',');
                        if (Clinical_ReferralsIds != null && Clinical_ReferralsIds != '') {
                            params["ReferralId"] = Clinical_ReferralsIds;
                            params["mode"] = "Edit";
                        } else {
                            params["ReferralId"] = "-1";
                        }
                        // Ast-357 
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Patient_Referrals', params);

                        break;

                    case 'NotesExtraInfo':
                        params["From"] = "NotesExtraInfo";
                        LoadActionPan('Clinical_NotesExtraInfo', params);

                        break;
                    case 'TransitionOfCare':
                        params["From"] = "TransitionOfCare";
                        LoadActionPan('Clinical_NotesExtraInfo', params);

                        break;

                    case 'Medication':
                    case 'Medications':
                        params["MedicationsId"] = "-1";
                        params["MedicationsTab"] = "Medications";
                        params["PrescriptionId"] = "-1";
                        LoadActionPan('Clinical_Medications', params);

                        break;
                    case 'Prescription':
                    case 'Prescriptions':
                        params["MedicationsId"] = "-1";
                        params["MedicationsTab"] = "Prescription";
                        params["PrescriptionId"] = "-1";
                        LoadActionPan('Clinical_Medications', params);

                        break;

                    case 'PhysicalExam':
                        var Clinical_PhysicalExamIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_physicalexam').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').map(function () {
                            return this.id.replace("Cli_PhysicalExam_Main", "");
                        }).get().join(',');
                        if (Clinical_PhysicalExamIds != null && Clinical_PhysicalExamIds != '') {
                            params["PhysicalExamTemplateId"] = Clinical_PhysicalExamIds;
                            params["mode"] = "Edit";
                        } else {
                            params["PhysicalExamTemplateId"] = "-1";
                        }
                        params["FromAdmin"] = 0;
                        params["ParentCtrl"] = 'clinicalTabProgressNote';
                        LoadActionPan('PhysicalExamTemplatesRevamp', params);
                        break;

                    case 'PlanOfCare':
                        var Clinical_PlanOfCareIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Planofcare').parent().parent().find('section[id*="Cli_PlanOfCare_Main"]').map(function () {
                            return this.id.replace("Cli_PlanOfCare_Main", "");
                        }).get().join(',');
                        if (Clinical_PlanOfCareIds != null && Clinical_PlanOfCareIds != '') {
                            params["PlanOfCareId"] = Clinical_PlanOfCareIds;
                            params["mode"] = "Edit";
                        } else {
                            params["PlanOfCareId"] = "-1";
                        }
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_PlanOfCare', params);

                        break;

                    case 'FunctionalAndCognitive':
                    case 'FuntionalAndCognitive':
                    case 'FunctionalandCognitive':
                        var Clinical_CognitiveIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_functionalandcognitive').parent().parent().find('section[id*="Cli_Cognitive_Main"]').map(function () {
                            return this.id.replace("Cli_Cognitive_Main", "");
                        }).get().join(',');
                        if (Clinical_CognitiveIds != null && Clinical_CognitiveIds != '') {
                            params["CognitiveId"] = Clinical_CognitiveIds;
                            params["mode"] = "Edit";
                        } else {
                            params["CognitiveId"] = "-1";
                        }
                        params["IsFromNote"] = Clinical_ProgressNote.params["IsFromPanel"] == true ? false : true;
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_Cognitive', params);

                        break;

                    case 'FollowUp':
                        {
                            var IsTCM = false;
                            if ($("#PatientProfile #hfDischargeDate").val() != "" && $("#PatientProfile #hfDischargeDate").val() != null && $('#' + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #lblNotesReason").text().toLowerCase() == "transitional care management" && Clinical_ProgressNote.params.IsPhoneEncounter == true && Clinical_ProgressNote.params.TemplateName.toLowerCase() == "phone encounter tcm") {
                                IsTCM = true;
                            } else {
                                IsTCM = false;
                            }

                            if (IsTCM) {
                                var FollowUpAppointmentIds = '';
                                if ($("#pnlClinicalProgressNote #ProgressnoteHTML ul li").find('section[id*="FollowUpAppointment"]').attr('id')) {
                                    FollowUpAppointmentIds = $("#pnlClinicalProgressNote #ProgressnoteHTML ul li").find('section').attr('id').split('_')[1];
                                }
                                if (FollowUpAppointmentIds != null && FollowUpAppointmentIds != '') {
                                    params["FollowUpAppointmentId"] = FollowUpAppointmentIds;
                                    params["mode"] = "Edit";
                                } else {
                                    params["FollowUpAppointmentId"] = "-1";
                                }
                                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                                params["CurrentNotesFacilityId"] = Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"];
                                params["CurrentNotesVisitDate"] = Clinical_ProgressNote.params["VisitDateForFollowUp"];
                                LoadActionPan('Clinical_FollowUpTCM', params);
                            }
                            else {
                                params["mode"] = "Edit";
                                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                                params["CurrentNotesFacilityId"] = Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"];
                                params["AppointmentTimeFrom"] = Clinical_ProgressNote.AppointmentTimeFrom;
                                params["AppointmentTimeTo"] = Clinical_ProgressNote.AppointmentTimeTo;
                                LoadActionPan('Clinical_FollowUpAppointment', params);
                            }

                        }
                        break;

                    case 'Complaints':
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["CompliantId"] = "-1";
                        LoadActionPan('Clinical_Complaints', params);

                        break;

                    case 'ReviewofSystems':
                        //var Clinical_ReviewofSystemsIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Physicalhx').parent().parent().find('section[id*="Cli_ReviewofSystems_Main"]').map(function () {
                        //    return this.id.replace("Cli_ReviewofSystems_Main", "");
                        //}).get().join(',');
                        //if (Clinical_ReviewofSystemsIds != null && Clinical_ReviewofSystemsIds != '') {
                        //    params["ROSSystemInfoID"] = Clinical_ReviewofSystemsIds;
                        //    params["mode"] = "Edit";
                        //} else {
                        //    params["ROSSystemInfoID"] = "-1";
                        //}
                        //params['isShowTemplate'] = ComponentId == null ? true : false;
                        //LoadActionPan('Clinical_ReviewofSystems', params);

                        //break;
                        // Added by Zia Mehmood
                    case 'ReviewofSystemsRevmap':
                        var Clinical_ReviewofSystemsIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Physicalhx').parent().parent().find('section[id*="Cli_ReviewofSystems_Main"]').map(function () {
                            return this.id.replace("Cli_ReviewofSystems_Main", "");
                        }).get().join(',');
                        if (Clinical_ReviewofSystemsIds != null && Clinical_ReviewofSystemsIds != '') {
                            params["ROSSystemInfoID"] = Clinical_ReviewofSystemsIds;
                            params["mode"] = "Edit";
                        } else {
                            params["ROSSystemInfoID"] = "-1";
                        }
                        params['isShowTemplate'] = ComponentId == null || ComponentId == 'undefined' || ComponentId == -1 || ComponentId == 'clinicalMenu_ReviewofSystems' ? true : false;
                        params['ROSTemplateId'] = ComponentId == null || ComponentId == 'undefined' || ComponentId == -1 || ComponentId == 'clinicalMenu_ReviewofSystems' ? 0 : ComponentId;
                        params['LoadFromNote'] = customFormName;
                        params["PanelID"] = "pnlClinicalROSTemplateDetailRevamp";
                        LoadActionPan('Clinical_ROSTemplateDetailRevamp', params);

                        break;

                    case 'ConsultationOrder':
                        params["ConsultationOrderId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_ConsultationOrder', params);

                        break;

                    case 'RadiologyOrder':
                    case 'DiagnosticImagingOrder':
                        //-------
                        if (!Clinical_ProgressNote.params.DefaultProblemsCount) {
                            Clinical_ProgressNote.params.DefaultProblemsCount = 0;
                        }

                        if ($("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").length > Clinical_ProgressNote.params.DefaultProblemsCount) {
                            params["isProblemAdded"] = true;
                        } else {
                            params["isProblemAdded"] = false;
                        }
                        //-------
                        params["RadiologyOrderId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["Type"] = "Order";
                        LoadActionPan('Clinical_RadiologyOrder', params);

                        break;

                    case 'LabOrders':
                    case 'LabOrder':

                        //-------
                        if (!Clinical_ProgressNote.params.DefaultProblemsCount) {
                            Clinical_ProgressNote.params.DefaultProblemsCount = 0;
                        }

                        if ($("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").length > Clinical_ProgressNote.params.DefaultProblemsCount) {
                            params["isProblemAdded"] = true;
                        } else {
                            params["isProblemAdded"] = false;
                        }
                        //-------

                        params["LabOrderId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["Type"] = "Order";
                        LoadActionPan('Clinical_LabOrder', params);

                        break;

                    case 'LabResults':
                        params["LabOrderId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["Type"] = "Result";
                        LoadActionPan('Clinical_LabOrder', params);

                        break;

                    case 'RadiologyResults':
                    case 'DiagnosticImagingResults':
                        params["RadiologyOrderId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["Type"] = "Result";
                        LoadActionPan('Clinical_RadiologyOrder', params);

                        break;

                    case 'ProcedureOrder':
                        params["ProcedureOrderId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_ProcedureOrder', params);

                        break;


                    case 'Procedures':
                        var Clinical_ProcedureIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedures').parent().parent().find('section[id*="Cli_Procedures_Main"]').map(function () {
                            return this.id.replace("Cli_Procedures_Main", "");
                        }).get().join(',');
                        if (Clinical_ProcedureIds != null && Clinical_ProcedureIds != '') {
                            params["ProcedureId"] = Clinical_ProcedureIds;
                            params["mode"] = "Edit";
                        } else {
                            params["ProcedureOrderId"] = "-1";
                        }
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_Procedures', params);
                        break;

                    case 'PatientEducation':
                        params["PatientEducationId"] = "-1";
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_PatientEducation', params);

                        break;

                    case 'BillingInformation':
                        params["VisitId"] = Clinical_ProgressNote.params.VisitId;
                        params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
                        params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
                        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["NoteStatus"] = $('#pnlClinicalProgressNote #hfNoteStatus').val();
                        params["FacilityId"] = $('#pnlClinicalProgressNote #hfFacilityId').val();
                        params["RefProviderId"] = $('#pnlClinicalProgressNote #hfRefProvider').val();
                        var BillingInfoId = parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val());

                        if ($('#' + Clinical_ProgressNote.params.PanelID + ' #ChkBox_LinkedAppointment').prop("checked") || BillingInfoId > 0) {
                            params["BillingInfoId"] = BillingInfoId;
                            params["AppointmentDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #txtLinkedAppointment').val();

                            LoadActionPan("BillingInformation", params);
                        }
                        else {
                            LoadActionPan("VisitTypeSelection", params);
                        }

                        break;

                    case 'History':
                        params["TabToSelected"] = "HistorySummary";
                        params["HxtabOrder"] = Clinical_ProgressNote.params["HxtabOrder"];
                        if (Clinical_ProgressNote.params["HxtabOrder"] != '') {
                            var tabArray = Clinical_ProgressNote.params["HxtabOrder"].split(',');
                            var TabToSelected = tabArray[tabArray.length - 1]
                            params["TabToSelected"] = TabToSelected;
                        }
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;

                    case 'CustomFormPreview':
                        params["CustomFormId"] = ComponentId;
                        params["CustomFormName"] = decodeURIComponent(customFormName);
                        params["CustomFormNameForDoc"] = customFormNameForDoc.indexOf('%') > 0 ? decodeURIComponent(customFormNameForDoc) : customFormNameForDoc;
                        params["CustomFormUniqueId"] = customFormUniqueId;
                        params["IsAddToNote"] = true;
                        LoadActionPan("Clinical_CustomFormsPreview", params);
                        break;
                    case 'CustomForms':
                        params["IsSelectForLookUp"] = true;
                        LoadActionPan("Clinical_CustomForms", params);
                        break;
                    case 'OrderSets':
                        LoadActionPan("Clinical_OrderSets", params);
                        break;
                    case 'VBP_PHQQuestionnaire':
                        params["VisitId"] = Clinical_ProgressNote.params.VisitId;
                        params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
                        params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
                        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan("VBP_PHQQuestionnaire", params);
                        break;
                    case 'VBP_PHQ2Questionnaire':
                        params["VisitId"] = Clinical_ProgressNote.params.VisitId;
                        params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
                        params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
                        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan("VBP_PHQ2Questionnaire", params);
                        break;

                    case 'HPIComplaints':
                        var Clinical_HPITemplateIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().find('section[id*="Cli_HPIComplaints_Temp"]').map(function () {
                            return this.id.replace("Cli_HPIComplaints_Temp", "");
                        }).get().join(',');
                        if (Clinical_HPITemplateIds != null && Clinical_HPITemplateIds != '') {
                            params["HPITemplateId"] = Clinical_HPITemplateIds;
                            params["mode"] = "Edit";
                        } else {
                            params["HPITemplateId"] = "-1";
                        }
                        params["FromAdmin"] = 0;
                        params["ParentCtrl"] = 'clinicalTabProgressNote';
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        LoadActionPan('Clinical_HPIComplaints', params);
                        break;
                    case 'Images':
                        Clinical_ProgressNote.NoteAttachment();
                        break;
                    case 'SocPsyandBehaviorHx':
                        var Clinical_SocPsyandBehaviorHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_SocPsyandBehaviorHx').parent().parent().find('section[id*="Cli_SocPsyandBehaviorHx_Main"]').map(function () {
                            return this.id.replace("Cli_SocPsyandBehaviorHx_Main", "");
                        }).get().join(',');
                        if (Clinical_SocPsyandBehaviorHxIds != null && Clinical_SocPsyandBehaviorHxIds != '') {
                            params["SocPsyandBehaviorHxId"] = Clinical_SocPsyandBehaviorHxIds;
                            params["mode"] = "Edit";
                        } else {
                            params["SocPsyandBehaviorHxId"] = "-1";
                        }

                        params["TabToSelected"] = "SocPsyandBehaviorHx";
                        LoadActionPan('Clinical_HistorySummary', params);

                        break;
                    case 'ImplantableDevices':
                        params["ImplantableDevicesPKId"] = "-1";
                        LoadActionPan('Clinical_Implantable', params);

                        break;
                    case 'Letter':
                    case 'PatientLetterPreview':
                        Clinical_ProgressNote.createLetter();
                        break;
                    case 'CarePlan':
                        params["CarePlanId"] = "-1";
                        LoadActionPan('Clinical_CarePlan', params);
                        break;
                    case 'Treatment':

                        var Clinical_TreatmentIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Immunization').parent().parent().find('section[id*="Cli_Treatment_Main"]').map(function () {
                            return this.id.replace("Cli_Treatment_Main", "");
                        }).get().join(',');
                        if (Clinical_TreatmentIds != null && Clinical_TreatmentIds != '') {
                            params["TreatmentIds"] = Clinical_TreatmentIds;
                            params["mode"] = "Edit";
                        } else {
                            params["TreatmentIds"] = "-1";
                        }
                        //Ast 357
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["ParentPanelID"] = Clinical_ProgressNote.params.PanelID;
                        LoadActionPan('Clinical_Treatment', params);

                        break;
                    default:
                        return false;
                }
                //findInDiv.hide(true);
            }
        });


    },

    LoadNotesComponentInEditMode: function (NoteComponent, NCId) {
        Clinical_ProgressNote.DetachedNoteComponentIds = [];
        Clinical_ProgressNote.AttachedNoteComponentIds = [];

        switch (NoteComponent) {
            case 'Vitals':
                var params = [];
                params["VitalSignId"] = NCId;
                params["NoteVitalSignId"] = params["VitalSignId"];
                params["mode"] = "Edit";
                params["RefCtrl"] = "hfVitalsId";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["FromAdmin"] = "0";
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_Vitals', params);

                break;

            case 'ProblemList':
                var params = [];
                params["ProblemListId"] = NCId;
                params["FromAdmin"] = "0";
                params["mode"] = "Edit";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["IsFromNote"] = true;
                if (Clinical_ProblemLists.bIsFirstLoad == false) {
                    $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                    RemoveAdminTab('clinicalTabProblemLists');
                }
                LoadActionPan('Clinical_ProblemLists', params);

                break;

            case 'Allergies':
                var params = [];
                params["AllergyId"] = NCId;
                params["FromAdmin"] = "0";
                params["mode"] = "Edit";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";

                LoadActionPan('Clinical_Allergies', params);

                break;

            case 'SocialHx':
                var params = [];
                params["SocialHxId"] = "-1";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["mode"] = "Add";
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_SocialHx', params);

                break;

            case 'BirthHx':
                var params = [];
                params["BirthHxId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_BirthHx', params);

                break;
            case 'SocPsyandBehaviorHx':
                var params = [];
                params["SocPsyandBehaviorHxId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_SocPsyandBehaviorHx', params);

                break;
            case 'Immunization':
                var params = [];
                params["ImmunizationId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_Immunization', params);

                break;

            case 'Referrals':
                var params = [];
                params["ReferralId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Patient_Referrals', params);

                break;

            case 'MedicalHx':
                var params = [];
                params["MedicalHxId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_MedicalHx', params);

                break;

            case 'Medications':
                var params = [];
                params["MedicationsId"] = "-1";
                params["MedicationsTab"] = "Medications";
                params["PrescriptionId"] = "-1";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["mode"] = "Add";
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_Medications', params);

                break;

            case 'Prescription':
                var params = [];
                params["MedicationsId"] = "-1";
                params["MedicationsTab"] = "Prescription";
                params["PrescriptionId"] = "-1";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["mode"] = "Add";
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_Medications', params);

                break;

            case 'FamilyHx':
                var params = [];
                params["FamilyHxId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_FamilyHx', params);

                break;

            case 'SurgicalHx':
                var params = [];
                params["SurgicalHxId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_SurgicalHx', params);

                break;

            case 'HospitalizationHx':
                var params = [];
                params["HospitalizationHxId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_HospitalizationHx', params);

                break;

            case 'ReviewofSystems':
                var params = [];
                params["ROSSystemInfoID"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + '  #ReviewofSystemsRevmapHeaderFromSOAP').length > 0) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + '  #ReviewofSystemsRevmapHeaderFromSOAP').trigger('onclick');
                }
                else {
                    LoadActionPan('Clinical_ReviewofSystems', params);
                }

                break;

            case 'PhysicalExam':
                //var params = [];
                //params["PhysicalExamTemplateId"]= "-1";
                //params["mode"] = "View";
                //params["FromAdmin"] = "0";
                //params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                //params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                //params["patientID"] = Clinical_ProgressNote.params.patientID;
                //params["ParentCtrl"] = "clinicalTabProgressNote";
                //LoadActionPan('Clinical_PhysicalExam', params);

                var params = [];
                if (PhysicalExamTemplateId && parseInt(PhysicalExamTemplateId) > 0) {
                    params["PhysicalExamTemplateId"] = PhysicalExamTemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["PhysicalExamTemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                LoadActionPan('PhysicalExamTemplateDetailRevamp', params);

                break;

            case 'PlanOfCare':
                var params = [];
                params["PlanOfCareId"] = NCId;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_PlanOfCare', params);
                break;
            case 'Conitive':
                var params = [];
                params["ConitiveId"] = NCId;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_Conitive', params);

                break;

            case 'ConsultationOrder':
                var params = [];
                params["ConsultationOrderId"] = -1;
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NoteId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";

                LoadActionPan('Clinical_ConsultationOrder', params);

                break;

            case 'RadiologyOrder':
                var params = [];
                params["RadiologyOrderId"] = -1;
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["Type"] = "Order";
                params["NoteId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";

                LoadActionPan('Clinical_RadiologyOrder', params);

                break;

            case 'LabOrders':
                var params = [];
                params["LabOrderId"] = -1;
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["Type"] = "Order";
                params["NoteId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_LabOrder', params);

                break;

            case 'LabResults':
                var params = [];
                params["LabResultId"] = -1;
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["Type"] = "Result";
                params["NoteId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_LabOrder', params);

                break;

            case 'RadiologyResults':
                var params = [];
                params["RadiologyResultId"] = -1;
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["Type"] = "Result";
                params["NoteId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_RadiologyOrder', params);

                break;

            case 'ProcedureOrder':
                var params = [];
                params["ProcedureOrderId"] = NCId;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["NoteId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('ClinicalProcedureOrderDetail', params);

                break;

            case 'FollowUp':
                {
                    var IsTCM = false;
                    if ($("#PatientProfile #hfDischargeDate").val() != "" && $("#PatientProfile #hfDischargeDate").val() != null && $('#' + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #lblNotesReason").text().toLowerCase() == "transitional care management" && Clinical_ProgressNote.params.IsPhoneEncounter == true && Clinical_ProgressNote.params.TemplateName.toLowerCase() == "phone encounter tcm") {
                        IsTCM = true;
                    } else {
                        IsTCM = false;
                    }
                    if (IsTCM) {
                        var params = [];
                        params["FollowUpId"] = "-1";
                        params["mode"] = "Add";
                        params["FromAdmin"] = "0";
                        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["patientID"] = Clinical_ProgressNote.params.patientID;
                        params["ParentCtrl"] = "clinicalTabProgressNote";
                        LoadActionPan('Clinical_FollowUpTCM', params);
                    }
                    else {
                        var params = [];
                        params["FollowUpId"] = "-1";
                        params["mode"] = "Add";
                        params["FromAdmin"] = "0";
                        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                        params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["ParentCtrl"] = "clinicalTabProgressNote";
                        LoadActionPan('Clinical_FollowUpAppointment', params);
                    }
                }

                break;

            case 'Procedures':
                var params = [];
                params["FollowUpId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["CurrentNotesProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                params["patientID"] = Clinical_ProgressNote.params.patientID;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Clinical_Procedures', params);

                break;

            default:
                return false;
        }
        // findInDiv.hide(true);
    },

    ProgressnoteHtmlHoverEvents: function () {
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML li:not(.initialVisitBody):not(.sopTextEditable):has(>header)').addClass('initialVisitBody');
    },

    //This Functions Removed the binded vital to Progress note
    RemoveComponentAttached: function (ComponentId) {
        if (ComponentId.indexOf("Vitals") >= 0) {

            EMRUtility.scrollToPNcomponent('clinical_vitals');
            Clinical_Vitals.DetachVitalSignFromNotes(ComponentId);
        } else if (ComponentId.indexOf("Problems") >= 0) {

            Clinical_ProblemLists.detachProblemListFromNotes(ComponentId).done(function () {
                Clinical_ProgressNote.EnableDisableCancerReportButton();
            });

        } else if (ComponentId.indexOf("Allergies") >= 0) {
            Clinical_Allergies.detachAllergyFromNotes(ComponentId);
        } else if (ComponentId.indexOf("SocialHx") >= 0) {
            Clinical_SocialHx.detachSocialHxFromNotes(ComponentId);
        } else if (ComponentId.indexOf("BirthHx") >= 0) {
            Clinical_BirthHx.detachBirthHxFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("Immunization") >= 0) {
            Clinical_ImmunizationDetail.detachImmunizationFromNotes(ComponentId);
        } else if (ComponentId.indexOf("Referrals") >= 0) {
            Patient_Referrals.detachReferralFromNotes(ComponentId);
        } else if (ComponentId.indexOf("MedicalHx") >= 0) {
            Clinical_MedicalHx.detachMedicalHxFromNotes(ComponentId);
        } else if (ComponentId.indexOf("Prescription") >= 0) {
            Clinical_Prescriptions.detachPrescriptionFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("FamilyHx") >= 0) {
            Clinical_FamilyHx.detachFamilyHxFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("SurgicalHx") >= 0) {
            Clinical_SurgicalHx.detachSurgicalHxFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("HospitalizationHx") >= 0) {
            Clinical_HospitalizationHx.detachHospitalizationHxFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("PhysicalExam") >= 0) {
            PhysicalExamTemplatesRevamp.detachPhysicalExamFromNotes();
            //Clinical_PhysicalExam.detachPhysicalExamFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("PlanOfCare") >= 0) {
            Clinical_PlanOfCare.detachPlanOfCareFromNotes(ComponentId);
        }

        else if (ComponentId.indexOf("Cognitive") >= 0) {
            Clinical_Cognitive.detachCognitiveFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("ConsultationOrder") >= 0) {
            ClinicalConsultationOrderDetail.detachConsultationOrderFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("RadiologyOrder") >= 0) {
            ClinicalRadiologyOrderDetail.detachRadiologyOrderFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("LabOrders") >= 0) {
            ClinicalLabOrderDetail.detachLabOrderFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("LabOrder") >= 0) {
            ClinicalLabOrderDetail.detachLabOrderFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("Letter") >= 0) {
            Create_Letter.detachPatLetterFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("LabResults") >= 0) {
            Clinical_LabOrder.detachLabResultFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("LabResult") >= 0) {
            Clinical_LabOrder.detachLabResultFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("RadiologyResults") >= 0) {
            Clinical_RadiologyOrder.detachRadiologyResultFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("RadiologyResult") >= 0) {
            Clinical_RadiologyOrder.detachRadiologyResultFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("OrderSets") >= 0) {
            Clinical_OrderSetDetails.detachOrderSetFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("Cli_Procedures_Main") >= 0) {
            Clinical_Procedures.detachProceduresFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("ProcedureOrder") >= 0) {
            ClinicalProcedureOrderDetail.detachProcedureOrderFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("FollowUp") >= 0) {
            EMRUtility.scrollToPNcomponent('clinical_followup');
            //   Clinical_PhysicalExam.detachPhysicalExamFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("Medications") >= 0) {
            Clinical_Medications.detachMedicationFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("ReviewofSystems") >= 0) {
            Clinical_ReviewofSystems.detachReviewofSystemsFromNotes(ComponentId);
        } else if (ComponentId.indexOf("Complaint") >= 0) {
            Clinical_Complaints.detachComplaintFromNotes(ComponentId);
        } else if (ComponentId.indexOf("PatientEducation") >= 0) {
            var patienteduid = ComponentId.match(/[\d\.]+/g).toString()
            Clinical_PatientEducation.detachPatientEducationFromNotes(patienteduid, "1");
        }
        else if (ComponentId.indexOf("Images") >= 0) {
            Clinical_ProgressNote.detachImagesFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("SocPsyandBehaviorHx") >= 0) {
            Clinical_SocPsyandBehaviorHx.detachSocPsyandBehaviorHxFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("ImplantableDevices") >= 0) {
            Clinical_Implantable.detachImplantableDeviceFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("CarePlan") >= 0) {
            Clinical_CarePlan.detachCarePlanFromNotes(ComponentId);
        }
        else if (ComponentId.indexOf("Treatment") >= 0) {
            Clinical_Treatment.detachTreatment(ComponentId);
        }
        // findInDiv.hide(true);
    },


    //This Functions edited the binded vital to Progress note and Add HTML to tiny mce and shows tinymce
    EditNotesComments_ComponentAttached: function (Control, event) {
        if (event != null)
            event.stopPropagation();
        if ($(Control).attr('id').indexOf('Cli_Images') >= 0) {
            var PatDocID = $(Control).attr('id').replace('Cli_Images_', '');
            var params = [];
            params["PatientID"] = Clinical_ProgressNote.params.patientID;
            params["PatDocID"] = PatDocID;
            params["NotesId"] = Clinical_ProgressNote.params.NotesId;
            //params["FolderID"] = $("#" + Patient_Document.params["PanelID"] + " #lstDocument li.active").val();
            params["mode"] = "Edit";
            params["RefForm"] = "frmClinicalProgressNote";
            params["FromAdmin"] = Clinical_ProgressNote.params["FromAdmin"];
            params["ParentCtrl"] = "clinicalTabProgressNote";
            params["PatientDetail"] = "1";

            LoadActionPan('Patient_Document_Image_Annotation', params);
        }

        else {

            if ($(Control).find('#Comments_' + $(Control).attr('id')).length == 0) {
                if ($(Control).attr('id').indexOf('Cli_Treatment') >= 0) {
                    $(Control).append('<li id="Comments_' + $(Control).attr("id") + '" class="mt-md" style="word-wrap:break-word"></li>')
                }
                else {
                    $(Control).find('ul').append('<li id="Comments_' + $(Control).attr("id") + '" class="mt-md" style="word-wrap:break-word"></li>')
                }
            }

            if ($(Control).closest('ul').parent().find('textarea').length <= 0) {
                $('#Comments_' + $(Control).attr('id')).hide();
                Clinical_ProgressNote.openTinyEditor(Control, false);
            } else {
                $('#Comments_' + $(Control).attr('id')).hide();
                Clinical_ProgressNote.openTinyEditor(Control, false);
            }
        }
    },
    updateTextAreaComponent: function (Name, Cntrl) {
        var objDeffered = $.Deferred();
        $("#Comments_" + Name).show();
        if (Cntrl.length > 0) {
            $("#Comments_" + Name).html(Cntrl[0].value);
        } else {
            $("#Comments_" + Name).html(Cntrl.value);
        }

        Clinical_ProgressNote.RemoveSection(Cntrl);
        $(Cntrl).remove();
        objDeffered.resolve();
        return objDeffered;
    },
    Edit_ComponentAttached: function (Control, Name, event) {
        if (event != null)
            event.stopPropagation();

        Clinical_ProgressNote.IsNoteComponentAvaliable(false, Name).done(function (res) {
            if (res == true) {

                if ($('.myTextEditorNotes').length > 0) {
                    $.when(Clinical_ProgressNote.updateProgressNoteHTML()).then(function () {
                        $Control = $(Control).closest('li');
                        var Name = Name || $Control.find('.NotesComponent').attr('title');

                        if (Name && Name != "")
                            Name = Name.replace(/\s/g, '');


                        if (Name == "Prescriptions") {
                            Name = "Prescription";
                        }
                        if (Name == "DiagnosticImagingOrder") {
                            Name = "RadiologyOrder";
                        }
                        if ($Control.closest('ul').parent().find('#' + Name).length == 0) {
                            $Control.find('header').after('<section id=' + Name + '><div id=' + Name + '><ul class="list-unstyled"><li id= Comments_' + Name + ' class="mt-md" style="word-wrap:break-word"></li> </ul></div></section>');
                        }

                        if ($Control.closest('ul').parent().find('textarea').length <= 0) {
                            $('#Comments_' + Name).hide();
                            Clinical_ProgressNote.openTinyEditor($Control, true, Name);

                        } else {
                            $('#Comments_' + Name).hide();
                            Clinical_ProgressNote.openTinyEditor($Control, true, Name);
                        }

                    });
                }
                else {
                    $Control = $(Control).closest('li');
                    var Name = Name || $Control.find('.NotesComponent').attr('title');

                    if (Name && Name != "")
                        Name = Name.replace(/\s/g, '');

                    if (Name == "Prescriptions") {
                        Name = "Prescription";
                    }
                    if (Name == "Social,PsychologicalandBehaviorHx") {
                        Name = "SocPsyandBehaviorHx";
                    }
                    if (Name == "DiagnosticImagingOrder") {
                        Name = "RadiologyOrder";
                    }
                    if ($Control.closest('ul').parent().find('#' + Name).length == 0) {
                        $Control.find('header').after('<section id=' + Name + '><div id=' + Name + '><ul class="list-unstyled"><li id= Comments_' + Name + ' class="mt-md" style="word-wrap:break-word"></li> </ul></div></section>');
                    }

                    if ($Control.closest('ul').parent().find('textarea').length <= 0) {
                        $('#Comments_' + Name).hide();
                        Clinical_ProgressNote.openTinyEditor($Control, true, Name);

                    } else {
                        $('#Comments_' + Name).hide();
                        Clinical_ProgressNote.openTinyEditor($Control, true, Name);
                    }

                }
            }
        });


    },

    RemoveSection: function (Control) {
        var ParentDivID = $(Control).parent().attr('id');

        var compName = $(Control).attr('name');
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #' + ParentDivID + ' #Comments_' + compName).text().replace(/\s/g, '') == "") {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #' + ParentDivID).find('section[id=' + compName + ']').remove();
        }


    },

    // Getting Progress Note Right Side Bar List, and attaching draggable object(plugin)
    Fill_NotesComponent: function () {

        Clinical_ProgressNote.getPrivileges().done(function (response) {
            if (response.status != false) {
                var jobj = JSON.parse(response);
                var obj = JSON.parse(jobj.responsePrivilages_JSON);
                if (Clinical_ProgressNote.IsNewNote) {
                    var complaintsLi = '<li  id="clinicalMenu_Medical_Complaints" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Complaints\');" title="Complaints">Complaints</a></li>';
                    var hpiComplaintsLi = '<li  id="clinicalMenu_Medical_HPIComplaints" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'HPIComplaints\');" title="Complaints">Complaints</a></li>';

                    var selectedComplaintsLI = (globalAppdata["IsDefaultHPI"] == "False" || globalAppdata["IsDefaultHPI"] == "0") ? complaintsLi : hpiComplaintsLi;
                    Clinical_ProgressNote.noteComponentsFill(obj, selectedComplaintsLI);
                }
                else {
                    Clinical_ProgressNote.checkComplaintType().done(function (cResponse) {
                        cResponse = JSON.parse(cResponse);
                        if (cResponse.status != false) {
                            var isHPIComplaint = cResponse.isHPIComplaint == 0 ? false : true;

                            var complaintsLi = '<li  id="clinicalMenu_Medical_Complaints" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Complaints\');" title="Complaints">Complaints</a></li>';
                            var hpiComplaintsLi = '<li  id="clinicalMenu_Medical_HPIComplaints" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'HPIComplaints\');" title="Complaints">Complaints</a></li>';

                            var selectedComplaintsLI = isHPIComplaint == false ? complaintsLi : hpiComplaintsLi;
                            Clinical_ProgressNote.noteComponentsFill(obj, selectedComplaintsLI);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
        });

    },

    noteComponentsFill: function (obj, selectedComplaintsLI) {

        $('#pnlClinicalProgressNote #NotesComponentList').html('<ul class="nav nav-main nav-small" id="NotesComponentList">'

               + '<li class="nav-parent " id="clinicalMenuSubjective" ><a data-toggle="tab" title="Subjective" href="#"><span class="bold">Subjective</span></a>'
                   + '<ul id="sortableSubjective" class="nav nav-children">'
                           + selectedComplaintsLI
                           + '<li  id="clinicalMenuHistroy" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'History\');" title="History">History</a></li>'
                           //+ '<li  id="clinicalMenu_ReviewofSystems" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'ReviewofSystems\');" title="Review of Systems">Review of Systems</a></li>'
                           + '<li  id="clinicalMenu_ReviewofSystems" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'ReviewofSystemsRevmap\');" title="Review of Systems">Review of Systems</a></li>'
                           + '<li  id="clinicalMenu_Medical_Allergies"  style="position: relative;"><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Allergies\');" title="Allergies">Allergies</a></li>'
                            + '<li  id="clinicalMenu_Medical_Medications" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Medications\');" title="Medications">Medications</a></li>'
                   + '</ul>'
               + '</li>'
                   + '<li class="nav-parent " id="clinicalMenuObjective" ><a data-toggle="tab" title="Objective" href="#"><span class=" bold">Objective</span></a>'
                       + '<ul id="sortableObjective" class="nav nav-children">'
                           + '<li  id="clinicalMenu_Medical_Vitals"  style="position: relative;"><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Vitals\');" title="Vitals">Vitals</a></li>'
                           + '<li  id="clinicalMenu_PhysicalExam" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'PhysicalExam\',\'-1\');" title="Physical Exam">Physical Exam</a></li>'
                           + '<li class="nav-parent" id="clinicalMenuResults"><a data-toggle="tab" title="Results" href="#"><span>Results</span> </a>'
                               + '<ul id="sortableResults" class="nav nav-children">'
                                   + '<li  id="clinicalMenu_Results_Lab" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'LabResults\');" title="Lab Results">Lab Results</a></li>'
                                   + '<li  id="clinicalMenu_Results_Radiology" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyResults\');" title="Diagnostic Imaging Results">Diagnostic Imaging Results</a></li>'
                               + '</ul>'
                           + '</li>'
                       + '</ul>'
                   + '</li>'
               + '<li class="nav-parent " id="clinicalMenuAssessment" ><a data-toggle="tab" title="Assessment" href="#"><span class=" bold">Assessment</span></a>'
                   + '<ul id="sortableAssessment" class="nav nav-children">'
                        + '<li  id="clinicalMenu_Medical_Problems"  style="position: relative;"><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Problems\');" title="Problems">Problems</a></li>'
                   + '</ul>'
               + '</li>'
               + '<li class="nav-parent" id="clinicalMenuPlan" ><a data-toggle="tab" title="Plan" href="#"><span class=" bold">Plan</span></a>'
                   + '<ul id="sortablePlan" class="nav nav-children">'
                   + '<li  id="clinicalMenu_Medical_Treatment" ><a  href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Treatment\');" title="Treatment">Treatment</a></li>'
                         + '<li class="nav-parent" id="clinicalMenuOrders"><a data-toggle="tab" title="Orders" href="#"><span>Orders</span> </a>'
                               + '<ul id="sortableOrders" class="nav nav-children">'
                                   + '<li  id="clinicalMenu_Medical_Prescription" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Prescription\');" title="Prescription">Prescription</a></li>'
                                   + '<li  id="clinicalMenu_Orders_Lab" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'LabOrders\');" title="Lab Orders">Lab</a></li>'
                                   + (obj[0] != null && obj[0].privilegasMessage == "" ? '<li  id="clinicalMenu_Orders_Procedure" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'ProcedureOrder\');" title="Procedure Order">Procedure</a></li>' : "")
                                   + '<li  id="clinicalMenu_Orders_Radiology" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyOrder\');" title="Diagnostic Imaging">Diagnostic Imaging</a></li>'
                                   + (obj[1] != null && obj[1].privilegasMessage == "" ? '<li  id="clinicalMenu_Orders_ConsultationOrder" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'ConsultationOrder\');" title="Consultation Order">Consultation</a></li>' : "")
                               + '</ul>'
                           + '</li>'
                           + '<li  id="clinicalMenu_Specialities_Immunization" ><a  href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Immunization\');" title="Immunization">Immunization</a></li>'
                           + '<li  id="clinicalMenu_Miscellaneous_CustomForm" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'OrderSets\');" title="Order Sets">Order Sets</a></li>'
                           + '<li  id="clinicalMenu_Medical_Procedures"  style="position: relative;"><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Procedures\');" title="Procedures">Procedures</a></li>'
                           + '<li  id="clinicalMenu_Miscellaneous_Referrals" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Referrals\');" title="Referrals">Referrals</a></li>'
                           + '<li  id="clinicalMenu_Medical_PatientEducation"><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'PatientEducation\');" title="PatientEducation">Patient Education</a></li>'
                           + '<li  id="clinicalMenu_Miscellaneous_Followup" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'FollowUp\');" title="Follow Up">Follow Up</a></li>'
                           + '<li  id="clinicalMenu_Miscellaneous_CarePlan" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'CarePlan\');" title="Care Plan">Care Plan</a></li>'
                   + '</ul>'
               + '</li>'
               + '<li class="nav-parent " id="clinicalMenuMiscellaneous"><a data-toggle="tab" title="Miscellaneous" href="#"><span class=" bold">Miscellaneous</span> </a>'
                   + '<ul id="sortableMiscellaneous" class="nav nav-children">'
                       + '<li  id="clinicalMenu_Miscellaneous_NotesExtraInfo" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'NotesExtraInfo\');" title="Notes Extra Info">Notes Extra Info</a></li>'
                       + ((globalAppdata["isConsolidatedCDACreationPreformance"] && globalAppdata["isConsolidatedCDACreationPreformance"].toLowerCase() == "false") ? "" : '<li  id="clinicalMenu_Cognitive" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'FunctionalAndCognitive\');Clinical_ProgressNote.FromComponentPanel();" title="Functional And Cognitive">Functional And Cognitive</a></li>')
                       + '<li  id="clinicalMenu_Miscellaneous_CustomForm" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'CustomForms\');" title="Custom Forms">Custom Forms</a></li>'
                       + ((globalAppdata["isImplantableDevices"] && globalAppdata["isImplantableDevices"].toLowerCase() == "false") ? "" : '<li id="clinicalMenu_Miscellaneous_ImplantableDevices" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'Implantable Devices\');" title="Implantable Devices">Implantable Devices</a></li>')

                   + '</ul>'
               + '</li>'
               + '<li id="clinicalMenu_TransitionOfCare" ><a href="javascript:Clinical_ProgressNote.SelectNotesComponentTab(\'TransitionOfCare\');" title="Transition of Care"><span class=" bold">Transition of Care</span> </a></li>');

        Clinical_ProgressNote.LoadComponentMenu();

        if (Clinical_ProgressNote.params["TemplateName"] != '')
            Clinical_ProgressNote.InitializeProgressNoteComponent();
        else {
            Clinical_ProgressNote.InitializeSubjectiveDragable();
            Clinical_ProgressNote.InitializeObjectiveDragable();
            Clinical_ProgressNote.InitializeAssesmentDragable();
            Clinical_ProgressNote.InitializePlanDragable();
            Clinical_ProgressNote.InitializeMiscellaneousDragable();
        }
        if (globalAppdata["IsNoteCompExpanded"] && globalAppdata["IsNoteCompExpanded"].toLowerCase() == "true") {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #NotesComponentList").find('.nav-parent').addClass('nav-expanded');
        }
        //Initilize Tree structure for right side bar of Patient Notess


        Clinical_ProgressNote.setTagsWidth();
        Clinical_ProgressNote.ShowHideComponetsHeaders();
        $(".initialVisit").css('min-height', '20px');

        var ProgressNote = $('#' + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML")

        if ($(ProgressNote).find("h4.blue").length == $(ProgressNote).find("h4.blue.hidden").length) {
            $(ProgressNote).find("#SubjectiveNoteComponentList li.sopTextEditable,#ObjectiveNoteComponentList li.sopTextEditable,#AssessmentNoteComponentList li.sopTextEditable,#PlanNoteComponentList li.sopTextEditable,#MiscellaneousNoteComponentList li.sopTextEditable").attr('style', 'min-height: 3px !important');
            $(ProgressNote).find("#SubjectiveNoteComponentList,#ObjectiveNoteComponentList,#AssessmentNoteComponentList,#PlanNoteComponentList,#MiscellaneousNoteComponentList").css('min-height', '3px');
            var isExists = false;
            $.each($(ProgressNote).find("#ProgressNoteComponentList").children(), function () {
                if ($(this).hasClass('sopTextEditable') && $(this).hasClass('ui-sortable-handle') && $(this).hasClass('defaultli')) {
                    isExists = true;
                }
            });

            if (isExists == false) {
                $(ProgressNote).find("#ProgressNoteComponentList").append('<li class="sopTextEditable defaultli ui-sortable-handle placeholder-free-text" ></li>');
            } else {
                if ($('#pnlClinicalProgressNote #NoteTemplate option:selected').text().trim() != "- Blank -") {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #ProgressNoteComponentList li.sopTextEditable.ui-sortable-handle.defaultli").addClass('placeholder-free-text');
                }
            }
        }
        else {
            $(ProgressNote).find("#SubjectiveNoteComponentList li.sopTextEditable,#ObjectiveNoteComponentList li.sopTextEditable,#AssessmentNoteComponentList li.sopTextEditable,#PlanNoteComponentList li.sopTextEditable,#MiscellaneousNoteComponentList li.sopTextEditable").attr('style', 'min-height: 10px !important;');
            $(ProgressNote).find("#SubjectiveNoteComponentList,#ObjectiveNoteComponentList,#AssessmentNoteComponentList,#PlanNoteComponentList,#MiscellaneousNoteComponentList").css('padding', '0px 10px 10px;');
        }
        // findInDiv.hide(true);

    },
    FromComponentPanel: function () {

        Clinical_ProgressNote.params["IsFromPanel"] = true;
    },

    getPrivileges: function () {
        var DetailInfoObj = [];
        var objDetail = {
        };
        var FormName = [];
        var Permission = [];
        FormName.push("Orders and Results_Procedure");
        FormName.push("Orders and Results_Consultation");
        Permission.push("VIEW");
        Permission.push("VIEW");
        objDetail["FormName"] = FormName;
        objDetail["Permission"] = Permission;

        var data = JSON.stringify(objDetail);
        return MDVisionService.APIService(data, "FormPrivilege", "AppPrivileges");
    },

    setTagsWidth: function () {

        $('.TagInserted').each(function () {
            $(this).val($(this).val() + ' ');
            var widthc = $(this).textWidth();
            $(this).css('width', Number(widthc + 10) + 'px');
        });


    },

    //This functions removed Component from Progress Note HTML
    RemoveComponentTab: function (ComponentName, ComponentId, NotesId, CustomFormUniqueId, customFormNameForDoc, PETemplateId) {


        Clinical_ProgressNote.IsNoteComponentAvaliable(false, ComponentName).done(function (res) {
            if (res == true) {

                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        if (ComponentName.split(' ').join('') == "Complaints") {
                            utility.myConfirm('28', function () {
                                if (ComponentName.split(' ').join('') == "Complaints") {
                                    EMRUtility.scrollToPNcomponent('clinical_complaint');
                                    Clinical_Complaints.detach_ComponentsComplaints(ComponentName, true, true);
                                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                                }
                            }, function () {
                            },
                            '1'
                        );
                        }
                        else if (ComponentName.split(' ').join('') == "HPIComplaints") {
                            utility.myConfirm('28', function () {
                                EMRUtility.scrollToPNcomponent('clinical_complaint');
                                if (PETemplateId) {
                                    Clinical_HPIComplaints.detach_ComponentsComplaints(ComponentName, true, false, PETemplateId);
                                }
                                else {
                                    Clinical_HPIComplaints.detach_ComponentsComplaints(ComponentName, true, true, PETemplateId);
                                }
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                            }, function () {
                            },
                            '1'
                        );
                        }
                        else if (ComponentName.split(' ').join('') == "PhysicalExam") {
                            utility.myConfirm('29', function () {
                                EMRUtility.scrollToPNcomponent('clinical_physicalexam');
                                if (PETemplateId) {
                                    PhysicalExamTemplatesRevamp.detach_ComponentsPhysicalExam(ComponentName, true, false, PETemplateId);

                                } else {
                                    PhysicalExamTemplatesRevamp.detach_ComponentsPhysicalExam(ComponentName, true, true, PETemplateId);
                                }

                                //Clinical_PhysicalExam.detach_ComponentsPhysicalExam(ComponentName, true, true);
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                            }, function () {
                            },
                                '29'
                            );
                        }
                        else {
                            if (ComponentName.split(' ').join('') == "ReviewofSystems" || ComponentName.split(' ').join('') == "ReviewofSystemsRevmap") {
                                EMRUtility.scrollToPNcomponent('clinical_reviewofsystems');
                                if (ComponentName == "ReviewofSystemsRevmap") {
                                    Clinical_ROSTemplateDetailRevamp.ROSTemplateDeleteFromNote(ComponentName, true, true, ComponentId, NotesId)

                                } else {
                                    Clinical_ReviewofSystems.detach_ComponentsReviewofSystems(ComponentName, true, true);
                                }
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                            } else {
                                utility.myConfirm('1', function () {

                                    if (ComponentName.split(' ').join('') == "Medication") {
                                        ComponentName = "Medications"
                                        EMRUtility.scrollToPNcomponent('clinical_medications');
                                    }
                                    if (ComponentName == "Vitals") {
                                        EMRUtility.scrollToPNcomponent('clinical_vitals');
                                        Clinical_Vitals.detach_ComponentsVitals(ComponentName, true, true);
                                    } else if (ComponentName.split(' ').join('') == "Problems") {
                                        EMRUtility.scrollToPNcomponent('clinical_problems');
                                        Clinical_ProblemLists.detach_ComponentsProblemList(ComponentName, true, true);
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').addClass("disabled");
                                    }
                                    else if (ComponentName.split(' ').join('') == "OrderSets") {
                                        EMRUtility.scrollToPNcomponent('clinical_ordersets');
                                        Clinical_OrderSetDetails.detach_ComponentsOrderSet(ComponentName, true, true);
                                    }
                                    else if (ComponentName == "Allergies") {
                                        EMRUtility.scrollToPNcomponent('clinical_allergies');
                                        Clinical_Allergies.detach_ComponentsAllergy(ComponentName, true, true);
                                    } else if (ComponentName.split(' ').join('') == "SocialHx") {
                                        Clinical_SocialHx.detach_ComponentsSocialHx(ComponentName, true, true);
                                    } else if (ComponentName.split(' ').join('') == "BirthHx") {
                                        Clinical_BirthHx.detach_ComponentsBirthHx(ComponentName, true, true);
                                    } else if (ComponentName.split(' ').join('') == "NotesExtraInfo") {
                                        EMRUtility.scrollToPNcomponent('clinical_notesextrainfo');
                                        Clinical_NotesExtraInfo.detach_NotesExtraInfo(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "Immunization") {
                                        Clinical_ImmunizationDetail.detach_ComponentsImmunization(ComponentName, true, true);
                                        EMRUtility.scrollToPNcomponent('clinical_immunization');
                                    }
                                    else if (ComponentName.split(' ').join('') == "Referrals") {
                                        EMRUtility.scrollToPNcomponent('clinical_referrals');
                                        Patient_Referrals.detach_ComponentsReferral(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "Medications") {
                                        EMRUtility.scrollToPNcomponent('clinical_medications');
                                        Clinical_Medications.detachMedicationsComponent(ComponentName, true, true);
                                    }

                                    else if (ComponentName.split(' ').join('') == "Prescription") {
                                        EMRUtility.scrollToPNcomponent('clinical_prescription');
                                        Clinical_Prescriptions.detach_ComponentsPrescription(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "MedicalHx") {
                                        Clinical_MedicalHx.detach_ComponentsMedicalHx(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "ConsultationOrder") {
                                        EMRUtility.scrollToPNcomponent('clinical_consultationorder');
                                        ClinicalConsultationOrderDetail.detach_ComponentsConsultationOrder(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "RadiologyOrder" || ComponentName.split(' ').join('') == "DiagnosticImagingOrder") {
                                        EMRUtility.scrollToPNcomponent('clinical_radiologyorder');
                                        ClinicalRadiologyOrderDetail.detach_ComponentsRadiologyOrder(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "LabOrder") {
                                        EMRUtility.scrollToPNcomponent('clinical_laborder');
                                        ClinicalLabOrderDetail.detach_ComponentsLabOrder(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "LabOrders") {
                                        EMRUtility.scrollToPNcomponent('clinical_laborders');
                                        ClinicalLabOrderDetail.detach_ComponentsLabOrder(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "Letter") {
                                        Create_Letter.detach_ComponentsPatLetter(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "LabResults") {
                                        EMRUtility.scrollToPNcomponent('clinical_labresults');
                                        Clinical_LabOrder.detach_ComponentsLabResult(ComponentName, true, true);
                                    }

                                    else if (ComponentName.split(' ').join('') == "RadiologyResults" || ComponentName.split(' ').join('') == "DiagnosticImagingResults") {
                                        EMRUtility.scrollToPNcomponent('clinical_diagnosticimagingresults');
                                        Clinical_RadiologyOrder.detach_ComponentsRadiologyResult(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "ProcedureOrder") {
                                        EMRUtility.scrollToPNcomponent('clinical_procedureorder');
                                        ClinicalProcedureOrderDetail.detach_ComponentsProcedureOrder(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "FamilyHx") {
                                        Clinical_FamilyHx.detach_ComponentsFamilyHx(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "SurgicalHx") {
                                        Clinical_SurgicalHx.detach_ComponentsSurgicalHx(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "HospitalizationHx") {
                                        Clinical_HospitalizationHx.detach_ComponentsHospitalizationHx(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "FollowUp") {
                                        //EMRUtility.scrollToPNcomponent('clinical_followup');
                                        Clinical_FollowUpAppointment.detach_ComponentsFollowUp(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "Procedures") {
                                        EMRUtility.scrollToPNcomponent('clinical_procedures');
                                        Clinical_Procedures.detach_ComponentsProcedures(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "PlanOfCare") {
                                        EMRUtility.scrollToPNcomponent('clinical_planofcare');
                                        Clinical_PlanOfCare.detach_ComponentsPlanOfCare(ComponentName, true, true);
                                    }

                                    else if (ComponentName.split(' ').join('') == "FunctionalAndCognitive") {
                                        EMRUtility.scrollToPNcomponent('clinical_functionalandcognitive');
                                        Clinical_Cognitive.detach_ComponentsCognitive(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "PatientEducation") {
                                        EMRUtility.scrollToPNcomponent('clinical_patienteducation');
                                        Clinical_PatientEducation.detach_ComponentsPatientEducation(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "CustomForms") {
                                        EMRUtility.scrollToPNcomponent('clinical_customforms');
                                        Clinical_CustomFormsPreview.detachCustomForm(ComponentId, true, true, CustomFormUniqueId);
                                    }
                                    else if (ComponentName == "CustomForm") {
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' .CustomFormsComponent').length > 1) {
                                            EMRUtility.scrollToPNcomponent('clinical_customform');
                                            Clinical_CustomFormsPreview.detachCustomFormComponent(ComponentId, true, true, CustomFormUniqueId, customFormNameForDoc, false);
                                        }
                                        else {
                                            EMRUtility.scrollToPNcomponent('clinical_customform');
                                            Clinical_CustomFormsPreview.detachCustomFormComponent(ComponentId, true, true, CustomFormUniqueId, customFormNameForDoc, true);
                                        }
                                    }
                                    else if (ComponentName.split(' ').join('') == "History") {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #ProgressnoteHTML').find('#clinicalMenuHistroy').closest('li').remove();
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #ProgressnoteHTML').find('clinical_history').closest('li').remove();
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ActionsInitialOfficeVisit button[title=History]').remove();

                                    }
                                    else if (ComponentName.split(' ').join('') == "Images") {
                                        Clinical_ProgressNote.detach_ComponentImages(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "SocPsyandBehaviorHx") {
                                        Clinical_SocPsyandBehaviorHx.detach_ComponentsSocPsyandBehaviorHx(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "ImplantableDevices") {
                                        EMRUtility.scrollToPNcomponent('clinical_implantabledevices');
                                        Clinical_Implantable.detach_ComponentsImplantableDevices(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "CarePlan") {
                                        EMRUtility.scrollToPNcomponent('clinical_careplan');
                                        Clinical_CarePlan.detach_ComponentsCarePlan(ComponentName, true, true);
                                    }
                                    else if (ComponentName.split(' ').join('') == "Treatment") {
                                        EMRUtility.scrollToPNcomponent('clinical_treatment');
                                        Clinical_Treatment.detach_ComponentsTreatment(ComponentName, true, true);
                                    }
                                    else {
                                        EMRUtility.scrollToPNcomponent('clinical_' + ComponentName.split(' ').join('').toLowerCase());
                                        Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
                                    }
                                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                                }, function () {
                                },
                                    '1'
                                );
                            }
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                // findInDiv.hide(true);

            }
        });



    },

    //This Function Updating Progress Note HTML, and removing HTML of Components Other than vitals
    Detach_ComponentsOthers: function (ComponentName, IsUpdate) {
        //$('clinical_' + ComponentName.toLowerCase().split(' ').join('')).parent().parent().remove();
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .' + ComponentName.replace(/\s+/g, '') + 'Component').attr('NoteComponentId');
        if (IsUpdate) {
            $.when(Clinical_ProgressNote.saveComponentSOAPText(ComponentName, true)).then(function () {
                utility.DisplayMessages("Successfully Deleted", 1);
                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
            });
        }
        else {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('clinical_' + ComponentName.toLowerCase().split(' ').join('')).parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText(ComponentName, true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                utility.DisplayMessages("Successfully Deleted", 1);
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('clinical_' + ComponentName.toLowerCase().split(' ').join('')).parent().parent().remove();
                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                utility.DisplayMessages("Successfully Deleted", 1);
            });
        }
        if (ComponentName == "Order Sets") {
            Clinical_ProgressNote.saveComponentSOAPText('Order Sets', true);
        }

    },

    //Initilize Tree structure for right side bar of Patient Notes, called only from Fill_NotesComponent Func of Progress Note
    LoadComponentMenu: function () {
        (function ($) {

            'use strict';

            var $items = $('.nav-small li.nav-parent');

            function expand($li) {
                $li.children('ul.nav-children').slideDown('fast', function () {
                    $li.addClass('nav-expanded');
                    $(this).css('display', '');
                    ensureVisible($li);
                });
            }

            function collapse($li) {
                $li.children('ul.nav-children').slideUp('fast', function () {
                    $(this).css('display', '');
                    $li.removeClass('nav-expanded');
                });
            }

            function ensureVisible($li) {
                var scroller = $li.offsetParent();
                if (!scroller.get(0)) {
                    return false;
                }

                var top = $li.position().top;
                if (top < 0) {
                    scroller.animate({
                        scrollTop: scroller.scrollTop() + top
                    }, 'fast');
                }
            }

            $items.find('> a').on('click', function (ev) {

                var $anchor = $(this),
                    $prev = $anchor.closest('ul.nav').find('> li.nav-expanded'),
                    $next = $anchor.closest('li'),
                    $prevExpanded = $anchor.closest('li.nav-expanded');

                if ($anchor.prop('href')) {
                    var arrowWidth = parseInt(window.getComputedStyle($anchor.get(0), ':after').width, 10) || 0;
                    if (ev.offsetX > $anchor.get(0).offsetWidth - arrowWidth) {
                        ev.preventDefault();
                    }
                }

                if (globalAppdata["IsNoteCompExpanded"] && globalAppdata["IsNoteCompExpanded"].toLowerCase() == "true") {
                    if ($prevExpanded.get(0) !== $next.get(0)) {
                        collapse($prevExpanded);
                        expand($next);
                    } else {
                        collapse($prevExpanded);
                    }
                }
                else {
                    if ($prev.get(0) !== $next.get(0)) {
                        collapse($prev);
                        expand($next);
                    } else {
                        collapse($prev);
                    }
                }
            });


        }).apply(this, [jQuery]);
    },

    //Unloading the Progress Note HTML
    UnLoad: function () {
        Clinical_Notes.params.NoteId = '';
        Clinical_Notes.params.mode = 'Add';
        Clinical_Notes.CopyNotes = false;
        Clinical_Notes.PrevNoteId = 0;
        Clinical_Notes.NewInsertTables = "";
        Clinical_PhoneEncounter.CopyNotes = false;
        Clinical_PhoneEncounter.PrevNoteId = 0;
        Clinical_PhoneEncounter.NewInsertTables = "";
        Clinical_ProgressNote.DefaultOrderSetID = 0;
        $('#pnlClinicalProgressNote #ProgressnoteHTML').html("");
        RemoveAdminTab('Clinical_ProgressNote');

        if ($("#mstrDivNotes button#clinicalTabNotes").length > 0) {
            $("#mstrDivNotes button#clinicalTabNotes").trigger("click");
        }

        Clinical_ProgressNote.disconnectUserFromSignalR();

    },

    //This function is called by Delete btn of Progress Note. It will remove current Progress note and post back user to Notes Form
    NotesDelete: function () {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {

            var selectedValue = Clinical_ProgressNote.params.NotesId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Notes.NotesDeleted(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if ($("#PatientProfile #hfPatientId").val() == Clinical_ProgressNote.params.patientID) {
                            setPatientBanner(Clinical_ProgressNote.params.patientID);
                        }
                        Clinical_ProgressNote.UnLoad();
                        Clinical_Notes.NotesSearch(null);
                        setTimeout(function () {
                            $('body').removeClass('modal-open');
                        }, 200);
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        setTimeout(function () {
                            $('body').removeClass('modal-open');
                        }, 200);
                    }
                });
            }
        }, function () {
            $('body').removeClass('modal-open');
        },
                            '1'
                                );

    },
    BillingInformationLoad: function () {
        var objData = new Object();
        objData["VisitId"] = Clinical_ProgressNote.params.AppointmentVisitId;
        objData["commandType"] = "billing_information_select_by_visitid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },
    SignAfterCQM: function (BillingInformation, Obj, customSigMsg, isComponentSelect, BillingInfoResponse) {
        var dfd = $.Deferred();
        //Start//02-05-2016//Ahmad Raza//logic for CDS Alert

        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!(Clinical_ProgressNote.params.IsPhoneEncounter || Clinical_ProgressNote.params.IsOutOfOfficeVisit) && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
            dfd.resolve();
        }
        else {
            Clinical_ProgressNote.params.IsOutOfOfficeVisit = false;
            if (BillingInfoResponse && BillingInfoResponse.BillingInfoFill_JSON) {

                $.when(Clinical_ProgressNote.BillingInformationBind(BillingInfoResponse, BillingInformation, Obj, customSigMsg, isComponentSelect)).then(function () {
                    dfd.resolve();
                });

            } else {
                //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
                Clinical_ProgressNote.BillingInformationLoad().done(function (response) {
                    response = JSON.parse(response);
                    $.when(Clinical_ProgressNote.BillingInformationBind(response, BillingInformation, Obj, customSigMsg, isComponentSelect)).then(function () {
                        dfd.resolve();
                    });
                });
            }

        }
        // findInDiv.hide(true);
        return dfd;
    },
    BillingInformationBind: function (response, BillingInformation, Obj, customSigMsg, isComponentSelect) {
        var dfdFinal = $.Deferred();

        var billingDetail = null;

        var isESuperBillSigned = false;
        var isJustNoteSign = false;
        if (response.status != false) {
            billingDetail = JSON.parse(response.BillingInfoFill_JSON);
        }

        var signMessage = "eSuperbill will also be signed. Are you sure you want to sign the provider note?";
        if (BillingInformation != null && Obj != null && customSigMsg != null && customSigMsg != "") {
            signMessage = customSigMsg;
        }
        else if (BillingInformation != null && Obj != null) {
            signMessage = "The provider note will also be signed. Are you sure you want to sign eSuperbill?";
        }
        if (billingDetail != null && billingDetail.Status == "Signed") {
            isESuperBillSigned = true;
            Clinical_ProgressNote.params.isESuperBillSigned = true;
            customSigMsg == null || customSigMsg == '' ? signMessage = "Are you sure you want to sign the provider note?" : signMessage = customSigMsg;

        } else if (billingDetail.BillingInfoId > 0) {
            if (customSigMsg == null || customSigMsg == "") {

                if (billingDetail.NotesId == null || billingDetail.NotesId == "" || billingDetail.NotesId < 0) {
                    signMessage = "Are you sure you want to sign the provider note?";
                    isJustNoteSign = true;
                }

            } else {
                signMessage = customSigMsg;
            }
        }


        utility.myConfirm(signMessage, function () {
            $('#pnlClinicalProgressNote #hfNoteStatus').val('Signed');
            var deffered = $.Deferred();

            var self = $("#" + Clinical_Notes.params["PanelID"]);
            var prntctlIsNotScheduler = true;
            $('#pnlClinicalProgressNote #hfNoteText').val($('#pnlClinicalProgressNote #ProgressnoteHTML').html());
            var myJSON = self.getMyJSONByName();
            var NotesId = Clinical_Notes.params.NotesId;
            if (BillingInformation != null && Obj != null) {
                NotesId = Obj.NotesId;
                if (Obj.prntCtrl && (Obj.prntCtrl == "schTabCalendar" || Obj.prntCtrl == "schTabMultipleView"))
                    prntctlIsNotScheduler = false;
            }
            if (!prntctlIsNotScheduler) {
                DashBoard.NotesUpdate(NotesId).done(function (response) {
                    if (response.status != false) {
                        Clinical_Notes.SetModifiedNoteCount();
                    }
                });
            }
            if (parseInt(NotesId) > 0 && prntctlIsNotScheduler) {
                Clinical_Notes.NotesUpdate(myJSON, NotesId).done(function (response) {

                    response = JSON.parse(response);

                    if (response.status != false) {
                        Clinical_Notes.SetModifiedNoteCount();
                        if (isESuperBillSigned == true) {
                            utility.DisplayMessages("Successfully Signed!", 1);
                            if (BillingInformation == null) {
                                utility.DisplayMessages("An eSuperbill for this Date of Service has already been signed", 3);
                            }

                        }
                        cmd = [];
                        cmd.TabID = "Clinical_NotesView";
                        cmd.PanelID = "Clinical_NotesView";
                        cmd.MasterTabID = "";
                        cmd.ParentTabID = "";
                        cmd.ContainerControlID = "Clinical_NotesView";
                        cmd.Selected = false;
                        cmd.isActionPan = true;
                        cmd.Container = "";
                        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesView.html";
                        cmd.ActionPanContainer = "actionPanNotesView";

                        var paramsnotesprev = [];
                        paramsnotesprev["FromAdmin"] = "0";
                        paramsnotesprev["NotesId"] = NotesId;
                        paramsnotesprev["PatientId"] = Clinical_ProgressNote.params.patientID;
                        paramsnotesprev["RefSearch"] = "DraftSearch";
                        paramsnotesprev["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        paramsnotesprev["FromProgressnote"] = "1";

                        paramsnotesprev["ParentCtrl"] = 'clinicalTabNotes'; //Clinical_Notes

                        paramsnotesprev["ParentCtrlPanelID"] = "pnlClinicalNotes";
                        var Tab = GetTab(cmd.TabID);
                        var ClinicalTab = cmd;
                        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                            $('#pnlClinicalProgressNote #Caller').text(' ' + $('#pnlClinicalProgressNote #txtCaller').val());
                            $('#pnlClinicalProgressNote #Receiver').text(' ' + $('#pnlClinicalProgressNote #txtReceiver').val());
                            Clinical_ProgressNote.StopTaskTime();
                        }
                        var dfd = new $.Deferred();
                        var html = utility.getTabHtml(cmd.TabID);
                        if (html) {

                            dfd.resolve(html);
                            if (cmd.Container) {

                                eval(cmd.ContainerControlID + '.Load')(paramsnotesprev);
                            }
                            return dfd.promise();
                        } else {
                            $.get(GetTab(cmd.TabID).Path, {
                                cache: false
                            }, function (content) {
                                html = content;
                                eval(cmd.ContainerControlID + '.Load')(paramsnotesprev);
                                dfd.resolve(html);
                            });
                        }
                        dfd.then(function () {
                            var ProgressNoteSign = $("<div id='progressnotesign' class='hidden' ></div>").append(html);
                            $("body").append(ProgressNoteSign);
                            $.when(Clinical_NotesView.NotesPreview(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params["CurrentNotesProviderId"], null, null, isComponentSelect)).then(function () {
                                var ProviderName = "";
                                var IsWaterMarkApplied = 0;
                                if (Clinical_NotesView.provider_detail != null) {
                                    // Begin Added by Azeem Raza Tayyab on 29-Mar-2017 to Fix issue EMR-3417
                                    var alreadySign = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#img_eSignature_ProgressNotes');
                                    if (alreadySign.length > 0 && !Clinical_ProgressNote.params.IsPhoneEncounter) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#img_eSignature_ProgressNotes').parent().remove();
                                    }
                                    var alreadySignProdr = $("#" + Clinical_ProgressNote.params.PanelID + " #ProgressnoteHTML").find('#signedByProvider');
                                    if (alreadySignProdr.length > 0 && !Clinical_ProgressNote.params.IsPhoneEncounter) {
                                        $("#" + Clinical_ProgressNote.params.PanelID + " #ProgressnoteHTML").find('#signedByProvider').remove();
                                    }
                                    var alreadySignBy = $("#" + Clinical_ProgressNote.params.PanelID + " #ProgressnoteHTML .list-unstyled li:contains('e-Signed by:')");
                                    if (alreadySignBy.length > 0 && !Clinical_ProgressNote.params.IsPhoneEncounter) {
                                        $("#" + Clinical_ProgressNote.params.PanelID + " #ProgressnoteHTML .list-unstyled li:contains('e-Signed by:')").parent().remove();
                                    }
                                    // End Added by Azeem Raza Tayyab on 29-Mar-2017 to Fix issue EMR-3417
                                    var provider_detail = Clinical_NotesView.provider_detail;
                                    ProviderName = provider_detail.txtShortName;
                                    var eSignature_image_Src = provider_detail.imgeSignature;
                                    var Is_eSignatured = provider_detail.chkIs_eSignatured && provider_detail.chkIs_eSignatured != "" ? JSON.parse(provider_detail.chkIs_eSignatured.toLowerCase()) : false;
                                    var Clinical_progressNotes_formSelector = Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote";

                                    if (eSignature_image_Src != "" && Is_eSignatured) {

                                        var isBrowserIE = providerDetail.GetIEVersion() > 0;
                                        if (isBrowserIE) {
                                            eSignature_image_Src = eSignature_image_Src.replace("System.Byte[]", "image/gif");
                                        }

                                        var imgeSignatureHtml = '<div class="SignatureComponent" NoteComponentId="NCDummyId" style="max-height:350px; overflow-y:auto;margin-top:15px;" >' +
                                                                    '<img id="img_eSignature_ProgressNotes" src="' + eSignature_image_Src + '" ' +
                                                                         'alt="" style="height: 125px; width: 315px;border:none;" ' +
                                                                         'class="img-responsive img-center mt-lg img-thumbnail"/>'
                                        '</div>';

                                        if (Clinical_ProgressNote.params.CCMDuration && Clinical_ProgressNote.params.FromCCM == "") Clinical_ProgressNote.params.FromCCM = true;

                                        if (!Clinical_ProgressNote.params.IsPhoneEncounter)
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append(imgeSignatureHtml);
                                        else if (alreadySign.length == 0) {
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append(imgeSignatureHtml);
                                        }
                                        // Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                                    }

                                    var signeDateTime = moment().format("dddd, MMMM Do YYYY") + ' at ' + moment().format("h:mm a");
                                    var ResourceProvider = "";
                                    var SignProvider = "";
                                    var ResourceproQualification = "";
                                    var ProvQualification = "";
                                    var dfdprov = $.Deferred();
                                    var ProviderId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfProviderId').val();
                                    var ResourceProviderId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfResourceProvider').val();
                                    var ProviderIds = ProviderId + "," + ResourceProviderId;
                                    providerDetail.GetMultipleProviderInfo_DBCall(ProviderIds).done(function (response) {

                                        if (response.status != false) {

                                            $.each(response.ProviderFill_JSON, function (i, item) {

                                                if (item.ProviderId == ResourceProviderId) {
                                                    var Resprovider_dtl = item.ProviderFill_JSON;
                                                    ResourceProvider = Resprovider_dtl.txtFirstName + ", " + Resprovider_dtl.txtLastName + " " + Resprovider_dtl.txtMI;
                                                    ResourceproQualification = Resprovider_dtl.txtQualification;
                                                }
                                                else if (item.ProviderId == ProviderId) {
                                                    var provider_dtl = item.ProviderFill_JSON;
                                                    SignProvider = provider_dtl.txtFirstName + ", " + provider_dtl.txtLastName + " " + provider_dtl.txtMI;
                                                    ProvQualification = provider_dtl.txtQualification;
                                                }
                                            });
                                        }

                                        dfdprov.resolve();
                                    });

                                    var userName = globalAppdata["AppUserFirstName"] + ' ' + globalAppdata["AppUserLastName"];
                                    dfdprov.then(function () {
                                        if (!Clinical_ProgressNote.params.IsPhoneEncounter) {
                                            if (!Clinical_ProgressNote.params.FromCCM) {

                                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #splitterbody #txtResourceProvider').val() == "") {
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li></ul>');
                                                } else {
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li><li id="ResourceProvidersign">' + ResourceProvider + ' ' + ResourceproQualification + ' I, ' + SignProvider + ' ' + ProvQualification + ' agree with ' + ResourceProvider + ' regarding the findings and plan of care as documented note above.</li></ul>');
                                                }
                                            } else {
                                                // CCM
                                                if (globalAppdata["AppUserNameFullName"].trim() == Clinical_ProgressNote.params.CurrentNotesProviderText.trim()) {
                                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #splitterbody #txtResourceProvider').val() == "") {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li></ul>');
                                                    } else {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li><li id="ResourceProvidersign">' + ResourceProvider + ' ' + ResourceproQualification + ' I, ' + SignProvider + ' ' + ProvQualification + ' agree with ' + ResourceProvider + ' regarding the findings and plan of care as documented note above.</li></ul>');
                                                    }
                                                }
                                                else {
                                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #splitterbody #txtResourceProvider').val() == "") {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">Chronic Care Manager completed 20 minutes or greater of non face to face contact with this patient during the course of the month. The patient\'s care plan was reviewed and I ' + Clinical_ProgressNote.params.CurrentNotesProviderText + ' agree with ' + userName + ' regarding instructions provided to this patient.</li></ul>');
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append(imgeSignatureHtml);
                                                    } else {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li><li id="ResourceProvidersign">' + ResourceProvider + ' ' + ResourceproQualification + ' I, ' + SignProvider + ' ' + ProvQualification + ' agree with ' + ResourceProvider + ' regarding the findings and plan of care as documented note above.</li></ul>');
                                                    }
                                                }
                                            }
                                        }
                                        else if (alreadySignProdr.length == 0) {
                                            if (!Clinical_ProgressNote.params.FromCCM) {
                                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #splitterbody #txtResourceProvider').val() == "") {
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li></ul>');
                                                } else {
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li><li id="ResourceProvidersign">' + ResourceProvider + ' ' + ResourceproQualification + ' I, ' + SignProvider + ' ' + ProvQualification + ' agree with ' + ResourceProvider + ' regarding the findings and plan of care as documented note above.</li></ul>');
                                                }
                                            }
                                            else {
                                                // CCM
                                                if (globalAppdata["AppUserNameFullName"].trim() == Clinical_ProgressNote.params.CurrentNotesProviderText.trim()) {
                                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #splitterbody #txtResourceProvider').val() == "") {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li></ul>');
                                                    } else {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li><li id="ResourceProvidersign">' + ResourceProvider + ' ' + ResourceproQualification + ' I, ' + SignProvider + ' ' + ProvQualification + ' agree with ' + ResourceProvider + ' regarding the findings and plan of care as documented note above.</li></ul>');
                                                    }
                                                }
                                                else {
                                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #splitterbody #txtResourceProvider').val() == "") {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">Chronic Care Manager completed 20 minutes or greater of non face to face contact with this patient during the course of the month. The patient\'s care plan was reviewed and I ' + Clinical_ProgressNote.params.CurrentNotesProviderText + ' agree with ' + userName + ' regarding instructions provided to this patient.</li></ul>');
                                                    } else {
                                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul id="signedByProvider" class="SignatureComponent list-unstyled" NoteComponentId="NCDummyId"><li id="signedBy">e-Signed by: ' + userName + ' on ' + signeDateTime + '</li><li id="ResourceProvidersign">' + ResourceProvider + ' ' + ResourceproQualification + ' I, ' + SignProvider + ' ' + ProvQualification + ' agree with ' + ResourceProvider + ' regarding the findings and plan of care as documented note above.</li></ul>');
                                                    }
                                                }
                                            }
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText('Signature', true).done(function () {
                                            $.when(Clinical_NotesView.NotesPreview(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params["CurrentNotesProviderId"], null, null, isComponentSelect, null, true)).then(function () {
                                                $.when(Clinical_NotesView.getPrintnotePDF(true, Clinical_ProgressNote.params.IsPhoneEncounter)).then(function () {


                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit').addClass('disableAll');
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' .splitterBody').addClass('disableAll');
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #sidebar-wrapper').addClass('disableAll');
                                                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#ChkBox_IsNonBilable').parent().addClass('disableAll');
                                                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSave ,#btnSign , #btnReview,#btnNotesDelete,#btnCreate_eSupperbill').addClass('disabled');
                                                    $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnPrint ,#btnAssign , #btnSend,#btnCreateLetter,#btnSyndromicSurveillance,#btnNoteCoSign,#btnNoteAmendment').removeClass('disabled');
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "none");
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "none");
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #btnNoteAttachment").addClass('disableAll');

                                                    //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                                                    Clinical_ProgressNote.saveComponentSOAPText('Billing Info');

                                                    utility.DisplayMessages("Successfully Signed!");

                                                    if (billingDetail.Status != "Signed" && isJustNoteSign != true) {

                                                        var responseIsCPTExsistsInEsupperbill;
                                                        $.when(responseIsCPTExsistsInEsupperbill = Clinical_ProgressNote.IsCPTExsistsInEsupperbill()).then(function () {
                                                            if (responseIsCPTExsistsInEsupperbill.response != "-1") {
                                                                if (($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('section[id*=Cli_Problems_Main]').length > 0 && $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('section[id*=Cli_Procedures_Main]').length > 0) || (responseIsCPTExsistsInEsupperbill.response == "1")) {
                                                                    Clinical_ProgressNote.CreateCharges(Obj);
                                                                    if ($('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val() == 'Signed') {
                                                                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                                                                        $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                                                                    }
                                                                    else {
                                                                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                                                                    }
                                                                }
                                                                else
                                                                    utility.DisplayMessages("Either Procedure or Diagnosis Code is missing. The charge will not be created", 3);
                                                            }

                                                        });
                                                    }
                                                    dfdFinal.resolve();
                                                });
                                            });
                                        });
                                    });
                                }
                            });

                        });
                    }
                    else {
                        dfdFinal.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                Clinical_ProgressNote.CreateCharges(Obj);
                if ($('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val() == 'Signed') {
                    $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                    $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                }
                else {
                    $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                }
                dfdFinal.resolve();
            }

        }, function () {
            Clinical_ProgressNote.params.CancelSign = true;
            dfdFinal.resolve();
        },
              'Confirm Sign'
              );




        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented

        //End//02-05-2016//Ahmad Raza//logic for CDS Alert
        return dfdFinal;
    },
    EnableDisableConcurrentNoteSignBtn: function (EnableOrDisable) {
        if (!Clinical_ProgressNote.SignalRHub)
            Clinical_ProgressNote.connectUserToSignalR();
        else {
            if (EnableOrDisable)
                Clinical_ProgressNote.SignalRHub.server.disableSignNoteBtn(Clinical_ProgressNote.params.NotesId, globalAppdata.AppUserId);
            else
                Clinical_ProgressNote.SignalRHub.server.enableSignNoteBtn(Clinical_ProgressNote.params.NotesId, globalAppdata.AppUserId);
        }
    },
    //This function is called by Sign btn of Progress Note. it ask for signing Progress note
    Sign: function (BillingInformation, Obj, customSigMsg, isComponentSelect, prntctrl, IsFromProgressNote, NotesId, isFromEsuperBill) {
        var dfd = $.Deferred();
        $.when(innerResponse = Clinical_ProgressNote.CheckIfNoteIsAccessdByOthersOrNot()).then(function () {
            if (innerResponse.response.Result) {
                Clinical_ProgressNote.EnableDisableConcurrentNoteSignBtn(true);
                utility.myConfirm((innerResponse.response.UserName ? utility.CapitalizeFirstCharCommaSepString(innerResponse.response.UserName) : "Another user") + " is accessing the note. Do you still want to Sign?", function () {
                    $.when(Clinical_ProgressNote.CountinueSign(BillingInformation, Obj, customSigMsg, isComponentSelect, prntctrl, IsFromProgressNote, NotesId, isFromEsuperBill)).then(function () {
                        dfd.resolve();
                    });
                },
                function () {
                    Clinical_ProgressNote.EnableDisableConcurrentNoteSignBtn(false);
                    dfd.resolve();
                }, '<b>Concurrent Access</b>');
            }
            else {
                $.when(Clinical_ProgressNote.CountinueSign(BillingInformation, Obj, customSigMsg, isComponentSelect, prntctrl, IsFromProgressNote, NotesId, isFromEsuperBill)).then(function () {
                    dfd.resolve();
                });
            }
        });
        return dfd;
    },
    CountinueSign: function (BillingInformation, Obj, customSigMsg, isComponentSelect, prntctrl, IsFromProgressNote, NotesId, isFromEsuperBill) {
        if (IsFromProgressNote)
            NotesId = Clinical_ProgressNote.params.NotesId;
        var dfd = $.Deferred();
        $.when(Clinical_ProgressNote.VBPWithReasoningLoad(BillingInformation, Obj, customSigMsg, isComponentSelect, prntctrl, NotesId, IsFromProgressNote, isFromEsuperBill)).done(function () {
            dfd.resolve();
            Clinical_NotesSearch.SetNotesCount();
        });
        return dfd;
    },

    CheckIfNoteIsAccessdByOthersOrNot: function () {
        var dfd = $.Deferred();
        Clinical_ProgressNote.AccessdByOthers_DbCall("Check_Note_Is_Accessd_By_Others").done(function (response) {
            response = JSON.parse(response);
            dfd.response = response;
            dfd.resolve();
        });
        return dfd;
    },
    removeSignNoteUsers: function () {
        Clinical_ProgressNote.AccessdByOthers_DbCall("remove_users_against_note").done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                if (!Clinical_ProgressNote.SignalRHub)
                    Clinical_ProgressNote.connectUserToSignalR();
                if (Clinical_ProgressNote.SignalRHub) {
                    Clinical_ProgressNote.SignalRHub.server.revokeNoteSignAccess(Clinical_ProgressNote.params.NotesId, globalAppdata.AppUserId, globalAppdata.AppUserNameFullName);
                }
                else
                    console.log('Not connected to server.');
            }
        });
    },

    AccessdByOthers_DbCall: function (commandName) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = commandName;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    IsCPTExsistsInEsupperbill: function (NoteId) {
        var dfd = $.Deferred();
        Clinical_ProgressNote.IsCPTExsistsInEsupperbill_DBCALL(NoteId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.response = response.IsCptExsistsInEsupperbill;
                dfd.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.response = "-1";
                dfd.resolve();
            }
        });
        return dfd;
    },
    IsCPTExsistsInEsupperbill_DBCALL: function (NoteId) {

        var objData = new Object();
        if (NoteId != null && typeof NoteId != typeof undefined && NoteId != "") {
            objData["NotesId"] = NoteId;
        }
        else {
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        }

        objData["commandType"] = "Is_CPT_Exsists_In_Esupperbill";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },
    //Create_eSupperbill: function () {
    //},


    CreateCharges: function (Obj) {
        if (BillingInformation == null) {
            BillingInformation = GetTab("BillingInformation");
            var params = Clinical_ProgressNote.initializeBillingInfoParams();
            $.when(BillingInformation.Load(params)).done(function (x) {
                Clinical_ProgressNote.CreateObjectForBilling();
            });
        }
        else if (BillingInformation != null && Obj == null) {
            Clinical_ProgressNote.CreateObjectForBilling();
        }
        else if (BillingInformation != null && Obj != null) {

            var found = false, name;
            for (name in BillingInformation.params) {
                if (BillingInformation.params.hasOwnProperty(name)) {
                    found = true;
                    break;
                }
            }

            if (BillingInformation.params && found == false) {
                BillingInformation.params = Clinical_ProgressNote.initializeBillingInfoParams();
            }

            BillingInformation.CreateCharge(Obj);

            if (parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val()) > 0) {
                Clinical_ProgressNote.Signed_BillingInfo().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        BillingInformation.Status = "Signed";
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }, function (response) {

                });
            }

        }
        // findInDiv.hide(true);
    },
    initializeBillingInfoParams: function () {
        var params = [];
        params["ParentCtrl"] = "clinicalTabProgressNote";
        params["FromAdmin"] = 0;
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["VisitId"] = Clinical_ProgressNote.params.VisitId;
        params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
        params["BillingInfoId"] = parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val());
        params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
        params["PatientId"] = Clinical_ProgressNote.params.patientID;
        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        params["PatientTypeId"] = Clinical_ProgressNote.params.PatientTypeId;
        params["AppointmentDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #txtLinkedAppointment').val();
        params["FacilityId"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #hfFacilityId').val();
        BillingInformation.PatientInfoJSON = {
        }
        if (Patient_Demographic.params && Patient_Demographic.params.PatientFacilityId) {

            if (params["FacilityId"] == "") {
                params["FacilityId"] = Patient_Demographic.params.PatientFacilityId;
                BillingInformation.PatientInfoJSON.FacilityID = Patient_Demographic.params.PatientFacilityId;
            }
            else
                BillingInformation.PatientInfoJSON.FacilityID = params["FacilityId"];

            BillingInformation.PatientInfoJSON.RefProviderID = $("#pnlClinicalProgressNote #hfRefProvider").val();
        }
        return params;
    },
    CreateObjectForBilling: function () {

        //-----------------------------------------------------

        var facilityId = $('#pnlClinicalProgressNote #hfFacilityId').val();
        Admin_Facility.SearchFacility(null, facilityId).done(function (response1) {
            var facPOS = "";
            if (response1.FacilityCount > 0) {
                var FacilityLoadJSONData = JSON.parse(response1.FacilityLoad_JSON)[0];
                facPOS = FacilityLoadJSONData.POSName;
            }
            BillingInformation.BillingInformationLoad().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    response.BillingInfoCPTFill_JSON = JSON.parse(response.BillingInfoCPTFill_JSON);
                    response.BillingInfoICDFill_JSON = JSON.parse(response.BillingInfoICDFill_JSON);

                    response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);
                    if (response.BillingInfoFill_JSON.Status == 'Signed' || $('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val() == 'Signed') {
                        BillingInformation.Status = 'Signed';
                    }

                    var Obj = {
                    };
                    var ICDs = []

                    for (var index in response.BillingInfoICDFill_JSON) {
                        var item = response.BillingInfoICDFill_JSON[index];
                        var currentICD = {
                        };
                        if (item.ICDType == "10") {
                            currentICD.ICDCode9 = '';
                            currentICD.ICDDescription9 = item.ICDCodeDescription.replace(/"/g, "'");;

                            currentICD.ICDCode10 = item.ICDCode;
                            currentICD.ICDDescription10 = item.ICDCodeDescription.replace(/"/g, "'");;

                            currentICD.SNOMEDCode = item.SNOMEDID;
                            currentICD.SNOMEDDescription = item.SNOMEDDescription;
                        }
                        else {
                            currentICD.ICDCode9 = item.ICDCode;
                            currentICD.ICDDescription9 = item.ICDCodeDescription.replace(/"/g, "'");;

                            currentICD.ICDCode10 = '';
                            currentICD.ICDDescription10 = item.ICDCodeDescription.replace(/"/g, "'");;
                            currentICD.SNOMEDCode = item.SNOMEDID;
                            currentICD.SNOMEDDescription = item.SNOMEDDescription;
                        }
                        ICDs.push(currentICD);
                    }


                    var CPTs = []

                    for (var index in response.BillingInfoCPTFill_JSON) {
                        var item = response.BillingInfoCPTFill_JSON[index];
                        var currentCPT = {
                        };
                        currentCPT.CPTCode = item.CPTCode;
                        currentCPT.CPTDescription = item.CPTDescription.replace(/"/g, "'").replace(/&#39;/g, "");
                        currentCPT.Modifier1 = item.Modifier1;
                        currentCPT.Modifier2 = item.Modifier2;
                        currentCPT.Modifier3 = item.Modifier3;
                        currentCPT.Modifier4 = item.Modifier4;
                        currentCPT.DxPointer1 = item.ICDPointer1;
                        currentCPT.DxPointer2 = item.ICDPointer2;
                        currentCPT.DxPointer3 = item.ICDPointer3;
                        currentCPT.DxPointer4 = item.ICDPointer4;
                        currentCPT.UnitsId = item.Units;
                        currentCPT.POS = facPOS;
                        currentCPT.DOSFrom = item.DOSFrom;
                        currentCPT.DOSTo = item.DOSTo;

                        CPTs.push(currentCPT);

                    }

                    Obj.CPTs = CPTs;
                    Obj.ICDs = ICDs;
                    Obj.POS = facPOS;

                    if (BillingInformation.params && BillingInformation.params.length == 0) {
                        BillingInformation.params = Clinical_ProgressNote.initializeBillingInfoParams();
                    }

                    BillingInformation.LoadAttachecdICDsAndCPTs().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            response.ProblemListFill_JSON = JSON.parse(response.ProblemListFill_JSON);
                            response.ProcedureListFill_JSON = JSON.parse(response.ProcedureListFill_JSON);

                            response.ProblemListFill_JSON = $.grep(response.ProblemListFill_JSON, function (a) {
                                return a.IsNoteLinked == "True";
                            });
                            response.ProcedureListFill_JSON = $.grep(response.ProcedureListFill_JSON, function (a) {
                                return a.IsNoteLinked == "1" || (parseInt(a.VaccineHxId) > 0 || parseInt(a.ImmTherInjectionId) > 0);
                            });


                            var counter = 0;
                            var objData = {
                            };
                            if (BillingInformation.params.BillingInfoId > 0) {
                                objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
                            }
                            else {
                                objData["BillingInfoId"] = '-1'
                            }
                            objData["commandType"] = "BILLING_INFORMATION_SAVE";
                            objData["NotesId"] = BillingInformation.params.NotesId;
                            objData["PatientId"] = BillingInformation.params.PatientId;
                            objData["ProviderId"] = BillingInformation.params["ProviderId"];
                            objData["FacilityId"] = BillingInformation.params["FacilityId"];
                            objData["VisitId"] = BillingInformation.params.VisitId;
                            objData["Status"] = 'Draft';
                            objData["VisitDate"] = BillingInformation.params.VisitDate;
                            objData["POS"] = facPOS;
                            objData.ICDs = [];
                            objData.CPTs = [];

                            for (var i in response.ProblemListFill_JSON) {
                                item = response.ProblemListFill_JSON[i];
                                var ICD = {
                                };
                                ICD.ICDCode9 = item.ICD9;
                                ICD.ICDCode10 = item.ICD10;
                                ICD.ICDDescription9 = item.ICD9_Description;
                                ICD.ICDDescription10 = item.ICD10_Description;
                                ICD.SNOMEDCode = item.SNOMEDID;
                                ICD.SNOMEDDescription = item.SNOMED_DESCRIPTION
                                objData.ICDs.push(ICD);
                            }

                            for (var i in response.ProcedureListFill_JSON) {
                                item = response.ProcedureListFill_JSON[i];
                                var currentCPT = {
                                };
                                currentCPT.CPTCode = item.CPTCode;
                                currentCPT.CPTDescription = item.CPT_DESCRIPTION.replace(/"/g, "'").replace(/&#39;/g, "").replace(/&amp;/g, '&');

                                var modifier1 = item.Modifier;
                                var modifier2 = "";
                                var modifier3 = "";
                                var modifier4 = "";


                                var icd_cods = item.ICDCodes.split(',');
                                var pinter1 = icd_cods.length > 0 && icd_cods[0] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[0], objData.ICDs) : '1';
                                var pinter2 = icd_cods.length > 1 && icd_cods[1] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[1], objData.ICDs) : '';
                                var pinter3 = icd_cods.length > 2 && icd_cods[2] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[2], objData.ICDs) : '';
                                var pinter4 = icd_cods.length > 3 && icd_cods[3] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[3], objData.ICDs) : '';

                                if (BillingInformation.params.BillingInfoId > 0 && Obj != null && Obj.CPTs.length > 0) {
                                    var cpt_obj = $.grep(Obj.CPTs, function (i, item) {
                                        return currentCPT.CPTCode.trim() == i.CPTCode;

                                    });
                                    if (cpt_obj.length > 0) {
                                        pinter1 = cpt_obj[0].DxPointer1;
                                        pinter2 = cpt_obj[0].DxPointer2;
                                        pinter3 = cpt_obj[0].DxPointer3;
                                        pinter4 = cpt_obj[0].DxPointer4;

                                        if (cpt_obj[0].Modifier1 != "")
                                            modifier1 = cpt_obj[0].Modifier1;
                                        modifier2 = cpt_obj[0].Modifier2;
                                        modifier3 = cpt_obj[0].Modifier3;
                                        modifier4 = cpt_obj[0].Modifier4;
                                    }
                                }

                                currentCPT.Modifier1 = modifier1;
                                currentCPT.Modifier2 = modifier2;
                                currentCPT.Modifier3 = modifier3;
                                currentCPT.Modifier4 = modifier4;
                                currentCPT.DxPointer1 = pinter1;
                                currentCPT.DxPointer2 = pinter2;
                                currentCPT.DxPointer3 = pinter3;
                                currentCPT.DxPointer4 = pinter4;
                                currentCPT.UnitsId = item.Unit;
                                currentCPT.Unit = item.Unit;
                                currentCPT.DOSFrom = item.StartDate;
                                currentCPT.DOSTo = item.EndDate;
                                currentCPT.CPTSNOMEDCodeId = item.SNOMEDID;
                                currentCPT.CPTSNOMEDDescription = item.SNOMED_DESCRIPTION;
                                currentCPT.txtFEE = item.Fee;
                                currentCPT.Fee = item.Fee;
                                if (i == 0) {
                                    currentCPT.Copay = item.Copay;
                                } else {
                                    currentCPT.Copay = "";
                                }
                                if (item.ExpectedFee && item.ExpectedFee != "0") {
                                    currentCPT.hfExpectedFee = item.ExpectedFee;
                                    currentCPT.Expectedfee = item.ExpectedFee;
                                }
                                else {
                                    currentCPT.hfExpectedFee = "0.00";
                                    currentCPT.Expectedfee = "0.00";
                                }
                                currentCPT.PatCharges = item.PatCharges;
                                currentCPT.Inscharges = item.Inscharges;
                                currentCPT.POS = facPOS;

                                //ignore duplicate CPT Code for charge creation
                                var isduplicate = $.grep(objData.CPTs, function (i, item) {
                                    return currentCPT.CPTCode.trim() == i.CPTCode && currentCPT.CPTDescription == i.CPTDescription;
                                });

                                if (isduplicate.length <= 0)
                                    objData.CPTs.push(currentCPT);
                            }

                            BillingInformation.BillingObj = objData;
                            BillingInformation.AttachtedCPTData = objData.CPTs;
                            //BillingInformation.AttachtedCPTData.reverse();
                            if (BillingInformation.params.BillingInfoId > 0) {
                                BillingInformation.CreateCharge(BillingInformation.BillingObj, true);
                            }
                            else {
                                BillingInformation.BillingInfoSave(objData).done(function (InnerResponse) {
                                    InnerResponse = JSON.parse(InnerResponse);
                                    if (InnerResponse.status != false) {
                                        BillingInformation.BillingObj.BillingInfoId = BillingInformation.params.BillingInfoId = InnerResponse.BillingInfoId;
                                        $('#pnlClinicalProgressNote #hfBillingInfoId').val(InnerResponse.BillingInfoId);
                                        BillingInformation.CreateCharge(BillingInformation.BillingObj, true);
                                    }
                                });
                            }
                        }
                    });

                    // findInDiv.hide(true);
                }
            });

        });
        //-----------------------------------------------------

    },

    Getpointers: function (code, ICDs) {
        var toReturn = '';
        $.each(ICDs, function (i, item) {
            if (code.trim() == item.ICDCode10) {
                toReturn = i + 1;
                return false;
            }
        });
        return toReturn.toString();
    },

    Signed_BillingInfo: function () {
        var objData = {
        };
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["BillingInfoId"] = parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val());
        objData["Status"] = "Signed";
        objData["commandType"] = "SIGNED_BILLINGINFO";
        var data = JSON.stringify(objData);
        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    IsBillingInformationCreated: function () {
        var objData = new Object();
        objData["BillingInfoId"] = null;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["commandType"] = "is_billing_information_created";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    BillingInfoPopUp: function () {

        var objDeffered = $.Deferred();
        var BillingInfoId = 0;
        Clinical_ProgressNote.IsBillingInformationCreated().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                if (response.IsBillingInfoCreated == true) {
                    $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(response.BillingInfoId);
                    BillingInfoId = response.BillingInfoId;
                }

                objDeffered.resolve(true);

            }
            else
                objDeffered.resolve(false);

        });

        objDeffered.done(function (res) {

            //var BillingInfoId = parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val()) || 0;
            if ($('#pnlClinicalProgressNote #hfNoteStatus').val() == 'Signed' && BillingInformation.Status && BillingInformation.Status == "Draft") {
                utility.DisplayMessages("No eSuperbill Information is associated with the current provider note.", 2);
                return false;
            }
            if ($('#pnlClinicalProgressNote #hfNoteStatus').val() == 'Signed' && BillingInfoId <= 0) {
                utility.DisplayMessages("No eSuperbill Information is associated with the current provider note.", 2);
                return false;
            }

            var params = [];
            params["ParentCtrl"] = "clinicalTabProgressNote";
            params["FromAdmin"] = 0;
            params["NotesId"] = Clinical_ProgressNote.params.NotesId;
            params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
            params["PatientId"] = Clinical_ProgressNote.params.patientID;
            params["VisitId"] = Clinical_ProgressNote.params.VisitId;
            params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
            params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
            params["FacilityId"] = $('#pnlClinicalProgressNote #hfFacilityId').val();
            params["NoteStatus"] = $('#pnlClinicalProgressNote #hfNoteStatus').val();
            params["RefProviderId"] = $('#pnlClinicalProgressNote #hfRefProvider').val();
            params["BillingInfoId"] = BillingInfoId;
            params["PatientTypeId"] = Clinical_ProgressNote.params.PatientTypeId;
            params["AppointmentDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #txtLinkedAppointment').val();
            if (BillingInfoId <= 0) {
                Clinical_ProgressNote.BillingInfoSave().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status) {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(response.BillingInfoId);
                        params["BillingInfoId"] = response.BillingInfoId;
                        LoadActionPan("BillingInformation", params);
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                });
            }
            else
                LoadActionPan("BillingInformation", params);
        });
    },
    BillingInfoSave: function () {
        var objData = {
        };
        objData["BillingInfoId"] = '-1'
        objData["commandType"] = "BILLING_INFORMATION_SAVE";
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        objData["VisitId"] = Clinical_ProgressNote.params.VisitId;
        objData["Status"] = 'Draft';
        objData["VisitDate"] = Clinical_ProgressNote.params.VisitDate;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    BillingInfo: function (isFromTab) {

        Clinical_ProgressNote.IsNoteComponentAvaliable(isFromTab, "eSuperbill").done(function (res) {
            if (res == true) {
                Clinical_ProgressNote.BillingInformationLoad_DbCall().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var ExisitingBillingInfoId = -1;
                        billingDetail = JSON.parse(response.BillingInfoFill_JSON);
                        billingDetail.BillingInfoId != null ? ExisitingBillingInfoId = billingDetail.BillingInfoId : ExisitingBillingInfoId = -1;
                        if (billingDetail.NotesId != null && billingDetail.NotesId != "" && billingDetail.NotesId > 0) {
                            ExisitingBillingInfoId = -1;
                        }

                        if ((ExisitingBillingInfoId == false || ExisitingBillingInfoId == null || ExisitingBillingInfoId < 0) || Clinical_ProgressNote.params.AppointmentVisitId < 0) {

                            var ccmCPTexists = false;

                            if ($('[id^=Cli_Procedures_Main]').text().indexOf('99490') > -1 || $('[id^=Cli_Procedures_Main]').text().indexOf('99487') > -1 || $('[id^=Cli_Procedures_Main]').text().indexOf('99489') > -1) {
                                ccmCPTexists = true
                            }

                            if (!ccmCPTexists && $('#pnlClinicalProgressNote #hfNoteStatus').val() != 'Signed') {
                                Clinical_ProgressNote.BillingInfoPopUp();
                            }
                            else {
                                //Clinical_ProgressNote.BillingInfoPopUp();
                                if ($('#pnlClinicalProgressNote #hfBillingInfoId').val() == "" && Clinical_ProgressNote.params.PatchBillingInfoId)
                                    $('#pnlClinicalProgressNote #hfBillingInfoId').val(Clinical_ProgressNote.params.PatchBillingInfoId);

                                if ($('#pnlClinicalProgressNote #hfNoteStatus').val() == 'Signed' && !$('#pnlClinicalProgressNote #hfBillingInfoId').val()) {
                                    utility.DisplayMessages("No eSuperbill Information is associated with the current provider note.", 2);
                                }
                                else {
                                    if (Clinical_ProgressNote.params.CCMDuration) {
                                        if ($('#pnlClinicalProgressNote #hfNoteStatus').val() == 'Signed') {
                                            Clinical_ProgressNote.BillingInfoPopUp();
                                        }
                                        else {
                                            utility.myConfirm('CCM Patient can be billed once in a calendar month. Do you want to continue?', function () {
                                                Clinical_ProgressNote.params.CCMForSigned = true;
                                                if (!Clinical_ProgressNote.params["NoteDate"])
                                                    Clinical_ProgressNote.params["NoteDate"] = $("#dtpVisitDate").val();

                                                if (Clinical_ProgressNote.params.CCMDuration && Clinical_ProgressNote.params.FromCCM == "")
                                                    Clinical_ProgressNote.params.FromCCM = true;

                                                Clinical_ProgressNote.params.ProviderId = $("#pnlClinicalProgressNote #hfProviderId").val();
                                                Clinical_ProgressNote.BillingInfoPopUp();
                                            }, function () {
                                            }, 'eSuperbill');
                                        }
                                    }
                                    else {
                                        Clinical_ProgressNote.BillingInfoPopUp();
                                    }
                                }
                            }
                        } else {
                            utility.DisplayMessages("An eSuperbill for this Date of Service has already been created", 3);
                        }
                    }
                });
            }
        });


    },

    BillingInformationLoad_DbCall: function () {
        var objData = new Object();
        objData["VisitId"] = Clinical_ProgressNote.params.AppointmentVisitId;
        objData["commandType"] = "billing_information_select_by_visitid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    GetAppointmentIdByNoteId: function (NoteId) {
        if ($('#' + Clinical_ProgressNote.params.PanelID + ' #hfAppointmentId').val() == '' && $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val() == '-1') {
            return "{ status: false }";
        }
        else {
            return "{ status: true }";
        }

    },
    Review: function () {
    },
    Send: function () {

        var dfdFinal = $.Deferred();
        var deffered = $.Deferred();

        $('#pnlClinicalProgressNote #hfNoteText').val($('#pnlClinicalProgressNote #ProgressnoteHTML').html());
        var NotesId = Clinical_Notes.params.NotesId;

        cmd = [];
        cmd.TabID = "Clinical_NotesView";
        cmd.PanelID = "Clinical_NotesView";
        cmd.MasterTabID = "";
        cmd.ParentTabID = "";
        cmd.ContainerControlID = "Clinical_NotesView";
        cmd.Selected = false;
        cmd.isActionPan = true;
        cmd.Container = "";
        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesView.html";
        cmd.ActionPanContainer = "actionPanNotesView";

        var paramsnotesprev = [];
        paramsnotesprev["FromAdmin"] = "0";
        paramsnotesprev["NotesId"] = NotesId;
        paramsnotesprev["PatientId"] = Clinical_ProgressNote.params.patientID;
        paramsnotesprev["RefSearch"] = "DraftSearch";
        paramsnotesprev["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        paramsnotesprev["FromProgressnote"] = "1";
        paramsnotesprev["ParentCtrl"] = 'clinicalTabNotes';
        paramsnotesprev["ParentCtrlPanelID"] = "pnlClinicalNotes";
        var Tab = GetTab(cmd.TabID);
        var ClinicalTab = cmd;

        var dfd = new $.Deferred();
        var html = utility.getTabHtml(cmd.TabID);
        if (html) {

            dfd.resolve(html);
            if (cmd.Container) {
                eval(cmd.ContainerControlID + '.Load')(paramsnotesprev);
            }
            return dfd.promise();
        } else {
            $.get(GetTab(cmd.TabID).Path, {
                cache: false
            }, function (content) {
                html = content;
                eval(cmd.ContainerControlID + '.Load')(paramsnotesprev);
                dfd.resolve(html);
            });
        }
        dfd.then(function () {
            var ProgressNoteSign = $("<div id='progressnotesign' class='hidden' ></div>").append(html);
            $("body").append(ProgressNoteSign);

            $.when(Clinical_NotesView.NotesPreview(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params["CurrentNotesProviderId"], null, null, false, null, true)).then(function () {
                //$.when(Clinical_NotesView.getPrintnotePDF(true, Clinical_ProgressNote.params.IsPhoneEncounter)).then(function () {
                $('#progressnotesign').remove();
                $('#' + Clinical_NotesView.params["PanelID"] + " #printcall").hide();
                var params = [];
                params["IsOptional"] = false;
                params["RefForm"] = "frmClinicalProgressNote";
                params["FacilityId"] = "-1";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["FromAdmin"] = "0";
                params["PDFBase64"] = Clinical_NotesView.pdf;
                params["ParentCtrl"] = "clinicalTabProgressNote";
                LoadActionPan('Batch_FaxSend', params);

                dfdFinal.resolve();
                //});
            });
        });

        return dfdFinal;
    },
    Assign: function () {
    },
    Print: function () {
        if (Clinical_ProgressNote.params.triggerCount != 1) {
            if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
                $.when(Clinical_PhoneEncounter.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"), true)).then(function () {
                    //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
                    AppPrivileges.GetFormPrivileges("Notes_Notes", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Admin_Facility.SearchFacility(null, $("#pnlClinicalProgressNote #hfFacilityId").val()).done(function (response1) {
                                if (response1.FacilityCount > 0) {
                                    var FacilityLoadJSONData = JSON.parse(response1.FacilityLoad_JSON)[0];
                                    Clinical_Notes.NotesPreview(Clinical_ProgressNote.params.NotesId, 'clinicalTabProgressNote', Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, Clinical_ProgressNote.params.VisitId, Clinical_ProgressNote.params.BillingInfoId, Clinical_ProgressNote.params.AppointmentDate, Clinical_ProgressNote.params.VisitId, '', Clinical_ProgressNote.params.PatientTypeId, $("#pnlClinicalProgressNote #hfFacilityId").val(), FacilityLoadJSONData.POSName, $('#pnlClinicalProgressNote #hfRefProvider').val(), '', Clinical_ProgressNote.params.IsPhoneEncounter);
                                }
                                else {
                                    Clinical_Notes.NotesPreview(Clinical_ProgressNote.params.NotesId, 'clinicalTabProgressNote', Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, Clinical_ProgressNote.params.VisitId, Clinical_ProgressNote.params.BillingInfoId, Clinical_ProgressNote.params.AppointmentDate, Clinical_ProgressNote.params.VisitId, '', Clinical_ProgressNote.params.PatientTypeId, $("#pnlClinicalProgressNote #hfFacilityId").val(), "", $('#pnlClinicalProgressNote #hfRefProvider').val(), '', Clinical_ProgressNote.params.IsPhoneEncounter);
                                }
                            });
                        }
                        else {
                            utility.DisplayMessages(strMessage, 2);
                        }
                    });
                    //End//29/12/2015//Ahmad Raza//Privileges logic implemented
                });
            }
            else {
                Clinical_Notes.saveNote($("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate"));
                //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
                AppPrivileges.GetFormPrivileges("Notes_Notes", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_Facility.SearchFacility(null, $("#pnlClinicalProgressNote #hfFacilityId").val()).done(function (response1) {
                            if (response1.FacilityCount > 0) {
                                var FacilityLoadJSONData = JSON.parse(response1.FacilityLoad_JSON)[0];
                                Clinical_Notes.NotesPreview(Clinical_ProgressNote.params.NotesId, 'clinicalTabProgressNote', Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, Clinical_ProgressNote.params.VisitId, Clinical_ProgressNote.params.BillingInfoId, Clinical_ProgressNote.params.AppointmentDate, Clinical_ProgressNote.params.VisitId, '', Clinical_ProgressNote.params.PatientTypeId, $("#pnlClinicalProgressNote #hfFacilityId").val(), FacilityLoadJSONData.POSName, $('#pnlClinicalProgressNote #hfRefProvider').val(), '', Clinical_ProgressNote.params.IsPhoneEncounter);
                            }
                            else {
                                Clinical_Notes.NotesPreview(Clinical_ProgressNote.params.NotesId, 'clinicalTabProgressNote', Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, Clinical_ProgressNote.params.VisitId, Clinical_ProgressNote.params.BillingInfoId, Clinical_ProgressNote.params.AppointmentDate, Clinical_ProgressNote.params.VisitId, '', Clinical_ProgressNote.params.PatientTypeId, $("#pnlClinicalProgressNote #hfFacilityId").val(), "", $('#pnlClinicalProgressNote #hfRefProvider').val(), '', Clinical_ProgressNote.params.IsPhoneEncounter);
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(strMessage, 2);
                    }
                });
                //End//29/12/2015//Ahmad Raza//Privileges logic implemented
            }

            Clinical_ProgressNote.params.triggerCount = 1;
        }

    },
    // -------------- Ref Provider -----------------

    CoSignNotesOpen: function () {

        var params = [];
        params["ParentCtrl"] = "clinicalTabProgressNote";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("Clinical_NotesCoSign", params);

    },

    AmendmentNotesOpen: function () {
        AppPrivileges.GetFormPrivileges("Notes_Amendment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = "clinicalTabProgressNote";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["FromAdmin"] = 0;
                params["mode"] = "Add";
                LoadActionPan("Clinical_NotesAmendment", params);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }

        });
    },

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        utility.SetKendoAutoCompleteSourceforValidate($('#pnlClinicalProgressNote #txtRefProvider'), RefProviderName, $('#pnlClinicalProgressNote #hfRefProvider'), RefProviderId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "clinicalTabProgressNote";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {

        var params = [];
        params["ReferringProviderId"] = $('#pnlClinicalProgressNote #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "clinicalTabProgressNote";

        LoadActionPan('referringproviderDetail', params);
    },

    HideRefProviderLink: function () {
        $('#pnlClinicalNotes #hfRefProvider').val("-1");
        $('#pnlClinicalNotes #lnkRefProviderEdit').css("display", "none");
        $('#pnlClinicalNotes #lblRefProvider').css("display", "inline");
    },
    createLetter: function () {
        Clinical_ProgressNote.DetachedNoteComponentIds = [];
        Clinical_ProgressNote.AttachedNoteComponentIds = [];
        var params = [];
        params["ParentCtrl"] = "clinicalTabProgressNote";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["patientID"] = Clinical_ProgressNote.params.patientID;
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        LoadActionPan("Patient_Letter", params);
    },
    // -------------- End Ref Provider -------------

    //---------------Progress note - Patient Notes Modal Functions
    UnLoadPatientNotesCategories: function () {

        $("#pnlClinicalProgressNote #actionPanClinicalProgressNote").modal('hide');

        setTimeout(function () {
            $("#pnlClinicalProgressNote #actionPanClinicalProgressNote").find('div').first().remove();
            $('body').removeClass('modal-open')
        }, 300);

    },

    // this function set checked to all Component of Patient Note opened in Modal
    CheckAllNotesCategories: function () {
        $('#pnlClinicalProgressNote #actionPanClinicalProgressNote #divNote_Details input[type="checkbox"]:not(:disabled)').prop('checked', true);
    },

    // this function Copies checked  all Component of Patient Note to Progress Note HTML
    CopyAllNotesCategories: function () {

        var VitalId = '';
        var VitalsHtml = '';
        var CopiedControlLength = $('#pnlClinicalProgressNote #actionPanClinicalProgressNote #divNote_Details input[type="checkbox"]:checked:not(:disabled)').length;
        if (CopiedControlLength > 0) {
            //ProgressNote Copy Previous Component confirm for over-write
            utility.myConfirm('18', function () {
                var CopiedControl = $('#pnlClinicalProgressNote #actionPanClinicalProgressNote #divNote_Details input[type="checkbox"]:checked:not(:disabled)').map(function () {

                    if (this.id == 'clinical_vitals') {
                        var elementVital = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section')[0].id.replace('Cli_Vitals_Main', '');

                        utility.myConfirm('15', function () {
                            Clinical_ProgressNote.CopyVital(elementVital);
                        }, function () {
                        }, '1');

                    } else if (this.id == 'clinical_problems') {

                        Clinical_ProblemLists.detach_ComponentsProblemList('Problems', true, true);

                        var SelectedProblemListsId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Problems_Main', '');
                        }).get().join();

                        if (SelectedProblemListsId != null && SelectedProblemListsId != '') {
                            Clinical_ProblemLists.getProblemListsInfo(SelectedProblemListsId);
                        }
                        else {
                            Clinical_ProblemLists.checkProblemListExists();
                        }

                    } else if (this.id == 'clinical_medications') {

                        Clinical_Medications.detachMedicationsComponent('Medications', true, true);
                        var SelectedMedicationsId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Medications_Main', '');
                        }).get().join();

                        if (SelectedMedicationsId != null && SelectedMedicationsId != '') {
                            Clinical_Medications.getMedicationsInfo(SelectedMedicationsId);
                        }
                        else {
                            Clinical_Medications.checkMedicationsExists();
                        }

                    } else if (this.id == 'clinical_allergies') {

                        Clinical_Allergies.detach_ComponentsAllergy('Allergies', true, true);

                        var SelectedAllergiesId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Allergies_Main', '');
                        }).get().join();

                        if (SelectedAllergiesId != null && SelectedAllergiesId != '') {
                            Clinical_Allergies.getAllergiesInfo(SelectedAllergiesId);
                        }
                        else {
                            Clinical_Allergies.checkAllergyExists();
                        }

                    } else if (this.id == "clinical_socialhx") {

                        var elementSocialHx = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section')[0].id.replace('Cli_SocialHx_Main', '');

                        if (elementSocialHx != null && elementSocialHx != '') {
                            Clinical_SocialHx.getSocialHxInfo(null, elementSocialHx);
                        }
                        else {
                            Clinical_SocialHx.checkSocialHxExists();
                        }
                    }
                    else if (this.id == 'clinical_prescriptions') {

                        Clinical_Prescriptions.detach_ComponentsPrescription('Prescriptions', true, true);

                        var SelectedPrescriptionsId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Prescriptions_Main', '');
                        }).get().join();

                        if (SelectedPrescriptionsId != null && SelectedPrescriptionsId != '') {
                            Clinical_Prescriptions.getPrescriptionsInfo(SelectedPrescriptionsId);
                        }
                        else {
                            Clinical_Prescriptions.checkPrescriptionExists();
                        }
                    }
                    else if (this.id == 'clinical_physicalexam') {

                        PhysicalExamTemplatesRevamp.detach_ComponentsPhysicalExam('PhysicalExam', true, true);

                        var selectedPhysicalExamId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_PhysicalExam_Main', '');
                        }).get().join();

                        if (selectedPhysicalExamId != null && selectedPhysicalExamId != '') {
                            PhysicalExamTemplatesRevamp.getPhysicalExamInfo(null, null, selectedPhysicalExamId);
                        }
                        else {
                            PhysicalExamTemplatesRevamp.checkPhysicalExamExists();
                        }
                    }

                    else if (this.id == 'clinical_complaints') {

                        Clinical_Complaints.detach_ComponentsComplaints('Complaints', true, true);

                        var SelectedComplaintsId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Complaints_Main', '');
                        }).get().join();

                        if (SelectedComplaintsId != null && SelectedComplaintsId != '') {
                            Clinical_Complaints.getComplaintInfo(null, SelectedComplaintsId);
                        }
                        else {
                            Clinical_Complaints.checkComplaintHxExists();
                        }
                    }

                    else if (this.id == 'clinical_labresults') {

                        Clinical_LabOrder.detach_ComponentsLabResult('LabResults', true, true);

                        var SelectedLabResultId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_LabResultDetail_Main', '');
                        }).get().join();

                        if (SelectedLabResultId != null && SelectedLabResultId != '') {
                            Clinical_LabOrder.getLabResultInfo(SelectedLabResultId);
                        }
                        else {
                            Clinical_LabOrder.checkResultExists();
                        }
                    }

                    else if (this.id == 'clinical_radiologyresults') {

                        Clinical_LabOrder.detach_ComponentsLabResult('RadiologyResults', true, true);

                        var SelectedLabResultId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_RadiologyResultDetail_Main', '');
                        }).get().join();

                        if (SelectedLabResultId != null && SelectedLabResultId != '') {
                            Clinical_RadiologyOrder.getRadiologyResultInfo(SelectedLabResultId);
                        }
                        else {
                            Clinical_RadiologyOrder.checkResultExists();
                        }
                    }

                    else if (this.id == 'clinical_diagnosis') {

                    }

                    else if (this.id == 'clinical_habits') {

                    }

                    else if (this.id == 'clinical_procedures') {

                        Clinical_Procedures.detach_ComponentsProcedures('Procedures', true, true);

                        var SelectedProceduresId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Procedures_Main', '');
                        }).get().join();

                        if (SelectedProceduresId != null && SelectedProceduresId != '') {
                            Clinical_Procedures.getProceduresInfo(SelectedProceduresId);
                        }
                        else {
                            Clinical_Procedures.checkProceduresExists();
                        }
                    }

                    else if (this.id == 'clinical_medicalhx') {

                        Clinical_MedicalHx.detach_ComponentsMedicalHx('MedicalHx', true, true);

                        var SelectedMedicalHxId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_MedicalHx_Main', '');
                        }).get().join();

                        if (SelectedMedicalHxId != null && SelectedMedicalHxId != '') {
                            Clinical_MedicalHx.getMedicalHxInfo(null, null, SelectedMedicalHxId);
                        }
                        else {
                            Clinical_MedicalHx.checkMedicalHxExists();
                        }
                    }

                    else if (this.id == 'clinical_familyhx') {

                        Clinical_FamilyHx.detach_ComponentsFamilyHx('FamilyHx', true, true);

                        var SelectedFamilyHxId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_FamilyHx_Main', '');
                        }).get().join();

                        if (SelectedFamilyHxId != null && SelectedFamilyHxId != '') {
                            Clinical_FamilyHx.getFamilyHxInfo(null, null, SelectedFamilyHxId);
                        }
                        else {
                            Clinical_FamilyHx.checkFamilyHxExists();
                        }
                    }

                    else if (this.id == 'clinical_surgicalhx') {

                        Clinical_SurgicalHx.detach_ComponentsSurgicalHx('SurgicalHx', true, true);

                        var SelectedSurgicalHxId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_SurgicalHx_Main', '');
                        }).get().join();

                        if (SelectedSurgicalHxId != null && SelectedSurgicalHxId != '') {
                            Clinical_SurgicalHx.getSurgicalHxInfo(null, null, SelectedSurgicalHxId);
                        }
                        else {
                            Clinical_SurgicalHx.checkSurgicalHxExists();
                        }
                    }

                    else if (this.id == 'clinical_environmentalhx') {

                    }

                    else if (this.id == 'clinical_sexualhx') {

                    }

                    else if (this.id == 'clinical_hospitalizationhx') {

                        Clinical_HospitalizationHx.detach_ComponentsHospitalizationHx('HospitalizationHx', true, true);

                        var SelectedHospitalizationHxId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_HospitalizationHx_Main', '');
                        }).get().join();

                        if (SelectedHospitalizationHxId != null && SelectedHospitalizationHxId != '') {
                            Clinical_HospitalizationHx.getHospitalizationHxInfo(null, null, SelectedHospitalizationHxId);
                        }
                        else {
                            Clinical_HospitalizationHx.checkHospitalizationHxExists();
                        }
                    }

                    else if (this.id == 'clinical_laborders') {

                        Clinical_LabOrderDetail.detach_ComponentsLabOrder('LabOrders', true, true);

                        var SelectedLaborderId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_LabOrderDetail_Main', '');
                        }).get().join();

                        if (SelectedLaborderId != null && SelectedLaborderId != '') {
                            Clinical_LabOrderDetail.getLabOrderInfo(SelectedLaborderId);
                        }
                        else {
                            Clinical_LabOrderDetail.checkLabOrderExists();
                        }
                    }

                    else if (this.id == 'clinical_immunizationorder') {

                    }

                    else if (this.id == 'clinical_procedureorder') {

                        ClinicalProcedureOrderDetail.detach_ComponentsProcedureOrder('ProcedureOrder', true, true);

                        var SelectedProcedureOrderId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_ProcedureOrderDetail_Main', '');
                        }).get().join();

                        if (SelectedProcedureOrderId != null && SelectedProcedureOrderId != '') {
                            ClinicalProcedureOrderDetail.getProcedureOrderInfo(SelectedProcedureOrderId);
                        }
                        else {
                            ClinicalProcedureOrderDetail.checkProcedureOrderExists();
                        }
                    }

                    else if (this.id == 'clinical_radiologyorder') {

                        ClinicalRadiologyOrderDetail.detach_ComponentsRadiologyOrder('RadiologyOrder', true, true);

                        var SelectedRadiologyOrderId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_RadiologyOrderDetail_Main', '');
                        }).get().join();

                        if (SelectedRadiologyOrderId != null && SelectedRadiologyOrderId != '') {
                            ClinicalRadiologyOrderDetail.getRadiologyOrderInfo(SelectedRadiologyOrderId);
                        }
                        else {
                            ClinicalRadiologyOrderDetail.checkRadiologyOrderExists();
                        }
                    }

                    else if (this.id == 'clinical_consultationorder') {

                        ClinicalConsultationOrderDetail.detach_ComponentsConsultationOrder('ConsultationOrder', true, true);

                        var SelectedConsultationOrderId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_ConsultationOrderDetail_Main', '');
                        }).get().join();

                        if (SelectedConsultationOrderId != null && SelectedConsultationOrderId != '') {
                            ClinicalConsultationOrderDetail.getConsultationOrderInfo(SelectedConsultationOrderId);
                        }
                        else {
                            ClinicalConsultationOrderDetail.checkConsultationOrderExists();
                        }
                    }

                    else if (this.id == 'clinical_refferalorders') {

                    }

                    else if (this.id == 'clinical_nursingorders') {

                    }

                    else if (this.id == 'clinical_eyeexam') {

                    }

                    else if (this.id == 'clinical_hearingexam') {

                    }

                    else if (this.id == 'clinical_screeningandprevention') {

                    }

                    else if (this.id == 'clinical_immunization') {

                        Clinical_ImmunizationDetail.detach_ComponentsImmunization('Immunization', false, true);

                        var SelectedImmunizationId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Immunization_Main', '');
                        }).get().join();

                        if (SelectedImmunizationId != null && SelectedImmunizationId != '') {
                            Clinical_ImmunizationDetail.getAdministerVaccineInfo(SelectedImmunizationId, null, Clinical_Notes.params.patientID, false);
                        }
                        else {
                            Clinical_ImmunizationDetail.checkImmunizationExists();
                        }
                    }
                    else if (this.id == 'clinical_referrals') {

                        Patient_Referrals.detach_ComponentsReferral('Referrals', false, true);

                        var SelectedReferralId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Patient_Referrals_Main', '');
                        }).get().join();

                        if (SelectedReferralId != null && SelectedReferralId != '') {
                            Patient_Referrals.getReferralInfo(SelectedReferralId, null, Clinical_Notes.params.patientID, false);
                        }
                        else {
                            Patient_Referrals.checkReferralExists();
                        }
                    }

                    else if (this.id == 'clinical_attachments') {

                    }

                    else if (this.id == 'clinical_letters') {

                    }

                    else if (this.id == 'clinical_followup') {

                        Clinical_FollowUpAppointment.detach_ComponentsFollowUp('FollowUp', true, true);
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_followup').append($('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section')[0].outerHTML);
                    }
                    else if (this.id == 'clinical_referraldocuments') {

                    }

                    else if (this.id == 'clinical_forms') {

                    }

                    else if (this.id == 'clinical_patienteducation') {

                    }

                    else if (this.id == 'clinical_growthchart') {

                    }

                    else if (this.id == 'clinical_faxes') {

                    }

                    else if (this.id == 'clinical_messages') {

                    }

                    else if (this.id == 'clinical_patientdirectives') {

                    }

                    else if (this.id == 'clinical_gynecologyhx') {

                    }

                    else if (this.id == 'clinical_menstruationhx') {

                    }

                    else if (this.id == 'clinical_obcarerecord') {

                    }

                    else if (this.id == 'clinical_obmedicalhx') {

                    }

                    else if (this.id == 'clinical_pregnancyhx') {

                    }

                    else if (this.id == 'clinical_obinitialexam') {

                    }

                    else if (this.id == 'clinical_fetalultrasound') {

                    }

                    else if (this.id == 'clinical_prenatalexam') {

                    }

                    else if (this.id == 'clinical_reviewofsystems') {

                        Clinical_ReviewofSystems.detach_ComponentsReviewofSystems('clinical_reviewofsystems', true, true);

                        var SelectedReviewofSystemsId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_ReviewofSystems_Main', '');
                        }).get().join();

                        if (SelectedReviewofSystemsId != null && SelectedReviewofSystemsId != '') {
                            Clinical_ReviewofSystems.getReviewofSystemsInfo(SelectedReviewofSystemsId);
                        }
                        else {
                            Clinical_ReviewofSystems.checkReviewofSystemsExists();
                        }
                    }
                    else if (this.id == 'clinical_birthhx') {

                        Clinical_BirthHx.detach_ComponentsBirthHx('BirthHx', true, true);

                        var SelectedBirthHxId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_BirthHx_Main', '');
                        }).get().join();

                        if (SelectedBirthHxId != null && SelectedBirthHxId != '') {
                            Clinical_BirthHx.getBirthHxInfo(null, SelectedBirthHxId);
                        }
                        else {
                            Clinical_BirthHx.checkBirthHxExists();
                        }
                    }

                    else if (this.id == 'clinical_planofcare') {

                        Clinical_PlanOfCare.detach_ComponentsPlanOfCare('PlanOfCare', true, true);

                        var SelectedPlanOfCareId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_PlanOfCare_Main', '');
                        }).get().join();
                        if (SelectedPlanOfCareId != null && SelectedPlanOfCareId != '') {
                            Clinical_PlanOfCare.getPlanOfCareInfo(null, null, SelectedPlanOfCareId);
                        }
                        else {
                            Clinical_PlanOfCare.checkPlanOfCareExists();
                        }
                    }

                    else if (this.id == 'clinical_functionalandcognitive') {

                        Clinical_Cognitive.detach_ComponentsCognitive('Cognitive', true, true);

                        var SelectedCognitiveId = $('#actionPanClinicalProgressNote ' + this.id).parents('li').find('section').map(function () {
                            return this.id.replace('Cli_Cognitive_Main', '');
                        }).get().join();

                        if (SelectedCognitiveId != null && SelectedCognitiveId != '') {
                            Clinical_Cognitive.getCognitiveInfo(null, null, SelectedCognitiveId);
                        }
                        else {
                            Clinical_Cognitive.checkCognitiveExists();
                        }
                    }
                });

                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                UnloadActionPan(Clinical_Vitals.params["ParentCtrl"]);
            }, function () {
                UnloadActionPan(Clinical_Vitals.params["ParentCtrl"]);
            }, '1');
        } else {
            utility.DisplayMessages("Please select any component to copy", 2);
        }
    },

    //---------------END Progress note - Patient Notes Modal Functions
    //--------------End Notes Categories----------------------


    // ------------------------Notes template Used Functions--------------------
    /*Author: Azhar Sial, Date: 03/17/2016
    Purpose: for Notes template Tags Replacement*/
    Fill_NotesTemplateComponent: function (response, PEDataTemptId, PETemplateId, ROSDataTemptId, ROSTemplateId, HPITemplateId) {
        var defferd = $.Deferred();

        var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

        var NotesText = Clinical_Notes_detail.NoteText;
        var $CustomFormTagsList = $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Custom"]');
        var $ClinicalTagsList = $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Clinical"]');
        var $FreeTextTags = $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[data-title^="{FT}"]');

        Clinical_ProgressNote.getFreeTextHtml($FreeTextTags);
        //to make compenents sortable in case of add note, removing div under ul
        ($ClinicalTagsList.parent()).each(function () {
            if ($(this).is('div') == true) {
                //$(this).replaceWith(function () {
                //    return $('.TagInserted,span', this);
                //});
                $(this).children().unwrap();
            }
        });
        ($CustomFormTagsList.parent()).each(function () {
            if ($(this).is('div') == true) {
                //$(this).replaceWith(function () {
                //    return $('.TagInserted,span', this);
                //});
                $(this).children().unwrap();
            }
        });
        if ($ClinicalTagsList.length > 0) {

            var dupes = {
            };
            var distinctList = [];

            $.each($ClinicalTagsList, function (i, el) {

                if (!dupes[el.id]) {
                    dupes[el.id] = true;
                    distinctList.push(el);
                } else {
                    $(el).remove();
                }
            });


            var NoteObj = {
            };
            $.each(distinctList, function (index, element) {
                var valueField = element.value;
                var component = Clinical_ProgressNote.ComponentsSoapTextCall(response, valueField, "", PEDataTemptId, PETemplateId, ROSDataTemptId, ROSTemplateId, HPITemplateId, false);
                if (component) {
                    NoteObj[component.Name] = component.IDs;
                }

            });

            if (Clinical_Notes_detail.OrderSetId && Clinical_Notes_detail.OrderSetName) {

                Clinical_ProgressNote.OrderSetSOAPText(Clinical_Notes_detail.OrderSetId, Clinical_Notes_detail.OrderSetName, Clinical_Notes_detail.OrderSetComments);
            }
            //$("#ProgressNoteComponentList li.initialVisitBody").detach().appendTo('#ProgressNoteComponentList')
        }

        if ($CustomFormTagsList.length > 0) {
            $CustomFormTagsList.each(function (index, element) {

                var valueField = element.value;
                if (xhrPool.length <= 0) {
                    Clinical_ProgressNote.ComponentsSoapTextCall(response, valueField, element.id, PEDataTemptId, PETemplateId, ROSDataTemptId, ROSTemplateId, HPITemplateId, false);
                } else {
                    // to Make call regressive
                    Clinical_ProgressNote.ComponentsSoapTextCall(response, valueField, element.id, PEDataTemptId, PETemplateId, ROSDataTemptId, ROSTemplateId, HPITemplateId, false);
                }
            });

            var sess_pollInterval = 2;
            Clinical_ProgressNote.intervalProcess = setInterval('Clinical_ProgressNote.checkProcessingDBCall();', sess_pollInterval);
        }
        else {
            Clinical_ProgressNote.checkProcessingDBCall();
        }

        if ($('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted#207').length > 0) {
            var editLink = '<a id="btnEditReason" onclick="Clinical_ProgressNote.Edit_ComponentReason(this);" class="btn btn-link btn-link-print btn-xs btnPNC_Edit hidden" title="Edit" name=""><i class="fa fa-edit"></i></a>';

            var reason;
            if ($('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted#207').length > 1) {
                $.each($('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted#207'), function () {
                    var attr1 = $(this).attr('Hide');
                    if (typeof attr1 !== typeof undefined && attr1 !== false) {
                        reason = $(this);
                    }
                });
            }
            else {
                reason = $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted#207');
            }
            reason = undefined;
            var reasonLi = '';
            if ($('#reasonLi').length > 0) {
                if ($('#reasonLi').find('section').length > 0) {
                    reasonLi = '<li id="reasonLi"><span id=' + $(reason).attr('id') + ' class="TagInserted ui-sortable-handle">' + $(reason).text() + '</span>' + editLink + '<section id="AppointmentReason">' + $('#reasonLi').find('section').html() + '</sectoin>' + '</li>';
                }
                else {
                    reasonLi = '<li id="reasonLi"><span id=' + $(reason).attr('id') + ' class="TagInserted ui-sortable-handle">' + $(reason).text() + '</span>' + editLink + '</li>';
                }
                $('#reasonLi').remove();
            }
            else {
                reasonLi = '<li id="reasonLi"><span id=' + $(reason).attr('id') + ' class="TagInserted ui-sortable-handle">' + $(reason).text() + '</span>' + editLink + '</li>';
            }
            if ($(reason).next().attr('id') != "reasonLi")
                $(reasonLi).insertAfter($(reason));

            var attr = $(reason).attr('Hide');
            if (typeof attr === typeof undefined || attr === false) {
                $(reason).attr("Hide", 1);
            }

            $(reason).hide();


            // Making reason editable


            if ($('#pnlClinicalProgressNote #ProgressnoteHTML #207').length == 1) {
                var $List = $(document.createElement('ul'));
                var Id = $('#pnlClinicalProgressNote #ProgressnoteHTML #207').attr('id');
                var text = "";
                if ($($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[0]).is('span')) {
                    text = $($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[0]).html();
                }
                else {
                    text = $($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[0]).find('span').html();
                }
                //text = text.replace(/\n/g, '<br>');
                //text = text.replace(/&nbsp;/g, '');
                $List.attr('class', 'list-unstyled');
                $List.append("<li id='" + Id + "' class='sopTextEditable ui-sortable-handle editableContentli customTextArea'>"
                + "<span class='editable' style='display: block;'>" + text + " </span>"
                + "<textarea onkeyup='Clinical_ProgressNote.autoExpandField(this)' spellcheck='true' class='edit form-control' style='display: none;'></textarea>"
                + "</li>");
                if ($('#reasonLi').length == 0) {
                    $($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[0]).replaceWith($List);
                }
                else if ($('#reasonLi').length > 0) {
                    $('#reasonLi').replaceWith($List);
                }

            }
            else {
                for (i = 0; i < $('#pnlClinicalProgressNote #ProgressnoteHTML #207').length; i++) {
                    if ($($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[i]).text().trim() != "" && $($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[i]).is(":visible")) {
                        var $List = $(document.createElement('ul'));
                        var Id = $('#pnlClinicalProgressNote #ProgressnoteHTML #207').attr('id');
                        var text = "";
                        if ($($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[i]).is('span')) {
                            text = $($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[i]).html();
                        }
                        else {
                            text = $($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[i]).find('span').html();
                        }
                        //text = text.replace(/\n/g, '<br>');
                        //text = text.replace(/&nbsp;/g, '');
                        $List.attr('class', 'list-unstyled');
                        $List.append("<li id='" + Id + "' class='sopTextEditable ui-sortable-handle editableContentli customTextArea' NoteComponentId='NCDummyId'>"
                        + "<span class='editable' style='display: block;'>" + text + " </span>"
                        + "<textarea onkeyup='Clinical_ProgressNote.autoExpandField(this)' spellcheck='true' class='edit form-control' style='display: none;'></textarea>"
                        + "</li>");

                        if ($('#reasonLi').length == 0) {
                            $($('#pnlClinicalProgressNote #ProgressnoteHTML #207')[i]).replaceWith($List);
                        }
                        else if ($('#reasonLi').length > 0) {
                            $('#reasonLi').replaceWith($List);
                        }
                        // break;
                    }
                }
            }
        }
        if ($ClinicalTagsList.length > 0 || $CustomFormTagsList.length > 0) {

            var tagsChangedInterval = 500;
            var ProcessDB = {
            };
            ProcessDB.def = defferd;
            ProcessDB.func = setInterval(function () {
                if (xhrPool.length <= 0) {
                    clearInterval(ProcessDB.func);
                    ProcessDB.def.resolve('ok');

                }
            }, tagsChangedInterval);
        }
        else {
            defferd.resolve();
        }
        return defferd;
    },

    Add_NoText: function () {

        $('#ProgressnoteHTML .NotesComponent').each(function () {
            if ($(this).attr('title') == "Problems") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_Problems_Main']").length > 0 || $("#" + 'Comments_Problems').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoProblems').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoProblems').length == 0)
                        $(this).parent().after('<section id=NoProblems>No Problems.</section>');
                }

            }
            else if ($(this).attr('title') == "Allergies") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_Allergies_Main']").length > 0 || $("#" + 'Comments_Allergies').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoAllergies').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoAllergies').length == 0)
                        $(this).parent().after('<section id=NoAllergies>No Known Drug Allergies (NKDA).</section>');
                }

            }
            else if ($(this).attr('title') == "Medications") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_Medications_Main']:not(section[id='Cli_Medications_Main0'])").length > 0 || $("#" + 'Comments_Medications').find('p').length > 0) {
                        $(this).closest('.initialVisitBody').find('#NoMedications').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoMedications').length == 0) {
                        var htmlForNoMedication = '<section id="Cli_Medications_Main0"> <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="Cli_Medication_0"><i class="fa fa-edit"></i></a>' +
                            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="Cli_Medications_Main0"  ><i class="fa fa-times"></i></a></div> ' +
                            '<div id="Cli_Medication_0"><ul class="list-unstyled"><li id="NoMedications"> No Active Medications</li></ul></div></section>';
                        $(this).parent().after(htmlForNoMedication);
                    }
                }

            }
            else if ($(this).attr('title') == "Surgical Hx") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_SurgicalHx_Main']").length > 0 || $("#" + 'Comments_SurgicalHx').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoSurgicalHx').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoSurgicalHx').length == 0)
                        $(this).parent().after('<section id=NoSurgicalHx>No Surgical History.</section>');
                }

            }
            else if ($(this).attr('title') == "Social Hx") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_SocialHx_Main']").length > 0 || $("#" + 'Comments_SocialHx').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoSocialHx').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoSocialHx').length == 0)
                        $(this).parent().after('<section id=NoSocialHx>No Social History.</section>');
                }

            }
            else if ($(this).attr('title') == "Family Hx") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_FamilyHx_Main']").length > 0 || $("#" + 'Comments_FamilyHx').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoFamilyHx').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoFamilyHx').length == 0)
                        $(this).parent().after('<section id=NoFamilyHx>No Family History.</section>');
                }

            }
            else if ($(this).attr('title') == "Birth Hx") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_BirthHx_Main']").length > 0 || $("#" + 'Comments_BirthHx').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoBirthHx').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoBirthHx').length == 0)
                        $(this).parent().after('<section id=NoBirthHx>No Birth History.</section>');
                }

            }
            else if ($(this).attr('title') == "Hospitalization Hx") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_HospitalizationHx_Main']").length > 0 || $("#" + 'Comments_HospitalizationHx').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoHospitalizationHx').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoHospitalizationHx').length == 0)
                        $(this).parent().after('<section id=NoHospitalizationHx>No Hospitalization History.</section>');
                }

            }
            else if ($(this).attr('title') == "Medical Hx") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_MedicalHx_Main']").length > 0 || $("#" + 'Comments_MedicalHx').find('p').length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoMedicalHx').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoMedicalHx').length == 0)
                        $(this).parent().after('<section id=NoMedicalHx>No Medical History.</section>');
                }

            }
            else if ($(this).attr('title') == "Social, Psychological and Behavior Hx") {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("section[id*='Cli_SocPsyandBehaviorHx_Main']").length > 0) {
                    $(this).closest('.initialVisitBody').find('#NoSocPsyandBehaviorHxHx').remove();
                } else {
                    if ($(this).closest('.initialVisitBody').find('#NoSocPsyandBehaviorHxHx').length == 0)
                        $(this).parent().after('<section id=NoSocPsyandBehaviorHxHx>No Social, Psychological and Behavior History.</section>');
                }
            }
        });
    },

    Edit_ComponentReason: function (Control, Name, event) {

        var Name = "AppointmentReason";
        $Control = $(Control).closest('li');
        if ($Control.find('Section #' + Name).length == 0) {
            $Control.find('a').after('<section id=' + Name + '><div id=' + Name + '><ul class="list-unstyled"><li id= Comments_' + Name + ' class="mt-md" style="word-wrap:break-word"></li> </ul></div></section>');
        }
        if ($Control.find('textarea').length <= 0) {
            $('#Comments_' + Name).hide();
            Clinical_ProgressNote.openTinyEditor($Control, true, Name);
        }
    },

    ComponentsSoapTextCall: function (response, valueField, customFormId, PEDataTemptId, PETemplateId, ROSDataTemptId, ROSTemplateId, HPITemplateId) {

        var hideAlertMessage = true;
        valueField = valueField.trim();
        if (valueField.indexOf('Custom') >= 0) {
            var customFormName = valueField;
            valueField = 'Custom';
        }

        var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

        switch (valueField) {
            case '{{ Clinical Vitals }}':
                try {
                    var vitals = JSON.parse(response.NotesVitals_JSON);
                    var attached_vitals = JSON.parse(response.Notes_Attached_VitalSigns_JSON);
                    var vitalId = Clinical_Vitals.CreateVitalBodyHTMLFromNotes(vitals, attached_vitals, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage, true);

                    return {
                        Name: "Vitals", IDs: vitalId
                    };
                }
                catch (ex) {
                    console.log("Exception at Vitals");
                    console.log(ex);
                }
                break;
            case '{{ Clinical Problems }}':
                try {
                    var Problems = JSON.parse(response.Notes_ProblemList_JSON);
                    var Attached_Problems = JSON.parse(response.Notes_Attached_ProblemList_JSON);
                    var ProblemsListId = Clinical_ProblemLists.createProblemListBodyHTMLFromNotes(Problems, Attached_Problems, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage, true);
                    if (ProblemsListId) {
                        Clinical_ProgressNote.HideShowBillingInfo();
                    }
                    return {
                        Name: "Problems", IDs: ProblemsListId
                    };
                }
                catch (ex) {
                    console.log("Exception at Problems");
                    console.log(ex);
                }
                break;
            case '{{ Clinical Allergies }}':
                try {
                    var Allergies = JSON.parse(response.NotesAllergies_JSON);
                    var allergyIds = Clinical_Allergies.createAllergyBodyHTMLFromNotes(Allergies, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', '', hideAlertMessage, true);

                    return {
                        Name: "Allergies", IDs: allergyIds
                    };
                }
                catch (ex) {
                    console.log("Exception at Allergies");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Social Hx }}':
                try {
                    var SocialHistory = JSON.parse(response.NotesSocialHistory_JSON);
                    var SocialHxids = Clinical_SocialHx.createSocialHxBodyHTMLFromNotes(SocialHistory, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);

                    return {
                        Name: "SocialHx", IDs: SocialHxids
                    };
                }
                catch (ex) {
                    console.log("Exception at Social Hx");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Medical Hx }}':
                try {
                    var MedicalHistory = JSON.parse(response.NotesMedicalHistory_JSON);
                    var MedicalHxIds = Clinical_MedicalHx.createMedicalHxBodyHTMLFromNotes(MedicalHistory, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);
                    return {
                        Name: "MedicalHx", IDs: MedicalHxIds
                    };
                }
                catch (ex) {
                    console.log("Exception at Medical Hx");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Birth Hx }}':
                try {
                    var BirthHistory = JSON.parse(response.NotesBirthHistory_JSON);
                    var BirthHxIds = Clinical_BirthHx.createBirthHxBodyHTMLFromNotes(BirthHistory, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);

                    return {
                        Name: "BirthHx", IDs: BirthHxIds
                    };
                }
                catch (ex) {
                    console.log("Exception at Birth Hx");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Immunization }}':

                try {
                    var Vaccines = JSON.parse(response.Notes_Immunization_Vaccines_JSON);
                    var Injections = JSON.parse(response.Notes_Immunization_Injections_JSON);
                    var Attached_Vaccines = JSON.parse(response.Notes_Attached_Immunization_Vaccines_JSON);
                    var Attached_Injections = JSON.parse(response.Notes_Attached_Immunization_Injections_JSON);
                    Clinical_ImmunizationDetail.getLatestImmunizationByPatientId(Vaccines, Injections, Attached_Vaccines, Attached_Injections, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true, true);
                }
                catch (ex) {
                    console.log("Exception at Immunization");
                    console.log(ex);
                }

                break;

                //case '{{ Patient Referrals }}':
                //    Patient_Referrals.getLatestReferralByPatientId(hideAlertMessage);
                //    break;

            case '{{ Clinical Family Hx }}':
                try {
                    var FamilyHistory = JSON.parse(response.NotesFamilyHistory_JSON);
                    var FamilyHxids = Clinical_FamilyHx.createFamilyHxBodyHTMLFromNotes(FamilyHistory, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);
                    return {
                        Name: "FamilyHx", IDs: FamilyHxids
                    };
                }
                catch (ex) {
                    console.log("Exception at Family Hx");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Surgical Hx }}':
                try {
                    var SurgicalHistory = JSON.parse(response.NotesSurgicalHistory_JSON);
                    var surgicalhxIds = Clinical_SurgicalHx.createSurgicalHxBodyHTMLFromNotes(SurgicalHistory, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);
                    return {
                        Name: "SurgicalHx", IDs: surgicalhxIds
                    };
                }
                catch (ex) {
                    console.log("Exception at Surgical Hx");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Hospitalization Hx }}':
                try {
                    var HospitalizationHistory = JSON.parse(response.NotesHospitalizationHistory_JSON);
                    var HhxIds = Clinical_HospitalizationHx.createHospitalizationHxBodyHTMLFromNotes(HospitalizationHistory, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);
                    return {
                        Name: "HospitalizationHx", IDs: HhxIds
                    };
                }
                catch (ex) {
                    console.log("Exception at Hospitalization Hx");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Complaints }}':
                try {
                    var Complaints = JSON.parse(response.NotesComplaints_JSON);
                    var NewComplaintId = response.NewComplaintId;
                    var HPINotesFindings = JSON.parse(response.NotesHPIFindings_JSON);
                    var NotesHPITemplate = JSON.parse(response.NotesHPITemplate_JSON);
                    var isPatientComplaintExists = response.IsPatientComplaintExists;
                    if (isPatientComplaintExists) {
                        if (globalAppdata["IsDefaultHPI"] == "True") {
                            Clinical_HPIComplaints.ComplaintId = NewComplaintId;
                            var Complaintids = Clinical_HPIComplaints.createComplaintBodyHTML("", Complaints.Complaints, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true, NewComplaintId);
                            Clinical_HPIComplaints.CreateHPITemplateHTMLForNotes(HPINotesFindings, NotesHPITemplate, true);
                        }
                        else {
                            var Complaintids = Clinical_Complaints.createComplaintBodyHTML("", Complaints.Complaints, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
                        }
                        var freeText = Clinical_ProgressNote.GetPrevComplaintFreeText(response.PreviousComplaintSoapText);
                        if (freeText) {
                            var cId = Complaints.Complaints[0].Expr1;
                            var newId = 'Comments_Cli_Complaint_' + cId;
                            var prevId = $(freeText).attr('id').replace('Comments_Cli_Complaint_', '');
                            $('#Cli_Complaint_' + cId).find('ul').append(freeText);
                            var freetextLi = $('#Cli_Complaint_' + cId).find('ul > li#Comments_Cli_Complaint_' + prevId);
                            if (freetextLi.length > 0) {
                                $(freetextLi).attr('id', newId);
                            }
                        }
                    }
                    else if (globalAppdata["IsDefaultHPI"] == "True" && HPITemplateId) {
                        Clinical_HPIComplaints.ComplaintId = NewComplaintId;
                        Clinical_HPIComplaints.CreateHPITemplateHTMLForNotes(HPINotesFindings, NotesHPITemplate, true);
                    }

                    return {
                        Name: "Complaints", IDs: Complaintids
                    };
                }
                catch (ex) {
                    console.log("Exception at Complaints");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Medication }}':
                try {
                    var Medications = JSON.parse(response.Notes_Medications_JSON);
                    var AttachedMedications = JSON.parse(response.Notes_Attached_Medications_JSON);

                    Clinical_Medications.createMedicationsBodyHTMLFromNotes(Medications, AttachedMedications, hideAlertMessage, true);
                }
                catch (ex) {
                    console.log("Exception at Medication");
                    console.log(ex);
                }
                break;

            case '{{ Clinical Prescription }}':
                try {
                    var Prescription = JSON.parse(response.NotesPrescription_JSON);
                    //var Prescriptionid = Clinical_Prescriptions.createPrescriptionBodyHTMLFromNotes(Prescription, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);
                    Clinical_Prescriptions.checkPrescriptionExists();
                    return {
                        Name: "Prescription", IDs: Prescription
                    };
                }
                catch (ex) {
                    console.log("Exception at Prescription");
                    console.log(ex);
                }
                break;
            case '{{ Clinical Procedures }}':
                var Procedures = JSON.parse(response.Notes_Procedures_JSON);
                var AttachedProcedures = JSON.parse(response.Notes_Attached_Procedures_JSON);
                var ProcedureId = Clinical_Procedures.createProceduresBodyHTMLForNotes(Procedures, AttachedProcedures, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);
                if (ProcedureId) {
                    Clinical_ProgressNote.HideShowBillingInfo();
                }
                return {
                    Name: "Procedures", IDs: ProcedureId
                };

                break;
            case '{{ Clinical Lab Order }}':
                if (Clinical_Notes_detail.OrderSetId) {
                    var response = JSON.parse(response.Notes_LabOrder_JSON);
                    if (response.status != false)
                        Clinical_LabOrder.createMedicationBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, null, true);
                }

                break;
            case '{{ Clinical Review of System }}':

                try {
                    if ((Clinical_Notes.params.mode == "Add" || Clinical_Notes.params.mode == "Edit") && ROSDataTemptId && ROSDataTemptId != null && ROSTemplateId != null && ROSDataTemptId != "") {

                        var soapText = null;
                        var ROSSystemInfoID = 0;

                        var ros = JSON.parse(response.NotesReviewofSystem_JSON);
                        if (ros) {
                            soapText = ros.SoapText;
                            ROSSystemInfoID = ros.ROSSystemInfoID;
                        }

                        // Clinical_ReviewofSystems.createReviewofSystemsBodyHTMLFromnotes(soapText, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, ROSSystemInfoID, true);
                        Clinical_ROSTemplateDetailRevamp.createReviewofSystemsBodyHTMLFromnotes(soapText, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, ROSSystemInfoID, true);
                    }
                }
                catch (ex) {
                    console.log("Exception at Review of System");
                    console.log(ex);
                }

                break;

            case '{{ Clinical Physical Exam }}':
                try {
                    if ((Clinical_Notes.params.mode == "Add" || Clinical_Notes.params.mode == "Edit")) {
                        PhysicalExamTemplatesRevamp.LoadPhysicalExamForNotes(response, true);
                    }
                }
                catch (ex) {
                    console.log("Exception at Physical Exam");
                    console.log(ex);
                }
                //  Clinical_Procedures.getLatestProceduresByPatientId();
                break;
            case '{{ Clinical Radiology Order }}':
            case '{{ Clinical Diagnostic Imaging Order }}':
                if (Clinical_Notes_detail.OrderSetId) {
                    var response = JSON.parse(response.Notes_DiagnosticImagingOrder_JSON);
                    if (response.status != false)
                        Clinical_RadiologyOrder.createRadiologyBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, null, true);

                }
                break;
            case '{{ Clinical Consultation Order }}':
                //  Clinical_Procedures.getLatestProceduresByPatientId();
                break;
            case '{{ Clinical Procedure Order }}':
                if (Clinical_Notes_detail.OrderSetId) {
                    var response = JSON.parse(response.Notes_ProcedureOrder_JSON);
                    if (response.status != false)
                        Clinical_ProcedureOrder.createProcedureOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, null, true);
                }
                break;
            case '{{ Clinical Follow Up }}':
                //  Clinical_Procedures.getLatestProceduresByPatientId();
                if (Clinical_ProgressNote.DefaultOrderSetID && response.FollowUp) {

                    var FollowUp = JSON.parse(response.FollowUp);

                    if (FollowUp.FollowUpText || FollowUp.Comments) {
                        Clinical_FollowUpAppointment.params["OrderSetId"] = FollowUp.OrderSetId;

                        var cval = "";
                        var ctype = "";
                        var Soap = FollowUp.FollowUpText.replace("Patient needs to be seen again in", "").trim().split(" ");
                        if (Soap.length == 2) {
                            cval = Soap[0];
                            ctype = Soap[1];
                        }
                        Clinical_FollowUpAppointment.CreateFollowUp_SOAP_TextProgressNote(FollowUp.FollowUpText, FollowUp.Comments, true, cval, ctype);
                    }

                }
                break;
            case '{{ Clinical Patient Education }}':
                if (Clinical_ProgressNote.DefaultOrderSetID && response.PatientEducationSoapCount > 0) {
                    Clinical_PatientEducation.createPatientEducationBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true, true);
                }
                break;
            case '{{ Clinical Referrals }}':
                try {
                    Patient_Referrals.checkReferralExists();
                    if (Clinical_ProgressNote.DefaultOrderSetID) {
                        Patient_Referrals.getLatestReferralByPatientId(hideAlertMessage, true, true);
                    }
                }
                catch (ex) {
                    console.log("Exception at Referrals");
                    console.log(ex);
                }
                break;
            case '{{ Clinical Functional and Cognitive }}':
                //Clinical_Cognitive.getLatestCognitiveByPatientId(hideAlertMessage);
                break;
            case '{{ Clinical Diagnostic Imaging Results }}':
            case '{{ Clinical Radiology Results }}':
                try {
                    Clinical_RadiologyOrder.checkResultExists();
                    //Clinical_RadiologyOrder.getLatestRadiologyResultByPatientId(hideAlertMessage, true);
                }
                catch (ex) {
                    console.log("Exception at Diagnostic Imaging Results");
                    console.log(ex);
                }
                break;
            case '{{ Clinical Lab Results }}':
                try {
                    Clinical_LabOrder.checkResultExists();
                    // Clinical_LabOrder.getLatestLabResultByPatientId(hideAlertMessage, true);
                }
                catch (ex) {
                    console.log("Exception at Lab Results");
                    console.log(ex);
                }
                break;
            case 'Custom':
                try {
                    Clinical_CustomForms.getCustomFormForNotes(customFormName, customFormId, true);
                }
                catch (ex) {
                    console.log("Exception at Custom");
                    console.log(ex);
                }
                break;
            case '{{ Clinical Treatment Plan }}':
                try {
                    if (globalAppdata["IsPrevNoteTreatmentComents"] == "True") {
                        var SoapText = ""
                        if (response.TreatmentPlanComment) {
                            var res = JSON.parse(response.TreatmentPlanComment);
                            SoapText = res.SoapText;
                        }
                        Clinical_Treatment.createTreatmentBodyHTML(null, null, true, SoapText);
                    }
                    else {
                        Clinical_Treatment.createTreatmentBodyHTML(null, null, true);
                    }
                }
                catch (ex) {
                    console.log("Exception at Treatment");
                    console.log(ex);
                }
                break;
            case '{{ Clinical Social, Psychological and Behavior Hx }}':
                try {
                    var SocialandBehaviorHx = JSON.parse(response.NotesSocPsyandBehaviorHx_JSON);
                    var SocialandBehaviorHxId = Clinical_SocPsyandBehaviorHx.createSocPsyandBehaviorHxBodyHTMLFromDefaultNotes(SocialandBehaviorHx, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true);
                    return {
                        Name: "SocPsyandBehaviorHx", IDs: SocialandBehaviorHxId
                    };
                }
                catch (ex) {
                    console.log("Exception at Social, Psychological and Behavior Hx");
                    console.log(ex);
                }
                break;
            default:
                break;
        }
        // findInDiv.hide(true);
    },
    GetPrevComplaintFreeText: function (soaptext) {

        var soap = $(soaptext).find('div [id^=Comments_Cli_Complaint_]');
        if (soap.length > 0) {
            soap = soap[0].outerHTML;
        }
        else {
            soap = '';
        }
        return soap;
    },
    getFreeTextHtml: function ($elem) {
        var insertedDiv = 0;
        var $Divs = [];
        $.each($elem, function (i, $item) {
            $($item).wrap('<div></div>');

            if ($($($item).parents('div')[0]).attr('id') == undefined) {
                insertedDiv++;
                $($($item).parents('div')[0]).attr('id', 'FreeTexts' + insertedDiv + '');
                $Divs.push($($item).parents('div')[0]);
            }

        });


        $.each($Divs, function (i, $item) {
            var Id = $($item).attr('id');
            var text = $($item).text().substring($($item).text().indexOf('{FT}') + 4, $($item).text().indexOf('{/FT}'));
            //   var $List = $(document.createElement('li'));
            text = text.replace(/\n/g, '<br>');
            text = text.replace(/&nbsp;/g, '');
            text = text.replace(/&amp;/g, '&');
            text = text.replace(/&apos;/, "'");
            //  $List.attr('class', 'list-unstyled');
            var $List = $("<li id='" + Id + "' class='sopTextEditable ui-sortable-handle editableContentli FreeTextComponent' NoteComponentId='NCDummyId'>"
            + "<span class='editable' style='display: block;'>" + text + " </span>"
            + "<textarea onkeyup='Clinical_ProgressNote.autoExpandField(this)' spellcheck='true' class='edit form-control' style='display: none;'></textarea>"
            + "</li>");

            $($item).replaceWith($List);
            if ($($($item).parents('div')[0]).attr('id') == undefined) {
                $($($item).parents('div')[0]).attr('id', 'FreeTexts');
            }
        });





    },

    checkProcessingDBCall: function () {

        if ($('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Clinical"]').length > 0) {
            $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Clinical"]').each(function (index, element) {

                var valueField = element.value;
                valueField = valueField.trim();

                switch (valueField) {

                    case '{{ Clinical Vitals }}':
                        // do stuff
                        Clinical_ProgressNote.checkComponentExists('clinical_vitals', 'Vitals');
                        if ($('clinical_vitals').closest('li').length > 0) {

                            $(element).replaceWith($('#ProgressnoteHTML clinical_vitals').closest('li'));
                        }
                        // $("#ProgressNoteComponentList li.initialVisitBody").detach().appendTo('#ProgressNoteComponentList')
                        break;

                    case '{{ Clinical Immunization }}':

                        Clinical_ProgressNote.checkComponentExists('Clinical_Immunization', 'Immunization');
                        if ($('Clinical_Immunization').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML Clinical_Immunization').closest('li'));
                        }

                        break;

                    case '{{ Clinical Problems }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_problems', 'Problems');
                        if ($('clinical_problems').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_problems').closest('li'));
                        }

                        break;
                    case '{{ Clinical Allergies }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_allergies', 'Allergies');
                        if ($('clinical_allergies').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_allergies').closest('li'));
                        }

                        break;
                    case '{{ Clinical Social Hx }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_socialhx', 'Social Hx');
                        if ($('clinical_socialhx').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_socialhx').closest('li'));
                        }

                        break;
                    case '{{ Clinical Medical Hx }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_medicalhx', 'Medical Hx');
                        if ($('clinical_medicalhx').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_medicalhx').closest('li'));
                        }

                        break;
                    case '{{ Clinical Birth Hx }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_birthhx', 'Birth Hx');
                        if ($('clinical_birthhx').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_birthhx').closest('li'));
                        }

                        break;
                    case '{{ Clinical Family Hx }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_familyhx', 'Family Hx');
                        if ($('clinical_familyhx').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_familyhx').closest('li'));
                        }

                        break;
                    case '{{ Clinical Surgical Hx }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_surgicalhx', 'Surgical Hx');
                        if ($('clinical_surgicalhx').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_surgicalhx').closest('li'));
                        }

                        break;
                    case '{{ Clinical Hospitalization Hx }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_hospitalizationhx', 'Hospitalization Hx');
                        if ($('clinical_hospitalizationhx').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_hospitalizationhx').closest('li'));
                        }

                        break;
                    case '{{ Clinical Complaints }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_complaints', 'Complaints');
                        if ($('clinical_complaints').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_complaints').closest('li'));
                        }

                        break;
                    case '{{ Clinical Medication }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_medications', 'Medications');
                        if ($('clinical_medications').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_medications').closest('li'));
                        }

                        break;
                    case '{{ Clinical Prescription }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_prescription', 'Prescription');
                        if ($('clinical_prescription').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_prescription').closest('li'));
                        }

                        break;
                    case '{{ Clinical Procedures }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_procedures', 'Procedures');
                        if ($('clinical_procedures').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_procedures').closest('li'));
                        }
                        break;
                    case '{{ Clinical Review of System }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_reviewofsystems', 'Review of Systems');
                        if ($('clinical_reviewofsystems').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_reviewofsystems').closest('li'));
                        }
                        break;
                    case '{{ Clinical Physical Exam }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_physicalexam', 'Physical Exam');
                        if ($('clinical_physicalexam').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_physicalexam').closest('li'));
                        }
                        break;

                    case '{{ Clinical Cognitive }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_functionalandcognitive', 'Cognitive');
                        if ($('clinical_functionalandcognitive').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_functionalandcognitive').closest('li'));
                        }
                        break;
                    case '{{ Clinical Diagnostic Imaging Order }}':
                    case '{{ Clinical Radiology Order }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_radiologyorder', 'Diagnostic Imaging Order');
                        if ($('clinical_radiologyorder').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_radiologyorder').closest('li'));
                        }
                        break;

                    case '{{ Clinical Lab Orders }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_laborders', 'Lab Orders');
                        if ($('clinical_laborders').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_laborders').closest('li'));
                        }
                        break;

                    case '{{ Clinical Lab Results }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_labresults', 'Lab Results');
                        if ($('clinical_labresults').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_labresults').closest('li'));
                        }
                        break;

                    case '{{ Clinical Radiology Results }}':
                    case '{{ Clinical Diagnostic Imaging Results }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_diagnosticimagingresults', 'Diagnostic Imaging Results');
                        if ($('clinical_diagnosticimagingresults').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_diagnosticimagingresults').closest('li'));
                        }
                        break;

                    case '{{ Clinical Consultation Order }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_consultationorder', 'Consultation Order');
                        if ($('clinical_consultationorder').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_consultationorder').closest('li'));
                        }
                        break;
                    case '{{ Clinical Procedure Order }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_procedureorder', 'Procedure Order');
                        if ($('clinical_procedureorder').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_procedureorder').closest('li'));
                        }
                        break;
                    case '{{ Clinical Follow Up }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_followup', 'Follow Up');
                        if ($('clinical_followup').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_followup').closest('li'));
                        }
                        break;
                    case '{{ Clinical Lab Order }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_laborders', 'Lab Order');
                        if ($('clinical_laborders').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_laborders').closest('li'));
                        }
                        break;
                    case '{{ Clinical Patient Education }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_patienteducation', 'Patient Education');
                        if ($('clinical_patienteducation').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_patienteducation').closest('li'));
                        }
                        break;
                    case '{{ Clinical Referrals }}':
                        Clinical_ProgressNote.checkComponentExists('clinical_referrals', 'Referrals');
                        if ($('clinical_referrals').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_referrals').closest('li'));
                        }
                        break;

                    case '{{ Clinical Functional and Cognitive }}':

                        Clinical_ProgressNote.checkComponentExists('clinical_functionalandcognitive', 'Functional and Cognitive');

                        if ($('clinical_functionalandcognitive').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_functionalandcognitive').closest('li'));
                        }

                        break;

                    case '{{ Clinical Notes Extra Info }}':

                        Clinical_ProgressNote.checkComponentExists('clinical_notesextrainfo', 'Notes Extra Info');

                        if ($('clinical_notesextrainfo').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_notesextrainfo').closest('li'));
                        }
                        break;
                    case '{{ Clinical Social, Psychological and Behavior Hx }}':
                        // do stuff
                        Clinical_ProgressNote.checkComponentExists('clinical_SocPsyandBehaviorHx', 'SocPsyandBehaviorHx');
                        if ($('clinical_SocPsyandBehaviorHx').closest('li').length > 0) {

                            $(element).replaceWith($('#ProgressnoteHTML clinical_SocPsyandBehaviorHx').closest('li'));
                        }
                        break;
                    case '{{ Clinical Treatment Plan }}':

                        Clinical_ProgressNote.checkComponentExists('clinical_treatment', 'Treatment');

                        if ($('clinical_treatment').closest('li').length > 0) {
                            $(element).replaceWith($('#ProgressnoteHTML clinical_treatment').closest('li'));
                        }
                        break;

                    default:
                        break;
                }
            });


            // finish loader only when there is no more Ajax call.
            if (xhrPool.length <= 0) {
                clearInterval(Clinical_ProgressNote.intervalProcess);
                $('#pnlClinicalProgressNote #ProgressnoteHTML p').replaceWith(function () {
                    if (!$(this).parent().is('li')) {
                        if ($(this).find('.initialVisitBody').length > 0) {
                            return $(this).html()
                        } else {
                            return $('<li/>', {
                                html: $(this).html()
                            });
                        }
                    }
                    else {
                        return $('<p/>', {
                            html: $(this).html()
                        });
                    }
                });

                //end Binding Edit/remove handler to components
            }

            //var html;
            //$('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted').each(function (index, element) {
            //    if ($(element).find('li.initialVisitBody') != null && $(element).find('li.initialVisitBody').length > 0) {
            //        html = $(element).find('li.initialVisitBody').map(function () { return this.outerHTML }).get().join('');

            //        $(element).replaceWith($(html));
            //    }
            //});
        } else {
            clearInterval(Clinical_ProgressNote.intervalProcess);
            $('#pnlClinicalProgressNote #ProgressnoteHTML p').replaceWith(function () {
                if (!$(this).parent().is('li')) {
                    if ($(this).find('.initialVisitBody').length > 0) {
                        return $(this).html()
                    } else {
                        return $('<li/>', {
                            html: $(this).html()
                        });
                    }
                }
                else {
                    return $('<p/>', {
                        html: $(this).html()
                    });
                }
            });
        }

        if ($('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Custom"]').length > 0) {
            Clinical_ProgressNote.checkComponentExists('clinical_customform', 'CustomForm');
            if ($('clinical_customform').closest('li').length > 0) {
                $('#pnlClinicalProgressNote #ProgressnoteHTML .TagInserted[value^="{{ Custom"]').each(function (index, element) {
                    $(element).replaceWith($('clinical_customform').get(index).closest('li'));
                });
            }
        }
    },
    openProblemLists: function () {

        var params = [];
        var PanelID = "";
        params["NoteId"] = Clinical_ProgressNote.params.NotesId;
        params["FromAdmin"] = "0";
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
        LoadActionPan('Clinical_NotesProblemLists', params);

    },
    checkComponentExists: function (component, componentTitle) {
        var title = componentTitle.split(' ').join('');
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML ' + component).length == 0) {
            if (componentTitle.toLowerCase() == 'complaints' && globalAppdata["IsDefaultHPI"] == "True") {
                title = "HPIComplaints";
            }
            if (title == "ReviewofSystems") {
                title = "ReviewofSystemsRevmap";
            }
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #ProgressNoteComponentList').append(' <li class="' + (componentTitle == "Functional and Cognitive" ? componentTitle = "Functional And Cognitive" : componentTitle).replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                        '<' + component + ' title="' + componentTitle + '"  id="' + 1 + '" class="NotesComponent ' + component + '">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + title + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + componentTitle + '">' + componentTitle + '</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + componentTitle.split(' ').join('') + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + componentTitle.split(' ').join('') + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</' + component + '> </header></li>');
        }
        else {
            if (title == "NotesExtraInfo")
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent').removeClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML ' + component).parent().parent().removeClass('hidden');
        }
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },
    // -----------------------end Notes template Used Functions----------------

    /*Author: M Ahmad Imran, Date: 04/26/2016
    Purpose: for Notes HL7 creation*/
    SelectSyndromicSurveillance: function (SyndromicType) {

        Clinical_ProgressNote.NotesHL7CreationDBcall(SyndromicType).done(function (response) {
            response = JSON.parse(response);

            if (response.status == true) {
                utility.DisplayMessages(response.Message, 1);
                var patientId = Clinical_ProgressNote.params.patientID;
                var uri = '';
                var dt = new Date();
                var strMimeType = "application/octet-stream";
                var dateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate() + '/' + dt.getHours() + '/' + dt.getMinutes() + '/' + dt.getSeconds();
                download(uri + response.HL7Message, +patientId + "[Syndromic]-" + dateString.replace(/\//g, '') + ".txt", strMimeType);

            }
            else {
                utility.DisplayMessages(response.Message, 4);
            }

        });
    },
    NotesHL7CreationDBcall: function (SyndromicType) {
        var objData = {
        };
        objData["SyndromicType"] = SyndromicType;
        if (Clinical_Notes.params.patientID != null) {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        if (Clinical_Notes.params.NotesId != null) {
            objData["NotesId"] = Clinical_Notes.params.NotesId
        }
        objData["ProviderId"] = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #frmClinicalProgressNote #hfProviderId').val();
        objData["commandType"] = "CREATE_NOTES_HL7";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    GetIndependentMacros: function () {
        var data = {};
        data["commandType"] = "Search_MacroDetailsForNotes";
        var obj = JSON.stringify(data);
        return MDVisionService.APIService(obj, "Macro", "Macro");
    },
    InitializeMacros: function () {
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #MacrosPanel').show();
        Clinical_ProgressNote.GetIndependentMacros().done(function (response) {
            response = JSON.parse(response);
            $.each(response.MacroDetails, function (i, item) {
                var button = '<button class="btn btn-xs btn-default tab_space" onclick="Clinical_ProgressNote.BindMacroToField(\'' + item.MacroId + '\',\'' + item.MacroName + '\',\'' + item.Description + '\'">' + item.MacroName + '</button>';
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #MacroButtons').append(button);
            });
        });
    },
    BindMacroToField: function (MacroId, MacroName, MacroDescription) {
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML .editableContentli').val($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML .editableContentli').val() + ' ' + MacroDescription);
    },
    initilizeEditableContent: function () {
        var $ProgressNoteDiv = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
        var clickedUL = null;
        //find li within the specic area
        $ProgressNoteDiv.on('click', ".defaultli", function () {

            clickedUL = $(this).closest('ul').attr('id');
            var freeTextClass = "FreeTextComponent";
            if (clickedUL == "PhoneEncounterData")
                freeTextClass = "CallSummaryTextArea"

            var currentindex = $(this).index();
            var $dynamicliItem = $('<li class="sopTextEditable ui-sortable-handle editableContentli ' + freeTextClass + '" NoteComponentId="NCDummyId" style="width: auto; right: auto; height: auto; bottom: auto;visibility: visible;"> </li>');
            var span = $('<span class="editable p-sm"></span>');
            var textBox = $('<textarea spellcheck="true" class="edit form-control"/>');

            Clinical_ProgressNote.InitializeMacros(textBox);
            $dynamicliItem.append(span);
            $dynamicliItem.append(textBox);

            if ($(this).closest("#PhoneEncounterData").length > 0) {
                $dynamicliItem.insertBefore($(this));
            } else {
                if ($(this).closest("#ProgressnoteHTML").find("h4.green").length == $(this).closest("#ProgressnoteHTML").find("h4.green.hidden").length && $(this).closest("#ProgressnoteHTML").find("h4.green").length > 0) {
                    if ($(this).closest("#ProgressnoteHTML").find("h4.green").length == $(this).closest("#ProgressnoteHTML").find("h4.green.hidden").length) {
                        $(this).closest("#ProgressnoteHTML").find('#ProgressNoteComponentList').prepend($dynamicliItem);
                    }
                }
                else
                    $dynamicliItem.insertBefore($(this));
            }
            $(this).removeClass('placeholder-free-text');
            textBox.focus();
        });

        $ProgressNoteDiv.on('click', ".editableContentli", function (event) {
            if (event.target.nodeName.toLowerCase() != "textarea") {
                var spans = $(this).find('.editable'); $(spans).hide().siblings(".edit").show().val($(spans).html().replace(/&nbsp;/g, '').replace(/&amp;/g, '&').replace(/&apos;/, "'").replace(/<br\s*[\/]?>/gi, "\n").replace(/(<([^>]+)>)/ig, "")).focus();
            }
        });
        $ProgressNoteDiv.on('click', ".editable", function () {
            $(this).hide().siblings(".edit").show().text($(this).html().replace(/&nbsp;/g, '').replace(/<br\s*[\/]?>/gi, "\n").replace(/(<([^>]+)>)/ig, "")).focus();
        });
        $ProgressNoteDiv.on('focusout', ".edit", function () {
            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #MacrosPanel').find(':focus').length > 0) {
                return;
            }
            var ccText = $(this).val();
            var $li = $(this).closest('li');

            if ($(this).parents('div').attr('id') != undefined) {
                if ($(this).parents('div').attr('id').indexOf('Complaint') > 0) {
                    var ccText = $(this).val();
                    var ccNum = parseInt($(this).closest('li').attr('id'));
                    var ccDetailId = parseInt($(this).closest('li').attr('complaintdetailid'));
                    $(this).closest('li').siblings('li #liHpi').children("p").children("span").each(function (i, e) {
                        if (i == ccNum) {
                            $(e).find('span').text(ccText);
                        }
                    });
                    var objData = {
                    };
                    objData["ComplaintDescription"] = ccText;
                    objData["ComplaintId"] = parseInt($li.attr('complaintdetailid'));
                    objData["ComplaintDetailId"] = parseInt($li.attr('complaintdetailid'));
                    var data = [];
                    data.push(objData);
                    if (ccText == "") {

                        Clinical_Complaints.DeleteComplaint(ccDetailId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_Complaints.LoadComplaints().done(function (response) {
                                    response = JSON.parse(response);
                                    var overAllComments = JSON.parse(response.ComplainteLoad_JSON).OverallComments;

                                    Clinical_Complaints.createComplaintBodyHTML(JSON.parse(response.ComplainteLoad_JSON)[0].OverallComments, JSON.parse(response.ComplainteDetailLoad_JSON), '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
                                });
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        Clinical_Complaints.UpdateComplaintFromNotes(data).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status == false) {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }
            }
            if ($(this).is(":visible") && $li.find('.editable').html() != ccText) {
                $(this).hide().siblings(".editable").show();
                $(this).siblings(".editable").css("display", "block");
                $(this).siblings(".editable").html(ccText.replace(/\r?\n/g, '<br/>'));

                var $ul = $(this).closest('ul').attr('id');
                var previousUL = null;
                $.each($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML ul'), function (i, item) {
                    if ($('#pnlClinicalProgressNote #NoteTemplate option:selected').text().trim() != "- Blank -") {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML ul#' + $ul + ' li.sopTextEditable.ui-sortable-handle.defaultli').addClass('placeholder-free-text');
                    } else {
                        if ($(this).attr('id') == $ul) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML ul#' + previousUL + ' li').first().addClass('placeholder-free-text');
                        }
                    }
                    previousUL = $(this).attr('id');


                });
                if ($(this).closest('li').hasClass('customTextArea')) {
                    Clinical_ProgressNote.saveComponentSOAPText('Free Text', null, this, null, null, true);
                }
                else {
                    if ($(this).parents('div').attr('id').indexOf('Complaint') > 0)
                        Clinical_ProgressNote.saveComponentSOAPText('Complaints', null);
                    else if ($(this).closest('li').hasClass('CallSummaryTextArea'))
                        Clinical_ProgressNote.saveComponentSOAPText('Free Text', null, this, null, null, false, null, true);
                    else
                        Clinical_ProgressNote.saveComponentSOAPText('Free Text', null, this);
                }
            } else if (ccText == "") {
                var $ul = $(this).closest('ul').attr('id');
                $(this).addClass('placeholder-free-text');
                $(this).hide().siblings(".editable").show();
                $(this).siblings(".editable").css("display", "block");
                $li.remove();
                if (clickedUL == "ProgressNoteComponentList") {
                    //$('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML ul#' + clickedUL).children().addClass('placeholder-free-text');
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #ProgressNoteComponentList li.sopTextEditable.ui-sortable-handle.defaultli").addClass('placeholder-free-text');
                } else {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML ul#' + clickedUL + ' li').first().addClass('placeholder-free-text');
                }
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            } else {
                $(this).hide().siblings(".editable").show();
                $(this).siblings(".editable").css("display", "block");
            }

        });

    },

    InitializeProgressNoteComponent: function () {
        $("#pnlClinicalProgressNote #NotesComponentList #NotesComponentList li:not(.nav-parent)").draggable({
            //  revert: true,
            revert: "invalid",
            appendTo: 'body',
            connectToSortable: "#pnlClinicalProgressNote #ProgressNoteComponentList",
            containment: '#pnlClinicalProgressNote',
            helper: function (ev, ui) {
                var droppedComponent_Text = this.textContent;
                var title = $(this.childNodes).attr('title').split(" ").join("");
                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                else if (this.id.indexOf('clinicalMenu_Medical_HPIComplaints') >= 0) {
                    title = "HPIComplaints";
                }
                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).parent().parent().hasClass('hidden')) {
                    //$('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).parent().parent()[0].attr("NoteComponentId","NCDummyId");

                    if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).parent().parent().length > 0) {
                        return $('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).parent().parent()[0].outerHTML;
                    }
                    else {
                        return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component"  NoteComponentId="NCDummyId" > <header>' +
     '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + $(this.childNodes).attr('title') + '"  id="' + this.id + '" class="NotesComponent">' +
     '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + title + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + $(this.childNodes).attr('title') + '">' + droppedComponent_Text + '</a> ' +
'<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + $(this.childNodes).attr('title').split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                     '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + $(this.childNodes).attr('title') + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';
                    }
                }
                else {
                    return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component"  NoteComponentId="NCDummyId" > <header>' +
                         '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + $(this.childNodes).attr('title') + '"  id="' + this.id + '" class="NotesComponent">' +
                         '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + title + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + $(this.childNodes).attr('title') + '">' + droppedComponent_Text + '</a> ' +
                  '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + $(this.childNodes).attr('title').split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                         '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + $(this.childNodes).attr('title') + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                    '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';
                }
            },
            stack: '#pnlClinicalProgressNote #ProgressnoteHTML',
            start: function (event, ui) {
                var droppedComponent_Text = this.textContent;
                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).parent().parent().hasClass('hidden')) {

                }
                    // if component is already dropped on Progress note, than stop user to not drag again the same component
                else if (!$('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).length == 0) {
                    return false;
                }
                $(this).css('z-index', '10');
            },
            stop: function (ev, ui) {
                $(this).css('z-index', '10');
                ui.helper.css('height', 'auto');
                ui.helper.css('width', 'auto');
                var droppedComponent = this.textContent;
                var thisid = this.id;
                if (droppedComponent == 'Procedure') {
                    droppedComponent = $(this.childNodes).attr('title');
                }
                else if (droppedComponent == 'Consultation') {
                    droppedComponent = $(this.childNodes).attr('title');
                }
                else if (droppedComponent == 'Radiology') {
                    droppedComponent = $(this.childNodes).attr('title');
                }
                else if (droppedComponent == 'Lab') {
                    droppedComponent = $(this.childNodes).attr('title');
                }

                if (droppedComponent == "Procedure") {
                    droppedComponent = "Procedure Order";
                }
                else if (droppedComponent == "Radiology" || droppedComponent == "Diagnostic Imaging") {
                    droppedComponent = "Radiology Order";
                }
                else if (droppedComponent == "Consultation") {
                    droppedComponent = "Consultation Order";
                }
                else if (droppedComponent == "Lab") {
                    droppedComponent = "Lab Orders";
                }

                setTimeout(function () {
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());

                    if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).parent().parent().find('section').length > 0)
                        $('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).parent().parent().find('section').remove();
                    if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).parent().parent().find('section').length == 0) {
                        var mainDiv = '#pnlClinicalProgressNote #ProgressnoteHTML';
                        var BillingComponent = $(mainDiv).find('clinical_billinginfo').closest('li');
                        $(mainDiv).find('clinical_billinginfo').closest('li').remove();
                        $(mainDiv).find('#ProgressNoteComponentList').append(BillingComponent);

                        if (droppedComponent == "Problems") {
                            Clinical_ProblemLists.getLatestProblemListByPatientId(null, droppedComponent);
                        } else if (droppedComponent == "Allergies") {
                            Clinical_Allergies.getLatestAllergyByPatientId(null, null, droppedComponent);
                        }
                        else if (droppedComponent == "Medications") {
                            Clinical_Medications.getLatestMedicationsByPatientId();
                        }
                        else if (droppedComponent == "Social Hx") {
                            Clinical_SocialHx.getLatestSocialHxByPatientId();
                        } else if (droppedComponent == "Birth Hx") {
                            Clinical_BirthHx.getLatestBirthHxByPatientId();
                        }
                        else if (droppedComponent == "Medical Hx") {
                            Clinical_MedicalHx.getLatestMedicalHxByPatientId();
                        }
                        else if (droppedComponent == "Family Hx") {
                            Clinical_FamilyHx.getLatestFamilyHxByPatientId();
                        }
                        else if (droppedComponent == "Surgical Hx") {
                            Clinical_SurgicalHx.getLatestSurgicalHxByPatientId();
                        }
                        else if (droppedComponent == "Hospitalization Hx") {
                            Clinical_HospitalizationHx.getLatestHospitalizationHxByPatientId();
                        }
                        else if (droppedComponent == "Vitals") {
                            Clinical_Vitals.GetLatestVitalByPatientId();
                        }
                        else if (droppedComponent == "Physical Exam") {
                            PhysicalExamTemplatesRevamp.checkPhysicalExamExists();//PhysicalExamTemplatesRevamp.getLatestPhysicalExamByPatientId();
                            Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                        }
                        else if (droppedComponent == "Lab Results") {
                            Clinical_LabOrder.getLatestLabResultByPatientId();
                        }
                        else if (droppedComponent == "Diagnostic Imaging Results") {
                            Clinical_RadiologyOrder.getLatestRadiologyResultByPatientId();
                        }
                        else if (droppedComponent == "Consultation Order") {
                            //  ClinicalConsultationOrderDetail.getLatestConsultationOrderByPatientId();
                            ClinicalConsultationOrderDetail.checkConsultationOrderExists();
                            Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                        }
                        else if (droppedComponent == "Lab Orders") {
                            // ClinicalLabOrderDetail.getLatestLabOrderByPatientId();
                            ClinicalLabOrderDetail.checkLabOrderExists();
                            Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                        }
                        else if (droppedComponent == "Procedure Order") {
                            //ClinicalProcedureOrderDetail.getLatestProcedureOrderByPatientId();
                            ClinicalProcedureOrderDetail.checkProcedureOrderExists();
                            Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                        }
                        else if (droppedComponent == "Immunization") {
                            Clinical_ImmunizationDetail.getLatestImmunizationsByPatientId();
                        }
                        else if (droppedComponent == "Referrals") {
                            Patient_Referrals.getLatestReferralByPatientId(null, true);
                        }
                        else if (droppedComponent == "Plan Of Care") {
                            Clinical_PlanOfCare.getLatestPlanOfCareByPatientId();
                        }
                        else if (droppedComponent == "Patient Education") {
                            //Clinical_PatientEducation.getLatestPatientEducationByPatientId();
                            Clinical_PatientEducation.checkPatientEducationExists();
                            Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                        }
                        else if (droppedComponent == "Functional And Cognitive") {
                            Clinical_Cognitive.getLatestCognitiveByPatientId();
                        }
                        else if (droppedComponent == "Procedures") {
                            Clinical_Procedures.getLatestProceduresByPatientId();
                        }
                        else {
                            var ComponentName = droppedComponent.toLowerCase().split(' ').join('');
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_' + ComponentName).length == 0) {

                                var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
                                var title = droppedComponent;
                                if (thisid.indexOf('clinicalMenu_Medical_HPIComplaints') >= 0) {
                                    title = "HPIComplaints";
                                }

                                if (title == "Radiology Order") {
                                    title = "Diagnostic Imaging Order";
                                    droppedComponent = "Radiology Order";

                                    $(CompnentSelector).append(' <li class="initialVisitBody ' + droppedComponent.replace(/\s+/g, '') + 'Component ' + title.replace(/\s+/g, '') + 'Component"  NoteComponentId="NCDummyId"> <header>' +
                                                             '<clinical_' + droppedComponent.split(" ").join("") + ' title="' + title + '"  id="' + this.id + '" class="NotesComponent">' +
                                                             '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + droppedComponent.split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + title + '">' + title + '</a> ' +
                                                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + droppedComponent.split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name="' + droppedComponent.split(" ").join("") + '"><i class="fa fa-edit"></i></a>' +
                                                                             '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent.split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                                        '</clinical_' + droppedComponent.split(" ").join("") + '> </header></li>');

                                }
                                else {

                                    $(CompnentSelector).append(' <li class="' + droppedComponent.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                                                '<clinical_' + ComponentName + ' title="' + droppedComponent + '"  id="' + Clinical_ProgressNote.params.NotesId + '" class="NotesComponent">' +
                                                '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + title + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + droppedComponent + '">' + droppedComponent + '</a> ' +
                                                         '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + droppedComponent + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                                                '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent.split(' ').join('') + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                           '</clinical_' + ComponentName + '> </header></li>');
                                }
                            }
                            else
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_' + ComponentName).parent().parent().removeClass('hidden');
                            Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
                            Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                        }
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
                        Clinical_ProgressNote.ShowHideComponetsHeaders();
                    }
                }, 1000);
                // findInDiv.hide(true);
            }
        });
    },

    InitializeSubjectiveDragable: function () {
        $("#pnlClinicalProgressNote #NotesComponentList #clinicalMenuSubjective li:not(.nav-parent)").draggable({
            //  revert: true,
            revert: "invalid",
            appendTo: 'body',
            connectToSortable: "#pnlClinicalProgressNote #SubjectiveNoteComponentList",
            containment: '#pnlClinicalProgressNote ',
            helper: function (ev, ui) {
                var droppedComponent_Text = this.textContent;
                var title = $(this.childNodes).attr('title').split(" ").join("");
                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                } else if (this.id.indexOf('clinicalMenu_Medical_HPIComplaints') >= 0) {
                    title = "HPIComplaints";
                }
                return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                         '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + $(this.childNodes).attr('title') + '"  id="' + this.id + '" class="NotesComponent">' +
                         '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + title + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + $(this.childNodes).attr('title') + '">' + droppedComponent_Text + '</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + $(this.childNodes).attr('title').split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                         '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + $(this.childNodes).attr('title') + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                    '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';

            },
            stack: '#pnlClinicalProgressNote #ProgressnoteHTML',
            start: function (event, ui) {
                var droppedComponent_Text = this.textContent;
                var ProgressNote = '#' + Clinical_ProgressNote.params["PanelID"];

                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                // if component is already dropped on Progress note, than stop user to not drag again the same component
                if (!$('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).length == 0) {
                    return false;
                }
                $(this).css('z-index', '10');
                $("#pnlClinicalProgressNote #SubjectiveNoteComponentList").prepend('<div class="border-dash p-default"><h4>Drop here</h4></div>');
            },
            stop: function (ev, ui) {
                $(this).css('z-index', '10');
                ui.helper.css('height', 'auto');
                ui.helper.css('width', 'auto');
                var droppedComponent = this.textContent;
                var thisid = this.id;
                $("#pnlClinicalProgressNote #SubjectiveNoteComponentList .border-dash").remove();

                var SubjectiveComponent = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #SubjectiveNoteComponentList li.initialVisitBody').length == 0;

                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length == 1 || SubjectiveComponent) {

                    setTimeout(function () {
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());

                        if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).parent().parent().find('section').length == 0) {
                            if (droppedComponent == "Problems") {
                                Clinical_ProblemLists.getLatestProblemListByPatientId(null, droppedComponent);
                            } else if (droppedComponent == "Allergies") {
                                Clinical_Allergies.getLatestAllergyByPatientId(null, null, droppedComponent);
                            }
                            else if (droppedComponent == "Medications") {
                                Clinical_Medications.getLatestMedicationsByPatientId();
                            }
                            else if (droppedComponent == "Social Hx") {
                                Clinical_SocialHx.getLatestSocialHxByPatientId();
                            } else if (droppedComponent == "Birth Hx") {
                                Clinical_BirthHx.getLatestBirthHxByPatientId();
                            }
                            else if (droppedComponent == "Medical Hx") {
                                Clinical_MedicalHx.getLatestMedicalHxByPatientId();
                            }
                            else if (droppedComponent == "Family Hx") {
                                Clinical_FamilyHx.getLatestFamilyHxByPatientId();
                            }
                            else if (droppedComponent == "Surgical Hx") {
                                Clinical_SurgicalHx.getLatestSurgicalHxByPatientId();
                            }
                            else if (droppedComponent == "Hospitalization Hx") {
                                Clinical_HospitalizationHx.getLatestHospitalizationHxByPatientId();
                            }
                            else if (droppedComponent == "Review of Systems") {
                                Clinical_ReviewofSystems.getLatestReviewofSystemsByPatientId();
                            }
                            else {
                                var ComponentName = droppedComponent.toLowerCase();
                                if (ComponentName != 'functional' && ComponentName != 'notes') {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + '  #SubjectiveNoteComponentList clinical_' + ComponentName).length == 0) {
                                        var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + '  #SubjectiveNoteComponentList ';
                                        var title = droppedComponent;
                                        if (thisid.indexOf('clinicalMenu_Medical_HPIComplaints') >= 0) {
                                            title = "HPIComplaints";
                                        }
                                        $(CompnentSelector).append(' <li class="initialVisitBody ' + droppedComponent.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                                                    '<clinical_' + ComponentName + ' title="' + droppedComponent + '"  id="' + Clinical_ProgressNote.params.NotesId + '" class="NotesComponent">' +
                                                    '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + title + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + droppedComponent + '">' + droppedComponent + '</a> ' +
                            '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + droppedComponent + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                                                    '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                               '</clinical_' + ComponentName + '> </header></li>');
                                        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());

                                    }
                                    Clinical_ProgressNote.saveComponentSOAPText('Complaints');
                                }
                            }
                            Clinical_ProgressNote.ShowHideComponetsHeaders();
                        }
                    }, 1000);
                } else if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length > 1 && $(this).hasClass('ui-draggable')) {
                    $('#pnlClinicalProgressNote #ProgressnoteHTML .ui-draggable').remove();
                }
                // findInDiv.hide(true);
            }
        });
    },

    InitializeObjectiveDragable: function () {
        $("#pnlClinicalProgressNote #NotesComponentList #clinicalMenuObjective li:not(.nav-parent)").draggable({
            //  revert: true,
            revert: "invalid",
            appendTo: 'body',
            connectToSortable: "#pnlClinicalProgressNote #ObjectiveNoteComponentList",
            containment: '#pnlClinicalProgressNote ',
            helper: function (ev, ui) {
                var droppedComponent_Text = this.textContent;

                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                } return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                         '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + $(this.childNodes).attr('title') + '"  id="' + this.id + '" class="NotesComponent">' +
                         '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + $(this.childNodes).attr('title').split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + $(this.childNodes).attr('title') + '">' + droppedComponent_Text + '</a> ' +
                          '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + $(this.childNodes).attr('title').split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                         '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + $(this.childNodes).attr('title') + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                    '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';

            },
            stack: '#pnlClinicalProgressNote #ProgressnoteHTML',
            start: function (event, ui) {
                var droppedComponent_Text = this.textContent;
                var ProgressNote = '#' + Clinical_ProgressNote.params["PanelID"];

                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                // if component is already dropped on Progress note, than stop user to not drag again the same component
                if (!$('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).length == 0) {
                    return false;
                }
                $(this).css('z-index', '10');
                $("#pnlClinicalProgressNote #ObjectiveNoteComponentList").prepend('<div class="border-dash p-default"><h4>Drop here</h4></div>');
            },
            stop: function (ev, ui) {
                $(this).css('z-index', '10');
                ui.helper.css('height', 'auto');
                ui.helper.css('width', 'auto');
                $("#pnlClinicalProgressNote #ObjectiveNoteComponentList .border-dash").remove();
                var droppedComponent = this.textContent;
                if (droppedComponent == 'Procedure') {
                    droppedComponent = $(this.childNodes).attr('title');
                }
                else if (droppedComponent == 'Consultation') {
                    droppedComponent = $(this.childNodes).attr('title');
                }
                else if (droppedComponent == 'Radiology') {
                    droppedComponent = $(this.childNodes).attr('title');
                }
                else if (droppedComponent == 'Lab') {
                    droppedComponent = $(this.childNodes).attr('title');
                }
                var ObjectiveComponent = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #SubjectiveNoteComponentList li.initialVisitBody').length == 0;

                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length == 1 || ObjectiveComponent) {
                    setTimeout(function () {
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());

                        if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).parent().parent().find('section').length == 0) {
                            if (droppedComponent == "Vitals") {
                                Clinical_Vitals.GetLatestVitalByPatientId();
                            }
                            else if (droppedComponent == "Physical Exam") {
                                PhysicalExamTemplatesRevamp.checkPhysicalExamExists();
                                Clinical_ProgressNote.saveComponentSOAPText('Physical Exam');//Clinical_PhysicalExam.getLatestPhysicalExamByPatientId();
                            }
                            else if (droppedComponent == "Lab Results") {
                                Clinical_LabOrder.getLatestLabResultByPatientId();
                            }
                            else if (droppedComponent == "Diagnostic Imaging Results") {
                                Clinical_RadiologyOrder.getLatestRadiologyResultByPatientId();
                            }
                            else {

                                var ComponentName = droppedComponent.toLowerCase();
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + '  #ObjectiveNoteComponentList clinical_' + ComponentName).length == 0) {

                                    var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + '  #SubjectiveNoteComponentList ';
                                    $(CompnentSelector).append(' <li class="initialVisitBody ' + droppedComponent.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                                                '<clinical_' + ComponentName + ' title="' + droppedComponent + '"  id="' + Clinical_ProgressNote.params.NotesId + '" class="NotesComponent">' +
                                                '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + droppedComponent + '">' + droppedComponent + '</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + droppedComponent + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                                                '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                           '</clinical_' + ComponentName + '> </header></li>');
                                    Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());

                                }
                                Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                            }
                            Clinical_ProgressNote.ShowHideComponetsHeaders();
                        }
                    }, 1000);
                } else if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length > 1 && $(this).hasClass('ui-draggable')) {
                    $('#pnlClinicalProgressNote #ProgressnoteHTML .ui-draggable').remove();
                }

                // findInDiv.hide(true);
            }
        });
    },

    InitializeAssesmentDragable: function () {
        $("#pnlClinicalProgressNote #NotesComponentList #clinicalMenuAssessment li:not(.nav-parent)").draggable({
            //  revert: true,
            revert: "invalid",
            appendTo: 'body',
            connectToSortable: "#pnlClinicalProgressNote #AssessmentNoteComponentList",
            containment: '#pnlClinicalProgressNote ',
            helper: function (ev, ui) {
                var droppedComponent_Text = this.textContent;

                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                } return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                         '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + $(this.childNodes).attr('title') + '"  id="' + this.id + '" class="NotesComponent">' +
                         '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + $(this.childNodes).attr('title').split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + $(this.childNodes).attr('title') + '">' + droppedComponent_Text + '</a> ' +
                    '<a onclick="Clinical_ProgressNote.openProblemLists();" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="OpenProblemLists" name=""><i class="fa fa-caret-down orange" aria-hidden="true"></i></a>' +
                          '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + $(this.childNodes).attr('title').split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                         '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + $(this.childNodes).attr('title') + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                    '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';
            },
            stack: '#pnlClinicalProgressNote #ProgressnoteHTML',
            start: function (event, ui) {
                var droppedComponent_Text = this.textContent;
                var ProgressNote = '#' + Clinical_ProgressNote.params["PanelID"];

                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                // if component is already dropped on Progress note, than stop user to not drag again the same component
                if (!$('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).length == 0) {
                    return false;
                }
                $(this).css('z-index', '10');
                $("#pnlClinicalProgressNote #AssessmentNoteComponentList").prepend('<div class="border-dash p-default"><h4>Drop here</h4></div>');
            },
            stop: function (ev, ui) {
                $(this).css('z-index', '10');
                ui.helper.css('height', 'auto');
                ui.helper.css('width', 'auto');
                var droppedComponent = this.textContent;
                $("#pnlClinicalProgressNote #AssessmentNoteComponentList .border-dash").remove();
                var AssessmentComponent = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #AssessmentNoteComponentList li.initialVisitBody').length == 0;

                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length == 1 || AssessmentComponent) {

                    setTimeout(function () {
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());

                        if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).parent().parent().find('section').length == 0) {
                            if (droppedComponent == "Problems") {
                                Clinical_ProblemLists.getLatestProblemListByPatientId(null, droppedComponent);
                            }
                            else {

                                var ComponentName = droppedComponent.toLowerCase();
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + '  #SubjectiveNoteComponentList clinical_' + ComponentName).length == 0) {

                                    var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + '  #SubjectiveNoteComponentList ';
                                    $(CompnentSelector).append(' <li class="initialVisitBody ' + droppedComponent.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                                                '<clinical_' + ComponentName + ' title="' + droppedComponent + '"  id="' + Clinical_ProgressNote.params.NotesId + '" class="NotesComponent">' +
                                                '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + droppedComponent + '">' + droppedComponent + '</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + droppedComponent + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name="' + droppedComponent_Text.split(" ").join("") + '"><i class="fa fa-edit"></i></a>' +
                                                                '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                           '</clinical_' + ComponentName + '> </header></li>');
                                    Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());

                                }
                                Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                            }

                            Clinical_ProgressNote.ShowHideComponetsHeaders();
                        }
                    }, 1000);
                } else if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length > 1 && $(this).hasClass('ui-draggable')) {
                    $('#pnlClinicalProgressNote #ProgressnoteHTML .ui-draggable').remove();
                }
                //   findInDiv.hide(true);

            }
        });
    },

    InitializePlanDragable: function () {
        $("#pnlClinicalProgressNote #NotesComponentList #clinicalMenuPlan li:not(.nav-parent)").draggable({
            //  revert: true,
            revert: "invalid",
            appendTo: 'body',
            connectToSortable: "#pnlClinicalProgressNote #PlanNoteComponentList",
            containment: '#pnlClinicalProgressNote ',
            helper: function (ev, ui) {
                var droppedComponent_Text = this.textContent;
                var title = $(this.childNodes).attr('title');
                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                if (title == "Diagnostic Imaging") {
                    title = "Diagnostic Imaging Order";
                    droppedComponent_Text = "Radiology Order";

                    return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component ' + title.replace(/\s+/g, '') + 'Component"  NoteComponentId="NCDummyId"> <header>' +
                                             '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + title + '"  id="' + this.id + '" class="NotesComponent">' +
                                             '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + droppedComponent_Text.split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + title + '">' + title + '</a> ' +
                                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + droppedComponent_Text.split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name="' + droppedComponent_Text.split(" ").join("") + '"><i class="fa fa-edit"></i></a>' +
                                                             '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent_Text.split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                        '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';

                }
                else {
                    return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                                                          '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + $(this.childNodes).attr('title') + '"  id="' + this.id + '" class="NotesComponent">' +
                                                          '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + $(this.childNodes).attr('title').split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + $(this.childNodes).attr('title') + '">' + droppedComponent_Text + '</a> ' +
                                                     '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + $(this.childNodes).attr('title').split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name="' + droppedComponent_Text.split(" ").join("") + '"><i class="fa fa-edit"></i></a>' +
                                                                          '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + $(this.childNodes).attr('title') + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                                     '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';
                }
            },
            stack: '#pnlClinicalProgressNote #ProgressnoteHTML',
            start: function (event, ui) {
                var droppedComponent_Text = this.textContent;
                var ProgressNote = '#' + Clinical_ProgressNote.params["PanelID"];

                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                // if component is already dropped on Progress note, than stop user to not drag again the same component
                if (!$('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).length == 0) {
                    return false;
                }
                $(this).css('z-index', '10');
                $("#pnlClinicalProgressNote #PlanNoteComponentList").prepend('<div class="border-dash p-default"><h4>Drop here</h4></div>');
            },

            stop: function (ev, ui) {

                $(this).css('z-index', '10');
                ui.helper.css('height', 'auto');
                ui.helper.css('width', 'auto');
                var droppedComponent = this.textContent;
                $("#pnlClinicalProgressNote #PlanNoteComponentList .border-dash").remove();

                if (droppedComponent == "Procedure") {
                    droppedComponent = "Procedure Order";
                }
                else if (droppedComponent == "Radiology" || droppedComponent == "Diagnostic Imaging") {
                    droppedComponent = "Radiology Order";
                }
                else if (droppedComponent == "Consultation") {
                    droppedComponent = "Consultation Order";
                }
                else if (droppedComponent == "Lab") {
                    droppedComponent = "Lab Orders";
                }

                var PlanComponent = $("#" + Clinical_ProgressNote.params["PanelID"] + ' #PlanNoteComponentList li.initialVisitBody').length == 0;

                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length == 1 || PlanComponent) {

                    setTimeout(function () {

                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());



                        if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).closest('li').find('section').length == 0) {

                            switch (droppedComponent) {

                                case "Consultation Order":
                                case "Radiology Order":
                                case "Lab Orders":
                                case "Procedure Order":

                                case "Follow Up":
                                case "Patient Education":
                                case "Diagnostic Imaging Order":

                                    Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);

                                    break;

                                case "Immunization":

                                    //Clinical_ImmunizationDetail.getLatestImmunizationByPatientId(true, true);
                                    Clinical_ImmunizationDetail.getLatestImmunizationsByPatientId();

                                    break;

                                case "Referrals":

                                    Patient_Referrals.getLatestReferralByPatientId(null, true);

                                    break;
                                case "Procedures":

                                    Clinical_Procedures.getLatestProceduresByPatientId(true);

                                    break;

                                case "Plan Of Care":

                                    Clinical_PlanOfCare.getLatestPlanOfCareByPatientId();

                                    break;
                                case "Care Plan":

                                    Clinical_CarePlan.getcarePlanByPatientId(true);
                                case "Treatment":

                                    Clinical_Treatment.createTreatmentBodyHTML(null, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', false);

                                    break;
                                default:

                                    var ComponentName = droppedComponent.toLowerCase();

                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + '  #PlanNoteComponentList clinical_' + ComponentName).length == 0) {

                                        var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + '  #PlanNoteComponentList ';

                                        $(CompnentSelector).append(' <li class="initialVisitBody ' + droppedComponent.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                                                    '<clinical_' + ComponentName + ' title="' + droppedComponent + '"  id="' + Clinical_ProgressNote.params.NotesId + '" class="NotesComponent">' +
                                                    '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + droppedComponent + '">' + droppedComponent + '</a> ' +
                                                     '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(\'' + $(this).parent().parent().parent() + ',\'' + droppedComponent + '\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name="' + droppedComponent_Text.split(" ").join("") + '"><i class="fa fa-edit"></i></a>' +
                                                                    '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                               '</clinical_' + ComponentName + '> </header></li>');

                                        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
                                        Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                                    }

                                    break;
                            }

                            Clinical_ProgressNote.ShowHideComponetsHeaders();
                        }

                    }, 1000);
                }
                else if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length > 1 && $(this).hasClass('ui-draggable')) {
                    $('#pnlClinicalProgressNote #ProgressnoteHTML .ui-draggable').remove();
                }

                // findInDiv.hide(true);
            }
        });
    },

    InitializeMiscellaneousDragable: function () {
        $("#pnlClinicalProgressNote #NotesComponentList #clinicalMenuMiscellaneous li:not(.nav-parent)").draggable({
            //  revert: true,
            revert: "invalid",
            appendTo: 'body',
            connectToSortable: "#pnlClinicalProgressNote #MiscellaneousNoteComponentList",
            containment: '#pnlClinicalProgressNote ',
            helper: function (ev, ui) {
                var droppedComponent_Text = this.textContent;
                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                } return ' <li class="initialVisitBody ' + droppedComponent_Text.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                         '<clinical_' + droppedComponent_Text.split(" ").join("") + ' title="' + $(this.childNodes).attr('title') + '"  id="' + this.id + '" class="NotesComponent">' +
                         '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + $(this.childNodes).attr('title').split(" ").join("") + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + $(this.childNodes).attr('title') + '">' + droppedComponent_Text + '</a> ' +
                    '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + $(this.childNodes).attr('title').split(" ").join("") + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                         '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + $(this.childNodes).attr('title') + '\',\'' + this.id + '\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                    '</clinical_' + droppedComponent_Text.split(" ").join("") + '> </header></li>';

            },
            stack: '#pnlClinicalProgressNote #ProgressnoteHTML',
            start: function (event, ui) {
                var droppedComponent_Text = this.textContent;
                var ProgressNote = '#' + Clinical_ProgressNote.params["PanelID"];

                if (this.id.indexOf('clinicalMenu_Orders_') >= 0) {
                    droppedComponent_Text = $(this.childNodes).attr('title');
                }
                // if component is already dropped on Progress note, than stop user to not drag again the same component
                if (!$('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent_Text.split(" ").join("")).length == 0) {
                    return false;
                }
                $(this).css('z-index', '10');
                $("#pnlClinicalProgressNote #MiscellaneousNoteComponentList").prepend('<div class="border-dash p-default"><h4>Drop here</h4></div>');
            },
            stop: function (ev, ui) {
                $(this).css('z-index', '10');
                ui.helper.css('height', 'auto');
                ui.helper.css('width', 'auto');
                var droppedComponent = this.textContent;
                $("#pnlClinicalProgressNote #MiscellaneousNoteComponentList .border-dash").remove();
                if (droppedComponent == "CustomForms") {
                    droppedComponent = "Custom Forms";
                }
                var MiscellaneousComponent = $("#" + Clinical_ProgressNote.params["PanelID"] + ' #MiscellaneousNoteComponentList li.initialVisitBody').length == 0;

                if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length == 1 || MiscellaneousComponent) {

                    setTimeout(function () {
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());

                        if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).parent().parent().find('section').length == 0) {
                            if (droppedComponent == "Functional And Cognitive") {
                                Clinical_Cognitive.getLatestCognitiveByPatientId();
                            }
                                //else if(droppedComponent == "Implantable Devices")
                                //    Clinical_Implantable.getLatestImplantableDeviceByPatientId();
                            else {

                                var ComponentName = droppedComponent.toLowerCase().split(" ").join("");
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #MiscellaneousNoteComponentList clinical_' + ComponentName).length == 0) {

                                    var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + '  #MiscellaneousNoteComponentList ';
                                    $(CompnentSelector).append(' <li class="initialVisitBody ' + droppedComponent.replace(/\s+/g, '') + 'Component" NoteComponentId="NCDummyId"> <header>' +
                                                '<clinical_' + droppedComponent.split(" ").join("") + ' title="' + droppedComponent + '"  id="' + Clinical_ProgressNote.params.NotesId + '" class="NotesComponent">' +
                                                '<a class="btn btn-link btn-link-print btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="' + droppedComponent + '">' + droppedComponent + '</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'' + droppedComponent + '\', event);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                                                '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'' + droppedComponent + '\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-link-print btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                           '</clinical_' + droppedComponent.split(" ").join("") + '> </header></li>');
                                    Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());

                                }
                                Clinical_ProgressNote.saveComponentSOAPText(droppedComponent);
                            }
                        }
                    }, 1000);
                } else if ($('#pnlClinicalProgressNote #ProgressnoteHTML clinical_' + droppedComponent.split(" ").join("")).length > 1 && $(this).hasClass('ui-draggable')) {
                    $('#pnlClinicalProgressNote #ProgressnoteHTML .ui-draggable').remove();
                }
                //findInDiv.hide(true);
            }
        });
    },

    ShowHideComponetsHeaders: function () {
        var subjComponents = $("#" + Clinical_ProgressNote.params["PanelID"] + " #SubjectiveNoteComponentList li.editableContentli");
        var objComponents = $("#" + Clinical_ProgressNote.params["PanelID"] + " #ObjectiveNoteComponentList li.editableContentli");
        var assesComponents = $("#" + Clinical_ProgressNote.params["PanelID"] + " #AssessmentNoteComponentList li.editableContentli");
        var planComponents = $("#" + Clinical_ProgressNote.params["PanelID"] + " #PlanNoteComponentList li.editableContentli");
        var subjHeaders = $("#" + Clinical_ProgressNote.params["PanelID"] + " #SubjectiveNoteComponentList header");
        var objHeaders = $("#" + Clinical_ProgressNote.params["PanelID"] + " #ObjectiveNoteComponentList header");
        var assesHeaders = $("#" + Clinical_ProgressNote.params["PanelID"] + " #AssessmentNoteComponentList header");
        var planHeaders = $("#" + Clinical_ProgressNote.params["PanelID"] + " #PlanNoteComponentList header");
        if ((subjComponents && subjComponents.length > 0) || (subjHeaders && subjHeaders.length > 0)) {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #SubjectiveNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {

            $("#" + Clinical_ProgressNote.params["PanelID"] + " #SubjectiveNoteComponentList").prev('h4').addClass('hidden');

        }
        if ((objComponents && objComponents.length > 0) || (objHeaders && objHeaders.length > 0)) {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ObjectiveNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ObjectiveNoteComponentList").prev('h4').addClass('hidden');
        }
        if ((assesComponents && assesComponents.length > 0) || (assesHeaders && assesHeaders.length > 0)) {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #AssessmentNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #AssessmentNoteComponentList").prev('h4').addClass('hidden');
        }
        if ((planComponents && planComponents.length > 0) || (planHeaders && planHeaders.length > 0)) {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #PlanNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #PlanNoteComponentList").prev('h4').addClass('hidden');
        }

    },
    //Author: Talha Tanweer
    //Date  : 03/August/2016

    GetIEVersion: function () {
        var sAgent = window.navigator.userAgent;
        var Idx = sAgent.indexOf("MSIE");

        // If IE, return version number.
        if (Idx > 0)
            return parseInt(sAgent.substring(Idx + 5, sAgent.indexOf(".", Idx)));

            // If IE 11 then look for Updated user agent string.
        else if (!!navigator.userAgent.match(/Trident\/7\./))
            return 11;

        else
            return 0; //It is not IE
    },

    bindDateAndTimepicker: function () {

        utility.CreateDatePicker('pnlClinicalProgressNote #frmClinicalProgressNote #dtpVisitDate',
            //on-change callback method
                         function (ev) {
                             if ($('#pnlClinicalProgressNote #frmClinicalProgressNote').data("bootstrapValidator") != null && typeof $('#frmClinicalProgressNote').data('bootstrapValidator') != 'undefined') {
                                 $('#pnlClinicalProgressNote #frmClinicalProgressNote').bootstrapValidator('revalidateField', 'VisitDate');
                             }
                         }, true);
        //on date change, validating time
        $('#pnlClinicalProgressNote #frmClinicalProgressNote #VisitTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false;
            if ($('#frmClinicalProgressNote').data('bootstrapValidator') != null && typeof $('#frmClinicalProgressNote').data('bootstrapValidator') != 'undefined') {
                $('#pnlClinicalProgressNote #frmClinicalProgressNote').bootstrapValidator('revalidateField', 'VisitTime');
            }
        });
        $('#pnlClinicalProgressNote #frmClinicalProgressNote #VisitTime').timepicker({
            defaultTime: 'now',
            minuteStep: 5//,
        });
        $('#pnlClinicalProgressNote #frmClinicalProgressNote #VisitTime').timepicker('setTime', new Date());
    },
    tinymceDestroy: function (controlSelector) {
        tinymce.EditorManager.execCommand('mceRemoveEditor', true, controlSelector);
        tinymce.remove();
    },
    problemDown: function (probId) {
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_ProgressNote.IsNoteComponentAvaliable(false, "Problems").done(function (res) {
                    if (res == true) {

                        var $secElmt = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #Cli_Problems_Main' + probId);
                        var from_Id = probId;
                        var to_Id = "";
                        to_Id = $secElmt.next().attr("Id").replace('Cli_Problems_Main', '');
                        if (from_Id && to_Id && from_Id != undefined && to_Id != undefined) {
                            Clinical_ProblemLists.changeProblemsOrder(from_Id, to_Id).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_ProgressNote.swapElements($secElmt, $secElmt.next());
                                    Clinical_ProgressNote.saveComponentSOAPText("Problems");
                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                            });
                        }

                    }
                });


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    problemUp: function (probId) {

        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_ProgressNote.IsNoteComponentAvaliable(false, "Problems").done(function (res) {
                    if (res == true) {

                        var $secElmt = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #Cli_Problems_Main' + probId);
                        var from_Id = probId;
                        var to_Id = "";
                        to_Id = $secElmt.prev().attr("Id").replace('Cli_Problems_Main', '');
                        if (from_Id && to_Id && from_Id != undefined && to_Id != undefined) {
                            Clinical_ProblemLists.changeProblemsOrder(from_Id, to_Id).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_ProgressNote.swapElements($secElmt.prev(), $secElmt);
                                    Clinical_ProgressNote.saveComponentSOAPText("Problems");
                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                            });
                        }

                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    swapElements: function (subject, object) {
        var subOrder = $(subject).attr("problemorder");
        var objOrder = $(object).attr("problemorder");
        $(object).attr("problemorder", subOrder);
        $(subject).attr("problemorder", objOrder);
        subject.insertAfter(object);
    },
    showHideUpDownProb: function () {
        var totalProblems = $("#" + Clinical_ProgressNote.params.PanelID + " section[id*='Cli_Problems_Main']").length;
        if (totalProblems > 0) {
            $("#" + Clinical_ProgressNote.params.PanelID).find(".probList section").sort(Clinical_ProgressNote.sort_problemsDesc).appendTo($("#" + Clinical_ProgressNote.params.PanelID).find(".probList"));
            $("#" + Clinical_ProgressNote.params.PanelID + " section[id*='Cli_Problems_Main']").length;
            $("#" + Clinical_ProgressNote.params.PanelID + " section[id*='Cli_Problems_Main']").find('a.up-row').show();
            $("#" + Clinical_ProgressNote.params.PanelID + " section[id*='Cli_Problems_Main']").find('a.down-row').show();
            $($("#" + Clinical_ProgressNote.params.PanelID + " section[id*='Cli_Problems_Main']")[0]).find('a.up-row').hide();
            $($("#" + Clinical_ProgressNote.params.PanelID + " section[id*='Cli_Problems_Main']")[totalProblems - 1]).find('a.down-row').hide();
        }
    },
    sort_problemsDesc: function (a, b) {
        var bOrder = parseInt($(b).attr('problemorder'));
        var aOrder = parseInt($(a).attr('problemorder'));
        if (bOrder > aOrder)
            return 1;
        else
            return -1
        return 0;
    },
    openTinyEditor: function (Control, IsComponentName, ComponentName) {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'clinicalTabProgressNote';
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        if (IsComponentName) {
            params["Contents"] = $(Control).find('#Comments_' + ComponentName).html();
            params["Name"] = ComponentName;
            params["Control"] = Control;
            params["ComponentName"] = ComponentName;
        }
        else {
            params["Contents"] = $(Control).find('#Comments_' + $(Control).attr('id')).html();
            params["Name"] = $(Control).attr('id');
            params["Control"] = Control;
            var ComponentName = $(Control).closest('.initialVisitBody').find('.NotesComponent').attr('title') == "Social, Psychological and Behavior Hx" ? "SocPsyandBehaviorHx" : $(Control).closest('.initialVisitBody').find('.NotesComponent').attr('title');
            params["ComponentName"] = ComponentName;
        }
        params["mode"] = "Edit";
        //$Control.find('#Comments_' + Name).html()
        //$(Control).find('#Comments_' + $(Control).attr('id')).html()
        LoadActionPan('TinymceEditor', params, Clinical_ProgressNote.params.PanelID);
    },
    OpenPhysicalExamTemplateAddEditMK: function (PhysicalExamTemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (PhysicalExamTemplateId != null && parseInt(PhysicalExamTemplateId) > 0) {
                    params["PhysicalExamTemplateId"] = PhysicalExamTemplateId;
                    params["SelectedPETemplateId"] = PhysicalExamTemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["PhysicalExamTemplateId"] = null;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                LoadActionPan('PhysicalExamTemplatesRevamp', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenPhysicalExamSysObservationDetail: function ($obj, PETemplateSystemId, PESystemId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (PESystemId != null && parseInt(PESystemId) > 0) {
                    params["PESystemId"] = PESystemId;
                    params["PETemplateSystemId"] = PETemplateSystemId;
                    params["mode"] = "Edit";
                }
                else {
                    params["PESystemId"] = -1;
                    params["mode"] = "Add";
                    params["PETemplateSystemId"] = PETemplateSystemId;
                }
                params["FromAdmin"] = "0";
                params["Content_style"] = $($obj).siblings('span').length > 0 ? $($($obj).siblings('span')[0]).attr('style') : "";
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                LoadActionPan('PhysicalExamSysObservationDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenROSSysCharcDetail: function ($obj, ROSTemplateId, ROSSystemId) {
        var params = [];
        if (ROSSystemId != null && parseInt(ROSSystemId) > 0) {
            params["ROSSystemId"] = ROSSystemId;
            params["ROSTemplateId"] = ROSTemplateId;
            params["mode"] = "Edit";
        }
        else {
            params["ROSSystemId"] = -1;
            params["mode"] = "Add";
            params["ROSTemplateId"] = ROSTemplateId;
        }
        params['isShowTemplate'] = false;
        params['LoadFromNote'] = "LoadFromNote";
        params["FromAdmin"] = "0";
        params["Content_style"] = $($obj).siblings('span').length > 0 ? $($($obj).siblings('span')[0]).attr('style') : "";
        params["ParentCtrl"] = 'clinicalTabProgressNote';
        params["PanelID"] = "pnlClinicalROSTemplateDetailRevamp";
        params["ComeFrom"] = 'ROSSystemDetail';
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        LoadActionPan('Clinical_ROSTemplateDetailRevamp', params);
    },
    OpenAOESysObservationDetail: function ($obj, AOETemplateSystemId, PESystemId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var parentSection = $($obj).parents("section[id*='Cli_LabOrderDetail_Main']");
                var templateSections = $(parentSection).find('section');
                var templateIDs = [];
                var templatesStyle = [];

                if ($(templateSections).length > 0) {
                    $.each(templateSections, function (i, item) {

                        templatesStyle.push([$(item).attr('id').split('_')[1], $(item).children('span').attr('style')]);
                        templateIDs.push($(item).attr('id').split('_')[1]);
                    });
                }

                var params = [];
                if (PESystemId != null && parseInt(PESystemId) > 0) {
                    params["PESystemId"] = PESystemId;
                    params["AOETemplateSystemId"] = AOETemplateSystemId;
                    params["mode"] = "Edit";
                }
                else {
                    params["PESystemId"] = -1;
                    params["mode"] = "Add";
                    params["AOETemplateSystemId"] = AOETemplateSystemId;
                }
                params["templateIDs"] = templateIDs.length > 0 ? templateIDs : null;
                params["templatesStyle"] = templatesStyle.length > 0 ? templatesStyle : null;
                params["FromAdmin"] = "0";
                //params["Content_style"] = $($obj).siblings('span').length > 0 ? $($($obj).siblings('span')[0]).attr('style') : "";
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                LoadActionPan('AOESysObservationDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenProcedureSysObservationDetail: function ($obj, ProcedureTemplateSystemId, PESystemId, ProcedureId) {
        var strMessage = "";

        var parentSection = $($obj).parents("section[id*='Cli_Procedures_Main']");
        var templateSections = $(parentSection).find('section');
        var templateIDs = [];
        var templatesStyle = [];
        if ($(templateSections).length > 0) {

            $.each(templateSections, function (i, item) {

                templatesStyle.push([$(item).attr('id').split('_')[1], $(item).children('span').attr('style')]);
                templateIDs.push($(item).attr('id').split('_')[1]);
            });
        }

        var params = [];
        if (PESystemId != null && parseInt(PESystemId) > 0) {
            params["PESystemId"] = PESystemId;
            params["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;
            params["mode"] = "Edit";
        }
        else {
            params["PESystemId"] = -1;
            params["mode"] = "Add";
            params["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;
        }
        params["templateIDs"] = templateIDs.length > 0 ? templateIDs : null;
        params["templatesStyle"] = templatesStyle.length > 0 ? templatesStyle : null;
        params["ProcedureId"] = ProcedureId;
        params["FromAdmin"] = "0";
        //params["Content_style"] = $($obj).siblings('span').length > 0 ? $($($obj).siblings('span')[0]).attr('style') : "";
        params["ParentCtrl"] = 'clinicalTabProgressNote';
        LoadActionPan('ProcedureSysObservationDetail', params);

    },
    OpenProcedureOrderSysObservationDetail: function ($obj, ProcedureTemplateSystemId, PESystemId, ProcedureOrderId) {
        var strMessage = "";

        var parentSection = $($obj).parents("section[id*='Cli_ProcedureOrderDetail_Main']");
        var templateSections = $(parentSection).find('section');
        var templateIDs = [];
        var templatesStyle = [];

        if ($(templateSections).length > 0) {

            $.each(templateSections, function (i, item) {

                templatesStyle.push([$(item).attr('id').split('_')[1], $(item).children('span').attr('style')]);
                templateIDs.push($(item).attr('id').split('_')[1]);
            });
        }

        var params = [];
        if (PESystemId != null && parseInt(PESystemId) > 0) {
            params["PESystemId"] = PESystemId;
            params["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;
            params["mode"] = "Edit";
        }
        else {
            params["PESystemId"] = -1;
            params["mode"] = "Add";
            params["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;
        }
        params["templateIDs"] = templateIDs.length > 0 ? templateIDs : null;
        params["templatesStyle"] = templatesStyle.length > 0 ? templatesStyle : null;
        params["ProcedureOrderId"] = ProcedureOrderId;
        params["FromAdmin"] = "0";
        //params["Content_style"] = $($obj).siblings('span').length > 0 ? $($($obj).siblings('span')[0]).attr('style') : "";
        params["ParentCtrl"] = 'clinicalTabProgressNote';
        LoadActionPan('ProcedureOrderSysObservationDetail', params);

    },
    OpenAOESysRadiologyObservationDetail: function ($obj, AOETemplateSystemId, PESystemId) {

        var strMessage = "";
        var parentSection = $($obj).parents("section[id*='Cli_RadiologyOrderDetail_Main']");
        //var templateSections = $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_RadiologyOrderDetail_Main'] li section");

        var templateSections = $(parentSection).find('section');
        var templateIDs = [];
        var templatesStyle = [];

        if ($(templateSections).length > 0) {

            $.each(templateSections, function (i, item) {

                templatesStyle.push([$(item).attr('id').split('_')[1], $(item).children('span').attr('style')]);
                templateIDs.push($(item).attr('id').split('_')[1]);
            });
        }
        var params = [];
        if (PESystemId != null && parseInt(PESystemId) > 0) {
            params["PESystemId"] = PESystemId;
            params["AOETemplateSystemId"] = AOETemplateSystemId;
            params["mode"] = "Edit";
        }
        else {
            params["PESystemId"] = -1;
            params["mode"] = "Add";
            params["AOETemplateSystemId"] = AOETemplateSystemId;
        }
        params["templateIDs"] = templateIDs.length > 0 ? templateIDs : null;
        params["templatesStyle"] = templatesStyle.length > 0 ? templatesStyle : null;
        params["FromAdmin"] = "0";
        //params["Content_style"] = $($obj).siblings('span').length > 0 ? $($($obj).siblings('span')[0]).attr('style') : "";
        params["ParentCtrl"] = 'clinicalTabProgressNote';
        LoadActionPan('AOESysRadiologyObservationDetail', params);

    },
    fixProblemOrderForOldSOAP: function () {
        if ($("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").length > 0) {
            var orderNumber = 1;//$('section[id*="Cli_Problems_Main"]').length;
            var PListId = [];
            $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").each(function (ind, item) {
                if (!$(item).attr('problemOrder')) {
                    var PLid = $(item).attr('id').replace('Cli_Problems_Main', '');
                    PListId.push(PLid);
                    if (!$(item).closest('li').hasClass('probList')) {
                        $(item).closest('li').addClass('probList');
                    }
                    var probListId = $(item).attr('id').replace('Cli_Problems_Main', '');
                    var downHTML = '<a href="javascript:void(0);" class="btn-xs on-default down-row" title="Down Record" onclick="Clinical_ProgressNote.problemDown(' + probListId + ')">'
                    + '<i class="fa fa-arrow-down black"></i></a>'
                    var upHTML = '<a href="javascript:void(0);" class="btn-xs on-default up-row" title="Up Record" onclick="Clinical_ProgressNote.problemUp(' + probListId + ')">'
                   + '<i class="fa fa-arrow-up black"></i></a>'
                    if ($(item).find('.pull-right').find('.down-row').length == 0) {
                        $(item).find('.pull-right').append(downHTML);
                    }
                    if ($(item).find('.pull-right').find('.up-row').length == 0) {
                        $(item).find('.pull-right').append(upHTML);
                    }
                    //orderNumber++;
                }
            });
            var ProblemListsId = "";
            if (PListId.join(",") != "") {
                ProblemListsId = PListId.join(",");
            }
            if (ProblemListsId != "")
                Clinical_ProblemLists.SearchProblemList(ProblemListsId, '1', '2000').done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_ProgressNote.setProblemOrderFromDB(response);
                    }
                });
        }
        Clinical_ProgressNote.params.DefaultProblemsCount = $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").length;
    },
    setProblemOrderFromDB: function (response) {
        if (response.ProblemListCount > 0) {
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            $.each(ProblemListLoadJSONData, function (i, item) {
                var probSec = $("#pnlClinicalProgressNote #ProgressnoteHTML section[id='Cli_Problems_Main" + item.ProblemListId + "']");
                if (probSec)
                    $(probSec).attr('problemorder', item.ProblemOrder);
            });
            Clinical_ProgressNote.showHideUpDownProb();
        }
    },
    autoExpandField: function (obj) {
        var scrollHeight = obj.scrollHeight;
        $(obj).height(scrollHeight - 6);
    },
    saveComponentSOAPText: function (ComponentName, hideAlertMessage, freeText, bMedReconciled, MedReconciledId, isCustomTextArea, customForm, IsCallSummary) {

        Clinical_ProgressNote.showHideUpDownProb();
        var self = $("#" + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote");
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML li:not(.initialVisitBody):not(.sopTextEditable):has(>header)').addClass('initialVisitBody');
        Clinical_ProgressNote.Add_NoText();

        var dfd = $.Deferred();
        var orderNo = -1;
        var secLookupId = 0;
        var section = '';
        var Component;
        var lookupId = "";
        var NoteComponentId = null;
        var ComponentSOAPText = '';
        var ParentComponent = null;
        var componentType = '';
        var noOfComponents = 0;

        if (freeText) {
            if (isCustomTextArea) {
                Component = $(freeText).closest('.CustomComponent');
            }
            else if (IsCallSummary) {
                Component = $(freeText).closest('.PhoneEncounterDataComponent');
            }
            else {
                Component = $(freeText).closest('li');
            }
        }
        else {
            Component = $('.' + ComponentName.replace(/\s+/g, '') + 'Component');
            if (Component.length == 0 && ComponentName.replace(/\s+/g, '') == "LabOrders")
                Component = $('.LabOrderComponent');
            else if (Component.length == 0 && ComponentName.replace(/\s+/g, '') == "LabOrder")
                Component = $('.LabOrdersComponent');
            else if (ComponentName.replace(/\s+/g, '') == "RadiologyOrder") {
                ComponentName = "Diagnostic Imaging Order";
                Component = $('.' + ComponentName.replace(/\s+/g, '') + 'Component');
            }
            else if (ComponentName == "Custom Forms") {
                Component = $(customForm).closest('li');
            }
        }
        if (Component && Component.length > 0) {

            var NoteComponentId = $(Component).attr('NoteComponentId');
            if (ComponentName == "Signature") {
                $(Component).each(function (index, item) {
                    ComponentSOAPText += $(item)[0].outerHTML;
                });
            }
            else if (ComponentName == "Custom Forms") {
                $(Component).each(function (index, item) {
                    ComponentSOAPText += $(item)[0].outerHTML;
                });
            }
            else {
                ComponentSOAPText = Component[0].outerHTML;
            }

            ParentComponent = $(Component).closest('.initialVisit');


            if (ParentComponent) {
                switch (ParentComponent.attr('id')) {
                    case "SubjectiveNoteComponentList":
                        subOrderNo = $('#SubjectiveNoteComponentList').children().index(Component);
                        noOfComponents = $('#SubjectiveNoteComponentList li[class*="Component"]').length;
                        componentType = "Subjective";
                        break;
                    case "ObjectiveNoteComponentList":
                        subOrderNo = $('#ObjectiveNoteComponentList').children().index(Component);
                        noOfComponents = $('#ObjectiveNoteComponentList li[class*="Component"]').length;
                        componentType = "Objective";
                        break;
                    case "AssessmentNoteComponentList":
                        subOrderNo = $('#AssessmentNoteComponentList').children().index(Component);
                        noOfComponents = $('#AssessmentNoteComponentList li[class*="Component"]').length;
                        componentType = "Assessment";
                        break;
                    case "PlanNoteComponentList":
                        subOrderNo = $('#PlanNoteComponentList').children().index(Component);
                        noOfComponents = $('#PlanNoteComponentList li[class*="Component"]').length;
                        componentType = "Plan";
                        break;
                    case "MiscellaneousNoteComponentList":
                        subOrderNo = $('#MiscellaneousNoteComponentList').children().index(Component);
                        noOfComponents = $('#MiscellaneousNoteComponentList li[class*="Component"]').length;
                        componentType = "Miscellaneous";
                        break;
                    case "ProgressNoteComponentList":
                        subOrderNo = $('#ProgressNoteComponentList').children().index(Component);
                        noOfComponents = $('#ProgressNoteComponentList li[class*="Component"]').length;
                        componentType = "Progress";
                        break;
                    default:
                        subOrderNo = 0;
                        break;
                }
                orderNo = subOrderNo;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == ComponentName.replace(/\s+/g, '').toLowerCase())
                        lookupId = itm.id;
                });

                if (ParentComponent.length) {
                    NoteSections.filter(function (itm, indx) {
                        if (itm.value.toLowerCase() == ParentComponent.attr('id').replace('NoteComponentList', '').toLowerCase())
                            secLookupId = itm.id;
                    });
                } else {
                    secLookupId = 0;
                }
                if (ComponentName == "Signature")
                    orderNo = 200;
                else if (ComponentName == "CoSign")
                    orderNo = 201;
                else if (ComponentName == "Implantable Devices") {
                    orderNo = 100;
                }

                if (ComponentSOAPText) {

                    Clinical_ProgressNote.saveComponentSOAPText_DBCall(ComponentSOAPText, orderNo, lookupId, NoteComponentId, secLookupId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_ProgressNote.ShowHideComponetsHeaders();
                            $(Component).attr('NoteComponentId', response.NoteComponentId);
                            if (noOfComponents > 1) {
                                Clinical_ProgressNote.SetNoteComponentsOrder(componentType);
                            }
                            var hfbMedReconciled = $("#" + Clinical_ProgressNote.params["PanelID"] + " #hfbMedReconciled");
                            var hfMedReconcileId = $("#" + Clinical_ProgressNote.params["PanelID"] + " #hfMedReconciledId");
                            if (bMedReconciled != null && bMedReconciled != "" && bMedReconciled == true) {
                                hfbMedReconciled.val("1");
                                hfMedReconcileId.val(MedReconciledId);
                            }
                            else {
                                hfbMedReconciled.val("0");
                                hfMedReconcileId.val("");
                            }
                            if (Clinical_Notes.params.mode != "Edit") {
                                $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').resetAllControls();
                                $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes [type=hidden]').val('');
                                params['IsFromCreateNote'] = true;
                                Clinical_Notes.params["mode"] = "Edit";
                                Clinical_Notes.AddProgressNoteTab();
                            }
                            if (!hideAlertMessage) {
                                utility.DisplayMessages(response.Message, 1);
                            }
                            dfd.resolve();
                        }
                        else {
                            if (Clinical_Allergies.params.ParentCtrl != "clinicalTabFaceSheet") {
                                utility.DisplayMessages(response.Message, 3);
                                dfd.resolve();
                            }
                        }
                    });
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
            }

        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    saveComponentSOAPText_DBCall: function (SOAPText, OrderNo, LookupId, NoteComponentId, secLookupId) {
        var objData = {
        };
        objData["NoteComponentsLookupId"] = LookupId;
        objData["SOAPText"] = SOAPText;
        objData["OrderNo"] = OrderNo;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["NoteSectionsLookupId"] = secLookupId;
        if (NoteComponentId && NoteComponentId != "NCDummyId") {
            objData["NoteComponentId"] = NoteComponentId;
            objData["commandType"] = "update_note_component";
        }
        else {
            objData["commandType"] = "insert_note_component";
        }
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "CLINICALNOTES", "NotesComponent");
    },
    //*************************************************
    // Save bulk Notes Components in NoteComponents Table
    // Perameter :hideAlertMessage=> To hide alter message
    // It will genrate NoteComponent List from DOM. in Case of Provider Note Template/
    //
    //*************************************************
    saveComponentSOAPTextBulk: function (hideAlertMessage, IsNoteUpdate) {
        var dfd = $.Deferred();
        var lstNotesComponent = [];
        var secLookupId;
        var orderNo = 0;
        Clinical_ProgressNote.showHideUpDownProb();
        var self = $("#" + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote");
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML li:not(.initialVisitBody):not(.sopTextEditable):has(>header)').addClass('initialVisitBody');
        Clinical_ProgressNote.Add_NoText();
        var uniquList = [];
        var customLookup = null;
        NoteComponents.filter(function (itm, indx) {
            if (itm.value.toLowerCase().replace(/\s+/g, '') == 'custom')
                customLookup = itm.id;
        });
        if (Clinical_ProgressNote.params.TemplateName) {
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == 'progress')
                    secLookupId = itm.id;
            });
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #ProgressNoteComponentList").children().each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                if (componentName && componentName.toLowerCase() == "esuperbill")
                    componentName = "BillingInfo";
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                $(ctlr).attr('CustomUI', ui);
                objData["UniqueId"] = ui;
                uniquList.push(ui);
            });
        }
        else {
            //For Subjective
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == 'subjective')
                    secLookupId = itm.id;
            });
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #SubjectiveNoteComponentList").children().not('.sopTextEditable').each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                $(ctlr).attr('CustomUI', ui);
                objData["UniqueId"] = ui;
                uniquList.push(ui);
            });
            //For Objective
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == 'objective')
                    secLookupId = itm.id;
            });
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #ObjectiveNoteComponentList").children().not('.sopTextEditable').each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                $(ctlr).attr('CustomUI', ui);
                objData["UniqueId"] = ui;
                uniquList.push(ui);
            });
            //For Assessment
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == 'assessment')
                    secLookupId = itm.id;
            });
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #AssessmentNoteComponentList").children().not('.sopTextEditable').each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                $(ctlr).attr('CustomUI', ui);
                objData["UniqueId"] = ui;
                uniquList.push(ui);
            });
            //For Plan
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == 'plan')
                    secLookupId = itm.id;
            });
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #PlanNoteComponentList").children().not('.sopTextEditable').each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                $(ctlr).attr('CustomUI', ui);
                objData["UniqueId"] = ui;
                uniquList.push(ui);
            });
            //For Miscellaneous
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == 'miscellaneous')
                    secLookupId = itm.id;
            });
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #MiscellaneousNoteComponentList").children().not('.sopTextEditable').each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                $(ctlr).attr('CustomUI', ui);
                objData["UniqueId"] = ui;
                uniquList.push(ui);
            });
            //For Progress
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == 'progress')
                    secLookupId = itm.id;
            });
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #ProgressNoteComponentList").children().not('.sopTextEditable').each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                if (componentName && componentName.toLowerCase() == "esuperbill")
                    componentName = "BillingInfo";
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                $(ctlr).attr('CustomUI', ui);
                objData["UniqueId"] = ui;
                uniquList.push(ui);
            });

        }
        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
            var ComponentName = "PhoneEncounterData";
            var Component = $('.' + ComponentName.replace(/\s+/g, '') + 'Component');
            if (Component && Component.length > 0) {
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == ComponentName.toLowerCase())
                        lookupId = itm.id;
                });
                NoteSections.filter(function (itm, indx) {
                    if (itm.value.toLowerCase() == 'phoneencounterdata')
                        secLookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(Component)[0].outerHTML;
                objData["NoteComponentId"] = $(Component).attr('NoteComponentId') == "NCDummyId" ? "0" : $(Component).attr('NoteComponentId');
                objData["OrderNo"] = 0;
                objData["NoteSectionsLookupId"] = secLookupId;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                var ui = utility.makeRendomKey();
                objData["UniqueId"] = ui;
                $(Component).attr('CustomUI', ui);
                uniquList.push(ui);
            }
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML ul[name='CCM']").each(function (i, ctlr) {
                var componentName = $.grep(this.className.split(" "), function (v, i) {
                    return v.indexOf('Component') > -1;
                }).join();
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace('Component', '').toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId != undefined ? lookupId : customLookup;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = null;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                objData["UniqueId"] = ui;
                $(ctlr).attr('CustomUI', ui);
                uniquList.push(ui);
            });
        }

        lstNotesComponent = JSON.stringify(lstNotesComponent);
        if (lstNotesComponent.length > 0) {
            Clinical_ProgressNote.saveComponentSOAPTextBulk_DBCall(lstNotesComponent, IsNoteUpdate).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.each(response.NoteComponentListFill_JSON, function (i, item) {
                        $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML [CustomUI='" + item.UniqueId + "']").attr('NoteComponentId', item.NoteComponentId).removeAttr('CustomUI');
                    });
                    if (!hideAlertMessage) {
                        utility.DisplayMessages(response.Message, 1);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                dfd.resolve(response.status);
            });
        }
        else {
            dfd.resolve(false);
        }
        return dfd;
    },
    //*************************************************
    // Save bulk DB Call Notes Components in NoteComponents Table
    // Perameter :componentList=> List of NoteComponent
    //*************************************************
    saveComponentSOAPTextBulk_DBCall: function (componentList, IsNoteUpdate) {
        var objData = new Object();
        objData["commandType"] = "insert_note_components_bulk";
        objData["NoteComponentist"] = JSON.parse(componentList);
        objData["IsNoteUpdate"] = IsNoteUpdate;
        //componentList.push(objData)
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "NotesComponent");
    },
    //*************************************************
    // Remove Note Component from NoteComponents Table
    // Perameter :NoteComponentId
    //*************************************************
    removeComponentSOAPText_DBCall: function (NoteComponentId) {
        var objData = {
        };
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["NoteComponentId"] = NoteComponentId;
        objData["commandType"] = "delete_note_component";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "NotesComponent");
    },

    SetNoteComponentsOrder: function (component) {

        var componentIds = [];
        var ulComponentList = '';
        if (component == "Subjective") {
            ulComponentList = $("#pnlClinicalProgressNote #SubjectiveNoteComponentList li");
        }
        else if (component == "Objective") {
            ulComponentList = $("#pnlClinicalProgressNote #ObjectiveNoteComponentList li");
        }
        else if (component == "Assessment") {
            ulComponentList = $("#pnlClinicalProgressNote #AssessmentNoteComponentList li");
        }
        else if (component == "Plan") {
            ulComponentList = $("#pnlClinicalProgressNote #PlanNoteComponentList li");
        }
        else if (component == "Progress") {
            ulComponentList = $("#pnlClinicalProgressNote #ProgressNoteComponentList li");
        }
        else {
            ulComponentList = $("#pnlClinicalProgressNote #MiscellaneousNoteComponentList li");
        }

        $(ulComponentList).each(function () {
            var componentId = $(this).attr('notecomponentid');
            if (componentId && componentId != "NCDummyId") {
                if (componentId && $.inArray(componentId, componentIds) == -1) {
                    componentIds.push($(this).attr('notecomponentid'));
                }
            }
        });

        Clinical_ProgressNote.SetNoteComponentsOrder_DbCall(componentIds.join()).done(function (response) {

        });
    },

    SetNoteComponentsOrder_DbCall: function (componentIds) {
        var objData = {
        };
        objData["NoteComponentIds"] = componentIds;
        objData["commandType"] = "set_notecomponents_order";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "NotesComponent");
    },
    LoadCDSAlertsForNotes: function () {

        if ($("div#PatientProfile #hfPatientId").val() != "") {
            $("#mainForm  li#CDSAlert").show();
            var triggerLocation = 'ProgressNote';


            ClinicalCDSDetail.showCDSAlert(triggerLocation, 0);

        }

    },

    appendHTMLToNote: function (HTML) {
        $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML #ProgressNoteComponentList").append(HTML);
        Clinical_ProgressNote.saveComponentSOAPText('CDS');
    },

    removeCDSQuestionnaireFromNote: function (obj) {

        var NoteComponentId = $(obj).closest("li").attr('NoteComponentId');
        if (NoteComponentId) {
            $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $(obj).closest("li").remove();
                    utility.DisplayMessages("Successfully Deleted", 1);
                    //setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            $(obj).closest("li").remove();
            utility.DisplayMessages("Successfully Deleted", 1);
        }
    },
    FixOldNotesForSOAPComponents: function (hideAlertMessage) {
        hideAlertMessage = true;
        var dfd = $.Deferred();
        var lstNotesComponent = [];
        var secLookupId;
        var orderNo = 0;
        Clinical_ProgressNote.showHideUpDownProb();
        var self = $("#" + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote");
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML li:not(.initialVisitBody):not(.sopTextEditable):has(>header)').addClass('initialVisitBody');
        Clinical_ProgressNote.Add_NoText();
        var uniquList = [];
        $(".NotesComponent").each(function () {
            var componentName = $(this).attr('title');
            if (componentName && componentName.toLowerCase() == "esuperbill")
                componentName = "BillingInfo";
            else if (componentName && componentName.toLowerCase() == "prescriptions")
                componentName = "Prescription";
            var ctlr = $(this).closest('.initialVisitBody');
            if (!$(ctlr).attr('NoteComponentId'))
                $(ctlr).attr('NoteComponentId', 'NCDummyId');
            var parentCtlr = $(ctlr).closest('.initialVisit');
            NoteSections.filter(function (itm, indx) {
                if (itm.value.toLowerCase() == $(parentCtlr).attr('id').replace('NoteComponentList', '').toLowerCase())
                    secLookupId = itm.id;
            });
            var lookupId;
            NoteComponents.filter(function (itm, indx) {
                if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.replace(/\s+/g, '').toLowerCase())
                    lookupId = itm.id;
            });
            if (ctlr.length > 0) {
                $(ctlr).addClass(componentName + "Component")
            }
            var objData = new Object();
            objData["NoteComponentsLookupId"] = lookupId;
            objData["SOAPText"] = $(ctlr)[0].outerHTML;
            objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
            objData["OrderNo"] = orderNo;
            objData["NoteSectionsLookupId"] = secLookupId;
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
            lstNotesComponent.push(objData);
            orderNo++;
            var ui = utility.makeRendomKey();
            $(ctlr).attr('CustomUI', ui);
            objData["UniqueId"] = ui;
            uniquList.push(ui);
        });
        if (Clinical_ProgressNote.params.IsPhoneEncounter == true) {
            $("#ProgressnoteHTML ul[name='CCM']").each(function (i, ctlr) {
                var componentName = 'CCMGoals';
                var lookupId;
                NoteComponents.filter(function (itm, indx) {
                    if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.toLowerCase())
                        lookupId = itm.id;
                });
                var objData = new Object();
                objData["NoteComponentsLookupId"] = lookupId;
                objData["SOAPText"] = $(ctlr)[0].outerHTML;
                objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
                objData["OrderNo"] = orderNo;
                objData["NoteSectionsLookupId"] = null;
                objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
                lstNotesComponent.push(objData);
                orderNo++;
                var ui = utility.makeRendomKey();
                objData["UniqueId"] = ui;
                $(ctlr).attr('CustomUI', ui);
                uniquList.push(ui);
                if (ctlr.length > 0) {
                    $(ctlr).addClass("CCMGoalsComponent")
                }
            });
        }
        $("#signedByProvider").each(function () {
            var ctlr = $(this);
            $(ctlr).attr('NoteComponentId', 'NCDummyId');
            var lookupId;
            NoteComponents.filter(function (itm, indx) {
                if (itm.value == 'Signature')
                    lookupId = itm.id;
            });
            var objData = new Object();
            objData["NoteComponentsLookupId"] = lookupId;
            objData["SOAPText"] = $(ctlr)[0].outerHTML;
            objData["NoteComponentId"] = 0;
            objData["OrderNo"] = 200;
            objData["NoteSectionsLookupId"] = null;
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
            lstNotesComponent.push(objData);
            orderNo++;
            var ui = utility.makeRendomKey();
            $(ctlr).attr('CustomUI', ui);
            objData["UniqueId"] = ui;
            uniquList.push(ui);
            if (ctlr.length > 0) {
                $(ctlr).addClass("SignatureComponent")
            }
        });
        $("#AmendmentSection").each(function () {
            var ctlr = $(this);
            $(ctlr).attr('NoteComponentId', 'NCDummyId');
            var lookupId;
            NoteComponents.filter(function (itm, indx) {
                if (itm.value == 'Amendment')
                    lookupId = itm.id;
            });
            var objData = new Object();
            objData["NoteComponentsLookupId"] = lookupId;
            objData["SOAPText"] = $(ctlr)[0].outerHTML;
            objData["NoteComponentId"] = 0;
            objData["OrderNo"] = 300;
            objData["NoteSectionsLookupId"] = null;
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
            lstNotesComponent.push(objData);
            orderNo++;
            var ui = utility.makeRendomKey();
            $(ctlr).attr('CustomUI', ui);
            objData["UniqueId"] = ui;
            uniquList.push(ui);
            if (ctlr.length > 0) {
                $(ctlr).addClass("AmendmentComponent")
            }
        });
        lstNotesComponent = JSON.stringify(lstNotesComponent);
        if (lstNotesComponent.length > 0) {
            Clinical_ProgressNote.saveComponentSOAPTextBulk_DBCall(lstNotesComponent).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.each(response.NoteComponentListFill_JSON, function (i, item) {
                        $("#ProgressnoteHTML [CustomUI='" + item.UniqueId + "']").attr('NoteComponentId', item.NoteComponentId).removeAttr('CustomUI');
                    });
                    if (!hideAlertMessage) {
                        utility.DisplayMessages(response.Message, 1);
                    }
                }
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },
    fixDivWithLiInNoteTemplate: function () {
        $('#pnlClinicalProgressNote #ProgressnoteHTML #ProgressNoteComponentList').children().each(function () {
            if ($(this).is('div') == true) {
                var currnDiv = $(this);
                var attrs = {
                };
                $.each(currnDiv[0].attributes, function (idx, attr) {
                    attrs[attr.nodeName] = attr.nodeValue;
                });
                if (!currnDiv.attr('class')) {
                    currnDiv.replaceWith($("<li/>", attrs).append(currnDiv.contents()).addClass('CustomComponent').attr('NoteComponentId', 'NCDummyId'));
                }
                else {
                    currnDiv.replaceWith($("<li/>", attrs).append(currnDiv.contents()).attr('NoteComponentId', 'NCDummyId'));
                }
            }
        });
    },
    saveCCMProgramComponents: function (hideAlertMessage) {
        hideAlertMessage = true;
        var dfd = $.Deferred();
        var lstNotesComponent = [];
        var secLookupId;
        var orderNo = 0;
        var uniquList = [];
        $("#ProgressnoteHTML ul[name='CCM']").each(function (i, ctlr) {
            var componentName = 'CCMGoals';
            var lookupId;
            NoteComponents.filter(function (itm, indx) {
                if (itm.value.toLowerCase().replace(/\s+/g, '') == componentName.toLowerCase())
                    lookupId = itm.id;
            });
            var objData = new Object();
            objData["NoteComponentsLookupId"] = lookupId;
            objData["SOAPText"] = $(ctlr)[0].outerHTML;
            objData["NoteComponentId"] = $(ctlr).attr('NoteComponentId') == "NCDummyId" ? "0" : $(ctlr).attr('NoteComponentId');
            objData["OrderNo"] = orderNo;
            objData["NoteSectionsLookupId"] = null;
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
            lstNotesComponent.push(objData);
            orderNo++;
            var ui = utility.makeRendomKey();
            objData["UniqueId"] = ui;
            $(ctlr).attr('CustomUI', ui);
            uniquList.push(ui);
        });
        lstNotesComponent = JSON.stringify(lstNotesComponent);
        if (lstNotesComponent.length > 0) {
            Clinical_ProgressNote.saveComponentSOAPTextBulk_DBCall(lstNotesComponent).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.each(response.NoteComponentListFill_JSON, function (i, item) {
                        $("#ProgressnoteHTML [CustomUI='" + item.UniqueId + "']").attr('NoteComponentId', item.NoteComponentId).removeAttr('CustomUI');
                    });
                    if (!hideAlertMessage) {
                        utility.DisplayMessages(response.Message, 1);
                    }
                }
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    OpenHPIComplaintsTemplateAddEdit: function (HPITemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_HPI Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (HPITemplateId != null && parseInt(HPITemplateId) > 0) {
                    params["HPITemplateId"] = HPITemplateId;
                    params["SelectedHPITemplateId"] = HPITemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["HPITemplateId"] = null;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                LoadActionPan('Clinical_HPIComplaints', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenHPIComplaintSymFindingDetail: function ($obj, HPITemplateSymptomId, HPISymptomsId, HPITemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_HPI Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (HPISymptomsId != null && parseInt(HPISymptomsId) > 0) {
                    params["HPISymptomsId"] = HPISymptomsId;
                    params["mode"] = "Edit";
                }
                else {
                    params["HPISymptomsId"] = -1;
                    params["mode"] = "Add";
                }
                params["HPITemplateId"] = HPITemplateId;
                params["HPITemplateSymptomId"] = HPITemplateSymptomId;
                params["SymptomName"] = $($obj).text().trim();
                params["FromAdmin"] = "0";
                params["Content_style"] = $($obj).siblings('span').length > 0 ? $($($obj).siblings('span')[0]).attr('style') : "";
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                LoadActionPan('Clinical_HPISymFindingDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    checkComplaintType: function () {
        var objDetail = {};
        objDetail["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objDetail["commandType"] = "is_hpi_complaint";

        var data = JSON.stringify(objDetail);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    OpenHPITemplateComplaintAddEdit: function (HPITemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_HPI Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (HPITemplateId != null && parseInt(HPITemplateId) > 0) {
                    params["HPITemplateId"] = HPITemplateId;
                    params["SelectedHPITemplateId"] = HPITemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["HPITemplateId"] = null;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = "0";
                params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                params["ParentCtrl"] = 'clinicalTabProgressNote';
                LoadActionPan('Clinical_HPIComplaints', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //----- NOTE ATCHMENT ------\\
    NoteAttachment: function () {
        Clinical_ProgressNote.DetachedNoteComponentIds = [];
        Clinical_ProgressNote.AttachedNoteComponentIds = [];

        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalProgressNote";
        params["FacilityId"] = "-1";
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabProgressNote";
        LoadActionPan('Patient_Document', params);
    },
    CancerReport: function () {
        //Clinical_ClinicalSummary.displayCancerReportSummaryHTML();
        Clinical_ClinicalSummary.downloadCancerSummaryXMLData();
        // send cancer Report....
        //var IsCancerReportSubmitted=false;
        //if (!IsCancerReportSubmitted) {
        //    IsCancerReportSubmitted = true;


        //    Clinical_ProgressNote.saveComponentSOAPText('Problems');
        //}
        //else {

        //    utility.DisplayMessages("Cancer Report is already submitted!", 3);
        //}
    },
    EnableDisableCancerReportButton: function () {
        if (globalAppdata["isTransitonCancerRegistries"] && globalAppdata["isTransitonCancerRegistries"].toLowerCase() == "false") {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').addClass("hidden");
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').addClass("disabled");
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').addClass("disabled");
        var VisitType = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ddlVisitType option:selected').text().trim();
        var HaveCancerDisease = false;
        $("#pnlClinicalProgressNote #ProgressnoteHTML section[id*='Cli_Problems_Main']").each(function () {
            if ($(this).attr("iscancerdisease") == "True") {
                HaveCancerDisease = true;
                return false;
            }
        });
        if (VisitType.toLowerCase().indexOf("cancer") >= 0 && HaveCancerDisease) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').removeClass("hidden");
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').removeClass("disabled");
        }


    },

    VisitTypeUpdate: function (visitTypeId, NotesId, AppointmentId) {
        var objData = new Object();
        objData["NotesId"] = NotesId;
        objData["PatientVisitType"] = visitTypeId;
        objData["AppointmentID"] = AppointmentId;
        objData["commandType"] = "UPDATE_VISIT_TYPE";

        var data = JSON.stringify(objData);

        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");

    },
    UpdateVisitType: function (t_his) {
        // if ($("#pnlClinicalProgressNote #ddlVisitType").val() != 0) {
        var visitTypeId = $("#pnlClinicalProgressNote #ddlVisitType").val();

        Clinical_ProgressNote.VisitTypeUpdate(visitTypeId, Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.AppointmentID).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Notes.AddProgressNoteTab();
                Clinical_ProgressNote.EnableDisableCancerReportButton();
                utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        // }
    },
    detach_ComponentImages: function (componentName, isUpdate, imageComponentRemove) {
        var clinicalImagesIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_images').parent().parent().find('section[id*="Cli_Images_Main"]').map(function () {
            return this.id.replace("Cli_Images_Main", "");
        }).get().join(',');

        if (imageComponentRemove) {
            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_images').parent().parent().attr('NoteComponentId');
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_images').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Images', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_images').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Images']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_images').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Images', true))
                }
                else {
                    if (NoteComponentId && NoteComponentId != "NCDummyId")
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                    else {
                        var def = $.Deferred();
                        promise.push(def);
                        def.resolve();
                    }
                }
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_images').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    utility.DisplayMessages('Successfully Deleted', 1);
                });
            }

        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_images').parent().parent().find('section[id*="Cli_Images_Main"]').remove();
        }

        if (clinicalImagesIds == "" || clinicalImagesIds == "undefined") {
            Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(clinicalImagesIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {
                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Images', true)).then(function () {
                            Clinical_ProgressNote.HideShowBillingInfo();
                        });
                    }
                    Clinical_ProgressNote.updateAttachDocumentButtonImg();
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    detachImagesComponentFromNotes_DBCall: function (clinicalImagesIds) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientDocumentIds"] = clinicalImagesIds;
        objData["commandType"] = "detach_documentsfromnote";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },


    detachImagesFromNotes: function (patientDocumentId) {
        var strMessage = "";
        if (strMessage == "") {
            utility.myConfirm('28', function () {
                var selectedValue = patientDocumentId.replace('Cli_Images_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_ProgressNote.DetachImageFromNotes_DBCall(selectedValue).done(function (response) {
                        if (response.status != false) {
                            $('#' + patientDocumentId).remove();
                            Clinical_ProgressNote.updateAttachDocumentButtonImg();
                            Clinical_ProgressNote.saveComponentSOAPText('Images');
                            Clinical_ProgressNote.hoverFunction();
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () {
            },
                '29'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    DetachImageFromNotes_DBCall: function (patientDocumentId) {

        var data = "DocumentID=" + patientDocumentId + "&NotesId=" + Clinical_ProgressNote.params.NotesId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT_IMAGE_ANNOTATION", "DETACH_PATIENT_DOCUMENT_TO_NOTE");

    },

    NoteAttachmentExists: function () {
        Clinical_ProgressNote.noteAttachmentExists_DBCall().done(function (response) {
            if (response.status != false) {

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    noteAttachmentExists_DBCall: function (clinicalImagesIds) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "isnoteattachmentexists";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },
    updateAttachDocumentButtonImg: function () {
        Clinical_ProgressNote.noteAttachmentExists_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.IsAttachmentExists == "1") {
                $("#" + Clinical_Notes.params.PanelID + " #noteatch_i").addClass("fa-paperclip");
            }
            else {
                $("#" + Clinical_Notes.params.PanelID + " #noteatch_i").removeClass("fa-paperclip");
            }
        });
    },
    SaveAndAttachProcedureReport_DbCall: function (patientID, NoteId, ProviderId, isFindingUpdated) {
        var objData = {
        };
        objData["PatientId"] = patientID;
        objData["NoteId"] = NoteId;
        objData["ProviderId"] = ProviderId;
        objData["FileName"] = Clinical_ProgressNote.SetFileName();
        objData["IsFindingUpdated"] = isFindingUpdated;
        objData["commandType"] = "saveandattachprocedurereport";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },
    SaveAndAttachProcedureReport: function (patientID, NoteId, ProviderId, isFindingUpdated) {
        Clinical_ProgressNote.SaveAndAttachProcedureReport_DbCall(patientID, NoteId, ProviderId, isFindingUpdated).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ProcedureIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedures').parent().parent().find('section[id*="Cli_Procedures_Main"]').map(function () {
                    return this.id.replace("Cli_Procedures_Main", "");
                });
                for (var j = 0; j < ProcedureIds.length; j++) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedures').parent().parent().find("section[id='Cli_Procedures_Main" + ProcedureIds[j] + "']").attr("patdocid", response.attachDocId);
                }
            }
            Clinical_ProgressNote.updateAttachDocumentButtonImg();
        });
    },
    SaveAndAttachOrderReport: function (orderType, orderIds, isFindingUpdated, hideAlert) {
        var dfd = $.Deferred();
        Clinical_ProgressNote.SaveAndAttachOrderReport_DBCall(orderType, orderIds, isFindingUpdated).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PatDocOrdersList_Count > 0) {
                    var patDocsList = JSON.parse(response.PatDocOrdersList)
                    if (orderType == "Radiology Order") {
                        var radiologyOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').map(function () {
                            return this.id.replace("Cli_RadiologyOrderDetail_Main", "");
                        });

                        if (radiologyOrderIds.length > 0) {
                            for (var i = 0; i < patDocsList.length; i++) {
                                for (var j = 0; j < radiologyOrderIds.length; j++) {
                                    if (patDocsList[i].OrderId == radiologyOrderIds[j]) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find("section[id='Cli_RadiologyOrderDetail_Main" + radiologyOrderIds[j] + "']").attr("patdocid", patDocsList[i].PatDocId);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (orderType == "Procedure Order") {
                        var ProcedureOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Procedureorder').parent().parent().find('section[id*="Cli_ProcedureOrderDetail_Main"]').map(function () {
                            return this.id.replace("Cli_ProcedureOrderDetail_Main", "");
                        });

                        if (ProcedureOrderIds.length > 0) {
                            for (var i = 0; i < patDocsList.length; i++) {
                                for (var j = 0; j < ProcedureOrderIds.length; j++) {
                                    if (patDocsList[i].OrderId == ProcedureOrderIds[j]) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Procedureorder').parent().parent().find("section[id='Cli_ProcedureOrderDetail_Main" + ProcedureOrderIds[j] + "']").attr("patdocid", patDocsList[i].PatDocId);
                                        break;
                                    }
                                }
                            }
                        }
                        Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder', true);
                    }

                }
                Clinical_ProgressNote.updateAttachDocumentButtonImg();
                if (!hideAlert) {
                    utility.DisplayMessages(response.Message, 1);
                }

                dfd.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },

    SaveAndAttachOrderReport_DBCall: function (orderType, orderIds, isFindingUpdated) {

        var objData = new Object();
        objData["OrderType"] = orderType;
        objData["OrderIds"] = orderIds;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["FileType"] = "application/pdf";
        objData["IsFindingUpdated"] = isFindingUpdated;

        objData["FileName"] = Clinical_ProgressNote.SetFileName();
        if (orderType == "Procedure Order") {
            objData["FolderName"] = "Proc Ord Report";
        }
        else {

            objData["FolderName"] = "Rad Ord Report";
        }
        objData["commandType"] = "saveandattachorderreport";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },

    SetFileName: function () {
        var patientName = $("#PatientProfile").find('#hfPatientFullNameOnly').val();
        var fName = patientName.substr(patientName.indexOf(",") + 1).trim();
        var lName = patientName.substr(0, patientName.indexOf(",")).trim();
        var dt = $.datepicker.formatDate('ddmmyy', new Date());
        var fileName = fName + lName + dt;
        return fileName;
    },

    signNote: function (NotesId, PatientId, IsFromProgressNote, IsPhoneEncounter, customSigMsg, hideMsg, isFromEsuperBill) {
        var dfd = $.Deferred();
        Clinical_ProgressNote.params.NotesId = NotesId;
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!(IsPhoneEncounter || Clinical_ProgressNote.params.IsOutOfOfficeVisit) && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
            dfd.resolve();
        }
        else {
            $.when(Clinical_ProgressNote.FillNoteSignData(NotesId, PatientId, IsFromProgressNote, IsPhoneEncounter, customSigMsg, hideMsg, isFromEsuperBill)).then(function () {
                dfd.resolve();
            });
        }
        //  findInDiv.hide(true);
        return dfd;
    },
    FillNoteSignData: function (NotesId, PatientId, IsFromProgressNote, IsPhoneEncounter, customSigMsg, hideMsg, isFromEsuperBill) {
        var dfd = $.Deferred();
        var VisitId;
        var BillingInfoId;
        var VisitDate;
        var ProviderId;
        var FacilityPOSCode;
        var ProviderFullName;
        var ResourceProviderId;
        var ResourceProviderName;
        var POS;
        var VisitReason;
        var VisitTime;
        var isNonBillable = false;

        Clinical_ProgressNote.FillNotes(null, NotesId, PatientId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                NotesId = Clinical_Notes_detail.NotesId;
                VisitId = Clinical_Notes_detail.VisitId;
                BillingInfoId = Clinical_Notes_detail.BillingInfoId;
                VisitDate = Clinical_Notes_detail.VisitDate;
                PatientId = Clinical_Notes_detail.PatientId;
                ProviderId = Clinical_Notes_detail.ProviderId;
                FacilityPOSCode = Clinical_Notes_detail.FacilityPOSCode;
                POS = Clinical_Notes_detail.FacilityPOSCode;
                VisitReason = Clinical_Notes_detail.VisitReason;
                VisitTime = Clinical_Notes_detail.VisitTime;
                isNonBillable = Clinical_Notes_detail.IsNonBilable;
                if (Clinical_Notes_detail.NoteStatus && Clinical_Notes_detail.NoteStatus == "Draft") {
                    var billingInfoStatus = "Draft";
                    var isESuperBillSigned = false;
                    var isJustNoteSign = false;
                    // for checking the status of billing info
                    var signMessage = "eSuperbill will also be signed. Are you sure you want to sign the provider note?";

                    if (isFromEsuperBill) {
                        signMessage = "Provider Note will also get signed. Are you sure you want to sign the esuperbill?";
                    }

                    if (customSigMsg) {
                        signMessage = customSigMsg;
                    }
                    if (Clinical_Notes_detail.billingInfoStatus == "Signed") {
                        isESuperBillSigned = true;
                        Clinical_ProgressNote.params.isESuperBillSigned = true;
                        if (customSigMsg) {
                            signMessage = customSigMsg;
                        }
                        else {
                            signMessage = "Are you sure you want to sign the provider note?";
                        }
                    }
                    Clinical_ProgressNote.params.IsNonBilable = isNonBillable;
                    if (isNonBillable) {
                        utility.myConfirm('System will not create claim. Are you sure you want to make the Visit as Non Billable?', function () {
                            $.when(Clinical_ProgressNote._NoteSignAndCreateClaim(NotesId, VisitId, VisitDate, VisitTime, VisitReason, PatientId, ProviderId, IsFromProgressNote, IsPhoneEncounter, signMessage, hideMsg)).then(function () {
                                dfd.resolve();
                            });
                        }, function () {
                            Clinical_ProgressNote.params.CancelSign = true;
                            Clinical_ProgressNote.EnableDisableConcurrentNoteSignBtn(false);
                            dfd.resolve();
                        }, 'Confirm Non Billable');
                    } else {
                        $.when(Clinical_ProgressNote._NoteSignAndCreateClaim(NotesId, VisitId, VisitDate, VisitTime, VisitReason, PatientId, ProviderId, IsFromProgressNote, IsPhoneEncounter, signMessage, hideMsg)).then(function () {
                            dfd.resolve();
                        });
                    }
                } else {
                    dfd.resolve();
                    utility.DisplayMessages("This note is already signed.", 3);
                }
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd;
    },
    _NoteSignAndCreateClaim: function (NotesId, VisitId, VisitDate, VisitTime, VisitReason, PatientId, ProviderId, IsFromProgressNote, IsPhoneEncounter, signMessage, hideMsg) {

        Clinical_ProgressNote.params.VisitDateTime = VisitDate + VisitTime;
        var dfd = $.Deferred();
        utility.myConfirm(signMessage, function () {
            Clinical_ProgressNote.signNote_DBCall(NotesId, VisitId, VisitDate, PatientId, ProviderId, IsFromProgressNote, false).done(function (response) {
                response = JSON.parse(response);
                if (response.status) {
                    var sig = "";
                    var isProceMissed = false;
                    var isProbMissed = false;
                    var IsNoteSignWOICDCode = false;
                    var IsPhoneEncounter = false;
                    var IsNoteSignWOCPTCode = false;
                    if (response.NoteComponentModel_JSON) {
                        sig = response.NoteComponentModel_JSON.SOAPText;
                        BillingInfoId = response.NoteComponentModel_JSON.BillingInfoId;
                        VisitId = response.NoteComponentModel_JSON.VisitId;
                        isProbMissed = response.NoteComponentModel_JSON.IsProblemMissed;
                        isProceMissed = response.NoteComponentModel_JSON.IsProcedureMissed;
                        ErrorMessage = response.NoteComponentModel_JSON.ErrorMessage;
                        IsNoteSignWOICDCode = response.NoteComponentModel_JSON.IsNoteSignWOICDCode;
                        IsNoteSignWOCPTCode = response.NoteComponentModel_JSON.IsNoteSignWOCPTCode;
                        IsPhoneEncounter = response.NoteComponentModel_JSON.IsPhoneEncounter;
                        var MUAlertsCount = response.NoteComponentModel_JSON.MUAlertsCount;
                        if (MUAlertsCount)
                            utility.toggelMU3Alerts(true, parseInt(MUAlertsCount));
                    }
                    Clinical_ProgressNote.params.AppointmentVisitId = VisitId;
                    var MissingConfirm = "";
                    if (isProceMissed && isProbMissed)
                        MissingConfirm = "Diagnosis(es) and Procedure(s) are missing in this encounter. Do you still want to sign?";
                    else if (isProceMissed && !isProbMissed)
                        MissingConfirm = "Procedure(s) are missing on this provider note. Do you still want to sign?";
                    else if (!isProceMissed && isProbMissed)
                        MissingConfirm = "Diagnosis(es) are missing on this provider note. Do you still want to sign?";

                    var MissingNoteSignWOCPTandICDCodeConfirm = "";
                    if (!IsNoteSignWOICDCode && !IsNoteSignWOCPTCode && isProceMissed && isProbMissed)
                        MissingNoteSignWOCPTandICDCodeConfirm = "Note cannot be signed without Diagnosis(es) and Procedure(s).";
                    else if (!IsNoteSignWOCPTCode && isProceMissed)
                        MissingNoteSignWOCPTandICDCodeConfirm = "Note cannot be signed without Procedure(s).";
                    else if (!IsNoteSignWOICDCode && isProbMissed)
                        MissingNoteSignWOCPTandICDCodeConfirm = "Note cannot be signed without Diagnosis(es).";

                    if (response.NoteComponentModel_JSON.IsNonBillable)
                        MissingNoteSignWOCPTandICDCodeConfirm = "";
                    if (MissingNoteSignWOCPTandICDCodeConfirm != "" && !IsPhoneEncounter) {
                        utility.myConfirm(MissingNoteSignWOCPTandICDCodeConfirm, function () {
                            Clinical_ProgressNote.EnableDisableConcurrentNoteSignBtn(false);
                            dfd.resolve();
                        }, function () {
                        }, '<b>Confirm Sign</b>', "OK", null, true);
                    }
                    else {
                        if (MissingConfirm != "") {
                            utility.myConfirm(MissingConfirm, function () {
                                Clinical_ProgressNote.signNote_DBCall(NotesId, VisitId, VisitDate, PatientId, ProviderId, IsFromProgressNote, true).done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status) {
                                        var isProceMissed = false;
                                        var isProbMissed = false;
                                        var ErrorMessage = "";
                                        if (response.NoteComponentModel_JSON) {
                                            sig = response.NoteComponentModel_JSON.SOAPText;
                                            BillingInfoId = response.NoteComponentModel_JSON.BillingInfoId;
                                            VisitId = response.NoteComponentModel_JSON.VisitId;
                                            isProbMissed = response.NoteComponentModel_JSON.IsProblemMissed;
                                            isProceMissed = response.NoteComponentModel_JSON.IsProcedureMissed;
                                            ErrorMessage = response.NoteComponentModel_JSON.ErrorMessage;
                                        }
                                        if (BillingInfoId > 0) {
                                            $.when(Clinical_ProgressNote.afterSigningNote(response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, VisitReason, sig, IsFromProgressNote, IsPhoneEncounter, hideMsg)).then(function () {
                                                dfd.resolve();
                                            });
                                        }
                                        else {
                                            $.when(Clinical_ProgressNote.afterSigningNote(response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, VisitReason, sig, IsFromProgressNote, IsPhoneEncounter, hideMsg)).then(function () {
                                                $.when(Clinical_ProgressNote.previewNote(response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, "Signed", VisitReason, IsPhoneEncounter)).then(function () {
                                                    dfd.resolve();
                                                    if (ErrorMessage)
                                                        utility.DisplayMessages(ErrorMessage, 2);
                                                });
                                            });
                                        }
                                    }
                                    else {
                                        dfd.resolve();
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }, function () {
                                Clinical_ProgressNote.EnableDisableConcurrentNoteSignBtn(false);
                                dfd.resolve();
                            },
                              'Confirm Sign'
                             );
                        }
                        else if (BillingInfoId > 0 && !ErrorMessage) {
                            $.when(Clinical_ProgressNote.afterSigningNote(response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, VisitReason, sig, IsFromProgressNote, IsPhoneEncounter, hideMsg, ErrorMessage)).then(function () {
                                dfd.resolve();
                            });
                        }
                        else if (!isProceMissed && !isProbMissed) {
                            $.when(Clinical_ProgressNote.afterSigningNote(response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, VisitReason, sig, IsFromProgressNote, IsPhoneEncounter, hideMsg, ErrorMessage)).then(function () {
                                if (ErrorMessage)
                                    utility.DisplayMessages(ErrorMessage, 2);
                                dfd.resolve();
                            });
                        }
                    }

                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () {
            Clinical_ProgressNote.EnableDisableConcurrentNoteSignBtn(false);
            Clinical_ProgressNote.params.CancelSign = true;
            dfd.resolve();
        }, 'Confirm Sign');
        return dfd;
    },
    afterSigningNote: function (response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, VisitReason, sig, IsFromProgressNote, IsPhoneEncounter, hideMsg, ErrorMessage) {
        var dfd = $.Deferred();
        if (IsPhoneEncounter == true) {
            $('#pnlClinicalProgressNote #Caller').text(' ' + $('#pnlClinicalProgressNote #txtCaller').val());
            $('#pnlClinicalProgressNote #Receiver').text(' ' + $('#pnlClinicalProgressNote #txtReceiver').val());
            Clinical_ProgressNote.StopTaskTime();
        }

        // PRD-433 To remove order set component from progress note soaptext, when user signs the progress note
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('.OrderSetsComponent').hide();
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ActionsInitialOfficeVisit').find("button[title='Order Sets']").hide();

        // if (IsFromProgressNote) {
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit').addClass('disableAll');
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' .splitterBody').addClass('disableAll');
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #sidebar-wrapper').addClass('disableAll');
        $('#' + Clinical_ProgressNote.params["PanelID"]).find('#ChkBox_IsNonBilable').parent().addClass('disableAll');
        $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnSave ,#btnSign , #btnReview,#btnNotesDelete,#btnCreate_eSupperbill').addClass('disabled');
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ddlVisitType').prop('disabled', true);
        $('#' + Clinical_ProgressNote.params["PanelID"]).find('#btnPrint ,#btnAssign , #btnSend,#btnCreateLetter,#btnSyndromicSurveillance,#btnNoteCoSign,#btnNoteAmendment').removeClass('disabled');
        $('#' + Clinical_ProgressNote.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "none");
        $('#' + Clinical_ProgressNote.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "none");
        $('#' + Clinical_ProgressNote.params["PanelID"] + " #btnNoteAttachment").addClass('disableAll');
        $('#' + Clinical_ProgressNote.params["PanelID"] + " #btnCreateLetter").addClass('disabled');
        $('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val('Signed');
        var isBrowserIE = providerDetail.GetIEVersion() > 0;
        $('#' + Clinical_ProgressNote.params.PanelID + ' #ProgressNoteComponentList').append(isBrowserIE ? sig.replace("System.Byte[]", "image/gif") : sig);
        //}
        if (parseInt(NotesId) > 0)
            Clinical_ProgressNote.removeSignNoteUsers();
        if (BillingInfoId > 0) {
            if (!ErrorMessage) {
                $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + VisitId + "','" + PatientId + "',event)");
                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
            }
            if (Clinical_ProgressNote.params.IsNonBilable && !ErrorMessage)
                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
            $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(BillingInfoId);
            BillingInformation.Status = "Signed";
            $.when(Clinical_ProgressNote.previewNote(response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, "Signed", VisitReason, IsPhoneEncounter)).done(function () {
                $.when(Clinical_ProgressNote.SignPatLetters(NotesId, PatientId)).done(function () {
                    dfd.resolve();
                });
                if (!hideMsg) {
                    utility.DisplayMessages("Successfully Signed!", 1);
                }
                if (Clinical_ProgressNote.params.IsNonBilable)
                    utility.DisplayMessages("eSuperbill signed successfully.", 1);
                else if (!ErrorMessage)
                    utility.DisplayMessages("Claim created successfully.", 1);
            });
        }
        else {
            if (parseInt(NotesId) > 0)
            {
                $.when(Clinical_ProgressNote.SignPatLetters(NotesId, PatientId)).done(function () {
                    if (!hideMsg) {
                        utility.DisplayMessages("Successfully Signed!", 1);
                    }
                    $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                    dfd.resolve();
                });
            }
            else {
                if (!hideMsg) {
                    utility.DisplayMessages("Successfully Signed!", 1);
                }
                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                dfd.resolve();
            }
        }
        return dfd;
    },
    SignPatLetters: function (NotesId, PatientId) {
        var objData = {};
        objData["NotesId"] = NotesId;
        objData["signedText"] = Create_Letter.SignPatientLetterForNote();
        objData["PatientId"] = PatientId;
        objData["commandType"] = "sign_pat_letter";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },
    signNoteget_DBCall: function (NotesIds) {
        var objData = {};
        objData["NotesId"] = NotesIds;
        objData["commandType"] = "sign_note_get";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    signNote_DBCall: function (NotesId, VisitId, VisitDate, PatientId, ProviderId, IsFromProgressNote, ConfirmSign) {
        var objData = {};
        objData["NotesId"] = NotesId;
        objData["VisitId"] = VisitId;
        objData["VisitDate"] = VisitDate;
        objData["PatientId"] = PatientId;
        objData["ProviderId"] = ProviderId;
        objData["IsFromProgressNote"] = IsFromProgressNote;
        objData["ConfirmSign"] = ConfirmSign;
        if (Clinical_ProgressNote.params.FromCCM)
            objData["FromCCM"] = Clinical_ProgressNote.params.FromCCM;
        else
            objData["FromCCM"] = null;

        if (sessionStorage.getItem("NoteMissingDataReason")) {
            objData["NoteMissingDataReason"] = sessionStorage.getItem("NoteMissingDataReason");
            sessionStorage.removeItem("NoteMissingDataReason");
        }
        else
            objData["NoteMissingDataReason"] = "";

        objData["commandType"] = "sign_note";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    signNote_DBCallMultiple: function (NotesIds, IsFromProgressNote) {
        var objData = {};
        objData["NotesIds"] = NotesIds;
        objData["IsFromProgressNote"] = IsFromProgressNote;
        objData["FromCCM"] = null;

        if (sessionStorage.getItem("NoteMissingDataReason")) {
            objData["NoteMissingDataReason"] = sessionStorage.getItem("NoteMissingDataReason");
            sessionStorage.removeItem("NoteMissingDataReason");
        }
        else
            objData["NoteMissingDataReason"] = "";

        objData["commandType"] = "sign_note_multiple";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },


    readytosignNote_DBCallMultiple: function (NotesIds, IsFromProgressNote) {
        var objData = {};
        objData["NotesIds"] = NotesIds;
        objData["IsFromProgressNote"] = IsFromProgressNote;
        objData["commandType"] = "note_ready_to_sign";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    previewNote: function (response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, NoteStatus, VisitReason, IsPhoneEncounter) {
        var dfd = new $.Deferred();
        Clinical_ProgressNote.patName = response.PatientName;
        Clinical_ProgressNote.patDOB = response.PatienDOB;
        Clinical_ProgressNote.ProviderName = response.ProviderName;
        Clinical_ProgressNote.DOS = response.DOS;
        Clinical_ProgressNote.patDOB = Clinical_ProgressNote.patDOB.replace('12:00AM', '');
        Clinical_ProgressNote.DOS = Clinical_ProgressNote.DOS.replace('12:00AM', '');
        Clinical_ProgressNote.params.PatientId = PatientId;
        var objHeader = $(response.ReportHeaderInfo);
        $(objHeader).find('li').each(function () {
            if ($(this).text().indexOf('DOB') > -1) {
                $(this).text($(this).text().replace('12:00AM', ''))
            }
        });
        var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
        // Adding Header Footer to Report, If Selected provider of patient has any Report Header | Change Implmeneted by Azhar Shahzad on Aug08/11/016
        if (response.ReportHeaderInfo == null || response.ReportHeaderInfo == '') {
            $("#signNotePrint header").remove();
            //Start//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
            var patientData = JSON.parse(response.NoteHeaderPatientData);
            var providerData = JSON.parse(response.NoteHeaderProviderData);
            var practiceData = JSON.parse(response.NoteHeaderPracticeData);
            if (patientData.length > 0) {
                var patientAccount = patientData[0].AccountNumber != "" ? "Acc. #: " + patientData[0].AccountNumber : "";
                var patientCell = patientData[0].CellNo != "" ? "Ph: " + patientData[0].CellNo : "";
                var patientName = (patientData[0].FirstName != "" ? patientData[0].FirstName : "") + (patientData[0].LastName != "" ? " " + patientData[0].LastName : "")
                var age = (patientData[0].Age != "" ? patientData[0].Age + " Y," : "") + " " + patientData[0].Gender + ", DOB: " + utility.RemoveTimeFromDate(null, patientData[0].DOB);
                $("#signNotePrint #PatientName").html(patientName);
                $("#signNotePrint #PatientAge").html(age);
                $("#signNotePrint #PatientAccount").html(patientAccount);
                $("#signNotePrint #PatientPhone").html(patientCell);
                $("#signNotePrint #PatientAddress").html(patientData[0].Address1);
            }
            if (providerData.length > 0) {
                var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "")
                $("#signNotePrint #ProviderName").html(providerName);
                $("#signNotePrint #Speciality").html(providerData[0].SpecialtyName);
            }
            if (practiceData.length > 0) {
                var city = practiceData[0].City;
                city += practiceData[0].State != "" ? ", " + practiceData[0].State : "";
                city += practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : "";
                $("#signNotePrint #PracticeName").html(practiceData[0].ShortName);
                $("#signNotePrint #PracticeAddress").html(practiceData[0].Address);
                $("#signNotePrint #PracticeCity").html(city);
                $("#signNotePrint #PracticePhone").html(practiceData[0].PhoneNo);
            }

            $("#signNotePrint #NoteDateTime").html(utility.RemoveTimeFromDate(null, VisitDate) + " " + VisitTime);
            $("#signNotePrint #VisitReason").html(VisitReason);

            Clinical_ProgressNote.params.BillingInfoId = BillingInfoId;
            Clinical_ProgressNote.params.NoteStatus = NoteStatus;
            $("#signNotePrint #PatientInfo").show();

            //End//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
        } else {
            response.ReportHeaderInfo = $(objHeader)[0].outerHTML;
            $("#signNotePrint #NoteDateTime").html(utility.RemoveTimeFromDate(null, VisitDate) + " " + VisitTime);
            $("#signNotePrint #VisitReason").html(VisitReason);
            Clinical_ProgressNote.params.BillingInfoId = BillingInfoId;
            Clinical_ProgressNote.params.NoteStatus = NoteStatus;

            $("#signNotePrint > div:not(#PatientInfo)").remove()
            $("#signNotePrint header").remove();
            $("#signNotePrint").prepend(response.ReportHeaderInfo);
            $("#signNotePrint #PatientInfo").hide();
        }

        Clinical_ProgressNote.params.VisitId = VisitId;
        Clinical_ProgressNote.params.VisitDate = VisitDate;

        /*Start Make Markup for PDF file*/


        $('#signNotePrint #ulContent').empty();
        $.each(NoteSections, function (i, itm) {
            if (NotesLoad_JSON[0].TemplateName) {
                if (itm.value == "Progress")
                    $("#signNotePrint #ulContent").append(itm.SectionMarkup);
            }
            else
                $('#signNotePrint #ulContent').append(itm.SectionMarkup);
        });
        var isBrowserIE = providerDetail.GetIEVersion() > 0;
        if (response.NoteComponentListFill_JSON.length > 0) {
            var temp = response.NoteComponentListFill_JSON;

            //PRD-433 To remove order set component from print preview note soaptext, when user signs the progress note
            if (NotesLoad_JSON[0].NoteStatus == 'Signed')
                temp = $.grep(response.NoteComponentListFill_JSON, function (item_) { return (item_.ComponentName != 'Order Sets') });

            $.each(temp, function (i, item) {
                //if (item.ComponentName != "Order Sets1") {
                if (item.SOAPText.indexOf('Info') > -1) {
                    item.SOAPText = item.SOAPText.split('(Info)').join('');
                }
                if (item.SectionName && item.SectionName != "PhoneEncounterData")
                    $("#signNotePrint #ulContent #" + item.SectionName + "NoteComponentList").append(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                else if (item.SectionName == "PhoneEncounterData")
                    $("#signNotePrint #ulContent ").prepend(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                else {
                    $("#signNotePrint #ulContent ").append(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                }
                //}
            });

        } else {
            $('#signNotePrint #ulContent').html(response.NotesHTML);
        }

        /*End Make Markup for PDF file*/
        var id = $('#signNotePrint #ulContent');
        if (globalAppdata["NoteFontSize"] == "10") {
            id.removeClass("font12 font14");
        } else if (globalAppdata["NoteFontSize"] == "12") {
            id.removeClass("font14");
            id.addClass("font12");
        } else if (globalAppdata["NoteFontSize"] == "14") {
            id.removeClass("font12");
            id.addClass("font14");
        }
        $('#signNotePrint #ulContent').find('li.initialVisitBody').each(function () {
            if ($(this).find('header + section').length == 0) {
                if ($(this).closest('li').find('section').length == 0) {
                    $(this).hide();
                }
            }
        });

        $('#signNotePrint #ulContent').find('li.sopTextEditable.defaultli').remove();
        $('#signNotePrint #ulContent ul').each(function () {
            if ($(this).find('li').length == 0) {
                $(this).css('min-height', '0px');
                $(this).css('padding', '0px');
            }
        });


        $('#signNotePrint #ulContent .placeholder-free-text').removeClass('placeholder-free-text');

        Clinical_ProgressNote.ShowHidePreviewHeaders();

        $('#signNotePrint #ulContent').find('.initialVisit').each(function () {
            if ($(this).find('.initialVisitBody').length == 0) {
                $(this).prev('h4').hide();
            }
        });

        var widthInit = 0;
        $('#signNotePrint ul.pull-right').each(function () {
            if ($(this).width() > widthInit) {
                widthInit = $(this).width();
            }
        })
        if (widthInit > 0)
            $('#signNotePrint ul.pull-right').width(widthInit);

        if (response.ReportFooterInfo != null && response.ReportFooterInfo != '') {
            $("#signNotePrint footer").remove();
            $("#signNotePrint").append(response.ReportFooterInfo);
        }
        $("#signNotePrint section[id*='Cli_BillingInfo']").parent().css("display", "none");
        $("#signNotePrint #clinicalMenu_BillingInfo").css("display", "none");
        BackgroundLoaderShow(true);
        Clinical_ProgressNote.getPrintnotePDF(true, IsPhoneEncounter).done(function () {
            BackgroundLoaderShow(false);
        });
        dfd.resolve('ok');


        return dfd.promise();
    },

    // Note Preview for bulk sign with deferred
    previewNoteBulkSign: function (response, NotesId, PatientId, ProviderId, VisitDate, VisitId, VisitTime, BillingInfoId, NoteStatus, VisitReason, IsPhoneEncounter) {
        $('#BackgroundLoader').show();
        var dfd = new $.Deferred();
        Clinical_ProgressNote.params.VisitDateTime = VisitDate + VisitTime;
        Clinical_ProgressNote.patName = response.ReportHeaderInfo[0].PatientName;
        Clinical_ProgressNote.patDOB = response.ReportHeaderInfo[0].PatientDOB;
        Clinical_ProgressNote.ProviderName = response.ReportHeaderInfo[0].ProviderName;
        Clinical_ProgressNote.DOS = response.ReportHeaderInfo[0].DOS;
        Clinical_ProgressNote.patDOB = Clinical_ProgressNote.patDOB.replace('12:00AM', '');
        Clinical_ProgressNote.DOS = Clinical_ProgressNote.DOS.replace('12:00AM', '');
        Clinical_ProgressNote.params.PatientId = PatientId;
        var objHeader = $(response.ReportHeaderInfo[0].Header);
        $(objHeader).find('li').each(function () {
            if ($(this).text().indexOf('DOB') > -1) {
                $(this).text($(this).text().replace('12:00AM', ''))
            }
        });
        var NotesLoad_JSON = response.NotesLoad_JSON;
        // Adding Header Footer to Report, If Selected provider of patient has any Report Header | Change Implmeneted by Azhar Shahzad on Aug08/11/016
        if (response.ReportHeaderInfo[0].Header == null || response.ReportHeaderInfo[0].Header == '') {
            $("#signNotePrint header").remove();
            var patientData = response.NoteHeaderPatientData;
            var providerData = response.NoteHeaderProviderData;
            var practiceData = response.NoteHeaderPracticeData;
            if (patientData.length > 0) {
                var patientAccount = patientData[0].AccountNumber != "" ? "Acc. #: " + patientData[0].AccountNumber : "";
                var patientCell = patientData[0].CellNo != "" ? "Ph: " + patientData[0].CellNo : "";
                var patientName = (patientData[0].FirstName != "" ? patientData[0].FirstName : "") + (patientData[0].LastName != "" ? " " + patientData[0].LastName : "")
                var age = (patientData[0].Age != "" ? patientData[0].Age + " Y," : "") + " " + patientData[0].Gender + ", DOB: " + utility.RemoveTimeFromDate(null, patientData[0].DOB);
                $("#signNotePrint #PatientName").html(patientName);
                $("#signNotePrint #PatientAge").html(age);
                $("#signNotePrint #PatientAccount").html(patientAccount);
                $("#signNotePrint #PatientPhone").html(patientCell);
                $("#signNotePrint #PatientAddress").html(patientData[0].Address1);
            }
            if (providerData.length > 0) {
                var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "")
                $("#signNotePrint #ProviderName").html(providerName);
                $("#signNotePrint #Speciality").html(providerData[0].SpecialtyName);
            }
            if (practiceData.length > 0) {
                var city = practiceData[0].City;
                city += practiceData[0].State != "" ? ", " + practiceData[0].State : "";
                city += practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : "";
                $("#signNotePrint #PracticeName").html(practiceData[0].ShortName);
                $("#signNotePrint #PracticeAddress").html(practiceData[0].Address);
                $("#signNotePrint #PracticeCity").html(city);
                $("#signNotePrint #PracticePhone").html(practiceData[0].PhoneNo);
            }
            $("#signNotePrint #NoteDateTime").html(utility.RemoveTimeFromDate(null, VisitDate) + " " + VisitTime);
            $("#signNotePrint #VisitReason").html(VisitReason);

            Clinical_ProgressNote.params.BillingInfoId = BillingInfoId;
            Clinical_ProgressNote.params.NoteStatus = NoteStatus;
            $("#signNotePrint #PatientInfo").show();
        } else {
            var ReportHeaderInfo = $(objHeader)[0].outerHTML;
            $("#signNotePrint #NoteDateTime").html(utility.RemoveTimeFromDate(null, VisitDate) + " " + VisitTime);
            $("#signNotePrint #VisitReason").html(VisitReason);
            Clinical_ProgressNote.params.BillingInfoId = BillingInfoId;
            Clinical_ProgressNote.params.NoteStatus = NoteStatus;

            $("#signNotePrint > div:not(#PatientInfo)").remove()
            $("#signNotePrint header").remove();
            $("#signNotePrint").prepend(ReportHeaderInfo);
            $("#signNotePrint #PatientInfo").hide();
        }

        Clinical_ProgressNote.params.VisitId = VisitId;
        Clinical_ProgressNote.params.VisitDate = VisitDate;
        /*Start Make Markup for PDF file*/

        $('#signNotePrint #ulContent').empty();
        $.each(NoteSections, function (i, itm) {
            if (NotesLoad_JSON[0].TemplateName) {
                if (itm.value == "Progress")
                    $("#signNotePrint #ulContent").append(itm.SectionMarkup);
            }
            else
                $('#signNotePrint #ulContent').append(itm.SectionMarkup);
        });
        var isBrowserIE = providerDetail.GetIEVersion() > 0;
        if (response.NoteComponentListFill_JSON.length > 0) {
            $.each(response.NoteComponentListFill_JSON, function (i, item) {
                if (item.SOAPText.indexOf('Info') > -1) {
                    item.SOAPText = item.SOAPText.split('(Info)').join('');
                }
                if (item.SectionName && item.SectionName != "PhoneEncounterData")
                    $("#signNotePrint #ulContent #" + item.SectionName + "NoteComponentList").append(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                else if (item.SectionName == "PhoneEncounterData")
                    $("#signNotePrint #ulContent ").prepend(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                else {
                    $("#signNotePrint #ulContent ").append(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                }
            });
        } else {
            $('#signNotePrint #ulContent').html(NotesLoad_JSON[0].NoteText);
        }

        /*End Make Markup for PDF file*/
        var id = $('#signNotePrint #ulContent');
        if (globalAppdata["NoteFontSize"] == "10") {
            id.removeClass("font12 font14");
        } else if (globalAppdata["NoteFontSize"] == "12") {
            id.removeClass("font14");
            id.addClass("font12");
        } else if (globalAppdata["NoteFontSize"] == "14") {
            id.removeClass("font12");
            id.addClass("font14");
        }
        $('#signNotePrint #ulContent').find('li.initialVisitBody').each(function () {
            if ($(this).find('header + section').length == 0) {
                if ($(this).closest('li').find('section').length == 0) {
                    $(this).hide();
                }
            }
        });

        $('#signNotePrint #ulContent').find('li.sopTextEditable.defaultli').remove();
        $('#signNotePrint #ulContent ul').each(function () {
            if ($(this).find('li').length == 0) {
                $(this).css('min-height', '0px');
                $(this).css('padding', '0px');
            }
        });

        $('#signNotePrint #ulContent .placeholder-free-text').removeClass('placeholder-free-text');
        Clinical_ProgressNote.ShowHidePreviewHeaders();
        $('#signNotePrint #ulContent').find('.initialVisit').each(function () {
            if ($(this).find('.initialVisitBody').length == 0) {
                $(this).prev('h4').hide();
            }
        });

        var widthInit = 0;
        $('#signNotePrint ul.pull-right').each(function () {
            if ($(this).width() > widthInit) {
                widthInit = $(this).width();
            }
        })
        if (widthInit > 0)
            $('#signNotePrint ul.pull-right').width(widthInit);

        if (response.ReportHeaderInfo[0].Footer != null && response.ReportHeaderInfo[0].Footer != '') {
            $("#signNotePrint footer").remove();
            $("#signNotePrint").append(response.ReportHeaderInfo[0].Footer);
        }
        $("#signNotePrint section[id*='Cli_BillingInfo']").parent().css("display", "none");
        $("#signNotePrint #clinicalMenu_BillingInfo").css("display", "none");
        Clinical_ProgressNote.getPrintnotePDFBulkSign(true, IsPhoneEncounter, NotesId).done(function () {
            $('#BackgroundLoader').hide();
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    // Print Note PDFPrintnotePDF for bulk sign with deferred
    getPrintnotePDFBulkSign: function (isSignNote, IsPhoneEncounter, NotesId) {
        var def = $.Deferred();
        var FooterText = $("#signNotePrint footer").text().split('Generated by: ').join('');
        // ----- footer
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $("#page-templateSignNote").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(FooterText);
            $(PageTemp).find('#patientname').text(Clinical_ProgressNote.patName);
            $(PageTemp).find('#providerName').text(Clinical_ProgressNote.ProviderName);
            $(PageTemp).find('#patientDOB').text(Clinical_ProgressNote.patDOB);
            $(PageTemp).find('#patientDOS').text(Clinical_ProgressNote.DOS);
            $("#page-templateSignNote").html(PageTemp);
        }
        else {
            var footerText = $("#signNotePrint footer").text().split('Generated by: ').join('');
            var insideHTML = $("#page-templateSignNote").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(footerText);
            $(PageTemp).find('#patientname').text(Clinical_ProgressNote.patName);
            $(PageTemp).find('#providerName').text(Clinical_ProgressNote.ProviderName);
            $(PageTemp).find('#patientDOB').text(Clinical_ProgressNote.patDOB);
            $(PageTemp).find('#patientDOS').text(Clinical_ProgressNote.DOS);
            $("#page-templateSignNote").html(PageTemp);
        }
        var insideHTML = $("#page-templateSignNote").html();
        var PageTemp = $(insideHTML);
        $("#page-templateSignNote").html(PageTemp);
        var height = 0;
        var topMargin = height <= 18 ? 18 : height + 3;
        if ($("#page-templateSignNote .blueBorderPrint").length >= 1) {
            $("#signNotePrint .form-group").remove();
            $("#signNotePrint footer").remove();
        }
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM("#signNotePrint", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($("#page-templateSignNote").html())
        }).then(function (group) {
            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                var myObject = new Object();
                myObject.PDFData = PrintPDFDataURL;
                myObject.PatientID = Clinical_ProgressNote.params.PatientId;
                if (IsPhoneEncounter) {
                    myObject.FolderName = "Phone Encounter";
                } else {
                    myObject.FolderName = "Progress Notes";
                }
                var datetime = Clinical_ProgressNote.params.VisitDateTime;
                datetime = datetime.replace("/", "-").replace(":", "").replace(" ", "");
                datetime = datetime.replace("/", "-").replace(":", "").replace(" ", "");
                var mnth = datetime.split('-')[0];
                var day = datetime.split('-')[1];
                var yearandtime = datetime.split('-')[2];
                mnth = mnth.length == 1 ? "0" + mnth : mnth;
                day = day.length == 1 ? "0" + day : day;
                datetime = mnth + "." + day + "." + yearandtime;
                var dt = datetime.substr(0, 10);
                var tm = datetime.substr(10, datetime.length + 1);
                datetime = dt;
                var providerLastName = Clinical_ProgressNote.ProviderName.split(' ')[1];
                var providerFirstName = Clinical_ProgressNote.ProviderName.split(' ')[0].substr(0, 1);
                var fname = datetime + "_" + providerLastName + ", " + providerFirstName;
                myObject.FileName = fname + ".pdf";
                myObject.FileType = "application/pdf";
                myObject.FolderId = "12";
                myObject.NotesId = NotesId;
                myObject.DOS = Clinical_ProgressNote.DOS;
                DashBoard.bulkSignNotes.push(myObject);
                $('#progressnotesign').remove();
                $('#signNotePrint #ulContent').html("");
                def.resolve();
            });
        });
        return def.promise();
    },

    getPrintnotePDF: function (isSignNote, IsPhoneEncounter) {
        var def = $.Deferred();
        var FooterText = $("#signNotePrint footer").text().split('Generated by: ').join('');
        $("#signNotePrint").show();
        $("#signNotePrint").css("display", "inline");
        // ----- footer
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $("#page-templateSignNote").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(FooterText);
            $(PageTemp).find('#patientname').text(Clinical_ProgressNote.patName);
            $(PageTemp).find('#providerName').text(Clinical_ProgressNote.ProviderName);
            $(PageTemp).find('#patientDOB').text(Clinical_ProgressNote.patDOB);
            $(PageTemp).find('#patientDOS').text(Clinical_ProgressNote.DOS);

            $("#page-templateSignNote").html(PageTemp);
        }
        else {
            var footerText = $("#signNotePrint footer").text().split('Generated by: ').join('');
            var insideHTML = $("#page-templateSignNote").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(footerText);
            $(PageTemp).find('#patientname').text(Clinical_ProgressNote.patName);
            $(PageTemp).find('#providerName').text(Clinical_ProgressNote.ProviderName);
            $(PageTemp).find('#patientDOB').text(Clinical_ProgressNote.patDOB);
            $(PageTemp).find('#patientDOS').text(Clinical_ProgressNote.DOS);
            $("#page-templateSignNote").html(PageTemp);

        }
        var insideHTML = $("#page-templateSignNote").html();
        var PageTemp = $(insideHTML);
        $("#page-templateSignNote").html(PageTemp);
        var height = 0;

        var topMargin = height <= 18 ? 18 : height + 3;

        if ($("#page-templateSignNote .blueBorderPrint").length >= 1) {
            $("#signNotePrint .form-group").remove();
            $("#signNotePrint footer").remove();
        }
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM("#signNotePrint", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($("#page-templateSignNote").html())
        }).then(function (group) {
            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                Clinical_ProgressNote.params["PDFData"] = PrintPDFDataURL;
                Clinical_ProgressNote.pdf = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                var data = new FormData();
                data.append('notes', Clinical_ProgressNote.params["PDFData"]);
                data.append("PatientID", Clinical_ProgressNote.params.PatientId);
                data.append("Folder", "12");
                IsPhoneEncounter ? data.append("FolderName", "Phone Encounter") : data.append("FolderName", "Progress Notes")
                var datetime = Clinical_ProgressNote.params.VisitDateTime;
                datetime = datetime.replace("/", "-").replace(":", "").replace(" ", "");
                datetime = datetime.replace("/", "-").replace(":", "").replace(" ", "");
                var mnth = datetime.split('-')[0];
                var day = datetime.split('-')[1];
                var yearandtime = datetime.split('-')[2];
                mnth = mnth.length == 1 ? "0" + mnth : mnth;
                day = day.length == 1 ? "0" + day : day;
                datetime = mnth + "." + day + "." + yearandtime;

                var dt = datetime.substr(0, 10);
                var tm = datetime.substr(10, datetime.length + 1);
                //datetime = dt + " " + tm;
                datetime = dt;
                var providerLastName = Clinical_ProgressNote.ProviderName.split(' ')[1];
                var providerFirstName = Clinical_ProgressNote.ProviderName.split(' ')[0].substr(0, 1);
                var fname = datetime + "_" + providerLastName + ", " + providerFirstName;

                data.append("FileName", fname + ".pdf");
                data.append("fileType", "application/pdf");

                var myObject = new Object();
                myObject.ddlFolder = "12";
                // prd-41
                myObject.noteVisitDate = Clinical_ProgressNote.params.VisitDate;
                myObject.TransitionId = Clinical_ProgressNote.params.NotesId;
                if (!IsPhoneEncounter) {
                    myObject.RefModuleName = "Progress Notes";
                }
                else {
                    myObject.RefModuleName = "Phone Encounter";
                }
                var myJSON = JSON.stringify(myObject);
                data.append("PatientDocumentData", myJSON);
                Document_Import.SaveImport(data).done(function (response) {
                    $('#progressnotesign').remove();
                    $('#signNotePrint #ulContent').html("");
                    def.resolve("ok");
                });
                $("#signNotePrint").hide();
            });

        });
        // ------------------------------------- End Download functionality--------------------------------------

        return def.promise();
    },
    ShowHidePreviewHeaders: function () {
        var contentList = '#signNotePrint #ulContent'
        var subjComponents = $(contentList + " #SubjectiveNoteComponentList li.editableContentli");
        var objComponents = $(contentList + " #ObjectiveNoteComponentList li.editableContentli");
        var assesComponents = $(contentList + " #AssessmentNoteComponentList li.editableContentli");
        var progressComponents = $(contentList + " #ProgressNoteComponentList li.editableContentli");
        var planComponents = $(contentList + " #PlanNoteComponentList li.editableContentli");
        var subjHeaders = $(contentList + " #SubjectiveNoteComponentList header + section");
        if (subjHeaders.length == 0) {
            subjHeaders = $(contentList + " #SubjectiveNoteComponentList").find('header').next().find('section');
        }
        var objHeaders = $(contentList + " #ObjectiveNoteComponentList header + section");
        if (objHeaders.length == 0) {
            objHeaders = $(contentList + " #ObjectiveNoteComponentList").find('header').next().find('section');
        }
        var assesHeaders = $(contentList + " #AssessmentNoteComponentList header + section");
        if (assesHeaders.length == 0) {
            assesHeaders = $(contentList + " #AssessmentNoteComponentList").find('header').next().find('section');
        }
        var planHeaders = $(contentList + " #PlanNoteComponentList header + section");
        if (planHeaders.length == 0) {
            planHeaders = $(contentList + " #PlanNoteComponentList").find('header').next().find('section');
        }
        //Begin Edited By Fahad Malik on 16-Dec-2016 to fix bug#: EMR-2231
        var ProgressNotehtm = $(contentList + " #ProgressNoteComponentList header #clinicalMenu_BillingInfo");
        ProgressNotehtm.closest('li').addClass("hidden");
        //End Edited By Fahad Malik on 16-Dec-2016 to fix bug#: EMR-2231
        if ((subjComponents && subjComponents.length > 0) || (subjHeaders && subjHeaders.length > 0)) {
            $(contentList + " #SubjectiveNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #SubjectiveNoteComponentList").prev('h4').remove();
            $(contentList + " #SubjectiveNoteComponentList").remove();
        }
        if ((objComponents && objComponents.length > 0) || (objHeaders && objHeaders.length > 0)) {
            $(contentList + " #ObjectiveNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #ObjectiveNoteComponentList").prev('h4').remove();
            $(contentList + " #ObjectiveNoteComponentList").remove();
        }
        if ((assesComponents && assesComponents.length > 0) || (assesHeaders && assesHeaders.length > 0)) {
            $(contentList + " #AssessmentNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #AssessmentNoteComponentList").prev('h4').remove();
            $(contentList + " #AssessmentNoteComponentList").remove();
        }
        if ((planComponents && planComponents.length > 0) || (planHeaders && planHeaders.length > 0)) {
            $(contentList + " #PlanNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #PlanNoteComponentList").prev('h4').remove();
            $(contentList + " #PlanNoteComponentList").remove();
        }
        $(contentList + " #ProgressNoteComponentList").find('clinical_billinginfo').closest('li.initialVisitBody').remove();

    },
    SignESuperBill: function (BillingInformation, Obj, customSigMsg, isComponentSelect, BillingInfoResponse) {
        var dfd = $.Deferred();
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!(Clinical_ProgressNote.params.IsPhoneEncounter || Clinical_ProgressNote.params.IsOutOfOfficeVisit) && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
            dfd.resolve();
        }
        else {
            Clinical_ProgressNote.params.IsOutOfOfficeVisit = false;
            if (BillingInfoResponse && BillingInfoResponse.BillingInfoFill_JSON) {

                $.when(Clinical_ProgressNote.BillingInfoSigned(BillingInfoResponse, BillingInformation, Obj, customSigMsg, isComponentSelect)).then(function () {
                    dfd.resolve();
                });

            } else {
                //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
                Clinical_ProgressNote.BillingInformationLoad().done(function (response) {
                    response = JSON.parse(response);
                    $.when(Clinical_ProgressNote.BillingInfoSigned(response, BillingInformation, Obj, customSigMsg, isComponentSelect)).then(function () {
                        dfd.resolve();
                    });
                });
            }

        }
        // findInDiv.hide(true);
        return dfd;
    },
    BillingInfoSigned: function (response, BillingInformation, Obj, customSigMsg, isComponentSelect) {
        var dfdFinal = $.Deferred();

        var billingDetail = null;

        var isESuperBillSigned = false;
        var isJustNoteSign = false;
        if (response.status != false) {
            billingDetail = JSON.parse(response.BillingInfoFill_JSON);
        }
        var signMessage = "eSuperbill will also be signed. Are you sure you want to sign the provider note?";
        if (BillingInformation != null && Obj != null && customSigMsg != null && customSigMsg != "") {
            signMessage = customSigMsg;
        }
        else if (BillingInformation != null && Obj != null) {
            signMessage = "The provider note will also be signed. Are you sure you want to sign eSuperbill?";
        }
        if (billingDetail != null && billingDetail.Status == "Signed") {
            isESuperBillSigned = true;
            Clinical_ProgressNote.params.isESuperBillSigned = true;
            customSigMsg == null || customSigMsg == '' ? signMessage = "Are you sure you want to sign the provider note?" : signMessage = customSigMsg;

        } else if (billingDetail.BillingInfoId > 0) {
            if (customSigMsg == null || customSigMsg == "") {

                if (billingDetail.NotesId == null || billingDetail.NotesId == "" || billingDetail.NotesId < 0) {
                    signMessage = "Are you sure you want to sign the provider note?";
                    isJustNoteSign = true;
                }

            } else {
                signMessage = customSigMsg;
            }
        }
        utility.myConfirm(signMessage, function () {
            $('#pnlClinicalProgressNote #hfNoteStatus').val('Signed');
            var deffered = $.Deferred();
            var self = $("#" + Clinical_Notes.params["PanelID"]);
            var prntctlIsNotScheduler = true;
            $('#pnlClinicalProgressNote #hfNoteText').val($('#pnlClinicalProgressNote #ProgressnoteHTML').html());
            var myJSON = self.getMyJSONByName();
            var NotesId = Clinical_Notes.params.NotesId;
            if (BillingInformation != null && Obj != null) {
                NotesId = Obj.NotesId;
                if (Obj.prntCtrl && (Obj.prntCtrl == "schTabCalendar" || Obj.prntCtrl == "schTabMultipleView"))
                    prntctlIsNotScheduler = false;
            }
            Clinical_ProgressNote.CreateCharges(Obj);
            if ($('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val() == 'Signed') {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
            }
            else {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
            }
            dfdFinal.resolve();

        }, function () {
            Clinical_ProgressNote.params.CancelSign = true;
            dfdFinal.resolve();
        },
              'Confirm Sign'
              );
        return dfdFinal;
    },
    addComment: function (id) {
        if ($("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id).find('#Comments_' + $("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id).attr('id')).length == 0) {
            $("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id).find('ul').append('<li id="Comments_' + $("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id).attr("id") + '" class="mt-md" style="word-wrap:break-word"></li>')
        }
        if ($("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id).closest('ul').parent().find('textarea').length <= 0) {
            $('#Comments_' + $("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id).attr('id')).hide();
            Clinical_ProgressNote.openTinyEditor($("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id), false);
        } else {
            $('#Comments_' + $("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id).attr('id')).hide();
            Clinical_ProgressNote.openTinyEditor($("#" + Clinical_ProgressNote.params["PanelID"] + " #" + id), false);
        }

    },
    OrderSetSOAPText: function (orderSetId, OrderSetName, Comments) {

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ordersets').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
            if ($(CompnentSelector).length > 0) {
                $(CompnentSelector).append(' <li class="OrderSetsComponent" NoteComponentId="NCDummyId"> <header>' +
                '<clinical_ordersets title="Order Sets" id="' + this.id + '" class="NotesComponent">' +
    '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'OrderSets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Order Sets">Order Sets</a> ' +
    '<a id="OSCoRemove" onclick="Clinical_ProgressNote.RemoveComponentTab(\'Order Sets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
    '</clinical_ordersets> </header></li>');
            } else {
                var insertedinPlan = false;

                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList').append('<li class="OrderSetsComponent" NoteComponentId="NCDummyId"> <header>' +
                '<clinical_ordersets title="Order Sets" id="' + this.id + '" class="NotesComponent">' +
                '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'OrderSets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Order Sets">Order Sets</a> ' +
                '<a id="OSCoRemove" onclick="Clinical_ProgressNote.RemoveComponentTab(\'Order Sets\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                '</clinical_ordersets> </header></li>');
            }


            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #ProgressNoteComponentList';
        var attachedOS = $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().find('div');


        var OrderSet_ID = "";

        OrderSet_ID = orderSetId;

        var attachedOSName = $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().find('#Cli_OrderSets_Main' + OrderSet_ID + ' ul').html();

        var checkAttachedOSName = attachedOSName;
        if (typeof attachedOSName != typeof undefined && attachedOSName != null && attachedOSName != "") {
            checkAttachedOSName = attachedOSName.split('<br>')[0].trim();
        }
        var CommentsVar = "";
        if (typeof Comments != typeof undefined && Comments != null && Comments != "") {
            CommentsVar = Comments;
        }
        if (checkAttachedOSName != OrderSetName) {

            var $mainDivFollowUp = $(document.createElement('div'));
            var $sectionBodyFollowUp = $(document.createElement('section'));
            var $detailsDiv = $(document.createElement('div'));

            $sectionBodyFollowUp.attr('id', "Cli_OrderSets_Main" + OrderSet_ID);
            $sectionBodyFollowUp.attr('IsDefaultOrderSet', "1");
            $sectionBodyFollowUp.append(' <div class="pull-right hidden">' +
            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_OrderSets_Main" + OrderSet_ID + '" ><i class="fa fa-times"></i></a>'
            + '</div> ');

            var $listFollowUp = $(document.createElement('ul'));
            // var txt = $.parseHTML(SoapText);

            $listFollowUp.append(OrderSetName + (CommentsVar != '' ? '<br>' + CommentsVar : ''));
            $detailsDiv.append($listFollowUp);
            $sectionBodyFollowUp.append($detailsDiv);
            $mainDivFollowUp.append($sectionBodyFollowUp);
            $mainDivFollowUp.html()

            $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().addClass('initialVisitBody');

            $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().append($mainDivFollowUp.html());
        } else {
            $(noteHTMLCtrl + ' clinical_ordersets').parent().parent().find('#Cli_OrderSets_Main' + OrderSet_ID + ' ul').html(OrderSetName + (CommentsVar != '' ? '<br>' + CommentsVar : ''));
        }
    },

    ResetDefaultOrderSet: function () {
        self = $('#' + Clinical_ProgressNote.params.PanelID);
        if (Clinical_ProgressNote.DefaultOrderSetId) {
            self.find(".OrderSet option:contains(" + Clinical_ProgressNote.DefaultOrderSetId + ")").attr('selected', true);
            //self.find('#ddlOrderSet option').filter(function () {
            //    return $.trim($(this).val()) == Clinical_ProgressNote.DefaultOrderSetId
            //}).attr('selected', true);
        }
    },
    DataFixesPDF: function (NotesIds) {
        Clinical_ProgressNote.DataFixes_DbCall(NotesIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status == true)
                console.log(response.Message);
            else
                console.log(response.Message);
        });
    },
    DataFixes_DbCall: function (NotesIds) {
        var objNotesModel = new Object();
        objNotesModel.NotesIds = NotesIds;
        return MDVisionService.APIServiceComplex(objNotesModel, "ClinicalNotes", "CreateNotesPDFFixesFiles");
    },


    //------Orthopedic Chart Starts

    OrthopedicChart: function () {

        var params = [];
        var PanelID = "";
        params["ParentCtrl"] = 'clinicalTabProgressNote';
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["FromAdmin"] = "0";
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];

        if (Clinical_ProgressNote.params["IsBodyPart"] == true) {
            LoadActionPan('Clinical_OrthopedicChartDetail', params);
        }
        else {
            LoadActionPan('Clinical_OrthopedicChart', params);
        }




    },

    //------Orthopedic Chart Ends

}


$.fn.textWidth = function () {
    var sensor = $('<label />').css({
        margin: 0, padding: 0
    });
    $("#" + Clinical_ProgressNote.params["PanelID"]).append(sensor);
    sensor.text($(this).val());
    var width = sensor.width();
    sensor.remove();
    return width;
};