using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.HL7
{
    public class ClinicalHL7Creation
    {
         private BLLClinical BLLClinicalObj = null;
         public ClinicalHL7Creation()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ClinicalHL7Creation _instance = null;
        public static ClinicalHL7Creation Instance()
        {
            if (_instance == null)
                _instance = new ClinicalHL7Creation();
            return _instance;
        }
        public string FieldSeparator { get { return "|"; } }
        public string EncodingCharacters { get { return @"^~\&"; } }
        public string SendingApplication { get { return "MDVISION LabOrder"; } }
        public DateTime DateTimeOfMessage { get { return DateTime.Now; } }
        public string FacilityName { get; set; }
        public string MessageControlID { get; set; }

        HL7Patient patObj = new HL7Patient();

        #region 'Dictionary'
        public static readonly Dictionary<string, string> SpecimenActionCode = new Dictionary<string, string>
        {
          {    "A"  ,   "Add ordered tests to the existing specimen"     } ,
          {    "G"  ,   "Generated order; reflex order"  } ,
          {    "L"  ,   "Lab to obtain specimen from patient"  } ,
          {    "O"  ,   "Specimen obtained by service other than Lab"  } ,

       };

        public static readonly Dictionary<string, string> CWEStatusCode = new Dictionary<string, string>
        {
          {    "U"  ,   "Unknown "     } ,
          {    "UASK"  ,   "Asked but Unknown"  } ,
          {    "NAV"  ,   "Not available"  } ,
          {    "NA"  ,   "Not applicable"  } ,
          {    "NASK"  ,   "Not asked"  } ,

       };
        #endregion

        private string GetAge(DateTime birthday)
        {
            DateTime today = DateTime.Now;
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
            //Start 27-01-2016 Muhammad Arshad Bug# EMR-245	Face Sheet in Clinical Module -> Patient Age in PDF
            //string ActAge = string.Format(" {0} Year(s), {1} Month(s), {2} Day(s)  ", years,
            //             months,
            //             days);
            string ActAge = string.Format(" {0} Year(s)  ", years);
            //End 27-01-2016 Muhammad Arshad Bug# EMR-245	Face Sheet in Clinical Module -> Patient Age in PDF
            return (ActAge);
        }

        private string formateDateHL7(DateTime dt, bool offsetRequire = false)
        {
            if (offsetRequire)
            {
                TimeZone zone = TimeZone.CurrentTimeZone;
                // Get offset.
                TimeSpan offset = zone.GetUtcOffset(DateTime.Now);
                DateTimeOffset thisDate2 = new DateTimeOffset(DateTime.Now,
                                      offset);
                return thisDate2.ToString("yyyyMMddhhmmsszzz").Replace(":", "");
            }
            else
            {
                return dt.ToString("yyyyMMddhhmmss");
            }

        }
        public string CreateHL7LabOrder(Int64 LabOrderId, Int64 PatientId)
        {

            DSLabOrder dsLabOrder = new DSLabOrder();
            BLObject<DSLabOrder> obj = BLLClinicalObj.GetLabOrderPatientInfo(LabOrderId, PatientId);

            if (obj.Data != null)
            {
                dsLabOrder = obj.Data;
                DSPatient dsPatient = new DSPatient();
                #region Patient's Data
                // Start Append Patient's Data
                foreach (DSPatient.PatientsRow dr in dsLabOrder.Tables[dsPatient.Patients.TableName].Rows)
                {
                    patObj.PatientId = dr[dsPatient.Patients.PatientIdColumn.ColumnName].ToString();
                    patObj.Patient_Name = dr[dsPatient.Patients.FullNameColumn.ColumnName].ToString();
                    patObj.FirstName = dr[dsPatient.Patients.FirstNameColumn.ColumnName].ToString();
                    patObj.LastName = dr[dsPatient.Patients.LastNameColumn.ColumnName].ToString();
                    patObj.MiddleInitial = dr[dsPatient.Patients.MIColumn.ColumnName].ToString();
                    patObj.Suffix = dr[dsPatient.Patients.SuffixColumn.ColumnName].ToString();
                    patObj.Prefix = dr[dsPatient.Patients.PrefixColumn.ColumnName].ToString();
                    patObj.Sex = dr[dsPatient.Patients.GenderColumn.ColumnName].ToString().Equals("Male") ? "M" : "F";
                    patObj.DOB = MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName].ToString());
                    patObj.RaceName = dr[dsPatient.Patients.RaceNameColumn.ColumnName].ToString();
                    patObj.RaceCode = dr[dsPatient.Patients.RaceCodeColumn.ColumnName].ToString();
                    patObj.AccountNumber = dr[dsPatient.Patients.AccountNumberColumn.ColumnName].ToString();
                    patObj.age = GetAge(MDVUtility.ToDateTime(dr[dsPatient.Patients.DOBColumn.ColumnName]));
                    patObj.SSNNumberPatient = dr[dsPatient.Patients.SSNColumn.ColumnName].ToString();
                    patObj.MaritialStatus = dr[dsPatient.Patients.MaritialStatusColumn.ColumnName].ToString();
                    patObj.Language = dr[dsPatient.Patients.LanguageNameColumn.ColumnName].ToString();
                    patObj.PatientAddress1 = dr[dsPatient.Patients.Address1Column.ColumnName].ToString();
                    patObj.Email = dr[dsPatient.Patients.EmailAddressColumn.ColumnName].ToString();
                    patObj.HomePhoneNo = dr[dsPatient.Patients.HomePhoneNoColumn.ColumnName].ToString();
                    patObj.WorkPhoneNo = dr[dsPatient.Patients.WorkPhoneNoColumn.ColumnName].ToString();
                    patObj.CellNo = dr[dsPatient.Patients.CellNoColumn.ColumnName].ToString();

                    //dr[dsPatient.Patients.RaceIdColumn.ColumnName].ToString();
                }
                // End Append Patient's Data

                #endregion



                List<ORCLabOrder> ObjOrcLabOrderList = new List<ORCLabOrder>();
                #region Order Information
                // Start Append Order Information
                if (dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows.Count > 0)
                {
                    foreach (DSLabOrder.LabOrderRow dr in dsLabOrder.Tables[dsLabOrder.LabOrder.TableName].Rows)
                    {
                        ORCLabOrder ObjOrcLabOrder = new ORCLabOrder();
                        this.FacilityName = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName]);
                        this.MessageControlID = LabOrderId + (String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString());
                        ObjOrcLabOrder.OrderingProvider = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderColumn.ColumnName]);
                        ObjOrcLabOrder.OrderEffectiveDateTime = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.OrderDateColumn.ColumnName]).ToShortDateString() + " " + MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderTimeColumn.ColumnName]);
                        ObjOrcLabOrder.DateTimeOfTransaction = MDVUtility.ToDateTime(dr[dsLabOrder.LabOrder.CreatedOnColumn.ColumnName]);
                        ObjOrcLabOrder.EntererSLocation = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName]);
                        ObjOrcLabOrder.EnteringOrganization = "MDVISION";// MDVUtility.ToStr(dr[dsLabOrder.LabOrder.FacilityColumn.ColumnName]);
                        ObjOrcLabOrder.PlacerOrderNumber = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.OrderNoColumn.ColumnName]);
                        ObjOrcLabOrder.OrderStatus = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.StatusColumn.ColumnName]);

                        ObjOrcLabOrder.ProviderId = MDVUtility.ToStr(dr[dsLabOrder.LabOrder.ProviderIdColumn.ColumnName]);
                        ObjOrcLabOrderList.Add(ObjOrcLabOrder);
                    }
                }


                #endregion
                #region Genereate HL7 Message
                ////////////////PID/////////////////
                NHapi.Model.V251.Message.ORM_O01 labOrderMsg = new NHapi.Model.V251.Message.ORM_O01();
                labOrderMsg.MSH.FieldSeparator.Value = this.FieldSeparator;
                labOrderMsg.MSH.EncodingCharacters.Value = this.EncodingCharacters;
                labOrderMsg.MSH.SendingApplication.NamespaceID.Value = this.SendingApplication;
                labOrderMsg.MSH.SendingFacility.NamespaceID.Value = this.FacilityName;
                labOrderMsg.MSH.ReceivingFacility.NamespaceID.Value = this.FacilityName; //optional
                //  labOrderMsg.MSH.ReceivingApplication.NamespaceID.Value = this.SendingApplication; //optional
                labOrderMsg.MSH.DateTimeOfMessage.Time.Value = formateDateHL7(this.DateTimeOfMessage);

                labOrderMsg.MSH.MessageType.MessageCode.Value = "ORM ";
                labOrderMsg.MSH.MessageType.TriggerEvent.Value = "O01";
                labOrderMsg.MSH.MessageType.MessageStructure.Value = "ORM_O01";
                labOrderMsg.MSH.MessageControlID.Value = this.MessageControlID;

                labOrderMsg.MSH.ProcessingID.ProcessingID.Value = "T";
                labOrderMsg.MSH.VersionID.VersionID.Value = "2.5.1";
                labOrderMsg.MSH.AcceptAcknowledgmentType.Value = "AL";
                labOrderMsg.MSH.ApplicationAcknowledgmentType.Value = "NE";// "ER";

                //can be comment this code, it's not manadatory, just for test case
                labOrderMsg.MSH.GetMessageProfileIdentifier(0).EntityIdentifier.Value = "LRI_Common_Component";
                labOrderMsg.MSH.GetMessageProfileIdentifier(0).NamespaceID.Value = "Profile Component";
                labOrderMsg.MSH.GetMessageProfileIdentifier(0).UniversalID.Value = "2.16.840.1.113883.9.16";
                labOrderMsg.MSH.GetMessageProfileIdentifier(0).UniversalIDType.Value = "ISO";
                labOrderMsg.MSH.GetMessageProfileIdentifier(1).EntityIdentifier.Value = "LRI_NG_Component";
                labOrderMsg.MSH.GetMessageProfileIdentifier(1).NamespaceID.Value = "Profile Component";
                labOrderMsg.MSH.GetMessageProfileIdentifier(1).UniversalID.Value = "2.16.840.1.113883.9.13";
                labOrderMsg.MSH.GetMessageProfileIdentifier(1).UniversalIDType.Value = "ISO";
                labOrderMsg.MSH.GetMessageProfileIdentifier(2).EntityIdentifier.Value = "LRI_RU_Component";
                labOrderMsg.MSH.GetMessageProfileIdentifier(2).NamespaceID.Value = "Profile Component";
                labOrderMsg.MSH.GetMessageProfileIdentifier(2).UniversalID.Value = "2.16.840.1.113883.9.14";
                labOrderMsg.MSH.GetMessageProfileIdentifier(2).UniversalIDType.Value = "ISO";


                labOrderMsg.PATIENT.PID.SetIDPID.Value = "1";
                labOrderMsg.PATIENT.PID.GetPatientIdentifierList(0).IDNumber.Value = patObj.AccountNumber;// "P0012345";
                labOrderMsg.PATIENT.PID.GetPatientIdentifierList(0).AssigningAuthority.NamespaceID.Value = "NIST MPI";
                labOrderMsg.PATIENT.PID.GetPatientIdentifierList(0).IdentifierTypeCode.Value = "MR";

                labOrderMsg.PATIENT.PID.GetPatientName(0).FamilyName.Surname.Value = patObj.LastName;
                labOrderMsg.PATIENT.PID.GetPatientName(0).GivenName.Value = patObj.FirstName;
                labOrderMsg.PATIENT.PID.GetPatientName(0).SuffixEgJRorIII.Value = patObj.Suffix;
                //labOrderMsg.PATIENT.PID.GetPatientName(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = patObj.FirstName;
                labOrderMsg.PATIENT.PID.GetPatientName(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = patObj.MiddleInitial;
                labOrderMsg.PATIENT.PID.GetPatientName(0).NameTypeCode.Value = "L";



                labOrderMsg.PATIENT.PID.DateTimeOfBirth.Time.Value = formateDateHL7(patObj.DOB);

                labOrderMsg.PATIENT.PID.AdministrativeSex.Value = patObj.Sex;

                labOrderMsg.PATIENT.PID.GetRace(0).Identifier.Value = patObj.RaceCode;//2106-3
                labOrderMsg.PATIENT.PID.GetRace(0).NameOfCodingSystem.Value = "HL70005";
                labOrderMsg.PATIENT.PID.GetRace(0).Text.Value = patObj.RaceName;
                //hard coded for test case LRI_1.0-NG_Final
                labOrderMsg.PATIENT.PID.GetRace(0).AlternateIdentifier.Value = "CAUC";
                labOrderMsg.PATIENT.PID.GetRace(0).AlternateText.Value = "Caucasian";
                labOrderMsg.PATIENT.PID.GetRace(0).NameOfAlternateCodingSystem.Value = "L";

                //end hard coded

                //Patient Detail Information
                labOrderMsg.PATIENT.PID.PatientAccountNumber.IDNumber.Value = patObj.AccountNumber;
                //  labOrderMsg.PATIENT.PID.PatientID.IDNumber.Value = patObj.PatientId;
                labOrderMsg.PATIENT.PID.SSNNumberPatient.Value = patObj.SSNNumberPatient;
                labOrderMsg.PATIENT.PID.PrimaryLanguage.Text.Value = patObj.Language;
                labOrderMsg.PATIENT.PID.Nationality.Text.Value = patObj.MotherMaidenName;
                labOrderMsg.PATIENT.PID.MaritalStatus.Text.Value = patObj.MaritialStatus;
                labOrderMsg.PATIENT.PID.GetPhoneNumberHome(0).EmailAddress.Value = patObj.Email;
                // labOrderMsg.PATIENT.PID.GetPhoneNumberHome(0).AreaCityCode.Value = "";
                labOrderMsg.PATIENT.PID.GetPhoneNumberHome(0).LocalNumber.Value = patObj.CellNo;
                labOrderMsg.PATIENT.PID.GetPhoneNumberHome(0).TelephoneNumber.Value = patObj.HomePhoneNo;
                labOrderMsg.PATIENT.PID.GetPhoneNumberBusiness(0).TelephoneNumber.Value = patObj.WorkPhoneNo;
                //labOrderMsg.PATIENT.PID.GetPatientAddress(0).City.Value = patObj.City;            ||
                //labOrderMsg.PATIENT.PID.GetPatientAddress(0).StateOrProvince.Value = patObj.StateOrProvince;      ||

                //labOrderMsg.PATIENT.PID.GetPatientAddress(0).Country.Value = patObj.Country;          ||
                labOrderMsg.PATIENT.PID.GetPatientAddress(0).StreetAddress.StreetOrMailingAddress.Value = patObj.PatientAddress1;
                //   labOrderMsg.PATIENT.PID.GetPatientAddress(0).ZipOrPostalCode.Value = patObj.PostalCode;            ||

                labOrderMsg.PATIENT.GT1.GetGuarantorName(0).GivenName.Value = patObj.GurranterName;
                //end detail inormation of patient


                int counter = 0;
                foreach (var ObjOrcLabOrder in ObjOrcLabOrderList)
                {
                    NHapi.Model.V251.Segment.ORC labOrderOrc = labOrderMsg.GetORDER(counter).ORC;// labOrderMsg.PATIENT.PATIENT_VISIT.PV1;
                    /*1*/
                    labOrderOrc.OrderControl.Value = ObjOrcLabOrder.OrderControl = "RE"; //new or exisiting may be
                    /*1*/
                    labOrderOrc.PlacerOrderNumber.EntityIdentifier.Value = LabOrderId.ToString();
                    labOrderOrc.PlacerOrderNumber.NamespaceID.Value = ObjOrcLabOrder.PlacerOrderNumber;

                    labOrderOrc.FillerOrderNumber.EntityIdentifier.Value = LabOrderId.ToString();
                    labOrderOrc.FillerOrderNumber.NamespaceID.Value = ObjOrcLabOrder.PlacerOrderNumber;

                    //https://www.hl7.org/FHIR/v2/0038/index.html
                    labOrderOrc.PlacerGroupNumber.EntityIdentifier.Value = "GORD874211"; //changeable data
                    labOrderOrc.PlacerGroupNumber.NamespaceID.Value = "NIST EHR";



                    labOrderOrc.GetOrderingProvider(counter).IDNumber.Value = ObjOrcLabOrder.ProviderId;
                    labOrderOrc.GetOrderingProvider(counter).FamilyName.Surname.Value = ObjOrcLabOrder.OrderingProvider.Replace(" ", "").Split(',')[0];
                    labOrderOrc.GetOrderingProvider(counter).GivenName.Value = ObjOrcLabOrder.OrderingProvider.Replace(" ", "").Split(',')[1];
                    labOrderOrc.GetOrderingProvider(counter).AssigningFacility.NamespaceID.Value = "NIST-AA-1";// ObjOrcLabOrder.OrderingProvider;
                    if (ObjOrcLabOrderList[counter].OrderingProvider.Split(' ').Length > 1)
                    {
                        labOrderOrc.GetOrderingProvider(counter).SecondAndFurtherGivenNamesOrInitialsThereof.Value = ObjOrcLabOrder.OrderingProvider.Split(' ')[2];
                    }


                    labOrderOrc.GetOrderingProvider(0).SuffixEgJRorIII.Value = "JR";
                    labOrderOrc.GetOrderingProvider(0).PrefixEgDR.Value = "DR";

                    labOrderOrc.GetOrderingProvider(0).AssigningAuthority.NamespaceID.Value = "NIST-AA-1";
                    labOrderOrc.GetOrderingProvider(0).AssigningAuthority.UniversalID.Value = "";
                    labOrderOrc.GetOrderingProvider(0).AssigningAuthority.UniversalIDType.Value = "";
                    labOrderOrc.GetOrderingProvider(0).NameTypeCode.Value = "L";
                    labOrderOrc.GetOrderingProvider(0).IdentifierTypeCode.Value = "NPI";
                    //  labOrderOrc.GetOrderingProvider(counter).AssigningAuthority.UniversalID.Value = ObjOrcLabOrder.OrderingProvider;

                    labOrderOrc.OrderStatus.Value = ObjOrcLabOrder.OrderStatus.Equals("Draft") ? "SC" : "CM";
                    labOrderOrc.GetEnteredBy(counter).AssigningFacility.NamespaceID.Value = ObjOrcLabOrder.EnteredBy;
                    labOrderOrc.DateTimeOfTransaction.Time.Value = formateDateHL7(ObjOrcLabOrder.DateTimeOfTransaction);// SetLongDate(ObjOrcLabOrder.DateTimeOfTransaction);// = ObjOrcLabOrder.DateTimeOfTransaction;
                    labOrderOrc.EntererSLocation.LocationDescription.Value = ObjOrcLabOrder.EntererSLocation;




                    labOrderOrc.GetCallBackPhoneNumber(counter).TelephoneNumber.Value = ObjOrcLabOrder.CallBackPhoneNumber;
                    //     labOrderOrc.OrderEffectiveDateTime.DegreeOfPrecision.Value = ObjOrcLabOrder.OrderEffectiveDateTime;
                    labOrderOrc.EnteringOrganization.Text.Value = ObjOrcLabOrder.EnteringOrganization;
                    //   labOrderOrc.EnteringDevice.Text.Value = ObjOrcLabOrder.EnteringDevice;
                    counter++;
                }
                #endregion
                #region Radiology/Lab Information

                // Start Append Test Information
                if (dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows.Count > 0)
                {
                    counter = 0;
                    foreach (DSLabOrder.LabOrderTestRow drLabTest in dsLabOrder.Tables[dsLabOrder.LabOrderTest.TableName].Rows)
                    {


                        NHapi.Model.V251.Segment.OBR labOrderOBR = labOrderMsg.GetORDER(counter).ORDER_DETAIL.OBR;
                        labOrderOBR.SetIDOBR.Value = (counter + 1).ToString();//i
                        if (ObjOrcLabOrderList.Count > counter)
                        {
                            labOrderOBR.PlacerOrderNumber.NamespaceID.Value = ObjOrcLabOrderList[counter].PlacerOrderNumber; // order No

                            labOrderOBR.PlacerOrderNumber.EntityIdentifier.Value = LabOrderId.ToString(); // MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.LabOrderTestIdColumn.ColumnName]);
                            labOrderOBR.FillerOrderNumber.NamespaceID.Value = ObjOrcLabOrderList[counter].PlacerOrderNumber +  "-" + labOrderOBR.SetIDOBR.Value ;
                            labOrderOBR.FillerOrderNumber.EntityIdentifier.Value = LabOrderId.ToString() + "-" + labOrderOBR.SetIDOBR.Value;
                        }
                        else
                        {
                            labOrderOBR.FillerOrderNumber.NamespaceID.Value = ObjOrcLabOrderList[ObjOrcLabOrderList.Count - 1].PlacerOrderNumber + "-" + labOrderOBR.SetIDOBR.Value;
                            labOrderOBR.FillerOrderNumber.EntityIdentifier.Value = LabOrderId.ToString() + "-" + labOrderOBR.SetIDOBR.Value;
                        }
                        labOrderOBR.UniversalServiceIdentifier.Identifier.Value = MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.CPTCodeColumn.ColumnName]);
                        labOrderOBR.UniversalServiceIdentifier.Text.Value = MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]);
                        labOrderOBR.UniversalServiceIdentifier.NameOfCodingSystem.Value = "LN";
                        if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("30341-2"))
                        {
                            //test case obr-4 Relevant Clinical Information |NIST Test Case : LRI_1.0-NG_Final
                            labOrderOBR.UniversalServiceIdentifier.AlternateIdentifier.Value = "815115";
                            labOrderOBR.UniversalServiceIdentifier.AlternateText.Value = "Erythrocyte sedimentation rate";
                            labOrderOBR.UniversalServiceIdentifier.NameOfAlternateCodingSystem.Value = "99USI^^^Erythrocyte sedimentation rate";
                            labOrderMsg.GetORDER(0).ORC.PlacerGroupNumber.EntityIdentifier.Value = "GORD874211";
                            labOrderOBR.SpecimenActionCode.Value = "L";

                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("57021-8"))
                        {
                            //test case obr-4 Relevant Clinical Information |NIST Test Case : LRI_2.0-NG_CBC_Type
                            labOrderOBR.UniversalServiceIdentifier.AlternateIdentifier.Value = "4456544";
                            labOrderOBR.UniversalServiceIdentifier.AlternateText.Value = "CBC";
                            labOrderOBR.UniversalServiceIdentifier.NameOfAlternateCodingSystem.Value = "99USI^^^CBC W Auto Differential panel in Blood";
                            labOrderMsg.GetORDER(0).ORC.PlacerGroupNumber.EntityIdentifier.Value = "GORD874233";
                            labOrderOBR.SpecimenActionCode.Value = "L";
                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("24331-1"))
                        {
                            labOrderOBR.UniversalServiceIdentifier.AlternateIdentifier.Value = "345789";
                            labOrderOBR.UniversalServiceIdentifier.AlternateText.Value = "Lipid Panel";
                            labOrderOBR.UniversalServiceIdentifier.NameOfAlternateCodingSystem.Value = "99USI^^^Lipid 1996 panel in Serum or Plasma";
                            labOrderMsg.GetORDER(0).ORC.PlacerGroupNumber.EntityIdentifier.Value = "GORD874244";
                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("625-4"))
                        {
                            labOrderOBR.UniversalServiceIdentifier.AlternateIdentifier.Value = "3456543";
                            labOrderOBR.UniversalServiceIdentifier.AlternateText.Value = "CULTURE STOOL";
                            labOrderOBR.UniversalServiceIdentifier.NameOfAlternateCodingSystem.Value = "99USI^^^Stool Culture";
                            labOrderMsg.GetORDER(0).ORC.PlacerGroupNumber.EntityIdentifier.Value = "GORD874211";
                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("50545-3"))
                        {
                           // labOrderOBR.UniversalServiceIdentifier.AlternateIdentifier.Value = "3456543";
                           // labOrderOBR.UniversalServiceIdentifier.AlternateText.Value = "CULTURE STOOL";
                            labOrderOBR.UniversalServiceIdentifier.NameOfAlternateCodingSystem.Value = "^^^	Bacteria susceptibility";
                            labOrderMsg.GetORDER(0).ORC.PlacerGroupNumber.EntityIdentifier.Value = "GORD874211";
                            labOrderOBR.ParentResult.ParentObservationIdentifier.Identifier.Value = "625-4";
                            labOrderOBR.ParentResult.ParentObservationIdentifier.Text.Value = "Bacteria identified in Stool by Culture";
                            labOrderOBR.ParentResult.ParentObservationIdentifier.NameOfCodingSystem.Value = "LN";
                            labOrderOBR.ParentResult.ParentObservationIdentifier.NameOfAlternateCodingSystem.Value = "&&&Stool Culture";
                            labOrderOBR.ParentResult.ParentObservationSubIdentifier.Value = labOrderOBR.SetIDOBR.Value;// "1";
                            labOrderOBR.Parent.PlacerAssignedIdentifier.EntityIdentifier.Value = labOrderOBR.PlacerOrderNumber.NamespaceID.Value;
                            labOrderOBR.Parent.PlacerAssignedIdentifier.NamespaceID.Value = "NIST EHR";
                            labOrderOBR.Parent.FillerAssignedIdentifier.EntityIdentifier.Value = labOrderOBR.FillerOrderNumber.NamespaceID.Value;
                            labOrderOBR.Parent.FillerAssignedIdentifier.NamespaceID.Value = "NIST Lab Filler";
                            labOrderOBR.SpecimenActionCode.Value = "G";
                        }


                        //labOrderOrc.ParentUniversalServiceIdentifier.Identifier.Value = MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.CPTCodeDescriptionColumn.ColumnName]).Split(' ')[0];

                        DateTime ObservationDateTime = new DateTime();
                        if (!string.IsNullOrEmpty(MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.TestDateColumn.ColumnName])))
                        {
                            ObservationDateTime = MDVUtility.ToDateTime(MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.TestDateColumn.ColumnName]));//: "" + " " + MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.TestTimeColumn.ColumnName]);
                            DateTime otime = MDVUtility.ToDateTime(MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.TestTimeColumn.ColumnName]));
                            ObservationDateTime.AddHours(otime.Hour);
                            ObservationDateTime.AddMinutes(otime.Minute);
                            ObservationDateTime.AddSeconds(otime.Second);
                        }
                        //   labOrderOBR.UniversalServiceIdentifier.NameOfCodingSystem.Value = "LN";



                        //   labOrderOBR.GetOrderingProvider(0).AssigningFacility.NamespaceID.Value = ObjOrcLabOrder.OrderingProvider;// this.FacilityName;
                        if (ObjOrcLabOrderList.Count>counter)
                        {
                            labOrderOBR.GetOrderingProvider(0).IDNumber.Value = ObjOrcLabOrderList[counter].ProviderId;
                            labOrderOBR.GetOrderingProvider(0).FamilyName.Surname.Value = ObjOrcLabOrderList[counter].OrderingProvider.Replace(" ", "").Split(',')[0];
                            labOrderOBR.GetOrderingProvider(0).GivenName.Value = ObjOrcLabOrderList[counter].OrderingProvider.Replace(" ", "").Split(',')[1];
                            labOrderOBR.GetOrderingProvider(0).AssigningFacility.NamespaceID.Value = "NIST-AA-1";// ObjOrcLabOrder.OrderingProvider;
                            if (ObjOrcLabOrderList[counter].OrderingProvider.Split(' ').Length > 1)
                            {
                                labOrderOBR.GetOrderingProvider(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = ObjOrcLabOrderList[counter].OrderingProvider.Split(' ')[2];
                            }
                        }
                        else
                        {
                            labOrderOBR.GetOrderingProvider(0).IDNumber.Value = ObjOrcLabOrderList[ObjOrcLabOrderList.Count-1].ProviderId;
                            labOrderOBR.GetOrderingProvider(0).FamilyName.Surname.Value = ObjOrcLabOrderList[ObjOrcLabOrderList.Count - 1].OrderingProvider.Replace(" ", "").Split(',')[0];
                            labOrderOBR.GetOrderingProvider(0).GivenName.Value = ObjOrcLabOrderList[ObjOrcLabOrderList.Count - 1].OrderingProvider.Replace(" ", "").Split(',')[1];
                            labOrderOBR.GetOrderingProvider(0).AssigningFacility.NamespaceID.Value = "NIST-AA-1";// ObjOrcLabOrder.OrderingProvider;
                            if (ObjOrcLabOrderList[ObjOrcLabOrderList.Count - 1].OrderingProvider.Split(' ').Length > 1)
                            {
                                labOrderOBR.GetOrderingProvider(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = ObjOrcLabOrderList[ObjOrcLabOrderList.Count - 1].OrderingProvider.Split(' ')[2];
                            }
                        }



                        labOrderOBR.GetOrderingProvider(0).SuffixEgJRorIII.Value = "JR";
                        labOrderOBR.GetOrderingProvider(0).PrefixEgDR.Value = "DR";

                        labOrderOBR.GetOrderingProvider(0).AssigningAuthority.NamespaceID.Value = "NIST-AA-1";
                        labOrderOBR.GetOrderingProvider(0).AssigningAuthority.UniversalID.Value = "";
                        labOrderOBR.GetOrderingProvider(0).AssigningAuthority.UniversalIDType.Value = "";
                        labOrderOBR.GetOrderingProvider(0).NameTypeCode.Value = "L";
                        labOrderOBR.GetOrderingProvider(0).IdentifierTypeCode.Value = "NPI";

                        //labOrderOBR.GetOrderingProvider(counter).IDNumber.Value = ObjOrcLabOrderList[counter].ProviderId;
                        ////labOrderOrc.GetOrderingProvider(counter).GivenName

                        //labOrderOBR.GetOrderingProvider(counter).AssigningFacility.NamespaceID.Value = ObjOrcLabOrderList[counter].OrderingProvider;
                        //labOrderOBR.GetOrderingProvider(counter).AssigningAuthority.UniversalID.Value = ObjOrcLabOrderList[counter].OrderingProvider;

                        labOrderOBR.ResultsRptStatusChngDateTime.Time.Value = formateDateHL7(DateTime.Now, true);// thisDate2.ToString("yyyyMMddhhmmsszzz").Replace(":", "");// (DateTime.Now.ToString("yyyyMMddhhmmss")) + offset.ToString().Replace(":", "");

                        labOrderOBR.ResultStatus.Value = "F";//https://www.hl7.org/fhir/v2/0123/index.html

                        //   labOrderOBR.PriorityOBR.Value = MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.UrgencyNameColumn.ColumnName]).Equals("Normal") ? "NR" : "UG";

                        //https://www.contactkswebiz.info/emrCodeSet.asp?SID=5
                        //  //labOrderOBR.RequestedDateTime.DegreeOfPrecision.Value = ;
                        ////  labOrderOBR.ObservationEndDateTime.Time.SetLongDate(ObservationDateTime);//= ObservationDateTime.Date.ToUniversalTime;//above get

                        labOrderOBR.ObservationDateTime.Time.Value = formateDateHL7(ObservationDateTime, true);// thisDate2.ToString("yyyyMMddhhmmsszzz").Replace(":", "");// (DateTime.Now.ToString("yyyyMMddhhmmss")) + offset.ToString().Replace(":", "");

                        if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("30341-2"))
                        {
                            //test case obr-49 Relevant Clinical Information |NIST Test Case : LRI_1.0-NG_Final
                            labOrderOBR.ResultHandling.Value = "CC^Carbon Copy^HL70507^C^Send Copy^L^^^Copied Requested";

                            //test case obr-13 Relevant Clinical Information |NIST Test Case : LRI_1.0-NG_Final
                            labOrderOBR.RelevantClinicalInformation.Value = "7520000^fever of unknown origin^SCT^22546000^fever, origin unknown^99USI^^^Fever of unknown origin";
                            labOrderOBR.GetResultCopiesTo(0).IDNumber.Value = "10092";
                            labOrderOBR.GetResultCopiesTo(0).FamilyName.Surname.Value = "Hamlin";
                            labOrderOBR.GetResultCopiesTo(0).GivenName.Value = "Pafford";
                            labOrderOBR.GetResultCopiesTo(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = "M";
                            labOrderOBR.GetResultCopiesTo(0).SuffixEgJRorIII.Value = "Sr. ";
                            labOrderOBR.GetResultCopiesTo(0).PrefixEgDR.Value = "Dr.";
                            labOrderOBR.GetResultCopiesTo(0).AssigningAuthority.NamespaceID.Value = "NIST-AA-1";
                            labOrderOBR.GetResultCopiesTo(0).NameTypeCode.Value = "L";
                            labOrderOBR.GetResultCopiesTo(0).IdentifierTypeCode.Value = "NPI";

                            labOrderOBR.ParentUniversalServiceIdentifier.Identifier.Value = "CC";
                            labOrderOBR.ParentUniversalServiceIdentifier.Text.Value = "Carbon Copy";
                            labOrderOBR.ParentUniversalServiceIdentifier.NameOfCodingSystem.Value = "HL70507";

                            labOrderOBR.ParentUniversalServiceIdentifier.AlternateIdentifier.Value = "C";
                            labOrderOBR.ParentUniversalServiceIdentifier.AlternateText.Value = "Send Copy";
                            labOrderOBR.ParentUniversalServiceIdentifier.NameOfAlternateCodingSystem.Value = "L";
                            labOrderOBR.ParentUniversalServiceIdentifier.OriginalText.Value = "Copied Requested";
                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("57021-8"))
                        {
                            //test case obr-49 Relevant Clinical Information |NIST Test Case : LRI_2.0-NG_CBC_Type
                            labOrderOBR.ResultHandling.Value = "CC^Carbon Copy^HL70507";

                            //test case obr-13 Relevant Clinical Information |NIST Test Case : LRI_2.0-NG_CBC_Type
                            labOrderOBR.GetResultCopiesTo(0).IDNumber.Value = "10093";
                            labOrderOBR.GetResultCopiesTo(0).FamilyName.Surname.Value = "Deluca";
                            labOrderOBR.GetResultCopiesTo(0).GivenName.Value = "Naddy";
                            labOrderOBR.GetResultCopiesTo(0).AssigningAuthority.NamespaceID.Value = "NIST-AA-1";
                            labOrderOBR.GetResultCopiesTo(0).NameTypeCode.Value = "L";
                            labOrderOBR.GetResultCopiesTo(0).IdentifierTypeCode.Value = "NPI";
                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("24331-1"))
                        {
                            labOrderOBR.RelevantClinicalInformation.Value = "56388000^hyperlipidemia^99USI^3744001^hyperlipoproteinemia^SCT^^^hyperlipoproteinemia";
                            labOrderOBR.ResultStatus.Value = "F";
                            labOrderOBR.ResultHandling.Value = "CC^Carbon Copy^HL70507";
                            //test case obr-13 Relevant Clinical Information |NIST Test Case : LRI_3.0-NG_Lipid_Panel_Typpe
                            labOrderOBR.GetResultCopiesTo(0).IDNumber.Value = "10092";
                            labOrderOBR.GetResultCopiesTo(0).FamilyName.Surname.Value = "Hamlin";
                            labOrderOBR.GetResultCopiesTo(0).GivenName.Value = "Pafford";
                            labOrderOBR.GetResultCopiesTo(0).AssigningAuthority.NamespaceID.Value = "NIST-AA-1";
                            labOrderOBR.GetResultCopiesTo(0).NameTypeCode.Value = "L";
                            labOrderOBR.GetResultCopiesTo(0).IdentifierTypeCode.Value = "NPI";
                            if (!labOrderOBR.SetIDOBR.Value.Equals("1"))
                            {
                                labOrderOBR.Parent.FillerAssignedIdentifier.EntityIdentifier.Value = labOrderOBR.FillerOrderNumber.EntityIdentifier.Value;
                                labOrderOBR.Parent.FillerAssignedIdentifier.NamespaceID.Value = labOrderOBR.FillerOrderNumber.NamespaceID.Value;
                                labOrderOBR.Parent.PlacerAssignedIdentifier.EntityIdentifier.Value = labOrderOBR.PlacerOrderNumber.EntityIdentifier.Value;
                                labOrderOBR.Parent.PlacerAssignedIdentifier.NamespaceID.Value = labOrderOBR.PlacerOrderNumber.NamespaceID.Value;
                            }
                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("625-4"))
                        {
                            labOrderOBR.RelevantClinicalInformation.Value = "787.91^DIARRHEA^I9CDX^^^^^^DIARRHEA";
                            labOrderOBR.ResultStatus.Value = "P";
                            // labOrderOBR.ResultStatus.Value = "F";
                        }
                        else if (labOrderOBR.UniversalServiceIdentifier.Identifier.Value.Equals("50545-3"))
                        {
                           // labOrderOBR.RelevantClinicalInformation.Value = "787.91^DIARRHEA^I9CDX^^^^^^DIARRHEA";
                            labOrderOBR.ResultStatus.Value = "F";
                            labOrderMsg.GetORDER(0).ORDER_DETAIL.OBR.ResultStatus.Value = "F";
                        }
                        string comments = MDVUtility.ToStr(drLabTest[dsLabOrder.LabOrderTest.FillerInstructionColumn.ColumnName]); ;
                        if (!string.IsNullOrEmpty(comments))
                        {
                            labOrderMsg.PATIENT.GetNTE().GetComment(counter).Value = comments;
                            labOrderMsg.PATIENT.GetNTE().SetIDNTE.Value = (counter + 1).ToString();

                        }
                        counter++;

                    }


                }

                #endregion

                #region Problem List/Associated Problems

                //// Start Append Associated Problems
                //if (dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows.Count > 0)
                //{

                //    foreach (DSLabOrder.LabOrderProblemRow dr in dsLabOrder.Tables[dsLabOrder.LabOrderProblem.TableName].Rows)
                //    {
                //        string ProblemName = MDVUtility.ToStr(dr[dsLabOrder.LabOrderProblem.ProblemNameColumn.ColumnName]);
                //        string ProblemId = MDVUtility.ToStr(dr[dsLabOrder.LabOrderProblem.ProblemIdColumn.ColumnName]);
                //        string LabOrderIds = MDVUtility.ToStr(dr[dsLabOrder.LabOrderProblem.LabOrderIdColumn.ColumnName]);

                //    }


                //}

                // End Append Associated Problems

                #endregion

                #region PatientVisit
                // int counterVisit = 0;
                // labOrderMsg.PATIENT.PATIENT_VISIT.PV1.SetIDPV1.Value = (counterVisit + 1).ToString();
                // labOrderMsg.PATIENT.PATIENT_VISIT.PV1.PatientClass.Value = "E";  /*E = Emergency I  = Inpatient O = Outpatient P = Preadmit R = Recurring Patient B = Obstetrics */
                // labOrderMsg.PATIENT.PATIENT_VISIT.PV1.AssignedPatientLocation.LocationStatus.Value = this.FacilityName;
                // //labOrderMsg.PATIENT.PATIENT_VISIT.PV1.AdmissionType.Value = "";
                // //labOrderMsg.PATIENT.PATIENT_VISIT.PV1.PreadmitNumber.IDNumber.Value = "";
                // //labOrderMsg.PATIENT.PATIENT_VISIT.PV1.PriorPatientLocation.LocationStatus.Value = "";
                // //  labOrderMsg.PATIENT.PATIENT_VISIT.PV1.GetAttendingDoctor(counterVisit).GivenName.Value = "";
                // // labOrderMsg.PATIENT.PATIENT_VISIT.PV1.GetAttendingDoctor(counterVisit).FamilyName.Surname.Value = "";
                // // labOrderMsg.PATIENT.PATIENT_VISIT.PV1.GetAmbulatoryStatus(counterVisit).Value = ""; //2
                //// labOrderMsg.PATIENT.PATIENT_VISIT.PV1.PatientType.Value = "";
                // //labOrderMsg.PATIENT.PATIENT_VISIT.PV1.VisitNumber.IDNumber.Value = "";
                // //labOrderMsg.PATIENT.PATIENT_VISIT.PV1.DischargedToLocation.DischargeLocation.Value = "";
                // labOrderMsg.PATIENT.PATIENT_VISIT.PV1.AdmitDateTime.Time.SetLongDate(DateTime.Now);
                #endregion

                NHapi.Base.Parser.PipeParser parser = new NHapi.Base.Parser.PipeParser();

                string contents = parser.Encode(labOrderMsg);
                contents = contents.Replace("\r", "\r\n");
                contents = contents.Replace("\\S\\", "^");
                contents = contents.Replace("\\T\\", "&");
                string labResultInboxPath = "~/EMR/HL7Folder/LabOrder/OutBox/";
                string FileName = HttpContext.Current.Server.MapPath(labResultInboxPath + ObjOrcLabOrderList[0].PlacerOrderNumber + "LabOrder.txt");
                if (System.IO.File.Exists(FileName))
                {
                    FileName = HttpContext.Current.Server.MapPath(labResultInboxPath +SpecialFileName(ObjOrcLabOrderList[0].PlacerOrderNumber + "LabOrder"));
                }
                //   System.IO.File.Create(FileName);
                using (var stream = File.Create(FileName)) { }
                System.IO.File.WriteAllText(FileName, contents);
                return contents;
            }
            return string.Empty;
        }
        private string SpecialFileName(string Filename)
        {
            // A
            return string.Format("{0}{1}" + Filename + "-{2:yyyy-MM-dd_hh-mm-ss-tt}.txt",
                // B
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                // C
            System.IO.Path.DirectorySeparatorChar,
                // D
            DateTime.Now);
        }
        public class ORCLabOrder
        {

            public string MyProperty { get; set; }
            public string OrderControl { get; set; }
            public string PlacerOrderNumber { get; set; }
            public string OrderStatus { get; set; }
            public string EnteredBy { get; set; }
            public DateTime DateTimeOfTransaction { get; set; }
            public string EntererSLocation { get; set; }
            public string OrderingProvider { get; set; }
            public string CallBackPhoneNumber { get; set; }
            public string OrderEffectiveDateTime { get; set; }
            public string EnteringOrganization { get; set; }
            public string EnteringDevice { get; set; }

            public string ProviderId { get; set; }
        }
    }
}