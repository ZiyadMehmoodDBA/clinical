<%@ Page Title="Home Page" Async="true" Language="C#" AutoEventWireup="true" CodeBehind="MDVisionBilling.aspx.cs" Inherits="MDVision.IEHR._Billing" %>

<!doctype html>
<html class="fixed sidebar-left-collapsed ">
<head id="PageHead" runat="server">
    <title>MDVision (Pvt) Ltd. IEMR</title>
    <link rel="shortcut icon" href="favicon.png" />
    <link rel="icon" href="favicon.ico?v=2" />
    <meta charset="UTF-8">
    <!-- Mobile Metas -->
    <meta name="viewport" content="width=device-width" />

    <%--prevent in-browser cache--%>

    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />

    <!-- Web Fonts  -->

    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800|Shadows+Into+Light" rel="stylesheet" type="text/css">
       
          
        <link href="Content/Default/bootstrap.css" rel="stylesheet" />
        <link href="Content/Default/bootstrap-multiselect.css" rel="stylesheet" />
        <link href="Content/Default/bootstrapValidator.css" rel="stylesheet" />
        <link href="Content/Default/datatables.css" rel="stylesheet" />
        <link href="Content/Default/datepicker3.css" rel="stylesheet" />
        <link href="Content/Default/font-awesome.css" rel="stylesheet" />
        <link href="Content/Default/open-iconic-bootstrap.css" rel="stylesheet" />
        <link href="Content/Default/select2.css" rel="stylesheet" />
        <link href="Scripts/js/literallycanvas/css/literallycanvas.css" rel="stylesheet" />
        <link href="Scripts/js/literallycanvas/css/sly-vertical.css" rel="stylesheet" />
        <link href="Content/Kendo/kendo.common.min.css" rel="stylesheet" />
        <link href="Content/Kendo/kendo.default.min.css" rel="stylesheet" />
        <link href="Content/Kendo/kendo.default.mobile.min.css" rel="stylesheet" />

        <%: Styles.Render("~/bundles/Default") %>
        <%: Styles.Render("~/bundles/Blue") %>

        <link href="Content/Common/style-common.css" rel="stylesheet" />

        <%:Scripts.Render("~/bundles/3rdPartyjs") %>
        <script src="Scripts/js/Kendo/kendo.all.min.js"></script>
        <%:Scripts.Render("~/bundles/Common") %>
        <%:Scripts.Render("~/bundles/BillingFiles") %>
        <%:Scripts.Render("~/bundles/CommonAdminFile4Other") %>
        
        <script src="Scripts/js/tinyMCE/js/tinymce/tinymce.min.js"></script>
        <script src="Common/ProviderNoteAccess/hubs"></script>
        
        
    </asp:PlaceHolder>
</head>

<body onload="initSession();">
    <form id="mainForm" runat="server">
        <div class='uil-ring-css' style='-webkit-transform: scale(0.97); display: none;' id="BackgroundLoader">
            <div>

                <div class="circle">
                </div>
                <div class="circle2">
                </div>
                <div class="circle3">
                </div>
                <div class="circle4">
                </div>
            </div>
        </div>
        <section class="body">
            <header class="header">
                <section class="hidden-sm hidden-xs hidden-md pt-lg pull-left size5per">
                    <img src="Content/images/logo-tiny.png" height="65" class="ml-sm" alt="MDVision Pvt Ltd." />
                </section>
                <section class="mainSec profile-small">
                    <section>
                        <div class="pprofile-adjust">
                            <div id="PatientProfile" class="widget-profile-info selected-patient alert fade in p-none" style="display: none;">
                                <button aria-hidden="true" class="fa fa-times noBorder" type="button" id="btnClosePatient" onclick="MainClosePatient();"></button>
                                <div class="profile-picture">
                                    <img id="imgPatientProfile" src="Content/images/default_male_profile.gif" />
                                </div>
                                <div class="profile-info">
                                    <asp:Label ID="lblPatientData" runat="server" />
                                    <br />
                                    <asp:Label ID="PreferredPhone" runat="server"></asp:Label>
                                    <asp:Label ID="PCP" runat="server" />
                                    <asp:Label ID="RefProvider" runat="server" />
                                    <asp:Label ID="PriInsurance" runat="server" />
                                    <asp:Label ID="InsBalance" runat="server" />
                                    <asp:Label ID="PatBalance" runat="server" />
                                    <asp:Label ID="CollBalance" runat="server" />
                                    <asp:Label ID="PatAdvanceBalance" runat="server" />
                                    <span class="hidden" id="sendBillingInquiry"><a class="btn btn-success btn-xs pt-none pb-none pl-xs pr-xs" href="#" onclick="Patient_Demographic.sendBillingInquiryEmail(this);" title="Send Billing Inquiry"><i class="fa fa-envelope"></i></a></span>
                                    <asp:HiddenField ID="hdnIsSessionTimeout" Value="0" runat="server" />
                                    <input type="hidden" id="hfPatientId" />
                                    <input type="hidden" id="hfAccountNo" />
                                    <input type="hidden" id="hfPatientFullName" />
                                    <input type="hidden" id="hfPatientFullNameOnly" />
                                    <input type="hidden" id="hfPatientRefProviderId" />
                                    <input type="hidden" id="hfPatientRefProviderName" />
                                    <input type="hidden" id="hfPatientPracticeId" />
                                    <input type="hidden" id="hfPatientFacilityId" />
                                    <input type="hidden" id="hfPatientInsuranceId" />
                                    <input type="hidden" id="hfDischargeDate" />
                                    <input type="hidden" id="hfPatientBalance" />

                                    <%-- patient specific values --%>

                                    <input type="hidden" id="hfPatientFacilityName" />
                                    <input type="hidden" id="hfPatientProviderName" />
                                    <input type="hidden" id="hfPatientProviderId" />

                                    <%-- Start 09/12/2015 Muhammad Irfan to set the patient Gender globally in hidden field, for use in SocialHx --%>
                                    <input type="hidden" id="hfPatientSex" />
                                    <%-- Start 09/12/2015 Muhammad Irfan to set the patient Gender globally in hidden field, for use in SocialHx --%>
                                    <%-- Start 19/01/2016 Muhammad Irfan to set the patient DOB globally in hidden field --%>
                                    <input type="hidden" id="hfPatientDOB" />
                                    <%-- Start 19/01/2016 Muhammad Irfan to set the patient DOB globally in hidden field --%>
                                    <input type="hidden" id="hfPatientMaritalStatus" />
                                    <input type="hidden" id="hfPatientEthnicityIds" />
                                    <input type="hidden" id="hfPatientRaceIds" />
                                    <input type="hidden" id="hfPatientAddress1" />
                                    <input type="hidden" id="hfPatientCity" />
                                    <input type="hidden" id="hfPatientState" />
                                    <input type="hidden" id="hfPatientZip" />
                                    <input type="hidden" id="hfPatientHomeTel" />
                                </div>
                            </div>
                        </div>


                        <div class="mr-sm pull-right">
                            <span id="userCurrentTime" clientidmode="Static" runat="server"></span>
                            <div class="clearfix"></div>
                            <div class="userbox">
                                <a href="#" data-toggle="dropdown">
                                    <i class="fa fa-question-circle" style="font-size: 20px" aria-hidden="true"></i>
                                    <%--   <i class="fa custom-caret"></i>--%>
                                </a>
                                <div class="dropdown-menu pt-xs">
                                    <ul class="list-unstyled">
                                        <li id="Li3" runat="server"><a class="noContentAfter" runat="server" href="javascript:OpenHelpScreen();">
                                            <i class="fa fa-newspaper-o"></i>Release Notes
                                        </a>
                                        </li>
                                        <li id="Li4" runat="server"><a class="noContentAfter" runat="server" href="javascript:OpenAboutScreen();">
                                            <i class="fa fa-info" style="font-style: italic" aria-hidden="true"></i>About
                                        </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div id="userbox" class="userbox">
                                <a href="#" data-toggle="dropdown">
                                    <figure class="profile-picture">
                                        <i class="fa fa-user fa-2x blue" runat="server" id="regularUserIcon"></i>
                                        <span class="animated infinite flash btn-flash" id="emergencyUserIconSpan">
                                            <i title="Emergency Access User" class="fa fa-user fa-2x red" runat="server" id="emergencyUserIcon"></i>
                                        </span>
                                    </figure>
                                    <div class="profile-info">
                                        <span class="name">
                                            <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label></span>
                                        <span class="role">
                                            <asp:Label ID="lblUserEntity" runat="server" Text=""></asp:Label></span>
                                    </div>
                                    <i class="fa custom-caret"></i>
                                </a>
                                <div class="dropdown-menu">
                                    <ul class="list-unstyled">
                                        <li class="divider"></li>

                                        <li id="dassett" runat="server">
                                            <a href="#" runat="server" title="DashBoardSetting" onclick="OpenDashBoardSetting();">
                                                <i class="fa fa-wrench"></i>
                                                My Settings </a>
                                        </li>
                                        <li id="chgpwd" runat="server">
                                            <a href="#" runat="server" title="ChangePassword" onclick="OpenDashBoardChgPwd();">
                                                <i class="fa fa-lock mr-xs"></i>
                                                Change Password </a>
                                        </li>
                                        <li class="dropdown dropdown-custom">
                                            <button type="button"><i class="fa fa-exchange"></i>Switch Entity</button>
                                            <asp:BulletedList ID="blistSwitchEntity" CssClass="dropdown-menu" runat="server" DisplayMode="LinkButton" OnClick="SwitchEntity_Click">
                                            </asp:BulletedList>
                                        </li>
                                        <li>
                                            <asp:Button ID="btnLogout" runat="server" Text="&#xf011; &nbsp;Logout" OnClientClick="return CustomLogOutRemovals();" OnClick="Logout_Click" />
                                        </li>


                                    </ul>

                                </div>
                            </div>


                        </div>
                    </section>
                    <div class="clearfix"></div>
                    <section class="controls-xs">
                        <div class="visible-xs toggle-sidebar-left" data-toggle-class="sidebar-left-opened" data-target="html" data-fire-event="sidebar-left-opened">
                            <i class="fa fa-bars" aria-label="Toggle sidebar"></i>
                        </div>
                        <div id="pnlTab1" class="pull-left">
                            <div class="tabs">
                                <ul class="nav nav-tabs">
                                    <li id="mstrTabDashBoard" visible="false" runat="server">
                                        <a href="MDVisionDefault.aspx#" data-toggle="tooltip" title="DashBoard">
                                            <span>DashBoard</span>
                                            <i class="fa fa-dashboard"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabPatient" runat="server" visible="false">
                                        <a href="MDVisionDefault.aspx#Patient" data-toggle="tooltip" title="Patient">
                                            <span>Patient</span>
                                            <i class="fa fa-user"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabSchedule" runat="server" visible="false">
                                        <a href="MDVisionDefault.aspx#Schedule" data-toggle="tooltip" title="Scheduler">
                                            <span>Scheduler</span>
                                            <i class="fa fa-calendar"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabClinical" runat="server" visible="false">
                                        <a href="MDVisionDefault.aspx#Clinical" data-toggle="tooltip" title="Clinical">
                                            <span>Clinical</span>
                                            <i class="fa fa-user"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabBilling" runat="server" visible="false">
                                        <a href="#Billing" data-toggle="tooltip" title="Billing" onclick="SelectTab('mstrTabBilling','false');">
                                            <span>Billing</span>
                                            <i class="fa fa-book"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabBatch" runat="server" visible="false">
                                        <a href="MDVisionDefault.aspx#Batch" data-toggle="tooltip" title="Batch">
                                            <span>Batch</span>
                                            <i class="fa fa-cubes"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabReports" runat="server" visible="false">
                                        <a href="MDVisionReports.aspx#Reports" data-toggle="tooltip" title="Reports">
                                            <span>Reports</span>
                                            <i class="fa fa-bar-chart"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabiTrack" runat="server" visible="false">
                                        <a href="MDVisioniTrack.aspx#iTrack" title=""  onclick="CheckDemographicDetailOnRedirect('MDVisioniTrack.aspx#iTrack', event);">
                                            <span>iTrack</span>
                                            <i class="glyphicon glyphicon-sort-by-attributes-alt"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabAuditbleEvents" runat="server" visible="false">
                                        <a href="MDVisionAuditableEvents.aspx#AuditableEvents" data-toggle="tooltip" title="Auditable Events">
                                            <span>Auditable Events</span>
                                            <i class="fa fa-calendar-check-o"></i>
                                        </a>
                                    </li>
                                    <li id="mstrTabAdmin" runat="server" visible="false">
                                        <a href="MDVisionAdmin.aspx#Admin" data-toggle="tooltip" title="Admin">
                                            <span>Admin</span>
                                            <i class="fa fa-cogs"></i>
                                        </a>
                                    </li>
                                </ul>
                            </div>

                        </div>

                        <div class="pull-right hidden-xs">
                            <ul class="notifications">
                                <li runat="server" id="Faxes">
                                    <a role="button" class="notification-icon hand-cursor" style="display: inline-block;" runat="server" id="btnFaxes" data-toggle="tooltip" data-placement="bottom" title="Fax" onclick="OpenFaxes();">
                                        <i class="fa fa-fax"></i>
                                        <span id="spnFaxesCount" runat="server" class="badge"></span>
                                    </a>
                                </li>
                                <li runat="server" id="Messages">
                                    <a role="button" class="notification-icon hand-cursor" style="display: none;" runat="server" id="btnPatientMessages" data-toggle="tooltip" data-placement="bottom" title="Messages" onclick="OpenUserMessages();">
                                        <i class="fa fa-envelope"></i>
                                        <span id="spnMessageCount" runat="server" class="badge"></span>
                                    </a>
                                </li>
                                <li runat="server" id="Tasks">
                                    <a role="button" class="notification-icon hand-cursor" style="display: none;" runat="server" id="btnUserTasks" data-toggle="tooltip" data-placement="bottom" title="Tasks" onclick="OpenUserTasks();">
                                        <i class="fa fa-tasks"></i>
                                        <span id="spnUserTasksCount" runat="server" class="badge"></span>
                                    </a>
                                </li>
                                <li runat="server" id="AddPatient">
                                    <a role="button" class="notification-icon hand-cursor" runat="server" id="btnAddPatient" onclick="Patient_Demographic.OpenDemographicQuick();" data-toggle="tooltip" data-placement="bottom" title="Quick Add Patient">
                                        <i class="fa fa-user-plus"></i>
                                    </a>
                                </li>
                                <li runat="server" id="PatientSearch">
                                    <a role="button" class="notification-icon hand-cursor" runat="server" id="btnPatientSearch" onclick="Patient_Demographic.OpenPatientSearch();" name="btnPatientSearch" data-toggle="tooltip" data-placement="bottom" title="Search&nbsp;Patient">
                                        <i class="fa fa-search"></i>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </section>
                </section>
            </header>
            <div class="inner-wrapper">
                <div id="pnlTab2">
                    <aside id="sidebar-left" class="sidebar-left">
                        <div class="sidebar-header">
                            <div class="sidebar-title"><%--Navigation --%><a id="anchorMainMenu" class="btn btn-link btn-lg menuBackBtn" href="#" onclick="ToggleMenu('MainMenu', 'ClinicalUL', '#hfCurrentMenuParentLiId', '#hfCurrentMenuChildLiId', '#hfPrevMenuParentLiId', '#hfPrevMenuChildLiId');"> <span data-toggle="tooltip" data-placement="right" title="" class="oi oi-account-logout pull-right" data-original-title="Back to Main Menu"></span></a></div>
                            <div class="sidebar-toggle hidden-xs" data-toggle-class="sidebar-left-collapsed" data-target="html" data-fire-event="sidebar-left-toggle"><i class="fa fa-bars" aria-label="Toggle sidebar"></i></div>
                        </div>
                        <div class="nano">
                            <div class="nano-content">
                                <nav id="menu" class="nav-main" role="navigation">
                                    <ul class="nav nav-main" id="mainUL">

                                        <li id="mstrMenuDashBoard" runat="server" class="nav-parent" visible="false">
                                            <a class="nav-parent noContentAfter" runat="server" href="MDVisionDefault.aspx">
                                                <i class="fa fa-dashboard" aria-hidden="true"></i>
                                                <span>DashBoard</span>
                                            </a>
                                        </li>
                                        <li id="mstrMenuPatient" class="nav-parent" runat="server" visible="false">
                                            <a class="noContentAfter" href="MDVisionDefault.aspx#Patient" title="Patient">
                                                <i class="fa fa-user" aria-hidden="true"></i>
                                                <span>Patient</span>
                                            </a>
                                        </li>
                                        <li class="nav-parent" id="mstrMenuSchedule" runat="server" visible="false">
                                            <a class="noContentAfter" href="MDVisionDefault.aspx#Schedule" title="Scheduler">
                                                <i class="fa fa-calendar" aria-hidden="true"></i>
                                                <span>Scheduler</span>
                                            </a>
                                        </li>

                                        <li class="nav-parent" id="mstrMenuClinical" runat="server" visible="false">
                                            <a class="noContentAfter" href="MDVisionDefault.aspx#Clinical" title="Clinical">
                                                <i class="fa fa-stethoscope" aria-hidden="true"></i>
                                                <span>Clinical</span>
                                            </a>
                                        </li>
                                        <li class="nav-parent" id="mstrMenuBilling" runat="server" visible="false">
                                            <a href="#Billing" data-toggle="tab" title="Billing" <%--onclick="SelectTab('mstrTabBilling','true');--%>">
                                                <i class="fa fa-book" aria-hidden="true"></i><span>Billing</span> </a>
                                            <ul class="nav nav-children">
                                                <li id="billMenuUnClaimedAppointment" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabUnClaimedAppointment','true');" title="Unclaimed Appointments">Unclaimed Appointments</a>
                                                </li>
                                                <li id="billMenuChargeSearch" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabChargeSearch','true');" title="Charges">Charges</a></li>
                                                <li id="billMenuOutOfOfficeVisits" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabOutOfOfficeVisits','true');" title="Out of Office Visits">Out of Office Visits</a>
                                                </li>
                                                <li id="billMenuChargeBatchSearch" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabChargeBatchSearch','true');" title="Charge Batch">Charge Batch</a>
                                                </li>
                                                <li id="billMenuClaimSubmission" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabClaimSubmission','true');" title="Claim Submission">Claim Submission</a>
                                                </li>
                                                <li id="billMenuPaymentPosting" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabPaymentPosting','true');" title="Payment Posting">Payment Posting</a>
                                                </li>
                                                <li id="billMenuCopayReceipt" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabCopayReceipt','true');" title="Copay Receipt Search">Copay Receipt</a>
                                                </li>
                                                <li id="billMenuPatientStatement" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabPatientStatement','true');" title="Patient Statement">Patient Statement</a>
                                                </li>
                                                <li id="billMenuPaymentBatchSearch" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabPaymentBatchSearch','true');" title="Payment Batch">Payment Batch</a>
                                                </li>
                                                <li id="billMenuEDIReport" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabEDIReport','true');" title="EDI Reports">EDI Reports</a>
                                                </li>
                                                <li id="billMenuERA" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabERA','true');" title="ERA">ERA</a>
                                                </li>
                                                <li id="billMenuEOBManualPosting" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('billTabEOBManualPosting','true');" title="EOB Posting">EOB Posting</a>
                                                </li>
                                                <li class="nav-parent" id="billMenuFollowUp" runat="server" visible="false">
                                                    <a>Follow Up</a>
                                                    <ul class="nav nav-children">
                                                        <li id="billMenuFollowUpPatientAR" runat="server" visible="false">
                                                            <a href="javascript:SelectTab('billTabFollowUpPatientAR','true');" title="Patient AR">Patient AR</a>
                                                        </li>
                                                        <li id="billMenuFollowUpInsuranceAR" runat="server" visible="false">
                                                            <a href="javascript:SelectTab('billTabFollowUpInsuranceAR','true');" title="Insurance AR">Insurance AR</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="nav-parent" id="mstrMenuBatch" runat="server" visible="false">
                                            <a href="#Batch" data-toggle="tab" title="Batch" onclick="SelectTab('mstrTabBatch','true');">
                                                <i class="fa fa-cubes " aria-hidden="true"></i>
                                                <span>Batch</span>
                                            </a>
                                            <ul class="nav nav-children">
                                                <li id="batchMenuDocuments" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabDocuments" title="Documents">Documents</a>
                                                </li>
                                                <li id="batchMenuMessages" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabMessages" title="Tasks">Tasks</a>
                                                </li>
                                                <li id="batchMenuPatientEligibility" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabPatientEligibility" title="Patient Eligibility">Patient Eligibility</a>
                                                </li>
                                                <li id="batchMenuEncounter" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabEncounter" title="Encounter">Encounter</a>
                                                </li>
                                                <li id="batchMenuFax" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabFax" title="Fax">Fax</a>
                                                </li>

                                                <li id="batchMenuClinicalQualityMeasure" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabClinicalQualityMeasure" title="Clinical Quality Measure">Clinical Quality Measure</a>
                                                </li>
                                                <li id="batchMenuIzendaReports" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabIzendaReports" title="Izenda Report Test">Izenda Report Test</a>
                                                </li>
                                                <li id="batchMenuPatientList" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabPatientList" title="Patient List">Patient List</a>
                                                </li>
                                                <li id="batchMenuImportCCDA" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabImportCCDA" title="Import CCDA">Import CCDA</a>
                                                </li>
                                                <li id="batchMenuExportCCDA" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabExportCCDA" title="Export CCDA">Export CCDA</a>
                                                </li>
                                                <li id="batchMenuImportHL7LabResults" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabImportHL7LabResults" title="Import HL7 Lab Results">Import HL7 Lab Results</a>
                                                </li>
                                                <li id="batchMenuImportHL7ImmunizationBatch" runat="server" visible="false">
                                                    <a href="MDVisionDefault.aspx#Batch/batchTabImportHL7ImmunizationBatch" title="Import Immunization HL7">Import Immunization HL7</a>
                                                </li>
                                            </ul>
                                        </li>
                                        <li id="mstrMenuReports" runat="server" class="nav-parent" visible="false">
                                            <a class="nav-parent noContentAfter" runat="server" href="MDVisionReports.aspx#Reports">
                                                <i class="fa fa-bar-chart" aria-hidden="true"></i>
                                                <span>Reports</span>
                                            </a>
                                        </li>
                                         <li class="nav-parent" id="mstrMenuiTrack" runat="server" visible="false">
                                            <a data-toggle="tab" title="iTrack" onclick="SelectTab('mstrTabiTrack','true');">
                                                <i class="glyphicon glyphicon-sort-by-attributes-alt" aria-hidden="true"></i><span>iTrack</span> </a>
                                            <ul class="nav nav-children">
                                               
                                                <li id="iTrackMenuMeaningfulUse"  class="nav-parent" runat="server" visible="false">
                                                   <a>Meaningful Use (MU)</a>
                                                    <ul class="nav nav-children">
                                                        <li id="iTrackMenuMuStage2" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="MU Stage 2">MU Stage 2</a>
                                                        </li>
                                                         <li id="iTrackMenuMuStage3" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="MU Stage 3">MU Stage 3</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li id="iTrackMenueCQMs" runat="server" visible="false">
                                                    <a href="javascript:SelectTab('iTrackTabDashboard','true');" title="eCQMs">eCQMs</a></li>
                                                 <li id="iTrackMenuMIPS"  class="nav-parent" runat="server" visible="false">
                                                   <a>MIPS</a>
                                                    <ul class="nav nav-children">
                                                        <li id="iTrackMenuMIPSummary" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="MIPS Summary">MIPS Summary</a>
                                                        </li>
                                                         <li id="iTrackMenuQuality" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="Quality">Quality</a>
                                                        </li>
                                                        <li id="iTrackMenuACI" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="MPromoting Interoperability (Formerly ACI)">Promoting Interoperability (Formally ACI)</a>
                                                        </li>
                                                         <li id="iTrackMenuIA" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="IA (Improvement Activities)">IA (Improvement Activities)</a>
                                                        </li>
                                                        <li id="iTrackMenuCost" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="Cost">Cost</a>
                                                        </li>
                                                         <li id="iTrackMenuSubmission" runat="server" visible="false">
                                                            <a href="MDVisioniTrack.aspx#iTrack/iTrakTabDashboard" title="Submission">Submission</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="nav-parent" id="mstrMenuAuditbleEvents" runat="server" visible="false">
                                            <a href="#AuditableEvents" data-toggle="tab" title="Auditable Events" onclick="SelectTab('mstrTabAuditbleEvents','true');">
                                                <i class="fa fa-calendar-check-o " aria-hidden="true"></i>
                                                <span>Auditable Events</span>
                                            </a>
                                            <ul class="nav nav-children">
                                                <li id="auditbleEventsMenuActivityLog" runat="server">
                                                    <a href="MDVisionAuditableEvents.aspx#AuditableEvents/auditbleEventsTabActivityLog" title="Activity Log">Activity Log
                                                    </a>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="nav-parent" id="mstrMenuAdmin" runat="server" visible="false">
                                            <a href="#Admin" data-toggle="tab" title="Admin" onclick="SelectTab('mstrTabAdmin','true');">
                                                <i class="fa fa-cogs " aria-hidden="true"></i>
                                                <span>Admin</span>
                                            </a>
                                            <ul class="nav nav-children">
                                                <li class="nav-parent" id="adminMenuSecurity" runat="server" visible="false">
                                                    <a>Security</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuUser" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabUser" title="User">User</a>
                                                        </li>
                                                        <li id="adminMenuCoWorkersGroup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabCoWorkersGroup" title="Co-workers Group">Co-workers Group</a>
                                                        </li>
                                                        <li id="adminMenuSecurityRoles" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabSecurityRoles" title="Security Roles">Security Roles</a>
                                                        </li>
                                                        <li id="adminMenuPrivilegeGroup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPrivilegeGroup" title="Security Entity Group">Security Entity Group</a>
                                                        </li>
                                                        <li id="adminMenuPasswordConfiguration" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPasswordConfiguration" title="Password Configuration">Password Configuration</a>
                                                        </li>
                                                      <%--  <li id="adminMenuLoggedInUsers" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabLoggedInUsers" title="Logged In Users">Logged In Users</a>
                                                        </li>--%>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuProfiles" runat="server" visible="false">
                                                    <a>Profiles</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuProvider" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabProvider" title="Provider">Provider</a>
                                                        </li>
                                                        <li id="adminMenuPractice" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPractice" title="Practice">Practice</a>
                                                        </li>
                                                        <li id="adminMenuFacility" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFacility" title="Facility">Facility</a></li>
                                                        <li id="adminMenuResources" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabResources" title="Resources">Resources</a>
                                                        </li>
                                                        <li id="adminMenuFaxSettings" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFaxSettings" title="Electronic Fax">Electronic Fax</a>
                                                        </li>
                                                        <li id="adminMenuSpecialty" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabSpecialty" title="Specialty">Specialty</a>
                                                        </li>
                                                        <li id="adminMenuBillingProviderSettings" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabBillingProviderSettings" title="Additional Billing Provider">Additional Billing Provider</a>
                                                        </li>
                                                        <li id="adminMenuBillingProvider" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabBillingProvider" title="Billing Provider">Billing Provider</a>
                                                        </li>
                                                        <li id="adminMenuReferringProvider" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabReferringProvider" title="Referring Provider">Referring Provider</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuSchedule" runat="server" visible="false">
                                                    <a>Schedule Setup</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuProviderSchedule" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabProviderSchedule" title="Provider Schedule">Provider Schedule</a>
                                                        </li>
                                                        <li id="adminMenuResourceSchedule" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabResourceSchedule" title="Resource Schedule">Resource Schedule</a>
                                                        </li>
                                                        <li id="adminMenuScheduleReason" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabScheduleReason" title="Schedule Reason">Schedule Reason</a>
                                                        </li>
                                                        <li id="adminMenuHolidays" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabHolidays" title="Holidays">Holidays</a>
                                                        </li>
                                                        <li id="adminMenuBlockHours" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabBlockHours" title="Block Hours">Block Hours</a>
                                                        </li>
                                                        <li id="adminMenuAppointmentStatus" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabAppointmentStatus" title="Appointment Status">Appointment Status</a>
                                                        </li>
                                                        <li id="adminMenuVisitTypeDurationGroup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabVisitTypeDurationGroup" title="Visit Type Duration Group">Visit Type Duration Group</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuCodes" runat="server" visible="false">
                                                    <a>Codes</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuCPTCode" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabCPTCode" title="CPT Code">CPT</a>
                                                        </li>
                                                        <li id="adminMenuICD" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabICD" title="ICD">ICD</a>
                                                        </li>
                                                        <li id="adminMenuModifier" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabModifier" title="Modifier">Modifier</a>
                                                        </li>
                                                        <li id="adminMenuRevenueCode" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabRevenueCode" title="Revenue Code">Revenue Code</a>
                                                        </li>
                                                        <li id="adminMenuProcedureCategory" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabProcedureCategory" title="Procedure Category">Procedure Category</a>
                                                        </li>
                                                        <li id="adminMenuPlaceOfService" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPlaceOfService" title="Place Of Service">Place Of Service</a>
                                                        </li>
                                                        <li id="adminMenuTypeOfService" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabTypeOfService" title="Type Of Service">Type Of Service</a>
                                                        </li>
                                                        <li id="adminMenuOccupationStatus" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabOccupationStatus" title="Occupation Status">Occupation Status</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuInsurance" runat="server" visible="false">
                                                    <a>Insurance Setup</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuInsur" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabInsur" title="Insurance">Insurance</a>
                                                        </li>
                                                        <li id="adminMenuPlanCategory" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPlanCategory" title="Plan Category">Plan Category</a>
                                                        </li>
                                                        <li id="adminMenuInsurancePlan" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabInsurancePlan" title="Insurance Plan">Insurance Plan</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuPaymentSetup" runat="server" visible="false">
                                                    <a>Payment Setup</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuLedgerAccount" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabLedgerAccount" title="Ledger Account">Ledger Account</a>
                                                        </li>
                                                        <li id="adminMenuERAAdjustmentCodes" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabERAAdjustmentCodes" title="ERA Adjustment Codes">ERA Adjustment Codes</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuFollowUp" runat="server" visible="false">
                                                    <a>Follow Up</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuFollowUpGroup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpGroup" title="Group">Group</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpReason" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpReason" title="Reason">Reason</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpType" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpType" title="Type">Type</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpRemittanceCode" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpRemittanceCode" title="Remittance Code">Remittance Code</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpAdjustmentCode" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpAdjustmentCode" title="Adjustment Code">Adjustment Code</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpAction" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpAction" title="Action">Action</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpClaimStatusCode" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpClaimStatusCode" title="Claim Status Code">Claim Status Code</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpClaimStatusCategoryCode" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpClaimStatusCategoryCode" title="Claim Status Category Code">Claim Status Category Code</a>
                                                        </li>
                                                        <li id="adminMenuFollowUpCodesMapping" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFollowUpCodesMapping" title="Codes Mapping">Codes Mapping</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuEDI" runat="server" visible="false">
                                                    <a>EDI Setup</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuClearingHouse" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabClearingHouse" title="Clearing House">Clearing House</a>
                                                        </li>
                                                        <li id="adminMenuSubmitterSetup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabSubmitterSetup" title="Submitter Setup">Submitter Setup</a>
                                                        </li>
                                                        <li id="adminMenuEDIReceiver" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabEDIReceiver" title="EDI Receiver">EDI Receiver</a>
                                                        </li>
                                                        <li id="adminMenuEDITaxIDSetup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabEDITaxIDSetup" title="EDI Tax ID Setup">EDI Tax ID Setup</a>
                                                        </li>
                                                        <li id="adminMenuEDISubmitInsurance" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabEDISubmitInsurance" title="EDI Submit Insurance">EDI Submit Insurance</a>
                                                        </li>
                                                        <li id="adminMenuEDIEligibilityInsurance" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabEDIEligibilityInsurance" title="EDI Eligibility Insurance">EDI Eligibility Insurance</a>
                                                        </li>
                                                        <li id="adminMenuEDIClaimStatusInsurance" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabEDIClaimStatusInsurance" title="EDI Claim Status Insurance">EDI Claim Status Insurance</a>
                                                        </li>
                                                        <li id="adminMenuEDIServiceHandle" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabEDIServiceHandle" title="EDI Service">EDI Service</a>
                                                        </li>
                                                        <li id="adminMenuPatientEligibilityService" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPatientEligibilityService" title="Patient Eligibility Service">Patient Eligibility Service</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuFeeSchedule" runat="server" visible="false">
                                                    <a>Fee Schedule</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuBasicFeeGroup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabBasicFeeGroup" title="Basic Fee Group">Basic Fee Group</a>
                                                        </li>
                                                        <li id="adminMenuFeeGroup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFeeGroup" title="Fee Group">Fee Group</a>
                                                        </li>
                                                        <li id="adminMenuPlanFeeLink" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPlanFeeLink" title="Plan Fee Link">Plan Fee Link</a>
                                                        </li>
                                                        <li id="adminMenuBasicFeeSchedule" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabBasicFeeSchedule" title="Basic Fee Schedule">Basic Fee Schedule</a>
                                                        </li>
                                                        <li id="adminMenuProcedureFeeSchedule" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabProcedureFeeSchedule" title="Fee Group Plan CPT">Fee Group Plan CPT</a>
                                                        </li>
                                                        <li id="adminMenuPOSFeeSchedule" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabPOSFeeSchedule" title="Fee Group Plan CPT POS">Fee Group Plan CPT POS</a>
                                                        </li>
                                                        <li id="adminMenuDrugCodeCost" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabDrugCodeCost" title="Drug Code Costs">Drug Code Costs</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuDocumentSetup" runat="server" visible="false">
                                                    <a>Document Setup</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuFolderType" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFolderType" title="Folder Type">Folder Type</a>
                                                        </li>
                                                        <li id="adminMenuFolder" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabFolder" title="Folder">Folder</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuBills" runat="server" visible="false">
                                                    <a>Billing</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuSupperBill" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabSupperBill" title="Super Bill">Super Bill</a>
                                                        </li>
                                                        <li id="adminMenuStatementMessage" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabStatementMessage" title="Statement Message">Statement Message</a>
                                                        </li>
                                                        <li id="adminMenuStatementGroup" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabStatementGroup" title="Statement Group">Statement Group</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuHL7" runat="server" visible="false">
                                                    <a>HL7</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuHL7ADT" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabHL7ADT" title="HL7ADT">ADT</a>
                                                        </li>
                                                        <li id="adminMenuHL7SIU" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabHL7SIU" title="HL7SIU">SIU</a>
                                                        </li>
                                                        <li id="adminMenuHL7DFT" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabHL7DFT" title="HL7DFT">DFT</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuClinical" runat="server" visible="false">
                                                    <a>
                                                        <i class="fa fa-align-left" aria-hidden="true"></i>
                                                        <span>Clinical</span> 
                                                    </a>
                                                    <ul class="nav nav-children">
                                                        <li class="nav-parent" id="adminMenuClinicalQuestionnaire" runat="server" visible="false">
                                                            <a>Clinical Questionnaire</a>
                                                            <ul class="nav nav-children">
                                                                <li id="adminMenuObservations" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabObservations" title="Observations">Observations</a>
                                                                </li>
                                                                <li id="adminMenuSystems" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabSystems" title="Systems">Systems</a>
                                                                </li>
                                                                <li id="adminMenuROSCharatristics" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabROSCharatristics" title="ROS Characteristics">ROS Characteristics</a>
                                                                </li>
                                                                <li id="adminMenuROSSystems" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabROSSystems" title="ROS Systems">ROS Systems</a>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li class="nav-parent" id="adminMenuClinicalReport" runat="server" visible="false">
                                                            <a>Report</a>
                                                            <ul class="nav nav-children">
                                                                <li id="adminMenuAuditReport" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabAuditReport" title="Audit Report">Audit Report</a>
                                                                </li>
                                                                <li id="adminMenuReportHeader" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabReportHeader" title="Report Header">Report Header</a>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li id="adminMenuCDS" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabCDS" title="Clinical Decision Support">Clinical Decision Support</a>
                                                        </li>
                                                        <li class="nav-parent" id="adminMenuPQRS" runat="server" visible="false">
                                                            <a>PQRS</a>
                                                            <ul class="nav nav-children">
                                                                <li id="adminMenuMeasureGroups" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabMeasureGroups" title="Measure Groups">Measure Groups</a>
                                                                </li>
                                                                <li id="adminMenuIndividualReporting" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabIndividualReporting" title="Individual Reporting">Individual Reporting</a>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li id="adminMenuLab" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabLab" title="Laboratory">Laboratory</a>
                                                        </li>
                                                        <li class="nav-parent" id="adminMenuTemplate" runat="server" visible="false">
                                                            <a>Template</a>
                                                            <ul class="nav nav-children">
                                                                <li id="adminMenuProviderNote" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabProviderNote" title="Provider Note Templates">Provider Note</a>
                                                                </li>
                                                                <li id="adminMenuTemplateLetter" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabTemplateLetter" title="Letter Templates">Letter</a>
                                                                </li>
                                                                <li id="adminMenuPhysicalExamTemplate" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabPhysicalExamTemplate" title="Physical Exam">Physical Exam</a>
                                                                </li>
                                                                <li id="adminMenuAOEtemplate" runat="server" visible="true">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabAOETemplate" title="AOE Templates">AOE Templates</a>
                                                                </li>
                                                                <li id="adminMenuProcedureTemplate" runat="server" visible="true">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabProcedureTemplate" title="Procedure Templates">Procedure Templates</a>
                                                                </li>
                                                                <li id="adminMenuReviewOfSystemsTemplate" runat="server" visible="false" style="display: none">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabReviewOfSystemsTemplate" title="Review of Systems Templates">Review of Systems</a>
                                                                </li>
                                                                <li id="adminMenuROSTemplateRevamp" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabROSTemplateRevamp" title="Review of System">Review of System</a>
                                                                </li>
                                                                <li id="adminMenuCustomForms" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabCustomForms" title="Custom Forms">Custom Forms</a>
                                                                </li>
                                                                <li id="adminMenuOrderSets" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabOrderSets" title="Order Sets">Order Sets</a>
                                                                </li>
                                                                <li class="nav-parent" id="adminMenuTemplateHPI" runat="server" visible="false">
                                                                    <a>HPI</a>
                                                                    <ul class="nav nav-children">
                                                                        <li id="adminMenuHPISymptoms" runat="server" visible="false">
                                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabHPISymptoms" title="HPI Symptoms">HPI Symptoms</a>
                                                                        </li>
                                                                        <li id="adminMenuHPIFindings" runat="server" visible="false">
                                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabHPIFindings" title="HPI Findings">HPI Findings</a>
                                                                        </li>
                                                                        <li id="adminMenuHPITemplate" runat="server" visible="false">
                                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabHPITemplate" title="HPI Template">HPI Template</a>
                                                                        </li>
                                                                    </ul>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li class="nav-parent" id="adminMenuDataTemplate" runat="server" visible="false" style="display: none;">
                                                            <a>Data Template</a>
                                                            <ul class="nav nav-children">
                                                                <li id="adminMenuReviewOfSystemsDataTemplate" runat="server" visible="false" style="display: none;">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabReviewOfSystemsDataTemplate" title="Review of Systems Data Templates">Review of Systems</a>
                                                                </li>
                                                                <li id="adminMenuPhysicalExamDataTemplate" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabPhysicalExamDataTemplate" title="Physical Exam Data Templates">Physical Exam</a>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li class="nav-parent" id="adminMenuFavorites" runat="server" visible="false">
                                                            <a>Favorites</a>
                                                            <ul class="nav nav-children">
                                                                <li id="adminMenuFavoritesVaccine" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesVaccine" title="Favorites Vaccine">Vaccine</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesTherapueticInjection" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesTherapueticInjection" title="Favorites Therapeutic Injection">Therapeutic Injection</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesComplaint" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesComplaint" title="Favorites Complaint">Chief Complaints</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesProblems" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesProblems" title="Favorites Problems">Problems</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesConsultationOrder" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesConsultationOrder" title="Favorites Consultation Order">Consultation Order</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesCustomForms" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesCustomForms" title="Custom Forms">Custom Forms</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesProcedureOrder" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesProcedureOrder" title="Favorites Procedure Order">Procedure Order</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesRadiologyOrder" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesRadiologyOrder" title="Favorites Diagnostic Imaging">Diagnostic Imaging</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesHistory" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesFamilyHistory" title="">History</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesLabOrder" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesLabOrder" title="Favorites Lab Order">Lab Order</a>
                                                                </li>
                                                                <li id="adminMenuFavoritesMedication" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabFavoritesMedication" title="Favorites Medication">Medication</a>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li class="nav-parent" id="adminMenuImmunization" runat="server" visible="false">
                                                            <a>Immunization</a>
                                                            <ul class="nav nav-children">
                                                                <li id="adminMenuImmunizationCategory" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabImmunizationCategory" title="Category">Category</a>
                                                                </li>
                                                                <li id="adminMenuImmunizationVaccineCrosswalk" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabImmunizationVaccineCrosswalk" title="Vaccine Crosswalk">Vaccine Crosswalk</a>
                                                                </li>
                                                                <li id="adminMenuImmunizationScheduleSetup" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabImmunizationScheduleSetup" title="Schedule Setup">Schedule Setup</a>
                                                                </li>
                                                                <li id="adminMenuImmunizationLotManagement" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabImmunizationLotManagement" title="Lot Management">Lot Management </a>
                                                                </li>
                                                                <li id="adminMenuImmunizationAddImmInj" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabImmunizationAddImmInj" title="Add Imm/Inj">Add Imm/Inj</a>
                                                                </li>
                                                                <li id="adminMenuImmunizationManufacturer" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabImmunizationManufacturer" title="Manufacturer">Manufacturer</a>
                                                                </li>
                                                                <li id="adminMenuImmunizationRegistery" runat="server" visible="false">
                                                                    <a href="MDVisionAdmin.aspx#Admin/adminTabImmunizationRegistery" title="Registry">Registry</a>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuCCM" runat="server" visible="false">
                                                    <a>CCM</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuCCMTemplates" runat="server" visible="false"><a href="MDVisionAdmin.aspx#Admin/adminTabCCMTemplates" title="Templates">Templates</a></li>
                                                        <li id="adminMenuCCMICDGroups" runat="server" visible="false"><a href="MDVisionAdmin.aspx#Admin/adminTabCCMICDGroups" title="ICD Groups">ICD Groups</a></li>
                                                        <li id="adminMenuCCMCareTeam" runat="server" visible="false"><a href="MDVisionAdmin.aspx#Admin/adminTabCCMCareTeam" title="Care Team">Care Team</a></li>
                                                    </ul>
                                                </li>
                                                <li class="nav-parent" id="adminMenuReminders" runat="server" visible="false">
                                                    <a>Reminders</a>
                                                    <ul class="nav nav-children">
                                                        <li id="adminMenuRemindersTemplates" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabRemindersTemplates" title="Templates">Templates</a>
                                                        </li>
                                                        <li id="adminMenuRemindersSettings" runat="server" visible="false">
                                                            <a href="MDVisionAdmin.aspx#Admin/adminTabRemindersSettings" title="Settings">Settings</a>
                                                        </li>
                                                    </ul>
                                                </li>

                                            </ul>
                                        </li>
                                    </ul>
                                    <ul class="nav nav-main clinicalMenu" id="ClinicalUL">
                                        <li class="nav-parent" id="clinicalMenuFaceSheet" style="display: block">
                                            <a title="Face Sheet" class="noContentAfter" href="#mstrTabClinical" onclick="ClinicalMenuSettings.TopButtons('clinicalMenuFaceSheet')">
                                                <i class="fa fa-user" aria-hidden="true"></i>
                                                <span>Face Sheet</span>
                                            </a>
                                        </li>
                                        <li class="nav-parent" id="clinicalMenuMedical" runat="server" visible="false">
                                            <a data-toggle="tab" title="Medical" href="#mstrTabMedical" onclick="ClinicalMenuSettings.TopButtons('clinicalMenuMedical'); ">
                                                <i class="fa fa-medkit" aria-hidden="true"></i>
                                                <span>Medical</span>
                                            </a>
                                            <ul id="sortableMedical" class="nav nav-children">
                                                <li class="ui-state-default" id="clinicalMenu_Medical_Vitals" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabVitals','true');" title="Vitals">Vitals</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_ProblemLists" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabProblemLists','true');" title="Problems">Problems</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_Immunization" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabImmunization','true');" title="Immunization">Immunization</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_Allergies" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabAllergies','true');" title="Allergies">Allergies</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_Medications" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabMedications','true');" title="Medications">Medications</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_PatientEducation" runat="server">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabPatientEducation','true');" title="Patient Education">Patient Education </a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_Procedures" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabProcedures','true');" title="Procedures">Procedures</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_CDS_Alerts" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabCDSAlerts','true');" title="CDS Alerts">CDS Alerts</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_Implantable" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabImplantable','true');" title="Implantable Devices">Implantable Devices</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_CarePlan" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabCarePlan','true');" title="Care Plan">Care Plan</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Medical_Cognitive" runat="server" visible="false">
                                                    <a href="#mstrTabMedical" onclick="javascript:SelectTab('clinicalTabCognitive','true');" title="Functional Cognitive and Mental Status">Functional Cognitive and </br> Mental Status</a>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="nav-parent" id="clinicalMenuHistroy" runat="server" visible="false">
                                            <a data-toggle="tab" title="Histroy" href="#mstrTabHistroy" onclick="ClinicalMenuSettings.TopButtons('clinicalMenuHistroy'); ">
                                                <i class="fa fa-history" aria-hidden="true"></i><span>History</span>
                                            </a>
                                            <ul id="sortableHistory" class="nav nav-children">
                                                <li class="ui-state-default" id="clinicalMenu_History_HistorySummary" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabHistorySummary','true');" title="History Summary">History Summary</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_History_SocialHx" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabSocialHx','true');" title="Social Hx">Social Hx</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_History_MedicalHx" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabMedicalHx','true');" title="Medical Hx">Medical Hx</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_History_FamilyHx" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabFamilyHx','true');" title="Family Hx">Family Hx</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_History_SurgicalHx" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabSurgicalHx','true');" title="Surgical Hx">Surgical Hx</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_History_BirthHx" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabBirthHx','true');" title="Birth Hx">Birth Hx</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_History_HospitalizationHx" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabHospitalizationHx','true');" title="Hospitalization Hx">Hospitalization Hx</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_History_SocPsyandBehaviorHx" runat="server" visible="false">
                                                    <a href="#mstrTabHistroy" onclick="javascript:SelectTab('clinicalTabSocPsyandBehaviorHx','true');" title="Social, Psychological and Behavior Hx">Social, Psychological and Behavior Hx</a>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="nav-parent" id="clinicalMenuOrders" runat="server" visible="false">
                                            <a data-toggle="tab" title="Orders" href="#mstrTabOrders" onclick="ClinicalMenuSettings.TopButtons('clinicalMenuOrders');">
                                                <i class="fa fa-flask" aria-hidden="true"></i><span>Orders & Results</span>
                                            </a>
                                            <ul id="sortableOrders" class="nav nav-children">
                                                <li class="ui-state-default" id="clinicalMenu_Orders_Lab" runat="server" visible="false">
                                                    <a href="#mstrTabOrders" onclick="javascript:SelectTab('clinicalTabLabOrder','true');" title="Lab">Lab</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Orders_Procedure" runat="server" visible="false">
                                                    <a href="#mstrTabOrders" onclick="javascript:SelectTab('clinicalTabProcedureOrder','true');" title="Procedure">Procedure</a>
                                                </li>

                                                <li class="ui-state-default" id="clinicalMenu_Orders_Radiology" runat="server" visible="false">
                                                    <a href="#mstrTabOrders" onclick="javascript:SelectTab('clinicalTabRadiologyOrder','true');" title="Diagnostic Imaging">Diagnostic Imaging</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Orders_Consultation" runat="server" visible="false">
                                                    <a href="#mstrTabOrders" onclick="javascript:SelectTab('clinicalTabConsultationOrder','true');" title="Consultation">Consultation</a>
                                                </li>
                                                <li class="ui-state-default" id="clinicalMenu_Orders_OrdersImmunization" runat="server" visible="false">
                                                    <a href="#mstrTabOrders" onclick="javascript:SelectTab('clinicalTabQuestion','true');" title="Immunization">Immunization</a>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="nav-parent" id="clinicalMenuNotes" runat="server" style="display: block">
                                            <a data-toggle="tab" class="noContentAfter" title="Notes" href="#mstrTabNotes" onclick="ClinicalMenuSettings.TopButtons('clinicalMenuNotes'); ">
                                                <i class="fa fa-sticky-note" aria-hidden="true"></i>
                                                <span>Notes</span>
                                            </a>
                                        </li>

                                    </ul>
                                </nav>
                            </div>
                        </div>
                    </aside>
                </div>
                <section role="main" class="content-body">
                    <div id="pnlTab3" class="hidden-xs">
                        <div class="tab-content full-tab-row">
                            <div class="tab-pane active" id="mstrDivDashBoard">
                                <div class=" btn-block ">
                                </div>
                            </div>

                            <div class="tab-pane" id="mstrDivPatient">
                            </div>
                            <div class="tab-pane" id="mstrDivSchedule">
                                <button type="button" class="btn btn-default btn-sm active" onclick="SelectTab('schTabCalendar','false');" title="Schedule Calendar" id="schTabCalendar">Calendar</button>
                                <button type="button" class="btn btn-default btn-sm" onclick="SelectTab('schTabWaitList','false');" title="Wait List" id="schTabWaitList">Wait List </button>
                            </div>
                            <div class="tab-pane" id="mstrDivBilling">
                            </div>

                            <div class="tab-pane" id="mstrDivClinical">
                            </div>
                            <div class="tab-pane" id="mstrDivFaceSheet">
                            </div>
                            <div class="tab-pane" id="mstrDivMedical">
                            </div>
                            <div class="tab-pane" id="mstrDivHistroy">
                            </div>
                            <div class="tab-pane" id="mstrDivNotes">
                            </div>
                            <div class="tab-pane" id="mstrDivOrders">
                            </div>

                            <div class="tab-pane" id="mstrDivSpecialities">
                            </div>

                            <div class="tab-pane" id="mstrDivMiscellaneous">
                            </div>

                            <div class="tab-pane" id="mstrDivWomenHealth">
                            </div>

                            <div class="tab-pane" id="mstrDivTemplateBuilder">
                            </div>

                            <div class="tab-pane" id="mstrDivAdmin">
                            </div>

                            <div class="tab-pane" id="mstrDivBatch">
                            </div>
                            <div class="tab-pane" id="mstrDivAuditbleEvents">
                            </div>
                            <div class="tab-pane" id="mstrDiviTrack">
                            </div>
                        </div>
                    </div>

                    <div id="ctrlPanDashBoard" style="display: none;">
                    </div>
                    <div id="actionPanHomeDashBoard" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanHome" style="display: none;"></div>
                    <div id="actionPanHome" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanPatient" style="display: none;">
                    </div>
                    <div id="actionPanPatient" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanSchedule" style="display: none;">
                    </div>
                    <div id="actionPanSchedule" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>


                    <div id="actionPanClinical" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanClinical" style="display: none;"></div>
                    <div id="actionPanNote" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanNote" style="display: none;"></div>

                    <div id="actionPanBilling" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanBilling" style="display: none;">
                    </div>
                    <div id="ctrlPanBatch" style="display: none;">
                    </div>
                    <div id="actionPanBatch" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="actionPanReports" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanReports" style="display: none;">
                    </div>
                    <div id="ReLogin" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="containerHelpDocument" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="containerAbout" tabindex="-1" role="dialog" aria-hidden="true"></div>

                    <div id="ctrlPanAdmin" style="display: none;"></div>
                    <div id="actionPanAdmin" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div id="ctrlPanAuditbleEvents" style="display: none;"></div>

                    <div id="actionPanAuditbleEvents" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                      <div id="ctrlPaniTrack" style="display: none;"></div>

                    <div id="actionPaniTrack" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"></div>
                    <div class="clearfix"></div>
                </section>
            </div>
        </section>
        <input type="hidden" id="hfCurrentMenuParentLiId" />
        <input type="hidden" id="hfCurrentMenuChildLiId" />
        <input type="hidden" id="hfPrevMenuParentLiId" />
        <input type="hidden" id="hfPrevMenuChildLiId" />
        <input type="hidden" id="hfMainMenuParentLiId" />
        <input type="hidden" id="hfMainMenuChildLiId" />
        <input type="hidden" id="hfClinicalMenuParentLiId" />
        <input type="hidden" id="hfClinicalMenuChildLiId" />
    </form>

    <!--Start Print Preview For PDF Creation-->
 
    <style type="text/css">
        .blueBgPrint {
            background: #005da9 !important;
            color: #fff !important;
        }

        .blueBorderPrint {
            border-bottom: 2px solid #005da9;
        }

        /*.btn-link-print {
            color: #000000 !important;
            font-size: 16px;
            font-weight: 700;
        }*/

        .line-height-fix {
            font-size: 14px !important;
        }

        .bluefgColor {
            color: #005da9 !important;
        }

        .blackfgColor {
            color: black !important;
        }
    </style>
    <script type="x/kendo-template" id="page-templateSignNote">

        <div class="page-template" id="legacyViewTemplateDiv">
            #if(pageNum != 1) {#
            <div id="patientHeaderInfo" style="position: absolute;left: 50px;right: 50px;font-size: 90%;top: 7px;font-size: 110%;" ul class="bluefgColor">
                Patient Name: <span id="patientname" class="blackfgColor"></span><span class="whiteColorPrint" style="float:right;">Provider Name: <span id="providerName" class="blackfgColor"></span></span>
            </div>
            <div id="patientHeaderInfo" style="position: absolute;left: 50px;right: 50px;font-size: 90%;top: 28px;font-size: 110%;" ul class="bluefgColor">
                DOB: <span id="patientDOB" class="blackfgColor"></span><span class="whiteColorPrint" style="float:right;">DOS: <span id="patientDOS" class="blackfgColor"></span></span>
            </div>
            #}#
            <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">
                <div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>
                <div id="ClinicalReportsFooterTextLegacy" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">
                    Generated by:<span id="ClinicalReportsFooterLegacy">MDVISION PM EMR</span><span class="whiteColorPrint" style="float:right;"> Page #:pageNum# of #:totalPages#</span>
                </div>
            </div>

        </div>
    </script>
    <div id="signNotePrint" style="display: none">
        <div id="PatientInfo" class="">
            <div class="col-sm-4 col-lg-2 pull-left">
                <img src="content/images/SHS-nav-logo-small-100.png" class="img-responsive">
            </div>
            <!--Begin Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090-->
            <ul class="list-unstyled pull-right line-height-fix" id="PracticeList">
                <li id="PracticeName" class="text-right"></li>
                <li id="PracticeAddress" class="text-right"></li>
                <li id="PracticeCity" class="text-right"></li>
                <li id="PracticePhone" class="text-right"></li>
            </ul>
            <div class="clearfix"></div>
            <div class="splitter m-none mt-xs">
                <div class="spacer3"></div>
            </div>

            <div class="spacer3"></div>
            <ul class="list-unstyled pull-left line-height-fix" id="PatientList">
                <li id="PatientName"></li>
                <li id="PatientAge"></li>
                <li id="PatientAccount"></li>
                <li id="PatientPhone"></li>
                <li id="PatientAddress"></li>
            </ul>

            <ul class="list-unstyled pull-right line-height-fix" id="ProviderList">
                <li id="ProviderName" class="text-right"></li>
                <li id="Specialty" class="text-right"></li>
                <li id="NoteDateTime" class="text-right"></li>
                <li id="VisitReason" class="text-right"></li>
                <!--END Edited By Fahad Malik on 28-11-2016 to fix bug#: EMR-2090-->
            </ul>
        </div>
        <div class="spacer5"></div>
        <!--<div class="splitter m-none  pl-sm pt-xxs pb-xxs">Initial Office Visit</div>-->

        <section class="panel panel-featured mt-lg">


            <div class="panel-body bg-white" id="ulContent">
            </div>
        </section>
    </div>
    <!--End Print Preview For PDF Creation-->

    <%: Scripts.Render("~/bundles/StartupBilling") %>

    <script type="text/javascript">
        //Set ToolTip.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip();
    $(function () {
        $(".sticky").sticky({
            topSpacing: 154,
            zIndex: 1019,
            stopper: "#footer"
        });
        var isTestDB = '<%=TestDb %>';
        var isEMRRequire = '<%=IsEmrRequire %>';
        //if (getParameterByName('isPopUp') == 'true') {
        if (getParameterByName('isPopUp') == 'true' || isTestDB.toLowerCase() == 'false') {
            EnableProtectedMode();
        }
        if (isEMRRequire.toLowerCase() == 'false') {
            $('#mstrTabClinical').hide();
            $('#mstrMenuClinical').hide();
            $('#Notes').hide();
        }


        // This is a temporary Implementation until we apply SignalR for all Quicj Links
        setInterval(function () {

            DashBoard.RefreshTasksCount().done(function (responseTasksCount) {

                if (responseTasksCount > 0) {
                    if (responseTasksCount != Number($("#spnUserTasksCount").html())) {
                        $("#spnUserTasksCount").empty();
                        $("#spnUserTasksCount").html(responseTasksCount);
                    }
                }
                else {

                }
            })

        }, 10000);
    
    });
    
    function EnableProtectedMode() {
        shortcut.add("Ctrl+Shift+I", function () { });
        shortcut.add("Ctrl+U", function () { });
        shortcut.add("Ctrl+S", function () { });
        shortcut.add("Ctrl+Shift+J", function () { });
        shortcut.add("F12", function () { });
        //Enable Right Click
        shortcut.add("Ctrl+Shift+L", function () {
            $('body').attr('oncontextmenu', 'return false');
        });
        //Disable Right Click
        shortcut.add("Ctrl+Shift+U", function () {
            $('body').removeAttr('oncontextmenu');
        });
        $(document).find('html header.header').get(0).ondblclick = function () {
            launchFullscreen(document.documentElement);
        };
        $('body').attr('oncontextmenu', 'return false');
    }
    function launchFullscreen(element) {
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) {
            element.msRequestFullscreen();
        }
    }
    function exitFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    }
    function getParameterByName(name) {
        var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    }

  

    function RefreshTasksCount() {
        var objData = {
        };
        objData["AppUserId"] = globalAppdata['AppUserId'];

        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSync(data, "Messages", "TasksCount");
    }

    </script>
</body>

</html>
