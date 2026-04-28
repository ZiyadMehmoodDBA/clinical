using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Data;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.CommonControls
{
    public class AdminCodes
    {

        private BLLAdminCodes BLLAdminCodesObj = null;
        public AdminCodes() {
            BLLAdminCodesObj = new BLLAdminCodes();
        
        
        }

        #region Variables
        private static AdminCodes instance = null;
        #endregion

        public static AdminCodes Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminCodes();

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
            public NameValuePair() { }

            public NameValuePair(string name, string value, string Refvalue = "", string Refname = "")
            {
                this.Name = name;
                this.Value = value;
                this.RefValue = Refvalue;
                this.RefName = Refname;
            }

        }//end class
      

       

        #region Codes support function

        public string GetCPTCode(string EntityID, string CPTCode, int IsEqule = 0)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = BLLAdminCodesObj.LookupCPTCode(EntityID, CPTCode, IsEqule);
            DSCodeLookup ds = objCode.Data;


            if (ds.Tables[ds.CPTCode.TableName].Rows.Count > 0)
            {
                list.Add(new NameValuePair("- SELECT -", ""));
                DataRow[] dRows = ds.Tables[ds.CPTCode.TableName].Select("1=1", ds.CPTCode.CPTCodeColumn.ColumnName);

                foreach (DataRow dr in dRows)
                {
                    list.Add(new NameValuePair(dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString() + " - " + dr[ds.CPTCode.DescriptionColumn.ColumnName].ToString() + "^" + "MDVision", dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString(), dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString()));
                }
            }

            return (Newtonsoft.Json.JsonConvert.SerializeObject(list));

        }
        public string ValidateCPTCode(string CPTCode)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = BLLAdminCodesObj.ValidateCPTCode(CPTCode);
            DSCodeLookup ds = objCode.Data;
            if (ds.Tables[ds.CPTCode.TableName].Rows.Count > 0)
            {
                list.Add(new NameValuePair("- SELECT -", ""));
                DataRow[] dRows = ds.Tables[ds.CPTCode.TableName].Select("1=1", ds.CPTCode.CPTCodeColumn.ColumnName);
                foreach (DataRow dr in dRows)
                {
                    list.Add(new NameValuePair(dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString() + " - " + dr[ds.CPTCode.DescriptionColumn.ColumnName].ToString() + "^" + "MDVision", dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString(), dr[ds.CPTCode.CPTCodeColumn.ColumnName].ToString()));
                }
            }
            return (Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        private string GetModifierCode(string ModifierCode, int IsEqule = 0, int iscode = 0)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = BLLAdminCodesObj.LookupModifier(ModifierCode, "1", IsEqule);
            DSCodeLookup ds = objCode.Data;


            if (ds.Tables[ds.Modifier.TableName].Rows.Count > 0)
            {
                list.Add(new NameValuePair("- SELECT -", ""));
                DataRow[] dRows = ds.Tables[ds.Modifier.TableName].Select("1=1", ds.Modifier.ModifierCodeColumn.ColumnName);

                foreach (DataRow dr in dRows)
                {
                    if (iscode == 1)
                        list.Add(new NameValuePair(dr[ds.Modifier.ModifierCodeColumn.ColumnName].ToString() + " - " + dr[ds.Modifier.DescriptionColumn.ColumnName].ToString(), dr[ds.Modifier.ModifierIdColumn.ColumnName].ToString(), dr[ds.Modifier.ModifierCodeColumn.ColumnName].ToString()));
                    else
                        list.Add(new NameValuePair(dr[ds.Modifier.ModifierCodeColumn.ColumnName].ToString() + " - " + dr[ds.Modifier.DescriptionColumn.ColumnName].ToString(), dr[ds.Modifier.ModifierCodeColumn.ColumnName].ToString(), dr[ds.Modifier.ModifierCodeColumn.ColumnName].ToString()));
                }
            }

            return (Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        public string GetICDCode(string EntityID, string ICDCode, int IsEqule = 0)
        {

            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSCodeLookup> objCode = BLLAdminCodesObj.LookupICDCode(EntityID, ICDCode, IsEqule);
            DSCodeLookup ds = objCode.Data;


            if (ds.Tables[ds.ICDCode.TableName].Rows.Count > 0)
            {
                list.Add(new NameValuePair("- SELECT -", ""));
                DataRow[] dRows = ds.Tables[ds.ICDCode.TableName].Select("1=1", ds.ICDCode.ICD9Column.ColumnName);

                foreach (DataRow dr in dRows)
                {
                    var Lexi = dr[ds.ICDCode.LexiCodeColumn].ToString();
                    var icd10 = dr[ds.ICDCode.ICD10Column].ToString();
                    var icd10desc = dr[ds.ICDCode.ICD10DescriptionColumn].ToString();
                    var Snomed = dr[ds.ICDCode.SNOMEDIdColumn].ToString();
                    var Snomeddesc = dr[ds.ICDCode.SNOMEDDescriptionColumn].ToString();

                    list.Add(new NameValuePair(Lexi + "!" + dr[ds.ICDCode.ICD9Column.ColumnName].ToString() + "+" + dr[ds.ICDCode.DescriptionColumn.ColumnName].ToString() + "@" + icd10 + "+" + icd10desc + "#" + Snomed + "+" + Snomeddesc + "^" + "MDVision", dr[ds.ICDCode.ICD9Column.ColumnName].ToString(), dr[ds.ICDCode.ICD9Column.ColumnName].ToString()));
                }
            }

            return (Newtonsoft.Json.JsonConvert.SerializeObject(list));

            //return getJSONofList(list);
        }

        private static string getJSONofList(HashSet<NameValuePair> list)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);

        }
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            switch (cammandAction)
            {
                case "CHECK_CPT_CODE":
                    {
                        string CPTCode = context.Request["cptCode"];
                        string EntityId = context.Request["entityId"];
                        string strJSONList = GetCPTCode(EntityId, CPTCode, 1);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONList);
                    }
                    break;
                case "CHECK_ICD_CODE":
                    {
                        string ICDCode = context.Request["icdCode"];
                        string EntityId = context.Request["entityId"];
                        string strJSONList = GetICDCode(EntityId, ICDCode, 1);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONList);
                    }
                    break;
                case "CHECK_MODIFIER_CODE":
                    {
                        string ModifierCode = context.Request["ModifierCode"];
                        string strJSONList = GetModifierCode(ModifierCode, 1,1);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONList);
                    }
                    break;
                case "GET_MODIFIER_CODE":
                    {
                        string ModifierCode = context.Request["ModifierCode"];
                        int iscode = MDVUtility.ToInt(context.Request["iscode"]);
                        string strJSONList = GetModifierCode(ModifierCode, 0, iscode);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONList);
                    }
                    break;
                case "VALIDATE_CPT_CODE":
                    {
                        string CPTCode = context.Request["cptCode"];
                        string strJSONList = ValidateCPTCode(CPTCode);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONList);
                    }
                    break;
            }
        }


        #endregion
    }
}