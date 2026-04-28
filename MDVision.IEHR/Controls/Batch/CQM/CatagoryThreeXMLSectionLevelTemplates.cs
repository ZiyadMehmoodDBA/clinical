using MDVision.Business.BCommon;
using MDVision.Business.BLL;
using MDVision.Common.Utilities;
using MDVision.Datasets;
using MDVision.Model.Batch.CQM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services.Description;

namespace MDVision.IEHR.Controls.Batch.CQM
{
    public class CatagoryThreeXMLSectionLevelTemplates
    {
        private BLLCQM BLLCQMObj = null;
        public CatagoryThreeXMLSectionLevelTemplates()
        {
            BLLCQMObj = new BLLCQM();
        }
        static DSProfile _dsProvider;
        static DSProfile _dsPractice;
        private static DSCQM _dsPatientDataSection;

        private static DSCQM _dsMeasureSection;
        private static DSCQM _dsReportingParameterSection;
        private static AmalgamatedClinicalDocument _document;
        private string startDate = "";
        private string endDate = "";
        private static bool isIndividual = false;


        private StructuredBody StructuredBody
        {
            get { return _document.component.Item as StructuredBody; }
        }
        public void BuildSectionLevelTemplate(AmalgamatedClinicalDocument document, Int64 providerId, DSProfile dsProvider, DSProfile dsPractice, string _startDate, string _endDate, string fieldsJSON = null)
        {
            if (fieldsJSON != null)
            {
                isIndividual = true;
            }
            else
            {
                isIndividual = false;
            }
            _document = document;
            _dsProvider = dsProvider;
            _dsPractice = dsPractice;

            _dsMeasureSection = GetMeasureSection_CatagoryThree(providerId, _startDate, _endDate, fieldsJSON);

            //_dsReportingParameterSection = GetReportingParameterSection_CategoryThree(providerId, null);
            startDate = _startDate;
            endDate = _endDate;

            SectionLevelTemplates(_startDate, _endDate);
        }
        private static DSCQM GetMeasureSection_CatagoryThree(Int64 providerId, string startDate, string endDate, string fieldsJSON = null)
        {
            if (startDate == null) throw new ArgumentNullException("startDate");
            if (endDate == null) throw new ArgumentNullException("endDate");
            var obj = new BLObject<DSCQM>();


            if (fieldsJSON != null)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var ProblemsList = ser.Deserialize<CQMProblemSearch>(MDVUtility.ToStr(fieldsJSON));
                if (ProblemsList.Problems.Count > 0)
                {
                    ProblemsList.ProblemListXML = MDVUtility.GetXmlOfObject(typeof(List<ProblemsList>), ProblemsList.Problems);
                }
                obj = new BLLCQM().Load_CQM(MDVUtility.ToInt64(ProblemsList.ProviderId), MDVUtility.ToStr(ProblemsList.NPI), MDVUtility.ToStr(ProblemsList.DateFrom), MDVUtility.ToStr(ProblemsList.DateTo), MDVUtility.ToStr(ProblemsList.PatientId), 0, MDVUtility.ToStr(ProblemsList.CQMId), MDVUtility.ToStr(ProblemsList.TIN), MDVUtility.ToInt64(ProblemsList.ProviderTypeId), MDVUtility.ToStr(ProblemsList.Address), MDVUtility.ToStr(ProblemsList.InsurancePlan), MDVUtility.ToStr(ProblemsList.EthnicityIds), MDVUtility.ToStr(ProblemsList.RaceIds), MDVUtility.ToStr(ProblemsList.AgeCondition_text), MDVUtility.ToInt64(ProblemsList.Age), MDVUtility.ToStr(ProblemsList.Sex_text), ProblemsList.ProblemListXML, 1, 1000);
            }
            else
            {
                obj = new BLLCQM().Load_CQM(providerId, null, startDate, endDate);
            }

            _dsMeasureSection = obj.Data;
            return _dsMeasureSection;
        }
        private static DSCQM GetPatientDataSection_CategoryThree(Int64 patientId, string nqfId)
        {
            var obj = new BLLCQM().PatientDataSection(patientId, nqfId);
            _dsPatientDataSection = obj.Data;
            return _dsPatientDataSection;
        }
        private static DSCQM GetReportingParameterSection_CategoryThree(Int64 providerId, string nqfId)
        {
            var obj = new BLLCQM().ReportingParameterSection(providerId, nqfId);
            _dsReportingParameterSection = obj.Data;
            return _dsReportingParameterSection;
        }
        private void SectionLevelTemplates(string startDate, string endDate)
        {
            SetHeader_Template();
            RecordTarget();
            //Device_Author();
            Author();
            Custodian();
            LegalAuthenticator();
            Participant();
            DocumentationOf(startDate, endDate);
            ReportingParameters(startDate, endDate);
            MeasureSection();
        }

        #region Header
        private static void SetHeader_Template()
        {
            _document.realmCode = new List<CS> { new CS { code = "US" } };
            _document.typeId = new InfrastructureRoottypeId
            {
                root = "2.16.840.1.113883.1.3",
                extension = "POCD_HD000040"
            };
            _document.templateId = new List<II>
            {
                new II
                {
                    root = "2.16.840.1.113883.10.20.27.1.1", extension="2017-06-01",
                }
            };
            _document.id = new II { root = Guid.NewGuid().ToString() };
            _document.code = new CE()
            {
                code = "55184-6",
                codeSystem = "2.16.840.1.113883.6.1",
                codeSystemName = "LOINC",
                displayName = "Quality Reporting Document Architecture Calculated Summary Report"
            };
            _document.title = new ST()
            {
                Text = new List<string>()
                {
                    "QRDA Calculated Summary Report"
                }
            };
            _document.effectiveTime = new TS
            {
                value = DateTime.Now.ToString("yyyyMMddHHmmss")
            };
            _document.confidentialityCode = new CE
            {
                code = "N",
                codeSystem = "2.16.840.1.113883.5.25"
            };
            _document.languageCode = new CS
            {
                code = "en-US"
            };
            _document.versionNumber = new INT()
            {
                value = "1"
            };
        }

        #endregion

        #region record target
        private static void RecordTarget()
        {
            _document.recordTarget = new List<RecordTarget>()
            {
                new RecordTarget()
                {
                    patientRole = new PatientRole()
                    {
                        id = new List<II>()
                        {
                            new II()
                            {
                                nullFlavor = "NA"
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Author
        private static void Device_Author()
        {
            if (_dsProvider.Tables[_dsProvider.Provider.TableName].Rows.Count > 0)
            {
                _document.author = new List<Author>()
                {
                    new Author()
                    {
                        time = new TS()
                        {
                            value = DateTime.Now.ToString("yyyyMMddHHmmss")
                        },
                        assignedAuthor = new AssignedAuthor()
                        {
                            id = new List<II>()
                            {
                                new II()
                                {
                                    root = "2.16.840.1.113883.4.6",
                                    nullFlavor = "UNK"
                                }
                            },
                            code = new CE()
                            {
                                code = "",
                                codeSystem = "2.16.840.1.113883.6.101",
                                displayName = ""
                            },
                            addr = new List<AD>()
                            {
                                SetAddress("Address", "City", "State","ZipCode", "US")
                            },
                            telecom = new List<TEL>()
                            {
                                new TEL()
                                {
                                    use = new List<string> {"WP"},
                                    value = "tel:" + _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn]
                                }
                            },
                            Item = new AuthoringDevice()
                            {
                                manufacturerModelName = new SC()
                                {
                                    Text = new List<string>()
                                    {
                                        "Model Name"
                                    }
                                },
                                softwareName = new SC()
                                {
                                    Text = new List<string>()
                                    {
                                        "Software Name"
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
        private static void Author()
        {
            if (_dsProvider.Tables[_dsProvider.Provider.TableName].Rows.Count > 0)
            {
                _document.author = new List<Author>
                {
                    new Author
                    {
                        time = new TS
                        {
                            value = DateTime.Now.ToString("yyyyMMddHHmmss")
                        },
                        assignedAuthor = new AssignedAuthor()
                        {
                            id = new List<II>
                            {
                                new II
                                {
                                    root = "2.16.840.1.113883.4.6",
                                    nullFlavor = "UNK"
                                }
                            },
                            addr = new List<AD>
                            {
                                SetAddress
                                (
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn].ToString()) ? "Address 1" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn].ToString(),
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn].ToString() ) ? "City" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn].ToString(),
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn].ToString() ) ? "State" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn].ToString(),
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.ZIPCodeColumn].ToString() ) ? "ZipCode" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.ZIPCodeColumn].ToString(),
                                "US"
                                )
                            },
                            Item = new Person
                            {
                                name = new List<PN>
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
                                                        _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.FirstNameColumn].ToString()
                                                    }
                                            },
                                            new enfamily
                                            {
                                                Text =
                                                    new List<string>
                                                    {
                                                        _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.LastNameColumn].ToString()
                                                    }
                                            }
                                        }
                                    }
                                }
                            },
                            representedOrganization = new Organization
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "2.16.840.1.113883.4.6",
                                        extension = "1235323023"
                                    }
                                },
                                name = new List<ON>()
                                {
                                    new ON()
                                    {
                                        Text =
                                            new List<string>()
                                            {
                                                _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.DescriptionColumn].ToString()
                                            }
                                    }
                                }
                            }
                        }
                    } //
                };
            }
        }

        #endregion

        #region Custadian
        private static void Custodian()
        {
            _document.custodian = new Custodian()
            {
                assignedCustodian = new AssignedCustodian()
                {
                    representedCustodianOrganization = new CustodianOrganization()
                    {
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = "2.16.840.1.113883.19.5",
                                extension = DateTime.Now.ToString("yyyyMMdd")
                            }
                        },
                        name = new ON()
                        {
                            Text = new List<string>()
                            {
                                _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.DescriptionColumn].ToString()
                            }
                        },
                        telecom = new TEL()
                        {
                            use = new List<string> { "WP" },
                            value = "UNK"// "tel:" + _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.PhoneNoColumn]
                        },
                        addr = SetAddress
                        (
                            string.IsNullOrEmpty(_dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.Address1Column].ToString()) ? "Address 1" : _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.Address1Column].ToString(),
                            string.IsNullOrEmpty(_dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.CityColumn].ToString()) ? "City" : _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.CityColumn].ToString(),
                            string.IsNullOrEmpty(_dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.StateColumn].ToString()) ? "State" : _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.StateColumn].ToString(),
                            string.IsNullOrEmpty(_dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.ZIPCodeColumn].ToString()) ? "ZipCode" : _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.ZIPCodeColumn].ToString(),
                            "US"
                        )
                    }
                }
            };
        }

        #endregion

        #region LegalAuthnticator
        private static void LegalAuthenticator()
        {
            if (_dsPractice.Tables[_dsPractice.Practice.TableName].Rows.Count > 0)
            {
                _document.legalAuthenticator = new LegalAuthenticator()
                {
                    time = new TS()
                    {
                        value = DateTime.Now.ToString("yyyyMMddHHmmss")
                    },
                    signatureCode = new CS()
                    {
                        code = "S"
                    },
                    assignedEntity = new AssignedEntity()
                    {
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = "2.16.840.1.113883.4.6" //Guid.NewGuid().ToString()
                                , extension = "1235323023"
                            }
                        },
                        addr = new List<AD>()
                        {
                            SetAddress
                            (
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn].ToString()) ? "Address 1" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.OfficeAddressColumn].ToString(),
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn].ToString() ) ? "City" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.CityColumn].ToString(),
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn].ToString() ) ? "State" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.StateColumn].ToString(),
                                string.IsNullOrEmpty(_dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.ZIPCodeColumn].ToString() ) ? "ZipCode" : _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.ZIPCodeColumn].ToString(),
                                "US"
                            )
                        },
                        telecom = new List<TEL>()
                        {
                            new TEL()
                            {
                                use = new List<string>(){"WP"},
                                value = _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.PhoneNoColumn].ToString()
                            }
                        },
                        assignedPerson = new Person()
                        {
                            name = new List<PN>()
                            {
                                new PN()
                                {
                                    Items = new List<ENXP>
                                        {
                                            new engiven
                                            {
                                                Text =
                                                    new List<string>
                                                    {
                                                        _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.FirstNameColumn].ToString()
                                                    }
                                            },
                                            new enfamily
                                            {
                                                Text =
                                                    new List<string>
                                                    {
                                                        _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][_dsProvider.Provider.LastNameColumn].ToString()
                                                    }
                                            }
                                        }
                                }
                            }
                        },
                        representedOrganization = new Organization()
                        {
                            id = new List<II>()
                            {
                                new II()
                                {
                                    root = "2.16.840.1.113883.4.6",
                                    extension = "1235323023"
                                }
                            },
                            name = new List<ON>()
                            {
                                new ON()
                                {
                                    Text = new List<string>()
                                    {
                                        _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][_dsPractice.Practice.DescriptionColumn].ToString()
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }

        #endregion

        #region Participant
        private static void Participant()
        {
            _document.participant = new List<Participant1>()
            {
                new Participant1()
                {
                    typeCode = "DEV",
                    associatedEntity = new AssociatedEntity()
                    {
                        classCode = "RGPR",
                        id = new List<II>()
                        {
                            //<!-- if the EHR has an ONC certification number, SHOULD use it here -->
                            new II()
                            {
                                root = "2.16.840.1.113883.3.2074.1",
                                extension = "1a2b3c",
                                assigningAuthorityName = "ONC"
                            },
                            //<!-- if the EHR has a CMS Security Code, MAY use it here -->
                            new II()
                            {
                                root = "2.16.840.1.113883.3.249.21",
                                extension = "98765"
                            }
                        },
                        code = new CE()
                        {
                            code = "129465004",
                            codeSystem = "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOMED-CT",
                            displayName = "medical record, device"
                        }
                    }

                }
            };
        }

        #endregion

        #region DocumentationOf
        private static void DocumentationOf(string startDate, string endDate)
        {
            if (_dsProvider.Tables[_dsProvider.Provider.TableName].Rows.Count > 0)
            {
                _document.documentationOf = new List<DocumentationOf>()
                {
                    new DocumentationOf()
                    {
                        typeCode = "DOC",
                        serviceEvent = new ServiceEvent()
                        {
                            classCode = "PCPR",
                            effectiveTime = new IVL_TS()
                            {
                                ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                Items = new QTY[]
                                {
                                    new IVXB_TS
                                    {
                                        value = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate).ToString("yyyyMMdd") : DateTime.Now.ToString("yyyyMMdd")
                                    },
                                    new IVXB_TS
                                    {
                                        value = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate).ToString("yyyyMMdd") : DateTime.Now.ToString("yyyyMMdd")
                                    }
                                }
                            },
                            performer = new List<Performer1>()
                            {
                                new Performer1()
                                {
                                    typeCode = x_ServiceEventPerformer.PRF,
                                    //time = new IVL_TS()
                                    //{
                                    //    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                    //    Items = new QTY[]
                                    //    {
                                    //        new IVXB_TS
                                    //        {
                                    //            value = DateTime.Now.ToString("yyyyMMdd")
                                    //        },
                                    //        new IVXB_TS
                                    //        {
                                    //            value = DateTime.Now.ToString("yyyyMMdd")
                                    //        }
                                    //    }
                                    //},
                                    assignedEntity = new AssignedEntity()
                                    {
                                        id = new List<II>()
                                        {
                                            new II()
                                            {
                                                root = "2.16.840.1.113883.4.6",
                                                nullFlavor = "UNK"
                                                //extension =
                                                //    _dsProvider.Tables[_dsProvider.Provider.TableName].Rows[0][
                                                //        _dsProvider.Provider.NPIColumn.ColumnName].ToString(),
                                                //assigningAuthorityName = "NPI"
                                            }
                                        },
                                        representedOrganization = new Organization()
                                        {
                                            id = new List<II>()
                                            {
                                                new II()
                                                {
                                                    root = "2.16.840.1.113883.4.2",
                                                    extension =
                                                        _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][
                                                            _dsPractice.Practice.EINColumn.ColumnName].ToString(),
                                                    assigningAuthorityName = "TIN"
                                                },
                                                new II()
                                                {
                                                    root = "2.16.840.1.113883.4.336",
                                                    extension =
                                                        string.IsNullOrEmpty(
                                                            _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0][
                                                                _dsPractice.Practice.TaxonomyCodeColumn.ColumnName]
                                                                .ToString())
                                                            ? "54321"
                                                            : _dsPractice.Tables[_dsPractice.Practice.TableName].Rows[0]
                                                                [
                                                                    _dsPractice.Practice.TaxonomyCodeColumn.ColumnName]
                                                                .ToString(),
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
        }

        #endregion

        #region Reporting Parameters
        private void ReportingParameters(string startDate_, string endDate_)
        {
            var startDate = !string.IsNullOrEmpty(startDate_)
                    ? DateTime.Parse(startDate_).ToString("yyyy MMMM dd")
                    : DateTime.Now.ToString("yyyy MMMM dd");

            var endDate = !string.IsNullOrEmpty(endDate_)
                    ? DateTime.Parse(endDate_).ToString("yyyy MMMM dd")
                    : DateTime.Now.ToString("yyyy MMMM dd");

            var objComponent3 = new Component3
            {
                section = new Section
                {
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.17.2.1"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.2.2"
                        }
                    },
                    code = new CE()
                    {
                        code = "55187-9",
                        codeSystem = "2.16.840.1.113883.6.1"
                    },
                    title = new ST()
                    {
                        Text = new List<string> { "Reporting Parameters" }
                    },
                    text = new StrucDocText()
                    {
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
                                            "Reporting period: " + startDate + " ,  " + endDate
                                        }
                                    }
                                }
                            }
                        }
                    },
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
                                    codeSystem = "2.16.840.1.113883.6.96",
                                    codeSystemName = "SNOMED-CT"
                                },
                                effectiveTime = new IVL_TS
                                {
                                    ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                                    Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = DateTime.Parse(startDate).ToString("yyyyMMdd")
                                        },
                                        new IVXB_TS
                                        {
                                            value = DateTime.Parse(endDate).ToString("yyyyMMdd")
                                        }
                                    }
                                }
                            }
                        },
                        new Entry()
                        {
                            Item = new Encounter()
                            {
                                classCode = "ENC",
                                moodCode = x_DocumentEncounterMood.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.27.3.11"
                                    }
                                },
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                effectiveTime = new IVL_TS()
                                {
                                    ItemsElementName = new[] {ItemsChoiceType2.low},
                                    Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = DateTime.Parse(startDate).ToString("yyyyMMdd")
                                        }
                                    }
                                }
                            }
                        },
                        new Entry()
                        {
                            Item = new Encounter()
                            {
                                classCode = "ENC",
                                moodCode = x_DocumentEncounterMood.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.27.3.11"
                                    }
                                },
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = Guid.NewGuid().ToString()
                                    }
                                },
                                effectiveTime = new IVL_TS()
                                {
                                    ItemsElementName = new[] {ItemsChoiceType2.low},
                                    Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = DateTime.Parse(endDate).ToString("yyyyMMdd")
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            StructuredBody.component.Add(objComponent3);
        }

        #endregion

        #region Measure Section

        private void MeasureSection()
        {
            #region Header Fixed

            var objComponent3 = new Component3
            {
                section = new Section()
                {
                    templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.2.2",
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.27.2.1", extension="2017-06-01"
                        },
                         new II
                        {
                            root = "2.16.840.1.113883.10.20.27.2.3", extension="2017-06-01"
                        }
                    },
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
                    },
                    entry = new List<Entry>()
                }
            };

            #endregion

            #region Table Section, For XML/XSLT, Presentable

            if (_dsMeasureSection.CQM.Rows.Count > 0)
            {
                for (var i = 0; i < _dsMeasureSection.CQM.Rows.Count; i++)
                {
                    // Measure Section Header
                    if (_dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].Rows.Count > 0)
                    {
                        var isMeasure =
                            _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                                .Where(r => r.Field<string>("CQMID") ==
                                            _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                                _dsMeasureSection.CQM.CQMIDColumn].ToString());

                        var isMeasureCount = isMeasure.AsEnumerable().Count(r => r.Field<string>("CQMID") ==
                                                                                 _dsMeasureSection.Tables[
                                                                                     _dsMeasureSection.CQM.TableName]
                                                                                     .Rows[i
                                                                                     ][_dsMeasureSection.CQM.CQMIDColumn
                                                                                     ]
                                                                                     .ToString());

                        var cqmId =
                            _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                _dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();

                        //ZK
                        //if (
                        //    cqmId == "0421a" || cqmId == "0421b"
                        //    //|| cqmId == "0022" || cqmId == "0028" || cqmId == "0043" ||
                        //    //cqmId == "0068" || cqmId == "0419" || cqmId == "CMS50v3" || cqmId == "0075" || cqmId == "0041"
                        //    )
                        //{

                        if (UInt64.Parse(isMeasureCount.ToString()) > 0)
                        {

                            // Handle for This Particular Measure
                            if (
                                _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                    _dsMeasureSection.CQM.CQMIDColumn].ToString() != "0421b")
                            {
                                objComponent3.section.text.Items.Add(
                                    StrucDocTableMeasureSection(
                                        _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i]));

                                //var objContent = new StrucDocContent
                                //{
                                //    styleCode = "Bold",
                                //    Text = new List<string>()
                                //    {
                                //        "Member of Measure Set: Clinical Quality Measure Set - b6ac13e2-beb8-4e4f-94ed-fcc397406cd8"
                                //    }
                                //};
                                //objComponent3.section.text.Items.Add(objContent);
                            }

                            var objList = new StrucDocList()
                            {
                                item = new List<StrucDocItem>()
                                    {
                                        new StrucDocItem()
                                        {
                                            Items = new List<object>()
                                            {
                                                new StrucDocContent()
                                                {
                                                    styleCode = "Bold",
                                                    Text = new List<string>() {"Performance Rate: "}
                                                }
                                            },
                                            Text = new List<string>()
                                            {
                                                string.IsNullOrEmpty(
                                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                                        _dsMeasureSection.CQM.PerfromanceRate1Column.ColumnName]
                                                        .ToString())
                                                    ? ""
                                                    : _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                                        _dsMeasureSection.CQM.PerfromanceRate1Column.ColumnName] + "%"
                                            }
                                        },
                                        new StrucDocItem()
                                        {
                                            Items = new List<object>()
                                            {
                                                new StrucDocContent()
                                                {
                                                    styleCode = "Bold",
                                                    Text = new List<string>() {"Reporting Rate: "}
                                                }
                                            },
                                            Text = new List<string>()
                                            {
                                                string.IsNullOrEmpty(
                                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                                        _dsMeasureSection.CQM.ReportingRate1Column.ColumnName].ToString())
                                                    ? ""
                                                    : _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                                        _dsMeasureSection.CQM.ReportingRate1Column.ColumnName] + "%"
                                            }
                                        }

                                    }
                            };
                            if (isIndividual == false && (cqmId == "CMS50v3" || cqmId == "0043" || cqmId == "0059" || cqmId == "CMS22v5" || cqmId == "0421a" || cqmId == "0075(A)" || cqmId == "0018" || cqmId == "0068" || cqmId == "0712(A)" || cqmId == "0022(A)" || cqmId == "2872" || cqmId == "0101" || cqmId == "0028" || cqmId == "0062" || cqmId == "0065" || cqmId == "0419" || cqmId == "0418" || cqmId == "0041" || cqmId == "0056" || cqmId == "0031" || cqmId == "0034"))
                            {
                                objComponent3.section.entry.Add(
                                   Set_ReportingParameters());
                            }
                            if (isIndividual == true && (cqmId == "CMS50v3" || cqmId == "0043" || cqmId == "0059" || cqmId == "CMS22v5" || cqmId == "0421a" || cqmId == "0075(A)" || cqmId == "0018" || cqmId == "0068" || cqmId == "0712(A)" || cqmId == "0022(A)" || cqmId == "2872" || cqmId == "0101" || cqmId == "0028" || cqmId == "0062" || cqmId == "0065" || cqmId == "0419" || cqmId == "0418" || cqmId == "0041" || cqmId == "0056" || cqmId == "0031" || cqmId == "0034"))
                            {
                                objComponent3.section.entry.Add(
                                   Set_ReportingParameters());
                            }

                            #region For 0075 and 0022

                            var objList1 = new StrucDocList()
                            {
                                item = new List<StrucDocItem>()
                                //{
                                //    new StrucDocItem()
                                //    {
                                //        Items = new List<object>()
                                //        {
                                //            new StrucDocContent()
                                //            {
                                //                styleCode = "Bold",
                                //                Text = new List<string>() {"Performance Rate: "}
                                //            }
                                //        },
                                //        Text = new List<string>()
                                //        {
                                //            string.IsNullOrEmpty(
                                //                _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                //                    _dsMeasureSection.CQM.PerfromanceRate2Column.ColumnName]
                                //                    .ToString())
                                //                ? ""
                                //                : _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                //                    _dsMeasureSection.CQM.PerfromanceRate2Column.ColumnName] + "%"
                                //        }
                                //    },
                                //    new StrucDocItem()
                                //    {
                                //        Items = new List<object>()
                                //        {
                                //            new StrucDocContent()
                                //            {
                                //                styleCode = "Bold",
                                //                Text = new List<string>() {"Reporting Rate: "}
                                //            }
                                //        },
                                //        Text = new List<string>()
                                //        {
                                //            string.IsNullOrEmpty(
                                //                _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                //                    _dsMeasureSection.CQM.ReportingRate2Column.ColumnName].ToString())
                                //                ? ""
                                //                : _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                //                    _dsMeasureSection.CQM.ReportingRate2Column.ColumnName] + "%"
                                //        }
                                //    }
                                //}
                            };

                            #endregion

                            #region Counts

                            var initialPopulationCount =
                                Convert.ToInt32(
                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                        _dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString());
                            var denominatorCount =
                                Convert.ToInt32(
                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                        _dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString());
                            var denominatorExclusionCount =
                                Convert.ToInt32(
                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                        _dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString());
                            var denominatorExceptionCount =
                                Convert.ToInt32(
                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                        _dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString());
                            var numeratorCount =
                                Convert.ToInt32(
                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                        _dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString());
                            var numeratorExclusionCount =
                                Convert.ToInt32(
                                    _dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName].Rows[i][
                                        _dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString());


                            #endregion

                            #region 14.1, Initial_Population, Denominator, DenominatorExclusion, DenominatorException, Numerator, NumeratorExclusion

                            if (initialPopulationCount > 0)
                            {
                                objList.item.Add(Initial_Population(initialPopulationCount, cqmId));
                            }
                            if (denominatorCount > 0)
                            {
                                objList.item.Add(Denominator(denominatorCount, cqmId));
                            }
                            if (denominatorExclusionCount > 0)
                            {
                                objList.item.Add(DenominatorExclusion(denominatorExclusionCount, cqmId));
                            }

                            //if (cqmId == "0041")
                            //{
                            if (denominatorExceptionCount > 0)
                            {
                                objList.item.Add(DenominatorException(denominatorExceptionCount, cqmId));
                            }
                            //}

                            if (cqmId == "0075" || cqmId == "0022")
                            {
                                if (numeratorCount > 0)
                                    objList.item.Add(Numerator(numeratorCount, cqmId));
                                if (numeratorExclusionCount > 0)
                                    objList1.item.Add(Numerator(numeratorExclusionCount, cqmId));
                            }
                            else
                            {
                                if (numeratorCount > 0)
                                    objList.item.Add(Numerator(numeratorCount, cqmId));
                            }

                            //if (numeratorExclusionCount > 0)
                            //{
                            //    objList.item.Add(NumeratorExclusion(numeratorExclusionCount, cqmId));
                            //}

                            objComponent3.section.text.Items.Add(objList);

                            if (cqmId == "0075" || cqmId == "0022")
                            {
                                if (numeratorExclusionCount > 0)
                                {
                                    objComponent3.section.text.Items.Add(objList1);
                                }
                            }

                            #endregion
                        }
                        //}
                    }
                }
            }

            #endregion

            #region Entry Section, For XML/XSLT, CypressVaildates

            for (var i = 0; i < _dsMeasureSection.CQM.Rows.Count; i++)
            {
                if (
                    _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString() !=
                    "0" &&
                    !string.IsNullOrEmpty(
                        _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString
                            ()))
                {

                    //ZK
                    _dsPatientDataSection = GetPatientDataSection_CategoryThree(0, "");

                    var dtPatientDataSection = _dsPatientDataSection.Tables[_dsPatientDataSection.CatagoryIII_PopulationValueSet.TableName].AsEnumerable()
                                   .Where(r => r.Field<string>("CQMId") == _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString());


                    //if (
                    //    _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0421a" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0421b" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0022" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0028" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0043" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0068" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0419" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "CMS50v3" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0075" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() ==
                    //    "0041"
                    //    )                    
                    //{

                    //}
                    //if (_dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() == "0022(A)" )
                    //{
                    if (_dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() == "0022(A)" || _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() == "0075(A)")
                    {
                        objComponent3.section.entry.Add(MeasureSection_EntrySection_421(_dsMeasureSection.CQM.Rows[i], _dsMeasureSection.CQM.Rows[i + 1], dtPatientDataSection));
                    }
                    else if (_dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() == "0712(A)")
                    {
                        objComponent3.section.entry.Add(MeasureSection_EntrySection_160(_dsMeasureSection.CQM.Rows[i], _dsMeasureSection.CQM.Rows[i + 1], _dsMeasureSection.CQM.Rows[i + 2], dtPatientDataSection));
                    }
                    else
                    {
                        if (_dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() != "0022(B)" && _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() != "0075(B)" && _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() != "0712(B)" && _dsMeasureSection.CQM.Rows[i][_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString() != "0712(C)")
                        {
                            objComponent3.section.entry.Add(MeasureSection_EntrySection(_dsMeasureSection.CQM.Rows[i], dtPatientDataSection));
                        }

                    }
                    //}
                }
            }

            #endregion

            StructuredBody.component.Add(objComponent3);
        }

        #endregion

        private static Entry Set_ReportingParameters()
        {

            var objReportingParameters = new Entry()
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
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED-CT"
                    },
                    effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = DateTime.Now.ToString("yyyyMMdd")
                                        },
                                        new IVXB_TS
                                        {
                                            value = DateTime.Now.ToString("yyyyMMdd")
                                        }
                                    }
                    }
                }


            };
            return objReportingParameters;
        }

        #region MeasureSection Part=1 -- XML, Visisble

        #region (14.2) Measure Section, XSL Schema To Be Visible, [Completed]
        private StrucDocItem Initial_Population(int count, string cqmId)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable().Where(r => r.Field<string>("CQMID") == cqmId);

            var objInitialPopulation = new StrucDocItem()
            {
                Items = new List<object>
                {
                    new StrucDocContent
                    {
                        styleCode = "Bold",
                        Text = new List<string>() {"InitialPatient Population: " + count}
                    }
                },
                Text = new List<string>()
            };

            var objStrucDocList = new StrucDocList
            {
                item = new List<StrucDocItem>()
            };

            var rowCollection = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("InitialPopulation") == "1");

            #region Gender, Ethnicity, Race, Medicare, Medicaid

            if (dtExists.Any())
            {
                GenderHandle(objStrucDocList, rowCollection, "InitialPopulation");
                EthnicityHandle(objStrucDocList, rowCollection, "InitialPopulation");
                RaceHandle(objStrucDocList, rowCollection, "InitialPopulation");
                MedicareHandle(objStrucDocList, rowCollection, "InitialPopulation");
                MedicaidHandle(objStrucDocList, rowCollection, "InitialPopulation");
            }

            #endregion

            objInitialPopulation.Items.Add(objStrucDocList);
            return objInitialPopulation;

        }
        private StrucDocItem Denominator(int count, string cqmId)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId);

            var strucDocItemDenominator = new StrucDocItem()
            {
                Items = new List<object>
                {
                    new StrucDocContent
                    {
                        styleCode = "Bold",
                        Text = new List<string>() {"Denominator: " + count}
                    }
                },
                Text = new List<string>()
            };

            var objStrucDocList = new StrucDocList
            {
                item = new List<StrucDocItem>()
            };

            var rowCollection = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                    .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("Denominator") == "1");

            #region Gender, Ethnicity, Race, Medicare, Medicaid

            if (dtExists.Any())
            {
                GenderHandle(objStrucDocList, rowCollection, "Denominator");
                EthnicityHandle(objStrucDocList, rowCollection, "Denominator");
                RaceHandle(objStrucDocList, rowCollection, "Denominator");
                MedicareHandle(objStrucDocList, rowCollection, "Denominator");
                MedicaidHandle(objStrucDocList, rowCollection, "Denominator");
            }


            #endregion

            strucDocItemDenominator.Items.Add(objStrucDocList);
            return strucDocItemDenominator;
        }
        private StrucDocItem DenominatorExclusion(int count, string cqmId)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId);

            var strucDocItemDenominatorExclusion = new StrucDocItem()
            {
                Items = new List<object>()
                {
                    new StrucDocContent
                    {
                        styleCode = "Bold",
                        Text = new List<string>() {"Denominator Exclusions: " + count}
                    }
                },
                Text = new List<string>()
            };

            StrucDocList objStrucDocList = new StrucDocList
            {
                item = new List<StrucDocItem>()
            };

            var rowCollection = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                    .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("DenominatorExclusion") == "1");

            #region Gender, Ethnicity, Race, Medicare, Medicaid

            if (dtExists.Any())
            {
                GenderHandle(objStrucDocList, rowCollection, "DenominatorExclusion");
                EthnicityHandle(objStrucDocList, rowCollection, "DenominatorExclusion");
                RaceHandle(objStrucDocList, rowCollection, "DenominatorExclusion");
                MedicareHandle(objStrucDocList, rowCollection, "DenominatorExclusion");
                MedicaidHandle(objStrucDocList, rowCollection, "DenominatorExclusion");
            }

            #endregion

            strucDocItemDenominatorExclusion.Items.Add(objStrucDocList);
            return strucDocItemDenominatorExclusion;
        }
        private StrucDocItem DenominatorException(int count, string cqmId)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
               .Where(r => r.Field<string>("CQMID") == cqmId);

            var strucDocItemDenominatorException = new StrucDocItem()
            {
                Items = new List<object>()
                {
                    new StrucDocContent
                    {
                        styleCode = "Bold",
                        Text = new List<string>() {"Denominator Exception: " + count}
                    }
                },
                Text = new List<string>()
            };

            var objStrucDocList = new StrucDocList
            {
                item = new List<StrucDocItem>()
            };

            var rowCollection = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                    .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("DenominatorException") == "1");

            #region Gender, Ethnicity, Race, Medicare, Medicaid

            if (dtExists.Any())
            {
                GenderHandle(objStrucDocList, rowCollection, "DenominatorException");
                EthnicityHandle(objStrucDocList, rowCollection, "DenominatorException");
                RaceHandle(objStrucDocList, rowCollection, "DenominatorException");
                MedicareHandle(objStrucDocList, rowCollection, "DenominatorException");
                MedicaidHandle(objStrucDocList, rowCollection, "DenominatorException");
            }

            #endregion

            strucDocItemDenominatorException.Items.Add(objStrucDocList);
            return strucDocItemDenominatorException;
        }
        private StrucDocItem Numerator(int count, string cqmId)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
               .Where(r => r.Field<string>("CQMID") == cqmId);
            var strucDocItemNumerator = new StrucDocItem()
            {
                Items = new List<object>()
                {
                    new StrucDocContent
                    {
                        styleCode = "Bold",
                        Text = new List<string>() {"Numerator: " + count}
                    }
                },
                Text = new List<string>()
            };

            StrucDocList objStrucDocList = new StrucDocList
            {
                item = new List<StrucDocItem>()
            };

            var rowCollection = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                    .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("Numerator") == "1");

            #region Gender, Ethnicity, Race, Medicare, Medicaid

            if (dtExists.Any())
            {
                GenderHandle(objStrucDocList, rowCollection, "Numerator");
                EthnicityHandle(objStrucDocList, rowCollection, "Numerator");
                RaceHandle(objStrucDocList, rowCollection, "Numerator");
                MedicareHandle(objStrucDocList, rowCollection, "Numerator");
                MedicaidHandle(objStrucDocList, rowCollection, "Numerator");
            }

            #endregion

            strucDocItemNumerator.Items.Add(objStrucDocList);
            return strucDocItemNumerator;
        }
        private StrucDocItem NumeratorExclusion(int count, string cqmId)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId);

            var strucDocItemNumeratorExclusion = new StrucDocItem()
            {
                Items = new List<object>()
                {
                    new StrucDocContent
                    {
                        styleCode = "Bold",
                        Text = new List<string>() {"Numerator Exclusion: " + count}
                    }
                },
                Text = new List<string>()
            };

            StrucDocList objStrucDocList = new StrucDocList
            {
                item = new List<StrucDocItem>()
            };

            var rowCollection = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                    .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("NumeratorExclusion") == "1");

            #region Gender, Ethnicity, Race, Medicare, Medicaid

            if (dtExists.Any())
            {
                GenderHandle(objStrucDocList, rowCollection, "NumeratorExclusion");
                EthnicityHandle(objStrucDocList, rowCollection, "NumeratorExclusion");
                RaceHandle(objStrucDocList, rowCollection, "NumeratorExclusion");
                MedicareHandle(objStrucDocList, rowCollection, "NumeratorExclusion");
                MedicaidHandle(objStrucDocList, rowCollection, "NumeratorExclusion");
            }

            #endregion

            strucDocItemNumeratorExclusion.Items.Add(objStrucDocList);
            return strucDocItemNumeratorExclusion;
        }

        #region Sub of (14.2)
        private void GenderHandle(StrucDocList objStrucDocList, EnumerableRowCollection<DataRow> drEnumerableRowCollectionGender, string columnName)
        {
            if (!drEnumerableRowCollectionGender.Any()) return;

            var dtGender = drEnumerableRowCollectionGender.CopyToDataTable();
            var dvGender = new DataView(dtGender);
            var dtGenderDistinct = dvGender.ToTable(true, "Gender");

            for (var i = 0; i < dtGenderDistinct.Rows.Count; i++)
            {
                if (dtGender.Rows[i][columnName].ToString() != "1") continue;

                var dtGenderMale = dtGender.AsEnumerable().Count(r => r.Field<string>("Gender") == "Male");
                var dtGenderFemale =
                    dtGender.AsEnumerable().Count(r => r.Field<string>("Gender") == "Female");
                var dtGenderOther = dtGender.AsEnumerable()
                    .Count(r => r.Field<string>("Gender") == "Other");

                var objGenderMale = new StrucDocItem()
                {
                    Items = new List<object>()
                    {
                        new StrucDocContent
                        {
                            styleCode = "Bold",
                            Text = new List<string>()
                            {
                                dtGenderDistinct.Rows[i][0] + ": "
                            }
                        }
                    },
                    Text = new List<string>()
                    {
                        dtGenderDistinct.Rows[i][0].ToString() == "Male"
                            ? dtGenderMale.ToString()
                            : dtGenderDistinct.Rows[i][0].ToString() == "Female"
                                ? dtGenderFemale.ToString()
                                : dtGenderOther.ToString()
                    }
                };
                objStrucDocList.item.Add(objGenderMale);
            }
        }
        private void EthnicityHandle(StrucDocList objStrucDocList, EnumerableRowCollection<DataRow> drEnumerableRowCollectionEthnicity, string columnName)
        {
            if (!drEnumerableRowCollectionEthnicity.Any()) return;

            var dtEthnicity = drEnumerableRowCollectionEthnicity.CopyToDataTable();

            var dvEthnicity = new DataView(dtEthnicity);
            var dtEthnicityDistinct = dvEthnicity.ToTable(true, "EthnicityDescription");

            for (var i = 0; i < dtEthnicityDistinct.Rows.Count; i++)
            {
                if (dtEthnicity.Rows[i][columnName].ToString() != "1") continue;

                var hispanicOrLatino =
                    dtEthnicity.AsEnumerable()
                        .Count(r => r.Field<string>("EthnicityDescription") == "Hispanic or Latino");

                var notHispanicOrLatino =
                    dtEthnicity.AsEnumerable()
                        .Count(r => r.Field<string>("EthnicityDescription") == "Not Hispanic or Latino");

                var refusedToReport =
                    dtEthnicity.AsEnumerable()
                        .Count(r => r.Field<string>("EthnicityDescription") == "Refused to Report");

                var unknown =
                    dtEthnicity.AsEnumerable()
                        .Count(r => r.Field<string>("EthnicityDescription") == "Unknown");

                var objEthnicityMale = new StrucDocItem()
                {
                    Items = new List<object>()
                    {
                        new StrucDocContent
                        {
                            styleCode = "Bold",
                            Text = new List<string>()
                            {
                                dtEthnicityDistinct.Rows[i][0] + ": "
                            }
                        }
                    },
                    Text = new List<string>()
                    {
                        dtEthnicityDistinct.Rows[i][0].ToString() == "Hispanic or Latino"
                            ? hispanicOrLatino.ToString()
                            : dtEthnicityDistinct.Rows[i][0].ToString() == "Not Hispanic or Latino"
                                ? notHispanicOrLatino.ToString()
                                : dtEthnicityDistinct.Rows[i][0].ToString() == "Refused to Report"
                                    ? refusedToReport.ToString()
                                    : unknown.ToString()
                    }
                };
                objStrucDocList.item.Add(objEthnicityMale);
            }
        }
        private void RaceHandle(StrucDocList objStrucDocList, EnumerableRowCollection<DataRow> drEnumerableRowCollectionRace, string columnName)
        {
            if (!drEnumerableRowCollectionRace.Any()) return;

            var dtRace = drEnumerableRowCollectionRace.CopyToDataTable();

            var dvRace = new DataView(dtRace);
            var dtRaceDistinct = dvRace.ToTable(true, "RaceDescription");

            for (var i = 0; i < dtRaceDistinct.Rows.Count; i++)
            {
                if (dtRace.Rows[i][columnName].ToString() != "1") continue;

                var americanIndianorAlaskaNative =
                    dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "American Indian or Alaska Native");

                var asian = dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Asian");

                var blackorAfricanAmerican =
                    dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Black or African American");

                var nativeHawaiianorOtherPacificIslander =
                    dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Native Hawaiian or Other Pacific Islander");

                var white = dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "White");

                var hispanic = dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Hispanic");

                var otherRace = dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Other Race");

                var otherPacificIslander =
                    dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Other Pacific Islander");

                var unreportedRefusedtoReport =
                    dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Unreported/Refused to Report");

                var declinedtoSpecify =
                    dtRace.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Declined to Specify");

                var objRaceMale = new StrucDocItem()
                {
                    Items = new List<object>()
                    {
                        new StrucDocContent
                        {
                            styleCode = "Bold",
                            Text = new List<string>()
                            {
                                dtRaceDistinct.Rows[i][0] + ": "
                            }
                        }
                    },
                    Text = new List<string>()
                    {
                        dtRaceDistinct.Rows[i][0].ToString() == "American Indian or Alaska Native"
                            ? americanIndianorAlaskaNative.ToString()
                            : dtRaceDistinct.Rows[i][0].ToString() == "Asian"
                                ? asian.ToString()
                                : dtRaceDistinct.Rows[i][0].ToString() == "Black or African American"
                                    ? blackorAfricanAmerican.ToString()
                                    : dtRaceDistinct.Rows[i][0].ToString() ==
                                      "Native Hawaiian or Other Pacific Islander"
                                        ? nativeHawaiianorOtherPacificIslander.ToString()
                                        : dtRaceDistinct.Rows[i][0].ToString() == "White"
                                            ? white.ToString()
                                            : dtRaceDistinct.Rows[i][0].ToString() == "Hispanic"
                                                ? hispanic.ToString()
                                                : dtRaceDistinct.Rows[i][0].ToString() == "Other Race"
                                                    ? otherRace.ToString()
                                                    : dtRaceDistinct.Rows[i][0].ToString() ==
                                                      "Other Pacific Islander"
                                                        ? otherPacificIslander.ToString()
                                                        : dtRaceDistinct.Rows[i][0].ToString() ==
                                                          "Unreported/Refused to Report"
                                                            ? unreportedRefusedtoReport.ToString()
                                                            : declinedtoSpecify.ToString()
                    }
                };
                objStrucDocList.item.Add(objRaceMale);
            }
        }
        private void MedicareHandle(StrucDocList objStrucDocList, EnumerableRowCollection<DataRow> drEnumerableRowCollectionMedicare, string columnName)
        {
            if (!drEnumerableRowCollectionMedicare.Any()) return;

            var dtMedicare = drEnumerableRowCollectionMedicare.CopyToDataTable();
            var dvMedicare = new DataView(dtMedicare);
            var dtMedicareDistinct = dvMedicare.ToTable(true, "Medicare");

            //for (var i = 0; i < dtMedicareDistinct.Rows.Count; i++)
            //{

            if (string.IsNullOrEmpty(dtMedicare.Rows[0][columnName].ToString())) return;

            var medicare = dtMedicare.AsEnumerable().Count(r => r.Field<string>("Medicare") == "Medicare");

            var objMedicare = new StrucDocItem()
            {
                Items = new List<object>()
                {
                    new StrucDocContent
                    {
                        styleCode = "Bold",
                        Text = new List<string>()
                        {
                            "Payer - Medicare: "
                        }
                    }
                },
                Text = new List<string>()
                {
                    dtMedicareDistinct.Rows[0][0].ToString() == "Medicare" ? medicare.ToString() : "0"
                }
            };
            objStrucDocList.item.Add(objMedicare);
            //}
        }
        private void MedicaidHandle(StrucDocList objStrucDocList, EnumerableRowCollection<DataRow> drEnumerableRowCollectionMedicaid, string columnName)
        {
            if (!drEnumerableRowCollectionMedicaid.Any()) return;

            var dtMedicaid = drEnumerableRowCollectionMedicaid.CopyToDataTable();
            var dvMedicaid = new DataView(dtMedicaid);

            var dtMedicaidDistinct = dvMedicaid.ToTable(true, "Medicaid");

            //for (var i = 0; i < dtMedicaidDistinct.Rows.Count; i++)
            //{
            //if (dtMedicaid.Rows[i][columnName].ToString() != "1") continue;
            if (string.IsNullOrEmpty(dtMedicaid.Rows[0][columnName].ToString())) return;

            var medicaid =
                dtMedicaid.AsEnumerable().Count(r => r.Field<string>("Medicaid") == "Medicaid");

            var objMedicaid = new StrucDocItem()
            {
                Items = new List<object>()
                    {
                        new StrucDocContent
                        {
                            styleCode = "Bold",
                            Text = new List<string>()
                            {
                                "Payer - Medicaid: "
                            }
                        }
                    },
                Text = new List<string>()
                    {
                        dtMedicaidDistinct.Rows[0][0].ToString() == "Medicaid" ? medicaid.ToString() : "0"
                    }
            };
            objStrucDocList.item.Add(objMedicaid);
            //}
        }

        #endregion

        #endregion

        #endregion

        #region MeasureSection Part=2 -- XML, Validation

        private Entry MeasureSection_EntrySection(DataRow drMeasureSection, EnumerableRowCollection<DataRow> drEnumerableRowCollectionCatagoryIiiValueSet)
        {
            var objEntry = new Entry();

            if (drMeasureSection.ItemArray.Any())
            {
                var objOrganizer = new Organizer()
                {
                    moodCode = "EVN",
                    classCode = x_ActClassDocumentEntryOrganizer.CLUSTER,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.98"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.1" , extension="2016-09-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.17" , extension="2016-11-01"
                        }

                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    statusCode = new CS() { code = "completed" },

                    #region Reference
                    reference = new List<Reference>()
                    {
                        new Reference()
                        {
                            typeCode = x_ActRelationshipExternalReference.REFR,
                            Item = new ExternalDocument()
                            {
                                classCode = "DOC",
                                moodCode = "EVN",
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.4.738",
                                        // Version Specific Identifier 
                                        extension =
                                            drMeasureSection[
                                                _dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName]
                                                .ToString()
                                    }
                                    //,
                                    //new II()
                                    //{
                                    //    root = "2.16.840.1.113883.3.560.1",
                                    //    // CQM ID
                                    //    extension =
                                    //        drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                                    //},
                                    //new II()
                                    //{
                                    //    root = "2.16.840.1.113883.3.560.101.2",
                                    //    // Tool identifier
                                    //    extension =
                                    //        drMeasureSection[_dsMeasureSection.CQM.eMeasureIdentifierColumn.ColumnName]
                                    //            .ToString()
                                    //}
                                },
                                text = new ED()
                                {
                                    Text = new List<string>()
                                    {
                                        drMeasureSection[_dsMeasureSection.CQM.MeasureColumn.ColumnName].ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "57024-2",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "Health Quality Measure Document"
                                }
                                //,
                              
                                //versionNumber = new INT()
                                //{
                                //    value = "3"
                                //}
                            }
                        }

                        //19-05-2016
                        //new Reference()
                        //{
                        //    typeCode = x_ActRelationshipExternalReference.REFR,
                        //    Item = new ExternalObservation()
                        //    {
                        //        id = new List<II>()
                        //        {
                        //            new II()
                        //            {
                        //                //This id SHALL equal the id of the corresponding measure set definition within the eMeasure (CONF:18356).
                        //                root = Guid.NewGuid().ToString()
                        //            }
                        //        },
                        //        //<!-- SHALL single value binding -->
                        //        code = new CD()
                        //        {
                        //            code = "55185-3",
                        //            codeSystem = "2.16.840.1.113883.6.1",
                        //            codeSystemName = "LOINC",
                        //            displayName = "measure set"
                        //        },
                        //        text = new ED()
                        //        {
                        //            Text = new List<string>()
                        //            {
                        //                drMeasureSection[_dsMeasureSection.CQM.MeasureColumn.ColumnName].ToString()
                        //            }
                        //        }
                        //    }
                        //}
                    },

                    #endregion

                    component = new List<Component4>()
                };



                var hqmfId = drMeasureSection[_dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName].ToString();

                objOrganizer.component.Add
                    (
                        GetPerformanceRate(ParseNInt(drMeasureSection[_dsMeasureSection.CQM.PerfromanceRate1Column.ColumnName].ToString()))
                    );

                objOrganizer.component.Add
                    (
                        GetReportingRate(ParseNInt(drMeasureSection[_dsMeasureSection.CQM.ReportingRate1Column.ColumnName].ToString()))
                    );

                var ippopulation = Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString());
                var denominator = Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString());
                var numerator = Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString());
                var numExclusion = Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString());
                var denExclusion = Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString());
                var denException = Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString());
                var cqmId = drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();


                #region InitialPopulation

                if (ippopulation > 0)
                {
                    objOrganizer.component.Add
                    (
                        GetInitialPopulationComponent(ippopulation, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                    );
                }
                else
                {
                    if (cqmId == "0043" || cqmId == "CMS50v3" || cqmId == "0041" || cqmId == "0418" || cqmId == "0419" || cqmId == "0421" || cqmId == "0043"
                        || cqmId == "0028" || cqmId == "0022" || cqmId == "0068" || cqmId == "0018" || cqmId == "0059" || cqmId == "0031" || cqmId == "0056"
                        || cqmId == "0034" || cqmId == "0062" || cqmId == "0101" || cqmId == "0065" || cqmId == "2872" || cqmId == "CMS22v5")
                    {
                        objOrganizer.component.Add
                      (
                          GetInitialPopulationComponent_Zero(ippopulation, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                      );
                    }
                }

                #endregion

                #region Denominator

                if (denominator > 0)
                {
                    objOrganizer.component.Add
                    (
                        GetDenominatorComponent(denominator, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                    );
                }
                else
                {
                    if (cqmId == "0043" || cqmId == "CMS50v3" || cqmId == "0041" || cqmId == "0418" || cqmId == "0419" || cqmId == "0421" || cqmId == "0043"
                        || cqmId == "0028" || cqmId == "0022" || cqmId == "0068" || cqmId == "0018" || cqmId == "0059" || cqmId == "0031" || cqmId == "0056"
                        || cqmId == "0034" || cqmId == "0062" || cqmId == "0101" || cqmId == "0065" || cqmId == "2872" || cqmId == "CMS22v5")
                    {
                        objOrganizer.component.Add
                         (
                             GetDenominatorComponent_Zero(denominator, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                         );
                    }
                }

                #endregion

                #region Numerator

                if (numerator > 0)
                {
                    if (cqmId == "0075" || cqmId == "0022")
                    {
                        if (cqmId == "0022")
                        {
                            objOrganizer.component.Add
                                (
                                    GetNumeratorComponent(numerator, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                                );

                            if (numExclusion == 0)
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent_Zero(numExclusion, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                            else
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent(numExclusion, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                        }
                        else
                        {
                            objOrganizer.component.Add
                                (
                                    GetNumeratorComponent(numerator, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                                );

                            if (numExclusion == 0)
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent_Zero(numExclusion, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                            else
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorTwoComponent(numExclusion, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                        }

                    }
                    else
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent(numerator, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }
                else
                {
                    if (cqmId == "0043" || cqmId == "CMS50v3" || cqmId == "0041" || cqmId == "0418" || cqmId == "0419" || cqmId == "0421" || cqmId == "0043"
                        || cqmId == "0028" || cqmId == "0022" || cqmId == "0068" || cqmId == "0018" || cqmId == "0059" || cqmId == "0031" || cqmId == "0056"
                        || cqmId == "0034" || cqmId == "0062" || cqmId == "0101" || cqmId == "0065" || cqmId == "2872" || cqmId == "CMS22v5")
                    {
                        objOrganizer.component.Add
                        (
                            GetNumeratorComponent_Zero(numerator, hqmfId, cqmId, drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                    }

                }

                #endregion

                #region DenominatorExclusion

                if (denExclusion > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExclusionComponent(denExclusion, hqmfId, cqmId,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmId == "0043" || cqmId == "CMS50v3" || cqmId == "0041" || cqmId == "0418" || cqmId == "0419" || cqmId == "0421" || cqmId == "0043"
                        || cqmId == "0028" || cqmId == "0022" || cqmId == "0068" || cqmId == "0018" || cqmId == "0059" || cqmId == "0031" || cqmId == "0056"
                        || cqmId == "0034" || cqmId == "0062" || cqmId == "0101" || cqmId == "0065" || cqmId == "2872" || cqmId == "CMS22v5")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExclusionComponent_Zero(denExclusion, hqmfId, cqmId,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorException

                if (denException > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExceptionComponent(denException, hqmfId, cqmId,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmId == "0043" || cqmId == "CMS50v3" || cqmId == "0041" || cqmId == "0418" || cqmId == "0419" || cqmId == "0421" || cqmId == "0043"
                        || cqmId == "0028" || cqmId == "0022" || cqmId == "0068" || cqmId == "0018" || cqmId == "0059" || cqmId == "0031" || cqmId == "0056"
                        || cqmId == "0034" || cqmId == "0062" || cqmId == "0101" || cqmId == "0065" || cqmId == "2872" || cqmId == "CMS22v5")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExceptionComponent_Zero(denException, hqmfId, cqmId,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region NumeratorExclusion

                if (numExclusion > 0)
                {
                    if (cqmId != "0075")
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorExclusionComponent(numExclusion, hqmfId, cqmId,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region rtest Commented

                //var objEntry1 = new Entry()
                //{
                //    Item = new Organizer()
                //    {
                //        moodCode = "EVN",
                //        classCode = x_ActClassDocumentEntryOrganizer.CLUSTER,
                //        templateId = new List<II>()
                //        {
                //            new II()
                //            {
                //                root = "2.16.840.1.113883.10.20.24.3.98"
                //            },
                //            new II()
                //            {
                //                root = "2.16.840.1.113883.10.20.27.3.1"
                //            }
                //        },
                //        id = new List<II>()
                //        {
                //            new II()
                //            {
                //                root = Guid.NewGuid().ToString()
                //            }
                //        },
                //        statusCode = new CS() { code = "completed" },

                //        #region Reference
                //        reference = new List<Reference>()
                //        {
                //            new Reference()
                //            {
                //                typeCode = x_ActRelationshipExternalReference.REFR,
                //                Item = new ExternalDocument()
                //                {
                //                    classCode = "DOC",
                //                    moodCode = "EVN",
                //                    id = new List<II>()
                //                    {
                //                        new II()
                //                        {
                //                            root = "2.16.840.1.113883.4.738",
                //                            // Version Specific Identifier 
                //                            extension = drMeasureSection[_dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName].ToString()
                //                        },
                //                        new II()
                //                        {
                //                            root = "2.16.840.1.113883.3.560.1",
                //                            // CQM ID
                //                            extension = drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                //                        },
                //                        new II()
                //                        {
                //                            root = "2.16.840.1.113883.3.560.101.2",
                //                            // Tool identifier
                //                            extension = drMeasureSection[_dsMeasureSection.CQM.eMeasureIdentifierColumn.ColumnName].ToString() 
                //                        }
                //                    },
                //                    text = new ED()
                //                    {
                //                        Text = new List<string>()
                //                        {
                //                            drMeasureSection[_dsMeasureSection.CQM.MeasureColumn.ColumnName].ToString()
                //                        }
                //                    },
                //                    code = new CD()
                //                    {
                //                        code = "57024-2",
                //                        codeSystem = "2.16.840.1.113883.6.1",
                //                        codeSystemName = "LOINC",
                //                        displayName = "Health Quality Measure Document"
                //                    }
                //                }
                //            },
                //            new Reference()
                //            {
                //                typeCode = x_ActRelationshipExternalReference.REFR,
                //                Item = new ExternalObservation()
                //                {
                //                    id = new List<II>()
                //                    {
                //                        new II()
                //                        {
                //                            //This id SHALL equal the id of the corresponding measure set definition within the eMeasure (CONF:18356).
                //                            root = Guid.NewGuid().ToString()
                //                        }
                //                    },
                //                    //<!-- SHALL single value binding -->
                //                    code = new CD()
                //                    {
                //                        code = "55185-3",
                //                        codeSystem = "2.16.840.1.113883.6.1",
                //                        codeSystemName = "LOINC",
                //                        displayName = "measure set"
                //                    },
                //                    text = new ED()
                //                    {
                //                        Text = new List<string>()
                //                        {
                //                            drMeasureSection[_dsMeasureSection.CQM.MeasureColumn.ColumnName].ToString()
                //                        }
                //                    }
                //                }
                //            }
                //        },

                //        #endregion

                //        #region Component

                //        component = new List<Component4>()
                //        {
                //            //5.4 Measure Data
                //            GetInitialPopulationComponent
                //            (
                //                Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString()), 
                //                "RefID", 
                //                drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                //            ),
                //            GetDenominatorComponent
                //            (
                //                Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString()),
                //                "RefID",
                //                drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                //            ),
                //            GetNumeratorComponent
                //            (
                //                Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString()),
                //                "RefID",
                //                drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                //            ),
                //            GetDenominatorExclusionComponent
                //            (
                //                Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString()),
                //                "RefID",
                //                drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                //            ),
                //            GetDenominatorExceptionComponent
                //            (
                //                Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString()),
                //                "RefID",
                //                drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                //            ),
                //            GetNumeratorExclusionComponent
                //            (
                //                Int64.Parse(drMeasureSection[_dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString()),
                //                "RefID",
                //                drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString()
                //            )
                //        }

                //        #endregion
                //    }
                //};

                #endregion

                objEntry.Item = objOrganizer;
            }

            return objEntry;
        }
        private Entry MeasureSection_EntrySection_421(DataRow drMeasureSection421A, DataRow drMeasureSection421B, EnumerableRowCollection<DataRow> drEnumerableRowCollectionCatagoryIiiValueSet)
        {
            var objEntry = new Entry();

            if (drMeasureSection421A.ItemArray.Any())
            {
                var objOrganizer = new Organizer()
                {
                    moodCode = "EVN",
                    classCode = x_ActClassDocumentEntryOrganizer.CLUSTER,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.98"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.1", extension="2016-09-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.17", extension="2016-11-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    statusCode = new CS() { code = "completed" },

                    #region Reference
                    reference = new List<Reference>()
                    {
                        new Reference()
                        {
                            typeCode = x_ActRelationshipExternalReference.REFR,
                            Item = new ExternalDocument()
                            {
                                classCode = "DOC",
                                moodCode = "EVN",
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.4.738",
                                        // Version Specific Identifier 
                                        extension =
                                            drMeasureSection421A[
                                                _dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName]
                                                .ToString()
                                    }
                                },
                                text = new ED()
                                {
                                    Text = new List<string>()
                                    {
                                        drMeasureSection421A[_dsMeasureSection.CQM.MeasureColumn.ColumnName].ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "57024-2",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "Health Quality Measure Document"
                                },
                                //setId = new II()
                                //{
                                //    root =  Guid.NewGuid().ToString()
                                //},
                                versionNumber = new INT()
                                {
                                    value = "3"
                                }
                            }
                        }
                    },

                    #endregion

                    component = new List<Component4>()
                };

                var hqmfId = drMeasureSection421A[_dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName].ToString();

                objOrganizer.component.Add
                    (
                        GetPerformanceRate(ParseNInt(drMeasureSection421A[_dsMeasureSection.CQM.PerfromanceRate1Column.ColumnName].ToString()))
                    );

                objOrganizer.component.Add
                    (
                        GetReportingRate(ParseNInt(drMeasureSection421A[_dsMeasureSection.CQM.ReportingRate1Column.ColumnName].ToString()))
                    );

                var ippopulationA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString());
                var denominatorA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString());
                var numeratorA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString());
                var numExclusionA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString());
                var denExclusionA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString());
                var denExceptionA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString());
                var cqmIdA = drMeasureSection421A[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();

                #region InitialPopulation

                if (ippopulationA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent(ippopulationA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdA == "0022(A)" || cqmIdA == "0075(A)")
                    {
                        objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent_Zero(ippopulationA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                    }
                }

                #endregion

                #region Denominator

                if (denominatorA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorComponent(denominatorA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdA == "0022(A)" || cqmIdA == "0075(A)")
                    {
                        objOrganizer.component.Add
                       (
                           GetDenominatorComponent_Zero(denominatorA, hqmfId, cqmIdA,
                               drEnumerableRowCollectionCatagoryIiiValueSet)
                       );
                    }
                }

                #endregion

                #region Numerator

                if (numeratorA > 0)
                {
                    if (cqmIdA == "0075" || cqmIdA == "0022")
                    {
                        if (cqmIdA == "0022")
                        {
                            objOrganizer.component.Add
                                (
                                    GetNumeratorComponent(numeratorA, hqmfId, cqmIdA,
                                        drEnumerableRowCollectionCatagoryIiiValueSet)
                                );

                            if (numExclusionA == 0)
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent_Zero(numExclusionA, hqmfId, cqmIdA,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                            else
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent(numExclusionA, hqmfId, cqmIdA,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                        }
                        else
                        {
                            objOrganizer.component.Add
                                (
                                    GetNumeratorComponent(numeratorA, hqmfId, cqmIdA,
                                        drEnumerableRowCollectionCatagoryIiiValueSet)
                                );

                            if (numExclusionA == 0)
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent_Zero(numExclusionA, hqmfId, cqmIdA,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                            else
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorTwoComponent(numExclusionA, hqmfId, cqmIdA,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                        }

                    }
                    else
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent(numeratorA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }
                else
                {
                    if (cqmIdA == "0022(A)" || cqmIdA == "0075(A)")
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent_Zero(numeratorA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorExclusion

                if (denExclusionA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExclusionComponent(denExclusionA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdA == "0022(A)" || cqmIdA == "0075(A)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExclusionComponent_Zero(denExclusionA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorException

                if (denExceptionA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExceptionComponent(denExceptionA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdA == "0022(A)"  || cqmIdA == "0075(A)" || cqmIdA == "0041" || cqmIdA == "0418")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExceptionComponent_Zero(denExceptionA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region NumeratorExclusion

                if (numExclusionA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetNumeratorExclusionComponent(numExclusionA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }

                #endregion

                var ippopulationB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString());
                var denominatorB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString());
                var numeratorB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString());
                var numExclusionB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString());
                var denExclusionB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString());
                var denExceptionB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString());
                var cqmIdB = drMeasureSection421B[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();

                #region InitialPopulation

                if (ippopulationB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent(ippopulationB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0022(B)" || cqmIdB == "0075(B)")
                    {
                        objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent_Zero(ippopulationB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                    }
                }

                #endregion

                #region Denominator

                if (denominatorB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorComponent(denominatorB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0022(B)" || cqmIdB == "0075(B)")
                    {
                        objOrganizer.component.Add
                          (
                              GetDenominatorComponent_Zero(denominatorB, hqmfId, cqmIdB,
                                  drEnumerableRowCollectionCatagoryIiiValueSet)
                          );
                    }
                }

                #endregion

                #region Numerator

                if (numeratorB > 0)
                {
                    if (cqmIdB == "0075" || cqmIdB == "0022")
                    {
                        if (cqmIdB == "0022")
                        {
                            objOrganizer.component.Add
                                (
                                    GetNumeratorComponent(numeratorB, hqmfId, cqmIdB,
                                        drEnumerableRowCollectionCatagoryIiiValueSet)
                                );

                            if (numExclusionB == 0)
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent_Zero(numExclusionB, hqmfId, cqmIdB,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                            else
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent(numExclusionB, hqmfId, cqmIdB,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                        }
                        else
                        {
                            objOrganizer.component.Add
                                (
                                    GetNumeratorComponent(numeratorB, hqmfId, cqmIdB,
                                        drEnumerableRowCollectionCatagoryIiiValueSet)
                                );

                            if (numExclusionB == 0)
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorComponent_Zero(numExclusionB, hqmfId, cqmIdB,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                            else
                            {
                                objOrganizer.component.Add
                                    (
                                        GetNumeratorTwoComponent(numExclusionB, hqmfId, cqmIdB,
                                            drEnumerableRowCollectionCatagoryIiiValueSet)
                                    );
                            }
                        }

                    }
                    else
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent(numeratorB, hqmfId, cqmIdB,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }
                else
                {
                    if (cqmIdB == "0022(B)" || cqmIdB == "0075(B)")
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent_Zero(numeratorB, hqmfId, cqmIdB,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorExclusion

                if (denExclusionB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExclusionComponent(denExclusionB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0421b" || cqmIdB == "0022(B)" || cqmIdB == "0075(B)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExclusionComponent_Zero(denExclusionB, hqmfId, cqmIdB,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorException

                if (denExceptionB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExceptionComponent(denExceptionB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0041" || cqmIdB == "0418" || cqmIdB == "0022(B)" || cqmIdB == "0075(B)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExceptionComponent_Zero(denExceptionB, hqmfId, cqmIdB,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region NumeratorExclusion

                if (numExclusionB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetNumeratorExclusionComponent(numExclusionB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }

                #endregion

                objEntry.Item = objOrganizer;
            }

            return objEntry;
        }

        private Entry MeasureSection_EntrySection_160(DataRow drMeasureSection421A, DataRow drMeasureSection421B, DataRow drMeasureSection421C, EnumerableRowCollection<DataRow> drEnumerableRowCollectionCatagoryIiiValueSet)
        {
            var objEntry = new Entry();

            if (drMeasureSection421A.ItemArray.Any())
            {
                var objOrganizer = new Organizer()
                {
                    moodCode = "EVN",
                    classCode = x_ActClassDocumentEntryOrganizer.CLUSTER,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.98"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.1", extension="2016-09-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.17", extension="2016-11-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    statusCode = new CS() { code = "completed" },

                    #region Reference
                    reference = new List<Reference>()
                    {
                        new Reference()
                        {
                            typeCode = x_ActRelationshipExternalReference.REFR,
                            Item = new ExternalDocument()
                            {
                                classCode = "DOC",
                                moodCode = "EVN",
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.4.738",
                                        // Version Specific Identifier 
                                        extension =
                                            drMeasureSection421A[
                                                _dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName]
                                                .ToString()
                                    }
                                },
                                text = new ED()
                                {
                                    Text = new List<string>()
                                    {
                                        drMeasureSection421A[_dsMeasureSection.CQM.MeasureColumn.ColumnName].ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "57024-2",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "Health Quality Measure Document"
                                },
                                //setId = new II()
                                //{
                                //    root =  Guid.NewGuid().ToString()
                                //},
                                versionNumber = new INT()
                                {
                                    value = "3"
                                }
                            }
                        }
                    },

                    #endregion

                    component = new List<Component4>()
                };

                var hqmfId = drMeasureSection421A[_dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName].ToString();

                objOrganizer.component.Add
                    (
                        GetPerformanceRate(ParseNInt(drMeasureSection421A[_dsMeasureSection.CQM.PerfromanceRate1Column.ColumnName].ToString()))
                    );

                objOrganizer.component.Add
                    (
                        GetReportingRate(ParseNInt(drMeasureSection421A[_dsMeasureSection.CQM.ReportingRate1Column.ColumnName].ToString()))
                    );

                var ippopulationA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString());
                var denominatorA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString());
                var numeratorA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString());
                var numExclusionA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString());
                var denExclusionA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString());
                var denExceptionA = Int64.Parse(drMeasureSection421A[_dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString());
                var cqmIdA = drMeasureSection421A[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();

                #region InitialPopulation

                if (ippopulationA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent(ippopulationA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {

                    if (cqmIdA == "0712(A)")
                    {
                        objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent_Zero(ippopulationA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                    }
                }

                #endregion

                #region Denominator

                if (denominatorA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorComponent(denominatorA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdA == "0712(A)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorComponent_Zero(denominatorA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region Numerator

                if (numeratorA > 0)
                {

                    objOrganizer.component.Add
                        (
                            GetNumeratorComponent(numeratorA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );

                }
                else
                {
                    if (cqmIdA == "0712(A)")
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent_Zero(numeratorA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorExclusion

                if (denExclusionA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExclusionComponent(denExclusionA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdA == "0712(A)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExclusionComponent_Zero(denExclusionA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorException

                if (denExceptionA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExceptionComponent(denExceptionA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdA == "0712(A)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExceptionComponent_Zero(denExceptionA, hqmfId, cqmIdA,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region NumeratorExclusion

                if (numExclusionA > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetNumeratorExclusionComponent(numExclusionA, hqmfId, cqmIdA,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }

                #endregion

                var ippopulationB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString());
                var denominatorB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString());
                var numeratorB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString());
                var numExclusionB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString());
                var denExclusionB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString());
                var denExceptionB = Int64.Parse(drMeasureSection421B[_dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString());
                var cqmIdB = drMeasureSection421B[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();

                #region InitialPopulation

                if (ippopulationB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent(ippopulationB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0712(B)")
                    {
                        objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent_Zero(ippopulationB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                    }
                }

                #endregion

                #region Denominator

                if (denominatorB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorComponent(denominatorB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0712(B)")
                    {
                        objOrganizer.component.Add
                        (
                            GetDenominatorComponent_Zero(denominatorB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                    }
                }

                #endregion

                #region Numerator

                if (numeratorB > 0)
                {

                    objOrganizer.component.Add
                        (
                            GetNumeratorComponent(numeratorB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );

                }
                else
                {
                    if (cqmIdB == "0712(B)")
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent_Zero(numeratorB, hqmfId, cqmIdB,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorExclusion

                if (denExclusionB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExclusionComponent(denExclusionB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0712(B)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExclusionComponent_Zero(denExclusionB, hqmfId, cqmIdB,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorException

                if (denExceptionB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExceptionComponent(denExceptionB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdB == "0712(B)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExceptionComponent_Zero(denExceptionB, hqmfId, cqmIdB,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region NumeratorExclusion

                if (numExclusionB > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetNumeratorExclusionComponent(numExclusionB, hqmfId, cqmIdB,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }

                #endregion


                var ippopulationC = Int64.Parse(drMeasureSection421C[_dsMeasureSection.CQM.InitialPopulationColumn.ColumnName].ToString());
                var denominatorC = Int64.Parse(drMeasureSection421C[_dsMeasureSection.CQM.DenominatorColumn.ColumnName].ToString());
                var numeratorC = Int64.Parse(drMeasureSection421C[_dsMeasureSection.CQM.NumeratorColumn.ColumnName].ToString());
                var numExclusionC = Int64.Parse(drMeasureSection421C[_dsMeasureSection.CQM.NumeratorExclusionColumn.ColumnName].ToString());
                var denExclusionC = Int64.Parse(drMeasureSection421C[_dsMeasureSection.CQM.DenominatorExclusionColumn.ColumnName].ToString());
                var denExceptionC = Int64.Parse(drMeasureSection421C[_dsMeasureSection.CQM.DenominatorExceptionColumn.ColumnName].ToString());
                var cqmIdC = drMeasureSection421C[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();

                #region InitialPopulation

                if (ippopulationC > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetInitialPopulationComponent(ippopulationC, hqmfId, cqmIdC,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdC == "0712(C)")
                    {
                        objOrganizer.component.Add
                       (
                           GetInitialPopulationComponent_Zero(ippopulationC, hqmfId, cqmIdC,
                               drEnumerableRowCollectionCatagoryIiiValueSet)
                       );
                    }
                }

                #endregion

                #region Denominator

                if (denominatorC > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorComponent(denominatorC, hqmfId, cqmIdC,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdC == "0712(C)")
                    {
                        objOrganizer.component.Add
                        (
                            GetDenominatorComponent_Zero(denominatorC, hqmfId, cqmIdC,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                    }

                }

                #endregion

                #region Numerator

                if (numeratorC > 0)
                {

                    objOrganizer.component.Add
                        (
                            GetNumeratorComponent(numeratorC, hqmfId, cqmIdC,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );

                }
                else
                {
                    if (cqmIdC == "0712(C)")
                    {
                        objOrganizer.component.Add
                            (
                                GetNumeratorComponent_Zero(numeratorC, hqmfId, cqmIdC,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorExclusion

                if (denExclusionC > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExclusionComponent(denExclusionC, hqmfId, cqmIdC,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdC == "0712(C)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExclusionComponent_Zero(denExclusionC, hqmfId, cqmIdC,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region DenominatorException

                if (denExceptionC > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetDenominatorExceptionComponent(denExceptionC, hqmfId, cqmIdC,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }
                else
                {
                    if (cqmIdC == "0712(C)")
                    {
                        objOrganizer.component.Add
                            (
                                GetDenominatorExceptionComponent_Zero(denExceptionC, hqmfId, cqmIdC,
                                    drEnumerableRowCollectionCatagoryIiiValueSet)
                            );
                    }
                }

                #endregion

                #region NumeratorExclusion

                if (numExclusionC > 0)
                {
                    objOrganizer.component.Add
                        (
                            GetNumeratorExclusionComponent(numExclusionC, hqmfId, cqmIdC,
                                drEnumerableRowCollectionCatagoryIiiValueSet)
                        );
                }

                #endregion



                objEntry.Item = objOrganizer;
            }

            return objEntry;
        }

        int? ParseNInt(string val)
        {
            int i;
            return int.TryParse(val, out i) ? (int?)i : null;
        }

        #region Measure Section XML Nodes, [Complete]

        #region List<KeyValue Pair> Dynamic Allocation to Nodes
        private List<KeyValuePair<string, string>> GenderPair(EnumerableRowCollection<DataRow> drEnumerableRowCollectionGender, string columnName, string cqmId)
        {
            if (drEnumerableRowCollectionGender == null)
                throw new ArgumentNullException("drEnumerableRowCollectionGender");

            var kvpList = new List<KeyValuePair<string, string>>();

            var dtGender = drEnumerableRowCollectionGender.CopyToDataTable();
            string[] TobeDistinct = { "PatientID", columnName, "Gender", "CQMID" };
            DataTable dtDistinct = GetDistinctRecords(dtGender, TobeDistinct);

            var dtGenderMale = dtDistinct.AsEnumerable().Count(r => r.Field<string>("Gender") == "Male" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var dtGenderFemale = dtDistinct.AsEnumerable().Count(r => r.Field<string>("Gender") == "Female" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var dtGenderOther = dtDistinct.AsEnumerable().Count(r => r.Field<string>("Gender") == "Other" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);

            kvpList.Insert(0, new KeyValuePair<string, string>("M", dtGenderMale.ToString()));
            kvpList.Insert(1, new KeyValuePair<string, string>("F", dtGenderFemale.ToString()));
            kvpList.Insert(2, new KeyValuePair<string, string>("O", dtGenderOther.ToString()));

            return kvpList;
        }

        private static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        private List<KeyValuePair<string, string>> EthnicityPair(EnumerableRowCollection<DataRow> drEnumerableRowCollectionEthnicity, string columnName, string cqmId)
        {
            if (drEnumerableRowCollectionEthnicity == null)
                throw new ArgumentNullException("drEnumerableRowCollectionEthnicity");

            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();

            var dtEthnicity = drEnumerableRowCollectionEthnicity.CopyToDataTable();
            string[] TobeDistinct = { "PatientID", columnName, "EthnicityDescription", "CQMID" };
            DataTable dtDistinct = GetDistinctRecords(dtEthnicity, TobeDistinct);

            var drHispanicorLatino = dtDistinct.AsEnumerable().Count(r => r.Field<string>("EthnicityDescription") == "Hispanic or Latino" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var drNotHispanicorLatino = dtDistinct.AsEnumerable().Count(r => r.Field<string>("EthnicityDescription") == "Not Hispanic or Latino" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var drRefusedtoReport = dtDistinct.AsEnumerable().Count(r => r.Field<string>("EthnicityDescription") == "Refused to Report" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var drUnknown = dtDistinct.AsEnumerable().Count(r => r.Field<string>("EthnicityDescription") == "Unknown" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);

            kvpList.Insert(0, new KeyValuePair<string, string>("Hispanic or Latino+2135-2", drHispanicorLatino.ToString()));
            kvpList.Insert(1, new KeyValuePair<string, string>("Not Hispanic or Latino+2186-5", drNotHispanicorLatino.ToString()));
            kvpList.Insert(2, new KeyValuePair<string, string>("Refused to Report+2145-2", drRefusedtoReport.ToString()));
            kvpList.Insert(3, new KeyValuePair<string, string>("Unknown+UNK", drUnknown.ToString()));

            return kvpList;
        }
        private List<KeyValuePair<string, string>> RacePair(EnumerableRowCollection<DataRow> drEnumerableRowCollectionRace, string columnName, string cqmId)
        {
            if (drEnumerableRowCollectionRace == null)
                throw new ArgumentNullException("drEnumerableRowCollectionRace");

            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();

            var dtRace = drEnumerableRowCollectionRace.CopyToDataTable();
            string[] TobeDistinct = { "PatientID", columnName, "RaceDescription", "CQMID" };
            DataTable dtDistinct = GetDistinctRecords(dtRace, TobeDistinct);

            var americanIndianorAlaskaNative = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "American Indian or Alaska Native" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var asian = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Asian" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var blackorAfricanAmerican = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Black or African American" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var nativeHawaiianorOtherPacificIslander = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Native Hawaiian or Other Pacific Islander" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var white = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "White" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var hispanic = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Hispanic" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var otherRace = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Other Race" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var otherPacificIslander = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Other Pacific Islander" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var unreportedRefusedtoReport = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Unreported/Refused to Report" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            var declinedtoSpecify = dtDistinct.AsEnumerable().Count(r => r.Field<string>("RaceDescription") == "Declined to Specify" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);

            kvpList.Insert(0, new KeyValuePair<string, string>("American Indian or Alaska Native+1002-5", americanIndianorAlaskaNative.ToString()));
            kvpList.Insert(1, new KeyValuePair<string, string>("Asian+2028-9", asian.ToString()));
            kvpList.Insert(2, new KeyValuePair<string, string>("Black or African American+2054-5", blackorAfricanAmerican.ToString()));
            kvpList.Insert(3, new KeyValuePair<string, string>("Native Hawaiian or Other Pacific Islander+2076-8", nativeHawaiianorOtherPacificIslander.ToString()));
            kvpList.Insert(4, new KeyValuePair<string, string>("White+2106-3", white.ToString()));
            kvpList.Insert(5, new KeyValuePair<string, string>("Hispanic+2135-2", hispanic.ToString()));
            kvpList.Insert(6, new KeyValuePair<string, string>("Other Race+2131-1", otherRace.ToString()));
            kvpList.Insert(7, new KeyValuePair<string, string>("Other Pacific Islander+2131-3", otherPacificIslander.ToString()));
            kvpList.Insert(8, new KeyValuePair<string, string>("Unreported/Refused to Report+2131-4", unreportedRefusedtoReport.ToString()));
            kvpList.Insert(9, new KeyValuePair<string, string>("Declined to Specify+2131-1", declinedtoSpecify.ToString()));
            //kvpList.Insert(9, new KeyValuePair<string, string>("Declined to Specify+2131-4", declinedtoSpecify.ToString()));

            return kvpList;
        }
        private List<KeyValuePair<string, string>> MedicarePair(EnumerableRowCollection<DataRow> drEnumerableRowCollectionMedicare, string columnName, string cqmId)
        {
            if (drEnumerableRowCollectionMedicare == null)
                throw new ArgumentNullException("drEnumerableRowCollectionMedicare");

            var kvpList = new List<KeyValuePair<string, string>>();
            var dtMedicare = drEnumerableRowCollectionMedicare.CopyToDataTable();
            string[] TobeDistinct = { "PatientID", columnName, "Medicare", "CQMID" };
            DataTable dtDistinct = GetDistinctRecords(dtMedicare, TobeDistinct);

            var medicare = dtDistinct.AsEnumerable().Count(r => r.Field<string>("Medicare") == "Medicare" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            kvpList.Insert(0, new KeyValuePair<string, string>("1", medicare.ToString()));

            return kvpList;
        }
        private List<KeyValuePair<string, string>> MedicaidPair(EnumerableRowCollection<DataRow> drEnumerableRowCollectionMedicaid, string columnName, string cqmId)
        {
            if (drEnumerableRowCollectionMedicaid == null)
                throw new ArgumentNullException("drEnumerableRowCollectionMedicaid");

            var kvpList = new List<KeyValuePair<string, string>>();
            var dtMedicaid = drEnumerableRowCollectionMedicaid.CopyToDataTable();
            string[] TobeDistinct = { "PatientID", columnName, "Medicaid", "CQMID" };
            DataTable dtDistinct = GetDistinctRecords(dtMedicaid, TobeDistinct);

            var medicaid = dtDistinct.AsEnumerable().Count(r => r.Field<string>("Medicaid") == "Medicaid" && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            kvpList.Insert(0, new KeyValuePair<string, string>("2", medicaid.ToString()));

            return kvpList;
        }
        private List<KeyValuePair<string, string>> otherPair(EnumerableRowCollection<DataRow> drEnumerableRowCollectionMedicaid, string columnName, string cqmId)
        {
            if (drEnumerableRowCollectionMedicaid == null)
                throw new ArgumentNullException("drEnumerableRowCollectionMedicaid");

            var kvpList = new List<KeyValuePair<string, string>>();
            var dtMedicaid = drEnumerableRowCollectionMedicaid.CopyToDataTable();
            string[] TobeDistinct = { "PatientID", columnName, "Medicaid", "Medicare", "CQMID" };
            DataTable dtDistinct = GetDistinctRecords(dtMedicaid, TobeDistinct);

            var medicaid = dtDistinct.AsEnumerable().Count(r => r.Field<string>("Medicaid") == null && r.Field<string>("Medicare") == null && r.Field<string>(columnName) == "1" && r.Field<string>("CQMID") == cqmId);
            kvpList.Insert(0, new KeyValuePair<string, string>("349", medicaid.ToString()));

            return kvpList;
        }

        #endregion
        private Component4 GetPerformanceRate(Int64? performanceRate)
        {
            if (performanceRate == null)
                performanceRate = 0;

            var objPerformanceRateComponent = new Component4();
            var objObservationParent = new Observation()
            {
                classCode = "OBS",
                moodCode = x_ActMoodDocumentObservation.EVN,
                templateId = new List<II>()
                {
                    new II()
                    {
                        root = "2.16.840.1.113883.10.20.27.3.14"
                    }
                },
                code = new CD()
                {
                    code = "72510-1",
                    codeSystem = "2.16.840.1.113883.6.1",
                    codeSystemName = "LOINC",
                    displayName = "Performance Rate"
                },
                statusCode = new CS()
                {
                    code = "completed"
                },
                value = new List<ANY>()
                {
                    new REAL()
                    {
                        value = performanceRate.ToString()
                    }
                },
                reference = new List<Reference>()
            };

            var objRef = new Reference()
            {
                typeCode = x_ActRelationshipExternalReference.REFR,
                Item = new ExternalObservation()
                {
                    moodCode = "EVN",
                    classCode = "OBS",
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    },
                    code = new CD()
                    {
                        code = "NUMER",
                        codeSystem = "2.16.840.1.113883.5.1063",
                        codeSystemName = "ObservationValue",
                        displayName = "Numerator"
                    }
                }
            };

            objObservationParent.reference.Add(objRef);
            objPerformanceRateComponent.Item = objObservationParent;
            return objPerformanceRateComponent;
        }
        private Component4 GetReportingRate(Int64? reportingRate)
        {
            if (reportingRate == null)
                reportingRate = 0;

            var objReportingRateComponent = new Component4();
            var objObservationParent = new Observation()
            {
                classCode = "OBS",
                moodCode = x_ActMoodDocumentObservation.EVN,
                templateId = new List<II>()
                {
                    new II()
                    {
                        root = "2.16.840.1.113883.10.20.27.3.15"
                    }
                },
                code = new CD()
                {
                    code = "72509-3",
                    codeSystem = "2.16.840.1.113883.6.1",
                    codeSystemName = "LOINC",
                    displayName = "Performance Rate"
                },
                statusCode = new CS()
                {
                    code = "completed"
                },
                value = new List<ANY>()
                {
                    new REAL()
                    {
                        value = reportingRate.ToString()
                    }
                }
            };
            objReportingRateComponent.Item = objObservationParent;
            return objReportingRateComponent;
        }
        private Component4 GetInitialPopulationComponent(Int64 initialPopulationCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId);

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "IPP");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }


            if (cqmId == "0421b")
            {
                vaueSet = "6E701B1C-6CA5-4AD5-98C9-5F766745EA89";
            }
            else if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "E1A2F7D4-0B24-47FC-9369-0700771B2D40" : vaueSet;
            }
            else if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "E1A2F7D4-0B24-47FC-9369-0700771B2D40" : vaueSet;
            }
            else if (cqmId == "0075(A)")
            {
                vaueSet = vaueSet == "" ? "5D01C844-C73F-4E5E-A43D-1DBF79CEEB87" : vaueSet;
            }
            else if (cqmId == "0075(B)")
            {
                vaueSet = vaueSet == "" ? "F3FC934B-7AA5-4749-9C33-5147BBB4FBFA" : vaueSet;
            }
            else if (cqmId == "CMS50v3")
            {
                vaueSet = vaueSet == "" ? "E7A4AE77-FC46-44F1-BF25-9E99A89CED9B" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "55D921A2-0700-43F0-8C6F-1DB4ADDD7FAB" : vaueSet;
            }
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "FF48C33E-49B3-45D1-AB44-D283F39AE73D" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "D412322D-11F1-4573-893E-E6A05855DE10" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "4D10500F-6738-4541-BF24-4C95C74B45AB" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "CA679308-8AF3-4374-91DC-907951440D72" : vaueSet;
            }
            else if (cqmId == "0022(A)")
            {
                vaueSet = vaueSet == "" ? "C7BCE5A3-AC0D-440E-AA29-C98239F37A8B" : vaueSet;
            }
            else if (cqmId == "0022(B)")
            {
                vaueSet = vaueSet == "" ? "BC02D7CE-7133-46C6-8592-658668B09948" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "C3E458D8-8ED4-4F09-A661-6221A2B9355D" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "3AD33404-E734-4F67-9144-E4B63CB3F4BE" : vaueSet;
            }
            else if (cqmId == "0059")
            {
                vaueSet = vaueSet == "" ? "0739FE2E-B8DE-4A56-B064-877CC8E0977D" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "66FD640C-70BB-400F-9926-98EA1ACEBEBA" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "92656CE7-C9B1-44A8-8778-A8EF1ED90A18" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "757F3066-31E7-45D1-BA50-3EFB27ABB8E5" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "5631A7DF-CA44-4AD4-A691-DC0CED303F6A" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "9AB84D25-5883-4D85-A868-0D702403F250" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "4D1C7452-B908-4616-B9E9-36C9AA71005B" : vaueSet;
            }
            else if (cqmId == "0062")
            {
                vaueSet = vaueSet == "" ? "A27FD818-3C97-4516-A796-319B89E306AC" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "0175F89E-9CD7-4CD8-8D7F-38CB825D2BDD" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "1D86EE69-7DA7-4A73-8611-2E71176568B6" : vaueSet;
            }
            else if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "78CD5ACE-054C-43DF-A743-5F1E5BA4C099" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "C7E7C7AF-464F-43A0-8C84-CDFF99953C97" : vaueSet;
            }


            var objInitialPopulationComponent = new Component4();
            var objObservationParent = new Observation();

            if (dtExists.Any())
            {
                var keyValuePairGender = GenderPair(dtExists, "InitialPopulation", cqmId);
                var keyValuePairEthnicity = EthnicityPair(dtExists, "InitialPopulation", cqmId);
                var keyValuePairRace = RacePair(dtExists, "InitialPopulation", cqmId);
                var keyValuePairMedicare = MedicarePair(dtExists, "InitialPopulation", cqmId);
                var keyValuePairMedicaid = MedicaidPair(dtExists, "InitialPopulation", cqmId);
                var keyValuePairOther = otherPair(dtExists, "InitialPopulation", cqmId);



                #region Observation

                objObservationParent = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5", extension="2016-09-01"
                        },
                         new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.16", extension="2016-11-01"
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        codeSystemName = "ActCode",
                        displayName = "Assertion"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "IPOP",
                            codeSystem = "2.16.840.1.113883.5.4",
                            codeSystemName = "ActCode",
                            displayName = "initial patient population"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>(),
                    reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
                };

                #endregion

                objObservationParent.entryRelationship.Add(InitialPopulation(initialPopulationCount));

                foreach (var gender in keyValuePairGender.Where(gender => gender.Value != "0"))
                {
                    objObservationParent.entryRelationship.Add(SexSupplementalDataElement(gender.Key,
                        Int64.Parse(gender.Value), hqmfId));
                }

                foreach (var ethnicity in keyValuePairEthnicity.Where(t => t.Value != "0"))
                {
                    var ethnicityDesc = ethnicity.Key.Split('+').First();
                    var ethnicityCode = ethnicity.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement(ethnicityDesc,
                        Int64.Parse(ethnicity.Value), ethnicityCode, hqmfId));
                }

                foreach (var race in keyValuePairRace.Where(t => t.Value != "0"))
                {
                    var raceDesc = race.Key.Split('+').First();
                    var raceCode = race.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(RaceSupplementalDataElement(raceDesc,
                        Int64.Parse(race.Value), raceCode, hqmfId));
                }

                foreach (var payer in keyValuePairMedicare.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairMedicaid.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairOther.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

            }

            objInitialPopulationComponent.Item = objObservationParent;

            return objInitialPopulationComponent;
        }

        private Component4 GetInitialPopulationComponent_Zero(Int64 initialPopulationCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "IPP");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }


            if (cqmId == "0421b")
            {
                vaueSet = "6E701B1C-6CA5-4AD5-98C9-5F766745EA89";
            }
            else if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "E1A2F7D4-0B24-47FC-9369-0700771B2D40" : vaueSet;
            }
            else if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "78CD5ACE-054C-43DF-A743-5F1E5BA4C099" : vaueSet;
            }
            else if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "E1A2F7D4-0B24-47FC-9369-0700771B2D40" : vaueSet;
            }
            else if (cqmId == "0075(A)")
            {
                vaueSet = vaueSet == "" ? "5D01C844-C73F-4E5E-A43D-1DBF79CEEB87" : vaueSet;
            }
            else if (cqmId == "0075(B)")
            {
                vaueSet = vaueSet == "" ? "F3FC934B-7AA5-4749-9C33-5147BBB4FBFA" : vaueSet;
            }
            else if (cqmId == "CMS50v3")
            {
                vaueSet = vaueSet == "" ? "E7A4AE77-FC46-44F1-BF25-9E99A89CED9B" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "55D921A2-0700-43F0-8C6F-1DB4ADDD7FAB" : vaueSet;
            }
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "FF48C33E-49B3-45D1-AB44-D283F39AE73D" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "D412322D-11F1-4573-893E-E6A05855DE10" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "4D10500F-6738-4541-BF24-4C95C74B45AB" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "CA679308-8AF3-4374-91DC-907951440D72" : vaueSet;
            }
            else if (cqmId == "0022(A)")
            {
                vaueSet = vaueSet == "" ? "C7BCE5A3-AC0D-440E-AA29-C98239F37A8B" : vaueSet;
            }
            else if (cqmId == "0022(B)")
            {
                vaueSet = vaueSet == "" ? "BC02D7CE-7133-46C6-8592-658668B09948" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "C3E458D8-8ED4-4F09-A661-6221A2B9355D" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "3AD33404-E734-4F67-9144-E4B63CB3F4BE" : vaueSet;
            }
            else if (cqmId == "0059")
            {
                vaueSet = vaueSet == "" ? "0739FE2E-B8DE-4A56-B064-877CC8E0977D" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "66FD640C-70BB-400F-9926-98EA1ACEBEBA" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "92656CE7-C9B1-44A8-8778-A8EF1ED90A18" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "757F3066-31E7-45D1-BA50-3EFB27ABB8E5" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "5631A7DF-CA44-4AD4-A691-DC0CED303F6A" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "9AB84D25-5883-4D85-A868-0D702403F250" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "4D1C7452-B908-4616-B9E9-36C9AA71005B" : vaueSet;
            }
            else if (cqmId == "0062")
            {
                vaueSet = vaueSet == "" ? "A27FD818-3C97-4516-A796-319B89E306AC" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "0175F89E-9CD7-4CD8-8D7F-38CB825D2BDD" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "1D86EE69-7DA7-4A73-8611-2E71176568B6" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "C7E7C7AF-464F-43A0-8C84-CDFF99953C97" : vaueSet;
            }


            var objInitialPopulationComponent = new Component4();
            var objObservationParent = new Observation();


            #region Observation

            objObservationParent = new Observation()
            {
                classCode = "OBS",
                moodCode = x_ActMoodDocumentObservation.EVN,
                templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5", extension="2016-09-01"
                        },
                         new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.16", extension="2016-11-01"
                        }
                    },
                code = new CD()
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4",
                    codeSystemName = "ActCode",
                    displayName = "Assertion"
                },
                statusCode = new CS()
                {
                    code = "completed"
                },
                value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "IPOP",
                            codeSystem = "2.16.840.1.113883.5.4",
                            codeSystemName = "ActCode",
                            displayName = "initial patient population"
                        }
                    },
                entryRelationship = new List<EntryRelationship>(),
                reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
            };

            #endregion

            objObservationParent.entryRelationship.Add(AggregateCount(initialPopulationCount));
            objObservationParent.entryRelationship.Add(SexSupplementalDataElement("UNK", Int64.Parse("0"), hqmfId, false));
            objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement("Not Hispanic or Latino", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(RaceSupplementalDataElement("American Indian or Alaska Native", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(PayerSupplementalDataElement("349", Int64.Parse("0"), "349", hqmfId));

            objInitialPopulationComponent.Item = objObservationParent;
            return objInitialPopulationComponent;
        }
        private Component4 GetDenominatorComponent(Int64 denominatorCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("Denominator") == "1");

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "DENOM");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }

            if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "B57EA797-15A1-4C60-B34C-BAE292FE3B76" : vaueSet;
            }
            else if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "CE973C7A-B867-422B-8408-83538E236039" : vaueSet;
            }
            else if (cqmId == "0075(A)")
            {
                vaueSet = vaueSet == "" ? "3AA8D67A-CA66-4161-9B7A-FFB731EFF022" : vaueSet;
            }
            else if (cqmId == "0075(B)")
            {
                vaueSet = vaueSet == "" ? "6CB61453-F874-48BD-BB0A-6EB67DC7FCBC" : vaueSet;
            }
            else if (cqmId == "CMS50v3")
            {
                vaueSet = vaueSet == "" ? "B710DA92-D4B9-4217-8D1E-94B35163BECE" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "CB0D06D7-9C40-45B0-AEC2-60C39262ABA0" : vaueSet;
            }
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "56E05D20-4529-4EB7-8E30-17C95BBC5223" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "375D0559-C749-4BB9-9267-81EDF447650B" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "36CCB819-C150-466D-A55A-0A175B19E751" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "FFC4DDE0-C91C-4235-BF69-E3EF79457FBB" : vaueSet;
            }
            else if (cqmId == "0022(A)")
            {
                vaueSet = vaueSet == "" ? "07F04D61-0383-487E-942C-690BBBC6437D" : vaueSet;
            }
            else if (cqmId == "0022(B)")
            {
                vaueSet = vaueSet == "" ? "00401314-1B01-4896-A9FC-E991CDF29B6B" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "FE6E1AA0-EE26-4F9E-A2FD-8E36058DCB47" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "E62FEBA3-0F98-460D-93CD-44314D7203A8" : vaueSet;
            }
            else if (cqmId == "0059")
            {
                vaueSet = vaueSet == "" ? "D346DA74-F16E-4159-BEDF-331BA28837FB" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "40FB2D5F-7DEF-4D41-80C4-F3FBA0ED5851" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "C7DFE664-71AE-4EAD-AB65-CDFCF825A44E" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "32635FEA-918B-438F-8421-8A6A14E238E8" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "9665A8D2-F896-47A9-AA7E-271E9815D3CE" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "61B09878-32B3-4ECB-8E78-D1093E1FDDF9" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "589C2FD6-6AA9-4AF8-9E1C-973170361917" : vaueSet;
            }
            else if (cqmId == "0062")
            {
                vaueSet = vaueSet == "" ? "0CC16DE4-474A-4F6E-BFB8-0104709B1E05" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "A3F77B7C-2398-470D-A359-E2A31539DF91" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "2DA1439F-7941-411C-8885-91306F0CD9DD" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "65AFB8BA-F5CC-4ABA-A3FB-2F95FEE2B6FA" : vaueSet;
            }

            var objGetDenominatorComponent = new Component4();
            var objObservationParent = new Observation();

            if (dtExists.Any())
            {
                var keyValuePairGender = GenderPair(dtExists, "Denominator", cqmId);
                var keyValuePairEthnicity = EthnicityPair(dtExists, "Denominator", cqmId);
                var keyValuePairRace = RacePair(dtExists, "Denominator", cqmId);
                var keyValuePairMedicare = MedicarePair(dtExists, "Denominator", cqmId);
                var keyValuePairMedicaid = MedicaidPair(dtExists, "Denominator", cqmId);
                var keyValuePairother = otherPair(dtExists, "Denominator", cqmId);


                #region Observation

                objObservationParent = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        displayName = "Assertion",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "DENOM",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Denominator",
                            codeSystemName = "ObservationValue"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>(),
                    reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
                };

                #endregion

                objObservationParent.entryRelationship.Add(AggregateCount(denominatorCount));

                foreach (var gender in keyValuePairGender.Where(gender => gender.Value != "0"))
                    objObservationParent.entryRelationship.Add(SexSupplementalDataElement(gender.Key, Int64.Parse(gender.Value), hqmfId));

                foreach (var ethnicity in keyValuePairEthnicity.Where(t => t.Value != "0"))
                {
                    var ethnicityDesc = ethnicity.Key.Split('+').First();
                    var ethnicityCode = ethnicity.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement(ethnicityDesc,
                        Int64.Parse(ethnicity.Value), ethnicityCode, hqmfId));
                }

                foreach (var race in keyValuePairRace.Where(t => t.Value != "0"))
                {
                    var raceDesc = race.Key.Split('+').First();
                    var raceCode = race.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(RaceSupplementalDataElement(raceDesc,
                        Int64.Parse(race.Value), raceCode, hqmfId));
                }

                foreach (var payer in keyValuePairMedicare.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairMedicaid.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairother.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));


            }

            objGetDenominatorComponent.Item = objObservationParent;
            return objGetDenominatorComponent;
        }
        private Component4 GetDenominatorComponent_Zero(Int64 denominatorCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "DENOM");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }

            if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "B57EA797-15A1-4C60-B34C-BAE292FE3B76" : vaueSet;
            }
            else if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "CE973C7A-B867-422B-8408-83538E236039" : vaueSet;
            }
            else if (cqmId == "0075(A)")
            {
                vaueSet = vaueSet == "" ? "3AA8D67A-CA66-4161-9B7A-FFB731EFF022" : vaueSet;
            }
            else if (cqmId == "0075(B)")
            {
                vaueSet = vaueSet == "" ? "6CB61453-F874-48BD-BB0A-6EB67DC7FCBC" : vaueSet;
            }
            else if (cqmId == "CMS50v3")
            {
                vaueSet = vaueSet == "" ? "B710DA92-D4B9-4217-8D1E-94B35163BECE" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "CB0D06D7-9C40-45B0-AEC2-60C39262ABA0" : vaueSet;
            }
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "56E05D20-4529-4EB7-8E30-17C95BBC5223" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "375D0559-C749-4BB9-9267-81EDF447650B" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "36CCB819-C150-466D-A55A-0A175B19E751" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "FFC4DDE0-C91C-4235-BF69-E3EF79457FBB" : vaueSet;
            }
            else if (cqmId == "0022(A)")
            {
                vaueSet = vaueSet == "" ? "07F04D61-0383-487E-942C-690BBBC6437D" : vaueSet;
            }
            else if (cqmId == "0022(B)")
            {
                vaueSet = vaueSet == "" ? "00401314-1B01-4896-A9FC-E991CDF29B6B" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "FE6E1AA0-EE26-4F9E-A2FD-8E36058DCB47" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "E62FEBA3-0F98-460D-93CD-44314D7203A8" : vaueSet;
            }
            else if (cqmId == "0059")
            {
                vaueSet = vaueSet == "" ? "D346DA74-F16E-4159-BEDF-331BA28837FB" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "40FB2D5F-7DEF-4D41-80C4-F3FBA0ED5851" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "C7DFE664-71AE-4EAD-AB65-CDFCF825A44E" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "32635FEA-918B-438F-8421-8A6A14E238E8" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "9665A8D2-F896-47A9-AA7E-271E9815D3CE" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "61B09878-32B3-4ECB-8E78-D1093E1FDDF9" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "589C2FD6-6AA9-4AF8-9E1C-973170361917" : vaueSet;
            }
            else if (cqmId == "0062")
            {
                vaueSet = vaueSet == "" ? "0CC16DE4-474A-4F6E-BFB8-0104709B1E05" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "A3F77B7C-2398-470D-A359-E2A31539DF91" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "2DA1439F-7941-411C-8885-91306F0CD9DD" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "65AFB8BA-F5CC-4ABA-A3FB-2F95FEE2B6FA" : vaueSet;
            }

            var objGetDenominatorComponent = new Component4();
            var objObservationParent = new Observation();

            #region Observation

            objObservationParent = new Observation()
            {
                classCode = "OBS",
                moodCode = x_ActMoodDocumentObservation.EVN,
                templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                code = new CD()
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4",
                    displayName = "Assertion",
                    codeSystemName = "ActCode"
                },
                statusCode = new CS()
                {
                    code = "completed"
                },
                value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "DENOM",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Denominator",
                            codeSystemName = "ObservationValue"
                        }
                    },
                entryRelationship = new List<EntryRelationship>(),
                reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
            };

            #endregion

            objObservationParent.entryRelationship.Add(AggregateCount(denominatorCount));
            objObservationParent.entryRelationship.Add(SexSupplementalDataElement("UNK", Int64.Parse("0"), hqmfId, false));
            objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement("Not Hispanic or Latino", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(RaceSupplementalDataElement("American Indian or Alaska Native", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(PayerSupplementalDataElement("349", Int64.Parse("0"), "349", hqmfId));

            objGetDenominatorComponent.Item = objObservationParent;
            return objGetDenominatorComponent;
        }
        private Component4 GetDenominatorExclusionComponent(Int64 denominatorExclusionCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("DenominatorExclusion") == "1");

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                            .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "DENEX");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }


            if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "74F900B9-65DA-4E6C-BB2D-592189ABDDE5" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "8C56B86F-EFE9-404D-B97C-F8FC4691D50D" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "EE5FFA67-A42D-4472-8159-ED4B87BE6CDA" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "D564DEEE-17E7-4442-921D-436E5113788A" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "55A6D5F3-2029-4896-B850-4C7894161D7D" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "EC5CC49E-42A7-4DD8-BD40-A6258E71DCB9" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "76B54A59-41A9-4664-B85C-F61238AE1DC4" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "29931862-020D-401E-B9E9-953791263D87" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "910A0EE9-ECDA-494C-83E9-30DD9E224FFB" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "02D13BF4-EF77-4DF4-A436-834AEDBE043C" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "93AA9C96-E1FE-435B-BA0B-FAF0C5592275" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "8867C7A7-F8F1-4B61-A6C7-9390C5B5B4EF" : vaueSet;
            }

            var objGetDenominatorExclusionComponent = new Component4();
            var objObservationParent = new Observation();

            if (dtExists.Any())
            {
                var keyValuePairGender = GenderPair(dtExists, "DenominatorExclusion", cqmId);
                var keyValuePairEthnicity = EthnicityPair(dtExists, "DenominatorExclusion", cqmId);
                var keyValuePairRace = RacePair(dtExists, "DenominatorExclusion", cqmId);
                var keyValuePairMedicare = MedicarePair(dtExists, "DenominatorExclusion", cqmId);
                var keyValuePairMedicaid = MedicaidPair(dtExists, "DenominatorExclusion", cqmId);
                var keyValuePairOther = otherPair(dtExists, "DenominatorExclusion", cqmId);

                #region Observation

                objObservationParent = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        displayName = "Assertion",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS() { code = "completed" },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "DENEX",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Denominator Exclusions",
                            codeSystemName = "ObservationValue"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>(),
                    reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
                };

                #endregion

                objObservationParent.entryRelationship.Add(AggregateCount(denominatorExclusionCount));

                foreach (var gender in keyValuePairGender.Where(gender => gender.Value != "0"))
                    objObservationParent.entryRelationship.Add(SexSupplementalDataElement(gender.Key, Int64.Parse(gender.Value), hqmfId));

                foreach (var ethnicity in keyValuePairEthnicity.Where(t => t.Value != "0"))
                {
                    var ethnicityDesc = ethnicity.Key.Split('+').First();
                    var ethnicityCode = ethnicity.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement(ethnicityDesc,
                        Int64.Parse(ethnicity.Value), ethnicityCode, hqmfId));
                }

                foreach (var race in keyValuePairRace.Where(t => t.Value != "0"))
                {
                    var raceDesc = race.Key.Split('+').First();
                    var raceCode = race.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(RaceSupplementalDataElement(raceDesc,
                        Int64.Parse(race.Value), raceCode, hqmfId));
                }

                foreach (var payer in keyValuePairMedicare.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairMedicaid.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairOther.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));


            }

            objGetDenominatorExclusionComponent.Item = objObservationParent;
            return objGetDenominatorExclusionComponent;
        }
        private Component4 GetDenominatorExclusionComponent_Zero(Int64 denominatorExclusionCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                            .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "DENEX");



            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }

            if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "74F900B9-65DA-4E6C-BB2D-592189ABDDE5" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "8C56B86F-EFE9-404D-B97C-F8FC4691D50D" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "EE5FFA67-A42D-4472-8159-ED4B87BE6CDA" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "D564DEEE-17E7-4442-921D-436E5113788A" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "55A6D5F3-2029-4896-B850-4C7894161D7D" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "EC5CC49E-42A7-4DD8-BD40-A6258E71DCB9" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "76B54A59-41A9-4664-B85C-F61238AE1DC4" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "29931862-020D-401E-B9E9-953791263D87" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "910A0EE9-ECDA-494C-83E9-30DD9E224FFB" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "02D13BF4-EF77-4DF4-A436-834AEDBE043C" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "93AA9C96-E1FE-435B-BA0B-FAF0C5592275" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "8867C7A7-F8F1-4B61-A6C7-9390C5B5B4EF" : vaueSet;
            }

            var objGetDenominatorExclusionComponent = new Component4();
            var objObservationParent = new Observation();

            #region Observation

            objObservationParent = new Observation()
            {
                classCode = "OBS",
                moodCode = x_ActMoodDocumentObservation.EVN,
                templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                code = new CD()
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4",
                    displayName = "Assertion",
                    codeSystemName = "ActCode"
                },
                statusCode = new CS() { code = "completed" },
                value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "DENEX",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Denominator Exclusions",
                            codeSystemName = "ObservationValue"
                        }
                    },
                entryRelationship = new List<EntryRelationship>(),
                reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
            };

            #endregion

            objObservationParent.entryRelationship.Add(AggregateCount(denominatorExclusionCount));
            objObservationParent.entryRelationship.Add(SexSupplementalDataElement("UNK", Int64.Parse("0"), hqmfId, false));
            objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement("Not Hispanic or Latino", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(RaceSupplementalDataElement("American Indian or Alaska Native", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(PayerSupplementalDataElement("349", Int64.Parse("0"), "349", hqmfId));

            objGetDenominatorExclusionComponent.Item = objObservationParent;
            return objGetDenominatorExclusionComponent;
        }
        private Component4 GetDenominatorExceptionComponent(Int64 denominatorException, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("DenominatorException") == "1");

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                                        .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "DENEXCEP");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }


            if (cqmId == "0028")
            {
                vaueSet = "655C9F36-02F7-498B-AE18-D322EA2B3726";
            }
            else if (cqmId == "0418")
            {
                vaueSet = "E98E90BE-F3BD-42CD-A194-D6BEFA1BB714";
            }
            //else if (cqmId == "0043")
            //{
            //    vaueSet = vaueSet == "" ? "CBD0926D-6088-44EE-883C-0A0F9E77E2A1" : vaueSet;
            //}
            //else if (cqmId == "0065")
            //{
            //    vaueSet = vaueSet == "" ? "F67DCC8F-4F0F-491B-957F-B21E721B040B" : vaueSet;
            //}
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "9411528A-67EF-439B-9C00-37372B2096E3" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "3C100EC4-2990-4D79-AE14-E816F5E78AC8" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "0E162220-C93B-48B0-AAB1-E2F8E7FE7EA1" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "655C9F36-02F7-498B-AE18-D322EA2B3726" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "CF7E1913-CB4E-42F7-BAEC-0EFFF01ECB17" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "2EE99933-7653-4C53-8506-2A76D0657064" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "2338C9B7-B2C5-4348-BF2B-C9E636FD7A45" : vaueSet;
            }

            var objGetDenominatorExceptionComponent = new Component4();
            var objObservationParent = new Observation();

            if (dtExists.Any())
            {
                var keyValuePairGender = GenderPair(dtExists, "DenominatorException", cqmId);
                var keyValuePairEthnicity = EthnicityPair(dtExists, "DenominatorException", cqmId);
                var keyValuePairRace = RacePair(dtExists, "DenominatorException", cqmId);
                var keyValuePairMedicare = MedicarePair(dtExists, "DenominatorException", cqmId);
                var keyValuePairMedicaid = MedicaidPair(dtExists, "DenominatorException", cqmId);
                var keyValuePairOther = otherPair(dtExists, "DenominatorException", cqmId);

                #region Observation

                objObservationParent = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        displayName = "Assertion",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS() { code = "completed" },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "DENEXCEP",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Denominator Exceptions",
                            codeSystemName = "ObservationValue"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>(),
                    reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
                };

                #endregion

                objObservationParent.entryRelationship.Add(AggregateCount(denominatorException));

                foreach (var gender in keyValuePairGender.Where(gender => gender.Value != "0"))
                    objObservationParent.entryRelationship.Add(SexSupplementalDataElement(gender.Key, Int64.Parse(gender.Value), hqmfId));

                foreach (var ethnicity in keyValuePairEthnicity.Where(t => t.Value != "0"))
                {
                    var ethnicityDesc = ethnicity.Key.Split('+').First();
                    var ethnicityCode = ethnicity.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement(ethnicityDesc,
                        Int64.Parse(ethnicity.Value), ethnicityCode, hqmfId));
                }

                foreach (var race in keyValuePairRace.Where(t => t.Value != "0"))
                {
                    var raceDesc = race.Key.Split('+').First();
                    var raceCode = race.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(RaceSupplementalDataElement(raceDesc,
                        Int64.Parse(race.Value), raceCode, hqmfId));
                }

                foreach (var payer in keyValuePairMedicare.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairMedicaid.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairOther.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

            }

            objGetDenominatorExceptionComponent.Item = objObservationParent;
            return objGetDenominatorExceptionComponent;
        }
        private Component4 GetDenominatorExceptionComponent_Zero(Int64 denominatorException, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "DENEXCEP");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }

            if (cqmId == "0028")
            {
                vaueSet = "655C9F36-02F7-498B-AE18-D322EA2B3726";
            }
            else if (cqmId == "0418")
            {
                vaueSet = "E98E90BE-F3BD-42CD-A194-D6BEFA1BB714";
            }
            //else if (cqmId == "0043")
            //{
            //    vaueSet = vaueSet == "" ? "CBD0926D-6088-44EE-883C-0A0F9E77E2A1" : vaueSet;
            //}
            //else if (cqmId == "0065")
            //{
            //    vaueSet = vaueSet == "" ? "F67DCC8F-4F0F-491B-957F-B21E721B040B" : vaueSet;
            //}
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "9411528A-67EF-439B-9C00-37372B2096E3" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "3C100EC4-2990-4D79-AE14-E816F5E78AC8" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "0E162220-C93B-48B0-AAB1-E2F8E7FE7EA1" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "655C9F36-02F7-498B-AE18-D322EA2B3726" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "CF7E1913-CB4E-42F7-BAEC-0EFFF01ECB17" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "2EE99933-7653-4C53-8506-2A76D0657064" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "2338C9B7-B2C5-4348-BF2B-C9E636FD7A45" : vaueSet;
            }

            var objGetDenominatorExceptionComponent = new Component4();
            var objObservationParent = new Observation();

            #region Observation

            objObservationParent = new Observation()
            {
                classCode = "OBS",
                moodCode = x_ActMoodDocumentObservation.EVN,
                templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                code = new CD()
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4",
                    displayName = "Assertion",
                    codeSystemName = "ActCode"
                },
                statusCode = new CS() { code = "completed" },
                value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "DENEXCEP",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Denominator Exceptions",
                            codeSystemName = "ObservationValue"
                        }
                    },
                entryRelationship = new List<EntryRelationship>(),
                reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
            };

            #endregion

            objObservationParent.entryRelationship.Add(AggregateCount(denominatorException));
            objObservationParent.entryRelationship.Add(SexSupplementalDataElement("UNK", Int64.Parse("0"), hqmfId, false));
            objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement("Not Hispanic or Latino", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(RaceSupplementalDataElement("American Indian or Alaska Native", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(PayerSupplementalDataElement("349", Int64.Parse("0"), "349", hqmfId));

            objGetDenominatorExceptionComponent.Item = objObservationParent;
            return objGetDenominatorExceptionComponent;
        }
        private Component4 GetNumeratorComponent(Int64 numeratorCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("Numerator") == "1");

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                                        .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "NUMER");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }

            if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "CBD0926D-6088-44EE-883C-0A0F9E77E2A1" : vaueSet;
            }
            else if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "F67DCC8F-4F0F-491B-957F-B21E721B040B" : vaueSet;
            }
            else if (cqmId == "0075(A)")
            {
                vaueSet = vaueSet == "" ? "6197C1DB-F85F-4D15-ACC8-E1774C611DDA" : vaueSet;
            }
            else if (cqmId == "0075(B)")
            {
                vaueSet = vaueSet == "" ? "E0990503-8406-427A-B021-ADDDACD08FDC" : vaueSet;
            }
            else if (cqmId == "CMS50v3")
            {
                vaueSet = vaueSet == "" ? "9824B759-A263-44DE-9F5E-93DA4E8F4627" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "D94BAE8B-1EAC-4832-B040-884B4BBC5BD0" : vaueSet;
            }
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "A6516D8B-0A7D-426D-A0EC-F3F4999A2588" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "EFFE261C-0D57-423E-992C-7141B132768C" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "51D06394-AF97-41DD-BF41-68D58786A9D2" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "44BEAC3C-8402-46F0-9494-81B33C502F0A" : vaueSet;
            }
            else if (cqmId == "0022(A)")
            {
                vaueSet = vaueSet == "" ? "7A0001AC-4BE0-4FAA-94AE-4843C9FFFCA8" : vaueSet;
            }
            else if (cqmId == "0022(B)")
            {
                vaueSet = vaueSet == "" ? "FA7BF805-C21E-4077-B43E-C63F8D17B5CF" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "0A3C80F4-A9FF-4BDF-B018-D647E7D777EB" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "F9FEBF42-4B21-47A9-B03E-D2DA5CF8492B" : vaueSet;
            }
            else if (cqmId == "0059")
            {
                vaueSet = vaueSet == "" ? "6D01A564-58CC-4CF5-929F-B83583701BFE" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "0AB40D2B-08CE-4185-8A54-336C3140644D" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "B5FA6E85-0F2E-4674-A3F8-E14D834E73AB" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "33538979-8425-45A4-B724-D74CC0A84EF3" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "2D4D6446-C9CD-4661-868B-C8B9B13A8E08" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "60EC6098-950A-40E5-994F-E9A62CFF6FC2" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "52ADE511-39D4-4CBC-84B6-A82059741359" : vaueSet;
            }
            else if (cqmId == "0062")
            {
                vaueSet = vaueSet == "" ? "5647E3D0-6550-4B0B-AE76-53CD9E99D20B" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "F0F4094E-CE53-44EE-8CEF-0555479E37B2" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "2C2431A3-7DE3-4C97-80C0-4BF96D4880F0" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "8964CE5A-64A4-4259-9359-39A1CDC07B8D" : vaueSet;
            }

            var objGetNumeratorComponent = new Component4();
            var objObservationParent = new Observation();

            if (dtExists.Any())
            {
                var keyValuePairGender = GenderPair(dtExists, "Numerator", cqmId);
                var keyValuePairEthnicity = EthnicityPair(dtExists, "Numerator", cqmId);
                var keyValuePairRace = RacePair(dtExists, "Numerator", cqmId);
                var keyValuePairMedicare = MedicarePair(dtExists, "Numerator", cqmId);
                var keyValuePairMedicaid = MedicaidPair(dtExists, "Numerator", cqmId);
                var keyValuePairother = otherPair(dtExists, "Numerator", cqmId);


                #region Observation

                objObservationParent = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        displayName = "Assertion",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS() { code = "completed" },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "NUMER",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Numerator",
                            codeSystemName = "ObservationValue"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>(),
                    reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
                };

                #endregion

                objObservationParent.entryRelationship.Add(AggregateCount(numeratorCount));

                foreach (var gender in keyValuePairGender.Where(gender => gender.Value != "0"))
                    objObservationParent.entryRelationship.Add(SexSupplementalDataElement(gender.Key, Int64.Parse(gender.Value), hqmfId));

                foreach (var ethnicity in keyValuePairEthnicity.Where(t => t.Value != "0"))
                {
                    var ethnicityDesc = ethnicity.Key.Split('+').First();
                    var ethnicityCode = ethnicity.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement(ethnicityDesc,
                        Int64.Parse(ethnicity.Value), ethnicityCode, hqmfId));
                }

                foreach (var race in keyValuePairRace.Where(t => t.Value != "0"))
                {
                    var raceDesc = race.Key.Split('+').First();
                    var raceCode = race.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(RaceSupplementalDataElement(raceDesc,
                        Int64.Parse(race.Value), raceCode, hqmfId));
                }

                foreach (var payer in keyValuePairMedicare.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairMedicaid.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairother.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

            }

            objGetNumeratorComponent.Item = objObservationParent;
            return objGetNumeratorComponent;
        }
        private Component4 GetNumeratorTwoComponent(Int64 numeratorCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("Numerator") == "1");

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                                        .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "NUMER2");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }
            //if (cqmId == "0419" || cqmId == "0419D")
            //{
            //    vaueSet = "14A01D7A-B936-4148-889F-C9B1A8B3D8A3";
            //}
            var objGetNumeratorComponent = new Component4();
            var objObservationParent = new Observation();

            if (dtExists.Any())
            {
                var keyValuePairGender = GenderPair(dtExists, "Numerator", cqmId);
                var keyValuePairEthnicity = EthnicityPair(dtExists, "Numerator", cqmId);
                var keyValuePairRace = RacePair(dtExists, "Numerator", cqmId);
                var keyValuePairMedicare = MedicarePair(dtExists, "Numerator", cqmId);
                var keyValuePairMedicaid = MedicaidPair(dtExists, "Numerator", cqmId);
                var keyValuePairOther = otherPair(dtExists, "Numerator", cqmId);

                #region Observation

                objObservationParent = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        displayName = "Assertion",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS() { code = "completed" },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "NUMER",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Numerator",
                            codeSystemName = "ObservationValue"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>(),
                    reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
                };

                #endregion

                objObservationParent.entryRelationship.Add(AggregateCount(numeratorCount));

                foreach (var gender in keyValuePairGender.Where(gender => gender.Value != "0"))
                    objObservationParent.entryRelationship.Add(SexSupplementalDataElement(gender.Key, Int64.Parse(gender.Value), hqmfId));

                foreach (var ethnicity in keyValuePairEthnicity.Where(t => t.Value != "0"))
                {
                    var ethnicityDesc = ethnicity.Key.Split('+').First();
                    var ethnicityCode = ethnicity.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement(ethnicityDesc,
                        Int64.Parse(ethnicity.Value), ethnicityCode, hqmfId));
                }

                foreach (var race in keyValuePairRace.Where(t => t.Value != "0"))
                {
                    var raceDesc = race.Key.Split('+').First();
                    var raceCode = race.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(RaceSupplementalDataElement(raceDesc,
                        Int64.Parse(race.Value), raceCode, hqmfId));
                }

                foreach (var payer in keyValuePairMedicare.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairMedicaid.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairOther.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

            }

            objGetNumeratorComponent.Item = objObservationParent;
            return objGetNumeratorComponent;
        }
        private Component4 GetNumeratorComponent_Zero(Int64 numeratorCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            //var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
            //    .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "NUMER2");

            var vaueSet = "";
            //var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            //if (catagoryIiiValueSet.Any())
            //{
            //    var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
            //    vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            //}

            if (cqmId == "0043")
            {
                vaueSet = vaueSet == "" ? "CBD0926D-6088-44EE-883C-0A0F9E77E2A1" : vaueSet;
            }
            else if (cqmId == "0065")
            {
                vaueSet = vaueSet == "" ? "F67DCC8F-4F0F-491B-957F-B21E721B040B" : vaueSet;
            }
            else if (cqmId == "0075(A)")
            {
                vaueSet = vaueSet == "" ? "6197C1DB-F85F-4D15-ACC8-E1774C611DDA" : vaueSet;
            }
            else if (cqmId == "0075(B)")
            {
                vaueSet = vaueSet == "" ? "E0990503-8406-427A-B021-ADDDACD08FDC" : vaueSet;
            }
            else if (cqmId == "CMS50v3")
            {
                vaueSet = vaueSet == "" ? "9824B759-A263-44DE-9F5E-93DA4E8F4627" : vaueSet;
            }
            else if (cqmId == "0418")
            {
                vaueSet = vaueSet == "" ? "D94BAE8B-1EAC-4832-B040-884B4BBC5BD0" : vaueSet;
            }
            else if (cqmId == "0041")
            {
                vaueSet = vaueSet == "" ? "A6516D8B-0A7D-426D-A0EC-F3F4999A2588" : vaueSet;
            }
            else if (cqmId == "0419")
            {
                vaueSet = vaueSet == "" ? "EFFE261C-0D57-423E-992C-7141B132768C" : vaueSet;
            }
            else if (cqmId == "0421" || cqmId == "0421a")
            {
                vaueSet = vaueSet == "" ? "51D06394-AF97-41DD-BF41-68D58786A9D2" : vaueSet;
            }
            else if (cqmId == "0028")
            {
                vaueSet = vaueSet == "" ? "44BEAC3C-8402-46F0-9494-81B33C502F0A" : vaueSet;
            }
            else if (cqmId == "0022(A)")
            {
                vaueSet = vaueSet == "" ? "7A0001AC-4BE0-4FAA-94AE-4843C9FFFCA8" : vaueSet;
            }
            else if (cqmId == "0022(B)")
            {
                vaueSet = vaueSet == "" ? "FA7BF805-C21E-4077-B43E-C63F8D17B5CF" : vaueSet;
            }
            else if (cqmId == "0068")
            {
                vaueSet = vaueSet == "" ? "0A3C80F4-A9FF-4BDF-B018-D647E7D777EB" : vaueSet;
            }
            else if (cqmId == "0018")
            {
                vaueSet = vaueSet == "" ? "F9FEBF42-4B21-47A9-B03E-D2DA5CF8492B" : vaueSet;
            }
            else if (cqmId == "0059")
            {
                vaueSet = vaueSet == "" ? "6D01A564-58CC-4CF5-929F-B83583701BFE" : vaueSet;
            }
            else if (cqmId == "0031")
            {
                vaueSet = vaueSet == "" ? "0AB40D2B-08CE-4185-8A54-336C3140644D" : vaueSet;
            }
            else if (cqmId == "0712(A)")
            {
                vaueSet = vaueSet == "" ? "B5FA6E85-0F2E-4674-A3F8-E14D834E73AB" : vaueSet;
            }
            else if (cqmId == "0712(B)")
            {
                vaueSet = vaueSet == "" ? "33538979-8425-45A4-B724-D74CC0A84EF3" : vaueSet;
            }
            else if (cqmId == "0712(C)")
            {
                vaueSet = vaueSet == "" ? "2D4D6446-C9CD-4661-868B-C8B9B13A8E08" : vaueSet;
            }
            else if (cqmId == "0056")
            {
                vaueSet = vaueSet == "" ? "60EC6098-950A-40E5-994F-E9A62CFF6FC2" : vaueSet;
            }
            else if (cqmId == "0034")
            {
                vaueSet = vaueSet == "" ? "52ADE511-39D4-4CBC-84B6-A82059741359" : vaueSet;
            }
            else if (cqmId == "0062")
            {
                vaueSet = vaueSet == "" ? "5647E3D0-6550-4B0B-AE76-53CD9E99D20B" : vaueSet;
            }
            else if (cqmId == "0101")
            {
                vaueSet = vaueSet == "" ? "F0F4094E-CE53-44EE-8CEF-0555479E37B2" : vaueSet;
            }
            else if (cqmId == "2872")
            {
                vaueSet = vaueSet == "" ? "2C2431A3-7DE3-4C97-80C0-4BF96D4880F0" : vaueSet;
            }
            else if (cqmId == "CMS22v5")
            {
                vaueSet = vaueSet == "" ? "8964CE5A-64A4-4259-9359-39A1CDC07B8D" : vaueSet;
            }

            var objGetNumeratorComponent = new Component4();
            var objObservationParent = new Observation();

            #region Observation

            objObservationParent = new Observation()
            {
                classCode = "OBS",
                moodCode = x_ActMoodDocumentObservation.EVN,
                templateId = new List<II>()
                {
                    new II()
                    {
                        root = "2.16.840.1.113883.10.20.27.3.5"
                    }
                },
                code = new CD()
                {
                    code = "ASSERTION",
                    codeSystem = "2.16.840.1.113883.5.4",
                    displayName = "Assertion",
                    codeSystemName = "ActCode"
                },
                statusCode = new CS() { code = "completed" },
                value = new List<ANY>()
                {
                    new CD()
                    {
                        code = "NUMER",
                        codeSystem = "2.16.840.1.113883.5.1063",
                        displayName = "Numerator",
                        codeSystemName = "ObservationValue"
                    }
                },
                entryRelationship = new List<EntryRelationship>(),
                reference = new List<Reference>()
                {
                    MeasureTemplateReference(vaueSet)
                }
            };

            #endregion

            objObservationParent.entryRelationship.Add(AggregateCount(numeratorCount));
            objObservationParent.entryRelationship.Add(SexSupplementalDataElement("UNK", Int64.Parse("0"), hqmfId, false));
            objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement("Not Hispanic or Latino", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(RaceSupplementalDataElement("American Indian or Alaska Native", Int64.Parse("0"), "UNK", hqmfId, false));
            objObservationParent.entryRelationship.Add(PayerSupplementalDataElement("349", Int64.Parse("0"), "349", hqmfId));

            objGetNumeratorComponent.Item = objObservationParent;
            return objGetNumeratorComponent;
        }
        private Component4 GetNumeratorExclusionComponent(Int64 numeratorExclusionCount, string hqmfId, string cqmId, EnumerableRowCollection<DataRow> drEnumerableRowCollection)
        {
            var dtExists = _dsMeasureSection.Tables[_dsMeasureSection.CQM_CQM_Details.TableName].AsEnumerable()
                .Where(r => r.Field<string>("CQMID") == cqmId && r.Field<string>("NumeratorExclusion") == "1");

            var dtCatagoryIiiValueSet = drEnumerableRowCollection.AsEnumerable()
                                        .Where(r => r.Field<string>("CQMId") == cqmId && r.Field<string>("Population") == "NUMEREX");

            var vaueSet = "";
            var catagoryIiiValueSet = dtCatagoryIiiValueSet as DataRow[] ?? dtCatagoryIiiValueSet.ToArray();

            if (catagoryIiiValueSet.Any())
            {
                var dtCatagoryIii = catagoryIiiValueSet.CopyToDataTable();
                vaueSet = dtCatagoryIii.Rows[0]["PopulationValueSet"].ToString();
            }

            var objGetNumeratorExclusionComponent = new Component4();
            var objObservationParent = new Observation();

            if (dtExists.Any())
            {
                var keyValuePairGender = GenderPair(dtExists, "NumeratorExclusion", cqmId);
                var keyValuePairEthnicity = EthnicityPair(dtExists, "NumeratorExclusion", cqmId);
                var keyValuePairRace = RacePair(dtExists, "NumeratorExclusion", cqmId);
                var keyValuePairMedicare = MedicarePair(dtExists, "NumeratorExclusion", cqmId);
                var keyValuePairMedicaid = MedicaidPair(dtExists, "NumeratorExclusion", cqmId);
                var keyValuePairOther = otherPair(dtExists, "NumeratorExclusion", cqmId);


                #region Observation

                objObservationParent = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.5"
                        }
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        displayName = "Assertion",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS() { code = "completed" },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = "NUMEX",
                            codeSystem = "2.16.840.1.113883.5.1063",
                            displayName = "Numerator Exclusions",
                            codeSystemName = "ObservationValue"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>(),
                    reference = new List<Reference>()
                    {
                        MeasureTemplateReference(vaueSet)
                    }
                };

                #endregion

                objObservationParent.entryRelationship.Add(AggregateCount(numeratorExclusionCount));

                foreach (var gender in keyValuePairGender.Where(gender => gender.Value != "0"))
                    objObservationParent.entryRelationship.Add(SexSupplementalDataElement(gender.Key, Int64.Parse(gender.Value), hqmfId));

                foreach (var ethnicity in keyValuePairEthnicity.Where(t => t.Value != "0"))
                {
                    var ethnicityDesc = ethnicity.Key.Split('+').First();
                    var ethnicityCode = ethnicity.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(EthnicitySupplementalDataElement(ethnicityDesc,
                        Int64.Parse(ethnicity.Value), ethnicityCode, hqmfId));
                }

                foreach (var race in keyValuePairRace.Where(t => t.Value != "0"))
                {
                    var raceDesc = race.Key.Split('+').First();
                    var raceCode = race.Key.Split('+').Last();

                    objObservationParent.entryRelationship.Add(RaceSupplementalDataElement(raceDesc,
                        Int64.Parse(race.Value), raceCode, hqmfId));
                }

                foreach (var payer in keyValuePairMedicare.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairMedicaid.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));

                foreach (var payer in keyValuePairOther.Where(t => t.Value != "0"))
                    objObservationParent.entryRelationship.Add(PayerSupplementalDataElement(payer.Key, Int64.Parse(payer.Value), payer.Key, hqmfId));


            }

            objGetNumeratorExclusionComponent.Item = objObservationParent;
            return objGetNumeratorExclusionComponent;
        }

        #endregion

        #region Measure Section Componnents
        private Reference MeasureTemplateReference()
        {
            var objMeasureTemplateReference = new Reference()
            {
                typeCode = x_ActRelationshipExternalReference.REFR,
                Item = new ExternalObservation()
                {
                    classCode = "OBS",
                    moodCode = "EVN",
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = Guid.NewGuid().ToString()
                        }
                    }
                }
            };
            return objMeasureTemplateReference;
        }
        private Reference MeasureTemplateReference(string valueSet)
        {
            var objMeasureTemplateReference = new Reference()
            {
                typeCode = x_ActRelationshipExternalReference.REFR,
                Item = new ExternalObservation()
                {
                    classCode = "OBS",
                    moodCode = "EVN",
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = valueSet == "" ?Guid.NewGuid().ToString(): valueSet
                        }
                    }
                }
            };
            return objMeasureTemplateReference;
        }
        private EntryRelationship AggregateCount(Int64 count)
        {
            var objAggregateCount = new EntryRelationship()
            {
                typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                inversionInd = true,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.3"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.24"
                        }
                    },
                    code = new CD()
                    {
                        code = "MSRAGG",
                        displayName = "rate aggregation",
                        codeSystem = "2.16.840.1.113883.5.4",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    value = new List<ANY>()
                    {
                        new INT()
                        {
                            value = count.ToString()
                        }
                    },
                    methodCode = new List<CE>()
                    {
                        new CE()
                        {
                            code = "COUNT",
                            displayName = "Count",
                            codeSystem = "2.16.840.1.113883.5.84",
                            codeSystemName = "ObservationMethod"
                        }
                    }
                }
            };
            return objAggregateCount;
        }
        private EntryRelationship InitialPopulation(Int64 initialPopulationcount)
        {
            var objInitialPopulationXml = new EntryRelationship()
            {
                typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                inversionInd = true,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.3"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.24"
                        }
                    },
                    code = new CD()
                    {
                        code = "MSRAGG",
                        displayName = "rate aggregation",
                        codeSystem = "2.16.840.1.113883.5.4",
                        codeSystemName = "ActCode"
                    },
                    value = new List<ANY>()
                    {
                        new INT()
                        {
                            value = initialPopulationcount.ToString()
                        }
                    },
                    methodCode = new List<CE>()
                    {
                        new CE()
                        {
                            code = "COUNT",
                            displayName = "Count",
                            codeSystem = "2.16.840.1.113883.5.84",
                            codeSystemName = "ObservationMethod"
                        }
                    }
                }
            };
            return objInitialPopulationXml;
        }
        private EntryRelationship SexSupplementalDataElement(string sex, Int64 count, string hqmfId, bool makeSagment = true)
        {
            var objEntryRelation = new EntryRelationship();
            if (makeSagment)
            {
                objEntryRelation = new EntryRelationship
                {
                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>()
                        {
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.6", extension="2016-09-01"
                            },
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.21", extension="2016-11-01"
                            }
                        },
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = hqmfId
                            }
                        },
                        code = new CD()
                        {
                            code = "76689-9",
                            displayName = "patient sex",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        },
                        statusCode = new CS()
                        {
                            code = "completed"
                        },
                        value = new List<ANY>()
                        {
                            new CD()
                            {
                                code = sex,
                                codeSystem = "2.16.840.1.113883.5.1",
                                codeSystemName = "AdministrativeGenderCode"
                            }
                        },
                        entryRelationship = new List<EntryRelationship>
                        {
                            AggregateCount(count)
                        }
                    }
                };
                return objEntryRelation;
            }
            else
            {
                objEntryRelation = new EntryRelationship
                {
                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>()
                        {
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.6", extension="2016-09-01"
                            },
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.21", extension="2016-11-01"
                            }
                        },
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = hqmfId
                            }
                        },
                        code = new CD()
                        {
                            code = "76689-9",
                            displayName = "patient sex",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        },
                        statusCode = new CS()
                        {
                            code = "completed"
                        },
                        value = new List<ANY>()
                        {
                            new CD()
                            {
                                nullFlavor = sex,
                                codeSystem = "2.16.840.1.113883.5.1",
                                codeSystemName = "AdministrativeGenderCode"
                            }
                        },
                        entryRelationship = new List<EntryRelationship>
                        {
                            AggregateCount(count)
                        }
                    }
                };
                return objEntryRelation;
            }
        }
        private EntryRelationship EthnicitySupplementalDataElement(string ethincity, Int64 count, string ethincityCode, string hqmfId, bool makeSagment = true)
        {
            var objEthnicitySupplementalDataElement = new EntryRelationship();
            if (makeSagment)
            {
                objEthnicitySupplementalDataElement = new EntryRelationship
                {
                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>()
                        {
                             new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.7", extension="2016-09-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.22", extension="2016-11-01"
                        }
                        },
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = hqmfId
                            }
                        },
                        code = new CD()
                        {
                            code = "69490-1",
                            displayName = "Ethnic",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        },
                        statusCode = new CS()
                        {
                            code = "completed"
                        },
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = "20150101"
                                        },
                                        new IVXB_TS
                                        {
                                            value = "20151231"
                                        }
                                    }
                        },
                        value = new List<ANY>()
                        {
                            new CD()
                            {
                                code = ethincityCode,
                                displayName = ethincity,
                                codeSystem = "2.16.840.1.113883.6.238",
                                codeSystemName = "Race &amp; Ethnicity - CDC"
                            }
                        },
                        entryRelationship = new List<EntryRelationship>()
                        {
                            AggregateCount(count)
                        }
                    }
                };
                return objEthnicitySupplementalDataElement;
            }
            else
            {
                objEthnicitySupplementalDataElement = new EntryRelationship()
                {
                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>()
                        {
                             new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.7", extension="2016-09-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.22", extension="2016-11-01"
                        }
                        },
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = hqmfId
                            }
                        },
                        code = new CD()
                        {
                            code = "69490-1",
                            displayName = "Ethnic",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        },
                        statusCode = new CS()
                        {
                            code = "completed"
                        },
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = "20150101"
                                        },
                                        new IVXB_TS
                                        {
                                            value = "20151231"
                                        }
                                    }
                        },
                        value = new List<ANY>()
                        {
                            new CD()
                            {
                                nullFlavor = ethincityCode,
                                displayName = ethincity,
                                codeSystem = "2.16.840.1.113883.6.238",
                                codeSystemName = "Race &amp; Ethnicity - CDC"
                            }
                        },
                        entryRelationship = new List<EntryRelationship>()
                        {
                            AggregateCount(count)
                        }
                    }
                };
                return objEthnicitySupplementalDataElement;
            }
        }
        private EntryRelationship RaceSupplementalDataElement(string race, Int64 count, string raceCode, string hqmfId, bool makeSagment = true)
        {
            var objRaceSupplementalDataElement = new EntryRelationship();
            if (makeSagment)
            {
                objRaceSupplementalDataElement = new EntryRelationship()
                {
                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>()
                        {
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.8", extension="2016-09-01"
                            },
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.19", extension="2016-11-01"
                            }
                        },
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = hqmfId
                            }
                        },
                        code = new CD()
                        {
                            code = "72826-1",
                            displayName = "Race",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        },
                        statusCode = new CS()
                        {
                            code = "completed"
                        },
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = "20150101"
                                        },
                                        new IVXB_TS
                                        {
                                            value = "20151231"
                                        }
                                    }
                        },
                        value = new List<ANY>()
                        {
                            new CD()
                            {
                                code = raceCode,
                                displayName = race,
                                codeSystem = "2.16.840.1.113883.6.238",
                                codeSystemName = "Race &amp; Ethnicity - CDC"

                            }
                        },
                        entryRelationship = new List<EntryRelationship>()
                        {
                            AggregateCount(count)
                        }
                    }
                };
                return objRaceSupplementalDataElement;
            }
            else
            {
                objRaceSupplementalDataElement = new EntryRelationship()
                {
                    typeCode = x_ActRelationshipEntryRelationship.COMP,
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>()
                        {
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.8", extension="2016-09-01"
                            }
                        },
                        id = new List<II>()
                        {
                            new II()
                            {
                                root = hqmfId
                            }
                        },
                        code = new CD()
                        {
                            code = "72826-1",
                            displayName = "Race",
                            codeSystem = "2.16.840.1.113883.6.1",
                            codeSystemName = "LOINC"
                        },
                        statusCode = new CS()
                        {
                            code = "completed"
                        },
                        effectiveTime = new IVL_TS
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = "20150101"
                                        },
                                        new IVXB_TS
                                        {
                                            value = "20151231"
                                        }
                                    }
                        },
                        value = new List<ANY>()
                        {
                            new CD()
                            {
                                nullFlavor = raceCode,
                                displayName = race,
                                codeSystem = "2.16.840.1.113883.6.238",
                                codeSystemName = "Race &amp; Ethnicity - CDC"

                            }
                        },
                        entryRelationship = new List<EntryRelationship>()
                        {
                            AggregateCount(count)
                        }
                    }
                };
                return objRaceSupplementalDataElement;
            }
        }
        private EntryRelationship PayerSupplementalDataElement(string payer, Int64 count, string payerCode, string hqmfId)
        {
            var objPayerSupplementalDataElement = new EntryRelationship
            {
                typeCode = x_ActRelationshipEntryRelationship.COMP,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.9", extension="2016-02-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.18", extension="2016-11-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = hqmfId
                        }
                    },
                    code = new CD()
                    {
                        code = "48768-6",
                        displayName = "Payment source",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    },
                    statusCode = new CS() { code = "completed" },
                    effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = DateTime.Parse(startDate).ToString("yyyyMMdd")
                                        },
                                        new IVXB_TS
                                        {
                                            value = DateTime.Parse(endDate).ToString("yyyyMMdd")
                                        }
                                    }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {

                            code = payerCode,
                            displayName = "Payer - Other", // payer
                            codeSystem = "2.16.840.1.113883.3.221.5",
                            codeSystemName = "Source of Payment Typology"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>
                    {
                        AggregateCount(count)
                    }
                }
            };
            return objPayerSupplementalDataElement;
        }
        private EntryRelationship PostalCodeSupplementalDataElement(string zipCode, Int64 count, string hqmfId)
        {
            var objPostalCodeSupplementalDataElement = new EntryRelationship()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.10",
                        }
                    },
                    code = new CD()
                    {
                        code = "184102003",
                        displayName = "patient postal code",
                        codeSystem = "2.16.840.1.113883.6.96",
                        codeSystemName = "SNOMED-CT"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    value = new List<ANY>()
                    {
                        new ST()
                        {
                            Text = new List<string>() {zipCode}
                        }
                    },
                    entryRelationship = new List<EntryRelationship>()
                    {
                            AggregateCount(count)
                    }
                }
            };
            return objPostalCodeSupplementalDataElement;
        }
        private EntryRelationship ReportingStratum(Int64 count, string hqmfId)
        {
            var objReportingStratum = new EntryRelationship
            {
                typeCode = x_ActRelationshipEntryRelationship.COMP,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>() { new II() { root = "2.16.840.1.113883.10.20.27.3.4" } },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        displayName = "Assertion",
                        codeSystem = "2.16.840.1.113883.5.4",
                        codeSystemName = "ActCode"
                    },
                    statusCode = new CS() { code = "completed" },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            nullFlavor = "OTH",
                            originalText = new ED() {Text = new List<string>() {"Stratum"}}
                        }
                    },
                    entryRelationship = new List<EntryRelationship>
                    {
                        AggregateCount(count)
                    },
                    reference = new List<Reference>()
                    {
                        new Reference()
                        {
                            typeCode = x_ActRelationshipExternalReference.REFR,
                            Item = new ExternalObservation
                            {
                                classCode = "OBS",
                                moodCode = "EVN",
                                id = new List<II>()
                                {
                                    new II()
                                    {
                                        root = hqmfId
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return objReportingStratum;
        }

        #region AweSomeness Overloaded

        private EntryRelationship SexSupplementalDataElement_Zero(string sex, Int64 count, string hqmfId)
        {
            var objEntryRelation = new EntryRelationship
            {
                typeCode = x_ActRelationshipEntryRelationship.COMP,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                       new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.6", extension="2016-09-01"
                            },
                            new II()
                            {
                                root = "2.16.840.1.113883.10.20.27.3.21", extension="2016-11-01"
                            }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = hqmfId
                        }
                    },
                    code = new CD()
                    {
                        code = "76689-9",
                        displayName = "patient sex",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            nullFlavor = sex,
                            codeSystem = "2.16.840.1.113883.5.1",
                            codeSystemName = "AdministrativeGenderCode"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>
                    {
                        AggregateCount(count)
                    }
                }
            };
            return objEntryRelation;
        }
        private EntryRelationship EthnicitySupplementalDataElement_Zero(string ethincity, Int64 count, string ethincityCode, string hqmfId)
        {
            var objEthnicitySupplementalDataElement = new EntryRelationship()
            {
                typeCode = x_ActRelationshipEntryRelationship.COMP,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.7", extension="2016-09-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.22", extension="2016-11-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = hqmfId
                        }
                    },
                    code = new CD()
                    {
                        code = "69490-1",
                        displayName = "Ethnic",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = "20150101"
                                        },
                                        new IVXB_TS
                                        {
                                            value = "20151231"
                                        }
                                    }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            nullFlavor = ethincityCode,
                            displayName = ethincity,
                            codeSystem = "2.16.840.1.113883.6.238",
                            codeSystemName = "Race &amp; Ethnicity - CDC"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>()
                    {
                        AggregateCount(count)
                    }
                }
            };
            return objEthnicitySupplementalDataElement;
        }
        private EntryRelationship RaceSupplementalDataElement_Zero(string race, Int64 count, string raceCode, string hqmfId)
        {
            var objRaceSupplementalDataElement = new EntryRelationship()
            {
                typeCode = x_ActRelationshipEntryRelationship.COMP,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.8", extension="2016-09-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = hqmfId
                        }
                    },
                    code = new CD()
                    {
                        code = "72826-1",
                        displayName = "Race",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                        Items = new QTY[]
                                    {
                                        new IVXB_TS
                                        {
                                            value = "20150101"
                                        },
                                        new IVXB_TS
                                        {
                                            value = "20151231"
                                        }
                                    }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            nullFlavor = raceCode,
                            displayName = race,
                            codeSystem = "2.16.840.1.113883.6.238",
                            codeSystemName = "Race &amp; Ethnicity - CDC"

                        }
                    },
                    entryRelationship = new List<EntryRelationship>()
                    {
                        AggregateCount(count)
                    }
                }
            };
            return objRaceSupplementalDataElement;
        }
        private EntryRelationship PayerSupplementalDataElement_Zero(string payer, Int64 count, string payerCode, string hqmfId)
        {
            var objPayerSupplementalDataElement = new EntryRelationship
            {
                typeCode = x_ActRelationshipEntryRelationship.COMP,
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>()
                    {
                       new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.9", extension="2016-02-01"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.27.3.18", extension="2016-11-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = hqmfId
                        }
                    },
                    code = new CD()
                    {
                        code = "48768-6",
                        displayName = "Payment source",
                        codeSystem = "2.16.840.1.113883.6.1",
                        codeSystemName = "LOINC"
                    },
                    statusCode = new CS() { code = "completed" },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[] { ItemsChoiceType2.low },
                        Items = new QTY[]
                        {
                            new IVXB_TS()
                            {
                                nullFlavor = "UNK"
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            nullFlavor = payerCode,
                            displayName = "Payer - Other", // payer
                            codeSystem = "2.16.840.1.113883.3.221.5",
                            codeSystemName = "Source of Payment Typology"
                        }
                    },
                    entryRelationship = new List<EntryRelationship>
                    {
                        AggregateCount(count)
                    }
                }
            };
            return objPayerSupplementalDataElement;
        }

        #endregion

        #endregion

        #endregion

        #region Set Address
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

        #region Measure Section, StrucDocTable
        private static StrucDocTable StrucDocTableMeasureSection(DataRow drMeasureSection) //DSCQM dsMeasureSection
        {
            var cqmid = drMeasureSection[_dsMeasureSection.CQM.CQMIDColumn.ColumnName].ToString();
            if (cqmid == "0421a")
            {
                cqmid = "0421";
            }

            if (drMeasureSection.ItemArray.Any())
            {
                var objMeasureSection = new StrucDocTable()
                {
                    thead = new StrucDocThead()
                    {
                        tr = new List<StrucDocTr>()
                        {
                            new StrucDocTr()
                            {
                                Items = new List<object>
                                {
                                    new StrucDocTh() {Text = new List<string> {"eMeasure Title"}},
                                    new StrucDocTh() {Text = new List<string> {"Version neutral identifier"}},
                                    new StrucDocTh() {Text = new List<string> {"eMeasure Version Number"}},
                                    new StrucDocTh() {Text = new List<string> {"NQF eMeasure Number"}},
                                    new StrucDocTh() {Text = new List<string> {"eMeasure Identifier (MAT)"}},
                                    new StrucDocTh() {Text = new List<string> {"Version specific identifier"}}
                                }
                            }
                        }
                    },
                    tbody = new List<StrucDocTbody>()
                    {
                        new StrucDocTbody()
                        {
                            tr = new List<StrucDocTr>()
                            {
                                new StrucDocTr()
                                {
                                    Items = new List<object>
                                    {
                                        new StrucDocTd()
                                        {
                                            Text =
                                                new List<string>
                                                {
                                                    drMeasureSection[_dsMeasureSection.CQM.MeasureColumn.ColumnName].ToString()
                                                }
                                        },
                                        new StrucDocTd() {Text = new List<string> {Guid.NewGuid().ToString()}},
                                        new StrucDocTd()
                                        {
                                            Text =
                                                new List<string>
                                                {
                                                    drMeasureSection[
                                                        _dsMeasureSection.CQM.eMeasureVersionNumberColumn.ColumnName].ToString()
                                                }
                                        },
                                        new StrucDocTd()
                                        {
                                            Text =
                                                new List<string>
                                                {
                                                    cqmid
                                                }
                                        },
                                        new StrucDocTd()
                                        {
                                            Text =
                                                new List<string>
                                                {
                                                    drMeasureSection[
                                                        _dsMeasureSection.CQM.eMeasureIdentifierColumn.ColumnName].ToString()
                                                }
                                        },
                                        new StrucDocTd()
                                        {
                                            Text = new List<string>
                                            {
                                                drMeasureSection[
                                                        _dsMeasureSection.CQM.VersionSpecificIdentifierColumn.ColumnName].ToString()
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
                return objMeasureSection;
            }
            else
            {
                var objMeasureSection = new StrucDocTable();
                return objMeasureSection;
            }
        }

        #endregion
    }
}