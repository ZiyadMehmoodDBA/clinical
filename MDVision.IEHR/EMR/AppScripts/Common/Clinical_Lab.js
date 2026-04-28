Clinical_Lab = {
    //Author: Abid Ali
    //Date :  31-03-2016
    //This file will handle all actions performed for Lab Order
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,
    Switch: 1,
    //Author: Abid Ali
    //Date :  31-03-2016
    //This function will handle fill of Lab Order 
    Load: function (params) {
        Clinical_Lab.params = params;
        Clinical_Lab.patientId = $("div#PatientProfile #hfPatientId").val();
        Clinical_Lab.params.mode = "Add";

        if (Clinical_Lab.params.PanelID != 'pnlClinicalLab') {
            Clinical_Lab.params.PanelID = Clinical_Lab.params.PanelID + ' #pnlClinicalLab';
        } else {
            Clinical_Lab.params.PanelID = 'pnlClinicalLab';
        }
        var self = $('#' + Clinical_Lab.params.PanelID);

        if (Clinical_Lab.bIsFirstLoad == true) {
        }
        //Load All Labs in grid

        self.loadDropDowns(true).done(function () {
            Clinical_Lab.LabSearch(0);
            Clinical_Lab.readyFunction();
        });

    },

    LoadLabType: function () {

        return Clinical_Lab.searchLab(null, 0, 1, 5000).done(function (response) {
            //Populate Distinct Values in typeArray
            response = JSON.parse(response);
            var typeArray = new Array();
            var data = JSON.parse(response.ClinicalLab_JSON);
            $.each(data, function (i, row) {
                if (jQuery.inArray(row.Type, typeArray) === -1) {
                    typeArray.push(row.Type);
                }
            });
            var ddType = $('#' + Clinical_Lab.params.PanelID + " #ddlLabType");
            ddType.empty();
            ddType.append($("<option />").val("").text('-All-'));
            if (typeArray.length > 0) {
                $.each(typeArray, function () {
                    ddType.append($("<option />").val(this).text(this));
                });
            }
        });
    },

    readyFunction: function () {

        $(function () {
            //$("#" + Clinical_Lab.params.PanelID + ' [data-plugin-toggle]').each(function () {
            //    var $this = $(this),
            //        opts = {};

            //    var pluginOptions = $this.data('plugin-options');
            //    if (pluginOptions)
            //        opts = pluginOptions;

            //    $this.themePluginToggle(opts);
            //});


            (function ($) {
                'use strict';
                $(function () {
                    $("#" + Clinical_Lab.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //This function will handle Add/Edit of Lab
    LabAddEdit: function (LabId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Laboratory", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (LabId != null && parseInt(LabId) > 0) {
                    params["LabId"] = LabId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_Lab.params["FromAdmin"];
                //// params["ParentCtrl"] = Admin_Provider.params["ParentCtrl"];
                //if (Admin_Provider.params["FromAdmin"] == "0") {
                //    params["ParentCtrl"] = 'Clinical_Lab';
                //}
                params["ParentCtrl"] = 'adminTabLab';
                params["TabID"] = 'ClinicalLabDetail';

                LoadActionPan('ClinicalLabDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Searches Lab Orders
    //Params: LabId, PageNo, rpp
    LabSearch: function (LabId, PageNo, rpp) {
        var strMessage = "";

        if ($("#" + Clinical_Lab.params.PanelID + " #pnlLab_Result").css("display") == "none") {
            $("#" + Clinical_Lab.params.PanelID + " #pnlLab_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Laboratory", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + Clinical_Lab.params.PanelID + " form");
                var myJSON = self.getMyJSONByName();

                Clinical_Lab.searchLab(myJSON, LabId, PageNo, rpp).done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {

                        Clinical_Lab.LabGridLoad(response);

                        var TableControl = Clinical_Lab.params.PanelID + " #pnlLab_Result #dgvLab";
                        var PagingPanelControlID = Clinical_Lab.params.PanelID + " #dgvLab_Paging";
                        var ClassControlName = "Clinical_Lab";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.ClinicalLabCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_Lab.LabSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);

                        //Load All Types
                        //Clinical_Lab.LoadLabType();
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads Lab Orders data in grid
    //Params: response
    LabGridLoad: function (response) {
        $("#" + Clinical_Lab.params.PanelID + " #pnlLab_Result #dgvLab").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#" + Clinical_Lab.params.PanelID + " #pnlLab_Result #dgvLab tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.ClinicalLabCount > 0) {
            var LabLoadJSONData = JSON.parse(response.ClinicalLab_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("onclick", "ClinicalLabDetail.LabEdit('" + item.LabId + "',event);");
                $row.attr("id", "gvLab_row" + item.LabId);
                $row.attr("LabId", item.LabId);


                if (item.IsActive == "True") {
                    isactive = 1
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 0
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var ActiveInactiveMethod = "Clinical_Lab.ActiveInactiveLab(" + item.LabId.trim() + "," + isactive + ",event);";

                $row.append('<td style="display:none;">' + item.LabId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_Lab.LabDelete(\'' + item.LabId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ClinicalLabDetail.LabEdit(\'' + item.LabId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + ActiveInactiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>'
                   + item.Name + '</td><td>' + item.LabTypeName + '<td>' + item.ModifiedOn + "<br/>" + item.ModifiedByName);
                $("#" + Clinical_Lab.params.PanelID + " #pnlLab_Result #dgvLab tbody").last().append($row);

                
            });
        }
        else {
            //Initialize data table with no record added
            $("#" + Clinical_Lab.params.PanelID + ' #pnlLab_Result #dgvLab').DataTable({
                "language": {
                    "emptyTable": "No Laboratory Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Searches Lab Orders
    //Params: LabData, LabId, PageNumber, RowsPerPage
    searchLab: function (LabData, LabId, PageNumber, RowsPerPage, IsActive) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        //Safty checks
        var objData = {};
        if (LabData != null)
            objData = JSON.parse(LabData);
        if (LabId == null || LabId == "")
            LabId = -1;

        objData["LabId"] = LabId;

        if (IsActive != null) {
            objData["IsActive"] = IsActive;
        }
        else {
            if (Clinical_Lab.Switch == 1) {
                objData["IsActive"] = 'True';
            }
            else {
                objData["IsActive"] = 'False';
            }
        }
        
        if (Clinical_Lab.params.FromAdmin == '1' && Clinical_Lab.params.TabID == 'adminTabLab') {
            objData["FormName"] = "Laboratory";
            objData["moduleName"] = "Admin";
        }

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_ClinicalLab";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLab");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Lab Order Reset
    LabReset: function () {

        utility.myConfirm('22', function () {
            $('#' + Clinical_Lab.params.PanelID + ' #frmClinicalLab').resetAllControls(null);
            Clinical_Lab.ValidateLab();
        }, function () { },
            '22'
        );
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Unload Lab
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_Lab.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(Clinical_Lab.params.PanelID + ' #frmClinicalCDS', function () {



                if (Clinical_Lab.params["FromAdmin"] == "0") {
                    if (Clinical_Lab.params != null && Clinical_Lab.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_Lab.params.ParentCtrl, 'Clinical_Lab');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_Lab');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_Lab.params["FromAdmin"] == "0") {
                    if (Clinical_Lab.params != null && Clinical_Lab.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_Lab.params.ParentCtrl, 'Clinical_Lab');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_Lab');
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
            if (Clinical_Lab.params["FromAdmin"] == "0") {
                if (Clinical_Lab.params != null && Clinical_Lab.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_Lab.params.ParentCtrl, 'Clinical_Lab');
                }
                else
                    UnloadActionPan(null, 'Clinical_Lab');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Delete the selected Lab Order
    LabDelete: function (LabId, event, PageNo, rpp) {
        var strMessage = "";
        event.stopPropagation();
        AppPrivileges.GetFormPrivileges("Laboratory", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        // var self = $("#" + Clinical_Lab.params.PanelID + " form");
                        // var myJSON = self.getMyJSONByName();

                        Clinical_Lab.deleteLab(LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_Lab.LabSearch("", 1, 15);
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to Delete the selected Lab Order
    deleteLab: function (LabId) {

        var objData = {};
        objData["LabId"] = LabId;

        objData["commandType"] = "delete_ClinicalLab";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLab");

    },

    activeClincialLabSearch: function (obj) {

        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Clinical_Lab.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Clinical_Lab.Switch = 1;
        }

        Clinical_Lab.LabSearch(0);
    },

    ActiveInactiveLab: function (labId, IsActive, event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        var ValueForAlert = null;
        if (IsActive == "1" || IsActive == undefined) {
            ValueForAlert = "0";
        } else {
            ValueForAlert = "1"
        }
        utility.SelectGridRow($("#gvLab_row" + labId));
        AppPrivileges.GetFormPrivileges("Laboratory", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = labId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Clinical_Lab.labUpdateActiveInactive(selectedValue, IsActive).done(function (response) {

                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_Lab.LabSearch(0);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                     '3', null, null, null, ValueForAlert
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    labUpdateActiveInactive: function (labId, IsActive) {

        var objData = {};

        objData["LabId"] = labId;
        objData["commandType"] = "Active_Inactive_ClinicalLab";

        if (IsActive === 1)
            objData["IsActive"] = "False";
        else
            objData["IsActive"] = "True";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLab");
    },
}