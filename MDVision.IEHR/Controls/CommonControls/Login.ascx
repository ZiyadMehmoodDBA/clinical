<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="MDVision.IEHR.Controls.CommonControls.Login" %>
<%@ Register src="~/Controls/Form/TextBox.ascx" tagname="TextBox" tagprefix="MDVision" %>
  <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
    </asp:PlaceHolder>
<asp:HiddenField ID="PasswordInput" ClientIDMode="Static" runat="server" />
<asp:Panel ID="pnlLogin" runat="server" DefaultButton="LoginButton">
<TABLE CELLSPACING="0" CELLPADDING="0" BORDER="0" class="loginLayout" height="100%" id="loginScreen">
	<TR>
		<TD VALIGN="middle" ALIGN="center" height="100%">
			 
    
                      <table border="0" cellspacing="0" cellpadding="0" class="LoginTable">
      <tr>
        <td width="358"></td>
        <td width="365" align="left"><table width="300" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td align="left" valign="top" class="heading">MDVision iEHR</td>
          </tr>
          <tr>
            <td align="left" class="text3">MDVision has combined the forces of experience and expertise in developing the point-of-care Web EMR that sets you free from the limits of time and space, â€˜practice now moving on the webâ€™. It is as simple in use as to surf the web and as advanced in design &amp; features that offers all what you need for a complete health care service. </td>
          </tr>
          <tr>
            <td align="left" class="heading"><img class="loginArrow" src="images/sp.gif" width="22" height="17" />Meaningful Use</td>
          </tr>
          <tr>
            <td align="left" class="text3">Meets all of the Meaningful Use measures and objectives. Tested and endorsed by physicians as criteria at its own.</td>
          </tr>
          <tr>
            <td align="left" class="heading"><img class="loginArrow" src="images/sp.gif" width="22" height="17" />Secure and Reliable</td>
          </tr>
          <tr>
            <td align="left" class="text3">Creates a secure working environment. Advanced security protocols come to work from the login and utilize sophisticated data protection algorithms.</td>
          </tr>
          <tr>
            <td height="180" align="left"></td>
          </tr>
        </table></td>
        <td width="293" align="left">
            
          <table width="238" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td colspan="2" align="left" class="text2">Username</td>
          </tr>
          <tr>
            <td width="238" height="42" colspan="2" align="center" class="text-field"><table width="216" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="26" height="22" align="center"><img class="iconUser" src="images/sp.gif" width="26" height="22" /></td>
                <td width="190" align="center">
                    <asp:TextBox ID="LoginUserName" TabIndex="1" CssClass="loginTxtInput" runat="server" ValidationGroup="Login_Validation"></asp:TextBox>
                                            
                
                
                </td>
              </tr>
            </table></td>
          </tr>
          <tr>
            <td colspan="2" align="left" class="text2">Password</td>
          </tr>
          
          <tr>
            <td width="238" height="42" colspan="2" align="center" class="text-field"><table width="216" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="26" height="22" align="center"><img class="iconPassword" src="images/sp.gif" /></td>
                <td width="190" align="center">
                <asp:TextBox ID="LoginPassword" TabIndex="2" CssClass="loginTxtInput" runat="server" ValidationGroup="Login_Validation"></asp:TextBox>
                                              
                
                </td>
              </tr>
            </table></td>
          </tr>
          <tr>
            <td width="134"><u><a class="text" href="#">Forgot your password</a></u></td>
            <td align="right" style="width:146;">
                <asp:LinkButton runat="server" ID="LoginButton" CssClass="btn_login" TabIndex="3" Text="Sign in" ValidationGroup="Login_Validation" OnClick="Login_Click"><img src="images/sp.gif"  border="0" /></asp:LinkButton>
            </td>
          </tr>
          <tr>
            <td height="230" colspan="2" valign="top" style="padding-top:20px;padding-left:10px;">
                <asp:RequiredFieldValidator runat="server" ID="rfvUserName" ControlToValidate="LoginUserName" ValidationGroup="Login_Validation" ErrorMessage="&lt;br /&gt;Username is Required to login to MDVision application&lt;br /&gt;" Display="Dynamic" EnableViewState="false"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator runat="server" ID="rfvLoginPassword" ControlToValidate="LoginPassword" ValidationGroup="Login_Validation" ErrorMessage="<br />Password is Required<br />" Display="Dynamic" EnableViewState="false"></asp:RequiredFieldValidator>
                <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" />
            </td>
          </tr>
        </table></td>
      </tr>
    </table>


        </TD>
	</TR>
</TABLE>


</asp:Panel>
<script language="javascript" type="text/javascript">
    $("#loginScreen").height($(document.getElementsByTagName('body')[0]).height());
    //alert($("#loginScreen").height());
    //alert($(document.getElementsByTagName('body')[0]).height());
</script>

