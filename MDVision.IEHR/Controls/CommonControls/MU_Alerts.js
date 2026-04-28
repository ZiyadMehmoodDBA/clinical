MU_Alerts = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        MU_Alerts.params = params;

        if (MU_Alerts.bIsFirstLoad) {
            MU_Alerts.bIsFirstLoad = false;
            MU_Alerts.params.PanelID = "pnlMUAlerts";
            MU_Alerts.LoadMU3Alert();

            if (MU_Alerts.params["SelectedTab"]) {
                MU_Alerts.activeTab(MU_Alerts.params["SelectedTab"])
            }
        }
    },

    LoadMU3Alert: function () {
        MU_Alerts.LoadMUAlert_DBCALL(MU_Alerts.params.PatientID, true, null).done(function (res) {
            if (res.status) {
                var data = JSON.parse(res.MUAlerts_JSON);

                var eCQMAlerts = data.filter(item=>item.Type == "eCQM");

                var Demographics = data.filter(item=>item.ProfileName == "Demographics");
                var HealthCare = data.filter(item=>item.ProfileName == "Healthcare Surveys");
                var SocialHx = data.filter(item=>item.ProfileName == "SocialHx");
                var PatientEducation = data.filter(item=>item.ProfileName == "Patient Education");
                var Reconciliation = data.filter(item=>item.ProfileName == "Reconciliation");
                var ePrescribing = data.filter(item=>item.ProfileName == "ePrescribing");
                var eAccess = data.filter(item=>item.ProfileName == "eAccess");
                var ViewDownloadTransmit = data.filter(item=>item.ProfileName == "ViewDownloadTransmit");
                var SecureMessaging = data.filter(item=>item.ProfileName == "SecureMessaging");
                var TransitionOfCare = data.filter(item=>item.ProfileName == "TransitionOfCare");
                var PatientPortalDocuments = data.filter(item=>item.ProfileName == "Patient Portal Document");
                var ReceiveAndIncorporate = data.filter(item=>item.ProfileName == "InCorporateSummaryOfCare");
                var Depression = data.filter(item=>item.ProfileName == "Depression Screening");
                var PatientHealthData = data.filter(item=>item.ProfileName == "Patient Document");
                var Diabetes = data.filter(item=>item.ProfileName == "Diabetes Screening");
                var Tabacoo = data.filter(item=>item.ProfileName == "Tobacco Screening");
                var IsCDS = data.filter(item=>item.ProfileName == "CDS Alert");

                if (Demographics.length > 0) {
                    $("#" + MU_Alerts.params.PanelID + " #div_demographic").css("display", "block");
                    var fields = Demographics[0].Fields.split(",");
                    for (var i = 0; i < fields.length; i++) {
                        if ($("#" + MU_Alerts.params.PanelID + " #div_demographic").find("span[val_^='" + fields[i] + "']")) {
                            $("#" + MU_Alerts.params.PanelID + " #div_demographic").find("span[val_^='" + fields[i] + "']").removeClass("green");
                            $("#" + MU_Alerts.params.PanelID + " #div_demographic").find("span[val_^='" + fields[i] + "']").addClass("red");
                        }
                    }
                }
                if (HealthCare.length > 0) {
                    $("#" + MU_Alerts.params.PanelID + " #div_Healthcare").css("display", "block");
                    var fields = HealthCare[0].Fields.split(",");
                    for (var i = 0; i < fields.length; i++) {
                        if ($("#" + MU_Alerts.params.PanelID + " #div_Healthcare").find("span[val_^='" + fields[i] + "']")) {
                            $("#" + MU_Alerts.params.PanelID + " #div_Healthcare").find("span[val_^='" + fields[i] + "']").removeClass("green");
                            $("#" + MU_Alerts.params.PanelID + " #div_Healthcare").find("span[val_^='" + fields[i] + "']").addClass("red");
                        }
                    }
                }
                if (SocialHx.length > 0) {
                    $("#" + MU_Alerts.params.PanelID + " #div_CareReporting").css("display", "block");
                    var fields = SocialHx[0].Fields.split(",");
                    for (var i = 0; i < fields.length; i++) {
                        if ($("#" + MU_Alerts.params.PanelID + " #div_CareReporting").find("span[val_^='" + fields[i] + "']")) {
                            $("#" + MU_Alerts.params.PanelID + " #div_CareReporting").find("span[val_^='" + fields[i] + "']").removeClass("green");
                            $("#" + MU_Alerts.params.PanelID + " #div_CareReporting").find("span[val_^='" + fields[i] + "']").addClass("red");
                        }
                    }
                }
                if (PatientEducation.length > 0 && PatientEducation[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_PatientEducation").css("display", "block");
                }
                if (Reconciliation.length > 0 && Reconciliation[0].IsShowAlert == true && Reconciliation[0].ProviderId + "" == globalAppdata.DefaultProviderId) {
                    $("#" + MU_Alerts.params.PanelID + " #div_Reconciliation").css("display", "block");
                }
                if (ePrescribing.length > 0 && ePrescribing[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_ePrescribing").css("display", "block");
                    $("#" + MU_Alerts.params.PanelID + " #ePrescriptionCount").html(ePrescribing.length);
                    var lis = "";
                    var Visits = [{ NotesId: 0, Count: 0 }];
                    $.each(ePrescribing, function () {

                        if (Visits.filter(item=>item.NotesId == this.NotesId).length <= 0) {
                            Visits.push({ NotesId: this.NotesId, Count: 1 });
                            lis += "<li>" + this.NoteDate + " " + this.NoteTime + "<span id=\"eP_" + this.NotesId + "\" style=\"margin-left: 5px;\" class=\"badge\">1</span></li>";
                        }
                        else {
                            var obj = Visits.filter(item=>item.NotesId == this.NotesId);
                            obj[0].Count = obj[0].Count + 1;
                        }

                    });

                    $("#" + MU_Alerts.params.PanelID + " #ePrescription").html(lis);
                    $.each(Visits, function () {
                        $("#" + MU_Alerts.params.PanelID + " #eP_" + this.NotesId).html(this.Count);
                    });


                }
                if (eAccess.length > 0 && eAccess[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_eAccess").css("display", "block");
                }
                if (ViewDownloadTransmit.length > 0 && ViewDownloadTransmit[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_ViewDownloadTransmit").css("display", "block");
                }
                if (SecureMessaging.length > 0 && SecureMessaging[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_SecureMessaging").css("display", "block");
                }
                if (TransitionOfCare.length > 0 && TransitionOfCare[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_TransitionOfCare").css("display", "block");
                    $("#" + MU_Alerts.params.PanelID + " #TransitionofCareCount").html(TransitionOfCare.length);
                    var lis = "";
                    $.each(TransitionOfCare, function () {
                        lis += "<li>" + this.NoteDate + " " + this.NoteTime + "</li>";
                    });

                    $("#" + MU_Alerts.params.PanelID + " #TOCVisits").html(lis);
                }
                if (Depression.length > 0 && Depression[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_PatDepressionScreening").css("display", "block");
                }
                if (PatientPortalDocuments.length > 0 && PatientPortalDocuments[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_PatPortalDocument").css("display", "block");
                }
                if (PatientHealthData.length > 0 && PatientHealthData[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_PatHealthData").css("display", "block");
                }
                if (ReceiveAndIncorporate.length > 0 && ReceiveAndIncorporate[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_ReceiveAndIncorporate").css("display", "block");
                }
                if (Diabetes.length > 0 && Diabetes[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_PatDiabetesScreening").css("display", "block");
                }
                if (Tabacoo.length > 0 && Tabacoo[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_PatTabaccoScreening").css("display", "block");
                }
                if (IsCDS.length > 0 && IsCDS[0].IsShowAlert == true) {
                    $("#" + MU_Alerts.params.PanelID + " #div_PatCDSAlert").css("display", "block");
                }
                MU_Alerts.DraweCQMAlerts(eCQMAlerts);
                MU_Alerts.MissingDataAlertCount();
            }
            else {
                utility.DisplayMessages(res.Message, 2);
            }
        });
    },

    DraweCQMAlerts: function (eCQMAlerts) {

        var filtered_Alerts = [];
        $.each(eCQMAlerts, function () {
            var $this = this;
            var isExists = filtered_Alerts.filter(function (obj) {
                return obj.ProfileName == $this.ProfileName;
            });
            if (isExists.length > 0) {
                isExists[0]["Visits"].push($this.NoteDate + " " + $this.NoteTime);
            }
            else {
                $this["Visits"] = [];
                $this["Visits"].push($this.NoteDate + " " + $this.NoteTime);
                filtered_Alerts.push($this);
            }
        });


        var Template = $('<div class="form-group"></div>');
        for (var i = 0; i < filtered_Alerts.length; i++) {
            var IsHighPriority = filtered_Alerts[i].IsHighPriority && (filtered_Alerts[i].IsHighPriority) + "".toLowerCase() == "true" ? "Yes" : "No";
            var Visits = "";
            $.each(filtered_Alerts[i].Visits, function () {
                Visits += "<li>" + this + "</li>"
            });

            var parser = new DOMParser;
            var dom = parser.parseFromString('<!doctype html><body>' + filtered_Alerts[i].Process, 'text/html');
            var decodedString = dom.body.textContent;

            var div_ = document.createElement("div");
            div_.innerHTML = decodedString;

            var item = '<div id="div_eCQM' + filtered_Alerts[i].AlertId + '" data-plugin-toggle style="display:block">'
                                            + '<section class="toggle">'
                                                + '<label>'
                                                    + filtered_Alerts[i].ProfileName
                                                    + '<span  class="badge pull-right" style="margin: 5px;">' + filtered_Alerts[i].Visits.length + '</span>'
                                               + ' </label>'
                                               + ' <div class="toggle-content">'
                                                  + '  <div class="container-alert">'
                                                 + '<b>Visit Date</b>'
                                                 + '<ul style="list-style:none;">'
                                                        + Visits
                                                 + '</ul>'
                                                    + '<table class="table table-bordered table-striped table-condensed table-hover mb-none" id="dgveCQM_' + filtered_Alerts[i].AlertId + '">'
                                                            + '<thead>'
                                                               + ' <tr>'
                                                                  + '  <th style="width:50%;">Measure Description</th>'
                                                                  + '  <th style="width:50%;">Measure Process</th>'
                                                               + ' </tr>'
                                                           + '</thead>'
                                                          + '<tbody>'
                                                             + '<td><p>' + filtered_Alerts[i].Fields + '</p><br /><span class="pull-left" >Measure Type:<b>&nbsp; ' + filtered_Alerts[i].MeasureType + '</b></span><span class="pull-right" >High Priority:<b>&nbsp; ' + IsHighPriority + '</b></span></td>'
                                                             + '<td><p>' + div_.innerHTML + '</p></td>'
                                                          + '</tbody>'
                                                      + '  </table>'
                                                      + '<div class="checkbox-custom checkbox-default pull-right" style="margin: 10px;display:none">'
                                                          + ' <input type="checkbox" name="donotshow_eCQM" alertId="' + filtered_Alerts[i].AlertId + '" id="donotshow_eCQM' + filtered_Alerts[i].AlertId + '">'
                                                          + ' <label class="control-label" for="donotshow_eCQM' + filtered_Alerts[i].AlertId + '">Ignore</label>'
                                                     + ' </div>'
                                                    + '</div>'
                                               + ' </div>'
                                          + '  </section>'
                                       + ' </div>';
            Template.append(item);
        }
        Template.append('<div class="clearfix"></div>');
        $("#" + MU_Alerts.params.PanelID + " #CQMAlerts").html(Template);

        $($("#" + MU_Alerts.params.PanelID + " #CQMAlerts").find($('[data-plugin-toggle]'))).each(function () {
            var $this = $(this),
				opts = {};
            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;
            $this.themePluginToggle(opts);
        });


    },

    UpdateMUAlertProfile: function (ProfileName, NotesId, PatientId, IsShowAlert, ProviderId, PendingPrescriptionIDs, Istoggel) {

        var MU_ARRAY = [];
        var Profile_array = ProfileName.indexOf(',') > 0 ? ProfileName.split(',') : [];
        if (Profile_array.length <= 0)
            Profile_array.push(ProfileName);

        if (Istoggel == false)
            Istoggel = false;
        else
            Istoggel = true;

        for (var i = 0; i < Profile_array.length; i++) {
            var profilename = Profile_array[i];
            if (profilename) {
                //Transition of Care
                if (profilename == "TransitionOfCare" && NotesId && PatientId) {
                    var obj = {
                        ProfileName: profilename,
                        Fields: "",
                        NotesId: NotesId,
                        PatientId: PatientId,
                        IsShowAlert: IsShowAlert != undefined ? IsShowAlert : false,
                        Type: "MU3"
                    };
                    MU_ARRAY.push(obj);
                }
                //Patient Electronic Access
                if (profilename == "eAccess" && PatientId) {
                    var obj = {
                        ProfileName: profilename,
                        Fields: "",
                        NotesId: 0, // send NotesId 0 deliberately, because it is not Note based alert.
                        PatientId: PatientId,
                        IsShowAlert: IsShowAlert != undefined ? IsShowAlert : false,
                        Type: "MU3"
                    };
                    MU_ARRAY.push(obj);
                }
                //Electronic Prescribing
                if (profilename == "ePrescribing" && NotesId && PatientId && PendingPrescriptionIDs.length > 0) {
                    for (var j = 0; j < PendingPrescriptionIDs.length; j++) {
                        var obj = {
                            ProfileName: profilename,
                            Fields: "",
                            NotesId: NotesId,
                            PatientId: PatientId,
                            PrescriptionId: PendingPrescriptionIDs[j].toString(),
                            IsShowAlert: IsShowAlert != undefined ? IsShowAlert : false,
                            Type: "MU3"
                        };
                        MU_ARRAY.push(obj);
                    }
                }
                //Reconciliation
                if (profilename == "Reconciliation" && ProviderId && PatientId) {
                    var obj = {
                        ProfileName: profilename,
                        Fields: "",
                        NotesId: 0,
                        PatientId: PatientId,
                        ProviderId: ProviderId,
                        IsShowAlert: IsShowAlert != undefined ? IsShowAlert : false,
                        Type: "MU3"
                    };
                    MU_ARRAY.push(obj);
                }
                //SecureMessaging
                if (profilename == "SecureMessaging" && PatientId) {
                    var obj = {
                        ProfileName: profilename,
                        Fields: "",
                        NotesId: 0,
                        PatientId: PatientId,
                        IsShowAlert: IsShowAlert != undefined ? IsShowAlert : false,
                        Type: "MU3"
                    };
                    MU_ARRAY.push(obj);
                }
                //InCorporateSummaryOfCare
                if (profilename == "InCorporateSummaryOfCare" && PatientId) {
                    var obj = {
                        ProfileName: profilename,
                        Fields: "",
                        NotesId: 0,
                        PatientId: PatientId,
                        IsShowAlert: IsShowAlert != undefined ? IsShowAlert : false,
                        Type: "MU3"
                    };
                    MU_ARRAY.push(obj);
                }
            }
        }



        if (MU_ARRAY.length > 0) {
            Patient_Demographic.UpdateMUAlert(MU_ARRAY).done(function (result) {
                if (result.status != false) {
                    var data = JSON.parse(result.MUAlerts_JSON);
                    var IsAnyOtherAlert = data.filter(item=>item.PatientId + "" == PatientId + "");
                    if (Istoggel) {
                        if (IsAnyOtherAlert.length > 0 && result.MissingDataAlertCount > 0) {
                            utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
                        }
                        else {
                            utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
                        }
                    }
                }
                else {
                    console.log(result.Message);
                }
            });
        }
    },

    OK: function () {
        var MU_ARRAY = [];

        //Update Patient Education 
        if ($("#" + MU_Alerts.params.PanelID + " #div_PatientEducation").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_PE").is(":checked")) {
            var obj = {
                ProfileName: "Patient Education",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //Clinical Information Reconciliation
        if ($("#" + MU_Alerts.params.PanelID + " #div_Reconciliation").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_RE").is(":checked")) {
            var obj = {
                ProfileName: "Reconciliation",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //Electronic Prescribing
        if ($("#" + MU_Alerts.params.PanelID + " #div_ePrescribing").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_ePrescribing").is(":checked")) {
            var obj = {
                ProfileName: "ePrescribing",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //Patient Electronic Access
        if ($("#" + MU_Alerts.params.PanelID + " #div_eAccess").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_eAccess").is(":checked")) {
            var obj = {
                ProfileName: "eAccess",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //View, Download and Transmit
        if ($("#" + MU_Alerts.params.PanelID + " #div_ViewDownloadTransmit").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_ViewDownloadTransmit").is(":checked")) {
            var obj = {
                ProfileName: "ViewDownloadTransmit",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //Secure Messaging
        if ($("#" + MU_Alerts.params.PanelID + " #div_SecureMessaging").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_SecureMessaging").is(":checked")) {
            var obj = {
                ProfileName: "SecureMessaging",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //Transition of Care
        if ($("#" + MU_Alerts.params.PanelID + " #div_TransitionOfCare").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_TransitionOfCare").is(":checked")) {
            var obj = {
                ProfileName: "TransitionOfCare",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //Patient Generated Health Data
        if ($("#" + MU_Alerts.params.PanelID + " #div_PatHealthData").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_PatientDocument").is(":checked")) {
            var obj = {
                ProfileName: "Patient Document",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //Received and Incorporate/Summary of Care
        if ($("#" + MU_Alerts.params.PanelID + " #div_ReceiveAndIncorporate").css("display") == "block"
            && $("#" + MU_Alerts.params.PanelID + " #donotshow_RI").is(":checked")) {
            var obj = {
                ProfileName: "InCorporateSummaryOfCare",
                Fields: "",
                PatientId: MU_Alerts.params.PatientID,
                IsShowAlert: false,
                Type: "MU3"
            };
            MU_ARRAY.push(obj);
        }
        //eCQM Alerts
        $.each($("#" + MU_Alerts.params.PanelID + " #CQMAlerts").find("input[type='checkbox']"), function () {
            if ($(this).is(":checked")) {
                var obj = {
                    AlertId: $(this).attr("alertId"),
                    Fields: "",
                    PatientId: MU_Alerts.params.PatientID,
                    IsShowAlert: false,
                    Type: "eCQM"
                };
                MU_ARRAY.push(obj);
            }
        });
        $.each($("#" + MU_Alerts.params.PanelID + " #div_PatDepressionScreening").find("input[type='checkbox']"), function () {
            if ($(this).is(":checked")) {
                var obj = {
                    ProfileName: "Depression Screening",
                    Fields: "",
                    PatientId: MU_Alerts.params.PatientID,
                    IsShowAlert: false,
                    Type: "Depression Screening"
                };
                MU_ARRAY.push(obj);
            }
        });
        $.each($("#" + MU_Alerts.params.PanelID + " #div_PatTabaccoScreening").find("input[type='checkbox']"), function () {
            if ($(this).is(":checked")) {
                var obj = {
                    ProfileName: "Tobacco Screening",
                    Fields: "",
                    PatientId: MU_Alerts.params.PatientID,
                    IsShowAlert: false,
                    Type: "Tobacco Screening"
                };
                MU_ARRAY.push(obj);
            }
        });
        $.each($("#" + MU_Alerts.params.PanelID + " #div_PatCDSAlert").find("input[type='checkbox']"), function () {
            if ($(this).is(":checked")) {
                var obj = {
                    ProfileName: "CDS Alert",
                    Fields: "",
                    PatientId: MU_Alerts.params.PatientID,
                    IsShowAlert: false,
                    Type: "IA"
                };
                MU_ARRAY.push(obj);
            }
        });
        $.each($("#" + MU_Alerts.params.PanelID + " #div_PatPortalDocument").find("input[type='checkbox']"), function () {
            if ($(this).is(":checked")) {
                var obj = {
                    ProfileName: "Patient Portal Document",
                    Fields: "",
                    PatientId: MU_Alerts.params.PatientID,
                    IsShowAlert: false,
                    Type: "IA"
                };
                MU_ARRAY.push(obj);
            }
        });


        if (MU_ARRAY.length > 0) {
            Patient_Demographic.UpdateMUAlert(MU_ARRAY).done(function (result) {
                if (result.status != false) {
                    var data = JSON.parse(result.MUAlerts_JSON);
                    var IsAnyOtherAlert = data.filter(item=>item.PatientId + "" == MU_Alerts.params.PatientID);
                    if (IsAnyOtherAlert.length > 0 && result.MissingDataAlertCount > 0) {
                        utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
                    }
                    else {
                        utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
                    }
                }
                else {
                    console.log(result.Message);
                }
                MU_Alerts.UnLoadTab();
            });
        }
        else {
            MU_Alerts.UnLoadTab();
        }
    },

    activeTab: function (tab) {
        $("#" + MU_Alerts.params.PanelID + " #listCQM,#listPI,#listIA,#CQMAlerts,#PIAlerts,#IAAlerts").removeClass('active');

        if (tab == "CQM") {
            $("#" + MU_Alerts.params.PanelID + " #listCQM").addClass('active');
            $("#" + MU_Alerts.params.PanelID + " #CQMAlerts").addClass('active');
        }
        else if (tab == "PI") {
            $("#" + MU_Alerts.params.PanelID + " #listPI").addClass('active');
            $("#" + MU_Alerts.params.PanelID + " #PIAlerts").addClass('active');
        }
        else {
            $("#" + MU_Alerts.params.PanelID + " #listIA").addClass('active');
            $("#" + MU_Alerts.params.PanelID + " #IAAlerts").addClass('active');
        }
    },

    MissingDataAlertCount: function () {
        var CQMAlertCount = 0, PIAlertCount = 0, IAAlertCount = 0, totalAlertsCount = 0;

        $('#' + MU_Alerts.params.PanelID + ' #CQMAlerts').find('div[id*=div_]').each(function () {
            if ($(this).css('display') == 'block')
                CQMAlertCount++;
        });

        $('#' + MU_Alerts.params.PanelID + ' #PIAlerts').find('div[id*=div_]').each(function () {
            if ($(this).css('display') == 'block')
                PIAlertCount++;
        });

        $('#' + MU_Alerts.params.PanelID + ' #IAAlerts').find('div[id*=div_]').each(function () {
            if ($(this).css('display') == 'block')
                IAAlertCount++;
        });

        if (CQMAlertCount != 0) {
            $('#' + MU_Alerts.params.PanelID + ' #CQMAlertCount').show();
            $('#' + MU_Alerts.params.PanelID + ' #CQMAlertCount').text(CQMAlertCount);
        }
        else {
            $('#' + MU_Alerts.params.PanelID + ' #CQMAlertCount').css('display', 'none');
            $('#' + MU_Alerts.params.PanelID + ' #CQMAlertCount').text('');
        }

        if (PIAlertCount != 0) {
            $('#' + MU_Alerts.params.PanelID + ' #PIAlertCount').show();
            $('#' + MU_Alerts.params.PanelID + ' #PIAlertCount').text(PIAlertCount);
        }
        else {
            $('#' + MU_Alerts.params.PanelID + ' #PIAlertCount').css('display', 'none');
            $('#' + MU_Alerts.params.PanelID + ' #PIAlertCount').text('');
        }

        if (IAAlertCount != 0) {
            $('#' + MU_Alerts.params.PanelID + ' #IAAlertCount').show();
            $('#' + MU_Alerts.params.PanelID + ' #IAAlertCount').text(IAAlertCount);
        }
        else {
            $('#' + MU_Alerts.params.PanelID + ' #IAAlertCount').css('display', 'none');
            $('#' + MU_Alerts.params.PanelID + ' #IAAlertCount').text('');
        }
    },

    LoadMUAlert_DBCALL: function (PatientId, IsShowAlert, Type, ProfileName) {

        var objData = {};
        objData["PatientId"] = PatientId
        objData["IsShowAlert"] = IsShowAlert;
        objData["Type"] = Type;
        objData["ProfileName"] = ProfileName;
        objData["CommandType"] = "load_mu_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "MUAlert");
    },

    UnLoadTab: function () {

        if (MU_Alerts.params != null && MU_Alerts.params.ParentCtrl != null) {
            UnloadActionPan(MU_Alerts.params.ParentCtrl, 'MU_Alerts');
        }
        else
            UnloadActionPan(null, 'MU_Alerts');
        MU_Alerts.params.TabID == "clinicalTabProgressNote" ? Clinical_ProgressNote.params.isCQMExists = 0 : "";
    },
}