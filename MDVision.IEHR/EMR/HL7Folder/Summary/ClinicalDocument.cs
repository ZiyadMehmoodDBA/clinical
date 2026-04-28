using System.Collections.Generic;
using System.Xml.Serialization;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Summary
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ClinicalDocument", Namespace = "urn:hl7-org:v3")]
    [System.Xml.Serialization.XmlRootAttribute("ClinicalDocument", Namespace = "urn:hl7-org:v3", IsNullable = false)]
    public partial class ClinicalDocument
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private II idField;

        private CE codeField;

        private ST titleField;

        private TS effectiveTimeField;

        private CE confidentialityCodeField;

        private CS languageCodeField;

        private II setIdField;

        private INT versionNumberField;

        private TS copyTimeField;

        private List<RecordTarget> recordTargetField;

        private List<Author> authorField;

        private DataEnterer dataEntererField;

        private List<Informant12> informantField;

        private Custodian custodianField;

        private List<InformationRecipient> informationRecipientField;

        private LegalAuthenticator legalAuthenticatorField;

        private List<Authenticator> authenticatorField;

        private List<Participant1> participantField;

        private List<InFulfillmentOf> inFulfillmentOfField;

        private List<DocumentationOf> documentationOfField;

        private List<RelatedDocument> relatedDocumentField;

        private List<Authorization> authorizationField;

        private Component1 componentOfField;

        private Component2 componentField;

        private string nullFlavorField;

        private ActClinicalDocument classCodeField;

        private bool classCodeFieldSpecified;

        private string moodCodeField;

        public ClinicalDocument()
        {
            this.classCodeField = ActClinicalDocument.DOCCLIN;
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public II id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ST title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public CE confidentialityCode
        {
            get
            {
                return this.confidentialityCodeField;
            }
            set
            {
                this.confidentialityCodeField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        public II setId
        {
            get
            {
                return this.setIdField;
            }
            set
            {
                this.setIdField = value;
            }
        }

        /// <remarks/>
        public INT versionNumber
        {
            get
            {
                return this.versionNumberField;
            }
            set
            {
                this.versionNumberField = value;
            }
        }

        /// <remarks/>
        public TS copyTime
        {
            get
            {
                return this.copyTimeField;
            }
            set
            {
                this.copyTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("recordTarget")]
        public List<RecordTarget> recordTarget
        {
            get
            {
                return this.recordTargetField;
            }
            set
            {
                this.recordTargetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        public DataEnterer dataEnterer
        {
            get
            {
                return this.dataEntererField;
            }
            set
            {
                this.dataEntererField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        public Custodian custodian
        {
            get
            {
                return this.custodianField;
            }
            set
            {
                this.custodianField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informationRecipient")]
        public List<InformationRecipient> informationRecipient
        {
            get
            {
                return this.informationRecipientField;
            }
            set
            {
                this.informationRecipientField = value;
            }
        }

        /// <remarks/>
        public LegalAuthenticator legalAuthenticator
        {
            get
            {
                return this.legalAuthenticatorField;
            }
            set
            {
                this.legalAuthenticatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("authenticator")]
        public List<Authenticator> authenticator
        {
            get
            {
                return this.authenticatorField;
            }
            set
            {
                this.authenticatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant1> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inFulfillmentOf")]
        public List<InFulfillmentOf> inFulfillmentOf
        {
            get
            {
                return this.inFulfillmentOfField;
            }
            set
            {
                this.inFulfillmentOfField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("documentationOf")]
        public List<DocumentationOf> documentationOf
        {
            get
            {
                return this.documentationOfField;
            }
            set
            {
                this.documentationOfField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("relatedDocument")]
        public List<RelatedDocument> relatedDocument
        {
            get
            {
                return this.relatedDocumentField;
            }
            set
            {
                this.relatedDocumentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("authorization")]
        public List<Authorization> authorization
        {
            get
            {
                return this.authorizationField;
            }
            set
            {
                this.authorizationField = value;
            }
        }

        /// <remarks/>
        public Component1 componentOf
        {
            get
            {
                return this.componentOfField;
            }
            set
            {
                this.componentOfField = value;
            }
        }

        /// <remarks/>
        public Component2 component
        {
            get
            {
                return this.componentField;
            }
            set
            {
                this.componentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActClinicalDocument classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classCodeSpecified
        {
            get
            {
                return this.classCodeFieldSpecified;
            }
            set
            {
                this.classCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class CS : CV
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PQR))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class CV : CE
    {
    }
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HXIT_CE))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVLevent))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CV))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PQR))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    [System.Xml.Serialization.XmlRootAttribute("ethnicGroupCode", Namespace = "urn:hl7-org:sdtc", IsNullable = false)]
    public partial class DE : CD
    {
    }
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HXIT_CE))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVLevent))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CV))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PQR))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    [System.Xml.Serialization.XmlRootAttribute("raceCode", Namespace = "urn:hl7-org:sdtc", IsNullable = false)]
    public partial class CE : CD
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BXIT_CD))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_CD))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CE))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HXIT_CE))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVLevent))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CV))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PQR))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class CD : ANY
    {

        private ED originalTextField;

        private List<CR> qualifierField;

        private List<CD> translationField;

        private string codeField;

        private string codeSystemField;

        private string codeSystemNameField;

        private string codeSystemVersionField;

        private string displayNameField;

        private string valueSetField;

        private string valueSet1Field;

        private string valueSetVersionField;

        private string valueSetVersion1Field;
        /// <remarks/>
        public ED originalText
        {
            get
            {
                return this.originalTextField;
            }
            set
            {
                this.originalTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("qualifier")]
        public List<CR> qualifier
        {
            get
            {
                return this.qualifierField;
            }
            set
            {
                this.qualifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("translation")]
        public List<CD> translation
        {
            get
            {
                return this.translationField;
            }
            set
            {
                this.translationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codeSystem
        {
            get
            {
                return this.codeSystemField;
            }
            set
            {
                this.codeSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codeSystemName
        {
            get
            {
                return this.codeSystemNameField;
            }
            set
            {
                this.codeSystemNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codeSystemVersion
        {
            get
            {
                return this.codeSystemVersionField;
            }
            set
            {
                this.codeSystemVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:hl7-org:sdtc")]
        public string valueSet
        {
            get
            {
                return this.valueSetField;
            }
            set
            {
                this.valueSetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:hl7-org:sdtc")]
        public string valueSetVersion
        {
            get
            {
                return this.valueSetVersionField;
            }
            set
            {
                this.valueSetVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ST))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ENXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ensuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enprefix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(engiven))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enfamily))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(endelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ADXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpprecinct))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostBox))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpbuildingNumberSuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryModeIdentifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryMode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationQualifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationArea))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcensusTract))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcareOf))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitID))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpadditionalLocator))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetName))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdirection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumberNumeric))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumber))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostalCode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcounty))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstate))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcountry))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SC))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(thumbnail))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class ED : BIN
    {

        private TEL referenceField;

        private thumbnail thumbnailField;

        private string mediaTypeField;

        private string languageField;

        private CompressionAlgorithm compressionField;

        private bool compressionFieldSpecified;

        private byte[] integrityCheckField;

        private IntegrityCheckAlgorithm integrityCheckAlgorithmField;

        public ED()
        {
            this.mediaTypeField = "text/plain";
            this.integrityCheckAlgorithmField = IntegrityCheckAlgorithm.SHA1;
        }

        /// <remarks/>
        public TEL reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        public thumbnail thumbnail
        {
            get
            {
                return this.thumbnailField;
            }
            set
            {
                this.thumbnailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        [System.ComponentModel.DefaultValueAttribute("text/plain")]
        public string mediaType
        {
            get
            {
                return this.mediaTypeField;
            }
            set
            {
                this.mediaTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public CompressionAlgorithm compression
        {
            get
            {
                return this.compressionField;
            }
            set
            {
                this.compressionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool compressionSpecified
        {
            get
            {
                return this.compressionFieldSpecified;
            }
            set
            {
                this.compressionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] integrityCheck
        {
            get
            {
                return this.integrityCheckField;
            }
            set
            {
                this.integrityCheckField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(IntegrityCheckAlgorithm.SHA1)]
        public IntegrityCheckAlgorithm integrityCheckAlgorithm
        {
            get
            {
                return this.integrityCheckAlgorithmField;
            }
            set
            {
                this.integrityCheckAlgorithmField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class TEL : URL
    {

        private List<SXCM_TS> useablePeriodField;

        private List<string> useField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("useablePeriod")]
        public List<SXCM_TS> useablePeriod
        {
            get
            {
                return this.useablePeriodField;
            }
            set
            {
                this.useablePeriodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public List<string> use
        {
            get
            {
                return this.useField;
            }
            set
            {
                this.useField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXPR_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_TS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_TS : TS
    {

        private SetOperator operatorField;

        public SXCM_TS()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum SetOperator
    {

        /// <remarks/>
        A,

        /// <remarks/>
        E,

        /// <remarks/>
        H,

        /// <remarks/>
        I,

        /// <remarks/>
        P,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UVP_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXPR_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_TS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    [System.Xml.Serialization.XmlRootAttribute("deceasedTime", Namespace = "urn:hl7-org:sdtc", IsNullable = false)]
    public partial class TS : QTY
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO_MO_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO_PQ_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO_QTY_QTY))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegionOfInterestvalue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HXIT_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BXIT_IVL_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UVP_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXPR_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_TS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public abstract partial class QTY : ANY
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GLIST_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GLIST_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SLIST_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SLIST_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EN))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TN))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ON))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PN))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AD))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(II))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InfrastructureRoottypeId))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CR))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CD))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BXIT_CD))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_CD))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CE))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HXIT_CE))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVLevent))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CV))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PQR))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(QTY))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO_MO_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO_PQ_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO_QTY_QTY))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegionOfInterestvalue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HXIT_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BXIT_IVL_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UVP_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXPR_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(URL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TEL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BIN))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ED))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ST))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ENXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ensuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enprefix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(engiven))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enfamily))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(endelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ADXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpprecinct))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostBox))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpbuildingNumberSuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryModeIdentifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryMode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationQualifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationArea))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcensusTract))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcareOf))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitID))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpadditionalLocator))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetName))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdirection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumberNumeric))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumber))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostalCode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcounty))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstate))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcountry))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SC))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(thumbnail))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ANYNonNull))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BN))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BL))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public abstract partial class ANY
    {

        private string nullFlavorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Component5", Namespace = "urn:hl7-org:v3")]
    public partial class Component5
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private Section sectionField;

        private string nullFlavorField;

        private ActRelationshipHasComponent typeCodeField;

        private bool typeCodeFieldSpecified;

        private bool contextConductionIndField;

        private bool contextConductionIndFieldSpecified;

        public Component5()
        {
            this.typeCodeField = ActRelationshipHasComponent.COMP;
            this.contextConductionIndField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public Section section
        {
            get
            {
                return this.sectionField;
            }
            set
            {
                this.sectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActRelationshipHasComponent typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool contextConductionInd
        {
            get
            {
                return this.contextConductionIndField;
            }
            set
            {
                this.contextConductionIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool contextConductionIndSpecified
        {
            get
            {
                return this.contextConductionIndFieldSpecified;
            }
            set
            {
                this.contextConductionIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "InfrastructureRoot.typeId", Namespace = "urn:hl7-org:v3")]
    public partial class InfrastructureRoottypeId : II
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InfrastructureRoottypeId))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    [System.Xml.Serialization.XmlRootAttribute("id", Namespace = "urn:hl7-org:sdtc", IsNullable = false)]
    public partial class II : ANY
    {

        private string rootField;

        private string extensionField;

        private string assigningAuthorityNameField;

        private bool displayableField;

        private bool displayableFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string root
        {
            get
            {
                return this.rootField;
            }
            set
            {
                this.rootField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string extension
        {
            get
            {
                return this.extensionField;
            }
            set
            {
                this.extensionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assigningAuthorityName
        {
            get
            {
                return this.assigningAuthorityNameField;
            }
            set
            {
                this.assigningAuthorityNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool displayable
        {
            get
            {
                return this.displayableField;
            }
            set
            {
                this.displayableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool displayableSpecified
        {
            get
            {
                return this.displayableFieldSpecified;
            }
            set
            {
                this.displayableFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Section", Namespace = "urn:hl7-org:v3")]
    public partial class Section
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private II idField;

        private CE codeField;

        private ST titleField;

        private StrucDocText textField;

        private CE confidentialityCodeField;

        private CS languageCodeField;

        private Subject subjectField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Entry> entryField;

        private List<Component5> componentField;

        private string idField1;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public Section()
        {
            this.classCodeField = "DOCSECT";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public II id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ST title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public StrucDocText text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CE confidentialityCode
        {
            get
            {
                return this.confidentialityCodeField;
            }
            set
            {
                this.confidentialityCodeField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entry")]
        public List<Entry> entry
        {
            get
            {
                return this.entryField;
            }
            set
            {
                this.entryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("component")]
        public List<Component5> component
        {
            get
            {
                return this.componentField;
            }
            set
            {
                this.componentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField1;
            }
            set
            {
                this.idField1 = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ENXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ensuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enprefix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(engiven))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enfamily))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(endelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ADXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpprecinct))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostBox))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpbuildingNumberSuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryModeIdentifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryMode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationQualifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationArea))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcensusTract))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcareOf))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitID))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpadditionalLocator))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetName))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdirection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumberNumeric))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumber))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostalCode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcounty))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstate))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcountry))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SC))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class ST : ED
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ensuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enprefix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(engiven))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enfamily))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(endelimiter))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class ENXP : ST
    {

        private string partTypeField;

        private List<string> qualifierField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string partType
        {
            get
            {
                return this.partTypeField;
            }
            set
            {
                this.partTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public List<string> qualifier
        {
            get
            {
                return this.qualifierField;
            }
            set
            {
                this.qualifierField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "en.suffix", Namespace = "urn:hl7-org:v3")]
    public partial class ensuffix : ENXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "en.prefix", Namespace = "urn:hl7-org:v3")]
    public partial class enprefix : ENXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "en.given", Namespace = "urn:hl7-org:v3")]
    public partial class engiven : ENXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "en.family", Namespace = "urn:hl7-org:v3")]
    public partial class enfamily : ENXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "en.delimiter", Namespace = "urn:hl7-org:v3")]
    public partial class endelimiter : ENXP
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpprecinct))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostBox))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpbuildingNumberSuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryModeIdentifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryMode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationQualifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationArea))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcensusTract))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcareOf))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitID))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpadditionalLocator))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetName))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdirection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumberNumeric))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumber))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostalCode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcounty))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstate))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcountry))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdelimiter))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class ADXP : ST
    {

        private string partTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string partType
        {
            get
            {
                return this.partTypeField;
            }
            set
            {
                this.partTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.precinct", Namespace = "urn:hl7-org:v3")]
    public partial class adxpprecinct : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.postBox", Namespace = "urn:hl7-org:v3")]
    public partial class adxppostBox : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.buildingNumberSuffix", Namespace = "urn:hl7-org:v3")]
    public partial class adxpbuildingNumberSuffix : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.deliveryModeIdentifier", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdeliveryModeIdentifier : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.deliveryMode", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdeliveryMode : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.deliveryInstallationQualifier", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdeliveryInstallationQualifier : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.deliveryInstallationArea", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdeliveryInstallationArea : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.deliveryInstallationType", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdeliveryInstallationType : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.deliveryAddressLine", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdeliveryAddressLine : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.censusTract", Namespace = "urn:hl7-org:v3")]
    public partial class adxpcensusTract : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.careOf", Namespace = "urn:hl7-org:v3")]
    public partial class adxpcareOf : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.unitType", Namespace = "urn:hl7-org:v3")]
    public partial class adxpunitType : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.unitID", Namespace = "urn:hl7-org:v3")]
    public partial class adxpunitID : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.additionalLocator", Namespace = "urn:hl7-org:v3")]
    public partial class adxpadditionalLocator : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.streetNameType", Namespace = "urn:hl7-org:v3")]
    public partial class adxpstreetNameType : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.streetNameBase", Namespace = "urn:hl7-org:v3")]
    public partial class adxpstreetNameBase : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.streetName", Namespace = "urn:hl7-org:v3")]
    public partial class adxpstreetName : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.direction", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdirection : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.houseNumberNumeric", Namespace = "urn:hl7-org:v3")]
    public partial class adxphouseNumberNumeric : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.houseNumber", Namespace = "urn:hl7-org:v3")]
    public partial class adxphouseNumber : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.streetAddressLine", Namespace = "urn:hl7-org:v3")]
    public partial class adxpstreetAddressLine : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.postalCode", Namespace = "urn:hl7-org:v3")]
    public partial class adxppostalCode : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.city", Namespace = "urn:hl7-org:v3")]
    public partial class adxpcity : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.county", Namespace = "urn:hl7-org:v3")]
    public partial class adxpcounty : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.state", Namespace = "urn:hl7-org:v3")]
    public partial class adxpstate : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.country", Namespace = "urn:hl7-org:v3")]
    public partial class adxpcountry : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "adxp.delimiter", Namespace = "urn:hl7-org:v3")]
    public partial class adxpdelimiter : ADXP
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SC : ST
    {

        private string codeField;

        private string codeSystemField;

        private string codeSystemNameField;

        private string codeSystemVersionField;

        private string displayNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codeSystem
        {
            get
            {
                return this.codeSystemField;
            }
            set
            {
                this.codeSystemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codeSystemName
        {
            get
            {
                return this.codeSystemNameField;
            }
            set
            {
                this.codeSystemNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codeSystemVersion
        {
            get
            {
                return this.codeSystemVersionField;
            }
            set
            {
                this.codeSystemVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Text", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocText
    {

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private string mediaTypeField;

        public StrucDocText()
        {
            this.mediaTypeField = "text/x-hl7-text+xml";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("br", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("content", typeof(StrucDocContent))]
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("list", typeof(StrucDocList))]
        [System.Xml.Serialization.XmlElementAttribute("paragraph", typeof(StrucDocParagraph))]
        [System.Xml.Serialization.XmlElementAttribute("renderMultiMedia", typeof(StrucDocRenderMultiMedia))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        [System.Xml.Serialization.XmlElementAttribute("table", typeof(StrucDocTable))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mediaType
        {
            get
            {
                return this.mediaTypeField;
            }
            set
            {
                this.mediaTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Content", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocContent
    {

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private StrucDocContentRevised revisedField;

        private bool revisedFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("br", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("content", typeof(StrucDocContent))]
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("renderMultiMedia", typeof(StrucDocRenderMultiMedia))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocContentRevised revised
        {
            get
            {
                return this.revisedField;
            }
            set
            {
                this.revisedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool revisedSpecified
        {
            get
            {
                return this.revisedFieldSpecified;
            }
            set
            {
                this.revisedFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Footnote", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocFootnote
    {

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("br", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("content", typeof(StrucDocContent))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("list", typeof(StrucDocList))]
        [System.Xml.Serialization.XmlElementAttribute("paragraph", typeof(StrucDocParagraph))]
        [System.Xml.Serialization.XmlElementAttribute("renderMultiMedia", typeof(StrucDocRenderMultiMedia))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        [System.Xml.Serialization.XmlElementAttribute("table", typeof(StrucDocTable))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.LinkHtml", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocLinkHtml
    {

        private List<object> itemsField;

        private List<string> textField;

        private string nameField;

        private string hrefField;

        private string relField;

        private string revField;

        private string titleField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rev
        {
            get
            {
                return this.revField;
            }
            set
            {
                this.revField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.FootnoteRef", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocFootnoteRef
    {

        private string idField;

        private string languageField;

        private string styleCodeField;

        private string iDREFField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string IDREF
        {
            get
            {
                return this.iDREFField;
            }
            set
            {
                this.iDREFField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.List", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocList
    {

        private StrucDocCaption captionField;

        private List<StrucDocItem> itemField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private StrucDocListListType listTypeField;

        public StrucDocList()
        {
            this.listTypeField = StrucDocListListType.unordered;
        }

        /// <remarks/>
        public StrucDocCaption caption
        {
            get
            {
                return this.captionField;
            }
            set
            {
                this.captionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public List<StrucDocItem> item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(StrucDocListListType.unordered)]
        public StrucDocListListType listType
        {
            get
            {
                return this.listTypeField;
            }
            set
            {
                this.listTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Caption", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocCaption
    {

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Sub", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocSub
    {

        private List<string> textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Sup", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocSup
    {

        private List<string> textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Item", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocItem
    {

        private StrucDocCaption captionField;

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        /// <remarks/>
        public StrucDocCaption caption
        {
            get
            {
                return this.captionField;
            }
            set
            {
                this.captionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("br", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("content", typeof(StrucDocContent))]
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("list", typeof(StrucDocList))]
        [System.Xml.Serialization.XmlElementAttribute("paragraph", typeof(StrucDocParagraph))]
        [System.Xml.Serialization.XmlElementAttribute("renderMultiMedia", typeof(StrucDocRenderMultiMedia))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        [System.Xml.Serialization.XmlElementAttribute("table", typeof(StrucDocTable))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Paragraph", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocParagraph
    {

        private StrucDocCaption captionField;

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        /// <remarks/>
        public StrucDocCaption caption
        {
            get
            {
                return this.captionField;
            }
            set
            {
                this.captionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("br", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("content", typeof(StrucDocContent))]
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("renderMultiMedia", typeof(StrucDocRenderMultiMedia))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.RenderMultiMedia", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocRenderMultiMedia
    {

        private StrucDocCaption captionField;

        private string referencedObjectField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        /// <remarks/>
        public StrucDocCaption caption
        {
            get
            {
                return this.captionField;
            }
            set
            {
                this.captionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREFS")]
        public string referencedObject
        {
            get
            {
                return this.referencedObjectField;
            }
            set
            {
                this.referencedObjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Table", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocTable
    {

        private StrucDocCaption captionField;

        private List<object> itemsField;

        private StrucDocThead theadField;

        private StrucDocTfoot tfootField;

        private List<StrucDocTbody> tbodyField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private string summaryField;

        private string widthField;

        private string borderField;

        private StrucDocTableFrame frameField;

        private bool frameFieldSpecified;

        private StrucDocTableRules rulesField;

        private bool rulesFieldSpecified;

        private string cellspacingField;

        private string cellpaddingField;

        /// <remarks/>
        public StrucDocCaption caption
        {
            get
            {
                return this.captionField;
            }
            set
            {
                this.captionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("col", typeof(StrucDocCol))]
        [System.Xml.Serialization.XmlElementAttribute("colgroup", typeof(StrucDocColgroup))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        public StrucDocThead thead
        {
            get
            {
                return this.theadField;
            }
            set
            {
                this.theadField = value;
            }
        }

        /// <remarks/>
        public StrucDocTfoot tfoot
        {
            get
            {
                return this.tfootField;
            }
            set
            {
                this.tfootField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tbody")]
        public List<StrucDocTbody> tbody
        {
            get
            {
                return this.tbodyField;
            }
            set
            {
                this.tbodyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string border
        {
            get
            {
                return this.borderField;
            }
            set
            {
                this.borderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTableFrame frame
        {
            get
            {
                return this.frameField;
            }
            set
            {
                this.frameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool frameSpecified
        {
            get
            {
                return this.frameFieldSpecified;
            }
            set
            {
                this.frameFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTableRules rules
        {
            get
            {
                return this.rulesField;
            }
            set
            {
                this.rulesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool rulesSpecified
        {
            get
            {
                return this.rulesFieldSpecified;
            }
            set
            {
                this.rulesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cellspacing
        {
            get
            {
                return this.cellspacingField;
            }
            set
            {
                this.cellspacingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cellpadding
        {
            get
            {
                return this.cellpaddingField;
            }
            set
            {
                this.cellpaddingField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Col", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocCol
    {

        private string idField;

        private string languageField;

        private string styleCodeField;

        private string spanField;

        private string widthField;

        private StrucDocColAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocColValign valignField;

        private bool valignFieldSpecified;

        public StrucDocCol()
        {
            this.spanField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string span
        {
            get
            {
                return this.spanField;
            }
            set
            {
                this.spanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocColAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocColValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocColAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocColValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Colgroup", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocColgroup
    {

        private List<StrucDocCol> colField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private string spanField;

        private string widthField;

        private StrucDocColgroupAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocColgroupValign valignField;

        private bool valignFieldSpecified;

        public StrucDocColgroup()
        {
            this.spanField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("col")]
        public List<StrucDocCol> col
        {
            get
            {
                return this.colField;
            }
            set
            {
                this.colField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string span
        {
            get
            {
                return this.spanField;
            }
            set
            {
                this.spanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocColgroupAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocColgroupValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocColgroupAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocColgroupValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Thead", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocThead
    {

        private List<StrucDocTr> trField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private StrucDocTheadAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocTheadValign valignField;

        private bool valignFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tr")]
        public List<StrucDocTr> tr
        {
            get
            {
                return this.trField;
            }
            set
            {
                this.trField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTheadAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTheadValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Tr", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocTr
    {

        private List<object> itemsField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private StrucDocTrAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocTrValign valignField;

        private bool valignFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("td", typeof(StrucDocTd))]
        [System.Xml.Serialization.XmlElementAttribute("th", typeof(StrucDocTh))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTrAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTrValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Td", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocTd
    {

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private string abbrField;

        private string axisField;

        private string headersField;

        private StrucDocTdScope scopeField;

        private bool scopeFieldSpecified;

        private string rowspanField;

        private string colspanField;

        private StrucDocTdAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocTdValign valignField;

        private bool valignFieldSpecified;

        public StrucDocTd()
        {
            this.rowspanField = "1";
            this.colspanField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("br", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("content", typeof(StrucDocContent))]
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("list", typeof(StrucDocList))]
        [System.Xml.Serialization.XmlElementAttribute("paragraph", typeof(StrucDocParagraph))]
        [System.Xml.Serialization.XmlElementAttribute("renderMultiMedia", typeof(StrucDocRenderMultiMedia))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string abbr
        {
            get
            {
                return this.abbrField;
            }
            set
            {
                this.abbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string axis
        {
            get
            {
                return this.axisField;
            }
            set
            {
                this.axisField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREFS")]
        public string headers
        {
            get
            {
                return this.headersField;
            }
            set
            {
                this.headersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTdScope scope
        {
            get
            {
                return this.scopeField;
            }
            set
            {
                this.scopeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool scopeSpecified
        {
            get
            {
                return this.scopeFieldSpecified;
            }
            set
            {
                this.scopeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string rowspan
        {
            get
            {
                return this.rowspanField;
            }
            set
            {
                this.rowspanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string colspan
        {
            get
            {
                return this.colspanField;
            }
            set
            {
                this.colspanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTdAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTdValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTdScope
    {

        /// <remarks/>
        row,

        /// <remarks/>
        col,

        /// <remarks/>
        rowgroup,

        /// <remarks/>
        colgroup,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTdAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTdValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Th", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocTh
    {

        private List<object> itemsField;

        private List<string> textField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private string abbrField;

        private string axisField;

        private string headersField;

        private StrucDocThScope scopeField;

        private bool scopeFieldSpecified;

        private string rowspanField;

        private string colspanField;

        private StrucDocThAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocThValign valignField;

        private bool valignFieldSpecified;

        public StrucDocTh()
        {
            this.rowspanField = "1";
            this.colspanField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("br", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("content", typeof(StrucDocContent))]
        [System.Xml.Serialization.XmlElementAttribute("footnote", typeof(StrucDocFootnote))]
        [System.Xml.Serialization.XmlElementAttribute("footnoteRef", typeof(StrucDocFootnoteRef))]
        [System.Xml.Serialization.XmlElementAttribute("linkHtml", typeof(StrucDocLinkHtml))]
        [System.Xml.Serialization.XmlElementAttribute("renderMultiMedia", typeof(StrucDocRenderMultiMedia))]
        [System.Xml.Serialization.XmlElementAttribute("sub", typeof(StrucDocSub))]
        [System.Xml.Serialization.XmlElementAttribute("sup", typeof(StrucDocSup))]
        public List<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string abbr
        {
            get
            {
                return this.abbrField;
            }
            set
            {
                this.abbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string axis
        {
            get
            {
                return this.axisField;
            }
            set
            {
                this.axisField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREFS")]
        public string headers
        {
            get
            {
                return this.headersField;
            }
            set
            {
                this.headersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocThScope scope
        {
            get
            {
                return this.scopeField;
            }
            set
            {
                this.scopeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool scopeSpecified
        {
            get
            {
                return this.scopeFieldSpecified;
            }
            set
            {
                this.scopeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string rowspan
        {
            get
            {
                return this.rowspanField;
            }
            set
            {
                this.rowspanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string colspan
        {
            get
            {
                return this.colspanField;
            }
            set
            {
                this.colspanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocThAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocThValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocThScope
    {

        /// <remarks/>
        row,

        /// <remarks/>
        col,

        /// <remarks/>
        rowgroup,

        /// <remarks/>
        colgroup,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocThAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocThValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTrAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTrValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTheadAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTheadValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Tfoot", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocTfoot
    {

        private List<StrucDocTr> trField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private StrucDocTfootAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocTfootValign valignField;

        private bool valignFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tr")]
        public List<StrucDocTr> tr
        {
            get
            {
                return this.trField;
            }
            set
            {
                this.trField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTfootAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTfootValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTfootAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTfootValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StrucDoc.Tbody", Namespace = "urn:hl7-org:v3")]
    public partial class StrucDocTbody
    {

        private List<StrucDocTr> trField;

        private string idField;

        private string languageField;

        private string styleCodeField;

        private StrucDocTbodyAlign alignField;

        private bool alignFieldSpecified;

        private string charField;

        private string charoffField;

        private StrucDocTbodyValign valignField;

        private bool valignFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tr")]
        public List<StrucDocTr> tr
        {
            get
            {
                return this.trField;
            }
            set
            {
                this.trField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKENS")]
        public string styleCode
        {
            get
            {
                return this.styleCodeField;
            }
            set
            {
                this.styleCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTbodyAlign align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool alignSpecified
        {
            get
            {
                return this.alignFieldSpecified;
            }
            set
            {
                this.alignFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string charoff
        {
            get
            {
                return this.charoffField;
            }
            set
            {
                this.charoffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StrucDocTbodyValign valign
        {
            get
            {
                return this.valignField;
            }
            set
            {
                this.valignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valignSpecified
        {
            get
            {
                return this.valignFieldSpecified;
            }
            set
            {
                this.valignFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTbodyAlign
    {

        /// <remarks/>
        left,

        /// <remarks/>
        center,

        /// <remarks/>
        right,

        /// <remarks/>
        justify,

        /// <remarks/>
        @char,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTbodyValign
    {

        /// <remarks/>
        top,

        /// <remarks/>
        middle,

        /// <remarks/>
        bottom,

        /// <remarks/>
        baseline,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTableFrame
    {

        /// <remarks/>
        @void,

        /// <remarks/>
        above,

        /// <remarks/>
        below,

        /// <remarks/>
        hsides,

        /// <remarks/>
        lhs,

        /// <remarks/>
        rhs,

        /// <remarks/>
        vsides,

        /// <remarks/>
        box,

        /// <remarks/>
        border,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocTableRules
    {

        /// <remarks/>
        none,

        /// <remarks/>
        groups,

        /// <remarks/>
        rows,

        /// <remarks/>
        cols,

        /// <remarks/>
        all,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocListListType
    {

        /// <remarks/>
        ordered,

        /// <remarks/>
        unordered,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:hl7-org:v3")]
    public enum StrucDocContentRevised
    {

        /// <remarks/>
        insert,

        /// <remarks/>
        delete,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = ".Subject", Namespace = "urn:hl7-org:v3")]
    public partial class Subject
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE awarenessCodeField;

        private RelatedSubject relatedSubjectField;

        private string nullFlavorField;

        private ParticipationTargetSubject typeCodeField;

        private bool typeCodeFieldSpecified;

        private string contextControlCodeField;

        public Subject()
        {
            this.typeCodeField = ParticipationTargetSubject.SBJ;
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE awarenessCode
        {
            get
            {
                return this.awarenessCodeField;
            }
            set
            {
                this.awarenessCodeField = value;
            }
        }

        /// <remarks/>
        public RelatedSubject relatedSubject
        {
            get
            {
                return this.relatedSubjectField;
            }
            set
            {
                this.relatedSubjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ParticipationTargetSubject typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelatedSubject", Namespace = "urn:hl7-org:v3")]
    public partial class RelatedSubject
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE codeField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private SubjectPerson subjectField;

        private string nullFlavorField;

        private string classCodeField;

        public RelatedSubject()
        {
            this.classCodeField = "PRS";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        public SubjectPerson subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        //[System.ComponentModel.DefaultValueAttribute(x_DocumentSubject.PRS)]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class AD : ANY
    {

        private List<ADXP> itemsField;

        private List<SXCM_TS> useablePeriodField;

        private List<string> textField;

        private List<string> useField;

        private bool isNotOrderedField;

        private bool isNotOrderedFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("additionalLocator", typeof(adxpadditionalLocator))]
        [System.Xml.Serialization.XmlElementAttribute("buildingNumberSuffix", typeof(adxpbuildingNumberSuffix))]
        [System.Xml.Serialization.XmlElementAttribute("careOf", typeof(adxpcareOf))]
        [System.Xml.Serialization.XmlElementAttribute("censusTract", typeof(adxpcensusTract))]
        [System.Xml.Serialization.XmlElementAttribute("city", typeof(adxpcity))]
        [System.Xml.Serialization.XmlElementAttribute("country", typeof(adxpcountry))]
        [System.Xml.Serialization.XmlElementAttribute("county", typeof(adxpcounty))]
        [System.Xml.Serialization.XmlElementAttribute("delimiter", typeof(adxpdelimiter))]
        [System.Xml.Serialization.XmlElementAttribute("deliveryAddressLine", typeof(adxpdeliveryAddressLine))]
        [System.Xml.Serialization.XmlElementAttribute("deliveryInstallationArea", typeof(adxpdeliveryInstallationArea))]
        [System.Xml.Serialization.XmlElementAttribute("deliveryInstallationQualifier", typeof(adxpdeliveryInstallationQualifier))]
        [System.Xml.Serialization.XmlElementAttribute("deliveryInstallationType", typeof(adxpdeliveryInstallationType))]
        [System.Xml.Serialization.XmlElementAttribute("deliveryMode", typeof(adxpdeliveryMode))]
        [System.Xml.Serialization.XmlElementAttribute("deliveryModeIdentifier", typeof(adxpdeliveryModeIdentifier))]
        [System.Xml.Serialization.XmlElementAttribute("direction", typeof(adxpdirection))]
        [System.Xml.Serialization.XmlElementAttribute("houseNumber", typeof(adxphouseNumber))]
        [System.Xml.Serialization.XmlElementAttribute("houseNumberNumeric", typeof(adxphouseNumberNumeric))]
        [System.Xml.Serialization.XmlElementAttribute("postBox", typeof(adxppostBox))]
        [System.Xml.Serialization.XmlElementAttribute("postalCode", typeof(adxppostalCode))]
        [System.Xml.Serialization.XmlElementAttribute("precinct", typeof(adxpprecinct))]
        [System.Xml.Serialization.XmlElementAttribute("state", typeof(adxpstate))]
        [System.Xml.Serialization.XmlElementAttribute("streetAddressLine", typeof(adxpstreetAddressLine))]
        [System.Xml.Serialization.XmlElementAttribute("streetName", typeof(adxpstreetName))]
        [System.Xml.Serialization.XmlElementAttribute("streetNameBase", typeof(adxpstreetNameBase))]
        [System.Xml.Serialization.XmlElementAttribute("streetNameType", typeof(adxpstreetNameType))]
        [System.Xml.Serialization.XmlElementAttribute("unitID", typeof(adxpunitID))]
        [System.Xml.Serialization.XmlElementAttribute("unitType", typeof(adxpunitType))]
        public List<ADXP> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("useablePeriod")]
        public List<SXCM_TS> useablePeriod
        {
            get
            {
                return this.useablePeriodField;
            }
            set
            {
                this.useablePeriodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public List<string> use
        {
            get
            {
                return this.useField;
            }
            set
            {
                this.useField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isNotOrdered
        {
            get
            {
                return this.isNotOrderedField;
            }
            set
            {
                this.isNotOrderedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isNotOrderedSpecified
        {
            get
            {
                return this.isNotOrderedFieldSpecified;
            }
            set
            {
                this.isNotOrderedFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SubjectPerson", Namespace = "urn:hl7-org:v3")]
    public partial class SubjectPerson
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private List<PN> nameField;

        private CE administrativeGenderCodeField;

        private TS birthTimeField;

        private BL deceasedIndField;

        private TS deceasedTimeField;

        private string nullFlavorField;

        private string classCodeField;

        private string determinerCodeField;

        public SubjectPerson()
        {
            this.classCodeField = "PSN";
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id", Namespace = "urn:hl7-org:sdtc")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public List<PN> name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public CE administrativeGenderCode
        {
            get
            {
                return this.administrativeGenderCodeField;
            }
            set
            {
                this.administrativeGenderCodeField = value;
            }
        }

        /// <remarks/>
        public TS birthTime
        {
            get
            {
                return this.birthTimeField;
            }
            set
            {
                this.birthTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:hl7-org:sdtc")]
        public BL deceasedInd
        {
            get
            {
                return this.deceasedIndField;
            }
            set
            {
                this.deceasedIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:hl7-org:sdtc")]
        public TS deceasedTime
        {
            get
            {
                return this.deceasedTimeField;
            }
            set
            {
                this.deceasedTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class PN : EN
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TN))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ON))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PN))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class EN : ANY
    {

        private List<ENXP> itemsField;

        private IVL_TS validTimeField;

        private List<string> textField;

        private List<string> useField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("delimiter", typeof(endelimiter))]
        [System.Xml.Serialization.XmlElementAttribute("family", typeof(enfamily))]
        [System.Xml.Serialization.XmlElementAttribute("given", typeof(engiven))]
        [System.Xml.Serialization.XmlElementAttribute("prefix", typeof(enprefix))]
        [System.Xml.Serialization.XmlElementAttribute("suffix", typeof(ensuffix))]
        public List<ENXP> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        public IVL_TS validTime
        {
            get
            {
                return this.validTimeField;
            }
            set
            {
                this.validTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public List<string> use
        {
            get
            {
                return this.useField;
            }
            set
            {
                this.useField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVL_TS : SXCM_TS
    {

        private QTY[] itemsField;

        private ItemsChoiceType2[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("center", typeof(TS))]
        [System.Xml.Serialization.XmlElementAttribute("high", typeof(IVXB_TS))]
        [System.Xml.Serialization.XmlElementAttribute("low", typeof(IVXB_TS))]
        [System.Xml.Serialization.XmlElementAttribute("width", typeof(PQ))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public QTY[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType2[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVXB_TS : TS
    {

        private bool inclusiveField;

        public IVXB_TS()
        {
            this.inclusiveField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HXIT_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BXIT_IVL_PQ))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class PQ : QTY
    {

        private List<PQR> translationField;

        private string valueField;

        private string unitField;

        public PQ()
        {
            this.unitField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("translation")]
        public List<PQR> translation
        {
            get
            {
                return this.translationField;
            }
            set
            {
                this.translationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class PQR : CV
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class HXIT_PQ : PQ
    {

        private IVL_TS validTimeField;

        /// <remarks/>
        public IVL_TS validTime
        {
            get
            {
                return this.validTimeField;
            }
            set
            {
                this.validTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_PQ))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class PPD_PQ : PQ
    {

        private PQ standardDeviationField;

        private ProbabilityDistributionType distributionTypeField;

        private bool distributionTypeFieldSpecified;

        /// <remarks/>
        public PQ standardDeviation
        {
            get
            {
                return this.standardDeviationField;
            }
            set
            {
                this.standardDeviationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ProbabilityDistributionType distributionType
        {
            get
            {
                return this.distributionTypeField;
            }
            set
            {
                this.distributionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool distributionTypeSpecified
        {
            get
            {
                return this.distributionTypeFieldSpecified;
            }
            set
            {
                this.distributionTypeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ProbabilityDistributionType
    {

        /// <remarks/>
        B,

        /// <remarks/>
        E,

        /// <remarks/>
        F,

        /// <remarks/>
        G,

        /// <remarks/>
        LN,

        /// <remarks/>
        N,

        /// <remarks/>
        T,

        /// <remarks/>
        U,

        /// <remarks/>
        X2,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVXB_PPD_PQ : PPD_PQ
    {

        private bool inclusiveField;

        public IVXB_PPD_PQ()
        {
            this.inclusiveField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_PQ))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_PPD_PQ : PPD_PQ
    {

        private SetOperator operatorField;

        public SXCM_PPD_PQ()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVL_PPD_PQ : SXCM_PPD_PQ
    {

        private PPD_PQ[] itemsField;

        private ItemsChoiceType1[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("center", typeof(PPD_PQ))]
        [System.Xml.Serialization.XmlElementAttribute("high", typeof(IVXB_PPD_PQ))]
        [System.Xml.Serialization.XmlElementAttribute("low", typeof(IVXB_PPD_PQ))]
        [System.Xml.Serialization.XmlElementAttribute("width", typeof(PPD_PQ))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public PPD_PQ[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType1[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3", IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {

        /// <remarks/>
        center,

        /// <remarks/>
        high,

        /// <remarks/>
        low,

        /// <remarks/>
        width,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVXB_PQ : PQ
    {

        private bool inclusiveField;

        public IVXB_PQ()
        {
            this.inclusiveField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PQ))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BXIT_IVL_PQ))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_PQ : PQ
    {

        private SetOperator operatorField;

        public SXCM_PQ()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BXIT_IVL_PQ))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVL_PQ : SXCM_PQ
    {

        private PQ[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("center", typeof(PQ))]
        [System.Xml.Serialization.XmlElementAttribute("high", typeof(IVXB_PQ))]
        [System.Xml.Serialization.XmlElementAttribute("low", typeof(IVXB_PQ))]
        [System.Xml.Serialization.XmlElementAttribute("width", typeof(PQ))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public PQ[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        center,

        /// <remarks/>
        high,

        /// <remarks/>
        low,

        /// <remarks/>
        width,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class BXIT_IVL_PQ : IVL_PQ
    {

        private string qtyField;

        public BXIT_IVL_PQ()
        {
            this.qtyField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string qty
        {
            get
            {
                return this.qtyField;
            }
            set
            {
                this.qtyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3", IncludeInSchema = false)]
    public enum ItemsChoiceType2
    {

        /// <remarks/>
        center,

        /// <remarks/>
        high,

        /// <remarks/>
        low,

        /// <remarks/>
        width,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class TN : EN
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class ON : EN
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    [System.Xml.Serialization.XmlRootAttribute("deceasedInd", Namespace = "urn:hl7-org:sdtc", IsNullable = false)]
    public partial class BL : ANY
    {

        private bool valueField;

        private bool valueFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_DocumentSubject
    {

        /// <remarks/>
        PAT,

        /// <remarks/>
        PRS,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ParticipationTargetSubject
    {

        /// <remarks/>
        SBJ,

        /// <remarks/>
        SPC,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Author", Namespace = "urn:hl7-org:v3")]
    public partial class Author
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE functionCodeField;

        private TS timeField;

        private AssignedAuthor assignedAuthorField;

        private string nullFlavorField;

        private string typeCodeField;

        private string contextControlCodeField;

        public Author()
        {
            this.typeCodeField = "AUT";
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE functionCode
        {
            get
            {
                return this.functionCodeField;
            }
            set
            {
                this.functionCodeField = value;
            }
        }

        /// <remarks/>
        public TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public AssignedAuthor assignedAuthor
        {
            get
            {
                return this.assignedAuthorField;
            }
            set
            {
                this.assignedAuthorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "AssignedAuthor", Namespace = "urn:hl7-org:v3")]
    public partial class AssignedAuthor
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private object itemField;

        private Organization representedOrganizationField;

        private string nullFlavorField;

        private string classCodeField;

        public AssignedAuthor()
        {
            this.classCodeField = "ASSIGNED";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assignedAuthoringDevice", typeof(AuthoringDevice))]
        [System.Xml.Serialization.XmlElementAttribute("assignedPerson", typeof(Person))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public Organization representedOrganization
        {
            get
            {
                return this.representedOrganizationField;
            }
            set
            {
                this.representedOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "AuthoringDevice", Namespace = "urn:hl7-org:v3")]
    public partial class AuthoringDevice
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE codeField;

        private SC manufacturerModelNameField;

        private SC softwareNameField;

        private List<MaintainedEntity> asMaintainedEntityField;

        private string nullFlavorField;

        private EntityClassDevice classCodeField;

        private bool classCodeFieldSpecified;

        private string determinerCodeField;

        public AuthoringDevice()
        {
            this.classCodeField = EntityClassDevice.DEV;
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public SC manufacturerModelName
        {
            get
            {
                return this.manufacturerModelNameField;
            }
            set
            {
                this.manufacturerModelNameField = value;
            }
        }

        /// <remarks/>
        public SC softwareName
        {
            get
            {
                return this.softwareNameField;
            }
            set
            {
                this.softwareNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("asMaintainedEntity")]
        public List<MaintainedEntity> asMaintainedEntity
        {
            get
            {
                return this.asMaintainedEntityField;
            }
            set
            {
                this.asMaintainedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EntityClassDevice classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classCodeSpecified
        {
            get
            {
                return this.classCodeFieldSpecified;
            }
            set
            {
                this.classCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "MaintainedEntity", Namespace = "urn:hl7-org:v3")]
    public partial class MaintainedEntity
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private IVL_TS effectiveTimeField;

        private Person maintainingPersonField;

        private string nullFlavorField;

        private string classCodeField;

        public MaintainedEntity()
        {
            this.classCodeField = "MNT";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public Person maintainingPerson
        {
            get
            {
                return this.maintainingPersonField;
            }
            set
            {
                this.maintainingPersonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Person", Namespace = "urn:hl7-org:v3")]
    public partial class Person
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<PN> nameField;

        private string nullFlavorField;

        private string classCodeField;

        private string determinerCodeField;
        private TS birthTimeField;
        
        public Person()
        {
            this.classCodeField = "PSN";
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public List<PN> name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:hl7-org:sdtc")]
        public TS birthTime
        {
            get
            {
                return this.birthTimeField;
            }
            set
            {
                this.birthTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum EntityClassDevice
    {

        /// <remarks/>
        DEV,

        /// <remarks/>
        CER,

        /// <remarks/>
        MODDV,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Organization", Namespace = "urn:hl7-org:v3")]
    public partial class Organization
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private List<ON> nameField;

        private List<TEL> telecomField;

        private List<AD> addrField;

        private CE standardIndustryClassCodeField;

        private OrganizationPartOf asOrganizationPartOfField;

        private string nullFlavorField;

        private string classCodeField;

        private string determinerCodeField;

        public Organization()
        {
            this.classCodeField = "ORG";
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public List<ON> name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        public CE standardIndustryClassCode
        {
            get
            {
                return this.standardIndustryClassCodeField;
            }
            set
            {
                this.standardIndustryClassCodeField = value;
            }
        }

        /// <remarks/>
        public OrganizationPartOf asOrganizationPartOf
        {
            get
            {
                return this.asOrganizationPartOfField;
            }
            set
            {
                this.asOrganizationPartOfField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "OrganizationPartOf", Namespace = "urn:hl7-org:v3")]
    public partial class OrganizationPartOf
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private CS statusCodeField;

        private IVL_TS effectiveTimeField;

        private Organization wholeOrganizationField;

        private string nullFlavorField;

        private string classCodeField;

        public OrganizationPartOf()
        {
            this.classCodeField = "PART";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public Organization wholeOrganization
        {
            get
            {
                return this.wholeOrganizationField;
            }
            set
            {
                this.wholeOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Informant12", Namespace = "urn:hl7-org:v3")]
    public partial class Informant12
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private object itemField;

        private string nullFlavorField;

        private string typeCodeField;

        private string contextControlCodeField;

        public Informant12()
        {
            this.typeCodeField = "INF";
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assignedEntity", typeof(AssignedEntity))]
        [System.Xml.Serialization.XmlElementAttribute("relatedEntity", typeof(RelatedEntity))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "AssignedEntity", Namespace = "urn:hl7-org:v3")]
    public partial class AssignedEntity
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private Person assignedPersonField;

        private Organization representedOrganizationField;

        private string nullFlavorField;

        private string classCodeField;

        public AssignedEntity()
        {
            this.classCodeField = "ASSIGNED";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        public Person assignedPerson
        {
            get
            {
                return this.assignedPersonField;
            }
            set
            {
                this.assignedPersonField = value;
            }
        }

        /// <remarks/>
        public Organization representedOrganization
        {
            get
            {
                return this.representedOrganizationField;
            }
            set
            {
                this.representedOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelatedEntity", Namespace = "urn:hl7-org:v3")]
    public partial class RelatedEntity
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE codeField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private IVL_TS effectiveTimeField;

        private Person relatedPersonField;

        private string nullFlavorField;

        private string classCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public Person relatedPerson
        {
            get
            {
                return this.relatedPersonField;
            }
            set
            {
                this.relatedPersonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Entry", Namespace = "urn:hl7-org:v3")]
    public partial class Entry
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private object itemField;

        private string nullFlavorField;

        private x_ActRelationshipEntry typeCodeField;

        private bool contextConductionIndField;

        private bool contextConductionIndFieldSpecified;

        public Entry()
        {
            this.typeCodeField = x_ActRelationshipEntry.COMP;
            this.contextConductionIndField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("act", typeof(Act))]
        [System.Xml.Serialization.XmlElementAttribute("encounter", typeof(Encounter))]
        [System.Xml.Serialization.XmlElementAttribute("observation", typeof(Observation))]
        [System.Xml.Serialization.XmlElementAttribute("observationMedia", typeof(ObservationMedia))]
        [System.Xml.Serialization.XmlElementAttribute("organizer", typeof(Organizer))]
        [System.Xml.Serialization.XmlElementAttribute("procedure", typeof(Procedure))]
        [System.Xml.Serialization.XmlElementAttribute("regionOfInterest", typeof(RegionOfInterest))]
        [System.Xml.Serialization.XmlElementAttribute("substanceAdministration", typeof(SubstanceAdministration))]
        [System.Xml.Serialization.XmlElementAttribute("supply", typeof(Supply))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(x_ActRelationshipEntry.COMP)]
        public x_ActRelationshipEntry typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool contextConductionInd
        {
            get
            {
                return this.contextConductionIndField;
            }
            set
            {
                this.contextConductionIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool contextConductionIndSpecified
        {
            get
            {
                return this.contextConductionIndFieldSpecified;
            }
            set
            {
                this.contextConductionIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Act", Namespace = "urn:hl7-org:v3")]
    public partial class Act
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private CS statusCodeField;

        private IVL_TS effectiveTimeField;

        private CE priorityCodeField;

        private CS languageCodeField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private string nullFlavorField;

        private x_ActClassDocumentEntryAct classCodeField;

        private x_DocumentActMood moodCodeField;

        private bool negationIndField;

        private bool negationIndFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public CE priorityCode
        {
            get
            {
                return this.priorityCodeField;
            }
            set
            {
                this.priorityCodeField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_ActClassDocumentEntryAct classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_DocumentActMood moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool negationInd
        {
            get
            {
                return this.negationIndField;
            }
            set
            {
                this.negationIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool negationIndSpecified
        {
            get
            {
                return this.negationIndFieldSpecified;
            }
            set
            {
                this.negationIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Specimen", Namespace = "urn:hl7-org:v3")]
    public partial class Specimen
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private SpecimenRole specimenRoleField;

        private string nullFlavorField;

        private string typeCodeField;

        public Specimen()
        {
            this.typeCodeField = "SPC";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public SpecimenRole specimenRole
        {
            get
            {
                return this.specimenRoleField;
            }
            set
            {
                this.specimenRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SpecimenRole", Namespace = "urn:hl7-org:v3")]
    public partial class SpecimenRole
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private PlayingEntity specimenPlayingEntityField;

        private string nullFlavorField;

        private RoleClassSpecimen classCodeField;

        private bool classCodeFieldSpecified;

        public SpecimenRole()
        {
            this.classCodeField = RoleClassSpecimen.SPEC;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public PlayingEntity specimenPlayingEntity
        {
            get
            {
                return this.specimenPlayingEntityField;
            }
            set
            {
                this.specimenPlayingEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public RoleClassSpecimen classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classCodeSpecified
        {
            get
            {
                return this.classCodeFieldSpecified;
            }
            set
            {
                this.classCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "PlayingEntity", Namespace = "urn:hl7-org:v3")]
    public partial class PlayingEntity
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE codeField;

        private List<PQ> quantityField;

        private List<PN> nameField;

        private TS birthTimeField;

        private ED descField;

        private string nullFlavorField;

        private string classCodeField;

        private string determinerCodeField;

        public PlayingEntity()
        {
            this.classCodeField = "ENT";
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("quantity")]
        public List<PQ> quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public List<PN> name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:hl7-org:sdtc")]
        public TS birthTime
        {
            get
            {
                return this.birthTimeField;
            }
            set
            {
                this.birthTimeField = value;
            }
        }

        /// <remarks/>
        public ED desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("ENT")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum RoleClassSpecimen
    {

        /// <remarks/>
        SPEC,

        /// <remarks/>
        ALQT,

        /// <remarks/>
        ISLT,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Performer2", Namespace = "urn:hl7-org:v3")]
    public partial class Performer2
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private IVL_TS timeField;

        private CE modeCodeField;

        private AssignedEntity assignedEntityField;

        private string nullFlavorField;

        private ParticipationPhysicalPerformer typeCodeField;

        private bool typeCodeFieldSpecified;

        public Performer2()
        {
            this.typeCodeField = ParticipationPhysicalPerformer.PRF;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public IVL_TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public CE modeCode
        {
            get
            {
                return this.modeCodeField;
            }
            set
            {
                this.modeCodeField = value;
            }
        }

        /// <remarks/>
        public AssignedEntity assignedEntity
        {
            get
            {
                return this.assignedEntityField;
            }
            set
            {
                this.assignedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ParticipationPhysicalPerformer typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ParticipationPhysicalPerformer
    {

        /// <remarks/>
        PRF,

        /// <remarks/>
        DIST,

        /// <remarks/>
        PPRF,

        /// <remarks/>
        SPRF,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Participant2", Namespace = "urn:hl7-org:v3")]
    public partial class Participant2
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private IVL_TS timeField;

        private CE awarenessCodeField;

        private ParticipantRole participantRoleField;

        private string nullFlavorField;

        private string typeCodeField;

        private string contextControlCodeField;

        public Participant2()
        {
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public IVL_TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public CE awarenessCode
        {
            get
            {
                return this.awarenessCodeField;
            }
            set
            {
                this.awarenessCodeField = value;
            }
        }

        /// <remarks/>
        public ParticipantRole participantRole
        {
            get
            {
                return this.participantRoleField;
            }
            set
            {
                this.participantRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ParticipantRole", Namespace = "urn:hl7-org:v3")]
    public partial class ParticipantRole
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private object itemField;

        private Entity scopingEntityField;

        private string nullFlavorField;

        private string classCodeField;

        public ParticipantRole()
        {
            this.classCodeField = "ROL";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("playingDevice", typeof(Device))]
        [System.Xml.Serialization.XmlElementAttribute("playingEntity", typeof(PlayingEntity))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public Entity scopingEntity
        {
            get
            {
                return this.scopingEntityField;
            }
            set
            {
                this.scopingEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("ROL")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Device", Namespace = "urn:hl7-org:v3")]
    public partial class Device
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE codeField;

        private SC manufacturerModelNameField;

        private SC softwareNameField;

        private string nullFlavorField;

        private EntityClassDevice classCodeField;

        private string determinerCodeField;

        public Device()
        {
            this.classCodeField = EntityClassDevice.DEV;
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public SC manufacturerModelName
        {
            get
            {
                return this.manufacturerModelNameField;
            }
            set
            {
                this.manufacturerModelNameField = value;
            }
        }

        /// <remarks/>
        public SC softwareName
        {
            get
            {
                return this.softwareNameField;
            }
            set
            {
                this.softwareNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(EntityClassDevice.DEV)]
        public EntityClassDevice classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Entity", Namespace = "urn:hl7-org:v3")]
    public partial class Entity
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private ED descField;

        private string nullFlavorField;

        private string classCodeField;

        private string determinerCodeField;

        public Entity()
        {
            this.classCodeField = "ENT";
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("ENT")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "EntryRelationship", Namespace = "urn:hl7-org:v3")]
    public partial class EntryRelationship
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private INT sequenceNumberField;

        private BL seperatableIndField;

        private object itemField;

        private string nullFlavorField;

        private x_ActRelationshipEntryRelationship typeCodeField;

        private bool inversionIndField;

        private bool inversionIndFieldSpecified;

        private bool contextConductionIndField;

        private bool negationIndField;

        private bool negationIndFieldSpecified;

        public EntryRelationship()
        {
            this.contextConductionIndField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public INT sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public BL seperatableInd
        {
            get
            {
                return this.seperatableIndField;
            }
            set
            {
                this.seperatableIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("act", typeof(Act))]
        [System.Xml.Serialization.XmlElementAttribute("encounter", typeof(Encounter))]
        [System.Xml.Serialization.XmlElementAttribute("observation", typeof(Observation))]
        [System.Xml.Serialization.XmlElementAttribute("observationMedia", typeof(ObservationMedia))]
        [System.Xml.Serialization.XmlElementAttribute("organizer", typeof(Organizer))]
        [System.Xml.Serialization.XmlElementAttribute("procedure", typeof(Procedure))]
        [System.Xml.Serialization.XmlElementAttribute("regionOfInterest", typeof(RegionOfInterest))]
        [System.Xml.Serialization.XmlElementAttribute("substanceAdministration", typeof(SubstanceAdministration))]
        [System.Xml.Serialization.XmlElementAttribute("supply", typeof(Supply))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_ActRelationshipEntryRelationship typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool inversionInd
        {
            get
            {
                return this.inversionIndField;
            }
            set
            {
                this.inversionIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool inversionIndSpecified
        {
            get
            {
                return this.inversionIndFieldSpecified;
            }
            set
            {
                this.inversionIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool contextConductionInd
        {
            get
            {
                return this.contextConductionIndField;
            }
            set
            {
                this.contextConductionIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool negationInd
        {
            get
            {
                return this.negationIndField;
            }
            set
            {
                this.negationIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool negationIndSpecified
        {
            get
            {
                return this.negationIndFieldSpecified;
            }
            set
            {
                this.negationIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegionOfInterestvalue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_INT))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_INT))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class INT : QTY
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "RegionOfInterest.value", Namespace = "urn:hl7-org:v3")]
    public partial class RegionOfInterestvalue : INT
    {

        private bool unsortedField;

        public RegionOfInterestvalue()
        {
            this.unsortedField = false;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool unsorted
        {
            get
            {
                return this.unsortedField;
            }
            set
            {
                this.unsortedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVXB_INT : INT
    {

        private bool inclusiveField;

        public IVXB_INT()
        {
            this.inclusiveField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_INT))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_INT : INT
    {

        private SetOperator operatorField;

        public SXCM_INT()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVL_INT : SXCM_INT
    {

        private INT[] itemsField;

        private ItemsChoiceType4[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("center", typeof(INT))]
        [System.Xml.Serialization.XmlElementAttribute("high", typeof(IVXB_INT))]
        [System.Xml.Serialization.XmlElementAttribute("low", typeof(IVXB_INT))]
        [System.Xml.Serialization.XmlElementAttribute("width", typeof(INT))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public INT[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType4[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3", IncludeInSchema = false)]
    public enum ItemsChoiceType4
    {

        /// <remarks/>
        center,

        /// <remarks/>
        high,

        /// <remarks/>
        low,

        /// <remarks/>
        width,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Encounter", Namespace = "urn:hl7-org:v3")]
    public partial class Encounter
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private CS statusCodeField;

        private IVL_TS effectiveTimeField;

        private CE dischargeDispositionCodeField;

        private CE priorityCodeField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private string nullFlavorField;

        private string classCodeField;

        private x_DocumentEncounterMood moodCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:hl7-org:sdtc")]
        public CE dischargeDispositionCode
        {
            get
            {
                return this.dischargeDispositionCodeField;
            }
            set
            {
                this.dischargeDispositionCodeField = value;
            }
        }

        /// <remarks/>
        public CE priorityCode
        {
            get
            {
                return this.priorityCodeField;
            }
            set
            {
                this.priorityCodeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_DocumentEncounterMood moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Reference", Namespace = "urn:hl7-org:v3")]
    public partial class Reference
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private BL seperatableIndField;

        private object itemField;

        private string nullFlavorField;

        private x_ActRelationshipExternalReference typeCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public BL seperatableInd
        {
            get
            {
                return this.seperatableIndField;
            }
            set
            {
                this.seperatableIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("externalAct", typeof(ExternalAct))]
        [System.Xml.Serialization.XmlElementAttribute("externalDocument", typeof(ExternalDocument))]
        [System.Xml.Serialization.XmlElementAttribute("externalObservation", typeof(ExternalObservation))]
        [System.Xml.Serialization.XmlElementAttribute("externalProcedure", typeof(ExternalProcedure))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_ActRelationshipExternalReference typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExternalAct", Namespace = "urn:hl7-org:v3")]
    public partial class ExternalAct
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public ExternalAct()
        {
            this.classCodeField = "ACT";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("ACT")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExternalDocument", Namespace = "urn:hl7-org:v3")]
    public partial class ExternalDocument
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private II setIdField;

        private INT versionNumberField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public ExternalDocument()
        {
            this.classCodeField = "DOC";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public II setId
        {
            get
            {
                return this.setIdField;
            }
            set
            {
                this.setIdField = value;
            }
        }

        /// <remarks/>
        public INT versionNumber
        {
            get
            {
                return this.versionNumberField;
            }
            set
            {
                this.versionNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("DOC")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExternalObservation", Namespace = "urn:hl7-org:v3")]
    public partial class ExternalObservation
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public ExternalObservation()
        {
            this.classCodeField = "OBS";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("OBS")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ExternalProcedure", Namespace = "urn:hl7-org:v3")]
    public partial class ExternalProcedure
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public ExternalProcedure()
        {
            this.classCodeField = "PROC";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ActRelationshipExternalReference
    {

        /// <remarks/>
        XCRPT,

        /// <remarks/>
        RPLC,

        /// <remarks/>
        SPRT,

        /// <remarks/>
        ELNK,

        /// <remarks/>
        REFR,

        /// <remarks/>
        SUBJ,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Precondition", Namespace = "urn:hl7-org:v3")]
    public partial class Precondition
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private Criterion criterionField;

        private string nullFlavorField;

        private string typeCodeField;

        public Precondition()
        {
            this.typeCodeField = "PRCN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public Criterion criterion
        {
            get
            {
                return this.criterionField;
            }
            set
            {
                this.criterionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Criterion", Namespace = "urn:hl7-org:v3")]
    public partial class Criterion
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CD codeField;

        private ED textField;

        private ANY valueField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public Criterion()
        {
            this.classCodeField = "OBS";
            this.moodCodeField = "EVN.CRT";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public ANY value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("OBS")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_DocumentEncounterMood
    {

        /// <remarks/>
        INT,

        /// <remarks/>
        APT,

        /// <remarks/>
        ARQ,

        /// <remarks/>
        EVN,

        /// <remarks/>
        PRMS,

        /// <remarks/>
        PRP,

        /// <remarks/>
        RQO,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Observation", Namespace = "urn:hl7-org:v3")]
    public partial class Observation
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ST derivationExprField;

        private ED textField;

        private CS statusCodeField;

        private IVL_TS effectiveTimeField;

        private CE priorityCodeField;

        private IVL_INT repeatNumberField;

        private CS languageCodeField;

        private List<ANY> valueField;

        private List<CE> interpretationCodeField;

        private List<CE> methodCodeField;

        private List<CD> targetSiteCodeField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private List<ReferenceRange> referenceRangeField;

        private string nullFlavorField;

        private string classCodeField;

        private x_ActMoodDocumentObservation moodCodeField;

        private bool negationIndField;

        private bool negationIndFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ST derivationExpr
        {
            get
            {
                return this.derivationExprField;
            }
            set
            {
                this.derivationExprField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public CE priorityCode
        {
            get
            {
                return this.priorityCodeField;
            }
            set
            {
                this.priorityCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_INT repeatNumber
        {
            get
            {
                return this.repeatNumberField;
            }
            set
            {
                this.repeatNumberField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public List<ANY> value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("interpretationCode")]
        public List<CE> interpretationCode
        {
            get
            {
                return this.interpretationCodeField;
            }
            set
            {
                this.interpretationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("methodCode")]
        public List<CE> methodCode
        {
            get
            {
                return this.methodCodeField;
            }
            set
            {
                this.methodCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("targetSiteCode")]
        public List<CD> targetSiteCode
        {
            get
            {
                return this.targetSiteCodeField;
            }
            set
            {
                this.targetSiteCodeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("referenceRange")]
        public List<ReferenceRange> referenceRange
        {
            get
            {
                return this.referenceRangeField;
            }
            set
            {
                this.referenceRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_ActMoodDocumentObservation moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool negationInd
        {
            get
            {
                return this.negationIndField;
            }
            set
            {
                this.negationIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool negationIndSpecified
        {
            get
            {
                return this.negationIndFieldSpecified;
            }
            set
            {
                this.negationIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReferenceRange", Namespace = "urn:hl7-org:v3")]
    public partial class ReferenceRange
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private ObservationRange observationRangeField;

        private string nullFlavorField;

        private string typeCodeField;

        public ReferenceRange()
        {
            this.typeCodeField = "REFV";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public ObservationRange observationRange
        {
            get
            {
                return this.observationRangeField;
            }
            set
            {
                this.observationRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ObservationRange", Namespace = "urn:hl7-org:v3")]
    public partial class ObservationRange
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CD codeField;

        private ED textField;

        private ANY valueField;

        private CE interpretationCodeField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public ObservationRange()
        {
            this.classCodeField = "OBS";
            this.moodCodeField = "EVN.CRT";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public ANY value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public CE interpretationCode
        {
            get
            {
                return this.interpretationCodeField;
            }
            set
            {
                this.interpretationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("OBS")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ActMoodDocumentObservation
    {

        /// <remarks/>
        INT,

        /// <remarks/>
        DEF,

        /// <remarks/>
        EVN,

        /// <remarks/>
        GOL,

        /// <remarks/>
        PRMS,

        /// <remarks/>
        PRP,

        /// <remarks/>
        RQO,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ObservationMedia", Namespace = "urn:hl7-org:v3")]
    public partial class ObservationMedia
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CS languageCodeField;

        private ED valueField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private string idField1;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        public ED value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField1;
            }
            set
            {
                this.idField1 = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Organizer", Namespace = "urn:hl7-org:v3")]
    public partial class Organizer
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private CS statusCodeField;

        private IVL_TS effectiveTimeField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private List<Component4> componentField;

        private string nullFlavorField;

        private x_ActClassDocumentEntryOrganizer classCodeField;

        private string moodCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("component")]
        public List<Component4> component
        {
            get
            {
                return this.componentField;
            }
            set
            {
                this.componentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_ActClassDocumentEntryOrganizer classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Component4", Namespace = "urn:hl7-org:v3")]
    public partial class Component4
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private INT sequenceNumberField;

        private BL seperatableIndField;

        private object itemField;

        private string nullFlavorField;

        private ActRelationshipHasComponent typeCodeField;

        private bool typeCodeFieldSpecified;

        private bool contextConductionIndField;

        private bool contextConductionIndFieldSpecified;

        public Component4()
        {
            this.typeCodeField = ActRelationshipHasComponent.COMP;
            this.contextConductionIndField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public INT sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public BL seperatableInd
        {
            get
            {
                return this.seperatableIndField;
            }
            set
            {
                this.seperatableIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("act", typeof(Act))]
        [System.Xml.Serialization.XmlElementAttribute("encounter", typeof(Encounter))]
        [System.Xml.Serialization.XmlElementAttribute("observation", typeof(Observation))]
        [System.Xml.Serialization.XmlElementAttribute("observationMedia", typeof(ObservationMedia))]
        [System.Xml.Serialization.XmlElementAttribute("organizer", typeof(Organizer))]
        [System.Xml.Serialization.XmlElementAttribute("procedure", typeof(Procedure))]
        [System.Xml.Serialization.XmlElementAttribute("regionOfInterest", typeof(RegionOfInterest))]
        [System.Xml.Serialization.XmlElementAttribute("substanceAdministration", typeof(SubstanceAdministration))]
        [System.Xml.Serialization.XmlElementAttribute("supply", typeof(Supply))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActRelationshipHasComponent typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool contextConductionInd
        {
            get
            {
                return this.contextConductionIndField;
            }
            set
            {
                this.contextConductionIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool contextConductionIndSpecified
        {
            get
            {
                return this.contextConductionIndFieldSpecified;
            }
            set
            {
                this.contextConductionIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Procedure", Namespace = "urn:hl7-org:v3")]
    public partial class Procedure
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private CS statusCodeField;

        private IVL_TS effectiveTimeField;

        private CE priorityCodeField;

        private CS languageCodeField;

        private List<CE> methodCodeField;

        private List<CD> approachSiteCodeField;

        private List<CD> targetSiteCodeField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private string nullFlavorField;

        private string classCodeField;

        private x_DocumentProcedureMood moodCodeField;

        private bool negationIndField;

        private bool negationIndFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public CE priorityCode
        {
            get
            {
                return this.priorityCodeField;
            }
            set
            {
                this.priorityCodeField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("methodCode")]
        public List<CE> methodCode
        {
            get
            {
                return this.methodCodeField;
            }
            set
            {
                this.methodCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("approachSiteCode")]
        public List<CD> approachSiteCode
        {
            get
            {
                return this.approachSiteCodeField;
            }
            set
            {
                this.approachSiteCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("targetSiteCode")]
        public List<CD> targetSiteCode
        {
            get
            {
                return this.targetSiteCodeField;
            }
            set
            {
                this.targetSiteCodeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_DocumentProcedureMood moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool negationInd
        {
            get
            {
                return this.negationIndField;
            }
            set
            {
                this.negationIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool negationIndSpecified
        {
            get
            {
                return this.negationIndFieldSpecified;
            }
            set
            {
                this.negationIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_DocumentProcedureMood
    {

        /// <remarks/>
        INT,

        /// <remarks/>
        APT,

        /// <remarks/>
        ARQ,

        /// <remarks/>
        DEF,

        /// <remarks/>
        EVN,

        /// <remarks/>
        PRMS,

        /// <remarks/>
        PRP,

        /// <remarks/>
        RQO,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "RegionOfInterest", Namespace = "urn:hl7-org:v3")]
    public partial class RegionOfInterest
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CS codeField;

        private List<RegionOfInterestvalue> valueField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private string idField1;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public RegionOfInterest()
        {
            this.classCodeField = "ROIOVL";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CS code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public List<RegionOfInterestvalue> value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return this.idField1;
            }
            set
            {
                this.idField1 = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SubstanceAdministration", Namespace = "urn:hl7-org:v3")]
    public partial class SubstanceAdministration
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private CS statusCodeField;

        private List<SXCM_TS> effectiveTimeField;

        private CE priorityCodeField;

        private IVL_INT repeatNumberField;

        private CE routeCodeField;

        private List<CD> approachSiteCodeField;

        private IVL_PQ doseQuantityField;

        private IVL_PQ rateQuantityField;

        private RTO_PQ_PQ maxDoseQuantityField;

        private CE administrationUnitCodeField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private Consumable consumableField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private string nullFlavorField;

        private string classCodeField;

        private x_DocumentSubstanceMood moodCodeField;

        private bool negationIndField;

        private bool negationIndFieldSpecified;

        public SubstanceAdministration()
        {
            this.classCodeField = "SBADM";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("effectiveTime")]
        public List<SXCM_TS> effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public CE priorityCode
        {
            get
            {
                return this.priorityCodeField;
            }
            set
            {
                this.priorityCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_INT repeatNumber
        {
            get
            {
                return this.repeatNumberField;
            }
            set
            {
                this.repeatNumberField = value;
            }
        }

        /// <remarks/>
        public CE routeCode
        {
            get
            {
                return this.routeCodeField;
            }
            set
            {
                this.routeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("approachSiteCode")]
        public List<CD> approachSiteCode
        {
            get
            {
                return this.approachSiteCodeField;
            }
            set
            {
                this.approachSiteCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_PQ doseQuantity
        {
            get
            {
                return this.doseQuantityField;
            }
            set
            {
                this.doseQuantityField = value;
            }
        }

        /// <remarks/>
        public IVL_PQ rateQuantity
        {
            get
            {
                return this.rateQuantityField;
            }
            set
            {
                this.rateQuantityField = value;
            }
        }

        /// <remarks/>
        public RTO_PQ_PQ maxDoseQuantity
        {
            get
            {
                return this.maxDoseQuantityField;
            }
            set
            {
                this.maxDoseQuantityField = value;
            }
        }

        /// <remarks/>
        public CE administrationUnitCode
        {
            get
            {
                return this.administrationUnitCodeField;
            }
            set
            {
                this.administrationUnitCodeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        public Consumable consumable
        {
            get
            {
                return this.consumableField;
            }
            set
            {
                this.consumableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_DocumentSubstanceMood moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool negationInd
        {
            get
            {
                return this.negationIndField;
            }
            set
            {
                this.negationIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool negationIndSpecified
        {
            get
            {
                return this.negationIndFieldSpecified;
            }
            set
            {
                this.negationIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class RTO_PQ_PQ : QTY
    {

        private PQ numeratorField;

        private PQ denominatorField;

        /// <remarks/>
        public PQ numerator
        {
            get
            {
                return this.numeratorField;
            }
            set
            {
                this.numeratorField = value;
            }
        }

        /// <remarks/>
        public PQ denominator
        {
            get
            {
                return this.denominatorField;
            }
            set
            {
                this.denominatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Consumable", Namespace = "urn:hl7-org:v3")]
    public partial class Consumable
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private ManufacturedProduct manufacturedProductField;

        private string nullFlavorField;

        private string typeCodeField;

        public Consumable()
        {
            this.typeCodeField = "CSM";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public ManufacturedProduct manufacturedProduct
        {
            get
            {
                return this.manufacturedProductField;
            }
            set
            {
                this.manufacturedProductField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ManufacturedProduct", Namespace = "urn:hl7-org:v3")]
    public partial class ManufacturedProduct
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private object itemField;

        private Organization manufacturerOrganizationField;

        private string nullFlavorField;

        private RoleClassManufacturedProduct classCodeField;

        private bool classCodeFieldSpecified;

        public ManufacturedProduct()
        {
            this.classCodeField = RoleClassManufacturedProduct.MANU;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("manufacturedLabeledDrug", typeof(LabeledDrug))]
        [System.Xml.Serialization.XmlElementAttribute("manufacturedMaterial", typeof(Material))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public Organization manufacturerOrganization
        {
            get
            {
                return this.manufacturerOrganizationField;
            }
            set
            {
                this.manufacturerOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public RoleClassManufacturedProduct classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classCodeSpecified
        {
            get
            {
                return this.classCodeFieldSpecified;
            }
            set
            {
                this.classCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "LabeledDrug", Namespace = "urn:hl7-org:v3")]
    public partial class LabeledDrug
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE codeField;

        private EN nameField;

        private string nullFlavorField;

        private string classCodeField;

        private EntityDeterminerDetermined determinerCodeField;

        private bool determinerCodeFieldSpecified;

        public LabeledDrug()
        {
            this.classCodeField = "MMAT";
            this.determinerCodeField = EntityDeterminerDetermined.KIND;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public EN name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EntityDeterminerDetermined determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool determinerCodeSpecified
        {
            get
            {
                return this.determinerCodeFieldSpecified;
            }
            set
            {
                this.determinerCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum EntityDeterminerDetermined
    {

        /// <remarks/>
        KIND,

        /// <remarks/>
        QUANTIFIED_KIND,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Material", Namespace = "urn:hl7-org:v3")]
    public partial class Material
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE codeField;

        private EN nameField;

        private ST lotNumberTextField;

        private string nullFlavorField;

        private string classCodeField;

        private EntityDeterminerDetermined determinerCodeField;

        private bool determinerCodeFieldSpecified;

        public Material()
        {
            this.classCodeField = "MMAT";
            this.determinerCodeField = EntityDeterminerDetermined.KIND;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public EN name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public ST lotNumberText
        {
            get
            {
                return this.lotNumberTextField;
            }
            set
            {
                this.lotNumberTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EntityDeterminerDetermined determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool determinerCodeSpecified
        {
            get
            {
                return this.determinerCodeFieldSpecified;
            }
            set
            {
                this.determinerCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum RoleClassManufacturedProduct
    {

        /// <remarks/>
        MANU,

        /// <remarks/>
        THER,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_DocumentSubstanceMood
    {

        /// <remarks/>
        INT,

        /// <remarks/>
        EVN,

        /// <remarks/>
        PRMS,

        /// <remarks/>
        PRP,

        /// <remarks/>
        RQO,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Supply", Namespace = "urn:hl7-org:v3")]
    public partial class Supply
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private CS statusCodeField;

        private List<SXCM_TS> effectiveTimeField;

        private List<CE> priorityCodeField;

        private IVL_INT repeatNumberField;

        private BL independentIndField;

        private PQ quantityField;

        private IVL_TS expectedUseTimeField;

        private Subject subjectField;

        private List<Specimen> specimenField;

        private Product productField;

        private List<Performer2> performerField;

        private List<Author> authorField;

        private List<Informant12> informantField;

        private List<Participant2> participantField;

        private List<EntryRelationship> entryRelationshipField;

        private List<Reference> referenceField;

        private List<Precondition> preconditionField;

        private string nullFlavorField;

        private ActClassSupply classCodeField;

        private x_DocumentSubstanceMood moodCodeField;

        public Supply()
        {
            this.classCodeField = ActClassSupply.SPLY;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("effectiveTime")]
        public List<SXCM_TS> effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("priorityCode")]
        public List<CE> priorityCode
        {
            get
            {
                return this.priorityCodeField;
            }
            set
            {
                this.priorityCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_INT repeatNumber
        {
            get
            {
                return this.repeatNumberField;
            }
            set
            {
                this.repeatNumberField = value;
            }
        }

        /// <remarks/>
        public BL independentInd
        {
            get
            {
                return this.independentIndField;
            }
            set
            {
                this.independentIndField = value;
            }
        }

        /// <remarks/>
        public PQ quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public IVL_TS expectedUseTime
        {
            get
            {
                return this.expectedUseTimeField;
            }
            set
            {
                this.expectedUseTimeField = value;
            }
        }

        /// <remarks/>
        public Subject subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specimen")]
        public List<Specimen> specimen
        {
            get
            {
                return this.specimenField;
            }
            set
            {
                this.specimenField = value;
            }
        }

        /// <remarks/>
        public Product product
        {
            get
            {
                return this.productField;
            }
            set
            {
                this.productField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer2> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("author")]
        public List<Author> author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("informant")]
        public List<Informant12> informant
        {
            get
            {
                return this.informantField;
            }
            set
            {
                this.informantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("participant")]
        public List<Participant2> participant
        {
            get
            {
                return this.participantField;
            }
            set
            {
                this.participantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entryRelationship")]
        public List<EntryRelationship> entryRelationship
        {
            get
            {
                return this.entryRelationshipField;
            }
            set
            {
                this.entryRelationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public List<Reference> reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("precondition")]
        public List<Precondition> precondition
        {
            get
            {
                return this.preconditionField;
            }
            set
            {
                this.preconditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActClassSupply classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_DocumentSubstanceMood moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Product", Namespace = "urn:hl7-org:v3")]
    public partial class Product
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private ManufacturedProduct manufacturedProductField;

        private string nullFlavorField;

        private string typeCodeField;

        public Product()
        {
            this.typeCodeField = "PRD";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public ManufacturedProduct manufacturedProduct
        {
            get
            {
                return this.manufacturedProductField;
            }
            set
            {
                this.manufacturedProductField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ActClassSupply
    {

        /// <remarks/>
        SPLY,

        /// <remarks/>
        DIET,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ActRelationshipHasComponent
    {

        /// <remarks/>
        COMP,

        /// <remarks/>
        ARR,

        /// <remarks/>
        CTRLV,

        /// <remarks/>
        DEP,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ActClassDocumentEntryOrganizer
    {

        /// <remarks/>
        BATTERY,

        /// <remarks/>
        CLUSTER,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ActRelationshipEntryRelationship
    {

        /// <remarks/>
        XCRPT,

        /// <remarks/>
        COMP,

        /// <remarks/>
        RSON,

        /// <remarks/>
        SPRT,

        /// <remarks/>
        CAUS,

        /// <remarks/>
        GEVL,

        /// <remarks/>
        MFST,

        /// <remarks/>
        REFR,

        /// <remarks/>
        SAS,

        /// <remarks/>
        SUBJ,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ActClassDocumentEntryAct
    {

        /// <remarks/>
        ACT,

        /// <remarks/>
        ACCM,

        /// <remarks/>
        CONS,

        /// <remarks/>
        CTTEVENT,

        /// <remarks/>
        INC,

        /// <remarks/>
        INFRM,

        /// <remarks/>
        PCPR,

        /// <remarks/>
        REG,

        /// <remarks/>
        SPCTRT,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_DocumentActMood
    {

        /// <remarks/>
        INT,

        /// <remarks/>
        APT,

        /// <remarks/>
        ARQ,

        /// <remarks/>
        DEF,

        /// <remarks/>
        EVN,

        /// <remarks/>
        PRMS,

        /// <remarks/>
        PRP,

        /// <remarks/>
        RQO,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ActRelationshipEntry
    {

        /// <remarks/>
        COMP,

        /// <remarks/>
        DRIV,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Component3", Namespace = "urn:hl7-org:v3")]
    public partial class Component3
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private Section sectionField;

        private string nullFlavorField;

        private ActRelationshipHasComponent typeCodeField;

        private bool typeCodeFieldSpecified;

        private bool contextConductionIndField;

        private bool contextConductionIndFieldSpecified;

        public Component3()
        {
            this.typeCodeField = ActRelationshipHasComponent.COMP;
            this.contextConductionIndField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public Section section
        {
            get
            {
                return this.sectionField;
            }
            set
            {
                this.sectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActRelationshipHasComponent typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool contextConductionInd
        {
            get
            {
                return this.contextConductionIndField;
            }
            set
            {
                this.contextConductionIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool contextConductionIndSpecified
        {
            get
            {
                return this.contextConductionIndFieldSpecified;
            }
            set
            {
                this.contextConductionIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StructuredBody", Namespace = "urn:hl7-org:v3")]
    public partial class StructuredBody
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE confidentialityCodeField;

        private CS languageCodeField;

        private List<Component3> componentField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public StructuredBody()
        {
            this.classCodeField = "DOCBODY";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE confidentialityCode
        {
            get
            {
                return this.confidentialityCodeField;
            }
            set
            {
                this.confidentialityCodeField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("component")]
        public List<Component3> component
        {
            get
            {
                return this.componentField;
            }
            set
            {
                this.componentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "NonXMLBody", Namespace = "urn:hl7-org:v3")]
    public partial class NonXMLBody
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private ED textField;

        private CE confidentialityCodeField;

        private CS languageCodeField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public NonXMLBody()
        {
            this.classCodeField = "DOCBODY";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public CE confidentialityCode
        {
            get
            {
                return this.confidentialityCodeField;
            }
            set
            {
                this.confidentialityCodeField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Component2", Namespace = "urn:hl7-org:v3")]
    public partial class Component2
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private object itemField;

        private string nullFlavorField;

        private ActRelationshipHasComponent typeCodeField;

        private bool typeCodeFieldSpecified;

        private bool contextConductionIndField;

        private bool contextConductionIndFieldSpecified;

        public Component2()
        {
            this.typeCodeField = ActRelationshipHasComponent.COMP;
            this.contextConductionIndField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nonXMLBody", typeof(NonXMLBody))]
        [System.Xml.Serialization.XmlElementAttribute("structuredBody", typeof(StructuredBody))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActRelationshipHasComponent typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool contextConductionInd
        {
            get
            {
                return this.contextConductionIndField;
            }
            set
            {
                this.contextConductionIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool contextConductionIndSpecified
        {
            get
            {
                return this.contextConductionIndFieldSpecified;
            }
            set
            {
                this.contextConductionIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "HealthCareFacility", Namespace = "urn:hl7-org:v3")]
    public partial class HealthCareFacility
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private Place locationField;

        private Organization serviceProviderOrganizationField;

        private string nullFlavorField;

        private RoleClassServiceDeliveryLocation classCodeField;

        public HealthCareFacility()
        {
            this.classCodeField = RoleClassServiceDeliveryLocation.SDLOC;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public Place location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public Organization serviceProviderOrganization
        {
            get
            {
                return this.serviceProviderOrganizationField;
            }
            set
            {
                this.serviceProviderOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(RoleClassServiceDeliveryLocation.SDLOC)]
        public RoleClassServiceDeliveryLocation classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Place", Namespace = "urn:hl7-org:v3")]
    public partial class Place
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private EN nameField;

        private AD addrField;

        private string nullFlavorField;

        private EntityClassPlace classCodeField;

        private bool classCodeFieldSpecified;

        private string determinerCodeField;

        public Place()
        {
            this.classCodeField = EntityClassPlace.PLC;
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public EN name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AD addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EntityClassPlace classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classCodeSpecified
        {
            get
            {
                return this.classCodeFieldSpecified;
            }
            set
            {
                this.classCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum EntityClassPlace
    {

        /// <remarks/>
        PLC,

        /// <remarks/>
        CITY,

        /// <remarks/>
        COUNTRY,

        /// <remarks/>
        COUNTY,

        /// <remarks/>
        PROVINCE,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum RoleClassServiceDeliveryLocation
    {

        /// <remarks/>
        SDLOC,

        /// <remarks/>
        DSDLOC,

        /// <remarks/>
        ISDLOC,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Location", Namespace = "urn:hl7-org:v3")]
    public partial class Location
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private HealthCareFacility healthCareFacilityField;

        private string nullFlavorField;

        private ParticipationTargetLocation typeCodeField;

        private bool typeCodeFieldSpecified;

        public Location()
        {
            this.typeCodeField = ParticipationTargetLocation.LOC;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public HealthCareFacility healthCareFacility
        {
            get
            {
                return this.healthCareFacilityField;
            }
            set
            {
                this.healthCareFacilityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ParticipationTargetLocation typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ParticipationTargetLocation
    {

        /// <remarks/>
        LOC,

        /// <remarks/>
        DST,

        /// <remarks/>
        ELOC,

        /// <remarks/>
        ORG,

        /// <remarks/>
        RML,

        /// <remarks/>
        VIA,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "EncounterParticipant", Namespace = "urn:hl7-org:v3")]
    public partial class EncounterParticipant
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private IVL_TS timeField;

        private AssignedEntity assignedEntityField;

        private string nullFlavorField;

        private x_EncounterParticipant typeCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public IVL_TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public AssignedEntity assignedEntity
        {
            get
            {
                return this.assignedEntityField;
            }
            set
            {
                this.assignedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_EncounterParticipant typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_EncounterParticipant
    {

        /// <remarks/>
        ADM,

        /// <remarks/>
        ATND,

        /// <remarks/>
        CON,

        /// <remarks/>
        DIS,

        /// <remarks/>
        REF,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ResponsibleParty", Namespace = "urn:hl7-org:v3")]
    public partial class ResponsibleParty
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private AssignedEntity assignedEntityField;

        private string nullFlavorField;

        private string typeCodeField;

        public ResponsibleParty()
        {
            this.typeCodeField = "RESP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public AssignedEntity assignedEntity
        {
            get
            {
                return this.assignedEntityField;
            }
            set
            {
                this.assignedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "EncompassingEncounter", Namespace = "urn:hl7-org:v3")]
    public partial class EncompassingEncounter
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private IVL_TS effectiveTimeField;

        private CE dischargeDispositionCodeField;

        private ResponsibleParty responsiblePartyField;

        private List<EncounterParticipant> encounterParticipantField;

        private Location locationField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public EncompassingEncounter()
        {
            this.classCodeField = "ENC";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        public CE dischargeDispositionCode
        {
            get
            {
                return this.dischargeDispositionCodeField;
            }
            set
            {
                this.dischargeDispositionCodeField = value;
            }
        }

        /// <remarks/>
        public ResponsibleParty responsibleParty
        {
            get
            {
                return this.responsiblePartyField;
            }
            set
            {
                this.responsiblePartyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("encounterParticipant")]
        public List<EncounterParticipant> encounterParticipant
        {
            get
            {
                return this.encounterParticipantField;
            }
            set
            {
                this.encounterParticipantField = value;
            }
        }

        /// <remarks/>
        public Location location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Component1", Namespace = "urn:hl7-org:v3")]
    public partial class Component1
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private EncompassingEncounter encompassingEncounterField;

        private string nullFlavorField;

        private ActRelationshipHasComponent typeCodeField;

        private bool typeCodeFieldSpecified;

        public Component1()
        {
            this.typeCodeField = ActRelationshipHasComponent.COMP;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public EncompassingEncounter encompassingEncounter
        {
            get
            {
                return this.encompassingEncounterField;
            }
            set
            {
                this.encompassingEncounterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActRelationshipHasComponent typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Consent", Namespace = "urn:hl7-org:v3")]
    public partial class Consent
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private CS statusCodeField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public Consent()
        {
            this.classCodeField = "CONS";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public CS statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Authorization", Namespace = "urn:hl7-org:v3")]
    public partial class Authorization
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private Consent consentField;

        private string nullFlavorField;

        private string typeCodeField;

        public Authorization()
        {
            this.typeCodeField = "AUTH";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public Consent consent
        {
            get
            {
                return this.consentField;
            }
            set
            {
                this.consentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ParentDocument", Namespace = "urn:hl7-org:v3")]
    public partial class ParentDocument
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CD codeField;

        private ED textField;

        private II setIdField;

        private INT versionNumberField;

        private string nullFlavorField;

        private ActClinicalDocument classCodeField;

        private bool classCodeFieldSpecified;

        private string moodCodeField;

        public ParentDocument()
        {
            this.classCodeField = ActClinicalDocument.DOCCLIN;
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CD code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ED text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public II setId
        {
            get
            {
                return this.setIdField;
            }
            set
            {
                this.setIdField = value;
            }
        }

        /// <remarks/>
        public INT versionNumber
        {
            get
            {
                return this.versionNumberField;
            }
            set
            {
                this.versionNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActClinicalDocument classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classCodeSpecified
        {
            get
            {
                return this.classCodeFieldSpecified;
            }
            set
            {
                this.classCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ActClinicalDocument
    {

        /// <remarks/>
        DOCCLIN,

        /// <remarks/>
        CDALVLONE,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "RelatedDocument", Namespace = "urn:hl7-org:v3")]
    public partial class RelatedDocument
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private ParentDocument parentDocumentField;

        private string nullFlavorField;

        private x_ActRelationshipDocument typeCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public ParentDocument parentDocument
        {
            get
            {
                return this.parentDocumentField;
            }
            set
            {
                this.parentDocumentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_ActRelationshipDocument typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ActRelationshipDocument
    {

        /// <remarks/>
        RPLC,

        /// <remarks/>
        APND,

        /// <remarks/>
        XFRM,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Performer1", Namespace = "urn:hl7-org:v3")]
    public partial class Performer1
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE functionCodeField;

        private IVL_TS timeField;

        private AssignedEntity assignedEntityField;

        private string nullFlavorField;

        private x_ServiceEventPerformer typeCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE functionCode
        {
            get
            {
                return this.functionCodeField;
            }
            set
            {
                this.functionCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public AssignedEntity assignedEntity
        {
            get
            {
                return this.assignedEntityField;
            }
            set
            {
                this.assignedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public x_ServiceEventPerformer typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_ServiceEventPerformer
    {

        /// <remarks/>
        PRF,

        /// <remarks/>
        PPRF,

        /// <remarks/>
        SPRF,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ServiceEvent", Namespace = "urn:hl7-org:v3")]
    public partial class ServiceEvent
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private IVL_TS effectiveTimeField;

        private List<Performer1> performerField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public ServiceEvent()
        {
            this.classCodeField = "ACT";
            this.moodCodeField = "EVN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("performer")]
        public List<Performer1> performer
        {
            get
            {
                return this.performerField;
            }
            set
            {
                this.performerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("ACT")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "DocumentationOf", Namespace = "urn:hl7-org:v3")]
    public partial class DocumentationOf
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private ServiceEvent serviceEventField;

        private string nullFlavorField;

        private string typeCodeField;

        public DocumentationOf()
        {
            this.typeCodeField = "DOC";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public ServiceEvent serviceEvent
        {
            get
            {
                return this.serviceEventField;
            }
            set
            {
                this.serviceEventField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Order", Namespace = "urn:hl7-org:v3")]
    public partial class Order
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private CE priorityCodeField;

        private string nullFlavorField;

        private string classCodeField;

        private string moodCodeField;

        public Order()
        {
            this.classCodeField = "ACT";
            this.moodCodeField = "RQO";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public CE priorityCode
        {
            get
            {
                return this.priorityCodeField;
            }
            set
            {
                this.priorityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("ACT")]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moodCode
        {
            get
            {
                return this.moodCodeField;
            }
            set
            {
                this.moodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "InFulfillmentOf", Namespace = "urn:hl7-org:v3")]
    public partial class InFulfillmentOf
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private Order orderField;

        private string nullFlavorField;

        private ActRelationshipFulfills typeCodeField;

        private bool typeCodeFieldSpecified;

        public InFulfillmentOf()
        {
            this.typeCodeField = ActRelationshipFulfills.FLFS;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public Order order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActRelationshipFulfills typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum ActRelationshipFulfills
    {

        /// <remarks/>
        FLFS,

        /// <remarks/>
        OCCR,

        /// <remarks/>
        OREF,

        /// <remarks/>
        SCH,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "AssociatedEntity", Namespace = "urn:hl7-org:v3")]
    public partial class AssociatedEntity
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private Person associatedPersonField;

        private Organization scopingOrganizationField;

        private string nullFlavorField;

        private string classCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        public Person associatedPerson
        {
            get
            {
                return this.associatedPersonField;
            }
            set
            {
                this.associatedPersonField = value;
            }
        }

        /// <remarks/>
        public Organization scopingOrganization
        {
            get
            {
                return this.scopingOrganizationField;
            }
            set
            {
                this.scopingOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Participant1", Namespace = "urn:hl7-org:v3")]
    public partial class Participant1
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CE functionCodeField;

        private IVL_TS timeField;

        private AssociatedEntity associatedEntityField;

        private string nullFlavorField;

        private string typeCodeField;

        private string contextControlCodeField;

        public Participant1()
        {
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CE functionCode
        {
            get
            {
                return this.functionCodeField;
            }
            set
            {
                this.functionCodeField = value;
            }
        }

        /// <remarks/>
        public IVL_TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public AssociatedEntity associatedEntity
        {
            get
            {
                return this.associatedEntityField;
            }
            set
            {
                this.associatedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Authenticator", Namespace = "urn:hl7-org:v3")]
    public partial class Authenticator
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private TS timeField;

        private CS signatureCodeField;

        private AssignedEntity assignedEntityField;

        private string nullFlavorField;

        private string typeCodeField;

        public Authenticator()
        {
            this.typeCodeField = "AUTHEN";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public CS signatureCode
        {
            get
            {
                return this.signatureCodeField;
            }
            set
            {
                this.signatureCodeField = value;
            }
        }

        /// <remarks/>
        public AssignedEntity assignedEntity
        {
            get
            {
                return this.assignedEntityField;
            }
            set
            {
                this.assignedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "LegalAuthenticator", Namespace = "urn:hl7-org:v3")]
    public partial class LegalAuthenticator
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private TS timeField;

        private CS signatureCodeField;

        private AssignedEntity assignedEntityField;

        private string nullFlavorField;

        private string typeCodeField;

        private string contextControlCodeField;

        public LegalAuthenticator()
        {
            this.typeCodeField = "LA";
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public CS signatureCode
        {
            get
            {
                return this.signatureCodeField;
            }
            set
            {
                this.signatureCodeField = value;
            }
        }

        /// <remarks/>
        public AssignedEntity assignedEntity
        {
            get
            {
                return this.assignedEntityField;
            }
            set
            {
                this.assignedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "IntendedRecipient", Namespace = "urn:hl7-org:v3")]
    public partial class IntendedRecipient
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private Person informationRecipientField;

        private Organization receivedOrganizationField;

        private string nullFlavorField;

        private x_InformationRecipientRole classCodeField;

        public IntendedRecipient()
        {
            this.classCodeField = x_InformationRecipientRole.ASSIGNED;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        public Person informationRecipient
        {
            get
            {
                return this.informationRecipientField;
            }
            set
            {
                this.informationRecipientField = value;
            }
        }

        /// <remarks/>
        public Organization receivedOrganization
        {
            get
            {
                return this.receivedOrganizationField;
            }
            set
            {
                this.receivedOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(x_InformationRecipientRole.ASSIGNED)]
        public x_InformationRecipientRole classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_InformationRecipientRole
    {

        /// <remarks/>
        ASSIGNED,

        /// <remarks/>
        HLTHCHRT,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "InformationRecipient", Namespace = "urn:hl7-org:v3")]
    public partial class InformationRecipient
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private IntendedRecipient intendedRecipientField;

        private string nullFlavorField;

        private x_InformationRecipient typeCodeField;

        public InformationRecipient()
        {
            this.typeCodeField = x_InformationRecipient.PRCP;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public IntendedRecipient intendedRecipient
        {
            get
            {
                return this.intendedRecipientField;
            }
            set
            {
                this.intendedRecipientField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(x_InformationRecipient.PRCP)]
        public x_InformationRecipient typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum x_InformationRecipient
    {

        /// <remarks/>
        PRCP,

        /// <remarks/>
        TRC,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "CustodianOrganization", Namespace = "urn:hl7-org:v3")]
    public partial class CustodianOrganization
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private ON nameField;

        private TEL telecomField;

        private AD addrField;

        private string nullFlavorField;

        private string classCodeField;

        private string determinerCodeField;

        public CustodianOrganization()
        {
            this.classCodeField = "ORG";
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public ON name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public TEL telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        public AD addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "AssignedCustodian", Namespace = "urn:hl7-org:v3")]
    public partial class AssignedCustodian
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CustodianOrganization representedCustodianOrganizationField;

        private string nullFlavorField;

        private string classCodeField;

        public AssignedCustodian()
        {
            this.classCodeField = "ASSIGNED";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CustodianOrganization representedCustodianOrganization
        {
            get
            {
                return this.representedCustodianOrganizationField;
            }
            set
            {
                this.representedCustodianOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Custodian", Namespace = "urn:hl7-org:v3")]
    public partial class Custodian
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private AssignedCustodian assignedCustodianField;

        private string nullFlavorField;

        private string typeCodeField;

        public Custodian()
        {
            this.typeCodeField = "CST";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public AssignedCustodian assignedCustodian
        {
            get
            {
                return this.assignedCustodianField;
            }
            set
            {
                this.assignedCustodianField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "DataEnterer", Namespace = "urn:hl7-org:v3")]
    public partial class DataEnterer
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private TS timeField;

        private AssignedEntity assignedEntityField;

        private string nullFlavorField;

        private string typeCodeField;

        private string contextControlCodeField;

        public DataEnterer()
        {
            this.typeCodeField = "ENT";
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public TS time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public AssignedEntity assignedEntity
        {
            get
            {
                return this.assignedEntityField;
            }
            set
            {
                this.assignedEntityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "LanguageCommunication", Namespace = "urn:hl7-org:v3")]
    public partial class LanguageCommunication
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private CS languageCodeField;

        private CE modeCodeField;

        private CE proficiencyLevelCodeField;

        private BL preferenceIndField;

        private string nullFlavorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public CS languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        public CE modeCode
        {
            get
            {
                return this.modeCodeField;
            }
            set
            {
                this.modeCodeField = value;
            }
        }

        /// <remarks/>
        public CE proficiencyLevelCode
        {
            get
            {
                return this.proficiencyLevelCodeField;
            }
            set
            {
                this.proficiencyLevelCodeField = value;
            }
        }

        /// <remarks/>
        public BL preferenceInd
        {
            get
            {
                return this.preferenceIndField;
            }
            set
            {
                this.preferenceIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Birthplace", Namespace = "urn:hl7-org:v3")]
    public partial class Birthplace
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private Place placeField;

        private string nullFlavorField;

        private string classCodeField;

        public Birthplace()
        {
            this.classCodeField = "BIRTHPL";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public Place place
        {
            get
            {
                return this.placeField;
            }
            set
            {
                this.placeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Guardian", Namespace = "urn:hl7-org:v3")]
    public partial class Guardian
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private CE codeField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private object itemField;

        private string nullFlavorField;

        private string classCodeField;

        public Guardian()
        {
            this.classCodeField = "GUARD";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public CE code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("guardianOrganization", typeof(Organization))]
        [System.Xml.Serialization.XmlElementAttribute("guardianPerson", typeof(Person))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "Patient", Namespace = "urn:hl7-org:v3")]
    public partial class Patient
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private II idField;

        private List<PN> nameField;

        private CE administrativeGenderCodeField;

        private TS birthTimeField;

        private CE maritalStatusCodeField;

        private CE religiousAffiliationCodeField;

        private CE raceCodeField;

        private List<CE> raceCode1Field;
        private List<CE> raceCode2Field;
        private CD valueSet1Field;
        private CE ethnicGroupCodeField;

        private List<Guardian> guardianField;

        private Birthplace birthplaceField;


        private string nullFlavorField;

        private string classCodeField;

        private string determinerCodeField;

        private List<DE> ethnicGroupCode1Field;

        private List<LanguageCommunication> languageCommunicationField;
        public Patient()
        {
            this.classCodeField = "PSN";
            this.determinerCodeField = "INSTANCE";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public II id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public List<PN> name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public CE administrativeGenderCode
        {
            get
            {
                return this.administrativeGenderCodeField;
            }
            set
            {
                this.administrativeGenderCodeField = value;
            }
        }

        /// <remarks/>
        public TS birthTime
        {
            get
            {
                return this.birthTimeField;
            }
            set
            {
                this.birthTimeField = value;
            }
        }

        /// <remarks/>
        public CE maritalStatusCode
        {
            get
            {
                return this.maritalStatusCodeField;
            }
            set
            {
                this.maritalStatusCodeField = value;
            }
        }

        /// <remarks/>
        public CE religiousAffiliationCode
        {
            get
            {
                return this.religiousAffiliationCodeField;
            }
            set
            {
                this.religiousAffiliationCodeField = value;
            }
        }

        /// <remarks/>
        public CE raceCode
        {
            get
            {
                return this.raceCodeField;
            }
            set
            {
                this.raceCodeField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("sdtc:valueSet")]
        public CD valueSet1
        {
            get
            {
                return this.valueSet1Field;
            }
            set
            {
                this.valueSet1Field = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sdtc:raceCode")]       
        public List<CE> raceCode1
        {
            get
            {
                return this.raceCode1Field;
            }
            set
            {
                this.raceCode1Field = value;
            }
        }

        /// <remarks/>

        [System.Xml.Serialization.XmlElementAttribute("raceCode", Namespace = "urn:hl7-org:sdtc")]
        public List<CE> raceCode2
        {
            get
            {
                return this.raceCode2Field;
            }
            set
            {
                this.raceCode2Field = value;
            }
        }

        /// <remarks/>
        public CE ethnicGroupCode
        {
            get
            {
                return this.ethnicGroupCodeField;
            }
            set
            {
                this.ethnicGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("guardian")]
        public List<Guardian> guardian
        {
            get
            {
                return this.guardianField;
            }
            set
            {
                this.guardianField = value;
            }
        }

        /// <remarks/>
        public Birthplace birthplace
        {
            get
            {
                return this.birthplaceField;
            }
            set
            {
                this.birthplaceField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sdtc:ethnicGroupCode")]
        public List<DE> ethnicGroupCode1
        {
            get
            {
                return this.ethnicGroupCode1Field;
            }
            set
            {
                this.ethnicGroupCode1Field = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("languageCommunication")]
        public List<LanguageCommunication> languageCommunication
        {
            get
            {
                return this.languageCommunicationField;
            }
            set
            {
                this.languageCommunicationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string determinerCode
        {
            get
            {
                return this.determinerCodeField;
            }
            set
            {
                this.determinerCodeField = value;
            }
        }
       
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "PatientRole", Namespace = "urn:hl7-org:v3")]
    public partial class PatientRole
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private List<II> idField;

        private List<AD> addrField;

        private List<TEL> telecomField;

        private Patient patientField;

        private Organization providerOrganizationField;

        private string nullFlavorField;

        private string classCodeField;

        public PatientRole()
        {
            this.classCodeField = "PAT";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id")]
        public List<II> id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addr")]
        public List<AD> addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("telecom")]
        public List<TEL> telecom
        {
            get
            {
                return this.telecomField;
            }
            set
            {
                this.telecomField = value;
            }
        }

        /// <remarks/>
        public Patient patient
        {
            get
            {
                return this.patientField;
            }
            set
            {
                this.patientField = value;
            }
        }

        /// <remarks/>
        public Organization providerOrganization
        {
            get
            {
                return this.providerOrganizationField;
            }
            set
            {
                this.providerOrganizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classCode
        {
            get
            {
                return this.classCodeField;
            }
            set
            {
                this.classCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "RecordTarget", Namespace = "urn:hl7-org:v3")]
    public partial class RecordTarget
    {

        private List<CS> realmCodeField;

        private InfrastructureRoottypeId typeIdField;

        private List<II> templateIdField;

        private PatientRole patientRoleField;

        private string nullFlavorField;

        private string typeCodeField;

        private string contextControlCodeField;

        public RecordTarget()
        {
            this.typeCodeField = "RCT";
            this.contextControlCodeField = "OP";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("realmCode")]
        public List<CS> realmCode
        {
            get
            {
                return this.realmCodeField;
            }
            set
            {
                this.realmCodeField = value;
            }
        }

        /// <remarks/>
        public InfrastructureRoottypeId typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateId")]
        public List<II> templateId
        {
            get
            {
                return this.templateIdField;
            }
            set
            {
                this.templateIdField = value;
            }
        }

        /// <remarks/>
        public PatientRole patientRole
        {
            get
            {
                return this.patientRoleField;
            }
            set
            {
                this.patientRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nullFlavor
        {
            get
            {
                return this.nullFlavorField;
            }
            set
            {
                this.nullFlavorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contextControlCode
        {
            get
            {
                return this.contextControlCodeField;
            }
            set
            {
                this.contextControlCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class GLIST_PQ : ANY
    {

        private PQ headField;

        private PQ incrementField;

        private string periodField;

        private string denominatorField;

        /// <remarks/>
        public PQ head
        {
            get
            {
                return this.headField;
            }
            set
            {
                this.headField = value;
            }
        }

        /// <remarks/>
        public PQ increment
        {
            get
            {
                return this.incrementField;
            }
            set
            {
                this.incrementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string period
        {
            get
            {
                return this.periodField;
            }
            set
            {
                this.periodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string denominator
        {
            get
            {
                return this.denominatorField;
            }
            set
            {
                this.denominatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class GLIST_TS : ANY
    {

        private TS headField;

        private PQ incrementField;

        private string periodField;

        private string denominatorField;

        /// <remarks/>
        public TS head
        {
            get
            {
                return this.headField;
            }
            set
            {
                this.headField = value;
            }
        }

        /// <remarks/>
        public PQ increment
        {
            get
            {
                return this.incrementField;
            }
            set
            {
                this.incrementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string period
        {
            get
            {
                return this.periodField;
            }
            set
            {
                this.periodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string denominator
        {
            get
            {
                return this.denominatorField;
            }
            set
            {
                this.denominatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SLIST_TS : ANY
    {

        private TS originField;

        private PQ scaleField;

        private string digitsField;

        /// <remarks/>
        public TS origin
        {
            get
            {
                return this.originField;
            }
            set
            {
                this.originField = value;
            }
        }

        /// <remarks/>
        public PQ scale
        {
            get
            {
                return this.scaleField;
            }
            set
            {
                this.scaleField = value;
            }
        }

        /// <remarks/>
        public string digits
        {
            get
            {
                return this.digitsField;
            }
            set
            {
                this.digitsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SLIST_PQ : ANY
    {

        private PQ originField;

        private PQ scaleField;

        private string digitsField;

        /// <remarks/>
        public PQ origin
        {
            get
            {
                return this.originField;
            }
            set
            {
                this.originField = value;
            }
        }

        /// <remarks/>
        public PQ scale
        {
            get
            {
                return this.scaleField;
            }
            set
            {
                this.scaleField = value;
            }
        }

        /// <remarks/>
        public string digits
        {
            get
            {
                return this.digitsField;
            }
            set
            {
                this.digitsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class CR : ANY
    {

        private CV nameField;

        private CD valueField;

        private bool invertedField;

        public CR()
        {
            this.invertedField = false;
        }

        /// <remarks/>
        public CV name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public CD value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool inverted
        {
            get
            {
                return this.invertedField;
            }
            set
            {
                this.invertedField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TEL))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public abstract partial class URL : ANY
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ED))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ST))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ENXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ensuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enprefix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(engiven))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(enfamily))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(endelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ADXP))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpprecinct))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostBox))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpbuildingNumberSuffix))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryModeIdentifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryMode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationQualifier))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationArea))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryInstallationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdeliveryAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcensusTract))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcareOf))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpunitID))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpadditionalLocator))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetNameBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetName))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdirection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumberNumeric))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxphouseNumber))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstreetAddressLine))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxppostalCode))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcounty))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpstate))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpcountry))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(adxpdelimiter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SC))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(thumbnail))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public abstract partial class BIN : ANY
    {

        private BinaryDataEncoding representationField;

        private List<string> textField;

        public BIN()
        {
            this.representationField = BinaryDataEncoding.TXT;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(BinaryDataEncoding.TXT)]
        public BinaryDataEncoding representation
        {
            get
            {
                return this.representationField;
            }
            set
            {
                this.representationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum BinaryDataEncoding
    {

        /// <remarks/>
        B64,

        /// <remarks/>
        TXT,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BN))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class ANYNonNull : ANY
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class BN : ANYNonNull
    {

        private bool valueField;

        private bool valueFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class RTO_MO_PQ : QTY
    {

        private MO numeratorField;

        private PQ denominatorField;

        /// <remarks/>
        public MO numerator
        {
            get
            {
                return this.numeratorField;
            }
            set
            {
                this.numeratorField = value;
            }
        }

        /// <remarks/>
        public PQ denominator
        {
            get
            {
                return this.denominatorField;
            }
            set
            {
                this.denominatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_MO))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_MO))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class MO : QTY
    {

        private string valueField;

        private string currencyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVXB_MO : MO
    {

        private bool inclusiveField;

        public IVXB_MO()
        {
            this.inclusiveField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_MO))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_MO : MO
    {

        private SetOperator operatorField;

        public SXCM_MO()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVL_MO : SXCM_MO
    {

        private MO[] itemsField;

        private ItemsChoiceType6[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("center", typeof(MO))]
        [System.Xml.Serialization.XmlElementAttribute("high", typeof(IVXB_MO))]
        [System.Xml.Serialization.XmlElementAttribute("low", typeof(IVXB_MO))]
        [System.Xml.Serialization.XmlElementAttribute("width", typeof(MO))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public MO[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType6[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3", IncludeInSchema = false)]
    public enum ItemsChoiceType6
    {

        /// <remarks/>
        center,

        /// <remarks/>
        high,

        /// <remarks/>
        low,

        /// <remarks/>
        width,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RTO))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class RTO_QTY_QTY : QTY
    {

        private QTY numeratorField;

        private QTY denominatorField;

        /// <remarks/>
        public QTY numerator
        {
            get
            {
                return this.numeratorField;
            }
            set
            {
                this.numeratorField = value;
            }
        }

        /// <remarks/>
        public QTY denominator
        {
            get
            {
                return this.denominatorField;
            }
            set
            {
                this.denominatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class RTO : RTO_QTY_QTY
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_REAL))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_REAL))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class REAL : QTY
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVXB_REAL : REAL
    {

        private bool inclusiveField;

        public IVXB_REAL()
        {
            this.inclusiveField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_REAL))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_REAL : REAL
    {

        private SetOperator operatorField;

        public SXCM_REAL()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVL_REAL : SXCM_REAL
    {

        private REAL[] itemsField;

        private ItemsChoiceType5[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("center", typeof(REAL))]
        [System.Xml.Serialization.XmlElementAttribute("high", typeof(IVXB_REAL))]
        [System.Xml.Serialization.XmlElementAttribute("low", typeof(IVXB_REAL))]
        [System.Xml.Serialization.XmlElementAttribute("width", typeof(REAL))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public REAL[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType5[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3", IncludeInSchema = false)]
    public enum ItemsChoiceType5
    {

        /// <remarks/>
        center,

        /// <remarks/>
        high,

        /// <remarks/>
        low,

        /// <remarks/>
        width,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class UVP_TS : TS
    {

        private double probabilityField;

        private bool probabilityFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double probability
        {
            get
            {
                return this.probabilityField;
            }
            set
            {
                this.probabilityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool probabilitySpecified
        {
            get
            {
                return this.probabilityFieldSpecified;
            }
            set
            {
                this.probabilityFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVXB_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SXCM_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_PPD_TS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class PPD_TS : TS
    {

        private PQ standardDeviationField;

        private ProbabilityDistributionType distributionTypeField;

        private bool distributionTypeFieldSpecified;

        /// <remarks/>
        public PQ standardDeviation
        {
            get
            {
                return this.standardDeviationField;
            }
            set
            {
                this.standardDeviationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ProbabilityDistributionType distributionType
        {
            get
            {
                return this.distributionTypeField;
            }
            set
            {
                this.distributionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool distributionTypeSpecified
        {
            get
            {
                return this.distributionTypeFieldSpecified;
            }
            set
            {
                this.distributionTypeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVXB_PPD_TS : PPD_TS
    {

        private bool inclusiveField;

        public IVXB_PPD_TS()
        {
            this.inclusiveField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EIVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IVL_PPD_TS))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PIVL_PPD_TS))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_PPD_TS : PPD_TS
    {

        private SetOperator operatorField;

        public SXCM_PPD_TS()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class EIVL_PPD_TS : SXCM_PPD_TS
    {

        private EIVLevent eventField;

        private IVL_PPD_PQ offsetField;

        /// <remarks/>
        public EIVLevent @event
        {
            get
            {
                return this.eventField;
            }
            set
            {
                this.eventField = value;
            }
        }

        /// <remarks/>
        public IVL_PPD_PQ offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "EIVL.event", Namespace = "urn:hl7-org:v3")]
    public partial class EIVLevent : CE
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class IVL_PPD_TS : SXCM_PPD_TS
    {

        private QTY[] itemsField;

        private ItemsChoiceType3[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("center", typeof(PPD_TS))]
        [System.Xml.Serialization.XmlElementAttribute("high", typeof(IVXB_PPD_TS))]
        [System.Xml.Serialization.XmlElementAttribute("low", typeof(IVXB_PPD_TS))]
        [System.Xml.Serialization.XmlElementAttribute("width", typeof(PPD_PQ))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public QTY[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType3[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3", IncludeInSchema = false)]
    public enum ItemsChoiceType3
    {

        /// <remarks/>
        center,

        /// <remarks/>
        high,

        /// <remarks/>
        low,

        /// <remarks/>
        width,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class PIVL_PPD_TS : SXCM_PPD_TS
    {

        private IVL_PPD_TS phaseField;

        private PPD_PQ periodField;

        private string alignmentField;

        private bool institutionSpecified1Field;

        public PIVL_PPD_TS()
        {
            this.institutionSpecified1Field = false;
        }

        /// <remarks/>
        public IVL_PPD_TS phase
        {
            get
            {
                return this.phaseField;
            }
            set
            {
                this.phaseField = value;
            }
        }

        /// <remarks/>
        public PPD_PQ period
        {
            get
            {
                return this.periodField;
            }
            set
            {
                this.periodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string alignment
        {
            get
            {
                return this.alignmentField;
            }
            set
            {
                this.alignmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("institutionSpecified")]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool institutionSpecified1
        {
            get
            {
                return this.institutionSpecified1Field;
            }
            set
            {
                this.institutionSpecified1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXPR_TS : SXCM_TS
    {

        private List<SXCM_TS> compField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("comp")]
        public List<SXCM_TS> comp
        {
            get
            {
                return this.compField;
            }
            set
            {
                this.compField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class EIVL_TS : SXCM_TS
    {

        private EIVLevent eventField;

        private IVL_PQ offsetField;

        /// <remarks/>
        public EIVLevent @event
        {
            get
            {
                return this.eventField;
            }
            set
            {
                this.eventField = value;
            }
        }

        /// <remarks/>
        public IVL_PQ offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class PIVL_TS : SXCM_TS
    {

        private IVL_TS phaseField;

        private PQ periodField;

        private string alignmentField;

        private bool institutionSpecified1Field;

        public PIVL_TS()
        {
            this.institutionSpecified1Field = false;
        }

        /// <remarks/>
        public IVL_TS phase
        {
            get
            {
                return this.phaseField;
            }
            set
            {
                this.phaseField = value;
            }
        }

        /// <remarks/>
        public PQ period
        {
            get
            {
                return this.periodField;
            }
            set
            {
                this.periodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string alignment
        {
            get
            {
                return this.alignmentField;
            }
            set
            {
                this.alignmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("institutionSpecified")]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool institutionSpecified1
        {
            get
            {
                return this.institutionSpecified1Field;
            }
            set
            {
                this.institutionSpecified1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class thumbnail : ED
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum CompressionAlgorithm
    {

        /// <remarks/>
        DF,

        /// <remarks/>
        GZ,

        /// <remarks/>
        Z,

        /// <remarks/>
        ZL,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public enum IntegrityCheckAlgorithm
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SHA-1")]
        SHA1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SHA-256")]
        SHA256,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class BXIT_CD : CD
    {

        private string qtyField;

        public BXIT_CD()
        {
            this.qtyField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string qty
        {
            get
            {
                return this.qtyField;
            }
            set
            {
                this.qtyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class SXCM_CD : CD
    {

        private SetOperator operatorField;

        public SXCM_CD()
        {
            this.operatorField = SetOperator.I;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SetOperator.I)]
        public SetOperator @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class HXIT_CE : CE
    {

        private IVL_TS validTimeField;

        /// <remarks/>
        public IVL_TS validTime
        {
            get
            {
                return this.validTimeField;
            }
            set
            {
                this.validTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:hl7-org:v3")]
    public partial class CO : CV
    {
    }

}
