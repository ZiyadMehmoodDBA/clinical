using MDVision.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MDVision.DataAccess.Helpers
{
    public class DALHelper
    {


        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in dt.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
            //// Example 1:
            //// Get private fields + non properties
            ////var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            //// Example 2: Your case
            //// Get all public fields
            //var fields = typeof(T).GetFields();

            //List<T> lst = new List<T>();

            //foreach (DataRow dr in dt.Rows)
            //{
            //    // Create the object of T
            //    var ob = Activator.CreateInstance<T>();

            //    foreach (var fieldInfo in fields)
            //    {
            //        foreach (DataColumn dc in dt.Columns)
            //        {
            //            // Matching the columns with fields
            //            if (fieldInfo.Name == dc.ColumnName)
            //            {
            //                // Get the value from the datatable cell
            //                object value = dr[dc.ColumnName];

            //                // Set the value into the object
            //                fieldInfo.SetValue(ob, value);
            //                break;
            //            }
            //        }
            //    }

            //    lst.Add(ob);
            //}

            //return lst;
        }


        public static List<MDVision.Model.Billing.PatientStatement.StatementDetail> DataTableStatementDetail<T>(DataTable dt) where T : class, new()
        {
            List<MDVision.Model.Billing.PatientStatement.StatementDetail> StatementDetailList = new List<MDVision.Model.Billing.PatientStatement.StatementDetail>();
            StatementDetailList = (from DataRow dr in dt.Rows
                           select new MDVision.Model.Billing.PatientStatement.StatementDetail()
                           {
                               //PatientId	ChargeCapId	Charges	Age	Procedure	Description	Date	PatBalance	Paid	LedgerType	
                               //PrintOnPatStmt	InsBalance	PlanPriority	NxtPlanPriority	LedgerAccountId	LedgerDesc

                               PatientId = MDVUtility.ToInt64(dr["PatientId"]),
                               Charges = MDVUtility.ToDecimal(dr["Charges"]),
                               ChargeCapId = MDVUtility.ToInt64(dr["ChargeCapId"]),
                               Procedure = dr["Procedure"].ToString(),
                               Description = dr["Description"].ToString(),
                               //Date = MDVUtility.ToDateTime(dr["Date"].ToString()),
                               PatBalance = MDVUtility.ToDecimal(dr["PatBalance"].ToString()),
                               Paid = MDVUtility.ToDecimal(dr["Paid"].ToString()),
                               //Age = MDVUtility.ToInt32(dr["Age"]),
                               //PrintOnPatStmt = dr["PrintOnPatStmt"].ToString(),
                               //ClaimNumber = dr["StudentName"].ToString(),
                               //Units = dr["StudentName"].ToString(),
                               //ChargeProviderId = dr["StudentName"].ToString(),
                               LedgerType = dr["LedgerType"].ToString(),
                               //FullName = dr["FullName"].ToString(),
                               PlanPriority = dr["PlanPriority"].ToString(),
                               //ChargeFacilityId = dr["StudentName"].ToString(),
                               NxtPlanPriority = dr["NxtPlanPriority"].ToString(),
                               LedgerAccountId = MDVUtility.ToInt32(dr["LedgerAccountId"]),

                           }).ToList();
            return StatementDetailList;
        }

        public static List<MDVision.Model.Billing.PatientStatement.StatementHeader> DataTableStatementHeader<T>(DataTable dt) where T : class, new()
        {
            List<MDVision.Model.Billing.PatientStatement.StatementHeader> StatementHeaderList = new List<MDVision.Model.Billing.PatientStatement.StatementHeader>();
            StatementHeaderList = (from DataRow dr in dt.Rows
                                   select new MDVision.Model.Billing.PatientStatement.StatementHeader()
                                   { 
                                       PatientId = MDVUtility.ToInt64(dr["PatientId"]),
                                       Charges = MDVUtility.Tofloat(dr["Charges"]),
                                       ChargeCapId = MDVUtility.ToInt64(dr["ChargeCapId"]),
                                       Procedure = dr["Procedure"].ToString(),
                                       Description = dr["Description"].ToString(),
                                       PatBalance = MDVUtility.Tofloat(dr["PatBalance"]),
                                       Age = MDVUtility.ToInt32(dr["Age"]),
                                       AccountNumber = dr["AccountNumber"].ToString(),
                                       FirstName = dr["FirstName"].ToString(),
                                       LastName = dr["LastName"].ToString(),
                                       FullName = dr["FullName"].ToString(),
                                       Address1 = dr["Address1"].ToString(),
                                       Address2 = dr["Address2"].ToString(),
                                       City = dr["City"].ToString(),
                                       State = dr["State"].ToString(),
                                       ZipCode = dr["ZipCode"].ToString(),
                                       ZipCodeExt = dr["ZipCodeExt"].ToString(),

                                       FacilityName = dr["FacilityName"].ToString(),
                                       FacilityAddress = dr["FacilityAddress"].ToString(),
                                       FacilityCity = dr["FacilityCity"].ToString(),
                                       FacilityState = dr["FacilityState"].ToString(),
                                       FacilityZipCode = dr["FacilityZipCode"].ToString(),
                                       FacilityZipCodeExt = dr["FacilityZipCodeExt"].ToString(),
                                       FacilityId = MDVUtility.ToInt64(dr["FacilityId"]),
                                       FacilityDescription = dr["FacilityDescription"].ToString(),

                                       LetterId = MDVUtility.ToInt64(dr["LetterId"]),
                                       LetterName = dr["LetterName"].ToString(),

                                       Message = dr["Message"].ToString(),
                                       FromName = dr["FromName"].ToString(),
                                       FromAddress = dr["FromAddress"].ToString(),
                                       FromCity = dr["FromCity"].ToString(),
                                       FromState = dr["FromState"].ToString(),
                                       FromZip = dr["FromZip"].ToString(),
                                       FromZipExt = dr["FromZipExt"].ToString(),

                                       RemitToName = dr["RemitToName"].ToString(),
                                       RemitToAddress = dr["RemitToAddress"].ToString(),
                                       RemitToCity = dr["RemitToCity"].ToString(),
                                       RemitToState = dr["RemitToState"].ToString(),
                                       RemitToZip = dr["RemitToZip"].ToString(),
                                       RemitToZipExt = dr["RemitToZipExt"].ToString(),

                                       OfcHoursFrom = dr["OfcHoursFrom"].ToString(),
                                       OfcHoursTo = dr["OfcHoursTo"].ToString(),
                                       PhoneNo = dr["phoneNo"].ToString(),

                                       InsBalance = MDVUtility.ToDecimal(dr["InsBalance"].ToString()),
                                       Date = MDVUtility.ToDateTime(dr["Date"].ToString()),
                                       //FirstMsgId = MDVUtility.ToInt32(dr["FirstMsgId"]),
                                       //FirstMessage = dr["FirstMessage"].ToString(),
                                       //SecondMsgId = MDVUtility.ToInt32(dr["SecondMsgId"]),
                                       //SecondMessage = dr["SecondMessage"].ToString(),
                                       //ThirdMsgId = MDVUtility.ToInt32(dr["ThirdMsgId"]),
                                       //ThirdMessage = dr["ThirdMessage"].ToString(),
                                       //FourthMsgId = MDVUtility.ToInt32(dr["FourthMsgId"]),
                                       //FourthMessage = dr["FourthMessage"].ToString(),
                                       //FifthMsgId = MDVUtility.ToInt32(dr["FifthMsgId"]),
                                       //FifthMessage = dr["FifthMessage"].ToString(),

                                   }).ToList();
            return StatementHeaderList;
        }



        public static DataTable ToDataTable<T>(List<T> items)
            {

                DataTable dataTable = new DataTable(typeof(T).Name);

                //Get all the properties

                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in Props)
                {

                    //Setting column names as Property names

                    dataTable.Columns.Add(prop.Name);

                }

                foreach (T item in items)
                {

                    var values = new object[Props.Length];

                    for (int i = 0; i < Props.Length; i++)
                    {

                        //inserting property values to datatable rows

                        values[i] = Props[i].GetValue(item, null);

                    }

                    dataTable.Rows.Add(values);

                }

                //put a breakpoint here and check datatable

                return dataTable;

            }

        

    }
}
