using MDVision.Common.Utilities;
using MDVision.IEHR.Common;
using MDVision.IEHR.EMR.Helpers.Clinical.iTrack;
using MDVision.IEHR.EMR.Helpers.Clinical.ReviewofSystems;
using MDVision.Model.iTrack;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.EMR.Services
{
    public class iTrackController : ApiController
    {
        [HttpPost]
        public string iTrackDashBoard(JObject AllData)
        {
            string response = null;
            iTrackHelper activityLogHelper = new iTrackHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            Dashboard model = ser.Deserialize<Dashboard>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "loadmipskpis")
            {
               
                    response = activityLogHelper.LoadMIPSKPIs(model);


            }
            else if (model.commandType.ToLower() == "loadgroupqualitymeasures")
            {

                response = activityLogHelper.LoadGroupQualityMeasures(model);


            }
            else if (model.commandType.ToLower() == "load_pi_datesbyproviderid")
            {
                response = activityLogHelper.GetAdminPrefPI_DatesByProviderid(model);
            }
            else if (model.commandType.ToLower() == "load_pi_datesbygroupid")
            {
                response = activityLogHelper.GetAdminPrefPI_DatesByGroupid(model);
            }
            else if (model.commandType.ToLower() == "auditbleeventsactivitylogcomponents")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ActivityLog", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = "";//activityLogHelper.loadAcitivityLogComponents(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "savemipspreferencesgroup")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ActivityLog", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = "";//activityLogHelper.loadAcitivityLogComponents(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "auditbleeventsactivitylogchanges")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ActivityLog", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = "";// activityLogHelper.loadAcitivityLogChanges(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }

            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
        #region Admin
        public string iTrackAdmin(JObject AllData)
        {
            string response = null;
            iTrackHelper iTrackHelper = new iTrackHelper();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            IndvidualProvider model = ser.Deserialize<IndvidualProvider>(MDVUtility.ToStr(AllData["data"]));
            if (model.commandType.ToLower() == "loadindividualprovider")
            {
                    response = iTrackHelper.SelectIndividualProvider(model);
            }
            else if (model.commandType.ToLower() == "loadpracticlookup")
            {
                    response = iTrackHelper.LoadPracticLookup(model.EntityId);
            }
            else if (model.commandType.ToLower() == "loadgroupcatlookup")
            {
                    response = iTrackHelper.LoadGroupCatLookup();
            }
            else if (model.commandType.ToLower() == "loadineligiblereasonlookup")
            {
                    response = iTrackHelper.LoadIneligibleReasonLookup();
            }
            else if (model.commandType.ToLower() == "loadparticipatingreasonlookup")
            {
                    response = iTrackHelper.LoadParticipatingReasonLookup();
            }
            else if (model.commandType.ToLower() == "loadgroupnamelookup")
            {
                    response = iTrackHelper.LoadGroupNameLookup();
            }
            else if (model.commandType.ToLower() == "savemipspreferencesgroup")
            {
                    response = iTrackHelper.SaveMIPSPreferencesGroup(model);
            }
            else if (model.commandType.ToLower() == "savemipspreferencesindvidual")
            {
                    response = iTrackHelper.SaveMIPSPreferencesIndvidual(model);
            }
            else if (model.commandType.ToLower() == "updatemipspreferencesindvidual")
            {
                    response = iTrackHelper.UpdateMIPSPreferencesIndvidual(model);
            }
            else if (model.commandType.ToLower() == "updateitrackreportingtype")
            {
                response = iTrackHelper.UpdateiTrackReportingType(model);
            }
            else if (model.commandType.ToLower() == "activeinactivemipspreferencesgroup")
            {
               
                    response = iTrackHelper.ActiveInActiveMIPSPreferencesGroup(model);
                

            }
            else if (model.commandType.ToLower() == "updatemipspreferencesgroup")
            {
               
                    response = iTrackHelper.UpdateMIPSPreferencesGroup(model);
               

            }
            else if (model.commandType.ToLower() == "loadmipsprovider")
            {
               
                    response = iTrackHelper.LoadMIPSProvider(model);
              
            }
            else if (model.commandType.ToLower() == "searchmimpsgrouppreferences")
            {
               
                    response = iTrackHelper.SearchMIPSGroupPreferences(model);
                
            }
            else if (model.commandType.ToLower() == "searchmipsproviderpreferences")
            {
               
                    response = iTrackHelper.SearchMIPSProviderPreferences(model);
               
            }
            else if (model.commandType.ToLower() == "selectmipsgrouppreferences")
            {
                
                    response = iTrackHelper.SelectMIPSGroupPreferences(model);
              
            }
            else if (model.commandType.ToLower() == "selectindividualpreferences")
            {
                
                    response = iTrackHelper.LoadIndividualProvider(model);
                
            }
            else if (model.commandType.ToLower() == "auditbleeventsactivitylogcomponents")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ActivityLog", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = "";//activityLogHelper.loadAcitivityLogComponents(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }
            else if (model.commandType.ToLower() == "auditbleeventsactivitylogchanges")
            {
                string privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("ActivityLog", "SEARCH")).ToString();
                if (string.IsNullOrEmpty(privilegasMessage))
                {
                    response = "";// activityLogHelper.loadAcitivityLogChanges(model);
                }
                else
                {
                    var responseObj = new
                    {
                        status = false,
                        Message = privilegasMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(responseObj));
                }

            }

            else
            {
                var ErrorMessage = new
                {
                    status = false,
                    Message = "No Method Found, which IT team has called for the operation, Please contact IT Administrator"
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(ErrorMessage));
            }
            return response;
        }
        #endregion
    }
}