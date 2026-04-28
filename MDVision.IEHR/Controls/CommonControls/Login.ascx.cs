using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MDVision.IEHR.Controls.CommonControls
{
    public partial class Login : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["logout"] == "1")
            {
                //logout
                //AppUser.Logout(this.Page, "~/default.aspx");

            }
            //Login.Attributes.Add("onmouseover", "this.className='loginBtnInputHover';");
            //Login.Attributes.Add("onmouseout", "this.className='loginBtnInput';");
            //Cancel.Attributes.Add("onmouseover", "this.className='loginBtnInputHover';");
            //Cancel.Attributes.Add("onmouseout", "this.className='loginBtnInput';");

        }

        protected void Login_Click(object sender, EventArgs e)
        {
            string username = LoginUserName.Text.ToUpper().Trim();
            string password = PasswordInput.Value.Trim();


            //string errMsg = "";

            //bool result = WebEMR.Common.AppUser.Login(username.ToUpper(), password, ref errMsg);
            //string returnURL = WebEMR.Util.Utility.IsNullStr(Request["ReturnUrl"], "~/default.aspx");
            //if (result)
            //    Response.Redirect(returnURL);
            //else
            //    lblErrMsg.Text = result == false ? errMsg : "";
            Response.Redirect("MDVisionDefault.aspx");
        }
    }
}