var StaticLookups = {
    GetGenderDemographic: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
              '{ "Name": "Male", "Value": "0", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
              '{ "Name": "Female", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
              '{ "Name": "Unknown", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetBirthSex: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
             '{ "Name": "Male", "Value": "Male", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
             '{ "Name": "Female", "Value": "Female", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
             '{ "Name": "Unknown", "Value": "Unknown", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetFollowUpCallStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Completed", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Left Message", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Answered", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Not Answered", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    //GetERAPayee: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Sovereign Medical Group LLC - New", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Sovereign Health Imaging LLC", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetERAStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Posted", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "UnPosted", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Partially Posted", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Manual Posted", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetAutoAction: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Claim Status – Plan", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Deductible – Patient", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Discount – Patient", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "No Patient Statement – Patient", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Patient Responsibility – Plan", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Resubmit – Plan", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Secondary Transfer  - Plan", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Write off – Plan", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Yes Patient Statement – Patient", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    getAllergyType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Drug Allergy", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Food Allergy", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Environment Allergy", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Others", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    getCDSQuestionnaireControlType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Text Area", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Text Box", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Yes No", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Yes No with note", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "DropDown", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    getCDSRecursivePeriod: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Day(s)", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Month(s)", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Year(s)", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    // fixme DUPLICATE
    getCDSReminderPeriod: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Day(s)", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Month(s)", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Year(s)", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',



    // GetChronicityLevel: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Chronic", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Acute", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Self-Limiting", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Intermittent", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Recurrent", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Distressful", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetCase: '[{ "Name": "- Select -", "Value": "0", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Normal", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Abnormal", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetDuration: '[{ "Name": "- Select -", "Value": "0", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Minutes", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Hours", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Days", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Weeks", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Months", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Years", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetFrequency: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Hourly", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Daily", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Weekly", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Monthly", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Quarterly", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Yearly", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    getClinicalLabOrderDiet: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Normal", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Fasting", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not Given", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Fasting Not Asked", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',



    GetFamilyHxFamilyMember: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Mother", "Value": "1", "RefValue": "72705000", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Father", "Value": "2", "RefValue": "66839005", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sister", "Value": "3", "RefValue": "27733009", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Brother", "Value": "4", "RefValue": "70924004", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Daughter", "Value": "5", "RefValue": "83420006", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Son", "Value": "6", "RefValue": "444241008", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Aunt", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Uncle", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Aunt", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Uncle", "Value": "10", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Grand Mother", "Value": "11", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Grand Father", "Value": "12", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Grand Mother", "Value": "13", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Grand Father", "Value": "14", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Great Aunt", "Value": "15", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Great Uncle", "Value": "16", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Great Aunt", "Value": "17", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Great Uncle", "Value": "18", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "19", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Child", "Value": "20", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "First Degree Relative", "Value": "21", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Great Grandmother", "Value": "22", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Great Grandfather", "Value": "23", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Great Grandmother", "Value": "24", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Great Grandfather", "Value": "25", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Maternal Relative", "Value": "26", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Paternal Relative", "Value": "27", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown Relative", "Value": "28", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Other", "Value": "29", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetFamilyHxHealthStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Alive", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Deceased", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetHospitalizationHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Completed", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Active", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Aborted", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Cancelled", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Scheduled", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetHospitalizationHxStay: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Days", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Weeks", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Months", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Years", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetImmunizationAlerts: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Normal", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Overdue", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Due", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "No Alert", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetLabCategory: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "In-House", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Send-Out", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetLabCodeSystem: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "CPT Codes", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Custom Codes", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetLabType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Radiology", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Diagnostic", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Pathology", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetMedicalHxDurationPeriod: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Minutes", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Hours", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Days", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Weeks", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Months", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Years", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetMedicalHxPattern: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Intermittent", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Persistent", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Episodic", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetMedicalHxSeverity: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Mild", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Moderate", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Severe", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',



    GetMedicalHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Active", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Inactive", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "In progress", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Complete", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Rolled out", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Resolved", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Controlled", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "N/A", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetMedicalHxTestResults: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Normal", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Abnormal", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "N/A", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetNoteStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Draft", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Signed", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    //[sp_PQRSReasonTypeLookup] 
    GetPQRSReasonType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Patient Reason", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Medical Reason", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "System Reason", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetPQRSTreatmentType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Medication", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Vaccination", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Procedure", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Lab Test", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    // [sp_QuestionTypelookup] 
    GetQuestionType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Text Field", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Radio Button", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Drop Down", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Multi Select Drop Down", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Fraction Field", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Date", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Time", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Date & Time", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Image Field", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Number Field", "Value": "10", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    //sp_SeverityLookup
    getSeverity: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Mild", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Moderate", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sever", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "UnKnown", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    //GetSeverityType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Unspecified Severity", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Severe Persistent", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Moderate Persistent", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Mild Persistent", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Mild Intermittent", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    //[sp_SocialHx_Alcohol_FrequencySelectLookup] 
    GetAlcoholFrequency: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Never", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Monthly or less", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "2 – 4 times a month", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "2 – 3 times a week", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "4+ times a week", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    //[sp_SocialHx_Alcohol_StatusSelectLookup] 
    GetAlcoholStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Does not drink", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Drinks daily", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Frequently drinks", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Has history of Alcoholism", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Occasional drinker", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Social drinker", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Drinking status unknown", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Denies Usage", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetAlcoholType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Beer", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Liquor", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Wine", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Whisky", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    //fixme DUPLICATE
    GetSocialHxCessationPeriod: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Days", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Weeks", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Months", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Years", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetSocialHxCounsellingPeriod: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Yes, less than 3 minutes", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Yes, 3 – 10 minutes", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Yes, More than 10 minutes", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Did not Counsel", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetDrugAbuseFrequencyMonthly: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Never", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Monthly or less", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "2 – 4 times a month", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "2 – 3 times a week", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "4+ times a week", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetDrugAbuseRoute: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Illicit", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Non – illicit", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Oral", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Intraveneous", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Topical", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Inhalation", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Injection", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    getCaffeineIntakHxFrequency: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "1-2 cups per day", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "2-3 cups per day", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "3-4 cups per day", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "More than 4 cups per day", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    getCaffeineIntakeHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Normally", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Abnormal", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Does not use", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    getExercisesHxDiet: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Normally", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Abnormal", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Does not use", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    getExercisesHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Daily", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Occasional", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Often", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "1-2 times in a week", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "3-4 times in a week", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Does not exercise", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    getExercisesHxType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Light", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Moderate", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Heavy", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Vigorous", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    getHousingHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Homeless", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Shelter", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Renting", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Owns", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Living with others", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    getOccupationStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unemployed", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Works at home", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Works part time", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Works full time", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Office worker", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Professional", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Manual worker", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    getSleepHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Normally", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Fitfully", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Less than 4 hours a night", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "More than 7 hours a night", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not normally", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sleeping is a problem for patient", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetSocialHxPreferences: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Men only", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Women only", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Both Men and Women", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetSexualHxProtectionMethod: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Abstinence", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Condoms", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Women Condoms", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "others", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetSexualHxProtectionPeriod: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "All of the time", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Most of the time", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Half of the time", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Some of the time", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetSexualHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sexually Active", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Practices safe sex", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not Sexually Active", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetSexualHxSTD: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Chlamydia", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "GC", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Syphilis", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Herpes", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Others", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetTobaccoCounsellingTopic: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Tobacco free home and car", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Changing daily routine", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Dealing with urges to smoke", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Getting support", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Anticipating/Avoiding triggers", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Secondhand Smoke", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Teach behavioral skills", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Reinforce benefits", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetTobaccoFrequency: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "5 or less", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "6 – 10 (Half a pack a day)", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "11 – 20 (Pack a day)", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "21 – 30 (More than a pack a day)", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "More than 31 (2+ packs a day)", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetTobaccoType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Cigarettes", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Cigars", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Pipe", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Electronic Cigarette", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Chewing Tobacco", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Snuff", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Smokeless Tobacco", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetSurgicalHxStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Successful", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unsuccessful", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetVaccineReaction: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Allergy", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Anaphylaxis", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Anxiety", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Constipation", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Constriction", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Euphoria", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Fainting Goat", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Happiness", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Hives", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Nausea", "Value": "10", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Pennisullin", "Value": "11", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Rash", "Value": "12", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Urticaria", "Value": "13", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',



    // GetVaccineRefusalReason: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetRegistryStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Active", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Inactive--Unspecified", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Inactive-Lost to follow-up (cannot contact)", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Inactive-Moved or gone elsewhere (transferred)", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Inactive-Permanently inactive", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


   // GetVaccineRoute: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Intradermal", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Intramuscular", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Nasal", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Intravenous", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Oral", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Other/Miscellaneous", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Subcutaneous", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Transdermal", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',



    GetVaccineSite: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Left Thigh", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Left Arm", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Left Deltoid", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Left Gluteous Medius", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Left Vastus Lateralis", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Left Lower Forearm", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Right Arm", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Right Thigh", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Right Vastus Lateralis", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Right Gluteous Medius", "Value": "10", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Right Deltoid", "Value": "11", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Right Lower Forearm", "Value": "12", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },'+
   '{ "Name": "Left Buttock", "Value": "13", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Right Buttock", "Value": "14", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',
  

   
    GetVaccineSourceOfHx: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "New immunization record ", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - source unspecified ", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - from other provider ", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - from parent\'s written record ", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - from parent\'s recall", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - from other registry ", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - from birth certificate ", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - from school record", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Historical information - from public agency ", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetVaccineVFC: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not VFC eligible", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "VFC eligible-Medicaid/Medicaid Managed Care", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "VFC eligible- Uninsured", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "VFC eligible- American Indian/Alaskan Native", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "VFC eligible-Federally Qualified Health Center Patient (under-insured)", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Deprecated [VFC eligible- State specific eligibility (e.g. S-CHIP plan)]", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Local-specific eligibility", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Deprecated [Not VFC eligible-underinsured]", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetVolume: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Bottle", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Syringe", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    // GetARType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Insurance", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Patient", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Provider", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Attorney", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    //'{ "Name": "Facility", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetAmendmentSource: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Provider", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Patient", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetChargeBatchStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not Started", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "In progress", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Completed", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Pending", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Information Required", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetCommunication: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Letter", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Phone", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Portal", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Text", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetCreditCardType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "VISA", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "MASTER", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "DISCOVER", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "AMEX", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetEthnicity: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Hispanic or Latino", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not Hispanic or Latino", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Refused to Report", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetIsuranceType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Primary", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Secondary", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Tertiary", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetMaritalStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Divorced", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Married", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Partner", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Single", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Widowed", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Legally Separated", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetMsgPriority: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "High", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Medium", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Low", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetMessageStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Resolved", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unresolved", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetPatientType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "New Patient", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Established Patient", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetPaymentType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Cash", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Check", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Credit Card", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Advance Payment", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetPreferredTime: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Any Time", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Morning", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "AfterNoon", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Evening", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetPrefix: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Dr", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Mr", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Mrs", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Ms", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Miss", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sir", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetRace: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "American Indian or Alaska Native", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Asian", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Black or African American", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Native Hawaiian or Other Pacific Islander", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "White", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Hispanic", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Other Race", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Other Pacific Islander", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unreported/Refused to Report", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Declined to Specify", "Value": "10", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetRelationship: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Spouse", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Grandfather or Grandmother", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Grandson or Granddaughter", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Nephew or Niece", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Foster child", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Ward", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Stepson or Stepdaughter", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Self", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Child", "Value": "9", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Employee", "Value": "10", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown", "Value": "11", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Handicapped Dependent", "Value": "12", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sponsored Dependent", "Value": "13", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Dependent of a minor dependent", "Value": "14", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Significant Other", "Value": "15", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Mother", "Value": "16", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Father", "Value": "17", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Emancipated Minor", "Value": "18", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Organ Donor", "Value": "19", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Cadaver Donor", "Value": "20", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Injured Plaintiff", "Value": "21", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Child where insured has no financial responsibility", "Value": "22", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Life Partner", "Value": "23", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Other Relationship", "Value": "24", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Brother", "Value": "25", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sister", "Value": "26", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Caregiver", "Value": "27", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   //'{ "Name": "Caregiver", "Value": "28", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Uncle", "Value": "29", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },'+
   '{ "Name": "Guardian", "Value": "30", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',


    GetSchoolStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Part Time", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Full Time", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not a Student", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetSmokersStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Current every day smoker", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Current some day smoker", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Former smoker", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Never smoker", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Smoker, current status unknown", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Unknown if ever smoked", "Value": "6", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Heavy tobacco smoker", "Value": "7", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Light tobacco smoker", "Value": "8", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetSuffix: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "II", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "III", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "IV", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Jr", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Sr", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetWaitListStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Canceled By Patient", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Not Available", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Booked", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Waiting", "Value": "4", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Canceled", "Value": "5", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetGender: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
              '{ "Name": "Male", "Value": "0", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
              '{ "Name": "Female", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
              '{ "Name": "Unknown", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetPreferredCommunicationMode: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Phone", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Text", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Email", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetPreferredPrimaryContact: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Home Phone", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Cell Phone", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Work Phone", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetLabFavoriteListType: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Lab Order", "Value": "LabOrder", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Lab Order Group", "Value": "LabOrderGroup", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetBillingInfoType: '[{"Name":"- Select -","Value":"","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"E/M Services New Pt","Value":"1","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"E/M Services Est Pt","Value":"2","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"After Hours","Value":"3","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Postoperative Care","Value":"4","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Consultations","Value":"5","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Work Related or Medical Disability Evaluation","Value":"6","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Basic Life and/or Disability Evaluation Services (Life Insurance)","Value":"7","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Transitional Care Mgmt. Services","Value":"8","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Medical Nutrition Therapy","Value":"9","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Eye Codes - Est Pt","Value":"10","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Eye Codes - New Pt","Value":"11","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Hospital/Inpatient","Value":"12","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Home Visit (House Call) Established Patient","Value":"13","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Home Visit (House Call) New Patient","Value":"14","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Nursing Home - Initial","Value":"15","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Nursing Home - Subsequent","Value":"16","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Preventive Medicine - Est Pt","Value":"17","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Annual Wellness Visit (Medicare)","Value":"18","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Preventive Medicine - New Pt","Value":"19","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Psychiatry - Group Psychotherapy","Value":"20","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Hospital Observation","Value":"21","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Observation or Inpatient Care","Value":"22","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Hospital Discharge Services","Value":"23","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Emergency Dept. (New or Est.Pt)","Value":"24","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Other Emergency Services","Value":"25","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Critical Care","Value":"26","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Domiciliary, Rest Home or Custodial Care (Boarding Home) also Assisted Living","Value":"27","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"New Patient","Value":"28","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Established Patient","Value":"29","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Domiciliary, Rest Home (Assisted Living Facility), or Home Care Plan Oversight Services","Value":"30","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Prolonged Services (Direct Patient Contact)","Value":"31","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Prolonged Service (Without Direct Patient Contact)","Value":"32","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Care Plan Oversight","Value":"33","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Advance Care Planning","Value":"34","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Chronic Care Mgmt","Value":"35","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Complex Chronic Care Mgmt","Value":"36","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Counseling Risk Factor Reduction and Behavior Change Intervention","Value":"37","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Medicare HCPCS Codes","Value":"38","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""}]',

    GetRadiologyReason: '[{"Name":"- Select -","Value":"","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"coronary artery disease","Value":"1","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"hypertension","Value":"2","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"joint pain","Value":"3","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"LBP","Value":"4","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"monitoring medication","Value":"5","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"rheumatoid arthritis","Value":"6","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"screening","Value":"7","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""}]',
    GetResultRadiologyResult: '[{"Name":"- Select -","Value":"","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Abnormal","Value":"Abnormal","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Critical High","Value":"Critical High","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Critical Low","Value":"Critical Low","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"High","Value":"High","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Low","Value":"Low","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Negative","Value":"Negative","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Osteoarthritis","Value":"Osteoarthritis","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Positive","Value":"Positive","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Rheumatoid Arthritis","Value":"Rheumatoid Arthritis","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""},{"Name":"Stable","Value":"Stable","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":""}]',
    GetMessagesPriority:'[{\"Name\":\"- Select -\",\"Value\":\"\",\"RefValue\":\"\",\"RefName\":\"\",\"IsActive\":\"\",\"ExName\":\"\",\"ExValue\":\"\"},{\"Name\":\"High\",\"Value\":\"1\",\"RefValue\":\"\",\"RefName\":\"\",\"IsActive\":\"\",\"ExName\":\"\",\"ExValue\":\"\"},{\"Name\":\"Medium\",\"Value\":\"2\",\"RefValue\":\"\",\"RefName\":\"\",\"IsActive\":\"\",\"ExName\":\"\",\"ExValue\":\"\"},{\"Name\":\"Low\",\"Value\":\"3\",\"RefValue\":\"\",\"RefName\":\"\",\"IsActive\":\"\",\"ExName\":\"\",\"ExValue\":\"\"}]',
    GetCarePlanStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Achieved", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Partially Achieved", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Not Achieved", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetCarePlanPatientPriority: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "High Priority", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Medium Priority", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Low Priority", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetCarePlanProviderPriority: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "High Priority", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Medium Priority", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
    '{ "Name": "Low Priority", "Value": "3", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetCarePlanConcernStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Active", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Completed", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetCarePlanRiskStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
 '{ "Name": "Active", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
 '{ "Name": "Completed", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',
    GetInterventionStatus: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
 '{ "Name": "Active", "Value": "1", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
 '{ "Name": "Completed", "Value": "2", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',
    GetRoleType: '[{ "Name": "One Time", "Value": "Onetime", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
   '{ "Name": "Recurring", "Value": "Recurring", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',

    GetDateRange: '[{ "Name": "- Select -", "Value": "", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
  '{ "Name": "2017-2025", "Value": "2017-2025", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
  '{ "Name": "2026-2035", "Value": "2026-2035", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
  '{ "Name": "2036-2055", "Value": "2036-2055", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
  '{ "Name": "2036-2055", "Value": "2036-2055", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" },' +
  '{ "Name": "2056-2065", "Value": "2056-2065", "RefValue": "", "RefName": "", "IsActive": "", "ExName": "", "ExValue": "" }]',
    GetMSPTypeDemographic: '[{"Name":"- Select -","Value":"","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary Working Aged Beneficiary or Spouse with Employer Group Health Plan","Value":"1","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary End-Stage Renal Disease Beneficiary in the Mandated Coordination Period with an Employer’s Group Health Plan","Value":"2","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary, No-fault Insurance inc-luding Auto is Primary","Value":"3","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary Worker’s Compensation","Value":"4","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary Public Health Service (PHS)or Other Federal Agency","Value":"5","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary Black Lung","Value":"6","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary Veteran’s Administration","Value":"7","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary Disabled Beneficiary Under Age 65 with Large Group Health Plan (LGHP)","Value":"8","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""},' +
        '{"Name":"Medicare Secondary, Other Liability Insurance is Primary","Value":"9","RefValue":"","RefName":"","IsActive":"","ExName":"","ExValue":"","Title":"","IsReferral":""}]',
};