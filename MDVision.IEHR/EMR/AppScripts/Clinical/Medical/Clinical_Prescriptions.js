/*Author: Muhammad Azhar Shahzad
Date: Januray 18, 2016
Reason: To show Prescriptions Refill and Pending Prescriptions Quick Links  */

Clinical_Prescriptions = {
    bIsFirstLoad: true,
    params: [],
    isViewed: false,
    IsPrescriptionDeleted: false,
    openDrFirstRefillPrescription: function () {
        BackgroundLoaderShow(true);
        var params = [];
        params["FromAdmin"] = 0;
        params["StartupScreen"] = "message";
        LoadActionPan("DRFirst", params);
    },
    openDrFirstPendingPrescription: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Prescription", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                BackgroundLoaderShow(true);
                var params = [];
                params["FromAdmin"] = 0;
                params["StartupScreen"] = "report";
                LoadActionPan("DRFirst", params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //-----------------------------------------------
    RefreshPrescriptionCount: function () {
        Clinical_Prescriptions.RefreshMessageAndTaskCount().done(function (response) {

            if (response.status != false) {
                $('#spnPrescriptionsRefillCount').text(response.refillPrescriptionCount);
                $('#spnPendingPrescriptionsCount').text(response.pendingPrescriptionCount);
            }

        });
    },

    RefreshPrescriptionCount_DBCALL: function () {
        var objData = [];

        objData["commandType"] = "GET_PRESCRIPTIONSCOUNT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Rcopia");
        // serach parameter , class name, command name of class

    },
    //Start By Khaleel Ur Rehman to check uncheck all checkboxs by a checkbox in header, Date : 22 Jan 2016.
    checkUncheckAll: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Medications.params.PanelID + " [name='SelectCheckBoxPrescription']").prop("checked", true);
        } else {
            $("#" + Clinical_Medications.params.PanelID + " [name='SelectCheckBoxPrescription']").prop("checked", false);
        }
        $("#" + Clinical_Medications.params.PanelID + " #dgvPrescriptions tbody").find('input[type="checkbox"]').each(function () {
            Clinical_Prescriptions.enableAddPrescription(this);
        });
    },
    //End By Khaleel Ur Rehman to check uncheck all checkboxs by a checkbox in header, Date : 22 Jan 2016.
    enableAddPrescription: function (currCHkBox) {
        if ($(currCHkBox).is(':checked')) {
            if ($.inArray(currCHkBox.id + 'prsc', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(currCHkBox.id + 'prsc');
            } if ($.inArray(currCHkBox.id + 'prsc', Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(currCHkBox.id + 'prsc');
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(currCHkBox.id + 'prsc');
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(currCHkBox.id + 'prsc', Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(currCHkBox.id + 'prsc');
            }
        }
    },

    /* This Function is used to get Prescriptions from DB and load that information to Grid and create pagination
        Author: ZeeshanAK  | Date: January 14, 2016  */
    prescriptionSearch: function (prescriptionId, pageNo, rpp) {
        var dfd = $.Deferred();
        /*
        Change Implement BY: Muhammad Azhar Shahzad
        Reason:Adding selection column of checkbox of Medications for Progress Notes
        Created Date: Januaray 15, 2016
    */
        if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_Medications.params.PanelID + " #dgvPrescriptions thead tr #selectRecordPrescriptions").length == 0) {
                //$("#" + Clinical_Medications.params.PanelID + " #dgvPrescriptions thead tr").prepend(' <th controlname="selectRecordPrescriptions" class="size20 p-none" id="selectRecordPrescriptions" coltype="checkbox"></th>');
                $("#" + Clinical_Medications.params.PanelID + " #dgvPrescriptions thead tr").prepend('<th><input type="checkbox" onchange="Clinical_Prescriptions.checkUncheckAll(this);" controlname="selectRecordPrescriptions" id="selectRecordPrescriptions" name="chkHeader" class="input-block text-center" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_Medications.params.PanelID + " #dgvPrescriptions th#selectRecordPrescriptions").remove();
        }
        //end change  Januaray 15, 2016
        var strMessage = "";
        var self = $("#" + Clinical_Medications.params.PanelID);
        var myJSON = self.getMyJSON();

        Clinical_Prescriptions.searchPrescriptions(myJSON, prescriptionId, pageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Start 07-11-2016 Humaira Yousaf for db audit
                Clinical_Prescriptions.isViewed = false;
                //End 07-11-2016 Humaira Yousaf for db audit
                $.when(Clinical_Prescriptions.prescriptionsGridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var TableControl = Clinical_Medications.params.PanelID + " #dgvPrescriptions";
                var PagingPanelControlID = Clinical_Medications.params.PanelID + " #divPrescriptions_Paging";
                var ClassControlName = "Clinical_Medications";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.PrescriptionCount, pageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (primaryID, pageNumber, resultPerPage) {
                    Clinical_Prescriptions.prescriptionSearch(primaryID, pageNumber, resultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });
        return dfd;
    },

    /* This function will handle load of Prescriptions. It represents service call to API
       Author: ZeeshanAK | Date: January 14,2015 */
    searchPrescriptions: function (notesData, prescriptionId, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = JSON.parse(notesData);

        objData["PrescriptionId"] = prescriptionId;

        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        if (Clinical_Medications.params.patientID != null) {
            objData["PatientId"] = Clinical_Medications.params.patientID;
        }
        else {
            objData["PatientId"] = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        //Start 07-11-2016 Humaira Yousaf for db audit
        objData["isViewed"] = Clinical_Prescriptions.isViewed;
        //End 07-11-2016 Humaira Yousaf for db audit
        objData["commandType"] = "SEARCH_PRESCRIPTIONS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Prescriptions");
    },

    /* This function will load grid for Prescriptions.
       Author: ZeeshanAK | Date: January 14,2015 */
    prescriptionsGridLoad: function (response) {
        var dfd = $.Deferred();
        //Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        if ($.fn.dataTable.isDataTable("#" + Clinical_Medications.params.PanelID + " #pnlPrescriptions_Result #dgvPrescriptions")) {
            $("#" + Clinical_Medications.params.PanelID + "  #pnlPrescriptions_Result #dgvPrescriptions").dataTable().fnClearTable();
            $("#" + Clinical_Medications.params.PanelID + "  #pnlPrescriptions_Result #dgvPrescriptions").dataTable().fnDestroy();
            $("#" + Clinical_Medications.params.PanelID + "  #pnlPrescriptions_Result #dgvPrescriptions tbody").find("tr").remove();
        }
        else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvPrescriptions') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_Medications.params.PanelID + " #pnlPrescriptions_Result #dgvPrescriptions tbody").find("tr").remove();
                    $("#" + Clinical_Medications.params.PanelID + " #pnlPrescriptions_Result #dgvPrescriptions").parent().parent().find('div.row').remove();
                }
            });
        }

        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search

        if (response.PrescriptionCount > 0) {
            var PrescriptinosLoadJSONData = JSON.parse(response.PrescriptionLoad_JSON);
            // get Actions
            //--------------------
            $.each(PrescriptinosLoadJSONData, function (i, item) {

                var serachstr = item.NDCID;
                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(serachstr, "Clinical_Medications", 1);
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvPrescription_row" + item.PrescriptionID + "'))");
                $row.attr("id", "gvPrescription_row" + item.PrescriptionID);
                $row.attr("status", item.Status);
                $row.attr("PrescriptionId", item.PrescriptionID);


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
                //adding checkboxes column and disabling that row, if Prescriptions already binded with notes
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.PrescriptionID + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.PrescriptionID + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }

                if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_Prescriptions.enableAddPrescription(this);" id="' + item.PrescriptionID + '" name="SelectCheckBoxPrescription" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //$row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.PrescriptionID + '</td><td>' + item.MedicationName + '</td><td>' +
                var comma = ",";
                if (item.PharmacyName == "") {
                    comma = "";
                }
                var actions = '&nbsp;<a class="btn btn-xs " href="javascript:void(O)" title="Activity Log" onclick="Clinical_Prescriptions.rowHistory(' + item.PrescriptionID + ');"> <i class="fa fa-history blue"></i></a>'
                //$row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.PrescriptionID + '</td><td>' + actions + '</td> <td>' + item.MedicationName + " " + $infoButtonrow + '</td><td>' +
                $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.PrescriptionID + '</td><td>' + actions + '</td> <td>' + item.MedicationName + " " + '</td><td>' +
                item.PharmacyName + comma + item.PharmacyAddress + comma + item.PharmacyCity + comma + item.PharmacyState + item.PharmacyZip + '</td>' + '</td><td>' + item.Refill + '</td>' + '</td><td>' + item.Status + '</td>' + '<td>' + utility.RemoveTimeFromDate(null, item.CreatedDate) + '</td>');
                $("#" + Clinical_Medications.params.PanelID + ' #pnlPrescriptions_Result #dgvPrescriptions tbody').last().append($row);
            });
        }
        else {
            $("#" + Clinical_Medications.params.PanelID + ' #pnlPrescriptions_Result #dgvPrescriptions').DataTable({
                "language": {
                    "emptyTable": "No Prescriptions"
                }, "bDestroy": true, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        // if ($.fn.dataTable.isDataTable('#' + Clinical_Medications.params.PanelID + " #dgvPrescriptions"))
        if ($('#' + Clinical_Medications.params.PanelID + " #pnlPrescriptions_Result #dgvPrescriptions_wrapper").length > 0) {
            //  $('#' + Clinical_Medications.params.PanelID + " #dgvMedications").dataTable().fnDestroy();
            //  $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
        }
        else {
            $('#' + Clinical_Medications.params.PanelID + " #pnlPrescriptions_Result #dgvPrescriptions").DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }
        $('#' + Clinical_Medications.params.PanelID + ' #dgvPrescriptions thead tr th:first-child').removeClass('sorting_asc');
        dfd.resolve();
        return dfd;
    },
    rowHistory: function (PrescriptionId) {
        var currentPrescriptionId = PrescriptionId != null ? PrescriptionId : -1;
        if (currentPrescriptionId > 0) {
            Clinical_Prescriptions.ShowHistory(currentPrescriptionId);
        }
    },
    ShowHistory: function (PrescriptionId) {
        if (Clinical_Medications.params.ParentCtrl == "Clinical_Treatment") {
            EMRUtility.showCurrentItemHistory(Clinical_Medications.params.PanelID, null, PrescriptionId, "Prescription", Clinical_Medications.params.patientID, "Clinical_Medications", null);
        }
        else {
            EMRUtility.showCurrentItemHistory(Clinical_Medications.params.PanelID, null, PrescriptionId, "Prescription", Clinical_Medications.params.patientID, Clinical_Medications.params.ParentCtrl != "clinicalTabProgressNote" ? Clinical_Medications.params.TabID : "Clinical_Medications", null);
        }
    },
    addPrescriptionsToNotesAndClose: function () {
        $.when(Clinical_Prescriptions.addPrescriptionsToNotes()).then(function () {
            if (Clinical_Medications.params && Clinical_Medications.params.ParentCtrl && Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
                Clinical_Medications.unloadBirthHistory();
            }
        });
    },
    //-----------------Progress Note Prescription-------------
    // added on Dec 04,2015 by Muhammad Azhar Shahzad
    //Call Back function to add component to Progress Note
    addPrescriptionsToNotes: function (selectedAttachedPrescriptions, selectedDetachedPrescriptions, hideAlertMessage) {
        var dfd = $.Deferred();
        /*
          Change implemented by: ZeeshanAK
          Reason:To acomodate adding both Medications and Prescriptions to Notes in one go
          Date: Januaray 25, 2016
        */
        var AttachedSelectedPrescriptions = [];
        var DettachedSelectedPrescriptions = [];
        if ((selectedAttachedPrescriptions != '' && selectedAttachedPrescriptions != null) || (selectedDetachedPrescriptions != '' && selectedDetachedPrescriptions != null)) {
            AttachedSelectedPrescriptions = selectedAttachedPrescriptions;
            DettachedSelectedPrescriptions = selectedDetachedPrescriptions;
        } else {
            var AttachSelectedMedsAndPrsc = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
            var DettachSelectedMedsAndPrsc = Clinical_ProgressNote.DetachedNoteComponentIds.slice();
            //Check for Medications Values
            if (AttachSelectedMedsAndPrsc.join().indexOf("meds") != -1 || DettachSelectedMedsAndPrsc.join().indexOf("meds") != -1) {
                var AttachSelectedMeds = EMRUtility.slicefunc(AttachSelectedMedsAndPrsc, "meds", 0, -4);
                var dettachSelectedMeds = EMRUtility.slicefunc(DettachSelectedMedsAndPrsc, "meds", 0, -4);
                Clinical_Medications.addMedicationsToNotes(AttachSelectedMeds, dettachSelectedMeds);
            }
            AttachedSelectedPrescriptions = EMRUtility.slicefunc(AttachSelectedMedsAndPrsc, "prsc", 0, -4);
            DettachedSelectedPrescriptions = EMRUtility.slicefunc(DettachSelectedMedsAndPrsc, "prsc", 0, -4);
        }

        // End of Zeeshan's change

        if (AttachedSelectedPrescriptions != null && AttachedSelectedPrescriptions != '') {
            for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Prescription_Main' + ALid).length != 0) {
                    var index = AttachedSelectedPrescriptions.indexOf(ALid);
                    if (index > -1) {
                        AttachedSelectedPrescriptions.splice(index, 1);
                    }
                }
            }
        }

        var detachedvalues = DettachedSelectedPrescriptions;
        if (detachedvalues.join() != null && detachedvalues.join() != '') {
            Clinical_Prescriptions.detachPrescriptionsFromNotes(detachedvalues).done(function () {
                if (AttachedSelectedPrescriptions.join() != null && AttachedSelectedPrescriptions.join() != '') {
                    $.when(Clinical_Prescriptions.attachPrescriptionsFromNotes(AttachedSelectedPrescriptions, hideAlertMessage)).then(function () {
                        dfd.resolve();
                    });
                } else {
                    $.when(Clinical_ProgressNote.saveComponentSOAPText('Prescription', true)).then(function () {
                        dfd.resolve();
                    });
                }
            });
        } else if (AttachedSelectedPrescriptions.join() != null && AttachedSelectedPrescriptions.join() != '') {
            $.when(Clinical_Prescriptions.attachPrescriptionsFromNotes(AttachedSelectedPrescriptions, hideAlertMessage)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },
    attachPrescriptionsFromNotes: function (AttachedSelectedPrescriptions, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_Prescriptions.getPrescriptionsInfo(AttachedSelectedPrescriptions.join(), hideAlertMessage).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_Medications.params != null && Clinical_Medications.params.PanelID.indexOf('pnlClinicalMedications') != -1) {
                    Clinical_Prescriptions.prescriptionSearch();
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            }, 5);

        });
        return dfd;
    },
    detachPrescriptionsFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        Clinical_Prescriptions.detachPrescriptionsFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Prescription_Main' + ALid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    resetPrescription: function () {
        $("#" + Clinical_Medications.params.PanelID + ' #pnlPrescriptions_Result [name=SelectCheckBoxPrescription]').prop('checked', false);
        $("#" + Clinical_Medications.params.PanelID + ' #pnlPrescriptions_Result [id=selectRecordPrescriptions]').prop('checked', false);
    },

    //this function is extra
    getPrescriptionInfo: function (PrescriptionsId) {
        if (PrescriptionsId == null || PrescriptionsId == '') {
            return false;
        }

        Clinical_Prescriptions.get_Prescriptions_ForSOAP_DBCall(PrescriptionsId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.PrescriptionSoapCount > 0) {
                        //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Medications').parent().parent().find('#Cli_Prescription_Main' + PrescriptionsId).length == 0) {
                        //    Clinical_Medications.detachPrescriptionFromNotes('Prescription', false, false, PrescriptionsId);
                        //}
                        Clinical_Prescriptions.createPrescriptionBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', PrescriptionsId);
                        if (Clinical_Medications.params != null && Clinical_Medications.params.PanelID.indexOf('pnlClinicalMedications') != -1) {
                            Clinical_Prescriptions.prescriptionSearch();
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
    },

    //This Function will check, if Prescriptions Soap is already attached in Progress note, if allergies is not attached than it will create main divs to attach allergy
    checkPrescriptionExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_prescription').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="PrescriptionComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_prescription title="Prescriptions"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Prescription\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Prescription">Prescription</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Prescription\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Prescription\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_prescription> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_prescription').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },

    createPrescriptionBodyHTMLFromNotes: function (Prescription, NoteHTMLCtrl, PrescriptionsId, hideAlertMessage) {
        var dfd = new $.Deferred();
        Clinical_Prescriptions.checkPrescriptionExists();
        if (Prescription == null || Prescription.length == 0) {
            return "";
        }

        var Prescriptionsoap_JSON = Prescription;
        var $mainDivPrescription = $(document.createElement('div'));

        var AListId = [];
        $.each(Prescriptionsoap_JSON, function (index, element) {
            var serachstr = element.NDCID;
            var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(serachstr, "clinicalTabProgressNote", 1);
            var ALid = element.PrescriptionID;
            var $SectionBodyPrescription = $(document.createElement('section'));
            $SectionBodyPrescription.attr('id', "Cli_Prescription_Main" + ALid);
            var $DetailsDiv = $(document.createElement('div'));
            $DetailsDiv.attr('id', "Cli_Prescription_" + ALid);
            var $ListPrescription = $(document.createElement('ul'));
            $ListPrescription.attr('class', 'list-unstyled')
            $SectionBodyPrescription.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Prescription_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Prescription_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');

            $ListPrescription.append("<li> <strong>" + (element.MedicationName == null || element.MedicationName == "" ? "" : element.MedicationName + " " + ", ") + " </strong>"
                + (element.Action == null || element.Action == "" ? "" : element.Action + " ")
                + (element.Dose == null || element.Dose == "" ? "" : element.Dose + " ")
                + (element.DoseUnit == null || element.DoseUnit == "" ? "" : element.DoseUnit + " ")
                + (element.Routeby == null || element.Routeby == "" ? "" : element.Routeby + " ")
                + (element.DoseTiming == null || element.DoseTiming == "" ? "" : element.DoseTiming + " ")
                + (element.Duration == null || element.Duration == "" || parseInt(element.Duration) == 0 ? "" : " for " + element.Duration + " Day(s) ")
                + (element.Quantity == null || element.Quantity == "" ? "" : ",Quantity " + element.Quantity)
                + (element.QuantityUnit == null || element.QuantityUnit == "" ? "" : " " + element.QuantityUnit) + "(s)"
                + (element.Refill == null || element.Refill == "" ? "" : " , Refill(s) " + element.Refill)
                + (element.Substitution == null || element.Substitution == "" ? "" : ((element.Substitution).toLowerCase() == 'n' ? ', Dispense as written ' : ', Substitution permitted '))
                + (element.CreatedDate == null || element.CreatedDate == "" ? "" : (utility.RemoveTimeFromDate(null, element.CreatedDate) == null ? "" : ", Prescribed on " + utility.RemoveTimeFromDate(null, element.CreatedDate)))
                + (element.ProviderName == null || element.ProviderName == "" ? "" : " by " + element.ProviderName)

            );

            $DetailsDiv.append($ListPrescription);
            $SectionBodyPrescription.append($DetailsDiv);
            if ($(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid).length == 0) {
                AListId.push(ALid);
                $mainDivPrescription.append($SectionBodyPrescription);
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid).html($SectionBodyPrescription.html());
                $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid + ' ul').append(CommentHTML);;
            }

        });
        if (AListId.join(",") != "") {
            PrescriptionsId = AListId.join(",");
        }
        var PrescriptionHtml = $mainDivPrescription.html();
        if (PrescriptionHtml != '') {

            $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().addClass('initialVisitBody');

            $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().append(PrescriptionHtml);

        }

        return dfd;
    },


    //This Function is used to create SOAP html and append it to  Progress note
    createPrescriptionBodyHTML: function (response, NoteHTMLCtrl, PrescriptionsId, hideAlertMessage) {
        var dfd = new $.Deferred();
        Clinical_Prescriptions.checkPrescriptionExists();
        if (response.PrescriptionsSoapCount > 0 && response.PrescriptionsSoap_JSON != null && response.PrescriptionsSoap_JSON != '') {
            var Prescriptionsoap_JSON = JSON.parse(response.PrescriptionsSoap_JSON);
            var $mainDivPrescription = $(document.createElement('div'));
            if (Prescriptionsoap_JSON == null || Prescriptionsoap_JSON.length == 0) {
                Clinical_ProgressNote.saveComponentSOAPText('Prescription', hideAlertMessage);
                return "";
            }
            var AListId = [];
            $.each(Prescriptionsoap_JSON, function (index, element) {
                var serachstr = element.NDCID;
                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(serachstr, "clinicalTabProgressNote", 1);
                var ALid = element.PrescriptionID;
                var $SectionBodyPrescription = $(document.createElement('section'));
                $SectionBodyPrescription.attr('id', "Cli_Prescription_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Prescription_" + ALid);
                var $ListPrescription = $(document.createElement('ul'));
                $ListPrescription.attr('class', 'list-unstyled')
                $SectionBodyPrescription.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Prescription_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Prescription_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                //$ListPrescription.append("<li> <strong>" + (element.MedicationName == null || element.MedicationName == "" ? "" : element.MedicationName + " " + $infoButtonrow + ", ") + " </strong>"
                $ListPrescription.append("<li> <strong>" + (element.MedicationName == null || element.MedicationName == "" ? "" : element.MedicationName + " " + ", ") + " </strong>"
                    + (element.Action == null || element.Action == "" ? "" : element.Action + " ")
                    + (element.Dose == null || element.Dose == "" ? "" : element.Dose + " ")
                    + (element.DoseUnit == null || element.DoseUnit == "" ? "" : element.DoseUnit + " ")
                    + (element.Routeby == null || element.Routeby == "" ? "" : element.Routeby + " ")
                    + (element.DoseTiming == null || element.DoseTiming == "" ? "" : element.DoseTiming + " ")
                    + (element.Duration == null || element.Duration == "" || parseInt(element.Duration) == 0 ? "" : " for " + element.Duration + " Day(s) ")
                    + (element.Quantity == null || element.Quantity == "" ? "" : ",Quantity " + element.Quantity)
                    + (element.QuantityUnit == null || element.QuantityUnit == "" ? "" : " " + element.QuantityUnit) + "(s)"
                    + (element.Refill == null || element.Refill == "" ? "" : " , Refill(s) " + element.Refill)
                    + (element.Substitution == null || element.Substitution == "" ? "" : ((element.Substitution).toLowerCase() == 'n' ? ', Dispense as written ' : ', Substitution permitted '))
                    + (element.CreatedDate == null || element.CreatedDate == "" ? "" : (utility.RemoveTimeFromDate(null, element.CreatedDate) == null ? "" : ", Prescribed on " + utility.RemoveTimeFromDate(null, element.CreatedDate)))
                    + (element.ProviderName == null || element.ProviderName == "" ? "" : " by " + element.ProviderName)
                    //End Change By Khaleel Ur Rehman
                );

                $DetailsDiv.append($ListPrescription);
                $SectionBodyPrescription.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivPrescription.append($SectionBodyPrescription);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid).html($SectionBodyPrescription.html());
                    $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().find('#Cli_Prescription_Main' + ALid + ' ul').append(CommentHTML);;
                }

            });
            if (AListId.join(",") != "") {
                PrescriptionsId = AListId.join(",");
            }
            if ($mainDivPrescription.html() != '') {
                $.when(Clinical_Prescriptions.updatePrescriptionHtml($mainDivPrescription.html(), PrescriptionsId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                    dfd.resolve();
                });
            } else {
                $.when(Clinical_Prescriptions.updatePrescriptionHtml('', PrescriptionsId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                    Clinical_ProgressNote.saveComponentSOAPText('Prescription', hideAlertMessage);
                    dfd.resolve();
                });

            }
        }
        return dfd;
    },

    getPrescriptionsInfo: function (PrescriptionsId, hideAlertMessage) {
        var dfd = new $.Deferred();
        if (PrescriptionsId == null || PrescriptionsId == '') {
            return false;
        }
        Clinical_Prescriptions.get_Prescriptions_ForSOAP_DBCall(PrescriptionsId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PrescriptionsSoapCount > 0) {
                    $.when(Clinical_Prescriptions.createPrescriptionBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', PrescriptionsId, hideAlertMessage)).then(function () {
                        dfd.resolve('ok');
                    });

                }
                else {
                    dfd.resolve('ok');
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve('ok');
            }

        });
        return dfd.promise();
    },

    // This Function is called by Progress Notes (Fill Prescriptions Func, CopyAllNotesCategories)
    updatePrescriptionHtml: function (PrescriptionHtml, PrescriptionID, NoteHTMLCtrl, hideAlertMessage) {
        var dfd = $.Deferred();
        $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().addClass('initialVisitBody');
        if (PrescriptionHtml != '') {
            $(NoteHTMLCtrl + ' clinical_prescription').parent().parent().append(PrescriptionHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (PrescriptionHtml != '') {
            $.when(Clinical_Prescriptions.attachPrescriptionWithNotes(PrescriptionID, hideAlertMessage)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;

    },
    //This Function detach Prescription From progress note

    detach_ComponentsPrescription: function (ComponentName, IsUpdate, PrescriptionComponentRemove) {
        var Clinical_PrescriptionIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_prescription').parent().parent().find('section[id*="Cli_Prescription_Main"]').map(function () {
            return this.id.replace("Cli_Prescription_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .PrescriptionComponent').attr('NoteComponentId');
        if (PrescriptionComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_prescription').parent().parent().addClass('hidden');
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .PrescriptionComponent').find('section[id*="Cli_Prescription_Main"]').remove();
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Prescription', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_prescription').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Prescriptions']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_prescription').parent().parent().addClass('hidden');
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .PrescriptionComponent').find('section[id*="Cli_Prescription_Main"]').remove();
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Prescription', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_prescription').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_prescription').parent().parent().find('section[id*="Cli_Prescription_Main"]').remove();
        }

        if (Clinical_PrescriptionIds == "" || Clinical_PrescriptionIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_prescription').parent().parent().addClass('hidden');
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .PrescriptionComponent').find('section[id*="Cli_Prescription_Main"]').remove();
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Prescription', true))
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
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_prescription').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                utility.DisplayMessages('Successfully Deleted', 1);
            });
        }
        else {
            Clinical_Prescriptions.detachPrescriptionsFromNotes_DBCall(Clinical_PrescriptionIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Prescription', true);
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

    //This Functions ask for Detaching Prescription sign from Progress Note for current Patient Selected
    detachPrescriptionFromNotes: function (PrescriptionID) {


        utility.myConfirm('1', function () {
            EMRUtility.scrollToPNcomponent('clinical_prescription');
            var selectedValue = PrescriptionID.replace('Cli_Prescription_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Prescriptions.detachPrescriptionsFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + PrescriptionID).remove();
                        Clinical_ProgressNote.saveComponentSOAPText('Prescription');
                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 15);
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

    },

    //This Functions attached Prescription sign to Progress Note for current Patient Selected
    attachPrescriptionWithNotes: function (prescriptionID, hideAlertMessage) {
        var dfd = $.Deferred();
        if (prescriptionID == "" || prescriptionID == "undefined") {
            dfd.resolve();
        }
        else {

            var PendingPrescriptionIDs = [];
            $.each(prescriptionID.split(","), function () {
                if ($("#" + Clinical_Medications.params.PanelID + " #gvPrescription_row" + this + "[status*='Pending']").length > 0) {
                    PendingPrescriptionIDs.push(this);
                }
            });

            Clinical_Prescriptions.attachPrescriptionWithNotes_DBCall(prescriptionID).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.when(Clinical_ProgressNote.saveComponentSOAPText('Prescription', hideAlertMessage)).then(function () {

                        if (PendingPrescriptionIDs.length > 0) {
                            MU_Alerts.UpdateMUAlertProfile("ePrescribing", Clinical_ProgressNote.params.NotesId, Clinical_Medications.params.patientID, true, null, PendingPrescriptionIDs);
                        }

                        dfd.resolve();
                    });
                    // Grid row was removing which was attaching to Note
                    //  $('#' + prescriptionID).remove();

                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        return dfd;
    },

    //This Function enable/disable add to note button
    enableAddPrescriptionWithNotes: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'prsc', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'prsc');
            } if ($.inArray(obj.id + 'prsc', Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id + 'prsc');
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id + 'prsc');
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'prsc', Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'prsc');
            }
        }
    },

    //If Prescriptions Component which is dropeed in Progress note has no Prescriptions attached, than it will call for Latest Prescriptions for this patient
    getLatestPrescriptionByPatientId: function (hideAlertMessage) {
        Clinical_Prescriptions.getLatestClinical_PrescriptionByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Prescriptions.createPrescriptionBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    getRcopiaInformaionForPrescriptionsSoap: function () {
        Clinical_Prescriptions.getLatestPrescriptionByPatientId();
    },

    //-----Server calls of Notes----------
    attachPrescriptionWithNotes_DBCall: function (PrescriptionID) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PrescriptionIDForSoap"] = PrescriptionID;
        objData["commandType"] = "attach_prescriptions_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Prescriptions");
    },

    detachPrescriptionsFromNotes_DBCall: function (PrescriptionID) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PrescriptionIDForSoap"] = PrescriptionID;
        objData["commandType"] = "detach_prescriptions_from_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Prescriptions");
    },

    get_Prescriptions_ForSOAP_DBCall: function (PrescriptionID) {
        var objData = new Object();
        objData["PrescriptionIDForSoap"] = PrescriptionID;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_Medications.params.patientID;
        objData["commandType"] = "get_Prescription_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Prescriptions");
    },

    getLatestClinical_PrescriptionByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "getlatest_prescriptionsby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Prescriptions");
    },
    getinfobuttondetials: function (searchstr) {
        var params = [];
        params["FromAdmin"] = "0";
        params["codeSystem"] = "2.16.840.1.113883.6.69";
        params["searchStr"] = searchstr;
        params["PatientId"] = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = Clinical_Prescriptions.params.TabID;
        params["UserName"] = globalAppdata.UserName;
        LoadActionPan('Clinical_InfoButtonView', params);
    },

    PrintClinicalPrescription: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Prescription", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + Clinical_Medications.params.PanelID);
                var myJSON = self.getMyJSON();
                Clinical_Prescriptions.searchPrescriptionsForPrint_DBCall().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var tbl = $('<table/>').append('<caption><b>Current Medications</b></caption><tr><th>Medication</th><th>Pharmacy</th><th>Refills</th><th>Provider</th><th>Prescribed On</th><th>Status</th></tr>');
                        if (response.PrescriptionCount > 0) {
                            var PrescriptinosLoadJSONData = JSON.parse(response.PrescriptionLoad_JSON);
                            $.each(PrescriptinosLoadJSONData, function (i, item) {
                                var $row = $('<tr/>');
                                $row.append('<td>' + (item.MedicationName ? item.MedicationName : "") + '</td><td>' + (item.PharmacyName ? item.PharmacyName : "") + '</td><td>' + (item.Refill ? item.Refill : "") + '</td><td>' + (item.Provider ? item.Provider : "") + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedDate) + '</td><td>' + (item.Status ? item.Status : "") + '</td>');
                                $(tbl).last().append($row);
                            });
                            Clinical_Medications.PrintReport('<table class="table table-bordered table-striped table-hover mb-none table-condensed">' + $(tbl).html() + '</table>');
                        } else {
                            var message = '';
                            message = 'No Prescription Found';
                            var $row = $('<tr/>');
                            $row.append('<td valign="top" colspan="8" align="center" class="dataTables_empty">' + message + '</td>');
                            $(tbl).last().append($row);
                            Clinical_Medications.PrintReport('<table class="table table-bordered table-striped table-hover mb-none table-condensed">' + $(tbl).html() + '</table>');
                        }
                    } else
                        utility.DisplayMessages(response.Message, 2);
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    searchPrescriptionsForPrint_DBCall: function () {
        var objData = {};
        if (Clinical_Medications.params.patientID != null)
            objData["PatientId"] = Clinical_Medications.params.patientID;
        else
            objData["PatientId"] = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["isViewed"] = Clinical_Prescriptions.isViewed;
        objData["commandType"] = "SEARCH_PRESCRIPTIONS_FOR_PRINT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Prescriptions");
    },
}