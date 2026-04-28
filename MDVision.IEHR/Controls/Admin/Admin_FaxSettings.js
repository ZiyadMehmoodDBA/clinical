Admin_FaxSettings = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_FaxSettings.params = params;
        EMRUtility.SwicthWidgetInializatoin();

     
        var switcher = $('#switchActive');
            
        $(switcher).attr('IsActive', '0');
        $('.ios-switch').attr('class', 'ios-switch on');
            Admin_FaxSettings.changeSwitch(switcher[0]);
            
        
       
        // load grid first time 


        //var providerDataStr = $("#frmId").getMyJSON();

        //var providerData = JSON.parse(providerDataStr);
        //providerData["ProviderId"] = "";
        //Admin_FaxSettings.loadProvidersFaxSettings(providerData).done(function (response) {
        //    Admin_FaxSettings.ProviderFaxGridLoad(response);
        //});

        //var FacilityDataStr = $("#frmId").getMyJSON();
        //var FacilityData = JSON.parse(FacilityDataStr);
        //FacilityData["FacilityId"] = "";
        //Admin_FaxSettings.loadFacilitysFaxSettings(FacilityData).done(function (response) {
        //    Admin_FaxSettings.FacilityFaxGridLoad(response);
        //});





        if (Admin_FaxSettings.params["FromAdmin"] == "0" && Admin_FaxSettings.params["PanelID"] == 'pnlAdminProvider')
            Admin_FaxSettings.params["FromAdmin"] = "1";

        if (Admin_FaxSettings.bIsFirstLoad) {
            Admin_FaxSettings.bIsFirstLoad = false;
        }
        //Admin_FaxSettings.GetAllProviders().done(function (response) {

        //    Admin_FaxSettings.ProviderGridLoad(response);

        //});

    },

    changeSwitch: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');

        if (IsActive == '1') {
            // Facility work
            Facilityhtml = '<div class="col-sm-3"><label class="control-label">Facility</label><select id="chkFacilityFax" name="Active" class="form-control" ddlist="GetFacility"></select> </div>  <div class="col-sm-2 pad-a-labelsize-btn pull-left"> <button class="btn btn-primary btn-sm rightbtn" id="btnFacilitySearch" type="submit" onclick="Admin_FaxSettings.SearchFacilityFaxSettings();">Search</button></div>';
            $('#Selector').html(Facilityhtml);
            var ddr = $("#pnl_FaxSetting");
            ddr.loadDropDowns();
            $('#pnlProvider_Result').hide();
            $('#pnlFacility_Result').show();

            Admin_FaxSettings.loadFacilities();

            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            // Provider work
            Providerhtml = '<div class="col-sm-3"><label class="control-label">Provider</label><select id="chkProviderFax" name="Active" class="form-control" ddlist="GetProvider"></select> </div>  <div class="col-sm-2 pad-a-labelsize-btn pull-left"> <button class="btn btn-primary btn-sm rightbtn" id="btnProviderSearch" type="submit" onclick="Admin_FaxSettings.SearchProviderFaxSettings();">Search</button></div>';
            $('#Selector').html(Providerhtml);
            var ddr = $("#pnl_FaxSetting");
            ddr.loadDropDowns();
            $('#pnlFacility_Result').hide();
            $('#pnlProvider_Result').show();
            Admin_FaxSettings.loadProviders();
            $(objThis).attr('IsActive', '1');
        }
    },


    loadProviders: function () {
        var providerDataStr = $("#frmId").getMyJSON();
        var providerData = JSON.parse(providerDataStr);
        providerData["ProviderId"] = "";
        Admin_FaxSettings.loadProvidersFaxSettings(providerData).done(function (response) {
              Admin_FaxSettings.ProviderFaxGridLoad(response);
        });

    },

    loadProvidersFaxSettings: function (resp) {
        var strdata = JSON.stringify(resp);
        strdata = "ProviderData=" + strdata;
        return MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "LOAD_PROVIDER");
    },

    ProviderFaxGridLoad: function (response) {
        var rows = "";
        if (response.status == "False" || response.status == false) {
            rows = '<tr><td colspan="4"><center>No Provider Found</center></td></tr>';
            if ($.fn.DataTable.isDataTable('#dgvProviderFaxSettings')) {
                 $("#dgvProviderFaxSettings").dataTable().fnDestroy();
            }
            $("#dgvProviderFaxSettings tbody").find("tr").remove();
            $('#dgvProviderFaxSettings tbody').append(rows);
            //if (!$.fn.DataTable.isDataTable('#dgvProviderFaxSettings')) {
            //    $("#dgvProviderFaxSettings").DataTable({
            //        "bInfo": true, "bPaginate": true, "bLengthChange": false, "bSort": false, "autoWidth": false,  "iDisplayLength": 15,
            //    }); // to remove records per page dropdown
            //}
        }
        else {
            var faxSettingJSON = JSON.parse(response.ProviderFaxFill_JSON);
            if ($.fn.DataTable.isDataTable('#dgvProviderFaxSettings')) {
                $("#dgvProviderFaxSettings").dataTable().fnDestroy();
            }
            $("#dgvProviderFaxSettings tbody").find("tr").remove();
            $.each(faxSettingJSON, function (i, item) {
                rows += '<tr onclick="Admin_FaxSettings.editFaxSettings(' + item.ProviderId + ',event);"><td style="display:none;">' + item.ProviderId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FaxSettings.ProviderFaxSettingsDelete(' + item.ProviderId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FaxSettings.editFaxSettings(' + item.ProviderId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp</td><td>' + item.DisplayName + '</td><td>' + item.FaxNo + '</td><td>' + item.ModifiedOn + '</td></tr>';
            });
            $('#dgvProviderFaxSettings tbody').append(rows);
            if (!$.fn.DataTable.isDataTable('#dgvProviderFaxSettings')) {
                $("#dgvProviderFaxSettings").DataTable({
                    "bInfo": true, "bPaginate": true, "bLengthChange": false, "bSort": false, "autoWidth": false,  "iDisplayLength": 15,
                }); // to remove records per page dropdown
            }
        }
    },

    loadFacilities: function () {
        var FacilityDataStr = $("#frmId").getMyJSON();
        var FacilityData = JSON.parse(FacilityDataStr);
        FacilityData["FacilityId"] = "";
        Admin_FaxSettings.loadFacilitysFaxSettings(FacilityData).done(function (response) {
            Admin_FaxSettings.FacilityFaxGridLoad(response);
        });
    },

    loadFacilitysFaxSettings: function (resp) {
        var strdata = JSON.stringify(resp);
        strdata = "FacilityData=" + strdata;
        return MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "LOAD_FACILITY");
    },

    FacilityFaxGridLoad: function (response) {
        var rows = "";

        if (response.status == "False" || response.status == false ) {
            rows = '<tr><td colspan="4"><center>No Facility Found</center></td></tr>';
            if ($.fn.DataTable.isDataTable('#dgvFacilityFaxSettings')) {
                $("#dgvFacilityFaxSettings").dataTable().fnDestroy();
            }
            $("#dgvFacilityFaxSettings tbody").find("tr").remove();

            $('#dgvFacilityFaxSettings tbody').append(rows);
            //if (!$.fn.DataTable.isDataTable('#dgvFacilityFaxSettings')) {
            //    $("#dgvFacilityFaxSettings").DataTable({
            //        "bInfo": true, "bPaginate": true, "bLengthChange": false, "bSort": false, "autoWidth": false,  "iDisplayLength": 15,
            //    }); // to remove records per page dropdown
            //}
        }
        else {
            var faxSettingJSON = JSON.parse(response.FacilityFaxFill_JSON);
            if ($.fn.DataTable.isDataTable('#dgvFacilityFaxSettings')) {
                $("#dgvFacilityFaxSettings").dataTable().fnDestroy();
            }
            $("#dgvFacilityFaxSettings tbody").find("tr").remove();
            $.each(faxSettingJSON, function (i, item) {
                rows += '<tr onclick="Admin_FaxSettings.editFacilityFaxSettings(' + item.FacilityId + ',event);"><td style="display:none;">' + item.FacilityId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FaxSettings.FacilityFaxSettingsDelete(' + item.FacilityId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FaxSettings.editFacilityFaxSettings(' + item.FacilityId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp</td><td>' + item.DisplayName + '</td><td>' + item.FaxNo + '</td><td>' + item.ModifiedOn + '</td></tr>';
            });

            $('#dgvFacilityFaxSettings tbody').append(rows);
            if (!$.fn.DataTable.isDataTable('#dgvFacilityFaxSettings')) {
                $("#dgvFacilityFaxSettings").DataTable({
                    "bInfo": true, "bPaginate": true, "bLengthChange": false, "bSort": false, "autoWidth": false,  "iDisplayLength": 15,
                }); // to remove records per page dropdown
            }
        }
    },

    SearchProviderFaxSettings: function () {
        var providerDataStr = $("#frmProviderIdFax").getMyJSON();

        var providerData = JSON.parse(providerDataStr);
        providerData["ProviderId"] = providerData.chkProviderFax;
        Admin_FaxSettings.loadProvidersFaxSettings(providerData).done(function (response) {
            Admin_FaxSettings.ProviderFaxGridLoad(response);
        });

    },

    SearchFacilityFaxSettings: function () {
        var FacilityDataStr = $("#frmProviderIdFax").getMyJSON();
        var FacilityData = JSON.parse(FacilityDataStr);
        FacilityData["FacilityId"] = FacilityData.chkFacilityFax;
        Admin_FaxSettings.loadFacilitysFaxSettings(FacilityData).done(function (response) {
            Admin_FaxSettings.FacilityFaxGridLoad(response);
        });
    },

    loadProviderFaxSetting: function () {

        var providerDataStr = $("#frmProviderId").getMyJSON();

        var providerData = JSON.parse(providerDataStr);
        providerData["ProviderId"] = "";
        Admin_FaxSettings.loadProvidersFaxSettings(providerData).done(function (response) {

            Admin_FaxSettings.ProviderFaxGridLoad(response);
        });

    },

    ProviderFaxSettingsDelete: function (id,event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm("1", function () {
            var data = "ProviderId=" + id;
            MDVisionService.defaultService(data, "ADMIN_PROVIDER_FAXSETTINGS", "DELETE_PROVIDER").done(function () {
                Admin_FaxSettings.loadProviderFaxSetting();
            }
            );
        }, function () { });
    },

    FacilityFaxSettingsDelete: function (id, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm("1", function () {
        var data = "FacilityId=" + id;
        MDVisionService.defaultService(data, "ADMIN_PROVIDER_FAXSETTINGS", "DELETE_FACILITY").done(function () {

            Admin_FaxSettings.loadFacilityFaxSetting();
        });
        
        }, function () { });
    },

    loadFacilityFaxSetting: function () {

        var FacilityDataStr = $("#frmFacilityId").getMyJSON();

        var FacilityData = JSON.parse(FacilityDataStr);
        FacilityData["FacilityId"] = "";
        Admin_FaxSettings.loadFacilitysFaxSettings(FacilityData).done(function (response) {

            Admin_FaxSettings.FacilityFaxGridLoad(response);
        });

    },

    loadFaxSettingsDetail: function () {
        var params = [];
        params["mode"] = "Add";

        var IsActive = $('#switchActive').attr('IsActive');

        if (IsActive == 1) {
            params["type"] = "Provider";
        }
        else {
            params["type"] = "Facility";
        }

        params["FromAdmin"] = Admin_Facility.params["FromAdmin"];
        if (Admin_Facility.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Admin_FaxSettings';
        }
        LoadActionPan('Admin_FaxSettingsDetail', params);
    },

    editFaxSettings: function (Id, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["mode"] = "Edit";
        params["Id"] = Id;
        
        var IsActive = $('#switchActive').attr('IsActive');

        params["type"] = "Provider";
       

        params["FromAdmin"] = Admin_Facility.params["FromAdmin"];
        if (Admin_Facility.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Admin_FaxSettings';
        }
        LoadActionPan('Admin_FaxSettingsDetail', params);

    },

    editFacilityFaxSettings: function (Id, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["mode"] = "Edit";
        params["Id"] = Id;
        params["Name"] = name;
        var IsActive = $('#switchActive').attr('IsActive');

        params["type"] = "Facility";

        
        params["FromAdmin"] = Admin_Facility.params["FromAdmin"];
        if (Admin_Facility.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Admin_FaxSettings';
        }
        LoadActionPan('Admin_FaxSettingsDetail', params);
    },

    UnLoadTab: function () {
        if (Admin_FaxSettings.params["FromAdmin"] == "0") {

            if (Admin_FaxSettings.params != null && Admin_FaxSettings.params.ParentCtrl != null && Admin_FaxSettings.params.PanelID != 'pnlAdminProvider') {
                UnloadActionPan(Admin_FaxSettings.params.ParentCtrl, 'Admin_FaxSettings', null, Admin_FaxSettings.params.PanelID);
            }
            else if (Admin_FaxSettings.params != null && Admin_FaxSettings.params.ParentCtrl != null) {
                UnloadActionPan(Admin_FaxSettings.params.ParentCtrl, 'Admin_FaxSettings');
            }
            else
                UnloadActionPan(null, 'Admin_FaxSettings');

        }
        else {
            RemoveAdminTab();
        }
    }

}
