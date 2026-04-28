using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using iTextSharp.text;
using MDVision.Datasets;
using MDVision.Business.BLL;

namespace MDVision.IEHR.Controls.Batch.CQM2017
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

        private static AmalgamatedClinicalDocument _document;

        private StructuredBody StructuredBody
        {
            get { return _document.component.Item as StructuredBody; }
        }

        public void BuildSectionLevelTemplate(AmalgamatedClinicalDocument document, DSCQM dsPatientDemoGraphic,
            DSProfile dsProvider, DSProfile dsPractice,
            string cqmid, Int64 patientId, Int64 providerId, Int64 practiceId, string startDate, string endDate, DSCQM dsMeasureSection)
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
            _dsPatientDataSectionCodes = PatientDataSectionCodes_CategoryOne(providerId, startDate, endDate, patientId.ToString(), 2, cqmid);

            #endregion

            #endregion

            SectionLevelTemplates(startDate, endDate, cqmid);
        }

        private void SectionLevelTemplates(string startDate, string endDate, string cqmid)
        {
            ReportingParameterSection(startDate, endDate);
            MeasureSection(cqmid);
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
        private static DSCQM GetPatientDataSection_CategoryOne(Int64 patientId, string nqfId)
        {
            var obj = new BLLCQM().PatientDataSection(patientId, nqfId);
            _dsPatientDataSection = obj.Data;
            return _dsPatientDataSection;
        }

        private static DSCQM PatientDataSectionCodes_CategoryOne(long providerId, string from, string to,
            string patientId = null, long reportType = 2, string cqmId = null)
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

            var obj = new BLLCQM().Load_CQM_Codes(providerId, from, to, patientId, reportType, cqmId, measurePart
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
            {
                cqmid = "0421a";
            }
            DataView dvMeasureSection = new DataView(_dsMeasureSection.Tables[_dsMeasureSection.CQM.TableName])
            {
                RowFilter = "[CQMID] = '" + cqmid + "'"
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
                                            "Reporting period: " +
                                            DateTime.Parse(startDate).ToString("yyyyMMdd")
                                            + " - " +
                                            DateTime.Parse(endDate).ToString("yyyyMMdd")
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
            }

            if (cqmid == "0022")
            {
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0018")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                PyhsicalExam(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0028")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationOrder(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
                TobbacoUser(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0075")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                LabOrder(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0419")
            {
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0421")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                PhysicalExam_421(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0043")
            {
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationAdministered_CVX(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0068")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                MedicationActive(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0418")
            {
                DiagnosisActiveConcernAct(objStrucDocTbody, objComponent3);
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                RiskCategoryAssessment(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "0041")
            {
                EncounterPerformed_Any(objStrucDocTbody, objComponent3);
                ProcedurePerformed(objStrucDocTbody, objComponent3);
            }
            else if (cqmid == "CMS50v3")
            {
                EncounterPerformed_OnlyCPT(objStrucDocTbody, objComponent3);
                InterventionPerformed(objStrucDocTbody, objComponent3);
                ProviderToProvider(objStrucDocTbody, objComponent3);
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
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "SNOMED"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
                        "Po1");

            if (problemCodesRowCollection.Any())
            {
                var dtProblemCode = problemCodesRowCollection.CopyToDataTable();

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

                    objComponent3.section.entry.Add(SetDiagnosisActiveConcernAct(dtProblemCodes.Rows[i]));
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
                    "StartDate", "EndDate", "valueset");

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

            //    objComponent3.section.entry.Add(
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
                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Medication Allergy: ",
                                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.MedicationAllergy.AllergenColumn].ToString(),
                                onSetDate
                            )
                    );

                objComponent3.section.entry.Add(SetMedicationAllergy(_dsPatientDataSection.Tables[tableName].Rows[i]));
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
                var dtMedicationOrderRe = dvMedicationOrder.ToTable(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate", "EndDate", "valueset");

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

            //    objComponent3.section.entry.Add(SetMedicationOrder(_dsPatientDataSection.Tables[tableName].Rows[i]));
            //}
        }
        private static void EncounterPerformed_Any(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes == null)
                return;
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Procedure_N");
            //&& r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "CPTCode"
            //&& r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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
        private static void EncounterPerformed_OnlyCPT(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName].Rows.Count <= 0)
                return;

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Procedure_N"
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
                .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure_N"
            && r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
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

            var proceduresCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(
                    r =>
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Procedure_W");
            //&& r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "SNOMED"
            //&& r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");

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
                                    "Procedure Performed: " +
                                    dtProcedureCodes.Rows[i]["ICDDescription"],
                                    dtProcedureCodes.Rows[i]["ICDDescription"].ToString(),
                                    procedureStartDate
                                )
                        );
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

            var riskCategoryAssessmentCodesRowCollection = _dsPatientDataSectionCodes.Tables[
                _dsPatientDataSectionCodes.CQMCodes.TableName].AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) == "Loinc" &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Lab" &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) != "Po1");
            //r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure"
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
                        SetRiskCategoryAssessment(dtProcedureCodes.Rows[i]));
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

                objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Diastolic Blood Pressure", "2.16.840.1.113883.3.526.3.1033", diastolicBpLoincCode, diastolicBpValue));
                objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "Systolic Blood Pressure", "2.16.840.1.113883.3.526.3.1032", systolicBpLoincCode, systolicBpValue));
            }
        }
        private static void PhysicalExam_421(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            if (_dsPatientDataSection == null)
                return;
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
                                "Physical Exam, Finding: BMI LOINC Value",
                                "BMI LOINC Value",
                                physicalExamStartDate
                            )
                    );

                var bmiLoinc = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.LOINICBMIColumn].ToString();
                var bmi = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.PhysicalExam.BMIColumn].ToString();

                objComponent3.section.entry.Add(SetPhysicalExam(_dsPatientDataSection.Tables[tableName].Rows[i], "BMI LOINC Value", "2.16.840.1.113883.3.600.1.681", bmiLoinc, bmi));

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
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) ==
                        "Lab"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.TitleColumn.ColumnName.ToString()) ==
                        "Loinc"
                        &&
                        r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataFromColumn.ColumnName.ToString()) !=
                        "Po1");

            if (labOrderRowCollection.Any())
            {
                var dtLabOrder = labOrderRowCollection.CopyToDataTable();

                var dvLabOrder = new DataView(dtLabOrder);
                var dtLabOrderRe = dvLabOrder.ToTable(true, "ICD", "ICDDescription", "Title", "DataType", "StartDate",
                    "EndDate", "valueset");

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
        private static void InterventionPerformed(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSectionCodes.CQMCodes.TableName;
            if (_dsPatientDataSectionCodes.Tables[tableName].Rows.Count <= 0) return;

            var interventionPerformedRowCollection = _dsPatientDataSectionCodes.Tables[_dsPatientDataSectionCodes.CQMCodes.TableName]
                .AsEnumerable()
                .Where(r => r.Field<string>(_dsPatientDataSectionCodes.CQMCodes.DataTypeColumn.ColumnName.ToString()) == "Procedure_N"
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
        private static void ProviderToProvider(StrucDocTbody objStrucDocTbody, Component3 objComponent3)
        {
            var tableName = _dsPatientDataSection.ProviderToProvider.TableName;
            if (_dsPatientDataSection.Tables[tableName].Rows.Count <= 0) return;

            for (var i = 0; i < _dsPatientDataSection.Tables[tableName].Rows.Count; i++)
            {
                var providerToProviderStartDate = _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.StartDateColumn].ToString() == ""
                    ? DateTime.Now.ToString("yyyyMMdd") :
                    DateTime.Parse(_dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.StartDateColumn].ToString()).ToString("yyyyMMdd");

                objStrucDocTbody.tr.Add
                    (
                        GetPatientDataSection_TRs
                            (
                                "Communication: From Provider to Provider: " + _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.SNOMEDDescriptionColumn],
                                _dsPatientDataSection.Tables[tableName].Rows[i][_dsPatientDataSection.ProviderToProvider.SNOMEDDescriptionColumn].ToString(),
                                providerToProviderStartDate
                            )
                    );

                objComponent3.section.entry.Add(SetProviderToProvider(_dsPatientDataSection.Tables[tableName].Rows[i]));
            }
        }

        #endregion

        #region Patient Data Section Set'em

        #region Diagnosis Active Concern Act
        private static Entry SetDiagnosisActiveConcernAct(DataRow drDiagnosisActiveConcernAct)
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
            //if (drFamilyHx == null) throw new ArgumentNullException(nameof(drFamilyHx));

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
                            Items = new QTY[]
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