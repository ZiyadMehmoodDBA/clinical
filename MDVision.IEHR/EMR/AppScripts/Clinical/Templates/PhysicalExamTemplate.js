PhysicalExamTemplate = {
    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This file will handle all actions performed for PhysicalExam Template
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,

    //Author: Muhammad Arshad
    //Date: 24-02-2016
    //This function will handle fill of PhysicalExamTemplate
    Load: function (params) {
        PhysicalExamTemplate.params = params;

        PhysicalExamTemplate.params.mode = "Add";

        if (PhysicalExamTemplate.params.PanelID != 'pnlPhysicalExamTemplate') {
            PhysicalExamTemplate.params.PanelID = PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate';
        } else {
            PhysicalExamTemplate.params.PanelID = 'pnlPhysicalExamTemplate';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + PhysicalExamTemplate.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-131 */
        utility.ValidateFromToDate('frmPhysicalExamTemplate', 'dpStartDate', 'dpEndDate', true);
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-131 */
        //utility.CreateDatePicker('frmPhysicalExamTemplate #dpStartDate', function () {
        //    //on-change callback method
        //});
        //utility.CreateDatePicker('frmPhysicalExamTemplate #dpEndDate', function () {
        //    //on-change callback method
        //});


        var self = $('#' + PhysicalExamTemplate.params.PanelID);
        //Start 08-03-2016 Farooq Ahmad to load Physical Exam Templates
        if (PhysicalExamTemplate.bIsFirstLoad == true) {
            PhysicalExamTemplate.loadPhysicalExamTemplateMK();
        }
        //End 08-03-2016 Farooq Ahmad to load Physical Exam Templates
        if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(PhysicalExamTemplate.params.PanelID, 'Medical', 'PhysicalExamTemplate', 'PhysicalExamTemplate.UnLoadTab();', 'frmPhysicalExamTemplate');
        }
        if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + PhysicalExamTemplate.params.PanelID + " div#FaceSheetPager", PhysicalExamTemplate.params.FaceSheetComponents, 'problem list');
        }

        //Start//24-02-2016//Ahmad Raza//empty record label and search box shown in grid
        $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlProvider_Result #dgvProvider').DataTable({
            "language": {
                "emptyTable": "No Record Found."
            }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
        });
        //End//24-02-2016//Ahmad Raza//empty record label and search box shown in grid

        //Start//24-02-2016//Ahmad Raza//Toggle switch logic
        var HtmlOfSwitch = '<div id="divSwitch"><span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="1" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="">' +
                          '</div><span class="pl-xs">Active</span> </div>';

        $("#" + PhysicalExamTemplate.params.PanelID + ' .datatables-header div:first').html(HtmlOfSwitch);
        PhysicalExamTemplate.readyFunction();
        //End//24-02-2016//Ahmad Raza//Toggle switch logic

    },

    //Start//24-02-2016//Ahmad Raza//plugin for Toggle switch
    readyFunction: function () {

        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    //End//24-02-2016//Ahmad Raza//plugin for Toggle switch
    //Author: Muhammad Arshad
    //Date: 24-02-2016
    //This function will handle Add/Edit of PhysicalExamTemplate based on given PhysicalExamTemplateId
    PhysicalExamTemplateAddEdit: function (PhysicalExamTemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                if (PhysicalExamTemplateId != null && parseInt(PhysicalExamTemplateId) > 0) {
                    params["PhysicalExamTemplateId"] = PhysicalExamTemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["PhysicalExamTemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = PhysicalExamTemplate.params["FromAdmin"];
                params["ParentCtrl"] = 'adminTabPhysicalExamTemplate';
                LoadActionPan('PhysicalExamTemplateDetailRevamp', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    // MK

    PhysicalExamTemplateAddEditMK: function (PhysicalExamTemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (PhysicalExamTemplateId != null && parseInt(PhysicalExamTemplateId) > 0) {
                    params["PhysicalExamTemplateId"] = PhysicalExamTemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["PhysicalExamTemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = PhysicalExamTemplate.params["FromAdmin"];
                params["ParentCtrl"] = 'adminTabPhysicalExamTemplate';
                LoadActionPan('PhysicalExamTemplateDetailRevamp', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    loadPhysicalExamTemplateMK: function () {
        PhysicalExamTemplate.PhysicalExamTemplateLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PhysicalExamTemplate.PhysicalExamTemplateGridLoadMK(response);
                var TableControl = PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates";
                var PagingPanelControlID = PhysicalExamTemplate.params.PanelID + " #dgvPhysExamTemplates_Paging";
                var ClassControlName = "PhysicalExamTemplate";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                //setTimeout(
                //    CreatePagination(response.PhysExamTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                //        PhysicalExamTemplate.PhysExamTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                //    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //Author: Farooq Ahmad
    //Date: 08/03/2016
    //Description: This function will load the physical Exam template
    loadPhysicalExamTemplate: function () {
        PhysicalExamTemplate.PhysicalExamTemplateLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PhysicalExamTemplate.PhysicalExamTemplateGridLoad(response);
                var TableControl = PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates";
                var PagingPanelControlID = PhysicalExamTemplate.params.PanelID + " #dgvPhysExamTemplates_Paging";
                var ClassControlName = "PhysicalExamTemplate";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                //setTimeout(
                //    CreatePagination(response.PhysExamTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                //        PhysicalExamTemplate.PhysExamTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                //    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //Author: Farooq Ahmad
    //Date: 08/03/2016
    //Description: This function will load the physical Exam template
    PhysicalExamTemplateLoad: function () {
        var objData = new Object();
        var IsActive = null;
        IsActive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var entityId = 0;
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {
            entityId = globalAppdata["SeletedEntityId"];
        }
        objData["TemplateId"] = 0;
        objData["IsActive"] = IsActive;
        objData["EntityId"] = entityId;
        objData["commandType"] = "Load_PhyscialExam_Template";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Farooq Ahmad
    //Date: 08/03/2016
    //Description: This function will bind the physical Exam template Grid
    PhysicalExamTemplateGridLoad: function (response) {
        var isactive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        try {
            $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        } catch (ex) {
            console.log(ex);
        }
        $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.PhysicalExamTemplateCount > 0) {
            var PhysicalExamTemplateJSONData = JSON.parse(response.PhysicalExamTemplate); //Parsing array to JSON

            $.each(PhysicalExamTemplateJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "PhysicalExamTemplate.PhysExamTemplateEdit('" + item.TemplateId + "'," + item.IsDefault.toLowerCase() + ",event);");
                $row.attr("id", "dgvPhysExamTemplates" + item.TemplateId);
                $row.attr("TemplateId", item.TemplateId);

                var IsDefault = false;

                var editCall = "PhysicalExamTemplate.PhysExamTemplateEdit('" + item.TemplateId + "'," + IsDefault + ",event);";
                $row.attr("onclick", editCall);
                if (item.IsActive == "True") {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isEventactive = 1;
                    isactive = 0;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var dateInNumberFormat = Number(new Date(item.LastUpdated));

                $row.append('<td class="hidden">' + dateInNumberFormat + '</td><td style="display:none;">' + item.TemplateId + '</td><td><a class="btn btn-xs" href="#" onclick="PhysicalExamTemplate.PhysExamTemplateDelete(\'' + item.TemplateId + '\',' + IsDefault + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="PhysicalExamTemplate.PhysExamTemplateEdit(\'' + item.TemplateId + '\',' + IsDefault + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="PhysicalExamTemplate.PhysExamTemplateActiveInactive(\'' + item.TemplateId + '\',' + IsDefault + ', ' + isEventactive + ',event);"><i class="' + tglclass + '"></i></a></td><td>' + item.PhysicalExamTemplateName + '</td><td>' + item.LastUpdated + '</td>');

                $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates tbody").last().append($row);
            });
        }
        else {
            $("#" + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates').DataTable({
                "language": {
                    "emptyTable": "No Physical Exam Template Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[0, "desc"]]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates'))
            ;
        else {
            $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
        var checked = '';
        if (isactive == "0" || isactive == 0) {
        } else if (isactive == null) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                       '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="PhysicalExamTemplate.activePhysExamTemplateSearch(this);">' +
                        '</div><span class="pl-xs">Active</span>';

        $("#" + PhysicalExamTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();

    },

    PhysicalExamTemplateGridLoadMK: function (response) {
        var isactive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        try {
            $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        } catch (ex) {
            console.log(ex);
        }
        $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.PhysicalExamTemplateCount > 0) {
            var PhysicalExamTemplateJSONData = JSON.parse(response.PhysicalExamTemplate); //Parsing array to JSON
            PhysicalExamTemplateJSONData = JSON.parse(PhysicalExamTemplateJSONData)
            $.each(PhysicalExamTemplateJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "PhysicalExamTemplate.PhysExamTemplateEdit('" + item.TemplateId + "'," + item.IsDefault.toLowerCase() + ",event);");
                $row.attr("id", "dgvPhysExamTemplates" + item.PETemplateId);
                $row.attr("TemplateId", item.PETemplateId);
                var IsDefault = false;

                var editCall = "PhysicalExamTemplate.PhysExamTemplateEdit('" + item.PETemplateId + "'," + IsDefault + ",event);";
                $row.attr("onclick", editCall);
                if (item.IsActive == "True") {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isEventactive = 1;
                    isactive = 0;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var dateInNumberFormat = Number(new Date(item.ModifiedOn));

                $row.append('<td class="hidden">' + dateInNumberFormat + '</td><td style="display:none;">' + item.PETemplateId + '</td><td><a class="btn btn-xs" href="#" onclick="PhysicalExamTemplate.PhysExamTemplateDelete(\'' + item.PETemplateId + '\',' + IsDefault + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="PhysicalExamTemplate.PhysExamTemplateEdit(\'' + item.PETemplateId + '\',' + IsDefault + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="PhysicalExamTemplate.PhysExamTemplateActiveInactive(\'' + item.PETemplateId + '\',' + IsDefault + ', ' + isEventactive + ',event);"><i class="' + tglclass + '"></i></a></td><td>' + item.Name + '</td><td>' + item.ModifiedOn + '</td>');

                $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates tbody").last().append($row);
            });
        }
        else {
            $("#" + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates').DataTable({
                "language": {
                    "emptyTable": "No Physical Exam Template Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[0, "desc"]]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates'))
            ;
        else {
            $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysExamTemplates").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
        var checked = '';
        if (isactive == "0" || isactive == 0) {
        } else if (isactive == null) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                       '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="PhysicalExamTemplate.activePhysExamTemplateSearch(this);">' +
                        '</div><span class="pl-xs">Active</span>';

        $("#" + PhysicalExamTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();

    },


    //Author: Azhar Shahzad
    //Date October 06, 2016
    //Toggle Active Inactive record
    activePhysExamTemplateSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        PhysicalExamTemplate.loadPhysicalExamTemplateMK();
    },

    PhysExamTemplateEdit: function (TemplateId, IsDefault, event) {
        if (IsDefault == false) {
            PhysicalExamTemplate.PhysicalExamTemplateAddEdit(TemplateId);
        }
        else {
            utility.DisplayMessages("You can not edit default template.", 3);
        }
        event.stopPropagation();
    },

    PhysExamTemplateDelete: function (TemplateId, IsDefault, event) {
        if (IsDefault == false) {

            utility.myConfirm('Are you sure to delete template?', function () {
                PhysicalExamTemplate.DeletePhysExamTemplate(TemplateId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        PhysicalExamTemplate.loadPhysicalExamTemplateMK();
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }, function () { },
                  'Confirm Delete'
              );



        }
        else {
            utility.DisplayMessages("You can not delete default template.", 3);
        }
        event.stopPropagation();
    },

    PhysExamTemplateActiveInactive: function (TemplateId, IsDefault, isactive, event) {
        if (IsDefault == false) {
            utility.myConfirm('3', function () {
                if (TemplateId == "" || TemplateId == "undefined") {
                }
                else {
                    PhysicalExamTemplate.updatePhysicalExamTemplateActiveInactive_Dbcall(TemplateId, isactive).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            PhysicalExamTemplate.loadPhysicalExamTemplateMK();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                     '3', null, null, null, isactive
                   );
        }
        else {
            utility.DisplayMessages("You can not inactive default template.", 3);
        }
        event.stopPropagation();
    },

    /*
 Author: Farooq Ahmad
 Purpose: to change active / in active records of Grid of Ros template
 Creation Date: May 06,2016 */
    updatePhysicalExamTemplateActiveInactive_Dbcall: function (TemplateId, IsActive) {
        var objData = {};
        objData["TemplateId"] = TemplateId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_PhysicalExamTemplate_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        //  var data = "NotesId=" + NotesId + "&IsActive=" + IsActive;
        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },


    DeletePhysExamTemplate: function (TemplateId) {
        var objData = new Object();
        objData["TemplateId"] = TemplateId;
        objData["commandType"] = "DELETE_PhysicalExamTemplate";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    DownloadProblems: function () {
        var objData = [];
        objData["commandType"] = "download_data";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "downloadProblemList");
    },
    PostXmlData: function () {

        //var alergi = '<?xml version="1.0" encoding="UTF-8"?><RCExtRequest version = "2.19"> <Caller>  <VendorName>vendorsh1100</VendorName>  <VendorPassword>b2xddjpn</VendorPassword> </Caller> <SystemName>vendorsh1100</SystemName> <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername> <Request>  <Command>send_allergy</Command>  <AllergyList>     <!-- Allergy defined by drug NDC id -->   <Allergy>    <ExternalID>testax1000</ExternalID>    <RcopiaID></RcopiaID>    <Deleted>n</Deleted>    <Status><Active/></Status>     <Patient>     <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>     <ExternalID>' + $externalid.text() + '</ExternalID>    </Patient>    <Allergen>     <Name>aspirin</Name>     <Drug>      <RcopiaID></RcopiaID>      <NDCID>58487000101</NDCID>     </Drug>    </Allergen>    <Reaction>nausea</Reaction>    <OnsetDate>11/01/2015</OnsetDate>   </Allergy>      <!-- Allergy defined by allergen name, Rcopia will try to match -->   <Allergy>    <ExternalID>testax1001</ExternalID>    <RcopiaID></RcopiaID>        <Deleted>n</Deleted>    <Status><Active/></Status>     <Patient>     <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>     <ExternalID>' + $externalid.text() + '</ExternalID>    </Patient>    <Allergen>     <Name>Lisinopril</Name>    </Allergen>     <Reaction>rash</Reaction>    <OnsetDate></OnsetDate>   </Allergy>      <!-- Allergy defined by Rcopia allergy group id -->   <Allergy>    <ExternalID>testax1002</ExternalID>    <RcopiaID></RcopiaID>    <Deleted>n</Deleted>    <Patient>     <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>     <ExternalID>' + $externalid.text() + '</ExternalID>    </Patient>    <Allergen>     <Name>Glutamic Acid</Name>     <Group>      <RcopiaID>620</RcopiaID>     </Group>    </Allergen>    <Reaction>sneezing</Reaction>    <OnsetDate></OnsetDate>   </Allergy>  </AllergyList> </Request></RCExtRequest>        ';
        //var medicationData = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.19'>    <Caller>        <VendorName>vendorsh1100</VendorName>        <VendorPassword>b2xddjpn</VendorPassword>    </Caller>    <SystemName>vendorsh1100</SystemName>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername>    <Request>        <Command>send_medication</Command>        <MedicationList>            <!-- Medication defined by NDC list, with structured sig -->            <Medication>                <Deleted>n</Deleted>                <RcopiaID></RcopiaID>                <ExternalID>testmed1000</ExternalID>                <Patient>                    <RcopiaID>" + $Recopiaid.text() + "</RcopiaID>                    <ExternalID>" + $externalid.text() + "</ExternalID>                </Patient>                <Provider>                    <Username>sprovider47</Username>                </Provider>                <Sig>                    <Drug>                        <NDCID>00071015694,52959076090,49999046790</NDCID>                        <BrandName>Lipitor</BrandName>                        <GenericName></GenericName>                        <Form>tablet</Form>                        <Strength>20 mg</Strength>                    </Drug>                    <Dose>1</Dose>                    <DoseUnit>tablet</DoseUnit>                    <DoseTiming>once per day</DoseTiming>                    <Duration></Duration>                    <Quantity>60</Quantity>                    <QuantityUnit>tablet</QuantityUnit>                    <Refills>2</Refills>                    <SubstitutionPermitted>y</SubstitutionPermitted>                    <OtherNotes></OtherNotes>                    <PatientNotes>take with a glass of water</PatientNotes>                </Sig>                <StartDate>11/01/2015</StartDate>               <StopDate></StopDate>                <FillDate>11/02/2015</FillDate>                <StopReason></StopReason>            </Medication>            <!-- Free text medication. Will not be used for interaction checking -->            <Medication>                <Deleted>n</Deleted>                <RcopiaID></RcopiaID>                <ExternalID>testmed1001</ExternalID>                <Patient>                    <RcopiaID></RcopiaID>                    <ExternalID>paltrowbruce</ExternalID>                </Patient>                <Provider>                    <Username>sprovider47</Username>                </Provider>                <Sig>                    <Drug>                        <NDCID></NDCID>                        <BrandName>Elphaba's Green Pill</BrandName>                        <GenericName></GenericName>                        <Strength></Strength>                    </Drug>                    <PatientNotes>One per day as needed for mild fatigue.</PatientNotes>                </Sig>                <StartDate>11/01/2015</StartDate>                <StopDate></StopDate>                <FillDate></FillDate>                <StopReason></StopReason>            </Medication>            <!-- This is a medication that is stopped -->            <Medication>                <Deleted>n</Deleted>                <RcopiaID></RcopiaID>                <ExternalID>testmed1002</ExternalID>                <Patient>                    <RcopiaID></RcopiaID>                    <ExternalID>paltrowbruce</ExternalID>                </Patient>                <Provider>                    <Username>sprovider47</Username>                </Provider>                <Sig>                    <Drug>                        <NDCID>00006022101,00006022131,51138047930</NDCID>                        <BrandName>Januvia</BrandName>                        <GenericName>Sitagliptin</GenericName>                        <Strength>25 mg</Strength>                    </Drug>                    <Dose>1</Dose>                    <DoseUnit>tablet</DoseUnit>                    <DoseTiming>once per day</DoseTiming>                    <Duration></Duration>                    <Quantity>60</Quantity>                    <QuantityUnit>tablet</QuantityUnit>                    <Refills>2</Refills>                    <SubstitutionPermitted>y</SubstitutionPermitted>                    <OtherNotes></OtherNotes>                    <PatientNotes>take with a glass of water</PatientNotes>                         </Sig>                <StartDate></StartDate>                <StopDate>11/01/2015</StopDate>                <FillDate></FillDate>                <StopReason></StopReason>            </Medication>  </MedicationList>    </Request></RCExtRequest>        ";
        //var problemListData = '<?xml version="1.0" encoding="UTF-8"?><RCExtRequest version = "2.19">    <Caller>        <VendorName>vendorsh1100</VendorName>        <VendorPassword>b2xddjpn</VendorPassword>    </Caller>    <SystemName>vendorsh1100</SystemName>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername>    <Request>                                <Command>send_problem</Command>                                <ProblemList>                                                <Problem>                                                                <Deleted>n</Deleted>                                                                <Status><Active/></Status>                                                                <OnsetDate>11/01/2015</OnsetDate>                                                                <RcopiaID></RcopiaID>                                                                <ExternalID>testdx1000</ExternalID>                                                                <Patient>                                                                    <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>                                                                     <ExternalID>' + $externalid.text() + '</ExternalID>                                                                </Patient>                                                                <ICD9>                                                                                <Code>250.00</Code>                                                                                <Description>DIABETES MELLITUS WITHOUT MENTION OF COMPLICATION</Description>                                                                </ICD9>                                                </Problem>                                                <Problem>                                                                <Deleted>n</Deleted>                                                                <Status><Active/></Status>                                                                <OnsetDate>11/01/2015</OnsetDate>                                                                <RcopiaID></RcopiaID>                                                                <ExternalID>testdx1001</ExternalID>                                                                <Patient>                                                                    <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>                                                                     <ExternalID>' + $externalid.text() + '</ExternalID>                                                                </Patient>                                                                <ICD10>                                                                                <Code>I11.0</Code>                                                                                <Description>HYPERTENSIVE HEART DISEASE WITH HEART FAILURE</Description>                                                                </ICD10>                                                </Problem>                                                                                       <Problem>                                                                <Deleted>n</Deleted>                                                                <Status><Active/></Status>                                                                <OnsetDate>11/01/2015</OnsetDate>                                                                <RcopiaID></RcopiaID>                                                                <ExternalID>testdx1002</ExternalID>                                                                <Patient>                                                                                <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>                                                                                <ExternalID>' + $externalid.text() + '</ExternalID>                                                                </Patient>                                                                <SNOMED>                                                                                <ConceptID>111297002</ConceptID>                                                                                <Description>NONPARALYTIC STROKE</Description>                                                                </SNOMED>                                                </Problem>                                </ProblemList>                </Request></RCExtRequest>        ';
        //var getXML = "";
        //$.get('./resources/XmlFile1.xml', function (res) {


        //    var serializer = new XMLSerializer();
        //    getXML = serializer.serializeToString(res);
        //})
        var inputdata = '<?xml version="1.0" encoding="UTF-8"?><RCExtRequest>	<Caller>		<VendorName>vendorsh1100</VendorName>		<VendorPassword>b2xddjpn</VendorPassword>	</Caller>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername>	<Request>		<Command>get_url</Command>	</Request></RCExtRequest>';
        //var invocation = new XMLHttpRequest();
        //var url = 'https://ans2.drfirst.com/getURL?xml=' + inputdata;


        //if (invocation) {
        //    invocation.open('GET', url, true);
        //    invocation.onreadystatechange = handler;
        //    invocation.onload = function (res) {
        //        var text = res.responseText;
        //        alert(text);
        //        var title = getTitle(text);

        //    };

        //    invocation.onerror = function (E) {
        //        console.log(e.message)
        //        alert('Woops, there was an error making the request. exception=' + e.message);
        //    };
        //    invocation.send();
        //}






        //$.ajax({
        //    type: 'GET',
        //    url: 'https://ans2.drfirst.com/getURL?xml=' + inputdata,
        //    dataType: 'jsonp',
        //    beforeSend: function () {
        //        BackgroundLoaderShow(true);
        //    },
        //    success: function (response) {
        //        alert(response.responsetext);
        //    },
        //    error: function (response) {
        //        alert(JSON.stringify(response));
        //    }
        //});
    },
    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "PhysicalExamTemplate", null, false);
    },
    // Validate and save/edit functions

    ValidatePhysicalExamTemplate: function () {
        $('#frmPhysicalExamTemplate')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   // Start 27/11/2015 Muhammad Irfan Bug # 91,92

                   ProblemName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   // End 27/11/2015 Muhammad Irfan Bug # 91,92

                   //Color: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            PhysicalExamTemplate.PhysicalExamTemplateSave();
        });
    },
    //openDrFirst: function () {
    //    BackgroundLoaderShow(true);
    //    var params = [];
    //    params["StartupScreen"] = "patient";
    //    params["PatientId"] = PhysicalExamTemplate.params.patientID;
    //    params["FromAdmin"] = 0;
    //    params["ParentCtrl"] = PhysicalExamTemplate.params.ParentCtrl != "clinicalTabProgressNote" ? PhysicalExamTemplate.params.TabID : "PhysicalExamTemplate";
    //    LoadActionPan("DRFirst", params);
    //},

    PhysicalExamTemplateSave: function () {
        //Start//13/01/2016//Ahmad Raza//fixed bug#EMR-212
        $("#" + PhysicalExamTemplate.params.PanelID + " #frmPhysicalExamTemplate").bootstrapValidator('revalidateField', 'ProblemName');
        if ($("#" + PhysicalExamTemplate.params.PanelID + " #frmPhysicalExamTemplate #txtProblems").val() != "") {
            //End//13/01/2016//Ahmad Raza//fixed bug#EMR-212
            var strMessage = "";
            $("#" + PhysicalExamTemplate.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            var self = $("#" + PhysicalExamTemplate.params.PanelID + " #frmPhysicalExamTemplate");
            var myJSON = self.getMyJSONByName();
            //return false;
            if (PhysicalExamTemplate.params.mode == "Add") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented for privileges
                // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
                var hfProblemText = $("#" + PhysicalExamTemplate.params.PanelID + " #hfIMOProblem").val();
                var changesProblemText = $("#" + PhysicalExamTemplate.params.PanelID + " #txtProblems").val();
                // End 19/01/2016 Muhammad Irfan for bug # EMR-219
                if (hfProblemText.toString() == changesProblemText.toString()) {
                    AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            PhysicalExamTemplate.SavePhysicalExamTemplate(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    utility.DisplayMessages(response.message, 1);
                                    if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
                                        PhysicalExamTemplate.getPhysicalExamTemplateInfo(response.ProblemListId);
                                    }
                                    else {
                                        PhysicalExamTemplate.PhysicalExamTemplateSearch();
                                    }

                                    $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').resetAllControls(null);
                                    $("#" + PhysicalExamTemplate.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                                    // Start 27/11/2015 Muhammad Irfan Bug # 91,92
                                    $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                                    $("#" + PhysicalExamTemplate.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
                                    // End 27/11/2015 Muhammad Irfan Bug # 91,92

                                    //if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
                                    //    PhysicalExamTemplate.getPhysicalExamTemplateInfo(response.ProblemListId);
                                    //}
                                    //Start//17/12/2015//Ahmad Raza//Serialization of form
                                    $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').data('serialize', $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').serialize());
                                    //End//17/12/2015//Ahmad Raza//Serialization of form
                                }
                                else {
                                    //utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });
                } else {
                    utility.DisplayMessages("Please Enter Valid Problem", 3);
                    $("#" + PhysicalExamTemplate.params.PanelID + " #txtProblems").val('');
                    $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                }
                //End//21/12/2015//Ahmad Raza//Logic implemented for privileges
            }
            else if (PhysicalExamTemplate.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        PhysicalExamTemplate.UpdatePhysicalExamTemplate(myJSON, PhysicalExamTemplate.params.ProblemListId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                //PhysicalExamTemplate.PhysicalExamTemplateSearch();
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.displaymessages(strmessage, 2);
                });
            }
        }
    },

    SavePhysicalExamTemplate: function (PhysicalExamTemplateData) {

        var objData = JSON.parse(PhysicalExamTemplateData);
        if (objData.PatientId == '') {
            objData.PatientId = PhysicalExamTemplate.params.patientID;
        }
        objData["commandType"] = "SAVE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    NoKnownProblem: function () {
        var strMessage = "";
        var self = $("#" + PhysicalExamTemplate.params.PanelID + " #frmPhysicalExamTemplate");
        var myJSON = self.getMyJSONByName();
        PhysicalExamTemplate.SavePhysicalExamTemplate(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlPhysicalExamTemplate_Result #btnNoKnownProblems").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                PhysicalExamTemplate.PhysicalExamTemplateSearch();
                $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').resetAllControls(null);
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    callJs: function () {
        var objData = [];
        objData["commandType"] = "download_data";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "callJs");
    },
    UpdatePhysicalExamTemplate: function (PhysicalExamTemplateData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(PhysicalExamTemplateData);
        if (objData.PatientId == '') {
            objData.PatientId = PhysicalExamTemplate.params.patientID;
        }
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_VITALS";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    // End Validate and save/edit functions

    // Search/Grid Load Functions
    //Adding Pagination on 04 Dec 2015 by Azhar
    PhysicalExamTemplateSearch: function (ProblemListId, PageNo, rpp) {

        //var PanelProblemListGrid = "#pnlPhysicalExamTemplate #pnlPhysicalExamTemplate_Result";
        //var ProblemListGridId = "#pnlPhysicalExamTemplate #dgvPhysicalExamTemplate";
        ////$(ChargeGridId).dataTable().fnDestroy();
        //$(ProblemListGridId + " tbody tr").remove();
        //PhysicalExamTemplate.EditableGrid = EMRUtility.MakeEditableGrid(PanelProblemListGrid, ProblemListGridId, PhysicalExamTemplate, "0", false, false, false, false);

        var strMessage = "";

        if ($("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result").css("display") == "none") {
            $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result").show();
        }
        //Start//21/12/2015//Ahmad Raza//Implimented Privileges for ProblemList Search
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                PhysicalExamTemplate.SearchProblemList(ProblemListId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //Adding selection column of checkbox of Problem lists for Progress Notes on 04 Dec 2015 by Azhar
                        if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
                            if ($("#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate thead tr #SelectRecord").length == 0) {
                                $("#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate thead tr").prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderProblemsList" onchange="PhysicalExamTemplate.checkUncheckAllProblemsList(this);"   class="input-block" coltype="checkbox"/> </th>');
                            }

                        } else {
                            $("#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate th#SelectRecord").remove();
                        }
                        //PhysicalExamTemplate.ProblemListGridLoad(response);
                        PhysicalExamTemplate.ProblemListGridLoadNew(response);
                        //Adding Pagination on 04 Dec 2015 by Azhar
                        var TableControl = PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate";
                        var PagingPanelControlID = PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate_Paging";
                        var ClassControlName = "PhysicalExamTemplate";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ProblemListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            PhysicalExamTemplate.PhysicalExamTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                        setTimeout(function () {
                            if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + PhysicalExamTemplate.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").length == 0) {
                                $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="PhysicalExamTemplate.addPhysicalExamTemplateToNotes();" disabled id="btnAddProbListToNotes">Add on Note</button>').insertAfter("#" + PhysicalExamTemplate.params.PanelID + "  #dgvPhysicalExamTemplate_Paging .pagination")
                            }
                        }, 11);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//21/12/2015//Ahmad Raza//Implimented Privileges for ProblemList Search
    },
    //Start by Khaleel Ur Rehman to check uncheck all problem list by a checkBox in header. Date: 22 Jan 2016.
    checkUncheckAllProblemsList: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $("#" + PhysicalExamTemplate.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", true);
        } else {
            $("#" + PhysicalExamTemplate.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", false);
        }
        $("#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate tbody").find('input[type="checkbox"]').each(function () {
            PhysicalExamTemplate.enableAddProbList(this);
        });
    },
    //End by Khaleel Ur Rehman to check uncheck all problem list by a checkBox in header. Date: 22 Jan 2016.
    ProblemListGridLoad: function (response) {
        //$("#pnlPhysicalExamTemplate #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
        //$("#pnlPhysicalExamTemplate #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate tbody").find("tr").remove();
        if (response.ProblemListCount > 0) {
            PhysicalExamTemplate.EditableGrid.datatable.clear().draw();
            ////new
            //var actions = "";
            //$("#pnlPhysicalExamTemplate #dgvPhysicalExamTemplate tr th").each(function () {
            //    if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
            //        var arrActionType = [];
            //        if ($(this).attr("ActionType") != null) {
            //            arrActionType = $(this).attr("ActionType").split(',');
            //            actions = EditableGrid.GetActions(arrActionType);
            //        }
            //    }
            //});

            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);

            $.each(ProblemListLoadJSONData, function (i, item) {
                //var charge_detail = JSON.parse(item);
                var ProblemListId = item.ProblemListId;
                var CurrentRow = PhysicalExamTemplate.AddNewPhysicalExamTemplateRow(ProblemListId, "Edit", null);
                //var VisitChargesTable = $("#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate");
                var self = $("#dgvPhysicalExamTemplate tr#" + ProblemListId);
                //utility.bindMyJSON(true, item, false, VitalsLoadJSONData);
                utility.bindMyJSONByName(true, item, false, self).done(function () {
                    //$('#pnlPhysicalExamTemplate #btnsave').text('Update');
                    //PhysicalExamTemplate.params["VitalSignsId"] = VitalSignsId;
                    //PhysicalExamTemplate.params["mode"] = "Edit";
                });

                var row = PhysicalExamTemplate.EditableGrid.datatable.row(CurrentRow);

                /********************************/
                var newChildRow = row.child();

                /********************************/


                row.child().loadDropDowns(true).done(function () {
                    utility.bindMyJSON(true, item, false, $(newChildRow));
                    //    //serialize data
                    //    // $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());

                });
            });
        }
        else {
            $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate").css("display", "none");
            $('#dgvPhysicalExamTemplate').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },

    SearchProblemList: function (ProblemListId, PageNumber, RowsPerPage) {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = IsCheckedIn;
        objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = PhysicalExamTemplate.params.NotesId == null ? 0 : PhysicalExamTemplate.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    // Search/Grid Load Functions
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");
        //var PriorAuthorization = "<div class='col-xs-2'><label class='control-label'>Prior Auth. Number</label><input class='form-control' id='txtPriorAuthorization" + ParentRowId + "' name='txtPriorAuthorization" + ParentRowId + "' type='text' placeholder='999-99-9999'  /></div>";
        //var PriorAuthorization = "<div class='col-xs-2'><label class='control-label'>Prior Auth. Number</label> <input class='form-control' type='text' list='ddlPriorAuthorization" + ParentRowId + "' name='PriorAuthorization" + ParentRowId + "' id='txtPriorAuthorization" + ParentRowId + "'/> <datalist id='ddlPriorAuthorization" + ParentRowId + "' name='PriorAuthorization" + ParentRowId + "><option  value='- SELECT -'></option></datalist></div>";
        var txtProblem = "<div class='col-xs-1'><label class='control-label'>Problem</label><input  class='form-control'type='text' id='txtProblem" + ParentRowId + "' name='Problem" + ParentRowId + "' disabled></div>";
        var txtDiagnosis = "<div class='col-xs-1'><label class='control-label'>ICD-9 ICD-10 Description</label><input class='form-control' id='Diagnosis" + ParentRowId + "' name='Diagnosis" + ParentRowId + "' type='text' /></div>";
        var txtChronicityLevel = "<div class='col-xs-1'><label class='control-label'>Chronicity Level</label><input class='form-control' id='txtChronicity" + ParentRowId + "' name='Chronicity" + ParentRowId + "' type='text' /></div>";
        var txtSeverity = "<div class='col-xs-1  size-min100'><label class='control-label'>Severity</label><input class='form-control' id='txtSeverity" + ParentRowId + "' name='Severity" + ParentRowId + "' type='text' /></div>";
        var ddlNDCMeasurement = "<div class='col-xs-2'><label class='control-label'>NDC Measurement Code</label><select id='ddlNDCMeasurement" + ParentRowId + "' name='ddlNDCMeasurement" + ParentRowId + "' class='form-control' ddlist='GetNDCMeasurementCode'></select></div>";
        var LineNotes = "<div class='col-xs-2'><label class='control-label'>Line Notes</label><textarea class='form-control' rows='1' spellcheck='true' id='txtComments" + ParentRowId + "' name='txtComments" + ParentRowId + "'></textarea></div>";
        var chkHold = "<div class='col-xs-1 pt-lg'><div class='checkbox-custom checkbox-default'><input type='checkbox' onclick=EncounterChargeCapture.validateIsHold(this,'divHoldDays" + ParentRowId + "') id='chkHold" + ParentRowId + "' value name='chkHold" + ParentRowId + "'/><label class='control-label'>Is Hold</label></div></div>";
        var HoldDays = "<div id='divHoldDays" + ParentRowId + "' style='display:none' class='col-xs-1'><label class='control-label'>Hold Days</label><input type='text' class='form-control' onfocusout=EncounterChargeCapture.validateHoldDays(this,'chkHold" + ParentRowId + "') id='txtHoldDays" + ParentRowId + "' data-mask='9?99' name='txtHoldDays" + ParentRowId + "'/></div>";
        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(txtProblem, txtDiagnosis, txtChronicityLevel, txtSeverity, ddlNDCMeasurement, LineNotes, chkHold, HoldDays, spacer);
        return ChildHTML;

    },
    //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
    AddNewPhysicalExamTemplateRow: function (RowId, mode, CurrRef, NotesId) {

        var CurrentRow = null;
        if (RowId && RowId > 0) {

            CurrentRow = PhysicalExamTemplate.EditableGrid.rowAdd(RowId, PhysicalExamTemplate.params.VitalSignsId, null, null, null, null, NotesId);

        }
        else {
            var TemplateRow = $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = PhysicalExamTemplate.EditableGrid.rowAdd(TemplateRowId - 1, PhysicalExamTemplate.params.VitalSignsId, null, null, null, null, null);
            //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        }

        var row = PhysicalExamTemplate.EditableGrid.datatable.row(CurrentRow);
        row.child(PhysicalExamTemplate.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        PhysicalExamTemplate.enableRemoveRow($(CurrentRow));
        return CurrentRow;
    },
    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },
    // end editable grid functions

    // editabele grid functions

    rowSave: function ($row, obj) {

        //if (obj.rowValidate($row)) {

        var _self = obj,
        $actions,
        values = [];

        if ($row.hasClass('adding')) {
            $row.removeClass('adding');
        }

        values = $row.find('td').map(function () {

            var $this = $(this);

            if ($this.hasClass('expand')) {
                return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
            }
            else if ($this.hasClass('actions')) {

                return _self.datatable.cell(this).data();
            }
            else if ($this.hasClass('ddl')) {
                return $.trim($this.find('select').val());

            } else {
                $obj_ = $this.find('input');

                if ($obj_.attr('type') == "checkbox") {
                    if ($obj_.prop('checked'))
                        return $.trim("True");
                    else
                        return $.trim("False");
                }
                else
                    return $.trim($obj_.val());
            }
        });

        var id = $row.attr("id");

        var myJSON = $row.getMyJSONByName();
        //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
        var NotesId = $row.attr("problemlistnotesid");
        //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        if (id && id > 0) {

            //Edit Record
            var strMessage = "";
            //Start//22/12/2015//Ahmad Raza//Logic implemented for Privileges
            AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    PhysicalExamTemplate.UpdatePhysicalExamTemplateRow(myJSON, id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            //Serialization
                            //  $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').data('serialize', $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').serialize());

                            //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                            //Start//31/12/2015//Ahmad Raza//Logic to update against current Note only
                            if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote" && NotesId.indexOf(PhysicalExamTemplate.params.NotesId) > -1) {
                                //Ends//31/12/2015//Ahmad Raza//Logic to update against current Note only
                                PhysicalExamTemplate.getPhysicalExamTemplateInfo(id);
                            }
                            //End//31/12/2015//Ahmad Raza//Bug#178 fixed
                            utility.DisplayMessages(response.message, 1);
                            PhysicalExamTemplate.PhysicalExamTemplateSearch();
                            //PhysicalExamTemplate.rowDraw($row, _self, values);
                            //  PhysicalExamTemplate.PhysicalExamTemplateSearch();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
            //End//22/12/2015//Ahmad Raza//Logic implemented for Privileges
        }

    },




    rowDetail: function ($row, ClassName) {
        var currentVitalSignId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentVitalSignId > 0) {
            PhysicalExamTemplate.VitalsEdit(currentVitalSignId);
        }
    },

    rowAdd: function () {
        //Start//21/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//21/12/2015//Ahmad Raza//Privileges logic implemented
    },

    rowRemove: function ($row, obj) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        var strMessage = "";
        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        //Start//15/1/2016//M Ahmad Imran//DrFirst Integration Changes
                        var description;
                        if ($row.find("td:nth-child(4)").html() != "") {
                            description = $row.find("td:nth-child(4)").html();
                        }
                        else {
                            description = $row.find("td:nth-child(3)").html();
                        }
                        //End//15/1/2016//M Ahmad Imran//DrFirst Integration Changes
                        PhysicalExamTemplate.DeleteProblemList(selectedValue, description, $row.find("td:nth-child(7)").html()).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                            //Start//28/12/2015//Ahmad Raza// No Known ProblemList hyperlink(link not visible when there is no problem list) issue fixed
                            PhysicalExamTemplate.PhysicalExamTemplateSearch();
                            //End//28/12/2015//Ahmad Raza// No Known ProblemList hyperlink(link not visible when there is no problem list) issue fixed

                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented

    },

    rowInactive: function ($row, obj) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        var strMessage = "";
        var id = $row.attr("id");
        var IsActive = null;
        IsActive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');

        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var IsActiveRecord = null;
                        IsActiveRecord = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
                        if (IsActiveRecord == "1") {
                            var params = [];
                            var PanelID = "";

                            if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
                                params["ParentCtrl"] = 'PhysicalExamTemplate';
                                PanelID = 'pnlClinicalProgressNote #pnlPhysicalExamTemplate';
                            }
                            else if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabFaceSheet") {
                                params["ParentCtrl"] = 'PhysicalExamTemplate';
                                PanelID = 'pnlClinicalFaceSheet #pnlPhysicalExamTemplate';
                            }
                            else {
                                params["ParentCtrl"] = 'clinicalTabPhysicalExamTemplate';
                                PanelID = 'pnlPhysicalExamTemplate';
                            }
                            params["ProblemListId"] = selectedValue;
                            //Start//Ahmad Raza//15/12/2015//InActive PopUp issue in ProgressNote Resolved
                            params["FromAdmin"] = "0";
                            //params["ParentCtrl"] = "PhysicalExamTemplate";
                            //End//Ahmad Raza//15/12/2015//InActive PopUp issue in ProgressNote Resolved
                            params["PatientId"] = $('#PatientProfile #hfPatientId').val();
                            LoadActionPan('Clinical_ProblemListInActive', params, PanelID);
                        } else {
                            IsActiveRecord = "0";
                            PhysicalExamTemplate.InActiveProblemList(selectedValue, null, null).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    PhysicalExamTemplate.PhysicalExamTemplateSearch();
                                }
                                else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                            });
                        }




                    }
                }, function () { },
                    '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },

    InActiveProblemList: function (ProblemListId, comments, endDate) {

        var IsCheckedIn = null;
        IsCheckedIn = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        //     var ProblemListId = PhysicalExamTemplate.params.ProblemListId;
        var patientId = PhysicalExamTemplate.params.PatientId;

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["PatientId"] = patientId;
        objData["InActiveChkBoxValue"] = null;
        objData["InActiveReason"] = null;
        objData["EndDate"] = null;
        objData["IsActive"] = IsCheckedIn;
        objData["IsActiveRecord"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    rowCancel: function ($row, obj) {


        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    rowExpand: function ($row, obj) {
        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);

        //Start//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes
        if (PhysicalExamTemplate.params.ActionPanContainer == "actionPanClinicalProgressNote") {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:nth-child(2) .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            }
            else {
                $row.find("td:nth-child(2) .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
        else {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            }
            else {
                $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
        //End//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes


    },

    ShowHideEditableGridRows: function (isShow) {

        var VitalsGridId = "#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = PhysicalExamTemplate.EditableGrid.datatable.row(parentRow);

            if (isShow == true) {

                row.child.show();
                $(parentRow).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");

            }
            else {

                $(parentRow).find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();

            }

        });

    },

    // end editable grid functions
    UpdatePhysicalExamTemplateRow: function (ProblemListData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListData);
        objData["ProblemListId"] = ProblemListId;
        objData["Comments"] = $('#' + PhysicalExamTemplate.params.PanelID + ' #hfGridComments').val();
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    //
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = PhysicalExamTemplate.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var CurrentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.ProblemId);

                currentChildRow.attr("parentvitalid", ParentRowId);
                currentChildRow.addClass("childRow-bg");
                $(currentChildRow).find("td:nth-child(1)").html("");
                $(currentChildRow).find("td:nth-child(2)").html("");
                $(currentChildRow).find("td:nth-child(3)").html("");
                //var currentChild = Clinical_Vitals.FillCurrentRow(currentChildRow, item, row, true);
                CurrentRowchilds = CurrentRowchilds.add(currentChildRow);
            });
            row.child(CurrentRowchilds).show();
            setTimeout(function () {
                row.child.hide();
            }, 100);

        }
        else {
            $(CurrentRow).find("td:nth-child(1)").html("");
        }

        return row.child();
    },
    DeleteProblemList: function (ProblemListId, Description, StartDate) {

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["commandType"] = "DELETE_PROBLEMLIST";
        objData["PatientId"] = PhysicalExamTemplate.params.patientID;
        objData["Description"] = Description;
        objData["StartDate"] = StartDate;


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "ProblemList");

    },

    // problem list grid load as per admin

    ProblemListGridLoadNew: function (response) {

        //Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search

        $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate").dataTable().fnClearTable();
        $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate tbody").find("tr").remove();


        //    if ($.fn.dataTable.isDataTable("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate")) {
        //        $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
        //    }

        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        var isactive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');



        if (response.ProblemListCount > 0) {
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);
            // get Actions
            var actions = "";
            $("#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EMREditableGrid.GetActions(arrActionType);
                    }
                }
            });
            if ($.fn.dataTable.isDataTable("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate")) {
                $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
            }
            //tem array to hold rows and childs
            var arraTemp = [];

            $.each(ProblemListLoadJSONData, function (i, item) {



                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.ProblemListId);
                //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                $row.attr("ProblemListNotesId", item.NoteId);
                //End//31/12/2015//Ahmad Raza//Bug#178 fixed
                var color = "";

                // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
                if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent") {
                    color = 'style = "color:green;font-weight:bold"'
                }
                if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity") {
                    color = 'style = "color:red;font-weight:bold"'
                }
                if (item.Severity == "Moderate Persistent") {
                    color = 'style = "color:orange;font-weight:bold"'
                }

                var comments = "";

                if (item.Comments != "") {
                    var commentsMethod = "PhysicalExamTemplate.AddComments('" + item.ProblemListId + "');";
                    //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                    comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                }
                var ProblemListId = item.ProblemListId;
                var ChildHistory_ProblemList = $.grep(ProblemListHistoryLoadJSONData, function (n, i) {
                    return n.ProblemId == ProblemListId;
                });
                if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote" && item.ProblemName == "No Known Problems") {
                    //$row.append('<td></td><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    //Start//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                    var SelectionCheckBoxColumn = "";
                    var Checked = "";
                    if (item.IsNoteLinked == "True") {
                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                            Checked = " ";
                        } else {
                            Checked = " checked";
                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                    }

                    if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="PhysicalExamTemplate.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                    } else {
                        SelectionCheckBoxColumn = "";
                    }
                    if (ChildHistory_ProblemList.length > 0) {
                        $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    } else {
                        $row.append(SelectionCheckBoxColumn + '<td></td><td></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    }
                    //End//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                }
                else {


                    if (item.ProblemName == "No Known Problems") {
                        $row.append('<td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    }
                    else {
                        //adding checkboxes column and disabling that row, if problem list already binded with notes
                        var SelectionCheckBoxColumn = "";
                        var Checked = "";
                        if (item.IsNoteLinked == "True") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                        } else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                        }

                        if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                            SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" onchange="PhysicalExamTemplate.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        } else {
                            SelectionCheckBoxColumn = "";
                        }
                        if (ChildHistory_ProblemList.length > 0) {
                            $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                        } else {
                            $row.append(SelectionCheckBoxColumn + '<td></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                        }
                    }
                }
                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }


                $("#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate tbody").last().append($row);

                var CurrentRowchilds = $();

                if (ChildHistory_ProblemList.length > 0) {
                    $.each(ChildHistory_ProblemList, function (i, item) {
                        // if (item.ProblemId == ProblemListId) {
                        //arrProblemListHistory.push(item);
                        if (item.Comments != "") {
                            var commentsMethod = "PhysicalExamTemplate.AddComments('" + item.ProblemListId + "');";
                            //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                            comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                        }
                        else {
                            comments = "";
                        }
                        var Title_Tooltip = "Inactive Reason: " + item.InActiveChkBoxValue + (item.EndDate != '' ? " <br/>End Date: " + utility.RemoveTimeFromDate(null, item.EndDate) : "") + (item.InActiveReason != '' ? " <br/>Comments: " + item.InActiveReason : "");
                        var IsActiveText = "";
                        if (item.IsActive == "True") {
                            IsActiveText = "<Label>[Active]</Label>";
                        } else {
                            IsActiveText = "<Label data-toggle='tooltip' data-placement='right' title='" + Title_Tooltip + "'>[Inactive]</Label>";
                        }

                        // Start 27/11/2015 Muhammad Irfan Change color of severity

                        var colorChild = "";

                        // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
                        if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent") {
                            colorChild = 'style = "color:green;font-weight:bold"'
                        }
                        if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity") {
                            colorChild = 'style = "color:red;font-weight:bold"'
                        }
                        if (item.Severity == "Moderate Persistent") {
                            colorChild = 'style = "color:orange;font-weight:bold"'
                        }

                        // End 27/11/2015 Muhammad Irfan Change color of severity

                        //Start//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed
                        if (PhysicalExamTemplate.params.ActionPanContainer == "actionPanClinicalProgressNote") {
                            var currentHistory = '<tr class="childRow-bg"><td></td><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + IsActiveText + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + colorChild + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td></tr>';

                        }
                        else {

                            var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + IsActiveText + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + colorChild + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td></tr>';
                        }
                        //End//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed

                        //arrProblemListHistory.push(currentHistory);
                        CurrentRowchilds = CurrentRowchilds.add(currentHistory);

                        // }
                    });
                }

                if (CurrentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: CurrentRowchilds });
            });

            //Inalize grid
            var PanelGrid = "#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result";
            var GridId = "#" + PhysicalExamTemplate.params.PanelID + " #dgvPhysicalExamTemplate";

            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search
            if (PhysicalExamTemplate.myGrid != null) {
                //  PhysicalExamTemplate.myGrid.$table.find("tbody tr").remove();
                // PhysicalExamTemplate.myGrid.$table.dataTable().fnClearTable()
                PhysicalExamTemplate.myGrid.$table.dataTable().fnDestroy();

                //  PhysicalExamTemplate.myGrid.datatable.clear().draw();
                $("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
            }
            //End By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

            //   if ($.fn.dataTable.isDataTable("#" + PhysicalExamTemplate.params.PanelID + " #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate") == false) {
            PhysicalExamTemplate.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, PhysicalExamTemplate, 0, false, true, false, true, false, null);
            //  $("#" + PhysicalExamTemplate.params.PanelID + " div.mystuff").attr('id', 'divSwitch');
            //   }

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (PhysicalExamTemplate.myGrid != null) {
                    var row = PhysicalExamTemplate.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            //Start//04//01//2015//Ahmad Raza//Sorting removed from first column of grid
            // $('#dgvPhysicalExamTemplate').dataTable().fnSettings().aoColumns[0].bSortable = false;
            $('#' + PhysicalExamTemplate.params.PanelID + ' #dgvPhysicalExamTemplate').dataTable().fnSettings().aoColumns[0].bSortable = false;
            //End//04//01//2015//Ahmad Raza//Sorting removed from first column of grid


            /* Start of Code for making No Known Problem List hyperlink inline with checkbox and search box.
             *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
             */
            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }

            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="">' +
                          '</div><span class="pl-xs">Active</span>';

            $("#" + PhysicalExamTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            //  if ($('#pnlPhysicalExamTemplate #switchActive').is(':checked') || $("#pnlPhysicalExamTemplate_Result #btnNoKnownProblems").is(':visible')) {



            $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #dgvPhysicalExamTemplate').DataTable({
                "language": {
                    "emptyTable": "No Record Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                                     '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="">' +
                                      '</div><span class="pl-xs">Active</span>';

            $("#" + PhysicalExamTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            if (response.ProblemListTotalCount == 0) {
                $("#pnlPhysicalExamTemplate_Result #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlPhysicalExamTemplate_Result #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */
        }


        EMRUtility.SwicthWidgetInializatoin();
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Start//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        // $('#dgvPhysicalExamTemplate thead tr th:first-child').removeClass('sorting_asc');
        $('#' + PhysicalExamTemplate.params.PanelID + ' #dgvPhysicalExamTemplate thead tr th:first-child').removeClass('sorting_asc');
        //End//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        //Editable Grid
        //var PanelGrid = "#pnlPhysicalExamTemplate #pnlPhysicalExamTemplate_Result";
        //var GridId = "#pnlPhysicalExamTemplate #dgvPhysicalExamTemplate";
        //EMRUtility.MakeEditableGrid(PanelGrid, GridId, PhysicalExamTemplate, 0);

    },

    //
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate', function () {
                if (PhysicalExamTemplate.params["FromAdmin"] == "0") {
                    if (PhysicalExamTemplate.params != null && PhysicalExamTemplate.params.ParentCtrl != null) {
                        UnloadActionPan(PhysicalExamTemplate.params.ParentCtrl, 'PhysicalExamTemplate');
                    }
                    else
                        UnloadActionPan(null, 'PhysicalExamTemplate');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_PhysicalExamTemplate").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (PhysicalExamTemplate.params["FromAdmin"] == "0") {
                    if (PhysicalExamTemplate.params != null && PhysicalExamTemplate.params.ParentCtrl != null) {
                        UnloadActionPan(PhysicalExamTemplate.params.ParentCtrl, 'PhysicalExamTemplate');
                    }
                    else
                        UnloadActionPan(null, 'PhysicalExamTemplate');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_PhysicalExamTemplate").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (PhysicalExamTemplate.params["FromAdmin"] == "0") {
                if (PhysicalExamTemplate.params != null && PhysicalExamTemplate.params.ParentCtrl != null) {
                    UnloadActionPan(PhysicalExamTemplate.params.ParentCtrl, 'PhysicalExamTemplate');
                }
                else
                    UnloadActionPan(null, 'PhysicalExamTemplate');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_PhysicalExamTemplate").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },

    //Comments Update

    AddComments: function (ProblemListId) {


        var params = [];
        var PanelID = "";
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
        if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'PhysicalExamTemplate';
            PanelID = 'pnlClinicalProgressNote #pnlPhysicalExamTemplate';
        }
            // Start By Babur on 2/15/2016 - Bug EMR-329 - FaceSheet in clinical module -> Problem list -> Edit comments
        else if (PhysicalExamTemplate.params.ParentCtrl == "clinicalTabFaceSheet") {
            params["ParentCtrl"] = 'PhysicalExamTemplate';
            PanelID = 'pnlClinicalFaceSheet #pnlPhysicalExamTemplate';
        }
            // End By Babur on 2/15/2016 - Bug EMR-329 - FaceSheet in clinical module -> Problem list -> Edit comments
        else {
            params["ParentCtrl"] = 'clinicalTabPhysicalExamTemplate';
            PanelID = 'pnlPhysicalExamTemplate';
        }

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */

        params["ProblemListId"] = ProblemListId;
        //Start//Ahmad Raza//15/12/2015//Comments Popup issue in ProgressNote, Resolved
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "PhysicalExamTemplate";
        //End//Ahmad Raza//15/12/2015//Comments Popup issue in ProgressNote, Resolved
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        LoadActionPan('PhysicalExamTemplateComments', params, PanelID);
    },

    //

    ActiveProblemSearch: function (objThis) {


        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        PhysicalExamTemplate.PhysicalExamTemplateSearch();
    },

    // Start 27/11/2015 Muhammad Irfan Bug # 93,94
    ResetDiagnosis: function () {

        if ($("#" + PhysicalExamTemplate.params.PanelID + " #txtProblems").val() == "") {
            $('#' + PhysicalExamTemplate.params.PanelID + ' #frmPhysicalExamTemplate').resetAllControls(null);
            $("#" + PhysicalExamTemplate.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
        }
    },
    // End 27/11/2015 Muhammad Irfan Bug # 93,94

    //-----------------Progress Note-------------
    // added on Dec 04,2015 by Muhammad Azhar Shahzad
    //Call Back function to add component to Progress Note
    addPhysicalExamTemplateToNotes: function () {
        //var SelectedPhysicalExamTemplate = $("#" + PhysicalExamTemplate.params.PanelID + ' [name=SelectCheckBoxProbList]:checked:not(:disabled)').map(function () { return this.id; }).get().join();
        //if (SelectedPhysicalExamTemplate != null && SelectedPhysicalExamTemplate != '') {
        //    PhysicalExamTemplate.getPhysicalExamTemplateInfo(SelectedPhysicalExamTemplate);
        //}
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        var strMessage = "";
        //Start//22/12/2015//Ahmad Raza//Logic implemented for Privileges
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var SelectedPhysicalExamTemplate = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                if (SelectedPhysicalExamTemplate != null && SelectedPhysicalExamTemplate != '') {
                    for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                        var PLid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ProblemList_Main' + PLid).length != 0) {
                            var index = SelectedPhysicalExamTemplate.indexOf(PLid);
                            if (index > -1) {
                                SelectedPhysicalExamTemplate.splice(index, 1);
                            }
                        }
                    }
                    if (SelectedPhysicalExamTemplate.join() != null && SelectedPhysicalExamTemplate.join() != '') {
                        PhysicalExamTemplate.getPhysicalExamTemplateInfo(SelectedPhysicalExamTemplate.join());
                    }
                }
                var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
                if (detachedvalues.join() != '') {

                    PhysicalExamTemplate.detachProblemsListFromNotes_DBCall(detachedvalues.join()).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            for (var i = 0; i < detachedvalues.length; i++) {
                                var PLid = detachedvalues[i];
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ProblemList_Main' + PLid).remove();
                            }

                            Clinical_ProgressNote.updateProgressNoteHTML();
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                            if (PhysicalExamTemplate.params != null && PhysicalExamTemplate.params.PanelID.indexOf('pnlPhysicalExamTemplate') != -1) {
                                PhysicalExamTemplate.PhysicalExamTemplateSearch();
                            }
                            //   utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //When User has attached Allergies with notes than add on note button should be disabled
                $("#" + PhysicalExamTemplate.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").prop('disabled', true);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    //this function will get Problem Lists Soap Text and attach that to Progress note
    getPhysicalExamTemplateInfo: function (PhysicalExamTemplateId) {
        if (PhysicalExamTemplateId == null || PhysicalExamTemplateId == '') {
            return false;
        }

        PhysicalExamTemplate.get_PhysicalExamTemplate_ForSOAP(PhysicalExamTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                if (response.PhysicalExamTemplateoapCount > 0) {

                    //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamTemplate').parent().parent().find('#Cli_ProblemList_Main' + PhysicalExamTemplateId).length == 0) {
                    //    PhysicalExamTemplate.detachProblemListFromNotes('ProblemList', false, false, PhysicalExamTemplateId);
                    //}
                    //Start//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    PhysicalExamTemplate.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', PhysicalExamTemplateId);
                    //End//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (PhysicalExamTemplate.params != null && PhysicalExamTemplate.params.PanelID.indexOf('pnlPhysicalExamTemplate') != -1) {
                        PhysicalExamTemplate.PhysicalExamTemplateSearch();
                    }

                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //This Function will check, if Problem Lists Soap is already attached in Progress note, if Problem Lists is not attached than it will create main divs to attach allergy
    checkProblemListExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamTemplate').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="PhysicalExamTemplateComponent"> <header>' +
                        '<PhysicalExamTemplate title="ProblemsList"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'PhysicalExamTemplate\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="PhysicalExamTemplate">Problem Lists</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'PhysicalExamTemplate\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</PhysicalExamTemplate> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createProblemListBodyHTML: function (response, NoteHTMLCtrl, PhysicalExamTemplateId) {
        PhysicalExamTemplate.checkProblemListExists();
        var PhysicalExamTemplateoap_JSON = JSON.parse(response.PhysicalExamTemplateoap_JSON);
        var $mainDivVital = $(document.createElement('div'));
        if (PhysicalExamTemplateoap_JSON == null || PhysicalExamTemplateoap_JSON.length == 0) {
            return "";
        }
        var PListId = [];
        $.each(PhysicalExamTemplateoap_JSON, function (index, element) {
            var color = "";

            // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
            if (element.Severity == "Mild Intermittent" || element.Severity == "Mild Persistent") {
                color = 'style = "color:green;font-weight:bold" ';
            }
            if (element.Severity == "Severe Persistent" || element.Severity == "Unspecified Severity") {
                color = 'style = "color:red;font-weight:bold" ';
            }
            if (element.Severity == "Moderate Persistent") {
                color = 'style = "color:orange;font-weight:bold" ';
            }

            var PLid = element.ProblemListId;
            var $SectionBodyVital = $(document.createElement('section'));
            $SectionBodyVital.attr('id', "Cli_ProblemList_Main" + PLid);
            var $DetailsDiv = $(document.createElement('div'));
            $DetailsDiv.attr('id', "Cli_ProblemList_" + PLid);
            var $ListVital = $(document.createElement('ul'));

            $ListVital.attr('class', 'list-unstyled')

            $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ProblemList_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ProblemList_Main" + PLid + '"  ><i class="fa fa-times"></i></a></div> ');


            $ListVital.append("<li><strong> " + element.ProblemName + " </strong>" + (element.Severity == '' ? "" : "(<span " + color + '>' + element.Severity + "</span>) ") +
                (element.ChronicityLevel == '' ? "" : ", " + element.ChronicityLevel) +
                (element.Description == '' ? "" : ", " + element.Description) +
                (element.StartDate == '' ? "" : ", Start on " +
                 utility.RemoveTimeFromDate(null, element.StartDate)) +
                (element.ModifiedOn == '' ? "" : " and modified on " +
               utility.RemoveTimeFromDate(null, element.ModifiedOn))
                );

            $ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
            $DetailsDiv.append($ListVital);
            $SectionBodyVital.append($DetailsDiv);
            //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_ProblemList').parent().parent().find('#Cli_ProblemList_Main' + PLid).length == 0) {
            if ($(NoteHTMLCtrl + ' PhysicalExamTemplate').parent().parent().find('#Cli_ProblemList_Main' + PLid).length == 0) {
                PListId.push(PLid);
                $mainDivVital.append($SectionBodyVital);
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' PhysicalExamTemplate').parent().parent().find('#Cli_ProblemList_Main' + PLid + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' PhysicalExamTemplate').parent().parent().find('#Cli_ProblemList_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' PhysicalExamTemplate').parent().parent().find('#Cli_ProblemList_Main' + PLid).html($SectionBodyVital.html());
                $(NoteHTMLCtrl + ' PhysicalExamTemplate').parent().parent().find('#Cli_ProblemList_Main' + PLid + ' ul').append(CommentHTML);;
            }

        });
        //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
        if (PListId.join(",") != "") {
            PhysicalExamTemplateId = PListId.join(",");
        }
        //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
        if ($mainDivVital.html() != '') {
            //Start//04//01//2015//Ahmad Raza//
            PhysicalExamTemplate.updateProblemListHtml($mainDivVital.html(), PhysicalExamTemplateId, NoteHTMLCtrl);
        } else {
            PhysicalExamTemplate.updateProblemListHtml('', PhysicalExamTemplateId, NoteHTMLCtrl);
            Clinical_ProgressNote.updateProgressNoteHTML();
        }

    },

    // This Function is called by Progress Notes (Fill Vitals Func, CopyAllNotesCategories)
    updateProblemListHtml: function (ProblemListHtml, ProblemListId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' PhysicalExamTemplate').parent().parent().addClass('initialVisitBody');
        if (ProblemListHtml != '') {
            $(NoteHTMLCtrl + ' PhysicalExamTemplate').parent().parent().append(ProblemListHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ProblemListHtml != '') {
            PhysicalExamTemplate.attachProblemListignFromNotes(ProblemListId);
        }

    },

    //This Functions ask for Detaching Vital sign from Progress Note for current Patient Selected
    detachProblemListFromNotes: function (ProblemsListId) {
        var strMessage = "";
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ProblemsListId.replace('Cli_ProblemList_Main', '');
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        PhysicalExamTemplate.detachProblemsListFromNotes_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $('#' + ProblemsListId).remove();
                                Clinical_ProgressNote.updateProgressNoteHTML();
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
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    //This Function detach Problem list From progress note
    detach_ComponentsProblemList: function (ComponentName, IsUpdate, ProblemListComponentRemove) {
        // var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Medical_Problems List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //  if (strMessage == "") {
        var Clinical_ProblemListIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamTemplate').parent().parent().find('section[id*="Cli_ProblemList_Main"]').map(function () {
            return this.id.replace("Cli_ProblemList_Main", "");
        }).get().join(',');
        if (Clinical_ProblemListIds == "" || Clinical_ProblemListIds == "undefined") {
        }
        else {


            PhysicalExamTemplate.detachProblemsListFromNotes_DBCall(Clinical_ProblemListIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.updateProgressNoteHTML();
                    }
                    //    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (ProblemListComponentRemove) {
            //Start//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Problem Lists']").remove();
            //End//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamTemplate').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamTemplate').parent().parent().find('section[id*="Cli_ProblemList_Main"]').remove();
        }
        // }
        // else
        //      utility.DisplayMessages(strMessage, 2);
        //});
    },

    //This Functions attached Vital sign to Progress Note for current Patient Selected
    attachProblemListignFromNotes: function (ProblemsListId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Medical_Problems List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = ProblemsListId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                PhysicalExamTemplate.attachProblemsListWithNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //var attachedVitals = JSON.parse(response.VitalsLoad_JSON);

                        //If Attached Vitals Made new inseration to Vitals Table than good ids should be attached to HTML
                        //  Clinical_ProgressNote.ChangeHTML(response);
                        Clinical_ProgressNote.updateProgressNoteHTML();
                        $('#' + ProblemsListId).remove();

                        // utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }


        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //This Function enable/disable add to note button
    enableAddProbList: function (obj) {
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
            $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlPhysicalExamTemplate_Result #chkHeaderProblemsList').prop('checked', false);
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
            $("#" + PhysicalExamTemplate.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").prop('disabled', false);
        } else {
            $("#" + PhysicalExamTemplate.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").prop('disabled', true);
        }

    },

    //If PhysicalExamTemplate Component which is dropeed in Progress note has no ProblemList attached, than it will call for Latest ProblemList for this patient
    getLatestProblemListByPatientId: function () {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            PhysicalExamTemplate.getLatestProblemListByPatientId_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.PhysicalExamTemplateoapCount > 0) {
                        PhysicalExamTemplate.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                    }
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },

    getRcopiaInformaionForPhysicalExamTemplateoap: function () {
        PhysicalExamTemplate.getLatestProblemListByPatientId();
    },
    //-----Server calls of Notes----------
    attachProblemsListWithNotes_DBCall: function (ProblemsListId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProblemListId"] = ProblemsListId;
        objData["commandType"] = "attach_PhysicalExamTemplate_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    detachProblemsListFromNotes_DBCall: function (ProblemsListId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProblemListId"] = ProblemsListId;
        objData["commandType"] = "detach_PhysicalExamTemplate_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    get_PhysicalExamTemplate_ForSOAP: function (PhysicalExamTemplateId) {

        var objData = new Object();
        objData["ProblemListId"] = PhysicalExamTemplateId;
        objData["PatientId"] = PhysicalExamTemplate.params.patientID;
        objData["commandType"] = "get_PhysicalExamTemplate_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    getLatestProblemListByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_problemlistby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },



    //--------------end progress Note-----------
}