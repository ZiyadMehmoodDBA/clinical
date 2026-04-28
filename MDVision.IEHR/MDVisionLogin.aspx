<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MDVisionLogin.aspx.cs" Inherits="MDVision.IEHR.Account.MDVisionLogin" %>

<!doctype html >
<html>

<head>
    <meta charset="utf-8">
    <title>MD Vision CSD LLC</title>
    <%--<link href="http://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800|Shadows+Into+Light" rel="stylesheet" type="text/css">--%>

    <link href="Content/Default/GoogleFonts.css" rel="stylesheet" />
    <!-- Mobile Metas -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- Vendor CSS -->
    <link rel="stylesheet" href="Content/Default/bootstrap.css" />
    <link rel="stylesheet" href="Content/style.css" />
    <link rel="stylesheet" href="Content/Default/font-awesome.css" />
    <link rel="shortcut icon" href="favicon.png" />
    <link rel="icon" href="favicon.ico?v=2" />

    <script>
        var wndw;
        function openNewWindow(url) {
            url = url + '?isPopUp=true';
            var fullWidth = screen.width;
            var fullHeight = screen.height
            wndw = window.open(url, "MDVision", "status=0, toolbar=0, location=0, menubar=0, directories=0, resizable=1, scrollbars=1, top=0, left=0,width=" + fullWidth + ",height=" + fullHeight + "");
            wndw.onload = function () {
            }
        }
    </script>
</head>

<body oncontextmenu="return false">
    <div class="container-fluid container-border-top">
        <div class="container">
            <div class="col-md-3 col-sm-4">
                <img class="img-responsive logo-adjest" src="Content/images/mdvision-logo.png" alt="MDVision">
            </div>

            <div class="col-md-3 col-sm-5 col-xs-12 member-login pull-right">

                <div class="col-md-12 col-sm-12 col-xs-12">
                    <form id="form1" runat="server">

                        <asp:Panel ID="PanelLogin" runat="server" DefaultButton="btnLogin">
                            <div class="clearfix"></div>
                            <h1>
                                <asp:Label ID="lblFormTitle" runat="server" Text="Label">Member Login</asp:Label></h1>
                            <div class="clearfix"></div>
                            <table runat="server" id="tdlogin" style="width: 100%;">
                                <tr>
                                    <td></td>

                                </tr>
                                <tr>

                                    <td>
                                        <div class="input-group margin-btm20">
                                            <span class="input-group-addon i-blu-color"><i class="fa fa-user"></i></span>


                                            <asp:TextBox ID="txtLoginUserName" TabIndex="1" CssClass="input-size-adj form-control" runat="server" placeholder="Enter User Name" onfocus="if(this.value=='Enter User Name') this.value='';" onblur="if(this.value=='') this.value='Enter User Name';"></asp:TextBox>
                                        </div>
                                    </td>

                                </tr>
                                <tr>

                                    <td>
                                        <div class="input-group margin-btm20">
                                            <div class="input-group-addon i-blu-color" style=""><i class="fa fa-lock"></i></div>
                                            <asp:TextBox ID="txtLoginPassword" TabIndex="2" CssClass="input-size-adj form-control" runat="server" TextMode="Password" placeholder="Password" onfocus="if(this.value=='Password') this.value='';" onblur="if(this.value=='') this.value='Password';"></asp:TextBox>
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnLogin" class="btn btn-primary btn-lg" runat="server" TabIndex="3" Text="Login" OnClick="Login_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="versionText">
                                        <asp:Label ID="lblAppVersionNo" runat="server" Text="Version 5.1"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" />
                                    </td>
                                </tr>
                            </table>

                            <table runat="server" id="tblChangePassword" style="width: 100%;" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPwdChangeMsg" ForeColor="Red" runat="server" Text="Your Password has Expired Please choose new Password"></asp:Label>

                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <div class="input-group margin-btm20">
                                            <span class="input-group-addon i-blu-color"><i class="fa fa-lock"></i></span>
                                            <asp:TextBox ID="txtNewPassword" TabIndex="2" CssClass="input-size-adj form-control" runat="server" TextMode="Password" placeholder="New Password" ></asp:TextBox>
                                        </div>
                                    </td>

                                </tr>
                                <tr>

                                    <td>
                                        <div class="input-group margin-btm20">
                                            <div class="input-group-addon i-blu-color" style=""><i class="fa fa-lock"></i></div>
                                            <asp:TextBox ID="txtConfirmPassword" TabIndex="2" CssClass="input-size-adj form-control" runat="server" TextMode="Password" placeholder="Confirm Password"></asp:TextBox>
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnChangePassword" class="btn btn-primary btn-lg" runat="server" TabIndex="3" Text="Change Password" OnClick="ChangePassword_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="versionText">
                                        <asp:Label ID="Label1" runat="server" Text="Version 5.1"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPasswordChangeError" runat="server" ForeColor="Red" />
                                    </td>
                                </tr>
                            </table>

                            <div runat="server" id="tbEntity" visible="false">

                                <div class="entity-box">

                                    <asp:BulletedList ID="blistEntity" CssClass="selectionBTN" runat="server" DisplayMode="LinkButton" OnClick="OnlistSelection">
                                    </asp:BulletedList>
                                </div>
                                <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" CssClass="btn btn-primary btn-cancel btn-lg" Text="Cancel" />


                                <asp:Label ID="lblProfileLoginError" runat="server" ForeColor="Red" />


                            </div>
                        </asp:Panel>
                    </form>
                </div>
            </div>


        </div>
    </div>
    <footer>
        <div class="container">
            <p>&copy; <%=DateTime.Now.Year.ToString()%> MD Vision CSD LLC</p>
        </div>
    </footer>

    <%: Scripts.Render("~/bundles/3rdPartyjs") %>
    <%: Scripts.Render("~/bundles/Common") %>

    <script type="text/javascript">
        $(document).ready(function () {

            var images = ['01.jpg', '02.jpg'];
            localStorage.removeItem("PatientSelectedScreen");
            localStorage.removeItem("BatchSelectedScreen");
            $('html').css({ 'background-image': 'url(Content/images/' + images[Math.floor(Math.random() * images.length)] + ')' });

            localStorage.removeItem("BatchSelectedScreen");
            localStorage.removeItem("PatientSelectedScreen");
        });
    </script>
    <script src="Scripts/js/tinyMCE/js/tinymce/tinymce.min.js"></script>
</body>
</html>

