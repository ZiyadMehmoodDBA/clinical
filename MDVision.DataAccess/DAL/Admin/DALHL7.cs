using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALHL7
    {
        

        #region "Stored Procedure Names"

        private const string PROC_Create_HL7_Message_SIU = "Provider.CreateHL7Message_SIU";
        private const string PROC_Select_HL7_Message_ACK = "dbo.SelectHL7ErrorAcknowledgement";
        private const string PROC_Insert_HL7_Message_ACK = "dbo.InsertHL7ErrorAcknowledgement";
        private const string ProcInsertHl7PatientMessage = "dbo.HL7PatientMessageLog";

        #endregion

        #region "Parameters"
        private const string PARM_PATIENT_ID = "@PatientID";
        private const string PARM_APPOINTMENT_ID = "@AppointmentId";

        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";


        #region HL7 Patient Message Paramters


        private const string PARM_FileName = "@FileName";
        private const string PARM_FileContent = "@FileContent";
        private const string PARM_MSH_SegmentTypeID = "@MSH_SegmentTypeID";
        private const string PARM_MSH_EncodingCharacters = "@MSH_EncodingCharacters";
        private const string PARM_MSH_SendingApplication = "@MSH_SendingApplication";
        private const string PARM_MSH_SendingFacility = "@MSH_SendingFacility";
        private const string PARM_MSH_ReceivingApplication = "@MSH_ReceivingApplication";
        private const string PARM_MSH_ReceivingFacility = "@MSH_ReceivingFacility";
        private const string PARM_MSH_MessageDateTime = "@MSH_MessageDateTime";
        private const string PARM_MSH_Security = "@MSH_Security";
        private const string PARM_MSH_MessageType = "@MSH_MessageType";
        private const string PARM_MSH_MessageControlID = "@MSH_MessageControlID";
        private const string PARM_MSH_ProcessingID = "@MSH_ProcessingID";
        private const string PARM_MSH_VersionID = "@MSH_VersionID";
        private const string PARM_EVN_SegmentTypeId = "@EVN_SegmentTypeId";
        private const string PARM_EVN_EventTypeCode = "@EVN_EventTypeCode";
        private const string PARM_EVN_RecordedDateTime = "@EVN_RecordedDateTime";
        private const string PARM_EVN_EventReasonCode = "@EVN_EventReasonCode";
        private const string PARM_PID_SegmentTypeId = "@PID_SegmentTypeId";
        private const string PARM_PID_SequenceNumber = "@PID_SequenceNumber";
        private const string PARM_PID_PatientIdeCwmrn = "@PID_PatientIdeCwmrn";
        private const string PARM_PID_ExternalPatientIdVendorMrn = "@PID_ExternalPatientIdVendorMrn";
        private const string PARM_PID_Pid04 = "@PID_Pid04";
        private const string PARM_PID_PatientName = "@PID_PatientName";
        private const string PARM_PID_LastName = "@PID_LastName";
        private const string PARM_PID_FirstName = "@PID_FirstName";
        private const string PARM_PID_MiddleInitial = "@PID_MiddleInitial";
        private const string PARM_PID_MothersMaidenName = "@PID_MothersMaidenName";
        private const string PARM_PID_DateOfBirth = "@PID_DateOfBirth";
        private const string PARM_PID_Sex = "@PID_Sex";
        private const string PARM_PID_Pid09 = "@PID_Pid09";
        private const string PARM_PID_Race = "@PID_Race";
        private const string PARM_PID_PatientAddress = "@PID_PatientAddress";
        private const string PARM_PID_Addr1 = "@PID_Addr1";
        private const string PARM_PID_Addr2 = "@PID_Addr2";
        private const string PARM_PID_City = "@PID_City";
        private const string PARM_PID_State = "@PID_State";
        private const string PARM_PID_Zip = "@PID_Zip";
        private const string PARM_PID_CountryCode = "@PID_CountryCode";
        private const string PARM_PID_Phone = "@PID_Phone";
        private const string PARM_PID_PhoneHome = "@PID_PhoneHome";
        private const string PARM_PID_PhoneEmail = "@PID_PhoneEmail";
        private const string PARM_PID_PhoneBusiness = "@PID_PhoneBusiness";
        private const string PARM_PID_PhoneCell = "@PID_PhoneCell";
        private const string PARM_PID_PrimaryLanguage = "@PID_PrimaryLanguage";
        private const string PARM_PID_MaritalStatus = "@PID_MaritalStatus";
        private const string PARM_PID_Religion = "@PID_Religion";
        private const string PARM_PID_PatientAccountNumber = "@PID_PatientAccountNumber";
        private const string PARM_PID_Ssn = "@PID_Ssn";
        private const string PARM_PID_Pid20 = "@PID_Pid20";
        private const string PARM_PID_Pid21 = "@PID_Pid21";
        private const string PARM_PID_Ethnicity = "@PID_Ethnicity";
        private const string PARM_PID_DefaultLocation = "@PID_DefaultLocation";
        private const string PARM_PID_StatementFlag = "@PID_StatementFlag";
        private const string PARM_PID_StatementSignatureDate = "@PID_StatementSignatureDate";
        private const string PARM_PID_PatientPreviousName = "@PID_PatientPreviousName";
        private const string PARM_PV1_SegmentTypeId = "@PV1_SegmentTypeId";
        private const string PARM_PV1_SequenceNumber = "@PV1_SequenceNumber";
        private const string PARM_PV1_PatientClass = "@PV1_PatientClass";
        private const string PARM_PV1_AssignedPatientLocation = "@PV1_AssignedPatientLocation";
        private const string PARM_PV1_LocationCode = "@PV1_LocationCode";
        private const string PARM_PV1_LocationDescription = "@PV1_LocationDescription";
        private const string PARM_PV1_AdmissionType = "@PV1_AdmissionType";
        private const string PARM_PV1_PreAdmitNumber = "@PV1_PreAdmitNumber";
        private const string PARM_PV1_PriorPatientLocation = "@PV1_PriorPatientLocation";
        private const string PARM_PV1_AttendingPrimaryCareDoctor = "@PV1_AttendingPrimaryCareDoctor";
        private const string PARM_PV1_ProviderCode = "@PV1_ProviderCode";
        private const string PARM_PV1_ProviderLastName = "@PV1_ProviderLastName";
        private const string PARM_PV1_ProviderFirstName = "@PV1_ProviderFirstName";
        private const string PARM_PV1_ProviderMiddleInitial = "@PV1_ProviderMiddleInitial";
        private const string PARM_PV1_ProviderPrefix = "@PV1_ProviderPrefix";
        private const string PARM_PV1_ProviderSuffix = "@PV1_ProviderSuffix";
        private const string PARM_PV1_ReferringProvider = "@PV1_ReferringProvider";
        private const string PARM_PV1_ReferringProviderCode = "@PV1_ReferringProviderCode";
        private const string PARM_PV1_ReferringProviderLastName = "@PV1_ReferringProviderLastName";
        private const string PARM_PV1_ReferringProviderFirstName = "@PV1_ReferringProviderFirstName";
        private const string PARM_PV1_ReferringProviderMiddleInitial = "@PV1_ReferringProviderMiddleInitial";
        private const string PARM_PV1_ReferringProviderPrefix = "@PV1_ReferringProviderPrefix";
        private const string PARM_PV1_ReferringProviderSuffix = "@PV1_ReferringProviderSuffix";
        private const string PARM_PV1_RenderingProvider = "@PV1_RenderingProvider";
        private const string PARM_PV1_RenderingProviderCode = "@PV1_RenderingProviderCode";
        private const string PARM_PV1_RenderingProviderLastName = "@PV1_RenderingProviderLastName";
        private const string PARM_PV1_RenderingProviderFirstName = "@PV1_RenderingProviderFirstName";
        private const string PARM_PV1_RenderingProviderMiddleInitial = "@PV1_RenderingProviderMiddleInitial";
        private const string PARM_PV1_RenderingProviderPrefix = "@PV1_RenderingProviderPrefix";
        private const string PARM_PV1_RenderingProviderSuffix = "@PV1_RenderingProviderSuffix";
        private const string PARM_PV1_pv1_10 = "@PV1_pv1_10";
        private const string PARM_PV1_pv1_11 = "@PV1_pv1_11";
        private const string PARM_PV1_pv1_12 = "@PV1_pv1_12";
        private const string PARM_PV1_pv1_13 = "@PV1_pv1_13";
        private const string PARM_PV1_pv1_14 = "@PV1_pv1_14";
        private const string PARM_PV1_pv1_15 = "@PV1_pv1_15";
        private const string PARM_PV1_pv1_16 = "@PV1_pv1_16";
        private const string PARM_PV1_pv1_17 = "@PV1_pv1_17";
        private const string PARM_PV1_pv1_18 = "@PV1_pv1_18";
        private const string PARM_PV1_VisitId = "@PV1_VisitId";
        private const string PARM_PV1_pv1_20 = "@PV1_pv1_20";
        private const string PARM_PV1_pv1_21 = "@PV1_pv1_21";
        private const string PARM_PV1_pv1_22 = "@PV1_pv1_22";
        private const string PARM_PV1_pv1_23 = "@PV1_pv1_23";
        private const string PARM_PV1_pv1_24 = "@PV1_pv1_24";
        private const string PARM_PV1_pv1_25 = "@PV1_pv1_25";
        private const string PARM_PV1_pv1_26 = "@PV1_pv1_26";
        private const string PARM_PV1_pv1_27 = "@PV1_pv1_27";
        private const string PARM_PV1_pv1_28 = "@PV1_pv1_28";
        private const string PARM_PV1_pv1_29 = "@PV1_pv1_29";
        private const string PARM_PV1_pv1_30 = "@PV1_pv1_30";
        private const string PARM_PV1_pv1_31 = "@PV1_pv1_31";
        private const string PARM_PV1_pv1_32 = "@PV1_pv1_32";
        private const string PARM_PV1_pv1_33 = "@PV1_pv1_33";
        private const string PARM_PV1_pv1_34 = "@PV1_pv1_34";
        private const string PARM_PV1_pv1_35 = "@PV1_pv1_35";
        private const string PARM_PV1_pv1_36 = "@PV1_pv1_36";
        private const string PARM_PV1_pv1_37 = "@PV1_pv1_37";
        private const string PARM_PV1_pv1_38 = "@PV1_pv1_38";
        private const string PARM_PV1_pv1_39 = "@PV1_pv1_39";
        private const string PARM_PV1_pv1_40 = "@PV1_pv1_40";
        private const string PARM_PV1_pv1_41 = "@PV1_pv1_41";
        private const string PARM_PV1_pv1_42 = "@PV1_pv1_42";
        private const string PARM_PV1_pv1_43 = "@PV1_pv1_43";
        private const string PARM_PV1_AdmitDateTime = "@PV1_AdmitDateTime";
        private const string PARM_PV1_pv1_45 = "@PV1_pv1_45";
        private const string PARM_PV1_pv1_46 = "@PV1_pv1_46";
        private const string PARM_PV1_pv1_47 = "@PV1_pv1_47";
        private const string PARM_PV1_pv1_48 = "@PV1_pv1_48";
        private const string PARM_PV1_pv1_49 = "@PV1_pv1_49";
        private const string PARM_PV1_AlternateVisitId = "@PV1_AlternateVisitId";
        private const string PARM_PD1_SegmentTypeID = "@PD1_SegmentTypeID";
        private const string PARM_PD1_SequenceNumber = "@PD1_SequenceNumber";
        private const string PARM_PD1_Pd1_02 = "@PD1_Pd1_02";
        private const string PARM_PD1_Pd1_03 = "@PD1_Pd1_03";
        private const string PARM_PD1_PrimaryCareProviderDoctorCode = "@PD1_PrimaryCareProviderDoctorCode";
        private const string PARM_PD1_ProviderCode = "@PD1_ProviderCode";
        private const string PARM_PD1_ProviderLastName = "@PD1_ProviderLastName";
        private const string PARM_PD1_ProviderFirstName = "@PD1_ProviderFirstName";
        private const string PARM_PD1_ProviderMiddleInitial = "@PD1_ProviderMiddleInitial";
        private const string PARM_PD1_ProviderPrefix = "@PD1_ProviderPrefix";
        private const string PARM_PD1_ProviderSuffix = "@PD1_ProviderSuffix";
        private const string PARM_PD1_Pd1_05 = "@PD1_Pd1_05";
        private const string PARM_PD1_ProviderFullName = "@PD1_ProviderFullName";
        private const string PARM_GT1_SegmentTypeId = "@GT1_SegmentTypeId";
        private const string PARM_GT1_SequenceNumber = "@GT1_SequenceNumber";
        private const string PARM_GT1_GuarantorNumber = "@GT1_GuarantorNumber";
        private const string PARM_GT1_GuarantorName = "@GT1_GuarantorName";
        private const string PARM_GT1_LastName = "@GT1_LastName";
        private const string PARM_GT1_FirstName = "@GT1_FirstName";
        private const string PARM_GT1_MiddleInitial = "@GT1_MiddleInitial";
        private const string PARM_GT1_GuarantorSpouseName = "@GT1_GuarantorSpouseName";
        private const string PARM_GT1_GuarantorAddress = "@GT1_GuarantorAddress";
        private const string PARM_GT1_Add1 = "@GT1_Add1";
        private const string PARM_GT1_Add2 = "@GT1_Add2";
        private const string PARM_GT1_City = "@GT1_City";
        private const string PARM_GT1_State = "@GT1_State";
        private const string PARM_GT1_Zip = "@GT1_Zip";
        private const string PARM_GT1_GuarantorPhoneNumHome = "@GT1_GuarantorPhoneNumHome";
        private const string PARM_GT1_GuarantorPhoneNumBusiness = "@GT1_GuarantorPhoneNumBusiness";
        private const string PARM_GT1_GuarantorDateofBirth = "@GT1_GuarantorDateofBirth";
        private const string PARM_GT1_GuarantorSex = "@GT1_GuarantorSex";
        private const string PARM_GT1_GuarantorType = "@GT1_GuarantorType";
        private const string PARM_GT1_GuarantorRelationship = "@GT1_GuarantorRelationship";
        private const string PARM_GT1_GuarantorSsn = "@GT1_GuarantorSsn";
        private const string PARM_GT1_GuarantorDateBegin = "@GT1_GuarantorDateBegin";
        private const string PARM_GT1_GuarantorDateEnd = "@GT1_GuarantorDateEnd";
        private const string PARM_GT1_GuarantorPriority = "@GT1_GuarantorPriority";
        private const string PARM_NK1_SegmentTypeID = "@NK1_SegmentTypeID";
        private const string PARM_NK1_SequenceNumber = "@NK1_SequenceNumber";
        private const string PARM_NK1_NextofKinName = "@NK1_NextofKinName";
        private const string PARM_NK1_NextofKinLastName = "@NK1_NextofKinLastName";
        private const string PARM_NK1_NextofKinFirstName = "@NK1_NextofKinFirstName";
        private const string PARM_NK1_Relationship = "@NK1_Relationship";
        private const string PARM_NK1_Address = "@NK1_Address";
        private const string PARM_NK1_Addr1 = "@NK1_Addr1";
        private const string PARM_NK1_Addr2 = "@NK1_Addr2";
        private const string PARM_NK1_City = "@NK1_City";
        private const string PARM_NK1_State = "@NK1_State";
        private const string PARM_NK1_Zip = "@NK1_Zip";
        private const string PARM_NK1_PhoneNumber = "@NK1_PhoneNumber";
        private const string PARM_IN1_SegmentTypeID = "@IN1_SegmentTypeID";
        private const string PARM_IN1_SequenceNumber = "@IN1_SequenceNumber";
        private const string PARM_IN1_InsurancePlanID = "@IN1_InsurancePlanID";
        private const string PARM_IN1_InsuranceCompanyID = "@IN1_InsuranceCompanyID";
        private const string PARM_IN1_InsuranceCompanyName = "@IN1_InsuranceCompanyName";
        private const string PARM_IN1_InsuranceCompanyAddress = "@IN1_InsuranceCompanyAddress";
        private const string PARM_IN1_InsuranceCompanyAddr1 = "@IN1_InsuranceCompanyAddr1";
        private const string PARM_IN1_InsuranceCompanyAddr2 = "@IN1_InsuranceCompanyAddr2";
        private const string PARM_IN1_InsuranceCompanyCity = "@IN1_InsuranceCompanyCity";
        private const string PARM_IN1_InsuranceCompanyState = "@IN1_InsuranceCompanyState";
        private const string PARM_IN1_InsuranceCompanyZip = "@IN1_InsuranceCompanyZip";
        private const string PARM_IN1_InsuranceCoContactPerson = "@IN1_InsuranceCoContactPerson";
        private const string PARM_IN1_InsuranceCoPhoneNumber = "@IN1_InsuranceCoPhoneNumber";
        private const string PARM_IN1_GroupNumber = "@IN1_GroupNumber";
        private const string PARM_IN1_GroupName = "@IN1_GroupName";
        private const string PARM_IN1_InsuredssGroupEmpID = "@IN1_InsuredssGroupEmpID";
        private const string PARM_IN1_InsuredsGroupEmpName = "@IN1_InsuredsGroupEmpName";
        private const string PARM_IN1_PlanEffectiveStartDate = "@IN1_PlanEffectiveStartDate";
        private const string PARM_IN1_PlanExpirationEndDate = "@IN1_PlanExpirationEndDate";
        private const string PARM_IN1_AuthorizationInformation = "@IN1_AuthorizationInformation";
        private const string PARM_IN1_PlanType = "@IN1_PlanType";
        private const string PARM_IN1_NameofInsured = "@IN1_NameofInsured";
        private const string PARM_IN1_NameofInsuredLast = "@IN1_NameofInsuredLast";
        private const string PARM_IN1_NameofInsuredFirst = "@IN1_NameofInsuredFirst";
        private const string PARM_IN1_NameofInsuredMI = "@IN1_NameofInsuredMI";
        private const string PARM_IN1_InsuredsRelationshiptoPatient = "@IN1_InsuredsRelationshiptoPatient";
        private const string PARM_IN1_InsuredsDateofBirth = "@IN1_InsuredsDateofBirth";
        private const string PARM_IN1_InsuredsAddress = "@IN1_InsuredsAddress";
        private const string PARM_IN1_InsuredsAddressAddr1 = "@IN1_InsuredsAddressAddr1";
        private const string PARM_IN1_InsuredsAddressAddr2 = "@IN1_InsuredsAddressAddr2";
        private const string PARM_IN1_InsuredsAddressCity = "@IN1_InsuredsAddressCity";
        private const string PARM_IN1_InsuredsAddressState = "@IN1_InsuredsAddressState";
        private const string PARM_IN1_InsuredsAddressZip = "@IN1_InsuredsAddressZip";
        private const string PARM_IN1_AssignmentofBenefits = "@IN1_AssignmentofBenefits";
        private const string PARM_IN1_CoordinationofBenefits = "@IN1_CoordinationofBenefits";
        private const string PARM_IN1_IN1_22 = "@IN1_IN1_22";
        private const string PARM_IN1_IN1_23 = "@IN1_IN1_23";
        private const string PARM_IN1_IN1_24 = "@IN1_IN1_24";
        private const string PARM_IN1_IN1_25 = "@IN1_IN1_25";
        private const string PARM_IN1_IN1_26 = "@IN1_IN1_26";
        private const string PARM_IN1_IN1_27 = "@IN1_IN1_27";
        private const string PARM_IN1_IN1_28 = "@IN1_IN1_28";
        private const string PARM_IN1_IN1_29 = "@IN1_IN1_29";
        private const string PARM_IN1_IN1_30 = "@IN1_IN1_30";
        private const string PARM_IN1_IN1_31 = "@IN1_IN1_31";
        private const string PARM_IN1_IN1_32 = "@IN1_IN1_32";
        private const string PARM_IN1_IN1_33 = "@IN1_IN1_33";
        private const string PARM_IN1_IN1_34 = "@IN1_IN1_34";
        private const string PARM_IN1_IN1_35 = "@IN1_IN1_35";
        private const string PARM_IN1_PolicyNumber = "@IN1_PolicyNumber";
        private const string PARM_SCH_SegmentTypeID = "@SCH_SegmentTypeID";
        private const string PARM_SCH_TempAppointmentID = "@SCH_TempAppointmentID";
        private const string PARM_SCH_AppointmentID = "@SCH_AppointmentID";
        private const string PARM_SCH_OccurrenceNumber = "@SCH_OccurrenceNumber";
        private const string PARM_SCH_PlacerGroupNumber = "@SCH_PlacerGroupNumber";
        private const string PARM_SCH_ScheduleID = "@SCH_ScheduleID";
        private const string PARM_SCH_EventReason = "@SCH_EventReason";
        private const string PARM_SCH_AppointmentReason = "@SCH_AppointmentReason";
        private const string PARM_SCH_AppointmentType = "@SCH_AppointmentType";
        private const string PARM_SCH_AppointmentDuration = "@SCH_AppointmentDuration";
        private const string PARM_SCH_AppointmentDurationUnit = "@SCH_AppointmentDurationUnit";
        private const string PARM_SCH_AppointmentTime = "@SCH_AppointmentTime";
        private const string PARM_SCH_AppointmentDuration_ = "@SCH_AppointmentDuration_";
        private const string PARM_SCH_AppointmentStartTime = "@SCH_AppointmentStartTime";
        private const string PARM_SCH_AppointmentEndTime = "@SCH_AppointmentEndTime";
        private const string PARM_SCH_SCH_12 = "@SCH_SCH_12";
        private const string PARM_SCH_SCH_13 = "@SCH_SCH_13";
        private const string PARM_SCH_SCH_14 = "@SCH_SCH_14";
        private const string PARM_SCH_SCH_15 = "@SCH_SCH_15";
        private const string PARM_SCH_SCH_16 = "@SCH_SCH_16";
        private const string PARM_SCH_SCH_17 = "@SCH_SCH_17";
        private const string PARM_SCH_SCH_18 = "@SCH_SCH_18";
        private const string PARM_SCH_SCH_19 = "@SCH_SCH_19";
        private const string PARM_SCH_ResourceName = "@SCH_ResourceName";
        private const string PARM_SCH_SCH_21 = "@SCH_SCH_21";
        private const string PARM_SCH_SCH_22 = "@SCH_SCH_22";
        private const string PARM_SCH_AppointmentBillingComments = "@SCH_AppointmentBillingComments";
        private const string PARM_SCH_AppointmentEncounterComments = "@SCH_AppointmentEncounterComments";
        private const string PARM_SCH_VisitStatus = "@SCH_VisitStatus";
        private const string PARM_AIG_SegmentTypeID = "@AIG_SegmentTypeID";
        private const string PARM_AIG_SequenceNumber = "@AIG_SequenceNumber";
        private const string PARM_AIG_AIG_02 = "@AIG_AIG_02";
        private const string PARM_AIG_Resource = "@AIG_Resource";
        private const string PARM_AIG_ResourceID = "@AIG_ResourceID";
        private const string PARM_AIG_ResourceLastName = "@AIG_ResourceLastName";
        private const string PARM_AIG_ResourceFirstName = "@AIG_ResourceFirstName";
        private const string PARM_AIG_ResourceTypeIdentifier = "@AIG_ResourceTypeIdentifier";
        private const string PARM_AIG_AIG_05 = "@AIG_AIG_05";
        private const string PARM_AIG_AIG_06 = "@AIG_AIG_06";
        private const string PARM_AIG_AIG_07 = "@AIG_AIG_07";
        private const string PARM_AIG_AppointmentStartDateTime = "@AIG_AppointmentStartDateTime";
        private const string PARM_AIG_AIG_09 = "@AIG_AIG_09";
        private const string PARM_AIG_AIG_10 = "@AIG_AIG_10";
        private const string PARM_AIG_Duration = "@AIG_Duration";
        private const string PARM_AIG_DurationUnits = "@AIG_DurationUnits";
        private const string PARM_AIP_SegmentTypeID = "@AIP_SegmentTypeID";
        private const string PARM_AIP_SequenceNumber = "@AIP_SequenceNumber";
        private const string PARM_AIP_SegmentActionCode = "@AIP_SegmentActionCode";
        private const string PARM_AIP_OrderingProvider = "@AIP_OrderingProvider";
        private const string PARM_AIP_OrderingProviderID = "@AIP_OrderingProviderID";
        private const string PARM_AIP_OrderingProviderLastName = "@AIP_OrderingProviderLastName";
        private const string PARM_AIP_OrderingProviderFirstName = "@AIP_OrderingProviderFirstName";
        private const string PARM_AIP_ResourceRole = "@AIP_ResourceRole";
        private const string PARM_AIP_AIP_05 = "@AIP_AIP_05";
        private const string PARM_AIP_AppointmentStartDateTime = "@AIP_AppointmentStartDateTime";
        private const string PARM_AIP_AIP_07 = "@AIP_AIP_07";
        private const string PARM_AIP_AIP_08 = "@AIP_AIP_08";
        private const string PARM_AIP_Duration = "@AIP_Duration";
        private const string PARM_AIP_DurationUnits = "@AIP_DurationUnits";
        private const string PARM_AIP_AIP_11 = "@AIP_AIP_11";
        private const string PARM_AIP_FillerStatusCode = "@AIP_FillerStatusCode";
        private const string PARM_AIL_SegmentTypeID = "@AIL_SegmentTypeID";
        private const string PARM_AIL_SequenceNumber = "@AIL_SequenceNumber";
        private const string PARM_AIL_SegmentActionCode = "@AIL_SegmentActionCode";
        private const string PARM_AIL_LocationResource = "@AIL_LocationResource";
        private const string PARM_AIL_LocationResourceID = "@AIL_LocationResourceID";
        private const string PARM_AIL_LocationResourceName = "@AIL_LocationResourceName";

        #endregion


        #endregion

        #region Constructors
        public DALHL7()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region "Get using dataset Functions"
        /// <summary>
        /// Load_Patient_Provider_Appointment_Visit
        /// </summary>
        /// <param name="patientID"></param>
        /// <returns></returns>
        public DSHL7 Load_Patient_Provider_Appointment_Visit(long patientID, long appointmentID)
        {
            DSHL7 ds = new DSHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(2);

                if (patientID <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientID);

                if (appointmentID <= 0)
                    dbManager.AddParameters(1, PARM_APPOINTMENT_ID, null);
                else
                    dbManager.AddParameters(1, PARM_APPOINTMENT_ID, appointmentID);

                ds = (DSHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Create_HL7_Message_SIU, ds, ds.CreateSIU.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHL7::Load_Patient_Provider_Appointment_Visit", PROC_Create_HL7_Message_SIU, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSHL7 Load_HL7_Error_ACK(long patientID)
        {
            DSHL7 ds = new DSHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (patientID <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, patientID);

                ds = (DSHL7)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_Select_HL7_Message_ACK, ds, ds.HL7ErrorAcknowledgement.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHL7::LoadErrorLog", PROC_Select_HL7_Message_ACK, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        public DSHL7 Insert_HL7_Error_ACK(string MessageType, string Segment, string ErrorType, string ErrorMessage, long PatientID)
        {
            DSHL7 ds = new DSHL7();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);

                if (PatientID <= 0)
                    dbManager.AddParameters(0, PARM_PATIENT_ID, null);
                else
                    dbManager.AddParameters(0, PARM_PATIENT_ID, PatientID);

                ds = (DSHL7)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_Insert_HL7_Message_ACK, ds, ds.HL7ErrorAcknowledgement.TableName);

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALHL7::LoadErrorLog", PROC_Insert_HL7_Message_ACK, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        #endregion

        public DSHL7 InsertHl7PatientMessage(DSHL7 ds)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                CreateParameters(dbManager, ds, true);
                ds = (DSHL7)dbManager.InsertDataSet(CommandType.StoredProcedure, ProcInsertHl7PatientMessage, ds, ds.CreateHL7PatientMessageLog.TableName);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALSection::InsertSection", ProcInsertHl7PatientMessage, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }


        private void CreateParameters(IDBManager dbManager, DSHL7 ds, Boolean IsInsert)
        {
            dbManager.CreateParameters(294);

            int index = 0;
 //dbManager.AddParameters([index++], PARM_SECTION_TYPE_ID, ds.CreateHL7PatientMessageLog. .ColumnName , DbType.String);
        //    dbManager.AddParameters([index++],  PARM_AIL_LocationResourceName     			         , ds.CreateHL7PatientMessageLog.LocationResour

           dbManager.AddParameters(index++,PARM_FileName,ds.CreateHL7PatientMessageLog.FileNameColumn.ColumnName  , DbType.String); 
           dbManager.AddParameters(index++,PARM_FileContent,ds.CreateHL7PatientMessageLog.FileContentColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_SegmentTypeID,ds.CreateHL7PatientMessageLog.MSH_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_EncodingCharacters,ds.CreateHL7PatientMessageLog.MSH_EncodingCharactersColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_SendingApplication,ds.CreateHL7PatientMessageLog.MSH_SendingApplicationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_SendingFacility,ds.CreateHL7PatientMessageLog.MSH_SendingFacilityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_ReceivingApplication,ds.CreateHL7PatientMessageLog.MSH_ReceivingApplicationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_ReceivingFacility,ds.CreateHL7PatientMessageLog.MSH_ReceivingFacilityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_MessageDateTime,ds.CreateHL7PatientMessageLog.MSH_MessageDateTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_Security,ds.CreateHL7PatientMessageLog.MSH_SecurityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_MessageType,ds.CreateHL7PatientMessageLog.MSH_MessageTypeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_MessageControlID,ds.CreateHL7PatientMessageLog.MSH_MessageControlIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_ProcessingID,ds.CreateHL7PatientMessageLog.MSH_ProcessingIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_MSH_VersionID,ds.CreateHL7PatientMessageLog.MSH_VersionIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_EVN_SegmentTypeId,ds.CreateHL7PatientMessageLog.EVN_SegmentTypeIdColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_EVN_EventTypeCode,ds.CreateHL7PatientMessageLog.EVN_EventTypeCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_EVN_RecordedDateTime,ds.CreateHL7PatientMessageLog.EVN_RecordedDateTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_EVN_EventReasonCode,ds.CreateHL7PatientMessageLog.EVN_EventReasonCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_SegmentTypeId,ds.CreateHL7PatientMessageLog.PID_SegmentTypeIdColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_SequenceNumber,ds.CreateHL7PatientMessageLog.PID_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PatientIdeCwmrn,ds.CreateHL7PatientMessageLog.PID_PatientIdeCwmrnColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_ExternalPatientIdVendorMrn,ds.CreateHL7PatientMessageLog.PID_ExternalPatientIdVendorMrnColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Pid04,ds.CreateHL7PatientMessageLog.PID_Pid04Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PatientName,ds.CreateHL7PatientMessageLog.PID_PatientNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_LastName,ds.CreateHL7PatientMessageLog.PID_LastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_FirstName,ds.CreateHL7PatientMessageLog.PID_FirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_MiddleInitial,ds.CreateHL7PatientMessageLog.PID_MiddleInitialColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_MothersMaidenName,ds.CreateHL7PatientMessageLog.PID_MothersMaidenNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_DateOfBirth,ds.CreateHL7PatientMessageLog.PID_DateOfBirthColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Sex,ds.CreateHL7PatientMessageLog.PID_SexColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Pid09,ds.CreateHL7PatientMessageLog.PID_Pid09Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Race,ds.CreateHL7PatientMessageLog.PID_RaceColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PatientAddress,ds.CreateHL7PatientMessageLog.PID_PatientAddressColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Addr1,ds.CreateHL7PatientMessageLog.PID_Addr1Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Addr2,ds.CreateHL7PatientMessageLog.PID_Addr2Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_City,ds.CreateHL7PatientMessageLog.PID_CityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_State,ds.CreateHL7PatientMessageLog.PID_StateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Zip,ds.CreateHL7PatientMessageLog.PID_ZipColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_CountryCode,ds.CreateHL7PatientMessageLog.PID_CountryCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Phone,ds.CreateHL7PatientMessageLog.PID_PhoneColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PhoneHome,ds.CreateHL7PatientMessageLog.PID_PhoneHomeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PhoneEmail,ds.CreateHL7PatientMessageLog.PID_PhoneEmailColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PhoneBusiness,ds.CreateHL7PatientMessageLog.PID_PhoneBusinessColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PhoneCell,ds.CreateHL7PatientMessageLog.PID_PhoneCellColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PrimaryLanguage,ds.CreateHL7PatientMessageLog.PID_PrimaryLanguageColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_MaritalStatus,ds.CreateHL7PatientMessageLog.PID_MaritalStatusColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Religion,ds.CreateHL7PatientMessageLog.PID_ReligionColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PatientAccountNumber,ds.CreateHL7PatientMessageLog.PID_PatientAccountNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Ssn,ds.CreateHL7PatientMessageLog.PID_SsnColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Pid20,ds.CreateHL7PatientMessageLog.PID_Pid20Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Pid21,ds.CreateHL7PatientMessageLog.PID_Pid21Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_Ethnicity,ds.CreateHL7PatientMessageLog.PID_EthnicityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_DefaultLocation,ds.CreateHL7PatientMessageLog.PID_DefaultLocationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_StatementFlag,ds.CreateHL7PatientMessageLog.PID_StatementFlagColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_StatementSignatureDate,ds.CreateHL7PatientMessageLog.PID_StatementSignatureDateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PID_PatientPreviousName,ds.CreateHL7PatientMessageLog.PID_PatientPreviousNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_SegmentTypeId,ds.CreateHL7PatientMessageLog.PV1_SegmentTypeIdColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_SequenceNumber,ds.CreateHL7PatientMessageLog.PV1_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_PatientClass,ds.CreateHL7PatientMessageLog.PV1_PatientClassColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_AssignedPatientLocation,ds.CreateHL7PatientMessageLog.PV1_AssignedPatientLocationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_LocationCode,ds.CreateHL7PatientMessageLog.PV1_LocationCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_LocationDescription,ds.CreateHL7PatientMessageLog.PV1_LocationDescriptionColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_AdmissionType,ds.CreateHL7PatientMessageLog.PV1_AdmissionTypeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_PreAdmitNumber,ds.CreateHL7PatientMessageLog.PV1_PreAdmitNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_PriorPatientLocation,ds.CreateHL7PatientMessageLog.PV1_PriorPatientLocationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_AttendingPrimaryCareDoctor,ds.CreateHL7PatientMessageLog.PV1_AttendingPrimaryCareDoctorColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ProviderCode,ds.CreateHL7PatientMessageLog.PV1_ProviderCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ProviderLastName,ds.CreateHL7PatientMessageLog.PV1_ProviderLastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ProviderFirstName,ds.CreateHL7PatientMessageLog.PV1_ProviderFirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ProviderMiddleInitial,ds.CreateHL7PatientMessageLog.PV1_ProviderMiddleInitialColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ProviderPrefix,ds.CreateHL7PatientMessageLog.PV1_ProviderPrefixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ProviderSuffix,ds.CreateHL7PatientMessageLog.PV1_ProviderSuffixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ReferringProvider,ds.CreateHL7PatientMessageLog.PV1_ReferringProviderColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ReferringProviderCode,ds.CreateHL7PatientMessageLog.PV1_ReferringProviderCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ReferringProviderLastName,ds.CreateHL7PatientMessageLog.PV1_ReferringProviderLastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ReferringProviderFirstName,ds.CreateHL7PatientMessageLog.PV1_ReferringProviderFirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ReferringProviderMiddleInitial,ds.CreateHL7PatientMessageLog.PV1_ReferringProviderMiddleInitialColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ReferringProviderPrefix,ds.CreateHL7PatientMessageLog.PV1_ReferringProviderPrefixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_ReferringProviderSuffix,ds.CreateHL7PatientMessageLog.PV1_ReferringProviderSuffixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_RenderingProvider,ds.CreateHL7PatientMessageLog.PV1_RenderingProviderColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_RenderingProviderCode,ds.CreateHL7PatientMessageLog.PV1_RenderingProviderCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_RenderingProviderLastName,ds.CreateHL7PatientMessageLog.PV1_RenderingProviderLastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_RenderingProviderFirstName,ds.CreateHL7PatientMessageLog.PV1_RenderingProviderFirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_RenderingProviderMiddleInitial,ds.CreateHL7PatientMessageLog.PV1_RenderingProviderMiddleInitialColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_RenderingProviderPrefix,ds.CreateHL7PatientMessageLog.PV1_RenderingProviderPrefixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_RenderingProviderSuffix,ds.CreateHL7PatientMessageLog.PV1_RenderingProviderSuffixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_10,ds.CreateHL7PatientMessageLog.PV1_pv1_10Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_11,ds.CreateHL7PatientMessageLog.PV1_pv1_11Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_12,ds.CreateHL7PatientMessageLog.PV1_pv1_12Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_13,ds.CreateHL7PatientMessageLog.PV1_pv1_13Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_14,ds.CreateHL7PatientMessageLog.PV1_pv1_14Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_15,ds.CreateHL7PatientMessageLog.PV1_pv1_15Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_16,ds.CreateHL7PatientMessageLog.PV1_pv1_16Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_17,ds.CreateHL7PatientMessageLog.PV1_pv1_17Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_18,ds.CreateHL7PatientMessageLog.PV1_pv1_18Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_VisitId,ds.CreateHL7PatientMessageLog.PV1_VisitIdColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_20,ds.CreateHL7PatientMessageLog.PV1_pv1_20Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_21,ds.CreateHL7PatientMessageLog.PV1_pv1_21Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_22,ds.CreateHL7PatientMessageLog.PV1_pv1_22Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_23,ds.CreateHL7PatientMessageLog.PV1_pv1_23Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_24,ds.CreateHL7PatientMessageLog.PV1_pv1_24Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_25,ds.CreateHL7PatientMessageLog.PV1_pv1_25Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_26,ds.CreateHL7PatientMessageLog.PV1_pv1_26Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_27,ds.CreateHL7PatientMessageLog.PV1_pv1_27Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_28,ds.CreateHL7PatientMessageLog.PV1_pv1_28Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_29,ds.CreateHL7PatientMessageLog.PV1_pv1_29Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_30,ds.CreateHL7PatientMessageLog.PV1_pv1_30Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_31,ds.CreateHL7PatientMessageLog.PV1_pv1_31Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_32,ds.CreateHL7PatientMessageLog.PV1_pv1_32Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_33,ds.CreateHL7PatientMessageLog.PV1_pv1_33Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_34,ds.CreateHL7PatientMessageLog.PV1_pv1_34Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_35,ds.CreateHL7PatientMessageLog.PV1_pv1_35Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_36,ds.CreateHL7PatientMessageLog.PV1_pv1_36Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_37,ds.CreateHL7PatientMessageLog.PV1_pv1_37Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_38,ds.CreateHL7PatientMessageLog.PV1_pv1_38Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_39,ds.CreateHL7PatientMessageLog.PV1_pv1_39Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_40,ds.CreateHL7PatientMessageLog.PV1_pv1_40Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_41,ds.CreateHL7PatientMessageLog.PV1_pv1_41Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_42,ds.CreateHL7PatientMessageLog.PV1_pv1_42Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_43,ds.CreateHL7PatientMessageLog.PV1_pv1_43Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_AdmitDateTime,ds.CreateHL7PatientMessageLog.PV1_AdmitDateTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_45,ds.CreateHL7PatientMessageLog.PV1_pv1_45Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_46,ds.CreateHL7PatientMessageLog.PV1_pv1_46Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_47,ds.CreateHL7PatientMessageLog.PV1_pv1_47Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_48,ds.CreateHL7PatientMessageLog.PV1_pv1_48Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_pv1_49,ds.CreateHL7PatientMessageLog.PV1_pv1_49Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PV1_AlternateVisitId,ds.CreateHL7PatientMessageLog.PV1_AlternateVisitIdColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_SegmentTypeID,ds.CreateHL7PatientMessageLog.PD1_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_SequenceNumber,ds.CreateHL7PatientMessageLog.PD1_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_Pd1_02,ds.CreateHL7PatientMessageLog.PD1_Pd1_02Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_Pd1_03,ds.CreateHL7PatientMessageLog.PD1_Pd1_03Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_PrimaryCareProviderDoctorCode,ds.CreateHL7PatientMessageLog.PD1_PrimaryCareProviderDoctorCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_ProviderCode,ds.CreateHL7PatientMessageLog.PD1_ProviderCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_ProviderLastName,ds.CreateHL7PatientMessageLog.PD1_ProviderLastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_ProviderFirstName,ds.CreateHL7PatientMessageLog.PD1_ProviderFirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_ProviderMiddleInitial,ds.CreateHL7PatientMessageLog.PD1_ProviderMiddleInitialColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_ProviderPrefix,ds.CreateHL7PatientMessageLog.PD1_ProviderPrefixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_ProviderSuffix,ds.CreateHL7PatientMessageLog.PD1_ProviderSuffixColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_Pd1_05,ds.CreateHL7PatientMessageLog.PD1_Pd1_05Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_PD1_ProviderFullName,ds.CreateHL7PatientMessageLog.PD1_ProviderFullNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_SegmentTypeId,ds.CreateHL7PatientMessageLog.GT1_SegmentTypeIdColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_SequenceNumber,ds.CreateHL7PatientMessageLog.GT1_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorNumber,ds.CreateHL7PatientMessageLog.GT1_GuarantorNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorName,ds.CreateHL7PatientMessageLog.GT1_GuarantorNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_LastName,ds.CreateHL7PatientMessageLog.GT1_LastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_FirstName,ds.CreateHL7PatientMessageLog.GT1_FirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_MiddleInitial,ds.CreateHL7PatientMessageLog.GT1_MiddleInitialColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorSpouseName,ds.CreateHL7PatientMessageLog.GT1_GuarantorSpouseNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorAddress,ds.CreateHL7PatientMessageLog.GT1_GuarantorAddressColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_Add1,ds.CreateHL7PatientMessageLog.GT1_Add1Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_Add2,ds.CreateHL7PatientMessageLog.GT1_Add2Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_City,ds.CreateHL7PatientMessageLog.GT1_CityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_State,ds.CreateHL7PatientMessageLog.GT1_StateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_Zip,ds.CreateHL7PatientMessageLog.GT1_ZipColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorPhoneNumHome,ds.CreateHL7PatientMessageLog.GT1_GuarantorPhoneNumHomeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorPhoneNumBusiness,ds.CreateHL7PatientMessageLog.GT1_GuarantorPhoneNumBusinessColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorDateofBirth,ds.CreateHL7PatientMessageLog.GT1_GuarantorDateofBirthColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorSex,ds.CreateHL7PatientMessageLog.GT1_GuarantorSexColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorType,ds.CreateHL7PatientMessageLog.GT1_GuarantorTypeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorRelationship,ds.CreateHL7PatientMessageLog.GT1_GuarantorRelationshipColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorSsn,ds.CreateHL7PatientMessageLog.GT1_GuarantorSsnColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorDateBegin,ds.CreateHL7PatientMessageLog.GT1_GuarantorDateBeginColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorDateEnd,ds.CreateHL7PatientMessageLog.GT1_GuarantorDateEndColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_GT1_GuarantorPriority,ds.CreateHL7PatientMessageLog.GT1_GuarantorPriorityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_SegmentTypeID,ds.CreateHL7PatientMessageLog.NK1_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_SequenceNumber,ds.CreateHL7PatientMessageLog.NK1_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_NextofKinName,ds.CreateHL7PatientMessageLog.NK1_NextofKinNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_NextofKinLastName,ds.CreateHL7PatientMessageLog.NK1_NextofKinLastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_NextofKinFirstName,ds.CreateHL7PatientMessageLog.NK1_NextofKinFirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_Relationship,ds.CreateHL7PatientMessageLog.NK1_RelationshipColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_Address,ds.CreateHL7PatientMessageLog.NK1_AddressColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_Addr1,ds.CreateHL7PatientMessageLog.NK1_Addr1Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_Addr2,ds.CreateHL7PatientMessageLog.NK1_Addr2Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_City,ds.CreateHL7PatientMessageLog.NK1_CityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_State,ds.CreateHL7PatientMessageLog.NK1_StateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_Zip,ds.CreateHL7PatientMessageLog.NK1_ZipColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_NK1_PhoneNumber,ds.CreateHL7PatientMessageLog.NK1_PhoneNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_SegmentTypeID,ds.CreateHL7PatientMessageLog.IN1_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_SequenceNumber,ds.CreateHL7PatientMessageLog.IN1_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsurancePlanID,ds.CreateHL7PatientMessageLog.IN1_InsurancePlanIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyID,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyName,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyAddress,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyAddressColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyAddr1,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyAddr1Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyAddr2,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyAddr2Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyCity,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyCityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyState,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyStateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCompanyZip,ds.CreateHL7PatientMessageLog.IN1_InsuranceCompanyZipColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCoContactPerson,ds.CreateHL7PatientMessageLog.IN1_InsuranceCoContactPersonColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuranceCoPhoneNumber,ds.CreateHL7PatientMessageLog.IN1_InsuranceCoPhoneNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_GroupNumber,ds.CreateHL7PatientMessageLog.IN1_GroupNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_GroupName,ds.CreateHL7PatientMessageLog.IN1_GroupNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredssGroupEmpID,ds.CreateHL7PatientMessageLog.IN1_InsuredssGroupEmpIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsGroupEmpName,ds.CreateHL7PatientMessageLog.IN1_InsuredsGroupEmpNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_PlanEffectiveStartDate,ds.CreateHL7PatientMessageLog.IN1_PlanEffectiveStartDateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_PlanExpirationEndDate,ds.CreateHL7PatientMessageLog.IN1_PlanExpirationEndDateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_AuthorizationInformation,ds.CreateHL7PatientMessageLog.IN1_AuthorizationInformationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_PlanType,ds.CreateHL7PatientMessageLog.IN1_PlanTypeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_NameofInsured,ds.CreateHL7PatientMessageLog.IN1_NameofInsuredColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_NameofInsuredLast,ds.CreateHL7PatientMessageLog.IN1_NameofInsuredLastColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_NameofInsuredFirst,ds.CreateHL7PatientMessageLog.IN1_NameofInsuredFirstColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_NameofInsuredMI,ds.CreateHL7PatientMessageLog.IN1_NameofInsuredMIColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsRelationshiptoPatient,ds.CreateHL7PatientMessageLog.IN1_InsuredsRelationshiptoPatientColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsDateofBirth,ds.CreateHL7PatientMessageLog.IN1_InsuredsDateofBirthColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsAddress,ds.CreateHL7PatientMessageLog.IN1_InsuredsAddressColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsAddressAddr1,ds.CreateHL7PatientMessageLog.IN1_InsuredsAddressAddr1Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsAddressAddr2,ds.CreateHL7PatientMessageLog.IN1_InsuredsAddressAddr2Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsAddressCity,ds.CreateHL7PatientMessageLog.IN1_InsuredsAddressCityColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsAddressState,ds.CreateHL7PatientMessageLog.IN1_InsuredsAddressStateColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_InsuredsAddressZip,ds.CreateHL7PatientMessageLog.IN1_InsuredsAddressZipColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_AssignmentofBenefits,ds.CreateHL7PatientMessageLog.IN1_AssignmentofBenefitsColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_CoordinationofBenefits,ds.CreateHL7PatientMessageLog.IN1_CoordinationofBenefitsColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_22,ds.CreateHL7PatientMessageLog.IN1_IN1_22Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_23,ds.CreateHL7PatientMessageLog.IN1_IN1_23Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_24,ds.CreateHL7PatientMessageLog.IN1_IN1_24Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_25,ds.CreateHL7PatientMessageLog.IN1_IN1_25Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_26,ds.CreateHL7PatientMessageLog.IN1_IN1_26Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_27,ds.CreateHL7PatientMessageLog.IN1_IN1_27Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_28,ds.CreateHL7PatientMessageLog.IN1_IN1_28Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_29,ds.CreateHL7PatientMessageLog.IN1_IN1_29Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_30,ds.CreateHL7PatientMessageLog.IN1_IN1_30Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_31,ds.CreateHL7PatientMessageLog.IN1_IN1_31Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_32,ds.CreateHL7PatientMessageLog.IN1_IN1_32Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_33,ds.CreateHL7PatientMessageLog.IN1_IN1_33Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_34,ds.CreateHL7PatientMessageLog.IN1_IN1_34Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_IN1_35,ds.CreateHL7PatientMessageLog.IN1_IN1_35Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_IN1_PolicyNumber,ds.CreateHL7PatientMessageLog.IN1_PolicyNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SegmentTypeID,ds.CreateHL7PatientMessageLog.SCH_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_TempAppointmentID,ds.CreateHL7PatientMessageLog.SCH_TempAppointmentIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentID,ds.CreateHL7PatientMessageLog.SCH_AppointmentIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_OccurrenceNumber,ds.CreateHL7PatientMessageLog.SCH_OccurrenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_PlacerGroupNumber,ds.CreateHL7PatientMessageLog.SCH_PlacerGroupNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_ScheduleID,ds.CreateHL7PatientMessageLog.SCH_ScheduleIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_EventReason,ds.CreateHL7PatientMessageLog.SCH_EventReasonColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentReason,ds.CreateHL7PatientMessageLog.SCH_AppointmentReasonColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentType,ds.CreateHL7PatientMessageLog.SCH_AppointmentTypeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentDuration,ds.CreateHL7PatientMessageLog.SCH_AppointmentDurationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentDurationUnit,ds.CreateHL7PatientMessageLog.SCH_AppointmentDurationUnitColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentTime,ds.CreateHL7PatientMessageLog.SCH_AppointmentTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentDuration_,ds.CreateHL7PatientMessageLog.SCH_AppointmentDuration_Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentStartTime,ds.CreateHL7PatientMessageLog.SCH_AppointmentStartTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentEndTime,ds.CreateHL7PatientMessageLog.SCH_AppointmentEndTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_12,ds.CreateHL7PatientMessageLog.SCH_SCH_12Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_13,ds.CreateHL7PatientMessageLog.SCH_SCH_13Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_14,ds.CreateHL7PatientMessageLog.SCH_SCH_14Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_15,ds.CreateHL7PatientMessageLog.SCH_SCH_15Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_16,ds.CreateHL7PatientMessageLog.SCH_SCH_16Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_17,ds.CreateHL7PatientMessageLog.SCH_SCH_17Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_18,ds.CreateHL7PatientMessageLog.SCH_SCH_18Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_19,ds.CreateHL7PatientMessageLog.SCH_SCH_19Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_ResourceName,ds.CreateHL7PatientMessageLog.SCH_ResourceNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_21,ds.CreateHL7PatientMessageLog.SCH_SCH_21Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_SCH_22,ds.CreateHL7PatientMessageLog.SCH_SCH_22Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentBillingComments,ds.CreateHL7PatientMessageLog.SCH_AppointmentBillingCommentsColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_AppointmentEncounterComments,ds.CreateHL7PatientMessageLog.SCH_AppointmentEncounterCommentsColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_SCH_VisitStatus,ds.CreateHL7PatientMessageLog.SCH_VisitStatusColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_SegmentTypeID,ds.CreateHL7PatientMessageLog.AIG_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_SequenceNumber,ds.CreateHL7PatientMessageLog.AIG_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_AIG_02,ds.CreateHL7PatientMessageLog.AIG_AIG_02Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_Resource,ds.CreateHL7PatientMessageLog.AIG_ResourceColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_ResourceID,ds.CreateHL7PatientMessageLog.AIG_ResourceIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_ResourceLastName,ds.CreateHL7PatientMessageLog.AIG_ResourceLastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_ResourceFirstName,ds.CreateHL7PatientMessageLog.AIG_ResourceFirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_ResourceTypeIdentifier,ds.CreateHL7PatientMessageLog.AIG_ResourceTypeIdentifierColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_AIG_05,ds.CreateHL7PatientMessageLog.AIG_AIG_05Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_AIG_06,ds.CreateHL7PatientMessageLog.AIG_AIG_06Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_AIG_07,ds.CreateHL7PatientMessageLog.AIG_AIG_07Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_AppointmentStartDateTime,ds.CreateHL7PatientMessageLog.AIG_AppointmentStartDateTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_AIG_09,ds.CreateHL7PatientMessageLog.AIG_AIG_09Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_AIG_10,ds.CreateHL7PatientMessageLog.AIG_AIG_10Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_Duration,ds.CreateHL7PatientMessageLog.AIG_DurationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIG_DurationUnits,ds.CreateHL7PatientMessageLog.AIG_DurationUnitsColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_SegmentTypeID,ds.CreateHL7PatientMessageLog.AIP_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_SequenceNumber,ds.CreateHL7PatientMessageLog.AIP_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_SegmentActionCode,ds.CreateHL7PatientMessageLog.AIP_SegmentActionCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_OrderingProvider,ds.CreateHL7PatientMessageLog.AIP_OrderingProviderColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_OrderingProviderID,ds.CreateHL7PatientMessageLog.AIP_OrderingProviderIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_OrderingProviderLastName,ds.CreateHL7PatientMessageLog.AIP_OrderingProviderLastNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_OrderingProviderFirstName,ds.CreateHL7PatientMessageLog.AIP_OrderingProviderFirstNameColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_ResourceRole,ds.CreateHL7PatientMessageLog.AIP_ResourceRoleColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_AIP_05,ds.CreateHL7PatientMessageLog.AIP_AIP_05Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_AppointmentStartDateTime,ds.CreateHL7PatientMessageLog.AIP_AppointmentStartDateTimeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_AIP_07,ds.CreateHL7PatientMessageLog.AIP_AIP_07Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_AIP_08,ds.CreateHL7PatientMessageLog.AIP_AIP_08Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_Duration,ds.CreateHL7PatientMessageLog.AIP_DurationColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_DurationUnits,ds.CreateHL7PatientMessageLog.AIP_DurationUnitsColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_AIP_11,ds.CreateHL7PatientMessageLog.AIP_AIP_11Column.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIP_FillerStatusCode,ds.CreateHL7PatientMessageLog.AIP_FillerStatusCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIL_SegmentTypeID,ds.CreateHL7PatientMessageLog.AIL_SegmentTypeIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIL_SequenceNumber,ds.CreateHL7PatientMessageLog.AIL_SequenceNumberColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIL_SegmentActionCode,ds.CreateHL7PatientMessageLog.AIL_SegmentActionCodeColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIL_LocationResource,ds.CreateHL7PatientMessageLog.AIL_LocationResourceColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index++,PARM_AIL_LocationResourceID,ds.CreateHL7PatientMessageLog.AIL_LocationResourceIDColumn.ColumnName,DbType.String);
           dbManager.AddParameters(index  ,PARM_AIL_LocationResourceName,ds.CreateHL7PatientMessageLog.AIL_LocationResourceNameColumn.ColumnName,DbType.String);



            //if (IsInsert == true)
            //    dbManager.AddParameters(0, PARM_SECTION_ID, ds..SectionIdColumn.ColumnName, DbType.Int64, ParamDirection.Output);
            //else
            //    dbManager.AddParameters(0, PARM_SECTION_ID, ds.ClinicalSection.SectionIdColumn.ColumnName, DbType.Int64);

         
        }

    }
}
