using MDVision.Business.BCommon;
using MDVision.DataAccess.DCommon; 
using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class IMO
    {
        #region Variables
        private static IMO instance = null;
        #endregion

        public static IMO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IMO();
                }
                return instance;
            }
        }
        public class NameValuePair
        {
            private string _name;
            private string _value;
            private string _Refvalue;
            private string _Refname;

            public string Name { get { return _name; } set { _name = value; } }
            public string Value { get { return _value; } set { _value = value; } }
            public string RefValue { get { return _Refvalue; } set { _Refvalue = value; } }
            public string RefName { get { return _Refname; } set { _Refname = value; } }
            public NameValuePair()
            {
            }

            public NameValuePair(string name, string value, string Refvalue = "", string Refname = "")
            {
                Name = name;
                Value = value;
                RefValue = Refvalue;
                RefName = Refname;
            }

        }//end class     
        public static String GetIMOCPTCode(string EntityID, String CPTCode, bool isMDVision = true,string Module="")
        {
            String jsonData = "";
            var keyValues = new Dictionary<string, string> { };
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            try
            {
                CPTCode = CPTCode.Trim();
                jsonData = new BLLIMO().SearchCPTCode(CPTCode);

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonData);
                var itemArray = SearchedfieldsJSON["data"]["items"];
                
                foreach (var item in itemArray)
                {
                    if (item["HCPCS_CODE"].ToString() != "" && (item["HCPCS_CODE"].ToString().ToLower()).IndexOf(CPTCode.ToLower()) >= 0)
                    {
                        item["CPT_CODE"] = item["HCPCS_CODE"];
                    }
                 
                    //keyValues.Add(item["CPT_CODE"].ToString(), item["CPT_DESC_SHORT"].ToString());
                    // Faizan Ameen  EMR-1714
                    // Dated 26-Oct-2016

                    if ((!string.IsNullOrEmpty(item["CPT_CODE"].ToString())) || (CPTCode.ToLower().Contains("sct") && CPTCode.ToLower().IndexOf("sct") == 0))
                    {
                        if (CPTCode.ToLower().Contains("sct42819"))
                        {
                            item["SCT_CONCEPT_ID"] = "428191000124101";
                        }
                        string value_  = HttpUtility.HtmlDecode(item["IMO_LEXICAL_CODE"].ToString() + "!" + item["CPT_CODE"].ToString() + "+" + item["title"].ToString() + "@" + item["ICDP_CODE"].ToString() + "+" + item["ICDP_DESC_SHORT"].ToString() + "#" + item["SCT_CONCEPT_ID"].ToString() + "+" + item["SNOMED_DESCRIPTION"].ToString() + "^" + "imo");
                        list.Add(new NameValuePair(value_, item["CPT_CODE"].ToString(), item["CPT_CODE"].ToString()));
                    }
                }

                // EMR-7334
                if (CPTCode.StartsWith("sct"))
                {
                    BLObject<DSProcedures> obj = new BLLClinical().selectCPTLookup(CPTCode.Substring(3));
                    if (obj.Data != null)
                    {
                        foreach (DSProcedures.CPTLookupRow dr in obj.Data.CPTLookup.Rows)
                        {
                            string value_ = HttpUtility.HtmlDecode(dr.LonicCode + "!" + dr.CPTCode + "+" + dr.CPT_Description + "@"  + "+"+ dr.CPT_Description  + "#"+ dr.SNOMEDId + "+" + dr.SNOMED_Description+ "^" + "imo");
                            list.Add(new NameValuePair(value_, dr.CPTCode, dr.CPTCode));
                        }
                       
                    }
                }


                if ((!string.IsNullOrEmpty(Module)) && Module== "EncounterChargeCapture")
                {
                    var distinctList= list.GroupBy(p => p.Value)
.Select(g => g.First())
.ToList();
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(distinctList));
                }
                else
                {
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(list));
                }


            }
            catch (Exception ex)
            {
                //MDVLogger.LogErrorMessage("Business Wrapper::GetIMOCPTCode", "IMO Service", ex);
                //if (isMDVision)
                //    return new AdminCodes().GetCPTCode(EntityID, CPTCode);
                //else
                return "";
            }
        }   
        public static String GetIMOICDCode(string EntityID, string ICDCode, bool isMDVision = true)
        {
            String jsonData = "";
            var keyValues = new Dictionary<string, string> { };
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            try
            {
                jsonData = new BLLIMO().SearchICDCode(ICDCode);

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonData);
                var itemArray = SearchedfieldsJSON["data"]["items"];

                foreach (var item in itemArray)
                {

                    // Faizan Ameen  EMR-1717
                    // Dated 26-Oct-2016

                    if (!string.IsNullOrEmpty(item["ICD10CM_CODE"].ToString()))
                    {

                        string ICDCde = item["ICD10CM_CODE"].ToString();

                        ICDCde = ICDCde.Substring(0, 3);
                    
                        if (ICDCde != "IMO")
                        {
                            list.Add(new NameValuePair(item["IMO_LEXICAL_CODE"].ToString() + "!" + item["kndg_code"].ToString() + "+" + item["title"].ToString() + "@" + item["ICD10CM_CODE"].ToString() + "+" + item["title"].ToString() + "#" + item["SCT_CONCEPT_ID"].ToString() + "+" + item["SNOMED_DESCRIPTION"].ToString() + "^" + "imo", item["ICD10CM_CODE"].ToString(), item["ICD10CM_CODE"].ToString()));
                        }
                    }
                    else
                    {
                        //keyValues.Add(item["CPT_CODE"].ToString(), item["CPT_DESC_SHORT"].ToString()); 
                        list.Add(new NameValuePair(item["IMO_LEXICAL_CODE"].ToString() + "!" + item["kndg_code"].ToString() + "+" + item["title"].ToString() + "@" + item["ICD10CM_CODE"].ToString() + "+" + item["title"].ToString() + "#" + item["SCT_CONCEPT_ID"].ToString() + "+" + item["SNOMED_DESCRIPTION"].ToString() + "^" + "imo", item["ICD10CM_CODE"].ToString(), item["ICD10CM_CODE"].ToString()));
                    }
                }
                return (Newtonsoft.Json.JsonConvert.SerializeObject(list));
                //if (isMDVision)
                //    return new AdminCodes().GetICDCode(EntityID, ICDCode);
                //else
                //    return "";

            }
            catch (Exception ex)
            {
                //MDVLogger.LogErrorMessage("Business Wrapper::GetIMOICDCode", "IMO Service", ex);
                //if (isMDVision)
                //    return new AdminCodes().GetICDCode(EntityID, ICDCode);
                //else
                return "";

            }

        }
        // 'GetIMOICDCode' Will be replaced with below mentioned method at due time, //MK
        public static String GetIMOICDCode_Revamped(string EntityID, string ICDCode, bool isMDVision = true)
        {
            String jsonData = "";
            var keyValues = new Dictionary<string, string> { };
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            try
            {
                jsonData = new BLLIMO().SearchICDCode(ICDCode);

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonData);
                var itemArray = SearchedfieldsJSON["data"]["items"];
                return (Newtonsoft.Json.JsonConvert.SerializeObject(itemArray));
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static String GetAllIMOICDCodes(string entityId, string ICDCode, Int64 pageNumber, Int64 rpp, bool isMdVision = true)
        {
            String jsonData = "";
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            Int32 pageNumber_ = Int32.Parse(pageNumber.ToString());
            Int32 rpp_ = Int32.Parse(rpp.ToString());
            try
            {
                jsonData = new BLLIMO().SearchAllICDCodes(ICDCode, pageNumber, rpp);

                System.Web.Script.Serialization.JavaScriptSerializer ser =
                    new System.Web.Script.Serialization.JavaScriptSerializer { MaxJsonLength = 2147483644 };
                var searchedfieldsJson = ser.Deserialize<dynamic>(jsonData);
                var itemArray = searchedfieldsJson["data"]["items"];
                foreach (var item in itemArray)
                {
                    //keyValues.Add(item["CPT_CODE"].ToString(), item["CPT_DESC_SHORT"].ToString()); 
                    list.Add(new KeyValuePair<string, string>(item["IMO_LEXICAL_CODE"].ToString() + "!" + item["kndg_code"].ToString() + "+" + item["title"].ToString() + "@" + item["ICD10CM_CODE"].ToString() + "+" + item["title"].ToString() + "#" + item["SCT_CONCEPT_ID"].ToString() + "+" + item["SNOMED_DESCRIPTION"].ToString() + "^" + "imo", item["ICD10CM_CODE"].ToString()));

                }

                DataTable dt = new DataTable("tblIMOICD");
                dt.Columns.Add("ICDId", typeof(Int32));
                dt.Columns.Add("LexiCode", typeof(string));
                dt.Columns.Add("ICD9", typeof(string));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("ICD10", typeof(string));
                dt.Columns.Add("ICD10Description", typeof(string));
                dt.Columns.Add("SNOMEDId", typeof(string));
                dt.Columns.Add("SNOMEDDescription", typeof(string));

                for (int i = 0; i < list.Count; i++)
                {
                    string _string = list[i].Key;
                    string lexiCode = list[i].Key.Split('!')[0];
                    string icd9 = list[i].Key.Substring(_string.IndexOf('!') + 1, _string.LastIndexOf('@')).Split('+')[0]; // str.Substring(0, str.LastIndexOf("#") + 1);
                    string icd9Description = list[i].Key.Substring(list[i].Key.IndexOf('!') + 1, (list[i].Key.LastIndexOf('@') - (list[i].Key.IndexOf('!')) - 1)).Split('+')[1];
                    string icd10 = list[i].Key.Substring(list[i].Key.IndexOf('@') + 1, (list[i].Key.LastIndexOf('#') - (list[i].Key.IndexOf('@')) - 1)).Split('+')[0];
                    string icd10Description = list[i].Key.Substring(list[i].Key.IndexOf('@') + 1, (list[i].Key.LastIndexOf('#') - (list[i].Key.IndexOf('@')) - 1)).Split('+')[1];
                    string snomed = list[i].Key.Substring(list[i].Key.IndexOf('#') + 1, (list[i].Key.LastIndexOf('^') - (list[i].Key.IndexOf('#')) - 1)).Split('+')[0];
                    string snomedDescription = list[i].Key.Substring(list[i].Key.IndexOf('#') + 1, (list[i].Key.LastIndexOf('^') - (list[i].Key.IndexOf('#')) - 1)).Split('+')[1];
                    dt.Rows.Add(i, lexiCode, icd9, icd9Description, icd10, icd10Description, snomed, snomedDescription);
                }

                DataTable dtPagae = new DataTable();
                dtPagae = dt.Clone();
                if (dt.Rows.Count > 0)
                    dtPagae = dt.Rows.Cast<DataRow>().Skip((pageNumber_ - 1) * rpp_).Take(rpp_).CopyToDataTable();
                var response = new
                {
                    status = true,
                    ICDCount = list.Count,
                    iTotalDisplayRecords = list.Count,
                    ICDLoad_JSON = MDVUtility.JSON_DataTable(dtPagae),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                //MDVLogger.LogErrorMessage("Business Wrapper::GetIMOICDCode", "IMO Service", ex);
                //if (isMDVision)
                //    return new AdminCodes().GetICDCode(EntityID, ICDCode);
                //else
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            }

        }
        public static String GetAllIMOCPTCodes(string entityId, string cptCode, Int64 pageNumber, Int64 rpp, bool isMdVision = true)
        {
            String jsonData = "";
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            Int32 pageNumber_ = Int32.Parse(pageNumber.ToString());
            Int32 rpp_ = Int32.Parse(rpp.ToString());
            try
            {
                jsonData = new BLLIMO().SearchAllCPTCodes(cptCode, pageNumber, rpp);

                System.Web.Script.Serialization.JavaScriptSerializer ser =
                    new System.Web.Script.Serialization.JavaScriptSerializer { MaxJsonLength = 2147483644 };
                var searchedfieldsJson = ser.Deserialize<dynamic>(jsonData);
                var itemArray = searchedfieldsJson["data"]["items"];
                foreach (var item in itemArray)
                {
                    //keyValues.AddSearchAllCPTCodesitem["CPT_CODE"].ToString(), item["CPT_DESC_SHORT"].ToString()); 
                    list.Add(new KeyValuePair<string, string>(item["IMO_LEXICAL_CODE"].ToString() + "!" + item["CPT_CODE"].ToString() + "+" + item["title"].ToString() + "@" + item["ICDP_CODE"].ToString() + "+" + item["ICDP_DESC_SHORT"].ToString() + "#" + item["SCT_CONCEPT_ID"].ToString() + "+" + item["SNOMED_DESCRIPTION"].ToString() + "^" + "imo" + "=" + item["HCPCS_CODE"].ToString(), item["CPT_CODE"].ToString()));

                }

                DataTable dt = new DataTable("tblIMOCPT");
                dt.Columns.Add("CPTId", typeof(Int32));
                dt.Columns.Add("LexiCode", typeof(string));
                dt.Columns.Add("CPTCode", typeof(string));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("ICD", typeof(string));
                dt.Columns.Add("ICDDescription", typeof(string));
                dt.Columns.Add("SNOMEDId", typeof(string));
                dt.Columns.Add("SNOMEDDescription", typeof(string));
                dt.Columns.Add("HCPCS_CODE", typeof(string));

                for (int i = 0; i < list.Count; i++)
                {
                    string _string = HttpUtility.HtmlDecode(list[i].Key);
                    string lexiCode = _string.Split('!')[0];
                    string cpt_Code = _string.Substring(_string.IndexOf('!') + 1, _string.LastIndexOf('@')).Split('+')[0];
                    string cpt_Description = _string.Substring(_string.IndexOf('!') + 1, (_string.LastIndexOf('@') - (_string.IndexOf('!')) - 1)).Split('+')[1];
                    string icd10 = _string.Substring(_string.IndexOf('@') + 1, (_string.LastIndexOf('#') - (_string.IndexOf('@')) - 1)).Split('+')[0];
                    string icd10Description = _string.Substring(_string.IndexOf('@') + 1, (_string.LastIndexOf('#') - (_string.IndexOf('@')) - 1)).Split('+')[1];
                    string snomed = _string.Substring(_string.IndexOf('#') + 1, (_string.LastIndexOf('^') - (_string.IndexOf('#')) - 1)).Split('+')[0];
                    string snomedDescription = _string.Substring(_string.IndexOf('#') + 1, (_string.LastIndexOf('^') - (_string.IndexOf('#')) - 1)).Split('+')[1];

                    string hcPCS = _string.Substring(_string.LastIndexOf('=') + 1);
                    if (string.IsNullOrEmpty(cpt_Code))
                    {
                        cpt_Code = hcPCS;
                    }

                    dt.Rows.Add(i, lexiCode, cpt_Code, cpt_Description, icd10, icd10Description, snomed, snomedDescription);
                }
                //AST - 330
                if (dt.Rows.Count > 0)
                {
                    DataTable dtPagae = dt.Rows.Cast<DataRow>().Skip((pageNumber_ - 1) * rpp_).Take(rpp_).CopyToDataTable();
                    var response = new
                    {
                        status = true,
                        CPTCount = list.Count,
                        iTotalDisplayRecords = list.Count,
                        CPTLoad_JSON = MDVUtility.JSON_DataTable(dtPagae)
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else {
                    var response = new
                    {
                        status = false,
                        CPTCount = 0,
                        iTotalDisplayRecords = 0,
                        Message= "No Record found."
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                }
                
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        #region Service Command Handler

        /// <summary>
        /// Commands the handler.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "GET_IMO_CPTCODE":
                    {
                        string CPTCode = context.Request["text"];
                        string EntityID = context.Request["entityID"];
                        bool isMDVision = true;
                        if (!string.IsNullOrEmpty(context.Request["isMDVision"]))
                            isMDVision = bool.Parse(context.Request["isMDVision"]);
                        string Module = context.Request["module"]; 
                        string strJSONData = GetIMOCPTCode(EntityID, CPTCode, isMDVision, Module);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "GET_IMO_ICDCODE":
                    {
                        string ICDCode = context.Request["text"];
                        string EntityID = context.Request["entityID"];

                        bool isMDVision = true;
                        if (!string.IsNullOrEmpty(context.Request["isMDVision"]))
                            isMDVision = bool.Parse(context.Request["isMDVision"]);

                        string strJSONData = GetIMOICDCode(EntityID, ICDCode, isMDVision);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
            }
        }


        #endregion
    }
}