Clinical_NotesSearch = {
    bIsFirstLoad: true,
    params: [],
    arrCQMReasoning: [],
    arrVBPReasoning: [],
    Load: function (params) {
        Clinical_NotesSearch.params = params;
        if (Clinical_NotesSearch.params != null && Clinical_NotesSearch.params.PanelID != "Clinical_NotesSearch") {
            Clinical_NotesSearch.params["PanelID"] = Clinical_NotesSearch.params["PanelID"] + ' #Clinical_NotesSearch';
        }
        else {
            Clinical_NotesSearch.params = [];
            Clinical_NotesSearch.params["PanelID"] = "Clinical_NotesSearch"
        }

        if (Clinical_NotesSearch.bIsFirstLoad) {
            Clinical_NotesSearch.bIsFirstLoad = false;
            var self = $('#Clinical_NotesSearch');
            self.loadDropDowns(true).done(function () {
                $('#Clinical_NotesSearch select[id=ddlNoteStatus] option:eq(1)').prop('selected', 'selected');
                $('#Clinical_NotesSearch #ddlprovider').val(globalAppdata["DefaultProviderId"]);
                $('#Clinical_NotesSearch select[id=ddlType] option:eq(1)').prop('selected', 'selected');
                Clinical_NotesSearch.NotesSearch(null, null);
            });
        }

        utility.CreateDatePicker('Clinical_NotesSearch #dpVisitFrom', function () {
        });
        utility.CreateDatePicker('Clinical_NotesSearch #dpVisitTo', function () {
        });
        if (Clinical_NotesSearch.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_NotesSearch.params.PanelID + " div#FaceSheetPager", Clinical_NotesSearch.params.FaceSheetComponents, 'notes');

            $('#' + Clinical_NotesSearch.params.PanelID + ' #sectionNotesSearch').css("display", "none");
        } else if (Clinical_NotesSearch.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #Clinical_NotesSearch' + " div#FaceSheetPager", Clinical_NotesSearch.params.FaceSheetComponents, 'notes');
            $('#' + Clinical_NotesSearch.params.PanelID + ' #sectionNotesSearch').css("display", "none");
        }
        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1356px");
            }
        });
    },

    // Grid Load And Search Functions

    NotesSearch: function (PageNo, rpp) {
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        utility.ValidateFromToDate('frmNotesSearch', 'dpVisitFrom', 'dpVisitTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        if ($("#Clinical_NotesSearch #pnlNotes_Result").css("display") == "none")
            $("#Clinical_NotesSearch #pnlNotes_Result").show();

        var self = $("#Clinical_NotesSearch");
        var myJSON = self.getMyJSONByName();

        Clinical_NotesSearch.SearchNotes(myJSON, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_NotesSearch.NotesSearchGridLoad(response);
                var TableControl = "Clinical_NotesSearch #dgvNotes";
                var PagingPanelControlID = "Clinical_NotesSearch #divAllNotesPaging";
                var ClassControlName = "Clinical_NotesSearch";
                var PagesToDisplay = 5;
                var iTotalDraftDisplayRecords = response.NotesCount;

                setTimeout(CreatePagination(response.NotesCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDraftDisplayRecords, function (a, PageNumber, ResultPerPage) {
                    Clinical_NotesSearch.NotesSearch(PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },


    NotesSearchGridLoad: function (response) {

        $("#Clinical_NotesSearch #dgvNotes").dataTable().fnDestroy();
        $("#Clinical_NotesSearch #dgvNotes tbody").find("tr").remove();

        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.NotesCount > 0) {
                var EncounterSearchJSONData = JSON.parse(response.NotesLoad_JSON);
                //  $('#Notes #spnNotesCount').text(response.NotesCount);
                //  $('#pnlDashboard div.wEncounter .badge').text(response.NotesCount);
                //$('#wpanel .slick-track div').each(function (i) {
                //    if ($(this).find('span:first').text() == 'Notes') {
                //        $(this).find('span:last').text(response.NotesCount);
                //        $(this).find('span:last').show();
                //    }

                //});
                // $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').text(response.NotesCount);
                $.each(EncounterSearchJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("id", "gvClinical_NotesSearch_row" + i + "");

                    var ClassDisabled = item.NoteStatus.toUpperCase() == "SIGNED" ? "disabled" : "";

                    var DemographicsMethod = "Clinical_NotesSearch.PatientDemographics('" + item.PatientId + "');";

                    var NoteDeleteMethod = "Clinical_NotesSearch.NotesDelete('" + item.NotesId + "');"

                    //var NoteStatusUpdateMethod = "Clinical_NotesSearch.NotesStatusUpdate('" + item.NotesId + "');"

                    var NoteStatusUpdateMethod = "Clinical_NotesSearch.NotesStatusUpdate('" + item.NotesId + "', '" + item.PatientId + "', '" + item.ProviderId + "',false,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "'," + item.BillingInfoId + ",'" + item.AppointmentDate + "'," + item.VisitId + ",'" + utility.RemoveTimeFromDate(null, item.NoteDate) + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "False" ? false : true) + ");";

                    var NotesPreview = "Clinical_NotesSearch.NotesPreview('" + item.NotesId + "',null,'" + item.PatientId + "','" + item.ProviderId + "','" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "False" ? false : true) + ",'" + item.BillingStatus + "');";

                    var EditProgressNoteMethod = "Clinical_NotesSearch.EditProgressNote('" + item.NotesId + "', '" + item.PatientId + "');"

                    var isVisible = 'style="display:none;';
                    if (response.HasDeleteRights != "No") {
                        isVisible = "";
                    }
                    if (item.NoteStatus.toUpperCase() == "SIGNED") {
                        $row.attr("onclick", NotesPreview);
                    }
                    else {
                        $row.attr("onclick", "Clinical_NotesSearch.EditProgressNoteRow(" + item.NotesId + ", " + item.PatientId + ", event);");
                    }
                    if (canSign) {
                        if (item.NoteStatus != "Signed")
                            $row.append('<td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + NoteDeleteMethod + '" ' + ' ' + isVisible + ' title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + EditProgressNoteMethod + '" title="Edit Note"> <i class="fa fa-edit black"></i></a> <a class="btn  btn-xs" href="#" onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + NoteStatusUpdateMethod + '" title="Sign Note"> <i class="fa fa-calculator black"></i></a>  </td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.AccountNumber + '</td><td>' + item.ProviderName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.CreatedBy + '</td><td>' + item.NoteStatus + '</td>');
                        else 
                            $row.append('<td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + NoteDeleteMethod + '" ' + ' ' + isVisible + ' title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + EditProgressNoteMethod + '" title="Edit Note"> <i class="fa fa-edit black"></i></a> <a class="btn  btn-xs" href="#" onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.AccountNumber + '</td><td>' + item.ProviderName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.CreatedBy + '</td><td>' + item.NoteStatus + '</td>');
                    }
                    else {
                        //$row.append('<td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + NoteDeleteMethod + '" ' + ' ' + isVisible + ' title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + EditProgressNoteMethod + '" title="Edit Note"> <i class="fa fa-edit black"></i></a> <a class="btn  btn-xs" href="#" onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.AccountNumber + '</td><td>' + item.ProviderName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.CreatedBy + '</td><td>' + item.NoteStatus + '</td>');
                        $row.append('<td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + NoteDeleteMethod + '" ' + ' ' + isVisible + ' title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="' + EditProgressNoteMethod + '" title="Edit Note"> <i class="fa fa-edit black"></i></a> <a class="btn  btn-xs" href="#" onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.AccountNumber + '</td><td>' + item.ProviderName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.CreatedBy + '</td><td>' + item.NoteStatus + '</td>');
                    }

                    $("#Clinical_NotesSearch #dgvNotes tbody").last().append($row);
                });
            }
            else {
                //$("#pnlDashboard #divEncounterPaging").css("display", "none");
                $('#Clinical_NotesSearch #dgvNotes').DataTable({
                    "language": {
                        "emptyTable": "No Notes/Phone Encounters Found"
                        //}, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }
            if ($.fn.dataTable.isDataTable('#Clinical_NotesSearch #dgvNotes'))
                ;
            else
                $("#Clinical_NotesSearch #dgvNotes").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            //$("#Clinical_NotesSearch #dgvNotes").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //  $('.table-responsive').css('min-height', '220px');


            $("#Clinical_NotesSearch #dgvNotes th")[7].click();

        });
    },

    SetNotesCount:function(){
        Clinical_NotesSearch.SearchNotesCount().done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                $("#spnNotesCount").text(response.Count);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    SearchNotesCount: function () {
        var objData = new Object();
        objData["commandType"] = "search_notes_count";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Notes");
    },


    SearchNotes: function (NotesData, PageNumber, RowsPerPage) {


        var noteType = "";
        if ($('#Clinical_NotesSearch #ddlType option:selected').text() == "- Select -") {
            noteType = ""
        } else {
            noteType = $('#Clinical_NotesSearch #ddlType option:selected').text();
        }
      
        var noteStatus = "";
        if ($('#Clinical_NotesSearch #ddlNoteStatus option:selected').text() == "- Select -") {
            noteStatus = ""
        } else {
            noteStatus = $('#Clinical_NotesSearch #ddlNoteStatus option:selected').text();
        }

        var objData = JSON.parse(NotesData);
        if (Clinical_NotesSearch.params.ParentCtrl == "clinicalTabFaceSheet") {

            objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();

        } else if (Clinical_NotesSearch.params.ParentCtrl == "Clinical_FaceSheet") {
            objData["PatientId"] = Clinical_FaceSheet.params.patientID;
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }

        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        objData["PageNo"] = PageNumber;
        objData["rpp"] = RowsPerPage;
        objData["NoteStatus"] = noteStatus;
        objData["NoteType"] = noteType;
        if (Clinical_NotesSearch.params.ParentCtrl == "clinicalTabFaceSheet") {
            objData["NoteStatus"] = "";
            objData["NoteType"] = "";
        }
        objData["commandType"] = "SEARCH_NOTES";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Notes");
    },
    //-------------------------------

    UnLoad: function () {
        var parentPanelId = null;
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_NotesSearch.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_NotesSearch.params.ParentCtrl == "Clinical_FaceSheet") {
            if (Clinical_NotesSearch.params["FromAdmin"] == "0") {
                if (Clinical_NotesSearch.params != null && Clinical_NotesSearch.params.ParentCtrl != null) {
                    if (Clinical_NotesSearch.params.ParentCtrl == "Clinical_FaceSheet") {
                        parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                        Clinical_FaceSheet.params.ChildPanelID = null;
                        UnloadActionPan(Clinical_NotesSearch.params.ParentCtrl, 'Clinical_NotesSearch', null, parentPanelId);
                    } else {
                        UnloadActionPan(Clinical_NotesSearch.params.ParentCtrl, 'Clinical_NotesSearch');
                    }
                }
                else
                    UnloadActionPan(null, 'Clinical_NotesSearch');
            }
            else {
                RemoveAdminTab();
            }
            objDeffered.resolve();
            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Clinical_NotesSearch.params != null && Clinical_NotesSearch.params.ParentCtrl) {
                UnloadActionPan(Clinical_NotesSearch.params.ParentCtrl);
                // Clinical_NotesSearch.params = null;
            }
            else {
                UnloadActionPan();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },


    NotesDelete: function (NotesId, event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = NotesId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Clinical_Notes.NotesDeleted(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_NotesSearch.NotesSearch();
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

    //NotesStatusUpdate: function (NotesId) {
    //    AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //        if (strMessage == "") {
    //            utility.myConfirm('Do you want to Sign this record?', function () {
    //                //Single Generic working function change too from Clinical_NotesSearch to DashBoard
    //                DashBoard.NotesUpdate(NotesId).done(function (response) {
    //                    if (response.status != false) {
    //                        utility.DisplayMessages("Successfully Signed!", 1);
    //                        Clinical_NotesSearch.NotesSearch();
    //                        // Clinical_Notes.UnLoad();
    //                        //UnloadActionPan(Clinical_Notes.params["ParentCtrl"]);
    //                    }
    //                    else {
    //                        utility.DisplayMessages(response.Message, 3);
    //                    }
    //                });

    //            }, function () { },
    //                    'Confirm Sign'
    //                );
    //        }
    //        else {
    //            utility.DisplayMessages(strMessage, 2);
    //        }
    //    });
    //},

    NotesDeleted: function (NotesId) {

        var data = "NotesID=" + NotesId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "CLINICAL_NOTES", "DELETE_CLINICAL_NOTES");
    },
    EditProgressNoteRow: function (NotesId, PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }

        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            Clinical_NotesSearch.EditProgressNote(NotesId, PatientId);
        }

    },
    EditProgressNote: function (NotesId, PatientId) {

        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {

                Clinical_NotesSearch.UnLoad();

                params["QuickAddPatient"] = true;
                if (params["patientID"] != PatientId) {
                    $.when(setPatientBanner(PatientId, "1")).then(function () {
                        if (NotesId != null && NotesId != '' && NotesId > 0) {
                            params["PatientId"] = PatientId;
                            params['NotesId'] = NotesId;
                            params["mode"] = "Edit";
                            params["ForProgressNote"] = true;
                        } else {
                            return;
                        }
                        var IsProgressNoteSelected = false;
                        if (GetSelectedTab("mstrTabClinical").ContainerControlID == 'Clinical_ProgressNote') {
                            IsProgressNoteSelected = true;
                        }

                        $("ul li[id*=mstrMenu]").hide();

                        if ($("html").hasClass("sidebar-left-collapsed")) {
                            $("html").removeClass("sidebar-left-collapsed");
                        }

                        $("#anchorMainMenu").show();
                        $("div[id*=mstrDiv]").hide();

                        $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {

                            $('#mstrTabClinical').siblings().removeClass('active');
                            $('#mstrTabClinical').addClass('active');
                            $('#ClinicalUL li').removeClass('nav-expanded nav-active');
                            $('#ClinicalUL li#clinicalMenuNotes').addClass('nav-expanded nav-active');
                            $('#ctrlPanDashBoard').hide();

                            EMRUtility.unSelectOtherTabs('mstrTabClinical', 'false');


                            javascript: ClinicalMenuClick(window.event, function () {
                                $.when(ClinicalMenuSettings.TopButtons('clinicalMenuNotes')).then(function () {
                                    ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
                                    if (($("#mstrDivNotes").length == 0 || IsProgressNoteSelected) && (Clinical_ProgressNote.params.NotesId != params['NotesId'])) {
                                        SelectTab("clinicalTabNotes", "false");
                                    }
                                    setTimeout(function () { Patient_Demographic.FillPatientAlertsCount('1'); }, 10);
                                });
                            }, 0, this, 'clinicalMenuNotes', 'li');
                        });
                    });
                } else {
                    if (NotesId != null && NotesId != '' && NotesId > 0) {

                        params['NotesId'] = NotesId;
                        params["mode"] = "Edit";
                        params["ForProgressNote"] = true;
                    } else {
                        return;
                    }


                    $("ul li[id*=mstrMenu]").hide();

                    if ($("html").hasClass("sidebar-left-collapsed")) {
                        $("html").removeClass("sidebar-left-collapsed");
                    }

                    $("#anchorMainMenu").show();
                    $("div[id*=mstrDiv]").hide();

                    $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {

                        $('#mstrTabClinical').siblings().removeClass('active');
                        $('#mstrTabClinical').addClass('active');
                        $('#ClinicalUL li').removeClass('nav-expanded nav-active');
                        $('#ClinicalUL li#clinicalMenuNotes').addClass('nav-expanded nav-active');
                        $('#ctrlPanDashBoard').hide();

                        EMRUtility.unSelectOtherTabs('mstrTabClinical', 'false');


                        javascript: ClinicalMenuClick(event, function () {
                            $.when(ClinicalMenuSettings.TopButtons('clinicalMenuNotes')).then(function () {
                                ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
                                if ($("#mstrDivNotes").length == 0 && (Clinical_ProgressNote.params.NotesId != params['NotesId'])) {
                                    SelectTab("clinicalTabNotes", "false");
                                }
                            });
                        }, 0, this, 'clinicalMenuNotes', 'li');
                    });
                }
                //var objDeffered = $.Deferred();
                //params["QuickAddPatient"] = true;

                //$.when(setPatientBanner(PatientId, "1")).then(function () {

                //    if ($('#clinicalMenuNotes a').length > 0 && $('#clinicalMenuNotes').parent().css("display") != 'none') {
                //        params["mode"] = "Edit";
                //        params["ForProgressNote"] = true;
                //        params["NotesId"] = NotesId;
                //        $('#clinicalMenuNotes a').trigger('click');
                //        Clinical_Notes.AddProgressNoteTab();
                //    }
                //    else {

                //        $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {

                //            params["mode"] = "Edit";
                //            params["ForProgressNote"] = true;
                //            params["NotesId"] = NotesId;

                //            /// Farooq - Select Tab other way
                //            $.when(SelectTab("clinicalTabNotes", "false")).then(function () {

                //                for (var i = 0; i < TabsArray.length; i++) {

                //                    if (TabsArray[i].TabID == "mstrTabDashBoard") TabsArray[i].Selected = false;
                //                    if (TabsArray[i].TabID == "mstrTabClinical") TabsArray[i].Selected = true;
                //                }

                //                //$('#pnlDashboard').hide();
                //                //$('#mstrTabClinical').addClass('active');
                //                $('#mstrTabClinical').siblings().removeClass('active');
                //                SelectTab('mstrTabClinical', 'false');
                //                Clinical_Notes.AddProgressNoteTab();
                //            });
                //        });
                //    }
                //});
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    PatientDemographics: function (patientid) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Clinical_NotesSearch";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    NotesPreview: function (NotesId, ParentCtrl, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter, BillingInfoStatus) {
        var params = [];
        params["FromAdmin"] = "0";
        params["NotesId"] = NotesId;
        params["PatientId"] = PatientId;
        params["RefSearch"] = "DraftSearch";
        params["ProviderId"] = ProviderId;
        params["VisitDate"] = VisitDate;

        params["BillingInfoId"] = BillingInfoId;
        params["AppointmentDate"] = AppointmentDate;
        params["FacilityId"] = FacilityId;
        params["RefProviderID"] = RefProviderID;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["PatientTypeId"] = PatientTypeId;
        params["POS"] = POS;
        params["IsPhoneEncounter"] = IsPhoneEncounter;
        params["BillingInfoStatus"] = BillingInfoStatus;

        params["ParentCtrl"] = 'Clinical_NotesSearch';
        LoadActionPan('Clinical_NotesView', params);
    },

    //----------------------- Start Sign Note Functions


    VBPWithReasoningLoad: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var objDeffered = $.Deferred();

        Clinical_ProgressNote.loadVBPWithReasoning(VisitDate, VisitDate, PatientId, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_NotesSearch.params["VBPResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == PatientId && a.NoteId == NotesId;
                    });
                    if (arrNonCompliantPatients.length > 0) {
                        Clinical_NotesSearch.arrVBPReasoning[PatientId] = JSON.stringify(arrNonCompliantPatients);
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
                            $.when(Clinical_NotesSearch.openMissingAlert_VBP(null, null, null, 'Clinical_NotesSearch', NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter, PHQ2Missing, PHQ9Missing)).then(function () {
                                Clinical_NotesSearch.params.isVBPExists = 1;
                                objDeffered.resolve();
                            });
                        }, function () {
                            $.when(Clinical_NotesSearch.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                                objDeffered.resolve();
                            });
                        },
                               '<b>2017 Value Based Program Missing Data Alert</b>', "Yes, I do", "No, not this time"
                          );
                    }
                    else {
                        $.when(Clinical_NotesSearch.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                            objDeffered.resolve();
                        });
                    }

                }
                else {
                    $.when(Clinical_NotesSearch.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                $.when(Clinical_NotesSearch.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                    objDeffered.resolve();
                });
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return objDeffered;
    },
    openMissingAlert_VBP: function (BillingInformation, Obj, customSigMsg, prntctrl, NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter, PHQ2Missing, PHQ9Missing) {
        var params = [];
        params["FromAdmin"] = "0";
        params["BillingInformation"] = BillingInformation;
        params["Obj"] = Obj;
        params["customSigMsg"] = customSigMsg;
        params["ParentCtrl"] = prntctrl;
        params["NoteId"] = NotesId;
        params["PatientIds"] = PatientId;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["isComponentSelect"] = isComponentSelect;
        params["VisitDate"] = VisitDate;
        params["BillingInfoId"] = BillingInfoId;
        params["AppointmentDate"] = AppointmentDate;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["PatientTypeId"] = PatientTypeId;
        params["FacilityId"] = FacilityId;
        params["POS"] = POS;
        params["RefProviderID"] = RefProviderID;
        params["IsPhoneEncounter"] = IsPhoneEncounter;
        params["PHQ2Missing"] = PHQ2Missing;
        params["PHQ9Missing"] = PHQ9Missing;

        LoadActionPan('VBP_MissingDataAlert', params);
    },

    NotesStatusUpdate: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        Clinical_ProgressNote.params.VisitDateTime = AppointmentDate;
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!IsPhoneEncounter && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
        }
        else {
            Clinical_NotesSearch.VBPWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter);
        }
    },

    CQMWithReasoningLoad: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var objDeffered = $.Deferred();
        Clinical_ProgressNote.loadCQMWithReasoning(VisitDate, VisitDate, PatientId, ProviderId, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_NotesSearch.params["CQMResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == PatientId;
                    });

                    Clinical_NotesSearch.arrCQMReasoning[PatientId] = JSON.stringify(arrNonCompliantPatients);
                    var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                    + " In order to qualify for the <b>2016 CQM incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                    + " against the CQM measures that you have planned to report this year. Do you want to enter the data here before signing off this note?";

                    utility.myConfirm(CQMFoundMsg, function () {
                        objDeffered.resolve();
                        Clinical_Notes.openPatientList(PatientId, isComponentSelect, ProviderId, VisitDate, NotesId, null, "Clinical_NotesSearch", BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId);
                    }, function () {
                        $.when(Clinical_NotesSearch.NotesStatusUpdateAfterCQM(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                            objDeffered.resolve();
                        });
                    },
                          '<b>2016 CQM Missing Data Alert</b>', "Yes, I do", "No, not this time"
                      );

                }
                else {
                    $.when(Clinical_NotesSearch.NotesStatusUpdateAfterCQM(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                objDeffered.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    NotesStatusUpdateAfterCQM: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!IsPhoneEncounter && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
        }
        else {
            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('Do you want to Sign this record?', function () {
                        $.when(Clinical_ProgressNote.signNote(NotesId, PatientId, false, IsPhoneEncounter,null,false)).done(function () {
                            var triggerLocation = 'Notes';
                            if (!IsPhoneEncounter) {
                                ClinicalCDSDetail.showCDSAlert(triggerLocation, 0);
                            }
                            $(" #mainForm #hfTriggerLocation").val('Notes');
                            Clinical_NotesSearch.SetNotesCount();
                            Clinical_NotesSearch.NotesSearch();
                        });
                    }, function () { },
                            'Confirm Sign'
                        );
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
    },

    LoadAttachecdICDsAndCPTs: function (NotesID, PatientID) {
        var objData = new Object();
        objData["NotesId"] = NotesID;
        objData["PatientId"] = PatientID;
        objData["commandType"] = "loadattachedproceduresandproblems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },
    CreateCharges: function (Obj, NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        BillingInformation.params = Clinical_NotesSearch.initializeBillingInfoParams(NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID);
        Clinical_NotesSearch.CreateObjectForBilling(POS);
    },


    initializeBillingInfoParams: function (NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        var params = [];
        params["ParentCtrl"] = "Clinical_NotesSearch";
        params["FromAdmin"] = 0;
        params["NotesId"] = NotesId;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["BillingInfoId"] = BillingInfoId;
        params["VisitDate"] = VisitDate;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["PatientTypeId"] = PatientTypeId;
        params["AppointmentDate"] = AppointmentDate;
        params["FacilityId"] = FacilityId;
        BillingInformation.PatientInfoJSON = {};
        BillingInformation.PatientInfoJSON.RefProviderID = RefProviderID;
        BillingInformation.PatientInfoJSON.FacilityID = FacilityId;
        return params;
    },


    CreateObjectForBilling: function (POSCode) {
        var facPOS = POSCode;
        BillingInformation.BillingInformationLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.BillingInfoCPTFill_JSON = JSON.parse(response.BillingInfoCPTFill_JSON);
                response.BillingInfoICDFill_JSON = JSON.parse(response.BillingInfoICDFill_JSON);

                response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);

                var Obj = {
                };
                var ICDs = []

                for (var index in response.BillingInfoICDFill_JSON) {
                    var item = response.BillingInfoICDFill_JSON[index];
                    var currentICD = {};
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
                    var currentCPT = {};
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
                BillingInformation.LoadAttachecdICDsAndCPTs().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        response.ProblemListFill_JSON = JSON.parse(response.ProblemListFill_JSON);
                        response.ProcedureListFill_JSON = JSON.parse(response.ProcedureListFill_JSON);

                        response.ProblemListFill_JSON = $.grep(response.ProblemListFill_JSON, function (a) { return a.IsNoteLinked == "True"; });
                        response.ProcedureListFill_JSON = $.grep(response.ProcedureListFill_JSON, function (a) {
                            return a.IsNoteLinked == "1" || (parseInt(a.VaccineHxId) > 0 || parseInt(a.ImmTherInjectionId) > 0);
                        });


                        var counter = 0;
                        var objData = {};
                        if (BillingInformation.params.BillingInfoId > 0) {
                            objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
                        }
                        else {
                            objData["BillingInfoId"] = '-1'
                        }
                        objData["POS"] = facPOS;
                        objData["commandType"] = "BILLING_INFORMATION_SAVE";
                        objData["NotesId"] = BillingInformation.params.NotesId;
                        objData["PatientId"] = BillingInformation.params.PatientId;
                        objData["ProviderId"] = BillingInformation.params["ProviderId"];
                        objData["VisitId"] = BillingInformation.params.VisitId;
                        objData["Status"] = 'Draft';
                        objData["VisitDate"] = BillingInformation.params.VisitDate;
                        objData.ICDs = [];
                        objData.CPTs = [];

                        for (var i in response.ProblemListFill_JSON) {
                            item = response.ProblemListFill_JSON[i];
                            var ICD = {};
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
                            currentCPT.Modifier1 = item.Modifier;
                            currentCPT.Modifier2 = "";
                            currentCPT.Modifier3 = "";
                            currentCPT.Modifier4 = "";

                            var icd_cods = item.ICDCodes.split(',');
                            var pinter1 = icd_cods.length > 0 && icd_cods[0] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[0], objData.ICDs) : '1';
                            var pinter2 = icd_cods.length > 1 && icd_cods[1] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[1], objData.ICDs) : '';
                            var pinter3 = icd_cods.length > 2 && icd_cods[2] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[2], objData.ICDs) : '';
                            var pinter4 = icd_cods.length > 3 && icd_cods[3] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[3], objData.ICDs) : '';
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
                            objData.CPTs.push(currentCPT);
                        }
                        BillingInformation.BillingObj = objData;
                        BillingInformation.AttachtedCPTData = objData.CPTs;
                        BillingInformation.AttachtedCPTData.reverse();
                        if (BillingInformation.params.BillingInfoId > 0) {
                            BillingInformation.CreateCharge(BillingInformation.BillingObj, true);
                        }
                        else {
                            BillingInformation.BillingInfoSave(objData).done(function (InnerResponse) {
                                InnerResponse = JSON.parse(InnerResponse);
                                if (InnerResponse.status != false) {
                                    BillingInformation.BillingObj.BillingInfoId = BillingInformation.params.BillingInfoId = InnerResponse.BillingInfoId;
                                    BillingInformation.CreateCharge(BillingInformation.BillingObj, true);
                                }
                            });
                        }
                    }
                });
                findInDiv.hide(true);
            }
        });
    },

    //----------------------- End Sign Note Functions
}
