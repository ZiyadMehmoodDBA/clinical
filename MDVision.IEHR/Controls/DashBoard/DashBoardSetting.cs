using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Web;

namespace MDVision.IEHR.Controls.DashBoard
{
    public class DashBoardSetting
    {
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        public DashBoardSetting()
        {

            BLLAdminSecurityObj = new BLLAdminSecurity();
        }
        #region Singleton
        private static DashBoardSetting _obj = null;
        public static DashBoardSetting Instance()
        {
            if (_obj == null)
                _obj = new DashBoardSetting();
            return _obj;
        }
        #endregion

        private string UpdateDashboardSetting(int DBSId, string IsActive)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                if (DBSId > 0)
                {

                    DSSettings dsDashboardsett = new DSSettings();
                    DSSettings.DashboardSettingsRow dr = dsDashboardsett.DashboardSettings.NewDashboardSettingsRow();

                    dr.DBSId = DBSId;

                    if (IsActive == "1")
                        IsActive = "True";
                    dr.IsActive = MDVUtility.ToStr(IsActive) == "True" ? true : false;

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    #region Database Updation
                    dsDashboardsett.DashboardSettings.AddDashboardSettingsRow(dr);
                    dsDashboardsett.DashboardSettings.AcceptChanges();

                    if (dsDashboardsett.Tables[dsDashboardsett.DashboardSettings.TableName].Rows.Count > 0)
                    {
                        dsDashboardsett.DashboardSettings.Rows[0].SetModified();
                        BLObject<DSSettings> obj = BLLAdminSecurityObj.UpdateDashboardSetting(dsDashboardsett);
                        if (obj.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {

                            var response = new
                            {
                                status = false,
                                Message = obj.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Dashboard Settings not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdateAllDashboardSetting(string data)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                dynamic items = JsonConvert.DeserializeObject<List<object>>(data);
                BLObject<DSSettings> objLoadSetting = BLLAdminSecurityObj.LoadDashBoardSetting(0, MDVUtility.ToLong(MDVSession.Current.AppUserId), "");
                DSSettings ds;
                ds = objLoadSetting.Data;
                foreach (dynamic item in items)
                {
                    DSSettings.DashboardSettingsRow drSetting = ds.Tables[ds.DashboardSettings.TableName].Rows.Find(MDVUtility.ToInt32(item.DBSId));
                    if (Convert.ToBoolean(MDVUtility.ToInt32(item.isActive)) == false)
                        drSetting[ds.DashboardSettings.IsDefaultColumn.ColumnName] = false;

                    drSetting[ds.DashboardSettings.IsActiveColumn.ColumnName] = Convert.ToBoolean(MDVUtility.ToInt32(item.isActive));
                    drSetting[ds.DashboardSettings.ModifiedByColumn.ColumnName] = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    drSetting[ds.DashboardSettings.ModifiedOnColumn.ColumnName] = DateTime.Now;
                }

                #region Database Updation

                if (ds.Tables[ds.DashboardSettings.TableName].Rows.Count > 0)
                {
                    BLObject<DSSettings> obj = BLLAdminSecurityObj.UpdateDashboardSetting(ds);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {

                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string GetEntityUserOptionShowICD10(string UserName)
        {
            bool result;
            result = BLLAdminSecurityObj.LoadEntityUserOptionShowICD10(UserName, MDVSession.Current.EntityId);
            var keyValues = new Dictionary<string, string> {
                 { "hfIsShowICD10", MDVUtility.ToStr(result)},

            };
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            js.MaxJsonLength = Int32.MaxValue;
            var response = new
            {
                status = true,

                ISShowICD10_JSON = js.Serialize(keyValues),
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }
        private string FillDefaultSetting(string userName)
        {
            try
            {

                DSUsers dsUser = new DSUsers();
                DSUsers dsUser1 = new DSUsers();
                BLObject<DSUsers> obj = null;
                string WorkWeekDays = string.Empty;
                string WorkWeekDaysIDs = string.Empty;
                obj = BLLAdminSecurityObj.LoadEntityUserOption(ref dsUser1, userName, MDVSession.Current.EntityId);
                dsUser = obj.Data;
                if (obj.Data != null)
                {
                    if (dsUser.Tables[dsUser.EntityUserOption.TableName].Rows.Count > 0)
                    {

                        DataRow dr = dsUser.Tables[dsUser.EntityUserOption.TableName].Rows[0];
                        WorkWeekDays = MDVUtility.ToStr(dr[dsUser.EntityUserOption.WorkWeekDaysColumn.ColumnName]);

                        if (!string.IsNullOrEmpty(WorkWeekDays))
                        {
                            if (WorkWeekDays.Split(',')[0].Equals("1")) { WorkWeekDaysIDs = WorkWeekDaysIDs + "1,"; } else { WorkWeekDaysIDs = WorkWeekDaysIDs + "0,"; }
                            if (WorkWeekDays.Split(',')[1].Equals("1")) { WorkWeekDaysIDs = WorkWeekDaysIDs + "2,"; } else { WorkWeekDaysIDs = WorkWeekDaysIDs + "0,"; }
                            if (WorkWeekDays.Split(',')[2].Equals("1")) { WorkWeekDaysIDs = WorkWeekDaysIDs + "3,"; } else { WorkWeekDaysIDs = WorkWeekDaysIDs + "0,"; }
                            if (WorkWeekDays.Split(',')[3].Equals("1")) { WorkWeekDaysIDs = WorkWeekDaysIDs + "4,"; } else { WorkWeekDaysIDs = WorkWeekDaysIDs + "0,"; }
                            if (WorkWeekDays.Split(',')[4].Equals("1")) { WorkWeekDaysIDs = WorkWeekDaysIDs + "5,"; } else { WorkWeekDaysIDs = WorkWeekDaysIDs + "0,"; }
                            if (WorkWeekDays.Split(',')[5].Equals("1")) { WorkWeekDaysIDs = WorkWeekDaysIDs + "6,"; } else { WorkWeekDaysIDs = WorkWeekDaysIDs + "0,"; }
                            if (WorkWeekDays.Split(',')[6].Equals("1")) { WorkWeekDaysIDs = WorkWeekDaysIDs + "7,"; } else { WorkWeekDaysIDs = WorkWeekDaysIDs + "0,"; }
                        }
                        else { WorkWeekDaysIDs = "0,0,0,0,0,0,0"; }

                        var keyValues = new Dictionary<string, string>
                        {
                            { "ddlPreferredScreenAfterLogin", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PreferredScreenColumn.ColumnName])},
                            { "ddlDefaultProvider", MDVUtility.ToStr(dr[dsUser.EntityUserOption.ProviderIdColumn.ColumnName])},
                            { "ddlDefaultBillingProvider", MDVUtility.ToStr(dr[dsUser.EntityUserOption.BillingProviderIdColumn.ColumnName])},
                            { "ddlPreferredSchedulerScreen", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PreferredSchScreenColumn.ColumnName])},
                            { "PreferredSchedulerScreen", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PreferredSchScreenNameColumn.ColumnName])},
                            { "ddlDefaultFacility", MDVUtility.ToStr(dr[dsUser.EntityUserOption.FacilityIdColumn.ColumnName])},
                            { "ddlDefaultTemplate", MDVUtility.ToStr(dr[dsUser.EntityUserOption.DefaultTemplateColumn.ColumnName])},
                            { "ddlDefaultResource", MDVUtility.ToStr(dr[dsUser.EntityUserOption.ResourceIdColumn.ColumnName])},
                            { "ddlDefaultSuperBill", MDVUtility.ToStr(dr[dsUser.EntityUserOption.DefaultSuperBillColumn.ColumnName])},
                            //{ "ddlPhoneNumber1", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPhoneNumber1Column.ColumnName]) == "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPhoneNumber1Column.ColumnName]) == "False"?"0": ""},
                            //{ "ddlPhoneNumber2", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPhoneNumber2Column.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPhoneNumber2Column.ColumnName]) == "False"?"0": ""},
                            { "ddlPCP", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPCPColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPCPColumn.ColumnName]) == "False"?"0": ""},
                            { "ddlReferringProvider", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBRefProviderColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBRefProviderColumn.ColumnName]) == "False"?"0": ""},
                            { "ddlPlanBalance", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPlanBalanceColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPlanBalanceColumn.ColumnName]) == "False"?"0": ""},
                            { "ddlPatientBalance", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPatientBalanceColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPatientBalanceColumn.ColumnName]) == "False"?"0": ""},
                            { "ddlclaimprinter", MDVUtility.ToStr(dr[dsUser.EntityUserOption.ClaimPrinterColumn.ColumnName])},
                            { "ddlclaimtray", MDVUtility.ToStr(dr[dsUser.EntityUserOption.ClaimTrayColumn.ColumnName])},
                            { "ddattachmentprinter", MDVUtility.ToStr(dr[dsUser.EntityUserOption.AttachmentPrinterColumn.ColumnName])},
                            { "ddlattachmenttray", MDVUtility.ToStr(dr[dsUser.EntityUserOption.AttachmentTrayColumn.ColumnName])},
                            { "txtRefreshTime", MDVUtility.ToStr(dr[dsUser.EntityUserOption.RefreshTimeColumn.ColumnName])},
                            { "ddlDefaultTheme", MDVUtility.ToStr(dr[dsUser.EntityUserOption.ThemeIdColumn.ColumnName])},

                            { "switchSearchPatient", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSearchPatientColumn.ColumnName])},
                            { "switchQuickPat", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsQuickAddPatientColumn.ColumnName])},
                            { "switchMessages", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsMessageColumn.ColumnName])},
                            { "switchAppointments", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsAppointmentColumn.ColumnName])},
                            { "switchNotes", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsNoteColumn.ColumnName])},
                            { "switchTasks", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsTaskColumn.ColumnName])},
                             { "switchFax", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsFaxColumn.ColumnName])},
                            { "switchPrescriptionsRefill", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPrescriptionsRefillColumn.ColumnName])},
                            { "switchPendingPrescriptions", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPendingPrescriptionsColumn.ColumnName])},
                            { "switchAssignedResultsAlert", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsAssignedResultsColumn.ColumnName])},
                            { "ddlRecentPatients", MDVUtility.ToStr(dr[dsUser.EntityUserOption.RecentPatientColumn.ColumnName])},
                            { "ddlPrimaryInsurance", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPrimaryInsuranceColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPrimaryInsuranceColumn.ColumnName]) == "False"?"0": ""},
                            { "ddlAdvanceBalance", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPatientAdvanceBalanceColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPatientAdvanceBalanceColumn.ColumnName]) == "False"?"0": ""},
                            { "ddlENMCodesTime", MDVUtility.ToStr(dr[dsUser.EntityUserOption.ENMCodesTimeColumn.ColumnName]) == "" ? "1" : MDVUtility.ToStr(dr[dsUser.EntityUserOption.ENMCodesTimeColumn.ColumnName])},
                            { "ddlNoteFontSize", MDVUtility.ToStr(dr[dsUser.EntityUserOption.NoteFontSizeColumn.ColumnName]) == "" ? "10" : MDVUtility.ToStr(dr[dsUser.EntityUserOption.NoteFontSizeColumn.ColumnName])},
                            { "ddlIsDefaultHPI", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDefaultHPIColumn.ColumnName]) == "" ? "False" : MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDefaultHPIColumn.ColumnName])},

                            { "chkCurrentMedications", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCurrentMedicationsColumn.ColumnName])},
                            { "chkActiveProblems", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsActiveProblemsColumn.ColumnName])},

                            { "chkActiveProcedures", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsActiveProceduresColumn.ColumnName])},
                            { "chkInActiveProcedures", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsInActiveProceduresColumn.ColumnName])},

                            { "chkFamilyHistory", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsFamilyHistoryColumn.ColumnName])},
                            { "chkBirthHistory", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsBirthHistoryColumn.ColumnName])},
                            { "chkPastMedications", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPastMedicationsColumn.ColumnName])},
                            { "chkInActiveProblems", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsInactiveProblemsColumn.ColumnName])},
                            { "chkSocialHistory", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSocialHistoryColumn.ColumnName])},
                            { "chkHospitalHistory", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsHospitalizationHistoryColumn.ColumnName])},
                            { "chkActiveAllergies", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsActiveAllergiesColumn.ColumnName])},
                            { "chkVitals", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsVitalsColumn.ColumnName])},
                            { "chkMedicalHistory", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsMedicalHistoryColumn.ColumnName])},
                            { "chkInActiveAllergies", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsInactiveAllergiesColumn.ColumnName])},
                            { "chkImmunizations", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsImmunizationsColumn.ColumnName])},
                            { "chkSurgicalHistory", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSurgicalHistoryColumn.ColumnName])},
                            { "ddlPreferredPhone", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PreferredPhoneColumn.ColumnName])},
                            { "ddlPreferredSchedulerScreen_text", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PreferredSchScreenNameColumn.ColumnName])},
                            { "ddlAppointmentStatus", MDVUtility.ToStr(dr[dsUser.EntityUserOption.AppointmentStatusColumn.ColumnName])},
                            { "switchImmunizationAlert", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsImmunizationAlertColumn.ColumnName])},
                            { "ddlSelectNoteComponents", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSelectNoteComponentsColumn.ColumnName])},
                            { "chkshowICD10", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsShowICD10Column.ColumnName])},
                            { "chkFacilityShortName", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsShowFacilityShortNameColumn.ColumnName])},
                            { "chkSendBillingInquiry", MDVUtility.ToStr(dr[dsUser.EntityUserOption.isSendBillingInquiryColumn.ColumnName])},
                            { "chkShowSuccessMessages", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsShowSuccessMessagesColumn.ColumnName])},
                            { "chkbxPETemplateNameRequired", MDVUtility.ToStr(dr[dsUser.EntityUserOption.ISPETemplateNameRequiredColumn.ColumnName])},
                            { "chkreferralRequired", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsReferralRequiredColumn.ColumnName])},
                            { "NotePreviewStyle", MDVUtility.ToStr(dr[dsUser.EntityUserOption.NotePrevieStyleColumn.ColumnName])},
                            { "ddlPreferredBillingScreen", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PreferredBillingScreenColumn.ColumnName])},
                            { "ddlPreferredBillingScreen_text", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PreferredBillingScreenNameColumn.ColumnName])},
                            { "EMCodeTypeIds", MDVUtility.ToStr(dr[dsUser.EntityUserOption.EMCodesTypeIdsColumn.ColumnName])},
                            { "RaceIds", MDVUtility.ToStr(dr[dsUser.EntityUserOption.RaceIdsColumn.ColumnName])},
                            { "ddlIsExpand", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsExpandColumn.ColumnName])},
                            { "chkSocPsyandBehaviorHx", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSocPsyandBehaviorHxColumn.ColumnName])},
                            { "ddlIsSearchCriteriaExpand", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSearchCriteriaExpandColumn.ColumnName])},
                            { "ddlCollectionBalance",MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCollectionColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCollectionColumn.ColumnName]) == "False"?"0": ""},
                            { "ddlDocumentPriority", MDVUtility.ToStr(dr[dsUser.EntityUserOption.DefaultDocumentPriorityIdColumn.ColumnName])},
                            { "ddlIsSchedulerTiemInterval", MDVUtility.ToStr(dr[dsUser.EntityUserOption.SchedulerTimeIntervalColumn.ColumnName])},
                            { "switchDocumentsAlert", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDocumentsAlertColumn.ColumnName])},
                            { "chkPatientName", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPatientNameColumn.ColumnName])},
                            { "chkDateOfBirth", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDOBColumn.ColumnName])},
                            { "chkAddress", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsAddressColumn.ColumnName])},
                            { "chkInsurancePlan", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsInsurancePlanColumn.ColumnName])},
                            { "chkSubscriberID", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSubscriberIDColumn.ColumnName])},
                            { "chkPatientAccountNo", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPatientAccountNoColumn.ColumnName])},
                            { "chkProvider", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsProviderColumn.ColumnName])},
                            { "chkPrevComplaints", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPrevNoteComplaintsColumn.ColumnName])},
                            { "WeekWorkDaysIds", WorkWeekDaysIDs},
                            { "ddlOrdersExpand", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsOrdersExpandColumn.ColumnName])},
                            { "ddlResultsExpand", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsResultsExpandColumn.ColumnName])},
                            { "chkPrevNoteROS", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPreviousNoteROSColumn.ColumnName])},
                            { "chkPrevNotePE", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPreviousNotePEColumn.ColumnName])},


                            { "chkDemographics", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDemographicsColumn.ColumnName])},
                            { "chkMU3FamilyHistory", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsMu3FamilyHistoryColumn.ColumnName])},
                            { "chkTransmissionkHealthCareSurveys", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsHealthcareSurveysColumn.ColumnName])},
                            { "chkTransmitImmunizationRegistries", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsImmunizationRegistriesColumn.ColumnName])},
                            { "chkPatientHealthInformationCapture", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPatientHealthInformationCaptureColumn.ColumnName])},
                            { "chkTransimissionCaseReporting", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsTransimissionCaseReportingColumn.ColumnName])},
                            { "chkTransitionCareDirectProject", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsTransitionDirectProjectColumn.ColumnName])},
                            { "chkPrivacyDataSegment", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDataSegmentationPrivacyColumn.ColumnName])},
                            { "chkImplantableDevices", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsImplantableDevicesColumn.ColumnName])},
                            { "chkTransitCancerRegistries", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsTransitCancerRegistriesColumn.ColumnName])},
                            { "chkConsolidatedCDACreationPreformance", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsConsolidatedCDAColumn.ColumnName])},
                            { "chkTransimissionAntimicobialUse", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsTransimissionAntimicobialColumn.ColumnName])},
                            { "chkSocPsyBehaviourHx", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsMU3SocPsyBehaviourHxColumn.ColumnName])},
                            { "chkDataExport", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDataExportColumn.ColumnName])},
                            { "chkCarePlan", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCarePlanColumn.ColumnName])},
                            { "ddlIsSelectCompOnCopyNote", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSelectCompOnCopyNoteColumn.ColumnName])},
                            { "ddlDefTabMedications", MDVUtility.ToStr(dr[dsUser.EntityUserOption.DefaultTabMedicationsColumn.ColumnName])}, //PRD-735 TahreeMalik
                            { "ddlBlueButtonNote", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsLandOnComponentColumn.ColumnName])},
                                    //PRD-31 by:MAHMAD
                            { "ddlIsExpandFolderTree", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsExpandFolderTreeColumn.ColumnName])},
                            { "iTrackDashboardIds", MDVUtility.ToStr(dr[dsUser.EntityUserOption.iTrackDashboardIdsColumn.ColumnName])},
                            { "chkPrevNoteProblems", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPrevNoteProblemsColumn.ColumnName])},
                            { "ddlCCMTimer", MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPatientCCMTimerColumn.ColumnName])== "True"?"1": MDVUtility.ToStr(dr[dsUser.EntityUserOption.PBPatientCCMTimerColumn.ColumnName]) == "False"?"0": ""},
                            { "chkPrevNoteTreatmentPlanComments", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPrevNoteTreatmentComentsColumn.ColumnName])},
                                    //PRD-31 by:MAHMAD
                            { "chkeAccess", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IseAccessColumn.ColumnName])},
                            { "chkePrescribing", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsePrescribingColumn.ColumnName])},
                            { "chkInCorporateSummaryOfCare", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsInCorporateSummaryOfCareColumn.ColumnName])},
                            { "chkPatientDocument", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPatientDocumentColumn.ColumnName])},
                            { "chkPatientEducation", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPatientEducationColumn.ColumnName])},
                            { "chkReconciliation", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsReconciliationColumn.ColumnName])},
                            { "chkSecureMessaging", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsSecureMessagingColumn.ColumnName])},
                            { "chkTransitionOfCare", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsTransitionOfCareColumn.ColumnName])},
                            { "chkViewDownloadTransmit", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsViewDownloadTransmitColumn.ColumnName])},

                            { "chkCMS65v7", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCMS65v7Column.ColumnName])},
                            { "chkCMS69v6", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCMS69v6Column.ColumnName])},
                            { "chkCMS68v7", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCMS68v7Column.ColumnName])},
                            { "chkCMS138v6", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCMS138v6Column.ColumnName])},
                            { "chkCMS165v6", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCMS165v6Column.ColumnName])},
                            { "chkCMS22v6", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCMS22v6Column.ColumnName])},

                            { "chkDepression", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsDepressionColumn.ColumnName])},
                            { "chkTobacco", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsTobaccoColumn.ColumnName])},
                            { "chkPatientPortalDocument", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsPatientPortalDocumentColumn.ColumnName])},
                            { "chkCDS", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsCDSColumn.ColumnName])},
                            { "ddlDefaultTabMessages", MDVUtility.ToStr(dr[dsUser.EntityUserOption.DefaultTabMessagesColumn.ColumnName])},
                            { "RecentMessagesTab", MDVUtility.ToStr(dr[dsUser.EntityUserOption.RecentMessagesTabColumn.ColumnName])},
                            { "chkIsNoteCompExpanded", MDVUtility.ToStr(dr[dsUser.EntityUserOption.IsNoteCompExpandedColumn.ColumnName])},

                    };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        js.MaxJsonLength = Int32.MaxValue;
                        var response = new
                        {
                            status = true,
                            EntityGroupCount = dsUser.Tables[dsUser.EntityUserOption.TableName].Rows.Count,
                            EntityGroupLoad_JSON = js.Serialize(keyValues),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            EntityGroupCount = 0,
                            Message = obj.Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdateDefaultSetting(string fieldsJSON, int EntityUserOptionId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSUsers dsDefaultsetting = new DSUsers();
                //DSUsers.EntityUserOptionRow dr = dsDefaultsetting.EntityUserOption.NewEntityUserOptionRow();

                DataTable dtRaceIds = new DataTable();

                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtRaceIds.Columns.Add(COLUMN);




                DSUsers dsUser1 = new DSUsers();
                BLObject<DSUsers> objUser = null;
                objUser = BLLAdminSecurityObj.LoadEntityUserOption(ref dsUser1, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), MDVSession.Current.EntityId);
                dsDefaultsetting = objUser.Data;
                foreach (DSUsers.EntityUserOptionRow dr in dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows)
                {
                    //dr.EntityUserOptionId = EntityUserOptionId;
                    dr.UserId = MDVSession.Current.AppUserId;
                    dr.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                    dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                    dr.IsActive = true;
                    dr.IsDefault = false;
                    // default Setting
                    if (SearchedfieldsJSON.ContainsKey("EMCodeTypeIds"))
                    {
                        if (SearchedfieldsJSON["EMCodeTypeIds"] == "")
                            dr[dsDefaultsetting.EntityUserOption.EMCodesTypeIdsColumn] = DBNull.Value;
                        else
                            dr.EMCodesTypeIds = MDVUtility.ToStr(SearchedfieldsJSON["EMCodeTypeIds"]);

                        MDVSession.Current.EMCodeTypeIds = SearchedfieldsJSON["EMCodeTypeIds"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlPreferredScreenAfterLogin"))
                    {
                        if (SearchedfieldsJSON["ddlPreferredScreenAfterLogin"] == "")
                            dr[dsDefaultsetting.EntityUserOption.PreferredScreenColumn] = DBNull.Value;
                        else
                            dr.PreferredScreen = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPreferredScreenAfterLogin"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultProvider"))
                    {
                        if (SearchedfieldsJSON["ddlDefaultProvider"] == "")
                            dr[dsDefaultsetting.EntityUserOption.ProviderIdColumn] = DBNull.Value;
                        else
                            dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultProvider"]);

                        MDVSession.Current.DefaultProviderId = SearchedfieldsJSON["ddlDefaultProvider"];
                        MDVSession.Current.DefaultProviderName = SearchedfieldsJSON["ddlDefaultProvider_text"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultBillingProvider"))
                    {
                        if (SearchedfieldsJSON["ddlDefaultBillingProvider"] == "")
                            dr[dsDefaultsetting.EntityUserOption.BillingProviderIdColumn] = DBNull.Value;
                        else
                            dr.BillingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultBillingProvider"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlPreferredSchedulerScreen"))
                    {
                        if (SearchedfieldsJSON["ddlPreferredSchedulerScreen"] == "")
                        {
                            dr[dsDefaultsetting.EntityUserOption.PreferredSchScreenColumn] = DBNull.Value;
                            dr[dsDefaultsetting.EntityUserOption.PreferredSchScreenNameColumn] = DBNull.Value;
                        }
                        else
                        {
                            dr.PreferredSchScreen = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPreferredSchedulerScreen"]);
                            dr.PreferredSchScreenName = MDVUtility.ToStr(SearchedfieldsJSON["ddlPreferredSchedulerScreen_text"]);

                        }
                    }


                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultFacility"))
                    {
                        if (SearchedfieldsJSON["ddlDefaultFacility"] == "")
                            dr[dsDefaultsetting.EntityUserOption.FacilityIdColumn] = DBNull.Value;
                        else
                            dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultFacility"]);
                        MDVSession.Current.DefaultFacilityId = SearchedfieldsJSON["ddlDefaultFacility"];
                        MDVSession.Current.DefaultFacilityName = SearchedfieldsJSON["ddlDefaultFacility_text"];
                        MDVSession.Current.DefaultFacilityDescription = SearchedfieldsJSON["FacilityDescription"];

                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultTemplate"))
                    {
                        if (SearchedfieldsJSON["ddlDefaultTemplate"] == "")
                            dr[dsDefaultsetting.EntityUserOption.DefaultTemplateColumn] = DBNull.Value;
                        else
                            dr.DefaultTemplate = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultTemplate"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultResource"))
                    {
                        if (SearchedfieldsJSON["ddlDefaultResource"] == "")
                            dr[dsDefaultsetting.EntityUserOption.ResourceIdColumn] = DBNull.Value;
                        else
                            dr.ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultResource"]);

                        MDVSession.Current.DefaultResourceId = SearchedfieldsJSON["ddlDefaultResource"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultTheme"))
                    {
                        if (SearchedfieldsJSON["ddlDefaultTheme"] == "")
                            dr[dsDefaultsetting.EntityUserOption.ThemeIdColumn] = DBNull.Value;
                        else
                            dr.ThemeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultTheme"]);
                        MDVSession.Current.DefaultThemeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlDefaultTheme_text"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultSuperBill"))
                    {
                        if (SearchedfieldsJSON["ddlDefaultSuperBill"] == "")
                            dr[dsDefaultsetting.EntityUserOption.DefaultSuperBillColumn] = DBNull.Value;
                        else
                            dr.DefaultSuperBill = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultSuperBill"]);
                    }

                    // Patient Banner

                    //if (SearchedfieldsJSON.ContainsKey("ddlPhoneNumber1") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPhoneNumber1"]))
                    //{
                    //    dr.PBPhoneNumber1 = MDVUtility.ToStr(SearchedfieldsJSON["ddlPhoneNumber1"]) == "1" ? true : false;
                    //}
                    //else
                    //{
                    //    dr[dsDefaultsetting.EntityUserOption.PBPhoneNumber1Column] = DBNull.Value;
                    //}
                    //if (SearchedfieldsJSON.ContainsKey("ddlPhoneNumber2") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPhoneNumber2"]))
                    //{
                    //    dr.PBPhoneNumber2 = MDVUtility.ToStr(SearchedfieldsJSON["ddlPhoneNumber2"]) == "1" ? true : false;
                    //}
                    //else
                    //{
                    //    dr[dsDefaultsetting.EntityUserOption.PBPhoneNumber2Column] = DBNull.Value;
                    //}
                    if (SearchedfieldsJSON.ContainsKey("ddlPCP"))
                    {
                        dr.PBPCP = MDVUtility.ToStr(SearchedfieldsJSON["ddlPCP"]) == "1" ? true : false;
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PBPCPColumn] = DBNull.Value;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlReferringProvider"))
                    {
                        dr.PBRefProvider = MDVUtility.ToStr(SearchedfieldsJSON["ddlReferringProvider"]) == "1" ? true : false;
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PBRefProviderColumn] = DBNull.Value;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlPlanBalance"))
                    {
                        dr.PBPlanBalance = MDVUtility.ToStr(SearchedfieldsJSON["ddlPlanBalance"]) == "1" ? true : false;
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PBPlanBalanceColumn] = DBNull.Value;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlPatientBalance"))
                    {
                        dr.PBPatientBalance = MDVUtility.ToStr(SearchedfieldsJSON["ddlPatientBalance"]) == "1" ? true : false;
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PBPatientBalanceColumn] = DBNull.Value;
                    }
                    // Primary insurance and Patient Advance balance
                    if (SearchedfieldsJSON.ContainsKey("ddlPrimaryInsurance"))
                    {
                        dr.PBPrimaryInsurance = MDVUtility.ToStr(SearchedfieldsJSON["ddlPrimaryInsurance"]) == "1" ? true : false;
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PBPrimaryInsuranceColumn] = DBNull.Value;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlAdvanceBalance"))
                    {
                        dr.PBPatientAdvanceBalance = MDVUtility.ToStr(SearchedfieldsJSON["ddlAdvanceBalance"]) == "1" ? true : false;
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PBPatientAdvanceBalanceColumn] = DBNull.Value;
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlPreferredPhone"))
                    {
                        dr.PreferredPhone = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPreferredPhone"]);
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PreferredPhoneColumn] = DBNull.Value;
                    }

                    // Printer Setting

                    if (SearchedfieldsJSON.ContainsKey("ddlclaimprinter_text"))
                    {
                        dr.ClaimPrinter = MDVUtility.ToStr(SearchedfieldsJSON["ddlclaimprinter_text"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlclaimtray"))
                    {
                        dr.ClaimTray = MDVUtility.ToStr(SearchedfieldsJSON["ddlclaimtray"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddattachmentprinter_text"))
                    {
                        dr.AttachmentPrinter = MDVUtility.ToStr(SearchedfieldsJSON["ddattachmentprinter_text"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlattachmenttray"))
                    {
                        dr.AttachmentTray = MDVUtility.ToStr(SearchedfieldsJSON["ddlattachmenttray"]);
                    }

                    if (SearchedfieldsJSON.ContainsKey("txtRefreshTime"))
                    {
                        dr.RefreshTime = MDVUtility.ToStr(SearchedfieldsJSON["txtRefreshTime"]);
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlRecentPatients"))
                    {
                        dr.RecentPatient = MDVUtility.ToInt32(SearchedfieldsJSON["ddlRecentPatients"]);
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlENMCodesTime") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlENMCodesTime"]))
                    {
                        dr.ENMCodesTime = MDVUtility.ToInt32(SearchedfieldsJSON["ddlENMCodesTime"]);
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlNoteFontSize") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlNoteFontSize"]))
                    {
                        dr.NoteFontSize = MDVUtility.ToInt32(SearchedfieldsJSON["ddlNoteFontSize"]);
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlIsDefaultHPI"))
                    {
                        dr.IsDefaultHPI = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsDefaultHPI"]) == "0" ? false : true;
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlCollectionBalance"))
                    {
                        dr.IsCollection = MDVUtility.ToStr(SearchedfieldsJSON["ddlCollectionBalance"]) == "1" ? true : false;
                    }
                    //dr.CreatedBy = MDVUtility.DecryptFrom64(Common.MDVSession.Current.AppUserName);
                    //dr.CreatedOn = DateTime.Now;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    //--------- Quick Links Settings IrFan --------
                    if (SearchedfieldsJSON.ContainsKey("switchSearchPatient"))
                    {

                        dr.IsSearchPatient = SearchedfieldsJSON["switchSearchPatient"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchQuickPat"))
                    {
                        dr.IsQuickAddPatient = SearchedfieldsJSON["switchQuickPat"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchMessages"))
                    {
                        dr.IsMessage = SearchedfieldsJSON["switchMessages"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchAppointments"))
                    {
                        dr.IsAppointment = SearchedfieldsJSON["switchAppointments"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchNotes"))
                    {
                        dr.IsNote = SearchedfieldsJSON["switchNotes"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchTasks"))
                    {
                        dr.IsTask = SearchedfieldsJSON["switchTasks"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchFax"))
                    {
                        dr.IsFax = SearchedfieldsJSON["switchFax"];
                    }

                    /* Author: Muhammad Azhar Shahzad
                                    Date: Januray 18, 2016
                                    Reason: To show Prescriptions Refill and Pending Prescriptions Quick Links*/
                    if (SearchedfieldsJSON.ContainsKey("switchPendingPrescriptions"))
                    {
                        dr.IsPendingPrescriptions = SearchedfieldsJSON["switchPendingPrescriptions"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchPrescriptionsRefill"))
                    {
                        dr.IsPrescriptionsRefill = SearchedfieldsJSON["switchPrescriptionsRefill"];
                    }
                    /* end Change BY Azhar
                                    Date: Januray 18, 2016 */
                    //--------------------- End Quick Add ------------------
                    if (SearchedfieldsJSON.ContainsKey("switchImmunizationAlert"))
                    {
                        dr.IsImmunizationAlert = SearchedfieldsJSON["switchImmunizationAlert"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchDocumentsAlert"))
                    {
                        dr.IsDocumentsAlert = SearchedfieldsJSON["switchDocumentsAlert"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkPatientName"))
                    {
                        dr.IsPatientName = SearchedfieldsJSON["chkPatientName"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkDateOfBirth"))
                    {
                        dr.IsDOB = SearchedfieldsJSON["chkDateOfBirth"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkAddress"))
                    {
                        dr.IsAddress = SearchedfieldsJSON["chkAddress"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkInsurancePlan"))
                    {
                        dr.IsInsurancePlan = SearchedfieldsJSON["chkInsurancePlan"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkSubscriberID"))
                    {
                        dr.IsSubscriberID = SearchedfieldsJSON["chkSubscriberID"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkPatientAccountNo"))
                    {
                        dr.IsPatientAccountNo = SearchedfieldsJSON["chkPatientAccountNo"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkProvider"))
                    {
                        dr.IsProvider = SearchedfieldsJSON["chkProvider"];
                    }


                    if (SearchedfieldsJSON.ContainsKey("chkCurrentMedications"))
                    {
                        dr.IsCurrentMedications = SearchedfieldsJSON["chkCurrentMedications"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkActiveProblems"))
                    {
                        dr.IsActiveProblems = SearchedfieldsJSON["chkActiveProblems"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkFamilyHistory"))
                    {
                        dr.IsFamilyHistory = SearchedfieldsJSON["chkFamilyHistory"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkBirthHistory"))
                    {
                        dr.IsBirthHistory = SearchedfieldsJSON["chkBirthHistory"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkPastMedications"))
                    {
                        dr.IsPastMedications = SearchedfieldsJSON["chkPastMedications"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkInActiveProblems"))
                    {
                        dr.IsInactiveProblems = SearchedfieldsJSON["chkInActiveProblems"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkSocialHistory"))
                    {
                        dr.IsSocialHistory = SearchedfieldsJSON["chkSocialHistory"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkHospitalHistory"))
                    {
                        dr.IsHospitalizationHistory = SearchedfieldsJSON["chkHospitalHistory"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkActiveProcedures"))
                    {
                        dr.IsActiveProcedures = SearchedfieldsJSON["chkActiveProcedures"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkInActiveProcedures"))
                    {
                        dr.IsInActiveProcedures = SearchedfieldsJSON["chkInActiveProcedures"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkActiveAllergies"))
                    {
                        dr.IsActiveAllergies = SearchedfieldsJSON["chkActiveAllergies"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkVitals"))
                    {
                        dr.IsVitals = SearchedfieldsJSON["chkVitals"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkMedicalHistory"))
                    {
                        dr.IsMedicalHistory = SearchedfieldsJSON["chkMedicalHistory"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkInActiveAllergies"))
                    {
                        dr.IsInactiveAllergies = SearchedfieldsJSON["chkInActiveAllergies"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkImmunizations"))
                    {
                        dr.IsImmunizations = SearchedfieldsJSON["chkImmunizations"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkSurgicalHistory"))
                    {
                        dr.IsSurgicalHistory = SearchedfieldsJSON["chkSurgicalHistory"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlAppointmentStatus"))
                    {
                        dr.AppointmentStatus = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAppointmentStatus"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlSelectNoteComponents"))
                    {
                        dr.IsSelectNoteComponents = MDVUtility.ToStr(SearchedfieldsJSON["ddlSelectNoteComponents"]) == "0" ? false : true;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlIsExpand"))
                    {
                        dr.IsExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsExpand"]) == "0" ? false : true;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlIsSearchCriteriaExpand"))
                    {
                        dr.IsSearchCriteriaExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsSearchCriteriaExpand"]) == "0" ? false : true;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority"))
                    {
                        if (SearchedfieldsJSON["ddlDocumentPriority"] == "")
                            dr[dsDefaultsetting.EntityUserOption.DefaultDocumentPriorityIdColumn] = DBNull.Value;
                        else
                            dr.DefaultDocumentPriorityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentPriority"]);
                        MDVSession.Current.DefaultDocumentPriorityId = SearchedfieldsJSON["ddlDocumentPriority"];
                        MDVSession.Current.DefaultDocumentPriorityName = SearchedfieldsJSON["ddlDocumentPriority_text"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkshowICD10"))
                    {
                        dr.IsShowICD10 = SearchedfieldsJSON["chkshowICD10"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkFacilityShortName"))
                    {
                        dr.IsShowFacilityShortName = SearchedfieldsJSON["chkFacilityShortName"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkSendBillingInquiry"))
                    {
                        dr.isSendBillingInquiry = SearchedfieldsJSON["chkSendBillingInquiry"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkShowSuccessMessages"))
                    {
                        dr.IsShowSuccessMessages = MDVUtility.ToStr(SearchedfieldsJSON["chkShowSuccessMessages"]) == "True" ? true : false;
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkreferralRequired"))
                    {
                        dr.IsReferralRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkreferralRequired"]) == "True" ? true : false;
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkbxPETemplateName"))
                    {
                        dr.ISPETemplateNameRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkbxPETemplateName"]) == "True" ? true : false;
                        MDVSession.Current.isPETemplateNameRequired = MDVUtility.ToStr(dr.ISPETemplateNameRequired);
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlNotePreviewStyle"))
                    {
                        dr.NotePrevieStyle = SearchedfieldsJSON["ddlNotePreviewStyle"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkSocPsyandBehaviorHx"))
                    {
                        dr.IsSocPsyandBehaviorHx = SearchedfieldsJSON["chkSocPsyandBehaviorHx"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlIsSchedulerTiemInterval") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlIsSchedulerTiemInterval"]))
                    {
                        dr.SchedulerTimeInterval = MDVUtility.ToInt32(SearchedfieldsJSON["ddlIsSchedulerTiemInterval"]);
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.SchedulerTimeIntervalColumn] = DBNull.Value;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlPreferredBillingScreen"))
                    {
                        if (SearchedfieldsJSON["ddlPreferredBillingScreen"] == "")
                        {
                            dr[dsDefaultsetting.EntityUserOption.PreferredBillingScreenColumn] = DBNull.Value;
                            dr[dsDefaultsetting.EntityUserOption.PreferredBillingScreenNameColumn] = DBNull.Value;
                        }
                        else
                        {
                            dr.PreferredBillingScreen = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPreferredBillingScreen"]);
                            dr.PreferredBillingScreenName = MDVUtility.ToStr(SearchedfieldsJSON["ddlPreferredBillingScreen_text"]);

                        }
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkPrevComplaints"))
                    {
                        dr.IsPrevNoteComplaints = SearchedfieldsJSON["chkPrevComplaints"];
                    }

                    if (SearchedfieldsJSON.ContainsKey("WorkingDaysList") && !string.IsNullOrEmpty(SearchedfieldsJSON["WorkingDaysList"]))
                    {
                        dr.WorkWeekDays = MDVUtility.ToStr(SearchedfieldsJSON["WorkingDaysList"]);
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlOrdersExpand"))
                    {
                        dr.IsOrdersExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlOrdersExpand"]) == "0" ? false : true;
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlResultsExpand"))
                    {
                        dr.IsResultsExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlResultsExpand"]) == "0" ? false : true;
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkDemographics") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkDemographics"])))
                        dr.IsDemographics = SearchedfieldsJSON["chkDemographics"];
                    else
                        dr.IsDemographics = false;
                    if (SearchedfieldsJSON.ContainsKey("chkMU3FamilyHistory") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkMU3FamilyHistory"])))
                        dr.IsMu3FamilyHistory = SearchedfieldsJSON["chkMU3FamilyHistory"];
                    else
                        dr.IsMu3FamilyHistory = false;
                    if (SearchedfieldsJSON.ContainsKey("chkTransmissionkHealthCareSurveys") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransmissionkHealthCareSurveys"])))
                        dr.IsHealthcareSurveys = SearchedfieldsJSON["chkTransmissionkHealthCareSurveys"];
                    else
                        dr.IsHealthcareSurveys = false;
                    if (SearchedfieldsJSON.ContainsKey("chkTransmitImmunizationRegistries") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransmitImmunizationRegistries"])))
                        dr.IsImmunizationRegistries = SearchedfieldsJSON["chkTransmitImmunizationRegistries"];
                    else
                        dr.IsImmunizationRegistries = false;
                    if (SearchedfieldsJSON.ContainsKey("chkPatientHealthInformationCapture") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientHealthInformationCapture"])))
                        dr.IsPatientHealthInformationCapture = SearchedfieldsJSON["chkPatientHealthInformationCapture"];
                    else
                        dr.IsPatientHealthInformationCapture = false;
                    if (SearchedfieldsJSON.ContainsKey("chkTransimissionCaseReporting") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransimissionCaseReporting"])))
                        dr.IsTransimissionCaseReporting = SearchedfieldsJSON["chkTransimissionCaseReporting"];
                    else
                        dr.IsTransimissionCaseReporting = false;
                    if (SearchedfieldsJSON.ContainsKey("chkTransitionCareDirectProject") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransitionCareDirectProject"])))
                        dr.IsTransitionDirectProject = SearchedfieldsJSON["chkTransitionCareDirectProject"];
                    else
                        dr.IsTransitionDirectProject = false;
                    if (SearchedfieldsJSON.ContainsKey("chkPrivacyDataSegment") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPrivacyDataSegment"])))
                        dr.IsDataSegmentationPrivacy = SearchedfieldsJSON["chkPrivacyDataSegment"];
                    else
                        dr.IsDataSegmentationPrivacy = false;
                    if (SearchedfieldsJSON.ContainsKey("chkImplantableDevices") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkImplantableDevices"])))
                        dr.IsImplantableDevices = SearchedfieldsJSON["chkImplantableDevices"];
                    else
                        dr.IsImplantableDevices = false;
                    if (SearchedfieldsJSON.ContainsKey("chkTransitCancerRegistries") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransitCancerRegistries"])))
                        dr.IsTransitCancerRegistries = SearchedfieldsJSON["chkTransitCancerRegistries"];
                    else
                        dr.IsTransitCancerRegistries = false;
                    if (SearchedfieldsJSON.ContainsKey("chkConsolidatedCDACreationPreformance") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkConsolidatedCDACreationPreformance"])))
                        dr.IsConsolidatedCDA = SearchedfieldsJSON["chkConsolidatedCDACreationPreformance"];
                    else
                        dr.IsConsolidatedCDA = false;
                    if (SearchedfieldsJSON.ContainsKey("chkTransimissionAntimicobialUse") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransimissionAntimicobialUse"])))
                        dr.IsTransimissionAntimicobial = SearchedfieldsJSON["chkTransimissionAntimicobialUse"];
                    else
                        dr.IsTransimissionAntimicobial = false;
                    if (SearchedfieldsJSON.ContainsKey("chkSocPsyBehaviourHx") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkSocPsyBehaviourHx"])))
                        dr.IsMU3SocPsyBehaviourHx = SearchedfieldsJSON["chkSocPsyBehaviourHx"];
                    else
                        dr.IsMU3SocPsyBehaviourHx = false;
                    if (SearchedfieldsJSON.ContainsKey("chkDataExport") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkDataExport"])))
                        dr.IsDataExport = SearchedfieldsJSON["chkDataExport"];
                    else
                        dr.IsDataExport = false;
                    if (SearchedfieldsJSON.ContainsKey("chkCarePlan") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCarePlan"])))
                        dr.IsCarePlan = SearchedfieldsJSON["chkCarePlan"];
                    else
                        dr.IsCarePlan = false;
                    if (SearchedfieldsJSON.ContainsKey("ddlIsSelectCompOnCopyNote"))
                    {
                        dr.IsSelectCompOnCopyNote = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsSelectCompOnCopyNote"]) == "0" ? false : true;
                    }
                    //PRD-31 by:MAHMAD
                    if (SearchedfieldsJSON.ContainsKey("ddlIsExpandFolderTree"))
                    {
                        dr.IsExpandFolderTree = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsExpandFolderTree"]) == "0" ? false : true;
                    }
                    //PRD-31 by:MAHMAD
                    if (SearchedfieldsJSON.ContainsKey("chkPrevNoteROS"))
                    {
                        dr.IsPreviousNoteROS = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNoteROS"]) == "True" ? true : false;
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkPrevNotePE"))
                    {
                        dr.IsPreviousNotePE = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNotePE"]) == "True" ? true : false;
                    }
                    if (SearchedfieldsJSON.ContainsKey("iTrackDashboardIds") && !string.IsNullOrEmpty(SearchedfieldsJSON["iTrackDashboardIds"]))
                    {
                        dr.iTrackDashboardIds = MDVUtility.ToStr(SearchedfieldsJSON["iTrackDashboardIds"]);
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.iTrackDashboardIdsColumn] = DBNull.Value;
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkPrevNoteTreatmentPlanComments"))
                    {
                        dr.IsPrevNoteTreatmentComents = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNoteTreatmentPlanComments"]) == "True" ? true : false;
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkIsNoteCompExpanded"))
                    {
                        dr.IsNoteCompExpanded = MDVUtility.ToStr(SearchedfieldsJSON["chkIsNoteCompExpanded"]) == "True" ? true : false;
                    }

                    if (SearchedfieldsJSON["RaceIds"] != "")
                    {
                        String[] substrings = SearchedfieldsJSON["RaceIds"].Split(',');
                        foreach (var substring in substrings)
                        {
                            DataRow Dr = dtRaceIds.NewRow();
                            Dr[0] = substring;
                            dtRaceIds.Rows.Add(Dr);
                        }
                        MDVSession.Current.RaceIds = SearchedfieldsJSON["RaceIds"];
                    }
                    else
                    {
                        DataRow Dr = dtRaceIds.NewRow();
                        Dr[0] = -1;
                        dtRaceIds.Rows.Add(Dr);
                        MDVSession.Current.RaceIds = "";
                    }
                    if (SearchedfieldsJSON.ContainsKey("chkPrevNoteProblems"))
                    {
                        dr.IsPrevNoteProblems = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNoteProblems"]) == "True" ? true : false;
                    }

                    if (SearchedfieldsJSON.ContainsKey("ddlCCMTimer"))
                    {
                        dr.PBPatientCCMTimer = MDVUtility.ToStr(SearchedfieldsJSON["ddlCCMTimer"]) == "1" ? true : false;
                    }
                    else
                    {
                        dr[dsDefaultsetting.EntityUserOption.PBPatientCCMTimerColumn] = DBNull.Value;
                    }

                    if (SearchedfieldsJSON.ContainsKey("chkeAccess") &&
                    !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkeAccess"])))
                        dr.IseAccess = SearchedfieldsJSON["chkeAccess"];
                    else
                        dr.IseAccess = false;

                    if (SearchedfieldsJSON.ContainsKey("chkePrescribing") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkePrescribing"])))
                        dr.IsePrescribing = SearchedfieldsJSON["chkePrescribing"];
                    else
                        dr.IsePrescribing = false;

                    if (SearchedfieldsJSON.ContainsKey("chkInCorporateSummaryOfCare") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkInCorporateSummaryOfCare"])))
                        dr.IsInCorporateSummaryOfCare = SearchedfieldsJSON["chkInCorporateSummaryOfCare"];
                    else
                        dr.IsInCorporateSummaryOfCare = false;

                    if (SearchedfieldsJSON.ContainsKey("chkPatientDocument") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientDocument"])))
                        dr.IsPatientDocument = SearchedfieldsJSON["chkPatientDocument"];
                    else
                        dr.IsPatientDocument = false;


                    if (SearchedfieldsJSON.ContainsKey("chkPatientEducation") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientEducation"])))
                        dr.IsPatientEducation = SearchedfieldsJSON["chkPatientEducation"];
                    else
                        dr.IsPatientEducation = false;


                    if (SearchedfieldsJSON.ContainsKey("chkReconciliation") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkReconciliation"])))
                        dr.IsReconciliation = SearchedfieldsJSON["chkReconciliation"];
                    else
                        dr.IsReconciliation = false;

                    if (SearchedfieldsJSON.ContainsKey("chkSecureMessaging") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkSecureMessaging"])))
                        dr.IsSecureMessaging = SearchedfieldsJSON["chkSecureMessaging"];
                    else
                        dr.IsSecureMessaging = false;


                    if (SearchedfieldsJSON.ContainsKey("chkTransitionOfCare") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransitionOfCare"])))
                        dr.IsTransitionOfCare = SearchedfieldsJSON["chkTransitionOfCare"];
                    else
                        dr.IsTransitionOfCare = false;

                    if (SearchedfieldsJSON.ContainsKey("chkViewDownloadTransmit") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkViewDownloadTransmit"])))
                        dr.IsViewDownloadTransmit = SearchedfieldsJSON["chkViewDownloadTransmit"];
                    else
                        dr.IsViewDownloadTransmit = false;

                    if (SearchedfieldsJSON.ContainsKey("chkCMS65v7") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS65v7"])))
                        dr.IsCMS65v7 = SearchedfieldsJSON["chkCMS65v7"];
                    else
                        dr.IsCMS65v7 = false;

                    if (SearchedfieldsJSON.ContainsKey("chkCMS69v6") &&
                      !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS69v6"])))
                        dr.IsCMS69v6 = SearchedfieldsJSON["chkCMS69v6"];
                    else
                        dr.IsCMS69v6 = false;

                    if (SearchedfieldsJSON.ContainsKey("chkCMS68v7") &&
                      !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS68v7"])))
                        dr.IsCMS68v7 = SearchedfieldsJSON["chkCMS68v7"];
                    else
                        dr.IsCMS68v7 = false;

                    if (SearchedfieldsJSON.ContainsKey("chkCMS138v6") &&
                      !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS138v6"])))
                        dr.IsCMS138v6 = SearchedfieldsJSON["chkCMS138v6"];
                    else
                        dr.IsCMS138v6 = false;

                    if (SearchedfieldsJSON.ContainsKey("chkCMS165v6") &&
                      !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS165v6"])))
                        dr.IsCMS165v6 = SearchedfieldsJSON["chkCMS165v6"];
                    else
                        dr.IsCMS165v6 = false;

                    if (SearchedfieldsJSON.ContainsKey("chkCMS22v6") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS22v6"])))
                        dr.IsCMS22v6 = SearchedfieldsJSON["chkCMS22v6"];
                    else
                        dr.IsCMS22v6 = false;

                    if (SearchedfieldsJSON.ContainsKey("chkDepression") &&
                      !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkDepression"])))
                        dr.IsDepression = SearchedfieldsJSON["chkDepression"];
                    else
                        dr.IsDepression = false;

                    if (SearchedfieldsJSON.ContainsKey("chkTobacco") &&
                     !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTobacco"])))
                        dr.IsTobacco = SearchedfieldsJSON["chkTobacco"];
                    else
                        dr.IsTobacco = false;

                    if (SearchedfieldsJSON.ContainsKey("chkPatientPortalDocument") &&
                     !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientPortalDocument"])))
                        dr.IsPatientPortalDocument = SearchedfieldsJSON["chkPatientPortalDocument"];
                    else
                        dr.IsPatientPortalDocument = false;

                    if (SearchedfieldsJSON.ContainsKey("chkCDS") &&
                     !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCDS"])))
                        dr.IsCDS = SearchedfieldsJSON["chkCDS"];
                    else
                        dr.IsCDS = false;
                    if (SearchedfieldsJSON.ContainsKey("ddlBlueButtonNote") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBlueButtonNote"])))
                        dr.IsLandOnComponent = MDVUtility.ToStr(SearchedfieldsJSON["ddlBlueButtonNote"]);
                    else
                        dr[dsDefaultsetting.EntityUserOption.IsLandOnComponentColumn] = DBNull.Value;

                    if (SearchedfieldsJSON.ContainsKey("ddlDefTabMedications"))         //PRD-735 TahreeMalik
                    {
                        dr.DefaultTabMedications = SearchedfieldsJSON["ddlDefTabMedications"];
                    }
                    if (SearchedfieldsJSON.ContainsKey("ddlDefaultTabMessages") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultTabMessages"]))
                    {
                        dr.DefaultTabMessages = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultTabMessages"]);
                    }
                    if (SearchedfieldsJSON.ContainsKey("switchAssignedResults"))        //PRD-785 TahreeMalik
                    {
                        dr.IsAssignedResults = SearchedfieldsJSON["switchAssignedResults"];
                    }
                }



                #region Database Updation
                //dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
                //dsDefaultsetting.EntityUserOption.AcceptChanges();

                if (dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows.Count > 0)
                {
                    //dsDefaultsetting.EntityUserOption.Rows[0].SetModified();
                    BLObject<DSUsers> obj = BLLAdminSecurityObj.UpdatDefaultSettings(dsDefaultsetting, dtRaceIds);
                    if (obj.Data != null)
                    {
                        SetApplicationConfig(dsDefaultsetting);
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string SaveDefaultSetting(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSUsers dsDefaultsetting = new DSUsers();
                DSUsers.EntityUserOptionRow dr = dsDefaultsetting.EntityUserOption.NewEntityUserOptionRow();


                // default Setting
                if (SearchedfieldsJSON.ContainsKey("ddlPreferredScreenAfterLogin") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPreferredScreenAfterLogin"]))
                {
                    dr.PreferredScreen = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPreferredScreenAfterLogin"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDefaultProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultProvider"]))
                {
                    dr.ProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultProvider"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDefaultBillingProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultBillingProvider"]))
                {
                    dr.BillingProviderId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultBillingProvider"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlPreferredSchedulerScreen") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPreferredSchedulerScreen"]))
                {
                    dr.PreferredSchScreen = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPreferredSchedulerScreen"]);
                    dr.PreferredSchScreenName = MDVUtility.ToStr(SearchedfieldsJSON["ddlPreferredSchedulerScreen_text"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDefaultFacility") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultFacility"]))
                {
                    dr.FacilityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultFacility"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDefaultTemplate") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultTemplate"]))
                {
                    dr.DefaultTemplate = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultTemplate"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDefaultResource") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultResource"]))
                {
                    dr.ResourceId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultResource"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDefaultSuperBill") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultSuperBill"]))
                {
                    dr.DefaultSuperBill = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultSuperBill"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlIsSchedulerTiemInterval") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlIsSchedulerTiemInterval"]))
                {
                    dr.SchedulerTimeInterval = MDVUtility.ToInt32(SearchedfieldsJSON["ddlIsSchedulerTiemInterval"]);
                }

                if (SearchedfieldsJSON.ContainsKey("ddlDefaultTheme") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultTheme"]))
                {
                    dr.ThemeId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultTheme"]);
                    MDVSession.Current.DefaultThemeName = MDVUtility.ToStr(SearchedfieldsJSON["ddlDefaultTheme_text"]);
                }

                // Patient Banner

                //if (SearchedfieldsJSON.ContainsKey("ddlPhoneNumber1") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPhoneNumber1"]))
                //{
                //    dr.PBPhoneNumber1 = MDVUtility.ToStr(SearchedfieldsJSON["ddlPhoneNumber1"]) == "1" ? true : false;
                //}
                //if (SearchedfieldsJSON.ContainsKey("ddlPhoneNumber2") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPhoneNumber2"]))
                //{
                //    dr.PBPhoneNumber2 = MDVUtility.ToStr(SearchedfieldsJSON["ddlPhoneNumber2"]) == "1" ? true : false;
                //}
                if (SearchedfieldsJSON.ContainsKey("ddlPCP") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPCP"]))
                {
                    dr.PBPCP = MDVUtility.ToStr(SearchedfieldsJSON["ddlPCP"]) == "1" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlReferringProvider") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlReferringProvider"]))
                {
                    dr.PBRefProvider = MDVUtility.ToStr(SearchedfieldsJSON["ddlReferringProvider"]) == "1" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlPlanBalance") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPlanBalance"]))
                {
                    dr.PBPlanBalance = MDVUtility.ToStr(SearchedfieldsJSON["ddlPlanBalance"]) == "1" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlPatientBalance") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPatientBalance"]))
                {
                    dr.PBPatientBalance = MDVUtility.ToStr(SearchedfieldsJSON["ddlPatientBalance"]) == "1" ? true : false;
                }

                if (SearchedfieldsJSON.ContainsKey("ddlPreferredPhone") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPreferredPhone"]))
                {
                    dr.PreferredPhone = MDVUtility.ToInt32(SearchedfieldsJSON["ddlPreferredPhone"]);
                }

                // Printer Setting

                if (SearchedfieldsJSON.ContainsKey("ddlclaimprinter_text") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlclaimprinter_text"]))
                {
                    dr.ClaimPrinter = MDVUtility.ToStr(SearchedfieldsJSON["ddlclaimprinter_text"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlclaimtray") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlclaimtray"]))
                {
                    dr.ClaimTray = MDVUtility.ToStr(SearchedfieldsJSON["ddlclaimtray"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddattachmentprinter_text") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddattachmentprinter_text"]))
                {
                    dr.AttachmentPrinter = MDVUtility.ToStr(SearchedfieldsJSON["ddattachmentprinter_text"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlattachmenttray") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlattachmenttray"]))
                {
                    dr.AttachmentTray = MDVUtility.ToStr(SearchedfieldsJSON["ddlattachmenttray"]);
                }
                if (SearchedfieldsJSON.ContainsKey("txtRefreshTime"))
                {
                    dr.RefreshTime = MDVUtility.ToStr(SearchedfieldsJSON["txtRefreshTime"]);
                }

                if (SearchedfieldsJSON.ContainsKey("ddlRecentPatients") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlRecentPatients"]))
                {
                    dr.RecentPatient = MDVUtility.ToInt32(SearchedfieldsJSON["ddlRecentPatients"]);
                }

                if (SearchedfieldsJSON.ContainsKey("ddlENMCodesTime") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlENMCodesTime"]))
                {
                    dr.ENMCodesTime = MDVUtility.ToInt32(SearchedfieldsJSON["ddlENMCodesTime"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlNoteFontSize") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlNoteFontSize"]))
                {
                    dr.NoteFontSize = MDVUtility.ToInt32(SearchedfieldsJSON["ddlNoteFontSize"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlPreferredBillingScreen") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlPreferredBillingScreen"]))
                {
                    dr.PreferredBillingScreen = MDVUtility.ToInt64(SearchedfieldsJSON["ddlPreferredBillingScreen"]);
                    dr.PreferredBillingScreenName = MDVUtility.ToStr(SearchedfieldsJSON["ddlPreferredBillingScreen_text"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlIsExpand") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlIsExpand"]))
                {
                    dr.IsExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsExpand"]) == "0" ? false : true;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlIsSearchCriteriaExpand") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlIsSearchCriteriaExpand"]))
                {
                    dr.IsSearchCriteriaExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsSearchCriteriaExpand"]) == "0" ? false : true;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlDocumentPriority") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDocumentPriority"]))
                {
                    dr.DefaultDocumentPriorityId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDocumentPriority"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlIsDefaultHPI"))
                {
                    dr.IsDefaultHPI = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsDefaultHPI"]) == "0" ? false : true;
                }
                if (SearchedfieldsJSON.ContainsKey("EMCodeTypeIds"))
                {
                    if (SearchedfieldsJSON["EMCodeTypeIds"] == "")
                        dr[dsDefaultsetting.EntityUserOption.EMCodesTypeIdsColumn] = DBNull.Value;
                    else
                        dr.EMCodesTypeIds = MDVUtility.ToStr(SearchedfieldsJSON["EMCodeTypeIds"]);

                    MDVSession.Current.EMCodeTypeIds = SearchedfieldsJSON["EMCodeTypeIds"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkShowSuccessMessages"))
                {
                    dr.IsShowSuccessMessages = MDVUtility.ToStr(SearchedfieldsJSON["chkShowSuccessMessages"]) == "True" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("chkreferralRequired"))
                {
                    dr.IsReferralRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkreferralRequired"]) == "True" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("chkbxPETemplateName"))
                {
                    dr.ISPETemplateNameRequired = MDVUtility.ToStr(SearchedfieldsJSON["chkbxPETemplateName"]) == "True" ? true : false;
                    MDVSession.Current.isPETemplateNameRequired = MDVUtility.ToStr(dr.ISPETemplateNameRequired);
                }


                dr.UserId = MDVSession.Current.AppUserId;
                dr.EntityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                dr.EntityRegCode = MDVSession.Current.EntityRegCode;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.IsDefault = true;
                dr.IsActive = true;
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                //--------- Quick Links Settings IrFan --------
                if (SearchedfieldsJSON.ContainsKey("switchSearchPatient"))
                {
                    dr.IsSearchPatient = SearchedfieldsJSON["switchSearchPatient"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchQuickPat"))
                {
                    dr.IsQuickAddPatient = SearchedfieldsJSON["switchQuickPat"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchMessages"))
                {
                    dr.IsMessage = SearchedfieldsJSON["switchMessages"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchAppointments"))
                {
                    dr.IsAppointment = SearchedfieldsJSON["switchAppointments"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchNotes"))
                {
                    dr.IsNote = SearchedfieldsJSON["switchNotes"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchTasks"))
                {
                    dr.IsTask = SearchedfieldsJSON["switchTasks"];
                }

                if (SearchedfieldsJSON.ContainsKey("switchFax"))
                {
                    dr.IsFax = SearchedfieldsJSON["switchFax"];
                }


                if (SearchedfieldsJSON.ContainsKey("chkPatientName"))
                {
                    dr.IsPatientName = SearchedfieldsJSON["chkPatientName"];
                }

                if (SearchedfieldsJSON.ContainsKey("chkDateOfBirth"))
                {
                    dr.IsDOB = SearchedfieldsJSON["chkDateOfBirth"];
                }

                if (SearchedfieldsJSON.ContainsKey("chkAddress"))
                {
                    dr.IsAddress = SearchedfieldsJSON["chkAddress"];
                }

                if (SearchedfieldsJSON.ContainsKey("chkInsurancePlan"))
                {
                    dr.IsInsurancePlan = SearchedfieldsJSON["chkInsurancePlan"];
                }

                if (SearchedfieldsJSON.ContainsKey("chkSubscriberID"))
                {
                    dr.IsSubscriberID = SearchedfieldsJSON["chkSubscriberID"];
                }

                if (SearchedfieldsJSON.ContainsKey("chkPatientAccountNo"))
                {
                    dr.IsPatientAccountNo = SearchedfieldsJSON["chkPatientAccountNo"];
                }

                if (SearchedfieldsJSON.ContainsKey("chkProvider"))
                {
                    dr.IsProvider = SearchedfieldsJSON["chkProvider"];
                }

                /* Author: Muhammad Azhar Shahzad
                                  Date: Januray 18, 2016
                                  Reason: To show Prescriptions Refill and Pending Prescriptions Quick Links*/
                if (SearchedfieldsJSON.ContainsKey("switchPendingPrescriptions"))
                {
                    dr.IsPendingPrescriptions = SearchedfieldsJSON["switchPendingPrescriptions"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchPrescriptionsRefill"))
                {
                    dr.IsPrescriptionsRefill = SearchedfieldsJSON["switchPrescriptionsRefill"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchImmunizationAlert"))
                {
                    dr.IsImmunizationAlert = SearchedfieldsJSON["switchImmunizationAlert"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchDocumentsAlert"))
                {
                    dr.IsDocumentsAlert = SearchedfieldsJSON["switchDocumentsAlert"];
                }

                if (SearchedfieldsJSON.ContainsKey("chkCurrentMedications"))
                {
                    dr.IsCurrentMedications = SearchedfieldsJSON["chkCurrentMedications"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkActiveProblems"))
                {
                    dr.IsActiveProblems = SearchedfieldsJSON["chkActiveProblems"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkFamilyHistory"))
                {
                    dr.IsFamilyHistory = SearchedfieldsJSON["chkFamilyHistory"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkBirthHistory"))
                {
                    dr.IsBirthHistory = SearchedfieldsJSON["chkBirthHistory"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkPastMedications"))
                {
                    dr.IsPastMedications = SearchedfieldsJSON["chkPastMedications"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkInActiveProblems"))
                {
                    dr.IsInactiveProblems = SearchedfieldsJSON["chkInActiveProblems"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkSocialHistory"))
                {
                    dr.IsSocialHistory = SearchedfieldsJSON["chkSocialHistory"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkHospitalHistory"))
                {
                    dr.IsHospitalizationHistory = SearchedfieldsJSON["chkHospitalHistory"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkActiveProcedures"))
                {
                    dr.IsActiveProcedures = SearchedfieldsJSON["chkActiveProcedures"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkInActiveProcedures"))
                {
                    dr.IsInActiveProcedures = SearchedfieldsJSON["chkInActiveProcedures"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkActiveAllergies"))
                {
                    dr.IsActiveAllergies = SearchedfieldsJSON["chkActiveAllergies"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkVitals"))
                {
                    dr.IsVitals = SearchedfieldsJSON["chkVitals"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkMedicalHistory"))
                {
                    dr.IsMedicalHistory = SearchedfieldsJSON["chkMedicalHistory"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkInActiveAllergies"))
                {
                    dr.IsInactiveAllergies = SearchedfieldsJSON["chkInActiveAllergies"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkImmunizations"))
                {
                    dr.IsImmunizations = SearchedfieldsJSON["chkImmunizations"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkSurgicalHistory"))
                {
                    dr.IsSurgicalHistory = SearchedfieldsJSON["chkSurgicalHistory"];
                }

                if (SearchedfieldsJSON.ContainsKey("ddlAppointmentStatus"))
                {
                    dr.AppointmentStatus = MDVUtility.ToInt64(SearchedfieldsJSON["ddlAppointmentStatus"]);
                }
                if (SearchedfieldsJSON.ContainsKey("ddlSelectNoteComponents"))
                {
                    dr.IsSelectNoteComponents = MDVUtility.ToStr(SearchedfieldsJSON["ddlSelectNoteComponents"]) == "0" ? false : true;
                }
                if (SearchedfieldsJSON.ContainsKey("chkshowICD10"))
                {
                    dr.IsShowICD10 = SearchedfieldsJSON["chkshowICD10"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkFacilityShortName"))
                {
                    dr.IsShowFacilityShortName = SearchedfieldsJSON["chkFacilityShortName"];
                }

                if (SearchedfieldsJSON.ContainsKey("ddlNotePreviewStyle"))
                {
                    dr.NotePrevieStyle = SearchedfieldsJSON["ddlNotePreviewStyle"];
                }
                if (SearchedfieldsJSON.ContainsKey("chkSocPsyandBehaviorHx"))
                {
                    dr.IsSocPsyandBehaviorHx = SearchedfieldsJSON["chkSocPsyandBehaviorHx"];
                }

                if (SearchedfieldsJSON.ContainsKey("ddlCollectionBalance"))
                {
                    dr.IsCollection = MDVUtility.ToStr(SearchedfieldsJSON["ddlCollectionBalance"]) == "1" ? true : false;
                }

                if (SearchedfieldsJSON.ContainsKey("chkPrevComplaints"))
                {
                    dr.IsPrevNoteComplaints = SearchedfieldsJSON["chkPrevComplaints"];
                }

                if (SearchedfieldsJSON.ContainsKey("WorkingDaysList") && !string.IsNullOrEmpty(SearchedfieldsJSON["WorkingDaysList"]))
                {
                    dr.WorkWeekDays = MDVUtility.ToStr(SearchedfieldsJSON["WorkingDaysList"]);
                }

                if (SearchedfieldsJSON.ContainsKey("ddlOrdersExpand") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlOrdersExpand"]))
                {
                    dr.IsOrdersExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlOrdersExpand"]) == "0" ? false : true;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlResultsExpand") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlResultsExpand"]))
                {
                    dr.IsResultsExpand = MDVUtility.ToStr(SearchedfieldsJSON["ddlResultsExpand"]) == "0" ? false : true;
                }

                if (SearchedfieldsJSON.ContainsKey("chkDemographics") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkDemographics"])))
                    dr.IsDemographics = SearchedfieldsJSON["chkDemographics"];
                else
                    dr.IsDemographics = false;
                if (SearchedfieldsJSON.ContainsKey("chkMU3FamilyHistory") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkMU3FamilyHistory"])))
                    dr.IsMu3FamilyHistory = SearchedfieldsJSON["chkMU3FamilyHistory"];
                else
                    dr.IsMu3FamilyHistory = false;
                if (SearchedfieldsJSON.ContainsKey("chkTransmissionkHealthCareSurveys") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransmissionkHealthCareSurveys"])))
                    dr.IsHealthcareSurveys = SearchedfieldsJSON["chkTransmissionkHealthCareSurveys"];
                else
                    dr.IsHealthcareSurveys = false;
                if (SearchedfieldsJSON.ContainsKey("chkTransmitImmunizationRegistries") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransmitImmunizationRegistries"])))
                    dr.IsImmunizationRegistries = SearchedfieldsJSON["chkTransmitImmunizationRegistries"];
                else
                    dr.IsImmunizationRegistries = false;
                if (SearchedfieldsJSON.ContainsKey("chkPatientHealthInformationCapture") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientHealthInformationCapture"])))
                    dr.IsPatientHealthInformationCapture = SearchedfieldsJSON["chkPatientHealthInformationCapture"];
                else
                    dr.IsPatientHealthInformationCapture = false;
                if (SearchedfieldsJSON.ContainsKey("chkTransimissionCaseReporting") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransimissionCaseReporting"])))
                    dr.IsTransimissionCaseReporting = SearchedfieldsJSON["chkTransimissionCaseReporting"];
                else
                    dr.IsTransimissionCaseReporting = false;
                if (SearchedfieldsJSON.ContainsKey("chkTransitionCareDirectProject") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransitionCareDirectProject"])))
                    dr.IsTransitionDirectProject = SearchedfieldsJSON["chkTransitionCareDirectProject"];
                else
                    dr.IsTransitionDirectProject = false;
                if (SearchedfieldsJSON.ContainsKey("chkPrivacyDataSegment") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPrivacyDataSegment"])))
                    dr.IsDataSegmentationPrivacy = SearchedfieldsJSON["chkPrivacyDataSegment"];
                else
                    dr.IsDataSegmentationPrivacy = false;
                if (SearchedfieldsJSON.ContainsKey("chkImplantableDevices") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkImplantableDevices"])))
                    dr.IsImplantableDevices = SearchedfieldsJSON["chkImplantableDevices"];
                else
                    dr.IsImplantableDevices = false;
                if (SearchedfieldsJSON.ContainsKey("chkTransitCancerRegistries") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransitCancerRegistries"])))
                    dr.IsTransitCancerRegistries = SearchedfieldsJSON["chkTransitCancerRegistries"];
                else
                    dr.IsTransitCancerRegistries = false;
                if (SearchedfieldsJSON.ContainsKey("chkConsolidatedCDACreationPreformance") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkConsolidatedCDACreationPreformance"])))
                    dr.IsConsolidatedCDA = SearchedfieldsJSON["chkConsolidatedCDACreationPreformance"];
                else
                    dr.IsConsolidatedCDA = false;
                if (SearchedfieldsJSON.ContainsKey("chkTransimissionAntimicobialUse") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransimissionAntimicobialUse"])))
                    dr.IsTransimissionAntimicobial = SearchedfieldsJSON["chkTransimissionAntimicobialUse"];
                else
                    dr.IsTransimissionAntimicobial = false;
                if (SearchedfieldsJSON.ContainsKey("chkSocPsyBehaviourHx") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkSocPsyBehaviourHx"])))
                    dr.IsMU3SocPsyBehaviourHx = SearchedfieldsJSON["chkSocPsyBehaviourHx"];
                else
                    dr.IsMU3SocPsyBehaviourHx = false;
                if (SearchedfieldsJSON.ContainsKey("chkDataExport") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkDataExport"])))
                    dr.IsDataExport = SearchedfieldsJSON["chkDataExport"];
                else
                    dr.IsDataExport = false;
                if (SearchedfieldsJSON.ContainsKey("chkCarePlan") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCarePlan"])))
                    dr.IsCarePlan = SearchedfieldsJSON["chkCarePlan"];
                else
                    dr.IsCarePlan = false;

                if (SearchedfieldsJSON.ContainsKey("chkPrevNoteROS"))
                {
                    dr.IsPreviousNoteROS = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNoteROS"]) == "True" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("chkPrevNotePE"))
                {
                    dr.IsPreviousNotePE = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNotePE"]) == "True" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlIsSelectCompOnCopyNote"))
                {
                    dr.IsSelectCompOnCopyNote = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsSelectCompOnCopyNote"]) == "0" ? false : true;
                }
                //PRD-31 by:MAHMAD
                if (SearchedfieldsJSON.ContainsKey("ddlIsExpandFolderTree"))
                {
                    dr.IsExpandFolderTree = MDVUtility.ToStr(SearchedfieldsJSON["ddlIsExpandFolderTree"]) == "0" ? false : true;
                }
                //PRD-31 by:MAHMAD
                if (SearchedfieldsJSON.ContainsKey("iTrackDashboardIds") && !string.IsNullOrEmpty(SearchedfieldsJSON["iTrackDashboardIds"]))
                {
                    dr.iTrackDashboardIds = MDVUtility.ToStr(SearchedfieldsJSON["iTrackDashboardIds"]);
                }
                else
                {
                    dr[dsDefaultsetting.EntityUserOption.iTrackDashboardIdsColumn] = DBNull.Value;
                }
                if (SearchedfieldsJSON.ContainsKey("chkPrevNoteProblems"))
                {
                    dr.IsPrevNoteProblems = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNoteProblems"]) == "True" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("ddlCCMTimer"))
                {
                    dr.PBPatientCCMTimer = MDVUtility.ToStr(SearchedfieldsJSON["ddlCCMTimer"]) == "1" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("chkPrevNoteTreatmentPlanComments"))
                {
                    dr.IsPrevNoteTreatmentComents = MDVUtility.ToStr(SearchedfieldsJSON["chkPrevNoteTreatmentPlanComments"]) == "True" ? true : false;
                }

                if (SearchedfieldsJSON.ContainsKey("chkIsNoteCompExpanded"))
                {
                    dr.IsNoteCompExpanded = MDVUtility.ToStr(SearchedfieldsJSON["chkIsNoteCompExpanded"]) == "True" ? true : false;
                }
                if (SearchedfieldsJSON.ContainsKey("chkeAccess") &&
                    !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkeAccess"])))
                    dr.IseAccess = SearchedfieldsJSON["chkeAccess"];
                else
                    dr.IseAccess = false;

                if (SearchedfieldsJSON.ContainsKey("chkePrescribing") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkePrescribing"])))
                    dr.IsePrescribing = SearchedfieldsJSON["chkePrescribing"];
                else
                    dr.IsePrescribing = false;

                if (SearchedfieldsJSON.ContainsKey("chkInCorporateSummaryOfCare") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkInCorporateSummaryOfCare"])))
                    dr.IsInCorporateSummaryOfCare = SearchedfieldsJSON["chkInCorporateSummaryOfCare"];
                else
                    dr.IsInCorporateSummaryOfCare = false;

                if (SearchedfieldsJSON.ContainsKey("chkPatientDocument") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientDocument"])))
                    dr.IsPatientDocument = SearchedfieldsJSON["chkPatientDocument"];
                else
                    dr.IsPatientDocument = false;


                if (SearchedfieldsJSON.ContainsKey("chkPatientEducation") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientEducation"])))
                    dr.IsPatientEducation = SearchedfieldsJSON["chkPatientEducation"];
                else
                    dr.IsPatientEducation = false;


                if (SearchedfieldsJSON.ContainsKey("chkReconciliation") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkReconciliation"])))
                    dr.IsReconciliation = SearchedfieldsJSON["chkReconciliation"];
                else
                    dr.IsReconciliation = false;

                if (SearchedfieldsJSON.ContainsKey("chkSecureMessaging") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkSecureMessaging"])))
                    dr.IsSecureMessaging = SearchedfieldsJSON["chkSecureMessaging"];
                else
                    dr.IsSecureMessaging = false;


                if (SearchedfieldsJSON.ContainsKey("chkTransitionOfCare") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransitionOfCare"])))
                    dr.IsTransitionOfCare = SearchedfieldsJSON["chkTransitionOfCare"];
                else
                    dr.IsTransitionOfCare = false;

                if (SearchedfieldsJSON.ContainsKey("chkViewDownloadTransmit") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkViewDownloadTransmit"])))
                    dr.IsViewDownloadTransmit = SearchedfieldsJSON["chkViewDownloadTransmit"];
                else
                    dr.IsViewDownloadTransmit = false;

                if (SearchedfieldsJSON.ContainsKey("chkTransitionOfCare") &&
                       !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTransitionOfCare"])))
                    dr.IsTransitionOfCare = SearchedfieldsJSON["chkTransitionOfCare"];
                else
                    dr.IsTransitionOfCare = false;

                if (SearchedfieldsJSON.ContainsKey("chkCMS65v7") &&
                   !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS65v7"])))
                    dr.IsCMS65v7 = SearchedfieldsJSON["chkCMS65v7"];
                else
                    dr.IsCMS65v7 = false;

                if (SearchedfieldsJSON.ContainsKey("chkCMS69v6") &&
                  !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS69v6"])))
                    dr.IsCMS69v6 = SearchedfieldsJSON["chkCMS69v6"];
                else
                    dr.IsCMS69v6 = false;

                if (SearchedfieldsJSON.ContainsKey("chkCMS68v7") &&
                  !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS68v7"])))
                    dr.IsCMS68v7 = SearchedfieldsJSON["chkCMS68v7"];
                else
                    dr.IsCMS68v7 = false;

                if (SearchedfieldsJSON.ContainsKey("chkCMS138v6") &&
                  !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS138v6"])))
                    dr.IsCMS138v6 = SearchedfieldsJSON["chkCMS138v6"];
                else
                    dr.IsCMS138v6 = false;

                if (SearchedfieldsJSON.ContainsKey("chkCMS165v6") &&
                  !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS165v6"])))
                    dr.IsCMS165v6 = SearchedfieldsJSON["chkCMS165v6"];
                else
                    dr.IsCMS165v6 = false;

                if (SearchedfieldsJSON.ContainsKey("chkCMS22v6") &&
                  !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCMS22v6"])))
                    dr.IsCMS22v6 = SearchedfieldsJSON["chkCMS22v6"];
                else
                    dr.IsCMS22v6 = false;

                if (SearchedfieldsJSON.ContainsKey("chkDepression") &&
                      !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkDepression"])))
                    dr.IsDepression = SearchedfieldsJSON["chkDepression"];
                else
                    dr.IsDepression = false;

                if (SearchedfieldsJSON.ContainsKey("chkTobacco") &&
                 !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkTobacco"])))
                    dr.IsTobacco = SearchedfieldsJSON["chkTobacco"];
                else
                    dr.IsTobacco = false;

                if (SearchedfieldsJSON.ContainsKey("chkPatientPortalDocument") &&
                 !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkPatientPortalDocument"])))
                    dr.IsPatientPortalDocument = SearchedfieldsJSON["chkPatientPortalDocument"];
                else
                    dr.IsPatientPortalDocument = false;

                if (SearchedfieldsJSON.ContainsKey("chkCDS") &&
                 !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chkCDS"])))
                    dr.IsCDS = SearchedfieldsJSON["chkCDS"];
                else
                    dr.IsCDS = false;
                if (SearchedfieldsJSON.ContainsKey("ddlBlueButtonNote") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["ddlBlueButtonNote"])))
                    dr.IsLandOnComponent = MDVUtility.ToStr(SearchedfieldsJSON["ddlBlueButtonNote"]);
                else
                    dr[dsDefaultsetting.EntityUserOption.IsLandOnComponentColumn] = DBNull.Value;
                if (SearchedfieldsJSON.ContainsKey("ddlDefaultTabMessages") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlDefaultTabMessages"]))
                {
                    dr.DefaultTabMessages = MDVUtility.ToInt64(SearchedfieldsJSON["ddlDefaultTabMessages"]);
                }
                DataTable dtRaceIds = new DataTable();
                DataColumn COLUMN = new DataColumn();
                COLUMN.ColumnName = "Id";
                COLUMN.DataType = typeof(int);
                dtRaceIds.Columns.Add(COLUMN);




                if (SearchedfieldsJSON["RaceIds"] != "")
                {
                    String[] substrings = SearchedfieldsJSON["RaceIds"].Split(',');
                    foreach (var substring in substrings)
                    {
                        DataRow Dr = dtRaceIds.NewRow();
                        Dr[0] = substring;
                        dtRaceIds.Rows.Add(Dr);
                    }
                    MDVSession.Current.RaceIds = SearchedfieldsJSON["RaceIds"];
                }
                else
                {
                    DataRow Dr = dtRaceIds.NewRow();
                    Dr[0] = -1;
                    dtRaceIds.Rows.Add(Dr);
                    MDVSession.Current.RaceIds = "";
                }

                if (SearchedfieldsJSON.ContainsKey("ddlDefTabMedications"))         //PRD-735 TahreeMalik
                {
                    dr.DefaultTabMedications = SearchedfieldsJSON["ddlDefTabMedications"];
                }
                if (SearchedfieldsJSON.ContainsKey("switchAssignedResults"))        //PRD-785 TahreeMalik
                {
                    dr.IsAssignedResults = SearchedfieldsJSON["switchAssignedResults"];
                }

                /* end Change BY Azhar
                                Date: Januray 18, 2016 */
                //--------------------- End Quick Add ------------------


                #region Database Insertion
                dsDefaultsetting.EntityUserOption.AddEntityUserOptionRow(dr);
                BLObject<DSUsers> obj = BLLAdminSecurityObj.InsertDefaultSettings(dsDefaultsetting, dtRaceIds);
                dsDefaultsetting = obj.Data;
                if (obj.Data != null)
                {
                    SetApplicationConfig(dsDefaultsetting);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        EntityUserOptionId = dsDefaultsetting.Tables[dsDefaultsetting.EntityUserOption.TableName].Rows[0][dsDefaultsetting.EntityUserOption.EntityUserOptionIdColumn.ColumnName]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// Sets the application configuration.
        /// </summary>
        /// <param name="objLogin">The object login.</param>
        public static void SetApplicationConfig(DSUsers dsDefaultsetting)
        {
            if (dsDefaultsetting != null)
            {
                if (dsDefaultsetting.EntityUserOption.Rows.Count > 0)
                {
                    MDVSession.Current.DefaultFacilityId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.FacilityIdColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultFacilityName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.FacilityNameColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultProviderId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ProviderIdColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultResourceId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ResourceIdColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultProviderName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ProviderNameColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultBillingProviderId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.BillingProviderIdColumn.ColumnName].ToString();
                    //MDVSession.Current.DefaultBillingProviderName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.BillingProviderNameColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultPracticeId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PracticeIdColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultPracticeName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PracticeNameColumn.ColumnName].ToString();
                    //MDVSession.Current.DefaultResourceId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ResourceIdColumn.ColumnName].ToString();
                    //MDVSession.Current.DefaultResourceName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ResourceNameColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultThemeId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ThemeIdColumn].ToString();
                    MDVSession.Current.DefaultThemeName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ThemeNameColumn].ToString();
                    MDVSession.Current.EntityUserOptionId = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.EntityUserOptionIdColumn.ColumnName].ToString();
                    MDVSession.Current.PreferredScreen = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PreferredScreenColumn.ColumnName].ToString();
                    MDVSession.Current.PreferredSchScreen = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PreferredSchScreenColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultTemplate = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.DefaultTemplateColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultSuperBill = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.DefaultSuperBillColumn.ColumnName].ToString();
                    MDVSession.Current.PBPhoneNumber1 = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPhoneNumber1Column.ColumnName].ToString();
                    MDVSession.Current.PBPhoneNumber2 = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPhoneNumber2Column.ColumnName].ToString();
                    MDVSession.Current.PBPCP = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPCPColumn.ColumnName].ToString();
                    MDVSession.Current.PBRefProvider = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBRefProviderColumn.ColumnName].ToString();
                    MDVSession.Current.PBPlanBalance = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPlanBalanceColumn.ColumnName].ToString();
                    MDVSession.Current.PBPatientBalance = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPatientBalanceColumn.ColumnName].ToString();
                    MDVSession.Current.IsPBCollection = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCollectionColumn.ColumnName].ToString();
                    MDVSession.Current.ClaimPrinter = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ClaimPrinterColumn.ColumnName].ToString();
                    MDVSession.Current.ClaimTray = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ClaimTrayColumn.ColumnName].ToString();
                    MDVSession.Current.AttachmentPrinter = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.AttachmentPrinterColumn.ColumnName].ToString();
                    MDVSession.Current.AttachmentTray = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.AttachmentTrayColumn.ColumnName].ToString();
                    MDVSession.Current.PreferredSchScreenName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PreferredSchScreenNameColumn.ColumnName].ToString();
                    MDVSession.Current.RefreshTime = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.RefreshTimeColumn.ColumnName].ToString();
                    string EntityUserRefreshTime = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.RefreshTimeColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultThemeName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ThemeNameColumn.ColumnName].ToString();
                    MDVSession.Current.FavListNames = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.FavListNamesColumn.ColumnName]);
                    MDVSession.Current.FreeTextNames = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.FreeTextICDColumn.ColumnName]);
                    //MDVSession.Current.PBPhone1 = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPhoneNumber1Column.ColumnName]);
                    //MDVSession.Current.PBPhone2 = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPhoneNumber2Column.ColumnName]);

                    MDVSession.Current.PBPatientAdvanceBalance = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPatientAdvanceBalanceColumn.ColumnName]);
                    MDVSession.Current.PBPrimaryInsurance = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPrimaryInsuranceColumn.ColumnName]);
                    MDVSession.Current.PreferredScreenName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PreferredScreenNameColumn.ColumnName].ToString();
                    MDVSession.Current.ENMCodesTime = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.ENMCodesTimeColumn.ColumnName].ToString();
                    MDVSession.Current.NoteFontSize = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.NoteFontSizeColumn.ColumnName].ToString();
                    MDVSession.Current.PreferredBillingScreen = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PreferredBillingScreenColumn.ColumnName].ToString();
                    MDVSession.Current.PreferredBillingScreenName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PreferredBillingScreenNameColumn.ColumnName].ToString();
                    MDVSession.Current.IsDefaultHPI = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsDefaultHPIColumn.ColumnName].ToString();
                    MDVSession.Current.IsShowFacilityShortName = dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsShowFacilityShortNameColumn.ColumnName].ToString();
                    MDVSession.Current.DefaultSchedulerTimeInterval = MDVUtility.ToInt32(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.SchedulerTimeIntervalColumn.ColumnName]);
                    //MDVSession.Current.IsPrevNoteComplaint = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsPrevNoteComplaintColumn.ColumnName]);
                    MDVSession.Current.IsShowSuccessMessages = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsShowSuccessMessagesColumn.ColumnName]);
                    MDVSession.Current.IsReferralRequired = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsReferralRequiredColumn.ColumnName]);
                    MDVSession.Current.IsOrdersExpand = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsOrdersExpandColumn.ColumnName]);
                    MDVSession.Current.IsResultsExpand = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsResultsExpandColumn.ColumnName]);
                    MDVSession.Current.NotePrevieStyle = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.NotePrevieStyleColumn.ColumnName]);


                    MDVSession.Current.isDemographics = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsDemographicsColumn.ColumnName]) == true ? "True" : "False";
                    MDVSession.Current.isMU3FamilyHistory = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsMu3FamilyHistoryColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isTransPubHealthAgHealthCareSurveys = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsHealthcareSurveysColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isTransmittoImmunizationRegistries = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsImmunizationRegistriesColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isPatientHealthInformationCapture = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsPatientHealthInformationCaptureColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isTransPubHealthAgCaseReporting = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsTransimissionCaseReportingColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isTransitionCareDirectProject = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsTransitionDirectProjectColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isImplantableDevices = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsImplantableDevicesColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isTransitonCancerRegistries = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsTransitCancerRegistriesColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isConsolidatedCDACreationPreformance = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsConsolidatedCDAColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isTransPubHealthAgAntimicobialUse = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsTransimissionAntimicobialColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isMU3SocPsycBehaviourHx = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsMU3SocPsyBehaviourHxColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isDataSegmentationPrivacy = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsDataSegmentationPrivacyColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isDataExport = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsDataExportColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.isCarePlan = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCarePlanColumn.ColumnName]) == true ? "True" : "False"; ;

                    MDVSession.Current.IsPreviousNotePE = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsPreviousNotePEColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.IsPreviousNoteROS = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsPreviousNoteROSColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.IsPreviousNoteComplaints = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsPrevNoteComplaintsColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.IsPreviousNoteProblems = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsPrevNoteProblemsColumn.ColumnName]) == true ? "True" : "False"; ;
                    MDVSession.Current.PBPatientCCMTimer = MDVUtility.ToStr(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.PBPatientCCMTimerColumn.ColumnName]);
                    MDVSession.Current.IsPrevNoteTreatmentComents = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsPrevNoteTreatmentComentsColumn.ColumnName]) == true ? "True" : "False"; ;

                    MDVSession.Current.IsCMS65v7 = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCMS65v7Column.ColumnName]) == true ? "True" : "False";
                    MDVSession.Current.IsCMS69v6 = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCMS69v6Column.ColumnName]) == true ? "True" : "False";
                    MDVSession.Current.IsCMS68v7 = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCMS68v7Column.ColumnName]) == true ? "True" : "False";
                    MDVSession.Current.IsCMS138v6 = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCMS138v6Column.ColumnName]) == true ? "True" : "False";
                    MDVSession.Current.IsCMS165v6 = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCMS165v6Column.ColumnName]) == true ? "True" : "False";
                    MDVSession.Current.IsCMS22v6 = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCMS22v6Column.ColumnName]) == true ? "True" : "False";
                    MDVSession.Current.IsCDS = MDVUtility.ToBool(dsDefaultsetting.EntityUserOption.Rows[0][dsDefaultsetting.EntityUserOption.IsCDSColumn.ColumnName]) == true ? "True" : "False";

                    if (EntityUserRefreshTime != "")
                    {
                        MDVSession.Current.RefreshTime = EntityUserRefreshTime;
                    }
                    else
                    {
                        //Default Refresh Time for DashBoard/Message/Task Count
                        MDVSession.Current.RefreshTime = "5";
                    }

                }
            }
        }

        private string UpdateUserPassword(int UserID, string UserJSON, bool IsAdmin)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(UserJSON);

                DSUsers dsUsers = null;
                BLObject<DSUsers> obj = BLLAdminSecurityObj.LoadModuleFormUser(UserID, 0, 0);
                if (obj.Data != null)
                {
                    dsUsers = obj.Data;
                    if (dsUsers.Users.Rows.Count > 0)
                    {
                        DSUsers.UsersRow dr = (DSUsers.UsersRow)dsUsers.Users.Rows[0];
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName.ToUpper()) == AppPrivileges.DefaultUser)
                        {

                            BLObject<string> regexObj = BLLAdminSecurityObj.GetPasswordRegex(UserID);

                            if (!MDVUtility.IsStringValidForRegex(MDVUtility.ToStr(SearchedfieldsJSON["txtnewpwd"]), regexObj.Data))
                            {
                                throw new Exception("Password did not meet the complexity Criteria");
                            }

                        }
                        if ((MDVUtility.ToStr(dr.UserPassword) == MDVUtility.EncryptToSHA256(MDVUtility.ToStr(SearchedfieldsJSON["txtoldpwd"]), dr.UserName)) || IsAdmin == true)
                        {
                            if (MDVUtility.ToStr(SearchedfieldsJSON["txtnewpwd"]) == MDVUtility.ToStr(SearchedfieldsJSON["txtconpwd"]))
                            {
                                //update password
                                dr.UserPassword = MDVUtility.EncryptToSHA256(MDVUtility.ToStr(SearchedfieldsJSON["txtnewpwd"]), dr.UserName);
                                if (dr.UserName.ToUpper() == MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper())
                                {
                                    dr.isFirstTimeloggedIn = true;
                                }
                                else { dr.isFirstTimeloggedIn = false; }
                                obj = BLLAdminSecurityObj.UpdateUserPassword(ref dsUsers);
                                if (obj.Data != null)
                                {
                                    var response = new
                                    {
                                        status = true,
                                        IsValidate = false,
                                        Message = Common.AppPrivileges.Password_Change_Success
                                    };
                                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                                }
                                else
                                {
                                    var response = new
                                    {
                                        status = false,
                                        IsValidate = false,
                                        Message = obj.Message
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                }
                            }
                            else
                            {
                                var response = new
                                {
                                    status = false,
                                    IsValidate = true,
                                    Message = "New & Confirm Password not match"
                                };
                                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                IsValidate = true,
                                Message = "Old Password is not correct"
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            IsValidate = false,
                            Message = AppPrivileges.No_Record_Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        IsValidate = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    IsValidate = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }
        private string GetPrinters()
        {
            try
            {
                DataTable dtprinter = new DataTable();
                dtprinter.Columns.Add("PrinterId", typeof(string));
                dtprinter.Columns.Add("PrinterName", typeof(string));

                System.Management.ManagementScope objMS =
                   new System.Management.ManagementScope(ManagementPath.DefaultPath);
                objMS.Connect();

                SelectQuery objQuery = new SelectQuery("SELECT * FROM Win32_Printer");
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher(objMS, objQuery);
                System.Management.ManagementObjectCollection objMOC = objMOS.Get();
                int a = 0;
                foreach (ManagementObject Printers in objMOC)
                {

                    if (Convert.ToBoolean(Printers["Local"]))       // LOCAL PRINTERS.
                    {
                        a++;
                        dtprinter.Rows.Add(a.ToString(), Printers["Name"].ToString());
                    }
                    if (Convert.ToBoolean(Printers["Network"]))     // ALL NETWORK PRINTERS.
                    {
                        a++;
                        dtprinter.Rows.Add(a.ToString(), Printers["Name"].ToString());
                    }
                }
                var response = new
                {
                    status = true,
                    Printer_JSON = MDVUtility.JSON_DataTable(dtprinter),

                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string LoadWidgetAndKPI(long dBSId)
        {
            try
            {
                DSSettings dsSettings = null;
                BLObject<DSSettings> obj;
                obj = BLLAdminSecurityObj.LoadDashBoardSetting(dBSId, MDVUtility.ToLong(MDVSession.Current.AppUserId), null);
                if (obj.Data != null)
                {
                    dsSettings = obj.Data;
                    DataTable DtWidget = dsSettings.DashboardSettings.Clone();
                    DataTable DtKPI = dsSettings.DashboardSettings.Clone();
                    Dictionary<string, string> KPICharts = new Dictionary<string, string>();
                    Dictionary<string, string> WidgetData = new Dictionary<string, string>();

                    foreach (DataRow dr in dsSettings.DashboardSettings.Rows)
                    {
                        string kpiName = dr[dsSettings.DashboardSettings.KPINameColumn.ColumnName].ToString();

                        if (dr[dsSettings.DashboardSettings.DBSTypeColumn.ColumnName].ToString().Contains("W"))
                        {
                            //Payments
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Payments")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Payments", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "TCM")
                            {
                                dr["PermissionAllowed"] = "YES";
                                DtWidget.Rows.Add(dr.ItemArray);

                            }
                            //Appointments
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Appointments")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointments", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Modified Notes
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Modified Notes")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modified Notes", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Messages
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Messages")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Messages", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Notes
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Encounter")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Tasks
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Tasks")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Documents
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Documents")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "CCM")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("CCM", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Copayment
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Copayment")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Copayment", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }

                            //Orders & Results
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Orders & Results")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Patient Changes
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Patient Changes")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Changes", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Referrals
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Referrals")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referrals", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //Portal Requests
                            //if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Portal Requests")
                            //{
                            //    string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Portal Requests", "VIEW", "Dash Board")).ToString();
                            //    if (string.IsNullOrEmpty(privilegasMessage))
                            //    {
                            //        dr["PermissionAllowed"] = "YES";
                            //        DtWidget.Rows.Add(dr.ItemArray);

                            //    }
                            //    else
                            //    {
                            //        dr["PermissionAllowed"] = "NO";
                            //        DtWidget.Rows.Add(dr.ItemArray);

                            //    }
                            //}
                            //Live Requests
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Live Requests")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Live Requests", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }
                            //--------------- Active Accounts
                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Active Accounts")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Active Accounts", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);
                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);
                                }
                            }

                            if (dr[dsSettings.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Patient Portal Requests")
                            {
                                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Portal Requests", "VIEW", "Dash Board")).ToString();
                                if (string.IsNullOrEmpty(privilegasMessage))
                                {
                                    dr["PermissionAllowed"] = "YES";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                                else
                                {
                                    dr["PermissionAllowed"] = "NO";
                                    DtWidget.Rows.Add(dr.ItemArray);

                                }
                            }

                        }
                        else if (dr[dsSettings.DashboardSettings.DBSTypeColumn.ColumnName].ToString().Contains("K"))
                        {
                            DtKPI.Rows.Add(dr.ItemArray);
                        }
                    }

                    var response = new
                    {
                        status = true,
                        DASHBOARDSETTING_WIDGET_JSON = MDVUtility.JSON_DataTable(DtWidget),
                        DASHBOARDSETTING_KPI_JSON = MDVUtility.JSON_DataTable(DtKPI),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "UPDATE_DASHBOARDSETTING":
                    {
                        string DBSId = context.Request["DBSId"];
                        string IsActive = context.Request["IsActive"];


                        string strJSONData = UpdateDashboardSetting(MDVUtility.ToInt(DBSId), MDVUtility.ToStr(IsActive));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_ALL_DASHBOARDSETTING":
                    {
                        string AllSetting = context.Request["data"];
                        string strJSONData = UpdateAllDashboardSetting(AllSetting);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;


                case "PRINTER":
                    {

                        string strJSONData = GetPrinters();

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_DEFAULT_SETTING":
                    {

                        string fieldsJSON = MDVUtility.ReplaceSpecialCharacters(context.Request["DefaultSettingData"]);
                        string strJSONData = SaveDefaultSetting(fieldsJSON);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_DEFAULT_SETTING":
                    {

                        string fieldsJSON = MDVUtility.ReplaceSpecialCharacters(context.Request["DefaultSettingData"]);
                        int EntityUserOptionId = MDVUtility.ToInt32(context.Request["EntityUserOptionId"]);
                        string strJSONData = UpdateDefaultSetting(fieldsJSON, EntityUserOptionId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_DEFAULT_SETTINGS":
                    {
                        string userName = MDVUtility.ToStr(context.Request["userName"]);
                        string strJSONData = FillDefaultSetting(userName);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);

                    }
                    break;
                case "FILL_SHOW_ICD10":
                    {
                        string userName = MDVUtility.ToStr(context.Request["userName"]);
                        string strJSONData = GetEntityUserOptionShowICD10(userName); ;
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_USER_PASSWORD":
                    {
                        string UserID = context.Request["UserID"];
                        string UserData = context.Request["UserData"];
                        Boolean IsAdmin = Convert.ToBoolean(context.Request["IsAdmin"]);
                        string strJSONData = UpdateUserPassword(MDVUtility.ToInt(UserID), MDVUtility.ToStr(UserData), IsAdmin);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_WIDGET_AND_KPI":
                    {
                        Int64 DBSId = MDVUtility.ToInt64(context.Request["DBSId"]);
                        string strJSONData = LoadWidgetAndKPI(DBSId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }


        #endregion
    }
}
