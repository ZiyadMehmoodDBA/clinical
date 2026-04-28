<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="MDVision.IEHR.Controls.Reports.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

 
    <%--<link href="~/Content/Default/bootstrap.css" rel="stylesheet"  type="text/css" />--%>
    <link type="text/css" rel="stylesheet" href="~/Content/Blue/bootstrap.css" />
    <style>
        @page {
            size: landscape;
            -webkit-transform: rotate(-90deg);
            -moz-transform: rotate(-90deg);
            filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);
        }
    </style>
</head>
<body oncontextmenu="return false">
    <form id="form1" runat="server">
        <div>

            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true" AsyncPostBackTimeout ="0"></asp:ScriptManager>
            <script type="text/javascript" language="javascript">
                //$(document).ready(function () { BackgroundLoaderShow(true); });
                Sys.Application.add_load(function () {
                   $find("RptViewer").add_propertyChanged(viewerPropertyChanged);
                });

                function viewerPropertyChanged(sender, e) {
                    if (e.get_propertyName() == "isLoading") {
                        if ($find("RptViewer").get_isLoading()) {
                            // Do something when loading starts
                            setTimeout(function () {
                                // $('#myIframe',
                                window.parent.$('#BackgroundLoader').show()
                                //).show();
                            }, 200);
                        }
                        else {
                            // Do something when loading stops
                            setTimeout(function () {
                                window.parent.$('#BackgroundLoader').hide();
                                setTimeout(function () {
                                    var contents = parent.iframeLoadedContent();

                                }, 100);
                            }, 200);
                        }
                    }
                };
            </script>
            <rsweb:ReportViewer ID="RptViewer" runat="server" AsyncRendering="True" SizeToReportContent="true"
                ProcessingMode="Remote" Width="100%" BackColor="#F7F8F9" Height="" PageCountMode="Actual">
            </rsweb:ReportViewer>

        </div>


    </form>
</body>
    <script src="../../Scripts/js/jquery-2.1.1.js"></script>
<script src="../../Scripts/js/bootstrap.js"></script>

</html>
