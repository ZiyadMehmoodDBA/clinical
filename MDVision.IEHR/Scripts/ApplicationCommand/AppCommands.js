var AppCommands = {
    Load: function () {

        AdminCommands();
        BatchCommands();
        ClinicalCommands();
        ScheduleCommands();
        BillingCommands();
        PatientCommands();
        CommonCommands();
        ReportsCommands();
        AduitableEventsCommands();
        DashBoardCommands();
        CCMCommands();
        iTrackCommands();
    },

    reloadTab: function () {
        var newArray = TabsArray.filter(function (i) {
            if (!isPatientTab(i)) {
                return i;
            }
        });


        TabsArray = newArray;
        PatientCommands();

    }


}

function AdminCommands() {


    var cmd = [];
    cmd.TabID = "Admin_Shifts",
    cmd.PanelID = "pnlAdminShifts";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_Shifts";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "ctrlPanAdmin";
    cmd.Path = "./Controls/Admin/Admin_Shifts.html";
    cmd.ActionPanContainer = "actionPanAdminShifts";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "mstrTabAdmin";
    cmd.PanelID = "mstrDivAdmin";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.Container = "ctrlPanAdmin";
    cmd.Path = "";
    cmd.ActionPanContainer = "actionPanAdmin";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_Insur";
    cmd.PanelID = "pnlAdminInsur";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_Insur";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Insur.html";
    cmd.ActionPanContainer = "actionPanAdminInsur";
    AddTab(cmd);



    var cmd = [];
    cmd.TabID = "practiceDetail";
    cmd.PanelID = "practiceDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "practiceDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Practice_Detail.html";
    cmd.ActionPanContainer = "actionPanPracticeDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "facilityDetail";
    cmd.PanelID = "facilityDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "facilityDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Facility_Detail.html";
    cmd.ActionPanContainer = "actionPanFacilityDetail";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "providerDetail";
    cmd.PanelID = "providerDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "providerDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Provider_Detail.html";
    cmd.ActionPanContainer = "actionPanProviderDetail";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "resourcesDetail";
    cmd.PanelID = "resourcesDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "resourcesDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Resources_Detail.html";
    cmd.ActionPanContainer = "actionPanAdminResourcesDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "specialtyDetail";
    cmd.PanelID = "specialtyDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "specialtyDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Specialty_Detail.html";
    cmd.ActionPanContainer = "actionPanAdminSpecialtyDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_DrugCodeCost_Detail";
    cmd.PanelID = "pnlAdminDrugCodeCost";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_DrugCodeCost_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_DrugCodeCost_Detail.html";
    cmd.ActionPanContainer = "actionPanDrugCodeCostDetail";
    AddTab(cmd);



    var cmd = [];
    cmd.TabID = "securityRoleDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "securityRoleDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_SecurityRole_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "userDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "userDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanUserDetail";
    cmd.Path = "./Controls/Admin/Admin_User_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_CoWorkersGroupDetail";
    cmd.PanelID = "pnlAdmin_CoWorkersGroupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_CoWorkersGroupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanAdminCoWorkersGroupDetail";
    cmd.Path = "./Controls/Admin/Admin_CoWorkersGroupDetail.html";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "privilegeGroupDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "privilegeGroupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_PrivilegeGroup_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "billingprovidersettingsDetail";
    cmd.PanelID = "billingprovidersettingsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "billingprovidersettingsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_BillingProviderSettings_Detail.html";
    cmd.ActionPanContainer = "actionPanAdminBillingProviderSettingsDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_BillingProvider_Detail";
    cmd.PanelID = "pnlAdmin_BillingProvider_Detail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_BillingProvider_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_BillingProvider_Detail.html";
    cmd.ActionPanContainer = "actionPanAdminBillingProvider_Detail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_BillingProvider";
    cmd.PanelID = "pnlAdminBillingProvider";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_BillingProvider";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_BillingProvider.html";
    cmd.ActionPanContainer = "actionPanAdminBillingProvider";

    AddTab(cmd);
    var cmd = [];
    cmd.TabID = "planCategoryDetail";
    cmd.PanelID = "planCategoryDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "planCategoryDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_PlanCategory_Detail.html";
    cmd.ActionPanContainer = "actionPanplanCategoryDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "insurDetail";
    cmd.PanelID = "insurDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "insurDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Insurance_Detail.html";
    cmd.ActionPanContainer = "actionPaninsurDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ICDDetail";
    cmd.PanelID = "ICDDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ICDDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ICD_Detail.html";
    cmd.ActionPanContainer = "actionPanICDDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "modifierDetail";
    cmd.PanelID = "modifierDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "modifierDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Modifier_Detail.html";
    cmd.ActionPanContainer = "actionPanmodifierDetail";
    AddTab(cmd);
    //Adnan Maqbool Dated Jan 12,2015 PMS-3345
    var cmd = [];
    cmd.TabID = "insurancePlanDetail";
    cmd.PanelID = "insurancePlanDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "insurancePlanDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPaninsurancePlanDetail";
    cmd.Path = "./Controls/Admin/Admin_InsurancePlan_Detail.html";
    AddTab(cmd);
    //END Adnan Maqbool Dated Jan 12,2015 PMS-3345
    var cmd = [];
    cmd.TabID = "procedurecategoryDetail";
    cmd.PanelID = "procedurecategoryDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "procedurecategoryDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ProcedureCategory_Detail.html";
    cmd.ActionPanContainer = "actionPanprocedurecategoryDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "typeOfServiceDetail";
    cmd.PanelID = "typeOfServiceDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "typeOfServiceDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_TypeOfService_Detail.html";
    cmd.ActionPanContainer = "actionPantypeOfServiceDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "cptcodeDetail";
    cmd.PanelID = "cptcodeDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "cptcodeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_CPTCode_Detail.html";
    cmd.ActionPanContainer = "actioncptcodeDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_OccupationStatusDetail";
    cmd.PanelID = "pnlOccupationStatusDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_OccupationStatusDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_OccupationStatusDetail.html";
    cmd.ActionPanContainer = "actionOccupationStatusDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "referringproviderDetail";
    cmd.PanelID = "referringproviderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "referringproviderDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ReferringProvider_Detail.html";
    cmd.ActionPanContainer = "actionPanAdminReferringProviderDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EDISubmitInsuranceDetail";
    cmd.PanelID = "EDISubmitInsuranceDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDISubmitInsuranceDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanAdminEDISubmitInsuranceDetail";
    cmd.Path = "./Controls/Admin/Admin_EDISubmitInsurance_Detail.html";
    AddTab(cmd);

    //Adnan Maqbool Dated Jan 14,2015 PMS-3345
    var cmd = [];
    cmd.TabID = "Admin_EDISubmitInsurance";
    cmd.PanelID = "pnlAdminEDISubmitInsurance";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_EDISubmitInsurance";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanAdminEDISubmitInsurance";
    cmd.Path = "./Controls/Admin/Admin_EDISubmitInsurance.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_EDIEligibilityInsurance";
    cmd.PanelID = "pnlAdminEDIEligibilityInsurance";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_EDIEligibilityInsurance";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanAdminEDIEligibilityInsurance";
    cmd.Path = "./Controls/Admin/Admin_EDIEligibilityInsurance.html";
    AddTab(cmd);
    //end Dated Jan 14,2015 PMS-3345

    var cmd = [];
    cmd.TabID = "revenuecodeDetail";
    cmd.PanelID = "revenuecodeDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "revenuecodeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_RevenueCode_Detail.html";
    cmd.ActionPanContainer = "actionPanrevenuecodeDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EDIClaimStatusInsuranceDetail";
    cmd.PanelID = "EDIClaimStatusInsuranceDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDIClaimStatusInsuranceDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_EDIClaimStatusInsurance_Detail.html";
    cmd.ActionPanContainer = "actionPanEDIClaimStatusInsuranceDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ledgeraccountDetail";
    cmd.PanelID = "ledgeraccountDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ledgeraccoutDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.ActionPanContainer = "actionPanledgeraccountDetail";
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_LedgerAccount_Detail.html";
    AddTab(cmd);
    //-------------------------------------------
    var cmd = [];
    cmd.TabID = "Admin_ERAAdjustmentCodesDetail";
    cmd.PanelID = "Admin_ERAAdjustmentCodesDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_ERAAdjustmentCodesDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.ActionPanContainer = "actionPanAdminERAAdjustmentCodesDetail";
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/ERA/Admin_ERAAdjustmentCodes_Detail.html";
    AddTab(cmd);
    //---------------
    var cmd = [];
    cmd.TabID = "EDIEligibilityInsuranceDetail";
    cmd.PanelID = "EDIEligibilityInsuranceDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDIEligibilityInsuranceDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_EDIEligibilityInsurance_Detail.html";
    cmd.ActionPanContainer = "actionPanAdminEDIEligibilityInsuranceDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "planfeelinkDetail";
    cmd.PanelID = "planfeelinkDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "planfeelinkDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_PlanFeeLink_Detail.html";
    cmd.ActionPanContainer = "actionPanplanfeelinkDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "clearingHouseDetail";
    cmd.PanelID = "clearingHouseDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "clearingHouseDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.ActionPanContainer = "actionPanclearingHouseDetail";
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ClearingHouse_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EDIServiceHandleDetail";
    cmd.PanelID = "EDIServiceHandleDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDIServiceHandleDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_EDIServiceHandle_Detail.html";
    cmd.ActionPanContainer = "actionPanEDIServiceHandleDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "PatientEligibilityServiceDetail";
    cmd.PanelID = "PatientEligibilityServiceDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "PatientEligibilityServiceDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_PatientEligibilityService_Detail.html";
    cmd.ActionPanContainer = "actionPanPatientEligibilityServiceDetail";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "feegroupDetail";
    cmd.PanelID = "feegroupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "feegroupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_FeeGroup_Detail.html";
    cmd.ActionPanContainer = "actionPanfeegroupDetail";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "placeOfServiceDetail";
    cmd.PanelID = "placeOfServiceDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "placeOfServiceDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_PlaceOfService_Detail.html";
    cmd.ActionPanContainer = "actionplaceOfServiceDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "basicfeegroupDetail";
    cmd.PanelID = "basicfeegroupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "basicfeegroupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_BasicFeeGroup_Detail.html";
    cmd.ActionPanContainer = "actionPanbasicfeegroupDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "basicFeeScheduleDetail";
    cmd.PanelID = "basicFeeScheduleDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "basicFeeScheduleDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_BasicFeeSchedule_Detail.html";
    cmd.ActionPanContainer = "actionPanbasicFeeScheduleDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "editaxidsetupDetail";
    cmd.PanelID = "editaxidsetupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "editaxidsetupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_EDITaxIDSetup_Detail.html";
    cmd.ActionPanContainer = "actionPaneditaxidsetupDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "procedureFeeScheduleDetail";
    cmd.PanelID = "procedureFeeScheduleDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "procedureFeeScheduleDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ProcedureFeeSchedule_Detail.html";
    cmd.ActionPanContainer = "actionPanprocedureFeeScheduleDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "POSFeeScheduleDetail";
    cmd.PanelID = "POSFeeScheduleDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "POSFeeScheduleDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_POSFeeSchedule_Detail.html";
    cmd.ActionPanContainer = "actionPanPOSFeeScheduleDetail";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "submitterSetupDetail";
    cmd.PanelID = "submitterSetupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "submitterSetupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_SubmitterSetup_Detail.html";
    cmd.ActionPanContainer = "actionPansubmitterSetupDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EDIReceiverDetail";
    cmd.PanelID = "EDIReceiverDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDIReceiverDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_EDIReceiver_Detail.html";
    cmd.ActionPanContainer = "actionPanEDIReceiverDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "placeofserviceDetailGrid";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "placeofserviceDetailGrid";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_PlaceOfService_DetailGrid.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "scheduleReasonDetail";
    cmd.PanelID = "scheduleReasonDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "scheduleReasonDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ScheduleReason_Detail.html";
    cmd.ActionPanContainer = "actionPanscheduleReasonDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "holidaysDetail";
    cmd.PanelID = "holidaysDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "holidaysDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Holidays_Detail.html";
    cmd.ActionPanContainer = "actionPanholidaysDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "appointmentStatusDetail";
    cmd.PanelID = "appointmentStatusDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "appointmentStatusDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_AppointmentStatus_Detail.html";
    cmd.ActionPanContainer = "actionPanappointmentStatusDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "blockHoursDetail";
    cmd.PanelID = "blockHoursDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "blockHoursDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_BlockHours_Detail.html";
    cmd.ActionPanContainer = "actionPanBlockHoursDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_BlockHours";
    cmd.PanelID = "pnlAdminBlockHours";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_BlockHours";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_BlockHours.html";
    cmd.ActionPanContainer = "actionPanAdminBlockHours";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "VisitTypeDetail";
    cmd.PanelID = "pnlAdmin_VisitTypeDurationGroupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "VisitTypeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanAdminVisitTypeDurationGroupDetail";
    cmd.Path = "./Controls/Admin/Admin_VisitTypeDurationGroup_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "providerscheduleDetail";
    cmd.PanelID = "providerscheduleDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "providerscheduleDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ProviderSchedule_Detail.html";
    cmd.ActionPanContainer = "actionPanproviderscheduleDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "resourcescheduleDetail";
    cmd.PanelID = "resourcescheduleDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "resourcescheduleDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ResourceSchedule_Detail.html";
    cmd.ActionPanContainer = "actionPanresourcescheduleDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "folderTypeDetail";
    cmd.PanelID = "folderTypeDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "folderTypeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_FolderType_Detail.html";
    cmd.ActionPanContainer = "actionPanfolderTypeDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "folderDetail";
    cmd.PanelID = "folderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "folderTypeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Folder_Detail.html";
    cmd.ActionPanContainer = "actionPanfolderDetail";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "SupperBillDetail";
    cmd.PanelID = "SupperBillDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "SupperBillDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_SupperBillDetail.html";
    cmd.ActionPanContainer = "actionPanSupperBillDetail";
    AddTab(cmd);

    /********PATIENT STATEMENT**************/
    var cmd = [];
    cmd.TabID = "Bill_PatientStatement";
    cmd.PanelID = "pnlBillPatientStatement";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PatientStatement";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/PatientStatement/Bill_PatientStatement.html";
    cmd.ActionPanContainer = "actionPanBillPatientStatement";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "Bill_PatientStatementBatch";
    cmd.PanelID = "pnlBillPatientStatementBatch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PatientStatementBatch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/PatientStatement/Bill_PatientStatementBatch.html";
    cmd.ActionPanContainer = "actionPanBillPatientStatementBatch";
    AddTab(cmd);

    // ------ Patient Statement Message -------//
    var cmd = [];
    cmd.TabID = "StatementMessageDetail";
    cmd.PanelID = "StatementMessageDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "StatementMessageDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/PatientStatement/Admin_StatementMessageDetail.html";
    cmd.ActionPanContainer = "actionPanStatementMessageDetail";
    AddTab(cmd);

    // ------ Patient Statement Group -------//
    var cmd = [];
    cmd.TabID = "StatementGroupDetail";
    cmd.PanelID = "StatementGroupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "StatementGroupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/PatientStatement/Admin_StatementGroupDetail.html";
    cmd.ActionPanContainer = "actionPanStatementGroupDetail";
    AddTab(cmd);


    /********PATIENT STATEMENT**************/
    var cmd = [];
    cmd.TabID = "Bill_PatientStatement";
    cmd.PanelID = "pnlBillPatientStatement";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PatientStatement";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/PatientStatement/Bill_PatientStatement.html";
    cmd.ActionPanContainer = "actionPanBillPatientStatement";
    AddTab(cmd);


    // ----- <Follow Up> ----- //

    var cmd = [];
    cmd.TabID = "followUpReasonDetail";
    cmd.PanelID = "followUpReasonDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "followUpReasonDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpReason_Detail.html";
    cmd.ActionPanContainer = "actionPanfollowUpReasonDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "followUpTypeDetail";
    cmd.PanelID = "followUpTypeDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "followUpTypeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpType_Detail.html";
    cmd.ActionPanContainer = "actionPanfollowUpTypeDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "followUpRemittanceCodeDetail";
    cmd.PanelID = "followUpRemittanceCodeDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "followUpRemittanceCodeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpRemittanceCode_Detail.html";
    cmd.ActionPanContainer = "actionPanfollowUpRemittanceCodeDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "followUpClaimStatusCodeDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "followUpClaimStatusCodeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpClaimStatusCode_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "followUpClaimStatusCategoryCodeDetail";
    cmd.PanelID = "followUpClaimStatusCategoryCodeDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "followUpClaimStatusCategoryCodeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpClaimStatusCategoryCode_Detail.html";
    cmd.ActionPanContainer = "actionPanfollowUpClaimStatusCategoryCodeDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "followUpCodesMappingDetail";
    cmd.PanelID = "followUpCodesMappingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "followUpCodesMappingDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpCodesMapping_Detail.html";
    cmd.ActionPanContainer = "actionPanFollowUpCodesMapping";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "FollowUp_Action_Detail";
    cmd.PanelID = "pnlFollowUpActionDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "FollowUp_Action_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanFollowUpActionDetail";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpAction_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_FollowUpGroup_Detail";
    cmd.PanelID = "pnlAdminFollowUpGroupDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_FollowUpGroup_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanFollowUpActionDetail";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpGroup_Detail.html";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "followUpAdjustmentCodeDetail";
    cmd.PanelID = "followUpAdjustmentCodeDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "followUpAdjustmentCodeDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/FollowUp/Admin_FollowUpAdjustmentCode_Detail.html";
    cmd.ActionPanContainer = "actionPanfollowUpAdjustmentCodeDetail";
    AddTab(cmd);
    // ----- </Follow Up> ----- //

    // ----- <IMO> ----- //
    var cmd = [];
    cmd.TabID = "IMODetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "IMODetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_IMOSearch_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_IMOICD";
    cmd.PanelID = "pnlAdminIMOICD";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_IMOICD";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanAdminIMOICD";
    cmd.Path = "./Controls/Admin/Admin_IMOICD.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_IMOCPT";
    cmd.PanelID = "pnlAdminIMOCPT";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_IMOCPT";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanAdminIMOCPT";
    cmd.Path = "./Controls/Admin/Admin_IMOCPT.html";
    AddTab(cmd);

    // ----- </IMO> ----- //

    cmd = [];
    cmd.TabID = "Clinical_PhysicalExamObservations";
    cmd.PanelID = "pnlAdminPEObservations";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "adminTabObservations";
    cmd.ContainerControlID = "Clinical_PhysicalExamObservations";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PhysicalExam/Clinical_PhysicalExamObservations.html";
    cmd.ActionPanContainer = "actionPanAdminPEObservations";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_ROSSystems";
    cmd.PanelID = "pnlROSSystems";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "adminTabROSSystems";
    cmd.ContainerControlID = "Clinical_ROSSystems";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/Clinical_ROSSystems.html";
    cmd.ActionPanContainer = "actionPanAdminROSSystems";
    AddTab(cmd);

    cmd =[];
    cmd.TabID = "MIPSPreference_IndividualProvider";
    cmd.PanelID = "pnlIndividualProvider";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "adminTabIndividualProvider";
    cmd.ContainerControlID = "MIPSPreference_IndividualProvider";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/MIPSPreference_IndividualProvider.html";
    cmd.ActionPanContainer = "actionPanIndividualProvider";
    AddTab(cmd);

    cmd =[];
    cmd.TabID = "MIPSPreference_IndividualProvider";
    cmd.PanelID = "pnlIndividualProvider";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "adminTabIndividualProvider";
    cmd.ContainerControlID = "MIPSPreference_IndividualProvider";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/MIPSPreference_IndividualProvider.html";
    cmd.ActionPanContainer = "actionPanIndividualProvider";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "MIPS_AdminPreferenceGroup";
    cmd.PanelID = "pnlMIPSAdminPreferenceGroup";
    cmd.MasterTabID = "adminTabIndividualProvider";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "MIPS_AdminPreferenceGroup";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/MIPS_AdminPreferenceGroup.html";
    cmd.ActionPanContainer = "actionPanMIPSAdminPreferenceGroup";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "MIPS_AdminPreferenceGroupDetail";
    cmd.PanelID = "pnlMIPSAdminPreferenceGroupDetail";
    cmd.MasterTabID = "adminTabIndividualProviderDetail";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "MIPS_AdminPreferenceGroupDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/MIPS_AdminPreferenceGroupDetail.html";
    cmd.ActionPanContainer = "actionPanMIPSAdminPreferenceGroupDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_AdminIPPreference";
    cmd.PanelID = "pnliTrackAdminIPPreference";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_AdminIPPreference";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPaniTrackAdminIPPreference";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminIPPreference.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "MIPS_AdminPreferenceGroup";
    cmd.PanelID = "pnlMIPSAdminPreferenceGroup";
    cmd.MasterTabID = "adminTabIndividualProvider";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "MIPS_AdminPreferenceGroup";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/MIPS_AdminPreferenceGroup.html";
    cmd.ActionPanContainer = "actionPanMIPSAdminPreferenceGroup";
    AddTab(cmd);

    //cmd = [];
    //cmd.TabID = "iTrack_AdminPreferenceIndividualReporting";
    //cmd.PanelID = "pnlMIPSAdminPreferenceIndividualReporting";
    //cmd.MasterTabID = "adminTabiTrackIndividualReporting";
    //cmd.ParentTabID = "";
    //cmd.ContainerControlID = "iTrack_AdminPreferenceIndividualReporting";
    //cmd.Selected = false;
    ////cmd.isActionPan = true;
    //cmd.Container = "";
    //cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminPreferenceIndividualReporting.html";
    //cmd.ActionPanContainer = "actionPanMIPSAdminPreferenceIndividualReporting";
    //AddTab(cmd);

    //cmd = [];
    //cmd.TabID = "iTrack_AdminPreferenceIndividualReportingDetail";
    //cmd.PanelID = "pnlMIPSAdminPreferenceIndividualReportingDetail";
    //cmd.ParentTabID = "";
    //cmd.MasterTabID = "adminTabiTrackIndividualReporting";
    //cmd.ContainerControlID = "iTrack_AdminPreferenceIndividualReportingDetail";
    //cmd.Selected = false;
    ////cmd.isActionPan = true;
    //cmd.Container = "";
    //cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminPreferenceIndividualReportingDetail.html";
    //cmd.ActionPanContainer = "actionPanMIPSAdminPreferenceIndividualReportingDetail";
    //AddTab(cmd);

    cmd = [];
    cmd.TabID = "iTrack_AdminPreferenceGroupReportingDetail";
    cmd.PanelID = "pnlMIPSAdminPreferenceGroupReportingDetail";
    cmd.ParentTabID = "";
    cmd.MasterTabID = "adminTabMIPSGroupReporting";
    cmd.ContainerControlID = "iTrack_AdminPreferenceGroupReportingDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminPreferenceGroupReportingDetail.html";
    cmd.ActionPanContainer = "actionPanGroupReportingDetail";
    AddTab(cmd);



    cmd = [];
    cmd.TabID = "MIPS_AdminPreferenceGroupDetail";
    cmd.PanelID = "pnlMIPSAdminPreferenceGroupDetail";
    cmd.MasterTabID = "adminTabIndividualProviderDetail";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "MIPS_AdminPreferenceGroupDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/MIPS_AdminPreferenceGroupDetail.html";
    cmd.ActionPanContainer = "actionPanMIPSAdminPreferenceGroupDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_AdminIPPreference";
    cmd.PanelID = "pnliTrackAdminIPPreference";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_AdminIPPreference";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPaniTrackAdminIPPreference";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminIPPreference.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_ROSCharatristics";
    cmd.PanelID = "pnlAdminROSChatristics";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "adminTabCharatristics";
    cmd.ContainerControlID = "Clinical_ROSCharatristics";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/Clinical_ROSCharatristics.html";
    cmd.ActionPanContainer = "actionPanAdminROSChatristics";
    AddTab(cmd);

    //Start 24-02-2016 Muhammad Arshad CDS Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalCDSDetail";
    cmd.PanelID = "ClinicalCDSDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalCDSDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalCDSDetail";
    cmd.Path = "./EMR/HTML/Clinical/CDS/Clinical_CDSDetail.html";
    AddTab(cmd);
    //End 24-02-2016 Muhammad Arshad CDS Detail Popup changes

    var cmd = [];
    cmd.TabID = "ClinicalCDSQuestionnaireDropdown";
    cmd.PanelID = "ClinicalCDSQuestionnaireDropdown";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalCDSQuestionnaireDropdown";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalCDSQuestionnaireDropdown";
    cmd.Path = "./EMR/HTML/Clinical/CDS/Clinical_CDSQuestionnaireDropdown.html";
    AddTab(cmd);

    //Start// 09-03-2016//Ahmad Raza//Adding Command to show CDS as popup
    var cmd = [];
    cmd.TabID = "Clinical_CDSAlert";
    cmd.PanelID = "pnlClinicalCDSAlert";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CDSAlert";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalCDSAlert";
    cmd.Path = "./EMR/HTML/Clinical/CDS/Clinical_CDSAlert.html";
    AddTab(cmd);
    //End// 09-03-2016//Ahmad Raza//Adding Command to show CDS as popup

    //Start// 09-03-2016//Ahmad Raza//Adding Command to show CDS as popup
    var cmd = [];
    cmd.TabID = "Clinical_CDSAlertDetail";
    cmd.PanelID = "pnlClinicalCDSAlertDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CDSAlertDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalCDSAlertDetail";
    cmd.Path = "./EMR/HTML/Clinical/CDS/Clinical_CDSAlertDetail.html";
    AddTab(cmd);
    //End// 09-03-2016//Ahmad Raza//Adding Command to show CDS as popup

    //Start 26-02-2016 Muhammad Arshad PhysicalExam Template Detail Popup changes
    var cmd = [];
    cmd.TabID = "PhysicalExamTemplateDetail";
    cmd.PanelID = "pnlPhysicalExamTemplateDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanPhysicalExamTemplateDetail";
    cmd.ContainerControlID = "PhysicalExamTemplateDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/PhysicalExamTemplateDetail.html";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "PhysicalExamTemplateDetailRevamp";
    cmd.PanelID = "pnlPhysicalExamTemplateDetailRevamp";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanPhysicalExamTemplateDetailRevamp";
    cmd.ContainerControlID = "PhysicalExamTemplateDetailRevamp";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/PhysicalExamTemplateDetailRevamp.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "AOETemplateDetail";
    cmd.PanelID = "pnlAOETemplateDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanAOETemplateDetail";
    cmd.ContainerControlID = "AOETemplateDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/AOETemplateDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ProcedureTemplateDetail";
    cmd.PanelID = "pnlProcedureTemplateDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanProcedureTemplateDetail";
    cmd.ContainerControlID = "ProcedureTemplateDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/ProcedureTemplateDetail.html";
    AddTab(cmd);

    //End 26-02-2016 Muhammad Arshad PhysicalExam Template Detail Popup changes

    //Start 29-02-2016 ZeeshanAK Review Of Systems Template Detail Popup changes
    var cmd = [];
    cmd.TabID = "ReviewOfSystemsTemplateDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ReviewOfSystemsTemplateDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/ReviewOfSystemsTemplateDetail.html";
    AddTab(cmd);
    //End 29-02-2016 ZeeshanAK Review Of Systems Template Detail Popup changes

    //Start 30-03-2016 ZeeshanAK Review Of Systems Data Template Detail Popup changes
    var cmd = [];
    cmd.TabID = "ReviewOfSystemsDataTemplateDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ReviewOfSystemsDataTemplateDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/DataTemplates/ReviewOfSystemsDataTemplateDetail.html";
    AddTab(cmd);
    //End 30-03-2016 ZeeshanAK Review Of Systems Data Template Detail Popup changes

    //Start 13-06-2016 Abid Ali Physical Exam Data Template Detail Popup changes
    var cmd = [];
    cmd.TabID = "PhysicalExamDataTemplateDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "PhysicalExamDataTemplateDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalPhysicalExamDataTemplateDetail";
    cmd.Path = "./EMR/HTML/Clinical/DataTemplates/PhysicalExamDataTemplateDetail.html";
    AddTab(cmd);
    //End 13-06-2016 Abid Ali Physical Exam Data Template Detail Popup changes

    //Start 13-06-2016 Abid Ali Physical Exam Data Template Save As Popup changes
    var cmd = [];
    cmd.TabID = "PhysicalExamDataTemplateSaveAs";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "PhysicalExamDataTemplateSaveAs";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/DataTemplates/PhysicalExamDataTemplateSaveAs.html";
    AddTab(cmd);
    //End 13-06-2016 Abid Ali Physical Exam Data Template Save As Popup changes

    //Start 01-07-2016 Farooq Ahmad Physical Exam Template Save As Popup changes
    var cmd = [];
    cmd.TabID = "PhysicalExamTemplateSaveAs";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "PhysicalExamTemplateSaveAs";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/PhysicalExamTemplateSaveAs.html";
    AddTab(cmd);


    //ROS Detail Revamp
    var cmd = [];
    cmd.TabID = "ROSTemplateDetailRevamp";
    cmd.PanelID = "pnlROSTemplateDetailRevamp";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ROSTemplateDetailRevamp";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/ROSTemplateDetailRevamp.html";
    cmd.ActionPanContainer = "actionPanROSTemplateDetailRevamp";
    AddTab(cmd);



    var cmd = [];
    cmd.TabID = "ROSTemplateRevamp";
    cmd.PanelID = "pnlROSTemplateRevamp";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ROSTemplateRevamp";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/ROSTemplateRevamp.html";
    cmd.ActionPanContainer = "actionPanROSTemplateRevamp";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_OrderSets";
    cmd.PanelID = "pnlClinicalOrderSets";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_OrderSets";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_OrderSets.html";
    cmd.ActionPanContainer = "actionPanClinicalOrderSets";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_OrderSetDetails";
    cmd.PanelID = "pnlClinicalOrderSetDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_OrderSetDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_OrderSetDetails.html";
    cmd.ActionPanContainer = "actionPanClinicalOrderSetDetails";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "OrderSet_PatientEducation";
    cmd.PanelID = "pnlClinicalOrderSetPatientEducation";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_PatientEducation";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_PatientEducation.html";
    cmd.ActionPanContainer = "actionPanClinicalOrderSetPatientEducation";
    AddTab(cmd);
    cmd = [];
    cmd.TabID = "OrderSet_ImmunizationDetail";
    cmd.PanelID = "pnlOrderSetImmunizationDetail";//"pnlClinicalQuestionGroupDetail
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_ImmunizationDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_ImmunizationDetail.html";
    cmd.ActionPanContainer = "actionPanOrderSetImmunizationDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "OrderSet_TherapeuticDetail";
    cmd.PanelID = "pnlOrderSetTherapeuticInjection";//"pnlClinicalQuestionGroupDetail
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_TherapeuticDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_TherapeuticDetail.html";
    cmd.ActionPanContainer = "actionPanOrderSetTherapeuticInjection";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "OrderSet_Medications";
    cmd.PanelID = "pnlOrderSetMedications";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_Medications";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_Medications.html";
    cmd.ActionPanContainer = "actionPanOrderSetMedications";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "FavMedication_Detail";
    cmd.PanelID = "pnlFavMedicationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "FavMedication_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/FavMedication_Detail.html";
    cmd.ActionPanContainer = "actionPanFavMedicationDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Favorite_MedicationDetail";
    cmd.PanelID = "pnlFavoriteMedicationDetail";
    cmd.ContainerControlID = "Favorite_MedicationDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_MedicationDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteMedicationDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Letter_Template";
    cmd.PanelID = "pnlLetterTemplate";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Letter_Template";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/TemplateBuilderNew/Letter_Template.html";
    cmd.ActionPanContainer = "actionPanLetterTemplate";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Add_Letter_Template";
    cmd.PanelID = "pnlAddLetterTemplate";
    cmd.ContainerControlID = "Add_Letter_Template";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/TemplateBuilderNew/Add_Letter_Template.html";
    cmd.ActionPanContainer = "actionPanAddLetterTemplate";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "Clinical_Provider_Note_Template";
    cmd.PanelID = "pnlClinicalProviderNoteTemplate";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_Provider_Note_Template";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_Provider_Note_Template.html";
    cmd.ActionPanContainer = "actionPanClinicalProviderNoteTemplate";
    AddTab(cmd);



    cmd = [];
    cmd.TabID = "Clinical_Add_Provider_Note_Template";
    cmd.PanelID = "pnlClinicalAddProviderNoteTemplate";
    cmd.ContainerControlID = "Clinical_Add_Provider_Note_Template";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_Add_Provider_Note_Template.html";
    cmd.ActionPanContainer = "actionPanClinicalAddProviderNoteTemplate";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_OrderSets";
    cmd.PanelID = "pnlClinicalOrderSets";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_OrderSets";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_OrderSets.html";
    cmd.ActionPanContainer = "actionPanClinicalOrderSets";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_CustomForms";
    cmd.PanelID = "pnlClinicalCustomForms";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CustomForms";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/CustomForms/Clinical_CustomForms.html";
    cmd.ActionPanContainer = "actionPanClinicalCustomForms";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_CustomFormsDetails";
    cmd.PanelID = "pnlClinicalCustomFormsDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CustomFormsDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/CustomForms/Clinical_CustomFormsDetails.html";
    cmd.ActionPanContainer = "actionPanAdminCustomForms";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_CustomFormsPreview";
    cmd.PanelID = "pnlClinicalCustomFormsPreview";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CustomFormsPreview";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/CustomForms/Clinical_CustomFormsPreview.html";
    cmd.ActionPanContainer = "actionPanClinicalCustomFormsPreview";
    AddTab(cmd);



    var cmd = [];
    cmd.TabID = "GlobalQuestionDetail";
    cmd.PanelID = "pnlGlobalQuestionDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "GlobalQuestionDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/CustomForms/Clinical_GlobalQuestionDetail.html";
    cmd.ActionPanContainer = "actionPanGlobalQuestionDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_GlobalQuestionGroup";
    cmd.PanelID = "pnlClinicalGlobalQuestionGroup";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_GlobalQuestionGroup";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/CustomForms/Clinical_GlobalQuestionGroup.html";
    cmd.ActionPanContainer = "actionPanGlobalQuestionGroup";
    AddTab(cmd);
    //add by azhar on 7/22/2016
    cmd = [];
    cmd.TabID = "PQRS_IndividualReporting_Detail";
    cmd.PanelID = "pnlPQRS_IndividualReporting_Detail";
    cmd.ContainerControlID = "PQRS_IndividualReporting_Detail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_IndividualReporting_Detail.html";
    cmd.ActionPanContainer = "actionPanPQRS_IndividualReporting_Detail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "PQRS_MeasureGroups_Detail";
    cmd.PanelID = "pnlPQRS_MeasureGroups_Detail";
    cmd.ContainerControlID = "PQRS_MeasureGroups_Detail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_MeasureGroups_Detail.html";
    cmd.ActionPanContainer = "actionPanPQRS_MeasureGroups_Detail";
    AddTab(cmd);
    cmd = [];
    cmd.TabID = "PQRS_CMSView";
    cmd.PanelID = "pnlPQRS_CMSView";
    cmd.ContainerControlID = "PQRS_CMSView";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_CMSView.html";
    cmd.ActionPanContainer = "actionPanPQRS_CMSView";
    AddTab(cmd);

    // end added by azhar on 7/22/2016


    //Start 16-03-2016 Muhammad Arshad RadiologyOrder Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalRadiologyOrderDetail";
    cmd.PanelID = "pnlClinicalRadiologyOrderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalRadiologyOrderDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalRadiologyOrderDetail";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_RadiologyOrderDetail.html";
    AddTab(cmd);
    //End 16-03-2016 Muhammad Arshad RadiologyOrder Detail Popup changes

    //Start 21-03-2016 Ahmad Raza ProcedureOrder  changes
    var cmd = [];
    cmd.TabID = "Clinical_ProcedureOrder";
    cmd.PanelID = "pnlClinicalProcedureOrder";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ProcedureOrder";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalProcedureOrder";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_ProcedureOrder.html";
    AddTab(cmd);
    //End 21-03-2016 Ahmad Raza ProcedureOrder  changes

    //Start 21-03-2016 Ahmad Raza ConsultationOrder  changes
    var cmd = [];
    cmd.TabID = "Clinical_ConsultationOrder";
    cmd.PanelID = "pnlClinicalConsultationOrder";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ConsultationOrder";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalConsultationOrder";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_ConsultationOrder.html";
    AddTab(cmd);
    //End 21-03-2016 Ahmad Raza ConsultationOrder  changes

    //Start 21-03-2016 Ahmad Raza RadiologyOrder  changes
    var cmd = [];
    cmd.TabID = "Clinical_RadiologyOrder";
    cmd.PanelID = "pnlClinicalRadiologyOrder";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_RadiologyOrder";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalRadiologyOrder";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_RadiologyOrder.html";
    AddTab(cmd);
    //End 21-03-2016 Ahmad Raza RadiologyOrder  changes

    //Start 17-03-2016 Muhammad Arshad ProcedureOrder Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalProcedureOrderDetail";
    cmd.PanelID = "pnlClinicalProcedureOrderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalProcedureOrderDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalProcedureOrderDetail";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_ProcedureOrderDetail.html";
    AddTab(cmd);
    //End 17-03-2016 Muhammad Arshad ProcedureOrder Detail Popup changes



    //Start 17-03-2016 Muhammad Arshad ConsultationOrder Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalConsultationOrderDetail";
    cmd.PanelID = "pnlClinicalConsultationOrderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalConsultationOrderDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalConsultationOrderDetail";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_ConsultationOrderDetail.html";
    AddTab(cmd);
    //End 17-03-2016 Muhammad Arshad ConsultationOrder Detail Popup changes

    //Start 22-03-2016 Humaira Yousaf to view PDF
    cmd = [];
    cmd.TabID = "Clinical_ProcedureOrderView";
    cmd.PanelID = "Clinical_ProcedureOrderView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ProcedureOrderView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_ProcedureOrderView.html";
    cmd.ActionPanContainer = "actionPanProcedureOrderView";
    AddTab(cmd);
    //End 22-03-2016 Humaira Yousaf to view PDF

    //Start 22-03-2016 Muhammad Ahmad Imran Favorite_Complaints Detail Popup changes
    cmd = [];
    cmd.TabID = "Favorite_Complaints_Detail";
    cmd.PanelID = "pnlFavoriteComplaintsDetail";
    cmd.ContainerControlID = "Favorite_Complaints_Detail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_Complaints_Detail.html";
    cmd.ActionPanContainer = "actionPanFavoriteComplaintsDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Favorite_Vaccine_Detail";
    cmd.PanelID = "pnlFavoriteVaccineDetail";
    cmd.ContainerControlID = "Favorite_Vaccine_Detail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_Vaccine_Detail.html";
    cmd.ActionPanContainer = "actionPanFavoriteVaccineDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Favorite_TherapueticInjection_Detail";
    cmd.PanelID = "pnlFavoriteTherapueticInjectionDetail";
    cmd.ContainerControlID = "Favorite_TherapueticInjection_Detail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_TherapueticInjection_Detail.html";
    cmd.ActionPanContainer = "actionPanFavoriteTherapueticInjectionDetail";
    AddTab(cmd);


    //Start 23-03-2016 Muhammad Arshad Favorite_ProcedureOrderDetail popup
    cmd = [];
    cmd.TabID = "Favorite_ProcedureOrderDetail";
    cmd.PanelID = "pnlFavoriteProcedureOrderDetail";
    cmd.ContainerControlID = "Favorite_ProcedureOrderDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_ProcedureOrderDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteProcedureOrderDetail";
    AddTab(cmd);
    //End 23-03-2016 Muhammad Arshad Favorite_ProcedureOrderDetail popup



    //Start 31-03-2016 Muhammad Ahmad Imran Favorite_ProcedureDetail popup
    cmd = [];
    cmd.TabID = "Favorite_ProcedureDetail";
    cmd.PanelID = "pnlFavoriteProcedureDetail";
    cmd.ContainerControlID = "Favorite_ProcedureDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_ProcedureDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteProcedureDetail";
    AddTab(cmd);
    //End 31-03-2016 Muhammad Ahmad Imran Favorite_ProcedureOrderDetail popup

    cmd = [];
    cmd.TabID = "Favorite_ProblemsDetail";
    cmd.PanelID = "pnlFavoriteProblemsDetail";
    cmd.ContainerControlID = "Favorite_ProblemsDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_ProblemsDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteProblemsDetail";
    AddTab(cmd);


    //Start 23-03-2016 Muhammad Arshad Favorite_ConsultationOrderDetail popup
    cmd = [];
    cmd.TabID = "Favorite_ConsultationOrderDetail";
    cmd.PanelID = "pnlFavoriteConsultationOrderDetail";
    cmd.ContainerControlID = "Favorite_ConsultationOrderDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_ConsultationOrderDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteConsultationOrderDetail";
    AddTab(cmd);
    //End 23-03-2016 Muhammad Arshad Favorite_ConsultationOrderDetail popup
    cmd = [];
    cmd.TabID = "Favorite_CustomFormsDetail";
    cmd.PanelID = "pnlFavoriteCustomFormsDetail";
    cmd.ContainerControlID = "Favorite_CustomFormsDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_CustomFormsDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteCustomFormsDetail";
    AddTab(cmd);
    //Start 23-03-2016 Muhammad Arshad Favorite_RadiologyOrderDetail popup
    cmd = [];
    cmd.TabID = "Favorite_RadiologyOrderDetail";
    cmd.PanelID = "pnlFavoriteRadiologyOrderDetail";
    cmd.ContainerControlID = "Favorite_RadiologyOrderDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_RadiologyOrderDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteRadiologyOrderDetail";
    AddTab(cmd);
    //End 23-03-2016 Muhammad Arshad Favorite_RadiologyOrderDetail popup
    //Start 23-03-2016 Humaira Yousaf to view PDF
    cmd = [];
    cmd.TabID = "Clinical_RadiologyOrderView";
    cmd.PanelID = "Clinical_RadiologyOrderView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_RadiologyOrderView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_RadiologyOrderView.html";
    cmd.ActionPanContainer = "actionPanRadiologyOrderView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_ConsultationOrderView";
    cmd.PanelID = "Clinical_ConsultationOrderView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ConsultationOrderView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_ConsultationOrderView.html";
    cmd.ActionPanContainer = "actionPanConsultationOrderView";
    AddTab(cmd);
    //End 23-03-2016 Humaira Yousaf to view PDF

    //Start 30-03-2016 Muhammad Arshad LabOrder Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalLabOrderDetail";
    cmd.PanelID = "pnlClinicalLabOrderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalLabOrderDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalLabOrderDetail";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_LabOrderDetail.html";
    AddTab(cmd);
    //End 30-03-2016 Muhammad Arshad LabOrder Detail Popup changes
    var cmd = [];
    cmd.TabID = "Clinical_LabOrderABNDetail";
    cmd.PanelID = "pnlClinical_LabOrderABNDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabOrderABNDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinical_LabOrderABNDetail";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_LabOrderABNDetail.html";
    AddTab(cmd);

    //Start 30-03-2016 Abid Ali LabOrder Detail AOE Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalLabOrderDetailAOE";
    cmd.PanelID = "ClinicalLabOrderDetailAOE";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalLabOrderDetailAOE";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalLabOrderDetailAOE";
    cmd.Path = "./EMR/HTML/Clinical/Orders/ClinicalLabOrderDetailAOE.html";
    AddTab(cmd);
    //End 30-03-2016 Abid Ali LabOrder Detail AOE Popup changes

    //Start//16-08-2016//Ahmad Raza//Patient List Popup Changes
    var cmd = [];
    cmd.TabID = "PQRS_Patient_List";
    cmd.PanelID = "Patient_List";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_List";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanPatientList";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_Patient_List.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "VBP_MissingDataAlert";
    cmd.PanelID = "pnlVBP_MissingDataAlert";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "VBP_MissingDataAlert";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanVBP_MissingDataAlert";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/VBP_MissingDataAlert.html";
    AddTab(cmd);
    //End//16-08-2016//Ahmad Raza//Patient List Popup Changes

    //Start//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Popup Changes
    var cmd = [];
    cmd.TabID = "PQRS_ICDCPTCodes";
    cmd.PanelID = "pnlICDCPTCodes";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "PQRS_ICDCPTCodes";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanICDCPTCodes";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_ICDCPTCodes.html";
    AddTab(cmd);
    //End//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Popup Changes


    var cmd = [];
    cmd.TabID = "PQRS_MissingDataView";
    cmd.PanelID = "pnlMissingDataView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "PQRS_MissingDataView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanMissingDataView";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_MissingDataView.html";
    AddTab(cmd);


    //Start 30-03-2016 Muhammad Arshad to view PDF
    cmd = [];
    cmd.TabID = "Clinical_LabOrderView";
    cmd.PanelID = "Clinical_LabOrderView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabOrderView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_LabOrderView.html";
    cmd.ActionPanContainer = "actionPanLabOrderView";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_LabOrderABNView";
    cmd.PanelID = "Clinical_LabOrderABNView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabOrderABNView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_LabOrderABNView.html";
    cmd.ActionPanContainer = "actionPanLabOrderABNView";
    AddTab(cmd);
    // Ahsan Nasir
    cmd = [];
    cmd.TabID = "Batch_FaxView";
    cmd.PanelID = "Batch_FaxView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Batch_FaxView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_FaxView.html";
    cmd.ActionPanContainer = "actionPanLabOrderABNView";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Batch_Fax";
    cmd.PanelID = "pnlBatchFax";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Batch_Fax";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_Fax.html";
    cmd.ActionPanContainer = "actionPanBatchFax";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Batch_Fax_QuickLink";
    cmd.PanelID = "pnlBatchFaxQuickLink";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Batch_Fax_QuickLink";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_Fax_QuickLink.html";
    cmd.ActionPanContainer = "actionPanBatchFaxQuickLink";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Batch_FaxSend";
    cmd.PanelID = "Batch_FaxSend";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Batch_FaxSend";
    cmd.Selected = false;
    //cmd.isActionPan = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_FaxSend.html";
    cmd.ActionPanContainer = "actionPanBatch_FaxSend";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Batch_FaxContacts";
    cmd.PanelID = "pnlBatch_FaxContacts";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "Batch_FaxContacts";
    cmd.ContainerControlID = "Batch_FaxContacts";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_FaxContacts.html";
    cmd.ActionPanContainer = "actionPanBatch_FaxContacts";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Batch_FaxSendAnnotate";
    cmd.PanelID = "pnlBatch_FaxSendAnnotate";
    cmd.MasterTabID = "Batch_FaxSend";
    cmd.ParentTabID = "Batch_FaxSend";
    cmd.ContainerControlID = "Batch_FaxSendAnnotate";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_FaxSendAnnotate.html";
    cmd.ActionPanContainer = "actionPanBatch_FaxSendAnnotate";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Batch_FaxClassifyPages";
    cmd.PanelID = "pnlBatch_FaxClassifyPage";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Batch_FaxClassifyPages";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanBatch_FaxClassifyPage";
    cmd.Path = "./Controls/Batch/Batch_FaxClassifyPages.html";
    AddTab(cmd);


    //Start 30-03-2016 Muhammad Arshad to view PDF

    //Start 31-03-2016 Humaira Yousaf for favorite family history
    cmd = [];
    cmd.TabID = "Favorite_FamilyHistoryDetail";
    cmd.PanelID = "pnlFavoriteFamilyHistoryDetail";
    cmd.ContainerControlID = "Favorite_FamilyHistoryDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_FamilyHistoryDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteFamilyHistoryDetail";
    AddTab(cmd);
    //End 31-03-2016 Humaira Yousaf for favorite family history

    //Start 31-03-2016 Humaira Yousaf for favorite medical history
    cmd = [];
    cmd.TabID = "Favorite_MedicalHistoryDetail";
    cmd.PanelID = "pnlFavoriteMedicalHistoryDetail";
    cmd.ContainerControlID = "Favorite_MedicalHistoryDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_MedicalHistoryDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteMedicalHistoryDetail";
    AddTab(cmd);
    //End 31-03-2016 Humaira Yousaf for favorite medical history

    cmd = [];
    cmd.TabID = "Favorite_HospitalizationHistoryDetail";
    cmd.PanelID = "pnlFavoriteHospitalizationHistoryDetail";
    cmd.ContainerControlID = "Favorite_HospitalizationHistoryDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_HospitalizationHistoryDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteHospitalizationHistoryDetail";
    AddTab(cmd);

    //Start 31-03-2016 Humaira Yousaf for favorite Surgical history
    cmd = [];
    cmd.TabID = "Favorite_SurgicalHistoryDetail";
    cmd.PanelID = "pnlFavoriteSurgicalHistoryDetail";
    cmd.ContainerControlID = "Favorite_SurgicalHistoryDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_SurgicalHistoryDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteSurgicalHistoryDetail";
    AddTab(cmd);
    //End 31-03-2016 Humaira Yousaf for favorite Surgical history


    //Start 31-03-2016 Muhammad Arshad Favorite_LabOrderDetail popup
    cmd = [];
    cmd.TabID = "Favorite_LabOrderDetail";
    cmd.PanelID = "pnlFavoriteLabOrderDetail";
    cmd.ContainerControlID = "Favorite_LabOrderDetail";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_LabOrderDetail.html";
    cmd.ActionPanContainer = "actionPanFavoriteLabOrderDetail";
    AddTab(cmd);
    //End 31-03-2016 Muhammad Arshad Favorite_LabOrderDetail popup





    //Start 05-04-2016 Muhammad Arshad Lab Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalLabDetail";
    cmd.PanelID = "pnlClinicalLabDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalLabDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalLabDetail";
    cmd.Path = "./EMR/HTML/Clinical/Lab/Clinical_LabDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_MacroDetail";
    cmd.PanelID = "pnlClinicalMacroDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_MacroDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalMacroDetail";
    cmd.Path = "./EMR/HTML/Clinical/Macros/Clinical_MacroDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_MacroQuickAddDetail";
    cmd.PanelID = "pnlMacroQuickAddDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_MacroQuickAddDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanMacroQuickAddDetail";
    cmd.Path = "./EMR/HTML/Clinical/Macros/Clinical_MacroQuickAddDetail.html";
    AddTab(cmd);
    //End 05-04-2016 Muhammad Arshad Lab Detail Popup changes

    cmd = [];
    cmd.TabID = "ClinicalLabTestAttributes";
    cmd.PanelID = "pnlClinicalLabTestAttributes";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalLabTestAttributes";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Lab/Clinical_LabTestAttributes.html";
    cmd.ActionPanContainer = "actionPanLabTestAttributes";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_LabTestAttributeResult";
    cmd.PanelID = "pnlClinicalLabTestAttributesResult";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabTestAttributeResult";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Lab/Clinical_LabTestAttributeResult.html";
    cmd.ActionPanContainer = "actionPanLabTestAttributesResult";
    AddTab(cmd);

    //Start 30-03-2016 Humaira Yousaf for Favorite History tab
    cmd = [];
    cmd.TabID = "Favorite_FamilyHistory";
    cmd.PanelID = "pnlFavoriteFamilyHistory";
    cmd.ContainerControlID = "Favorite_History";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "actionPanFavoriteHistory";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_FamilyHistory.html";
    cmd.ActionPanContainer = "actionPanFavoriteFamilyHistory";
    AddTab(cmd);
    //End 30-03-2016 Humaira Yousaf for Favorite History tab

    //Start 07-04-2016 Muhammad Arshad AuditReport Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalAuditReportDetail";
    cmd.PanelID = "pnlClinicalAuditReport";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalAuditReportDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalAuditReport";
    cmd.Path = "./EMR/HTML/Clinical/AuditReport/Clinical_AuditReport.html";
    AddTab(cmd);
    //End 07-04-2016 Muhammad Arshad AuditReport Detail Popup changes

    //Start 15-04-2016 Muhammad Arshad LabResult Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalLabResultDetail";
    cmd.PanelID = "pnlClinicalLabResultDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalLabResultDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalLabResultDetail";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_LabResultTrendsLetter";
    cmd.PanelID = "pnlClinical_LabResultTrendsLetter";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabResultTrendsLetter";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinical_LabResultTrendsLetter";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultTrendsLetter.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ClinicalLabResultTrends";
    cmd.PanelID = "pnlClinicalLabResultTrends";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalLabResultTrends";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalLabResultTrends";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultTrends.html";
    AddTab(cmd);
    

    var cmd = [];
    cmd.TabID = "ClinicalLabResultTrendsGraphs";
    cmd.PanelID = "pnlClinicalLabResultTrendsGraphs";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalLabResultTrendsGraphs";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalLabResultTrendsGraphs";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultTrendsGraphs.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_LabResultTrendsNotes";
    cmd.PanelID = "pnlClinical_LabResultTrendsNotes";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabResultTrendsNotes";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinical_LabResultTrendsNotes";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultTrendsNotes.html";
    AddTab(cmd);

    //End 15-04-2016 Muhammad Arshad LabResult Detail Popup changes

    //Start 15-04-2016 Muhammad Arshad LOINC Popup changes
    var cmd = [];
    cmd.TabID = "Clinical_LOINC";
    cmd.PanelID = "pnlClinicalLOINC";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LOINC";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalLOINC";
    cmd.Path = "./EMR/HTML/Clinical/LOINC/Clinical_LOINC.html";
    AddTab(cmd);
    //End 15-04-2016 Muhammad Arshad LOINC Popup changes

    //Start 21-04-2016 Muhammad Arshad RadiologyResult Detail Popup changes
    var cmd = [];
    cmd.TabID = "ClinicalRadiologyResultDetail";
    cmd.PanelID = "pnlClinicalRadiologyResultDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ClinicalRadiologyResultDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalRadiologyResultDetail";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_RadiologyResultDetail.html";
    AddTab(cmd);
    //End 21-04-2016 Muhammad Arshad RadiologyResult Detail Popup changes

    cmd = [];
    cmd.TabID = "Admin_ScheduleReason";
    cmd.PanelID = "pnlAdminScheduleReason";
    cmd.MasterTabID = "mstrTabAdmin";
    //cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_ScheduleReason";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ScheduleReason.html";
    cmd.ActionPanContainer = "actionPanAdminScheduleReason";
    AddTab(cmd);
    //Start 25-04-2016 Humaira Yousaf to view lab result pdf
    cmd = [];
    cmd.TabID = "Clinical_LabResultView";
    cmd.PanelID = "Clinical_LabResultView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabResultView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultView.html";
    cmd.ActionPanContainer = "actionPanLabResultView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_LabResultTrendsView";
    cmd.PanelID = "Clinical_LabResultTrendsView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabResultTrendsView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultTrendsView.html";
    cmd.ActionPanContainer = "actionPanLabResultView";
    AddTab(cmd);

    //End 25-04-2016 Humaira Yousaf to view lab result pdf

    //Start 02-05-2016 Humaira Yousaf to view radiology result pdf
    cmd = [];
    cmd.TabID = "Clinical_RadiologyResultView";
    cmd.PanelID = "Clinical_RadiologyResultView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_RadiologyResultView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_RadiologyResultView.html";
    cmd.ActionPanContainer = "actionPanRadiologyResultView";
    AddTab(cmd);
    //End 02-05-2016 Humaira Yousaf to view radiology result pdf

    cmd = [];
    cmd.TabID = "remindersTemplatesDetail";
    cmd.PanelID = "pnlRemindersTemplateDetail";
    cmd.MasterTabID = "mstrTabAdmin";
    //cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "remindersTemplatesDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_RemindersTemplates_Detail.html";
    cmd.ActionPanContainer = "actionPanRemindersTemplateDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "remindersSettingsDetail";
    cmd.PanelID = "pnlRemindersSettingDetail";
    cmd.MasterTabID = "mstrTabAdmin";
    //cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "remindersSettingsDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_RemindersSettings_Detail.html";
    cmd.ActionPanContainer = "actionPanRemindersSettingDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "remindersDetail";
    cmd.PanelID = "pnlRemindersDetail";
    cmd.MasterTabID = "mstrTabAdmin";
    //cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "remindersDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Reminders_Detail.html";
    cmd.ActionPanContainer = "actionPanRemindersDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Admin_Provider_eSignature";
    cmd.PanelID = "pnleSignatureDetail";
    cmd.MasterTabID = "";
    //cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Provider_eSignature.html";
    cmd.ActionPanContainer = "actionPanSignatureDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_AddReportHeaderTemplate";
    cmd.PanelID = "pnlAddReportHeaderTemplate";
    cmd.MasterTabID = "";
    //cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Clinical_AddReportHeaderTemplate";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReportHeader/Clinical_AddReportHeaderTemplate.html";
    cmd.ActionPanContainer = "actionPanAddReportHeaderTemplate";
    AddTab(cmd);

    // Added || 9 August 2016 || Talha Tanweer
    cmd = [];
    cmd.TabID = "Clinical_PreviewReportHeaderTemplate";
    cmd.PanelID = "pnlAddReportHeaderTemplate";
    cmd.MasterTabID = "";
    //cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Clinical_PreviewReportHeaderTemplate";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReportHeader/Clinical_PreviewReportHeaderTemplate.html";
    cmd.ActionPanContainer = "actionPanPreviewReportHeaderTemplate";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "Clinical_PhysicalExamSystemsDetail";
    cmd.PanelID = "pnlAdminPESystemsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_PhysicalExamSystemsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PhysicalExam/Clinical_PhysicalExamSystemsDetail.html";
    cmd.ActionPanContainer = "actionPanAdminPESystemsDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_PhysicalExamObservationsDetail";
    cmd.PanelID = "pnlAdminPEObservationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_PhysicalExamObservationsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PhysicalExam/Clinical_PhysicalExamObservationsDetail.html";
    cmd.ActionPanContainer = "actionPanAdminPEObservationDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_HPIFindingsDetail";
    cmd.PanelID = "pnlAdminHPIFindingsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_HPIFindingsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_HPIFindingsDetail.html";
    cmd.ActionPanContainer = "actionPanAdminHPIFindingsDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_HPISymptomsDetail";
    cmd.PanelID = "pnlAdminHPISymptomsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_HPISymptomsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_HPISymptomsDetail.html";
    cmd.ActionPanContainer = "actionPanAdminHPISymptomsDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_HPIFindings";
    cmd.PanelID = "pnlAdminHPIFindings";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_HPIFindings";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_HPIFindings.html";
    cmd.ActionPanContainer = "actionPanAdminHPIFindings";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_HPISymFindingDetail";
    cmd.PanelID = "pnlClinicalHPISymFindingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanClinicalHPISymFindingDetail";
    cmd.ContainerControlID = "Clinical_HPISymFindingDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_HPISymFindingDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_ROSCharatristicsDetail";
    cmd.PanelID = "pnlAdminROSCharatristicsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ROSCharatristicsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/Clinical_ROSCharatristicsDetail.html";
    cmd.ActionPanContainer = "actionPanAdminROSCharatristicsDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_ROSSystemsDetail";
    cmd.PanelID = "pnlAdminROSSystemsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ROSSystemsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/Clinical_ROSSystemsDetail.html";
    cmd.ActionPanContainer = "actionPanAdminROSSystemsDetail";
    AddTab(cmd);

    /* CCM */
    cmd = [];
    cmd.TabID = "Admin_CCMTemplates";
    cmd.PanelID = "pnlAdminCCMTemplates";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_CCMTemplates";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/CCM/Admin_CCMTemplates.html";
    cmd.ActionPanContainer = "actionPanAdminCCMTemplates";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_CCMTemplateDetails";
    cmd.PanelID = "pnlAdminCCMTemplateDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_CCMTemplateDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/CCM/Admin_CCMTemplateDetails.html";
    cmd.ActionPanContainer = "actionPanAdminCCMTemplateDetails";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_CCMTemplatePreview";
    cmd.PanelID = "pnlAdminCCMTemplatePreview";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_CCMTemplatePreview";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/CCM/Admin_CCMTemplatePreview.html";
    cmd.ActionPanContainer = "";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_CCMQuestionDetails";
    cmd.PanelID = "pnlAdminCCMQuestionDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_CCMQuestionDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/CCM/Admin_CCMQuestionDetails.html";
    cmd.ActionPanContainer = "actionPanAdminCCMQuestionDetails";
    AddTab(cmd);
    /* CCM */



    var cmd = [];
    cmd.TabID = "CCMEnrolledGoals";
    cmd.PanelID = "pnlCCMEnrolledGoals";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMEnrolledGoals";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMEnrolledGoals.html";
    cmd.ActionPanContainer = "actionPanCCMEnrolledGoals";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "CCMCareTeam";
    cmd.PanelID = "pnlCCMCareTeam";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMCareTeam";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCM_CareTeam.html";
    cmd.ActionPanContainer = "actionPanCCMCareTeam";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "CCMTermination";
    cmd.PanelID = "pnlCCMTermination";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMTermination";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMTermination.html";
    cmd.ActionPanContainer = "actionPanCCMTermination";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_CCMICDGroups_Detail";
    cmd.PanelID = "CCMICDGroupsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_CCMICDGroups_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/CCM/Admin_CCMICDGroups_Detail.html";
    cmd.ActionPanContainer = "actionPanCCMICDGroupsDetail";
    AddTab(cmd);

    // Order Sets
    var cmd = [];
    cmd.TabID = "OrderSet_Problems";
    cmd.PanelID = "pnlOrderSetProblemLists";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_Problems";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_Problems.html";
    cmd.ActionPanContainer = "actionPanOrderSetProblemLists";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "OrderSet_Procedures";
    cmd.PanelID = "pnlOrderSetProcedures";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_Procedures";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_Procedures.html";
    cmd.ActionPanContainer = "actionPanOrderSetProcedures";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "OrderSet_LabOrder";
    cmd.PanelID = "pnlOrderSetLabOrder";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_LabOrder";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_LabOrder.html";
    cmd.ActionPanContainer = "actionPanOrderSetLabOrder";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "OrderSet_LabOrderDetails";
    cmd.PanelID = "pnlOrderSetLabOrderDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_LabOrderDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_LabOrderDetails.html";
    cmd.ActionPanContainer = "actionPanOrderSetLabOrderDetails";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "OrderSet_Radiologyrder";
    cmd.PanelID = "pnlOrderSetRadiologyOrder";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_RadiologyOrder";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_RadiologyOrder.html";
    cmd.ActionPanContainer = "actionPanOrderSetRadiologyOrder";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "OrderSet_ProcedureOrderDetails";
    cmd.PanelID = "pnlOsProcedureOrderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_ProcedureOrderDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_ProcedureOrderDetails.html";
    cmd.ActionPanContainer = "actionPanOsProcedureOrderDetail";
    AddTab(cmd);



    var cmd = [];
    cmd.TabID = "OrderSet_RadiologyOrderDetails";
    cmd.PanelID = "pnlOrderSetRadiologyOrderDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_RadiologyOrderDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_RadiologyOrderDetails.html";
    cmd.ActionPanContainer = "actionPanOrderSetRadiologyOrderDetails";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "TinymceEditor";
    cmd.PanelID = "pnlTinymceEditor";
    cmd.ContainerControlID = "TinymceEditor";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/TinymceEditor.html";
    cmd.ActionPanContainer = "actionPanTinymceEditor";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "HPITemplateDetail";
    cmd.PanelID = "pnlHPITemplateDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanHPITemplateDetail";
    cmd.ContainerControlID = "HPITemplateDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/HPITemplateDetail.html";
    AddTab(cmd);

}

function BatchCommands() {

    var cmd = [];
    cmd.TabID = "mstrTabBatch";
    cmd.PanelID = "mstrDivBatch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.Container = "ctrlPanBatch";
    cmd.Path = "";
    cmd.ActionPanContainer = "actionPanBatch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Batch_ClinicalQualityMeasure";
    cmd.PanelID = "pnlBatchClinicalQualityMeasure";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Batch_ClinicalQualityMeasure";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_ClinicalQualityMeasure.html";
    cmd.ActionPanContainer = "actionPanBatchClinicalQualityMeasure";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Batch_ClinicalQualityMeasureDetail";
    cmd.PanelID = "pnlBatchClinicalQualityMeasureDetail";
    //cmd.MasterTabID = "mstrTabBatch";
    //cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Batch_ClinicalQualityMeasureDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_ClinicalQualityMeasure_Detail.html";
    cmd.ActionPanContainer = "actionPanBatchClinicalQualityMeasureDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Batch_ImportCCDA";
    cmd.PanelID = "pnlBatchImportCCDA";
    cmd.ContainerControlID = "Batch_ImportCCDA";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_ImportCCDA.html";
    cmd.ActionPanContainer = "actionPanBatchImportCCDA";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Batch_ExportCCDA";
    cmd.PanelID = "pnlBatchExportCCDA";
    cmd.ContainerControlID = "Batch_ExportCCDA";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_ExportCCDA.html";
    cmd.ActionPanContainer = "actionPanBatchExportCCDA";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Batch_PatientImportCCDA";
    cmd.PanelID = "pnlBatchPatientImportCCDA";
    cmd.ContainerControlID = "Batch_PatientImportCCDA";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_PatientImportCCDA.html";
    cmd.ActionPanContainer = "actionPanBatchPatientImportCCDA";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Batch_Fax";
    cmd.PanelID = "pnlBatchFax";
    cmd.ContainerControlID = "Batch_Fax";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_Fax.html";
    cmd.ActionPanContainer = "actionPanBatchFax";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Batch_ImportHL7ImmunizationBatch";
    cmd.PanelID = "pnlBatchImportHL7ImmunizationBatch";
    cmd.ContainerControlID = "Batch_ImportHL7ImmunizationBatch";
    cmd.Selected = false;
    cmd.MasterTabID = "";
    cmd.Container = "";
    cmd.Path = "./Controls/Batch/Batch_ImportHL7ImmunizationBatch.html";
    cmd.ActionPanContainer = "actionPanBatchImportHL7ImmunizationBatch";
    AddTab(cmd);
}

function ClinicalCommands() {


    var cmd = [];
    cmd.TabID = "DRFirst";
    cmd.PanelID = "pnlClinicalDRFirst";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "DRFirst";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/DRFirst.html";
    cmd.ActionPanContainer = "actionPanClinicalDRFirst";
    AddTab(cmd);




    var cmd = [];
    cmd.TabID = "mstrTabClinical";
    cmd.PanelID = "mstrDivClinical";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.Container = "ctrlPanClinical";
    cmd.Path = "";
    cmd.ActionPanContainer = "actionPanClinical";
    AddTab(cmd);
  


    cmd = [];
    cmd.TabID = "questionGroupDetail";
    cmd.PanelID = "";//"pnlClinicalQuestionGroupDetail
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "questionGroupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/TemplateBuilder/Clinical_Question_Group_Detail.html";
    //cmd.ActionPanContainer = "actionPanAdminCPTCode";
    AddTab(cmd);
    cmd = [];
    cmd.TabID = "Clinical_Template_Detail";
    cmd.PanelID = "pnlClinicalTemplate";//"pnlClinicalQuestionGroupDetail
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_Template_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/TemplateBuilder/Clinical_Template_Detail.html";
    //cmd.ActionPanContainer = "actionPanAdminCPTCode";
    AddTab(cmd);
    /*********************/

    cmd = [];
    cmd.TabID = "sectionDetail";
    cmd.PanelID = "sectionDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "sectionDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/TemplateBuilder/Clinical_Section_Detail.html";
    cmd.ActionPanContainer = "actionPansectionDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "sectionHeadingDetail";
    cmd.PanelID = "sectionHeadingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "sectionDetail";
    cmd.ContainerControlID = "sectionHeadingDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/TemplateBuilder/Clinical_Section_Heading.html";
    cmd.ActionPanContainer = "actionPansectionHeadingDetail";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "questionDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "questionDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/TemplateBuilder/Clinical_Question_Detail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "letterDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "letterDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Clinical/LetterDesign/Design_Letter_Detail.html";
    cmd.ActionPanContainer = "actionPanLetterDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "designLetterDataFieldsCreate";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "designLetterDataFieldsCreate";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Clinical/LetterDesign/Design_Letter_DataFields_Create.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "designLetterDataFieldsInsert";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "designLetterDataFieldsInsert";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Clinical/LetterDesign/Design_Letter_DataFields_Insert.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "designLetterPrinting";
    cmd.PanelID = "designLetterPrinting";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "designLetterPrinting";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Clinical/LetterDesign/Design_LetterPrinting.html";
    cmd.ActionPanContainer = "actionPanDesignLetterPrinting";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_Vitals";
    cmd.PanelID = "pnlClinicalVitals";

    cmd.ContainerControlID = "Clinical_Vitals";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_Vitals.html";
    cmd.ActionPanContainer = "actionPanClinicalVitals";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_NotesSearch";
    cmd.PanelID = "Clinical_NotesSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_NotesSearch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesSearch.html";
    cmd.ActionPanContainer = "actionPanNotesSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_NotesView";
    cmd.PanelID = "Clinical_NotesView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_NotesView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesView.html";
    cmd.ActionPanContainer = "actionPanNotesView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "NoteTemplatePreview";
    cmd.PanelID = "NoteTemplatePreview";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "NoteTemplatePreview";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/NoteTemplatePreview.html";
    cmd.ActionPanContainer = "actionPanNotesTemplateView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_NotesComponentSelection";
    cmd.PanelID = "Clinical_NotesComponentSelection";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_NotesComponentSelection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesComponentSelection.html";
    cmd.ActionPanContainer = "actionPanNotesComponentSelection";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_Copy_Note_Component_Selection";
    cmd.PanelID = "Clinical_Copy_Note_Component_Selection";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_Copy_Note_Component_Selection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_Copy_Note_Component_Selection.html";
    cmd.ActionPanContainer = "actionPanNoteCopyComponentSelection";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_VitalsComments";
    cmd.PanelID = "Clinical_VitalsComments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_VitalsComments";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_VitalsComments.html";
    cmd.ActionPanContainer = "actionPanVitalsComments";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_ProblemLists";
    cmd.PanelID = "pnlClinicalProblemLists";

    cmd.ContainerControlID = "Clinical_ProblemLists";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_ProblemLists.html";
    cmd.ActionPanContainer = "actionPanClinicalProblemLists";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_OrthoProblemList";
    cmd.PanelID = "pnlOrthoProblemList";
    cmd.ContainerControlID = "Clinical_OrthoProblemList";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "ctrlPanOrthoDetail";
    cmd.Path = "./EMR/HTML/Clinical/Orthopedic/Clinical_OrthoProblemList.html";
    cmd.ActionPanContainer = "actionPanOrthoProblemList";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_Complaints";
    cmd.PanelID = "pnlClinicalComplaints";

    cmd.ContainerControlID = "Clinical_Complaints";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_Complaints.html";
    cmd.ActionPanContainer = "actionPanClinicalComplaints";
    AddTab(cmd);

    
    cmd = [];
    cmd.TabID = "Clinical_Treatment";
    cmd.PanelID = "pnlClinicalTreatment";
    cmd.ContainerControlID = "Clinical_Treatment";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Treatment/Clinical_Treatment.html";
    cmd.ActionPanContainer = "actionPanClinicalTreatment";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Treatment_ProblemSelection";
    cmd.PanelID = "pnlTreatment_ProblemSelection";
    cmd.ContainerControlID = "Treatment_ProblemSelection";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Treatment/Treatment_ProblemSelection.html";
    cmd.ActionPanContainer = "actionPanTreatment_ProblemSelection";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_ComplaintsFaceSheet";
    cmd.PanelID = "pnlClinicalComplaintsFaceSheet";

    cmd.ContainerControlID = "Clinical_ComplaintsFaceSheet";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/FaceSheet/Clinical_ComplaintsFaceSheet.html";
    cmd.ActionPanContainer = "actionPanClinicalComplaintsFaceSheet";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_NotesExtraInfo";
    cmd.PanelID = "pnlClinicalNotesExtraInfo";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_NotesExtraInfo";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesExtraInfo.html";
    cmd.ActionPanContainer = "actionPanClinicalNotesExtraInfo";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_Procedures";
    cmd.PanelID = "pnlClinicalProcedures";

    cmd.ContainerControlID = "Clinical_Procedures";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_Procedures.html";
    cmd.ActionPanContainer = "actionPanClinicalProcedures";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_ProceduresComments";
    cmd.PanelID = "Clinical_ProcedureComments";
    cmd.ContainerControlID = "Clinical_ProceduresComments";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_ProceduresComments.html";
    cmd.ActionPanContainer = "actionPanProcedureComments";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_LabResultComments";
    cmd.PanelID = "pnlClinicalLabResultComments";
    cmd.ContainerControlID = "Clinical_LabResultComments";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultComments.html";
    cmd.ActionPanContainer = "actionPanProcedureComments";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_Allergies";
    cmd.PanelID = "pnlClinicalAllergies";

    cmd.ContainerControlID = "Clinical_Allergies";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_Allergies.html";
    cmd.ActionPanContainer = "actionPanClinicalAllergies";
    AddTab(cmd);

    //Start || 15 March, 2016 || ZeeshanAK || Added for Procedures
    cmd = [];
    cmd.TabID = "Clinical_Procedures";
    cmd.PanelID = "pnlClinicalProcedures";

    cmd.ContainerControlID = "Clinical_Procedures";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_Procedures.html";
    cmd.ActionPanContainer = "actionPanClinicalProcedures";
    AddTab(cmd);
    //End   || 15 March, 2016 || ZeeshanAK || Added for Procedures

    cmd = [];
    cmd.TabID = "MUStage1";
    cmd.PanelID = "pnlMUStage1";
    cmd.ContainerControlID = "MUStage1";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/MUReport/MUStage1.html";
    cmd.ActionPanContainer = "actionPanMUStage1";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_Medications";
    cmd.PanelID = "pnlClinicalMedications";
    cmd.ContainerControlID = "Clinical_Medications";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_Medications.html";
    cmd.ActionPanContainer = "actionPanClinicalMedications";
    AddTab(cmd);

    //Start//22-04-2016//Ahmad Raza//adding command for lab order
    cmd = [];
    cmd.TabID = "Clinical_LabOrder";
    cmd.PanelID = "pnlClinicalLabOrder";
    cmd.ContainerControlID = "Clinical_LabOrder";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_LabOrder.html";
    cmd.ActionPanContainer = "actionPanClinicalLabOrder";
    AddTab(cmd);
    //End//22-04-2016//Ahmad Raza//adding command for lab order

    //Added By Azhar Shahzad, for Lab Results Message Upload
    cmd = [];
    cmd.TabID = "Clinical_LabResultHL7_Import";
    cmd.PanelID = "pnlLabResultHL7_Import";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_LabResultHL7_Import";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanLabResultHL7_Import";
    cmd.Path = "./EMR/HTML/Clinical/Results/Clinical_LabResultHL7_Import.html";
    AddTab(cmd);

    //Start//22-04-2016//Ahmad Raza//adding command for lab order
    cmd = [];
    cmd.TabID = "Clinical_RadiologyOrder";
    cmd.PanelID = "pnlClinicalRadiologyOrder";
    cmd.ContainerControlID = "Clinical_RadiologyOrder";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_RadiologyOrder.html";
    cmd.ActionPanContainer = "actionPanClinicalRadiologyOrder";
    AddTab(cmd);
    //End//22-04-2016//Ahmad Raza//adding command for lab order

    cmd = [];
    cmd.TabID = "Clinical_SocialHx";
    cmd.PanelID = "pnlClinicalSocialHx";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_SocialHx";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_SocialHx.html";
    cmd.ActionPanContainer = "actionPanClinicalSocialHx";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_BirthHx";
    cmd.PanelID = "pnlClinicalBirthHx";
    cmd.ContainerControlID = "Clinical_BirthHx";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_BirthHx.html";
    cmd.ActionPanContainer = "actionPanClinicalBirthHx";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_SocPsyandBehaviorHx";
    cmd.PanelID = "pnlClinicalSocPsyandBehaviorHx";
    cmd.ContainerControlID = "Clinical_SocPsyandBehaviorHx";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_SocPsyandBehaviorHx.html";
    cmd.ActionPanContainer = "actionPanClinicalSocPsyandBehaviorHx";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_SocPsyandBehaviorHxView";
    cmd.PanelID = "pnlClinicalSocPsyandBehaviorHxView";
    cmd.ContainerControlID = "Clinical_SocPsyandBehaviorHxView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_SocPsyandBehaviorHxView.html";
    cmd.ActionPanContainer = "actionPanClinicalSocPsyandBehaviorHxView";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_ProblemListsComments";
    cmd.PanelID = "Clinical_ProblemListsComments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ProblemListsComments";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_ProblemListsComments.html";
    cmd.ActionPanContainer = "actionPanProblemListsComments";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_ProblemDetails";
    cmd.PanelID = "Clinical_ProblemDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ProblemDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_ProblemDetails.html";
    cmd.ActionPanContainer = "actionPanProblemListDetails";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "OrderSet_OrdAndPatientProbSelection";
    cmd.PanelID = "pnlOrderSetAndPatientProblem";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_OrdAndPatientProbSelection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_OrdAndPatientProbSelection.html";
    cmd.ActionPanContainer = "actionPanOrderSetPatientProblem";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_NotesProblemLists";
    cmd.PanelID = "Clinical_NotesProblemLists";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_NotesProblemLists";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_NotesProblemLists.html";
    cmd.ActionPanContainer = "actionPanNotesProblemLists";
    AddTab(cmd);

    //Start// 30/11/2015 Ahmad Raza//For AllergiesComments Popup
    cmd = [];
    cmd.TabID = "Clinical_AllergiesComments";
    cmd.PanelID = "Clinical_AllergiesComments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_AllergiesComments";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_AllergiesComments.html";
    cmd.ActionPanContainer = "actionPanAllergiesComments";
    AddTab(cmd);
    //End// 30/11/2015 Ahmad Raza//For AllergiesComments Popup

    cmd = [];
    cmd.TabID = "Clinical_ProblemListInActive";
    cmd.PanelID = "Clinical_ProblemListInActive";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ProblemListInActive";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_ProblemListInActive.html";
    cmd.ActionPanContainer = "actionPanProblemListInActive";
    AddTab(cmd);


    //Start// 30/11/2015 Ahmad Raza//For AllergiesInActive Popup
    cmd = [];
    cmd.TabID = "Clinical_AllergyInActive";
    cmd.PanelID = "Clinical_AllergyInActive";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_AllergyInActive";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_AllergyInActive.html";
    cmd.ActionPanContainer = "actionPanAllergyInActive";
    AddTab(cmd);
    //End // 30/11/2015 Ahmad Raza//For AllergiesInActive Popup

    /* Start 21/12/2015 Muhammad Irfan to open appointments detail from facesheet */
    cmd = [];
    cmd.TabID = "clinicalFaceSheetAppointments";
    cmd.PanelID = "clinicalFaceSheetAppointments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "clinicalFaceSheetAppointments";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/FaceSheet/Clinical_FaceSheetAppointments_Detail.html";
    cmd.ActionPanContainer = "actionPanClinicalFaceSheetAppointment";
    AddTab(cmd);
    /* End 21/12/2015 Muhammad Irfan to open appointments detail from facesheet */


    /* Start 21/12/2015 Muhammad Irfan to open appointments detail from facesheet */
    cmd = [];
    cmd.TabID = "Clinical_FaceSheetView";
    cmd.PanelID = "Clinical_FaceSheetView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_FaceSheetView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/FaceSheet/Clinical_FaceSheetView.html";
    cmd.ActionPanContainer = "actionPanFaceSheetView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_FaceSheet";
    cmd.PanelID = "pnlClinicalFaceSheet";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_FaceSheet";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/FaceSheet/Clinical_FaceSheet.html";
    cmd.ActionPanContainer = "actionPanClinicalFaceSheet";
    AddTab(cmd);
    /* End 21/12/2015 Muhammad Irfan to open appointments detail from facesheet */

    //Start//15-04-2016//Ahmad Raza// commands to open PDF View of AuditReport
    cmd = [];
    cmd.TabID = "Clinical_AuditReportView";
    cmd.PanelID = "Clinical_AuditReportView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_AuditReportView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/AuditReport/Clinical_AuditReportView.html";
    cmd.ActionPanContainer = "actionPanAuditReportView";
    AddTab(cmd);
    //End//15-04-2016//Ahmad Raza// commands to open PDF View of AuditReport

    /* Start 21/12/2015 Muhammad Irfan to open appointments detail from facesheet */
    cmd = [];
    cmd.TabID = "Clinical_InfoButtonView";
    cmd.PanelID = "pnlClinical_InfoButtonView";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_InfoButtonView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/InfoButton/Clinical_InfoButtonView.html";
    cmd.ActionPanContainer = "actionPanInfoButtonView";
    AddTab(cmd);

    /* Start 11/01/2015 Muhammad Irfan define action pan for MedicalHx */

    cmd = [];
    cmd.TabID = "Clinical_MedicalHx";
    cmd.PanelID = "pnlClinicalMedicalHx";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_MedicalHx";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_MedicalHx.html";
    cmd.ActionPanContainer = "actionPanClinicalMedicalHx";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_HistorySummary";
    cmd.PanelID = "pnlClinicalHistorySummary";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_HistorySummary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_HistorySummary.html";
    cmd.ActionPanContainer = "actionPanClinicalHistorySummary";
    AddTab(cmd);
    /* End 11/01/2015 Muhammad Irfan define action pan for MedicalHx */

    //Start//20/01/2016//Ahmad Raza//Defining commands to open FamilyHx,SurgicalHx,HospitalizationHx
    cmd = [];
    cmd.TabID = "Clinical_FamilyHx";
    cmd.PanelID = "pnlClinicalFamilyHx";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_FamilyHx";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_FamilyHx.html";
    cmd.ActionPanContainer = "actionPanClinicalFamilyHx";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_SurgicalHx";
    cmd.PanelID = "pnlClinicalSurgicalHx";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_SurgicalHx";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_SurgicalHx.html";
    cmd.ActionPanContainer = "actionPanClinicalSurgicalHx";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_HospitalizationHx";
    cmd.PanelID = "pnlClinicalHospitalizationHx";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_HospitalizationHx";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_HospitalizationHx.html";
    cmd.ActionPanContainer = "actionPanClinicalHospitalizationHx";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_PhysicalExam";
    cmd.PanelID = "pnlClinicalPhysicalExam";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_PhysicalExam";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PhysicalExam/Clinical_PhysicalExam.html";
    cmd.ActionPanContainer = "actionPanClinicalPhysicalExam";
    AddTab(cmd);
    //End//20/01/2016//Ahmad Raza//Defining commands to open FamilyHx,SurgicalHx,HospitalizationHx

    //Start//31/03/2016//Ahmad Raza//Defining commands to open PlanOfCare
    cmd = [];
    cmd.TabID = "Clinical_PlanOfCare";
    cmd.PanelID = "pnlPlanOfCare";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_PlanOfCare";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_PlanOfCare.html";
    cmd.ActionPanContainer = "actionPanPlanOfCare";
    AddTab(cmd);
    //End//31/03/2016//Ahmad Raza//Defining commands to open PlanOfCare


    cmd = [];
    cmd.TabID = "Clinical_ReviewofSystems";
    cmd.PanelID = "pnlClinicalReviewofSystems";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_ReviewofSystems";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewofSystems/Clinical_ReviewofSystems.html";
    cmd.ActionPanContainer = "actionPanClinicalReviewofSystems";
    AddTab(cmd);
    // Added by Zia Mehmood
    cmd = [];
    cmd.TabID = "Clinical_ROSTemplateDetailRevamp";
    cmd.PanelID = "pnlClinicalROSTemplateDetailRevamp";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_ROSTemplateDetailRevamp";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/Clinical_ROSTemplateDetailRevamp.html";
    cmd.ActionPanContainer = "actionPanClinicalROSTemplateDetailRevamp";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_FollowUpAppointment";
    cmd.PanelID = "pnlClinicalFollowUpAppointment";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_FollowUpAppointment";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/FollowUp/Clinical_FollowUpAppointment.html";
    cmd.ActionPanContainer = "actionPanClinicalFollowUpAppointment";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_FollowUpTCM";
    cmd.PanelID = "pnlClinicalFollowUpTCM";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_FollowUpTCM";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/FollowUp/Clinical_FollowUpTCM.html";
    cmd.ActionPanContainer = "actionPanClinicalFollowUpTCM";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "OrderSet_FollowUp";
    cmd.PanelID = "pnlOrderSetFollowUpAppointment";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "OrderSet_FollowUp";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_FollowUp.html";
    cmd.ActionPanContainer = "actionPanOrderSetFollowUpAppointment";
    AddTab(cmd);

    /* Start 31/03/2016 Muhammad Arshad to open clinical Summary */
    cmd = [];
    cmd.TabID = "Clinical_ClinicalSummary";
    cmd.PanelID = "clinicalSummary";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ClinicalSummary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_ClinicalSummary.html";
    cmd.ActionPanContainer = "actionPanClinicalSummary";
    AddTab(cmd);
    /* End 31/03/2016 Muhammad Arshad to open clinical Summary */
    cmd = [];
    cmd.TabID = "Clinical_CaseReports";
    cmd.PanelID = "CaseReports";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CaseReports";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/CaseReports/Clinical_CaseReports.html";
    cmd.ActionPanContainer = "actionPanCaseReports";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_CaseReportsHTML";
    cmd.PanelID = "Clinical_CaseReportsHTML";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CaseReportsHTML";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/CaseReports/Clinical_CaseReportsHTML.html";
    cmd.ActionPanContainer = "actionPanCaseReportsHTML";
    AddTab(cmd);

    /* Start 04/04/2016 Muhammad Arshad to open clinical SummaryHTML */
    cmd = [];
    cmd.TabID = "Clinical_ClinicalSummaryHTML";
    cmd.PanelID = "Clinical_ClinicalSummaryHTML";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ClinicalSummaryHTML";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_ClinicalSummaryHTML.html";
    cmd.ActionPanContainer = "actionPanClinicalSummaryHTML";
    AddTab(cmd);
    /* End 04/04/2016 Muhammad Arshad to open clinical SummaryHTML */


    /* Start 08/11/2017 Sameer Ahmed to open clinical Continuity Care Document */
    cmd = [];
    cmd.TabID = "Clinical_ReferralNote";
    cmd.PanelID = "clinicalReferralNote";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ReferralNote";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_ReferralNote.html";
    cmd.ActionPanContainer = "actionPanClinicalReferralNote";
    AddTab(cmd);
    /* End 08/11/2017 Sameer Ahmed to open clinical Continuity Care Document */

    /* Start 08/11/2017 Sameer Ahmed to open clinical Continuity Care Document */
    cmd = [];
    cmd.TabID = "Clinical_ContinuityofCareDocument";
    cmd.PanelID = "clinicalContinuityofCareDocument";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ContinuityofCareDocument";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_ContinuityofCareDocument.html";
    cmd.ActionPanContainer = "actionPanClinicalContinuityofCareDocument";
    AddTab(cmd);
    /* End 08/11/2017 Sameer Ahmed to open clinical Continuity Care Document */


    /* Start 05/04/2016 Farooq Ahmad to open clinical Referral */
    cmd = [];
    cmd.TabID = "Clinical_ReferralSummary";
    cmd.PanelID = "Clinical_ReferralSummary";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ReferralSummary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_ReferralSummary.html";
    cmd.ActionPanContainer = "actionPanReferralSummary";
    AddTab(cmd);
    /* End 05/04/2016 Farooq Ahmad to open clinical Referral */

    /* Start 05/05/2016 Farooq Ahmad to open TransmitCCDA */
    cmd = [];
    cmd.TabID = "Clinical_TransmitCCDA";
    cmd.PanelID = "Clinical_TransmitCCDA";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_TransmitCCDA";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_TransmitCCDA.html";
    cmd.ActionPanContainer = "actionPanTransmitCCDA";
    AddTab(cmd);
    /* End 05/05/2016 Farooq Ahmad to open TransmitCCDA */

    cmd = [];
    cmd.TabID = "Clinical_TransmitReferralNote";
    cmd.PanelID = "Clinical_TransmitReferralNote";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_TransmitReferralNote";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_TransmitReferralNote.html";
    cmd.ActionPanContainer = "actionPanTransmitReferralNote";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_TransmitContinuityofCareDocument";
    cmd.PanelID = "Clinical_TransmitContinuityofCareDocument";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_TransmitContinuityofCareDocument";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_TransmitContinuityofCareDocument.html";
    cmd.ActionPanContainer = "actionPanTransmitContinuityofCareDocument";
    AddTab(cmd);

    //Start 07-20-2016 Abid Ali commands to open template Type
    cmd = [];
    cmd.TabID = "Clinical_SuperBillTemplate";
    cmd.PanelID = "pnlClinicalSuperBillTemplate";
    //cmd.MasterTabID = "mstrTabBilling";
    cmd.ContainerControlID = "Clinical_SuperBillTemplate";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/BillingInformation/Clinical_SuperBillTemplate.html";
    cmd.ActionPanContainer = "actionPanClinicalSuperBillTemplate";
    AddTab(cmd);

    //End 07-20-2016 Abid Ali commands to open template Type

    //Start 28-07-2016 Humaira Yousaf for Patient Education
    cmd = [];
    cmd.TabID = "Clinical_PatientEducation";
    cmd.PanelID = "pnlClinicalPatientEducation";
    cmd.ContainerControlID = "Clinical_PatientEducation";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_PatientEducation.html";
    cmd.ActionPanContainer = "actionPanClinicalPatientEducation";
    AddTab(cmd);
    //End 28-07-2016 Humaira Yousaf for Patient Education'

    var cmd = [];
    cmd.TabID = "Clinical_CDSAlerts";
    cmd.PanelID = "pnlClinicalCDSAlerts";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CDSAlerts";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalCDSAlerts";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_CDSAlerts.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_CDSAlertDetails";
    cmd.PanelID = "pnlClinicalCDSAlertDetails";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_CDSAlertDetails";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanClinicalCDSAlertDetails";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_CDSAlertDetails.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "PhysicalExamSysObservationDetail";
    cmd.PanelID = "pnlPhysicalExamSysObservationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanPhysicalExamSysObservationDetail";
    cmd.ContainerControlID = "PhysicalExamSysObservationDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/PhysicalExamSysObservationDetail.html";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "AOESysObservationDetail";
    cmd.PanelID = "pnlAOESysObservationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanAOESysObservationDetail";
    cmd.ContainerControlID = "AOESysObservationDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/AOESysObservationDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ProcedureSysObservationDetail";
    cmd.PanelID = "pnlProcedureSysObservationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanProcedureSysObservationDetail";
    cmd.ContainerControlID = "ProcedureSysObservationDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/ProcedureSysObservationDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ProcedureOrderSysObservationDetail";
    cmd.PanelID = "pnlProcedureOrderSysObservationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanProcedureOrderSysObservationDetail";
    cmd.ContainerControlID = "ProcedureOrderSysObservationDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/ProcedureOrderSysObservationDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "AOESysRadiologyObservationDetail";
    cmd.PanelID = "pnlAOESysRadiologyObservationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanAOESysRadiologyObservationDetail";
    cmd.ContainerControlID = "AOESysRadiologyObservationDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/AOESysRadiologyObservationDetail.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "PhysicalExamTemplatesRevamp";
    cmd.PanelID = "pnlPhysicalExamTemplatesRevamp";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanPhysicalExamTemplatesRevamp";
    cmd.ContainerControlID = "PhysicalExamTemplatesRevamp";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/PhysicalExamTemplatesRevamp.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "VBP_PHQQuestionnaire";
    cmd.PanelID = "pnlPHQQuestionnaire";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanPHQQuestionnaire";
    cmd.ContainerControlID = "VBP_PHQQuestionnaire";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/VBP_PHQQuestionnaire.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "VBP_PHQ2Questionnaire";
    cmd.PanelID = "pnlPHQ2Questionnaire";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ActionPanContainer = "actionPanPHQ2Questionnaire";
    cmd.ContainerControlID = "VBP_PHQ2Questionnaire";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/VBP_PHQ2Questionaire.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_HPIComplaints";
    cmd.PanelID = "pnlClinicalHPIComplaints";

    cmd.ContainerControlID = "Clinical_HPIComplaints";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_HPIComplaints.html";
    cmd.ActionPanContainer = "actionPanClinicalHPIComplaints";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_CarePlan";
    cmd.PanelID = "pnlClinicalCarePlan";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_CarePlan";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_CarePlan.html";
    cmd.ActionPanContainer = "actionPanClinicalCarePlan";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_Cognitive";
    cmd.PanelID = "pnlClinicalCognitive";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_Cognitive";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_Cognitive.html";
    cmd.ActionPanContainer = "actionPanClinicalCognitive";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_HealthCareSurvey";
    cmd.PanelID = "clinicalHealthCareSurvey";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_HealthCareSurvey";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/ClinicalSummary/Clinical_HealthCareSurvey.html";
    cmd.ActionPanContainer = "actionPanClinicalHealthCareSurvey";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_FaceSheetComponentSelection";
    cmd.PanelID = "Clinical_FaceSheetComponentSelection";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_FaceSheetComponentSelection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/FaceSheet/Clinical_FaceSheetComponentSelection.html";
                  
    cmd.ActionPanContainer = "actionPanFaceSheetComponentSelection";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_OrthopedicChart";
    cmd.PanelID = "pnlOrthopedicChart";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_OrthopedicChart";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orthopedic/Clinical_OrthopedicChart.html";
    cmd.ActionPanContainer = "actionPanOrthopedicChart";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_OrthopedicChartDetail";
    cmd.PanelID = "pnlOrthopedicChartDetail";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "Clinical_OrthopedicChartDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orthopedic/Clinical_OrthopedicChartDetail.html";
    cmd.ActionPanContainer = "actionPanOrthopedicChartDetail";
    AddTab(cmd);
}

function ScheduleCommands() {

    var cmd = [];
    cmd.TabID = "mstrTabSchedule";
    cmd.PanelID = "mstrDivSchedule";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "schedule";
    cmd.Selected = false;
    cmd.Container = "ctrlPanSchedule";
    cmd.Path = "";
    cmd.ActionPanContainer = "actionPanSchedule";
    AddTab(cmd);

    var cmd = [];
    //Schedualing tabs
    cmd.TabID = "schTabCalendar";
    cmd.PanelID = "pnlPMSScheduler";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "mstrTabSchedule";
    cmd.ContainerControlID = "PMSScheduler";
    cmd.Selected = true;
    cmd.Container = "ctrlPanSchedule";
    cmd.Path = "./Controls/Scheduler/PMSScheduler.html";
    cmd.ActionPanContainer = "actionPanPMSScheduler";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "schTabMultipleView";
    cmd.PanelID = "pnlScheduleMuliView";//panal id from opening html control
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "mstrTabSchedule";
    cmd.ContainerControlID = "Scheduling_MuliView";
    cmd.Selected = true;
    cmd.Container = "ctrlPanSchedule";// in which container it will be open
    cmd.Path = "./Controls/Scheduling/Scheduling_MuliView.html";
    cmd.ActionPanContainer = "actionPanScheduleMuliView";//action pan of opening panal
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "schTabWaitList";
    cmd.PanelID = "pnlScheduleWaitList";//panal id from opening html control
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "mstrTabSchedule";
    cmd.ContainerControlID = "Scheduling_WaitList";
    cmd.Selected = true;
    cmd.Container = "ctrlPanSchedule";// in which container it will be open
    cmd.Path = "./Controls/Scheduling/Scheduling_WaitList.html";
    cmd.ActionPanContainer = "actionPanScheduleWaitList";//action pan of opening panal
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "blckreasonDetail";
    cmd.PanelID = "blckreasonDetail";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "blckreasonDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_SlotBlockUnblock.html";
    cmd.ActionPanContainer = "actionPanSlotBlockUnBlock";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "schAppointmentStatus";
    cmd.PanelID = "schAppointmentStatus";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "schAppointmentStatus";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_SearchAppointmentByStatus.html";
    cmd.ActionPanContainer = "actionPanSchAppointmentStatus";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "schEditSlot";
    cmd.PanelID = "schEditSlot";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "schEditSlot";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_EditSlot.html";
    cmd.ActionPanContainer = "actionPanSchEditSlot";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "multipleViewGroup";
    cmd.PanelID = "multipleViewGroup";
    cmd.MasterTabID = "pnlScheduleMuliView";
    cmd.ParentTabID = "pnlScheduleMuliView";
    cmd.ContainerControlID = "multipleViewGroup";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_MultipleView_Group.html";
    cmd.ActionPanContainer = "actionPanMultipleViewGroup";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "multipleViewGroupDetail";
    cmd.PanelID = "multipleViewGroupDetail";
    cmd.MasterTabID = "pnlScheduleMuliView";
    cmd.ParentTabID = "multipleViewGroup";
    cmd.ContainerControlID = "multipleViewGroupDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_MultipleView_Group_Detail.html";
    cmd.ActionPanContainer = "actionPanMultipleViewGroupDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "schcopayment";
    cmd.PanelID = "schcopayment";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "schcopayment";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_Copayment.html";
    cmd.ActionPanContainer = "actionPanCopayment";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_UnallocatedCopayment";
    cmd.PanelID = "pnlUnallocatedCopayment";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "Scheduling_UnallocatedCopayment";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_UnallocatedCopayment.html";
    cmd.ActionPanContainer = "actionPanUnallocatedCopayment";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "schcheckin";
    cmd.PanelID = "schcheckin";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "schcheckin";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_CheckIn.html";
    cmd.ActionPanContainer = "actionPanCheckIn";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_CheckInReason";
    cmd.PanelID = "pnlScheduling_CheckInReason";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_CheckInReason";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_CheckInReason.html";
    cmd.ActionPanContainer = "actionPanScheduling_CheckInReason";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "schcheckout";
    cmd.PanelID = "schcheckout";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "schcheckout";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_CheckOut.html";
    cmd.ActionPanContainer = "actionPanCheckOut";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "schChangeFacility";
    cmd.PanelID = "schChangeFacility";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabSearch";
    cmd.ContainerControlID = "schChangeFacility";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_ChangeFacility.html";
    cmd.ActionPanContainer = "actionPanSchChangeFacility";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "schTabSearch";
    cmd.PanelID = "pnlScheduleSearch";//panal id from opening html control
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "mstrTabSchedule";
    cmd.ContainerControlID = "Scheduling_Search";
    cmd.Selected = true;
    cmd.Container = "ctrlPanSchedule";// in which container it will be open
    cmd.Path = "./Controls/Scheduling/Scheduling_Search.html";
    cmd.ActionPanContainer = "actionPanScheduleSearch";//action pan of opening panal
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "appointmentHistory";
    cmd.PanelID = "pnlappointmenthistory";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "appointmentHistory";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_Appointment_History.html";
    cmd.ActionPanContainer = "actionPanAppointmentHistory";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "appointmentDetail";
    cmd.PanelID = "appointmentDetail";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "appointmentDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_Appointment_Detail.html";
    cmd.ActionPanContainer = "actionPanAppointmentDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_Force_Booking";
    cmd.PanelID = "PnlSchedulingForceBooking";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "Scheduling_Force_Booking";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_Force_Booking.html";
    cmd.ActionPanContainer = "actionPanSchedulingForceBooking";
    AddTab(cmd);
    cmd = [];
    cmd.TabID = "schwaitlistdetail";
    cmd.PanelID = "schwaitlistdetail";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "schwaitlistdetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_WaitListDeatil.html";
    cmd.ActionPanContainer = "actionPanWaitListdetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_AppointmentSearch";
    cmd.PanelID = "Scheduling_AppointmentSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_AppointmentSearch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_AppointmentSearch.html";
    cmd.ActionPanContainer = "actionPanAppointmentSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_AppointmentSearch";
    cmd.PanelID = "Scheduling_AppointmentSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_AppointmentSearch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_AppointmentSearch.html";
    cmd.ActionPanContainer = "actionPanAppointmentSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_BlockAppointment_Summary";
    cmd.PanelID = "Scheduling_BlockAppointment_Summary";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_BlockAppointment_Summary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_BlockAppointment_Summary.html";
    cmd.ActionPanContainer = "actionPanBlockAppointmentSummary";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_RescheduleAppointment";
    cmd.PanelID = "pnlRescheduleAppointment";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_RescheduleAppointment";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_RescheduleAppointment.html";
    cmd.ActionPanContainer = "actionPanRescheduleAppointment";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_RescheduleSearch";
    cmd.PanelID = "pnlRescheduleSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_RescheduleSearch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_Reschedule_Search.html";
    cmd.ActionPanContainer = "actionPanRescheduleSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Unallocated_CopaymentView";
    cmd.PanelID = "Unallocated_CopaymentView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Unallocated_CopaymentView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_Unallocated_CopaymentView.html";
    cmd.ActionPanContainer = "actionPanUnallocatedCopayView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_CopaymentView";
    cmd.PanelID = "pnlScheduling_CopaymentView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_CopaymentView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_CopaymentView.html";
    cmd.ActionPanContainer = "actionPanSchedulingCopaymentView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Scheduling_ProviderAppointmentPrint";
    cmd.PanelID = "pnlScheduling_ProviderAppointmentPrint";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabCalendar";
    cmd.ContainerControlID = "Scheduling_ProviderAppointmentPrint";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_ProviderAppointmentPrint.html";
    cmd.ActionPanContainer = "actionPanSchedulingPrint";
    AddTab(cmd);

}
function ReportsCommands() {

    var cmd = [];
    cmd.TabID = "mstrTabReports";
    cmd.PanelID = "pnlReportsSSRSDashboard";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ReportsSSRSDashboard";
    cmd.Selected = false;
    cmd.Container = "ctrlPanReports";
    cmd.Path = "./Controls/Reports/ReportsSSRSDashboard.html";
    cmd.ActionPanContainer = "actionPanReportsSSRSDashboard";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "ReportsSSRSPrintView";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ReportsSSRSPrintView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Reports/ReportsSSRSPrintView.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "BillClaimSubmitHistoryPrint";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "BillClaimSubmitHistoryPrint";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Claims/Bill_ClaimSubmitHistory_Print.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ReportsViewer_Detail";
    cmd.PanelID = "ReportsSSRSPrintViewDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ReportsViewer_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Reports/ReportsViewer_Detail.html";
    cmd.ActionPanContainer = "actionPanReportsSSRSPrintViewDetail";
    AddTab(cmd);
   
    var cmd = [];
    cmd.TabID = "MonthlyPaymentTrend_Detail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "MonthlyPaymentTrend_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Reports/FinancialKPI/MonthlyPaymentTrend_Detail.html";
    AddTab(cmd);
}

function BillingCommands() {
    var cmd = [];
    cmd.TabID = "mstrTabBilling";
    cmd.PanelID = "mstrDivBilling";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.Container = "ctrlPanBilling";
    cmd.Path = "";
    cmd.ActionPanContainer = "actionPanBilling";
    AddTab(cmd);


    //Used in Billing Charges Logs History
    var cmd = [];
    cmd.TabID = "Activity_Log";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Activity_Log";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/Activity_Log.html";
    cmd.ActionPanContainer = "actionPanActivityLog";
    AddTab(cmd)

    cmd = [];
    cmd.TabID = "Bill_ChargeSearch";
    cmd.PanelID = "pnlBillChargeSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ChargeSearch";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Charges/Bill_ChargeSearch.html";
    cmd.ActionPanContainer = "actionPanBillChargeSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_ChargeSearch_AddNote";
    cmd.PanelID = "pnlBill_ChargeSearch_AddNote";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ChargeSearch_AddNote";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Charges/Bill_ChargeSearch_AddNote.html";
    cmd.ActionPanContainer = "actionPanBill_ChargeSearch_AddNote";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_ChargeBatchSearch";
    cmd.PanelID = "pnlBillChargeBatchSearch";//panal id from opening html control
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ChargeBatchSearch";
    cmd.Selected = false;
    cmd.Container = "";// in which container it will be open
    cmd.Path = "./Controls/Billing/Charges/Bill_ChargeBatchSearch.html";
    cmd.ActionPanContainer = "actionPanBillChargeBatchSearch";//action pan of opening panal
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "chargeBatchDetail";
    cmd.PanelID = "pnlChargeBatchDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "chargeBatchDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Charges/Bill_ChargeBatch_Detail.html";
    cmd.ActionPanContainer = "actionPanChargeBatchDetail";
    cmd.ParallelActionPanContainer = "alternateActionPanChargeBatchDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ChargeBatch_Viewer";
    cmd.PanelID = "pnlChargeBatchViewer";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ChargeBatch_Viewer";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Charges/Bill_ChargeBatch_Detail_Viewer.html";
    cmd.ActionPanContainer = "actionPanChargeBatchViewer";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Encounter_Visits";
    cmd.PanelID = "pnlEncounter";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Encounter_Visits";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Encounter/Encounter_Visits.html";
    cmd.ActionPanContainer = "actionPanEncounter";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EncounterChargeCapture";
    cmd.PanelID = "pnlEncounterChargeCapture";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EncounterChargeCapture";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Encounter/Encounter_ChargeCapture.html";
    cmd.ActionPanContainer = "actionPanEncounterChargeCapture";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Encounter_NDCSelection";
    cmd.PanelID = "pnlEncounter_NDCSelection";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Encounter_NDCSelection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Encounter/Encounter_NDCSelection.html";
    cmd.ActionPanContainer = "actionPanEncounter_NDCSelection";
    AddTab(cmd);

    


    var cmd = [];
    cmd.TabID = "Encounter_CreateClaim";
    cmd.PanelID = "pnlEncounterCreateClaim";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Encounter_CreateClaim";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Encounter/Encounter_CreateClaim.html";
    cmd.ActionPanContainer = "actionPanEncounterCreateClaim";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "visitDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "visitDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Charges/Bill_Visit_Detail.html";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "chargeSearchDetail";
    cmd.PanelID = "chargeSearchDetail";
    cmd.MasterTabID = "mstrTabBilling";
    cmd.ParentTabID = "pnlBillChargeSearch";
    cmd.ContainerControlID = "chargeSearchDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Charges/Bill_Charge_Detail.html";
    cmd.ActionPanContainer = "actionPanChargeSearchDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "chargesViewDetail";
    cmd.PanelID = "chargesViewDetail";
    cmd.MasterTabID = "mstrTabBilling";
    cmd.ParentTabID = "pnlBillChargeBatchSearch";
    cmd.ContainerControlID = "chargesViewDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Charges/Bill_ChargeView.html";
    cmd.ActionPanContainer = "actionPanchargesViewDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Bill_PaymentByBatch";
    cmd.PanelID = "pnlBillPaymentByBatch";
    cmd.MasterTabID = "mstrTabBilling";
    cmd.ParentTabID = "pnlPaymentBatchDetail";
    cmd.ContainerControlID = "Bill_PaymentByBatch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_PaymentByBatch.html";
    cmd.ActionPanContainer = "actionPanBillPaymentByBatch";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Bill_InsurancePaymentByBatch";
    cmd.PanelID = "pnlBillInsurancePaymentByBatch";
    cmd.MasterTabID = "mstrTabBilling";
    cmd.ParentTabID = "pnlPaymentBatchDetail";
    cmd.ContainerControlID = "Bill_InsurancePaymentByBatch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_InsurancePaymentByBatch.html";
    cmd.ActionPanContainer = "actionPanBillInsurancePaymentByBatch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "claimViewDetail";
    cmd.PanelID = "claimViewDetail";
    cmd.MasterTabID = "mstrTabBilling";
    cmd.ParentTabID = "pnlBillChargeBatchSearch";
    cmd.ContainerControlID = "claimViewDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Claims/Bill_ClaimView.html";
    cmd.ActionPanContainer = "actionPanclaimViewDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EncounterClaimSummary";
    cmd.PanelID = "pnlEncounterClaimSummary";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EncounterClaimSummary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Encounter/Encounter_ClaimSummary.html";
    cmd.ActionPanContainer = "actionPanEncounterClaimSummary";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "paymentBatchDetail";
    cmd.PanelID = "pnlPaymentBatchDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "paymentBatchDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_PaymentBatch_Detail.html";
    cmd.ActionPanContainer = "actionPanPaymentBatchDetail";
    cmd.ParallelActionPanContainer = "alternateActionPanPaymentBatchDetail";
    AddTab(cmd);

    /***** PAYMENT BATCH SEARCH start *********/
    var cmd = [];
    cmd.TabID = "Bill_PaymentBatchSearch";
    cmd.PanelID = "pnlBillPaymentBatchSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PaymentBatchSearch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_PaymentBatchSearch.html";
    cmd.ActionPanContainer = "actionPanBillPaymentBatchSearch";
    AddTab(cmd);
    /***** PAYMENT BATCH SEARCH end *********/

    var cmd = [];
    cmd.TabID = "Bill_PaymentPosting";
    cmd.PanelID = "pnlBillPaymentPosting";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PaymentPosting";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_PaymentPosting.html";
    cmd.ActionPanContainer = "actionPanBillPaymentPosting";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Scheduling_UnallocatedCopayment_Search";
    cmd.PanelID = "pnlScheduling_UnallocatedCopayment_Search";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Scheduling_UnallocatedCopayment_Search";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_UnallocatedCopayment_Search.html";
    cmd.ActionPanContainer = "actionPanScheduling_UnallocatedCopayment_Search";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "BillLedgerDetail";
    cmd.PanelID = "pnlBillLedgerDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "BillLedgerDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_Ledger_Detail.html";
    cmd.ActionPanContainer = "actionPanBillLedgerDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_PatientResponsibilityPayment";
    cmd.PanelID = "pnlBillPatientResponsibilityPayment";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PatientResponsibilityPayment";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_PatientResponsibilityPayment.html";
    cmd.ActionPanContainer = "actionPanBillPatientResponsibilityPayment";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EDIClaimViewDetail";
    cmd.PanelID = "EDIClaimViewDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDIClaimViewDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Claims/Bill_EDIClaimView.html";
    cmd.ActionPanContainer = "actionPanEDIClaimViewDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EDIReviewReport";
    cmd.PanelID = "EDIReviewReport";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDIReviewReport";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Claims/Bill_EDIReviewReport.html";
    cmd.ActionPanContainer = "actionPanEDIReviewReport";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_ERA_ElectronicEOB";
    cmd.PanelID = "pnlElectronicEOB";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ERA_ElectronicEOB";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERA_ElectronicEOB.html";
    cmd.ActionPanContainer = "actionPanElectronicEOB";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_ClaimHcfaForm";
    cmd.PanelID = "Bill_ClaimHcfaForm";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ClaimHcfaForm";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Claims/Bill_ClaimHcfaForm.html";
    cmd.ActionPanContainer = "actionPanBill_ClaimHcfaForm";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_ClaimSubmissionErrorReport";
    cmd.PanelID = "Bill_ClaimSubmissionErrorReport";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ClaimSubmissionErrorReport";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Claims/Bill_ClaimSubmissionErrorReport.html";
    cmd.ActionPanContainer = "actionPanBill_ClaimSubmissionErrorReport";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "EDIBatchDetail";
    cmd.PanelID = "EDIBatchDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "EDIBatchDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Claims/Bill_EDIBatchDetail.html";
    cmd.ActionPanContainer = "actionPanEDIBatchDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ERADetail";
    cmd.PanelID = "ERADetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ERADetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERA_Detail.html";
    cmd.ActionPanContainer = "actionPanERADetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_ERA_Charge_Link_Wizard";
    cmd.PanelID = "pnlERAChargeLinkWizard";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ERA_Charge_Link_Wizard";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERA_Charge_Link_Wizard.html";
    cmd.ActionPanContainer = "actionPanERAChargeLinkWizard";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_ERA_Summary";
    cmd.PanelID = "pnlERASummary";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ERA_Summary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERA_Summary.html";
    cmd.ActionPanContainer = "actionPanERASummary";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "ERA_ChargeSearch";
    cmd.PanelID = "pnlBillERAChargeSearch";
    cmd.MasterTabID = "mstrTabBilling";
    //cmd.ParentTabID = "pnlBillChargeSearch";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ERA_ChargeSearch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERA_Charge.html";
    cmd.ActionPanContainer = "actionPanBillERAChargeSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_ERACharge_Detail";
    cmd.PanelID = "pnlBillERAChargeDetailSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ERACharge_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERACharge_Detail.html";
    cmd.ActionPanContainer = "actionPanBillERAChargeDetailSearch";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Bill_ERALinkedCharge_History";
    cmd.PanelID = "pnlBillERALinkedChargeHistory";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ERALinkedCharge_History";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERALinkedCharge_History.html";
    cmd.ActionPanContainer = "actionPanBillERALinkedChargeHistory";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Bill_FollowUpPatientAR_Detail";
    cmd.PanelID = "pnlBillFollowUpPatientARDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_FollowUpPatientAR_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/FollowUp/Bill_FollowUpPatientAR_Detail.html";
    cmd.ActionPanContainer = "actionPanBillFollowUpPatientARDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_FollowUpInsuranceAR_Detail";
    cmd.PanelID = "pnlBillFollowUpInsuranceARDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_FollowUpInsuranceAR_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/FollowUp/Bill_FollowUpInsuranceAR_Detail.html";
    cmd.ActionPanContainer = "actionPanBillFollowUpInsuranceARDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_FollowUpClaimSplit";
    cmd.PanelID = "pnlBillFollowUpClaimSplit";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_FollowUpClaimSplit";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/FollowUp/Bill_FollowUpClaimSplit.html";
    cmd.ActionPanContainer = "actionPanBillFollowUpClaimSplit";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_FollowUpARHistory";
    cmd.PanelID = "pnlBillFollowUpARHistory";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_FollowUpARHistory";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/FollowUp/Bill_FollowUpARHistory.html";
    cmd.ActionPanContainer = "actionPanBillFollowUpARHistory";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_FollowUpARCall";
    cmd.PanelID = "pnlBillFollowUpARCall";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_FollowUpARCall";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/FollowUp/Bill_FollowUpARCall.html";
    cmd.ActionPanContainer = "actionPanBillFollowUpARCall";
    AddTab(cmd);

    //Begin Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
    var cmd = [];
    cmd.TabID = "OutOfOfficeVisits";
    cmd.PanelID = "pnlBillOutOfOfficeVisits";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OutOfOfficeVisits";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = cmd.Path = "./EMR/HTML/Clinical/BillingInformation/OutOfOfficeVisits.html";;
    cmd.ActionPanContainer = "actionPanBillOutOfOfficeVisits";
    AddTab(cmd);
    //End Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
    var cmd = [];
    cmd.TabID = "Bill_EOBManualPosting";
    cmd.PanelID = "pnlBillEOBManualPosting";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_EOBManualPosting";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_EOBManualPosting.html";
    cmd.ActionPanContainer = "actionPanBillEOBManualPosting";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_Insurance_Payment_Detail";
    cmd.PanelID = "pnlBillInsurancePaymentDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_Insurance_Payment_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_Insurance_Payment_Detail.html";
    cmd.ActionPanContainer = "actionPanBillInsurancePaymentDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_Insurance_PaymentPosting_Detail";
    cmd.PanelID = "pnlBillInsurancePaymentPostingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_Insurance_PaymentPosting_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_InsurancePaymentPosting_Detail.html";
    cmd.ActionPanContainer = "actionPanBillInsurancePaymentPostingDetail";
    AddTab(cmd);
}

function iTrackCommands() {
    var cmd = [];
    cmd.TabID = "mstrTabiTrack";
    cmd.PanelID = "mstrDiviTrack";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_Dashboard";
    cmd.Selected = false;
    cmd.Container = "ctrlPaniTrack";
    cmd.Path = "";
    cmd.ActionPanContainer = "actionPaniTrack";
    AddTab(cmd);
  

    var cmd = [];
    cmd.TabID = "iTrack_Dashboard";
    cmd.PanelID = "pnliTrackDashboard";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_Dashboard";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_Dashboard.html";
    cmd.ActionPanContainer = "actionPaniTrackDashboard";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrackMIPSummary";
    cmd.PanelID = "pnliTrackMIPSummary";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_MIPSummary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_MIPSummary.html";
    cmd.ActionPanContainer = "actionPaniTrackMIPSummary";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_eCQMs";
    cmd.PanelID = "pnliTrackeCQMs";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_eCQMs";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_eCQMs.html";
    cmd.ActionPanContainer = "actionPaniTrack_eCQMs";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_eCQMsDetail";
    cmd.PanelID = "pnliTrackeCQMsDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_eCQMsDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_eCQMsDetail.html";
    cmd.ActionPanContainer = "actionPaniTrackeCQMsDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_Quality";
    cmd.PanelID = "pnliTrackQuality";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_Quality";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_Quality.html";
    cmd.ActionPanContainer = "actionPaniTrackQuality";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_PromotingInteroperability";
    cmd.PanelID = "pnliTrackPromotingInteroperability";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_PromotingInteroperability";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_PromotingInteroperability.html";
    cmd.ActionPanContainer = "actionPaniTrackPromotingInteroperability";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_Cost";
    cmd.PanelID = "pnliTrackCost";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_Cost";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_Cost.html";
    cmd.ActionPanContainer = "actionPaniTrackCost";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_Submission";
    cmd.PanelID = "pnliTrackSubmission";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_Submission";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_Submission.html";
    cmd.ActionPanContainer = "actionPaniTrackSubmission";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_ImprovementActivities";
    cmd.PanelID = "pnliTrackImprovementActivities";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_ImprovementActivities";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_ImprovementActivities.html";
    cmd.ActionPanContainer = "actionPaniTrackImprovementActivities";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_AdminPreferenceIndividualReportingDetail";
    cmd.PanelID = "pnlMIPSAdminPreferenceIndividualReportingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_AdminPreferenceIndividualReportingDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminPreferenceIndividualReportingDetail.html";
    cmd.ActionPanContainer = "actionPanIndividualReportingDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_MUStage3";
    cmd.PanelID = "pnlMUStage3";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_MUStage3";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_MUStage3.html";
    cmd.ActionPanContainer = "actionPanMUStage3";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_MUReport";
    cmd.PanelID = "pnliTrack_MUReport";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_MUReport";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_MUReport.html";
    cmd.ActionPanContainer = "actionPaniTrack_MUReport";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "iTrack_MIPSPrintPreview";
    cmd.PanelID = "iTrack_MIPSPrintPreview";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_MIPSPrintPreview";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_MIPSPrintPreview.html";
    cmd.ActionPanContainer = "actionPanMIPSPrintPreview";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "iTrack_QualityMeasureDetail";
    cmd.PanelID = "pnliTrackQualityMeasureDetail";
    cmd.ContainerControlID = "iTrack_QualityMeasureDetail";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPaniTrackQualityMeasureDetail";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_QualityMeasureDetail.html";
    
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "iTrack_PromotingInteroperabilityDetail";
    cmd.PanelID = "pnliTrackPromotingInteroperabilityDetail";
    cmd.ContainerControlID = "iTrack_PromotingInteroperabilityDetail";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPaniTrackPromotingInteroperabilityDetail";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_PromotingInteroperabilityDetail.html";

    AddTab(cmd);

    cmd = [];
    cmd.TabID = "iTrack_PromotingInteroperabilityDetail";
    cmd.PanelID = "pnliTrackPromotingInteroperabilityDetail";
    cmd.ContainerControlID = "iTrack_PromotingInteroperabilityDetail";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPaniTrackPromotingInteroperabilityDetail";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_PromotingInteroperabilityDetail.html";

    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_MIPSGraph";
    cmd.PanelID = "pnlMipsGraph";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_MIPSGraph";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanMipsGraph";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_MIPSGraph.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_QualityGraph";
    cmd.PanelID = "pnlQualityGraph";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_QualityGraph";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanQualityGraph";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_QualityGraph.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "iTrack_PromotingInteroperabilityGraph";
    cmd.PanelID = "pnlPromotingInteroperabilityGraph";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "iTrack_PromotingInteroperabilityGraph";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanPromotingInteroperabilityGraph";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_PromotingInteroperabilityGraph.html";
    AddTab(cmd);
}

function PatientCommands() {
    var cmd = [];
    cmd.TabID = "mstrTabPatient";
    cmd.PanelID = "mstrDivPatient";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    //cmd.ContainerControlID = "patient";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.Container = "ctrlPanPatient";
    cmd.Path = "";
    cmd.ActionPanContainer = "actionPanPatient";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Activity_Log";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Activity_Log";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    //cmd.ActionPanContainer = "";
    cmd.Path = "./Controls/CommonControls/Activity_Log.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_Audit_Log";
    cmd.PanelID = "pnlPatientAuditLog";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Audit_Log";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanPatientAuditLog";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Audit_Log.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_Reminder_Log";
    cmd.PanelID = "pnlPatientReminderLog";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Reminder_Log";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanPatientReminderLog";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Reminder_Log.html";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_Fax_Log";
    cmd.PanelID = "pnlPatientFaxLog";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Fax_Log";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanPatientFaxLog";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Fax_Log.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Document";
    cmd.PanelID = "pnlPatientDocument";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ContainerControlID = "Patient_Document";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document.html";
    cmd.ActionPanContainer = "actionPanPatientDocument";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Insurance";
    cmd.PanelID = "pnlPatientInsurance";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Insurance";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Insurance.html";
    cmd.ActionPanContainer = "actionPanPatientInsurance";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Case";
    cmd.PanelID = "pnlPatientCase";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Case";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Case/Patient_Case.html";
    cmd.ActionPanContainer = "actionPanPatientCase";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Search";
    cmd.PanelID = "Patient_Search";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Search";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Patient_Search.html";
    cmd.ActionPanContainer = "actionPanPatientSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "DemographicLabel";
    cmd.PanelID = "DemographicLabel";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "DemographicLabel";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_DemographicLabel.html";
    cmd.ActionPanContainer = "actionPanDemographicLabel";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_Provider";
    cmd.PanelID = "pnlAdminProvider";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_Provider";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Provider.html";
    cmd.ActionPanContainer = "actionPanAdminProvider";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Admin_CheckInApp";
    cmd.PanelID = "pnlAdminCheckInApp";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_CheckInApp";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_CheckInApp.html";
    cmd.ActionPanContainer = "actionPanAdminCheckInApp";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Referral";
    cmd.PanelID = "pnlPatientReferral";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Referral";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Referral.html";
    cmd.ActionPanContainer = "actionPanPatientReferral";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_Facility";
    cmd.PanelID = "pnlAdminFacility";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_Facility";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Facility.html";
    cmd.ActionPanContainer = "actionPanAdminFacility";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Admin_Drug_Code_Cost";
    cmd.PanelID = "pnlAdminDrugCodeCost";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_Drug_Code_Cost";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Durg_Code_Cost.html";
    cmd.ActionPanContainer = "actionPanAdminDrugCodeCost";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Admin_Specialty";
    cmd.PanelID = "pnlAdminSpecialty";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_Specialty";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Specialty.html";
    cmd.ActionPanContainer = "actionPanAdminSpecialty";
    AddTab(cmd);
    // PAR/NonPAR Provider Command
    cmd = [];
    cmd.TabID = "Admin_ParticipentProvider",
    cmd.PanelID = "pnlAdminParticipentProvider";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_ParticipentProvider";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "ctrlPanAdmin";
    cmd.Path = "./Controls/Admin/Admin_ParticipentProvider.html";
    cmd.ActionPanContainer = "actionPanAdminParticipentProvider";
    AddTab(cmd);
    //Par/Non Par Provider Detail  Command
    var cmd = [];
    cmd.TabID = "ParticipentProviderDetail";
    cmd.PanelID = "ParticipentProviderDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "ParticipentProviderDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ParticipentProviderDetail.html";
    cmd.ActionPanContainer = "actionPanAdminParticipentProviders";
    AddTab(cmd);
    cmd = [];
    cmd.TabID = "Admin_FaxSettingsDetail";
    cmd.PanelID = "pnlAdminFaxSettingsDetail";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "Admin_FaxSettings";
    cmd.ContainerControlID = "Admin_FaxSettingsDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_FaxSettingsDetail.html";
    cmd.ActionPanContainer = "actionPanAdminFaxSettingsDetail";
    AddTab(cmd);
    // CaseAdjuster  Command
    cmd = [];
    cmd.TabID = "Patient_CaseAdjuster",
    cmd.PanelID = "pnlCaseAdjuster";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Patient_CaseAdjuster";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "ctrlPanPatient";
    cmd.Path = "./Controls/Patient/Case/Patient_CaseAdjuster.html";
    cmd.ActionPanContainer = "actionPanCaseAdjuster";
    AddTab(cmd);
    // CaseAdjuster Detail Command
    cmd = [];
    cmd.TabID = "CaseAdjusterDetail",
    cmd.PanelID = "CaseAdjusterDetail";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "CaseAdjusterDetail";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "ctrlPanPatient";
    cmd.Path = "./Controls/Patient/Case/Patient_CaseAdjusterDetail.html";
    cmd.ActionPanContainer = "actionPanCaseAdjusterDetail";
    AddTab(cmd);
    // case Document
    cmd = [];
    cmd.TabID = "Patient_Case_Document",
    cmd.PanelID = "pnlCaseDocument";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Patient_Case_Document";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "ctrlPanPatient";
    cmd.Path = "./Controls/Patient/Case/Patient_Case_Document.html";
    cmd.ActionPanContainer = "actionPanCaseDocument";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Create_Letter";
    cmd.PanelID = "pnlCreate_Letter";
    cmd.ContainerControlID = "Create_Letter";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Create_Letter.html";
    cmd.ActionPanContainer = "actionPanCreate_Letter";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_NotesCoSign";
    cmd.PanelID = "pnlClinical_NotesCoSign";
    cmd.ContainerControlID = "Clinical_NotesCoSign";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesCoSign.html";
    cmd.ActionPanContainer = "actionPanClinical_NotesCoSign";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_NotesAmendment";
    cmd.PanelID = "pnlClinical_NotesAmendment";
    cmd.ContainerControlID = "Clinical_NotesCoSign";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_NotesAmendment.html";
    cmd.ActionPanContainer = "actionPanClinical_NotesAmendment";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "SelectLetter_Template";
    cmd.PanelID = "pnlSelectLetter_Template";
    cmd.ContainerControlID = "SelectLetter_Template";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/SelectLetter_Template.html";
    cmd.ActionPanContainer = "actionPanSelectLetter_Template";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Select_CustomForm";
    cmd.PanelID = "pnlSelectCustomForm";
    cmd.ContainerControlID = "Select_CustomForm";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Select_CustomForm.html";
    cmd.ActionPanContainer = "actionPanSelectCustomForm";
    cmd.MasterTabID = "";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_CustomForm";
    cmd.PanelID = "pnlPatientCustomForm";
    cmd.ContainerControlID = "Patient_CustomForm";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_CustomForm.html";
    cmd.ActionPanContainer = "actionPanCustomForm";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Appointments";
    cmd.PanelID = "pnlPatientAppointments";
    cmd.ContainerControlID = "Patient_Appointments";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_Appointments.html";
    cmd.ActionPanContainer = "actionPatientAppointments";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Favorite_CustomForms";
    cmd.PanelID = "pnlFavoriteCustomForms";
    cmd.ContainerControlID = "Favorite_CustomForms";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_CustomForms.html";
    cmd.ActionPanContainer = "actionPanFavoriteCustomForms";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Patient_Letter";
    cmd.PanelID = "pnlLetter";
    cmd.ContainerControlID = "Patient_Letter";
    cmd.Selected = true;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_Letter.html";
    cmd.ActionPanContainer = "actionPanLetter";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Admin_ClearingHouse";
    cmd.PanelID = "pnlAdminClearingHouse";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_ClearingHouse";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ClearingHouse.html";
    cmd.ActionPanContainer = "actionPanAdminClearingHouse";
    AddTab(cmd);

    //MK
    cmd = [];
    cmd.TabID = "Admin_EDIServiceHandle";
    cmd.PanelID = "pnlAdminEDIServiceHandle";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_EDIServiceHandle";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_EDIServiceHandle.html";
    cmd.ActionPanContainer = "actionPanAdminEDIServiceHandle";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_HL7SIU";
    cmd.PanelID = "pnlAdminHL7SIU";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_HL7SIU";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_HL7SIU.html";
    cmd.ActionPanContainer = "actionPanAdminHL7SIU";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_HL7ADT";
    cmd.PanelID = "pnlAdminHL7ADT";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_HL7ADT";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_HL7ADT.html";
    cmd.ActionPanContainer = "actionPanAdminHL7ADT";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_HL7DFT";
    cmd.PanelID = "pnlAdminHL7DFT";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_HL7DFT";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_HL7DFT.html";
    cmd.ActionPanContainer = "actionPanAdminHL7DFT";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_HL7ViewFile";
    cmd.PanelID = "pnlAdminHL7ViewFile";
    cmd.MasterTabID = "mstrTabAdmin";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_HL7ViewFile";
    cmd.Selected = false;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_HL7ViewFile.html";
    cmd.ActionPanContainer = "actionPanAdminHL7ViewFile";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_ReferringProvider";
    cmd.PanelID = "pnlAdminReferringProvider";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Admin_ReferringProvider";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ReferringProvider.html";
    cmd.ActionPanContainer = "actionPanAdminReferringProvider";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Employer";
    cmd.PanelID = "pnlEmployer";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Patient_Employer";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Employer.html";
    cmd.ActionPanContainer = "actionPanEmployer";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "employerDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "employerDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Employer_Detail.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_InsurancePlan";
    cmd.PanelID = "pnlAdminInsurancePlan";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Admin_InsurancePlan";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_InsurancePlan.html";
    cmd.ActionPanContainer = "actionPanAdminInsurancePlan";
    AddTab(cmd);



    cmd = [];
    cmd.TabID = "Admin_InsurancePlanAddress";
    cmd.PanelID = "pnlAdminInsurancePlanAddress";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Admin_InsurancePlanAddress";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_InsurancePlanAddress.html";
    cmd.ActionPanContainer = "actionPanAdminInsurancePlanAddress";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Lawyer";
    cmd.PanelID = "pnlLawyer";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Patient_Lawyer";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Lawyer.html";
    cmd.ActionPanContainer = "actionPanLawyer";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "lawyerDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "lawyerDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Lawyer_Detail.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "patientReferralSearch";
    cmd.PanelID = "patientReferralSearch";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "patientReferralSearch";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Referral_Search.html";
    cmd.ActionPanContainer = "actionPanPatientReferralSearch";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Guarantor";
    cmd.PanelID = "pnlGuarantor";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Patient_Guarantor";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Guarantor.html";
    cmd.ActionPanContainer = "actionPanGuarantor";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "guarantorDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "guarantorDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Guarantor_Detail.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_AccountManager";
    cmd.PanelID = "pnlPatientAccountManager";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_AccountManager";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/PatientPortal/Patient_AccountManager.html";
    cmd.ActionPanContainer = "actionPanPatientAccountManager";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_EmergencyContact";
    cmd.PanelID = "pnlEmergencyContact";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Patient_EmergencyContact";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_EmergencyContact.html";
    cmd.ActionPanContainer = "actionPanEmergencyContact";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Preferences";
    cmd.PanelID = "pnlPatientPreferences";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Patient_Preferences";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Preferences.html";
    cmd.ActionPanContainer = "actionPanPatientPreferences";
    AddTab(cmd);

    /*****/
    var cmd = [];
    cmd.TabID = "advancePaymentDetail";
    cmd.PanelID = "pnlPatientAdvancePayment";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "advancePaymentDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_AdvancePayment_Detail.html";
    cmd.ActionPanContainer = "actionPanPatientAdvancePayment";
    AddTab(cmd);

    //AdvancePaymentSearch
    cmd = [];
    cmd.TabID = "Patient_AdvancePayment";
    cmd.PanelID = "pnlAdvancePayment";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_AdvancePayment";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_AdvancePayment.html";
    cmd.ActionPanContainer = "actionPanAdvancePayment";
    AddTab(cmd);
    /******/

    //Start || 16 April, 2016 || ZeeshanAK || Changes for DOC 34 - Break The Glass
    cmd = [];
    cmd.TabID = "Restrict_User";
    cmd.PanelID = "pnlRestrictUser";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Restrict_User";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Restrict_User.html";
    cmd.ActionPanContainer = "actionPanRestrictUser";
    AddTab(cmd);
    //End   || 16 April, 2016 || ZeeshanAK || Changes for DOC 34 - Break The Glass


    cmd = [];
    cmd.TabID = "Patient_School";
    cmd.PanelID = "pnlSchool";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Patient_School";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_School.html";
    cmd.ActionPanContainer = "actionPanSchool";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "schoolDetail";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "schoolDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_School_Detail.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Family";
    cmd.PanelID = "pnlFamily";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabDemographic";
    cmd.ContainerControlID = "Patient_EmergencyContact";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Family.html";
    cmd.ActionPanContainer = "actionPanFamily";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_DemographicQuick";
    cmd.PanelID = "pnlDemographicQuick";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_DemographicQuick";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Demographic_Quick.html";
    cmd.ActionPanContainer = "actionPanDemographicQuick";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Demographic_PrintView";
    cmd.PanelID = "pnlPatient_Demographic_PrintView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Demographic_PrintView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Demographic_PrintView.html";
    cmd.ActionPanContainer = "actionPanPatient_Demographic_PrintView";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "demographicDetail";
    cmd.PanelID = "pnldemographicDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "demographicDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Demographic_Detail.html";
    cmd.ActionPanContainer = "actionPandemographicDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_PreAuthorization";
    cmd.PanelID = "pnlPreAuthorization";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Patient_PreAuthorization";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_PreAuthorization.html";
    cmd.ActionPanContainer = "actionPanPreAuthorization";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_CPTCode";
    cmd.PanelID = "pnlAdminCPTCode";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Admin_CPTCode";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_CPTCode.html";
    cmd.ActionPanContainer = "actionPanAdminCPTCode";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_OccupationStatus";
    cmd.PanelID = "pnlAdminOccupationStatus";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Admin_OccupationStatus";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_OccupationStatus.html";
    cmd.ActionPanContainer = "actionPanAdminOccupationStatus";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Admin_ICD";
    cmd.PanelID = "pnlAdminICD";
    //cmd.MasterTabID = "mstrTabPatient";
    //cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Admin_ICD";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_ICD.html";
    cmd.ActionPanContainer = "actionPanAdminICD";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Admin_Modifier";
    cmd.PanelID = "pnlAdminModifier";
    //cmd.MasterTabID = "mstrTabPatient";
    //cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "Admin_Modifier";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Modifier.html";
    cmd.ActionPanContainer = "actionPanAdminModifier";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_Resources";
    cmd.PanelID = "pnlAdminResources";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabSearch";
    cmd.ContainerControlID = "Admin_Resources";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Resources.html";
    cmd.ActionPanContainer = "actionPanAdminResources";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_Practice";
    cmd.PanelID = "pnlAdminPractice";
    cmd.MasterTabID = "mstrTabSchedule";
    cmd.ParentTabID = "schTabSearch";
    cmd.ContainerControlID = "Admin_Practice";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/Admin_Practice.html";
    cmd.ActionPanContainer = "actionPanAdminPractice";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_MessageAdd";
    cmd.PanelID = "pnlPatientMessageAdd";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageAdd";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Message_Add.html";
    cmd.ActionPanContainer = "actionPanPatientMessageAdd";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Referrals_Incoming_Detail";
    cmd.PanelID = "pnlPatientReferralsIncomingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Referrals_Incoming_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_Referrals_Incoming_Detail.html";
    cmd.ActionPanContainer = "actionPanPatientReferralsIncomingDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Referrals_Outgoing_Detail";
    cmd.PanelID = "pnlPatientReferralsOutgoingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Referrals_Outgoing_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_Referrals_Outgoing_Detail.html";
    cmd.ActionPanContainer = "actionPanPatientReferralsOutgoingDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_MedText_Referrals";
    cmd.PanelID = "pnlPatientMedTextReferrals";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MedText_Referrals";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_MedText_Referrals.html";
    cmd.ActionPanContainer = "actionPanPatientMedTextReferrals";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Document_Scanner";
    cmd.PanelID = "pnlDocument_Scanner";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Document_Scanner";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/Document_Scanner.html";
    cmd.ActionPanContainer = "actionPanDocument_Scanner";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "OrderSet_Patient_Referrals_Outgoing_Detail";
    cmd.PanelID = "pnlOrderSetPatientReferralsOutgoingDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "OrderSet_Patient_Referrals_Outgoing_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Templates/OrderSet/OrderSet_Patient_Referrals_Outgoing_Detail.html";
    cmd.ActionPanContainer = "actionPanOrderSetPatientReferralsOutgoingDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_MessageEdit";
    cmd.PanelID = "pnlPatientMessageEdit";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageEdit";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Message_Edit.html";
    cmd.ActionPanContainer = "actionPanPatientMessageEdit";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_MessageReply";
    cmd.PanelID = "pnlPatientMessageReply";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageReply";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Message_Reply.html";
    cmd.ActionPanContainer = "actionPanPatientMessageReply";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_MessageAlert";
    cmd.PanelID = "pnlPatientMessageAlert";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageAlert";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Message_Alert.html";
    cmd.ActionPanContainer = "actionPanPatientMessageAlert";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Message";
    cmd.PanelID = "pnlPatientMessage";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Message";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Message.html";
    cmd.ActionPanContainer = "actionPanPatientMessage";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "User_Message";
    cmd.PanelID = "pnlUserMessage";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "User_Message";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/User_Message.html";
    cmd.ActionPanContainer = "actionPanUserMessage";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "User_Task";
    cmd.PanelID = "pnlUserTask";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "User_Task";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/User_Task.html";
    cmd.ActionPanContainer = "actionPanUserTask";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Document_AssignedTo";
    cmd.PanelID = "pnlDocumentAssignedTo";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Document_AssignedTo";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_AssignedTo.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Document_Import";
    cmd.PanelID = "pnlDocumentImport";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Document_Import";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanDocumentImport";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_Import.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Document_Image_Annotation";
    cmd.PanelID = "pnlImageAnnotation";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Document_Image_Annotation";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.ActionPanContainer = "actionPanImageAnnotation";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_Image_Annotation.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Document_View";
    cmd.PanelID = "pnlDocumentView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Document_AssignedTo";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_AssignedTo.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Document_Export";
    cmd.PanelID = "pnlDocumentExport";
    cmd.ActionPanContainer = "actionPanDocumentExport";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Document_Export";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_Export.html";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Document_Viewer";
    cmd.PanelID = "pnlDocumentViewer";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Document_Viewer";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_Viewer.html";
    cmd.ActionPanContainer = "actionPanDocumentViewer";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Document_Scan";
    cmd.PanelID = "pnlDocumentScan";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Document_Scan";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_Scan.html";
    cmd.ActionPanContainer = "actionPanDocument_Scan";
    AddTab(cmd);

      cmd = [];
    cmd.TabID = "Patient_Document_Search";
    cmd.PanelID = "pnlPatientDocument_Search";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Document_Search";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_Search.html";
    cmd.ActionPanContainer = "actionPanPatientDocument_Search";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Patient_Information_Submission";
    cmd.PanelID = "pnlPatientInformationSubmission";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Information_Submission";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Information_Submission.html";
    cmd.ActionPanContainer = "actionPanPatientInformationSubmission";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Patient_Information_Delete";
    cmd.PanelID = "pnlPatientInformationDelete";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Information_Delete";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Information_Delete.html";
    cmd.ActionPanContainer = "actionPanPatientInformationDelete";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Patient_Information_Import";
    cmd.PanelID = "pnlPatientInformationImport";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Information_Import";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Information_Import.html";
    cmd.ActionPanContainer = "actionPanPatientInformationImport";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Patient_Information_Viewer";
    cmd.PanelID = "pnlPatientInformationViewer";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Information_Viewer";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Information_Viewer.html";
    cmd.ActionPanContainer = "actionPanPatientInformationViewer";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "emergencyContactDetail";
    cmd.PanelID = "emergencyContactDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "emergencyContactDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_EmergencyContact_Detail.html";
    cmd.ActionPanContainer = "actionPanEmergencyContractDetail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_Family_Detail";
    cmd.PanelID = "pnlFamilyDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Family_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Demographics/Patient_Family_Detail.html";
    cmd.ActionPanContainer = "actionPanPatient_Family_Detail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "appointmentSummary";
    cmd.PanelID = "pnlAppointmentSummary";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "appointmentSummary";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduling/Scheduling_Appointment_Summary.html";
    cmd.ActionPanContainer = "actionPanAppointmentSummary";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "Activity_LogNotes";
    cmd.PanelID = "pnlLogNotes";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Activity_LogNotes";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/Activity_LogNotes.html";
    cmd.ActionPanContainer = "actionPanLogNotes";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Clinical_Note_Components_Audit";
    cmd.PanelID = "pnlNote_Components_Audit";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_Note_Components_Audit";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_Note_Components_Audit.html";
    cmd.ActionPanContainer = "actionPanNote_Components_Audit";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_Documents_Audit";
    cmd.PanelID = "pnlPatient_Documents_Audit";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Documents_Audit";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Documents_Audit.html";
    cmd.ActionPanContainer = "actionPanPatient_Documents_Audit";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "Patient_PreAuthorization_Detail";
    cmd.PanelID = "pnlPreAuthorizationDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_PreAuthorization_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_PreAuthorization_Detail.html";
    cmd.ActionPanContainer = "actionPanPreAuthorization_Detail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Case_Detail";
    cmd.PanelID = "pnlPatientCaseDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Case_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Case/Patient_Case_Detail.html";
    cmd.ActionPanContainer = "actionPanPatientCaseDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "OCR_Scanner";
    cmd.PanelID = "pnlOCRScannerViewer";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "patTabInsurance";
    cmd.ContainerControlID = "OCR_Scanner";
    cmd.Selected = false;
    //cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/OCR_Scanner.html";
    cmd.ActionPanContainer = "actionPanOCRScannerViewer";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Clinical_ImmunizationDetail";
    cmd.PanelID = "pnlClinicalImmunizationDetail";//"pnlClinicalQuestionGroupDetail
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ImmunizationDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/Medical/Clinical_ImmunizationDetail.html";
    cmd.ActionPanContainer = "actionPanClinicalImmunizationDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_Implantable";
    cmd.PanelID = "pnlClinicalImplantable";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_Implantable";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/Medical/Clinical_Implantable.html";
    cmd.ActionPanContainer = "actionPanClinicalImplantable";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_ImplantableDetail";
    cmd.PanelID = "pnlClinicalImplantableDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_ImplantableDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/Medical/Clinical_ImplantableDetail.html";
    cmd.ActionPanContainer = "actionPanClinicalImplantableDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_TherapeuticInjection";
    cmd.PanelID = "pnlClinicalTherapeuticInjection";//"pnlClinicalQuestionGroupDetail
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_TherapeuticInjection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/Immunization/Immunization_TherapeuticInjection.html";
    cmd.ActionPanContainer = "actionPanClinicalTherapeuticInjection";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_SchedulerView";
    cmd.PanelID = "pnlClinical_SchedulerView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_SchedulerView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_SchedulerView.html";
    cmd.ActionPanContainer = "actionPanClinical_SchedulerView";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Immunization_LotTypeSelection";
    cmd.PanelID = "pnlImmunizationLotTypeSelection";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_LotTypeSelection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_LotTypeSelection.html";
    cmd.ActionPanContainer = "actionPanImmunizationLotTypeSelection";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_VaccineTypeSelection";
    cmd.PanelID = "pnlImmunizationVaccineTypeSelection";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_VaccineTypeSelection";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_VaccineTypeSelection.html";
    cmd.ActionPanContainer = "actionPanImmunizationVaccineTypeSelection";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Immunization_ImmunizationAddImmInj";
    cmd.PanelID = "pnlImmunization_ImmunizationAddImmInj";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_ImmunizationAddImmInj";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_ImmunizationAddImmInj.html";
    cmd.ActionPanContainer = "actionPanImmunization_ImmunizationAddImmInj";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_Manufacturer";
    cmd.PanelID = "pnlImmunization_Manufacturer";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_Manufacturer";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_Manufacturer.html";
    cmd.ActionPanContainer = "actionPanImmunization_Manufacturer";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_ManufacturerDetail";
    cmd.PanelID = "pnlImmunization_ManufacturerDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_ManufacturerDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_ManufacturerDetail.html";
    cmd.ActionPanContainer = "actionPanImmunization_ManufacturerDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_Registery";
    cmd.PanelID = "pnlImmunization_Registery";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_Registery";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_Registery.html";
    cmd.ActionPanContainer = "actionPanImmunization_Registery";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_RegisteryDetail";
    cmd.PanelID = "pnlImmunization_RegisteryrDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_RegisteryDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_RegisteryDetail.html";
    cmd.ActionPanContainer = "actionPanImmunization_RegisteryDetail";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Immunization_AddVaccine";
    cmd.PanelID = "pnlImmunization_AddVaccine";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_AddVaccine";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_AddVaccine.html";
    cmd.ActionPanContainer = "actionPanImmunization_AddVaccine";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_TherapeuticDetail";
    cmd.PanelID = "pnlImmunization_TherapeuticDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_TherapeuticDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_TherapeuticDetail.html";
    cmd.ActionPanContainer = "actionPanImmunization_TherapeuticDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_LotNumberDetail";
    cmd.PanelID = "pnlImmunization_LotNumberDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_LotNumberDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_LotNumberDetail.html";
    cmd.ActionPanContainer = "actionPanImmunization_LotNumberDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_SearchVaccine";
    cmd.PanelID = "pnlImmunization_SearchVaccine";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_SearchVaccine";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_SearchVaccine.html";
    cmd.ActionPanContainer = "actionPanImmunization_SearchVaccine";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_LotNumber";
    cmd.PanelID = "pnlImmunization_LotNumber";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_LotNumber";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_LotNumber.html";
    cmd.ActionPanContainer = "actionPanImmunization_LotNumber";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_Category";
    cmd.PanelID = "pnlImmunization_Category";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_Category";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_Category.html";
    cmd.ActionPanContainer = "actionPanImmunization_Category";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_AlertPreview";
    cmd.PanelID = "pnlImmunization_AlertPreview";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_AlertPreview";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_AlertPreview.html";
    cmd.ActionPanContainer = "actionPanImmunization_AlertPreview";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_AlertPrint";
    cmd.PanelID = "pnlIImmunization_AlertPrint";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_AlertPrint";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_AlertPrint.html";
    cmd.ActionPanContainer = "actionPanImmunization_AlertPrint";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Clinical_Immunization_Preview";
    cmd.PanelID = "pnlClinical_Immunization_Preview";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_Immunization_Preview";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Clinical_Immunization_Preview.html";
    cmd.ActionPanContainer = "actionPanClinical_Immunization_Preview";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_CatchupScheduler";
    cmd.PanelID = "pnlImmunization_CatchupScheduler";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_CatchupScheduler";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_CatchupScheduler.html";
    cmd.ActionPanContainer = "actionPanImmunization_CatchupScheduler";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_VaccineCrosswalk";
    cmd.PanelID = "pnlImmunization_VaccineCrosswalk";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_VaccineCrosswalk";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_VaccineCrosswalk.html";
    cmd.ActionPanContainer = "actionPanImmunization_VaccineCrosswalk";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_ScheduleSetup";
    cmd.PanelID = "pnlImmunization_ScheduleSetup";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_ScheduleSetup";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_ScheduleSetup.html";
    cmd.ActionPanContainer = "actionPanImmunization_ScheduleSetup";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Immunization_QueryResponseDetail";
    cmd.PanelID = "pnlImmunization_QueryResponseDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Immunization_QueryResponseDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_QueryResponseDetail.html";
    cmd.ActionPanContainer = "actionPanImmunization_QueryResponseDetail";
    AddTab(cmd);

    
    cmd = [];
    cmd.TabID = "Clinical_Immunization";
    cmd.PanelID = "pnlClinicalImmunization";//"pnlClinicalQuestionGroupDetail
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Clinical_Immunization";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./emr/html/Clinical/Medical/Clinical_Immunization.html";
    cmd.ActionPanContainer = "actionPanClinicalImmunization";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Referrals";
    cmd.PanelID = "pnlPatientReferrals";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Referrals";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_Referrals.html";
    cmd.ActionPanContainer = "actionPanPatientReferrals";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_ReferralsView";
    cmd.PanelID = "pnlPatient_ReferralView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_ReferralsView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Patient/Patient_ReferralView.html";
    cmd.ActionPanContainer = "actionPanPatient_ReferralView";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_Eligibility";
    cmd.PanelID = "pnlPatientEligibility";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Eligibility";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Eligibility.html";
    cmd.ActionPanContainer = "actionPanPatientEligibility";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_Eligibility_Detail";
    cmd.PanelID = "pnlPatientEligibilityDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Eligibility_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Insurance/Patient_Eligibility_Detail.html";
    cmd.ActionPanContainer = "actionPanPatientEligibilityDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Outstanding_Visit";
    cmd.PanelID = "pnlOutstandingVisit";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Outstanding_Visit";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/Outstanding_Visit.html";
    cmd.ActionPanContainer = "actionPanOutstandingVisit";
    AddTab(cmd);

    //PATIENT LEDGER
    cmd = [];
    cmd.TabID = "Patient_Ledger";
    cmd.PanelID = "pnlPatientLedger";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_Ledger";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/Patient_Ledger.html";
    cmd.ActionPanContainer = "actionPanPatientLedger";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_PatientPayments";
    cmd.PanelID = "pnlPatientPayments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PatientPayments";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_PatientPayments.html";
    cmd.ActionPanContainer = "actionPanPatientPayments";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_PatientPaymentsPrint";
    cmd.PanelID = "pnlPreviewPatientPayments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_PatientPaymentsPrint";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_PatientPaymentsPrint.html";
    cmd.ActionPanContainer = "actionPanPreviewPatientPayments";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Bill_ReceivedPatientPayments";
    cmd.PanelID = "pnlReceivedPatientPayments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ReceivedPatientPayments";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/Payments/Bill_ReceivedPatientPayments.html";
    cmd.ActionPanContainer = "actionPanReceivedPatientPayments";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_ERA_277CA";
    cmd.PanelID = "pnlReport_ERA_277CA";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Bill_ERA_277CA";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/ERA/Bill_ERA_277CA.html";
    cmd.ActionPanContainer = "actionPanReport_ERA_277CA";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "Patient_AccountManager_Detail";
    cmd.PanelID = "Patient_AccountManager_Detail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_AccountManager_Detail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/PatientPortal/Patient_AccountManager_Detail.html";
    cmd.ActionPanContainer = "actionPanPatient_AccountManager_Detail";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_MessageCompose";
    cmd.PanelID = "pnlPatientMessageCompose";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageCompose";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Message_Compose.html";
    cmd.ActionPanContainer = "actionPanPatientMessageCompose";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_MessageCreate";
    cmd.PanelID = "pnlPatientMessageCreate";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageCreate";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Message_Create.html";
    cmd.ActionPanContainer = "actionPanPatientMessageCreate";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Direct_MessageCreate";
    cmd.PanelID = "pnlDirectMessageCreate";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Direct_MessageCreate";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Direct_MessageCreate.html";
    cmd.ActionPanContainer = "actionPanDirectMessageCreate";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_MessageXmlView";
    cmd.PanelID = "pnlPatientMessageXmlView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageXmlView";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_MessageXmlView.html";
    cmd.ActionPanContainer = "actionPanPatientMessageXmlView";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_MessageKey";
    cmd.PanelID = "pnlPatientMessageKey";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_MessageKey";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Messagekey.html";
    cmd.ActionPanContainer = "actionPanPatientMessageKey";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_UserMessagesQuickLink";
    cmd.PanelID = "pnlPatientUserMessagesQuickLink";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_UserMessagesQuickLink";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_UserMessagesQuickLink.html";
    cmd.ActionPanContainer = "actionPanPatientUserMessagesQuickLink";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "MU_Alerts";
    cmd.PanelID = "pnlMUAlerts";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "MU_Alerts";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/MU_Alerts.html";
    cmd.ActionPanContainer = "actionPanMUAlerts";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Patient_TaskDetail";
    cmd.PanelID = "pnlPatientTaskDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_TaskDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Messages/Patient_Task_Detail.html";
    cmd.ActionPanContainer = "actionPanPatientTaskDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Patient_Document_Note";
    cmd.PanelID = "pnlDocumentNote";
    cmd.MasterTabID = "mstrTabPatient";
    cmd.ContainerControlID = "Patient_Document_Note";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_Document_Note.html";
    cmd.ActionPanContainer = "actionPanDocumentNote";
    AddTab(cmd);
}
function DashBoardCommands() {
    var cmd = [];
    //Top Level Tabs
    cmd.TabID = "mstrTabDashBoard";
    cmd.PanelID = "pnlDashboard";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "DashBoard";
    cmd.Selected = true;
    cmd.Container = "ctrlPanDashBoard";
    cmd.Path = "./Controls/DashBoard/DashBoard.html";
    cmd.ActionPanContainer = "actionPanDashboard";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "DashBoardSetting";
    cmd.PanelID = "pnldashboardsetting";
    cmd.MasterTabID = "mstrTabDashBoard";
    cmd.ParentTabID = "mstrTabDashBoard";
    cmd.ContainerControlID = "DashBoardSetting";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/DashBoard/DashBoardSetting.html";
    cmd.ActionPanContainer = "actionPanDashboardboardsetting";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "DashBoardChangePwd";
    cmd.PanelID = "pnldashboardchangepass";
    cmd.MasterTabID = "mstrTabDashBoard";
    cmd.ParentTabID = "mstrTabDashBoard";
    cmd.ContainerControlID = "DashBoardChangePwd";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/DashBoard/DashBoardChangePass.html";
    cmd.ActionPanContainer = "actionPanDashboardboardchangepass";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "UserReLogin";
    cmd.PanelID = "pnlUserReLogin";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/User_ReLogin.html";
    cmd.ActionPanContainer = "";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "BillingInformation";
    cmd.PanelID = "pnlBillingInformation";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/BillingInformation/BillingInformation.html";
    cmd.ActionPanContainer = "actionPanBillingInformation";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ENMCodeSuggest";
    cmd.PanelID = "pnlENMCodeSuggest";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/BillingInformation/ENMCodeSuggest.html";
    cmd.ActionPanContainer = "actionPanENMCodeSuggest";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "VisitTypeSelection";
    cmd.PanelID = "pnlVisitTypeSelection";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/BillingInformation/VisitTypeSelection.html";
    cmd.ActionPanContainer = "actionPanVisitTypeSelection";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "ClinicalBarCodeView";
    cmd.PanelID = "pnlClinicalBarCodeView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clincal_BarCodeView.html";
    cmd.ActionPanContainer = "actionPanClinicalBarCodeView";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "PMSScheduler_AppointmentCancellation";
    cmd.PanelID = "pnlPMSScheduler_AppointmentCancellation";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Scheduler/PMSScheduler_AppointmentCancellation.html";
    cmd.ActionPanContainer = "actionPanPMSScheduler_AppointmentCancellation";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "Bill_FollowUpARComments";
    cmd.PanelID = "pnlFollowUpARComments";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Billing/FollowUp/Bill_FollowUpARComments.html";
    cmd.ActionPanContainer = "actionPanFollowUpARComments";
    AddTab(cmd);


    var cmd = [];
    cmd.TabID = "ClincalFavGroupBarCodeView";
    cmd.PanelID = "pnlClincalFavGroupBarCodeView";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/HTML/Clinical/Orders/Clincal_FavGroupBarCodeView.html";
    cmd.ActionPanContainer = "actionPanClinicalFavGroupBarCodeView";
    AddTab(cmd);

    var cmd = [];
    cmd = [];
    cmd.TabID = "MobileAppRequest";
    cmd.PanelID = "pnlMobileAppRequest";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "MobileAppRequest";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/LiveRequests/MobileAppRequests/MobileAppRequest.html";
    cmd.ActionPanContainer = "actionPanMobileAppRequest";
    AddTab(cmd);

}
function CCMCommands() {
    cmd = [];
    cmd.TabID = "CCM_Patient_Hub";
    cmd.PanelID = "pnlCCM_Patient_Hub";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCM_Patient_Hub";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCM_Hub.html";
    cmd.ActionPanContainer = "actionPanCCM_Patient_Hub";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCMEnrollmentInfo";
    cmd.PanelID = "pnlCCMEnrollmentInfo";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMEnrollmentInfo";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMEnrollmentInfo.html";
    cmd.ActionPanContainer = "actionPanCCMEnrollmentInfo";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCMEnrollmentDecline";
    cmd.PanelID = "pnlCCMEnrollmentDecline";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMEnrollmentDecline";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMEnrollmentDecline.html";
    cmd.ActionPanContainer = "actionPanCCMEnrollmentDecline";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCMAgreement";
    cmd.PanelID = "pnlCCMAgreement";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMAgreement";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMAgreement.html";
    cmd.ActionPanContainer = "actionPanCCMAgreement";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCMConsent";
    cmd.PanelID = "CCMConsent";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMConsent";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMConsent.html";
    cmd.ActionPanContainer = "actionPanCCMConsent";
    AddTab(cmd);

    //Program Update section (start)
    cmd = [];
    cmd.TabID = "CCMProgramUpdate";
    cmd.PanelID = "pnlCCMProgramUpdate";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMProgramUpdate";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMProgramUpdate.html";
    cmd.ActionPanContainer = "actionPanCCMProgramUpdate";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCMTaskTimerDetail";
    cmd.PanelID = "pnlCCMTaskTimerDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMTaskTimerDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMTaskTimerDetail.html";
    cmd.ActionPanContainer = "actionPanCCMTaskTimerDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCMTaskTimerHistory";
    cmd.PanelID = "pnlCCMTaskTimerHistory";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMTaskTimerHistory";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMTaskTimerHistory.html";
    cmd.ActionPanContainer = "actionPanCCMTaskTimerHistory";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCMCallDetailsHistory";
    cmd.PanelID = "pnlCCMCallDetailsHistory";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMCallDetailsHistory";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMCallDetailsHistory.html";
    cmd.ActionPanContainer = "actionPanCCMCallDetailsHistory";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "CCMProgressUpdateHistory";
    cmd.PanelID = "pnlCCMProgressUpdateHistory";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCMProgressUpdateHistory";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCMProgressUpdateHistory.html";
    cmd.ActionPanContainer = "actionPanCCMProgressUpdateHistory";
    AddTab(cmd);

    //Program Update section (end)

    cmd = [];
    cmd.TabID = "CCM_CarePlanDetail";
    cmd.PanelID = "CCM_CarePlanDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCM_CarePlanDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCM_CarePlanDetail.html";
    cmd.ActionPanContainer = "actionPanCCM_CarePlanDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCM_HRAssessmentDetail";
    cmd.PanelID = "CCM_HRAssessmentDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCM_HRAssessmentDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCM_HRAssessmentDetail.html";
    cmd.ActionPanContainer = "actionPanCCM_HRAssessmentDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "Admin_CCMCareTeamDetail";
    cmd.PanelID = "pnlAdmin_CCMCareTeamDetail";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Admin_CCMCareTeamDetail";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Admin/CCM/Admin_CCMCareTeamDetail.html";
    cmd.ActionPanContainer = "actionPanAdmin_CCMCareTeamDetail";
    AddTab(cmd);

    cmd = [];
    cmd.TabID = "CCM_AddNewAssessment";
    cmd.PanelID = "pnlAddNewAssessment";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "CCM_AddNewAssessment";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./CCM/HTML/CCM_AddNewAssessment.html";
    cmd.ActionPanContainer = "actionPanAddNewAssessment";
    AddTab(cmd);


    cmd = [];
    cmd.TabID = "Patient_DocumentTag";
    cmd.PanelID = "pnlPatientDocumentsTag";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "Patient_DocumentTag";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/Patient/Document/Patient_DocumentTag.html";
    cmd.ActionPanContainer = "actionPatientDocumentsTag";
    AddTab(cmd);

}

function CommonCommands() {


    cmd = [];
    cmd.TabID = "uploadImage";
    cmd.PanelID = "";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "uploadImage";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/Upload_Image.html";
    cmd.ActionPanContainer = "";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "HelpScreen";
    cmd.PanelID = "pnlHelpScreen";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/Help_Screen.html";
    cmd.ActionPanContainer = "";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "AboutScreen";
    cmd.PanelID = "pnlAboutScreen";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/About_Screen.html";
    cmd.ActionPanContainer = "";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "OrthopedicComplaints";
    cmd.PanelID = "pnlOrthopedicComplaints";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./Controls/CommonControls/OrthopedicComplaints.html";
    cmd.ActionPanContainer = "";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "IA_DiabetesScreening";
    cmd.PanelID = "pnlDiabtesScreening";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "IA_DiabetesScreening";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/AppScripts/Clinical/PQRSAdmin/IA_DiabetesScreening.html";
    cmd.ActionPanContainer = "actionPanDiabtesScreening";
    AddTab(cmd);

    var cmd = [];
    cmd.TabID = "IA_TabacooScreening";
    cmd.PanelID = "pnlTabacooScreening";
    cmd.MasterTabID = "";
    cmd.ContainerControlID = "IA_TabacooScreening";
    cmd.Selected = false;
    cmd.isActionPan = true;
    cmd.Container = "";
    cmd.Path = "./EMR/AppScripts/Clinical/PQRSAdmin/IATabacooScreening.html";
    cmd.ActionPanContainer = "actionPanTabacooScreening";
    AddTab(cmd);

}
function isAdminTab(Tab) {
    if (Tab.MasterTabID == 'mstrTabAdmin' || Tab.TabID == 'mstrTabAdmin') {
        return true;
    }
    return false;
}

function isPatientTab(Tab) {
    if (Tab.MasterTabID == 'mstrTabPatient' || Tab.TabID == 'mstrTabPatient') {
        return true;
    }
    return false;
}

function AddTab(cmd) {
    var Tab = new Object();
    Tab["Selected"] = cmd.Selected;
    Tab["TabID"] = cmd.TabID;
    Tab["MasterTabID"] = cmd.MasterTabID;
    Tab["ParentTabID"] = cmd.ParentTabID;
    Tab["ParentChildTabID"] = cmd.ParentChildTabID;
    Tab["PanelID"] = cmd.PanelID;
    Tab["Active"] = false;
    Tab["ContainerControlID"] = cmd.ContainerControlID;
    Tab["Container"] = cmd.Container;
    Tab["Path"] = cmd.Path;
    Tab["isActionPan"] = cmd.isActionPan;
    Tab["defaultSelectedTabID"] = cmd.defaultSelectedTabID;
    Tab["ActionPanContainer"] = cmd.ActionPanContainer;
    Tab["ParallelActionPanContainer"] = cmd.ParallelActionPanContainer;
    TabsArray.push(Tab);
}
function AduitableEventsCommands() {
    var cmd = [];
    cmd.TabID = "mstrTabAuditbleEvents";
    cmd.PanelID = "mstrDivAuditbleEvents";
    cmd.MasterTabID = "";
    cmd.ParentTabID = "";
    cmd.ContainerControlID = "AuditbleEvents_ActivityLog";
    cmd.Selected = true;
    cmd.Container = "ctrlPanAuditbleEvents";
    cmd.Path = "./EMR/HTML/AuditbleEvents/AuditbleEvents_ActivityLog.html";
    cmd.ActionPanContainer = "actionPanAuditbleEvents";
    AddTab(cmd);
}
function RemoveTab(cmd) {
    var newArray = TabsArray.filter(function (TabIndex) {
        if (cmd.TabID != TabIndex.TabID) {
            return TabIndex;
        }
    });

    TabsArray = newArray;
    store.saveSession('TabsArray', TabsArray);
}

function RemoveTabContainerControlID(cmd) {
    var newArray = TabsArray.filter(function (TabIndex) {
        if (cmd.ContainerControlID == TabIndex.ContainerControlID) {
            TabIndex.ContainerControlID = "";
            return TabIndex;
        }
    });
}

