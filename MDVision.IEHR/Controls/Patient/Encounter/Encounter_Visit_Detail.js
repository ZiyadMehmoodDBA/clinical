EncounterVisit_Detail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        EncounterVisit_Detail.params = params;
        if (EncounterVisit_Detail.bIsFirstLoad) {
            EncounterVisit_Detail.bIsFirstLoad = false;
            var self = $('#' + EncounterVisit_Detail.params.PanelID);
            //self.loadDropDowns(true).done(function () {

            //});
            //EncounterVisit_Detail.LoadAllAutocomplete();
        }
    },

    CloseVisitTab: function (obj) {
        var TabID = $(obj).parent().attr("id");
        //$(obj).parent().remove();
        //SelectTab('patTabEncounter', 'false');
        //$("#patTabEncounter").click();
        EncounterVisit_Detail.CloseChildVisit(TabID);
        RemoveEncounterTab(TabID);
        SelectCurrentTab("patTabEncounter", true);
        $('#patTabEncounter').trigger("click");
        EncounterChargeCapture.EditableGrid = null;

    },

    CloseChildVisit: function (TabID) {
        var Tab = GetTab(TabID);
        if (typeof Tab == "undefined")
            return;
        var TabsChild = $.grep(TabsArray, function (item) {
            return item.MasterTabID == Tab.TabID && item.ParentTabID == Tab.TabID;
        });
        $.each(TabsChild, function (index, element) {
            RemoveAdminTab(TabsChild[index].TabID, "Encounter");
            EncounterVisit_Detail.RemoveEncounterTabFromStore(TabsChild[index].TabID);
        });

    },

    RemoveEncounterTabFromStore: function (TabID) {
        if (LoadedEncounterTabs != null) {
            var UpdatedArray = [];
            UpdatedArray = jQuery.grep(LoadedEncounterTabs, function (value) {
                return value.TabID != TabID;
            });
            LoadedEncounterTabs = UpdatedArray;
        }
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', true).done(function (result) {
            $('#' + EncounterVisit_Detail.params["PanelID"] + " input#txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + EncounterVisit_Detail.params["PanelID"] + " #hfProvider").val(ui.item.id); // add the selected id
                        if ($("#" + EncounterVisit_Detail.params["PanelID"] + " #lnkProviderEdit").css("display") == "none") {
                            $("#" + EncounterVisit_Detail.params["PanelID"] + " #lnkProviderEdit").css("display", "inline");
                            $("#" + EncounterVisit_Detail.params["PanelID"] + " #lblProvider").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetFacility', true).done(function (result) {
            $('#' + EncounterVisit_Detail.params["PanelID"] + " input#txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + EncounterVisit_Detail.params["PanelID"] + " #hfFacility").val(ui.item.id); // add the selected id
                        if ($("#" + EncounterVisit_Detail.params["PanelID"] + " #lnkFacilityEdit").css("display") == "none") {
                            $("#" + EncounterVisit_Detail.params["PanelID"] + " #lnkFacilityEdit").css("display", "inline");
                            $("#" + EncounterVisit_Detail.params["PanelID"] + " #lblFacility").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
    },

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabEncounter";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + EncounterVisit_Detail.params["PanelID"] + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'patTabEncounter';
        LoadActionPan('providerDetail', params);
    },

    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabEncounter";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + EncounterVisit_Detail.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'patTabEncounter';
        LoadActionPan('facilityDetail', params);
    },



    UnLoad: function () {
        if (EncounterVisit_Detail.params != null && EncounterVisit_Detail.params.ParentCtrl != null) {
            UnloadActionPan(EncounterVisit_Detail.params.ParentCtrl, 'EncounterVisit_Detail');
        }
        else
            UnloadActionPan(null, 'EncounterVisit_Detail');
    },

}

