using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDVision.Model.Common
{
    public class ModelUtility
    {
        public static List<string> GetReadersColumnList(IDataReader reader)
        {

            List<string> incommingColumnList = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var t = reader.GetFieldType(i);
                incommingColumnList.Add(reader.GetName(i));
            }
            return incommingColumnList;
        }

        public static object MapValue(IDataReader reader, string columnName, List<string> incommingColumnList)
        {
            if (incommingColumnList.Contains(columnName))
            {
                return reader[columnName];
            }
            else
            {
                return null;
            }
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

            return DateTime.TryParse(ModelUtility.ToStr(val), out dt) ? dt : (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        }

        public static string ToStr(Object val, string defaultValue = "")
        {
            return val == null ? defaultValue : @val.ToString();
        }

        public static int ToInt(Object val)
        {
            int i;
            return int.TryParse(ModelUtility.ToStr(val), out i) ? i : 0;
        }

        public static Int16 ToInt16(Object val)
        {
            Int16 i;
            return Int16.TryParse(ModelUtility.ToStr(val), out i) ? i : Convert.ToInt16(0);
        }
        public static Int32 ToInt32(Object val)
        {
            Int32 i;
            return Int32.TryParse(ModelUtility.ToStr(val), out i) ? i : 0;
        }
        public static Int64 ToInt64(Object val)
        {
            Int64 i;
            return Int64.TryParse(ModelUtility.ToStr(val), out i) ? i : Convert.ToInt64(0);
        }

        public static Double ToDouble(Object val)
        {
            Double dbl;
            return Double.TryParse(ModelUtility.ToStr(val), out dbl) ? dbl : 0.0;
        }
        public static float Tofloat(Object val)
        {
            float fl;
            return float.TryParse(ModelUtility.ToStr(val), out fl) ? fl : (float)0.0;
        }
        public static Decimal ToDecimal(Object val)
        {
            Decimal dec;
            return Decimal.TryParse(ModelUtility.ToStr(val), out dec) ? dec : (Decimal)0.0;
        }
        public static Boolean IsNumeric(Object val)
        {
            Double i = 0;
            if (Double.TryParse(ModelUtility.ToStr(val), out i))
            {
                return true;
            }
            return false;
        }
        public static Boolean ToBool(Object val)
        {
            bool b;
            return bool.TryParse(ModelUtility.ToStr(val), out b) ? b : false;
        }
        public static string ToBoolString(Object val)
        {
            string[] tureValues = {"1","true","True","TRUE" };
            string valStr = ModelUtility.ToStr(val);
            if(valStr.Length == 0)
            {
                return "";
            }
            else if(tureValues.Contains(valStr))
            {
                return "True";
            }

            return "False";
        }

        public static string ToByteString(Object val)
        {
            if (ModelUtility.ToBool(val))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public static byte ToByte(Object val)
        {
            byte i;
            return byte.TryParse(ModelUtility.ToStr(val), out i) ? i : (byte)(0);
        }

    }
}
