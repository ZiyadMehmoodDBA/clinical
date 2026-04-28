Clinical_AuditReport = {
    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //This file will handle all actions performed for AuditReport
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,

    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //This function will handle fill of AuditReport 
    Load: function (params) {
        Clinical_AuditReport.params = params;

        Clinical_AuditReport.patientId = $("div#PatientProfile #hfPatientId").val();

        Clinical_AuditReport.params.mode = "Add";

        if (Clinical_AuditReport.params.PanelID != 'pnlClinicalAuditReport') {
            Clinical_AuditReport.params.PanelID = Clinical_AuditReport.params.PanelID + ' #pnlClinicalAuditReport';
        } else {
            Clinical_AuditReport.params.PanelID = 'pnlClinicalAuditReport';
        }
        var self = $('#' + Clinical_AuditReport.params.PanelID);
        if (Clinical_AuditReport.bIsFirstLoad == true) {
            Clinical_AuditReport.bIsFirstLoad = false;

            self.loadDropDowns(true).done(function () {
                if (globalAppdata["AppUserName"].toLowerCase() != "mdvision") {
                    CacheManager.BindDropDownsByID($('#' + Clinical_AuditReport.params.PanelID + ' #frmClinicalAuditReport #ddlAuditUser'), 'GetUsers', true, 1).done(function () {
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divAuditUser #ddlAuditUser').find('option:contains(' + "- Select -" + ')').remove();
                        Clinical_AuditReport.IntializeMultiSelectDropDownUserName('divAuditUser #ddlAuditUser');
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divAuditUser').find('.input-group-btn').css({ 'background': 'blue' });
                    });
                    CacheManager.BindDropDownsByID($('#' + Clinical_AuditReport.params.PanelID + ' #frmClinicalAuditReport #ddlUserAuditUser'), 'GetUsers', true, 1).done(function () {
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divUserAuditUser #ddlUserAuditUser').find('option:contains(' + "- Select -" + ')').remove();
                        Clinical_AuditReport.IntializeMultiSelectDropDownUserName('divUserAuditUser #ddlUserAuditUser');
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divUserAuditUser').find('.input-group-btn').css({ 'background': 'blue' });
                    });
                }
                else {
                    CacheManager.BindDropDownsByID($('#' + Clinical_AuditReport.params.PanelID + ' #frmClinicalAuditReport #ddlAuditUser'), 'GetUsers', true, 0).done(function () {
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divAuditUser #ddlAuditUser').find('option:contains(' + "- Select -" + ')').remove();
                        Clinical_AuditReport.IntializeMultiSelectDropDownUserName('divAuditUser #ddlAuditUser');
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divAuditUser').find('.input-group-btn').css({ 'background': 'blue' });
                    });
                    CacheManager.BindDropDownsByID($('#' + Clinical_AuditReport.params.PanelID + ' #frmClinicalAuditReport #ddlUserAuditUser'), 'GetUsers', true, 0).done(function () {
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divUserAuditUser #ddlUserAuditUser').find('option:contains(' + "- Select -" + ')').remove();
                        Clinical_AuditReport.IntializeMultiSelectDropDownUserName('divUserAuditUser #ddlUserAuditUser');
                        $('#' + Clinical_AuditReport.params.PanelID + ' #divUserAuditUser').find('.input-group-btn').css({ 'background': 'blue' });
                    });
                   
                }

                if (!$('#' + Clinical_AuditReport.params.PanelID + ' #ddlPatAccount').data("kendoMultiSelect"))
                    utility.InitKendoPatAccountAutoComplete(Clinical_AuditReport.params.PanelID + ' #ddlPatAccount', Clinical_AuditReport.params.PanelID + ' #hfPatientId');
                //Start//16-04-2016//Ahmad Raza//Logic to select current UserName,From and ToDate by Default and searching audit report against them
                $("#" + Clinical_AuditReport.params.PanelID + " #ddlAuditUser option").each(function () {
                    if ($(this).text() == globalAppdata["AppUserName"]) {
                        $(this).attr('selected', 'selected');
                    }
                });
                $("#" + Clinical_AuditReport.params.PanelID + " #ddlUserAuditUser option").each(function () {
                    if ($(this).text() == globalAppdata["AppUserName"]) {
                        $(this).attr('selected', 'selected');
                    }
                });
                utility.CreateDatePicker(Clinical_AuditReport.params.PanelID + '  #dtpFromDate', function () {
                }, true);
                utility.CreateDatePicker(Clinical_AuditReport.params.PanelID + '  #dtpUserFromDate', function () {
                }, true);
                utility.CreateDatePicker(Clinical_AuditReport.params.PanelID + '  #dtpToDate', function () {
                }, true);
                utility.CreateDatePicker(Clinical_AuditReport.params.PanelID + '  #dtpUserToDate', function () {
                }, true);
                var currentDate = new Date();
                var yesterdayDate = new Date(currentDate.getTime() + (-1) * 24 * 60 * 60 * 1000);
                $("#" + Clinical_AuditReport.params.PanelID + " #frmClinicalAuditReport #dtpFromDate").datepicker("setDate", yesterdayDate);
                $("#" + Clinical_AuditReport.params.PanelID + " #frmClinicalAuditReport #dtpToDate").datepicker({ dateFormat: "mm-dd-yy" }).datepicker("setDate", "0");
                $("#" + Clinical_AuditReport.params.PanelID + " #frmClinicalAuditReport #dtpUserFromDate").datepicker("setDate", yesterdayDate);
                $("#" + Clinical_AuditReport.params.PanelID + " #frmClinicalAuditReport #dtpUserToDate").datepicker({ dateFormat: "mm-dd-yy" }).datepicker("setDate", "0");
                Clinical_AuditReport.auditReportSearch();
                //End//16-04-2016//Ahmad Raza//Logic to select current UserName,From and ToDate by Default and searching audit report against them
            });

            //Start 04-03-2016 Humaira Yousaf to load AuditReport
            //Start//10-03-2016//Ahmad Raza//logic to open AuditReportSearch popup from Facesheet 
            if (Clinical_AuditReport.params.TabID == "clinicalTabFaceSheet") {
                //Clinical_AuditReport.AuditReportSearch(null, null, null, $(" #mainForm  li#AuditReportAlert input").val(), 'Yes');
                $('#' + Clinical_AuditReport.params.PanelID + ' #btn-add').hide();
            }
            else {
                // Clinical_AuditReport.AuditReportSearch();
            }
            //End//10-03-2016//Ahmad Raza//logic to open AuditReportSearch popup from Facesheet
            //End 04-03-2016 Humaira Yousaf to load AuditReport
        }

        //utility.CreateDatePicker(Clinical_AuditReport.params.PanelID + ' #dtpFromDate', function () {
        //    //on-change callback method 
        //});

        //utility.CreateDatePicker(Clinical_AuditReport.params.PanelID + ' #dtpToDate', function () {
        //    //on-change callback method 
        //});
        utility.ValidateFromToDate(Clinical_AuditReport.params.PanelID + ' #pnlAuditReport_Search', "dtpFromDate", "dtpToDate", true);
        utility.ValidateFromToDate(Clinical_AuditReport.params.PanelID + ' #pnlAuditReport_Search', "dtpUserFromDate", "dtpUserToDate", true);
        $('#UserTabControl').addClass('hidden');
    },

    IntializeMultiSelectDropDownUserName: function (CtrlId) {
        $('#' + Clinical_AuditReport.params.PanelID + ' #' + CtrlId).multiselect('destroy');
        $('#' + Clinical_AuditReport.params.PanelID + ' #' + CtrlId).multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247
        });
        $('#' + Clinical_AuditReport.params.PanelID + ' #' + CtrlId).val("");
    },

    openAuditReportView: function () {

        var params = [];


        params["FromAdmin"] = 0;
        params["isPopup"] = 1;
        LoadActionPan("Clinical_AuditReportView", params);

    },


    //Start//07-04-2016//Ahmad Raza//plugin for Toggle switch 
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
    //End//07-04-2016//Ahmad Raza//plugin for Toggle switch 

    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //This function will handle autocomplete of Patient
    BindPatientAccount: function () {
        $("#" + Clinical_AuditReport.params.PanelID + " #txtPatientName").autocomplete({
            autoFocus: true,
            source: function (request, response) {
                utility.Keyupdelay(function () {
                    var AccountNo = $('#' + Clinical_AuditReport.params.PanelID + ' #txtPatientName').val();
                    if (AccountNo.length > 2) {
                        appointmentDetail.LoadActivePatients(AccountNo).done(function (responseData) {
                            if (responseData.status != false) {
                                if (responseData.PatientCount > 0) {
                                    var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                                    var AllPatients = [];
                                    $.each(PatientLoadJSONData, function (i, item) {

                                        AllPatients.push({ id: item.PatientId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName });


                                    });
                                    response(AllPatients);
                                }
                            }
                        });
                    }

                });
            },

            select: function (event, ui) {
                
                //$("#appointmentDetail #txtAccountNo").val(ui.item.id); // add the selected id
                //$("#appointmentDetail #txtFullName").val(ui.item.patientName);
                setTimeout(function () {
                    $("#" + Clinical_AuditReport.params["PanelID"] + " #hfPatientId").val(ui.item.id);
                    $("#" + Clinical_AuditReport.params["PanelID"] + " #txtPatientName").val(ui.item.value.split(" ")[0]);
                    $("#" + Clinical_AuditReport.params["PanelID"] + " #txtPatientFullName").val(ui.item.FullName);


                    //Clinical_AuditReport.LoadPatientCase(ui.item.id);
                    //Clinical_AuditReport.EnableClaimCaseFields();
                }, 100);

                //$("#hfpatientid").val(ui.item.id);
            }
        });
    },

    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //This function will handle search of Patient 
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = Clinical_AuditReport.params["TabID"];
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName) {
        $("#" + Clinical_AuditReport.params["PanelID"] + " #hfPatientId").val(PatientId);
        $("#" + Clinical_AuditReport.params["PanelID"] + " #txtPatientName").val(patFullName.split(" ")[0]);
        UnloadActionPan(Clinical_AuditReport.params["TabID"]);
    },
    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //This function will handle Add/Edit of AuditReport based on given AuditReportId
    AuditReportAddEdit: function (AuditReportId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Report_Audit Report", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (AuditReportId != null && parseInt(AuditReportId) > 0) {
                    params["AuditReportId"] = AuditReportId;
                    params["mode"] = "Edit";
                }
                else {
                    params["AuditReportId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_AuditReport.params["FromAdmin"];
                //// params["ParentCtrl"] = Admin_Provider.params["ParentCtrl"];
                //if (Admin_Provider.params["FromAdmin"] == "0") {
                //    params["ParentCtrl"] = 'Clinical_AuditReport';
                //}
                params["ParentCtrl"] = 'adminTabAuditReport';
                params["TabID"] = 'ClinicalAuditReportDetail';

                LoadActionPan('ClinicalAuditReportDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    ValidateAuditReport: function () {
        $('#frmClinicalAuditReport')
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
            Clinical_AuditReport.AuditReportSave();
        });
    },

    //Function Name: auditReportSearch
    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //Description: Searches AuditReport data
    //Params: AuditReportId, PageNo, rpp
    auditReportSearch: function (PrimaryID, pageNumber, rowsPerPage) {
        var strMessage = "";


        AppPrivileges.GetFormPrivileges("Clinical_Report_Audit Report", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result").css("display") == "none") {
                    $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result").show();
                }
                var self = $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Search");
                var myJSON = self.getMyJSONByName();

                Clinical_AuditReport.searchAuditReport(myJSON, pageNumber, rowsPerPage).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if ($('#pnlClinicalAuditReport #LiPatient').parent().hasClass('active') == true) {
                            $('#pnlClinicalAuditReport #btn-add').removeClass('hidden');
                            $('#UserTabControl').addClass('hidden');
                            $('#PatientTabControl').removeClass('hidden');
                            //  For Patient

                            Clinical_AuditReport.auditReportGridLoad(response);

                            //server side pagination
                            var TableControl = Clinical_AuditReport.params.PanelID + " #dgvAuditReport";
                            var PagingPanelControlID = Clinical_AuditReport.params.PanelID + " #dgvAuditReport_Paging";
                            var ClassControlName = "Clinical_AuditReport";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.DBAuditCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                                Clinical_AuditReport.auditReportSearch(PrimaryID, pageNumber, resultPerPage);
                            }), 10);

                            //server side pagination

                            //  },0,response);
                        } else { //  For User

                            $('#pnlClinicalAuditReport #btn-add').addClass('hidden');
                            Clinical_AuditReport.auditReportGridLoadUser(response);
                            $('#UserTabControl').removeClass('hidden');
                            $('#PatientTabControl').addClass('hidden');
                            //server side pagination
                            var TableControl = Clinical_AuditReport.params.PanelID + " #dgvAuditReportUser";
                            var PagingPanelControlID = Clinical_AuditReport.params.PanelID + " #dgvAuditReportUser_Paging";
                            var ClassControlName = "Clinical_AuditReport";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.DBAuditCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                                Clinical_AuditReport.auditReportSearch(PrimaryID, pageNumber, resultPerPage);
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
    //Function Name: auditReportGridLoad
    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //Description: Loads AuditReport data in grid
    //Params: response
    auditReportGridLoad: function (response) {
        $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result #dgvAuditReport").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result #dgvAuditReport tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.DBAuditCount > 0) {
            var DBAuditLoadJSONData = JSON.parse(response.DBAuditLoad_JSON); //Parsing array to JSON
            $.each(DBAuditLoadJSONData, function (i, item) {
                //             if (!(item.OriginalValue == "" && item.CurrentValue == "")) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Clinical_AuditReport.AuditReportEdit('" + item.DBAuditId + "',event);");
                $row.attr("id", "gvAuditReport_row" + item.DBAuditId);
                $row.attr("AuditReportId", item.DBAuditId);
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

                var currentCreatedDate = new Date(item.CreatedDate);
                var currentHours = currentCreatedDate.getHours();
                //Start//14-04-2016//Ahmad Raza//Adding preceeding 0 if hours,minutes or second's length is 1
                if ($.trim(currentHours).length == 1) {
                    currentHours = '0' + currentHours;
                }
                var currentMinutes = currentCreatedDate.getMinutes();
                if ($.trim(currentMinutes).length == 1) {
                    currentMinutes = '0' + currentMinutes;
                }
                var currentSeconds = currentCreatedDate.getSeconds();
                if ($.trim(currentSeconds).length == 1) {
                    currentSeconds = '0' + currentSeconds;
                }
                //End//14-04-2016//Ahmad Raza//Adding preceeding 0 if hours,minutes or second's length is 1
                var currentTime = currentHours + ":" + currentMinutes + ":" + currentSeconds;

                var tempCurrent;
                var tempOriginal;

                tempCurrent = item.CurrentValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').replace(/&amp;/g, "&");
                tempOriginal = item.OriginalValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').replace(/&amp;/g, "&");

                var rex = /<img .*?>/ig;
                tempCurrent = tempCurrent.replace(rex, "");
                tempOriginal = tempOriginal.replace(rex, "");

                if (item.CurrentValue != "") {
                    tempCurrent = $.parseHTML(tempCurrent);
                }
                else {
                    tempCurrent = "";
                }

                if (tempOriginal != "") {
                    tempOriginal = $.parseHTML(tempOriginal);
                }
                else {
                    tempOriginal = "";
                }

                var refinedText = "";
                $(tempCurrent).each(function (index, val) {
                    refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    $(this).children().each(function (index, val) {
                        refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    });
                    refinedText += " ";
                });
                currentItem = refinedText;

                refinedText = "";
                $(tempOriginal).each(function (index, val) {
                    refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    $(this).children().each(function (index, val) {
                        refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    });
                });
                originalItem = refinedText;
                if ((item.ModuleName == "Medical_Allergies" && item.DisplayName == "On SetDate") || (item.ModuleName == "Medical Medications" && item.DisplayName == "Stopped On") || (item.ModuleName == "Medical Medications" && item.DisplayName == "Started On")) {
                    $row.append('<td style="display:none;">' + item.AuditReportId + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedDate) + '</td><td>' + currentTime + '</td><td>' + item.AccountNumber + '</td><td>' + item.PatientName + '</td><td>' + item.UserName + '</td><td>' + item.ModuleName + '</td><td>' + item.DBAuditAction + '</td><td>' + item.DisplayName + '</td><td id="o' + item.AuditReportId + '">' + Clinical_AuditReport.removeHTMLTags('' + originalItem + '') + '</td><td id="c' + item.AuditReportId + '">' + Clinical_AuditReport.removeHTMLTags('' + utility.RemoveTimeFromDate(null, currentItem) + '') + '</td>');
                    $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result #dgvAuditReport tbody").last().append($row);
                }
                //Start 27-05-2016 Humaira Yousaf to exclude note text
                else if ((item.ModuleName != "Notes" || item.ModuleName != "Medical Medications") && (item.DisplayName != "Note Text" && item.DisplayName != "Is Deleted")) {
                    $row.append('<td style="display:none;">' + item.AuditReportId + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedDate) + '</td><td>' + currentTime + '</td><td>' + item.AccountNumber + '</td><td>' + item.PatientName + '</td><td>' + item.UserName + '</td><td>' + item.ModuleName + '</td><td>' + item.DBAuditAction + '</td><td>' + item.DisplayName + '</td><td id="o' + item.AuditReportId + '">' + Clinical_AuditReport.removeHTMLTags('' + originalItem + '') + '</td><td id="c' + item.AuditReportId + '">' + Clinical_AuditReport.removeHTMLTags('' + currentItem + '') + '</td>');
                    $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result #dgvAuditReport tbody").last().append($row);
                }
                //End 27-05-2016 Humaira Yousaf to exclude note text

            });
        }
            //Start 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1185
        else {
            $("#" + Clinical_AuditReport.params.PanelID + ' #pnlAuditReport_Result #dgvAuditReport').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Record Found."
                }, "autoWidth": false, "bLengthChange": false, "bDestroy": true
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_AuditReport.params.PanelID + ' #pnlAuditReport_Result #dgvAuditReport'))
            ;
        else {
            $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result #dgvAuditReport").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [] }); // to remove records per page dropdown({ "bInfo": true, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": true, "aTargets": [1] }] }); // to remove records per page dropdown          
        }
        //End 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1185
    },

    auditReportGridLoadUser: function (response) {
        try {
            $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_User #dgvAuditReportUser").dataTable().fnDestroy();
        }//making sure all the instances created of datatables should be destroyed before recreating it
        catch (e) {
            //Handle errors here
        }
        $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_User #dgvAuditReportUser tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.DBAuditCount > 0) {
            var DBAuditLoadJSONData = JSON.parse(response.DBAuditLoad_JSON); //Parsing array to JSON
            $.each(DBAuditLoadJSONData, function (i, item) {
                //             if (!(item.OriginalValue == "" && item.CurrentValue == "")) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Clinical_AuditReport.AuditReportEdit('" + item.DBAuditId + "',event);");
                $row.attr("id", "dgvAuditReportUser" + item.DBAuditId);
                $row.attr("AuditReportId", item.DBAuditId);
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

                var currentCreatedDate = new Date(item.Date);
                var currentHours = currentCreatedDate.getHours();
                //Start//14-04-2016//Ahmad Raza//Adding preceeding 0 if hours,minutes or second's length is 1
                if ($.trim(currentHours).length == 1) {
                    currentHours = '0' + currentHours;
                }
                var currentMinutes = currentCreatedDate.getMinutes();
                if ($.trim(currentMinutes).length == 1) {
                    currentMinutes = '0' + currentMinutes;
                }
                var currentSeconds = currentCreatedDate.getSeconds();
                if ($.trim(currentSeconds).length == 1) {
                    currentSeconds = '0' + currentSeconds;
                }
                //End//14-04-2016//Ahmad Raza//Adding preceeding 0 if hours,minutes or second's length is 1
                var currentTime = currentHours + ":" + currentMinutes + ":" + currentSeconds;

                var currentval;
                var originalval;
                if (item.CurrentValue == "True")
                { currentval = "Yes"; }
                else if (item.CurrentValue == "False") {
                    currentval = "No";
                }
                else { currentval = item.CurrentValue; }
                if (item.OriginalValue == "True")
                { originalval = "Yes"; }
                else if (item.OriginalValue == "False") {
                    originalval = "No";
                }
                else { originalval = item.OriginalValue; }
                var tempCurrent;
                var tempOriginal;

                tempCurrent = currentval.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').replace(/&amp;/g, "&");
                tempOriginal = originalval.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').replace(/&amp;/g, "&");

                var rex = /<img .*?>/ig;
                tempCurrent = tempCurrent.replace(rex, "");
                tempOriginal = tempOriginal.replace(rex, "");

                if (item.CurrentValue != "") {
                    tempCurrent = $.parseHTML(tempCurrent);
                }
                else {
                    tempCurrent = "";
                }

                if (tempOriginal != "") {
                    tempOriginal = $.parseHTML(tempOriginal);
                }
                else {
                    tempOriginal = "";
                }

                var refinedText = "";
                $(tempCurrent).each(function (index, val) {
                    refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    $(this).children().each(function (index, val) {
                        refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    });
                    refinedText += " ";
                });
                currentItem = refinedText;

                refinedText = "";
                $(tempOriginal).each(function (index, val) {
                    refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    $(this).children().each(function (index, val) {
                        refinedText += $(val).text().trim() == "" ? $(val).val() : $(val).text().trim();
                    });
                });
                originalItem = refinedText;
                //Start 27-05-2016 Humaira Yousaf to exclude note text
                if (item.ModuleName != "Notes" && item.DisplayName != "Note Text") {
                    $row.append('<td style="display:none;">' + item.AuditReportId + '</td><td>' + item.Date.replace(" 12:00:00 AM", "") + '</td><td>' + item.Time + '</td><td>' + item.UserName + '</td><td>' + item.AdminUser + '</td><td>' + item.Action + '</td><td>' + item.Field + '</td><td id="o' + item.AuditReportId + '">' + Clinical_AuditReport.removeHTMLTags('' + originalItem + '') + '</td><td id="c' + item.AuditReportId + '">' + Clinical_AuditReport.removeHTMLTags('' + currentItem + '') + '</td>');
                    $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_User #dgvAuditReportUser tbody").last().append($row);
                }
                //End 27-05-2016 Humaira Yousaf to exclude note text

            });
        }
            //Start 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1185
        else {
            $("#" + Clinical_AuditReport.params.PanelID + ' #pnlAuditReport_User #dgvAuditReportUser').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Record Found."
                }, "autoWidth": false, "bLengthChange": false, "bDestroy": true
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_AuditReport.params.PanelID + ' #pnlAuditReport_User #dgvAuditReportUser'))
            ;
        else {
            $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_User #dgvAuditReportUser").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [] }); // to remove records per page dropdown({ "bInfo": true, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": true, "aTargets": [1] }] }); // to remove records per page dropdown          
        }
        //End 24-05-2016 Edit By Humaira Yousaf Bug# EMR-1185
    },

    removeHTMLTags: function (htmlString) {
        if (htmlString == "") {
            htmlString = '<p></p>';
        }

        if (htmlString) {
            var mydiv = document.createElement("div");
            mydiv.innerHTML = htmlString;

            if (document.all) // IE Stuff
            {
                return mydiv.innerText;

            }
            else // Mozilla does not work with innerText
            {
                return mydiv.textContent;
            }
        }
    },

    //Function Name: searchAuditReport
    //Author: Muhammad Arshad
    //Date: 14-04-2016
    //Description: Searches AuditReport
    //Params: AuditReportId, PageNumber, RowsPerPage
    searchAuditReport: function (AuditReportData, PageNumber, RowsPerPage) {
        var objData = JSON.parse(AuditReportData);
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        //objData["AuditReportId"] = AuditReportId;


        if ($('#pnlClinicalAuditReport #LiPatient').parent().hasClass('active') == true) {
            objData["commandType"] = "fill_AuditReport";
            objData["UserType"] = $("#" + Clinical_AuditReport.params.PanelID + " #ddlAuditRole").val();
            objData["PageNumber"] = PageNumber;
            objData["RowsPerPage"] = RowsPerPage;
            objData["CreatedDateFrom"] = $("#" + Clinical_AuditReport.params.PanelID + " #dtpFromDate").val();
            objData["CreatedDateTo"] = $("#" + Clinical_AuditReport.params.PanelID + " #dtpToDate").val();
            objData["AuditUserIds"] = objData["AuditUser"];
        }
        else {

            objData["commandType"] = "FILL_AUDITREPORT_USER";
            objData["AuditAction"] = $("#" + Clinical_AuditReport.params.PanelID + " #ddlUserAuditAction").val();
            objData["UserType"] = $("#" + Clinical_AuditReport.params.PanelID + " #ddlUserAuditRole").val();
            objData["PageNumber"] = PageNumber;
            objData["RowsPerPage"] = RowsPerPage;
            objData["FromDate"] = $("#" + Clinical_AuditReport.params.PanelID + " #dtpUserFromDate").val();
            objData["ToDate"] = $("#" + Clinical_AuditReport.params.PanelID + " #dtpUserToDate").val();
            objData["AuditUserIds"] = objData["UserAuditUser"];
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditReport", "AuditReport");

    },

    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_AuditReport.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(Clinical_AuditReport.params.PanelID + ' #frmClinicalAuditReport', function () {
                if (Clinical_AuditReport.params["FromAdmin"] == "0") {
                    if (Clinical_AuditReport.params != null && Clinical_AuditReport.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_AuditReport.params.ParentCtrl, 'Clinical_AuditReport');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_AuditReport');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_AuditReport").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_AuditReport.params["FromAdmin"] == "0") {
                    if (Clinical_AuditReport.params != null && Clinical_AuditReport.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_AuditReport.params.ParentCtrl, 'Clinical_AuditReport');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_AuditReport');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_AuditReport").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Clinical_AuditReport.params["FromAdmin"] == "0") {
                if (Clinical_AuditReport.params != null && Clinical_AuditReport.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_AuditReport.params.ParentCtrl, 'Clinical_AuditReport');
                }
                else
                    UnloadActionPan(null, 'Clinical_AuditReport');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_AuditReport").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },
    //Function Name: AuditReportEdit
    //Author: Muhammad Arshad
    //Date: 07-04-2016
    //Description: Opens AuditReport popup to edit data
    //Params: AuditReportId, event
    AuditReportEdit: function (AuditReportId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Result #dgvAuditReport #gvAuditReport_row" + AuditReportId));
        AppPrivileges.GetFormPrivileges("Clinical_Report_Audit Report", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = AuditReportId;
                if (Clinical_AuditReport.params.TabID == "clinicalTabFaceSheet") {
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var params = [];
                        params["AuditReportId"] = selectedValue;
                        params["mode"] = "Edit";
                        params["FromAdmin"] = 0;
                        params["ParentCtrl"] = "Clinical_AuditReport";
                        LoadActionPan('Clinical_AuditReportAlertDetail', params);
                    }
                }
                else {
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var params = [];
                        params["AuditReportId"] = selectedValue;
                        params["mode"] = "Edit";
                        LoadActionPan('ClinicalAuditReportDetail', params);
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
}