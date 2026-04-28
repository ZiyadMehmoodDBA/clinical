/// Author: ZeeshanAK
/// Purpose:  Created to handel Break The Glass
/// Date : April 16, 2016


using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.EMR.Model.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using MDVision.IEHR.Controls.Patient.Demographics;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.IEHR.EMR.Helpers.Clinical.Medical;
using System.Threading;
using MDVision.IEHR.Common;
using Newtonsoft.Json.Linq;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Patient
{
    public class PatientBreakTheGlassHelper
    {
        private BLLPatient BLLPatientObj = null;
        public PatientBreakTheGlassHelper()
        {
            BLLPatientObj = new BLLPatient();
        }


        private static PatientBreakTheGlassHelper _instance = null;
        public static PatientBreakTheGlassHelper Instance()
        {
            if (_instance == null)
                _instance = new PatientBreakTheGlassHelper();
            return _instance;
        }

        public string breakTheGlassReasonSave(PatientBreakTheGlassModel model)
        {
            try
            {
                DSPatientBreakGlass dsBTG = new DSPatientBreakGlass();
                DSPatientBreakGlass.PatUsrBrkGlassReasonRow dr = dsBTG.PatUsrBrkGlassReason.NewPatUsrBrkGlassReasonRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                dr.UserId = MDVSession.Current.AppUserId;
                dr.BreakReason = model.BreakGlassReason;
                dr.IsBreakGlassOn = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsBTG.PatUsrBrkGlassReason.AddPatUsrBrkGlassReasonRow(dr);
                BLObject<DSPatientBreakGlass> obj = BLLPatientObj.insertPatientBreakGlassReason(dsBTG);
                dsBTG = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        restreicedUserId = dsBTG.Tables[dsBTG.PatUsrBrkGlassReason.TableName].Rows[0][dsBTG.PatUsrBrkGlassReason.PatUsrBrkGlassReasonIdColumn.ColumnName]
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string restrictUserSave(PatientBreakTheGlassModel model)
        {
            try
            {
                DSPatientBreakGlass dsBTG = new DSPatientBreakGlass();
                DSPatientBreakGlass.PatUserBreakGlassRow dr = dsBTG.PatUserBreakGlass.NewPatUserBreakGlassRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);
                dr.UserId = MDVUtility.ToInt64(model.UserId);
                dr.IsBreakGlassAllow = true;

                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                dsBTG.PatUserBreakGlass.AddPatUserBreakGlassRow(dr);
                BLObject<DSPatientBreakGlass> obj = BLLPatientObj.insertPatientBreakGlass(dsBTG);
                dsBTG = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        restreicedUserId = dsBTG.Tables[dsBTG.PatUserBreakGlass.TableName].Rows[0][dsBTG.PatUserBreakGlass.PatUserBreakGlassIdColumn.ColumnName]
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string restrictUserLoad(PatientBreakTheGlassModel model)
        {
            try
            {

                DSPatientBreakGlass dsBTG = null;
                BLObject<DSPatientBreakGlass> obj;
                // obj = BLLPatientObj.loadPatientBreakGlass(MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.PatientId), null);
                obj = BLLPatientObj.loadPatientBreakGlass(0, MDVUtility.ToInt64(model.PatientId), null);

                dsBTG = obj.Data;
                if (obj.Data != null)
                {
                    int restrictedUserTotalCount = 0;
                    //if (dsBTG.Tables[dsBTG.ProblemList.TableName].Rows.Count == 0)
                    //{
                    //    if (model.IsActive.Equals("1"))
                    //    {
                    //        obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "0", "0");
                    //    }
                    //    else
                    //    {
                    //        obj = BLLClinicalObj.LoadProblemLists(MDVUtility.ToInt64(model.ProblemListId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(model.NoteId), "0", "1");
                    //    }

                    //    if (obj.Data != null)
                    //    {
                    //        DSPatientBreakGlass dsBTGInActive = obj.Data;
                    //        ProblemListTotalCount = dsBTGInActive.Tables[dsBTGInActive.ProblemList.TableName].Rows.Count;
                    //    }
                    //}
                    //else
                    //{
                    restrictedUserTotalCount = dsBTG.Tables[dsBTG.PatUserBreakGlass.TableName].Rows.Count;
                    //}
                    var response = new
                    {
                        status = true,
                        restrictedUserTotalCount = restrictedUserTotalCount,
                        restrictedUserCount = dsBTG.Tables[dsBTG.PatUserBreakGlass.TableName].Rows.Count,
                        restrictedUserLoad_JSON = MDVUtility.JSON_DataTable(dsBTG.Tables[dsBTG.PatUserBreakGlass.TableName]),
                        //iTotalDisplayRecords = (dsBTG.PatUserBreakGlass.Rows.Count > 0) ? dsBTG.PatUserBreakGlass.Rows[0][dsBTG.PatUserBreakGlass.RecordCountColumn.ColumnName] : 0,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        restrictedUserCount = 0,
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string deletePatientBreakGlass(PatientBreakTheGlassModel model)
        {
            try
            {
                BLObject<string> obj = BLLPatientObj.deletePatientBreakGlass(MDVUtility.ToStr(model.PatUserBreakGlassId));
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
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        public string updateUserBreakGlass(PatientBreakTheGlassModel model)
        {
            try
            {
                if (MDVUtility.ToStr(model.IsBreakGlassAllow) == "True")
                {
                    model.IsBreakGlassAllow = false;
                }
                else
                {
                    model.IsBreakGlassAllow = true;
                }
                #region Database Insertion
                // dsBTG.PatUserBreakGlass.AddPatUserBreakGlassRow(dr);
                BLObject<string> obj = BLLPatientObj.updatePatientBreakGlass(MDVUtility.ToInt64(model.UserId), MDVUtility.ToInt64(model.PatientId), model.IsBreakGlassAllow, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName), DateTime.Now);
                //dsBTG = obj.Data;

                if (obj.Data == "")
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
                        message = obj.Message
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
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

    }
}