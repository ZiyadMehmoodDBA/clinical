using System;
using System.Collections.Generic;
using MDVision.Datasets;
//using MDVision.IEHR.Controls.Batch.CQM;
using System.Data;
using MDVision.Business.BLL;
using System.Linq;
using MDVision.Common.Utilities;
using MDVision.IEHR.EMR.Model.PQRS;

namespace MDVision.IEHR.EMR.Model.QRDA
{
    public class PQRS_IndividualReportingModels
    {
    }

    public class PQRS_IndividualReportingSearchModel
    {
        public int PageNumber { get; set; }
        public int RowsPerPage { get; set; }
        public string commandType { get; set; }
        public string ProviderId { get; set; }
        public string SpecialityId { get; set; }
        public string IsActive { get; set; }
        public Int64 MeasureIndividualId { get; set; }
        
    }

    public class PQRS_IndividualReportingFillModel
    {
        public string MeasureIndividualId { get; set; }
        public string SubmissionYear { get; set; }
        public string PerformanceYear { get; set; }
        public string MeasureGroupId_text { get; set; }
        public string MemberProvider { get; set; }
        public string MeasureGroupId { get; set; }
        public string GroupName { get; set; }
        public string PageNumber{ get; set; }
        public string RowspPage { get; set; }
        public Int64 EntityId { get; set; }
        public bool IsReported { get; set; }
        public bool IsActive { get; set; }
        public string ProviderId { get; set; }
        public string MeasureIds { get; set; }
        public string PracticeIds { get; set; }
        public string SpecialityId { get; set; }
        public string CQMMeasureIds { get; set; }
        public string VBPMeasureIds { get; set; }
        public string IAMeasureIds { get; set; }
        public string CreatedOn { get; set; }
        public string TotalProviders { get; set; }
        public string is_Active { get; set; }
        public string RecordCount { get; set; }
    }

    public class PQRS_MeasureIndividualSelectModel
    {
        public string providerName { get; set; }
        public string SubmissionYear { get; set; }
        public string specialityName { get; set; }
        public string createdOn { get; set; }
        public Int64 measureIndividualId { get; set; }
        public bool IsActive { get; set; }
        public bool IsReported { get; set; }
    }
    public class QRDAXMLDocumentLevelTemplates
    {
        static DSPQRS _dsPatientDemoGraphic;
        static DSProfile _dsProvider;
        static DSProfile _dsPractice;

        static ClinicalDocument _document;

        public void BuildDocumentLevelTemplate(ClinicalDocument document, DSPQRS dsPatientDemoGraphic, DSProfile dsProvider, DSProfile dsPractice)
        {
            _document = document;
            _dsPatientDemoGraphic = dsPatientDemoGraphic;
            _dsProvider = dsProvider;
            _dsPractice = dsPractice;

            DocumentLevelTemplates();
        }
        private void DocumentLevelTemplates()
        {
            SetHeader_Template();
            SetPatient_RecordTarget();
            Person_Author();
            Custodian();
            LegalAuthenticator();
            DocumentOff();

        }

        #region General Header
        private static void SetHeader_Template()
        {
            _document.realmCode = new List<CS>();
            _document.realmCode.Add(new CS() { code = "US" });

            _document.typeId = new InfrastructureRoottypeId
            {
                root = "2.16.840.1.113883.1.3",
                extension = "POCD_HD000040"
            };
            _document.templateId = new List<II>
            {
                 new II
                {
                    root = "2.16.840.1.113883.10.20.22.1.1"
                    //,extension = "Which Time will be displayed Here ??????????" //"2014-06-09"
                },
                new II
                {
                    root = "2.16.840.1.113883.10.20.24.1.1"
                    //,extension = "Which Time will be displayed Here ??????????" //"2014-06-09"
                },
                new II
                {
                    root = "2.16.840.1.113883.10.20.24.1.2"
                    //,extension = "Which Time will be displayed Here ??????????" //"2014-06-09"
                },
                new II
                {
                    root = "2.16.840.1.113883.10.20.24.1.3"
                    //,extension = "Which Time will be displayed Here ??????????" //"2014-06-09"
                }
            };
            _document.id = new II
            {
                root = Guid.NewGuid().ToString()
            };
            //_document.versionNumber = new INT { value=""};

            _document.code = new CE
            {
                code = "55182-0",
                displayName = "Quality Measure Report",
                codeSystem = "2.16.840.1.113883.6.1",
                codeSystemName = "LOINC"
            };

            _document.title = new ST()
            {
                Text = new List<string> { "Physician Quality Reporting System (PQRS) Individual QRDA-I" }
            };

            _document.effectiveTime = new IVL_TS
            {
                value = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            _document.confidentialityCode = new CE
            {
                code = "N",
                displayName = "normal",
                codeSystem = "2.16.840.1.113883.5.25",
                codeSystemName = "Confidentiality"

            };
            _document.languageCode = new CS
            {
                code = "en-US",
                displayName = "2.16.840.1.113883.1.11.1152",
                codeSystemName = "Internet Society Language",
            };
            //_document.
        }
        #endregion

        #region record target
        private static void SetPatient_RecordTarget()
        {
            if (_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows.Count > 0)
            {
                var patientFirstName =
                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                        _dsPatientDemoGraphic.PatientPQRS.FirstNameColumn.ColumnName].ToString();

                var patientLastName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientPQRS.LastNameColumn.ColumnName].ToString();

                var phone = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientPQRS.HomePhoneNoColumn.ColumnName].ToString();
                string RaceCode = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                           ][
                                               _dsPatientDemoGraphic.PatientPQRS.RaceCodeColumn.ColumnName].ToString();
                CE raceCodeInfo = new CE();
                if (string.IsNullOrEmpty(RaceCode))
                {
                    RaceCode = "ASKU";
                    raceCodeInfo.nullFlavor = RaceCode;
                }
                else
                {
                    raceCodeInfo.code = RaceCode;
                    raceCodeInfo.displayName =
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                        ][
                                            _dsPatientDemoGraphic.PatientPQRS.RaceDescriptionColumn.ColumnName]
                                        .ToString();
                    raceCodeInfo.codeSystem = "2.16.840.1.113883.6.238";
                    raceCodeInfo.codeSystemName = "Race & Ethnicity - CDC";
                }
                CE ethnicGroupCode = new CE();
                string ethnicCOde = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.EthnicityCodeColumn.ColumnName]
                                            .ToString();
                if (string.IsNullOrEmpty(ethnicCOde))
                {
                    ethnicGroupCode.nullFlavor = "ASKU";
                }
                else if (ethnicCOde.Equals("UNK"))
                {

                    ethnicGroupCode.nullFlavor = ethnicCOde;
                }
                else
                {
                    ethnicGroupCode.code =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.EthnicityCodeColumn.ColumnName]
                                            .ToString();
                    ethnicGroupCode.displayName =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.EthnicityDescriptionColumn.ColumnName]
                                            .ToString();
                    ethnicGroupCode.codeSystem = "2.16.840.1.113883.6.238";
                    ethnicGroupCode.codeSystemName = "Race & Ethnicity - CDC";
                }



                if (!string.IsNullOrEmpty(phone))
                    if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-")))
                        phone = String.Format("{0:(###) ###-####}", double.Parse(phone));

                //The recordTarget records the administrative and demographic data of the patient whose health information is described by the clinical document; each recordTarget must contain at least one patientRole element
                _document.recordTarget = new List<RecordTarget>()
                {
                    //SHALL contain at least one [1..*] recordTarget (CONF:1098-5266).
                    new RecordTarget()
                    {
                        //Such recordTargets SHALL contain exactly one [1..1] patientRole (CONF:1098-5267).
                        patientRole =new PatientRole ()
                        {
                            //This patientRole SHALL contain at least one [1..*] id (CONF:16858).
                            id = new List<II>
                            {
                                //new II
                                //{
                                //    root = "2.16.840.1.113883.4.572",
                                //    extension = "111223333A"
                                //},
                                new II
                                {
                                    root = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientPQRS.AccountNumberColumn.ColumnName].ToString(),
                                   // extension = "112233"
                                }
                            },
                            //This patientRole SHALL contain at least one [1..*] US Realm Address (AD.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.2) (CONF:1098-5271).
                            addr = new List<AD>()
                            {
                                SetAddress(
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientPQRS.Address1Column.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientPQRS.CityColumn.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientPQRS.StateColumn.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientPQRS.ZIPCodeColumn.ColumnName].ToString(),
                                    "US")
                            },
                            //This patientRole SHALL contain at least one [1..*] telecom (CONF:1098-5280).
                            //1. Such telecoms SHOULD contain zero or one [0..1] @use, which SHALL be selected from ValueSet Telecom Use (US Realm Header) urn:oid:2.16.840.1.113883.11.20.9.20 DYNAMIC (CONF:1098-5375).
                            telecom = new List<TEL>()
                            {
                                new TEL
                                {
                                    value =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.HomePhoneNoColumn.ColumnName].ToString
                                            () == ""
                                            ? ""
                                            : "tel: +" + phone,
                                    use = new List<string> {"HP"}
                                }
                            },
                            patient =new MDVision.IEHR.EMR.Model.PQRS.Patient()
                            {
                                //This patient SHALL contain at least one [1..*] US Realm Person Name (PN.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.1.1) (CONF:1098-5284).
                                name = new List<PN>()
                                {
                                    new PN()
                                    {
                                        Items = new List<ENXP>
                                        {
                                            new engiven {Text = new List<string> {patientFirstName}},
                                            new enfamily {Text = new List<string> {patientLastName}}
                                        }
                                    }
                                },
                                //This patient SHALL contain exactly one [1..1] administrativeGenderCode, which SHALL be selected from ValueSet Administrative Gender (HL7 V3) urn:oid:2.16.840.1.113883.1.11.1 DYNAMIC (CONF:1098-6394).
                                administrativeGenderCode = new CE
                                {
                                    code =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.GenderColumn.ColumnName].ToString()
                                            .Substring(0, 1),
                                    displayName =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.GenderColumn.ColumnName].ToString(),
                                    codeSystem = "2.16.840.1.113883.5.1",
                                    codeSystemName = "AdministrativeGender"
                                },
                                //This patient SHALL contain exactly one [1..1] birthTime (CONF:1098-5298).
                                //a. SHALL be precise to year (CONF:1098-5299).
                                //b. SHOULD be precise to day (CONF:1098-5300).
                                //For cases where information about newborn's time of birth needs to be captured.
                                //c. MAY be precise to the minute (CONF:1098-32418).
                                birthTime = new TS
                                {
                                    value =
                                        Convert.ToDateTime(
                                            _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName]
                                                .Rows[0][
                                                    _dsPatientDemoGraphic.PatientPQRS.DOBColumn.ColumnName].ToString())
                                            .ToString("yyyyMMdd")
                                },
                                //This patient SHOULD contain zero or one [0..1] maritalStatusCode, which SHALL be selected from ValueSet Marital Status urn:oid:2.16.840.1.113883.1.11.12212 DYNAMIC (CONF:1098-5303).
                                maritalStatusCode = new CE
                                {
                                    code =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.MaritialStatusColumn.ColumnName]
                                            .ToString()
                                            .Substring(0, 1),
                                    displayName =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientPQRS.MaritialStatusColumn.ColumnName]
                                            .ToString(),
                                    codeSystem = "2.16.840.1.113883.5.2",
                                    codeSystemName = "MaritalStatusCode"
                                },
                               
                                //This patient SHALL contain exactly one [1..1] raceCode, which SHALL be selected from ValueSet Race Category Excluding Nulls urn:oid:2.16.840.1.113883.3.2074.1.1.3 DYNAMIC (CONF:1098-5322).
                                raceCode = raceCodeInfo,
                                //This patient SHALL contain exactly one [1..1] ethnicGroupCode, which SHALL be selected from ValueSet Ethnicity urn:oid:2.16.840.1.114222.4.11.837 DYNAMIC (CONF:1098-5323).
                                ethnicGroupCode = ethnicGroupCode,
                                //This patient SHALL contain at least one [1..*] languageCommunication (CONF:1098-5406).
                                //a. Such languageCommunications SHALL contain exactly one [1..1] languageCode, which SHALL be selected from ValueSet PatientLanguage urn:oid:2.16.840.1.113883.11.20.9.64 DYNAMIC (CONF:1098-5407).
                                //b. Such languageCommunications MAY contain zero or one [0..1] modeCode, which SHALL be selected from ValueSet LanguageAbilityMode urn:oid:2.16.840.1.113883.1.11.12249 DYNAMIC (CONF:1098-5409).
                                //c. Such languageCommunications SHOULD contain zero or one [0..1] proficiencyLevelCode, which SHALL be selected from ValueSet LanguageAbilityProficiency urn:oid:2.16.840.1.113883.1.11.12199 DYNAMIC (CONF:1098-9965).
                                //d. Such languageCommunications SHOULD contain zero or one [0..1] preferenceInd (CONF:1098-5414).
                                languageCommunication =new List<LanguageCommunication>()
                                {
                                    new LanguageCommunication
                                    {
                                        languageCode = new CS()
                                        {
                                            code =
                                                _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName
                                                    ].Rows[0][
                                                        _dsPatientDemoGraphic.PatientPQRS.LanguageCodeColumn.ColumnName]
                                                    .ToString()
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
        #endregion



        #region Author
        //The author element represents the creator of the clinical document. The author may be a device or a person.
        private static void Person_Author()
        {
            _document.author = new List<Author>();
            var objAuthor = new Author();

            //SHALL contain at least one [1..*] author (CONF:1098-5444).
            _document.author.Add(objAuthor);

            //Such authors SHALL contain exactly one [1..1] US Realm Date and Time (DTM.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.4)
            objAuthor.time = new TS
            {
                value = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            //Such authors SHALL contain exactly one [1..1] assignedAuthor (CONF:1098-5448).
            objAuthor.assignedAuthor = new AssignedAuthor
            {
                //This assignedAuthor SHALL contain at least one [1..*] id (CONF:1098-5449).
                id = new List<II>
                {
                    new II
                    {
                        root = "2.16.840.1.113883.4.6",
                        extension =
                            string.IsNullOrEmpty(
                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                    _dsProvider.Provider.NPIColumn.ColumnName].ToString())
                                ? "123456789"
                                : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                    _dsProvider.Provider.NPIColumn.ColumnName].ToString()
                    }
                },

                //This assignedAuthor SHOULD contain zero or one [0..1] code (CONF:1098-16787).
                code = new CE
                {
                    code = "163W00000X",
                    displayName = "Registered nurse",
                    codeSystem = "2.16.840.1.113883.5.53",
                    codeSystemName = "Health Care Provider Taxonomy"
                },
                addr = new List<AD>()
            };

            //This assignedAuthor SHALL contain at least one [1..*] US Realm Address (AD.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.2) (CONF:1098-5452).
            var addrs =
                SetAddress(
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                        _dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString() == ""
                        ? "N/A"
                        : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                            _dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString(),
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                        _dsProvider.Provider.CityColumn.ColumnName].ToString() == ""
                        ? "N/A"
                        : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                            _dsProvider.Provider.CityColumn.ColumnName].ToString(),
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                        _dsProvider.Provider.StateColumn.ColumnName].ToString() == ""
                        ? "N/A"
                        : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                            _dsProvider.Provider.StateColumn.ColumnName].ToString(),
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                        _dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString() == ""
                        ? "N/A"
                        : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                            _dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString(),
                    "US");

            objAuthor.assignedAuthor.addr.Add(addrs);

            //This assignedAuthor SHALL contain at least one [1..*] telecom (CONF:1098-5428).
            var phone =
                _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientPQRS.HomePhoneNoColumn.ColumnName].ToString();
            if (!string.IsNullOrEmpty(phone))
                if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-")))
                    phone = String.Format("{0:(###) ###-####}", double.Parse(phone));

            objAuthor.assignedAuthor.telecom = new List<TEL>();
            var objTel = new TEL
            {
                value =
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                        _dsProvider.Provider.PhoneNoColumn.ColumnName].ToString()
                    == ""
                        ? ""
                        : "tel: +" + phone,
                use = new List<string> { "HP" } // Best Recomendations : Should

            };
            objAuthor.assignedAuthor.telecom.Add(objTel);

            //This assignedAuthor SHOULD contain zero or one [0..1] assignedPerson (CONF:1098-5430).
            Person objPerson = new Person();
            objAuthor.assignedAuthor.Item = objPerson;

            objPerson.name = new List<PN>
            {
                new PN
                {
                    Items = new List<ENXP>
                    {
                        new engiven
                        {
                            Text =
                                new List<string>
                                {
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                        _dsProvider.Provider.FirstNameColumn.ColumnName].ToString()
                                }
                        },
                        new enfamily
                        {
                            Text =
                                new List<string>
                                {
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                        _dsProvider.Provider.LastNameColumn.ColumnName].ToString()
                                }
                        }
                    }
                }
            };
            //This assignedAuthor SHOULD contain zero or one [0..1] assignedAuthoringDevice (CONF:1098-16783).
            // !Understood
        }
        private static void TestMethod()
        {
            _document.author = new List<Author>();
            Author objAuthor = new Author();
            _document.author.Add(objAuthor);
            objAuthor.time = new TS
            {
                value = "20161231000000+0500"
            };
            objAuthor.assignedAuthor = new AssignedAuthor
            {
                id = new List<II>
                {
                    new II
                    {
                        root = "2.16.840.1.113883.19.5", //NPI
                        extension = "Which Number will be displayed Here ??????????" //"KP00017dev"
                    }
                },
                code = new CE
                {
                    code = "200000000X",
                    codeSystem = "2.16.840.1.113883.6.101",
                    displayName = "Allopathic Osteopathic Physicians"
                },
                addr = new List<AD>()
            };


            AD objAdress = SetAddress("AddressLine1", "City", "State", "ZipCode", "Country");
            objAuthor.assignedAuthor.addr.Add(objAdress);

            objAuthor.assignedAuthor.telecom = new List<TEL>();
            TEL objTelePhone = new TEL
            {
                use = new List<string> { "WP" },
                value = "tel: +" + "(000)000-0000"
            };
            objAuthor.assignedAuthor.telecom.Add(objTelePhone);

            AuthoringDevice objAssignedAuthorDevice = new AuthoringDevice();
            objAuthor.assignedAuthor.Item = objAssignedAuthorDevice;
            objAssignedAuthorDevice.manufacturerModelName = new SC
            {
                Text = new List<string>
                {
                    "Some Name"
                }
            };

            objAssignedAuthorDevice.softwareName = new SC
            {
                Text = new List<string>
                {
                    "Some Name"
                }
            };

        }

        #endregion

        #region Custodian
        private static void Custodian()
        {
            //SHALL contain exactly one [1..1] custodian (CONF:1098-5519).
            _document.custodian = new Custodian
            {
                assignedCustodian = new AssignedCustodian
                {
                    //This assignedCustodian SHALL contain exactly one [1..1] representedCustodianOrganization (CONF:1098-5521).
                    representedCustodianOrganization = new CustodianOrganization
                    {
                        //This representedCustodianOrganization SHALL contain at least one [1..*] id (CONF:1098-5522).
                        id = new List<II>
                        {
                            new II
                            {
                                root = "2.16.840.1.113883.4.6",
                                extension = string.IsNullOrEmpty(
                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                    _dsProvider.Provider.NPIColumn.ColumnName].ToString())
                                ? "123456789"
                                : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                    _dsProvider.Provider.NPIColumn.ColumnName].ToString()
                            }
                        },
                        //This representedCustodianOrganization SHALL contain exactly one [1..1] name (CONF:1098-5524).
                        name = new ON
                        {
                            Text = new List<string> { _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.DescriptionColumn.ColumnName].ToString() }
                        },
                        //This representedCustodianOrganization SHALL contain exactly one [1..1] telecom (CONF:1098-5525).
                        telecom = new TEL()
                    }
                }
            };
            //var objCustodian = new Custodian();

            //This custodian SHALL contain exactly one [1..1] assignedCustodian (CONF:1098-5520).
            var phone = _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.PhoneNoColumn.ColumnName].ToString();
            if (!string.IsNullOrEmpty(phone))
                if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-")))
                    phone = String.Format("{0:(###) ###-####}", double.Parse(phone));

            var telObj = new TEL
            {
                value = _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.PhoneNoColumn.ColumnName].ToString() == "" ? ""
                : "tel: +" + phone,
                use = new List<string> { "WP" }
            };

            _document.custodian.assignedCustodian.representedCustodianOrganization.telecom = telObj;

            //This representedCustodianOrganization SHALL contain exactly one [1..1] US Realm Address (AD.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.2) (CONF:1098-5559).
            _document.custodian.assignedCustodian.representedCustodianOrganization.addr = new AD();
            var objAddr = SetAddress(
                                    _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.AddressColumn.ColumnName].ToString(),
                                    _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.CityColumn.ColumnName].ToString(),
                                    _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.StateColumn.ColumnName].ToString(),
                                    _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.ZIPCodeColumn.ColumnName].ToString(),
                                    "US"
                                    );

            _document.custodian.assignedCustodian.representedCustodianOrganization.addr = objAddr;
        }
        #endregion

        #region Legal Authenticator
        private static void LegalAuthenticator()
        {
            _document.legalAuthenticator = new LegalAuthenticator
            {
                time = new TS
                {
                    value = DateTime.Now.ToString("yyyyMMddHHmmss")
                },
                signatureCode = new CS
                {
                    code = "S"
                },
                assignedEntity = new AssignedEntity
                {
                    //This assignedEntity SHALL contain at least one [1..*] id (CONF:1098-5586).
                    //1. Such ids MAY contain zero or one [0..1] @root="2.16.840.1.113883.4.6" National Provider Identifier (CONF:1098-16823).
                    id = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.4.6",
                            extension = _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.NPIColumn.ColumnName].ToString()
                        }
                    },
                    //This assignedEntity MAY contain zero or one [0..1] code, which SHOULD be selected from ValueSet Healthcare Provider Taxonomy (HIPAA) urn:oid:2.16.840.1.114222.4.11.1066 DYNAMIC (CONF:1098-17000).
                    //code = new CE
                    //{
                    //    code = "207QA0505X",
                    //    displayName = "Adult Medicine",
                    //    codeSystem = "2.16.840.1.113883.5.53",
                    //    codeSystemName = "Health Care Provider Taxonomy"
                    //},
                    //This assignedEntity SHALL contain at least one [1..*] US Realm Address (AD.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.2) (CONF:1098-5589).
                    addr = new List<AD>()
                }
            };

            //The legalAuthenticator, if present, SHALL contain exactly one [1..1] US Realm Date and Time (DTM.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.4) (CONF:1098-5580).

            //The legalAuthenticator, if present, SHALL contain exactly one [1..1] signatureCode (CONF:1098-5583).
            //i. This signatureCode SHALL contain exactly one [1..1] @code="S" (CodeSystem: Participationsignature urn:oid:2.16.840.1.113883.5.89 STATIC) (CONF:1098-5584).

            //The legalAuthenticator, if present, MAY contain zero or one [0..1] sdtc:signatureText (CONF:1098-30810).
            // !important

            //The legalAuthenticator, if present, SHALL contain exactly one [1..1] assignedEntity (CONF:1098-5585).

            var objAddr = SetAddress(
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString(),
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn.ColumnName].ToString(),
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn.ColumnName].ToString(),
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString(),
                                    "US"
                                    );
            _document.legalAuthenticator.assignedEntity.addr.Add(objAddr);

            //This assignedEntity SHALL contain at least one [1..*] telecom (CONF:1098-5595).
            _document.legalAuthenticator.assignedEntity.telecom = new List<TEL>
            {
                new TEL
                {
                    use = new List<string> { "WP" },
                    value = _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn].ToString() == "" ?
                    "" :
                    string.Format("{0:(###) ###-####}", "tel:" + _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn])
                }
            };

            //This assignedEntity SHALL contain exactly one [1..1] assignedPerson (CONF:1098-5597).
            _document.legalAuthenticator.assignedEntity.assignedPerson = new Person
            {
                //This assignedPerson SHALL contain at least one [1..*] US Realm Person Name (PN.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.1.1) (CONF:1098-5598).
                name = new List<PN>
                {
                    new PN
                    {
                        Items = new List<ENXP>
                        {
                            new engiven {Text = new List<string> { _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.FirstNameColumn.ColumnName].ToString() }},
                            new enfamily {Text = new List<string> { _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.LastNameColumn.ColumnName].ToString() }}
                        }
                    }
                }
            };
            _document.legalAuthenticator.assignedEntity.representedOrganization = new Organization()
            {
                name = new List<ON>()
                {
                    new ON()
                    {
                        Text = new List<string>()
                        {
                            _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.DescriptionColumn.ColumnName].ToString() == "" ? "" : _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.DescriptionColumn.ColumnName].ToString()
                        }
                    }
                }
            };

        }
        #endregion

        #region Document Off
        private static void DocumentOff()
        {
            DateTime patientDob =
                Convert.ToDateTime(_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientPQRS.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientPQRS.DOBColumn.ColumnName].ToString());

            _document.documentationOf = new List<DocumentationOf>
            {
                new DocumentationOf
                {
                    typeCode = "DOC",
                    serviceEvent = new ServiceEvent
                    {
                        classCode = "PCPR",
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS
                                {
                                    //<!-- The low value represents when the summarized provision of care began.
                                    //In this scenario, the patient's date of birth -->
                                    value = patientDob.ToString("yyyyMMdd")
                                },
                                new IVXB_TS
                                {
                                    //<!-- The high value represents when the summarized provision of care being ended.
                                    //In this scenario, when chart summary was created -->
                                    value = DateTime.Now.ToString("yyyyMMdd")
                                }
                            }

                        },
                        performer = new List<Performer1>
                        {
                            new Performer1
                            {
                                typeCode = x_ServiceEventPerformer.PRF,
                                assignedEntity = new AssignedEntity
                                {
                                    id = new List<II>
                                    {
                                        new II
                                        {
                                            root = "2.16.840.1.113883.4.6",
                                            extension = _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.NPIColumn.ColumnName].ToString()
                                        }
                                    },
                                    code = new CE()
                                    {
                                        code = string.IsNullOrEmpty(
                                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                                    _dsProvider.Provider.TaxonomyCodeColumn.ColumnName].ToString())
                                                ? "123456789"
                                                : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                                    _dsProvider.Provider.TaxonomyCodeColumn.ColumnName].ToString(),
                                        codeSystem = "2.16.840.1.113883.5.53",
                                        codeSystemName = "Health Care Provider Taxonomy",
                                        displayName = "Adult Medicine" // What will Appear Here  ??????????
                                    },
                                    representedOrganization = new Organization
                                    {
                                        id = new List<II>
                                        {
                                            new II
                                            {
                                                extension = "1234567",
                                                root = "2.16.840.1.113883.4.2",
                                                assigningAuthorityName = "TIN"
                                            },
                                            new II
                                            {
                                                extension = "54321",
                                                root = "2.16.840.1.113883.4.336",
                                                assigningAuthorityName = "CCN"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
        #endregion

        #region Address Setter
        private static AD SetAddress(string addressLine1, string city, string state, string zipCode, string country)
        {
            var address = new AD
            {
                Items = new List<ADXP>
                {
                    new adxpstreetAddressLine { Text = new List<string> { addressLine1 } },
                    new adxpcity { Text = new List<string> { city } },
                    new adxpstate { Text = new List<string> { state } },
                    new adxppostalCode { Text = new List<string> { zipCode } },
                    new adxpcountry { Text = new List<string> { string.IsNullOrEmpty(country) ? "US" : country } }
                }
            };
            return address;
        }
        #endregion

    }

    public class QRDXMLSectionLevelTemplates// : CatagoryOneXMLGenerator
    {

        private static DSPQRS _dsMeasureSection;
        //   private static DSCQM _dsReportingParameterSection;

        private static DSCQM _dsPatientDataSection;
        //  private static DSCQM _dsPatientDataSectionCodes;

        //private static DSCQM _dsDiagnosisActiveConcernAct;
        //private static DSCQM _dsFamilyHx;
        //private static DSCQM _dsMedicationActive;
        //private static DSCQM _dsMedicationAdministered;
        //private static DSCQM _dsMedicationAllergy;
        //private static DSCQM _dsMedicationOrder;
        //private static DSCQM _dsProcedureOrder;

        private static ClinicalDocument _document;

        private StructuredBody StructuredBody
        {
            get { return _document.component.Item as StructuredBody; }
        }

        public void BuildSectionLevelTemplate(ClinicalDocument document, DSPQRS dsPatientDemoGraphic,
            DSProfile dsProvider, DSProfile dsPractice,
            string measureID, Int64 patientId, Int64 providerId, Int64 practiceId, string startDate, string endDate, DSPQRS dsMeasureSection)
        {
            _document = document;

            #region InitializeSegments

            #region InitializeMeasureSection

            _dsMeasureSection = dsMeasureSection;

            #endregion

            #region InitializeReportingParameterSection

            //_dsReportingParameterSection = GetReportingParameterSection_CategoryOne(providerId, measureID);


            #endregion

            #region InitializePatientDataSection

            _dsPatientDataSection = GetPatientDataSection_CategoryOne(patientId, measureID);
            //    _dsPatientDataSectionCodes = PatientDataSectionCodes_CategoryOne(providerId, startDate, endDate, patientId.ToString(), 2, measureID);

            #endregion

            #endregion

            SectionLevelTemplates(startDate, endDate, measureID);
        }

        private void SectionLevelTemplates(string startDate, string endDate, string measureID)
        {
            ReportingParameterSection(startDate, endDate);
            MeasureSection(measureID);
            setDataSectionsDetail(measureID);
        }


        #region Data Capture for MeasureSection, ReportingParameterSection and PatientDataSection

        #region Measure Section

        private static DSPQRS GetMeasureSection_CategoryOne(Int64 providerId, string measureID)
        {
            //var obj = new BLLPQRS().Load_CQM(providerId, null, null, null, 0, measureID);
            //_dsMeasureSection = obj.Data;
            return _dsMeasureSection;
        }

        #endregion

        #region Reporting Parameter Section

        //private static DSCQM GetReportingParameterSection_CategoryOne(Int64 providerId, string nqfId)
        //{
        //    // var obj = new BLLCQM().ReportingParameterSection(providerId, nqfId);
        //    //_dsReportingParameterSection = obj.Data;
        //    return _dsReportingParameterSection;
        //}

        #endregion

        #region PatientData Section
        private static DSCQM GetPatientDataSection_CategoryOne(Int64 patientId, string nqfId)
        {
            var obj = new BLLCQM().PatientDataSection(patientId, nqfId);
            _dsPatientDataSection = obj.Data;
            return _dsPatientDataSection;
        }

        //private static DSCQM PatientDataSectionCodes_CategoryOne(long providerId, string from, string to,
        //    string patientId = null, long reportType = 2, string cqmId = null)
        //{
        //    const int measurePart = 0;
        //    if (cqmId == "0421a")
        //    {
        //        cqmId = "0421";
        //        // measurePart = 1;
        //    }
        //    else if (cqmId == "0421b")
        //    {
        //        cqmId = "0421";
        //        // measurePart = 2;
        //    }

        //    //var obj = new BLLCQM().Load_CQM_Codes(providerId, from, to, patientId, reportType, cqmId, measurePart
        //    //);
        //    //_dsPatientDataSectionCodes = obj.Data;
        //    return _dsPatientDataSectionCodes;
        //}

        #endregion

        #endregion

        #region Implemenatation for MeasureSection, ReportingParameterSection and PatientDataSection

        #region MeasureSection

        private void MeasureSection(string measureID)
        {
            if (measureID == "0421")
            {
                measureID = "0421a";
            }
            DataView dvMeasureSection = new DataView(_dsMeasureSection.Tables[_dsMeasureSection.PQRS.TableName])
            {
                RowFilter = "[MeasureId] = '" + measureID + "'"
            };
            DataTable dtMeasureSection = dvMeasureSection.ToTable();

            if (dtMeasureSection.Rows.Count > 0)
            {
                #region Header Fixed

                var versionNeutralidentifier = Guid.NewGuid().ToString();

                var objComponent3 = new Component3
                {
                    section = new Section()
                    {
                        templateId = new List<II>
                        {
                            //<!-- This is the templateId for Measure Section -->
                            new II
                            {
                                root = "2.16.840.1.113883.10.20.24.2.2",
                            },
                            //<!-- This is the templateId for Measure Section QDM -->
                            new II
                            {
                                root = "2.16.840.1.113883.10.20.24.2.3"
                            }
                        },
                        //<!-- This is the LOINC code for "Measure document". This stays the same for all measure section required by QRDA standard -->
                        code = new CE
                        {
                            code = "55186-1",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        },
                        title = new ST
                        {
                            Text = new List<string> { "Measure Section" }
                        },
                        text = new StrucDocText()
                        {
                            Items = new List<object>()
                        }
                    }
                };

                #endregion

                #region Text Node

                var strucDocTable = new StrucDocTable();
                objComponent3.section.text.Items.Add(strucDocTable);

                strucDocTable.border = "1";
                strucDocTable.width = "100%";

                strucDocTable.thead = new StrucDocThead { tr = new List<StrucDocTr>() };

                var strucDocTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTh {Text = new List<string> {"eMeasure Version Number"}},
                        new StrucDocTh {Text = new List<string> {"eMeasure Title"}},
                        new StrucDocTh {Text = new List<string> {"NQF eMeasure Number"}},
                        new StrucDocTh {Text = new List<string> {"Version neutral identifier"}},
                        new StrucDocTh {Text = new List<string> {"eMeasure Identifier (MAT)"}},
                        new StrucDocTh {Text = new List<string> {"Version specific identifier"}}
                    }
                };
                strucDocTable.thead.tr.Add(strucDocTr);
                strucDocTable.tbody = new List<StrucDocTbody>();

                var objStrucDocTbody = new StrucDocTbody();
                strucDocTable.tbody.Add(objStrucDocTbody);

                objStrucDocTbody.tr = new List<StrucDocTr>();
                var bodyObjStrucDocTr = new StrucDocTr
                {
                    Items = new List<object>
                    {
                        new StrucDocTd
                        {
                            Text =
                                new List<string>
                                {
                                    dtMeasureSection.Rows[0]["eMeasureVersionNumber"].ToString()
                                }
                        },
                        new StrucDocTd
                        {
                            Text = new List<string>
                            {
                                dtMeasureSection.Rows[0]["Measure"].ToString()
                            }
                        },
                        new StrucDocTd
                        {
                            Text =
                                new List<string>
                                {
                                    dtMeasureSection.Rows[0]["CQMID"].ToString()
                                }
                        },
                        new StrucDocTd {Text = new List<string> {versionNeutralidentifier}},
                        new StrucDocTd
                        {
                            Text =
                                new List<string>
                                {
                                    dtMeasureSection.Rows[0]["eMeasureIdentifier"].ToString()
                                }
                        },
                        new StrucDocTd
                        {
                            Text =
                                new List<string>
                                {
                                    dtMeasureSection.Rows[0]["VersionSpecificIdentifier"].ToString()
                                }
                        }
                    }
                };
                objStrucDocTbody.tr.Add(bodyObjStrucDocTr);
                StructuredBody.component.Add(objComponent3);

                #endregion

                #region Entry Node

                objComponent3.section.entry = new List<Entry>();
                var objEntry = new Entry();
                objComponent3.section.entry.Add(objEntry);

                objEntry.Item = new Organizer()
                {
                    classCode = x_ActClassDocumentEntryOrganizer.CLUSTER,
                    moodCode = "EVN",
                    templateId = new List<II>
                    {
                        //<!-- This is the templateId for Measure Reference -->
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.98"
                        },
                        //<!-- This is the templateId for eMeasure Reference QDM -->
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.97"
                        }
                    },
                    id = new List<II>
                    {
                        new II
                        {
                            root = Guid.NewGuid().ToString()
                            //extension = "db2d7bfe-af58-4886-9e14-dd6bd50ae630"
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },

                    #region Reference
                    reference = new List<Reference>()
                    {
                        new Reference()
                        {
                            typeCode = x_ActRelationshipExternalReference.REFR,
                            Item = new ExternalDocument()
                            {
                                moodCode = "EVN",
                                classCode = "DOC",
                                id = new List<II>
                                {
                                    //<!-- SHALL: This is the version specific identifier for eMeasure: QualityMeasureDocument/id - the OID in the @root indicates that the @extension (which is a GUID) contains the version specific identifier for eMeasure -->
                                    new II
                                    {
                                        root = "2.16.840.1.113883.4.738",
                                        //Note: This OID indicates that the @extension contains the version specific identifier for the eMeasure.
                                        extension = dtMeasureSection.Rows[0]["VersionSpecificIdentifier"].ToString()
                                    },
                                    //<!-- SHOULD: This is the NQF Number, root is an NQF OID and for eMeasure Number and extension is the eMeasure's NQF number -->
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.3.560.1",
                                        extension = dtMeasureSection.Rows[0]["CQMID"].ToString()
                                    },
                                    //<!-- SHOULD: eMeasure Measure Authoring Tool Identifier (not the real root yet-->
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.3.560.101.2",
                                        extension = dtMeasureSection.Rows[0]["eMeasureIdentifier"].ToString()
                                    }
                                },
                                //<!-- SHOULD This is the title of the eMeasure -->
                                text = new ED
                                {
                                    Text = new List<string>
                                    {
                                        dtMeasureSection.Rows[0]["Measure"].ToString()
                                    }
                                },
                                //<!-- SHOULD: setId is the eMeasure version neutral id -->
                                setId = new II
                                {
                                    root = versionNeutralidentifier
                                },
                                //<!-- This is the sequential eMeasure Version number -->
                                versionNumber = new INT
                                {
                                    value = dtMeasureSection.Rows[0]["eMeasureVersionNumber"].ToString()
                                }
                            }
                        }
                    }
                    #endregion
                };

                #endregion

            }
        }

        #endregion

        #region ReportingParameterSection
        private void ReportingParameterSection(string startDate, string endDate)
        {
            var objComponent3 = new Component3
            {
                section = new Section
                {
                    #region Header Fixed

                    classCode = "DOCSECT",
                    moodCode = "EVN",
                    templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.17.2.1"
                        }
                    },
                    code = new CE
                    {
                        code = "55187-9",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    },
                    title = new ST
                    {
                        Text = new List<string> { "Reporting Parameters" }
                    },

                    #endregion

                    #region Text

                    text = new StrucDocText
                    {
                        mediaType = "text/x-hl7-text+xml",
                        Items = new List<object>()
                        {
                            new StrucDocList()
                            {
                                item = new List<StrucDocItem>()
                                {
                                    new StrucDocItem()
                                    {
                                        Text = new List<string>()
                                        {
                                            "Reporting period: " +(string.IsNullOrEmpty(startDate)?string.Empty:
                                            DateTime.Parse(startDate).ToString("yyyyMMdd")
                                            + " - " )+(string.IsNullOrEmpty(endDate)?string.Empty:
                                            DateTime.Parse(endDate).ToString("yyyyMMdd"))
                                        }
                                    }
                                }
                            }
                        }
                    },

                    #endregion

                    #region Entry

                    entry = new List<Entry>()
                    {
                        new Entry
                        {
                            typeCode = x_ActRelationshipEntry.DRIV,
                            Item = new Act()
                            {
                                moodCode = x_DocumentActMood.EVN,
                                classCode = x_ActClassDocumentEntryAct.ACT,
                                templateId = new List<II>
                                {
                                    new II
                                    {
                                        root = "2.16.840.1.113883.10.20.17.3.8"
                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD
                                {
                                    code = "252116004",
                                    displayName = "Observation Parameters",
                                    codeSystem = "2.16.840.1.113883.6.96"
                                    //codeSystemName = "SNOMED_CT"
                                },
                                effectiveTime = new IVL_TS
                                {
                                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                    Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = string.IsNullOrEmpty(startDate)?string.Empty:DateTime.Parse(startDate).ToString("yyyyMMdd")
                                        },
                                        new IVXB_TS
                                        {
                                            value = string.IsNullOrEmpty(endDate)?string.Empty:DateTime.Parse(endDate).ToString("yyyyMMdd")
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            };
            StructuredBody.component.Add(objComponent3);
        }
        #endregion

        #region PatientDataSection

        private static StrucDocTr GetPatientDataSection_TRs(string description, string codes, string time, string status, string results, string fields)
        {
            var bodyObjStrucDocTr = new StrucDocTr
            {
                Items = new List<object>
                {
                    new StrucDocTd() {Text = new List<string> {description}},
                    new StrucDocTd() {Text = new List<string> {codes}},
                    new StrucDocTd() {Text = new List<string> {time}},
                    new StrucDocTd() {Text = new List<string> {status}},
                    new StrucDocTd() {Text = new List<string> {results}},
                    new StrucDocTd() {Text = new List<string> {fields}}
                }
            };
            return bodyObjStrucDocTr;
        }

        private void setDataSectionsDetail(string measureID) {
            //objStrucDocTbody.tr = new List<StrucDocTr>();
            //objComponent3.section.entry = new List<Entry>();

            if (measureID == "0421a" || measureID == "0421b")
            {
                measureID = "0421";
            }
            if (measureID == "001" || measureID == "0075")
            {
                PatientDataSection(measureID, "Diagnoses");
                PatientDataSection(measureID, "Encounters");
                PatientDataSection(measureID, "Laboratory Tests");
             
            }
            //if (measureID == "0022")
            //{
            //    PatientDataSection(measureID, "Diagnoses");
            //    PatientDataSection(measureID, "Encounters");
            //    MedicationOrder(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0018")
            //{
            //    PatientDataSection(measureID, "Diagnoses");
            //    PatientDataSection(measureID, "Encounters");
            //    PyhsicalExam(objStrucDocTbody, objComponent3);
            //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0028")
            //{
            //    PatientDataSection(measureID, "Diagnoses");
            //    PatientDataSection(measureID, "Encounters");
            //    MedicationOrder(objStrucDocTbody, objComponent3);
            //    RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            //    TobbacoUser(objStrucDocTbody, objComponent3);
            //}
           
            //else if (measureID == "0419")
            //{
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0421")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    PhysicalExam_421(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0043")
            //{
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    MedicationAdministered_CVX(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0068")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    MedicationActive(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0418")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0041")
            //{
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "CMS50v3")
            //{
            //    EncounterPerformed_OnlyCPT(objStrucDocTbody, objComponent3);
            //    InterventionPerformed(objStrucDocTbody, objComponent3);
            //    ProviderToProvider(objStrucDocTbody, objComponent3);
            //}
            //else
            //{
            //    //    DiagnosisActiveConcernActPatch(objStrucDocTbody, objComponent3);
            //    //    FamilyHx(objStrucDocTbody, objComponent3);
            //    //    MedicationActive(objStrucDocTbody, objComponent3);
            //    //    MedicationAdministered(objStrucDocTbody, objComponent3);
            //    //    MedicationAllergy(objStrucDocTbody, objComponent3);
            //    //    MedicationOrder(objStrucDocTbody, objComponent3);
            //    //    ProcedureOrder(objStrucDocTbody, objComponent3);
            //    //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //    //    RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            //    //    TobbacoUser(objStrucDocTbody, objComponent3);
            //}
        }
        private void PatientDataSection(string measureID, string TitleSection)
        {
            #region Add Root

            var objComponent3 = new Component3
            {
                section = new Section
                {
                    moodCode = "EVN",
                    classCode = "DOCSECT",
                    templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.17.2.4"
                        }
                        ,
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.2.1"
                        }
                    },
                    // ToASK
                    code = new CE
                    {
                        code = "55188-7",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    },
                    title = new ST
                    {
                        Text = new List<string> { TitleSection }
                    },
                    text = new StrucDocText
                    {
                        mediaType = "text/x-hl7-text+xml",
                        Items = new List<object>()
                    },
                    entry = new List<Entry>()
                }
            };

            #endregion

            #region Table Header

            var strucDocTable = new StrucDocTable();
            objComponent3.section.text.Items.Add(strucDocTable);

            strucDocTable.border = "1";
            strucDocTable.width = "100%";


            strucDocTable.thead = new StrucDocThead { tr = new List<StrucDocTr>() };

            var strucDocTr = new StrucDocTr
            {
                Items = new List<object>
                {
                    new StrucDocTh {Text = new List<string> {"Description"}},
                    new StrucDocTh {Text = new List<string> {"Codes"}},
                    new StrucDocTh {Text = new List<string> {"Time"}},
                    new StrucDocTh {Text = new List<string> {"Status"}},
                    new StrucDocTh {Text = new List<string> {"Results"}},
                    new StrucDocTh {Text = new List<string> {"Fields"}},
                }
            };
            strucDocTable.thead.tr.Add(strucDocTr);
            strucDocTable.tbody = new List<StrucDocTbody>();

            var objStrucDocTbody = new StrucDocTbody();
            strucDocTable.tbody.Add(objStrucDocTbody);

            #endregion

            SetComponenetsDetail(objStrucDocTbody, objComponent3, measureID, TitleSection);
            StructuredBody.component.Add(objComponent3);
        }
        private static void SetComponenetsDetail(StrucDocTbody objStrucDocTbody, Component3 objComponent3, string measureID, string DataSection)
        {
            objStrucDocTbody.tr = new List<StrucDocTr>();
            objComponent3.section.entry = new List<Entry>();

            
                if (DataSection.Equals("Diagnoses"))
                {
                    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                }
                else if (DataSection.Equals("Encounters")) { EncounterPerformed_Any(objStrucDocTbody, objComponent3); }
                else if (DataSection.Equals("Laboratory Tests")) { LabOrder(objStrucDocTbody, objComponent3); }
                else
                {
                    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                    LabOrder(objStrucDocTbody, objComponent3);
                }

           
            //if (measureID == "0022")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    MedicationOrder(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0018")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    PyhsicalExam(objStrucDocTbody, objComponent3);
            //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0028")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    MedicationOrder(objStrucDocTbody, objComponent3);
            //    RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            //    TobbacoUser(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0075")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    LabOrder(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0419")
            //{
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0421")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    PhysicalExam_421(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0043")
            //{
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    MedicationAdministered_CVX(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0068")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    MedicationActive(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0418")
            //{
            //    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "0041")
            //{
            //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //}
            //else if (measureID == "CMS50v3")
            //{
            //    EncounterPerformed_OnlyCPT(objStrucDocTbody, objComponent3);
            //    InterventionPerformed(objStrucDocTbody, objComponent3);
            //    ProviderToProvider(objStrucDocTbody, objComponent3);
            //}
            //else
            //{
            //    //    DiagnosisActiveConcernActPatch(objStrucDocTbody, objComponent3);
            //    //    FamilyHx(objStrucDocTbody, objComponent3);
            //    //    MedicationActive(objStrucDocTbody, objComponent3);
            //    //    MedicationAdministered(objStrucDocTbody, objComponent3);
            //    //    MedicationAllergy(objStrucDocTbody, objComponent3);
            //    //    MedicationOrder(objStrucDocTbody, objComponent3);
            //    //    ProcedureOrder(objStrucDocTbody, objComponent3);
            //    //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
            //    //    ProcedurePerformed(objStrucDocTbody, objComponent3);
            //    //    RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            //    //    TobbacoUser(objStrucDocTbody, objComponent3);
            //}
        }
        private static void SetComponenets(StrucDocTbody objStrucDocTbody, Component3 objComponent3, string measureID, string DataSection)
        {
            objStrucDocTbody.tr = new List<StrucDocTr>();
            objComponent3.section.entry = new List<Entry>();

            if (measureID == "0421a" || measureID == "0421b")
            {
                measureID = "0421";
            }
            if (measureID == "001")
            {
                if (DataSection.Equals("Diagnoses"))
                {
                    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                }
                else if (DataSection.Equals("Encounters")) { EncounterPerformed_Any(objStrucDocTbody, objComponent3); }
                else if (DataSection.Equals("Laboratory Tests")) { LabOrder(objStrucDocTbody, objComponent3); }
                else
                {
                    DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                    LabOrder(objStrucDocTbody, objComponent3);
                }

            }
            if (measureID == "0022")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0018")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                PyhsicalExam(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0028")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                TobbacoUser(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0075")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                LabOrder(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0419")
            {
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0421")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                PhysicalExam_421(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0043")
            {
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationAdministered_CVX(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0068")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationActive(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0418")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "0041")
            {
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
            }
            else if (measureID == "CMS50v3")
            {
                EncounterPerformed_OnlyCPT(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);
                ProviderToProvider(objStrucDocTbody, objComponent3);
            }
            else
            {
                //    DiagnosisActiveConcernActPatch(objStrucDocTbody, objComponent3);
                //    FamilyHx(objStrucDocTbody, objComponent3);
                //    MedicationActive(objStrucDocTbody, objComponent3);
                //    MedicationAdministered(objStrucDocTbody, objComponent3);
                //    MedicationAllergy(objStrucDocTbody, objComponent3);
                //    MedicationOrder(objStrucDocTbody, objComponent3);
                //    ProcedureOrder(objStrucDocTbody, objComponent3);
                //    EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                //    ProcedurePerformed(objStrucDocTbody, objComponent3);
                //    RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                //    TobbacoUser(objStrucDocTbody, objComponent3);
            }
        }

        #region PatientDataSection SetComponenets
        private static void DiagnosisActiveConcernAct(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.DiagnosisActiveConcernAct.TableName].Rows.Count <= 0)
                return;

            foreach (DataRow dr in _dsPatientDataSection.Tables[_dsPatientDataSection.DiagnosisActiveConcernAct.TableName].Rows)
            {


                string SnomedId = MDVUtility.ToStr(dr["SNOMEDID"]);
                string Snomed_Desc = MDVUtility.ToStr(dr["SNOMED_DESCRIPTION"]);
                string ICD9 = MDVUtility.ToStr(dr["ICD9"]);
                string ICD10 = MDVUtility.ToStr(dr["ICD10"]);
                string ICD9_desc = MDVUtility.ToStr(dr["ICD9_Description"]);
                string ICD10_desc = MDVUtility.ToStr(dr["ICD10_Description"]);
                string diagnosisStartDate = (string)dr["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMddhhmm")
                            : DateTime.Parse(dr["StartDate"].ToString()).ToString("yyyyMMddhhmm");
                string status = Convert.ToBoolean(dr["IsActive"].ToString()) ? "active" : "inactive";
                string results = string.Empty;
                string fields = string.Empty;

                string codes = (string.IsNullOrEmpty(SnomedId) ? "" : "SNOMED: " + SnomedId) +
                    (string.IsNullOrEmpty(ICD9) ? "" : "ICD-9: " + ICD9) +
                    (string.IsNullOrEmpty(ICD10) ? "" : "ICD-10: " + ICD10);
                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Diagnosis, Active "
                                , codes, diagnosisStartDate, status, null, null
                            )
                    );

                objComponent3.section.entry.Add(SetDiagnosisActiveConcernAct(dr));

            }
        }
        private static void DiagnosisActiveConcernActPatch(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.DiagnosisActiveConcernAct.TableName;

            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var onSetDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.DiagnosisActiveConcernAct.StartDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.DiagnosisActiveConcernAct.StartDateColumn].ToString()).ToString("yyyyMMdd");
                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Diagnosis Active: ",
                //                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.DiagnosisActiveConcernAct.SNOMED_DESCRIPTIONColumn].ToString(),
                //                onSetDate
                //            )
                //    );

                objComponent3.section.entry.Add(SetDiagnosisActiveConcernActPatch(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void FamilyHx(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows.Count; i++)
            {
                //objStrucDocTbody.tr.Add
                //    (
                //        //GetPatientDataSection_TRs
                //        //    (
                //        //        "Diagnosis Family History: ",
                //        //        _dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows[i][_dsPatientDataSection.FamilyHx.FamilyHx_FamilyMemberDescriptionColumn].ToString(),
                //        //        DateTime.Parse(_dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows[i][_dsPatientDataSection.FamilyHx.CreatedOnColumn].ToString()).ToString("yyyyMMdd")
                //        //    )
                //    );
                objComponent3.section.entry.Add(SetFamilyHx(_dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows[i]));
            }
        }
        //private static void MedicationActiveOld(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        //{
        //    var tableName = _dsPatientDataSection.MedicationActive.TableName;
        //    if (_dsPatientDataSection.Tables[tableName].Rows.Count > 0)
        //    {
        //        for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
        //        {
        //            var medicationStartDate =
        //                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationActive.MedicationStartDateColumn].ToString() == ""
        //                    ? ""
        //                    : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationActive.MedicationStartDateColumn].ToString()).ToString("yyyyMMdd");

        //            objStrucDocTbody.tr.Add
        //                (
        //                    GetPatientDataSection_TRs
        //                        (
        //                            "Medication Active: ",
        //                            _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationActive.DrugDrugDescriptionColumn]
        //                            + " " +
        //                            _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationActive.MedicationActiveColumn]
        //                            + " " +
        //                            _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationActive.MedicationDoseColumn]
        //                            + " " +
        //                            _dsPatientDataSection.Tables[tableName].Rows[i][
        //                                _dsPatientDataSection.MedicationActive.MedicationUnitColumn]
        //                            + " " +
        //                            _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationActive.MedicationRouteByColumn]
        //                            + " " +
        //                            _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationActive.MedicationDoseTimingColumn],
        //                            medicationStartDate
        //                        )
        //                );
        //            objComponent3.section.Entry.Add(SetMedicationActiveNew(_dsPatientDataSection.Tables[tableName].Rows[i]));
        //        }
        //    }
        //}
        private static void MedicationActive(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var medicationCodesRowCollection = _dsPatientDataSection.Tables[
                _dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Drug"
                        &&
                        r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "RxnormID"
                        &&
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
                        "Po1");

            if (medicationCodesRowCollection.Any())
            {
                var dtProcedureCode = medicationCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var medicationStartDate =
                        (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        //GetPatientDataSection_TRs
                    //        //    (
                    //        //        "Medication Active: " +
                    //        //        dtProcedureCodes.Rows[i]["ICDDescription"],
                    //        //        dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                    //        //        medicationStartDate
                    //        //    )
                    //    );

                    objComponent3.section.entry.Add(SetMedicationActive(dtProcedureCodes.Rows[i]));
                }
            }
        }
        private static void MedicationAdministered(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.MedicationAdministered.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0)
                return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var medicationStartDate =
                    _dsPatientDataSection.Tables[tableName].Rows[i][
                        _dsPatientDataSection.MedicationAdministered.MedicationStartDateColumn].ToString() == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(
                            _dsPatientDataSection.Tables[tableName].Rows[i][
                                _dsPatientDataSection.MedicationAdministered.MedicationStartDateColumn].ToString())
                            .ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Medication Administered: ",
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.MedicationAdministered.DrugDrugDescriptionColumn]
                //                + " " +
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.MedicationAdministered.MedicationActiveColumn]
                //                + " " +
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.MedicationAdministered.MedicationDoseColumn]
                //                + " " +
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.MedicationAdministered.MedicationUnitColumn]
                //                + " " +
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.MedicationAdministered.MedicationRouteByColumn]
                //                + " " +
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.MedicationAdministered.MedicationDoseTimingColumn],
                //                medicationStartDate
                //            )
                //    );

                objComponent3.section.entry.Add(
                    SetMedicationAdministered(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void MedicationAdministered_CVX(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var medicationCodesRowCollection = _dsPatientDataSection.Tables[
                _dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Vaccine" &&
                        r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "CVX" &&
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
                        "Po1");

            if (medicationCodesRowCollection.Any())
            {
                var dtProcedureCode = medicationCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var medicationStartDate =
                        (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        GetPatientDataSection_TRs
                    //            (
                    //                "Medication Administered: " +
                    //                dtProcedureCodes.Rows[i]["ICDDescription"],
                    //                dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                    //                medicationStartDate
                    //            )
                    //    );
                    objComponent3.section.entry.Add(SetMedicationAdministered_CVX(dtProcedureCodes.Rows[i]));
                }
            }

            //var tableName = _dsPatientDataSection.MedicationAdministered_CVX.TableName;
            //if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0)
            //    return;

            //for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            //{
            //    var medicationStartDate =
            //        _dsPatientDataSection.Tables[tableName].Rows[i][
            //            _dsPatientDataSection.MedicationAdministered_CVX.AdministrationDateColumn].ToString() == ""
            //            ? DateTime.Now.ToString("yyyyMMdd")
            //            : DateTime.Parse(
            //                _dsPatientDataSection.Tables[tableName].Rows[i][
            //                    _dsPatientDataSection.MedicationAdministered_CVX.AdministrationDateColumn].ToString())
            //                .ToString("yyyyMMdd");
            //    objStrucDocTbody.tr.Add
            //        (
            //            GetPatientDataSection_TRs
            //                (
            //                    "Medication Administered: ",
            //                    _dsPatientDataSection.Tables[tableName].Rows[i][
            //                        _dsPatientDataSection.MedicationAdministered_CVX.CVXShortDescriptionColumn].ToString
            //                        (),
            //                    medicationStartDate
            //                )
            //        );

            //    objComponent3.section.Entry.Add(
            //        SetMedicationAdministered_CVX(_dsPatientDataSection.Tables[tableName].Rows[i]));
            //}
        }
        private static void MedicationAllergy(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.MedicationAllergy.TableName;

            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var onSetDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationAllergy.OnSetDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationAllergy.OnSetDateColumn].ToString()).ToString("yyyyMMdd");
                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Medication Allergy: ",
                //                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationAllergy.AllergenColumn].ToString(),
                //                onSetDate
                //            )
                //    );

                objComponent3.section.entry.Add(SetMedicationAllergy(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void MedicationOrder(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {

            if (_dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var medicationOrderRowCollection = _dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                    .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Drug"
                        && r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "RxnormID"
                        && r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            if (medicationOrderRowCollection.Any())
            {
                var dtMedicationOrder = medicationOrderRowCollection.CopyToDataTable();

                var dvMedicationOrder = new DataView(dtMedicationOrder);
                var dtMedicationOrderRe = dvMedicationOrder.ToTable(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate", "valueset");

                for (var i = 0; i < dtMedicationOrderRe.Rows.Count; i++)
                {
                    var medicationStartDate = (string)dtMedicationOrderRe.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtMedicationOrderRe.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        GetPatientDataSection_TRs
                    //            (
                    //                "Medication Order: " +
                    //                dtMedicationOrderRe.Rows[i]["ICDDescription"],
                    //                dtMedicationOrderRe.Rows[i]["ICDDescription"].ToString(),
                    //                medicationStartDate
                    //            )
                    //    );
                    objComponent3.section.entry.Add(
                        SetMedicationOrder(dtMedicationOrderRe.Rows[i]));
                }
            }

            //for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            //{
            //    var medicationStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.MedicationStartDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.MedicationStartDateColumn].ToString()).ToString("yyyyMMdd");

            //    objStrucDocTbody.tr.Add
            //        (
            //            GetPatientDataSection_TRs
            //                (
            //                    "Medication Order: ",
            //                    _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.DrugDrugDescriptionColumn] + " " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.MedicationActiveColumn] + " " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.MedicationDoseColumn] + " " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.MedicationUnitColumn] + " " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.MedicationRouteByColumn] + " " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationOrder.MedicationDoseTimingColumn],
            //                    medicationStartDate
            //                )
            //        );

            //    objComponent3.section.Entry.Add(SetMedicationOrder(_dsPatientDataSection.Tables[tableName].Rows[i]));
            //}
        }
        private static void EncounterPerformed_Any(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.EncounterPerformed.TableName].Rows.Count <= 0)
                return;
            {
                foreach (DataRow dr in _dsPatientDataSection.Tables[_dsPatientDataSection.EncounterPerformed.TableName].Rows)
                {


                    string procedureStartDate = (string)dr["ProcedureStartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd  hh:mm")
                        : DateTime.Parse(dr["ProcedureStartDate"].ToString()).ToString("yyyyMMdd hh:mm");

                    string procedureEndDate = (string)dr["ProcedureEndDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd  hh:mm")
                        : DateTime.Parse(dr["ProcedureEndDate"].ToString()).ToString("yyyyMMdd hh:mm");
                    string CPTCode = MDVUtility.ToStr(dr["CPTCode"]);
                    string CPTCode_desc = MDVUtility.ToStr(dr["CPTDescription"]);
                    string ICD9 = MDVUtility.ToStr(dr["ICD9"]);
                    string ICD10 = MDVUtility.ToStr(dr["ICD10"]);
                    string SnomedId = MDVUtility.ToStr(dr["SNOMEDID"]);
                    string Snomed_Desc = MDVUtility.ToStr(dr["SNOMEDDescription"]);

                    string ICD9_desc = MDVUtility.ToStr(dr["ICD9Description"]);
                    string ICD10_desc = MDVUtility.ToStr(dr["ICD10Description"]);
                    string codes = (string.IsNullOrEmpty(CPTCode) ? "" : "CPT: " + CPTCode) +
                        (string.IsNullOrEmpty(SnomedId) ? "" : "SNOMED: " + SnomedId) +
                    (string.IsNullOrEmpty(ICD9) ? "" : "ICD-9: " + ICD9) +
                    (string.IsNullOrEmpty(ICD10) ? "" : "ICD-10: " + ICD10);


                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Encounter, Performed", codes, procedureStartDate + " - " + procedureEndDate, "performed", null, null

                                )
                        );
                    //objComponent3.section.entry.Add(
                    //    SetEncounterPerformed(dr));
                }
            }
        }
        private static void EncounterPerformed_OnlyCPT(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSection.Tables[
                _dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Procedure_N"
                        &&
                        r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "CPTCode");
            //&& r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1"

            if (proceduresCodesRowCollection.Any())
            {
                var dtProcedureCode = proceduresCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset");
                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        GetPatientDataSection_TRs
                    //            (
                    //                "Encounter Performed: " +
                    //                dtProcedureCodes.Rows[i]["ICDDescription"],
                    //                dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                    //                procedureStartDate
                    //            )
                    //    );
                    objComponent3.section.entry.Add(
                        SetEncounterPerformed(dtProcedureCodes.Rows[i]));
                }
            }
        }
        private static void EncounterPerformed_OnlySNOMED(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.EncounterPerformed.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            var proceduresCodesRowCollection = _dsPatientDataSection.Tables[
                _dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure_N"
            && r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
            && r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            if (proceduresCodesRowCollection.Any())
            {
                var dtProcedureCode = proceduresCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate", "valueset");
                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        GetPatientDataSection_TRs
                    //            (
                    //                "Encounter Performed: " +
                    //                dtProcedureCodes.Rows[i]["ICDDescription"],
                    //                dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                    //                procedureStartDate
                    //            )
                    //    );
                    objComponent3.section.entry.Add(
                        SetEncounterPerformed(dtProcedureCodes.Rows[i]));
                }
            }
        }
        private static void ProcedurePerformed(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSection.Tables[
                _dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Procedure_W");
            //&& r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
            //&& r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            if (proceduresCodesRowCollection.Any())
            {
                var dtProcedureCode = proceduresCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        GetPatientDataSection_TRs
                    //            (
                    //                "Procedure Performed: " +
                    //                dtProcedureCodes.Rows[i]["ICDDescription"],
                    //                dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                    //                procedureStartDate
                    //            )
                    //    );
                    objComponent3.section.entry.Add(
                        SetProcedurePerformed(dtProcedureCodes.Rows[i]));
                }
            }

            #region Commented

            //for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            //{
            //    var procedureStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedurePerformed.ProcedureStartDateColumn].ToString() == "" ? "" : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedurePerformed.ProcedureStartDateColumn].ToString()).ToString("yyyyMMdd");

            //    objStrucDocTbody.tr.Add
            //        (
            //            GetPatientDataSection_TRs
            //                (
            //                    "Procedure Performed: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedurePerformed.SNOMEDDescriptionColumn],
            //                    _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedurePerformed.SNOMEDDescriptionColumn].ToString(),
            //                    procedureStartDate
            //                )
            //        );

            //    objComponent3.section.Entry.Add(SetProcedurePerformed(_dsPatientDataSection.Tables[tableName].Rows[i]));
            //}

            #endregion
        }
        private static void ProcedureOrder(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.ProcedureOrder.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var procedureStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedureOrder.ProceduresStartDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedureOrder.ProceduresStartDateColumn].ToString()).ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Procedure Order: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedureOrder.ProceduresCPT_DESCRIPTIONColumn],
                //                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedureOrder.ProceduresCPT_DESCRIPTIONColumn].ToString(),
                //                procedureStartDate
                //            )
                //    );

                objComponent3.section.entry.Add(SetProcedureOrder(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void RiskCategoryAssessment(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSection.Tables[
                _dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "Loinc" &&
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Lab" &&
                        r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            //r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
            //&& 
            if (riskCategoryAssessmentCodesRowCollection.Any())
            {
                var dtProcedureCode = riskCategoryAssessmentCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        GetPatientDataSection_TRs
                    //            (
                    //                "Risk Category Assessment: " +
                    //                dtProcedureCodes.Rows[i]["ICDDescription"],
                    //                dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                    //                procedureStartDate
                    //            )
                    //    );
                    objComponent3.section.entry.Add(
                        SetRiskCategoryAssessment(dtProcedureCodes.Rows[i]));
                }
            }
        }
        private static void RiskCategoryAssessmentPatch(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.RiskCatagoryAssesment.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            #region Commented Yet

            //var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
            //        .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "LOINC"
            //        && r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            ////r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
            ////&& 
            //if (riskCategoryAssessmentCodesRowCollection.Any())
            //{
            //    var dtProcedureCode = riskCategoryAssessmentCodesRowCollection.CopyToDataTable();

            //    var dvProcedureCodes = new DataView(dtProcedureCode);
            //    var dtProcedureCodes = dvProcedureCodes.ToTable(); //(true, "ICD", "ICDDescription", "Title", "DataType",
            //    //    "StartDate", "EndDate");

            //    for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
            //    {
            //        var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
            //                ? DateTime.Now.ToString("yyyyMMdd")
            //                : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

            //        objStrucDocTbody.tr.Add
            //            (
            //                GetPatientDataSection_TRs
            //                    (
            //                        "Risk Category Assessment: " +
            //                        dtProcedureCodes.Rows[i]["ICDDescription"],
            //                        dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
            //                        procedureStartDate
            //                    )
            //            );
            //        objComponent3.section.Entry.Add(
            //            SetRiskCategoryAssessment(dtProcedureCodes.Rows[i]));
            //    }
            //}

            #endregion

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var riskCatagoryAssesmentStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.OrderDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.OrderDateColumn].ToString()).ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Risk Category Assessment: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.LOINCDescriptionColumn],
                //                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.LOINCDescriptionColumn].ToString(),
                //                riskCatagoryAssesmentStartDate
                //            )
                //    );

                objComponent3.section.entry.Add(SetRiskCategoryAssessmentPatch(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }

        }
        private static void TobbacoUserPatch(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.TobbacoUser.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            #region Commented

            //var tobbacoUserCodesRowCollection = _dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
            //        .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
            //        && r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            ////r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
            ////&& 
            //if (tobbacoUserCodesRowCollection.Any())
            //{
            //    var dtProcedureCode = tobbacoUserCodesRowCollection.CopyToDataTable();

            //    var dvProcedureCodes = new DataView(dtProcedureCode);
            //    var dtProcedureCodes = dvProcedureCodes.ToTable(); //(true, "ICD", "ICDDescription", "Title", "DataType",
            //    //    "StartDate", "EndDate");

            //    for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
            //    {
            //        var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
            //                ? DateTime.Now.ToString("yyyyMMdd")
            //                : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

            //        objStrucDocTbody.tr.Add
            //            (
            //                GetPatientDataSection_TRs
            //                    (
            //                        "Patient Characteristic: " +
            //                        dtProcedureCodes.Rows[i]["ICDDescription"],
            //                        dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
            //                        procedureStartDate
            //                    )
            //            );
            //        objComponent3.section.Entry.Add(
            //            SetTobbacoUser(dtProcedureCodes.Rows[i]));
            //    }
            //}

            #endregion

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var tobbacoUserStartDate =
                    _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.TobbacoUser.SocialHxDateColumn
                        ].ToString() == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(
                            _dsPatientDataSection.Tables[tableName].Rows[i][
                                _dsPatientDataSection.TobbacoUser.SocialHxDateColumn].ToString()).ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Patient Characteristic: " +
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn],
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn].ToString(),
                //                tobbacoUserStartDate
                //            )
                //    );

                objComponent3.section.entry.Add(SetTobbacoUser(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void TobbacoUser(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.TobbacoUser.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            #region Commented

            //var tobbacoUserCodesRowCollection = _dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
            //        .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
            //        && r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            ////r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
            ////&& 
            //if (tobbacoUserCodesRowCollection.Any())
            //{
            //    var dtProcedureCode = tobbacoUserCodesRowCollection.CopyToDataTable();

            //    var dvProcedureCodes = new DataView(dtProcedureCode);
            //    var dtProcedureCodes = dvProcedureCodes.ToTable(); //(true, "ICD", "ICDDescription", "Title", "DataType",
            //    //    "StartDate", "EndDate");

            //    for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
            //    {
            //        var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
            //                ? DateTime.Now.ToString("yyyyMMdd")
            //                : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

            //        objStrucDocTbody.tr.Add
            //            (
            //                GetPatientDataSection_TRs
            //                    (
            //                        "Patient Characteristic: " +
            //                        dtProcedureCodes.Rows[i]["ICDDescription"],
            //                        dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
            //                        procedureStartDate
            //                    )
            //            );
            //        objComponent3.section.Entry.Add(
            //            SetTobbacoUser(dtProcedureCodes.Rows[i]));
            //    }
            //}

            #endregion

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var tobbacoUserStartDate =
                    _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.TobbacoUser.SocialHxDateColumn
                        ].ToString() == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(
                            _dsPatientDataSection.Tables[tableName].Rows[i][
                                _dsPatientDataSection.TobbacoUser.SocialHxDateColumn].ToString()).ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Patient Characteristic: " +
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn],
                //                _dsPatientDataSection.Tables[tableName].Rows[i][
                //                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn].ToString(),
                //                tobbacoUserStartDate
                //            )
                //    );

                objComponent3.section.entry.Add(SetTobbacoUser(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void PyhsicalExam(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.PhysicalExam.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            #region Commented

            //var pyhsicalExamRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
            //        .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "LOINC"
            //        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            //if (pyhsicalExamRowCollection.Any())
            //{
            //    var dtProcedureCode = pyhsicalExamRowCollection.CopyToDataTable();

            //    var dvProcedureCodes = new DataView(dtProcedureCode);
            //    var dtProcedureCodes = dvProcedureCodes.ToTable(); //(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate");

            //    for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
            //    {
            //        var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
            //                ? DateTime.Now.ToString("yyyyMMdd")
            //                : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

            //        objStrucDocTbody.tr.Add
            //            (
            //                GetPatientDataSection_TRs
            //                    (
            //                        "Physical Exam, Finding: " +
            //                        dtProcedureCodes.Rows[i]["ICDDescription"],
            //                        dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
            //                        procedureStartDate
            //                    )
            //            );
            //        objComponent3.section.Entry.Add(SetPhysicalExam(dtProcedureCodes.Rows[i]));
            //    }
            //}

            #endregion

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var physicalExamStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString() == ""
                    ? "" :
                    DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString()).ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Physical Exam, Finding: Diastolic Blood Pressure",
                //                "Diastolic Blood Pressure",
                //                physicalExamStartDate
                //            )
                //    );

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Physical Exam: Systolic Blood Pressure",
                //                "Systolic Blood Pressure",
                //                physicalExamStartDate
                //            )
                //    );

                var systolicBpLoincCode = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.LOINICSystolicColumn].ToString();
                var diastolicBpLoincCode = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.LOINICDiastolicColumn].ToString();

                var systolicBpValue = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.SystolicColumn].ToString();
                var diastolicBpValue = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.DiastolicColumn].ToString();

                objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Diastolic Blood Pressure", "2.16.840.1.113883.3.526.3.1033", diastolicBpLoincCode, diastolicBpValue));
                objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Systolic Blood Pressure", "2.16.840.1.113883.3.526.3.1032", systolicBpLoincCode, systolicBpValue));
            }
        }
        private static void PhysicalExam_421(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.PhysicalExam.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var physicalExamStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString() == ""
                    ? "" :
                    DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString()).ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Physical Exam, Finding: BMI LOINC Value",
                //                "BMI LOINC Value",
                //                physicalExamStartDate
                //            )
                //    );

                var BMILoinc = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.LOINICBMIColumn].ToString();
                var BMI = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.BMIColumn].ToString();

                objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "BMI LOINC Value", "2.16.840.1.113883.3.600.1.681", BMILoinc, BMI));

            }
        }
        private static void LabOrder(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.RiskCatagoryAssesment.TableName].Rows.Count <= 0)
                return;

            foreach (DataRow dr in _dsPatientDataSection.Tables[_dsPatientDataSection.RiskCatagoryAssesment.TableName].Rows)
            {


                var OrderDateTime = (string)dr["OrderDate"] == ""
                    ? DateTime.Now.ToString("yyyyMMdd")
                    : DateTime.Parse(dr["OrderDate"].ToString()).ToString("yyyyMMdd") + dr["OrderTime"].ToString();
                string CPTCode = MDVUtility.ToStr(dr["CPTCode"]);
                string CPTCodeDescription = MDVUtility.ToStr(dr["CPTCodeDescription"]);
                string LOINC = MDVUtility.ToStr(dr["LOINC"]);
                string LOINCDescription = MDVUtility.ToStr(dr["LOINCDescription"]);

                string status = MDVUtility.ToStr(dr["Status"].ToString());
                string results = MDVUtility.ToStr(dr["result"]);
                string fields = string.Empty;

                string codes = (string.IsNullOrEmpty(CPTCode) ? "" : "CPTCode: " + CPTCode) +
                    (string.IsNullOrEmpty(LOINC) ? "" : "LOINC: " + LOINC);
                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Laboratory Test "
                                , codes, OrderDateTime, status, results, fields
                            )
                    );

                //objComponent3.section.entry.Add(
                //        SetLabOrder(dr));

            }
        }
        private static void InterventionPerformed(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.CQMCodes.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            var interventionPerformedRowCollection = _dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName]
                .AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure_N"
                        && r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
                        && r.Field<string>(_dsPatientDataSection.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            if (interventionPerformedRowCollection.Any())
            {
                var dtInterventionPerformed = interventionPerformedRowCollection.CopyToDataTable();

                var dvInterventionPerformed = new DataView(dtInterventionPerformed);
                var dtLabOrderRe = dvInterventionPerformed.ToTable(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtLabOrderRe.Rows.Count; i++)
                {
                    var medicationStartDate = (string)dtLabOrderRe.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtLabOrderRe.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    //objStrucDocTbody.tr.Add
                    //    (
                    //        GetPatientDataSection_TRs
                    //            (
                    //                "Laboratory Test, Result: " +
                    //                dtLabOrderRe.Rows[i]["ICDDescription"],
                    //                dtLabOrderRe.Rows[i]["ICDDescription"].ToString(),
                    //                medicationStartDate
                    //            )
                    //    );
                    objComponent3.section.entry.Add(
                        SetInterventionPerformed(dtLabOrderRe.Rows[i]));
                }
            }
        }
        private static void ProviderToProvider(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.ProviderToProvider.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var providerToProviderStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.StartDateColumn].ToString() == ""
                    ? DateTime.Now.ToString("yyyyMMdd") :
                    DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.StartDateColumn].ToString()).ToString("yyyyMMdd");

                //objStrucDocTbody.tr.Add
                //    (
                //        GetPatientDataSection_TRs
                //            (
                //                "Communication: From Provider to Provider: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.SNOMEDDescriptionColumn],
                //                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.SNOMEDDescriptionColumn].ToString(),
                //                providerToProviderStartDate
                //            )
                //    );

                objComponent3.section.entry.Add(SetProviderToProvider(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }

        #endregion

        #region Patient Data Section Set'em

        #region Diagnosis Active Concern Act
        private static Entry SetDiagnosisActiveConcernAct(DataRow drDiagnosisActiveConcernAct)
        {
            var codeValue = drDiagnosisActiveConcernAct["ICD10"].ToString();
            var diseaseName = drDiagnosisActiveConcernAct["ICD10_Description"].ToString();
            var codeSystem = drDiagnosisActiveConcernAct["Code"].ToString();
            var valueset = drDiagnosisActiveConcernAct["Codetype"].ToString();

            Entry entryDiagnosisActiveConcernAct = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.22.4.4"
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.11"
                        }
                    },
                    id = new List<II>
                    {
                        new II
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    code = new CD()
                    {
                        code = "282291009",
                        displayName = "diagnosis",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT"
                    },
                    text = new ED()
                    {
                        Text = new List<string>
                        {
                            "Diagnosis, Active: " + diseaseName
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["StartDate"].ToString())
                                    ? ""
                                    : DateTime.Parse(drDiagnosisActiveConcernAct["StartDate"].ToString())
                                        .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["EndDate"].ToString())
                                    ? ""
                                    : DateTime.Parse(drDiagnosisActiveConcernAct["EndDate"].ToString())
                                        .ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = codeValue,
                            displayName = diseaseName,
                            codeSystem = codeSystem.Contains("SNOMED")
                                ? "2.16.840.1.113883.6.96"
                                : codeSystem.Contains("CPT")
                                    ? "2.16.840.1.113883.6.12"
                                    : codeSystem.Contains("ICD9")
                                        ? "2.16.840.1.113883.6.103"
                                        : codeSystem.Contains("Loinc")
                                            ? "2.16.840.1.113883.6.1"
                                            : codeSystem.Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                            codeSystemName = codeSystem,
                            originalText = new ED() {Text = new List<string>() {"Diagnosis, Active: " + diseaseName}},
                            translation = new List<CD>()
                            {
                                new CD()
                                {
                                    code = codeValue,
                                    codeSystem = codeSystem.Contains("SNOMED")
                                        ? "2.16.840.1.113883.6.96"
                                        : codeSystem.Contains("CPT")
                                            ? "2.16.840.1.113883.6.12"
                                            : codeSystem.Contains("ICD9")
                                                ? "2.16.840.1.113883.6.103"
                                                : codeSystem.Contains("LOINC")
                                                    ? "2.16.840.1.113883.6.1"
                                                    : codeSystem.Contains("ICD10") ? "2.16.840.1.113883.6.90" : ""
                                }
                            },
                            valueSet = valueset
                        }
                    },
                    entryRelationship = new List<EntryRelationship>()
                    {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.REFR,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.6"
                                        //4.47 Problem Status (DEPRECATED) - Deprecated
                                    },
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.94"
                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "33999-4",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "status"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = "55561003",
                                        displayName = "active",
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        codeSystemName = "SNOMED CT"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return entryDiagnosisActiveConcernAct;
        }
        private static Entry SetDiagnosisActiveConcernActPatch(DataRow drDiagnosisActiveConcernAct)
        {
            var codeValue = drDiagnosisActiveConcernAct[_dsPatientDataSection.DiagnosisActiveConcernAct.SNOMEDIDColumn.ColumnName].ToString();
            var diseaseName = drDiagnosisActiveConcernAct[_dsPatientDataSection.DiagnosisActiveConcernAct.SNOMED_DESCRIPTIONColumn.ColumnName].ToString();
            //var codeSystem = drDiagnosisActiveConcernAct["Title"].ToString();
            var valueset = "2.16.840.1.113883.3.600.1.1623";//drDiagnosisActiveConcernAct["valueset"].ToString();

            Entry entryDiagnosisActiveConcernAct = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.22.4.4"
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.11"
                        }
                    },
                    id = new List<II>
                    {
                        new II
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    code = new CD()
                    {
                        code = "282291009",
                        displayName = "diagnosis",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED CT"
                    },
                    text = new ED()
                    {
                        Text = new List<string>
                        {
                            "Diagnosis, Active: " + diseaseName
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(
                                        drDiagnosisActiveConcernAct[
                                            _dsPatientDataSection.DiagnosisActiveConcernAct.StartDateColumn.ColumnName].ToString
                                            ())
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : DateTime.Parse(
                                            drDiagnosisActiveConcernAct[
                                                _dsPatientDataSection.DiagnosisActiveConcernAct.StartDateColumn.ColumnName]
                                                .ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(
                                        drDiagnosisActiveConcernAct[
                                            _dsPatientDataSection.DiagnosisActiveConcernAct.EndDateColumn.ColumnName].ToString
                                            ())
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : DateTime.Parse(
                                            drDiagnosisActiveConcernAct[
                                                _dsPatientDataSection.DiagnosisActiveConcernAct.EndDateColumn.ColumnName]
                                                .ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = codeValue,
                            displayName = diseaseName,
                            codeSystem = "2.16.840.1.113883.6.96",
                            originalText = new ED() {Text = new List<string>() {"Diagnosis, Active: " + diseaseName}},
                            translation = new List<CD>()
                            {
                                new CD()
                                {
                                    code = codeValue,
                                    codeSystem = "2.16.840.1.113883.6.96",
                                }
                            },
                            valueSet = valueset
                        }
                    },
                    entryRelationship = new List<EntryRelationship>()
                    {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.REFR,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.6"
                                        //4.47 Problem Status (DEPRECATED) - Deprecated
                                    },
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.94"
                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "33999-4",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "status"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = "55561003",
                                        displayName = "active",
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        codeSystemName = "SNOMED CT"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return entryDiagnosisActiveConcernAct;
        }

        #endregion

        #region FamilyHx
        private static Entry SetFamilyHx(DataRow drFamilyHx)
        {
            if (drFamilyHx == null) throw new ArgumentNullException("drFamilyHx");

            var entryFamilyHx = new Entry()
            {
                Item = new Organizer()
                {
                    classCode = x_ActClassDocumentEntryOrganizer.CLUSTER,
                    moodCode = "EVN",
                    templateId = new List<II>
                    {
                        new II
                        {
                            //<!-- C-CDA R2 Family History Organizer (V2) -->
                            root = "2.16.840.1.113883.10.20.22.4.45",
                            extension = DateTime.Now.ToString("yyyy-MM-dd") //"Which Time will be displayed Here ??????????"
                            //DateTime.Now.ToString("yyyy-MM-dd")
                        },
                        new II()
                        {
                            //<!-- Diagnosis Family History (V2) templateId -->
                            root = "2.16.840.1.113883.10.20.24.3.12",
                            extension = DateTime.Now.ToString("yyyy-MM-dd") //"Date which ???????????"
                        }
                    },
                    statusCode = new CS
                    {
                        code = "completed"
                    },
                    subject = new Subject
                    {
                        relatedSubject = new RelatedSubject()
                        {
                            classCode = x_DocumentSubject.PRS,
                            code = new CS()
                            {
                                code =
                                    drFamilyHx[_dsPatientDataSection.FamilyHx.FamilyHx_FamilyMemberDescriptionColumn.ColumnName]
                                        .ToString(),
                                nullFlavor = "NA", // codeSystem = "2.16.840.1.113883.5.111",
                                //codeSystemName = "HL7 FamilyMember"
                                //displayName = ""
                            }
                        }
                    },
                    effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value =
                                                DateTime.Parse(
                                                    drFamilyHx[_dsPatientDataSection.FamilyHx.FamilyHxDateColumn.ColumnName]
                                                        .ToString()).ToString("yyyy")
                                        },
                                        new IVXB_TS
                                        {
                                            value = DateTime.Now.ToString("yyyy")
                                        }
                                    }
                    },
                    //FamilyHistoryObservation
                    component = new List<Component4>()
                    {
                        new Component4
                        {
                            Item = new Observation
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>
                                {
                                    new II
                                    {
                                        //<!-- Conforms to C-CDA Family History Observation (V2) -->
                                        root = "2.16.840.1.113883.10.20.22.4.46",
                                        extension = DateTime.Now.ToString("yyyy-MM-dd")
                                    },
                                    new II()
                                    {
                                        //<!-- Diagnosis Family History Observation -->
                                        root = "2.16.840.1.113883.10.20.24.3.112",
                                        extension = DateTime.Now.ToString("yyyy-MM-dd")
                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD
                                {
                                    code = "75323-6",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "Condition"
                                },
                                statusCode = new CS
                                {
                                    code = "completed"
                                },
                                effectiveTime = new IVL_TS
                                {
                                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                    Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value =
                                                DateTime.Parse(
                                                    drFamilyHx[_dsPatientDataSection.FamilyHx.FamilyHxDateColumn.ColumnName]
                                                        .ToString()).ToString("yyyy")
                                        },
                                        new IVXB_TS
                                        {
                                            value = DateTime.Now.ToString("yyyy")
                                        }
                                    }
                                },
                                value = new List<ANY>
                                {
                                    new CD
                                    {
                                        code = drFamilyHx[_dsPatientDataSection.FamilyHx.SNOMEDIDColumn.ColumnName].ToString(),
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        displayName =
                                            drFamilyHx[_dsPatientDataSection.FamilyHx.SNOMEDDescriptionColumn.ColumnName].ToString
                                                (),
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return entryFamilyHx;
        }

        #endregion

        #region MedicationActive
        private static Entry SetMedicationActive(DataRow drMedicationActive)
        {
            var medicationActiveCodesRowCollection = _dsPatientDataSection.Tables[_dsPatientDataSection.CQMCodes.TableName].AsEnumerable()
                    .Where(r => r.Field<string>(_dsPatientDataSection.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Drug"
                        && r.Field<string>(_dsPatientDataSection.CQMCodes.TitleColumn.ColumnName.ToString()) == "RxnormID");

            DataTable dtMedicationActiveCodes = null;
            if (medicationActiveCodesRowCollection.Any())
            {
                var dtProblemCode = medicationActiveCodesRowCollection.CopyToDataTable();
                var dvProblemCodes = new DataView(dtProblemCode);
                dtMedicationActiveCodes = dvProblemCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate", "Condition");
            }
            DataRow drnew = null;
            if (dtMedicationActiveCodes != null && dtMedicationActiveCodes.Rows.Count > 0)
            {
                drnew = dtMedicationActiveCodes.Rows[0];
            }

            var startTime = drMedicationActive["StartDate"].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(drMedicationActive["StartDate"].ToString()).ToString("yyyyMMdd");
            var stopTime = drMedicationActive["EndDate"].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(drMedicationActive["EndDate"].ToString()).ToString("yyyyMMdd");
            var medDescription = string.IsNullOrEmpty(drMedicationActive["ICDDescription"].ToString())
                ? drMedicationActive["Condition"].ToString()
                : drMedicationActive["ICDDescription"].ToString();


            var entryMedicationActive = new Entry()
            {
                Item = new SubstanceAdministration()
                {
                    classCode = "SBADM",
                    moodCode = x_DocumentSubstanceMood.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.22.4.16",
                            extension = DateTime.Now.ToString("yyyyMMdd")
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.41",
                            extension = DateTime.Now.ToString("yyyyMMdd")
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>()
                        {
                            "Medication Active: " + medDescription
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "active"
                    },
                    effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS()
                                {
                                    value = startTime

                                },
                                new IVXB_TS()
                                {
                                    value = stopTime
                                }
                            }
                        }
                        //,new PIVL_TS()
                        //{
                        //    @operator = SetOperator.A,
                        //    institutionSpecified1 = true,
                        //    period = new PQ()
                        //    {
                        //        value =
                        //            string.IsNullOrEmpty(drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName]
                        //                .ToString()) ? "0": drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName]
                        //                .ToString(),
                        //        unit =
                        //            string.IsNullOrEmpty(drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName].ToString())
                        //            ? "h" : drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName].ToString()
                        //    }
                        //}
                    },
                    //doseQuantity = new IVL_PQ()
                    //{
                    //    value =
                    //        drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationQuantityColumn.ColumnName]
                    //            .ToString() == "" ? "1" : drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationQuantityColumn.ColumnName].ToString()
                    //    // TEMP, If No Dose What to Set
                    //},
                    consumable = new Consumable()
                    {
                        manufacturedProduct = new ManufacturedProduct()
                        {
                            classCode = RoleClassManufacturedProduct.MANU,
                            templateId = new List<II>()
                            {
                                new II()
                                {
                                    root = "2.16.840.1.113883.10.20.22.4.23",
                                    extension =DateTime.Now.ToString("yyyy-MM-dd")
                                }
                            },
                            id = new List<II>()
                            {
                                new II()
                                {
                                    root = Guid.NewGuid().ToString()
                                }
                            },
                            Item = new Material()
                            {
                                code = new CE()
                                {
                                    code = drMedicationActive["ICD"].ToString().Trim(),
                                    codeSystem = "2.16.840.1.113883.6.88",
                                    valueSet = drMedicationActive["valueset"].ToString().Trim()
                                }
                                //IsRxNorm_MedicationActive(drMedicationActive, drnew)
                            }
                            //manufacturerOrganization = new Organization()
                            //{
                            //    name = new List<ON>()
                            //    {
                            //        new ON()
                            //        {
                            //            Text = new List<string>()
                            //            {
                            //                drMedicationActive[_dsPatientDataSection.MedicationActive.PharmacyPharmacyNameColumn.ColumnName].ToString() 
                            //                == "" ? "Rock Ridge Pharmacy" :drMedicationActive[_dsPatientDataSection.MedicationActive.PharmacyPharmacyNameColumn.ColumnName].ToString()
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            };
            return entryMedicationActive;
        }

        //private static Entry SetMedicationActiveNew( DataRow drMedicationActive )
        //{

        //    var startTime = string.IsNullOrEmpty(drMedicationActive["StartDate"].ToString()) ? "" : DateTime.Parse(drMedicationActive["StartDate"].ToString()).ToString("yyyyMMddHHmmss");
        //    var stopTime = string.IsNullOrEmpty(drMedicationActive["EndDate"].ToString()) ? "" : DateTime.Parse(drMedicationActive["EndDate"].ToString()).ToString("yyyyMMddHHmmss");

        //    var entryMedicationActive = new Entry()
        //    {
        //        Item = new SubstanceAdministration()
        //        {
        //            classCode = "SBADM",
        //            moodCode = x_DocumentSubstanceMood.EVN,
        //            templateId = new List<II>()
        //            {
        //                new II()
        //                {
        //                    root = "2.16.840.1.113883.10.20.22.4.16",
        //                    extension = DateTime.Now.ToString("yyyyMMdd")
        //                },
        //                new II()
        //                {
        //                    root = "2.16.840.1.113883.10.20.24.3.41",
        //                    extension = DateTime.Now.ToString("yyyyMMdd")
        //                }
        //            },
        //            id = new List<II>()
        //            {
        //                new II()
        //                {
        //                    root = Guid.NewGuid().ToString()
        //                }
        //            },
        //            text = new ED()
        //            {
        //                Text = new List<string>()
        //                {
        //                    "Medication Active: " +
        //                    drMedicationActive[
        //                        _dsPatientDataSection.MedicationOrder.DrugDrugDescriptionColumn.ColumnName]
        //                }
        //            },
        //            statusCode = new CS()
        //            {
        //                code = "active"
        //            },
        //            effectiveTime = new List<SXCM_TS>()
        //            {
        //                new IVL_TS()
        //                {
        //                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
        //                    Items = new QTY[]
        //                    {
        //                        new IVXB_TS()
        //                        {
        //                            value = startTime

        //                        },
        //                        new IVXB_TS()
        //                        {
        //                            value = stopTime
        //                        }
        //                    }
        //                },
        //                new PIVL_TS()
        //                {
        //                    @operator = SetOperator.A,
        //                    institutionSpecified1 = true,
        //                    period = new PQ()
        //                    {
        //                        value =
        //                            string.IsNullOrEmpty(
        //                                drMedicationActive[
        //                                    _dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName]
        //                                    .ToString())
        //                                ? "0"
        //                                : drMedicationActive[
        //                                    _dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName]
        //                                    .ToString(),
        //                        unit =
        //                            string.IsNullOrEmpty(
        //                                drMedicationActive[
        //                                    _dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName]
        //                                    .ToString())
        //                                ? "h"
        //                                : drMedicationActive[
        //                                    _dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName]
        //                                    .ToString()
        //                    }
        //                }
        //            },
        //            doseQuantity = new IVL_PQ()
        //            {
        //                value =
        //                    drMedicationActive[
        //                        _dsPatientDataSection.MedicationActive.MedicationQuantityColumn.ColumnName]
        //                        .ToString() == ""
        //                        ? "1"
        //                        : drMedicationActive[
        //                            _dsPatientDataSection.MedicationActive.MedicationQuantityColumn.ColumnName].ToString
        //                            ()
        //                // TEMP, If No Dose What to Set
        //            },
        //            consumable = new Consumable()
        //            {
        //                manufacturedProduct = new ManufacturedProduct()
        //                {
        //                    classCode = RoleClassManufacturedProduct.MANU,
        //                    templateId = new List<II>()
        //                    {
        //                        new II()
        //                        {
        //                            root = "2.16.840.1.113883.10.20.22.4.23",
        //                            extension = DateTime.Now.ToString("yyyy-MM-dd")
        //                        }
        //                    },
        //                    id = new List<II>()
        //                    {
        //                        new II()
        //                        {
        //                            root = Guid.NewGuid().ToString()
        //                        }
        //                    },
        //                    Item = new Material()
        //                    {
        //                        code = IsRxNorm_MedicationActive(drMedicationActive, drnew)
        //                    },
        //                    manufacturerOrganization = new Organization()
        //                    {
        //                        name = new List<ON>()
        //                        {
        //                            new ON()
        //                            {
        //                                Text = new List<string>()
        //                                {
        //                                    drMedicationActive[
        //                                        _dsPatientDataSection.MedicationActive.PharmacyPharmacyNameColumn
        //                                            .ColumnName].ToString()
        //                                    == ""
        //                                        ? "Rock Ridge Pharmacy"
        //                                        : drMedicationActive[
        //                                            _dsPatientDataSection.MedicationActive.PharmacyPharmacyNameColumn
        //                                                .ColumnName].ToString()
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    };

        //    return entryMedicationActive;
        //    }
        private static CE IsRxNorm_MedicationActive(DataRow drMedicationActive, DataRow drMedicationActiveCode)
        {
            if (string.IsNullOrEmpty(drMedicationActive[_dsPatientDataSection.MedicationActive.DrugRxnormIDColumn.ColumnName].ToString()))
            {
                if (drMedicationActiveCode != null)
                {
                    var code = new CE()
                    {
                        nullFlavor = "OTH",
                        codeSystem = "2.16.840.1.113883.6.88",
                        translation = new List<CD>()
                        {
                            new CD()
                            {
                                code = drMedicationActiveCode["ICD"].ToString(),
                                codeSystem =  drMedicationActiveCode["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                : drMedicationActiveCode["Title"].ToString().Contains("CPT") ? "2.16.840.1.113883.6.12"
                                : drMedicationActiveCode["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                : drMedicationActiveCode["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                : drMedicationActiveCode["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                                codeSystemName = drMedicationActiveCode["Title"].ToString(),
                                displayName = drMedicationActiveCode["ICDDescription"].ToString(),
                                valueSet = "2.16.840.1.113883.3.464.1003.196.12.1211"
                            }
                        }
                    };
                    return code;
                }
                else
                {
                    var code = new CE()
                    {
                        nullFlavor = "OTH",
                        codeSystem = "2.16.840.1.113883.6.88",
                        translation = new List<CD>()
                        {
                            new CD()
                            {
                                code = "410942007",
                                codeSystem = "2.16.840.1.113883.6.96",
                                codeSystemName = "SNOMED CT",
                                displayName = "drug or medication",
                                valueSet = "2.16.840.1.113883.3.464.1003.196.12.1211"
                            }
                        }
                    };
                    return code;
                }
            }
            else
            {
                var code = new CE()
                {
                    code =
                        drMedicationActive[_dsPatientDataSection.MedicationActive.DrugRxnormIDColumn.ColumnName].ToString(),
                    codeSystem = "2.16.840.1.113883.6.88",
                    codeSystemName = "RxNorm",
                    displayName =
                        drMedicationActive[_dsPatientDataSection.MedicationActive.DrugDrugDescriptionColumn.ColumnName]
                            .ToString(),
                    valueSet = "2.16.840.1.113883.3.464.1003.196.12.1211"
                };
                return code;
            }
        }
        private static CE IsRxNorm_MedicationActive(DataRow drMedicationActive)
        {
            if (
                string.IsNullOrEmpty(
                    drMedicationActive[_dsPatientDataSection.MedicationActive.DrugRxnormIDColumn.ColumnName].ToString()))
            {
                var code = new CE()
                {
                    nullFlavor = "OTH",
                    codeSystem = "2.16.840.1.113883.6.88",
                    translation = new List<CD>()
                    {
                        new CD()
                        {
                            code = "410942007",
                            codeSystem = "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOMED CT",
                            displayName = "drug or medication",
                            valueSet = "2.16.840.1.113883.3.464.1003.110.12.1027" // "2.16.840.1.113883.3.464.1003.196.12.1253"
                        }
                    }
                };
                return code;
            }
            else
            {
                var code = new CE()
                {
                    code =
                        drMedicationActive[_dsPatientDataSection.MedicationActive.DrugRxnormIDColumn.ColumnName].ToString(),
                    codeSystem = "2.16.840.1.113883.6.88",
                    codeSystemName = "RxNorm",
                    displayName =
                        drMedicationActive[_dsPatientDataSection.MedicationActive.DrugDrugDescriptionColumn.ColumnName]
                            .ToString(),
                    valueSet = "2.16.840.1.113883.3.464.1003.110.12.1027 " //"2.16.840.1.113883.3.464.1003.196.12.1253"
                };
                return code;
            }
        }

        #endregion

        #region MedicationAdministered
        private static Entry SetMedicationAdministered(DataRow drMedicationAdministered)
        {
            var entryMedicationAdministered = new Entry()
            {
                Item = new Act()
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.42",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>()
                        {
                            "Medication, Administered: " +
                            drMedicationAdministered[
                                _dsPatientDataSection.MedicationOrder.DrugDrugDescriptionColumn.ColumnName]
                        }
                    },
                    code = new CD()
                    {
                        code = "416118004",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED-CT",
                        displayName = "Administration"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                        {
                            new IVXB_TS()
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        drMedicationAdministered[_dsPatientDataSection.MedicationAdministered.MedicationStartDateColumn.ColumnName].ToString())
                                        ? DateTime.Now.ToString("yyyyMMdd")
                                        : DateTime.Parse(
                                            drMedicationAdministered[_dsPatientDataSection.MedicationAdministered.MedicationStartDateColumn.ColumnName].ToString())
                                            .ToString("yyyyMMdd")
                            },
                            new IVXB_TS()
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        drMedicationAdministered[
                                            _dsPatientDataSection.MedicationAdministered.MedicationStopDateColumn.ColumnName].ToString())
                                        ? DateTime.Now.ToString("yyyyMMdd")
                                        : DateTime.Parse(
                                            drMedicationAdministered[_dsPatientDataSection.MedicationAdministered.MedicationStopDateColumn.ColumnName].ToString())
                                            .ToString("yyyyMMdd")
                            },
                        }
                    },
                    entryRelationship = new List<EntryRelationship>()
                    {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.COMP,
                            Item = new SubstanceAdministration()
                            {
                                classCode = "SBADM",
                                moodCode = x_DocumentSubstanceMood.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.16",
                                        extension = DateTime.Now.ToString("yyyy-MM-dd")
                                    }
                                },
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                text = new ED()
                                {
                                    Text = new List<string>()
                                    {
                                        "Medication Administered: " +
                                        drMedicationAdministered[
                                            _dsPatientDataSection.MedicationAdministered.DrugDrugDescriptionColumn
                                                .ColumnName]
                                    }
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                effectiveTime = new List<SXCM_TS>()
                                {
                                    new IVL_TS()
                                    {
                                        ItemsElementName =new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                        Items = new QTY[]
                                        {
                                            new IVXB_TS()
                                            {
                                                value =
                                                    string.IsNullOrEmpty(
                                                        drMedicationAdministered[_dsPatientDataSection.MedicationAdministered.MedicationStartDateColumn.ColumnName]
                                                        .ToString())
                                                        ? DateTime.Now.ToString("yyyyMMdd")
                                                        : DateTime.Parse(
                                                            drMedicationAdministered[_dsPatientDataSection.MedicationAdministered.MedicationStartDateColumn.ColumnName]
                                                            .ToString())
                                                            .ToString("yyyyMMdd")
                                            },
                                            new IVXB_TS()
                                            {
                                                value =
                                                    string.IsNullOrEmpty(
                                                        drMedicationAdministered[_dsPatientDataSection.MedicationAdministered.MedicationStopDateColumn.ColumnName]
                                                        .ToString())
                                                        ? DateTime.Now.ToString("yyyyMMdd")
                                                        : DateTime.Parse(
                                                            drMedicationAdministered[_dsPatientDataSection.MedicationAdministered.MedicationStopDateColumn.ColumnName]
                                                            .ToString()).ToString("yyyyMMdd")
                                            },
                                        }

                                    }
                                },
                                //doseQuantity = new IVL_PQ()
                                //{
                                //    value =
                                //        drMedicationAdministered[
                                //            _dsPatientDataSection.MedicationAdministered.MedicationQuantityColumn
                                //                .ColumnName]
                                //            .ToString() == ""
                                //            ? "1"
                                //            : drMedicationAdministered[
                                //                _dsPatientDataSection.MedicationAdministered.MedicationQuantityColumn
                                //                    .ColumnName]
                                //                .ToString()
                                //},
                                consumable = new Consumable()
                                {
                                    manufacturedProduct = new ManufacturedProduct()
                                    {
                                        classCode = RoleClassManufacturedProduct.MANU,
                                        templateId = new List<II>()
                                        {
                                            new II()
                                            {
                                                root = "2.16.840.1.113883.10.20.22.4.23",
                                                extension = DateTime.Now.ToString("yyyy-MM-dd")
                                            }
                                        },
                                        id = new List<II>()
                                        {
                                            new II()
                                            {
                                                root = Guid.NewGuid().ToString()
                                            }
                                        },
                                        Item = new Material()
                                        {
                                            code =
                                                new CE()
                                                {
                                                    code =
                                                        drMedicationAdministered[_dsPatientDataSection.MedicationActive.DrugRxnormIDColumn
                                                                .ColumnName].ToString(),
                                                    codeSystem = "2.16.840.1.113883.6.88",
                                                    codeSystemName = "RxNorm",
                                                    displayName = drMedicationAdministered[_dsPatientDataSection.MedicationActive.DrugDrugDescriptionColumn.ColumnName]
                                                            .ToString(),
                                                    valueSet = "2.16.840.1.113883.3.464.1003.110.12.1027"
                                                }
                                        }
                                        //manufacturerOrganization = new Organization()
                                        //{
                                        //    name = new List<ON>()
                                        //    {
                                        //        new ON()
                                        //        {
                                        //            Text = new List<string>()
                                        //            {
                                        //                drMedicationAdministered[
                                        //                    _dsPatientDataSection.MedicationAdministered
                                        //                        .PharmacyPharmacyNameColumn
                                        //                        .ColumnName].ToString() == ""
                                        //                    ? "Rock Ridge Pharmacy"
                                        //                    : drMedicationAdministered[
                                        //                        _dsPatientDataSection.MedicationAdministered
                                        //                            .PharmacyPharmacyNameColumn
                                        //                            .ColumnName].ToString()
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return entryMedicationAdministered;
        }
        private static Entry SetMedicationAdministered_CVX(DataRow drMedicationAdministeredCvx)
        {
            var codeValue = drMedicationAdministeredCvx["ICD"].ToString();
            var codeDescription = drMedicationAdministeredCvx["ICDDescription"].ToString();
            var codeSystem = drMedicationAdministeredCvx["Title"].ToString();
            var valueset = drMedicationAdministeredCvx["valueset"].ToString();

            var entryMedicationAdministeredCvx = new Entry()
            {
                Item = new Act()
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.42",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>()
                        {
                            "Medication, Administered: " + codeDescription
                        }
                    },
                    code = new CD()
                    {
                        code = "416118004",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED-CT",
                        displayName = "Administration"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                        {
                            new IVXB_TS()
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS()
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                        }
                    },
                    entryRelationship = new List<EntryRelationship>()
                    {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.COMP,
                            Item = new SubstanceAdministration()
                            {
                                classCode = "SBADM",
                                moodCode = x_DocumentSubstanceMood.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.16",
                                        extension = DateTime.Now.ToString("yyyy-MM-dd")
                                    }
                                },
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                text = new ED()
                                {
                                    Text = new List<string>()
                                    {
                                        "Medication Administered: " + codeDescription
                                    }
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                effectiveTime = new List<SXCM_TS>()
                                {
                                    new IVL_TS()
                                    {
                                        ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                        Items =new QTY[]
                                        {
                                            new IVXB_TS()
                                            {
                                                value =
                                                    string.IsNullOrEmpty(
                                                    Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString())
                                                        .ToString("yyyyMMddHHmmss"))
                                                    ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                                    : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString())
                                                        .ToString("yyyyMMddHHmmss")
                                            },
                                            new IVXB_TS()
                                            {
                                                value =
                                                    string.IsNullOrEmpty(
                                                    Convert.ToDateTime(drMedicationAdministeredCvx["EndDate"].ToString())
                                                        .ToString("yyyyMMddHHmmss"))
                                                    ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                                    : Convert.ToDateTime(drMedicationAdministeredCvx["EndDate"].ToString())
                                                        .ToString("yyyyMMddHHmmss")
                                            },
                                        }

                                    }
                                },
                                consumable = new Consumable()
                                {
                                    manufacturedProduct = new ManufacturedProduct()
                                    {
                                        classCode = RoleClassManufacturedProduct.MANU,
                                        templateId = new List<II>()
                                        {
                                            new II()
                                            {
                                                root = "2.16.840.1.113883.10.20.22.4.23",
                                                extension = DateTime.Now.ToString("yyyy-MM-dd")
                                            }
                                        },
                                        id = new List<II>()
                                        {
                                            new II()
                                            {
                                                root = Guid.NewGuid().ToString()
                                            }
                                        },
                                        Item = new Material()
                                        {
                                            code =
                                                new CE()
                                                {
                                                    code = codeValue,
                                                    codeSystem = "2.16.840.1.113883.12.292",
                                                    valueSet = valueset
                                                }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return entryMedicationAdministeredCvx;
        }

        #endregion

        #region Medication Allergy
        private static Entry SetMedicationAllergy(DataRow drMedicationAllergy)
        {
            var entryMedicationAllergy = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.22.4.7",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.44",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                        {
                            //Note: If the allergy/intolerance is known to be resolved, but the date of resolution is not known, 
                            //then the high element SHALL be present, and the nullFlavor attribute SHALL be set to 'UNK'.
                            new IVXB_TS()
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        drMedicationAllergy[
                                            _dsPatientDataSection.MedicationAllergy.OnSetDateColumn.ColumnName].ToString
                                            ())
                                        ? DateTime.Now.ToString("yyyyMMdd")
                                        : DateTime.Parse(
                                            drMedicationAllergy[
                                                _dsPatientDataSection.MedicationAllergy.OnSetDateColumn.ColumnName]
                                                .ToString()).ToString("yyyyMMdd")
                            },
                            new IVXB_TS()
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        drMedicationAllergy[
                                            _dsPatientDataSection.MedicationAllergy.LastModifiedColumn.ColumnName]
                                            .ToString())
                                        ? DateTime.Now.ToString("yyyyMMdd")
                                        : DateTime.Parse(
                                            drMedicationAllergy[
                                                _dsPatientDataSection.MedicationAllergy.LastModifiedColumn.ColumnName]
                                                .ToString()).ToString("yyyyMMdd")
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "416098002",
                            codeSystem = "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOMED CT",
                            displayName = "Drug allergy"
                        }
                    },
                    participant = new List<Participant2>()
                    {
                        new Participant2()
                        {
                            typeCode = "CSM",
                            participantRole = new ParticipantRole()
                            {
                                classCode = "MANU",
                                Item = new PlayingEntity()
                                {
                                    classCode = "MMAT",
                                    code = new CE()
                                    {
                                        code =
                                            drMedicationAllergy[
                                                _dsPatientDataSection.MedicationAllergy.RxnormIDColumn.ColumnName]
                                                .ToString(), // rxNNorm Code will approach here
                                        codeSystem = "2.16.840.1.113883.6.88",
                                        codeSystemName = "RxNorm",
                                        displayName =
                                            drMedicationAllergy[
                                                _dsPatientDataSection.MedicationAllergy.AllergenColumn.ColumnName]
                                                .ToString()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return entryMedicationAllergy;
        }

        #endregion

        #region Medication Order
        private static Entry SetMedicationOrder(DataRow drMedicationOrder)
        {
            var codeValue = drMedicationOrder["ICD"].ToString();
            var codeDescription = drMedicationOrder["ICDDescription"].ToString();
            var valueset = drMedicationOrder["valueset"].ToString();

            var entryMedicationOrder = new Entry()
            {
                Item = new SubstanceAdministration()
                {
                    classCode = "SBADM",
                    moodCode = x_DocumentSubstanceMood.RQO,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.22.4.42",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.47",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>()
                        {
                            "Medication Order: " + codeDescription
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "new"
                    },
                    effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items =new QTY[]
                            {
                                new IVXB_TS
                                {
                                    value =
                                        string.IsNullOrEmpty(
                                            Convert.ToDateTime(drMedicationOrder["StartDate"].ToString())
                                                .ToString("yyyyMMddHHmmss"))
                                            ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                            : Convert.ToDateTime(drMedicationOrder["StartDate"].ToString())
                                                .ToString("yyyyMMddHHmmss")

                                },
                                new IVXB_TS
                                {
                                    value =
                                        string.IsNullOrEmpty(
                                            Convert.ToDateTime(drMedicationOrder["EndDate"].ToString())
                                                .ToString("yyyyMMddHHmmss"))
                                            ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                            : Convert.ToDateTime(drMedicationOrder["EndDate"].ToString())
                                                .ToString("yyyyMMddHHmmss")

                                }
                            }
                        }
                        //new SXCM_TS()
                        //{
                        //    value =
                        //        drMedicationOrder[
                        //            _dsPatientDataSection.MedicationOrder.MedicationStartDateColumn.ColumnName].ToString()
                        //},
                        //new SXCM_TS()
                        //{
                        //    @operator = SetOperator.A,
                        //    value =
                        //        drMedicationOrder[_dsPatientDataSection.MedicationOrder.MedicationStopDateColumn.ColumnName
                        //            ].ToString()
                        //}
                    },
                    consumable = new Consumable()
                    {
                        manufacturedProduct = new ManufacturedProduct()
                        {
                            classCode = RoleClassManufacturedProduct.MANU,
                            templateId = new List<II>()
                            {
                                new II()
                                {
                                    root = "2.16.840.1.113883.10.20.22.4.23",
                                    extension = DateTime.Now.ToString("yyyy-MM-dd")
                                }
                            },
                            id = new List<II>()
                            {
                                new II()
                                {
                                    root = Guid.NewGuid().ToString()
                                }
                            },
                            Item = new Material()
                            {
                                code = new CE()
                                {
                                    code = codeValue,
                                    codeSystem = "2.16.840.1.113883.6.88",
                                    valueSet = valueset //"2.16.840.1.113883.3.464.1003.196.12.1253"
                                }
                                //code = IsRxNorm_MedicationActive(drMedicationOrder)
                            }
                            //manufacturerOrganization = new Organization()
                            //{
                            //    name = new List<ON>()
                            //    {
                            //        new ON()
                            //        {
                            //            Text = new List<string>()
                            //            {
                            //                ""
                            //                //drMedicationOrder[
                            //                //    _dsPatientDataSection.MedicationOrder.PharmacyPharmacyNameColumn.ColumnName].ToString() == ""
                            //                //    ? "Rock Ridge Pharmacy"
                            //                //    : drMedicationOrder[_dsPatientDataSection.MedicationOrder.PharmacyPharmacyNameColumn.ColumnName].ToString()
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                    //author = new List<Author>
                    //{
                    //    new Author
                    //    {
                    //        templateId = new List<II>
                    //        {
                    //            new II
                    //            {
                    //                root = "2.16.840.1.113883.10.20.22.4.119"
                    //            }
                    //        },
                    //        time = new TS()
                    //        {
                    //            value = DateTime.Now.ToString("yyyyMMddHHmmss")
                    //        },
                    //        assignedAuthor = new AssignedAuthor
                    //        {
                    //            id = new List<II>
                    //            {
                    //                new II
                    //                {
                    //                    // what the extension would be ?
                    //                    root = "2.16.840.1.113883.4.6",
                    //                    extension = DateTime.Now.ToString("yyyyMMdd")
                    //                }
                    //            },
                    //            code = new CE
                    //            {
                    //                // Is it Referring Provider ?
                    //                code =
                    //                    drMedicationOrder[
                    //                        _dsPatientDataSection.MedicationOrder.ProviderTaxonomyCodeColumn.ColumnName]
                    //                        .ToString(),
                    //                codeSystem = "2.16.840.1.113883.6.101",
                    //                displayName =
                    //                    drMedicationOrder[
                    //                        _dsPatientDataSection.MedicationOrder.ProviderSpecialityDescriptionColumn
                    //                            .ColumnName].ToString(),
                    //                codeSystemName = "Healthcare Provider Taxonomy (HIPAA)"
                    //            }
                    //        }
                    //    }
                    //}
                }
            };
            return entryMedicationOrder;
        }

        #endregion

        #region Procedure Order
        private static Entry SetProcedureOrder(DataRow drProcedureOrder)
        {
            var entryProcedureOrder = new Entry()
            {
                Item = new Procedure()
                {
                    moodCode = x_DocumentProcedureMood.RQO,
                    classCode = "PROC",
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.22.4.41",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.63",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    code = new CD()
                    {
                        code = drProcedureOrder[_dsPatientDataSection.ProcedureOrder.ProceduresCPTCodeColumn].ToString(),
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "CPT",
                        displayName =
                            drProcedureOrder[_dsPatientDataSection.ProcedureOrder.ProceduresCPT_DESCRIPTIONColumn]
                                .ToString()
                        //valueSet = "{$QDMElementValueSetOID}"
                    },
                    statusCode = new CS()
                    {
                        code = "new"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                        {
                            new IVXB_TS()
                            {
                                value =
                                    drProcedureOrder[_dsPatientDataSection.ProcedureOrder.ProceduresStartDateColumn]
                                        .ToString() == ""
                                        ? DateTime.Now.ToString("yyyyMMdd")
                                        : DateTime.Parse(
                                            drProcedureOrder[
                                                _dsPatientDataSection.ProcedureOrder.ProceduresStartDateColumn].ToString
                                                ()).ToString("yyyyMMdd")
                            },
                            new IVXB_TS()
                            {
                                value =
                                    drProcedureOrder[_dsPatientDataSection.ProcedureOrder.ProceduresEndDateColumn]
                                        .ToString() == ""
                                        ? DateTime.Now.ToString("yyyyMMdd")
                                        : DateTime.Parse(
                                            drProcedureOrder[
                                                _dsPatientDataSection.ProcedureOrder.ProceduresEndDateColumn].ToString())
                                            .ToString("yyyyMMdd")
                            }
                        }
                    },
                    author = new List<Author>()
                    {
                        new Author
                        {
                            templateId = new List<II>
                            {
                                new II
                                {
                                    root = "2.16.840.1.113883.10.20.22.4.119"
                                }
                            },
                            time = new TS()
                            {
                                value = DateTime.Now.ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "2.16.840.1.113883.4.6",
                                        extension =
                                            drProcedureOrder[_dsPatientDataSection.ProcedureOrder.ProviderNPIColumn]
                                                .ToString()
                                    }
                                },
                                code = new CE
                                {
                                    code =
                                        drProcedureOrder[_dsPatientDataSection.ProcedureOrder.ProviderTaxonomyCodeColumn
                                            ].ToString() == ""
                                            ? "207RG0100X"
                                            : drProcedureOrder[
                                                _dsPatientDataSection.ProcedureOrder.ProviderTaxonomyCodeColumn]
                                                .ToString(),
                                    codeSystem = "2.16.840.1.113883.5.53",
                                    displayName =
                                        drProcedureOrder[_dsPatientDataSection.ProcedureOrder.ProviderDescriptionColumn]
                                            .ToString(),
                                    codeSystemName = "Healthcare Provider Taxonomy (HIPAA)"
                                }
                            }
                        }
                    }
                }
            };
            return entryProcedureOrder;
        }

        #endregion

        #region Encounter Performed
        private static Entry SetEncounterPerformed(DataRow drEncounterPerformed)
        {
            var codeValue = drEncounterPerformed["ICD"].ToString();
            var diseaseName = drEncounterPerformed["ICDDescription"].ToString();
            var valueset = drEncounterPerformed["valueset"].ToString(); //drEncounterPerformed["valueset"].ToString();

            var objEncounterPerformed = new Entry()
            {
                Item = new Encounter()
                {
                    classCode = "ENC",
                    moodCode = x_DocumentEncounterMood.EVN,
                    templateId = new List<II>()
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.49"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.23"}
                    },
                    id = new List<II>
                    {
                        new II
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = drEncounterPerformed["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                : drEncounterPerformed["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                : drEncounterPerformed["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                : drEncounterPerformed["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                : drEncounterPerformed["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                        valueSet = valueset,
                        originalText = new ED()
                        {
                            Text = new List<string>()
                            {
                                "Encounter, Performed: " + diseaseName
                            }
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>
                        {
                            "Encounter, Performed: " + diseaseName
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drEncounterPerformed["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drEncounterPerformed["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drEncounterPerformed["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drEncounterPerformed["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                            }
                        }
                    }
                }
            };
            return objEncounterPerformed;
        }

        #endregion

        #region Procedure Performed
        private static Entry SetProcedurePerformed(DataRow drProcedurePerformed)
        {
            var codeValue = drProcedurePerformed["ICD"].ToString();
            var diseaseName = drProcedurePerformed["ICDDescription"].ToString();
            var valueset = drProcedurePerformed["valueset"].ToString();

            var objProcedurePerformed = new Entry()
            {
                Item = new Procedure()
                {
                    classCode = "PROC",
                    moodCode = x_DocumentProcedureMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.14"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.64"}
                    },
                    id = new List<II>
                    {
                        new II
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = drProcedurePerformed["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                : drProcedurePerformed["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                : drProcedurePerformed["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                : drProcedurePerformed["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                : drProcedurePerformed["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                        valueSet = valueset,
                        originalText = new ED()
                        {
                            Text = new List<string>()
                            {
                                "Procedure Performed: " + diseaseName
                            }
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>
                        {
                            "Procedure Performed : " + diseaseName
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "Completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drProcedurePerformed["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drProcedurePerformed["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drProcedurePerformed["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drProcedurePerformed["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                            }
                        }
                    }
                }
            };
            return objProcedurePerformed;
        }

        #endregion

        #region Risk Catagory Assesment
        private static Entry SetRiskCategoryAssessment(DataRow drRiskCategoryAssessment)
        {
            var loincCodeValue = drRiskCategoryAssessment["ICD"].ToString();
            var loincDescription = drRiskCategoryAssessment["ICDDescription"].ToString();
            string valueSet = drRiskCategoryAssessment["valueset"].ToString(); //"2.16.840.1.113883.3.526.3.1278";
            //var date = DateTime.Parse(drRiskCategoryAssessment["OrderDate"].ToString()).ToString("yyyyMMdd");
            // var time = DateTime.Parse(drRiskCategoryAssessment["OrderTime"].ToString().ToString("HHmmss");

            var objRiskCategoryAssessment = new Entry()
            {
                Item = new Procedure()
                {
                    classCode = "OBS",
                    moodCode = x_DocumentProcedureMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.69"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.69"}
                    },
                    id = new List<II>
                    {
                        new II {root = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = loincCodeValue,
                        codeSystem = "2.16.840.1.113883.6.1",
                        valueSet = valueSet,
                        originalText = new ED()
                        {
                            Text = new List<string>()
                            {
                                "Risk Category Assessment: " + loincDescription
                            }
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>
                        {
                            loincDescription
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "Completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                           new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            }
                        }
                    }

                }
            };
            return objRiskCategoryAssessment;
        }

        private static Entry SetRiskCategoryAssessmentPatch(DataRow drRiskCategoryAssessment)
        {
            var loincCodeValue = drRiskCategoryAssessment["LOINC"].ToString();
            var loincDescription = drRiskCategoryAssessment["LOINCDescription"].ToString();
            string valueSet = "2.16.840.1.113883.3.526.3.1278";
            //var date = DateTime.Parse(drRiskCategoryAssessment["OrderDate"].ToString()).ToString("yyyyMMdd");
            // var time = DateTime.Parse(drRiskCategoryAssessment["OrderTime"].ToString().ToString("HHmmss");

            var objRiskCategoryAssessment = new Entry()
            {
                Item = new Procedure()
                {
                    classCode = "OBS",
                    moodCode = x_DocumentProcedureMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.69"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.69"}
                    },
                    id = new List<II>
                    {
                        new II {root = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = loincCodeValue,
                        codeSystem = "2.16.840.1.113883.6.1",
                        valueSet = valueSet,
                        originalText = new ED()
                        {
                            Text = new List<string>()
                            {
                                "Risk Category Assessment: " + loincDescription
                            }
                        }
                    },
                    text = new ED()
                    {
                        Text = new List<string>
                        {
                            loincDescription
                        }
                    },
                    statusCode = new CS()
                    {
                        code = "Completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drRiskCategoryAssessment["OrderDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drRiskCategoryAssessment["OrderDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                            },
                            new IVXB_TS
                            {
                                nullFlavor = "NA"
                                // value = string.IsNullOrEmpty(Convert.ToDateTime(drRiskCategoryAssessment["OrderDate"].ToString()).ToString("yyyyMMddHHmmss"))
                                //? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["OrderDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                    }

                }
            };
            return objRiskCategoryAssessment;
        }

        #endregion

        #region Tobbaco User
        private static Entry SetTobbacoUser(DataRow drTobbacoUser)
        {
            var snomedCodeValue = drTobbacoUser["SNOMEDCTCode"].ToString();
            var snomedDescription = drTobbacoUser["SNOMEDCTDescription"].ToString();
            const string tobbacoValueSet = "2.16.840.1.113883.3.526.3.1170";

            var objTobbacoUser = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.85"}
                    },
                    id = new List<II>
                    {
                        new II {root = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        displayName = "Assertion",
                        codeSystem = "2.16.840.1.113883.5.4",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drTobbacoUser["SocialHxDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drTobbacoUser["SocialHxDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                            },
                            new IVXB_TS
                            {
                                nullFlavor = "UNK"
                                //value =
                                //    string.IsNullOrEmpty(
                                //        Convert.ToDateTime(drRiskCategoryAssessment["EndDate"].ToString())
                                //            .ToString("yyyyMMddHHmmss"))
                                //        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                //        : Convert.ToDateTime(drRiskCategoryAssessment["EndDate"].ToString())
                                //            .ToString("yyyyMMddHHmmss")

                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = snomedCodeValue,
                            codeSystem = "2.16.840.1.113883.6.96",
                            valueSet = tobbacoValueSet,
                            originalText = new ED()
                            {
                                Text = new List<string>()
                                {
                                    "Patient Characteristic: " + snomedDescription
                                }
                            }
                        }
                    }
                }
            };
            return objTobbacoUser;
        }

        #endregion

        #region Physical Exam
        private static Entry SetPhysicalExam(DataRow drPhysicalExam, string bloodPressureType, string valueSet, string loincCodeValue, string bloodPressureValue)
        {
            var objTobbacoUser = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.2"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.57"}
                    },
                    id = new List<II>
                    {
                        new II {root = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = loincCodeValue,
                        codeSystem = "2.16.840.1.113883.6.1",
                        valueSet = valueSet
                    },
                    text = new ED()
                    {
                        Text = new List<string> { "Physical Exam, Finding: " + bloodPressureType }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"

                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drPhysicalExam["VitalSignDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drPhysicalExam["VitalSignDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drPhysicalExam["VitalSignDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drPhysicalExam["VitalSignDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new PQ()
                        {
                            value = bloodPressureValue,
                            unit = "mmHg"
                        }
                    }
                }
            };
            return objTobbacoUser;
        }

        #endregion

        #region Lab Order Test
        private static Entry SetLabOrder(DataRow drLabOrder)
        {
            var codeValue = drLabOrder["ICD"].ToString();
            var codeDescription = drLabOrder["ICDDescription"].ToString();
            var valueset = drLabOrder["valueset"].ToString();
            var objLabOrder = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.2"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.40"}
                    },
                    id = new List<II>
                    {
                        new II {root = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = "2.16.840.1.113883.6.1",
                        valueSet = valueset
                    },
                    text = new ED()
                    {
                        Text = new List<string> { "Laboratory Test, Result: " + codeDescription }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"

                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drLabOrder["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drLabOrder["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drLabOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drLabOrder["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new PQ()
                        {
                            value = "37",
                            unit = "mg/dL"
                        }
                    }
                }
            };
            return objLabOrder;
        }

        #endregion

        #region Intervention Performed
        private static Entry SetInterventionPerformed(DataRow drInterventionPerformed)
        {
            var codeValue = drInterventionPerformed["ICD"].ToString();
            var codeDescription = drInterventionPerformed["ICDDescription"].ToString();
            var valueset = drInterventionPerformed["valueset"].ToString();
            var codeSystemName = drInterventionPerformed["Title"].ToString();
            var objInterventionPerformed = new Entry()
            {
                Item = new Act()
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.12"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.32"}
                    },
                    id = new List<II>
                    {
                        new II {root = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                    : codeSystemName.Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                    : codeSystemName.Contains("ICD") ? "2.16.840.1.113883.6.4"
                                    : codeSystemName.Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                    : codeSystemName.Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                        valueSet = valueset
                    },
                    text = new ED()
                    {
                        Text = new List<string> { "Intervention, Performed: " + codeDescription }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"

                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            }
                        }
                    }
                }
            };
            return objInterventionPerformed;
        }

        #endregion

        #region Provider To Provider

        private static Entry SetProviderToProvider(DataRow drProviderToProvider)
        {
            var codeValue = string.IsNullOrEmpty(drProviderToProvider["SNOMED"].ToString()) ? "371530004" : drProviderToProvider["SNOMED"].ToString();
            var codeDescription = drProviderToProvider["SNOMEDDescription"].ToString();
            var valueset = "2.16.840.1.113883.3.464.1003.121.12.1006"; //drProviderToProvider["valueset"].ToString();
            var codeSystemName = "2.16.840.1.113883.6.96"; //drProviderToProvider["Title"].ToString();

            var objdrProviderToProvider = new Entry()
            {
                Item = new Act()
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.4"}
                    },
                    id = new List<II>
                    {
                        new II {root = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = codeSystemName,
                        valueSet = valueset
                    },
                    text = new ED()
                    {
                        Text = new List<string> { "Intervention, Performed: " + codeDescription }
                    },
                    statusCode = new CS()
                    {
                        code = "completed"

                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low, ItemsChoiceType2.high
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drProviderToProvider["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drProviderToProvider["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drProviderToProvider["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drProviderToProvider["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    participant = new List<Participant2>()
                    {
                        new Participant2()
                        {
                            typeCode = "AUT",
                            participantRole = new ParticipantRole()
                            {
                                classCode = "ASSIGNED",
                                code = new CE()
                                {
                                    code = "158965000",
                                    codeSystem = "2.16.840.1.113883.6.96",
                                    codeSystemName = "SNOMED CT",
                                    displayName = "Medical Practitioner"
                                }
                            }
                        },
                        new Participant2()
                        {
                            typeCode = "IRCP",
                            participantRole = new ParticipantRole()
                            {
                                classCode = "ASSIGNED",
                                code = new CE()
                                {
                                    code = "158965000",
                                    codeSystem = "2.16.840.1.113883.6.96",
                                    codeSystemName = "SNOMED CT",
                                    displayName = "Medical Practitioner"
                                }
                            }
                        }
                    }
                }
            };

            return objdrProviderToProvider;
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }
}