using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BLL;
using MDVision.Model.Clinical.EMCodeGenerator;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
namespace MDVision.IEHR.EMR.Helpers.Clinical.BillingInformation
{
    public class EMCodeGeneratorHelper
    {
        private BLLEMCodeGenerator BLLEMCode = null;

        public EMCodeGeneratorHelper()
        {
            BLLEMCode = new BLLEMCodeGenerator();
        }
        private static EMCodeGeneratorHelper _instance = null;
        public static EMCodeGeneratorHelper Instance()
        {
            if (_instance == null)
                _instance = new EMCodeGeneratorHelper();
            return _instance;
        }

        private EMCodeGeneratorDataHolder DataHolder = null;
        public string SuggestNewPatientEMCodes(long PatientId, long NotesId)
        {
            EMCodes _EMCode = new EMCodes();
            EMCodeGeneratorDataHolder data = null;
            long UserId = MDVSession.Current.AppUserId;
            try
            {
                data = BLLEMCode.LoadEMCodeGeneratorData(PatientId, NotesId, UserId);
                if (data.ErrorMessage != null)
                {
                    var response = new
                    {
                        status = false,
                        message = data.ErrorMessage
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    this.DataHolder = data;
                    EMCodeGeneratorHistoryModel History = data._EMCodeHistory;
                    EMCodeGeneratorMDMModel MDM = data._EMCodeMDM;
                    List<EMCodeGeneratorExamModel> Exam = data._EMCodeExam;
                    string Code = "";
                    // Check if only one component is attached then suggest 99201 otherwise suggest through algorithm
                    long NumberOfHistoryComponents = History.ROS_Count + History.Hx_Count + History.HPI_Count;
                    long NumberOfMDMComponents = 0;
                    long NumberOfExamComponents = 0;
                    if (History.IsChiefComplaint == true)
                    {
                        NumberOfHistoryComponents++;
                    }

                    if (MDM.HasLabOrderOrResult == true || MDM.HasLabResultRemarksOrComments == true || MDM.HasRadiologyOrderOrResult == true || MDM.HasRadiologyResultRemarksOrComments == true)
                    {
                        NumberOfMDMComponents++;
                    }
                    if (MDM.NoOfICD > 0)
                    {
                        NumberOfMDMComponents++;
                    }
                    if (Exam.Count > 0)
                    {
                        NumberOfExamComponents++;
                    }
                    if ((NumberOfExamComponents + NumberOfHistoryComponents + NumberOfMDMComponents) == 1)
                    {
                        Code = "99201";
                    }

                    else
                    {
                        #region MDM
                        // Check ICD's
                        if (MDM.NoOfICD == 1)
                        {
                            _EMCode.increaseProbability("99201");
                            _EMCode.increaseProbability("99202");
                        }
                        else if (MDM.NoOfICD == 2)
                        {
                            _EMCode.increaseProbability("99202");
                        }
                        else if (MDM.NoOfICD == 3)
                        {
                            _EMCode.increaseProbability("99204");
                        }
                        else if (MDM.NoOfICD >= 4)
                        {
                            _EMCode.increaseProbability("99205");
                        }
                        // Check Data points
                        if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true) && (MDM.HasLabResultRemarksOrComments != true && MDM.HasRadiologyResultRemarksOrComments != true))
                        {
                            _EMCode.increaseProbability("99203");
                        }
                        else if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true) && (MDM.HasLabResultRemarksOrComments == true && MDM.HasLabResultRemarksOrComments == true))
                        {
                            _EMCode.increaseProbability("99205");
                        }
                        else if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == false) || (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == true))
                        {
                            _EMCode.increaseProbability("99201");
                            _EMCode.increaseProbability("99202");
                        }
                        else if ((MDM.HasLabOrderOrResult == true && MDM.HasLabResultRemarksOrComments == true) || (MDM.HasRadiologyOrderOrResult == true && MDM.HasRadiologyResultRemarksOrComments == true))
                        {
                            _EMCode.increaseProbability("99204");
                        }
                        #endregion

                        #region History
                        if (History.IsChiefComplaint == true)
                        {
                            _EMCode.increaseProbability("99201");
                            _EMCode.increaseProbability("99202");
                            _EMCode.increaseProbability("99203");
                            _EMCode.increaseProbability("99204");
                            _EMCode.increaseProbability("99205");
                        }
                        // HPI
                        if (History.HPI_Count > 0 && History.HPI_Count < 4)
                        {
                            _EMCode.increaseProbability("99201");
                            _EMCode.increaseProbability("99202");
                        }
                        else if (History.HPI_Count > 3)
                        {
                            _EMCode.increaseProbability("99203");
                            _EMCode.increaseProbability("99204");
                            _EMCode.increaseProbability("99205");
                        }
                        // ROS
                        //if (History.ROS_Count == 0)
                        //{
                        //    _EMCode.increaseProbability("99201");
                        //}
                        if (History.ROS_Count == 1)
                        {
                            _EMCode.increaseProbability("99202");
                        }
                        else if (History.ROS_Count > 1 && History.ROS_Count < 10)
                        {
                            _EMCode.increaseProbability("99203");
                        }
                        else if (History.ROS_Count > 9)
                        {
                            _EMCode.increaseProbability("99204");
                            _EMCode.increaseProbability("99205");
                        }
                        // Previous History
                        //if (History.Hx_Count == 0)
                        //{
                        //    _EMCode.increaseProbability("99201");
                        //    _EMCode.increaseProbability("99202");
                        //}
                        if (History.Hx_Count == 1)
                        {
                            _EMCode.increaseProbability("99203");
                        }
                        else if (History.Hx_Count > 2)
                        {
                            if (History.FamilyHx == 1 && History.MedicalHx == 1 && History.SocialHx == 1)
                            {
                                _EMCode.increaseProbability("99204");
                                _EMCode.increaseProbability("99205");
                            }
                            else
                            {
                                _EMCode.increaseProbability("99203");
                            }
                        }
                        #endregion

                        #region Exam
                        int NoOfSystems = Exam.Count;
                        if (NoOfSystems > 0)
                        {
                            int totalElementsInAllSystems = 0;
                            foreach (var elem in Exam)
                            {
                                totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                            }
                            if (NoOfSystems == 1)     // Single Organ System
                            {
                                foreach (var elem in Exam)
                                {
                                    if (elem.NoOfElements >= elem.TotalNoOfElementsInSystem)
                                    {
                                        _EMCode.increaseProbability("99204");
                                        _EMCode.increaseProbability("99205");
                                    }

                                    if (elem.NoOfElements > 0 && elem.NoOfElements < 6)
                                    {
                                        _EMCode.increaseProbability("99201");
                                    }
                                    else if (elem.NoOfElements >= 6 && elem.NoOfElements < 12)
                                    {
                                        _EMCode.increaseProbability("99202");
                                    }
                                    else if (elem.NoOfElements > 11)
                                    {
                                        _EMCode.increaseProbability("99203");
                                    }
                                }
                            }
                            else if (NoOfSystems > 1) // Multi Organ System
                            {
                                int elementCounter = 0;
                                bool TwoElementsInEachSystem = false;
                                foreach (var elem in Exam)
                                {
                                    if (elem.NoOfElements >= 2)  // Check if all systems have 2 elements
                                    {
                                        elementCounter++;
                                    }
                                }
                                if (elementCounter == Exam.Count)
                                {
                                    TwoElementsInEachSystem = true;
                                }

                                if (TwoElementsInEachSystem == true)
                                {
                                    if (NoOfSystems > 5 && NoOfSystems < 10)
                                    {
                                        _EMCode.increaseProbability("99203");
                                    }
                                    else if (NoOfSystems > 8)
                                    {
                                        _EMCode.increaseProbability("99204");
                                        _EMCode.increaseProbability("99205");
                                    }
                                }
                                else
                                {
                                    if (totalElementsInAllSystems > 0 && totalElementsInAllSystems < 6)
                                    {
                                        _EMCode.increaseProbability("99201");
                                    }
                                    else if (totalElementsInAllSystems > 5 && totalElementsInAllSystems < 12)
                                    {
                                        _EMCode.increaseProbability("99202");
                                    }
                                    else if (totalElementsInAllSystems > 12)
                                    {
                                        _EMCode.increaseProbability("99203");
                                    }
                                }
                            }
                        }
                        #endregion

                        Code = _EMCode.getNewPatientCodeSuggestion();
                    }
                    string suggestions = SuggestNextNewPatientEMCode(Code);
                    var response = new
                    {
                        status = true,
                        SuggestedCode = Code,
                        NextSuggestions = suggestions
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception e)
            {
                var response = new
                {
                    status = false,
                    message = e.Message
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string SuggestEstablishedPatientEMCodes(long PatientId, long NotesId)
        {
            EMCodes _EMCode = new EMCodes();
            EMCodeGeneratorDataHolder data = null;
            long UserId = MDVSession.Current.AppUserId;
            data = BLLEMCode.LoadEMCodeGeneratorData(PatientId, NotesId, UserId);
            if (data.ErrorMessage != null)
            {
                var response = new
                {
                    status = false,
                    message = data.ErrorMessage
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            else
            {
                this.DataHolder = data;
                EMCodeGeneratorHistoryModel History = data._EMCodeHistory;
                EMCodeGeneratorMDMModel MDM = data._EMCodeMDM;
                List<EMCodeGeneratorExamModel> Exam = data._EMCodeExam;
                #region MDM
                // Check ICD's

                 if (MDM.NoOfICD == 1)
                {
                    _EMCode.increaseProbability("99212");
                }
                else if (MDM.NoOfICD == 2)
                {
                    _EMCode.increaseProbability("99213");
                }
                else if (MDM.NoOfICD == 3)
                {
                    _EMCode.increaseProbability("99214");
                }
                else if (MDM.NoOfICD >= 4)
                {
                    _EMCode.increaseProbability("99215");
                }
                if (MDM.HasLabOrderOrResult == false && MDM.HasLabResultRemarksOrComments == false && MDM.HasRadiologyOrderOrResult == false && MDM.HasRadiologyResultRemarksOrComments == false)
                {
                    _EMCode.increaseProbability("99211");
                }
                else if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true) && (MDM.HasLabResultRemarksOrComments != true && MDM.HasRadiologyResultRemarksOrComments != true))
                {
                    _EMCode.increaseProbability("99213");
                }
                else if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true) && (MDM.HasLabResultRemarksOrComments == true && MDM.HasLabResultRemarksOrComments == true))
                {
                    _EMCode.increaseProbability("99215");
                }
                else if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == false) || (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == true))
                {
                    _EMCode.increaseProbability("99212");
                }
                else if ((MDM.HasLabOrderOrResult == true && MDM.HasLabResultRemarksOrComments == true) || (MDM.HasRadiologyOrderOrResult == true && MDM.HasRadiologyResultRemarksOrComments == true))
                {
                    _EMCode.increaseProbability("99214");
                }
                #endregion
                #region History
                if (History.IsChiefComplaint == true)
                {
                    _EMCode.increaseProbability("99212");
                    _EMCode.increaseProbability("99213");
                    _EMCode.increaseProbability("99214");
                    _EMCode.increaseProbability("99215");
                }
                //else
                //{
                //    _EMCode.increaseProbability("99211");
                //}
                //if (History.HPI_Count == 0)
                //{
                //    _EMCode.increaseProbability("99211");
                //}
                if (History.HPI_Count > 0 && History.HPI_Count < 4)
                {
                    _EMCode.increaseProbability("99212");
                    _EMCode.increaseProbability("99213");
                }
                else if (History.HPI_Count > 3)
                {
                    _EMCode.increaseProbability("99214");
                    _EMCode.increaseProbability("99215");
                }
                // ROS
                //if (History.ROS_Count == 0)
                //{
                //    _EMCode.increaseProbability("99211");
                //    _EMCode.increaseProbability("99212");
                //}
                if (History.ROS_Count == 1)
                {
                    _EMCode.increaseProbability("99213");
                }
                else if (History.ROS_Count > 1 && History.ROS_Count < 10)
                {
                    _EMCode.increaseProbability("99214");
                }
                else if (History.ROS_Count > 9)
                {
                    _EMCode.increaseProbability("99215");
                }
                // Previous History
                //if (History.Hx_Count == 0)
                //{
                //    _EMCode.increaseProbability("99211");
                //    _EMCode.increaseProbability("99212");
                //    _EMCode.increaseProbability("99213");
                //}
                if (History.Hx_Count >= 1 && History.Hx_Count < 3)
                {
                    _EMCode.increaseProbability("99214");
                }
                else if (History.Hx_Count > 2)
                {
                    if (History.FamilyHx == 1 && History.MedicalHx == 1 && History.SocialHx == 1)
                    {
                        _EMCode.increaseProbability("99215");
                    }
                    else
                    {
                        _EMCode.increaseProbability("99214");
                    }
                }
                #endregion
                #region Exam
                int NoOfSystems = Exam.Count;
                if (NoOfSystems > 0)
                {
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }
                    if (NoOfSystems == 1)     // Single Organ System
                    {
                        foreach (var elem in Exam)
                        {
                            if (elem.NoOfElements >= elem.TotalNoOfElementsInSystem)
                            {
                                _EMCode.increaseProbability("99215");
                            }

                            if (elem.NoOfElements > 0 && elem.NoOfElements < 6)
                            {
                                _EMCode.increaseProbability("99212");
                            }
                            else if (elem.NoOfElements >= 6 && elem.NoOfElements < 12)
                            {
                                _EMCode.increaseProbability("99213");
                            }
                            else if (elem.NoOfElements > 11)
                            {
                                _EMCode.increaseProbability("99214");
                            }
                        }
                    }
                    else if (NoOfSystems > 1) // Multi Organ System
                    {
                        int elementCounter = 0;
                        bool TwoElementsInEachSystem = false;
                        foreach (var elem in Exam)
                        {
                            if (elem.NoOfElements >= 2)  // Check if all systems have 2 elements
                            {
                                elementCounter++;
                            }
                        }
                        if (elementCounter == Exam.Count)
                        {
                            TwoElementsInEachSystem = true;
                        }
                        if (TwoElementsInEachSystem == true)
                        {
                            if (NoOfSystems > 5 && NoOfSystems < 10)
                            {
                                _EMCode.increaseProbability("99214");
                            }
                            else if (NoOfSystems > 8)
                            {
                                _EMCode.increaseProbability("99215");
                            }
                            else if (NoOfSystems < 6)
                            {
                                if (totalElementsInAllSystems > 0 && totalElementsInAllSystems < 6)
                                {
                                    _EMCode.increaseProbability("99212");
                                }
                                else if (totalElementsInAllSystems > 5 && totalElementsInAllSystems < 12)
                                {
                                    _EMCode.increaseProbability("99213");
                                }
                                else if (totalElementsInAllSystems > 12)
                                {
                                    _EMCode.increaseProbability("99214");
                                }
                            }
                        }
                        else
                        {
                            if (totalElementsInAllSystems > 0 && totalElementsInAllSystems < 6)
                            {
                                _EMCode.increaseProbability("99212");
                            }
                            else if (totalElementsInAllSystems > 5 && totalElementsInAllSystems < 12)
                            {
                                _EMCode.increaseProbability("99213");
                            }
                            else if (totalElementsInAllSystems > 12)
                            {
                                _EMCode.increaseProbability("99214");
                            }
                        }
                    }
                }
                //else
                //{
                //    _EMCode.increaseProbability("99211");
                //}
                #endregion

                string Code = _EMCode.getEstablishedPatientCodeSuggestion();
                string suggestions = SuggestNextEstablishedPatientEMCode(Code);
                var response = new
                {
                    status = true,
                    SuggestedCode = Code,
                    NextSuggestions = suggestions
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string SuggestNextNewPatientEMCode(string PreviousCode)
        {
            List<EMCodeGeneratorExamModel> Exam = this.DataHolder._EMCodeExam;
            EMCodeGeneratorHistoryModel History = this.DataHolder._EMCodeHistory;
            EMCodeGeneratorMDMModel MDM = this.DataHolder._EMCodeMDM;
            string suggestions = "";
            int NoOfSystems = Exam.Count;
            if (PreviousCode == "99201")
            {
                if (History.IsChiefComplaint != true) {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 1 HPI element ;";
                }
                if (History.ROS_Count == 0)
                {
                    suggestions += "Add one more system in ROS ;";
                }
                if (NoOfSystems > 0)
                {
                    int remainingElements = 0;
                    int totalElementsInAllSystems = 0;
                        foreach (var elem in Exam)
                        {
                            totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                        }
                        if ((remainingElements = 6 - totalElementsInAllSystems) > 0)
                        {
                            suggestions += "Add at least " + remainingElements + " more characteristics from one or more system(s) in physical exam ;";
                        }
                }
                else
                {
                    suggestions += "At least six characteristic from any system(s) of physical exam ;";
                }
                if (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Lab order/Lab result / Radiology order/ Radiology Result ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "Diagnosis Code";
                } 
            }
            else if (PreviousCode == "99202")
            {
                if (History.IsChiefComplaint != true)
                {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 4 HPI elements ;";
                }
                else if (History.HPI_Count > 0 && History.HPI_Count < 4)
                {
                    long remainingHPI = 4 - History.HPI_Count;
                    suggestions += "Record at least " + remainingHPI + " more HPI elements ;";
                }
                if (History.ROS_Count == 0)
                {
                    suggestions += "Add two more systems in ROS ;";
                }
                else if (History.ROS_Count == 1)
                {
                    suggestions += "Add one more system in ROS ;";
                }
                if (History.Hx_Count == 0)
                {
                    suggestions += "Add one element from family/social/medical history ;";
                }
                if (NoOfSystems > 0)
                {
                    int remainingElements = 0;
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }
                    if (((remainingElements = 12 - totalElementsInAllSystems) > 0) && NoOfSystems == 1)
                    {
                        suggestions += "Add at least " + remainingElements + " more characteristics from two or more system(s) in physical exam ;";
                    }
                    else if (((remainingElements = 6 - Exam.Count) > 0) && (NoOfSystems > 1)) // 6 systems
                    {
                        int elementCounter = 0;
                        bool TwoElementsInEachSystem = false;
                        foreach (var elem in Exam)
                        {
                            if (elem.NoOfElements >= 2)  // Check if all systems have 2 elements
                            {
                                elementCounter++;
                            }
                        }
                        if (elementCounter == Exam.Count)
                        {
                            TwoElementsInEachSystem = true;
                        }
                        if (TwoElementsInEachSystem != true)
                        {
                            suggestions += "Add at least " + remainingElements + " systems in physical exam and record two elements in each ;";
                        }
                    }
                    else if (totalElementsInAllSystems < 12 && (NoOfSystems > 1))
                    {
                        remainingElements = 12 - totalElementsInAllSystems;
                        suggestions += "Add at least " + remainingElements + " systems in physical exam and record two elements in each ;";
                    }
                }
                else
                {
                    suggestions += "Add At least 12 elements in one or more systems OR at least 6 systems and 2 elements from each ;";
                }
                if (MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Radiology order/ Radiology Result ;";
                }
                else if(MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == true)
                {
                    suggestions += "A Lab order/ Lab Result ;";
                }
                else if (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Lab order/ Lab Result and Radiology order/ Radiology Result ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "2 Diagnosis Codes ;";
                }
                else if (MDM.NoOfICD == 1)
                {
                    suggestions += "1 Diagnosis Code ;";
                }
            }
            else if (PreviousCode == "99203")
            {
                if (History.IsChiefComplaint != true)
                {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 4 HPI elements ;";
                }
                else if (History.HPI_Count > 0 && History.HPI_Count < 4)
                {
                    long remainingHPI = 4 - History.HPI_Count;
                    suggestions += "Record at least " + remainingHPI + " more HPI elements ;";
                }
                long RosElements = 0;
                if ((RosElements = 10 - History.ROS_Count) > 0)
                {
                    suggestions += "Add " + RosElements + " more system in ROS ;";
                }
                if (History.Hx_Count == 0)
                {
                    suggestions += "Add one element from family, social and medical history ;";
                }
                else if (History.FamilyHx != 1 && History.SocialHx != 1 && History.MedicalHx != 1)
                {
                    if (History.FamilyHx != 1)
                    {
                        suggestions += "Add one more element from family history ;";
                    }
                    else if (History.SocialHx != 1)
                    {
                        suggestions += "Add one more element from social history ;";
                    }
                    else
                    {
                        suggestions += "Add one more element from medical history ;";
                    }
                }
                if (NoOfSystems > 1)
                {
                    int remainingElements = 0;
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }
                    
                    if (((remainingElements = 9 - Exam.Count) > 0) && (NoOfSystems > 1))
                    {
                        //int elementCounter = 0;
                        //bool TwoElementsInEachSystem = false;
                        //foreach (var elem in Exam)
                        //{
                        //    if (elem.NoOfElements >= 2)  // Check if all systems have 2 elements
                        //    {
                        //        elementCounter++;
                        //    }
                        //}
                        //if (elementCounter == Exam.Count)
                        //{
                        //    TwoElementsInEachSystem = true;
                        //}
                        //if (TwoElementsInEachSystem == true && remainingElements != 9)
                        //{
                            suggestions += "Add at least " + remainingElements + " systems in physical exam and record two elements in each ;";
                      //  }
                    }
                }
                else if (NoOfSystems == 1)
                {
                    int counter = 0;
                    foreach(var elem in Exam)
                    {
                        if (elem.TotalNoOfElementsInSystem >= elem.NoOfElements)
                        {
                            counter++;
                        }
                        if (counter != Exam.Count)
                        {
                            suggestions += "Include all elements of this system in physical exam ;";
                        }
                    }
                }
                else
                {
                    suggestions += "Include all elements of at least one system OR Add 9 systems and record two elements from each in physical exam ;";
                }
                if (MDM.HasLabOrderOrResult == false && MDM.HasLabResultRemarksOrComments == false && MDM.HasRadiologyOrderOrResult == false && MDM.HasRadiologyResultRemarksOrComments == false)
                {
                    suggestions += "A Lab order/ Lab Result or Radiology order/ Radiology Result remarks or comments ;";
                }
                else if (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == true && MDM.HasRadiologyResultRemarksOrComments != true)
                {
                    suggestions += "A Radiology order/ Radiology Result remarks or comments;";
                }
                else if (MDM.HasLabOrderOrResult == true && MDM.HasLabResultRemarksOrComments == false && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Lab order/ Lab Result remarks or comments ;";
                }
                else if (MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true && MDM.HasRadiologyResultRemarksOrComments == false && MDM.HasLabResultRemarksOrComments == false)
                {
                    suggestions += "A Lab order/ Lab Result or Radiology order/ Radiology Result remarks or comments ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "3 Diagnosis Codes ;";
                }
                else 
                {
                    int NoOfCodes = 0;
                    if ((NoOfCodes = 3 - MDM.NoOfICD) > 0)
                    {
                        if (NoOfCodes == 1)
                        {
                            suggestions += NoOfCodes + " Diagnosis Code ;";
                        }
                        else
                        {
                            suggestions += NoOfCodes + " Diagnosis Codes ;";
                        }
                    }
                }
            }
            else if (PreviousCode == "99204")
            {
                if (History.IsChiefComplaint != true)
                {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 4 HPI elements ;";
                }
                else if (History.HPI_Count > 0 && History.HPI_Count < 4)
                {
                    long remainingHPI = 4 - History.HPI_Count;
                    suggestions += "Record at least " + remainingHPI + " more HPI elements ;";
                }
                long RosElements = 0;
                if ((RosElements = 10 - History.ROS_Count) > 0)
                {
                    suggestions += "Add " + RosElements + " more systems in ROS ;";
                }
           
                if (History.Hx_Count == 0)
                {
                    suggestions += "Add one element from family, social and medical history ;";
                }
                else if (History.FamilyHx != 1 && History.SocialHx != 1 && History.MedicalHx != 1)
                {
                    if (History.FamilyHx != 1)
                    {
                        suggestions += "Add one more element from family history ;";
                    }
                    else if (History.SocialHx != 1)
                    {
                        suggestions += "Add one more element from social history ;";
                    }
                    else
                    {
                        suggestions += "Add one more element from medical history ;";
                    }
                }
                if (NoOfSystems > 1)
                {
                    int remainingElements = 0;
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }

                    if (((remainingElements = 9 - Exam.Count) > 0) && (NoOfSystems > 1))
                    {
                       // int elementCounter = 0;
                        //bool TwoElementsInEachSystem = false;
                        //foreach (var elem in Exam)
                        //{
                        //    if (elem.NoOfElements >= 2)  // Check if all systems have 2 elements
                        //    {
                        //        elementCounter++;
                        //    }
                        //}
                        //if (elementCounter == Exam.Count)
                        //{
                        //    TwoElementsInEachSystem = true;
                        //}
                        //if (TwoElementsInEachSystem == true && remainingElements != 9)
                            suggestions += "Add at least " + remainingElements + " systems in physical exam and record two elements in each ;";
                    }
                }
                else if (NoOfSystems == 1)
                {
                    int counter = 0;
                    foreach (var elem in Exam)
                    {
                        if (elem.TotalNoOfElementsInSystem >= elem.NoOfElements)
                        {
                            counter++;
                        }
                        if (counter != Exam.Count)
                        {
                            suggestions += "Include all elements of this system in physical exam ;";
                        }
                    }
                }
                else
                {
                    suggestions += "Include all elements of at least one system OR Add 9 systems and record two elements from each in physical exam ;";
                }
                if ((MDM.HasLabOrderOrResult == false && MDM.HasLabResultRemarksOrComments == false ) && (MDM.HasRadiologyOrderOrResult == false && MDM.HasRadiologyResultRemarksOrComments == false))
                {
                    suggestions += "A Lab order/ Lab Result and Radiology order/ Radiology Result remarks and comments ;";
                }
                else if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true) && (MDM.HasLabResultRemarksOrComments == false || MDM.HasRadiologyResultRemarksOrComments == false))
                {
                    suggestions += "A Lab order/ Lab Result and Radiology order/ Radiology Result remarks and comments ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "4 Diagnosis Codes ;";
                }
                else
                {
                    int NoOfCodes = 0;
                    if ((NoOfCodes = 4 - MDM.NoOfICD) > 0)
                    {
                        if (NoOfCodes == 1)
                        {
                            suggestions += NoOfCodes + " Diagnosis Code ;";
                        }
                        else
                        {
                            suggestions += NoOfCodes + " Diagnosis Codes ;";
                        }
                    }
                }
            }
            if (suggestions == "")
            {
                suggestions += "There are currently no suggestions to show. ;";
            }
            return suggestions;
        }
        public string SuggestNextEstablishedPatientEMCode(string PreviousCode)
        {
            List<EMCodeGeneratorExamModel> Exam = this.DataHolder._EMCodeExam;
            EMCodeGeneratorHistoryModel History = this.DataHolder._EMCodeHistory;
            EMCodeGeneratorMDMModel MDM = this.DataHolder._EMCodeMDM;
            string suggestions = "";
            int NoOfSystems = Exam.Count;
            if (PreviousCode == "99211")
            {
                if (History.IsChiefComplaint != true)
                {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 1 HPI element ;";
                }
                if (NoOfSystems > 0)
                {
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }
                    if (totalElementsInAllSystems == 0)
                    {
                        suggestions += "Add at least element from one or more system(s) in physical exam ;";
                    }
                }
                else
                {
                    suggestions += "Add at least one element from one or more system(s) in physical exam ;";
                }
                if (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Lab order/Lab result / Radiology order/ Radiology Result ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "Diagnosis Code";
                }
            }
            else if (PreviousCode == "99212")
            {
                if (History.IsChiefComplaint != true)
                {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 4 HPI elements ;";
                }
                if (History.ROS_Count == 0)
                {
                    suggestions += "Add one more system in ROS ;";
                }
           
                if (NoOfSystems > 0)
                {
                    int remainingElements = 0;
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }
                    if ((remainingElements = 6 - totalElementsInAllSystems) > 0) // 6 systems
                    {
                         suggestions += "Add at least " + remainingElements + " more elements in one or more systems in physical exam and record two elements in each ;";   
                    }
                }
                else
                {
                    suggestions += "Add at least 6 elements in one or more systems of physical exam. ;";
                }
                if (MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Radiology order/ Radiology Result ;";
                }
                else if (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == true)
                {
                    suggestions += "A Lab order/ Lab Result ;";
                }
                else if (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Lab order/ Lab Result and Radiology order/ Radiology Result ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "2 Diagnosis Codes ;";
                }
                else if (MDM.NoOfICD == 1)
                {
                    suggestions += "1 Diagnosis Code ;";
                }

            }
            else if (PreviousCode == "99213")
            {
                if (History.IsChiefComplaint != true)
                {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 4 HPI elements ;";
                }
                else if (History.HPI_Count > 0 && History.HPI_Count < 4)
                {
                    long remainingHPI = 4 - History.HPI_Count;
                    suggestions += "Record at least " + remainingHPI + " more HPI elements ;";
                }
                long RosElements = 0;
                if ((RosElements = 2 - History.ROS_Count) > 0)
                {
                    suggestions += "Add at least" + RosElements + " more systems in ROS ;";
                }
                if (History.Hx_Count == 0)
                {
                    suggestions += "Add one element from family / social / medical history ;";
                }

                if (NoOfSystems > 0)
                {
                    int remainingElements = 0;
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }
                    if (((remainingElements = 12 - totalElementsInAllSystems) > 0) && NoOfSystems == 1)
                    {
                        suggestions += "Add at least " + remainingElements + " more characteristics from this system(s) in physical exam ;";
                    }
                    else if ((remainingElements = 6 - Exam.Count) > 0) // 6 systems
                    {
                        //int elementCounter = 0;
                        //bool TwoElementsInEachSystem = false;
                        //foreach (var elem in Exam)
                        //{
                        //    if (elem.NoOfElements >= 2)  // Check if all systems have 2 elements
                        //    {
                        //        elementCounter++;
                        //    }
                        //}
                        //if (elementCounter == Exam.Count)
                        //{
                        //    TwoElementsInEachSystem = true;
                        //}
                        //if (TwoElementsInEachSystem != true)
                        //{
                            suggestions += "Add at least " + remainingElements + " systems in physical exam and record two elements in each ;";
                      //  }
                    }
                }
                else
                {
                    suggestions += "At least 12 elements from two or more systems OR at least 6 systems and two elements from each. ;";
                }
                if (MDM.HasLabOrderOrResult == false && MDM.HasLabResultRemarksOrComments == false && MDM.HasRadiologyOrderOrResult == false && MDM.HasRadiologyResultRemarksOrComments == false)
                {
                    suggestions += "A Lab order/ Lab Result or Radiology order/ Radiology Result remarks or comments ;";
                }
                else if (MDM.HasLabOrderOrResult == false && MDM.HasRadiologyOrderOrResult == true && MDM.HasRadiologyResultRemarksOrComments != true)
                {
                    suggestions += "A Radiology order/ Radiology Result remarks or comments;";
                }
                else if (MDM.HasLabOrderOrResult == true && MDM.HasLabResultRemarksOrComments == false && MDM.HasRadiologyOrderOrResult == false)
                {
                    suggestions += "A Lab order/ Lab Result remarks or comments ;";
                }
                else if (MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true && MDM.HasRadiologyResultRemarksOrComments == false && MDM.HasLabResultRemarksOrComments == false)
                {
                    suggestions += "A Lab order/ Lab Result and Radiology order/ Radiology Result remarks or comments ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "3 Diagnosis Codes ;";
                }
                else
                {
                    int NoOfCodes = 0;
                    if ((NoOfCodes = 3 - MDM.NoOfICD) > 0)
                    {
                        if (NoOfCodes == 1)
                        {
                            suggestions += NoOfCodes + " Diagnosis Code ;";
                        }
                        else
                        {
                            suggestions += NoOfCodes + " Diagnosis Codes ;";
                        }
                    }
                }
            }
            else if (PreviousCode == "99214")
            {
                if (History.IsChiefComplaint != true)
                {
                    suggestions += "Add Chief Complaint ;";
                }
                if (History.HPI_Count == 0)
                {
                    suggestions += "Record at least 4 HPI elements ;";
                }
                else if (History.HPI_Count > 0 && History.HPI_Count < 4)
                {
                    long remainingHPI = 4 - History.HPI_Count;
                    suggestions += "Record at least " + remainingHPI + " more HPI elements ;";
                }
                long RosElements = 0;
                if ((RosElements = 10 - History.ROS_Count) > 0)
                {
                    suggestions += "Add " + RosElements + " more systems in ROS ;";
                }
                if (History.Hx_Count == 0)
                {
                    suggestions += "Add one element from family, social and medical history ;";
                }
                else if (History.FamilyHx != 1 && History.SocialHx != 1 && History.MedicalHx != 1)
                {
                    if (History.FamilyHx != 1)
                    {
                        suggestions += "Add one more element from family history ;";
                    }
                    else if (History.SocialHx != 1)
                    {
                        suggestions += "Add one more element from social history ;";
                    }
                    else
                    {
                        suggestions += "Add one more element from medical history ;";
                    }
                }
                if (NoOfSystems > 1)
                {
                    int remainingElements = 0;
                    int totalElementsInAllSystems = 0;
                    foreach (var elem in Exam)
                    {
                        totalElementsInAllSystems = totalElementsInAllSystems + elem.NoOfElements;
                    }

                    if (((remainingElements = 9 - Exam.Count) > 0) && (NoOfSystems > 1))
                    {
                        // int elementCounter = 0;
                        //bool TwoElementsInEachSystem = false;
                        //foreach (var elem in Exam)
                        //{
                        //    if (elem.NoOfElements >= 2)  // Check if all systems have 2 elements
                        //    {
                        //        elementCounter++;
                        //    }
                        //}
                        //if (elementCounter == Exam.Count)
                        //{
                        //    TwoElementsInEachSystem = true;
                        //}
                        //if (TwoElementsInEachSystem == true && remainingElements != 9)
                        suggestions += "Add at least " + remainingElements + " systems in physical exam and record two elements in each ;";
                    }
                }
                else if (NoOfSystems == 1)
                {
                    int counter = 0;
                    foreach (var elem in Exam)
                    {
                        if (elem.TotalNoOfElementsInSystem == elem.NoOfElements)
                        {
                            counter++;
                        }
                        if (counter != Exam.Count)
                        {
                            suggestions += "Include all elements of this system in physical exam ;";
                        }
                    }
                }
                else
                {
                    suggestions += "Include all elements of at least one system OR Add 9 systems and record two elements from each in physical exam ;";
                }
                if ((MDM.HasLabOrderOrResult == false && MDM.HasLabResultRemarksOrComments == false) && (MDM.HasRadiologyOrderOrResult == false && MDM.HasRadiologyResultRemarksOrComments == false))
                {
                    suggestions += "A Lab order/ Lab Result and Radiology order/ Radiology Result remarks and comments ;";
                }
                else if ((MDM.HasLabOrderOrResult == true && MDM.HasRadiologyOrderOrResult == true ) && (MDM.HasLabResultRemarksOrComments == false || MDM.HasRadiologyResultRemarksOrComments == false))
                {
                    suggestions += "A Lab order/ Lab Result and Radiology order/ Radiology Result remarks and comments ;";
                }
                if (MDM.NoOfICD <= 0)
                {
                    suggestions += "4 Diagnosis Codes ;";
                }
                else
                {
                    int NoOfCodes = 0;
                    if ((NoOfCodes = 4 - MDM.NoOfICD) > 0)
                    {
                        if (NoOfCodes == 1)
                        {
                            suggestions += NoOfCodes + " Diagnosis Code ;";
                        }
                        else
                        {
                            suggestions += NoOfCodes + " Diagnosis Codes ;";
                        }
                    }
                }
            }
            if (suggestions == "")
            {
                suggestions += "There are currently no suggestions to show. ;";
            }
            return suggestions;
        }

    }
}