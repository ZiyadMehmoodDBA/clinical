Clinical_Implantable = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,
    orderSearchGridId: 'dgvClinicalImplantable',
    Load: function (params) {
        Clinical_Implantable.params = params;

        if (Clinical_Implantable.params.PanelID != 'pnlClinicalImplantable') {
            Clinical_Implantable.params.PanelID = Clinical_Implantable.params.PanelID + ' #pnlClinicalImplantable';
        } else {
            Clinical_Implantable.params.PanelID = 'pnlClinicalImplantable';
        }

        if (Clinical_Implantable.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_Implantable.params.PanelID + " div#FaceSheetPager", Clinical_Implantable.params.FaceSheetComponents, 'implantabledevices');
        } else if (Clinical_Implantable.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalImplantable' + " div#FaceSheetPager", Clinical_Implantable.params.FaceSheetComponents, 'implantabledevices');
        }

        if (Clinical_Implantable.params.ParentCtrl == 'clinicalTabProgressNote') {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Implantable.params.PanelID, 'Miscellaneous', 'ImplantableDevices', 'Clinical_Implantable.UnLoadTab();', 'frmClinicalImplantable');
            $("#" + Clinical_Implantable.params.PanelID + " #btnAddDevicesToNote").show();
            $("#" + Clinical_Implantable.params.PanelID + " #btnAddDevicesToNote").prop("disabled", true);
        }
        else {
            $("#" + Clinical_Implantable.params.PanelID + " #btnAddDevicesToNote").hide();
        }

        var self = $('#' + Clinical_Implantable.params.PanelID);

        Clinical_Implantable.ImplantableSearch('0');
        Clinical_Implantable.domreadyFunctions();

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1412px");
            }
        });
    },

    ImplantableSearch: function (ImplantableDevicesPKId, pageNo, rpp) {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Clinical_Implantable.params["PanelID"] + " #pnlClinicalImplantable_Result").css("display") === "none")
                    $("#" + Clinical_Implantable.params["PanelID"] + " #pnlClinicalImplantable_Result").show();

                Clinical_Implantable.SearchImplantable(ImplantableDevicesPKId, pageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (Clinical_Implantable.params.ParentCtrl == "clinicalTabProgressNote") {
                            if ($("#" + Clinical_Implantable.params.PanelID + " #dgvClinicalImplantable thead tr #SelectRecord").length == 0) {
                                $("#" + Clinical_Implantable.params.PanelID + " #dgvClinicalImplantable thead tr").prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderImplantableDevices" onchange="Clinical_Implantable.checkUncheckAllDevices(this);"   class="pull-left" coltype="checkbox"/> </th>');
                            }
                        } else {
                            $("#" + Clinical_Implantable.params.PanelID + " #dgvClinicalImplantable th#SelectRecord").remove();
                        }
                        Clinical_Implantable.ImplantableGridLoad(response);
                        response = JSON.parse(response);
                        var tableControl = Clinical_Implantable.params["PanelID"] + " #dgvClinicalImplantable";
                        var pagingPanelControlId = Clinical_Implantable.params["PanelID"] + " #divClinicalImplantablePaging";
                        var classControlName = "Clinical_Implantable";
                        var pagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout
                        (
                            CreatePagination(response.implantableDevicesCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                                function (primaryId, pageNumber, resultPerPage) {
                                    Clinical_Implantable.ImplantableSearch(primaryId, pageNumber, resultPerPage);
                                }
                            ), 10);
                    } else {
                        $("#" + Clinical_Implantable.params["PanelID"] + " #dgvClinicalImplantable").dataTable().fnDestroy();
                        $("#" + Clinical_Implantable.params["PanelID"] + " #pnlClinicalImplantable_Result #dgvClinicalImplantable tbody").find("tr").remove();
                        $("#" + Clinical_Implantable.params["PanelID"] + " #divClinicalImplantablePaging").remove();

                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchImplantable: function (ImplantableDevicesPKId, pageNumber, rowsPerPage) {
        if (pageNumber == null)
            pageNumber = 1;
        if (rowsPerPage == null)
            rowsPerPage = 15;
        var params = {};
        params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
        params["PatientId"] = Clinical_Implantable.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : (Clinical_Implantable.params.ParentCtrl == "clinicalTabFaceSheet" ? Clinical_Implantable.params.PatientId : Clinical_Implantable.params.patientID);
        params["IsActive"] = $("#" + Clinical_Implantable.params.PanelID + " #deviceSwitchActive").attr('isactive');
        params["NotesId"] = Clinical_Implantable.params.ParentCtrl == "clinicalTabProgressNote" ? Clinical_Implantable.params.NotesId : "0";
        params["PageNumber"] = pageNumber;
        params["RowsPerPage"] = rowsPerPage;
        params["commandType"] = "load_ImplantableDevices";
        var data = JSON.stringify(params);
        return MDVisionService.APIService(data, "Implantable", "Implantable");
    },

    ImplantableGridLoad: function (response) {

        if ($.fn.dataTable.isDataTable("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #" + Clinical_Implantable.orderSearchGridId)) {
            $("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #" + Clinical_Implantable.orderSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #" + Clinical_Implantable.orderSearchGridId).dataTable().fnDestroy();
        }

        $("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #" + Clinical_Implantable.orderSearchGridId + " tbody").find("tr").remove();
        response = JSON.parse(response);
        if (response.implantableDevicesCount > 0) {
            var devicesAttachedWithNotesJSONData = JSON.parse(response.listNotesLinked);
            var implantableDevicesLoadJSONData = JSON.parse(response.listImplantableDevices);
            $.each(implantableDevicesLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvImplantable_row" + item.ImplantableDevicesPKId);
                $row.attr("ImplantableId", item.ImplantableDevicesPKId);
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

                if (Clinical_Implantable.params.ParentCtrl == "clinicalTabProgressNote") {
                    var SelectionCheckBoxColumn = "";
                    var Checked = "";
                    if (devicesAttachedWithNotesJSONData.length != 0) {
                        Clinical_ProgressNote.AttachedNoteComponentIds = $(devicesAttachedWithNotesJSONData).map(function () {
                            return this.ImplantableDevicesPKId;
                        }).get();

                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ImplantableDevicesPKId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1)
                            Checked = " ";
                        else
                            Checked = " checked";
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ImplantableDevicesPKId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1)
                            Checked = " checked";
                        else
                            Checked = "";
                        SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_Implantable.enableAddDevice(this,event);" id="' + item.ImplantableDevicesPKId + '" name="SelectCheckBoxDevicesList" ' + Checked + ' class="input-block"/></td>';
                        $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.ImplantableDevicesPKId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceDelete(\'' + item.ImplantableDevicesPKId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceEdit(\'' + item.ImplantableDevicesPKId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceActiveInactive(\'' + item.ImplantableDevicesPKId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.DI + '</td><td>' + item.GMDNPName + '</td><td>' + item.Status + '</td><td>' + item.TargetSite + '</td><td>' + item.BrandName + '</td><td>' + item.Serial_Number + '</td><td>' + item.Lot_Number + '</td><td>' + item.Manufacturing_Date + '</td><td>' + item.Expiration_Date + '</td>');
                        $("#pnlClinicalImplantable_Result #dgvClinicalImplantable tbody").last().append($row);
                        $("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #dgvClinicalImplantable tbody").last().append($row);
                    }
                    else {
                        SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_Implantable.enableAddDevice(this,event);" id="' + item.ImplantableDevicesPKId + '" name="SelectCheckBoxDevicesList" ' + Checked + ' class="input-block"/></td>';
                        $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.ImplantableDevicesPKId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceDelete(\'' + item.ImplantableDevicesPKId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceEdit(\'' + item.ImplantableDevicesPKId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceActiveInactive(\'' + item.ImplantableDevicesPKId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.DI + '</td><td>' + item.GMDNPName + '</td><td>' + item.Status + '</td><td>' + item.TargetSite + '</td><td>' + item.BrandName + '</td><td>' + item.Serial_Number + '</td><td>' + item.Lot_Number + '</td><td>' + item.Manufacturing_Date + '</td><td>' + item.Expiration_Date + '</td>');
                        $("#pnlClinicalImplantable_Result #dgvClinicalImplantable tbody").last().append($row);
                        $("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #dgvClinicalImplantable tbody").last().append($row);
                    }
                }
                else {
                    $row.append('<td style="display:none;">' + item.ImplantableDevicesPKId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceDelete(\'' + item.ImplantableDevicesPKId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceEdit(\'' + item.ImplantableDevicesPKId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Implantable.ImplantableDeviceActiveInactive(\'' + item.ImplantableDevicesPKId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.DI + '</td><td>' + item.GMDNPName + '</td><td>' + item.Status + '</td><td>' + item.TargetSite + '</td><td>' + item.BrandName + '</td><td>' + item.Serial_Number + '</td><td>' + item.Lot_Number + '</td><td>' + item.Manufacturing_Date + '</td><td>' + item.Expiration_Date + '</td>');
                    $("#pnlClinicalImplantable_Result #dgvClinicalImplantable tbody").last().append($row);
                    $("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #dgvClinicalImplantable tbody").last().append($row);
                }
            });
        }
        else {
            $("#" + Clinical_Implantable.params.PanelID + ' #pnlClinicalImplantable_Result #dgvClinicalImplantable').DataTable({
                "bdestroy": true,
                "language": {
                    "emptyTable": "No Implantable Device Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_Implantable.params.PanelID + ' #pnlClinicalImplantable_Result #dgvClinicalImplantable'))
            ;
        else {
            $("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result #dgvClinicalImplantable").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_Implantable.params.PanelID + " #pnlClinicalImplantable_Result");
    },

    ImplantableDeviceEdit: function (ImplantableDevicesPKId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Clinical_Implantable.params.ParentCtrl == 'clinicalTabProgressNote') {
                    var params = [];
                    params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
                    params["PatientID"] = Clinical_Implantable.params.patientID;
                    params["ParentCtrl"] = "Clinical_Implantable";
                    params["ParentCtrlPanelID"] = Clinical_Implantable.params.PanelID;
                    params["FromNoteId"] = Clinical_Implantable.params.NotesId;
                    params["TabID"] = "Clinical_ImplantableDetail";
                    params["mode"] = "Edit";
                    params["FromAdmin"] = 0;
                    LoadActionPan('Clinical_ImplantableDetail', params, Clinical_Implantable.params.PanelID);
                }
                else if (Clinical_Implantable.params.ParentCtrl == 'clinicalTabFaceSheet' || Clinical_Implantable.params.ParentCtrl == 'Clinical_FaceSheet') {
                    var params = [];
                    params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
                    params["PatientID"] = Clinical_Implantable.params.PatientId;
                    params["ParentCtrl"] = "Clinical_Implantable";
                    params["ParentCtrlPanelID"] = Clinical_Implantable.params.PanelID;
                    params["TabID"] = "Clinical_ImplantableDetail";
                    params["mode"] = "Edit";
                    params["FromAdmin"] = 0;
                    LoadActionPan('Clinical_ImplantableDetail', params, Clinical_Implantable.params.PanelID);
                }
                else {
                    var params = [];
                    params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
                    params["PatientID"] = Clinical_Implantable.params.patientID;
                    params["TabID"] = "Clinical_ImplantableDetail";
                    params["mode"] = "Edit";
                    params["FromAdmin"] = 0;
                    params["ParentCtrl"] = 'clinicalTabImplantable';
                    LoadActionPan('Clinical_ImplantableDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ImplantableDeviceDelete: function (ImplantableDevicesPKId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var ImplantableDeviceId = ImplantableDevicesPKId;
                    if (ImplantableDeviceId == "" || ImplantableDeviceId == "undefined") {
                    }
                    else {
                        Clinical_Implantable.DeleteImplantableDevice(ImplantableDeviceId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status == true) {
                                Clinical_Implantable.ImplantableSearch();
                                utility.DisplayMessages(response.message, 1);
                            }
                            else
                                utility.DisplayMessages(response.message, 3);

                        });
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DeleteImplantableDevice: function (ImplantableDeviceId) {
        var params = {};
        params["ImplantableDevicesPKId"] = ImplantableDeviceId;
        params["commandType"] = "delete_ImplantableDevice";
        var data = JSON.stringify(params);
        return MDVisionService.APIService(data, "Implantable", "Implantable");
    },

    checkUncheckAllDevices: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Implantable.params.PanelID + " [name='SelectCheckBoxDevicesList']").prop("checked", true);
        } else {
            $("#" + Clinical_Implantable.params.PanelID + " [name='SelectCheckBoxDevicesList']").prop("checked", false);
        }
        $("#" + Clinical_Implantable.params.PanelID + " #dgvClinicalImplantable tbody").find('input[type="checkbox"]').each(function () {
            Clinical_Implantable.enableAddDevice(this);
        });
    },

    ImplantableAdd: function (ImplantableDevicesPKId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Clinical_Implantable.params.ParentCtrl == 'clinicalTabProgressNote') {
                    var params = [];
                    params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
                    params["PatientID"] = Clinical_Implantable.params.patientID;
                    params["ParentCtrl"] = "Clinical_Implantable";
                    params["ParentCtrlPanelID"] = Clinical_Implantable.params.PanelID;
                    params["FromNoteId"] = Clinical_Implantable.params.NotesId;
                    params["TabID"] = "Clinical_ImplantableDetail";
                    params["mode"] = "Add";
                    params["FromAdmin"] = 0;
                    LoadActionPan('Clinical_ImplantableDetail', params, Clinical_Implantable.params.PanelID);
                }
                else if (Clinical_Implantable.params.ParentCtrl == 'clinicalTabFaceSheet' || Clinical_Implantable.params.ParentCtrl == 'Clinical_FaceSheet') {
                    var params = [];
                    params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
                    params["PatientID"] = Clinical_Implantable.params.PatientId;
                    params["ParentCtrl"] = "Clinical_Implantable";
                    params["ParentCtrlPanelID"] = Clinical_Implantable.params.PanelID;
                    params["TabID"] = "Clinical_ImplantableDetail";
                    params["mode"] = "Add";
                    params["FromAdmin"] = 0;
                    LoadActionPan('Clinical_ImplantableDetail', params, Clinical_Implantable.params.PanelID);
                }
                else {
                    var params = [];
                    params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
                    params["PatientID"] = Clinical_Implantable.params.patientID;
                    params["TabID"] = "Clinical_ImplantableDetail";
                    params["mode"] = "Add";
                    params["FromAdmin"] = 0;
                    params["ParentCtrl"] = 'clinicalTabImplantable';
                    LoadActionPan('Clinical_ImplantableDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ImplantableDeviceActiveInactive: function (ImplantableDevicesPKId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var ImplantableDeviceId = ImplantableDevicesPKId;
                    if (ImplantableDeviceId == "" || ImplantableDeviceId == "undefined") {
                    }
                    else {
                        Clinical_Implantable.UpdateImplantableDevice(ImplantableDeviceId, IsActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_Implantable.ImplantableSearch();
                                utility.DisplayMessages(response.message, 1);
                            }
                            else
                                utility.DisplayMessages(response.message, 3);
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

    ActiveDeviceSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
            if (Clinical_Implantable.params.ParentCtrl == 'clinicalTabProgressNote') {
                $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlClinicalImplantable_Result #chkHeaderImplantableDevices').prop('checked', true);
                $("#" + Clinical_Implantable.params.PanelID + " #btnAddDevicesToNote").prop('disabled', true);
            }      
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        Clinical_Implantable.ImplantableSearch();
    },

    enableAddDevice: function (obj) {
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
            $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlClinicalImplantable_Result #chkHeaderImplantableDevices').prop('checked', false);
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }
        }
        if ($("#" + Clinical_Implantable.params.PanelID + " #deviceSwitchActive").attr('isactive') == 1) {
            if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
                $("#" + Clinical_Implantable.params.PanelID + " #btnAddDevicesToNote").prop('disabled', false);
            } else {
                $("#" + Clinical_Implantable.params.PanelID + " #btnAddDevicesToNote").prop('disabled', true);
            }
        }
        else {
            $("#" + Clinical_Implantable.params.PanelID + " #btnAddDevicesToNote").prop('disabled', true);
        }
        
    },

    UpdateImplantableDevice: function (ImplantableDevicesPKId, IsActive) {
        var params = {};
        params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
        params["IsActive"] = IsActive;
        params["commandType"] = "activeInactive_ImplantableDevice";
        var data = JSON.stringify(params);
        return MDVisionService.APIService(data, "Implantable", "Implantable");
    },

    UnLoadTab: function () {
        var parentPanelId = null;
        if (Clinical_Implantable.params.ParentCtrl == "Clinical_FaceSheet") {
            parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
            Clinical_FaceSheet.params.ChildPanelID = null;
            UnloadActionPan(Clinical_Implantable.params.ParentCtrl, 'Clinical_Implantable', null, parentPanelId);
        } else {
            if (Clinical_Implantable.params.FromAdmin == "0") {
                UnloadActionPan(Clinical_Implantable.params["ParentCtrl"], "Clinical_Implantable");
            }
            else {
                RemoveAdminTab();
            }
        }
    },

    checkDevicesExists: function (ImplantableDevicesPKIds) {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_implantabledevices').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #MiscellaneousNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
            var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab(\'ImplantableDevices\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');';
            (globalAppdata["isImplantableDevices"] && globalAppdata["isImplantableDevices"].toLowerCase() == "false") ? onClick = "" : "";
            $(CompnentSelector).append(' <li class="ImplantableDevicesComponent" NoteComponentId="NCDummyId"> <header>' +
                '<clinical_implantabledevices title="Implantable Devices"  id="' + this.id + '" class="NotesComponent">' +
                    '<a class="btn btn-link btn-xs" onClick=' + onClick + ' title="ImplantableDevices">Implantable Devices</a> ' +
                    '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Implantable Devices\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                    '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Implantable Devices\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                '</clinical_implantabledevices> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_implantabledevices').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },

    createDevicesBodyHTML: function (response, NoteHTMLCtrl, ImplantableDevicesPKIds, hideAlertMessage, dontshowPhqPopup, phqtextneeded) {
        Clinical_Implantable.checkDevicesExists(ImplantableDevicesPKIds);

        if (response.listImplantableDevices != null && response.listImplantableDevices != '') {
            var ImplantableDevicesoap_JSON = response.listImplantableDevices;
            var $mainDivVital = $(document.createElement('div'));

            if (ImplantableDevicesoap_JSON == null || ImplantableDevicesoap_JSON.length == 0) {
                Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices', hideAlertMessage);
                return "";
            }
            if (response.implantableDevicesCount > 0) {
                var IListId = [];
                $.each(ImplantableDevicesoap_JSON, function (index, element) {
                    var color = "";
                    var ILid = element.ImplantableDevicesPKId;
                    var $SectionBodyVital = $(document.createElement('section'));
                    $SectionBodyVital.attr('id', "Cli_ImplantableDevices_Main" + ILid);
                    var $DetailsDiv = $(document.createElement('div'));
                    $DetailsDiv.attr('id', "Cli_ImplantableDevices_" + ILid);
                    var $ListVital = $(document.createElement('ul'));

                    $ListVital.attr('class', 'list-unstyled')

                    $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ImplantableDevices_" + ILid + '"><i class="fa fa-edit"></i></a>' +
                        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ImplantableDevices_Main" + ILid + '"  ><i class="fa fa-times"></i></a></div> ');

                    $ListVital.append("<li>" +
                        (element.GMDNPName == '' ? "" : "<b>Device Name:</b> " + element.GMDNPName +", " )
                        + (element.TargetSite == '' ? "" : "<b>TargetSite:</b> " + element.TargetSite + ", ")
                        + (element.Procedure == '' ? "" : "<b>Procedure(s):</b> " + element.Procedures + ", ")
                        + (element.ImplantDate == '' ? "" : "<b>Implanted On:</b> " + utility.RemoveTimeFromDate(null, element.ImplantDate)) + "</li>");

                    //$ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments+"</li>");             

                    $DetailsDiv.append($ListVital);
                    $SectionBodyVital.append($DetailsDiv);
                    if ($(NoteHTMLCtrl + ' clinical_implantabledevices').parent().parent().find('#Cli_ImplantableDevices_Main' + ILid).length == 0) {
                        IListId.push(ILid);
                        $mainDivVital.append($SectionBodyVital);
                    } else {

                        var CommentHTML = "";
                        var CommentsID = $(NoteHTMLCtrl + ' clinical_implantabledevices').parent().parent().find('#Cli_ImplantableDevices_Main' + ILid + ' ul li:Last').attr('id');
                        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                            CommentHTML = $(NoteHTMLCtrl + ' clinical_implantabledevices').parent().parent().find('#Cli_ImplantableDevices_Main' + ILid + ' ul li:Last').get(0).outerHTML;
                        }
                        $(NoteHTMLCtrl + ' clinical_implantabledevices').parent().parent().find('#Cli_ImplantableDevices_Main' + ILid).html($SectionBodyVital.html());
                        $(NoteHTMLCtrl + ' clinical_implantabledevices').parent().parent().find('#Cli_ImplantableDevices_Main' + ILid + ' ul').append(CommentHTML);;
                    }

                });

                if (IListId.join(",") != "") {
                    ImplantableDevicesPKId = IListId.join(",");
                }

                if ($mainDivVital.html() != '') {
                    Clinical_Implantable.updateDevicesHtml($mainDivVital.html(), ImplantableDevicesPKId, NoteHTMLCtrl, hideAlertMessage);
                    //Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices', hideAlertMessage);
                } else {
                    Clinical_Implantable.updateDevicesHtml('', ImplantableDevicesPKId, NoteHTMLCtrl);
                    //Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices', hideAlertMessage);
                }
            }
        }
    },

    updateDevicesHtml: function (DevicesHtml, ImplantableDevicesPKId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt) {
        $(NoteHTMLCtrl + ' clinical_implantabledevices').parent().parent().addClass('initialVisitBody');
        if (DevicesHtml != '') {
            $(NoteHTMLCtrl + ' clinical_implantabledevices').parent().parent().append(DevicesHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (DevicesHtml != '') {
            Clinical_Implantable.attachDevicesFromNotes(ImplantableDevicesPKId, hideAlertMessage, bNotSaveCompt);
        }
    },

    attachDevicesFromNotes: function (ImplantableDevicesPKId, hideAlertMessage) {
        var strMessage = "";
        if (strMessage == "") {

            var selectedValue = ImplantableDevicesPKId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Implantable.attachDevicesWithNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices');
                        Clinical_ProgressNote.HideShowBillingInfo();
                        Clinical_Implantable.saveAssociatedProcedures(selectedValue);       //insert associated procedures in Clinical.Procedures table
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    attachDevicesWithNotes_DBCall: function (ImplantableDevicesPKIds) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ImplantableDevicesPKId"] = ImplantableDevicesPKIds;
        objData["commandType"] = "attach_devices_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "Implantable", "Implantable");
    },

    addDevicesToNotes: function () {
        var SelectedDevices = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
        if (SelectedDevices != null && SelectedDevices != '') {
            for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                var ILid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ImplantableDevices_Main' + ILid).length != 0) {
                    var index = SelectedDevices.indexOf(ILid);
                    if (index > -1) {
                        SelectedDevices.splice(index, 1);
                    }
                }
            }
        }
        
        var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
        if (detachedvalues.join() != '') {
            Clinical_Implantable.detachDevicesFromNotes(detachedvalues).done(function () {
                if (SelectedDevices.join() != null && SelectedDevices.join() != '') {
                    Clinical_Implantable.attachImplantableDevicesFromNotes(SelectedDevices);
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices');
                }
            });
        } else if (SelectedDevices.join() != null && SelectedDevices.join() != '') {
            Clinical_Implantable.getDevicesInfo(SelectedDevices.join());
        }

        if (Clinical_Implantable.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Clinical_Implantable.params.ParentCtrl, 'Clinical_Implantable');
        }
    },

    get_Devices_ForSOAP: function (ImplantableDevicesPKId) {
        var objData = new Object();
        objData["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
        objData["PatientId"] = Clinical_Implantable.params.patientID;
        objData["NoteProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "get_devices_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Implantable", "Implantable");
    },

    getDevicesInfo: function (ImplantableDevicesPKId, hideAlertMessage) {
        if (ImplantableDevicesPKId == null || ImplantableDevicesPKId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_Implantable.get_Devices_ForSOAP(ImplantableDevicesPKId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_Implantable.createDevicesBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', ImplantableDevicesPKId, hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    detachDevicesFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue;
                if (detachedvalues.indexOf("Cli_ImplantableDevices_Main") > -1) {
                    selectedValue = detachedvalues.replace('Cli_ImplantableDevices_Main', '');
                    utility.myConfirm('1', function () {

                        if (selectedValue == "" || selectedValue == "undefined") {
                            dfd.resolve('ok');
                        }
                        else {
                            Clinical_Implantable.detachDevicesFromNotes_DBCall(selectedValue).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    var ILid = selectedValue.split(',');

                                    for (var i = 0; i < ILid.length; i++) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ImplantableDevices_Main' + ILid[i]).remove();
                                    }
                                    Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices');
                                    Clinical_ProgressNote.HideShowBillingInfo();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                                dfd.resolve('ok');
                            });
                        }
                    }, function () { },
                                        '1'
                                    );
                }
                else {
                    selectedValue = detachedvalues.join(",");
                    if (selectedValue == "" || selectedValue == "undefined") {
                        dfd.resolve('ok');
                    }
                    else {
                        Clinical_Implantable.detachDevicesFromNotes_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                var ILid = selectedValue.split(',');

                                for (var i = 0; i < ILid.length; i++) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ImplantableDevices_Main' + ILid[i]).remove();
                                }

                                Clinical_ProgressNote.HideShowBillingInfo();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                            dfd.resolve('ok');
                        });
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        return dfd.promise();
    },

    detachDevicesFromNotes_DBCall: function (ImplantableDevicesPKId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
        objData["commandType"] = "detach_devices_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "Implantable", "Implantable");
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Clinical_Implantable.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Clinical_Implantable.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });
    },

    detach_ComponentsImplantableDevices: function (ComponentName, IsUpdate, ImplantableDevicesComponentRemove) {
        var Clinical_ImplantableDeviceIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_implantabledevices').parent().parent().find('section[id*="Cli_ImplantableDevices_Main"]').map(function () {
            return this.id.replace("Cli_ImplantableDevices_Main", "");
        }).get().join(',');

        if (ImplantableDevicesComponentRemove) {
            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_implantabledevices').parent().parent().attr('NoteComponentId');
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_implantabledevices').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_implantabledevices').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"]+ " #ActionsInitialOfficeVisit button[title='Implantable Device Lists']").remove();
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Implantable Device Lists']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_implantabledevices').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_implantabledevices').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_implantabledevices').parent().parent().find('section[id*="Cli_ImplantableDevices_Main"]').remove();
        }

        if (Clinical_ImplantableDeviceIds == "" || Clinical_ImplantableDeviceIds == "undefined") {
            Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            Clinical_Implantable.detachDevicesFromNotes_DBCall(Clinical_ImplantableDeviceIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices', true);
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

    detachImplantableDeviceFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue;
                if (detachedvalues.indexOf("Cli_ImplantableDevices_Main") > -1) {
                    selectedValue = detachedvalues.replace('Cli_ImplantableDevices_Main', '');
                    utility.myConfirm('1', function () {

                        EMRUtility.scrollToPNcomponent('clinical_implantabledevices');
                        if (selectedValue == "" || selectedValue == "undefined") {
                            dfd.resolve('ok');
                        }
                        else {
                            Clinical_Implantable.detachDevicesFromNotes_DBCall(selectedValue).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    var ILid = selectedValue.split(',');

                                    for (var i = 0; i < ILid.length; i++) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ImplantableDevices_Main' + ILid[i]).remove();
                                    }
                                    Clinical_ProgressNote.saveComponentSOAPText('Implantable Devices');
                                    Clinical_ProgressNote.HideShowBillingInfo();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                                dfd.resolve('ok');
                            });
                        }
                    }, function () { },
                                        '1'
                                    );
                }
                else {
                    selectedValue = detachedvalues.join(",");
                    if (selectedValue == "" || selectedValue == "undefined") {
                        dfd.resolve('ok');
                    }
                    else {
                        Clinical_Implantable.detachDevicesFromNotes_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                var ILid = selectedValue.split(',');

                                for (var i = 0; i < ILid.length; i++) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ImplantableDevices_Main' + ILid[i]).remove();
                                }
                                Clinical_ProgressNote.HideShowBillingInfo();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                            dfd.resolve('ok');
                        });
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        return dfd.promise();
    },

    attachImplantableDevicesFromNotes: function (SelectedDevices, hideAlertMessage) {
        Clinical_Implantable.getDevicesInfo(SelectedDevices.join(), hideAlertMessage).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                //Clinical_Implantable.saveAssociatedProcedures(SelectedDevices.join());
                if (Clinical_Implantable.params != null && Clinical_Implantable.params.PanelID != null && Clinical_Implantable.params.PanelID.indexOf('pnlClinicalImplantable') != -1) {
                    Clinical_Implantable.ImplantableSearch();
                }
            }, 5);
        });
    },

    saveAssociatedProcedures: function (ImplantableDeviceIds){
        Clinical_Implantable.saveAssociatedProcedures_DbCall(ImplantableDeviceIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ProcedureIds = response.ProcedureIds;
                if (ProcedureIds != "") {
                    Clinical_Implantable.attachAssociatedProceduresWithNote(ProcedureIds);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    saveAssociatedProcedures_DbCall: function (ImplantableDeviceIds) {
        var objData = {};
        objData["PatientId"] = Clinical_Implantable.params.patientID;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ImplantableDevicesPKId"] = ImplantableDeviceIds;
        objData["commandType"] = "save_associated_procedures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Implantable", "Implantable");
    },

    attachAssociatedProceduresWithNote: function (ProcedureIds) {
        Clinical_Implantable.attachAssociatedProceduresWithNote_DBCall(ProcedureIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    attachAssociatedProceduresWithNote_DBCall: function (ProcedureIds) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureId"] = ProcedureIds;
        objData["commandType"] = "attach_procedures_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

    //getLatestImplantableDeviceByPatientId: function (hideAlertMessage) {
    //    var strMessage = '';
    //    if (strMessage == "") {
    //        Clinical_Implantable.getLatestImplantableDeviceByPatientId_DBCall().done(function (response) {
    //            response = JSON.parse(response);
    //            if (response.status != false) {
    //                Clinical_Implantable.createDevicesBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
    //            }
    //            else {
    //                utility.DisplayMessages(strMessage, 3);
    //            }

    //        });
    //    }
    //    else {
    //        utility.DisplayMessages(strMessage, 3);
    //    }
    //},

    //getLatestImplantableDeviceByPatientId_DBCall: function () {
    //    var objData = new Object();
    //    if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
    //        objData["PatientId"] = 0;
    //    } else {
    //        objData["PatientId"] = Clinical_Notes.params.patientID;
    //    }
    //    objData["commandType"] = "getLatest_ImplantableDevice_ByPatientId";
    //    var data = JSON.stringify(objData);
    //    return MDVisionService.APIService(data, "Implantable", "Implantable");

    //},
}