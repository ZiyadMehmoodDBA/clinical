ROSTemplateRevamp = {
    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This file will handle all actions performed for PhysicalExam Template
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,

    //Author: Muhammad Arshad
    //Date: 24-02-2016
    //This function will handle fill of ROSTemplateRevamp
    Load: function (params) {
        ROSTemplateRevamp.params = params;

        ROSTemplateRevamp.params.mode = "Add";

        if (ROSTemplateRevamp.params.PanelID != 'pnlROSTemplateRevamp') {
            ROSTemplateRevamp.params.PanelID = ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp';
        } else {
            ROSTemplateRevamp.params.PanelID = 'pnlROSTemplateRevamp';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + ROSTemplateRevamp.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
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


        var self = $('#' + ROSTemplateRevamp.params.PanelID);
        //Start 08-03-2016 Farooq Ahmad to load Physical Exam Templates
        if (ROSTemplateRevamp.bIsFirstLoad == true) {
            ROSTemplateRevamp.loadROSTemplateMK();
        }
        //End 08-03-2016 Farooq Ahmad to load Physical Exam Templates
        if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(ROSTemplateRevamp.params.PanelID, 'Medical', 'ROSTemplateRevamp', 'ROSTemplateRevamp.UnLoadTab();', 'frmPhysicalExamTemplate');
        }
        if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + ROSTemplateRevamp.params.PanelID + " div#FaceSheetPager", ROSTemplateRevamp.params.FaceSheetComponents, 'problem list');
        }

        //Start//24-02-2016//Ahmad Raza//empty record label and search box shown in grid
        $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlProvider_Result #dgvProvider').DataTable({
            "language": {
                "emptyTable": "No Record Found."
            }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
        });
        //End//24-02-2016//Ahmad Raza//empty record label and search box shown in grid

        //Start//24-02-2016//Ahmad Raza//Toggle switch logic
        var HtmlOfSwitch = '<div id="divSwitch"><span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="1" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="">' +
                          '</div><span class="pl-xs">Active</span> </div>';

        $("#" + ROSTemplateRevamp.params.PanelID + ' .datatables-header div:first').html(HtmlOfSwitch);
        ROSTemplateRevamp.readyFunction();
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
    //This function will handle Add/Edit of ROSTemplateRevamp based on given PhysicalExamTemplateId
    ROSTemplateAddEdit: function (PhysicalExamTemplateId) {
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
                params["FromAdmin"] = ROSTemplateRevamp.params["FromAdmin"];
                params["ParentCtrl"] = 'ROSTemplateRevamp';
                LoadActionPan('ROSTemplateDetailRevamp', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    // MK

    ROSTemplateRevampAddEditMK: function (ROSTemplateRevampId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (ROSTemplateRevampId != null && parseInt(ROSTemplateRevampId) > 0) {
                    params["ROSTemplateRevampId"] = ROSTemplateRevampId;
                    params["mode"] = "Edit";
                }
                else {
                    params["ROSTemplateRevampId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = ROSTemplateRevamp.params["FromAdmin"];
                params["ParentCtrl"] = 'ROSTemplateRevamp';
                LoadActionPan('ROSTemplateDetailRevamp', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);ROSTemplateRevamp
        });
    },

    loadROSTemplateMK: function () {
        ROSTemplateRevamp.ROSTemplateLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ROSTemplateRevamp.ROSTemplateGridLoadMK(response);
                var TableControl = ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates";
                var PagingPanelControlID = ROSTemplateRevamp.params.PanelID + " #dgvPhysExamTemplates_Paging";
                var ClassControlName = "ROSTemplateRevamp";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                //setTimeout(
                //    CreatePagination(response.PhysExamTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                //        ROSTemplateRevamp.PhysExamTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
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
    loadROSTemplate: function () {
        ROSTemplateRevamp.ROSTemplateLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ROSTemplateRevamp.ROSTemplateGridLoad(response);
                var TableControl = ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates";
                var PagingPanelControlID = ROSTemplateRevamp.params.PanelID + " #dgvPhysExamTemplates_Paging";
                var ClassControlName = "ROSTemplateRevamp";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                //setTimeout(
                //    CreatePagination(response.PhysExamTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                //        ROSTemplateRevamp.PhysExamTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
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
    ROSTemplateLoad: function () {
        var objData = new Object();
        var IsActive = null;
        IsActive = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');
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
        objData["commandType"] = "Load_ROSRevamp_Template";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSRevampTemplate");
    },

    //Author: Farooq Ahmad
    //Date: 08/03/2016
    //Description: This function will bind the physical Exam template Grid
    ROSTemplateGridLoad: function (response) {
        var isactive = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');
        try {
            $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        } catch (ex) {
            console.log(ex);
        }
        $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.PhysicalExamTemplateCount > 0) {
            var PhysicalExamTemplateJSONData = JSON.parse(response.ROSTemplateRevamp); //Parsing array to JSON

            $.each(PhysicalExamTemplateJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "ROSTemplateRevamp.ROSTemplateEdit('" + item.TemplateId + "'," + item.IsDefault.toLowerCase() + ",event);");
                $row.attr("id", "dgvPhysExamTemplates" + item.TemplateId);
                $row.attr("TemplateId", item.TemplateId);

                var IsDefault = false;

                var editCall = "ROSTemplateRevamp.ROSTemplateEdit('" + item.TemplateId + "'," + IsDefault + ",event);";
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

                $row.append('<td class="hidden">' + dateInNumberFormat + '</td><td style="display:none;">' + item.TemplateId + '</td><td><a class="btn btn-xs" href="#" onclick="ROSTemplateRevamp.ROSTemplateDelete(\'' + item.TemplateId + '\',' + IsDefault + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ROSTemplateRevamp.ROSTemplateEdit(\'' + item.TemplateId + '\',' + IsDefault + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ROSTemplateRevamp.ROSTemplateActiveInactive(\'' + item.TemplateId + '\',' + IsDefault + ', ' + isEventactive + ',event);"><i class="' + tglclass + '"></i></a></td><td>' + item.PhysicalExamTemplateName + '</td><td>' + item.LastUpdated + '</td>');

                $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates tbody").last().append($row);
            });
        }
        else {
            $("#" + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates').DataTable({
                "language": {
                    "emptyTable": "No ROS Template Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[0, "desc"]]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates'))
            ;
        else {
            $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
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
                       '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="ROSTemplateRevamp.activeROSTemplateSearch(this);">' +
                        '</div><span class="pl-xs">Active</span>';

        $("#" + ROSTemplateRevamp.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();

    },

    ROSTemplateGridLoadMK: function (response) {
        var isactivebtn = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');
        try {
            $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        } catch (ex) {
            console.log(ex);
        }
        $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.PhysicalExamTemplateCount > 0) {
            var PhysicalExamTemplateJSONData = JSON.parse(response.PhysicalExamTemplate); //Parsing array to JSON
            PhysicalExamTemplateJSONData = JSON.parse(PhysicalExamTemplateJSONData)
            $.each(PhysicalExamTemplateJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "ROSTemplateRevamp.ROSTemplateEdit('" + item.TemplateId + "'," + item.IsDefault.toLowerCase() + ",event);");
                $row.attr("id", "dgvPhysExamTemplates" + item.ROSTemplateId);
                $row.attr("TemplateId", item.ROSTemplateId);
                var IsDefault = false;

                var editCall = "ROSTemplateRevamp.ROSTemplateEdit('" + item.ROSTemplateId + "'," + IsDefault + ",event);";
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

                $row.append('<td class="hidden">' + dateInNumberFormat + '</td><td style="display:none;">' + item.ROSTemplateId + '</td><td><a class="btn btn-xs" href="#" onclick="ROSTemplateRevamp.ROSTemplateDelete(\'' + item.ROSTemplateId + '\',' + IsDefault + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ROSTemplateRevamp.ROSTemplateEdit(\'' + item.ROSTemplateId + '\',' + IsDefault + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ROSTemplateRevamp.ROSTemplateActiveInactive(\'' + item.ROSTemplateId + '\',' + IsDefault + ', ' + isEventactive + ',event);"><i class="' + tglclass + '"></i></a></td><td>' + item.Name + '</td><td>' + item.ModifiedOn + '</td>');

                $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates tbody").last().append($row);
            });
        }
        else {
            $("#" + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates').DataTable({
                "language": {
                    "emptyTable": "No ROS Template Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[0, "desc"]]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates'))
            ;
        else {
            $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
        var checked = '';
        if (isactivebtn == "0" || isactivebtn == 0) {
        } else if (isactivebtn == null) {
            isactivebtn = "1";
            checked = 'checked="checked"';
        } else {
            isactivebtn = "1";
            checked = 'checked="checked"';
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                       '<input id="switchActive" isactive="' + isactivebtn + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="ROSTemplateRevamp.activeROSTemplateSearch(this);">' +
                        '</div><span class="pl-xs">Active</span>';

        $("#" + ROSTemplateRevamp.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();

    },


    //Author: Azhar Shahzad
    //Date October 06, 2016
    //Toggle Active Inactive record
    activeROSTemplateSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        ROSTemplateRevamp.loadROSTemplateMK();
    },

    ROSTemplateEdit: function (TemplateId, IsDefault, event) {
        if (IsDefault == false) {
            ROSTemplateRevamp.ROSTemplateAddEdit(TemplateId);
        }
        else {
            utility.DisplayMessages("You can not edit default template.", 3);
        }
        event.stopPropagation();
    },

    ROSTemplateDelete: function (TemplateId, IsDefault, event) {
        if (IsDefault == false) {

            utility.myConfirm('Are you sure to delete template?', function () {
                ROSTemplateRevamp.DeleteROSTemplate(TemplateId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        ROSTemplateRevamp.loadROSTemplateMK();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
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

    ROSTemplateActiveInactive: function (TemplateId, IsDefault, isactive, event) {
        if (IsDefault == false) {
            utility.myConfirm('3', function () {
                if (TemplateId == "" || TemplateId == "undefined") {
                }
                else {
                    ROSTemplateRevamp.updateROSTemplateActiveInactive_Dbcall(TemplateId, isactive).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            ROSTemplateRevamp.loadROSTemplateMK();
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
    updateROSTemplateActiveInactive_Dbcall: function (TemplateId, IsActive) {
        var objData = {};
        objData["TemplateId"] = TemplateId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "activeinactive_reviewofsystem_template";
        var data = JSON.stringify(objData);
        //  var data = "NotesId=" + NotesId + "&IsActive=" + IsActive;
        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
    },


    DeleteROSTemplate: function (TemplateId) {
        var objData = new Object();
        objData["TemplateId"] = TemplateId;
        objData["commandType"] = "delete_reviewofsystem_template"; //DELETE_PhysicalExamTemplate
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
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
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ROSTemplateRevamp", null, false);
    },
    // Validate and save/edit functions

    ValidateROSTemplate: function () {
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
            ROSTemplateRevamp.ROSTemplateSave();
        });
    },
    //openDrFirst: function () {
    //    BackgroundLoaderShow(true);
    //    var params = [];
    //    params["StartupScreen"] = "patient";
    //    params["PatientId"] = ROSTemplateRevamp.params.patientID;
    //    params["FromAdmin"] = 0;
    //    params["ParentCtrl"] = ROSTemplateRevamp.params.ParentCtrl != "clinicalTabProgressNote" ? ROSTemplateRevamp.params.TabID : "ROSTemplateRevamp";
    //    LoadActionPan("DRFirst", params);
    //},

    ROSTemplateSave: function () {
        //Start//13/01/2016//Ahmad Raza//fixed bug#EMR-212
        $("#" + ROSTemplateRevamp.params.PanelID + " #frmPhysicalExamTemplate").bootstrapValidator('revalidateField', 'ProblemName');
        if ($("#" + ROSTemplateRevamp.params.PanelID + " #frmPhysicalExamTemplate #txtProblems").val() != "") {
            //End//13/01/2016//Ahmad Raza//fixed bug#EMR-212
            var strMessage = "";
            $("#" + ROSTemplateRevamp.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            var self = $("#" + ROSTemplateRevamp.params.PanelID + " #frmPhysicalExamTemplate");
            var myJSON = self.getMyJSONByName();
            //return false;
            if (ROSTemplateRevamp.params.mode == "Add") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented for privileges
                // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
                var hfProblemText = $("#" + ROSTemplateRevamp.params.PanelID + " #hfIMOProblem").val();
                var changesProblemText = $("#" + ROSTemplateRevamp.params.PanelID + " #txtProblems").val();
                // End 19/01/2016 Muhammad Irfan for bug # EMR-219
                if (hfProblemText.toString() == changesProblemText.toString()) {
                    AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            ROSTemplateRevamp.ROSExamTemplate(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    utility.DisplayMessages(response.message, 1);
                                    if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                                        ROSTemplateRevamp.getROSTemplateInfo(response.ProblemListId);
                                    }
                                    else {
                                        ROSTemplateRevamp.ROSTemplateSearch();
                                    }

                                    $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').resetAllControls(null);
                                    $("#" + ROSTemplateRevamp.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                                    // Start 27/11/2015 Muhammad Irfan Bug # 91,92
                                    $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                                    $("#" + ROSTemplateRevamp.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
                                    // End 27/11/2015 Muhammad Irfan Bug # 91,92

                                    //if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                                    //    ROSTemplateRevamp.getROSTemplateInfo(response.ProblemListId);
                                    //}
                                    //Start//17/12/2015//Ahmad Raza//Serialization of form
                                    $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').data('serialize', $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').serialize());
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
                    $("#" + ROSTemplateRevamp.params.PanelID + " #txtProblems").val('');
                    $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                }
                //End//21/12/2015//Ahmad Raza//Logic implemented for privileges
            }
            else if (ROSTemplateRevamp.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ROSTemplateRevamp.UpdateROSTemplate(myJSON, ROSTemplateRevamp.params.ProblemListId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                //ROSTemplateRevamp.ROSTemplateSearch();
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

    ROSExamTemplate: function (PhysicalExamTemplateData) {

        var objData = JSON.parse(PhysicalExamTemplateData);
        if (objData.PatientId == '') {
            objData.PatientId = ROSTemplateRevamp.params.patientID;
        }
        objData["commandType"] = "SAVE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    NoKnownProblem: function () {
        var strMessage = "";
        var self = $("#" + ROSTemplateRevamp.params.PanelID + " #frmPhysicalExamTemplate");
        var myJSON = self.getMyJSONByName();
        ROSTemplateRevamp.ROSExamTemplate(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlROSTemplateRevamp_Result #btnNoKnownProblems").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                ROSTemplateRevamp.ROSTemplateSearch();
                $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').resetAllControls(null);
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
    UpdateROSTemplate: function (PhysicalExamTemplateData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(PhysicalExamTemplateData);
        if (objData.PatientId == '') {
            objData.PatientId = ROSTemplateRevamp.params.patientID;
        }
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_VITALS";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    // End Validate and save/edit functions

    // Search/Grid Load Functions
    //Adding Pagination on 04 Dec 2015 by Azhar
    ROSTemplateSearch: function (ProblemListId, PageNo, rpp) {

        //var PanelProblemListGrid = "#pnlROSTemplateRevamp #pnlROSTemplateRevamp_Result";
        //var ProblemListGridId = "#pnlROSTemplateRevamp #dgvPhysicalExamTemplate";
        ////$(ChargeGridId).dataTable().fnDestroy();
        //$(ProblemListGridId + " tbody tr").remove();
        //ROSTemplateRevamp.EditableGrid = EMRUtility.MakeEditableGrid(PanelProblemListGrid, ProblemListGridId, ROSTemplateRevamp, "0", false, false, false, false);

        var strMessage = "";

        if ($("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result").css("display") == "none") {
            $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result").show();
        }
        //Start//21/12/2015//Ahmad Raza//Implimented Privileges for ProblemList Search
        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                ROSTemplateRevamp.SearchProblemList(ProblemListId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //Adding selection column of checkbox of Problem lists for Progress Notes on 04 Dec 2015 by Azhar
                        if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                            if ($("#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate thead tr #SelectRecord").length == 0) {
                                $("#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate thead tr").prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderProblemsList" onchange="ROSTemplateRevamp.checkUncheckAllProblemsList(this);"   class="input-block" coltype="checkbox"/> </th>');
                            }

                        } else {
                            $("#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate th#SelectRecord").remove();
                        }
                        //ROSTemplateRevamp.ProblemListGridLoad(response);
                        ROSTemplateRevamp.ProblemListGridLoadNew(response);
                        //Adding Pagination on 04 Dec 2015 by Azhar
                        var TableControl = ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate";
                        var PagingPanelControlID = ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate_Paging";
                        var ClassControlName = "ROSTemplateRevamp";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ProblemListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            ROSTemplateRevamp.ROSTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                        setTimeout(function () {
                            if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + ROSTemplateRevamp.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").length == 0) {
                                $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="ROSTemplateRevamp.addROSTemplateToNotes();" disabled id="btnAddProbListToNotes">Add on Note</button>').insertAfter("#" + ROSTemplateRevamp.params.PanelID + "  #dgvPhysicalExamTemplate_Paging .pagination")
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
            $("#" + ROSTemplateRevamp.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", true);
        } else {
            $("#" + ROSTemplateRevamp.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", false);
        }
        $("#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate tbody").find('input[type="checkbox"]').each(function () {
            ROSTemplateRevamp.enableAddProbList(this);
        });
    },
    //End by Khaleel Ur Rehman to check uncheck all problem list by a checkBox in header. Date: 22 Jan 2016.
    ProblemListGridLoad: function (response) {
        //$("#pnlROSTemplateRevamp #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
        //$("#pnlROSTemplateRevamp #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate tbody").find("tr").remove();
        if (response.ProblemListCount > 0) {
            ROSTemplateRevamp.EditableGrid.datatable.clear().draw();
            ////new
            //var actions = "";
            //$("#pnlROSTemplateRevamp #dgvPhysicalExamTemplate tr th").each(function () {
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
                var CurrentRow = ROSTemplateRevamp.AddNewROSTemplateRow(ProblemListId, "Edit", null);
                //var VisitChargesTable = $("#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate");
                var self = $("#dgvPhysicalExamTemplate tr#" + ProblemListId);
                //utility.bindMyJSON(true, item, false, VitalsLoadJSONData);
                utility.bindMyJSONByName(true, item, false, self).done(function () {
                    //$('#pnlROSTemplateRevamp #btnsave').text('Update');
                    //ROSTemplateRevamp.params["VitalSignsId"] = VitalSignsId;
                    //ROSTemplateRevamp.params["mode"] = "Edit";
                });

                var row = ROSTemplateRevamp.EditableGrid.datatable.row(CurrentRow);

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
            $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate").css("display", "none");
            $('#dgvPhysicalExamTemplate').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },

    SearchProblemList: function (ProblemListId, PageNumber, RowsPerPage) {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');
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
        objData["NoteId"] = ROSTemplateRevamp.params.NotesId == null ? 0 : ROSTemplateRevamp.params.NotesId;
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
    AddNewROSTemplateRow: function (RowId, mode, CurrRef, NotesId) {

        var CurrentRow = null;
        if (RowId && RowId > 0) {

            CurrentRow = ROSTemplateRevamp.EditableGrid.rowAdd(RowId, ROSTemplateRevamp.params.VitalSignsId, null, null, null, null, NotesId);

        }
        else {
            var TemplateRow = $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = ROSTemplateRevamp.EditableGrid.rowAdd(TemplateRowId - 1, ROSTemplateRevamp.params.VitalSignsId, null, null, null, null, null);
            //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        }

        var row = ROSTemplateRevamp.EditableGrid.datatable.row(CurrentRow);
        row.child(ROSTemplateRevamp.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        ROSTemplateRevamp.enableRemoveRow($(CurrentRow));
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
                    ROSTemplateRevamp.UpdateROSTemplateRow(myJSON, id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            //Serialization
                            //  $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').data('serialize', $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').serialize());

                            //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                            //Start//31/12/2015//Ahmad Raza//Logic to update against current Note only
                            if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote" && NotesId.indexOf(ROSTemplateRevamp.params.NotesId) > -1) {
                                //Ends//31/12/2015//Ahmad Raza//Logic to update against current Note only
                                ROSTemplateRevamp.getROSTemplateInfo(id);
                            }
                            //End//31/12/2015//Ahmad Raza//Bug#178 fixed
                            utility.DisplayMessages(response.message, 1);
                            ROSTemplateRevamp.ROSTemplateSearch();
                            //ROSTemplateRevamp.rowDraw($row, _self, values);
                            //  ROSTemplateRevamp.ROSTemplateSearch();
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
            ROSTemplateRevamp.VitalsEdit(currentVitalSignId);
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
                        ROSTemplateRevamp.DeleteProblemList(selectedValue, description, $row.find("td:nth-child(7)").html()).done(function (response) {
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
                            ROSTemplateRevamp.ROSTemplateSearch();
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
        IsActive = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');

        AppPrivileges.GetFormPrivileges("Clinical_Template_Physical Exam", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var IsActiveRecord = null;
                        IsActiveRecord = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');
                        if (IsActiveRecord == "1") {
                            var params = [];
                            var PanelID = "";

                            if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                                params["ParentCtrl"] = 'ROSTemplateRevamp';
                                PanelID = 'pnlClinicalProgressNote #pnlROSTemplateRevamp';
                            }
                            else if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabFaceSheet") {
                                params["ParentCtrl"] = 'ROSTemplateRevamp';
                                PanelID = 'pnlClinicalFaceSheet #pnlROSTemplateRevamp';
                            }
                            else {
                                params["ParentCtrl"] = 'clinicalTabPhysicalExamTemplate';
                                PanelID = 'pnlROSTemplateRevamp';
                            }
                            params["ProblemListId"] = selectedValue;
                            //Start//Ahmad Raza//15/12/2015//InActive PopUp issue in ProgressNote Resolved
                            params["FromAdmin"] = "0";
                            //params["ParentCtrl"] = "ROSTemplateRevamp";
                            //End//Ahmad Raza//15/12/2015//InActive PopUp issue in ProgressNote Resolved
                            params["PatientId"] = $('#PatientProfile #hfPatientId').val();
                            LoadActionPan('Clinical_ProblemListInActive', params, PanelID);
                        } else {
                            IsActiveRecord = "0";
                            ROSTemplateRevamp.InActiveProblemList(selectedValue, null, null).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    ROSTemplateRevamp.ROSTemplateSearch();
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
        IsCheckedIn = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        //     var ProblemListId = ROSTemplateRevamp.params.ProblemListId;
        var patientId = ROSTemplateRevamp.params.PatientId;

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
        if (ROSTemplateRevamp.params.ActionPanContainer == "actionPanClinicalProgressNote") {
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

        var VitalsGridId = "#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = ROSTemplateRevamp.EditableGrid.datatable.row(parentRow);

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
    UpdateROSTemplateRow: function (ProblemListData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListData);
        objData["ProblemListId"] = ProblemListId;
        objData["Comments"] = $('#' + ROSTemplateRevamp.params.PanelID + ' #hfGridComments').val();
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    //
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = ROSTemplateRevamp.EditableGrid.datatable.row(CurrentRow);
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
        objData["PatientId"] = ROSTemplateRevamp.params.patientID;
        objData["Description"] = Description;
        objData["StartDate"] = StartDate;


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ROSTemplateRevamp", "ProblemList");

    },

    // problem list grid load as per admin

    ProblemListGridLoadNew: function (response) {

        //Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search

        $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate").dataTable().fnClearTable();
        $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate tbody").find("tr").remove();


        //    if ($.fn.dataTable.isDataTable("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate")) {
        //        $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
        //    }

        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        var isactive = $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #divSwitch #switchActive').attr('isactive');



        if (response.ProblemListCount > 0) {
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);
            // get Actions
            var actions = "";
            $("#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EMREditableGrid.GetActions(arrActionType);
                    }
                }
            });
            if ($.fn.dataTable.isDataTable("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate")) {
                $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
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
                    var commentsMethod = "ROSTemplateRevamp.AddComments('" + item.ProblemListId + "');";
                    //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                    comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                }
                var ProblemListId = item.ProblemListId;
                var ChildHistory_ProblemList = $.grep(ProblemListHistoryLoadJSONData, function (n, i) {
                    return n.ProblemId == ProblemListId;
                });
                if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote" && item.ProblemName == "No Known Problems") {
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

                    if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="ROSTemplateRevamp.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
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

                        if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                            SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" onchange="ROSTemplateRevamp.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
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


                $("#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate tbody").last().append($row);

                var CurrentRowchilds = $();

                if (ChildHistory_ProblemList.length > 0) {
                    $.each(ChildHistory_ProblemList, function (i, item) {
                        // if (item.ProblemId == ProblemListId) {
                        //arrProblemListHistory.push(item);
                        if (item.Comments != "") {
                            var commentsMethod = "ROSTemplateRevamp.AddComments('" + item.ProblemListId + "');";
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
                        if (ROSTemplateRevamp.params.ActionPanContainer == "actionPanClinicalProgressNote") {
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
            var PanelGrid = "#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result";
            var GridId = "#" + ROSTemplateRevamp.params.PanelID + " #dgvPhysicalExamTemplate";

            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search
            if (ROSTemplateRevamp.myGrid != null) {
                //  ROSTemplateRevamp.myGrid.$table.find("tbody tr").remove();
                // ROSTemplateRevamp.myGrid.$table.dataTable().fnClearTable()
                ROSTemplateRevamp.myGrid.$table.dataTable().fnDestroy();

                //  ROSTemplateRevamp.myGrid.datatable.clear().draw();
                $("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate").dataTable().fnDestroy();
            }
            //End By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

            //   if ($.fn.dataTable.isDataTable("#" + ROSTemplateRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate") == false) {
            ROSTemplateRevamp.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, ROSTemplateRevamp, 0, false, true, false, true, false, null);
            //  $("#" + ROSTemplateRevamp.params.PanelID + " div.mystuff").attr('id', 'divSwitch');
            //   }

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (ROSTemplateRevamp.myGrid != null) {
                    var row = ROSTemplateRevamp.myGrid.datatable.row(item.row);
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
            $('#' + ROSTemplateRevamp.params.PanelID + ' #dgvPhysicalExamTemplate').dataTable().fnSettings().aoColumns[0].bSortable = false;
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

            $("#" + ROSTemplateRevamp.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            //  if ($('#pnlROSTemplateRevamp #switchActive').is(':checked') || $("#pnlROSTemplateRevamp_Result #btnNoKnownProblems").is(':visible')) {



            $('#' + ROSTemplateRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #dgvPhysicalExamTemplate').DataTable({
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

            $("#" + ROSTemplateRevamp.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            if (response.ProblemListTotalCount == 0) {
                $("#pnlROSTemplateRevamp_Result #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlROSTemplateRevamp_Result #btnNoKnownProblems").hide();
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
        $('#' + ROSTemplateRevamp.params.PanelID + ' #dgvPhysicalExamTemplate thead tr th:first-child').removeClass('sorting_asc');
        //End//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        //Editable Grid
        //var PanelGrid = "#pnlROSTemplateRevamp #pnlROSTemplateRevamp_Result";
        //var GridId = "#pnlROSTemplateRevamp #dgvPhysicalExamTemplate";
        //EMRUtility.MakeEditableGrid(PanelGrid, GridId, ROSTemplateRevamp, 0);

    },

    //
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate', function () {
                if (ROSTemplateRevamp.params["FromAdmin"] == "0") {
                    if (ROSTemplateRevamp.params != null && ROSTemplateRevamp.params.ParentCtrl != null) {
                        UnloadActionPan(ROSTemplateRevamp.params.ParentCtrl, 'ROSTemplateRevamp');
                    }
                    else
                        UnloadActionPan(null, 'ROSTemplateRevamp');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_PhysicalExamTemplate").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (ROSTemplateRevamp.params["FromAdmin"] == "0") {
                    if (ROSTemplateRevamp.params != null && ROSTemplateRevamp.params.ParentCtrl != null) {
                        UnloadActionPan(ROSTemplateRevamp.params.ParentCtrl, 'ROSTemplateRevamp');
                    }
                    else
                        UnloadActionPan(null, 'ROSTemplateRevamp');
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
            if (ROSTemplateRevamp.params["FromAdmin"] == "0") {
                if (ROSTemplateRevamp.params != null && ROSTemplateRevamp.params.ParentCtrl != null) {
                    UnloadActionPan(ROSTemplateRevamp.params.ParentCtrl, 'ROSTemplateRevamp');
                }
                else
                    UnloadActionPan(null, 'ROSTemplateRevamp');
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
        if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'ROSTemplateRevamp';
            PanelID = 'pnlClinicalProgressNote #pnlROSTemplateRevamp';
        }
            // Start By Babur on 2/15/2016 - Bug EMR-329 - FaceSheet in clinical module -> Problem list -> Edit comments
        else if (ROSTemplateRevamp.params.ParentCtrl == "clinicalTabFaceSheet") {
            params["ParentCtrl"] = 'ROSTemplateRevamp';
            PanelID = 'pnlClinicalFaceSheet #pnlROSTemplateRevamp';
        }
            // End By Babur on 2/15/2016 - Bug EMR-329 - FaceSheet in clinical module -> Problem list -> Edit comments
        else {
            params["ParentCtrl"] = 'clinicalTabPhysicalExamTemplate';
            PanelID = 'pnlROSTemplateRevamp';
        }

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */

        params["ProblemListId"] = ProblemListId;
        //Start//Ahmad Raza//15/12/2015//Comments Popup issue in ProgressNote, Resolved
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "ROSTemplateRevamp";
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

        ROSTemplateRevamp.ROSTemplateSearch();
    },

    // Start 27/11/2015 Muhammad Irfan Bug # 93,94
    ResetDiagnosis: function () {

        if ($("#" + ROSTemplateRevamp.params.PanelID + " #txtProblems").val() == "") {
            $('#' + ROSTemplateRevamp.params.PanelID + ' #frmPhysicalExamTemplate').resetAllControls(null);
            $("#" + ROSTemplateRevamp.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
        }
    },
    // End 27/11/2015 Muhammad Irfan Bug # 93,94

    //-----------------Progress Note-------------
    // added on Dec 04,2015 by Muhammad Azhar Shahzad
    //Call Back function to add component to Progress Note
    addROSTemplateToNotes: function () {
        //var SelectedPhysicalExamTemplate = $("#" + ROSTemplateRevamp.params.PanelID + ' [name=SelectCheckBoxProbList]:checked:not(:disabled)').map(function () { return this.id; }).get().join();
        //if (SelectedPhysicalExamTemplate != null && SelectedPhysicalExamTemplate != '') {
        //    ROSTemplateRevamp.getROSTemplateInfo(SelectedPhysicalExamTemplate);
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
                        ROSTemplateRevamp.getROSTemplateInfo(SelectedPhysicalExamTemplate.join());
                    }
                }
                var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
                if (detachedvalues.join() != '') {

                    ROSTemplateRevamp.detachProblemsListFromNotes_DBCall(detachedvalues.join()).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            for (var i = 0; i < detachedvalues.length; i++) {
                                var PLid = detachedvalues[i];
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ProblemList_Main' + PLid).remove();
                            }

                            Clinical_ProgressNote.updateProgressNoteHTML();
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                            if (ROSTemplateRevamp.params != null && ROSTemplateRevamp.params.PanelID.indexOf('pnlROSTemplateRevamp') != -1) {
                                ROSTemplateRevamp.ROSTemplateSearch();
                            }
                            //   utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //When User has attached Allergies with notes than add on note button should be disabled
                $("#" + ROSTemplateRevamp.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").prop('disabled', true);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    //this function will get Problem Lists Soap Text and attach that to Progress note
    getROSTemplateInfo: function (PhysicalExamTemplateId) {
        if (PhysicalExamTemplateId == null || PhysicalExamTemplateId == '') {
            return false;
        }

        ROSTemplateRevamp.get_ROSTemplate_ForSOAP(PhysicalExamTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                if (response.PhysicalExamTemplateoapCount > 0) {

                    //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML ROSTemplateRevamp').parent().parent().find('#Cli_ProblemList_Main' + PhysicalExamTemplateId).length == 0) {
                    //    ROSTemplateRevamp.detachProblemListFromNotes('ProblemList', false, false, PhysicalExamTemplateId);
                    //}
                    //Start//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    ROSTemplateRevamp.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', PhysicalExamTemplateId);
                    //End//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (ROSTemplateRevamp.params != null && ROSTemplateRevamp.params.PanelID.indexOf('pnlROSTemplateRevamp') != -1) {
                        ROSTemplateRevamp.ROSTemplateSearch();
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
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML ROSTemplateRevamp').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="PhysicalExamTemplateComponent"> <header>' +
                        '<ROSTemplateRevamp title="ProblemsList"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ROSTemplateRevamp\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="ROSTemplateRevamp">Problem Lists</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ROSTemplateRevamp\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</ROSTemplateRevamp> </header></li>');
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
        ROSTemplateRevamp.checkProblemListExists();
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


            $ListVital.append("<li><strong> " + element.ProblemName + " </strong>" + (element.ChronicityLevel == '' ? "" : " Chronicity: " + element.ChronicityLevel) + ((element.Severity && element.ChronicityLevel) ? ", " : "") + (element.Severity == '' ? "" : "<span " + color + '>' + element.Severity + "</span> ") +
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
            if ($(NoteHTMLCtrl + ' ROSTemplateRevamp').parent().parent().find('#Cli_ProblemList_Main' + PLid).length == 0) {
                PListId.push(PLid);
                $mainDivVital.append($SectionBodyVital);
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' ROSTemplateRevamp').parent().parent().find('#Cli_ProblemList_Main' + PLid + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' ROSTemplateRevamp').parent().parent().find('#Cli_ProblemList_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' ROSTemplateRevamp').parent().parent().find('#Cli_ProblemList_Main' + PLid).html($SectionBodyVital.html());
                $(NoteHTMLCtrl + ' ROSTemplateRevamp').parent().parent().find('#Cli_ProblemList_Main' + PLid + ' ul').append(CommentHTML);;
            }

        });
        //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
        if (PListId.join(",") != "") {
            PhysicalExamTemplateId = PListId.join(",");
        }
        //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
        if ($mainDivVital.html() != '') {
            //Start//04//01//2015//Ahmad Raza//
            ROSTemplateRevamp.updateProblemListHtml($mainDivVital.html(), PhysicalExamTemplateId, NoteHTMLCtrl);
        } else {
            ROSTemplateRevamp.updateProblemListHtml('', PhysicalExamTemplateId, NoteHTMLCtrl);
            Clinical_ProgressNote.updateProgressNoteHTML();
        }

    },

    // This Function is called by Progress Notes (Fill Vitals Func, CopyAllNotesCategories)
    updateProblemListHtml: function (ProblemListHtml, ProblemListId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' ROSTemplateRevamp').parent().parent().addClass('initialVisitBody');
        if (ProblemListHtml != '') {
            $(NoteHTMLCtrl + ' ROSTemplateRevamp').parent().parent().append(ProblemListHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ProblemListHtml != '') {
            ROSTemplateRevamp.attachProblemListignFromNotes(ProblemListId);
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
                        ROSTemplateRevamp.detachProblemsListFromNotes_DBCall(selectedValue).done(function (response) {
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
        var Clinical_ProblemListIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML ROSTemplateRevamp').parent().parent().find('section[id*="Cli_ProblemList_Main"]').map(function () {
            return this.id.replace("Cli_ProblemList_Main", "");
        }).get().join(',');
        if (Clinical_ProblemListIds == "" || Clinical_ProblemListIds == "undefined") {
        }
        else {


            ROSTemplateRevamp.detachProblemsListFromNotes_DBCall(Clinical_ProblemListIds).done(function (response) {
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML ROSTemplateRevamp').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML ROSTemplateRevamp').parent().parent().find('section[id*="Cli_ProblemList_Main"]').remove();
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
                ROSTemplateRevamp.attachProblemsListWithNotes_DBCall(selectedValue).done(function (response) {
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
            $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlROSTemplateRevamp_Result #chkHeaderProblemsList').prop('checked', false);
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
            $("#" + ROSTemplateRevamp.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").prop('disabled', false);
        } else {
            $("#" + ROSTemplateRevamp.params.PanelID + "  #dgvPhysicalExamTemplate_Paging #btnAddProbListToNotes").prop('disabled', true);
        }

    },

    //If ROSTemplateRevamp Component which is dropeed in Progress note has no ProblemList attached, than it will call for Latest ProblemList for this patient
    getLatestProblemListByPatientId: function () {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            ROSTemplateRevamp.getLatestProblemListByPatientId_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.PhysicalExamTemplateoapCount > 0) {
                        ROSTemplateRevamp.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
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
        ROSTemplateRevamp.getLatestProblemListByPatientId();
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

    get_ROSTemplate_ForSOAP: function (PhysicalExamTemplateId) {

        var objData = new Object();
        objData["ProblemListId"] = PhysicalExamTemplateId;
        objData["PatientId"] = ROSTemplateRevamp.params.patientID;
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