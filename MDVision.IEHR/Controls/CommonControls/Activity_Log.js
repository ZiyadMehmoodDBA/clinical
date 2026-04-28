Activity_Log = {
    params: [],
    bIsFirstLoad: true,
    RequireSubmit: true,
    ActualVisitID: null,
    ActualChargeCaptureID: null,
    IsChargeCapture: null,
    Load: function (params) {

        Activity_Log.params = params;
        Activity_Log.ActualVisitID = null;
        Activity_Log.ActualChargeCaptureID = null;
        Activity_Log.IsChargeCapture = null;

        //Added by Abid for opening in notes
        if (params['ParentCtrlPanelID'] != null)
            Activity_Log.params["PanelID"] = params['ParentCtrlPanelID'];
        var ActivityType = "NewEntryUser";//old NewEntry
        if (Activity_Log.params["PanelID"] == 'pnlBillChargeSearch' && Activity_Log.params["VisitId"] != null) {
            $('#' + Activity_Log.params["PanelID"] + ' #modaldialog').addClass('modal-dialog modal-dialog-full');
            $('#' + Activity_Log.params["PanelID"] + ' #ulTabs').removeClass("hidden");
            $('#' + Activity_Log.params["PanelID"] + ' #activityLogpnlBody').addClass("tabs-custom-body");
            Activity_Log.RequireSubmit = true;
            Activity_Log.ActualVisitID = Activity_Log.params["VisitId"];
            if (Activity_Log.params["ChargeCapId"]) {
                Activity_Log.ActualChargeCaptureID = Activity_Log.params["ChargeCapId"];
            }
            Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, ActivityType, null, null);
        }
        else if ((Activity_Log.params["PanelID"].indexOf("pnlEncounterChargeCapture") > -1 || Activity_Log.params["PanelID"].indexOf("pnlElectronicEOB") > -1) && Activity_Log.params["VisitId"] != null) {
            $('#' + Activity_Log.params["PanelID"] + ' #modaldialog').addClass('modal-dialog modal-dialog-full');
            $('#' + Activity_Log.params["PanelID"] + ' #ulTabs').removeClass("hidden");
            $('#' + Activity_Log.params["PanelID"] + ' #activityLogpnlBody').addClass("tabs-custom-body");
            Activity_Log.RequireSubmit = true;
            Activity_Log.ActualVisitID = Activity_Log.params["VisitId"];
            if (Activity_Log.params["ChargeCapId"]) {
                Activity_Log.ActualChargeCaptureID = Activity_Log.params["ChargeCapId"];
            }

            if (Activity_Log.params.IsNotesLog == true)
                $('#' + Activity_Log.params["PanelID"] + ' #liNotesHistory a').trigger('click');
            else {
                Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, ActivityType, null, null);
                Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, "DeletedCharges", null, null);
            }
        }
        else if (Activity_Log.params["ParentCtrl"] == 'chargeSearchDetail' && Activity_Log.params.ChargeCapId != null) {
            Activity_Log.RequireSubmit = true;
            Activity_Log.ActualChargeCaptureID = Activity_Log.params["ChargeCapId"];
            Activity_Log.LoadActivityLogs("", null, Activity_Log.params.ChargeCapId, 'PatientCharges', ActivityType, null, 'PatientCharges');
            $('#' + Activity_Log.params["PanelID"] + ' #modaldialog').addClass('modal-dialog modal-dialog-full');
        }
        else if ((Activity_Log.params["PanelID"] == 'pnlBillChargeSearch' || Activity_Log.params["ParentCtrl"] == "EncounterChargeCapture") && Activity_Log.params["VisitId"] != null) {
            Activity_Log.RequireSubmit = true;
            Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, ActivityType, null, null);
            $('#' + Activity_Log.params["PanelID"] + ' #modaldialog').addClass('modal-dialog modal-dialog-full');
        }
        else if (Activity_Log.params["PanelID"].indexOf("pnlActivityLog") > -1) {
            Activity_Log.RequireSubmit = false;
            Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, null, null, ActivityType, null, 'Patients,PatientInsurance,PatientAppointments,PatientReferrals');
        }
        else if (Activity_Log.params["ActivityLogType"] != null && Activity_Log.params["ActivityLogType"].indexOf("CurrentItemActivityLog") > -1 && Activity_Log.params["DBTableName"] == "Modified Notes") {
            Activity_Log.RequireSubmit = false;
            Activity_Log.LoadActivityLogs(null, null, Activity_Log.params["ColumnKeyId"], 'Modified Notes', ActivityType, null, null, Activity_Log.params["ParentKeyColumnId"]);
        }
        else if (Activity_Log.params["ActivityLogType"] != null && Activity_Log.params["ActivityLogType"].indexOf("CurrentItemActivityLog") > -1) {
            Activity_Log.RequireSubmit = false;
            Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["ColumnKeyId"], null, ActivityType, null, Activity_Log.params["DBTableName"], Activity_Log.params["ParentKeyColumnId"]);
            //Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, "DeletedCharges", null, Activity_Log.params["DBTableName"]);
        }
        else if (Activity_Log.params["ParentCtrl"] == 'Admin_DrugCodeCost_Detail' && Activity_Log.params.CPTCodeCostID != null) {
            Activity_Log.RequireSubmit = false;
            Activity_Log.LoadActivityLogsDrugCodeCost("", null, Activity_Log.params.CPTCodeCostID, 'DrugCodeCost', ActivityType, null, 'CPTCodeCost');
        }
        else if (Activity_Log.params["ParentCtrl"] == 'userDetail' && Activity_Log.params.UserId != null) {
            Activity_Log.RequireSubmit = false;
            Activity_Log.LoadActivityLogsUserProfile("", null, Activity_Log.params.UserId, null, ActivityType, null, 'Users,UsersEntityGroup,ModuleFormUsersPrivileges,ModuleFormUsers', null);
        }
        else if (Activity_Log.params["AdminHistory"] == '1') {
            Activity_Log.RequireSubmit = false;
            Activity_Log.LoadActivityLogs("", null, Activity_Log.params.ColumnKeyId, Activity_Log.params.ProfileName, ActivityType, null, Activity_Log.params.DBTableName, null);
        }
        else {
            Activity_Log.RequireSubmit = false;
            Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, null, null, ActivityType, null);
        }

    },

    LoadActivityLogs: function (PatientId, DBAuditId, ColumnKeyId, ProfileName, ActivityType, EntryDate, DBTableName, ParentKeyColumnId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (ActivityType == "NewEntry" || ActivityType == "NewEntryUser") {

                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName, ParentKeyColumnId).done(function (response) {
                        if (response.status != false) {
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry").show();
                            }
                            Activity_Log.GridLoad(response.NewEntry_JSON, response.NewEntryCount, "NewEntry", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry");
                            // $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry tbody tr:first").trigger('click');
                            if (typeof response.NewEntryCount == "undefined" || response.NewEntryCount == 0) {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #UserHeading").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").show();

                                Activity_Log.GridLoad(null, null, "User", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User #dgvUser");

                                Activity_Log.GridLoad(null, null, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                            }
                            if (ActivityType == "NewEntryUser" && response.NewEntryCount != 0) {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry tbody tr:first").addClass("active");
                                Activity_Log.usersGridResponse(response);
                            }
                            //------SUbmit-Resubmit
                            if (Activity_Log.RequireSubmit && Activity_Log.IsChargeCapture) {
                                Activity_Log.params["RequireSubmit_JSON"] = response.RequireSubmit_JSON;
                                Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                            } else if (Activity_Log.RequireSubmit) {
                                if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").css("display") == "none") {
                                    $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").show();
                                }
                                if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").css("display") == "none") {
                                    $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").show();
                                }
                                Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                            } else {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").hide();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").hide();
                            }

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (ActivityType == "User") { //User

                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                        if (response.status != false) {
                            Activity_Log.usersGridResponse(response);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (ActivityType == "Changes") {
                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                        if (response.status != false) {
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").show();
                            }
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").show();
                            }
                            if (ProfileName == "Users" || ProfileName == "UsersEntityGroup" || ProfileName == "ModuleFormUsersPrivileges") {

                                var FieldJson = JSON.parse(response.Field_JSON);
                                FieldJson = FieldJson.filter(function (item) { return item.Field != "EntityId" && item.Field != "UserRoleId" && item.Field != "CoWorkersGroupId" && item.Field != "EmergencyRoleId" });
                                var FieldJsonCount = FieldJson.length;
                                FieldJson = JSON.stringify(FieldJson);
                                Activity_Log.GridLoad(FieldJson, FieldJsonCount, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                            }
                            else { Activity_Log.GridLoad(response.Field_JSON, response.FieldCount, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges"); }
                        }
                        else if (response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else if (ActivityType == "DeletedCharges") {
                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName).done(function (response) {
                        if (response.status != false && response.DeletedChargesCount != "DeletedChargesCount" && response.DeletedChargesCount > 0) {
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges").show();
                            }
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #DeletedChargesHeading").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #DeletedChargesHeading").show();
                            }
                            Activity_Log.GridLoad(response.DeletedCharges_JSON, response.DeletedChargesCount, "DeletedCharges", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges #dgvDeletedCharges");
                            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges #dgvDeletedCharges tbody tr:first").trigger('click');

                            if (typeof response.DeletedChargesCount == "DeletedChargesCount") {
                                Activity_Log.GridLoad(null, null, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                            }
                        }
                        else if (response.Message != "undefined" && response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //else if (ActivityType == "RequireSubmit") {
                //    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                //        if (response.status != false) {
                //            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").css("display") == "none") {
                //                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").show();
                //            }
                //            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").css("display") == "none") {
                //                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").show();
                //            }
                //            Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                //        }
                //        else {
                //            utility.DisplayMessages(response.Message, 3);
                //        }
                //    });
                //}

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    LoadActivityLogsDrugCodeCost: function (CPTCodeCostID, DBAuditId, ColumnKeyId, ProfileName, ActivityType, EntryDate, DBTableName, ParentKeyColumnId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("DrugCodeCost", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (ActivityType == "NewEntry" || ActivityType == "NewEntryUser") {

                    Activity_Log.ActivityLogsLoad(null, null, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName, ParentKeyColumnId, "").done(function (response) {
                        if (response.status != false) {
                            Activity_Log.params["PanelID"] = 'pnlActivityLog';
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry").show();
                            }
                            Activity_Log.GridLoad(response.NewEntry_JSON, response.NewEntryCount, "NewEntry", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry");
                            // $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry tbody tr:first").trigger('click');
                            if (typeof response.NewEntryCount == "undefined" || response.NewEntryCount == 0) {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #UserHeading").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").show();

                                Activity_Log.GridLoad(null, null, "User", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User #dgvUser");

                                Activity_Log.GridLoad(null, null, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                            }
                            if (ActivityType == "NewEntryUser" && response.NewEntryCount != 0) {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry tbody tr:first").addClass("active");
                                Activity_Log.usersGridResponse(response);
                            }
                            //------SUbmit-Resubmit
                            if (Activity_Log.RequireSubmit && Activity_Log.IsChargeCapture) {
                                Activity_Log.params["RequireSubmit_JSON"] = response.RequireSubmit_JSON;
                                Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                            } else if (Activity_Log.RequireSubmit) {
                                if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").css("display") == "none") {
                                    $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").show();
                                }
                                if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").css("display") == "none") {
                                    $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").show();
                                }
                                Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                            } else {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").hide();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").hide();
                            }

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (ActivityType == "User") { //User

                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                        if (response.status != false) {
                            Activity_Log.usersGridResponse(response);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (ActivityType == "Changes") {
                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                        if (response.status != false) {
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").show();
                            }
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").show();
                            }
                            Activity_Log.GridLoad(response.Field_JSON, response.FieldCount, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                        }
                        else if (response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else if (ActivityType == "DeletedCharges") {
                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName).done(function (response) {
                        if (response.status != false && response.DeletedChargesCount != "DeletedChargesCount" && response.DeletedChargesCount > 0) {
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges").show();
                            }
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #DeletedChargesHeading").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #DeletedChargesHeading").show();
                            }
                            Activity_Log.GridLoad(response.DeletedCharges_JSON, response.DeletedChargesCount, "DeletedCharges", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges #dgvDeletedCharges");
                            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges #dgvDeletedCharges tbody tr:first").trigger('click');

                            if (typeof response.DeletedChargesCount == "DeletedChargesCount") {
                                Activity_Log.GridLoad(null, null, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                            }
                        }
                        else if (response.Message != "undefined" && response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //else if (ActivityType == "RequireSubmit") {
                //    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                //        if (response.status != false) {
                //            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").css("display") == "none") {
                //                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").show();
                //            }
                //            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").css("display") == "none") {
                //                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").show();
                //            }
                //            Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                //        }
                //        else {
                //            utility.DisplayMessages(response.Message, 3);
                //        }
                //    });
                //}

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    LoadActivityLogsUserProfile: function (UserId, DBAuditId, ColumnKeyId, ProfileName, ActivityType, EntryDate, DBTableName, ParentKeyColumnId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (ActivityType == "NewEntry" || ActivityType == "NewEntryUser") {

                    Activity_Log.ActivityLogsLoad(null, null, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName, ParentKeyColumnId, "").done(function (response) {
                        if (response.status != false) {
                            Activity_Log.params["PanelID"] = 'pnlActivityLog';
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry").show();
                            }
                            // filter Profiles
                            var newEntery = JSON.parse(response.NewEntry_JSON);
                            newEntery = newEntery.filter(function (item) { return item.ProfileName == 'Users' });
                            var newEnteryCount = newEntery.length;
                            newEntery = JSON.stringify(newEntery);
                            Activity_Log.GridLoad(newEntery, newEnteryCount, "NewEntry", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry");
                            if (typeof response.NewEntryCount == "undefined" || response.NewEntryCount == 0) {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #UserHeading").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").show();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").show();

                                Activity_Log.GridLoad(null, null, "User", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User #dgvUser");

                                Activity_Log.GridLoad(null, null, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                            }
                            if (ActivityType == "NewEntryUser" && response.NewEntryCount != 0) {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NewEntry #dgvNewEntry tbody tr:first").addClass("active");
                                var UserList = JSON.parse(response.User_JSON);
                                $.each(UserList, function (k, v) {
                                    if (this.ProfileName != "Users") { this.ProfileName = "Users" };
                                });
                                response.User_JSON = JSON.stringify(UserList);
                                Activity_Log.usersGridResponse(response);
                            }
                            //------SUbmit-Resubmit
                            if (Activity_Log.RequireSubmit && Activity_Log.IsChargeCapture) {
                                Activity_Log.params["RequireSubmit_JSON"] = response.RequireSubmit_JSON;
                                Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                            } else if (Activity_Log.RequireSubmit) {
                                if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").css("display") == "none") {
                                    $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").show();
                                }
                                if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").css("display") == "none") {
                                    $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").show();
                                }
                                Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                            } else {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").hide();
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").hide();
                            }

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (ActivityType == "User") { //User

                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                        if (response.status != false) {
                            Activity_Log.usersGridResponse(response);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (ActivityType == "Changes") {
                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                        if (response.status != false) {
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes").show();
                            }
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #ChangesHeading").show();
                            }
                            Activity_Log.GridLoad(response.Field_JSON, response.FieldCount, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                        }
                        else if (response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else if (ActivityType == "DeletedCharges") {
                    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName).done(function (response) {
                        if (response.status != false && response.DeletedChargesCount != "DeletedChargesCount" && response.DeletedChargesCount > 0) {
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges").show();
                            }
                            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #DeletedChargesHeading").css("display") == "none") {
                                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #DeletedChargesHeading").show();
                            }
                            Activity_Log.GridLoad(response.DeletedCharges_JSON, response.DeletedChargesCount, "DeletedCharges", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges #dgvDeletedCharges");
                            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_DeletedCharges #dgvDeletedCharges tbody tr:first").trigger('click');

                            if (typeof response.DeletedChargesCount == "DeletedChargesCount") {
                                Activity_Log.GridLoad(null, null, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
                            }
                        }
                        else if (response.Message != "undefined" && response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //else if (ActivityType == "RequireSubmit") {
                //    Activity_Log.ActivityLogsLoad(PatientId, null, ColumnKeyId, ProfileName, EntryDate, ActivityType).done(function (response) {
                //        if (response.status != false) {
                //            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").css("display") == "none") {
                //                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").show();
                //            }
                //            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").css("display") == "none") {
                //                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").show();
                //            }
                //            Activity_Log.GridLoad(response.RequireSubmit_JSON, response.RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
                //        }
                //        else {
                //            utility.DisplayMessages(response.Message, 3);
                //        }
                //    });
                //}

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    usersGridResponse: function (response) {
        if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User").css("display") == "none") {
            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User").show();
        }
        if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #UserHeading").css("display") == "none") {
            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #UserHeading").show();
        }
        if (Activity_Log.params.ParentCtrl == "userDetail")
            Activity_Log.GridLoad(response.User_JSON, response.UserCount, "User", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User #dgvUser");
        else
            Activity_Log.GridLoad(response.User_JSON, response.UserCount, "User", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User #dgvUser");
        if (typeof response.UserCount == "undefined" || response.UserCount == 0) {
            Activity_Log.GridLoad(null, null, "Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges");
        } else {
            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User #dgvUser tbody tr:first").addClass("active");
            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_User #dgvUser tbody tr:first").trigger('click');
        }
        //if (response.UserCount ==0) {
        // Activity_Log.GridLoad(response.Field_JSON, response.FieldCount, "Field", "#frmActivityLog #pnlActivityLog_Changes #dgvChanges");
        //}
        if (Activity_Log.IsChargeCapture && Activity_Log.params["RequireSubmit_JSON"]) {
            var Submit_JSON = JSON.parse(Activity_Log.params["RequireSubmit_JSON"]);
            //    .filter(function (i, n) {
            //    return i.ColumnKeyId === '' + ColumnKeyId + '';
            //});
            var RequireSubmitCount = Submit_JSON.length;
            var RequireSubmit_JSON = JSON.stringify(Submit_JSON);
            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").css("display") == "none") {
                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").show();
            }
            if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").css("display") == "none") {
                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").show();
            }
            Activity_Log.GridLoad(RequireSubmit_JSON, RequireSubmitCount, "RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit #dgvRequireSubmit");
        } else if (!Activity_Log.RequireSubmit) {
            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_RequireSubmit").hide();
            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #RequireSubmitHeading").hide();
        }
    },

    GridLoad: function (response, count, gridType, gridId) {

        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "";
        if (gridType == "NewEntry" || gridType == "NewEntryUser") {
            emptyTableMsg = "No New Entry Found";
        }
        else if (gridType == "User") {
            emptyTableMsg = "No User Found";
        }
        else if (gridType == "DeletedCharges") {
            emptyTableMsg = "No Charge Found";
        }
        else if (gridType == "Field") {
            emptyTableMsg = "No Field Found";
        }
        else if (gridType == "RequireSubmit") {
            emptyTableMsg = "No Submit/Re-Submit Found";
        }
        if (count != null && count > 0) {
            var firstRowId = "";
            var PatientLoadJSONData = JSON.parse(response);
            if (gridType == "Field") {
                if (PatientLoadJSONData[0].ProfileName == "Referral Management") {
                    PatientLoadJSONData = PatientLoadJSONData.filter(obj => {
                        return obj.Field != 'Patient Name'
                    });
                }
            }
            $.each(PatientLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";
                if (gridType == "NewEntry" || gridType == "NewEntryUser") {
                    _rowId = "dgvNewEntry_row" + i;
                    //prd 19 Change Profile name if profile name is PatientPayment.
                    if (Activity_Log.params['ParentCtrl'] && Activity_Log.params['ParentCtrl'] == "BillLedgerDetail" && Activity_Log.params['ProfileName'] == 'Payment Posting' && Activity_Log.params['ProfileName']) {
                        $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + 'Account ledger' + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td><td>' + item.EntryDate + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.ProfileName + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td><td>' + item.EntryDate + '</td>');
                    }

                    //$row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.ProfileName + '</td><td>' + item.User + '</td><td>' + item.EntryDate + '</td><td>' + item.Field + '</td><td>' + item.OriginalValue + '</td><td>' + item.CurrentValue + '</td>');
                    if (Activity_Log.params.VisitId && Activity_Log.params.VisitId != null && params["PanelID"] != "pnlActivityLog") {
                        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + Activity_Log.params.VisitId + ",'" + item.ProfileName + "','User',null);");
                    } else if (Activity_Log.params.ChargeCapId && Activity_Log.params.ChargeCapId != null && params["PanelID"] != "pnlActivityLog") {
                        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs('',null," + item.ColumnKeyId + ",'" + item.ProfileName + "','User',null);");
                    } else if (Activity_Log.params.CPTCodeCostID && Activity_Log.params.CPTCodeCostID != null && params["ParentCtrl"] != "Admin_DrugCodeCost_Detail") {
                        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogsDrugCodeCost(" + Activity_Log.params.CPTCodeCostID + ",null," + item.ColumnKeyId + ",'" + item.ProfileName + "','User',null);");
                    } else if (Activity_Log.params["ParentCtrl"] == 'userDetail' && Activity_Log.params.UserId != null) {
                        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogsUserProfile(" + "''" + ",null," + item.ColumnKeyId + "," + null + "," + "'NewEntryUser'" + "," + null + "," + "'Users,UsersEntityGroup,ModuleFormUsersPrivileges,ModuleFormUsers'" + ",null);");
                    }
                    else {
                        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + item.ColumnKeyId + ",'" + item.ProfileName + "','User',null);");
                    }

                }
                else if (gridType == "User") {
                    _rowId = "dgvUser_row" + i;
                    if (Activity_Log.params.ParentCtrl == "userDetail")
                        $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td style="display:none;">' + item.ProfileName + '</td><td>' + item.Actions + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td><td>' + item.Date/*.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')*/ + '</td>');
                    else
                        //prd 19 Change profile name 
                        if (Activity_Log.params['ParentCtrl'] && Activity_Log.params['ParentCtrl'] == "BillLedgerDetail" && Activity_Log.params['ProfileName'] == 'Payment Posting' && Activity_Log.params['ProfileName']) {
                            $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + 'Account ledger' + '</td><td>' + item.Actions + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td><td>' + item.Date/*.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')*/ + '</td>');
                        }
                        else {
                            $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.ProfileName + '</td><td>' + item.Actions + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td><td>' + item.Date/*.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')*/ + '</td>');
                        }
                    // $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.ProfileName + '</td><td>' + item.Actions + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td><td>' + item.Date/*.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')*/ + '</td>');
                    if (item.Actions == "View" || item.Actions == 'Deleted') {
                        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.GridLoad(null, null, 'Field','#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges');");
                    } else
                        if (Activity_Log.params.VisitId != null && params["PanelID"] != "pnlActivityLog") {
                            $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + Activity_Log.params.VisitId + ",'" + item.ProfileName + "','Changes','" + item.Date + "');");
                        } else if (Activity_Log.params.ChargeCapId != null && params["PanelID"] != "pnlActivityLog") {
                            $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs('',null," + item.ColumnKeyId + ",'" + item.ProfileName + "','Changes','" + item.Date + "');");
                        } else {
                            $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + item.ColumnKeyId + ",'" + item.ProfileName + "','Changes','" + item.Date + "');");
                        }

                } else if (gridType == "DeletedCharges") {
                    _rowId = "dgvDeletedCharges_row" + i;
                    $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.ProfileName + '</td><td>' + item.Actions + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td><td>' + item.Date/*.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')*/ + '</td>');
                    if (item.Actions == "View" || item.Actions == 'Deleted') {
                        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.GridLoad(null, null, 'Field','#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges');");
                    } else
                        if (Activity_Log.params.VisitId != null && params["PanelID"] != "pnlActivityLog") {
                            $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + item.ColumnKeyId + ",'" + item.ProfileName + "','Changes','" + item.Date + "');");
                        } else if (Activity_Log.params.ChargeCapId != null && params["PanelID"] != "pnlActivityLog") {
                            $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs('',null," + item.ColumnKeyId + ",'" + item.ProfileName + "','Changes','" + item.Date + "');");
                        } else {
                            $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + item.ColumnKeyId + ",'" + item.ProfileName + "','Changes','" + item.Date + "');");
                        }

                }
                else if (gridType == "Field") {
                    _rowId = "dgvChanges_row" + i;
                    //EMR-1133 fix by azhar
                    var OriginalValue = item.OriginalValue;
                    var CurrentValue = item.CurrentValue;
                    if (item.Field == 'Note Text') {
                        CurrentValue = CurrentValue.replace(/&quot;/g, '"');
                        CurrentValue = CurrentValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        CurrentValue = CurrentValue.replace(/&nbsp;/g, '');
                        OriginalValue = OriginalValue.replace(/&quot;/g, '"');
                        OriginalValue = OriginalValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        OriginalValue = OriginalValue.replace(/&nbsp;/g, '');
                    }
                    if (item.Field == 'Patient Image' || item.Field == 'Patient Profile Thumbnail') {

                        CurrentValue = CurrentValue != "" ? '<img id="imgCurrent" class="img-responsive img-center" src="' + CurrentValue + '" alt="Current Patient Image"/>' : "";
                        OriginalValue = OriginalValue != "" ? '<img id="imgCurrent" class="img-responsive img-center" src="' + OriginalValue + '" alt="Original Patient Image"/>' : "";

                    }
                    if (item.Field == 'SSN') {
                        if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {
                        }
                        else {

                            if (CurrentValue != "") {
                                var last4digit = CurrentValue.slice(-4);
                                CurrentValue = "XXX-XX-" + last4digit;
                            }
                            if (OriginalValue != "") {
                                var last4digits = OriginalValue.slice(-4);
                                OriginalValue = "XXX-XX-" + last4digits;
                            }
                        }
                    }



                    if (item.Field == 'Order Date' || ((item.ProfileName == 'LabOrderTest' || item.ProfileName == 'RadiologyOrderTest') && item.Field == "Date")
                    || (item.ProfileName == 'Medical_Vitals' && item.Field == 'Vital SignDate')
                    || (item.ProfileName == 'Medical_Allergies' && item.Field == 'On SetDate')
                    || (item.ProfileName == 'Medical_Prescription' && item.Field == 'CreatedDate')
                    || (item.ProfileName == 'Medical_Problems List' && item.Field == 'Start Date')
                    || (item.ProfileName == 'Medical_Procedures' && item.Field == 'Start Date')
                    || (item.ProfileName == 'Medical_Procedures' && item.Field == 'End Date')
                    || (item.ProfileName == 'History_Medical Hx' && item.Field == 'Date')
                    || (item.ProfileName == 'History_Medical Hx_Disease' && item.Field == 'From Date')
                    || (item.ProfileName == 'History_Medical Hx_Disease' && item.Field == 'To Date')
                    || (item.ProfileName == 'History_Birth Hx_General' && item.Field == 'Date Admitted')
                    || (item.ProfileName == 'History_Hospitalization Hx_Disease' && item.Field == 'AdmissionDate')
                    || (item.ProfileName == 'History_Hospitalization Hx_Disease' && item.Field == 'DischargeDate')
                    || (item.ProfileName == 'Orders and Procedure Order' && item.Field == 'Order Date')
                    || (item.ProfileName == 'Orders and Results_Consultation' && item.Field == 'Date')
                    || (item.ProfileName == 'History_Surgical Hx_Disease' && item.Field == 'SurgeryDate')
                    || (item.ProfileName == 'History_Birth Hx_General' && item.Field == 'Patient DOB')
                    || (item.ProfileName == 'History_Hospitalization Hx' && item.Field == 'HospitalizationHxDate')
                    || (item.ProfileName == 'Orders and Results_Procedure' && item.Field == 'Date')
                    || (item.ProfileName == 'ProcedureOrderTest' && item.Field == 'Date')
                    || (item.ProfileName == 'Appointment' && item.Field == 'Appointment Date')
                    || (item.ProfileName == 'Referral Management' && item.Field == 'From Date')
                    || (item.ProfileName == 'Referral Management' && item.Field == 'To Date')
                        ) {

                        CurrentValue = CurrentValue != "" ? utility.RemoveTimeFromDate(null, CurrentValue) : "";
                        OriginalValue = OriginalValue != "" ? utility.RemoveTimeFromDate(null, OriginalValue) : "";

                    }
                    if (item.Field == 'Comments') {
                        $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.Field + '</td><td>' + utility.decodeHtml(OriginalValue) + '</td><td>' + utility.decodeHtml(CurrentValue) + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.Field + '</td><td>' + OriginalValue + '</td><td>' + CurrentValue + '</td>');
                    }
                    
                    // disabled the anchor click, because it's only for user to view
                    if (item.Field == 'Note Text') { $row.find('a').addClass('disableAll'); }
                    //if (Activity_Log.RequireSubmit) {
                    //    if (Activity_Log.params.VisitId != null) {
                    //        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + Activity_Log.params.VisitId + ",'" + item.ProfileName + "','RequireSubmit','" + item.Date + "');");
                    //    } else if (Activity_Log.params.ChargeCapId != null) {
                    //        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs('',null," + Activity_Log.params.ChargeCapId + ",'" + item.ProfileName + "','RequireSubmit','" + item.Date + "');");
                    //    } else {
                    //        $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); Activity_Log.LoadActivityLogs(" + Activity_Log.params.patientID + ",null," + item.ColumnKeyId + ",'" + item.ProfileName + "','RequireSubmit','" + item.Date + "');");
                    //    }
                    //    // $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "'));");
                    //} else {

                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "'));");
                    //}
                }
                else if (gridType == "RequireSubmit") {
                    _rowId = "dgvRequireSubmit_row" + i;
                    var Submit = "Submitted";
                    if (item.CurrentValue == "3" || item.CurrentValue.toLowerCase() == "resubmit") {
                        Submit = "ReSubmit";
                    }
                    $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.ProfileName + '</td><td>' + item.DisplayName + '</td><td>' + item.PatientInsurance + '</td><td class="ellipses size-max90" title="' + item.UserName + '">' + item.UserName + '</td><td>' + item.CreatedDate + '</td><td>' + item.ColumnDatatype + '</td><td>' + Submit + '</td><td>' + item.IsCrossedOver + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "'));");
                }
                if (gridType == "User")//There's no ActivityLogId
                {
                    if (firstRowId == "") {
                        firstRowId = _rowId;
                    }
                    $row.attr("id", _rowId);
                }
                else {
                    if (firstRowId == "") {
                        firstRowId = _rowId;
                    }
                    $row.attr("id", _rowId);
                }

                if (Activity_Log.params.ParentCtrl == "userDetail" && $(gridId).attr("id") == "dgvUser")
                { $(gridId + " thead").find("th").eq(1).css("display", "none"); $(gridId + " tbody").last().append($row); }
                else
                    $(gridId + " tbody").last().append($row);
            });


            //if (firstRowId != "") {
            //    //$(gridId + " tbody #" + firstRowId).addClass('active');
            //  //.fir + firstRowId).trigger('click');

            //}

            if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
                ;
            else
                $(gridId).DataTable({ bDestroy: true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
        else {
            if (gridType == "User") {
                if ($.fn.dataTable.isDataTable("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges"))
                    $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges").dataTable().fnDestroy();

                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges" + " tbody").find("tr").remove();

                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges").DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": "No Field Change Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }

            if (!$(gridId).parent().parent().hasClass("dataTables_wrapper")) {
                $(gridId).DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": emptyTableMsg
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
                $(gridId + " tbody").last().append($row);
            }
        }

    },

    ActivityLogsLoad: function (PatientId, DBAuditId, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName, ParentKeyColumnId, CPTCodeCostID, UserId) {
        if (DBAuditId == null) {
            DBAuditId = "";
        }
        if (ColumnKeyId == null) {
            ColumnKeyId = "";
        }
        if (ProfileName == null) {
            ProfileName = "";
        }
        if (EntryDate == null) {
            EntryDate = "";
        }
        if (ActivityType == null) {
            ActivityType = "";
        }
        if (PatientId == null) {
            PatientId = "";
        }
        if (DBTableName == null) {
            DBTableName = "";
        }
        if (ParentKeyColumnId == null) {
            ParentKeyColumnId = "";
        }

        if (!ProfileName && Activity_Log.params["ActionPanContainer"] == "actionPanPatientAuditLog")
            ProfileName = "Appointment,Demographic,Emergency Contact,Patient Family,Patient Insurance,Preferences,Referral,Referral Management";

        var data = null;
        // search parameter , class name, command name of class
        if (Activity_Log.params["CPTCodeCostID"] != null) {
            data = "CPTCodeCostID=" + CPTCodeCostID + "&DBAuditID=" + DBAuditId + "&ColumnKeyId=" + ColumnKeyId + "&ProfileName=" + ProfileName + "&EntryDate=" + EntryDate + "&ActivityType=" + ActivityType + "&DBTableName=" + DBTableName + "&ParentKeyColumnId=" + ParentKeyColumnId;
            return MDVisionService.defaultService(data, "PATIENT_ACTIVITY_LOG", "LOAD_DRUGCODECOST_ACTIVITY_LOG");
        }
        else if (Activity_Log.params["UserId"] != null) {
            if (ActivityType == "Changes") {
                ProfileName = "";
            }
            data = "UserId=" + UserId + "&DBAuditID=" + DBAuditId + "&ColumnKeyId=" + ColumnKeyId + "&ProfileName=" + ProfileName + "&EntryDate=" + EntryDate + "&ActivityType=" + ActivityType + "&DBTableName=" + DBTableName + "&ParentKeyColumnId=" + ParentKeyColumnId;
            return MDVisionService.defaultService(data, "PATIENT_ACTIVITY_LOG", "LOAD_USER_ACTIVITY_LOG");
        }
        else if ((Activity_Log.params["VisitId"] != null || ColumnKeyId != null) && Activity_Log.params["CPTCodeCostID"] == null) {
            if (Activity_Log.params["VisitId"] != null && ActivityType == "DeletedCharges")
                data = "PatientID=" + PatientId + "&DBAuditID=" + DBAuditId + "&ProfileName=" + ProfileName + "&EntryDate=" + EntryDate + "&ActivityType=" + ActivityType + "&DBTableName=" + DBTableName + "&VisitId=" + Activity_Log.params["VisitId"] + "&ParentKeyColumnId=" + ParentKeyColumnId;
            else
                data = "PatientID=" + PatientId + "&DBAuditID=" + DBAuditId + "&ColumnKeyId=" + ColumnKeyId + "&ProfileName=" + ProfileName + "&EntryDate=" + EntryDate + "&ActivityType=" + ActivityType + "&DBTableName=" + DBTableName + "&ParentKeyColumnId=" + ParentKeyColumnId;
            return MDVisionService.defaultService(data, "PATIENT_ACTIVITY_LOG", "LOAD_PATIENTVISITS_ACTIVITY_LOG");
        } else {
            return MDVisionService.defaultService(data, "DRUGCODECOST_ACTIVITY_LOG", "LOAD_DRUGCODECOST_ACTIVITY_LOG");
        }

    },

    UnLoad: function () {

        if (Activity_Log.params != null && Activity_Log.params.ParentCtrl && Activity_Log.params['ParentCtrlPanelID'] != null) {

            UnloadActionPan(Activity_Log.params["ParentCtrl"], "Activity_Log", null, Activity_Log.params["ParentCtrlPanelID"]);
        }
        else if (Activity_Log.params != null && Activity_Log.params.ParentCtrl) {
            UnloadActionPan(Activity_Log.params.ParentCtrl);
        }
        else
            UnloadActionPan();


    },
    LoadPatientHistory: function () {
        Activity_Log.HideShowDivs();
        Activity_Log.RequireSubmit = false;
        Activity_Log.IsChargeCapture = null;
        Activity_Log.RemoveRowsFromAllGrids();
        Activity_Log.params["ChargeCapId"] = null;
        Activity_Log.params["VisitId"] = null;
        Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, null, null, "NewEntryUser", null, 'Patients,PatientInsurance');
    },
    LoadClaimHistory: function () {
        Activity_Log.HideShowDivs();
        Activity_Log.RemoveRowsFromAllGrids();
        Activity_Log.IsChargeCapture = null;
        Activity_Log.RequireSubmit = true;
        if (Activity_Log.ActualVisitID) {
            Activity_Log.params["ChargeCapId"] = null;
            Activity_Log.params["VisitId"] = Activity_Log.ActualVisitID
        }
        else {
            Activity_Log.params["VisitId"] = 0;
        }
        Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, "NewEntryUser", null, null);
        Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, "DeletedCharges", null, "PatientVisits,PatientCharges");
    },
    LoadChargesHistory: function () {
        Activity_Log.HideShowDivs();
        Activity_Log.RemoveRowsFromAllGrids();
        Activity_Log.RequireSubmit = true;
        Activity_Log.IsChargeCapture = true;
        if (Activity_Log.ActualChargeCaptureID) {
            Activity_Log.params["VisitId"] = null;
            Activity_Log.params["ChargeCapId"] = Activity_Log.ActualChargeCaptureID
        } else {
            Activity_Log.params["ChargeCapId"] = 0;
        }
        Activity_Log.LoadActivityLogs("", null, Activity_Log.params.ChargeCapId, 'PatientCharges', "NewEntryUser", null, 'PatientCharges');
    },
    RemoveRowsFromAllGrids: function () {
        Activity_Log.RemoveRowsFromGrid("User", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #dgvUser");
        Activity_Log.RemoveRowsFromGrid("Field", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #dgvChanges");
        Activity_Log.RemoveRowsFromGrid("RequireSubmit", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #dgvRequireSubmit");
        Activity_Log.RemoveRowsFromGrid("DeletedCharges", "#" + Activity_Log.params["PanelID"] + " #frmActivityLog #dgvDeletedCharges");
    },
    RemoveRowsFromGrid: function (gridType, gridId) {
        if (gridType == "User") {
            emptyTableMsg = "No User Found";
        }
        else if (gridType == "DeletedCharges") {
            emptyTableMsg = "No Charge Found";
        }
        else if (gridType == "Field") {
            emptyTableMsg = "No Field Found";
        }
        else if (gridType == "RequireSubmit") {
            emptyTableMsg = "No Submit/Re-Submit Found";
        }
        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();
        if (gridType == "User") {
            if ($.fn.dataTable.isDataTable("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges"))
                $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges").dataTable().fnDestroy();

            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges" + " tbody").find("tr").remove();

            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges").DataTable({
                bDestroy: true,
                "language": {
                    "emptyTable": "No Field Change Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if (!$(gridId).parent().parent().hasClass("dataTables_wrapper")) {
            $(gridId).DataTable({
                bDestroy: true,
                "language": {
                    "emptyTable": emptyTableMsg
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        else {
            var $row = $('<tr/>');
            $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
            $(gridId + " tbody").last().append($row);
        }
    },

    LoadNotesHistory: function () {

        Activity_Log.RemoveRowsFromAllGrids();
        Activity_Log.IsChargeCapture = null;
        Activity_Log.RequireSubmit = false;
        if (Activity_Log.ActualVisitID) {
            Activity_Log.params["ChargeCapId"] = null;
            Activity_Log.params["VisitId"] = Activity_Log.ActualVisitID
        }
        else {
            Activity_Log.params["VisitId"] = 0;
        }
        //Activity_Log.LoadActivityLogs(Activity_Log.params.patientID, null, Activity_Log.params["VisitId"], null, "NewEntryUser", null, null);
        Activity_Log.LoadActivityLogsNotesComments(Activity_Log.params.patientID, null, Activity_Log.ActualVisitID, null, "Changes", null, "PatientVisits");

    },

    LoadActivityLogsNotesComments: function () {
        $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #divAuditTablesData").hide();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Activity_Log.ActivityLogsNotesLoad(Activity_Log.params.patientID, Activity_Log.ActualVisitID, 'PatientVisits').done(function (response) {
                    if (response.status != false) {
                        if ($("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NotesLog").css("display") == "none") {
                            $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NotesLog").show();
                        }
                        Activity_Log.NotesGridLoad(response);
                    }
                    else if (response.Message != "") {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ActivityLogsNotesLoad: function (patientId, visitId, tableName) {
        data = "PatientID=" + patientId + "&VisitID=" + visitId + "&DBTableName=" + tableName;
        return MDVisionService.defaultService(data, "PATIENT_ACTIVITY_LOG", "LOAD_NOTE_COMMENTS_AUDIT");
    },

    NotesGridLoad: function (response) {

        $("#dgvNotesLog").dataTable().fnDestroy();
        $("#dgvNotesLog tbody").find("tr").remove();
        $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #dgvNotesLogPrint tbody tr").remove();
        if (response.LogCount > 0) {
            var LogLoad_JSONData = JSON.parse(response.LogLoad_JSON);
            $.each(LogLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td style="display:none;">' + item.DBAuditId + '</td><td>' + item.CreatedDate + '</td><td>' + item.UserFullName + '</td> <td>' + item.CurrentValue + '</td>');
                $("#dgvNotesLog tbody").last().append($row);
            });

            $.each(LogLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td style="display:none;">' + item.DBAuditId + '</td><td>' + item.CreatedDate + '</td><td>' + item.UserFullName + '</td> <td>' + item.CurrentValue + '</td>');
                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #dgvNotesLogPrint tbody").last().append($row);
            });
        }
        else {
            $('#dgvNotesLog').DataTable({
                "language": {
                    "emptyTable": "No Notes Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvNotesLog'))
            ;
        else
            $("#dgvNotesLog").DataTable({ "bSort": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


    },

    HideShowDivs: function () {
        $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #divAuditTablesData").show();
        $("#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_NotesLog").hide();

    },

    PrintNotesLog: function () {
        Activity_Log.LoadPatientVisitInfoPrint(Activity_Log.ActualVisitID, Activity_Log.params.patientID).done(function (response) {
            if (response.status != false) {
                var VisitInfoJSon = JSON.parse(response.VisitNotePrintLoad_JSON);
                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #DivClaimInfo #SpnPatientName").text(VisitInfoJSon[0].PatientName);
                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #DivClaimInfo #SpnDOS").text(utility.RemoveTimeFromDate(null, VisitInfoJSon[0].DOSFrom));
                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #DivClaimInfo #SpnClaimDate").text(utility.RemoveTimeFromDate(null, VisitInfoJSon[0].CreatedOn));
                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #DivClaimInfo #SpnClaimNumber").text(VisitInfoJSon[0].ClaimNumber);
                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #DivClaimInfo #SpnProvider").text(VisitInfoJSon[0].ProviderName);
                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #DivClaimInfo #SpnFacility").text(VisitInfoJSon[0].FacilityName);
                var Rows = $("#pnlActivityLog #pnlActivityLog_NotesLog #dgvNotesLog tbody tr").clone();
                ////$("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #dgvNotesLogPrint tbody").empty();
                //$("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #dgvNotesLogPrint tbody tr").remove();
                //$.each(Rows, function (i, item) {
                //    $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintBody #dgvNotesLogPrint tbody").last().append(item);
                //});
                var d = new Date($('#userCurrentTime').text());
                if (d == "Invalid Date")
                    d = new Date(Date($('#userCurrentTime').text()))

                $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog #DivPrintHeader #DivCurrentDate").html(utility.DateTemplate(d));

                var params = [];
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["IsFromActivityLog"] = true;
                params["ParentCtrl"] = 'Activity_Log';
                //params["PanelID"] = "pnlPreviewPatientPayments"

                LoadActionPan('Bill_PatientPaymentsPrint', params);
            } else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },

    LoadPatientVisitInfoPrint: function (VisitId, PatientId) {
        data = "VisitId=" + VisitId + "&PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "PATIENT_ACTIVITY_LOG", "LOAD_PATIENTVISITS_PRINT_INFO");
    },
}
