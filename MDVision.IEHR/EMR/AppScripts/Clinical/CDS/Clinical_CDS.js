Clinical_CDS = {
    //Author: Muhammad Arshad
    //Date: 24-02-2016
    //This file will handle all actions performed for CDS
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,
    Switch: 1,

    //Author: Muhammad Arshad
    //Date: 24-02-2016
    //This function will handle fill of CDS
    Load: function (params) {
        Clinical_CDS.params = params;

        Clinical_CDS.patientId = $("div#PatientProfile #hfPatientId").val();

        Clinical_CDS.params.mode = "Add";

        if (Clinical_CDS.params.PanelID != 'pnlClinicalCDS') {
            Clinical_CDS.params.PanelID = Clinical_CDS.params.PanelID + ' #pnlClinicalCDS';
        } else {
            Clinical_CDS.params.PanelID = 'pnlClinicalCDS';
        }
        var self = $('#' + Clinical_CDS.params.PanelID);
        if (Clinical_CDS.bIsFirstLoad == true) {
            //Start 04-03-2016 Humaira Yousaf to load CDS
           //Start//10-03-2016//Ahmad Raza//logic to open CDSSearch popup from Facesheet
            if (Clinical_CDS.params.TabID == "clinicalTabFaceSheet")
            {
                Clinical_CDS.CDSSearch(null, null, null, $(" #mainForm  li#CDSAlert input").val(), 'Yes');
                $('#' + Clinical_CDS.params.PanelID + ' #btn-add').hide();
            }
            else
            {
                Clinical_CDS.CDSSearch();
            }
             //End//10-03-2016//Ahmad Raza//logic to open CDSSearch popup from Facesheet
            //End 04-03-2016 Humaira Yousaf to load CDS
        }


        //Start//24-02-2016//Ahmad Raza//Toggle switch logic
        //var HtmlOfSwitch = '<div id="divSwitch"><span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
        //                 '<input id="switchActive" isactive="1" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="">' +
        //                  '</div><span class="pl-xs">Active</span> </div>';

      //  $("#" + Clinical_CDS.params.PanelID + ' #pnlCDS_Result div:first').html(HtmlOfSwitch);
          // Clinical_CDS.domreadyFunctions();
        //End//24-02-2016//Ahmad Raza//Toggle switch logic




    },

    //Function Name: activeCDSSearch
    //Author Name: Ahmad Raza
    //Created Date: 19-04-2016
    //Description: function to search CDS active/inactive
    activeCDSSearch: function (obj) {

        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Clinical_CDS.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Clinical_CDS.Switch = 1;
        }

        Clinical_CDS.CDSSearch();

    },

    //Function Name: showHistory
    //Author Name: Ahmad Raza
    //Created Date: 20-04-2016
    //Description: function to show history log
    showHistory: function (cdsId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        EMRUtility.showCurrentItemHistory(Clinical_CDS.params.PanelID, null, null, "CDS,CDSProblem,CDSMedication,CDSAllergy,CDSLabResult,CDSVitals", null, Clinical_CDS.params.TabID, cdsId);
    },
    domreadyFunctions: function () {
        $(function () {
            $("#" + Clinical_CDS.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });


            (function ($) {
                'use strict';
                $(function () {
                    $("#" + Clinical_CDS.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },
   //Start//24-02-2016//Ahmad Raza//plugin for Toggle switch
    readyFunction: function(){

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
    //This function will handle Add/Edit of CDS based on given CDSId
    CDSAddEdit: function (CDSId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (CDSId!=null && parseInt(CDSId) > 0) {
                    params["CDSId"] = CDSId;
                    params["mode"] = "Edit";
                }
                else {
                    params["CDSId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_CDS.params["FromAdmin"];
                //// params["ParentCtrl"] = Admin_Provider.params["ParentCtrl"];
                //if (Admin_Provider.params["FromAdmin"] == "0") {
                //    params["ParentCtrl"] = 'Clinical_CDS';
                //}
                params["ParentCtrl"] = 'adminTabCDS';
                params["TabID"] = 'ClinicalCDSDetail';

                LoadActionPan('ClinicalCDSDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ValidateCDS: function () {
        $('#frmClinicalCDS')
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
            Clinical_CDS.CDSSave();
        });
    },

    //Function Name: CDSSearch
    //Author Name: Humaira Yousaf
    //Created Date: 03-03-2016
    //Description: Searches CDS data
    //Params: cdsId, PageNo, rpp
    CDSSearch: function (cdsId, PageNo, rpp, CDSIDs, isPopup,isLoad) {
        var strMessage = "";


        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result").css("display") == "none") {
                    $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result").show();
                }
                Clinical_CDS.searchCDS(cdsId, PageNo, rpp, CDSIDs, isPopup, isLoad).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_CDS.CDSGridLoad(response);
                        var TableControl = Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS";
                        var PagingPanelControlID = Clinical_CDS.params.PanelID + " #dgvCDS_Paging";
                        var ClassControlName = "Clinical_CDS";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.CDSCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_CDS.CDSSearch(PrimaryID, PageNumber, ResultPerPage);
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
    //Function Name: CDSGridLoad
    //Author Name: Humaira Yousaf
    //Created Date: 03-03-2016
    //Description: Loads CDS data in grid
    //Params: response
    CDSGridLoad: function (response) {
        //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
        var isactive = $('#' + Clinical_CDS.params.PanelID + ' #pnlCDS_Result #divSwitch #switchActive').attr('isactive');
        //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS")) {
            $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS").dataTable().fnClearTable();
            $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS").dataTable().fnDestroy();
            $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS tbody").find("tr").remove();
        }
        //End 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable

    //    $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
    //    $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.CDSCount > 0) {
            var CDSLoadJSONData = JSON.parse(response.CDSLoad_JSON); //Parsing array to JSON
            $.each(CDSLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Clinical_CDS.CDSEdit('" + item.CDSId + "',event);");
                $row.attr("id", "gvCDS_row" + item.CDSId);
                $row.attr("CDSId", item.CDSId);
                if (item.IsActive == "True") {
                    //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
                    //  isactive = 0;
                    //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
                    // isactive = 1;
                    //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                if (Clinical_CDS.params.TabID == "clinicalTabFaceSheet")
                {
                    $row.append('<td style="display:none;">' + item.CDSId + '</td><td>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDS.CDSEdit(\'' + item.CDSId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + item.Title + '</td><td>' + item.Developer + '</td><td>' + item.ReferenceURL + '</td><td>' + item.FundingSource + '</td><td>' + item.Release + '</td><td>' + item.RevisionDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.Comments + '</td>');
                }
                else
                {
                    $row.append('<td style="display:none;">' + item.CDSId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_CDS.CDSDelete(\'' + item.CDSId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDS.CDSEdit(\'' + item.CDSId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDS.CDSActiveInactive(\'' + item.CDSId + '\', ' + isactive + ',event);"><i class="' + tglclass + '"></i></a>&nbsp;<a href="#" class="btn-xs on-editing row-history mr-none btn" onclick="Clinical_CDS.showHistory(\'' + item.CDSId + '\',event);" title="Record History"><i class="fa fa-history blue"></i></a></td><td>' + item.Title + '</td><td>' + item.Developer + '</td><td>' + item.ReferenceURL + '</td><td>' + item.FundingSource + '</td><td>' + item.Release + '</td><td>' + item.RevisionDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.Comments + '</td>');
                }


                $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS tbody").last().append($row);
            });

            //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803

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
                             '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_CDS.activeCDSSearch(this);">' +
                              '</div><span class="pl-xs">Active</span>';

            $("#" + Clinical_CDS.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            EMRUtility.SwicthWidgetInializatoin();
            //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803

        }
        else {
            $("#" + Clinical_CDS.params.PanelID + ' #pnlCDS_Result #dgvCDS').DataTable({"destroy": true,
                "language": {
                    "emptyTable": "No CDS Found"
                }, "autoWidth": false, "bLengthChange": false,"aaSorting": [[ 1, "desc" ]],  "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_CDS.params.PanelID + ' #pnlCDS_Result #dgvCDS'))
            ;
        else {
            $("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS").DataTable({"destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false,  "autoWidth": false,"aaSorting": [[ 1, "desc" ]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
        //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
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
                         '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_CDS.activeCDSSearch(this);">' +
                          '</div><span class="pl-xs">Active</span>';

        $("#" + Clinical_CDS.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();
        //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803

    },
    //Function Name: searchCDS
    //Author Name: Humaira Yousaf
    //Created Date: 03-03-2016
    //Description: Searches CDS
    //Params: CDSId, PageNumber, RowsPerPage
    searchCDS: function (CDSId, PageNumber, RowsPerPage, CDSIDs, isPopup, isLoad) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();

        objData["CDSId"] = CDSId;
        objData["CDSIDs"] = CDSIDs;
        objData["isPopup"] = isPopup;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_CDS.patientId;
        if (isLoad != "" && isLoad != null) {
            objData["CDSIDs"] = CDSId;
        }
        if (Clinical_CDS.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }
        objData["commandType"] = "search_cds";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");

    },

    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_CDS.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(Clinical_CDS.params.PanelID + ' #frmClinicalCDS', function () {
                if (Clinical_CDS.params["FromAdmin"] == "0") {
                    if (Clinical_CDS.params != null && Clinical_CDS.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_CDS.params.ParentCtrl, 'Clinical_CDS');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_CDS');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_CDS.params["FromAdmin"] == "0") {
                    if (Clinical_CDS.params != null && Clinical_CDS.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_CDS.params.ParentCtrl, 'Clinical_CDS');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_CDS');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Clinical_CDS.params["FromAdmin"] == "0") {
                if (Clinical_CDS.params != null && Clinical_CDS.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_CDS.params.ParentCtrl, 'Clinical_CDS');
                }
                else
                    UnloadActionPan(null, 'Clinical_CDS');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },
    //Function Name: CDSEdit
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Opens CDS popup to edit data
    //Params: CDSId, event
    CDSEdit: function (CDSId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Clinical_CDS.params.PanelID + " #pnlCDS_Result #dgvCDS #gvCDS_row" + CDSId));
        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = CDSId;
                if (Clinical_CDS.params.TabID == "clinicalTabFaceSheet") {
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var params = [];
                        params["CDSId"] = selectedValue;
                        params["mode"] = "Edit";
                        params["FromAdmin"] = 0;
                        params["ParentCtrl"] = "Clinical_CDS";
                        LoadActionPan('Clinical_CDSAlertDetail', params);
                    }
                }
                else {
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var params = [];
                        params["CDSId"] = selectedValue;
                        params["mode"] = "Edit";
                        LoadActionPan('ClinicalCDSDetail', params);
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


//Start//24-02-2016//Ahmad Raza//plugin for Toggle switch
domReadyFunction: function(){

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

    //Function Name: CDSActiveInactive
    //Author Name: Ahmad Raza
    //Created Date: 19-04-2016
    //Description: function to Active/InActive CDS
CDSActiveInactive: function (id, isActive, event) {
    if (event != null) {
        event.stopPropagation();
    }
    //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    var strMessage = "";
    // var id = id;//$row.attr("id");
    var ValueForAlert = null;
    if (isActive == "1" || isActive == undefined) {
        ValueForAlert = "0";
    } else {
        ValueForAlert = "1"
    }
    AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = id;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var IsActiveRecord = null;
                    //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
                    IsActiveRecord = $('#' + Clinical_CDS.params.PanelID + ' #pnlCDS_Result #switchActive').attr('isactive');
                    //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
                    if (IsActiveRecord == "1") {

                        Clinical_CDS.activeInActiveCDS(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Clinical_CDS.CDSSearch();
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                        });

                    } else {
                        IsActiveRecord = "0";
                        Clinical_CDS.activeInActiveCDS(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Clinical_CDS.CDSSearch();
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                        });
                    }




                }
            }, function () { },
              '3', null, null, null, ValueForAlert
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
    });
    //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
},

    //Function Name: activeInActiveCDS
    //Author Name: Ahmad Raza
    //Created Date: 19-04-2016
    //Description: function to call DB to Active/InActive CDS
activeInActiveCDS: function (ProblemListId) {

    var IsCheckedIn = null;
    var isActive = null;
    //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
    IsCheckedIn = $('#' + Clinical_CDS.params.PanelID + ' #pnlCDS_Result #switchActive').attr('isactive');
    //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
    if (IsCheckedIn == "1") {
        isActive = false;
    }
    else if (IsCheckedIn == "0") {
        isActive = true;
    }
    var objData = new Object();
    objData["CDSId"] = ProblemListId;
    objData["IsActive"] = isActive;
    objData["commandType"] = "active_inactive_cds";

    var data = JSON.stringify(objData);
   return MDVisionService.APIService(data, "CDS", "CDS");

},

     //Function Name: CDSDelete
    //Author Name: Ahmad Raza
    //Created Date: 19-04-2016
    //Description: function to delete CDS
CDSDelete: function (id, obj) {
    if (obj != null) {
        obj.stopPropagation();
    }
    //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    var strMessage = "";
   // var id = $row.attr("id");
    AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = id;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    //Start//15/1/2016//M Ahmad Imran//DrFirst Integration Changes

                    //End//15/1/2016//M Ahmad Imran//DrFirst Integration Changes
                    Clinical_CDS.deleteCDS(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                        //Start//28/12/2015//Ahmad Raza// No Known ProblemList hyperlink(link not visible when there is no problem list) issue fixed
                        Clinical_CDS.CDSSearch();
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

    //Function Name: deleteCDS
    //Author Name: Ahmad Raza
    //Created Date: 19-04-2016
    //Description: function to call DB to delete CDS
deleteCDS: function (ProblemListId) {
    var isActive = null;
    //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
    IsCheckedIn = $('#' + Clinical_CDS.params.PanelID + ' #pnlCDS_Result #switchActive').attr('isactive');
    //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-803
    var objData = new Object();
    if (IsCheckedIn == "1") {
        isActive = true;
    }
    else if (IsCheckedIn == "0") {
        isActive = false;
    }
    objData["CDSId"] = ProblemListId;
    objData["IsActive"] = isActive;
    objData["commandType"] = "DELETE_CDS";

    var data = JSON.stringify(objData);
    return MDVisionService.APIService(data, "CDS", "CDS");

},






}