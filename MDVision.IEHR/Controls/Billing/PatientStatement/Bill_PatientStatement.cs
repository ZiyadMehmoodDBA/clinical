using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System.Data;
using MDVision.IEHR.Common;
using MDVision.IEHR.Model.Billing.PatientStatement;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.Threading.Tasks;

namespace MDVision.IEHR.Controls.Billing.PatientStatement
{
    public class Bill_PatientStatement
    {
        private BLLBillingClaim BLLBillingClaimObj = null;
        private BLLBilling BLLBillingObj = null;
        public Bill_PatientStatement()
        {
            BLLBillingClaimObj = new BLLBillingClaim();
            BLLBillingObj = new BLLBilling();
        }
        #region Singleton
        private static Bill_PatientStatement _obj = null;
        public static Bill_PatientStatement Instance()
        {
            if (_obj == null)
                _obj = new Bill_PatientStatement();
            return _obj;
        }
        #endregion

        #region Static Variables
        //  public int statementCount = 0;

        // public StringBuilder sbStatementXml = new StringBuilder();
        #endregion
        #region Private Functions



        private string SearchPatientStatement(string fieldsJSON, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                DSPatientStatement dsCharge = null;
                BLObject<DSPatientStatement> obj = null;
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                long PatientAccountNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]) ? MDVUtility.ToLong(SearchedfieldsJSON["hfPatientId"]) : 0;

                string LastName = !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientLastName"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtPatientLastName"]) : "";
                string FirstName = !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientFirstName"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtPatientFirstName"]) : "";

                long FacilityId = !string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]) ? MDVUtility.ToLong(SearchedfieldsJSON["hfFacility"]) : 0;
                long age = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlAge"]) ? MDVUtility.ToLong(SearchedfieldsJSON["ddlAge"]) : 0;
                DateTime? dosFrom = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom"]) : null;
                DateTime? dosTo = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSTo"]) : null;
                string statementFormat = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatementFormat"]) ? MDVUtility.ToStr(SearchedfieldsJSON["ddlStatementFormat"]) : "";

                obj = BLLBillingObj.LoadPatientStatement(PatientAccountNumber, LastName, FirstName, FacilityId, age, dosFrom, dosTo, statementFormat);

                dsCharge = obj.Data;
                if (obj.Data != null)
                {
                    if (dsCharge.Tables[dsCharge.PatientStatement.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = dsCharge.Tables[dsCharge.PatientStatement.TableName].Rows.Count,
                            iTotalDisplayRecords = "",//dsCharge.PatientStatement.Rows[0][dsCharge.PatientStatement.RecordCountColumn.ColumnName],
                            PatientStatementLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientStatement.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string SearchPatientStatement(PatientStatementModel model)
        {
            try
            {
                DSPatientStatement dsCharge = null;
                BLObject<DSPatientStatement> obj = null;

                long PatientAccountNumber = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToLong(model.PatientId) : 0;
                int pageNumber = !string.IsNullOrEmpty(model.PageNumber) ? MDVUtility.ToInt32(model.PageNumber) : 1;
                int rowsPerPage = !string.IsNullOrEmpty(model.RowsPerPage) ? MDVUtility.ToInt32(model.RowsPerPage) : 15;

                string LastName = !string.IsNullOrEmpty(model.LastName) ? MDVUtility.ToStr(model.LastName) : "";
                string FirstName = !string.IsNullOrEmpty(model.FirstName) ? MDVUtility.ToStr(model.FirstName) : "";

                long FacilityId = !string.IsNullOrEmpty(model.FacilityId) ? MDVUtility.ToLong(model.FacilityId) : 0;
                long age = !string.IsNullOrEmpty(model.Age) ? MDVUtility.ToLong(model.Age) : 0;
                DateTime? dosFrom = String.IsNullOrEmpty(model.DOSFrom) ? (DateTime?)null : DateTime.Parse(model.DOSFrom);
                DateTime? dosTo = String.IsNullOrEmpty(model.DOSTo) ? (DateTime?)null : DateTime.Parse(model.DOSTo);
                string statementFormat = !string.IsNullOrEmpty(model.StatementFormat) ? MDVUtility.ToStr(model.StatementFormat) : "";
                Double patBalGreaterThan = !string.IsNullOrEmpty(model.PatBalGreater) ? MDVUtility.ToDouble(model.PatBalGreater) : 0;
                Double patientBalLessThan = !string.IsNullOrEmpty(model.PatBalLess) ? MDVUtility.ToDouble(model.PatBalLess) : 0;
                DateTime? LastStatementDateFrom = String.IsNullOrEmpty(model.LastStatmentDateFrom) ? (DateTime?)null : DateTime.Parse(model.LastStatmentDateFrom);
                DateTime? LastStatementDateTo = String.IsNullOrEmpty(model.LastStatmentDateTo) ? (DateTime?)null : DateTime.Parse(model.LastStatmentDateTo);
                bool isIgnoreCycleDaysChecked = model.IgnoreCycleDays ? MDVUtility.ToBool(model.IgnoreCycleDays) : false;


                obj = BLLBillingObj.PatientStatementSearch(PatientAccountNumber, LastName, FirstName, FacilityId, age, dosFrom, dosTo, LastStatementDateFrom, LastStatementDateTo, patBalGreaterThan, patientBalLessThan, isIgnoreCycleDaysChecked, statementFormat, pageNumber, rowsPerPage);


                if (obj != null && obj.Data != null)
                {
                    dsCharge = obj.Data;
                    if (dsCharge.Tables[dsCharge.PatientStatement.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = dsCharge.Tables[dsCharge.PatientStatement.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCharge.PatientStatement.Rows[0][dsCharge.PatientStatement.RecordCountColumn.ColumnName],
                            PatientStatementLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientStatement.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

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
        private string PrintPatientStatement(string fieldJSON)
        {

            try
            {
                List<object> items = JsonConvert.DeserializeObject<List<object>>(fieldJSON);
                List<object> lstStatement = new List<object>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                lstStatement = BLLBillingObj.MakePatientStatement(items);
                string allJsnDate = js.Serialize(lstStatement);
                var response = new
                {
                    status = true,
                    Statement_JSON = allJsnDate,
                    Statement_JSON_Count = lstStatement.Count,
                    Message = Common.AppPrivileges.No_Record_Message
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string GetSubmittedStatementHTML(Int64 SubmittedStatementId)
        {
            try
            {
                BLObject<DSPatientStatement> obj = null;
                DSPatientStatement ds = new DSPatientStatement();
                obj = BLLBillingObj.GetSubmittedStatementHTML(SubmittedStatementId);
                string sumittedStatementHtml = "";
                ds = obj.Data;
                if(ds.SubmittedStatement.Rows.Count > 0)
                {
                  sumittedStatementHtml = MDVUtility.ToStr(ds.SubmittedStatement.Rows[0][ds.SubmittedStatement.StatementColumn.ColumnName]);
                }
               
                if (!string.IsNullOrEmpty(sumittedStatementHtml))
                {
                    var response = new
                    {
                        status = true,
                        SubmittedStatementLoad_JSON = sumittedStatementHtml,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        SubmittedStatementLoad_JSON = sumittedStatementHtml,
                        Message = Common.AppPrivileges.No_Record_Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
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


        public string PrintPatientStatement(PatientStatementModel model)
        {
            try
            {
                List<object> items = JsonConvert.DeserializeObject<List<object>>(model.ItemList);
                List<object> lstStatement = new List<object>();
                JavaScriptSerializer js = new JavaScriptSerializer();
           
                lstStatement = BLLBillingObj.MakePatientStatement(items);
                string allJsnDate = js.Serialize(lstStatement);
                var response = new
                {
                    status = true,
                    Statement_JSON = allJsnDate,
                    Statement_JSON_Count = lstStatement.Count,
                    Message = Common.AppPrivileges.No_Record_Message
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string PatientStatementXML(string fieldJSON)
        {
            try
            {
                List<object> items = JsonConvert.DeserializeObject<List<object>>(fieldJSON);

                List<object> lstStatement = new List<object>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                lstStatement = BLLBillingObj.MakePatientStatement(items);
                string xmlString = "";


                foreach (Dictionary<string, string> statement in lstStatement)
                {
                    DSPatientStatement dsPatientStatement = new DSPatientStatement();
                    string strStatementHeader = statement[dsPatientStatement.StatementHeader.TableName];
                    string strStatementFooter = statement[dsPatientStatement.StatementFooter.TableName];
                    string strStatementDetail = statement[dsPatientStatement.StatementDetail.TableName];
                    DataTable dtStatementHeader = new DataTable();
                    DataTable dtStatementFooter = new DataTable();
                    DataTable dtStatementDetail = new DataTable();

                    dtStatementHeader = JsonStringToDataTable(strStatementHeader);

                    for (int i = 0; i < dtStatementHeader.Columns.Count; i++)
                    {
                        if (dtStatementHeader.Rows[0][i].ToString().Contains("#"))
                        {
                            dtStatementHeader.Rows[0][i] = dtStatementHeader.Rows[0][i].ToString().Replace('#', ',');
                        }
                    }

                    dtStatementFooter = JsonStringToDataTable(strStatementFooter);
                    dtStatementDetail = JsonStringToDataTable(strStatementDetail);

                    foreach (DataRow dr in dtStatementHeader.Rows)
                    {
                        if (String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatientStatement.StatementHeader.LetterIdColumn.ColumnName])))
                        {
                            dr[dsPatientStatement.StatementHeader.LetterIdColumn.ColumnName] = DBNull.Value;
                        }

                        dsPatientStatement.StatementHeader.ImportRow(dr);
                    }

                    foreach (DataRow dr in dtStatementFooter.Rows)
                    {
                        dsPatientStatement.StatementFooter.ImportRow(dr);
                    }
                    foreach (DataRow dr in dtStatementDetail.Rows)
                    {
                        dsPatientStatement.StatementDetail.ImportRow(dr);
                    }

                    DSPatientStatement.StatementHeaderRow statementHeaderRow = dsPatientStatement.Tables[dsPatientStatement.StatementHeader.TableName].Rows[0] as DSPatientStatement.StatementHeaderRow;
                    DSPatientStatement.StatementFooterRow statementFooterRow = dsPatientStatement.Tables[dsPatientStatement.StatementFooter.TableName].Rows[0] as DSPatientStatement.StatementFooterRow;

                    XmlDocument xmlDocument = new XmlDocument();

                    XmlElement xmlPatient = xmlDocument.AppendChild(xmlDocument.CreateElement("Patient")) as XmlElement;
                    XmlElement xmlPatientInformation = xmlPatient.AppendChild(xmlDocument.CreateElement("PatientInformation")) as XmlElement;


                    XmlElement xmlPatientFullName = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("PatientName")) as XmlElement;
                    xmlPatientFullName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.PatFullNameColumn.ColumnName]);

                    XmlElement xmlPatientName = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("Name")) as XmlElement;
                    xmlPatientName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FirstNameColumn.ColumnName]) + " " + MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.LastNameColumn.ColumnName]);

                    XmlElement xmlPatientAddress = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("Address")) as XmlElement;
                    xmlPatientAddress.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.Address1Column.ColumnName] + ", " + statementHeaderRow[dsPatientStatement.StatementHeader.Address2Column.ColumnName]);
                    XmlElement xmlPatientCity = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("City")) as XmlElement;
                    xmlPatientCity.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.CityColumn.ColumnName]);
                    XmlElement xmlPatientState = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("State")) as XmlElement;
                    xmlPatientState.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.StateColumn.ColumnName]);
                    XmlElement xmlPatientZip = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("ZipCode")) as XmlElement;
                    xmlPatientZip.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.ZipCodeColumn.ColumnName]);
                    XmlElement xmlPatientZipExt = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("ZipCodeExt")) as XmlElement;
                    xmlPatientZipExt.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.ZipCodeExtColumn.ColumnName]);

                    XmlElement xmlPatientAccountNumber = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("AccountNumber")) as XmlElement;
                    xmlPatientAccountNumber.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.AccountNumberColumn.ColumnName]);

                    XmlElement xmlFacilityInformation = xmlPatient.AppendChild(xmlDocument.CreateElement("FacilityInformation")) as XmlElement;
                    XmlElement xmlFacilityName = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("Name")) as XmlElement;
                    xmlFacilityName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityDescriptionColumn.ColumnName]);
                    XmlElement xmlFacilityAddress = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("Address")) as XmlElement;
                    xmlFacilityAddress.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityAddressColumn.ColumnName]);
                    XmlElement xmlFacilityCity = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("City")) as XmlElement;
                    xmlFacilityCity.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityCityColumn.ColumnName]);
                    XmlElement xmlFacilityState = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("State")) as XmlElement;
                    xmlFacilityState.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityStateColumn.ColumnName]);
                    XmlElement xmlFacilityZip = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("ZipCode")) as XmlElement;
                    xmlFacilityZip.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityZipCodeColumn.ColumnName]);
                    XmlElement xmlFacilityZipExt = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("ZipCodeExt")) as XmlElement;
                    xmlFacilityZipExt.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityZipCodeExtColumn.ColumnName]);


                    string xmlChargesCollection = "";

                    List<DataTable> result = dsPatientStatement.Tables[dsPatientStatement.StatementDetail.TableName].AsEnumerable()
                                            .GroupBy(row => row.Field<Int64>(dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName))
                                            .Select(g => g.CopyToDataTable())
                                            .ToList();

                    foreach (DataTable splitDS in result)
                    {
                        XmlDocument xmlDocumentChanges = new XmlDocument();
                        XmlElement xmlCharge = xmlDocumentChanges.AppendChild(xmlDocumentChanges.CreateElement("Charge")) as XmlElement;
                        XmlElement xmlProvider = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Provider")) as XmlElement;
                        XmlElement xmlChargeDate = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Date")) as XmlElement;
                        if (MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName]) != null && MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName]) != "")
                        {

                            if (MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.FullNameColumn.ColumnName]).Contains("-"))
                            {
                                xmlProvider.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.FullNameColumn.ColumnName]).Replace('-', ',');
                            }
                            else
                            {
                                xmlProvider.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.FullNameColumn.ColumnName]);
                            }

                            xmlChargeDate.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.DateColumn.ColumnName]);
                        }
                        XmlElement xmlChargeProcedure = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Procedure")) as XmlElement;
                        xmlChargeProcedure.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName]);

                        XmlElement xmlChargeDescription = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Description")) as XmlElement;
                        xmlChargeDescription.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName]);

                        XmlElement xmlChargeFee = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Fee")) as XmlElement;
                        xmlChargeFee.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.ChargesColumn.ColumnName]);


                        string xmlLedgerCollection = "";

                        for (int j = 1; j < splitDS.Rows.Count; j++)
                        {
                            XmlDocument xmlDocumentLeders = new XmlDocument();
                            XmlElement xmlLedger = xmlDocumentLeders.AppendChild(xmlDocumentLeders.CreateElement("Ledger")) as XmlElement;

                            XmlElement xmlLedgerDate = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Date")) as XmlElement;
                            xmlLedgerDate.InnerText = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.DateColumn.ColumnName]);

                            XmlElement xmlLedgerDescription = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Description")) as XmlElement;
                            xmlLedgerDescription.InnerText = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName]);

                            XmlElement xmlLedgerPaid = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Paid")) as XmlElement;
                            xmlLedgerPaid.InnerText = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);

                            //XmlElement xmlLedgerBalance = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Balance")) as XmlElement;
                            //xmlLedgerBalance.InnerText = "$111.05";

                            xmlLedgerCollection += xmlDocumentLeders.InnerXml;

                        }

                        XmlElement xmlLedgers = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Legders")) as XmlElement;
                        xmlLedgers.InnerXml = xmlLedgerCollection;
                        xmlChargesCollection += xmlDocumentChanges.InnerXml;
                    }


                    //for (int i = 0; i < 3; i++)
                    //{
                    //    XmlDocument xmlDocumentChanges = new XmlDocument();
                    //    XmlElement xmlCharge = xmlDocumentChanges.AppendChild(xmlDocumentChanges.CreateElement("Charge")) as XmlElement;

                    //    XmlElement xmlChargeDate = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Date")) as XmlElement;
                    //    xmlChargeDate.InnerText = "7/26/2015";

                    //    XmlElement xmlChargeProcedure = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Procedure")) as XmlElement;
                    //    xmlChargeProcedure.InnerText = "00300";

                    //    XmlElement xmlChargeDescription = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Description")) as XmlElement;
                    //    xmlChargeDescription.InnerText = "this is description";

                    //    XmlElement xmlChargeFee = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Fee")) as XmlElement;
                    //    xmlChargeFee.InnerText = "$121.36";


                    //    string xmlLedgerCollection = "";

                    //    for (int j = 0; j < 2; j++)
                    //    {
                    //        XmlDocument xmlDocumentLeders = new XmlDocument();
                    //        XmlElement xmlLedger = xmlDocumentLeders.AppendChild(xmlDocumentLeders.CreateElement("Ledger")) as XmlElement;

                    //        XmlElement xmlLedgerDate = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Date")) as XmlElement;
                    //        xmlLedgerDate.InnerText = "08/24/2015";

                    //        XmlElement xmlLedgerDescription = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Description")) as XmlElement;
                    //        xmlLedgerDescription.InnerText = "this is ledger description";

                    //        XmlElement xmlLedgerPaid = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Paid")) as XmlElement;
                    //        xmlLedgerPaid.InnerText = "$10.31";

                    //        //XmlElement xmlLedgerBalance = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Balance")) as XmlElement;
                    //        //xmlLedgerBalance.InnerText = "$111.05";

                    //        xmlLedgerCollection += xmlDocumentLeders.InnerXml;

                    //    }

                    //    XmlElement xmlLedgers = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Legders")) as XmlElement;
                    //    xmlLedgers.InnerXml = xmlLedgerCollection;
                    //    xmlChargesCollection += xmlDocumentChanges.InnerXml;

                    //}
                    XmlElement xmlCharges = xmlPatient.AppendChild(xmlDocument.CreateElement("Charges")) as XmlElement;
                    xmlCharges.InnerXml = xmlChargesCollection;





                    XmlElement xmlBalance = xmlPatient.AppendChild(xmlDocument.CreateElement("Balance")) as XmlElement;
                    xmlBalance.InnerText = MDVUtility.ToStr(statementFooterRow[dsPatientStatement.StatementFooter.PatBalanceColumn.ColumnName]);

                    XmlElement xmlAging = xmlPatient.AppendChild(xmlDocument.CreateElement("Aging")) as XmlElement;
                    XmlElement xmlAging0_30 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging0_30")) as XmlElement;
                    xmlAging0_30.InnerText = MDVUtility.ToStr(statementFooterRow[dsPatientStatement.StatementFooter._Age0_30Column.ColumnName]);
                    XmlElement xmlAging31_60 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging31_60")) as XmlElement;
                    xmlAging31_60.InnerText = MDVUtility.ToStr(statementFooterRow[dsPatientStatement.StatementFooter._Age31_60Column.ColumnName]);
                    XmlElement xmlAging61_90 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging61_90")) as XmlElement;
                    xmlAging61_90.InnerText = MDVUtility.ToStr(statementFooterRow[dsPatientStatement.StatementFooter._Age61_90Column.ColumnName]);
                    XmlElement xmlAging91_120 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging91_120")) as XmlElement;
                    xmlAging91_120.InnerText = MDVUtility.ToStr(statementFooterRow[dsPatientStatement.StatementFooter._Age91_120Column.ColumnName]);
                    XmlElement xmlAging121_above = xmlAging.AppendChild(xmlDocument.CreateElement("Aging121_above")) as XmlElement;
                    xmlAging121_above.InnerText = MDVUtility.ToStr(statementFooterRow[dsPatientStatement.StatementFooter._Age121_OnwardColumn.ColumnName]);

                    xmlString += xmlDocument.OuterXml;


                    //dsPatientStatement.StatementHeader.Merge(JsonStringToDataTable(strStatementHeader));// as DSPatientStatement.StatementHeaderDataTable;


                    //dsPatientStatement.StatementHeader.ImportRow


                }
                string outerXML = "<?xml version='1.0' ?><root>" + xmlString + "</root>";

                var response = new
                {
                    status = true,
                    Statement_XML = outerXML,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        public string PatientStatementXML(PatientStatementModel model)
        {
            try
            {
                List<object> items = JsonConvert.DeserializeObject<List<object>>(model.ItemList);

                List<object> lstStatement = new List<object>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                lstStatement = BLLBillingObj.MakePatientStatement(items);
                string outerXML = CreateXml(lstStatement);

                var response = new
                {
                    status = true,
                    Statement_XML = outerXML,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        /// <summary>
        /// for Multithreading process
        /// </summary>
        /// <param name="sharedVariable"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string PatientStatementXML(SharedVariable sharedVariable, PatientStatementModel model)
        {
            try
            {
                List<object> items = JsonConvert.DeserializeObject<List<object>>(model.ItemList);

                List<object> lstStatement = new List<object>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                lstStatement = BLLBillingObj.MakePatientStatement(sharedVariable, items);
                string outerXML = CreateXml(lstStatement);
                var response = new
                {
                    status = true,
                    Statement_XML = outerXML,
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string CreateXml(List<object> lstStatement)
        {
            try
            {
                string xmlString = "";
                foreach (Dictionary<string, string> statement in lstStatement)
                {
                    DSPatientStatement dsPatientStatement = new DSPatientStatement();
                    string strStatementHeader = statement[dsPatientStatement.StatementHeader.TableName];
                    string strStatementFooter = statement[dsPatientStatement.StatementFooter.TableName];
                    string strStatementDetail = statement[dsPatientStatement.StatementDetail.TableName];
                    DataTable dtStatementHeader = new DataTable();
                    DataTable dtStatementFooter = new DataTable();
                    DataTable dtStatementDetail = new DataTable();

                    dtStatementHeader = JsonStringToDataTable(strStatementHeader);

                    for (int i = 0; i < dtStatementHeader.Columns.Count; i++)
                    {
                        if (dtStatementHeader.Rows[0][i].ToString().Contains("#"))
                        {
                            dtStatementHeader.Rows[0][i] = dtStatementHeader.Rows[0][i].ToString().Replace('#', ',');
                        }
                    }

                    dtStatementFooter = JsonStringToDataTable(strStatementFooter);
                    dtStatementDetail = JsonStringToDataTable(strStatementDetail);

                    foreach (DataRow dr in dtStatementHeader.Rows)
                    {
                        if (String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsPatientStatement.StatementHeader.LetterIdColumn.ColumnName])))
                        {
                            dr[dsPatientStatement.StatementHeader.LetterIdColumn.ColumnName] = DBNull.Value;
                        }

                        dsPatientStatement.StatementHeader.ImportRow(dr);
                    }

                    foreach (DataRow dr in dtStatementFooter.Rows)
                    {
                        dsPatientStatement.StatementFooter.ImportRow(dr);
                    }
                    foreach (DataRow dr in dtStatementDetail.Rows)
                    {
                        dsPatientStatement.StatementDetail.ImportRow(dr);
                    }

                    DSPatientStatement.StatementHeaderRow statementHeaderRow = dsPatientStatement.Tables[dsPatientStatement.StatementHeader.TableName].Rows[0] as DSPatientStatement.StatementHeaderRow;
                    DSPatientStatement.StatementFooterRow statementFooterRow = dsPatientStatement.Tables[dsPatientStatement.StatementFooter.TableName].Rows[0] as DSPatientStatement.StatementFooterRow;

                    XmlDocument xmlDocument = new XmlDocument();

                    XmlElement xmlPatient = xmlDocument.AppendChild(xmlDocument.CreateElement("Patient")) as XmlElement;
                    XmlElement xmlPatientInformation = xmlPatient.AppendChild(xmlDocument.CreateElement("PatientInformation")) as XmlElement;
                    XmlElement xmlPatientFullName = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("PatientName")) as XmlElement;
                    xmlPatientFullName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.PatFullNameColumn.ColumnName]) ;

                    XmlElement xmlPatientName = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("Name")) as XmlElement;
                    xmlPatientName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FirstNameColumn.ColumnName]) + " " + MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.LastNameColumn.ColumnName]);


                    XmlElement xmlPatientAddress = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("Address")) as XmlElement;
                    xmlPatientAddress.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.Address1Column.ColumnName] + ", " + statementHeaderRow[dsPatientStatement.StatementHeader.Address2Column.ColumnName]);
                    XmlElement xmlPatientCity = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("City")) as XmlElement;
                    xmlPatientCity.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.CityColumn.ColumnName]);
                    XmlElement xmlPatientState = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("State")) as XmlElement;
                    xmlPatientState.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.StateColumn.ColumnName]);
                    XmlElement xmlPatientZip = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("ZipCode")) as XmlElement;
                    xmlPatientZip.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.ZipCodeColumn.ColumnName]);
                    XmlElement xmlPatientZipExt = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("ZipCodeExt")) as XmlElement;
                    xmlPatientZipExt.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.ZipCodeExtColumn.ColumnName]);

                    XmlElement xmlPatientAccountNumber = xmlPatientInformation.AppendChild(xmlDocument.CreateElement("AccountNumber")) as XmlElement;
                    xmlPatientAccountNumber.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.AccountNumberColumn.ColumnName]);

                    XmlElement xmlFacilityInformation = xmlPatient.AppendChild(xmlDocument.CreateElement("FacilityInformation")) as XmlElement;
                    XmlElement xmlFacilityName = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("Name")) as XmlElement;
                    xmlFacilityName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityDescriptionColumn.ColumnName]);
                    XmlElement xmlFacilityAddress = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("Address")) as XmlElement;
                    xmlFacilityAddress.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityAddressColumn.ColumnName]);
                    XmlElement xmlFacilityCity = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("City")) as XmlElement;
                    xmlFacilityCity.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityCityColumn.ColumnName]);
                    XmlElement xmlFacilityState = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("State")) as XmlElement;
                    xmlFacilityState.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityStateColumn.ColumnName]);
                    XmlElement xmlFacilityZip = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("ZipCode")) as XmlElement;
                    xmlFacilityZip.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityZipCodeColumn.ColumnName]);
                    XmlElement xmlFacilityZipExt = xmlFacilityInformation.AppendChild(xmlDocument.CreateElement("ZipCodeExt")) as XmlElement;
                    xmlFacilityZipExt.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FacilityZipCodeExtColumn.ColumnName]);

                    //Patient from info
                    XmlElement xmlStatementFromInformation = xmlPatient.AppendChild(xmlDocument.CreateElement("StatementFromInformation")) as XmlElement;
                    XmlElement xmlStatementFromName = xmlStatementFromInformation.AppendChild(xmlDocument.CreateElement("Name")) as XmlElement;
                    xmlStatementFromName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FromNameColumn.ColumnName]);
                    XmlElement xmlStatementFromAddress = xmlStatementFromInformation.AppendChild(xmlDocument.CreateElement("Address")) as XmlElement;
                    xmlStatementFromAddress.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FromAddressColumn.ColumnName]);
                    XmlElement xmlStatementFromCity = xmlStatementFromInformation.AppendChild(xmlDocument.CreateElement("City")) as XmlElement;
                    xmlStatementFromCity.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FromCityColumn.ColumnName]);
                    XmlElement xmlStatementFromState = xmlStatementFromInformation.AppendChild(xmlDocument.CreateElement("State")) as XmlElement;
                    xmlStatementFromState.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FromStateColumn.ColumnName]);
                    XmlElement xmlStatementFromZip = xmlStatementFromInformation.AppendChild(xmlDocument.CreateElement("ZipCode")) as XmlElement;
                    xmlStatementFromZip.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FromZipColumn.ColumnName]);
                    XmlElement xmlStatementFromExt = xmlStatementFromInformation.AppendChild(xmlDocument.CreateElement("ZipCodeExt")) as XmlElement;
                    xmlStatementFromExt.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.FromZipExtColumn.ColumnName]);
                    /////////////////////////
                    //Patient Remit To info
                    XmlElement xmlRemitToInformation = xmlPatient.AppendChild(xmlDocument.CreateElement("RemitToInformation")) as XmlElement;
                    XmlElement xmlRemitToName = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("Name")) as XmlElement;
                    xmlRemitToName.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.RemitToNameColumn.ColumnName]);
                    XmlElement xmlRemitToAddress = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("Address")) as XmlElement;
                    xmlRemitToAddress.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.RemitToAddressColumn.ColumnName]);
                    XmlElement xmlRemitToCity = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("City")) as XmlElement;
                    xmlRemitToCity.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.RemitToCityColumn.ColumnName]);
                    XmlElement xmlRemitToState = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("State")) as XmlElement;
                    xmlRemitToState.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.RemitToStateColumn.ColumnName]);
                    XmlElement xmlRemitToZip = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("ZipCode")) as XmlElement;
                    xmlRemitToZip.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.RemitToZipColumn.ColumnName]);
                    XmlElement xmlRemitToExt = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("ZipCodeExt")) as XmlElement;
                    xmlRemitToExt.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.RemitToZipExtColumn.ColumnName]);
                    XmlElement xmlOfcHoursFrom = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("OfficeHoursFrom")) as XmlElement;
                    xmlOfcHoursFrom.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.OfcHoursFromColumn.ColumnName]);
                    XmlElement xmlOfcHoursTo = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("OfficeHoursTo")) as XmlElement;
                    xmlOfcHoursTo.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.OfcHoursToColumn.ColumnName]);
                    XmlElement xmlPhoneNumber = xmlRemitToInformation.AppendChild(xmlDocument.CreateElement("PhoneNumber")) as XmlElement;
                    xmlPhoneNumber.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.PhoneNoColumn.ColumnName]);
                    //////////////////////////////////////////////
                    XmlElement xmlMessageInformation = xmlPatient.AppendChild(xmlDocument.CreateElement("Message")) as XmlElement;
                    xmlMessageInformation.InnerText = MDVUtility.ToStr(statementHeaderRow[dsPatientStatement.StatementHeader.MessageColumn.ColumnName]);

                    string xmlChargesCollection = "";

                    List<DataTable> result = dsPatientStatement.Tables[dsPatientStatement.StatementDetail.TableName].AsEnumerable()
                                            .GroupBy(row => row.Field<Int64>(dsPatientStatement.StatementDetail.ChargeCapIdColumn.ColumnName))
                                            .Select(g => g.CopyToDataTable())
                                            .ToList();

                    int count = 0;
                    foreach (DataTable splitDS in result)
                    {
                        XmlDocument xmlDocumentChanges = new XmlDocument();
                        XmlElement xmlCharge = xmlDocumentChanges.AppendChild(xmlDocumentChanges.CreateElement("Charge")) as XmlElement;

                        //if (MDVUtility.ToStr(splitDS.Rows[count][dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName]) != null && MDVUtility.ToStr(splitDS.Rows[count][dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName]) != "")
                        //{
                        XmlElement xmlProvider = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Provider")) as XmlElement;
                        XmlElement xmlChargeDate = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Date")) as XmlElement;
                        if (MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.FullNameColumn.ColumnName]).Contains("-"))
                        {
                            xmlProvider.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.FullNameColumn.ColumnName]).Replace('-', ',');
                        }
                        else
                        {
                            xmlProvider.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.FullNameColumn.ColumnName]);
                        }

                        xmlChargeDate.InnerText = Convert.ToDateTime(splitDS.Rows[0][dsPatientStatement.StatementDetail.DateColumn.ColumnName]).ToString("MM/dd/yyyy");
                        //}

                        //count++;
                        XmlElement xmlChargeProcedure = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Procedure")) as XmlElement;
                        xmlChargeProcedure.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName]);

                        XmlElement xmlChargeDescription = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Description")) as XmlElement;
                        string Procedure = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.ProcedureColumn.ColumnName]);
                        if (Procedure == "99201" || Procedure == "99202" || Procedure == "99203" || Procedure == "99204" || Procedure == "99205")
                        {
                            xmlChargeDescription.InnerText = "Office Outpatient New";
                        }
                        else if (Procedure == "99211" || Procedure == "99212" || Procedure == "99213" || Procedure == "99214" || Procedure == "99215")
                        {
                            xmlChargeDescription.InnerText = "Office Outpatient Visit";
                        }
                        else
                        {
                            xmlChargeDescription.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName]);
                        }

                        XmlElement xmlChargeFee = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Fee")) as XmlElement;
                        xmlChargeFee.InnerText = MDVUtility.ToStr(splitDS.Rows[0][dsPatientStatement.StatementDetail.ChargesColumn.ColumnName]);


                        string xmlLedgerCollection = "";

                        for (int j = 1; j < splitDS.Rows.Count; j++)
                        {
                            XmlDocument xmlDocumentLeders = new XmlDocument();
                            XmlElement xmlLedger = xmlDocumentLeders.AppendChild(xmlDocumentLeders.CreateElement("Ledger")) as XmlElement;

                            //XmlElement xmlLedgerDate = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Date")) as XmlElement;
                            //xmlLedgerDate.InnerText = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.DateColumn.ColumnName]);


                            XmlElement xmlLedgerDescription = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Description")) as XmlElement;
                            string Description = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName]);

                            string[] arrayDescription = Description.Split('#');
                            if (arrayDescription.Length > 1)
                            {
                                if (arrayDescription[1].Trim() == "")
                                {
                                    xmlLedgerDescription.InnerText = arrayDescription[0];
                                }
                                else
                                {
                                    xmlLedgerDescription.InnerText = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName]);

                                }
                            }
                            else
                            {
                                xmlLedgerDescription.InnerText = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.DescriptionColumn.ColumnName]);

                            }

                            XmlElement xmlLedgerPaid = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Paid")) as XmlElement;
                            xmlLedgerPaid.InnerText = MDVUtility.ToStr(splitDS.Rows[j][dsPatientStatement.StatementDetail.PaidColumn.ColumnName]);

                            //XmlElement xmlLedgerBalance = xmlLedger.AppendChild(xmlDocumentLeders.CreateElement("Balance")) as XmlElement;
                            //xmlLedgerBalance.InnerText = "$111.05";

                            xmlLedgerCollection += xmlDocumentLeders.InnerXml;

                        }

                        XmlElement xmlLedgers = xmlCharge.AppendChild(xmlDocumentChanges.CreateElement("Legders")) as XmlElement;
                        xmlLedgers.InnerXml = xmlLedgerCollection;
                        xmlChargesCollection += xmlDocumentChanges.InnerXml;
                    }

                    XmlElement xmlCharges = xmlPatient.AppendChild(xmlDocument.CreateElement("Charges")) as XmlElement;
                    xmlCharges.InnerXml = xmlChargesCollection;

                    XmlElement xmlBalance = xmlPatient.AppendChild(xmlDocument.CreateElement("Balance")) as XmlElement;
                    double balance = Math.Round(MDVUtility.Tofloat(statementFooterRow[dsPatientStatement.StatementFooter.PatBalanceColumn.ColumnName]), 2);
                    xmlBalance.InnerText = MDVUtility.ToStr(balance);
                    XmlElement xmlAging = xmlPatient.AppendChild(xmlDocument.CreateElement("Aging")) as XmlElement;
                    XmlElement xmlAging0_30 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging0_30")) as XmlElement;
                    double balanceAge0_30 = Math.Round(MDVUtility.Tofloat(statementFooterRow[dsPatientStatement.StatementFooter._Age0_30Column.ColumnName]), 2);
                    xmlAging0_30.InnerText = MDVUtility.ToStr(balanceAge0_30);
                    XmlElement xmlAging31_60 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging31_60")) as XmlElement;
                    double balanceAge31_60 = Math.Round(MDVUtility.Tofloat(statementFooterRow[dsPatientStatement.StatementFooter._Age31_60Column.ColumnName]), 2);
                    xmlAging31_60.InnerText = MDVUtility.ToStr(balanceAge31_60);
                    XmlElement xmlAging61_90 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging61_90")) as XmlElement;
                    double balanceAging61_90 = Math.Round(MDVUtility.Tofloat(statementFooterRow[dsPatientStatement.StatementFooter._Age61_90Column.ColumnName]), 2);
                    xmlAging61_90.InnerText = MDVUtility.ToStr(balanceAging61_90);
                    XmlElement xmlAging91_120 = xmlAging.AppendChild(xmlDocument.CreateElement("Aging91_120")) as XmlElement;
                    double balanceAging91_120 = Math.Round(MDVUtility.Tofloat(statementFooterRow[dsPatientStatement.StatementFooter._Age91_120Column.ColumnName]), 2);
                    xmlAging91_120.InnerText = MDVUtility.ToStr(balanceAging91_120);
                    XmlElement xmlAging121_above = xmlAging.AppendChild(xmlDocument.CreateElement("Aging121_above")) as XmlElement;
                    double balanceAging121_above = Math.Round(MDVUtility.Tofloat(statementFooterRow[dsPatientStatement.StatementFooter._Age121_OnwardColumn.ColumnName]), 2);
                    xmlAging121_above.InnerText = MDVUtility.ToStr(balanceAging121_above);

                    xmlString += xmlDocument.OuterXml;


                }
                return "<?xml version='1.0' ?><root>" + xmlString + "</root>";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable convertStringToDataTable(string data)
        {
            DataTable dataTable = new DataTable();
            bool columnsAdded = false;
            foreach (string row in data.Split('$'))
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (string cell in row.Split('|'))
                {
                    string[] keyValue = cell.Split('~');
                    if (!columnsAdded)
                    {
                        DataColumn dataColumn = new DataColumn(keyValue[0]);
                        dataTable.Columns.Add(dataColumn);
                    }
                    dataRow[keyValue[0]] = keyValue[0];
                }
                columnsAdded = true;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }


            return dt;
        }


        private string SavePatientStatement(string data, string statement, string cleainghouseId, string isFromSubmit)
        {
            try
            {

                //List<object> lstStatement = new List<object>();
                //string visitIds = "";
                //JavaScriptSerializer ser = new JavaScriptSerializer();
                //dynamic obj = ser.Deserialize<object>(data);
                //DSPatientStatement dsPatientStatement = new DSPatientStatement();
                //dynamic header = ser.Deserialize<object>(obj[dsPatientStatement.StatementHeader.TableName]);
                //dynamic footer = ser.Deserialize<object>(obj[dsPatientStatement.StatementFooter.TableName]);
                //dynamic detail = ser.Deserialize<object>(obj[dsPatientStatement.StatementDetail.TableName]);


                //foreach (dynamic detailHash in detail)
                //{

                //    visitIds += MDVUtility.ToStr(detailHash[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]) + ",";
                //}
                //DSPatientStatement.PatientStatementRow dr = dsPatientStatement.PatientStatement.NewPatientStatementRow();
                //foreach (dynamic footerHash in footer)
                //{
                //    dr.Age = MDVUtility.ToStr(footerHash[dsPatientStatement.StatementFooter.AgeColumn.ColumnName]);
                //    dr.PatBalance = MDVUtility.ToDouble(footerHash[dsPatientStatement.StatementFooter.PatBalanceColumn.ColumnName]);
                //}
                //foreach (dynamic headerHash in header)
                //{

                //    dr.PatientId = MDVUtility.ToInt64(headerHash[dsPatientStatement.StatementHeader.PatientIdColumn.ColumnName]);
                //    dr.LastName = MDVUtility.ToStr(headerHash[dsPatientStatement.StatementHeader.LastNameColumn.ColumnName]);
                //    dr.FirstName = MDVUtility.ToStr(headerHash[dsPatientStatement.StatementHeader.FirstNameColumn.ColumnName]);
                //    dr.FacilityId = MDVUtility.ToInt64(headerHash[dsPatientStatement.StatementHeader.FacilityIdColumn.ColumnName]);
                //    //dr.ProviderId = MDVUtility.ToInt64(headerHash[dsPatientStatement.StatementHeader.ProviderIdColumn.ColumnName]);
                //    //dr.AdvancePaymentId = MDVUtility.ToInt64(headerHash[dsPatientStatement.StatementHeader.AdvancePaymentIdColumn.ColumnName]);

                //    dr.LastStatementDate = DateTime.Now;
                //    dr.Statement = statement;
                //}
                //dr.VisitIDs = visitIds.Remove(visitIds.Length - 1, 1);
                //dr.isActive = true;
                //dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.CreatedOn = DateTime.Now;
                //dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //dr.ModifiedOn = DateTime.Now;

                //#region Database Insertion
                //dsPatientStatement.PatientStatement.AddPatientStatementRow(dr);

                ////start syed zia , bug #PMS-4575
                //string isSubmitted = BLLBillingObj.StatementSubmittedStatus(MDVUtility.ToInt64(dsPatientStatement.PatientStatement[0].PatientId));
                //if (isSubmitted == "0")
                //{
                //    //if clearing houseId is empty it does not upload xml on ftp 
                //    if (isFromSubmit == "true" && cleainghouseId != "")
                //    {
                //        string StatementXml = PatientStatementXML(obj[dsPatientStatement.StatementHeader.TableName]);
                //        //upload xml on Ftp
                //        dynamic objXml = ser.Deserialize<object>(StatementXml);
                //        string patientStatementXML = objXml["Statement_XML"];
                //        BLObject<bool> objresponse = BLLBillingClaimObj.UploadPatientStatement(MDVUtility.ToConvertInt32(cleainghouseId), patientStatementXML);

                //        if (objresponse.Data != true)
                //        {

                //            var response = new
                //            {
                //                status = false,
                //                Message = objresponse.Message
                //            };
                //            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                //        }
                //    }

                //    BLObject<DSPatientStatement> BLobj = BLLBillingObj.InsertPatientStatement(dsPatientStatement);
                //    if (BLobj.Data != null)
                //    {
                //        var response = new
                //        {
                //            status = true,
                //            Message = Common.AppPrivileges.Statement_Submitted_Success_Message,
                //        };
                //        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                //    }
                //    else
                //    {
                //        var response = new
                //        {
                //            status = false,
                //            Message = BLobj.Message
                //        };
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                //    }
                //}
                //else
                //{
                //    var response = new
                //    {
                //        status = false,
                //        Message = "Already Submitted"
                //    };
                //    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                //}
                ////end syed zia , bug #PMS-4575
                //#endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }


            return "";
        }

        public string SavePatientStatement(PatientStatementModel model)
        {
            try
            {
                // string submit_Message = "";
                var statementJson =  model.ItemList;
                List<Dictionary<string, string>> SelectedStatementList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(statementJson);
                Bill_PatientStatement statementObj = new Bill_PatientStatement();


                //intialize session variable
                SharedVariable SharedVariable = new SharedVariable();
                SharedVariable.EntityId = MDVSession.Current.EntityId;
                SharedVariable.AppUserId = MDVSession.Current.AppUserId;
                SharedVariable.UserName = MDVSession.Current.AppUserName;
                SharedVariable.ClientId = MDVSession.Current.ClientId;
                SharedVariable.AppPassWord = MDVSession.Current.AppPassWord;
                SharedVariable.WebEntityURL = MDVSession.Current.WebEntityURL;
                string WinscpPatch = string.Empty;
                //WINSCP path
                if (HttpContext.Current != null)
                    WinscpPatch = MDVUtility.GetDLLPath("~/DLL", "winscp", "exe");
                else
                    WinscpPatch = System.Configuration.ConfigurationManager.AppSettings["WinSCP_Path"];


                #region batch submission

                int TotalStatementInBatch = MDVUtility.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TotalStatementInBatch"]);
                double selectedStatements = SelectedStatementList.Count;
                double totalBatches = Math.Ceiling(selectedStatements / TotalStatementInBatch);

                Dictionary<int, Task<PatientStatementModel>> tasks = new Dictionary<int, Task<PatientStatementModel>>();
                int skipItem = 0;

                for (int i = 0; i < totalBatches; i++)
                {
                    List<Dictionary<string, string>> lstSelectedStatementList = SelectedStatementList.Skip(skipItem).Take(TotalStatementInBatch).ToList();
                    Task<PatientStatementModel> task = new Task<PatientStatementModel>(() => statementObj.SubmitPatientStatement(SharedVariable, WinscpPatch, lstSelectedStatementList, model));
                    task.Start();
                    tasks.Add(i, task);
                    skipItem = skipItem + TotalStatementInBatch;
                }

                Task.WaitAll(tasks.Values.ToArray());
                tasks.OrderByDescending(p => p.Key);
                int total_submitted_claims = 0;
                List<PatientStatementModel> list = tasks.Values.ToList<Task<PatientStatementModel>>().Select(p => p.Result).ToList<PatientStatementModel>();

                if (list.Where(p => p.Status == true).Count() > 0)
                    total_submitted_claims = list.Where(p => p.Status == true).Sum(q => q.SubmittedStatementCount);

                if (total_submitted_claims > 0)
                {
                    var response = new
                    {
                        status = true,
                        Message = total_submitted_claims + " " + Common.AppPrivileges.Statement_Submitted_Success_Message,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = list.FirstOrDefault(p => p.Message != null).Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                #endregion


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }

        }

        /// <summary>
        /// Submit patient statement 
        /// </summary>
        /// <param name="sharedVariable"></param>
        /// <param name="WinscpPatch"></param>
        /// <param name="SelectedStatementList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private PatientStatementModel SubmitPatientStatement(SharedVariable sharedVariable, string WinscpPatch, List<Dictionary<string, string>> SelectedStatementList, PatientStatementModel model)
        {
            PatientStatementModel objModel = new PatientStatementModel();

            try
            {
                string strPatientStatementXML = "";
                List<object> lstStatement = new List<object>();
                DSPatientStatement dsPatientStatement = new DSPatientStatement();
               // string jsonHeader = model.ItemList;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string jsonHeader = ser.Serialize(SelectedStatementList);

                for (int i = 0; i < SelectedStatementList.Count; i++)
                {
                  
                    string isSubmitted = "0";
                    string batchResubmit = SelectedStatementList[i].FirstOrDefault(x => x.Key == "BatchResubmit").Value;
                    if (!string.IsNullOrEmpty(batchResubmit))
                    {
                        batchResubmit = batchResubmit.ToLower();
                    }
                    /********  if incoming request is for resubmit then don't check whether the statement is already submitted or not  *******/
                    if (batchResubmit != "true" && model.IgnoreCycleDays != true)
                    {
                        //check either statement already submitted or not
                        
                            isSubmitted = BLLBillingObj.StatementSubmittedStatus(sharedVariable, MDVUtility.ToInt64(SelectedStatementList[i].FirstOrDefault(x => x.Key == "PatientId").Value));
                        
                    }
                    if (isSubmitted == "0")
                    {
                       

                        //foreach (dynamic detailHash in objdetail)
                        //{
                        //    visitIds += MDVUtility.ToStr(detailHash[dsPatientStatement.StatementDetail.VisitIdColumn.ColumnName]) + ",";
                        //}
                        DSPatientStatement.PatientStatementRow dr = dsPatientStatement.PatientStatement.NewPatientStatementRow();
                        //foreach (dynamic footerHash in objfooter)
                        //{
                        //    dr.Age = MDVUtility.ToStr(footerHash[dsPatientStatement.StatementFooter.AgeColumn.ColumnName]);
                        //    dr.PatBalance = MDVUtility.ToDouble(footerHash[dsPatientStatement.StatementFooter.PatBalanceColumn.ColumnName]);
                        //}
                     
                        dr.PatBalance = MDVUtility.ToDouble(SelectedStatementList[i].FirstOrDefault(x => x.Key == "PatBalance").Value);
                        dr.PatientId = MDVUtility.ToInt64(SelectedStatementList[i].FirstOrDefault(x => x.Key == "PatientId").Value);
                        dr.LastName = MDVUtility.ToStr(SelectedStatementList[i].FirstOrDefault(x => x.Key == "LastName").Value);
                        dr.FirstName = MDVUtility.ToStr(SelectedStatementList[i].FirstOrDefault(x => x.Key == "FirstName").Value);
                        dr.FacilityId = MDVUtility.ToInt64(SelectedStatementList[i].FirstOrDefault(x => x.Key == "FacilityId").Value);
                        dr.SubmittedChargeIds = MDVUtility.ToStr(SelectedStatementList[i].FirstOrDefault(x => x.Key == "SubmittedChargeIds").Value);
                        dr.VisitIDs = MDVUtility.ToStr(SelectedStatementList[i].FirstOrDefault(x => x.Key == "VisitIDs").Value);
                        dr.GuarantorId = MDVUtility.ToInt64(SelectedStatementList[i].FirstOrDefault(x => x.Key == "GuarantorId").Value);
                        dr.Age = MDVUtility.ToStr(SelectedStatementList[i].FirstOrDefault(x => x.Key == "Age").Value);
                        
                        dr.LastStatementDate = DateTime.Now;
                        dr.Statement = model.MarkUp;

                       // dr.VisitIDs = visitIds.Remove(visitIds.Length - 1, 1);
                        dr.isActive = true;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(sharedVariable.UserName);
                        dr.CreatedOn = DateTime.Now;
                        dr.ModifiedBy = MDVUtility.DecryptFrom64(sharedVariable.UserName);
                        dr.ModifiedOn = DateTime.Now;
                       
                        #region Database Insertion
                        dsPatientStatement.PatientStatement.AddPatientStatementRow(dr);

                        //bulk insertion of statement at the end of loop
                        if (i == SelectedStatementList.Count - 1)
                        {
                            //To show total number of submitted statement
                            objModel.SubmittedStatementCount = SelectedStatementList.Count;
                            //if clearing houseId is empty it will not upload xml on ftp 
                            if (model.isFromSubmitted == "true" && model.ClearingHouseId != "")
                            {

                                //Get Xml of selected patient statements ///////////////////////////////
                                JavaScriptSerializer serilizeHeader = new JavaScriptSerializer();
                                PatientStatementModel modelHeader = new PatientStatementModel();
                                modelHeader.ItemList = jsonHeader;
                                string StatementXml = PatientStatementXML(sharedVariable, modelHeader);

                                //upload xml on Ftp
                                dynamic objXml = ser.Deserialize<object>(StatementXml);
                                strPatientStatementXML = objXml["Statement_XML"];

                                BLObject<bool> objresponse = BLLBillingClaimObj.UploadPatientStatement(sharedVariable, WinscpPatch, MDVUtility.ToConvertInt32(model.ClearingHouseId), strPatientStatementXML);

                                if (objresponse.Data != true)
                                {
                                    objModel.Status = false;
                                    objModel.Message = objresponse.Message;
                                    return objModel;
                                }
                            }

                            // TODO Add BAtch submission process here //////////////////////////
                            bool IsElectronic = model.isFromSubmitted == "true" ? true : false;
                           
                            if (batchResubmit == "True")
                            {
                                Int32 BatchId  = MDVUtility.ToInt32(SelectedStatementList[i].FirstOrDefault(x => x.Key == "BatchId").Value);
                                ResubmitPatientStatementsBatch(sharedVariable, BatchId);
                            }
                            InsertPatientStatementsBatch(sharedVariable, dsPatientStatement, MDVUtility.ToInt32(model.ClearingHouseId), IsElectronic, strPatientStatementXML);

                            BLObject<DSPatientStatement> BLobj = BLLBillingObj.InsertPatientStatement(sharedVariable, dsPatientStatement);

                            //if exception occured during insertion then return 
                            if (BLobj.Data == null)
                            {
                                objModel.Status = false;
                                objModel.Message = BLobj.Message;
                                return objModel;
                            }
                            else
                            {
                                objModel.Status = true;

                            }
                        }

                    }

                        #endregion
                }

                // if all statement already submitted
                if (objModel.Status == false)
                {
                    objModel.Status = false;
                    objModel.Message = "Already Submitted";
                    return objModel;
                }
                else
                {
                    return objModel;
                }

            }
            catch (Exception ex)
            {
                objModel.Status = false;
                objModel.Message =MDVCustomException.HumanReadableMessage(ex.Message);
                return objModel;
            }

        }


        private string SearchSubmittedPatientStatement(string fieldsJSON, Int64 PatientStatementID, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                DSPatientStatement dsPatientStatement = null;
                BLObject<DSPatientStatement> obj = null;
                long PatientAccountNumber = 0;
                string LastName = "";
                string FirstName = "";
                long FacilityId = 0;
                long age = 0;
                DateTime? dosFrom = null;
                DateTime? dosTo = null;
                string statementFormat = "";

                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                if (fieldsJSON != "null")
                {
                    var SearchedfieldsJSON = ser.Deserialize<dynamic>(fieldsJSON);

                    if (SearchedfieldsJSON.Count > 0)
                    {

                        if (SearchedfieldsJSON.ContainsKey("hfPatientId"))
                            PatientAccountNumber = !string.IsNullOrEmpty(SearchedfieldsJSON["hfPatientId"]) ? MDVUtility.ToLong(SearchedfieldsJSON["hfPatientId"]) : 0;
                        if (SearchedfieldsJSON.ContainsKey("txtPatientLastName"))
                            LastName = !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientLastName"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtPatientLastName"]) : "";
                        if (SearchedfieldsJSON.ContainsKey("txtPatientFirstName"))
                            FirstName = !string.IsNullOrEmpty(SearchedfieldsJSON["txtPatientFirstName"]) ? MDVUtility.ToStr(SearchedfieldsJSON["txtPatientFirstName"]) : "";
                        if (SearchedfieldsJSON.ContainsKey("hfFacility"))
                            FacilityId = !string.IsNullOrEmpty(SearchedfieldsJSON["hfFacility"]) ? MDVUtility.ToLong(SearchedfieldsJSON["hfFacility"]) : 0;
                        if (SearchedfieldsJSON.ContainsKey("ddlAge"))
                            age = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlAge"]) ? MDVUtility.ToLong(SearchedfieldsJSON["ddlAge"]) : 0;
                        if (SearchedfieldsJSON.ContainsKey("dtpDOSFrom"))
                            dosFrom = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSFrom"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSFrom"]) : null;
                        if (SearchedfieldsJSON.ContainsKey("dtpDOSTo"))
                            dosTo = !string.IsNullOrEmpty(SearchedfieldsJSON["dtpDOSTo"]) ? MDVUtility.ToDateTime(SearchedfieldsJSON["dtpDOSTo"]) : null;
                        if (SearchedfieldsJSON.ContainsKey("ddlStatementFormat"))
                            statementFormat = !string.IsNullOrEmpty(SearchedfieldsJSON["ddlStatementFormat"]) ? MDVUtility.ToStr(SearchedfieldsJSON["ddlStatementFormat"]) : "";

                        obj = BLLBillingObj.LoadPrintedPatientStatement(PatientStatementID, PatientAccountNumber, LastName, FirstName, FacilityId, age, dosFrom, dosTo);
                    }
                    else
                    {
                        obj = BLLBillingObj.LoadPrintedPatientStatement(PatientStatementID, 0, null, null, 0, 0, null, null);
                    }

                }
                else
                {
                    obj = BLLBillingObj.LoadPrintedPatientStatement(PatientStatementID, 0, null, null, 0, 0, null, null);
                }
                dsPatientStatement = obj.Data;
                if (dsPatientStatement.Tables[dsPatientStatement.PatientStatement.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = dsPatientStatement.Tables[dsPatientStatement.PatientStatement.TableName].Rows.Count,
                        iTotalDisplayRecords = "",//dsCharge.PatientStatement.Rows[0][dsCharge.PatientStatement.RecordCountColumn.ColumnName],
                        PatientStatementLoad_JSON = MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.PatientStatement.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = "Record not found."
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string SearchSubmittedPatientStatement(PatientStatementModel model)
        {
            try
            {
                DSPatientStatement dsPatientStatement = null;
                BLObject<DSPatientStatement> obj = null;

                if (model != null)
                {

                    long PatientAccountNumber = !string.IsNullOrEmpty(model.PatientId) ? MDVUtility.ToLong(model.PatientId) : 0;

                    string LastName = !string.IsNullOrEmpty(model.LastName) ? MDVUtility.ToStr(model.LastName) : "";
                    string FirstName = !string.IsNullOrEmpty(model.FirstName) ? MDVUtility.ToStr(model.FirstName) : "";
                    int pageNumber = !string.IsNullOrEmpty(model.PageNumber) ? MDVUtility.ToInt32(model.PageNumber) : 1;
                    int rowsPerPage = !string.IsNullOrEmpty(model.RowsPerPage) ? MDVUtility.ToInt32(model.RowsPerPage) : 15;
                    long FacilityId = !string.IsNullOrEmpty(model.FacilityId) ? MDVUtility.ToLong(model.FacilityId) : 0;
                    long age = !string.IsNullOrEmpty(model.Age) ? MDVUtility.ToLong(model.Age) : 0;
                    //DateTime? dosFrom = String.IsNullOrEmpty(model.DOSFrom) ? (DateTime?)null : DateTime.Parse(model.DOSFrom);
                    //DateTime? dosTo = String.IsNullOrEmpty(model.DOSTo) ? (DateTime?)null : DateTime.Parse(model.DOSTo);
                    string statementFormat = !string.IsNullOrEmpty(model.StatementFormat) ? MDVUtility.ToStr(model.StatementFormat) : "";
                    Int64 patBalGreaterThan = !string.IsNullOrEmpty(model.PatBalGreater) ? MDVUtility.ToLong(model.PatBalGreater) : 0;
                    Int64 patientBalLessThan = !string.IsNullOrEmpty(model.PatBalLess) ? MDVUtility.ToLong(model.PatBalLess) : 0;
                    DateTime? LastStatementDateFrom = String.IsNullOrEmpty(model.LastStatmentDateFrom) ? (DateTime?)null : DateTime.Parse(model.LastStatmentDateFrom);
                    DateTime? LastStatementDateTo = String.IsNullOrEmpty(model.LastStatmentDateTo) ? (DateTime?)null : DateTime.Parse(model.LastStatmentDateTo);
                    int ClearingHouseId = string.IsNullOrEmpty(model.ClearingHouse) ? 0 : Convert.ToInt32(model.ClearingHouse);
                    obj = BLLBillingObj.LoadPatientSubmittedStatement(MDVUtility.ToInt64(model.PatientStatementID), PatientAccountNumber, LastName, FirstName, FacilityId, age, LastStatementDateFrom, LastStatementDateTo, patBalGreaterThan, patientBalLessThan, ClearingHouseId, pageNumber, rowsPerPage);


                }

                dsPatientStatement = obj.Data;
                if (dsPatientStatement.Tables[dsPatientStatement.PatientStatement.TableName].Rows.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = dsPatientStatement.Tables[dsPatientStatement.PatientStatement.TableName].Rows.Count,
                        iTotalDisplayRecords = dsPatientStatement.PatientStatement.Rows[0][dsPatientStatement.PatientStatement.RecordCountColumn.ColumnName],
                        PatientStatementLoad_JSON = MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.PatientStatement.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = "Record not found."
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private string DeletePatientStatement(long PatStmtID)
        {

            try
            {
                if (PatStmtID == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeletePatientStatement(PatStmtID);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string DeletePatientStatement(PatientStatementModel model)
        {
            try
            {
                long PatStmtID = MDVUtility.ToLong(model.PatientStatementID);
                if (PatStmtID == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Delete_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.DeletePatientStatement(PatStmtID);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        #region Statement Batch
        internal string SearchPatientStatementsBatch(PatientStatementModel model)
        {
            try
            {
                DSPatientStatement dsPatientStatement = null;
                BLObject<DSPatientStatement> obj = null;

                string BatchNumber = !string.IsNullOrEmpty(model.BatchNumber) ? MDVUtility.ToStr(model.BatchNumber) : "";
                int pageNumber = !string.IsNullOrEmpty(model.PageNumber) ? MDVUtility.ToInt32(model.PageNumber) : 1;
                int rowsPerPage = !string.IsNullOrEmpty(model.RowsPerPage) ? MDVUtility.ToInt32(model.RowsPerPage) : 15;
                string ClearingHouse = !string.IsNullOrEmpty(model.ClearingHouse) ? MDVUtility.ToStr(model.ClearingHouse) : "";
                DateTime? SubmittedDate = String.IsNullOrEmpty(model.SubmittedDate) ? (DateTime?)null : DateTime.Parse(model.SubmittedDate);
                string SubmittedBy = !string.IsNullOrEmpty(model.SubmittedBy) ? MDVUtility.ToStr(model.SubmittedBy) : "";
                string SubmitType = !string.IsNullOrEmpty(model.SubmitType) ? MDVUtility.ToStr(model.SubmitType) : "";
                string BatchStatus = !string.IsNullOrEmpty(model.BatchStatus) ? MDVUtility.ToStr(model.BatchStatus) : "";
                Int64 SubmittedById =  MDVUtility.ToInt64(model.SubmittedById);
                obj = BLLBillingObj.SearchPatientStatementsBatch(0, BatchNumber, SubmittedDate, SubmittedBy, SubmittedById, SubmitType, MDVUtility.ToInt32(ClearingHouse), BatchStatus, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));

                dsPatientStatement = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatientStatement.SubmittedStatementBatch.Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementBatchCount = dsPatientStatement.Tables[dsPatientStatement.SubmittedStatementBatch.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatientStatement.SubmittedStatementBatch.Rows[0][dsPatientStatement.SubmittedStatementBatch.RecordCountColumn.ColumnName],
                            PatientStatementBatchLoad_JSON = MDVUtility.JSON_DataTable(dsPatientStatement.Tables[dsPatientStatement.SubmittedStatementBatch.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        private int InsertPatientStatementsBatch(SharedVariable sharedVariable, DSPatientStatement dsPatientStatement, int clearingHouseId = 0, bool submitType = true, string BatchXML = "")
        {
            int BatchId = 0;
            try
            {

                DSPatientStatement.SubmittedStatementBatchRow dr = dsPatientStatement.SubmittedStatementBatch.NewSubmittedStatementBatchRow();

                dr.SubmittedDate = DateTime.Now;
                dr.SubmittedBy = MDVUtility.DecryptFrom64(sharedVariable.UserName);
                dr.SubmitType = submitType;
                dr.ClearingHouseId = clearingHouseId;
                dr.TotalPatients = dsPatientStatement.PatientStatement.Rows.Count;
                dr.BatchStatus = "Submitted";
                dr.BatchXML = BatchXML;
                dr.EntityId = MDVUtility.ToInt32(sharedVariable.EntityId);
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(sharedVariable.UserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(sharedVariable.UserName);
                dr.ModifiedOn = DateTime.Now;

                dsPatientStatement.SubmittedStatementBatch.AddSubmittedStatementBatchRow(dr);



                BLObject<DSPatientStatement> obj = null;


                obj = BLLBillingObj.InsertPatientStatementsBatch(sharedVariable, dsPatientStatement);

                dsPatientStatement = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatientStatement.SubmittedStatementBatch.Rows.Count > 0)
                    {
                        BatchId = MDVUtility.ToInt32(dsPatientStatement.SubmittedStatementBatch.Rows[0][dsPatientStatement.SubmittedStatementBatch.BatchIdColumn.ColumnName]);

                        if (BatchId > 0)
                        {
                            foreach (DSPatientStatement.PatientStatementRow drr in dsPatientStatement.PatientStatement.Rows)
                            {
                                drr.BatchId = BatchId;
                            }
                        }
                    }
                }


                return BatchId;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string ResubmitPatientStatementsBatch(SharedVariable sharedVariable, int BatchId)
        {
            try
            {
                if (BatchId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Resubmit_Error_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLBillingObj.ResubmitPatientStatementsBatch(sharedVariable, BatchId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Resubmit_Visit_Charge_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string SearchBatchXML(long BatchId)
        {
            try
            {
                DSPatientStatement dsPatientStatement = null;
                BLObject<DSPatientStatement> obj = null;

                obj = BLLBillingObj.SearchPatientStatementsBatch(BatchId, "", null, "", null, "", 0, "", 1, 100);

                dsPatientStatement = obj.Data;
                if (obj.Data != null)
                {
                    if (dsPatientStatement.SubmittedStatementBatch.Rows.Count > 0)
                    {

                        var response = new
                        {
                            status = true,
                            Statement_XML = dsPatientStatement.SubmittedStatementBatch.Rows[0][dsPatientStatement.SubmittedStatementBatch.BatchXMLColumn.ColumnName],
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            Message = "No XML found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        internal string SearchBatchDetail(long BatchId)
        {
            try
            {
                DSPatientStatement dsCharge = null;
                BLObject<DSPatientStatement> obj = null;

                obj = BLLBillingObj.SearchPatientStatementsBatchDetail(BatchId);

                dsCharge = obj.Data;
                if (obj.Data != null)
                {
                    if (dsCharge.Tables[dsCharge.PatientStatement.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = dsCharge.Tables[dsCharge.PatientStatement.TableName].Rows.Count,
                            iTotalDisplayRecords = dsCharge.PatientStatement.Rows[0][dsCharge.PatientStatement.RecordCountColumn.ColumnName],
                            PatientStatementLoad_JSON = MDVUtility.JSON_DataTable(dsCharge.Tables[dsCharge.PatientStatement.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientStatementCount = 0,
                            Message = "Record not found."
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientStatementCount = 0,
                        Message = obj.Message
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }

            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message =MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }


        #endregion


        #endregion

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

                case "SEARCH_PATIENT_STATEMENT":
                    {
                        string fieldsJSON = context.Request["PatientStatementData"];


                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        string strJSONData = SearchPatientStatement(fieldsJSON, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "PRINT_PATIENT_STATEMENT":
                    {
                        string fieldsJSON = context.Request["data"];
                        string strJSONData = PrintPatientStatement(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SAVE_PATIENT_STATEMENT":
                    {

                        string fieldsJSON = MDVUtility.ReplaceSpecialCharacters(context.Request["data"]);
                        string statement = context.Request["statement"];
                        string isFromSubmitted = context.Request["isFromSubmitted"];
                        string clearingHouseId = context.Request["clearingHouseId"];
                        string strJSONData = SavePatientStatement(fieldsJSON, statement, clearingHouseId, isFromSubmitted);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "SEARCH_SUBMITTED_PATIENT_STATEMENT":
                    {
                        string fieldsJSON = context.Request["PatientStatementData"];


                        Int32 PageNumber = MDVUtility.ToInt32(context.Request["PageNumber"]);
                        Int32 RowsPerPage = MDVUtility.ToInt32(context.Request["RowsPerPage"]);
                        Int64 PatientStatementID = MDVUtility.ToInt64(context.Request["PatientStatementID"]);
                        string strJSONData = SearchSubmittedPatientStatement(fieldsJSON, PatientStatementID, PageNumber, RowsPerPage);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "DELETE_PATIENT_STATEMENT":
                    {
                        long PatStmtID = MDVUtility.ToLong(context.Request["PatStmtID"]);
                        string strJSONData = DeletePatientStatement(PatStmtID);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;
                case "PATIENT_STATEMENT_XML":
                    {
                        string fieldsJSON = context.Request["data"];
                        string strJSONData = PatientStatementXML(fieldsJSON);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

            }
        }
    }

        #endregion
}
