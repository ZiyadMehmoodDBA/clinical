Letter_Template = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Letter_Template.params = params;

        if (Letter_Template.params.PanelID != 'pnlLetterTemplate') {
            Letter_Template.params.PanelID = Letter_Template.params.PanelID + ' #pnlLetterTemplate';
        } else {
            Letter_Template.params.PanelID = 'pnlLetterTemplate';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Letter_Template.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        var self = $('#' + Letter_Template.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Letter_Template.letterTemplatesSearch();
        });
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (Letter_Template.params["FromAdmin"] == "0") {
            if (Letter_Template.params != null && Letter_Template.params.ParentCtrl != null) {
                UnloadActionPan(Letter_Template.params.ParentCtrl, 'Letter_Template');
            }
            else
                UnloadActionPan(null, 'Letter_Template');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    AddTemplate: function () {
        var params = [];
        params["ParentCtrl"] = "Letter_Template";
        params["mode"] = "Add";
        params["FromAdmin"] = 0;
        LoadActionPan("Add_Letter_Template", params);
    },
    letterTemplatesSearch: function (TemplateLetterId, PageNo, rpp) {

        //AppPrivileges.GetFormPrivileges(" Notes", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {


        $('#pnlClinicalComplaints #ulCompliantDisease').html("");
        $('#pnlClinicalComplaints #ulCompliantDisease').html("");

        AppPrivileges.GetFormPrivileges("Template_Letter", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlLetterTemplate #pnlLetterTemplate_Result").css("display") == "none") {
                    $("#pnlLetterTemplate #pnlLetterTemplate_Result").show();
                }

                var self = $("#pnlLetterTemplate #frmLetterTemplate");
                var myJSON = self.getMyJSON();
                //var myJSON = self.getMyJSONByName();
                Letter_Template.searchLetterTemplates(myJSON, TemplateLetterId, PageNo, rpp).done(function (response) {//kr
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Letter_Template.letterTemplatesGridLoad(response);//kr
                        var TableControl = "pnlLetterTemplate #dgvLetterTemplate";
                        var PagingPanelControlID = "pnlLetterTemplate #divLetterTemplatelistPaging";
                        var ClassControlName = "Letter_Template";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.LetterTemplatesCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Letter_Template.letterTemplatesSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                    }
                    else {
                        utility.DisplayMessages(strMessage, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

        //});
    },
    searchLetterTemplates: function (letterTemplateData, TemplateLetterId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        } 
        var objData = JSON.parse(letterTemplateData);
        objData["TemplateLetterId"] = TemplateLetterId;
        objData["Name"] = unescape(objData.Name);
        objData["Description"] = unescape(objData.Description);
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        if (Letter_Template.Switch == 1) objData["IsActive"] = true; else objData["IsActive"] = false;
        objData["commandType"] = "SEARCH_LETTER_TEMPLATES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },
    letterTemplatesGridLoad: function (response) {
        $("#pnlLetterTemplate #dgvLetterTemplate").dataTable().fnDestroy();
        $("#pnlLetterTemplate #pnlLetterTemplate_Result #dgvLetterTemplate tbody").find("tr").remove();
        //var isactive = $('#' + Letter_Template.params.PanelID + ' #pnlLetterTemplate_Result #divSwitch #switchActive').attr('isactive');
        //var isactive = $('#' + Letter_Template.params.PanelID + ' #pnlLetterTemplate_Result #switchActive').attr('isactive');

        if (response.LetterTemplatesCount > 0) {
            var letterTemplateJSONData = JSON.parse(response.LetterTemplatesLoad_JSON);
            $.each(letterTemplateJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Letter_Template.RowEdit('" + item.TemplateLetterId + "',event);");
                $row.attr("TemplateLetterId", item.TemplateLetterId);

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

                //$row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Sign Note" onclick="Clinical_Notes.SignNotes(' + item.NotesId + ');" ' + Isdisabled + '> <i class="fa fa-calculator black"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td>' + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.ChiefComplaint + '</td>' + '</td><td>' + item.NoteStatus + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td>');
                //$row.append('<td style="display:none;">' + item.TemplateLetterId + '</td><td></td><td>' + item.Name + '</td>' + '</td><td>' + item.Description + '</td>' + '</td><td>' + item.CategoryName + '</td>' + '</td><td>' + item.ModifiedOn + '</td>');
                $row.append('<td style="display:none;">' + item.TemplateLetterId + '</td><td><a ' + ' class="btn  btn-xs" href="#" onclick="Letter_Template.letterTemplateDelete(' + item.TemplateLetterId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + ' class="btn btn-xs" href="#" onclick="Letter_Template.RowEdit(' + item.TemplateLetterId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Letter_Template.LetterTemplateActiveInactive(' + item.TemplateLetterId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.Name + '</td>' + '</td><td>' + item.Description + '</td>' + '</td><td>' + item.CategoryName + '</td>' + '</td><td>' + item.ModifiedOn + " By " + item.ModifiedByName + '</td>');
                $("#pnlLetterTemplate_Result #dgvLetterTemplate tbody").last().append($row);
            });
        }
        else {
            $('#pnlLetterTemplate #dgvLetterTemplate').DataTable({
                "language": {
                    "emptyTable": "No Letter Template Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlLetterTemplate #dgvLetterTemplate'))
            ;
        else {
            $("#pnlLetterTemplate #pnlLetterTemplate_Result #dgvLetterTemplate").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
        var isactive;
        var checked;
        if (Letter_Template.Switch == 1) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "0";
            checked = '';
        }

        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                     '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Letter_Template.ActiveLetterTemplatesSearch(this);">' +
                      '</div><span class="pl-xs">Active</span>';
        $("#" + Letter_Template.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        //-----------------------
        $('#' + Letter_Template.params.PanelID + ' #dgvLetterTemplate').dataTable().fnSettings().aoColumns[0].bSortable = false;
        //----------------------------------
        EMRUtility.SwicthWidgetInializatoin();
        //-----------------------
    },
    LetterTemplateActiveInactive: function (TemplateLetterId, isActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template_Letter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = TemplateLetterId;
                    if (selectedValue == "" || typeof selectedValue == "undefined") {
                    }
                    else {

                        Letter_Template.UpdateLetterTemplateActiveInactive(selectedValue, isActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Letter_Template.letterTemplatesSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
            '3', null, null, null, isActive
        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    UpdateLetterTemplateActiveInactive: function (TemplateLetterId, isActive) {
        var objData = {};
        objData["TemplateLetterId"] = TemplateLetterId;
        if (isActive == 1) objData["IsActive"] = true; else objData["IsActive"] = false;
        objData["commandType"] = "ActiveInactive_Template_Letter";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },
    ActiveLetterTemplatesSearch: function (obj) {


        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Letter_Template.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Letter_Template.Switch = 1;
        }

        Letter_Template.letterTemplatesSearch();
    },
    letterTemplateDelete: function (TemplateLetterId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template_Letter", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('33', function () {
                    var selectedValue = TemplateLetterId;
                    var oTable = $('#dgvLetterTemplate').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || typeof selectedValue == "undefined") {
                    }
                    else {
                        Letter_Template.letterTemplatesDeleted(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Letter_Template.letterTemplatesSearch();
                                utility.DisplayMessages(response.Message, 1);
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
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    letterTemplatesDeleted: function (TemplateLetterId) {
        var objData = {};
        objData["TemplateLetterId"] = TemplateLetterId;
        objData["commandType"] = "DELETE_LETTER_TEMPLATE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },
    RowEdit: function (TemplateLetterId,event) {

        if (event != null) {
            event.stopPropagation();
        }

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template_Letter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                //params["mode"] = "Add";
                params["TemplateLetterId"] = TemplateLetterId;
                params["ParentCtrl"] = "Letter_Template";
                params["FromAdmin"] = 0;
                params["mode"] = "Edit";
                LoadActionPan("Add_Letter_Template", params);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    }

}
