using NHapi.Base.Parser;
using NHapi.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using MDVision.Business.BCommon;
using System.Globalization;
using MDVision.Common.Utilities;

namespace MDVision.IEHR.EMR.Model.HL7
{
    public class ClinicalHL7Reader
    {
        private static ClinicalHL7Reader _instance = null;
        public static ClinicalHL7Reader Instance()
        {
            if (_instance == null)
                _instance = new ClinicalHL7Reader();
            return _instance;
        }
        public static DateTime ChangeTime(DateTime dateTime, int Year, int Month, int Day, int hours, int minutes, int seconds)
        {
            return new DateTime(
                Year,
                Month,
                Day,
                hours,
                minutes,
                seconds,
                00,
                dateTime.Kind);
        }
        public static readonly Dictionary<string, string> interpetationCodes = new Dictionary<string, string>
        {
          {    "L"  ,  "Abnormally Low"},// "Below low normal "     } ,
          {    "H"  ,   "Above high normal"  } ,
          {    "LU"  ,   "Low Urgent"  } ,
          {    "HU"  ,   "High Urgent"  } ,
          {    "LL"  ,   "Below lower panic limits"  } ,
          {    "HH"  ,   "Abnormally High"},//"Above upper panic limits" },
          {    "<"  ,   "Below absolute low-off instrument scale" },
          {    ">"  ,   "Above absolute high-off instrument scale" },
          {    "N"  ,   "Normal" },//"Normal (applies to non-numeric results)" },
          {    "Neg",   "Negative" },
          {    "A"  ,   "Abnormal" },//"Abnormal (applies to non-numeric results)" },
          {    "AA"  ,   "Very abnormal (applies to non-numeric units, analogous to panic limits for numeric units) " },
          {    "null"  ,   "No range defined, or normal ranges don't apply" },
          {    "U"  ,   "Significant change up" },
          {    "D"  ,   "Significant change down" },
          {    "B"  ,   "Better—use when direction not relevant" },
          {    "W"  ,   "Worse—use when direction not relevant" },
          {    "S"  ,   "Susceptible. Indicates for microbiology susceptibilities only" },
          {    "R"  ,   "Resistant. Indicates for microbiology susceptibilities only." },
          {    "I"  ,   "Intermediate. Indicates for microbiology susceptibilities only." },
          {    "MS"  ,   "Moderately susceptible. Indicates for microbiology susceptibilities only." }

       };

        public static readonly Dictionary<string, string> specimenRejectReason = new Dictionary<string, string>
        {
          {    "EX"  ,  "Expired"},
          {    "QS"  ,   "Quantitiy not sufficient"  } ,
          {    "RA"  ,   "Missing patient ID number"  } ,
          {    "RB"  ,   "Broken container"  } ,
          {    "RC"  ,   "Clotting"  } ,
          {    "RD"  ,   "Missing collection date"},
          {    "RE"  ,   "Missing patient name" },
          {    "RH"  ,   "Hemolysis" },
          {    "RI"  ,   "Identification problem" },
          {    "RM"  ,   "Labeling" },
          {    "RN"  ,   "Contamination" },
          {    "RP"  ,   "Missing phlebotomist ID" },
          {    "RR"  ,   "Improper storage" },
          {    "RS"  ,   "Name misspelling" }

       };
        public List<OBR_HL7> getHL7Message(string message)
        {
            List<OBR_HL7> OBRobjList = new List<OBR_HL7>();
            try
            {
                PipeParser parser = new PipeParser();
                IMessage m = parser.Parse(message);
                if (m != null)
                {
                    NHapi.Model.V251.Message.ORU_R01 qryR01 = m as NHapi.Model.V251.Message.ORU_R01;

                    if (qryR01 != null)
                    {

                        var ORDER_OBSERVATIONCount = qryR01.GetPATIENT_RESULT().ORDER_OBSERVATIONRepetitionsUsed;
                        var pidinfo = qryR01.GetPATIENT_RESULT().PATIENT.PID;
                        string PatientAccountNumber = pidinfo.PatientAccountNumber.IDNumber.Value;

                        for (int i = 0; i < ORDER_OBSERVATIONCount; i++)
                        {
                            OBR_HL7 OBRobj = new OBR_HL7();
                            string OrderId = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION(i).ORC.PlacerOrderNumber.EntityIdentifier.Value;
                            OBRobj.OrderId = OrderId;
                            var ObservationRequest = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION(i).OBR;
                            OBRobj.OBRSetId = ObservationRequest.SetIDOBR.Value;//X
                            OBRobj.PlacerOrderNumberLabTestId = ObservationRequest.PlacerOrderNumber.EntityIdentifier.Value;
                            OBRobj.FillerOrderNumber = ObservationRequest.FillerOrderNumber.EntityIdentifier.Value;
                            // UniversalServiceIdentifier = USI
                            var USI = ObservationRequest.UniversalServiceIdentifier;
                            OBRobj.USIIdentifier = USI.Identifier.Value; //R
                            OBRobj.USIText = USI.Text.Value; //RE
                            OBRobj.USICodingSystem = USI.NameOfCodingSystem.Value; //RE
                            if (!string.IsNullOrEmpty(ObservationRequest.ObservationDateTime.Time.Value))
                            {
                                var tempTime = ObservationRequest.ObservationDateTime.Time;
                                OBRobj.ObserVationDateTime = ChangeTime(DateTime.Now, tempTime.Year, tempTime.Month, tempTime.Day,
                                    tempTime.Hour, tempTime.Minute, tempTime.Second);
                            }
                            // OBRobj.ObserVationDateTime = ObservationRequest.ObservationDateTime.Time.GetAsDate();
                            if (!string.IsNullOrEmpty(ObservationRequest.ObservationEndDateTime.Time.Value))
                            {
                                var tempTime = ObservationRequest.ObservationEndDateTime.Time;
                                OBRobj.ObserVationEndDateTime = ChangeTime(DateTime.Now, tempTime.Year, tempTime.Month, tempTime.Day,
                                    tempTime.Hour, tempTime.Minute, tempTime.Second);
                            }
                            // OBRobj.ObserVationEndDateTime = ObservationRequest.ObservationEndDateTime.Time.GetAsDate(); // required but can be Empty
                            //string OBRResultStatus = ObservationRequest.ResultStatus.Value;

                            OBRobj.SpecimenActionCode = ObservationRequest.SpecimenActionCode.Value;
                            OBRobj.SpecimenSource = ObservationRequest.SpecimenSource.SpecimenSourceNameOrCode.Identifier.Value;//RE
                            // OBRobj.OBROrderingProvider = ObservationRequest.GetOrderingProvider(i).AssigningFacility.NamespaceID.Value;//R //check

                            OBRobj.FillerField1 = ObservationRequest.FillerField1.Value;//O
                            OBRobj.FillerField2 = ObservationRequest.FillerField2.Value;//O
                            if (!string.IsNullOrEmpty(ObservationRequest.ResultsRptStatusChngDateTime.Time.Value))
                            {
                                var tempTime = ObservationRequest.ResultsRptStatusChngDateTime.Time;
                                OBRobj.ResultsRptStatusChngDateTime = ChangeTime(DateTime.Now, tempTime.Year, tempTime.Month, tempTime.Day,
                                    tempTime.Hour, tempTime.Minute, tempTime.Second);
                            }
                            //OBRobj.ResultsRptStatusChngDateTime = ObservationRequest.ResultsRptStatusChngDateTime.Time.GetAsDate();//RE
                            OBRobj.ResultStatus = ObservationRequest.ResultStatus.Value; //R

                            // var ObservationTestId = ob.FillerField1.Value;
                            //  string PlacerOrderNumber = ob.PlacerOrderNumber.NamespaceID.Value;
                            //  ObservationTestId = ob.PlacerOrderNumber.EntityIdentifier.Value;
                            //  DSLabOrder dsLabOrder = null, dsLabProblems = null;
                            //     BLObject<DSLabOrder> obj = BLLClinicalObj.LoadLabOrder(MDVUtility.ToInt64(LabOrderId), MDVUtility.ToInt64(PatientId), 0, model.PageNumber, model.RowsPerPage, "", "", 0, "", "", "", 0, "1", "");
                            if (qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION(i) != null)
                            {
                                OBRobj.OBRcount = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION(i).OBSERVATIONRepetitionsUsed;
                                var orcInfo = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION(i).ORC;
                                OBRobj.OBXmodalList = new List<OBX_HL7>();
                                for (int j = 0; j < OBRobj.OBRcount; j++)
                                {
                                    if (qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION(i).GetOBSERVATION(j) != null)
                                    {
                                        OBX_HL7 OBXObj = new OBX_HL7();
                                        var OBXinfo = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION(i).GetOBSERVATION(j).OBX;


                                        if (!string.IsNullOrEmpty(OBXinfo.SetIDOBX.Value))
                                        {
                                            OBXObj.SetIDOBX = OBXinfo.SetIDOBX.Value;
                                            OBXObj.ValueType = OBXinfo.ValueType.Value;
                                            if (OBXObj.ValueType.Equals("NM"))
                                            {
                                                for (int z = 0; z < OBXinfo.ObservationValueRepetitionsUsed; z++)
                                                {
                                                    /*CE Coded Element NM Numeric SN Structured Numeric ST String Data TX Text Data FT Formatted text*/
                                                    OBXObj.ObservationValue = OBXinfo.GetObservationValue(z).Data.ToString();
                                                    OBXObj.Units = OBXinfo.Units.Identifier.Value; //RE
                                                    OBXObj.ReferencesRange = OBXinfo.ReferencesRange.Value;
                                                    OBXObj.AbnormalFlags = OBXinfo.GetAbnormalFlags(z).Value;
                                                }
                                                //string prob = OBXinfo.Probability.Value;
                                                //string ObservationValue = OBXinfo.ObservationValueRepetitionsUsed;
                                                //   string NatureOfAbnormalTest = OBXinfo.GetNatureOfAbnormalTest(0).Value;
                                                OBXObj.ObservationResultStatus = OBXinfo.ObservationResultStatus.Value;
                                            }
                                            else if (OBXObj.ValueType.Equals("CWE"))
                                            {

                                                for (int z = 0; z < OBXinfo.ObservationValueRepetitionsUsed; z++)
                                                {
                                                    NHapi.Model.V251.Datatype.CWE ce = (OBXinfo.GetObservationValue(z).Data) as NHapi.Model.V251.Datatype.CWE;// new NHapi.Model.V251.Datatype.CWE(OBXinfo.GetObservationValue(0));// new NHapi.Model.V251.Datatype.CWE();
                                                    /*CE Coded Element NM Numeric SN Structured Numeric ST String Data TX Text Data FT Formatted text*/
                                                    OBXObj.ObservationValue = ce.Text.Value;
                                                    //OBXObj.ObservationValue = ce.Identifier.Value + ", " +
                                                    //    ce.Text.Value + ", " +
                                                    //    ce.NameOfCodingSystem.Value + ", " +
                                                    //    ce.OriginalText.Value;//.GetObservationValue(0).Data.ToString();
                                                    OBXObj.Units = "NA";// OBXinfo.Units.Identifier.Value; //RE
                                                    OBXObj.ReferencesRange = null;
                                                    OBXObj.AbnormalFlags = OBXinfo.GetAbnormalFlags(z).Value;
                                                }
                                                //string prob = OBXinfo.Probability.Value;
                                                //string ObservationValue = OBXinfo.ObservationValueRepetitionsUsed;
                                                //   string NatureOfAbnormalTest = OBXinfo.GetNatureOfAbnormalTest(0).Value;
                                                OBXObj.ObservationResultStatus = OBXinfo.ObservationResultStatus.Value;
                                            }
                                            else if (OBXObj.ValueType.Equals("TX"))
                                            {
                                                for (int z = 0; z < OBXinfo.ObservationValueRepetitionsUsed; z++)
                                                {
                                                    /*CE Coded Element NM Numeric SN Structured Numeric ST String Data TX Text Data FT Formatted text*/
                                                    OBXObj.ObservationValue = OBXinfo.GetObservationValue(z).Data.ToString();
                                                    OBXObj.Units = null;// OBXinfo.Units.Identifier.Value; //RE
                                                    OBXObj.ReferencesRange = null;
                                                    OBXObj.AbnormalFlags = OBXinfo.GetAbnormalFlags(z).Value;
                                                }
                                                //string prob = OBXinfo.Probability.Value;
                                                //string ObservationValue = OBXinfo.ObservationValueRepetitionsUsed;
                                                //   string NatureOfAbnormalTest = OBXinfo.GetNatureOfAbnormalTest(0).Value;
                                                OBXObj.ObservationResultStatus = OBXinfo.ObservationResultStatus.Value;
                                            }
                                            else if (OBXObj.ValueType.Equals("SN"))
                                            {
                                                for (int z = 0; z < OBXinfo.ObservationValueRepetitionsUsed; z++)
                                                {
                                                    /*CE Coded Element NM Numeric SN Structured Numeric ST String Data TX Text Data FT Formatted text*/
                                                    NHapi.Model.V251.Datatype.SN sn = OBXinfo.GetObservationValue(z).Data as NHapi.Model.V251.Datatype.SN;
                                                    OBXObj.ObservationValue = sn.Comparator.Value + sn.Num1.Value;
                                                    OBXObj.Units = OBXinfo.Units.Identifier.Value; //RE
                                                    OBXObj.ReferencesRange = OBXinfo.ReferencesRange.Value;
                                                    OBXObj.AbnormalFlags = OBXinfo.GetAbnormalFlags(z).Value;
                                                }
                                                OBXObj.ObservationResultStatus = OBXinfo.ObservationResultStatus.Value;
                                            }
                                            if (!string.IsNullOrEmpty(OBXObj.AbnormalFlags))
                                            {
                                                if (OBXObj.AbnormalFlags.Equals("N") && OBXObj.ObservationValue.ToLower().IndexOf("negative") != -1)
                                                {
                                                    OBXObj.AbnormalFlags = interpetationCodes["Neg"];
                                                }
                                                else
                                                {
                                                    OBXObj.AbnormalFlags = interpetationCodes[OBXObj.AbnormalFlags];
                                                }
                                            }
                                            OBXObj.observationIdentifier = OBXinfo.ObservationIdentifier.Identifier.Value;//loinc
                                            OBXObj.ObIdentifierSystem = OBXinfo.ObservationIdentifier.NameOfCodingSystem.Value;
                                            if (!string.IsNullOrEmpty(OBXObj.ObIdentifierSystem) && OBXObj.ObIdentifierSystem.Equals("LN"))
                                            {
                                                OBXObj.LoincCode = OBXObj.observationIdentifier;
                                                OBXObj.LoincDesc = OBXinfo.ObservationIdentifier.Text.Value;
                                            }
                                            OBXObj.obServationSubId = OBXinfo.ObservationSubID.Value;


                                            //  string UserDefinedAccessChecks = OBXinfo.UserDefinedAccessChecks.Value;
                                            //  string ProducerSReference = OBXinfo.ProducerSReference.Identifier.Value;
                                            //   string ObservationMethod = OBXinfo.GetObservationMethod(0).Identifier.Value;

                                            //Setting NTE information
                                            int NTECount = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetNTE().CommentRepetitionsUsed;
                                            for (int k = 0; k < NTECount; k++)
                                            {
                                                OBXObj.FillerInstructions = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetNTE().GetComment(k).Value;
                                                OBRobj.Comments = OBXObj.FillerInstructions;
                                            }
                                            //Getting TQ Information
                                            int QTYCount = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().TIMING_QTYRepetitionsUsed;
                                            if (QTYCount < 1 || string.IsNullOrEmpty(qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetTIMING_QTY(0).TQ1.EndDateTime.Time.Value))
                                            {
                                                if (!string.IsNullOrEmpty(OBXinfo.DateTimeOfTheObservation.Time.Value))
                                                {
                                                    var tempTime = OBXinfo.DateTimeOfTheObservation.Time;
                                                    OBXObj.DateTimeOfTheObservation = ChangeTime(DateTime.Now, tempTime.Year, tempTime.Month, tempTime.Day,
                                                        tempTime.Hour, tempTime.Minute, tempTime.Second);
                                                }
                                                else
                                                {
                                                    OBXObj.DateTimeOfTheObservation = null;
                                                }
                                                //   OBXObj.DateTimeOfTheObservation = OBXinfo.DateTimeOfTheObservation.Time.GetAsDate();
                                            }
                                            else
                                            {
                                                for (int n = 0; n < QTYCount; n++)
                                                {
                                                    if (!string.IsNullOrEmpty(qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetTIMING_QTY(n).TQ1.EndDateTime.Time.Value))
                                                    {
                                                        var tempTime = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetTIMING_QTY(n).TQ1.EndDateTime.Time;
                                                        OBXObj.DateTimeOfTheObservation = ChangeTime(DateTime.Now, tempTime.Year, tempTime.Month, tempTime.Day,
                                                            tempTime.Hour, tempTime.Minute, tempTime.Second);
                                                    }
                                                    else
                                                    {
                                                        OBXObj.DateTimeOfTheObservation = null;
                                                    }
                                                    //  OBXObj.DateTimeOfTheObservation = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetTIMING_QTY(n).TQ1.EndDateTime.Time.GetAsDate();
                                                    if (!string.IsNullOrEmpty(qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetTIMING_QTY(n).TQ1.StartDateTime.Time.Value))
                                                    {
                                                        var tempTime = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetTIMING_QTY(n).TQ1.StartDateTime.Time;
                                                        OBXObj.EndDateTimeOfTheObservation = ChangeTime(DateTime.Now, tempTime.Year, tempTime.Month, tempTime.Day,
                                                            tempTime.Hour, tempTime.Minute, tempTime.Second);
                                                    }
                                                    else
                                                    {
                                                        OBXObj.EndDateTimeOfTheObservation = null;
                                                    }
                                                    // OBXObj.EndDateTimeOfTheObservation = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetTIMING_QTY(n).TQ1.StartDateTime.Time.GetAsDate();
                                                }
                                            }

                                            OBRobj.OBXmodalList.Add(OBXObj);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            #region SPM
                            int countSpecimen = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().SPECIMENRepetitionsUsed;
                            for (int sp = 0; sp < countSpecimen; sp++)
                            {
                                SPM_HL7 SPMObj = new SPM_HL7();
                                NHapi.Model.V251.Segment.SPM SPMQry = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetSPECIMEN(sp).SPM;
                                if (!string.IsNullOrEmpty(SPMQry.SpecimenCollectionDateTime.RangeStartDateTime.Time.Value))
                                {
                                    var tempTime = SPMQry.SpecimenCollectionDateTime.RangeStartDateTime.Time;
                                    SPMObj.CollectionDateTime = ChangeTime(DateTime.Now, tempTime.Year, tempTime.Month, tempTime.Day,
                                        tempTime.Hour, tempTime.Minute, tempTime.Second);
                                }
                                // SPMObj.CollectionDateTime = SPMQry.SpecimenCollectionDateTime.RangeStartDateTime.Time.GetAsDate();
                                SPMObj.LabOrderId = MDVUtility.ToLong(OrderId);
                                SPMObj.LabResultSpecimenId = -(sp + 1);
                                SPMObj.NameofCodingSystem = SPMQry.SpecimenType.NameOfCodingSystem.Value;
                                SPMObj.OriginalText = SPMQry.SpecimenType.OriginalText.Value;
                                SPMObj.Quantity = SPMQry.SpecimenCurrentQuantity.Quantity.Value;
                                SPMObj.SpecimenId = -(sp + 1);
                                SPMObj.SpecimenType = SPMQry.SpecimenType.Identifier.Value;
                                for (int z = 0; z < SPMQry.SpecimenRejectReasonRepetitionsUsed; z++)
                                {
                                    string RejectReason = SPMQry.GetSpecimenRejectReason(z).Identifier.Value;
                                    if (!string.IsNullOrEmpty(RejectReason))
                                    {
                                        SPM_RejectReason_HL7 RejectReasonObj = new SPM_RejectReason_HL7();
                                        RejectReasonObj = new SPM_RejectReason_HL7();//specimenRejectReason
                                        RejectReasonObj.Identifier = RejectReason;
                                        RejectReasonObj.AlternateIdentifier = SPMQry.GetSpecimenRejectReason(z).AlternateIdentifier.Value;
                                        RejectReasonObj.AlternateText = SPMQry.GetSpecimenRejectReason(z).AlternateText.Value;
                                        RejectReasonObj.NameofAlternateCodingSystem = SPMQry.GetSpecimenRejectReason(z).NameOfAlternateCodingSystem.Value;
                                        RejectReasonObj.NameofCodingSystem = SPMQry.GetSpecimenRejectReason(z).NameOfCodingSystem.Value;
                                        RejectReasonObj.OriginalText = SPMQry.GetSpecimenRejectReason(z).OriginalText.Value;
                                        RejectReasonObj.Text = SPMQry.GetSpecimenRejectReason(z).Text.Value;
                                        SPMObj.RejectReasonList.Add(RejectReasonObj);
                                    }
                                }
                                if (SPMQry.SpecimenConditionRepetitionsUsed != null && SPMQry.SpecimenConditionRepetitionsUsed > 0)
                                {
                                    if (!string.IsNullOrEmpty(SPMQry.GetSpecimenCondition(0).Identifier.Value))
                                    {
                                        SPMObj.Identifier = SPMQry.GetSpecimenCondition(0).Identifier.Value;
                                        SPMObj.ConditionText = SPMQry.GetSpecimenCondition(0).Text.Value;
                                        SPMObj.ConditionNOCSystem = SPMQry.GetSpecimenCondition(0).NameOfCodingSystem.Value;
                                        SPMObj.AlternateIdentifier = SPMQry.GetSpecimenCondition(0).AlternateIdentifier.Value;
                                        SPMObj.AlternateText = SPMQry.GetSpecimenCondition(0).AlternateText.Value;
                                        SPMObj.NameofAlternateCodingSystem = SPMQry.GetSpecimenCondition(0).NameOfAlternateCodingSystem.Value;
                                        SPMObj.ConditionOriginalText = SPMQry.GetSpecimenCondition(0).OriginalText.Value;
                                        SPMObj.Text = SPMQry.SpecimenType.Text.Value;
                                    }
                                }

                                OBRobj.SPMmodalList.Add(SPMObj);
                            }
                            #endregion
                            OBRobjList.Add(OBRobj);
                        }
                    }
                }
                //


                //  int NTECount = qryR01.GetPATIENT_RESULT().GetORDER_OBSERVATION().GetSPECIMEN().SPM.NumberOfSpecimenContainers.Value;
                //
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return OBRobjList;
        }
    }
}