using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using System.Net.Mime;
using System.Configuration;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Patient.PatientPortal
{
    public class Patient_AccountManager
    {
        private BLLPatient BLLPatientObj = null;
        public Patient_AccountManager()
        {
            BLLPatientObj = new BLLPatient();
        }

        #region Singleton
        private static Patient_AccountManager _obj = null;
        public static Patient_AccountManager Instance()
        {
            if (_obj == null)
                _obj = new Patient_AccountManager();
            return _obj;
        }
        #endregion

        #region Private Functions

        #region "Generate Password"
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion


        private string SavePatientAccountManager(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                //  DSPayment dsAdvancePayment = new DSPayment();
                DSPatientPortal dsPatientPortal = new DSPatientPortal();

                DSPatientPortal.PatientLoginRow dr = dsPatientPortal.PatientLogin.NewPatientLoginRow();
                // DSPayment.AdvancePaymentRow dr = dsAdvancePayment.AdvancePayment.NewAdvancePaymentRow();


                string userName = MDVUtility.ToStr(SearchedfieldsJSON["txtUserName"]);
                //string password = MDVUtility.ToStr(SearchedfieldsJSON["txtpassword"]);
                string password = RandomString(8);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtUserName"]))
                    dr.UserName = MDVUtility.ToStr(SearchedfieldsJSON["txtUserName"]);


                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtpassword"]))
                    dr.Password = MDVUtility.EncryptTo64(password);

                if (!string.IsNullOrEmpty(SearchedfieldsJSON["txtAccountEmail"]))
                    dr.Email = MDVUtility.ToStr(SearchedfieldsJSON["txtAccountEmail"]);


                if (!string.IsNullOrEmpty(SearchedfieldsJSON["ddlSecurityQuestion"]))
                    dr.SecurityQuestion = MDVUtility.ToStr(SearchedfieldsJSON["ddlSecurityQuestion"]);

                if (!string.IsNullOrWhiteSpace(SearchedfieldsJSON["txtAnswer"]))
                    dr.SecurityAnswer = MDVUtility.ToStr(SearchedfieldsJSON["txtAnswer"]);

                //if (SearchedfieldsJSON["unlockAccount"] != null)
                //    dr.UnLockAccount = (bool)SearchedfieldsJSON["unlockAccount"];
                if (SearchedfieldsJSON["unlockAccount"] != null)
                    dr.UnLockAccount = MDVUtility.ToStr(SearchedfieldsJSON["unlockAccount"]) == "True" ? false : true;

                if (SearchedfieldsJSON["disableAccount"] != null)
                    dr.DisableAccount = (bool)(SearchedfieldsJSON["disableAccount"]);

                dr.IsFirstLogin = true;


                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion

                dsPatientPortal.PatientLogin.AddPatientLoginRow(dr);
                BLObject<DSPatientPortal> obj = BLLPatientObj.InsertPatientLogin(dsPatientPortal);
                if (obj.Data != null)
                {
                    string patientID = dsPatientPortal.Tables[dsPatientPortal.PatientLogin.TableName].Rows[0][dsPatientPortal.PatientLogin.PatientIdColumn.ColumnName].ToString();
                    SendEMail(patientID, userName, password);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        PatientLoginID = dsPatientPortal.Tables[dsPatientPortal.PatientLogin.TableName].Rows[0][dsPatientPortal.PatientLogin.PatientLoginIdColumn.ColumnName],
                        password = password,
                        PatientPortalURL = ConfigurationManager.AppSettings["PatientPortalURL"]
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string UpdatePatientAccountManager(string fieldsJSON, Int32 PatLoginId)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                DSPatientPortal dsPatPortal = new DSPatientPortal();
                BLObject<DSPatientPortal> obj = BLLPatientObj.LoadPatientLogin(PatLoginId, "", MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]));
                dsPatPortal = obj.Data;
                foreach (DSPatientPortal.PatientLoginRow dr in dsPatPortal.Tables[dsPatPortal.PatientLogin.TableName].Rows)
                {
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                    dr.UserName = MDVUtility.ToStr(SearchedfieldsJSON["txtUserName"]);
                    dr.Password = MDVUtility.EncryptTo64(MDVUtility.ToStr(SearchedfieldsJSON["txtpassword"]));
                    dr.Email = MDVUtility.ToStr(SearchedfieldsJSON["txtAccountEmail"]);
                    dr.SecurityQuestion = MDVUtility.ToStr(SearchedfieldsJSON["ddlSecurityQuestion"]);
                    dr.SecurityAnswer = MDVUtility.ToStr(SearchedfieldsJSON["txtAnswer"]);
                    dr.UnLockAccount = MDVUtility.ToStr(SearchedfieldsJSON["unlockAccount"]) == "True" ? false : true;
                    dr.DisableAccount = (bool)(SearchedfieldsJSON["disableAccount"]);
                    //dr.PatientLoginId = PatLoginId;
                    //dr.IsActive = true;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation
                if (dsPatPortal.Tables[dsPatPortal.PatientLogin.TableName].Rows.Count > 0)
                {
                    BLObject<DSPatientPortal> objInsert = BLLPatientObj.UpdatePatientLogin(dsPatPortal);
                    if (objInsert.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objInsert.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string FillPatientAccountManager(Int64 PatientId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(PatientId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSPatientPortal dsPatientPortal = null;
                    BLObject<DSPatientPortal> obj = BLLPatientObj.LoadPatientLogin(0, "", PatientId);
                    if (obj.Data != null)
                    {
                        dsPatientPortal = obj.Data;
                        if (dsPatientPortal.Tables[dsPatientPortal.PatientLogin.TableName].Rows.Count > 0)
                        {
                            DataRow dr = dsPatientPortal.Tables[dsPatientPortal.PatientLogin.TableName].Rows[0];
                            var keyValues = new Dictionary<string, string>
                            {
                                { "ddlSecurityQuestion", MDVUtility.ToStr(dr[dsPatientPortal.PatientLogin.SecurityQuestionIdColumn.ColumnName])},
                                { "txtpassword", MDVUtility.DecryptFrom64(MDVUtility.ToStr(dr[dsPatientPortal.PatientLogin.PasswordColumn.ColumnName]))},
                                { "txtAnswer", MDVUtility.ToStr(dr[dsPatientPortal.PatientLogin.SecurityAnswerColumn.ColumnName])},
                                //{ "unlockAccount", MDVUtility.ToStr(dr[dsPatientPortal.PatientLogin.UnLockAccountColumn.ColumnName])},
                                { "chkunlockAccount", MDVUtility.ToStr(dr[dsPatientPortal.PatientLogin.UnLockAccountColumn.ColumnName])},
                                { "disableAccount", MDVUtility.ToStr(dr[dsPatientPortal.PatientLogin.DisableAccountColumn.ColumnName])}
                            };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                PatientPortalFill_JSON = js.Serialize(keyValues),
                                PatientPortalURL = ConfigurationManager.AppSettings["PatientPortalURL"],
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = MDVUtility.ToStr(Common.AppPrivileges.No_Record_Message)
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = MDVCustomException.HumanReadableMessage(obj.Message),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SavePatientRepresentative(string fieldsJSON)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);
                bool isPrivilege = false;
                DSPatientPortal dsPatientPortal = new DSPatientPortal();

                DSPatientPortal.PatientRepresentativeRow dr = dsPatientPortal.PatientRepresentative.NewPatientRepresentativeRow();
                if (SearchedfieldsJSON.ContainsKey("hfPatientRepresentativeId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientRepresentativeId"]))
                    dr.RepresentativeId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientRepresentativeId"]);
                if (SearchedfieldsJSON.ContainsKey("hfPatientId") && !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]))
                    dr.PatientId = MDVUtility.ToInt64(SearchedfieldsJSON["hfPatientId"]);
                if (SearchedfieldsJSON.ContainsKey("ddlRelation") && !string.IsNullOrEmpty(SearchedfieldsJSON["ddlRelation"]))
                    dr.RelationId = MDVUtility.ToInt64(SearchedfieldsJSON["ddlRelation"]);
                if (SearchedfieldsJSON.ContainsKey("chbxIsHealthRecordPrivilege") && !string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chbxIsHealthRecordPrivilege"])))
                    isPrivilege = string.IsNullOrEmpty(MDVUtility.ToStr(SearchedfieldsJSON["chbxIsHealthRecordPrivilege"])) ? false : MDVUtility.ToBool(MDVUtility.ToStr(SearchedfieldsJSON["chbxIsHealthRecordPrivilege"]));

                dr.IsHealthRecordPrivilege = isPrivilege;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;
                dr.RepresentativePassword = MDVUtility.EncryptTo64(RandomString(8));


                #region Database Insertion

                dsPatientPortal.PatientRepresentative.AddPatientRepresentativeRow(dr);
                BLObject<DSPatientPortal> obj = BLLPatientObj.InsertPatientRepresentative(dsPatientPortal);
                if (obj.Data != null)
                {
                    string Errormessage = "";
                    dsPatientPortal = obj.Data;
                    if (dsPatientPortal.PatientRepresentative.Rows.Count > 0)
                        Errormessage = SendEMailToPatientRepresentative(ref dsPatientPortal);

                    if (string.IsNullOrEmpty(Errormessage))
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Save_Message + " but " + Errormessage,
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string SendEMailToPatientRepresentative(ref DSPatientPortal dsPatientPortal)
        {
            string message = "";
            try
            {
                string mailAddress = dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows[0][dsPatientPortal.PatientRepresentative.RepreseentativeEmailColumn.ColumnName].ToString();
                if (mailAddress != "")
                {
                    string password = MDVUtility.DecryptFrom64(dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows[0][dsPatientPortal.PatientRepresentative.RepresentativePasswordColumn.ColumnName].ToString());
                    string userName = dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows[0][dsPatientPortal.PatientRepresentative.RepresentativeUserNameColumn.ColumnName].ToString();
                    string repFirstName = dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows[0][dsPatientPortal.PatientRepresentative.FirstNameColumn.ColumnName].ToString();
                    string repLastName = dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows[0][dsPatientPortal.PatientRepresentative.LastNameColumn.ColumnName].ToString();
                    string patientFName = dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows[0][dsPatientPortal.PatientRepresentative.PatientFirstNameColumn.ColumnName].ToString();
                    string patientLName = dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows[0][dsPatientPortal.PatientRepresentative.PatientLastNameColumn.ColumnName].ToString();
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");
                    mail.From = new MailAddress("no_reply@sovms.com");
                    mail.To.Add(mailAddress);
                    mail.Subject = "Sovereign Health System Patient Portal";
                    LinkedResource inlineLogo;
                    mail.Body = GetFormattedEmailMessage(userName, password, out inlineLogo, patientFName, patientLName, repFirstName, repLastName);
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new NetworkCredential("no_reply@sovms.com", "Naka5116");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                else
                    message = "Couldn't send E-mail, Representative's E-mail Doesn't Exist.";
            }
            catch (Exception ex)
            {
                message = "Some error occurred, Couldn't send E-mail.";
            }

            return message;
        }
        private string GetFormattedEmailMessage(string userName, string password, out LinkedResource inlineLogo, string patientFName, string patientLName, string RepFName, string RepLName)
        {
            StringBuilder str = new StringBuilder();
            const string space = " ";
            const string newLine = "<br/>";
            const string comma = ",";
            string portalLink = ConfigurationManager.AppSettings["PatientPortalURL"];
            string portalLink_ = ConfigurationManager.AppSettings["PatientPortalURL"] + "?" + userName;
            string url = @"<a href=""" + portalLink_ + @""">" + portalLink + @"</a>";
            inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/SHS-nav-logo.png"));
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            string imageTag = string.Format(@"<img width = 180 height = 80 alt='Image of Seal' src='cid:{0}'>", inlineLogo.ContentId);
            Func<string, string> makeAnchorTag = link => @"<a href=""" + link + @""">" + link + @"</a>";
            Func<string, string> boldArial = text => "<b style='font-family: Arial, Helvetica, sans-serif;'>" + text + "</b>";
            str.Append(@"<html><body><div style='font-family: Calibri, Helvetica, sans-serif;'>");
            str.Append("<b>Welcome").Append(space).Append(RepLName).Append(comma).Append(space).Append(RepFName).Append("!</b>");
            str.Append(newLine);
            str.Append(newLine).Append("Welcome to Sovereign Health Medical Group’s Patient Portal! This e-mail was sent by your doctor’s office ").Append(" to notify you of the credentials and guide on how to use the patient portal! you have been added as a representative of ").Append(patientLName).Append(comma).Append(space).Append(patientFName);
            str.Append(newLine);
            str.Append(newLine).Append("Please click on following link to view the patient portal.");
            str.Append(newLine).Append(newLine).Append(url);
            str.Append(newLine).Append(newLine).Append("You can find your credentials below:");
            str.Append(newLine).Append(newLine).Append("User Name:").Append(space).Append(userName);
            str.Append(newLine).Append("Password:").Append(space).Append(password);
            str.Append(newLine);
            str.Append(newLine);
            str.Append(newLine).Append("Sincerely,");
            str.Append(newLine).Append("<b>").Append("Sovereign Health Medical Group Staff").Append("</b>");
            str.Append("</div></body></html>");
            return str.ToString();
        }
        private string LoadPatientRepresentative(long PatRepresentativeId, long PatientId)
        {
            try
            {
                DSPatientPortal dsPatientPortal = null;
                BLObject<DSPatientPortal> obj;
                obj = BLLPatientObj.LoadPatientRepresentative(PatRepresentativeId, PatientId);
                dsPatientPortal = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        PatientRepresentativeCount = dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName].Rows.Count,
                        PatientRepresentativeLoad_JSON = MDVUtility.JSON_DataTable(dsPatientPortal.Tables[dsPatientPortal.PatientRepresentative.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        PatientRepresentativeCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string DeleteRepresentative(Int64 RepresentativeId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(RepresentativeId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLPatientObj.DeletePatRepresentative(MDVUtility.ToStr(RepresentativeId));
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SendEMail(string patientID, string userName, string password)
        {
            //DALPatientLogin objDALPatient = new DALPatientLogin();
            //JavaScriptSerializer serialize = new JavaScriptSerializer();
            //Dictionary<string, string> objData = serialize.Deserialize<Dictionary<string, string>>(data);
            //string userName = objData["UserName"];
            //int securityQuestion = Convert.ToInt32(objData["SecurityQuestion"]);
            //string securityAnswer = objData["SecurityAnswer"];
            //DSPatient ds = objDALPatient.LoadPatientLogin(0, userName);
            //DSPatient.PatientLoginRow dr = ds.Tables[ds.PatientLogin.TableName].Rows[0] as DSPatient.PatientLoginRow;
            //if (
            //    Convert.ToInt32(dr[ds.PatientLogin.SecurityQuestionIdColumn.ColumnName]) == securityQuestion
            //    &&
            //    dr[ds.PatientLogin.SecurityAnswerColumn].ToString() == securityAnswer
            //    )

            DSPatient dsPatientPatient = new DSPatient();
            //Start || 01 July, 2016 || ZeeshanAK || Fix for email not going out
            BLObject<DSPatient> obj = BLLPatientObj.FillPatientAndInsuranceById(MDVUtility.ToInt64(patientID), 0, 0, "Appointment");
            //End   || 01 July, 2016 || ZeeshanAK || Fix for email not going out
            dsPatientPatient = obj.Data;

            string patientFName = "";
            string patientLName = "";
            string mailAddress = "";
            string provFName = "";
            string provLName = "";
            string facilityAddress = "";
            string facilityCity = "";
            string facilityState = "";
            string facilityZip = "";
            string facilityZipExt = "";

            if (dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows.Count > 0)
            {
                patientFName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FirstNameColumn.ColumnName].ToString();
                patientLName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.LastNameColumn.ColumnName].ToString();
                mailAddress = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.EmailAddressColumn.ColumnName].ToString();
                provFName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.ProviderFirstNameColumn.ColumnName].ToString();
                provLName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.ProviderLastNameColumn.ColumnName].ToString();
                facilityAddress = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityAddressColumn.ColumnName].ToString();
                facilityCity = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityCityColumn.ColumnName].ToString();
                facilityState = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityStateColumn.ColumnName].ToString();
                facilityZip = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityZipColumn.ColumnName].ToString();
                facilityZipExt = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityZipExtColumn.ColumnName].ToString();
            }
            if (mailAddress != "")
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");

                mail.From = new MailAddress("no_reply@sovms.com");
                mail.To.Add(mailAddress); //dr.EmailAddress "  ;
                mail.Subject = "Sovereign Health System Patient Portal";

                LinkedResource inlineLogo;
                mail.Body = GetFormattedEmailMessage(userName, password, out inlineLogo, provFName, provLName, facilityAddress, facilityCity, facilityState, facilityZip, facilityZipExt, patientFName, patientLName);
                //string Body = GetFormattedEmailMessage(userName, password, out inlineLogo, provFName, provLName, facilityAddress, facilityCity, facilityState, facilityZip, facilityZipExt);
                //var view = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
                //view.LinkedResources.Add(inlineLogo);
                //mail.AlternateViews.Add(view);

                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                //no_reply@sovms.com
                SmtpServer.Credentials = new NetworkCredential("no_reply@sovms.com", "Naka5116");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }

            return "";
        }

        private string GetFormattedEmailMessage(string userName, string password, out LinkedResource inlineLogo, string provFName, string provLName, string facAddress, string facCity, string facState, string facZip, string facZipExt, string firstName, string lastname)
        {
            string ppText = "Through the Patient Portal, you will be able to ask questions, request prescription refills and referrals, update personal information, review published lab results, statements and your Personal Health Record [PHR] … all from the comfort of your home, whenever it’s convenient for you! By using the Patient Portal you no longer have to call the office, leave a message, and wait for a response to get the results of your lab work; those results will be available to you on the Portal. You no longer have to call with a question or concern; you can send a message to the office through the Portal and expect a prompt reply. Begin today to take an active role in managing your health care!";
            //    StringBuilder stringBuilder = new StringBuilder();
            //    stringBuilder.Append("<div> UserName: ").Append(userName).Append("</div>");
            //    stringBuilder.Append("<br/>");
            //    stringBuilder.Append("<div> Password").Append(password).Append("</div>");
            // --------------------------------------
            //   string userName = "steve.john02061980";
            string doctorFullName = "Dr. " + provFName + " " + provLName;
            //string PartialAddress1 = "85 Harristown Road";
            string PartialAddress1 = facAddress;
            //string PartialAddress2 = "Glen Rock, NJ 07452-3307";
            string PartialAddress2 = facCity + ", " + facState + " " + facZip + "-" + facZipExt;
            string SupportEmail = "support@mdvision.net";
            string SupportWebsite = "www.sovereignhealthsystem.com";
            // --------------------------------------

            StringBuilder str = new StringBuilder();
            const string space = " ";
            const string newLine = "<br/>";
            const string comma = ",";
            //const string portalLink = @"http://demoportal.sovereignhealthsystem.com/";
            string portalLink = ConfigurationManager.AppSettings["PatientPortalURL"];
            string portalLink_ = ConfigurationManager.AppSettings["PatientPortalURL"] + "?" + userName;



            string url = @"<a href=""" + portalLink_ + @""">" + portalLink + @"</a>";

            inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/SHS-nav-logo.png"));
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            string imageTag = string.Format(@"<img width = 180 height = 80 alt='Image of Seal' src='cid:{0}'>", inlineLogo.ContentId);

            Func<string, string> makeAnchorTag = link => @"<a href=""" + link + @""">" + link + @"</a>";
            Func<string, string> boldArial = text => "<b style='font-family: Arial, Helvetica, sans-serif;'>" + text + "</b>";

            str.Append(@"<html><body><div style='font-family: Calibri, Helvetica, sans-serif;'>");
            str.Append("<b>Welcome").Append(space).Append(lastname).Append(comma).Append(space).Append(firstName).Append("!</b>");
            str.Append(newLine);
            //str.Append(newLine).Append("We are pleased to confirm your registration with Sovereign Health System Patient Portal. Please click on following link to view the patient portal.");
            str.Append(newLine).Append("Welcome to Sovereign Health Medical Group’s Patient Portal! This e-mail was sent by your doctor’s office ").Append(doctorFullName).Append(" to notify you of the credentials and guide on how to use the patient portal! You have received this e-mail because you signed up to be web enabled at your recent doctor’s appointment.");
            str.Append(newLine);
            str.Append(newLine).Append("Please click on following link to view the patient portal.");
            str.Append(newLine).Append(newLine).Append(url);
            str.Append(newLine).Append(newLine).Append("You can find your credentials below:");
            //str.Append(newLine);
            str.Append(newLine).Append(newLine).Append("User Name:").Append(space).Append(userName);
            str.Append(newLine).Append("Password:").Append(space).Append(password);
            str.Append(newLine);
            str.Append(newLine).Append(ppText);

            str.Append(newLine);
            str.Append(newLine).Append("Sincerely,");
            str.Append(newLine).Append("<b>").Append("Sovereign Health Medical Group Staff").Append("</b>");

            //str.Append(newLine).Append(newLine).Append(newLine).Append("<b>").Append("Best Regards,").Append("</b>");
            //str.Append(newLine).Append(doctorFullName);
            //str.Append(newLine);
            //str.Append(newLine).Append(imageTag);
            //str.Append(newLine);
            //str.Append(newLine).Append(PartialAddress1);
            //str.Append(newLine).Append(PartialAddress2);
            ////str.Append(newLine).Append(boldArial("Email:")).Append(space).Append(makeAnchorTag(SupportEmail));
            //str.Append(newLine).Append(makeAnchorTag(SupportWebsite));
            str.Append("</div></body></html>");

            return str.ToString();
            //  return stringBuilder.ToString();
        }


        private string loadHeaderTemplateData(Int64 patientId, Int64 providerId)
        {
            try
            {

                MDVision.IEHR.EMR.Model.ReportHeader.ReportHeader_TagsSelectModel model = MDVision.IEHR.EMR.Helpers.Clinical.ReportHeader.ReportHeaderHelper.Instance().getReportHeaderTagsHTML(patientId, providerId, 0, "Patient Portal");
                if (model != null)
                {
                    var response = new
                    {
                        status = true,
                        ReportHeaderInfo = HttpUtility.HtmlDecode(model.Header),
                        ReportFooterInfo = HttpUtility.HtmlDecode(model.Footer)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = Common.AppPrivileges.No_Record_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        private string ResetPatientPassword(Int64 PatientId, string PatientEmail)
        {
            try
            {

                DSPatientPortal dsPatPortal = new DSPatientPortal();
                BLObject<DSPatientPortal> obj = BLLPatientObj.LoadPatientLogin(0, "", PatientId);
                dsPatPortal = obj.Data;
                DataRow dr_ = dsPatPortal.Tables[dsPatPortal.PatientLogin.TableName].Rows[0];
                string password = MDVUtility.ToStr(dr_[dsPatPortal.PatientLogin.PasswordColumn.ColumnName]);
                password = MDVUtility.DecryptFrom64(password);
                foreach (DSPatientPortal.PatientLoginRow dr in dsPatPortal.Tables[dsPatPortal.PatientLogin.TableName].Rows)
                {
                    //dr.Password = MDVUtility.ToStr("c292ZXJlaWduaGVhbHRoIzE=");
                    dr.Email = MDVUtility.ToStr(PatientEmail);
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation
                if (dsPatPortal.Tables[dsPatPortal.PatientLogin.TableName].Rows.Count > 0)
                {
                    BLObject<DSPatientPortal> objInsert = BLLPatientObj.UpdatePatientLogin(dsPatPortal);
                    if (objInsert.Data != null)
                    {
                        SendPassWordResetEmail(MDVUtility.ToStr(PatientId), password);
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objInsert.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        private string SendPassWordResetEmail(string patientID, string password)
        {

            DSPatient dsPatientPatient = new DSPatient();
            BLObject<DSPatient> obj = BLLPatientObj.FillPatientAndInsuranceById(MDVUtility.ToInt64(patientID), 0, 0, "Appointment");
            dsPatientPatient = obj.Data;

            string patientFName = "";
            string patientLName = "";
            string mailAddress = "";
            string provFName = "";
            string provLName = "";
            string facilityAddress = "";
            string facilityCity = "";
            string facilityState = "";
            string facilityZip = "";
            string facilityZipExt = "";

            if (dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows.Count > 0)
            {
                patientFName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FirstNameColumn.ColumnName].ToString();
                patientLName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.LastNameColumn.ColumnName].ToString();
                mailAddress = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.EmailAddressColumn.ColumnName].ToString();
                provFName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.ProviderFirstNameColumn.ColumnName].ToString();
                provLName = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.ProviderLastNameColumn.ColumnName].ToString();
                facilityAddress = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityAddressColumn.ColumnName].ToString();
                facilityCity = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityCityColumn.ColumnName].ToString();
                facilityState = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityStateColumn.ColumnName].ToString();
                facilityZip = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityZipColumn.ColumnName].ToString();
                facilityZipExt = dsPatientPatient.Tables[dsPatientPatient.Patients.TableName].Rows[0][dsPatientPatient.Patients.FacilityZipExtColumn.ColumnName].ToString();
            }
            if (mailAddress != "")
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");

                mail.From = new MailAddress("no_reply@sovms.com");
                mail.To.Add(mailAddress);
                mail.Subject = "Sovereign Health System Patient Portal";

                LinkedResource inlineLogo;
                //string Body = GetFormattedEmailMessageResetPassword(patientFName, password, out inlineLogo, provFName, provLName, facilityAddress, facilityCity, facilityState, facilityZip, facilityZipExt);
                //var view = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
                //view.LinkedResources.Add(inlineLogo);
                //mail.AlternateViews.Add(view);

                mail.Body = GetFormattedEmailMessageResetPassword(patientFName, password, out inlineLogo, provFName, provLName, facilityAddress, facilityCity, facilityState, facilityZip, facilityZipExt, patientFName);

                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("no_reply@sovms.com", "Naka5116");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }

            return "";
        }

        private string GetFormattedEmailMessageResetPassword(string userName, string password, out LinkedResource inlineLogo, string provFName, string provLName, string facAddress, string facCity, string facState, string facZip, string facZipExt, string firstName)
        {
            //    StringBuilder stringBuilder = new StringBuilder();
            //    stringBuilder.Append("<div> UserName: ").Append(userName).Append("</div>");
            //    stringBuilder.Append("<br/>");
            //    stringBuilder.Append("<div> Password").Append(password).Append("</div>");
            // --------------------------------------
            //   string userName = "steve.john02061980";
            //string doctorFullName = "Dr. John Hajjar";
            string doctorFullName = "Dr. " + provFName + " " + provLName;
            //string PartialAddress1 = "85 Harristown Road";
            string PartialAddress1 = facAddress;
            //string PartialAddress2 = "Glen Rock, NJ 07452-3307";
            string PartialAddress2 = facCity + ", " + facState + " " + facZip + "-" + facZipExt;
            string SupportEmail = "support@mdvision.net";
            string SupportWebsite = "www.sovereignhealthsystem.com";
            // --------------------------------------

            StringBuilder str = new StringBuilder();
            const string space = " ";
            const string newLine = "<br/>";
            string portalLink = ConfigurationManager.AppSettings["PatientPortalURL"];
            string url = @"<a href=""" + portalLink + @""">" + portalLink + @"</a>";

            inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/SHS-nav-logo.png"));
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            string imageTag = string.Format(@"<img width = 180 height = 80 alt='Image of Seal' src='cid:{0}'>", inlineLogo.ContentId);

            //string imgURL = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/SHS-nav-logo.png");
            //string imageTag ="<img alt='Image of Seal' src="+ imgURL +">";

            Func<string, string> makeAnchorTag = link => @"<a href=""" + link + @""">" + link + @"</a>";
            Func<string, string> boldArial = text => "<b style='font-family: Arial, Helvetica, sans-serif;'>" + text + "</b>";
            //   <font face="verdana"    Calibri

            str.Append(@"<html><body><div style='font-family: Calibri, Helvetica, sans-serif;'>");
            str.Append("<b>Welcome").Append(space).Append(firstName).Append("!</b>");
            str.Append(newLine);
            //str.Append(newLine).Append(newLine).Append(url);
            str.Append(newLine).Append(newLine).Append("Your new Password is below:");
            //str.Append(newLine).Append(newLine).Append("User Name:").Append(space).Append(userName);
            str.Append(newLine);
            str.Append(newLine).Append("Password:").Append(space).Append(password);
            //str.Append(newLine).Append(newLine).Append(newLine).Append("<b>").Append("Best Regards,").Append("</b>");
            //str.Append(newLine).Append(doctorFullName);
            //str.Append(newLine);
            //str.Append(newLine).Append(imageTag);
            //str.Append(newLine);
            //str.Append(newLine).Append(PartialAddress1);
            //str.Append(newLine).Append(PartialAddress2);
            ////str.Append(newLine).Append(boldArial("Email:")).Append(space).Append(makeAnchorTag(SupportEmail));
            //str.Append(newLine).Append(makeAnchorTag(SupportWebsite));
            str.Append(newLine);
            str.Append(newLine).Append("Sincerely,");
            str.Append(newLine).Append("<b>").Append("Sovereign Health Medical Group").Append("</b>");
            str.Append("</div></body></html>");

            return str.ToString();
            //  return stringBuilder.ToString();
        }

        private string CompanyLogo()
        {
            try
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/SHS-nav-logo.png"));
                return Convert.ToBase64String(imageArray);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string UpdatePatientRepresentative(Int64 patRepId, string relationId, bool IshealthRecordAccessPrivilege)
        {
            try
            {

                DSPatientPortal dsPatPortal = new DSPatientPortal();
                BLObject<DSPatientPortal> obj = BLLPatientObj.LoadPatientRepresentative(patRepId, 0);
                dsPatPortal = obj.Data;
                foreach (DSPatientPortal.PatientRepresentativeRow dr in dsPatPortal.Tables[dsPatPortal.PatientRepresentative.TableName].Rows)
                {
                    if (!string.IsNullOrEmpty(MDVUtility.ToStr(relationId)))
                        dr.RelationId = MDVUtility.ToInt32(relationId);
                    else
                        dr[dsPatPortal.PatientRepresentative.RelationIdColumn] = DBNull.Value;
                    dr.IsHealthRecordPrivilege = IshealthRecordAccessPrivilege;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation
                if (dsPatPortal.Tables[dsPatPortal.PatientRepresentative.TableName].Rows.Count > 0)
                {
                    BLObject<DSPatientPortal> objInsert = BLLPatientObj.UpdatePatientRepresentative(dsPatPortal);
                    if (objInsert.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            message = Common.AppPrivileges.Update_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objInsert.Message
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler

        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SAVE_ACCOUNT_MANAGER":
                    {
                        string fieldsJSON = context.Request["AccountManagerData"];
                        string strJSONData = SavePatientAccountManager(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "UPDATE_ACCOUNT_MANAGER":
                    {
                        string fieldsJSON = context.Request["AccountManagerData"];
                        Int32 PatLoginId = MDVUtility.ToInt32(context.Request["PatLoginId"]);

                        string strJSONData = UpdatePatientAccountManager(fieldsJSON, PatLoginId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "FILL_PATIENT_ACCOUNT_MANAGER":
                    {
                        string strPatientId = context.Request["PatientId"];
                        string strJSONData = FillPatientAccountManager(MDVUtility.ToInt64(strPatientId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_REPRESENTATIVE":
                    {
                        string fieldsJSON = context.Request["RepresentativeData"];
                        string strJSONData = SavePatientRepresentative(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_PATIENT_REPRESENTATIVE":
                    {
                        string fieldsJSON = context.Request["AppointmentStatusData"];
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientId"]);
                        string strJSONData = LoadPatientRepresentative(0, PatientId);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_REPRESENTATIVE":
                    {
                        string RepresentativeId = context.Request["RepresentativeId"];
                        string strJSONData = DeleteRepresentative(MDVUtility.ToInt64(RepresentativeId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "RESET_PATIENT_PASSWORD":
                    {
                        string PatientId = context.Request["PatientId"];
                        string PatientEmail = context.Request["PatientEmail"];
                        string strJSONData = ResetPatientPassword(MDVUtility.ToInt64(PatientId), MDVUtility.ToStr(PatientEmail));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "LOAD_TEMPLATEHEADER_DATA":
                    {
                        string PatientId = context.Request["PatientId"];
                        string ProviderId = context.Request["ProviderId"];
                        string strJSONData = loadHeaderTemplateData(MDVUtility.ToInt64(PatientId), MDVUtility.ToInt64(ProviderId));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "UPDATE_PATIENT_REPRESENTATIVE":
                    {
                        string RepresentativeId = context.Request["RepresentativeId"];
                        string RelationShipId = context.Request["RelationShipId"];
                        string HealthPrivilegeBit = context.Request["HealthPrivilegeBit"];
                        string strJSONData = UpdatePatientRepresentative(MDVUtility.ToInt64(RepresentativeId), MDVUtility.ToStr(RelationShipId), MDVUtility.ToBool(HealthPrivilegeBit));

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }
        #endregion
    }
}