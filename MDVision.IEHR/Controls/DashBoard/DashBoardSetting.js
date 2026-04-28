DashBoardSetting = {
    params: null,
    multipleEMCodeIds: '',
    multipleWeekWorkDaysIds: '',
    iTrackDashboardIds: '',
    bIsFirstLoad: true,
    selected_theme: "",
    Load: function (params) {
        DashBoardSetting.params = params;
        //if (DashBoardSetting.params.PanelID != "pnldashboardsetting")
        //    DashBoardSetting.params.PanelID = DashBoardSetting.params.PanelID + ' #pnldashboardsetting';
        DashBoardSetting.params.PanelID = 'pnldashboardsetting';
        if (DashBoardSetting.bIsFirstLoad) {
            DashBoardSetting.bIsFirstLoad = false;
            //var self = $('#'+DashBoardSetting.params.PanelID);
            var self = $('#pnldashboardsetting #dynamicDD');
            DashBoardSetting.SearchModules(null).done(function (response) {
                if (response.status != false) {
                    DashBoardSetting.BindModules(response);
                    //DashBoardSetting.DefaultSettingFill();
                }
                //commented by Arsalan because we are not using this for the time.
                //DashBoardSetting.SearchPrinters().done(function (response) {
                //    if (response.status != false) {
                //        DashBoardSetting.BindPrinters(response);
                //DashBoardSetting.DefaultSettingFill();
                var BillingModuleId = 0;
                $.each(JSON.parse(response.ALLOWMODULE_JSON), function (index, item) {
                    if (item.ModuleName.toLowerCase() == "billing") {
                        BillingModuleId = item.ModuleId;
                    }
                });
                DashBoardSetting.SearchBillingModuleForms(BillingModuleId, globalAppdata["AppUserId"]).done(function (response) {

                    if (response.status != false) {
                        DashBoardSetting.BindBillingModuleForms(response);
                    }
                });
                securityRoleDetail.FillSecurityRole(0, 4, 0).done(function (response) {

                    if (response.status != false) {
                        DashBoardSetting.BindModuleForms(response);
                        DashBoardSetting.LoadWidgetsAndKPI(0).done(function (response) {

                            if (response.status != false) {

                                $('#' + DashBoardSetting.params.PanelID + ' #pwidget').empty();
                                $('#' + DashBoardSetting.params.PanelID + ' #pkpi').empty();

                                var widgets = JSON.parse(response.DASHBOARDSETTING_WIDGET_JSON);
                                var kpis = JSON.parse(response.DASHBOARDSETTING_KPI_JSON);

                                var table = $(' <table class="table table-responsive table-bordered table-condensed" id="widtable">    </table>');
                                $('#widtable').children('tbody');
                                for (var w = 0; w < widgets.length; w++) {
                                    var acin = 'success';
                                    var disable_class = "";
                                    disable_class = widgets[w].PermissionAllowed == "NO" ? "disableAll" : "";

                                    if (widgets[w].WidgetsName == "Encounter") {
                                        widgets[w].WidgetsName = "Notes";
                                    }

                                    if (widgets[w].IsActive == "True" && widgets[w].PermissionAllowed == "YES")
                                        table.append('<tr><td style="vertical-align:middle;">' + widgets[w].WidgetsName + '</td> <td><div class="btnWidgetSwitch switch switch-xs switch-success ' + disable_class + '" widgetID = "' + widgets[w].DBSId + '"> <input id="' + widgets[w].DBSId + '" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch /> </div> </td> </tr>');
                                    else if (widgets[w].IsActive == "False" && widgets[w].PermissionAllowed == "YES")
                                        table.append('<tr><td style="vertical-align:middle;">' + widgets[w].WidgetsName + '</td> <td><div class="btnWidgetSwitch switch switch-xs switch-success ' + disable_class + '" widgetID = "' + widgets[w].DBSId + '"> <input id="' + widgets[w].DBSId + '" type="checkbox"  name="switch" data-plugin-ios-switch /> </div> </td> </tr>');
                                }
                                $('#' + DashBoardSetting.params.PanelID + ' #pwidget').append(table);

                                var table1 = $(' <table class="table table-responsive table-bordered table-condensed" id="kpitable">    </table>');
                                $('#kpitable').children('tbody');
                                for (var k = 0; k < kpis.length; k++) {
                                    var acin = 'success';
                                    if (kpis[k].IsActive == "True")
                                        table1.append('<tr><td style="vertical-align:middle;">' + kpis[k].KPIName + '</td> <td><div class="btnKPISwitch switch switch-xs switch-success" widgetid = "' + kpis[k].DBSId + '"> <input id="' + kpis[k].DBSId + '" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch  /> </div> </td> </tr>');
                                    else
                                        table1.append('<tr><td style="vertical-align:middle;">' + kpis[k].KPIName + '</td> <td><div class="btnKPISwitch switch switch-xs switch-success" widgetid = "' + kpis[k].DBSId + '"> <input id="' + kpis[k].DBSId + '" type="checkbox" name="switch" data-plugin-ios-switch  /> </div> </td> </tr>');

                                }
                                $('#' + DashBoardSetting.params.PanelID + ' #pkpi').append(table1);

                                (function ($) {

                                    'use strict';

                                    if (typeof Switch !== 'undefined' && $.isFunction(Switch)) {

                                        $(function () {
                                            $('[data-plugin-ios-switch]').each(function () {
                                                var $this = $(this);

                                                $this.themePluginIOS7Switch();
                                            });
                                        });

                                    }


                                }).apply(this, [jQuery]);

                                $('#' + DashBoardSetting.params.PanelID + ' [data-plugin-toggle]').each(function () {
                                    var $this = $(this),
                                        opts = {};

                                    var pluginOptions = $this.data('plugin-options');
                                    if (pluginOptions)
                                        opts = pluginOptions;

                                    $this.themePluginToggle(opts);
                                });

                                $('#widtable .disableAll').find('div [class="on-background background-fill"]').css({ "background": "rgb(195, 188, 188)" });

                                //$('input[type=checkbox]').change(function () {
                                //    //alert($(this).is(":checked"));
                                //    var id = this.id;
                                //    if ($(this).is(":checked") == true) {
                                //        DashBoardSetting.ActiveInactivecontrol(id, 1);
                                //    }
                                //    else {
                                //        DashBoardSetting.ActiveInactivecontrol(id, 0);
                                //    }

                                //});
                                //DashBoardSetting.DefaultSettingFill();
                                $('#dynamicDD').loadDropDowns(false).done(function () {


                                    //var self = $('#pnldashboardsetting #setdefault');
                                    //self.find('#ddlRaceIds').attr('ddlist', 'GetRaceByDescription');
                                    //var data = "ID=1&ID=-1&ID2=" + ProviderId;
                                    //$(self).find.loadDropDowns(true, data).done(function () {
                                    //});



                                    //CacheManager.BindDropDownsByID($('#' + DashBoardSetting.params.PanelID + ' #ddlDefaultSuperBill'), 'GetSupperBill', false, Number(globalAppdata.DefaultPracticeId));
                                    DashBoardSetting.IntializeMultiSelectDropDownEMCodesType();
                                    DashBoardSetting.IntializeMultiSelectDropDownWorkWeekDays();
                                    DashBoardSetting.IntializeMultiSelectDropDownRace();
                                    DashBoardSetting.IntializeMultiSelectDropDowniTrackDashboard();
                                    DashBoardSetting.enableDisableDropDownLists('ddlEMCodeType');
                                    $("#ddlEMCodeType option[value='']").remove();
                                    if ($('#divEMCodeTypes').find('div > .multiselect-container.dropdown-menu > li > a > label:contains(" - Select -")')) {
                                        $('#divEMCodeTypes').find('div > .multiselect-container.dropdown-menu > li > a > label:contains(" - Select -")').remove();
                                    }
                                    DashBoardSetting.DefaultSettingFill();
                                });
                            } else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });

                        //DashBoardSetting.DefaultSettingFill();

                    }
                });
                //    }
                //});
            });
            //DashBoardSetting.LoadDashBoardSetting();
            $("#" + DashBoardSetting.params.PanelID).find(".autoPopulate").on("change", function () {
                DashBoardSetting.checkOrUncheckSelectAll();
            });
            $("#" + DashBoardSetting.params.PanelID).find(".chkMU3").on("change", function () {
                DashBoardSetting.checkOrUncheckSelectAllMU3();
            });
            $("#" + DashBoardSetting.params.PanelID).find(".chkMIPSPI").on("change", function () {
                DashBoardSetting.checkOrUncheckSelectAllMIPS_(".chkMIPSPI", "chkSelectAllMIPSPI");
            });
            $("#" + DashBoardSetting.params.PanelID).find(".chkMIPSCQM").on("change", function () {
                DashBoardSetting.checkOrUncheckSelectAllMIPS_(".chkMIPSCQM", "chkSelectAllMIPSCQM");
            });
            $("#" + DashBoardSetting.params.PanelID).find(".chkMIPSIA").on("change", function () {
                DashBoardSetting.checkOrUncheckSelectAllMIPS_(".chkMIPSIA", "chkSelectAllMIPSIA");
            });
        }

    },
    //Begin Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
    GetNotesTemplates: function () {
        setTimeout(function () {
            var ProviderId = null;
            var self = $('#pnldashboardsetting #divDefaultTemplate');
            if (ProviderId == null) {
                ProviderId = $("#pnldashboardsetting #ddlDefaultProvider").val();
            }
            self.find('#ddlDefaultTemplate').empty();
            self.find('#ddlDefaultTemplate').attr('ddlist', 'GetNoteTemplate');
            var data = "IsActive=1&ID=-1&ID2=" + ProviderId;
            self.loadDropDowns(true, data).done(function () {
                self.find('#ddlDefaultTemplate option').filter(function () {
                    return $.trim($(this).val()) == globalAppdata.DefaultTemplate;
                }).attr('selected', true);
            });
        }, 2);
    },
    //End Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
    LoadDashBoardSetting: function () {
        DashBoardSetting.LoadWidgetsAndKPI(0).done(function (response) {

            if (response.status != false) {

                $('#' + DashBoardSetting.params.PanelID + ' #pwidget').empty();
                $('#' + DashBoardSetting.params.PanelID + ' #pkpi').empty();

                var widgets = JSON.parse(response.DASHBOARDSETTING_WIDGET_JSON);
                var kpis = JSON.parse(response.DASHBOARDSETTING_KPI_JSON);

                var table = $(' <table class="table table-responsive table-bordered table-condensed" id="widtable">    </table>');
                $('#widtable').children('tbody');
                for (var w = 0; w < widgets.length; w++) {
                    var acin = 'success';
                    if (widgets[w].IsActive == "True")
                        table.append('<tr><td style="vertical-align:middle;">' + widgets[w].WidgetsName + '</td> <td><div class="btnWidgetSwitch switch switch-xs switch-success" widgetID = "' + widgets[w].DBSId + '"> <input id="' + widgets[w].DBSId + '" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch /> </div> </td> </tr>');
                    else
                        table.append('<tr><td style="vertical-align:middle;">' + widgets[w].WidgetsName + '</td> <td><div class="btnWidgetSwitch switch switch-xs switch-success" widgetID = "' + widgets[w].DBSId + '"> <input id="' + widgets[w].DBSId + '" type="checkbox"  name="switch" data-plugin-ios-switch /> </div> </td> </tr>');
                }
                $('#' + DashBoardSetting.params.PanelID + ' #pwidget').append(table);

                var table1 = $(' <table class="table table-responsive table-bordered table-condensed" id="kpitable">    </table>');
                $('#kpitable').children('tbody');
                for (var k = 0; k < kpis.length; k++) {
                    var acin = 'success';
                    if (kpis[k].IsActive == "True")
                        table1.append('<tr><td style="vertical-align:middle;">' + kpis[k].KPIName + '</td> <td><div class="btnKPISwitch switch switch-xs switch-success" widgetid = "' + kpis[k].DBSId + '"> <input id="' + kpis[k].DBSId + '" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch  /> </div> </td> </tr>');
                    else
                        table1.append('<tr><td style="vertical-align:middle;">' + kpis[k].KPIName + '</td> <td><div class="btnKPISwitch switch switch-xs switch-success" widgetid = "' + kpis[k].DBSId + '"> <input id="' + kpis[k].DBSId + '" type="checkbox" name="switch" data-plugin-ios-switch  /> </div> </td> </tr>');

                }
                $('#' + DashBoardSetting.params.PanelID + ' #pkpi').append(table1);

                (function ($) {

                    'use strict';

                    if (typeof Switch !== 'undefined' && $.isFunction(Switch)) {

                        $(function () {
                            $('[data-plugin-ios-switch]').each(function () {
                                var $this = $(this);

                                $this.themePluginIOS7Switch();
                            });
                        });

                    }

                }).apply(this, [jQuery]);

                //$('input[type=checkbox]').change(function () {
                //    //alert($(this).is(":checked"));
                //    var id = this.id;
                //    if ($(this).is(":checked") == true) {
                //        DashBoardSetting.ActiveInactivecontrol(id, 1);
                //    }
                //    else {
                //        DashBoardSetting.ActiveInactivecontrol(id, 0);
                //    }

                //});
                //DashBoardSetting.DefaultSettingFill();
            } else {

                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadWidgetsAndKPI: function (DBSId) {

        var data = "DBSId=" + DBSId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "LOAD_WIDGET_AND_KPI");

    },


    ActiveInactivecontrol: function (dbsid, isactive) {
        DashBoardSetting.UpdateDashBoardSetting(dbsid, isactive).done(function (response) {
            if (response != false) {
                //  DashBoardSetting.LoadDashBoardSetting();
                utility.DisplayMessages("Successfully Saved.", 1);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    UnLoad: function () {
        if (DashBoardSetting.params != null && DashBoardSetting.params.ParentCtrl != null) {
            UnloadActionPan(DashBoardSetting.params.ParentCtrl, "DashBoardSetting");
        } else
            UnloadActionPan();
    },
    //#endregion Server Side Calls
    UpdateDashBoardSetting: function (DBSId, IsActive) {
        var data = "DBSId=" + DBSId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "UPDATE_DASHBOARDSETTING");
    },
    UpdateAllDashBoardSetting: function (strData) {
        var data = "data=" + strData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "UPDATE_ALL_DASHBOARDSETTING");
    },
    SearchModules: function (ModuleId) {
        var data = "ModuleId=" + ModuleId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "SEARCH_REPORTS_PRIVILEGES");
    },
    SearchBillingModuleForms: function (ModuleId, UserId) {
        var data = "moduleId=" + ModuleId + "&UserId=" + UserId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "SEARCH_BILLING_MODULE_FORM");
    },
    SearchPrinters: function () {
        var data = ""
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "PRINTER");
    },

    BindModules: function (response) {
        var Module_Detail = JSON.parse(response.ALLOWMODULE_JSON);
        var modulename;
        var $select = $('#ddlPreferredScreenAfterLogin');
        $select.find('option').remove();
        $select.append('<option value="">- Select -</option>');
        var Moduleids = "";
        $.each(Module_Detail, function (i, item) {
            modulename = item.ModuleName;

            $select.append('<option value=' + item.ModuleId + '>' + item.ModuleName + '</option>');
            //Moduleids += item.ModuleId + ',';


        });
        //Moduleids = Moduleids.replace(undefined, '');
    },

    BindPrinters: function (response) {
        var Printer_Detail = JSON.parse(response.Printer_JSON);
        var printername;
        var $select = $('#ddlclaimprinter');
        $select.find('option').remove();
        $select.append('<option value="">- Select -</option>');

        $.each(Printer_Detail, function (i, item) {

            $select.append('<option value="' + item.PrinterName + '">' + item.PrinterName + '</option>');

        });
        var $select1 = $('#ddattachmentprinter');
        $select1.find('option').remove();
        $select1.append('<option value="">- Select -</option>');

        $.each(Printer_Detail, function (i, item) {

            $select1.append('<option value="' + item.PrinterName + '">' + item.PrinterName + '</option>');

        });

    },
    BindModuleForms: function (response) {

        var ModuleForm_Detail = JSON.parse(response.Forms_JSON);
        var $select = $('#ddlPreferredSchedulerScreen');
        $select.find('option').remove();
        $select.append('<option value="">- Select -</option>')
        $.each(ModuleForm_Detail, function (i, item) {
            if (item.FormName == "Wait List" || item.FormName == "Scheduler Search" || item.FormName == "Schedule Group") {
                $select.append('<option value=' + item.FormsId + '>' + item.FormName + '</option>');
            } else if (item.FormName == "Calendar") {

                for (var f = 0; f < 3; f++) {
                    var formnam;
                    if (f == 0)
                        formnam = item.FormName + "Day";
                    if (f == 1)
                        formnam = item.FormName + "Week";
                    if (f == 2)
                        formnam = item.FormName + "Month";

                    $select.append('<option value=' + item.FormsId + '>' + formnam + '</option>');
                }
            }


        });


    },
    BindBillingModuleForms: function (response) {
        var ModuleForm_Detail = JSON.parse(response.AssignedModuleForm_JSON)
        var $select = $('#ddlPreferredBillingScreen');
        $select.find('option').remove();
        $select.append('<option value="">- Select -</option>')
        $.each(ModuleForm_Detail, function (i, item) {
            if (item.FormName == "Miscellaneous_eSuperbill") {
                $select.append('<option value=' + item.FormsId + '>Out of Office Visits</option>');
            }
            else {
                $select.append('<option value=' + item.FormsId + '>' + item.FormName + '</option>');
            }
        });
    },
    SaveDefaultSetting: function (DefaultSettingData) {
        var data_ = utility.replaceSpecialCharacters(DefaultSettingData);
        var data = "DefaultSettingData=" + data_;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "SAVE_DEFAULT_SETTING");
    },
    UpdateDefaultSetting: function (DefaultSettingData, EntityUserOptionId) {
        var data_ = utility.replaceSpecialCharacters(DefaultSettingData);
        var data = "DefaultSettingData=" + data_ + "&EntityUserOptionId=" + EntityUserOptionId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "UPDATE_DEFAULT_SETTING");
    },

    SetDefaultSetting: function (RaceIds) {
        SetGlobalAppData('DefaultProviderId', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultProvider").val());
        SetGlobalAppData('DefaultFacilityId', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultFacility").val());
        SetGlobalAppData('DefaultResourceId', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultResource").val());
        SetGlobalAppData('DefaultProviderName', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultProvider option:selected").text());
        SetGlobalAppData('DefaultFacilityName', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultFacility option:selected").text());
        SetGlobalAppData('DefaultFacilityDescription', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultFacility option:selected").attr('FacilityDescription'));
        SetGlobalAppData('DefaultResourceName', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultResource option:selected").text());
        //Begin Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
        SetGlobalAppData('DefaultBillingProviderName', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultBillingProvider option:selected").text());
        SetGlobalAppData('DefaultBillingProviderId', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultBillingProvider").val());
        SetGlobalAppData('PreferredScreenName', $("#" + DashBoardSetting.params.PanelID + " #ddlPreferredScreenAfterLogin option:selected").text());
        SetGlobalAppData('PreferredSchScreenName', $("#" + DashBoardSetting.params.PanelID + " #ddlPreferredSchedulerScreen option:selected").text());
        SetGlobalAppData('PreferredBillingScreenName', $("#" + DashBoardSetting.params.PanelID + " #ddlPreferredBillingScreen option:selected").text());
        SetGlobalAppData('DefaultTemplate', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultTemplate").val());
        SetGlobalAppData('DefaultSuperBill', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultSuperBill").val());
        //SetGlobalAppData('ENMCodesTime', $("#" + DashBoardSetting.params.PanelID + " #ddlENMCodesTime").val());
        SetGlobalAppData('PreferredAppointmentStatus', $("#" + DashBoardSetting.params.PanelID + " #ddlAppointmentStatus").val());
        SetGlobalAppData('DefaultTabMessages', $("#" + DashBoardSetting.params.PanelID + " #ddlDefaultTabMessages").val());
        var WeekWorkDays = $("#" + DashBoardSetting.params.PanelID + " #ddlSchedulerWorkWeekDays option:Selected").map(function () {
            return this.value;
        }).get().join(',');
        SetGlobalAppData('WeekWorkDaysIds', WeekWorkDays);
        var iTrackDashboardIds = $("#" + DashBoardSetting.params.PanelID + " #ddliTrackDashboard option:Selected").map(function () {
            return this.value;
        }).get().join(',');
        SetGlobalAppData('iTrackDashboardIds', iTrackDashboardIds);
        SetGlobalAppData('NoteFontSize', $("#" + DashBoardSetting.params.PanelID + " #ddlNoteFontSize").val());
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlIsDefaultHPI").val() == "0") {
            SetGlobalAppData('IsDefaultHPI', "False");
        } else {
            SetGlobalAppData('IsDefaultHPI', "True");
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlSelectNoteComponents").val() == "0") {
            SetGlobalAppData('IsSelectNoteComponent', "False");
        } else {
            SetGlobalAppData('IsSelectNoteComponent', "True");
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlIsExpand").val() == "0") {
            SetGlobalAppData('IsExpand', "False");
        } else {
            SetGlobalAppData('IsExpand', "True");
        }

        if ($("#" + DashBoardSetting.params.PanelID + " #ddlIsSelectCompOnCopyNote").val() == "0") {
            SetGlobalAppData('IsSelectCompOnCopyNote', "False");
        } else {
            SetGlobalAppData('IsSelectCompOnCopyNote', "True");
        }
        //PRD-31 by:MAHMAD
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlIsExpandFolderTree").val() == "0") {
            SetGlobalAppData('IsExpandFolderTree', "False");
        } else {
            SetGlobalAppData('IsExpandFolderTree', "True");
        }
        //PRD-31 by:MAHMAD
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlIsSearchCriteriaExpand").val() == "0") {
            SetGlobalAppData('IsSearchCriteriaExpand', "False");
        } else {
            SetGlobalAppData('IsSearchCriteriaExpand', "True");
        }
        SetGlobalAppData('EMCodeTypeIds', DashBoardSetting.multipleEMCodeIds);
        if (typeof RaceIds != typeof undefined && (RaceIds != "" || RaceIds == "")) {
            SetGlobalAppData('RaceIds', RaceIds);
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlBlueButtonNote").val() != "") {
            if ($("#" + DashBoardSetting.params.PanelID + " #ddlBlueButtonNote").val() == "2")
                SetGlobalAppData('isLandOnComponent', "True");
            else
                SetGlobalAppData('isLandOnComponent', "False");
        }
        else
            SetGlobalAppData('isLandOnComponent', "False");
        

        // EMR - 4447 Fixation by Bilawal Khan
        SetGlobalAppData('NotePrevieStyle', $("#" + DashBoardSetting.params.PanelID + " #ddlNotePreviewStyle").val());



        //End Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
        SetGlobalAppData('DefaultDocumentPriorityId', $("#" + DashBoardSetting.params.PanelID + " #ddlDocumentPriority").val());
        SetGlobalAppData('DefaultDocumentPriorityName', $("#" + DashBoardSetting.params.PanelID + " #ddlDocumentPriority option:selected").text());
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlIsSchedulerTiemInterval").val() != "") {
            SetGlobalAppData('SchedulerTimeInterval', $("#" + DashBoardSetting.params.PanelID + " #ddlIsSchedulerTiemInterval").val());
        } else {
            SetGlobalAppData('SchedulerTimeInterval', 0);
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #chkShowSuccessMessages").prop('checked')) {
            SetGlobalAppData('IsShowSuccessMessages', 'True');
        } else {
            SetGlobalAppData('IsShowSuccessMessages', 'False');
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlOrdersExpand").val() == "0") {
            SetGlobalAppData('IsOrdersExpand', "False");
        } else {
            SetGlobalAppData('IsOrdersExpand', "True");
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #ddlResultsExpand").val() == "0") {
            SetGlobalAppData('IsResultsExpand', "False");
        } else {
            SetGlobalAppData('IsResultsExpand', "True");
        }
        SetGlobalAppData('isDemographics', $("#" + DashBoardSetting.params.PanelID + " #divchkDemographics #chkDemographics").is(":checked").toString());
        SetGlobalAppData('isMU3FamilyHistory', $("#" + DashBoardSetting.params.PanelID + " #divchkMU3FamilyHistory #chkMU3FamilyHistory").is(":checked").toString());
        SetGlobalAppData('isTransPubHealthAgHealthCareSurveys', $("#" + DashBoardSetting.params.PanelID + " #divchkTransmissionkHealthCareSurveys #chkTransmissionkHealthCareSurveys").is(":checked").toString());
        SetGlobalAppData('isTransmittoImmunizationRegistries', $("#" + DashBoardSetting.params.PanelID + " #divchkTransmitImmunizationRegistries #chkTransmitImmunizationRegistries").is(":checked").toString());
        SetGlobalAppData('isPatientHealthInformationCapture', $("#" + DashBoardSetting.params.PanelID + " #divchkPatientHealthInformationCapture #chkPatientHealthInformationCapture").is(":checked").toString());
        SetGlobalAppData('isTransPubHealthAgCaseReporting', $("#" + DashBoardSetting.params.PanelID + " #divchkTransimissionCaseReporting #chkTransimissionCaseReporting").is(":checked").toString());
        SetGlobalAppData('isTransitionCareDirectProject', $("#" + DashBoardSetting.params.PanelID + " #divchkTransitionCareDirectProject #chkTransitionCareDirectProject").is(":checked").toString());
        SetGlobalAppData('isImplantableDevices', $("#" + DashBoardSetting.params.PanelID + " #divchkImplantableDevices #chkImplantableDevices").is(":checked").toString());
        SetGlobalAppData('isTransitonCancerRegistries', $("#" + DashBoardSetting.params.PanelID + " #divchkTransitCancerRegistries #chkTransitCancerRegistries").is(":checked").toString());
        SetGlobalAppData('isDataSegmentationPrivacy', $("#" + DashBoardSetting.params.PanelID + " #divchkPrivacyDataSegment #chkPrivacyDataSegment").is(":checked").toString());
        SetGlobalAppData('isConsolidatedCDACreationPreformance', $("#" + DashBoardSetting.params.PanelID + " #divchkConsolidatedCDACreationPreformance #chkConsolidatedCDACreationPreformance").is(":checked").toString());
        SetGlobalAppData('isTransPubHealthAgAntimicobialUse', $("#" + DashBoardSetting.params.PanelID + " #divchkTransimissionAntimicobialUse #chkTransimissionAntimicobialUse").is(":checked").toString());
        SetGlobalAppData('isMU3SocPsycBehaviourHx', $("#" + DashBoardSetting.params.PanelID + " #divchkSocPsyBehaviourHx #chkSocPsyBehaviourHx").is(":checked").toString());
        SetGlobalAppData('isDataExport', $("#" + DashBoardSetting.params.PanelID + " #divchkDataExport #chkDataExport").is(":checked").toString());
        SetGlobalAppData('isCarePlan', $("#" + DashBoardSetting.params.PanelID + " #divchkCarePlan #chkCarePlan").is(":checked").toString());

        SetGlobalAppData('IsCMS65v7', $("#" + DashBoardSetting.params.PanelID + " #divMIPSAlerts #chkCMS65v7").is(":checked").toString());
        SetGlobalAppData('IsCMS69v6', $("#" + DashBoardSetting.params.PanelID + " #divMIPSAlerts #chkCMS69v6").is(":checked").toString());
        SetGlobalAppData('IsCMS68v7', $("#" + DashBoardSetting.params.PanelID + " #divMIPSAlerts #chkCMS68v7").is(":checked").toString());
        SetGlobalAppData('IsCMS138v6', $("#" + DashBoardSetting.params.PanelID + " #divMIPSAlerts #chkCMS138v6").is(":checked").toString());
        SetGlobalAppData('IsCMS165v6', $("#" + DashBoardSetting.params.PanelID + " #divMIPSAlerts #chkCMS165v6").is(":checked").toString());
        SetGlobalAppData('IsCMS22v6', $("#" + DashBoardSetting.params.PanelID + " #divMIPSAlerts #chkCMS22v6").is(":checked").toString());
        SetGlobalAppData('IsCDS', $("#" + DashBoardSetting.params.PanelID + " #divMIPSAlerts #chkCDS").is(":checked").toString());

        if ($("#" + DashBoardSetting.params.PanelID + " #divchkPrevPE #chkPrevNotePE").is(":checked")) {
            SetGlobalAppData('IsPreviousNotePE', 'True');
        } else {
            SetGlobalAppData('IsPreviousNotePE', 'False');
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #divchkbxPETemplateName #chkbxPETemplateName").is(":checked"))
            SetGlobalAppData('IsPETemplateNameRequired', 'True');
        else
            SetGlobalAppData('IsPETemplateNameRequired', 'False');

        if ($("#" + DashBoardSetting.params.PanelID + " #divchkPrevROS #chkPrevNoteROS").is(":checked")) {
            SetGlobalAppData('IsPreviousNoteROS', 'True');
        } else {
            SetGlobalAppData('IsPreviousNoteROS', 'False');
        }

        if ($("#" + DashBoardSetting.params.PanelID + " #divchkPrevComplaints #chkPrevComplaints").is(":checked")) {
            SetGlobalAppData('IsPreviousNoteComplaints', 'True');
        } else {
            SetGlobalAppData('IsPreviousNoteComplaints', 'False');
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #divchkPrevProblmes #chkPrevNoteProblems").is(":checked")) {
            SetGlobalAppData('IsPreviousNoteProblems', 'True');
        } else {
            SetGlobalAppData('IsPreviousNoteProblems', 'False');
        }
        if ($("#" + DashBoardSetting.params.PanelID + " #divPrevNoteTreatmentPlanComments #chkPrevNoteTreatmentPlanComments").is(":checked")) {
            SetGlobalAppData('IsPrevNoteTreatmentComents', 'True');
        } else {
            SetGlobalAppData('IsPrevNoteTreatmentComents', 'False');
        }

        SetGlobalAppData('DefaultTabMedications', $("#" + DashBoardSetting.params.PanelID + " #ddlDefTabMedications").val());
        if ($("#" + DashBoardSetting.params.PanelID + " #divchkIsNoteCompExpanded #chkIsNoteCompExpanded").is(":checked")) {
            SetGlobalAppData('IsNoteCompExpanded', 'True');
        } else {
            SetGlobalAppData('IsNoteCompExpanded', 'False');
        }
    },
    resetAlerts: function () {
        localStorage.removeItem(globalAppdata.AppUserName);
        utility.DisplayMessages("Successfully Updated", 1);
    },
    DefaultSettingSave: function () {

        var self = $("#" + DashBoardSetting.params.PanelID);
        if (self.find('#divMIPSAlerts input.chkMIPSPI[type="checkbox"]:not(:checked)').length == 0 && self.find('#divMIPSAlerts input.chkMIPSCQM[type="checkbox"]:not(:checked)').length == 0 && self.find('#divMIPSAlerts input.chkMIPSIA[type="checkbox"]:not(:checked)').length == 0)
            DashBoardSetting.DefaultSettingSave_(self);
        else {
            utility.myConfirm('65', function () {
                DashBoardSetting.DefaultSettingSave_(self);
            }, function () {
            }, '1');
        }
    },
    DefaultSettingSave_: function (self) {

        var myJSON = self.getMyJSON();
        var EMCodeTypeIds = self.find('#ddlEMCodeType option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var RaceIds = self.find('#ddlRaceIds option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var weekIDs = '';
        self.find('#ddlSchedulerWorkWeekDays option').each(function (i, val) {

            weekIDs += val.selected == true ? '1,' : '0,';

        });
        var iTrackDashboardIds = self.find("#ddliTrackDashboard option:Selected").map(function () {
            return this.value;
        }).get().join(',');
        var objData = JSON.parse(myJSON);
        objData["EMCodeTypeIds"] = EMCodeTypeIds;
        objData["WeekWorkDaysIds"] = weekIDs;
        objData["iTrackDashboardIds"] = iTrackDashboardIds;
        objData["RaceIds"] = RaceIds;
        myJSON = JSON.stringify(objData);
        DashBoardSetting.multipleEMCodeIds = EMCodeTypeIds;
        globalAppdata['IsReferralRequired'] = $("#" + DashBoardSetting.params.PanelID + " #chkreferralRequired").prop("checked");
        if (globalAppdata['EntityUserOptionId'] == "" || globalAppdata['EntityUserOptionId'] == undefined) {

            DashBoardSetting.SaveDefaultSetting(myJSON).done(function (response) {
                if (response.status != false) {
                    globalAppdata['EntityUserOptionId'] = response.EntityUserOptionId;
                    DashBoardSetting.SetDefaultSetting(RaceIds);
                    utility.DisplayMessages(response.message, 1);
                    DashBoardSetting.ChangeThemeColor(false);
                    DashBoard.LoadDashboardAppointmentCount();
                    DashBoardSetting.UnLoad();
                    DashBoardSetting.QuickLinksSettings(myJSON);
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    if (scheduler) {
                        PMSScheduler.SchedulerGetMinorTickCount();
                        PMSScheduler.SetSchedulerMinorMajorTick();
                    }

                    if (DashBoardSetting.params.ParentCtrl == "mstrTabDashBoard") {
                        //$('#pnlDashboard #pnlSchAppointmentGrid #divSwitch #switchVisit').attr('VisitStatus', globalAppdata["PreferredAppointmentStatus"]);
                        DashBoard.IsCheckedIn($('#pnlDashboard #pnlSchAppointmentGrid #divSwitch #switchVisit'));
                    } else if (DashBoardSetting.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_ProgressNote.ChangeNoteFontSize()
                    }

                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });


        } else if (globalAppdata['EntityUserOptionId'] != "" || globalAppdata['EntityUserOptionId'] != undefined) {
            var objData = JSON.parse(myJSON);
            objData["FacilityDescription"] = $('#ddlDefaultFacility option:selected').attr('FacilityDescription');
            var weeklylist = '';
            self.find('#divSchedulerWorkWeekDays option').each(function (i, val) {
                weeklylist += val.selected == true ? '1,' : '0,';

            });
            objData["WorkingDaysList"] = weeklylist;
            myJSON = JSON.stringify(objData);
            DashBoardSetting.UpdateDefaultSetting(myJSON, globalAppdata['EntityUserOptionId']).done(function (response) {
                if (response.status != false) {
                    DashBoardSetting.SetDefaultSetting(RaceIds);
                    utility.DisplayMessages(response.message, 1);
                    //UnloadActionPan();
                    DashBoardSetting.ChangeThemeColor(false);
                    DashBoard.LoadDashboardAppointmentCount();
                    DashBoardSetting.UnLoad();
                    DashBoardSetting.QuickLinksSettings(myJSON);
                    if (!utility.CheckPageLoaded("mdvisionadmin.aspx") && !utility.CheckPageLoaded("mdvisionbilling.aspx") && !utility.CheckPageLoaded("mdvisionauditableevents.aspx")) {
                        PMSScheduler.SchedulerGetMinorTickCount();
                        PMSScheduler.SetSchedulerMinorMajorTick();
                    }
                    if (DashBoardSetting.params.ParentCtrl == "mstrTabDashBoard") {
                        //$('#pnlDashboard #pnlSchAppointmentGrid #divSwitch #switchVisit').attr('VisitStatus', globalAppdata["PreferredAppointmentStatus"]);
                        //DashBoard.IsCheckedIn($('#pnlDashboard #pnlSchAppointmentGrid #divSwitch #switchVisit'));
                        $('#pnlDashboard #widgetpanel div').removeClass("active");
                        $('#pnlDashboard #widgetpanel div:first-child').addClass("active");
                        DashBoard.LoadWidgetControl('Orders&Results', null);
                    } else if (DashBoardSetting.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_ProgressNote.ChangeNoteFontSize();
                    }
                    else if (DashBoardSetting.params.ParentCtrl == "clinicalTabNotes") {
                        Clinical_Notes.DashBoardDefaultCheckBoxes();
                    }
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }


    },
    getPrivileges: function () {
        var DetailInfoObj = [];
        var objDetail = {};
        var FormName = [];
        var Permission = [];
        FormName.push("Notes_Phone Encounter");
        Permission.push("ADD");
        FormName.push("Notes_Phone Encounter");
        Permission.push("EDIT");
        FormName.push("Notes_Notes");
        Permission.push("ADD");
        FormName.push("Notes_Notes");
        Permission.push("EDIT");
        objDetail["FormName"] = FormName;
        objDetail["Permission"] = Permission;
        // DetailInfoObj.push(objDetail);
        var data = JSON.stringify(objDetail);
        return MDVisionService.APIService(data, "FormPrivilege", "AppPrivileges");
    },
    DefaultSettingFill: function () {
        DashBoardSetting.FillDefaultSettings(globalAppdata.AppUserName).done(function (response) {
            if (response.status == true) {

                DashBoardSetting.getPrivileges().done(function (response) {
                    var isAllowed = false;
                    if (response.status != false) {
                        var jobj = JSON.parse(response);
                        var obj = JSON.parse(jobj.responsePrivilages_JSON);
                        $.each(obj, function (index, Item) {
                            if (Item.FormName == "Notes_Phone Encounter") {
                                if (Item.privilegasMessage == "") {
                                    isAllowed = true;
                                }
                            } else if (Item.FormName == "Notes_Notes") {
                                if (Item.privilegasMessage == "") {
                                    isAllowed = true;
                                }
                            }
                            if (isAllowed == true) {
                                $('#divAutoPopulateNote').removeClass('hidden');
                                $('#divTransferDatatoNewNote').removeClass('hidden');
                            }

                        });

                    }
                });
                var self = $('#' + DashBoardSetting.params.PanelID);
                //utility.bindMyJSON(true, JSON.parse(response.EntityGroupLoad_JSON), false, self);
                if (response.EntityGroupCount > 0) {
                    var keys = Object.keys(JSON.parse(response.EntityGroupLoad_JSON));
                    var entityJSON = JSON.parse(response.EntityGroupLoad_JSON);


                    if (entityJSON.ddlDefaultTheme == "1") {
                        DashBoardSetting.selected_theme = "Black";
                    } else if (entityJSON.ddlDefaultTheme == "3") {
                        DashBoardSetting.selected_theme = "Gray";
                    } else {
                        DashBoardSetting.selected_theme = "Blue";
                    }

                    //-- is collection----

                    if (globalAppdata["IsCollection"].toLowerCase() != "true") {
                        $("#divCollectionBalance").hide();
                    }
                    //////////////////////////


                    // ----- Quick Links Fill
                    if (entityJSON.switchSearchPatient == "False") {
                        $('#divSwitchSearchPat #switchSearchPatient').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchSearchPat #switchSearchPatient input').attr('checked', false);
                    }
                    if (entityJSON.switchQuickPat == "False") {
                        $('#divSwitchQuickPat #switchQuickPat').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchQuickPat #switchQuickPat input').attr('checked', false);
                    }
                    if (entityJSON.switchMessages == "False") {
                        $('#divSwitchMessages #switchMessages').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchMessages #switchMessages input').attr('checked', false);
                    }
                    if (entityJSON.switchAppointments == "False") {
                        $('#divSwitchAppointments #switchAppointments').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchAppointments #switchAppointments input').attr('checked', false);
                    }
                    if (entityJSON.switchNotes == "False") {
                        $('#divSwitchNotes #switchNotes').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchNotes #switchNotes input').attr('checked', false);
                    }
                    if (entityJSON.switchFax == "False") {
                        $('#divSwitchFax #switchFax').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchFax #switchFax input').attr('checked', false);
                    }
                    if (entityJSON.switchTasks == "False") {
                        $('#divSwitchTasks #switchTasks').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchTasks #switchTasks input').attr('checked', false);
                    }
                    if (entityJSON.switchPrescriptionsRefill == "False") {
                        $('#divSwitchPrescriptionsRefill #switchPrescriptionsRefill').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchPrescriptionsRefill #switchPrescriptionsRefill input').attr('checked', false);
                    }
                    if (entityJSON.switchPendingPrescriptions == "False") {
                        $('#divSwitchPendingPrescriptions #switchPendingPrescriptions').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchPendingPrescriptions #switchPendingPrescriptions input').attr('checked', false);
                    }
                    if (entityJSON.switchImmunizationAlert == "False") {
                        $('#divSwitchImmunizationAlert #switchImmunizationAlert').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchImmunizationAlert #switchImmunizationAlert input').attr('checked', false);
                    }
                    if (entityJSON.switchDocumentsAlert == "False") {
                        $('#divSwitchDocumentsAlert #switchDocumentsAlert').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchDocumentsAlert #switchDocumentsAlert input').attr('checked', false);
                    }
                    if (entityJSON.switchAssignedResultsAlert == "False") {
                        $('#divSwitchAssignedResults #switchAssignedResults').children().first().removeClass('ios-switch on').addClass('ios-switch off');
                        $('#divSwitchAssignedResults #switchAssignedResults input').attr('checked', false);
                    }
                    // ------ End Quick Links Fill

                    //Start//03-11-2016//Ahmad Raza//Note AutoPopulate fill logic

                    if (entityJSON.chkActiveAllergies == "False") {
                        $('#divchkActiveAllergies #chkActiveAllergies input').attr('checked', false);
                    }
                    if (entityJSON.chkActiveProblems == "False") {
                        $('#divchkActiveProblems #chkActiveProblems input').attr('checked', false);
                    }
                    if (entityJSON.chkActiveProcedures == "False") {
                        $('#divchkActiveProcedures #chkActiveProcedures input').attr('checked', false);
                    } else {
                        $('#divchkActiveProcedures #chkActiveProcedures input').prop('checked', true);
                    }

                    if (entityJSON.isLandOnComponent == "2")
                        SetGlobalAppData('isLandOnComponent', "True");
                    else
                        SetGlobalAppData('isLandOnComponent', "False");
                    if (entityJSON.chkInActiveProcedures == "False") {
                        $('#divchkInActiveProcedures #chkInActiveProcedures input').attr('checked', false);
                    } else {
                        $('#divchkInActiveProcedures #chkInActiveProcedures input').prop('checked', true);
                    }
                    if (entityJSON.chkBirthHistory == "False") {
                        $('#divchkBirthHistory #chkBirthHistory input').attr('checked', false);
                    }
                    if (entityJSON.chkCurrentMedications == "False") {
                        $('#divchkCurrentMedications #chkCurrentMedications input').attr('checked', false);
                    }
                    if (entityJSON.chkFamilyHistory == "False") {
                        $('#divchkFamilyHistory #chkFamilyHistory input').attr('checked', false);
                    }
                    if (entityJSON.chkHospitalHistory == "False") {
                        $('#divchkHospitalHistory #chkHospitalHistory input').attr('checked', false);
                    }
                    if (entityJSON.chkImmunizations == "False") {
                        $('#divchkImmunizations #chkImmunizations input').attr('checked', false);
                    }
                    if (entityJSON.chkInActiveAllergies == "False") {
                        $('#divchkInActiveAllergies #chkInActiveAllergies input').attr('checked', false);
                    }
                    if (entityJSON.chkInActiveProblems == "False") {
                        $('#divchkInActiveProblems #chkInActiveProblems input').attr('checked', false);
                    }
                    if (entityJSON.chkMedicalHistory == "False") {
                        $('#divchkMedicalHistory #chkMedicalHistory input').attr('checked', false);
                    }
                    if (entityJSON.chkPastMedications == "False") {
                        $('#divchkPastMedications #chkPastMedications input').attr('checked', false);
                    }
                    if (entityJSON.chkSocialHistory == "False") {
                        $('#divchkSocialHistory #chkSocialHistory input').attr('checked', false);
                    }
                    if (entityJSON.chkSurgicalHistory == "False") {
                        $('#divchkSurgicalHistory #chkSurgicalHistory input').attr('checked', false);
                    }
                    if (entityJSON.chkVitals == "False") {
                        $('#divchkVitals #chkVitals input').attr('checked', false);
                    }
                    if (entityJSON.chkshowICD10 == "False") {
                        $('#divchkshowICD10 #chkshowICD10 input').attr('checked', false);
                    }

                    if (entityJSON.chkFacilityShortName.toLowerCase() == "false") {
                        $('#divchkFacilityShortName #chkFacilityShortName').attr('checked', false);
                    }
                    else if (entityJSON.chkFacilityShortName.toLowerCase() == "true") {
                        $('#divchkFacilityShortName #chkFacilityShortName').attr('checked', true);
                    }
                    if (entityJSON.chkreferralRequired.toLowerCase() == "false") {
                        $('#divchkReferralRequired #chkreferralRequired').attr('checked', false);
                    }
                    else if (entityJSON.chkreferralRequired.toLowerCase() == "true") {
                        $('#divchkReferralRequired #chkreferralRequired').attr('checked', true);
                    }
                    if (entityJSON.chkbxPETemplateNameRequired.toLowerCase() == "false") {
                        $('#divchkbxPETemplateName #chkbxPETemplateName').attr('checked', false);
                    }
                    else if (entityJSON.chkbxPETemplateNameRequired.toLowerCase() == "true") {
                        $('#divchkbxPETemplateName #chkbxPETemplateName').attr('checked', true);
                    }
                    if (entityJSON.chkSendBillingInquiry.toLowerCase() == "false") {
                        $('#divchkSendBillingInquiry #chkSendBillingInquiry').attr('checked', false);
                    }
                    else if (entityJSON.chkSendBillingInquiry.toLowerCase() == "true") {
                        $('#divchkSendBillingInquiry #chkSendBillingInquiry').attr('checked', true);
                    }
                    if (entityJSON.chkShowSuccessMessages.toLowerCase() == "false") {
                        $('#divchkShowSuccessMessages #chkShowSuccessMessages').attr('checked', false);
                    }
                    else if (entityJSON.chkShowSuccessMessages.toLowerCase() == "true") {
                        $('#divchkShowSuccessMessages #chkShowSuccessMessages').attr('checked', true);
                    }

                    if (entityJSON.chkSocPsyandBehaviorHx == "False") {
                        $('#divchkSocPsyandBehaviorHx #chkSocPsyandBehaviorHx input').attr('checked', false);
                    } else {
                        $('#divchkSocPsyandBehaviorHx #chkSocPsyandBehaviorHx input').prop('checked', true);
                    }

                    if (entityJSON.chkPatientName == "False") {
                        $('#divchkPatientName #chkPatientName input').attr('checked', false);
                    } else {
                        $('#divchkPatientName #chkPatientName input').prop('checked', true);
                    }

                    if (entityJSON.chkDateOfBirth == "False") {
                        $('#divchkDateOfBirth #chkDateOfBirth input').attr('checked', false);
                    } else {
                        $('#divchkDateOfBirth #chkDateOfBirth input').prop('checked', true);
                    }

                    if (entityJSON.chkAddress == "False") {
                        $('#divchkAddress #chkAddress input').attr('checked', false);
                    } else {
                        $('#divchkAddress #chkAddress input').prop('checked', true);
                    }
                    if (entityJSON.chkInsurancePlan == "False") {
                        $('#divchkInsurancePlan #chkInsurancePlan input').attr('checked', false);
                    } else {
                        $('#divchkInsurancePlan #chkInsurancePlan input').prop('checked', true);
                    }
                    if (entityJSON.chkSubscriberID == "False") {
                        $('#divchkSubscriberID #chkSubscriberID input').attr('checked', false);
                    } else {
                        $('#divchkSubscriberID #chkSubscriberID input').prop('checked', true);
                    }
                    if (entityJSON.chkPatientAccountNo == "False") {
                        $('#divchkPatientAccountNo #chkPatientAccountNo input').attr('checked', false);
                    } else {
                        $('#divchkPatientAccountNo #chkPatientAccountNo input').prop('checked', true);
                    }
                    if (entityJSON.chkProvider == "False") {
                        $('#divchkProvider #chkProvider input').attr('checked', false);
                    } else {
                        $('#divchkProvider #chkProvider input').prop('checked', true);
                    }
                    //End//03-11-2016//Ahmad Raza//Note AutoPopulate fill logic
                    for (var i = 0; i < keys.length; i++) {
                        self.find('#' + keys[i]).val(JSON.parse(response.EntityGroupLoad_JSON)[keys[i]]);
                    }
                    if (entityJSON.ddlPreferredSchedulerScreen_text && entityJSON.ddlPreferredSchedulerScreen_text != "") {
                        self.find('#ddlPreferredSchedulerScreen option').filter(function () {
                            return $.trim($(this).text().toLowerCase()) == entityJSON.ddlPreferredSchedulerScreen_text.toLowerCase();
                        }).prop('selected', true);
                    }
                    if (entityJSON.ddlPreferredBillingScreen_text && entityJSON.ddlPreferredBillingScreen_text != "") {
                        self.find('#ddlPreferredBillingScreen option').filter(function () {
                            return $.trim($(this).text().toLowerCase()) == entityJSON.ddlPreferredBillingScreen_text.toLowerCase();
                        }).prop('selected', true);
                    }
                    //if (entityJSON.ddlENMCodesTime && entityJSON.ddlENMCodesTime != "") {
                    //    self.find('#ddlENMCodesTime option').filter(function () {
                    //        return $(this).val() == entityJSON.ddlENMCodesTime;
                    //    }).prop('selected', true);
                    //}
                    if (entityJSON.ddlNoteFontSize && entityJSON.ddlNoteFontSize != "") {
                        self.find('#ddlNoteFontSize option').filter(function () {
                            return $(this).val() == entityJSON.ddlNoteFontSize;
                        }).prop('selected', true);
                    }
                    if (entityJSON.ddlSelectNoteComponents == "False") {
                        self.find('#ddlSelectNoteComponents').val(0);
                    } else {
                        self.find('#ddlSelectNoteComponents').val(1);
                    }

                    if (entityJSON.ddlIsExpand == "False") {
                        self.find('#ddlIsExpand').val(0);
                    } else {
                        self.find('#ddlIsExpand').val(1);
                    }
                    if (entityJSON.ddlIsSearchCriteriaExpand == "False") {
                        self.find('#ddlIsSearchCriteriaExpand').val(0);
                    } else {
                        self.find('#ddlIsSearchCriteriaExpand').val(1);
                    }
                    if (entityJSON.ddlIsSelectCompOnCopyNote == "False") {
                        self.find('#ddlIsSelectCompOnCopyNote').val(0);
                    } else {
                        self.find('#ddlIsSelectCompOnCopyNote').val(1);
                    }
                    //PRD-31 by:MAHMAD
                    if (entityJSON.ddlIsExpandFolderTree == "False") {
                        self.find('#ddlIsExpandFolderTree').val(0);
                    } else {
                        self.find('#ddlIsExpandFolderTree').val(1);
                    }
                    //PRD-31 by:MAHMAD
                    if (entityJSON.NotePreviewStyle)
                        self.find('#ddlNotePreviewStyle').val(entityJSON.NotePreviewStyle);

                    if (entityJSON.ddlDefTabMedications == 'Prescriptions')       //PRD-735 TahreeMalik
                        self.find('#ddlDefTabMedications').val(entityJSON.ddlDefTabMedications);
                    else
                        self.find('#ddlDefTabMedications').val("Medications");

                    if (entityJSON.ddlIsDefaultHPI == "False") {
                        self.find('#ddlIsDefaultHPI').val(0);
                    } else {
                        self.find('#ddlIsDefaultHPI').val(1);
                    }
                    if (entityJSON.EMCodeTypeIds != 'undefined' && entityJSON.EMCodeTypeIds != '') {
                        $('#' + DashBoardSetting.params.PanelID + " #ddlEMCodeType").val(entityJSON.EMCodeTypeIds.split(','));

                        $('#' + DashBoardSetting.params.PanelID + " #ddlEMCodeType").multiselect("refresh");
                        $('#' + DashBoardSetting.params.PanelID + " #ddlEMCodeType").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        DashBoardSetting.multipleEMCodeIds = entityJSON.EMCodeTypeIds;
                    } else {
                        $('#' + DashBoardSetting.params.PanelID + " #ddlEMCodeType").find("option:selected").removeAttr("selected");
                        $('#' + DashBoardSetting.params.PanelID + " #ddlEMCodeType").multiselect("refresh");
                    }
                    if (entityJSON.WeekWorkDaysIds != 'undefined' && entityJSON.WeekWorkDaysIds != '') {
                        $('#' + DashBoardSetting.params.PanelID + " #ddlSchedulerWorkWeekDays").val(entityJSON.WeekWorkDaysIds.split(','));

                        $('#' + DashBoardSetting.params.PanelID + " #ddlSchedulerWorkWeekDays").multiselect("refresh");
                        $('#' + DashBoardSetting.params.PanelID + " #ddlSchedulerWorkWeekDays").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        DashBoardSetting.multipleWeekWorkDaysIds = entityJSON.WeekWorkDaysIds;

                    } else {
                        $('#' + DashBoardSetting.params.PanelID + " #ddlSchedulerWorkWeekDays").find("option:selected").removeAttr("selected");
                        $('#' + DashBoardSetting.params.PanelID + " #ddlSchedulerWorkWeekDays").multiselect("refresh");
                    }
                    if (entityJSON.iTrackDashboardIds && entityJSON.iTrackDashboardIds != '') {
                        $('#' + DashBoardSetting.params.PanelID + " #ddliTrackDashboard").val(entityJSON.iTrackDashboardIds.split(','));

                        $('#' + DashBoardSetting.params.PanelID + " #ddliTrackDashboard").multiselect("refresh");
                        $('#' + DashBoardSetting.params.PanelID + " #ddliTrackDashboard").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        DashBoardSetting.multipleiTrackDashboardIds = entityJSON.WeekWorkDaysIds;

                    } else {
                        $('#' + DashBoardSetting.params.PanelID + " #ddliTrackDashboard").find("option:selected").removeAttr("selected");
                        $('#' + DashBoardSetting.params.PanelID + " #ddliTrackDashboard").multiselect("refresh");
                    }
                    if (entityJSON.RaceIds != 'undefined' && entityJSON.RaceIds != '') {
                        $('#' + DashBoardSetting.params.PanelID + " #ddlRaceIds").val(entityJSON.RaceIds.split(','));

                        $('#' + DashBoardSetting.params.PanelID + " #ddlRaceIds").multiselect("refresh");
                        $('#' + DashBoardSetting.params.PanelID + " #ddlRaceIds").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        DashBoardSetting.multipleEMCodeIds = entityJSON.RaceIds;
                    } else {
                        $('#' + DashBoardSetting.params.PanelID + " #ddlRaceIds").find("option:selected").removeAttr("selected");
                        $('#' + DashBoardSetting.params.PanelID + " #ddlRaceIds").multiselect("refresh");
                    }
                    if (entityJSON.chkPrevComplaints == "False") {
                        $('#divchkPrevComplaints #chkPrevComplaints').attr('checked', false);
                    }
                    if (entityJSON.chkPrevNoteROS == "False") {
                        $('#divchkPrevROS #chkPrevNoteROS').attr('checked', false);
                    }
                    if (entityJSON.chkPrevNotePE == "False") {
                        $('#divchkPrevPE #chkPrevNotePE').attr('checked', false);
                    }
                    if (entityJSON.chkPrevNoteProblems == "False") {
                        $('#divchkPrevProblmes #chkPrevNoteProblems').attr('checked', false);
                    }
                    if (entityJSON.chkPrevNoteTreatmentPlanComments == "True") {
                        $('#divPrevNoteTreatmentPlanComments #chkPrevNoteTreatmentPlanComments').attr('checked', true);
                    }
                    else {
                        $('#divPrevNoteTreatmentPlanComments #chkPrevNoteTreatmentPlanComments').attr('checked', false);
                    }
                    //Begin Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
                    DashBoardSetting.GetNotesTemplates();
                    //End Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885

                    if (entityJSON.ddlOrdersExpand == "False") {
                        self.find('#ddlOrdersExpand').val(0);
                    } else {
                        self.find('#ddlOrdersExpand').val(1);
                    }

                    if (entityJSON.ddlResultsExpand == "False") {
                        self.find('#ddlResultsExpand').val(0);
                    } else {
                        self.find('#ddlResultsExpand').val(1);
                    }
                    if (entityJSON.chkIsNoteCompExpanded == "True") {
                        $('#divchkIsNoteCompExpanded #chkIsNoteCompExpanded').attr('checked', true);
                    }
                    else {
                        $('#divchkIsNoteCompExpanded #chkIsNoteCompExpanded').attr('checked', false);
                    }

                    entityJSON.chkDemographics == "True" ? $('#divchkDemographics #chkDemographics').attr('checked', true) : $('#divchkDemographics #chkDemographics').attr('checked', false);
                    entityJSON.chkMU3FamilyHistory == "True" ? $('#divchkMU3FamilyHistory #chkMU3FamilyHistory').attr('checked', true) : $('#divchkMU3FamilyHistory #chkMU3FamilyHistory').attr('checked', false);
                    entityJSON.chkTransmissionkHealthCareSurveys == "True" ? $('#divchkTransmissionkHealthCareSurveys #chkTransmissionkHealthCareSurveys').attr('checked', true) : $('#divchkTransmissionkHealthCareSurveys #chkTransmissionkHealthCareSurveys').attr('checked', false);
                    entityJSON.chkTransmitImmunizationRegistries == "True" ? $('#divchkTransmitImmunizationRegistries #chkTransmitImmunizationRegistries').attr('checked', true) : $('#divchkTransmitImmunizationRegistries #chkTransmitImmunizationRegistries').attr('checked', false);
                    entityJSON.chkPatientHealthInformationCapture == "True" ? $('#divchkPatientHealthInformationCapture #chkPatientHealthInformationCapture').attr('checked', true) : $('#divchkPatientHealthInformationCapture #chkPatientHealthInformationCapture').attr('checked', false);
                    entityJSON.chkTransimissionCaseReporting == "True" ? $('#divchkTransimissionCaseReporting #chkTransimissionCaseReporting').attr('checked', true) : $('#divchkTransimissionCaseReporting #chkTransimissionCaseReporting').attr('checked', false);
                    entityJSON.chkTransitionCareDirectProject == "True" ? $('#divchkTransitionCareDirectProject #chkTransitionCareDirectProject').attr('checked', true) : $('#divchkTransitionCareDirectProject #chkTransitionCareDirectProject').attr('checked', false);
                    entityJSON.chkPrivacyDataSegment == "True" ? $('#divchkPrivacyDataSegment #chkPrivacyDataSegment').attr('checked', true) : $('#divchkPrivacyDataSegment #chkPrivacyDataSegment').attr('checked', false);
                    entityJSON.chkImplantableDevices == "True" ? $('#divchkImplantableDevices #chkImplantableDevices').attr('checked', true) : $('#divchkImplantableDevices #chkImplantableDevices').attr('checked', false);
                    entityJSON.chkTransitCancerRegistries == "True" ? $('#divchkTransitCancerRegistries #chkTransitCancerRegistries').attr('checked', true) : $('#divchkTransitCancerRegistries #chkTransitCancerRegistries').attr('checked', false);
                    entityJSON.chkConsolidatedCDACreationPreformance == "True" ? $('#divchkConsolidatedCDACreationPreformance #chkConsolidatedCDACreationPreformance').attr('checked', true) : $('#divchkConsolidatedCDACreationPreformance #chkConsolidatedCDACreationPreformance').attr('checked', false);
                    entityJSON.chkTransimissionAntimicobialUse == "True" ? $('#divchkTransimissionAntimicobialUse #chkTransimissionAntimicobialUse').attr('checked', true) : $('#divchkTransimissionAntimicobialUse #chkTransimissionAntimicobialUse').attr('checked', false);
                    entityJSON.chkSocPsyBehaviourHx == "True" ? $('#divchkSocPsyBehaviourHx #chkSocPsyBehaviourHx').attr('checked', true) : $('#divchkSocPsyBehaviourHx #chkSocPsyBehaviourHx').attr('checked', false);
                    entityJSON.chkDataExport == "True" ? $('#divchkDataExport #chkDataExport').attr('checked', true) : $('#divchkDataExport #chkDataExport').attr('checked', false);
                    entityJSON.chkCarePlan == "True" ? $('#divchkCarePlan #chkCarePlan').attr('checked', true) : $('#divchkCarePlan #chkCarePlan').attr('checked', false);

                    entityJSON.chkeAccess == "True" ? $('#divMIPSAlerts #chkeAccess').attr('checked', true) : $('#divMIPSAlerts #chkeAccess').attr('checked', false);
                    entityJSON.chkePrescribing == "True" ? $('#divMIPSAlerts #chkePrescribing').attr('checked', true) : $('#divMIPSAlerts #chkePrescribing').attr('checked', false);
                    entityJSON.chkInCorporateSummaryOfCare == "True" ? $('#divMIPSAlerts #chkInCorporateSummaryOfCare').attr('checked', true) : $('#divMIPSAlerts #chkInCorporateSummaryOfCare').attr('checked', false);
                    entityJSON.chkPatientDocument == "True" ? $('#divMIPSAlerts #chkPatientDocument').attr('checked', true) : $('#divMIPSAlerts #chkPatientDocument').attr('checked', false);
                    entityJSON.chkPatientEducation == "True" ? $('#divMIPSAlerts #chkPatientEducation').attr('checked', true) : $('#divMIPSAlerts #chkPatientEducation').attr('checked', false);
                    entityJSON.chkReconciliation == "True" ? $('#divMIPSAlerts #chkReconciliation').attr('checked', true) : $('#divMIPSAlerts #chkReconciliation').attr('checked', false);
                    entityJSON.chkSecureMessaging == "True" ? $('#divMIPSAlerts #chkSecureMessaging').attr('checked', true) : $('#divMIPSAlerts #chkSecureMessaging').attr('checked', false);
                    entityJSON.chkTransitionOfCare == "True" ? $('#divMIPSAlerts #chkTransitionOfCare').attr('checked', true) : $('#divMIPSAlerts #chkTransitionOfCare').attr('checked', false);
                    entityJSON.chkViewDownloadTransmit == "True" ? $('#divMIPSAlerts #chkViewDownloadTransmit').attr('checked', true) : $('#divMIPSAlerts #chkViewDownloadTransmit').attr('checked', false);

                    entityJSON.chkCMS65v7 == "True" ? $('#divMIPSAlerts #chkCMS65v7').attr('checked', true) : $('#divMIPSAlerts #chkCMS65v7').attr('checked', false);
                    entityJSON.chkCMS69v6 == "True" ? $('#divMIPSAlerts #chkCMS69v6').attr('checked', true) : $('#divMIPSAlerts #chkCMS69v6').attr('checked', false);
                    entityJSON.chkCMS68v7 == "True" ? $('#divMIPSAlerts #chkCMS68v7').attr('checked', true) : $('#divMIPSAlerts #chkCMS68v7').attr('checked', false);
                    entityJSON.chkCMS138v6 == "True" ? $('#divMIPSAlerts #chkCMS138v6').attr('checked', true) : $('#divMIPSAlerts #chkCMS138v6').attr('checked', false);
                    entityJSON.chkCMS165v6 == "True" ? $('#divMIPSAlerts #chkCMS165v6').attr('checked', true) : $('#divMIPSAlerts #chkCMS165v6').attr('checked', false);
                    entityJSON.chkCMS22v6 == "True" ? $('#divMIPSAlerts #chkCMS22v6').attr('checked', true) : $('#divMIPSAlerts #chkCMS22v6').attr('checked', false);

                    entityJSON.chkDepression == "True" ? $('#divMIPSAlerts #chkDepression').attr('checked', true) : $('#divMIPSAlerts #chkDepression').attr('checked', false);
                    entityJSON.chkTobacco == "True" ? $('#divMIPSAlerts #chkTobacco').attr('checked', true) : $('#divMIPSAlerts #chkTobacco').attr('checked', false);
                    entityJSON.chkPatientPortalDocument == "True" ? $('#divMIPSAlerts #chkPatientPortalDocument').attr('checked', true) : $('#divMIPSAlerts #chkPatientPortalDocument').attr('checked', false);
                    entityJSON.chkCDS == "True" ? $('#divMIPSAlerts #chkCDS').attr('checked', true) : $('#divMIPSAlerts #chkCDS').attr('checked', false);


                    DashBoardSetting.checkOrUncheckSelectAll();
                    DashBoardSetting.checkOrUncheckSelectAllMU3();
                    DashBoardSetting.checkOrUncheckSelectAllMIPS();
                }
            } else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    IntializeMultiSelectDropDownEMCodesType: function () {
        $('#' + DashBoardSetting.params.PanelID + ' #ddlEMCodeType').multiselect('destroy');
        $('#' + DashBoardSetting.params.PanelID + ' #ddlEMCodeType').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 150,
        });
    },

    IntializeMultiSelectDropDownWorkWeekDays: function () {
        $('#' + DashBoardSetting.params.PanelID + ' #ddlSchedulerWorkWeekDays').multiselect('destroy');
        $('#' + DashBoardSetting.params.PanelID + ' #ddlSchedulerWorkWeekDays').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 150,
        });
    },
    IntializeMultiSelectDropDownWorkWeek: function () {
        $('#' + DashBoardSetting.params.PanelID + ' #ddlSchedulerWorkWeekDays').multiselect('destroy');

        $('#' + DashBoardSetting.params.PanelID + ' #ddlSchedulerWorkWeekDays').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 150,
        });
    },
    IntializeMultiSelectDropDownRace: function () {
        $('#' + DashBoardSetting.params.PanelID + ' #ddlRaceIds').multiselect('destroy');
        $('#' + DashBoardSetting.params.PanelID + ' #ddlRaceIds').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 150,
        });
    },
    IntializeMultiSelectDropDowniTrackDashboard: function () {
        $('#' + DashBoardSetting.params.PanelID + ' #ddliTrackDashboard').multiselect('destroy');
        $('#' + DashBoardSetting.params.PanelID + ' #ddliTrackDashboard').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 150,
        });
    },
    enableDisableDropDownLists: function (ddlIds) {
        if (typeof ddlIds != 'undefined' && ddlIds != null) {
            //-------------------------------
            var self = $('#pnldashboardsetting');
            var codeIds = self.find('#divEMCodeTypes ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                return this.value;
            }).get().join(',');
            //--------------------------------
            if (codeIds.length > 0) {
                ddlCommaSeparatedIds = ddlIds.split(',');
                var parrentPanelId = "#" + DashBoardSetting.params["PanelID"];
                $.each(ddlCommaSeparatedIds, function (index, Item) {
                    $(parrentPanelId + " #" + Item).multiselect('enable');
                });
                DashBoardSetting.multipleEMCodeIds = ddlIds;
            }
        }
    },
    FillDefaultSettings: function (userName) {
        var data = "userName=" + userName;
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "FILL_DEFAULT_SETTINGS");
    },
    DashBoardSettingSave: function () {

        var allSettting = new Array();
        var activeWidgets = null;
        var inActiceWidgets = null;
        //var activeKPI = null;
        //var inActiceKPI = null;

        //activeWidgets = $('#' + DashBoardSetting.params.PanelID + ' #setdashboard #WidgetContainer #widtable .btnWidgetSwitch .ios-switch.on');
        //inActiceWidgets = $('#' + DashBoardSetting.params.PanelID + ' #setdashboard #WidgetContainer #widtable .btnWidgetSwitch .ios-switch').not('.on');
        activeWidgets = $('#' + DashBoardSetting.params.PanelID + ' #setdashboard .ios-switch.on');
        inActiceWidgets = $('#' + DashBoardSetting.params.PanelID + ' #setdashboard .ios-switch').not('.on');

        //activeKPI = $('#' + DashBoardSetting.params.PanelID + ' #setdashboard #KPIContainer #kpitable .btnKPISwitch .ios-switch.on');
        //inActiceKPI = $('#' + DashBoardSetting.params.PanelID + ' #setdashboard #KPIContainer #kpitable .btnKPISwitch .ios-switch').not('.on');

        activeWidgets.each(function (i, item) {
            //DashBoardSetting.ActiveInactivecontrol($(item).parent().attr('widgetid'), 1);
            var objSettings = new Object();
            objSettings.DBSId = $(item).parent().attr('widgetid');
            objSettings.isActive = 1;
            allSettting.push(objSettings);
        });

        inActiceWidgets.each(function (i, item) {
            //DashBoardSetting.ActiveInactivecontrol($(item).parent().attr('widgetid'), 0);
            var objSettings = new Object();
            objSettings.DBSId = $(item).parent().attr('widgetid');
            objSettings.isActive = 0;
            allSettting.push(objSettings);
        });
        var strJSON = JSON.stringify(allSettting);

        DashBoardSetting.UpdateAllDashBoardSetting(strJSON).done(function (response) {
            if (response != false) {
                if (!utility.CheckPageLoaded("mdvisionadmin.aspx")) {
                    $('.slide-ul').unslick();
                    $('#pnlDashboard #kpipanel').empty();
                    $('#pnlDashboard #widgetpanel').empty();
                    $('#pnlDashboard #ActiveWidget').val('');
                    //DashBoard.bIsFirstLoad = true;
                    //DashBoard.Load();
                    DashBoard.LoadDashBoardSetting(0);
                }
                utility.DisplayMessages("Successfully Saved.", 1);
                DashBoardSetting.UnLoad();
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

        //activeKPI.each(function (i, item) {
        //    DashBoardSetting.ActiveInactivecontrol($(item).parent().attr('kpiid'), 1);
        //});

        //inActiceKPI.each(function (i, item) {
        //    DashBoardSetting.ActiveInactivecontrol($(item).parent().attr('kpiid'), 0);
        //});


    },


    QuickLinksSettings: function (myJSON) {
        var data = $.parseJSON(myJSON);

        //------------------------
        if (data.switchSearchPatient == true)
            SetGlobalAppData('IsSearchPatient', 'True');
        else
            SetGlobalAppData('IsSearchPatient', 'False');

        SetGlobalAppData('WeekWorkDaysIds', data.WeekWorkDaysIds);
        //------------------------
        if (data.switchAppointments == true)
            SetGlobalAppData('IsAppointment', 'True');
        else
            SetGlobalAppData('IsAppointment', 'False');
        //------------------------
        if (data.switchQuickPat == true)
            SetGlobalAppData('IsQuickAddPatient', 'True');
        else
            SetGlobalAppData('IsQuickAddPatient', 'False');
        //-------------------------
        if (data.switchNotes == true)
            SetGlobalAppData('IsNote', 'True');
        else
            SetGlobalAppData('IsNote', 'False');
        //--------------------------
        if (data.switchFax == true)
            SetGlobalAppData('IsFax', 'True');
        else
            SetGlobalAppData('IsFax', 'False');
        //--------------------------
        if (data.switchTasks == true)
            SetGlobalAppData('IsTask', 'True');
        else
            SetGlobalAppData('IsTask', 'False');
        //--------------------------
        if (data.switchMessages == true)
            SetGlobalAppData('IsMessage', 'True');
        else
            SetGlobalAppData('IsMessage', 'False');
        //--------------------------
        //--------------------------
        if (data.switchPrescriptionsRefill == true)
            SetGlobalAppData('IsPendingPrescriptions', 'True');
        else
            SetGlobalAppData('IsPendingPrescriptions', 'False');
        //--------------------------
        //--------------------------
        if (data.switchPendingPrescriptions == true)
            SetGlobalAppData('IsPrescriptionsRefill', 'True');
        else
            SetGlobalAppData('IsPrescriptionsRefill', 'False');

        if (data.switchImmunizationAlert == true)
            SetGlobalAppData('IsImmunizationAlert', 'True');
        else
            SetGlobalAppData('IsImmunizationAlert', 'False');

        if (data.switchDocumentsAlert == true)
            SetGlobalAppData('IsDocumentsAlert', 'True');
        else
            SetGlobalAppData('IsDocumentsAlert', 'False');

        if (data.switchAssignedResults == true)
            SetGlobalAppData('IsAssignedResults', 'True');
        else
            SetGlobalAppData('IsAssignedResults', 'False');
        //--------------------------

        //----------- Start Patient Banner Settings
        //if (data.ddlPhoneNumber1 == "1") {
        //    SetGlobalAppData('PBPhone1', 'True');
        //} else {
        //    SetGlobalAppData('PBPhone1', 'False');
        //}
        //if (data.ddlPhoneNumber2 == "1") {
        //    SetGlobalAppData('PBPhone2', 'True');
        //} else {
        //    SetGlobalAppData('PBPhone2', 'False');
        //}
        if (data.ddlPreferredPhone == "1") {
            SetGlobalAppData('PBPreferredPhone', '1');
        } else if (data.ddlPreferredPhone == "2") {
            SetGlobalAppData('PBPreferredPhone', '2');
        } else if (data.ddlPreferredPhone == "3") {
            SetGlobalAppData('PBPreferredPhone', '3');
        } else {
            SetGlobalAppData('PBPreferredPhone', '');
        }


        if (data.ddlPCP == "1") {
            SetGlobalAppData('PBPCP', 'True');
        } else {
            SetGlobalAppData('PBPCP', 'False');
        }
        if (data.ddlReferringProvider == "1") {
            SetGlobalAppData('PBRefProvider', 'True');
        } else {
            SetGlobalAppData('PBRefProvider', 'False');
        }
        if (data.ddlPlanBalance == "1") {
            SetGlobalAppData('PBPlanBalance', 'True');
        } else {
            SetGlobalAppData('PBPlanBalance', 'False');
        }
        if (data.ddlPatientBalance == "1") {
            SetGlobalAppData('PBPatientBalance', 'True');
        } else {
            SetGlobalAppData('PBPatientBalance', 'False');
        }
        //For banner advance balance and primary insurance
        if (data.ddlAdvanceBalance == "1") {
            SetGlobalAppData('PBPatientAdvanceBalance', 'True');
        } else {
            SetGlobalAppData('PBPatientAdvanceBalance', 'False');
        }
        if (data.ddlCCMTimer == "1") {
            SetGlobalAppData('PBPatientCCMTimer', 'True');
        } else {
            SetGlobalAppData('PBPatientCCMTimer', 'False');
        }
        if (data.ddlPrimaryInsurance == "1") {
            SetGlobalAppData('PBPrimaryInsurance', 'True');
        } else {
            SetGlobalAppData('PBPrimaryInsurance', 'False');
        }

        if (data.ddlCollectionBalance == "1") {
            SetGlobalAppData('IsPBCollection', 'True');
        } else {
            SetGlobalAppData('IsPBCollection', 'False');
        }
        if (data.chkFacilityShortName == true) {
            SetGlobalAppData('IsShowFacilityShortName', 'True');
        } else {
            SetGlobalAppData('IsShowFacilityShortName', 'False');
        }
        if (data.chkSendBillingInquiry) {
            SetGlobalAppData('sendBillingInquiry', 'True');
            if ($('#hfPatientBalance').val() > 0) {
                $('#sendBillingInquiry').removeClass('hidden');
            }
        } else {
            SetGlobalAppData('sendBillingInquiry', 'False');
            $('#sendBillingInquiry').addClass('hidden');
        }

        //
        //----------- End Patient Banner Settings

        TopButtonsHideShow();
        Patient_Demographic.UpdateBalancesInBanner();
    },
    ChangeThemeColor: function (isStartup) {
        var themeColor = null;
        if (!isStartup)
            themeColor = $('#' + DashBoardSetting.params.PanelID).find('#ddlDefaultTheme option:selected').text().toLowerCase();
        else if (isStartup && globalAppdata.DefaultThemeName != null) {
            themeColor = globalAppdata.DefaultThemeName.toLowerCase();
        } else if (globalAppdata.AppUserName == "" || globalAppdata.AppUserName == null) {
            var basePath = window.location.pathname.replace('MDVisionDefault.aspx', '');
            window.location.href = basePath + 'MDVisionLogin.aspx';
        }
        var previousSelectedTheme = "Blue";

        var currentSelectedTheme = "Blue";
        if (themeColor != '- select -' && themeColor.length != 0) {
            currentSelectedTheme = themeColor.charAt(0).toUpperCase() + themeColor.slice(1).toLowerCase();;
        }
        var basePath = window.location.pathname.replace('MDVisionDefault.aspx', '');
        basePath = basePath.replace('MDVisionAdmin.aspx', '');
        basePath = basePath.replace('MDVisionAuditableEvents.aspx', '');

        //if ($("link[href = '" + basePath + "Content/Black/theme-custom.css" + "']").length > 0) {
        //    previousSelectedTheme = "Black";
        //}
        //else if ($("link[href = '" + basePath + "Content/Gray/theme-custom.css" + "']").length > 0) {
        //    previousSelectedTheme = "Gray";
        //}

        if (isStartup || DashBoardSetting.selected_theme.length <= 0) {
            previousSelectedTheme = "Blue";
        } else {
            previousSelectedTheme = DashBoardSetting.selected_theme;
        }
        if ($("link[href = '" + basePath + "Content/" + previousSelectedTheme + "/theme-custom.css" + "']").length > 0) {
            $("link[href = '" + basePath + "Content/" + previousSelectedTheme + "/theme-custom.css']").attr('href', "" + basePath + "Content/" + currentSelectedTheme + "/theme-custom.css");
            $("link[href = '" + basePath + "Content/" + previousSelectedTheme + "/default.css']").attr('href', "" + basePath + "Content/" + currentSelectedTheme + "/default.css");
            $("link[href = '" + basePath + "Content/" + previousSelectedTheme + "/theme.css']").attr('href', "" + basePath + "Content/" + currentSelectedTheme + "/theme.css");
        } else if ($("link[href = '" + basePath + "Content/" + previousSelectedTheme + "/" + previousSelectedTheme + ".min.css" + "']").length > 0) {
            $("link[href = '" + basePath + "Content/" + previousSelectedTheme + "/" + previousSelectedTheme + ".min.css']").attr('href', "" + basePath + "Content/" + currentSelectedTheme + "/" + currentSelectedTheme + ".min.css");
        } else if ($('link[href*="/bundles/' + previousSelectedTheme + '"]').length > 0) {
            $('link[href*="/bundles/' + previousSelectedTheme + '"]').attr('href', $('link[href*="/bundles/' + previousSelectedTheme + '"]').attr('href').replace(previousSelectedTheme, currentSelectedTheme));
        }
    },
    SelectAllChange: function (obj) {
        if ($(obj).prop("checked")) {
            $.each($("#" + DashBoardSetting.params.PanelID).find(".autoPopulate"), function (i, item) {
                $(item).prop("checked", true);
            });
        }
        else {
            $.each($("#" + DashBoardSetting.params.PanelID).find(".autoPopulate"), function (i, item) {
                $(item).prop("checked", false);
            });
        }
    },
    checkOrUncheckSelectAll: function () {
        var AllChecked = true;
        $.each($("#" + DashBoardSetting.params.PanelID).find(".autoPopulate"), function (i, item) {
            if (!$(item).prop("checked")) {
                AllChecked = false;
            }
        });
        if (AllChecked) {
            $("#" + DashBoardSetting.params.PanelID + " #chkSelectAll").prop("checked", true);
        }
        else {
            $("#" + DashBoardSetting.params.PanelID + " #chkSelectAll").prop("checked", false);
        }
    },

    SelectAllChangeMU3: function (obj) {
        if ($(obj).prop("checked")) {
            $.each($("#" + DashBoardSetting.params.PanelID).find(".chkMU3"), function (i, item) {
                $(item).prop("checked", true);
            });
        }
        else {
            $.each($("#" + DashBoardSetting.params.PanelID).find(".chkMU3"), function (i, item) {
                $(item).prop("checked", false);
            });
        }
    },

    SelectAllChangMIPS: function (obj, alertType) {
        var _chkbxclass = "";
        if (alertType == "PI")
            _chkbxclass = ".chkMIPSPI";
        else if (alertType == "CQM")
            _chkbxclass = ".chkMIPSCQM";
        else if (alertType == "IA")
            _chkbxclass = ".chkMIPSIA";

        if ($(obj).prop("checked")) {
            $.each($("#" + DashBoardSetting.params.PanelID).find(_chkbxclass), function (i, item) {
                $(item).prop("checked", true);
            });
        }
        else {
            $.each($("#" + DashBoardSetting.params.PanelID).find(_chkbxclass), function (i, item) {
                $(item).prop("checked", false);
            });
        }
    },
    checkOrUncheckSelectAllMIPS: function () {
        var arr = [{ "Measure": "PI", "chkbxclass": ".chkMIPSPI", "SelectAllChkbxId": "chkSelectAllMIPSPI" }, { "Measure": "CQM", "chkbxclass": ".chkMIPSCQM", "SelectAllChkbxId": "chkSelectAllMIPSCQM" }, { "Measure": "IA", "chkbxclass": ".chkMIPSIA", "SelectAllChkbxId": "chkSelectAllMIPSIA" }];
        $.each(arr, function (indx, itm) {
            DashBoardSetting.checkOrUncheckSelectAllMIPS_(itm.chkbxclass, itm.SelectAllChkbxId);
        });
    },
    checkOrUncheckSelectAllMIPS_: function (chkbxclass, SelectAllChkbxId) {
        var AllChecked = true;
        $.each($("#" + DashBoardSetting.params.PanelID).find(chkbxclass), function (i, item) {
            if (!$(item).prop("checked"))
                AllChecked = false;
        });
        if (AllChecked)
            $("#" + DashBoardSetting.params.PanelID + " #" + SelectAllChkbxId).prop("checked", true);
        else
            $("#" + DashBoardSetting.params.PanelID + " #" + SelectAllChkbxId).prop("checked", false);
    },

    checkOrUncheckSelectAllMU3: function () {
        var AllChecked = true;
        $.each($("#" + DashBoardSetting.params.PanelID).find(".chkMU3"), function (i, item) {
            if (!$(item).prop("checked")) {
                AllChecked = false;
            }
        });
        if (AllChecked) {
            $("#" + DashBoardSetting.params.PanelID + " #chkSelectAllMU3").prop("checked", true);
        }
        else {
            $("#" + DashBoardSetting.params.PanelID + " #chkSelectAllMU3").prop("checked", false);
        }
    }
}