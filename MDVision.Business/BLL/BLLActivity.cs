using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDVision.Business.BLL
{
    public class BLLActivity
    {
        public static DataTable DBAudit(DataTable dt)
        {


            DataTable dtActivity = new DataTable();
            dtActivity.Columns.Add("OriginalValue");
            dtActivity.Columns.Add("CurrentValue");
            dtActivity.Columns.Add("ProfileName");
            dtActivity.Columns.Add("Field");
            dtActivity.Columns.Add("Actions");
            dtActivity.Columns.Add("KeyValue");
            dtActivity.Columns.Add("dbTableName");
            dtActivity.Columns.Add("KeyColumn");
            dtActivity.Columns.Add("ColumnDataType");



            //get data from db form table using ----    dt.TableName; 
            //get DBAudit schema
            string ColumnsName = "FirstName|First Name,LastName|Last Name"; /// get colunm name using tableName
            string Action = "I,U,D";
            string dbTableName = "Patients";
            string FormName = "Demographic";

            Dictionary<string, string> ColumnsDictionary = GetColumnsName(ColumnsName);


            foreach (DataRow row in dt.GetChanges().Rows)
            {

                if ((row.RowState == DataRowState.Added))
                {
                    if (Action.Contains('I'))
                    {
                        foreach (DataColumn col in row.Table.Columns)
                        {
                            if (ColumnsDictionary.ContainsKey(col.ColumnName))
                            {
                                DataRow drActivity = dtActivity.NewRow();
                                DataColumn[] columnsKey;
                                columnsKey = row.Table.PrimaryKey;

                                if (columnsKey.Length > 0)
                                {
                                    drActivity["KeyValue"] = row[columnsKey[0].ColumnName].ToString();
                                    drActivity["KeyColumn"] = columnsKey[0].ColumnName;
                                }
                                drActivity["ColumnDataType"] = col.DataType;
                                drActivity["OriginalValue"] = "";
                                drActivity["CurrentValue"] = row[col.ColumnName].ToString();
                                drActivity["ProfileName"] = FormName;
                                drActivity["dbTableName"] = dbTableName;
                                drActivity["Field"] = col.ColumnName;
                                drActivity["Actions"] = "Insert";
                                dtActivity.Rows.Add(drActivity);
                            }
                        }
                    }

                }
                else if ((row.RowState == DataRowState.Modified))
                {
                    if (Action.Contains('U'))
                    {
                        foreach (DataColumn col in row.Table.Columns)
                        {
                            if (ColumnsDictionary.ContainsKey(col.ColumnName) && !row[col.ColumnName].ToString().Equals(row[col.ColumnName, DataRowVersion.Original].ToString()))
                            {
                                DataColumn[] columnsKey;
                                columnsKey = row.Table.PrimaryKey;
                                DataRow drActivity = dtActivity.NewRow();


                                if (columnsKey.Length > 0)
                                {
                                    drActivity["KeyValue"] = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                    drActivity["KeyColumn"] = columnsKey[0].ColumnName;
                                }

                                drActivity["OriginalValue"] = row[col.ColumnName, DataRowVersion.Original].ToString();
                                drActivity["ColumnDataType"] = col.DataType;
                                drActivity["CurrentValue"] = row[col.ColumnName].ToString();
                                drActivity["ProfileName"] = FormName;
                                drActivity["dbTableName"] = dbTableName;
                                drActivity["Field"] = col.ColumnName;
                                drActivity["Actions"] = "Update";
                                dtActivity.Rows.Add(drActivity);
                            }

                        }

                    }

                }
                else if ((row.RowState == DataRowState.Deleted))
                {
                    if (Action.Contains('D'))
                    {
                        foreach (DataColumn col in row.Table.Columns)
                        {
                            if (ColumnsDictionary.ContainsKey(col.ColumnName))
                            {
                                DataColumn[] columnsKey;
                                columnsKey = row.Table.PrimaryKey;
                                DataRow drActivity = dtActivity.NewRow();
                                if (columnsKey.Length > 0)
                                {
                                    drActivity["KeyValue"] = row[columnsKey[0].ColumnName, DataRowVersion.Original].ToString();
                                    drActivity["KeyColumn"] = columnsKey[0].ColumnName;
                                }

                                drActivity["OriginalValue"] = row[col.ColumnName, DataRowVersion.Original].ToString();
                                drActivity["CurrentValue"] = "";

                                drActivity["ProfileName"] = FormName;
                                drActivity["dbTableName"] = dbTableName;
                                drActivity["Field"] = col.ColumnName;
                                drActivity["Actions"] = "Delete";
                                dtActivity.Rows.Add(drActivity);
                            }

                        }

                    }

                }
            }
            return dtActivity;
        }


        private static Dictionary<string, string> GetColumnsName(string ColumnsName)
        {


            string[] ColumnsSplit = ColumnsName.Split(',');

            Dictionary<string, string> ColumnsDictionary = new Dictionary<string, string>();

            foreach (string Column in ColumnsSplit)
            {
                string[] nameCaption = Column.Split('|');
                if (nameCaption.Length == 2 && nameCaption[1].Trim() != "")
                    ColumnsDictionary.Add(nameCaption[0].Trim(), nameCaption[1].Trim());
                else if (nameCaption.Length == 2 && nameCaption[1].Trim() == "")
                    ColumnsDictionary.Add(nameCaption[0].Trim(), nameCaption[0].Trim());

            }



            return ColumnsDictionary;
        }
        //private static void AuditRow(OracleTransaction transaction, DataRow row, string type, string userip)
        //{




        //}

    }
}
