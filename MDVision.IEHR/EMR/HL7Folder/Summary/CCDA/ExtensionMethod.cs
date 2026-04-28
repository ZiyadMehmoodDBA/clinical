using MDVision.IEHR.EMR.Helpers.Clinical.Summary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.HL7Folder.Summary.CCDA
{
    public static class ExtensionMethod
    {
        #region Exposed Methods for DataTable
        public static bool IsNull(this DataTable dt)
        {
            return dt == null;
        }

        public static bool HasRows(this DataTable dt)
        {
            return dt.Rows != null && dt.Rows.Count > 0;
        }

        public static DataRow GetFirstRow(this DataTable dt)
        {
            DataRow row = null;
            if (dt.HasRows())
            {
                row = dt.Rows[0];
            }
            return row;
        }

        public static DataRowCollection GetRows(this DataTable dt)
        {
            DataRowCollection rows = null;
            if (dt.HasRows())
            {
                rows = dt.Rows;
            }
            return rows;
        }

        public static DataRowCollection GetRows(this DataTable dt, string key, object value)
        {
            DataRowCollection rows = null;
            if (dt.HasRows())
            {
                DataRow[] array = dt.Select(string.Format("{0}='{1}'", key, value));
                if (array != null && array.Length > 0)
                {
                    rows = array.CopyToDataTable().Rows;
                }
            }
            return rows;
        }

        public static DataTable RemoveRelations(this DataTable dt)
        {
            if (dt != null && dt.DataSet != null && dt.DataSet.Tables != null)
            {
                dt.DataSet.Tables.Remove(dt);
            }
            return dt;
        }
        #endregion Exposed Methods

        #region Exposed Methods DataSet
        public static bool IsNull(this DataSet ds)
        {
            return ds == null;
        }

        public static bool HasTable(this DataSet ds, string tableName)
        {
            return !ds.IsNull() && ds.Tables.Contains(tableName);
        }

        public static bool HasTable(this DataSet ds, int index = 0)
        {
            return !ds.IsNull() && ds.Tables.Count > index;
        }

        public static DataTable GetTable(this DataSet ds, string tableName)
        {
            DataTable table = null;
            if (ds.HasTable(tableName))
            {
                table = ds.Tables[tableName];
            }
            return table;
        }

        public static DataTable GetTable(this DataSet ds, int index = 0)
        {
            DataTable table = null;
            if (ds.HasTable(index))
            {
                table = ds.Tables[index];
            }
            return table;
        }

        public static bool HasRows(this DataSet ds, string tableName)
        {
            return ds.HasTable(tableName) && ds.GetTable(tableName).HasRows();
        }

        public static bool HasRows(this DataSet ds, int index = 0)
        {
            return ds.HasTable(index) && ds.GetTable(index).HasRows();
        }

        public static DataRow GetFirstRow(this DataSet ds, string tableName)
        {
            DataRow row = null;
            if (ds.HasRows(tableName))
            {
                row = ds.GetTable(tableName).GetFirstRow();
            }
            return row;
        }

        public static DataRow GetFirstRow(this DataSet ds, int index = 0)
        {
            DataRow row = null;
            if (ds.HasRows(index))
            {
                row = ds.GetTable(index).GetFirstRow();
            }
            return row;
        }

        public static DataRowCollection GetRows(this DataSet ds, string tableName)
        {
            DataRowCollection rows = null;
            if (ds.HasRows(tableName))
            {
                rows = ds.GetTable(tableName).Rows;
            }
            return rows;
        }

        public static DataRowCollection GetRows(this DataSet ds, int index = 0)
        {
            DataRowCollection rows = null;
            if (ds.HasRows(index))
            {
                rows = ds.GetTable(index).Rows;
            }
            return rows;
        }

        public static DataRowCollection GetRows(this DataSet ds, string key, object value, int index = 0)
        {
            DataRowCollection rows = null;
            if (ds.HasRows(index))
            {
                DataRow[] array = ds.GetTable(index).Select(string.Format("{0}='{1}'", key, value));
                if (array != null && array.Length > 0)
                {
                    rows = array.CopyToDataTable().Rows;
                }
            }
            return rows;
        }
        #endregion Exposed Methods

        #region Exposed  Objects
        public static Boolean ToFormatedBoolean(this object obj, Boolean defaultValue = false)
        {
            if (obj != null)
            {
                if (obj.ToString().Equals("1"))
                {
                    obj = "True";
                }
                else if (obj.ToString().Equals("0"))
                {
                    obj = "False";
                }
            }
            Boolean.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Byte ToFormatedByte(this object obj, Byte defaultValue = 0)
        {
            Byte.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static SByte ToFormatedSByte(this object obj, SByte defaultValue = 0)
        {
            SByte.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Char ToFormatedChar(this object obj, Char defaultValue = '\0')
        {
            Char.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static DateTime ToFormatedDateTime(this object obj, DateTime defaultValue = new DateTime())
        {
            DateTime.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Single ToFormatedSingle(this object obj, Single defaultValue = 0)
        {
            Single.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Double ToFormatedDouble(this object obj, Double defaultValue = 0)
        {
            Double.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Decimal ToFormatedDecimal(this object obj, Decimal defaultValue = 0)
        {
            Decimal.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static UInt16 ToFormatedUInt16(this object obj, UInt16 defaultValue = 0)
        {
            UInt16.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static UInt32 ToFormatedUInt32(this object obj, UInt32 defaultValue = 0)
        {
            UInt32.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static UInt64 ToFormatedUInt64(this object obj, UInt64 defaultValue = 0)
        {
            UInt64.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static ObjType ToObjType<ObjType>(this object obj)
        {
            return obj != DBNull.Value ? (ObjType)obj : default(ObjType);
        }

        public static object ToObjType(this object obj, Type type)
        {
            switch (type.Name)
            {
                case "Boolean":
                    return ToFormatedBoolean(obj);

                case "Byte":
                    return ToFormatedByte(obj);

                case "SByte":
                    return ToFormatedSByte(obj);

                case "Char":
                    return ToFormatedChar(obj);

                case "Decimal":
                    return ToFormatedDecimal(obj);

                case "Single":
                    return ToFormatedSingle(obj);

                case "Double":
                    return ToFormatedDouble(obj);

                case "UInt16":
                    return ToFormatedUInt16(obj);

                case "UInt32":
                    return ToFormatedUInt32(obj);

                case "UInt64":
                    return ToFormatedUInt64(obj);

                case "Int16":
                    return ToFormatedInt16(obj);

                case "Int32":
                    return ToFormatedInt32(obj);

                case "Int64":
                    return ToFormatedInt64(obj);

                case "String":
                    return ToFormatedString(obj);

                case "Byte[]":
                    return ToFormatedByteArray(obj);

                case "DateTime":
                    return ToFormatedDateTime(obj);

                case "Guid":
                    return ToFormatedGuid(obj);

                default:
                    return obj;
            }
        }

        public static Int16 ToFormatedInt16(this object obj, Int16 defaultValue = 0)
        {
            Int16.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Int32 ToFormatedInt32(this object obj, Int32 defaultValue = 0)
        {
            Int32.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Int64 ToFormatedInt64(this object obj, Int64 defaultValue = 0)
        {
            Int64.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static string ToFormatedString(this object obj)
        {
            return obj != null && obj != DBNull.Value ? Convert.ToString(obj) : "";
        }

        public static string ToFormatedString(this object obj, string format)
        {
            string returnValue = obj.ToFormatedString();
            if (!string.IsNullOrEmpty(returnValue))
            {
                format = string.Format("{{0:{0}}}", format);
                returnValue = string.Format(format, obj);
            }
            return returnValue;
        }

        public static String ToFormatedString(this object obj, string format = "", ImportCCDA.TextFormateType textFormateType = ImportCCDA.TextFormateType.Normal)
        {
            string returnValue = obj != null && obj != DBNull.Value ? Convert.ToString(obj) : string.Empty;
            if (!string.IsNullOrEmpty(returnValue))
            {
                if (!string.IsNullOrEmpty(format))
                {
                    string strFormat = string.Format("{{0:{0}}}", format);
                    returnValue = string.Format(strFormat, obj);
                }

                if (textFormateType != ImportCCDA.TextFormateType.Normal)
                {
                    switch (textFormateType)
                    {
                        case ImportCCDA.TextFormateType.Upper:
                            returnValue = returnValue.ToUpper();
                            break;
                        case ImportCCDA.TextFormateType.Lower:
                            returnValue = returnValue.ToLower();
                            break;
                        case ImportCCDA.TextFormateType.Title:
                            returnValue = returnValue.ToTitle();
                            break;
                    }
                }
            }
            return returnValue;
        }

        public static Guid ToFormatedGuid(this object obj)
        {
            Guid defaultValue = Guid.Empty;
            Guid.TryParse(obj.ToFormatedString(), out defaultValue);
            return defaultValue;
        }

        public static Byte[] ToFormatedByteArray(this object obj, Byte[] defaultValue = null)
        {
            Byte[] returnValue = null;
            try
            {
                returnValue = (Byte[])obj;
            }
            catch
            {
                returnValue = defaultValue;
            }
            return returnValue;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null || obj == DBNull.Value;
        }

        #endregion Exposed Methods

        #region Exposed Methods String
        public static string RemoveExtraSpaces(this string s)
        {
            string str = "";
            if (!string.IsNullOrWhiteSpace(s))
            {
                str = s;
                while (true)
                {
                    if (str.StartsWith(" "))
                    {
                        str = str.TrimStart(new char[] { ' ' });
                    }
                    else if (str.EndsWith(" "))
                    {
                        str = str.TrimEnd(new char[] { ' ' });
                    }
                    else if (str.Contains("  "))
                    {
                        str = str.Replace("  ", " ");
                    }
                    else break;
                }
            }
            return str;
        }

        public static string ReplaceStart(this string s, string replace)
        {
            string str = s;
            if (!string.IsNullOrEmpty(replace))
            {
                while (str.StartsWith(replace, StringComparison.InvariantCultureIgnoreCase))
                {
                    str = str.Remove(0, replace.Length);
                }
            }
            return str;
        }

        public static string ReplaceStart(this string s, string[] replaces)
        {
            string str = s;
            if (replaces != null && replaces.Length > 0)
            {
                while (true)
                {
                    string strAfter = str;
                    foreach (string replace in replaces)
                    {
                        strAfter = strAfter.ReplaceStart(replace);
                    }
                    if (!str.Equals(strAfter))
                    {
                        str = strAfter;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return str;
        }

        public static string ToTitle(this string s)
        {
            string str = "";
            if (!string.IsNullOrWhiteSpace(s))
            {
                bool newWord = true;
                foreach (char c in s)
                {
                    if (newWord) { str = string.Concat(str, Char.ToUpper(c)); newWord = false; }
                    else str = string.Concat(str, Char.ToLower(c));
                    if (c == ' ') newWord = true;
                }
            }
            return str;
        }

        public static string[] SplitFirst(this string s, char ch)
        {
            string[] strs = new string[2];
            strs[0] = s.Substring(0, s.IndexOf(ch));
            strs[1] = s.Substring(s.IndexOf(ch) + 1, (s.Length - s.IndexOf(ch)) - 1);
            return strs;
        }

        public static DateTime? ToFormatedDateTimeByFormat(this string str, string sourceFormat = "")
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFormat))
                {
                    string dateTime = str;
                    string zone = "";
                    if (str.Contains("-"))
                    {
                        dateTime = str.Substring(0, str.IndexOf('-'));
                        zone = str.Substring(str.IndexOf('-'), str.Length - str.IndexOf('-'));
                    }
                    else if (str.Contains("+"))
                    {
                        dateTime = str.Substring(0, str.IndexOf('+'));
                        zone = str.Substring(str.IndexOf('+'), str.Length - str.IndexOf('+'));
                    }
                    else if (str.Contains(" "))
                    {
                        dateTime = str.Substring(0, str.IndexOf(' '));
                        str = dateTime;
                    }
                    dateTime = dateTime.Replace(".", "");
                    if (dateTime.Length >= 4)
                    {
                        sourceFormat = string.Format("{0}yyyy", sourceFormat);
                    }
                    if (dateTime.Length >= 6)
                    {
                        sourceFormat = string.Format("{0}MM", sourceFormat);
                    }
                    if (dateTime.Length >= 8)
                    {
                        sourceFormat = string.Format("{0}dd", sourceFormat);
                    }
                    if (dateTime.Length >= 10)
                    {
                        sourceFormat = string.Format("{0}HH", sourceFormat);
                    }
                    if (dateTime.Length >= 12)
                    {
                        sourceFormat = string.Format("{0}mm", sourceFormat);
                    }
                    if (dateTime.Length >= 14)
                    {
                        sourceFormat = string.Format("{0}ss", sourceFormat);
                    }
                    if (dateTime.Length >= 17)
                    {
                        sourceFormat = string.Format("{0}fff", sourceFormat);
                    }
                    if (!string.IsNullOrEmpty(zone))
                    {
                        sourceFormat = string.Format("{0}zzzz", sourceFormat);
                    }

                }
                return DateTime.ParseExact(str, sourceFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        #endregion Exposed Methods

        #region Extension Methods CCDA
        public static bool Validate(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document, string templateId)
        {
            bool validCcda = false;
            if (string.IsNullOrWhiteSpace(templateId) || !document.HasComponents()) return false;
            var templates = document.templateId;
            if (templates.Any(template => template.root == templateId))
            {
                validCcda = true;
            }
            return validCcda;
        }

        public static PatientRole GetPatientRole(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document)
        {
            PatientRole pr = null;
            if (document != null && !document.recordTarget.IsNullOrEmpty() && document.recordTarget[0].patientRole != null)
            {
                pr = document.recordTarget[0].patientRole;
            }
            return pr;
        }

        public static Patient GetPatient(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document)
        {
            Patient p = null;
            PatientRole pr = document.GetPatientRole();
            if (pr != null && pr.patient != null)
            {
                p = pr.patient;
            }
            return p;
        }

        public static bool HasComponents(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document)
        {
            if (document != null && document.component != null && document.component.Item != null && !(document.component.Item as StructuredBody).component.IsNullOrEmpty())
            {
                return true;
            }
            return false;
        }

        public static List<Component3> GetComponents(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document)
        {
            List<Component3> components = null;
            if (HasComponents(document))
            {
                components = (document.component.Item as StructuredBody).component;
            }
            return components;
        }

        public static bool GetComponentByTemplateId(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document, ref Component3 component, string templateId)
        {
            bool isComponentExist = false;
            if (!String.IsNullOrWhiteSpace(templateId) && document.HasComponents())
            {
                List<Component3> docComponents = document.GetComponents();
                foreach (Component3 comp in docComponents)
                {
                    if (comp != null && comp.section != null && !comp.section.templateId.IsNullOrEmpty())
                    {
                        if (comp.section.templateId.Any(id => id != null && id.root == templateId))
                        {
                            component = comp;
                            isComponentExist = true;
                        }
                    }

                    if (isComponentExist) { break; }
                }
            }
            return isComponentExist;
        }

        public static bool GetComponentByTemplateIDs(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document, ref Component3 component, List<String> templateIDs)
        {
            bool isComponentExist = false;
            if (!templateIDs.IsNullOrEmpty() && document.HasComponents())
            {
                List<Component3> docComponents = document.GetComponents();
                foreach (Component3 comp in docComponents)
                {
                    if (comp != null && comp.section != null && !comp.section.templateId.IsNullOrEmpty())
                    {
                        List<II> sectionTemplateIDs = comp.section.templateId;

                        foreach (II t in sectionTemplateIDs)
                        {
                            if (templateIDs.Any(t1 => t.root == t1))
                            {
                                component = comp;
                                isComponentExist = true;
                            }

                            if (isComponentExist) { break; }
                        }
                    }

                    if (isComponentExist) { break; }
                }
            }

            return isComponentExist;
        }

        public static bool GetEntry(this Component3 comp, ref List<Entry> entryList)
        {
            if (comp != null && comp.section != null && !comp.section.entry.IsNullOrEmpty())
            {
                entryList = comp.section.entry;
                return true;
            }
            return false;
        }

        public static bool GetComponents(this MDVision.IEHR.EMR.HL7Folder.Summary.CCDA.ClinicalDocument document, ref List<Component3> componentList)
        {
            bool isExist = false;
            if (document != null && document.component != null && document.component.Item is StructuredBody &&
                !(document.component.Item as StructuredBody).component.IsNullOrEmpty())
            {
                componentList = (document.component.Item as StructuredBody).component;
                isExist = true;
            }
            return isExist;
        }

        public static Boolean IsNullOrEmpty(this ICollection collection, int lessThenOrEqualTo = 0)
        {
            return collection == null || collection.Count <= lessThenOrEqualTo;
        }

        #endregion
    }
}