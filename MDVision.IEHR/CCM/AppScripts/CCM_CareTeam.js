CCMCareTeam = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCMCareTeam.params = params;

        if (CCMCareTeam.params.PanelID != 'pnlCCMCareTeam') {
            CCMCareTeam.params.PanelID = CCMCareTeam.params.PanelID + ' #pnlCCMCareTeam';
        } else {
            CCMCareTeam.params.PanelID = 'pnlCCMCareTeam';
        }

        if (CCMCareTeam.params.ProviderId != null || CCMCareTeam.params.ProviderId != "") {

            var self = $('#' + CCMCareTeam.params.PanelID);
            self.loadDropDowns(true).done(function () {
                var objData = new Object();
                objData["EnrollmentInfoId"] = '-1';// CCMCareTeam.params.EnrollmentInfoId;
                objData["PatientId"] = CCMCareTeam.params.PatientId;
                objData["ProviderId"] = CCMCareTeam.params.ProviderId;
                objData["CareTeamId"] = "0";

                CCMCareTeam.ProviderCareTeamLoad(objData).done(function (response) {
                    if (response.status != false) {
                        if (response.PHCount > 0) {
                            CCMCareTeam.CCMICDGroupsGridLoad(response);
                        }
                        else {

                            CCMCareTeam.ProviderCareTeamDelete(objData).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages("No Care Team member found", 3);
                                    // Start 06-12-2017 Edit By Humaira Yousaf Bug# MU3-433
                                    if (CCMCareTeam.params.ParentCtrl == "Clinical_CarePlan") {
                                        $('#' + Clinical_CarePlan.params.PanelID + " #dgvCareTeam tbody").empty();
                                        $('#' + Clinical_CarePlan.params.PanelID + " #dgvCareTeam thead").empty();
                                    }
                                    else {
                                        $("#tblPatientHubCareTeam tr").remove();
                                    }
                                    // End 06-12-2017 Edit By Humaira Yousaf Bug# MU3-433
                                    CCMCareTeam.UnLoad();
                                }
                            });
                        }
                    }
                    else {
                        utility.DisplayMessages("Issue encountered while loading care team members.", 3);
                        CCMCareTeam.UnLoad();
                    }
                });
            });
        }
    },


    EnrolledCareTeamInsert: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "InsertCCMPatientHUBEnrolledCareTeam");
    },

    ProviderCareTeamLoad: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "loadPatientHubCareTeam");
    },

    ProviderCareTeamDelete: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "DeleteCareTeamProviderTemplate");
    },

    CCMICDGroupsGridLoad: function (response) {
        if (response.status == false) {
            response = [];
        }
        else {
            response = JSON.parse(response.PHList_JSON);
        }
        var data = new kendo.data.DataSource({
            data: response,
            pageSize: 15
        });

        $("#dgvCCMCareTeam").kendoGrid({
            dataSource: data,
            resizable: true,
            scrollable: false,
            noRecords: true,
            messages: {
                noRecords: "There is no data on current page"
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 50, 100],
                buttonCount: 5
            },
            columns: [
            { title: "Action", width: "20px", template: '#=CCMCareTeam.ActionCCMCareTeam(data)#' },
            { title: "Name", field: "Name", width: "100px" },
            { title: "Provider", field: "ProviderName", width: "100px" },
            { title: "Care Manager", field: "CareManagerName", width: "100px" },
            { title: "Care Coordinator", field: "CareCoordinatorName", width: "100px" },
            { title: "Care Giver", field: "CareGiverName", width: "100px" },
            { title: "Last Updated", field: "LastUpdated", width: "100px" }
            ],
        });
    },

    ActionCCMCareTeam: function (data) {

        var selectCCMCareTeam = '';
        if (CCMCareTeam.params.ParentCtrl == "Clinical_CarePlan") {
            selectCCMCareTeam = "Clinical_CarePlan.CareTeamMemberSelect( '" + data.CareTeamId + "' , '" + data.Name + "' , '" + data.ProviderName + "', '" + data.CareManagerName + "', '" + data.CareCoordinatorName + "', '" + data.CareGiverName + "', '" + data.Specialty + "', '" + data.ProviderPhone + "', '" + data.CareManagerPhone + "', '" + data.CareCoordinatorPhone + "', '" + data.CareGiverPhone + "')";
        }
        else {
            selectCCMCareTeam = "CCMCareTeam.CCMCareTeamMemberSelect( '" + data.CareTeamId + "' , '" + data.Name + "' , '" + data.ProviderName + "', '" + data.CareManagerName + "', '" + data.CareCoordinatorName + "', '" + data.CareGiverName + "', '" + data.LastUpdated + "', '" + data.Specialty + "', '" + data.ProviderPhone + "', '" + data.CareManagerPhone + "', '" + data.CareCoordinatorPhone + "', '" + data.CareGiverPhone + "')";

            // selectCCMCareTeam = "CCMCareTeam.CCMCareTeamMemberSelect( '" + data.CareTeamId + "' , '" + data.Name + "' , '" + data.ProviderName + "', '" + data.CareManagerName + "', '" + data.CareCoordinatorName + "', '" + data.CareGiverName + "', '" + data.LastUpdated + "', '" + data.Specialty + "', '" + data.ProviderPhone + "', '" + data.CareManagerPhone + "', '" + data.CareCoordinatorPhone + "', '" + data.CareGiverPhone + "')";
        }
        return '<a class="btn  btn-xs" href="javascript:;" onclick="' + selectCCMCareTeam + '" title="Select CareTeam"><i class="fa fa-check black"></i></a>';
    },

    CCMCareTeamMemberSelect: function (CareTeamId, Name, ProviderName, CareManagerName, CareCoordinatorName, CareGiverName, LastUpdated, Specialty, ProviderPhone, CareManagerPhone, CareCoordinatorPhone, CareGiverPhone) {

        var objData = new Object();
        objData["EnrollmentInfoId"] = CCMCareTeam.params.EnrollmentInfoId;
        objData["PatientId"] = CCMCareTeam.params.PatientId;
        objData["ProviderId"] = CCMCareTeam.params.ProviderId;
        objData["CareTeamId"] = CareTeamId;

        CCMCareTeam.EnrolledCareTeamInsert(objData).done(function (response) {

            $("#tblPatientHubCareTeam tr").remove();
            table = $('#tblPatientHubCareTeam tbody');

            tableHead = $('#pnlCCM_Patient_Hub #tblPatientHubCareTeam thead');
            tableHead.append('<tr><th>Name</th><th>Role</th><th>Speciality</th><th>Contact Number</th></tr>');

            table.append('<tr id=' + CareTeamId + '><td>  ' + CareManagerName + '</td><td>Care Manager</td><td></td><td>' + CareManagerPhone + '</td></tr>');
            table.append('<tr id=' + CareTeamId + '><td>  ' + CareCoordinatorName + '</td><td>Care Coordinator</td><td></td><td>' + CareCoordinatorPhone + '</td></tr>');
            table.append('<tr id=' + CareTeamId + '><td>  ' + CareGiverName + '</td><td>Care Giver</td><td></td><td>' + CareGiverPhone + '</td></tr>');
            table.append('<tr id=' + CareTeamId + '><td>  ' + ProviderName + '</td><td>Provider</td><td>' + Specialty + '</td><td>' + ProviderPhone + '</td></tr>');
            CCMCareTeam.UnLoad();
        });

    },

    UnLoad: function () {
        var objDeffered = $.Deferred();
        UnloadActionPan(CCMCareTeam.params.ParentCtrl, 'CCMCareTeam');
        objDeffered.resolve();
        return objDeffered;
    }
}
