
Patient_Letter = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Patient_Letter.params = params;
        //Patient_Letter.params["patientID"]
        if (Patient_Letter.params.PanelID != 'pnlLetter') {
            Patient_Letter.params.PanelID = Patient_Letter.params.PanelID + ' #pnlLetter';
        } else {
            Patient_Letter.params.PanelID = 'pnlLetter';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Patient_Letter.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        
        EMRUtility.ValidateFromToDate('frmLetter', 'dpDateFrom', 'dpDateTo', true, function () { }, function () { }, "To Date should be greater than From Date");
        
        var self = $('#' + Patient_Letter.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Patient_Letter.patientTemplateLetterSearch();
        });
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (Patient_Letter.params["FromAdmin"] == "0") {
            if (Patient_Letter.params != null && Patient_Letter.params.ParentCtrl != null) {
                UnloadActionPan(Patient_Letter.params.ParentCtrl, 'Patient_Letter');
            }
            else
                UnloadActionPan(null, 'Patient_Letter');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    AddLetter: function () {
        AppPrivileges.GetFormPrivileges("Letter", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                //params["mode"] = "Add";
                params["ParentCtrl"] = "Patient_Letter";
                params["FromAdmin"] = 0;
                params["mode"] = "Add";
                params["PatientId"] = Patient_Letter.params["patientID"];
                if (Patient_Letter.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Patient_Letter';
                    params["ParentCtrlPanelID"] = Patient_Letter.params.PanelID;
                    params["NotesId"] = Patient_Letter.params.NotesId;
                    LoadActionPan('SelectLetter_Template', params, Patient_Letter.params.PanelID);
                }
                else
                    LoadActionPan("SelectLetter_Template", params);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    checkUncheckAllPatLetters: function (chkBox) {
        if ($(chkBox).is(':checked'))
            $("#" + Patient_Letter.params.PanelID + " [name='SelectCheckBoxPatLetter']").prop("checked", true);
        else
            $("#" + Patient_Letter.params.PanelID + " [name='SelectCheckBoxPatLetter']").prop("checked", false);
        $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter tbody").find('input[type="checkbox"]').each(function () {
            Patient_Letter.AttachDetachPatLetterIds(this);
        });
    },
    AttachDetachPatLetterIds: function (obj, event) {
        if (event != null)
            event.stopPropagation();
        var totalRows = $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter tbody tr input:checked").length;
        if (totalRows == selectedRows)
            $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter #SelectPatLetter input[type='checkbox']").prop("checked", true);
        else 
            $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter #SelectPatLetter input[type='checkbox']").prop("checked", false);
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'PatLetter', Clinical_ProgressNote.AttachedNoteComponentIds) == -1)
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'PatLetter');
            if ($.inArray(obj.id + 'PatLetter', Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id + 'PatLetter');
                if (index > -1)
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
            }
        }
        else {
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id + 'PatLetter');
            if (index > -1)
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            if ($.inArray(obj.id + 'PatLetter', Clinical_ProgressNote.DetachedNoteComponentIds) == -1)
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'PatLetter');
        }

        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0)
            $("#" + Patient_Letter.params.PanelID + "  #divPatLetterTempAddNote #btnAddPatLetterToNotes").prop('disabled', false);
        else 
            $("#" + Patient_Letter.params.PanelID + "  #divPatLetterTempAddNote #btnAddPatLetterToNotes").prop('disabled', true);
    },
    /*
    Author : Khaleel Ur Rehman.
    Purpose : TO search Patient Letter.
    Date : 09-March-2016.
    */
    patientTemplateLetterSearch: function (patientletterId, PageNo, rpp) {
     
        if ($('#' + Patient_Letter.params.PanelID + " #pnlLetterTemplate_Result").css("display") == "none")
            $('#' + Patient_Letter.params.PanelID + " #pnlLetterTemplate_Result").show();
        if (Patient_Letter.params.ParentCtrl && Patient_Letter.params.ParentCtrl == "clinicalTabProgressNote")
            $("#" +Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #divPatLetterTempAddNote #btnAddPatLetterToNotes").removeClass('hidden');
        else
            $("#" +Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #divPatLetterTempAddNote #btnAddPatLetterToNotes").addClass('hidden');
           
        var self = $('#' + Patient_Letter.params.PanelID + " #frmLetter");
        var myJSON = self.getMyJSON();
        Patient_Letter.searchPatientTemplateLetter(myJSON, patientletterId, PageNo, rpp).done(function (response) {//kr
            response = JSON.parse(response);
            if (response.status != false) {
                Patient_Letter.patientTemplateLetterGridLoad(response);
                var TableControl = "pnlLetter #dgvPatientTemplateLetter";
                var PagingPanelControlID = "pnlLetter #divPatientTemplateLetterlistPaging";
                var ClassControlName = "Patient_Letter";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.PatientTemplateLetterCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Patient_Letter.patientTemplateLetterSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    /*
    Author : Khaleel Ur Rehman.
    Purpose : TO search Patient Letter.
    Date : 09-March-2016.
    */
    searchPatientTemplateLetter: function (patientletterTemplateData, patientletterId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(patientletterTemplateData);
        objData["Patient_Letter_Id"] = patientletterId;
        objData["PatientId"] = Patient_Letter.params["patientID"];
        objData["Description"] = unescape(objData.Description);
        objData["Name"] = unescape(objData.Name);
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        Patient_Letter.params.ParentCtrl == "clinicalTabProgressNote" ? objData["NotesId"] = Clinical_ProgressNote.params.NotesId : objData["NotesId"] = 0;
        if (Patient_Letter.Switch == 1) objData["Isactive"] = true; else objData["Isactive"] = false;
        objData["VisitDate"] = $('#' + Patient_Letter.params.PanelID + " #dpDateFrom").val();
        objData["VisitDateTo"] = $('#' + Patient_Letter.params.PanelID + " #dpDateTo").val();
        objData["commandType"] = "SEARCH_PATIENT_TEMPLATELETTER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },
    /*
    Author : Khaleel Ur Rehman.
    Purpose : TO load Patient Letter.
    Date : 09-March-2016.
    */
    patientTemplateLetterGridLoad: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter")) {
            $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter").dataTable().fnClearTable();
            $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter").dataTable().fnDestroy();
            $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvPatientTemplateLetter') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter tbody").find("tr").remove();
                    $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter").parent().parent().find('div.row').remove();
                }
            });
        }

        if (Patient_Letter.params.ParentCtrl && Patient_Letter.params.ParentCtrl == "clinicalTabProgressNote") {
            $("#" + Patient_Letter.params.PanelID + " #dgvPatientTemplateLetter th#PatLetterActions").remove();
            if ($("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter thead tr #SelectPatLetter").length == 0) {
                $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter thead tr").prepend('<th id="SelectPatLetter" class="size10 center" coltype="checkbox"><input type="checkbox" id="chkHeaderPatientLetter" onchange="Patient_Letter.checkUncheckAllPatLetters(this);" class="pull-left" coltype="checkbox"/> </th>');
            }
        }
        else {
            $("#" + Patient_Letter.params.PanelID + " #dgvPatientTemplateLetter th#SelectPatLetter").remove();
            if ($("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter thead tr #PatLetterActions").length == 0)
                $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter thead tr th:first").after('<th id="PatLetterActions" class="size90">Actions</th>');
        }
        if (response.PatientTemplateLetterCount > 0) {
            var PatientTemplateLetterJSONData = JSON.parse(response.PatientTemplateLetterLoad_JSON);
            $.each(PatientTemplateLetterJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("PatientLetterId", item.Patient_Letter_Id);

                if (item.Isactive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var LetterName = "'" + item.Name + "'";
                var Status = "'" + item.Status + "'";
                var Isdisabled = "disabled =true";
                if (item.Status == "Draft" || item.Status == "") {
                    Isdisabled = "";
                }
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "1") {
                    if ($.inArray(item.Patient_Letter_Id + 'PatLetter', Clinical_ProgressNote.AttachedNoteComponentIds) == -1)
                        Clinical_ProgressNote.AttachedNoteComponentIds.push(item.Patient_Letter_Id + 'PatLetter');
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.Patient_Letter_Id + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1)
                        Checked = " ";
                    else 
                        Checked = " checked";
                }
                else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.Patient_Letter_Id + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1)
                        Checked = " checked";
                    else 
                        Checked = "";
                }
                if (Patient_Letter.params.ParentCtrl != "clinicalTabProgressNote")
                    Checked = "";
                if (Patient_Letter.params.ParentCtrl == "clinicalTabProgressNote")
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" onclick="Patient_Letter.AttachDetachPatLetterIds(this,event);" id="' + item.Patient_Letter_Id + '" name="SelectCheckBoxPatLetter" ' + Checked + ' class="input-block text-center"/></td>';
                var ActiveInActive = Patient_Letter.params.ParentCtrl != "clinicalTabProgressNote" ? '<a class="btn  btn-xs" href="#" onclick="Patient_Letter.PatientLetterActiveInactive(' + item.Patient_Letter_Id + "," + isactive + ',event);" title="' + activeTitle + '" ' + Isdisabled + '><i class="' + tglclass + '"></i></a>' : "";
                var editMode = 'onclick="Patient_Letter.RowEdit('+ item.Patient_Letter_Id + ',' + LetterName + ',' + Status + ', event);"';
                var ActionCol = "";
                if (Patient_Letter.params.ParentCtrl != "clinicalTabProgressNote")
                    ActionCol = '<td><a ' + Isdisabled + ' class="btn  btn-xs" href="#" onclick="Patient_Letter.PatientLetterDelete(' + item.Patient_Letter_Id + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + ' class="btn btn-xs" href="javascript:void(0);"'+ editMode + ' title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' + ActiveInActive + '</td>';
                $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.Patient_Letter_Id + '</td>' + ActionCol + '<td ' + editMode + '>' + item.Name + '</td><td ' + editMode + '>' + item.Description + '</td><td ' + editMode + '>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td ' + editMode + '>' + item.VisitTime + '</td><td ' + editMode + '>' + item.CategoryName + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.ModifiedOn + " By " + item.ModifiedByName + '</td>');
                $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter tbody").last().append($row);
            });
        }
        else {
            $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter").DataTable({
                "language": {
                    "emptyTable": "No Patient Letter Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1] }]
            });
        }


        if ($.fn.dataTable.isDataTable("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter"))
            ;
        else {
            $("#" + Patient_Letter.params.PanelID + " #pnlPatientTemplateLetter_Result #dgvPatientTemplateLetter").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 1] }] }); // to remove records per page dropdown
        }

        if (Patient_Letter.params.ParentCtrl != "clinicalTabProgressNote") {
            var isactive;
            var checked;
            if (Patient_Letter.Switch == 1) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "0";
                checked = '';
            }

            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Patient_Letter.ActivePatientLetterTemplatesSearch(this);">' +
                          '</div><span class="pl-xs">Active</span>';
            $("#" + Patient_Letter.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            //$('#' + Patient_Letter.params.PanelID + ' #dgvPatientTemplateLetter').dataTable().fnSettings().aoColumns[0].bSortable = false;
            EMRUtility.SwicthWidgetInializatoin();
        }
        
    },
    /*
    Author : M Ahmad Imran.
    Purpose : TO update Patient Letter.
    Date : 10-March-2016.
    */
    RowEdit: function (PatientLetterId, TemplateLetterName, Status, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = "Patient_Letter";
        params["Status"] = Status;
        params["FromAdmin"] = 0;
        params["Patient_Letter_Id"] = PatientLetterId;
        params["TemplateLetterText"] = TemplateLetterName;
        params["mode"] = "Edit";
        params["PatientId"] = Patient_Letter.params["patientID"];
        if (Patient_Letter.params["ParentCtrl"] == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'Patient_Letter';
            params["ParentCtrlPanelID"] = Patient_Letter.params.PanelID;
            params["NotesId"] = Patient_Letter.params.NotesId;
            LoadActionPan('Create_Letter', params, Patient_Letter.params.PanelID);
        }
        else
            LoadActionPan("Create_Letter", params);
    },
    RowEditFromNotes: function (PatientLetterId, TemplateLetterName, Status, event) {
        if (event != null)
            event.stopPropagation();
        var params = [];
        params["ParentCtrl"] = "clinicalTabProgressNote";
        params["Status"] = Status;
        params["FromAdmin"] = 0;
        params["Patient_Letter_Id"] = PatientLetterId;
        params["TemplateLetterText"] = TemplateLetterName;
        params["mode"] = "Edit";
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["PatientId"] = Patient_Letter.params["patientID"];
        LoadActionPan("Create_Letter", params);
    },
    /*
    Author : Khaleel Ur Rehman.
    Purpose : TO update Patient Letter.
    Date : 09-March-2016.
    */
    ActivePatientLetterTemplatesSearch: function (obj) {


        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Patient_Letter.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Patient_Letter.Switch = 1;
        }

        Patient_Letter.patientTemplateLetterSearch();
    },
    /*
    Author : Khaleel Ur Rehman.
    Purpose : To delete Patient Letter.
    Date : 09-March-2016.
    */
    PatientLetterDelete: function (PatientLetterId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Letter", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('34', function () {
            var selectedValue = PatientLetterId;
            var oTable = $('#dgvPatientTemplateLetter').DataTable();
            var ind = $(this).index();
            var idx = oTable.row(this).index();
            if (selectedValue == "" || typeof selectedValue == "undefined") {
            }
            else {
                Patient_Letter.PatientLetterDeleted(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Patient_Letter.patientTemplateLetterSearch();
                        utility.DisplayMessages(response.Message, 1);
                        //---------------if delete from Create Letter----
                        setTimeout(function () {
                            Create_Letter.UnLoadTab();
                            setTimeout(function () {
                                UnloadActionPan("Patient_Letter", 'SelectLetter_Template');

                            }, 100);

                        }, 100);
                        //--------------------------------------------------
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () {

        },
            '1'
        );
        //    } else {
        //        utility.DisplayMessages(strMessage, 2);
        //    }
        //});
    },
    PatientLetterDeleted: function (PatientLetterId) {
        var objData = {};
        objData["Patient_Letter_Id"] = PatientLetterId;
        objData["commandType"] = "DELETE_PATIENT_LETTER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },
    PatientLetterActiveInactive: function (PatientLetterLetterId, isActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Letter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('3', function () {
            var selectedValue = PatientLetterLetterId;
            if (selectedValue == "" || typeof selectedValue == "undefined") {
            }
            else {

                Patient_Letter.UpdatePatientLetterActiveInactive(selectedValue, isActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Patient_Letter.patientTemplateLetterSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '3', null, null, null, isActive
        );
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },
    /*
    Author : Khaleel Ur Rehman.
    Purpose : To update Patient Letter.
    Date : 09-March-2016.
    */
    UpdatePatientLetterActiveInactive: function (PatientLetterId, isActive) {
        var objData = {};
        objData["Patient_Letter_Id"] = PatientLetterId;
        objData["PatientId"] = Patient_Letter.params["patientID"];
        if (isActive == 1) objData["Isactive"] = true; else objData["Isactive"] = false;
        objData["commandType"] = "ActiveInactive_Patient_Letter";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },
}
