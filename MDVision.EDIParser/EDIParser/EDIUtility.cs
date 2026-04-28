namespace EDIParser
{
    using System;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;
    using MDVision.Common.Utilities;

    public class EDIUtility
    {
        //public static char D_ELMT = '*';
        //public static char D_S_ELMT = ':';
        //public static char D_SGMT = '~';

        //public static string DateToString(DateTime d)
        //{
        //    string str3;
        //    try
        //    {
        //        string str = null;
        //        string str2 = null;
        //        str = d.Year.ToString();
        //        str2 = str2 + str;
        //        str = d.Month.ToString();
        //        if (Convert.ToInt32(str) < 10)
        //        {
        //            str = "0" + str;
        //        }
        //        str2 = str2 + str;
        //        str = d.Day.ToString();
        //        if (Convert.ToInt32(str) < 10)
        //        {
        //            str = "0" + str;
        //        }
        //        str2 = str2 + str;
        //        str = d.Hour.ToString();
        //        if (Convert.ToInt32(str) < 10)
        //        {
        //            str = "0" + str;
        //        }
        //        str2 = str2 + str;
        //        str = d.Minute.ToString();
        //        if (Convert.ToInt32(str) < 10)
        //        {
        //            str = "0" + str;
        //        }
        //        str2 = str2 + str;
        //        str = d.Second.ToString();
        //        if (Convert.ToInt32(str) < 10)
        //        {
        //            str = "0" + str;
        //        }
        //        str3 = str2 + str;
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //    return str3;
        //}

        //public static DataSet DecryptDataSet(string EncrptedXmlDataSet, ref DataSet ds)
        //{
        //    DataSet set;
        //    try
        //    {
        //        if (EncrptedXmlDataSet != null)
        //        {
        //            XmlTextReader reader = new XmlTextReader(new StringReader(DecryptFrom64(EncrptedXmlDataSet)));
        //            ds.ReadXml(reader, XmlReadMode.DiffGram);
        //        }
        //        set = ds;
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //    return set;
        //}

        //public static string DecryptFrom64(string encodedData)
        //{
        //    byte[] bytes = Convert.FromBase64String(encodedData);
        //    return Encoding.ASCII.GetString(bytes);
        //}

        //public static string EncryptDataSet(ref DataSet ds)
        //{
        //    string str;
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
        //        ds.WriteXml(writer, XmlWriteMode.DiffGram);
        //        str = EncryptTo64(sb.ToString());
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //    return str;
        //}

        //public static string EncryptTo64(string toEncode)
        //{
        //    return Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));
        //}

        //public static string Get12HoursTime(string hr)
        //{
        //    if (ToInt(hr) == 0)
        //    {
        //        return "12 am";
        //    }
        //    if (ToInt(hr) > 12)
        //    {
        //        return ((ToInt(hr) - 12) + " pm");
        //    }
        //    if (ToInt(hr) == 12)
        //    {
        //        return (12 + " pm");
        //    }
        //    return (hr + " am");
        //}

        //public static string Get24HourTime(int hour, int minute, string ToD)
        //{
        //    int year = DateTime.Now.Year;
        //    int month = DateTime.Now.Month;
        //    int day = DateTime.Now.Day;
        //    if (ToD.ToUpper() == "PM")
        //    {
        //        hour = (hour % 12) + 12;
        //    }
        //    if (hour == 0x18)
        //    {
        //        hour = 0x17;
        //        minute = 0x3b;
        //    }
        //    DateTime time = new DateTime(year, month, day, hour, minute, 0);
        //    return time.ToString("HHmm");
        //}

        //public static object[] GetArrayFromCommaList(string commaList)
        //{
        //    if ((commaList == null) || (commaList == ""))
        //    {
        //        return new object[0];
        //    }
        //    string[] strArray = commaList.Split(new char[] { ',' });
        //    object[] objArray = new object[strArray.Length];
        //    int num = 0;
        //    foreach (string str in strArray)
        //    {
        //        objArray[num++] = str.Trim();
        //    }
        //    return objArray;
        //}

        //public static string GetCommaListFromArray(object[] arr)
        //{
        //    string str = "";
        //    foreach (object obj2 in arr)
        //    {
        //        str = str + obj2.ToString() + ",";
        //    }
        //    return str.TrimEnd(new char[] { ',' });
        //}

        //public static string GetDateDDMMYYY(string dt)
        //{
        //    if (dt == "")
        //    {
        //        return "";
        //    }
        //    dt = dt.Contains(" ") ? dt.Substring(0, dt.IndexOf(" ")) : dt;
        //    dt = dt.Trim();
        //    string str = LPad(dt.Substring(0, dt.IndexOf("/")), "0", 2);
        //    string str2 = LPad(dt.Substring(dt.IndexOf("/") + 1, 2).Replace("/", ""), "0", 2);
        //    string str3 = LPad(dt.Substring(dt.LastIndexOf("/") + 1), "0", 4);
        //    if (Convert.ToInt32(str2) > 12)
        //    {
        //        return (str2 + "/" + str + "/" + str3);
        //    }
        //    return (str + "/" + str2 + "/" + str3);
        //}

        //public static DateTime GetDateFromNumber(string number)
        //{
        //    number = number.Replace(" ", "");
        //    string str = number;
        //    if (number.Length >= 6)
        //    {
        //        str = number.Substring(4, 2).Trim() + "/" + number.Substring(6, 2).Trim() + "/" + number.Substring(0, 4).Trim();
        //    }
        //    return Convert.ToDateTime(str);
        //}

        //public static string GetDateMMDDYYY(string dt)
        //{
        //    if (dt == "")
        //    {
        //        return "";
        //    }
        //    dt = dt.Contains(" ") ? dt.Substring(0, dt.IndexOf(" ")) : dt;
        //    dt = dt.Trim();
        //    string str = LPad(dt.Substring(0, dt.IndexOf("/")), "0", 2);
        //    string str2 = LPad(dt.Substring(dt.IndexOf("/") + 1, 2).Replace("/", ""), "0", 2);
        //    string str3 = LPad(dt.Substring(dt.LastIndexOf("/") + 1), "0", 4);
        //    if (Convert.ToInt32(str) > 12)
        //    {
        //        return (str2 + "/" + str + "/" + str3);
        //    }
        //    return (str + "/" + str2 + "/" + str3);
        //}

        //public static string GetDisplay(bool val)
        //{
        //    return (val ? "" : "none");
        //}

        //public static string GetFormattedBool(bool val, string trueText, string falseText)
        //{
        //    return (val ? trueText : falseText);
        //}

        //private static string GetNum(string Str)
        //{
        //    if (Str == null)
        //    {
        //        return string.Empty;
        //    }
        //    StringBuilder builder = new StringBuilder();
        //    char[] chArray = null;
        //    chArray = Str.ToCharArray();
        //    foreach (char ch in chArray)
        //    {
        //        if (char.IsNumber(ch))
        //        {
        //            builder.Append(ch);
        //        }
        //    }
        //    return builder.ToString();
        //}

        //public static string GetTimeFromNumbers(string t)
        //{
        //    t = t.Trim();
        //    if (t.Length == 5)
        //    {
        //        t = t.Substring(0, 1) + ":" + t.Substring(1, 2) + ":" + t.Substring(3, 2);
        //        return t;
        //    }
        //    if (t.Length == 6)
        //    {
        //        t = t.Substring(0, 2) + ":" + t.Substring(2, 2) + ":" + t.Substring(4, 2);
        //    }
        //    return t;
        //}

        //public static string GetUniqueCode()
        //{
        //    DateTime utcNow = DateTime.UtcNow;
        //    string str = LPad(utcNow.Day.ToString(), "0", 2);
        //    string str2 = LPad(utcNow.Month.ToString(), "0", 2);
        //    string str3 = utcNow.Year.ToString();
        //    string str4 = LPad(utcNow.Hour.ToString(), "0", 2);
        //    string str5 = LPad(utcNow.Minute.ToString(), "0", 2);
        //    string str6 = LPad(utcNow.Second.ToString(), "0", 2);
        //    return (str3 + str2 + str + str4 + str5 + str6);
        //}

        //public static string GetUniqueCode_Short()
        //{
        //    DateTime utcNow = DateTime.UtcNow;
        //    string str = LPad(utcNow.Day.ToString(), "0", 2);
        //    string str2 = LPad(utcNow.Month.ToString(), "0", 2);
        //    return (utcNow.Year.ToString() + str2 + str);
        //}

        //public static string GetVisibility(bool val)
        //{
        //    return (val ? "visible" : "hidden");
        //}

        //public static string GetYesNo(bool val)
        //{
        //    return (val ? "Yes" : "<span style='color:red'>No</span>");
        //}

        //public static bool IsDate(string value)
        //{
        //    DateTime time;
        //    return DateTime.TryParse(value, out time);
        //}

        //public static object IsNull(object val, object defaultVal)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return defaultVal;
        //    }
        //    return val;
        //}

        //public static object IsNullBool(object val)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return DBNull.Value;
        //    }
        //    return Convert.ToBoolean(val);
        //}

        //public static object IsNullBool(object val, bool defaultVal)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return defaultVal;
        //    }
        //    return Convert.ToBoolean(val);
        //}

        //public static object IsNullDate(object val)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return DBNull.Value;
        //    }
        //    return Convert.ToDateTime(val);
        //}

        //public static object IsNullDate(object val, DateTime defaultVal)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return defaultVal;
        //    }
        //    return Convert.ToDateTime(val);
        //}

        //public static object IsNullDec(object val)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return DBNull.Value;
        //    }
        //    return Convert.ToDecimal(val);
        //}

        //public static object IsNullInt(object val)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return DBNull.Value;
        //    }
        //    return Convert.ToInt32(val);
        //}

        //public static object IsNullStr(object val)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return DBNull.Value;
        //    }
        //    return val.ToString();
        //}

        //public static string IsNullStr(object val, string defaultVal)
        //{
        //    if ((val == null) || (val.ToString() == DBNull.Value.ToString()))
        //    {
        //        return defaultVal;
        //    }
        //    return val.ToString();
        //}

        //public static bool IsNumeric(object val)
        //{
        //    double result = 0.0;
        //    return double.TryParse(ToStr(val, ""), out result);
        //}

        //public static string LPad(string str, string fillChar, int digits)
        //{
        //    if (str.Length <= digits)
        //    {
        //        for (int i = 0; i < (digits - str.Length); i++)
        //        {
        //            str = fillChar + str;
        //        }
        //    }
        //    return str;
        //}

        //public static string PhoneNo(string Str)
        //{
        //    Str = MDVUtility.GetNum(Str);


        //    StringBuilder builder = new StringBuilder();
        //    if (Str.Length == 10)
        //    {
        //        builder.Append("(");
        //        builder.Append(Str.Substring(0, 3));
        //        builder.Append(")");
        //        builder.Append(Str.Substring(3, 3));
        //        builder.Append("-");
        //        builder.Append(Str.Substring(6, 4));
        //        Str = builder.ToString();
        //    }
        //    return Str;
        //}

        //public static void SetDelimiters(string StrIn)
        //{
        //    try
        //    {
        //        string str = StrIn.Substring(0, 0x6a);
        //        if (StrIn.Substring(str.Length - 1, 1).Length > 0)
        //        {
        //            D_SGMT = StrIn.Substring(str.Length - 1, 1).ToCharArray()[0];
        //        }
        //        if (StrIn.Substring(str.Length - 3, 1).Length > 0)
        //        {
        //            D_ELMT = StrIn.Substring(str.Length - 3, 1).ToCharArray()[0];
        //        }
        //        if (StrIn.Substring(str.Length - 2, 1).Length > 0)
        //        {
        //            D_S_ELMT = StrIn.Substring(str.Length - 2, 1).ToCharArray()[0];
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        //public static DateTime StringToDate(string StrDate)
        //{
        //    try
        //    {
        //        if (StrDate.Length == 8)
        //        {
        //            StrDate = StrDate.PadRight(14, '0');
        //        }
        //        if (StrDate.Length == 14)
        //        {
        //            if (!MDVUtility.IsNumeric(StrDate))
        //            {
        //                throw new Exception("String format is not numeric.");
        //            }
        //            int year = 0;
        //            int month = 0;
        //            int day = 0;
        //            int hour = 0;
        //            int minute = 0;
        //            year = Convert.ToInt32(StrDate.Substring(0, 4));
        //            month = Convert.ToInt32(StrDate.Substring(4, 2));
        //            day = Convert.ToInt32(StrDate.Substring(6, 2));
        //            hour = Convert.ToInt32(StrDate.Substring(8, 2));
        //            minute = Convert.ToInt32(StrDate.Substring(10, 2));
        //            return new DateTime(year, month, day, hour, minute, Convert.ToInt32(StrDate.Substring(12, 2)));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return DateTime.Now;
        //}

        //public static DateTime StringToTime(string Str24Time)
        //{
        //    try
        //    {
        //        int year = DateTime.Now.Year;
        //        int month = DateTime.Now.Month;
        //        int day = DateTime.Now.Day;
        //        int hour = 0;
        //        int minute = 0;
        //        if (Str24Time.Length == 4)
        //        {
        //            hour = Convert.ToInt32(Str24Time.Substring(0, 2));
        //            minute = Convert.ToInt32(Str24Time.Substring(2, 2));
        //        }
        //        return new DateTime(year, month, day, hour, minute, 0);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return DateTime.Now;
        //}

        //public static byte ToByte(object val)
        //{
        //    byte num;
        //    return (byte.TryParse(ToStr(val, ""), out num) ? num : ((byte) 0));
        //}

        //public static byte ToByte(object val, byte alternateVal)
        //{
        //    byte num;
        //    return (byte.TryParse(ToStr(val, ""), out num) ? num : alternateVal);
        //}

        //public static int ToConvertInt32(object val)
        //{
        //    int num;
        //    return (int.TryParse(ToStr(val, ""), out num) ? num : 0);
        //}

        //public static DateTime ToDateTime(object val)
        //{
        //    DateTime time;
        //    return (DateTime.TryParse(val.ToString(), out time) ? time : DateTime.MinValue);
        //}

        //public static DateTime ToDateTime(object val, DateTime alternateVal)
        //{
        //    DateTime time;
        //    return (DateTime.TryParse(val.ToString(), out time) ? time : alternateVal);
        //}

        //public static DateTime ToDateTime(object val, string alternateVal)
        //{
        //    DateTime time;
        //    return (DateTime.TryParse(val.ToString(), out time) ? time : Convert.ToDateTime(alternateVal));
        //}

        //public static decimal ToDecimal(object val)
        //{
        //    decimal num;
        //    return (decimal.TryParse(ToStr(val, ""), out num) ? num : 0M);
        //}

        //public static double ToDouble(object val)
        //{
        //    double num;
        //    return (double.TryParse(MDVUtility.ToStr(val, ""), out num) ? num : 0.0);
        //}

        //public static double ToDouble(object val, double alternateVal)
        //{
        //    double num;
        //    return (double.TryParse(ToStr(val, ""), out num) ? num : -1.0);
        //}

        //public static float Tofloat(object val)
        //{
        //    float num;
        //    return (float.TryParse(ToStr(val, ""), out num) ? num : 0f);
        //}

        //public static int ToInt(object val)
        //{
        //    int num;
        //    return (int.TryParse(ToStr(val, ""), out num) ? num : 0);
        //}

        //public static int ToInt(object val, int alternateVal)
        //{
        //    int num;
        //    return (int.TryParse(ToStr(val, ""), out num) ? num : alternateVal);
        //}

        //public static short ToInt16(object val)
        //{
        //    short num;
        //    return (short.TryParse(ToStr(val, ""), out num) ? num : Convert.ToInt16(0));
        //}

        //public static short ToInt16(object val, short alternateVal)
        //{
        //    short num;
        //    return (short.TryParse(ToStr(val, ""), out num) ? num : alternateVal);
        //}

        //public static int ToInt32(object val)
        //{
        //    int num;
        //    return (int.TryParse(ToStr(val, ""), out num) ? num : 0);
        //}

        //public static int ToInt32(object val, int alternateVal)
        //{
        //    int num;
        //    return (int.TryParse(ToStr(val, ""), out num) ? num : alternateVal);
        //}

        //public static long ToInt64(object val)
        //{
        //    long num;
        //    return (long.TryParse(ToStr(val, ""), out num) ? num : Convert.ToInt64(0));
        //}

        //public static long ToInt64(object val, long alternateVal)
        //{
        //    long num;
        //    return (long.TryParse(ToStr(val, ""), out num) ? num : alternateVal);
        //}

        //public static long ToLong(object val)
        //{
        //    long num;
        //    return (long.TryParse(MDVUtility.ToStr(val, ""), out num) ? num : 0L);
        //}

        //public static string ToStr(object val, string defaultValue = "")
        //{
        //    return ((val == null) ? defaultValue : val.ToString());
        //}

        //public static string TrimText(string text, int totChars)
        //{
        //    if ((text != "") && (text.Length > totChars))
        //    {
        //        return (text.Substring(0, totChars) + "...");
        //    }
        //    return text;
        //}

        //public static string WhichEDI(string StrIn)
        //{
        //    string str2;
        //    try
        //    {
        //        string[] strArray = null;
        //        string[] strArray2 = null;
        //        strArray = StrIn.Split(new char[] { D_SGMT });
        //        foreach (string str in strArray)
        //        {
        //            if (str.Substring(0, 2) == "GS")
        //            {
        //                strArray2 = str.Split(new char[] { D_ELMT });
        //                if (strArray2.Length > 0)
        //                {
        //                    string str3 = strArray2[1];
        //                    if (str3 != null)
        //                    {
        //                        if (!(str3 == "HB"))
        //                        {
        //                            if (str3 == "HP")
        //                            {
        //                                return "835";
        //                            }
        //                            if (str3 == "FA")
        //                            {
        //                                return "997";
        //                            }
        //                            if (str3 == "HN")
        //                            {
        //                                return "277";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            return "271";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        str2 = "UnknownEDI";
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //    return str2;
        //}

        //public static string ZipCode(string Str)
        //{
        //    Str =  MDVUtility.GetNum(Str);
           
        //    StringBuilder builder = new StringBuilder();
        //    if (Str.Length == 9)
        //    {
        //        builder.Append(Str.Substring(0, 5));
        //        builder.Append("-");
        //        builder.Append(Str.Substring(5, 4));
        //        Str = builder.ToString();
        //    }
        //    return Str;
        //}

        //public sealed class ControlChars
        //{
        //    public const char Back = '\b';
        //    public const char Comma = ',';
        //    public const char Cr = '\r';
        //    public const string CrLf = "\r\n";
        //    public const char FormFeed = '\f';
        //    public const char Lf = '\n';
        //    public const string NewLine = "\r\n";
        //    public const char NullChar = '\0';
        //    public const char Quote = '"';
        //    public const char Tab = '\t';
        //    public const char VerticalTab = '\v';
        //}
    }
}

