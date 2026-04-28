using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Clinical.EMCodeGenerator;
using System.Linq;
namespace MDVision.DataAccess.DAL.Clinical
{

    public class DALEMCodeGenerator
    {
        #region Variable

        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALEMCodeGenerator"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALEMCodeGenerator()
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


        #region "Stored Procedure Names"

        private const string PROC_EMCODEGENERATOR_SELECT = "Clinical.sp_EMCodeGenerator_Select";

        #endregion

        #region Functions

        public EMCodeGeneratorDataHolder LoadEMCodeGeneratorData(long PatientId, long NotesId, long UserId)
        {
            EMCodeGeneratorDataHolder EMData = new EMCodeGeneratorDataHolder();
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                if (PatientId == 0)
                    dbManager.AddParameters(0, "@PatientId", null);
                else
                    dbManager.AddParameters(0, "@PatientId", PatientId);

                if (NotesId == 0)
                    dbManager.AddParameters(1, "@NotesId", null);
                else
                    dbManager.AddParameters(1, "@NotesId", NotesId);
                dbManager.AddParameters(2, "@UserId", UserId);
                dbManager.AddParameters(3, "@RecordCount", 5);

                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_EMCODEGENERATOR_SELECT);
              
                // History 
                while (reader.Read())
                {
                    EMData._EMCodeHistory.IsChiefComplaint = Convert.ToBoolean(reader["IsChiefComplaint"]);
                    EMData._EMCodeHistory.HPI_Count = Convert.ToInt64(reader["HPI_Count"]);
                    EMData._EMCodeHistory.Hx_Count = Convert.ToInt64(reader["Hx_Count"]);
                    EMData._EMCodeHistory.ROS_Count = Convert.ToInt64(reader["ROS_Count"]);
                    EMData._EMCodeHistory.HPI_AssociatedSignsAndSymptoms = Convert.ToString(reader["HPI_AssociatedSignsAndSymptoms"]);
                    EMData._EMCodeHistory.HPI_Context = Convert.ToInt64(reader["HPI_Context"]);
                    EMData._EMCodeHistory.HPI_Duration = Convert.ToInt64(reader["HPI_Duration"]);
                    EMData._EMCodeHistory.HPI_Location = Convert.ToString(reader["HPI_Location"]);
                    EMData._EMCodeHistory.HPI_Quality = Convert.ToInt64(reader["HPI_Quality"]);
                    EMData._EMCodeHistory.HPI_Severity = Convert.ToInt64(reader["HPI_Severity"]);
                    EMData._EMCodeHistory.HPI_Timing = Convert.ToInt64(reader["HPI_Timing"]);
                    EMData._EMCodeHistory.HPI_ModifyingFactors = Convert.ToInt64(reader["HPI_ModifyingFactors"]);
                    EMData._EMCodeHistory.MedicalHx = Convert.ToInt64(reader["MedicalHx"]);
                    EMData._EMCodeHistory.SocialHx = Convert.ToInt64(reader["SocialHx"]);
                    EMData._EMCodeHistory.FamilyHx = Convert.ToInt64(reader["FamilyHx"]);
                }
                reader.NextResult();
                // Physical Exam 
                EMCodeGeneratorExamModel examModel = null;
                string ElemDesc = "";
                while (reader.Read())
                {
                    examModel = new EMCodeGeneratorExamModel();
                    examModel.PatientPhysicalExamSystemId = Convert.ToInt64(reader["PatientPhysicalExamSystemId"]);
                    ElemDesc = Convert.ToString(reader["NoOfElements"]);
                    string[] splitted = ElemDesc.Split(new char [] {',',':',';','.','-' });
                    string[] Observations = splitted.Distinct().ToArray();
                    examModel.NoOfElements = Observations.Length;
                    // examModel.SystemName = Convert.ToString(reader["SystemName"]);
                    // examModel.SystemDescription = Convert.ToString(reader["SystemDescription"]);
                    examModel.TotalNoOfElementsInSystem = Convert.ToInt16(reader["TotalNoOfElementsInSystem"]);
                    EMData._EMCodeExam.Add(examModel);
                }
                reader.NextResult();
                //MDM
                while (reader.Read())
                {
                    EMData._EMCodeMDM.HasLabOrderOrResult = Convert.ToBoolean(reader["HasLabOrderOrResult"]);
                    EMData._EMCodeMDM.HasLabResultRemarksOrComments = Convert.ToBoolean(reader["HasLabResultRemarksOrComments"]);
                    EMData._EMCodeMDM.HasRadiologyOrderOrResult = Convert.ToBoolean(reader["HasRadiologyOrderOrResult"]);
                    EMData._EMCodeMDM.HasRadiologyResultRemarksOrComments = Convert.ToBoolean(reader["HasRadiologyResultRemarksOrComments"]);
                    EMData._EMCodeMDM.NoOfICD = Convert.ToInt16(reader["NoOfICD"]);
                }

                return EMData;
            }
            catch (Exception ex)
            {
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    EMData.ErrorMessage = str[1].ToString();
                else
                    EMData.ErrorMessage = ex.Message;
                return EMData;
            }

        }
        #endregion
        }
}
