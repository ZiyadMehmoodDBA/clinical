using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.IEHR.Common;
using MDVision.IEHR.Controls.CommonControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace MDVision.IEHR.Controls.Admin
{

    public class Admin_IMOSearch_Detail
    {
        #region Singleton

        private static Admin_IMOSearch_Detail _instance = null;
        public static Admin_IMOSearch_Detail Instance()
        {
            return _instance ?? (_instance = new Admin_IMOSearch_Detail());
        }

        #endregion

        #region Data Members
        //AppConfig.dsImo dsImo;
        #endregion

        #region Private function's

        private static string RemoveConsecutive(string text, string value)
        {
            string regex = string.Format(@"({0})+", Regex.Escape(value));
            return Regex.Replace(text, regex, match => value);
        }
        private string SearchImoProbem(string lexiCodeId)
        {
            object Obj = new object(); // For locking functionality
            lock (Obj)
            {
                if (lexiCodeId == null) throw new ArgumentNullException("lexiCodeId");
                try
                {
                    MDVSession.Current.dsImo = new BLLIMO().GetIcdDetails(lexiCodeId);
                    if (MDVSession.Current.dsImo.Tables[MDVSession.Current.dsImo.MODIFIERS1.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            IMOCount = MDVSession.Current.dsImo.Tables["tmpDataTableRowsToColumnStructure"].Rows.Count,
                            IMOLoad_JSON = MDVUtility.JSON_DataTable(MDVSession.Current.dsImo.Tables["tmpDataTableRowsToColumnStructure"]),
                            IMOLoad_JSONCombinations = MDVUtility.JSON_DataTable(MDVSession.Current.dsImo.Tables[MDVSession.Current.dsImo.MODIFIER.TableName])
                        };
                        return (JsonConvert.SerializeObject(response));
                        
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            IMOCount = 0,
                            Message = "No Record found.",
                        };
                        return (JsonConvert.SerializeObject(response));
                    }

                }
                catch (Exception ex)
                {
                    var response = new
                    {
                        status = false,
                        Message =MDVCustomException.HumanReadableMessage(ex.Message),
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
        }
        private string SearchIcd10(string modifiers)
        {
            object Obj = new object(); // For locking functionality
            lock (Obj)
            {
                try
                {
                    #region Comma Handle's

                    string modifiersAgainstSelectedRow = RemoveConsecutive(modifiers, ",");

                    if (modifiersAgainstSelectedRow.Substring(modifiersAgainstSelectedRow.Length - 1, 1) == ",")
                        modifiersAgainstSelectedRow = modifiersAgainstSelectedRow.Remove(modifiersAgainstSelectedRow.Length - 1);
                    if (modifiersAgainstSelectedRow.Substring(0, 1) == ",")
                        modifiersAgainstSelectedRow = modifiersAgainstSelectedRow.Remove(0, 1);

                    #endregion

                    #region Get Modifiers Combinations

                    DataTable dtModifierCode = MDVSession.Current.dsImo.Tables[MDVSession.Current.dsImo.MODIFIER.TableName];
                    DataView dvModifierCode = new DataView(dtModifierCode);
                    string filterModifierCode = string.Empty;
                    for (int i = 0; i < modifiersAgainstSelectedRow.Split(',').Count(); i++)
                    {
                        filterModifierCode += (filterModifierCode == "" ? "" : " OR");
                        filterModifierCode += " (" + MDVSession.Current.dsImo.MODIFIER.MODIFIER_TITLEColumn.ColumnName + " = '" + Convert.ToString(modifiersAgainstSelectedRow.Split(',')[i]) + "' OR " + MDVSession.Current.dsImo.MODIFIER.MODIFIER_ALT_TITLEColumn.ColumnName + " = '" + Convert.ToString(modifiersAgainstSelectedRow.Split(',')[i]) + "')";
                    }
                    dvModifierCode.RowFilter = filterModifierCode.Trim();
                    dtModifierCode = dvModifierCode.ToTable(true, MDVSession.Current.dsImo.MODIFIER.MODIFIER_CODEColumn.ColumnName);

                    #endregion

                    #region List of Modifiers Codes

                    List<string> modifierCode = new List<string>();
                    for (int i = 0; i < dvModifierCode.Count; i++)
                        modifierCode.Add(dtModifierCode.Rows[i][0].ToString());

                    #endregion

                    #region ICD's Against Modifiers

                    DataTable dtIcd10Modifiers = MDVSession.Current.dsImo.Tables[MDVSession.Current.dsImo.RECORD.TableName];
                    DataView dvIcd10Modifiers = new DataView(dtIcd10Modifiers);

                    string filterIcd10Modifiers = string.Empty;
                    foreach (string dlItem in modifierCode.Where(dlItem => dlItem != null))
                    {
                        filterIcd10Modifiers += (filterIcd10Modifiers == "" ? "" : " AND");
                        filterIcd10Modifiers += " (" + MDVSession.Current.dsImo.RECORD.MODIFIERSColumn.ColumnName + " like '" + Convert.ToString(dlItem) + "' OR " + MDVSession.Current.dsImo.RECORD.MODIFIERSColumn.ColumnName + " like '" + Convert.ToString(dlItem) + ",%' OR " + MDVSession.Current.dsImo.RECORD.MODIFIERSColumn.ColumnName + " like '%," + Convert.ToString(dlItem) + "' OR " + MDVSession.Current.dsImo.RECORD.MODIFIERSColumn.ColumnName + " like '%," + Convert.ToString(dlItem) + ",%')";
                    }

                    dvIcd10Modifiers.RowFilter = filterIcd10Modifiers.Trim();
                    dtIcd10Modifiers = dvIcd10Modifiers.ToTable(true, MDVSession.Current.dsImo.RECORD.I10_CODEColumn.ColumnName, MDVSession.Current.dsImo.RECORD.I10_TITLEColumn.ColumnName, MDVSession.Current.dsImo.RECORD.TITLEColumn.ColumnName, MDVSession.Current.dsImo.RECORD.ICD9_BASE_TEXT_CODEColumn.ColumnName, MDVSession.Current.dsImo.RECORD.BASE_TEXTColumn.ColumnName, MDVSession.Current.dsImo.RECORD.SNOMEDIDColumn.ColumnName, MDVSession.Current.dsImo.RECORD.TERMColumn.ColumnName);

                    #endregion

                    #region Response

                    var response = new
                    {
                        status = true,
                        ICD10Count = dtIcd10Modifiers.Rows.Count,
                        ICD10Load_JSON = MDVUtility.JSON_DataTable(dtIcd10Modifiers)
                    };
                    return (JsonConvert.SerializeObject(response));

                    #endregion
                }
                catch (Exception ex)
                {
                    #region Response

                    var response = new
                    {
                        status = false,
                        Message =MDVCustomException.HumanReadableMessage(ex.Message),
                    };
                    return (JsonConvert.SerializeObject(response));

                    #endregion
                }
            }
        }

        #endregion

        #region Service Command Handler
        /// <summary>
        /// Handle the User Commands and call to the respective methods.
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();

            switch (cammandAction)
            {
                case "SEARCH_IMO":
                    {
                        string lexiCodeId = MDVUtility.ToStr(context.Request["lexiCodeId"]);
                        string strJsonData = SearchImoProbem(lexiCodeId);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;

                case "SEARCH_ICD10":
                    {
                        string modifiersArray = context.Request["modifiersArray"];
                        //string lexiCodeId = MDVUtility.ToStr(context.Request["lexiCodeId"]);
                        string strJsonData = SearchIcd10(modifiersArray);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    } break;
            }
        }

        #endregion
    }
}