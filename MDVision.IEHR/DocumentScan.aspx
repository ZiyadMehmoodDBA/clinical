<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentScan.aspx.cs" Inherits="MDVision.IEHR.Controls.CommonControls.DocumentScan" %>

<!DOCTYPE html>
<html class="fixed sidebar-left-collapsed" style="height: 100%">
<head>
    <title>MDVision Document Scan</title>
    <link rel="shortcut icon" href="favicon.png" />
    <link rel="icon" href="favicon.ico?v=2" />
    <meta charset="UTF-8">
    <!-- Mobile Metas -->
    <meta name="viewport" content="width=device-width" />

    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />

    <!-- Web Fonts  -->

    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800|Shadows+Into+Light" rel="stylesheet" type="text/css">
        <link href="Content/Default/font-awesome.css" rel="stylesheet" />

        <%: Styles.Render("~/bundles/ScanCSS") %>
        <%: Styles.Render("~/bundles/Default") %>
        <%: Styles.Render("~/bundles/Blue") %>
        <%: Scripts.Render("~/bundles/Scan3rdPartyJS") %>
         <script src="Scripts/js/Kendo/kendo.all.min.js"></script>
        <%: Scripts.Render("~/bundles/ScanJS") %>

        <style>
            /*Ticket no AST-41*/
            .imgTag_Selected {
                background-color: #fff !important;
                border: none !important;
            }

            .ds-dwt-container-box > div > canvas:nth-child(2) {
                display: none !important;
            }
        </style>

    </asp:PlaceHolder>
</head>

<body onload="OpenDocumentScan()">
    <form id="form1" runat="server">
        <div class='uil-ring-css' style='-webkit-transform: scale(0.97); display: none;' id="BackgroundLoader"></div>
        <div id="pnlPatientInsurance">
            <div id="actionPanPatientInsurance">
            </div>
        </div>
        <div id="pnlDemographicQuick">
            <div id="actionPanDemographicQuick">
            </div>
        </div>
        <div id="pnlDemographic">
             <div id="actionPanDemographic"></div>
            <div id="pnldemographicDetail">
                <div id="actionPandemographicDetail">
                </div>
            </div>
            <div id="frmDemographic">
                <input type="hidden" id="hfPractice" />
            </div>
        </div>
    </form>
</body>

</html>
