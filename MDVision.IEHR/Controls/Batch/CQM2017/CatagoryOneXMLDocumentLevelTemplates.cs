using System;
using System.Collections.Generic;
using MDVision.Datasets;

namespace MDVision.IEHR.Controls.Batch.CQM2017
{
    public class CatagoryOneXMLDocumentLevelTemplates : CatagoryOneXMLGenerator
    {

        static DSCQM _dsPatientDemoGraphic;
        static DSProfile _dsProvider;
        static DSProfile _dsPractice;

        static AmalgamatedClinicalDocument _document;

        public void BuildDocumentLevelTemplate(AmalgamatedClinicalDocument document, DSCQM dsPatientDemoGraphic, DSProfile dsProvider, DSProfile dsPractice)
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
            //DocumentOff();
        }

        #region General Header
        private static void SetHeader_Template()
        {
            _document.realmCode = new List<CS>
            {
                new CS
                {
                    code = "US"
                }
            };
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
                new II()
                {
                    root = "2.16.840.1.113883.10.20.24.1.1"
                    //,extension = "Which Time will be displayed Here ??????????" //"2014-06-09"
                },
                new II()
                {
                    root = "2.16.840.1.113883.10.20.24.1.2"
                    //,extension = "Which Time will be displayed Here ??????????" //"2014-06-09"
                }
            };
            _document.id = new II { root = Guid.NewGuid().ToString() };
            //<code code="55182-0" displayName="Quality Measure Report" codeSystemName="LOINC" codeSystem="2.16.840.1.113883.6.1"/>
            _document.code = new CE
            {
                code = "55182-0",
                displayName = "Quality Measure Report",
                codeSystem = "2.16.840.1.113883.6.1",
                codeSystemName = "LOINC"
            };
            _document.title = new ST()
            {
                Text = new List<string> { "Patient Chart Summary" }
            };

            _document.effectiveTime = new TS
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
                code = "en-US"
            };
        }
        #endregion

        #region record target
        private static void SetPatient_RecordTarget()
        {
            if (_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows.Count > 0)
            {
                var patientFirstName =
                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                        _dsPatientDemoGraphic.PatientsCQM.FirstNameColumn.ColumnName].ToString();

                var patientLastName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientsCQM.LastNameColumn.ColumnName].ToString();

                var phone = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientsCQM.HomePhoneNoColumn.ColumnName].ToString();

                if (!string.IsNullOrEmpty(phone))
                    if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-")))
                        phone =  "{double.Parse(phone):(###) ###-####}";

                //The recordTarget records the administrative and demographic data of the patient whose health information is described by the clinical document; each recordTarget must contain at least one patientRole element
                _document.recordTarget = new List<RecordTarget>()
                {
                    //SHALL contain at least one [1..*] recordTarget (CONF:1098-5266).
                    new RecordTarget()
                    {
                        //Such recordTargets SHALL contain exactly one [1..1] patientRole (CONF:1098-5267).
                        patientRole = new PatientRole()
                        {
                            //This patientRole SHALL contain at least one [1..*] id (CONF:1098-5268).
                            id = new List<II>
                            {
                                new II
                                {
                                    root = "2.16.840.1.113883.4.1",
                                    extension = "444-22-2222"
                                }
                            },
                            //This patientRole SHALL contain at least one [1..*] US Realm Address (AD.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.2) (CONF:1098-5271).
                            addr = new List<AD>()
                            {
                                SetAddress(
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientsCQM.Address1Column.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientsCQM.CityColumn.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientsCQM.StateColumn.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                                        _dsPatientDemoGraphic.PatientsCQM.ZIPCodeColumn.ColumnName].ToString(),
                                    "US")
                            },
                            //This patientRole SHALL contain at least one [1..*] telecom (CONF:1098-5280).
                            //1. Such telecoms SHOULD contain zero or one [0..1] @use, which SHALL be selected from ValueSet Telecom Use (US Realm Header) urn:oid:2.16.840.1.113883.11.20.9.20 DYNAMIC (CONF:1098-5375).
                            telecom = new List<TEL>()
                            {
                                new TEL
                                {
                                    value =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.HomePhoneNoColumn.ColumnName].ToString
                                            () == ""
                                            ? ""
                                            : "tel: +" + phone,
                                    use = new List<string> {"HP"}
                                }
                            },
                            patient = new Patient()
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
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.GenderColumn.ColumnName].ToString()
                                            .Substring(0, 1),
                                    displayName =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.GenderColumn.ColumnName].ToString(),
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
                                            _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName]
                                                .Rows[0][
                                                    _dsPatientDemoGraphic.PatientsCQM.DOBColumn.ColumnName].ToString())
                                            .ToString("yyyyMMdd")
                                },
                                //This patient SHOULD contain zero or one [0..1] maritalStatusCode, which SHALL be selected from ValueSet Marital Status urn:oid:2.16.840.1.113883.1.11.12212 DYNAMIC (CONF:1098-5303).
                                maritalStatusCode = new CE
                                {
                                    code =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.MaritialStatusColumn.ColumnName]
                                            .ToString()
                                            .Substring(0, 1),
                                    displayName =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.MaritialStatusColumn.ColumnName]
                                            .ToString(),
                                    codeSystem = "2.16.840.1.113883.5.2",
                                    codeSystemName = "MaritalStatusCode"
                                },
                                //This patient SHALL contain exactly one [1..1] raceCode, which SHALL be selected from ValueSet Race Category Excluding Nulls urn:oid:2.16.840.1.113883.3.2074.1.1.3 DYNAMIC (CONF:1098-5322).
                                raceCode = new CE
                                {
                                    code =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.RaceCodeColumn.ColumnName].ToString(),
                                    displayName =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.RaceDescriptionColumn.ColumnName]
                                            .ToString(),
                                    codeSystem = "2.16.840.1.113883.6.238",
                                    codeSystemName = "Race & Ethnicity - CDC"
                                },
                                //This patient SHALL contain exactly one [1..1] ethnicGroupCode, which SHALL be selected from ValueSet Ethnicity urn:oid:2.16.840.1.114222.4.11.837 DYNAMIC (CONF:1098-5323).
                                ethnicGroupCode = new CE
                                {
                                    code =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.EthnicityCodeColumn.ColumnName]
                                            .ToString(),
                                    displayName =
                                        _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0
                                            ][
                                                _dsPatientDemoGraphic.PatientsCQM.EthnicityDescriptionColumn.ColumnName]
                                            .ToString(),
                                    codeSystem = "2.16.840.1.113883.6.238",
                                    codeSystemName = "Race & Ethnicity - CDC"
                                },
                                //This patient SHALL contain at least one [1..*] languageCommunication (CONF:1098-5406).
                                //a. Such languageCommunications SHALL contain exactly one [1..1] languageCode, which SHALL be selected from ValueSet PatientLanguage urn:oid:2.16.840.1.113883.11.20.9.64 DYNAMIC (CONF:1098-5407).
                                //b. Such languageCommunications MAY contain zero or one [0..1] modeCode, which SHALL be selected from ValueSet LanguageAbilityMode urn:oid:2.16.840.1.113883.1.11.12249 DYNAMIC (CONF:1098-5409).
                                //c. Such languageCommunications SHOULD contain zero or one [0..1] proficiencyLevelCode, which SHALL be selected from ValueSet LanguageAbilityProficiency urn:oid:2.16.840.1.113883.1.11.12199 DYNAMIC (CONF:1098-9965).
                                //d. Such languageCommunications SHOULD contain zero or one [0..1] preferenceInd (CONF:1098-5414).
                                languageCommunication = new List<LanguageCommunication>
                                {
                                    new LanguageCommunication
                                    {
                                        languageCode = new CS()
                                        {
                                            code =
                                                _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName
                                                    ].Rows[0][
                                                        _dsPatientDemoGraphic.PatientsCQM.LanguageCodeColumn.ColumnName]
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

        #region Author, Old but hardly gold

        ////The author element represents the creator of the clinical document. The author may be a device or a person.
        //private static void Person_Author()
        //{
        //    //This assignedAuthor SHALL contain at least one [1..*] telecom (CONF:1098-5428).
        //    var phone = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.HomePhoneNoColumn.ColumnName].ToString();
        //    if (!string.IsNullOrEmpty(phone))
        //        if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-")))
        //            phone = String.Format("{0:(###) ###-####}", double.Parse(phone));

        //    _document.author = new List<Author>()
        //    {
        //        //SHALL contain at least one [1..*] author (CONF:1098-5444).
        //        new Author()
        //        {
        //            //Such authors SHALL contain exactly one [1..1] US Realm Date and Time (DTM.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.4)
        //            time = new TS()
        //            {
        //                value = DateTime.Now.ToString("yyyyMMddHHmmss")
        //            },

        //            //Such authors SHALL contain exactly one [1..1] assignedAuthor (CONF:1098-5448).
        //            assignedAuthor = new AssignedAuthor
        //            {
        //                //This assignedAuthor SHALL contain at least one [1..*] id (CONF:1098-5449).
        //                id = new List<II>
        //                {
        //                    new II
        //                    {
        //                        root = "2.16.840.1.113883.4.6",
        //                        extension =
        //                            string.IsNullOrEmpty(
        //                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                    _dsProvider.Provider.NPIColumn.ColumnName].ToString())
        //                                ? "123456789"
        //                                : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                    _dsProvider.Provider.NPIColumn.ColumnName].ToString()
        //                    }
        //                },
        //                //This assignedAuthor SHOULD contain zero or one [0..1] code (CONF:1098-16787).
        //                code = new CE
        //                {
        //                    code = "163W00000X",
        //                    displayName = "Registered nurse",
        //                    codeSystem = "2.16.840.1.113883.5.53",
        //                    codeSystemName = "Health Care Provider Taxonomy"
        //                },

        //                //This assignedAuthor SHALL contain at least one [1..*] US Realm Address (AD.US.FIELDED) (identifier: urn:oid:2.16.840.1.113883.10.20.22.5.2) (CONF:1098-5452).
        //                addr = new List<AD>()
        //                {
        //                    SetAddress(
        //                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                    _dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString() == ""
        //                                    ? "N/A"
        //                                    : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                        _dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString(),
        //                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                    _dsProvider.Provider.CityColumn.ColumnName].ToString() == ""
        //                                    ? "N/A"
        //                                    : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                        _dsProvider.Provider.CityColumn.ColumnName].ToString(),
        //                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                    _dsProvider.Provider.StateColumn.ColumnName].ToString() == ""
        //                                    ? "N/A"
        //                                    : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                        _dsProvider.Provider.StateColumn.ColumnName].ToString(),
        //                                _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                    _dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString() == ""
        //                                    ? "N/A"
        //                                    : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                        _dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString(),
        //                                "US")
        //                },

        //                //This assignedAuthor SHALL contain at least one [1..*] telecom (CONF:1098-5428).
        //                telecom = new List<TEL>()
        //                {
        //                    new TEL()
        //                    {
        //                        value =
        //                            _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
        //                                _dsProvider.Provider.PhoneNoColumn.ColumnName].ToString()
        //                            == ""
        //                                ? ""
        //                                : "tel: +" + phone,
        //                        use = new List<string> { "HP" } // Best Recomendations : Should
        //                    }
        //                },

        //                //This assignedAuthor SHOULD contain zero or one [0..1] assignedPerson (CONF:1098-5430).
        //                Item = new List<PN>()
        //                {
        //                    new PN()
        //                    {
        //                        Items = new List<ENXP>
        //                        {
        //                            new engiven {  Text = new List<string> { _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.FirstNameColumn.ColumnName].ToString() }},
        //                            new enfamily { Text = new List<string> { _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.LastNameColumn.ColumnName].ToString()  }}
        //                        }
        //                    }
        //                }
        //                //This assignedAuthor SHOULD contain zero or one [0..1] assignedAuthoringDevice (CONF:1098-16783).
        //                // !Understood
        //            }                    
        //        }
        //    };
        //}

        //private static void TestMethod()
        //{
        //    _document.author = new List<Author>();       // <author> person
        //    Author objAuthor = new Author();
        //    _document.author.Add(objAuthor);
        //    objAuthor.time = new TS                 // time..will by dynamic
        //    {
        //        value = "20111231124411+0500"
        //    };
        //    objAuthor.assignedAuthor = new AssignedAuthor
        //    {
        //        id = new List<II>
        //        {
        //            new II
        //            {
        //                root = "2.16.840.1.113883.19.5", //NPI
        //                extension = "KP00017dev"
        //            }
        //        },
        //        code = new CE
        //        {
        //            code = "200000000X",
        //            codeSystem = "2.16.840.1.113883.6.101",
        //            displayName = "Allopathic Osteopathic Physicians"
        //        },
        //        addr = new List<AD>()
        //    };


        //    AD objAdress = SetAddress("21 North Ave.", "Burlington", "MA", "02368", "US"); // this is address tag content and parameters..will be dynamic
        //    objAuthor.assignedAuthor.addr.Add(objAdress);  // <addr>

        //    objAuthor.assignedAuthor.telecom = new List<TEL>();
        //    TEL objTelePhone = new TEL // use means telephone type like work or cell phone and value contains actual phone number..will be dynamic
        //    {
        //        use = new List<string> { "WP" },
        //        value = "tel: +" + "(555)555-1003"
        //    };
        //    objAuthor.assignedAuthor.telecom.Add(objTelePhone);

        //    AuthoringDevice objAssignedAuthorDevice = new AuthoringDevice();
        //    objAuthor.assignedAuthor.Item = objAssignedAuthorDevice;
        //    objAssignedAuthorDevice.manufacturerModelName = new SC
        //    {
        //        Text = new List<string>
        //        {
        //            "Good Health Medical Device"
        //        }
        //    };

        //    objAssignedAuthorDevice.softwareName = new SC
        //    {
        //        Text = new List<string>
        //        {
        //            "Good Health Report Generator"
        //        }
        //    };

        //}

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
                _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientsCQM.HomePhoneNoColumn.ColumnName].ToString();
            if (!string.IsNullOrEmpty(phone))
                if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-")))
                    phone =  "{double.Parse(phone):(###) ###-####}";

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
                    phone =  "{double.Parse(phone):(###) ###-####}";

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
                    //value = _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn].ToString() == "" ?
                    //"" :
                    //     "{"tel:" + _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn]:(###) ###-####}"
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
        // disable once UnusedMember.Local
        private static void DocumentOff()
        {
            DateTime patientDob =
                Convert.ToDateTime(_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][
                    _dsPatientDemoGraphic.PatientsCQM.DOBColumn.ColumnName].ToString());

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
}