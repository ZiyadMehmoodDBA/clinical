using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using iTextSharp.text;
using MDVision.Datasets;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Batch.CQM
{
    public class CatagoryOneXMLSectionLevelTemplates : CatagoryOneXMLGenerator
    {

        private static DSCQM _dsMeasureSection;
        private static DSCQM _dsReportingParameterSection;

        private static DSCQM _dsPatientDataSection;
        private static DSCQM _dsPatientDataSectionCodes;

        private static DSCQM _dsDiagnosisActiveConcernAct;
        private static DSCQM _dsFamilyHx;
        private static DSCQM _dsMedicationActive;
        private static DSCQM _dsMedicationAdministered;
        private static DSCQM _dsMedicationAllergy;
        private static DSCQM _dsMedicationOrder;
        private static DSCQM _dsProcedureOrder;

        public static string CQMID = "";
        public static string interventionExtension = "";
        private static AmalgamatedClinicalDocument _document;

        private StructuredBody StructuredBody
        {
            get { return _document.component.Item as StructuredBody; }
        }

        public void BuildSectionLevelTemplate(AmalgamatedClinicalDocument document, DSCQM dsPatientDemoGraphic, DSProfile dsProvider, DSProfile dsPractice, string cqmid, Int64 patientId, Int64 providerId, Int64 practiceId, string startDate, string endDate, DSCQM dsMeasureSection, bool isC1 = false)
        {
            _document = document;

            #region InitializeSegments

            #region InitializeMeasureSection

            _dsMeasureSection = dsMeasureSection;

            #endregion

            #region InitializeReportingParameterSection

            //_dsReportingParameterSection = GetReportingParameterSection_CategoryOne(providerId, cqmid);


            #endregion

            #region InitializePatientDataSection

            _dsPatientDataSection = GetPatientDataSection_CategoryOne(patientId, cqmid);
            _dsPatientDataSectionCodes = PatientDataSectionCodes_CategoryOne(providerId, startDate, endDate, patientId.ToString(), 2, cqmid, isC1);

            #endregion

            #endregion

            SectionLevelTemplates(startDate, endDate, cqmid);
        }

        private void SectionLevelTemplates(string startDate, string endDate, string cqmid)
        {
            MeasureSection(cqmid);
            ReportingParameterSection(startDate, endDate);
            PatientDataSection(cqmid);
        }


        #region Data Capture for MeasureSection, ReportingParameterSection and PatientDataSection

        #region Measure Section

        private static DSCQM GetMeasureSection_CategoryOne(Int64 providerId, string cqmid)
        {
            var obj = new BLLCQM().Load_CQM(providerId, null, null, null, null, 0, cqmid);
            _dsMeasureSection = obj.Data;
            return _dsMeasureSection;
        }

        #endregion

        #region Reporting Parameter Section

        private static DSCQM GetReportingParameterSection_CategoryOne(Int64 providerId, string nqfId)
        {
            var obj = new BLLCQM().ReportingParameterSection(providerId, nqfId);
            _dsReportingParameterSection = obj.Data;
            return _dsReportingParameterSection;
        }

        #endregion

        #region PatientData Section
        private static DSCQM GetPatientDataSection_CategoryOne(Int64 patientId, string nqfId, string loinc = "")
        {
            var obj = new BLLCQM().PatientDataSection(patientId, nqfId, loinc);
            _dsPatientDataSection = obj.Data;
            return _dsPatientDataSection;
        }

        private static DSCQM PatientDataSectionCodes_CategoryOne(long providerId, string from, string to,
            string patientId = null, long reportType = 2, string cqmId = null, bool isC1 = false)
        {
            const int measurePart = 0;
            if (cqmId == "0421a")
            {
                cqmId = "0421";
                // measurePart = 1;
            }
            else if (cqmId == "0421b")
            {
                cqmId = "0421";
                // measurePart = 2;
            }

            var obj = new BLLCQM().Load_CQM_Codes(providerId, from, to, patientId, reportType, cqmId, measurePart, isC1
            );
            _dsPatientDataSectionCodes = obj.Data;
            return _dsPatientDataSectionCodes;
        }

        #endregion

        #endregion

        #region Implemenatation for MeasureSection, ReportingParameterSection and PatientDataSection

        #region MeasureSection

        private void MeasureSection(string cqmid)
        {
            if (cqmid == "0421")
                cqmid = "0421a";
            DataView dvMeasureSection = new DataView(_dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName])
            {
                RowFilter = "[CQMID] = '" + cqmid + "'"
            };
            DataTable dtMeasureSection = dvMeasureSection.ToTable();

            if (dtMeasureSection.Rows.Count > 0)
            {
                #region Header Fixed

                var versionNeutralidentifier = dtMeasureSection.Rows[0]["eMeasureIdentifier"].ToString() == "" ? dtMeasureSection.Rows[0]["eMeasureIdentifier(MAT)"].ToString() : dtMeasureSection.Rows[0]["eMeasureIdentifier"].ToString(); // Guid.NewGuid().ToString();
                var objComponent3 = new Component3
                {
                    section = new Section()
                    {
                        classCode = "DOCSECT",
                        moodCode = "EVN",
                        templateId = new List<II>
                        {
                            new II
                            {
                                root = "2.16.840.1.113883.10.20.24.2.2",
                            },
                            new II
                            {
                                root = "2.16.840.1.113883.10.20.24.2.3"
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
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.98"
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.97"
                        }
                    },
                    id = new List<II>
                    {
                        new II
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = dtMeasureSection.Rows[0]["VersionSpecificIdentifier"].ToString()
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
                                    new II
                                    {
                                        root = "2.16.840.1.113883.4.738",
                                        extension = dtMeasureSection.Rows[0]["VersionSpecificIdentifier"].ToString()
                                    },
                                    //new II()
                                    //{
                                    //    root = "2.16.840.1.113883.3.560.1",
                                    //    extension = dtMeasureSection.Rows[0]["CQMID"].ToString()
                                    //},
                                    //new II()
                                    //{
                                    //    root = "2.16.840.1.113883.3.560.101.2",
                                    //    extension = dtMeasureSection.Rows[0]["eMeasureIdentifier"].ToString()
                                    //}
                                },
                                text = new ED
                                {
                                    Text = new List<string>
                                    {
                                        dtMeasureSection.Rows[0]["Measure"].ToString()
                                    }
                                },
                                setId = new II
                                {
                                    root = versionNeutralidentifier
                                },
                                versionNumber = new INT
                                {
                                    value = "5" //dtMeasureSection.Rows[0]["eMeasureVersionNumber"].ToString()
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
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.17.2.1",
                            extension = "2015-07-01"
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
                        //mediaType = "text/x-hl7-text+xml",
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
                                            "Reporting period: " + DateTime.Parse(startDate).ToString("yyyyMMdd") + " - " + DateTime.Parse(endDate).ToString("yyyyMMdd")
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
                                    },
                                    new II
                                    {
                                        root = "2.16.840.1.113883.10.20.17.3.8",
                                        extension = "2015-07-01"
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
                                            value = DateTime.Parse(startDate).ToString("yyyyMMdd")
                                        },
                                        new IVXB_TS
                                        {
                                            value = DateTime.Parse(endDate).ToString("yyyyMMdd")
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

        private static StrucDocTr GetPatientDataSection_TRs(string dataElement, string val, string dateTime)
        {
            var bodyObjStrucDocTr = new StrucDocTr
            {
                Items = new List<object>
                {
                    new StrucDocTd() {Text = new List<string> {dataElement}},
                    new StrucDocTd() {Text = new List<string> {val}},
                    new StrucDocTd() {Text = new List<string> {dateTime}}
                }
            };
            return bodyObjStrucDocTr;
        }
        private void PatientDataSection(string cqmid)
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
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.2.1",
                            extension = "2016-02-01"
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.2.1",
                            extension = "2015-07-01"
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
                        Text = new List<string> { "Patient Data" }
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
                    new StrucDocTh {Text = new List<string> {"Data Element"}},
                    new StrucDocTh {Text = new List<string> {"Value"}},
                    new StrucDocTh {Text = new List<string> {"Date/Time"}}
                }
            };
            strucDocTable.thead.tr.Add(strucDocTr);
            strucDocTable.tbody = new List<StrucDocTbody>();

            var objStrucDocTbody = new StrucDocTbody();
            strucDocTable.tbody.Add(objStrucDocTbody);

            #endregion

            SetComponenets(objStrucDocTbody, objComponent3, cqmid);
            StructuredBody.component.Add(objComponent3);
        }

        private static void SetComponenets(StrucDocTbody objStrucDocTbody, Component3 objComponent3, string cqmid)
        {
            objStrucDocTbody.tr = new List<StrucDocTr>();
            objComponent3.section.entry = new List<Entry>();

            if (cqmid == "0421a" || cqmid == "0421b")
            {
                cqmid = "0421";
                CQMID = "0421";
            }

            if (cqmid == "0712(A)" || cqmid == "0712(B)" || cqmid == "0712(C)")
            {
                cqmid = "0712";
                CQMID = "0712";
            }
            if (cqmid == "0022(A)" || cqmid == "0022(B)")
            {
                cqmid = "0022";
                CQMID = "0022";
            }
            if (cqmid == "0075(A)" || cqmid == "0075(B)")
            {
                cqmid = "0075";
                CQMID = "0075";
            }

            if (cqmid == "0022")
            {
                CQMID = "0022";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0018")
            {
                CQMID = "0018";
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_OnlySNOMED(objStrucDocTbody, objComponent3);
                PyhsicalExam(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0028")
            {
                CQMID = "0028";
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
                MedicationActive(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                PatientCharacteristics(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0075")
            {
                CQMID = "0075";
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                LabOrder(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0419")
            {
                CQMID = "0419";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0421")
            {
                CQMID = "0421";
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
                InterventionActive(objStrucDocTbody, objComponent3);
                PhysicalExam_421_New1(objStrucDocTbody, objComponent3);


            }
            else if (cqmid == "0043")
            {
                CQMID = "0043";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                MedicationAdministered_CVX(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0068")
            {
                CQMID = "0068";

                EncounterPerformed_OnlyCPT(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                MedicationActive(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0418")
            {
                CQMID = "0418";
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                MedicationActive(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);
                InterventionActive(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0059")
            {
                CQMID = "0059";
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                LabOrder(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0041")
            {
                CQMID = "0041";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                MedicationAdministered_CVX(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                PatientToProvider(objStrucDocTbody, objComponent3);
                ImmunizationAllergyandIntolerance(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "CMS50v3")
            {
                CQMID = "CMS50v3";
                EncounterPerformed_OnlyCPT(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);
                ProviderToProvider(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0031")
            {
                CQMID = "0031";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                DiagnosticStudy(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0056")
            {
                CQMID = "0056";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                PyhsicalExam(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0034")
            {
                CQMID = "0034";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                LabOrder(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0062")
            {
                CQMID = "0062";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);
                MedicationActive(objStrucDocTbody, objComponent3);
                LabOrder(objStrucDocTbody, objComponent3);
                PatientCharacteristics_New(objStrucDocTbody, objComponent3);

            }
            else if (cqmid == "0712")
            {
                CQMID = "0712";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);
                MedicationActive(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                InterventionActive(objStrucDocTbody, objComponent3);
                PatientCharacteristics_New(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0101")
            {
                CQMID = "0101";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0065")
            {
                CQMID = "0065";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                PyhsicalExam(objStrucDocTbody, objComponent3);
                setPhysicalExam0065(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "2872")
            {
                CQMID = "2872";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "CMS22v5")
            {
                CQMID = "CMS22v5";
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                //PyhsicalExam(objStrucDocTbody, objComponent3);
                setPhysicalExam222(objStrucDocTbody, objComponent3);
                DiagnosticStudy(objStrucDocTbody, objComponent3);
                LabOrder(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
                InterventionActive(objStrucDocTbody, objComponent3);
            }
            else
            {
                //DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                //FamilyHx(objStrucDocTbody, objComponent3);
                //MedicationActive(objStrucDocTbody, objComponent3);
                //MedicationAdministered(objStrucDocTbody, objComponent3);
                //MedicationAllergy(objStrucDocTbody, objComponent3);
                //MedicationOrder(objStrucDocTbody, objComponent3);
                //ProcedureOrder(objStrucDocTbody, objComponent3);
                //EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                //ProcedurePerformed(objStrucDocTbody, objComponent3);
                //RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                //TobbacoUser(objStrucDocTbody, objComponent3);
            }
        }

        #region PatientDataSection SetComponenets
        private static void DiagnosisActiveConcernAct(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes == null)
                return;

            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var problemCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Problem"
                        &&
                        (r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "SNOMED"
                        ||
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "ICD9"
                         ||
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "ICD10")
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
                        "Po1");

            if (problemCodesRowCollection.Any())
            {
                var dtProblemCode = problemCodesRowCollection.CopyToDataTable();

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ICD"));
                dt.Columns.Add(new DataColumn("ICDDescription"));
                dt.Columns.Add(new DataColumn("Title"));
                dt.Columns.Add(new DataColumn("DataType"));
                dt.Columns.Add(new DataColumn("StartDate"));
                dt.Columns.Add(new DataColumn("EndDate"));
                dt.Columns.Add(new DataColumn("valueset"));
                dt.Columns.Add(new DataColumn("Condition"));

                bool rowdeleted = false;




                var dvProblemCodes = new DataView(dtProblemCode);
                var dtProblemCodes = dvProblemCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtProblemCodes.Rows.Count; i++)
                {
                    var diagnosisStartDate =
                        (string)dtProblemCodes.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtProblemCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Diagnosis Active: " +
                                    dtProblemCodes.Rows[i]["ICDDescription"],
                                    dtProblemCodes.Rows[i]["ICDDescription"].ToString(),
                                    diagnosisStartDate
                                )
                        );

                    string TargetSideCode = "";
                    string TragetSideDisplayName = "";
                    string TragetSideValueSet = "";
                    if (dtProblemCodes.Rows.Count > i - 1 && (dtProblemCodes.Rows[i][1].ToString() == "Right" || dtProblemCodes.Rows[i][1].ToString() == "Left"))
                    {

                        TargetSideCode = dtProblemCodes.Rows[i][0].ToString();
                        TragetSideDisplayName = dtProblemCodes.Rows[i][1].ToString();
                        TragetSideValueSet = dtProblemCodes.Rows[i][6].ToString();


                        if (rowdeleted == true)
                        {
                            objComponent3.section.entry.Add(SetDiagnosisActiveConcernAct(dt.Rows[0], TargetSideCode, TragetSideDisplayName, TragetSideValueSet));
                        }
                        else
                        {
                            objComponent3.section.entry.Add(SetDiagnosisActiveConcernAct(dtProblemCodes.Rows[i - 1], TargetSideCode, TragetSideDisplayName, TragetSideValueSet));
                            DataRow dr = dt.NewRow();
                            dr["ICD"] = dtProblemCodes.Rows[i - 1][0];
                            dr["ICDDescription"] = dtProblemCodes.Rows[i - 1][1];
                            dr["Title"] = dtProblemCodes.Rows[i - 1][2];
                            dr["DataType"] = dtProblemCodes.Rows[i - 1][3];
                            dr["StartDate"] = dtProblemCodes.Rows[i - 1][4];
                            dr["EndDate"] = dtProblemCodes.Rows[i - 1][5];
                            dr["valueset"] = dtProblemCodes.Rows[i - 1][6];
                            dr["Condition"] = dtProblemCodes.Rows[i - 1][7];
                            dt.Rows.Add(dr);
                            //dtProblemCodes.Rows[i].Delete();
                            rowdeleted = true;
                        }
                    }
                    else
                    {
                        objComponent3.section.entry.Add(SetDiagnosisActiveConcernAct(dtProblemCodes.Rows[i], TargetSideCode, TragetSideDisplayName, TragetSideValueSet));
                    }

                }
            }
        }
        private static void DiagnosisActiveConcernActPatch(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.DiagnosisActiveConcernAct.TableName;

            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var onSetDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.DiagnosisActiveConcernAct.StartDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.DiagnosisActiveConcernAct.StartDateColumn].ToString()).ToString("yyyyMMdd");
                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Diagnosis Active: ",
                                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.DiagnosisActiveConcernAct.SNOMED_DESCRIPTIONColumn].ToString(),
                                onSetDate
                            )
                    );

                objComponent3.section.entry.Add(SetDiagnosisActiveConcernActPatch(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void FamilyHx(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows.Count; i++)
            {
                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Diagnosis Family History: ",
                                _dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows[i][_dsPatientDataSection.FamilyHx.FamilyHx_FamilyMemberDescriptionColumn].ToString(),
                                DateTime.Parse(_dsPatientDataSection.Tables[_dsPatientDataSection.FamilyHx.TableName].Rows[i][_dsPatientDataSection.FamilyHx.CreatedOnColumn].ToString()).ToString("yyyyMMdd")
                            )
                    );
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
        //            objComponent3.section.entry.Add(SetMedicationActiveNew(_dsPatientDataSection.Tables[tableName].Rows[i]));
        //        }
        //    }
        //}

        private static void MedicationActive(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var medicationCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Drug"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "RxnormID"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Medication Active: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    medicationStartDate
                                )
                        );
                    if (dtProcedureCodes.Rows[i]["ICD"].ToString() != "")
                    {
                        objComponent3.section.entry.Add(SetMedicationActive(dtProcedureCodes.Rows[i]));
                    }

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

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Medication Administered: ",
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.MedicationAdministered.DrugDrugDescriptionColumn]
                                + " " +
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.MedicationAdministered.MedicationActiveColumn]
                                + " " +
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.MedicationAdministered.MedicationDoseColumn]
                                + " " +
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.MedicationAdministered.MedicationUnitColumn]
                                + " " +
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.MedicationAdministered.MedicationRouteByColumn]
                                + " " +
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.MedicationAdministered.MedicationDoseTimingColumn],
                                medicationStartDate
                            )
                    );

                objComponent3.section.entry.Add(
                    SetMedicationAdministered(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }

        private static void MedicationAdministered_CVX(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var medicationCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Vaccine" &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "CVX" &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Medication Administered: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    medicationStartDate
                                )
                        );
                    if (dtProcedureCodes.Rows[i]["ICD"].ToString() != "")
                    {
                        objComponent3.section.entry.Add(SetMedicationAdministered_CVX(dtProcedureCodes.Rows[i]));
                    }

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

            //    objComponent3.section.entry.Add(
            //        SetMedicationAdministered_CVX(_dsPatientDataSection.Tables[tableName].Rows[i]));
            //}
        }
        private static void ImmunizationAllergyandIntolerance(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.ProviderToProvider.TableName;

            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var administeredDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.AdministrationDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.AdministrationDateColumn].ToString()).ToString("yyyyMMdd");
                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Immunization Allergy: ",
                                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.CVXCodeColumn].ToString(),
                                administeredDate
                            )
                    );

                objComponent3.section.entry.Add(SetImmunizationIntolerance(_dsPatientDataSection.Tables[tableName].Rows[i]));


            }
        }
        private static void MedicationOrder(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {

            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var medicationOrderRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                    .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Drug"
                        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "RxnormID"
                        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            if (medicationOrderRowCollection.Any())
            {
                var dtMedicationOrder = medicationOrderRowCollection.CopyToDataTable();

                var dvMedicationOrder = new DataView(dtMedicationOrder);
                var dtMedicationOrderRe = dvMedicationOrder.ToTable(true, "PatientId", "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtMedicationOrderRe.Rows.Count; i++)
                {
                    var medicationStartDate = (string)dtMedicationOrderRe.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtMedicationOrderRe.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Medication Order: " +
                                    dtMedicationOrderRe.Rows[i]["ICDDescription"],
                                    dtMedicationOrderRe.Rows[i]["ICDDescription"].ToString(),
                                    medicationStartDate
                                )
                        );
                    if (CQMID == "CMS22v5")
                    {
                        objComponent3.section.entry.Add(
                       SetMedicationOrder22v5(dtMedicationOrderRe.Rows[i]));
                    }
                    else
                    {
                        objComponent3.section.entry.Add(
                            SetMedicationOrder(dtMedicationOrderRe.Rows[i]));
                    }
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

            //    objComponent3.section.entry.Add(SetMedicationOrder(_dsPatientDataSection.Tables[tableName].Rows[i]));
            //}
        }

        private static void EncounterPerformed_Any(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes == null)
                return;
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable().
                Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Encounter");

            if (proceduresCodesRowCollection.Any())
            {
                var dtProcedureCode = proceduresCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset");
                //Dictionary<string, List<string>> myDict;
                var myDict = new Dictionary<string, string>();
                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Encounter Performed: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    //string val = dtProcedureCodes.Rows[i]["valueset"].ToString() + dtProcedureCodes.Rows[i]["StartDate"].ToString();
                    //if (!(myDict.ContainsKey(dtProcedureCodes.Rows[i]["ICD"].ToString()) &&
                    //    myDict[dtProcedureCodes.Rows[i]["ICD"].ToString()].Equals(val))) 
                    //{
                    //    myDict.Add(dtProcedureCodes.Rows[i]["ICD"].ToString(), val);
                    if (dtProcedureCodes.Rows[i]["ICD"].ToString().IndexOf('|') > -1)
                    {
                        objComponent3.section.entry.Add(SetEncounterPerformed160(dtProcedureCodes.Rows[i]));
                    }
                    else
                    {
                        objComponent3.section.entry.Add(SetEncounterPerformed(dtProcedureCodes.Rows[i]));
                    }


                }
            }
        }

        private static void EncounterPerformed_OnlyCPT(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Encounter"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "CPTCode");
            //&& r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1"

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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Encounter Performed: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    objComponent3.section.entry.Add(
                        SetEncounterPerformed(dtProcedureCodes.Rows[i]));
                }
            }
        }

        private static void EncounterPerformed_OnlySNOMED(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.EncounterPerformed.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Encounter"
            && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "CPTCode"
            && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Encounter Performed: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    objComponent3.section.entry.Add(
                        SetEncounterPerformed(dtProcedureCodes.Rows[i]));
                }
            }
        }

        private static void ProcedurePerformed(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable().
                Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure");

            if (proceduresCodesRowCollection.Any())
            {
                var dtProcedureCode = proceduresCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Procedure Performed: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    if (CQMID == "0041")
                    {

                        if (dtProcedureCodes.Rows[i]["ICD"].ToString() != "" && (dtProcedureCodes.Rows[i]["Condition"].ToString().IndexOf("Intolerance:") < 0))
                        {
                            objComponent3.section.entry.Add(
                            SetProcedurePerformed(dtProcedureCodes.Rows[i]));
                        }

                        string[] intoleranceCodes = { "86198006", "G0008", "Q2034", "Q2035", "Q2036", "Q2037", "Q2038", "Q2039", "90630", "90653", "90654", "90655",
                            "90656", "90657","90658","90660","90661","90662","90664","90666","90667","90668","90672","90673","90685","90686","90687","90688" };

                        bool isCodeFound = Array.IndexOf(intoleranceCodes, dtProcedureCodes.Rows[i]["ICD"].ToString()) > -1;

                        if (dtProcedureCodes.Rows[i]["ICD"].ToString() != "" && isCodeFound == true && dtProcedureCodes.Rows[i]["Condition"].ToString().IndexOf("not done:") < 0 && dtProcedureCodes.Rows[i]["Condition"].ToString().IndexOf("Intolerance:") > 0)
                        {
                            objComponent3.section.entry.Add(
                            SetProcedurePerformed0041(dtProcedureCodes.Rows[i]));
                        }
                    }
                    else
                    {
                        if (dtProcedureCodes.Rows[i]["ICD"].ToString() != "")
                        {
                            objComponent3.section.entry.Add(
                            SetProcedurePerformed(dtProcedureCodes.Rows[i]));
                        }

                    }
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

            //    objComponent3.section.entry.Add(SetProcedurePerformed(_dsPatientDataSection.Tables[tableName].Rows[i]));
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

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Procedure Order: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedureOrder.ProceduresCPT_DESCRIPTIONColumn],
                                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProcedureOrder.ProceduresCPT_DESCRIPTIONColumn].ToString(),
                                procedureStartDate
                            )
                    );

                objComponent3.section.entry.Add(SetProcedureOrder(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }

        private static void RiskCategoryAssessment(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            //var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSectionCodes.Tables[
            //    _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
            //    .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "Loinc" &&
            //            r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Lab" &&
            //            r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSectionCodes.Tables[
               _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
               .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "RiskAssessment" &&
                       r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Risk Category Assessment: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    string ResultSnomed = "";
                    string ResultValueSet = "";
                    var LOINCResultValue = "";
                    if (dtProcedureCodes.Rows[i]["ICD"].ToString().IndexOf('|') > -1)
                    {
                        LOINCResultValue = dtProcedureCodes.Rows[i]["ICD"].ToString().Split('|')[1];
                    }
                    if (LOINCResultValue != "")
                    {

                        objComponent3.section.entry.Add(
                           SetRiskCategoryAssessment160(dtProcedureCodes.Rows[i], LOINCResultValue));
                    }
                    else
                    {
                        //if (dtProcedureCodes.Rows.Count > 1 && dtProcedureCodes.Rows[0][0].ToString().Contains("-") && dtProcedureCodes.Rows[0][0].ToString() == dtProcedureCodes.Rows[1][1].ToString() && dtProcedureCodes.Rows[1][2].ToString().ToLower() == "snomed")
                        if (dtProcedureCodes.Rows.Count > 1 && dtProcedureCodes.Rows[0][0].ToString().Contains("-") && dtProcedureCodes.Rows[1][2].ToString().ToLower() == "snomed")
                        {
                            ResultSnomed = dtProcedureCodes.Rows[1][0].ToString();
                            ResultValueSet = dtProcedureCodes.Rows[1][6].ToString();
                            objComponent3.section.entry.Add(
                            SetRiskCategoryAssessment(dtProcedureCodes.Rows[i], ResultSnomed, ResultValueSet));
                            dtProcedureCodes.Rows[1].Delete();
                        }
                        else
                        {
                            objComponent3.section.entry.Add(
                            SetRiskCategoryAssessment(dtProcedureCodes.Rows[i], ResultSnomed, ResultValueSet));
                        }
                    }
                }
            }
        }
        private static void RiskCategoryAssessmentPatch(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.RiskCatagoryAssesment.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            #region Commented Yet

            //var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
            //        .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "LOINC"
            //        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            ////r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
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
            //        objComponent3.section.entry.Add(
            //            SetRiskCategoryAssessment(dtProcedureCodes.Rows[i]));
            //    }
            //}

            #endregion

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var riskCatagoryAssesmentStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.OrderDateColumn].ToString() == "" ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.OrderDateColumn].ToString()).ToString("yyyyMMdd");

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Risk Category Assessment: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.LOINCDescriptionColumn],
                                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.RiskCatagoryAssesment.LOINCDescriptionColumn].ToString(),
                                riskCatagoryAssesmentStartDate
                            )
                    );

                objComponent3.section.entry.Add(SetRiskCategoryAssessmentPatch(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }

        }
        private static void TobbacoUserPatch(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.TobbacoUser.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            #region Commented

            //var tobbacoUserCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
            //        .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
            //        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            ////r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
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
            //        objComponent3.section.entry.Add(
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

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Patient Characteristic: " +
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn],
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn].ToString(),
                                tobbacoUserStartDate
                            )
                    );

                objComponent3.section.entry.Add(SetTobbacoUser(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }
        private static void TobbacoUser(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.TobbacoUser.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            #region Commented

            //var tobbacoUserCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
            //        .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
            //        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            ////r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
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
            //        objComponent3.section.entry.Add(
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

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Patient Characteristic: " +
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn],
                                _dsPatientDataSection.Tables[tableName].Rows[i][
                                    _dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn].ToString(),
                                tobbacoUserStartDate
                            )
                    );

                objComponent3.section.entry.Add(SetTobbacoUser(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }


        private static void PatientCharacteristics(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {

            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;
            var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSectionCodes.Tables[
               _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
               .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Tobacco" &&
                       r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Risk Category Assessment: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    objComponent3.section.entry.Add(
                        SetPatientCharacteristics(dtProcedureCodes.Rows[i]));
                }
            }

            //var tableName = _dsPatientDataSection.TobbacoUser.TableName;
            //if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;


            //for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            //{
            //    var tobbacoUserStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.TobbacoUser.SocialHxDateColumn].ToString() == ""
            //            ? DateTime.Now.ToString("yyyyMMdd") : DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.TobbacoUser.SocialHxDateColumn].ToString()).ToString("yyyyMMdd");

            //    objStrucDocTbody.tr.Add
            //        (
            //            GetPatientDataSection_TRs
            //                (
            //                    "Patient Characteristic: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn],
            //                    _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.TobbacoUser.SNOMEDCTDescriptionColumn].ToString(),
            //                    tobbacoUserStartDate
            //                )
            //        );

            //    objComponent3.section.entry.Add(SetPatientCharacteristics(_dsPatientDataSection.Tables[tableName].Rows[i]));
            //}
        }

        private static void PatientCharacteristics_New(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {

            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;
            var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSectionCodes.Tables[
               _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
               .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Tobacco" &&
                       r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Risk Category Assessment: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    objComponent3.section.entry.Add(
                        SetPatientCharacteristics_New(dtProcedureCodes.Rows[i]));
                }
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
            //        objComponent3.section.entry.Add(SetPhysicalExam(dtProcedureCodes.Rows[i]));
            //    }
            //}

            #endregion

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var physicalExamStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString() == ""
                    ? "" :
                    DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString()).ToString("yyyyMMdd");

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Physical Exam, Finding: Diastolic Blood Pressure",
                                "Diastolic Blood Pressure",
                                physicalExamStartDate
                            )
                    );

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Physical Exam: Systolic Blood Pressure",
                                "Systolic Blood Pressure",
                                physicalExamStartDate
                            )
                    );

                var systolicBpLoincCode = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.LOINICSystolicColumn].ToString();
                var diastolicBpLoincCode = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.LOINICDiastolicColumn].ToString();

                var systolicBpValue = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.SystolicColumn].ToString();
                var diastolicBpValue = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.DiastolicColumn].ToString();
                if (diastolicBpValue != "")
                {
                    objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Diastolic Blood Pressure", "2.16.840.1.113883.3.526.3.1033", diastolicBpLoincCode, diastolicBpValue));

                }
                if (systolicBpValue != "")
                {
                    objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Systolic Blood Pressure", "2.16.840.1.113883.3.526.3.1032", systolicBpLoincCode, systolicBpValue));

                }
            }
        }

        private static void setPhysicalExam222(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var pyhsicalExamRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                    .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "LOINC"
                    && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            if (pyhsicalExamRowCollection.Any())
            {
                var dtProcedureCode = pyhsicalExamRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable();

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Physical Exam, Finding: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );

                    if (dtProcedureCodes.Rows[i]["ICD"].ToString().IndexOf('|') > -1 && dtProcedureCodes.Rows[i]["ICD"].ToString().Split('|')[1] != "" && dtProcedureCodes.Rows[i]["ICD"].ToString().Split('|')[1] != "0")
                    {
                        objComponent3.section.entry.Add(SetPhysicalExamCMS22(dtProcedureCodes.Rows[i], " Delta systolic blood pressure", "2.16.840.1.113883.3.464.1003.121.12.1013", "", ""));
                    }
                }
            }

        }

        private static void setPhysicalExam0065(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var pyhsicalExamRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                    .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "LOINC"
                    && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

            if (pyhsicalExamRowCollection.Any())
            {
                var dtProcedureCode = pyhsicalExamRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(); //(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                            ? DateTime.Now.ToString("yyyyMMdd")
                            : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Physical Exam, Finding: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    var ICDColumn = dtProcedureCodes.Rows[i]["ICD"].ToString();
                    if (ICDColumn.IndexOf('|') > -1 && dtProcedureCodes.Rows[i]["ICD"].ToString().Split('|')[0] == "71789-2")
                    {
                        var systolicLOINC = dtProcedureCodes.Rows[i]["ICD"].ToString().Split('|')[0];
                        var startdate = dtProcedureCodes.Rows[i]["StartDate"].ToString();
                        var enddate = dtProcedureCodes.Rows[i]["EndDate"].ToString();
                        var valueset = dtProcedureCodes.Rows[i]["valueset"].ToString();
                        var systolicvalue = dtProcedureCodes.Rows[i]["ICD"].ToString().Split('|')[1];

                        objComponent3.section.entry.Add(SetPhysicalExam(dtProcedureCodes.Rows[i], " Delta systolic blood pressure", "2.16.840.1.113883.3.464.1003.121.12.1013", systolicLOINC, systolicvalue));
                    }
                }
            }
        }

        private static void PhysicalExam_421_New1(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {


            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count > 0)
            {

                var tableName = _dsPatientDataSection.PhysicalExam.TableName;
                if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;


                for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
                {
                    var physicalExamStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString() == ""
                        ? "" :
                        DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.VitalSignDateColumn].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Physical Exam, Finding: Diastolic Blood Pressure",
                                    "Diastolic Blood Pressure",
                                    physicalExamStartDate
                                )
                        );

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Physical Exam: Systolic Blood Pressure",
                                    "Systolic Blood Pressure",
                                    physicalExamStartDate
                                )
                        );

                    var bmiValue = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.BMIColumn].ToString();
                    var bmiValueSet = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.BMIValueSetColumn].ToString();
                    var negationValue = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.ActionPerformedColumn].ToString();
                    var negationValueSet = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.BMIValueSetColumn].ToString();


                    if (negationValue != "")
                    {
                        objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Body mass index", "2.16.840.1.113883.3.600.1.681", "39156-5", bmiValue, negationValue, negationValueSet));
                    }
                    else
                    {
                        if (bmiValue != "")
                        {
                            objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Body mass index", "2.16.840.1.113883.3.600.1.681", "39156-5", bmiValue, "", negationValueSet));
                        }

                    }

                }
            }
        }

        private static void LabOrder(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var labOrderRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName]
                .AsEnumerable()
                .Where(
                    r =>
                        (r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Lab"
                        ||
                         r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Lab Result"
                        ||
                         r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Lab Test")
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()).ToUpper() ==
                        "LOINC"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
                        "Po1");

            if (labOrderRowCollection.Any())
            {
                var dtLabOrder = labOrderRowCollection.CopyToDataTable();

                var dvLabOrder = new DataView(dtLabOrder);
                var dtLabOrderRe = dvLabOrder.ToTable(true, "PatientId", "ICD", "ICDDescription", "Title", "DataType", "StartDate",
                    "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtLabOrderRe.Rows.Count; i++)
                {
                    var medicationStartDate = (string)dtLabOrderRe.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtLabOrderRe.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Laboratory Test, Result: " +
                                    dtLabOrderRe.Rows[i]["ICDDescription"],
                                    dtLabOrderRe.Rows[i]["ICDDescription"].ToString(),
                                    medicationStartDate
                                )
                        );
                    objComponent3.section.entry.Add(
                        SetLabOrder(dtLabOrderRe.Rows[i]));
                }
            }
        }
        private static void DiagnosticStudy(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var diagnosticStudyRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName]
                .AsEnumerable()
                .Where(
                    r =>
                        (r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Radiology"
                        ||
                         r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Radiology Result"
                        ||
                         r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Radiology Test")
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "LOINC"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
                        "Po1");

            if (diagnosticStudyRowCollection.Any())
            {
                var dtLabOrder = diagnosticStudyRowCollection.CopyToDataTable();

                var dvLabOrder = new DataView(dtLabOrder);
                var dtLabOrderRe = dvLabOrder.ToTable(true, "PatientId", "ICD", "ICDDescription", "Title", "DataType", "StartDate",
                    "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtLabOrderRe.Rows.Count; i++)
                {
                    var medicationStartDate = (string)dtLabOrderRe.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtLabOrderRe.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Radiology Test, Result: " +
                                    dtLabOrderRe.Rows[i]["ICDDescription"],
                                    dtLabOrderRe.Rows[i]["ICDDescription"].ToString(),
                                    medicationStartDate
                                )
                        );
                    objComponent3.section.entry.Add(
                        SetDiagnosticStudy(dtLabOrderRe.Rows[i]));
                }
            }
        }

        private static void InterventionPerformed(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSectionCodes.CQMCodes.TableName;
            if (_dsPatientDataSectionCodes.Tables[tableName].Rows.Count <= 0) return;

            var interventionPerformedRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName]
                .AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Intervention"
                        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
                        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Laboratory Test, Result: " +
                                    dtLabOrderRe.Rows[i]["ICDDescription"],
                                    dtLabOrderRe.Rows[i]["ICDDescription"].ToString(),
                                    medicationStartDate
                                )
                        );
                    objComponent3.section.entry.Add(
                        SetInterventionPerformed(dtLabOrderRe.Rows[i]));
                }
            }
        }

        private static void InterventionActive(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSectionCodes.CQMCodes.TableName;
            if (_dsPatientDataSectionCodes.Tables[tableName].Rows.Count <= 0) return;

            var interventionPerformedRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName]
                .AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Intervention"
                        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
                        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Laboratory Test, Result: " +
                                    dtLabOrderRe.Rows[i]["ICDDescription"],
                                    dtLabOrderRe.Rows[i]["ICDDescription"].ToString(),
                                    medicationStartDate
                                )
                        );


                    objComponent3.section.entry.Add(
                        SetInterventionActive(dtLabOrderRe.Rows[i]));

                }
            }
        }

        private static void ProviderToProvider(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable().
                Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.ConditionColumn.ColumnName.ToString()).IndexOf("From Provider to Provider:") > -1);

            if (proceduresCodesRowCollection.Any())
            {
                var dtProcedureCode = proceduresCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Communication: From Provider To Provider: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    if (dtProcedureCodes.Rows[i]["ICD"].ToString() != "")
                    {
                        objComponent3.section.entry.Add(
                        SetProviderToProvider(dtProcedureCodes.Rows[i]));
                    }

                }
            }
        }

        private static void PatientToProvider(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable().
                Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.ConditionColumn.ColumnName.ToString()).IndexOf("From Patient to Provider:") > -1);

            if (proceduresCodesRowCollection.Any())
            {
                var dtProcedureCode = proceduresCodesRowCollection.CopyToDataTable();

                var dvProcedureCodes = new DataView(dtProcedureCode);
                var dtProcedureCodes = dvProcedureCodes.ToTable(true, "ICD", "ICDDescription", "Title", "DataType",
                    "StartDate", "EndDate", "valueset", "Condition");

                for (var i = 0; i < dtProcedureCodes.Rows.Count; i++)
                {
                    var procedureStartDate = (string)dtProcedureCodes.Rows[i]["StartDate"] == ""
                        ? DateTime.Now.ToString("yyyyMMdd")
                        : DateTime.Parse(dtProcedureCodes.Rows[i]["StartDate"].ToString()).ToString("yyyyMMdd");

                    objStrucDocTbody.tr.Add
                        (
                            GetPatientDataSection_TRs
                                (
                                    "Communication: From Patient To Provider: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
                    if (dtProcedureCodes.Rows[i]["ICD"].ToString() != "")
                    {
                        objComponent3.section.entry.Add(
                        SetPatientToProvider(dtProcedureCodes.Rows[i]));
                    }

                }
            }
        }

        #endregion

        #region Patient Data Section Set'em

        #region Diagnosis Active Concern Act
        private static Entry SetDiagnosisActiveConcernActOld(DataRow drDiagnosisActiveConcernAct)
        {
            var codeValue = drDiagnosisActiveConcernAct["ICD"].ToString();
            var diseaseName = drDiagnosisActiveConcernAct["ICDDescription"].ToString();
            var codeSystem = drDiagnosisActiveConcernAct["Title"].ToString();
            var valueset = drDiagnosisActiveConcernAct["valueset"].ToString();

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
                                    codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                    :codeSystem.Contains("SNOMED")
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

        private static Entry SetDiagnosisActiveConcernAct(DataRow drDiagnosisActiveConcernAct, string TargetSideCode = "", string TragetSideDisplayName = "", string TragetSideValueSet = "")
        {
            var codeValue = drDiagnosisActiveConcernAct["ICD"].ToString();
            var diseaseName = drDiagnosisActiveConcernAct["ICDDescription"].ToString();
            var codeSystem = drDiagnosisActiveConcernAct["Title"].ToString();
            var valueset = drDiagnosisActiveConcernAct["valueset"].ToString();
            var actionPerformed = drDiagnosisActiveConcernAct["ICD"].ToString();

            if (TragetSideDisplayName == "Left" || TragetSideDisplayName == "Right")
            {
                Entry entryDiagnosisActiveConcernAct = new Entry()
                {
                    Item = new Act()
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.EVN,
                        templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.22.4.3",
                            extension = "2015-08-01"
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.137"
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
                            code = "CONC",
                            displayName = "Concern",
                            codeSystem = "2.16.840.1.113883.5.6"
                        },
                        statusCode = new CS()
                        {
                            code = "active"
                        },
                        effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["StartDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["EndDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["EndDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                        },
                        entryRelationship = new List<EntryRelationship>()
                    {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.4",
                                        extension = "2015-08-01"
                                    },
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.135"
                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "29308-4",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "status",
                                    translation= new List<CD>
                                    {
                                        new CD
                                        {
                                            code = "282291009",
                                            codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96"
                                        }
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
                                            value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["StartDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        },
                                        new IVXB_TS
                                        {
                                            value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["EndDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["EndDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        }
                                    }
                                },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = codeValue,
                                        //displayName = diseaseName,
                                        codeSystem = codeSystem.Contains("SNOMED") ? "2.16.840.1.113883.6.96" : codeSystem.Contains("CPT") ? "2.16.840.1.113883.6.12" : codeSystem.Contains("ICD9") ? "2.16.840.1.113883.6.103" : codeSystem.Contains("Loinc") ? "2.16.840.1.113883.6.1" : codeSystem.Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                                        codeSystemName = codeSystem,
                                        originalText = new ED() {Text = new List<string>() {"Diagnosis, Active: " + diseaseName}},
                                        //translation = new List<CD>()
                                        //{
                                        //    new CD()
                                        //    {
                                        //        code = codeValue,
                                        //        codeSystem = codeSystem.Contains("SNOMED") ? "2.16.840.1.113883.6.96" : codeSystem.Contains("CPT") ? "2.16.840.1.113883.6.12" : codeSystem.Contains("ICD9") ? "2.16.840.1.113883.6.103" : codeSystem.Contains("LOINC") ? "2.16.840.1.113883.6.1" : codeSystem.Contains("ICD10") ? "2.16.840.1.113883.6.90" : ""
                                        //    }
                                        //},
                                        valueSet = valueset.Trim()
                                    }
                                },
                                targetSiteCode = new List<CD>(){

                                    new CD(){
                                        code = TargetSideCode,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        displayName = TragetSideDisplayName,
                                        valueSet = TragetSideValueSet
                                }
                              },
                            }
                        }
                    }
                    }
                };
                return entryDiagnosisActiveConcernAct;
            }
            else
            {
                Entry entryDiagnosisActiveConcernAct = new Entry()
                {
                    Item = new Act()
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.EVN,
                        templateId = new List<II>
                    {
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.22.4.3",
                            extension = "2015-08-01"
                        },
                        new II
                        {
                            root = "2.16.840.1.113883.10.20.24.3.137"
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
                            code = "CONC",
                            displayName = "Concern",
                            codeSystem = "2.16.840.1.113883.5.6"
                        },
                        statusCode = new CS()
                        {
                            code = "active"
                        },
                        effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[] { ItemsChoiceType2.low, ItemsChoiceType2.high },
                            Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["StartDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["EndDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["EndDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                        },
                        entryRelationship = new List<EntryRelationship>()
                    {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.22.4.4",
                                        extension = "2015-08-01"
                                    },
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.135"
                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "29308-4",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "status",
                                    translation= new List<CD>
                                    {
                                        new CD
                                        {
                                            code = "282291009",
                                            codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96"
                                        }
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
                                            value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["StartDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        },
                                        new IVXB_TS
                                        {
                                            value = string.IsNullOrEmpty(drDiagnosisActiveConcernAct["EndDate"].ToString()) ? "" : DateTime.Parse(drDiagnosisActiveConcernAct["EndDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        }
                                    }
                                },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = codeValue,
                                        //displayName = diseaseName,
                                        codeSystem = codeSystem.Contains("SNOMED") ? "2.16.840.1.113883.6.96" : codeSystem.Contains("CPT") ? "2.16.840.1.113883.6.12" : codeSystem.Contains("ICD9") ? "2.16.840.1.113883.6.103" : codeSystem.Contains("Loinc") ? "2.16.840.1.113883.6.1" : codeSystem.Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                                        codeSystemName = codeSystem,
                                        originalText = new ED() {Text = new List<string>() {"Diagnosis, Active: " + diseaseName}},
                                        //translation = new List<CD>()
                                        //{
                                        //    new CD()
                                        //    {
                                        //        code = codeValue,
                                        //        codeSystem = codeSystem.Contains("SNOMED") ? "2.16.840.1.113883.6.96" : codeSystem.Contains("CPT") ? "2.16.840.1.113883.6.12" : codeSystem.Contains("ICD9") ? "2.16.840.1.113883.6.103" : codeSystem.Contains("LOINC") ? "2.16.840.1.113883.6.1" : codeSystem.Contains("ICD10") ? "2.16.840.1.113883.6.90" : ""
                                        //    }
                                        //},
                                        valueSet = valueset.Trim()
                                    }
                                },

                            }
                        }
                    }
                    }
                };
                return entryDiagnosisActiveConcernAct;
            }
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
                                    codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96",
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
            var medicationActiveCodesRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                    .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Drug"
                        && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "RxnormID");

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
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.41",
                            extension = "2016-02-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                        ,new PIVL_TS()
                        {
                            @operator = SetOperator.A,
                            institutionSpecified1 = true,
                            period = new PQ()
                            {
                                value = "1", //string.IsNullOrEmpty(drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName].ToString()) ? "0": drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName].ToString(),
                                unit = "d" //string.IsNullOrEmpty(drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName].ToString()) ? "h" : drMedicationActive[_dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName].ToString()
                            }
                        }
                    },
                    doseQuantity = new IVL_PQ()
                    {
                        value = "1",
                        unit = "{tbl}"
                        // TEMP, If No Dose What to Set
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
                                    extension = "2014-06-09"
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
                            },
                            manufacturerOrganization = new Organization()
                            {
                                name = new List<ON>
                                {
                                    new ON
                                    {
                                        Text = new List<string>
                                        {
                                             "" //string.IsNullOrEmpty(drMedicationActive[_dsPatientDataSection.MedicationActive.DrugBrandNameColumn.ColumnName].ToString()) ? "" :drMedicationActive[_dsPatientDataSection.MedicationActive.DrugBrandNameColumn.ColumnName].ToString();
                                        }
                                    }
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
                            //                drMedicationActive[_dsPatientDataSection.MedicationActive.PharmacyPharmacyNameColumn.ColumnName].ToString() 
                            //                == "" ? "Rock Ridge Pharmacy" :drMedicationActive[_dsPatientDataSection.MedicationActive.PharmacyPharmacyNameColumn.ColumnName].ToString()
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    },
                    author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }
                                //code = new CE
                                //{
                                //    code = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderTaxonomyCodeColumn.ColumnName].ToString(),
                                //    codeSystem = "2.16.840.1.113883.6.101",
                                //    displayName = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderSpecialityDescriptionColumn.ColumnName].ToString(),
                                //    codeSystemName = "Healthcare Provider Taxonomy (HIPAA)"
                                //}
                            }
                        }
                    }
                }
            };
            return entryMedicationActive;
        }


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
                                        ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
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
        private static Entry SetMedicationAdministered_CVXOld(DataRow drMedicationAdministeredCvx)
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
                    negationInd = true,
                    templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.140",
                            extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115", extension= Guid.NewGuid().ToString()
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
                                        root = "2.16.840.1.113883.10.20.22.4.52",
                                        extension = "2014-06-09"
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
                                                root = "2.16.840.1.113883.10.20.22.4.54",
                                                extension = "2014-06-09"
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
                                                    nullFlavor = "NA",
                                                    valueSet = "2.16.840.1.113883.3.526.3.1254"
                                                }
                                        }
                                    }
                                }
                            }
                        }
                    },
                }
            };
            return entryMedicationAdministeredCvx;
        }

        private static Entry SetMedicationAdministered_CVX(DataRow drMedicationAdministeredCvx)
        {
            var codeValue = drMedicationAdministeredCvx["ICD"].ToString();
            var codeDescription = drMedicationAdministeredCvx["ICDDescription"].ToString();
            var codeSystem = drMedicationAdministeredCvx["Title"].ToString();
            var valueset = drMedicationAdministeredCvx["valueset"].ToString();
            var condition = drMedicationAdministeredCvx["Condition"].ToString();
            if (condition.IndexOf("not done:") > -1 && codeValue != "" && codeValue != null)
            {
                var entryMedicationAdministeredCvx = new Entry()
                {
                    Item = new Act()
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.EVN,
                        negationInd = true,
                        templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.140"
                            //extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                        id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS()
                            {
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
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
                                        root = "2.16.840.1.113883.10.20.22.4.52",
                                        extension = "2014-06-09"
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
                                        Items = new QTY[]
                                        {
                                            new IVXB_TS()
                                            {
                                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
                                            },
                                            new IVXB_TS()
                                            {
                                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")
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
                                                root = "2.16.840.1.113883.10.20.22.4.54",
                                                extension = "2014-06-09"
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
                                                     nullFlavor = "NA",
                                                    valueSet = "2.16.840.1.113883.3.526.3.1254"
                                            },
                                            lotNumberText = new ST
                                            {
                                                Text = new List<string> { "1" }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                              Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    displayName = "reason",
                                    codeSystemName = "LOINC"

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
                                value =DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                        }
                    },
                                 value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = codeValue,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet= valueset
                                    }
                                }
                            }
                        }
                    },
                    }
                };

                return entryMedicationAdministeredCvx;
            }
            else
            {
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
                            root = "2.16.840.1.113883.10.20.24.3.140"
                            //extension = DateTime.Now.ToString("yyyy-MM-dd")
                        }
                    },
                        id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS()
                            {
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
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
                                        root = "2.16.840.1.113883.10.20.22.4.52",
                                        extension = "2014-06-09"
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
                                        Items = new QTY[]
                                        {
                                            new IVXB_TS()
                                            {
                                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
                                            },
                                            new IVXB_TS()
                                            {
                                                value = string.IsNullOrEmpty( Convert.ToDateTime(drMedicationAdministeredCvx["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationAdministeredCvx["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")
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
                                                root = "2.16.840.1.113883.10.20.22.4.54",
                                                extension = "2014-06-09"
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
                                                    codeSystem = "2.16.840.1.113883.12.292",
                                                    valueSet = valueset,
                                                    originalText = new ED
                                                    {
                                                        Text = new List<string>
                                                        {
                                                            "Medication Administered: " + codeDescription
                                                        }
                                                    }
                                            },
                                            lotNumberText = new ST
                                            {
                                                Text = new List<string> { "1" }
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
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.44",
                            extension = "2016-02-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()
                        }
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
                            displayName = "Drug alle rgy"
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
                                        codeSystem = "2.16.840.1.113883.12.292",
                                        valueSet = "2.16.840.1.113883.3.526.3.1254"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return entryMedicationAllergy;
        }

        private static Entry SetImmunizationIntolerance(DataRow drMedicationAllergy)
        {

            var condition = drMedicationAllergy[_dsPatientDataSection.MedicationAdministered_CVX.AdministrationDateColumn.ColumnName].ToString();
            var secondRoot = "2.16.840.1.113883.10.20.24.3.44";
            var valueCode = "416098002";
            var valueDisplayName = "Drug allergy";

            if ((condition.ToLower()).IndexOf("intolerance") > -1)
            {
                secondRoot = "2.16.840.1.113883.10.20.24.3.46";
                valueCode = "59037007";
                valueDisplayName = "Drug intolerance";
            }

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
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = secondRoot,
                            extension = "2016-02-01"
                        }
                    },
                    id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()
                        }
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
                        ItemsElementName = new[] { ItemsChoiceType2.low },
                        Items = new QTY[]
                        {
                            //Note: If the allergy/intolerance is known to be resolved, but the date of resolution is not known, 
                            //then the high element SHALL be present, and the nullFlavor attribute SHALL be set to 'UNK'.
                            new IVXB_TS()
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        drMedicationAllergy[
                                            _dsPatientDataSection.MedicationAdministered_CVX.AdministrationDateColumn.ColumnName].ToString
                                            ())
                                        ? DateTime.Now.ToString("yyyyMMdd")
                                        : DateTime.Parse(
                                            drMedicationAllergy[
                                                _dsPatientDataSection.MedicationAdministered_CVX.AdministrationDateColumn.ColumnName]
                                                .ToString()).ToString("yyyyMMdd")
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = valueCode,
                            codeSystem = "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOMED CT",
                            displayName = valueDisplayName
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
                                                _dsPatientDataSection.MedicationAdministered_CVX.CVXCodeColumn.ColumnName]
                                                .ToString(), // rxNNorm Code will approach here
                                        codeSystem = "2.16.840.1.113883.12.292",
                                        valueSet = "2.16.840.1.113883.3.526.3.1254"
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
            var condition = drMedicationOrder["Condition"].ToString();
            string Reason = "";
            string ReasonComments = "";
            string ReasonValueset = "";

            var patientId = drMedicationOrder["PatientId"].ToString();
            var duration = "";
            var unit = "";
            _dsPatientDataSection = GetPatientDataSection_CategoryOne(Convert.ToInt64(patientId), "", "");
            if (_dsPatientDataSection.MedicationActive.Rows.Count > 0)
            {
                duration = _dsPatientDataSection.MedicationActive.Rows[0][_dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName].ToString();
                unit = _dsPatientDataSection.MedicationActive.Rows[0][_dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName].ToString();
            }

            if (drMedicationOrder["ICD"].ToString() != "" && drMedicationOrder["ICD"].ToString().IndexOf('|') > -1)
            {
                Reason = drMedicationOrder["ICD"].ToString().Split('|')[1];
                codeValue = drMedicationOrder["ICD"].ToString().Split('|')[0];
                codeValue = codeValue == "" ? drMedicationOrder["ICD"].ToString().Split('|')[1] : codeValue;
            }
            if (drMedicationOrder["ICDDescription"].ToString() != "" && drMedicationOrder["ICDDescription"].ToString().IndexOf('|') > -1)
            {
                ReasonComments = drMedicationOrder["ICDDescription"].ToString().Split('|')[1];
                codeDescription = drMedicationOrder["ICDDescription"].ToString().Split('|')[0];
            }
            if (drMedicationOrder["valueset"].ToString() != "" && drMedicationOrder["valueset"].ToString().IndexOf('|') > -1)
            {
                ReasonValueset = drMedicationOrder["valueset"].ToString().Split('|')[1];
                valueset = drMedicationOrder["valueset"].ToString().Split('|')[0];
                if (condition.IndexOf("not done:") > -1)
                {
                    valueset = drMedicationOrder["valueset"].ToString().Split('|')[1];
                }
                
            }

            if (condition.IndexOf("not done:") > -1)
            {
                var entryMedicationOrder = new Entry()
                {
                    Item = new SubstanceAdministration()
                    {
                        classCode = "SBADM",
                        moodCode = x_DocumentSubstanceMood.RQO,
                        negationInd = true,
                        templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.22.4.42",
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.47",
                            extension = "2016-02-01"
                        }
                    },
                        id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                            code = "active"
                        },
                        effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                                },
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")

                                }
                            }
                        },
                        new PIVL_TS
                        {
                            institutionSpecified1 = true,
                            @operator = SetOperator.A,
                            period = new PQ
                            {
                                nullFlavor = "NA"

                            }
                        }
                    },
                        doseQuantity = new IVL_PQ
                        {
                            nullFlavor = "NA"
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
                                    extension = "2014-06-09"
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
                                        nullFlavor = "NA",
                                        valueSet = valueset

                                    }
                                }

                            }
                        },
                        author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }

                            }
                        }
                    },
                        entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                 effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low
                        },
                            Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                        }
                        },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = codeValue,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                       valueSet ="2.16.840.1.113883.3.600.1.1502"
                                    }
                                }
                            }
                        }
                    }
                    }
                };
                return entryMedicationOrder;
            }
            else
            {

                if (drMedicationOrder["ICD"].ToString().IndexOf('|') > -1 && !string.IsNullOrEmpty(drMedicationOrder["ICD"].ToString().Split('|')[1]) && drMedicationOrder["ICD"].ToString().Split('|')[1] != "0")
                {

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
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.47",
                            extension = "2016-02-01"
                        }
                    },
                            id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                                code = "active"
                            },
                            effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                                },
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")

                                }
                            }
                        },
                        new PIVL_TS
                        {
                            institutionSpecified1 = true,
                            @operator = SetOperator.A,
                            period = new PQ
                            {
                                value = duration,
                                unit = "d"
                            }
                        }
                    },
                            doseQuantity = new IVL_PQ
                            {
                                unit = unit,
                                value = "1"
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
                                    extension = "2014-06-09"
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
                                            valueSet = valueset,
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                        {
                                            "Medication Order: " + codeDescription
                                        }
                                            }
                                        }
                                    }

                                }
                            },
                            author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }
                                //code = new CE
                                //{
                                //    code = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderTaxonomyCodeColumn.ColumnName].ToString(),
                                //    codeSystem = "2.16.840.1.113883.6.101",
                                //    displayName = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderSpecialityDescriptionColumn.ColumnName].ToString(),
                                //    codeSystemName = "Healthcare Provider Taxonomy (HIPAA)"
                                //}
                            }
                        }
                    },
                            entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                 effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low
                        },
                            Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                        }
                        },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = Reason,
                                        displayName = ReasonComments,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet = ReasonValueset
                                    }
                                }
                            }
                        }
                    }
                        }
                    };
                    return entryMedicationOrder;
                }
                else
                {

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
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.47",
                            extension = "2016-02-01"
                        }
                    },
                            id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                                code = "active"
                            },
                            effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                                },
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")

                                }
                            }
                        },
                        new PIVL_TS
                        {
                            institutionSpecified1 = true,
                            @operator = SetOperator.A,
                            period = new PQ
                            {
                                value = duration,
                                unit = "d"
                            }
                        }
                    },
                            repeatNumber = new IVL_INT
                            {
                                value = "1"
                            },
                            doseQuantity = new IVL_PQ
                            {
                                unit = unit,
                                value = "1"
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
                                    extension = "2014-06-09"
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
                                            valueSet = valueset,
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                        {
                                            "Medication Order: " + codeDescription
                                        }
                                            }
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
                            },
                            author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }
                                //code = new CE
                                //{
                                //    code = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderTaxonomyCodeColumn.ColumnName].ToString(),
                                //    codeSystem = "2.16.840.1.113883.6.101",
                                //    displayName = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderSpecialityDescriptionColumn.ColumnName].ToString(),
                                //    codeSystemName = "Healthcare Provider Taxonomy (HIPAA)"
                                //}
                            }
                        }
                    }
                        }
                    };
                    return entryMedicationOrder;
                }
            }
        }

        private static Entry SetMedicationOrder22v5(DataRow drMedicationOrder)
        {
            var codeValue = drMedicationOrder["ICD"].ToString();
            var codeDescription = drMedicationOrder["ICDDescription"].ToString();
            var valueset = drMedicationOrder["valueset"].ToString();
            var condition = drMedicationOrder["Condition"].ToString();
            string Reason = "";
            string ReasonComments = "";
            string ReasonValueset = "";

            var patientId = drMedicationOrder["PatientId"].ToString();
            var duration = "";
            var unit = "";
            _dsPatientDataSection = GetPatientDataSection_CategoryOne(Convert.ToInt64(patientId), "", "");
            if (_dsPatientDataSection.MedicationActive.Rows.Count > 0)
            {
                duration = _dsPatientDataSection.MedicationActive.Rows[0][_dsPatientDataSection.MedicationActive.MedicationDurationColumn.ColumnName].ToString();
                unit = _dsPatientDataSection.MedicationActive.Rows[0][_dsPatientDataSection.MedicationActive.MedicationUnitColumn.ColumnName].ToString();
            }

            if (drMedicationOrder["ICD"].ToString() != "" && drMedicationOrder["ICD"].ToString().IndexOf('|') > -1)
            {
                Reason = drMedicationOrder["ICD"].ToString().Split('|')[1];
                codeValue = drMedicationOrder["ICD"].ToString().Split('|')[0];
                codeValue = codeValue == "" ? drMedicationOrder["ICD"].ToString().Split('|')[1] : codeValue;
            }
            if (drMedicationOrder["ICDDescription"].ToString() != "" && drMedicationOrder["ICDDescription"].ToString().IndexOf('|') > -1)
            {
                ReasonComments = drMedicationOrder["ICDDescription"].ToString().Split('|')[1];
                codeDescription = drMedicationOrder["ICDDescription"].ToString().Split('|')[0];
            }
            if (drMedicationOrder["valueset"].ToString() != "" && drMedicationOrder["valueset"].ToString().IndexOf('|') > -1)
            {
                ReasonValueset = drMedicationOrder["valueset"].ToString().Split('|')[1];
                valueset = drMedicationOrder["valueset"].ToString().Split('|')[0];
            }

            if (condition.IndexOf("not done:") > -1)
            {
                var entryMedicationOrder = new Entry()
                {
                    Item = new SubstanceAdministration()
                    {
                        classCode = "SBADM",
                        moodCode = x_DocumentSubstanceMood.RQO,
                        negationInd = true,
                        templateId = new List<II>()
                    {
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.22.4.42",
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.47",
                            extension = "2016-02-01"
                        }
                    },
                        id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                            code = "active"
                        },
                        effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                                },
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")

                                }
                            }
                        },
                        new PIVL_TS
                        {
                            institutionSpecified1 = true,
                            @operator = SetOperator.A,
                            period = new PQ
                            {
                                nullFlavor = "NA"

                            }
                        }
                    },
                        doseQuantity = new IVL_PQ
                        {
                            nullFlavor = "NA"
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
                                    extension = "2014-06-09"
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
                                        nullFlavor = "NA",
                                        valueSet = valueset

                                    }
                                }

                            }
                        },
                        author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }

                            }
                        }
                    },
                        entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                 effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low
                        },
                            Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                        }
                        },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = Reason,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                       valueSet = ReasonValueset
                                    }
                                }
                            }
                        }
                    }
                    }
                };
                return entryMedicationOrder;
            }
            else
            {

                if (drMedicationOrder["ICD"].ToString().IndexOf('|') > -1 && !string.IsNullOrEmpty(drMedicationOrder["ICD"].ToString().Split('|')[1]) && drMedicationOrder["ICD"].ToString().Split('|')[1] != "0")
                {

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
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.47",
                            extension = "2016-02-01"
                        }
                    },
                            id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                                code = "active"
                            },
                            effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                                },
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")

                                }
                            }
                        },
                        new PIVL_TS
                        {
                            institutionSpecified1 = true,
                            @operator = SetOperator.A,
                            period = new PQ
                            {
                                value = duration,
                                unit = "d"
                            }
                        }
                    },
                            doseQuantity = new IVL_PQ
                            {
                                unit = unit,
                                value = "1"
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
                                    extension = "2014-06-09"
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
                                            valueSet = valueset,
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                        {
                                            "Medication Order: " + codeDescription
                                        }
                                            }
                                        }
                                    }

                                }
                            },
                            author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }
                                //code = new CE
                                //{
                                //    code = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderTaxonomyCodeColumn.ColumnName].ToString(),
                                //    codeSystem = "2.16.840.1.113883.6.101",
                                //    displayName = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderSpecialityDescriptionColumn.ColumnName].ToString(),
                                //    codeSystemName = "Healthcare Provider Taxonomy (HIPAA)"
                                //}
                            }
                        }
                    },
                            entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                 effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low
                        },
                            Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value =
                                    DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                        }
                        },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = Reason,
                                        displayName = ReasonComments,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet = ReasonValueset
                                    }
                                }
                            }
                        }
                    }
                        }
                    };
                    return entryMedicationOrder;
                }
                else
                {

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
                            extension = "2014-06-09"
                        },
                        new II()
                        {
                            root = "2.16.840.1.113883.10.20.24.3.47",
                            extension = "2016-02-01"
                        }
                    },
                            id = new List<II>()
                    {
                        new II()
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                                code = "active"
                            },
                            effectiveTime = new List<SXCM_TS>()
                    {
                        new IVL_TS()
                        {
                            ItemsElementName = new[] {ItemsChoiceType2.low, ItemsChoiceType2.high},
                            Items = new QTY[]
                            {
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                                },
                                new IVXB_TS
                                {
                                    value = string.IsNullOrEmpty(Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drMedicationOrder["EndDate"].ToString()).ToString("yyyyMMddHHmmss")

                                }
                            }
                        },
                        new PIVL_TS
                        {
                            institutionSpecified1 = true,
                            @operator = SetOperator.A,
                            period = new PQ
                            {
                                value = duration,
                                unit = "d"
                            }
                        }
                    },
                            repeatNumber = new IVL_INT
                            {
                                value = "1"
                            },
                            doseQuantity = new IVL_PQ
                            {
                                unit = unit,
                                value = "1"
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
                                    extension = "2014-06-09"
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
                                            valueSet = valueset,
                                            originalText = new ED
                                            {
                                                Text = new List<string>
                                        {
                                            "Medication Order: " + codeDescription
                                        }
                                            }
                                        }
                                    }
                                }
                            },
                            author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }
                            }
                        }
                    }
                        }
                    };
                    return entryMedicationOrder;
                }
            }
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
        private static Entry SetEncounterPerformedOld(DataRow drEncounterPerformed)
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
        private static Entry SetEncounterPerformed(DataRow drEncounterPerformed)
        {
            var codeValue = drEncounterPerformed["ICD"].ToString();
            var diseaseName = drEncounterPerformed["ICDDescription"].ToString();
            var valueset = drEncounterPerformed["valueset"].ToString();

            var objEncounterPerformed = new Entry()
            {
                Item = new Act
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>()
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.133"}
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
                        code = "ENC",
                        codeSystem = "2.16.840.1.113883.5.6",
                        displayName = "Encounter",
                        codeSystemName = "ActClass"
                    },
                    entryRelationship = new List<EntryRelationship>
                    {
                        new EntryRelationship
                        {
                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                            Item = new Encounter
                            {
                                classCode = "ENC",
                                moodCode = x_DocumentEncounterMood.EVN,
                                templateId = new List<II>()
                                {
                                    new II {root = "2.16.840.1.113883.10.20.22.4.49", extension = "2015-08-01" },
                                    new II {root = "2.16.840.1.113883.10.20.24.3.23", extension = "2016-02-01" }
                                },
                                id = new List<II>
                                {
                                    new II { root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString() }
                                },
                                code = new CD()
                                {
                                    code = codeValue,
                                    codeSystem = drEncounterPerformed["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                            : (drEncounterPerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                                            : (drEncounterPerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("D")) ? "2.16.840.1.113883.6.13"
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
                                            value = string.IsNullOrEmpty( Convert.ToDateTime(drEncounterPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drEncounterPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        },
                                        new IVXB_TS
                                        {
                                            value = string.IsNullOrEmpty( Convert.ToDateTime(drEncounterPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drEncounterPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return objEncounterPerformed;
        }

        private static Entry SetEncounterPerformed160(DataRow drEncounterPerformed)
        {
            var codeValue = drEncounterPerformed["ICD"].ToString().Split('|')[0];
            var valueCode = drEncounterPerformed["ICD"].ToString().Split('|')[1];
            var codeValueset = drEncounterPerformed["valueset"].ToString().Split('|')[0];
            var valueValueset = drEncounterPerformed["valueset"].ToString().Split('|')[1];
            var diseaseName = drEncounterPerformed["ICDDescription"].ToString();
            var codesys = "";
            if (diseaseName.IndexOf('|') > -1)
            {
                diseaseName = drEncounterPerformed["ICDDescription"].ToString().Split('|')[0];
                codesys = drEncounterPerformed["ICDDescription"].ToString().Split('|')[1];
            }
            var objEncounterPerformed = new Entry()
            {
                Item = new Act
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>()
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.133"}
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
                        code = "ENC",
                        codeSystem = "2.16.840.1.113883.5.6",
                        displayName = "Encounter",
                        codeSystemName = "ActClass"
                    },
                    entryRelationship = new List<EntryRelationship>
                    {
                        new EntryRelationship
                        {
                            typeCode = x_ActRelationshipEntryRelationship.SUBJ,
                            Item = new Encounter
                            {
                                classCode = "ENC",
                                moodCode = x_DocumentEncounterMood.EVN,
                                templateId = new List<II>()
                                {
                                    new II {root = "2.16.840.1.113883.10.20.22.4.49", extension = "2015-08-01" },
                                    new II {root = "2.16.840.1.113883.10.20.24.3.23", extension = "2016-02-01" }
                                },
                                id = new List<II>
                                {
                                    new II { root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString() }
                                },
                                code = new CD()
                                {
                                    code = codeValue,
                                    codeSystem =
                                    drEncounterPerformed["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                            : (drEncounterPerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                                            : (drEncounterPerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("D")) ? "2.16.840.1.113883.6.13"
                                            : drEncounterPerformed["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                            : drEncounterPerformed["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                            : drEncounterPerformed["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            : drEncounterPerformed["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                                    valueSet = codeValueset,
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
                                            value = string.IsNullOrEmpty( Convert.ToDateTime(drEncounterPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drEncounterPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        },
                                        new IVXB_TS
                                        {
                                            value = string.IsNullOrEmpty( Convert.ToDateTime(drEncounterPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drEncounterPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")
                                        }
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

                                                code = new CD()
                                                {
                                                    code = "8319008",
                                                    codeSystem = "2.16.840.1.113883.6.1",
                                                    displayName = "Principal Diagnosis",
                                                    codeSystemName = "SNOMED CT"

                                                },

                                                value = new List<ANY>()
                                                {
                                                new CD()
                                                {
                                                    code = valueCode,
                                                    codeSystem = codesys != "" ?codesys :(Char.IsLetter(valueCode[0]) == true ? "2.16.840.1.113883.6.90":"2.16.840.1.113883.6.96"),
                                                    valueSet=valueValueset
                                             }
                                          }
                                        }
                                      }
                                   }
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
            var condition = drProcedurePerformed["Condition"].ToString();

            if (condition.IndexOf("not done:") > -1 && codeValue != "" && codeValue != null)
            {
                string negationValueset = "2.16.840.1.113883.3.526.3.402";
                if (CQMID == "0419")
                {
                    negationValueset = "2.16.840.1.113883.3.600.1.462";
                }

                var objProcedurePerformed = new Entry()
                {
                    Item = new Procedure()
                    {
                        negationInd = true,
                        classCode = "PROC",
                        moodCode = x_DocumentProcedureMood.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.14", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.64", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
                        }
                    },
                        code = new CD()
                        {
                            nullFlavor = "NA",
                            valueSet = negationValueset

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
                                    string.IsNullOrEmpty(Convert.ToDateTime(drProcedurePerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drProcedurePerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drProcedurePerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drProcedurePerformed["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")

                            }
                        }
                        },
                        entryRelationship = new List<EntryRelationship>
                         {
                            new EntryRelationship()
                          {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                              Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    displayName = "reason",
                                    codeSystemName = "LOINC"

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
                                value =DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                        }
                    },
                                 value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = codeValue,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet= valueset
                                    }
                                }
                            }
                        }
                      }
                    }
                };
                return objProcedurePerformed;
            }
            else
            {
                var objProcedurePerformed = new Entry()
                {
                    Item = new Procedure()
                    {
                        classCode = "PROC",
                        moodCode = x_DocumentProcedureMood.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.14", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.64", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II
                        {
                            root = "1.3.6.1.4.1.115",
                            extension = Guid.NewGuid().ToString()
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
                            },
                            translation = new List<CD>
                        {
                            new CD
                            {
                                code = codeValue,
                                codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                            : drProcedurePerformed["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                            : (drProcedurePerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                                            : (drProcedurePerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("D")) ? "2.16.840.1.113883.6.13"
                                            : drProcedurePerformed["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                            : drProcedurePerformed["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                            : drProcedurePerformed["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            : drProcedurePerformed["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
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
                                    string.IsNullOrEmpty(Convert.ToDateTime(drProcedurePerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drProcedurePerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")

                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drProcedurePerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drProcedurePerformed["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")

                            }
                        }
                        }
                    }
                };
                return objProcedurePerformed;

            }
        }

        private static Entry SetProcedurePerformed0041(DataRow drProcedurePerformed)
        {
            var codeValue = drProcedurePerformed["ICD"].ToString();
            var diseaseName = drProcedurePerformed["ICDDescription"].ToString();
            var valueset = drProcedurePerformed["valueset"].ToString();
            var condition = drProcedurePerformed["Condition"].ToString();

            var objProcedurePerformed = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",

                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.62", extension = "2016-02-01"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.104", extension="2016-02-01"}
                    },
                    id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        codeSystemName = "ActCode",
                        displayName = "Assertion",


                    },

                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                    {
                            ItemsChoiceType2.low
                        },
                        Items = new QTY[]
                    {
                           new IVXB_TS
                            {
                                value = DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                    }
                    },
                    value = new List<ANY>
                        {
                        new CD
                        {
                            code ="102460003",
                            codeSystem="2.16.840.1.113883.6.96",
                            codeSystemName="SNOMED CT",
                            displayName="Decreased tolerance"
                        }
                      },
                    entryRelationship = new List<EntryRelationship>
                         {
                       new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.CAUS,
                            inversionInd = true,
                              Item = new Procedure()
                            {
                                classCode = "PROC",
                                moodCode = x_DocumentProcedureMood.EVN,
                                templateId = new List<II>
                                {
                                    new II {root = "2.16.840.1.113883.10.20.24.3.64", extension = "2016-02-01"},
                                    new II {root = "2.16.840.1.113883.10.20.22.4.14", extension="2014-06-09"}
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = codeValue,
                                    codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                            : drProcedurePerformed["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                            : (drProcedurePerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                                            : (drProcedurePerformed["Title"].ToString().Contains("CPTCode") && codeValue.StartsWith("D")) ? "2.16.840.1.113883.6.13"
                                            : drProcedurePerformed["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                            : drProcedurePerformed["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                            : drProcedurePerformed["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            : drProcedurePerformed["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            : drProcedurePerformed["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                                    valueSet = valueset

                                },
                                 text = new ED()
                                {
                                    Text = new List<string>
                                    {
                                        "Procedure, Intolerance: Influenza Vaccination"
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
                                        value =Convert.ToDateTime(drProcedurePerformed["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                                    },
                                     new IVXB_TS
                                    {
                                        value =Convert.ToDateTime(drProcedurePerformed["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")

                                    }
                                    }
                                }

                            }
                        }
                    }

                }
            };
            return objProcedurePerformed;


        }
        #endregion

        #region Risk Catagory Assesment
        private static Entry SetRiskCategoryAssessmentOld(DataRow drRiskCategoryAssessment)
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

        private static Entry SetRiskCategoryAssessment(DataRow drRiskCategoryAssessment, string resultSNOMED = "", string resultValueSet = "")
        {
            var CodeValue = drRiskCategoryAssessment["ICD"].ToString();
            var negativeVlueset = "2.16.840.1.113883.3.600.2452";
            if (CodeValue == "105480006")
            {
                negativeVlueset = "2.16.840.1.113883.3.600.2452";
            }
            else if (CodeValue == "183932001")
            {
                negativeVlueset = "2.16.840.1.113883.3.464.1003.118.12.1028";
            }
            if (CQMID == "0028" && CodeValue == "183932001")
            {
                negativeVlueset = "2.16.840.1.113883.3.526.3.1278";
            }
            else if (CQMID == "0418" && drRiskCategoryAssessment["Condition"].ToString().IndexOf("not done:") > -1 && drRiskCategoryAssessment["Condition"].ToString().ToLower().IndexOf("adolescent depression screening") > -1)
            {
                negativeVlueset = "2.16.840.1.113883.3.600.2452";
            }
            else if (CQMID == "0418" && drRiskCategoryAssessment["Condition"].ToString().IndexOf("not done:") > -1 && drRiskCategoryAssessment["Condition"].ToString().ToLower().IndexOf("adult depression screening") > -1)
            {
                negativeVlueset = "2.16.840.1.113883.3.600.2449";
            }

            var codeDescription = drRiskCategoryAssessment["ICDDescription"].ToString();
            string valueSet = drRiskCategoryAssessment["valueset"].ToString();
            string Condition = drRiskCategoryAssessment["Condition"].ToString();
            if (Condition.IndexOf("Patient Reason refused") > -1 || Condition.IndexOf("not done:") > -1)
            {
                var objRiskCategoryAssessment = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        negationInd = true,
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.69"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.69", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            //code = CodeValue,
                            //codeSystem = drRiskCategoryAssessment["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                            //                    : (drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") && CodeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                            //                    : drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                            //                    : drRiskCategoryAssessment["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                            //                    : drRiskCategoryAssessment["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                            //                    : drRiskCategoryAssessment["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                            nullFlavor = "NA",
                            valueSet = negativeVlueset,
                            originalText = new ED()
                            {
                                Text = new List<string>()
                            {
                                "Risk Category Assessment: " + codeDescription
                            }
                            },
                            //    translation = new List<CD>
                            //   {
                            //    new CD
                            //    {
                            //        code = CodeValue,
                            //        codeSystem = CodeValue.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96"
                            //        //codeSystem = drRiskCategoryAssessment["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                            //                    //: (drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") && CodeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                            //                    //: drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                            //                    //: drRiskCategoryAssessment["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                            //                    //: drRiskCategoryAssessment["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                            //                    //: drRiskCategoryAssessment["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                            //    }
                            //}
                        },
                        text = new ED()
                        {
                            Text = new List<string>
                        {
                            codeDescription
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
                                    string.IsNullOrEmpty( Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                        },
                        value = new List<ANY>
                        {
                        new CD
                        {
                            nullFlavor ="UNK"
                        }
                      },
                        entryRelationship = new List<EntryRelationship>
                         {
                       new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                              Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    displayName = "reason",
                                    codeSystemName = "LOINC"

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
                                value =DateTime.Now.ToString("yyyyMMddHHmmss")
                                       

                            //},
                            //new IVXB_TS
                            //{
                            //    nullFlavor = "NA"
                            //    // value = string.IsNullOrEmpty(Convert.ToDateTime(drRiskCategoryAssessment["OrderDate"].ToString()).ToString("yyyyMMddHHmmss"))
                            //    //? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["OrderDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                                 value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = CodeValue,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet=valueSet
                                    }
                                }
                            }
                        }
                    }

                    }
                };
                return objRiskCategoryAssessment;
            }
            else
            {
                var objRiskCategoryAssessment = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.69"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.69", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            code = CodeValue,
                            codeSystem = CodeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                                : drRiskCategoryAssessment["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                                : (drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") && CodeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                                                : (drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") && CodeValue.StartsWith("D")) ? "2.16.840.1.113883.6.13"
                                                : drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                                : drRiskCategoryAssessment["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                                : drRiskCategoryAssessment["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                                : drRiskCategoryAssessment["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                                : drRiskCategoryAssessment["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                            valueSet = valueSet,
                            originalText = new ED()
                            {
                                Text = new List<string>()
                            {
                                "Risk Category Assessment: " + codeDescription
                            }
                            },
                            translation = new List<CD>
                        {
                            new CD
                            {
                                code = CodeValue,
                                codeSystem = CodeValue.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96"
                                //codeSystem = drRiskCategoryAssessment["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                            //: (drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") && CodeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                                            //: drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                            //: drRiskCategoryAssessment["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                            //: drRiskCategoryAssessment["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            //: drRiskCategoryAssessment["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                            }
                        }
                        },
                        text = new ED()
                        {
                            Text = new List<string>
                        {
                            codeDescription
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
                                    string.IsNullOrEmpty( Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                        },
                        value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = resultSNOMED != "" ? resultSNOMED : CodeValue,
                                        codeSystem = resultSNOMED != "" ? (resultSNOMED.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96"):(CodeValue.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96"),
                                        valueSet=resultValueSet != "" ? resultValueSet : valueSet,
                                    }
                                }
                    }
                };
                return objRiskCategoryAssessment;
            }

        }
        private static Entry SetRiskCategoryAssessment160(DataRow drRiskCategoryAssessment, string LOINCResultValue = "")
        {
            var CodeValue = drRiskCategoryAssessment["ICD"].ToString().Split('|')[0];

            var codeDescription = drRiskCategoryAssessment["ICDDescription"].ToString();
            string valueSet = drRiskCategoryAssessment["valueset"].ToString();
            string Condition = drRiskCategoryAssessment["Condition"].ToString();

            var objRiskCategoryAssessment = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.69"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.69", extension = "2016-02-01"}
                    },
                    id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = CodeValue,
                        codeSystem = CodeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                            : drRiskCategoryAssessment["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                            : (drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") && CodeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                                            : (drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") && CodeValue.StartsWith("D")) ? "2.16.840.1.113883.6.13"
                                            : drRiskCategoryAssessment["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                            : drRiskCategoryAssessment["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                            : drRiskCategoryAssessment["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            : drRiskCategoryAssessment["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            : drRiskCategoryAssessment["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                        valueSet = valueSet,
                        originalText = new ED()
                        {
                            Text = new List<string>()
                            {
                                "Risk Category Assessment: " + codeDescription
                            }
                        }

                    },
                    text = new ED()
                    {
                        Text = new List<string>
                        {
                            codeDescription
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
                                    string.IsNullOrEmpty( Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drRiskCategoryAssessment["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                    }
                    },
                    value = new List<ANY>()
                    {
                        new PQ()
                        {
                            value = "12",//LOINCResultValue,
                            unit = "1"
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

        private static Entry SetPatientCharacteristics(DataRow drTobbacoUser)
        {
            var snomedCodeValue = drTobbacoUser["ICD"].ToString();
            var snomedDescription = drTobbacoUser["ICDDescription"].ToString();
            string tobbacoValueSet = drTobbacoUser["valueset"].ToString(); //"2.16.840.1.113883.3.526.3.1170";

            var objTobbacoUser = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.103", extension = "2016-02-01"}
                    },
                    id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115" ,extension = Guid.NewGuid().ToString()}
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
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drTobbacoUser["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drTobbacoUser["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                nullFlavor = "UNK"
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = snomedCodeValue,
                            codeSystem =  "2.16.840.1.113883.6.96",
                            //codeSystem = drTobbacoUser["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                            //                : (drTobbacoUser["Title"].ToString().Contains("CPTCode") && snomedCodeValue.StartsWith("G")) ? "2.16.840.1.113883.6.285"
                            //                : drTobbacoUser["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                            //                : drTobbacoUser["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                            //                : drTobbacoUser["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                            //                : drTobbacoUser["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                            valueSet = tobbacoValueSet,
                            originalText = new ED()
                            {
                                Text = new List<string>()
                                {
                                    "Patient Characteristic: " + snomedDescription
                                }
                            },
                            translation = new List<CD>
                            {
                                new CD
                                {
                                    code = snomedCodeValue,
                                    codeSystem = snomedCodeValue.Contains("-") ? "2.16.840.1.113883.6.1": "2.16.840.1.113883.6.96"
                                }
                            }
                        }
                    }
                }
            };
            return objTobbacoUser;
        }

        private static Entry SetPatientCharacteristics_New(DataRow drTobbacoUser)
        {
            var snomedCodeValue = drTobbacoUser["ICD"].ToString();
            var snomedDescription = drTobbacoUser["ICDDescription"].ToString();
            string tobbacoValueSet = drTobbacoUser["valueset"].ToString(); //"2.16.840.1.113883.3.526.3.1170";

            var objTobbacoUser = new Entry()
            {
                Item = new Observation()
                {
                    classCode = "OBS",
                    moodCode = x_ActMoodDocumentObservation.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.79", extension = "2015-08-01"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.54", extension = "2016-02-01"}
                    },
                    id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115" ,extension = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = "ASSERTION",
                        codeSystem = "2.16.840.1.113883.5.4",
                        codeSystemName = "HL7ActCode"
                    },
                    statusCode = new CS()
                    {
                        code = "completed"
                    },
                    effectiveTime = new IVL_TS()
                    {
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType2.low
                        },
                        Items = new QTY[]
                        {
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drTobbacoUser["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drTobbacoUser["StartDate"].ToString()).ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    value = new List<ANY>()
                    {
                        new CD()
                        {
                            code = snomedCodeValue,
                            codeSystem =  "2.16.840.1.113883.6.96",
                            codeSystemName = "SNOME CT",
                            displayName = "Dead"

                        }
                    }
                }
            };
            return objTobbacoUser;
        }
        #endregion

        #region Physical Exam
        private static Entry SetPhysicalExam(DataRow drPhysicalExam, string bloodPressureType, string valueSet, string loincCodeValue, string bloodPressureValue, string negationValue = "", string negationValueset = "")
        {
            var startdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var enddate = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (drPhysicalExam.Table.Columns.Contains("ICD") == true && drPhysicalExam["ICD"].ToString().IndexOf('|') > -1 && drPhysicalExam["ICD"].ToString().Split('|')[0] == "71789-2")
            {
                startdate = drPhysicalExam["StartDate"].ToString();
                enddate = drPhysicalExam["EndDate"].ToString();
            }
            else
            {
                startdate = drPhysicalExam["VitalSignDate"].ToString();
                enddate = drPhysicalExam["VitalSignDate"].ToString();
            }

            if (negationValue != "")
            {
                var objTobbacoUser = new Entry()
                {
                    Item = new Observation()
                    {
                        negationInd = true,
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.13", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.59", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II {root="1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            nullFlavor = "NA",
                            valueSet = "2.16.840.1.113883.3.600.1.681"
                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Physical Exam, Performed: " + bloodPressureType }
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
                                        Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(enddate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(enddate)
                                            .ToString("yyyyMMddHHmmss")
                            }
                                       }
                        },
                        value = new List<ANY>()
                    {
                        new CD()
                        {
                            nullFlavor = "UNK"
                        }
                    },
                        entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[]
                            {
                            ItemsChoiceType2.low
                        },
                            Items = new QTY[]
                            {
                            new IVXB_TS
                            {
                                value =  DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                          }
                        },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = negationValue,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet = negationValueset
                                    }
                                }
                            }
                        }
                    }
                    }
                };
                return objTobbacoUser;
            }
            else
            {
                var objTobbacoUser = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.13", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.59", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II {root="1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            code = loincCodeValue,
                            codeSystem = "2.16.840.1.113883.6.1",
                            valueSet = valueSet
                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Physical Exam, Performed: " + bloodPressureType }
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
                                        Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(enddate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(enddate)
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
        }

        private static Entry SetPhysicalExamCMS22(DataRow drPhysicalExam, string bloodPressureType, string valueSet, string loincCodeValue, string bloodPressureValue, string negationValue = "", string negationValueset = "")
        {
            var startdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var enddate = DateTime.Now.ToString("yyyyMMddHHmmss");
            startdate = drPhysicalExam["StartDate"].ToString();
            enddate = drPhysicalExam["EndDate"].ToString();

            var upperValue = "";
            var lowerValue = "";
            var upperValueset = "";
            var lowerValueset = "";
            var ResultValue = "";

            if (drPhysicalExam["Condition"].ToString().IndexOf("not done:") > -1)
            {
                if (drPhysicalExam["ICD"].ToString() != "" && drPhysicalExam["ICD"].ToString().IndexOf('|') > -1)
                {
                    upperValue = drPhysicalExam["ICD"].ToString().Split('|')[0];
                    lowerValue = drPhysicalExam["ICD"].ToString().Split('|')[1];

                }
                if (drPhysicalExam["valueset"].ToString() != "" && drPhysicalExam["valueset"].ToString().IndexOf('|') > -1)
                {
                    upperValueset = drPhysicalExam["valueset"].ToString().Split('|')[1];
                    lowerValueset = drPhysicalExam["valueset"].ToString().Split('|')[0];
                }
            }
            else
            {

                //bloodPressureValue = drPhysicalExam["valueset"].ToString();
                valueSet = drPhysicalExam["valueset"].ToString();
                if (drPhysicalExam["ICD"].ToString().IndexOf('|') > 0)
                {
                    loincCodeValue = drPhysicalExam["ICD"].ToString().Split('|')[0];
                    ResultValue = drPhysicalExam["ICD"].ToString().Split('|')[1];
                }
            }

            if (drPhysicalExam["Condition"].ToString().IndexOf("not done:") > -1)
            {
                var objTobbacoUser = new Entry()
                {
                    Item = new Observation()
                    {
                        negationInd = true,
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.13", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.59", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II {root="1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            nullFlavor = "NA",
                            valueSet = upperValueset
                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Physical Exam, Performed: " + bloodPressureType }
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
                                        Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(enddate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(enddate)
                                            .ToString("yyyyMMddHHmmss")
                            }
                                       }
                        },
                        value = new List<ANY>()
                    {
                        new CD()
                        {
                            nullFlavor = "UNK"
                        }
                    },
                        entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                effectiveTime = new IVL_TS()
                        {
                            ItemsElementName = new[]
                            {
                            ItemsChoiceType2.low
                        },
                            Items = new QTY[]
                            {
                            new IVXB_TS
                            {
                                value =  DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                          }
                        },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = lowerValue,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet = lowerValueset
                                    }
                                }
                            }
                        }
                    }
                    }
                };
                return objTobbacoUser;
            }
            else
            {
                var objTobbacoUser = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.13", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.59", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II {root="1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            code = loincCodeValue,
                            codeSystem = "2.16.840.1.113883.6.1",
                            valueSet = valueSet
                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Physical Exam, Performed: " + bloodPressureType }
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
                                        Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(startdate)
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(enddate)
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(enddate)
                                            .ToString("yyyyMMddHHmmss")
                            }
                            }
                        },
                        value = new List<ANY>()
                    {
                        new PQ()
                        {
                            value = ResultValue,
                            unit = "mmHg"
                        }
                    }
                    }
                };
                return objTobbacoUser;

            }
        }

        #endregion

        #region Lab Order Test
        private static Entry SetLabOrder(DataRow drLabOrder)
        {
            var codeValue = drLabOrder["ICD"].ToString();
            var codeDescription = drLabOrder["ICDDescription"].ToString();
            var valueset = drLabOrder["valueset"].ToString();
            var patientId = drLabOrder["PatientId"].ToString();
            var Condition = drLabOrder["Condition"].ToString();
            _dsPatientDataSection = GetPatientDataSection_CategoryOne(Convert.ToInt64(patientId), "", codeValue);
            var negationCodeValue = "";
            var negationValueset = "";
            var result = "";
            var unit = "";

            if (_dsPatientDataSection.PatientToProvider.Rows.Count > 0)
            {
                result = _dsPatientDataSection.PatientToProvider.Rows[0][_dsPatientDataSection.PatientToProvider.ResultColumn.ColumnName].ToString();
                unit = _dsPatientDataSection.PatientToProvider.Rows[0][_dsPatientDataSection.PatientToProvider.UnitColumn.ColumnName].ToString();
            }

            if (CQMID == "0062" || CQMID == "0034")
            {
                result = result == "" ? result = "Negative" : result;
            }

            if (codeValue.IndexOf('|') > -1)
            {
                codeValue = drLabOrder["ICD"].ToString().Split('|')[0];
                negationCodeValue = drLabOrder["ICD"].ToString().Split('|')[1];
            }
            if (valueset.IndexOf('|') > -1)
            {
                valueset = drLabOrder["valueset"].ToString().Split('|')[1];
                negationValueset = drLabOrder["valueset"].ToString().Split('|')[0];
            }

            if (Condition.IndexOf("not done:") > -1)
            {
                var objLabOrder = new Entry()
                {
                    Item = new Observation()
                    {
                        negationInd = true,
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.RQO,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.44" , extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.37" , extension = "2016-02-01"},

                    },
                        id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            nullFlavor = "NA",
                            valueSet = valueset
                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Laboratory Test, Result: " + codeDescription }
                        },
                        statusCode = new CS()
                        {
                            code = "active"

                        },
                        author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                       nullFlavor = "NA", root =  Guid.NewGuid().ToString()
                                    }
                                }

                            }
                        }
                    },
                        entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                effectiveTime = new IVL_TS()
                            {
                                ItemsElementName = new[]
                            {
                            ItemsChoiceType2.low
                        },
                                Items = new QTY[]
                            {

                            new IVXB_TS
                            {
                                value =  DateTime.Now.ToString("yyyyMMddHHmmss")
                            }
                            }
                            },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = negationCodeValue,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                       valueSet = negationValueset
                                    }
                                }
                            }
                        }
                    }
                    }
                };
                return objLabOrder;
            }
            else if (CQMID == "0062" || CQMID == "0034")
            {
                var objLabOrder = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.38" , extension = "2016-02-01"},

                    },
                        id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
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
                        new ST()
                        {
                          Text = new List<string> { result }
                        }
                    }
                    }
                };
                return objLabOrder;
            }
            else
            {

                var objLabOrder = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.38" , extension = "2016-02-01"},

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
                            value = result,
                            unit = unit
                        }
                    }
                    }
                };
                return objLabOrder;
            }
        }

        #endregion

        private static Entry SetDiagnosticStudy(DataRow drDiagnosticStudy)
        {
            var codeValue = drDiagnosticStudy["ICD"].ToString();
            var codeDescription = drDiagnosticStudy["ICDDescription"].ToString();
            var valueset = drDiagnosticStudy["valueset"].ToString();
            var patientId = drDiagnosticStudy["PatientId"].ToString();
            var condition = drDiagnosticStudy["Condition"].ToString();
            var negationValueset = "";
            var negationCodeValue = "";
            if (valueset.IndexOf('|') > -1)
            {
                negationValueset = drDiagnosticStudy["valueset"].ToString().Split('|')[1];
                valueset = drDiagnosticStudy["valueset"].ToString().Split('|')[0];

            }
            if (codeValue != "" && codeValue.IndexOf('|') > -1)
            {
                codeValue = drDiagnosticStudy["ICD"].ToString().Split('|')[0];
                negationCodeValue = drDiagnosticStudy["ICD"].ToString().Split('|')[1];

            }
            // _dsPatientDataSection = GetPatientDataSection_CategoryOne(Convert.ToInt64(patientId), "", codeValue);


            //var result = _dsPatientDataSection.LabOrder.Rows[0][_dsPatientDataSection.LabOrder.ResultColumn.ColumnName].ToString();
            // var unit = _dsPatientDataSection.LabOrder.Rows[0][_dsPatientDataSection.LabOrder.UnitColumn.ColumnName].ToString();
            if (condition.IndexOf("not done:") > -1)
            {
                var objLabOrder = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.RQO,
                        negationInd = true,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.44" , extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.17" , extension = "2016-02-01"},

                    },
                        id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            nullFlavor = "NA",
                            valueSet = negationValueset
                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Diagnostic Study, Performed: " + codeDescription }
                        },
                        statusCode = new CS()
                        {
                            code = "active"

                        },
                        author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                       nullFlavor = "NA", root =  Guid.NewGuid().ToString()
                                    }
                                }

                            }
                        }
                    },
                        entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                effectiveTime = new IVL_TS()
                            {
                                ItemsElementName = new[]
                            {
                            ItemsChoiceType2.low
                        },
                                Items = new QTY[]
                            {

                            new IVXB_TS
                            {
                                value =  DateTime.Now.ToString("yyyyMMddHHmmss")
                            }
                            }
                            },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = negationCodeValue,

                                        codeSystem = "2.16.840.1.113883.6.96",
                                       valueSet = valueset
                                    }
                                }
                            }
                        }
                    }

                    }
                };
                return objLabOrder;
            }
            else
            {
                var objLabOrder = new Entry()
                {
                    Item = new Observation()
                    {
                        classCode = "OBS",
                        moodCode = x_ActMoodDocumentObservation.EVN,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.13" , extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.18" , extension = "2016-02-01"},

                    },
                        id = new List<II>
                    {
                        new II {root = "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            code = codeValue,
                            codeSystem = "2.16.840.1.113883.6.1",
                            valueSet = valueset
                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Diagnostic Study, Performed: " + codeDescription }
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
                                        Convert.ToDateTime(drDiagnosticStudy["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drDiagnosticStudy["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drDiagnosticStudy["EndDate"].ToString()).ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drDiagnosticStudy["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            }
                            }
                        },
                        value = new List<ANY>()
                    {
                        new PQ()
                        {
                            nullFlavor = "UNK"
                        }
                    }
                    }
                };
                return objLabOrder;
            }
        }

        #region Intervention Performed
        private static Entry SetInterventionPerformedOld(DataRow drInterventionPerformed)
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
                        codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                    : codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
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

        private static Entry SetInterventionPerformed(DataRow drInterventionPerformed)
        {
            var codeValue = drInterventionPerformed["ICD"].ToString();
            var codeDescription = drInterventionPerformed["ICDDescription"].ToString();
            var valueset = drInterventionPerformed["valueset"].ToString();
            var codeSystemName = drInterventionPerformed["Title"].ToString();
            interventionExtension = Guid.NewGuid().ToString();
            var objInterventionPerformed = new Entry()
            {
                Item = new Act()
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.12", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.32", extension = "2016-02-01"}
                    },
                    id = new List<II>
                    {
                        new II {root= "1.3.6.1.4.1.115", extension = interventionExtension }


                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                    : codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                    : codeSystemName.Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                    : codeSystemName.Contains("ICD") ? "2.16.840.1.113883.6.4"
                                    : codeSystemName.Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                    : codeSystemName.Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                        valueSet = valueset,
                        originalText = new ED
                        {
                            Text = new List<string>
                            {
                                "Intervention, Performed: " + codeDescription
                            }
                        },
                        translation = new List<CD>
                        {
                            new CD
                            {
                                code = codeValue,
                                codeSystem =codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                             :codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                             : codeSystemName.Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                             : codeSystemName.Contains("ICD") ? "2.16.840.1.113883.6.4"
                                             : codeSystemName.Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                             : codeSystemName.Contains("ICD10") ? "2.16.840.1.113883.6.90" : ""
                            }
                        }
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
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            }
                        }
                    }
                }
            };
            return objInterventionPerformed;
        }

        private static Entry SetInterventionActive(DataRow drInterventionPerformed)
        {
            var codeValue = drInterventionPerformed["ICD"].ToString();
            var codeDescription = drInterventionPerformed["ICDDescription"].ToString();
            var valueset = drInterventionPerformed["valueset"].ToString();
            var codeSystemName = drInterventionPerformed["Title"].ToString();
            string Reason = "";
            string ReasonComments = "";
            string ReasonValueSet = "";
            if (drInterventionPerformed["ICD"].ToString() != "" && drInterventionPerformed["ICD"].ToString().IndexOf('|') > -1)
            {
                Reason = drInterventionPerformed["ICD"].ToString().Split('|')[1];
                codeValue = drInterventionPerformed["ICD"].ToString().Split('|')[0];
            }
            if (drInterventionPerformed["ICDDescription"].ToString() != "" && drInterventionPerformed["ICDDescription"].ToString().IndexOf('|') > -1)
            {
                ReasonComments = drInterventionPerformed["ICDDescription"].ToString().Split('|')[1];
                codeDescription = drInterventionPerformed["ICDDescription"].ToString().Split('|')[0];
            }
            if (drInterventionPerformed["valueset"].ToString() != "" && drInterventionPerformed["valueset"].ToString().IndexOf('|') > -1)
            {
                ReasonValueSet = drInterventionPerformed["valueset"].ToString().Split('|')[1];
                valueset = drInterventionPerformed["valueset"].ToString().Split('|')[0];
            }

            if (ReasonValueSet == "" && Reason == "198941007")
            {
                ReasonValueSet = "2.16.840.1.113883.3.600.2395";
            }
            if (drInterventionPerformed["Condition"].ToString().IndexOf("not done:") > -1)
            {
                var objInterventionPerformed = new Entry()
                {
                    Item = new Act()
                    {
                        classCode = x_ActClassDocumentEntryAct.ACT,
                        moodCode = x_DocumentActMood.RQO,
                        negationInd = true,
                        templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.39", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.31", extension = "2016-02-01"}
                    },
                        id = new List<II>
                    {
                        new II {root= "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                        code = new CD()
                        {
                            nullFlavor = "NA",

                            valueSet = ReasonValueSet,


                        },
                        text = new ED()
                        {
                            Text = new List<string> { "Intervention, Order: " + codeDescription }
                        },
                        statusCode = new CS()
                        {
                            code = "active"

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
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            }
                                        }
                        },
                        author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }

                            }
                        }
                    },
                        entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
                                },
                                statusCode = new CS()
                                {
                                    code = "completed"
                                },
                                effectiveTime = new IVL_TS()
                            {
                                ItemsElementName = new[]
                            {
                            ItemsChoiceType2.low
                        },
                                Items = new QTY[]
                            {

                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            }
                            }
                            },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = Reason,

                                        codeSystem = "2.16.840.1.113883.6.96",
                                       valueSet = valueset
                                    }
                                }
                            }
                        }
                    }
                    }
                };
                return objInterventionPerformed;
            }
            else
            {

                if (drInterventionPerformed["ICD"].ToString().IndexOf('|') > -1 && !string.IsNullOrEmpty(drInterventionPerformed["ICD"].ToString().Split('|')[1]) && drInterventionPerformed["ICD"].ToString().Split('|')[1] != "0")
                {

                    var objInterventionPerformed = new Entry()
                    {
                        Item = new Act()
                        {
                            classCode = x_ActClassDocumentEntryAct.ACT,
                            moodCode = x_DocumentActMood.RQO,
                            templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.39", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.31", extension = "2016-02-01"}
                    },
                            id = new List<II>
                    {
                        new II {root= "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                            code = new CD()
                            {
                                code = codeValue,
                                codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                        : codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                        : codeSystemName.Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                        : codeSystemName.Contains("ICD") ? "2.16.840.1.113883.6.4"
                                        : codeSystemName.Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                        : codeSystemName.Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                                valueSet = valueset,
                                originalText = new ED
                                {
                                    Text = new List<string>
                            {
                                "Intervention, Order: " + codeDescription
                            }
                                },
                                translation = new List<CD>
                        {
                            new CD
                            {
                                code = codeValue,
                                codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                             : codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                             : codeSystemName.Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                             : codeSystemName.Contains("ICD") ? "2.16.840.1.113883.6.4"
                                             : codeSystemName.Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                             : codeSystemName.Contains("ICD10") ? "2.16.840.1.113883.6.90" : ""
                            }
                        }
                            },
                            text = new ED()
                            {
                                Text = new List<string> { "Intervention, Order: " + codeDescription }
                            },
                            statusCode = new CS()
                            {
                                code = "active"

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
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            }
                            }
                            },
                            author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }

                            }
                        }
                    },
                            entryRelationship = new List<EntryRelationship>()
                        {
                        new EntryRelationship()
                        {
                            typeCode = x_ActRelationshipEntryRelationship.RSON,
                            Item = new Observation()
                            {
                                classCode = "OBS",
                                moodCode = x_ActMoodDocumentObservation.EVN,
                                templateId = new List<II>()
                                {
                                    new II()
                                    {
                                        root = "2.16.840.1.113883.10.20.24.3.88",
                                        extension = "2014-12-01"

                                    }
                                },
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root = "1.3.6.1.4.1.115",
                                        extension = Guid.NewGuid().ToString()
                                    }
                                },
                                code = new CD()
                                {
                                    code = "77301-0",
                                    codeSystem = "2.16.840.1.113883.6.1",
                                    codeSystemName = "LOINC",
                                    displayName = "reason"
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
                                value = DateTime.Now.ToString("yyyyMMddHHmmss")

                            }
                          }
                        },
                                value = new List<ANY>()
                                {
                                    new CD()
                                    {
                                        code = Reason,
                                        displayName = ReasonComments == "" ? "SNOMED" : ReasonComments,
                                        codeSystem = "2.16.840.1.113883.6.96",
                                        valueSet = ReasonValueSet
                                    }
                                }
                            }
                        }
                    }
                        }
                    };
                    return objInterventionPerformed;
                }
                else
                {

                    var objInterventionPerformed = new Entry()
                    {
                        Item = new Act()
                        {
                            classCode = x_ActClassDocumentEntryAct.ACT,
                            moodCode = x_DocumentActMood.RQO,
                            templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.22.4.39", extension = "2014-06-09"},
                        new II {root = "2.16.840.1.113883.10.20.24.3.31", extension = "2016-02-01"}
                    },
                            id = new List<II>
                    {
                        new II {root= "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                            code = new CD()
                            {
                                code = codeValue,
                                codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                            : codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                            : codeSystemName.Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                            : codeSystemName.Contains("ICD") ? "2.16.840.1.113883.6.4"
                                            : codeSystemName.Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                            : codeSystemName.Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                                valueSet = valueset,
                                originalText = new ED
                                {
                                    Text = new List<string>
                            {
                                "Intervention, Order: " + codeDescription
                            }
                                },
                                translation = new List<CD>
                        {
                            new CD
                            {
                                code = codeValue,
                                codeSystem = codeValue.Contains("-") ? "2.16.840.1.113883.6.1"
                                             : codeSystemName.Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                             : codeSystemName.Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                             : codeSystemName.Contains("ICD") ? "2.16.840.1.113883.6.4"
                                             : codeSystemName.Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                             : codeSystemName.Contains("ICD10") ? "2.16.840.1.113883.6.90" : ""
                            }
                        }
                            },
                            text = new ED()
                            {
                                Text = new List<string> { "Intervention, Order: " + codeDescription }
                            },
                            statusCode = new CS()
                            {
                                code = "active"

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
                                value = string.IsNullOrEmpty( Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["StartDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value = string.IsNullOrEmpty(Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()).ToString("yyyyMMddHHmmss")) ? DateTime.Now.ToString("yyyyMMddHHmmss") : Convert.ToDateTime(drInterventionPerformed["EndDate"].ToString()) .ToString("yyyyMMddHHmmss")
                            }
                                }
                            },
                            author = new List<Author>
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
                                value = DateTime.Now.AddYears(-1).ToString("yyyyMMddHHmmss")
                            },
                            assignedAuthor = new AssignedAuthor
                            {
                                id = new List<II>
                                {
                                    new II
                                    {
                                        root =  Guid.NewGuid().ToString()
                                    }
                                }
                                //code = new CE
                                //{
                                //    code = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderTaxonomyCodeColumn.ColumnName].ToString(),
                                //    codeSystem = "2.16.840.1.113883.6.101",
                                //    displayName = drMedicationOrder[_dsPatientDataSection.MedicationOrder.ProviderSpecialityDescriptionColumn.ColumnName].ToString(),
                                //    codeSystemName = "Healthcare Provider Taxonomy (HIPAA)"
                                //}
                            }
                        }
                    }
                        }
                    };
                    return objInterventionPerformed;
                }
            }
        }
        #endregion

        #region Provider To Provider
        private static Entry SetProviderToProvider(DataRow drProviderToProvider)
        {
            var codeValue = string.IsNullOrEmpty(drProviderToProvider["ICD"].ToString()) ? "371530004" : drProviderToProvider["ICD"].ToString();
            var codeDescription = drProviderToProvider["ICDDescription"].ToString();
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
                        new II {root = "2.16.840.1.113883.10.20.24.3.4", extension = "2016-02-01"}
                    },
                    id = new List<II>
                    {
                        new II {root="1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = codeSystemName,
                        valueSet = valueset
                    },
                    text = new ED()
                    {
                        Text = new List<string> { "Communication From Provider to Provider: " + codeDescription }
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

                    },
                    inFulfillmentOf1 = new List<InFulfillmentOf1>()
                    {
                        new InFulfillmentOf1(){
                           typeCode =   ActRelationshipFulfills.FLFS,
                          templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.126", extension = "2014-12-01"}
                    },
                    actReference = new ActReference(){
                       classCode = "ACT",
                       moodCode = x_DocumentActMood.EVN,
                       id = new List<II>
                    {
                        new II {root="1.3.6.1.4.1.115", extension = interventionExtension}
                    }
                   }

              }
             }
                }
            };

            return objdrProviderToProvider;
        }
        #endregion

        private static Entry SetPatientToProvider(DataRow drPatientToProvider)
        {
            var codeValue = drPatientToProvider["ICD"].ToString();
            var codeDescription = drPatientToProvider["ICDDescription"].ToString();
            var valueset = drPatientToProvider["valueset"].ToString();
            var condition = drPatientToProvider["Condition"].ToString();

            var objdrPatientToProvider = new Entry()
            {
                Item = new Act()
                {
                    classCode = x_ActClassDocumentEntryAct.ACT,
                    moodCode = x_DocumentActMood.EVN,
                    templateId = new List<II>
                    {
                        new II {root = "2.16.840.1.113883.10.20.24.3.2", extension = "2016-02-01"}
                    },
                    id = new List<II>
                    {
                        new II {root= "1.3.6.1.4.1.115", extension = Guid.NewGuid().ToString()}
                    },
                    code = new CD()
                    {
                        code = codeValue,
                        codeSystem = drPatientToProvider["Title"].ToString().Contains("SNOMED") ? "2.16.840.1.113883.6.96"
                                    : drPatientToProvider["Title"].ToString().Contains("CPTCode") ? "2.16.840.1.113883.6.12"
                                    : drPatientToProvider["Title"].ToString().Contains("ICD9") ? "2.16.840.1.113883.6.103"
                                    : drPatientToProvider["Title"].ToString().Contains("Loinc") ? "2.16.840.1.113883.6.1"
                                    : drPatientToProvider["Title"].ToString().Contains("ICD10") ? "2.16.840.1.113883.6.90" : "",
                        valueSet = valueset
                    },
                    text = new ED()
                    {
                        Text = new List<string> { "Communication From Patient to Provider: " + codeDescription }
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
                                        Convert.ToDateTime(drPatientToProvider["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drPatientToProvider["StartDate"].ToString())
                                            .ToString("yyyyMMddHHmmss")
                            },
                            new IVXB_TS
                            {
                                value =
                                    string.IsNullOrEmpty(
                                        Convert.ToDateTime(drPatientToProvider["EndDate"].ToString())
                                            .ToString("yyyyMMddHHmmss"))
                                        ? DateTime.Now.ToString("yyyyMMddHHmmss")
                                        : Convert.ToDateTime(drPatientToProvider["EndDate"].ToString())
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
                                classCode = "PAT",
                                code = new CE()
                                {
                                    code = "116154003",
                                    codeSystem = "2.16.840.1.113883.6.96",
                                    codeSystemName = "SNOMED CT",
                                    displayName = "Patient"
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

            return objdrPatientToProvider;
        }

        #endregion

        #endregion

        #endregion
    }
}