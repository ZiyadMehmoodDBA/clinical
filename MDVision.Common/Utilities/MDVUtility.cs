using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MDVision.Common.Shared;
using System.Net;
using System.Drawing;
using MDVision.Common.Logging;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using MDVision.Model.Clinical.Orderset;
using System.Configuration;
using System.Web.Configuration;

namespace MDVision.Common.Utilities
{
    public class MDVUtility
    {

        public static Dictionary<string, string> SpecialCharacters = new Dictionary<string, string>()
        {
            { "%*%", "&"}
        };

        public static char D_ELMT = '*';
        public static char D_S_ELMT = ':';
        public static char D_SGMT = '~';

        #region General Functions

        public static string GetYesNo(bool val)
        {
            return val ? "Yes" : "<span style='color:red'>No</span>";
        }

        public static string GetDisplay(bool val)
        {
            return val ? "" : "none";
        }

        public static string GetVisibility(bool val)
        {
            return val ? "visible" : "hidden";
        }


        public static string GetFormattedBool(bool val, string trueText, string falseText)
        {
            return val ? trueText : falseText;
        }


        public static string LPad(string str, string fillChar, int digits)
        {
            if (str.Length <= digits)
            {
                for (int i = 0; i < digits - str.Length; i++)
                    str = fillChar + str;

                //do nothing
            }

            return str;
        }

        public static string GetUniqueCode()
        {
            DateTime d = DateTime.UtcNow;
            string dd = LPad(d.Day.ToString(), "0", 2);
            string mm = LPad(d.Month.ToString(), "0", 2);
            string yyyy = d.Year.ToString();
            string hh = LPad(d.Hour.ToString(), "0", 2);
            string min = LPad(d.Minute.ToString(), "0", 2);
            string ss = LPad(d.Second.ToString(), "0", 2);

            return yyyy + mm + dd + hh + min + ss;

        }

        public static string GetUniqueCode_Short()
        {
            DateTime d = DateTime.UtcNow;
            string dd = LPad(d.Day.ToString(), "0", 2);
            string mm = LPad(d.Month.ToString(), "0", 2);
            string yyyy = d.Year.ToString();

            return yyyy + mm + dd;
        }

        public static object[] GetArrayFromCommaList(string commaList)
        {
            if (commaList == null || commaList == "")
                return new object[] { };

            string[] paramList = commaList.Split(',');
            object[] arr = new object[paramList.Length];

            int i = 0;
            foreach (string str in paramList)
            {
                arr[i++] = str.Trim();
            }

            return arr;
        }

        public static string GetCommaListFromArray(object[] arr)
        {
            string commaList = "";

            foreach (object obj in arr)
            {
                commaList += obj.ToString() + ",";
            }

            commaList = commaList.TrimEnd(',');
            return commaList;
        }

        public static string TrimText(string text, int totChars)
        {
            if (text == "")
                return text;

            if (text.Length > totChars)
                return text.Substring(0, totChars) + "...";
            else
                return text;
        }

        public static List<List<T>> splitList<T>(List<T> source, long size)
        {
            return source
                     .Select((x, i) => new { Index = i, Value = x })
                     .GroupBy(x => x.Index / size)
                     .Select(x => x.Select(v => v.Value).ToList()).ToList();

        }

        public static DataTable ConvertCommaSepatedValuesToDataTable(string values)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "Id", DataType = Type.GetType("System.Int64") });
            if (!string.IsNullOrEmpty(values))
            {
                string[] strArry = values.Split(',');
                for (long i = 0; i < strArry.Length; i++)
                    if (MDVUtility.IsNumericWithOutFloting(strArry[i]))
                        dt.Rows.Add(strArry[i]);
            }
            else
                dt.Rows.Add(0);
            return dt;
        }

        #endregion

        #region DataType Conversion

        #region Date Conversion

        public static string Get12HoursTime(string hr)
        {
            if (MDVUtility.ToInt(hr) == 0)
                return "12 am";

            if (MDVUtility.ToInt(hr) > 12)

                return MDVUtility.ToInt(hr) - 12 + " pm";

            else if (MDVUtility.ToInt(hr) == 12)
                return 12 + " pm";
            else
                return hr + " am";

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(Object val)
        {
            DateTime dt;
            //  return DateTime.TryParse(val.ToString(), out dt) ? dt : DateTime.MinValue;
            // Minimum  datetime changed to 1/1/1753 By Faizan Ameen.

            return DateTime.TryParse(val.ToString(), out dt) ? dt : (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        }

        public static DateTime ToDateTime(Object val, string alternateVal)
        {
            DateTime dt;
            return DateTime.TryParse(val.ToString(), out dt) ? dt : Convert.ToDateTime(alternateVal);
        }

        public static DateTime ToDateTime(Object val, DateTime alternateVal)
        {
            DateTime dt;
            return DateTime.TryParse(val.ToString(), out dt) ? dt : alternateVal;
        }


        #endregion

        #region Strings

        public static string ToStr(Object val, string defaultValue = "")
        {
            return val == null ? defaultValue : @val.ToString();
        }

        /// <summary>
        /// this function will return the string enclosing it in "'" {single quote}
        /// also replace single quote within the string with {two single quotes} i.e., "''"
        /// such type of formatting is typically used in LINQ
        /// </summary>
        /// <param name="val"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ToLINQFormatString(Object val, string defaultValue = "")
        {
            string returnString = "";

            if (val == null)
            {

                return defaultValue;
            }
            returnString = @val.ToString().Replace("'", "''");

            return ("'" + returnString + "'");
        }



        public static string ToStrNullZero(Object val, string defaultValue = "")
        {
            return val == null || ToStr(val) == "undefined" || ToStr(val) == "0" || ToStr(val) == "null" ? defaultValue : val.ToString();
        }



        #endregion

        #region Numbers

        public static int ToInt(Object val, int alternateVal)
        {
            int i;
            return int.TryParse(MDVUtility.ToStr(val), out i) ? i : alternateVal;
        }

        public static int ToInt(Object val)
        {
            int i;
            return int.TryParse(MDVUtility.ToStr(val), out i) ? i : 0;
        }

        public static Int16 ToInt16(Object val, Int16 alternateVal)
        {
            Int16 i;
            return Int16.TryParse(MDVUtility.ToStr(val), out i) ? i : alternateVal;
        }

        public static Int16 ToInt16(Object val)
        {
            Int16 i;
            return Int16.TryParse(MDVUtility.ToStr(val), out i) ? i : Convert.ToInt16(0);
        }

        public static Int32 ToInt32(Object val, Int32 alternateVal)
        {
            Int32 i;
            return Int32.TryParse(MDVUtility.ToStr(val), out i) ? i : alternateVal;
        }
        public static Int32 ToInt32(Object val)
        {
            Int32 i;
            return Int32.TryParse(MDVUtility.ToStr(val), out i) ? i : 0;
        }
        public static Int32 ToConvertInt32(Object val)
        {
            Int32 i;
            return Int32.TryParse(MDVUtility.ToStr(val), out i) ? i : 0;
        }


        public static Int64 ToInt64(Object val, Int64 alternateVal)
        {
            Int64 i;
            return Int64.TryParse(MDVUtility.ToStr(val), out i) ? i : alternateVal;
        }

        public static Int64 ToInt64(Object val)
        {
            Int64 i;
            return Int64.TryParse(MDVUtility.ToStr(val), out i) ? i : Convert.ToInt64(0);
        }

        public static Double ToDouble(Object val)
        {
            Double dbl;
            return Double.TryParse(MDVUtility.ToStr(val), out dbl) ? dbl : 0.0;
        }
        public static long ToLong(Object val)
        {
            long dbl;
            return long.TryParse(MDVUtility.ToStr(val), out dbl) ? dbl : 0;
        }
        public static Double ToDouble(Object val, Double alternateVal)
        {
            Double dbl;
            return Double.TryParse(MDVUtility.ToStr(val), out dbl) ? dbl : -1;
        }
        public static float Tofloat(Object val)
        {
            float fl;
            return float.TryParse(MDVUtility.ToStr(val), out fl) ? fl : (float)0.0;
        }

        public static Decimal ToDecimal(Object val)
        {
            Decimal dec;
            return Decimal.TryParse(MDVUtility.ToStr(val), out dec) ? dec : (Decimal)0.0;
        }
        public static Boolean IsNumeric(Object val)
        {
            Double i = 0;
            if (Double.TryParse(MDVUtility.ToStr(val), out i))
            {
                return true;
            }
            return false;


        }
        public static Boolean IsNumericWithOutFloting(Object val)
        {
            long i = 0;
            if (long.TryParse(MDVUtility.ToStr(val), out i))
                return true;
            return false;
        }

        #endregion

        #region Boolean

        public static Boolean ToBool(Object val)
        {
            bool b;
            return bool.TryParse(MDVUtility.ToStr(val), out b) ? b : false;
        }
        public static bool StringToBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
                
            switch (value.ToLower())
            {
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                default:
                    return false;
            }
        }

        //public static Boolean ToBool(Object val, bool alternateVal)
        //{
        //    bool b;
        //    return bool.TryParse(MDVUtility.ToStr(val), out b) ? b : alternateVal;
        //}


        #endregion

        #region Byte

        public static byte ToByte(Object val, byte alternateVal)
        {
            byte i;
            return byte.TryParse(MDVUtility.ToStr(val), out i) ? i : alternateVal;
        }

        public static byte ToByte(Object val)
        {
            byte i;
            return byte.TryParse(MDVUtility.ToStr(val), out i) ? i : (byte)(0);
        }

        #endregion


        #region Convert List To DataTable

        public static DataTable CreateTable<T>()
        {
            var entityType = typeof(T);
            var table = new DataTable(entityType.Name);
            var properties = TypeDescriptor.GetProperties(entityType);
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, prop.PropertyType);
            return table;
        }
        public static DataTable ConvertListToDataTable<T>(IList<T> list)
        {
            var table = CreateTable<T>();
            var entityType = typeof(T);
            var properties = TypeDescriptor.GetProperties(entityType);
            foreach (var item in list)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item);
                table.Rows.Add(row);
            }
            return table;
        }

        #endregion

        #region DataTable to List<T>
        /// <summary>
        /// Function to Convert Data Table to List
        /// It requires datatable as perameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion

        #endregion

        #region Null Values


        public static object IsNullInt(Object val)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return DBNull.Value;
            else
                return Convert.ToInt32(val);
        }


        public static Object IsNull(Object val, Object defaultVal)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return defaultVal;
            else
                return val;
        }

        public static object IsNullStr(Object val)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return DBNull.Value;
            else
                return val.ToString();
        }

        public static String IsNullStr(Object val, string defaultVal)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return defaultVal;
            else
                return val.ToString();
        }

        public static object IsNullBool(Object val, Boolean defaultVal)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return defaultVal;
            else
                return Convert.ToBoolean(val);
        }

        public static object IsNullBool(Object val)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return DBNull.Value;
            else
                return Convert.ToBoolean(val);
        }

        public static object IsNullDate(Object val, DateTime defaultVal)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return defaultVal;
            else
                return Convert.ToDateTime(val);
        }


        public static object IsNullDate(Object val)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return DBNull.Value;
            else
                return Convert.ToDateTime(val);
        }


        public static object IsNullDec(Object val)
        {
            if (val == null || val.ToString() == DBNull.Value.ToString())
                return DBNull.Value;
            else
                return Convert.ToDecimal(val);
        }

        #endregion

        #region Dates

        public static string Get24HourTime(int hour, int minute, string ToD)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;

            if (ToD.ToUpper() == "PM")
                hour = (hour % 12) + 12;

            if (hour == 24)
            {
                hour = 23;
                minute = 59;
            }

            return new DateTime(year, month, day, hour, minute, 0).ToString("HHmm");
        }

        public static DateTime GetDateFromNumber(string number)
        {
            //i-e: number = 20101231
            number = number.Replace(" ", "");
            string d = number;

            if (number.Length >= 6)
                d = number.Substring(4, 2).Trim() + "/" + number.Substring(6, 2).Trim() + "/" + number.Substring(0, 4).Trim();

            return Convert.ToDateTime(d);
        }

        public static string GetDateDDMMYYY(string dt)
        {

            string part1;
            string part2;
            string part3;

            if (dt == "") return "";

            dt = dt.Contains(" ") ? dt.Substring(0, dt.IndexOf(" ")) : dt;
            dt = dt.Trim();

            part1 = LPad(dt.Substring(0, dt.IndexOf("/")), "0", 2);
            part2 = LPad(dt.Substring(dt.IndexOf("/") + 1, 2).Replace("/", ""), "0", 2);
            part3 = LPad(dt.Substring(dt.LastIndexOf("/") + 1), "0", 4);


            if (Convert.ToInt32(part2) > 12) //it's a mm/dd/yyyy
                return part2 + "/" + part1 + "/" + part3;
            else
                return part1 + "/" + part2 + "/" + part3;

        }

        public static string GetDateMMDDYYY(string dt)
        {
            if (dt == "")
                return "";

            string part1;
            string part2;
            string part3;

            dt = dt.Contains(" ") ? dt.Substring(0, dt.IndexOf(" ")) : dt;
            dt = dt.Trim();

            part1 = LPad(dt.Substring(0, dt.IndexOf("/")), "0", 2);
            part2 = LPad(dt.Substring(dt.IndexOf("/") + 1, 2).Replace("/", ""), "0", 2);
            part3 = LPad(dt.Substring(dt.LastIndexOf("/") + 1), "0", 4);


            if (Convert.ToInt32(part1) > 12) //it's a dd/mm/yyyy
                return part2 + "/" + part1 + "/" + part3;
            else
                return part1 + "/" + part2 + "/" + part3;

        }

        public static string GetTimeFromNumbers(string t)
        {
            t = t.Trim();
            if (t.Length == 5)
            {
                t = t.Substring(0, 1) + ":" + t.Substring(1, 2) + ":" + t.Substring(3, 2);
            }
            else if (t.Length == 6)
            {
                //t = (Convert.ToInt32(t.Substring(0, 2)) > 23 ? "00" : t.Substring(0, 2)) + ":" + t.Substring(2, 2) + ":" + t.Substring(4, 2);
                t = t.Substring(0, 2) + ":" + t.Substring(2, 2) + ":" + t.Substring(4, 2);
            }
            return t;

        }

        public static bool IsDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }

        public static string DateToString(DateTime d)
        {


            try
            {
                string datetemp = null;
                string result = null;

                datetemp = d.Year.ToString();
                result = result + datetemp;
                datetemp = d.Month.ToString();
                if (Convert.ToInt32(datetemp) < 10)
                {
                    datetemp = "0" + datetemp;
                }
                result = result + datetemp;
                datetemp = d.Day.ToString();
                if (Convert.ToInt32(datetemp) < 10)
                {
                    datetemp = "0" + datetemp;
                }
                result = result + datetemp;
                datetemp = d.Hour.ToString();
                if (Convert.ToInt32(datetemp) < 10)
                {
                    datetemp = "0" + datetemp;
                }
                result = result + datetemp;

                datetemp = d.Minute.ToString();
                if (Convert.ToInt32(datetemp) < 10)
                {
                    datetemp = "0" + datetemp;
                }
                result = result + datetemp;

                datetemp = d.Second.ToString();
                if (Convert.ToInt32(datetemp) < 10)
                {
                    datetemp = "0" + datetemp;
                }
                result = result + datetemp;

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static DateTime StringToDate(string StrDate)
        {
            try
            {


                if (StrDate.Length == 8)
                {
                    StrDate = StrDate.PadRight(14, '0');
                }

                if (StrDate.Length == 14)
                {
                    if (IsNumeric(StrDate) == true)
                    {
                        int YYYY = 0;
                        int mm = 0;
                        int DD = 0;
                        int HH = 0;
                        int MN = 0;
                        int SS = 0;
                        YYYY = Convert.ToInt32(StrDate.Substring(0, 4));
                        mm = Convert.ToInt32(StrDate.Substring(4, 2));
                        DD = Convert.ToInt32(StrDate.Substring(6, 2));
                        HH = Convert.ToInt32(StrDate.Substring(8, 2));
                        MN = Convert.ToInt32(StrDate.Substring(10, 2));
                        SS = Convert.ToInt32(StrDate.Substring(12, 2));

                        DateTime dt = new DateTime(YYYY, mm, DD, HH, MN, SS);
                        return dt;
                    }
                    else

                        throw new Exception("String format is not numeric.");
                }



            }
            catch (Exception ex)
            {
                //MDVLogger.LogErrorMessage("Utility::StringToDate", ex);
            }
            return System.DateTime.Now;
        }


        public static DateTime StringToTime(string Str24Time)
        {
            try
            {

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int day = DateTime.Now.Day;

                int hour = 0;
                int minute = 0;

                if (Str24Time.Length == 4)
                {
                    hour = Convert.ToInt32(Str24Time.Substring(0, 2));
                    minute = Convert.ToInt32(Str24Time.Substring(2, 2));
                }



                return new DateTime(year, month, day, hour, minute, 0);
            }
            catch (Exception ex)
            {
                // MDVLogger.LogErrorMessage("Utility::StringToTime", ex);
            }
            return System.DateTime.Now;
        }

        public static bool IsDateTime(string StrDate)
        {
            try
            {
                Convert.ToDateTime(StrDate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Encrypt and Decrypt data


        public static string EncryptToSHA256(string data, string salt)
        {

            //to bypass encryption
            // return data;
            if (salt != null)
            {
                salt = salt.ToLower();
                data += salt;
            }

            SHA256 hasher = SHA256.Create();


            byte[] hashedData = hasher.ComputeHash(Encoding.Unicode.GetBytes(data));


            // convert to a hexadecimal string for saving

            StringBuilder sb = new StringBuilder(hashedData.Length * 2);

            foreach (byte b in hashedData)
            {

                sb.AppendFormat("{0:x2}", b);

            }

            return sb.ToString();

        }

        public static string EncryptTo64(string toEncode)
        {

            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }



        /// <summary>
        ///  DecryptFrom64
        /// </summary>
        /// <param name="encodedData"></param>
        /// <returns></returns>
        public static string DecryptFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;


        }

        //protected string DecodePassword(string userName, string password)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        return string.Empty;
        //    char firstChar = userName[0];
        //    int i = 96310 + Convert.ToInt32(firstChar);
        //    return (password + i.ToString());
        //}
        public static DataSet DecryptDataSet(string EncrptedXmlDataSet, ref DataSet ds)
        {
            try
            {

                if (EncrptedXmlDataSet != null)
                {
                    string xmlString = DecryptFrom64(EncrptedXmlDataSet);
                    System.Xml.XmlTextReader xmlReader = new System.Xml.XmlTextReader(new System.IO.StringReader(xmlString));
                    ds.ReadXml(xmlReader, XmlReadMode.DiffGram);

                }

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static string EncryptDataSet(ref DataSet ds)
        {
            try
            {
                System.Text.StringBuilder xmlString = new System.Text.StringBuilder();
                System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(new System.IO.StringWriter(xmlString));
                ds.WriteXml(xmlWriter, XmlWriteMode.DiffGram);

                return EncryptTo64(xmlString.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetXmlOfObject(Type type, object obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(type);
            XmlDocument doc = new XmlDocument();

            System.IO.StringWriter sww = new System.IO.StringWriter();
            XmlWriterSettings xmlsettings = new XmlWriterSettings();
            xmlsettings.Encoding = System.Text.Encoding.UTF8;

            XmlWriter writer = XmlWriter.Create(sww, xmlsettings);
            xsSubmit.Serialize(writer, obj);
            return stripNS(XElement.Parse(sww.ToString())).ToString();
        }
        public static XElement stripNS(XElement root)
        {
            return new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(el => stripNS(el)) :
                    (object)root.Value
            );
        }

        #endregion

        #region JSON support function
        public static string GetQueryValue(string queryString, string ParameterName)
        {
            return HttpUtility.ParseQueryString(queryString).Get(ParameterName);

        }

        public static string JSON_DataTable(DataTable table, string fieldwithformat = "", bool bEncodeHTML = true)
        {
            System.Collections.Specialized.NameValueCollection fieldsformatcollection = new System.Collections.Specialized.NameValueCollection();

            if (fieldwithformat != "")
            {
                string[] strfieldswithformat = fieldwithformat.Split(',');

                foreach (string strfieldwithformat in strfieldswithformat)
                {
                    string[] strformat = strfieldwithformat.Split('|');
                    fieldsformatcollection.Add(strformat[0], strformat[1]);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in table.Rows)
            {
                if (sb.Length != 0)
                    sb.Append(",");
                sb.Append("{");
                StringBuilder sb2 = new StringBuilder();
                foreach (DataColumn col in table.Columns)
                {
                    string fieldname = col.ColumnName;

                    string fieldvalue = dr[fieldname].ToString();

                    if (sb2.Length != 0)
                        sb2.Append(",");
                    if (bEncodeHTML)
                        fieldvalue = HttpUtility.HtmlEncode(fieldvalue);
                    fieldvalue = fieldvalue.Replace(System.Environment.NewLine, "");
                    fieldvalue = fieldvalue.Replace("\r", "");
                    fieldvalue = fieldvalue.Replace("\n", "");
                    fieldvalue = fieldvalue.Replace("\t", "");
                    fieldvalue = fieldvalue.Replace("\\", "\\\\");

                    if (fieldwithformat != "")
                    {
                        if (fieldsformatcollection[fieldname] != null)
                        {
                            if (col.DataType == typeof(DateTime))//|| col.DataType == typeof(String)
                            {
                                fieldvalue = MDVUtility.ToDateTime(fieldvalue).ToString(fieldsformatcollection[fieldname]);
                            }
                            //if (fieldsformatcollection[fieldname].ToUpper() == ("PatientStatusToWebColor").ToUpper())
                            //{
                            //    //fieldvalue = MDVUtility.PatientStatusToWebColor(fieldvalue);
                            //}
                            //if (fieldsformatcollection[fieldname].ToUpper() == ("StringToColor").ToUpper())
                            //{
                            //    //fieldvalue = MDVUtility.StringToColor(fieldvalue);
                            //}
                        }
                    }

                    sb2.Append(string.Format("\"{0}\":\"{1}\"", fieldname, fieldvalue));

                }
                sb.Append(sb2.ToString());
                sb.Append("}");
            }

            sb.Insert(0, "[");
            sb.Append("]");
            fieldsformatcollection.Clear();
            return sb.ToString();


        }
        #endregion

        #region Image file function
        public static string GetImagePath(string ImagePath, string EntityId, string FileName, string FileExtension)
        {
            string strPath;
            if (EntityId == "")
                strPath = ImagePath;
            else

                strPath = ImagePath + "/" + EntityId;

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(strPath)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strPath));
            }

            strPath += "/" + FileName + "." + FileExtension;

            return strPath;

        }

        public static string GetImagePath(string ImagePath, string FileName, string FileExtension)
        {
            string strPath = ImagePath;

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(strPath)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strPath));
            }

            strPath += "/" + FileName + "." + FileExtension;

            return HttpContext.Current.Server.MapPath(strPath);

        }
        public static string GetDLLPath(string DLLPath, string FileName, string FileExtension)
        {
            string strPath = DLLPath;

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(strPath)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strPath));
            }

            strPath += "/" + FileName + "." + FileExtension;

            return HttpContext.Current.Server.MapPath(strPath);

        }
        public static string CreateImageFile(byte[] PatientImage, string ImagePath, string EntityId, string FileName, string FileExtension)
        {
            string strPath = ImagePath + "/" + EntityId;
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(strPath)))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strPath));
            }




            //DeleteImageFile(ImagePath, EntityId, FileName, FileExtension);

            strPath += "/" + FileName + "." + FileExtension;
            System.IO.FileStream _FileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath(strPath), System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from
            // a byte array.
            _FileStream.Write(PatientImage, 0, PatientImage.Length);

            // close file stream
            _FileStream.Close();

            strPath = strPath.Replace("~", "");

            return strPath;

        }
        public static byte[] GetImageFile(string ImagePath, string EntityId, string FileName, string FileExtension)
        {
            byte[] PatientImage = null;
            string strPath = ImagePath + "/" + EntityId;
            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(strPath)))
            {
                strPath += "/" + FileName + "." + FileExtension;


                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(strPath)))
                {


                    System.IO.FileStream _FileStream = new System.IO.FileStream(HttpContext.Current.Server.MapPath(strPath), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    // Read the source file into a byte array.
                    PatientImage = new byte[_FileStream.Length];

                    _FileStream.Read(PatientImage, 0, PatientImage.Length);

                    // close file stream
                    _FileStream.Close();


                }

            }





            return PatientImage;

        }
        public static bool DeleteImageFile(string ImagePath, string EntityId, string FileName, string FileExtension)
        {
            string strPath = ImagePath + "/" + EntityId;
            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(strPath)))
            {
                strPath += "/" + FileName + "." + FileExtension;


                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(strPath)))
                {
                    System.IO.FileInfo objfile = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(strPath));
                    objfile.Delete();
                }

                //foreach (var f in System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(strPath)))
                //    System.IO.File.Delete(f);
            }

            return true;
        }

        /// <summary>
        ///Resize the image according to the given dimensions keeping the aspect ratio constant
        /// </summary>
        /// <param name="base64">base64 string of the image which is to be resized</param>
        /// <param name="maxWidth"> maximum expected width of image</param>
        /// <param name="maxHeight">maximum expected height of image</param>
        /// <returns>Proportionally resized image according to the provided dimensions</returns>
        public static string ResizeImage(string base64, int maxWidth, int maxHeight)
        {
            Bitmap thumb;
            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (System.Drawing.Image originalImage = System.Drawing.Image.FromStream(ms))
                {
                    double ratioX = (double)maxWidth / originalImage.Width;
                    double ratioY = (double)maxHeight / originalImage.Height;
                    double ratio = Math.Min(ratioX, ratioY);
                    int newWidth = (int)(originalImage.Width * ratio);
                    int newHeight = (int)(originalImage.Height * ratio);

                    thumb = new Bitmap(newWidth, newHeight);

                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }
                }

            }

            return BitmapToBase64(thumb);
        }
        public static string BitmapToBase64(Bitmap bi)
        {
            Bitmap bImage = bi;
            MemoryStream ms = new MemoryStream();
            bImage.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage);
        }


        public static string CropAndResizeImage(System.Drawing.Image img, int targetWidth, int targetHeight, int x1, int y1, int x2, int y2, ImageFormat imageFormat)
        {
            var bmp = new Bitmap(targetWidth, targetHeight);
            Graphics g = Graphics.FromImage(bmp);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            int width = x2 - x1;
            int height = y2 - y1;

            g.DrawImage(img, new System.Drawing.Rectangle(0, 0, targetWidth, targetHeight), x1, y1, width, height, GraphicsUnit.Pixel);

            var memStream = new MemoryStream();
            bmp.Save(memStream, imageFormat);

            return Convert.ToBase64String(memStream.ToArray());
            //  return Image.FromStream(memStream);
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="img">The image to be resized</param>
        /// <param name="targetWidth">Width of the target</param>
        /// <param name="targetHeight">Height of the target</param>
        /// <param name="imageFormat">The image format</param>
        /// <returns>A resized base64 image</returns>
        public static string ResizeImage(System.Drawing.Image img, int targetWidth, int targetHeight, ImageFormat imageFormat)
        {
            return CropAndResizeImage(img, targetWidth, targetHeight, 0, 0, img.Width, img.Height, imageFormat);
        }

        /// <summary>
        /// Crops the image.
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="x1">The position x1.</param>
        /// <param name="y1">The position y1.</param>
        /// <param name="x2">The position x2.</param>
        /// <param name="y2">The position y2.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>A cropped base64 image.</returns>
        public static string CropImage(System.Drawing.Image img, int x1, int y1, int x2, int y2, ImageFormat imageFormat)
        {
            return CropAndResizeImage(img, x2 - x1, y2 - y1, x1, y1, x2, y2, imageFormat);
        }


        #endregion

        #region HELPERS
        public static int getPdfPagesCount(byte[] byteArrr)
        {
            PdfReader pdfReader = new PdfReader(byteArrr);
            return pdfReader.NumberOfPages;
        }

        public static string GetLanIPAddress()
        {

            //Get the Host Name

            string stringHostName = Dns.GetHostName();

            //Get The Ip Host Entry

            IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);

            //Get The Ip Address From The Ip Host Entry Address List
            string IP = "";
            System.Net.IPAddress[] arrIpAddress = ipHostEntries.AddressList;
            foreach (System.Net.IPAddress ipAddress in arrIpAddress)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IP = ipAddress.ToString();
                }
            }

            return IP;

        }
        public static byte[] CombineMultipleByteArrays(List<byte[]> lstByteArray)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        foreach (var p in lstByteArray)
                        {
                            using (var reader = new PdfReader(p))
                            {
                                copy.AddDocument(reader);
                            }
                        }

                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }

        #endregion

        #region " PDF Creator "

        public class PDFCreator
        {
            public iTextSharp.text.Font _largeFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            public iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            public iTextSharp.text.Font _smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            public PdfWriter Writer { get; set; }
            public Document Document { get; set; }

            public Stream Stream { get; set; }

            public float Column { get; private set; }

            public float Row { get; private set; }

            public float RowMargin { get; private set; }

            public bool IsUppercase { get; set; }


            public PDFCreator(ref Document doc, Stream stream, bool IsUppercase = false, string document_creator = "MDVision", string document_author = "MDVision", string document_title = "MDVision Document", string document_subject = "")
            {
                this.Document = doc;

                this.Writer = PdfWriter.GetInstance(Document, stream);
                this.Column = (Document.PageSize.Width / 42.55f);
                this.RowMargin = Document.PageSize.Height / 29.55f;
                this.Row = (Document.PageSize.Height - RowMargin);
                this.Document.AddAuthor(document_author);
                this.Document.AddTitle(document_title);
                this.Document.AddSubject(document_subject);
                this.Document.AddCreator(document_creator);
                this.IsUppercase = IsUppercase;
            }

            ~PDFCreator()
            {
                try
                {
                    // this.Document.Close();
                    // this.Writer.Close();
                }
                catch (Exception ex)
                {

                    MDVLogger.PresentationErrorLog("PDFCreatorDestructor", ex, MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper());
                }
            }

            public float WriteText(string text, float column, float rowMargin, bool InserNewLine = false, int Align = PdfContentByte.ALIGN_LEFT)
            {
                if (this.IsUppercase)
                    text = text.ToUpper();

                float nextline = 0f;
                string line1 = text;
                string line2 = string.Empty;
                if (InserNewLine)
                {
                    string[] str_temp = text.Split('\n');
                    if (str_temp.Length > 1)
                    {
                        line1 = str_temp[0];
                        line2 = str_temp[1];
                    }

                }

                PdfContentByte obj = Writer.DirectContent;
                obj.BeginText();
                obj.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false), 9);
                obj.SetTextMatrix(column, Document.PageSize.Height - rowMargin);
                //obj.ShowText(line1);
                obj.ShowTextAligned(Align, line1, column, Document.PageSize.Height - rowMargin, 0f);
                obj.EndText();

                if (line2 != string.Empty)
                {
                    float number = (rowMargin / this.RowMargin) + 0.5f;
                    WriteText(line2, column, this.RowMargin * number);
                    nextline = 0.5f;
                }

                return nextline;
            }
        }




        public partial class AddFooterHeader : PdfPageEventHelper
        {
            // This is the contentbyte object of the writer
            PdfContentByte cb;

            // we will put the final number of pages in a template
            PdfTemplate headerTemplate, footerTemplate;

            // this is the BaseFont we are going to use for the header / footer
            BaseFont bf = null;

            // This keeps track of the creation time
            DateTime PrintTime = DateTime.Now;


            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            public PdfPTable ReportHeaderTable;
            public bool IsFooterExist { get; set; }
            public string FooterGeneratedBy { get; set; }
            public PdfPTable ReportFooterTable = null;
            #endregion


            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    //  bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    footerTemplate = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {

                }
                catch (System.IO.IOException ioe)
                {

                }
            }

            public AddFooterHeader(PdfPTable _ReportHeaderTable, bool _IsFooterExist, string _FooterGeneratedBy, PdfPTable _FooterTable)
            {
                ReportHeaderTable = _ReportHeaderTable;
                FooterGeneratedBy = _FooterGeneratedBy;
                IsFooterExist = _IsFooterExist;
                ReportFooterTable = _FooterTable;
            }
            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                base.OnEndPage(writer, doc);
                if (writer.PageNumber > 1 && this.ReportHeaderTable == null)
                {
                    float topMargin1 = 30;
                    cb.MoveTo(20, doc.PageSize.Height - topMargin1);
                    cb.LineTo(doc.PageSize.Width - 20, doc.PageSize.Height - topMargin1);
                    cb.Stroke();
                }
                #region adding footer
                if (IsFooterExist)
                {
                    if (ReportFooterTable == null && !string.IsNullOrEmpty(FooterGeneratedBy))
                    {
                        String text = "Generated By: " + FooterGeneratedBy.ToString();
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 10);
                        cb.SetTextMatrix(20, doc.PageSize.GetBottom(30));
                        cb.ShowText(text);
                        cb.EndText();
                        #region adding lines to footer
                        //Move the pointer and draw line to separate footer section from rest of page
                        cb.MoveTo(20, doc.PageSize.GetBottom(45));
                        cb.LineTo(doc.PageSize.Width - 20, doc.PageSize.GetBottom(45));
                        cb.Stroke();
                        #endregion
                    }
                    else
                    {
                        if (this.ReportFooterTable != null && this.ReportFooterTable.Rows.Count > 0)
                        {
                            this.ReportFooterTable.HorizontalAlignment = Element.ALIGN_CENTER;
                            this.ReportFooterTable.WriteSelectedRows(0, -1, 20, doc.PageSize.GetBottom(30), writer.DirectContent);
                        }
                    }
                }
                cb.AddTemplate(footerTemplate, 20, doc.PageSize.GetBottom(30));
                #endregion

                #region adding header
                if (this.ReportHeaderTable != null && this.ReportHeaderTable.Rows.Count > 0)
                {
                    this.ReportHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    this.ReportHeaderTable.WriteSelectedRows(0, -1, 20, doc.PageSize.Height - 30, writer.DirectContent);
                    if (writer.PageNumber > 1)
                    {
                        #region adding lines to header
                        //Move the pointer and draw line to separate header section from rest of page
                        float topMargin = ReportHeaderTable.CalculateHeights() + 30;
                        cb.MoveTo(20, doc.PageSize.Height - topMargin);
                        cb.LineTo(doc.PageSize.Width - 20, doc.PageSize.Height - topMargin);
                        cb.Stroke();
                        #endregion
                    }
                }
                #endregion

            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                footerTemplate.BeginText();
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                footerTemplate.SetFontAndSize(bf, 20);
                footerTemplate.SetTextMatrix(document.PageSize.GetRight(140), 0);

                footerTemplate.ShowText((writer.PageNumber).ToString());
                footerTemplate.EndText();

                //cb.BeginText();
                //cb.SetFontAndSize(bf, 10);
                //cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                //cb.ShowText((writer.PageNumber).ToString());
                //cb.EndText();

                //cb.AddTemplate(footerTemplate, 20, document.PageSize.GetBottom(30));
            }
        }
        #endregion

        #region " DataSet "

        public static void GetUpperLowerCaseDataSet(DataSet dataset, bool IsUpper)
        {
            foreach (DataTable table in dataset.Tables)
            {
                foreach (DataColumn col in table.Columns)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (IsUpper)
                            row[col.ColumnName] = row[col.ColumnName].ToString().ToUpper();
                        else
                            row[col.ColumnName] = row[col.ColumnName].ToString().ToLower();

                    }
                }
            }
        }

        public static DataRow CopyRow(DataRow src, DataSet ds, string TableName, List<string> IgnoreList)
        {
            DataRow new_dr = ds.Tables[TableName].NewRow();
            for (int i = 0; i < ds.Tables[TableName].Columns.Count; i++)
            {
                if (!string.IsNullOrEmpty(ToStr(src[ds.Tables[TableName].Columns[i].ColumnName])) && !IgnoreList.Contains(ds.Tables[TableName].Columns[i].ColumnName))
                    new_dr[ds.Tables[TableName].Columns[i].ColumnName] = src[ds.Tables[TableName].Columns[i].ColumnName];
            }

            return new_dr;

        }


        #endregion

        #region Format
        public static string ZipCode(string Str)
        {
            Str = GetNum(Str);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Str.Length == 9)
            {
                sb.Append(Str.Substring(0, 5));
                sb.Append("-");
                sb.Append(Str.Substring(5, 4));
                Str = sb.ToString();
            }
            return Str;
        }
        private static string GetNum(string Str)
        {
            if (Str == null)
            {
                return string.Empty;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            char[] Arr = null;
            Arr = Str.ToCharArray();


            foreach (char ch in Arr)
            {

                if (char.IsNumber(ch) == true)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }

        public static string ToTitleCase(string Str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Str.ToLower());
        }

        public static string PhoneNo(string Str)
        {
            Str = GetNum(Str);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Str.Length == 10)
            {

                sb.Append("(");
                sb.Append(Str.Substring(0, 3));
                sb.Append(")");
                sb.Append(Str.Substring(3, 3));
                sb.Append("-");
                sb.Append(Str.Substring(6, 4));
                Str = sb.ToString();
            }
            return Str;
        }

        public static string FormatPhoneNumber(string str)
        {
            if (!string.IsNullOrEmpty(ToStr(str)))
                return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            else
                return string.Empty;
        }

        public static Boolean isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
        }

        //public static bool ValidateFormat(string format, string inputText)
        //{
        //    try
        //    {
        //        MaskedTextBox MaskEditor = new MaskedTextBox();
        //        if (string.IsNullOrEmpty(format) & string.IsNullOrEmpty(inputText))
        //        {
        //            return true;
        //        }
        //        else if (string.IsNullOrEmpty(format))
        //        {
        //            return true;
        //        }
        //        System.ComponentModel.MaskedTextProvider msk = default(System.ComponentModel.MaskedTextProvider);
        //        string maskingstring_1 = format.Split('%')[0];

        //        string[] allMasks = format.Split('|');
        //        string OrginalInput = inputText;
        //        //if (allMasks.Length > 1)
        //        //{
        //        foreach (string strMask in allMasks)
        //        {
        //            if (string.IsNullOrEmpty(strMask))
        //            {
        //                continue;
        //            }
        //            string maskingstring;
        //            maskingstring = strMask.Replace("~", "?");
        //            maskingstring = maskingstring.Replace("*", "A");
        //            inputText = OrginalInput;
        //            if (maskingstring.Contains("%"))
        //            {
        //                string maskingstring_ = maskingstring.Split('%')[0];
        //                MaskEditor.Mask = maskingstring_;
        //                msk = MaskEditor.MaskedTextProvider;
        //                if (inputText.Length > maskingstring_.Length)
        //                    inputText = inputText.Remove(maskingstring_.Length);
        //                if (msk.VerifyString(inputText) == true && maskingstring_.Length <= inputText.Length)
        //                {
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                MaskEditor.Mask = maskingstring;

        //                msk = MaskEditor.MaskedTextProvider;
        //                if (msk.VerifyString(inputText) == true && maskingstring.Length == inputText.Length)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //        //}

        //        bool HasAlphabets = MDVUtility.isAlphaNumeric(maskingstring_1);
        //        if (HasAlphabets)
        //        {
        //            var formatChar = format.ToLower().ToCharArray();
        //            for (int i = 0; i < formatChar.Length; i++)
        //            {
        //                if (char.IsLetter(formatChar[i]))
        //                {
        //                    if (!(formatChar[i] == inputText.ToLower()[i]))
        //                    {
        //                        return false;
        //                    }
        //                }
        //            }
        //        }


        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        // MessageBox.Show(ex.Message, "Mask", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //}
        public static bool ValidateFormatRegex(string format, string inputText)
        {
            string pattern = string.Empty;
            format = format.Replace("!per", "%").ToUpper();
            //if (format.Contains("|"))
            //{
            //    pattern = "\\b" + format.Replace("~", "[a-zA-Z]").Replace("#", "[0-9]").Replace("*", "[a-zA-Z0-9]").Replace("%", "[\\w-]+").Replace("|", "\\b|\\b") + "\\b";
            //}
            //else if (format.Contains("~") || format.Contains('#') || format.Contains("*") || format.Contains("%"))
            //{
            //    pattern = "^" + format.Replace("~", "[a-zA-Z]").Replace("#", "[0-9]").Replace("*", "[a-zA-Z0-9]").Replace("%", "[\\w-]+") + "\\b";
            //}
            //else
            //{
            //    pattern = "^(" + format + ")\\b";
            //}
            pattern = GenerateRegex(format);
            Regex regex = new Regex(pattern);
            Match match = regex.Match(inputText);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GenerateRegex(string format)
        {
            string pattern = string.Empty;
            format = format.Replace("!per", "%");
            if (format.Contains("|"))
            {
                pattern = "^\\b(?i)" + format.Replace("~", "[a-zA-Z]").Replace("#", "[0-9]").Replace("*", "[a-zA-Z0-9]").Replace("%", "[\\w-]+").Replace("|", "\\b|\\b(?i)") + "\\b";
            }
            else if (format.Contains("~") || format.Contains('#') || format.Contains("*") || format.Contains("%"))
            {
                pattern = "^(?i)" + format.Replace("~", "[a-zA-Z]").Replace("#", "[0-9]").Replace("*", "[a-zA-Z0-9]").Replace("%", "[\\w-]+") + "\\b";
            }
            else if (format.Contains("_") || format.Contains("-"))
            {
                pattern = "^(?i)(" + format + ")";
            }
            else
            {
                pattern = "^(?i)(" + format + ")\\b";
            }

            return pattern;
        }


        /// <summary>
        /// generate Regular expression against the provided Criteia
        /// </summary>
        /// <param name="minPasswordLength"></param>
        /// <param name="minSpecialCharacter"></param>
        /// <param name="minAlphaCharacter"></param>
        /// <param name="minNumeriCharacter"></param>
        /// <param name="minUppercaseeCharacter"></param>
        /// <returns>return the Regex String</returns>
        public static string GenerateRegex(int minPasswordLength = 8, int minSpecialCharacter = 0, int minAlphaCharacter = 0, int minNumeriCharacter = 0, int minUppercaseeCharacter = 0)
        {
            string minPasswordLengthExp = "";
            string minSpecialCharacterExp = "";
            string minAlphaCharacterExp = "";
            string minNumeriCharacterExp = "";
            string minUppercaseeCharacterExp = "";

            if (minPasswordLength > 0)
            {
                minPasswordLengthExp = ".{" + minPasswordLength.ToString() + ",}";
            }

            if (minSpecialCharacter > 0)
            {
                minSpecialCharacterExp = @"(?=(.*\W){" + minSpecialCharacter.ToString() + "})";
            }
            if (minAlphaCharacter > 0)
            {
                minAlphaCharacterExp = @"(?=(.*[a-z]){" + minAlphaCharacter.ToString() + "})";
            }
            if (minNumeriCharacter > 0)
            {
                minNumeriCharacterExp = @"(?=(.*\d){" + minNumeriCharacter.ToString() + "})";
            }
            if (minUppercaseeCharacter > 0)
            {
                minUppercaseeCharacterExp = @"(?=(.*[A-Z]){" + minUppercaseeCharacter.ToString() + "})";
            }

            //string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            string strRegex = @"" + minNumeriCharacterExp + minAlphaCharacterExp + minUppercaseeCharacterExp + minSpecialCharacterExp + minPasswordLengthExp;
            return strRegex;
        }

        public static bool IsStringValidForRegex(string stringToValidate, string regex)
        {
            Regex regexObj = new Regex(regex);
            Match match = regexObj.Match(stringToValidate);

            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        public static string ReplaceSpecialCharacters(string target)
        {
            StringBuilder str = new StringBuilder();
            foreach (var item in SpecialCharacters)
            {
                str.Append(target.Replace(item.Key, item.Value));
            }
            return str.ToString();
        }


        public static Double ToAmount(Object val)
        {
            Double dbl;
            return Double.TryParse(MDVUtility.ToStr(val), out dbl) ? Math.Round(dbl, 2) : -1;
        }

        public static string To24HrTime(string time)
        {
            try
            {
                char[] delimiters = new char[] { ':', ' ' };
                string[] spltTime = time.Split(delimiters);

                int hour;
                int minute;
                int.TryParse(spltTime[0], out hour);
                int.TryParse(spltTime[1], out minute);
                int seconds = 0;

                string amORpm = spltTime[2];

                if (amORpm.ToUpper() == "PM")
                {
                    hour = (hour % 12) + 12;
                }

                return new TimeSpan(hour, minute, seconds).ToString();
            }
            catch (Exception)
            {
                return time;
            }

        }

        public static string To12HrTime(string time)
        {
            try
            {
                char[] delimiters = new char[] { ':', ' ' };
                string[] spltTime = time.Split(delimiters);

                int hour;
                int minute;
                int.TryParse(spltTime[0], out hour);
                int.TryParse(spltTime[1], out minute);


                if (hour > 12)
                {
                    hour = hour - 12;

                    string hour_ = hour.ToString().Length == 1 ? "0" + hour : hour.ToString();
                    string minute_ = minute.ToString().Length == 1 ? "0" + minute : minute.ToString();
                    return hour_ + ":" + minute_ + " PM";
                }
                else
                {

                    string hour_ = hour.ToString().Length == 1 ? "0" + hour : hour.ToString();
                    string minute_ = minute.ToString().Length == 1 ? "0" + minute : minute.ToString();
                    return hour_ + ":" + minute_ + " AM";
                }

            }
            catch (Exception)
            {
                return time;
            }

        }


        #endregion

        #region Edi
        public static void SetDelimiters(string StrIn)
        {
            try
            {


                string str = StrIn.Substring(0, 0x6a);
                if (StrIn.Substring(str.Length - 1, 1).Length > 0)
                {
                    D_SGMT = StrIn.Substring(str.Length - 1, 1).ToCharArray()[0];
                }
                if (StrIn.Substring(str.Length - 3, 1).Length > 0)
                {
                    D_ELMT = StrIn.Substring(str.Length - 3, 1).ToCharArray()[0];
                }
                if (StrIn.Substring(str.Length - 2, 1).Length > 0)
                {
                    D_S_ELMT = StrIn.Substring(str.Length - 2, 1).ToCharArray()[0];
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public static string WhichEDI(string StrIn)
        {
            string str2;

            try
            {
                string[] strArray = null;
                string[] strArray2 = null;
                strArray = StrIn.Split(new char[] { D_SGMT });
                foreach (string str in strArray)
                {
                    if (str.Substring(0, 2) == "GS")
                    {
                        strArray2 = str.Split(new char[] { D_ELMT });
                        if (strArray2.Length > 0)
                        {
                            string str3 = strArray2[1];
                            if (str3 != null)
                            {
                                if (!(str3 == "HB"))
                                {
                                    if (str3 == "HP")
                                    {
                                        return "835";
                                    }
                                    if (str3 == "FA")
                                    {
                                        return "997";
                                    }
                                    if (str3 == "HN")
                                    {
                                        return "277";
                                    }
                                }
                                else
                                {
                                    return "271";
                                }
                            }
                        }
                    }
                }
                str2 = "UnknownEDI";
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str2;
        }
        #endregion
        
        #region Control Chars
        public sealed class ControlChars
        {
            public const char Back = '\b';
            public const char Cr = '\r';
            public const string CrLf = "\r\n";
            public const char FormFeed = '\f';
            public const char Lf = '\n';
            public const string NewLine = "\r\n";
            public const char NullChar = '\0';
            public const char Quote = '"';
            public const char Tab = '\t';
            public const char VerticalTab = '\v';
            public const char Comma = ',';

        }
        #endregion

        #region Function Use In DrFirst Interaction
        // Author:  Muhammad Ahmad Imran
        // Created Date: 14/01/2016
        //OverView: Add new method GetHeadXmlForSendToDrFirst for GET XML For Add Patient In DrFirst
        public static string GetHeadXmlForSendToDrFirst(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername)
        {
            string HeaderXml = "<Caller><VendorName>" + VendorName + "</VendorName><VendorPassword>" + VendorPassword + "</VendorPassword></Caller><SystemName>" + SystemName + "</SystemName><RcopiaPracticeUsername>" + RcopiaPracticeUsername + "</RcopiaPracticeUsername><Request>";
            return HeaderXml;
        }
        public static string GetXmlForAddPatient(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, DataRow patient, string PatientID)
        {
            char Gender;
            if (patient["Gender"].ToString() == "Male")
            {
                Gender = 'm';
            }
            else
            {
                Gender = 'f';
            }
            ;
            //patient["Address2"].ToString()
            //    uploadPatient = uploadPatient.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
            string strPatientFName = patient["FirstName"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
            string strPatientMName = patient["MI"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
            string strPatientLName = patient["LastName"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

            string strAddress1 = patient["Address1"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
            string strAddress2 = patient["Address2"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

            string PatientXml = "<Command>send_patient</Command>                <PatientList>            <Patient>                    <ExternalID>" + PatientID + "</ExternalID>                    <FirstName>" + strPatientFName + "</FirstName>                    <MiddleName>" + strPatientMName + "</MiddleName>                    <LastName>" + strPatientLName + "</LastName>                    <DOB>" + patient["DOB"].ToString() + "</DOB>                    <Sex>" + Gender + "</Sex>                    <HomePhone>" + patient["HomePhoneNo"].ToString() + "</HomePhone>                    <Address1>" + strAddress1 + "</Address1>                    <Address2>" + strAddress2 + "</Address2>                    <City>" + patient["City"].ToString() + "</City>                    <State>" + patient["State"].ToString() + "</State>                    <Zip>" + patient["ZIPCode"].ToString() + "</Zip>            </Patient>	        </PatientList>    </Request></RCExtRequest>";
            PatientXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + PatientXml;
            return PatientXml;
        }
        //public static string Ge
        public static string GetXmlForUpdatePatient(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, DataRow patient)
        {
            char Gender;
            if (patient["Gender"].ToString() == "Male")
            {
                Gender = 'm';
            }
            else
            {
                Gender = 'f';
            }

            string strPatientFName = patient["FirstName"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
            string strPatientMName = patient["MI"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
            string strPatientLName = patient["LastName"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

            string strAddress1 = patient["Address1"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
            string strAddress2 = patient["Address2"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

            string PatientXml = "<Command>send_patient</Command>                <PatientList>            <Patient>                    <ExternalID>" + patient["PatientId"].ToString() + "</ExternalID>                    <FirstName>" + strPatientFName + "</FirstName>                    <MiddleName>" + strPatientMName + "</MiddleName>                    <LastName>" + strPatientLName + "</LastName>                    <DOB>" + patient["DOB"].ToString() + "</DOB>                    <Sex>" + Gender + "</Sex>                    <HomePhone>" + patient["HomePhoneNo"] + "</HomePhone>                    <Address1>" + strAddress1 + "</Address1>                    <Address2>" + strAddress2 + "</Address2>                    <City>" + patient["City"] + "</City>                    <State>" + patient["State"] + "</State>                    <Zip>" + patient["ZIPCode"] + "</Zip>            </Patient>	        </PatientList>    </Request></RCExtRequest>";
            PatientXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + PatientXml;
            return PatientXml;
        }
        public static string GetXmlForAddProblemList(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, DataRow Problem, string ProblemID)
        {
            string ProblemXml = "";
            var startDate = "";
            object value = Problem["StartDate"];

            if (value == DBNull.Value)
            {

                startDate = "";
            }
            else
            {
                startDate = GetDateMMDDYYY(ToStr(Problem["StartDate"]));
            }
            if (Problem.Table.Columns.Contains("ICD10") && Problem.Table.Columns.Contains("ICD10_Description") && !String.IsNullOrEmpty(Problem["ICD10"].ToString()) && !String.IsNullOrEmpty(Problem["ICD10_Description"].ToString()))
            {
                var description = "";
                var Code = "";
                var ICD10 = Problem["ICD10"].ToString();
                var ICD10_Description = Problem["ICD10_Description"].ToString();
                if (ICD10.Count() > 0)
                {
                    Code = ICD10;
                }
                if (ICD10_Description.Count() > 0)
                {
                    description = ICD10_Description;
                }


                ProblemXml = "<Command>send_problem</Command>		<ProblemList>			<Problem>				<Deleted>n</Deleted>				<Status><Active/></Status>				<OnsetDate>" + startDate + "</OnsetDate>				<RcopiaID></RcopiaID>				<ExternalID>" + Problem["ProblemListId"] + "</ExternalID>				<Patient>				    <RcopiaID></RcopiaID>				     <ExternalID>" + Problem["PatientId"] + "</ExternalID>				</Patient>				<ICD10>					<Code>" + Code + "</Code>					<Description>" + description + "</Description>				</ICD10>			</Problem>		</ProblemList>        </Request></RCExtRequest>";
            }
            else if (Problem.Table.Columns.Contains("Description") && !String.IsNullOrEmpty(Problem["Description"].ToString()))
            {
                var getcode = Problem["Description"].ToString().Split('-');
                var description = "";
                var Code = "";
                if (getcode.Count() == 2)
                {
                    Code = getcode[0];
                    description = getcode[1];
                }
                else if (getcode.Count() == 3)
                {
                    Code = getcode[1];
                    description = getcode[2];
                }

                ProblemXml = "<Command>send_problem</Command>		<ProblemList>			<Problem>				<Deleted>n</Deleted>				<Status><Active/></Status>				<OnsetDate>" + startDate + "</OnsetDate>				<RcopiaID></RcopiaID>				<ExternalID>" + Problem["ProblemListId"] + "</ExternalID>				<Patient>				    <RcopiaID></RcopiaID>				     <ExternalID>" + Problem["PatientId"] + "</ExternalID>				</Patient>				<ICD10>					<Code>" + Code + "</Code>					<Description>" + description + "</Description>				</ICD10>			</Problem>		</ProblemList>        </Request></RCExtRequest>";
            }

            ProblemXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + ProblemXml;
            return ProblemXml;
        }
        public static string GetXmlForUpdateProblem(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, DataRow Problem)
        {
            string ProblemXml = "";
            string status = "<Active/>";
            string Deleted = "n";
            if (Convert.ToBoolean(Problem["IsActive"]) != true)
            {
                if (Problem["InActiveChkBoxValue"].ToString() != null)
                {
                    if (Problem["InActiveChkBoxValue"].ToString() == "Resolved")
                    {
                        status = "<Resolved/>";
                        Deleted = "y";
                    }
                    else
                    {
                        status = "<Inactive/>";
                        Deleted = "y";
                    }

                }
            }
            var startDate = "";
            object value = Problem["StartDate"];

            if (value == DBNull.Value)
            {

                startDate = "";
            }
            else
            {
                startDate = GetDateMMDDYYY(ToStr(Problem["StartDate"]));
            }

            if (!String.IsNullOrEmpty(Problem["Description"].ToString()))
            {
                var getcode = Problem["Description"].ToString().Split('-');
                var description = "";
                var Code = "";
                if (getcode.Count() > 1)
                {
                    Code = getcode[1];
                }
                for (int i = 2; i < getcode.Count(); i++)
                {
                    description += getcode[i];
                }


                ProblemXml = "<Command>send_problem</Command>		<ProblemList>			<Problem>				<Deleted>" + Deleted + "</Deleted>				<Status>" + status + "</Status>				<OnsetDate>" + startDate + "</OnsetDate>				<RcopiaID></RcopiaID>				<ExternalID>" + Problem["ProblemListId"] + "</ExternalID>				<Patient>				    <RcopiaID></RcopiaID>				     <ExternalID>" + Problem["PatientId"] + "</ExternalID>				</Patient>				<ICD10>					<Code>" + Code + "</Code>						<Description>" + description + "</Description>				</ICD10>			</Problem>		</ProblemList>        </Request></RCExtRequest>";
            }
            else
            {
                var getcode = Problem["ProblemName"].ToString().Split('-');
                var description = "";
                var Code = "";
                if (getcode.Count() > 1)
                {
                    Code = getcode[1];
                }
                for (int i = 2; i < getcode.Count(); i++)
                {
                    description += getcode[i];
                }
                ProblemXml = "<Command>send_problem</Command>		<ProblemList>			<Problem>				<Deleted>" + Deleted + "</Deleted>				<Status>" + status + "</Status>				<OnsetDate>" + startDate + "</OnsetDate>				<RcopiaID></RcopiaID>				<ExternalID>" + Problem["ProblemListId"] + "</ExternalID>				<Patient>				    <RcopiaID></RcopiaID>				     <ExternalID>" + Problem["PatientId"] + "</ExternalID>				</Patient>				<ICD10>					<Code>" + Code + "</Code>						<Description>" + description + "</Description>				</ICD10>			</Problem>		</ProblemList>        </Request></RCExtRequest>";
            }



            ProblemXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + ProblemXml;
            return ProblemXml;
        }
        public static string GetXmlForUpdateMedication(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, List<OS_MedicationModel> MedicationModelList, string ProviderUsername = "", long PatientId = 0)
        {

            string MedicationXml = "";
            MedicationXml += "<Command>send_medication</Command>        <MedicationList>";
            foreach (OS_MedicationModel model in MedicationModelList)
            {
                MedicationXml += "<Medication>";
                MedicationXml += "<Deleted>n</Deleted>";
                MedicationXml += "<RcopiaID></RcopiaID>";
                MedicationXml += "<ExternalID></ExternalID>";
                MedicationXml += "<Patient>";
                MedicationXml += "    <RcopiaID></RcopiaID>";
                MedicationXml += "    <ExternalID>" + PatientId + "</ExternalID>";
                MedicationXml += "</Patient>";
                MedicationXml += "<Provider>";
                MedicationXml += "    <Username>" + ProviderUsername + "</Username>";
                MedicationXml += "</Provider>";
                MedicationXml += "<Sig>";
                MedicationXml += "    <Drug>";
                MedicationXml += "        <NDCID>" + (string.IsNullOrEmpty(model.NDCID) ? "" : model.NDCID) + "</NDCID>";
                MedicationXml += "        <BrandName>" + (string.IsNullOrEmpty(model.BrandName) ? "" : model.BrandName) + "</BrandName>";
                MedicationXml += "        <GenericName>" + (string.IsNullOrEmpty(model.GenericName) ? "" : model.GenericName) + "</GenericName>";
                MedicationXml += "        <Form>" + (string.IsNullOrEmpty(model.Form) ? "" : model.Form) + "</Form>";
                MedicationXml += "        <Strength>" + (string.IsNullOrEmpty(model.Strength) ? "" : model.Strength) + "</Strength>";
                MedicationXml += "    </Drug>";
                MedicationXml += "    <Action>" + (string.IsNullOrEmpty(model.ActionName) ? "" : model.ActionName) + "</Action>";
                MedicationXml += "    <Dose>" + (string.IsNullOrEmpty(model.Dose) ? "" : model.Dose) + "</Dose>";
                MedicationXml += "    <DoseUnit>" + (string.IsNullOrEmpty(model.DoseUnitName) ? "" : model.DoseUnitName) + "</DoseUnit>";
                MedicationXml += "    <Route>" + (string.IsNullOrEmpty(model.RouteName) ? "" : model.RouteName) + "</Route>";
                MedicationXml += "    <DoseTiming>" + (string.IsNullOrEmpty(model.DoseTimingName) ? "" : model.DoseTimingName) + "</DoseTiming>";
                MedicationXml += "    <DoseOther>" + (string.IsNullOrEmpty(model.DoseOtherName) ? "" : model.DoseOtherName) + "</DoseOther>";
                //MedicationXml += "    <Duration>" + (string.IsNullOrEmpty(model.DurationName) ? "" : model.DurationName) + "</Duration>";
                var duration = model.DurationName.Replace("days", "").Trim();
                if (duration == "one day")
                {
                    duration = "1";
                }
                else if (duration == "two")
                {
                    duration = "2";
                }
                else if (duration == "three")
                {
                    duration = "3";
                }
                else if (duration == "four")
                {
                    duration = "4";
                }
                else if (duration == "five")
                {
                    duration = "5";
                }
                else if (duration == "six")
                {
                    duration = "6";
                }
                else if (duration == "seven")
                {
                    duration = "7";
                }
                MedicationXml += "    <Duration>" + (string.IsNullOrEmpty(duration) ? "" : duration) + "</Duration>";
                MedicationXml += "    <Quantity>" + (string.IsNullOrEmpty(model.Quantity) ? "" : model.Quantity) + "</Quantity>";
                MedicationXml += "    <QuantityUnit>" + (string.IsNullOrEmpty(model.QuantityUnitName) ? "" : model.QuantityUnitName) + "</QuantityUnit>";

                MedicationXml += "    <Refills>" + (string.IsNullOrEmpty(model.Refill) ? "" : (model.Refill == "none" ? "" : model.Refill)) + "</Refills>";
                if (model.DirectionsToPharmacist == "Substitution permitted")
                {
                    MedicationXml += "    <SubstitutionPermitted>y</SubstitutionPermitted>";
                }
                else
                {
                    MedicationXml += "    <SubstitutionPermitted>n</SubstitutionPermitted>";
                }
                MedicationXml += "    <OtherNotes>" + (string.IsNullOrEmpty(model.PrescriptionsOtherNotes) ? "" : model.PrescriptionsOtherNotes) + "</OtherNotes>";
                MedicationXml += "    <PatientNotes>" + (string.IsNullOrEmpty(model.AddDirectionToPatient) ? "" : model.AddDirectionToPatient) + "</PatientNotes>";
                MedicationXml += "    <Comments>" + (string.IsNullOrEmpty(model.Comments) ? "" : model.Comments) + "</Comments>";
                MedicationXml += "</Sig>";
                MedicationXml += "<StartDate>"+DateTime.Now.ToShortDateString()+"</StartDate>";
                MedicationXml += "<StopDate></StopDate>";
                MedicationXml += "<StopReason></StopReason>";
                MedicationXml += "</Medication>";
            }
            MedicationXml += "</MedicationList>";
            MedicationXml += "</Request>";
            MedicationXml += "</RCExtRequest>";
            MedicationXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + MedicationXml;
            return MedicationXml;
        }
        public static string GetXmlForUpdateMedicationDelete(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, List<OS_MedicationModel> MedicationModelList, string ProviderUsername = "", long PatientId = 0)
        {
            string MedicationXml = "";
            MedicationXml += "<Command>send_medication</Command>        <MedicationList>";
            foreach (OS_MedicationModel model in MedicationModelList)
            {
                MedicationXml += "<Medication>";
                MedicationXml += "<Deleted>y</Deleted>";
                MedicationXml += "<RcopiaID>" + model.RcopiaID + "</RcopiaID>";
                MedicationXml += "<ExternalID></ExternalID>";
                MedicationXml += "<Patient>";
                MedicationXml += "    <RcopiaID></RcopiaID>";
                MedicationXml += "    <ExternalID>" + PatientId + "</ExternalID>";
                MedicationXml += "</Patient>";
                MedicationXml += "<Provider>";
                MedicationXml += "    <Username>" + ProviderUsername + "</Username>";
                MedicationXml += "</Provider>";
                MedicationXml += "<Sig>";
                MedicationXml += "    <Drug>";
                MedicationXml += "        <NDCID>" + (string.IsNullOrEmpty(model.NDCID) ? "" : model.NDCID) + "</NDCID>";
                MedicationXml += "        <BrandName>" + (string.IsNullOrEmpty(model.BrandName) ? "" : model.BrandName) + "</BrandName>";
                MedicationXml += "        <GenericName>" + (string.IsNullOrEmpty(model.GenericName) ? "" : model.GenericName) + "</GenericName>";
                MedicationXml += "        <Form>" + (string.IsNullOrEmpty(model.Form) ? "" : model.Form) + "</Form>";
                MedicationXml += "        <Strength>" + (string.IsNullOrEmpty(model.Strength) ? "" : model.Strength) + "</Strength>";
                MedicationXml += "    </Drug>";
                MedicationXml += "    <Action>" + (string.IsNullOrEmpty(model.ActionName) ? "" : model.ActionName) + "</Action>";
                MedicationXml += "    <Dose>" + (string.IsNullOrEmpty(model.Dose) ? "" : model.Dose) + "</Dose>";
                MedicationXml += "    <DoseUnit>" + (string.IsNullOrEmpty(model.DoseUnitName) ? "" : model.DoseUnitName) + "</DoseUnit>";
                MedicationXml += "    <Route>" + (string.IsNullOrEmpty(model.RouteName) ? "" : model.RouteName) + "</Route>";
                MedicationXml += "    <DoseTiming>" + (string.IsNullOrEmpty(model.DoseTimingName) ? "" : model.DoseTimingName) + "</DoseTiming>";
                MedicationXml += "    <DoseOther>" + (string.IsNullOrEmpty(model.DoseOtherName) ? "" : model.DoseOtherName) + "</DoseOther>";
                var duration = model.DurationName.Replace("days", "").Trim();
                if (duration == "one day")
                {
                    duration = "1";
                }
                else if (duration == "two")
                {
                    duration = "2";
                }
                else if (duration == "three")
                {
                    duration = "3";
                }
                else if (duration == "four")
                {
                    duration = "4";
                }
                else if (duration == "five")
                {
                    duration = "5";
                }
                else if (duration == "six")
                {
                    duration = "6";
                }
                else if (duration == "seven")
                {
                    duration = "7";
                }
                MedicationXml += "    <Duration>" + (string.IsNullOrEmpty(duration) ? "" : duration) + "</Duration>";
                MedicationXml += "    <Quantity>" + (string.IsNullOrEmpty(model.Quantity) ? "" : model.Quantity) + "</Quantity>";
                MedicationXml += "    <QuantityUnit>" + (string.IsNullOrEmpty(model.QuantityUnitName) ? "" : model.QuantityUnitName) + "</QuantityUnit>";
                MedicationXml += "    <Refills>" + (string.IsNullOrEmpty(model.Refill) ? "" : (model.Refill == "none" ? "" : model.Refill)) + "</Refills>";
                MedicationXml += "    <SubstitutionPermitted>" + (string.IsNullOrEmpty(model.DirectionsToPharmacist) ? "y" : model.DirectionsToPharmacist) + "</SubstitutionPermitted>";
                MedicationXml += "    <OtherNotes>" + (string.IsNullOrEmpty(model.PrescriptionsOtherNotes) ? "" : model.PrescriptionsOtherNotes) + "</OtherNotes>";
                MedicationXml += "    <PatientNotes>" + (string.IsNullOrEmpty(model.AddDirectionToPatient) ? "" : model.AddDirectionToPatient) + "</PatientNotes>";
                MedicationXml += "    <Comments>" + (string.IsNullOrEmpty(model.Comments) ? "" : model.Comments) + "</Comments>";
                MedicationXml += "</Sig>";
                MedicationXml += "<StartDate>" + DateTime.Now.ToShortDateString() + "</StartDate>";
                MedicationXml += "<StopDate></StopDate>";
                MedicationXml += "<StopReason></StopReason>";
                MedicationXml += "</Medication>";
            }
            MedicationXml += "</MedicationList>";
            MedicationXml += "</Request>";
            MedicationXml += "</RCExtRequest>";
            MedicationXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + MedicationXml;
            return MedicationXml;
        }

        public static string GetXmlForDeleteProblem(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, string PatientID, string ProblemListID, string description, string StartDate)
        {
            var getcode = description.Split('-');
            var Description = "";
            var Code = "";
            if (getcode.Count() > 1)
            {
                Code = getcode[1];
            }
            for (int i = 2; i < getcode.Count(); i++)
            {
                Description += getcode[i];
            }
            string ProblemXml = "<Command>send_problem</Command>		<ProblemList>			<Problem>				<Deleted>y</Deleted>				<Status><Deleted/></Status>				<OnsetDate>" + StartDate + "</OnsetDate>				<RcopiaID></RcopiaID>				<ExternalID>" + ProblemListID + "</ExternalID>				<Patient>				    <RcopiaID></RcopiaID>				     <ExternalID>" + PatientID + "</ExternalID>				</Patient>				<ICD10>					<Code>" + Code + "</Code>					<Description>" + Description + "</Description>				</ICD10>			</Problem>		</ProblemList>        </Request></RCExtRequest>";
            ProblemXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + ProblemXml;
            return ProblemXml;
        }
        public static string GetXmlForDownloadAllergy(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, string PatientID, string AllergyLastUpdateDate)
        {
            string DownloadAlergiData = " <Command>update_allergy</Command>        <LastUpdateDate>" + AllergyLastUpdateDate + "</LastUpdateDate>              <ReturnAllNDCIDs>y</ReturnAllNDCIDs>            <Patient>                   <RcopiaID></RcopiaID>                   <ExternalID>" + PatientID + "</ExternalID>          </Patient>    </Request></RCExtRequest>";
            DownloadAlergiData = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + DownloadAlergiData;
            return DownloadAlergiData;
        }
        public static string GetXmlForDownloadMedication(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, string PatientID, string MedicationLastUpdateDate, string MedicationLastUpdateDateForLIMP)
        {
            string DownloadMedicationData = "";
            if (MedicationLastUpdateDateForLIMP == "")
            {
                DownloadMedicationData = "         <Command>update_medication</Command>		<LastUpdateDate>" + MedicationLastUpdateDate + "</LastUpdateDate>            <Patient>                <RcopiaID></RcopiaID>                <ExternalID>" + PatientID + "</ExternalID>            </Patient>    </Request></RCExtRequest>";
            }
            else
            {
                DownloadMedicationData = "         <Command>update_medication</Command>		<LastUpdateDate>" + MedicationLastUpdateDateForLIMP + "</LastUpdateDate>                </Request></RCExtRequest>";
            }
            DownloadMedicationData = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + DownloadMedicationData;
            return DownloadMedicationData;
        }

        public static string GetXmlForDownloadPrescription(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, string PatientID, string PrescriptionLastUpdateDate, string PrescriptionLastUpdateDateForLIMP)
        {
            string DownloadPrescriptionData = "";
            if (PrescriptionLastUpdateDateForLIMP == "")
            {
                DownloadPrescriptionData = "         <Command>update_prescription</Command>        <LastUpdateDate>" + PrescriptionLastUpdateDate + "</LastUpdateDate>        <Patient>            <RcopiaID></RcopiaID>            <ExternalID>" + PatientID + "</ExternalID>        </Patient>        <Status>all</Status>    </Request></RCExtRequest>";
            }
            else
            {
                DownloadPrescriptionData = "         <Command>update_prescription</Command>        <LastUpdateDate>" + PrescriptionLastUpdateDateForLIMP + "</LastUpdateDate>                <Status>all</Status>    </Request></RCExtRequest>";
            }

            DownloadPrescriptionData = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + DownloadPrescriptionData;
            return DownloadPrescriptionData;
        }

        public static string GetXmlForDownloadDrug(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, string DrugName)
        {
            string SearchDrug = "";
            SearchDrug = "         <Command>search_drug_detail</Command>		<Search>			<SearchString>" + DrugName + "</SearchString>		</Search>	</Request></RCExtRequest>";
            SearchDrug = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + SearchDrug;
            return SearchDrug;
        }




        public static string GetXmlForNotificationCount(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername)
        {
            string DownloadNotificationData = "<Command>get_notification_count</Command><ReturnPrescriptionIDs>y</ReturnPrescriptionIDs><Type>all</Type></Request></RCExtRequest>";
            DownloadNotificationData = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + DownloadNotificationData;
            return DownloadNotificationData;
        }
        public static string GetXMLForReviwedStatus(string VendorName, string VendorPassword, string SystemName, string RcopiaPracticeUsername, string PatientId)
        {
            string ReviewedXml = "";
            ReviewedXml = "<Command>get_review_status</Command>		<PatientList>			<Patient>				<ExternalID>" + PatientId + "</ExternalID>			</Patient> 		</PatientList>	</Request></RCExtRequest>";
            ReviewedXml = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.32'>" + GetHeadXmlForSendToDrFirst(VendorName, VendorPassword, SystemName, RcopiaPracticeUsername) + ReviewedXml;
            return ReviewedXml;
        }
        public static string GetSsoUrl(string PatientID, string StartScreen, string rcopiaportal_system_name, string rcopiapractice_user_name, string rcopiapatient_system_name, string secretkey, string UserName, string WebBrowserURL, string RcopiaScretkey)
        {


            string DrFirstDefaulturl = string.Concat(WebBrowserURL, "?");
            string strHashedPassword = string.Empty;
            string rcopia_portal_system_name = rcopiaportal_system_name;
            string rcopia_practice_user_name = rcopiapractice_user_name;

            //By Babur on 5/17/2016 - Commented below line inorder to launch DrFirst with other Physician
             string rcopia_user_external_id = "externaldoc_123";

           // string rcopia_user_external_id = WebConfigurationManager.AppSettings["rcopia_user_external_id"].ToString();

            string rcopia_user_id = UserName;


            //FIXME
            //if ((ClientConfiguration.UserName.ToUpper() == "DEMO") || (ClientConfiguration.UserName.ToUpper() == "SCOTT")) //demo user equals admin on DRFirst
            //{
            //    rcopia_user_external_id = "ext_doc_123";
            //}

            string service = "rcopia";
            string action = "login";
            string startup_screen = StartScreen;
            string rcopia_patient_system_name = rcopiapatient_system_name;
            string rcopia_patient_external_id = PatientID;
            string skip_auth = "n";
            string secret_key = RcopiaScretkey;
            string close_window = "n";
            string allow_popup_screens = "n";
            DateTime dateAndTime = DateTime.Now;
            dateAndTime.ToUniversalTime();
            string date = DateTime.UtcNow.ToString("MM-dd-yyHH:mm:ss");
            string formatedatetime = date.Replace(@"-", "");
            string finaltime = formatedatetime.Replace(@":", "");
            string limp_mode = "y";
            string Macinitialstring = string.Empty;
            string initialString = string.Empty;
            string initialStringWithoutPatient = string.Empty;
            string DrfirstFinalUrl = string.Empty;


            if (startup_screen == "message")
            {
                //if (WebBrowserURL == "rx301.drfirst.com/sso/portalServices")
                //{
                //    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "" + secret_key + "";
                //    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "";
                //}
                //else
                //{
                //    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "" + secret_key + "";
                //    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "";
                //}

                if (WebBrowserURL == "https://web2.drfirst.com/sso/portalServices")
                {
                    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode  + "&time=" + finaltime + "" + secret_key + "";
                    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&time=" + finaltime + "";
                }
                else
                {
                    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&time=" + finaltime + "" + secret_key + "";
                    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&time=" + finaltime + "";
                }
                using (MD5 md5 = MD5.Create())
                {
                    strHashedPassword = md5.Hash(Macinitialstring);

                }
                string MACUppercase = strHashedPassword.ToUpper();
                initialStringWithoutPatient += "&MAC=" + MACUppercase;
                DrfirstFinalUrl = DrFirstDefaulturl + initialStringWithoutPatient;
            }
            else if (startup_screen == "report")
            {
                //if (WebBrowserURL == "https://rx301.drfirst.com/sso/portalServices")
                //{
                //    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "" + secret_key + "";
                //    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "";
                //}
                //else
                //{
                //    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "" + secret_key + "";
                //    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&skip_auth=" + skip_auth + "&time=" + finaltime + "";
                //}
                if (WebBrowserURL == "https://web2.drfirst.com/sso/portalServices")
                {
                    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&time=" + finaltime + "" + secret_key + "";
                    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&time=" + finaltime + "";
                }
                else
                {
                    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode  + "&time=" + finaltime + "" + secret_key + "";
                    initialStringWithoutPatient = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&allow_popup_screens=" + allow_popup_screens + "&limp_mode=" + limp_mode + "&time=" + finaltime + "";
                }
                using (MD5 md5 = MD5.Create())
                {
                    strHashedPassword = md5.Hash(Macinitialstring);

                }
                string MACUppercase = strHashedPassword.ToUpper();
                initialStringWithoutPatient += "&MAC=" + MACUppercase;
                DrfirstFinalUrl = DrFirstDefaulturl + initialStringWithoutPatient;
            }
            else
            {
                //if (WebBrowserURL == "https://rx301.drfirst.com/sso/portalServices")
                //{
                //    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens + "&skip_auth=" + skip_auth + "&time=" + finaltime + "" + secret_key + "";
                //    initialString = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens + "&skip_auth=" + skip_auth + "&time=" + finaltime + "";
                //}
                //else
                //{
                //    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens + "&skip_auth=" + skip_auth + "&time=" + finaltime + "" + secret_key + "";
                //    initialString = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens + "&skip_auth=" + skip_auth + "&time=" + finaltime + "";
                //}
                if (WebBrowserURL == "https://web2.drfirst.com/sso/portalServices")
                {
                    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens  + "&time=" + finaltime + "" + secret_key + "";
                    initialString = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_id=" + rcopia_user_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens + "&time=" + finaltime + "";
                }
                else
                {
                    Macinitialstring = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens + "&time=" + finaltime + "" + secret_key + "";
                    initialString = "rcopia_portal_system_name=" + rcopia_portal_system_name + "&rcopia_practice_user_name=" + rcopia_practice_user_name + "&rcopia_user_external_id=" + rcopia_user_external_id + "&service=" + service + "&action=" + action + "&startup_screen=" + startup_screen + "&rcopia_patient_system_name=" + rcopia_patient_system_name + "&rcopia_patient_external_id=" + rcopia_patient_external_id + "&allow_popup_screens=" + allow_popup_screens + "&time=" + finaltime + "";
                }

                using (MD5 md5 = MD5.Create())
                {
                    strHashedPassword = md5.Hash(Macinitialstring);

                }
                string MACUppercase = strHashedPassword.ToUpper();
                initialString += "&MAC=" + MACUppercase;
                DrfirstFinalUrl = DrFirstDefaulturl + initialString;
            }

            return DrfirstFinalUrl;
        }

        #endregion

        #region DBNULL

        public static string CheckStringNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return obj.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckStringNull", ex);
            }
        }

        public static int CheckIntegerNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if ((obj == DBNull.Value) || (obj is string && string.IsNullOrEmpty((string)obj)))
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToInt32(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckIntegerNull", ex);
            }
        }

        public static byte CheckByteNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if ((obj == DBNull.Value) || (obj is string && string.IsNullOrEmpty((string)obj)))
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToByte(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckIntegerNull", ex);
            }
        }

        public static long CheckLongNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToInt64(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckLongNull", ex);
            }
        }

        public static double CheckDoubleNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToDouble(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckDoubleNull", ex);
            }
        }

        public static bool CheckBooleanNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return false;
                    }
                    else
                    {
                        return System.Convert.ToBoolean(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckDoubleNull", ex);
            }
        }

        public static short CheckShortNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return ((short)(obj));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckIntegerNull", ex);
            }
        }

        public static string CheckDateTimeNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return "";
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return "";
                    }
                    else
                    {
                        return (Convert.ToDateTime(obj)).ToString();
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("CheckDateTimeNull", ex);
            }
        }

        public static Decimal CheckDecimalNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToDecimal(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckDecimalNull", ex);
            }
        }

        public static float CheckfloatNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return float.Parse(obj.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckfloatNull", ex);
            }
        }

        public static byte[] CheckImageNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return (System.Byte[])(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckfloatNull", ex);
            }
        }


        #endregion

        #region FUNCTIONS FROM WEB LAYER start

        public enum NumbersType
        {
            TelephoneNumber = 1,
            SSN = 2,
        }

        public static string GetFormatedNumber(long Number, NumbersType Type)
        {
            try
            {
                if (Number > 0)
                {
                    switch (Type)
                    {
                        case NumbersType.TelephoneNumber:
                            return String.Format("{0:(000) 000-0000}", Number);
                        case NumbersType.SSN:
                            return String.Format("{0:000-00-0000}", Number);
                        default:
                            return MDVUtility.ToStr(Number);
                    }
                }
                else
                    return string.Empty;

            }
            catch (Exception)
            {
                return MDVUtility.ToStr(Number);
            }

        }

        public static string GetSimpleNumber(string Number)
        {
            try
            {
                if (!string.IsNullOrEmpty(Number))
                    return Number.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
                else
                    return Number;
            }
            catch (Exception)
            {
                return Number;
            }

        }
        public static string ConvertHashIntoSpace(string value)
        {
            return value.Replace("#", " ");
        }

        public static string ConvertSpaceIntoHash(string value)
        {
            return value.Replace(" ", "#");
        }

        public static string AddWaterMarkOnImage(string originalImage, string waterMarkText)
        {
            Graphics g = null;
            string base64String = "";

            try
            {
                byte[] data = Convert.FromBase64String(originalImage);
                using (var stream = new MemoryStream(data, 0, data.Length))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                    System.Drawing.Font font = new System.Drawing.Font("verdana", 12, FontStyle.Bold, GraphicsUnit.Pixel);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    //choose color and transparency
                    Color color = Color.FromArgb(147, 147, 147);

                    SolidBrush brush = new SolidBrush(color);

                    //draw text on image
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, 0, image.Size.Width, image.Size.Height);

                        graphics.DrawString(waterMarkText, font, brush, rectangle, stringFormat);
                        graphics.Dispose();
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream, image.RawFormat);
                        byte[] imageBytes = memoryStream.ToArray();

                        // Convert byte[] to Base64 String
                        base64String = Convert.ToBase64String(imageBytes);
                    }

                    // Following line to test Rendered image by saving on disk
                    //image.Save("E:\\File.jpeg", ImageFormat.Jpeg);

                    return base64String;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //if (image != null)
                //    image.Dispose();

                if (g != null)
                    g.Dispose();
            }

            return base64String;
        }

        #endregion
        
        #region Unit Conversion


        /// <summary>
        /// Converts the feet to inches in such a way that fractional part is considered as literal inches
        /// e.g., 4.5 feet is 4feet and 5 inches instead of 4feet and 6 inches
        /// </summary>
        /// <param name="feet"></param>
        /// <returns>total number of inches according to avove mentioned assumption</returns>
        public static double convertFeetToInches(string feet)
        {
            double inches = 0;
            double wholePart;
            double fractionalPart = 0;
            double totalFeet = 0;
            totalFeet = MDVUtility.Tofloat(feet);

            wholePart = Math.Floor(totalFeet);
            fractionalPart = totalFeet - wholePart;

            inches = (wholePart * 12) + Math.Round(fractionalPart * 10);

            return inches;

        }




        #endregion

        #region Legacy Notes

        public partial class PDFFooterHeader : PdfPageEventHelper
        {
            PdfContentByte cb;

            PdfTemplate headerTemplate, footerTemplate;

            BaseFont bf = null;

            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            public PdfPTable ReportHeaderTable;
            public bool IsFooterExist { get; set; }
            public string FooterGeneratedBy { get; set; }
            public PdfPTable ReportFooterTable = null;
            #endregion


            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    footerTemplate = cb.CreateTemplate(100, 100);
                }
                catch (DocumentException de)
                { }
                catch (System.IO.IOException ioe)
                { }
            }

            public PDFFooterHeader(PdfPTable _ReportHeaderTable, bool _IsFooterExist, string _FooterGeneratedBy, PdfPTable _FooterTable)
            {
                ReportHeaderTable = _ReportHeaderTable;
                FooterGeneratedBy = _FooterGeneratedBy;
                IsFooterExist = _IsFooterExist;
                ReportFooterTable = _FooterTable;
            }
            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                base.OnEndPage(writer, doc);
                #region adding footer
                if (IsFooterExist)
                {
                    if (this.ReportFooterTable != null && this.ReportFooterTable.Rows.Count > 0)
                    {
                        this.ReportFooterTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        this.ReportFooterTable.WriteSelectedRows(0, -1, 10, doc.PageSize.GetBottom(30), writer.DirectContent);
                    }
                }
                cb.AddTemplate(footerTemplate, 10, doc.PageSize.GetBottom(30));
                #endregion

                #region adding header
                if (writer.PageNumber > 1)
                {
                    if (this.ReportHeaderTable != null && this.ReportHeaderTable.Rows.Count > 0)
                    {
                        this.ReportHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        this.ReportHeaderTable.WriteSelectedRows(0, -1, 10, doc.PageSize.Height - 5, writer.DirectContent);
                    }
                }
                #endregion

            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
                footerTemplate.BeginText();
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                footerTemplate.SetFontAndSize(bf, 20);
                footerTemplate.SetTextMatrix(document.PageSize.GetRight(140), 0);
                footerTemplate.ShowText((writer.PageNumber).ToString());
                footerTemplate.EndText();
            }
        }

        #endregion Legacy Notes

        #region patient
        public static string GetAge(DateTime birthday)
        {
            try
            {
                //int age = ((DateTime.Now.Year - birthday.Year) * 372 + (DateTime.Now.Month - birthday.Month) * 31 + (DateTime.Now.Day - birthday.Day)) / 372;

                //int age2 = Convert.ToInt32(Math.Round(today.Subtract(birthday).TotalDays * 0.00273790926));

                DateTime today = DateTime.Now;
                //TimeSpan ts = today - birthday;
                //int Age3 = ts.Days / 365;
                // DateTime Age = DateTime.MinValue.AddDays(ts.Days);

                int days = today.Day - birthday.Day;
                if (days < 0)
                {
                    today = today.AddMonths(-1);
                    days += DateTime.DaysInMonth(today.Year, today.Month);
                }

                int months = today.Month - birthday.Month;
                if (months < 0)
                {
                    today = today.AddYears(-1);
                    months += 12;
                }

                int years = today.Year - birthday.Year;


                string ActAge = string.Format(" {0} Year(s), {1} Month(s), {2} Day(s)  ", years,
                             months,
                             days);

                var response = new
                {
                    status = true,
                    ActualAge = ActAge
                };
               return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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


    }
}
