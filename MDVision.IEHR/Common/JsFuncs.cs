using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;

namespace MDVision.IEHR.Common
{

    public class JsFuncs
    {

        #region "Ajax Funcs"
        public static void MsgBox(Control page, string msg)
        {
            
            msg = msg.Replace("\"", "\\\"");
          

            ScriptManager.RegisterClientScriptBlock(page, page.GetType(),
                                                    "jsm" + new Random().ToString().Replace(".", ""),
                                                    "alert('" + msg + "')", true);


        }

        public static bool ConfirmBox(Control page, string msg, string hiddenField2Set)
        {
          
            msg = msg.Replace("\"", "\\\"");
          


            ScriptManager.RegisterClientScriptBlock(page, page.GetType(),
                                                    "jsc" + new Random().ToString().Replace(".", ""),  "document.getElementById('" + hiddenField2Set + "').value = confirm('" + msg + "')", true);

            return false;
        }

        public static void RegisterJS(Control page, string script)
        { 
            script = script.Replace("\"", "\\\"");
          

            if (page != null)
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(),
                                                     Guid.NewGuid().ToString() + new Random().ToString().Replace(".", ""), script, true);
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(), Guid.NewGuid().ToString() + new Random().ToString().Replace(".", ""), script, true);

        }

        public static void RunJS(Control page, string script)
        { 
            script = script.Replace("\"", "\\\"");
    

            if (page != null)
                ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString() + new Random().ToString().Replace(".", ""), script, true);

        }
        public static void Redirect(Control page, string url)
        {
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(),
                                                    "jsrd" + new Random().ToString().Replace(".", ""), "window.open('" + url + "', '_parent')", true);
        }

        #endregion

        #region "JQuery"
        private static string GetjQueryCode(string jsCodetoRun)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("$(document).ready(function() {");
            sb.AppendLine(jsCodetoRun);
            sb.AppendLine(" });");

            return sb.ToString();
        }

        private static void RunjQueryCode(Page aspxPage, string jsCodetoRun)
        {

            ScriptManager requestSM = ScriptManager.GetCurrent(aspxPage);
            if (requestSM != null && requestSM.IsInAsyncPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(aspxPage, typeof(Page), Guid.NewGuid().ToString(), GetjQueryCode(jsCodetoRun), false);
            }
            else
            {
                aspxPage.ClientScript.RegisterClientScriptBlock(typeof(Page),
                                                       Guid.NewGuid().ToString(),
                                                       GetjQueryCode(jsCodetoRun),
                                                       false);
            }
        }


        #endregion
         
    }
}
