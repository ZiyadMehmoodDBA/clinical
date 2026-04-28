Patient_Audit_Log = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Patient_Audit_Log.bIsFirstLoad) {
            Patient_Audit_Log.params = params;
            if (Patient_Audit_Log.params.PanelID != 'pnlPatientAuditLog') {
                Patient_Audit_Log.params.PanelID = Patient_Audit_Log.params.PanelID + ' #pnlPatientAuditLog';
            } else {
                Patient_Audit_Log.params.PanelID = 'pnlPatientAuditLog';
            }
            Patient_Audit_Log.SelectTab('Activity_Log');
            Patient_Audit_Log.bIsFirstLoad = false;
        }
        else {
            var activeTab = $("#" + Patient_Audit_Log.params.PanelID + " #ulAuditTabs li.active").text();
            if (activeTab == "General") {
                Patient_Audit_Log.SelectTab("Activity_Log");
            }
           else if (activeTab == "Reminder") {
                Patient_Audit_Log.SelectTab("Patient_Reminder_Log");
            }
            else if (activeTab == "Fax") {
                Patient_Audit_Log.SelectTab("Patient_Fax_Log");
            }
        }
    },
    SelectTab: function (Type) {
        Patient_Audit_Log.Type = Type;
        $("#" + Patient_Audit_Log.params.PanelID + " #ulAuditTabs li").removeClass("active");
        $("#" + Patient_Audit_Log.params.PanelID + " #General,#Reminder,#Fax").removeClass("active");
            if (Type == "Activity_Log")
            {
                $("#" + Patient_Audit_Log.params.PanelID + " #ulAuditTabs" + " #liGeneral").addClass("active");
                $("#" + Patient_Audit_Log.params.PanelID + " #General").addClass("active");
                $("#" + Patient_Audit_Log.params.PanelID + " #General").empty();
                Patient_Audit_Log.LoadSelectedTabPage("Activity_Log");
            }
            else if (Type == "Patient_Reminder_Log") {
                $("#" + Patient_Audit_Log.params.PanelID + " #ulAuditTabs" + " #liReminder").addClass("active");
                $("#" + Patient_Audit_Log.params.PanelID + " #Reminder").addClass("active");
                $("#" + Patient_Audit_Log.params.PanelID + " #Reminder").empty();
                Patient_Audit_Log.LoadSelectedTabPage("Patient_Reminder_Log");
            }
            else if (Type == "Patient_Fax_Log") {
                $("#" + Patient_Audit_Log.params.PanelID + " #ulAuditTabs" + " #liFax").addClass("active");
                $("#" + Patient_Audit_Log.params.PanelID + " #Fax").addClass("active");
                $("#" + Patient_Audit_Log.params.PanelID + " #Fax").empty();
                Patient_Audit_Log.LoadSelectedTabPage("Patient_Fax_Log");
            }
        
       

    },
    LoadSelectedTabPage: function (CtrltName) {
        var Tab = GetTab(CtrltName);
        var ajax_get = $.get(Tab.Path, {
            cache: false
        }, function (content) {
            var myDiv = $("<div></div>").append(content);
            html = String(myDiv.first("div").html());
            var params = [];
            if (Tab.TabID == "Activity_Log") {

                $("#" + Patient_Audit_Log.params.PanelID + " #General").append(html);
                $("#" + Patient_Audit_Log.params.PanelID + " #General #pnlActivityLog").css("display", "block");
                params = Patient_Audit_Log.params;
            }
            else if (Tab.TabID == "Patient_Reminder_Log") {
                
                $("#" + Patient_Audit_Log.params.PanelID + " #Reminder").append(html);
                $("#" + Patient_Audit_Log.params.PanelID + " #Reminder #pnlPatientReminderLog").css("display", "block");
                params = Patient_Audit_Log.params;
            }
            else if (Tab.TabID == "Patient_Fax_Log") {

                $("#" + Patient_Audit_Log.params.PanelID + " #Fax").append(html);
                $("#" + Patient_Audit_Log.params.PanelID + " #Fax #pnlPatientFaxLog").css("display", "block");
                params = Patient_Audit_Log.params;
            }
            eval(Tab.ContainerControlID + '.Load')(params);

        }, "html");
    },
    UnLoad: function () {

    },

}