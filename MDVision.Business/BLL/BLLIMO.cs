using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;
using MDVision.Datasets;
using WinSCP;
using System.Globalization;

namespace MDVision.Business.BLL
{
    public class BLLIMO
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminProfile"/> class.
        /// </summary>
        public BLLIMO()
        {
            //SharedVariable SharedObj
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this.SharedObj = SharedObj;
            //Add your own initialization code after the InitializeComponent() call

        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            components = new Container();
        }
        #endregion

        #region Variable


        private DataSet _dataSetFromXmlString;

        #endregion

        #region Public Functions

        #region CPT

        public String SearchCPTCode(String searchText)
        {
            String jsonData = "";
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOCPTPort));
                    string search = "search^20|5|2|1^" + searchText + "|Distinctby(title)|showfields(IMO_LEXICAL_CODE, CPT_CODE, title, ICDP_CODE, ICDP_DESC_SHORT, SCT_CONCEPT_ID, SNOMED_DESCRIPTION, HCPCS_CODE,LOINC_CODE,LOINC_DESC_SHORT)^" + MDVSession.Current.IMO_ID + Environment.NewLine;
                 
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        byte[] bytes = ASCIIEncoding.ASCII.GetBytes(search);
                        ns.Write(bytes, 0, bytes.Length);
                        jsonData = ReadBuffer(ns);
                    }
                    tcpClient.Close();
                }
            }
            catch
            {
                throw new Exception();
            }
            return jsonData;
        }
        
        public String SearchAllCPTCodes(string searchText, Int64 pageNumber, Int64 rpp)
        {
            String jsonData = "";
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOCPTPort));
                    string search = "search^100|5|2|" + pageNumber + "^" + searchText + "|Distinctby(CPT_CODE,title,SNOMED_DESCRIPTION)|showfields(IMO_LEXICAL_CODE, CPT_CODE, title, ICDP_CODE, ICDP_DESC_SHORT, SCT_CONCEPT_ID, SNOMED_DESCRIPTION, HCPCS_CODE,LOINC_CODE,LOINC_DESC_SHORT)^" + MDVSession.Current.IMO_ID + Environment.NewLine;
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(search);
                        ns.Write(bytes, 0, bytes.Length);
                        jsonData = ReadBuffer(ns);
                    }
                    tcpClient.Close();
                }
            }
            catch
            {
                throw new Exception();
            }
            //jsonData = "[{'Encounter type':'initial encounter (active treatment)','Sublocation of acetabulum':'anterior wall','Fracture alignment':'displaced','Fracture morphology':'associated transverse-posterior','Laterality':'left'},{'Encounter type':'sequela (late effects)','Sublocation of acetabulum':'posterior wall','Fracture alignment':'nondisplaced','Fracture          morphology':'transverse','Laterality':'right'},{'Encounter type':'','Sublocation of acetabulum':'anterior column','Fracture alignment':'','Fracture morphology':'','Laterality':'unspecified laterality'},{'Encounter type':'','Sublocation of acetabulum':'posterior column','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'dome','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'medial wall','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'other portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'unspecified portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''}]";
            return jsonData;
        }

        #endregion
        #region LOINC
        public String SearchLOINCCode(String searchText)
        {
            String jsonData = "";
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    searchText = "LOINC"+searchText+"IMO";
                    tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOCPTPort));
                    string search = "search^20|5|2|1^" + searchText + "|Distinctby(title)|showfields(IMO_LEXICAL_CODE, CPT_CODE, title, ICDP_CODE, ICDP_DESC_SHORT, SCT_CONCEPT_ID, SNOMED_DESCRIPTION, HCPCS_CODE,LOINC_CODE,LOINC_DESC_SHORT)^" + MDVSession.Current.IMO_ID + Environment.NewLine;

                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        byte[] bytes = ASCIIEncoding.ASCII.GetBytes(search);
                        ns.Write(bytes, 0, bytes.Length);
                        jsonData = ReadBuffer(ns);
                    }
                    tcpClient.Close();
                }
            }
            catch
            {
                throw new Exception();
            }
            return jsonData;
        }
        public String SearchAllLOINCCodes(string searchText, Int64 pageNumber, Int64 rpp)
        {
            String jsonData = "";
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    searchText = "LOINC" + searchText + "IMO";
                    tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOCPTPort));
                    string search = "search^100|5|2|" + pageNumber + "^" + searchText + "|Distinctby(CPT_CODE,title,SNOMED_DESCRIPTION)|showfields(IMO_LEXICAL_CODE, CPT_CODE, title, ICDP_CODE, ICDP_DESC_SHORT, SCT_CONCEPT_ID, SNOMED_DESCRIPTION, HCPCS_CODE,LOINC_CODE,LOINC_DESC_SHORT)^" + MDVSession.Current.IMO_ID + Environment.NewLine;
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(search);
                        ns.Write(bytes, 0, bytes.Length);
                        jsonData = ReadBuffer(ns);
                    }
                    tcpClient.Close();
                }
            }
            catch
            {
                throw new Exception();
            }
            //jsonData = "[{'Encounter type':'initial encounter (active treatment)','Sublocation of acetabulum':'anterior wall','Fracture alignment':'displaced','Fracture morphology':'associated transverse-posterior','Laterality':'left'},{'Encounter type':'sequela (late effects)','Sublocation of acetabulum':'posterior wall','Fracture alignment':'nondisplaced','Fracture          morphology':'transverse','Laterality':'right'},{'Encounter type':'','Sublocation of acetabulum':'anterior column','Fracture alignment':'','Fracture morphology':'','Laterality':'unspecified laterality'},{'Encounter type':'','Sublocation of acetabulum':'posterior column','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'dome','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'medial wall','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'other portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'unspecified portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''}]";
            return jsonData;
        }

        public static List<Model.CCDA.CPTLookupModel> GetIMOCPTByLOINCCode(string LOINCCode)
        {
            List<Model.CCDA.CPTLookupModel> CPTLookupsList = new List<Model.CCDA.CPTLookupModel>();
            try
            {
                String jsonData = "";
                var keyValues = new Dictionary<string, string> { };

                jsonData = new BLLIMO().SearchLOINCCode(LOINCCode);

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonData);
                var itemArray = SearchedfieldsJSON["data"]["items"];
                foreach (var item in itemArray)
                {
                    CPTLookupsList.Add(new Model.CCDA.CPTLookupModel
                    {
                        CPTCodeDescription = item["title"].ToString(),
                        CPTCode = item["CPTCode"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
            }
            return CPTLookupsList;
        }

        #endregion

        #region ICD

        /// <summary>
        /// ICD Search for autocomplete
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public String SearchICDCode(string searchText)
        {
            String jsonData = "";
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOICDPort));
                    string search = "search^20|5|2|1^" + searchText + "|Distinctby(title,ICD10CM_CODE)|showfields(IMO_LEXICAL_CODE, title,kndg_code, ICD10CM_CODE, ICD10CM_TITLE, SCT_CONCEPT_ID, SNOMED_DESCRIPTION)^" + MDVSession.Current.IMO_ID + Environment.NewLine;
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        byte[] bytes = ASCIIEncoding.ASCII.GetBytes(search);
                        ns.Write(bytes, 0, bytes.Length);
                        jsonData = ReadBuffer(ns);
                    }
                    tcpClient.Close();
                }
            }
            catch
            {
                throw new Exception();
            }
            //jsonData = "[{'Encounter type':'initial encounter (active treatment)','Sublocation of acetabulum':'anterior wall','Fracture alignment':'displaced','Fracture morphology':'associated transverse-posterior','Laterality':'left'},{'Encounter type':'sequela (late effects)','Sublocation of acetabulum':'posterior wall','Fracture alignment':'nondisplaced','Fracture          morphology':'transverse','Laterality':'right'},{'Encounter type':'','Sublocation of acetabulum':'anterior column','Fracture alignment':'','Fracture morphology':'','Laterality':'unspecified laterality'},{'Encounter type':'','Sublocation of acetabulum':'posterior column','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'dome','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'medial wall','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'other portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'unspecified portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''}]";
            return jsonData;
        }

        /// <summary>
        /// ICD search on detail Grid
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rpp"></param>
        /// <returns></returns>
        public String SearchAllICDCodes(string searchText, Int64 pageNumber, Int64 rpp)
        {
            String jsonData = "";
            try
            {
                // setting page number 0 so that, IMO should return all records.
                pageNumber = 0;
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOICDPort));
                    string search = "search^100|5|2|" + pageNumber + "^" + searchText + "|Distinctby(title,ICD10CM_CODE)|showfields(IMO_LEXICAL_CODE, title,kndg_code, ICD10CM_CODE, ICD10CM_TITLE, SCT_CONCEPT_ID, SNOMED_DESCRIPTION)^" + MDVSession.Current.IMO_ID + Environment.NewLine;
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(search);
                        ns.Write(bytes, 0, bytes.Length);
                        jsonData = ReadBuffer(ns);
                    }
                    tcpClient.Close();
                }
            }
            catch
            {
                throw new Exception();
            }
            //jsonData = "[{'Encounter type':'initial encounter (active treatment)','Sublocation of acetabulum':'anterior wall','Fracture alignment':'displaced','Fracture morphology':'associated transverse-posterior','Laterality':'left'},{'Encounter type':'sequela (late effects)','Sublocation of acetabulum':'posterior wall','Fracture alignment':'nondisplaced','Fracture          morphology':'transverse','Laterality':'right'},{'Encounter type':'','Sublocation of acetabulum':'anterior column','Fracture alignment':'','Fracture morphology':'','Laterality':'unspecified laterality'},{'Encounter type':'','Sublocation of acetabulum':'posterior column','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'dome','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'medial wall','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'other portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''},{'Encounter type':'','Sublocation of acetabulum':'unspecified portion of acetabulum','Fracture alignment':'','Fracture morphology':'','Laterality':''}]";
            return jsonData;
        }

        /// <summary>
        /// Get Complete deatils for Selected ICD
        /// </summary>
        /// <param name="lexiCode"></param>
        /// <returns></returns>
        public DSIMO GetIcdDetails(string lexiCode)
        {
            DSIMO dsImo = new DSIMO();

            #region Prepare DataSet against LexiCode

            // Get XML against selected ICD
            string xmlString = GetXmlString(lexiCode);

            //XmlDocument doc = new XmlDocument();
            //var a = System.AppDomain.CurrentDomain.BaseDirectory;
            //doc.Load("E:\\MyXML.xml");

            //StringWriter xmlString = new StringWriter();
            //XmlTextWriter tx = new XmlTextWriter(xmlString);
            //doc.WriteTo(tx);
            //string axmlString = xmlString.ToString();
            //axmlString = axmlString.Replace("<MODIFIERS><MODIFIER>", "<MODIFIERS1><MODIFIER>").Replace("</MODIFIER></MODIFIERS>", "</MODIFIER></MODIFIERS1>");

            // Generate DataSet from XMLString
            _dataSetFromXmlString = ConvertXmlToDataSet(xmlString);

            #endregion

            #region Mapping Generated DataSet with Application DataSet

            _MappingXmlToApplicatoinDataSet(dsImo);

            #endregion

            #region Extract Modifiers From DataTable

            // Get all the Modifers from DataTable
            List<KeyValuePair<int, string>> keyValuemodifiers = new List<KeyValuePair<int, string>>();

            if (dsImo.MODIFIERS1.Rows.Count > 0)
            {
                foreach (DataRow drModifiers in dsImo.MODIFIERS1.Rows)
                {
                    keyValuemodifiers.AddRange(from DataRow drRecord in dsImo.RECORD.Rows
                                               let modifiersRecordId = drModifiers[dsImo.MODIFIERS1.RECORD_IdColumn.ColumnName].ToString()
                                               let recordsRecordId = drRecord[dsImo.MODIFIERS1.RECORD_IdColumn.ColumnName].ToString()
                                               where modifiersRecordId == recordsRecordId
                                               select new KeyValuePair<int, string>
                                                                           (Int32.Parse(drModifiers[dsImo.MODIFIERS1.MODIFIERS1_IdColumn.ColumnName].ToString()),
                                                                           drRecord[dsImo.RECORD.TITLEColumn.ColumnName].ToString()));
                }

                #endregion

                #region DataRow To DataColumn Structure

                // 1.1 Creating TemporaryTable to Convert From DataRows to DataColumn Structure
                DataTable tmpDataTableRowsToColumnStructure = new DataTable("tmpDataTableRowsToColumnStructure");

                // Building Column Headers for 1.1 Table

                string buildString = "";
                foreach (KeyValuePair<int, string> modifiers in keyValuemodifiers)
                {
                    buildString = modifiers.Value;
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    buildString = textInfo.ToTitleCase(buildString);
                    //buildString = buildString.Replace(" ", string.Empty);
                    tmpDataTableRowsToColumnStructure.Columns.Add(buildString);
                }

                // Get modifier details against each modifier
                Int32 dataTableHandleCount = 0;
                int columnIndex = 0;
                foreach (KeyValuePair<int, string> modifiers in keyValuemodifiers)
                {
                    Int32 firstEntity = Int32.Parse(keyValuemodifiers[0].Key.ToString());

                    var rows = from row in dsImo.MODIFIER.AsEnumerable()
                               where row.Field<Int32>(dsImo.MODIFIER.MODIFIERS1_IdColumn.ColumnName) == firstEntity
                               select row;
                    dataTableHandleCount = rows.Count();

                    int rowIndex = 0;
                    var modifiers1 = modifiers;
                    foreach (DataRow drModifier in from DataRow drModifier in dsImo.MODIFIER.Rows
                                                   let modifierModifiers1Id = drModifier[dsImo.MODIFIER.MODIFIERS1_IdColumn.ColumnName].ToString()
                                                   let modifiersModifiers1Id = modifiers1.Key.ToString()
                                                   where modifierModifiers1Id == modifiersModifiers1Id
                                                   select drModifier)
                    {
                        tmpDataTableRowsToColumnStructure.Columns[buildString].Table.Rows.Add(drModifier[dsImo.MODIFIER.MODIFIER_ALT_TITLEColumn.ColumnName].ToString());
                        tmpDataTableRowsToColumnStructure.Rows[rowIndex][columnIndex] = drModifier[dsImo.MODIFIER.MODIFIER_ALT_TITLEColumn.ColumnName].ToString();
                        rowIndex++;
                    }
                    columnIndex++;
                }

                int dataTableRowsCount = dataTableHandleCount;
                EnumerableRowCollection<DSIMO.MODIFIERRow> totalModifiers = from rws in dsImo.MODIFIER.AsEnumerable()
                                                                            select rws;

                for (int i = 0; i < (totalModifiers.Count() - dataTableRowsCount); i++)
                    tmpDataTableRowsToColumnStructure.Rows[dataTableRowsCount + i][0] = string.Empty;

                // Removing Empty Records from 1.1 DataTable
                //RemoveEmptyRowsFromDataTable(tmpDataTableRowsToColumnStructure);

                #endregion

                dsImo.Tables.Add(tmpDataTableRowsToColumnStructure.Copy());
            }
            return dsImo;
        }

        /// <summary>
        /// Mapping returned XML to DataSet for Easier Manuplation of Data
        /// </summary>
        /// <param name="dsImo"></param>
        private void _MappingXmlToApplicatoinDataSet(DSIMO dsImo)
        {
            if (_dataSetFromXmlString.Tables[dsImo.MODIFIER.TableName] != null)
                if (_dataSetFromXmlString.Tables[dsImo.MODIFIER.TableName].Rows.Count > 0)
                    foreach (DataRow dr in _dataSetFromXmlString.Tables[dsImo.MODIFIER.TableName].Rows)
                    {
                        dsImo.MODIFIER.ImportRow(dr);
                    }
            if (_dataSetFromXmlString.Tables[dsImo.MODIFIERS1.TableName] != null)
                if (_dataSetFromXmlString.Tables[dsImo.MODIFIERS1.TableName].Rows.Count > 0)
                    foreach (DataRow dr in _dataSetFromXmlString.Tables[dsImo.MODIFIERS1.TableName].Rows)
                    {
                        dsImo.MODIFIERS1.ImportRow(dr);
                    }
            if (_dataSetFromXmlString.Tables[dsImo.RECORD.TableName] != null)
                if (_dataSetFromXmlString.Tables[dsImo.RECORD.TableName].Rows.Count > 0)
                    foreach (DataRow dr in _dataSetFromXmlString.Tables[dsImo.RECORD.TableName].Rows)
                    {
                        dsImo.RECORD.ImportRow(dr);
                    }
        }

        /// <summary>
        /// Filtering ICD records against modifiers combinations
        /// </summary>
        /// <param name="dataRow"></param>
        public void OnModifierSelectedIndex(DataRow dataRow)
        {
            DSIMO dsImo = new DSIMO();
            //var gvCellsCount = GridView2.SelectedRow.Cells.Count;
            var gvCellsCount = dataRow.ItemArray.Length;

            string modifiersAgainstSelectedRow = string.Empty;

            for (int i = 0; i < gvCellsCount; i++)
            {
                //modifiersAgainstSelectedRow += GridView2.SelectedRow.Cells[i].Text + ",";
                modifiersAgainstSelectedRow += dataRow[i] + ",";
            }

            modifiersAgainstSelectedRow = modifiersAgainstSelectedRow.Remove(modifiersAgainstSelectedRow.Length - 1);

            DataSet xmLtoDataSet = _dataSetFromXmlString;
            _MappingXmlToApplicatoinDataSet(dsImo);

            List<string> modifierCode = new List<string>();
            int modifierTitleCount = 0;
            foreach (DataRow drModifier in dsImo.MODIFIER.Rows)
            {
                if (modifierTitleCount < modifiersAgainstSelectedRow.Split(',').Count())
                {
                    var modifierModifierTitle = drModifier[dsImo.MODIFIER.MODIFIER_TITLEColumn.ColumnName].ToString();
                    if (modifierModifierTitle == modifiersAgainstSelectedRow.Split(',')[modifierTitleCount])
                    {
                        modifierCode.Add(drModifier[dsImo.MODIFIER.MODIFIER_CODEColumn.ColumnName].ToString());
                        modifierTitleCount++;
                    }
                }
            }

            string modifierCodes = String.Join(",", modifierCode);

            DataTable dtModifiers = dsImo.RECORD as DataTable;
            DataView dvfilteredData = new DataView(dtModifiers);

            string filter = string.Empty;
            foreach (string dlItem in modifierCode)
            {
                if (dlItem != null)
                {
                    filter += (filter == "" ? "" : " AND");
                    filter += " (MODIFIERS like '" + Convert.ToString(dlItem) + "' OR MODIFIERS like '" + Convert.ToString(dlItem) + ",%' OR MODIFIERS like '%," + Convert.ToString(dlItem) + "' OR MODIFIERS like '%," + Convert.ToString(dlItem) + ",%')";
                }
            }

            dvfilteredData.RowFilter = filter.Trim();

            dtModifiers = dvfilteredData.ToTable();

            List<string> lstString = new List<string>();

            foreach (DataRow drModifier in dtModifiers.Rows)
            {
                var i10Code = drModifier[dtModifiers.Columns[dsImo.RECORD.I10_CODEColumn.ColumnName]].ToString();
                var i10Title = drModifier[dtModifiers.Columns[dsImo.RECORD.I10_TITLEColumn.ColumnName]].ToString();

                lstString.Add(i10Code + i10Title);

                //HtmlGenericControl li = new HtmlGenericControl("li");
                //LinkButton lnk = new LinkButton
                //{
                //    ID = i10Code.ToString(),
                //    Text = i10Code.ToString() + " -  " + i10Title.ToString()
                //};
                //lnk.Click += Clicked;
                //li.Controls.Add(lnk);
                //ul_ICD.Controls.Add(li);
            }
            var aa = lstString;
        }


        public String SearchICDbySNOMEDCode(string searchText)
        {
            String jsonData = "";
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOICDPort));
                    string search = "search^20|5|2|1^sct" + searchText + "|Distinctby(title,ICD10CM_CODE)|showfields(title,ICD9CM_CODE, ICD9CM_TITLE, SCT_CONCEPT_ID)^" + MDVSession.Current.IMO_ID + Environment.NewLine;
                    using (System.Net.Sockets.NetworkStream ns = tcpClient.GetStream())
                    {
                        byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(search);
                        ns.Write(bytes, 0, bytes.Length);
                        jsonData = ReadBuffer(ns);
                    }
                    tcpClient.Close();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return jsonData;
        }

        public static List<Model.CCDA.ICDLookupModel> GetIMOICDbySNOMEDCode(string SDNOMEDCode)
        {
            List<Model.CCDA.ICDLookupModel> ICDLookupsList = new List<Model.CCDA.ICDLookupModel>();
            try
            {
                String jsonData = "";
                var keyValues = new Dictionary<string, string> { };

                jsonData = new BLLIMO().SearchICDbySNOMEDCode(SDNOMEDCode);

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(jsonData);
                var itemArray = SearchedfieldsJSON["data"]["items"];
                foreach (var item in itemArray)
                {
                    ICDLookupsList.Add(new Model.CCDA.ICDLookupModel
                    {
                        title = item["title"].ToString(),
                        SCT_CONCEPT_ID = item["SCT_CONCEPT_ID"].ToString(),
                        ICD9CM_CODE = item["kndg_code"].ToString(),
                        ICD9CM_TITLE = item["kndg_title"].ToString(),
                        ICD10CM_CODE = item["ICD10CM_CODE"].ToString(),
                        ICD10CM_TITLE = item["ICD10CM_TITLE"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return ICDLookupsList;
        }


        #endregion

        #endregion

        #region Private Functions

        /// <summary>
        /// Get Formatted XML String
        /// </summary>
        /// <param name="lexiCode"></param>
        /// <returns></returns>
        private string GetXmlString(string lexiCode)
        {
            if (lexiCode == null) throw new ArgumentNullException("lexiCode");
            var xmlString = string.Empty;
            using (TcpClient tcpClient = new TcpClient())
            {
                tcpClient.Connect(MDVSession.Current.IMOHostName, Convert.ToInt32(MDVSession.Current.IMOICDPort));
                string search = "detail^" + lexiCode + "^1^" + MDVSession.Current.IMO_ID + Environment.NewLine;

                using (NetworkStream ns = tcpClient.GetStream())
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(search);
                    ns.Write(bytes, 0, bytes.Length);
                    string result = ReadBuffer(ns);
                    if (result != "" && result[0] == '<')
                        try
                        {
                            XmlDocument doc = new XmlDocument();
                            result = "<IMO>" + result + "</IMO>";
                            doc.LoadXml(result);

                            // Replacing Nodes with same name for further processing
                            xmlString = result.Replace("<MODIFIERS><MODIFIER>", "<MODIFIERS1><MODIFIER>").Replace("</MODIFIER></MODIFIERS>", "</MODIFIER></MODIFIERS1>");
                        }
                        catch
                        {
                            throw new Exception("Error While Loading XML");
                        }
                }
                tcpClient.Close();
            }
            return xmlString;
        }

        /// <summary>
        /// Convert XML to DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        private DataSet ConvertXmlToDataSet(string xmlData)
        {
            if (xmlData == null) throw new ArgumentNullException("xmlData");
            StringReader stream;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDs = new DataSet();
                stream = new StringReader(xmlData);
                reader = new XmlTextReader(stream);
                xmlDs.ReadXml(reader, XmlReadMode.IgnoreSchema);
                return xmlDs;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        /// <summary>
        /// Remove Empty Rows from DataTable
        /// </summary>
        /// <param name="datatable"></param>
        private static void RemoveEmptyRowsFromDataTable(DataTable datatable)
        {
            if (datatable == null) throw new ArgumentNullException("datatable");
            for (int i = datatable.Rows.Count - 1; i >= 0; i--)
            {
                var columnsCount = datatable.Columns.Count;
                string dynamicIf = "";
                for (int cols = 0; cols < columnsCount; cols++)
                {
                    dynamicIf += "(dt.Rows[" + i + "][" + cols + "] == DBNull.Value || string.IsNullOrEmpty(dt.Rows[" + i + "][" + cols + "].ToString())) && ";
                }
                dynamicIf = dynamicIf.Remove(dynamicIf.Length - 3);

                if ((datatable.Rows[i][0] == DBNull.Value || datatable.Rows[i][0].ToString() == "") && (datatable.Rows[i][1] == DBNull.Value || datatable.Rows[i][1].ToString() == "") && (datatable.Rows[i][2] == DBNull.Value || datatable.Rows[i][2].ToString() == "") && (datatable.Rows[i][3] == DBNull.Value || datatable.Rows[i][3].ToString() == "") && (datatable.Rows[i][4] == DBNull.Value || datatable.Rows[i][4].ToString() == ""))
                    datatable.Rows[i].Delete();
            }

            datatable.AcceptChanges();
        }

        /// <summary>
        /// Read the Buffer
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        private String ReadBuffer(NetworkStream ns)
        {
            byte[] lenghtBuffer = new byte[4];
            int bytesRead = 0;
            while (bytesRead < 4)
                bytesRead += ns.Read(lenghtBuffer, bytesRead, 4 - bytesRead);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(lenghtBuffer);
            int responseLength = BitConverter.ToInt32(lenghtBuffer, 0);
            byte[] buffer = new byte[responseLength];
            bytesRead = 0;
            while (bytesRead < responseLength)
                bytesRead += ns.Read(buffer, bytesRead, responseLength - bytesRead);
            string result = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRead);
            return result;
        }

        #endregion

        #region Commnented

        //public string SearchIMOICDCode(string searchText)
        //{
        //    String jsonData = string.Empty;
        //    using (TcpClient tcpClient = new TcpClient())
        //    {
        //        tcpClient.Connect(SharedObj.IMOHostName, Convert.ToInt32(SharedObj.IMOICDPort));
        //        string search = "search^20|5|2|1^" + searchText + "^" + SharedObj.IMO_ID + System.Environment.NewLine;
        //        using (NetworkStream ns = tcpClient.GetStream())
        //        {
        //            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(search);
        //            ns.Write(bytes, 0, bytes.Length);
        //            jsonData = ReadBuffer(ns);
        //        }
        //        tcpClient.Close();
        //    }
        //    return jsonData;
        //}

        #endregion

    }
}
