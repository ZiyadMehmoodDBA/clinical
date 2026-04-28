Patient_Referrals = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    Type: "Outgoing",
    Load: function (params) {
        Patient_Referrals.params = params;
        Patient_Referrals.params.mode = "Add";
        Patient_Referrals.Type = "Outgoing";
        if (Patient_Referrals.params.PanelID != 'pnlPatientReferrals') {
            Patient_Referrals.params.PanelID = Patient_Referrals.params.PanelID + ' #pnlPatientReferrals';
        } else {
            Patient_Referrals.params.PanelID = 'pnlPatientReferrals';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Patient_Referrals.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet") {
            $("#" + Patient_Referrals.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        // $(Patient_Referrals.params.PanelID + " #headingTitle").html("Search Incoming Referrals");
        Patient_Referrals.domReadyFunc();
        // Commented against Bug No. IMP-478
        //if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
        //    $("#" + Patient_Referrals.params.PanelID + ' #ulReferralTabs a[href="#Incoming"]').trigger('click');
        //}
        //else {
        $("#" + Patient_Referrals.params.PanelID + ' #ulReferralTabs a[href="#Outgoing"]').trigger('click');
        //}
        Patient_Referrals.LoadAllAutocomplete();
        utility.CreateDatePicker(Patient_Referrals.params.PanelID + ' #IncomingReferralSearch' + ' #dtDateFrom', function () {
            //on-change callback method
        }, false);
        utility.CreateDatePicker(Patient_Referrals.params.PanelID + ' #IncomingReferralSearch' + ' #dtDateTo', function () {
            //on-change callback method
        }, false);
        utility.CreateDatePicker(Patient_Referrals.params.PanelID + ' #OutgoingReferralSearch' + ' #dtDateFrom', function () {
            //on-change callback method
        }, false);
        utility.CreateDatePicker(Patient_Referrals.params.PanelID + ' #OutgoingReferralSearch' + ' #dtDateTo', function () {
            //on-change callback method
        }, false);

        utility.ValidateFromToDate('IncomingReferralSearch', "dtDateFrom", "dtDateTo", true);
        utility.ValidateFromToDate('OutgoingReferralSearch', "dtDateFrom", "dtDateTo", true);

        //ReferralSearch is already calling in Tab Select and on above there is click trigger of Outgoing Tab. so this call in unnecessary.
        //Patient_Referrals.ReferralSearch();

        //Patient_Referrals.ReferralSearch(0, 1,15,"Incoming");
        Patient_Referrals.ValidateIncomingTab();
        var self = $('#' + Patient_Referrals.params.PanelID);
        if (Patient_Referrals.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                //        Patient_Referrals.ValidateProblemLists();


                //        Patient_Referrals.bIsFirstLoad = false;
                //        //Serialization
                //        //$('#' + Patient_Referrals.params.PanelID + ' #frmClinicalProblemLists').data('serialize', $('#' + Patient_Referrals.params.PanelID + ' #frmClinicalProblemLists').serialize());

            });
            Patient_Referrals.fillRefferal();
        }

        if (Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Patient_Referrals.params.PanelID + " div#FaceSheetPager", Patient_Referrals.params.FaceSheetComponents, 'Referrals');
        } else if (Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlPatientReferrals' + " div#FaceSheetPager", Patient_Referrals.params.FaceSheetComponents, 'Referrals');
        }

        Patient_Referrals.SetCollapseExpandPanelOutgoing();

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1356px");
            }
        });
    },

    SetCollapseExpandPanelOutgoing: function () {

        //1- Initialization
        $('#' + Patient_Referrals.params.PanelID + ' #Outgoing .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Patient_Referrals.params.PanelID + ' #Outgoing .splitterBtn a'));

        $('#' + Patient_Referrals.params.PanelID + ' #Outgoing .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Patient_Referrals.params.PanelID + ' #Outgoing  #splitterBody').attr('class', 'splitterBody active');
            $('#' + Patient_Referrals.params.PanelID + ' #Outgoing  #splitterBody').show();
        }
        else {
            $('#' + Patient_Referrals.params.PanelID + ' #Outgoing  #splitterBody').removeClass('splitterBody active');
            $('#' + Patient_Referrals.params.PanelID + ' #Outgoing  #splitterBody').hide();
        }

    },
    ValidateIncomingTab: function () {
        $('#IncomingReferralSearch')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   RefProvider: {
                       group: '.col-sm-6',
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
    ValidateOutcomingTab: function () {
        $('#OutgoingReferralSearch')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   Provider: {
                       group: '.col-sm-6',
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
    domReadyFunc: function () {

        $(document).ready(function () {

            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });
        });

    },
    // Search/Grid Load Functions
    //Adding Pagination ReferralSearchon 04 Dec 2015 by Azhar
    ReferralSearch: function (ReferralId, PageNo, rpp) {
        var deffered = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var PnlResult = "";
                var dgvDivId = "";
                var pagingDivId = "";
                if (Patient_Referrals.Type == "Incoming") {
                    PnlResult = "pnlInComingReferal_Result";
                    dgvDivId = "dgvInComingReferral";
                    pagingDivId = "dgvInComingReferral_Paging";
                }
                else {
                    PnlResult = "pnlOutgoingReferal_Result";
                    dgvDivId = "dgvOutgoingReferral";
                    pagingDivId = "dgvOutgoingReferral_Paging";
                }

                var strMessage = "";

                if ($("#" + Patient_Referrals.params.PanelID + " #" + PnlResult).css("display") == "none") {
                    $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult).show();
                }
                Patient_Referrals.SearchReferral(Patient_Referrals.Type, ReferralId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //Adding selection column of checkbox of Problem lists for Progress Notes on 04 Dec 2015 by Azhar
                        if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                            if ($("#" + Patient_Referrals.params.PanelID + " #" + dgvDivId + " thead tr #SelectRecord").length == 0) {
                                $("#" + Patient_Referrals.params.PanelID + " #" + dgvDivId + " thead tr").prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderReferral' + Patient_Referrals.Type + '" onchange="Patient_Referrals.checkUncheckAllReferral(this,\'' + dgvDivId + '\',\'' + Patient_Referrals.Type + '\');"   class="input-block" coltype="checkbox"/> </th>');
                            }

                        } else {
                            $("#" + Patient_Referrals.params.PanelID + " #" + dgvDivId + " th#SelectRecord").remove();
                        }
                        //Patient_Referrals.ProblemListGridLoad(response);
                        Patient_Referrals.ReferralGridLoadNew(Patient_Referrals.Type, response);
                        //Adding Pagination on 04 Dec 2015 by Azhar
                        var TableControl = Patient_Referrals.params.PanelID + " #" + pagingDivId;
                        var PagingPanelControlID = Patient_Referrals.params.PanelID + " #" + pagingDivId;
                        var ClassControlName = "Patient_Referrals";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ReferralListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Patient_Referrals.ReferralSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                        setTimeout(function () {
                            if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Patient_Referrals.params.PanelID + "  #" + pagingDivId + " #btnAddReferralToNotes").length == 0) {
                                $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Patient_Referrals.addReferralsToNotes(\'' + Patient_Referrals.Type + '\');" id="btnAddReferralToNotes">Add on Note</button>').insertAfter("#" + Patient_Referrals.params.PanelID + "  #" + pagingDivId + " .pagination")
                            }
                        }, 11);


                        deffered.resolve();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        return deffered.promise();
        //End//21/12/2015//Ahmad Raza//Implimented Privileges for ProblemList Search
    },

    ReferralGridLoadNew: function (Type, response) {
        //Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        var PnlResult = "";
        var dgvDivId = "";
        var pagingDivId = "";
        if (Type == "Incoming") {
            PnlResult = "pnlInComingReferal_Result";
            dgvDivId = "dgvInComingReferral";
            pagingDivId = "dgvInComingReferral_Paging";
        }
        else {

            PnlResult = "pnlOutgoingReferal_Result";
            dgvDivId = "dgvOutgoingReferral";
            pagingDivId = "dgvOutgoingReferral_Paging";
        }
        var isDataactive = $('#' + Patient_Referrals.params.PanelID + ' #' + PnlResult + ' #divSwitch #switchActive').attr('isactive');
        // get Actions
        var actions = "";
        $("#" + Patient_Referrals.params.PanelID + " #" + dgvDivId + " tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    //actions = EMREditableGrid.GetActions(arrActionType);
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + Patient_Referrals.params.PanelID + " #" + PnlResult);
                }
            }
        });

        //$("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnClearTable();
        //$("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId + " tbody").find("tr").remove();


        if ($.fn.dataTable.isDataTable("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId)) {
            $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnClearTable();
            $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
            $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId + " tbody").find("tr").remove();
        }


        //    if ($.fn.dataTable.isDataTable("#" + Patient_Referrals.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
        //        $("#" + Patient_Referrals.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
        //    }

        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search




        if (response.ReferralListCount > 0) {
            //$('#' + Patient_Referrals.params.PanelID + ' div#divShowHistory').removeClass("hidden");
            var ReferralLoadJSONData = JSON.parse(response.ReferralListLoad_JSON);

            if ($.fn.dataTable.isDataTable("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId)) {
                $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
            }
            //tem array to hold rows and childs
            var arraTemp = [];



            $.each(ReferralLoadJSONData, function (i, item) {


                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.ReferralId);
                //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                $row.attr("ProblemListNotesId", item.NoteId);
                //End//31/12/2015//Ahmad Raza//Bug#178 fixed




                //$row.append('<td></td><td></td><td class="actions" id="' + item.ReferralId + '" ></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                //Start//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ReferralId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ReferralId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }

                if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                    // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Patient_Referrals.enableAddReferral(this,\'' + Type + '\',event);" id="' + item.ReferralId + '" name="SelectCheckBox' + Type + '" ' + Checked + ' class="input-block text-center"/></td>';
                    // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                } else {
                    SelectionCheckBoxColumn = "";
                }
                var proceduresHtml = "";
                var procedures = item.Procedures.split(',');
                for (var p = 0; p < procedures.length; p++) {
                    proceduresHtml = proceduresHtml + procedures[p] + "<br>";
                }

                var tglclass = "";
                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                    disableclass = "disableAll";
                }

                var btnedit = '&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Patient_Referrals.ReferralEdit(' + item.ReferralId + ',false,event);utility.SelectGridRow($(this));"   title="Edit Record"><i class="fa fa-edit black"></i></a>';
                var btnview = '&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Patient_Referrals.ReferralEdit(' + item.ReferralId + ',true,event);utility.SelectGridRow($(this));"   title="View Record"><i class="fa fa-eye blue"></i></a>';


                var source = "MD Vision";
                if (item.MedTextAppointmentId)
                    source = "MedText";

                var status = "Sent Successfully";
                if (item.IsDraft.toLowerCase() == "true") {
                    status = "Saved as Draft";
                    btnview = '';
                    if (source == "MedText" && response.MedTextURL) {
                        var url = response.MedTextURL.replace("{MedTextReferralId}", item.MedTextAppointmentId);
                        btnedit = '&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Patient_Referrals.OpenMedText(\'' + url + '\',event);utility.SelectGridRow($(this));"   title="Edit Record"><i class="fa fa-edit black"></i></a>';
                    }
                }
                else {
                    btnedit = '';
                }


                //NewActions = '&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Patient_Referrals.ReferralEdit(' + item.ReferralId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>'
                NewActions = '&nbsp;<a class="btn  btn-xs"  href="#" onclick="Patient_Referrals.DeleteReferral(\'' + item.ReferralId + '\',this,event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Referrals.rowInactive(\'' + item.ReferralId + '\', ' + isactive + ',event);" title="Inactive Record"><i class="' + tglclass + '"></i></a>' + '&nbsp;<a title="View Referral" class="btn  btn-xs" href="#" onclick="Patient_Referrals.printReferral(' + item.ReferralId + ',event);"> <i class="fa fa-credit-card blue"></i></a>' + btnedit + btnview;
                if (item.Type == 'Outgoing') {
                    if (item.RefProviderName != '') {
                        $row.append(SelectionCheckBoxColumn + '<td>' + NewActions + '</td><td>' + source + '</td><td>' + utility.RemoveTimeFromDate(null, item.Date) + ' ' + item.Time + '</td><td>' + item.Procedures + '</td><td>' + item.RefProviderName + ' (' + item.RefProviderEntityName + ')</td><td>' + item.ToSpecialtyName + '</td><td>' + item.ProviderName + '</td><td>' + $($("#" + Patient_Referrals.params.PanelID + " #ddlVisitType > option")[item.Visits]).text() + '</td><td>' + item.AssigneeName + ' </td><td>' + status + '</td>');
                    }
                    else {
                        $row.append(SelectionCheckBoxColumn + '<td>' + NewActions + '</td><td>' + source + '</td><td>' + utility.RemoveTimeFromDate(null, item.Date) + ' ' + item.Time + '</td><td>' + item.Procedures + '</td><td>' + item.RefProviderName + '</td><td>' + item.ToSpecialtyName + '</td><td>' + item.ProviderName + '</td><td>' + $($("#" + Patient_Referrals.params.PanelID + " #ddlVisitType > option")[item.Visits]).text() + '</td><td>' + item.AssigneeName + ' </td><td>' + status + '</td>');
                    }
                }
                else {
                    $row.append(SelectionCheckBoxColumn + '<td>' + NewActions + '</td><td>' + source + '</td><td>' + utility.RemoveTimeFromDate(null, item.Date) + ' ' + item.Time + '</td><td>' + $($("#" + Patient_Referrals.params.PanelID + " #ddlVisitType > option")[item.Visits]).text() + '</td><td>' + item.PatientInsuranceName + '</td><td>' + item.RefProviderName + '</td><td>' + item.ProviderName + '</td><td>' + $($("#" + Patient_Referrals.params.PanelID + " #ddlStatus > option")[item.Status]).text() + '</td><td> ' + item.AssigneeName + ' </td>');
                }


                //End//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row

                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }


                $("#" + Patient_Referrals.params.PanelID + " #" + dgvDivId + " tbody").last().append($row);

                var CurrentRowchilds = $();
                if (CurrentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: CurrentRowchilds });
            });

            //Inalize grid
            var PanelGrid = "#" + Patient_Referrals.params.PanelID + " #" + PnlResult;
            var GridId = "#" + Patient_Referrals.params.PanelID + " #" + dgvDivId;

            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search
            if (Patient_Referrals.myGrid != null) {

                if ($.fn.dataTable.isDataTable(Patient_Referrals.myGrid)) {
                    Patient_Referrals.myGrid.$table.dataTable().fnDestroy();
                } else {
                    Patient_Referrals.myGrid = null;
                }

                if ($.fn.dataTable.isDataTable("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId)) {
                    $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
                }
            }

            Patient_Referrals.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Patient_Referrals, 0, false, true, false, true, false, null);

            $.each(arraTemp, function (i, item) {

                if (Patient_Referrals.myGrid != null) {
                    var row = Patient_Referrals.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            //Start//04//01//2015//Ahmad Raza//Sorting removed from first column of grid
            // $('#dgvProblemLists').dataTable().fnSettings().aoColumns[0].bSortable = false;
            //$('#' + Patient_Referrals.params.PanelID + ' #' + dgvDivId).dataTable().fnSettings().aoColumns[0].bSortable = false;
            //End//04//01//2015//Ahmad Raza//Sorting removed from first column of grid


            /* Start of Code for making No Known Problem List hyperlink inline with checkbox and search box.
             *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
             */
            var checked = '';
            if (isDataactive == "0") {
            } else if (isactive == null) {
                isDataactive = "1";
                checked = 'checked="checked"';
            } else {
                isDataactive = "1";
                checked = 'checked="checked"';
            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="' + isDataactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Patient_Referrals.ActiveReferralSearch(this,\'' + Type + '\');">' +
                          '</div><span class="pl-xs">Active</span>';

            $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            //  if ($('#pnlClinicalProblemLists #switchActive').is(':checked') || $("#pnlProblemLists_Result #btnNoKnownProblems").is(':visible')) {
            if ($('#' + Patient_Referrals.params.PanelID + ' div#divShowHistory').hasClass("hidden") == false) {
                $('#' + Patient_Referrals.params.PanelID + ' div#divShowHistory').addClass("hidden");
            }
            var NotFoundMessage = "";
            if (Type == "Incoming") {
                NotFoundMessage = "No Incoming Referral Found.";
            }
            else {
                NotFoundMessage = "No Outgoing Referral Found.";
            }

            $('#' + Patient_Referrals.params.PanelID + ' #' + PnlResult + ' #' + dgvDivId).DataTable({
                "language": {
                    "emptyTable": NotFoundMessage
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
            var checked = '';
            if (isDataactive == "0") {
            } else if (isDataactive == null) {
                isDataactive = "1";
                checked = 'checked="checked"';
            } else {
                isDataactive = "1";
                checked = 'checked="checked"';
            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isDataactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Patient_Referrals.ActiveReferralSearch(this,\'' + Type + '\');">' +
                         '</div><span class="pl-xs">Active</span>';


            $("#" + Patient_Referrals.params.PanelID + " #" + PnlResult + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }


        EMRUtility.SwicthWidgetInializatoin();
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Start//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        // $('#dgvProblemLists thead tr th:first-child').removeClass('sorting_asc');
        $('#' + Patient_Referrals.params.PanelID + ' #' + dgvDivId + ' thead tr th:first-child').removeClass('sorting_asc');
        //End//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        //Editable Grid
        //var PanelGrid = "#pnlClinicalProblemLists #pnlProblemLists_Result";
        //var GridId = "#pnlClinicalProblemLists #dgvProblemLists";
        //EMRUtility.MakeEditableGrid(PanelGrid, GridId, Patient_Referrals, 0);

    },

    OpenMedText: function (Url) {

        var params =[];
        params["FromAdmin"]= "0";
        params["MedTextUrl"]= Url;
        params["ParentCtrl"] = "Patient_Referrals";
        LoadActionPan('Patient_MedText_Referrals', params);
    },

    //Start by M Ahmad Imran to check uncheck all Referral by a checkBox in header. Date: 22 Jan 2016.
    checkUncheckAllReferral: function (chkBox, DivId, Type) {
        if ($(chkBox).is(':checked')) {
            $("#" + Patient_Referrals.params.PanelID + " #" + DivId + " [name='SelectCheckBox" + Type + "']").prop("checked", true);
        } else {
            $("#" + Patient_Referrals.params.PanelID + " #" + DivId + " [name='SelectCheckBox" + Type + "']").prop("checked", false);
        }
        $("#" + Patient_Referrals.params.PanelID + " #" + DivId + " tbody").find('input[type="checkbox"]').each(function () {
            Patient_Referrals.enableAddReferral(this, Type);
        });
    },
    //This Function enable/disable add to note button
    enableAddReferral: function (obj, Type, event) {

        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }
        var PnlResult = "";
        var pagingDivId = "";
        if (Type == "Incoming") {
            PnlResult = "pnlInComingReferal_Result";
            pagingDivId = "dgvInComingReferral_Paging";
        }
        else {
            PnlResult = "pnlOutgoingReferal_Result";
            pagingDivId = "dgvOutgoingReferral_Paging";
        }

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id);
            } if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id);
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            $("#" + Clinical_ProgressNote.params.PanelID + ' #' + PnlResult + ' #chkHeaderReferral' + Type).prop('checked', false);
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }
        }

        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
            $("#" + Patient_Referrals.params.PanelID + "  #" + pagingDivId + " #btnAddReferralToNotes").prop('disabled', false);
        } else {
            $("#" + Patient_Referrals.params.PanelID + "  #" + pagingDivId + " #btnAddReferralToNotes").prop('disabled', true);
        }
    },
    SearchReferral: function (Type, ReferralId, PageNumber, RowsPerPage) {

        var PnlResult = "";
        var dgvDivId = "";
        var pagingDivId = "";
        var SearchFormDivId = "";
        if (Type == "Incoming") {
            PnlResult = "pnlInComingReferal_Result";
            dgvDivId = "dgvInComingReferral";
            pagingDivId = "dgvInComingReferral_Paging";
            SearchFormDivId = "IncomingReferralSearch";
        }
        else {
            PnlResult = "pnlOutgoingReferal_Result";
            dgvDivId = "dgvOutgoingReferral";
            pagingDivId = "dgvOutgoingReferral_Paging";
            SearchFormDivId = "OutgoingReferralSearch";
        }

        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Patient_Referrals.params.PanelID + ' #' + PnlResult + ' #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var self = $('#' + Patient_Referrals.params.PanelID + ' #' + SearchFormDivId);
        var objData = self != null ? self.getMyJSONByName() : "{}";
        objData = JSON.parse(objData);
        objData["PatientId"] = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = IsCheckedIn;
        objData["ReferralId"] = ReferralId;

        if (Type == "Outgoing") {
            objData["ProviderId"] = $('#' + Patient_Referrals.params.PanelID + ' #Outgoing #ddlReferralFrom').val();
            objData["RefProviderId"] = $('#' + Patient_Referrals.params.PanelID + ' #Outgoing #ddlReferralTo').val();
        }
        else {
            objData["ProviderId"] = $('#' + Patient_Referrals.params.PanelID + ' #Incoming #ddlReferralTo').val();
            objData["RefProviderId"] = $('#' + Patient_Referrals.params.PanelID + ' #Incoming #ddlReferralFrom').val();
        }

        //objData["Status"] = $('#' + Patient_Referrals.params.PanelID + ' #ddlStatus').val();
        //objData["Visits"] = $('#' + Patient_Referrals.params.PanelID + ' #ddlVisitType').val();

        if (Type == 'Outgoing') {
            objData["CPTCodeDescription"] = objData["CPTCode"];
        }
        if (Type == 'Incoming') {
            objData["PatientInsurance"] = $('#' + Patient_Referrals.params.PanelID + ' #ddlInsurancePlan').val() == "" ? '0' : $('#' + Patient_Referrals.params.PanelID + ' #ddlInsurancePlan').val();
        }

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["LoadFor"] = "Grid";
        objData["Type"] = Type;
        objData["commandType"] = "SEARCH_REFERRAL";
        if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["NoteId"] = Patient_Referrals.params.NotesId == null ? 0 : Patient_Referrals.params.NotesId;
        }
        else {
            objData["NoteId"] = 0;
        }
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },
    ActiveReferralSearch: function (objThis, Type) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        //Patient_Referrals.ReferralSearch(Type);
        Patient_Referrals.Type = Type;
        Patient_Referrals.ReferralSearch();

    },


    rowInactive: function ($row, obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var IsActive = obj;
        var strMessage = "";
        var id = $row;//.attr("id");
        AppPrivileges.GetFormPrivileges("Referrals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Referrals.InActiveReferral(selectedValue, null, null).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Referrals.ReferralSearch();
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                        });
                    }

                }, function () { },
                            '3', null, null, null, IsActive
                        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    InActiveReferral: function (ReferralId, comments, endDate) {
        var PnlResult = "";
        var pagingDivId = "";
        if (Patient_Referrals.Type == "Incoming") {
            PnlResult = "pnlInComingReferal_Result";
            pagingDivId = "dgvInComingReferral_Paging";
        }
        else {
            PnlResult = "pnlOutgoingReferal_Result";
            pagingDivId = "dgvOutgoingReferral_Paging";
        }

        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Patient_Referrals.params.PanelID + ' #' + PnlResult + ' #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + Patient_Referrals.params.PanelID + ' #' + PnlResult + ' #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        //     var ProblemListId = Patient_Referrals.params.ProblemListId;
        var patientId = Patient_Referrals.params.patientID;

        var objData = new Object();
        objData["ReferralId"] = ReferralId;
        objData["PatientId"] = patientId;
        objData["IsActive"] = IsCheckedIn;
        objData["IsActiveRecord"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_REFERRAL";
        objData["Type"] = Patient_Referrals.Type;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },

    ReferralEdit: function (ReferralId,IsViewOnly, event) {

        if (event != null) {
            event.stopPropagation();
        }

        if (event.target instanceof HTMLAnchorElement || event.target.nodeName.toLowerCase() == 'i' || (event.target instanceof HTMLInputElement
       && event.target.getAttribute('type') == 'checkbox') != true) {
            if (Patient_Referrals.Type == "Incoming") {
                var params = [];
                var PanelID = "";
                if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Patient_Referrals';
                    PanelID = 'pnlClinicalProgressNote #pnlPatientReferrals';
                } else {
                    params["ParentCtrl"] = 'patTabReferrals';
                    PanelID = 'pnlPatientReferrals';
                }
                if (Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {

                    params["ParentCtrl"] = 'Patient_Referrals';
                    PanelID = 'pnlClinicalFaceSheet #pnlPatientReferrals';
                    params["ParentCtrlPanelID"] = Patient_Referrals.params.PanelID;
                }
                //params["ParentCtrl"] = Patient_Referrals.params.ParentCtrl != "clinicalTabProgressNote" ? "patTabReferrals" : "Patient_Referrals";
                params["FromAdmin"] = 0;
                params["ReferralId"] = ReferralId;
                params["mode"] = "Edit";
                params["IsViewOnly"] = IsViewOnly;
                params["PatientId"] = Patient_Referrals.params["patientID"];
                LoadActionPan("Patient_Referrals_Incoming_Detail", params, PanelID);
            }
            else {
                var params = [];
                var PanelID = "";
                if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Patient_Referrals';
                    PanelID = 'pnlClinicalProgressNote #pnlPatientReferrals';
                    params["ParentCtrlPanelID"] = Patient_Referrals.params.PanelID;
                } else {
                    params["ParentCtrl"] = 'patTabReferrals';
                    PanelID = 'pnlPatientReferrals';
                }

                if (Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {

                    params["ParentCtrl"] = 'Patient_Referrals';
                    PanelID = 'pnlClinicalFaceSheet #pnlPatientReferrals';
                    params["ParentCtrlPanelID"] = Patient_Referrals.params.PanelID;
                }
                //params["ParentCtrl"] = Patient_Referrals.params.ParentCtrl != "clinicalTabProgressNote" ? "patTabReferrals" : "Patient_Referrals";
                params["FromAdmin"] = 0;
                params["ReferralId"] = ReferralId;
                params["mode"] = "Edit";
                params["IsViewOnly"] = IsViewOnly;
                params["PatientId"] = Patient_Referrals.params["patientID"];
                LoadActionPan("Patient_Referrals_Outgoing_Detail", params, PanelID);
            }
        }
    },

    rowRemove: function ($row, obj, event) {

        if (event != null) {
            event.stopPropagation();
        }

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    if ($row.hasClass('adding')) {
                    }
                    //var _self = obj;
                    //_self.datatable.row($row.get(0)).remove().draw();
                    if (parseInt($row.attr("id")) > 0) {
                        Patient_Referrals.DeleteReferral($row.attr("id"), $row, obj);
                    }
                    else {
                        var _self = obj;
                        _self.datatable.row($row.get(0)).remove().draw();
                        utility.DisplayMessages("Successfully Deleted", 1);
                    }

                }, function () {
                },
                            '1'
            );
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    DeleteReferral: function (ReferralId, $row, obj) {
        if (obj != null) {
            obj.stopPropagation();
        }

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Patient_Referrals.DeleteReferral_DBCall(ReferralId).done(function (response) {

                        response = JSON.parse(response);
                        if (response.status != false) {
                            Patient_Referrals.ReferralSearch();
                            utility.DisplayMessages(response.Message, 1);
                            var _self = $row;
                            // _self.datatable.row($row.get(0)).remove().draw();
                        } else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                    });

                }, function () { },
                    '3'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    DeleteReferral_DBCall: function (ReferralId) {

        var objData = new Object();
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "DELETE_REFERRAL";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    //Function Name: printReferral
    //Author Name: M Ahmad Imran
    //Created Date: 17-05-2016
    //Description: Creates PDF to view Referral
    printReferral: function (ReferralId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = Patient_Referrals.params.ParentCtrl != "clinicalTabProgressNote" ? "patTabReferrals" : "Patient_Referrals";
        var PanelID = '';
        if (Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {

            params["ParentCtrl"] = 'Patient_Referrals';
            PanelID = 'pnlClinicalFaceSheet #pnlPatientReferrals';
        }
        else if (Patient_Referrals.params.ParentCtrl == "patTabReferrals") {

            params["ParentCtrl"] = 'Patient_Referrals';
            PanelID = 'pnlPatientReferrals';
        }
        else if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {

            params["ParentCtrl"] = 'Patient_Referrals';
            PanelID = 'pnlClinicalProgressNote #pnlPatientReferrals';
        }
        else {
            params["ParentCtrl"] = 'Patient_Referrals';
            PanelID = 'pnlPatientReferrals';
        }

        params["ReferralId"] = ReferralId;
        //LoadActionPan('Patient_ReferralsView', params, PanelID);
        Patient_ReferralsView.ReferralPreview(params.PatientId, params.UserId, params.ReferralId);
    },

    //-----------------Progress Note-------------
    // added on Dec 04,2015 by Muhammad Ahmad Imran
    //Call Back function to add component to Progress Note
    addReferralsToNotes: function (Type) {
        Patient_Referrals.Type = Type;
        var PnlResult = "";
        var dgvDivId = "";
        var pagingDivId = "";
        if (Patient_Referrals.Type == "Incoming") {
            PnlResult = "pnlInComingReferal_Result";
            dgvDivId = "dgvInComingReferral";
            pagingDivId = "dgvInComingReferral_Paging";
        }
        else {
            PnlResult = "pnlOutgoingReferal_Result";
            dgvDivId = "dgvOutgoingReferral";
            pagingDivId = "dgvOutgoingReferral_Paging";
        }

        $("#" + Patient_Referrals.params.PanelID + " #dgvOutgoingReferral tbody").find('input[type="checkbox"]').each(function () {
            Patient_Referrals.enableAddReferral(this, 'OutGoing', null);
        });
        var SelectedReferrals = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
        if (SelectedReferrals != null && SelectedReferrals != '') {
            for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                var PLid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Patient_Referral_Main' + PLid).length != 0) {
                    var index = SelectedReferrals.indexOf(PLid);
                    if (index > -1) {
                        SelectedReferrals.splice(index, 1);
                    }
                }
            }
        }

        var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
        if (detachedvalues.join() != '') {
            Patient_Referrals.detachReferralFromNotesOnAt(detachedvalues).done(function () {
                if (SelectedReferrals.join() != null && SelectedReferrals.join() != '') {
                    Patient_Referrals.attachReferralFromNotesAT(SelectedReferrals);
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Referrals');
                }
            });

        } else if (SelectedReferrals.join() != null && SelectedReferrals.join() != '') {
            Patient_Referrals.attachReferralFromNotesAT(SelectedReferrals);
        }
        //When User has attached Allergies with notes than add on note button should be disabled
        $("#" + Patient_Referrals.params.PanelID + "  #" + dgvDivId + " #btnAddReferralToNotes").prop('disabled', true);

        if (Patient_Referrals.params && Patient_Referrals.params.ParentCtrl && Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Patient_Referrals.params.ParentCtrl, 'Patient_Referrals');
            EMRUtility.scrollToPNcomponent('clinical_referrals');
        }
        //  Patient_Referrals.UnLoadTab();
    },

    attachReferralFromNotesAT: function (SelectedReferrals) {
        Patient_Referrals.getReferralInfo(SelectedReferrals.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Patient_Referrals.params != null && Patient_Referrals.params.PanelID.indexOf('pnlPatientReferrals') != -1) {
                    Patient_Referrals.ReferralSearch();
                }
            }, 5);
        });
    },

    detachReferralFromNotesOnAt: function (detachedvalues) {
        var dfd = new $.Deferred();
        Patient_Referrals.detachReferralFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var PLid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Patient_Referrals_Main' + PLid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    //this function will get Problem Lists Soap Text and attach that to Progress note
    getReferralInfo: function (ReferralId) {
        if (ReferralId == null || ReferralId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Patient_Referrals.SearchReferralForSoapText(ReferralId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.ReferralListCount > 0) {
                        //Start//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                        Patient_Referrals.createReferralBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', ReferralId);
                        //End//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed


                    }
                    if (response.ProblemListSoapCount > 0) {
                        Clinical_ProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    //This Function will check, if Problem Lists Soap is already attached in Progress note, if Problem Lists is not attached than it will create main divs to attach allergy
    checkReferralExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_referrals').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ReferralsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_referrals title="Referrals"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Referrals\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Referrals">Referrals</a> ' +
      '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Referrals\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Referrals\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_referrals> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });


        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_referrals').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },
    createReferralBodyHTML: function (response, NoteHTMLCtrl, ReferralId, hideAlertMessage, fromprogressnote, bNotSaveComponent) {

        Patient_Referrals.checkReferralExists();
        var PListId = [];

        if (response.ReferralListCount > 0) {
            var ReferralSoap_JSON = JSON.parse(response.ReferralData_JSON);
            var $mainDivVital = $(document.createElement('div'));

            if (ReferralSoap_JSON == null || ReferralSoap_JSON.length == 0) {
                return "";
            }

            $.each(ReferralSoap_JSON, function (index, element) {

                var PLid = element.ReferralId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Patient_Referrals_Main" + PLid);

                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Patient_Referrals_" + PLid);

                var $ListVital = $(document.createElement('ul'));
                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Patient_Referrals_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Patient_Referrals_Main" + PLid + '"  ><i class="fa fa-times"></i></a></div> ');

                var $DetailsTable = $(document.createElement('table'));
                $DetailsTable.attr('id', "Patient_ReferralsTable" + PLid);
                $DetailsTable.attr('border', '0');

                if (element.Type == 'Incoming') {
                    $DetailsTable.append("<tr><td><strong> Date:</strong></td><td>&nbsp</td><td class='pr-sm'>" + element.Date + " " + element.Time + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                    $DetailsTable.append("<tr><td><strong> Category:</strong></td><td>&nbsp</td><td>" + element.Type + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                }
                else {
                    if (!fromprogressnote) {
                        $DetailsTable.append("<tr><td><strong> Date:</strong></td><td>&nbsp</td><td class='pr-sm'>" + element.Date + " " + element.Time + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                        $DetailsTable.append("<tr><td><strong> Category:</strong></td><td>&nbsp</td><td>" + element.Type + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                    }
                }

                if (element.Type == 'Incoming') {

                    var statusName = Patient_Referrals.getStatusName(element.Status);

                    if (element.Visits != "" && element.Visits != null) {
                        var visitType = Patient_Referrals.getVisitName(element.Visits);
                        $DetailsTable.append("<tr>" + "<tr><td><strong> Visit Type:</strong></td><td>&nbsp</td><td>" + visitType + "</td>" + "<td>&nbsp</td>" + "<td><strong> Status:</strong></td><td>&nbsp</td><td>" + statusName + "</td></tr>");
                    }
                    else {
                        if (statusName != '' && statusName != null) {
                            $DetailsTable.append("<tr><td><strong> Status:</strong></td><td>&nbsp</td><td>" + statusName + "</td></tr>");
                        }
                    }
                }
                else {
                    if (element.Visits != "" && element.Visits != null) {

                        if (!fromprogressnote) {
                            var visitType = Patient_Referrals.getVisitName(element.Visits);

                            $DetailsTable.append("<tr>" + (visitType && visitType != "" ? "<td><strong> Type:</strong></td><td>&nbsp</td><td>" + visitType + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td></td>" + (element.SpecialityToName && element.SpecialityToName != "" ? "<td><strong class='noWordBreak'>Referral To Specialty:</strong></td><td>&nbsp</td><td>" + element.SpecialityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");

                        }
                    } else {
                        $DetailsTable.append("<tr>" + (element.SpecialityToName && element.SpecialityToName != "" ? "<td><strong class='noWordBreak'> Referral To Specialty:</strong></td><td>&nbsp</td><td>" + element.SpecialityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td></td>" + ("" != "" ? "<td><strong class='noWordBreak'>Referral To Specialty:</strong></td><td>&nbsp</td><td>" + "" + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                    }
                }

                if (element.Type == 'Incoming') {
                    if (element.Provider != "" && element.Provider != null) {
                        $DetailsTable.append("<tr>" + (element.Provider != "" ? "<td><strong> Referral To:</strong></td><td>&nbsp</td><td>" + element.Provider + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td>&nbsp</td>" + (element.RefProvider != "" ? "<td><strong> Referral From:</strong></td><td>&nbsp</td><td>" + element.RefProvider + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                    }
                    else {
                        $DetailsTable.append("<tr><td><strong> Referral From:</strong></td><td>&nbsp</td><td>" + element.RefProvider + "</td></tr>");
                    }
                }
                else {
                    if (!fromprogressnote) {
                        if (element.RefProvider != "" && element.RefProvider != null) {
                            $DetailsTable.append("<tr>" + (element.RefProvider != "" ? "<td><strong> Referral To:</strong></td><td>&nbsp</td><td>" + element.RefProvider + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td>&nbsp</td>" + (element.Provider != "" ? "<td><strong> Referred From:</strong></td><td>&nbsp</td><td>" + element.Provider + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                        }
                        else {
                            if (element.Provider != null && typeof element.Provider != typeof undefined) {
                                $DetailsTable.append("<tr>" + (element.Provider != "" ? "<td><strong> Referred From:</strong></td><td>&nbsp</td><td>" + element.Provider + "</td>" : "") + "</tr>");
                            }
                            else {
                                $DetailsTable.append("<tr>" + (element.ProviderName && element.ProviderName != "" ? "<td><strong> Referred From:</strong></td><td>&nbsp</td><td>" + element.ProviderName + "</td>" : "") + "</tr>");
                            }
                        }
                    }
                }

                if (element.Type == 'Outgoing' && !fromprogressnote) {
                    if (element.FacilityFromName != "" && element.FacilityToName != "") {
                        $DetailsTable.append("<tr>" + (element.FacilityToName != "" && element.FacilityToName != null ? "<td><strong> Facility To:</strong></td><td>&nbsp</td><td>" + element.FacilityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "<td>&nbsp</td>" + (element.FacilityFromName != "" && element.FacilityFromName != null ? "<td><strong> Facility From:</strong></td><td>&nbsp</td><td>" + element.FacilityFromName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                    }
                    else if (element.FacilityFromName == "" && element.FacilityToName != "") {
                        $DetailsTable.append("<tr>" + (element.FacilityToName != "" && element.FacilityToName != null ? "<td><strong> Facility To:</strong></td><td>&nbsp</td><td>" + element.FacilityToName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                    }
                    else if (element.FacilityFromName != "" && element.FacilityToName == "") {
                        $DetailsTable.append("<tr>" + (element.FacilityFromName != "" && element.FacilityFromName != null ? "<td><strong> Facility From:</strong></td><td>&nbsp</td><td>" + element.FacilityFromName + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                    }

                    if (element.ReferralProcedure != null && typeof element.ReferralProcedure != typeof undefined) {
                        $.each(element.ReferralProcedure, function (index, element) {
                            if (element.ShowCPTCode == 1) {
                                $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Procedure(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element.Procedure + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                            }
                            else {
                                $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Procedure(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element.CPTCodeDescription + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                            }
                        });
                    }
                    else {
                        if (element.Procedures != null && typeof element.Procedures != typeof undefined) {
                            $.each(element.Procedures.split(','), function (index, element) {
                                $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Procedure(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                            });
                        }
                    }

                    if (element.ReferralProblemList != null && typeof element.ReferralProblemList != typeof undefined) {
                        $.each(element.ReferralProblemList, function (index, element) {
                            $DetailsTable.append("<tr>" + (index == 0 ? "<td><strong>Problem(s)</strong></td>" : "<td></td>") + "<td>&nbsp</td><td>" + element.ProblemName + "</td><td>&nbsp</td><td></td><td>&nbsp</td><td></td></tr>");
                        });
                    }
                }

                if (element.Type == 'Incoming') {
                    if (element.Comments != "" && element.Comments != null) {

                        $DetailsTable.append("<tr>" + (element.Comments != "" ? "<td><strong> Comments:</strong></td><td>&nbsp</td><td>" + element.Comments + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                    }
                } else {
                    if (element.Comments != "" && element.Comments != null) {
                        if (!fromprogressnote)
                            $DetailsTable.append("<tr>" + (element.Comments != "" ? "<td><strong> Comments:</strong></td><td>&nbsp</td><td>" + element.Comments + "</td>" : "<td></td><td>&nbsp</td><td></td>") + "</tr>");
                    }
                }



                $ListVital.append($DetailsTable);
                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_ProblemList').parent().parent().find('#Cli_ProblemList_Main' + PLid).length == 0) {
                if ($(NoteHTMLCtrl + ' clinical_referrals').parent().parent().find('#Patient_Referrals_Main' + PLid).length == 0) {
                    PListId.push(PLid);
                    $mainDivVital.append($SectionBodyVital);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_referrals').parent().parent().find('#Patient_Referrals_Main' + PLid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_referrals').parent().parent().find('#Patient_Referrals_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_referrals').parent().parent().find('#Patient_Referrals_Main' + PLid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_referrals').parent().parent().find('#Patient_Referrals_Main' + PLid + ' ul').append(CommentHTML);;
                }

            });
            //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if (PListId.join(",") != "") {
                ReferralId = PListId.join(",");
            }
            //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if ($mainDivVital.html() != '') {
                //Start//04//01//2015//Ahmad Raza//
                Patient_Referrals.updateReferralHtml($mainDivVital.html(), ReferralId, NoteHTMLCtrl, hideAlertMessage, bNotSaveComponent);
            } else {
                Patient_Referrals.updateReferralHtml('', ReferralId, NoteHTMLCtrl, hideAlertMessage, bNotSaveComponent);
            }
        }
    },
    // This Function is called by Progress Notes (Fill Vitals Func, CopyAllNotesCategories)
    updateReferralHtml: function (ProblemListHtml, ProblemListId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt) {
        $(NoteHTMLCtrl + ' clinical_referrals').parent().parent().addClass('initialVisitBody');
        if (ProblemListHtml != '') {
            $(NoteHTMLCtrl + ' clinical_referrals').parent().parent().append(ProblemListHtml);
        }


        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ProblemListHtml != '') {
            Patient_Referrals.attachReferralFromNotes(ProblemListId, hideAlertMessage, bNotSaveCompt);
        }

    },
    attachReferralFromNotes: function (ReferralId, hideAlertMessage, bNotSaveCompt) {

        var selectedValue = ReferralId;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Patient_Referrals.attachReferralWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (!bNotSaveCompt)
                        Clinical_ProgressNote.saveComponentSOAPText('Referrals', hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

    },
    attachReferralWithNotes_DBCall: function (ReferralId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "attach_referral_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    detachReferralFromNotes_DBCall: function (ReferralId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "detach_referral_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    SearchReferralForSoapText: function (ReferralId, Mode, PageNumber, RowsPerPage) {

        var objData = new Object();
        objData["ReferralId"] = ReferralId;
        objData["PatientId"] = Patient_Referrals.params.patientID;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "get_referral_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },
    //This Functions ask for Detaching Referral from Progress Note for current Patient Selected
    detachReferralFromNotes: function (ReferralId) {
        var strMessage = "";
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        //AppPrivileges.GetFormPrivileges("Medical_Problems List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        // if (strMessage == "") {
        utility.myConfirm('1', function () {
            EMRUtility.scrollToPNcomponent('clinical_referrals');
            var selectedValue = ReferralId.replace('Patient_Referrals_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_Referrals.detachReferralFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + ReferralId).remove();
                        Clinical_ProgressNote.saveComponentSOAPText('Referrals');
                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                        //   utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );
        //}
        //else
        //   utility.DisplayMessages(strMessage, 2);
        //});
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    //If ProblemLists Component which is dropeed in Progress note has no ProblemList attached, than it will call for Latest ProblemList for this patient
    getLatestReferralByPatientId: function (hideAlertMessage, fromprogressnote, bNotSaveComponent) {

        Patient_Referrals.getLatestReferralByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Patient_Referrals.createReferralBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, undefined, bNotSaveComponent);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });

    },
    getLatestReferralByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "getlatest_referralby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },


    //This Function detach Referral From progress note
    detach_ComponentsReferral: function (ComponentName, IsUpdate, ComponentRemove) {

        var Clinical_ReferralIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_referrals').parent().parent().find('section[id*="Patient_Referrals_Main"]').map(function () {
            return this.id.replace("Patient_Referrals_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ReferralsComponent').attr('NoteComponentId');
        if (ComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_referrals').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Referrals', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Referrals']").remove();
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_referrals').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Referrals']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_referrals').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Referrals', true))
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
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Referrals']").remove();
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_referrals').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    utility.DisplayMessages('Successfully Deleted', 1);
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_referrals').parent().parent().find('section[id*="Patient_Referrals_Main"]').remove();
        }

        if (Clinical_ReferralIds == "" || Clinical_ReferralIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_referrals').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Referrals', true))
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
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Referrals']").remove();
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_referrals').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                utility.DisplayMessages('Successfully Deleted', 1);
            });
        }
        else {
            Patient_Referrals.detachReferralFromNotes_DBCall(Clinical_ReferralIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Referrals', true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    // problem list grid load as per admin

    AddOutgoingReferrals: function () {
        var params = [];
        // Ast-357
        if (Patient_Referrals.params.ParentCtrl && Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
            params["IsFromNote"] = true;
        }
        else {
            params["IsFromNote"] = false;
        }
        params["CurrentNotesProviderId"] = Patient_Referrals.params["CurrentNotesProviderId"];
        params["ParentCtrl"] = Patient_Referrals.params.ParentCtrl != "clinicalTabProgressNote" ? "patTabReferrals" : "Patient_Referrals";

        //Start// For Bug#QAC1-260
        //if (Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet") {
        //    params["ParentCtrl"] = "Patient_Referrals";
        //    params["PanelID"] = Patient_Referrals.params.PanelID;
        //}
        //else
        if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {
            params["ParentCtrl"] = 'Patient_Referrals';
            params["ParentCtrlPanelID"] = Patient_Referrals.params.PanelID;
        }
        else {
            params["ParentCtrl"] = "patTabReferrals";
        }
        //End// For Bug#QAC1-260
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["PatientId"] = Patient_Referrals.params["patientID"];
        LoadActionPan("Patient_Referrals_Outgoing_Detail", params, Patient_Referrals.params.PanelID);
    },
    AddIncomingReferrals: function () {
        var params = [];
        params["ParentCtrl"] = Patient_Referrals.params.ParentCtrl != "clinicalTabProgressNote" ? "patTabReferrals" : "Patient_Referrals";

        //Start// For Bug#QAC1-260
        if (Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {
            params["ParentCtrl"] = "Patient_Referrals";
            params["PanelID"] = Patient_Referrals.params.PanelID;
        }
        //End// For Bug#QAC1-260
        params["FromAdmin"] = 0;

        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["PatientId"] = Patient_Referrals.params["patientID"];
        LoadActionPan("Patient_Referrals_Incoming_Detail", params);
    },
    SelectTab: function (Type) {
        Patient_Referrals.Type = Type;
        //$(Patient_Referrals.params.PanelID + " #headingTitle").html("Search " + Type + " Referrals");
        if (Patient_Referrals.Type == "Incoming") {
            Patient_Referrals.ValidateIncomingTab();
            if (!$("#" + Patient_Referrals.params.PanelID + " #Incoming").hasClass("active")) {
                $("#" + Patient_Referrals.params.PanelID + " #Incoming").addClass("active");
            }
            $("#" + Patient_Referrals.params.PanelID + " #Outgoing").removeClass("active");
        }
        else {
            Patient_Referrals.ValidateOutcomingTab();
            $("#" + Patient_Referrals.params.PanelID + " #Incoming").removeClass("active");
            if (!$("#" + Patient_Referrals.params.PanelID + " #Outgoing").hasClass("active")) {
                $("#" + Patient_Referrals.params.PanelID + " #Outgoing").addClass("active");
            }
        }

        Patient_Referrals.ReferralSearch();
        return true;
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindDropDownsByID("#pnlPatientReferrals #ddlInsurancePlan", 'GetPatientInsurance', true, (Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val())).done(function () {
        });


    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Referrals";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Patient_Referrals.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Patient_Referrals.params.PanelID);
    },

    UnLoadTab: function () {
        var parentPanelId = null;
        var objDeffered = $.Deferred();
        if (Patient_Referrals.params["FromAdmin"] == "0") {
            if (Patient_Referrals.params != null && Patient_Referrals.params.ParentCtrl != null) {
                if (Patient_Referrals != null && Patient_Referrals.params.ParentCtrl != null && Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                    var exist = false;
                    $("#" + Patient_Referrals.params.PanelID + " #dgvOutgoingReferral tbody").find('input[type="checkbox"]').each(function () {
                        if (this.checked) {
                            exist = true;
                        }
                        if (exist) {
                            return false;
                        }
                    });
                    if (exist) {
                        utility.myConfirmNote('1', function () {
                            Patient_Referrals.addReferralsToNotes('Outgoing');
                            UnloadActionPan(Patient_Referrals.params.ParentCtrl, 'Patient_Referrals');
                            EMRUtility.scrollToPNcomponent('clinical_referrals');
                        }, "", function () {
                            UnloadActionPan(Patient_Referrals.params.ParentCtrl, 'Patient_Referrals');
                            EMRUtility.scrollToPNcomponent('clinical_referrals');
                        });
                    }
                    else {
                        UnloadActionPan(Patient_Referrals.params.ParentCtrl, 'Patient_Referrals');
                        EMRUtility.scrollToPNcomponent('clinical_referrals');
                    }

                    //Patient_Referrals.addReferralsToNotes('Outgoing');
                } else {
                    if (Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {
                        parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                        Clinical_FaceSheet.params.ChildPanelID = null;
                        UnloadActionPan(Patient_Referrals.params.ParentCtrl, 'Patient_Referrals', null, parentPanelId);
                    } else {
                        UnloadActionPan(Patient_Referrals.params.ParentCtrl, 'Patient_Referrals');
                    }
                }
            }
            else
                UnloadActionPan(null, 'Patient_Referrals');
        }
        else {

            RemoveAdminTab();
        }

        if (Patient_Referrals.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_FaceSheet.loadFaceSheet();
        }

        objDeffered.resolve();
        return objDeffered;



    },

    validateSpecialCharacters: function (event) {
        var valid = (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }
    },
    bindAutoComplete: function (element, refCtrlId) {

        var hiddenCrtl = $('#' + Patient_Referrals.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCode'));
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Patient_Referrals", null, true);
    },

    fillRefferal: function () {
        Patient_Referrals.fillRefferalFromDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                //var referringProviders = JSON.parse(response.ReferralFrom);
                //var providers = JSON.parse(response.ReferralTo);

                var $ddlReferralFromIncoming = $('#' + Patient_Referrals.params.PanelID + ' #Incoming #ddlReferralFrom');
                var $ddlReferralToIncoming = $('#' + Patient_Referrals.params.PanelID + ' #Incoming #ddlReferralTo');

                var $ddlReferralFromOutgoing = $('#' + Patient_Referrals.params.PanelID + ' #Outgoing #ddlReferralFrom');
                var $ddlReferralToOutgoing = $('#' + Patient_Referrals.params.PanelID + ' #Outgoing #ddlReferralTo');

                $ddlReferralFromIncoming.empty();
                $ddlReferralToOutgoing.empty();

                $ddlReferralToIncoming.empty();
                $ddlReferralFromOutgoing.empty();

                var incomingReferralFrom = JSON.parse(response.IncomingReferralFrom);
                var incomingReferralTo = JSON.parse(response.IncomingReferralTo);
                var outgoingReferralFrom = JSON.parse(response.OutgoingReferralFrom);
                var outgoingReferralTo = JSON.parse(response.OutgoingReferralTo);

                $.each(incomingReferralFrom, function (i, item) {
                    $ddlReferralFromIncoming.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });

                $.each(incomingReferralTo, function (i, item) {
                    $ddlReferralToIncoming.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });

                $.each(outgoingReferralFrom, function (i, item) {
                    $ddlReferralFromOutgoing.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });

                $.each(outgoingReferralTo, function (i, item) {
                    $ddlReferralToOutgoing.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });
            }

        });
    },

    fillRefferalFromDBCall: function () {
        var objData = new Object();
        objData["PatientId"] = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "getreferringfromprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },

    ReferralSearchByType: function (sender) {
        if ($(sender).parent().parent().attr('id') == "IncomingReferralSearch") {
            Patient_Referrals.Type = "Incoming";
        }
        else {
            Patient_Referrals.Type = "Outgoing";
        }
        Patient_Referrals.ReferralSearch();
    },
    //Function Name: getVisitName
    //Author: Humaira Yousaf
    //Date: 19-08-2016
    //Description: Gets Visit Name by Visit Id
    getVisitName: function (visitId) {
        var typeName = '';
        switch (parseInt(visitId)) {
            case 1:
                typeName = "Follow Up";
                break;
            case 2:
                typeName = "New Patient";
                break;
            case 3:
                typeName = "Physical";
                break;
            case 4:
                typeName = "Surgical Clearance";
                break;
            case 5:
                typeName = "Consult";
                break;
            case 6:
                typeName = "Office Visit";
                break;
            case 7:
                typeName = "Immunization";
                break;
            case 8:
                typeName = "Injection";
                break;
            case 9:
                typeName = "Bloodwork";
                break;
            case 10:
                typeName = "Ultrasound";
                break;
            case 11:
                typeName = "Procedure";
                break;
            case 12:
                typeName = "Surgery";
                break;
            case 13:
                typeName = "Infusion";
                break;
            case 14:
                typeName = "Test";
                break;
            default:
                break;
        }

        return typeName;
    },
    //Function Name: getStatusName
    //Author: Humaira Yousaf
    //Date: 19-08-2016
    //Description: Gets Status Name by Status Id
    getStatusName: function (statusId) {

        var statusName = '';
        switch (parseInt(statusId)) {
            case 1:
                statusName = "Not started";
                break;
            case 2:
                statusName = "In progress";
                break;
            case 3:
                statusName = "Completed";
                break;
            default:
                break;
        }

        return statusName;
    },
}