using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Configuration;
using MDVision.MDVision.WebAPI;
using MDVision.WebAPI.Results;
using MDVision.Model.Security;
using MDVision.DataAccess.DAL.Admin;

namespace MDVision.WebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AccountController()
        {

            _repo = new AuthRepository();
        }

        // POST api/Account/Register
        //[AllowAnonymous]
        //[Route("Register")]
        //public async Task<IHttpActionResult> Register(UserModel userModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    IdentityResult result = await _repo.RegisterUser(userModel);

        //    IHttpActionResult errorResult = GetErrorResult(result);

        //    if (errorResult != null)
        //    {
        //        return errorResult;
        //    }

        //    return Ok();
        //}

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            //  var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            //if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            //{
            //    return BadRequest(redirectUriValidationResult);
            //}

            // ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            //if (externalLogin == null)
            //{
            //    return InternalServerError();
            //}

            //if (externalLogin.LoginProvider != provider)
            //{
            //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //    return new ChallengeResult(provider, this);
            //}

            //IdentityUser user = await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            // bool hasRegistered = user != null;

            //redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
            //                                redirectUri,
            //                                externalLogin.ExternalAccessToken,
            //                                externalLogin.LoginProvider,
            //                                hasRegistered.ToString(),
            //                                externalLogin.UserName);

            return Redirect(redirectUri);

        }

        //// POST api/Account/RegisterExternal
        //[AllowAnonymous]
        //[Route("RegisterExternal")]
        //public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
        //    if (verifiedAccessToken == null)
        //    {
        //        return BadRequest("Invalid Provider or External Access Token");
        //    }

        //    IdentityUser user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

        //    bool hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        return BadRequest("External user is already registered");
        //    }

        //    user = new IdentityUser() { UserName = model.UserName };

        //    IdentityResult result = await _repo.CreateAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    var info = new ExternalLoginInfo()
        //    {
        //        DefaultUserName = model.UserName,
        //        Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
        //    };

        //    result = await _repo.AddLoginAsync(user.Id, info.Login);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    //generate access token response
        //    var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

        //    return Ok(accessTokenResponse);
        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("ObtainLocalAccessToken")]
        //public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        //{

        //    if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
        //    {
        //        return BadRequest("Provider or external access token is not sent");
        //    }

        //    var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
        //    if (verifiedAccessToken == null)
        //    {
        //        return BadRequest("Invalid Provider or External Access Token");
        //    }

        //    IdentityUser user = await _repo.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

        //    bool hasRegistered = user != null;

        //    if (!hasRegistered)
        //    {
        //        return BadRequest("External user is not registered");
        //    }

        //    //generate access token response
        //    var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

        //    return Ok(accessTokenResponse);

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        //[Authorize]
        [Authorize]
        [Route("myAccount")]
        [HttpGet]
        public string myAccount(string data)
        {
            JObject OBJ = new JObject();
            OBJ.Add("NAME", "MOHSIN");
            //return Ok(OBJ as dynamic);
            return "";
        }

        [Authorize]
        [Route("ChangePassword")]
        [HttpGet]
        public string ChangePassword(string data)
        {
            // DALPatientLogin objDALPatient = new DALPatientLogin();
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            Dictionary<string, string> objData = serialize.Deserialize<Dictionary<string, string>>(data);
            //objDALPatient.UpdatePatientPassword(Convert.ToInt32(objData["patientId"]), Utility.Encrypt(objData["newPassword"]));
            return "";
        }
        [Route("SaveEntity")]
        [HttpPost]

        public IHttpActionResult SaveEntity(EntityData Model)
        {
            bool status = WebApiCommon.CheckNetwrokIP(Model.NetWorkIP);
            if (status == false)
            {
                var response = new
                {
                    status = true,
                    Message = "Not a part of our system network.",
                   

                };
                return Json(response);
            }
            else
            {
                string message = DALCustomers.Instance.SaveEntityData(Model);
                if (message == null)
                {
                    var response = new
                    {
                        status = true,
                        message = ""
                    };
                    return Json(response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = message
                    };
                    return Json(response);
                }

            }
        }
        ////[Authorize]
        //[Route("ForgotPassword")]
        //[HttpGet]
        //public bool ForgotPassword(string data)
        //{
        //    DALPatientLogin objDALPatient = new DALPatientLogin();
        //    JavaScriptSerializer serialize = new JavaScriptSerializer();
        //    Dictionary<string, string> objData = serialize.Deserialize<Dictionary<string, string>>(data);
        //    string userName = objData["UserName"];
        //    int securityQuestion = Convert.ToInt32(objData["SecurityQuestion"]);
        //    string securityAnswer = objData["SecurityAnswer"];
        //    DSPatient ds = objDALPatient.LoadPatientLogin(0, userName);
        //    DSPatient.PatientLoginRow dr = ds.Tables[ds.PatientLogin.TableName].Rows[0] as DSPatient.PatientLoginRow;
        //    if (
        //        Convert.ToInt32(dr[ds.PatientLogin.SecurityQuestionIdColumn.ColumnName]) == securityQuestion
        //        &&
        //        dr[ds.PatientLogin.SecurityAnswerColumn].ToString() == securityAnswer
        //        )
        //    {
        //        if (!string.IsNullOrWhiteSpace(dr.EmailAddress))
        //        {
        //            MailMessage mail = new MailMessage();
        //            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

        //            mail.From = new MailAddress("mdvisionangular@gmail.com");
        //            mail.To.Add(dr.EmailAddress);
        //            mail.Subject = "Forgot Password, Here you get!";

        //            LinkedResource inlineLogo;
        //            string Body = GetFormattedEmailMessage(dr.UserName, Utility.Decrypt(dr.Password), out inlineLogo);
        //            var view = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
        //            view.LinkedResources.Add(inlineLogo);
        //            mail.AlternateViews.Add(view);

        //            //mail.Body = GetFormattedEmailMessage(dr.UserName, Utility.Decrypt(dr.Password));
        //            mail.IsBodyHtml = true;
        //            SmtpServer.Port = 587;
        //            SmtpServer.Credentials = new NetworkCredential("mdvisionangular@gmail.com", "mdvision123");
        //            SmtpServer.EnableSsl = true;

        //            SmtpServer.Send(mail);

        //            return true;
        //        }
        //    }


        //    else
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //[Authorize]
        //[Route("ResetAccount")]
        //[HttpGet]
        //public string ResetAccount(string data)
        //{
        //    DALPatientLogin objDALPatient = new DALPatientLogin();
        //    JavaScriptSerializer serialize = new JavaScriptSerializer();
        //    Dictionary<string, string> objData = serialize.Deserialize<Dictionary<string, string>>(data);

        //    int PatientId = Convert.ToInt32(objData["patientId"]);
        //    string patientUserName = objData["patientUserName"];
        //    string oldPassword = Utility.Encrypt(objData["oldPassword"]);
        //    string newPassword = Utility.Encrypt(objData["newPassword"]);

        //    string securityQuestionId = objData["squestion"];
        //    string securityAnswer = objData["answer"];

        //    DSPatient ds = new DSPatient();
        //    ds = objDALPatient.LoadPatientLogin(0, patientUserName);
        //    if (!string.IsNullOrEmpty(oldPassword))
        //    {
        //        DSPatient.PatientLoginRow dr = null;
        //        if (ds.Tables[ds.PatientLogin.TableName].Rows.Count > 0)
        //        {
        //            dr = ds.Tables[ds.PatientLogin.TableName].Rows[0] as DSPatient.PatientLoginRow;
        //            if (dr[ds.PatientLogin.PasswordColumn.ColumnName].ToString() == oldPassword)
        //            {
        //                try
        //                {
        //                    objDALPatient.UpdatePatientPassword(PatientId, newPassword);
        //                    var response = new
        //                    {
        //                        status = true,
        //                        message = "Password updated",
        //                    };
        //                }
        //                catch (Exception e)
        //                {
        //                    var response = new
        //                    {
        //                        status = false,
        //                        message = e.Message
        //                    };
        //                }


        //            }
        //            else
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    message = "old password not matched"
        //                };

        //                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //            }
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(securityQuestionId) && securityQuestionId != "0")
        //    {
        //        DSPatient dsPatientLogin = new DSPatient();
        //        dsPatientLogin = objDALPatient.LoadPatientLogin(0, patientUserName);

        //        if (dsPatientLogin.Tables[dsPatientLogin.PatientLogin.TableName].Rows.Count > 0)
        //        {
        //            dsPatientLogin.Tables[dsPatientLogin.PatientLogin.TableName].Rows[0][dsPatientLogin.PatientLogin.SecurityQuestionIdColumn.ColumnName] = Convert.ToInt32(securityQuestionId);
        //            dsPatientLogin.Tables[dsPatientLogin.PatientLogin.TableName].Rows[0][dsPatientLogin.PatientLogin.SecurityAnswerColumn.ColumnName] = securityAnswer;

        //            dsPatientLogin.Tables[dsPatientLogin.PatientLogin.TableName].Rows[0][dsPatientLogin.PatientLogin.ModifiedOnColumn.ColumnName] = DateTime.Now;

        //            objDALPatient.UpdatePatientLogin(dsPatientLogin);
        //        }

        //    }
        //    var resp = new
        //    {
        //        status = true,
        //        message = "Succesfully Updated",
        //    };

        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(resp));
        //}

        ////[Authorize]
        //[Route("LockAccount")]
        //[HttpGet]
        //public string LockAccount(string data)
        //{
        //    DALPatientLogin objDALPatient = new DALPatientLogin();
        //    JavaScriptSerializer serialize = new JavaScriptSerializer();
        //    Dictionary<string, string> objData = serialize.Deserialize<Dictionary<string, string>>(data);


        //    string patientUserName = objData["userName"];
        //    DSPatient ds = new DSPatient();
        //    DSPatient dsPatientLogin = new DSPatient();
        //    dsPatientLogin = objDALPatient.LoadPatientLogin(0, patientUserName);
        //    var resp = new Object();
        //    try
        //    {
        //        if (dsPatientLogin.Tables[dsPatientLogin.PatientLogin.TableName].Rows.Count > 0)
        //        {
        //            dsPatientLogin.Tables[dsPatientLogin.PatientLogin.TableName].Rows[0][dsPatientLogin.PatientLogin.UnLockAccountColumn.ColumnName] = false;
        //            objDALPatient.UpdatePatientLogin(dsPatientLogin);
        //            resp = new
        //            {
        //                status = true,
        //                message = "Your Account is Locked/Disabled . Please contact System Administrator ",
        //            };
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resp = new
        //       {
        //           status = false,
        //           message = e.Message,
        //       };
        //    }
        //    return (Newtonsoft.Json.JsonConvert.SerializeObject(resp));
        //}





    }
}

