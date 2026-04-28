//Author: ZeeshanAK
//Date: 16 April, 2016
//This file will handle all actions performed for Restrict user popup

Restrict_User = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,

    Load: function (params) {
        Restrict_User.params = params;

        if (Restrict_User.params.PanelID != 'pnlRestrictUser') {
            Restrict_User.params.PanelID = Restrict_User.params.PanelID + ' #pnlRestrictUser';
        } else {
            Restrict_User.params.PanelID = 'pnlRestrictUser';
        }
        var self = $('#' + Restrict_User.params.PanelID + " #frmPatientRestrictUser");
        self.loadDropDowns(true);

        Restrict_User.restrictUserLoad();
    },

    SelectUser:function(ThisCTRL){
        if ($(ThisCTRL).val() != "") {
            Restrict_User.restrictUserSave($(ThisCTRL).val());
        }
    },

    unload: function () {
        // UnloadActionPan(null, 'Restrict_User');
        if (Restrict_User.params != null && Restrict_User.params.ParentCtrl != "") {
            UnloadActionPan(Restrict_User.params.ParentCtrl);
        }
        else
            UnloadActionPan();
    },

    // To save a restricted user
    // Author: ZeeshanAK | Date: April 16, 2016
    restrictUserSave: function (userId) {
        Restrict_User.restrictUserSave_DBCall(userId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.restreicedUserId == null) {
                    utility.DisplayMessages("User already restricted!", 3);
                } else {
                    utility.DisplayMessages("Saved successfully!", 1);
                    Restrict_User.restrictUserLoad();

                }
            }
        });
    },

    // Restrict user load
    // Author: ZeeshanAK | Date: April 16, 2016
    restrictUserLoad: function () {
        Restrict_User.restrictUserLoad_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Restrict_User.restrictUserGridLoad(response);
            }
        });
    },

    // Grid load for restricted users
    // Author: ZeeshanAK | Date: April 16, 2016
    restrictUserGridLoad: function (response, pageNo, rpp) {
        if ($.fn.dataTable.isDataTable("#" + Restrict_User.params["PanelID"] + " #pnlRestrictUser_Result #dgvRestrictUser")) {
            $("#" + Restrict_User.params["PanelID"] + " #pnlRestrictUser_Result #dgvRestrictUser").dataTable().fnDestroy();
        }
        $("#" + Restrict_User.params["PanelID"] + " #pnlRestrictUser_Result #dgvRestrictUser tbody").find("tr").remove();

        if (response.restrictedUserCount > 0) {
            var restrictedUserLoadJsonData = JSON.parse(response.restrictedUserLoad_JSON);
            $.each(restrictedUserLoadJsonData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Restrict_User.DocumentPreview('" + item.PatientID + "', '" + item.PatDocId + "', '" + item.PatEducationId + "', event);utility.SelectGridRow($(this))");
                $row.attr("id", "dgvRestrictUserPatient" + item.PatUserBreakGlassId);
                $row.attr("UserId", item.UserId);
                var checked = '';
                var switchOnOff = '';

                if (item.IsBreakGlassAllow == "True") {
                    isactive = "1";
                    checked = 'checked="checked"';
                    switchOnOff = 'on';
                } else {
                    isactive = "0";
                    checked = '';
                    switchOnOff = '';
                }
                var HtmlOfSwitch = '<div class="btnWidgetSwitch switch switch-xs switch-success">' +
                      '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Restrict_User.EnableDisableBTG(this,\'' + item.UserId + '\',\'' + item.PatientId + '\',\'' + item.IsBreakGlassAllow + '\');\">' +
                       '</div>';

                $row.append('<td><a class="btn  btn-xs" href="#" onclick="Restrict_User.restrictUserDelete(\'' + item.PatUserBreakGlassId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + item.LastName + ', ' + item.FirstName + '</td><td><div id="divSwitch">' + HtmlOfSwitch + '</div>' + '</td>');
                $("#" + Restrict_User.params["PanelID"] + " #pnlRestrictUser_Result #dgvRestrictUser tbody").last().append($row);
            });

            var infoRows = $("#" + Restrict_User.params["PanelID"] + " #pnlRestrictUser_Result #dgvRestrictUser tbody").find("tr");
            //Start || 26 April, 2016 || ZeeshanAK || Change made for fixing EMR-806
            $('#' + Restrict_User.params.PanelID + ' #pnlRestrictUser_Result #dgvRestrictUser').DataTable(
                {
                    "language": {
                        "emptyTable": "No Restricted User Found."
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
                }
            );
        }
        else {
            $('#' + Restrict_User.params["PanelID"] + ' #pnlRestrictUser_Result #dgvRestrictUser').DataTable({
                "language": {
                    "emptyTable": "No Restricted User Found."
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }
        //End   || 26 April, 2016 || ZeeshanAK || Change made for fixing EMR-806

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        EMRUtility.SwicthWidgetInializatoin();
    },

    // EnableDisable Break The Glass for a restricted user
    // Author: ZeeshanAK | Date: April 18, 2016
    EnableDisableBTG: function (obj, UserId, PatientId, IsBreakGlassAllow) {
        Restrict_User.restrictUserUpdate_DBCall(UserId, PatientId, IsBreakGlassAllow).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                Restrict_User.restrictUserLoad();

            }
        });
    },

    // Delete a restricted user
    // Author: ZeeshanAK | Date: April 18, 2016
    restrictUserDelete: function (PatientBreakGlassId) {
        utility.myConfirm('1', function () {
            Restrict_User.restrictUserDelete_DBCall(PatientBreakGlassId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Restrict_User.restrictUserLoad();
                }
            });
        }, function () { },
                    '1'
                );
    },

    // ************************ DB CALLS ************************

    // DB Call to save a restricted user
    // Author: ZeeshanAK | Date: April 16, 2016
    restrictUserSave_DBCall: function (userId) {
        var objData = {};
        //objData["PatUserBreakGlassId"] = true;
        objData["UserId"] = userId;
        objData["PatientId"] = params.patientID == null || params.patientID < 1 ? Restrict_User.params.patientID : params.patientID;

        objData["commandType"] = "restrict_user_save";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "PatientBreakTheGlass", "BreakTheGlass");
    },

    // DB Call for Restrict user load
    // Author: ZeeshanAK | Date: April 16, 2016
    restrictUserLoad_DBCall: function () {
        var objData = {};
        //objData["PatUserBreakGlassId"] = true;
        //objData["UserId"] = userId;
        objData["PatientId"] = params.patientID == null || params.patientID < 1 ? Restrict_User.params.patientID : params.patientID;

        objData["commandType"] = "restrict_user_load";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "PatientBreakTheGlass", "BreakTheGlass");
    },

    // DB Call to delete Restricted user
    // Author: ZeeshanAK | Date: April 18, 2016
    restrictUserDelete_DBCall: function (PatientBreakGlassId) {
        var objData = {};
        objData["PatUserBreakGlassId"] = PatientBreakGlassId;
        objData["commandType"] = "restrict_user_delete";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "PatientBreakTheGlass", "BreakTheGlass");
    },

    // DB Call to update a restricted user
    // Author: ZeeshanAK | Date: April 18, 2016
    restrictUserUpdate_DBCall: function (UserId, PatientId, IsBreakGlassAllow) {
        var objData = {};
        objData["IsBreakGlassAllow"] = IsBreakGlassAllow;
        objData["UserId"] = UserId;
        objData["PatientId"] = PatientId;

        objData["commandType"] = "update_user_btg";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "PatientBreakTheGlass", "BreakTheGlass");
    },
    saveBreakGlassReason_DBCall: function (ReasonText, PatientId) {
        var objData = {};
        objData["PatientId"] = PatientId;
        objData["BreakGlassReason"] = ReasonText;

        objData["commandType"] = "SAVE_BREAKGLASS_REASON";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "PatientBreakTheGlass", "BreakTheGlass");
    },

}