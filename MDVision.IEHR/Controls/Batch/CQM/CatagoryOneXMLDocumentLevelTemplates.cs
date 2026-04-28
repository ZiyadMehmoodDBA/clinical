using System;
using System.Collections.Generic;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.PQRS;

namespace MDVision.IEHR.Controls.Batch.CQM
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
            InformationRecipient();
            Custodian();
            LegalAuthenticator();
            DocumentOff();
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
                    root = "2.16.840.1.113883.10.20.22.1.1",
                    extension = "2015-08-01"
                },
                new II()
                {
                    root = "2.16.840.1.113883.10.20.24.1.1",
                    extension = "2016-02-01"
                },
                new II()
                {
                    root = "2.16.840.1.113883.10.20.24.1.2",
                    extension = "2016-02-01"
                },
                new II()
                {
                    root = "2.16.840.1.113883.10.20.24.1.3",
                    extension = "2015-07-01"
                }
            };
            _document.id = new II {root = Guid.NewGuid().ToString()};
            _document.code = new CE
            {
                code = "55182-0",
                displayName = "Quality Measure Report",
                codeSystem = "2.16.840.1.113883.6.1",
                codeSystemName = "LOINC"
            };
            _document.title = new ST()
            {
                Text = new List<string> { "QRDA Incidence Report" }
            };

            _document.effectiveTime = new TS
            {
                value = DateTime.Now.ToString("yyyyMMddHHmmss")
            };
            _document.confidentialityCode = new CE
            {
                code = "N",
                //displayName = "normal",
                codeSystem = "2.16.840.1.113883.5.25",
                //codeSystemName = "Confidentiality"

            };
            _document.languageCode = new CS
            {
                code = "en"
            };
        }
        #endregion

        #region record target
        private static void SetPatient_RecordTarget()
        {
            if (_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows.Count > 0)
            {
                var patientFirstName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.FirstNameColumn.ColumnName].ToString();
                var patientLastName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.LastNameColumn.ColumnName].ToString();
                var phone = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.HomePhoneNoColumn.ColumnName].ToString();

                if (!string.IsNullOrEmpty(phone))
                    if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-")))
                        phone = String.Format("{0:(###) ###-####}", double.Parse(phone));

                _document.recordTarget = new List<RecordTarget>()
                {
                    new RecordTarget()
                    {
                        patientRole = new PatientRole()
                        {
                            id = new List<II>
                            {
                                new II
                                {
                                    root = "2.16.840.1.113883.4.572",
                                    extension = Guid.NewGuid().ToString()
                                },
                                new II
                                {
                                    root = "1.3.6.1.4.1.115",
                                    extension = Guid.NewGuid().ToString()
                                }
                            },
                            addr = new List<AD>()
                            {
                                SetAddress( 
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.Address1Column.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.CityColumn.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.StateColumn.ColumnName].ToString(),
                                    _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.ZIPCodeColumn.ColumnName].ToString(),
                                    "US")
                            },
                            telecom = new List<TEL>()
                            {
                                new TEL
                                {
                                    value = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.HomePhoneNoColumn.ColumnName].ToString() == "" ? "" : "tel: +" + phone,
                                    use = new List<string> {"HP"}
                                }
                            },
                            patient = new Patient()
                            {
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
                                administrativeGenderCode = new CE
                                {
                                    code = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.GenderColumn.ColumnName].ToString() .Substring(0, 1),
                                    displayName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.GenderColumn.ColumnName].ToString(),
                                    codeSystem = "2.16.840.1.113883.5.1",
                                    codeSystemName = "HL7 AdministrativeGender"
                                },
                                birthTime = new TS
                                {
                                    value = Convert.ToDateTime( _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.DOBColumn.ColumnName].ToString()).ToString("yyyyMMdd")
                                },
                                maritalStatusCode = new CE
                                {
                                    code = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.MaritialStatusColumn.ColumnName].ToString() != "" ? _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.MaritialStatusColumn.ColumnName].ToString().Substring(0, 1):"U",
                                    displayName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.MaritialStatusColumn.ColumnName].ToString() == "" ? "Unknown":_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.MaritialStatusColumn.ColumnName].ToString(),
                                    codeSystem = "2.16.840.1.113883.5.2",
                                    codeSystemName = "MaritalStatusCode"
                                },
                                raceCode = new CE
                                {
                                    code = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.RaceCodeColumn.ColumnName].ToString(),
                                    displayName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.RaceDescriptionColumn.ColumnName].ToString(),
                                    codeSystem = "2.16.840.1.113883.6.238",
                                    codeSystemName = "CDC Race and Ethnicity"
                                },
                                ethnicGroupCode = new CE
                                {
                                    code = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.EthnicityCodeColumn.ColumnName].ToString(),
                                    displayName = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.EthnicityDescriptionColumn.ColumnName].ToString(),
                                    codeSystem = "2.16.840.1.113883.6.238",
                                    codeSystemName = "CDC Race and Ethnicity"
                                },
                                languageCommunication = new List<LanguageCommunication>
                                {
                                    new LanguageCommunication
                                    {
                                        templateId = new List<II>
                                        {
                                              new II
                                            {
                                                root = "2.16.840.1.113883.3.88.11.83.2",
                                                assigningAuthorityName = "HITSP/C83"
                                            },
                                            new II
                                            {
                                                root = "1.3.6.1.4.1.19376.1.5.3.1.2.1",
                                                assigningAuthorityName = "IHE/PCC"
                                            }
                                        },
                                        languageCode = new CS()
                                        {
                                            code = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.LanguageCodeColumn.ColumnName].ToString()
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
        private static void Person_Author()
        {
            _document.author = new List<Author>();
            var objAuthor = new Author();
            _document.author.Add(objAuthor);
            objAuthor.time = new TS
            {
                value = DateTime.Now.ToString("yyyyMMddHHmmss")
            };
            objAuthor.assignedAuthor = new AssignedAuthor
            {
                id = new List<II>
                {
                    new II
                    {
                        root = "2.16.840.1.113883.4.6",
                        extension = string.IsNullOrEmpty( _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.NPIColumn.ColumnName].ToString())? "123456789" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.NPIColumn.ColumnName].ToString()
                    }
                },
                code = new CE
                {
                    code = "163W00000X",
                    displayName = "Registered nurse",
                    codeSystem = "2.16.840.1.113883.5.53",
                    codeSystemName = "Health Care Provider Taxonomy"
                },
                addr = new List<AD>()
            };
            var addrs = SetAddress ( 
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString() == ""? "N/A" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString(),
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn.ColumnName].ToString() == "" ? "N/A" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn.ColumnName].ToString(),
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn.ColumnName].ToString() == "" ? "N/A" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn.ColumnName].ToString(),
                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString() == "" ? "N/A" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][ _dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString(),
                    "US" );

            objAuthor.assignedAuthor.addr.Add(addrs);
            var phone = _dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.HomePhoneNoColumn.ColumnName].ToString();
            if (!string.IsNullOrEmpty(phone)) if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-"))) phone = String.Format("{0:(###) ###-####}", double.Parse(phone));

            objAuthor.assignedAuthor.telecom = new List<TEL>();
            var objTel = new TEL
            {
                value = _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn.ColumnName].ToString() == "" ? "" : "tel: +" + phone,
                use = new List<string> {"HP"}

            };
            objAuthor.assignedAuthor.telecom.Add(objTel);
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
                            Text = new List<string> { _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.FirstNameColumn.ColumnName].ToString() }
                        },
                        new enfamily
                        {
                            Text = new List<string> { _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.LastNameColumn.ColumnName].ToString() }
                        }
                    }
                }
            };
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
            _document.custodian = new Custodian
            {
                assignedCustodian = new AssignedCustodian
                {
                    representedCustodianOrganization = new CustodianOrganization
                    {
                        id = new List<II>
                        {
                            new II
                            {
                                root = "2.16.840.1.113883.19.5",
                                extension = string.IsNullOrEmpty( _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.NPIColumn.ColumnName].ToString()) ? "123456789" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.NPIColumn.ColumnName].ToString()
                            }
                        },
                        name = new ON
                        {
                            Text = new List<string> { _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.DescriptionColumn.ColumnName].ToString() }
                        },
                        telecom = new TEL()
                    }
                }
            };
            var phone = _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.PhoneNoColumn.ColumnName].ToString();
            if (!string.IsNullOrEmpty(phone)) if (!(phone.Contains("(") || phone.Contains(")") || phone.Contains("-"))) phone = String.Format("{0:(###) ###-####}", double.Parse(phone));

            var telObj = new TEL
            {
                value = _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.PhoneNoColumn.ColumnName].ToString() == "" ? "" : "tel: +" + phone,
                use = new List<string> { "WP" }
            };
            _document.custodian.assignedCustodian.representedCustodianOrganization.telecom = telObj;
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

        #region Information Recepiant

        private static void InformationRecipient()
        {
            _document.informationRecipient = new List<InformationRecipient>
            {
                new InformationRecipient
                {
                    intendedRecipient = new IntendedRecipient
                    {
                        id = new List<II>
                        {
                            new II
                            {
                                root = "2.16.840.1.113883.3.249.7",
                                extension = "PQRS_MU_INDIVIDUAL"
                            }
                        }
                    }
                }
            };
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
                    id = new List<II>
                    {
                        new II
                        {
                            root =  Guid.NewGuid().ToString(), //"2.16.840.1.113883.4.6",
                            extension = _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.NPIColumn.ColumnName].ToString()
                        }
                    },
                    addr = new List<AD>()
                }
            };
            var objAddr = SetAddress(
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn.ColumnName].ToString(),
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn.ColumnName].ToString(),
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn.ColumnName].ToString(),
                                    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.ZIPCodeColumn.ColumnName].ToString(),
                                    "US"
                                    );
            _document.legalAuthenticator.assignedEntity.addr.Add(objAddr);
            _document.legalAuthenticator.assignedEntity.telecom = new List<TEL>
            {
                new TEL
                {
                    use = new List<string> { "WP" },
                    value = _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn].ToString() == "" ? 
                    "" : string.Format("{0:(###) ###-####}", "tel:" + _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn])
                }
            };
            _document.legalAuthenticator.assignedEntity.assignedPerson = new Person
            {
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
                id = new List<II>
                {
                    new II
                    {
                        root = "2.16.840.1.113883.19.5"
                    }
                },
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
            DateTime patientDob = Convert.ToDateTime(_dsPatientDemoGraphic.Tables[_dsPatientDemoGraphic.PatientsCQM.TableName].Rows[0][_dsPatientDemoGraphic.PatientsCQM.DOBColumn.ColumnName].ToString());
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
                                    value = patientDob.ToString("yyyyMMdd")
                                },
                                new IVXB_TS
                                {
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
                                        code = string.IsNullOrEmpty( _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.TaxonomyCodeColumn.ColumnName].ToString()) ? "123456789" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.TaxonomyCodeColumn.ColumnName].ToString(),
                                        codeSystem = "2.16.840.1.113883.6.101",
                                        codeSystemName = "Healthcare Provider Taxonomy (HIPAA)",
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
                use = new List<string>() { "HP" },
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